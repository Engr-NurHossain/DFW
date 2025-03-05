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
	public partial class RestaurantLocationDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTRESTAURANTLOCATION = "InsertRestaurantLocation";
		private const string UPDATERESTAURANTLOCATION = "UpdateRestaurantLocation";
		private const string DELETERESTAURANTLOCATION = "DeleteRestaurantLocation";
		private const string GETRESTAURANTLOCATIONBYID = "GetRestaurantLocationById";
		private const string GETALLRESTAURANTLOCATION = "GetAllRestaurantLocation";
		private const string GETPAGEDRESTAURANTLOCATION = "GetPagedRestaurantLocation";
		private const string GETRESTAURANTLOCATIONMAXIMUMID = "GetRestaurantLocationMaximumId";
		private const string GETRESTAURANTLOCATIONROWCOUNT = "GetRestaurantLocationRowCount";	
		private const string GETRESTAURANTLOCATIONBYQUERY = "GetRestaurantLocationByQuery";
		#endregion
		
		#region Constructors
		public RestaurantLocationDataAccess(ClientContext context) : base(context) { }
		public RestaurantLocationDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="restaurantLocationObject"></param>
		private void AddCommonParams(SqlCommand cmd, RestaurantLocationBase restaurantLocationObject)
		{	
			AddParameter(cmd, pGuid(RestaurantLocationBase.Property_CompanyId, restaurantLocationObject.CompanyId));
			AddParameter(cmd, pNVarChar(RestaurantLocationBase.Property_LocationName, 100, restaurantLocationObject.LocationName));
			AddParameter(cmd, pNVarChar(RestaurantLocationBase.Property_StreetAddress, restaurantLocationObject.StreetAddress));
			AddParameter(cmd, pNVarChar(RestaurantLocationBase.Property_Address2, restaurantLocationObject.Address2));
			AddParameter(cmd, pNVarChar(RestaurantLocationBase.Property_City, 50, restaurantLocationObject.City));
			AddParameter(cmd, pNVarChar(RestaurantLocationBase.Property_State, 50, restaurantLocationObject.State));
			AddParameter(cmd, pNVarChar(RestaurantLocationBase.Property_Zip, 50, restaurantLocationObject.Zip));
			AddParameter(cmd, pNVarChar(RestaurantLocationBase.Property_ContactName, 100, restaurantLocationObject.ContactName));
			AddParameter(cmd, pNVarChar(RestaurantLocationBase.Property_ContactPhone, 50, restaurantLocationObject.ContactPhone));
			AddParameter(cmd, pNVarChar(RestaurantLocationBase.Property_ContactEmail, 50, restaurantLocationObject.ContactEmail));
			AddParameter(cmd, pGuid(RestaurantLocationBase.Property_CreatedBy, restaurantLocationObject.CreatedBy));
			AddParameter(cmd, pDateTime(RestaurantLocationBase.Property_CreatedDate, restaurantLocationObject.CreatedDate));
			AddParameter(cmd, pGuid(RestaurantLocationBase.Property_LastUpdatedBy, restaurantLocationObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(RestaurantLocationBase.Property_LastUpdatedDate, restaurantLocationObject.LastUpdatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts RestaurantLocation
        /// </summary>
        /// <param name="restaurantLocationObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(RestaurantLocationBase restaurantLocationObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTRESTAURANTLOCATION);
	
				AddParameter(cmd, pInt32Out(RestaurantLocationBase.Property_Id));
				AddCommonParams(cmd, restaurantLocationObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					restaurantLocationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					restaurantLocationObject.Id = (Int32)GetOutParameter(cmd, RestaurantLocationBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(restaurantLocationObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates RestaurantLocation
        /// </summary>
        /// <param name="restaurantLocationObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(RestaurantLocationBase restaurantLocationObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATERESTAURANTLOCATION);
				
				AddParameter(cmd, pInt32(RestaurantLocationBase.Property_Id, restaurantLocationObject.Id));
				AddCommonParams(cmd, restaurantLocationObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					restaurantLocationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(restaurantLocationObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes RestaurantLocation
        /// </summary>
        /// <param name="Id">Id of the RestaurantLocation object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETERESTAURANTLOCATION);	
				
				AddParameter(cmd, pInt32(RestaurantLocationBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(RestaurantLocation), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves RestaurantLocation object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the RestaurantLocation object to retrieve</param>
        /// <returns>RestaurantLocation object, null if not found</returns>
		public RestaurantLocation Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTAURANTLOCATIONBYID))
			{
				AddParameter( cmd, pInt32(RestaurantLocationBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all RestaurantLocation objects 
        /// </summary>
        /// <returns>A list of RestaurantLocation objects</returns>
		public RestaurantLocationList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLRESTAURANTLOCATION))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all RestaurantLocation objects by PageRequest
        /// </summary>
        /// <returns>A list of RestaurantLocation objects</returns>
		public RestaurantLocationList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDRESTAURANTLOCATION))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				RestaurantLocationList _RestaurantLocationList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _RestaurantLocationList;
			}
		}
		
		/// <summary>
        /// Retrieves all RestaurantLocation objects by query String
        /// </summary>
        /// <returns>A list of RestaurantLocation objects</returns>
		public RestaurantLocationList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTAURANTLOCATIONBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get RestaurantLocation Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of RestaurantLocation
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTAURANTLOCATIONMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get RestaurantLocation Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of RestaurantLocation
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _RestaurantLocationRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTAURANTLOCATIONROWCOUNT))
			{
				SqlDataReader reader;
				_RestaurantLocationRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _RestaurantLocationRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills RestaurantLocation object
        /// </summary>
        /// <param name="restaurantLocationObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(RestaurantLocationBase restaurantLocationObject, SqlDataReader reader, int start)
		{
			
				restaurantLocationObject.Id = reader.GetInt32( start + 0 );			
				restaurantLocationObject.CompanyId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) restaurantLocationObject.LocationName = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) restaurantLocationObject.StreetAddress = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) restaurantLocationObject.Address2 = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) restaurantLocationObject.City = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) restaurantLocationObject.State = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) restaurantLocationObject.Zip = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) restaurantLocationObject.ContactName = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) restaurantLocationObject.ContactPhone = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) restaurantLocationObject.ContactEmail = reader.GetString( start + 10 );			
				restaurantLocationObject.CreatedBy = reader.GetGuid( start + 11 );			
				restaurantLocationObject.CreatedDate = reader.GetDateTime( start + 12 );			
				restaurantLocationObject.LastUpdatedBy = reader.GetGuid( start + 13 );			
				restaurantLocationObject.LastUpdatedDate = reader.GetDateTime( start + 14 );			
			FillBaseObject(restaurantLocationObject, reader, (start + 15));

			
			restaurantLocationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills RestaurantLocation object
        /// </summary>
        /// <param name="restaurantLocationObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(RestaurantLocationBase restaurantLocationObject, SqlDataReader reader)
		{
			FillObject(restaurantLocationObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves RestaurantLocation object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>RestaurantLocation object</returns>
		private RestaurantLocation GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					RestaurantLocation restaurantLocationObject= new RestaurantLocation();
					FillObject(restaurantLocationObject, reader);
					return restaurantLocationObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of RestaurantLocation objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of RestaurantLocation objects</returns>
		private RestaurantLocationList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//RestaurantLocation list
			RestaurantLocationList list = new RestaurantLocationList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					RestaurantLocation restaurantLocationObject = new RestaurantLocation();
					FillObject(restaurantLocationObject, reader);

					list.Add(restaurantLocationObject);
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
