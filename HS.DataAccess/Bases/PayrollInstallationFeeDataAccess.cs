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
	public partial class PayrollInstallationFeeDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPAYROLLINSTALLATIONFEE = "InsertPayrollInstallationFee";
		private const string UPDATEPAYROLLINSTALLATIONFEE = "UpdatePayrollInstallationFee";
		private const string DELETEPAYROLLINSTALLATIONFEE = "DeletePayrollInstallationFee";
		private const string GETPAYROLLINSTALLATIONFEEBYID = "GetPayrollInstallationFeeById";
		private const string GETALLPAYROLLINSTALLATIONFEE = "GetAllPayrollInstallationFee";
		private const string GETPAGEDPAYROLLINSTALLATIONFEE = "GetPagedPayrollInstallationFee";
		private const string GETPAYROLLINSTALLATIONFEEMAXIMUMID = "GetPayrollInstallationFeeMaximumId";
		private const string GETPAYROLLINSTALLATIONFEEROWCOUNT = "GetPayrollInstallationFeeRowCount";	
		private const string GETPAYROLLINSTALLATIONFEEBYQUERY = "GetPayrollInstallationFeeByQuery";
		#endregion
		
		#region Constructors
		public PayrollInstallationFeeDataAccess(ClientContext context) : base(context) { }
		public PayrollInstallationFeeDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="payrollInstallationFeeObject"></param>
		private void AddCommonParams(SqlCommand cmd, PayrollInstallationFeeBase payrollInstallationFeeObject)
		{	
			AddParameter(cmd, pGuid(PayrollInstallationFeeBase.Property_PayrollInstallationFeeId, payrollInstallationFeeObject.PayrollInstallationFeeId));
			AddParameter(cmd, pGuid(PayrollInstallationFeeBase.Property_CompanyId, payrollInstallationFeeObject.CompanyId));
			AddParameter(cmd, pDouble(PayrollInstallationFeeBase.Property_InstallationFeeMin, payrollInstallationFeeObject.InstallationFeeMin));
			AddParameter(cmd, pDouble(PayrollInstallationFeeBase.Property_InstallationFeeMax, payrollInstallationFeeObject.InstallationFeeMax));
			AddParameter(cmd, pDouble(PayrollInstallationFeeBase.Property_Amount, payrollInstallationFeeObject.Amount));
			AddParameter(cmd, pGuid(PayrollInstallationFeeBase.Property_CreatedBy, payrollInstallationFeeObject.CreatedBy));
			AddParameter(cmd, pDateTime(PayrollInstallationFeeBase.Property_CreatedDate, payrollInstallationFeeObject.CreatedDate));
			AddParameter(cmd, pGuid(PayrollInstallationFeeBase.Property_LastUpdateBy, payrollInstallationFeeObject.LastUpdateBy));
			AddParameter(cmd, pDateTime(PayrollInstallationFeeBase.Property_LastUpdateDate, payrollInstallationFeeObject.LastUpdateDate));
			AddParameter(cmd, pGuid(PayrollInstallationFeeBase.Property_TermSheetId, payrollInstallationFeeObject.TermSheetId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PayrollInstallationFee
        /// </summary>
        /// <param name="payrollInstallationFeeObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PayrollInstallationFeeBase payrollInstallationFeeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPAYROLLINSTALLATIONFEE);
	
				AddParameter(cmd, pInt32Out(PayrollInstallationFeeBase.Property_Id));
				AddCommonParams(cmd, payrollInstallationFeeObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					payrollInstallationFeeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					payrollInstallationFeeObject.Id = (Int32)GetOutParameter(cmd, PayrollInstallationFeeBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(payrollInstallationFeeObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PayrollInstallationFee
        /// </summary>
        /// <param name="payrollInstallationFeeObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PayrollInstallationFeeBase payrollInstallationFeeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPAYROLLINSTALLATIONFEE);
				
				AddParameter(cmd, pInt32(PayrollInstallationFeeBase.Property_Id, payrollInstallationFeeObject.Id));
				AddCommonParams(cmd, payrollInstallationFeeObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					payrollInstallationFeeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(payrollInstallationFeeObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PayrollInstallationFee
        /// </summary>
        /// <param name="Id">Id of the PayrollInstallationFee object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPAYROLLINSTALLATIONFEE);	
				
				AddParameter(cmd, pInt32(PayrollInstallationFeeBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PayrollInstallationFee), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PayrollInstallationFee object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PayrollInstallationFee object to retrieve</param>
        /// <returns>PayrollInstallationFee object, null if not found</returns>
		public PayrollInstallationFee Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLINSTALLATIONFEEBYID))
			{
				AddParameter( cmd, pInt32(PayrollInstallationFeeBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PayrollInstallationFee objects 
        /// </summary>
        /// <returns>A list of PayrollInstallationFee objects</returns>
		public PayrollInstallationFeeList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPAYROLLINSTALLATIONFEE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PayrollInstallationFee objects by PageRequest
        /// </summary>
        /// <returns>A list of PayrollInstallationFee objects</returns>
		public PayrollInstallationFeeList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPAYROLLINSTALLATIONFEE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PayrollInstallationFeeList _PayrollInstallationFeeList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PayrollInstallationFeeList;
			}
		}
		
		/// <summary>
        /// Retrieves all PayrollInstallationFee objects by query String
        /// </summary>
        /// <returns>A list of PayrollInstallationFee objects</returns>
		public PayrollInstallationFeeList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLINSTALLATIONFEEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get PayrollInstallationFee Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of PayrollInstallationFee
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLINSTALLATIONFEEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PayrollInstallationFee Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PayrollInstallationFee
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PayrollInstallationFeeRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLINSTALLATIONFEEROWCOUNT))
			{
				SqlDataReader reader;
				_PayrollInstallationFeeRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PayrollInstallationFeeRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PayrollInstallationFee object
        /// </summary>
        /// <param name="payrollInstallationFeeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PayrollInstallationFeeBase payrollInstallationFeeObject, SqlDataReader reader, int start)
		{
			
				payrollInstallationFeeObject.Id = reader.GetInt32( start + 0 );			
				payrollInstallationFeeObject.PayrollInstallationFeeId = reader.GetGuid( start + 1 );			
				payrollInstallationFeeObject.CompanyId = reader.GetGuid( start + 2 );			
				payrollInstallationFeeObject.InstallationFeeMin = reader.GetDouble( start + 3 );			
				payrollInstallationFeeObject.InstallationFeeMax = reader.GetDouble( start + 4 );			
				payrollInstallationFeeObject.Amount = reader.GetDouble( start + 5 );			
				payrollInstallationFeeObject.CreatedBy = reader.GetGuid( start + 6 );			
				if(!reader.IsDBNull(7)) payrollInstallationFeeObject.CreatedDate = reader.GetDateTime( start + 7 );			
				payrollInstallationFeeObject.LastUpdateBy = reader.GetGuid( start + 8 );			
				if(!reader.IsDBNull(9)) payrollInstallationFeeObject.LastUpdateDate = reader.GetDateTime( start + 9 );			
				payrollInstallationFeeObject.TermSheetId = reader.GetGuid( start + 10 );			
			FillBaseObject(payrollInstallationFeeObject, reader, (start + 11));

			
			payrollInstallationFeeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PayrollInstallationFee object
        /// </summary>
        /// <param name="payrollInstallationFeeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PayrollInstallationFeeBase payrollInstallationFeeObject, SqlDataReader reader)
		{
			FillObject(payrollInstallationFeeObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PayrollInstallationFee object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PayrollInstallationFee object</returns>
		private PayrollInstallationFee GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PayrollInstallationFee payrollInstallationFeeObject= new PayrollInstallationFee();
					FillObject(payrollInstallationFeeObject, reader);
					return payrollInstallationFeeObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PayrollInstallationFee objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PayrollInstallationFee objects</returns>
		private PayrollInstallationFeeList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PayrollInstallationFee list
			PayrollInstallationFeeList list = new PayrollInstallationFeeList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PayrollInstallationFee payrollInstallationFeeObject = new PayrollInstallationFee();
					FillObject(payrollInstallationFeeObject, reader);

					list.Add(payrollInstallationFeeObject);
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
