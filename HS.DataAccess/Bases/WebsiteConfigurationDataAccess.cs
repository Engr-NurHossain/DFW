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
	public partial class WebsiteConfigurationDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTWEBSITECONFIGURATION = "InsertWebsiteConfiguration";
		private const string UPDATEWEBSITECONFIGURATION = "UpdateWebsiteConfiguration";
		private const string DELETEWEBSITECONFIGURATION = "DeleteWebsiteConfiguration";
		private const string GETWEBSITECONFIGURATIONBYID = "GetWebsiteConfigurationById";
		private const string GETALLWEBSITECONFIGURATION = "GetAllWebsiteConfiguration";
		private const string GETPAGEDWEBSITECONFIGURATION = "GetPagedWebsiteConfiguration";
		private const string GETWEBSITECONFIGURATIONMAXIMUMID = "GetWebsiteConfigurationMaximumId";
		private const string GETWEBSITECONFIGURATIONROWCOUNT = "GetWebsiteConfigurationRowCount";	
		private const string GETWEBSITECONFIGURATIONBYQUERY = "GetWebsiteConfigurationByQuery";
		#endregion
		
		#region Constructors
		public WebsiteConfigurationDataAccess(ClientContext context) : base(context) { }
		public WebsiteConfigurationDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="websiteConfigurationObject"></param>
		private void AddCommonParams(SqlCommand cmd, WebsiteConfigurationBase websiteConfigurationObject)
		{	
			AddParameter(cmd, pGuid(WebsiteConfigurationBase.Property_CompanyId, websiteConfigurationObject.CompanyId));
			AddParameter(cmd, pNVarChar(WebsiteConfigurationBase.Property_SiteName, websiteConfigurationObject.SiteName));
			AddParameter(cmd, pNVarChar(WebsiteConfigurationBase.Property_DomainName, websiteConfigurationObject.DomainName));
			AddParameter(cmd, pNVarChar(WebsiteConfigurationBase.Property_Phone, 150, websiteConfigurationObject.Phone));
			AddParameter(cmd, pBool(WebsiteConfigurationBase.Property_IsEmail, websiteConfigurationObject.IsEmail));
			AddParameter(cmd, pNVarChar(WebsiteConfigurationBase.Property_ThemeLoc, websiteConfigurationObject.ThemeLoc));
			AddParameter(cmd, pGuid(WebsiteConfigurationBase.Property_CreatedBy, websiteConfigurationObject.CreatedBy));
			AddParameter(cmd, pDateTime(WebsiteConfigurationBase.Property_CreatedDate, websiteConfigurationObject.CreatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts WebsiteConfiguration
        /// </summary>
        /// <param name="websiteConfigurationObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(WebsiteConfigurationBase websiteConfigurationObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTWEBSITECONFIGURATION);
	
				AddParameter(cmd, pInt32Out(WebsiteConfigurationBase.Property_Id));
				AddCommonParams(cmd, websiteConfigurationObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					websiteConfigurationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					websiteConfigurationObject.Id = (Int32)GetOutParameter(cmd, WebsiteConfigurationBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(websiteConfigurationObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates WebsiteConfiguration
        /// </summary>
        /// <param name="websiteConfigurationObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(WebsiteConfigurationBase websiteConfigurationObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEWEBSITECONFIGURATION);
				
				AddParameter(cmd, pInt32(WebsiteConfigurationBase.Property_Id, websiteConfigurationObject.Id));
				AddCommonParams(cmd, websiteConfigurationObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					websiteConfigurationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(websiteConfigurationObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes WebsiteConfiguration
        /// </summary>
        /// <param name="Id">Id of the WebsiteConfiguration object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEWEBSITECONFIGURATION);	
				
				AddParameter(cmd, pInt32(WebsiteConfigurationBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(WebsiteConfiguration), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves WebsiteConfiguration object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the WebsiteConfiguration object to retrieve</param>
        /// <returns>WebsiteConfiguration object, null if not found</returns>
		public WebsiteConfiguration Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETWEBSITECONFIGURATIONBYID))
			{
				AddParameter( cmd, pInt32(WebsiteConfigurationBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all WebsiteConfiguration objects 
        /// </summary>
        /// <returns>A list of WebsiteConfiguration objects</returns>
		public WebsiteConfigurationList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLWEBSITECONFIGURATION))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all WebsiteConfiguration objects by PageRequest
        /// </summary>
        /// <returns>A list of WebsiteConfiguration objects</returns>
		public WebsiteConfigurationList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDWEBSITECONFIGURATION))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				WebsiteConfigurationList _WebsiteConfigurationList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _WebsiteConfigurationList;
			}
		}
		
		/// <summary>
        /// Retrieves all WebsiteConfiguration objects by query String
        /// </summary>
        /// <returns>A list of WebsiteConfiguration objects</returns>
		public WebsiteConfigurationList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETWEBSITECONFIGURATIONBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get WebsiteConfiguration Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of WebsiteConfiguration
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETWEBSITECONFIGURATIONMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get WebsiteConfiguration Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of WebsiteConfiguration
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _WebsiteConfigurationRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETWEBSITECONFIGURATIONROWCOUNT))
			{
				SqlDataReader reader;
				_WebsiteConfigurationRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _WebsiteConfigurationRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills WebsiteConfiguration object
        /// </summary>
        /// <param name="websiteConfigurationObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(WebsiteConfigurationBase websiteConfigurationObject, SqlDataReader reader, int start)
		{
			
				websiteConfigurationObject.Id = reader.GetInt32( start + 0 );			
				websiteConfigurationObject.CompanyId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) websiteConfigurationObject.SiteName = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) websiteConfigurationObject.DomainName = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) websiteConfigurationObject.Phone = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) websiteConfigurationObject.IsEmail = reader.GetBoolean( start + 5 );			
				if(!reader.IsDBNull(6)) websiteConfigurationObject.ThemeLoc = reader.GetString( start + 6 );			
				websiteConfigurationObject.CreatedBy = reader.GetGuid( start + 7 );			
				websiteConfigurationObject.CreatedDate = reader.GetDateTime( start + 8 );			
			FillBaseObject(websiteConfigurationObject, reader, (start + 9));

			
			websiteConfigurationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills WebsiteConfiguration object
        /// </summary>
        /// <param name="websiteConfigurationObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(WebsiteConfigurationBase websiteConfigurationObject, SqlDataReader reader)
		{
			FillObject(websiteConfigurationObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves WebsiteConfiguration object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>WebsiteConfiguration object</returns>
		private WebsiteConfiguration GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					WebsiteConfiguration websiteConfigurationObject= new WebsiteConfiguration();
					FillObject(websiteConfigurationObject, reader);
					return websiteConfigurationObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of WebsiteConfiguration objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of WebsiteConfiguration objects</returns>
		private WebsiteConfigurationList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//WebsiteConfiguration list
			WebsiteConfigurationList list = new WebsiteConfigurationList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					WebsiteConfiguration websiteConfigurationObject = new WebsiteConfiguration();
					FillObject(websiteConfigurationObject, reader);

					list.Add(websiteConfigurationObject);
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
