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
	public partial class RestMenuCategoryDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTRESTMENUCATEGORY = "InsertRestMenuCategory";
		private const string UPDATERESTMENUCATEGORY = "UpdateRestMenuCategory";
		private const string DELETERESTMENUCATEGORY = "DeleteRestMenuCategory";
		private const string GETRESTMENUCATEGORYBYID = "GetRestMenuCategoryById";
		private const string GETALLRESTMENUCATEGORY = "GetAllRestMenuCategory";
		private const string GETPAGEDRESTMENUCATEGORY = "GetPagedRestMenuCategory";
		private const string GETRESTMENUCATEGORYMAXIMUMID = "GetRestMenuCategoryMaximumId";
		private const string GETRESTMENUCATEGORYROWCOUNT = "GetRestMenuCategoryRowCount";	
		private const string GETRESTMENUCATEGORYBYQUERY = "GetRestMenuCategoryByQuery";
		#endregion
		
		#region Constructors
		public RestMenuCategoryDataAccess(ClientContext context) : base(context) { }
		public RestMenuCategoryDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="restMenuCategoryObject"></param>
		private void AddCommonParams(SqlCommand cmd, RestMenuCategoryBase restMenuCategoryObject)
		{	
			AddParameter(cmd, pGuid(RestMenuCategoryBase.Property_CompanyId, restMenuCategoryObject.CompanyId));
			AddParameter(cmd, pGuid(RestMenuCategoryBase.Property_MenuId, restMenuCategoryObject.MenuId));
			AddParameter(cmd, pGuid(RestMenuCategoryBase.Property_CategoryId, restMenuCategoryObject.CategoryId));
			AddParameter(cmd, pDateTime(RestMenuCategoryBase.Property_CreatedDate, restMenuCategoryObject.CreatedDate));
			AddParameter(cmd, pGuid(RestMenuCategoryBase.Property_CreatedBy, restMenuCategoryObject.CreatedBy));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts RestMenuCategory
        /// </summary>
        /// <param name="restMenuCategoryObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(RestMenuCategoryBase restMenuCategoryObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTRESTMENUCATEGORY);
	
				AddParameter(cmd, pInt32Out(RestMenuCategoryBase.Property_Id));
				AddCommonParams(cmd, restMenuCategoryObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					restMenuCategoryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					restMenuCategoryObject.Id = (Int32)GetOutParameter(cmd, RestMenuCategoryBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(restMenuCategoryObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates RestMenuCategory
        /// </summary>
        /// <param name="restMenuCategoryObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(RestMenuCategoryBase restMenuCategoryObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATERESTMENUCATEGORY);
				
				AddParameter(cmd, pInt32(RestMenuCategoryBase.Property_Id, restMenuCategoryObject.Id));
				AddCommonParams(cmd, restMenuCategoryObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					restMenuCategoryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(restMenuCategoryObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes RestMenuCategory
        /// </summary>
        /// <param name="Id">Id of the RestMenuCategory object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETERESTMENUCATEGORY);	
				
				AddParameter(cmd, pInt32(RestMenuCategoryBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(RestMenuCategory), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves RestMenuCategory object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the RestMenuCategory object to retrieve</param>
        /// <returns>RestMenuCategory object, null if not found</returns>
		public RestMenuCategory Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTMENUCATEGORYBYID))
			{
				AddParameter( cmd, pInt32(RestMenuCategoryBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all RestMenuCategory objects 
        /// </summary>
        /// <returns>A list of RestMenuCategory objects</returns>
		public RestMenuCategoryList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLRESTMENUCATEGORY))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all RestMenuCategory objects by PageRequest
        /// </summary>
        /// <returns>A list of RestMenuCategory objects</returns>
		public RestMenuCategoryList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDRESTMENUCATEGORY))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				RestMenuCategoryList _RestMenuCategoryList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _RestMenuCategoryList;
			}
		}
		
		/// <summary>
        /// Retrieves all RestMenuCategory objects by query String
        /// </summary>
        /// <returns>A list of RestMenuCategory objects</returns>
		public RestMenuCategoryList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTMENUCATEGORYBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get RestMenuCategory Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of RestMenuCategory
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTMENUCATEGORYMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get RestMenuCategory Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of RestMenuCategory
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _RestMenuCategoryRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTMENUCATEGORYROWCOUNT))
			{
				SqlDataReader reader;
				_RestMenuCategoryRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _RestMenuCategoryRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills RestMenuCategory object
        /// </summary>
        /// <param name="restMenuCategoryObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(RestMenuCategoryBase restMenuCategoryObject, SqlDataReader reader, int start)
		{
			
				restMenuCategoryObject.Id = reader.GetInt32( start + 0 );			
				restMenuCategoryObject.CompanyId = reader.GetGuid( start + 1 );			
				restMenuCategoryObject.MenuId = reader.GetGuid( start + 2 );			
				restMenuCategoryObject.CategoryId = reader.GetGuid( start + 3 );			
				restMenuCategoryObject.CreatedDate = reader.GetDateTime( start + 4 );			
				restMenuCategoryObject.CreatedBy = reader.GetGuid( start + 5 );			
			FillBaseObject(restMenuCategoryObject, reader, (start + 6));

			
			restMenuCategoryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills RestMenuCategory object
        /// </summary>
        /// <param name="restMenuCategoryObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(RestMenuCategoryBase restMenuCategoryObject, SqlDataReader reader)
		{
			FillObject(restMenuCategoryObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves RestMenuCategory object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>RestMenuCategory object</returns>
		private RestMenuCategory GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					RestMenuCategory restMenuCategoryObject= new RestMenuCategory();
					FillObject(restMenuCategoryObject, reader);
					return restMenuCategoryObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of RestMenuCategory objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of RestMenuCategory objects</returns>
		private RestMenuCategoryList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//RestMenuCategory list
			RestMenuCategoryList list = new RestMenuCategoryList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					RestMenuCategory restMenuCategoryObject = new RestMenuCategory();
					FillObject(restMenuCategoryObject, reader);

					list.Add(restMenuCategoryObject);
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
