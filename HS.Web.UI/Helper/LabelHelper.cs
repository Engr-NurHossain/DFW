using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using HS.Framework.Utils;
using HS.Entities;
using System.Web.Mvc;
using HS.Facade;
using HS.Framework;
using System.Globalization;
using System.Text.RegularExpressions;

namespace HS.Web.UI.Helper
{
    public class LabelHelper
    {
        public static string ParserHelper(string BodyContent, Hashtable TemplateValue)
        {
            EmailParser ParserHelper = new EmailParser(BodyContent, TemplateValue, false); 
            return ParserHelper.Parse();
        }
        public static class SystemUser
        {
            public static string ID { get { return "22222222-2222-2222-2222-222222222222"; } }
        }
        public static class CustomerConvertType
        {
            public static string ManuallyConverted { get { return "Manually Converted"; } }
        }
        public static class PublicOpenTypes
        {
            public static string Ticket { get { return "Ticket"; } }
            public static string Survey { get { return "Survey"; } }
            public static string CustomSurvey { get { return "CustomSurvey"; } }
        }
        public static class UserType
        {
            public static string Customer { get { return "Customer"; } }
            public static string Lead { get { return "Lead"; } }
            public static string Employee { get { return "Employee"; } }
            public static string Vendor { get { return "Vendor"; } }
            public static string Opportunity { get { return "Opportunity"; } }
        }
        public static class FortePaymentStatus
        {
            public static string Authorized { get { return "Authorized"; } }
            public static string Complete { get { return "Complete"; } }
            public static string Declined { get { return "Declined"; } }
            public static string Failed { get { return "Failed"; } }
            public static string Funded { get { return "Funded"; } }
            public static string Ready { get { return "Ready"; } }
            public static string Rejected { get { return "Rejected"; } }
            public static string Review { get { return "Review"; } }
            public static string Settled { get { return "Settled"; } }
            public static string Settling { get { return "Settling"; } }
            public static string Unfunded { get { return "Unfunded"; } }
            public static string Voided { get { return "Voided"; } }
        }
        public static class CardType
        {
            public static string Visa { get { return "Visa"; } }
            public static string MasterCard { get { return "MasterCard"; } }
            public static string AmericanExpress { get { return "AmericanExpress"; } }
            public static string Discover { get { return "Discover"; } }
            public static string JCB { get { return "JCB"; } }
            public static string Others { get { return "Others"; } }
        }

        public static class CustomSurveyStatus
        {
            public static string Created { get { return "Created"; } }
            public static string Opened { get { return "Opened"; } }
            public static string Submitted { get { return "Submitted"; } }
        }

        public static class CustomerCreditType
        {
            public static string Credit { get { return "Credit"; } }
            public static string Debit { get { return "Debit"; } }
        }
        public static class CustomerAddress
        {
            public static string BookingBillingAddress { get { return "BookingBillingAddress"; } }
            public static string BookingPickupAddress { get { return "BookingPickupAddress"; } }
            public static string BookingDropOffAddress { get { return "BookingDropOffAddress"; } }
        }

        public static class PaymentGetway
        {
            public static string Authorize { get { return "Authorize.Net"; } }
            public static string Forte { get { return "Forte"; } }
        }
        public static class CustomSurveyQuestionTypes
        {
            public static string CheckBox { get { return "CheckBox"; } }
            public static string Radio { get { return "Radio"; } }
            public static string DropDown { get { return "DropDown"; } }
            public static string Text { get { return "Text"; } }
            public static string TextArea { get { return "TextArea"; } }
            public static string Signature { get { return "Signature"; } }
            public static string CustomerNumber { get { return "CustomerNumber"; } }
            public static string CustomerName { get { return "CustomerName"; } }
            public static string AlarmComFooter { get { return "AlarmComFooter"; } }
        }
        public static class TransactionExpenseType
        {
            public static string Automated { get { return "Automated"; } }
            public static string Manual { get { return "Manual"; } }
        }
        public static class ActivityAction
        {
            public static string PageLoad { get { return "Page Load"; } }
            public static string PageSubmit { get { return "Submit"; } }
            public static string Click { get { return "Click"; } }
            public static string Delete { get { return "Delete"; } }
            public static string Create { get { return "Create"; } }
            public static string Update { get { return "Update"; } }
            public static string Failed { get { return "Failed"; } }
            public static string Success { get { return "Success"; } }
            public static string AddCustomer { get { return "AddCustomer"; } }
            public static string UpdateCustomer { get { return "UpdateCustomer"; } }
            public static string AddNote { get { return "AddNote"; } }
            public static string UpdateNote { get { return "UpdateNote"; } }
            public static string AddAdditionalContact { get { return "AddAdditionalContact"; } }
            public static string UpdateAdditionalContact { get { return "UpdateAdditionalContact"; } }
            public static string AddOpportunity { get { return "AddOpportunity"; } }
            public static string UpdateOpportunity { get { return "UpdateOpportunity"; } }
            public static string AddCredential { get { return "AddCredential"; } }
            public static string UpdateCredential { get { return "UpdateCredential"; } }
            public static string AddTicket { get { return "AddTicket"; } }
            public static string UpdateTicket { get { return "UpdateTicket"; } }
            public static string AddEstimate { get { return "AddEstimate"; } }
            public static string UpdateEstimate { get { return "UpdateEstimate"; } }
            public static string AddInvoice { get { return "AddInvoice"; } }
            public static string UpdateInvoice { get { return "UpdateInvoice"; } }
            public static string SendInvoice { get { return "SendInvoice"; } }
            public static string AddFile { get { return "AddFile"; } }
            public static string MailSend { get { return "MailSend"; } }
            public static string UpdateAlarm_com { get { return "UpdateAlarm.com"; } }
            public static string AddAlarm_com { get { return "AddAlarm.com"; } }

