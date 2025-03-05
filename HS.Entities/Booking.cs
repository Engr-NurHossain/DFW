using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class Booking 
	{
        public string CustomerName { set; get; }
        public string CustomerBussinessName { get; set; }
        public string UserNum { get; set; }
        public string CreatedByVal { get; set; }
        public int CustomerIntId { get; set; }
        public string BookingMessage { set; get; }
        public List<BookingDetails> BookingDetailsList { get; set; }
        public string CustomerMailAddress { get; set; }
        public DateTime? CustomerViewedTime { set; get; }
        public string CustomerViewedType { set; get; }
        public int BookingCount { get; set; }
        #region Pdf company info
        //For Pdf Head Company Address
        public string CompanyInfo { get; set; }
        #endregion
        #region Pdf User Stamp
        public List<CustomerAgreement> CustomerAgreement { set; get; }
        #endregion
        #region CustomerAddress
        public int BillingAddressID { get; set; }
        public int DropoffLocationID { get; set; }
        public int PickupLocationID { get; set; }


        #endregion
    }

    //Create Booking 
    public class CreateBooking
    {
        public Booking Booking { set; get; }
        public List<BookingDetails> BookingDetailsList { set; get; }
        public List<BookingExtraItem> BookingExtraItem { set; get; }
        public string EmailAddress { set; get; }
        public double SubTotal { set; get; } 
        public string PhoneNum { get; set; }

        public string BookingShipping { get; set; }
        public string EmailDescription { get; set; }
        public string EmailSubject { get; set; }
        public string CCMail { get; set; }
        public EmailTemplate EmailTemplate { get; set; }

        #region CustomerInfo
        public string CustomerName { set; get; }
        public string CusBussinessName { get; set; }
        public string CustomerNo { set; get; }
        public string CustomerInfo { get; set; }
        public string CusType { get; set; }
        public string CustomerStreetInfo { get; set; }
        public string CustomerStreet { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerCity { set; get; }
        public string CustomerState { set; get; }
        public string CustomerZipCode { set; get; }
        #endregion

        #region Company Info
        public string CompanyEmail { set; get; }
        public string CompanyName { set; get; }
        public string CompanyAddress { set; get; }
        public string CompanyWebsite { set; get; }
        public string companyStreetInfo { get; set; }
        public string CompanyStreet { get; set; }
        public string CompanyLogo { get; set; }
        public string CompanyCity { get; set; }
        public string CompanyState { get; set; }
        public string CompanyZip { get; set; }
        public string CompanyPhone { get; set; }
        public string CompanyInfo { get; set; }
        #endregion
    }

    public class BookingListShapePackage
    {
        public List<Lookup> ListLookUp { get; set; }
        public List<Package> ListPackage { get; set; }
    }

    public class PackageListWithIncludeAndRate
    {
        public int Id { get; set; }
        public string PackageName { get; set; }
        public double PackageRate { get; set; }
        public string IncludedPack { get; set; }
    }

    public class PackageListWithIncludedItem
    {
        public int PackId { get; set; }
        public string PackageInclude { get; set; }
    }
    public class BookingEmailSentResponse
    {
        public string FileLocation { set; get; }
        public bool IsSent { set; get; }
    }
    public class CreateCustomerBooking
    {
        public Booking Booking { set; get; }
        public List<Booking> BookingList { set; get; }
        public string CustomerName { set; get; }
        public string CustomerEmailAddress { set; get; }
        public string CustomerContactNumber { set; get; }
        public string CompanyEmail { set; get; }
        public string CompanyName { set; get; }
        public string CompanyLogo { set; get; }
        public string CompanyAddress { set; get; }
        public string BookingId { set; get; }
        public string CompanyPhone { get; set; }
        public Guid CustomerId { set; get; }
        public string ShortUrl { set; get; }
        public string EmailBody { set; get; }
        public string SMSBody { set; get; }
        public EmailTemplate EmailTemplate { get; set; }
    }
}
