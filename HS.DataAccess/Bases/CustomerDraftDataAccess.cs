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
	public partial class CustomerDraftDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMERDRAFT = "InsertCustomerDraft";
		private const string UPDATECUSTOMERDRAFT = "UpdateCustomerDraft";
		private const string DELETECUSTOMERDRAFT = "DeleteCustomerDraft";
		private const string GETCUSTOMERDRAFTBYID = "GetCustomerDraftById";
		private const string GETALLCUSTOMERDRAFT = "GetAllCustomerDraft";
		private const string GETPAGEDCUSTOMERDRAFT = "GetPagedCustomerDraft";
		private const string GETCUSTOMERDRAFTMAXIMUMID = "GetCustomerDraftMaximumId";
		private const string GETCUSTOMERDRAFTROWCOUNT = "GetCustomerDraftRowCount";	
		private const string GETCUSTOMERDRAFTBYQUERY = "GetCustomerDraftByQuery";
		#endregion
		
		#region Constructors
		public CustomerDraftDataAccess(ClientContext context) : base(context) { }
		public CustomerDraftDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerDraftObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerDraftBase customerDraftObject)
		{	
			AddParameter(cmd, pGuid(CustomerDraftBase.Property_CustomerId, customerDraftObject.CustomerId));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_CustomerNo, 50, customerDraftObject.CustomerNo));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_Title, 150, customerDraftObject.Title));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_FirstName, 150, customerDraftObject.FirstName));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_LastName, 150, customerDraftObject.LastName));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_SSN, 50, customerDraftObject.SSN));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_Type, 50, customerDraftObject.Type));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_BusinessName, 250, customerDraftObject.BusinessName));
			AddParameter(cmd, pDateTime(CustomerDraftBase.Property_DateofBirth, customerDraftObject.DateofBirth));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_PrimaryPhone, 50, customerDraftObject.PrimaryPhone));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_SecondaryPhone, 50, customerDraftObject.SecondaryPhone));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_CellNo, 50, customerDraftObject.CellNo));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_Fax, 50, customerDraftObject.Fax));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_EmailAddress, 50, customerDraftObject.EmailAddress));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_CallingTime, 50, customerDraftObject.CallingTime));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_Address, customerDraftObject.Address));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_Address2, customerDraftObject.Address2));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_Street, 500, customerDraftObject.Street));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_City, 50, customerDraftObject.City));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_State, 50, customerDraftObject.State));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_ZipCode, 50, customerDraftObject.ZipCode));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_Country, 50, customerDraftObject.Country));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_StreetPrevious, 500, customerDraftObject.StreetPrevious));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_CityPrevious, 50, customerDraftObject.CityPrevious));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_StatePrevious, 50, customerDraftObject.StatePrevious));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_ZipCodePrevious, 50, customerDraftObject.ZipCodePrevious));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_CountryPrevious, 50, customerDraftObject.CountryPrevious));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_AccountNo, 50, customerDraftObject.AccountNo));
			AddParameter(cmd, pBool(CustomerDraftBase.Property_IsAlarmCom, customerDraftObject.IsAlarmCom));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_CreditScore, 50, customerDraftObject.CreditScore));
			AddParameter(cmd, pInt32(CustomerDraftBase.Property_CreditScoreValue, customerDraftObject.CreditScoreValue));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_ContractTeam, 50, customerDraftObject.ContractTeam));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_FundingCompany, 50, customerDraftObject.FundingCompany));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_MonthlyMonitoringFee, 50, customerDraftObject.MonthlyMonitoringFee));
			AddParameter(cmd, pBool(CustomerDraftBase.Property_CellularBackup, customerDraftObject.CellularBackup));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_LeadSource, 50, customerDraftObject.LeadSource));
			AddParameter(cmd, pBool(CustomerDraftBase.Property_CustomerFunded, customerDraftObject.CustomerFunded));
			AddParameter(cmd, pBool(CustomerDraftBase.Property_Maintenance, customerDraftObject.Maintenance));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_Note, customerDraftObject.Note));
			AddParameter(cmd, pDateTime(CustomerDraftBase.Property_SalesDate, customerDraftObject.SalesDate));
			AddParameter(cmd, pDateTime(CustomerDraftBase.Property_InstallDate, customerDraftObject.InstallDate));
			AddParameter(cmd, pDateTime(CustomerDraftBase.Property_CutInDate, customerDraftObject.CutInDate));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_Installer, 60, customerDraftObject.Installer));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_Soldby, 60, customerDraftObject.Soldby));
			AddParameter(cmd, pDateTime(CustomerDraftBase.Property_FundingDate, customerDraftObject.FundingDate));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_MiddleName, 50, customerDraftObject.MiddleName));
			AddParameter(cmd, pDateTime(CustomerDraftBase.Property_JoinDate, customerDraftObject.JoinDate));
			AddParameter(cmd, pDateTime(CustomerDraftBase.Property_ReminderDate, customerDraftObject.ReminderDate));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_QA1, 60, customerDraftObject.QA1));
			AddParameter(cmd, pDateTime(CustomerDraftBase.Property_QA1Date, customerDraftObject.QA1Date));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_QA2, 60, customerDraftObject.QA2));
			AddParameter(cmd, pDateTime(CustomerDraftBase.Property_QA2Date, customerDraftObject.QA2Date));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_Status, 50, customerDraftObject.Status));
			AddParameter(cmd, pDouble(CustomerDraftBase.Property_BillAmount, customerDraftObject.BillAmount));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_PaymentMethod, 50, customerDraftObject.PaymentMethod));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_BillCycle, 50, customerDraftObject.BillCycle));
			AddParameter(cmd, pInt32(CustomerDraftBase.Property_BillDay, customerDraftObject.BillDay));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_BillNotes, customerDraftObject.BillNotes));
			AddParameter(cmd, pBool(CustomerDraftBase.Property_BillTax, customerDraftObject.BillTax));
			AddParameter(cmd, pDouble(CustomerDraftBase.Property_BillOutStanding, customerDraftObject.BillOutStanding));
			AddParameter(cmd, pDateTime(CustomerDraftBase.Property_ServiceDate, customerDraftObject.ServiceDate));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_Area, 50, customerDraftObject.Area));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_StreetType, 50, customerDraftObject.StreetType));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_Appartment, 50, customerDraftObject.Appartment));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_Latlng, 500, customerDraftObject.Latlng));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_SecondCustomerNo, 50, customerDraftObject.SecondCustomerNo));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_AdditionalCustomerNo, 50, customerDraftObject.AdditionalCustomerNo));
			AddParameter(cmd, pBool(CustomerDraftBase.Property_IsTechCallPassed, customerDraftObject.IsTechCallPassed));
			AddParameter(cmd, pBool(CustomerDraftBase.Property_IsDirect, customerDraftObject.IsDirect));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_AuthorizeRefId, 250, customerDraftObject.AuthorizeRefId));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_AuthorizeCusProfileId, 250, customerDraftObject.AuthorizeCusProfileId));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_AuthorizeCusPaymentProfileId, 250, customerDraftObject.AuthorizeCusPaymentProfileId));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_AuthorizeDescription, 500, customerDraftObject.AuthorizeDescription));
			AddParameter(cmd, pBool(CustomerDraftBase.Property_IsRequiredCsvSync, customerDraftObject.IsRequiredCsvSync));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_Passcode, 50, customerDraftObject.Passcode));
			AddParameter(cmd, pDouble(CustomerDraftBase.Property_ActivationFee, customerDraftObject.ActivationFee));
			AddParameter(cmd, pDateTime(CustomerDraftBase.Property_FirstBilling, customerDraftObject.FirstBilling));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_ActivationFeePaymentMethod, 50, customerDraftObject.ActivationFeePaymentMethod));
			AddParameter(cmd, pBool(CustomerDraftBase.Property_IsActive, customerDraftObject.IsActive));
			AddParameter(cmd, pDateTime(CustomerDraftBase.Property_LastGeneratedInvoice, customerDraftObject.LastGeneratedInvoice));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_Singature, 250, customerDraftObject.Singature));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_CrossStreet, 500, customerDraftObject.CrossStreet));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_DBA, 250, customerDraftObject.DBA));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_AlarmRefId, 50, customerDraftObject.AlarmRefId));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_TransunionRefId, 50, customerDraftObject.TransunionRefId));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_MonitronicsRefId, 50, customerDraftObject.MonitronicsRefId));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_CentralStationRefId, 50, customerDraftObject.CentralStationRefId));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_CmsRefId, 50, customerDraftObject.CmsRefId));
			AddParameter(cmd, pBool(CustomerDraftBase.Property_PreferedEmail, customerDraftObject.PreferedEmail));
			AddParameter(cmd, pBool(CustomerDraftBase.Property_PreferedSms, customerDraftObject.PreferedSms));
			AddParameter(cmd, pBool(CustomerDraftBase.Property_IsAgreement, customerDraftObject.IsAgreement));
			AddParameter(cmd, pBool(CustomerDraftBase.Property_IsFireAccount, customerDraftObject.IsFireAccount));
			AddParameter(cmd, pGuid(CustomerDraftBase.Property_CreatedByUid, customerDraftObject.CreatedByUid));
			AddParameter(cmd, pDateTime(CustomerDraftBase.Property_CreatedDate, customerDraftObject.CreatedDate));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_LastUpdatedBy, 50, customerDraftObject.LastUpdatedBy));
			AddParameter(cmd, pGuid(CustomerDraftBase.Property_LastUpdatedByUid, customerDraftObject.LastUpdatedByUid));
			AddParameter(cmd, pDateTime(CustomerDraftBase.Property_LastUpdatedDate, customerDraftObject.LastUpdatedDate));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_BusinessAccountType, 50, customerDraftObject.BusinessAccountType));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_PhoneType, 50, customerDraftObject.PhoneType));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_Carrier, 50, customerDraftObject.Carrier));
			AddParameter(cmd, pGuid(CustomerDraftBase.Property_ReferringCustomer, customerDraftObject.ReferringCustomer));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_EsistingPanel, 150, customerDraftObject.EsistingPanel));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_Ownership, 50, customerDraftObject.Ownership));
			AddParameter(cmd, pDouble(CustomerDraftBase.Property_PurchasePrice, customerDraftObject.PurchasePrice));
			AddParameter(cmd, pNVarChar(CustomerDraftBase.Property_ContractValue, 50, customerDraftObject.ContractValue));
			AddParameter(cmd, pGuid(CustomerDraftBase.Property_ChildOf, customerDraftObject.ChildOf));
			AddParameter(cmd, pBool(CustomerDraftBase.Property_EmailVerified, customerDraftObject.EmailVerified));
			AddParameter(cmd, pBool(CustomerDraftBase.Property_HomeVerified, customerDraftObject.HomeVerified));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts CustomerDraft
        /// </summary>
        /// <param name="customerDraftObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerDraftBase customerDraftObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMERDRAFT);
	
				AddParameter(cmd, pInt32(CustomerDraftBase.Property_Id, customerDraftObject.Id));
				AddCommonParams(cmd, customerDraftObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerDraftObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerDraftObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates CustomerDraft
        /// </summary>
        /// <param name="customerDraftObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerDraftBase customerDraftObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMERDRAFT);
				
				AddParameter(cmd, pInt32(CustomerDraftBase.Property_Id, customerDraftObject.Id));
				AddCommonParams(cmd, customerDraftObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerDraftObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerDraftObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes CustomerDraft
        /// </summary>
        /// <param name="Id">Id of the CustomerDraft object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMERDRAFT);	
				
				AddParameter(cmd, pInt32(CustomerDraftBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(CustomerDraft), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves CustomerDraft object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerDraft object to retrieve</param>
        /// <returns>CustomerDraft object, null if not found</returns>
		public CustomerDraft Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERDRAFTBYID))
			{
				AddParameter( cmd, pInt32(CustomerDraftBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
		#endregion
		
		#region GetAll Method
		/// <summary>
        /// Retrieves all CustomerDraft objects 
        /// </summary>
        /// <returns>A list of CustomerDraft objects</returns>
		public CustomerDraftList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMERDRAFT))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all CustomerDraft objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerDraft objects</returns>
		public CustomerDraftList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMERDRAFT))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerDraftList _CustomerDraftList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerDraftList;
			}
		}
		
		/// <summary>
        /// Retrieves all CustomerDraft objects by query String
        /// </summary>
        /// <returns>A list of CustomerDraft objects</returns>
		public CustomerDraftList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERDRAFTBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get CustomerDraft Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of CustomerDraft
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERDRAFTMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get CustomerDraft Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of CustomerDraft
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerDraftRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERDRAFTROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerDraftRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerDraftRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills CustomerDraft object
        /// </summary>
        /// <param name="customerDraftObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerDraftBase customerDraftObject, SqlDataReader reader, int start)
		{
			
				customerDraftObject.Id = reader.GetInt32( start + 0 );			
				customerDraftObject.CustomerId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) customerDraftObject.CustomerNo = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) customerDraftObject.Title = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) customerDraftObject.FirstName = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) customerDraftObject.LastName = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) customerDraftObject.SSN = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) customerDraftObject.Type = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) customerDraftObject.BusinessName = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) customerDraftObject.DateofBirth = reader.GetDateTime( start + 9 );			
				if(!reader.IsDBNull(10)) customerDraftObject.PrimaryPhone = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) customerDraftObject.SecondaryPhone = reader.GetString( start + 11 );			
				if(!reader.IsDBNull(12)) customerDraftObject.CellNo = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) customerDraftObject.Fax = reader.GetString( start + 13 );			
				if(!reader.IsDBNull(14)) customerDraftObject.EmailAddress = reader.GetString( start + 14 );			
				if(!reader.IsDBNull(15)) customerDraftObject.CallingTime = reader.GetString( start + 15 );			
				if(!reader.IsDBNull(16)) customerDraftObject.Address = reader.GetString( start + 16 );			
				if(!reader.IsDBNull(17)) customerDraftObject.Address2 = reader.GetString( start + 17 );			
				if(!reader.IsDBNull(18)) customerDraftObject.Street = reader.GetString( start + 18 );			
				if(!reader.IsDBNull(19)) customerDraftObject.City = reader.GetString( start + 19 );			
				if(!reader.IsDBNull(20)) customerDraftObject.State = reader.GetString( start + 20 );			
				if(!reader.IsDBNull(21)) customerDraftObject.ZipCode = reader.GetString( start + 21 );			
				if(!reader.IsDBNull(22)) customerDraftObject.Country = reader.GetString( start + 22 );			
				if(!reader.IsDBNull(23)) customerDraftObject.StreetPrevious = reader.GetString( start + 23 );			
				if(!reader.IsDBNull(24)) customerDraftObject.CityPrevious = reader.GetString( start + 24 );			
				if(!reader.IsDBNull(25)) customerDraftObject.StatePrevious = reader.GetString( start + 25 );			
				if(!reader.IsDBNull(26)) customerDraftObject.ZipCodePrevious = reader.GetString( start + 26 );			
				if(!reader.IsDBNull(27)) customerDraftObject.CountryPrevious = reader.GetString( start + 27 );			
				if(!reader.IsDBNull(28)) customerDraftObject.AccountNo = reader.GetString( start + 28 );			
				if(!reader.IsDBNull(29)) customerDraftObject.IsAlarmCom = reader.GetBoolean( start + 29 );			
				if(!reader.IsDBNull(30)) customerDraftObject.CreditScore = reader.GetString( start + 30 );			
				if(!reader.IsDBNull(31)) customerDraftObject.CreditScoreValue = reader.GetInt32( start + 31 );			
				if(!reader.IsDBNull(32)) customerDraftObject.ContractTeam = reader.GetString( start + 32 );			
				if(!reader.IsDBNull(33)) customerDraftObject.FundingCompany = reader.GetString( start + 33 );			
				if(!reader.IsDBNull(34)) customerDraftObject.MonthlyMonitoringFee = reader.GetString( start + 34 );			
				if(!reader.IsDBNull(35)) customerDraftObject.CellularBackup = reader.GetBoolean( start + 35 );			
				if(!reader.IsDBNull(36)) customerDraftObject.LeadSource = reader.GetString( start + 36 );			
				if(!reader.IsDBNull(37)) customerDraftObject.CustomerFunded = reader.GetBoolean( start + 37 );			
				if(!reader.IsDBNull(38)) customerDraftObject.Maintenance = reader.GetBoolean( start + 38 );			
				if(!reader.IsDBNull(39)) customerDraftObject.Note = reader.GetString( start + 39 );			
				if(!reader.IsDBNull(40)) customerDraftObject.SalesDate = reader.GetDateTime( start + 40 );			
				if(!reader.IsDBNull(41)) customerDraftObject.InstallDate = reader.GetDateTime( start + 41 );			
				if(!reader.IsDBNull(42)) customerDraftObject.CutInDate = reader.GetDateTime( start + 42 );			
				if(!reader.IsDBNull(43)) customerDraftObject.Installer = reader.GetString( start + 43 );			
				if(!reader.IsDBNull(44)) customerDraftObject.Soldby = reader.GetString( start + 44 );			
				if(!reader.IsDBNull(45)) customerDraftObject.FundingDate = reader.GetDateTime( start + 45 );			
				if(!reader.IsDBNull(46)) customerDraftObject.MiddleName = reader.GetString( start + 46 );			
				if(!reader.IsDBNull(47)) customerDraftObject.JoinDate = reader.GetDateTime( start + 47 );			
				if(!reader.IsDBNull(48)) customerDraftObject.ReminderDate = reader.GetDateTime( start + 48 );			
				if(!reader.IsDBNull(49)) customerDraftObject.QA1 = reader.GetString( start + 49 );			
				if(!reader.IsDBNull(50)) customerDraftObject.QA1Date = reader.GetDateTime( start + 50 );			
				if(!reader.IsDBNull(51)) customerDraftObject.QA2 = reader.GetString( start + 51 );			
				if(!reader.IsDBNull(52)) customerDraftObject.QA2Date = reader.GetDateTime( start + 52 );			
				if(!reader.IsDBNull(53)) customerDraftObject.Status = reader.GetString( start + 53 );			
				if(!reader.IsDBNull(54)) customerDraftObject.BillAmount = reader.GetDouble( start + 54 );			
				if(!reader.IsDBNull(55)) customerDraftObject.PaymentMethod = reader.GetString( start + 55 );			
				if(!reader.IsDBNull(56)) customerDraftObject.BillCycle = reader.GetString( start + 56 );			
				if(!reader.IsDBNull(57)) customerDraftObject.BillDay = reader.GetInt32( start + 57 );			
				if(!reader.IsDBNull(58)) customerDraftObject.BillNotes = reader.GetString( start + 58 );			
				if(!reader.IsDBNull(59)) customerDraftObject.BillTax = reader.GetBoolean( start + 59 );			
				if(!reader.IsDBNull(60)) customerDraftObject.BillOutStanding = reader.GetDouble( start + 60 );			
				if(!reader.IsDBNull(61)) customerDraftObject.ServiceDate = reader.GetDateTime( start + 61 );			
				if(!reader.IsDBNull(62)) customerDraftObject.Area = reader.GetString( start + 62 );			
				if(!reader.IsDBNull(63)) customerDraftObject.StreetType = reader.GetString( start + 63 );			
				if(!reader.IsDBNull(64)) customerDraftObject.Appartment = reader.GetString( start + 64 );			
				if(!reader.IsDBNull(65)) customerDraftObject.Latlng = reader.GetString( start + 65 );			
				if(!reader.IsDBNull(66)) customerDraftObject.SecondCustomerNo = reader.GetString( start + 66 );			
				if(!reader.IsDBNull(67)) customerDraftObject.AdditionalCustomerNo = reader.GetString( start + 67 );			
				if(!reader.IsDBNull(68)) customerDraftObject.IsTechCallPassed = reader.GetBoolean( start + 68 );			
				if(!reader.IsDBNull(69)) customerDraftObject.IsDirect = reader.GetBoolean( start + 69 );			
				if(!reader.IsDBNull(70)) customerDraftObject.AuthorizeRefId = reader.GetString( start + 70 );			
				if(!reader.IsDBNull(71)) customerDraftObject.AuthorizeCusProfileId = reader.GetString( start + 71 );			
				if(!reader.IsDBNull(72)) customerDraftObject.AuthorizeCusPaymentProfileId = reader.GetString( start + 72 );			
				if(!reader.IsDBNull(73)) customerDraftObject.AuthorizeDescription = reader.GetString( start + 73 );			
				if(!reader.IsDBNull(74)) customerDraftObject.IsRequiredCsvSync = reader.GetBoolean( start + 74 );			
				if(!reader.IsDBNull(75)) customerDraftObject.Passcode = reader.GetString( start + 75 );			
				if(!reader.IsDBNull(76)) customerDraftObject.ActivationFee = reader.GetDouble( start + 76 );			
				if(!reader.IsDBNull(77)) customerDraftObject.FirstBilling = reader.GetDateTime( start + 77 );			
				if(!reader.IsDBNull(78)) customerDraftObject.ActivationFeePaymentMethod = reader.GetString( start + 78 );			
				if(!reader.IsDBNull(79)) customerDraftObject.IsActive = reader.GetBoolean( start + 79 );			
				if(!reader.IsDBNull(80)) customerDraftObject.LastGeneratedInvoice = reader.GetDateTime( start + 80 );			
				if(!reader.IsDBNull(81)) customerDraftObject.Singature = reader.GetString( start + 81 );			
				if(!reader.IsDBNull(82)) customerDraftObject.CrossStreet = reader.GetString( start + 82 );			
				if(!reader.IsDBNull(83)) customerDraftObject.DBA = reader.GetString( start + 83 );			
				if(!reader.IsDBNull(84)) customerDraftObject.AlarmRefId = reader.GetString( start + 84 );			
				if(!reader.IsDBNull(85)) customerDraftObject.TransunionRefId = reader.GetString( start + 85 );			
				if(!reader.IsDBNull(86)) customerDraftObject.MonitronicsRefId = reader.GetString( start + 86 );			
				if(!reader.IsDBNull(87)) customerDraftObject.CentralStationRefId = reader.GetString( start + 87 );			
				if(!reader.IsDBNull(88)) customerDraftObject.CmsRefId = reader.GetString( start + 88 );			
				if(!reader.IsDBNull(89)) customerDraftObject.PreferedEmail = reader.GetBoolean( start + 89 );			
				if(!reader.IsDBNull(90)) customerDraftObject.PreferedSms = reader.GetBoolean( start + 90 );			
				if(!reader.IsDBNull(91)) customerDraftObject.IsAgreement = reader.GetBoolean( start + 91 );			
				if(!reader.IsDBNull(92)) customerDraftObject.IsFireAccount = reader.GetBoolean( start + 92 );			
				customerDraftObject.CreatedByUid = reader.GetGuid( start + 93 );			
				if(!reader.IsDBNull(94)) customerDraftObject.CreatedDate = reader.GetDateTime( start + 94 );			
				customerDraftObject.LastUpdatedBy = reader.GetString( start + 95 );			
				customerDraftObject.LastUpdatedByUid = reader.GetGuid( start + 96 );			
				customerDraftObject.LastUpdatedDate = reader.GetDateTime( start + 97 );			
				if(!reader.IsDBNull(98)) customerDraftObject.BusinessAccountType = reader.GetString( start + 98 );			
				if(!reader.IsDBNull(99)) customerDraftObject.PhoneType = reader.GetString( start + 99 );			
				if(!reader.IsDBNull(100)) customerDraftObject.Carrier = reader.GetString( start + 100 );			
				customerDraftObject.ReferringCustomer = reader.GetGuid( start + 101 );			
				if(!reader.IsDBNull(102)) customerDraftObject.EsistingPanel = reader.GetString( start + 102 );			
				if(!reader.IsDBNull(103)) customerDraftObject.Ownership = reader.GetString( start + 103 );			
				if(!reader.IsDBNull(104)) customerDraftObject.PurchasePrice = reader.GetDouble( start + 104 );			
				if(!reader.IsDBNull(105)) customerDraftObject.ContractValue = reader.GetString( start + 105 );			
				customerDraftObject.ChildOf = reader.GetGuid( start + 106 );			
				if(!reader.IsDBNull(107)) customerDraftObject.EmailVerified = reader.GetBoolean( start + 107 );			
				if(!reader.IsDBNull(108)) customerDraftObject.HomeVerified = reader.GetBoolean( start + 108 );			
			FillBaseObject(customerDraftObject, reader, (start + 109));

			
			customerDraftObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills CustomerDraft object
        /// </summary>
        /// <param name="customerDraftObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerDraftBase customerDraftObject, SqlDataReader reader)
		{
			FillObject(customerDraftObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves CustomerDraft object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerDraft object</returns>
		private CustomerDraft GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					CustomerDraft customerDraftObject= new CustomerDraft();
					FillObject(customerDraftObject, reader);
					return customerDraftObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of CustomerDraft objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerDraft objects</returns>
		private CustomerDraftList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//CustomerDraft list
			CustomerDraftList list = new CustomerDraftList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					CustomerDraft customerDraftObject = new CustomerDraft();
					FillObject(customerDraftObject, reader);

					list.Add(customerDraftObject);
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
