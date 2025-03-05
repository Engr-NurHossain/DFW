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
	public partial class PayrollCustomerTypeDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPAYROLLCUSTOMERTYPE = "InsertPayrollCustomerType";
		private const string UPDATEPAYROLLCUSTOMERTYPE = "UpdatePayrollCustomerType";
		private const string DELETEPAYROLLCUSTOMERTYPE = "DeletePayrollCustomerType";
		private const string GETPAYROLLCUSTOMERTYPEBYID = "GetPayrollCustomerTypeById";
		private const string GETALLPAYROLLCUSTOMERTYPE = "GetAllPayrollCustomerType";
		private const string GETPAGEDPAYROLLCUSTOMERTYPE = "GetPagedPayrollCustomerType";
		private const string GETPAYROLLCUSTOMERTYPEMAXIMUMID = "GetPayrollCustomerTypeMaximumId";
		private const string GETPAYROLLCUSTOMERTYPEROWCOUNT = "GetPayrollCustomerTypeRowCount";	
		private const string GETPAYROLLCUSTOMERTYPEBYQUERY = "GetPayrollCustomerTypeByQuery";
		#endregion
		
		#region Constructors
		public PayrollCustomerTypeDataAccess(ClientContext context) : base(context) { }
		public PayrollCustomerTypeDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="payrollCustomerTypeObject"></param>
		private void AddCommonParams(SqlCommand cmd, PayrollCustomerTypeBase payrollCustomerTypeObject)
		{	
			AddParameter(cmd, pGuid(PayrollCustomerTypeBase.Property_PayrollCustomerTypeId, payrollCustomerTypeObject.PayrollCustomerTypeId));
			AddParameter(cmd, pGuid(PayrollCustomerTypeBase.Property_CompanyId, payrollCustomerTypeObject.CompanyId));
			AddParameter(cmd, pNVarChar(PayrollCustomerTypeBase.Property_CustomerType, 50, payrollCustomerTypeObject.CustomerType));
			AddParameter(cmd, pInt32(PayrollCustomerTypeBase.Property_Point, payrollCustomerTypeObject.Point));
			AddParameter(cmd, pGuid(PayrollCustomerTypeBase.Property_CreatedBy, payrollCustomerTypeObject.CreatedBy));
			AddParameter(cmd, pDateTime(PayrollCustomerTypeBase.Property_CreatedDate, payrollCustomerTypeObject.CreatedDate));
			AddParameter(cmd, pGuid(PayrollCustomerTypeBase.Property_LastUpdateBy, payrollCustomerTypeObject.LastUpdateBy));
			AddParameter(cmd, pDateTime(PayrollCustomerTypeBase.Property_LastUpdateDate, payrollCustomerTypeObject.LastUpdateDate));
			AddParameter(cmd, pGuid(PayrollCustomerTypeBase.Property_TermSheetId, payrollCustomerTypeObject.TermSheetId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PayrollCustomerType
        /// </summary>
        /// <param name="payrollCustomerTypeObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PayrollCustomerTypeBase payrollCustomerTypeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPAYROLLCUSTOMERTYPE);
	
				AddParameter(cmd, pInt32Out(PayrollCustomerTypeBase.Property_Id));
				AddCommonParams(cmd, payrollCustomerTypeObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					payrollCustomerTypeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					payrollCustomerTypeObject.Id = (Int32)GetOutParameter(cmd, PayrollCustomerTypeBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(payrollCustomerTypeObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PayrollCustomerType
        /// </summary>
        /// <param name="payrollCustomerTypeObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PayrollCustomerTypeBase payrollCustomerTypeObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPAYROLLCUSTOMERTYPE);
				
				AddParameter(cmd, pInt32(PayrollCustomerTypeBase.Property_Id, payrollCustomerTypeObject.Id));
				AddCommonParams(cmd, payrollCustomerTypeObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					payrollCustomerTypeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(payrollCustomerTypeObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PayrollCustomerType
        /// </summary>
        /// <param name="Id">Id of the PayrollCustomerType object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPAYROLLCUSTOMERTYPE);	
				
				AddParameter(cmd, pInt32(PayrollCustomerTypeBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PayrollCustomerType), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PayrollCustomerType object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PayrollCustomerType object to retrieve</param>
        /// <returns>PayrollCustomerType object, null if not found</returns>
		public PayrollCustomerType Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLCUSTOMERTYPEBYID))
			{
				AddParameter( cmd, pInt32(PayrollCustomerTypeBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PayrollCustomerType objects 
        /// </summary>
        /// <returns>A list of PayrollCustomerType objects</returns>
		public PayrollCustomerTypeList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPAYROLLCUSTOMERTYPE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PayrollCustomerType objects by PageRequest
        /// </summary>
        /// <returns>A list of PayrollCustomerType objects</returns>
		public PayrollCustomerTypeList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPAYROLLCUSTOMERTYPE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PayrollCustomerTypeList _PayrollCustomerTypeList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PayrollCustomerTypeList;
			}
		}
		
		/// <summary>
        /// Retrieves all PayrollCustomerType objects by query String
        /// </summary>
        /// <returns>A list of PayrollCustomerType objects</returns>
		public PayrollCustomerTypeList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLCUSTOMERTYPEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get PayrollCustomerType Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of PayrollCustomerType
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLCUSTOMERTYPEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PayrollCustomerType Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PayrollCustomerType
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PayrollCustomerTypeRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLCUSTOMERTYPEROWCOUNT))
			{
				SqlDataReader reader;
				_PayrollCustomerTypeRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PayrollCustomerTypeRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PayrollCustomerType object
        /// </summary>
        /// <param name="payrollCustomerTypeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PayrollCustomerTypeBase payrollCustomerTypeObject, SqlDataReader reader, int start)
		{
			
				payrollCustomerTypeObject.Id = reader.GetInt32( start + 0 );			
				payrollCustomerTypeObject.PayrollCustomerTypeId = reader.GetGuid( start + 1 );			
				payrollCustomerTypeObject.CompanyId = reader.GetGuid( start + 2 );			
				payrollCustomerTypeObject.CustomerType = reader.GetString( start + 3 );			
				payrollCustomerTypeObject.Point = reader.GetInt32( start + 4 );			
				payrollCustomerTypeObject.CreatedBy = reader.GetGuid( start + 5 );			
				if(!reader.IsDBNull(6)) payrollCustomerTypeObject.CreatedDate = reader.GetDateTime( start + 6 );			
				payrollCustomerTypeObject.LastUpdateBy = reader.GetGuid( start + 7 );			
				if(!reader.IsDBNull(8)) payrollCustomerTypeObject.LastUpdateDate = reader.GetDateTime( start + 8 );			
				payrollCustomerTypeObject.TermSheetId = reader.GetGuid( start + 9 );			
			FillBaseObject(payrollCustomerTypeObject, reader, (start + 10));

			
			payrollCustomerTypeObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PayrollCustomerType object
        /// </summary>
        /// <param name="payrollCustomerTypeObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PayrollCustomerTypeBase payrollCustomerTypeObject, SqlDataReader reader)
		{
			FillObject(payrollCustomerTypeObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PayrollCustomerType object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PayrollCustomerType object</returns>
		private PayrollCustomerType GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PayrollCustomerType payrollCustomerTypeObject= new PayrollCustomerType();
					FillObject(payrollCustomerTypeObject, reader);
					return payrollCustomerTypeObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PayrollCustomerType objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PayrollCustomerType objects</returns>
		private PayrollCustomerTypeList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PayrollCustomerType list
			PayrollCustomerTypeList list = new PayrollCustomerTypeList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PayrollCustomerType payrollCustomerTypeObject = new PayrollCustomerType();
					FillObject(payrollCustomerTypeObject, reader);

					list.Add(payrollCustomerTypeObject);
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
