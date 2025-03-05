using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Excel = ClosedXML.Excel;
using System.Globalization;
using System.IO;
using HS.Entities;
using HS.Framework;
using NLog;

namespace HS.Web.UI.Controllers
{
    public class ADSController : BaseController
    {
        public ADSController()
        {
            logger = LogManager.GetCurrentClassLogger();
        }
        // GET: ADS
        public ActionResult Index()
        {
            return View();
        }
        
        public JsonResult CustomerImport(string File, string isCustomer,string PlatForm)
        {
            string subPath = "~/LeadImportReports"; // your code goes here

            bool exists = System.IO.Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath(subPath));
            if (!exists)
                System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath(subPath));
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var serverFile = Server.MapPath(File);
            if (System.IO.File.Exists(serverFile))
            {
                FileInfo ExcelFile = new FileInfo(serverFile);
                if (ExcelFile.Length > 0)
                {
                    var workbook = new Excel.XLWorkbook(ExcelFile.FullName);
                    Excel.IXLWorksheet workSheet = workbook.Worksheet(1);
                    var xlRange = workSheet.RangeUsed();
                    int rowCount = xlRange.Rows().Count();
                    int colCount = xlRange.Cells().Count();
                    var LookUpList = _Util.Facade.LookupFacade.GetAllLookup();
                    List<Employee> employees = _Util.Facade.EmployeeFacade.GetAllEmployee();
                    for (int i = 2; i <= rowCount; i++)
                    {
                        string SoldBy = "";
                        string Installer = "";

                        Customer customer = new Customer();
                        customer.CustomerId = Guid.NewGuid();

                        CustomerCompany CC = new CustomerCompany();
                        CC.CompanyId = CurrentUser.CompanyId.Value;
                        CC.CustomerId = customer.CustomerId;
                        CC.IsLead = false;

                        CustomerCancel CustomerCancel = new CustomerCancel();
                        CustomerCancel.CompanyId = CurrentUser.CompanyId.Value;
                        CustomerCancel.CustomerId = customer.CustomerId;
                        CustomerCancel.EmployeeId = new Guid("22222222-2222-2222-2222-222222222222");
                        CustomerCancel.IsActivated = true;

                        PaymentInfoCustomer PIC = new PaymentInfoCustomer();
                        PaymentProfileCustomer PPC = new PaymentProfileCustomer();
                        PaymentInfo PI = new PaymentInfo();

                        try
                        {
                            for (int j = 1; j <= colCount; j++)
                            {
                                if (xlRange.Cell(i, j) != null && xlRange.Cell(i, j).Value != null)
                                {
                                    try
                                    {
                                        #region Migration Conditions
                                        var value = xlRange.Cell(i, j).Value.ToString();
                                        var header = xlRange.Cell(1, j).Value.ToString();
                                        if (string.IsNullOrWhiteSpace(header))
                                        {
                                            break;
                                        }
                                        else if (header == "CustomerAccountId")
                                        {
                                            customer.AdditionalCustomerNo = value;
                                            continue;
                                        }
                                        else if(header == "MonitoringId")
                                        {
                                            customer.AccountNo = value;
                                            continue;
                                        }
                                        else if (header == "LastName")
                                        {
                                            customer.LastName = value;
                                            continue;
                                        }
                                        else if (header == "FirstName")
                                        {
                                            customer.FirstName = value;
                                            continue;
                                        }
                                        else if (header == "Company")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "Company: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "Address1")
                                        {
                                            customer.Street = value;
                                            continue;
                                        }
                                        else if (header == "Address2")
                                        {
                                            customer.Address2 = value;
                                            continue;
                                        }
                                        else if (header == "Address3")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "Address3: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "City")
                                        {
                                            customer.City = value;
                                            continue;
                                        }
                                        else if (header == "State")
                                        {
                                            customer.State = value;
                                            continue;
                                        }
                                        else if (header == "County")
                                        {
                                            customer.County = value;
                                            continue;
                                        }
                                        else if (header == "Zip")
                                        {
                                            customer.ZipCode = value;
                                            continue;
                                        }
                                        else if (header == "Email")
                                        {
                                            customer.EmailAddress = value;
                                            continue;
                                        }
                                        else if (header == "HomeOwnerVerification")
                                        {
                                            customer.HomeOwner = value;
                                            continue;
                                        }
                                        else if (header == "Phone1")
                                        {
                                            customer.PrimaryPhone = value.PhoneNumberFormat();
                                            continue;
                                        }
                                        else if (header == "Phone2")
                                        {
                                            customer.SecondaryPhone = value.PhoneNumberFormat();
                                            continue;
                                        }
                                        else if (header == "Phone3")
                                        {
                                            customer.CellNo = value.PhoneNumberFormat();
                                            continue;
                                        }
                                        else if (header == "DateOfBirth")
                                        {
                                            DateTime DateOfBirth = new DateTime();

                                            if (DateTime.TryParse(value, out DateOfBirth))
                                            {
                                                customer.DateofBirth = DateOfBirth;
                                            }
                                            continue;
                                        }
                                        else if (header == "Area")
                                        {
                                            //Do not import
                                            continue;
                                        }
                                        else if (header == "CreditGrade")
                                        {
                                            customer.CreditScore = value;
                                            continue;
                                        }
                                        else if (header == "CreditScore")
                                        {
                                            /*int CreditScoreValue = 0;

                                            if(int.TryParse(value,out CreditScoreValue))
                                            {
                                                customer.CreditScoreValue = CreditScoreValue;
                                            }*/
                                            customer.CreditScore = value;
                                            continue;
                                        }
                                        else if (header == "CreditRunBy")
                                        {
                                            //Carlos will give us the user list
                                            continue;
                                        }
                                        else if (header == "StatusID")
                                        {
                                            //Need To discuss
                                            continue;
                                        }
                                        else if (header == "SystemName")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "SystemName: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "SystemPackageName")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "SystemName: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "Takeover")
                                        {
                                            int TakeOver = 0;
                                            if(int.TryParse(value,out TakeOver) && TakeOver ==1)
                                            {
                                                customer.InstallType = "Take_Over";
                                            }
                                            continue;
                                        }
                                        else if (header == "SignalsConfirmationNumber")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "Signals ConfirmationNumber: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "OnlineConfirmation")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "Online Confirmation: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "Language")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "Language: "+value+@" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "IsRebateOffered")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "IsRebateOffered: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "IsRebateReceived")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "IsRebateReceived: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "ActivationFeeAmount")
                                        {
                                            double ActivationFee = 0;
                                            if (double.TryParse(value, out ActivationFee))
                                            {
                                                customer.ActivationFee = ActivationFee;
                                            }
                                            continue;
                                        }
                                        else if (header == "VIPServiceFeeAmount")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "VIPServiceFeeAmount: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "IsRebatePaid")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "IsRebatePaid: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "CollectionsAmount")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "CollectionsAmount: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "PreInstallSurvey")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "PreInstallSurvey: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "PostInstallSurvey")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "PostInstallSurvey: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "AccountHolder")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "AccountHolder: " + value + @" 
";
                                                customer.Ownership = value;
                                            }
                                            continue;
                                        }
                                        else if (header == "MonitoringCompany")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "MonitoringCompany: " + value + @" 
";
                                                customer.CSProvider = value;
                                            }
                                            continue;
                                        }
                                        else if (header == "InstallDate")
                                        {
                                            DateTime InstallDate = new DateTime();
                                            if (DateTime.TryParse(value, out InstallDate))
                                            {
                                                customer.InstallDate = InstallDate;
                                            }
                                            else
                                            {
                                                customer.InstallDate = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "SaleDate")
                                        {
                                            DateTime SalesDate = new DateTime();
                                            if (DateTime.TryParse(value, out SalesDate))
                                            {
                                                customer.SalesDate = SalesDate;
                                            }
                                            else
                                            {
                                                customer.SalesDate = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "Submitted_Date")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "Submitted_Date: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "Funded_Date")
                                        {
                                            DateTime FundingDate = new DateTime();
                                            if (DateTime.TryParse(value, out FundingDate))
                                            {
                                                customer.FundingDate = FundingDate;
                                            }
                                            else
                                            {
                                                customer.FundingDate = null;
                                            }
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "Funded_Date: " + value + @" 
";
                                            } 
                                            continue;
                                        }
                                        else if (header == "Declined_Date")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "Declined_Date: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "ChargedBack_Date")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "ChargedBack_Date: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "CollectionsDate")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "CollectionsDate: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "ContractExtensionDate")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "ContractExtensionDate: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "SavedDate")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "SavedDate: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "DateCreated")
                                        {
                                            DateTime DateCreated = new DateTime();
                                            if (DateTime.TryParse(value, out DateCreated))
                                            {
                                                customer.CreatedDate = DateCreated;
                                                customer.JoinDate = DateCreated;
                                            }
                                            else
                                            {
                                                customer.CreatedDate = DateTime.Now.UTCCurrentTime() ;
                                            }
                                            continue;
                                        }
                                        else if (header == "DateCancelled")
                                        {
                                            DateTime DateCancelled = new DateTime();
                                            if (DateTime.TryParse(value, out DateCancelled))
                                            {
                                                CustomerCancel.CancelDatet = DateCancelled;
                                            }
                                            continue;
                                        }
                                        else if (header == "CancellationReason")
                                        {
                                            CustomerCancel.CancelReason = value;
                                        }
                                        else if (header == "DateCreatedLong")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "DateCreatedLong: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "MMRBase")
                                        {
                                            customer.MonthlyMonitoringFee = value;
                                            continue;
                                        }
                                        else if (header == "SalesRepID")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "SalesRepID: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "SalesRep")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "SalesRep: " + value + @"
";
                                                SoldBy = value;
                                            }
                                            continue;
                                        }
                                        else if (header == "TechnicianID")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "TechnicianID: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "Technician")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "Technician: " + value + @"
