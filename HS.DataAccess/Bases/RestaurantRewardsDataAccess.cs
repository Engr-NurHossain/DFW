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
	public partial class RestaurantRewardsDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTRESTAURANTREWARDS = "InsertRestaurantRewards";
		private const string UPDATERESTAURANTREWARDS = "UpdateRestaurantRewards";
		private const string DELETERESTAURANTREWARDS = "DeleteRestaurantRewards";
		private const string GETRESTAURANTREWARDSBYID = "GetRestaurantRewardsById";
		private const string GETALLRESTAURANTREWARDS = "GetAllRestaurantRewards";
		private const string GETPAGEDRESTAURANTREWARDS = "GetPagedRestaurantRewards";
		private const string GETRESTAURANTREWARDSMAXIMUMID = "GetRestaurantRewardsMaximumId";
		private const string GETRESTAURANTREWARDSROWCOUNT = "GetRestaurantRewardsRowCount";	
		private const string GETRESTAURANTREWARDSBYQUERY = "GetRestaurantRewardsByQuery";
		#endregion
		
		#region Constructors
		public RestaurantRewardsDataAccess(ClientContext context) : base(context) { }
		public RestaurantRewardsDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="restaurantRewardsObject"></param>
		private void AddCommonParams(SqlCommand cmd, RestaurantRewardsBase restaurantRewardsObject)
		{	
			AddParameter(cmd, pGuid(RestaurantRewardsBase.Property_CompanyId, restaurantRewardsObject.CompanyId));
			AddParameter(cmd, pDouble(RestaurantRewardsBase.Property_DollarSpent, restaurantRewardsObject.DollarSpent));
			AddParameter(cmd, pDouble(RestaurantRewardsBase.Property_ReedemValue, restaurantRewardsObject.ReedemValue));
			AddParameter(cmd, pBool(RestaurantRewardsBase.Property_Status, restaurantRewardsObject.Status));
			AddParameter(cmd, pDateTime(RestaurantRewardsBase.Property_CreatedDate, restaurantRewardsObject.CreatedDate));
			AddParameter(cmd, pGuid(RestaurantRewardsBase.Property_CreatedBy, restaurantRewardsObject.CreatedBy));
			AddParameter(cmd, pGuid(RestaurantRewardsBase.Property_LastUpdatedBy, restaurantRewardsObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(RestaurantRewardsBase.Property_LastUpdatedDate, restaurantRewardsObject.LastUpdatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts RestaurantRewards
        /// </summary>
        /// <param name="restaurantRewardsObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(RestaurantRewardsBase restaurantRewardsObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTRESTAURANTREWARDS);
	
				AddParameter(cmd, pInt32Out(RestaurantRewardsBase.Property_Id));
				AddCommonParams(cmd, restaurantRewardsObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					restaurantRewardsObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					restaurantRewardsObject.Id = (Int32)GetOutParameter(cmd, RestaurantRewardsBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(restaurantRewardsObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates RestaurantRewards
        /// </summary>
        /// <param name="restaurantRewardsObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(RestaurantRewardsBase restaurantRewardsObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATERESTAURANTREWARDS);
				
				AddParameter(cmd, pInt32(RestaurantRewardsBase.Property_Id, restaurantRewardsObject.Id));
				AddCommonParams(cmd, restaurantRewardsObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					restaurantRewardsObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(restaurantRewardsObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes RestaurantRewards
        /// </summary>
        /// <param name="Id">Id of the RestaurantRewards object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETERESTAURANTREWARDS);	
				
				AddParameter(cmd, pInt32(RestaurantRewardsBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(RestaurantRewards), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves RestaurantRewards object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the RestaurantRewards object to retrieve</param>
        /// <returns>RestaurantRewards object, null if not found</returns>
		public RestaurantRewards Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTAURANTREWARDSBYID))
			{
				AddParameter( cmd, pInt32(RestaurantRewardsBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all RestaurantRewards objects 
        /// </summary>
        /// <returns>A list of RestaurantRewards objects</returns>
		public RestaurantRewardsList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLRESTAURANTREWARDS))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all RestaurantRewards objects by PageRequest
        /// </summary>
        /// <returns>A list of RestaurantRewards objects</returns>
		public RestaurantRewardsList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDRESTAURANTREWARDS))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				RestaurantRewardsList _RestaurantRewardsList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _RestaurantRewardsList;
			}
		}
		
		/// <summary>
        /// Retrieves all RestaurantRewards objects by query String
        /// </summary>
        /// <returns>A list of RestaurantRewards objects</returns>
		public RestaurantRewardsList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTAURANTREWARDSBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get RestaurantRewards Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of RestaurantRewards
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTAURANTREWARDSMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get RestaurantRewards Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of RestaurantRewards
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _RestaurantRewardsRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTAURANTREWARDSROWCOUNT))
			{
				SqlDataReader reader;
				_RestaurantRewardsRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _RestaurantRewardsRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills RestaurantRewards object
        /// </summary>
        /// <param name="restaurantRewardsObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(RestaurantRewardsBase restaurantRewardsObject, SqlDataReader reader, int start)
		{
			
				restaurantRewardsObject.Id = reader.GetInt32( start + 0 );			
				restaurantRewardsObject.CompanyId = reader.GetGuid( start + 1 );			
				restaurantRewardsObject.DollarSpent = reader.GetDouble( start + 2 );			
				restaurantRewardsObject.ReedemValue = reader.GetDouble( start + 3 );			
				restaurantRewardsObject.Status = reader.GetBoolean( start + 4 );			
				restaurantRewardsObject.CreatedDate = reader.GetDateTime( start + 5 );			
				restaurantRewardsObject.CreatedBy = reader.GetGuid( start + 6 );			
				restaurantRewardsObject.LastUpdatedBy = reader.GetGuid( start + 7 );			
				restaurantRewardsObject.LastUpdatedDate = reader.GetDateTime( start + 8 );			
			FillBaseObject(restaurantRewardsObject, reader, (start + 9));

			
			restaurantRewardsObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills RestaurantRewards object
        /// </summary>
        /// <param name="restaurantRewardsObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(RestaurantRewardsBase restaurantRewardsObject, SqlDataReader reader)
		{
			FillObject(restaurantRewardsObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves RestaurantRewards object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>RestaurantRewards object</returns>
		private RestaurantRewards GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					RestaurantRewards restaurantRewardsObject= new RestaurantRewards();
					FillObject(restaurantRewardsObject, reader);
					return restaurantRewardsObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of RestaurantRewards objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of RestaurantRewards objects</returns>
		private RestaurantRewardsList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//RestaurantRewards list
			RestaurantRewardsList list = new RestaurantRewardsList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					RestaurantRewards restaurantRewardsObject = new RestaurantRewards();
					FillObject(restaurantRewardsObject, reader);

					list.Add(restaurantRewardsObject);
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
