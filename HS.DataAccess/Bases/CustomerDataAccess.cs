using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.DataAccess;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;
using System.Linq;
namespace HS.DataAccess
{
	public partial class CustomerDataAccess : BaseDataAccess
	{
		#region Constants
		private const string INSERTCUSTOMER = "InsertCustomer";
		private const string UPDATECUSTOMER = "UpdateCustomer";
		private const string DELETECUSTOMER = "DeleteCustomer";
		private const string GETCUSTOMERBYID = "GetCustomerById";
		private const string GETALLCUSTOMER = "GetAllCustomer";
		private const string GETPAGEDCUSTOMER = "GetPagedCustomer";
		private const string GETCUSTOMERMAXIMUMID = "GetCustomerMaximumId";
		private const string GETCUSTOMERROWCOUNT = "GetCustomerRowCount";	
		private const string GETCUSTOMERBYQUERY = "GetCustomerByQuery";
		#endregion
		
		#region Constructors
		public CustomerDataAccess(ClientContext context) : base(context) { }
		public CustomerDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion
				
		#region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerObject"></param>
		private void AddCommonParams(SqlCommand cmd, CustomerBase customerObject)
		{	
			AddParameter(cmd, pGuid(CustomerBase.Property_CustomerId, customerObject.CustomerId));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_CustomerNo, 50, customerObject.CustomerNo));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_Title, 150, customerObject.Title));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_FirstName, 150, customerObject.FirstName));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_LastName, 150, customerObject.LastName));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_SSN, 50, customerObject.SSN));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_Type, 50, customerObject.Type));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_BusinessName, 250, customerObject.BusinessName));
			AddParameter(cmd, pDateTime(CustomerBase.Property_DateofBirth, customerObject.DateofBirth));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_PrimaryPhone, 50, customerObject.PrimaryPhone));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_SecondaryPhone, 50, customerObject.SecondaryPhone));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_CellNo, 50, customerObject.CellNo));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_Fax, 50, customerObject.Fax));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_EmailAddress, 50, customerObject.EmailAddress));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_CallingTime, 50, customerObject.CallingTime));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_Address, customerObject.Address));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_Address2, customerObject.Address2));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_Street, 500, customerObject.Street));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_City, 50, customerObject.City));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_State, 50, customerObject.State));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_ZipCode, 50, customerObject.ZipCode));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_Country, 50, customerObject.Country));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_StreetPrevious, 500, customerObject.StreetPrevious));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_CityPrevious, 50, customerObject.CityPrevious));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_StatePrevious, 50, customerObject.StatePrevious));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_ZipCodePrevious, 50, customerObject.ZipCodePrevious));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_CountryPrevious, 50, customerObject.CountryPrevious));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_AccountNo, 50, customerObject.AccountNo));
			AddParameter(cmd, pBool(CustomerBase.Property_IsAlarmCom, customerObject.IsAlarmCom));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_CreditScore, 50, customerObject.CreditScore));
			AddParameter(cmd, pInt32(CustomerBase.Property_CreditScoreValue, customerObject.CreditScoreValue));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_ContractTeam, 50, customerObject.ContractTeam));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_FundingCompany, 50, customerObject.FundingCompany));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_MonthlyMonitoringFee, 50, customerObject.MonthlyMonitoringFee));
			AddParameter(cmd, pBool(CustomerBase.Property_CellularBackup, customerObject.CellularBackup));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_LeadSource, 50, customerObject.LeadSource));
			AddParameter(cmd, pBool(CustomerBase.Property_CustomerFunded, customerObject.CustomerFunded));
			AddParameter(cmd, pBool(CustomerBase.Property_Maintenance, customerObject.Maintenance));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_Note, customerObject.Note));
			AddParameter(cmd, pDateTime(CustomerBase.Property_SalesDate, customerObject.SalesDate));
			AddParameter(cmd, pDateTime(CustomerBase.Property_InstallDate, customerObject.InstallDate));
			AddParameter(cmd, pDateTime(CustomerBase.Property_CutInDate, customerObject.CutInDate));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_Installer, 60, customerObject.Installer));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_Soldby, 60, customerObject.Soldby));
			AddParameter(cmd, pDateTime(CustomerBase.Property_FundingDate, customerObject.FundingDate));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_MiddleName, 50, customerObject.MiddleName));
			AddParameter(cmd, pDateTime(CustomerBase.Property_JoinDate, customerObject.JoinDate));
			AddParameter(cmd, pDateTime(CustomerBase.Property_ReminderDate, customerObject.ReminderDate));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_QA1, 60, customerObject.QA1));
			AddParameter(cmd, pDateTime(CustomerBase.Property_QA1Date, customerObject.QA1Date));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_QA2, 60, customerObject.QA2));
			AddParameter(cmd, pDateTime(CustomerBase.Property_QA2Date, customerObject.QA2Date));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_Status, 50, customerObject.Status));
			AddParameter(cmd, pDouble(CustomerBase.Property_BillAmount, customerObject.BillAmount));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_PaymentMethod, 50, customerObject.PaymentMethod));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_BillCycle, 50, customerObject.BillCycle));
			AddParameter(cmd, pInt32(CustomerBase.Property_BillDay, customerObject.BillDay));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_BillNotes, customerObject.BillNotes));
			AddParameter(cmd, pBool(CustomerBase.Property_BillTax, customerObject.BillTax));
			AddParameter(cmd, pDouble(CustomerBase.Property_BillOutStanding, customerObject.BillOutStanding));
			AddParameter(cmd, pDateTime(CustomerBase.Property_ServiceDate, customerObject.ServiceDate));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_Area, 50, customerObject.Area));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_StreetType, 50, customerObject.StreetType));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_Appartment, 50, customerObject.Appartment));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_Latlng, 500, customerObject.Latlng));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_SecondCustomerNo, 50, customerObject.SecondCustomerNo));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_AdditionalCustomerNo, 50, customerObject.AdditionalCustomerNo));
			AddParameter(cmd, pBool(CustomerBase.Property_IsTechCallPassed, customerObject.IsTechCallPassed));
			AddParameter(cmd, pBool(CustomerBase.Property_IsDirect, customerObject.IsDirect));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_AuthorizeRefId, 250, customerObject.AuthorizeRefId));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_AuthorizeCusProfileId, 250, customerObject.AuthorizeCusProfileId));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_AuthorizeCusPaymentProfileId, 250, customerObject.AuthorizeCusPaymentProfileId));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_AuthorizeDescription, 500, customerObject.AuthorizeDescription));
			AddParameter(cmd, pBool(CustomerBase.Property_IsRequiredCsvSync, customerObject.IsRequiredCsvSync));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_Passcode, 50, customerObject.Passcode));
			AddParameter(cmd, pDouble(CustomerBase.Property_ActivationFee, customerObject.ActivationFee));
			AddParameter(cmd, pDateTime(CustomerBase.Property_FirstBilling, customerObject.FirstBilling));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_ActivationFeePaymentMethod, 50, customerObject.ActivationFeePaymentMethod));
			AddParameter(cmd, pBool(CustomerBase.Property_IsActive, customerObject.IsActive));
			AddParameter(cmd, pDateTime(CustomerBase.Property_LastGeneratedInvoice, customerObject.LastGeneratedInvoice));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_Singature, 250, customerObject.Singature));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_CrossStreet, 500, customerObject.CrossStreet));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_DBA, 250, customerObject.DBA));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_AlarmRefId, 50, customerObject.AlarmRefId));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_TransunionRefId, 50, customerObject.TransunionRefId));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_MonitronicsRefId, 50, customerObject.MonitronicsRefId));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_CentralStationRefId, 50, customerObject.CentralStationRefId));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_CmsRefId, 50, customerObject.CmsRefId));
			AddParameter(cmd, pBool(CustomerBase.Property_PreferedEmail, customerObject.PreferedEmail));
			AddParameter(cmd, pBool(CustomerBase.Property_PreferedSms, customerObject.PreferedSms));
			AddParameter(cmd, pBool(CustomerBase.Property_IsAgreement, customerObject.IsAgreement));
			AddParameter(cmd, pBool(CustomerBase.Property_IsFireAccount, customerObject.IsFireAccount));
			AddParameter(cmd, pGuid(CustomerBase.Property_CreatedByUid, customerObject.CreatedByUid));
			AddParameter(cmd, pDateTime(CustomerBase.Property_CreatedDate, customerObject.CreatedDate));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_LastUpdatedBy, 50, customerObject.LastUpdatedBy));
			AddParameter(cmd, pGuid(CustomerBase.Property_LastUpdatedByUid, customerObject.LastUpdatedByUid));
			AddParameter(cmd, pDateTime(CustomerBase.Property_LastUpdatedDate, customerObject.LastUpdatedDate));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_BusinessAccountType, 50, customerObject.BusinessAccountType));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_PhoneType, 50, customerObject.PhoneType));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_Carrier, 50, customerObject.Carrier));
			AddParameter(cmd, pGuid(CustomerBase.Property_ReferringCustomer, customerObject.ReferringCustomer));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_EsistingPanel, 150, customerObject.EsistingPanel));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_Ownership, 50, customerObject.Ownership));
			AddParameter(cmd, pDouble(CustomerBase.Property_PurchasePrice, customerObject.PurchasePrice));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_ContractValue, 50, customerObject.ContractValue));
			AddParameter(cmd, pGuid(CustomerBase.Property_ChildOf, customerObject.ChildOf));
			AddParameter(cmd, pBool(CustomerBase.Property_EmailVerified, customerObject.EmailVerified));
			AddParameter(cmd, pBool(CustomerBase.Property_HomeVerified, customerObject.HomeVerified));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_County, 50, customerObject.County));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_CustomerToken, 150, customerObject.CustomerToken));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_PaymentToken, 150, customerObject.PaymentToken));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_ScheduleToken, 150, customerObject.ScheduleToken));
			AddParameter(cmd, pDateTime(CustomerBase.Property_EstCloseDate, customerObject.EstCloseDate));
			AddParameter(cmd, pDateTime(CustomerBase.Property_ProjectWalkDate, customerObject.ProjectWalkDate));
			AddParameter(cmd, pInt32(CustomerBase.Property_BranchId, customerObject.BranchId));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_SubscriptionStatus, 50, customerObject.SubscriptionStatus));
			AddParameter(cmd, pDouble(CustomerBase.Property_AnnualRevenue, customerObject.AnnualRevenue));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_Website, 100, customerObject.Website));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_Market, 100, customerObject.Market));
			AddParameter(cmd, pInt32(CustomerBase.Property_Passengers, customerObject.Passengers));
			AddParameter(cmd, pDouble(CustomerBase.Property_Budget, customerObject.Budget));
			AddParameter(cmd, pInt32(CustomerBase.Property_SmartSetUpStep, customerObject.SmartSetUpStep));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_CustomerAccountType, 50, customerObject.CustomerAccountType));
			AddParameter(cmd, pBool(CustomerBase.Property_IsPrimaryPhoneVerified, customerObject.IsPrimaryPhoneVerified));
			AddParameter(cmd, pBool(CustomerBase.Property_IsSecondaryPhoneVerified, customerObject.IsSecondaryPhoneVerified));
			AddParameter(cmd, pBool(CustomerBase.Property_IsCellNoVerified, customerObject.IsCellNoVerified));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_HomeOwner, 50, customerObject.HomeOwner));
			AddParameter(cmd, pGuid(CustomerBase.Property_AccessGivenTo, customerObject.AccessGivenTo));
			AddParameter(cmd, pDateTime(CustomerBase.Property_DoNotCall, customerObject.DoNotCall));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_PreferredContactMethod, 50, customerObject.PreferredContactMethod));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_SalesLocation, 50, customerObject.SalesLocation));
			AddParameter(cmd, pBool(CustomerBase.Property_IsReceivePaymentMail, customerObject.IsReceivePaymentMail));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_BestTimeToCall, 50, customerObject.BestTimeToCall));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_CSProvider, 50, customerObject.CSProvider));
			AddParameter(cmd, pInt32(CustomerBase.Property_RenewalTerm, customerObject.RenewalTerm));
			AddParameter(cmd, pBool(CustomerBase.Property_PreferedCall, customerObject.PreferedCall));
			AddParameter(cmd, pBool(CustomerBase.Property_IsAgreementSend, customerObject.IsAgreementSend));
			AddParameter(cmd, pBool(CustomerBase.Property_IsACHDiscount, customerObject.IsACHDiscount));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_CustomerStatus, 50, customerObject.CustomerStatus));
			AddParameter(cmd, pInt32(CustomerBase.Property_TransferCustomerId, customerObject.TransferCustomerId));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_CancellationSignature, customerObject.CancellationSignature));
			AddParameter(cmd, pDateTime(CustomerBase.Property_OriginalContactDate, customerObject.OriginalContactDate));
			AddParameter(cmd, pGuid(CustomerBase.Property_DuplicateCustomer, customerObject.DuplicateCustomer));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_InspectionCompany, 60, customerObject.InspectionCompany));
			AddParameter(cmd, pGuid(CustomerBase.Property_SoldBy2, customerObject.SoldBy2));
			AddParameter(cmd, pGuid(CustomerBase.Property_SoldBy3, customerObject.SoldBy3));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_ContactedPerviously, 50, customerObject.ContactedPerviously));
			AddParameter(cmd, pDateTime(CustomerBase.Property_MovingDate, customerObject.MovingDate));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_InstalledStatus, 50, customerObject.InstalledStatus));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_AcquiredFrom, 150, customerObject.AcquiredFrom));
			AddParameter(cmd, pDateTime(CustomerBase.Property_FollowUpDate, customerObject.FollowUpDate));
			AddParameter(cmd, pDouble(CustomerBase.Property_BuyoutAmountByADS, customerObject.BuyoutAmountByADS));
			AddParameter(cmd, pDouble(CustomerBase.Property_BuyoutAmountBySalesRep, customerObject.BuyoutAmountBySalesRep));
			AddParameter(cmd, pDouble(CustomerBase.Property_FinancedTerm, customerObject.FinancedTerm));
			AddParameter(cmd, pDouble(CustomerBase.Property_FinancedAmount, customerObject.FinancedAmount));
			AddParameter(cmd, pDouble(CustomerBase.Property_Levels, customerObject.Levels));
			AddParameter(cmd, pDouble(CustomerBase.Property_SoldAmount, customerObject.SoldAmount));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_AgreementEmail, 100, customerObject.AgreementEmail));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_AgreementPhoneNo, 100, customerObject.AgreementPhoneNo));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_TaxExemption, 50, customerObject.TaxExemption));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_AppoinmentSet, 50, customerObject.AppoinmentSet));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_BillingEmail, 50, customerObject.BillingEmail));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_BillingPhone, 50, customerObject.BillingPhone));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_BillingContact, 50, customerObject.BillingContact));
			AddParameter(cmd, pDateTime(CustomerBase.Property_LastOpenedDate, customerObject.LastOpenedDate));
			AddParameter(cmd, pDateTime(CustomerBase.Property_DisconnectServiceDate, customerObject.DisconnectServiceDate));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_UCCRefId, 50, customerObject.UCCRefId));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_PlatformId, customerObject.PlatformId));
			AddParameter(cmd, pInt32(CustomerBase.Property_RecommendedLevel, customerObject.RecommendedLevel));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_ProfileImage, customerObject.ProfileImage));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_BrinksRefId, 50, customerObject.BrinksRefId));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_MapscoNo, 50, customerObject.MapscoNo));
			AddParameter(cmd, pDateTime(CustomerBase.Property_CustomerSignatureDate, customerObject.CustomerSignatureDate));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_LeadSourceType, 500, customerObject.LeadSourceType));
			AddParameter(cmd, pGuid(CustomerBase.Property_MoveCustomerId, customerObject.MoveCustomerId));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_EcontractId, 200, customerObject.EcontractId));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_EcontractEnvlobeId, 200, customerObject.EcontractEnvlobeId));
			AddParameter(cmd, pGuid(CustomerBase.Property_Soldby1, customerObject.Soldby1));
			AddParameter(cmd, pNVarChar(CustomerBase.Property_SearchText, customerObject.SearchText));
			AddParameter(cmd, pBool(CustomerBase.Property_IsContractSigned, customerObject.IsContractSigned));
			AddParameter(cmd, pDateTime(CustomerBase.Property_CustomerFundedDate, customerObject.CustomerFundedDate));
		}
		#endregion
		
		#region Insert Method
		/// <summary>
        /// Inserts Customer
        /// </summary>
        /// <param name="customerObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
		public long Insert(CustomerBase customerObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(INSERTCUSTOMER);
	
				AddParameter(cmd, pInt32Out(CustomerBase.Property_Id));
				AddCommonParams(cmd, customerObject);
			
				long result = InsertRecord(cmd);
				if (result > 0)
				{
					customerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
					customerObject.Id = (Int32)GetOutParameter(cmd, CustomerBase.Property_Id);
				}
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectInsertException(customerObject, x);
			}
		}
		#endregion
		
		#region Update Method
		/// <summary>
        /// Updates Customer
        /// </summary>
        /// <param name="customerObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
		public long Update(CustomerBase customerObject)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(UPDATECUSTOMER);
				
				AddParameter(cmd, pInt32(CustomerBase.Property_Id, customerObject.Id));
				AddCommonParams(cmd, customerObject);
	
				long result = UpdateRecord(cmd);
				if (result > 0)
					customerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
				return result;
			}
			catch(SqlException x)
			{
				throw new ObjectUpdateException(customerObject, x);
			}
		}
		#endregion
		
		#region Delete Method
		/// <summary>
        /// Deletes Customer
        /// </summary>
        /// <param name="Id">Id of the Customer object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
		public long Delete(Int32 _Id)
		{
			try
			{
				SqlCommand cmd = GetSPCommand(DELETECUSTOMER);	
				
				AddParameter(cmd, pInt32(CustomerBase.Property_Id, _Id));
				 
				return DeleteRecord(cmd);
			}
			catch(SqlException x)
			{
				throw new ObjectDeleteException(typeof(Customer), _Id, x);
			}
			
		}
		#endregion
		
		#region Get By Id Method
		/// <summary>
        /// Retrieves Customer object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the Customer object to retrieve</param>
        /// <returns>Customer object, null if not found</returns>
		public Customer Get(Int32 _Id)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERBYID))
			{
				AddParameter( cmd, pInt32(CustomerBase.Property_Id, _Id));

				return GetObject(cmd);
			}
		}
        public DataSet GetAllJobsDetailByFilter(Guid comid, string from, string to, Guid userid, string status, string type, bool permission)
        {
            string subquery = "";
            string UserQuery = "";
            string StatusQuery = "";
            string TypeQuery = "";
            string PermissionQuery = "";
            if (!permission)
            {
                PermissionQuery = "and tk.IsClosed = 0";
            }
            if (!string.IsNullOrWhiteSpace(from) && !string.IsNullOrWhiteSpace(to))
            {
                from = from + " 00:00:00";
                to = to + " 23:59:59";
                subquery = string.Format("and tk.CompletionDate between '{0}' and '{1}'", from, to);
            }
            else
            {
                var date = DateTimeExtension.StartOfWeek(DateTime.Now);
                from = date.ToString("yyyy-MM-dd") + " 00:00:00";
                to = date.AddDays(6).ToString("yyyy-MM-dd") + " 23:59:59";
                subquery = string.Format("and tk.CompletionDate between '{0}' and '{1}'", from, to);
            }
            if (userid != new Guid())
            {
                UserQuery = string.Format("and (assign.UserId = '{0}' or tk.CreatedBy = '{0}')", userid);
            }
            if (!string.IsNullOrWhiteSpace(status) && status != "-1")
            {
                if (status.ToLower() == "completed")
                {
                    StatusQuery = string.Format("and tk.[Status] = '{0}'", status);
                }
                else
                {
                    StatusQuery = string.Format("and tk.[Status] != '{0}'", status);
                }
            }
            if (!string.IsNullOrWhiteSpace(type) && type != "-1")
            {
                TypeQuery = string.Format("and tk.TicketType = '{0}'", type);
            }
            string sqlQuery = @"select * into #temp from [Lookup] where DataKey = 'TicketStatus'

                                select tk.*, case when cus.BusinessName != '' and cus.BusinessName is not null then cus.BusinessName else cus.FirstName + ' ' + cus.LastName end as CustomerName, cus.Id as CustomerIntId, assign.FirstName + ' ' + assign.LastName as AssignedPerson
                                , createdby.FirstName + ' ' + createdby.LastName as CreatedPerson, #t.DisplayText as AssignedStatus
                                ,cus.Street, cus.City, cus.State, cus.ZipCode
                                ,ca.AppointmentStartTime, ca.AppointmentEndTime
                                from Ticket tk
                                left join Customer cus on cus.CustomerId = tk.CustomerId
                                left join TicketUser tu on tu.TiketId = tk.TicketId -- and tu.IsPrimary = 1
                                left join Employee assign on assign.UserId = tu.UserId 
                                left join Employee createdby on createdby.UserId = tk.CreatedBy
                                left join CustomerAppointment ca on ca.AppointmentId = tk.TicketId
                                left join #temp #t on #t.DataValue = tk.[Status]
                                where tk.CompanyId = '{0}' and tk.[Status] NOT LIKE '%cancel%' and tk.[Status] NOT LIKE '%lost%' and tk.[Status] NOT LIKE '%no%' 
                                {5} 
                                {1}
                                {2}
                                {3}
                                {4}
                                drop table #temp";
            try
            {
                sqlQuery = string.Format(sqlQuery, comid, subquery, UserQuery, StatusQuery, TypeQuery, PermissionQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region GetAll Method
        /// <summary>
        /// Retrieves all Customer objects 
        /// </summary>
        /// <returns>A list of Customer objects</returns>
        public CustomerList GetAll()
		{
			using( SqlCommand cmd = GetSPCommand(GETALLCUSTOMER))
			{
				return GetList(cmd, ALL_AVAILABLE_RECORDS);
			}
		}
		
		
		/// <summary>
        /// Retrieves all Customer objects by PageRequest
        /// </summary>
        /// <returns>A list of Customer objects</returns>
		public CustomerList GetPaged(PagedRequest request)
		{
			using( SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMER))
			{
				AddParameter( cmd, pInt32Out("TotalRows") );
			 	AddParameter( cmd, pInt32("PageIndex", request.PageIndex) );
				AddParameter( cmd, pInt32("RowPerPage", request.RowPerPage) );
				AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause) );
				AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn) );
				AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder) );
				
				CustomerList _CustomerList = GetList(cmd, ALL_AVAILABLE_RECORDS);
				request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
				return _CustomerList;
			}
		}
        public DataSet GetJobDetailByJobId(Guid ticketid)
        {
            string sqlQuery = @"declare @ticketid uniqueidentifier
                                set @ticketid = '{0}'

                                select * into #temp from [Lookup] where DataKey = 'TicketStatus'

                                select tk.*
                                ,cus.FirstName + ' ' + cus.LastName as CustomerName, assign.FirstName + ' ' + assign.LastName as AssignedPerson
                                , createdby.FirstName + ' ' + createdby.LastName as CreatedPerson, #t.DisplayText as AssignedStatus
								,cus.ProfileImage
								,iif(cus.PrimaryPhone is not null and cus.PrimaryPhone != '', cus.PrimaryPhone, cus.CellNo) as Phone
								,cus.EmailAddress
                                ,cus.Id as CustomerIntId
								,cus.Street, cus.City, cus.[State], cus.ZipCode
                                ,iif((select count(*) from Invoice inv where inv.BookingId != '' and inv.BookingId is not null and inv.BookingId = tk.BookingId) > 0, 1, 0) as HasInv
                                ,iif((select count(*) from TicketReply tr where tr.TicketId = tk.TicketId and CHARINDEX('<p>', tr.[Message]) > 0) > 0, 1, 0) as HasNote
                                ,iif((select count(*) from TicketReply tr where tr.TicketId = tk.TicketId) > 0, 1, 0) as HasLog
								,IIF((select count(*) from Booking bk where bk.BookingId = tk.BookingId) > 0, 1, 0) as HasBooking
								,isnull((select sum(inv.BalanceDue) from Invoice inv where inv.CustomerId = cus.CustomerId and inv.[Status] != 'Init' and inv.IsEstimate = 0 and (inv.[Status] = 'Open' or inv.[Status] = 'Partial')), 0) as InvoiceBalanceDue
                                ,IIF((select count(*) from TicketFile tf where tf.TicketId = tk.TicketId) > 0, 1, 0) as HasFile
                                from Ticket tk
                                left join Customer cus on cus.CustomerId = tk.CustomerId
                                left join TicketUser tu on tu.TiketId = tk.TicketId and tu.IsPrimary = 1
                                left join Employee assign on assign.UserId = tu.UserId 
                                left join Employee createdby on createdby.UserId = tk.CreatedBy
                                left join #temp #t on #t.DataValue = tk.[Status]
                                where tk.TicketId = @ticketid

                                drop table #temp";
            try
            {
                sqlQuery = string.Format(sqlQuery, ticketid);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetJobSchedulesByTicketId(Guid ticketid)
        {
            string sqlQuery = @"declare @ticketid uniqueidentifier
                                set @ticketid = '{0}'

                                select tk.* 
                                from Ticket tk
                                where tk.TicketId = @ticketid

                                select tu.* 
                                from TicketUser tu
                                where tu.TiketId = @ticketid

                                select ca.* 
                                from CustomerAppointment ca
                                left join Ticket tk on ca.AppointmentId = tk.TicketId
                                where tk.TicketId = @ticketid

                                select ama.* 
                                from AdditionalMembersAppointment ama
                                left join Ticket tk on ama.AppointmentId = tk.TicketId
                                where tk.TicketId = @ticketid";
            try
            {
                sqlQuery = string.Format(sqlQuery, ticketid);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public bool ApiPermissionGroupMapByUserIdCompanyIdandPermissionName(Guid UserId, Guid CompanyId, string Key)
        {
            string sqlQuery = @"select _pgm.* from PermissionGroupMap _pgm
                                    where _pgm.PermissionGroupId in (select up.PermissionGroupId from UserPermission up 
                                    where up.UserId = '{0}' 
	                                and up.CompanyId = '{1}')
									and _pgm.PermissionId = (select top(1) Id from Permission where Name = '{2}')
	                                and _pgm.IsActive = 1";

            try
            {
                sqlQuery = string.Format(sqlQuery, UserId, CompanyId, Key);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    List<PermissionGroupMap> modelList = new List<PermissionGroupMap>();
                    if (dsResult != null)
                    {
                        DataTable dtResultd = new DataTable();
                        dtResultd = dsResult.Tables[0];
                        modelList = (from DataRow dr in dtResultd.Rows
                                     select new PermissionGroupMap()
                                     {
                                         Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                         IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
                                         PermissionGroupId = dr["PermissionGroupId"] != DBNull.Value ? Convert.ToInt32(dr["PermissionGroupId"]) : 0,
                                         PermissionId = dr["PermissionId"] != DBNull.Value ? Convert.ToInt32(dr["PermissionId"]) : 0
                                     }).ToList();
                        if (modelList != null && modelList.Count > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Retrieves all Customer objects by query String
        /// </summary>
        /// <returns>A list of Customer objects</returns>
        public CustomerList GetByQuery(String query)
		{
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERBYQUERY))
			{
				AddParameter(cmd, pNVarChar("Query", 4000, query) );
				return GetList(cmd, ALL_AVAILABLE_RECORDS);;
			}
		}
		
		#endregion
		
		
		#region Get Customer Maximum Id Method
		/// <summary>
        /// Retrieves Get Maximum Id of Customer
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetMaxId()
		{
			Int32 _MaximumId = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERMAXIMUMID))
			{
				SqlDataReader reader;
				_MaximumId = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _MaximumId;
		}
		
		#endregion
		
		#region Get Customer Row Count Method
		/// <summary>
        /// Retrieves Get Total Rows of Customer
        /// </summary>
        /// <returns>Int32 type object</returns>
		public Int32 GetRowCount()
		{
			Int32 _CustomerRowCount = 0; 
			using( SqlCommand cmd = GetSPCommand(GETCUSTOMERROWCOUNT))
			{
				SqlDataReader reader;
				_CustomerRowCount = (Int32) SelectRecords(cmd, out reader);
				reader.Close();
				reader.Dispose();
			}
			return _CustomerRowCount;
		}
		
		#endregion
	
		#region Fill Methods
		/// <summary>
        /// Fills Customer object
        /// </summary>
        /// <param name="customerObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
		protected void FillObject(CustomerBase customerObject, SqlDataReader reader, int start)
		{
			
				customerObject.Id = reader.GetInt32( start + 0 );			
				customerObject.CustomerId = reader.GetGuid( start + 1 );			
				if(!reader.IsDBNull(2)) customerObject.CustomerNo = reader.GetString( start + 2 );			
				if(!reader.IsDBNull(3)) customerObject.Title = reader.GetString( start + 3 );			
				if(!reader.IsDBNull(4)) customerObject.FirstName = reader.GetString( start + 4 );			
				if(!reader.IsDBNull(5)) customerObject.LastName = reader.GetString( start + 5 );			
				if(!reader.IsDBNull(6)) customerObject.SSN = reader.GetString( start + 6 );			
				if(!reader.IsDBNull(7)) customerObject.Type = reader.GetString( start + 7 );			
				if(!reader.IsDBNull(8)) customerObject.BusinessName = reader.GetString( start + 8 );			
				if(!reader.IsDBNull(9)) customerObject.DateofBirth = reader.GetDateTime( start + 9 );			
				if(!reader.IsDBNull(10)) customerObject.PrimaryPhone = reader.GetString( start + 10 );			
				if(!reader.IsDBNull(11)) customerObject.SecondaryPhone = reader.GetString( start + 11 );			
				if(!reader.IsDBNull(12)) customerObject.CellNo = reader.GetString( start + 12 );			
				if(!reader.IsDBNull(13)) customerObject.Fax = reader.GetString( start + 13 );			
				if(!reader.IsDBNull(14)) customerObject.EmailAddress = reader.GetString( start + 14 );			
				if(!reader.IsDBNull(15)) customerObject.CallingTime = reader.GetString( start + 15 );			
				if(!reader.IsDBNull(16)) customerObject.Address = reader.GetString( start + 16 );			
				if(!reader.IsDBNull(17)) customerObject.Address2 = reader.GetString( start + 17 );			
				if(!reader.IsDBNull(18)) customerObject.Street = reader.GetString( start + 18 );			
				if(!reader.IsDBNull(19)) customerObject.City = reader.GetString( start + 19 );			
				if(!reader.IsDBNull(20)) customerObject.State = reader.GetString( start + 20 );			
				if(!reader.IsDBNull(21)) customerObject.ZipCode = reader.GetString( start + 21 );			
				if(!reader.IsDBNull(22)) customerObject.Country = reader.GetString( start + 22 );			
				if(!reader.IsDBNull(23)) customerObject.StreetPrevious = reader.GetString( start + 23 );			
				if(!reader.IsDBNull(24)) customerObject.CityPrevious = reader.GetString( start + 24 );			
				if(!reader.IsDBNull(25)) customerObject.StatePrevious = reader.GetString( start + 25 );			
				if(!reader.IsDBNull(26)) customerObject.ZipCodePrevious = reader.GetString( start + 26 );			
				if(!reader.IsDBNull(27)) customerObject.CountryPrevious = reader.GetString( start + 27 );			
				if(!reader.IsDBNull(28)) customerObject.AccountNo = reader.GetString( start + 28 );			
				if(!reader.IsDBNull(29)) customerObject.IsAlarmCom = reader.GetBoolean( start + 29 );			
				if(!reader.IsDBNull(30)) customerObject.CreditScore = reader.GetString( start + 30 );			
				if(!reader.IsDBNull(31)) customerObject.CreditScoreValue = reader.GetInt32( start + 31 );			
				if(!reader.IsDBNull(32)) customerObject.ContractTeam = reader.GetString( start + 32 );			
				if(!reader.IsDBNull(33)) customerObject.FundingCompany = reader.GetString( start + 33 );			
				if(!reader.IsDBNull(34)) customerObject.MonthlyMonitoringFee = reader.GetString( start + 34 );			
				if(!reader.IsDBNull(35)) customerObject.CellularBackup = reader.GetBoolean( start + 35 );			
				if(!reader.IsDBNull(36)) customerObject.LeadSource = reader.GetString( start + 36 );			
				if(!reader.IsDBNull(37)) customerObject.CustomerFunded = reader.GetBoolean( start + 37 );			
				if(!reader.IsDBNull(38)) customerObject.Maintenance = reader.GetBoolean( start + 38 );			
				if(!reader.IsDBNull(39)) customerObject.Note = reader.GetString( start + 39 );			
				if(!reader.IsDBNull(40)) customerObject.SalesDate = reader.GetDateTime( start + 40 );			
				if(!reader.IsDBNull(41)) customerObject.InstallDate = reader.GetDateTime( start + 41 );			
				if(!reader.IsDBNull(42)) customerObject.CutInDate = reader.GetDateTime( start + 42 );			
				if(!reader.IsDBNull(43)) customerObject.Installer = reader.GetString( start + 43 );			
				if(!reader.IsDBNull(44)) customerObject.Soldby = reader.GetString( start + 44 );			
				if(!reader.IsDBNull(45)) customerObject.FundingDate = reader.GetDateTime( start + 45 );			
				if(!reader.IsDBNull(46)) customerObject.MiddleName = reader.GetString( start + 46 );			
				if(!reader.IsDBNull(47)) customerObject.JoinDate = reader.GetDateTime( start + 47 );			
				if(!reader.IsDBNull(48)) customerObject.ReminderDate = reader.GetDateTime( start + 48 );			
				if(!reader.IsDBNull(49)) customerObject.QA1 = reader.GetString( start + 49 );			
				if(!reader.IsDBNull(50)) customerObject.QA1Date = reader.GetDateTime( start + 50 );			
				if(!reader.IsDBNull(51)) customerObject.QA2 = reader.GetString( start + 51 );			
				if(!reader.IsDBNull(52)) customerObject.QA2Date = reader.GetDateTime( start + 52 );			
				if(!reader.IsDBNull(53)) customerObject.Status = reader.GetString( start + 53 );			
				if(!reader.IsDBNull(54)) customerObject.BillAmount = reader.GetDouble( start + 54 );			
				if(!reader.IsDBNull(55)) customerObject.PaymentMethod = reader.GetString( start + 55 );			
				if(!reader.IsDBNull(56)) customerObject.BillCycle = reader.GetString( start + 56 );			
				if(!reader.IsDBNull(57)) customerObject.BillDay = reader.GetInt32( start + 57 );			
				if(!reader.IsDBNull(58)) customerObject.BillNotes = reader.GetString( start + 58 );			
				if(!reader.IsDBNull(59)) customerObject.BillTax = reader.GetBoolean( start + 59 );			
				if(!reader.IsDBNull(60)) customerObject.BillOutStanding = reader.GetDouble( start + 60 );			
				if(!reader.IsDBNull(61)) customerObject.ServiceDate = reader.GetDateTime( start + 61 );			
				if(!reader.IsDBNull(62)) customerObject.Area = reader.GetString( start + 62 );			
				if(!reader.IsDBNull(63)) customerObject.StreetType = reader.GetString( start + 63 );			
				if(!reader.IsDBNull(64)) customerObject.Appartment = reader.GetString( start + 64 );			
				if(!reader.IsDBNull(65)) customerObject.Latlng = reader.GetString( start + 65 );			
				if(!reader.IsDBNull(66)) customerObject.SecondCustomerNo = reader.GetString( start + 66 );			
				if(!reader.IsDBNull(67)) customerObject.AdditionalCustomerNo = reader.GetString( start + 67 );			
				if(!reader.IsDBNull(68)) customerObject.IsTechCallPassed = reader.GetBoolean( start + 68 );			
				if(!reader.IsDBNull(69)) customerObject.IsDirect = reader.GetBoolean( start + 69 );			
				if(!reader.IsDBNull(70)) customerObject.AuthorizeRefId = reader.GetString( start + 70 );			
				if(!reader.IsDBNull(71)) customerObject.AuthorizeCusProfileId = reader.GetString( start + 71 );			
				if(!reader.IsDBNull(72)) customerObject.AuthorizeCusPaymentProfileId = reader.GetString( start + 72 );			
				if(!reader.IsDBNull(73)) customerObject.AuthorizeDescription = reader.GetString( start + 73 );			
				if(!reader.IsDBNull(74)) customerObject.IsRequiredCsvSync = reader.GetBoolean( start + 74 );			
				if(!reader.IsDBNull(75)) customerObject.Passcode = reader.GetString( start + 75 );			
				if(!reader.IsDBNull(76)) customerObject.ActivationFee = reader.GetDouble( start + 76 );			
				if(!reader.IsDBNull(77)) customerObject.FirstBilling = reader.GetDateTime( start + 77 );			
				if(!reader.IsDBNull(78)) customerObject.ActivationFeePaymentMethod = reader.GetString( start + 78 );			
				if(!reader.IsDBNull(79)) customerObject.IsActive = reader.GetBoolean( start + 79 );			
				if(!reader.IsDBNull(80)) customerObject.LastGeneratedInvoice = reader.GetDateTime( start + 80 );			
				if(!reader.IsDBNull(81)) customerObject.Singature = reader.GetString( start + 81 );			
				if(!reader.IsDBNull(82)) customerObject.CrossStreet = reader.GetString( start + 82 );			
				if(!reader.IsDBNull(83)) customerObject.DBA = reader.GetString( start + 83 );			
				if(!reader.IsDBNull(84)) customerObject.AlarmRefId = reader.GetString( start + 84 );			
				if(!reader.IsDBNull(85)) customerObject.TransunionRefId = reader.GetString( start + 85 );			
				if(!reader.IsDBNull(86)) customerObject.MonitronicsRefId = reader.GetString( start + 86 );			
				if(!reader.IsDBNull(87)) customerObject.CentralStationRefId = reader.GetString( start + 87 );			
				if(!reader.IsDBNull(88)) customerObject.CmsRefId = reader.GetString( start + 88 );			
				if(!reader.IsDBNull(89)) customerObject.PreferedEmail = reader.GetBoolean( start + 89 );			
				if(!reader.IsDBNull(90)) customerObject.PreferedSms = reader.GetBoolean( start + 90 );			
				if(!reader.IsDBNull(91)) customerObject.IsAgreement = reader.GetBoolean( start + 91 );			
				if(!reader.IsDBNull(92)) customerObject.IsFireAccount = reader.GetBoolean( start + 92 );			
				customerObject.CreatedByUid = reader.GetGuid( start + 93 );			
				customerObject.CreatedDate = reader.GetDateTime( start + 94 );			
				customerObject.LastUpdatedBy = reader.GetString( start + 95 );			
				customerObject.LastUpdatedByUid = reader.GetGuid( start + 96 );			
				customerObject.LastUpdatedDate = reader.GetDateTime( start + 97 );			
				if(!reader.IsDBNull(98)) customerObject.BusinessAccountType = reader.GetString( start + 98 );			
				if(!reader.IsDBNull(99)) customerObject.PhoneType = reader.GetString( start + 99 );			
				if(!reader.IsDBNull(100)) customerObject.Carrier = reader.GetString( start + 100 );			
				customerObject.ReferringCustomer = reader.GetGuid( start + 101 );			
				if(!reader.IsDBNull(102)) customerObject.EsistingPanel = reader.GetString( start + 102 );			
				if(!reader.IsDBNull(103)) customerObject.Ownership = reader.GetString( start + 103 );			
				if(!reader.IsDBNull(104)) customerObject.PurchasePrice = reader.GetDouble( start + 104 );			
				if(!reader.IsDBNull(105)) customerObject.ContractValue = reader.GetString( start + 105 );			
				customerObject.ChildOf = reader.GetGuid( start + 106 );			
				if(!reader.IsDBNull(107)) customerObject.EmailVerified = reader.GetBoolean( start + 107 );			
				if(!reader.IsDBNull(108)) customerObject.HomeVerified = reader.GetBoolean( start + 108 );			
				if(!reader.IsDBNull(109)) customerObject.County = reader.GetString( start + 109 );			
				if(!reader.IsDBNull(110)) customerObject.CustomerToken = reader.GetString( start + 110 );			
				if(!reader.IsDBNull(111)) customerObject.PaymentToken = reader.GetString( start + 111 );			
				if(!reader.IsDBNull(112)) customerObject.ScheduleToken = reader.GetString( start + 112 );			
				if(!reader.IsDBNull(113)) customerObject.EstCloseDate = reader.GetDateTime( start + 113 );			
				if(!reader.IsDBNull(114)) customerObject.ProjectWalkDate = reader.GetDateTime( start + 114 );			
				if(!reader.IsDBNull(115)) customerObject.BranchId = reader.GetInt32( start + 115 );			
				if(!reader.IsDBNull(116)) customerObject.SubscriptionStatus = reader.GetString( start + 116 );			
				if(!reader.IsDBNull(117)) customerObject.AnnualRevenue = reader.GetDouble( start + 117 );			
				if(!reader.IsDBNull(118)) customerObject.Website = reader.GetString( start + 118 );			
				if(!reader.IsDBNull(119)) customerObject.Market = reader.GetString( start + 119 );			
				if(!reader.IsDBNull(120)) customerObject.Passengers = reader.GetInt32( start + 120 );			
				if(!reader.IsDBNull(121)) customerObject.Budget = reader.GetDouble( start + 121 );			
				if(!reader.IsDBNull(122)) customerObject.SmartSetUpStep = reader.GetInt32( start + 122 );			
				if(!reader.IsDBNull(123)) customerObject.CustomerAccountType = reader.GetString( start + 123 );			
				if(!reader.IsDBNull(124)) customerObject.IsPrimaryPhoneVerified = reader.GetBoolean( start + 124 );			
				if(!reader.IsDBNull(125)) customerObject.IsSecondaryPhoneVerified = reader.GetBoolean( start + 125 );			
				if(!reader.IsDBNull(126)) customerObject.IsCellNoVerified = reader.GetBoolean( start + 126 );			
				if(!reader.IsDBNull(127)) customerObject.HomeOwner = reader.GetString( start + 127 );			
				customerObject.AccessGivenTo = reader.GetGuid( start + 128 );			
				if(!reader.IsDBNull(129)) customerObject.DoNotCall = reader.GetDateTime( start + 129 );			
				if(!reader.IsDBNull(130)) customerObject.PreferredContactMethod = reader.GetString( start + 130 );			
				if(!reader.IsDBNull(131)) customerObject.SalesLocation = reader.GetString( start + 131 );			
				if(!reader.IsDBNull(132)) customerObject.IsReceivePaymentMail = reader.GetBoolean( start + 132 );			
				if(!reader.IsDBNull(133)) customerObject.BestTimeToCall = reader.GetString( start + 133 );			
				if(!reader.IsDBNull(134)) customerObject.CSProvider = reader.GetString( start + 134 );			
				if(!reader.IsDBNull(135)) customerObject.RenewalTerm = reader.GetInt32( start + 135 );			
				if(!reader.IsDBNull(136)) customerObject.PreferedCall = reader.GetBoolean( start + 136 );			
				if(!reader.IsDBNull(137)) customerObject.IsAgreementSend = reader.GetBoolean( start + 137 );			
				if(!reader.IsDBNull(138)) customerObject.IsACHDiscount = reader.GetBoolean( start + 138 );			
				if(!reader.IsDBNull(139)) customerObject.CustomerStatus = reader.GetString( start + 139 );			
				if(!reader.IsDBNull(140)) customerObject.TransferCustomerId = reader.GetInt32( start + 140 );			
				if(!reader.IsDBNull(141)) customerObject.CancellationSignature = reader.GetString( start + 141 );			
				if(!reader.IsDBNull(142)) customerObject.OriginalContactDate = reader.GetDateTime( start + 142 );			
				customerObject.DuplicateCustomer = reader.GetGuid( start + 143 );			
				if(!reader.IsDBNull(144)) customerObject.InspectionCompany = reader.GetString( start + 144 );			
				customerObject.SoldBy2 = reader.GetGuid( start + 145 );			
				customerObject.SoldBy3 = reader.GetGuid( start + 146 );			
				if(!reader.IsDBNull(147)) customerObject.ContactedPerviously = reader.GetString( start + 147 );			
				if(!reader.IsDBNull(148)) customerObject.MovingDate = reader.GetDateTime( start + 148 );			
				if(!reader.IsDBNull(149)) customerObject.InstalledStatus = reader.GetString( start + 149 );			
				if(!reader.IsDBNull(150)) customerObject.AcquiredFrom = reader.GetString( start + 150 );			
				if(!reader.IsDBNull(151)) customerObject.FollowUpDate = reader.GetDateTime( start + 151 );			
				if(!reader.IsDBNull(152)) customerObject.BuyoutAmountByADS = reader.GetDouble( start + 152 );			
				if(!reader.IsDBNull(153)) customerObject.BuyoutAmountBySalesRep = reader.GetDouble( start + 153 );			
				if(!reader.IsDBNull(154)) customerObject.FinancedTerm = reader.GetDouble( start + 154 );			
				if(!reader.IsDBNull(155)) customerObject.FinancedAmount = reader.GetDouble( start + 155 );			
				if(!reader.IsDBNull(156)) customerObject.Levels = reader.GetDouble( start + 156 );			
				if(!reader.IsDBNull(157)) customerObject.SoldAmount = reader.GetDouble( start + 157 );			
				if(!reader.IsDBNull(158)) customerObject.AgreementEmail = reader.GetString( start + 158 );			
				if(!reader.IsDBNull(159)) customerObject.AgreementPhoneNo = reader.GetString( start + 159 );			
				if(!reader.IsDBNull(160)) customerObject.TaxExemption = reader.GetString( start + 160 );			
				if(!reader.IsDBNull(161)) customerObject.AppoinmentSet = reader.GetString( start + 161 );			
				if(!reader.IsDBNull(162)) customerObject.BillingEmail = reader.GetString( start + 162 );			
				if(!reader.IsDBNull(163)) customerObject.BillingPhone = reader.GetString( start + 163 );			
				if(!reader.IsDBNull(164)) customerObject.BillingContact = reader.GetString( start + 164 );			
				if(!reader.IsDBNull(165)) customerObject.LastOpenedDate = reader.GetDateTime( start + 165 );			
				if(!reader.IsDBNull(166)) customerObject.DisconnectServiceDate = reader.GetDateTime( start + 166 );			
				if(!reader.IsDBNull(167)) customerObject.UCCRefId = reader.GetString( start + 167 );			
				if(!reader.IsDBNull(168)) customerObject.PlatformId = reader.GetString( start + 168 );			
				if(!reader.IsDBNull(169)) customerObject.RecommendedLevel = reader.GetInt32( start + 169 );			
				if(!reader.IsDBNull(170)) customerObject.ProfileImage = reader.GetString( start + 170 );			
				if(!reader.IsDBNull(171)) customerObject.BrinksRefId = reader.GetString( start + 171 );			
				if(!reader.IsDBNull(172)) customerObject.MapscoNo = reader.GetString( start + 172 );			
				if(!reader.IsDBNull(173)) customerObject.CustomerSignatureDate = reader.GetDateTime( start + 173 );			
				if(!reader.IsDBNull(174)) customerObject.LeadSourceType = reader.GetString( start + 174 );			
				customerObject.MoveCustomerId = reader.GetGuid( start + 175 );			
				if(!reader.IsDBNull(176)) customerObject.EcontractId = reader.GetString( start + 176 );			
				if(!reader.IsDBNull(177)) customerObject.EcontractEnvlobeId = reader.GetString( start + 177 );			
				customerObject.Soldby1 = reader.GetGuid( start + 178 );			
				if(!reader.IsDBNull(179)) customerObject.SearchText = reader.GetString( start + 179 );			
				if(!reader.IsDBNull(180)) customerObject.IsContractSigned = reader.GetBoolean( start + 180 );			
				if(!reader.IsDBNull(181)) customerObject.CustomerFundedDate = reader.GetDateTime( start + 181 );			
			FillBaseObject(customerObject, reader, (start + 182));

			
			customerObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;	
		}
		
		/// <summary>
        /// Fills Customer object
        /// </summary>
        /// <param name="customerObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
		protected void FillObject(CustomerBase customerObject, SqlDataReader reader)
		{
			FillObject(customerObject, reader, 0);
		}
		
		/// <summary>
        /// Retrieves Customer object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>Customer object</returns>
		private Customer GetObject(SqlCommand cmd)
		{
			SqlDataReader reader;
			long rows = SelectRecords(cmd, out reader);

			using(reader)
			{
				if(reader.Read())
				{
					Customer customerObject= new Customer();
					FillObject(customerObject, reader);
					return customerObject;
				}
				else
				{
					return null;
				}				
			}
		}
		
		/// <summary>
        /// Retrieves list of Customer objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of Customer objects</returns>
		private CustomerList GetList(SqlCommand cmd, long rows)
		{
			// Select multiple records
			SqlDataReader reader;
			long result = SelectRecords(cmd, out reader);

			//Customer list
			CustomerList list = new CustomerList();

			using( reader )
			{
				// Read rows until end of result or number of rows specified is reached
				while( reader.Read() && rows-- != 0 )
				{
					Customer customerObject = new Customer();
					FillObject(customerObject, reader);

					list.Add(customerObject);
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
