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
	public partial class PaymentInfoCustomerDraftDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPAYMENTINFOCUSTOMERDRAFT = "InsertPaymentInfoCustomerDraft";
		private const string UPDATEPAYMENTINFOCUSTOMERDRAFT = "UpdatePaymentInfoCustomerDraft";
		private const string DELETEPAYMENTINFOCUSTOMERDRAFT = "DeletePaymentInfoCustomerDraft";
		private const string GETPAYMENTINFOCUSTOMERDRAFTBYID = "GetPaymentInfoCustomerDraftById";
		private const string GETALLPAYMENTINFOCUSTOMERDRAFT = "GetAllPaymentInfoCustomerDraft";
		private const string GETPAGEDPAYMENTINFOCUSTOMERDRAFT = "GetPagedPaymentInfoCustomerDraft";
		private const string GETPAYMENTINFOCUSTOMERDRAFTMAXIMUMID = "GetPaymentInfoCustomerDraftMaximumId";
		private const string GETPAYMENTINFOCUSTOMERDRAFTROWCOUNT = "GetPaymentInfoCustomerDraftRowCount";	
		private const string GETPAYMENTINFOCUSTOMERDRAFTBYQUERY = "GetPaymentInfoCustomerDraftByQuery";
		#endregion
		
		#region Constructors
		public PaymentInfoCustomerDraftDataAccess(ClientContext context) : base(context) { }
		public PaymentInfoCustomerDraftDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="paymentInfoCustomerDraftObject"></param>
		private void AddCommonParams(SqlCommand cmd, PaymentInfoCustomerDraftBase paymentInfoCustomerDraftObject)
		{	
			AddParameter(cmd, pGuid(PaymentInfoCustomerDraftBase.Property_CompanyId, paymentInfoCustomerDraftObject.CompanyId));
			AddParameter(cmd, pGuid(PaymentInfoCustomerDraftBase.Property_CustomerId, paymentInfoCustomerDraftObject.CustomerId));
			AddParameter(cmd, pInt32(PaymentInfoCustomerDraftBase.Property_PaymentInfoId, paymentInfoCustomerDraftObject.PaymentInfoId));
			AddParameter(cmd, pNVarChar(PaymentInfoCustomerDraftBase.Property_Type, 50, paymentInfoCustomerDraftObject.Type));
			AddParameter(cmd, pNVarChar(PaymentInfoCustomerDraftBase.Property_Payfor, 50, paymentInfoCustomerDraftObject.Payfor));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PaymentInfoCustomerDraft
        /// </summary>
        /// <param name="paymentInfoCustomerDraftObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PaymentInfoCustomerDraftBase paymentInfoCustomerDraftObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPAYMENTINFOCUSTOMERDRAFT);
	
				AddParameter(cmd, pInt32Out(PaymentInfoCustomerDraftBase.Property_Id));
				AddCommonParams(cmd, paymentInfoCustomerDraftObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					paymentInfoCustomerDraftObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					paymentInfoCustomerDraftObject.Id = (Int32)GetOutParameter(cmd, PaymentInfoCustomerDraftBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(paymentInfoCustomerDraftObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PaymentInfoCustomerDraft
        /// </summary>
        /// <param name="paymentInfoCustomerDraftObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PaymentInfoCustomerDraftBase paymentInfoCustomerDraftObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPAYMENTINFOCUSTOMERDRAFT);
				
				AddParameter(cmd, pInt32(PaymentInfoCustomerDraftBase.Property_Id, paymentInfoCustomerDraftObject.Id));
				AddCommonParams(cmd, paymentInfoCustomerDraftObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					paymentInfoCustomerDraftObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(paymentInfoCustomerDraftObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PaymentInfoCustomerDraft
        /// </summary>
        /// <param name="Id">Id of the PaymentInfoCustomerDraft object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPAYMENTINFOCUSTOMERDRAFT);	
				
				AddParameter(cmd, pInt32(PaymentInfoCustomerDraftBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PaymentInfoCustomerDraft), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PaymentInfoCustomerDraft object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PaymentInfoCustomerDraft object to retrieve</param>
        /// <returns>PaymentInfoCustomerDraft object, null if not found</returns>
		public PaymentInfoCustomerDraft Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYMENTINFOCUSTOMERDRAFTBYID))
			{
				AddParameter( cmd, pInt32(PaymentInfoCustomerDraftBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PaymentInfoCustomerDraft objects 
        /// </summary>
        /// <returns>A list of PaymentInfoCustomerDraft objects</returns>
		public PaymentInfoCustomerDraftList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPAYMENTINFOCUSTOMERDRAFT))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PaymentInfoCustomerDraft objects by PageRequest
        /// </summary>
        /// <returns>A list of PaymentInfoCustomerDraft objects</returns>
		public PaymentInfoCustomerDraftList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPAYMENTINFOCUSTOMERDRAFT))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PaymentInfoCustomerDraftList _PaymentInfoCustomerDraftList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PaymentInfoCustomerDraftList;
			}
		}
		
		/// <summary>
        /// Retrieves all PaymentInfoCustomerDraft objects by query String
        /// </summary>
        /// <returns>A list of PaymentInfoCustomerDraft objects</returns>
		public PaymentInfoCustomerDraftList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYMENTINFOCUSTOMERDRAFTBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get PaymentInfoCustomerDraft Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of PaymentInfoCustomerDraft
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYMENTINFOCUSTOMERDRAFTMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PaymentInfoCustomerDraft Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PaymentInfoCustomerDraft
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PaymentInfoCustomerDraftRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYMENTINFOCUSTOMERDRAFTROWCOUNT))
			{
				SqlDataReader reader;
				_PaymentInfoCustomerDraftRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PaymentInfoCustomerDraftRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PaymentInfoCustomerDraft object
        /// </summary>
        /// <param name="paymentInfoCustomerDraftObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PaymentInfoCustomerDraftBase paymentInfoCustomerDraftObject, SqlDataReader reader, int start)
		{
			
				paymentInfoCustomerDraftObject.Id = reader.GetInt32( start + 0 );			
				paymentInfoCustomerDraftObject.CompanyId = reader.GetGuid( start + 1 );			
				paymentInfoCustomerDraftObject.CustomerId = reader.GetGuid( start + 2 );			
				paymentInfoCustomerDraftObject.PaymentInfoId = reader.GetInt32( start + 3 );			
				if(!reader.IsDBNull(4)) paymentInfoCustomerDraftObject.Type = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) paymentInfoCustomerDraftObject.Payfor = reader.GetString( start + 5 );			
			FillBaseObject(paymentInfoCustomerDraftObject, reader, (start + 6));

			
			paymentInfoCustomerDraftObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PaymentInfoCustomerDraft object
        /// </summary>
        /// <param name="paymentInfoCustomerDraftObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PaymentInfoCustomerDraftBase paymentInfoCustomerDraftObject, SqlDataReader reader)
		{
			FillObject(paymentInfoCustomerDraftObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PaymentInfoCustomerDraft object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PaymentInfoCustomerDraft object</returns>
		private PaymentInfoCustomerDraft GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PaymentInfoCustomerDraft paymentInfoCustomerDraftObject= new PaymentInfoCustomerDraft();
					FillObject(paymentInfoCustomerDraftObject, reader);
					return paymentInfoCustomerDraftObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PaymentInfoCustomerDraft objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PaymentInfoCustomerDraft objects</returns>
		private PaymentInfoCustomerDraftList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PaymentInfoCustomerDraft list
			PaymentInfoCustomerDraftList list = new PaymentInfoCustomerDraftList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PaymentInfoCustomerDraft paymentInfoCustomerDraftObject = new PaymentInfoCustomerDraft();
					FillObject(paymentInfoCustomerDraftObject, reader);

					list.Add(paymentInfoCustomerDraftObject);
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
