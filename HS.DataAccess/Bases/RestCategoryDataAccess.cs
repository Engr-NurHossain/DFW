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
	public partial class RestCategoryDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTRESTCATEGORY = "InsertRestCategory";
		private const string UPDATERESTCATEGORY = "UpdateRestCategory";
		private const string DELETERESTCATEGORY = "DeleteRestCategory";
		private const string GETRESTCATEGORYBYID = "GetRestCategoryById";
		private const string GETALLRESTCATEGORY = "GetAllRestCategory";
		private const string GETPAGEDRESTCATEGORY = "GetPagedRestCategory";
		private const string GETRESTCATEGORYMAXIMUMID = "GetRestCategoryMaximumId";
		private const string GETRESTCATEGORYROWCOUNT = "GetRestCategoryRowCount";	
		private const string GETRESTCATEGORYBYQUERY = "GetRestCategoryByQuery";
		#endregion
		
		#region Constructors
		public RestCategoryDataAccess(ClientContext context) : base(context) { }
		public RestCategoryDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="restCategoryObject"></param>
		private void AddCommonParams(SqlCommand cmd, RestCategoryBase restCategoryObject)
		{	
			AddParameter(cmd, pGuid(RestCategoryBase.Property_CategoryId, restCategoryObject.CategoryId));
			AddParameter(cmd, pNVarChar(RestCategoryBase.Property_CategoryName, 100, restCategoryObject.CategoryName));
			AddParameter(cmd, pNVarChar(RestCategoryBase.Property_Description, restCategoryObject.Description));
			AddParameter(cmd, pNVarChar(RestCategoryBase.Property_DaysAvailable, 250, restCategoryObject.DaysAvailable));
			AddParameter(cmd, pNVarChar(RestCategoryBase.Property_TimeAvailable, 250, restCategoryObject.TimeAvailable));
			AddParameter(cmd, pBool(RestCategoryBase.Property_Status, restCategoryObject.Status));
			AddParameter(cmd, pNVarChar(RestCategoryBase.Property_Image, restCategoryObject.Image));
			AddParameter(cmd, pDateTime(RestCategoryBase.Property_CreatedDate, restCategoryObject.CreatedDate));
			AddParameter(cmd, pGuid(RestCategoryBase.Property_CreatedBy, restCategoryObject.CreatedBy));
			AddParameter(cmd, pGuid(RestCategoryBase.Property_LastUpdatedBy, restCategoryObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(RestCategoryBase.Property_LastUpdatedDate, restCategoryObject.LastUpdatedDate));
			AddParameter(cmd, pGuid(RestCategoryBase.Property_CompanyId, restCategoryObject.CompanyId));
			AddParameter(cmd, pNVarChar(RestCategoryBase.Property_UrlSlug, 150, restCategoryObject.UrlSlug));
			AddParameter(cmd, pNVarChar(RestCategoryBase.Property_DaysAvailableOption, 50, restCategoryObject.DaysAvailableOption));
			AddParameter(cmd, pNVarChar(RestCategoryBase.Property_TimeAvailableOption, 50, restCategoryObject.TimeAvailableOption));
			AddParameter(cmd, pNVarChar(RestCategoryBase.Property_WebsiteURL, restCategoryObject.WebsiteURL));
			AddParameter(cmd, pNVarChar(RestCategoryBase.Property_MetaTitle, restCategoryObject.MetaTitle));
			AddParameter(cmd, pNVarChar(RestCategoryBase.Property_MetaDescription, restCategoryObject.MetaDescription));
			AddParameter(cmd, pInt32(RestCategoryBase.Property_OrderBy, restCategoryObject.OrderBy));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts RestCategory
        /// </summary>
        /// <param name="restCategoryObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(RestCategoryBase restCategoryObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTRESTCATEGORY);
	
				AddParameter(cmd, pInt32Out(RestCategoryBase.Property_Id));
				AddCommonParams(cmd, restCategoryObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					restCategoryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					restCategoryObject.Id = (Int32)GetOutParameter(cmd, RestCategoryBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(restCategoryObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates RestCategory
        /// </summary>
        /// <param name="restCategoryObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(RestCategoryBase restCategoryObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATERESTCATEGORY);
				
				AddParameter(cmd, pInt32(RestCategoryBase.Property_Id, restCategoryObject.Id));
				AddCommonParams(cmd, restCategoryObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					restCategoryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(restCategoryObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes RestCategory
        /// </summary>
        /// <param name="Id">Id of the RestCategory object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETERESTCATEGORY);	
				
				AddParameter(cmd, pInt32(RestCategoryBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(RestCategory), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves RestCategory object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the RestCategory object to retrieve</param>
        /// <returns>RestCategory object, null if not found</returns>
		public RestCategory Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTCATEGORYBYID))
			{
				AddParameter( cmd, pInt32(RestCategoryBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all RestCategory objects 
        /// </summary>
        /// <returns>A list of RestCategory objects</returns>
		public RestCategoryList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLRESTCATEGORY))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all RestCategory objects by PageRequest
        /// </summary>
        /// <returns>A list of RestCategory objects</returns>
		public RestCategoryList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDRESTCATEGORY))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				RestCategoryList _RestCategoryList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _RestCategoryList;
			}
		}
		
		/// <summary>
        /// Retrieves all RestCategory objects by query String
        /// </summary>
        /// <returns>A list of RestCategory objects</returns>
		public RestCategoryList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTCATEGORYBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get RestCategory Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of RestCategory
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTCATEGORYMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get RestCategory Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of RestCategory
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _RestCategoryRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTCATEGORYROWCOUNT))
			{
				SqlDataReader reader;
				_RestCategoryRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _RestCategoryRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills RestCategory object
        /// </summary>
        /// <param name="restCategoryObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(RestCategoryBase restCategoryObject, SqlDataReader reader, int start)
		{
			
				restCategoryObject.Id = reader.GetInt32( start + 0 );			
				restCategoryObject.CategoryId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) restCategoryObject.CategoryName = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) restCategoryObject.Description = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) restCategoryObject.DaysAvailable = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) restCategoryObject.TimeAvailable = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) restCategoryObject.Status = reader.GetBoolean( start + 6 );			
				if(!reader.IsDBNull(7)) restCategoryObject.Image = reader.GetString( start + 7 );			
				restCategoryObject.CreatedDate = reader.GetDateTime( start + 8 );			
				restCategoryObject.CreatedBy = reader.GetGuid( start + 9 );			
				restCategoryObject.LastUpdatedBy = reader.GetGuid( start + 10 );			
				restCategoryObject.LastUpdatedDate = reader.GetDateTime( start + 11 );			
				restCategoryObject.CompanyId = reader.GetGuid( start + 12 );			
				if(!reader.IsDBNull(13)) restCategoryObject.UrlSlug = reader.GetString( start + 13 );			
				if(!reader.IsDBNull(14)) restCategoryObject.DaysAvailableOption = reader.GetString( start + 14 );			
				if(!reader.IsDBNull(15)) restCategoryObject.TimeAvailableOption = reader.GetString( start + 15 );			
				if(!reader.IsDBNull(16)) restCategoryObject.WebsiteURL = reader.GetString( start + 16 );			
				if(!reader.IsDBNull(17)) restCategoryObject.MetaTitle = reader.GetString( start + 17 );			
				if(!reader.IsDBNull(18)) restCategoryObject.MetaDescription = reader.GetString( start + 18 );			
				if(!reader.IsDBNull(19)) restCategoryObject.OrderBy = reader.GetInt32( start + 19 );			
			FillBaseObject(restCategoryObject, reader, (start + 20));

			
			restCategoryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills RestCategory object
        /// </summary>
        /// <param name="restCategoryObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(RestCategoryBase restCategoryObject, SqlDataReader reader)
		{
			FillObject(restCategoryObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves RestCategory object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>RestCategory object</returns>
		private RestCategory GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					RestCategory restCategoryObject= new RestCategory();
					FillObject(restCategoryObject, reader);
					return restCategoryObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of RestCategory objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of RestCategory objects</returns>
		private RestCategoryList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//RestCategory list
			RestCategoryList list = new RestCategoryList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					RestCategory restCategoryObject = new RestCategory();
					FillObject(restCategoryObject, reader);

					list.Add(restCategoryObject);
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