            public static string Updateucc { get { return "Updateucc"; } }
            public static string Terminate { get { return "Terminate"; } }

            public static string Transfer { get { return "Transfer"; } }
            public static string Move { get { return "Move"; } }

            public static string UpdateDocumentFileManagement { get { return "UpdateDocumentFileManagement"; } }
            public static string AddDocumentFileManagement { get { return "AddDocumentFileManagement"; } }
            public static string AddRemineder { get { return "AddReminder"; } }
            public static string RefundOrCredit { get { return "RefundOrCredit"; } }
            public static string UpdateRemineder { get { return "UpdateReminder"; } }
            public static string AddReminderReply { get { return "AddReminderReply"; } }
            public static string UpdateReminderReply { get { return "UpdateReminderReply"; } }
            public static string CapturePayment { get { return "CapturePayment"; } }
            public static string PaymentReceived { get { return "PaymentReceived"; } }

            public static string AddPayment { get { return "AddPayment"; } }
            public static string TicketNotification { get { return "TicketNotification"; } }
            public static string Cloned { get { return "Cloned"; } }
        }

        public static class InvoiceStatus
        {
            public static string Init { get { return "Init"; } } // Initial Not valid
            public static string Open { get { return "Open"; } } // Not paid
            public static string Due { get { return "Due"; } } // Not paid
            public static string Paid { get { return "Paid"; } }
            public static string Credited { get { return "Credited"; } }
            public static string RolledOver { get { return "Rolled Over"; } }
            public static string Partial { get { return "Partial"; } } //Partilly paid
            public static string Cancelled { get { return "Cancelled"; } } //
            public static string Declined { get { return "Declined"; } } //Declined by authorize.net
            //public static string Refunded { get { return "Refunded"; } }
            public static string OnHold { get { return "OnHold"; } } // If balance is not same ase payment
            public static string Excess { get { return "Excess"; } }
            public static string Verify { get { return "Verify"; } }
        }
        public static class InvoiceNumberForARB
        {
            public static string MonitoringFee { get { return "MonitoringFee"; } }
        }
        public static class PaymentMethod //lookup datakey ='PaymentMethod'
        {
            public static string ACH { get { return "ACH"; } }
            public static string Check { get { return "Check"; } }
            public static string CreditCard { get { return "Credit Card"; } }
            public static string DebitCard { get { return "Debit Card"; } }
            public static string Cash { get { return "Cash"; } }
            public static string GeneralCustomerCredit { get { return "General Customer Credit"; } }
            public static string Invoice { get { return "Invoice"; } }
            public static string Financed { get { return "Financed"; } }
            public static string GetwayExisting { get { return "GetwayExisting"; } }
            public static string Others { get { return "Others"; } }
            public static string CustomerProfile { get { return "CustomerProfile"; } }
            public static string CustomerCredit { get { return "CustomerCredit"; } }
            public static string OnFile { get { return "OnFile"; } }
            public static string Promo { get { return "Promo"; } }
        }
        public static class InvoiceFor
        {
            public static string ACH { get { return "ACH"; } } // If Transaction payment method = ach
            public static string Check { get { return "Check"; } }
            public static string CreditCard { get { return "Credit Card"; } }// If Transaction payment method = Credit card
            public static string DebitCard { get { return "Debit Card"; } }
            public static string Cash { get { return "Cash"; } }
            public static string Others { get { return "Others"; } }//Usually not using this one
            public static string Invoice { get { return "Invoice"; } }//Manually generated invoice 
            public static string SystemGenerated { get { return "SystemGenerated"; } }//Other than ach/credit card and also
            public static string SystemGeneratedInvoice { get { return "RecurringInvoice"; } }//Other than ach/credit card and also
            public static string ActivationNonConforming { get { return "ActivationNonConforming"; } }
            public static string LaborFee { get { return "LaborFee"; } }
            public static string Service { get { return "Service"; } }
            public static string OnetimeService { get { return "OnetimeService"; } }
            public static string Equipment { get { return "Equipment"; } }
        }
        public static class UserTypes
        {
            public static string SysAdmin { get { return "SysAdmin"; } }
            public static string Admin { get { return "Admin"; } }
            public static string SalesManager { get { return "Sales Manager"; } }
            public static string SalesPerson { get { return "Sales Person"; } }
            public static string ServiceManager { get { return "Service Manager"; } }
            public static string CertifiedAffiliate { get { return "Certified Affiliate"; } }
            public static string Regional { get { return "Regional"; } }
            public static string SalesRep { get { return "Sales Rep"; } }
            public static string ServicePerson { get { return "Service Person"; } }
            public static string QAManager { get { return "QA Manager"; } }
            public static string QA { get { return "QA"; } }
            public static string Installer { get { return "Installer"; } }
            public static string Technician { get { return "Technician"; } }
            public static string Installation { get { return "Installation"; } }
            public static string Custom { get { return "Custom"; } }
            public static string IeateryAdmin { get { return "IeateryAdmin"; } }
        }
        public static class UserTags
        {
            public static string Technicians { get { return "Technician"; } }
            public static string QA { get { return "QA"; } }
            public static string SalesPerson { get { return "SalesPerson"; } }
            public static string ServicePerson { get { return "ServicePerson"; } }
            public static string Installer { get { return "Installer"; } }
            public static string Admin { get { return "Admin"; } }
            public static string HRManager { get { return "HRManager"; } }
            public static string Recruit { get { return "Recruit"; } }
            public static string Customer { get { return "Customer"; } }
            public static string Partner { get { return "Partner"; } }
            public static string ShowAllSchedules { get { return "ShowAllSchedules"; } }
        }
        public static class BillCycle
        {
            public static string Daily { get { return "Daily"; } }
            public static string Weekly { get { return "Weekly"; } }
            public static string Bi_Weekly { get { return "Bi-Weekly"; } }
            public static string Monthly { get { return "Monthly"; } }//1
            public static string Semi_Monthly { get { return "Semi-Monthly"; } }
            public static string Bi_Monthly { get { return "Bi-Monthly"; } }//2
            public static string Quarterly { get { return "Quarterly"; } }
            public static string SemiAnnual { get { return "Semi-Annual"; } } 
            public static string SemiAnnually { get { return "Semi-Annually"; } }//Forte
            public static string Annual { get { return "Annual"; } }
            public static string Annually { get { return "Annually"; } }//Forte
        }
        public static class CustomerAgreementHistory
        {
            public static string AgreementCreate { get { return "AgreementCreate"; } }
            public static string AgreementSend { get { return "AgreementSend"; } }
            public static string AgreementSign { get { return "AgreementSign"; } }
            public static string AgreementComplete { get { return "AgreementComplete"; } }
        }
        public static class CustomerAgreementLog
        {
            public static string LoadAgreement { get { return "LoadAgreement"; } }
            public static string SignAgreement { get { return "SignAgreement"; } }
            public static string SubmitAgreement { get { return "SubmitAgreement"; } }
            public static string LoadEstimate { get { return "LoadEstimate"; } }
            public static string SignEstimate { get { return "SignEstimate"; } }
            public static string SubmitEstimate { get { return "SubmitEstimate"; } }
            public static string LoadBooking { get { return "LoadBooking"; } }
            public static string SubmitBooking { get { return "SubmitBooking"; } }
            public static string SignBooking { get { return "SignBooking"; } }
        }
        public static class RecruitmentForms
        {
            public static string W9Form { get { return "W9 Form"; } }
            public static string W4Form { get { return "W4 Form"; } }
            public static string I9Form { get { return "I9 Form"; } }
            public static string DrivingLicense { get { return "Driving License"; } }
            public static string StateLicenseTX { get { return "State License TX"; } }
        }
        public static class CorrespondenceMessageTyp
        {
            public static string Email { get { return "Email"; } }
            public static string SMS { get { return "SMS"; } }
        }
        public static class ActivityType
        {
            public static string Email { get { return "Email"; } }
            public static string Text { get { return "Text"; } }
        }
        public static class RugType
        {
            public static string Rectangle { get { return "Rectangle"; } }
            public static string Square { get { return "Rectangle/Square"; } }
            public static string Circle { get { return "Circle"; } }
            public static string Oval { get { return "Oval"; } }
        }
        public static class ActivityAssociateType
        {
            public static string Account { get { return "Account"; } }
            public static string Lead { get { return "Lead"; } }
            public static string Opportunity { get { return "Opportunity"; } }
            public static string Contact { get { return "Contact"; } }
        }
        public static class ActivityStatus
        {
            public static string Pending { get { return "Pending"; } }
            public static string Completed { get { return "Completed"; } }
        }
        public static class CustomerType
        {
            public static string Residential { get { return "Residential"; } }
            public static string Commercial { get { return "Commercial"; } }
        }
        public static class EstimateStatus
        {
            public static string Init { get { return "Init"; } }
            public static string Created { get { return "Created"; } }
            public static string Open { get { return "Open"; } }
            public static string SentToCustomer { get { return "Sent To Customer"; } }
            public static string ResendToCustomer { get { return "Resend To Customer"; } }
            public static string CustomerViewed { get { return "Customer Viewed"; } }
            public static string Signed { get { return "Signed"; } }
            public static string ContractSingned { get { return "Contract Signed"; } }
            public static string Declined { get { return "Declined"; } }
            public static string CancelEstimate { get { return "Cancelled"; } }
            public static string Completed { get { return "Completed"; } }
            public static string Accepted { get { return "Accepted"; } }
            public static string Draft { get { return "Draft"; } }
        }
        public static class EstimatorStatus
        {
            public static string Init { get { return "Init"; } }
            public static string Accepted { get { return "Accepted"; } }
            public static string Declined { get { return "Declined"; } }
            public static string Open { get { return "Open"; } }
            public static string Pending { get { return "Pending"; } }
            public static string SentToCustomer { get { return "Sent To Customer"; } }
            public static string ResendToCustomer { get { return "Resend To Customer"; } }
            public static string Signed { get { return "Signed"; } }
            public static string ContractSingned { get { return "Contract Signed"; } }
            public static string CustomerViewed { get { return "Customer Viewed"; } }

        }
        public static class EstimatePaymentTerms
        {
            public static string FiftyFifty { get { return "50UponAcceptance50UponCompletion"; } }
            public static string DueonAcceptance { get { return "DueonAcceptance"; } }
            public static string DueUponCompletion { get { return "DueUponCompletion"; } }
        }
        public static class EstimateSubject
        {
            public static string EstimateEmailSubject { get { return "New Estimate from {0}: {1}"; } }
        }

