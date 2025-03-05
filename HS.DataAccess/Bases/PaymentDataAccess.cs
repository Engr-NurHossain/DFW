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
	public partial class PaymentDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPAYMENT = "InsertPayment";
		private const string UPDATEPAYMENT = "UpdatePayment";
		private const string DELETEPAYMENT = "DeletePayment";
		private const string GETPAYMENTBYID = "GetPaymentById";
		private const string GETALLPAYMENT = "GetAllPayment";
		private const string GETPAGEDPAYMENT = "GetPagedPayment";
		private const string GETPAYMENTMAXIMUMID = "GetPaymentMaximumId";
		private const string GETPAYMENTROWCOUNT = "GetPaymentRowCount";	
		private const string GETPAYMENTBYQUERY = "GetPaymentByQuery";
		#endregion
		
		#region Constructors
		public PaymentDataAccess(ClientContext context) : base(context) { }
		public PaymentDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="paymentObject"></param>
		private void AddCommonParams(SqlCommand cmd, PaymentBase paymentObject)
		{	
			AddParameter(cmd, pGuid(PaymentBase.Property_CompanyId, paymentObject.CompanyId));
			AddParameter(cmd, pGuid(PaymentBase.Property_InvoiceId, paymentObject.InvoiceId));
			AddParameter(cmd, pInt32(PaymentBase.Property_PaymentMethodId, paymentObject.PaymentMethodId));
			AddParameter(cmd, pDouble(PaymentBase.Property_Amount, paymentObject.Amount));
			AddParameter(cmd, pDateTime(PaymentBase.Property_PaymentDate, paymentObject.PaymentDate));
			AddParameter(cmd, pNVarChar(PaymentBase.Property_PaymentBy, 50, paymentObject.PaymentBy));
			AddParameter(cmd, pInt32(PaymentBase.Property_CreditCardId, paymentObject.CreditCardId));
			AddParameter(cmd, pNVarChar(PaymentBase.Property_ReferenceNo, 250, paymentObject.ReferenceNo));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts Payment
        /// </summary>
        /// <param name="paymentObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PaymentBase paymentObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPAYMENT);
	
				AddParameter(cmd, pInt32Out(PaymentBase.Property_Id));
				AddCommonParams(cmd, paymentObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					paymentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					paymentObject.Id = (Int32)GetOutParameter(cmd, PaymentBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(paymentObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates Payment
        /// </summary>
        /// <param name="paymentObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PaymentBase paymentObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPAYMENT);
				
				AddParameter(cmd, pInt32(PaymentBase.Property_Id, paymentObject.Id));
				AddCommonParams(cmd, paymentObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					paymentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(paymentObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes Payment
        /// </summary>
        /// <param name="Id">Id of the Payment object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPAYMENT);	
				
				AddParameter(cmd, pInt32(PaymentBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(Payment), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves Payment object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the Payment object to retrieve</param>
        /// <returns>Payment object, null if not found</returns>
		public Payment Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYMENTBYID))
			{
				AddParameter( cmd, pInt32(PaymentBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all Payment objects 
        /// </summary>
        /// <returns>A list of Payment objects</returns>
		public PaymentList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPAYMENT))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all Payment objects by PageRequest
        /// </summary>
        /// <returns>A list of Payment objects</returns>
		public PaymentList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPAYMENT))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PaymentList _PaymentList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PaymentList;
			}
		}
		
		/// <summary>
        /// Retrieves all Payment objects by query String
        /// </summary>
        /// <returns>A list of Payment objects</returns>
		public PaymentList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYMENTBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get Payment Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of Payment
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYMENTMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get Payment Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of Payment
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PaymentRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYMENTROWCOUNT))
			{
				SqlDataReader reader;
				_PaymentRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PaymentRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills Payment object
        /// </summary>
        /// <param name="paymentObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PaymentBase paymentObject, SqlDataReader reader, int start)
		{
			
				paymentObject.Id = reader.GetInt32( start + 0 );			
				paymentObject.CompanyId = reader.GetGuid( start + 1 );			
				paymentObject.InvoiceId = reader.GetGuid( start + 2 );			
				paymentObject.PaymentMethodId = reader.GetInt32( start + 3 );			
				paymentObject.Amount = reader.GetDouble( start + 4 );			
				paymentObject.PaymentDate = reader.GetDateTime( start + 5 );			
				paymentObject.PaymentBy = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) paymentObject.CreditCardId = reader.GetInt32( start + 7 );			
				if(!reader.IsDBNull(8)) paymentObject.ReferenceNo = reader.GetString( start + 8 );			
			FillBaseObject(paymentObject, reader, (start + 9));

			
			paymentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills Payment object
        /// </summary>
        /// <param name="paymentObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PaymentBase paymentObject, SqlDataReader reader)
		{
			FillObject(paymentObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves Payment object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>Payment object</returns>
		private Payment GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					Payment paymentObject= new Payment();
					FillObject(paymentObject, reader);
					return paymentObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of Payment objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of Payment objects</returns>
		private PaymentList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//Payment list
			PaymentList list = new PaymentList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					Payment paymentObject = new Payment();
					FillObject(paymentObject, reader);

					list.Add(paymentObject);
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
