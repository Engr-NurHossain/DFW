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
	public partial class PayrollTermSheetManagerDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPAYROLLTERMSHEETMANAGER = "InsertPayrollTermSheetManager";
		private const string UPDATEPAYROLLTERMSHEETMANAGER = "UpdatePayrollTermSheetManager";
		private const string DELETEPAYROLLTERMSHEETMANAGER = "DeletePayrollTermSheetManager";
		private const string GETPAYROLLTERMSHEETMANAGERBYID = "GetPayrollTermSheetManagerById";
		private const string GETALLPAYROLLTERMSHEETMANAGER = "GetAllPayrollTermSheetManager";
		private const string GETPAGEDPAYROLLTERMSHEETMANAGER = "GetPagedPayrollTermSheetManager";
		private const string GETPAYROLLTERMSHEETMANAGERMAXIMUMID = "GetPayrollTermSheetManagerMaximumId";
		private const string GETPAYROLLTERMSHEETMANAGERROWCOUNT = "GetPayrollTermSheetManagerRowCount";	
		private const string GETPAYROLLTERMSHEETMANAGERBYQUERY = "GetPayrollTermSheetManagerByQuery";
		#endregion
		
		#region Constructors
		public PayrollTermSheetManagerDataAccess(ClientContext context) : base(context) { }
		public PayrollTermSheetManagerDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="payrollTermSheetManagerObject"></param>
		private void AddCommonParams(SqlCommand cmd, PayrollTermSheetManagerBase payrollTermSheetManagerObject)
		{	
			AddParameter(cmd, pGuid(PayrollTermSheetManagerBase.Property_PayrollTermSheetManagerId, payrollTermSheetManagerObject.PayrollTermSheetManagerId));
			AddParameter(cmd, pGuid(PayrollTermSheetManagerBase.Property_CompanyId, payrollTermSheetManagerObject.CompanyId));
			AddParameter(cmd, pGuid(PayrollTermSheetManagerBase.Property_TermSheetId, payrollTermSheetManagerObject.TermSheetId));
			AddParameter(cmd, pGuid(PayrollTermSheetManagerBase.Property_ManagerId, payrollTermSheetManagerObject.ManagerId));
			AddParameter(cmd, pNVarChar(PayrollTermSheetManagerBase.Property_Type, 100, payrollTermSheetManagerObject.Type));
			AddParameter(cmd, pDouble(PayrollTermSheetManagerBase.Property_Value, payrollTermSheetManagerObject.Value));
			AddParameter(cmd, pGuid(PayrollTermSheetManagerBase.Property_CreatedBy, payrollTermSheetManagerObject.CreatedBy));
			AddParameter(cmd, pDateTime(PayrollTermSheetManagerBase.Property_CreatedDate, payrollTermSheetManagerObject.CreatedDate));
			AddParameter(cmd, pGuid(PayrollTermSheetManagerBase.Property_LastUpdateBy, payrollTermSheetManagerObject.LastUpdateBy));
			AddParameter(cmd, pDateTime(PayrollTermSheetManagerBase.Property_LastUpdateDate, payrollTermSheetManagerObject.LastUpdateDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PayrollTermSheetManager
        /// </summary>
        /// <param name="payrollTermSheetManagerObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PayrollTermSheetManagerBase payrollTermSheetManagerObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPAYROLLTERMSHEETMANAGER);
	
				AddParameter(cmd, pInt32Out(PayrollTermSheetManagerBase.Property_Id));
				AddCommonParams(cmd, payrollTermSheetManagerObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					payrollTermSheetManagerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					payrollTermSheetManagerObject.Id = (Int32)GetOutParameter(cmd, PayrollTermSheetManagerBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(payrollTermSheetManagerObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PayrollTermSheetManager
        /// </summary>
        /// <param name="payrollTermSheetManagerObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PayrollTermSheetManagerBase payrollTermSheetManagerObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPAYROLLTERMSHEETMANAGER);
				
				AddParameter(cmd, pInt32(PayrollTermSheetManagerBase.Property_Id, payrollTermSheetManagerObject.Id));
				AddCommonParams(cmd, payrollTermSheetManagerObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					payrollTermSheetManagerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(payrollTermSheetManagerObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PayrollTermSheetManager
        /// </summary>
        /// <param name="Id">Id of the PayrollTermSheetManager object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPAYROLLTERMSHEETMANAGER);	
				
				AddParameter(cmd, pInt32(PayrollTermSheetManagerBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PayrollTermSheetManager), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PayrollTermSheetManager object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PayrollTermSheetManager object to retrieve</param>
        /// <returns>PayrollTermSheetManager object, null if not found</returns>
		public PayrollTermSheetManager Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLTERMSHEETMANAGERBYID))
			{
				AddParameter( cmd, pInt32(PayrollTermSheetManagerBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PayrollTermSheetManager objects 
        /// </summary>
        /// <returns>A list of PayrollTermSheetManager objects</returns>
		public PayrollTermSheetManagerList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPAYROLLTERMSHEETMANAGER))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PayrollTermSheetManager objects by PageRequest
        /// </summary>
        /// <returns>A list of PayrollTermSheetManager objects</returns>
		public PayrollTermSheetManagerList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPAYROLLTERMSHEETMANAGER))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PayrollTermSheetManagerList _PayrollTermSheetManagerList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PayrollTermSheetManagerList;
			}
		}
		
		/// <summary>
        /// Retrieves all PayrollTermSheetManager objects by query String
        /// </summary>
        /// <returns>A list of PayrollTermSheetManager objects</returns>
		public PayrollTermSheetManagerList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLTERMSHEETMANAGERBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get PayrollTermSheetManager Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of PayrollTermSheetManager
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLTERMSHEETMANAGERMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PayrollTermSheetManager Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PayrollTermSheetManager
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PayrollTermSheetManagerRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLTERMSHEETMANAGERROWCOUNT))
			{
				SqlDataReader reader;
				_PayrollTermSheetManagerRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PayrollTermSheetManagerRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PayrollTermSheetManager object
        /// </summary>
        /// <param name="payrollTermSheetManagerObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PayrollTermSheetManagerBase payrollTermSheetManagerObject, SqlDataReader reader, int start)
		{
			
				payrollTermSheetManagerObject.Id = reader.GetInt32( start + 0 );			
				payrollTermSheetManagerObject.PayrollTermSheetManagerId = reader.GetGuid( start + 1 );			
				payrollTermSheetManagerObject.CompanyId = reader.GetGuid( start + 2 );			
				payrollTermSheetManagerObject.TermSheetId = reader.GetGuid( start + 3 );			
				payrollTermSheetManagerObject.ManagerId = reader.GetGuid( start + 4 );			
				payrollTermSheetManagerObject.Type = reader.GetString( start + 5 );			
				payrollTermSheetManagerObject.Value = reader.GetDouble( start + 6 );			
				payrollTermSheetManagerObject.CreatedBy = reader.GetGuid( start + 7 );			
				if(!reader.IsDBNull(8)) payrollTermSheetManagerObject.CreatedDate = reader.GetDateTime( start + 8 );			
				payrollTermSheetManagerObject.LastUpdateBy = reader.GetGuid( start + 9 );			
				if(!reader.IsDBNull(10)) payrollTermSheetManagerObject.LastUpdateDate = reader.GetDateTime( start + 10 );			
			FillBaseObject(payrollTermSheetManagerObject, reader, (start + 11));

			
			payrollTermSheetManagerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PayrollTermSheetManager object
        /// </summary>
        /// <param name="payrollTermSheetManagerObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PayrollTermSheetManagerBase payrollTermSheetManagerObject, SqlDataReader reader)
		{
			FillObject(payrollTermSheetManagerObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PayrollTermSheetManager object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PayrollTermSheetManager object</returns>
		private PayrollTermSheetManager GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PayrollTermSheetManager payrollTermSheetManagerObject= new PayrollTermSheetManager();
					FillObject(payrollTermSheetManagerObject, reader);
					return payrollTermSheetManagerObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PayrollTermSheetManager objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PayrollTermSheetManager objects</returns>
		private PayrollTermSheetManagerList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PayrollTermSheetManager list
			PayrollTermSheetManagerList list = new PayrollTermSheetManagerList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PayrollTermSheetManager payrollTermSheetManagerObject = new PayrollTermSheetManager();
					FillObject(payrollTermSheetManagerObject, reader);

					list.Add(payrollTermSheetManagerObject);
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
