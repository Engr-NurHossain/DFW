﻿using System;
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
	public partial class MenuItemDetailDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTMENUITEMDETAIL = "InsertMenuItemDetail";
		private const string UPDATEMENUITEMDETAIL = "UpdateMenuItemDetail";
		private const string DELETEMENUITEMDETAIL = "DeleteMenuItemDetail";
		private const string GETMENUITEMDETAILBYID = "GetMenuItemDetailById";
		private const string GETALLMENUITEMDETAIL = "GetAllMenuItemDetail";
		private const string GETPAGEDMENUITEMDETAIL = "GetPagedMenuItemDetail";
		private const string GETMENUITEMDETAILMAXIMUMID = "GetMenuItemDetailMaximumId";
		private const string GETMENUITEMDETAILROWCOUNT = "GetMenuItemDetailRowCount";	
		private const string GETMENUITEMDETAILBYQUERY = "GetMenuItemDetailByQuery";
		#endregion
		
		#region Constructors
		public MenuItemDetailDataAccess(ClientContext context) : base(context) { }
		public MenuItemDetailDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="menuItemDetailObject"></param>
		private void AddCommonParams(SqlCommand cmd, MenuItemDetailBase menuItemDetailObject)
		{	
			AddParameter(cmd, pInt32(MenuItemDetailBase.Property_MenuItemId, menuItemDetailObject.MenuItemId));
			AddParameter(cmd, pInt32(MenuItemDetailBase.Property_MenuId, menuItemDetailObject.MenuId));
			AddParameter(cmd, pInt32(MenuItemDetailBase.Property_ToppingId, menuItemDetailObject.ToppingId));
			AddParameter(cmd, pInt32(MenuItemDetailBase.Property_CategoryId, menuItemDetailObject.CategoryId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts MenuItemDetail
        /// </summary>
        /// <param name="menuItemDetailObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(MenuItemDetailBase menuItemDetailObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTMENUITEMDETAIL);
	
				AddParameter(cmd, pInt32Out(MenuItemDetailBase.Property_Id));
				AddCommonParams(cmd, menuItemDetailObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					menuItemDetailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					menuItemDetailObject.Id = (Int32)GetOutParameter(cmd, MenuItemDetailBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(menuItemDetailObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates MenuItemDetail
        /// </summary>
        /// <param name="menuItemDetailObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(MenuItemDetailBase menuItemDetailObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEMENUITEMDETAIL);
				
				AddParameter(cmd, pInt32(MenuItemDetailBase.Property_Id, menuItemDetailObject.Id));
				AddCommonParams(cmd, menuItemDetailObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					menuItemDetailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(menuItemDetailObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes MenuItemDetail
        /// </summary>
        /// <param name="Id">Id of the MenuItemDetail object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEMENUITEMDETAIL);	
				
				AddParameter(cmd, pInt32(MenuItemDetailBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(MenuItemDetail), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves MenuItemDetail object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the MenuItemDetail object to retrieve</param>
        /// <returns>MenuItemDetail object, null if not found</returns>
		public MenuItemDetail Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETMENUITEMDETAILBYID))
			{
				AddParameter( cmd, pInt32(MenuItemDetailBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all MenuItemDetail objects 
        /// </summary>
        /// <returns>A list of MenuItemDetail objects</returns>
		public MenuItemDetailList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLMENUITEMDETAIL))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all MenuItemDetail objects by PageRequest
        /// </summary>
        /// <returns>A list of MenuItemDetail objects</returns>
		public MenuItemDetailList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDMENUITEMDETAIL))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				MenuItemDetailList _MenuItemDetailList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _MenuItemDetailList;
			}
		}
		
		/// <summary>
        /// Retrieves all MenuItemDetail objects by query String
        /// </summary>
        /// <returns>A list of MenuItemDetail objects</returns>
		public MenuItemDetailList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETMENUITEMDETAILBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get MenuItemDetail Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of MenuItemDetail
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETMENUITEMDETAILMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get MenuItemDetail Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of MenuItemDetail
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _MenuItemDetailRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETMENUITEMDETAILROWCOUNT))
			{
				SqlDataReader reader;
				_MenuItemDetailRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MenuItemDetailRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills MenuItemDetail object
        /// </summary>
        /// <param name="menuItemDetailObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(MenuItemDetailBase menuItemDetailObject, SqlDataReader reader, int start)
		{
			
				menuItemDetailObject.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) menuItemDetailObject.MenuItemId = reader.GetInt32( start + 1 );			
				if(!reader.IsDBNull(2)) menuItemDetailObject.MenuId = reader.GetInt32( start + 2 );			
				if(!reader.IsDBNull(3)) menuItemDetailObject.ToppingId = reader.GetInt32( start + 3 );			
				if(!reader.IsDBNull(4)) menuItemDetailObject.CategoryId = reader.GetInt32( start + 4 );			
			FillBaseObject(menuItemDetailObject, reader, (start + 5));

			
			menuItemDetailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills MenuItemDetail object
        /// </summary>
        /// <param name="menuItemDetailObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(MenuItemDetailBase menuItemDetailObject, SqlDataReader reader)
		{
			FillObject(menuItemDetailObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves MenuItemDetail object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>MenuItemDetail object</returns>
		private MenuItemDetail GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					MenuItemDetail menuItemDetailObject= new MenuItemDetail();
					FillObject(menuItemDetailObject, reader);
					return menuItemDetailObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of MenuItemDetail objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of MenuItemDetail objects</returns>
		private MenuItemDetailList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//MenuItemDetail list
			MenuItemDetailList list = new MenuItemDetailList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					MenuItemDetail menuItemDetailObject = new MenuItemDetail();
					FillObject(menuItemDetailObject, reader);

					list.Add(menuItemDetailObject);
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