        public static class InvoiceSubject
        {
            public static string InvoiceEmailSubject { get { return "New Invoice from {0}: {1}"; } }
        }
        public static class GlobalSettingInputType
        {
            public static string Editor { get { return "editor"; } }
            public static string TextBox { get { return "textbox"; } }
            public static string TextArea { get { return "textarea"; } }
            public static string Number { get { return "number"; } }
            public static string CheckBox { get { return "checkbox"; } }
            public static string DropDown { get { return "dropdown"; } }
            public static string DateTime { get { return "datetime"; } }
            public static string Image { get { return "img"; } }
            public static string SignImage { get { return "imgsignature"; } }
            public static string CheAndTebox { get { return "cheandtebox"; } }
            public static string ChecAndTxtBox { get { return "checandtxtbox"; } }
            public static string MulDrpDwn { get { return "muldrpdwn"; } }
            public static string Password { get { return "password"; } }
        }

        public static class GlobalSettingInputTag
        {
            public static string CustomerSetting { get { return "customersetting"; } }
            public static string CompanySetting { get { return "companysetting"; } }
            public static string ThirdPartySetting { get { return "thirdpartysetting"; } }
            public static string UtilitySetting { get { return "utilitysetting"; } }
            public static string HomePageSetting { get { return "HomePageSetting"; } }
            public static string JupiterSetting { get { return "jupitersetting"; } }
            public static string RecurringBillingSetting { get { return "recurringbillingsetting"; } }
        }
        public static class CustomerMigrationPlatforms
        {
            public static string Jupiter { get { return "Jupiter"; } }
            public static string Agemni { get { return "Agemni"; } }
        }
        public static class AlarmCustomerStatus
        {
            public static string Init { get { return "Init"; } }
            public static string Created { get { return "Created"; } }
            public static string Commited { get { return "Commited"; } }
        }
        public static class PhoneNumFormat
        {
            public static string Format(string Num)
            {
                string FormatedPhoneNo = "";
                if (Num.Length == 10)
                {
                    FormatedPhoneNo = string.Format("({0}) {1}-{2}", Num.Substring(0, 3), Num.Substring(3, 3), Num.Substring(6));
                }
                else
                {
                    FormatedPhoneNo = Num;
                }
                return string.IsNullOrWhiteSpace(FormatedPhoneNo) ? "" : FormatedPhoneNo;
            }
        }

