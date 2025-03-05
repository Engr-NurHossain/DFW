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
	public partial class CategoryDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCATEGORY = "InsertCategory";
		private const string UPDATECATEGORY = "UpdateCategory";
		private const string DELETECATEGORY = "DeleteCategory";
		private const string GETCATEGORYBYID = "GetCategoryById";
		private const string GETALLCATEGORY = "GetAllCategory";
		private const string GETPAGEDCATEGORY = "GetPagedCategory";
		private const string GETCATEGORYMAXIMUMID = "GetCategoryMaximumId";
		private const string GETCATEGORYROWCOUNT = "GetCategoryRowCount";	
		private const string GETCATEGORYBYQUERY = "GetCategoryByQuery";
		#endregion
		
		#region Constructors
		public CategoryDataAccess(ClientContext context) : base(context) { }
		public CategoryDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="categoryObject"></param>
		private void AddCommonParams(SqlCommand cmd, CategoryBase categoryObject)
		{	
			AddParameter(cmd, pGuid(CategoryBase.Property_CategoryId, categoryObject.CategoryId));
			AddParameter(cmd, pNVarChar(CategoryBase.Property_CategoryName, 100, categoryObject.CategoryName));
			AddParameter(cmd, pNVarChar(CategoryBase.Property_Description, categoryObject.Description));
			AddParameter(cmd, pNVarChar(CategoryBase.Property_DaysAvailable, 250, categoryObject.DaysAvailable));
			AddParameter(cmd, pNVarChar(CategoryBase.Property_TimeAvailable, 250, categoryObject.TimeAvailable));
			AddParameter(cmd, pBool(CategoryBase.Property_Status, categoryObject.Status));
			AddParameter(cmd, pNVarChar(CategoryBase.Property_Image, categoryObject.Image));
			AddParameter(cmd, pDateTime(CategoryBase.Property_CreatedDate, categoryObject.CreatedDate));
			AddParameter(cmd, pGuid(CategoryBase.Property_CreatedBy, categoryObject.CreatedBy));
			AddParameter(cmd, pGuid(CategoryBase.Property_LastUpdatedBy, categoryObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(CategoryBase.Property_LastUpdatedDate, categoryObject.LastUpdatedDate));
			AddParameter(cmd, pGuid(CategoryBase.Property_CompanyId, categoryObject.CompanyId));
			AddParameter(cmd, pNVarChar(CategoryBase.Property_UrlSlug, 150, categoryObject.UrlSlug));
			AddParameter(cmd, pNVarChar(CategoryBase.Property_DaysAvailableOption, 50, categoryObject.DaysAvailableOption));
			AddParameter(cmd, pNVarChar(CategoryBase.Property_TimeAvailableOption, 50, categoryObject.TimeAvailableOption));
			AddParameter(cmd, pNVarChar(CategoryBase.Property_WebsiteURL, categoryObject.WebsiteURL));
			AddParameter(cmd, pNVarChar(CategoryBase.Property_MetaTitle, categoryObject.MetaTitle));
			AddParameter(cmd, pNVarChar(CategoryBase.Property_MetaDescription, categoryObject.MetaDescription));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts Category
        /// </summary>
        /// <param name="categoryObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CategoryBase categoryObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCATEGORY);
	
				AddParameter(cmd, pInt32Out(CategoryBase.Property_Id));
				AddCommonParams(cmd, categoryObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					categoryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					categoryObject.Id = (Int32)GetOutParameter(cmd, CategoryBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(categoryObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates Category
        /// </summary>
        /// <param name="categoryObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CategoryBase categoryObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECATEGORY);
				
				AddParameter(cmd, pInt32(CategoryBase.Property_Id, categoryObject.Id));
				AddCommonParams(cmd, categoryObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					categoryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(categoryObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes Category
        /// </summary>
        /// <param name="Id">Id of the Category object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECATEGORY);	
				
				AddParameter(cmd, pInt32(CategoryBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(Category), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves Category object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the Category object to retrieve</param>
        /// <returns>Category object, null if not found</returns>
		public Category Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCATEGORYBYID))
			{
				AddParameter( cmd, pInt32(CategoryBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all Category objects 
        /// </summary>
        /// <returns>A list of Category objects</returns>
		public CategoryList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCATEGORY))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all Category objects by PageRequest
        /// </summary>
        /// <returns>A list of Category objects</returns>
		public CategoryList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCATEGORY))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CategoryList _CategoryList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CategoryList;
			}
		}
		
		/// <summary>
        /// Retrieves all Category objects by query String
        /// </summary>
        /// <returns>A list of Category objects</returns>
		public CategoryList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCATEGORYBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get Category Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of Category
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCATEGORYMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get Category Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of Category
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CategoryRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCATEGORYROWCOUNT))
			{
				SqlDataReader reader;
				_CategoryRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CategoryRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills Category object
        /// </summary>
        /// <param name="categoryObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CategoryBase categoryObject, SqlDataReader reader, int start)
		{
			
				categoryObject.Id = reader.GetInt32( start + 0 );			
				categoryObject.CategoryId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) categoryObject.CategoryName = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) categoryObject.Description = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) categoryObject.DaysAvailable = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) categoryObject.TimeAvailable = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) categoryObject.Status = reader.GetBoolean( start + 6 );			
				if(!reader.IsDBNull(7)) categoryObject.Image = reader.GetString( start + 7 );			
				categoryObject.CreatedDate = reader.GetDateTime( start + 8 );			
				categoryObject.CreatedBy = reader.GetGuid( start + 9 );			
				categoryObject.LastUpdatedBy = reader.GetGuid( start + 10 );			
				categoryObject.LastUpdatedDate = reader.GetDateTime( start + 11 );			
				categoryObject.CompanyId = reader.GetGuid( start + 12 );			
				if(!reader.IsDBNull(13)) categoryObject.UrlSlug = reader.GetString( start + 13 );			
				if(!reader.IsDBNull(14)) categoryObject.DaysAvailableOption = reader.GetString( start + 14 );			
				if(!reader.IsDBNull(15)) categoryObject.TimeAvailableOption = reader.GetString( start + 15 );			
				if(!reader.IsDBNull(16)) categoryObject.WebsiteURL = reader.GetString( start + 16 );			
				if(!reader.IsDBNull(17)) categoryObject.MetaTitle = reader.GetString( start + 17 );			
				if(!reader.IsDBNull(18)) categoryObject.MetaDescription = reader.GetString( start + 18 );			
			FillBaseObject(categoryObject, reader, (start + 19));

			
			categoryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills Category object
        /// </summary>
        /// <param name="categoryObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CategoryBase categoryObject, SqlDataReader reader)
		{
			FillObject(categoryObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves Category object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>Category object</returns>
		private Category GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					Category categoryObject= new Category();
					FillObject(categoryObject, reader);
					return categoryObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of Category objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of Category objects</returns>
		private CategoryList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//Category list
			CategoryList list = new CategoryList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					Category categoryObject = new Category();
					FillObject(categoryObject, reader);

					list.Add(categoryObject);
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
