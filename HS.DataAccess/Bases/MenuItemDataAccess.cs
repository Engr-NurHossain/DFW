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
	public partial class MenuItemDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTMENUITEM = "InsertMenuItem";
		private const string UPDATEMENUITEM = "UpdateMenuItem";
		private const string DELETEMENUITEM = "DeleteMenuItem";
		private const string GETMENUITEMBYID = "GetMenuItemById";
		private const string GETALLMENUITEM = "GetAllMenuItem";
		private const string GETPAGEDMENUITEM = "GetPagedMenuItem";
		private const string GETMENUITEMMAXIMUMID = "GetMenuItemMaximumId";
		private const string GETMENUITEMROWCOUNT = "GetMenuItemRowCount";	
		private const string GETMENUITEMBYQUERY = "GetMenuItemByQuery";
		#endregion
		
		#region Constructors
		public MenuItemDataAccess(ClientContext context) : base(context) { }
		public MenuItemDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="menuItemObject"></param>
		private void AddCommonParams(SqlCommand cmd, MenuItemBase menuItemObject)
		{	
			AddParameter(cmd, pGuid(MenuItemBase.Property_ItemId, menuItemObject.ItemId));
			AddParameter(cmd, pNVarChar(MenuItemBase.Property_ItemName, 50, menuItemObject.ItemName));
			AddParameter(cmd, pNVarChar(MenuItemBase.Property_ItemNumber, 50, menuItemObject.ItemNumber));
			AddParameter(cmd, pNVarChar(MenuItemBase.Property_ItemLevel, 50, menuItemObject.ItemLevel));
			AddParameter(cmd, pNVarChar(MenuItemBase.Property_Description, menuItemObject.Description));
			AddParameter(cmd, pNVarChar(MenuItemBase.Property_Photo, menuItemObject.Photo));
			AddParameter(cmd, pInt32(MenuItemBase.Property_MaxQty, menuItemObject.MaxQty));
			AddParameter(cmd, pNVarChar(MenuItemBase.Property_DaysAvailable, menuItemObject.DaysAvailable));
			AddParameter(cmd, pNVarChar(MenuItemBase.Property_TimeAvailable, menuItemObject.TimeAvailable));
			AddParameter(cmd, pDouble(MenuItemBase.Property_Price, menuItemObject.Price));
			AddParameter(cmd, pBool(MenuItemBase.Property_Status, menuItemObject.Status));
			AddParameter(cmd, pNVarChar(MenuItemBase.Property_DaysAvailableOption, 50, menuItemObject.DaysAvailableOption));
			AddParameter(cmd, pNVarChar(MenuItemBase.Property_TimeAvailableOption, 50, menuItemObject.TimeAvailableOption));
			AddParameter(cmd, pGuid(MenuItemBase.Property_CompanyId, menuItemObject.CompanyId));
			AddParameter(cmd, pNVarChar(MenuItemBase.Property_UrlSlug, menuItemObject.UrlSlug));
			AddParameter(cmd, pNVarChar(MenuItemBase.Property_WebsiteURL, menuItemObject.WebsiteURL));
			AddParameter(cmd, pNVarChar(MenuItemBase.Property_MetaTitle, menuItemObject.MetaTitle));
			AddParameter(cmd, pNVarChar(MenuItemBase.Property_MetaDescription, menuItemObject.MetaDescription));
			AddParameter(cmd, pInt32(MenuItemBase.Property_DeliveryTime, menuItemObject.DeliveryTime));
			AddParameter(cmd, pBool(MenuItemBase.Property_IsTax, menuItemObject.IsTax));
			AddParameter(cmd, pDouble(MenuItemBase.Property_TaxPercentage, menuItemObject.TaxPercentage));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts MenuItem
        /// </summary>
        /// <param name="menuItemObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(MenuItemBase menuItemObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTMENUITEM);
	
				AddParameter(cmd, pInt32Out(MenuItemBase.Property_Id));
				AddCommonParams(cmd, menuItemObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					menuItemObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					menuItemObject.Id = (Int32)GetOutParameter(cmd, MenuItemBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(menuItemObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates MenuItem
        /// </summary>
        /// <param name="menuItemObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(MenuItemBase menuItemObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEMENUITEM);
				
				AddParameter(cmd, pInt32(MenuItemBase.Property_Id, menuItemObject.Id));
				AddCommonParams(cmd, menuItemObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					menuItemObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(menuItemObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes MenuItem
        /// </summary>
        /// <param name="Id">Id of the MenuItem object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEMENUITEM);	
				
				AddParameter(cmd, pInt32(MenuItemBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(MenuItem), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves MenuItem object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the MenuItem object to retrieve</param>
        /// <returns>MenuItem object, null if not found</returns>
		public MenuItem Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETMENUITEMBYID))
			{
				AddParameter( cmd, pInt32(MenuItemBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all MenuItem objects 
        /// </summary>
        /// <returns>A list of MenuItem objects</returns>
		public MenuItemList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLMENUITEM))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all MenuItem objects by PageRequest
        /// </summary>
        /// <returns>A list of MenuItem objects</returns>
		public MenuItemList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDMENUITEM))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				MenuItemList _MenuItemList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _MenuItemList;
			}
		}
		
		/// <summary>
        /// Retrieves all MenuItem objects by query String
        /// </summary>
        /// <returns>A list of MenuItem objects</returns>
		public MenuItemList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETMENUITEMBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get MenuItem Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of MenuItem
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETMENUITEMMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get MenuItem Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of MenuItem
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _MenuItemRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETMENUITEMROWCOUNT))
			{
				SqlDataReader reader;
				_MenuItemRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MenuItemRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills MenuItem object
        /// </summary>
        /// <param name="menuItemObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(MenuItemBase menuItemObject, SqlDataReader reader, int start)
		{
			
				menuItemObject.Id = reader.GetInt32( start + 0 );			
				menuItemObject.ItemId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) menuItemObject.ItemName = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) menuItemObject.ItemNumber = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) menuItemObject.ItemLevel = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) menuItemObject.Description = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) menuItemObject.Photo = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) menuItemObject.MaxQty = reader.GetInt32( start + 7 );			
				if(!reader.IsDBNull(8)) menuItemObject.DaysAvailable = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) menuItemObject.TimeAvailable = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) menuItemObject.Price = reader.GetDouble( start + 10 );			
				if(!reader.IsDBNull(11)) menuItemObject.Status = reader.GetBoolean( start + 11 );			
				if(!reader.IsDBNull(12)) menuItemObject.DaysAvailableOption = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) menuItemObject.TimeAvailableOption = reader.GetString( start + 13 );			
				menuItemObject.CompanyId = reader.GetGuid( start + 14 );			
				if(!reader.IsDBNull(15)) menuItemObject.UrlSlug = reader.GetString( start + 15 );			
				if(!reader.IsDBNull(16)) menuItemObject.WebsiteURL = reader.GetString( start + 16 );			
				if(!reader.IsDBNull(17)) menuItemObject.MetaTitle = reader.GetString( start + 17 );			
				if(!reader.IsDBNull(18)) menuItemObject.MetaDescription = reader.GetString( start + 18 );			
				if(!reader.IsDBNull(19)) menuItemObject.DeliveryTime = reader.GetInt32( start + 19 );			
				if(!reader.IsDBNull(20)) menuItemObject.IsTax = reader.GetBoolean( start + 20 );			
				if(!reader.IsDBNull(21)) menuItemObject.TaxPercentage = reader.GetDouble( start + 21 );			
			FillBaseObject(menuItemObject, reader, (start + 22));

			
			menuItemObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills MenuItem object
        /// </summary>
        /// <param name="menuItemObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(MenuItemBase menuItemObject, SqlDataReader reader)
		{
			FillObject(menuItemObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves MenuItem object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>MenuItem object</returns>
		private MenuItem GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					MenuItem menuItemObject= new MenuItem();
					FillObject(menuItemObject, reader);
					return menuItemObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of MenuItem objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of MenuItem objects</returns>
		private MenuItemList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//MenuItem list
			MenuItemList list = new MenuItemList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					MenuItem menuItemObject = new MenuItem();
					FillObject(menuItemObject, reader);

					list.Add(menuItemObject);
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
