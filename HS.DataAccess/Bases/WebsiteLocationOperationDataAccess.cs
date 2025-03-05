using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.DataAccess;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.DataAccess
{
	public partial class WebsiteLocationOperationDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTWEBSITELOCATIONOPERATION = "InsertWebsiteLocationOperation";
		private const string UPDATEWEBSITELOCATIONOPERATION = "UpdateWebsiteLocationOperation";
		private const string DELETEWEBSITELOCATIONOPERATION = "DeleteWebsiteLocationOperation";
		private const string GETWEBSITELOCATIONOPERATIONBYID = "GetWebsiteLocationOperationById";
		private const string GETALLWEBSITELOCATIONOPERATION = "GetAllWebsiteLocationOperation";
		private const string GETPAGEDWEBSITELOCATIONOPERATION = "GetPagedWebsiteLocationOperation";
		private const string GETWEBSITELOCATIONOPERATIONMAXIMUMID = "GetWebsiteLocationOperationMaximumId";
		private const string GETWEBSITELOCATIONOPERATIONROWCOUNT = "GetWebsiteLocationOperationRowCount";	
		private const string GETWEBSITELOCATIONOPERATIONBYQUERY = "GetWebsiteLocationOperationByQuery";
		#endregion
		
		#region Constructors
		public WebsiteLocationOperationDataAccess(ClientContext context) : base(context) { }
		public WebsiteLocationOperationDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="websiteLocationOperationObject"></param>
		private void AddCommonParams(SqlCommand cmd, WebsiteLocationOperationBase websiteLocationOperationObject)
		{	
			AddParameter(cmd, pInt32(WebsiteLocationOperationBase.Property_SiteLocationId, websiteLocationOperationObject.SiteLocationId));
			AddParameter(cmd, pNVarChar(WebsiteLocationOperationBase.Property_HoursofOperation, 50, websiteLocationOperationObject.HoursofOperation));
			AddParameter(cmd, pNVarChar(WebsiteLocationOperationBase.Property_OperationStartTime, 50, websiteLocationOperationObject.OperationStartTime));
			AddParameter(cmd, pNVarChar(WebsiteLocationOperationBase.Property_OperationEndTime, 50, websiteLocationOperationObject.OperationEndTime));
			AddParameter(cmd, pGuid(WebsiteLocationOperationBase.Property_CreatedBy, websiteLocationOperationObject.CreatedBy));
			AddParameter(cmd, pDateTime(WebsiteLocationOperationBase.Property_CreatedDate, websiteLocationOperationObject.CreatedDate));
			AddParameter(cmd, pGuid(WebsiteLocationOperationBase.Property_CompanyId, websiteLocationOperationObject.CompanyId));
			AddParameter(cmd, pNVarChar(WebsiteLocationOperationBase.Property_StoreOperationStartTime, 250, websiteLocationOperationObject.StoreOperationStartTime));
			AddParameter(cmd, pNVarChar(WebsiteLocationOperationBase.Property_StoreOperationEndTime, 250, websiteLocationOperationObject.StoreOperationEndTime));
			AddParameter(cmd, pBool(WebsiteLocationOperationBase.Property_IsAdditional, websiteLocationOperationObject.IsAdditional));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts WebsiteLocationOperation
        /// </summary>
        /// <param name="websiteLocationOperationObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(WebsiteLocationOperationBase websiteLocationOperationObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTWEBSITELOCATIONOPERATION);
	
				AddParameter(cmd, pInt32Out(WebsiteLocationOperationBase.Property_Id));
				AddCommonParams(cmd, websiteLocationOperationObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					websiteLocationOperationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					websiteLocationOperationObject.Id = (Int32)GetOutParameter(cmd, WebsiteLocationOperationBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(websiteLocationOperationObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates WebsiteLocationOperation
        /// </summary>
        /// <param name="websiteLocationOperationObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(WebsiteLocationOperationBase websiteLocationOperationObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEWEBSITELOCATIONOPERATION);
				
				AddParameter(cmd, pInt32(WebsiteLocationOperationBase.Property_Id, websiteLocationOperationObject.Id));
				AddCommonParams(cmd, websiteLocationOperationObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					websiteLocationOperationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(websiteLocationOperationObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes WebsiteLocationOperation
        /// </summary>
        /// <param name="Id">Id of the WebsiteLocationOperation object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEWEBSITELOCATIONOPERATION);	
				
				AddParameter(cmd, pInt32(WebsiteLocationOperationBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(WebsiteLocationOperation), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves WebsiteLocationOperation object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the WebsiteLocationOperation object to retrieve</param>
        /// <returns>WebsiteLocationOperation object, null if not found</returns>
		public WebsiteLocationOperation Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETWEBSITELOCATIONOPERATIONBYID))
			{
				AddParameter( cmd, pInt32(WebsiteLocationOperationBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all WebsiteLocationOperation objects 
        /// </summary>
        /// <returns>A list of WebsiteLocationOperation objects</returns>
		public WebsiteLocationOperationList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLWEBSITELOCATIONOPERATION))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all WebsiteLocationOperation objects by PageRequest
        /// </summary>
        /// <returns>A list of WebsiteLocationOperation objects</returns>
		public WebsiteLocationOperationList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDWEBSITELOCATIONOPERATION))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				WebsiteLocationOperationList _WebsiteLocationOperationList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _WebsiteLocationOperationList;
			}
		}
		
		/// <summary>
        /// Retrieves all WebsiteLocationOperation objects by query String
        /// </summary>
        /// <returns>A list of WebsiteLocationOperation objects</returns>
		public WebsiteLocationOperationList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETWEBSITELOCATIONOPERATIONBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get WebsiteLocationOperation Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of WebsiteLocationOperation
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETWEBSITELOCATIONOPERATIONMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get WebsiteLocationOperation Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of WebsiteLocationOperation
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _WebsiteLocationOperationRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETWEBSITELOCATIONOPERATIONROWCOUNT))
			{
				SqlDataReader reader;
				_WebsiteLocationOperationRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _WebsiteLocationOperationRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills WebsiteLocationOperation object
        /// </summary>
        /// <param name="websiteLocationOperationObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(WebsiteLocationOperationBase websiteLocationOperationObject, SqlDataReader reader, int start)
		{
			
				websiteLocationOperationObject.Id = reader.GetInt32( start + 0 );			
				websiteLocationOperationObject.SiteLocationId = reader.GetInt32( start + 1 );			
				if(!reader.IsDBNull(2)) websiteLocationOperationObject.HoursofOperation = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) websiteLocationOperationObject.OperationStartTime = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) websiteLocationOperationObject.OperationEndTime = reader.GetString( start + 4 );			
				websiteLocationOperationObject.CreatedBy = reader.GetGuid( start + 5 );			
				websiteLocationOperationObject.CreatedDate = reader.GetDateTime( start + 6 );			
				websiteLocationOperationObject.CompanyId = reader.GetGuid( start + 7 );			
				if(!reader.IsDBNull(8)) websiteLocationOperationObject.StoreOperationStartTime = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) websiteLocationOperationObject.StoreOperationEndTime = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) websiteLocationOperationObject.IsAdditional = reader.GetBoolean( start + 10 );			
			FillBaseObject(websiteLocationOperationObject, reader, (start + 11));

			
			websiteLocationOperationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills WebsiteLocationOperation object
        /// </summary>
        /// <param name="websiteLocationOperationObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(WebsiteLocationOperationBase websiteLocationOperationObject, SqlDataReader reader)
		{
			FillObject(websiteLocationOperationObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves WebsiteLocationOperation object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>WebsiteLocationOperation object</returns>
		private WebsiteLocationOperation GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					WebsiteLocationOperation websiteLocationOperationObject= new WebsiteLocationOperation();
					FillObject(websiteLocationOperationObject, reader);
					return websiteLocationOperationObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of WebsiteLocationOperation objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of WebsiteLocationOperation objects</returns>
		private WebsiteLocationOperationList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//WebsiteLocationOperation list
			WebsiteLocationOperationList list = new WebsiteLocationOperationList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					WebsiteLocationOperation websiteLocationOperationObject = new WebsiteLocationOperation();
					FillObject(websiteLocationOperationObject, reader);

					list.Add(websiteLocationOperationObject);
				}
				
				// Close the reader in order to receive output parameters
				// Output parameters are not available until reader is closed.
				reader.Close();
			}

			return list;
		}
		
		#endregion
	}	
}
