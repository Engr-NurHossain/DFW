using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using HS.Framework;
using HS.Framework.DataAccess;
using HS.Framework.Exceptions;
using System;
using System.Data.SqlClient;

namespace HS.DataAccess
{
    public partial class CustomerExtendedDataAccess : BaseDataAccess
    {
        #region Constants
        private const string INSERTCUSTOMEREXTENDED = "InsertCustomerExtended";
        private const string UPDATECUSTOMEREXTENDED = "UpdateCustomerExtended";
        private const string DELETECUSTOMEREXTENDED = "DeleteCustomerExtended";
        private const string GETCUSTOMEREXTENDEDBYID = "GetCustomerExtendedById";
        private const string GETALLCUSTOMEREXTENDED = "GetAllCustomerExtended";
        private const string GETPAGEDCUSTOMEREXTENDED = "GetPagedCustomerExtended";
        private const string GETCUSTOMEREXTENDEDMAXIMUMID = "GetCustomerExtendedMaximumId";
        private const string GETCUSTOMEREXTENDEDROWCOUNT = "GetCustomerExtendedRowCount";
        private const string GETCUSTOMEREXTENDEDBYQUERY = "GetCustomerExtendedByQuery";
        #endregion

        #region Constructors

        public CustomerExtendedDataAccess(ClientContext context) : base(context) { }
        public CustomerExtendedDataAccess(SqlTransaction transaction, ClientContext context) : base(transaction, context) { }
        #endregion

