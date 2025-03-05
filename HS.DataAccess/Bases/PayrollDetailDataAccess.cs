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
	public partial class PayrollDetailDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPAYROLLDETAIL = "InsertPayrollDetail";
		private const string UPDATEPAYROLLDETAIL = "UpdatePayrollDetail";
		private const string DELETEPAYROLLDETAIL = "DeletePayrollDetail";
		private const string GETPAYROLLDETAILBYID = "GetPayrollDetailById";
		private const string GETALLPAYROLLDETAIL = "GetAllPayrollDetail";
		private const string GETPAGEDPAYROLLDETAIL = "GetPagedPayrollDetail";
		private const string GETPAYROLLDETAILMAXIMUMID = "GetPayrollDetailMaximumId";
		private const string GETPAYROLLDETAILROWCOUNT = "GetPayrollDetailRowCount";	
		private const string GETPAYROLLDETAILBYQUERY = "GetPayrollDetailByQuery";
		#endregion
		
		#region Constructors
		public PayrollDetailDataAccess(ClientContext context) : base(context) { }
		public PayrollDetailDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="payrollDetailObject"></param>
		private void AddCommonParams(SqlCommand cmd, PayrollDetailBase payrollDetailObject)
		{	
			AddParameter(cmd, pGuid(PayrollDetailBase.Property_CompanyId, payrollDetailObject.CompanyId));
			AddParameter(cmd, pNVarChar(PayrollDetailBase.Property_RepName, 50, payrollDetailObject.RepName));
			AddParameter(cmd, pDouble(PayrollDetailBase.Property_RepCommission, payrollDetailObject.RepCommission));
			AddParameter(cmd, pDouble(PayrollDetailBase.Property_RepHoldback, payrollDetailObject.RepHoldback));
			AddParameter(cmd, pNVarChar(PayrollDetailBase.Property_OverrideRep1, 100, payrollDetailObject.OverrideRep1));
			AddParameter(cmd, pNVarChar(PayrollDetailBase.Property_Override1, 100, payrollDetailObject.Override1));
			AddParameter(cmd, pNVarChar(PayrollDetailBase.Property_OverrideRep2, 100, payrollDetailObject.OverrideRep2));
			AddParameter(cmd, pNVarChar(PayrollDetailBase.Property_Override2, 100, payrollDetailObject.Override2));
			AddParameter(cmd, pDateTime(PayrollDetailBase.Property_RepPaidDate, payrollDetailObject.RepPaidDate));
			AddParameter(cmd, pNVarChar(PayrollDetailBase.Property_TechName, 50, payrollDetailObject.TechName));
			AddParameter(cmd, pDouble(PayrollDetailBase.Property_TechPay, payrollDetailObject.TechPay));
			AddParameter(cmd, pDouble(PayrollDetailBase.Property_TechHoldback, payrollDetailObject.TechHoldback));
			AddParameter(cmd, pDateTime(PayrollDetailBase.Property_TechPaidDate, payrollDetailObject.TechPaidDate));
			AddParameter(cmd, pDouble(PayrollDetailBase.Property_OpenerCommission, payrollDetailObject.OpenerCommission));
			AddParameter(cmd, pNVarChar(PayrollDetailBase.Property_MiscRep1, 50, payrollDetailObject.MiscRep1));
			AddParameter(cmd, pDouble(PayrollDetailBase.Property_MiscCommission1, payrollDetailObject.MiscCommission1));
			AddParameter(cmd, pNVarChar(PayrollDetailBase.Property_MiscRep2, 50, payrollDetailObject.MiscRep2));
			AddParameter(cmd, pDouble(PayrollDetailBase.Property_MiscCommission2, payrollDetailObject.MiscCommission2));
			AddParameter(cmd, pNVarChar(PayrollDetailBase.Property_MiscRep3, 50, payrollDetailObject.MiscRep3));
			AddParameter(cmd, pDouble(PayrollDetailBase.Property_MiscCommission3, payrollDetailObject.MiscCommission3));
			AddParameter(cmd, pNVarChar(PayrollDetailBase.Property_MiscRep4, 50, payrollDetailObject.MiscRep4));
			AddParameter(cmd, pDouble(PayrollDetailBase.Property_MiscCommission4, payrollDetailObject.MiscCommission4));
			AddParameter(cmd, pNVarChar(PayrollDetailBase.Property_MiscRep5, 50, payrollDetailObject.MiscRep5));
			AddParameter(cmd, pDouble(PayrollDetailBase.Property_MiscCommission5, payrollDetailObject.MiscCommission5));
			AddParameter(cmd, pDateTime(PayrollDetailBase.Property_MiscPaidDate, payrollDetailObject.MiscPaidDate));
			AddParameter(cmd, pGuid(PayrollDetailBase.Property_CreatedBy, payrollDetailObject.CreatedBy));
			AddParameter(cmd, pDateTime(PayrollDetailBase.Property_CreatedDate, payrollDetailObject.CreatedDate));
			AddParameter(cmd, pGuid(PayrollDetailBase.Property_LastUpdatedBy, payrollDetailObject.LastUpdatedBy));
			AddParameter(cmd, pDateTime(PayrollDetailBase.Property_LastUpdatedDate, payrollDetailObject.LastUpdatedDate));
			AddParameter(cmd, pInt32(PayrollDetailBase.Property_RMAAccountNo, payrollDetailObject.RMAAccountNo));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PayrollDetail
        /// </summary>
        /// <param name="payrollDetailObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PayrollDetailBase payrollDetailObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPAYROLLDETAIL);
	
				AddParameter(cmd, pInt32Out(PayrollDetailBase.Property_Id));
				AddCommonParams(cmd, payrollDetailObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					payrollDetailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					payrollDetailObject.Id = (Int32)GetOutParameter(cmd, PayrollDetailBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(payrollDetailObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PayrollDetail
        /// </summary>
        /// <param name="payrollDetailObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PayrollDetailBase payrollDetailObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPAYROLLDETAIL);
				
				AddParameter(cmd, pInt32(PayrollDetailBase.Property_Id, payrollDetailObject.Id));
				AddCommonParams(cmd, payrollDetailObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					payrollDetailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(payrollDetailObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PayrollDetail
        /// </summary>
        /// <param name="Id">Id of the PayrollDetail object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPAYROLLDETAIL);	
				
				AddParameter(cmd, pInt32(PayrollDetailBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PayrollDetail), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PayrollDetail object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PayrollDetail object to retrieve</param>
        /// <returns>PayrollDetail object, null if not found</returns>
		public PayrollDetail Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLDETAILBYID))
			{
				AddParameter( cmd, pInt32(PayrollDetailBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PayrollDetail objects 
        /// </summary>
        /// <returns>A list of PayrollDetail objects</returns>
		public PayrollDetailList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPAYROLLDETAIL))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PayrollDetail objects by PageRequest
        /// </summary>
        /// <returns>A list of PayrollDetail objects</returns>
		public PayrollDetailList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPAYROLLDETAIL))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PayrollDetailList _PayrollDetailList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PayrollDetailList;
			}
		}
		
		/// <summary>
        /// Retrieves all PayrollDetail objects by query String
        /// </summary>
        /// <returns>A list of PayrollDetail objects</returns>
		public PayrollDetailList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLDETAILBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get PayrollDetail Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of PayrollDetail
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLDETAILMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PayrollDetail Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PayrollDetail
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PayrollDetailRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLDETAILROWCOUNT))
			{
				SqlDataReader reader;
				_PayrollDetailRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PayrollDetailRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PayrollDetail object
        /// </summary>
        /// <param name="payrollDetailObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PayrollDetailBase payrollDetailObject, SqlDataReader reader, int start)
		{
			
				payrollDetailObject.Id = reader.GetInt32( start + 0 );			
				payrollDetailObject.CompanyId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) payrollDetailObject.RepName = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) payrollDetailObject.RepCommission = reader.GetDouble( start + 3 );			
				if(!reader.IsDBNull(4)) payrollDetailObject.RepHoldback = reader.GetDouble( start + 4 );			
				if(!reader.IsDBNull(5)) payrollDetailObject.OverrideRep1 = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) payrollDetailObject.Override1 = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) payrollDetailObject.OverrideRep2 = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) payrollDetailObject.Override2 = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) payrollDetailObject.RepPaidDate = reader.GetDateTime( start + 9 );			
				if(!reader.IsDBNull(10)) payrollDetailObject.TechName = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) payrollDetailObject.TechPay = reader.GetDouble( start + 11 );			
				if(!reader.IsDBNull(12)) payrollDetailObject.TechHoldback = reader.GetDouble( start + 12 );			
				if(!reader.IsDBNull(13)) payrollDetailObject.TechPaidDate = reader.GetDateTime( start + 13 );			
				if(!reader.IsDBNull(14)) payrollDetailObject.OpenerCommission = reader.GetDouble( start + 14 );			
				if(!reader.IsDBNull(15)) payrollDetailObject.MiscRep1 = reader.GetString( start + 15 );			
				if(!reader.IsDBNull(16)) payrollDetailObject.MiscCommission1 = reader.GetDouble( start + 16 );			
				if(!reader.IsDBNull(17)) payrollDetailObject.MiscRep2 = reader.GetString( start + 17 );			
				if(!reader.IsDBNull(18)) payrollDetailObject.MiscCommission2 = reader.GetDouble( start + 18 );			
				if(!reader.IsDBNull(19)) payrollDetailObject.MiscRep3 = reader.GetString( start + 19 );			
				if(!reader.IsDBNull(20)) payrollDetailObject.MiscCommission3 = reader.GetDouble( start + 20 );			
				if(!reader.IsDBNull(21)) payrollDetailObject.MiscRep4 = reader.GetString( start + 21 );			
				if(!reader.IsDBNull(22)) payrollDetailObject.MiscCommission4 = reader.GetDouble( start + 22 );			
				if(!reader.IsDBNull(23)) payrollDetailObject.MiscRep5 = reader.GetString( start + 23 );			
				if(!reader.IsDBNull(24)) payrollDetailObject.MiscCommission5 = reader.GetDouble( start + 24 );			
				if(!reader.IsDBNull(25)) payrollDetailObject.MiscPaidDate = reader.GetDateTime( start + 25 );			
				payrollDetailObject.CreatedBy = reader.GetGuid( start + 26 );			
				payrollDetailObject.CreatedDate = reader.GetDateTime( start + 27 );			
				payrollDetailObject.LastUpdatedBy = reader.GetGuid( start + 28 );			
				payrollDetailObject.LastUpdatedDate = reader.GetDateTime( start + 29 );			
				if(!reader.IsDBNull(30)) payrollDetailObject.RMAAccountNo = reader.GetInt32( start + 30 );			
			FillBaseObject(payrollDetailObject, reader, (start + 31));

			
			payrollDetailObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PayrollDetail object
        /// </summary>
        /// <param name="payrollDetailObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PayrollDetailBase payrollDetailObject, SqlDataReader reader)
		{
			FillObject(payrollDetailObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PayrollDetail object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PayrollDetail object</returns>
		private PayrollDetail GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PayrollDetail payrollDetailObject= new PayrollDetail();
					FillObject(payrollDetailObject, reader);
					return payrollDetailObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PayrollDetail objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PayrollDetail objects</returns>
		private PayrollDetailList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PayrollDetail list
			PayrollDetailList list = new PayrollDetailList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PayrollDetail payrollDetailObject = new PayrollDetail();
					FillObject(payrollDetailObject, reader);

					list.Add(payrollDetailObject);
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
