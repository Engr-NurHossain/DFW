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
	public partial class MenuItemCategoryToppingMapDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTMENUITEMCATEGORYTOPPINGMAP = "InsertMenuItemCategoryToppingMap";
		private const string UPDATEMENUITEMCATEGORYTOPPINGMAP = "UpdateMenuItemCategoryToppingMap";
		private const string DELETEMENUITEMCATEGORYTOPPINGMAP = "DeleteMenuItemCategoryToppingMap";
		private const string GETMENUITEMCATEGORYTOPPINGMAPBYID = "GetMenuItemCategoryToppingMapById";
		private const string GETALLMENUITEMCATEGORYTOPPINGMAP = "GetAllMenuItemCategoryToppingMap";
		private const string GETPAGEDMENUITEMCATEGORYTOPPINGMAP = "GetPagedMenuItemCategoryToppingMap";
		private const string GETMENUITEMCATEGORYTOPPINGMAPMAXIMUMID = "GetMenuItemCategoryToppingMapMaximumId";
		private const string GETMENUITEMCATEGORYTOPPINGMAPROWCOUNT = "GetMenuItemCategoryToppingMapRowCount";	
		private const string GETMENUITEMCATEGORYTOPPINGMAPBYQUERY = "GetMenuItemCategoryToppingMapByQuery";
		#endregion
		
		#region Constructors
		public MenuItemCategoryToppingMapDataAccess(ClientContext context) : base(context) { }
		public MenuItemCategoryToppingMapDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="menuItemCategoryToppingMapObject"></param>
		private void AddCommonParams(SqlCommand cmd, MenuItemCategoryToppingMapBase menuItemCategoryToppingMapObject)
		{	
			AddParameter(cmd, pGuid(MenuItemCategoryToppingMapBase.Property_CompanyId, menuItemCategoryToppingMapObject.CompanyId));
			AddParameter(cmd, pGuid(MenuItemCategoryToppingMapBase.Property_MenuId, menuItemCategoryToppingMapObject.MenuId));
			AddParameter(cmd, pGuid(MenuItemCategoryToppingMapBase.Property_ToppingCategoryId, menuItemCategoryToppingMapObject.ToppingCategoryId));
			AddParameter(cmd, pGuid(MenuItemCategoryToppingMapBase.Property_ItemId, menuItemCategoryToppingMapObject.ItemId));
			AddParameter(cmd, pDateTime(MenuItemCategoryToppingMapBase.Property_CreatedDate, menuItemCategoryToppingMapObject.CreatedDate));
			AddParameter(cmd, pGuid(MenuItemCategoryToppingMapBase.Property_CreatedBy, menuItemCategoryToppingMapObject.CreatedBy));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts MenuItemCategoryToppingMap
        /// </summary>
        /// <param name="menuItemCategoryToppingMapObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(MenuItemCategoryToppingMapBase menuItemCategoryToppingMapObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTMENUITEMCATEGORYTOPPINGMAP);
	
				AddParameter(cmd, pInt32Out(MenuItemCategoryToppingMapBase.Property_Id));
				AddCommonParams(cmd, menuItemCategoryToppingMapObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					menuItemCategoryToppingMapObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					menuItemCategoryToppingMapObject.Id = (Int32)GetOutParameter(cmd, MenuItemCategoryToppingMapBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(menuItemCategoryToppingMapObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates MenuItemCategoryToppingMap
        /// </summary>
        /// <param name="menuItemCategoryToppingMapObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(MenuItemCategoryToppingMapBase menuItemCategoryToppingMapObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEMENUITEMCATEGORYTOPPINGMAP);
				
				AddParameter(cmd, pInt32(MenuItemCategoryToppingMapBase.Property_Id, menuItemCategoryToppingMapObject.Id));
				AddCommonParams(cmd, menuItemCategoryToppingMapObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					menuItemCategoryToppingMapObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(menuItemCategoryToppingMapObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes MenuItemCategoryToppingMap
        /// </summary>
        /// <param name="Id">Id of the MenuItemCategoryToppingMap object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEMENUITEMCATEGORYTOPPINGMAP);	
				
				AddParameter(cmd, pInt32(MenuItemCategoryToppingMapBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(MenuItemCategoryToppingMap), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves MenuItemCategoryToppingMap object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the MenuItemCategoryToppingMap object to retrieve</param>
        /// <returns>MenuItemCategoryToppingMap object, null if not found</returns>
		public MenuItemCategoryToppingMap Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETMENUITEMCATEGORYTOPPINGMAPBYID))
			{
				AddParameter( cmd, pInt32(MenuItemCategoryToppingMapBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all MenuItemCategoryToppingMap objects 
        /// </summary>
        /// <returns>A list of MenuItemCategoryToppingMap objects</returns>
		public MenuItemCategoryToppingMapList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLMENUITEMCATEGORYTOPPINGMAP))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all MenuItemCategoryToppingMap objects by PageRequest
        /// </summary>
        /// <returns>A list of MenuItemCategoryToppingMap objects</returns>
		public MenuItemCategoryToppingMapList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDMENUITEMCATEGORYTOPPINGMAP))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				MenuItemCategoryToppingMapList _MenuItemCategoryToppingMapList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _MenuItemCategoryToppingMapList;
			}
		}
		
		/// <summary>
        /// Retrieves all MenuItemCategoryToppingMap objects by query String
        /// </summary>
        /// <returns>A list of MenuItemCategoryToppingMap objects</returns>
		public MenuItemCategoryToppingMapList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETMENUITEMCATEGORYTOPPINGMAPBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get MenuItemCategoryToppingMap Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of MenuItemCategoryToppingMap
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETMENUITEMCATEGORYTOPPINGMAPMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get MenuItemCategoryToppingMap Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of MenuItemCategoryToppingMap
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _MenuItemCategoryToppingMapRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETMENUITEMCATEGORYTOPPINGMAPROWCOUNT))
			{
				SqlDataReader reader;
				_MenuItemCategoryToppingMapRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MenuItemCategoryToppingMapRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills MenuItemCategoryToppingMap object
        /// </summary>
        /// <param name="menuItemCategoryToppingMapObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(MenuItemCategoryToppingMapBase menuItemCategoryToppingMapObject, SqlDataReader reader, int start)
		{
			
				menuItemCategoryToppingMapObject.Id = reader.GetInt32( start + 0 );			
				menuItemCategoryToppingMapObject.CompanyId = reader.GetGuid( start + 1 );			
				menuItemCategoryToppingMapObject.MenuId = reader.GetGuid( start + 2 );			
				menuItemCategoryToppingMapObject.ToppingCategoryId = reader.GetGuid( start + 3 );			
				menuItemCategoryToppingMapObject.ItemId = reader.GetGuid( start + 4 );			
				menuItemCategoryToppingMapObject.CreatedDate = reader.GetDateTime( start + 5 );			
				menuItemCategoryToppingMapObject.CreatedBy = reader.GetGuid( start + 6 );			
			FillBaseObject(menuItemCategoryToppingMapObject, reader, (start + 7));

			
			menuItemCategoryToppingMapObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills MenuItemCategoryToppingMap object
        /// </summary>
        /// <param name="menuItemCategoryToppingMapObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(MenuItemCategoryToppingMapBase menuItemCategoryToppingMapObject, SqlDataReader reader)
		{
			FillObject(menuItemCategoryToppingMapObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves MenuItemCategoryToppingMap object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>MenuItemCategoryToppingMap object</returns>
		private MenuItemCategoryToppingMap GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					MenuItemCategoryToppingMap menuItemCategoryToppingMapObject= new MenuItemCategoryToppingMap();
					FillObject(menuItemCategoryToppingMapObject, reader);
					return menuItemCategoryToppingMapObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of MenuItemCategoryToppingMap objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of MenuItemCategoryToppingMap objects</returns>
		private MenuItemCategoryToppingMapList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//MenuItemCategoryToppingMap list
			MenuItemCategoryToppingMapList list = new MenuItemCategoryToppingMapList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					MenuItemCategoryToppingMap menuItemCategoryToppingMapObject = new MenuItemCategoryToppingMap();
					FillObject(menuItemCategoryToppingMapObject, reader);

					list.Add(menuItemCategoryToppingMapObject);
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