";
                                                Installer = value;
                                            }
                                            continue;
                                        }
                                        else if (header == "FriendsFamilyRepID")
                                        {
                                            //Do not import
                                            continue;
                                        }
                                        else if (header == "FriendsFamilyRep")
                                        {
                                            //Do not import
                                            continue;
                                        }
                                        else if (header == "SavedByID")
                                        {
                                            //Do not import
                                            continue;
                                        }
                                        else if (header == "SavedBy")
                                        {
                                            //Do not import
                                            continue;
                                        }
                                        else if (header == "RepEmployeeID")
                                        {
                                            //Carlos will give us the user list
                                            continue;
                                        }
                                        else if (header == "TechEmployeeID")
                                        {
                                            //Carlos will give us the user list
                                            continue;
                                        }
                                        else if (header == "Status")
                                        {
                                            customer.Status = value;
                                            customer.CustomerStatus = value;
                                            if(value == "LEAD")
                                            {
                                                CC.IsLead = true;
                                            }
                                            continue;
                                        }
                                        else if (header == "CustomStatus")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "CustomStatus: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "EquipmentStatus")
                                        {
                                            //Need to update Lookup
                                            customer.InstalledStatus = value;
                                        }
                                        else if (header == "isAccountOnline")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "isAccountOnline: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "isAccountInService")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "isAccountInService: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "CaseCount")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "CaseCount: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "ContractTerm")
                                        {
                                            double ContractTerm = 0;
                                            if(double.TryParse(value,out ContractTerm) && ContractTerm >0) 
                                            {
                                                customer.ContractTeam = (ContractTerm / 12).ToString();
                                            }
                                            continue;
                                        }
                                        else if (header == "BillingDayOfMonth")
                                        {
                                            int BillDay = 0;
                                            if (int.TryParse(value, out BillDay) && BillDay > 0)
                                            {
                                                customer.BillDay = BillDay;
                                            }
                                            continue;
                                        }
                                        else if (header == "BillingMethod")
                                        {
                                            if(value == "EFT")
                                            {
                                                value = Helper.LabelHelper.PaymentMethod.ACH;
                                            }
                                            customer.PaymentMethod = value;
                                            continue;
                                        }
                                        else if (header == "MMRTotal")
                                        {
                                            double BillAmount = 0;
                                            if(double.TryParse(value, out BillAmount))
                                            {
                                                customer.BillAmount = BillAmount;
                                            }
                                            continue;
                                        }
                                        else if (header == "CreditCardNumber")
                                        {
                                            //Can't Use this data
                                            PI.CardNumber = value;
                                            continue;
                                        }
                                        else if (header == "CCExpiration")
                                        {
                                            //Can't Use this data
                                            PI.CardExpireDate = value;
                                            continue;
                                        }
                                        else if (header == "RoutingNumber")
                                        {
                                            //Can't Use this data
                                            PI.RoutingNo = value;
                                            continue;
                                        }
                                        else if (header == "AccountNumber")
                                        {
                                            //Can't Use this data
                                            PI.AcountNo = value;
                                            continue;
                                        }
                                        else if (header == "BankName")
                                        {
                                            //Can't Use this data
                                            PI.BankName = value;
                                            continue;
                                        }
                                        else if (header == "ActiveSubscriptionID")
                                        {
                                            customer.AuthorizeRefId = value;
                                            continue;
                                        }
                                        else if (header == "SSN")
                                        {
                                            //Can't Use this data
                                            customer.SSN = value;
                                            continue;
                                        }
                                        else if (header == "PointsUsedSales")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "PointsUsedSales: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "PointsSoldSales")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "PointsSoldSales: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "PurchasePriceTotalSales")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "PurchasePriceTotalSales: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "PurchasePriceTotalTech")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "PurchasePriceTotalTech: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "PointBalanceSales")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "PointBalanceSales: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "EquipmentBalanceSales")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "EquipmentBalanceSales: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "PointsUsedTech")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "PointsUsedTech: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "PointsSoldTech")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "PointsSoldTech: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "EquipmentBalanceTech")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "EquipmentBalanceTech: " + value + @" 
";
                                            }
                                            continue;

                                        }
                                        else if (header == "PointsGiven")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "PointsGiven: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "Rep Deductions")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "Rep Deductions: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "Tech Deductions")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "Tech Deductions: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "TotalCreditsRun")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "TotalCreditsRun: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "Equifax Full")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "Equifax Full: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "Equifax Pre")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "Equifax Pre: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "Transunion Full")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "Transunion Full: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "Transunion Pre")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "Transunion Pre: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "Experian Full")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "Experian Full: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "Experian Pre")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "Experian Pre: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "TotalCreditHits")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "TotalCreditHits: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "AccountBalance")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "AccountBalance: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "TermRemaining")
                                        {
                                            //Do not import
                                            continue;
                                        }
                                        else if (header == "PermitStatus")
                                        {
                                            //Do not import
                                            continue;
                                        }
                                        else if (header == "EscalationStatus")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "EscalationStatus: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "SetupByApplication")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "SetupByApplication: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "AppointmentSetter")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "AppointmentSetter: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "MailingAddress")
                                        {
                                            customer.Address = value;
                                            continue;
                                        }
                                        else if (header == "BillingAddress")
                                        {
                                            customer.Address2 = value;
                                            continue;
                                        }
                                        else if (header == "AbortCode")
                                        {
                                            customer.Passcode = value;
                                            continue;
                                        }
                                        else if (header == "CellSerialNumber")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "CellSerialNumber: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "AlarmDotComCustomerAccount")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.AlarmRefId = value;
                                                customer.IsAlarmCom = true;
                                            }
                                            else
                                            {
                                                customer.IsAlarmCom = false;
                                            }
                                            continue;
                                        }
                                        else if (header == "OriginalSalesRep")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "OriginalSalesRep: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "SystemServices")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "SystemServices: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "ContractId")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "ContractId: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "TransactionID")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "TransactionID: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "Escalation")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "Escalation: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "AreaName")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "AreaName: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "CustomerLeadStatus")
                                        {
                                            customer.CustomerStatus = value;
                                            continue;
                                        }
                                        else if (header == "CustomerLeadStatusId")
                                        {
                                            //DO not import
                                            continue;
                                        }
                                        else if (header == "CustomerLeadSource")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.LeadSource = value.Replace("(", "_").Replace(")", "_").Replace("/", "_");
                                            }
                                            continue;
                                        }
                                        else if (header == "CustomerLeadSourceID")
                                        {
                                            //DO not import
                                            continue;
                                        }
                                        else if (header == "FollowUpDate")
                                        {
                                            DateTime FollowUpDate = new DateTime();
                                            if (DateTime.TryParse(value, out FollowUpDate))
                                            {
                                                customer.FollowUpDate = FollowUpDate;
                                            }
                                            else
                                            {
                                                customer.QA1Date = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "DateModified")
                                        {
                                            DateTime LastUpdatedDate = new DateTime();
                                            if (DateTime.TryParse(value, out LastUpdatedDate))
                                            {
                                                customer.LastUpdatedDate = LastUpdatedDate;
                                            }
                                            else
                                            {
                                                customer.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                                            }
                                            continue;
                                        }
                                        else if (header == "AlternateAccountId")
                                        {
                                            customer.SecondCustomerNo = value;
                                        }
                                        else if (header == "Opportunity_DateBegin")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "Opportunity_DateBegin: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "Opportunity_DateEnd")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "Opportunity_DateEnd: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "MoveIn_Date")
                                        {
                                            DateTime MovingDate = new DateTime();
                                            if (DateTime.TryParse(value, out MovingDate))
                                            {
                                                customer.MovingDate = MovingDate;
                                            }
                                            continue;
                                        }
                                        else if (header == "CustomerReferral")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "CustomerReferral: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "LastNote")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "LastNote: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "OriginalScheduledDate")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.Note += "OriginalScheduledDate: " + value + @" 
