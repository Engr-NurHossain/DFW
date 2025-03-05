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
	public partial class TransactionExpenseDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTTRANSACTIONEXPENSE = "InsertTransactionExpense";
		private const string UPDATETRANSACTIONEXPENSE = "UpdateTransactionExpense";
		private const string DELETETRANSACTIONEXPENSE = "DeleteTransactionExpense";
		private const string GETTRANSACTIONEXPENSEBYID = "GetTransactionExpenseById";
		private const string GETALLTRANSACTIONEXPENSE = "GetAllTransactionExpense";
		private const string GETPAGEDTRANSACTIONEXPENSE = "GetPagedTransactionExpense";
		private const string GETTRANSACTIONEXPENSEMAXIMUMID = "GetTransactionExpenseMaximumId";
		private const string GETTRANSACTIONEXPENSEROWCOUNT = "GetTransactionExpenseRowCount";	
		private const string GETTRANSACTIONEXPENSEBYQUERY = "GetTransactionExpenseByQuery";
		#endregion
		
		#region Constructors
		public TransactionExpenseDataAccess(ClientContext context) : base(context) { }
		public TransactionExpenseDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="transactionExpenseObject"></param>
		private void AddCommonParams(SqlCommand cmd, TransactionExpenseBase transactionExpenseObject)
		{	
			AddParameter(cmd, pGuid(TransactionExpenseBase.Property_CompanyId, transactionExpenseObject.CompanyId));
			AddParameter(cmd, pGuid(TransactionExpenseBase.Property_CustomerId, transactionExpenseObject.CustomerId));
			AddParameter(cmd, pNVarChar(TransactionExpenseBase.Property_Type, 50, transactionExpenseObject.Type));
			AddParameter(cmd, pDouble(TransactionExpenseBase.Property_Amount, transactionExpenseObject.Amount));
			AddParameter(cmd, pNVarChar(TransactionExpenseBase.Property_Status, 50, transactionExpenseObject.Status));
			AddParameter(cmd, pDateTime(TransactionExpenseBase.Property_ExpenseDate, transactionExpenseObject.ExpenseDate));
			AddParameter(cmd, pNVarChar(TransactionExpenseBase.Property_CardTransactionId, 50, transactionExpenseObject.CardTransactionId));
			AddParameter(cmd, pNVarChar(TransactionExpenseBase.Property_PaymentMethod, 50, transactionExpenseObject.PaymentMethod));
			AddParameter(cmd, pNVarChar(TransactionExpenseBase.Property_CheckNo, 150, transactionExpenseObject.CheckNo));
			AddParameter(cmd, pNVarChar(TransactionExpenseBase.Property_ReferenceNo, 150, transactionExpenseObject.ReferenceNo));
			AddParameter(cmd, pNVarChar(TransactionExpenseBase.Property_Description, transactionExpenseObject.Description));
			AddParameter(cmd, pDateTime(TransactionExpenseBase.Property_CreatedDate, transactionExpenseObject.CreatedDate));
			AddParameter(cmd, pGuid(TransactionExpenseBase.Property_CreatedBy, transactionExpenseObject.CreatedBy));
			AddParameter(cmd, pNVarChar(TransactionExpenseBase.Property_RefType, 50, transactionExpenseObject.RefType));
			AddParameter(cmd, pGuid(TransactionExpenseBase.Property_UserId, transactionExpenseObject.UserId));
			AddParameter(cmd, pNVarChar(TransactionExpenseBase.Property_ExpenseType, 50, transactionExpenseObject.ExpenseType));
			AddParameter(cmd, pNVarChar(TransactionExpenseBase.Property_FilePath, 500, transactionExpenseObject.FilePath));
			AddParameter(cmd, pInt32(TransactionExpenseBase.Property_TicketNo, transactionExpenseObject.TicketNo));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts TransactionExpense
        /// </summary>
        /// <param name="transactionExpenseObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(TransactionExpenseBase transactionExpenseObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTTRANSACTIONEXPENSE);
	
				AddParameter(cmd, pInt32Out(TransactionExpenseBase.Property_Id));
				AddCommonParams(cmd, transactionExpenseObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					transactionExpenseObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					transactionExpenseObject.Id = (Int32)GetOutParameter(cmd, TransactionExpenseBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(transactionExpenseObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates TransactionExpense
        /// </summary>
        /// <param name="transactionExpenseObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(TransactionExpenseBase transactionExpenseObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATETRANSACTIONEXPENSE);
				
				AddParameter(cmd, pInt32(TransactionExpenseBase.Property_Id, transactionExpenseObject.Id));
				AddCommonParams(cmd, transactionExpenseObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					transactionExpenseObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(transactionExpenseObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes TransactionExpense
        /// </summary>
        /// <param name="Id">Id of the TransactionExpense object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETETRANSACTIONEXPENSE);	
				
				AddParameter(cmd, pInt32(TransactionExpenseBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(TransactionExpense), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves TransactionExpense object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the TransactionExpense object to retrieve</param>
        /// <returns>TransactionExpense object, null if not found</returns>
		public TransactionExpense Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETTRANSACTIONEXPENSEBYID))
			{
				AddParameter( cmd, pInt32(TransactionExpenseBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all TransactionExpense objects 
        /// </summary>
        /// <returns>A list of TransactionExpense objects</returns>
		public TransactionExpenseList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLTRANSACTIONEXPENSE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all TransactionExpense objects by PageRequest
        /// </summary>
        /// <returns>A list of TransactionExpense objects</returns>
		public TransactionExpenseList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDTRANSACTIONEXPENSE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				TransactionExpenseList _TransactionExpenseList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _TransactionExpenseList;
			}
		}
		
		/// <summary>
        /// Retrieves all TransactionExpense objects by query String
        /// </summary>
        /// <returns>A list of TransactionExpense objects</returns>
		public TransactionExpenseList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETTRANSACTIONEXPENSEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get TransactionExpense Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of TransactionExpense
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTRANSACTIONEXPENSEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get TransactionExpense Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of TransactionExpense
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _TransactionExpenseRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTRANSACTIONEXPENSEROWCOUNT))
			{
				SqlDataReader reader;
				_TransactionExpenseRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _TransactionExpenseRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills TransactionExpense object
        /// </summary>
        /// <param name="transactionExpenseObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(TransactionExpenseBase transactionExpenseObject, SqlDataReader reader, int start)
		{
			
				transactionExpenseObject.Id = reader.GetInt32( start + 0 );			
				transactionExpenseObject.CompanyId = reader.GetGuid( start + 1 );			
				transactionExpenseObject.CustomerId = reader.GetGuid( start + 2 );			
				transactionExpenseObject.Type = reader.GetString( start + 3 );			
				transactionExpenseObject.Amount = reader.GetDouble( start + 4 );			
				transactionExpenseObject.Status = reader.GetString( start + 5 );			
				transactionExpenseObject.ExpenseDate = reader.GetDateTime( start + 6 );			
				if(!reader.IsDBNull(7)) transactionExpenseObject.CardTransactionId = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) transactionExpenseObject.PaymentMethod = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) transactionExpenseObject.CheckNo = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) transactionExpenseObject.ReferenceNo = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) transactionExpenseObject.Description = reader.GetString( start + 11 );			
				transactionExpenseObject.CreatedDate = reader.GetDateTime( start + 12 );			
				transactionExpenseObject.CreatedBy = reader.GetGuid( start + 13 );			
				if(!reader.IsDBNull(14)) transactionExpenseObject.RefType = reader.GetString( start + 14 );			
				transactionExpenseObject.UserId = reader.GetGuid( start + 15 );			
				if(!reader.IsDBNull(16)) transactionExpenseObject.ExpenseType = reader.GetString( start + 16 );			
				if(!reader.IsDBNull(17)) transactionExpenseObject.FilePath = reader.GetString( start + 17 );			
				if(!reader.IsDBNull(18)) transactionExpenseObject.TicketNo = reader.GetInt32( start + 18 );			
			FillBaseObject(transactionExpenseObject, reader, (start + 19));

			
			transactionExpenseObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills TransactionExpense object
        /// </summary>
        /// <param name="transactionExpenseObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(TransactionExpenseBase transactionExpenseObject, SqlDataReader reader)
		{
			FillObject(transactionExpenseObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves TransactionExpense object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>TransactionExpense object</returns>
		private TransactionExpense GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					TransactionExpense transactionExpenseObject= new TransactionExpense();
					FillObject(transactionExpenseObject, reader);
					return transactionExpenseObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of TransactionExpense objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of TransactionExpense objects</returns>
		private TransactionExpenseList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//TransactionExpense list
			TransactionExpenseList list = new TransactionExpenseList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					TransactionExpense transactionExpenseObject = new TransactionExpense();
					FillObject(transactionExpenseObject, reader);

					list.Add(transactionExpenseObject);
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
