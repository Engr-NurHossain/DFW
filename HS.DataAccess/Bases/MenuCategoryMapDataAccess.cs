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
	public partial class MenuCategoryMapDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTMENUCATEGORYMAP = "InsertMenuCategoryMap";
		private const string UPDATEMENUCATEGORYMAP = "UpdateMenuCategoryMap";
		private const string DELETEMENUCATEGORYMAP = "DeleteMenuCategoryMap";
		private const string GETMENUCATEGORYMAPBYID = "GetMenuCategoryMapById";
		private const string GETALLMENUCATEGORYMAP = "GetAllMenuCategoryMap";
		private const string GETPAGEDMENUCATEGORYMAP = "GetPagedMenuCategoryMap";
		private const string GETMENUCATEGORYMAPMAXIMUMID = "GetMenuCategoryMapMaximumId";
		private const string GETMENUCATEGORYMAPROWCOUNT = "GetMenuCategoryMapRowCount";	
		private const string GETMENUCATEGORYMAPBYQUERY = "GetMenuCategoryMapByQuery";
		#endregion
		
		#region Constructors
		public MenuCategoryMapDataAccess(ClientContext context) : base(context) { }
		public MenuCategoryMapDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="menuCategoryMapObject"></param>
		private void AddCommonParams(SqlCommand cmd, MenuCategoryMapBase menuCategoryMapObject)
		{	
			AddParameter(cmd, pGuid(MenuCategoryMapBase.Property_CompanyId, menuCategoryMapObject.CompanyId));
			AddParameter(cmd, pGuid(MenuCategoryMapBase.Property_MenuId, menuCategoryMapObject.MenuId));
			AddParameter(cmd, pGuid(MenuCategoryMapBase.Property_CategoryId, menuCategoryMapObject.CategoryId));
			AddParameter(cmd, pDateTime(MenuCategoryMapBase.Property_CreatedDate, menuCategoryMapObject.CreatedDate));
			AddParameter(cmd, pGuid(MenuCategoryMapBase.Property_CreatedBy, menuCategoryMapObject.CreatedBy));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts MenuCategoryMap
        /// </summary>
        /// <param name="menuCategoryMapObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(MenuCategoryMapBase menuCategoryMapObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTMENUCATEGORYMAP);
	
				AddParameter(cmd, pInt32Out(MenuCategoryMapBase.Property_Id));
				AddCommonParams(cmd, menuCategoryMapObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					menuCategoryMapObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					menuCategoryMapObject.Id = (Int32)GetOutParameter(cmd, MenuCategoryMapBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(menuCategoryMapObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates MenuCategoryMap
        /// </summary>
        /// <param name="menuCategoryMapObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(MenuCategoryMapBase menuCategoryMapObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEMENUCATEGORYMAP);
				
				AddParameter(cmd, pInt32(MenuCategoryMapBase.Property_Id, menuCategoryMapObject.Id));
				AddCommonParams(cmd, menuCategoryMapObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					menuCategoryMapObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(menuCategoryMapObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes MenuCategoryMap
        /// </summary>
        /// <param name="Id">Id of the MenuCategoryMap object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEMENUCATEGORYMAP);	
				
				AddParameter(cmd, pInt32(MenuCategoryMapBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(MenuCategoryMap), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves MenuCategoryMap object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the MenuCategoryMap object to retrieve</param>
        /// <returns>MenuCategoryMap object, null if not found</returns>
		public MenuCategoryMap Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETMENUCATEGORYMAPBYID))
			{
				AddParameter( cmd, pInt32(MenuCategoryMapBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all MenuCategoryMap objects 
        /// </summary>
        /// <returns>A list of MenuCategoryMap objects</returns>
		public MenuCategoryMapList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLMENUCATEGORYMAP))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all MenuCategoryMap objects by PageRequest
        /// </summary>
        /// <returns>A list of MenuCategoryMap objects</returns>
		public MenuCategoryMapList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDMENUCATEGORYMAP))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				MenuCategoryMapList _MenuCategoryMapList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _MenuCategoryMapList;
			}
		}
		
		/// <summary>
        /// Retrieves all MenuCategoryMap objects by query String
        /// </summary>
        /// <returns>A list of MenuCategoryMap objects</returns>
		public MenuCategoryMapList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETMENUCATEGORYMAPBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get MenuCategoryMap Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of MenuCategoryMap
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETMENUCATEGORYMAPMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get MenuCategoryMap Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of MenuCategoryMap
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _MenuCategoryMapRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETMENUCATEGORYMAPROWCOUNT))
			{
				SqlDataReader reader;
				_MenuCategoryMapRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MenuCategoryMapRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills MenuCategoryMap object
        /// </summary>
        /// <param name="menuCategoryMapObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(MenuCategoryMapBase menuCategoryMapObject, SqlDataReader reader, int start)
		{
			
				menuCategoryMapObject.Id = reader.GetInt32( start + 0 );			
				menuCategoryMapObject.CompanyId = reader.GetGuid( start + 1 );			
				menuCategoryMapObject.MenuId = reader.GetGuid( start + 2 );			
				menuCategoryMapObject.CategoryId = reader.GetGuid( start + 3 );			
				menuCategoryMapObject.CreatedDate = reader.GetDateTime( start + 4 );			
				menuCategoryMapObject.CreatedBy = reader.GetGuid( start + 5 );			
			FillBaseObject(menuCategoryMapObject, reader, (start + 6));

			
			menuCategoryMapObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills MenuCategoryMap object
        /// </summary>
        /// <param name="menuCategoryMapObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(MenuCategoryMapBase menuCategoryMapObject, SqlDataReader reader)
		{
			FillObject(menuCategoryMapObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves MenuCategoryMap object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>MenuCategoryMap object</returns>
		private MenuCategoryMap GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					MenuCategoryMap menuCategoryMapObject= new MenuCategoryMap();
					FillObject(menuCategoryMapObject, reader);
					return menuCategoryMapObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of MenuCategoryMap objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of MenuCategoryMap objects</returns>
		private MenuCategoryMapList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//MenuCategoryMap list
			MenuCategoryMapList list = new MenuCategoryMapList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					MenuCategoryMap menuCategoryMapObject = new MenuCategoryMap();
					FillObject(menuCategoryMapObject, reader);

					list.Add(menuCategoryMapObject);
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
