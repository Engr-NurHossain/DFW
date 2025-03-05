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
	public partial class MenuItemCategoryURLDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTMENUITEMCATEGORYURL = "InsertMenuItemCategoryURL";
		private const string UPDATEMENUITEMCATEGORYURL = "UpdateMenuItemCategoryURL";
		private const string DELETEMENUITEMCATEGORYURL = "DeleteMenuItemCategoryURL";
		private const string GETMENUITEMCATEGORYURLBYID = "GetMenuItemCategoryURLById";
		private const string GETALLMENUITEMCATEGORYURL = "GetAllMenuItemCategoryURL";
		private const string GETPAGEDMENUITEMCATEGORYURL = "GetPagedMenuItemCategoryURL";
		private const string GETMENUITEMCATEGORYURLMAXIMUMID = "GetMenuItemCategoryURLMaximumId";
		private const string GETMENUITEMCATEGORYURLROWCOUNT = "GetMenuItemCategoryURLRowCount";	
		private const string GETMENUITEMCATEGORYURLBYQUERY = "GetMenuItemCategoryURLByQuery";
		#endregion
		
		#region Constructors
		public MenuItemCategoryURLDataAccess(ClientContext context) : base(context) { }
		public MenuItemCategoryURLDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="menuItemCategoryURLObject"></param>
		private void AddCommonParams(SqlCommand cmd, MenuItemCategoryURLBase menuItemCategoryURLObject)
		{	
			AddParameter(cmd, pInt32(MenuItemCategoryURLBase.Property_MenuId, menuItemCategoryURLObject.MenuId));
			AddParameter(cmd, pInt32(MenuItemCategoryURLBase.Property_MenuItemId, menuItemCategoryURLObject.MenuItemId));
			AddParameter(cmd, pNVarChar(MenuItemCategoryURLBase.Property_MenuCategoryURL, menuItemCategoryURLObject.MenuCategoryURL));
			AddParameter(cmd, pNVarChar(MenuItemCategoryURLBase.Property_ItemCategoryURL, menuItemCategoryURLObject.ItemCategoryURL));
			AddParameter(cmd, pDateTime(MenuItemCategoryURLBase.Property_CreatedDate, menuItemCategoryURLObject.CreatedDate));
			AddParameter(cmd, pGuid(MenuItemCategoryURLBase.Property_CreatedBy, menuItemCategoryURLObject.CreatedBy));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts MenuItemCategoryURL
        /// </summary>
        /// <param name="menuItemCategoryURLObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(MenuItemCategoryURLBase menuItemCategoryURLObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTMENUITEMCATEGORYURL);
	
				AddParameter(cmd, pInt32Out(MenuItemCategoryURLBase.Property_Id));
				AddCommonParams(cmd, menuItemCategoryURLObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					menuItemCategoryURLObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					menuItemCategoryURLObject.Id = (Int32)GetOutParameter(cmd, MenuItemCategoryURLBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(menuItemCategoryURLObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates MenuItemCategoryURL
        /// </summary>
        /// <param name="menuItemCategoryURLObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(MenuItemCategoryURLBase menuItemCategoryURLObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEMENUITEMCATEGORYURL);
				
				AddParameter(cmd, pInt32(MenuItemCategoryURLBase.Property_Id, menuItemCategoryURLObject.Id));
				AddCommonParams(cmd, menuItemCategoryURLObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					menuItemCategoryURLObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(menuItemCategoryURLObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes MenuItemCategoryURL
        /// </summary>
        /// <param name="Id">Id of the MenuItemCategoryURL object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEMENUITEMCATEGORYURL);	
				
				AddParameter(cmd, pInt32(MenuItemCategoryURLBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(MenuItemCategoryURL), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves MenuItemCategoryURL object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the MenuItemCategoryURL object to retrieve</param>
        /// <returns>MenuItemCategoryURL object, null if not found</returns>
		public MenuItemCategoryURL Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETMENUITEMCATEGORYURLBYID))
			{
				AddParameter( cmd, pInt32(MenuItemCategoryURLBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all MenuItemCategoryURL objects 
        /// </summary>
        /// <returns>A list of MenuItemCategoryURL objects</returns>
		public MenuItemCategoryURLList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLMENUITEMCATEGORYURL))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all MenuItemCategoryURL objects by PageRequest
        /// </summary>
        /// <returns>A list of MenuItemCategoryURL objects</returns>
		public MenuItemCategoryURLList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDMENUITEMCATEGORYURL))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				MenuItemCategoryURLList _MenuItemCategoryURLList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _MenuItemCategoryURLList;
			}
		}
		
		/// <summary>
        /// Retrieves all MenuItemCategoryURL objects by query String
        /// </summary>
        /// <returns>A list of MenuItemCategoryURL objects</returns>
		public MenuItemCategoryURLList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETMENUITEMCATEGORYURLBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get MenuItemCategoryURL Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of MenuItemCategoryURL
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETMENUITEMCATEGORYURLMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get MenuItemCategoryURL Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of MenuItemCategoryURL
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _MenuItemCategoryURLRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETMENUITEMCATEGORYURLROWCOUNT))
			{
				SqlDataReader reader;
				_MenuItemCategoryURLRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MenuItemCategoryURLRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills MenuItemCategoryURL object
        /// </summary>
        /// <param name="menuItemCategoryURLObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(MenuItemCategoryURLBase menuItemCategoryURLObject, SqlDataReader reader, int start)
		{
			
				menuItemCategoryURLObject.Id = reader.GetInt32( start + 0 );			
				menuItemCategoryURLObject.MenuId = reader.GetInt32( start + 1 );			
				menuItemCategoryURLObject.MenuItemId = reader.GetInt32( start + 2 );			
				if(!reader.IsDBNull(3)) menuItemCategoryURLObject.MenuCategoryURL = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) menuItemCategoryURLObject.ItemCategoryURL = reader.GetString( start + 4 );			
				menuItemCategoryURLObject.CreatedDate = reader.GetDateTime( start + 5 );			
				menuItemCategoryURLObject.CreatedBy = reader.GetGuid( start + 6 );			
			FillBaseObject(menuItemCategoryURLObject, reader, (start + 7));

			
			menuItemCategoryURLObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills MenuItemCategoryURL object
        /// </summary>
        /// <param name="menuItemCategoryURLObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(MenuItemCategoryURLBase menuItemCategoryURLObject, SqlDataReader reader)
		{
			FillObject(menuItemCategoryURLObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves MenuItemCategoryURL object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>MenuItemCategoryURL object</returns>
		private MenuItemCategoryURL GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					MenuItemCategoryURL menuItemCategoryURLObject= new MenuItemCategoryURL();
					FillObject(menuItemCategoryURLObject, reader);
					return menuItemCategoryURLObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of MenuItemCategoryURL objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of MenuItemCategoryURL objects</returns>
		private MenuItemCategoryURLList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//MenuItemCategoryURL list
			MenuItemCategoryURLList list = new MenuItemCategoryURLList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					MenuItemCategoryURL menuItemCategoryURLObject = new MenuItemCategoryURL();
					FillObject(menuItemCategoryURLObject, reader);

					list.Add(menuItemCategoryURLObject);
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