        public static class AlarmCustomerActions
        {
            public static string CreateCustomer { get { return "CreateCustomer"; } }
            public static string CreateCommiment { get { return "CreateCommitment"; } }
            public static string ActiveateCommitment { get { return "ActiveateCommitment"; } }
        }

        public static class CurrentTransMakeCurrency
        {
            public static bool GetEstimateSignaturePermission(Guid CompanyId)
            {
                if (CompanyId == Guid.Empty)
                {
                    return false;
                }

                Facade.GlobalSettingsFacade globalsetting = new Facade.GlobalSettingsFacade();
                GlobalSetting gb = globalsetting.GetGlobalSettingsByKey(CompanyId, "EstimateSignature"); 
                return gb != null && gb.Value.ToLower() == "true" ? true : false;
            }

            public static bool GetEstimateSignaturePermissionAPI(Guid CompanyId, string constr)
            {
                if (CompanyId == Guid.Empty)
                {
                    return false;
                }

                Facade.GlobalSettingsFacade globalsetting = new Facade.GlobalSettingsFacade(constr);
                GlobalSetting gb = globalsetting.GetGlobalSettingsByKey(CompanyId, "EstimateSignature");
                return gb != null && gb.Value.ToLower() == "true" ? true : false;
            }

            public static string MakeCurrency(Guid CompanyId)
            {
                if(CompanyId == Guid.Empty)
                {
                    return "$";
                }

                Facade.GlobalSettingsFacade globalsetting = new Facade.GlobalSettingsFacade();
                string CurrentTransMakeCurrency = globalsetting.GetCurrentCurrencyByCompanyId(CompanyId);
                return string.IsNullOrWhiteSpace(CurrentTransMakeCurrency) ? "$" : CurrentTransMakeCurrency;
            }

