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
	public partial class PayrollHoldBackDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPAYROLLHOLDBACK = "InsertPayrollHoldBack";
		private const string UPDATEPAYROLLHOLDBACK = "UpdatePayrollHoldBack";
		private const string DELETEPAYROLLHOLDBACK = "DeletePayrollHoldBack";
		private const string GETPAYROLLHOLDBACKBYID = "GetPayrollHoldBackById";
		private const string GETALLPAYROLLHOLDBACK = "GetAllPayrollHoldBack";
		private const string GETPAGEDPAYROLLHOLDBACK = "GetPagedPayrollHoldBack";
		private const string GETPAYROLLHOLDBACKMAXIMUMID = "GetPayrollHoldBackMaximumId";
		private const string GETPAYROLLHOLDBACKROWCOUNT = "GetPayrollHoldBackRowCount";	
		private const string GETPAYROLLHOLDBACKBYQUERY = "GetPayrollHoldBackByQuery";
		#endregion
		
		#region Constructors
		public PayrollHoldBackDataAccess(ClientContext context) : base(context) { }
		public PayrollHoldBackDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="payrollHoldBackObject"></param>
		private void AddCommonParams(SqlCommand cmd, PayrollHoldBackBase payrollHoldBackObject)
		{	
			AddParameter(cmd, pGuid(PayrollHoldBackBase.Property_PayrollHoldBackId, payrollHoldBackObject.PayrollHoldBackId));
			AddParameter(cmd, pGuid(PayrollHoldBackBase.Property_CompanyId, payrollHoldBackObject.CompanyId));
			AddParameter(cmd, pNVarChar(PayrollHoldBackBase.Property_HoldBack, 100, payrollHoldBackObject.HoldBack));
			AddParameter(cmd, pDouble(PayrollHoldBackBase.Property_Percentage, payrollHoldBackObject.Percentage));
			AddParameter(cmd, pGuid(PayrollHoldBackBase.Property_CreatedBy, payrollHoldBackObject.CreatedBy));
			AddParameter(cmd, pDateTime(PayrollHoldBackBase.Property_CreatedDate, payrollHoldBackObject.CreatedDate));
			AddParameter(cmd, pGuid(PayrollHoldBackBase.Property_LastUpdateBy, payrollHoldBackObject.LastUpdateBy));
			AddParameter(cmd, pDateTime(PayrollHoldBackBase.Property_LastUpdateDate, payrollHoldBackObject.LastUpdateDate));
			AddParameter(cmd, pGuid(PayrollHoldBackBase.Property_TermSheetId, payrollHoldBackObject.TermSheetId));
			AddParameter(cmd, pNVarChar(PayrollHoldBackBase.Property_Type, 100, payrollHoldBackObject.Type));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PayrollHoldBack
        /// </summary>
        /// <param name="payrollHoldBackObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PayrollHoldBackBase payrollHoldBackObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPAYROLLHOLDBACK);
	
				AddParameter(cmd, pInt32Out(PayrollHoldBackBase.Property_Id));
				AddCommonParams(cmd, payrollHoldBackObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					payrollHoldBackObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					payrollHoldBackObject.Id = (Int32)GetOutParameter(cmd, PayrollHoldBackBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(payrollHoldBackObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PayrollHoldBack
        /// </summary>
        /// <param name="payrollHoldBackObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PayrollHoldBackBase payrollHoldBackObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPAYROLLHOLDBACK);
				
				AddParameter(cmd, pInt32(PayrollHoldBackBase.Property_Id, payrollHoldBackObject.Id));
				AddCommonParams(cmd, payrollHoldBackObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					payrollHoldBackObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(payrollHoldBackObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PayrollHoldBack
        /// </summary>
        /// <param name="Id">Id of the PayrollHoldBack object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPAYROLLHOLDBACK);	
				
				AddParameter(cmd, pInt32(PayrollHoldBackBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PayrollHoldBack), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PayrollHoldBack object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PayrollHoldBack object to retrieve</param>
        /// <returns>PayrollHoldBack object, null if not found</returns>
		public PayrollHoldBack Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLHOLDBACKBYID))
			{
				AddParameter( cmd, pInt32(PayrollHoldBackBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PayrollHoldBack objects 
        /// </summary>
        /// <returns>A list of PayrollHoldBack objects</returns>
		public PayrollHoldBackList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPAYROLLHOLDBACK))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PayrollHoldBack objects by PageRequest
        /// </summary>
        /// <returns>A list of PayrollHoldBack objects</returns>
		public PayrollHoldBackList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPAYROLLHOLDBACK))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PayrollHoldBackList _PayrollHoldBackList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PayrollHoldBackList;
			}
		}
		
		/// <summary>
        /// Retrieves all PayrollHoldBack objects by query String
        /// </summary>
        /// <returns>A list of PayrollHoldBack objects</returns>
		public PayrollHoldBackList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLHOLDBACKBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get PayrollHoldBack Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of PayrollHoldBack
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLHOLDBACKMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PayrollHoldBack Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PayrollHoldBack
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PayrollHoldBackRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLHOLDBACKROWCOUNT))
			{
				SqlDataReader reader;
				_PayrollHoldBackRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PayrollHoldBackRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PayrollHoldBack object
        /// </summary>
        /// <param name="payrollHoldBackObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PayrollHoldBackBase payrollHoldBackObject, SqlDataReader reader, int start)
		{
			
				payrollHoldBackObject.Id = reader.GetInt32( start + 0 );			
				payrollHoldBackObject.PayrollHoldBackId = reader.GetGuid( start + 1 );			
				payrollHoldBackObject.CompanyId = reader.GetGuid( start + 2 );			
				payrollHoldBackObject.HoldBack = reader.GetString( start + 3 );			
				payrollHoldBackObject.Percentage = reader.GetDouble( start + 4 );			
				payrollHoldBackObject.CreatedBy = reader.GetGuid( start + 5 );			
				if(!reader.IsDBNull(6)) payrollHoldBackObject.CreatedDate = reader.GetDateTime( start + 6 );			
				payrollHoldBackObject.LastUpdateBy = reader.GetGuid( start + 7 );			
				if(!reader.IsDBNull(8)) payrollHoldBackObject.LastUpdateDate = reader.GetDateTime( start + 8 );			
				payrollHoldBackObject.TermSheetId = reader.GetGuid( start + 9 );			
				if(!reader.IsDBNull(10)) payrollHoldBackObject.Type = reader.GetString( start + 10 );			
			FillBaseObject(payrollHoldBackObject, reader, (start + 11));

			
			payrollHoldBackObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PayrollHoldBack object
        /// </summary>
        /// <param name="payrollHoldBackObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PayrollHoldBackBase payrollHoldBackObject, SqlDataReader reader)
		{
			FillObject(payrollHoldBackObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PayrollHoldBack object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PayrollHoldBack object</returns>
		private PayrollHoldBack GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PayrollHoldBack payrollHoldBackObject= new PayrollHoldBack();
					FillObject(payrollHoldBackObject, reader);
					return payrollHoldBackObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PayrollHoldBack objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PayrollHoldBack objects</returns>
		private PayrollHoldBackList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PayrollHoldBack list
			PayrollHoldBackList list = new PayrollHoldBackList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PayrollHoldBack payrollHoldBackObject = new PayrollHoldBack();
					FillObject(payrollHoldBackObject, reader);

					list.Add(payrollHoldBackObject);
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
