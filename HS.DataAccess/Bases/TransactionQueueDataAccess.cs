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
	public partial class TransactionQueueDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTTRANSACTIONQUEUE = "InsertTransactionQueue";
		private const string UPDATETRANSACTIONQUEUE = "UpdateTransactionQueue";
		private const string DELETETRANSACTIONQUEUE = "DeleteTransactionQueue";
		private const string GETTRANSACTIONQUEUEBYID = "GetTransactionQueueById";
		private const string GETALLTRANSACTIONQUEUE = "GetAllTransactionQueue";
		private const string GETPAGEDTRANSACTIONQUEUE = "GetPagedTransactionQueue";
		private const string GETTRANSACTIONQUEUEMAXIMUMID = "GetTransactionQueueMaximumId";
		private const string GETTRANSACTIONQUEUEROWCOUNT = "GetTransactionQueueRowCount";	
		private const string GETTRANSACTIONQUEUEBYQUERY = "GetTransactionQueueByQuery";
		#endregion
		
		#region Constructors
		public TransactionQueueDataAccess(ClientContext context) : base(context) { }
		public TransactionQueueDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="transactionQueueObject"></param>
		private void AddCommonParams(SqlCommand cmd, TransactionQueueBase transactionQueueObject)
		{	
			AddParameter(cmd, pGuid(TransactionQueueBase.Property_CustomerId, transactionQueueObject.CustomerId));
			AddParameter(cmd, pNVarChar(TransactionQueueBase.Property_InvoiceId, 100, transactionQueueObject.InvoiceId));
			AddParameter(cmd, pDouble(TransactionQueueBase.Property_Amount, transactionQueueObject.Amount));
			AddParameter(cmd, pDateTime(TransactionQueueBase.Property_CreatedDate, transactionQueueObject.CreatedDate));
			AddParameter(cmd, pGuid(TransactionQueueBase.Property_CreatedBy, transactionQueueObject.CreatedBy));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts TransactionQueue
        /// </summary>
        /// <param name="transactionQueueObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(TransactionQueueBase transactionQueueObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTTRANSACTIONQUEUE);
	
				AddParameter(cmd, pInt32Out(TransactionQueueBase.Property_Id));
				AddCommonParams(cmd, transactionQueueObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					transactionQueueObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					transactionQueueObject.Id = (Int32)GetOutParameter(cmd, TransactionQueueBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(transactionQueueObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates TransactionQueue
        /// </summary>
        /// <param name="transactionQueueObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(TransactionQueueBase transactionQueueObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATETRANSACTIONQUEUE);
				
				AddParameter(cmd, pInt32(TransactionQueueBase.Property_Id, transactionQueueObject.Id));
				AddCommonParams(cmd, transactionQueueObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					transactionQueueObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(transactionQueueObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes TransactionQueue
        /// </summary>
        /// <param name="Id">Id of the TransactionQueue object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETETRANSACTIONQUEUE);	
				
				AddParameter(cmd, pInt32(TransactionQueueBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(TransactionQueue), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves TransactionQueue object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the TransactionQueue object to retrieve</param>
        /// <returns>TransactionQueue object, null if not found</returns>
		public TransactionQueue Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETTRANSACTIONQUEUEBYID))
			{
				AddParameter( cmd, pInt32(TransactionQueueBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all TransactionQueue objects 
        /// </summary>
        /// <returns>A list of TransactionQueue objects</returns>
		public TransactionQueueList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLTRANSACTIONQUEUE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all TransactionQueue objects by PageRequest
        /// </summary>
        /// <returns>A list of TransactionQueue objects</returns>
		public TransactionQueueList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDTRANSACTIONQUEUE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				TransactionQueueList _TransactionQueueList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _TransactionQueueList;
			}
		}
		
		/// <summary>
        /// Retrieves all TransactionQueue objects by query String
        /// </summary>
        /// <returns>A list of TransactionQueue objects</returns>
		public TransactionQueueList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETTRANSACTIONQUEUEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get TransactionQueue Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of TransactionQueue
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTRANSACTIONQUEUEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get TransactionQueue Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of TransactionQueue
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _TransactionQueueRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETTRANSACTIONQUEUEROWCOUNT))
			{
				SqlDataReader reader;
				_TransactionQueueRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _TransactionQueueRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills TransactionQueue object
        /// </summary>
        /// <param name="transactionQueueObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(TransactionQueueBase transactionQueueObject, SqlDataReader reader, int start)
		{
			
				transactionQueueObject.Id = reader.GetInt32( start + 0 );			
				transactionQueueObject.CustomerId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) transactionQueueObject.InvoiceId = reader.GetString( start + 2 );			
				transactionQueueObject.Amount = reader.GetDouble( start + 3 );			
				transactionQueueObject.CreatedDate = reader.GetDateTime( start + 4 );			
				transactionQueueObject.CreatedBy = reader.GetGuid( start + 5 );			
			FillBaseObject(transactionQueueObject, reader, (start + 6));

			
			transactionQueueObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills TransactionQueue object
        /// </summary>
        /// <param name="transactionQueueObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(TransactionQueueBase transactionQueueObject, SqlDataReader reader)
		{
			FillObject(transactionQueueObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves TransactionQueue object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>TransactionQueue object</returns>
		private TransactionQueue GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					TransactionQueue transactionQueueObject= new TransactionQueue();
					FillObject(transactionQueueObject, reader);
					return transactionQueueObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of TransactionQueue objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of TransactionQueue objects</returns>
		private TransactionQueueList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//TransactionQueue list
			TransactionQueueList list = new TransactionQueueList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					TransactionQueue transactionQueueObject = new TransactionQueue();
					FillObject(transactionQueueObject, reader);

					list.Add(transactionQueueObject);
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