";
                                            }
                                            continue;
                                        }
                                        else if (header == "BuyoutAmount")
                                        {
                                            double BuyoutAmount = 0;
                                            if(double.TryParse(value,out BuyoutAmount))
                                            {
                                                customer.BuyoutAmountByADS = BuyoutAmount;
                                            }
                                            continue;
                                        }
                                        else if (header == "Latitude")
                                        {
                                            customer.Latlng = value;
                                            continue;
                                        }
                                        else if (header == "Longitude")
                                        {
                                            customer.Latlng += "," + value;
                                            continue;
                                        }
                                        else if (header == "MiddleName")
                                        {
                                            customer.MiddleName = value;
                                            continue;
                                        }
                                        else if (header == "QACompletedDate")
                                        {
                                            DateTime QA1Date = new DateTime();
                                            if (DateTime.TryParse(value, out QA1Date))
                                            {
                                                customer.QA1Date = QA1Date;
                                            }
                                            //else
                                            //{
                                            //    //customer.QA1Date = null;
                                            //}
                                            continue;
                                        }
                                        else if (header == "PostInstallQACompletedDate")
                                        {
                                            DateTime QA2Date = new DateTime();
                                            if (DateTime.TryParse(value, out QA2Date))
                                            {
                                                customer.QA2Date = QA2Date;
                                            }
                                            else
                                            {
                                                //customer.QA2Date = null;
                                            }
                                            continue;
                                        }
                                        else if (header == "FundingPurchasePrice")
                                        {
                                            double PurchasePrice = 0;
                                            if(double.TryParse(value,out PurchasePrice))
                                            {
                                                customer.PurchasePrice = PurchasePrice;
                                            }
                                            
                                        }
                                        else if (header == "FundingPurchaseMultiple")
                                        {
                                            //No Data
                                            continue;
                                        }
                                        else if (header == "FundingPurchaseDiscount")
                                        {
                                            //No Data
                                            continue;
                                        }
                                        #endregion
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                            }

                            if (!string.IsNullOrWhiteSpace(customer.AdditionalCustomerNo))
                            {
                                #region Insert Customer Cancell
                                if (CustomerCancel.CancelDatet != null || !string.IsNullOrWhiteSpace(CustomerCancel.CancelReason))
                                {
                                    _Util.Facade.CustomerFacade.InsertCustomerCancel(CustomerCancel);
                                    customer.IsActive = false;
                                }
                                else
                                {
                                    customer.IsActive = true;
                                }
                                #endregion

                                #region Not null Data insert
                                if (customer.ReferringCustomer == null)
                                {
                                    customer.ReferringCustomer = new Guid();
                                }
                                if (customer.ChildOf == null)
                                {
                                    customer.ChildOf = new Guid();
                                }
                                if (customer.Soldby == null)
                                {
                                    customer.Soldby = new Guid().ToString();
                                }
                                if (customer.SoldBy2 == null)
                                {
                                    customer.SoldBy2 = new Guid();
                                }
                                if (customer.SoldBy3 == null)
                                {
                                    customer.SoldBy3 = new Guid();
                                }
                                if (customer.DuplicateCustomer == null)
                                {
                                    customer.DuplicateCustomer = new Guid();
                                }
                                if (customer.AccessGivenTo == null)
                                {
                                    customer.AccessGivenTo = new Guid();
                                }
                                if (customer.AccessGivenTo == null)
                                {
                                    customer.AccessGivenTo = new Guid();
                                }
                                if (customer.CreatedByUid == null)
                                {
                                    customer.CreatedByUid = CurrentUser.UserId;
                                }
                                if (customer.LastUpdatedDate ==null || customer.LastUpdatedDate == new DateTime())
                                {
                                    customer.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                                }
                                if(customer.CreatedDate == null)
                                {
                                    customer.CreatedDate = DateTime.Now.UTCCurrentTime();
                                }
                                if(customer.JoinDate == null)
                                {
                                    customer.JoinDate = DateTime.Now.UTCCurrentTime();
                                }
                                #endregion

                                customer.LastUpdatedByUid = CurrentUser.UserId;
                                customer.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                                string Note = customer.Note;
                                customer.Note = "";

                                #region SoldBY
                                if (!string.IsNullOrWhiteSpace(SoldBy))
                                {
                                    Employee emp = employees.Where(x => x.FirstName + " " + x.LastName == SoldBy).FirstOrDefault();
                                    if (emp != null)
                                    {
                                        customer.Soldby = emp.UserId.ToString();
                                        customer.Soldby1 = emp.UserId;
                                    }
                                }
                                #endregion
                                #region Installer
                                if (!string.IsNullOrWhiteSpace(Installer))
                                {
                                    Employee emp = employees.Where(x => x.FirstName + " " + x.LastName == Installer).FirstOrDefault();
                                    if (emp != null)
                                    {
                                        customer.Installer = emp.UserId.ToString();
                                    }
                                }
                                #endregion

                                customer.Id = (int)_Util.Facade.CustomerFacade.InsertCustomer(customer);
                                customer.Note = Note;
                                int SableId = 0;
                                int.TryParse(customer.AdditionalCustomerNo, out SableId);

                                #region CustomerMigration
                                CustomerMigration CM = new CustomerMigration() {
                                    CompanyId = CurrentUser.CompanyId.Value,
                                    Note = customer.Note,
                                    CreatedBy = customer.CreatedByUid,
                                    CreatedDate = customer.CreatedDate,
                                    CustomerId = customer.CustomerId,
                                    Platform = PlatForm,
                                    RefenrenceId = SableId, 
                                };
                                _Util.Facade.CustomerFacade.InsertCustomerMigration(CM);
                                #endregion

                                #region Customer Company Insert
                                if (CC.IsLead == false)
                                {
                                    CC.ConvertionDate = customer.JoinDate;
                                    CC.IsActive = true;
                                }
                                _Util.Facade.CustomerFacade.InsertCustomerCompany(CC);
                                #endregion

                                #region CustomerNote Insert
                                if (!string.IsNullOrWhiteSpace(customer.Note))
                                {
                                    CustomerNote note = new CustomerNote()
                                    {
                                        Notes = customer.Note,
                                        CustomerId = customer.CustomerId,
                                        CompanyId= CC.CompanyId,
                                        CreatedDate = customer.CreatedDate,
                                        IsEmail = false,
                                        IsText = false,
                                        IsShedule = false,
                                        IsFollowUp = false,
                                        IsActive = true,
                                        CreatedBy ="SystemUser",
                                        IsClose = false,
                                        IsAllDay = false,
                                        CreatedByUid = new Guid("22222222-2222-2222-2222-222222222222"),
                                        IsPin = false
                                    };
                                    _Util.Facade.NotesFacade.InsertCustomerNote(note);
                                }

                                #endregion


                                #region Payment info Insert
                                PI.CompanyId = CurrentUser.CompanyId.Value;

                                #endregion

                            }
                        }
                        catch (Exception ex)
                        {
                            logger.Error(ex);
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\ADS Customer Import Err.txt"), true))
                            {
                                file.WriteLine("Error AT "+customer.Id+" "+customer.AdditionalCustomerNo);
                                file.Close();
                            }
                        }
                    }
                }
            }

            return Json(1);
        }



        #region ADS AGEMNI CUSTOMER IMPORT

        public JsonResult ADSAgemniCustomerImport(string File, string isCustomer)
        {
            string subPath = "~/LeadImportReports"; // your code goes here

            bool exists = System.IO.Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath(subPath));
            if (!exists)
                System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath(subPath));
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var serverFile = Server.MapPath(File);
            if (System.IO.File.Exists(serverFile))
            {
                FileInfo ExcelFile = new FileInfo(serverFile);
                if (ExcelFile.Length > 0)
                {
                    var workbook = new Excel.XLWorkbook(ExcelFile.FullName);
                    Excel.IXLWorksheet workSheet = workbook.Worksheet(1);
                    var xlRange = workSheet.RangeUsed();
                    int rowCount = xlRange.Rows().Count();
                    int colCount = xlRange.Cells().Count();
                    var LookUpList = _Util.Facade.LookupFacade.GetAllLookup();
                    List<Employee> employees = _Util.Facade.EmployeeFacade.GetAllEmployee();
                    for (int i = 2; i <= rowCount; i++)
                    {
                        string SoldBy = "";
                        string Installer = "";

                        Customer customer = new Customer();
                        customer.CustomerId = Guid.NewGuid();

                        CustomerCompany CC = new CustomerCompany();
                        CC.CompanyId = CurrentUser.CompanyId.Value;
                        CC.CustomerId = customer.CustomerId;
                        //CC.IsLead = false;

                        //CustomerCancel CustomerCancel = new CustomerCancel();
                        //CustomerCancel.CompanyId = CurrentUser.CompanyId.Value;
                        //CustomerCancel.CustomerId = customer.CustomerId;
                        //CustomerCancel.EmployeeId = new Guid("22222222-2222-2222-2222-222222222222");
                        //CustomerCancel.IsActivated = true;

                        //PaymentInfoCustomer PIC = new PaymentInfoCustomer();
                        //PaymentProfileCustomer PPC = new PaymentProfileCustomer();
                        //PaymentInfo PI = new PaymentInfo();

                        try
                        {
                            for (int j = 1; j <= colCount; j++)
                            {
                                if (xlRange.Cell(i, j) != null && xlRange.Cell(i, j).Value != null)
                                {
                                    try
                                    {
                                        #region Migration Conditions
                                        var value = xlRange.Cell(i, j).Value.ToString();
                                        var header = xlRange.Cell(1, j).Value.ToString();
                                        if (string.IsNullOrWhiteSpace(header))
                                        {
                                            break;
                                        }
                                        else if (header == "Name")
                                        {
                                            var parts = value.Split(' ');
                                            string lastName = parts.LastOrDefault();
                                            string firstName = string.Join(" ", parts.Take(parts.Length - 1));
                                            if(firstName==null || firstName=="")
                                            {
                                                firstName = lastName;
                                                lastName = "";
                                            }
                                            customer.FirstName = firstName;
                                            customer.LastName = lastName;
                                            continue;
                                        }
                                        else if (header == "Type")
                                        {
                                            if(value== "Customer")
                                            {
                                                //customer.IsLead = false;
                                                CC.IsLead = false;
                                            }
                                            else
                                            {
                                                //customer.IsLead = true;
                                                CC.IsLead = true;
                                            }
                                            continue;
                                        }
                                        else if (header == "CustomerID")
                                        {
                                            customer.AdditionalCustomerNo = value;
                                            continue;
                                        }
                                        else if (header == "WoType-Status")
                                        {
                                            customer.Status = value;
                                            continue;
                                        }
                                        else if (header == "Street")
                                        {
                                            customer.Street = value;
                                            continue;
                                        }
                                        else if (header == "City")
                                        {
                                            customer.City = value;
                                            continue;
                                        }
                                        else if (header == "State")
                                        {
                                            customer.State = value;
                                            continue;
                                        }
                                        else if (header == "ZipCode")
                                        {
                                            customer.ZipCode = value;
                                            continue;
                                        }
                                        else if (header == "Phone")
                                        {
                                            customer.PrimaryPhone = value;
                                            continue;
                                        }
                                        else if (header == "Date")
                                        {
                                            DateTime InstallDate = new DateTime();
                                            if (DateTime.TryParse(value, out InstallDate))
                                            {
                                                customer.CreatedDate = InstallDate;
                                                customer.JoinDate = InstallDate;
                                            }
                                            continue;
                                        }
                                        else if (header == "LeadRep")
                                        {
                                            customer.Note = "SalesRep:" + value;
                                            continue;
                                        }
                                        
                                        #endregion
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                            }

                            if (!string.IsNullOrWhiteSpace(customer.AdditionalCustomerNo))
                            {
                                #region Insert Customer Cancell
                                
                                customer.IsActive = true;
                               
                                #endregion

                                #region Not null Data insert
                                if (customer.ReferringCustomer == null)
                                {
                                    customer.ReferringCustomer = new Guid();
                                }
                                if (customer.ChildOf == null)
                                {
                                    customer.ChildOf = new Guid();
                                }
                                if (customer.Soldby == null)
                                {
                                    customer.Soldby = new Guid().ToString();
                                }
                                if (customer.SoldBy2 == null)
                                {
                                    customer.SoldBy2 = new Guid();
                                }
                                if (customer.SoldBy3 == null)
                                {
                                    customer.SoldBy3 = new Guid();
                                }
                                if (customer.DuplicateCustomer == null)
                                {
                                    customer.DuplicateCustomer = new Guid();
                                }
                                if (customer.AccessGivenTo == null)
                                {
                                    customer.AccessGivenTo = new Guid();
                                }
                                if (customer.AccessGivenTo == null)
                                {
                                    customer.AccessGivenTo = new Guid();
                                }
                                if (customer.CreatedByUid == null)
                                {
                                    customer.CreatedByUid = CurrentUser.UserId;
                                }
                                if (customer.LastUpdatedDate == null || customer.LastUpdatedDate == new DateTime())
                                {
                                    customer.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                                }
                                if (customer.CreatedDate == null)
                                {
                                    customer.CreatedDate = DateTime.Now.UTCCurrentTime();
                                }
                                if (customer.JoinDate == null)
                                {
                                    customer.JoinDate = DateTime.Now.UTCCurrentTime();
                                }
                                #endregion
                                customer.CreatedByUid = new Guid("22222222-2222-2222-2222-222222222222");
                                customer.LastUpdatedByUid = CurrentUser.UserId;
                                customer.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                                //string Note = customer.Note;
                                //customer.Note = "";

                                #region SoldBY
                                if (!string.IsNullOrWhiteSpace(SoldBy))
                                {
                                    Employee emp = employees.Where(x => x.FirstName + " " + x.LastName == SoldBy).FirstOrDefault();
                                    if (emp != null)
                                    {
                                        customer.Soldby = emp.UserId.ToString();
                                    }
                                }
                                #endregion
                                #region Installer
                                if (!string.IsNullOrWhiteSpace(Installer))
                                {
                                    Employee emp = employees.Where(x => x.FirstName + " " + x.LastName == Installer).FirstOrDefault();
                                    if (emp != null)
                                    {
                                        customer.Installer = emp.UserId.ToString();
                                    }
                                }
                                #endregion

                                customer.Id = (int)_Util.Facade.CustomerFacade.InsertCustomer(customer);
                                //customer.Note = Note;
                                int SableId = 0;
                                int.TryParse(customer.AdditionalCustomerNo, out SableId);

                                #region CustomerMigration
                                CustomerMigration CM = new CustomerMigration()
                                {
                                    CompanyId = CurrentUser.CompanyId.Value,
                                    Note = customer.Note,
                                    CreatedBy = customer.CreatedByUid,
                                    CreatedDate = customer.CreatedDate,
                                    CustomerId = customer.CustomerId,
                                    Platform = "Agemni",
                                    RefenrenceId = SableId,
                                };
                                _Util.Facade.CustomerFacade.InsertCustomerMigration(CM);
                                #endregion

                                #region Customer Company Insert
                                if (CC.IsLead == false)
                                {
                                    CC.ConvertionDate = customer.JoinDate;
                                    CC.IsActive = true;
                                }
                                _Util.Facade.CustomerFacade.InsertCustomerCompany(CC);
                                #endregion

                                #region CustomerNote Insert
                                //if (!string.IsNullOrWhiteSpace(customer.Note))
                                //{
                                //    CustomerNote note = new CustomerNote()
                                //    {
                                //        Notes = customer.Note,
                                //        CustomerId = customer.CustomerId,
                                //        CompanyId = CC.CompanyId,
                                //        CreatedDate = customer.CreatedDate,
                                //        IsEmail = false,
                                //        IsText = false,
                                //        IsShedule = false,
                                //        IsFollowUp = false,
                                //        IsActive = true,
                                //        CreatedBy = "SystemUser",
                                //        IsClose = false,
                                //        IsAllDay = false,
                                //        CreatedByUid = new Guid("22222222-2222-2222-2222-222222222222"),
                                //        IsPin = false
                                //    };
                                //    _Util.Facade.NotesFacade.InsertCustomerNote(note);
                                //}

                                #endregion


                                #region Payment info Insert
                                //PI.CompanyId = CurrentUser.CompanyId.Value;

                                #endregion

                            }
                        }
                        catch (Exception)
                        {
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\ADS Customer Import Err.txt"), true))
                            {
                                file.WriteLine("Error AT " + customer.Id + " " + customer.AdditionalCustomerNo);
                                file.Close();
                            }
                        }
                    }
                }
            }

            return Json(1);
        }

        #endregion



        #region ADS Billing And Cases
        public JsonResult ADSBillingImport(string File)
        {
            string subPath = "~/LeadImportReports"; // your code goes here

            bool exists = System.IO.Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath(subPath));
            if (!exists)
                System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath(subPath));
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var serverFile = Server.MapPath(File);
            Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentUser.UserId);

            List<CustomerMigration> customerMigrations = _Util.Facade.CustomerFacade.GetAllCustomerMigration("ADS");
            if (customerMigrations!= null && customerMigrations.Count > 0)
            {
                if (System.IO.File.Exists(serverFile))
                {
                    FileInfo ExcelFile = new FileInfo(serverFile);
                    if (ExcelFile.Length > 0)
                    {
                        var workbook = new Excel.XLWorkbook(ExcelFile.FullName);
                        Excel.IXLWorksheet workSheet = workbook.Worksheet(1);
                        var xlRange = workSheet.RangeUsed();
                        int rowCount = xlRange.Rows().Count();
                        int colCount = xlRange.Cells().Count();
                        var LookUpList = _Util.Facade.LookupFacade.GetAllLookup();

                        for (int i = 2; i <= rowCount; i++)
                        {
                            //Customer Customer = new Customer();
                            //Customer.CustomerId = new Guid();
                            CustomerMigration Customer = new CustomerMigration();
                            Customer.CustomerId = new Guid();

                            CustomerNote CustomerNote = new CustomerNote();
                            CustomerNote.CompanyId = CurrentUser.CompanyId.Value;
                            int SableId = 0;

                            try
                            {
                                for (int j = 1; j <= colCount; j++)
                                {
                                    if (xlRange.Cell(i, j) != null && xlRange.Cell(i, j).Value != null)
                                    {
                                        try
                                        {
                                            #region Migration Conditions
                                            var value = xlRange.Cell(i, j).Value.ToString();
                                            var header = xlRange.Cell(1, j).Value.ToString();
                                            if (string.IsNullOrWhiteSpace(header))
                                            {
                                                break;
                                            }
                                            else if (header == "CustomerAccountID")
                                            {
                                                int AcctID = 0;

                                                if (!string.IsNullOrWhiteSpace(value) && int.TryParse(value, out AcctID) && AcctID > 0)
                                                {
                                                    //Customer = _Util.Facade.CustomerFacade.GetCustomerByAdditionalCustomerNo(value);
                                                    //Customer = _Util.Facade.CustomerFacade.GetCustomerMigrationByReferenceId(value);
                                                    Customer = customerMigrations.Where(x => x.RefenrenceId == AcctID).FirstOrDefault();

                                                    if (Customer != null && Customer.CustomerId != Guid.Empty)
                                                    {
                                                        Customer.CustomerId = Customer.CustomerId;
                                                        int.TryParse(value, out SableId);
                                                    }
                                                    else
                                                    {
                                                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\ADS Customer Not Found on Billing Report.txt"), true))
                                                        {
                                                            file.WriteLine("Customer Not Found: " + value);
                                                            file.Close();
                                                        }
                                                        break;
                                                    }
                                                }
                                                continue;
                                            }
                                            else if (header == "City")
                                            {
                                                if (!string.IsNullOrWhiteSpace(value))
                                                {
                                                    CustomerNote.Notes += "City: " + value + @"
";
                                                }
                                                continue;
                                            }
                                            else if (header == "State")
                                            {
                                                if (!string.IsNullOrWhiteSpace(value))
                                                {
                                                    CustomerNote.Notes += "State: " + value + @"
";
                                                }
                                                continue;
                                            }
                                            else if (header == "County")
                                            {
                                                if (!string.IsNullOrWhiteSpace(value))
                                                {
                                                    CustomerNote.Notes += "County: " + value + @"
";
                                                }
                                                continue;
                                            }
                                            else if (header == "AccountHolderID")
                                            {
                                                if (!string.IsNullOrWhiteSpace(value))
                                                {
                                                    CustomerNote.Notes += "AccountHolderID: " + value + @"
";
                                                }
                                                continue;
                                            }
                                            else if (header == "AccountHolder")
                                            {
                                                if (!string.IsNullOrWhiteSpace(value))
                                                {
                                                    CustomerNote.Notes += "AccountHolder: " + value + @"
";
                                                }
                                                continue;
                                            }
                                            else if (header == "BillingMethod")
                                            {
                                                if (!string.IsNullOrWhiteSpace(value))
                                                {
                                                    CustomerNote.Notes += "BillingMethod: " + value + @"
";
                                                }
                                                continue;
                                            }
                                            else if (header == "BillingTransactionType")
                                            {
                                                if (!string.IsNullOrWhiteSpace(value))
                                                {
                                                    CustomerNote.Notes += "BillingTransactionType: " + value + @"
";
                                                }
                                                continue;
                                            }
                                            else if (header == "BillingCategory")
                                            {
                                                if (!string.IsNullOrWhiteSpace(value))
                                                {
                                                    CustomerNote.Notes += "BillingCategory: " + value + @"
";
                                                }
                                                continue;
                                            }
                                            else if (header == "TransactionAmount")
                                            {
                                                if (!string.IsNullOrWhiteSpace(value))
                                                {
                                                    CustomerNote.Notes += "TransactionAmount: " + value + @"
";
                                                }
                                                continue;
                                            }
                                            else if (header == "CustomerName")
                                            {
                                                if (!string.IsNullOrWhiteSpace(value))
                                                {
                                                    CustomerNote.Notes += "CustomerName: " + value + @"
";
                                                }
                                                continue;
                                            }
                                            else if (header == "ProcessorTransactionID")
                                            {
                                                if (!string.IsNullOrWhiteSpace(value))
                                                {
                                                    CustomerNote.Notes += "ProcessorTransactionID: " + value + @"
";
                                                }
                                                continue;
                                            }
                                            else if (header == "ProcessorResultCode")
                                            {
                                                if (!string.IsNullOrWhiteSpace(value))
                                                {
                                                    CustomerNote.Notes += "ProcessorResultCode: " + value + @"
";
                                                }
                                                continue;
                                            }
                                            else if (header == "ProcessorResultCodeText")
                                            {
                                                if (!string.IsNullOrWhiteSpace(value))
                                                {
                                                    CustomerNote.Notes += "ProcessorResultCodeText: " + value + @"
";
                                                }
                                                continue;
                                            }
                                            else if (header == "ProcessorResponseReasonCode")
                                            {
                                                if (!string.IsNullOrWhiteSpace(value))
                                                {
                                                    CustomerNote.Notes += "ProcessorResponseReasonCode: " + value + @"
";
                                                }
                                                continue;
                                            }
                                            else if (header == "ProcessorResponseReasonCodeText")
                                            {
                                                if (!string.IsNullOrWhiteSpace(value))
                                                {
                                                    CustomerNote.Notes += "ProcessorResponseReasonCodeText: " + value + @"
";
                                                }
                                                continue;
                                            }
                                            else if (header == "PaymentProcessResultCode")
                                            {
                                                if (!string.IsNullOrWhiteSpace(value))
                                                {
                                                    CustomerNote.Notes += "PaymentProcessResultCode: " + value + @"
";
                                                }
                                                continue;
                                            }
                                            else if (header == "ProcessorResultMessage")
                                            {
                                                if (!string.IsNullOrWhiteSpace(value))
                                                {
                                                    CustomerNote.Notes += "ProcessorResultMessage: " + value + @"
";
                                                }
                                                continue;
                                            }
                                            else if (header == "ProcessorAuthCode")
                                            {
                                                if (!string.IsNullOrWhiteSpace(value))
                                                {
                                                    CustomerNote.Notes += "ProcessorAuthCode: " + value + @"
";
                                                }
                                                continue;
                                            }
                                            else if (header == "TransactionDate")
                                            {
                                                if (!string.IsNullOrWhiteSpace(value))
                                                {
                                                    CustomerNote.Notes += "TransactionDate: " + value + @"
";
                                                }
                                                continue;
                                            }
                                            else if (header == "GeneratedBy")
                                            {
                                                if (!string.IsNullOrWhiteSpace(value))
                                                {
                                                    CustomerNote.Notes += "GeneratedBy: " + value;
                                                }
                                                continue;
                                            }
                                            #endregion
                                        }
                                        catch (Exception)
                                        {

                                        }
                                    }
                                }

                                if ( Customer!=null && Customer.CustomerId != new Guid())
                                {
                                    #region CustomerNote Insert
                                    if (!string.IsNullOrWhiteSpace(CustomerNote.Notes))
                                    {
                                        CustomerNote note = new CustomerNote()
                                        {
                                            Notes = @"Billing Note: 
" + CustomerNote.Notes,
                                            CustomerId = Customer.CustomerId,
                                            CompanyId = CurrentUser.CompanyId.Value,
                                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                                            IsEmail = false,
                                            IsText = false,
                                            IsShedule = false,
                                            IsFollowUp = false,
                                            IsActive = true,
                                            CreatedBy = emp.FirstName + " " + emp.LastName,
                                            IsClose = false,
                                            IsAllDay = false,
                                            CreatedByUid = CurrentUser.UserId,
                                            IsPin = false,
                                            IsOverview = false
                                        };
                                        _Util.Facade.NotesFacade.InsertCustomerNote(note);
                                    }

                                    #endregion
                                }
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }
                }
            }
            return Json(1);
        }
        public JsonResult ADSCasesImport(string File)
        {
            string subPath = "~/LeadImportReports"; // your code goes here

            bool exists = System.IO.Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath(subPath));
            if (!exists)
                System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath(subPath));
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var serverFile = Server.MapPath(File);
            if (System.IO.File.Exists(serverFile))
            {
                FileInfo ExcelFile = new FileInfo(serverFile);
                if (ExcelFile.Length > 0)
                {
                    var workbook = new Excel.XLWorkbook(ExcelFile.FullName);
                    Excel.IXLWorksheet workSheet = workbook.Worksheet(1);
                    var xlRange = workSheet.RangeUsed();
                    int rowCount = xlRange.Rows().Count();
                    int colCount = xlRange.Cells().Count();
                    var LookUpList = _Util.Facade.LookupFacade.GetAllLookup();
                    Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentUser.UserId);
                    List<CustomerMigration> customerMigrations = _Util.Facade.CustomerFacade.GetAllCustomerMigration("ADS");

                    for (int i = 2; i <= rowCount; i++)
                    {
                        //Customer Customer = new Customer();
                        //Customer.CustomerId = new Guid();

                        CustomerMigration customer = new CustomerMigration();
                        customer.CustomerId = new Guid();

                        CustomerNote CustomerNote = new CustomerNote();
                        CustomerNote.CompanyId = CurrentUser.CompanyId.Value;
                        int SableId = 0;
                        
                        try
                        {
                            for (int j = 1; j <= colCount; j++)
                            {
                                if (xlRange.Cell(i, j) != null && xlRange.Cell(i, j).Value != null)
                                {
                                    try
                                    {
                                        #region Migration Conditions
                                        var value = xlRange.Cell(i, j).Value.ToString();
                                        var header = xlRange.Cell(1, j).Value.ToString();
                                        if (string.IsNullOrWhiteSpace(header))
                                        {
                                            break;
                                        }
                                        else if (header == "AccountId")
                                        {
                                            int AcctID = 0;

                                            if (!string.IsNullOrWhiteSpace(value) && int.TryParse(value, out AcctID) && AcctID > 0)
                                            {
                                                //customer = _Util.Facade.CustomerFacade.GetCustomerByAdditionalCustomerNo(value);
                                                customer = customerMigrations.Where(x => x.RefenrenceId == AcctID).FirstOrDefault();

                                                if (customer != null)
                                                {
                                                    customer.CustomerId = customer.CustomerId;
                                                    int.TryParse(value, out SableId);
                                                }
                                                else
                                                {
                                                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\ADS Customer Not Found on Cases Report.txt"), true))
                                                    {
                                                        file.WriteLine("Customer Not Found: " + value);
                                                        file.Close();
                                                    }
                                                    break;
                                                }
                                            }
                                            continue;
                                        }
                                        else if (header == "Monitoring")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "Monitoring: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "AreaId")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "AreaId: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "AreaName")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "AreaName: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "CustomerFirstName")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "CustomerFirstName: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "CustomerLastName")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "CustomerLastName: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "Address1")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "Address1: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "Address2")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "Address2: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "City")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "City: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "State")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "State: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "County")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "County: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "Zip")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "Zip: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "AccountHolder")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "AccountHolder: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "IsAccountHold")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "IsAccountHold: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "CaseType")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "CaseType: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "IsServiceCaseType")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "IsServiceCaseType: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "CaseReason")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "CaseReason: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "CaseQueue")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "CaseQueue: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "CustomerName")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "CustomerName: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "CustomerPhone1")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "CustomerPhone1: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "InstallDate")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "InstallDate: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "SaleDate")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "SaleDate: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "DateOpened")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "DateOpened: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "DateReviewed")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "DateReviewed: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "DateResolved")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "DateResolved: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "TechActualArrivalDate")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "TechActualArrivalDate: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "TechActualDepartureDate")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "TechActualDepartureDate: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "TechMileage")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "TechMileage: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "TripFeeAmount")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "TripFeeAmount: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "WaveTripFee")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "WaveTripFee: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "DaysOpen")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "DaysOpen: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "AccountStatus")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "AccountStatus: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "AssignedToUserName")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "AssignedToUserName: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "SalesRep")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "SalesRep: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "Technician")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "Technician: " + value + @"
