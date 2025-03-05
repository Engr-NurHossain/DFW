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
	public partial class DeclinedTransactionsDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTDECLINEDTRANSACTIONS = "InsertDeclinedTransactions";
		private const string UPDATEDECLINEDTRANSACTIONS = "UpdateDeclinedTransactions";
		private const string DELETEDECLINEDTRANSACTIONS = "DeleteDeclinedTransactions";
		private const string GETDECLINEDTRANSACTIONSBYID = "GetDeclinedTransactionsById";
		private const string GETALLDECLINEDTRANSACTIONS = "GetAllDeclinedTransactions";
		private const string GETPAGEDDECLINEDTRANSACTIONS = "GetPagedDeclinedTransactions";
		private const string GETDECLINEDTRANSACTIONSMAXIMUMID = "GetDeclinedTransactionsMaximumId";
		private const string GETDECLINEDTRANSACTIONSROWCOUNT = "GetDeclinedTransactionsRowCount";	
		private const string GETDECLINEDTRANSACTIONSBYQUERY = "GetDeclinedTransactionsByQuery";
		#endregion
		
		#region Constructors
		public DeclinedTransactionsDataAccess(ClientContext context) : base(context) { }
		public DeclinedTransactionsDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="declinedTransactionsObject"></param>
		private void AddCommonParams(SqlCommand cmd, DeclinedTransactionsBase declinedTransactionsObject)
		{	
			AddParameter(cmd, pGuid(DeclinedTransactionsBase.Property_CompanyId, declinedTransactionsObject.CompanyId));
			AddParameter(cmd, pGuid(DeclinedTransactionsBase.Property_CustomerId, declinedTransactionsObject.CustomerId));
			AddParameter(cmd, pNVarChar(DeclinedTransactionsBase.Property_TransactionId, 50, declinedTransactionsObject.TransactionId));
			AddParameter(cmd, pNVarChar(DeclinedTransactionsBase.Property_InvoiceId, 50, declinedTransactionsObject.InvoiceId));
			AddParameter(cmd, pNVarChar(DeclinedTransactionsBase.Property_Reason, 200, declinedTransactionsObject.Reason));
			AddParameter(cmd, pDateTime(DeclinedTransactionsBase.Property_ReturnedDate, declinedTransactionsObject.ReturnedDate));
			AddParameter(cmd, pDouble(DeclinedTransactionsBase.Property_ReturnAmount, declinedTransactionsObject.ReturnAmount));
			AddParameter(cmd, pDouble(DeclinedTransactionsBase.Property_SubmitAmount, declinedTransactionsObject.SubmitAmount));
			AddParameter(cmd, pDateTime(DeclinedTransactionsBase.Property_SettlementBatch, declinedTransactionsObject.SettlementBatch));
			AddParameter(cmd, pNVarChar(DeclinedTransactionsBase.Property_Comment, declinedTransactionsObject.Comment));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts DeclinedTransactions
        /// </summary>
        /// <param name="declinedTransactionsObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(DeclinedTransactionsBase declinedTransactionsObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTDECLINEDTRANSACTIONS);
	
				AddParameter(cmd, pInt32Out(DeclinedTransactionsBase.Property_Id));
				AddCommonParams(cmd, declinedTransactionsObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					declinedTransactionsObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					declinedTransactionsObject.Id = (Int32)GetOutParameter(cmd, DeclinedTransactionsBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(declinedTransactionsObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates DeclinedTransactions
        /// </summary>
        /// <param name="declinedTransactionsObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(DeclinedTransactionsBase declinedTransactionsObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEDECLINEDTRANSACTIONS);
				
				AddParameter(cmd, pInt32(DeclinedTransactionsBase.Property_Id, declinedTransactionsObject.Id));
				AddCommonParams(cmd, declinedTransactionsObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					declinedTransactionsObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(declinedTransactionsObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes DeclinedTransactions
        /// </summary>
        /// <param name="Id">Id of the DeclinedTransactions object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEDECLINEDTRANSACTIONS);	
				
				AddParameter(cmd, pInt32(DeclinedTransactionsBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(DeclinedTransactions), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves DeclinedTransactions object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the DeclinedTransactions object to retrieve</param>
        /// <returns>DeclinedTransactions object, null if not found</returns>
		public DeclinedTransactions Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETDECLINEDTRANSACTIONSBYID))
			{
				AddParameter( cmd, pInt32(DeclinedTransactionsBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all DeclinedTransactions objects 
        /// </summary>
        /// <returns>A list of DeclinedTransactions objects</returns>
		public DeclinedTransactionsList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLDECLINEDTRANSACTIONS))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all DeclinedTransactions objects by PageRequest
        /// </summary>
        /// <returns>A list of DeclinedTransactions objects</returns>
		public DeclinedTransactionsList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDDECLINEDTRANSACTIONS))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				DeclinedTransactionsList _DeclinedTransactionsList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _DeclinedTransactionsList;
			}
		}
		
		/// <summary>
        /// Retrieves all DeclinedTransactions objects by query String
        /// </summary>
        /// <returns>A list of DeclinedTransactions objects</returns>
		public DeclinedTransactionsList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETDECLINEDTRANSACTIONSBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get DeclinedTransactions Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of DeclinedTransactions
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETDECLINEDTRANSACTIONSMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get DeclinedTransactions Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of DeclinedTransactions
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _DeclinedTransactionsRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETDECLINEDTRANSACTIONSROWCOUNT))
			{
				SqlDataReader reader;
				_DeclinedTransactionsRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _DeclinedTransactionsRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills DeclinedTransactions object
        /// </summary>
        /// <param name="declinedTransactionsObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(DeclinedTransactionsBase declinedTransactionsObject, SqlDataReader reader, int start)
		{
			
				declinedTransactionsObject.Id = reader.GetInt32( start + 0 );			
				declinedTransactionsObject.CompanyId = reader.GetGuid( start + 1 );			
				declinedTransactionsObject.CustomerId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) declinedTransactionsObject.TransactionId = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) declinedTransactionsObject.InvoiceId = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) declinedTransactionsObject.Reason = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) declinedTransactionsObject.ReturnedDate = reader.GetDateTime( start + 6 );			
				if(!reader.IsDBNull(7)) declinedTransactionsObject.ReturnAmount = reader.GetDouble( start + 7 );			
				if(!reader.IsDBNull(8)) declinedTransactionsObject.SubmitAmount = reader.GetDouble( start + 8 );			
				if(!reader.IsDBNull(9)) declinedTransactionsObject.SettlementBatch = reader.GetDateTime( start + 9 );			
				if(!reader.IsDBNull(10)) declinedTransactionsObject.Comment = reader.GetString( start + 10 );			
			FillBaseObject(declinedTransactionsObject, reader, (start + 11));

			
			declinedTransactionsObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills DeclinedTransactions object
        /// </summary>
        /// <param name="declinedTransactionsObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(DeclinedTransactionsBase declinedTransactionsObject, SqlDataReader reader)
		{
			FillObject(declinedTransactionsObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves DeclinedTransactions object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>DeclinedTransactions object</returns>
		private DeclinedTransactions GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					DeclinedTransactions declinedTransactionsObject= new DeclinedTransactions();
					FillObject(declinedTransactionsObject, reader);
					return declinedTransactionsObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of DeclinedTransactions objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of DeclinedTransactions objects</returns>
		private DeclinedTransactionsList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//DeclinedTransactions list
			DeclinedTransactionsList list = new DeclinedTransactionsList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					DeclinedTransactions declinedTransactionsObject = new DeclinedTransactions();
					FillObject(declinedTransactionsObject, reader);

					list.Add(declinedTransactionsObject);
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
