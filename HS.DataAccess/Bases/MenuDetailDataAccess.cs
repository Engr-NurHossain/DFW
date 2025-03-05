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
	public partial class MenuDetailDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTMENUDETAIL = "InsertMenuDetail";
		private const string UPDATEMENUDETAIL = "UpdateMenuDetail";
		private const string DELETEMENUDETAIL = "DeleteMenuDetail";
		private const string GETMENUDETAILBYID = "GetMenuDetailById";
		private const string GETALLMENUDETAIL = "GetAllMenuDetail";
		private const string GETPAGEDMENUDETAIL = "GetPagedMenuDetail";
		private const string GETMENUDETAILMAXIMUMID = "GetMenuDetailMaximumId";
		private const string GETMENUDETAILROWCOUNT = "GetMenuDetailRowCount";	
		private const string GETMENUDETAILBYQUERY = "GetMenuDetailByQuery";
		#endregion
		
		#region Constructors
		public MenuDetailDataAccess(ClientContext context) : base(context) { }
		public MenuDetailDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="menuDetailObject"></param>
		private void AddCommonParams(SqlCommand cmd, MenuDetailBase menuDetailObject)
		{	
			AddParameter(cmd, pInt32(MenuDetailBase.Property_MenuId, menuDetailObject.MenuId));
			AddParameter(cmd, pInt32(MenuDetailBase.Property_ToppingCategoryId, menuDetailObject.ToppingCategoryId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts MenuDetail
        /// </summary>
        /// <param name="menuDetailObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(MenuDetailBase menuDetailObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTMENUDETAIL);
	
				AddParameter(cmd, pInt32Out(MenuDetailBase.Property_Id));
				AddCommonParams(cmd, menuDetailObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					menuDetailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					menuDetailObject.Id = (Int32)GetOutParameter(cmd, MenuDetailBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(menuDetailObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates MenuDetail
        /// </summary>
        /// <param name="menuDetailObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(MenuDetailBase menuDetailObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEMENUDETAIL);
				
				AddParameter(cmd, pInt32(MenuDetailBase.Property_Id, menuDetailObject.Id));
				AddCommonParams(cmd, menuDetailObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					menuDetailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(menuDetailObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes MenuDetail
        /// </summary>
        /// <param name="Id">Id of the MenuDetail object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEMENUDETAIL);	
				
				AddParameter(cmd, pInt32(MenuDetailBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(MenuDetail), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves MenuDetail object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the MenuDetail object to retrieve</param>
        /// <returns>MenuDetail object, null if not found</returns>
		public MenuDetail Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETMENUDETAILBYID))
			{
				AddParameter( cmd, pInt32(MenuDetailBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all MenuDetail objects 
        /// </summary>
        /// <returns>A list of MenuDetail objects</returns>
		public MenuDetailList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLMENUDETAIL))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all MenuDetail objects by PageRequest
        /// </summary>
        /// <returns>A list of MenuDetail objects</returns>
		public MenuDetailList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDMENUDETAIL))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				MenuDetailList _MenuDetailList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _MenuDetailList;
			}
		}
		
		/// <summary>
        /// Retrieves all MenuDetail objects by query String
        /// </summary>
        /// <returns>A list of MenuDetail objects</returns>
		public MenuDetailList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETMENUDETAILBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get MenuDetail Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of MenuDetail
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETMENUDETAILMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get MenuDetail Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of MenuDetail
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _MenuDetailRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETMENUDETAILROWCOUNT))
			{
				SqlDataReader reader;
				_MenuDetailRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MenuDetailRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills MenuDetail object
        /// </summary>
        /// <param name="menuDetailObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(MenuDetailBase menuDetailObject, SqlDataReader reader, int start)
		{
			
				menuDetailObject.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) menuDetailObject.MenuId = reader.GetInt32( start + 1 );			
				if(!reader.IsDBNull(2)) menuDetailObject.ToppingCategoryId = reader.GetInt32( start + 2 );			
			FillBaseObject(menuDetailObject, reader, (start + 3));

			
			menuDetailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills MenuDetail object
        /// </summary>
        /// <param name="menuDetailObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(MenuDetailBase menuDetailObject, SqlDataReader reader)
		{
			FillObject(menuDetailObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves MenuDetail object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>MenuDetail object</returns>
		private MenuDetail GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					MenuDetail menuDetailObject= new MenuDetail();
					FillObject(menuDetailObject, reader);
					return menuDetailObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of MenuDetail objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of MenuDetail objects</returns>
		private MenuDetailList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//MenuDetail list
			MenuDetailList list = new MenuDetailList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					MenuDetail menuDetailObject = new MenuDetail();
					FillObject(menuDetailObject, reader);

					list.Add(menuDetailObject);
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
