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
	public partial class QAFinanceDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTQAFINANCE = "InsertQAFinance";
		private const string UPDATEQAFINANCE = "UpdateQAFinance";
		private const string DELETEQAFINANCE = "DeleteQAFinance";
		private const string GETQAFINANCEBYID = "GetQAFinanceById";
		private const string GETALLQAFINANCE = "GetAllQAFinance";
		private const string GETPAGEDQAFINANCE = "GetPagedQAFinance";
		private const string GETQAFINANCEMAXIMUMID = "GetQAFinanceMaximumId";
		private const string GETQAFINANCEROWCOUNT = "GetQAFinanceRowCount";	
		private const string GETQAFINANCEBYQUERY = "GetQAFinanceByQuery";
		#endregion
		
		#region Constructors
		public QAFinanceDataAccess(ClientContext context) : base(context) { }
		public QAFinanceDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="qAFinanceObject"></param>
		private void AddCommonParams(SqlCommand cmd, QAFinanceBase qAFinanceObject)
		{	
			AddParameter(cmd, pGuid(QAFinanceBase.Property_CustomerId, qAFinanceObject.CustomerId));
			AddParameter(cmd, pGuid(QAFinanceBase.Property_CompanyId, qAFinanceObject.CompanyId));
			AddParameter(cmd, pNVarChar(QAFinanceBase.Property_IsUnderstandTwoPayment, 50, qAFinanceObject.IsUnderstandTwoPayment));
			AddParameter(cmd, pNVarChar(QAFinanceBase.Property_UnderstandPaymentReasonNote, 500, qAFinanceObject.UnderstandPaymentReasonNote));
			AddParameter(cmd, pNVarChar(QAFinanceBase.Property_IsUnderstandSmartHomeInstall, 50, qAFinanceObject.IsUnderstandSmartHomeInstall));
			AddParameter(cmd, pDouble(QAFinanceBase.Property_FinancedAmount, qAFinanceObject.FinancedAmount));
			AddParameter(cmd, pNVarChar(QAFinanceBase.Property_FinancedAmountIsCorrect, 50, qAFinanceObject.FinancedAmountIsCorrect));
			AddParameter(cmd, pNVarChar(QAFinanceBase.Property_FinancedAmountUpdateNote, 500, qAFinanceObject.FinancedAmountUpdateNote));
			AddParameter(cmd, pNVarChar(QAFinanceBase.Property_IsUnderstandServicePayment, 50, qAFinanceObject.IsUnderstandServicePayment));
			AddParameter(cmd, pNVarChar(QAFinanceBase.Property_IsMinimumEquipmentMonthlyPayment, 50, qAFinanceObject.IsMinimumEquipmentMonthlyPayment));
			AddParameter(cmd, pNVarChar(QAFinanceBase.Property_IsUnderstandMinimumMonthlyPayment, 50, qAFinanceObject.IsUnderstandMinimumMonthlyPayment));
			AddParameter(cmd, pNVarChar(QAFinanceBase.Property_IsUnderstandSameAsCash, 50, qAFinanceObject.IsUnderstandSameAsCash));
			AddParameter(cmd, pNVarChar(QAFinanceBase.Property_IsUnderstandInterestAccrues, 50, qAFinanceObject.IsUnderstandInterestAccrues));
			AddParameter(cmd, pNVarChar(QAFinanceBase.Property_IsAnyQuestion, 50, qAFinanceObject.IsAnyQuestion));
			AddParameter(cmd, pNVarChar(QAFinanceBase.Property_ManualNote, qAFinanceObject.ManualNote));
			AddParameter(cmd, pNVarChar(QAFinanceBase.Property_CreatedBy, 50, qAFinanceObject.CreatedBy));
			AddParameter(cmd, pGuid(QAFinanceBase.Property_CreatedByUid, qAFinanceObject.CreatedByUid));
			AddParameter(cmd, pDateTime(QAFinanceBase.Property_CreatedDate, qAFinanceObject.CreatedDate));
			AddParameter(cmd, pGuid(QAFinanceBase.Property_LastUpdatedByUid, qAFinanceObject.LastUpdatedByUid));
			AddParameter(cmd, pDateTime(QAFinanceBase.Property_LastUpdatedDate, qAFinanceObject.LastUpdatedDate));
			AddParameter(cmd, pBool(QAFinanceBase.Property_IsCompleted, qAFinanceObject.IsCompleted));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts QAFinance
        /// </summary>
        /// <param name="qAFinanceObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(QAFinanceBase qAFinanceObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTQAFINANCE);
	
				AddParameter(cmd, pInt32Out(QAFinanceBase.Property_Id));
				AddCommonParams(cmd, qAFinanceObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					qAFinanceObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					qAFinanceObject.Id = (Int32)GetOutParameter(cmd, QAFinanceBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(qAFinanceObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates QAFinance
        /// </summary>
        /// <param name="qAFinanceObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(QAFinanceBase qAFinanceObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEQAFINANCE);
				
				AddParameter(cmd, pInt32(QAFinanceBase.Property_Id, qAFinanceObject.Id));
				AddCommonParams(cmd, qAFinanceObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					qAFinanceObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(qAFinanceObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes QAFinance
        /// </summary>
        /// <param name="Id">Id of the QAFinance object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEQAFINANCE);	
				
				AddParameter(cmd, pInt32(QAFinanceBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(QAFinance), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves QAFinance object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the QAFinance object to retrieve</param>
        /// <returns>QAFinance object, null if not found</returns>
		public QAFinance Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETQAFINANCEBYID))
			{
				AddParameter( cmd, pInt32(QAFinanceBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all QAFinance objects 
        /// </summary>
        /// <returns>A list of QAFinance objects</returns>
		public QAFinanceList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLQAFINANCE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all QAFinance objects by PageRequest
        /// </summary>
        /// <returns>A list of QAFinance objects</returns>
		public QAFinanceList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDQAFINANCE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				QAFinanceList _QAFinanceList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _QAFinanceList;
			}
		}
		
		/// <summary>
        /// Retrieves all QAFinance objects by query String
        /// </summary>
        /// <returns>A list of QAFinance objects</returns>
		public QAFinanceList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETQAFINANCEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get QAFinance Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of QAFinance
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETQAFINANCEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get QAFinance Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of QAFinance
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _QAFinanceRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETQAFINANCEROWCOUNT))
			{
				SqlDataReader reader;
				_QAFinanceRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _QAFinanceRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills QAFinance object
        /// </summary>
        /// <param name="qAFinanceObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(QAFinanceBase qAFinanceObject, SqlDataReader reader, int start)
		{
			
				qAFinanceObject.Id = reader.GetInt32( start + 0 );			
				qAFinanceObject.CustomerId = reader.GetGuid( start + 1 );			
				qAFinanceObject.CompanyId = reader.GetGuid( start + 2 );			
				if(!reader.IsDBNull(3)) qAFinanceObject.IsUnderstandTwoPayment = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) qAFinanceObject.UnderstandPaymentReasonNote = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) qAFinanceObject.IsUnderstandSmartHomeInstall = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) qAFinanceObject.FinancedAmount = reader.GetDouble( start + 6 );			
				if(!reader.IsDBNull(7)) qAFinanceObject.FinancedAmountIsCorrect = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) qAFinanceObject.FinancedAmountUpdateNote = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) qAFinanceObject.IsUnderstandServicePayment = reader.GetString( start + 9 );			
				if(!reader.IsDBNull(10)) qAFinanceObject.IsMinimumEquipmentMonthlyPayment = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) qAFinanceObject.IsUnderstandMinimumMonthlyPayment = reader.GetString( start + 11 );			
				if(!reader.IsDBNull(12)) qAFinanceObject.IsUnderstandSameAsCash = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) qAFinanceObject.IsUnderstandInterestAccrues = reader.GetString( start + 13 );			
				if(!reader.IsDBNull(14)) qAFinanceObject.IsAnyQuestion = reader.GetString( start + 14 );			
				if(!reader.IsDBNull(15)) qAFinanceObject.ManualNote = reader.GetString( start + 15 );			
				if(!reader.IsDBNull(16)) qAFinanceObject.CreatedBy = reader.GetString( start + 16 );			
				qAFinanceObject.CreatedByUid = reader.GetGuid( start + 17 );			
				qAFinanceObject.CreatedDate = reader.GetDateTime( start + 18 );			
				qAFinanceObject.LastUpdatedByUid = reader.GetGuid( start + 19 );			
				qAFinanceObject.LastUpdatedDate = reader.GetDateTime( start + 20 );			
				qAFinanceObject.IsCompleted = reader.GetBoolean( start + 21 );			
			FillBaseObject(qAFinanceObject, reader, (start + 22));

			
			qAFinanceObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills QAFinance object
        /// </summary>
        /// <param name="qAFinanceObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(QAFinanceBase qAFinanceObject, SqlDataReader reader)
		{
			FillObject(qAFinanceObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves QAFinance object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>QAFinance object</returns>
		private QAFinance GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					QAFinance qAFinanceObject= new QAFinance();
					FillObject(qAFinanceObject, reader);
					return qAFinanceObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of QAFinance objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of QAFinance objects</returns>
		private QAFinanceList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//QAFinance list
			QAFinanceList list = new QAFinanceList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					QAFinance qAFinanceObject = new QAFinance();
					FillObject(qAFinanceObject, reader);

					list.Add(qAFinanceObject);
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
