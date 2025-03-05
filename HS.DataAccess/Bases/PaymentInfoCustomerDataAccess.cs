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
	public partial class PaymentInfoCustomerDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPAYMENTINFOCUSTOMER = "InsertPaymentInfoCustomer";
		private const string UPDATEPAYMENTINFOCUSTOMER = "UpdatePaymentInfoCustomer";
		private const string DELETEPAYMENTINFOCUSTOMER = "DeletePaymentInfoCustomer";
		private const string GETPAYMENTINFOCUSTOMERBYID = "GetPaymentInfoCustomerById";
		private const string GETALLPAYMENTINFOCUSTOMER = "GetAllPaymentInfoCustomer";
		private const string GETPAGEDPAYMENTINFOCUSTOMER = "GetPagedPaymentInfoCustomer";
		private const string GETPAYMENTINFOCUSTOMERMAXIMUMID = "GetPaymentInfoCustomerMaximumId";
		private const string GETPAYMENTINFOCUSTOMERROWCOUNT = "GetPaymentInfoCustomerRowCount";	
		private const string GETPAYMENTINFOCUSTOMERBYQUERY = "GetPaymentInfoCustomerByQuery";
		#endregion
		
		#region Constructors
		public PaymentInfoCustomerDataAccess(ClientContext context) : base(context) { }
		public PaymentInfoCustomerDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="paymentInfoCustomerObject"></param>
		private void AddCommonParams(SqlCommand cmd, PaymentInfoCustomerBase paymentInfoCustomerObject)
		{	
			AddParameter(cmd, pGuid(PaymentInfoCustomerBase.Property_CompanyId, paymentInfoCustomerObject.CompanyId));
			AddParameter(cmd, pGuid(PaymentInfoCustomerBase.Property_CustomerId, paymentInfoCustomerObject.CustomerId));
			AddParameter(cmd, pInt32(PaymentInfoCustomerBase.Property_PaymentInfoId, paymentInfoCustomerObject.PaymentInfoId));
			AddParameter(cmd, pNVarChar(PaymentInfoCustomerBase.Property_Type, 50, paymentInfoCustomerObject.Type));
			AddParameter(cmd, pNVarChar(PaymentInfoCustomerBase.Property_Payfor, 50, paymentInfoCustomerObject.Payfor));
			AddParameter(cmd, pBool(PaymentInfoCustomerBase.Property_IsPaid, paymentInfoCustomerObject.IsPaid));
			AddParameter(cmd, pDateTime(PaymentInfoCustomerBase.Property_PaymentDate, paymentInfoCustomerObject.PaymentDate));
			AddParameter(cmd, pInt32(PaymentInfoCustomerBase.Property_ForMonths, paymentInfoCustomerObject.ForMonths));
			AddParameter(cmd, pNVarChar(PaymentInfoCustomerBase.Property_InvoiceId, 50, paymentInfoCustomerObject.InvoiceId));
			AddParameter(cmd, pNVarChar(PaymentInfoCustomerBase.Property_Comment, 500, paymentInfoCustomerObject.Comment));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PaymentInfoCustomer
        /// </summary>
        /// <param name="paymentInfoCustomerObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PaymentInfoCustomerBase paymentInfoCustomerObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPAYMENTINFOCUSTOMER);
	
				AddParameter(cmd, pInt32Out(PaymentInfoCustomerBase.Property_Id));
				AddCommonParams(cmd, paymentInfoCustomerObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					paymentInfoCustomerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					paymentInfoCustomerObject.Id = (Int32)GetOutParameter(cmd, PaymentInfoCustomerBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(paymentInfoCustomerObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PaymentInfoCustomer
        /// </summary>
        /// <param name="paymentInfoCustomerObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PaymentInfoCustomerBase paymentInfoCustomerObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPAYMENTINFOCUSTOMER);
				
				AddParameter(cmd, pInt32(PaymentInfoCustomerBase.Property_Id, paymentInfoCustomerObject.Id));
				AddCommonParams(cmd, paymentInfoCustomerObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					paymentInfoCustomerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(paymentInfoCustomerObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PaymentInfoCustomer
        /// </summary>
        /// <param name="Id">Id of the PaymentInfoCustomer object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPAYMENTINFOCUSTOMER);	
				
				AddParameter(cmd, pInt32(PaymentInfoCustomerBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PaymentInfoCustomer), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PaymentInfoCustomer object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PaymentInfoCustomer object to retrieve</param>
        /// <returns>PaymentInfoCustomer object, null if not found</returns>
		public PaymentInfoCustomer Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYMENTINFOCUSTOMERBYID))
			{
				AddParameter( cmd, pInt32(PaymentInfoCustomerBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PaymentInfoCustomer objects 
        /// </summary>
        /// <returns>A list of PaymentInfoCustomer objects</returns>
		public PaymentInfoCustomerList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPAYMENTINFOCUSTOMER))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PaymentInfoCustomer objects by PageRequest
        /// </summary>
        /// <returns>A list of PaymentInfoCustomer objects</returns>
		public PaymentInfoCustomerList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPAYMENTINFOCUSTOMER))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PaymentInfoCustomerList _PaymentInfoCustomerList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PaymentInfoCustomerList;
			}
		}
        /// <summary>
        /// Retrieves all PaymentInfoCustomer objects by query String
        /// </summary>
        /// <returns>A list of PaymentInfoCustomer objects</returns>
        public PaymentInfoCustomerList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYMENTINFOCUSTOMERBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get PaymentInfoCustomer Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of PaymentInfoCustomer
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYMENTINFOCUSTOMERMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PaymentInfoCustomer Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PaymentInfoCustomer
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PaymentInfoCustomerRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYMENTINFOCUSTOMERROWCOUNT))
			{
				SqlDataReader reader;
				_PaymentInfoCustomerRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PaymentInfoCustomerRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PaymentInfoCustomer object
        /// </summary>
        /// <param name="paymentInfoCustomerObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PaymentInfoCustomerBase paymentInfoCustomerObject, SqlDataReader reader, int start)
		{
			
				paymentInfoCustomerObject.Id = reader.GetInt32( start + 0 );			
				paymentInfoCustomerObject.CompanyId = reader.GetGuid( start + 1 );			
				paymentInfoCustomerObject.CustomerId = reader.GetGuid( start + 2 );			
				paymentInfoCustomerObject.PaymentInfoId = reader.GetInt32( start + 3 );			
				if(!reader.IsDBNull(4)) paymentInfoCustomerObject.Type = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) paymentInfoCustomerObject.Payfor = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) paymentInfoCustomerObject.IsPaid = reader.GetBoolean( start + 6 );			
				if(!reader.IsDBNull(7)) paymentInfoCustomerObject.PaymentDate = reader.GetDateTime( start + 7 );			
				if(!reader.IsDBNull(8)) paymentInfoCustomerObject.ForMonths = reader.GetInt32( start + 8 );			
				if(!reader.IsDBNull(9)) paymentInfoCustomerObject.InvoiceId = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) paymentInfoCustomerObject.Comment = reader.GetString( start + 10 );			
			FillBaseObject(paymentInfoCustomerObject, reader, (start + 11));

			
			paymentInfoCustomerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PaymentInfoCustomer object
        /// </summary>
        /// <param name="paymentInfoCustomerObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PaymentInfoCustomerBase paymentInfoCustomerObject, SqlDataReader reader)
		{
			FillObject(paymentInfoCustomerObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PaymentInfoCustomer object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PaymentInfoCustomer object</returns>
		private PaymentInfoCustomer GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PaymentInfoCustomer paymentInfoCustomerObject= new PaymentInfoCustomer();
					FillObject(paymentInfoCustomerObject, reader);
					return paymentInfoCustomerObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PaymentInfoCustomer objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PaymentInfoCustomer objects</returns>
		private PaymentInfoCustomerList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PaymentInfoCustomer list
			PaymentInfoCustomerList list = new PaymentInfoCustomerList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PaymentInfoCustomer paymentInfoCustomerObject = new PaymentInfoCustomer();
					FillObject(paymentInfoCustomerObject, reader);

					list.Add(paymentInfoCustomerObject);
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
