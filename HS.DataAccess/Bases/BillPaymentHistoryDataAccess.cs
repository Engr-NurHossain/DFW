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
	public partial class BillPaymentHistoryDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTBILLPAYMENTHISTORY = "InsertBillPaymentHistory";
		private const string UPDATEBILLPAYMENTHISTORY = "UpdateBillPaymentHistory";
		private const string DELETEBILLPAYMENTHISTORY = "DeleteBillPaymentHistory";
		private const string GETBILLPAYMENTHISTORYBYID = "GetBillPaymentHistoryById";
		private const string GETALLBILLPAYMENTHISTORY = "GetAllBillPaymentHistory";
		private const string GETPAGEDBILLPAYMENTHISTORY = "GetPagedBillPaymentHistory";
		private const string GETBILLPAYMENTHISTORYMAXIMUMID = "GetBillPaymentHistoryMaximumId";
		private const string GETBILLPAYMENTHISTORYROWCOUNT = "GetBillPaymentHistoryRowCount";	
		private const string GETBILLPAYMENTHISTORYBYQUERY = "GetBillPaymentHistoryByQuery";
		#endregion
		
		#region Constructors
		public BillPaymentHistoryDataAccess(ClientContext context) : base(context) { }
		public BillPaymentHistoryDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="billPaymentHistoryObject"></param>
		private void AddCommonParams(SqlCommand cmd, BillPaymentHistoryBase billPaymentHistoryObject)
		{	
			AddParameter(cmd, pInt32(BillPaymentHistoryBase.Property_BillPaymentId, billPaymentHistoryObject.BillPaymentId));
			AddParameter(cmd, pInt32(BillPaymentHistoryBase.Property_InvoiceId, billPaymentHistoryObject.InvoiceId));
			AddParameter(cmd, pDouble(BillPaymentHistoryBase.Property_Amount, billPaymentHistoryObject.Amount));
			AddParameter(cmd, pDouble(BillPaymentHistoryBase.Property_Balance, billPaymentHistoryObject.Balance));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts BillPaymentHistory
        /// </summary>
        /// <param name="billPaymentHistoryObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(BillPaymentHistoryBase billPaymentHistoryObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTBILLPAYMENTHISTORY);
	
				AddParameter(cmd, pInt32Out(BillPaymentHistoryBase.Property_Id));
				AddCommonParams(cmd, billPaymentHistoryObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					billPaymentHistoryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					billPaymentHistoryObject.Id = (Int32)GetOutParameter(cmd, BillPaymentHistoryBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(billPaymentHistoryObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates BillPaymentHistory
        /// </summary>
        /// <param name="billPaymentHistoryObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(BillPaymentHistoryBase billPaymentHistoryObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEBILLPAYMENTHISTORY);
				
				AddParameter(cmd, pInt32(BillPaymentHistoryBase.Property_Id, billPaymentHistoryObject.Id));
				AddCommonParams(cmd, billPaymentHistoryObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					billPaymentHistoryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(billPaymentHistoryObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes BillPaymentHistory
        /// </summary>
        /// <param name="Id">Id of the BillPaymentHistory object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEBILLPAYMENTHISTORY);	
				
				AddParameter(cmd, pInt32(BillPaymentHistoryBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(BillPaymentHistory), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves BillPaymentHistory object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the BillPaymentHistory object to retrieve</param>
        /// <returns>BillPaymentHistory object, null if not found</returns>
		public BillPaymentHistory Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETBILLPAYMENTHISTORYBYID))
			{
				AddParameter( cmd, pInt32(BillPaymentHistoryBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all BillPaymentHistory objects 
        /// </summary>
        /// <returns>A list of BillPaymentHistory objects</returns>
		public BillPaymentHistoryList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLBILLPAYMENTHISTORY))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all BillPaymentHistory objects by PageRequest
        /// </summary>
        /// <returns>A list of BillPaymentHistory objects</returns>
		public BillPaymentHistoryList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDBILLPAYMENTHISTORY))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				BillPaymentHistoryList _BillPaymentHistoryList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _BillPaymentHistoryList;
			}
		}
		
		/// <summary>
        /// Retrieves all BillPaymentHistory objects by query String
        /// </summary>
        /// <returns>A list of BillPaymentHistory objects</returns>
		public BillPaymentHistoryList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETBILLPAYMENTHISTORYBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get BillPaymentHistory Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of BillPaymentHistory
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETBILLPAYMENTHISTORYMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get BillPaymentHistory Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of BillPaymentHistory
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _BillPaymentHistoryRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETBILLPAYMENTHISTORYROWCOUNT))
			{
				SqlDataReader reader;
				_BillPaymentHistoryRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _BillPaymentHistoryRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills BillPaymentHistory object
        /// </summary>
        /// <param name="billPaymentHistoryObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(BillPaymentHistoryBase billPaymentHistoryObject, SqlDataReader reader, int start)
		{
			
				billPaymentHistoryObject.Id = reader.GetInt32( start + 0 );			
				billPaymentHistoryObject.BillPaymentId = reader.GetInt32( start + 1 );			
				billPaymentHistoryObject.InvoiceId = reader.GetInt32( start + 2 );			
				billPaymentHistoryObject.Amount = reader.GetDouble( start + 3 );			
				billPaymentHistoryObject.Balance = reader.GetDouble( start + 4 );			
			FillBaseObject(billPaymentHistoryObject, reader, (start + 5));

			
			billPaymentHistoryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills BillPaymentHistory object
        /// </summary>
        /// <param name="billPaymentHistoryObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(BillPaymentHistoryBase billPaymentHistoryObject, SqlDataReader reader)
		{
			FillObject(billPaymentHistoryObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves BillPaymentHistory object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>BillPaymentHistory object</returns>
		private BillPaymentHistory GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					BillPaymentHistory billPaymentHistoryObject= new BillPaymentHistory();
					FillObject(billPaymentHistoryObject, reader);
					return billPaymentHistoryObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of BillPaymentHistory objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of BillPaymentHistory objects</returns>
		private BillPaymentHistoryList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//BillPaymentHistory list
			BillPaymentHistoryList list = new BillPaymentHistoryList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					BillPaymentHistory billPaymentHistoryObject = new BillPaymentHistory();
					FillObject(billPaymentHistoryObject, reader);

					list.Add(billPaymentHistoryObject);
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