            public static string MakeCurrencyAPI(Guid CompanyId, string constr)
            {
                if (CompanyId == Guid.Empty)
                {
                    return "$";
                }

                Facade.GlobalSettingsFacade globalsetting = new Facade.GlobalSettingsFacade(constr);
                string CurrentTransMakeCurrency = globalsetting.GetCurrentCurrencyByCompanyId(CompanyId);
                return string.IsNullOrWhiteSpace(CurrentTransMakeCurrency) ? "$" : CurrentTransMakeCurrency;
            }

            public static string MakeCurrency(string CompanyId ="")
            {
                if (string.IsNullOrWhiteSpace(CompanyId))
                {
                    string CurrentTransMakeCurrency = "$";
                    if (HttpContext.Current != null && HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated )
                    {
                        var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)HttpContext.Current.User;
                        Facade.GlobalSettingsFacade globalsetting = new Facade.GlobalSettingsFacade();
                        CurrentTransMakeCurrency = globalsetting.GetCurrentCurrencyByCompanyId(CurrentUser.CompanyId.Value);
                    }

                    return CurrentTransMakeCurrency;
                } 
                else
                {
                    string CurrentTransMakeCurrency = "$";
                    Guid CompanyGId = new Guid();
                    if(Guid.TryParse(CompanyId,out CompanyGId) && CompanyGId != new Guid())
                    {
                        Facade.GlobalSettingsFacade globalsetting = new Facade.GlobalSettingsFacade();
                        CurrentTransMakeCurrency = globalsetting.GetCurrentCurrencyByCompanyId(new Guid(CompanyId));
                    }
                    return CurrentTransMakeCurrency;
                }
            }
        }
        public static string HtmlToPlainText(string html)
        {
            const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";//matches one or more (white space or line breaks) between '>' and '<'
            const string stripFormatting = @"<[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
            const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
            var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
            var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
            var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);

            var text = html;
            //Decode html specific characters
            text = System.Net.WebUtility.HtmlDecode(text);
            //Remove tag whitespace/line breaks
            text = tagWhiteSpaceRegex.Replace(text, "><");
            //Replace <br /> with line breaks
            text = lineBreakRegex.Replace(text, Environment.NewLine);
            //Strip formatting
            text = stripFormattingRegex.Replace(text, string.Empty);

