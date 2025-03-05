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
	public partial class PaymentProfileCustomerDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPAYMENTPROFILECUSTOMER = "InsertPaymentProfileCustomer";
		private const string UPDATEPAYMENTPROFILECUSTOMER = "UpdatePaymentProfileCustomer";
		private const string DELETEPAYMENTPROFILECUSTOMER = "DeletePaymentProfileCustomer";
		private const string GETPAYMENTPROFILECUSTOMERBYID = "GetPaymentProfileCustomerById";
		private const string GETALLPAYMENTPROFILECUSTOMER = "GetAllPaymentProfileCustomer";
		private const string GETPAGEDPAYMENTPROFILECUSTOMER = "GetPagedPaymentProfileCustomer";
		private const string GETPAYMENTPROFILECUSTOMERMAXIMUMID = "GetPaymentProfileCustomerMaximumId";
		private const string GETPAYMENTPROFILECUSTOMERROWCOUNT = "GetPaymentProfileCustomerRowCount";	
		private const string GETPAYMENTPROFILECUSTOMERBYQUERY = "GetPaymentProfileCustomerByQuery";
		#endregion
		
		#region Constructors
		public PaymentProfileCustomerDataAccess(ClientContext context) : base(context) { }
		public PaymentProfileCustomerDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="paymentProfileCustomerObject"></param>
		private void AddCommonParams(SqlCommand cmd, PaymentProfileCustomerBase paymentProfileCustomerObject)
		{	
			AddParameter(cmd, pGuid(PaymentProfileCustomerBase.Property_CompanyId, paymentProfileCustomerObject.CompanyId));
			AddParameter(cmd, pGuid(PaymentProfileCustomerBase.Property_CustomerId, paymentProfileCustomerObject.CustomerId));
			AddParameter(cmd, pInt32(PaymentProfileCustomerBase.Property_PaymentInfoId, paymentProfileCustomerObject.PaymentInfoId));
			AddParameter(cmd, pNVarChar(PaymentProfileCustomerBase.Property_Type, 50, paymentProfileCustomerObject.Type));
			AddParameter(cmd, pBool(PaymentProfileCustomerBase.Property_IsDefault, paymentProfileCustomerObject.IsDefault));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PaymentProfileCustomer
        /// </summary>
        /// <param name="paymentProfileCustomerObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PaymentProfileCustomerBase paymentProfileCustomerObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPAYMENTPROFILECUSTOMER);
	
				AddParameter(cmd, pInt32Out(PaymentProfileCustomerBase.Property_Id));
				AddCommonParams(cmd, paymentProfileCustomerObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					paymentProfileCustomerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					paymentProfileCustomerObject.Id = (Int32)GetOutParameter(cmd, PaymentProfileCustomerBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(paymentProfileCustomerObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PaymentProfileCustomer
        /// </summary>
        /// <param name="paymentProfileCustomerObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PaymentProfileCustomerBase paymentProfileCustomerObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPAYMENTPROFILECUSTOMER);
				
				AddParameter(cmd, pInt32(PaymentProfileCustomerBase.Property_Id, paymentProfileCustomerObject.Id));
				AddCommonParams(cmd, paymentProfileCustomerObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					paymentProfileCustomerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(paymentProfileCustomerObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PaymentProfileCustomer
        /// </summary>
        /// <param name="Id">Id of the PaymentProfileCustomer object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPAYMENTPROFILECUSTOMER);	
				
				AddParameter(cmd, pInt32(PaymentProfileCustomerBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PaymentProfileCustomer), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PaymentProfileCustomer object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PaymentProfileCustomer object to retrieve</param>
        /// <returns>PaymentProfileCustomer object, null if not found</returns>
		public PaymentProfileCustomer Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYMENTPROFILECUSTOMERBYID))
			{
				AddParameter( cmd, pInt32(PaymentProfileCustomerBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PaymentProfileCustomer objects 
        /// </summary>
        /// <returns>A list of PaymentProfileCustomer objects</returns>
		public PaymentProfileCustomerList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPAYMENTPROFILECUSTOMER))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PaymentProfileCustomer objects by PageRequest
        /// </summary>
        /// <returns>A list of PaymentProfileCustomer objects</returns>
		public PaymentProfileCustomerList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPAYMENTPROFILECUSTOMER))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PaymentProfileCustomerList _PaymentProfileCustomerList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PaymentProfileCustomerList;
			}
		}
		
		/// <summary>
        /// Retrieves all PaymentProfileCustomer objects by query String
        /// </summary>
        /// <returns>A list of PaymentProfileCustomer objects</returns>
		public PaymentProfileCustomerList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYMENTPROFILECUSTOMERBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get PaymentProfileCustomer Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of PaymentProfileCustomer
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYMENTPROFILECUSTOMERMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PaymentProfileCustomer Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PaymentProfileCustomer
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PaymentProfileCustomerRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYMENTPROFILECUSTOMERROWCOUNT))
			{
				SqlDataReader reader;
				_PaymentProfileCustomerRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PaymentProfileCustomerRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PaymentProfileCustomer object
        /// </summary>
        /// <param name="paymentProfileCustomerObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PaymentProfileCustomerBase paymentProfileCustomerObject, SqlDataReader reader, int start)
		{
			
				paymentProfileCustomerObject.Id = reader.GetInt32( start + 0 );			
				paymentProfileCustomerObject.CompanyId = reader.GetGuid( start + 1 );			
				paymentProfileCustomerObject.CustomerId = reader.GetGuid( start + 2 );			
				paymentProfileCustomerObject.PaymentInfoId = reader.GetInt32( start + 3 );			
				if(!reader.IsDBNull(4)) paymentProfileCustomerObject.Type = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) paymentProfileCustomerObject.IsDefault = reader.GetBoolean( start + 5 );			
			FillBaseObject(paymentProfileCustomerObject, reader, (start + 6));

			
			paymentProfileCustomerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PaymentProfileCustomer object
        /// </summary>
        /// <param name="paymentProfileCustomerObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PaymentProfileCustomerBase paymentProfileCustomerObject, SqlDataReader reader)
		{
			FillObject(paymentProfileCustomerObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PaymentProfileCustomer object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PaymentProfileCustomer object</returns>
		private PaymentProfileCustomer GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PaymentProfileCustomer paymentProfileCustomerObject= new PaymentProfileCustomer();
					FillObject(paymentProfileCustomerObject, reader);
					return paymentProfileCustomerObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PaymentProfileCustomer objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PaymentProfileCustomer objects</returns>
		private PaymentProfileCustomerList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PaymentProfileCustomer list
			PaymentProfileCustomerList list = new PaymentProfileCustomerList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PaymentProfileCustomer paymentProfileCustomerObject = new PaymentProfileCustomer();
					FillObject(paymentProfileCustomerObject, reader);

					list.Add(paymentProfileCustomerObject);
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
