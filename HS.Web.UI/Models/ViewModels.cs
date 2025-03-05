using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HS.Entities;
using System.Web.Mvc;

namespace HS.Web.UI.Models
{
    public class ViewModels
    {

    }
    public class RugCondtionsModel
    {
        public List<SelectListItem> RugCondtitions { set; get; }
        public List<TicketFile> RugImages { set; get; }
        public string Comment { set; get; }
        public int DetailId { set; get; }
    }
    public class CustomerHeaderfliterBar
    {
        public List<SelectListItem> SalesUserList { set; get; }
        public List<SelectListItem> TechUserList { set; get; }
        public List<SelectListItem> PaymentMethodList { set; get; }
        public List<SelectListItem> FundingCompanyList { set; get; }
        public List<SelectListItem> CustomerStatusFilter { set; get; }
        public List<SelectListItem> PackageList { set; get; }
        public List<SelectListItem> BranchList { set; get; }
        public List<SelectListItem> OthersCustomerFilter { set; get; }
        
    }
    public class CustomerHeaderMoneyAndfliterBar
    {
        public CustomerHeaderMoneyBar CustomerHeaderMoneyBarModel { set; get; }
        public CustomerHeaderfliterBar CustomerHeaderfliterBarModel { set; get; }
        public string Firstdate { get; set; }
        public string Lastdate { set; get; }

    }
}