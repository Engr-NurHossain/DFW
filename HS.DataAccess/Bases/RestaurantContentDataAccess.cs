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
	public partial class RestaurantContentDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTRESTAURANTCONTENT = "InsertRestaurantContent";
		private const string UPDATERESTAURANTCONTENT = "UpdateRestaurantContent";
		private const string DELETERESTAURANTCONTENT = "DeleteRestaurantContent";
		private const string GETRESTAURANTCONTENTBYID = "GetRestaurantContentById";
		private const string GETALLRESTAURANTCONTENT = "GetAllRestaurantContent";
		private const string GETPAGEDRESTAURANTCONTENT = "GetPagedRestaurantContent";
		private const string GETRESTAURANTCONTENTMAXIMUMID = "GetRestaurantContentMaximumId";
		private const string GETRESTAURANTCONTENTROWCOUNT = "GetRestaurantContentRowCount";	
		private const string GETRESTAURANTCONTENTBYQUERY = "GetRestaurantContentByQuery";
		#endregion
		
		#region Constructors
		public RestaurantContentDataAccess(ClientContext context) : base(context) { }
		public RestaurantContentDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="restaurantContentObject"></param>
		private void AddCommonParams(SqlCommand cmd, RestaurantContentBase restaurantContentObject)
		{	
			AddParameter(cmd, pGuid(RestaurantContentBase.Property_CompanyId, restaurantContentObject.CompanyId));
			AddParameter(cmd, pNVarChar(RestaurantContentBase.Property_PageName, 100, restaurantContentObject.PageName));
			AddParameter(cmd, pNVarChar(RestaurantContentBase.Property_AnchorText, 100, restaurantContentObject.AnchorText));
			AddParameter(cmd, pNVarChar(RestaurantContentBase.Property_MetaTitle, 100, restaurantContentObject.MetaTitle));
			AddParameter(cmd, pNVarChar(RestaurantContentBase.Property_MetaDescription, restaurantContentObject.MetaDescription));
			AddParameter(cmd, pBool(RestaurantContentBase.Property_IsPublish, restaurantContentObject.IsPublish));
			AddParameter(cmd, pBool(RestaurantContentBase.Property_IsNavigation, restaurantContentObject.IsNavigation));
			AddParameter(cmd, pNVarChar(RestaurantContentBase.Property_FolderName, 100, restaurantContentObject.FolderName));
			AddParameter(cmd, pNVarChar(RestaurantContentBase.Property_ContentURL, restaurantContentObject.ContentURL));
			AddParameter(cmd, pGuid(RestaurantContentBase.Property_CreatedBy, restaurantContentObject.CreatedBy));
			AddParameter(cmd, pDateTime(RestaurantContentBase.Property_CreatedDate, restaurantContentObject.CreatedDate));
			AddParameter(cmd, pGuid(RestaurantContentBase.Property_LastUpdatedBy, restaurantContentObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(RestaurantContentBase.Property_LastUpdatedDate, restaurantContentObject.LastUpdatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts RestaurantContent
        /// </summary>
        /// <param name="restaurantContentObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(RestaurantContentBase restaurantContentObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTRESTAURANTCONTENT);
	
				AddParameter(cmd, pInt32Out(RestaurantContentBase.Property_Id));
				AddCommonParams(cmd, restaurantContentObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					restaurantContentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					restaurantContentObject.Id = (Int32)GetOutParameter(cmd, RestaurantContentBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(restaurantContentObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates RestaurantContent
        /// </summary>
        /// <param name="restaurantContentObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(RestaurantContentBase restaurantContentObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATERESTAURANTCONTENT);
				
				AddParameter(cmd, pInt32(RestaurantContentBase.Property_Id, restaurantContentObject.Id));
				AddCommonParams(cmd, restaurantContentObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					restaurantContentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(restaurantContentObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes RestaurantContent
        /// </summary>
        /// <param name="Id">Id of the RestaurantContent object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETERESTAURANTCONTENT);	
				
				AddParameter(cmd, pInt32(RestaurantContentBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(RestaurantContent), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves RestaurantContent object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the RestaurantContent object to retrieve</param>
        /// <returns>RestaurantContent object, null if not found</returns>
		public RestaurantContent Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTAURANTCONTENTBYID))
			{
				AddParameter( cmd, pInt32(RestaurantContentBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all RestaurantContent objects 
        /// </summary>
        /// <returns>A list of RestaurantContent objects</returns>
		public RestaurantContentList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLRESTAURANTCONTENT))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all RestaurantContent objects by PageRequest
        /// </summary>
        /// <returns>A list of RestaurantContent objects</returns>
		public RestaurantContentList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDRESTAURANTCONTENT))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				RestaurantContentList _RestaurantContentList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _RestaurantContentList;
			}
		}
		
		/// <summary>
        /// Retrieves all RestaurantContent objects by query String
        /// </summary>
        /// <returns>A list of RestaurantContent objects</returns>
		public RestaurantContentList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTAURANTCONTENTBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get RestaurantContent Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of RestaurantContent
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTAURANTCONTENTMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get RestaurantContent Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of RestaurantContent
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _RestaurantContentRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTAURANTCONTENTROWCOUNT))
			{
				SqlDataReader reader;
				_RestaurantContentRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _RestaurantContentRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills RestaurantContent object
        /// </summary>
        /// <param name="restaurantContentObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(RestaurantContentBase restaurantContentObject, SqlDataReader reader, int start)
		{
			
				restaurantContentObject.Id = reader.GetInt32( start + 0 );			
				restaurantContentObject.CompanyId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) restaurantContentObject.PageName = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) restaurantContentObject.AnchorText = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) restaurantContentObject.MetaTitle = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) restaurantContentObject.MetaDescription = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) restaurantContentObject.IsPublish = reader.GetBoolean( start + 6 );			
				if(!reader.IsDBNull(7)) restaurantContentObject.IsNavigation = reader.GetBoolean( start + 7 );			
				if(!reader.IsDBNull(8)) restaurantContentObject.FolderName = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) restaurantContentObject.ContentURL = reader.GetString( start + 9 );			
				restaurantContentObject.CreatedBy = reader.GetGuid( start + 10 );			
				restaurantContentObject.CreatedDate = reader.GetDateTime( start + 11 );			
				restaurantContentObject.LastUpdatedBy = reader.GetGuid( start + 12 );			
				restaurantContentObject.LastUpdatedDate = reader.GetDateTime( start + 13 );			
			FillBaseObject(restaurantContentObject, reader, (start + 14));

			
			restaurantContentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills RestaurantContent object
        /// </summary>
        /// <param name="restaurantContentObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(RestaurantContentBase restaurantContentObject, SqlDataReader reader)
		{
			FillObject(restaurantContentObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves RestaurantContent object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>RestaurantContent object</returns>
		private RestaurantContent GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					RestaurantContent restaurantContentObject= new RestaurantContent();
					FillObject(restaurantContentObject, reader);
					return restaurantContentObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of RestaurantContent objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of RestaurantContent objects</returns>
		private RestaurantContentList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//RestaurantContent list
			RestaurantContentList list = new RestaurantContentList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					RestaurantContent restaurantContentObject = new RestaurantContent();
					FillObject(restaurantContentObject, reader);

					list.Add(restaurantContentObject);
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
