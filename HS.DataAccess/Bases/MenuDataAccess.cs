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
	public partial class MenuDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTMENU = "InsertMenu";
		private const string UPDATEMENU = "UpdateMenu";
		private const string DELETEMENU = "DeleteMenu";
		private const string GETMENUBYID = "GetMenuById";
		private const string GETALLMENU = "GetAllMenu";
		private const string GETPAGEDMENU = "GetPagedMenu";
		private const string GETMENUMAXIMUMID = "GetMenuMaximumId";
		private const string GETMENUROWCOUNT = "GetMenuRowCount";	
		private const string GETMENUBYQUERY = "GetMenuByQuery";
		#endregion
		
		#region Constructors
		public MenuDataAccess(ClientContext context) : base(context) { }
		public MenuDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="menuObject"></param>
		private void AddCommonParams(SqlCommand cmd, MenuBase menuObject)
		{	
			AddParameter(cmd, pGuid(MenuBase.Property_CompanyId, menuObject.CompanyId));
			AddParameter(cmd, pGuid(MenuBase.Property_MenuId, menuObject.MenuId));
			AddParameter(cmd, pNVarChar(MenuBase.Property_MenuName, 50, menuObject.MenuName));
			AddParameter(cmd, pBool(MenuBase.Property_Status, menuObject.Status));
			AddParameter(cmd, pNVarChar(MenuBase.Property_TimeAvailable, 250, menuObject.TimeAvailable));
			AddParameter(cmd, pNVarChar(MenuBase.Property_DaysAvailable, 250, menuObject.DaysAvailable));
			AddParameter(cmd, pNVarChar(MenuBase.Property_Description, menuObject.Description));
			AddParameter(cmd, pNVarChar(MenuBase.Property_Photo, menuObject.Photo));
			AddParameter(cmd, pDateTime(MenuBase.Property_CreatedDate, menuObject.CreatedDate));
			AddParameter(cmd, pGuid(MenuBase.Property_CreatedBy, menuObject.CreatedBy));
			AddParameter(cmd, pGuid(MenuBase.Property_LastUpdatedBy, menuObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(MenuBase.Property_LastUpdatedDate, menuObject.LastUpdatedDate));
			AddParameter(cmd, pNVarChar(MenuBase.Property_DaysAvailableOption, 50, menuObject.DaysAvailableOption));
			AddParameter(cmd, pNVarChar(MenuBase.Property_TimeAvailableOption, 50, menuObject.TimeAvailableOption));
			AddParameter(cmd, pNVarChar(MenuBase.Property_UrlSlug, menuObject.UrlSlug));
			AddParameter(cmd, pNVarChar(MenuBase.Property_WebsiteURL, menuObject.WebsiteURL));
			AddParameter(cmd, pNVarChar(MenuBase.Property_MetaTitle, menuObject.MetaTitle));
			AddParameter(cmd, pNVarChar(MenuBase.Property_MetaDescription, menuObject.MetaDescription));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts Menu
        /// </summary>
        /// <param name="menuObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(MenuBase menuObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTMENU);
	
				AddParameter(cmd, pInt32Out(MenuBase.Property_Id));
				AddCommonParams(cmd, menuObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					menuObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					menuObject.Id = (Int32)GetOutParameter(cmd, MenuBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(menuObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates Menu
        /// </summary>
        /// <param name="menuObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(MenuBase menuObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEMENU);
				
				AddParameter(cmd, pInt32(MenuBase.Property_Id, menuObject.Id));
				AddCommonParams(cmd, menuObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					menuObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(menuObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes Menu
        /// </summary>
        /// <param name="Id">Id of the Menu object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEMENU);	
				
				AddParameter(cmd, pInt32(MenuBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(Menu), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves Menu object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the Menu object to retrieve</param>
        /// <returns>Menu object, null if not found</returns>
		public Menu Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETMENUBYID))
			{
				AddParameter( cmd, pInt32(MenuBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all Menu objects 
        /// </summary>
        /// <returns>A list of Menu objects</returns>
		public MenuList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLMENU))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all Menu objects by PageRequest
        /// </summary>
        /// <returns>A list of Menu objects</returns>
		public MenuList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDMENU))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				MenuList _MenuList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _MenuList;
			}
		}
		
		/// <summary>
        /// Retrieves all Menu objects by query String
        /// </summary>
        /// <returns>A list of Menu objects</returns>
		public MenuList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETMENUBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get Menu Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of Menu
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETMENUMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get Menu Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of Menu
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _MenuRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETMENUROWCOUNT))
			{
				SqlDataReader reader;
				_MenuRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MenuRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills Menu object
        /// </summary>
        /// <param name="menuObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(MenuBase menuObject, SqlDataReader reader, int start)
		{
			
				menuObject.Id = reader.GetInt32( start + 0 );			
				menuObject.CompanyId = reader.GetGuid( start + 1 );			
				menuObject.MenuId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) menuObject.MenuName = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) menuObject.Status = reader.GetBoolean( start + 4 );			
				if(!reader.IsDBNull(5)) menuObject.TimeAvailable = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) menuObject.DaysAvailable = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) menuObject.Description = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) menuObject.Photo = reader.GetString( start + 8 );			
				menuObject.CreatedDate = reader.GetDateTime( start + 9 );			
				menuObject.CreatedBy = reader.GetGuid( start + 10 );			
				menuObject.LastUpdatedBy = reader.GetGuid( start + 11 );			
				menuObject.LastUpdatedDate = reader.GetDateTime( start + 12 );			
				if(!reader.IsDBNull(13)) menuObject.DaysAvailableOption = reader.GetString( start + 13 );			
				if(!reader.IsDBNull(14)) menuObject.TimeAvailableOption = reader.GetString( start + 14 );			
				if(!reader.IsDBNull(15)) menuObject.UrlSlug = reader.GetString( start + 15 );			
				if(!reader.IsDBNull(16)) menuObject.WebsiteURL = reader.GetString( start + 16 );			
				if(!reader.IsDBNull(17)) menuObject.MetaTitle = reader.GetString( start + 17 );			
				if(!reader.IsDBNull(18)) menuObject.MetaDescription = reader.GetString( start + 18 );			
			FillBaseObject(menuObject, reader, (start + 19));

			
			menuObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills Menu object
        /// </summary>
        /// <param name="menuObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(MenuBase menuObject, SqlDataReader reader)
		{
			FillObject(menuObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves Menu object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>Menu object</returns>
		private Menu GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					Menu menuObject= new Menu();
					FillObject(menuObject, reader);
					return menuObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of Menu objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of Menu objects</returns>
		private MenuList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//Menu list
			MenuList list = new MenuList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					Menu menuObject = new Menu();
					FillObject(menuObject, reader);

					list.Add(menuObject);
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
