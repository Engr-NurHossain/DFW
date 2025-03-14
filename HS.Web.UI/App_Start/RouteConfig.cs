using HS.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HS.Web.UI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            string DomainPath = "";

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "DashBoard",
                url: DomainPath + "DashBoard",
                defaults: new { controller = "app", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Passwordreset",
                url: DomainPath + "passwordreset",
                defaults: new { controller = "Login", action = "PasswordReset", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "CustomerImgShow",
                url: DomainPath + "CustomerImgShow/W={W}/H={H}/CustomerId={CustomerId}/UserName={UserName}/CompanyId={CompanyId}/Image_Preview.jpg",
                defaults: new { controller = "Image", action = "CustomerImgShow", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "WebLocImageShow",
                url: DomainPath + "{STATE}/{CITY}/{NAME}/Image/W{W}/H{H}/{ID}/{CompanyId}/Image_Preview.png",
                defaults: new { controller = "Image", action = "WebLocImageShow", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "WebLocCoverImageShow",
                url: DomainPath + "{STATE}/{CITY}/{NAME}/CoverImage/W{W}/H{H}/{ID}/{CompanyId}/Image_Preview.png",
                defaults: new { controller = "Image", action = "WebLocCoverImageShow", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "CategoryImageShow",
                url: DomainPath + "{STATE}/{CITY}/{NAME}/Category/{CategoryName}/Image/W{W}/H{H}/{ID}/{CompanyId}/Image_Preview.png",
                defaults: new { controller = "Image", action = "CategoryImageShow", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "MenuItemImageShow",
                url: DomainPath + "{STATE}/{CITY}/{NAME}/Item/{ItemName}/Image/W{W}/H{H}/{ID}/{CompanyId}/Image_Preview.png",
                defaults: new { controller = "Image", action = "MenuItemImageShow", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "MenuImageShow",
                url: DomainPath + "{STATE}/{CITY}/{NAME}/Menu/{MenuName}/Image/W{W}/H{H}/{ID}/{CompanyId}/Image_Preview.png",
                defaults: new { controller = "Image", action = "MenuImageShow", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "EstimateCameraImgShow",
                url: DomainPath + "EstimateCameraImgShow/W={W}/H={H}/InvoiceId={InvoiceId}/ImageType={ImageType}/UserName={UserName}/CompanyId={CompanyId}/Image_Preview.jpg",
                defaults: new { controller = "Image", action = "EstimateCameraImgShow", id = UrlParameter.Optional }
            );
            routes.MapRoute(
             name: "PhotoRoute4",
             url: DomainPath + "EmpShow/W{W}H{H}X{X}/",
             defaults: new { controller = "Image", action = "EmpShow", id = UrlParameter.Optional }
           );
            routes.MapRoute(
              name: "PhotoRoute1",
              url: DomainPath + "EmpShow/W{W}H{H}X{EMP}D{Demo}/",
              defaults: new { controller = "Image", action = "EmpShow", id = UrlParameter.Optional }
            );
            routes.MapRoute(
              name: "PhotoRoute2",
              url: DomainPath + "EmpShow/W{W}H{H}/",
              defaults: new { controller = "Image", action = "EmpShow", id = UrlParameter.Optional }
            );
            routes.MapRoute(
              name: "PhotoRoute3",
              url: DomainPath + "TicketStatusImageShow/W{W}H{H}X{STATUS}/",
              defaults: new { controller = "Image", action = "TicketStatusImageShow", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "ScheduleInfo",
                url: DomainPath + "ScheduleInfo",
                defaults: new { controller = "Schedule", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "CalendarMap",
                url: DomainPath + "CalendarMap",
                defaults: new { controller = "Calendar", action = "LoadMap", id = UrlParameter.Optional }
            );            
            routes.MapRoute(
                name: "Calendar",
                url: DomainPath + "calendar",
                defaults: new { controller = "Calendar", action = "Index", id = UrlParameter.Optional }
            );
            //routes.MapRoute(
            //    name: "Create Restaurant",
            //    url: DomainPath + "signup-restaurant",
            //    defaults: new { controller = "CreateResturant", action = "CreateResturantPartial", id = UrlParameter.Optional }
            //);
            routes.MapRoute(
                name: "AdjusmentPartial",
                url: DomainPath + "Adjusment",
                defaults: new { controller = "Adjusment", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Disclousure",
                url: DomainPath + "Disclousure",
                defaults: new { controller = "Public", action = "DisclousureLeadAgreement", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "ErrorPage",
                url: DomainPath + "ErrorPage",
                defaults: new { controller = "Home", action = "ERRORPage" }
            );
            routes.MapRoute(
               name: "LeadEditInfo",
               url: DomainPath + "LeadsVerificationDetail/{id}",
               defaults: new { controller = "Leads", action = "SetupIndex", id = UrlParameter.Optional }
           );
            routes.MapRoute(
              name: "LeadEditInfov2",
              url: DomainPath + "LeadsInfo/{id}",
              defaults: new { controller = "Leads", action = "SetupIndex", id = UrlParameter.Optional }
          );
            routes.MapRoute(
               name: "CreateLead",
               url: DomainPath + "CreateLead/{LeadId}",
               defaults: new { controller = "Leads", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "LeadImportFromCMS",
               url: DomainPath + "LeadImportFromCMS",
               defaults: new { controller = "Leads", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
             name: "Thank You",
             url: DomainPath + "thank-you",
             defaults: new { controller = "Login", action = "CreateRestaurantGreetingsPartial", id = UrlParameter.Optional }
           );
            routes.MapRoute(
                name: "LeadEditInfoPartial",
                url: DomainPath + "Leads/LeadVerificationPartial/{id}",
                defaults: new { controller = "Leads", action = "LeadVerificationPartial", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "AggrementQstn",
                url: DomainPath + "AggrementQstn",
                defaults: new { controller = "SmartLeads", action = "SmartLeadSetupIndex", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "MyTicket",
                url: DomainPath + "MyTicket",
                defaults: new { controller = "Ticket", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "ShowIeateryTermsAndConditions",
                url: DomainPath + "about-us/terms-and-conditions",
                defaults: new { controller = "CreateResturant", action = "ShowIeateryTermsAndConditions", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "ShowIeateryPrivacyPolicy",
                url: DomainPath + "about-us/privacy",
                defaults: new { controller = "CreateResturant", action = "ShowIeateryPrivacyPolicy", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Leads-Agreement",
                url: DomainPath + "Leads-Agreement/{code}",
                defaults: new { controller = "Public", action = "LeadsAgreementDocument", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "File-Template",
                url: DomainPath + "File-Template/{code}",
                defaults: new { controller = "Public", action = "FileTemplateDocument", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Customer-Ticket",
                url: DomainPath + "Customer-Ticket/{code}",
                defaults: new { controller = "Public", action = "CustomerTicketAddendumDocument", id = UrlParameter.Optional }
            );
            routes.MapRoute(
              name: "Leads-Cancellation-Agreement",
              url: DomainPath + "Leads-Cancellation-Agreement/{code}",
              defaults: new { controller = "Public", action = "LeadsCancellationAgreementDocument", id = UrlParameter.Optional }
          );
            routes.MapRoute(
                name: "Customer-Estimator",
                url: DomainPath + "Customer-Estimator/{code}",
                defaults: new { controller = "Public", action = "EstimatorSign", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Leads-Estimate",
                url: DomainPath + "Leads-Estimate/{code}",
                defaults: new { controller = "Public", action = "LeadsEstimate", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Requisition-Order",
                url: DomainPath + "Requisition-Order/{code}",
                defaults: new { controller = "Public", action = "RequisitionOrder", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Customer-Invoice",
                url: DomainPath + "Customer-Invoice/{code}",
                defaults: new { controller = "Public", action = "CustomerInvoice", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Customer-Invoice-Statement",
                url: DomainPath + "Customer-Invoice-Statement/{code}",
                defaults: new { controller = "Public", action = "CustomerInvoiceStatement", id = UrlParameter.Optional }
            );
            routes.MapRoute(
             name: "Customer-Refer",
             url: DomainPath + "Customer-Refer/{code}",
             defaults: new { controller = "Public", action = "CustomerRefer", id = UrlParameter.Optional }
         );
            routes.MapRoute(
                name: "Supplier-PO",
                url: DomainPath + "Supplier-PO/{code}",
                defaults: new { controller = "Public", action = "SupplierPO", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "ShortUrlHandler",
               url: DomainPath + "shrt/{code}",
               defaults: new { controller = "Public", action = "ShortUrlHandler", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "Leads-Booking",
               url: "Leads-Booking/{code}",
               defaults: new { controller = "Public", action = "LeadsBooking", id = UrlParameter.Optional }
           );
            routes.MapRoute(
                name: "DocumentCenterInfo",
                url: DomainPath + "DocumentCenterInfo",
                defaults: new { controller = "Leads", action = "SetupIndex", id = UrlParameter.Optional }
            );
           
            routes.MapRoute(
                name: "MassRestock",
                url: DomainPath + "MassRestock",
                defaults: new { controller = "Inventory", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "TagManagement",
                url: DomainPath + "ManagerTag",
                defaults: new { controller = "App", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "MassPO",
               url: DomainPath + "MassPO",
               defaults: new { controller = "Inventory", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "TechReorderPoint",
               url: DomainPath + "TechReorderPoint",
               defaults: new { controller = "Inventory", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
                name: "LeadVerifyAndSetupDetail",
                url: DomainPath + "LeadVerifyAndSetupDetail",
                defaults: new { controller = "Leads", action = "SetupIndex", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Admintration",
                url: DomainPath + "Admintration",
                defaults: new { controller = "app", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "FeesInfo",
                url: DomainPath + "FeesInfo",
                defaults: new { controller = "Fees", action = "Index", id = UrlParameter.Optional }
            );
            
            routes.MapRoute(
               name: "CityTaxInfo",
               url: DomainPath + "CityTaxInfo",
               defaults: new { controller = "CityTax", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "CompanyBranchInfo",
               url: DomainPath + "CompanyBranchInfo",
               defaults: new { controller = "CompanyBranch", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "packagesettings",
               url: DomainPath + "packagesettings",
               defaults: new { controller = "Leads", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
                name: "surveysettings",
                url: DomainPath + "surveysettings",
                defaults: new { controller = "Survey", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "TicketEmailSettings",
                url: DomainPath + "TicketEmailSettings",
                defaults: new { controller = "Ticket", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
              name: "smartpackagesettings",
              url: DomainPath + "smartpackagesettings",
              defaults: new { controller = "SmartPackageSetup", action = "Index", id = UrlParameter.Optional }
          );
            routes.MapRoute(
               name: "FundingCompany",
               url: DomainPath + "FundingCompany",
               defaults: new { controller = "Funding", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "LeadVerifyInfo",
               url: DomainPath + "LeadVerifyInfo/{id}",
               defaults: new { controller = "Leads", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
              name: "LeadSetupInfo",
              url: DomainPath + "LeadSetup/{id}",
              defaults: new { controller = "Leads", action = "LeadSetupIndex", id = UrlParameter.Optional }
          );
            routes.MapRoute(
               name: "LeadSetupInfoPartial",
               url: DomainPath + "Leads/LeadSetupPartial/{id}",
               defaults: new { controller = "Leads", action = "LeadSetupPartial", id = UrlParameter.Optional }
           );
            routes.MapRoute(
              name: "SmartLeadSetupInfo",
              url: DomainPath + "SmartLeadSetup/{id}",
              defaults: new { controller = "SmartLeads", action = "SmartLeadSetupIndex", id = UrlParameter.Optional }
          );
            routes.MapRoute(
               name: "SmartLeadSetupInfoPartial",
               url: DomainPath + "SmartLeads/SmartLeadSetupPartial/{id}",
               defaults: new { controller = "SmartLeads", action = "SmartLeadSetupPartial", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "CustomerRoutePartial",
               url: DomainPath + "customer-route",
               defaults: new { controller = "Customer", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "CompanyInfo",
               url: DomainPath + "CompanyInfo",
               defaults: new { controller = "Company", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "TechScheduleSettingInfo",
               url: DomainPath + "TechScheduleSettingInfo",
               defaults: new { controller = "TechScheduleSetting", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "AccountHolderInfo",
               url: DomainPath + "AccountHolderInfo",
               defaults: new { controller = "AccountHolder", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "CredentialSettingInfo",
               url: DomainPath + "CredentialSettingInfo",
               defaults: new { controller = "CredentialSetting", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "ActivationFeeInfo",
               url: DomainPath + "ActivationFeeInfo",
               defaults: new { controller = "ActivationFee", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
             name: "Suppliers",
             url: DomainPath + "Suppliers",
             defaults: new { controller = "Supplier", action = "Index", id = UrlParameter.Optional }
         );
            routes.MapRoute(
                name: "Bill",
                url: DomainPath + "Bill",
                defaults: new { controller = "app", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Search",
                url: DomainPath + "Search",
                defaults: new { controller = "app", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "UserFileDownload",
               url: DomainPath + "UserFileDownload/{id}",
               defaults: new { controller = "File", action = "UserFileDownload", id = UrlParameter.Optional }
           );

            routes.MapRoute(
             name: "CompanyFileDownload",
             url: DomainPath + "CompanyFileDownload/{id}",
             defaults: new { controller = "Public", action = "DownloadFile", id = UrlParameter.Optional }
         );
            routes.MapRoute(
               name: "DocumentCenterDownload",
               url: DomainPath + "DocumentCenterDownload/{id}",
               defaults: new { controller = "File", action = "DocumentCenterDownload", id = UrlParameter.Optional }
           );
            routes.MapRoute(
              name: "Download",
              url: DomainPath + "Download/{id}",
              defaults: new { controller = "File", action = "Download", id = UrlParameter.Optional }
          );
            routes.MapRoute(
              name: "DownloadEqFile",
              url: DomainPath + "DownloadEqFile/{id}",
              defaults: new { controller = "File", action = "DownloadEqFile", id = UrlParameter.Optional }
          );
            #region Oportunity , Contacts and Activity
            routes.MapRoute(
                name: "Contacts",
                url: DomainPath + "Contacts",
                defaults: new { controller = "Contact", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Opportunities",
                url: DomainPath + "Opportunities",
                defaults: new { controller = "Opportunity", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "Activities",
               url: DomainPath + "Activities",
               defaults: new { controller = "Activity", action = "Index", id = UrlParameter.Optional }
           );
            #endregion
            #region TechTicket
            routes.MapRoute(
                name: "editticket",
                url: DomainPath + "edit-ticket",
                defaults: new { controller = "Ticket", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "TechTicket",
                url: DomainPath + "TechnesianTickets",
                defaults: new { controller = "Ticket", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
              name: "TechCloseTicket",
              url: DomainPath + "TechnesianCloseTickets",
              defaults: new { controller = "Ticket", action = "Index", id = UrlParameter.Optional }
          );
            routes.MapRoute(
           name: "TechOpenTicket",
           url: DomainPath + "TechnesianOpenTickets/{id}",
           defaults: new { controller = "Ticket", action = "Index", id = UrlParameter.Optional }
       );
            routes.MapRoute(
          name: "TechEquipmentList",
          url: DomainPath + "Tech/EquipmentList",
          defaults: new { controller = "App", action = "Index", id = UrlParameter.Optional }
      );


            routes.MapRoute(
         name: "TechUpsoldList",
         url: DomainPath + "Tech/UpsoldList/{id}",
         defaults: new { controller = "App", action = "Index", id = UrlParameter.Optional }
     );

            routes.MapRoute(
         name: "TechServiceList",
         url: DomainPath + "Tech/ServiceList",
         defaults: new { controller = "App", action = "Index", id = UrlParameter.Optional }
     );

            routes.MapRoute(
       name: "GoBackList",
       url: DomainPath + "GoBackList",
       defaults: new { controller = "App", action = "Index", id = UrlParameter.Optional }
   );

            #endregion
            #region Survey
            routes.MapRoute(
                name: "EmployeeSurvey",
                url: DomainPath + "EmployeeSurvey/{Id}",
                defaults: new { controller = "Survey", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "RunSurvey",
               url: DomainPath + "RunSurvey/{Id}",
               defaults: new { controller = "Survey", action = "Index", id = UrlParameter.Optional }
           );
            #endregion

            #region Customer 
            routes.MapRoute(
              name: "Customer",
              url: DomainPath + "Customer",
              defaults: new { controller = "Customer", action = "Index", id = UrlParameter.Optional }
          );
            routes.MapRoute(
                name: "Customerlist",
                url: DomainPath + "Customer-list",
                defaults: new { controller = "Customer", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
              name: "Customerdetails",
              url: DomainPath + "Customer/Customerdetail/{id}",
              defaults: new { controller = "Customer", action = "Index", id = UrlParameter.Optional }
          );
            #region Contact,Opportunity,Activity details
            routes.MapRoute(
            name: "Contactdetails",
            url: DomainPath + "Contact/ContactDetail/{id}",
            defaults: new { controller = "Contact", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
            name: "Activitydetails",
            url: DomainPath + "Activity/ActivityDetail/{id}",
            defaults: new { controller = "Activity", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
            name: "OpportunityDetails",
            url: DomainPath + "Opportunity/OpportunityDetail/{id}",
            defaults: new { controller = "Opportunity", action = "Index", id = UrlParameter.Optional }
            );
            #endregion
            routes.MapRoute(
              name: "SupplierDetails",
              url: DomainPath + "Supplier/SupplierDetail/{id}",
              defaults: new { controller = "Supplier", action = "Index", id = UrlParameter.Optional }
          );
            routes.MapRoute(
                name: "ServiceProduct",
                url: DomainPath + "ServiceProduct",
                defaults: new { controller = "Customer", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "ProductCategory",
                url: DomainPath + "ProductCategory",
                defaults: new { controller = "Customer", action = "Index", id = UrlParameter.Optional }
            );
            #endregion
            routes.MapRoute(
              name: "Order",
              url: DomainPath + "Order",
              defaults: new { controller = "Order", action = "Index", id = UrlParameter.Optional }
          );
            routes.MapRoute(
              name: "Review",
              url: DomainPath + "Review",
              defaults: new { controller = "Order", action = "Index", id = UrlParameter.Optional }
          );
            routes.MapRoute(
              name: "Reward",
              url: DomainPath + "Reward",
              defaults: new { controller = "Order", action = "Index", id = UrlParameter.Optional }
          );
            routes.MapRoute(
              name: "Marketing",
              url: DomainPath + "Marketing",
              defaults: new { controller = "Marketing", action = "Index", id = UrlParameter.Optional }
          );
            routes.MapRoute(
               "Admin_elmah",
               "elmah/log",
               new { action = "Index", controller = "ElmahLog", type = UrlParameter.Optional }
           );
            routes.MapRoute(
                name: "Check",
                url: DomainPath + "Check",
                defaults: new { controller = "app", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "TechSchedule",
                url: DomainPath + "TechSchedule",
                defaults: new { controller = "TechSchedule", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Reports",
                url: DomainPath + "Reports",
                defaults: new { controller = "Reports", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
              name: "leadsReports",
              url: DomainPath + "leadsReports",
              defaults: new { controller = "Reports", action = "Index", id = UrlParameter.Optional }
          );
            routes.MapRoute(
              name: "HudsonLeadsReport",
              url: DomainPath + "HudsonLeadsReport",
              defaults: new { controller = "Reports", action = "Index", id = UrlParameter.Optional }
          );
            routes.MapRoute(
              name: "ActivitiesReport",
              url: DomainPath + "ActivitiesReport",
              defaults: new { controller = "Reports", action = "Index", id = UrlParameter.Optional }
          );
            routes.MapRoute(
              name: "AccountsReport",
              url: DomainPath + "AccountsReport",
              defaults: new { controller = "Reports", action = "Index", id = UrlParameter.Optional }
          );
            routes.MapRoute(
              name: "OpportunitiesReport",
              url: DomainPath + "OpportunitiesReport",
              defaults: new { controller = "Reports", action = "Index", id = UrlParameter.Optional }
          );
            routes.MapRoute(
              name: "SoftBacklogReport",
              url: DomainPath + "SoftBacklogReport",
              defaults: new { controller = "Reports", action = "Index", id = UrlParameter.Optional }
          );
            routes.MapRoute(
              name: "HardBacklogReport",
              url: DomainPath + "HardBacklogReport",
              defaults: new { controller = "Reports", action = "Index", id = UrlParameter.Optional }
          );
            routes.MapRoute(
                name: "CustomerReports",
                url: DomainPath + "CustomerReports",
                defaults: new { controller = "Reports", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "LoadOrderReports",
                url: DomainPath + "LoadOrderReports",
                defaults: new { controller = "Order", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "InventoryReports",
                url: DomainPath + "InventoryReports",
                defaults: new { controller = "Reports", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
             name: "ExpenseReports",
             url: DomainPath + "ExpenseReports",
             defaults: new { controller = "Reports", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
            name: "PayrollReports",
            url: DomainPath + "PayrollReports",
            defaults: new { controller = "Reports", action = "Index", id = UrlParameter.Optional }
            );

          routes.MapRoute(
          name: "ReportUpload",
          url: DomainPath + "ReportUpload",
          defaults: new { controller = "Reports", action = "Index", id = UrlParameter.Optional }
          );

            routes.MapRoute(
           name: "TicketReports",
           url: DomainPath + "TicketReports",
           defaults: new { controller = "Reports", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
          name: "TechnicianReports",
           url: DomainPath + "TechnicianReports",
          defaults: new { controller = "Reports", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
           name: "InstallationTrackerReport",
           url: DomainPath + "InstallationTrackerReport",
           defaults: new { controller = "Reports", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
           name: "CSRActivityReport",
           url: DomainPath + "CSRActivityReport",
           defaults: new { controller = "Reports", action = "Index", id = UrlParameter.Optional }
           );

           // routes.MapRoute(
           //name: "CompletedInventoryReport",
           //url: DomainPath + "CompletedInventoryReport",
           //defaults: new { controller = "Reports", action = "Index", id = UrlParameter.Optional }
           //);
            routes.MapRoute(
           name: "CancellationCue",
           url: DomainPath + "CancellationCue",
           defaults: new { controller = "Reports", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
           name: "SalesMatrixReports",
           url: DomainPath + "SalesMatrixReports",
           defaults: new { controller = "Reports", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "HrReports",
               url: DomainPath + "hrreports",
               defaults: new { controller = "Reports", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "UpsellsReports",
               url: DomainPath + "UpsellsReports",
               defaults: new { controller = "Reports", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
              name: "estimates",
              url: DomainPath + "estimates",
              defaults: new { controller = "Reports", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
           name: "AgingReport",
           url: DomainPath + "AgingReport",
           defaults: new { controller = "Reports", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
           name: "SalesReports",
           url: DomainPath + "SalesReports",
           defaults: new { controller = "Reports", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
           name: "BrinksReport",
           url: DomainPath + "BrinksReport",
           defaults: new { controller = "Reports", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
            name: "UccReport",
            url: DomainPath + "UccReport",
            defaults: new { controller = "Reports", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
           name: "BookingSalesReports",
           url: DomainPath + "BookingSalesReport",
           defaults: new { controller = "Reports", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
            name: "JobsReports",
            url: DomainPath + "JobsReports",
            defaults: new { controller = "Reports", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
            name: "JobCostingReports",
            url: DomainPath + "JobCostingReports",
            defaults: new { controller = "Reports", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
            name: "RMRReports",
            url: DomainPath + "RMRReports",
            defaults: new { controller = "Reports", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
            name: "HistoryReports",
            url: DomainPath + "HistoryReports",
            defaults: new { controller = "Reports", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "ConversionReports",
                url: DomainPath + "ConversionReports",
                defaults: new { controller = "Reports", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "PartnerReports",
                url: DomainPath + "PartnerReports",
                defaults: new { controller = "Reports", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "LeadSourceReports",
                url: DomainPath + "LeadSourceReports",
                defaults: new { controller = "Reports", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "TechInventoryReports",
                url: DomainPath + "TechInventoryReports",
                defaults: new { controller = "Reports", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Notifications",
                url: DomainPath + "Notifications",
                defaults: new { controller = "Notification", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Company",
                url: DomainPath + "Company",
                defaults: new { controller = "app", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Banking",
                url: DomainPath + "Banking",
                defaults: new { controller = "app", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "ThirdPartyApi",
                url: DomainPath + "ThirdPartyApi",
                defaults: new { controller = "app", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "ReportSales",
                url: DomainPath + "ReportSales",
                defaults: new { controller = "app", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "ReportSchedule",
                url: DomainPath + "ReportSchedule",
                defaults: new { controller = "app", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "ReportEmployee",
                url: DomainPath + "ReportEmployee",
                defaults: new { controller = "app", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "ReportCustomer",
                url: DomainPath + "ReportCustomer",
                defaults: new { controller = "app", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "VendorCredit",
                url: DomainPath + "VendorCredit",
                defaults: new { controller = "app", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Expence",
                url: DomainPath + "Expence",
                defaults: new { controller = "app", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Configuration",
                url: DomainPath + "Configuration",
                defaults: new { controller = "app", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Inventory",
                url: DomainPath + "Inventory",
                defaults: new { controller = "app", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Panel",
                url: DomainPath + "Panel",
                defaults: new { controller = "Panel", action = "CustomerPanelPartial", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "MMRsInfo",
                url: DomainPath + "MMRsInfo",
                defaults: new { controller = "Setup", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "ProductClassInfo",
                url: DomainPath + "ProductClassInfo",
                defaults: new { controller = "Inventory", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "EquipmentDetails",
                url: DomainPath + "Inventory/EquipmentDetail/{id}",
                defaults: new { controller = "Inventory", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "ServiceInventory",
               url: DomainPath + "ServiceInventory",
               defaults: new { controller = "Inventory", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
                name: "ServiceDetails",
                url: DomainPath + "Inventory/ServiceDetail/{id}",
                defaults: new { controller = "Inventory", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "InstallCustomerSystemNo",
                url: DomainPath + "InstallCustomerSystemNo",
                defaults: new { controller = "Customer", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "InstallCustomerSystemPrefix",
                url: DomainPath + "InstallCustomerSystemPrefix",
                defaults: new { controller = "Customer", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "PanelTypeInfo",
                url: DomainPath + "PanelTypeInfo",
                defaults: new { controller = "Panel", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "uisetupdetails ",
                url: DomainPath + "uisetupdetails",
                defaults: new { controller = "Setup", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "SetupDetailsInfo",
                url: DomainPath + "SetupDetailsInfo",
                defaults: new { controller = "Setup", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "calendarsetup",
                url: DomainPath + "calendarsetup",
                defaults: new { controller = "Calendar", action = "CalendarSettings", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "Localization",
               url: DomainPath + "Localization",
               defaults: new { controller = "Setup", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
                name: "SetupDropdowns",
                url: DomainPath + "SetupDropdowns",
                defaults: new { controller = "Setup", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "BuildVersion",
                url: DomainPath + "BuildVersion",
                defaults: new { controller = "App", action = "Index", id = UrlParameter.Optional }
            );


            #region leadsController
            routes.MapRoute(
               name: "Lead",
               url: DomainPath + "Lead",
               defaults: new { controller = "Leads", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "Lead-list",
               url: DomainPath + "lead-list",
               defaults: new { controller = "Leads", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
                 name: "LeadsDetails",
                 url: DomainPath + "Lead/Leadsdetail/{id}",
                 defaults: new { controller = "Leads", action = "Index", id = UrlParameter.Optional }
             );

            #endregion

            routes.MapRoute(
               name: "Vendor",
               url: DomainPath + "Vendor",
               defaults: new { controller = "app", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "Sales",
               url: DomainPath + "Sales",
               defaults: new { controller = "Sales", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
              name: "Knowledgebase",
              url: DomainPath + "Knowledgebase",
              defaults: new { controller = "Sales", action = "Index", id = UrlParameter.Optional }
          );
            routes.MapRoute(
                name: "documentlibrary/Id",
                url: DomainPath + "documentlibrary/Id={Id}",
                defaults: new { controller = "Sales", action = "ShowDocumentLibraryArticle", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "knowledgebase/Id",
                url: DomainPath + "knowledgebase/Id={Id}",
                defaults: new { controller = "Sales", action = "ShowArticle", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Schedule",
                url: DomainPath + "Schedule",
                defaults: new { controller = "app", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "RMRSystemSetting",
                url: DomainPath + "rmrsystemsetting",
                defaults: new { controller = "RecurringBilling", action = "RMRSystemSetting", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "rmr",
                url: DomainPath + "rmr",
                defaults: new { controller = "RecurringBilling", action = "RMRReports", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "knowledgebasesettings",
                url: DomainPath + "knowledgebasesettings",
                defaults: new { controller = "Sales", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
              name: "AssignArticle",
              url: DomainPath + "assignarticles",
              defaults: new { controller = "Sales", action = "Index", id = UrlParameter.Optional }
          );
            routes.MapRoute(
              name: "AssignArticleReport",
              url: DomainPath + "assignarticle-reports",
              defaults: new { controller = "Reports", action = "Index", id = UrlParameter.Optional }
          );
            routes.MapRoute(
                name: "EmployeeSchedule",
                url: DomainPath + "EmployeeSchedule",
                defaults: new { controller = "app", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Employee",
                url: DomainPath + "Employee",
                defaults: new { controller = "app", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "TimeAttendance",
                url: DomainPath + "TimeAttendance",
                defaults: new { controller = "app", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Payroll",
                url: DomainPath + "Payroll",
                defaults: new { controller = "app", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Analytics",
                url: DomainPath + "Analytics",
                defaults: new { controller = "app", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Manufacturers",
                url: DomainPath + "Manufacturers",
                defaults: new { controller = "app", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Marchents",
                url: DomainPath + "Marchents",
                defaults: new { controller = "app", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Logout",
                url: DomainPath + "Logout",
                defaults: new { controller = "Login", action = "Logout", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "MenuManagement",
                url: DomainPath + "MenuManagement",
                defaults: new { controller = "MenuManagement", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Website",
                url: DomainPath + "title-settings",
                defaults: new { controller = "Website", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "UserMgmt",
                url: DomainPath + "UserMgmt",
                defaults: new { controller = "app", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "UserGroup",
               url: DomainPath + "UserGroup",
               defaults: new { controller = "app", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "UserTeam",
               url: DomainPath + "UserTeam",
               defaults: new { controller = "app", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
                name: "CompanyBranch",
                url: DomainPath + "CompanyBranch",
                defaults: new { controller = "CompanyBranch", action = "CompanyBranchPartial", id = UrlParameter.Optional }
            );
            #region Setup
            routes.MapRoute(
              name: "Settings",
              url: DomainPath + "Settings",
              defaults: new { controller = "Setup", action = "Index", id = UrlParameter.Optional }
          );
            routes.MapRoute(
              name: "GridSettings",
              url: DomainPath + "GridSettings",
              defaults: new { controller = "Setup", action = "Index", id = UrlParameter.Optional }
          );


            #endregion

            #region UserMgmt
            routes.MapRoute(
                name: "UserProfile",
                url: DomainPath + "UserProfile",
                defaults: new { controller = "UserMgmt", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "UserInformation",
                url: DomainPath + "UserInformation/{Id}",
                defaults: new { controller = "UserMgmt", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "UserInformationRecruit",
                url: DomainPath + "UserInformationRecruit/{id}",
                defaults: new { controller = "UserMgmt", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "ForgotPassword",
               url: DomainPath + "ForgotPassword",
               defaults: new { controller = "Login", action = "ForgotPassword", id = UrlParameter.Optional }
           );
            #endregion

            #region Matrix
            routes.MapRoute(
                name: "MatrixPartial",
                url: DomainPath + "Matrix",
                defaults: new { controller = "Matrix", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "InstallMatrix",
                url: DomainPath + "InstallMatrix",
                defaults: new { controller = "Matrix", action = "Index", id = UrlParameter.Optional }
            );

            #endregion
            #region ReportsController
            routes.MapRoute(
                 name: "Invoice",
                 url: DomainPath + "Invoice/{CompanyId}/{CustomerId}/{InvoiceId}",
                 defaults: new { controller = "Login", action = "AccountVerification", id = UrlParameter.Optional }
             );

            #endregion

            routes.MapRoute(
                 name: "DefaultAccountVerification",
                 url: DomainPath + "AccountVerification/{verifysalt}/{userId}",
                 defaults: new { controller = "Login", action = "AccountVerification", id = UrlParameter.Optional } 
             );
            routes.MapRoute(
                 name: "ResetPassword",
                 url: DomainPath + "ResetPass/{verifysalt}/{userId}",
                 defaults: new { controller = "Login", action = "ResetPass", id = UrlParameter.Optional }
             );
            routes.MapRoute(
                 name: "Recruit",
                 url: DomainPath + "Recruitment",
                 defaults: new { controller = "Recruit", action = "Index", id = UrlParameter.Optional }
             );
            routes.MapRoute(
                name: "W9Form",
                url: DomainPath + "Recruitment/W9Form/",/*{IsPdf}*/
                defaults: new { controller = "Recruit", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "W4Form",
               url: DomainPath + "Recruitment/W4Form/",/*{IsPdf}*/
               defaults: new { controller = "Recruit", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "I9Form",
               url: DomainPath + "Recruitment/I9Form/",/*{IsPdf}*/
               defaults: new { controller = "Recruit", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "DrivingLicnese",
               url: DomainPath + "Recruitment/DrivingLicnese",/*{IsPdf}*/
               defaults: new { controller = "Recruit", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "StateLicTx",
               url: DomainPath + "Recruitment/StateLicTx",/*{IsPdf}*/
               defaults: new { controller = "Recruit", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
             name: "RestrictedZipCode",
             url: DomainPath + "RestrictedZipCode",/*{IsPdf}*/
             defaults: new { controller = "CityTax", action = "Index", id = UrlParameter.Optional }
         );

         
            #region TimeClock
            routes.MapRoute(
                name: "TimeClock",
                url: DomainPath + "TimeClock",
                defaults: new { controller = "TimeClockPto", action = "Index", id = UrlParameter.Optional }
            );
            #endregion

            #region VehicleManagement
            routes.MapRoute(
                name: "VehicleManagement",
                url: DomainPath + "VehicleManagement",
                defaults: new { controller = "VehicleManagement", action = "Index", id = UrlParameter.Optional }
            );
            #endregion
            #region Announcement
            routes.MapRoute(
                name: "CustomerAnnouncement",
                url: DomainPath + "CustomerAnnouncement",
                defaults: new { controller = "Customer", action = "AnnouncementIndex", id = UrlParameter.Optional }
            );
            #endregion
            #region ServiceAreaZipCode
            routes.MapRoute(
                name: "ServiceAreaZipCode",
                url: DomainPath + "ServiceAreaZipCode",
                defaults: new { controller = "ServiceArea", action = "Index", id = UrlParameter.Optional }
            );
            #endregion

            #region EmailTemplating
            routes.MapRoute(
               name: "Templates",
               url: DomainPath + "Templates",/*{IsPdf}*/
               defaults: new { controller = "Email", action = "Index", id = UrlParameter.Optional }
           );
            #endregion

            #region CreditGrades
            routes.MapRoute(
               name: "CreditGrades",
               url: DomainPath + "CreditGrades",/*{IsPdf}*/
               defaults: new { controller = "Customer", action = "Index", id = UrlParameter.Optional }
           );
            #endregion

            routes.MapRoute(
                 name: "Error404",
                 url: DomainPath + "Error404",
                 defaults: new { controller = "App", action = "Error", id = UrlParameter.Optional }
             );
            routes.MapRoute(
                name: "Default",
                url:  "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
