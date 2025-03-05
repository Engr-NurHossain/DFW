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
	public partial class PayrollSingleItemSettingsDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTPAYROLLSINGLEITEMSETTINGS = "InsertPayrollSingleItemSettings";
		private const string UPDATEPAYROLLSINGLEITEMSETTINGS = "UpdatePayrollSingleItemSettings";
		private const string DELETEPAYROLLSINGLEITEMSETTINGS = "DeletePayrollSingleItemSettings";
		private const string GETPAYROLLSINGLEITEMSETTINGSBYID = "GetPayrollSingleItemSettingsById";
		private const string GETALLPAYROLLSINGLEITEMSETTINGS = "GetAllPayrollSingleItemSettings";
		private const string GETPAGEDPAYROLLSINGLEITEMSETTINGS = "GetPagedPayrollSingleItemSettings";
		private const string GETPAYROLLSINGLEITEMSETTINGSMAXIMUMID = "GetPayrollSingleItemSettingsMaximumId";
		private const string GETPAYROLLSINGLEITEMSETTINGSROWCOUNT = "GetPayrollSingleItemSettingsRowCount";	
		private const string GETPAYROLLSINGLEITEMSETTINGSBYQUERY = "GetPayrollSingleItemSettingsByQuery";
		#endregion
		
		#region Constructors
		public PayrollSingleItemSettingsDataAccess(ClientContext context) : base(context) { }
		public PayrollSingleItemSettingsDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="payrollSingleItemSettingsObject"></param>
		private void AddCommonParams(SqlCommand cmd, PayrollSingleItemSettingsBase payrollSingleItemSettingsObject)
		{	
			AddParameter(cmd, pGuid(PayrollSingleItemSettingsBase.Property_SingleItemSettingsId, payrollSingleItemSettingsObject.SingleItemSettingsId));
			AddParameter(cmd, pGuid(PayrollSingleItemSettingsBase.Property_CompanyId, payrollSingleItemSettingsObject.CompanyId));
			AddParameter(cmd, pNVarChar(PayrollSingleItemSettingsBase.Property_SearchKey, 100, payrollSingleItemSettingsObject.SearchKey));
			AddParameter(cmd, pNVarChar(PayrollSingleItemSettingsBase.Property_SearchValue, 100, payrollSingleItemSettingsObject.SearchValue));
			AddParameter(cmd, pBool(PayrollSingleItemSettingsBase.Property_IsActive, payrollSingleItemSettingsObject.IsActive));
			AddParameter(cmd, pGuid(PayrollSingleItemSettingsBase.Property_CreatedBy, payrollSingleItemSettingsObject.CreatedBy));
			AddParameter(cmd, pDateTime(PayrollSingleItemSettingsBase.Property_CreatedDate, payrollSingleItemSettingsObject.CreatedDate));
			AddParameter(cmd, pGuid(PayrollSingleItemSettingsBase.Property_LastUpdateBy, payrollSingleItemSettingsObject.LastUpdateBy));
			AddParameter(cmd, pDateTime(PayrollSingleItemSettingsBase.Property_LastUpdateDate, payrollSingleItemSettingsObject.LastUpdateDate));
			AddParameter(cmd, pGuid(PayrollSingleItemSettingsBase.Property_TermSheetId, payrollSingleItemSettingsObject.TermSheetId));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts PayrollSingleItemSettings
        /// </summary>
        /// <param name="payrollSingleItemSettingsObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(PayrollSingleItemSettingsBase payrollSingleItemSettingsObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTPAYROLLSINGLEITEMSETTINGS);
	
				AddParameter(cmd, pInt32Out(PayrollSingleItemSettingsBase.Property_Id));
				AddCommonParams(cmd, payrollSingleItemSettingsObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					payrollSingleItemSettingsObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					payrollSingleItemSettingsObject.Id = (Int32)GetOutParameter(cmd, PayrollSingleItemSettingsBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(payrollSingleItemSettingsObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates PayrollSingleItemSettings
        /// </summary>
        /// <param name="payrollSingleItemSettingsObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(PayrollSingleItemSettingsBase payrollSingleItemSettingsObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATEPAYROLLSINGLEITEMSETTINGS);
				
				AddParameter(cmd, pInt32(PayrollSingleItemSettingsBase.Property_Id, payrollSingleItemSettingsObject.Id));
				AddCommonParams(cmd, payrollSingleItemSettingsObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					payrollSingleItemSettingsObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(payrollSingleItemSettingsObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes PayrollSingleItemSettings
        /// </summary>
        /// <param name="Id">Id of the PayrollSingleItemSettings object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETEPAYROLLSINGLEITEMSETTINGS);	
				
				AddParameter(cmd, pInt32(PayrollSingleItemSettingsBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(PayrollSingleItemSettings), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves PayrollSingleItemSettings object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the PayrollSingleItemSettings object to retrieve</param>
        /// <returns>PayrollSingleItemSettings object, null if not found</returns>
		public PayrollSingleItemSettings Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLSINGLEITEMSETTINGSBYID))
			{
				AddParameter( cmd, pInt32(PayrollSingleItemSettingsBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all PayrollSingleItemSettings objects 
        /// </summary>
        /// <returns>A list of PayrollSingleItemSettings objects</returns>
		public PayrollSingleItemSettingsList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLPAYROLLSINGLEITEMSETTINGS))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all PayrollSingleItemSettings objects by PageRequest
        /// </summary>
        /// <returns>A list of PayrollSingleItemSettings objects</returns>
		public PayrollSingleItemSettingsList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDPAYROLLSINGLEITEMSETTINGS))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				PayrollSingleItemSettingsList _PayrollSingleItemSettingsList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _PayrollSingleItemSettingsList;
			}
		}
		
		/// <summary>
        /// Retrieves all PayrollSingleItemSettings objects by query String
        /// </summary>
        /// <returns>A list of PayrollSingleItemSettings objects</returns>
		public PayrollSingleItemSettingsList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLSINGLEITEMSETTINGSBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get PayrollSingleItemSettings Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of PayrollSingleItemSettings
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLSINGLEITEMSETTINGSMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get PayrollSingleItemSettings Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of PayrollSingleItemSettings
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _PayrollSingleItemSettingsRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETPAYROLLSINGLEITEMSETTINGSROWCOUNT))
			{
				SqlDataReader reader;
				_PayrollSingleItemSettingsRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _PayrollSingleItemSettingsRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills PayrollSingleItemSettings object
        /// </summary>
        /// <param name="payrollSingleItemSettingsObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(PayrollSingleItemSettingsBase payrollSingleItemSettingsObject, SqlDataReader reader, int start)
		{
			
				payrollSingleItemSettingsObject.Id = reader.GetInt32( start + 0 );			
				payrollSingleItemSettingsObject.SingleItemSettingsId = reader.GetGuid( start + 1 );			
				payrollSingleItemSettingsObject.CompanyId = reader.GetGuid( start + 2 );			
				payrollSingleItemSettingsObject.SearchKey = reader.GetString( start + 3 );			
				payrollSingleItemSettingsObject.SearchValue = reader.GetString( start + 4 );			
				payrollSingleItemSettingsObject.IsActive = reader.GetBoolean( start + 5 );			
				payrollSingleItemSettingsObject.CreatedBy = reader.GetGuid( start + 6 );			
				if(!reader.IsDBNull(7)) payrollSingleItemSettingsObject.CreatedDate = reader.GetDateTime( start + 7 );			
				payrollSingleItemSettingsObject.LastUpdateBy = reader.GetGuid( start + 8 );			
				if(!reader.IsDBNull(9)) payrollSingleItemSettingsObject.LastUpdateDate = reader.GetDateTime( start + 9 );			
				payrollSingleItemSettingsObject.TermSheetId = reader.GetGuid( start + 10 );			
			FillBaseObject(payrollSingleItemSettingsObject, reader, (start + 11));

			
			payrollSingleItemSettingsObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills PayrollSingleItemSettings object
        /// </summary>
        /// <param name="payrollSingleItemSettingsObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(PayrollSingleItemSettingsBase payrollSingleItemSettingsObject, SqlDataReader reader)
		{
			FillObject(payrollSingleItemSettingsObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves PayrollSingleItemSettings object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>PayrollSingleItemSettings object</returns>
		private PayrollSingleItemSettings GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					PayrollSingleItemSettings payrollSingleItemSettingsObject= new PayrollSingleItemSettings();
					FillObject(payrollSingleItemSettingsObject, reader);
					return payrollSingleItemSettingsObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of PayrollSingleItemSettings objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of PayrollSingleItemSettings objects</returns>
		private PayrollSingleItemSettingsList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//PayrollSingleItemSettings list
			PayrollSingleItemSettingsList list = new PayrollSingleItemSettingsList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					PayrollSingleItemSettings payrollSingleItemSettingsObject = new PayrollSingleItemSettings();
					FillObject(payrollSingleItemSettingsObject, reader);

					list.Add(payrollSingleItemSettingsObject);
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
