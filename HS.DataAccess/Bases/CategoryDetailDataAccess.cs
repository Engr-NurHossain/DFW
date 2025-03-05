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
	public partial class CategoryDetailDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCATEGORYDETAIL = "InsertCategoryDetail";
		private const string UPDATECATEGORYDETAIL = "UpdateCategoryDetail";
		private const string DELETECATEGORYDETAIL = "DeleteCategoryDetail";
		private const string GETCATEGORYDETAILBYID = "GetCategoryDetailById";
		private const string GETALLCATEGORYDETAIL = "GetAllCategoryDetail";
		private const string GETPAGEDCATEGORYDETAIL = "GetPagedCategoryDetail";
		private const string GETCATEGORYDETAILMAXIMUMID = "GetCategoryDetailMaximumId";
		private const string GETCATEGORYDETAILROWCOUNT = "GetCategoryDetailRowCount";	
		private const string GETCATEGORYDETAILBYQUERY = "GetCategoryDetailByQuery";
		#endregion
		
		#region Constructors
		public CategoryDetailDataAccess(ClientContext context) : base(context) { }
		public CategoryDetailDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="categoryDetailObject"></param>
		private void AddCommonParams(SqlCommand cmd, CategoryDetailBase categoryDetailObject)
		{	
			AddParameter(cmd, pInt32(CategoryDetailBase.Property_MenuId, categoryDetailObject.MenuId));
			AddParameter(cmd, pInt32(CategoryDetailBase.Property_CategoryId, categoryDetailObject.CategoryId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CategoryDetail
        /// </summary>
        /// <param name="categoryDetailObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CategoryDetailBase categoryDetailObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCATEGORYDETAIL);
	
				AddParameter(cmd, pInt32Out(CategoryDetailBase.Property_Id));
				AddCommonParams(cmd, categoryDetailObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					categoryDetailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					categoryDetailObject.Id = (Int32)GetOutParameter(cmd, CategoryDetailBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(categoryDetailObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CategoryDetail
        /// </summary>
        /// <param name="categoryDetailObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CategoryDetailBase categoryDetailObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECATEGORYDETAIL);
				
				AddParameter(cmd, pInt32(CategoryDetailBase.Property_Id, categoryDetailObject.Id));
				AddCommonParams(cmd, categoryDetailObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					categoryDetailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(categoryDetailObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CategoryDetail
        /// </summary>
        /// <param name="Id">Id of the CategoryDetail object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECATEGORYDETAIL);	
				
				AddParameter(cmd, pInt32(CategoryDetailBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CategoryDetail), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CategoryDetail object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CategoryDetail object to retrieve</param>
        /// <returns>CategoryDetail object, null if not found</returns>
		public CategoryDetail Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCATEGORYDETAILBYID))
			{
				AddParameter( cmd, pInt32(CategoryDetailBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CategoryDetail objects 
        /// </summary>
        /// <returns>A list of CategoryDetail objects</returns>
		public CategoryDetailList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCATEGORYDETAIL))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CategoryDetail objects by PageRequest
        /// </summary>
        /// <returns>A list of CategoryDetail objects</returns>
		public CategoryDetailList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCATEGORYDETAIL))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CategoryDetailList _CategoryDetailList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CategoryDetailList;
			}
		}
		
		/// <summary>
        /// Retrieves all CategoryDetail objects by query String
        /// </summary>
        /// <returns>A list of CategoryDetail objects</returns>
		public CategoryDetailList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCATEGORYDETAILBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CategoryDetail Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CategoryDetail
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCATEGORYDETAILMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CategoryDetail Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CategoryDetail
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CategoryDetailRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCATEGORYDETAILROWCOUNT))
			{
				SqlDataReader reader;
				_CategoryDetailRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CategoryDetailRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CategoryDetail object
        /// </summary>
        /// <param name="categoryDetailObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CategoryDetailBase categoryDetailObject, SqlDataReader reader, int start)
		{
			
				categoryDetailObject.Id = reader.GetInt32( start + 0 );			
				if(!reader.IsDBNull(1)) categoryDetailObject.MenuId = reader.GetInt32( start + 1 );			
				if(!reader.IsDBNull(2)) categoryDetailObject.CategoryId = reader.GetInt32( start + 2 );			
			FillBaseObject(categoryDetailObject, reader, (start + 3));

			
			categoryDetailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CategoryDetail object
        /// </summary>
        /// <param name="categoryDetailObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CategoryDetailBase categoryDetailObject, SqlDataReader reader)
		{
			FillObject(categoryDetailObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CategoryDetail object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CategoryDetail object</returns>
		private CategoryDetail GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CategoryDetail categoryDetailObject= new CategoryDetail();
					FillObject(categoryDetailObject, reader);
					return categoryDetailObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CategoryDetail objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CategoryDetail objects</returns>
		private CategoryDetailList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CategoryDetail list
			CategoryDetailList list = new CategoryDetailList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CategoryDetail categoryDetailObject = new CategoryDetail();
					FillObject(categoryDetailObject, reader);

					list.Add(categoryDetailObject);
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
