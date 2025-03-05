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
	public partial class MenuItemCategoryMapDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTMENUITEMCATEGORYMAP = "InsertMenuItemCategoryMap";
		private const string UPDATEMENUITEMCATEGORYMAP = "UpdateMenuItemCategoryMap";
		private const string DELETEMENUITEMCATEGORYMAP = "DeleteMenuItemCategoryMap";
		private const string GETMENUITEMCATEGORYMAPBYID = "GetMenuItemCategoryMapById";
		private const string GETALLMENUITEMCATEGORYMAP = "GetAllMenuItemCategoryMap";
		private const string GETPAGEDMENUITEMCATEGORYMAP = "GetPagedMenuItemCategoryMap";
		private const string GETMENUITEMCATEGORYMAPMAXIMUMID = "GetMenuItemCategoryMapMaximumId";
		private const string GETMENUITEMCATEGORYMAPROWCOUNT = "GetMenuItemCategoryMapRowCount";	
		private const string GETMENUITEMCATEGORYMAPBYQUERY = "GetMenuItemCategoryMapByQuery";
		#endregion
		
		#region Constructors
		public MenuItemCategoryMapDataAccess(ClientContext context) : base(context) { }
		public MenuItemCategoryMapDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="menuItemCategoryMapObject"></param>
		private void AddCommonParams(SqlCommand cmd, MenuItemCategoryMapBase menuItemCategoryMapObject)
		{	
			AddParameter(cmd, pGuid(MenuItemCategoryMapBase.Property_CompanyId, menuItemCategoryMapObject.CompanyId));
			AddParameter(cmd, pGuid(MenuItemCategoryMapBase.Property_MenuId, menuItemCategoryMapObject.MenuId));
			AddParameter(cmd, pGuid(MenuItemCategoryMapBase.Property_CategoryId, menuItemCategoryMapObject.CategoryId));
			AddParameter(cmd, pGuid(MenuItemCategoryMapBase.Property_ItemId, menuItemCategoryMapObject.ItemId));
			AddParameter(cmd, pDateTime(MenuItemCategoryMapBase.Property_CreatedDate, menuItemCategoryMapObject.CreatedDate));
			AddParameter(cmd, pGuid(MenuItemCategoryMapBase.Property_CreatedBy, menuItemCategoryMapObject.CreatedBy));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts MenuItemCategoryMap
        /// </summary>
        /// <param name="menuItemCategoryMapObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(MenuItemCategoryMapBase menuItemCategoryMapObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTMENUITEMCATEGORYMAP);
	
				AddParameter(cmd, pInt32Out(MenuItemCategoryMapBase.Property_Id));
				AddCommonParams(cmd, menuItemCategoryMapObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					menuItemCategoryMapObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					menuItemCategoryMapObject.Id = (Int32)GetOutParameter(cmd, MenuItemCategoryMapBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(menuItemCategoryMapObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates MenuItemCategoryMap
        /// </summary>
        /// <param name="menuItemCategoryMapObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(MenuItemCategoryMapBase menuItemCategoryMapObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEMENUITEMCATEGORYMAP);
				
				AddParameter(cmd, pInt32(MenuItemCategoryMapBase.Property_Id, menuItemCategoryMapObject.Id));
				AddCommonParams(cmd, menuItemCategoryMapObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					menuItemCategoryMapObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(menuItemCategoryMapObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes MenuItemCategoryMap
        /// </summary>
        /// <param name="Id">Id of the MenuItemCategoryMap object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEMENUITEMCATEGORYMAP);	
				
				AddParameter(cmd, pInt32(MenuItemCategoryMapBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(MenuItemCategoryMap), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves MenuItemCategoryMap object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the MenuItemCategoryMap object to retrieve</param>
        /// <returns>MenuItemCategoryMap object, null if not found</returns>
		public MenuItemCategoryMap Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETMENUITEMCATEGORYMAPBYID))
			{
				AddParameter( cmd, pInt32(MenuItemCategoryMapBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all MenuItemCategoryMap objects 
        /// </summary>
        /// <returns>A list of MenuItemCategoryMap objects</returns>
		public MenuItemCategoryMapList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLMENUITEMCATEGORYMAP))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all MenuItemCategoryMap objects by PageRequest
        /// </summary>
        /// <returns>A list of MenuItemCategoryMap objects</returns>
		public MenuItemCategoryMapList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDMENUITEMCATEGORYMAP))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				MenuItemCategoryMapList _MenuItemCategoryMapList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _MenuItemCategoryMapList;
			}
		}
		
		/// <summary>
        /// Retrieves all MenuItemCategoryMap objects by query String
        /// </summary>
        /// <returns>A list of MenuItemCategoryMap objects</returns>
		public MenuItemCategoryMapList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETMENUITEMCATEGORYMAPBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get MenuItemCategoryMap Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of MenuItemCategoryMap
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETMENUITEMCATEGORYMAPMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get MenuItemCategoryMap Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of MenuItemCategoryMap
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _MenuItemCategoryMapRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETMENUITEMCATEGORYMAPROWCOUNT))
			{
				SqlDataReader reader;
				_MenuItemCategoryMapRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MenuItemCategoryMapRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills MenuItemCategoryMap object
        /// </summary>
        /// <param name="menuItemCategoryMapObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(MenuItemCategoryMapBase menuItemCategoryMapObject, SqlDataReader reader, int start)
		{
			
				menuItemCategoryMapObject.Id = reader.GetInt32( start + 0 );			
				menuItemCategoryMapObject.CompanyId = reader.GetGuid( start + 1 );			
				menuItemCategoryMapObject.MenuId = reader.GetGuid( start + 2 );			
				menuItemCategoryMapObject.CategoryId = reader.GetGuid( start + 3 );			
				menuItemCategoryMapObject.ItemId = reader.GetGuid( start + 4 );			
				menuItemCategoryMapObject.CreatedDate = reader.GetDateTime( start + 5 );			
				menuItemCategoryMapObject.CreatedBy = reader.GetGuid( start + 6 );			
			FillBaseObject(menuItemCategoryMapObject, reader, (start + 7));

			
			menuItemCategoryMapObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills MenuItemCategoryMap object
        /// </summary>
        /// <param name="menuItemCategoryMapObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(MenuItemCategoryMapBase menuItemCategoryMapObject, SqlDataReader reader)
		{
			FillObject(menuItemCategoryMapObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves MenuItemCategoryMap object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>MenuItemCategoryMap object</returns>
		private MenuItemCategoryMap GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					MenuItemCategoryMap menuItemCategoryMapObject= new MenuItemCategoryMap();
					FillObject(menuItemCategoryMapObject, reader);
					return menuItemCategoryMapObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of MenuItemCategoryMap objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of MenuItemCategoryMap objects</returns>
		private MenuItemCategoryMapList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//MenuItemCategoryMap list
			MenuItemCategoryMapList list = new MenuItemCategoryMapList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					MenuItemCategoryMap menuItemCategoryMapObject = new MenuItemCategoryMap();
					FillObject(menuItemCategoryMapObject, reader);

					list.Add(menuItemCategoryMapObject);
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