            return text;
        }
        public static class FormGenerationDataType
        {
            public static string TextBox { get { return "textbox"; } }
            public static string Calendar { get { return "calendar"; } }
            public static string DropDown { get { return "dropdown"; } }
            public static string TextArea { get { return "textarea"; } }
        }
        public static class DropdownCustomKeys
        {
            public static string MMR { get { return "_MMR"; } }
            public static string SalesPerson { get { return "_Sales"; } }
            public static string Qa { get { return "_Qa"; } }
            public static string Activation { get { return "_Activation"; } }
            public static string FundCompany { get { return "_Fund"; } }
        }
        public static class TicketStatus
        {
            public static string Closed { get { return "Closed"; } }
            public static string InProgress { get { return "InProgress"; } } 
            public static string Created { get { return "Created"; } }
            public static string Completed { get { return "Completed"; } }
            public static string Incomplete { get { return "Incomplete"; } }
            public static string Lost { get { return "Lost"; } }
            public static string Init { get { return "Init"; } }
            public static string AccountOnline { get { return "AccountOnline"; } }
        }
        public static class CommissionType
        {
            public static string PackageRMR { get { return "PackageRMR"; } }
            public static string AddedRMR { get { return "AddedRMR"; } }
            public static string TechCommission { get { return "TechCommission"; } }
            public static string AddedRMRTech { get { return "AddedRMRTech "; } }
        }
        public static class PurchaseOrderStatus
        {
            public static string Init { get { return "Init"; } }
            public static string Created { get { return "Created"; } }
            public static string SentToVendor { get { return "Sent to Vendor"; } }
            public static string Received { get { return "Received"; } }
            public static string RecieveOn { get { return "RecieveOn"; } }
            public static string ReceivedPartially { get { return "Received Partially"; } }
            public static string Paid { get { return "Paid"; } }
            public static string BillCreated { get { return "Bill Created"; } }
        }
        public static class DemandOrderStatus
        {
            public static string Init { get { return "Init"; } }
            public static string Created { get { return "Created"; } }
            public static string POCreated { get { return "PO Created"; } }
            public static string DOComplete { get { return "Completed"; } }
            public static string DOPartialComplete { get { return "Partial Completed"; } }
            public static string SentToVendor { get { return "Sent to Vendor"; } }
            public static string Received { get { return "Received"; } }
            public static string RecieveOn { get { return "RecieveOn"; } }
            public static string ReceivedPartially { get { return "Received Partially"; } }
            public static string Paid { get { return "Paid"; } }
            public static string BillCreated { get { return "Bill Created"; } }
            public static string ReadyToReceived { get { return "Ready To Received"; } }
            public static string PartiallyReadyToReceived { get { return "Partially Ready To Received"; } }
            public static string WaitingForReceived { get { return "Waiting For Received"; } }
        }
        public static class BadInventoryStatus
        {
            public static string Created { get { return "Created"; } }
            public static string Send { get { return "Send"; } }
            public static string Received { get { return "Received"; } }
        }
        public static class PurchaseOrderAction
        {
            public static string Approve { get { return "Approve"; } }
            public static string NotApprove { get { return "NotApprove"; } }
            public static string RecieveOn { get { return "RecieveOn"; } }
        }
        public static class LeadConvertionType
        {
            public static string Manual { get { return "Manually Converted"; } }
            public static string System { get { return "System Generated"; } }
       
        }
        public static class TicketType
        {
            public static string Installtion { get { return "Installation"; } }
            public static string Service { get { return "Service"; } }
            public static string Inspection { get { return "Inspection"; } }
            public static string Billing { get { return "Billing"; } }
            public static string ToDo { get { return "ToDo"; } }
            public static string PO { get { return "PO"; } }
            public static string Rough { get { return "Rough"; } }
            public static string Trim { get { return "Trim"; } }
            public static string Estimate { get { return "Estimate"; } }
            public static string Invoice { get { return "Invoice"; } }
            public static string PickUp { get { return "Pick Up"; } }
            public static string DropOff { get { return "Drop Off"; } }
            public static string Transfer { get { return "Transfer"; } }
            public static string InstallResign { get { return "Install Resign"; } }
            public static string InstallPurchase { get { return "Install Purchase"; } }
            public static string InstallMove { get { return "Install Move"; } }
            public static string Pending { get { return "Pending"; } }
        }
        public static class NotificationType
        {
            public static string Customer { get { return "Customer"; } }
            public static string Employee { get { return "Employee"; } } 
        }
        public static class EquipmentFileType
        {
            public static string ProfilePicture { get { return "ProfilePicture"; } }
            public static string File { get { return "File"; } }
        }
        public static class EquipmentType
        {
            public static string Equipment { get { return "Equipment"; } }
            public static string Service { get { return "Service"; } }
        }
        public static class TimeClockType
        {
            public static string ClockIn { get { return "Clock In"; } }
            public static string ClockOut { get { return "Clock Out"; } }
        }
        public static class TicketTimeClockType
        {
            public static string Start { get { return "start"; } }
            public static string End { get { return "end"; } }
        }
        public static class PTOStatus
        {
            public static string SentToSupervisor { get { return "Sent To Supervisor"; } }
            public static string Accepted { get { return "Accepted"; } }
            public static string Rejected { get { return "Rejected"; } }
        }
        public static class InventoryDescription
        {
            public static string Addedfrompurchaseorder { get { return "Added to warehouse by PO"; } } //Add
            public static string AddedToWarehouseManually { get { return "Added to warehouse manually."; } } //Add
            public static string RemovedFromWarehouseManually { get { return "Removed from warehouse manually."; } } //Release
            public static string Warehousetobranch { get { return "Warehouse to branch"; } } //Release
            public static string Branchtotechnician { get { return "Branch to technician"; } } //Release
            public static string Warehousetotechnician { get { return "Warehouse to technician"; } }  //Release
            public static string Techniciantotechnician { get { return "Technician to technician"; } } 
            public static string Warehousetosale { get { return "Warehouse to sale"; } }  //Release
            public static string Techniciantosale { get { return "Technician to sale"; } } //Release
        }
        public static class InventoryType
        {
            public static string Add { get { return "Add"; } }
            public static string Release { get { return "Release"; } }
        }
        public static class ServiceOptionsType
        {
            public static string Location { get { return "Location"; } }
            public static string Type { get { return "Type"; } }
            public static string Model { get { return "Model"; } }
            public static string Finish { get { return "Finish"; } }
            public static string Capacity { get { return "Capacity"; } }
        }
        public static class BookingStatus
        {
            public static string Init { get { return "Init"; } }
            public static string Approved { get { return "Approved"; } }
            public static string Created { get { return "Created"; } }
            public static string SentToCustomer { get { return "Sent To Customer"; } }
            public static string ResendToCustomer { get { return "Resend To Customer"; } }
            public static string CustomerViewed { get { return "Customer Viewed"; } }
            public static string Signed { get { return "Signed"; } }
            public static string Declined { get { return "Declined"; } }
            public static string CancelBooking { get { return "Booking Cancelled By "; } }
            public static string Cancelled { get { return "Cancelled"; } }
        }

        public static class WatermarkStatus
        {
            public static string NotApplicable { get { return "N.A"; } }
            public static string Pending { get { return "Pending"; } }
            public static string Processing { get { return "Processing"; } }
            public static string Completed { get { return "Completed"; } }
            public static string Failure { get { return "Failure"; } }

        }

        public static class AWSProcessStatus
        {
            public static string Uploaded { get { return "Uploaded"; } }
            public static string NotUploaded { get { return "NotUploaded"; } }
            public static string Local { get { return "LocalFileExists"; } }
            public static string MassWatermark { get { return "MassWatermark"; } }

        }


        public static List<SelectListItem> GetDropDownListByKey(string key)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)HttpContext.Current.User;
            List<SelectListItem> dropdownlist = new List<SelectListItem>();
            Facade.LookupFacade lookup = new Facade.LookupFacade();
            EmployeeFacade ef = new EmployeeFacade();
            if (!string.IsNullOrWhiteSpace(key))
            {
                dropdownlist.AddRange(lookup.GetLookupByKey(key).Select(x => new SelectListItem()
                {
                    Text = x.DisplayText.ToString(),
                    Value = x.DataValue.ToString()
                }).ToList());
                if (dropdownlist.Count > 0)
                    return dropdownlist;
                else
                {
                    if (key == DropdownCustomKeys.MMR)
                    {
                        CustomerFacade Cf = new CustomerFacade();
                        dropdownlist.Add(new SelectListItem()
                        {
                            Text = "Please Select",
                            Value = String.Empty
                        });
                        dropdownlist.AddRange(Cf.GetMMRValueListByCompanyId(CurrentUser.CompanyId.Value).Select(x => new SelectListItem()
                        {
                            Text = x.Name.ToString(),
                            Value = x.Value.ToString()
                        }).ToList());
                    }
                    if (key == DropdownCustomKeys.SalesPerson)
                    {
                        if (CurrentUser.UserRole == "Admin" || CurrentUser.UserRole == "SysAdmin")
                        {
                            var objcurrentlog = ef.GetEmployeeByEmployeeId(CurrentUser.UserId);
                            dropdownlist.Add(new SelectListItem()
                            {
                                Text = "Please Select One",
                                Value = ""
                            });
                            if (objcurrentlog != null)
                            {
                                dropdownlist.Add(new SelectListItem()
                                {
                                    Text = objcurrentlog.FirstName + " " + objcurrentlog.LastName,
                                    Value = objcurrentlog.UserId.ToString()
                                });
                            }
                            dropdownlist.AddRange(ef.GetEmployeeByCompanyIdAndTag(CurrentUser.CompanyId.Value, LabelHelper.UserTags.SalesPerson, new Guid()).Select(x =>
                                  new SelectListItem()
                                  {
                                      Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                                      Value = x.UserId.ToString()
                                  }).ToList());
                        }
                        else
                        {
                            var objEmp = ef.GetEmployeeByEmployeeId(CurrentUser.UserId);
                            if (objEmp != null)
                            {
                                dropdownlist.Add(new SelectListItem()
                                {
                                    Text = objEmp.FirstName.ToString() + " " + objEmp.LastName.ToString(),
                                    Value = CurrentUser.UserId.ToString()
                                });
                            }
                        }
                    }
                    if(key == DropdownCustomKeys.Qa)
                    {
                        dropdownlist.Add(new SelectListItem()
                        {
                            Text = "Please Select One",
                            Value = ""
                        });
                        dropdownlist.AddRange(ef.GetAllEmployee(CurrentUser.CompanyId.Value).Select(x =>
                                  new SelectListItem()
                                  {
                                      Text = x.FirstName.ToString() + " " + x.LastName.ToString(),
                                      Value = x.UserId.ToString()
                                  }).ToList());
                    }
                    if (key == DropdownCustomKeys.Activation)
                    {
                        CustomerFacade Cf = new CustomerFacade();
                        dropdownlist.Add(new SelectListItem()
                        {
                            Text = "Please Select",
                            Value = String.Empty
                        });
                        dropdownlist.AddRange(Cf.GetActivationFeeByCompanyId(CurrentUser.CompanyId.Value).Select(x => new SelectListItem()
                        {
                            Text = x.Name.ToString(),
                            Value = x.Fee.ToString()
                        }).ToList());
                    }
                    if (key == DropdownCustomKeys.FundCompany)
                    {
                        FundFacade ff = new FundFacade();
                        dropdownlist.Add(new SelectListItem()
                        {
                            Text = "Please Select",
                            Value = String.Empty
                        });
                        dropdownlist.AddRange(ff.GetAllFundingCompany().Select(x =>
                             new SelectListItem()
                             {
                                 Text = x.Name.ToString(),
                                 Value = x.Value.ToString()
                             }).ToList());
                    }
                }
            }
            return dropdownlist;
        }

        public static string FormatAmount(double? value)
        {
            string formatted = "0.00";
            if (value.HasValue)
            {
                formatted = string.Format(CultureInfo.InvariantCulture, "{0:N}", value);

            }
            return formatted;
        }
        public static string FormatTo3DP(double? value)
        {
            string formatted = "0.000";
            if (value.HasValue)
            {
                formatted = String.Format("{0:#,0.000}", value);

            }
            return formatted;
        }

        public static string FormatCount(double? value)
        {
            string formatted = "0";
            if (value.HasValue)
            {
                formatted = string.Format("{0:#,##0}", value);

            }
            return formatted;
        }

        public static string NumberFormat(int? value)
        {
            string formatted = "0";
            if (value.HasValue)
            {
                formatted = string.Format(CultureInfo.InvariantCulture, "{0:N}", value);

            }
            string []formatedstring = formatted.Split('.');
            return formatedstring[0];
        }
        public static string FormatFourDecimalAmount(double? value)
        {
            string formatted = "0.0000";
            if (value.HasValue)
            {
                formatted = string.Format(CultureInfo.InvariantCulture, "{0:N4}", value);

            }
            return formatted;
        }

        public static class CustomerAdditionalContactChecked
        {
            public static string PointContact { get { return "Point of Contact"; } }
            public static string AlternateContact { get { return "Alternate Contact"; } }
            public static string AuthorizedUser { get { return "Authorized User"; } }
            public static string EmergencyContact { get { return "Emergency Contact"; } }
        }

        public static class TimeFormatUsingDateTime
        {
            public static string For12Hours(DateTime date)
            {
                string FormatDate = "";
                if(date != null && date != new DateTime())
                {
                    FormatDate = string.Format("{0,8:h:mm tt}", date);
                }
                return FormatDate;
            }

            public static string For24Hours(DateTime date)
            {
                string FormatDate = "";
                if (date != null && date != new DateTime())
                {
                    FormatDate = string.Format("{0,8:H:mm tt}", date);
                }
                return FormatDate;
            }

            public static string ForHoursStringSingleFormat(string data)
            {
                string FormatDate = "";
                if (!string.IsNullOrWhiteSpace(data))
                {
                    var spdata = data.Split(':');
                    if (spdata.Length > 1)
                    {
                        if (spdata[0] == "01" || spdata[0] == "02" || spdata[0] == "03" || spdata[0] == "04" || spdata[0] == "05" || spdata[0] == "06" || spdata[0] == "07" || spdata[0] == "08" || spdata[0] == "09")
                        {
                            string firstdata = spdata[0].Replace("0", "");
                            FormatDate = firstdata + ":" + spdata[1].Replace("AM", " AM").Replace("PM", " PM");
                        }
                        else
                        {
                            FormatDate = spdata[0] + ":" + spdata[1].Replace("AM", " AM").Replace("PM", " PM"); ;
                        }
                    }
                    else
                    {
                        FormatDate = data;
                    }
                }
                return FormatDate;
            }

        }
    }
}