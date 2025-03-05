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
	public partial class PayrollAgreementLengthDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPAYROLLAGREEMENTLENGTH = "InsertPayrollAgreementLength";
		private const string UPDATEPAYROLLAGREEMENTLENGTH = "UpdatePayrollAgreementLength";
		private const string DELETEPAYROLLAGREEMENTLENGTH = "DeletePayrollAgreementLength";
		private const string GETPAYROLLAGREEMENTLENGTHBYID = "GetPayrollAgreementLengthById";
		private const string GETALLPAYROLLAGREEMENTLENGTH = "GetAllPayrollAgreementLength";
		private const string GETPAGEDPAYROLLAGREEMENTLENGTH = "GetPagedPayrollAgreementLength";
		private const string GETPAYROLLAGREEMENTLENGTHMAXIMUMID = "GetPayrollAgreementLengthMaximumId";
		private const string GETPAYROLLAGREEMENTLENGTHROWCOUNT = "GetPayrollAgreementLengthRowCount";	
		private const string GETPAYROLLAGREEMENTLENGTHBYQUERY = "GetPayrollAgreementLengthByQuery";
		#endregion
		
		#region Constructors
		public PayrollAgreementLengthDataAccess(ClientContext context) : base(context) { }
		public PayrollAgreementLengthDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="payrollAgreementLengthObject"></param>
		private void AddCommonParams(SqlCommand cmd, PayrollAgreementLengthBase payrollAgreementLengthObject)
		{	
			AddParameter(cmd, pGuid(PayrollAgreementLengthBase.Property_PayrollAgreementLengthId, payrollAgreementLengthObject.PayrollAgreementLengthId));
			AddParameter(cmd, pGuid(PayrollAgreementLengthBase.Property_CompanyId, payrollAgreementLengthObject.CompanyId));
			AddParameter(cmd, pNVarChar(PayrollAgreementLengthBase.Property_AgreementLength, 50, payrollAgreementLengthObject.AgreementLength));
			AddParameter(cmd, pInt32(PayrollAgreementLengthBase.Property_Point, payrollAgreementLengthObject.Point));
			AddParameter(cmd, pGuid(PayrollAgreementLengthBase.Property_CreatedBy, payrollAgreementLengthObject.CreatedBy));
			AddParameter(cmd, pDateTime(PayrollAgreementLengthBase.Property_CreatedDate, payrollAgreementLengthObject.CreatedDate));
			AddParameter(cmd, pGuid(PayrollAgreementLengthBase.Property_LastUpdateBy, payrollAgreementLengthObject.LastUpdateBy));
			AddParameter(cmd, pDateTime(PayrollAgreementLengthBase.Property_LastUpdateDate, payrollAgreementLengthObject.LastUpdateDate));
			AddParameter(cmd, pGuid(PayrollAgreementLengthBase.Property_TermSheetId, payrollAgreementLengthObject.TermSheetId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PayrollAgreementLength
        /// </summary>
        /// <param name="payrollAgreementLengthObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PayrollAgreementLengthBase payrollAgreementLengthObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPAYROLLAGREEMENTLENGTH);
	
				AddParameter(cmd, pInt32Out(PayrollAgreementLengthBase.Property_Id));
				AddCommonParams(cmd, payrollAgreementLengthObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					payrollAgreementLengthObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					payrollAgreementLengthObject.Id = (Int32)GetOutParameter(cmd, PayrollAgreementLengthBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(payrollAgreementLengthObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PayrollAgreementLength
        /// </summary>
        /// <param name="payrollAgreementLengthObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PayrollAgreementLengthBase payrollAgreementLengthObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPAYROLLAGREEMENTLENGTH);
				
				AddParameter(cmd, pInt32(PayrollAgreementLengthBase.Property_Id, payrollAgreementLengthObject.Id));
				AddCommonParams(cmd, payrollAgreementLengthObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					payrollAgreementLengthObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(payrollAgreementLengthObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PayrollAgreementLength
        /// </summary>
        /// <param name="Id">Id of the PayrollAgreementLength object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPAYROLLAGREEMENTLENGTH);	
				
				AddParameter(cmd, pInt32(PayrollAgreementLengthBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PayrollAgreementLength), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PayrollAgreementLength object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PayrollAgreementLength object to retrieve</param>
        /// <returns>PayrollAgreementLength object, null if not found</returns>
		public PayrollAgreementLength Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLAGREEMENTLENGTHBYID))
			{
				AddParameter( cmd, pInt32(PayrollAgreementLengthBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PayrollAgreementLength objects 
        /// </summary>
        /// <returns>A list of PayrollAgreementLength objects</returns>
		public PayrollAgreementLengthList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPAYROLLAGREEMENTLENGTH))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PayrollAgreementLength objects by PageRequest
        /// </summary>
        /// <returns>A list of PayrollAgreementLength objects</returns>
		public PayrollAgreementLengthList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPAYROLLAGREEMENTLENGTH))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PayrollAgreementLengthList _PayrollAgreementLengthList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PayrollAgreementLengthList;
			}
		}
		
		/// <summary>
        /// Retrieves all PayrollAgreementLength objects by query String
        /// </summary>
        /// <returns>A list of PayrollAgreementLength objects</returns>
		public PayrollAgreementLengthList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLAGREEMENTLENGTHBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get PayrollAgreementLength Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of PayrollAgreementLength
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLAGREEMENTLENGTHMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PayrollAgreementLength Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PayrollAgreementLength
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PayrollAgreementLengthRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLAGREEMENTLENGTHROWCOUNT))
			{
				SqlDataReader reader;
				_PayrollAgreementLengthRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PayrollAgreementLengthRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PayrollAgreementLength object
        /// </summary>
        /// <param name="payrollAgreementLengthObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PayrollAgreementLengthBase payrollAgreementLengthObject, SqlDataReader reader, int start)
		{
			
				payrollAgreementLengthObject.Id = reader.GetInt32( start + 0 );			
				payrollAgreementLengthObject.PayrollAgreementLengthId = reader.GetGuid( start + 1 );			
				payrollAgreementLengthObject.CompanyId = reader.GetGuid( start + 2 );			
				payrollAgreementLengthObject.AgreementLength = reader.GetString( start + 3 );			
				payrollAgreementLengthObject.Point = reader.GetInt32( start + 4 );			
				payrollAgreementLengthObject.CreatedBy = reader.GetGuid( start + 5 );			
				if(!reader.IsDBNull(6)) payrollAgreementLengthObject.CreatedDate = reader.GetDateTime( start + 6 );			
				payrollAgreementLengthObject.LastUpdateBy = reader.GetGuid( start + 7 );			
				if(!reader.IsDBNull(8)) payrollAgreementLengthObject.LastUpdateDate = reader.GetDateTime( start + 8 );			
				payrollAgreementLengthObject.TermSheetId = reader.GetGuid( start + 9 );			
			FillBaseObject(payrollAgreementLengthObject, reader, (start + 10));

			
			payrollAgreementLengthObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PayrollAgreementLength object
        /// </summary>
        /// <param name="payrollAgreementLengthObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PayrollAgreementLengthBase payrollAgreementLengthObject, SqlDataReader reader)
		{
			FillObject(payrollAgreementLengthObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PayrollAgreementLength object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PayrollAgreementLength object</returns>
		private PayrollAgreementLength GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PayrollAgreementLength payrollAgreementLengthObject= new PayrollAgreementLength();
					FillObject(payrollAgreementLengthObject, reader);
					return payrollAgreementLengthObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PayrollAgreementLength objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PayrollAgreementLength objects</returns>
		private PayrollAgreementLengthList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PayrollAgreementLength list
			PayrollAgreementLengthList list = new PayrollAgreementLengthList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PayrollAgreementLength payrollAgreementLengthObject = new PayrollAgreementLength();
					FillObject(payrollAgreementLengthObject, reader);

					list.Add(payrollAgreementLengthObject);
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
