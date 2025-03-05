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
	public partial class PayrollMonthlyProductionBonusDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPAYROLLMONTHLYPRODUCTIONBONUS = "InsertPayrollMonthlyProductionBonus";
		private const string UPDATEPAYROLLMONTHLYPRODUCTIONBONUS = "UpdatePayrollMonthlyProductionBonus";
		private const string DELETEPAYROLLMONTHLYPRODUCTIONBONUS = "DeletePayrollMonthlyProductionBonus";
		private const string GETPAYROLLMONTHLYPRODUCTIONBONUSBYID = "GetPayrollMonthlyProductionBonusById";
		private const string GETALLPAYROLLMONTHLYPRODUCTIONBONUS = "GetAllPayrollMonthlyProductionBonus";
		private const string GETPAGEDPAYROLLMONTHLYPRODUCTIONBONUS = "GetPagedPayrollMonthlyProductionBonus";
		private const string GETPAYROLLMONTHLYPRODUCTIONBONUSMAXIMUMID = "GetPayrollMonthlyProductionBonusMaximumId";
		private const string GETPAYROLLMONTHLYPRODUCTIONBONUSROWCOUNT = "GetPayrollMonthlyProductionBonusRowCount";	
		private const string GETPAYROLLMONTHLYPRODUCTIONBONUSBYQUERY = "GetPayrollMonthlyProductionBonusByQuery";
		#endregion
		
		#region Constructors
		public PayrollMonthlyProductionBonusDataAccess(ClientContext context) : base(context) { }
		public PayrollMonthlyProductionBonusDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="payrollMonthlyProductionBonusObject"></param>
		private void AddCommonParams(SqlCommand cmd, PayrollMonthlyProductionBonusBase payrollMonthlyProductionBonusObject)
		{	
			AddParameter(cmd, pGuid(PayrollMonthlyProductionBonusBase.Property_PayrollMonthlyProductionBonusId, payrollMonthlyProductionBonusObject.PayrollMonthlyProductionBonusId));
			AddParameter(cmd, pGuid(PayrollMonthlyProductionBonusBase.Property_CompanyId, payrollMonthlyProductionBonusObject.CompanyId));
			AddParameter(cmd, pInt32(PayrollMonthlyProductionBonusBase.Property_MonthlyProductionBonusMin, payrollMonthlyProductionBonusObject.MonthlyProductionBonusMin));
			AddParameter(cmd, pInt32(PayrollMonthlyProductionBonusBase.Property_MonthlyProductionBonusMax, payrollMonthlyProductionBonusObject.MonthlyProductionBonusMax));
			AddParameter(cmd, pInt32(PayrollMonthlyProductionBonusBase.Property_Point, payrollMonthlyProductionBonusObject.Point));
			AddParameter(cmd, pGuid(PayrollMonthlyProductionBonusBase.Property_CreatedBy, payrollMonthlyProductionBonusObject.CreatedBy));
			AddParameter(cmd, pDateTime(PayrollMonthlyProductionBonusBase.Property_CreatedDate, payrollMonthlyProductionBonusObject.CreatedDate));
			AddParameter(cmd, pGuid(PayrollMonthlyProductionBonusBase.Property_LastUpdateBy, payrollMonthlyProductionBonusObject.LastUpdateBy));
			AddParameter(cmd, pDateTime(PayrollMonthlyProductionBonusBase.Property_LastUpdateDate, payrollMonthlyProductionBonusObject.LastUpdateDate));
			AddParameter(cmd, pGuid(PayrollMonthlyProductionBonusBase.Property_TermSheetId, payrollMonthlyProductionBonusObject.TermSheetId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PayrollMonthlyProductionBonus
        /// </summary>
        /// <param name="payrollMonthlyProductionBonusObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PayrollMonthlyProductionBonusBase payrollMonthlyProductionBonusObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPAYROLLMONTHLYPRODUCTIONBONUS);
	
				AddParameter(cmd, pInt32Out(PayrollMonthlyProductionBonusBase.Property_Id));
				AddCommonParams(cmd, payrollMonthlyProductionBonusObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					payrollMonthlyProductionBonusObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					payrollMonthlyProductionBonusObject.Id = (Int32)GetOutParameter(cmd, PayrollMonthlyProductionBonusBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(payrollMonthlyProductionBonusObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PayrollMonthlyProductionBonus
        /// </summary>
        /// <param name="payrollMonthlyProductionBonusObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PayrollMonthlyProductionBonusBase payrollMonthlyProductionBonusObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPAYROLLMONTHLYPRODUCTIONBONUS);
				
				AddParameter(cmd, pInt32(PayrollMonthlyProductionBonusBase.Property_Id, payrollMonthlyProductionBonusObject.Id));
				AddCommonParams(cmd, payrollMonthlyProductionBonusObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					payrollMonthlyProductionBonusObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(payrollMonthlyProductionBonusObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PayrollMonthlyProductionBonus
        /// </summary>
        /// <param name="Id">Id of the PayrollMonthlyProductionBonus object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPAYROLLMONTHLYPRODUCTIONBONUS);	
				
				AddParameter(cmd, pInt32(PayrollMonthlyProductionBonusBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PayrollMonthlyProductionBonus), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PayrollMonthlyProductionBonus object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PayrollMonthlyProductionBonus object to retrieve</param>
        /// <returns>PayrollMonthlyProductionBonus object, null if not found</returns>
		public PayrollMonthlyProductionBonus Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLMONTHLYPRODUCTIONBONUSBYID))
			{
				AddParameter( cmd, pInt32(PayrollMonthlyProductionBonusBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PayrollMonthlyProductionBonus objects 
        /// </summary>
        /// <returns>A list of PayrollMonthlyProductionBonus objects</returns>
		public PayrollMonthlyProductionBonusList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPAYROLLMONTHLYPRODUCTIONBONUS))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PayrollMonthlyProductionBonus objects by PageRequest
        /// </summary>
        /// <returns>A list of PayrollMonthlyProductionBonus objects</returns>
		public PayrollMonthlyProductionBonusList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPAYROLLMONTHLYPRODUCTIONBONUS))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PayrollMonthlyProductionBonusList _PayrollMonthlyProductionBonusList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PayrollMonthlyProductionBonusList;
			}
		}
		
		/// <summary>
        /// Retrieves all PayrollMonthlyProductionBonus objects by query String
        /// </summary>
        /// <returns>A list of PayrollMonthlyProductionBonus objects</returns>
		public PayrollMonthlyProductionBonusList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLMONTHLYPRODUCTIONBONUSBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get PayrollMonthlyProductionBonus Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of PayrollMonthlyProductionBonus
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLMONTHLYPRODUCTIONBONUSMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PayrollMonthlyProductionBonus Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PayrollMonthlyProductionBonus
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PayrollMonthlyProductionBonusRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLMONTHLYPRODUCTIONBONUSROWCOUNT))
			{
				SqlDataReader reader;
				_PayrollMonthlyProductionBonusRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PayrollMonthlyProductionBonusRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PayrollMonthlyProductionBonus object
        /// </summary>
        /// <param name="payrollMonthlyProductionBonusObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PayrollMonthlyProductionBonusBase payrollMonthlyProductionBonusObject, SqlDataReader reader, int start)
		{
			
				payrollMonthlyProductionBonusObject.Id = reader.GetInt32( start + 0 );			
				payrollMonthlyProductionBonusObject.PayrollMonthlyProductionBonusId = reader.GetGuid( start + 1 );			
				payrollMonthlyProductionBonusObject.CompanyId = reader.GetGuid( start + 2 );			
				payrollMonthlyProductionBonusObject.MonthlyProductionBonusMin = reader.GetInt32( start + 3 );			
				payrollMonthlyProductionBonusObject.MonthlyProductionBonusMax = reader.GetInt32( start + 4 );			
				payrollMonthlyProductionBonusObject.Point = reader.GetInt32( start + 5 );			
				payrollMonthlyProductionBonusObject.CreatedBy = reader.GetGuid( start + 6 );			
				if(!reader.IsDBNull(7)) payrollMonthlyProductionBonusObject.CreatedDate = reader.GetDateTime( start + 7 );			
				payrollMonthlyProductionBonusObject.LastUpdateBy = reader.GetGuid( start + 8 );			
				if(!reader.IsDBNull(9)) payrollMonthlyProductionBonusObject.LastUpdateDate = reader.GetDateTime( start + 9 );			
				payrollMonthlyProductionBonusObject.TermSheetId = reader.GetGuid( start + 10 );			
			FillBaseObject(payrollMonthlyProductionBonusObject, reader, (start + 11));

			
			payrollMonthlyProductionBonusObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PayrollMonthlyProductionBonus object
        /// </summary>
        /// <param name="payrollMonthlyProductionBonusObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PayrollMonthlyProductionBonusBase payrollMonthlyProductionBonusObject, SqlDataReader reader)
		{
			FillObject(payrollMonthlyProductionBonusObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PayrollMonthlyProductionBonus object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PayrollMonthlyProductionBonus object</returns>
		private PayrollMonthlyProductionBonus GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PayrollMonthlyProductionBonus payrollMonthlyProductionBonusObject= new PayrollMonthlyProductionBonus();
					FillObject(payrollMonthlyProductionBonusObject, reader);
					return payrollMonthlyProductionBonusObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PayrollMonthlyProductionBonus objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PayrollMonthlyProductionBonus objects</returns>
		private PayrollMonthlyProductionBonusList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PayrollMonthlyProductionBonus list
			PayrollMonthlyProductionBonusList list = new PayrollMonthlyProductionBonusList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PayrollMonthlyProductionBonus payrollMonthlyProductionBonusObject = new PayrollMonthlyProductionBonus();
					FillObject(payrollMonthlyProductionBonusObject, reader);

					list.Add(payrollMonthlyProductionBonusObject);
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
