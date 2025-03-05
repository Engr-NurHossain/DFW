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
	public partial class TransactionHistoryDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTTRANSACTIONHISTORY = "InsertTransactionHistory";
		private const string UPDATETRANSACTIONHISTORY = "UpdateTransactionHistory";
		private const string DELETETRANSACTIONHISTORY = "DeleteTransactionHistory";
		private const string GETTRANSACTIONHISTORYBYID = "GetTransactionHistoryById";
		private const string GETALLTRANSACTIONHISTORY = "GetAllTransactionHistory";
		private const string GETPAGEDTRANSACTIONHISTORY = "GetPagedTransactionHistory";
		private const string GETTRANSACTIONHISTORYMAXIMUMID = "GetTransactionHistoryMaximumId";
		private const string GETTRANSACTIONHISTORYROWCOUNT = "GetTransactionHistoryRowCount";	
		private const string GETTRANSACTIONHISTORYBYQUERY = "GetTransactionHistoryByQuery";
		#endregion
		
		#region Constructors
		public TransactionHistoryDataAccess(ClientContext context) : base(context) { }
		public TransactionHistoryDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="transactionHistoryObject"></param>
		private void AddCommonParams(SqlCommand cmd, TransactionHistoryBase transactionHistoryObject)
		{	
			AddParameter(cmd, pInt32(TransactionHistoryBase.Property_TransactionId, transactionHistoryObject.TransactionId));
			AddParameter(cmd, pInt32(TransactionHistoryBase.Property_InvoiceId, transactionHistoryObject.InvoiceId));
			AddParameter(cmd, pDouble(TransactionHistoryBase.Property_Amout, transactionHistoryObject.Amout));
			AddParameter(cmd, pDouble(TransactionHistoryBase.Property_Balance, transactionHistoryObject.Balance));
			AddParameter(cmd, pGuid(TransactionHistoryBase.Property_ReceivedBy, transactionHistoryObject.ReceivedBy));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts TransactionHistory
        /// </summary>
        /// <param name="transactionHistoryObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(TransactionHistoryBase transactionHistoryObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTTRANSACTIONHISTORY);
	
				AddParameter(cmd, pInt32Out(TransactionHistoryBase.Property_Id));
				AddCommonParams(cmd, transactionHistoryObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					transactionHistoryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					transactionHistoryObject.Id = (Int32)GetOutParameter(cmd, TransactionHistoryBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(transactionHistoryObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates TransactionHistory
        /// </summary>
        /// <param name="transactionHistoryObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(TransactionHistoryBase transactionHistoryObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATETRANSACTIONHISTORY);
				
				AddParameter(cmd, pInt32(TransactionHistoryBase.Property_Id, transactionHistoryObject.Id));
				AddCommonParams(cmd, transactionHistoryObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					transactionHistoryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(transactionHistoryObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes TransactionHistory
        /// </summary>
        /// <param name="Id">Id of the TransactionHistory object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETETRANSACTIONHISTORY);	
				
				AddParameter(cmd, pInt32(TransactionHistoryBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(TransactionHistory), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves TransactionHistory object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the TransactionHistory object to retrieve</param>
        /// <returns>TransactionHistory object, null if not found</returns>
		public TransactionHistory Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETTRANSACTIONHISTORYBYID))
			{
				AddParameter( cmd, pInt32(TransactionHistoryBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all TransactionHistory objects 
        /// </summary>
        /// <returns>A list of TransactionHistory objects</returns>
		public TransactionHistoryList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLTRANSACTIONHISTORY))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all TransactionHistory objects by PageRequest
        /// </summary>
        /// <returns>A list of TransactionHistory objects</returns>
		public TransactionHistoryList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDTRANSACTIONHISTORY))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				TransactionHistoryList _TransactionHistoryList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _TransactionHistoryList;
			}
		}
		
		/// <summary>
        /// Retrieves all TransactionHistory objects by query String
        /// </summary>
        /// <returns>A list of TransactionHistory objects</returns>
		public TransactionHistoryList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETTRANSACTIONHISTORYBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get TransactionHistory Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of TransactionHistory
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTRANSACTIONHISTORYMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get TransactionHistory Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of TransactionHistory
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _TransactionHistoryRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTRANSACTIONHISTORYROWCOUNT))
			{
				SqlDataReader reader;
				_TransactionHistoryRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _TransactionHistoryRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills TransactionHistory object
        /// </summary>
        /// <param name="transactionHistoryObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(TransactionHistoryBase transactionHistoryObject, SqlDataReader reader, int start)
		{
			
				transactionHistoryObject.Id = reader.GetInt32( start + 0 );			
				transactionHistoryObject.TransactionId = reader.GetInt32( start + 1 );			
				transactionHistoryObject.InvoiceId = reader.GetInt32( start + 2 );			
				transactionHistoryObject.Amout = reader.GetDouble( start + 3 );			
				transactionHistoryObject.Balance = reader.GetDouble( start + 4 );			
				transactionHistoryObject.ReceivedBy = reader.GetGuid( start + 5 );			
			FillBaseObject(transactionHistoryObject, reader, (start + 6));

			
			transactionHistoryObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills TransactionHistory object
        /// </summary>
        /// <param name="transactionHistoryObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(TransactionHistoryBase transactionHistoryObject, SqlDataReader reader)
		{
			FillObject(transactionHistoryObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves TransactionHistory object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>TransactionHistory object</returns>
		private TransactionHistory GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					TransactionHistory transactionHistoryObject= new TransactionHistory();
					FillObject(transactionHistoryObject, reader);
					return transactionHistoryObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of TransactionHistory objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of TransactionHistory objects</returns>
		private TransactionHistoryList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//TransactionHistory list
			TransactionHistoryList list = new TransactionHistoryList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					TransactionHistory transactionHistoryObject = new TransactionHistory();
					FillObject(transactionHistoryObject, reader);

					list.Add(transactionHistoryObject);
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
