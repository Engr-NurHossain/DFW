using HS.Entities;
using HS.Entities.Custom;
using HS.Framework;
using HS.Framework.Utils;
using HS.SMS;
using HS.Web.UI.Helper;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace HS.Web.UI.Controllers
{
    public class SMSController : BaseController
    {
        [Authorize]
        [HttpPost]
        public JsonResult SMSAgreementLink(int leadid,string PrefferedNO)
        { 
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            
            List<string> ReceiverNumberList = new List<string>();

            if (!_Util.Facade.CustomerFacade.CustomerIsInCompany(leadid, CurrentUser.CompanyId.Value))
            {
                return Json(new { result = false, message = "Lead is from another company." });
            }

            var Cus = _Util.Facade.CustomerFacade.GetCustomerById(leadid);
            //Cus.Soldby = CurrentUser.UserId.ToString();
            Cus.AgreementPhoneNo = PrefferedNO;
            _Util.Facade.CustomerFacade.UpdateCustomer(Cus);
            string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(leadid + "#" + Cus.EmailAddress + "#" + CurrentUser.CompanyId.Value.ToString());
            //string fullurl = string.Concat(AppConfig.SiteDomain, "/Leads-Agreement/", encryptedurl);
            string fullurl = string.Concat(AppConfig.SiteDomain, "/Public/LeadsAgreementDocument/?code=", encryptedurl);
            string shortUrl = "";
            string SMSText = "";
            string ReceiverNumber = "";
            ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, Cus.CustomerId);
            shortUrl = string.Concat(AppConfig.ShortSiteDomain, "/shrt/", ShortUrl.Code);
            //SMSText = string.Concat("Here is your agreements", Environment.NewLine, shortUrl);
            #region ReceiverNumber Setup
            if (!string.IsNullOrWhiteSpace(PrefferedNO))
            {
                ReceiverNumber = PrefferedNO.Replace("-", "");
            }
            if (!string.IsNullOrWhiteSpace(Cus.SecondaryPhone))
            {
                ReceiverNumber = Cus.SecondaryPhone.Replace("-", "");
            }
            else if (!string.IsNullOrWhiteSpace(Cus.PrimaryPhone))
            {
                ReceiverNumber = Cus.PrimaryPhone.Replace("-", "");
            }
            else
            {
                return Json(new { result = false, message = "Lead has no phone number available." });
            }
            ReceiverNumberList.Add(ReceiverNumber);
            #endregion
            SMSAgreement smsAgreement = new SMSAgreement();
          
            smsAgreement.ShortUrl = shortUrl;
            smsAgreement.CompanyName = CurrentUser.CompanyName;
            string phonenumber = string.Join(";", ReceiverNumberList);
            if (_Util.Facade.SMSFacade.SendAgrementSMS(smsAgreement, CurrentUser.UserId, CurrentUser.CompanyId.Value,ReceiverNumberList,false, string.Concat(CurrentUser.FirstName, " ", CurrentUser.LastName)) == true)
            {
                LeadCorrespondence LeadCorrespondence = new LeadCorrespondence()
                {
                    CompanyId = CurrentUser.CompanyId.Value,
                    CustomerId = Cus.CustomerId,
                    Type = "SMS",
                    ToMobileNo = phonenumber,
                    BodyContent = "Lead Agreement Document",
                    SentDate = DateTime.Now.UTCCurrentTime(),
                    LastUpdatedDate = DateTime.Now,
                    SentBy = CurrentUser.UserId
                };
                _Util.Facade.LeadCorrespondenceFacade.InsertCorrespondence(LeadCorrespondence);
                return Json(new { result = true, message = string.Format("SMS sent to {0} successfully.", ReceiverNumber) });
            }
            else
            {
                return Json(new { result = false, message = string.Format("Sending to {0} failed.", ReceiverNumber) });
            }
              
        }


        public JsonResult SMSAgreementLinkForPrintBlank(int leadid, string PrefferedNO, bool? IsRecreate, int? agreementtempid, bool? isinvoice,string invoiceid, bool? isestimator, int? estid, Guid? userid,bool? firstpage,bool? commercial)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            List<string> ReceiverNumberList = new List<string>();

            if (!_Util.Facade.CustomerFacade.CustomerIsInCompany(leadid, CurrentUser.CompanyId.Value))
            {
                return Json(new { result = false, message = "Lead is from another company." });
            }
            DateTime FixDate = DateTime.Now.UTCCurrentTime();

            var Cus = _Util.Facade.CustomerFacade.GetCustomerById(leadid);
            //Cus.Soldby = CurrentUser.UserId.ToString();
            _Util.Facade.CustomerFacade.UpdateCustomer(Cus);
            int? ticketid = 0;
            string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(leadid + "#" + Cus.EmailAddress + "#" + CurrentUser.CompanyId.Value.ToString() + "#" + (IsRecreate.HasValue ? IsRecreate.Value : false) + "#" + (agreementtempid.HasValue ? agreementtempid.Value : 0) + "#" + (firstpage.HasValue ? firstpage.Value : false) + "#" + (ticketid.HasValue ? ticketid.Value : 0) + "#" + (isinvoice.HasValue ? isinvoice : false) + "#" + (!string.IsNullOrWhiteSpace(invoiceid) ? invoiceid : "") + "#" + (isestimator.HasValue ? isestimator.Value : false) + "#" + (estid.HasValue ? estid.Value : 0) + "#" + (userid != Guid.Empty ? userid : new Guid()) + "#" + (commercial.HasValue ? commercial.Value : false));
            //string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(leadid + "#" + Cus.EmailAddress + "#" + CurrentUser.CompanyId.Value.ToString() + "#" + (IsRecreate.HasValue ? IsRecreate.Value : false) + "#" + (agreementtempid.HasValue ? agreementtempid.Value : 0) + "#" + (isinvoice.HasValue? isinvoice:false) + "#" + (!string.IsNullOrEmpty(invoiceid)?invoiceid:"") + "#" + (isestimator.HasValue ? isestimator.Value : false) + "#" + (estid.HasValue ? estid.Value : 0) + "#" + (userid !=Guid.Empty ? userid : new Guid()));
            //string fullurl = string.Concat(AppConfig.SiteDomain, "/Leads-Agreement/", encryptedurl);
            string fullurl = string.Concat(AppConfig.SiteDomain, "/public/LeadsAgreementDocument/?code=", encryptedurl);
            string shortUrl = "";
            string SMSText = "";
            string ReceiverNumber = "";
            ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, Cus.CustomerId);
            shortUrl = string.Concat(AppConfig.ShortSiteDomain, "/shrt/", ShortUrl.Code);
            //SMSText = string.Concat("Here is your agreements", Environment.NewLine, shortUrl);
            #region ReceiverNumber Setup
            if (!string.IsNullOrWhiteSpace(PrefferedNO))
            {
                ReceiverNumber = PrefferedNO.Replace("-", "");
            }
            else if (!string.IsNullOrWhiteSpace(Cus.SecondaryPhone))
            {
                ReceiverNumber = Cus.SecondaryPhone.Replace("-", "");
            }
            else if (!string.IsNullOrWhiteSpace(Cus.PrimaryPhone))
            {
                ReceiverNumber = Cus.PrimaryPhone.Replace("-", "");
            }
            else
            {
                return Json(new { result = false, message = "Lead has no phone number available." });
            }
            ReceiverNumberList.Add(ReceiverNumber);
            #endregion
            #region model for creating pdf
            InstallationAgreementModel Model = new InstallationAgreementModel();
            Model.EmergencyContactList = new List<EmergencyContact>();
            Model.ListAgreementAnswer = new List<AgreementAnswer>();
            Model.EquipmentList = new List<Equipment>();
            Model.CustomerAgreement = new List<CustomerAgreement>();
            Model.SingleCustomerAgreement = new CustomerAgreement();
            var taxtotal = 0.0;
            bool AgreementResult = false;
            //var ActivationfeeValue = 0.0;
            //var IsActivationFee = _Util.Facade.ActivationFeeFacade.GetActivationFeeByCompanyId(CurrentUser.CompanyId.Value);
            //if (IsActivationFee != null)
            //{
            //    ActivationfeeValue = IsActivationFee.Fee;
            //}
            Customer Cuss = new Customer();
            CustomerExtended CusExd = new CustomerExtended();
            Company Com = new Company();
            if (leadid>0)
            {
                if (!_Util.Facade.CustomerFacade.CustomerIsInCompany(leadid, CurrentUser.CompanyId.Value))
                {
                    return null;
                }
                Cus = _Util.Facade.CustomerFacade.GetCustomersById(leadid);
                if (Cus != null)
                {
                    CusExd = _Util.Facade.CustomerFacade.GetCustomerExtendedByCustomerId(Cus.CustomerId);
                }
                Com = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value);

                string ContractTerm = "";
                string ContractTermInWord = "";
                if (!string.IsNullOrWhiteSpace(Cus.ContractTeam) && Cus.ContractTeam != "-1")
                {
                    if (Cus.ContractTeam.ToLower() == "month to month")
                    {
                        ContractTerm = Cus.ContractTeam;
                        ContractTermInWord = Cus.ContractTeam;
                    }
                    else
                    {
                        ContractTerm = (Convert.ToInt32(Math.Round(Convert.ToDouble(Cus.ContractTeam) * 12))).ToString();
                        ContractTermInWord = NumberToWords((Convert.ToInt32(Math.Round(Convert.ToDouble(Cus.ContractTeam) * 12))));
                    }

                }

                #region Sold by change
                //Person who sends the mail to Customer will be counted as sold by
                //Cus.Soldby = CurrentUser.UserId.ToString();
                _Util.Facade.CustomerFacade.UpdateCustomer(Cus);
                #endregion

                var UpfrontAddOnTotal = 0.0;
                var UpfrontAddOnTotalPromo = 0.0;
                bool IsUpfrontPromo = false;
                bool IsServicePromo = false;
                var MonthlyServiceFeeTotal = 0.0;
                var TotalMonthlyMonitoring = 0.0;
                var NewSubTotal = 0.0;
                var TotalDueAtSigning = 0.0;
                var EquipmentTotalPrice = 0.0;
                var ServiceTotalPrice = 0.0;
                var AgreementSubtotal = 0.0;
                var AgreementTotal = 0.0;
                var AgreementTax = 0.0;
                var NotARBEnabledTotalPrice = 0.0;
                string InstallTypeName = "";
                bool IsNonConfirming = false;
                var NonConfirmingFee = 0.0;
                var AdvanceServiceFeeTotal = 0.0;
                if (Cus.CreditScoreValue == null)
                {
                    Cus.CreditScoreValue = 0;
                }
                var PackageCustomer = _Util.Facade.PackageFacade.GetPackageCustomerByCustomerId(Cus.CustomerId);
                if (PackageCustomer != null && PackageCustomer.NonConforming && PackageCustomer.NonConformingFee > 0 && (Cus.CreditScoreValue < PackageCustomer.MinCredit || Cus.CreditScoreValue > PackageCustomer.MaxCredit))
                {
                    IsNonConfirming = true;
                    NonConfirmingFee = PackageCustomer.NonConformingFee.Value;
                }
                var SmartPackageEquipmentServiceList = new List<SmartPackageEquipmentService>();
                var GetCityTaxList = _Util.Facade.CompanyBranchFacade.GetCityTaxRate(Cus.CustomerId, Com.CompanyId);
                if (GetCityTaxList.Count > 0)
                {
                    foreach (var item1 in GetCityTaxList)
                    {
                        AgreementTax = item1.Rate;
                    }
                }
                else
                {
                    Guid CustomerId = new Guid();
                    if (Cus != null)
                    {
                        CustomerId = Cus.CustomerId;
                    }
                    var GetSalesTax = _Util.Facade.GlobalSettingsFacade.GetSalesTax(Com.CompanyId, CustomerId);
                    if (GetSalesTax != null)
                    {
                        AgreementTax = Convert.ToDouble(GetSalesTax.Value);
                    }
                }
                #region Invoice
                Invoice invoice = new Invoice();
                CustomerProratedBill cpb = new CustomerProratedBill();
                cpb = _Util.Facade.InvoiceFacade.GetCusProratedBillByCustomerId(Cus.CustomerId);
                List<InvoiceDetail> invoiceList = new List<InvoiceDetail>();
                if (!string.IsNullOrWhiteSpace(invoiceid) && isinvoice == true)
                {
                    invoice = _Util.Facade.InvoiceFacade.GetByInvoiceId(invoiceid);
                    if (invoice != null)
                    {
                        invoiceList = _Util.Facade.InvoiceFacade.GetInvoiceDetialsListByInvoiceId(invoice.InvoiceId);
                    }
                }
                #endregion
                #region Estimator
                Employee userInfo = new Employee();
                if (userid != Guid.Empty)
                {
                    userInfo = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(userid.Value);
                }
                CreateEstimator createest = new CreateEstimator();
                if (isestimator.Value && estid > 0)
                {
                    CreateEstimator ca = new CreateEstimator();
                    ca.EstimatorSetting = new EstimatorSetting();
                    ca.Company = Com;

                    ca.Estimator = _Util.Facade.EstimatorFacade.GetById(estid.Value);
                    ca._EstimatorPDFFilter = _Util.Facade.EstimatorFacade.GetEstimatorPdfFilterByComIdCusIdUserId(Com.CompanyId, userid.Value, ca.Estimator.CustomerId);
                    ca.estimatorDetails = _Util.Facade.EstimatorFacade.GetEstimatorDetailListByEstimatorId(ca.Estimator.EstimatorId);
                    ca.estimatorServices = _Util.Facade.EstimatorFacade.GetEstimatorServicesByEstimatorId(ca.Estimator.EstimatorId);
                    if (ca.Estimator == null || ca.Estimator.CompanyId != Com.CompanyId)
                    {
                        return null;
                    }
                    if ((ca.estimatorDetails == null || ca.estimatorDetails.Count() == 0) && (ca.estimatorServices == null || ca.estimatorServices.Count() == 0))
                    {
                        return null;
                    }
                    Customer tempCUstomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(ca.Estimator.CustomerId);
                    if (tempCUstomer == null)
                    {
                        return null;
                    }

                    CreateEstimator processedModel = GetEstimatorModelById(ca.Estimator, ca.estimatorDetails, ca.estimatorServices, Com, tempCUstomer, ca._EstimatorPDFFilter, Com.CompanyId);
                    Estimator estimator = _Util.Facade.EstimatorFacade.GetEstimatorByEstimatorId(ca.Estimator.EstimatorId);
                    if (estimator != null)
                    {
                        ViewBag.CoverLetter = estimator.CoverLetter;
                        processedModel.Estimator.CoverLetter = estimator.CoverLetter;
                        processedModel.Estimator.CoverLetterFile = estimator.CoverLetterFile;
                        processedModel.Estimator.ServicePlanType = estimator.ServicePlanType;
                        processedModel.Estimator.ServicePlanRate = estimator.ServicePlanRate;
                        processedModel.Estimator.ServicePlanAmount = estimator.ServicePlanAmount;
                        processedModel.Estimator.ServiceTaxAmount = estimator.ServiceTaxAmount;
                        processedModel.Estimator.ServiceTotalAmount = estimator.ServiceTotalAmount;
                        processedModel.Estimator.ShowServicePlan = estimator.ShowServicePlan;
                        processedModel.Estimator.ShowService = estimator.ShowService;
                        processedModel.Estimator.ServicePlanTypeName = "Service Plan";

                        SelectListItem selectListItem = _Util.Facade.LookupFacade.GetDropdownsByKey("ServicePlans").Where(x => x.Value == estimator.ServicePlanType).FirstOrDefault();
                        if (selectListItem != null)
                        {
                            processedModel.Estimator.ServicePlanTypeName = selectListItem.Text;
                        }

                    }
                    if (ca.estimatorDetails != null)
                    {
                        foreach (var item in ca.estimatorDetails)
                        {
                            Manufacturer Manufacturer = _Util.Facade.EquipmentFacade.GetManufacturerByManufacturerId(item.ManufacturerId);
                            if (Manufacturer != null)
                            {
                                item.Manufacturer = Manufacturer.Name;
                            }
                            item.CreatedDate = DateTime.Now.UTCCurrentTime();
                            ca.SubTotal = ca.SubTotal + (item.TotalPrice.HasValue ? item.TotalPrice.Value : 0);
                            item.EquipmentFile = _Util.Facade.EquipmentFileFacade.GetEquipmentFilesByEquipmentIdAndFileType(item.EquipmentId, LabelHelper.EquipmentFileType.ProfilePicture).FirstOrDefault();
                            if (item.EquipmentFile == null)
                            {
                                item.EquipmentFile = new EquipmentFile();
                            }
                        }
                    }
                    if (ca.estimatorServices != null)
                    {
                        foreach (var item in ca.estimatorServices)
                        {
                            processedModel.ServiceSubTotal += ca.ServiceSubTotal + item.Amount;
                        }
                        processedModel.TotalServiceAmount = processedModel.ServiceSubTotal + ca.ServiceTax;
                    }
                    createest = processedModel;
                    createest.eSecurityLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/eSecurity_logo.png");
                    createest.specializedLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/specialized_property_management_logo.png");
                    string EstimatorContractTerm = "";
                    if (!string.IsNullOrWhiteSpace(createest.Estimator.ContractTerm) && createest.Estimator.ContractTerm != "-1")
                    {
                        if (createest.Estimator.ContractTerm.ToLower() == "month to month")
                        {
                            EstimatorContractTerm = createest.Estimator.ContractTerm;
                        }
                        else
                        {
                            EstimatorContractTerm = (Convert.ToInt32(Math.Round(Convert.ToDouble(createest.Estimator.ContractTerm) * 12))).ToString() + " month";
                        }

                    }
                    createest.EstimatorContractTerm = EstimatorContractTerm;
                }
                #endregion
                var CustomEquipmentList = new List<Equipment>();
                if (firstpage == true || IsRecreate == true)
                {
                    CustomEquipmentList = _Util.Facade.EquipmentFacade.GetSmartEquipmentListByCustomerIdAndCompanyId(Cus.CustomerId, Com.CompanyId, (firstpage.HasValue ? firstpage.Value : false), (ticketid.HasValue ? ticketid.Value : 0));
                    if (CustomEquipmentList.Count > 0)
                    {
                        foreach (var item in CustomEquipmentList)
                        {
                            EquipmentTotalPrice += item.Total;
                            UpfrontAddOnTotal += item.Total;
                        }
                    }
                }
                else
                {
                    CustomEquipmentList = _Util.Facade.EquipmentFacade.GetSmartEquipmentListByCustomerIdAndCompanyId(Cus.CustomerId, Com.CompanyId, (firstpage.HasValue ? firstpage.Value : false), (ticketid.HasValue ? ticketid.Value : 0));
                    if (CustomEquipmentList.Count > 0)
                    {
                        foreach (var item in CustomEquipmentList)
                        {
                            EquipmentTotalPrice += item.Total;
                            UpfrontAddOnTotal += item.Total;
                        }
                    }
                }
                var CustomServiceList = new List<Equipment>();
                if (firstpage == true || IsRecreate == true)
                {
                    CustomServiceList = _Util.Facade.EquipmentFacade.GetSmartServiceListByCustomerIdAndCompanyId(Cus.CustomerId, Com.CompanyId, (firstpage.HasValue ? firstpage.Value : false), (ticketid.HasValue ? ticketid.Value : 0));
                    if (CustomServiceList.Count > 0)
                    {
                        foreach (var item in CustomServiceList)
                        {
                            EquipmentTotalPrice += item.Total;
                            ServiceTotalPrice += item.Total;
                            MonthlyServiceFeeTotal += item.Total;
                        }
                    }
                }
                else
                {
                    CustomServiceList = _Util.Facade.EquipmentFacade.GetSmartServiceListByCustomerIdAndCompanyId(Cus.CustomerId, Com.CompanyId, (firstpage.HasValue ? firstpage.Value : false), (ticketid.HasValue ? ticketid.Value : 0));
                    if (CustomServiceList.Count > 0)
                    {
                        foreach (var item in CustomServiceList)
                        {
                            EquipmentTotalPrice += item.Total;
                            ServiceTotalPrice += item.Total;
                            MonthlyServiceFeeTotal += item.Total;
                        }
                    }
                }
                var NotARBEnabledServiceList = new List<Equipment>();
                if (firstpage == true || IsRecreate == true || commercial == true)
                {
                    NotARBEnabledServiceList = _Util.Facade.EquipmentFacade.GetNotARBEnabledSmartServiceListByCustomerIdAndCompanyId(Cus.CustomerId, Com.CompanyId, (firstpage.HasValue ? firstpage.Value : false), (ticketid.HasValue ? ticketid.Value : 0));
                    if (NotARBEnabledServiceList.Count > 0)
                    {
                        foreach (var item in NotARBEnabledServiceList)
                        {
                            NotARBEnabledTotalPrice += item.Total;
                        }
                    }
                }
                else
                {
                    NotARBEnabledServiceList = _Util.Facade.EquipmentFacade.GetNotARBEnabledSmartServiceListFromService(Cus.CustomerId, Com.CompanyId);
                    if (NotARBEnabledServiceList.Count > 0)
                    {
                        foreach (var item in NotARBEnabledServiceList)
                        {
                            NotARBEnabledTotalPrice += item.Total;
                        }
                    }
                }
                #region Advance Monitoring Service Month

                PaymentInfoCustomer paycus = new PaymentInfoCustomer();
                paycus = _Util.Facade.PaymentInfoCustomerFacade.GetPaymentInfoCustomerByCustomerIdAndPayForService(Cus.CustomerId);
                int ForMonth = 1;
                if (paycus != null && paycus.ForMonths.HasValue)
                {
                    ForMonth = paycus.ForMonths.Value;
                }
                if (ForMonth > 1)
                {
                    AdvanceServiceFeeTotal = MonthlyServiceFeeTotal * (ForMonth - 1);

                }
                #endregion
                Cus.MonthlyMonitoringFee = Convert.ToString(ServiceTotalPrice);
                TotalMonthlyMonitoring = MonthlyServiceFeeTotal;
                NewSubTotal = TotalMonthlyMonitoring + UpfrontAddOnTotal;
                if (CustomServiceList.Count > 0 || CustomEquipmentList.Count > 0)
                {
                    if (PackageCustomer != null && PackageCustomer.ActivationFee.HasValue)
                    {
                        AgreementSubtotal = PackageCustomer.ActivationFee.Value + EquipmentTotalPrice;
                        NewSubTotal = PackageCustomer.ActivationFee.Value + EquipmentTotalPrice;
                    }
                    else
                    {
                        AgreementSubtotal = EquipmentTotalPrice;
                        NewSubTotal = EquipmentTotalPrice;
                    }
                }
                if (IsNonConfirming && NonConfirmingFee > 0)
                {
                    AgreementSubtotal = AgreementSubtotal + NonConfirmingFee;
                    NewSubTotal = NewSubTotal + NonConfirmingFee;
                }
                if (AgreementTax != 0.0)
                {
                    taxtotal = (AgreementSubtotal / 100) * AgreementTax;
                    Model.TaxTotal = taxtotal;
                    AgreementTotal = AgreementSubtotal + taxtotal;
                    TotalDueAtSigning = NewSubTotal + taxtotal;
                }
                else
                {
                    Model.TaxTotal = 0.0;
                    AgreementTotal = AgreementSubtotal;
                    TotalDueAtSigning = NewSubTotal;
                }
                var PackageCustomerDetails = _Util.Facade.PackageFacade.GetPackageCustomerByCustomerIdandCompanyId(Cus.CustomerId, CurrentUser.CompanyId.Value);
                if (PackageCustomerDetails != null)
                {
                    InstallTypeName = _Util.Facade.PackageFacade.SmartInstallTypeNameByInstallTypeId(Convert.ToInt32(PackageCustomerDetails.SmartInstallTypeId));
                    SmartPackageEquipmentServiceList = _Util.Facade.PackageFacade.GetAllSmartPackageIncludeEquipmentByPackageIdAndCompanyId(PackageCustomerDetails.PackageId, CurrentUser.CompanyId.Value);
                }
                var PaymentDetails = _Util.Facade.PaymentInfoFacade.GetAllPaymentInfoByCustomerIdAndCompanyId(Cus.CustomerId, CurrentUser.CompanyId.Value).Where(m => m.PayFor == "First Month").FirstOrDefault();
                var agreementPayment = _Util.Facade.PaymentInfoFacade.GetLeadAgreementPaymentInfoByCustomerId(Cus.CustomerId);
                string paymentoverviewheader = "";
                string paymentoverviewdata = "";
                if (agreementPayment != null && agreementPayment.Count > 0)
                {
                    paymentoverviewheader = "<table style='border-collapse:collapse; width:100%; font-family:Arial; table-layout:fixed; font-size:13px;'>{0}</table>";
                    foreach (var pay in agreementPayment)
                    {
                        var sppay = pay.Type.Split('_');
                        if (sppay.Length > 0)
                        {
                            if (sppay[0] == "CC")
                            {
                                var cardNumber = pay.CardNumber.Replace('-', ' ').Replace(" ", "");
                                if (cardNumber.Length == 16)
                                {
                                    paymentoverviewdata += @"<tr>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Account Name: " + pay.AccountName + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Card Number: " + string.Concat("".PadLeft(12, '*'), cardNumber.Substring(cardNumber.Length - 4)) + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Expire Date: " + pay.CardExpireDate + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Security Code: " + string.Concat("".PadLeft(2, '*'), pay.CardSecurityCode.Substring(pay.CardSecurityCode.Length - 1)) + @"</td>
                                                        </tr>";
                                }
                                else
                                {
                                    paymentoverviewdata += @"<tr>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Account Name: " + pay.AccountName + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Card Number: " + string.Concat("".PadLeft(11, '*'), cardNumber.Substring(cardNumber.Length - 4)) + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Expire Date: " + pay.CardExpireDate + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Security Code: " + string.Concat("".PadLeft(2, '*'), pay.CardSecurityCode.Substring(pay.CardSecurityCode.Length - 1)) + @"</td>
                                                        </tr>";
                                }
                            }
                            else if (sppay[0] == "ACH")
                            {
                                paymentoverviewdata += @"<tr>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Account Name: " + pay.AccountName + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Account Type: " + pay.BankAccountType + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Routing No: " + pay.RoutingNo + @"</td>
                                                        <td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Account No: " + string.Concat("".PadLeft(pay.AcountNo.Length - 4, '*'), pay.AcountNo.Substring(pay.AcountNo.Length - 4)) + @"</td>
                                                        </tr>";
                            }
                        }
                    }
                }
                var CustomerAddress = AddressHelper.MakeAddress(Cus);
                var CustomerInstallAddress = AddressHelper.MakeInstallAddress(Cus);
                CustomerSignature cs = new CustomerSignature();
                GlobalSetting glbs = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "CompanySignature");
                string cusSignature = "";
                string cussignDate = "";
                DateTime cussignDateVal = new DateTime();
                if (firstpage.HasValue && firstpage.Value == true)
                {
                    cs = _Util.Facade.CustomerSignatureFacade.GetCustomerSignatureByReferenceIdcharCustomerIdType(Cus.CustomerId, agreementtempid.ToString(), "First Page");
                }
                else if (commercial.HasValue && commercial.Value == true)
                {
                    cs = _Util.Facade.CustomerSignatureFacade.GetCustomerSignatureByReferenceIdcharCustomerIdType(Cus.CustomerId, agreementtempid.ToString(), "Commercial");
                }
                else if (isinvoice.HasValue && isinvoice.Value == true)
                {
                    cs = _Util.Facade.CustomerSignatureFacade.GetCustomerSignatureByReferenceIdcharCustomerIdType(Cus.CustomerId, agreementtempid.ToString(), "Estimate");
                }
                else if (isestimator.HasValue && isestimator.Value == true)
                {
                    cs = _Util.Facade.CustomerSignatureFacade.GetCustomerSignatureByReferenceIdcharCustomerIdType(Cus.CustomerId, agreementtempid.ToString(), "Estimator");
                }
                else if (IsRecreate.HasValue && IsRecreate.Value == true)
                {
                    cs = _Util.Facade.CustomerSignatureFacade.GetCustomerSignatureByReferenceIdcharCustomerIdType(Cus.CustomerId, agreementtempid.ToString(), "Recreate");
                }
                else if (Cus != null && agreementtempid.HasValue && agreementtempid.Value > 0)
                {
                    cs = _Util.Facade.CustomerSignatureFacade.GetCustomerSignatureByReferenceIdcharCustomerIdType(Cus.CustomerId, agreementtempid.ToString(), "Agreement File");
                }
                else
                {
                    cusSignature = Cus.Singature;
                    if (Cus.CustomerSignatureDate != null && Cus.CustomerSignatureDate != new DateTime())
                    {
                        cussignDate = Cus.CustomerSignatureDate.Value.UTCToClientTime().ToString("M/dd/yy");
                        cussignDateVal = Cus.CustomerSignatureDate.Value.UTCToClientTime();
                    }

                }
                if (cs != null && (agreementtempid != 0 || (firstpage.HasValue && firstpage.Value == true) || (IsRecreate.HasValue && IsRecreate.Value == true) || (commercial.HasValue && commercial.Value == true)))
                {
                    cusSignature = cs.Signature;
                    if (cs.CreatedDate != null && cs.CreatedDate != new DateTime())
                    {
                        cussignDate = cs.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                        cussignDateVal = cs.CreatedDate.UTCToClientTime();
                    }

                }
                //(!string.IsNullOrWhiteSpace(Cus.MonthlyMonitoringFee) ? Convert.ToDouble(Cus.MonthlyMonitoringFee) : 1) * Convert.ToDouble(ContractTerm),

                Double MMR = 1;
                Double CTerm = 0;

                double.TryParse(Cus.MonthlyMonitoringFee, out MMR);
                double.TryParse(ContractTerm, out CTerm);
                //(!string.IsNullOrWhiteSpace() ? Convert.ToDouble(Cus.MonthlyMonitoringFee) : 1) * Convert.ToDouble(ContractTerm);
                Double TotalPayments = MMR * CTerm;
                #region For Promo Pyment Method
                List<PaymentInfoCustomer> paycusList = new List<PaymentInfoCustomer>();
                PaymentProfileCustomer paymentProfile = new PaymentProfileCustomer();
                paycusList = _Util.Facade.PaymentInfoCustomerFacade.GetAllPaymentInfoCustomerByCustomerId(Cus.CustomerId);
                if (paycusList != null && paycusList.Count > 0)
                {
                    foreach (var item in paycusList)
                    {
                        paymentProfile = _Util.Facade.PaymentInfoCustomerFacade.GetPaymentProfileByPaymentInfoId(item.PaymentInfoId);
                        if (paymentProfile != null && paymentProfile.Type == LabelHelper.PaymentMethod.Promo)
                        {
                            if (item.Payfor == "Activation Fee")
                            {
                                NonConfirmingFee = 0.0;

                                if (PackageCustomer != null)
                                {
                                    PackageCustomer.ActivationFee = 0.0;
                                }
                                NewSubTotal = NewSubTotal - (PackageCustomer.AdditionFee + NonConfirmingFee);
                            }
                            else if (item.Payfor == "Equipment")
                            {
                                IsUpfrontPromo = true;
                                NewSubTotal = NewSubTotal - UpfrontAddOnTotal;
                            }
                            else if (item.Payfor == "Service")
                            {
                                NewSubTotal = NewSubTotal - TotalMonthlyMonitoring;
                                IsServicePromo = true;

                            }


                        }

                    }

                }
                #endregion

                #region LeadSource
                string LeadSource = "";
                Lookup leadsource = _Util.Facade.LookupFacade.GetLookupByKeyAndValueAndCompanyId("LeadSource", Cus.LeadSource, Com.CompanyId);
                LeadSource = leadsource != null && !string.IsNullOrWhiteSpace(leadsource.DisplayText) && leadsource.DataValue != "-1" ? leadsource.DisplayText : "";
                #endregion

                Model = new InstallationAgreementModel()
                {
                    CSIDNumber = Cus.Id,
                    LeadSource = LeadSource,
                    IsNonConfirming = IsNonConfirming,
                    NonConfirmingFee = NonConfirmingFee,
                    InstallDate = Cus.InstallDate != null ? Convert.ToDateTime(Cus.InstallDate).ToShortDateString() : "",
                    OriginalContactDate = (Cus.OriginalContactDate != null && Cus.OriginalContactDate.HasValue) ? Cus.OriginalContactDate.Value : new DateTime(),
                    AccountType = Cus.Type,
                    Referredby = Cus.ReferringCustomer != Guid.Empty ? _Util.Facade.CustomerFacade.GetCustomerNameById(Cus.ReferringCustomer) : "",
                    SocialSecurityNumber = Cus.SSN,
                    //Owner2ndPhone = Cus.SecondaryPhone,
                    Owner2ndPhone = Cus.PrimaryPhone,
                    InitialStreet = Cus.Street,
                    InitialCity = Cus.City,
                    InitialCountry = Cus.County,
                    InitialState = Cus.State,
                    InitialZip = Cus.ZipCode,
                    InitialApt = Cus.Appartment,
                    BillingCity = Cus.CityPrevious,
                    BillingState = Cus.StatePrevious,
                    BillingZip = Cus.ZipCodePrevious,
                    BillingCountry = Cus.CountryPrevious,
                    BillingStreet = Cus.StreetPrevious,
                    InstallTypeName = InstallTypeName,
                    SmartPackageEquipmentServiceList = SmartPackageEquipmentServiceList,
                    UpfrontAddOnTotal = UpfrontAddOnTotal,
                    UpfrontAddOnTotalPromo = UpfrontAddOnTotalPromo,
                    IsUpfrontPromo = IsUpfrontPromo,
                    IsServicePromo = IsServicePromo,
                    MonthlyServiceFeeTotal = MonthlyServiceFeeTotal,
                    TotalMonthlyMonitoring = TotalMonthlyMonitoring,
                    NewSubTotal = NewSubTotal,
                    TotalDueAtSigning = TotalDueAtSigning,
                    PaymentDetails = PaymentDetails != null ? PaymentDetails : new PaymentInfo(),
                    DisplayName = Cus.DisplayName,
                    BillingAddress = CustomerAddress,
                    OwnerAddress = CustomerAddress,
                    InstallAddress = CustomerInstallAddress,
                    OwnerEmail = Cus.EmailAddress,
                    //OwnerPhone = Cus.PrimaryPhone,
                    OwnerPhone = Cus.CellNo,
                    OwnerName = Cus.FirstName + " " + Cus.LastName,
                    FirstName = Cus.FirstName,
                    LastName = Cus.LastName,
                    EmergencyContactList = _Util.Facade.EmergencyContactFacade.GetAllEmergencyContactByCustomerIdAndCompanyId(Cus.CustomerId, Com.CompanyId),
                    CompanyName = Com.CompanyName,
                    CompanySate = string.Format("{0}, {1} {2}", Com.City, Com.State, Com.ZipCode),
                    CompanyStreet = Com.Street,
                    CompanyWebsite = Com.Website,
                    SubscribedMonths = ContractTerm,
                    SubscribedMonthsInWord = ContractTermInWord,
                    RenewalMonths = Cus.RenewalTerm.HasValue ? Cus.RenewalTerm.Value : 0,
                    Password = Cus.Passcode,
                    DateOfTransaction = FixDate.UTCToClientTime(),
                    CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyEmailLogoByCompanyId(Com.CompanyId),
                    EquipmentList = CustomEquipmentList.ToList(),
                    ServiceList = CustomServiceList.ToList(),
                    ActivationFee = (PackageCustomer != null && PackageCustomer.ActivationFee.HasValue) ? PackageCustomer.ActivationFee.Value : 0,
                    BusinessName = Cus.BusinessName,
                    MonthlyMonitoringFee = Cus.MonthlyMonitoringFee,
                    EffectiveDate = FixDate.UTCToClientTime(),
                    CustomerSignature = cusSignature,
                    CustomerSignatureStringDate = cussignDate,
                    CustomerSignatureStringDateVal = cussignDateVal,
                    CustomerAgreement = _Util.Facade.CustomerAgreementFacade.GetCustomerAgreementByCompanyIdAndCustomerId1(Com.CompanyId, Cus.CustomerId),
                    Subtotal = AgreementSubtotal,
                    Tax = AgreementTax,
                    TaxTotal = taxtotal,
                    Total = AgreementTotal,
                    EContractId = Cus.Id,
                    ListAgreementAnswer = _Util.Facade.AgreementFacade.GetAllAgreementAnswerByCustomerId(Cus.CustomerId),
                    SalesRepresentative = !string.IsNullOrEmpty(Cus.Soldby) ? _Util.Facade.EmployeeFacade.GetEmployeeNumByEmployeeId(new Guid(Cus.Soldby)).ToString() : "",
                    TotalPayments = TotalPayments,
                    SingleCustomerAgreement = _Util.Facade.CustomerAgreementFacade.GetIpAndUserAgentByCustomerIdAndCompanyId(Com.CompanyId, Cus.CustomerId),
                    ListContactEmergency = string.Format(paymentoverviewheader, paymentoverviewdata),
                    ListPaymentInfo = string.Format(paymentoverviewheader, paymentoverviewdata),
                    DoingBusinessAs = Cus.DBA,
                    DispalyName = Cus.DisplayName,
                    CompanyPhone = Com.Phone,
                    FirstPage = firstpage.HasValue ? firstpage.Value : false,
                    Commercial = commercial.HasValue ? commercial.Value : false,
                    IsInvoice = isinvoice.Value,
                    InvoiceId = invoiceid,
                    InvoiceList = invoiceList,
                    IsEstimator = isestimator.HasValue ? isestimator.Value : false,
                    createEst = createest,
                    userInfo = userInfo,
                    inv = invoice,
                    NotARBEnabledServiceList = NotARBEnabledServiceList.ToList(),
                    NotARBEnabledTotalPrice = NotARBEnabledTotalPrice,
                    ProratedAmout = cpb != null ? Math.Round(cpb.Amount, 2, MidpointRounding.AwayFromZero) : 0.0,
                    FinancedAmout = Cus != null && Cus.FinancedAmount != null ? Math.Round(Cus.FinancedAmount.Value, 2, MidpointRounding.AwayFromZero) : 0.0,
                    MonthlyFinanceRate = CusExd != null && CusExd.MonthlyFinanceRate != null ? Math.Round(CusExd.MonthlyFinanceRate.Value, 2, MidpointRounding.AwayFromZero) : 0.0,
                    AdvanceServiceFeeTotal = AdvanceServiceFeeTotal
                };
                if (agreementtempid != 0)
                {
                    if (cs != null && glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) && !string.IsNullOrWhiteSpace(cs.Signature))
                    {
                        Model.CompanySignature = glbs.Value;
                        if (cs.CreatedDate != null && cs.CreatedDate != new DateTime())
                        {
                            Model.CompanySignatureDate = cs.CreatedDate.UTCToClientTime().ToString("M/dd/yy");
                        }
                    }
                }
                else if (firstpage == true || IsRecreate == true || commercial == true)
                {
                    if (glbs != null && !string.IsNullOrWhiteSpace(glbs.Value))
                    {
                        Model.CompanySignature = glbs.Value;
                        Model.CompanySignatureDate = cussignDate;
                    }
                }
                else
                {
                    if (Cus != null && glbs != null && !string.IsNullOrWhiteSpace(glbs.Value) && !string.IsNullOrWhiteSpace(Cus.Singature))
                    {
                        Model.CompanySignature = glbs.Value;
                        if (Cus.CustomerSignatureDate != null && Cus.CustomerSignatureDate != new DateTime())
                        {
                            Model.CompanySignatureDate = Cus.CustomerSignatureDate.Value.UTCToClientTime().ToString("M/dd/yy");
                        }
                    }
                }

                if (Model.EmergencyContactList == null)
                {
                    Model.EmergencyContactList = new List<EmergencyContact>();
                }
            }
            else
            {
                Model.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CurrentUser.CompanyId.Value);
            }

            //  return View(Model);
            Model.KazarLogo = string.Concat(AppConfig.SiteDomain, "/Content/img/kazar_logo.PNG");
            ViewBag.CompanyId = Com.CompanyId.ToString();
            Model.CompanyId = Com.CompanyId.ToString();
            Model.CurrentCurrency = _Util.Facade.GlobalSettingsFacade.GetCurrentCurrencyByCompanyId(Com.CompanyId);
            string body = _Util.Facade.AgreementFacade.MakeSmartAgreementPdf(Model, agreementtempid.HasValue ? agreementtempid.Value : 0);
            ViewBag.Body = body;
            #region File Save
            ViewAsPdf actionPDF;

            actionPDF = new Rotativa.ViewAsPdf("~/Views/SmartLeads/SmartInstallationAgreement.cshtml",Model)
            {
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = { Left = 1, Right = 1 },

            };

            byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
            Random rand = new Random();
            string filename = ConfigurationManager.AppSettings["File.LeadToCustomerAgreement"];
            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName.ReplaceSpecialChar();
            var pdftempFolderName = string.Format(filename, comname) + rand.Next().ToString() + leadid + "AgreementMail.pdf";
            string Serverfilename = FileHelper.GetFileFullPath(pdftempFolderName);
            FileHelper.SaveFile(applicationPDFData, Serverfilename);
            #endregion
            #endregion

            SMSAgreement smsAgreement = new SMSAgreement();

            smsAgreement.ShortUrl = shortUrl;
            smsAgreement.CompanyName = CurrentUser.CompanyName;
            string phonenumber = string.Join(";", ReceiverNumberList);
            if (_Util.Facade.SMSFacade.SendAgrementSMS(smsAgreement, CurrentUser.UserId, CurrentUser.CompanyId.Value, ReceiverNumberList, false, string.Concat(CurrentUser.FirstName, " ", CurrentUser.LastName)) == true)
            {
                #region insert LeadCorrespondence
                if (IsRecreate == true)
                {
                    LeadCorrespondence LeadCorrespondence = new LeadCorrespondence()
                    {
                        CompanyId = CurrentUser.CompanyId.Value,
                        CustomerId = Cus.CustomerId,
                        Type = "SMS",
                        ToMobileNo = phonenumber,
                        BodyContent = "Recreate Agreement",
                        SentDate = DateTime.Now.UTCCurrentTime(),
                        LastUpdatedDate = DateTime.Now,
                        SentBy = CurrentUser.UserId
                    };
                    _Util.Facade.LeadCorrespondenceFacade.InsertCorrespondence(LeadCorrespondence);
                }
                else if (firstpage == true)
                {
                    LeadCorrespondence LeadCorrespondence = new LeadCorrespondence()
                    {
                        CompanyId = CurrentUser.CompanyId.Value,
                        CustomerId = Cus.CustomerId,
                        Type = "SMS",
                        ToMobileNo = phonenumber,
                        BodyContent = "FirstPage Agreement",
                        SentDate = DateTime.Now.UTCCurrentTime(),
                        LastUpdatedDate = DateTime.Now,
                        SentBy = CurrentUser.UserId
                    };
                    _Util.Facade.LeadCorrespondenceFacade.InsertCorrespondence(LeadCorrespondence);
                }
                else if (commercial == true)
                {
                    LeadCorrespondence LeadCorrespondence = new LeadCorrespondence()
                    {
                        CompanyId = CurrentUser.CompanyId.Value,
                        CustomerId = Cus.CustomerId,
                        Type = "SMS",
                        ToMobileNo = phonenumber,
                        BodyContent = "Commercial Agreement",
                        SentDate = DateTime.Now.UTCCurrentTime(),
                        LastUpdatedDate = DateTime.Now,
                        SentBy = CurrentUser.UserId
                    };
                    _Util.Facade.LeadCorrespondenceFacade.InsertCorrespondence(LeadCorrespondence);
                }
                else
                {
                    LeadCorrespondence LeadCorrespondence = new LeadCorrespondence()
                    {
                        CompanyId = CurrentUser.CompanyId.Value,
                        CustomerId = Cus.CustomerId,
                        Type = "SMS",
                        ToMobileNo = phonenumber,
                        BodyContent = "Smart Lead Agreement",
                        SentDate = DateTime.Now.UTCCurrentTime(),
                        LastUpdatedDate = DateTime.Now,
                        SentBy = CurrentUser.UserId
                    };
                    _Util.Facade.LeadCorrespondenceFacade.InsertCorrespondence(LeadCorrespondence);
                }
                #endregion

                if (IsRecreate == true)
                {
            
                    string FileName = "Recreate_Agreement";
                    #region file save to customer file
                    CustomerFile cfs = new CustomerFile()
                    {
                        FileDescription = Cus.Id + "_" + Regex.Replace(FileName, @"\s+", String.Empty) + "_SMS" + ".pdf",
                        Filename = "/" + pdftempFolderName,
                        FileId = Guid.NewGuid(),
                        FileFullName = Regex.Replace(FileName, @"\s+", String.Empty) + ".pdf",
                        Uploadeddate = DateTime.Now.UTCCurrentTime(),
                        CustomerId = Cus.CustomerId,
                        CompanyId = CurrentUser.CompanyId.Value,
                        IsActive = true,
                        CreatedBy = CurrentUser.UserId,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        UpdatedBy = CurrentUser.UserId,
                        UpdatedDate = DateTime.Now.UTCCurrentTime(),
                        WMStatus = LabelHelper.WatermarkStatus.Pending,
                        AWSProcessStatus = LabelHelper.AWSProcessStatus.Local
                    };
                    _Util.Facade.CustomerFileFacade.InsertCustomerFile(cfs);
                    string logMessage = Regex.Replace(FileName, @"\s+", String.Empty) + ".pdf " + "file send to " + ReceiverNumber;
                    base.AddUserActivityForCustomer(logMessage, LabelHelper.ActivityAction.AddDocumentFileManagement, null, Cus.Id, null);
                    #endregion
                }
                else if (firstpage == true)
                {
   
                    string FileName = "Firstpage_Agreement";
                    #region file save to customer file
                    CustomerFile cfs = new CustomerFile()
                    {
                        FileDescription = Cus.Id + "_" + Regex.Replace(FileName, @"\s+", String.Empty) + "_SMS" + ".pdf",
                        Filename = "/" + pdftempFolderName,
                        FileId = Guid.NewGuid(),
                        FileFullName = Regex.Replace(FileName, @"\s+", String.Empty) + ".pdf",
                        Uploadeddate = DateTime.Now.UTCCurrentTime(),
                        CustomerId = Cus.CustomerId,
                        CompanyId = CurrentUser.CompanyId.Value,
                        IsActive = true,
                        CreatedBy = CurrentUser.UserId,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        UpdatedBy = CurrentUser.UserId,
                        UpdatedDate = DateTime.Now.UTCCurrentTime()
                    };
                    _Util.Facade.CustomerFileFacade.InsertCustomerFile(cfs);
                    string logMessage = Regex.Replace(FileName, @"\s+", String.Empty) + ".pdf " + "file send to " + ReceiverNumber;
                    base.AddUserActivityForCustomer(logMessage, LabelHelper.ActivityAction.AddDocumentFileManagement, null, Cus.Id, null);
                    #endregion
                }
               else if (commercial == true)
                {

                    string FileName = "Commercial_Agreement";
                    #region file save to customer file
                    CustomerFile cfs = new CustomerFile()
                    {
                        FileDescription = Cus.Id+"_"+Regex.Replace(FileName, @"\s+", String.Empty) + "_SMS" + ".pdf",
                        Filename = "/" + pdftempFolderName,
                        FileId = Guid.NewGuid(),
                        FileFullName = Regex.Replace(FileName, @"\s+", String.Empty) + ".pdf",
                        Uploadeddate = DateTime.Now.UTCCurrentTime(),
                        CustomerId = Cus.CustomerId,
                        CompanyId = CurrentUser.CompanyId.Value,
                        IsActive = true,
                        CreatedBy = CurrentUser.UserId,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        UpdatedBy = CurrentUser.UserId,
                        UpdatedDate = DateTime.Now.UTCCurrentTime()
                    };
                    _Util.Facade.CustomerFileFacade.InsertCustomerFile(cfs);
                    string logMessage = Regex.Replace(FileName, @"\s+", String.Empty) + ".pdf " + "file send to " + ReceiverNumber;
                    base.AddUserActivityForCustomer(logMessage, LabelHelper.ActivityAction.AddDocumentFileManagement, null, Cus.Id, null);
                    #endregion
                }
                else
                {
                    string FileName = "Smart_Lead_Agreement";
                    #region file save to customer file
                    CustomerFile cfs = new CustomerFile()
                    {
                        FileDescription = Cus.Id + "_" + Regex.Replace(FileName, @"\s+", String.Empty) + "_SMS" + ".pdf",
                        Filename = "/" + pdftempFolderName,
                        FileId = Guid.NewGuid(),
                        FileFullName = Regex.Replace(FileName, @"\s+", String.Empty) + ".pdf",
                        Uploadeddate = DateTime.Now.UTCCurrentTime(),
                        CustomerId = Cus.CustomerId,
                        CompanyId = CurrentUser.CompanyId.Value,
                        IsActive = true,
                        CreatedBy = CurrentUser.UserId,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                        UpdatedBy = CurrentUser.UserId,
                        UpdatedDate = DateTime.Now.UTCCurrentTime()
                    };
                    _Util.Facade.CustomerFileFacade.InsertCustomerFile(cfs);
                    string logMessage = Regex.Replace(FileName, @"\s+", String.Empty) + ".pdf " + "file send to " + ReceiverNumber;
                    base.AddUserActivityForCustomer(logMessage, LabelHelper.ActivityAction.AddDocumentFileManagement, null, Cus.Id, null);
                    #endregion
                }
                Customer cus2 = _Util.Facade.CustomerFacade.GetCustomerById(Cus.Id);
                cus2.IsAgreementSend = true;
                if(IsRecreate == true)
                {
                    cus2.Singature = "";
                }
                
                _Util.Facade.CustomerFacade.UpdateCustomer(cus2);



                return Json(new { result = true, message = string.Format("SMS sent to {0} successfully.", ReceiverNumber) });
            }
            else
            {
                return Json(new { result = false, message = string.Format("Sending to {0} failed.", ReceiverNumber) });
            }

        }

        //public JsonResult SMSFileLinkForPrintBlank(int? cusid, string PrefferedNO, int? fileid)
        //{
        //    var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

        //    List<string> ReceiverNumberList = new List<string>();
        //    FileTemplate ft = new FileTemplate();
        //    CustomerAgreementTemplate cat = new CustomerAgreementTemplate();
        //    if (fileid.HasValue)
        //    {
        //        cat = _Util.Facade.FileFacade.GetCustomerAgreementTemplateById(fileid.Value);
        //    }
        //    if (cat != null && cat.IsFileTemplate == true && cat.ReferenceTemplateId.HasValue)
        //    {
        //        ft = _Util.Facade.FileFacade.GetFileTemplateById(cat.ReferenceTemplateId.Value);
        //    }
        //    if (!_Util.Facade.CustomerFacade.CustomerIsInCompany(cusid.Value, CurrentUser.CompanyId.Value))
        //    {
        //        return Json(new { result = false, message = "File is from another company." });
        //    }

        //    var Cus = _Util.Facade.CustomerFacade.GetCustomerById(cusid.Value);

        //    string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(cusid + "#" + Cus.EmailAddress + "#" + CurrentUser.CompanyId.Value.ToString() + "#" + fileid+"#"+CurrentUser.UserId);
        //    string fullurl = string.Concat(AppConfig.SiteDomain, "/File-Template/", encryptedurl);
        //    string shortUrl = "";
        //    string ReceiverNumber = "";
        //    ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, Cus.CustomerId);
        //    shortUrl = string.Concat(AppConfig.SiteDomain, "/shrt/", ShortUrl.Code);
        //    #region ReceiverNumber Setup
        //    if (!string.IsNullOrWhiteSpace(PrefferedNO))
        //    {
        //        ReceiverNumber = PrefferedNO.Replace("-", "");
        //    }
        //    else if (!string.IsNullOrWhiteSpace(Cus.SecondaryPhone))
        //    {
        //        ReceiverNumber = Cus.SecondaryPhone.Replace("-", "");
        //    }
        //    else if (!string.IsNullOrWhiteSpace(Cus.PrimaryPhone))
        //    {
        //        ReceiverNumber = Cus.PrimaryPhone.Replace("-", "");
        //    }
        //    else
        //    {
        //        return Json(new { result = false, message = "Phone number is not available." });
        //    }
        //    ReceiverNumberList.Add(ReceiverNumber);
        //    #endregion
        //    SMSFile smsFile = new SMSFile();
        //    if (ft != null)
        //    {
        //        if (ft.IsCustomerSignRequired == false)
        //        {
        //            smsFile.ShortUrl = "Please review the attached file from the link. " + shortUrl;
        //        }
        //        else
        //        {
        //            smsFile.ShortUrl = "Please review and sign the attached file from the link. " + shortUrl;
        //        }
        //    }
        //    else
        //    {
        //        smsFile.ShortUrl = "Please review and sign the attached file from the link. " + shortUrl;
        //    }
        //    //smsFile.ShortUrl = shortUrl;
        //    string phonenumber = string.Join(";", ReceiverNumberList);
        //    if (_Util.Facade.SMSFacade.SendFileSMS(smsFile, CurrentUser.CompanyId.Value, ReceiverNumberList, false, string.Concat(CurrentUser.FirstName, " ", CurrentUser.LastName)) == true)
        //    {
        //        return Json(new { result = true, message = string.Format("SMS sent to {0} successfully.", ReceiverNumber) });
        //    }
        //    else
        //    {
        //        return Json(new { result = false, message = string.Format("Sending to {0} failed.", ReceiverNumber) });
        //    }
        //}


        [Authorize]
        [HttpPost]
        public JsonResult SendEstimateText(Guid CustomerId, int InvoiceId, string ContactNumber, string Message)
        {
            if (string.IsNullOrWhiteSpace(ContactNumber))
            {
                return Json(new { result = false });
            }
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
             
            Customer tempCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
            if (!tempCustomer.PreferedSms.HasValue || !tempCustomer.PreferedSms.Value)
            {
                tempCustomer.PreferedSms = true;
                _Util.Facade.CustomerFacade.UpdateCustomer(tempCustomer);
            }

             
            List<string> ReceiverNumberList = new List<string>();
            if (Message.IndexOf("##url##") > -1)
            {
                string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(CustomerId
                    + "#"
                    + CurrentUser.CompanyId.Value
                    + "#"
                    + InvoiceId);
                string fullurl = string.Concat(AppConfig.SiteDomain, "/Leads-Estimate/", encryptedurl);
                ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, CustomerId);
                Message = Message.Replace("##url##", ShortUrl.Code);
            }
            //Message = string.Concat("Here is your estimate", Environment.NewLine, shortUrl);
            #region ReceiverNumber Setup 
            ReceiverNumberList.Add(ContactNumber);
            #endregion
            InvoiceSms invSms = new InvoiceSms();

            invSms.Message = Message;
            if (_Util.Facade.SMSFacade.SendInvSMS(invSms,CurrentUser.UserId, CurrentUser.CompanyId.Value, ReceiverNumberList, false, string.Concat(CurrentUser.FirstName, " ", CurrentUser.LastName)) == true)
            {
                var invobj = _Util.Facade.InvoiceFacade.GetInvoiceById(InvoiceId);
                if (invobj != null)
                {
                    invobj.Status = HS.Web.UI.Helper.LabelHelper.EstimateStatus.Created;
                    _Util.Facade.InvoiceFacade.UpdateInvoice(invobj);
                }
                LeadCorrespondence objcorres = new LeadCorrespondence()
                {
                    CompanyId = CurrentUser.CompanyId.Value,
                    CustomerId = tempCustomer.CustomerId,
                    Type = LabelHelper.CorrespondenceMessageTyp.SMS,
                    ToMobileNo = ContactNumber,
                    BodyContent = Message,
                    SentDate = DateTime.Now.UTCCurrentTime(),
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                    SentBy = CurrentUser.UserId
                };
                _Util.Facade.LeadCorrespondenceFacade.InsertCorrespondence(objcorres);
                return Json(new { result = true, message = string.Format("SMS sent to {0} successfully.", ContactNumber) });
            }
            else
            {
                return Json(new { result = false, message = string.Format("Sending to {0} failed.", ContactNumber) });
            }
               // bool result = _Util.Facade.SMSFacade.SendSMS(CurrentUser.CompanyId.Value, Message, ReceiverNumberList, false, string.Concat(CurrentUser.FirstName, " ", CurrentUser.LastName));
            //if (result)
            //{
               
            //}
            //else
            //{
              
            //}
        }

        [Authorize]
        [HttpPost]
        public JsonResult SendInvoiceText(Guid CustomerId, int InvoiceId, string ContactNumber, string Message)
        {
            if (string.IsNullOrWhiteSpace(ContactNumber))
            {
                return Json(new { result = false });
            }
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            
            Customer tempCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
            if (!tempCustomer.PreferedSms.HasValue || !tempCustomer.PreferedSms.Value)
            {
                tempCustomer.PreferedSms = true;
                _Util.Facade.CustomerFacade.UpdateCustomer(tempCustomer);
            }

            List<string> ReceiverNumberList = new List<string>();
            if (Message.IndexOf("##url##") > -1)
            {
                string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(InvoiceId
                    + "#"
                    + CurrentUser.CompanyId.Value
                    + "#"
                    + CustomerId);
                string fullurl = string.Concat(AppConfig.SiteDomain, "/Customer-Invoice/", encryptedurl);
                ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, CustomerId);
                Message = Message.Replace("##url##", ShortUrl.Code);
            }
            //Message = string.Concat("Here is your estimate", Environment.NewLine, shortUrl);
            #region ReceiverNumber Setup 
            ReceiverNumberList.Add(ContactNumber);
            #endregion
            InvoiceSms invSms = new InvoiceSms();

            invSms.Message = Message;

            if (_Util.Facade.SMSFacade.SendInvSMS(invSms, CurrentUser.UserId, CurrentUser.CompanyId.Value, ReceiverNumberList, false, string.Concat(CurrentUser.FirstName, " ", CurrentUser.LastName)) == true)
            {
                LeadCorrespondence objcorres = new LeadCorrespondence()
                {
                    CompanyId = CurrentUser.CompanyId.Value,
                    CustomerId = tempCustomer.CustomerId,
                    Type = LabelHelper.CorrespondenceMessageTyp.SMS,
                    ToMobileNo = ContactNumber,
                    BodyContent = Message,
                    SentDate = DateTime.Now.UTCCurrentTime(),
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                    SentBy = CurrentUser.UserId
                };
                _Util.Facade.LeadCorrespondenceFacade.InsertCorrespondence(objcorres);
                return Json(new { result = true, message = string.Format("SMS sent to {0} successfully.", ContactNumber) });
            }
            else
            {
                return Json(new { result = false, message = string.Format("Sending to {0} failed.", ContactNumber) });
            }
               // bool result = _Util.Facade.SMSFacade.SendSMS(CurrentUser.CompanyId.Value, Message, ReceiverNumberList,false,string.Concat(CurrentUser.FirstName," ",CurrentUser.LastName));
            
            //if (result)
            //{
            //    LeadCorrespondence objcorres = new LeadCorrespondence()
            //    {
            //        CompanyId = CurrentUser.CompanyId.Value,
            //        CustomerId = tempCustomer.CustomerId,
            //        Type = LabelHelper.CorrespondenceMessageTyp.SMS,
            //        ToMobileNo = ContactNumber,
            //        BodyContent = Message,
            //        SentDate = DateTime.Now,
            //        LastUpdatedDate = DateTime.Now,
            //        SentBy = CurrentUser.UserId
            //    };
            //    _Util.Facade.LeadCorrespondenceFacade.InsertCorrespondence(objcorres);
            //    return Json(new { result = true, message = string.Format("SMS sent to {0} successfully.", ContactNumber) });
            //}
            //else
            //{
            //    return Json(new { result = false, message = string.Format("Sending to {0} failed.", ContactNumber) });
            //}
        }

        [Authorize]
        [HttpPost]
        public JsonResult SendCustomerInfoText(Guid CustomerId, string ContactNumber, string Message)
        {
            if (string.IsNullOrWhiteSpace(ContactNumber))
            {
                return Json(new { result = false });
            }
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            Customer tempCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
            if (!tempCustomer.PreferedSms.HasValue || !tempCustomer.PreferedSms.Value)
            {
                tempCustomer.PreferedSms = true;
                _Util.Facade.CustomerFacade.UpdateCustomer(tempCustomer);
            }

            List<string> ReceiverNumberList = new List<string>();
            if (Message.IndexOf("##url##") > -1)
            {
                string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(                  
                     CurrentUser.CompanyId.Value
                    + "#"
                    + CustomerId);
                string fullurl = string.Concat(AppConfig.SiteDomain, "/Customer/", encryptedurl);
                ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, CustomerId);
                Message = Message.Replace("##url##", ShortUrl.Code);
            }
            //Message = string.Concat("Here is your estimate", Environment.NewLine, shortUrl);
            #region ReceiverNumber Setup 
            ReceiverNumberList.Add(ContactNumber);
            #endregion
            CustomerInfoSms cusSms = new CustomerInfoSms();

            cusSms.Message = Message;
            if (_Util.Facade.SMSFacade.SendCustomerInfoSMS(cusSms, CurrentUser.UserId, CurrentUser.CompanyId.Value, ReceiverNumberList, false, string.Concat(CurrentUser.FirstName, " ", CurrentUser.LastName)) == true)

            {
                LeadCorrespondence objcorres = new LeadCorrespondence()
                {
                    CompanyId = CurrentUser.CompanyId.Value,
                    CustomerId = tempCustomer.CustomerId,
                    Type = LabelHelper.CorrespondenceMessageTyp.SMS,
                    ToMobileNo = ContactNumber,
                    BodyContent = Message,
                    SentDate = DateTime.Now.UTCCurrentTime(),
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                    SentBy = CurrentUser.UserId
                };
                _Util.Facade.LeadCorrespondenceFacade.InsertCorrespondence(objcorres);
                return Json(new { result = true, message = string.Format("SMS sent to {0} successfully.", ContactNumber) });
            }
            else
            {
                return Json(new { result = false, message = string.Format("Sending to {0} failed.", ContactNumber) });
            }

        }
        [Authorize]
        [HttpPost]
        public JsonResult SendRequisitionText(string ContactNumber, string Message)
        {
            if (string.IsNullOrWhiteSpace(ContactNumber))
            {
                return Json(new { result = false });
            }
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            //Customer tempCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
            //if (!tempCustomer.PreferedSms.HasValue || !tempCustomer.PreferedSms.Value)
            //{
            //    tempCustomer.PreferedSms = true;
            //    _Util.Facade.CustomerFacade.UpdateCustomer(tempCustomer);
            //}

            List<string> ReceiverNumberList = new List<string>();
            if (Message.IndexOf("##url##") > -1)
            {
                string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(CurrentUser.CompanyId.Value.ToString());
                string fullurl = string.Concat(AppConfig.SiteDomain, "/Requisition-Order/", encryptedurl);
                ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, CurrentUser.CompanyId.Value);
                Message = Message.Replace("##url##", ShortUrl.Code);
            }
            //Message = string.Concat("Here is your estimate", Environment.NewLine, shortUrl);
            #region ReceiverNumber Setup 
            ReceiverNumberList.Add(ContactNumber);
            #endregion
            RequisitionSms reqSms = new RequisitionSms();

            reqSms.Message = Message;

            if (_Util.Facade.SMSFacade.SendReqSMS(reqSms,CurrentUser.UserId, CurrentUser.CompanyId.Value, ReceiverNumberList, false, string.Concat(CurrentUser.FirstName, " ", CurrentUser.LastName)) == true)
            {
                //LeadCorrespondence objcorres = new LeadCorrespondence()
                //{
                //    CompanyId = CurrentUser.CompanyId.Value,
                //    CustomerId = tempCustomer.CustomerId,
                //    Type = LabelHelper.CorrespondenceMessageTyp.SMS,
                //    ToMobileNo = ContactNumber,
                //    BodyContent = Message,
                //    SentDate = DateTime.Now.UTCCurrentTime(),
                //    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                //    SentBy = CurrentUser.UserId
                //};
                //_Util.Facade.LeadCorrespondenceFacade.InsertCorrespondence(objcorres);
                return Json(new { result = true, message = string.Format("SMS sent to {0} successfully.", ContactNumber) });
            }
            else
            {
                return Json(new { result = false, message = string.Format("Sending to {0} failed.", ContactNumber) });
            }

        }
        [Authorize]
        [HttpPost]
        public JsonResult SendPurchaseOrderText(int POId, string ContactNumber, string Message)
        {
            if (string.IsNullOrWhiteSpace(ContactNumber))
            {
                return Json(new { result = false });
            }
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
             
            if (Message.IndexOf("##url##") > -1)
            {
                PurchaseOrder PO = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderById(POId);

                string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(PO.PurchaseOrderId
                                        + "#"
                                        + CurrentUser.CompanyId.Value
                                        + "#"
                                        + PO.SuplierId);
                string fullurl = string.Concat(AppConfig.SiteDomain, "/Supplier-PO/", encryptedurl);
                ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, null);
                Message = Message.Replace("##url##", ShortUrl.Code);
            }
            //Message = string.Concat("Here is your estimate", Environment.NewLine, shortUrl);
            #region ReceiverNumber Setup
            List<string> ReceiverNumberList = new List<string>();
            ReceiverNumberList.Add(ContactNumber);
            #endregion

            PurchaseOrderSms poSms = new PurchaseOrderSms();

            poSms.Message = Message;

            if (_Util.Facade.SMSFacade.SendPurchesOrderSMS(poSms, CurrentUser.UserId, CurrentUser.CompanyId.Value, ReceiverNumberList, false, string.Concat(CurrentUser.FirstName, " ", CurrentUser.LastName)) == true)
            {
                return Json(new { result = true, message = string.Format("SMS sent to {0} successfully.", ContactNumber) });
            }
            else
            {
                return Json(new { result = false, message = string.Format("Sending to {0} failed.", ContactNumber) });
            }
            //    bool result = _Util.Facade.SMSFacade.SendSMS(CurrentUser.CompanyId.Value, Message, ReceiverNumberList, false, string.Concat(CurrentUser.FirstName, " ", CurrentUser.LastName));

            //if (result)
            //{
            //    return Json(new { result = true, message = string.Format("SMS sent to {0} successfully.", ContactNumber) });
            //}
            //else
            //{
            //    return Json(new { result = false, message = string.Format("Sending to {0} failed.", ContactNumber) });
            //}
        }

        [Authorize]
        [HttpPost]
        public JsonResult SMSToSalesPerson(Guid CustomerId , string mailBody, string contact)
        {
            bool result = false; 
            string message = "Text Msg sending failed.";
            List<string> Receiverperson = new List<string>();
            List<string> ReceiverNumber = new List<string>();
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            
            if(!string.IsNullOrWhiteSpace(contact))
            {

                var numbers = contact.Split(';');
                foreach(var item in numbers)
                {
                    if (!string.IsNullOrWhiteSpace(item) && item.Length>3)
                    {
                        ReceiverNumber.Add(item);
                    }
                }
                if (ReceiverNumber.Count() > 0)
                {
                    SalesPersonSms saleSms = new SalesPersonSms();

                    saleSms.Message = mailBody;

                    if (_Util.Facade.SMSFacade.SendSalesSMS(saleSms, CurrentUser.UserId, CurrentUser.CompanyId.Value, ReceiverNumber, true, string.Concat(CurrentUser.FirstName, " ", CurrentUser.LastName)) == true)
                    {
                        result = true;
                        message = "Message sent successfully.";

                        LeadCorrespondence objCorres = new LeadCorrespondence()
                        {
                            CompanyId = CurrentUser.CompanyId.Value,
                            CustomerId = CustomerId,
                            Type = LabelHelper.CorrespondenceMessageTyp.SMS,
                            ToMobileNo = string.Join(";", ReceiverNumber),
                            BodyContent = mailBody,
                            SentDate = DateTime.Now.UTCCurrentTime(),
                            LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                            SentBy = CurrentUser.UserId
                        };
                        _Util.Facade.LeadCorrespondenceFacade.InsertCorrespondence(objCorres);

                        var AssignAdmin = Guid.Empty;
                        var AdminDetails = _Util.Facade.UserLoginFacade.GetAllUserMgmtListByCompanyId(CurrentUser.CompanyId.Value, null, null, null, null).Where(m => m.AccessRights == LabelHelper.UserTags.Admin).FirstOrDefault(); if (AdminDetails != null && AdminDetails.UserId != null)
                        {
                            AssignAdmin = AdminDetails.UserId;
                        }
                        Activity activity = new Activity()
                        {
                            ActivityId=Guid.NewGuid(),
                            ActivityType=LabelHelper.ActivityType.Text,
                            AssignedTo= AssignAdmin,
                            Status=LabelHelper.ActivityStatus.Completed,
                            AssociatedWith = CustomerId,
                            AssociatedType = LabelHelper.ActivityAssociateType.Account,
                            Note = mailBody,
                            CreatedBy = CurrentUser.UserId,
                            CreatedDate=DateTime.Now
                        };
                        _Util.Facade.ActivityFacade.InsertActivity(activity);
                    } 
                    //result = _Util.Facade.SMSFacade.SendSMS(CurrentLoggedInUser.CompanyId.Value, mailBody, ReceiverNumber, false, string.Concat(CurrentLoggedInUser.FirstName + " " + CurrentLoggedInUser.LastName));
                    //if (result == true)
                    //{
                       
                    //}
                }
            } 
            return Json(new { result = result, message = message });
        }

        //SMS For Booking Text Method 
        [Authorize]
        [HttpPost]
        public JsonResult SendBookingText(Guid CustomerId, int BookingId, string ContactNumber, string Message)
        {
            if (string.IsNullOrWhiteSpace(ContactNumber))
            {
                return Json(new { result = false });
            }
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            Customer tempCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
            if (!tempCustomer.PreferedSms.HasValue || !tempCustomer.PreferedSms.Value)
            {
                tempCustomer.PreferedSms = true;
                _Util.Facade.CustomerFacade.UpdateCustomer(tempCustomer);
            }

            List<string> ReceiverNumberList = new List<string>();
            if (Message.IndexOf("##url##") > -1)
            {
                string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(CustomerId
                    + "#"
                    + CurrentUser.CompanyId.Value
                    + "#"
                    + BookingId);
                string fullurl = string.Concat(AppConfig.SiteDomain, "/Leads-Booking/", encryptedurl);
                ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, CustomerId);
                Message = Message.Replace("##url##", ShortUrl.Code);
            }
            //Message = string.Concat("Here is your estimate", Environment.NewLine, shortUrl);
            #region ReceiverNumber Setup 
            ReceiverNumberList.Add(ContactNumber);
            #endregion

            bool result = _Util.Facade.SMSFacade.SendSMS(CurrentUser.CompanyId.Value,CurrentUser.UserId, Message, ReceiverNumberList, false, string.Concat(CurrentUser.FirstName, " ", CurrentUser.LastName));
            if (result)
            {
                var bookObj = _Util.Facade.BookingFacade.GetBookingById(BookingId);
                if (bookObj != null)
                {
                    bookObj.Status = HS.Web.UI.Helper.LabelHelper.BookingStatus.SentToCustomer;
                    _Util.Facade.BookingFacade.UpdateBooking(bookObj);
                }
                LeadCorrespondence objcorres = new LeadCorrespondence()
                {
                    CompanyId = CurrentUser.CompanyId.Value,
                    CustomerId = tempCustomer.CustomerId,
                    Type = LabelHelper.CorrespondenceMessageTyp.SMS,
                    ToMobileNo = ContactNumber,
                    BodyContent = Message,
                    SentDate = DateTime.Now,
                    LastUpdatedDate = DateTime.Now,
                    SentBy = CurrentUser.UserId
                };
                _Util.Facade.LeadCorrespondenceFacade.InsertCorrespondence(objcorres);
                return Json(new { result = true, message = string.Format("SMS sent to {0} successfully.", ContactNumber) });
            }
            else
            {
                return Json(new { result = false, message = string.Format("Sending to {0} failed.", ContactNumber) });
            }
        }


        public ActionResult SMSTemplate()
        {
            if (!base.IsPermitted(UserPermissions.MenuPermissions.QuickMenuMyCompanyEmailTemplateSettings))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            List<SMSTemplate> TemplteList = _Util.Facade.SMSFacade.GetAllTemplateByCompanyId(CurrentUser.CompanyId.Value).OrderBy(x => x.Name).ToList();
            return View(TemplteList);
        }


        [Authorize]
        [HttpPost]
        public JsonResult SendEstimatorText(Guid CustomerId, int EstimatorId, string ContactNumber, string Message)
        {
            if (string.IsNullOrWhiteSpace(ContactNumber))
            {
                return Json(new { result = false });
            }
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            Customer tempCustomer = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(CustomerId);
            if (!tempCustomer.PreferedSms.HasValue || !tempCustomer.PreferedSms.Value)
            {
                tempCustomer.PreferedSms = true;
                _Util.Facade.CustomerFacade.UpdateCustomer(tempCustomer);
            }


            List<string> ReceiverNumberList = new List<string>();
            if (Message.IndexOf("##url##") > -1)
            {
                string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(CustomerId
                    + "#"
                    + CurrentUser.CompanyId.Value
                    + "#"
                    + EstimatorId);
                string fullurl = string.Concat(AppConfig.SiteDomain, "/Leads-Estimator/", encryptedurl);
                ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, CustomerId);
                Message = Message.Replace("##url##", ShortUrl.Code);
            }
            #region ReceiverNumber Setup 
            ReceiverNumberList.Add(ContactNumber);
            #endregion
            EstimatorSms estSms = new EstimatorSms();

            estSms.Message = Message;
            if (_Util.Facade.SMSFacade.SendEstimatorSMS(estSms,CurrentUser.UserId, CurrentUser.CompanyId.Value, ReceiverNumberList, false, string.Concat(CurrentUser.FirstName, " ", CurrentUser.LastName)) == true)
            {
                var invobj = _Util.Facade.EstimatorFacade.GetEstimatorById(EstimatorId);
                if (invobj != null)
                {
                    invobj.Status = HS.Web.UI.Helper.LabelHelper.EstimateStatus.Open;
                    _Util.Facade.EstimatorFacade.UpdateEstimator(invobj);
                }
                LeadCorrespondence objcorres = new LeadCorrespondence()
                {
                    CompanyId = CurrentUser.CompanyId.Value,
                    CustomerId = tempCustomer.CustomerId,
                    Type = LabelHelper.CorrespondenceMessageTyp.SMS,
                    ToMobileNo = ContactNumber,
                    BodyContent = Message,
                    SentDate = DateTime.Now.UTCCurrentTime(),
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                    SentBy = CurrentUser.UserId
                };
                _Util.Facade.LeadCorrespondenceFacade.InsertCorrespondence(objcorres);
                return Json(new { result = true, message = string.Format("SMS sent to {0} successfully.", ContactNumber) });
            }
            else
            {
                return Json(new { result = false, message = string.Format("Sending to {0} failed.", ContactNumber) });
            }
        }
        #region Number to Words for contract term
        private static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000000) > 0)
            {
                words += NumberToWords(number / 1000000000) + " billion ";
                number %= 1000000000;
            }

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += " " + unitsMap[number % 10];
                }
            }
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            words = textInfo.ToTitleCase(words);
            return words;
        }
        #endregion
        #region Estimator Model
        private CreateEstimator GetEstimatorModelById(Estimator Invoice, List<EstimatorDetail> InvoiceDetialList, List<EstimatorService> EstimatorServiceList, Company tempCom, Customer tempCUstomer, EstimatorPDFFilter EstimatorPDFFilters, Guid comid)
        {
            //var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            CreateEstimator Model = new CreateEstimator();
            Model.Estimator = Invoice;
            Model.estimatorDetails = InvoiceDetialList;
            Model.estimatorServices = EstimatorServiceList;
            Model.Estimator.CustomerName = tempCUstomer.Title + " " + tempCUstomer.FirstName + " " + tempCUstomer.LastName;

            //  Model.Invoice.IsEstimate = false;


            //Model.Invoice.InvoiceDate = Invoice.InvoiceDate.HasValue ? Invoice.InvoiceDate.Value : Model.Invoice.InvoiceDate.Value.ClientToUTCTime();
            //if (!string.IsNullOrWhiteSpace(Model.Invoice.EstimateTerm) && Model.Invoice.EstimateTerm != "-1")
            //    Model.Invoice.EstimateTerm = _Util.Facade.LookupFacade.GetDisplayTextByDataValueFromLLookup(Model.Invoice.EstimateTerm);
            //Model.Invoice.DueDate = Invoice.DueDate.HasValue ? Invoice.DueDate.Value : Model.Invoice.DueDate.Value.ClientToUTCTime();
            #region Discount Calculation 
            //if (!string.IsNullOrWhiteSpace(Model.Invoice.DiscountType))
            //{
            //    if (Model.Invoice.DiscountType == "amount")
            //    {
            //        if (Invoice.Discountpercent != null)
            //        {
            //            Model.Discount = Invoice.Discountpercent.Value;
            //        }
            //    }
            //    else
            //    {
            //        if (Invoice.Discountpercent != null)
            //        {
            //            Model.Discount = ((Invoice.Discountpercent / 100) * Model.SubTotal).Value;
            //        }
            //    }
            //}
            #endregion

            #region making Name of Address Bold
            //if (!string.IsNullOrWhiteSpace(Model.Invoice.BillingAddress))
            //{
            //    var split = Model.Invoice.BillingAddress.Split(new string[] { "\n" }, StringSplitOptions.None);

            //    if (split.Count() > 0)
            //    {
            //        string NewAddress = "";
            //        split[0] = "<b>" + split[0] + "</b>";

            //        foreach (var item in split)
            //        {
            //            NewAddress += item + Environment.NewLine;
            //        }
            //        Model.Invoice.BillingAddress = NewAddress;
            //    }
            //}
            //if (!string.IsNullOrWhiteSpace(Model.Invoice.ShippingAddress))
            //{
            //    var split = Model.Invoice.ShippingAddress.Split(new string[] { "\n" }, StringSplitOptions.None);

            //    if (split.Count() > 0)
            //    {
            //        string NewAddress = "";
            //        split[0] = "<b>" + split[0] + "</b>";

            //        foreach (var item in split)
            //        {
            //            NewAddress += item + Environment.NewLine;
            //        }
            //        Model.Invoice.ShippingAddress = NewAddress;
            //    }
            //}
            //if (!string.IsNullOrWhiteSpace(Model.InvoiceShipping))
            //{
            //    var split = Model.InvoiceShipping.Split(new string[] { "\n" }, StringSplitOptions.None);

            //    if (split.Count() > 0)
            //    {
            //        string NewAddress = "";
            //        split[0] = "<b>" + split[0] + "</b>";

            //        foreach (var item in split)
            //        {
            //            NewAddress += item + Environment.NewLine;
            //        }
            //        Model.InvoiceShipping = NewAddress;
            //    }
            //}
            #endregion

            //customer name is customer business name here 
            if (tempCUstomer.BusinessName != "")
            {
                Model.CustomerName = tempCUstomer.BusinessName;
            }
            else
            {
                Model.CustomerName = Model.Estimator.CustomerName;
            }
            Model.CusBussinessName = tempCUstomer.BusinessName;

            Model.SubTotal = 0;
            if (Model.estimatorDetails != null)
            {
                foreach (var item in Model.estimatorDetails)
                {
                    //   item.CreatedBy = User.Identity.Name;
                    item.CreatedDate = DateTime.Now.UTCCurrentTime();
                    //   item.CompanyId = CurrentUser.CompanyId.Value;
                    Model.SubTotal = Model.SubTotal + item.TotalPrice.Value;
                }
            }

            //if (string.IsNullOrWhiteSpace(Model.Invoice.InvoiceMessage))
            //{
            //    Model.Invoice.InvoiceMessage = _Util.Facade.GlobalSettingsFacade.GetInvoiceMessageByCompanyId(CurrentUser.CompanyId.Value);
            //}
            Model.CompanyAddress = tempCom.Address;
            Model.CompanyStreet = tempCom.Street;
            string ComCity = "";
            string ComState = "";
            if (!string.IsNullOrWhiteSpace(tempCom.City))
            {
                ComCity = tempCom.City.UppercaseFirst() + ", ";
            }
            if (!string.IsNullOrWhiteSpace(tempCom.State))
            {
                ComState = tempCom.State + " ";
            }
            #region Company Info
            Model.companyStreetInfo = ComCity + ComState + tempCom.ZipCode;
            Model.CompanyCity = tempCom.City.UppercaseFirst();
            Model.CompanyState = tempCom.State;
            Model.CompanyZip = tempCom.ZipCode;
            Model.CompanyPhone = tempCom.Phone;
            Model.CompanyEmail = tempCom.EmailAdress;
            Model.CompanyName = tempCom.CompanyName;
            Model.PhoneNo = tempCom.Phone;
            Model.CompanyWebsite = tempCom.Website;
            #endregion
            #region Customer Info
            Model.CustomerInfo = _Util.Facade.GlobalSettingsFacade.GetCustomerAddressFormat(comid);
            Model.CustomerCity = tempCUstomer.City.UppercaseFirst();
            Model.CustomerState = tempCUstomer.State;
            Model.CustomerZipCode = tempCUstomer.ZipCode;
            Model.CustomerNo = tempCUstomer.CustomerNo;
            Model.CustomerStreet = tempCUstomer.Street;
            Model.CompanyInfo = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(comid);
            //ViewBag.ShippingValue = _Util.Facade.GlobalSettingsFacade.GetShippingSettingCompanyId(tempCom.CompanyId);
            #endregion
            Model._EstimatorPDFFilter = EstimatorPDFFilters;

            Model.ShowEstimatorShippingAddress = _Util.Facade.GlobalSettingsFacade.GetShippingSettingCompanyId(comid).ToLower() == "true" ? true : false;

            if (string.IsNullOrWhiteSpace(tempCom.CompanyLogo))
            {
                tempCom.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(comid);
            }
            Model.CompanyLogo = tempCom.CompanyLogo;
            //if (!string.IsNullOrWhiteSpace(Model.Invoice.DiscountType))
            //{
            //    if (Model.Invoice.DiscountType != "amount")
            //    {
            //        if (Model.Invoice.Discountpercent.HasValue && Model.Invoice.Discountpercent.Value > 0)
            //        {
            //            Model.Discount = ((Model.Invoice.Discountpercent / 100) * Model.SubTotal).Value;
            //        }
            //    }
            //}
            //if (Model.Invoice.BalanceDue > 0)
            //{
            //    Model.AmountInWord = NumberToWords(Model.Invoice.BalanceDue.Value); //NumberToWords(Model.Invoice.TotalAmount.Value);
            //}
            return Model;
        }
        #endregion
    }
}