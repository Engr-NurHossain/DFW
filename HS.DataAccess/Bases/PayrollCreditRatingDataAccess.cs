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
	public partial class PayrollCreditRatingDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPAYROLLCREDITRATING = "InsertPayrollCreditRating";
		private const string UPDATEPAYROLLCREDITRATING = "UpdatePayrollCreditRating";
		private const string DELETEPAYROLLCREDITRATING = "DeletePayrollCreditRating";
		private const string GETPAYROLLCREDITRATINGBYID = "GetPayrollCreditRatingById";
		private const string GETALLPAYROLLCREDITRATING = "GetAllPayrollCreditRating";
		private const string GETPAGEDPAYROLLCREDITRATING = "GetPagedPayrollCreditRating";
		private const string GETPAYROLLCREDITRATINGMAXIMUMID = "GetPayrollCreditRatingMaximumId";
		private const string GETPAYROLLCREDITRATINGROWCOUNT = "GetPayrollCreditRatingRowCount";	
		private const string GETPAYROLLCREDITRATINGBYQUERY = "GetPayrollCreditRatingByQuery";
		#endregion
		
		#region Constructors
		public PayrollCreditRatingDataAccess(ClientContext context) : base(context) { }
		public PayrollCreditRatingDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="payrollCreditRatingObject"></param>
		private void AddCommonParams(SqlCommand cmd, PayrollCreditRatingBase payrollCreditRatingObject)
		{	
			AddParameter(cmd, pGuid(PayrollCreditRatingBase.Property_PayrollCreditRatingId, payrollCreditRatingObject.PayrollCreditRatingId));
			AddParameter(cmd, pGuid(PayrollCreditRatingBase.Property_CompanyId, payrollCreditRatingObject.CompanyId));
			AddParameter(cmd, pInt32(PayrollCreditRatingBase.Property_MinCredit, payrollCreditRatingObject.MinCredit));
			AddParameter(cmd, pInt32(PayrollCreditRatingBase.Property_MaxCredit, payrollCreditRatingObject.MaxCredit));
			AddParameter(cmd, pInt32(PayrollCreditRatingBase.Property_Point, payrollCreditRatingObject.Point));
			AddParameter(cmd, pGuid(PayrollCreditRatingBase.Property_CreatedBy, payrollCreditRatingObject.CreatedBy));
			AddParameter(cmd, pDateTime(PayrollCreditRatingBase.Property_CreatedDate, payrollCreditRatingObject.CreatedDate));
			AddParameter(cmd, pGuid(PayrollCreditRatingBase.Property_LastUpdateBy, payrollCreditRatingObject.LastUpdateBy));
			AddParameter(cmd, pDateTime(PayrollCreditRatingBase.Property_LastUpdateDate, payrollCreditRatingObject.LastUpdateDate));
			AddParameter(cmd, pGuid(PayrollCreditRatingBase.Property_TermSheetId, payrollCreditRatingObject.TermSheetId));
			AddParameter(cmd, pBool(PayrollCreditRatingBase.Property_ACHBonusWaived, payrollCreditRatingObject.ACHBonusWaived));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PayrollCreditRating
        /// </summary>
        /// <param name="payrollCreditRatingObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PayrollCreditRatingBase payrollCreditRatingObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPAYROLLCREDITRATING);
	
				AddParameter(cmd, pInt32Out(PayrollCreditRatingBase.Property_Id));
				AddCommonParams(cmd, payrollCreditRatingObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					payrollCreditRatingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					payrollCreditRatingObject.Id = (Int32)GetOutParameter(cmd, PayrollCreditRatingBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(payrollCreditRatingObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PayrollCreditRating
        /// </summary>
        /// <param name="payrollCreditRatingObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PayrollCreditRatingBase payrollCreditRatingObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPAYROLLCREDITRATING);
				
				AddParameter(cmd, pInt32(PayrollCreditRatingBase.Property_Id, payrollCreditRatingObject.Id));
				AddCommonParams(cmd, payrollCreditRatingObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					payrollCreditRatingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(payrollCreditRatingObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PayrollCreditRating
        /// </summary>
        /// <param name="Id">Id of the PayrollCreditRating object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPAYROLLCREDITRATING);	
				
				AddParameter(cmd, pInt32(PayrollCreditRatingBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PayrollCreditRating), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PayrollCreditRating object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PayrollCreditRating object to retrieve</param>
        /// <returns>PayrollCreditRating object, null if not found</returns>
		public PayrollCreditRating Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLCREDITRATINGBYID))
			{
				AddParameter( cmd, pInt32(PayrollCreditRatingBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PayrollCreditRating objects 
        /// </summary>
        /// <returns>A list of PayrollCreditRating objects</returns>
		public PayrollCreditRatingList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPAYROLLCREDITRATING))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PayrollCreditRating objects by PageRequest
        /// </summary>
        /// <returns>A list of PayrollCreditRating objects</returns>
		public PayrollCreditRatingList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPAYROLLCREDITRATING))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PayrollCreditRatingList _PayrollCreditRatingList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PayrollCreditRatingList;
			}
		}
		
		/// <summary>
        /// Retrieves all PayrollCreditRating objects by query String
        /// </summary>
        /// <returns>A list of PayrollCreditRating objects</returns>
		public PayrollCreditRatingList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLCREDITRATINGBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get PayrollCreditRating Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of PayrollCreditRating
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLCREDITRATINGMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PayrollCreditRating Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PayrollCreditRating
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PayrollCreditRatingRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLCREDITRATINGROWCOUNT))
			{
				SqlDataReader reader;
				_PayrollCreditRatingRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PayrollCreditRatingRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PayrollCreditRating object
        /// </summary>
        /// <param name="payrollCreditRatingObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PayrollCreditRatingBase payrollCreditRatingObject, SqlDataReader reader, int start)
		{
			
				payrollCreditRatingObject.Id = reader.GetInt32( start + 0 );			
				payrollCreditRatingObject.PayrollCreditRatingId = reader.GetGuid( start + 1 );			
				payrollCreditRatingObject.CompanyId = reader.GetGuid( start + 2 );			
				payrollCreditRatingObject.MinCredit = reader.GetInt32( start + 3 );			
				payrollCreditRatingObject.MaxCredit = reader.GetInt32( start + 4 );			
				payrollCreditRatingObject.Point = reader.GetInt32( start + 5 );			
				payrollCreditRatingObject.CreatedBy = reader.GetGuid( start + 6 );			
				if(!reader.IsDBNull(7)) payrollCreditRatingObject.CreatedDate = reader.GetDateTime( start + 7 );			
				payrollCreditRatingObject.LastUpdateBy = reader.GetGuid( start + 8 );			
				if(!reader.IsDBNull(9)) payrollCreditRatingObject.LastUpdateDate = reader.GetDateTime( start + 9 );			
				payrollCreditRatingObject.TermSheetId = reader.GetGuid( start + 10 );			
				if(!reader.IsDBNull(11)) payrollCreditRatingObject.ACHBonusWaived = reader.GetBoolean( start + 11 );			
			FillBaseObject(payrollCreditRatingObject, reader, (start + 12));

			
			payrollCreditRatingObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PayrollCreditRating object
        /// </summary>
        /// <param name="payrollCreditRatingObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PayrollCreditRatingBase payrollCreditRatingObject, SqlDataReader reader)
		{
			FillObject(payrollCreditRatingObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PayrollCreditRating object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PayrollCreditRating object</returns>
		private PayrollCreditRating GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PayrollCreditRating payrollCreditRatingObject= new PayrollCreditRating();
					FillObject(payrollCreditRatingObject, reader);
					return payrollCreditRatingObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PayrollCreditRating objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PayrollCreditRating objects</returns>
		private PayrollCreditRatingList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PayrollCreditRating list
			PayrollCreditRatingList list = new PayrollCreditRatingList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PayrollCreditRating payrollCreditRatingObject = new PayrollCreditRating();
					FillObject(payrollCreditRatingObject, reader);

					list.Add(payrollCreditRatingObject);
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
