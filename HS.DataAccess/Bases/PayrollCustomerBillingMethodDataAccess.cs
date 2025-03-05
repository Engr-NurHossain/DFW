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
	public partial class PayrollCustomerBillingMethodDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPAYROLLCUSTOMERBILLINGMETHOD = "InsertPayrollCustomerBillingMethod";
		private const string UPDATEPAYROLLCUSTOMERBILLINGMETHOD = "UpdatePayrollCustomerBillingMethod";
		private const string DELETEPAYROLLCUSTOMERBILLINGMETHOD = "DeletePayrollCustomerBillingMethod";
		private const string GETPAYROLLCUSTOMERBILLINGMETHODBYID = "GetPayrollCustomerBillingMethodById";
		private const string GETALLPAYROLLCUSTOMERBILLINGMETHOD = "GetAllPayrollCustomerBillingMethod";
		private const string GETPAGEDPAYROLLCUSTOMERBILLINGMETHOD = "GetPagedPayrollCustomerBillingMethod";
		private const string GETPAYROLLCUSTOMERBILLINGMETHODMAXIMUMID = "GetPayrollCustomerBillingMethodMaximumId";
		private const string GETPAYROLLCUSTOMERBILLINGMETHODROWCOUNT = "GetPayrollCustomerBillingMethodRowCount";	
		private const string GETPAYROLLCUSTOMERBILLINGMETHODBYQUERY = "GetPayrollCustomerBillingMethodByQuery";
		#endregion
		
		#region Constructors
		public PayrollCustomerBillingMethodDataAccess(ClientContext context) : base(context) { }
		public PayrollCustomerBillingMethodDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="payrollCustomerBillingMethodObject"></param>
		private void AddCommonParams(SqlCommand cmd, PayrollCustomerBillingMethodBase payrollCustomerBillingMethodObject)
		{	
			AddParameter(cmd, pGuid(PayrollCustomerBillingMethodBase.Property_PayrollCustomerBillingMethodId, payrollCustomerBillingMethodObject.PayrollCustomerBillingMethodId));
			AddParameter(cmd, pGuid(PayrollCustomerBillingMethodBase.Property_CompanyId, payrollCustomerBillingMethodObject.CompanyId));
			AddParameter(cmd, pNVarChar(PayrollCustomerBillingMethodBase.Property_BillingMethod, 50, payrollCustomerBillingMethodObject.BillingMethod));
			AddParameter(cmd, pInt32(PayrollCustomerBillingMethodBase.Property_Point, payrollCustomerBillingMethodObject.Point));
			AddParameter(cmd, pGuid(PayrollCustomerBillingMethodBase.Property_CreatedBy, payrollCustomerBillingMethodObject.CreatedBy));
			AddParameter(cmd, pDateTime(PayrollCustomerBillingMethodBase.Property_CreatedDate, payrollCustomerBillingMethodObject.CreatedDate));
			AddParameter(cmd, pGuid(PayrollCustomerBillingMethodBase.Property_LastUpdateBy, payrollCustomerBillingMethodObject.LastUpdateBy));
			AddParameter(cmd, pDateTime(PayrollCustomerBillingMethodBase.Property_LastUpdateDate, payrollCustomerBillingMethodObject.LastUpdateDate));
			AddParameter(cmd, pGuid(PayrollCustomerBillingMethodBase.Property_TermSheetId, payrollCustomerBillingMethodObject.TermSheetId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PayrollCustomerBillingMethod
        /// </summary>
        /// <param name="payrollCustomerBillingMethodObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PayrollCustomerBillingMethodBase payrollCustomerBillingMethodObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPAYROLLCUSTOMERBILLINGMETHOD);
	
				AddParameter(cmd, pInt32Out(PayrollCustomerBillingMethodBase.Property_Id));
				AddCommonParams(cmd, payrollCustomerBillingMethodObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					payrollCustomerBillingMethodObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					payrollCustomerBillingMethodObject.Id = (Int32)GetOutParameter(cmd, PayrollCustomerBillingMethodBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(payrollCustomerBillingMethodObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PayrollCustomerBillingMethod
        /// </summary>
        /// <param name="payrollCustomerBillingMethodObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PayrollCustomerBillingMethodBase payrollCustomerBillingMethodObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPAYROLLCUSTOMERBILLINGMETHOD);
				
				AddParameter(cmd, pInt32(PayrollCustomerBillingMethodBase.Property_Id, payrollCustomerBillingMethodObject.Id));
				AddCommonParams(cmd, payrollCustomerBillingMethodObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					payrollCustomerBillingMethodObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(payrollCustomerBillingMethodObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PayrollCustomerBillingMethod
        /// </summary>
        /// <param name="Id">Id of the PayrollCustomerBillingMethod object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPAYROLLCUSTOMERBILLINGMETHOD);	
				
				AddParameter(cmd, pInt32(PayrollCustomerBillingMethodBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PayrollCustomerBillingMethod), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PayrollCustomerBillingMethod object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PayrollCustomerBillingMethod object to retrieve</param>
        /// <returns>PayrollCustomerBillingMethod object, null if not found</returns>
		public PayrollCustomerBillingMethod Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLCUSTOMERBILLINGMETHODBYID))
			{
				AddParameter( cmd, pInt32(PayrollCustomerBillingMethodBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PayrollCustomerBillingMethod objects 
        /// </summary>
        /// <returns>A list of PayrollCustomerBillingMethod objects</returns>
		public PayrollCustomerBillingMethodList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPAYROLLCUSTOMERBILLINGMETHOD))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PayrollCustomerBillingMethod objects by PageRequest
        /// </summary>
        /// <returns>A list of PayrollCustomerBillingMethod objects</returns>
		public PayrollCustomerBillingMethodList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPAYROLLCUSTOMERBILLINGMETHOD))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PayrollCustomerBillingMethodList _PayrollCustomerBillingMethodList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PayrollCustomerBillingMethodList;
			}
		}
		
		/// <summary>
        /// Retrieves all PayrollCustomerBillingMethod objects by query String
        /// </summary>
        /// <returns>A list of PayrollCustomerBillingMethod objects</returns>
		public PayrollCustomerBillingMethodList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLCUSTOMERBILLINGMETHODBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get PayrollCustomerBillingMethod Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of PayrollCustomerBillingMethod
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLCUSTOMERBILLINGMETHODMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PayrollCustomerBillingMethod Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PayrollCustomerBillingMethod
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PayrollCustomerBillingMethodRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLCUSTOMERBILLINGMETHODROWCOUNT))
			{
				SqlDataReader reader;
				_PayrollCustomerBillingMethodRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PayrollCustomerBillingMethodRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PayrollCustomerBillingMethod object
        /// </summary>
        /// <param name="payrollCustomerBillingMethodObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PayrollCustomerBillingMethodBase payrollCustomerBillingMethodObject, SqlDataReader reader, int start)
		{
			
				payrollCustomerBillingMethodObject.Id = reader.GetInt32( start + 0 );			
				payrollCustomerBillingMethodObject.PayrollCustomerBillingMethodId = reader.GetGuid( start + 1 );			
				payrollCustomerBillingMethodObject.CompanyId = reader.GetGuid( start + 2 );			
				payrollCustomerBillingMethodObject.BillingMethod = reader.GetString( start + 3 );			
				payrollCustomerBillingMethodObject.Point = reader.GetInt32( start + 4 );			
				payrollCustomerBillingMethodObject.CreatedBy = reader.GetGuid( start + 5 );			
				if(!reader.IsDBNull(6)) payrollCustomerBillingMethodObject.CreatedDate = reader.GetDateTime( start + 6 );			
				payrollCustomerBillingMethodObject.LastUpdateBy = reader.GetGuid( start + 7 );			
				if(!reader.IsDBNull(8)) payrollCustomerBillingMethodObject.LastUpdateDate = reader.GetDateTime( start + 8 );			
				payrollCustomerBillingMethodObject.TermSheetId = reader.GetGuid( start + 9 );			
			FillBaseObject(payrollCustomerBillingMethodObject, reader, (start + 10));

			
			payrollCustomerBillingMethodObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PayrollCustomerBillingMethod object
        /// </summary>
        /// <param name="payrollCustomerBillingMethodObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PayrollCustomerBillingMethodBase payrollCustomerBillingMethodObject, SqlDataReader reader)
		{
			FillObject(payrollCustomerBillingMethodObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PayrollCustomerBillingMethod object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PayrollCustomerBillingMethod object</returns>
		private PayrollCustomerBillingMethod GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PayrollCustomerBillingMethod payrollCustomerBillingMethodObject= new PayrollCustomerBillingMethod();
					FillObject(payrollCustomerBillingMethodObject, reader);
					return payrollCustomerBillingMethodObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PayrollCustomerBillingMethod objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PayrollCustomerBillingMethod objects</returns>
		private PayrollCustomerBillingMethodList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PayrollCustomerBillingMethod list
			PayrollCustomerBillingMethodList list = new PayrollCustomerBillingMethodList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PayrollCustomerBillingMethod payrollCustomerBillingMethodObject = new PayrollCustomerBillingMethod();
					FillObject(payrollCustomerBillingMethodObject, reader);

					list.Add(payrollCustomerBillingMethodObject);
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