        #region AddCommonParams Method
        /// <summary>
        /// Add common parameters before calling a procedure
        /// </summary>
        /// <param name="cmd">command object, where parameters will be added</param>
        /// <param name="customerExtendedObject"></param>
        private void AddCommonParams(SqlCommand cmd, CustomerExtendedBase customerExtendedObject)
        {
            AddParameter(cmd, pGuid(CustomerExtendedBase.Property_CustomerId, customerExtendedObject.CustomerId));
            AddParameter(cmd, pBool(CustomerExtendedBase.Property_Takeover, customerExtendedObject.Takeover));
            AddParameter(cmd, pBool(CustomerExtendedBase.Property_PreWired, customerExtendedObject.PreWired));
            AddParameter(cmd, pBool(CustomerExtendedBase.Property_HardWired, customerExtendedObject.HardWired));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_CSAgreement, 50, customerExtendedObject.CSAgreement));
            AddParameter(cmd, pGuid(CustomerExtendedBase.Property_SalesPerson4, customerExtendedObject.SalesPerson4));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_FinanceCompany, 100, customerExtendedObject.FinanceCompany));
            AddParameter(cmd, pDateTime(CustomerExtendedBase.Property_ContractStartDate, customerExtendedObject.ContractStartDate));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_RemainingContractTerm, 150, customerExtendedObject.RemainingContractTerm));
            AddParameter(cmd, pBool(CustomerExtendedBase.Property_IsFinanced, customerExtendedObject.IsFinanced));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_Pets, 150, customerExtendedObject.Pets));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_PetsType, 150, customerExtendedObject.PetsType));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_Repair, 150, customerExtendedObject.Repair));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_RepairType, 150, customerExtendedObject.RepairType));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_BirthDateCoupon, 150, customerExtendedObject.BirthDateCoupon));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_VipClubMember, 150, customerExtendedObject.VipClubMember));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_RWST1, 50, customerExtendedObject.RWST1));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_RWST2, 50, customerExtendedObject.RWST2));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_RWST3, 50, customerExtendedObject.RWST3));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_RWST4, 50, customerExtendedObject.RWST4));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_RWST5, 50, customerExtendedObject.RWST5));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_RWST6, 50, customerExtendedObject.RWST6));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_RWST7, 50, customerExtendedObject.RWST7));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_RWST8, 50, customerExtendedObject.RWST8));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_RWST9, 50, customerExtendedObject.RWST9));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_RWST10, 50, customerExtendedObject.RWST10));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_RWST11, 50, customerExtendedObject.RWST11));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_RWST12, 50, customerExtendedObject.RWST12));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_RWST13, 50, customerExtendedObject.RWST13));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_RWST14, 50, customerExtendedObject.RWST14));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_RWST15, 50, customerExtendedObject.RWST15));
            AddParameter(cmd, pDateTime(CustomerExtendedBase.Property_RepsAssignedDate, customerExtendedObject.RepsAssignedDate));
            AddParameter(cmd, pGuid(CustomerExtendedBase.Property_ContractSentBy, customerExtendedObject.ContractSentBy));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_SecondaryFirstName, 50, customerExtendedObject.SecondaryFirstName));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_SecondaryLastName, 50, customerExtendedObject.SecondaryLastName));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_SecondarySSN, 50, customerExtendedObject.SecondarySSN));
            AddParameter(cmd, pDateTime(CustomerExtendedBase.Property_SecondaryBirthDate, customerExtendedObject.SecondaryBirthDate));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_SecondaryEmail, 50, customerExtendedObject.SecondaryEmail));
            AddParameter(cmd, pBool(CustomerExtendedBase.Property_FundingResult, customerExtendedObject.FundingResult));
            AddParameter(cmd, pDouble(CustomerExtendedBase.Property_GrossFundedAmount, customerExtendedObject.GrossFundedAmount));
            AddParameter(cmd, pDouble(CustomerExtendedBase.Property_NetFundedAmount, customerExtendedObject.NetFundedAmount));
            AddParameter(cmd, pDouble(CustomerExtendedBase.Property_DiscountFundedAmount, customerExtendedObject.DiscountFundedAmount));
            AddParameter(cmd, pDouble(CustomerExtendedBase.Property_DiscountFundedPercentage, customerExtendedObject.DiscountFundedPercentage));
            AddParameter(cmd, pDouble(CustomerExtendedBase.Property_CustomerPaymentAmount, customerExtendedObject.CustomerPaymentAmount));
            AddParameter(cmd, pDouble(CustomerExtendedBase.Property_FinanceRepCommissionRate, customerExtendedObject.FinanceRepCommissionRate));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_LoanNumber, 100, customerExtendedObject.LoanNumber));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_CreditAppNumber, 100, customerExtendedObject.CreditAppNumber));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_Term, 100, customerExtendedObject.Term));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_APR, 100, customerExtendedObject.APR));
            AddParameter(cmd, pDouble(CustomerExtendedBase.Property_MaxCreditLimit, customerExtendedObject.MaxCreditLimit));
            AddParameter(cmd, pDateTime(CustomerExtendedBase.Property_ApprovalDate, customerExtendedObject.ApprovalDate));
            AddParameter(cmd, pDouble(CustomerExtendedBase.Property_MonthlyFinanceRate, customerExtendedObject.MonthlyFinanceRate));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_Batch, 50, customerExtendedObject.Batch));
            AddParameter(cmd, pGuid(CustomerExtendedBase.Property_FinanceRep, customerExtendedObject.FinanceRep));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_CreditTransectionId, 100, customerExtendedObject.CreditTransectionId));
            AddParameter(cmd, pInt32(CustomerExtendedBase.Property_BounceMatchId, customerExtendedObject.BounceMatchId));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_BounceStatus, 50, customerExtendedObject.BounceStatus));
            AddParameter(cmd, pDateTime(CustomerExtendedBase.Property_InstallFinishDate, customerExtendedObject.InstallFinishDate));
            AddParameter(cmd, pInt32(CustomerExtendedBase.Property_PromotionMonth, customerExtendedObject.PromotionMonth));
            AddParameter(cmd, pInt32(CustomerExtendedBase.Property_PrepaidMonth, customerExtendedObject.PrepaidMonth));
            AddParameter(cmd, pDateTime(CustomerExtendedBase.Property_PaymentEffectiveDate, customerExtendedObject.PaymentEffectiveDate));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_FacebookProfileUrl, 250, customerExtendedObject.FacebookProfileUrl));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_GoogleProfileUrl, 250, customerExtendedObject.GoogleProfileUrl));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_FacebookName, 250, customerExtendedObject.FacebookName));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_GoogleName, 250, customerExtendedObject.GoogleName));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_LeadVersion, 50, customerExtendedObject.LeadVersion));
            AddParameter(cmd, pGuid(CustomerExtendedBase.Property_AppoinmentSetBy, customerExtendedObject.AppoinmentSetBy));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_IsPcApplicationId, 50, customerExtendedObject.IsPcApplicationId));
            AddParameter(cmd, pBool(CustomerExtendedBase.Property_IsPcCreditApproved, customerExtendedObject.IsPcCreditApproved));
            AddParameter(cmd, pDateTime(CustomerExtendedBase.Property_BrinksCancelDate, customerExtendedObject.BrinksCancelDate));
            AddParameter(cmd, pDateTime(CustomerExtendedBase.Property_ReceivedDate, customerExtendedObject.ReceivedDate));
            AddParameter(cmd, pDateTime(CustomerExtendedBase.Property_BrinksFundingDate, customerExtendedObject.BrinksFundingDate));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_PlanCode, 50, customerExtendedObject.PlanCode));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_NewMMR, 50, customerExtendedObject.NewMMR));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_LoanAmount, 50, customerExtendedObject.LoanAmount));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_Payout, 50, customerExtendedObject.Payout));
            AddParameter(cmd, pDateTime(CustomerExtendedBase.Property_FinanceFundingDate, customerExtendedObject.FinanceFundingDate));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_BrinksFundingStatus, 50, customerExtendedObject.BrinksFundingStatus));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_FinanceFundingStatus, 50, customerExtendedObject.FinanceFundingStatus));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_IsPcAppStatus, 100, customerExtendedObject.IsPcAppStatus));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_AlarmBasicPackage, 50, customerExtendedObject.AlarmBasicPackage));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_NMCRefId, 50, customerExtendedObject.NMCRefId));
            AddParameter(cmd, pDateTime(CustomerExtendedBase.Property_CustomerSince, customerExtendedObject.CustomerSince));
            AddParameter(cmd, pDateTime(CustomerExtendedBase.Property_ResignDate, customerExtendedObject.ResignDate));
            AddParameter(cmd, pInt32(CustomerExtendedBase.Property_MonthlyBatch, customerExtendedObject.MonthlyBatch));
            AddParameter(cmd, pBool(CustomerExtendedBase.Property_IsAgreementSMSSend, customerExtendedObject.IsAgreementSMSSend));
            AddParameter(cmd, pBool(CustomerExtendedBase.Property_UnlinkCustomer, customerExtendedObject.UnlinkCustomer));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_GeeseLead, 50, customerExtendedObject.GeeseLead));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_PowerPayAppId, 50, customerExtendedObject.PowerPayAppId));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_PowerPayAppStatus, 50, customerExtendedObject.PowerPayAppStatus));
            AddParameter(cmd, pInt32(CustomerExtendedBase.Property_GeeseCount, customerExtendedObject.GeeseCount));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_AvantgradRefId, 50, customerExtendedObject.AvantgradRefId));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_DrivingLicense, 50, customerExtendedObject.DrivingLicense));
            AddParameter(cmd, pDateTime(CustomerExtendedBase.Property_CreatedDay, customerExtendedObject.CreatedDay));
            AddParameter(cmd, pGuid(CustomerExtendedBase.Property_ResignedBy, customerExtendedObject.ResignedBy));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_ContractType, 50, customerExtendedObject.ContractType));
            AddParameter(cmd, pBool(CustomerExtendedBase.Property_IsSignAgrSendToCus, customerExtendedObject.IsSignAgrSendToCus));
            AddParameter(cmd, pBool(CustomerExtendedBase.Property_IsTestAccount, customerExtendedObject.IsTestAccount));
            AddParameter(cmd, pDecimal(CustomerExtendedBase.Property_DealerFee, customerExtendedObject.DealerFee));
            AddParameter(cmd, pDateTime(CustomerExtendedBase.Property_ContractCreatedDate, customerExtendedObject.ContractCreatedDate));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_Warranty, 100, customerExtendedObject.Warranty));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_Keypad, 100, customerExtendedObject.Keypad));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_FrontEnd, 100, customerExtendedObject.FrontEnd));
            AddParameter(cmd, pDateTime(CustomerExtendedBase.Property_CycleStartDate, customerExtendedObject.CycleStartDate));
            AddParameter(cmd, pNVarChar(CustomerExtendedBase.Property_CellSerialNo, 150, customerExtendedObject.CellSerialNo));
        }
        #endregion

        #region Insert Method
        /// <summary>
        /// Inserts CustomerExtended
        /// </summary>
        /// <param name="customerExtendedObject">Object to be inserted</param>
        /// <returns>Number of rows affected</returns>
        public long Insert(CustomerExtendedBase customerExtendedObject)
        {
            try
            {
                SqlCommand cmd = GetSPCommand(INSERTCUSTOMEREXTENDED);

                AddParameter(cmd, pInt32Out(CustomerExtendedBase.Property_Id));
                AddCommonParams(cmd, customerExtendedObject);

                long result = InsertRecord(cmd);
                if (result > 0)
                {
                    customerExtendedObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
                    customerExtendedObject.Id = (Int32)GetOutParameter(cmd, CustomerExtendedBase.Property_Id);
                }
                return result;
            }
            catch (SqlException x)
            {
                throw new ObjectInsertException(customerExtendedObject, x);
            }
        }
        #endregion

        #region Update Method
        /// <summary>
        /// Updates CustomerExtended
        /// </summary>
        /// <param name="customerExtendedObject">Object to be updated</param>
        /// <returns>Number of rows affected</returns>
        public long Update(CustomerExtendedBase customerExtendedObject)
        {
            try
            {
                SqlCommand cmd = GetSPCommand(UPDATECUSTOMEREXTENDED);

                AddParameter(cmd, pInt32(CustomerExtendedBase.Property_Id, customerExtendedObject.Id));
                AddCommonParams(cmd, customerExtendedObject);

                long result = UpdateRecord(cmd);
                if (result > 0)
                    customerExtendedObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
                return result;
            }
            catch (SqlException x)
            {
                throw new ObjectUpdateException(customerExtendedObject, x);
            }
        }
        #endregion

        #region Delete Method
        /// <summary>
        /// Deletes CustomerExtended
        /// </summary>
        /// <param name="Id">Id of the CustomerExtended object that will be deleted</param>
        /// <returns>Number of rows affected</returns>
        public long Delete(Int32 _Id)
        {
            try
            {
                SqlCommand cmd = GetSPCommand(DELETECUSTOMEREXTENDED);

                AddParameter(cmd, pInt32(CustomerExtendedBase.Property_Id, _Id));

                return DeleteRecord(cmd);
            }
            catch (SqlException x)
            {
                throw new ObjectDeleteException(typeof(CustomerExtended), _Id, x);
            }

        }
        #endregion

        #region Get By Id Method
        /// <summary>
        /// Retrieves CustomerExtended object using it's Id
        /// </summary>
        /// <param name="Id">The Id of the CustomerExtended object to retrieve</param>
        /// <returns>CustomerExtended object, null if not found</returns>
        public CustomerExtended Get(Int32 _Id)
        {
            using (SqlCommand cmd = GetSPCommand(GETCUSTOMEREXTENDEDBYID))
            {
                AddParameter(cmd, pInt32(CustomerExtendedBase.Property_Id, _Id));

                return GetObject(cmd);
            }
        }
        #endregion

        #region GetAll Method
        /// <summary>
        /// Retrieves all CustomerExtended objects 
        /// </summary>
        /// <returns>A list of CustomerExtended objects</returns>
        public CustomerExtendedList GetAll()
        {
            using (SqlCommand cmd = GetSPCommand(GETALLCUSTOMEREXTENDED))
            {
                return GetList(cmd, ALL_AVAILABLE_RECORDS);
            }
        }


        /// <summary>
        /// Retrieves all CustomerExtended objects by PageRequest
        /// </summary>
        /// <returns>A list of CustomerExtended objects</returns>
        public CustomerExtendedList GetPaged(PagedRequest request)
        {
            using (SqlCommand cmd = GetSPCommand(GETPAGEDCUSTOMEREXTENDED))
            {
                AddParameter(cmd, pInt32Out("TotalRows"));
                AddParameter(cmd, pInt32("PageIndex", request.PageIndex));
                AddParameter(cmd, pInt32("RowPerPage", request.RowPerPage));
                AddParameter(cmd, pNVarChar("WhereClause", 4000, request.WhereClause));
                AddParameter(cmd, pNVarChar("SortColumn", 128, request.SortColumn));
                AddParameter(cmd, pNVarChar("SortOrder", 4, request.SortOrder));

                CustomerExtendedList _CustomerExtendedList = GetList(cmd, ALL_AVAILABLE_RECORDS);
                request.TotalRows = Convert.ToInt32(GetOutParameter(cmd, "TotalRows"));
                return _CustomerExtendedList;
            }
        }

        /// <summary>
        /// Retrieves all CustomerExtended objects by query String
        /// </summary>
        /// <returns>A list of CustomerExtended objects</returns>
        public CustomerExtendedList GetByQuery(String query)
        {
            using (SqlCommand cmd = GetSPCommand(GETCUSTOMEREXTENDEDBYQUERY))
            {
                AddParameter(cmd, pNVarChar("Query", 4000, query));
                return GetList(cmd, ALL_AVAILABLE_RECORDS); ;
            }
        }

        #endregion


        #region Get CustomerExtended Maximum Id Method
        /// <summary>
        /// Retrieves Get Maximum Id of CustomerExtended
        /// </summary>
        /// <returns>Int32 type object</returns>
        public Int32 GetMaxId()
        {
            Int32 _MaximumId = 0;
            using (SqlCommand cmd = GetSPCommand(GETCUSTOMEREXTENDEDMAXIMUMID))
            {
                SqlDataReader reader;
                _MaximumId = (Int32)SelectRecords(cmd, out reader);
                reader.Close();
                reader.Dispose();
            }
            return _MaximumId;
        }

        #endregion

        #region Get CustomerExtended Row Count Method
        /// <summary>
        /// Retrieves Get Total Rows of CustomerExtended
        /// </summary>
        /// <returns>Int32 type object</returns>
        public Int32 GetRowCount()
        {
            Int32 _CustomerExtendedRowCount = 0;
            using (SqlCommand cmd = GetSPCommand(GETCUSTOMEREXTENDEDROWCOUNT))
            {
                SqlDataReader reader;
                _CustomerExtendedRowCount = (Int32)SelectRecords(cmd, out reader);
                reader.Close();
                reader.Dispose();
            }
            return _CustomerExtendedRowCount;
        }

        #endregion

        #region Fill Methods
        /// <summary>
        /// Fills CustomerExtended object
        /// </summary>
        /// <param name="customerExtendedObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        /// <param name="start">The ordinal position from which to start reading the reader</param>
        protected void FillObject(CustomerExtendedBase customerExtendedObject, SqlDataReader reader, int start)
        {

            customerExtendedObject.Id = reader.GetInt32(start + 0);
            customerExtendedObject.CustomerId = reader.GetGuid(start + 1);
            if (!reader.IsDBNull(2)) customerExtendedObject.Takeover = reader.GetBoolean(start + 2);
            if (!reader.IsDBNull(3)) customerExtendedObject.PreWired = reader.GetBoolean(start + 3);
            if (!reader.IsDBNull(4)) customerExtendedObject.HardWired = reader.GetBoolean(start + 4);
            if (!reader.IsDBNull(5)) customerExtendedObject.CSAgreement = reader.GetString(start + 5);
            customerExtendedObject.SalesPerson4 = reader.GetGuid(start + 6);
            if (!reader.IsDBNull(7)) customerExtendedObject.FinanceCompany = reader.GetString(start + 7);
            if (!reader.IsDBNull(8)) customerExtendedObject.ContractStartDate = reader.GetDateTime(start + 8);
            if (!reader.IsDBNull(9)) customerExtendedObject.RemainingContractTerm = reader.GetString(start + 9);
            if (!reader.IsDBNull(10)) customerExtendedObject.IsFinanced = reader.GetBoolean(start + 10);
            if (!reader.IsDBNull(11)) customerExtendedObject.Pets = reader.GetString(start + 11);
            if (!reader.IsDBNull(12)) customerExtendedObject.PetsType = reader.GetString(start + 12);
            if (!reader.IsDBNull(13)) customerExtendedObject.Repair = reader.GetString(start + 13);
            if (!reader.IsDBNull(14)) customerExtendedObject.RepairType = reader.GetString(start + 14);
            if (!reader.IsDBNull(15)) customerExtendedObject.BirthDateCoupon = reader.GetString(start + 15);
            if (!reader.IsDBNull(16)) customerExtendedObject.VipClubMember = reader.GetString(start + 16);
            if (!reader.IsDBNull(17)) customerExtendedObject.RWST1 = reader.GetString(start + 17);
            if (!reader.IsDBNull(18)) customerExtendedObject.RWST2 = reader.GetString(start + 18);
            if (!reader.IsDBNull(19)) customerExtendedObject.RWST3 = reader.GetString(start + 19);
            if (!reader.IsDBNull(20)) customerExtendedObject.RWST4 = reader.GetString(start + 20);
            if (!reader.IsDBNull(21)) customerExtendedObject.RWST5 = reader.GetString(start + 21);
            if (!reader.IsDBNull(22)) customerExtendedObject.RWST6 = reader.GetString(start + 22);
            if (!reader.IsDBNull(23)) customerExtendedObject.RWST7 = reader.GetString(start + 23);
            if (!reader.IsDBNull(24)) customerExtendedObject.RWST8 = reader.GetString(start + 24);
            if (!reader.IsDBNull(25)) customerExtendedObject.RWST9 = reader.GetString(start + 25);
            if (!reader.IsDBNull(26)) customerExtendedObject.RWST10 = reader.GetString(start + 26);
            if (!reader.IsDBNull(27)) customerExtendedObject.RWST11 = reader.GetString(start + 27);
            if (!reader.IsDBNull(28)) customerExtendedObject.RWST12 = reader.GetString(start + 28);
            if (!reader.IsDBNull(29)) customerExtendedObject.RWST13 = reader.GetString(start + 29);
            if (!reader.IsDBNull(30)) customerExtendedObject.RWST14 = reader.GetString(start + 30);
            if (!reader.IsDBNull(31)) customerExtendedObject.RWST15 = reader.GetString(start + 31);
            if (!reader.IsDBNull(32)) customerExtendedObject.RepsAssignedDate = reader.GetDateTime(start + 32);
            customerExtendedObject.ContractSentBy = reader.GetGuid(start + 33);
            if (!reader.IsDBNull(34)) customerExtendedObject.SecondaryFirstName = reader.GetString(start + 34);
            if (!reader.IsDBNull(35)) customerExtendedObject.SecondaryLastName = reader.GetString(start + 35);
            if (!reader.IsDBNull(36)) customerExtendedObject.SecondarySSN = reader.GetString(start + 36);
            if (!reader.IsDBNull(37)) customerExtendedObject.SecondaryBirthDate = reader.GetDateTime(start + 37);
            if (!reader.IsDBNull(38)) customerExtendedObject.SecondaryEmail = reader.GetString(start + 38);
            if (!reader.IsDBNull(39)) customerExtendedObject.FundingResult = reader.GetBoolean(start + 39);
            if (!reader.IsDBNull(40)) customerExtendedObject.GrossFundedAmount = reader.GetDouble(start + 40);
            if (!reader.IsDBNull(41)) customerExtendedObject.NetFundedAmount = reader.GetDouble(start + 41);
            if (!reader.IsDBNull(42)) customerExtendedObject.DiscountFundedAmount = reader.GetDouble(start + 42);
            if (!reader.IsDBNull(43)) customerExtendedObject.DiscountFundedPercentage = reader.GetDouble(start + 43);
            if (!reader.IsDBNull(44)) customerExtendedObject.CustomerPaymentAmount = reader.GetDouble(start + 44);
            if (!reader.IsDBNull(45)) customerExtendedObject.FinanceRepCommissionRate = reader.GetDouble(start + 45);
            if (!reader.IsDBNull(46)) customerExtendedObject.LoanNumber = reader.GetString(start + 46);
            if (!reader.IsDBNull(47)) customerExtendedObject.CreditAppNumber = reader.GetString(start + 47);
            if (!reader.IsDBNull(48)) customerExtendedObject.Term = reader.GetString(start + 48);
            if (!reader.IsDBNull(49)) customerExtendedObject.APR = reader.GetString(start + 49);
            if (!reader.IsDBNull(50)) customerExtendedObject.MaxCreditLimit = reader.GetDouble(start + 50);
            if (!reader.IsDBNull(51)) customerExtendedObject.ApprovalDate = reader.GetDateTime(start + 51);
            if (!reader.IsDBNull(52)) customerExtendedObject.MonthlyFinanceRate = reader.GetDouble(start + 52);
            if (!reader.IsDBNull(53)) customerExtendedObject.Batch = reader.GetString(start + 53);
            customerExtendedObject.FinanceRep = reader.GetGuid(start + 54);
            if (!reader.IsDBNull(55)) customerExtendedObject.CreditTransectionId = reader.GetString(start + 55);
            if (!reader.IsDBNull(56)) customerExtendedObject.BounceMatchId = reader.GetInt32(start + 56);
            if (!reader.IsDBNull(57)) customerExtendedObject.BounceStatus = reader.GetString(start + 57);
            if (!reader.IsDBNull(58)) customerExtendedObject.InstallFinishDate = reader.GetDateTime(start + 58);
            if (!reader.IsDBNull(59)) customerExtendedObject.PromotionMonth = reader.GetInt32(start + 59);
            if (!reader.IsDBNull(60)) customerExtendedObject.PrepaidMonth = reader.GetInt32(start + 60);
            if (!reader.IsDBNull(61)) customerExtendedObject.PaymentEffectiveDate = reader.GetDateTime(start + 61);
            if (!reader.IsDBNull(62)) customerExtendedObject.FacebookProfileUrl = reader.GetString(start + 62);
            if (!reader.IsDBNull(63)) customerExtendedObject.GoogleProfileUrl = reader.GetString(start + 63);
            if (!reader.IsDBNull(64)) customerExtendedObject.FacebookName = reader.GetString(start + 64);
            if (!reader.IsDBNull(65)) customerExtendedObject.GoogleName = reader.GetString(start + 65);
            if (!reader.IsDBNull(66)) customerExtendedObject.LeadVersion = reader.GetString(start + 66);
            customerExtendedObject.AppoinmentSetBy = reader.GetGuid(start + 67);
            if (!reader.IsDBNull(68)) customerExtendedObject.IsPcApplicationId = reader.GetString(start + 68);
            if (!reader.IsDBNull(69)) customerExtendedObject.IsPcCreditApproved = reader.GetBoolean(start + 69);
            if (!reader.IsDBNull(70)) customerExtendedObject.BrinksCancelDate = reader.GetDateTime(start + 70);
            if (!reader.IsDBNull(71)) customerExtendedObject.ReceivedDate = reader.GetDateTime(start + 71);
            if (!reader.IsDBNull(72)) customerExtendedObject.BrinksFundingDate = reader.GetDateTime(start + 72);
            if (!reader.IsDBNull(73)) customerExtendedObject.PlanCode = reader.GetString(start + 73);
            if (!reader.IsDBNull(74)) customerExtendedObject.NewMMR = reader.GetString(start + 74);
            if (!reader.IsDBNull(75)) customerExtendedObject.LoanAmount = reader.GetString(start + 75);
            if (!reader.IsDBNull(76)) customerExtendedObject.Payout = reader.GetString(start + 76);
            if (!reader.IsDBNull(77)) customerExtendedObject.FinanceFundingDate = reader.GetDateTime(start + 77);
            if (!reader.IsDBNull(78)) customerExtendedObject.BrinksFundingStatus = reader.GetString(start + 78);
            if (!reader.IsDBNull(79)) customerExtendedObject.FinanceFundingStatus = reader.GetString(start + 79);
            if (!reader.IsDBNull(80)) customerExtendedObject.IsPcAppStatus = reader.GetString(start + 80);
            if (!reader.IsDBNull(81)) customerExtendedObject.AlarmBasicPackage = reader.GetString(start + 81);
            if (!reader.IsDBNull(82)) customerExtendedObject.NMCRefId = reader.GetString(start + 82);
            if (!reader.IsDBNull(83)) customerExtendedObject.CustomerSince = reader.GetDateTime(start + 83);
            if (!reader.IsDBNull(84)) customerExtendedObject.ResignDate = reader.GetDateTime(start + 84);
            if (!reader.IsDBNull(85)) customerExtendedObject.MonthlyBatch = reader.GetInt32(start + 85);
            if (!reader.IsDBNull(86)) customerExtendedObject.IsAgreementSMSSend = reader.GetBoolean(start + 86);
            if (!reader.IsDBNull(87)) customerExtendedObject.UnlinkCustomer = reader.GetBoolean(start + 87);
            if (!reader.IsDBNull(88)) customerExtendedObject.GeeseLead = reader.GetString(start + 88);
            if (!reader.IsDBNull(89)) customerExtendedObject.PowerPayAppId = reader.GetString(start + 89);
            if (!reader.IsDBNull(90)) customerExtendedObject.PowerPayAppStatus = reader.GetString(start + 90);
            if (!reader.IsDBNull(91)) customerExtendedObject.GeeseCount = reader.GetInt32(start + 91);
            if (!reader.IsDBNull(92)) customerExtendedObject.AvantgradRefId = reader.GetString(start + 92);
            if (!reader.IsDBNull(93)) customerExtendedObject.DrivingLicense = reader.GetString(start + 93);
            if (!reader.IsDBNull(94)) customerExtendedObject.CreatedDay = reader.GetDateTime(start + 94);
            customerExtendedObject.ResignedBy = reader.GetGuid(start + 95);
            if (!reader.IsDBNull(96)) customerExtendedObject.ContractType = reader.GetString(start + 96);
            if (!reader.IsDBNull(97)) customerExtendedObject.IsSignAgrSendToCus = reader.GetBoolean(start + 97);
            if (!reader.IsDBNull(98)) customerExtendedObject.IsTestAccount = reader.GetBoolean(start + 98);
            if (!reader.IsDBNull(99)) customerExtendedObject.DealerFee = reader.GetDecimal(start + 99);
            if (!reader.IsDBNull(100)) customerExtendedObject.ContractCreatedDate = reader.GetDateTime(start + 100);
            if (!reader.IsDBNull(101)) customerExtendedObject.Warranty = reader.GetString(start + 101);
            if (!reader.IsDBNull(102)) customerExtendedObject.Keypad = reader.GetString(start + 102);
            if (!reader.IsDBNull(103)) customerExtendedObject.FrontEnd = reader.GetString(start + 103);
            if (!reader.IsDBNull(104)) customerExtendedObject.CycleStartDate = reader.GetDateTime(start + 104);
            if (!reader.IsDBNull(105)) customerExtendedObject.CellSerialNo = reader.GetString(start + 105);
            FillBaseObject(customerExtendedObject, reader, (start + 106));


            customerExtendedObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
        }

        /// <summary>
        /// Fills CustomerExtended object
        /// </summary>
        /// <param name="customerExtendedObject">The object to be filled</param>
        /// <param name="reader">The reader to use to fill a single object</param>
        protected void FillObject(CustomerExtendedBase customerExtendedObject, SqlDataReader reader)
        {
            FillObject(customerExtendedObject, reader, 0);
        }

        /// <summary>
        /// Retrieves CustomerExtended object from SqlCommand, after database query
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <returns>CustomerExtended object</returns>
        private CustomerExtended GetObject(SqlCommand cmd)
        {
            SqlDataReader reader;
            long rows = SelectRecords(cmd, out reader);

            using (reader)
            {
                if (reader.Read())
                {
                    CustomerExtended customerExtendedObject = new CustomerExtended();
                    FillObject(customerExtendedObject, reader);
                    return customerExtendedObject;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Retrieves list of CustomerExtended objects from SqlCommand, after database query
        /// number of rows retrieved and returned depends upon the rows field value
        /// </summary>
        /// <param name="cmd">The command object to use for query</param>
        /// <param name="rows">Number of rows to process</param>
        /// <returns>A list of CustomerExtended objects</returns>
        private CustomerExtendedList GetList(SqlCommand cmd, long rows)
        {
            // Select multiple records
            SqlDataReader reader;
            long result = SelectRecords(cmd, out reader);

            //CustomerExtended list
            CustomerExtendedList list = new CustomerExtendedList();

            using (reader)
            {
                // Read rows until end of result or number of rows specified is reached
                while (reader.Read() && rows-- != 0)
                {
                    CustomerExtended customerExtendedObject = new CustomerExtended();
                    FillObject(customerExtendedObject, reader);

                    list.Add(customerExtendedObject);
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