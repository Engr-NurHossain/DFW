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
	public partial class RestMenuDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTRESTMENU = "InsertRestMenu";
		private const string UPDATERESTMENU = "UpdateRestMenu";
		private const string DELETERESTMENU = "DeleteRestMenu";
		private const string GETRESTMENUBYID = "GetRestMenuById";
		private const string GETALLRESTMENU = "GetAllRestMenu";
		private const string GETPAGEDRESTMENU = "GetPagedRestMenu";
		private const string GETRESTMENUMAXIMUMID = "GetRestMenuMaximumId";
		private const string GETRESTMENUROWCOUNT = "GetRestMenuRowCount";	
		private const string GETRESTMENUBYQUERY = "GetRestMenuByQuery";
		#endregion
		
		#region Constructors
		public RestMenuDataAccess(ClientContext context) : base(context) { }
		public RestMenuDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="restMenuObject"></param>
		private void AddCommonParams(SqlCommand cmd, RestMenuBase restMenuObject)
		{	
			AddParameter(cmd, pGuid(RestMenuBase.Property_CompanyId, restMenuObject.CompanyId));
			AddParameter(cmd, pGuid(RestMenuBase.Property_MenuId, restMenuObject.MenuId));
			AddParameter(cmd, pNVarChar(RestMenuBase.Property_MenuName, 50, restMenuObject.MenuName));
			AddParameter(cmd, pBool(RestMenuBase.Property_Status, restMenuObject.Status));
			AddParameter(cmd, pNVarChar(RestMenuBase.Property_TimeAvailable, 250, restMenuObject.TimeAvailable));
			AddParameter(cmd, pNVarChar(RestMenuBase.Property_DaysAvailable, 250, restMenuObject.DaysAvailable));
			AddParameter(cmd, pNVarChar(RestMenuBase.Property_Description, restMenuObject.Description));
			AddParameter(cmd, pNVarChar(RestMenuBase.Property_Photo, restMenuObject.Photo));
			AddParameter(cmd, pDateTime(RestMenuBase.Property_CreatedDate, restMenuObject.CreatedDate));
			AddParameter(cmd, pGuid(RestMenuBase.Property_CreatedBy, restMenuObject.CreatedBy));
			AddParameter(cmd, pGuid(RestMenuBase.Property_LastUpdatedBy, restMenuObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(RestMenuBase.Property_LastUpdatedDate, restMenuObject.LastUpdatedDate));
			AddParameter(cmd, pNVarChar(RestMenuBase.Property_DaysAvailableOption, 50, restMenuObject.DaysAvailableOption));
			AddParameter(cmd, pNVarChar(RestMenuBase.Property_TimeAvailableOption, 50, restMenuObject.TimeAvailableOption));
			AddParameter(cmd, pNVarChar(RestMenuBase.Property_UrlSlug, restMenuObject.UrlSlug));
			AddParameter(cmd, pNVarChar(RestMenuBase.Property_WebsiteURL, restMenuObject.WebsiteURL));
			AddParameter(cmd, pNVarChar(RestMenuBase.Property_MetaTitle, restMenuObject.MetaTitle));
			AddParameter(cmd, pNVarChar(RestMenuBase.Property_MetaDescription, restMenuObject.MetaDescription));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts RestMenu
        /// </summary>
        /// <param name="restMenuObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(RestMenuBase restMenuObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTRESTMENU);
	
				AddParameter(cmd, pInt32Out(RestMenuBase.Property_Id));
				AddCommonParams(cmd, restMenuObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					restMenuObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					restMenuObject.Id = (Int32)GetOutParameter(cmd, RestMenuBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(restMenuObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates RestMenu
        /// </summary>
        /// <param name="restMenuObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(RestMenuBase restMenuObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATERESTMENU);
				
				AddParameter(cmd, pInt32(RestMenuBase.Property_Id, restMenuObject.Id));
				AddCommonParams(cmd, restMenuObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					restMenuObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(restMenuObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes RestMenu
        /// </summary>
        /// <param name="Id">Id of the RestMenu object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETERESTMENU);	
				
				AddParameter(cmd, pInt32(RestMenuBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(RestMenu), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves RestMenu object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the RestMenu object to retrieve</param>
        /// <returns>RestMenu object, null if not found</returns>
		public RestMenu Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTMENUBYID))
			{
				AddParameter( cmd, pInt32(RestMenuBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all RestMenu objects 
        /// </summary>
        /// <returns>A list of RestMenu objects</returns>
		public RestMenuList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLRESTMENU))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all RestMenu objects by PageRequest
        /// </summary>
        /// <returns>A list of RestMenu objects</returns>
		public RestMenuList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDRESTMENU))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				RestMenuList _RestMenuList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _RestMenuList;
			}
		}
		
		/// <summary>
        /// Retrieves all RestMenu objects by query String
        /// </summary>
        /// <returns>A list of RestMenu objects</returns>
		public RestMenuList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTMENUBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get RestMenu Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of RestMenu
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTMENUMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get RestMenu Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of RestMenu
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _RestMenuRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTMENUROWCOUNT))
			{
				SqlDataReader reader;
				_RestMenuRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _RestMenuRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills RestMenu object
        /// </summary>
        /// <param name="restMenuObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(RestMenuBase restMenuObject, SqlDataReader reader, int start)
		{
			
				restMenuObject.Id = reader.GetInt32( start + 0 );			
				restMenuObject.CompanyId = reader.GetGuid( start + 1 );			
				restMenuObject.MenuId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) restMenuObject.MenuName = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) restMenuObject.Status = reader.GetBoolean( start + 4 );			
				if(!reader.IsDBNull(5)) restMenuObject.TimeAvailable = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) restMenuObject.DaysAvailable = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) restMenuObject.Description = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) restMenuObject.Photo = reader.GetString( start + 8 );			
				restMenuObject.CreatedDate = reader.GetDateTime( start + 9 );			
				restMenuObject.CreatedBy = reader.GetGuid( start + 10 );			
				restMenuObject.LastUpdatedBy = reader.GetGuid( start + 11 );			
				restMenuObject.LastUpdatedDate = reader.GetDateTime( start + 12 );			
				if(!reader.IsDBNull(13)) restMenuObject.DaysAvailableOption = reader.GetString( start + 13 );			
				if(!reader.IsDBNull(14)) restMenuObject.TimeAvailableOption = reader.GetString( start + 14 );			
				if(!reader.IsDBNull(15)) restMenuObject.UrlSlug = reader.GetString( start + 15 );			
				if(!reader.IsDBNull(16)) restMenuObject.WebsiteURL = reader.GetString( start + 16 );			
				if(!reader.IsDBNull(17)) restMenuObject.MetaTitle = reader.GetString( start + 17 );			
				if(!reader.IsDBNull(18)) restMenuObject.MetaDescription = reader.GetString( start + 18 );			
			FillBaseObject(restMenuObject, reader, (start + 19));

			
			restMenuObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills RestMenu object
        /// </summary>
        /// <param name="restMenuObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(RestMenuBase restMenuObject, SqlDataReader reader)
		{
			FillObject(restMenuObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves RestMenu object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>RestMenu object</returns>
		private RestMenu GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					RestMenu restMenuObject= new RestMenu();
					FillObject(restMenuObject, reader);
					return restMenuObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of RestMenu objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of RestMenu objects</returns>
		private RestMenuList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//RestMenu list
			RestMenuList list = new RestMenuList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					RestMenu restMenuObject = new RestMenu();
					FillObject(restMenuObject, reader);

					list.Add(restMenuObject);
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
