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
	public partial class PayrollMileageDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPAYROLLMILEAGE = "InsertPayrollMileage";
		private const string UPDATEPAYROLLMILEAGE = "UpdatePayrollMileage";
		private const string DELETEPAYROLLMILEAGE = "DeletePayrollMileage";
		private const string GETPAYROLLMILEAGEBYID = "GetPayrollMileageById";
		private const string GETALLPAYROLLMILEAGE = "GetAllPayrollMileage";
		private const string GETPAGEDPAYROLLMILEAGE = "GetPagedPayrollMileage";
		private const string GETPAYROLLMILEAGEMAXIMUMID = "GetPayrollMileageMaximumId";
		private const string GETPAYROLLMILEAGEROWCOUNT = "GetPayrollMileageRowCount";	
		private const string GETPAYROLLMILEAGEBYQUERY = "GetPayrollMileageByQuery";
		#endregion
		
		#region Constructors
		public PayrollMileageDataAccess(ClientContext context) : base(context) { }
		public PayrollMileageDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="payrollMileageObject"></param>
		private void AddCommonParams(SqlCommand cmd, PayrollMileageBase payrollMileageObject)
		{	
			AddParameter(cmd, pGuid(PayrollMileageBase.Property_PayrollMileageId, payrollMileageObject.PayrollMileageId));
			AddParameter(cmd, pGuid(PayrollMileageBase.Property_CompanyId, payrollMileageObject.CompanyId));
			AddParameter(cmd, pDouble(PayrollMileageBase.Property_MileageMin, payrollMileageObject.MileageMin));
			AddParameter(cmd, pDouble(PayrollMileageBase.Property_MileageMax, payrollMileageObject.MileageMax));
			AddParameter(cmd, pDouble(PayrollMileageBase.Property_Amount, payrollMileageObject.Amount));
			AddParameter(cmd, pGuid(PayrollMileageBase.Property_CreatedBy, payrollMileageObject.CreatedBy));
			AddParameter(cmd, pDateTime(PayrollMileageBase.Property_CreatedDate, payrollMileageObject.CreatedDate));
			AddParameter(cmd, pGuid(PayrollMileageBase.Property_LastUpdateBy, payrollMileageObject.LastUpdateBy));
			AddParameter(cmd, pDateTime(PayrollMileageBase.Property_LastUpdateDate, payrollMileageObject.LastUpdateDate));
			AddParameter(cmd, pGuid(PayrollMileageBase.Property_TermSheetId, payrollMileageObject.TermSheetId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PayrollMileage
        /// </summary>
        /// <param name="payrollMileageObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PayrollMileageBase payrollMileageObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPAYROLLMILEAGE);
	
				AddParameter(cmd, pInt32Out(PayrollMileageBase.Property_Id));
				AddCommonParams(cmd, payrollMileageObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					payrollMileageObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					payrollMileageObject.Id = (Int32)GetOutParameter(cmd, PayrollMileageBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(payrollMileageObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PayrollMileage
        /// </summary>
        /// <param name="payrollMileageObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PayrollMileageBase payrollMileageObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPAYROLLMILEAGE);
				
				AddParameter(cmd, pInt32(PayrollMileageBase.Property_Id, payrollMileageObject.Id));
				AddCommonParams(cmd, payrollMileageObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					payrollMileageObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(payrollMileageObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PayrollMileage
        /// </summary>
        /// <param name="Id">Id of the PayrollMileage object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPAYROLLMILEAGE);	
				
				AddParameter(cmd, pInt32(PayrollMileageBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PayrollMileage), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PayrollMileage object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PayrollMileage object to retrieve</param>
        /// <returns>PayrollMileage object, null if not found</returns>
		public PayrollMileage Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLMILEAGEBYID))
			{
				AddParameter( cmd, pInt32(PayrollMileageBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PayrollMileage objects 
        /// </summary>
        /// <returns>A list of PayrollMileage objects</returns>
		public PayrollMileageList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPAYROLLMILEAGE))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PayrollMileage objects by PageRequest
        /// </summary>
        /// <returns>A list of PayrollMileage objects</returns>
		public PayrollMileageList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPAYROLLMILEAGE))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PayrollMileageList _PayrollMileageList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PayrollMileageList;
			}
		}
		
		/// <summary>
        /// Retrieves all PayrollMileage objects by query String
        /// </summary>
        /// <returns>A list of PayrollMileage objects</returns>
		public PayrollMileageList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLMILEAGEBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get PayrollMileage Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of PayrollMileage
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLMILEAGEMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PayrollMileage Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PayrollMileage
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PayrollMileageRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLMILEAGEROWCOUNT))
			{
				SqlDataReader reader;
				_PayrollMileageRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PayrollMileageRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PayrollMileage object
        /// </summary>
        /// <param name="payrollMileageObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PayrollMileageBase payrollMileageObject, SqlDataReader reader, int start)
		{
			
				payrollMileageObject.Id = reader.GetInt32( start + 0 );			
				payrollMileageObject.PayrollMileageId = reader.GetGuid( start + 1 );			
				payrollMileageObject.CompanyId = reader.GetGuid( start + 2 );			
				payrollMileageObject.MileageMin = reader.GetDouble( start + 3 );			
				payrollMileageObject.MileageMax = reader.GetDouble( start + 4 );			
				payrollMileageObject.Amount = reader.GetDouble( start + 5 );			
				payrollMileageObject.CreatedBy = reader.GetGuid( start + 6 );			
				if(!reader.IsDBNull(7)) payrollMileageObject.CreatedDate = reader.GetDateTime( start + 7 );			
				payrollMileageObject.LastUpdateBy = reader.GetGuid( start + 8 );			
				if(!reader.IsDBNull(9)) payrollMileageObject.LastUpdateDate = reader.GetDateTime( start + 9 );			
				payrollMileageObject.TermSheetId = reader.GetGuid( start + 10 );			
			FillBaseObject(payrollMileageObject, reader, (start + 11));

			
			payrollMileageObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PayrollMileage object
        /// </summary>
        /// <param name="payrollMileageObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PayrollMileageBase payrollMileageObject, SqlDataReader reader)
		{
			FillObject(payrollMileageObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PayrollMileage object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PayrollMileage object</returns>
		private PayrollMileage GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PayrollMileage payrollMileageObject= new PayrollMileage();
					FillObject(payrollMileageObject, reader);
					return payrollMileageObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PayrollMileage objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PayrollMileage objects</returns>
		private PayrollMileageList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PayrollMileage list
			PayrollMileageList list = new PayrollMileageList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PayrollMileage payrollMileageObject = new PayrollMileage();
					FillObject(payrollMileageObject, reader);

					list.Add(payrollMileageObject);
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
