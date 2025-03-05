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
	public partial class CustomerPackageServiceDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERPACKAGESERVICE = "InsertCustomerPackageService";
		private const string UPDATECUSTOMERPACKAGESERVICE = "UpdateCustomerPackageService";
		private const string DELETECUSTOMERPACKAGESERVICE = "DeleteCustomerPackageService";
		private const string GETCUSTOMERPACKAGESERVICEBYID = "GetCustomerPackageServiceById";
		private const string GETALLCUSTOMERPACKAGESERVICE = "GetAllCustomerPackageService";
		private const string GETPAGEDCUSTOMERPACKAGESERVICE = "GetPagedCustomerPackageService";
		private const string GETCUSTOMERPACKAGESERVICEMAXIMUMID = "GetCustomerPackageServiceMaximumId";
		private const string GETCUSTOMERPACKAGESERVICEROWCOUNT = "GetCustomerPackageServiceRowCount";	
		private const string GETCUSTOMERPACKAGESERVICEBYQUERY = "GetCustomerPackageServiceByQuery";
		#endregion
		
		#region Constructors
		public CustomerPackageServiceDataAccess(ClientContext context) : base(context) { }
		public CustomerPackageServiceDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerPackageServiceObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerPackageServiceBase customerPackageServiceObject)
		{	
			AddParameter(cmd, pGuid(CustomerPackageServiceBase.Property_CompanyId, customerPackageServiceObject.CompanyId));
			AddParameter(cmd, pGuid(CustomerPackageServiceBase.Property_CustomerId, customerPackageServiceObject.CustomerId));
			AddParameter(cmd, pGuid(CustomerPackageServiceBase.Property_PackageId, customerPackageServiceObject.PackageId));
			AddParameter(cmd, pGuid(CustomerPackageServiceBase.Property_EquipmentId, customerPackageServiceObject.EquipmentId));
			AddParameter(cmd, pDouble(CustomerPackageServiceBase.Property_MonthlyRate, customerPackageServiceObject.MonthlyRate));
			AddParameter(cmd, pDouble(CustomerPackageServiceBase.Property_DiscountRate, customerPackageServiceObject.DiscountRate));
			AddParameter(cmd, pDouble(CustomerPackageServiceBase.Property_Total, customerPackageServiceObject.Total));
			AddParameter(cmd, pGuid(CustomerPackageServiceBase.Property_ManufacturerId, customerPackageServiceObject.ManufacturerId));
			AddParameter(cmd, pGuid(CustomerPackageServiceBase.Property_LocationId, customerPackageServiceObject.LocationId));
			AddParameter(cmd, pGuid(CustomerPackageServiceBase.Property_TypeId, customerPackageServiceObject.TypeId));
			AddParameter(cmd, pGuid(CustomerPackageServiceBase.Property_ModelId, customerPackageServiceObject.ModelId));
			AddParameter(cmd, pGuid(CustomerPackageServiceBase.Property_FinishId, customerPackageServiceObject.FinishId));
			AddParameter(cmd, pGuid(CustomerPackageServiceBase.Property_CapacityId, customerPackageServiceObject.CapacityId));
			AddParameter(cmd, pBool(CustomerPackageServiceBase.Property_IsPackageService, customerPackageServiceObject.IsPackageService));
			AddParameter(cmd, pBool(CustomerPackageServiceBase.Property_IsNonCommissionable, customerPackageServiceObject.IsNonCommissionable));
			AddParameter(cmd, pInt32(CustomerPackageServiceBase.Property_AppointmentIntId, customerPackageServiceObject.AppointmentIntId));
			AddParameter(cmd, pBool(CustomerPackageServiceBase.Property_IsInvoice, customerPackageServiceObject.IsInvoice));
			AddParameter(cmd, pInt32(CustomerPackageServiceBase.Property_AppointmentEquipmentIntId, customerPackageServiceObject.AppointmentEquipmentIntId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerPackageService
        /// </summary>
        /// <param name="customerPackageServiceObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerPackageServiceBase customerPackageServiceObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERPACKAGESERVICE);
	
				AddParameter(cmd, pInt32Out(CustomerPackageServiceBase.Property_Id));
				AddCommonParams(cmd, customerPackageServiceObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerPackageServiceObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerPackageServiceObject.Id = (Int32)GetOutParameter(cmd, CustomerPackageServiceBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerPackageServiceObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerPackageService
        /// </summary>
        /// <param name="customerPackageServiceObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerPackageServiceBase customerPackageServiceObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERPACKAGESERVICE);
				
				AddParameter(cmd, pInt32(CustomerPackageServiceBase.Property_Id, customerPackageServiceObject.Id));
				AddCommonParams(cmd, customerPackageServiceObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerPackageServiceObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerPackageServiceObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerPackageService
        /// </summary>
        /// <param name="Id">Id of the CustomerPackageService object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERPACKAGESERVICE);	
				
				AddParameter(cmd, pInt32(CustomerPackageServiceBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerPackageService), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerPackageService object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerPackageService object to retrieve</param>
        /// <returns>CustomerPackageService object, null if not found</returns>
		public CustomerPackageService Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERPACKAGESERVICEBYID))
			{
				AddParameter( cmd, pInt32(CustomerPackageServiceBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerPackageService objects 
        /// </summary>
        /// <returns>A list of CustomerPackageService objects</returns>
		public CustomerPackageServiceList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERPACKAGESERVICE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerPackageService objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerPackageService objects</returns>
		public CustomerPackageServiceList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERPACKAGESERVICE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerPackageServiceList _CustomerPackageServiceList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerPackageServiceList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerPackageService objects by query String
        /// </summary>
        /// <returns>A list of CustomerPackageService objects</returns>
		public CustomerPackageServiceList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERPACKAGESERVICEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerPackageService Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerPackageService
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERPACKAGESERVICEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerPackageService Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerPackageService
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerPackageServiceRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERPACKAGESERVICEROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerPackageServiceRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerPackageServiceRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerPackageService object
        /// </summary>
        /// <param name="customerPackageServiceObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerPackageServiceBase customerPackageServiceObject, SqlDataReader reader, int start)
		{
			
				customerPackageServiceObject.Id = reader.GetInt32( start + 0 );			
				customerPackageServiceObject.CompanyId = reader.GetGuid( start + 1 );			
				customerPackageServiceObject.CustomerId = reader.GetGuid( start + 2 );			
				customerPackageServiceObject.PackageId = reader.GetGuid( start + 3 );			
				customerPackageServiceObject.EquipmentId = reader.GetGuid( start + 4 );			
				if(!reader.IsDBNull(5)) customerPackageServiceObject.MonthlyRate = reader.GetDouble( start + 5 );			
				if(!reader.IsDBNull(6)) customerPackageServiceObject.DiscountRate = reader.GetDouble( start + 6 );			
				if(!reader.IsDBNull(7)) customerPackageServiceObject.Total = reader.GetDouble( start + 7 );			
				customerPackageServiceObject.ManufacturerId = reader.GetGuid( start + 8 );			
				customerPackageServiceObject.LocationId = reader.GetGuid( start + 9 );			
				customerPackageServiceObject.TypeId = reader.GetGuid( start + 10 );			
				customerPackageServiceObject.ModelId = reader.GetGuid( start + 11 );			
				customerPackageServiceObject.FinishId = reader.GetGuid( start + 12 );			
				customerPackageServiceObject.CapacityId = reader.GetGuid( start + 13 );			
				if(!reader.IsDBNull(14)) customerPackageServiceObject.IsPackageService = reader.GetBoolean( start + 14 );			
				if(!reader.IsDBNull(15)) customerPackageServiceObject.IsNonCommissionable = reader.GetBoolean( start + 15 );			
				if(!reader.IsDBNull(16)) customerPackageServiceObject.AppointmentIntId = reader.GetInt32( start + 16 );			
				if(!reader.IsDBNull(17)) customerPackageServiceObject.IsInvoice = reader.GetBoolean( start + 17 );			
				if(!reader.IsDBNull(18)) customerPackageServiceObject.AppointmentEquipmentIntId = reader.GetInt32( start + 18 );			
			FillBaseObject(customerPackageServiceObject, reader, (start + 19));

			
			customerPackageServiceObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerPackageService object
        /// </summary>
        /// <param name="customerPackageServiceObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerPackageServiceBase customerPackageServiceObject, SqlDataReader reader)
		{
			FillObject(customerPackageServiceObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerPackageService object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerPackageService object</returns>
		private CustomerPackageService GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerPackageService customerPackageServiceObject= new CustomerPackageService();
					FillObject(customerPackageServiceObject, reader);
					return customerPackageServiceObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerPackageService objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerPackageService objects</returns>
		private CustomerPackageServiceList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerPackageService list
			CustomerPackageServiceList list = new CustomerPackageServiceList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerPackageService customerPackageServiceObject = new CustomerPackageService();
					FillObject(customerPackageServiceObject, reader);

					list.Add(customerPackageServiceObject);
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
