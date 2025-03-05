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
	public partial class RestaurantSiteConfigurationDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTRESTAURANTSITECONFIGURATION = "InsertRestaurantSiteConfiguration";
		private const string UPDATERESTAURANTSITECONFIGURATION = "UpdateRestaurantSiteConfiguration";
		private const string DELETERESTAURANTSITECONFIGURATION = "DeleteRestaurantSiteConfiguration";
		private const string GETRESTAURANTSITECONFIGURATIONBYID = "GetRestaurantSiteConfigurationById";
		private const string GETALLRESTAURANTSITECONFIGURATION = "GetAllRestaurantSiteConfiguration";
		private const string GETPAGEDRESTAURANTSITECONFIGURATION = "GetPagedRestaurantSiteConfiguration";
		private const string GETRESTAURANTSITECONFIGURATIONMAXIMUMID = "GetRestaurantSiteConfigurationMaximumId";
		private const string GETRESTAURANTSITECONFIGURATIONROWCOUNT = "GetRestaurantSiteConfigurationRowCount";	
		private const string GETRESTAURANTSITECONFIGURATIONBYQUERY = "GetRestaurantSiteConfigurationByQuery";
		#endregion
		
		#region Constructors
		public RestaurantSiteConfigurationDataAccess(ClientContext context) : base(context) { }
		public RestaurantSiteConfigurationDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="restaurantSiteConfigurationObject"></param>
		private void AddCommonParams(SqlCommand cmd, RestaurantSiteConfigurationBase restaurantSiteConfigurationObject)
		{	
			AddParameter(cmd, pGuid(RestaurantSiteConfigurationBase.Property_CompanyId, restaurantSiteConfigurationObject.CompanyId));
			AddParameter(cmd, pNVarChar(RestaurantSiteConfigurationBase.Property_SiteName, 100, restaurantSiteConfigurationObject.SiteName));
			AddParameter(cmd, pNVarChar(RestaurantSiteConfigurationBase.Property_DomainName, restaurantSiteConfigurationObject.DomainName));
			AddParameter(cmd, pNVarChar(RestaurantSiteConfigurationBase.Property_StorePhone, 50, restaurantSiteConfigurationObject.StorePhone));
			AddParameter(cmd, pNVarChar(RestaurantSiteConfigurationBase.Property_SendOrdersEmail, 50, restaurantSiteConfigurationObject.SendOrdersEmail));
			AddParameter(cmd, pNVarChar(RestaurantSiteConfigurationBase.Property_ThemeURL, restaurantSiteConfigurationObject.ThemeURL));
			AddParameter(cmd, pGuid(RestaurantSiteConfigurationBase.Property_CreatedBy, restaurantSiteConfigurationObject.CreatedBy));
			AddParameter(cmd, pDateTime(RestaurantSiteConfigurationBase.Property_CreatedDate, restaurantSiteConfigurationObject.CreatedDate));
			AddParameter(cmd, pGuid(RestaurantSiteConfigurationBase.Property_LastUpdatedBy, restaurantSiteConfigurationObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(RestaurantSiteConfigurationBase.Property_LastUpdatedDate, restaurantSiteConfigurationObject.LastUpdatedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts RestaurantSiteConfiguration
        /// </summary>
        /// <param name="restaurantSiteConfigurationObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(RestaurantSiteConfigurationBase restaurantSiteConfigurationObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTRESTAURANTSITECONFIGURATION);
	
				AddParameter(cmd, pInt32Out(RestaurantSiteConfigurationBase.Property_Id));
				AddCommonParams(cmd, restaurantSiteConfigurationObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					restaurantSiteConfigurationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					restaurantSiteConfigurationObject.Id = (Int32)GetOutParameter(cmd, RestaurantSiteConfigurationBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(restaurantSiteConfigurationObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates RestaurantSiteConfiguration
        /// </summary>
        /// <param name="restaurantSiteConfigurationObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(RestaurantSiteConfigurationBase restaurantSiteConfigurationObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATERESTAURANTSITECONFIGURATION);
				
				AddParameter(cmd, pInt32(RestaurantSiteConfigurationBase.Property_Id, restaurantSiteConfigurationObject.Id));
				AddCommonParams(cmd, restaurantSiteConfigurationObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					restaurantSiteConfigurationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(restaurantSiteConfigurationObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes RestaurantSiteConfiguration
        /// </summary>
        /// <param name="Id">Id of the RestaurantSiteConfiguration object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETERESTAURANTSITECONFIGURATION);	
				
				AddParameter(cmd, pInt32(RestaurantSiteConfigurationBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(RestaurantSiteConfiguration), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves RestaurantSiteConfiguration object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the RestaurantSiteConfiguration object to retrieve</param>
        /// <returns>RestaurantSiteConfiguration object, null if not found</returns>
		public RestaurantSiteConfiguration Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTAURANTSITECONFIGURATIONBYID))
			{
				AddParameter( cmd, pInt32(RestaurantSiteConfigurationBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all RestaurantSiteConfiguration objects 
        /// </summary>
        /// <returns>A list of RestaurantSiteConfiguration objects</returns>
		public RestaurantSiteConfigurationList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLRESTAURANTSITECONFIGURATION))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all RestaurantSiteConfiguration objects by PageRequest
        /// </summary>
        /// <returns>A list of RestaurantSiteConfiguration objects</returns>
		public RestaurantSiteConfigurationList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDRESTAURANTSITECONFIGURATION))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				RestaurantSiteConfigurationList _RestaurantSiteConfigurationList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _RestaurantSiteConfigurationList;
			}
		}
		
		/// <summary>
        /// Retrieves all RestaurantSiteConfiguration objects by query String
        /// </summary>
        /// <returns>A list of RestaurantSiteConfiguration objects</returns>
		public RestaurantSiteConfigurationList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETRESTAURANTSITECONFIGURATIONBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get RestaurantSiteConfiguration Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of RestaurantSiteConfiguration
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTAURANTSITECONFIGURATIONMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get RestaurantSiteConfiguration Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of RestaurantSiteConfiguration
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _RestaurantSiteConfigurationRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETRESTAURANTSITECONFIGURATIONROWCOUNT))
			{
				SqlDataReader reader;
				_RestaurantSiteConfigurationRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _RestaurantSiteConfigurationRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills RestaurantSiteConfiguration object
        /// </summary>
        /// <param name="restaurantSiteConfigurationObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(RestaurantSiteConfigurationBase restaurantSiteConfigurationObject, SqlDataReader reader, int start)
		{
			
				restaurantSiteConfigurationObject.Id = reader.GetInt32( start + 0 );			
				restaurantSiteConfigurationObject.CompanyId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) restaurantSiteConfigurationObject.SiteName = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) restaurantSiteConfigurationObject.DomainName = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) restaurantSiteConfigurationObject.StorePhone = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) restaurantSiteConfigurationObject.SendOrdersEmail = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) restaurantSiteConfigurationObject.ThemeURL = reader.GetString( start + 6 );			
				restaurantSiteConfigurationObject.CreatedBy = reader.GetGuid( start + 7 );			
				restaurantSiteConfigurationObject.CreatedDate = reader.GetDateTime( start + 8 );			
				restaurantSiteConfigurationObject.LastUpdatedBy = reader.GetGuid( start + 9 );			
				restaurantSiteConfigurationObject.LastUpdatedDate = reader.GetDateTime( start + 10 );			
			FillBaseObject(restaurantSiteConfigurationObject, reader, (start + 11));

			
			restaurantSiteConfigurationObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills RestaurantSiteConfiguration object
        /// </summary>
        /// <param name="restaurantSiteConfigurationObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(RestaurantSiteConfigurationBase restaurantSiteConfigurationObject, SqlDataReader reader)
		{
			FillObject(restaurantSiteConfigurationObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves RestaurantSiteConfiguration object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>RestaurantSiteConfiguration object</returns>
		private RestaurantSiteConfiguration GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					RestaurantSiteConfiguration restaurantSiteConfigurationObject= new RestaurantSiteConfiguration();
					FillObject(restaurantSiteConfigurationObject, reader);
					return restaurantSiteConfigurationObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of RestaurantSiteConfiguration objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of RestaurantSiteConfiguration objects</returns>
		private RestaurantSiteConfigurationList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//RestaurantSiteConfiguration list
			RestaurantSiteConfigurationList list = new RestaurantSiteConfigurationList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					RestaurantSiteConfiguration restaurantSiteConfigurationObject = new RestaurantSiteConfiguration();
					FillObject(restaurantSiteConfigurationObject, reader);

					list.Add(restaurantSiteConfigurationObject);
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