";

                                            }
                                            continue;
                                        }
                                        else if (header == "AssignedToPosition")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "AssignedToPosition: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "OpenedBy")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "OpenedBy: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "ReviewedBy")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "ReviewedBy: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "ResolvedBy")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "ResolvedBy: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "CustomerCaseStatusId")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "CustomerCaseStatusId: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "CaseStatus")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "CaseStatus: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "CustomerCaseCustomStatusId")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "CustomerCaseCustomStatusId: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "CustomStatusName")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "CustomStatusName: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "ScheduledBy")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "ScheduledBy: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "DateScheduled")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "DateScheduled: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "ThirdPartyJobNumber")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "ThirdPartyJobNumber: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "FirstNote")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "FirstNote: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "LastNote")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "LastNote: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "DateLastUpdated")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "DateLastUpdated: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "NewEquipmentList")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "NewEquipmentList: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "NewEquipmentTotalCost")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "NewEquipmentTotalCost: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "TotalSupplierPrice")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "TotalSupplierPrice: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "Funded_Date")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "Funded_Date: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "OpenHolds")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "OpenHolds: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "OpenCases")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "OpenCases: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "CaseCategory")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "CaseCategory: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "ThirdPartyCauseCode")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "ThirdPartyCauseCode: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "ThirdPartyProblemCode")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "ThirdPartyProblemCode: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "ThirdPartyIsSignalsChecked")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "ThirdPartyIsSignalsChecked: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "BillableCompany")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "BillableCompany: " + value + @"
