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
	public partial class WebsiteLocationMapInfoDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTWEBSITELOCATIONMAPINFO = "InsertWebsiteLocationMapInfo";
		private const string UPDATEWEBSITELOCATIONMAPINFO = "UpdateWebsiteLocationMapInfo";
		private const string DELETEWEBSITELOCATIONMAPINFO = "DeleteWebsiteLocationMapInfo";
		private const string GETWEBSITELOCATIONMAPINFOBYID = "GetWebsiteLocationMapInfoById";
		private const string GETALLWEBSITELOCATIONMAPINFO = "GetAllWebsiteLocationMapInfo";
		private const string GETPAGEDWEBSITELOCATIONMAPINFO = "GetPagedWebsiteLocationMapInfo";
		private const string GETWEBSITELOCATIONMAPINFOMAXIMUMID = "GetWebsiteLocationMapInfoMaximumId";
		private const string GETWEBSITELOCATIONMAPINFOROWCOUNT = "GetWebsiteLocationMapInfoRowCount";	
		private const string GETWEBSITELOCATIONMAPINFOBYQUERY = "GetWebsiteLocationMapInfoByQuery";
		#endregion
		
		#region Constructors
		public WebsiteLocationMapInfoDataAccess(ClientContext context) : base(context) { }
		public WebsiteLocationMapInfoDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="websiteLocationMapInfoObject"></param>
		private void AddCommonParams(SqlCommand cmd, WebsiteLocationMapInfoBase websiteLocationMapInfoObject)
		{	
			AddParameter(cmd, pGuid(WebsiteLocationMapInfoBase.Property_CompanyId, websiteLocationMapInfoObject.CompanyId));
			AddParameter(cmd, pNVarChar(WebsiteLocationMapInfoBase.Property_Latitude, 250, websiteLocationMapInfoObject.Latitude));
			AddParameter(cmd, pNVarChar(WebsiteLocationMapInfoBase.Property_Longitude, 250, websiteLocationMapInfoObject.Longitude));
			AddParameter(cmd, pGuid(WebsiteLocationMapInfoBase.Property_CreatedBy, websiteLocationMapInfoObject.CreatedBy));
			AddParameter(cmd, pDateTime(WebsiteLocationMapInfoBase.Property_CreatedDate, websiteLocationMapInfoObject.CreatedDate));
			AddParameter(cmd, pGuid(WebsiteLocationMapInfoBase.Property_LastUpdatedBy, websiteLocationMapInfoObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(WebsiteLocationMapInfoBase.Property_LastUpdatedDate, websiteLocationMapInfoObject.LastUpdatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts WebsiteLocationMapInfo
        /// </summary>
        /// <param name="websiteLocationMapInfoObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(WebsiteLocationMapInfoBase websiteLocationMapInfoObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTWEBSITELOCATIONMAPINFO);
	
				AddParameter(cmd, pInt32Out(WebsiteLocationMapInfoBase.Property_Id));
				AddCommonParams(cmd, websiteLocationMapInfoObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					websiteLocationMapInfoObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					websiteLocationMapInfoObject.Id = (Int32)GetOutParameter(cmd, WebsiteLocationMapInfoBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(websiteLocationMapInfoObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates WebsiteLocationMapInfo
        /// </summary>
        /// <param name="websiteLocationMapInfoObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(WebsiteLocationMapInfoBase websiteLocationMapInfoObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEWEBSITELOCATIONMAPINFO);
				
				AddParameter(cmd, pInt32(WebsiteLocationMapInfoBase.Property_Id, websiteLocationMapInfoObject.Id));
				AddCommonParams(cmd, websiteLocationMapInfoObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					websiteLocationMapInfoObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(websiteLocationMapInfoObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes WebsiteLocationMapInfo
        /// </summary>
        /// <param name="Id">Id of the WebsiteLocationMapInfo object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEWEBSITELOCATIONMAPINFO);	
				
				AddParameter(cmd, pInt32(WebsiteLocationMapInfoBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(WebsiteLocationMapInfo), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves WebsiteLocationMapInfo object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the WebsiteLocationMapInfo object to retrieve</param>
        /// <returns>WebsiteLocationMapInfo object, null if not found</returns>
		public WebsiteLocationMapInfo Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETWEBSITELOCATIONMAPINFOBYID))
			{
				AddParameter( cmd, pInt32(WebsiteLocationMapInfoBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all WebsiteLocationMapInfo objects 
        /// </summary>
        /// <returns>A list of WebsiteLocationMapInfo objects</returns>
		public WebsiteLocationMapInfoList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLWEBSITELOCATIONMAPINFO))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all WebsiteLocationMapInfo objects by PageRequest
        /// </summary>
        /// <returns>A list of WebsiteLocationMapInfo objects</returns>
		public WebsiteLocationMapInfoList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDWEBSITELOCATIONMAPINFO))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				WebsiteLocationMapInfoList _WebsiteLocationMapInfoList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _WebsiteLocationMapInfoList;
			}
		}
		
		/// <summary>
        /// Retrieves all WebsiteLocationMapInfo objects by query String
        /// </summary>
        /// <returns>A list of WebsiteLocationMapInfo objects</returns>
		public WebsiteLocationMapInfoList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETWEBSITELOCATIONMAPINFOBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get WebsiteLocationMapInfo Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of WebsiteLocationMapInfo
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETWEBSITELOCATIONMAPINFOMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get WebsiteLocationMapInfo Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of WebsiteLocationMapInfo
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _WebsiteLocationMapInfoRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETWEBSITELOCATIONMAPINFOROWCOUNT))
			{
				SqlDataReader reader;
				_WebsiteLocationMapInfoRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _WebsiteLocationMapInfoRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills WebsiteLocationMapInfo object
        /// </summary>
        /// <param name="websiteLocationMapInfoObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(WebsiteLocationMapInfoBase websiteLocationMapInfoObject, SqlDataReader reader, int start)
		{
			
				websiteLocationMapInfoObject.Id = reader.GetInt32( start + 0 );			
				websiteLocationMapInfoObject.CompanyId = reader.GetGuid( start + 1 );			
				websiteLocationMapInfoObject.Latitude = reader.GetString( start + 2 );			
				websiteLocationMapInfoObject.Longitude = reader.GetString( start + 3 );			
				websiteLocationMapInfoObject.CreatedBy = reader.GetGuid( start + 4 );			
				websiteLocationMapInfoObject.CreatedDate = reader.GetDateTime( start + 5 );			
				websiteLocationMapInfoObject.LastUpdatedBy = reader.GetGuid( start + 6 );			
				websiteLocationMapInfoObject.LastUpdatedDate = reader.GetDateTime( start + 7 );			
			FillBaseObject(websiteLocationMapInfoObject, reader, (start + 8));

			
			websiteLocationMapInfoObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills WebsiteLocationMapInfo object
        /// </summary>
        /// <param name="websiteLocationMapInfoObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(WebsiteLocationMapInfoBase websiteLocationMapInfoObject, SqlDataReader reader)
		{
			FillObject(websiteLocationMapInfoObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves WebsiteLocationMapInfo object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>WebsiteLocationMapInfo object</returns>
		private WebsiteLocationMapInfo GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					WebsiteLocationMapInfo websiteLocationMapInfoObject= new WebsiteLocationMapInfo();
					FillObject(websiteLocationMapInfoObject, reader);
					return websiteLocationMapInfoObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of WebsiteLocationMapInfo objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of WebsiteLocationMapInfo objects</returns>
		private WebsiteLocationMapInfoList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//WebsiteLocationMapInfo list
			WebsiteLocationMapInfoList list = new WebsiteLocationMapInfoList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					WebsiteLocationMapInfo websiteLocationMapInfoObject = new WebsiteLocationMapInfo();
					FillObject(websiteLocationMapInfoObject, reader);

					list.Add(websiteLocationMapInfoObject);
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
