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
	public partial class RestToppingCategoryDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTRESTTOPPINGCATEGORY = "InsertRestToppingCategory";
		private const string UPDATERESTTOPPINGCATEGORY = "UpdateRestToppingCategory";
		private const string DELETERESTTOPPINGCATEGORY = "DeleteRestToppingCategory";
		private const string GETRESTTOPPINGCATEGORYBYID = "GetRestToppingCategoryById";
		private const string GETALLRESTTOPPINGCATEGORY = "GetAllRestToppingCategory";
		private const string GETPAGEDRESTTOPPINGCATEGORY = "GetPagedRestToppingCategory";
		private const string GETRESTTOPPINGCATEGORYMAXIMUMID = "GetRestToppingCategoryMaximumId";
		private const string GETRESTTOPPINGCATEGORYROWCOUNT = "GetRestToppingCategoryRowCount";	
		private const string GETRESTTOPPINGCATEGORYBYQUERY = "GetRestToppingCategoryByQuery";
		#endregion
		
		#region Constructors
		public RestToppingCategoryDataAccess(ClientContext context) : base(context) { }
		public RestToppingCategoryDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="restToppingCategoryObject"></param>
		private void AddCommonParams(SqlCommand cmd, RestToppingCategoryBase restToppingCategoryObject)
		{	
			AddParameter(cmd, pGuid(RestToppingCategoryBase.Property_ToppingCategoryId, restToppingCategoryObject.ToppingCategoryId));
			AddParameter(cmd, pNVarChar(RestToppingCategoryBase.Property_ToppingCategory, 250, restToppingCategoryObject.ToppingCategory));
			AddParameter(cmd, pGuid(RestToppingCategoryBase.Property_CompanyId, restToppingCategoryObject.CompanyId));
			AddParameter(cmd, pInt32(RestToppingCategoryBase.Property_RequiredItem, restToppingCategoryObject.RequiredItem));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts RestToppingCategory
        /// </summary>
        /// <param name="restToppingCategoryObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(RestToppingCategoryBase restToppingCategoryObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTRESTTOPPINGCATEGORY);
	
				AddParameter(cmd, pInt32Out(RestToppingCategoryBase.Property_Id));
				AddCommonParams(cmd, restToppingCategoryObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					restToppingCategoryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					restToppingCategoryObject.Id = (Int32)GetOutParameter(cmd, RestToppingCategoryBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(restToppingCategoryObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates RestToppingCategory
        /// </summary>
        /// <param name="restToppingCategoryObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(RestToppingCategoryBase restToppingCategoryObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATERESTTOPPINGCATEGORY);
				
				AddParameter(cmd, pInt32(RestToppingCategoryBase.Property_Id, restToppingCategoryObject.Id));
				AddCommonParams(cmd, restToppingCategoryObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					restToppingCategoryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(restToppingCategoryObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes RestToppingCategory
        /// </summary>
        /// <param name="Id">Id of the RestToppingCategory object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETERESTTOPPINGCATEGORY);	
				
				AddParameter(cmd, pInt32(RestToppingCategoryBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(RestToppingCategory), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves RestToppingCategory object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the RestToppingCategory object to retrieve</param>
        /// <returns>RestToppingCategory object, null if not found</returns>
		public RestToppingCategory Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTTOPPINGCATEGORYBYID))
			{
				AddParameter( cmd, pInt32(RestToppingCategoryBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all RestToppingCategory objects 
        /// </summary>
        /// <returns>A list of RestToppingCategory objects</returns>
		public RestToppingCategoryList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLRESTTOPPINGCATEGORY))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all RestToppingCategory objects by PageRequest
        /// </summary>
        /// <returns>A list of RestToppingCategory objects</returns>
		public RestToppingCategoryList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDRESTTOPPINGCATEGORY))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				RestToppingCategoryList _RestToppingCategoryList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _RestToppingCategoryList;
			}
		}
		
		/// <summary>
        /// Retrieves all RestToppingCategory objects by query String
        /// </summary>
        /// <returns>A list of RestToppingCategory objects</returns>
		public RestToppingCategoryList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTTOPPINGCATEGORYBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get RestToppingCategory Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of RestToppingCategory
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTTOPPINGCATEGORYMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get RestToppingCategory Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of RestToppingCategory
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _RestToppingCategoryRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTTOPPINGCATEGORYROWCOUNT))
			{
				SqlDataReader reader;
				_RestToppingCategoryRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _RestToppingCategoryRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills RestToppingCategory object
        /// </summary>
        /// <param name="restToppingCategoryObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(RestToppingCategoryBase restToppingCategoryObject, SqlDataReader reader, int start)
		{
			
				restToppingCategoryObject.Id = reader.GetInt32( start + 0 );			
				restToppingCategoryObject.ToppingCategoryId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) restToppingCategoryObject.ToppingCategory = reader.GetString( start + 2 );			
				restToppingCategoryObject.CompanyId = reader.GetGuid( start + 3 );			
				if(!reader.IsDBNull(4)) restToppingCategoryObject.RequiredItem = reader.GetInt32( start + 4 );			
			FillBaseObject(restToppingCategoryObject, reader, (start + 5));

			
			restToppingCategoryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills RestToppingCategory object
        /// </summary>
        /// <param name="restToppingCategoryObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(RestToppingCategoryBase restToppingCategoryObject, SqlDataReader reader)
		{
			FillObject(restToppingCategoryObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves RestToppingCategory object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>RestToppingCategory object</returns>
		private RestToppingCategory GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					RestToppingCategory restToppingCategoryObject= new RestToppingCategory();
					FillObject(restToppingCategoryObject, reader);
					return restToppingCategoryObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of RestToppingCategory objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of RestToppingCategory objects</returns>
		private RestToppingCategoryList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//RestToppingCategory list
			RestToppingCategoryList list = new RestToppingCategoryList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					RestToppingCategory restToppingCategoryObject = new RestToppingCategory();
					FillObject(restToppingCategoryObject, reader);

					list.Add(restToppingCategoryObject);
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
