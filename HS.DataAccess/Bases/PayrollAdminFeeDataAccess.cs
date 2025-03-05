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
	public partial class PayrollAdminFeeDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPAYROLLADMINFEE = "InsertPayrollAdminFee";
		private const string UPDATEPAYROLLADMINFEE = "UpdatePayrollAdminFee";
		private const string DELETEPAYROLLADMINFEE = "DeletePayrollAdminFee";
		private const string GETPAYROLLADMINFEEBYID = "GetPayrollAdminFeeById";
		private const string GETALLPAYROLLADMINFEE = "GetAllPayrollAdminFee";
		private const string GETPAGEDPAYROLLADMINFEE = "GetPagedPayrollAdminFee";
		private const string GETPAYROLLADMINFEEMAXIMUMID = "GetPayrollAdminFeeMaximumId";
		private const string GETPAYROLLADMINFEEROWCOUNT = "GetPayrollAdminFeeRowCount";	
		private const string GETPAYROLLADMINFEEBYQUERY = "GetPayrollAdminFeeByQuery";
		#endregion
		
		#region Constructors
		public PayrollAdminFeeDataAccess(ClientContext context) : base(context) { }
		public PayrollAdminFeeDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="payrollAdminFeeObject"></param>
		private void AddCommonParams(SqlCommand cmd, PayrollAdminFeeBase payrollAdminFeeObject)
		{	
			AddParameter(cmd, pGuid(PayrollAdminFeeBase.Property_PayrollAdminFeeId, payrollAdminFeeObject.PayrollAdminFeeId));
			AddParameter(cmd, pGuid(PayrollAdminFeeBase.Property_CompanyId, payrollAdminFeeObject.CompanyId));
			AddParameter(cmd, pNVarChar(PayrollAdminFeeBase.Property_AdminFee, 100, payrollAdminFeeObject.AdminFee));
			AddParameter(cmd, pDouble(PayrollAdminFeeBase.Property_Amount, payrollAdminFeeObject.Amount));
			AddParameter(cmd, pGuid(PayrollAdminFeeBase.Property_CreatedBy, payrollAdminFeeObject.CreatedBy));
			AddParameter(cmd, pDateTime(PayrollAdminFeeBase.Property_CreatedDate, payrollAdminFeeObject.CreatedDate));
			AddParameter(cmd, pGuid(PayrollAdminFeeBase.Property_LastUpdateBy, payrollAdminFeeObject.LastUpdateBy));
			AddParameter(cmd, pDateTime(PayrollAdminFeeBase.Property_LastUpdateDate, payrollAdminFeeObject.LastUpdateDate));
			AddParameter(cmd, pGuid(PayrollAdminFeeBase.Property_TermSheetId, payrollAdminFeeObject.TermSheetId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PayrollAdminFee
        /// </summary>
        /// <param name="payrollAdminFeeObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PayrollAdminFeeBase payrollAdminFeeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPAYROLLADMINFEE);
	
				AddParameter(cmd, pInt32Out(PayrollAdminFeeBase.Property_Id));
				AddCommonParams(cmd, payrollAdminFeeObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					payrollAdminFeeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					payrollAdminFeeObject.Id = (Int32)GetOutParameter(cmd, PayrollAdminFeeBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(payrollAdminFeeObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PayrollAdminFee
        /// </summary>
        /// <param name="payrollAdminFeeObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PayrollAdminFeeBase payrollAdminFeeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPAYROLLADMINFEE);
				
				AddParameter(cmd, pInt32(PayrollAdminFeeBase.Property_Id, payrollAdminFeeObject.Id));
				AddCommonParams(cmd, payrollAdminFeeObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					payrollAdminFeeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(payrollAdminFeeObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PayrollAdminFee
        /// </summary>
        /// <param name="Id">Id of the PayrollAdminFee object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPAYROLLADMINFEE);	
				
				AddParameter(cmd, pInt32(PayrollAdminFeeBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PayrollAdminFee), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PayrollAdminFee object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PayrollAdminFee object to retrieve</param>
        /// <returns>PayrollAdminFee object, null if not found</returns>
		public PayrollAdminFee Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLADMINFEEBYID))
			{
				AddParameter( cmd, pInt32(PayrollAdminFeeBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PayrollAdminFee objects 
        /// </summary>
        /// <returns>A list of PayrollAdminFee objects</returns>
		public PayrollAdminFeeList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPAYROLLADMINFEE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PayrollAdminFee objects by PageRequest
        /// </summary>
        /// <returns>A list of PayrollAdminFee objects</returns>
		public PayrollAdminFeeList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPAYROLLADMINFEE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PayrollAdminFeeList _PayrollAdminFeeList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PayrollAdminFeeList;
			}
		}
		
		/// <summary>
        /// Retrieves all PayrollAdminFee objects by query String
        /// </summary>
        /// <returns>A list of PayrollAdminFee objects</returns>
		public PayrollAdminFeeList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLADMINFEEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get PayrollAdminFee Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of PayrollAdminFee
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLADMINFEEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PayrollAdminFee Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PayrollAdminFee
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PayrollAdminFeeRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLADMINFEEROWCOUNT))
			{
				SqlDataReader reader;
				_PayrollAdminFeeRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PayrollAdminFeeRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PayrollAdminFee object
        /// </summary>
        /// <param name="payrollAdminFeeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PayrollAdminFeeBase payrollAdminFeeObject, SqlDataReader reader, int start)
		{
			
				payrollAdminFeeObject.Id = reader.GetInt32( start + 0 );			
				payrollAdminFeeObject.PayrollAdminFeeId = reader.GetGuid( start + 1 );			
				payrollAdminFeeObject.CompanyId = reader.GetGuid( start + 2 );			
				payrollAdminFeeObject.AdminFee = reader.GetString( start + 3 );			
				payrollAdminFeeObject.Amount = reader.GetDouble( start + 4 );			
				payrollAdminFeeObject.CreatedBy = reader.GetGuid( start + 5 );			
				if(!reader.IsDBNull(6)) payrollAdminFeeObject.CreatedDate = reader.GetDateTime( start + 6 );			
				payrollAdminFeeObject.LastUpdateBy = reader.GetGuid( start + 7 );			
				if(!reader.IsDBNull(8)) payrollAdminFeeObject.LastUpdateDate = reader.GetDateTime( start + 8 );			
				payrollAdminFeeObject.TermSheetId = reader.GetGuid( start + 9 );			
			FillBaseObject(payrollAdminFeeObject, reader, (start + 10));

			
			payrollAdminFeeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PayrollAdminFee object
        /// </summary>
        /// <param name="payrollAdminFeeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PayrollAdminFeeBase payrollAdminFeeObject, SqlDataReader reader)
		{
			FillObject(payrollAdminFeeObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PayrollAdminFee object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PayrollAdminFee object</returns>
		private PayrollAdminFee GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PayrollAdminFee payrollAdminFeeObject= new PayrollAdminFee();
					FillObject(payrollAdminFeeObject, reader);
					return payrollAdminFeeObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PayrollAdminFee objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PayrollAdminFee objects</returns>
		private PayrollAdminFeeList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PayrollAdminFee list
			PayrollAdminFeeList list = new PayrollAdminFeeList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PayrollAdminFee payrollAdminFeeObject = new PayrollAdminFee();
					FillObject(payrollAdminFeeObject, reader);

					list.Add(payrollAdminFeeObject);
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
