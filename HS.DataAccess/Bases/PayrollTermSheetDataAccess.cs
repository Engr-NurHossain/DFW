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
	public partial class PayrollTermSheetDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPAYROLLTERMSHEET = "InsertPayrollTermSheet";
		private const string UPDATEPAYROLLTERMSHEET = "UpdatePayrollTermSheet";
		private const string DELETEPAYROLLTERMSHEET = "DeletePayrollTermSheet";
		private const string GETPAYROLLTERMSHEETBYID = "GetPayrollTermSheetById";
		private const string GETALLPAYROLLTERMSHEET = "GetAllPayrollTermSheet";
		private const string GETPAGEDPAYROLLTERMSHEET = "GetPagedPayrollTermSheet";
		private const string GETPAYROLLTERMSHEETMAXIMUMID = "GetPayrollTermSheetMaximumId";
		private const string GETPAYROLLTERMSHEETROWCOUNT = "GetPayrollTermSheetRowCount";	
		private const string GETPAYROLLTERMSHEETBYQUERY = "GetPayrollTermSheetByQuery";
		#endregion
		
		#region Constructors
		public PayrollTermSheetDataAccess(ClientContext context) : base(context) { }
		public PayrollTermSheetDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="payrollTermSheetObject"></param>
		private void AddCommonParams(SqlCommand cmd, PayrollTermSheetBase payrollTermSheetObject)
		{	
			AddParameter(cmd, pGuid(PayrollTermSheetBase.Property_TermSheetId, payrollTermSheetObject.TermSheetId));
			AddParameter(cmd, pGuid(PayrollTermSheetBase.Property_CompanyId, payrollTermSheetObject.CompanyId));
			AddParameter(cmd, pNVarChar(PayrollTermSheetBase.Property_Name, 100, payrollTermSheetObject.Name));
			AddParameter(cmd, pBool(PayrollTermSheetBase.Property_IsBase, payrollTermSheetObject.IsBase));
			AddParameter(cmd, pBool(PayrollTermSheetBase.Property_IsActive, payrollTermSheetObject.IsActive));
			AddParameter(cmd, pGuid(PayrollTermSheetBase.Property_CreatedBy, payrollTermSheetObject.CreatedBy));
			AddParameter(cmd, pDateTime(PayrollTermSheetBase.Property_CreatedDate, payrollTermSheetObject.CreatedDate));
			AddParameter(cmd, pGuid(PayrollTermSheetBase.Property_LastUpdateBy, payrollTermSheetObject.LastUpdateBy));
			AddParameter(cmd, pDateTime(PayrollTermSheetBase.Property_LastUpdateDate, payrollTermSheetObject.LastUpdateDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PayrollTermSheet
        /// </summary>
        /// <param name="payrollTermSheetObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PayrollTermSheetBase payrollTermSheetObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPAYROLLTERMSHEET);
	
				AddParameter(cmd, pInt32Out(PayrollTermSheetBase.Property_Id));
				AddCommonParams(cmd, payrollTermSheetObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					payrollTermSheetObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					payrollTermSheetObject.Id = (Int32)GetOutParameter(cmd, PayrollTermSheetBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(payrollTermSheetObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PayrollTermSheet
        /// </summary>
        /// <param name="payrollTermSheetObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PayrollTermSheetBase payrollTermSheetObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPAYROLLTERMSHEET);
				
				AddParameter(cmd, pInt32(PayrollTermSheetBase.Property_Id, payrollTermSheetObject.Id));
				AddCommonParams(cmd, payrollTermSheetObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					payrollTermSheetObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(payrollTermSheetObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PayrollTermSheet
        /// </summary>
        /// <param name="Id">Id of the PayrollTermSheet object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPAYROLLTERMSHEET);	
				
				AddParameter(cmd, pInt32(PayrollTermSheetBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PayrollTermSheet), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PayrollTermSheet object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PayrollTermSheet object to retrieve</param>
        /// <returns>PayrollTermSheet object, null if not found</returns>
		public PayrollTermSheet Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLTERMSHEETBYID))
			{
				AddParameter( cmd, pInt32(PayrollTermSheetBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PayrollTermSheet objects 
        /// </summary>
        /// <returns>A list of PayrollTermSheet objects</returns>
		public PayrollTermSheetList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPAYROLLTERMSHEET))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PayrollTermSheet objects by PageRequest
        /// </summary>
        /// <returns>A list of PayrollTermSheet objects</returns>
		public PayrollTermSheetList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPAYROLLTERMSHEET))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PayrollTermSheetList _PayrollTermSheetList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PayrollTermSheetList;
			}
		}
		
		/// <summary>
        /// Retrieves all PayrollTermSheet objects by query String
        /// </summary>
        /// <returns>A list of PayrollTermSheet objects</returns>
		public PayrollTermSheetList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLTERMSHEETBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get PayrollTermSheet Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of PayrollTermSheet
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLTERMSHEETMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PayrollTermSheet Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PayrollTermSheet
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PayrollTermSheetRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLTERMSHEETROWCOUNT))
			{
				SqlDataReader reader;
				_PayrollTermSheetRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PayrollTermSheetRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PayrollTermSheet object
        /// </summary>
        /// <param name="payrollTermSheetObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PayrollTermSheetBase payrollTermSheetObject, SqlDataReader reader, int start)
		{
			
				payrollTermSheetObject.Id = reader.GetInt32( start + 0 );			
				payrollTermSheetObject.TermSheetId = reader.GetGuid( start + 1 );			
				payrollTermSheetObject.CompanyId = reader.GetGuid( start + 2 );			
				payrollTermSheetObject.Name = reader.GetString( start + 3 );			
				payrollTermSheetObject.IsBase = reader.GetBoolean( start + 4 );			
				payrollTermSheetObject.IsActive = reader.GetBoolean( start + 5 );			
				payrollTermSheetObject.CreatedBy = reader.GetGuid( start + 6 );			
				if(!reader.IsDBNull(7)) payrollTermSheetObject.CreatedDate = reader.GetDateTime( start + 7 );			
				payrollTermSheetObject.LastUpdateBy = reader.GetGuid( start + 8 );			
				if(!reader.IsDBNull(9)) payrollTermSheetObject.LastUpdateDate = reader.GetDateTime( start + 9 );			
			FillBaseObject(payrollTermSheetObject, reader, (start + 10));

			
			payrollTermSheetObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PayrollTermSheet object
        /// </summary>
        /// <param name="payrollTermSheetObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PayrollTermSheetBase payrollTermSheetObject, SqlDataReader reader)
		{
			FillObject(payrollTermSheetObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PayrollTermSheet object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PayrollTermSheet object</returns>
		private PayrollTermSheet GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PayrollTermSheet payrollTermSheetObject= new PayrollTermSheet();
					FillObject(payrollTermSheetObject, reader);
					return payrollTermSheetObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PayrollTermSheet objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PayrollTermSheet objects</returns>
		private PayrollTermSheetList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PayrollTermSheet list
			PayrollTermSheetList list = new PayrollTermSheetList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PayrollTermSheet payrollTermSheetObject = new PayrollTermSheet();
					FillObject(payrollTermSheetObject, reader);

					list.Add(payrollTermSheetObject);
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
