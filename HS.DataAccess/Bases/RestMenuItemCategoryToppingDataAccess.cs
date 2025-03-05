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
	public partial class RestMenuItemCategoryToppingDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTRESTMENUITEMCATEGORYTOPPING = "InsertRestMenuItemCategoryTopping";
		private const string UPDATERESTMENUITEMCATEGORYTOPPING = "UpdateRestMenuItemCategoryTopping";
		private const string DELETERESTMENUITEMCATEGORYTOPPING = "DeleteRestMenuItemCategoryTopping";
		private const string GETRESTMENUITEMCATEGORYTOPPINGBYID = "GetRestMenuItemCategoryToppingById";
		private const string GETALLRESTMENUITEMCATEGORYTOPPING = "GetAllRestMenuItemCategoryTopping";
		private const string GETPAGEDRESTMENUITEMCATEGORYTOPPING = "GetPagedRestMenuItemCategoryTopping";
		private const string GETRESTMENUITEMCATEGORYTOPPINGMAXIMUMID = "GetRestMenuItemCategoryToppingMaximumId";
		private const string GETRESTMENUITEMCATEGORYTOPPINGROWCOUNT = "GetRestMenuItemCategoryToppingRowCount";	
		private const string GETRESTMENUITEMCATEGORYTOPPINGBYQUERY = "GetRestMenuItemCategoryToppingByQuery";
		#endregion
		
		#region Constructors
		public RestMenuItemCategoryToppingDataAccess(ClientContext context) : base(context) { }
		public RestMenuItemCategoryToppingDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="restMenuItemCategoryToppingObject"></param>
		private void AddCommonParams(SqlCommand cmd, RestMenuItemCategoryToppingBase restMenuItemCategoryToppingObject)
		{	
			AddParameter(cmd, pGuid(RestMenuItemCategoryToppingBase.Property_CompanyId, restMenuItemCategoryToppingObject.CompanyId));
			AddParameter(cmd, pGuid(RestMenuItemCategoryToppingBase.Property_MenuId, restMenuItemCategoryToppingObject.MenuId));
			AddParameter(cmd, pGuid(RestMenuItemCategoryToppingBase.Property_ToppingCategoryId, restMenuItemCategoryToppingObject.ToppingCategoryId));
			AddParameter(cmd, pGuid(RestMenuItemCategoryToppingBase.Property_ItemId, restMenuItemCategoryToppingObject.ItemId));
			AddParameter(cmd, pDateTime(RestMenuItemCategoryToppingBase.Property_CreatedDate, restMenuItemCategoryToppingObject.CreatedDate));
			AddParameter(cmd, pGuid(RestMenuItemCategoryToppingBase.Property_CreatedBy, restMenuItemCategoryToppingObject.CreatedBy));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts RestMenuItemCategoryTopping
        /// </summary>
        /// <param name="restMenuItemCategoryToppingObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(RestMenuItemCategoryToppingBase restMenuItemCategoryToppingObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTRESTMENUITEMCATEGORYTOPPING);
	
				AddParameter(cmd, pInt32Out(RestMenuItemCategoryToppingBase.Property_Id));
				AddCommonParams(cmd, restMenuItemCategoryToppingObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					restMenuItemCategoryToppingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					restMenuItemCategoryToppingObject.Id = (Int32)GetOutParameter(cmd, RestMenuItemCategoryToppingBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(restMenuItemCategoryToppingObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates RestMenuItemCategoryTopping
        /// </summary>
        /// <param name="restMenuItemCategoryToppingObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(RestMenuItemCategoryToppingBase restMenuItemCategoryToppingObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATERESTMENUITEMCATEGORYTOPPING);
				
				AddParameter(cmd, pInt32(RestMenuItemCategoryToppingBase.Property_Id, restMenuItemCategoryToppingObject.Id));
				AddCommonParams(cmd, restMenuItemCategoryToppingObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					restMenuItemCategoryToppingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(restMenuItemCategoryToppingObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes RestMenuItemCategoryTopping
        /// </summary>
        /// <param name="Id">Id of the RestMenuItemCategoryTopping object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETERESTMENUITEMCATEGORYTOPPING);	
				
				AddParameter(cmd, pInt32(RestMenuItemCategoryToppingBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(RestMenuItemCategoryTopping), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves RestMenuItemCategoryTopping object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the RestMenuItemCategoryTopping object to retrieve</param>
        /// <returns>RestMenuItemCategoryTopping object, null if not found</returns>
		public RestMenuItemCategoryTopping Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTMENUITEMCATEGORYTOPPINGBYID))
			{
				AddParameter( cmd, pInt32(RestMenuItemCategoryToppingBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all RestMenuItemCategoryTopping objects 
        /// </summary>
        /// <returns>A list of RestMenuItemCategoryTopping objects</returns>
		public RestMenuItemCategoryToppingList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLRESTMENUITEMCATEGORYTOPPING))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all RestMenuItemCategoryTopping objects by PageRequest
        /// </summary>
        /// <returns>A list of RestMenuItemCategoryTopping objects</returns>
		public RestMenuItemCategoryToppingList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDRESTMENUITEMCATEGORYTOPPING))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				RestMenuItemCategoryToppingList _RestMenuItemCategoryToppingList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _RestMenuItemCategoryToppingList;
			}
		}
		
		/// <summary>
        /// Retrieves all RestMenuItemCategoryTopping objects by query String
        /// </summary>
        /// <returns>A list of RestMenuItemCategoryTopping objects</returns>
		public RestMenuItemCategoryToppingList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTMENUITEMCATEGORYTOPPINGBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get RestMenuItemCategoryTopping Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of RestMenuItemCategoryTopping
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTMENUITEMCATEGORYTOPPINGMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get RestMenuItemCategoryTopping Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of RestMenuItemCategoryTopping
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _RestMenuItemCategoryToppingRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTMENUITEMCATEGORYTOPPINGROWCOUNT))
			{
				SqlDataReader reader;
				_RestMenuItemCategoryToppingRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _RestMenuItemCategoryToppingRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills RestMenuItemCategoryTopping object
        /// </summary>
        /// <param name="restMenuItemCategoryToppingObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(RestMenuItemCategoryToppingBase restMenuItemCategoryToppingObject, SqlDataReader reader, int start)
		{
			
				restMenuItemCategoryToppingObject.Id = reader.GetInt32( start + 0 );			
				restMenuItemCategoryToppingObject.CompanyId = reader.GetGuid( start + 1 );			
				restMenuItemCategoryToppingObject.MenuId = reader.GetGuid( start + 2 );			
				restMenuItemCategoryToppingObject.ToppingCategoryId = reader.GetGuid( start + 3 );			
				restMenuItemCategoryToppingObject.ItemId = reader.GetGuid( start + 4 );			
				restMenuItemCategoryToppingObject.CreatedDate = reader.GetDateTime( start + 5 );			
				restMenuItemCategoryToppingObject.CreatedBy = reader.GetGuid( start + 6 );			
			FillBaseObject(restMenuItemCategoryToppingObject, reader, (start + 7));

			
			restMenuItemCategoryToppingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills RestMenuItemCategoryTopping object
        /// </summary>
        /// <param name="restMenuItemCategoryToppingObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(RestMenuItemCategoryToppingBase restMenuItemCategoryToppingObject, SqlDataReader reader)
		{
			FillObject(restMenuItemCategoryToppingObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves RestMenuItemCategoryTopping object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>RestMenuItemCategoryTopping object</returns>
		private RestMenuItemCategoryTopping GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					RestMenuItemCategoryTopping restMenuItemCategoryToppingObject= new RestMenuItemCategoryTopping();
					FillObject(restMenuItemCategoryToppingObject, reader);
					return restMenuItemCategoryToppingObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of RestMenuItemCategoryTopping objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of RestMenuItemCategoryTopping objects</returns>
		private RestMenuItemCategoryToppingList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//RestMenuItemCategoryTopping list
			RestMenuItemCategoryToppingList list = new RestMenuItemCategoryToppingList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					RestMenuItemCategoryTopping restMenuItemCategoryToppingObject = new RestMenuItemCategoryTopping();
					FillObject(restMenuItemCategoryToppingObject, reader);

					list.Add(restMenuItemCategoryToppingObject);
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
