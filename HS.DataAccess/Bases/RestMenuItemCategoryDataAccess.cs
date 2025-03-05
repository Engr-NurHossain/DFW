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
	public partial class RestMenuItemCategoryDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTRESTMENUITEMCATEGORY = "InsertRestMenuItemCategory";
		private const string UPDATERESTMENUITEMCATEGORY = "UpdateRestMenuItemCategory";
		private const string DELETERESTMENUITEMCATEGORY = "DeleteRestMenuItemCategory";
		private const string GETRESTMENUITEMCATEGORYBYID = "GetRestMenuItemCategoryById";
		private const string GETALLRESTMENUITEMCATEGORY = "GetAllRestMenuItemCategory";
		private const string GETPAGEDRESTMENUITEMCATEGORY = "GetPagedRestMenuItemCategory";
		private const string GETRESTMENUITEMCATEGORYMAXIMUMID = "GetRestMenuItemCategoryMaximumId";
		private const string GETRESTMENUITEMCATEGORYROWCOUNT = "GetRestMenuItemCategoryRowCount";	
		private const string GETRESTMENUITEMCATEGORYBYQUERY = "GetRestMenuItemCategoryByQuery";
		#endregion
		
		#region Constructors
		public RestMenuItemCategoryDataAccess(ClientContext context) : base(context) { }
		public RestMenuItemCategoryDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="restMenuItemCategoryObject"></param>
		private void AddCommonParams(SqlCommand cmd, RestMenuItemCategoryBase restMenuItemCategoryObject)
		{	
			AddParameter(cmd, pGuid(RestMenuItemCategoryBase.Property_CompanyId, restMenuItemCategoryObject.CompanyId));
			AddParameter(cmd, pGuid(RestMenuItemCategoryBase.Property_MenuId, restMenuItemCategoryObject.MenuId));
			AddParameter(cmd, pGuid(RestMenuItemCategoryBase.Property_CategoryId, restMenuItemCategoryObject.CategoryId));
			AddParameter(cmd, pGuid(RestMenuItemCategoryBase.Property_ItemId, restMenuItemCategoryObject.ItemId));
			AddParameter(cmd, pDateTime(RestMenuItemCategoryBase.Property_CreatedDate, restMenuItemCategoryObject.CreatedDate));
			AddParameter(cmd, pGuid(RestMenuItemCategoryBase.Property_CreatedBy, restMenuItemCategoryObject.CreatedBy));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts RestMenuItemCategory
        /// </summary>
        /// <param name="restMenuItemCategoryObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(RestMenuItemCategoryBase restMenuItemCategoryObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTRESTMENUITEMCATEGORY);
	
				AddParameter(cmd, pInt32Out(RestMenuItemCategoryBase.Property_Id));
				AddCommonParams(cmd, restMenuItemCategoryObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					restMenuItemCategoryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					restMenuItemCategoryObject.Id = (Int32)GetOutParameter(cmd, RestMenuItemCategoryBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(restMenuItemCategoryObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates RestMenuItemCategory
        /// </summary>
        /// <param name="restMenuItemCategoryObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(RestMenuItemCategoryBase restMenuItemCategoryObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATERESTMENUITEMCATEGORY);
				
				AddParameter(cmd, pInt32(RestMenuItemCategoryBase.Property_Id, restMenuItemCategoryObject.Id));
				AddCommonParams(cmd, restMenuItemCategoryObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					restMenuItemCategoryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(restMenuItemCategoryObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes RestMenuItemCategory
        /// </summary>
        /// <param name="Id">Id of the RestMenuItemCategory object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETERESTMENUITEMCATEGORY);	
				
				AddParameter(cmd, pInt32(RestMenuItemCategoryBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(RestMenuItemCategory), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves RestMenuItemCategory object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the RestMenuItemCategory object to retrieve</param>
        /// <returns>RestMenuItemCategory object, null if not found</returns>
		public RestMenuItemCategory Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTMENUITEMCATEGORYBYID))
			{
				AddParameter( cmd, pInt32(RestMenuItemCategoryBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all RestMenuItemCategory objects 
        /// </summary>
        /// <returns>A list of RestMenuItemCategory objects</returns>
		public RestMenuItemCategoryList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLRESTMENUITEMCATEGORY))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all RestMenuItemCategory objects by PageRequest
        /// </summary>
        /// <returns>A list of RestMenuItemCategory objects</returns>
		public RestMenuItemCategoryList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDRESTMENUITEMCATEGORY))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				RestMenuItemCategoryList _RestMenuItemCategoryList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _RestMenuItemCategoryList;
			}
		}
		
		/// <summary>
        /// Retrieves all RestMenuItemCategory objects by query String
        /// </summary>
        /// <returns>A list of RestMenuItemCategory objects</returns>
		public RestMenuItemCategoryList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTMENUITEMCATEGORYBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get RestMenuItemCategory Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of RestMenuItemCategory
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTMENUITEMCATEGORYMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get RestMenuItemCategory Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of RestMenuItemCategory
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _RestMenuItemCategoryRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTMENUITEMCATEGORYROWCOUNT))
			{
				SqlDataReader reader;
				_RestMenuItemCategoryRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _RestMenuItemCategoryRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills RestMenuItemCategory object
        /// </summary>
        /// <param name="restMenuItemCategoryObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(RestMenuItemCategoryBase restMenuItemCategoryObject, SqlDataReader reader, int start)
		{
			
				restMenuItemCategoryObject.Id = reader.GetInt32( start + 0 );			
				restMenuItemCategoryObject.CompanyId = reader.GetGuid( start + 1 );			
				restMenuItemCategoryObject.MenuId = reader.GetGuid( start + 2 );			
				restMenuItemCategoryObject.CategoryId = reader.GetGuid( start + 3 );			
				restMenuItemCategoryObject.ItemId = reader.GetGuid( start + 4 );			
				restMenuItemCategoryObject.CreatedDate = reader.GetDateTime( start + 5 );			
				restMenuItemCategoryObject.CreatedBy = reader.GetGuid( start + 6 );			
			FillBaseObject(restMenuItemCategoryObject, reader, (start + 7));

			
			restMenuItemCategoryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills RestMenuItemCategory object
        /// </summary>
        /// <param name="restMenuItemCategoryObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(RestMenuItemCategoryBase restMenuItemCategoryObject, SqlDataReader reader)
		{
			FillObject(restMenuItemCategoryObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves RestMenuItemCategory object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>RestMenuItemCategory object</returns>
		private RestMenuItemCategory GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					RestMenuItemCategory restMenuItemCategoryObject= new RestMenuItemCategory();
					FillObject(restMenuItemCategoryObject, reader);
					return restMenuItemCategoryObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of RestMenuItemCategory objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of RestMenuItemCategory objects</returns>
		private RestMenuItemCategoryList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//RestMenuItemCategory list
			RestMenuItemCategoryList list = new RestMenuItemCategoryList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					RestMenuItemCategory restMenuItemCategoryObject = new RestMenuItemCategory();
					FillObject(restMenuItemCategoryObject, reader);

					list.Add(restMenuItemCategoryObject);
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