";
                                            }
                                            continue;
                                        }
                                        else if (header == "SecondaryAssignedToUserName")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "SecondaryAssignedToUserName: " + value;
                                            }
                                            continue;
                                        }
                                        #endregion
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                            }

                            if (customer != null &&  customer.CustomerId != new Guid())
                            {
                                #region CustomerNote Insert
                                if (!string.IsNullOrWhiteSpace(CustomerNote.Notes))
                                {
                                    CustomerNote note = new CustomerNote()
                                    {
                                        Notes = CustomerNote.Notes,
                                        CustomerId = customer.CustomerId,
                                        CompanyId = CurrentUser.CompanyId.Value,
                                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                                        IsEmail = false,
                                        IsText = false,
                                        IsShedule = false,
                                        IsFollowUp = false,
                                        IsActive = true,
                                        CreatedBy = emp.FirstName + " " + emp.LastName,
                                        IsClose = false,
                                        IsAllDay = false,
                                        CreatedByUid = CurrentUser.UserId,
                                        IsPin = false,
                                        IsOverview = false
                                    };
                                    _Util.Facade.NotesFacade.InsertCustomerNote(note);
                                }

                                #endregion
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
            return Json(1);
        }
        #endregion

        #region Point Cases And Notes
        public JsonResult PointTier32CasesImport(string File)
        {
            string subPath = "~/LeadImportReports"; // your code goes here

            bool exists = System.IO.Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath(subPath));
            if (!exists)
                System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath(subPath));
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var serverFile = Server.MapPath(File);
            if (System.IO.File.Exists(serverFile))
            {
                FileInfo ExcelFile = new FileInfo(serverFile);
                if (ExcelFile.Length > 0)
                {
                    var workbook = new Excel.XLWorkbook(ExcelFile.FullName);
                    Excel.IXLWorksheet workSheet = workbook.Worksheet(1);
                    var xlRange = workSheet.RangeUsed();
                    int rowCount = xlRange.Rows().Count();
                    int colCount = xlRange.Cells().Count();
                    var LookUpList = _Util.Facade.LookupFacade.GetAllLookup();
                    Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentUser.UserId);
                    List<CustomerMigration> CustomerMigrations = _Util.Facade.CustomerFacade.GetAllCustomerMigration("Point Tier32");
                    for (int i = 2; i <= rowCount; i++)
                    {
                        //Customer Customer = new Customer();
                        //Customer.CustomerId = new Guid();

                        CustomerMigration Customer = new CustomerMigration();
                        Customer.CustomerId = new Guid();

                        CustomerNote CustomerNote = new CustomerNote();
                        CustomerNote.CompanyId = CurrentUser.CompanyId.Value;
                        int SableId = 0;
                        
                        try
                        {
                            for (int j = 1; j <= colCount; j++)
                            {
                                if (xlRange.Cell(i, j) != null && xlRange.Cell(i, j).Value != null)
                                {
                                    try
                                    {
                                        #region Migration Conditions
                                        var value = xlRange.Cell(i, j).Value.ToString();
                                        var header = xlRange.Cell(1, j).Value.ToString();
                                        if (string.IsNullOrWhiteSpace(header))
                                        {
                                            break;
                                        }
                                        else if (header == "AccountId")
                                        {
                                            int AcctId = 0;
                                            if (!string.IsNullOrWhiteSpace(value)&&int.TryParse(value,out AcctId) && AcctId >0)
                                            {


                                                //Customer = _Util.Facade.CustomerFacade.GetCustomerByAdditionalCustomerNo(value);
                                                Customer = CustomerMigrations.Where(x => x.RefenrenceId == AcctId).FirstOrDefault();

                                                if (Customer != null)
                                                {
                                                    Customer.CustomerId = Customer.CustomerId;
                                                    int.TryParse(value, out SableId);
                                                }
                                                else
                                                {
                                                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\Point Customer Not Found on Cases Report.txt"), true))
                                                    {
                                                        file.WriteLine("Customer Not Found: " + value);
                                                        file.Close();
                                                    }
                                                    break;
                                                }
                                            }
                                            continue;
                                        }
                                        #region Other Conditions
                                        else if (header == "Monitoring")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "Monitoring: " + value + @"\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "AreaId")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "AreaId: " + value + @"\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "AreaName")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "AreaName: " + value + @"\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "CustomerFirstName")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "CustomerFirstName: " + value + @"\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "CustomerLastName")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "CustomerLastName: " + value + @"\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "Address1")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "Address1: " + value + @"\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "Address2")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "Address2: " + value + @"\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "City")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "City: " + value + @"\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "State")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "State: " + value + @"\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "County")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "County: " + value + @"\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "Zip")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "Zip: " + value + @"\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "AccountHolder")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "AccountHolder: " + value + @"\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "IsAccountHold")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "IsAccountHold: " + value + @"\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "CaseType")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "CaseType: " + value + @"\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "IsServiceCaseType")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "IsServiceCaseType: " + value + @"\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "CaseReason")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "CaseReason: " + value + @"\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "CaseQueue")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "CaseQueue: " + value + @"\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "CustomerName")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "CustomerName: " + value + @"\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "CustomerPhone1")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "CustomerPhone1: " + value + @"\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "InstallDate")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "InstallDate: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "SaleDate")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "SaleDate: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "DateOpened")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "DateOpened: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "DateReviewed")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "DateReviewed: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "DateResolved")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "DateResolved: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "TechActualArrivalDate")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "TechActualArrivalDate: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "TechActualDepartureDate")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "TechActualDepartureDate: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "TechMileage")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "TechMileage: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "TripFeeAmount")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "TripFeeAmount: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "WaveTripFee")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "WaveTripFee: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "DaysOpen")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "DaysOpen: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "AccountStatus")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "AccountStatus: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "AssignedToUserName")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "AssignedToUserName: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "SalesRep")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "SalesRep: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "Technician")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "Technician: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "AssignedToPosition")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "AssignedToPosition: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "OpenedBy")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "OpenedBy: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "ReviewedBy")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "ReviewedBy: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "ResolvedBy")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "ResolvedBy: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "CustomerCaseStatusId")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "CustomerCaseStatusId: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "CaseStatus")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "CaseStatus: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "CustomerCaseCustomStatusId")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "CustomerCaseCustomStatusId: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "CustomStatusName")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "CustomStatusName: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "ScheduledBy")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "ScheduledBy: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "DateScheduled")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "DateScheduled: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "ThirdPartyJobNumber")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "ThirdPartyJobNumber: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "FirstNote")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "FirstNote: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "LastNote")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "LastNote: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "DateLastUpdated")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "DateLastUpdated: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "NewEquipmentList")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "NewEquipmentList: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "NewEquipmentTotalCost")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "NewEquipmentTotalCost: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "TotalSupplierPrice")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "TotalSupplierPrice: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "Funded_Date")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "Funded_Date: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "OpenHolds")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "OpenHolds: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "OpenCases")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "OpenCases: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "CaseCategory")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "CaseCategory: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "ThirdPartyCauseCode")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "ThirdPartyCauseCode: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "ThirdPartyProblemCode")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "ThirdPartyProblemCode: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "ThirdPartyIsSignalsChecked")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "ThirdPartyIsSignalsChecked: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "BillableCompany")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "BillableCompany: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "SecondaryAssignedToUserName")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "SecondaryAssignedToUserName: " + value;
                                            }
                                            continue;
                                        }
                                        #endregion
                                        #endregion
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                            }

                            if (Customer != null && Customer.CustomerId != new Guid())
                            { 
                                #region CustomerNote Insert
                                if (!string.IsNullOrWhiteSpace(CustomerNote.Notes))
                                {
                                    CustomerNote note = new CustomerNote()
                                    {
                                        Notes = CustomerNote.Notes,
                                        CustomerId = Customer.CustomerId,
                                        CompanyId = CurrentUser.CompanyId.Value,
                                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                                        IsEmail = false,
                                        IsText = false,
                                        IsShedule = false,
                                        IsFollowUp = false,
                                        IsActive = true,
                                        CreatedBy = emp.FirstName + " " + emp.LastName,
                                        IsClose = false,
                                        IsAllDay = false,
                                        CreatedByUid = CurrentUser.UserId,
                                        IsPin = false,
                                        IsOverview = false
                                    };
                                    _Util.Facade.NotesFacade.InsertCustomerNote(note);
                                }

                                #endregion
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
            return Json(1);
        }
        public JsonResult PointTier32NotesImport(string File)
        {
            string subPath = "~/LeadImportReports"; // your code goes here

            bool exists = System.IO.Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath(subPath));
            if (!exists)
                System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath(subPath));
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var serverFile = Server.MapPath(File);
            if (System.IO.File.Exists(serverFile))
            {
                FileInfo ExcelFile = new FileInfo(serverFile);
                if (ExcelFile.Length > 0)
                {
                    var workbook = new Excel.XLWorkbook(ExcelFile.FullName);
                    Excel.IXLWorksheet workSheet = workbook.Worksheet(1);
                    var xlRange = workSheet.RangeUsed();
                    int rowCount = xlRange.Rows().Count();
                    int colCount = xlRange.Cells().Count();
                    var LookUpList = _Util.Facade.LookupFacade.GetAllLookup();
                    List<CustomerMigration> customerMigrations = _Util.Facade.CustomerFacade.GetAllCustomerMigration("Point Tier32");
                    Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentUser.UserId);

                    for (int i = 2; i <= rowCount; i++)
                    {
                        //Customer Customer = new Customer();
                        //Customer.CustomerId = new Guid();
                        CustomerMigration Customer = new CustomerMigration();
                        Customer.CustomerId = new Guid();

                        CustomerNote CustomerNote = new CustomerNote();
                        CustomerNote.CompanyId = CurrentUser.CompanyId.Value;
                        int SableId = 0;
                        
                        try
                        {
                            for (int j = 1; j <= colCount; j++)
                            {
                                if (xlRange.Cell(i, j) != null && xlRange.Cell(i, j).Value != null)
                                {
                                    try
                                    {
                                        #region Migration Conditions
                                        var value = xlRange.Cell(i, j).Value.ToString();
                                        var header = xlRange.Cell(1, j).Value.ToString();
                                        if (string.IsNullOrWhiteSpace(header))
                                        {
                                            break;
                                        }
                                        else if (header == "AccountId")
                                        {
                                            int AcctId = 0;

                                            if (!string.IsNullOrWhiteSpace(value) && int.TryParse(value,out AcctId) && AcctId> 0)
                                            {
                                                //Customer = _Util.Facade.CustomerFacade.GetCustomerByAdditionalCustomerNo(value);
                                                Customer  = customerMigrations.Where(x=>x.RefenrenceId == AcctId).FirstOrDefault();

                                                if (Customer != null && Customer.CustomerId != Guid.Empty)
                                                {
                                                    Customer.CustomerId = Customer.CustomerId;
                                                    int.TryParse(value, out SableId);
                                                }
                                                else
                                                {
                                                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\Point Customer Not Found on Notes Report.txt"), true))
                                                    {
                                                        file.WriteLine("Customer Not Found: " + value);
                                                        file.Close();
                                                    }
                                                    break;
                                                }
                                            }
                                            continue;
                                        }
                                        else if (header == "Note")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "Note: " + value + "@\n";
                                            }
                                            continue;
                                        }
                                        else if (header == "Date")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes += "Date: " + value;
                                            }
                                            continue;
                                        }
                                        #endregion
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                            }

                            if (Customer != null && Customer.CustomerId != new Guid())
                            {
                                #region CustomerNote Insert
                                if (!string.IsNullOrWhiteSpace(CustomerNote.Notes))
                                {
                                    CustomerNote note = new CustomerNote()
                                    {
                                        Notes = CustomerNote.Notes,
                                        CustomerId = Customer.CustomerId,
                                        CompanyId = CurrentUser.CompanyId.Value,
                                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                                        IsEmail = false,
                                        IsText = false,
                                        IsShedule = false,
                                        IsFollowUp = false,
                                        IsActive = true,
                                        CreatedBy = emp.FirstName + " " + emp.LastName,
                                        IsClose = false,
                                        IsAllDay = false,
                                        CreatedByUid = CurrentUser.UserId,
                                        IsPin = false,
                                        IsOverview = false
                                    };
                                    _Util.Facade.NotesFacade.InsertCustomerNote(note);
                                }

                                #endregion
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
            return Json(1);
        }

        #endregion

        #region CustomerNotes Data
        public JsonResult CustomerNoteImport(string File,string Platform)
        {
            string subPath = "~/LeadImportReports"; // your code goes here

            bool exists = System.IO.Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath(subPath));
            if (!exists)
                System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath(subPath));
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var serverFile = Server.MapPath(File);
            if (System.IO.File.Exists(serverFile))
            {
                FileInfo ExcelFile = new FileInfo(serverFile);
                if (ExcelFile.Length > 0)
                {
                    var workbook = new Excel.XLWorkbook(ExcelFile.FullName);
                    Excel.IXLWorksheet workSheet = workbook.Worksheet(1);
                    var xlRange = workSheet.RangeUsed();
                    int rowCount = xlRange.Rows().Count();
                    int colCount = xlRange.Cells().Count();
                    var LookUpList = _Util.Facade.LookupFacade.GetAllLookup();
                    Employee emp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentUser.UserId);
                    List<CustomerMigration> CustomerMigrations = _Util.Facade.CustomerFacade.GetAllCustomerMigration(Platform);
                    for (int i = 2; i <= rowCount; i++)
                    {
                        //Customer Customer = new Customer();
                        //Customer.CustomerId = new Guid();

                        CustomerMigration Customer = new CustomerMigration();
                        Customer.CustomerId = new Guid();

                        CustomerNote CustomerNote = new CustomerNote();
                        CustomerNote.CompanyId = CurrentUser.CompanyId.Value;
                        CustomerNote.CreatedDate = DateTime.Now.UTCCurrentTime();
                        int SableId = 0;

                        try
                        {
                            for (int j = 1; j <= colCount; j++)
                            {
                                if (xlRange.Cell(i, j) != null && xlRange.Cell(i, j).Value != null)
                                {
                                    try
                                    {
                                        #region Migration Conditions
                                        var value = xlRange.Cell(i, j).Value.ToString();
                                        var header = xlRange.Cell(1, j).Value.ToString();
                                        if (string.IsNullOrWhiteSpace(header))
                                        {
                                            break;
                                        }
                                        else if (header == "AccountId")
                                        {
                                            int AcctId = 0;
                                            if (!string.IsNullOrWhiteSpace(value) && int.TryParse(value, out AcctId) && AcctId > 0)
                                            {


                                                //Customer = _Util.Facade.CustomerFacade.GetCustomerByAdditionalCustomerNo(value);
                                                Customer = CustomerMigrations.Where(x => x.RefenrenceId == AcctId).FirstOrDefault();

                                                if (Customer != null)
                                                {
                                                    Customer.CustomerId = Customer.CustomerId;
                                                    int.TryParse(value, out SableId);
                                                }
                                                else
                                                {
                                                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\Point Customer Not Found For Customer Notes.txt"), true))
                                                    {
                                                        file.WriteLine("Customer Not Found: " + value);
                                                        file.Close();
                                                    }
                                                    break;
                                                }
                                            }
                                            continue;
                                        }
                                        #region Other Conditions
                                        else if (header == "Monitoring")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                CustomerNote.Notes = value ;
                                            }
                                            continue;
                                        }
                                        else if (header == "Date")
                                        {
                                            DateTime CreatedDate = new DateTime();
                                            if (!string.IsNullOrWhiteSpace(value) && DateTime.TryParse(value, out CreatedDate) && CreatedDate != new DateTime())
                                            {
                                                CustomerNote.CreatedDate  = CreatedDate;
                                            }
                                            continue;
                                        }
                                        #endregion
                                        #endregion
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                            }

                            if (Customer != null && Customer.CustomerId != new Guid())
                            {
                                #region CustomerNote Insert
                                if (!string.IsNullOrWhiteSpace(CustomerNote.Notes))
                                {
                                    CustomerNote note = new CustomerNote()
                                    {
                                        Notes = CustomerNote.Notes,
                                        CustomerId = Customer.CustomerId,
                                        CompanyId = CurrentUser.CompanyId.Value,
                                        CreatedDate = CustomerNote.CreatedDate,
                                        IsEmail = false,
                                        IsText = false,
                                        IsShedule = false,
                                        IsFollowUp = false,
                                        IsActive = true,
                                        CreatedBy = emp.FirstName + " " + emp.LastName,
                                        IsClose = false,
                                        IsAllDay = false,
                                        CreatedByUid = CurrentUser.UserId,
                                        IsPin = false,
                                        IsOverview = false
                                    };
                                    _Util.Facade.NotesFacade.InsertCustomerNote(note);
                                }

                                #endregion
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
            return Json(1);
        }

        #endregion

        #region Customer Status
        public JsonResult CustomerStatusUpload()
        {

            return Json(1);
        }

        #endregion

        #region Paid Commission
        public JsonResult SavePaidCommissionFile(string File)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var serverFile = Server.MapPath(File);
            if (System.IO.File.Exists(serverFile))
            {
                FileInfo ExcelFile = new FileInfo(serverFile);
                if (ExcelFile.Length > 0)
                {
                    var workbook = new Excel.XLWorkbook(ExcelFile.FullName);
                    Excel.IXLWorksheet workSheet = workbook.Worksheet(1);
                    var xlRange = workSheet.RangeUsed();
                    int rowCount = xlRange.Rows().Count();
                    int colCount = xlRange.Cells().Count();

                    for (int i = 2; i <= rowCount; i++)
                    {
                        PayrollDetail PaidCommission = new PayrollDetail();

                        PaidCommission.CompanyId = CurrentUser.CompanyId.Value;
                        PaidCommission.CreatedBy = CurrentUser.UserId;
                        PaidCommission.CreatedDate = DateTime.Now.UTCCurrentTime();
                        PaidCommission.LastUpdatedBy = CurrentUser.UserId;
                        PaidCommission.LastUpdatedDate = DateTime.Now.UTCCurrentTime();

                        try
                        {
                            for (int j = 1; j <= colCount; j++)
                            {
                                if (xlRange.Cell(i, j) != null && xlRange.Cell(i, j).Value != null)
                                {
                                    try
                                    {
                                        #region Migration Conditions
                                        var value = xlRange.Cell(i, j).Value.ToString();
                                        var header = xlRange.Cell(1, j).Value.ToString();
                                        if (string.IsNullOrWhiteSpace(header))
                                        {
                                            break;
                                        }
                                        else if (header == "RMA Account No")
                                        {
                                            int RMAAccountNo = 0;
                                            if (int.TryParse(value, out RMAAccountNo))
                                            {
                                                PaidCommission.RMAAccountNo = RMAAccountNo;
                                            }
                                            continue;
                                        }
                                        else if (header == "Rep Name")
                                        {
                                            PaidCommission.RepName = value;
                                            continue;
                                        }
                                        else if (header == "Rep Commission")
                                        {
                                            double RepCommission = 0;
                                            if (double.TryParse(value, out RepCommission))
                                            {
                                                PaidCommission.RepCommission = RepCommission;
                                            }
                                            continue;
                                        }
                                        else if (header == "Rep Holdback")
                                        {
                                            double RepHoldback = 0;
                                            if (double.TryParse(value, out RepHoldback))
                                            {
                                                PaidCommission.RepHoldback = RepHoldback;
                                            }
                                            continue;
                                        }
                                        else if (header == "Override Rep1")
                                        {
                                            PaidCommission.OverrideRep1 = value;
                                            continue;
                                        }
                                        else if (header == "Override 1") 
                                        {
                                            PaidCommission.Override1 = value;
                                            continue;
                                        }
                                        else if (header == "Override Rep2")
                                        {
                                            PaidCommission.OverrideRep2 = value;
                                            continue;
                                        }
                                        else if (header == "Override 2")
                                        {
                                            PaidCommission.Override2 = value;
                                            continue;
                                        }
                                        else if (header == "Rep Paid Date")
                                        {
                                            DateTime RepPaidDate = new DateTime();

                                            if (DateTime.TryParse(value, out RepPaidDate))
                                            {
                                                PaidCommission.RepPaidDate = RepPaidDate;
                                            }
                                            continue;
                                        }
                                        else if (header == "Tech Name")
                                        {
                                            PaidCommission.TechName = value;
                                            continue;
                                        }
                                        else if (header == "Tech Pay")
                                        {
                                            double TechPay = 0;
                                            if (double.TryParse(value, out TechPay))
                                            {
                                                PaidCommission.TechPay = TechPay;
                                            }
                                            continue;
                                        }
                                        else if (header == "Tech Holdback")
                                        {
                                            double TechHoldback = 0;
                                            if (double.TryParse(value, out TechHoldback))
                                            {
                                                PaidCommission.TechHoldback = TechHoldback;
                                            }
                                            continue;
                                        }
                                        else if (header == "Tech Paid Date")
                                        {
                                            DateTime TechPaidDate = new DateTime();

                                            if (DateTime.TryParse(value, out TechPaidDate))
                                            {
                                                PaidCommission.TechPaidDate = TechPaidDate;
                                            }
                                            continue;
                                        }
                                        else if (header == "Opener Commission")
                                        {
                                            double OpenerCommission = 0;
                                            if (double.TryParse(value, out OpenerCommission))
                                            {
                                                PaidCommission.OpenerCommission = OpenerCommission;
                                            }
                                            continue;
                                        }
                                        else if (header == "Misc Rep1")
                                        {
                                            PaidCommission.MiscRep1 = value;
                                            continue;
                                        }
                                        else if (header == "Misc Commission 1")
                                        {
                                            double MiscCommission1 = 0;
                                            if (double.TryParse(value, out MiscCommission1))
                                            {
                                                PaidCommission.MiscCommission1 = MiscCommission1;
                                            }
                                            continue;
                                        }
                                        else if (header == "Misc Rep2")
                                        {
                                            PaidCommission.MiscRep2 = value;
                                            continue;
                                        }
                                        else if (header == "Misc Commission 2")
                                        {
                                            double MiscCommission2 = 0;
                                            if (double.TryParse(value, out MiscCommission2))
                                            {
                                                PaidCommission.MiscCommission2 = MiscCommission2;
                                            }
                                            continue;
                                        }
                                        else if (header == "Misc Rep3")
                                        {
                                            PaidCommission.MiscRep3 = value;
                                            continue;
                                        }
                                        else if (header == "Misc Commission 3")
                                        {
                                            double MiscCommission3 = 0;
                                            if (double.TryParse(value, out MiscCommission3))
                                            {
                                                PaidCommission.MiscCommission3 = MiscCommission3;
                                            }
                                            continue;
                                        }
                                        else if (header == "Misc Rep4")
                                        {
                                            PaidCommission.MiscRep4 = value;
                                            continue;
                                        }
                                        else if (header == "Misc Commission 4")
                                        {
                                            double MiscCommission4 = 0;
                                            if (double.TryParse(value, out MiscCommission4))
                                            {
                                                PaidCommission.MiscCommission4 = MiscCommission4;
                                            }
                                            continue;
                                        }
                                        else if (header == "Misc Rep5")
                                        {
                                            PaidCommission.MiscRep5 = value;
                                            continue;
                                        }
                                        else if (header == "Misc Commission 5")
                                        {
                                            double MiscCommission5 = 0;
                                            if (double.TryParse(value, out MiscCommission5))
                                            {
                                                PaidCommission.MiscCommission5 = MiscCommission5;
                                            }
                                            continue;
                                        }
                                        else if (header == "Misc Paid Date")
                                        {
                                            DateTime MiscPaidDate = new DateTime();

                                            if (DateTime.TryParse(value, out MiscPaidDate))
                                            {
                                                PaidCommission.MiscPaidDate = MiscPaidDate;
                                            }
                                            continue;
                                        }

                                        #endregion
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                            }

                            _Util.Facade.EmployeeFacade.InsertPayrollDetail(PaidCommission);



                        }

                        catch (Exception ex)
                        {
                            logger.Error(ex, File);
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\ADS Customer Import Err.txt"), true))
                            {
                                //file.WriteLine("Error AT " + customer.Id + " " + customer.AdditionalCustomerNo);
                                file.Close();
                            }
                        }
                    }
                }
            }

            return Json(1);
        }


        #endregion

        #region Brinks Cancel Queue
        public JsonResult SaveCancelQueueFile(string File)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var serverFile = Server.MapPath(File);
            if (System.IO.File.Exists(serverFile))
            {
                FileInfo ExcelFile = new FileInfo(serverFile);
                if (ExcelFile.Length > 0)
                {
                    var workbook = new Excel.XLWorkbook(ExcelFile.FullName);
                    Excel.IXLWorksheet workSheet = workbook.Worksheet(1);
                    var xlRange = workSheet.RangeUsed();
                    int rowCount = xlRange.Rows().Count();
                    int colCount = xlRange.Cells().Count();

                    for (int i = 2; i <= rowCount; i++)
                    {
                        CancelQueue queue = new CancelQueue();


                        try
                        {
                            for (int j = 1; j <= colCount; j++)
                            {
                                if (xlRange.Cell(i, j) != null && xlRange.Cell(i, j).Value != null)
                                {
                                    try
                                    {
                                        #region Migration Conditions
                                        var value = xlRange.Cell(i, j).Value.ToString();
                                        var header = xlRange.Cell(1, j).Value.ToString();
                                        if (string.IsNullOrWhiteSpace(header))
                                        {
                                            break;
                                        }
                                        else if (header == "CS Number")
                                        {
                                           
                                                queue.CsNumber = value;
                                            
                                            continue;
                                        }
                                        else if (header == "Cancel Date")
                                        {
                                            DateTime CancelDate = new DateTime();
                                            DateTime.TryParse(value, out CancelDate);
                                            queue.CancelDate = CancelDate;
                                            continue;
                                        }
                                     
                                    
                                        #endregion
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                            }

                            if(!string.IsNullOrEmpty(queue.CsNumber))
                            {
                                Customer Cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerNo(queue.CsNumber);
                                if(Cus != null)
                                {
                                    CustomerExtended cusExt = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(Cus.CustomerId);
                                    if(cusExt != null)
                                    {
                                        cusExt.BrinksCancelDate = queue.CancelDate;
                                        _Util.Facade.CustomerFacade.UpdateCustomerExtended(cusExt);
                                    }
                                    else
                                    {
                                        cusExt = new CustomerExtended();
                                        cusExt.CustomerId = Cus.CustomerId;
                                        cusExt.BrinksCancelDate = queue.CancelDate;
                                        _Util.Facade.CustomerFacade.InsertCustomerExtended(cusExt);
                                    }
                                    #region Update Brinks Funding Status
                                    if (cusExt.BrinksCancelDate != null && cusExt.BrinksCancelDate != new DateTime() && cusExt.BrinksFundingDate != null && cusExt.BrinksFundingDate != new DateTime())
                                    {
                                        double days = (cusExt.BrinksFundingDate.Value.Date - cusExt.BrinksCancelDate.Value.Date).TotalDays;
                                        if(days > 365)
                                        {
                                            cusExt.BrinksFundingStatus = "Funded (Stage2)";
                                        }
                                        else
                                        {
                                            cusExt.BrinksFundingStatus = "Charged Back";
                                        }
                                  
                                        _Util.Facade.CustomerFacade.UpdateCustomerExtended(cusExt);
                                    }
                                    #endregion
                                }
                            }




                        }

                        catch (Exception ex)
                        {
                            logger.Error(ex, File);
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\ADS Customer Import Err.txt"), true))
                            {
                                //file.WriteLine("Error AT " + customer.Id + " " + customer.AdditionalCustomerNo);
                                file.Close();
                            }
                        }
                    }
                }
            }

            return Json(1);
        }
        public JsonResult SaveCustomerListFile(string File)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var serverFile = Server.MapPath(File);
            if (System.IO.File.Exists(serverFile))
            {
                FileInfo ExcelFile = new FileInfo(serverFile);
                if (ExcelFile.Length > 0)
                {
                    var workbook = new Excel.XLWorkbook(ExcelFile.FullName);
                    Excel.IXLWorksheet workSheet = workbook.Worksheet(1);
                    var xlRange = workSheet.RangeUsed();
                    int rowCount = xlRange.Rows().Count();
                    int colCount = xlRange.Cells().Count();

                    for (int i = 2; i <= rowCount; i++)
                    {
                        BrinksReportCustomer model = new BrinksReportCustomer();


                        try
                        {
                            for (int j = 1; j <= colCount; j++)
                            {
                                if (xlRange.Cell(i, j) != null && xlRange.Cell(i, j).Value != null)
                                {
                                    try
                                    {
                                        #region Migration Conditions
                                        var value = xlRange.Cell(i, j).Value.ToString();
                                        var header = xlRange.Cell(1, j).Value.ToString();
                                        if (string.IsNullOrWhiteSpace(header))
                                        {
                                            break;
                                        }
                                        else if (header == "Account")
                                        {

                                            model.Account = value;

                                            continue;
                                        }
                                        else if (header == "Received Date")
                                        {
                                            DateTime ReceivedDate = new DateTime();
                                            DateTime.TryParse(value, out ReceivedDate);
                                            model.ReceivedDate = ReceivedDate;
                                            continue;
                                        }
                                        else if (header == "Funding Date")
                                        {
                                            DateTime FundingDate = new DateTime();
                                            DateTime.TryParse(value, out FundingDate);
                                            model.FundingDate = FundingDate;
                                            continue;
                                        }
                                        else if (header == "Gross Funding Amount")
                                        {
                                            double FundingAmmount = new double();
                                            double.TryParse(value, out FundingAmmount);
                                            model.GrossFundingAmmount = FundingAmmount;
                                            continue;
                                        }

                                        #endregion
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(model.Account))
                            {
                                Customer Cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerNo(model.Account);
                                if (Cus != null)
                                {
                                    
                                    CustomerExtended cusExt = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(Cus.CustomerId);
                                    if (cusExt != null)
                                    {
                                        cusExt.BrinksFundingDate = model.FundingDate;
                                        cusExt.ReceivedDate = model.ReceivedDate;
                                        cusExt.GrossFundedAmount = model.GrossFundingAmmount;
                                        _Util.Facade.CustomerFacade.UpdateCustomerExtended(cusExt);
                                    }
                                    else
                                    {
                                        cusExt = new CustomerExtended();
                                        cusExt.BrinksFundingDate = model.FundingDate;
                                        cusExt.ReceivedDate = model.ReceivedDate;
                                        cusExt.GrossFundedAmount = model.GrossFundingAmmount;
                                        _Util.Facade.CustomerFacade.InsertCustomerExtended(cusExt);
                                    }

                                    #region Update Brinks Funding Status
                                    if(cusExt.BrinksFundingDate != null && cusExt.BrinksFundingDate != new DateTime())
                                    {
                                        cusExt.BrinksFundingStatus = "Funded (Stage1)";
                                        _Util.Facade.CustomerFacade.UpdateCustomerExtended(cusExt);
                                    }
                                    #endregion
                                }
                            }




                        }

                        catch (Exception ex)
                        {
                            logger.Error(ex);
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\ADS Customer Import Err.txt"), true))
                            {
                                //file.WriteLine("Error AT " + customer.Id + " " + customer.AdditionalCustomerNo);
                                file.Close();
                            }
                        }
                    }
                }
            }

            return Json(1);
        }

        public JsonResult SaveFundingVerificationFile(string File)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var serverFile = Server.MapPath(File);
            if (System.IO.File.Exists(serverFile))
            {
                FileInfo ExcelFile = new FileInfo(serverFile);
                if (ExcelFile.Length > 0)
                {
                    var workbook = new Excel.XLWorkbook(ExcelFile.FullName);
                    Excel.IXLWorksheet workSheet = workbook.Worksheet(1);
                    var xlRange = workSheet.RangeUsed();
                    int rowCount = xlRange.Rows().Count();
                    int colCount = xlRange.Cells().Count();

                    for (int i = 2; i <= rowCount; i++)
                    {
                        FundingVerification model = new FundingVerification();


                        try
                        {
                            for (int j = 1; j <= colCount; j++)
                            {
                                if (xlRange.Cell(i, j) != null && xlRange.Cell(i, j).Value != null)
                                {
                                    try
                                    {
                                        #region Migration Conditions
                                        var value = xlRange.Cell(i, j).Value.ToString();
                                        var header = xlRange.Cell(1, j).Value.ToString();
                                        if (string.IsNullOrWhiteSpace(header))
                                        {
                                            break;
                                        }
                                        else if (header == "Date")
                                        {
                                            DateTime FundingDate = new DateTime();
                                            DateTime.TryParse(value, out FundingDate);
                                            model.FundingDate = FundingDate;
                                            continue;
                                        }
                                        else if (header == "CS#")
                                        {

                                            model.CsNumber = value;

                                            continue;
                                        }
                                       
                                        else if (header == "Finance Company")
                                        { 
                                            model.FinanceCompany = value;
                                            continue;
                                        }
                                        else if (header == "Plan Code")
                                        {
                                            model.PlanCode = value;
                                            continue;
                                        }
                                       
                                        else if (header == "New MMR")
                                        {
                                            model.NewMMR = value;
                                            continue;
                                        }
                                        else if (header == "Loan Amount")
                                        {
                                            model.LoanAmount = value;
                                            continue;
                                        }
                                        else if (header == "Pay Out")
                                        {
                                            model.PayOut = value;
                                            continue;
                                        }
                                     
                                        #endregion
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(model.CsNumber))
                            {
                                Customer Cus = _Util.Facade.CustomerFacade.GetCustomerByCustomerNo(model.CsNumber);
                                if (Cus != null)
                                {
                                   
                                    CustomerExtended cusExt = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(Cus.CustomerId);
                                    if (cusExt != null)
                                    {
                                        cusExt.FinanceFundingDate = model.FundingDate;
                                        cusExt.FinanceCompany = model.FinanceCompany;
                                        cusExt.NewMMR = model.NewMMR;
                                        cusExt.Payout = model.PayOut;
                                        cusExt.PlanCode = model.PlanCode;
                                        cusExt.LoanAmount = model.LoanAmount;
                                        _Util.Facade.CustomerFacade.UpdateCustomerExtended(cusExt);
                                    }
                                    else
                                    {
                                        cusExt = new CustomerExtended();
                                        cusExt.FinanceFundingDate = model.FundingDate;
                                        cusExt.FinanceCompany = model.FinanceCompany;
                                        cusExt.NewMMR = model.NewMMR;
                                        cusExt.Payout = model.PayOut;
                                        cusExt.PlanCode = model.PlanCode;
                                        cusExt.LoanAmount = model.LoanAmount;
                                        _Util.Facade.CustomerFacade.InsertCustomerExtended(cusExt);
                                    }
                                }
                            }




                        }

                        catch (Exception ex)
                        {
                            logger.Error(ex);
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\ADS Customer Import Err.txt"), true))
                            {
                                //file.WriteLine("Error AT " + customer.Id + " " + customer.AdditionalCustomerNo);
                                file.Close();
                            }
                        }
                    }
                }
            }

            return Json(1);
        }
        
        #endregion

        #region DallasSecurity
        public JsonResult SyncDallasCustomer(string File)
        { 
            string subPath = "~/LeadImportReports"; // your code goes here

            bool exists = System.IO.Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath(subPath));
            if (!exists)
                System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath(subPath));
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var serverFile = Server.MapPath(File);
            if (System.IO.File.Exists(serverFile))
            {
                FileInfo ExcelFile = new FileInfo(serverFile);
                if (ExcelFile.Length > 0)
                {
                    var workbook = new Excel.XLWorkbook(ExcelFile.FullName);
                    Excel.IXLWorksheet workSheet = workbook.Worksheet(1);
                    var xlRange = workSheet.RangeUsed();
                    int rowCount = xlRange.Rows().Count();
                    int colCount = xlRange.Cells().Count(); 
                    List<Customer> customers = _Util.Facade.CustomerFacade.GetAllCustomer();
                    for (int i = 2; i <= rowCount; i++)
                    { 
                        Customer customer = new Customer();
                        try
                        {
                            for (int j = 1; j <= colCount; j++)
                            {
                                if (xlRange.Cell(i, j) != null && xlRange.Cell(i, j).Value != null)
                                {
                                    try
                                    {
                                        #region Migration Conditions
                                        var value = xlRange.Cell(i, j).Value.ToString();
                                        var header = xlRange.Cell(1, j).Value.ToString();
                                        if (string.IsNullOrWhiteSpace(header))
                                        {
                                            break;
                                        } 
                                        else if (header == "CSAccountNum")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer = customers.Where(x=>x.CustomerNo == value.Trim()).FirstOrDefault();
                                                if (customer == null)
                                                {
                                                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\MapscoOutput.txt"), true))
                                                    {
                                                        file.WriteLine("Customer Not Found for "+value);
                                                        file.Close();
                                                    }
                                                    break;
                                                }
                                                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\MapscoOutput.txt"), true))
                                                {
                                                    file.WriteLine("CustomerID: "+customer.Id+" CSAccount: " + value);
                                                    file.Close();
                                                }
                                            }
                                            continue;
                                        } 
                                        else if (header == "MapscoNumber")
                                        {
                                            if (!string.IsNullOrWhiteSpace(value))
                                            {
                                                customer.MapscoNo = value;
                                            }
                                            continue;
                                        }else if (header == "Warranty" && (!string.IsNullOrWhiteSpace(value)))
                                        {
                                            customer.Note += " Warranty: "+value;
                                        }
                                        else if (header == "Comment" && (!string.IsNullOrWhiteSpace(value)))
                                        {
                                            customer.Note += " Comment: " + value;
                                        }
                                        else if (header == "SpecialInstructionsID" && (!string.IsNullOrWhiteSpace(value)))
                                        {
                                            customer.Note += " SpecialInstructionsID: " + value;
                                        }

                                        #endregion
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                            }
                            
                            if(customer != null && customer.Id > 0)
                            {
                                _Util.Facade.CustomerFacade.UpdateCustomer(customer);
                            }
                        }
                        catch (Exception)
                        {
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\ADS Customer Import Err.txt"), true))
                            {
                                file.WriteLine("Error AT " + customer.Id + " " + customer.AdditionalCustomerNo);
                                file.Close();
                            }
                        }
                    }
                }
            }



            return Json(true);
        }

        #endregion
        
    }
}