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
	public partial class PayrollBaseMultipleDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPAYROLLBASEMULTIPLE = "InsertPayrollBaseMultiple";
		private const string UPDATEPAYROLLBASEMULTIPLE = "UpdatePayrollBaseMultiple";
		private const string DELETEPAYROLLBASEMULTIPLE = "DeletePayrollBaseMultiple";
		private const string GETPAYROLLBASEMULTIPLEBYID = "GetPayrollBaseMultipleById";
		private const string GETALLPAYROLLBASEMULTIPLE = "GetAllPayrollBaseMultiple";
		private const string GETPAGEDPAYROLLBASEMULTIPLE = "GetPagedPayrollBaseMultiple";
		private const string GETPAYROLLBASEMULTIPLEMAXIMUMID = "GetPayrollBaseMultipleMaximumId";
		private const string GETPAYROLLBASEMULTIPLEROWCOUNT = "GetPayrollBaseMultipleRowCount";	
		private const string GETPAYROLLBASEMULTIPLEBYQUERY = "GetPayrollBaseMultipleByQuery";
		#endregion
		
		#region Constructors
		public PayrollBaseMultipleDataAccess(ClientContext context) : base(context) { }
		public PayrollBaseMultipleDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="payrollBaseMultipleObject"></param>
		private void AddCommonParams(SqlCommand cmd, PayrollBaseMultipleBase payrollBaseMultipleObject)
		{	
			AddParameter(cmd, pGuid(PayrollBaseMultipleBase.Property_PayrollBaseMultipleId, payrollBaseMultipleObject.PayrollBaseMultipleId));
			AddParameter(cmd, pGuid(PayrollBaseMultipleBase.Property_CompanyId, payrollBaseMultipleObject.CompanyId));
			AddParameter(cmd, pNVarChar(PayrollBaseMultipleBase.Property_BaseMultiple, 50, payrollBaseMultipleObject.BaseMultiple));
			AddParameter(cmd, pDouble(PayrollBaseMultipleBase.Property_Amount, payrollBaseMultipleObject.Amount));
			AddParameter(cmd, pGuid(PayrollBaseMultipleBase.Property_CreatedBy, payrollBaseMultipleObject.CreatedBy));
			AddParameter(cmd, pDateTime(PayrollBaseMultipleBase.Property_CreatedDate, payrollBaseMultipleObject.CreatedDate));
			AddParameter(cmd, pGuid(PayrollBaseMultipleBase.Property_LastUpdateBy, payrollBaseMultipleObject.LastUpdateBy));
			AddParameter(cmd, pDateTime(PayrollBaseMultipleBase.Property_LastUpdateDate, payrollBaseMultipleObject.LastUpdateDate));
			AddParameter(cmd, pGuid(PayrollBaseMultipleBase.Property_TermSheetId, payrollBaseMultipleObject.TermSheetId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PayrollBaseMultiple
        /// </summary>
        /// <param name="payrollBaseMultipleObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PayrollBaseMultipleBase payrollBaseMultipleObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPAYROLLBASEMULTIPLE);
	
				AddParameter(cmd, pInt32Out(PayrollBaseMultipleBase.Property_Id));
				AddCommonParams(cmd, payrollBaseMultipleObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					payrollBaseMultipleObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					payrollBaseMultipleObject.Id = (Int32)GetOutParameter(cmd, PayrollBaseMultipleBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(payrollBaseMultipleObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PayrollBaseMultiple
        /// </summary>
        /// <param name="payrollBaseMultipleObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PayrollBaseMultipleBase payrollBaseMultipleObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPAYROLLBASEMULTIPLE);
				
				AddParameter(cmd, pInt32(PayrollBaseMultipleBase.Property_Id, payrollBaseMultipleObject.Id));
				AddCommonParams(cmd, payrollBaseMultipleObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					payrollBaseMultipleObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(payrollBaseMultipleObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PayrollBaseMultiple
        /// </summary>
        /// <param name="Id">Id of the PayrollBaseMultiple object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPAYROLLBASEMULTIPLE);	
				
				AddParameter(cmd, pInt32(PayrollBaseMultipleBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PayrollBaseMultiple), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PayrollBaseMultiple object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PayrollBaseMultiple object to retrieve</param>
        /// <returns>PayrollBaseMultiple object, null if not found</returns>
		public PayrollBaseMultiple Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLBASEMULTIPLEBYID))
			{
				AddParameter( cmd, pInt32(PayrollBaseMultipleBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PayrollBaseMultiple objects 
        /// </summary>
        /// <returns>A list of PayrollBaseMultiple objects</returns>
		public PayrollBaseMultipleList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPAYROLLBASEMULTIPLE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PayrollBaseMultiple objects by PageRequest
        /// </summary>
        /// <returns>A list of PayrollBaseMultiple objects</returns>
		public PayrollBaseMultipleList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPAYROLLBASEMULTIPLE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PayrollBaseMultipleList _PayrollBaseMultipleList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PayrollBaseMultipleList;
			}
		}
		
		/// <summary>
        /// Retrieves all PayrollBaseMultiple objects by query String
        /// </summary>
        /// <returns>A list of PayrollBaseMultiple objects</returns>
		public PayrollBaseMultipleList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLBASEMULTIPLEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get PayrollBaseMultiple Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of PayrollBaseMultiple
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLBASEMULTIPLEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PayrollBaseMultiple Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PayrollBaseMultiple
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PayrollBaseMultipleRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLBASEMULTIPLEROWCOUNT))
			{
				SqlDataReader reader;
				_PayrollBaseMultipleRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PayrollBaseMultipleRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PayrollBaseMultiple object
        /// </summary>
        /// <param name="payrollBaseMultipleObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PayrollBaseMultipleBase payrollBaseMultipleObject, SqlDataReader reader, int start)
		{
			
				payrollBaseMultipleObject.Id = reader.GetInt32( start + 0 );			
				payrollBaseMultipleObject.PayrollBaseMultipleId = reader.GetGuid( start + 1 );			
				payrollBaseMultipleObject.CompanyId = reader.GetGuid( start + 2 );			
				payrollBaseMultipleObject.BaseMultiple = reader.GetString( start + 3 );			
				payrollBaseMultipleObject.Amount = reader.GetDouble( start + 4 );			
				payrollBaseMultipleObject.CreatedBy = reader.GetGuid( start + 5 );			
				if(!reader.IsDBNull(6)) payrollBaseMultipleObject.CreatedDate = reader.GetDateTime( start + 6 );			
				payrollBaseMultipleObject.LastUpdateBy = reader.GetGuid( start + 7 );			
				if(!reader.IsDBNull(8)) payrollBaseMultipleObject.LastUpdateDate = reader.GetDateTime( start + 8 );			
				payrollBaseMultipleObject.TermSheetId = reader.GetGuid( start + 9 );			
			FillBaseObject(payrollBaseMultipleObject, reader, (start + 10));

			
			payrollBaseMultipleObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PayrollBaseMultiple object
        /// </summary>
        /// <param name="payrollBaseMultipleObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PayrollBaseMultipleBase payrollBaseMultipleObject, SqlDataReader reader)
		{
			FillObject(payrollBaseMultipleObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PayrollBaseMultiple object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PayrollBaseMultiple object</returns>
		private PayrollBaseMultiple GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PayrollBaseMultiple payrollBaseMultipleObject= new PayrollBaseMultiple();
					FillObject(payrollBaseMultipleObject, reader);
					return payrollBaseMultipleObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PayrollBaseMultiple objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PayrollBaseMultiple objects</returns>
		private PayrollBaseMultipleList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PayrollBaseMultiple list
			PayrollBaseMultipleList list = new PayrollBaseMultipleList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PayrollBaseMultiple payrollBaseMultipleObject = new PayrollBaseMultiple();
					FillObject(payrollBaseMultipleObject, reader);

					list.Add(payrollBaseMultipleObject);
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
