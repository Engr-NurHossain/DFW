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
	public partial class TransactionDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTTRANSACTION = "InsertTransaction";
		private const string UPDATETRANSACTION = "UpdateTransaction";
		private const string DELETETRANSACTION = "DeleteTransaction";
		private const string GETTRANSACTIONBYID = "GetTransactionById";
		private const string GETALLTRANSACTION = "GetAllTransaction";
		private const string GETPAGEDTRANSACTION = "GetPagedTransaction";
		private const string GETTRANSACTIONMAXIMUMID = "GetTransactionMaximumId";
		private const string GETTRANSACTIONROWCOUNT = "GetTransactionRowCount";	
		private const string GETTRANSACTIONBYQUERY = "GetTransactionByQuery";
		#endregion
		
		#region Constructors
		public TransactionDataAccess(ClientContext context) : base(context) { }
		public TransactionDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="transactionObject"></param>
		private void AddCommonParams(SqlCommand cmd, TransactionBase transactionObject)
		{	
			AddParameter(cmd, pGuid(TransactionBase.Property_CompanyId, transactionObject.CompanyId));
			AddParameter(cmd, pGuid(TransactionBase.Property_CustomerId, transactionObject.CustomerId));
			AddParameter(cmd, pNVarChar(TransactionBase.Property_Type, 50, transactionObject.Type));
			AddParameter(cmd, pDouble(TransactionBase.Property_Amount, transactionObject.Amount));
			AddParameter(cmd, pNVarChar(TransactionBase.Property_Status, 50, transactionObject.Status));
			AddParameter(cmd, pDateTime(TransactionBase.Property_TransacationDate, transactionObject.TransacationDate));
			AddParameter(cmd, pNVarChar(TransactionBase.Property_CardTransactionId, 50, transactionObject.CardTransactionId));
			AddParameter(cmd, pNVarChar(TransactionBase.Property_PaymentMethod, 50, transactionObject.PaymentMethod));
			AddParameter(cmd, pNVarChar(TransactionBase.Property_CheckNo, 150, transactionObject.CheckNo));
			AddParameter(cmd, pNVarChar(TransactionBase.Property_ReferenceNo, 150, transactionObject.ReferenceNo));
			AddParameter(cmd, pNVarChar(TransactionBase.Property_AddedBy, 50, transactionObject.AddedBy));
			AddParameter(cmd, pDateTime(TransactionBase.Property_AddedDate, transactionObject.AddedDate));
			AddParameter(cmd, pGuid(TransactionBase.Property_CreatedBy, transactionObject.CreatedBy));
			AddParameter(cmd, pInt32(TransactionBase.Property_PaymentInfoId, transactionObject.PaymentInfoId));
			AddParameter(cmd, pNVarChar(TransactionBase.Property_Note, transactionObject.Note));
			AddParameter(cmd, pInt32(TransactionBase.Property_PaymentProfileId, transactionObject.PaymentProfileId));
			AddParameter(cmd, pBool(TransactionBase.Property_IsRMR, transactionObject.IsRMR));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts Transaction
        /// </summary>
        /// <param name="transactionObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(TransactionBase transactionObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTTRANSACTION);
	
				AddParameter(cmd, pInt32Out(TransactionBase.Property_Id));
				AddCommonParams(cmd, transactionObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					transactionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					transactionObject.Id = (Int32)GetOutParameter(cmd, TransactionBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(transactionObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates Transaction
        /// </summary>
        /// <param name="transactionObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(TransactionBase transactionObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATETRANSACTION);
				
				AddParameter(cmd, pInt32(TransactionBase.Property_Id, transactionObject.Id));
				AddCommonParams(cmd, transactionObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					transactionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(transactionObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes Transaction
        /// </summary>
        /// <param name="Id">Id of the Transaction object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETETRANSACTION);	
				
				AddParameter(cmd, pInt32(TransactionBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(Transaction), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves Transaction object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the Transaction object to retrieve</param>
        /// <returns>Transaction object, null if not found</returns>
		public Transaction Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETTRANSACTIONBYID))
			{
				AddParameter( cmd, pInt32(TransactionBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all Transaction objects 
        /// </summary>
        /// <returns>A list of Transaction objects</returns>
		public TransactionList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLTRANSACTION))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all Transaction objects by PageRequest
        /// </summary>
        /// <returns>A list of Transaction objects</returns>
		public TransactionList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDTRANSACTION))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				TransactionList _TransactionList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _TransactionList;
			}
		}
		
		/// <summary>
        /// Retrieves all Transaction objects by query String
        /// </summary>
        /// <returns>A list of Transaction objects</returns>
		public TransactionList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETTRANSACTIONBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get Transaction Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of Transaction
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTRANSACTIONMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get Transaction Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of Transaction
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _TransactionRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTRANSACTIONROWCOUNT))
			{
				SqlDataReader reader;
				_TransactionRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _TransactionRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills Transaction object
        /// </summary>
        /// <param name="transactionObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(TransactionBase transactionObject, SqlDataReader reader, int start)
		{
			
				transactionObject.Id = reader.GetInt32( start + 0 );			
				transactionObject.CompanyId = reader.GetGuid( start + 1 );			
				transactionObject.CustomerId = reader.GetGuid( start + 2 );			
				transactionObject.Type = reader.GetString( start + 3 );			
				transactionObject.Amount = reader.GetDouble( start + 4 );			
				transactionObject.Status = reader.GetString( start + 5 );			
				transactionObject.TransacationDate = reader.GetDateTime( start + 6 );			
				if(!reader.IsDBNull(7)) transactionObject.CardTransactionId = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) transactionObject.PaymentMethod = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) transactionObject.CheckNo = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) transactionObject.ReferenceNo = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) transactionObject.AddedBy = reader.GetString( start + 11 );			
				if(!reader.IsDBNull(12)) transactionObject.AddedDate = reader.GetDateTime( start + 12 );			
				transactionObject.CreatedBy = reader.GetGuid( start + 13 );			
				if(!reader.IsDBNull(14)) transactionObject.PaymentInfoId = reader.GetInt32( start + 14 );			
				if(!reader.IsDBNull(15)) transactionObject.Note = reader.GetString( start + 15 );			
				if(!reader.IsDBNull(16)) transactionObject.PaymentProfileId = reader.GetInt32( start + 16 );			
				if(!reader.IsDBNull(17)) transactionObject.IsRMR = reader.GetBoolean( start + 17 );			
			FillBaseObject(transactionObject, reader, (start + 18));

			
			transactionObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills Transaction object
        /// </summary>
        /// <param name="transactionObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(TransactionBase transactionObject, SqlDataReader reader)
		{
			FillObject(transactionObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves Transaction object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>Transaction object</returns>
		private Transaction GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					Transaction transactionObject= new Transaction();
					FillObject(transactionObject, reader);
					return transactionObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of Transaction objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of Transaction objects</returns>
		private TransactionList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//Transaction list
			TransactionList list = new TransactionList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					Transaction transactionObject = new Transaction();
					FillObject(transactionObject, reader);

					list.Add(transactionObject);
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
