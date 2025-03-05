using System;
using System.Data;
using System.Data.SqlClient;
using HS.Framework;
using HS.Framework.DataAccess;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using NLog;

namespace HS.DataAccess
{
	public partial class CustomerPackageEqpDataAccess : BaseDataAccess
	{

        protected Logger logger = LogManager.GetCurrentClassLogger();

        #region Constants
        private const string INSERTCUSTOMERPACKAGEEQP = "InsertCustomerPackageEqp";
		private const string UPDATECUSTOMERPACKAGEEQP = "UpdateCustomerPackageEqp_v2";
		private const string DELETECUSTOMERPACKAGEEQP = "DeleteCustomerPackageEqp";
		private const string GETCUSTOMERPACKAGEEQPBYID = "GetCustomerPackageEqpById";
		private const string GETALLCUSTOMERPACKAGEEQP = "GetAllCustomerPackageEqp";
		private const string GETPAGEDCUSTOMERPACKAGEEQP = "GetPagedCustomerPackageEqp";
		private const string GETCUSTOMERPACKAGEEQPMAXIMUMID = "GetCustomerPackageEqpMaximumId";
		private const string GETCUSTOMERPACKAGEEQPROWCOUNT = "GetCustomerPackageEqpRowCount";	
		private const string GETCUSTOMERPACKAGEEQPBYQUERY = "GetCustomerPackageEqpByQuery";
		#endregion
		
		#region Constructors
		public CustomerPackageEqpDataAccess(ClientContext context) : base(context) { }
		public CustomerPackageEqpDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerPackageEqpObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerPackageEqpBase customerPackageEqpObject)
		{	
			AddParameter(cmd, pGuid(CustomerPackageEqpBase.Property_CompanyId, customerPackageEqpObject.CompanyId));
			AddParameter(cmd, pGuid(CustomerPackageEqpBase.Property_CustomerId, customerPackageEqpObject.CustomerId));
			AddParameter(cmd, pGuid(CustomerPackageEqpBase.Property_PackageId, customerPackageEqpObject.PackageId));
			AddParameter(cmd, pGuid(CustomerPackageEqpBase.Property_EquipmentId, customerPackageEqpObject.EquipmentId));
			AddParameter(cmd, pBool(CustomerPackageEqpBase.Property_IsIncluded, customerPackageEqpObject.IsIncluded));
			AddParameter(cmd, pBool(CustomerPackageEqpBase.Property_IsDevice, customerPackageEqpObject.IsDevice));
			AddParameter(cmd, pBool(CustomerPackageEqpBase.Property_IsOptionalEqp, customerPackageEqpObject.IsOptionalEqp));
			AddParameter(cmd, pInt32(CustomerPackageEqpBase.Property_Quantity, customerPackageEqpObject.Quantity));
			AddParameter(cmd, pDouble(CustomerPackageEqpBase.Property_UnitPrice, customerPackageEqpObject.UnitPrice));
			AddParameter(cmd, pDouble(CustomerPackageEqpBase.Property_DiscountUnitPricce, customerPackageEqpObject.DiscountUnitPricce));
			AddParameter(cmd, pDouble(CustomerPackageEqpBase.Property_DiscountPckage, customerPackageEqpObject.DiscountPckage));
			AddParameter(cmd, pDouble(CustomerPackageEqpBase.Property_Total, customerPackageEqpObject.Total));
			AddParameter(cmd, pBool(CustomerPackageEqpBase.Property_IsServiceEquipment, customerPackageEqpObject.IsServiceEquipment));
			AddParameter(cmd, pGuid(CustomerPackageEqpBase.Property_ServiceId, customerPackageEqpObject.ServiceId));
			AddParameter(cmd, pBool(CustomerPackageEqpBase.Property_IsTransfered, customerPackageEqpObject.IsTransfered));
			AddParameter(cmd, pBool(CustomerPackageEqpBase.Property_IsEqpExist, customerPackageEqpObject.IsEqpExist));
			AddParameter(cmd, pBool(CustomerPackageEqpBase.Property_IsPackageEqp, customerPackageEqpObject.IsPackageEqp));
			AddParameter(cmd, pBool(CustomerPackageEqpBase.Property_IsNonCommissionable, customerPackageEqpObject.IsNonCommissionable));
			AddParameter(cmd, pInt32(CustomerPackageEqpBase.Property_AppointmentIntId, customerPackageEqpObject.AppointmentIntId));
			AddParameter(cmd, pBool(CustomerPackageEqpBase.Property_IsInvoice, customerPackageEqpObject.IsInvoice));
			AddParameter(cmd, pInt32(CustomerPackageEqpBase.Property_AppointmentEquipmentIntId, customerPackageEqpObject.AppointmentEquipmentIntId));
			AddParameter(cmd, pDouble(CustomerPackageEqpBase.Property_DiscountPercent, customerPackageEqpObject.DiscountPercent));
			AddParameter(cmd, pDouble(CustomerPackageEqpBase.Property_DiscountInAmount, customerPackageEqpObject.DiscountInAmount));



		}
		#endregion

		#region Insert Method
		/// <summary>
		/// Inserts CustomerPackageEqp
		/// </summary>
		/// <param name="customerPackageEqpObject">Object to be inserted</param>
		/// <returns>Number of rows affected</returns>
		public long Insert(CustomerPackageEqpBase customerPackageEqpObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERPACKAGEEQP);
	
