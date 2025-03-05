using HS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace HS.API.Models
{
    public class CustomModel
    {

    }
    public class CompanyModel
    {
        public string CompanyName { get; set; }
        public Guid CompanylId { get; set; }
    }
    public class CustomerModel
    {
        public string CustomerName { get; set; }
        public Guid CustomerId { get; set; }
    }

    public class FileType
    {
        public string filename { get; set; }
        public string filepath { get; set; }
    }

    public class EstimateListModel
    {
        public List<InvoiceDetail> ListEstimate { get; set; }
    }

    public class APISelectListItem
    {
        public string text { get; set; }
        public string value { get; set; }
        public bool selected { get; set; }
        public string color { get; set; }
        public string image { get; set; }
    }

   
    public class CustomBranchModel
    {
        public Guid CompanyId { get; set; }
        public string Logo { get; set; }
    }

    public class EstimateEmailModel
    {
        public string email { get; set; }
        public string subject { get; set; }
        public string bodycontent { get; set; }
    }
    public class ApiResponse<T>
    {
        public T result { get; set; }

        
        public bool success { get; set; }

       
        public string error { get; set; }

        public ApiResponse(T result, bool success, string error)
        {
            this.result = result;
            this.success= success;
            this.error = error;
        }

        public static ApiResponse<T> Success(T result) => new ApiResponse<T>(result, true, null);

        public static ApiResponse<T> Error(string message) => new ApiResponse<T>(default, false, message);
    }

    public class SmartLeadsResponse
    {
        public int? LeadId { get; set; }
        public string InvoiceId { get; set; }
        public bool IsInvoice { get; set; }
        public bool FirstPage { get; set; }
        public bool Recreate { get; set; }
        public int TicketId { get; set; }
        public bool IsEstimator { get; set; }
        public int EstId { get; set; }
        public string EstimatorId { get; set; }
        public int AgreementDocumentHeight { get; set; }
        public string StringHeight { get; set; }
        public bool IsRecreate { get; set; }
        public string ContractDate { get; set; }
        public string DocURL { get; set; }
        public Customer CustomerDetails { get; set; }
        //  public List<SelectListItem> MultipleDoc { get; set; }
        public string url { get; set; }
    }

    public class LookupResponseModel
    {
        public string text { get; set; }
        public string value { get; set; }
        public int order { get; set; }
        public bool active { get; set; }
    }
    public class NoteResponseModel
    {
        public int id { get; set; }
      
        public Guid userGuid { get; set; }
        public string message { get; set; }
        public DateTime date { get; set; }
    }

    public class SurveyItem
    {
        public string name { get; set; }
        public Guid guid { get; set; }
    }

    public class CustomerResponse
    {
        public int id { get; set; }
        public Guid customerGuid { get; set; }
        public string name { get; set; }
        public string business { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string type { get; set; }
        public string county { get; set; }
        public string verbalPassword { get; set; }
        public string zip { get; set; }
        public string profilePicture { get; set; }
    }
    public class EquipmentSuggestionResponseModel
    {
        public int id { get; set; }
        public Guid equipmentGuid { get; set; }
        public string name { get; set; }
        public string sku { get; set; }
        public string barcode { get; set; }
        public double point { get; set; }
        public int onHand { get; set; }
        public double unitCost { get; set; }
        public double unitPrice { get; set; }
    }
    public class ServiceSuggestionResponseModel
    {
        public int id { get; set; }
        public Guid serviceGuid { get; set; }
        public string name { get; set; }
        public double unitCost { get; set; }
        public double unitPrice { get; set; }
    }

    public class EstimateSMSModel
    {
        public string smsbody { get; set; }
        public string contactno { get; set; }
    }

    public class CustomPrincipal
    {
        public string Username { set; get; }
        public Guid Userid { set; get; }
    }

    public class FavouriteEquipmentsModel
    {
        public int Id { get; set; }
        public Guid EquipmentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SKU { get; set; }
        public double Retail { get; set; }
    }

    public class TimeClockHistory
    {
        public int Id { get; set; }
        public string ClockInOutDate { get; set; }
        public string ClockInTime { get; set; }
        public string ClockOutTime { get; set; }
        public string ClockInNote { get; set; }
        public string ClockOutNote { get; set; }
        public string ClockInPosition { get; set; }
        public string ClockOutPosition { get; set; }
        public string TimeSpent { get; set; }

    }

    public class CustomerCustomModel
    {
        public int Id { get; set; }
        public Guid CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string StreetType { get; set; }
        public string Apartment { get; set; }
        public string PrimaryPhone { get; set; }
        public string SecondaryPhone { get; set; }
        public string CellNo { get; set; }
        public string CustomerType { get; set; }
    }

    public class TimeClockStatus
    {
        public string Label { get; set; }
        public string Value { get; set; }
    }

    public class StockItemCustomModel
    {
        public int ItemId { get; set; }
        public bool IsAvailable { get; set; }
    }
  
}