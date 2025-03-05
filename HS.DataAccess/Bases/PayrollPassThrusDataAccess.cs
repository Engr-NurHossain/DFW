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
	public partial class PayrollPassThrusDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPAYROLLPASSTHRUS = "InsertPayrollPassThrus";
		private const string UPDATEPAYROLLPASSTHRUS = "UpdatePayrollPassThrus";
		private const string DELETEPAYROLLPASSTHRUS = "DeletePayrollPassThrus";
		private const string GETPAYROLLPASSTHRUSBYID = "GetPayrollPassThrusById";
		private const string GETALLPAYROLLPASSTHRUS = "GetAllPayrollPassThrus";
		private const string GETPAGEDPAYROLLPASSTHRUS = "GetPagedPayrollPassThrus";
		private const string GETPAYROLLPASSTHRUSMAXIMUMID = "GetPayrollPassThrusMaximumId";
		private const string GETPAYROLLPASSTHRUSROWCOUNT = "GetPayrollPassThrusRowCount";	
		private const string GETPAYROLLPASSTHRUSBYQUERY = "GetPayrollPassThrusByQuery";
		#endregion
		
		#region Constructors
		public PayrollPassThrusDataAccess(ClientContext context) : base(context) { }
		public PayrollPassThrusDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="payrollPassThrusObject"></param>
		private void AddCommonParams(SqlCommand cmd, PayrollPassThrusBase payrollPassThrusObject)
		{	
			AddParameter(cmd, pGuid(PayrollPassThrusBase.Property_PayrollPassThrusId, payrollPassThrusObject.PayrollPassThrusId));
			AddParameter(cmd, pGuid(PayrollPassThrusBase.Property_CompanyId, payrollPassThrusObject.CompanyId));
			AddParameter(cmd, pNVarChar(PayrollPassThrusBase.Property_PassThrus, 100, payrollPassThrusObject.PassThrus));
			AddParameter(cmd, pBool(PayrollPassThrusBase.Property_IsBase, payrollPassThrusObject.IsBase));
			AddParameter(cmd, pDouble(PayrollPassThrusBase.Property_Amount, payrollPassThrusObject.Amount));
			AddParameter(cmd, pGuid(PayrollPassThrusBase.Property_CreatedBy, payrollPassThrusObject.CreatedBy));
			AddParameter(cmd, pDateTime(PayrollPassThrusBase.Property_CreatedDate, payrollPassThrusObject.CreatedDate));
			AddParameter(cmd, pGuid(PayrollPassThrusBase.Property_LastUpdateBy, payrollPassThrusObject.LastUpdateBy));
			AddParameter(cmd, pDateTime(PayrollPassThrusBase.Property_LastUpdateDate, payrollPassThrusObject.LastUpdateDate));
			AddParameter(cmd, pGuid(PayrollPassThrusBase.Property_TermSheetId, payrollPassThrusObject.TermSheetId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PayrollPassThrus
        /// </summary>
        /// <param name="payrollPassThrusObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PayrollPassThrusBase payrollPassThrusObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPAYROLLPASSTHRUS);
	
				AddParameter(cmd, pInt32Out(PayrollPassThrusBase.Property_Id));
				AddCommonParams(cmd, payrollPassThrusObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					payrollPassThrusObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					payrollPassThrusObject.Id = (Int32)GetOutParameter(cmd, PayrollPassThrusBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(payrollPassThrusObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PayrollPassThrus
        /// </summary>
        /// <param name="payrollPassThrusObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PayrollPassThrusBase payrollPassThrusObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPAYROLLPASSTHRUS);
				
				AddParameter(cmd, pInt32(PayrollPassThrusBase.Property_Id, payrollPassThrusObject.Id));
				AddCommonParams(cmd, payrollPassThrusObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					payrollPassThrusObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(payrollPassThrusObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PayrollPassThrus
        /// </summary>
        /// <param name="Id">Id of the PayrollPassThrus object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPAYROLLPASSTHRUS);	
				
				AddParameter(cmd, pInt32(PayrollPassThrusBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PayrollPassThrus), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PayrollPassThrus object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PayrollPassThrus object to retrieve</param>
        /// <returns>PayrollPassThrus object, null if not found</returns>
		public PayrollPassThrus Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLPASSTHRUSBYID))
			{
				AddParameter( cmd, pInt32(PayrollPassThrusBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PayrollPassThrus objects 
        /// </summary>
        /// <returns>A list of PayrollPassThrus objects</returns>
		public PayrollPassThrusList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPAYROLLPASSTHRUS))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PayrollPassThrus objects by PageRequest
        /// </summary>
        /// <returns>A list of PayrollPassThrus objects</returns>
		public PayrollPassThrusList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPAYROLLPASSTHRUS))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PayrollPassThrusList _PayrollPassThrusList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PayrollPassThrusList;
			}
		}
		
		/// <summary>
        /// Retrieves all PayrollPassThrus objects by query String
        /// </summary>
        /// <returns>A list of PayrollPassThrus objects</returns>
		public PayrollPassThrusList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLPASSTHRUSBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get PayrollPassThrus Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of PayrollPassThrus
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLPASSTHRUSMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PayrollPassThrus Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PayrollPassThrus
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PayrollPassThrusRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLPASSTHRUSROWCOUNT))
			{
				SqlDataReader reader;
				_PayrollPassThrusRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PayrollPassThrusRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PayrollPassThrus object
        /// </summary>
        /// <param name="payrollPassThrusObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PayrollPassThrusBase payrollPassThrusObject, SqlDataReader reader, int start)
		{
			
				payrollPassThrusObject.Id = reader.GetInt32( start + 0 );			
				payrollPassThrusObject.PayrollPassThrusId = reader.GetGuid( start + 1 );			
				payrollPassThrusObject.CompanyId = reader.GetGuid( start + 2 );			
				payrollPassThrusObject.PassThrus = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) payrollPassThrusObject.IsBase = reader.GetBoolean( start + 4 );			
				payrollPassThrusObject.Amount = reader.GetDouble( start + 5 );			
				payrollPassThrusObject.CreatedBy = reader.GetGuid( start + 6 );			
				if(!reader.IsDBNull(7)) payrollPassThrusObject.CreatedDate = reader.GetDateTime( start + 7 );			
				payrollPassThrusObject.LastUpdateBy = reader.GetGuid( start + 8 );			
				if(!reader.IsDBNull(9)) payrollPassThrusObject.LastUpdateDate = reader.GetDateTime( start + 9 );			
				payrollPassThrusObject.TermSheetId = reader.GetGuid( start + 10 );			
			FillBaseObject(payrollPassThrusObject, reader, (start + 11));

			
			payrollPassThrusObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PayrollPassThrus object
        /// </summary>
        /// <param name="payrollPassThrusObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PayrollPassThrusBase payrollPassThrusObject, SqlDataReader reader)
		{
			FillObject(payrollPassThrusObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PayrollPassThrus object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PayrollPassThrus object</returns>
		private PayrollPassThrus GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PayrollPassThrus payrollPassThrusObject= new PayrollPassThrus();
					FillObject(payrollPassThrusObject, reader);
					return payrollPassThrusObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PayrollPassThrus objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PayrollPassThrus objects</returns>
		private PayrollPassThrusList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PayrollPassThrus list
			PayrollPassThrusList list = new PayrollPassThrusList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PayrollPassThrus payrollPassThrusObject = new PayrollPassThrus();
					FillObject(payrollPassThrusObject, reader);

					list.Add(payrollPassThrusObject);
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
