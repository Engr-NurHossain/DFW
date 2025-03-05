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
	public partial class PaymentInfoSupplierDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPAYMENTINFOSUPPLIER = "InsertPaymentInfoSupplier";
		private const string UPDATEPAYMENTINFOSUPPLIER = "UpdatePaymentInfoSupplier";
		private const string DELETEPAYMENTINFOSUPPLIER = "DeletePaymentInfoSupplier";
		private const string GETPAYMENTINFOSUPPLIERBYID = "GetPaymentInfoSupplierById";
		private const string GETALLPAYMENTINFOSUPPLIER = "GetAllPaymentInfoSupplier";
		private const string GETPAGEDPAYMENTINFOSUPPLIER = "GetPagedPaymentInfoSupplier";
		private const string GETPAYMENTINFOSUPPLIERMAXIMUMID = "GetPaymentInfoSupplierMaximumId";
		private const string GETPAYMENTINFOSUPPLIERROWCOUNT = "GetPaymentInfoSupplierRowCount";	
		private const string GETPAYMENTINFOSUPPLIERBYQUERY = "GetPaymentInfoSupplierByQuery";
		#endregion
		
		#region Constructors
		public PaymentInfoSupplierDataAccess(ClientContext context) : base(context) { }
		public PaymentInfoSupplierDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="paymentInfoSupplierObject"></param>
		private void AddCommonParams(SqlCommand cmd, PaymentInfoSupplierBase paymentInfoSupplierObject)
		{	
			AddParameter(cmd, pGuid(PaymentInfoSupplierBase.Property_CompanyId, paymentInfoSupplierObject.CompanyId));
			AddParameter(cmd, pInt32(PaymentInfoSupplierBase.Property_SupplierId, paymentInfoSupplierObject.SupplierId));
			AddParameter(cmd, pInt32(PaymentInfoSupplierBase.Property_PaymentInfoId, paymentInfoSupplierObject.PaymentInfoId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PaymentInfoSupplier
        /// </summary>
        /// <param name="paymentInfoSupplierObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PaymentInfoSupplierBase paymentInfoSupplierObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPAYMENTINFOSUPPLIER);
	
				AddParameter(cmd, pInt32Out(PaymentInfoSupplierBase.Property_Id));
				AddCommonParams(cmd, paymentInfoSupplierObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					paymentInfoSupplierObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					paymentInfoSupplierObject.Id = (Int32)GetOutParameter(cmd, PaymentInfoSupplierBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(paymentInfoSupplierObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PaymentInfoSupplier
        /// </summary>
        /// <param name="paymentInfoSupplierObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PaymentInfoSupplierBase paymentInfoSupplierObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPAYMENTINFOSUPPLIER);
				
				AddParameter(cmd, pInt32(PaymentInfoSupplierBase.Property_Id, paymentInfoSupplierObject.Id));
				AddCommonParams(cmd, paymentInfoSupplierObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					paymentInfoSupplierObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(paymentInfoSupplierObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PaymentInfoSupplier
        /// </summary>
        /// <param name="Id">Id of the PaymentInfoSupplier object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPAYMENTINFOSUPPLIER);	
				
				AddParameter(cmd, pInt32(PaymentInfoSupplierBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PaymentInfoSupplier), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PaymentInfoSupplier object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PaymentInfoSupplier object to retrieve</param>
        /// <returns>PaymentInfoSupplier object, null if not found</returns>
		public PaymentInfoSupplier Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYMENTINFOSUPPLIERBYID))
			{
				AddParameter( cmd, pInt32(PaymentInfoSupplierBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PaymentInfoSupplier objects 
        /// </summary>
        /// <returns>A list of PaymentInfoSupplier objects</returns>
		public PaymentInfoSupplierList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPAYMENTINFOSUPPLIER))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PaymentInfoSupplier objects by PageRequest
        /// </summary>
        /// <returns>A list of PaymentInfoSupplier objects</returns>
		public PaymentInfoSupplierList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPAYMENTINFOSUPPLIER))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PaymentInfoSupplierList _PaymentInfoSupplierList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PaymentInfoSupplierList;
			}
		}
		
		/// <summary>
        /// Retrieves all PaymentInfoSupplier objects by query String
        /// </summary>
        /// <returns>A list of PaymentInfoSupplier objects</returns>
		public PaymentInfoSupplierList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYMENTINFOSUPPLIERBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get PaymentInfoSupplier Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of PaymentInfoSupplier
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYMENTINFOSUPPLIERMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PaymentInfoSupplier Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PaymentInfoSupplier
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PaymentInfoSupplierRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYMENTINFOSUPPLIERROWCOUNT))
			{
				SqlDataReader reader;
				_PaymentInfoSupplierRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PaymentInfoSupplierRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PaymentInfoSupplier object
        /// </summary>
        /// <param name="paymentInfoSupplierObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PaymentInfoSupplierBase paymentInfoSupplierObject, SqlDataReader reader, int start)
		{
			
				paymentInfoSupplierObject.Id = reader.GetInt32( start + 0 );			
				paymentInfoSupplierObject.CompanyId = reader.GetGuid( start + 1 );			
				paymentInfoSupplierObject.SupplierId = reader.GetInt32( start + 2 );			
				paymentInfoSupplierObject.PaymentInfoId = reader.GetInt32( start + 3 );			
			FillBaseObject(paymentInfoSupplierObject, reader, (start + 4));

			
			paymentInfoSupplierObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PaymentInfoSupplier object
        /// </summary>
        /// <param name="paymentInfoSupplierObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PaymentInfoSupplierBase paymentInfoSupplierObject, SqlDataReader reader)
		{
			FillObject(paymentInfoSupplierObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PaymentInfoSupplier object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PaymentInfoSupplier object</returns>
		private PaymentInfoSupplier GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PaymentInfoSupplier paymentInfoSupplierObject= new PaymentInfoSupplier();
					FillObject(paymentInfoSupplierObject, reader);
					return paymentInfoSupplierObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PaymentInfoSupplier objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PaymentInfoSupplier objects</returns>
		private PaymentInfoSupplierList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PaymentInfoSupplier list
			PaymentInfoSupplierList list = new PaymentInfoSupplierList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PaymentInfoSupplier paymentInfoSupplierObject = new PaymentInfoSupplier();
					FillObject(paymentInfoSupplierObject, reader);

					list.Add(paymentInfoSupplierObject);
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
