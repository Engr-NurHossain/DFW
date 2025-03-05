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
	public partial class BillPaymentDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTBILLPAYMENT = "InsertBillPayment";
		private const string UPDATEBILLPAYMENT = "UpdateBillPayment";
		private const string DELETEBILLPAYMENT = "DeleteBillPayment";
		private const string GETBILLPAYMENTBYID = "GetBillPaymentById";
		private const string GETALLBILLPAYMENT = "GetAllBillPayment";
		private const string GETPAGEDBILLPAYMENT = "GetPagedBillPayment";
		private const string GETBILLPAYMENTMAXIMUMID = "GetBillPaymentMaximumId";
		private const string GETBILLPAYMENTROWCOUNT = "GetBillPaymentRowCount";	
		private const string GETBILLPAYMENTBYQUERY = "GetBillPaymentByQuery";
		#endregion
		
		#region Constructors
		public BillPaymentDataAccess(ClientContext context) : base(context) { }
		public BillPaymentDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="billPaymentObject"></param>
		private void AddCommonParams(SqlCommand cmd, BillPaymentBase billPaymentObject)
		{	
			AddParameter(cmd, pGuid(BillPaymentBase.Property_CompanyId, billPaymentObject.CompanyId));
			AddParameter(cmd, pGuid(BillPaymentBase.Property_CustomerId, billPaymentObject.CustomerId));
			AddParameter(cmd, pNVarChar(BillPaymentBase.Property_Type, 50, billPaymentObject.Type));
			AddParameter(cmd, pDouble(BillPaymentBase.Property_Amount, billPaymentObject.Amount));
			AddParameter(cmd, pNVarChar(BillPaymentBase.Property_Status, 50, billPaymentObject.Status));
			AddParameter(cmd, pDateTime(BillPaymentBase.Property_TransacationDate, billPaymentObject.TransacationDate));
			AddParameter(cmd, pNVarChar(BillPaymentBase.Property_PaymentMethod, 50, billPaymentObject.PaymentMethod));
			AddParameter(cmd, pNVarChar(BillPaymentBase.Property_ReferenceNo, 150, billPaymentObject.ReferenceNo));
			AddParameter(cmd, pNVarChar(BillPaymentBase.Property_AddedBy, 50, billPaymentObject.AddedBy));
			AddParameter(cmd, pDateTime(BillPaymentBase.Property_AddedDate, billPaymentObject.AddedDate));
			AddParameter(cmd, pInt32(BillPaymentBase.Property_PaymentInfoId, billPaymentObject.PaymentInfoId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts BillPayment
        /// </summary>
        /// <param name="billPaymentObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(BillPaymentBase billPaymentObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTBILLPAYMENT);
	
				AddParameter(cmd, pInt32Out(BillPaymentBase.Property_Id));
				AddCommonParams(cmd, billPaymentObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					billPaymentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					billPaymentObject.Id = (Int32)GetOutParameter(cmd, BillPaymentBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(billPaymentObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates BillPayment
        /// </summary>
        /// <param name="billPaymentObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(BillPaymentBase billPaymentObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEBILLPAYMENT);
				
				AddParameter(cmd, pInt32(BillPaymentBase.Property_Id, billPaymentObject.Id));
				AddCommonParams(cmd, billPaymentObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					billPaymentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(billPaymentObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes BillPayment
        /// </summary>
        /// <param name="Id">Id of the BillPayment object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEBILLPAYMENT);	
				
				AddParameter(cmd, pInt32(BillPaymentBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(BillPayment), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves BillPayment object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the BillPayment object to retrieve</param>
        /// <returns>BillPayment object, null if not found</returns>
		public BillPayment Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETBILLPAYMENTBYID))
			{
				AddParameter( cmd, pInt32(BillPaymentBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all BillPayment objects 
        /// </summary>
        /// <returns>A list of BillPayment objects</returns>
		public BillPaymentList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLBILLPAYMENT))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all BillPayment objects by PageRequest
        /// </summary>
        /// <returns>A list of BillPayment objects</returns>
		public BillPaymentList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDBILLPAYMENT))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				BillPaymentList _BillPaymentList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _BillPaymentList;
			}
		}
		
		/// <summary>
        /// Retrieves all BillPayment objects by query String
        /// </summary>
        /// <returns>A list of BillPayment objects</returns>
		public BillPaymentList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETBILLPAYMENTBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get BillPayment Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of BillPayment
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETBILLPAYMENTMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get BillPayment Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of BillPayment
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _BillPaymentRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETBILLPAYMENTROWCOUNT))
			{
				SqlDataReader reader;
				_BillPaymentRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _BillPaymentRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills BillPayment object
        /// </summary>
        /// <param name="billPaymentObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(BillPaymentBase billPaymentObject, SqlDataReader reader, int start)
		{
			
				billPaymentObject.Id = reader.GetInt32( start + 0 );			
				billPaymentObject.CompanyId = reader.GetGuid( start + 1 );			
				billPaymentObject.CustomerId = reader.GetGuid( start + 2 );			
				billPaymentObject.Type = reader.GetString( start + 3 );			
				billPaymentObject.Amount = reader.GetDouble( start + 4 );			
				billPaymentObject.Status = reader.GetString( start + 5 );			
				billPaymentObject.TransacationDate = reader.GetDateTime( start + 6 );			
				if(!reader.IsDBNull(7)) billPaymentObject.PaymentMethod = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) billPaymentObject.ReferenceNo = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) billPaymentObject.AddedBy = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) billPaymentObject.AddedDate = reader.GetDateTime( start + 10 );			
				if(!reader.IsDBNull(11)) billPaymentObject.PaymentInfoId = reader.GetInt32( start + 11 );			
			FillBaseObject(billPaymentObject, reader, (start + 12));

			
			billPaymentObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills BillPayment object
        /// </summary>
        /// <param name="billPaymentObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(BillPaymentBase billPaymentObject, SqlDataReader reader)
		{
			FillObject(billPaymentObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves BillPayment object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>BillPayment object</returns>
		private BillPayment GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					BillPayment billPaymentObject= new BillPayment();
					FillObject(billPaymentObject, reader);
					return billPaymentObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of BillPayment objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of BillPayment objects</returns>
		private BillPaymentList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//BillPayment list
			BillPaymentList list = new BillPaymentList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					BillPayment billPaymentObject = new BillPayment();
					FillObject(billPaymentObject, reader);

					list.Add(billPaymentObject);
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
