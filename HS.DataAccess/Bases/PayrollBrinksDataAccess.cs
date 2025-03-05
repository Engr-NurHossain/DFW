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
	public partial class PayrollBrinksDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPAYROLLBRINKS = "InsertPayrollBrinks";
		private const string UPDATEPAYROLLBRINKS = "UpdatePayrollBrinks";
		private const string DELETEPAYROLLBRINKS = "DeletePayrollBrinks";
		private const string GETPAYROLLBRINKSBYID = "GetPayrollBrinksById";
		private const string GETALLPAYROLLBRINKS = "GetAllPayrollBrinks";
		private const string GETPAGEDPAYROLLBRINKS = "GetPagedPayrollBrinks";
		private const string GETPAYROLLBRINKSMAXIMUMID = "GetPayrollBrinksMaximumId";
		private const string GETPAYROLLBRINKSROWCOUNT = "GetPayrollBrinksRowCount";	
		private const string GETPAYROLLBRINKSBYQUERY = "GetPayrollBrinksByQuery";
		#endregion
		
		#region Constructors
		public PayrollBrinksDataAccess(ClientContext context) : base(context) { }
		public PayrollBrinksDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="payrollBrinksObject"></param>
		private void AddCommonParams(SqlCommand cmd, PayrollBrinksBase payrollBrinksObject)
		{	
			AddParameter(cmd, pGuid(PayrollBrinksBase.Property_PayrollBrinksId, payrollBrinksObject.PayrollBrinksId));
			AddParameter(cmd, pGuid(PayrollBrinksBase.Property_CustomerId, payrollBrinksObject.CustomerId));
			AddParameter(cmd, pGuid(PayrollBrinksBase.Property_SalesPersonId, payrollBrinksObject.SalesPersonId));
			AddParameter(cmd, pGuid(PayrollBrinksBase.Property_TicketId, payrollBrinksObject.TicketId));
			AddParameter(cmd, pDouble(PayrollBrinksBase.Property_MMR, payrollBrinksObject.MMR));
			AddParameter(cmd, pDouble(PayrollBrinksBase.Property_Multiple, payrollBrinksObject.Multiple));
			AddParameter(cmd, pDouble(PayrollBrinksBase.Property_GrossPay, payrollBrinksObject.GrossPay));
			AddParameter(cmd, pDouble(PayrollBrinksBase.Property_Deductions, payrollBrinksObject.Deductions));
			AddParameter(cmd, pDouble(PayrollBrinksBase.Property_Adjustments, payrollBrinksObject.Adjustments));
			AddParameter(cmd, pDouble(PayrollBrinksBase.Property_NetPay, payrollBrinksObject.NetPay));
			AddParameter(cmd, pGuid(PayrollBrinksBase.Property_CreatedBy, payrollBrinksObject.CreatedBy));
			AddParameter(cmd, pDateTime(PayrollBrinksBase.Property_CreatedDate, payrollBrinksObject.CreatedDate));
			AddParameter(cmd, pGuid(PayrollBrinksBase.Property_LastUpdateBy, payrollBrinksObject.LastUpdateBy));
			AddParameter(cmd, pDateTime(PayrollBrinksBase.Property_LastUpdateDate, payrollBrinksObject.LastUpdateDate));
			AddParameter(cmd, pDouble(PayrollBrinksBase.Property_HoldBack, payrollBrinksObject.HoldBack));
			AddParameter(cmd, pDouble(PayrollBrinksBase.Property_PassThrus, payrollBrinksObject.PassThrus));
			AddParameter(cmd, pNVarChar(PayrollBrinksBase.Property_FundingStatus, 50, payrollBrinksObject.FundingStatus));
			AddParameter(cmd, pBool(PayrollBrinksBase.Property_IsPaid, payrollBrinksObject.IsPaid));
			AddParameter(cmd, pInt32(PayrollBrinksBase.Property_BatchNo, payrollBrinksObject.BatchNo));
			AddParameter(cmd, pBool(PayrollBrinksBase.Property_IsManagerPayroll, payrollBrinksObject.IsManagerPayroll));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PayrollBrinks
        /// </summary>
        /// <param name="payrollBrinksObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PayrollBrinksBase payrollBrinksObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPAYROLLBRINKS);
	
				AddParameter(cmd, pInt32Out(PayrollBrinksBase.Property_Id));
				AddCommonParams(cmd, payrollBrinksObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					payrollBrinksObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					payrollBrinksObject.Id = (Int32)GetOutParameter(cmd, PayrollBrinksBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(payrollBrinksObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PayrollBrinks
        /// </summary>
        /// <param name="payrollBrinksObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PayrollBrinksBase payrollBrinksObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPAYROLLBRINKS);
				
				AddParameter(cmd, pInt32(PayrollBrinksBase.Property_Id, payrollBrinksObject.Id));
				AddCommonParams(cmd, payrollBrinksObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					payrollBrinksObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(payrollBrinksObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PayrollBrinks
        /// </summary>
        /// <param name="Id">Id of the PayrollBrinks object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPAYROLLBRINKS);	
				
				AddParameter(cmd, pInt32(PayrollBrinksBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PayrollBrinks), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PayrollBrinks object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PayrollBrinks object to retrieve</param>
        /// <returns>PayrollBrinks object, null if not found</returns>
		public PayrollBrinks Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLBRINKSBYID))
			{
				AddParameter( cmd, pInt32(PayrollBrinksBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PayrollBrinks objects 
        /// </summary>
        /// <returns>A list of PayrollBrinks objects</returns>
		public PayrollBrinksList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPAYROLLBRINKS))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PayrollBrinks objects by PageRequest
        /// </summary>
        /// <returns>A list of PayrollBrinks objects</returns>
		public PayrollBrinksList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPAYROLLBRINKS))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PayrollBrinksList _PayrollBrinksList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PayrollBrinksList;
			}
		}
		
		/// <summary>
        /// Retrieves all PayrollBrinks objects by query String
        /// </summary>
        /// <returns>A list of PayrollBrinks objects</returns>
		public PayrollBrinksList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLBRINKSBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get PayrollBrinks Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of PayrollBrinks
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLBRINKSMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PayrollBrinks Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PayrollBrinks
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PayrollBrinksRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLBRINKSROWCOUNT))
			{
				SqlDataReader reader;
				_PayrollBrinksRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PayrollBrinksRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PayrollBrinks object
        /// </summary>
        /// <param name="payrollBrinksObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PayrollBrinksBase payrollBrinksObject, SqlDataReader reader, int start)
		{
			
				payrollBrinksObject.Id = reader.GetInt32( start + 0 );			
				payrollBrinksObject.PayrollBrinksId = reader.GetGuid( start + 1 );			
				payrollBrinksObject.CustomerId = reader.GetGuid( start + 2 );			
				payrollBrinksObject.SalesPersonId = reader.GetGuid( start + 3 );			
				payrollBrinksObject.TicketId = reader.GetGuid( start + 4 );			
				if(!reader.IsDBNull(5)) payrollBrinksObject.MMR = reader.GetDouble( start + 5 );			
				if(!reader.IsDBNull(6)) payrollBrinksObject.Multiple = reader.GetDouble( start + 6 );			
				if(!reader.IsDBNull(7)) payrollBrinksObject.GrossPay = reader.GetDouble( start + 7 );			
				if(!reader.IsDBNull(8)) payrollBrinksObject.Deductions = reader.GetDouble( start + 8 );			
				if(!reader.IsDBNull(9)) payrollBrinksObject.Adjustments = reader.GetDouble( start + 9 );			
				if(!reader.IsDBNull(10)) payrollBrinksObject.NetPay = reader.GetDouble( start + 10 );			
				payrollBrinksObject.CreatedBy = reader.GetGuid( start + 11 );			
				if(!reader.IsDBNull(12)) payrollBrinksObject.CreatedDate = reader.GetDateTime( start + 12 );			
				payrollBrinksObject.LastUpdateBy = reader.GetGuid( start + 13 );			
				if(!reader.IsDBNull(14)) payrollBrinksObject.LastUpdateDate = reader.GetDateTime( start + 14 );			
				if(!reader.IsDBNull(15)) payrollBrinksObject.HoldBack = reader.GetDouble( start + 15 );			
				if(!reader.IsDBNull(16)) payrollBrinksObject.PassThrus = reader.GetDouble( start + 16 );			
				if(!reader.IsDBNull(17)) payrollBrinksObject.FundingStatus = reader.GetString( start + 17 );			
				if(!reader.IsDBNull(18)) payrollBrinksObject.IsPaid = reader.GetBoolean( start + 18 );			
				if(!reader.IsDBNull(19)) payrollBrinksObject.BatchNo = reader.GetInt32( start + 19 );			
				if(!reader.IsDBNull(20)) payrollBrinksObject.IsManagerPayroll = reader.GetBoolean( start + 20 );			
			FillBaseObject(payrollBrinksObject, reader, (start + 21));

			
			payrollBrinksObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PayrollBrinks object
        /// </summary>
        /// <param name="payrollBrinksObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PayrollBrinksBase payrollBrinksObject, SqlDataReader reader)
		{
			FillObject(payrollBrinksObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PayrollBrinks object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PayrollBrinks object</returns>
		private PayrollBrinks GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PayrollBrinks payrollBrinksObject= new PayrollBrinks();
					FillObject(payrollBrinksObject, reader);
					return payrollBrinksObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PayrollBrinks objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PayrollBrinks objects</returns>
		private PayrollBrinksList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PayrollBrinks list
			PayrollBrinksList list = new PayrollBrinksList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PayrollBrinks payrollBrinksObject = new PayrollBrinks();
					FillObject(payrollBrinksObject, reader);

					list.Add(payrollBrinksObject);
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
