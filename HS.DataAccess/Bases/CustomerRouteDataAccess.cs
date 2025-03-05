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
	public partial class CustomerRouteDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERROUTE = "InsertCustomerRoute";
		private const string UPDATECUSTOMERROUTE = "UpdateCustomerRoute";
		private const string DELETECUSTOMERROUTE = "DeleteCustomerRoute";
		private const string GETCUSTOMERROUTEBYID = "GetCustomerRouteById";
		private const string GETALLCUSTOMERROUTE = "GetAllCustomerRoute";
		private const string GETPAGEDCUSTOMERROUTE = "GetPagedCustomerRoute";
		private const string GETCUSTOMERROUTEMAXIMUMID = "GetCustomerRouteMaximumId";
		private const string GETCUSTOMERROUTEROWCOUNT = "GetCustomerRouteRowCount";	
		private const string GETCUSTOMERROUTEBYQUERY = "GetCustomerRouteByQuery";
		#endregion
		
		#region Constructors
		public CustomerRouteDataAccess(ClientContext context) : base(context) { }
		public CustomerRouteDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerRouteObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerRouteBase customerRouteObject)
		{	
			AddParameter(cmd, pGuid(CustomerRouteBase.Property_CustomerId, customerRouteObject.CustomerId));
			AddParameter(cmd, pGuid(CustomerRouteBase.Property_RouteId, customerRouteObject.RouteId));
			AddParameter(cmd, pNVarChar(CustomerRouteBase.Property_Name, 50, customerRouteObject.Name));
			AddParameter(cmd, pDateTime(CustomerRouteBase.Property_LastVisit, customerRouteObject.LastVisit));
			AddParameter(cmd, pGuid(CustomerRouteBase.Property_CreatedBy, customerRouteObject.CreatedBy));
			AddParameter(cmd, pDateTime(CustomerRouteBase.Property_CreatedDate, customerRouteObject.CreatedDate));
			AddParameter(cmd, pGuid(CustomerRouteBase.Property_UserId, customerRouteObject.UserId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerRoute
        /// </summary>
        /// <param name="customerRouteObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerRouteBase customerRouteObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERROUTE);
	
				AddParameter(cmd, pInt32Out(CustomerRouteBase.Property_Id));
				AddCommonParams(cmd, customerRouteObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerRouteObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerRouteObject.Id = (Int32)GetOutParameter(cmd, CustomerRouteBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerRouteObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerRoute
        /// </summary>
        /// <param name="customerRouteObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerRouteBase customerRouteObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERROUTE);
				
				AddParameter(cmd, pInt32(CustomerRouteBase.Property_Id, customerRouteObject.Id));
				AddCommonParams(cmd, customerRouteObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerRouteObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerRouteObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerRoute
        /// </summary>
        /// <param name="Id">Id of the CustomerRoute object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERROUTE);	
				
				AddParameter(cmd, pInt32(CustomerRouteBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerRoute), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerRoute object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerRoute object to retrieve</param>
        /// <returns>CustomerRoute object, null if not found</returns>
		public CustomerRoute Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERROUTEBYID))
			{
				AddParameter( cmd, pInt32(CustomerRouteBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerRoute objects 
        /// </summary>
        /// <returns>A list of CustomerRoute objects</returns>
		public CustomerRouteList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERROUTE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerRoute objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerRoute objects</returns>
		public CustomerRouteList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERROUTE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerRouteList _CustomerRouteList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerRouteList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerRoute objects by query String
        /// </summary>
        /// <returns>A list of CustomerRoute objects</returns>
		public CustomerRouteList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERROUTEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerRoute Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerRoute
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERROUTEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerRoute Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerRoute
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerRouteRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERROUTEROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerRouteRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerRouteRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerRoute object
        /// </summary>
        /// <param name="customerRouteObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerRouteBase customerRouteObject, SqlDataReader reader, int start)
		{
			
				customerRouteObject.Id = reader.GetInt32( start + 0 );			
				customerRouteObject.CustomerId = reader.GetGuid( start + 1 );			
				customerRouteObject.RouteId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) customerRouteObject.Name = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) customerRouteObject.LastVisit = reader.GetDateTime( start + 4 );			
				customerRouteObject.CreatedBy = reader.GetGuid( start + 5 );			
				customerRouteObject.CreatedDate = reader.GetDateTime( start + 6 );			
				customerRouteObject.UserId = reader.GetGuid( start + 7 );			
			FillBaseObject(customerRouteObject, reader, (start + 8));

			
			customerRouteObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerRoute object
        /// </summary>
        /// <param name="customerRouteObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerRouteBase customerRouteObject, SqlDataReader reader)
		{
			FillObject(customerRouteObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerRoute object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerRoute object</returns>
		private CustomerRoute GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerRoute customerRouteObject= new CustomerRoute();
					FillObject(customerRouteObject, reader);
					return customerRouteObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerRoute objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerRoute objects</returns>
		private CustomerRouteList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerRoute list
			CustomerRouteList list = new CustomerRouteList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerRoute customerRouteObject = new CustomerRoute();
					FillObject(customerRouteObject, reader);

					list.Add(customerRouteObject);
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
