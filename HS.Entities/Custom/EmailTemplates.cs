using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace HS.Entities
{
    public class VerifyEmail 
    {
        public string Name { set; get; }
        public string EmailVerificationLink { set; get; }
        public string ToEmail { set; get; } 
    }
    public class BookingSignNotificationEmail
    {
        public string CustomerName { set; get; }
        public string SalesPersonsName { set; get; }
        public string ToEmail { get; set; }
        public string BookingNO { set; get; }
        public double TotalAmount { set; get; }
        public Guid CompanyId { set; get; }
        public string CompanyName { set; get; }
        public string DeclinationReason { set; get; }

        //SalesPerson or Customer
        public string EmailTo { set; get; }

        public string Logo { get; set; }
        public string Facebook { get; set; }
        public string Youtube { get; set; }
    }
    public class BookingCreatedEmail
    {
        public string FromEmail { set; get; }
        public string FromName { set; get; }
        public string ToEmail { set; get; }
        public string CompanyName { set; get; }
        public string BookingId { set; get; }
        public string BalanceAmount { set; get; }
        public string CustomerName { set; get; }
        public string BookingLink { set; get; }
        public string EmailBody { set; get; }
        public Attachment BookingPdf { set; get; }
        public string Logo { get; set; }
        public string Facebook { get; set; }
        public string Youtube { get; set; }
        public string Subject { get; set; }
        public string CustomerId { get; set; }
        public string EmployeeId { get; set; }
        public string CCEmail { get; set; }
    }
    public class ShareCompanyFile
    {
        public string FileLocationLink { set; get; }
        public string ToEmail { set; get; }
    }

    public class ResetPasswordEmail 
    {
        public string Name { set; get; }
        public string EmailVerificationLink { set; get; }
        public string ToEmail { set; get; }
        
    }
    public class EmailToSalesPersonFromLeads 
    {
        public string SentBy { set; get; }
        public string MailPersonName { set; get; }
        public string SendMailPersonName { set; get; }
        public string EmailBody { set; get; }
        public string ToEmail { set; get; }
        public string Subject { set; get; }

    }
    public class EmailWorkOrderComplete 
    {
        public string Name { set; get; }
        public string EmailBody { set; get; }
        public string ToEmail { set; get; }
        public string WorkOrderProductName { get; set; }
        public int WorkOrderProductQuantity { get; set; }
        public double WorkOrderProductUnitPrice { get; set; }
        public double WorkOrderTotalPrice { get; set; } 
    }
    public class EmailCreateWorkOrder 
    {
        public string EmployeeName { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerMiddleName { get; set; }
        public string CustomerLastName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string AppointmentStartTime { get; set; }
        public string AppointmentEndTime { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        public string Name { set; get; }
        public string ToEmail { set; get; }
        public string CustomerName { get; set; } 
    }
    public class EmailServicekOrderComplete 
    {
        public string Name { set; get; }
        public string EmailBody { set; get; }
        public string ToEmail { set; get; } 
    }
    public class WorkOrderInformationSendEMail 
    {
        public string Name { set; get; }
        public string EmailBody { set; get; }
        public string ToEmail { set; get; }
        public string ServiceNum { get; set; }
        public int ServiceQuan { get; set; }
        public double ServiceUnit { get; set; }
        public double ServicePrice { get; set; }
        public string ServiceList { get; set; }
        public string ServiceListTable { set; get; } 
        public string WorkInstallType { get; set; }
        public double WorkAmount { get; set; }
    }
    public class InvoiceCreatedEmail 
    { 
        public string FromEmail { set; get; }
        public string FromName { set; get; }
        public string ToEmail { set; get; }
        public string CompanyName { set; get; }
        public string InvoiceId { set; get; }
        public string DueDate { set; get; } 
        public string BalanceDue { set; get; }
        public string CustomerName { set; get; } 
        public string InvoiceLink { set; get; }
        public string EmailBody { set; get; }
        public Attachment InvoicePdf { set; get; } 
        public List<Attachment> attachedmentList { get; set; }
        public string Subject { get; set; }
        public string CustomerId { get; set; }
        public string EmployeeId { get; set; }
        public string ccEmail { get; set; }
        public string BccEmail { get; set; }
        public List<string> ListImageUrl { get; set; }
    }
    public class SendEmailToSelectedCustomer
    {
        public string FromEmail { set; get; }
        public string FromName { set; get; }
        public string ToEmail { set; get; }
        public string CompanyName { set; get; }
        public string InvoiceId { set; get; }
        public string DueDate { set; get; }
        public string BalanceDue { set; get; }
        public string CustomerName { set; get; }
        public string InvoiceLink { set; get; }
        public string EmailBody { set; get; }
        public Attachment InvoicePdf { set; get; }
        public string Subject { get; set; }
        public string CustomerId { get; set; }
        public string EmployeeId { get; set; }
        public string ccEmail { get; set; }
        public List<string> ListImageUrl { get; set; }
    }
    public class SendEstimatorInEmail
    {
        public string CompanyName { get; set; }
        public string CustomerName { get; set; }
        public string Subject { get; set; }
        public string ToEmail { set; get; }
        public string ccEmail { get; set; }
        public string EstimatorId { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string ExpDate { set; get; }
        public string SalesGuy { get; set; }
        public string SalesPhone { get; set; }
        public string Url { get; set; }
        public string EmailBody { get; set; }
        public Attachment EstimatorPdf { set; get; }
    }

    public class EvaluationRemainderEmail
    {
        public string CompanyName { get; set; }
        public string EmployeeName { get; set; }
        public string Subject { get; set; }
        public string ToEmail { set; get; }
        public string ccEmail { get; set; }
        public string EmailBody { get; set; }
        public string EvaluationType { get; set; }
        public string NextEvaluation { get; set; }
        public string LastEvaluation { get; set; }

    }
    public class SendCusInfoInEmail
    {
        public string CompanyName { get; set; }
        public string CustomerName { get; set; }
        public string Subject { get; set; }
        public string ToEmail { set; get; }
        public string ccEmail { get; set; }
        
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string SalesGuy { get; set; }
        public string SalesPhone { get; set; }
        public string Url { get; set; }
        public string EmailBody { get; set; }
    }
    public class RequisitionCreatedEmail
    {
        public string FromEmail { set; get; }
        public string FromName { set; get; }
        public string ToEmail { set; get; }
        public string CompanyName { set; get; }
        public string InvoiceId { set; get; }
        public string DueDate { set; get; }
        public string BalanceDue { set; get; }
        public string CustomerName { set; get; }
        public string InvoiceLink { set; get; }
        public string EmailBody { set; get; }
        public Attachment RequisitionPdf { set; get; }
        public string Subject { get; set; }
        public string CustomerId { get; set; }
        public string EmployeeId { get; set; }
        public string ccEmail { get; set; }
    }

    public class TicketSentEmail
    {
        public string FromEmail { set; get; }
        public string FromName { set; get; }
        public string ToEmail { set; get; }
        public string CompanyName { set; get; }
        public string CustomerName { set; get; }
        public string EmailBody { set; get; }
        public Attachment TicketPdf { set; get; }
        public string Subject { get; set; }
        public string CustomerId { get; set; }

    }
    public class FundingSendEmail
    {
        public string FromEmail { set; get; }
        public string FromName { set; get; }
        public string ToEmail { set; get; }
        public string CompanyName { set; get; }
        public string TransactionId { set; get; }
        public string CustomerName { set; get; }
        public string EmailBody { set; get; }
        public Attachment FundingPdf { set; get; }
        public string Subject { get; set; }
        public string ccEmail { get; set; }
    }
    public class PurchaseOrderCreatedEmail 
    {
        public Guid CompanyId { set; get; }
        public string ReplyEmail { set; get; }
        public string FromName { set; get; }
        public string ToEmail { set; get; }
        public string ccEmail { set; get; }
        public string CompanyName { set; get; }
        public string POId { set; get; }
        public string OrderDate { set; get; } 
        public string EmailBody { set; get; }
        public Attachment PurchaseOrderPdf { set; get; } 
        public string Subject { get; set; }
        public Guid UserId { get; set; }
    }
    public class FileAttachmentEmail 
    {
        public string ToEmail { set; get; }
        public string CompanyName { set; get; }
        public string CustomerName { set; get; }
        public string EmailBody { set; get; }
        public Attachment FileAttachmentPdf { set; get; } 
        public string Subject { get; set; }
    }
    public class EmailToEmployeeFromCustomerNote 
    {
        public string CustomerName { get; set; }
        public string Notes { get; set; }
        public string ToEmail { get; set; }
        public string EmailReciver { get; set; } 
    }
    public class EmailToResponsiblePersonFromCustomerNote 
    {
        public string CustomerNum { get; set; }
        public string Notes { get; set; }
        public string ToEmail { get; set; }
        public string ReceiverName { get; set; } 
    }
    public class EstimatorApprovedEmail
    {
        
        public string EstimatorId { get; set; }
        public string CustomerName { get; set; }
        public string UserName { get; set; }
        public string Notes { get; set; }
        public string ToEmail { get; set; }
        public string EmailReciver { get; set; } 
        public Guid CompanyId { set; get; } 
        public List<string> ReceiverEmailList { get; set; }
        public bool IsSystemAutoSent { get; set; }
        public string FromName { get; set; }   
        public Guid SendBy { get; set; }
        public string EmailTo { set; get; }
        public string SalesPersonsName { set; get; }
        public string CustomerLink { set; get; }
        public Attachment EstimatorPdf { set; get; }
        public string Status { get; set; }
    }
    public class EmailToEmployeeFromFollowUpNote 
    {
        public string CustomerName { get; set; }
        public string EmailBody { get; set; }
        public string ToEmail { get; set; } 
        public string AssignPersonName { get; set; }
        public int CustomerIntId { get; set; }
        public string CreatedByName { get; set; }
        public string AttnBy { get; set; }
        public string CreatedOn { get; set; }
    }

    //[Shariful-16-9-19]
    public class DeclineMail
    {
        public string CustomerName { get; set; }
        public string EmailBody { get; set; }
        public string ToEmail { get; set; }
        public string DeclinationReason { set; get; }
    }
    //[~Shariful-16-9-19]

    public class EmailConvertLeadToCustomer 
    {
        public string CustomerName { get; set; }
        public string ToEmail { get; set; }
        public string EmailBody { get; set; } 
    }
    public class EmailNotConvertLeadToCustomer 
    {
        public string CustomerName { get; set; }
        public string ToEmail { get; set; }
        public string EmailBody { get; set; } 
    }
    public class SurveyEmail
    {
        public string Name { set; get; }
        public string SurveyLink { set; get; }
        public string AssignTo { set; get; }
        public string ToEmail { set; get; }
        public string Subject { set; get; }
        public string RequestedBy { get; set; }
    }

    public class SurveyEmailConfirmation
    {
        public string Name { set; get; }
        public string Content { set; get; }
        public string ToEmail { get; set; }
        public string Subject { set; get; }
        public Attachment ReviewPdf { set; get; }
        public string ReplyEmail { set; get; }

    }
    public class EmailNotSetCustomerBilling 
    {
        public string CustomerName { get; set; }
        public string ToEmail { get; set; }
        public string EmailBody { get; set; } 
    }
    public class EmailToLeadFromCurrentLoggedinUser 
    {
        public string Name { set; get; }
        public string EmailBody { set; get; }
        public string ToEmail { set; get; }
        public string Subject { set; get; } 
    }

    public class ServiceOrderInformationSendEMail 
    {
        public string Name { set; get; }
        public string EmailBody { set; get; }
        public string ToEmail { set; get; }
        public string Servicename { get; set; }
        public int Servicequantity { get; set; }
        public double ServiceunitPrice { get; set; }
        public double Servicetotalprice { get; set; }
        public string ServiceList { get; set; } 
    }
    public class EmailCreateServiceOrder 
    {

        public string EmployeeName { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerMiddleName { get; set; }
        public string CustomerLastName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string AppointmentStartTime { get; set; }
        public string AppointmentEndTime { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    
        public string Name { set; get; }
        public string ToEmail { set; get; }
        public string CustomerName { get; set; } 
    }
    public class SetupLeadCustormer 
    {

        public string CustomerFirstName { get; set; }
        public string CustomerMiddleName { get; set; }
        public string CustomerLastName { get; set; }
        public string EmailBody { set; get; }

        public string Name { set; get; }
        public string ToEmail { set; get; }

        public string SalesPersonsName { set; get; }
        public string ToSalesPersonsEmail { set; get; }

        public string CustomerName { get; set; }
        public string CustomerNo { set; get; }
        public string CompanyName { get; set; } 
        public Attachment PdfAggrement { get; set; }
        public string CustomerId { get; set; }
        public string EmployeeId { get; set; }
        public string InvoiceId { set; get; }
        public string PaymentMethod { set; get; }
        public double AmountPaid { set; get; }
        public double TotalAmount { set; get; }
        public double BalanceDue { set; get; }
        public string TransactionId { set; get; }
        public string Description { set; get; }
    }
    public class LeadsAggrement 
    {
        public string CustomerNum { get; set; } 
        public Attachment LeadsAggrementpdf { get; set; }
        public string ToEmail { get; set; }
        public string BodyLink { get; set; }
        public string CustomerId { get; set; }
        public string EmployeeId { get; set; }
    }
    public class FileManagement
    {
        public string CustomerNum { get; set; }
        public Attachment fileManagementpdf { get; set; }
        public string ToEmail { get; set; }
        public bool IsFileWithoutCustomerSign { get; set; }
        public string BodyLink { get; set; }
        public string Subject { get; set; }
        public string CompanyName { get; set; }
        public string Body { get; set; }
        public string CustomerId { get; set; }
        public string EmployeeId { get; set; }
    }

    public class LeadCreation
    {
        public string CustomerNum { get; set; }
        public string ToEmail { get; set; }
        public string BodyContent { get; set; }
        public string BodyLink { get; set; }
        public string CustomerId { get; set; }
        public string EmployeeId { get; set; }
    }
    public class CredentialMail
    {
 
        public string ToEmail { get; set; }
        public string Message { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string CustomerName { get; set; }
    }
    public class LeadtoCustomer 
    {

        public string CustomerFirstName { get; set; }
        public string CustomerMiddleName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerAddress { get; set; }

        public string EmployeeFirstName { get; set; }
        public string EmployeeLastName { get; set; }
        public string EmployeeEmail { get; set; }

        public string Name { set; get; }
        public string ToEmail { set; get; }
        public string CustomerName { get; set; }
        public string EmployeeName { get; set; }
    }
    public class LeadtoCustomerCancellation
    {

        public string CustomerFirstName { get; set; }
        public string CustomerMiddleName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerAddress { get; set; }

        public string EmployeeFirstName { get; set; }
        public string EmployeeLastName { get; set; }
        public string EmployeeEmail { get; set; }

        public string Name { set; get; }
        public string ToEmail { set; get; }
        public string CustomerName { get; set; }
        public string EmployeeName { get; set; }
        public Attachment fileManagementpdf { get; set; }
    }
    public class EmailOfEstimateNotConvertedNotification 
    {
        public string InvoiceCreator { get; set; }
        public string ToEmail { get; set; }
        public string EmailBody { get; set; } 
    }
    public class EstimateSignNotificationEmail 
    {
        public string CustomerName { set; get; }
        public string SalesPersonsName { set; get; }
        public string ToEmail { get; set; }
        public string EstimateNO { set; get; }
        public double TotalAmount { set; get; }
        public Guid CompanyId { set; get; }
        public string CompanyName { set; get; }
        public string DeclinationReason { set; get; }
        //SalesPerson or Customer
        public string EmailTo { set; get; }

        public string CreatedByName { set; get; }

    }
    public class RequestAdminEmail 
    {
        public string Name { get; set; }
        public string Comapny { get; set; }
        public string Email { get; set; }
        public string Phone { get; set;}
    }
    public class OTPEmail
    {
        public string Name { get; set; }
        public Guid CompanyId { get; set; }
        public string ToEmail { get; set; }
        public string OTP { get; set; }
    }
    public class SendFriendEmail
    {
        public string Name { get; set; }
        public string SenderName { get; set; }
        public Guid CompanyId { get; set; }
        public string ToEmail { get; set; }
        public string Messsage { get; set; }
        public string ShortLink { get; set; }
    }
    public class SendSurveyEmail
    {
        public string Name { get; set; }
        public string SenderName { get; set; }
        public Guid CompanyId { get; set; }
        public string ToEmail { get; set; }
        public string shortLink { get; set; }
        public string CompanyName { set; get; }
        public string Subject { set; get; }
        public string Header { set; get; }
   
    }
    public class UpdateCustomerConfirmation
    {
        public string Name { get; set; }
        public Guid CompanyId { get; set; }
        public string ToEmail { get; set; }
        public string ColumnName { get; set; }
    }
    public class SubscribedToAthorizeNotification
    {
        public string ToEmail { set; get; }
        public string CustomerName { set; get; }
        public string CompanyName { set; get; }
        public string SubscriptionAmount { set; get; }
        public string SubscriptionPeriod { set; get; }
        public string BillingCycle { set; get; }
        public string PaymentMethod { set; get; }
        public string ReplyTo { set; get; }
        public string ReplyToName { set; get; }
        public Guid CompanyId { set; get; }
    }

    public class TicketNotificationEmails
    {
        public Guid CompanyId { set; get; }
        public string TicketNumber { set; get;  }
        public string TicketMessage { set; get; }
        public string ToEmail { set; get; } 
        public string CreatedByName { set; get; }
        public string CreatedForCustomerName { set; get; }
        public string Subject { set; get; }
        public string BodyMessage { set; get; }
        public string HeaderMessage { set; get; }
        public string TicketUrl { set; get; }
        public string AppointmentStartTime { get; set; }
        public string AppointmentEndTime { get; set; }
        public string CustomerAddress { get; set; }
        public string TicketStatus { get; set; }
        public DateTime CompletionDate { get; set; }
    }

    public class ActivityNotificationEmail
    {
        public string Name { get; set; }
        public string ToEmail { get; set; }
        public string Body { get; set; }
    }

    public class LateNotificationTicketEmail
    {
        public int TicketId { get; set; }
        public string CustomerName { get; set; }
        public string ToEmail { get; set; }
        public string Body { get; set; }
    }

    public class TicketCustomerNotificationEmail
    {
        public string CustomerName { get; set; }
        public string ToEmail { get; set; }
        public string Body { get; set; }
    }

    public class InventoryTechReceiveNotificationEmail
    {
        public string Name { get; set; }
        public string ToEmail { get; set; }
        public string Body { get; set; }
    }

    public class CustomerTicketAddendumEmail
    {
        public string CustomerName { get; set; }
        public string ToEmail { get; set; }
        public string BodyLink { get; set; }
    }

    public class RUGTicketAgreementEmail
    {
        public string Name { get; set; }
        public string ToEmail { get; set; }
        public string Body { get; set; }
        public Attachment TicketAgreementPdf { get; set; }
    }

    public class TicketPDFEmailModel
    {
        public string Name { get; set; }
        public string ToEmail { get; set; }
        public string Body { get; set; }
        public Attachment attachment { get; set; }
    }
    public class NotificationEmail
    {
        public string ToEmail { get; set; }
        public string EmailBody { get; set; }
        public string Subject { get; set; }
        public string ToWhom { get; set; }
    }
    public class FileManagementNotificationEmail
    {
        public string ToEmail { get; set; }
        public string CustomerNameWithId { get; set; }
    }
    public class RugTrackerBookingEmail
    {
        public string OrderID { set; get; }
        public string CustomerId { set; get; }
        public string CustomerName { set; get; }        
        public string ToEmail { set; get; }
        public string Phone { set; get; }
        public string Street { set; get; }
        public string Address { set; get; }       
        public string DropoffDateTime { get; set; }
        public string PickupDateTime { get; set; }
        public string Amount { set; get; }
        public string Tax { set; get; }
        public string TotalAmount { set; get; }
        public string CustomerLink { set; get; }        
        public string RugInformation { set; get; }
    }
}
