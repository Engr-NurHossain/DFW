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
	public partial class RestMenuItemAdditionalContentDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTRESTMENUITEMADDITIONALCONTENT = "InsertRestMenuItemAdditionalContent";
		private const string UPDATERESTMENUITEMADDITIONALCONTENT = "UpdateRestMenuItemAdditionalContent";
		private const string DELETERESTMENUITEMADDITIONALCONTENT = "DeleteRestMenuItemAdditionalContent";
		private const string GETRESTMENUITEMADDITIONALCONTENTBYID = "GetRestMenuItemAdditionalContentById";
		private const string GETALLRESTMENUITEMADDITIONALCONTENT = "GetAllRestMenuItemAdditionalContent";
		private const string GETPAGEDRESTMENUITEMADDITIONALCONTENT = "GetPagedRestMenuItemAdditionalContent";
		private const string GETRESTMENUITEMADDITIONALCONTENTMAXIMUMID = "GetRestMenuItemAdditionalContentMaximumId";
		private const string GETRESTMENUITEMADDITIONALCONTENTROWCOUNT = "GetRestMenuItemAdditionalContentRowCount";	
		private const string GETRESTMENUITEMADDITIONALCONTENTBYQUERY = "GetRestMenuItemAdditionalContentByQuery";
		#endregion
		
		#region Constructors
		public RestMenuItemAdditionalContentDataAccess(ClientContext context) : base(context) { }
		public RestMenuItemAdditionalContentDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="restMenuItemAdditionalContentObject"></param>
		private void AddCommonParams(SqlCommand cmd, RestMenuItemAdditionalContentBase restMenuItemAdditionalContentObject)
		{	
			AddParameter(cmd, pGuid(RestMenuItemAdditionalContentBase.Property_ItemId, restMenuItemAdditionalContentObject.ItemId));
			AddParameter(cmd, pNVarChar(RestMenuItemAdditionalContentBase.Property_Name, 250, restMenuItemAdditionalContentObject.Name));
			AddParameter(cmd, pNVarChar(RestMenuItemAdditionalContentBase.Property_ImageLoc, restMenuItemAdditionalContentObject.ImageLoc));
			AddParameter(cmd, pGuid(RestMenuItemAdditionalContentBase.Property_CreatedBy, restMenuItemAdditionalContentObject.CreatedBy));
			AddParameter(cmd, pDateTime(RestMenuItemAdditionalContentBase.Property_CreatedDate, restMenuItemAdditionalContentObject.CreatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts RestMenuItemAdditionalContent
        /// </summary>
        /// <param name="restMenuItemAdditionalContentObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(RestMenuItemAdditionalContentBase restMenuItemAdditionalContentObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTRESTMENUITEMADDITIONALCONTENT);
	
				AddParameter(cmd, pInt32Out(RestMenuItemAdditionalContentBase.Property_Id));
				AddCommonParams(cmd, restMenuItemAdditionalContentObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					restMenuItemAdditionalContentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					restMenuItemAdditionalContentObject.Id = (Int32)GetOutParameter(cmd, RestMenuItemAdditionalContentBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(restMenuItemAdditionalContentObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates RestMenuItemAdditionalContent
        /// </summary>
        /// <param name="restMenuItemAdditionalContentObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(RestMenuItemAdditionalContentBase restMenuItemAdditionalContentObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATERESTMENUITEMADDITIONALCONTENT);
				
				AddParameter(cmd, pInt32(RestMenuItemAdditionalContentBase.Property_Id, restMenuItemAdditionalContentObject.Id));
				AddCommonParams(cmd, restMenuItemAdditionalContentObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					restMenuItemAdditionalContentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(restMenuItemAdditionalContentObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes RestMenuItemAdditionalContent
        /// </summary>
        /// <param name="Id">Id of the RestMenuItemAdditionalContent object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETERESTMENUITEMADDITIONALCONTENT);	
				
				AddParameter(cmd, pInt32(RestMenuItemAdditionalContentBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(RestMenuItemAdditionalContent), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves RestMenuItemAdditionalContent object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the RestMenuItemAdditionalContent object to retrieve</param>
        /// <returns>RestMenuItemAdditionalContent object, null if not found</returns>
		public RestMenuItemAdditionalContent Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTMENUITEMADDITIONALCONTENTBYID))
			{
				AddParameter( cmd, pInt32(RestMenuItemAdditionalContentBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all RestMenuItemAdditionalContent objects 
        /// </summary>
        /// <returns>A list of RestMenuItemAdditionalContent objects</returns>
		public RestMenuItemAdditionalContentList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLRESTMENUITEMADDITIONALCONTENT))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all RestMenuItemAdditionalContent objects by PageRequest
        /// </summary>
        /// <returns>A list of RestMenuItemAdditionalContent objects</returns>
		public RestMenuItemAdditionalContentList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDRESTMENUITEMADDITIONALCONTENT))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				RestMenuItemAdditionalContentList _RestMenuItemAdditionalContentList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _RestMenuItemAdditionalContentList;
			}
		}
		
		/// <summary>
        /// Retrieves all RestMenuItemAdditionalContent objects by query String
        /// </summary>
        /// <returns>A list of RestMenuItemAdditionalContent objects</returns>
		public RestMenuItemAdditionalContentList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTMENUITEMADDITIONALCONTENTBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get RestMenuItemAdditionalContent Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of RestMenuItemAdditionalContent
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTMENUITEMADDITIONALCONTENTMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get RestMenuItemAdditionalContent Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of RestMenuItemAdditionalContent
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _RestMenuItemAdditionalContentRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTMENUITEMADDITIONALCONTENTROWCOUNT))
			{
				SqlDataReader reader;
				_RestMenuItemAdditionalContentRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _RestMenuItemAdditionalContentRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills RestMenuItemAdditionalContent object
        /// </summary>
        /// <param name="restMenuItemAdditionalContentObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(RestMenuItemAdditionalContentBase restMenuItemAdditionalContentObject, SqlDataReader reader, int start)
		{
			
				restMenuItemAdditionalContentObject.Id = reader.GetInt32( start + 0 );			
				restMenuItemAdditionalContentObject.ItemId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) restMenuItemAdditionalContentObject.Name = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) restMenuItemAdditionalContentObject.ImageLoc = reader.GetString( start + 3 );			
				restMenuItemAdditionalContentObject.CreatedBy = reader.GetGuid( start + 4 );			
				restMenuItemAdditionalContentObject.CreatedDate = reader.GetDateTime( start + 5 );			
			FillBaseObject(restMenuItemAdditionalContentObject, reader, (start + 6));

			
			restMenuItemAdditionalContentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills RestMenuItemAdditionalContent object
        /// </summary>
        /// <param name="restMenuItemAdditionalContentObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(RestMenuItemAdditionalContentBase restMenuItemAdditionalContentObject, SqlDataReader reader)
		{
			FillObject(restMenuItemAdditionalContentObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves RestMenuItemAdditionalContent object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>RestMenuItemAdditionalContent object</returns>
		private RestMenuItemAdditionalContent GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					RestMenuItemAdditionalContent restMenuItemAdditionalContentObject= new RestMenuItemAdditionalContent();
					FillObject(restMenuItemAdditionalContentObject, reader);
					return restMenuItemAdditionalContentObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of RestMenuItemAdditionalContent objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of RestMenuItemAdditionalContent objects</returns>
		private RestMenuItemAdditionalContentList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//RestMenuItemAdditionalContent list
			RestMenuItemAdditionalContentList list = new RestMenuItemAdditionalContentList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					RestMenuItemAdditionalContent restMenuItemAdditionalContentObject = new RestMenuItemAdditionalContent();
					FillObject(restMenuItemAdditionalContentObject, reader);

					list.Add(restMenuItemAdditionalContentObject);
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