				AddParameter(cmd, pInt32Out(CustomerPackageEqpBase.Property_Id));
				AddCommonParams(cmd, customerPackageEqpObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerPackageEqpObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerPackageEqpObject.Id = (Int32)GetOutParameter(cmd, CustomerPackageEqpBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerPackageEqpObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerPackageEqp
        /// </summary>
        /// <param name="customerPackageEqpObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerPackageEqpBase customerPackageEqpObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERPACKAGEEQP);
				
                AddParameter(cmd, pInt32(CustomerPackageEqpBase.Property_Id, customerPackageEqpObject.Id));
				AddCommonParams(cmd, customerPackageEqpObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerPackageEqpObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
                logger.Error(x);
                throw new ObjectUpdateException(customerPackageEqpObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerPackageEqp
        /// </summary>
        /// <param name="Id">Id of the CustomerPackageEqp object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERPACKAGEEQP);	
				
				AddParameter(cmd, pInt32(CustomerPackageEqpBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerPackageEqp), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerPackageEqp object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerPackageEqp object to retrieve</param>
        /// <returns>CustomerPackageEqp object, null if not found</returns>
		public CustomerPackageEqp Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERPACKAGEEQPBYID))
			{
				AddParameter( cmd, pInt32(CustomerPackageEqpBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerPackageEqp objects 
        /// </summary>
        /// <returns>A list of CustomerPackageEqp objects</returns>
		public CustomerPackageEqpList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERPACKAGEEQP))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerPackageEqp objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerPackageEqp objects</returns>
		public CustomerPackageEqpList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERPACKAGEEQP))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerPackageEqpList _CustomerPackageEqpList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerPackageEqpList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerPackageEqp objects by query String
        /// </summary>
        /// <returns>A list of CustomerPackageEqp objects</returns>
		public CustomerPackageEqpList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERPACKAGEEQPBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerPackageEqp Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerPackageEqp
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERPACKAGEEQPMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerPackageEqp Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerPackageEqp
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerPackageEqpRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERPACKAGEEQPROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerPackageEqpRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerPackageEqpRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerPackageEqp object
        /// </summary>
        /// <param name="customerPackageEqpObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerPackageEqpBase customerPackageEqpObject, SqlDataReader reader, int start)
		{
			
				customerPackageEqpObject.Id = reader.GetInt32( start + 0 );			
				customerPackageEqpObject.CompanyId = reader.GetGuid( start + 1 );			
				customerPackageEqpObject.CustomerId = reader.GetGuid( start + 2 );			
				customerPackageEqpObject.PackageId = reader.GetGuid( start + 3 );			
				customerPackageEqpObject.EquipmentId = reader.GetGuid( start + 4 );			
				customerPackageEqpObject.IsIncluded = reader.GetBoolean( start + 5 );			
				customerPackageEqpObject.IsDevice = reader.GetBoolean( start + 6 );			
				customerPackageEqpObject.IsOptionalEqp = reader.GetBoolean( start + 7 );			
				if(!reader.IsDBNull(8)) customerPackageEqpObject.Quantity = reader.GetInt32( start + 8 );			
				if(!reader.IsDBNull(9)) customerPackageEqpObject.UnitPrice = reader.GetDouble( start + 9 );			
				if(!reader.IsDBNull(10)) customerPackageEqpObject.DiscountUnitPricce = reader.GetDouble( start + 10 );			
				if(!reader.IsDBNull(11)) customerPackageEqpObject.DiscountPckage = reader.GetDouble( start + 11 );			
				if(!reader.IsDBNull(12)) customerPackageEqpObject.Total = reader.GetDouble( start + 12 );			
				if(!reader.IsDBNull(13)) customerPackageEqpObject.IsServiceEquipment = reader.GetBoolean( start + 13 );			
				customerPackageEqpObject.ServiceId = reader.GetGuid( start + 14 );			
				if(!reader.IsDBNull(15)) customerPackageEqpObject.IsTransfered = reader.GetBoolean( start + 15 );			
				if(!reader.IsDBNull(16)) customerPackageEqpObject.IsEqpExist = reader.GetBoolean( start + 16 );			
				if(!reader.IsDBNull(17)) customerPackageEqpObject.IsPackageEqp = reader.GetBoolean( start + 17 );			
				if(!reader.IsDBNull(18)) customerPackageEqpObject.IsNonCommissionable = reader.GetBoolean( start + 18 );			
				if(!reader.IsDBNull(19)) customerPackageEqpObject.AppointmentIntId = reader.GetInt32( start + 19 );			
				if(!reader.IsDBNull(20)) customerPackageEqpObject.IsInvoice = reader.GetBoolean( start + 20 );			
				if(!reader.IsDBNull(21)) customerPackageEqpObject.AppointmentEquipmentIntId = reader.GetInt32( start + 21 );
				 if (!reader.IsDBNull(22)) customerPackageEqpObject.DiscountPercent = Decimal.ToDouble(reader.GetDecimal(start + 22));
				 if (!reader.IsDBNull(23)) customerPackageEqpObject.DiscountInAmount = Decimal.ToDouble(reader.GetDecimal(start + 23));
            FillBaseObject(customerPackageEqpObject, reader, (start + 24));

			
			customerPackageEqpObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerPackageEqp object
        /// </summary>
        /// <param name="customerPackageEqpObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerPackageEqpBase customerPackageEqpObject, SqlDataReader reader)
		{
			FillObject(customerPackageEqpObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerPackageEqp object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerPackageEqp object</returns>
		private CustomerPackageEqp GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerPackageEqp customerPackageEqpObject= new CustomerPackageEqp();
					FillObject(customerPackageEqpObject, reader);
					return customerPackageEqpObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerPackageEqp objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerPackageEqp objects</returns>
		private CustomerPackageEqpList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerPackageEqp list
			CustomerPackageEqpList list = new CustomerPackageEqpList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerPackageEqp customerPackageEqpObject = new CustomerPackageEqp();
					FillObject(customerPackageEqpObject, reader);

					list.Add(customerPackageEqpObject);
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
