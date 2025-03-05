namespace HS.Framework
{
    public class UserPermissions
    {
        public static class Dashboard
        {
            public static int View { get { return 101; } }
            public static int SummaryReports { get { return 110; } }
            public static int LeadReports { get { return 111; } }
            public static int OrderReports { get { return 151; } }
            public static int RevenueReports { get { return 152; } }
            public static int AvgTicketReports { get { return 153; } }
            public static int CustomerReports { get { return 112; } }
            public static int EstimateInvoiceReports { get { return 113; } }
            public static int RMRAccountReports { get { return 114; } }

            public static int ActivitiyReports { get { return 115; } }
            public static int OpportunityReports { get { return 116; } }

            public static int GraphReports { get { return 120; } }
            public static int LeadReportsGraph { get { return 121; } }
            public static int CustomerReportsGraph { get { return 122; } }
            public static int EstimateInvoiceReportsGraph { get { return 123; } }
            public static int RMRAccountReportsGraph { get { return 124; } }

            public static int ActivitiyReportsGraph { get { return 125; } }
            public static int OpportunityReportsGraph { get { return 126; } }

            public static int Reminders { get { return 130; } }
            public static int AssignedTickets { get { return 131; } }
            public static int UpCommingBirthDays { get { return 132; } }
            public static int UpcomingAnniversaries { get { return 133; } }

            public static int ConvertionReports { get { return 141; } }
            public static int JobReports { get { return 142; } }

            public static int TechnicianLastWeekPay { get { return 143; } }
            public static int TechnicianUpsoldRMR { get { return 144; } }
            public static int TechnicianUpsoldPieces { get { return 145; } }
            public static int Technician90DaysGoBack { get { return 146; } }

            public static int ShowUpsoldBlock { get { return 147; } }
            public static int ShowTechnicianCloseTicketBlock { get { return 148; } }
            public static int ShowTechnicianSalesReport { get { return 149; } }
            public static int ShowTechnicianOpenTicketBlock { get { return 150; } }

            public static int ShowJobInDashboard { get { return 161; } }
            public static int ShowTaskInDashboard { get { return 162; } }
            public static int CurrentUserReminders { get { return 163; } }
            public static int EstimatorList { get { return 164; } }

            public static int ShowAllTicketInDashboard { get { return 165; } }
            public static int GeeseActivityLog { get { return 166; } }

            public static int DashboardSalesReport { get { return 167; } }
            public static int Lead { get { return 168; } }
            public static int Remove { get { return 169; } }
            public static int GoodLead { get { return 170; } }
            public static int Sales { get { return 171; } }
            public static int FirstCall { get { return 172; } }
            public static int Appointment { get { return 173; } }
            public static int SalesFunded { get { return 174; } }
            public static int Cellular { get { return 175; } }
            public static int Smart { get { return 176; } }
            public static int LandLine { get { return 177; } }
            public static int PSP { get { return 178; } }
            public static int GSP { get { return 179; } }
            public static int RMR { get { return 180; } }
            public static int CreditCard { get { return 181; } }
            public static int Financed { get { return 182; } }
            public static int TotalUpfront { get { return 183; } }


        }
        public static class CustomerDashBoard
        {
            public static int CustomerDetails { get { return 201; } }
            public static int CustomerDetailsPersonalInfo { get { return 202; } }
            public static int CustomerDetailsPresentAddress { get { return 203; } }
            public static int CustomerDetailsBilingInfo { get { return 204; } }
            public static int CustomerDetailsAccountHistory { get { return 205; } }
            public static int CustomerDetailsInvoiceHistory { get { return 206; } }
            public static int CustomerDetailsAccountActivity { get { return 207; } }
            public static int CustomerDetailsCustomerViewingHistory { get { return 208; } }

            public static int CustomerInvoiceTab { get { return 220; } }
            public static int CustomerInvoiceList { get { return 221; } }

            public static int CustomerTicketTab { get { return 230; } }
            public static int CustomerTicketList { get { return 231; } }
        }

        //public static class LeftMenuPermission
        //{
        //    public static int Dashboard { get { return 1001; } }
        //    public static int Leads { get { return 1002; } }
        //    public static int Customer { get { return 1003; } }
        //    public static int Schedule { get { return 1004; } }
        //    public static int Inventory { get { return 1005; } }
        //    public static int Sales { get { return 1006; } }
        //    public static int Bill { get { return 1007; } }
        //    public static int Report { get { return 1008; } }
        //}

        public class QTIPermission
        {
            public static int AddKnowledgeBaseContact { get { return 26033; } }
            public static int AddKnowledgeBase { get { return 26023; } }
            public static int DownloadKnowledgebaseList { get { return 26037; } }
            public static int RemoveDocumentFlag { get { return 26018; } }
            public static int EditKnowledgeBase { get { return 26019; } }
            public static int AssignKnowledgebaseToUser { get { return 26044; } }
            public static int RevertKnowledgebase { get { return 26025; } }
            public static int KnowledgebaseSettingCogwheel { get { return 26045; } }
            public static int DeleteKnowledgeBase { get { return 26016; } }
        }
        public static class TopMenuPermission
        {
            public static int TopSetUpMenu1 { get { return 20001; } }
            public static int TopSetUpMenu2 { get { return 20002; } }
            public static int TopMenuCompanyEdit { get { return 20003; } }
        }

        public static class MenuPermissions
        { /*1001*/
            public static int LeftMenuDashboard { get { return 1001; } }
            public static int LeftMenuLeads { get { return 1002; } }
            public static int LeftMenuCustomer { get { return 1003; } }
            public static int LeftMenuSchedule { get { return 1004; } }

            public static int LeftMenuInventory { get { return 1005; } }
            public static int LeftMenuSales { get { return 1006; } }
            public static int LeftMenuExpense { get { return 1007; } }
            public static int LeftMenuReport { get { return 1008; } }
            public static int LeftMenuRecruit { get { return 1009; } }
            #region Added for hudson bus
            public static int LeftMenuContacts { get { return 1042; } }
            public static int LeftMenuOpportunities { get { return 1043; } }
            public static int LeftMenuActivities { get { return 1044; } }
            public static int LeftMenuTickets { get { return 1094; } }
            public static int LeftMenuKnowledgebase { get { return 1099; } }
            public static int LeftMenuAssignArticle { get { return 1135; } }
            public static int LeftMenuAssignArticleReports { get { return 1136; } }
            public static int LeftMenuClassroomAdminView { get { return 1137; } }
            #endregion

            public static int QuickMenuMyCompany { get { return 1010; } }
            public static int QuickMenuMyCompanyCompanyandSystemSettings { get { return 1011; } }
            public static int QuickMenuMyCompanyFees { get { return 1012; } }
            public static int QuickMenuMyCompanyManageUser { get { return 1013; } }
            public static int QuickMenuMyCompanyBranchSettings { get { return 1014; } }
            public static int QuickMenuMyCompanySalesMatrix { get { return 1015; } }
            public static int QuickMenuMyCompanyInstallationMatrix { get { return 1016; } }
            public static int QuickMenuMyCompanyIncomeAndExpenseSettings { get { return 1017; } }
            public static int QuickMenuProducts { get { return 1018; } }
            public static int QuickMenuProductsProductCatagories { get { return 1019; } }
            public static int QuickMenuProductsProductClass { get { return 1020; } }
            public static int QuickMenuProductsProductsandServices { get { return 1021; } }
            public static int QuickMenuProductsCustomerPanelType { get { return 1022; } }
            public static int QuickMenuProductsManufacturers { get { return 1023; } }
            public static int QuickMenuTools { get { return 1024; } }
            public static int QuickMenuToolsCredentialSetting { get { return 1025; } }
            public static int QuickMenuToolsAccountHolder { get { return 1026; } }
            public static int QuickMenuToolsCustomerSystemNo { get { return 1027; } }
            public static int QuickMenuToolsPackageSetup { get { return 1028; } }
            public static int QuickMenuProfileCompany { get { return 1029; } }
            public static int QuickMenuMyCompanyManageUserGroup { get { return 1030; } }
            public static int QuickMenuMyCompanyValueSettings { get { return 1031; } }
            public static int QuickMenuMyCompanyEmailTemplateSettings { get { return 1032; } }
            public static int QuickMenuToolsFundingCompany { get { return 1033; } }
            public static int QuickMenuToolsArrivalTimeSetting { get { return 1034; } }
            public static int TopMenuTimeClock { get { return 1035; } }
            public static int TopMenuNotification { get { return 1036; } }
            public static int TopMenuGlobalSearch { get { return 1037; } }
            public static int TopMenuSetting { get { return 1039; } }
            public static int QuickMenuToolsSmartPackageSetup { get { return 1038; } }
            public static int QuickMenuToolsPrintBlankAgreement { get { return 1040; } }
            public static int QuickMenuMyCompanyUISettings { get { return 1041; } }
            public static int QuickMenuImportFromCMS { get { return 1046; } }
            public static int QuickMenuTagManagement { get { return 4444; } }

            public static int QuickMenuProductsService { get { return 1000016; } }
            public static int QuickMenuProductsVehicalManagement { get { return 1000015; } }
            public static int QuickMenuProductsVendor { get { return 1000017; } }

            public static int QuickMenuTimeClock { get { return 1045; } }
            public static int QuickMenuPTOTab { get { return 100000001; } }

            public static int QuickMenuMyCompanyRestrictedZipCode { get { return 1090; } }

            public static int QuickSurveySetup { get { return 1047; } }
            public static int TicketNotificationSetting { get { return 3098; } }
            public static int AddPTOEmployee { get { return 1000018; } }
            public static int DeleteEmpTimeClock { get { return 1048; } }

            public static int LeftMenuBookingSalesReport { get { return 1049; } }
            public static int LeftMenuActivitiesReport { get { return 1050; } }
            public static int LeftMenuLeadsReport { get { return 1051; } }
            public static int LeftMenuAccountsReport { get { return 1052; } }
            public static int LeftMenuOpportunitiesReport { get { return 1053; } }
            public static int LeftMenuHudsonLeadsReport { get { return 1054; } }
            public static int LeftMenuSoftBacklogReport { get { return 1055; } }
            public static int LeftMenuHardBacklogReport { get { return 1056; } }
            public static int LeftMenuCustomerReport { get { return 1057; } }
            public static int LeftMenuInventoryReports { get { return 1058; } }
            public static int LeftMenuExpenseReports { get { return 1059; } }
            public static int LeftMenuPayrollReports { get { return 1060; } }
            public static int LeftMenuTicketReports { get { return 1061; } }
            //public static int LeftMenuTechnicianReports { get { return 1080; } }
            public static int LeftMenuSalesReports { get { return 1062; } }
            public static int LeftMenuCancellationCue { get { return 1063; } }
            public static int LeftMenuPartnerReports { get { return 1064; } }
            public static int LeftMenuSalesMatrixReports { get { return 1065; } }
            public static int LeftMenuLeadSourceReports { get { return 1067; } }
            public static int LeftMenuTechInventoryReports { get { return 1068; } }

            public static int QuickMenuMyCompanyFileTemplateSettings { get { return 1066; } }

            public static int LeftMenuOrders { get { return 1069; } }
            public static int LeftMenuMarketing { get { return 1070; } }
            public static int LeftMenuMenuManagement { get { return 1071; } }
            public static int LeftMenuOrdersReport { get { return 1072; } }
            public static int LeftMenuWebsite { get { return 1073; } }

            public static int QuickMenuMyCompanyLanguageSetup { get { return 1074; } }
            public static int QuickMenuMyCompanyAnnouncement { get { return 1075; } }

            public static int CreditGrade { get { return 1076; } }
            public static int UpsellsReport { get { return 1077; } }
            public static int LeftMenuBrinksReport { get { return 1078; } }
            public static int QuickMenuAggrementQuestion { get { return 1079; } }
            public static int LeftMenuAgingReport { get { return 1081; } }

            public static int LeftMenuReportUpload { get { return 1082; } }
            public static int LeftMenuCalendar { get { return 1083; } }
            public static int LeftMenuRoute { get { return 1084; } }
            public static int LeftMenuUser { get { return 1085; } }
            public static int QuickMenuManageUserHideEmployeeStatus { get { return 1086; } }
            public static int QuickMenuManageUserHideUserGroup { get { return 1087; } }
            public static int QuickMenuManageUserHideDownloadUserButton { get { return 1088; } }
            public static int QuickMenuMyCompanyManageUserShowRouteList { get { return 1089; } }
            public static int QuickMenuMyCompanyManageUserShowRoute { get { return 1091; } }
            public static int QuickMenuMyCompanyManageGroup { get { return 1092; } }
            public static int LeftMenuRMRReport { get { return 1093; } }
            public static int LeftMenuUccReport { get { return 1095; } }
            public static int LeftMenuHistoryReport { get { return 1096; } }
            public static int LeftMenuHRReports { get { return 1098; } }
            public static int QuickMenuTitleSetting { get { return 1100; } }
            public static int EquipmentFavouritePermission { get { return 7040; } }
            public static int ShowMyTicketsPermission { get { return 7041; } }
            public static int ShowNonCommission { get { return 7042; } }
            //public static int QuickMenuMyCompanyCreditScoreGrade { get { return 7043; } }
            //public static int LeftMenuInstallationTrackerReport { get { return 7044; } }
            //public static int LeftMenuCompletedInventoryReport { get { return 7045; } }
            //public static int LeftMenuCSRActivityReport { get { return 7046; } }
            public static int HudsonVendorAccountReport { get { return 7047; } }
            public static int JobCostingReport { get { return 7053; } }

            public static int TicketEmailSetup { get { return 7060; } }
            public static int QuickMenuMyCompanyManageTeam { get { return 1097; } }
            public static int ShowHideArticleBox { get { return 1123; } }
            public static int InventoryEdit { get { return 1124; } }


        }           //1001
        public static class LeadPermissions
        { /*2001*/
            public static int LeadCreate { get { return 2001; } }
            public static int LeadSetupAfterCreate { get { return 2002; } }
            public static int LeadVarify { get { return 2003; } }
            public static int LeadRequestTech { get { return 2004; } }
            public static int LeadDocumentCenter { get { return 2005; } }
            public static int LeadSetupPackageTab { get { return 2006; } }
            public static int LeadSetupEquipmentsTab { get { return 2007; } }
            public static int LeadSetupServiceTab { get { return 2008; } }
            public static int LeadSetupPaymentTab { get { return 2009; } }
            public static int LeadSetupEmergencyTab { get { return 2010; } }
            public static int LeadEdit { get { return 2011; } }
            public static int LeadDelete { get { return 2012; } }
            public static int LeadSearch { get { return 2013; } }
            public static int LeadFilter { get { return 2014; } }
            public static int LeadList { get { return 2015; } }
            public static int LeadCreateStatement { get { return 2016; } }
            public static int LeadEmailWithPrint { get { return 2017; } }
            //public static int ExportLeadList { get { return 2018; } } No longer in use
            public static int ImportLeads { get { return 2019; } }
            public static int LeadCogwheel { get { return 2020; } }
            public static int LeadCogwheelCustomizecolumn { get { return 2021; } }
            public static int LeadCogwheelCreateEmailTemplate { get { return 2022; } }
            public static int LeadCogwheelSourceSettings { get { return 2023; } }
            public static int LeadDetails { get { return 2024; } }
            public static int LeadDetailsPersonalInfo { get { return 2025; } }
            public static int LeadDetailsSetup { get { return 2026; } }
            public static int LeadDetailsVerifyInfo { get { return 2027; } }
            public static int LeadDetailsQA1 { get { return 2028; } }
            public static int LeadDetailsTechCall { get { return 2029; } }
            public static int LeadDetailsQA2 { get { return 2030; } }
            public static int LeadCreateReport { get { return 2031; } }
            public static int LeadNotesTab { get { return 2032; } }
            public static int LeadNotesAdd { get { return 2033; } }
            public static int LeadNotesEdit { get { return 2034; } }
            public static int LeadNotesDelete { get { return 2035; } }
            public static int LeadNotesList { get { return 2036; } }
            public static int LeadFollowUpTab { get { return 2037; } }
            public static int LeadFollowUpAdd { get { return 2038; } }
            public static int LeadFollowUpEdit { get { return 2039; } }
            public static int LeadFollowUpDelete { get { return 2040; } }
            public static int LeadFollowUpList { get { return 2041; } }
            public static int LeadEmailTab { get { return 2042; } }
            public static int SendEmailToTeam { get { return 2043; } }
            public static int SendEmailToLead { get { return 2044; } }
            public static int LeadTechScheduleTab { get { return 2045; } }
            public static int LeadTechScheduleAdd { get { return 2046; } }
            public static int LeadTechScheduleEdit { get { return 2047; } }
            public static int LeadTechScheduleDelete { get { return 2048; } }
            public static int LeadTechScheduleList { get { return 2049; } }
            public static int LeadEstimateDelete { get { return 2050; } }
            public static int LeadEstimateApprove { get { return 2051; } }
            public static int LeadFileDelete { get { return 2052; } }
            public static int LeadSmartSetup { get { return 2053; } }
            public static int LeadCreditScore { get { return 2054; } }
            public static int LeadCreditCheckReport { get { return 2055; } }
            public static int LeadCreditCheck { get { return 2056; } }
            public static int LeadSmartSetupConvertToSale { get { return 2058; } }
            public static int LeadBookingCountTab { get { return 2060; } }
            public static int SmartSetupEditServiceProductRate { get { return 2061; } }
            public static int SmartSetupEditServiceDiscountRate { get { return 2062; } }
            public static int LeadReportExport { get { return 2063; } }
            public static int ShowLeadTicket { get { return 2064; } }
            public static int LeadAddNoteSendNote { get { return 2065; } }
            public static int ShowSoldBy2 { get { return 9508; } }
            public static int ShowSoldBy3 { get { return 9509; } }
            public static int ShowInspectionCompany { get { return 9510; } }
            public static int FollowUpDateFilter { get { return 2068; } }
            public static int LeadEstimatesCountTab { get { return 2069; } }
            public static int LeadEstimatesAmountCountTab { get { return 2070; } }
            public static int AllowInvoicePaymentMethod { get { return 2071; } }
            public static int AllowCheckPaymentMethod { get { return 2072; } }
            public static int AllowCashPaymentMethod { get { return 2073; } }
            public static int AllowCustomContractTerm { get { return 2074; } }
            public static int LeadDetailsAdditionalInfoAction { get { return 2075; } }
            public static int SmartSetUpSaveReview { get { return 2076; } }
            public static int SmartSetUpRenewalTerm { get { return 2077; } }
            public static int SmartSetUpServicePackageEqp { get { return 2078; } }
            public static int ShowAllServiceInSmartSetup { get { return 2079; } }
            public static int HardCreditCheckEFX { get { return 2080; } }
            public static int SoftCreditCheckEFX { get { return 2081; } }
            public static int HardCreditCheckTU { get { return 2082; } }
            public static int SoftCreditCheckTU { get { return 2083; } }
            public static int LeadCreditScoreGrade { get { return 2084; } }
            public static int LeadGridSummary { get { return 2085; } }
            public static int LeadOverviewNoteAdd { get { return 2086; } }

            public static int SendEcontractToBrinks { get { return 2087; } }
            public static int NewLeadsFilter { get { return 2088; } }
            public static int LeadSourceType { get { return 2089; } }
            public static int LeadEstimateSaveAndReview { get { return 2092; } }
            public static int AllowFinancedPaymentMethod { get { return 2093; } }
            public static int ShowOriginalContractDate { get { return 2094; } }

            public static int LeadCreateV2 { get { return 2095; } }
            public static int QATab { get { return 2096; } }
            public static int ISPCApplyFinance { get { return 2097; } }
            public static int SmartSetUpIncludeEqpEdit { get { return 2098; } }

            public static int ShowNFTAccount { get { return 2099; } }

            public static int ShowAddNoteButton { get { return 2100; } }

            public static int ShowNoteArea { get { return 2101; } }

            public static int LeadFileDocumentManagement { get { return 2102; } }
            public static int PaymentMethodPromo { get { return 2103; } }

            public static int PowerPayApplyFinance { get { return 2104; } }

            public static int SendISPCContract { get { return 2105; } }

            public static int AddLeadLeadStatusEnabled { get { return 2106; } }

            public static int ShowLeadConvertToSale { get { return 2107; } }
            public static int ShowLeadMapDirection { get { return 2108; } }
            public static int ShowBookLead { get { return 2109; } }

        }           //2001
        public static class CustomerPermissions
        { /*3001*/
            public static int CustomerCreate { get { return 3001; } }
            public static int CustomerSummmery { get { return 3002; } }
            public static int CustomerVarify { get { return 3003; } }
            public static int CustomerRequestTech { get { return 3004; } }
            public static int CustomerFiles { get { return 3005; } }
            public static int CustomerSetupPackageTab { get { return 3006; } }
            public static int CustomerSetupEquipmentsTab { get { return 3007; } }
            public static int CustomerSetupServiceTab { get { return 3008; } }
            public static int CustomerSetupPaymentTab { get { return 3009; } }
            public static int CustomerSetupEmergencyTab { get { return 3010; } }
            public static int CustomerEdit { get { return 3011; } }
            public static int CustomerDelete { get { return 3012; } }
            public static int CustomerSearch { get { return 3013; } }
            public static int CustomerFilter { get { return 3014; } }
            public static int CustomerList { get { return 3015; } }
            public static int CustomerCreateStatement { get { return 3016; } }
            public static int CustomerPrintList { get { return 3017; } }
            public static int ExportCustomerList { get { return 3018; } }
            public static int ImportCustomers { get { return 3019; } }
            public static int CustomerCogwheel { get { return 3020; } }
            public static int CustomerCogwheelCustomizecolumn { get { return 3021; } }
            public static int CustomerCogwheelCreateEmailTemplate { get { return 3022; } }
            public static int CustomerCogwheelSourceSettings { get { return 3023; } }
            public static int CustomerDetails { get { return 3024; } }
            public static int CustomerDetailsPersonalInfo { get { return 3025; } }
            public static int CustomerDetailsPresentAddress { get { return 3026; } }
            public static int CustomerDetailsBilingInfo { get { return 3027; } }
            public static int CustomerDetailsAccountHistory { get { return 3028; } }
            public static int CustomerDetailsInvoiceHistory { get { return 3029; } }
            public static int CustomerDetailsAccountActivity { get { return 3030; } }
            public static int CustomerCreateReport { get { return 3031; } }
            public static int CustomerFollowUpTab { get { return 3032; } }
            public static int CustomerFollowUpAdd { get { return 3033; } }
            public static int CustomerFollowUpEdit { get { return 3034; } }
            public static int CustomerFollowUpDelete { get { return 3035; } }
            public static int CustomerFollowUpList { get { return 3036; } }
            public static int CustomerEmailTab { get { return 3037; } }
            public static int SendEmailToTeam { get { return 3038; } }
            public static int SendEmailToCustomer { get { return 3039; } }
            public static int CustomerTechScheduleTab { get { return 3040; } }
            public static int CustomerTechScheduCustomerd { get { return 3041; } }
            public static int CustomerTechScheduleEdit { get { return 3042; } }
            public static int CustomerTechScheduleDelete { get { return 3043; } }
            public static int CustomerTechScheduleList { get { return 3044; } }
            public static int CustomerInvoiceList { get { return 3045; } }
            public static int CustomerInvoiceAdd { get { return 3046; } }
            public static int CustomerInvoicePrint { get { return 3047; } }
            public static int CustomerInvoiceAddNote { get { return 3048; } }
            public static int CustomerInvoiceSave { get { return 3049; } }
            public static int CustomerInvoiceReceivePayment { get { return 3050; } }
            public static int CustomerInvoiceReceivePaymentSave { get { return 3051; } }
            public static int CustomerFundingList { get { return 3052; } }
            public static int CustomerWorkOrderList { get { return 3053; } }
            public static int CustomerWorkOrderAdd { get { return 3054; } }
            public static int CustomerServiceOrderList { get { return 3055; } }
            public static int CustomerServiceOrderAdd { get { return 3056; } }
            public static int CustomerFilesList { get { return 3057; } }
            public static int CustomerFilesAdd { get { return 3058; } }
            public static int CustomerNotesList { get { return 3059; } }
            public static int CustomerNotesTab { get { return 3060; } }
            public static int CustomerNotesAdd { get { return 3061; } }
            public static int CustomerNotesEdit { get { return 3062; } }
            public static int CustomerNotesDelete { get { return 3063; } }
            public static int CustomerScheduleTab { get { return 3064; } }
            public static int CustomerScheduleAdd { get { return 3065; } }
            public static int CustomerScheduleEdit { get { return 3066; } }
            public static int CustomerScheduleDelete { get { return 3067; } }
            public static int CustomerInvoiceDelete { get { return 3068; } }
            public static int CustomerEstimateDelete { get { return 3069; } }
            public static int CustomerFundingDelete { get { return 3070; } }
            public static int CustomerEstimateApprove { get { return 3071; } }
            public static int CustomerTicketDelete { get { return 3072; } }
            public static int CustomerTicketShowCost { get { return 3073; } }
            public static int CustomerTicketShowRetailPrice { get { return 3074; } }
            public static int CustomerCredential { get { return 3075; } }
            public static int CustomerFileDelete { get { return 3076; } }
            public static int CustomerSmartSetup { get { return 3077; } }
            public static int CustomerTicketShowDescription { get { return 3078; } }
            public static int CustomerTicketCommission { get { return 3079; } }
            public static int CustomerTicketCompletedDate { get { return 3080; } }
            public static int CustomerPayMethodFilter { get { return 3081; } }
            public static int TechFilter { get { return 3082; } }
            public static int PackageFilter { get { return 3083; } }
            public static int OverviewDeliveryDate { get { return 3084; } }
            public static int OverviewCredit { get { return 3085; } }
            public static int OverviewOpenInvoice { get { return 3086; } }
            public static int OverviewUnpaidInvoice { get { return 3087; } }
            public static int OverviewTotalRevenew { get { return 3088; } }
            public static int OverviewPickUpDate { get { return 3089; } }
            public static int CustomerCreditScore { get { return 3090; } }
            public static int CustomerCreditCheckReport { get { return 3091; } }
            public static int CustomerCreditCheck { get { return 3095; } }
            public static int CustomerTicketDispatch { get { return 3092; } }
            public static int CustomerTicketEquipmentSoldBy { get { return 3093; } }
            public static int CustomerTicketServiceSoldBy { get { return 3094; } }
            public static int ReceivePaymentCreditMemo { get { return 3096; } }
            public static int TicketDeletePermission { get { return 3097; } }
            public static int RescheduleTicket { get { return 3099; } }
            public static int CustomerInvoiceReceivePaymentShowRefNo { get { return 3100; } }
            public static int TicketNotifythecustomer { get { return 3101; } }
            public static int TicketShowEquipment { get { return 3102; } }
            public static int ShowTicketAttachService { get { return 3103; } }
            public static int ShowTicketLogActive { get { return 3104; } }
            public static int CreateLead { get { return 3105; } }
            public static int TransferCustomer { get { return 3106; } }
            public static int AlarmCustomerCommitment { get { return 3107; } }

            public static int CustomerAddNoteSendNote { get { return 3108; } }
            public static int CustomerInspection { get { return 3110; } }

            public static int TicketSurvey { get { return 3109; } }
            public static int FollowUpDateFilter { get { return 3111; } }

            public static int TicketSignature { get { return 1000000000; } }
            public static int CloseTicketPermission { get { return 20000000; } }
            public static int CompleteTicketPermission { get { return 20000001; } }
            public static int ShowPaymentProfileInformation { get { return 1000000011; } }
            public static int CustomerCreditRefund { get { return 3112; } }
            public static int CustomerNonConfirmingfeeUpdate { get { return 3113; } }
            public static int BranchFilter { get { return 3115; } }
            public static int CustomerTicketReason { get { return 3114; } }
            public static int CustomerTicketRugLocation { get { return 3116; } }
            public static int CustomerFunding { get { return 3117; } }
            public static int ThirdPartyApi { get { return 3118; } }
            public static int CancelCustomer { get { return 3119; } }
            public static int AddCustomerBillingFundingTab { get { return 3120; } }
            public static int CustomerInformationEdit { get { return 3121; } }

            public static int CustomerBarTotalRMRCount { get { return 3122; } }
            public static int CustomerBarEstimateAmount { get { return 3123; } }
            public static int CustomerBarDueAmount { get { return 3124; } }

            public static int TicketShowEstimate { get { return 3129; } }

            public static int HardCreditCheckEFX { get { return 3125; } }

            public static int SoftCreditCheckEFX { get { return 3126; } }
            public static int HardCreditCheckTU { get { return 3127; } }
            public static int SoftCreditCheckTU { get { return 3128; } }

            public static int ShowPaymentMathodInformation { get { return 3130; } }
            public static int ShowSSNFullNumber { get { return 3131; } }
            public static int ShowAllCustomerList { get { return 3132; } }
            public static int OverviewNumberOfOrder { get { return 3133; } }
            public static int OverviewAvgOrderSize { get { return 3134; } }
            public static int OverviewLoyaltypoints { get { return 3135; } }
            public static int ImportADSFile { get { return 3136; } }

            public static int ShowLeadDetailsACHDeleteButton { get { return 3137; } }
            public static int ShowLeadDetailsCreditCardDeleteButton { get { return 3138; } }
            public static int LeadDetailsLeadVerificationDeleteButton { get { return 3139; } }

            public static int CustomerImport { get { return 3140; } }

            public static int AddInvoiceAddtoCredit { get { return 3141; } }
            public static int AddInvoiceRefund { get { return 3142; } }

            public static int AddCustomerSalesDateEnabled { get { return 3143; } }
            public static int AddCustomerInstallDateEnabled { get { return 3144; } }
            public static int AddCustomerFundingDateEnabled { get { return 3145; } }
            public static int AddCustomerForteSubscriptionID { get { return 3146; } }
            public static int ShowBrinksTab { get { return 3147; } }

            public static int AddCustomerLeadSourceEditable { get { return 3148; } }

            public static int ShowAllPayroll { get { return 3149; } }

            public static int CloseAndOpenTicketPermission { get { return 3150; } }
            public static int TicketIsPrivate { get { return 3151; } }
            public static int CustomerLogTab { get { return 3152; } }
            public static int CustomerActivationFeelEdit { get { return 3153; } }
            public static int ActivateCustomerPermission { get { return 3154; } }
            public static int FollowupPinTopPermission { get { return 3155; } }
            public static int InvoicePrintPermission { get { return 3156; } }
            public static int InvoiceDownloadPermission { get { return 3157; } }
            public static int AddCustomerStartDateEnabled { get { return 3158; } }
            public static int AddCustomerCreditGradeEnabled { get { return 3159; } }
            public static int AddCustomerMonitoringFeeEnabled { get { return 3160; } }
            public static int AddCustomerBillAmountEnabled { get { return 3161; } }
            public static int AddCustomerContractTermEnabled { get { return 3162; } }
            public static int TicketReportDownLoadButton { get { return 3163; } }
            public static int SalesReportBookingDownLoadButton { get { return 3164; } }
            public static int CustomerFileDocumentManagement { get { return 3165; } }
            public static int EmergencyContactAddItem { get { return 3166; } }
            public static int ShowSalesPersonEmail { get { return 3167; } }
            public static int ShowSignatureEstimatePdf { get { return 3168; } }
            public static int CustomerTicketAgreement { get { return 3169; } }
            //public static int ShowSingleContractAgreement { get { return 3170; } }
            public static int CustomerFirstNameNotRequired { get { return 3171; } }
            public static int ShowCancellationAgreement { get { return 3172; } }
            public static int HideCellNoForIeatery { get { return 3173; } }
            public static int HideSecondaryPhoneForIeatery { get { return 3174; } }
            public static int HidePrimaryPhoneForIeatery { get { return 3175; } }
            public static int HideEmailAddressForIeatery { get { return 3176; } }
            public static int CustomerHeaderBlockForIeatery { get { return 3177; } }
            public static int CustomerNoteSendToGlobalMail { get { return 3178; } }
            public static int MapscoNoPermission { get { return 3179; } }
            public static int CustomerCreditScoreGrade { get { return 3180; } }
            public static int UnpaidInvoiceTab { get { return 3181; } }
            public static int PaidInvoiceTab { get { return 3182; } }
            public static int RolledOverInvoiceTab { get { return 3183; } }
            public static int ShowAllLeadList { get { return 3184; } }
            public static int CustomerOverviewNoteAdd { get { return 3185; } }
            public static int PaymentMethodOthers { get { return 3187; } }
            public static int ShowInvoiceMemo { get { return 3186; } }
            public static int InvoiceSendText { get { return 3188; } }
            public static int HomeOwnerVerification { get { return 3189; } }
            public static int EmailAddressVerification { get { return 3190; } }
            public static int PrimaryPhoneVerification { get { return 3191; } }
            public static int CellNoVerification { get { return 3192; } }
            public static int SecondaryPhoneVerification { get { return 3193; } }
            public static int CustomerBillingDisplay { get { return 3194; } }
            public static int CustomerEstimateSaveAndReview { get { return 3197; } }
            public static int CustomerEstimateClone { get { return 3199; } }
            public static int CustomerInvoiceClone { get { return 3200; } }
            public static int CustomerContractStartDate { get { return 3201; } }
            public static int CustomerRemainingContract { get { return 3202; } }
            public static int AddEstimateNote { get { return 3203; } }
            public static int GenerateProrate { get { return 3206; } }
            public static int ChangeVerbalPassword { get { return 3204; } }
            public static int ChangeEmergencyContact { get { return 3205; } }
            public static int InstalationTrackerReportDownLoadButton { get { return 3207; } }
            public static int CompletedInventoryReportDownLoadButton { get { return 3208; } }

            public static int TechnicianReportDownLoadButton { get { return 3209; } }

            public static int FollowupReport { get { return 3210; } }

            public static int ShowBrinksPaymentInfo { get { return 3211; } }
            public static int ResendCancellationFile { get { return 3212; } }
            public static int ShowUpfrontMonth { get { return 3213; } }
            public static int ShowCancellationRemainingBalance { get { return 3214; } }
            public static int JobNo { get { return 3215; } }

            public static int AlarmSettings { get { return 3216; } }

            public static int BrinksSettings { get { return 3217; } }

            public static int UccSettings { get { return 3218; } }

            public static int PaymentOptionCCEdit { get { return 3219; } }

            public static int PaymentOptionACHEdit { get { return 3220; } }
            public static int UseSecondaryContact { get { return 3221; } }
            public static int CustomerFundingCommission { get { return 3222; } }
            public static int BatchNumberEditable { get { return 3223; } }
            public static int MendatoryCheckBoxForBrinks { get { return 3224; } }
            public static int CreateAccountIfContractSigned { get { return 3225; } }
            public static int UCCSyncCustomer { get { return 3231; } }
            public static int AlarmSyncCustomer { get { return 3226; } }
            public static int BrinksSyncCustomer { get { return 3227; } }
            public static int UCCUnassociateCustomer { get { return 3228; } }
            public static int AlarmUnassociateCustomer { get { return 3229; } }
            public static int BrinksUnassociateCustomer { get { return 3230; } }

            public static int OnlyResendMailForEstimate { get { return 3232; } }
            public static int ShowNMCConnectTab { get { return 3233; } }
            public static int SendAttachmentsWithEstimate { get { return 3234; } }

            public static int ShowFileSmartAgreementDFW { get { return 3235; } }
            public static int EditSoldBy { get { return 3236; } }

            public static int ShowFileInstallationCompletionChecklist { get { return 3237; } }
            public static int ShowFileServiceCallCompletionChecklist { get { return 3238; } }

            public static int ShowCreatedDateOnAddReminder { get { return 3239; } }
            public static int ShowReminderCompletedDateOnAddReminder { get { return 3240; } }
            public static int CustomerEstimate { get { return 3241; } }

            public static int ShowFileCancellation { get { return 3242; } }
            public static int ShowTaskReply { get { return 3244; } }
            public static int ShowEstimatorCogwheel { get { return 3243; } }
            public static int AllRecordsReportTab { get { return 3245; } }
            public static int CustomerPackageActivationFee { get { return 3246; } }
            public static int OverviewTotalCollected { get { return 3247; } }
            public static int TaskReplyEditDelete { get { return 3248; } }

            public static int ShowEstimatorSendContract { get { return 3249; } }
            public static int ShowCustomerDetailAddresss { get { return 3250; } }
            public static int AddRouteButton { get { return 3251; } }
            public static int DeleteRouteButton { get { return 3252; } }

            public static int RMRTemplateSettingsShowEnable { get { return 3253; } }
            public static int ShowAvantgradTab { get { return 3254; } }
            public static int ShowISPCTab { get { return 3255; } }
            public static int CSNameSelected { get { return 3256; } }

            public static int ShowTicketBackwardButton { get { return 3257; } }
            public static int ShowTicketAttachInvoice { get { return 3258; } }
            public static int ShowTicketAttachFile { get { return 3259; } }

            public static int CustomerInfoShare { get { return 3260; } }

            public static int CustomerUnpaidInvoiceStatement { get { return 3261; } }
            public static int CustomerRMRInvoice { get { return 3262; } }
            public static int CustomerRMRHistory { get { return 3263; } }
            public static int CustomerRMRLog { get { return 3264; } }
            public static int ShowCustomerMapDirection { get { return 3265; } }
            public static int RMRCreditAdd { get { return 3266; } }
            public static int ShowCustomerCreditDeleteBtn { get { return 3267; } }
            public static int CustomerMessageAdd { get { return 3268; } }
            public static int ShowDescriptionMessageList { get { return 3269; } }

            public static int ShowEquipCategoryInEstimate { get { return 3270; } }
            public static int ShowNoteAndReminderTeam { get { return 3271; } }
            public static int HideLeadSourceAndStatus { get { return 3272; } }
            public static int EnableInvoiceStatusForRolledOver { get { return 3273; } }
            public static int InvoiceTicketSaveBtn { get { return 3274; } }
            public static int MarkClientorLeadasTestAccount { get { return 3275; } }
            public static int CustomerEstimatorsCreatePO { get { return 3276; } }
            public static int Taxchceckbox { get { return 3277; } }
            public static int ShowCancellationExpirationDays { get { return 3278; } }
            public static int CustomerEstimatorClone { get { return 3279; } }
            public static int MonthlyBatchNumberEditable { get { return 3280; } }
            public static int ContractValuationEditable { get { return 3281; } }
            public static int RMRTemplateEditDeleteIconShow { get { return 3282; } }
            public static int RMRTemplateRMRAuditTab { get { return 3283; } }
            public static int ShowEstimatorCoverPage { get { return 3284; } }
            public static int ShowEstimatorOverride { get { return 3285; } }
            public static int CustomerProductLineItemEdit { get { return 3286; } }
            public static int EditCost { get { return 3287; } }
            public static int EditPrice { get { return 3288; } }
            public static int RMRMarginTab { get { return 3289; } }

        }
        public static class MenuManagementPermissions
        {
            public static int MenusTab { get { return 16001; } }
            public static int CategoriesTab { get { return 16002; } }
            public static int ToppingsTab { get { return 16003; } }
            public static int SpecialsTab { get { return 16005; } }
            public static int ArchivedItemsTab { get { return 16006; } }
            public static int MenuDelete { get { return 16004; } }
            public static int CategoryDelete { get { return 16007; } }
            public static int ToppingDelete { get { return 16008; } }
            public static int AllMenuItemTab { get { return 16009; } }
        }
        public static class WebsitePermissions
        {
            public static int SiteConfigurationTab { get { return 17001; } }
            public static int LocationsTab { get { return 17002; } }
            public static int ContentTab { get { return 17003; } }
            public static int DevButton { get { return 17004; } }

        }
        public static class OrderPermissions
        {
            public static int OrdersTab { get { return 18001; } }
            public static int ReservationsTab { get { return 18002; } }
            public static int CateringsTab { get { return 18003; } }
        }
        public static class MarketingPermissions
        {
            public static int SMSTab { get { return 19001; } }
            public static int EmailTab { get { return 19002; } }
            public static int SocialMediaTab { get { return 19003; } }
            public static int PaidAdvertisementTab { get { return 19004; } }
            public static int SocialMediaBox { get { return 19005; } }
            public static int EmailBox { get { return 19006; } }
            public static int SMSBox { get { return 19007; } }
            public static int PaidAdvertisementBox { get { return 19008; } }
        }
        public static class UserMgmtPermissions
        {
            /*4001*/
            public static int UserList { get { return 4001; } }
            public static int AddUser { get { return 4002; } }
            public static int UserChangePermissionGroup { get { return 4003; } }
            public static int ManageUserGroup { get { return 4004; } }
            public static int ManageUserGroupAdd { get { return 4005; } }
            public static int ManageUserGroupAssignPermission { get { return 4006; } }
            public static int UserResendEmail { get { return 4007; } }
            public static int DeactivateUser { get { return 4008; } }
            public static int SaveUserInformation { get { return 4009; } }
            public static int DeleteUser { get { return 4010; } }
            public static int QuickMenuManageUserHideUserImportButton { get { return 4024; } }



            public static int DetailTab { get { return 4101; } }
            public static int HRTab { get { return 4102; } }
            public static int ManagementTab { get { return 4103; } }
            public static int DocumentTab { get { return 4104; } }
            public static int PermissionTab { get { return 4105; } }
            public static int ReviewTab { get { return 4106; } }

            public static int InformationBox { get { return 4201; } }
            public static int DetailBox { get { return 4202; } }
            public static int ProfileBox { get { return 4203; } }
            public static int PasswordBox { get { return 4204; } }
            public static int CalanderBox { get { return 4205; } }
            public static int ExpirationDateBox { get { return 4206; } }
            public static int BranchBox { get { return 4207; } }
            public static int IPListBox { get { return 4208; } }
            public static int LeadSourcesBox { get { return 4209; } }
            public static int UserSecondaryAddress { get { return 4210; } }
            public static int HideUserDetail { get { return 4211; } }


            public static int HireDate { get { return 4301; } }
            public static int JobTitle { get { return 4302; } }
            public static int SalesCommission { get { return 4303; } }
            public static int TechCommission { get { return 4304; } }
            public static int Season { get { return 4305; } }
            public static int LicenseNo { get { return 4306; } }
            public static int SuperVisor { get { return 4307; } }
            public static int BadgerUserId { get { return 4308; } }
            public static int HideNoAutoClockOut { get { return 4309; } }

            public static int WriteupDelete { get { return 4401; } }
            public static int OccuranceDelete { get { return 4501; } }
            public static int ShowUserPassword { get { return 4502; } }
            public static int CustomerViewOnly { get { return 4503; } }
            public static int BrinksDelearCredential { get { return 4504; } }
            public static int IsSalesMatrix { get { return 4505; } }
            public static int EmployeeAvailibilitySection { get { return 4512; } }
            public static int PTORemaining { get { return 4513; } }

            //public static int PrimaryAddressRequired { get { return 4513; } }
            //public static int EmployeeQualification { get { return 4514; } }

            //public static int ActivateUser{ get { return 4004; } }
            //public static int DeactivateUser{ get { return 4005; } }
            //public static int UserResendEmail{ get { return 4006; } }
            //public static int UserDetailTab { get { return 4007; } }
            //public static int UserDetailInformation{ get { return 4008; } }
            //public static int UserChangePassword{ get { return 4009; } }

            //public static int SaveUserInformation{ get { return 4011; } }
            //public static int DeleteUser         { get { return 4012; } }
            //public static int EditUserPermissions{ get { return 4013; } }
            //public static int HRDocTab   { get { return 4014; } }
            //public static int FindHrDoc  { get { return 4015; } }
            //public static int DeleteHrDoc{ get { return 4016; } }
            //public static int AddHrDoc   { get { return 4017; } }
            //public static int UserPermissionsTab { get { return 4018; } }
            //public static int UserNameChange { get { return 4019; } }
            //public static int PasswordChange { get { return 4020; } }

            //public static int ManageUserGroupEdit { get { return 4023; } }
            //public static int ManageUserGroupDelete { get { return 4023; } }

        }       //4001 

        public static class SalesPermissions
        {/*5001*/
            public static int AllSalesTab { get { return 5001; } }
            public static int AllSalesSummery { get { return 5002; } }
            public static int AllSalesPrint { get { return 5003; } }
            public static int AllSalesReceivePayment { get { return 5004; } }
            public static int SalesInvoiceTab { get { return 5005; } }
            public static int SalesProductsListTab { get { return 5006; } }
            public static int SalesEditProduct { get { return 5007; } }
            public static int SalesAccountReceivableTab { get { return 5008; } }
            public static int AutomaticRecurringBilling { get { return 5009; } }
            public static int AccountReceivable { get { return 5010; } }

            public static int SalesRecurringBillingNotSet { get { return 5011; } }
            public static int RecurringBillingInvoiceGenerateManually { get { return 5012; } }


        }          //5001
        public static class ExpensePermissions
        { /*6001*/
            public static int ExpenseBillingTab { get { return 6001; } }
            public static int ExpenseBillingSummary { get { return 6002; } }
            public static int ExpenseBillingMakePayment { get { return 6003; } }
            public static int ExpneseBillingAddBill { get { return 6004; } }
            public static int ExpenseBillingPrintCheque { get { return 6005; } }
            public static int ExpenseVendorsTab { get { return 6006; } }
            public static int ExpneseVendorsSummary { get { return 6007; } }
            public static int ExpenseVendorsCreateABill { get { return 6008; } }
            public static int ExpenseVendorsEdit { get { return 6009; } }
            public static int ExpneseVendorsAdd { get { return 6010; } }
            public static int ExpneseVendorsDetail { get { return 6011; } }
            public static int VendorsDetailEditVendor { get { return 6012; } }
            public static int VendorsDetailCreateABill { get { return 6013; } }
            public static int VendorsDetailSummary { get { return 6014; } }
            public static int VendorsDetailOpenPayment { get { return 6015; } }
            public static int VendorsDetailOpenBill { get { return 6016; } }
            public static int VendorsDetailMakePayment { get { return 6017; } }
            public static int VendorsDetailPrintCheque { get { return 6018; } }
            public static int ExpensePayrollTab { get { return 6019; } }
            public static int ExpenseAccountsPayableTab { get { return 6020; } }
            public static int AccountsPayableOpenBill { get { return 6021; } }
            public static int AccountsPayableMakePayment { get { return 6022; } }
            public static int SupplierBillListSearch { get { return 6023; } }
            public static int ExpenseVendorsImport { get { return 6024; } }
            public static int ExpenseVendorsDelete { get { return 6025; } }
        }        //6001
        public static class ReportsPermissions
        {/*7001*/
            //public static int LeadsReport { get { return 7001; } } //No longer in use
            public static int LeadsReportDownload { get { return 7002; } }
            public static int InvoiceReport { get { return 7003; } }
            public static int InvoiceReportDownload { get { return 7004; } }
            public static int EstimateReport { get { return 7005; } }
            public static int EstimateReportDownload { get { return 7006; } }
            public static int PaymentReceivedReport { get { return 7007; } }
            public static int PaymentReceivedReportDownload { get { return 7008; } }
            public static int BillsReport { get { return 7009; } }
            public static int BillsReportDownload { get { return 7010; } }
            public static int BillPaymentReport { get { return 7011; } }
            public static int CustomerReportCustomerTab { get { return 7012; } }
            public static int CustomerReportConvertedCustomerTab { get { return 7013; } }
            public static int CustomerReportDelinquentCustomerTab { get { return 7014; } }
            public static int CustomerReportTransferCustomerTab { get { return 7015; } }
            public static int CustomerReportInactiveCustomerTab { get { return 7027; } }

            public static int InventoryReportWarehouseInventoryTab { get { return 7016; } }
            public static int InventoryReportBadInventoryTab { get { return 7017; } }
            public static int InventoryReportTruckInventoryTab { get { return 7018; } }
            public static int InventoryReportTransferInventoryTab { get { return 7019; } }
            public static int InventoryReportSummaryTab { get { return 7020; } }

            public static int SalesReportSalesTab { get { return 7021; } }
            public static int SalesReportRecurringBillingTab { get { return 7022; } }
            public static int SalesReportSummaryTab { get { return 7023; } }
            public static int SalesReportInvoiceListTab { get { return 7024; } }
            public static int SalesReportCollectionTab { get { return 7025; } }
            public static int ShowAllUserUpsellsReport { get { return 7026; } }
            public static int ShowAllReportData { get { return 7028; } }
            public static int BrinksReportBrinksTab { get { return 7029; } }
            public static int BrinksReportSoldTab { get { return 7030; } }
            public static int InventoryReportPOReportTab { get { return 7031; } }
            public static int InventoryCountReportTab { get { return 7032; } }
            public static int InventoryUsagebyAccountTab { get { return 7033; } }
            public static int InventoryRMAReportTab { get { return 7034; } }
            public static int InventoryPurchaseOrderTab { get { return 7035; } }
            public static int CustomerReportRWSReportTab { get { return 7036; } }
            public static int CompletedJobInventoryReportTab { get { return 7037; } }
            public static int ServiceTrackerReportTab { get { return 7038; } }

            public static int CustomerReportFinanceDealsTab { get { return 7039; } }
            public static int PayrollTermSheetManagerTab { get { return 7048; } }

            public static int SalesMatrixDownloadButton { get { return 7049; } }

            public static int SalesReportNewSalesTab { get { return 7050; } }
            public static int TasksReportTab { get { return 7051; } }

            public static int AllTaskReportList { get { return 7052; } }
            public static int TechTruckInventoryReportTab { get { return 7054; } }
            public static int RecurringBillingReportTab { get { return 7055; } }

            public static int CustomerFinancedReport { get { return 7056; } }
            public static int TechUsedReportTab { get { return 7057; } }
            public static int TechOrderReportTab { get { return 7058; } }
            public static int MoveCustomerReportTab { get { return 7059; } }
            public static int InstalledDealsReport { get { return 7061; } }
            public static int EstimateReportListTab { get { return 7065; } }

            public static int TaxCollectionReport { get { return 7066; } }

            public static int CancellationQueueDocumentSigned { get { return 7067; } }
            public static int RMRInvoiceReport { get { return 7068; } }
            public static int RMRHistoryReport { get { return 7069; } }
            public static int RMRLogReport { get { return 7070; } }
            public static int CreditHistoryReport { get { return 7071; } }
            public static int SMSHistoryReport { get { return 7072; } }
            public static int SalesPersonReport { get { return 7073; } }
            public static int SalesReportVariableCostTab { get { return 7074; } }
            public static int SalesReportTechUpSalesTab { get { return 7075; } }
            public static int PayrollReportInsideCommissionTab { get { return 7076; } }
            public static int ServiceSalesReport { get { return 7077; } }
            public static int RMRACHReturnsReport { get { return 7078; } }
            public static int RMRCreditReport { get { return 7079; } }
            public static int NewSalesReportExport { get { return 7080; } }
            public static int NewSalesReport2Export { get { return 7081; } }
            public static int InventoryReportPOReportFinanceTab { get { return 7082; } }
            public static int InventoryReportPOReportInventoryTab { get { return 7083; } }
            public static int CustomerReportTestAccountTab { get { return 7084; } }
        }        //7001 
        public static class InventoryPermissions
        { /*8001*/
            public static int InventoryWareHouseTab { get { return 8001; } }
            public static int PurchaseOrderWareHouseTab { get { return 8002; } }
            public static int InventoryBranchTab { get { return 8003; } }
            public static int PurchaseOrderBranchTab { get { return 8004; } }
            public static int InventoryTechTab { get { return 8005; } }
            public static int PurchaseOrderTechTab { get { return 8006; } }
            public static int PurchaseOrderAddWareHouse { get { return 8007; } }
            public static int PurchaseOrderAddBranch { get { return 8008; } }
            public static int PurchaseOrderAddTech { get { return 8009; } }
            public static int InventroyFilter { get { return 8010; } }
            public static int TechAllInventory { get { return 8011; } }
            public static int TechOwnInventory { get { return 8012; } }
            public static int TechAllPurchaseOrder { get { return 8013; } }
            public static int TechOwnPurchaseOrder { get { return 8014; } }
            public static int BranchAllInventory { get { return 8015; } }
            public static int BranchOwnInventory { get { return 8016; } }
            public static int BranchAllPurchaseOrder { get { return 8017; } }
            public static int BranchOwnPurchaseOrder { get { return 8018; } }
            public static int InventorySummary { get { return 8018; } }
            public static int BadInventoryTab { get { return 8020; } }
            public static int InventorySetting { get { return 8021; } }
            public static int CategoryField { get { return 8022; } }
            public static int DemandOrderTab { get { return 8023; } }
            public static int TechReorderPoint { get { return 8024; } }
            public static int ShowInventoryTechReceiveTab { get { return 8025; } }
            public static int TechReceiveAction { get { return 8026; } }
            public static int InventoryTabMassPO { get { return 8027; } }
            public static int InventoryTabImport { get { return 8028; } }

            public static int InventoryTabDeleteEquipment { get { return 8029; } }
            public static int RequestOrderList { get { return 8030; } }
            public static int MassRestock { get { return 8031; } }
            public static int InventoryNewEquipment { get { return 8032; } }
            public static int InventoryIsTaxable { get { return 8033; } }
            public static int ShowOrderForInAddPurchaseOrder { get { return 8034; } }
            public static int PurchaseOrderAction { get { return 8035; } }
            public static int PurchaseOrderReceiveBy { get { return 8036; } }
            public static int ImportEquipmentForRichmond { get { return 8037; } }
            public static int PurchaseOrderCreate { get { return 8038; } }
            public static int ShowAllTechnicianPO { get { return 8039; } }
            public static int ShowEstimatorIdFilter { get { return 8040; } }
            public static int ShowVendorCost { get { return 8041; } }
            public static int InventoryListDownloadAndCustomizeColumn { get { return 8042; } }
            public static int EquipHistoryTab { get { return 8042; } }
            public static int ShowInventoryEquipHistoryTab { get { return 8043; } }
            public static int TechDropDownEditable { get { return 8043; } }
            public static int TechTransferApprove { get { return 8044; } }
            public static int ShowTransferLog { get { return 8045; } }
            public static int ShowWarehouseHistory { get { return 8046; } }
            public static int TechInvShowHideVendorCost { get { return 8047; } }
            public static int DevPermissions { get { return 8050; } }
            public static int InventoryTechTransferTab { get { return 8051; } }

        }      //8001
        public static class MyCompanyPermissions
        {/*My Company Permissions*/
            public static int CompanyAndSystemSettings { get { return 9001; } }
            public static int GlobalSettingsTab { get { return 9002; } }
            public static int GlobalSettingsEdit { get { return 9003; } }
            public static int AlarmComTab { get { return 9004; } }
            public static int AlarmComAdd { get { return 9005; } }
            public static int AlarmComEdit { get { return 9006; } }
            public static int AlarmComDelete { get { return 9007; } }
            public static int AuthorizeNetTab { get { return 9008; } }
            public static int AuthorizeNetAdd { get { return 9009; } }
            public static int AuthorizeNetEdit { get { return 9010; } }
            public static int AuthorizeNetDelete { get { return 9011; } }
            public static int Fees { get { return 9101; } }
            public static int ActivationFeeTab { get { return 9102; } }
            public static int ActivationFeeAdd { get { return 9103; } }
            public static int ActivationFeeEdit { get { return 9104; } }
            public static int ActivationFeeDelete { get { return 9105; } }
            public static int MMRFeeTab { get { return 9106; } }
            public static int MMRFeeAdd { get { return 9107; } }
            public static int MMRFeeEdit { get { return 9108; } }
            public static int MMRFeeDelete { get { return 9109; } }
            public static int ServiceFeeTab { get { return 9110; } }
            public static int ServiceFeeAdd { get { return 9111; } }
            public static int ServiceFeeEdit { get { return 9112; } }
            public static int ServiceFeeDelete { get { return 9113; } }
            public static int BranchSetting { get { return 9201; } }
            public static int BranchSettingEdit { get { return 9202; } }
            public static int BranchSettingChangeLogo { get { return 9203; } }
            public static int BranchSettingAdd { get { return 9204; } }
            public static int BrnachSettingDelete { get { return 9205; } }
            public static int SalesMatrix { get { return 9301; } }
            public static int SalesMatrixAdd { get { return 9302; } }
            public static int SalesMatrixEdit { get { return 9303; } }
            public static int SalesMatrixDelete { get { return 9304; } }
            //public static int InstallationMatrix { get { return 9401; } } No longer in use
            public static int IncomeAndExpense { get { return 9501; } }
            public static int TicketClone { get { return 9502; } }
            public static int TicketRecreateAgreement { get { return 9503; } }
            public static int TicketSmartSetup { get { return 9504; } }
            public static int TicketTimer { get { return 9505; } }
            public static int ShowInvoiceInfo { get { return 9506; } }
            public static int ShowContractSignedOrNot { get { return 9508; } }
            public static int ShowAllSalesPerson { get { return 9509; } }
            public static int ShowCreditScore { get { return 9511; } }
            public static int ShowCreditScoreGrade { get { return 9512; } }
            public static int TicketGenerateDocument { get { return 9513; } }
            public static int AddWorkToBePerformedAddendum { get { return 9514; } }

            public static int ShowContractSignedUpdateButton { get { return 9515; } }

            public static int ShowAgreementinfoPopup { get { return 9516; } }
            public static int FileManagementFileDelete { get { return 9517; } }
            public static int AddAlarmAccBasedOnContract { get { return 9518; } }
            public static int AddArmingOption { get { return 9519; } }
        }      //9001
        public static class ProductsPermissions
        {
            //public static int ProductCategories { get { return 10001; } } //No longer in use
            public static int ProductCategoriesAdd { get { return 10002; } }
            public static int ProductCategoriesEdit { get { return 10003; } }
            public static int ProductCategoriesDelete { get { return 10004; } }
            public static int ProductClass { get { return 10005; } }
            public static int ProductClassAdd { get { return 10006; } }
            public static int ProductClassEdit { get { return 10007; } }
            public static int ProductClassDelete { get { return 10008; } }
            public static int ProductsAndServices { get { return 10009; } }
            public static int ProductsAndServicesAdd { get { return 10010; } }
            public static int ProductsAndServicesEdit { get { return 10011; } }
            public static int ProductsAndServicesDelete { get { return 10012; } }
            //public static int CustomerPanelType { get { return 10013; } } //No longer in use
            public static int CustomerPanelTypeAdd { get { return 10014; } }
            public static int CustomerPanelTypeEdit { get { return 10015; } }
            public static int CustomerPanelTypeDelete { get { return 10016; } }

            //public static int Manufacturers { get { return 10017; } } // No longer in use
            public static int ManufacturersAdd { get { return 10018; } }
            public static int ManufacturersEdit { get { return 10019; } }
            public static int ManufacturersDelete { get { return 10020; } }
            public static int ProductCategoryDelete { get { return 10021; } }
        }       //10001
        public static class ToolsPermissions
        {
            public static int CredentialSettings { get { return 11001; } }
            public static int CredentialSettingsAdd { get { return 11002; } }
            public static int CredentialSettingsEdit { get { return 11003; } }
            public static int CredentialSettingsDelete { get { return 11004; } }
            public static int AccountHolder { get { return 11005; } }
            public static int AccountHolderAdd { get { return 11006; } }
            public static int AccountHolderEdit { get { return 11007; } }
            public static int AccountHolderDelete { get { return 11008; } }
            //public static int CustomerSystemNo { get { return 11009; } } No longer in use
            public static int CustomerSystemNoAdd { get { return 11010; } }
            public static int CustomerSystemNoEdit { get { return 11011; } }
            public static int CustomerSystemNoDelete { get { return 11012; } }
            //public static int PackageSetup { get { return 11013; } } //No longer in use
            public static int PackageSetupAdd { get { return 11014; } }
            public static int PackageSetupEdit { get { return 11015; } }
            public static int PackageSetupDelete { get { return 11016; } }
            public static int PackageSetupAddCSPrefix { get { return 11017; } }

        }          //11001

        public static class ReportMenuPermission
        {
            public static int PayrollTab { get { return 13001; } }
            public static int TimeClockTab { get { return 13002; } }
            public static int SalesTab { get { return 13003; } }
            public static int TechsTab { get { return 13004; } }
            public static int AddMemberTab { get { return 13005; } }
            public static int ServiceCallTab { get { return 13006; } }
            public static int FollowUpTab { get { return 13007; } }
            public static int RescheduleTab { get { return 13008; } }
            public static int FundingTab { get { return 13009; } }
            public static int UserPaymentTab { get { return 13010; } }

            public static int TimeClockTimeClockTab { get { return 13011; } }
            public static int TimeClockPTOTab { get { return 13012; } }
            public static int TimeClockEmployeesTimeClockTab { get { return 13013; } }
            public static int TimeClockEmployeesPTOTab { get { return 13014; } }
            public static int PaidCommissionTab { get { return 13015; } }
            public static int FundingBrinksTab { get { return 13016; } }
            public static int AccrualPTOTab { get { return 13017; } }

        }
        public static class ContactPermissions
        {
            public static int BadgerMap { get { return 14001; } }
        }
        public static class SchedulePermission
        {
            public static int ShowMapBtn { get { return 15001; } }
            public static int TicketStatusImageSetup { get { return 15002; } }
            public static int ShowAllUsersOwnCalendar { get { return 4000; } }
            public static int ScheduleCalendarPermission { get { return 15003; } }
            public static int ScheduleDateHeaderChange { get { return 15004; } }
            public static int PreviousDateSchedulePermission { get { return 15005; } }
            public static int ShowUsersHaveEventCalendar { get { return 15006; } }
            public static int CustomCalendarSettingsPermission { get { return 15007; } }
            public static int CustomCalendarDayOffPermission { get { return 15008; } }
            public static int CustomCalendarWeeklyButtonShowPermission { get { return 15009; } }
            public static int ShowSelectAllAndNoneTechnicianListInCalendar { get { return 15010; } }
            public static int EnableEventDragDropPermission { get { return 15011; } }
            public static int MapPointerEditPermission { get { return 15012; } }
            public static int DeactivateTicketShowButton { get { return 15013; } }
        }

        public static class CustomerTicketPermission
        {
            public static int CustomerTicketCreateInvoice { get { return 95008; } }
            public static int RugTicketReport { get { return 95009; } }
            public static int ShowAllTicket { get { return 95010; } }
            public static int AddSalesCommission { get { return 95011; } }
            public static int AddTechCommission { get { return 95012; } }
            public static int AddRescheduleCommission { get { return 95013; } }
            public static int AddFollowUpCommission { get { return 95014; } }
            public static int AddAddMemberCommission { get { return 95015; } }
            public static int AddServiceCallCommission { get { return 95016; } }
            public static int TicketScheduleOnPermission { get { return 95017; } }
            public static int TicketStartTimePermission { get { return 95018; } }
            public static int TicketEndTimePermission { get { return 95019; } }
            public static int TicketAssignedPermission { get { return 95020; } }
            public static int TicketAdditionalPermission { get { return 95021; } }
            public static int CreateTicketPermission { get { return 95022; } }
            public static int ExportTicketPermission { get { return 95023; } }
            public static int ChangeTicketStatusPermission { get { return 95024; } }
            public static int AgemniTicket { get { return 95025; } }
            public static int AppointmentDateReport { get { return 95026; } }
            public static int DateReferenceReport { get { return 95027; } }
            public static int GoBackReport { get { return 95028; } }
            public static int InstalledEquipmentReport { get { return 95029; } }
            public static int CodeSafetyDocument { get { return 95030; } }
            public static int SendAssignedEmail { get { return 95031; } }
            public static int CustomerTicketCreateDo { get { return 95032; } }
            public static int ShowAllCommission { get { return 95033; } }
            public static int TicketSummaryReport { get { return 95034; } }
            public static int TicketEquipmentReleaseButton { get { return 95035; } }
            public static int ShowLostTicket { get { return 95036; } }
            public static int FirstPagePermission { get { return 95037; } }
            public static int PRReportTab { get { return 95038; } }
            public static int SendTicketPDFEmail { get { return 95039; } }
            public static int CommissionAdjustment { get { return 95040; } }
            public static int ShowCommissionCalculation { get { return 95041; } }
            public static int CommissionDelete { get { return 95042; } }
            public static int ShowAssignToDetail { get { return 95043; } }
            public static int TicketFieldsHideDisablePermission { get { return 95044; } }
            public static int TicketBalanceDuePermission { get { return 95045; } }
            public static int CustomerTicketShowInstall { get { return 95046; } }
            public static int CustomerTicketShowPoint { get { return 95047; } }
            public static int EditAndShowDiffrentTicketAddress { get { return 95048; } }
            public static int InstallationTrackerReport { get { return 95049; } }
            public static int UserActivityReport { get { return 95050; } }
            public static int TechnicianReport { get { return 95051; } }
            public static int InstalledTicketReport { get { return 95052; } }
            public static int SoldBy { get { return 95053; } }
            public static int InstalledBy { get { return 95054; } }
            public static int AllEquipmentReport { get { return 95055; } }

            public static int ShowTicketEquipmentCost { get { return 95056; } }

            public static int ShowInvoiceEquipmentCost { get { return 95057; } }

            public static int ShowCommissionCalculationInTicket { get { return 95058; } }
            public static int EditReleasedEquipmentUnitPrice { get { return 95059; } }
            public static int TicketEquipmentUndoButton { get { return 95060; } }
            public static int TicketTypeChange { get { return 95061; } }
            public static int TicketCommercialAgreement { get { return 95062; } }

            public static int ShowCreateAddendum { get { return 95063; } }
            public static int ShowMapDirection { get { return 95064; } }
            public static int DisclaimerTextForThompson { get { return 95065; } }
            public static int DisableActionBtnatSmartSetup { get { return 95066; } }
            public static int RMRCommissionEdit { get { return 95067; } }
            public static int TicketAttachMISC { get { return 95068; } }
            public static int FinRepCommission { get { return 95069; } }
            public static int IsAgreementTicket { get { return 95070; } }
            public static int SendNotificationButton { get { return 95069; } }
        }

        public class TimeClockPtoPermission
        {
            public static int EmployeesTimeClockListAll { get { return 88801; } }
            public static int ShowAllEmployeeInEmployeeTimeClock { get { return 88803; } }
            public static int DeleteEmployeePto { get { return 88802; } }
            public static int EmployeesListShowForAddDayOff { get { return 88804; } }
        }

        public class IeateryPermission
        {
            public static int PaidRestaurantPermission { get { return 65501; } }
        }

        public static class InvoicePermissions
        {
            public static int InvoiceDetailsLineItemDiscountAmountShow { get { return 21001; } }
        }
        public static class RMRPermissions
        {
            public static int GroupRMRInvoiceEmailButtonShow { get { return 22001; } }
            public static int GroupRMRInvoicePrintButtonShow { get { return 22002; } }
            public static int GroupRMRStatementEmailButtonShow { get { return 22003; } }
            public static int GroupRMRStatementPrintButtonShow { get { return 22004; } }
            public static int CreatingRMRUnpaidInvoiceButtonShow { get { return 22005; } }
            public static int CollectPaymentUnpaidRMRButtonShow { get { return 22006; } }
            public static int DownloadUnpaidRMRInvoiceButtonShow { get { return 22007; } }
            public static int UploadInvoiceInformationForPaymentButtonShow { get { return 22008; } }
            public static int CustomerRMRGenerateReport { get { return 22009; } }
        }
    }
}