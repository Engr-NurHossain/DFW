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
	public partial class RestaurantCouponsDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTRESTAURANTCOUPONS = "InsertRestaurantCoupons";
		private const string UPDATERESTAURANTCOUPONS = "UpdateRestaurantCoupons";
		private const string DELETERESTAURANTCOUPONS = "DeleteRestaurantCoupons";
		private const string GETRESTAURANTCOUPONSBYID = "GetRestaurantCouponsById";
		private const string GETALLRESTAURANTCOUPONS = "GetAllRestaurantCoupons";
		private const string GETPAGEDRESTAURANTCOUPONS = "GetPagedRestaurantCoupons";
		private const string GETRESTAURANTCOUPONSMAXIMUMID = "GetRestaurantCouponsMaximumId";
		private const string GETRESTAURANTCOUPONSROWCOUNT = "GetRestaurantCouponsRowCount";	
		private const string GETRESTAURANTCOUPONSBYQUERY = "GetRestaurantCouponsByQuery";
		#endregion
		
		#region Constructors
		public RestaurantCouponsDataAccess(ClientContext context) : base(context) { }
		public RestaurantCouponsDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="restaurantCouponsObject"></param>
		private void AddCommonParams(SqlCommand cmd, RestaurantCouponsBase restaurantCouponsObject)
		{	
			AddParameter(cmd, pGuid(RestaurantCouponsBase.Property_CompanyId, restaurantCouponsObject.CompanyId));
			AddParameter(cmd, pNVarChar(RestaurantCouponsBase.Property_CouponCode, restaurantCouponsObject.CouponCode));
			AddParameter(cmd, pDateTime(RestaurantCouponsBase.Property_StartDate, restaurantCouponsObject.StartDate));
			AddParameter(cmd, pDateTime(RestaurantCouponsBase.Property_EndDate, restaurantCouponsObject.EndDate));
			AddParameter(cmd, pNVarChar(RestaurantCouponsBase.Property_Discount, 250, restaurantCouponsObject.Discount));
			AddParameter(cmd, pNVarChar(RestaurantCouponsBase.Property_MinimumOrder, 250, restaurantCouponsObject.MinimumOrder));
			AddParameter(cmd, pNVarChar(RestaurantCouponsBase.Property_ReedemRequired, 250, restaurantCouponsObject.ReedemRequired));
			AddParameter(cmd, pDateTime(RestaurantCouponsBase.Property_CreatedDate, restaurantCouponsObject.CreatedDate));
			AddParameter(cmd, pDateTime(RestaurantCouponsBase.Property_LastUpdatedDate, restaurantCouponsObject.LastUpdatedDate));
			AddParameter(cmd, pGuid(RestaurantCouponsBase.Property_CreatedBy, restaurantCouponsObject.CreatedBy));
			AddParameter(cmd, pGuid(RestaurantCouponsBase.Property_LastUpdatedBy, restaurantCouponsObject.LastUpdatedBy));
			AddParameter(cmd, pNVarChar(RestaurantCouponsBase.Property_DiscountType, 150, restaurantCouponsObject.DiscountType));
			AddParameter(cmd, pBool(RestaurantCouponsBase.Property_Status, restaurantCouponsObject.Status));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts RestaurantCoupons
        /// </summary>
        /// <param name="restaurantCouponsObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(RestaurantCouponsBase restaurantCouponsObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTRESTAURANTCOUPONS);
	
				AddParameter(cmd, pInt32Out(RestaurantCouponsBase.Property_Id));
				AddCommonParams(cmd, restaurantCouponsObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					restaurantCouponsObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					restaurantCouponsObject.Id = (Int32)GetOutParameter(cmd, RestaurantCouponsBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(restaurantCouponsObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates RestaurantCoupons
        /// </summary>
        /// <param name="restaurantCouponsObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(RestaurantCouponsBase restaurantCouponsObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATERESTAURANTCOUPONS);
				
				AddParameter(cmd, pInt32(RestaurantCouponsBase.Property_Id, restaurantCouponsObject.Id));
				AddCommonParams(cmd, restaurantCouponsObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					restaurantCouponsObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(restaurantCouponsObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes RestaurantCoupons
        /// </summary>
        /// <param name="Id">Id of the RestaurantCoupons object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETERESTAURANTCOUPONS);	
				
				AddParameter(cmd, pInt32(RestaurantCouponsBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(RestaurantCoupons), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves RestaurantCoupons object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the RestaurantCoupons object to retrieve</param>
        /// <returns>RestaurantCoupons object, null if not found</returns>
		public RestaurantCoupons Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTAURANTCOUPONSBYID))
			{
				AddParameter( cmd, pInt32(RestaurantCouponsBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all RestaurantCoupons objects 
        /// </summary>
        /// <returns>A list of RestaurantCoupons objects</returns>
		public RestaurantCouponsList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLRESTAURANTCOUPONS))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all RestaurantCoupons objects by PageRequest
        /// </summary>
        /// <returns>A list of RestaurantCoupons objects</returns>
		public RestaurantCouponsList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDRESTAURANTCOUPONS))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				RestaurantCouponsList _RestaurantCouponsList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _RestaurantCouponsList;
			}
		}
		
		/// <summary>
        /// Retrieves all RestaurantCoupons objects by query String
        /// </summary>
        /// <returns>A list of RestaurantCoupons objects</returns>
		public RestaurantCouponsList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTAURANTCOUPONSBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get RestaurantCoupons Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of RestaurantCoupons
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTAURANTCOUPONSMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get RestaurantCoupons Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of RestaurantCoupons
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _RestaurantCouponsRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTAURANTCOUPONSROWCOUNT))
			{
				SqlDataReader reader;
				_RestaurantCouponsRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _RestaurantCouponsRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills RestaurantCoupons object
        /// </summary>
        /// <param name="restaurantCouponsObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(RestaurantCouponsBase restaurantCouponsObject, SqlDataReader reader, int start)
		{
			
				restaurantCouponsObject.Id = reader.GetInt32( start + 0 );			
				restaurantCouponsObject.CompanyId = reader.GetGuid( start + 1 );			
				restaurantCouponsObject.CouponCode = reader.GetString( start + 2 );			
				restaurantCouponsObject.StartDate = reader.GetDateTime( start + 3 );			
				restaurantCouponsObject.EndDate = reader.GetDateTime( start + 4 );			
				restaurantCouponsObject.Discount = reader.GetString( start + 5 );			
				restaurantCouponsObject.MinimumOrder = reader.GetString( start + 6 );			
				restaurantCouponsObject.ReedemRequired = reader.GetString( start + 7 );			
				restaurantCouponsObject.CreatedDate = reader.GetDateTime( start + 8 );			
				restaurantCouponsObject.LastUpdatedDate = reader.GetDateTime( start + 9 );			
				restaurantCouponsObject.CreatedBy = reader.GetGuid( start + 10 );			
				restaurantCouponsObject.LastUpdatedBy = reader.GetGuid( start + 11 );			
				if(!reader.IsDBNull(12)) restaurantCouponsObject.DiscountType = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) restaurantCouponsObject.Status = reader.GetBoolean( start + 13 );			
			FillBaseObject(restaurantCouponsObject, reader, (start + 14));

			
			restaurantCouponsObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills RestaurantCoupons object
        /// </summary>
        /// <param name="restaurantCouponsObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(RestaurantCouponsBase restaurantCouponsObject, SqlDataReader reader)
		{
			FillObject(restaurantCouponsObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves RestaurantCoupons object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>RestaurantCoupons object</returns>
		private RestaurantCoupons GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					RestaurantCoupons restaurantCouponsObject= new RestaurantCoupons();
					FillObject(restaurantCouponsObject, reader);
					return restaurantCouponsObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of RestaurantCoupons objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of RestaurantCoupons objects</returns>
		private RestaurantCouponsList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//RestaurantCoupons list
			RestaurantCouponsList list = new RestaurantCouponsList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					RestaurantCoupons restaurantCouponsObject = new RestaurantCoupons();
					FillObject(restaurantCouponsObject, reader);

					list.Add(restaurantCouponsObject);
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
