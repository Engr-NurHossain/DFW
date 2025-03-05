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
	public partial class PaymentRevenueDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPAYMENTREVENUE = "InsertPaymentRevenue";
		private const string UPDATEPAYMENTREVENUE = "UpdatePaymentRevenue";
		private const string DELETEPAYMENTREVENUE = "DeletePaymentRevenue";
		private const string GETPAYMENTREVENUEBYID = "GetPaymentRevenueById";
		private const string GETALLPAYMENTREVENUE = "GetAllPaymentRevenue";
		private const string GETPAGEDPAYMENTREVENUE = "GetPagedPaymentRevenue";
		private const string GETPAYMENTREVENUEMAXIMUMID = "GetPaymentRevenueMaximumId";
		private const string GETPAYMENTREVENUEROWCOUNT = "GetPaymentRevenueRowCount";	
		private const string GETPAYMENTREVENUEBYQUERY = "GetPaymentRevenueByQuery";
		#endregion
		
		#region Constructors
		public PaymentRevenueDataAccess(ClientContext context) : base(context) { }
		public PaymentRevenueDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="paymentRevenueObject"></param>
		private void AddCommonParams(SqlCommand cmd, PaymentRevenueBase paymentRevenueObject)
		{	
			AddParameter(cmd, pGuid(PaymentRevenueBase.Property_CompanyId, paymentRevenueObject.CompanyId));
			AddParameter(cmd, pGuid(PaymentRevenueBase.Property_CustomerId, paymentRevenueObject.CustomerId));
			AddParameter(cmd, pNVarChar(PaymentRevenueBase.Property_Type, 50, paymentRevenueObject.Type));
			AddParameter(cmd, pDouble(PaymentRevenueBase.Property_Amount, paymentRevenueObject.Amount));
			AddParameter(cmd, pNVarChar(PaymentRevenueBase.Property_Status, 50, paymentRevenueObject.Status));
			AddParameter(cmd, pDateTime(PaymentRevenueBase.Property_TransacationDate, paymentRevenueObject.TransacationDate));
			AddParameter(cmd, pNVarChar(PaymentRevenueBase.Property_CardTransactionId, 50, paymentRevenueObject.CardTransactionId));
			AddParameter(cmd, pNVarChar(PaymentRevenueBase.Property_PaymentMethod, 50, paymentRevenueObject.PaymentMethod));
			AddParameter(cmd, pNVarChar(PaymentRevenueBase.Property_ReferenceNo, 150, paymentRevenueObject.ReferenceNo));
			AddParameter(cmd, pNVarChar(PaymentRevenueBase.Property_AddedBy, 50, paymentRevenueObject.AddedBy));
			AddParameter(cmd, pDateTime(PaymentRevenueBase.Property_AddedDate, paymentRevenueObject.AddedDate));
			AddParameter(cmd, pInt32(PaymentRevenueBase.Property_PaymentInfoId, paymentRevenueObject.PaymentInfoId));
			AddParameter(cmd, pNVarChar(PaymentRevenueBase.Property_Desccription, paymentRevenueObject.Desccription));
			AddParameter(cmd, pInt32(PaymentRevenueBase.Property_WorkOrder, paymentRevenueObject.WorkOrder));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PaymentRevenue
        /// </summary>
        /// <param name="paymentRevenueObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PaymentRevenueBase paymentRevenueObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPAYMENTREVENUE);
	
				AddParameter(cmd, pInt32Out(PaymentRevenueBase.Property_Id));
				AddCommonParams(cmd, paymentRevenueObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					paymentRevenueObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					paymentRevenueObject.Id = (Int32)GetOutParameter(cmd, PaymentRevenueBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(paymentRevenueObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PaymentRevenue
        /// </summary>
        /// <param name="paymentRevenueObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PaymentRevenueBase paymentRevenueObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPAYMENTREVENUE);
				
				AddParameter(cmd, pInt32(PaymentRevenueBase.Property_Id, paymentRevenueObject.Id));
				AddCommonParams(cmd, paymentRevenueObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					paymentRevenueObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(paymentRevenueObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PaymentRevenue
        /// </summary>
        /// <param name="Id">Id of the PaymentRevenue object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPAYMENTREVENUE);	
				
				AddParameter(cmd, pInt32(PaymentRevenueBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PaymentRevenue), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PaymentRevenue object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PaymentRevenue object to retrieve</param>
        /// <returns>PaymentRevenue object, null if not found</returns>
		public PaymentRevenue Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYMENTREVENUEBYID))
			{
				AddParameter( cmd, pInt32(PaymentRevenueBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PaymentRevenue objects 
        /// </summary>
        /// <returns>A list of PaymentRevenue objects</returns>
		public PaymentRevenueList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPAYMENTREVENUE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PaymentRevenue objects by PageRequest
        /// </summary>
        /// <returns>A list of PaymentRevenue objects</returns>
		public PaymentRevenueList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPAYMENTREVENUE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PaymentRevenueList _PaymentRevenueList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PaymentRevenueList;
			}
		}
		
		/// <summary>
        /// Retrieves all PaymentRevenue objects by query String
        /// </summary>
        /// <returns>A list of PaymentRevenue objects</returns>
		public PaymentRevenueList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYMENTREVENUEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get PaymentRevenue Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of PaymentRevenue
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYMENTREVENUEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PaymentRevenue Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PaymentRevenue
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PaymentRevenueRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYMENTREVENUEROWCOUNT))
			{
				SqlDataReader reader;
				_PaymentRevenueRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PaymentRevenueRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PaymentRevenue object
        /// </summary>
        /// <param name="paymentRevenueObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PaymentRevenueBase paymentRevenueObject, SqlDataReader reader, int start)
		{
			
				paymentRevenueObject.Id = reader.GetInt32( start + 0 );			
				paymentRevenueObject.CompanyId = reader.GetGuid( start + 1 );			
				paymentRevenueObject.CustomerId = reader.GetGuid( start + 2 );			
				paymentRevenueObject.Type = reader.GetString( start + 3 );			
				paymentRevenueObject.Amount = reader.GetDouble( start + 4 );			
				paymentRevenueObject.Status = reader.GetString( start + 5 );			
				paymentRevenueObject.TransacationDate = reader.GetDateTime( start + 6 );			
				if(!reader.IsDBNull(7)) paymentRevenueObject.CardTransactionId = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) paymentRevenueObject.PaymentMethod = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) paymentRevenueObject.ReferenceNo = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) paymentRevenueObject.AddedBy = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) paymentRevenueObject.AddedDate = reader.GetDateTime( start + 11 );			
				if(!reader.IsDBNull(12)) paymentRevenueObject.PaymentInfoId = reader.GetInt32( start + 12 );			
				if(!reader.IsDBNull(13)) paymentRevenueObject.Desccription = reader.GetString( start + 13 );			
				if(!reader.IsDBNull(14)) paymentRevenueObject.WorkOrder = reader.GetInt32( start + 14 );			
			FillBaseObject(paymentRevenueObject, reader, (start + 15));

			
			paymentRevenueObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PaymentRevenue object
        /// </summary>
        /// <param name="paymentRevenueObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PaymentRevenueBase paymentRevenueObject, SqlDataReader reader)
		{
			FillObject(paymentRevenueObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PaymentRevenue object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PaymentRevenue object</returns>
		private PaymentRevenue GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PaymentRevenue paymentRevenueObject= new PaymentRevenue();
					FillObject(paymentRevenueObject, reader);
					return paymentRevenueObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PaymentRevenue objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PaymentRevenue objects</returns>
		private PaymentRevenueList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PaymentRevenue list
			PaymentRevenueList list = new PaymentRevenueList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PaymentRevenue paymentRevenueObject = new PaymentRevenue();
					FillObject(paymentRevenueObject, reader);

					list.Add(paymentRevenueObject);
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
