using HS.DataAccess;
using HS.Entities;
using HS.Framework;
using HS.Framework.Utils;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HS.Facade
{
    public class AgreementFacade : BaseFacade
    {
        public AgreementFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        AgreementQuestionDataAccess _AgreementQuestionDataAccess
        {
            get
            {
                return (AgreementQuestionDataAccess)_ClientContext[typeof(AgreementQuestionDataAccess)];
            }
        }
        AgreementAnswerDataAccess _AgreementAnswerDataAccess
        {
            get
            {
                return (AgreementAnswerDataAccess)_ClientContext[typeof(AgreementAnswerDataAccess)];
            }
        }
        EmailTemplateDataAccess _EmailTemplateDataAccess
        {
            get
            {
                return (EmailTemplateDataAccess)_ClientContext[typeof(EmailTemplateDataAccess)];
            }
        }
        GlobalSettingDataAccess _GlobalSettingDataAccess
        {
            get
            {
                return (GlobalSettingDataAccess)_ClientContext[typeof(GlobalSettingDataAccess)];
            }
        }

        PaymentInfoDataAccess _PaymentInfoDataAccess
        {
            get
            {
                return (PaymentInfoDataAccess)_ClientContext[typeof(PaymentInfoDataAccess)];
            }
        }

        PaymentInfoCustomerDataAccess _PaymentInfoCustomerDataAccess
        {
            get
            {
                return (PaymentInfoCustomerDataAccess)_ClientContext[typeof(PaymentInfoCustomerDataAccess)];
            }
        }

        PaymentProfileCustomerDataAccess _PaymentProfileCustomerDataAccess
        {
            get
            {
                return (PaymentProfileCustomerDataAccess)_ClientContext[typeof(PaymentProfileCustomerDataAccess)];
            }
        }

        CustomerDataAccess _CustomerDataAccess
        {
            get
            {
                return (CustomerDataAccess)_ClientContext[typeof(CustomerDataAccess)];
            }
        }

        ContractAgreementTemplateDataAccess _ContractAgreementTemplateDataAccess
        {
            get
            {
                return (ContractAgreementTemplateDataAccess)_ClientContext[typeof(ContractAgreementTemplateDataAccess)];
            }
        }

        CustomerAgreementTemplateDataAccess _CustomerAgreementTemplateDataAccess
        {
            get
            {
                return (CustomerAgreementTemplateDataAccess)_ClientContext[typeof(CustomerAgreementTemplateDataAccess)];
            }
        }

        public List<AgreementQuestion> GetAllAgreementQuestions()
        {
            return _AgreementQuestionDataAccess.GetByQuery(string.Format("IsActive = 'True'"));
        }
        public AgreementQuestion GetQuestionByQuestionId(int value)
        {
            return _AgreementQuestionDataAccess.Get(value);
        }
        public List<AgreementAnswer> GetAllAgreementAnswerByCustomerId(Guid cusid)
        {
            return _AgreementAnswerDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", cusid)).ToList();
        }
        public bool UpdateAgreementAnswer(AgreementAnswer aa)
        {
            return _AgreementAnswerDataAccess.Update(aa) > 0;
        }
        public long InsertAgreementAnswer(AgreementAnswer aa)
        {
            return _AgreementAnswerDataAccess.Insert(aa);
        }
        public bool DeleteAgreementAnswerByCustomerId(Guid id)
        {
            return _AgreementAnswerDataAccess.DeleteAgreementAnswerByCustomerId(id);
        }
        public List<AgreementAnswer> GetAllAgreementAnswerByCustomerIdAndQuestionId(Guid customerid)
        {
            return _AgreementAnswerDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerid)).ToList();
        }
        public AgreementAnswer GetAgreementAnswerByCustomerIdAndQuestionId(Guid cusid, int id)
        {
            return _AgreementAnswerDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and QuestionId = '{1}'", cusid, id)).FirstOrDefault();
        }
        public bool UpdateAgreementQues(AgreementQuestion aq)
        {
            return _AgreementQuestionDataAccess.Update(aq) > 0;
        }
        public List<AgreementAnswer> GetAllAgreementAnswerByCustomerIdAndQId(Guid cusid)
        {
            return _AgreementAnswerDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and (QuestionId = 1 or QuestionId = 3 or QuestionId = 5)", cusid)).ToList();
        }
        public List<AgreementAnswer> GetAllAgreementAnswerByCustomerIdAndQId1(Guid cusid)
        {
            return _AgreementAnswerDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and (QuestionId = 1 or QuestionId = 3 or QuestionId = 4 or QuestionId = 5)", cusid)).ToList();
        }
        public bool DeleteAgreementAnswerByCustomerIdAndQuesId(Guid id, int Quesid)
        {
            return _AgreementAnswerDataAccess.DeleteAgreementAnswerByCustomerIdAndQuesId(id, Quesid);
        }
        public string MakeAgreementPdf(InstallationAgreementModel agreementPdf)
        {
            string Body = "";
            string ContactName = "";
            string ContactPhone = "";
            string CenterSpace = "";
            string PhoneType = "";
            string ContactRelationship = "";
            string HasKey = "";
            string EmergencyContactList = "";
            string EquipmentName = "";
            string MonthlyRate = "";
            string DiscountRate = "";
            string Total = "";
            string EquipmentNameRab = "";
            string EquipmentPoint = "";
            string EquipmentQuantity = "";
            string EquipmentQuantityRab = "";
            string EquipmentRetail = "";
            string EquipmentCostRab = "";
            string EquipmentOldPrice = "";
            string EquipmentTotalPrice = "";
            string EquipmentTotalPriceRab = "";
            string EquipmentList = "";
            string EquipmentListRab = "";
            string ListAgreementAnswer = "";
            string divAnsval = "";
            string CustomerAgreement = "";
            string CustomerAgreementTable = "";
            string currentCurrency = "";
            if (agreementPdf.CurrentCurrency != null)
            {
                currentCurrency = agreementPdf.CurrentCurrency;
            }
            else
            {
                currentCurrency = "$";
            }
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("CompanyName", agreementPdf.CompanyName);
                templateVars.Add("CompanyStreet", agreementPdf.CompanyStreet);
                templateVars.Add("CompanySate", agreementPdf.CompanySate);
                templateVars.Add("CompanyWebsite", agreementPdf.CompanyWebsite);
                templateVars.Add("OwnerName", agreementPdf.OwnerName);
                templateVars.Add("BusinessName", agreementPdf.BusinessName);
                templateVars.Add("EffectiveDate", agreementPdf.EffectiveDate);
                templateVars.Add("OwnerAddress", agreementPdf.OwnerAddress);
                templateVars.Add("OwnerEmail", agreementPdf.OwnerEmail);
                templateVars.Add("OwnerPhone", agreementPdf.OwnerPhone);
                templateVars.Add("CustomerSignature", agreementPdf.CustomerSignature);
                templateVars.Add("CustomerSignatureDate", agreementPdf.CustomerSignatureStringDate);
                if (!string.IsNullOrWhiteSpace(agreementPdf.CustomerSignature))
                {
                    templateVars.Add("CompanySignature", agreementPdf.CompanySignature);
                    templateVars.Add("CompanySignatureDate", agreementPdf.CompanySignatureDate);
                }
                templateVars.Add("MonthlyMonitoringFee", currentCurrency + agreementPdf.MonthlyMonitoringFee);
                templateVars.Add("Tax", currentCurrency + FormatAmount(agreementPdf.TaxTotal));
                templateVars.Add("Total", currentCurrency + FormatAmount(agreementPdf.Total));
                templateVars.Add("CompanyLogo", agreementPdf.CompanyLogo);
                templateVars.Add("DateOfTransaction", agreementPdf.DateOfTransaction.AddDays(3).ToString());
                templateVars.Add("Date", agreementPdf.DateOfTransaction.AddDays(3).Day);
                templateVars.Add("Month", agreementPdf.DateOfTransaction.AddDays(3).ToString("MMMM"));
                templateVars.Add("Year", agreementPdf.DateOfTransaction.AddDays(3).ToString("yy"));
                templateVars.Add("SubscribedMonths", agreementPdf.SubscribedMonths);
                templateVars.Add("TotalPayments", currentCurrency + Convert.ToDouble(agreementPdf.SubscribedMonths) * Convert.ToDouble(agreementPdf.MonthlyMonitoringFee));
                templateVars.Add("Subtotal", currentCurrency + FormatAmount(agreementPdf.Subtotal));


                templateVars.Add("ActivationFee", currentCurrency + FormatAmount(agreementPdf.ActivationFee));


                if (!string.IsNullOrWhiteSpace(agreementPdf.SalesRepresentative) && agreementPdf.SalesRepresentative != "-1")
                {
                    templateVars.Add("SalesRepresentative", agreementPdf.SalesRepresentative);
                }
                else
                {
                    templateVars.Add("SalesRepresentative", "");
                }
                foreach (var item in agreementPdf.EmergencyContactList)
                {
                    ContactName = "<div style='width: 25%; float:left;'><span>&nbsp;" + item.FirstName + " " + item.LastName + " </span></div>";
                    ContactPhone = " <div style='width: 15%; float:left; text-align:center'><span> &nbsp;" + @item.Phone + "</span></div>";
                    CenterSpace = " <div style='width: 15%; float:left; text-align:center'><span> &nbsp;</span></div>";
                    if (item.PhoneType != "-1")
                    {
                        PhoneType = "<div style='width: 15%; float:left; text-align:center'><span>&nbsp;" + item.PhoneType + "</span></div>";
                    }
                    else
                    {
                        PhoneType = "<div style='width: 15%; float:left; text-align:center'><span>&nbsp;</span></div>";
                    }
                    if (item.RelationShip != "-1")
                    {
                        ContactRelationship = "<div style='width: 15%; float:left; text-align:center'><span>&nbsp;" + item.RelationShip + "</span></div>";
                    }
                    else
                    {
                        ContactRelationship = "<div style='width: 15%; float:left; text-align:center'><span>&nbsp;</span></div>";
                    }
                    if (item.HasKey == "True")
                    {
                        HasKey = " <div style='width: 15%; float:left; text-align:right'> <span>&nbsp;Yes</span></div>";
                    }
                    else
                    {
                        HasKey = " <div style='width: 15%; float:left; text-align:right'> <span>&nbsp;No</span></div>";
                    }
                    EmergencyContactList += ContactName + ContactPhone + CenterSpace + PhoneType + ContactRelationship + HasKey;
                }
                foreach (var item in agreementPdf.EquipmentList)
                {
                    EquipmentName = "  <div style = 'width:35%;float:left;padding-left:10px;'><span> &nbsp;" + item.Name + " </span></div> ";
                    EquipmentPoint = "  <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + item.Point + " </span></div> ";
                    EquipmentQuantity = " <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + item.QuantityAppointmentEquipment + " </span></div>";
                    EquipmentRetail = " <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + currentCurrency + FormatAmount(item.Retail) + " </span></div>";
                    var OldPrice = item.Retail * item.QuantityAppointmentEquipment;
                    EquipmentOldPrice = " <div style = 'width:10%;float:left;text-align:right'><span>" + currentCurrency + FormatAmount(OldPrice) + " </span></div>";
                    EquipmentTotalPrice = "  <div style = 'width:8%;float:left;text-align:right'><span> &nbsp;" + currentCurrency + FormatAmount(item.TotalPrice) + " </span></div>";
                    EquipmentList += " <div style='padding-top:5px;'>" + EquipmentName + EquipmentPoint + EquipmentQuantity + EquipmentRetail + EquipmentOldPrice + EquipmentTotalPrice + "</div>";
                }
                foreach (var item in agreementPdf.EquipmentList)
                {
                    EquipmentNameRab = "<div style = 'width:35%;float:left;font-weight:600;padding-left:10px;padding-top:5px;border-right:1px solid black'><span> " + item.Name + " </span></div> ";
                    //EquipmentPoint = "  <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + item.Point + " </span></div> ";
                    EquipmentQuantityRab = " <div style = 'width:15%;float:left;font-weight:600;text-align:center;padding-top:5px ;border-right:1px solid black'><span> " + item.QuantityAppointmentEquipment + " </span></div>";
                    EquipmentCostRab = " <div style = 'float:left;font-weight:600;text-align:center;padding-top:5px;'><span>" + currentCurrency + FormatAmount(item.TotalPrice) + " </span></div>";
                    //var OldPrice = item.Retail * item.QuantityAppointmentEquipment;
                    //EquipmentOldPrice = " <div style = 'width:10%;float:left;text-align:right'><span>" + currentCurrency + String.Format("{0:0,0.00}", OldPrice) + " </span></div>";
                    EquipmentTotalPriceRab = "<div style = 'width:15%;float:left;font-weight:600;text-align:center;padding-top:5px;border-right:1px solid black'><span><strike>" + currentCurrency + FormatAmount(item.TotalPrice) + " </strike></span></div>";
                    EquipmentListRab += " <div style='width:100%;border:1px solid black;float: left;'>" + EquipmentQuantityRab + EquipmentNameRab + EquipmentTotalPriceRab + EquipmentCostRab + "</div>";
                }
                if (agreementPdf.ListAgreementAnswer != null && agreementPdf.ListAgreementAnswer.Count > 0)
                {
                    string ansval = "";
                    foreach (var item in agreementPdf.ListAgreementAnswer)
                    {
                        if (item.QuestionId == 1)
                        {
                            if (item.Answer == "true")
                            {
                                ansval = "Yes";
                            }
                            else
                            {
                                ansval = "No";
                            }
                            divAnsval = "<div style='word-wrap: break-word;'>Are you the homeowner? (" + ansval + ")</div>";
                        }
                        if (item.QuestionId == 2)
                        {
                            if (item.Answer == "true")
                            {
                                ansval = "Yes";
                            }
                            else
                            {
                                ansval = "No";
                            }
                            divAnsval = "<div style='word-wrap: break-word;'>Is your home new construction? (" + ansval + ")</div>";
                        }
                        if (item.QuestionId == 3)
                        {
                            if (item.Answer == "true")
                            {
                                ansval = "Yes";
                            }
                            else
                            {
                                ansval = "No";
                            }

                            divAnsval = " <div style='word-wrap: break-word;'> Are you under any contractual agreement/ obligation with any other company for monitoring services? (" + ansval + ")</div> ";
                        }
                        if (item.QuestionId == 4)
                        {
                            if (item.Answer == "true")
                            {
                                ansval = "Yes";
                            }
                            else
                            {
                                ansval = "No";
                            }
                            divAnsval = " <div style='word-wrap: break-word;'>  I understand that the Company or any representaive of the Company cannot be responsible for cancelling any services with my current security company(if applicable). (" + ansval + ")</div> ";
                        }
                        if (item.QuestionId == 5)
                        {
                            if (item.Answer == "true")
                            {
                                ansval = "Yes";
                            }
                            else
                            {
                                ansval = "No";
                            }
                            String month = "";
                            if (agreementPdf.SubscribedMonths == "1")
                            {
                                month = "<span>month</span>";
                            }
                            else
                            {
                                month = "<span>months</span>";
                            }
                            divAnsval = "<div style = 'word-wrap: break-word;'> I understand that I have signed an agreement to receive monitoring services for<Span style = 'text-decoration:underline;font-weight :600;'> &nbsp; &nbsp; &nbsp; &nbsp;" + agreementPdf.SubscribedMonths + " &nbsp; &nbsp; &nbsp; &nbsp;</span><span>" + month + "</span></div>";
                        }
                        ListAgreementAnswer += divAnsval;
                    }
                }
                else
                {
                    ListAgreementAnswer = "<div style='word-wrap: break-word;'>1. Are you the homeowner?</div><div style = 'word-wrap: break-word;' > 2.Is your home new construction?</div><div style = 'word-wrap: break-word;' >3.Are you under any contractual agreement / obligation with any other company for monitoring services?</div> <div style = 'word-wrap: break-word;' > 4.I understand that the Company or any representaive of the Company cannot be responsible for cancelling any services with my current security company(if applicable).</div><div style = 'word-wrap: break-word;' >5.I understand that I have signed an agreement to receive monitoring services for<span style = 'text-decoration:underline;font-weight :600;' > &nbsp; &nbsp; &nbsp; &nbsp;" + agreementPdf.SubscribedMonths + "&nbsp;&nbsp;&nbsp;&nbsp;</span> <span> month </span></div>";
                }
                if (agreementPdf.CustomerAgreement != null)
                {
                    foreach (var item in agreementPdf.CustomerAgreement)
                    {

                        if (item.Type == "LoadAgreement")
                        {
                            CustomerAgreement = "<span> Load: </span><span style = 'font-weight:600;'>" + DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("MM/dd/yyyy") + "+ at+" + DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("hh:mm tt") + " </span><br/>";
                        }
                        if (item.Type == "SignAgreement")
                        {
                            CustomerAgreement = "<span> Sign: </span><span style = 'font-weight:600;'>" + DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("MM/dd/yyyy") + "+ at+" + DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("hh:mm tt") + " </span><br/>";

                        }
                        if (item.Type == "SubmitAgreement")
                        {
                            CustomerAgreement = "<span> Submit: </span><span style = 'font-weight:600;'>" + DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("MM/dd/yyyy") + "+ at+" + DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("hh:mm tt") + " </span><br/>";
                        }
                    }
                    CustomerAgreementTable = "<table style='width:100%'><tr style='background-color:darkgray;'><th style='width:33%; text-align:center;'>IP</th><th style='width:33%; text-align:center;'>USER AGENT</th><th style='width:33%; text-align:center;'>TIMESTAMP</th> </tr><tr><td style='width:33%; text-align:center;'>" + agreementPdf.SingleCustomerAgreement.IP + "</td><td style='width:33%; text-align:center;'>" + agreementPdf.SingleCustomerAgreement.UserAgent + "</td><td style='width:33%; text-align:center;'>" + CustomerAgreement + " </td></tr>";
                }
                templateVars.Add("EmergencyContactList", EmergencyContactList);
                templateVars.Add("EquipmentList", EquipmentList);
                templateVars.Add("EquipmentListRab", EquipmentListRab);
                templateVars.Add("ListAgreementAnswer", ListAgreementAnswer);
                templateVars.Add("CustomerAgreementTable", CustomerAgreementTable);
                EmailParser parser = null;
                EmailTemplate agreementTemplate = _EmailTemplateDataAccess.GetEmailTemplateByKeyAndCompanyId(agreementPdf.CompanyId, EmailTemplateKey.Agreement.AgreementRMR);


                parser = new EmailParser(HttpContext.Current.Server.MapPath(agreementTemplate.BodyFile), templateVars, true);
                MailMessage message = new MailMessage();
                message.Body = parser.Parse();
                Body = message.Body;



            }
            catch (Exception ex)
            {
                //Logger.AddElmah(ex);
            }

            return Body;
        }
        public string FormatAmount(double? value)
        {
            string formatted = "0.00";
            if (value.HasValue)
            {
                formatted = string.Format(CultureInfo.InvariantCulture, "{0:N}", value);

            }
            return formatted;
        }

        #region Estimator detail list
        private string GetEstimatorLineItems(List<HS.Entities.EstimatorDetail> estimatorDetails, string Currency, HS.Entities.EstimatorPDFFilter filters)
        {
            string result = "";
            double TotalCatagoryCost = 0;
            foreach (HS.Entities.EstimatorDetail item in estimatorDetails)
            {
                TotalCatagoryCost += item.TotalPrice.HasValue ? item.TotalPrice.Value : 0.0;

                result += "<tr style='border-bottom:1px solid #ccc;'>"
              + "<td style='padding:5px 0px 5px 40px;'>";
                if (filters.IncludeImage.Value == true && item.EquipmentFile != null && !string.IsNullOrWhiteSpace(item.EquipmentFile.Filename))
                {
                    result += "<img src ='" +
                        item.EquipmentFile.Filename +
                        "' alt='Alternate Text' style='width:25px;height:25px'/>";
                }
                if (!string.IsNullOrWhiteSpace(item.PartDescription))
                {
                    result += "<span>"
                        + item.PartDescription +
                        "</span><br/>";
                }
                if (filters.IncludeManufacturer.Value == true)
                {
                    result += "<span>" +
                        item.Manufacturer +
                        "</span>";
                }
                result += "</td>" +
             "<td style='padding:5px 0px 5px 40px;'>";
                if (!string.IsNullOrWhiteSpace(item.PartNumber))
                {
                    result += "<b>" +
                        item.PartNumber
                        + "</b>";
                }
                if (filters.IncludeVariation.Value)
                {
                    result += "<span>" +
                        item.Variation
                        + "</span>";
                }
                result += "</td>"
             + "<td style='padding:5px 0px 5px 40px;'>";
                if (item.Qunatity != null)
                {
                    result += "<span>" +
                        item.Qunatity +
                        "</span><br/>";
                }
                result += "</td>";
                if (filters.IncludeCost.HasValue && filters.IncludeCost.Value == true)
                {
                    result += "<td style='padding:5px 0px 5px 40px;'>";
                    if (item.UnitCost != null)
                    {
                        result += "<span>" +
                            Currency + FormatAmount(item.UnitCost) +
                            "</span><br/>";
                    }
                    result += "</td>"
                + "<td style='padding:5px 0px 5px 40px;'>";
                    if (item.TotalCost != null)
                    {
                        result += "<span>" +
                            Currency + FormatAmount(item.TotalCost)
                            + "</span><br/>";
                    }
                    result += "</td>";
                }
                if (filters.IncludeProfit.Value || filters.IncludeMargin.Value)
                {
                    result += "<td style='padding:5px 0px 5px 40px;'>";
                    if (item.Profit != null)
                    {
                        result += "<span>" +
                            Currency + FormatAmount(item.Profit) +
                            "</span><br/>";
                    }
                    result += "</td>";
                }
                if (filters.IncludeOverhead.Value)
                {
                    result += "<td>";
                    if (item.Overhead != null)
                    {
                        result += "<span>" +
                            Currency + FormatAmount(item.Overhead) +
                            "</span><br/>";
                    }
                    result += "</td>";
                }
                if (filters.WithoutPricing.Value
                    || (filters.WithoutIndividualMaterialPricing.Value && item.CategoryVal != "Labor")
                    || (filters.WithoutIndividualLaborPricing.Value && item.CategoryVal == "Labor"))
                {

                }
                else
                {
                    result += "<td>";
                    if (item.TotalPrice != null)
                    {
                        result += "<span>" +
                            Currency + FormatAmount(item.TotalPrice / item.Qunatity)
                            + "</span><br/>";
                    }
                    result += "</td>"
                + "<td style='text-align:right;'>";
                    if (item.TotalPrice != null)
                    {
                        result += "<span>" +
                            Currency + FormatAmount(item.TotalPrice)
                            + "</span><br/>";
                    }
                    result += "</td>";
                }
                result += "</tr>";
            }

            if (!filters.WithoutPricing.Value && !filters.GroupedbyNone.Value)
            {
                result += "<tr>"
                + "<td style='padding:5px 10px 5px 40px; text-align:right;' colspan='10'>"
                + "<b>"
                + "<span>"
                + "Sub Total: "
                + Currency + FormatAmount(TotalCatagoryCost)
                + "</span></b>"
                + "</td>"
                + "</tr>";
                TotalCatagoryCost = 0;
            }
            return result;
        }
        #endregion


        //public string UploadFileToAWSS3(string localFilePath, string bucketName, string subDirectoryInBucket, string fileNameInS3)
        //{
        //    string returnurl = "";

        //    IAmazonS3 client = Amazon.AWSClientFactory.CreateAmazonS3Client(RegionEndpoint.EUWest1); // need regionendpoint for our bucket

        //    // create a TransferUtility instance passing it the IAmazonS3 created in the first step
        //    TransferUtility utility = new TransferUtility(client);

        //    // making a TransferUtilityUploadRequest instance
        //    TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();

        //    if (string.IsNullOrEmpty(subDirectoryInBucket))
        //    {
        //        request.BucketName = bucketName; //no subdirectory just bucket name
        //    }
        //    else
        //    {   // subdirectory and bucket name
        //        request.BucketName = bucketName + @"/" + subDirectoryInBucket;
        //    }

        //    request.Key = fileNameInS3; //file name up in S3
        //    request.FilePath = localFilePath; //local file name
        //    utility.Upload(request); //commensing the transfer



        //    return returnurl;
        //}
        public string MakeSmartAgreementPdf(InstallationAgreementModel agreementPdf, int? templateid)
        {
            string Body = "";
            string ContactName = "";
            string ContactPhone = "";
            string CenterSpace = "";
            string PhoneType = "";
            string ContactRelationship = "";
            string HasKey = "";
            string ContactNameHeader = "";
            string ContactPhoneHeader = "";
            string PhoneTypeHeader = "";
            string ContactRelationshipHeader = "";
            string EmergencyContactList = "";
            string EstimateEmergencyContactList = "";
            string EquipmentName = "";
            string ServiceName = "";
            string MonthlyRate = "";
            string DiscountRate = "";
            string Total = "";
            string UnitPrice = "";

            string CommercialEquipmentList = "";
            string CommercialServiceList = "";

            string InvoiceQTY = "";
            string InvoiceName = "";
            string InvoicePrice = "";
            string InvoiceSubTotal = "";
            double InvoiceTotalSubTotal = 0.0;
            double EstimateUpfrontTotalSubTotal = 0.0;
            double EstimateRecurringTotalSubTotal = 0.0;
            double InvoiceTotalSubTotalWithUpfront = 0.0;
            double InvoiceFinalTotal = 0.0;
            double EstimateSigningAmount = 0.0;
            double EstimateSigningAmountWithProrate = 0.0;
            string InvoiceList = "";
            string FinancedAmount = "";

            string ProductDFW = "";
            string SKUDFW = "";
            string QuantityDFW = "";
            string UnitPriceDFW = "";
            string DiscountUnitPriceDFW = "";
            string TotalDiscountDFW = "";
            string TotalPriceDFW = "";

            string EquipmentQuantity = "";
            string DiscountUnitPrice = "";
            string TotalEquipment = "";
            string EquipmentList = "";
            string EquipmentListRab = "";
            string EquipmentListDFW = "";
            string EquipmentListTable = "";
            string ServiceList = "";
            string ServiceListRab = "";
            string ServiceListDFW = "";
            string ServiceListOnit = "";
            string SmartPackageEquipmentServiceList = "";
            string ListAgreementAnswer = "";
            string divAnsval = "";
            string CustomerAgreement = "";
            string CustomerAgreementTable = "";
            string currentCurrency = "";
            string Upfronaddon = "";
            string Service = "";
            string UpFront = "";
            string Qty = "";
            string TotalDfw = "";
            string MonthlyService = "";
            string Type = "";
            string Monthly = "";
            string ServiceListTable = "";
            bool isServiceDetail = false;
            string CardOrAccNo = "";
            string AccountType = "";
            var SSN = "";
            var OriginalContactDateTemplate = "";
            string AgreementAccountMessage = "By signing here you agree to authorize the company or its agents or assigns to initiate monthly debit entries to your checking, savings,or {0} ({1}) for the Total Monthly Amount referenced in paragraph 2.2with the depository or credit card company named above.";
            string AgreementAccountMessageInvoice = "By signing here you agree to authorize the company or its agents or assignee to initiate monthly invoice at the address above.";
            string CardTypeMMR = "";
            string SubscribedMonthsText = "";
            string RenewalMonthsText = "";
            string NotARBEnabledServiceList = "";
            string LabourfeeDfw = "";
            string LabourFeeRmR = "";

            string EstimatorDetailList = "";
            string EstimateUpfrontChargeList = "";
            string EstimateRecurringChargeList = "";
            string EstimatorProduct = "";
            string EstimatorSKU = "";
            string EstimatorQTY = "";
            string EstimatorUCOST = "";
            string EstimatorTCOST = "";
            string EstimatorPROFIT = "";
            string EstimatorOVERHEAD = "";
            string EstimatorServiceList = "";
            string EstimatorSerSERVICE = "";
            string EstimatorSerQTY = "";
            string EstimatorSerUNITPRICE = "";
            string EstimatorSerTOTALPRICE = "";
            string StandardPlan = "";
            string PremiumPlan = "";

            string NonConfirmingFeeDivCommFire = string.Empty;
            string residentialSearchKey = "ResidentialTechFirstHourCost";
            GlobalSetting ResidentialTechFirstHourCost = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", agreementPdf.CompanyId, residentialSearchKey)).FirstOrDefault();

            if (ResidentialTechFirstHourCost != null)
            {
                string value = ResidentialTechFirstHourCost.Value;
                Console.WriteLine("The value for the key 'ResidentialTechFirstHourCost' is: " + value);
                agreementPdf.ResidentialTechFirstHourCost = value;
            }
            else
            {
                Console.WriteLine("No setting found for the specified key.");
            }
            string commercialSearchKey = "CommercialTechFirstHourCost";
            GlobalSetting commercialTechFirstHourCost = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", agreementPdf.CompanyId, commercialSearchKey)).FirstOrDefault();

            if (commercialTechFirstHourCost != null)
            {
                string value = commercialTechFirstHourCost.Value;
                Console.WriteLine("The value for the key 'CommercialTechFirstHourCost' is: " + value);
                agreementPdf.CommercialTechFirstHourCost = value;
            }
            else
            {
                Console.WriteLine("No setting found for the specified key.");
            }

            bool IsBBBConflict = false;
            if (agreementPdf.CustomerAgreement != null)
            {
                IsBBBConflict = agreementPdf.CustomerAgreement.Where(m => m.Type == "AgreementComplete").Count() > 0;
            }
            double AdvanceMonitoringFee = 0.0;
            double MonthlyServiceFee = 0.0;
            GlobalSetting glLabourFee = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", agreementPdf.CompanyId, "HasLabourFee")).FirstOrDefault();

            if (glLabourFee != null)
            {
                if (glLabourFee.Value == "true")
                {
                    LabourfeeDfw = string.Format(@"  <tr style='height:25px;'>
                        <td valign='middle' style='font-weight:bold; border:1px solid #000; text-align:right; padding-right:5px; background-color:#f3f3f3;'>+  Labor Fee</td>
                        <td style='border:1px solid #000;text-align: right;padding-right: 10px;'>{0}</td>
                    </tr>", currentCurrency + FormatAmount(agreementPdf.LabourFee));
                    LabourFeeRmR = string.Format(@" <div style='width:100%;float:left;margin-top:5px;border-top:1px solid;margin-bottom:5px'>
                                    <div style='width:80%;float:left;padding-left:10px;padding-top:5px'>
                                        <span>Labor Fee</span>
                                    </div>
                                    <div style='width:18%;float:left;text-align:right;padding-top:5px'>
                                        {0}
                                    </div>
                                </div>", agreementPdf.LabourFee);
                }
                else
                {
                    LabourfeeDfw = "";
                    LabourFeeRmR = "";
                }
            }

            if (!string.IsNullOrWhiteSpace(agreementPdf.SubscribedMonths) && agreementPdf.SubscribedMonths.ToLower() != "month to month")
            {
                if (agreementPdf.SubscribedMonths == "1")
                {
                    SubscribedMonthsText = "Month";
                }
                else
                {
                    SubscribedMonthsText = "Months";
                }
            }
            if (agreementPdf.RenewalMonths == 1)
            {
                RenewalMonthsText = "Month";
            }
            else
            {
                RenewalMonthsText = "Months";
            }
            var ArbEnabledServiceList = agreementPdf.ServiceList.Where(m => m.IsARBEnabled == true).ToList();
            DataTable dt = _PaymentInfoDataAccess.GetAllPaymentInfoByCustomerIdAndCompanyId(agreementPdf.CustomerAgreement.FirstOrDefault().CustomerId, new Guid(agreementPdf.CompanyId));
            List<PaymentInfo> PaymentInfoList = new List<PaymentInfo>();
            PaymentInfoList = (from DataRow dr in dt.Rows
                               select new PaymentInfo()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   CompanyId = (Guid)dr["CompanyId"],
                                   AccountName = dr["AccountName"].ToString(),
                                   BankAccountType = dr["BankAccountType"].ToString(),
                                   RoutingNo = dr["RoutingNo"].ToString(),
                                   AcountNo = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["AcountNo"].ToString()),
                                   CardType = dr["CardType"].ToString(),
                                   CardNumber = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["CardNumber"].ToString()),
                                   CardExpireDate = dr["CardExpireDate"].ToString(),
                                   CardSecurityCode = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["CardSecurityCode"].ToString()),
                                   PayFor = dr["PayFor"].ToString(),
                                   Type = dr["Type"].ToString(),
                                   PaymentCustomerId = (Guid)dr["PaymentCustomerId"],
                                   CheckNo = dr["CheckNo"].ToString(),
                                   EcheckType = dr["EcheckType"].ToString(),
                                   BankName = dr["BankName"].ToString(),
                               }).ToList();
            var MMRPaymentProfile = PaymentInfoList.Where(m => m.PayFor == "MMR").FirstOrDefault();
            if (MMRPaymentProfile != null)
            {
                CardTypeMMR = MMRPaymentProfile.CardType;
            }
            var CreditCardInfo = PaymentInfoList.Where(x => x.CardSecurityCode != "" && x.CardNumber != "" && x.CardExpireDate != "" && x.PayFor == "MMR").FirstOrDefault();
            var ACHInfo = PaymentInfoList.Where(x => x.BankAccountType != "" && x.EcheckType != "" && x.RoutingNo != "" && x.AcountNo != "" && x.PayFor == "MMR").FirstOrDefault();
            if (CreditCardInfo != null)
            {
                AccountType = "Credit card account no ";
                CardOrAccNo = string.Concat("".PadLeft(12, '*'), ((CreditCardInfo.CardNumber).Substring(((CreditCardInfo.CardNumber).Length - 4))));
            }
            else if (ACHInfo != null && ACHInfo.AcountNo.Length > 4)
            {
                AccountType = "ACH account no ";
                CardOrAccNo = ACHInfo.Id > 0 && ACHInfo.AcountNo != null ? string.Concat("".PadLeft(ACHInfo.AcountNo.Length - 4, '*'), ACHInfo.AcountNo.Substring(ACHInfo.AcountNo.Length - 4)) : "";
            }

            GlobalSetting gl = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", agreementPdf.CompanyId, "AgreementServiceDetails")).FirstOrDefault();

            if (gl != null)
            {
                if (gl.Value == "true")
                {
                    isServiceDetail = true;
                }
                else
                {
                    isServiceDetail = false;
                }
            }
            else
            {
                isServiceDetail = false;
            }

            if (agreementPdf.CurrentCurrency != null)
            {
                currentCurrency = agreementPdf.CurrentCurrency;
            }
            else
            {
                currentCurrency = "$";
            }
            agreementPdf.ACHDiscountAmount = 0;
            var objpayinfocus = _PaymentInfoCustomerDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and Payfor = 'MMR'", agreementPdf.CustomerAgreement.FirstOrDefault().CustomerId)).FirstOrDefault();
            if (objpayinfocus != null)
            {
                var objpayprofile = _PaymentProfileCustomerDataAccess.GetByQuery(string.Format("PaymentInfoId = {0}", objpayinfocus.PaymentInfoId)).FirstOrDefault();
                if (objpayprofile != null && objpayprofile.Type.ToLower().IndexOf("ach_") > -1)
                {
                    var objglobal = _GlobalSettingDataAccess.GetByQuery(string.Format("SearchKey = 'ACHDiscount'")).FirstOrDefault();
                    if (objglobal != null)
                    {
                        agreementPdf.ACHDiscountAmount = Convert.ToDouble(objglobal.Value);
                    }
                }
            }
            try
            {
                Hashtable templateVars = new Hashtable();
                if (CardTypeMMR == "Invoice")
                {
                    templateVars.Add("AgreementAccountMessage", AgreementAccountMessageInvoice);
                }
                else
                {
                    AgreementAccountMessage = string.Format(AgreementAccountMessage, AccountType, CardOrAccNo);
                    templateVars.Add("AgreementAccountMessage", AgreementAccountMessage);
                }
                templateVars.Add("CardOrAccNo", CardOrAccNo);
                templateVars.Add("PaymentAccType", AccountType);
                templateVars.Add("CSIDNumber", agreementPdf.CSIDNumber);
                templateVars.Add("LeadSource", agreementPdf.LeadSource);
                templateVars.Add("FileId", !string.IsNullOrWhiteSpace(agreementPdf.FileId) ? agreementPdf.FileId : "");
                templateVars.Add("IPAddress", !string.IsNullOrWhiteSpace(agreementPdf.CusSignIP) ? agreementPdf.CusSignIP : "");
                if (agreementPdf.OriginalContactDate != new DateTime())
                {
                    OriginalContactDateTemplate = "<tr style='height: 30px;'><td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>Original Contract Date</td><td style='border: 2px solid #000; font-weight:bold; padding-left:10px;'>" + agreementPdf.OriginalContactDate.ToString("MM/dd/yy") + "</td></tr>";
                }
                templateVars.Add("InstallDate", OriginalContactDateTemplate);
                templateVars.Add("EstimateInstallDate", DateTime.Now.DateFormat());

                templateVars.Add("AccountType", !string.IsNullOrWhiteSpace(agreementPdf.AccountType) && agreementPdf.AccountType != "-1" ? agreementPdf.AccountType : "");
                templateVars.Add("Referredby", agreementPdf.Referredby);
                if (!string.IsNullOrEmpty(agreementPdf.SocialSecurityNumber) && agreementPdf.SocialSecurityNumber.Length > 4)
                {
                    var FormateSSN = agreementPdf.SocialSecurityNumber.Substring(agreementPdf.SocialSecurityNumber.Length - 4);
                    SSN = String.Format("{0:xxx-xx-0000}", Convert.ToInt32(FormateSSN));
                }
                templateVars.Add("FirstName", agreementPdf.FirstName);
                templateVars.Add("LastName", agreementPdf.LastName);
                templateVars.Add("SocialSecurityNumber", SSN);
                templateVars.Add("Owner2ndPhone", Extentions.PhoneNumberFormatNew(agreementPdf.Owner2ndPhone));
                templateVars.Add("InstallAddress", agreementPdf.InstallAddress);
                templateVars.Add("InitialStreet", agreementPdf.InitialStreet);
                templateVars.Add("InitialCity", agreementPdf.InitialCity);
                templateVars.Add("InitialCountry", agreementPdf.InitialCountry);
                templateVars.Add("InitialState", agreementPdf.InitialState);
                templateVars.Add("InitialZip", agreementPdf.InitialZip);
                templateVars.Add("BillingStreet", agreementPdf.BillingStreet);
                templateVars.Add("BillingCity", agreementPdf.BillingCity);
                templateVars.Add("BillingCountry", agreementPdf.BillingCountry);
                templateVars.Add("BillingState", agreementPdf.BillingState);
                templateVars.Add("BillingZip", agreementPdf.BillingZip);
                templateVars.Add("InitialApt", agreementPdf.InitialApt);
                templateVars.Add("InstallTypeName", agreementPdf.InstallTypeName);
                templateVars.Add("DateOfAgreement", DateTime.Now.DateFormat());

                if (agreementPdf.InstallTypeName.ToLower() == "prewire" || agreementPdf.InstallTypeName.ToLower() == "upgrade")
                {
                    templateVars.Add("CustomerSignatureCondition", "<b>*PREWIRED OR UPGRADED SYSTEMS MUST Sign HERE</b><img src='" + agreementPdf.CustomerSignature + "' style='width:300px; height:100px;' />");
                }
                double collectedamount = 0.0;
                var CustomerPaymentInfo = GetAllPaidPaymentInfoCustomer(new Guid(agreementPdf.CompanyId), agreementPdf.CustomerAgreement.FirstOrDefault().CustomerId);
                if (agreementPdf.AdvanceServiceFeeTotal > 0)
                {
                    AdvanceMonitoringFee = agreementPdf.AdvanceServiceFeeTotal;
                }

                
                var ContractCreatedDateVal = agreementPdf.ContractCreatedDateVal;
                if (string.IsNullOrEmpty(ContractCreatedDateVal))
                {
                    ContractCreatedDateVal = DateTime.UtcNow.ToString("M/d/yyyy");
                }

                var upfrontamount = agreementPdf.UpfrontAddOnTotal;
                var EquipDiscount = 0.00;
                var SubTotalBeforeDiscount = 0.00;
                var EquipIsPcnt = false;
                var EqpActualDiscount = 0.00;
                var EqpDiscAmount = 0.00;
                if (agreementPdf.ACHDiscountAmount > 0)
                {

                    templateVars.Add("ACHDiscountAmount", "<tr style='background-color:#bfbfbf; color:#000; height: 30px;'><td style='text-align:center; border: 2px solid #000; border-right:1px solid #000; padding:0px 10px; font-size:15px; font-weight:bold;' colspan = 4>Discount</td><td style='text-align:center; border: 2px solid #000; border-left:1px solid #000;'><span>-</span>" + currentCurrency + FormatAmount(agreementPdf.ACHDiscountAmount) + "</td></tr>");
                    var serviceFeeTotal = agreementPdf.MonthlyServiceFeeTotal - agreementPdf.ACHDiscountAmount;

                    if (agreementPdf.IsUpfrontPromo == true)
                    {
                        upfrontamount = agreementPdf.UpfrontAddOnTotalPromo;
                    }

                    var activationamount = agreementPdf.ActivationFee;
                    var onetimeservicefee = 0.0;
                    if (agreementPdf.NotARBEnabledTotalPrice > 0)
                    {
                        onetimeservicefee = agreementPdf.NotARBEnabledTotalPrice;
                        //templateVars.Add("OneTimeServiceFee", "<tr style='height: 25px;'><td valign='middle' style='font-weight:bold; border: 1px solid #000; text-align:right; padding-right:5px; background-color:#f3f3f3;'>+  One Time Service Fee</td><td style='border: 1px solid #000;text-align: right;padding-right: 10px;'>" + currentCurrency + FormatAmount(onetimeservicefee) + "</td></tr>");
                    }
                    agreementPdf.NonConfirmingFee = agreementPdf.NonConfirmingFee;
                    if (agreementPdf.IsServicePromo != true)
                    {
                        MonthlyServiceFee = serviceFeeTotal;
                    }
                    var payCus = _PaymentInfoCustomerDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and Payfor = 'Service'", agreementPdf.CustomerAgreement.FirstOrDefault().CustomerId)).FirstOrDefault();
                    if (payCus != null && payCus.ForMonths.HasValue && payCus.ForMonths > 1)
                    {
                        AdvanceMonitoringFee = MonthlyServiceFee * (payCus.ForMonths.Value - 1);
                    }
                    var subtotalamount = MonthlyServiceFee + AdvanceMonitoringFee + upfrontamount + activationamount + agreementPdf.NonConfirmingFee + onetimeservicefee;
                    double TaxableSubtotal = 0.0;

                    if (glLabourFee != null && glLabourFee.Value == "true")
                    {
                        subtotalamount = subtotalamount + agreementPdf.LabourFee;
                        TaxableSubtotal = subtotalamount - agreementPdf.LabourFee;
                    }
                    else
                    {
                        TaxableSubtotal = subtotalamount;
                    }
                    agreementPdf.TaxTotal = TaxableSubtotal * (agreementPdf.Tax / 100);
                    var duesignamount = subtotalamount + agreementPdf.TaxTotal;
                    if (agreementPdf.IsServicePromo != true)
                    {
                        MonthlyServiceFee = serviceFeeTotal;
                    }

                    ////  " Mayur " Discount change if ACH payment" :: Start
                    foreach (Equipment d in agreementPdf.EquipmentList)
                    {
                        if (d.DiscountInCurrency > 0.0)
                        {
                            EquipIsPcnt = false;
                            EquipDiscount = d.DiscountInCurrency;
                        }
                        else
                        {
                            EquipIsPcnt = true;
                            EquipDiscount = d.DiscountPercentage;
                        }
                        break;
                    }
                    if (EquipIsPcnt)
                    {
                        EqpDiscAmount = ((upfrontamount * EquipDiscount) / 100);
                    }
                    else
                    {
                        EqpDiscAmount = EquipDiscount;
                    }
                    EqpActualDiscount = EqpDiscAmount;
                    subtotalamount = MonthlyServiceFee + AdvanceMonitoringFee + upfrontamount + activationamount + agreementPdf.NonConfirmingFee + onetimeservicefee;
                    SubTotalBeforeDiscount = subtotalamount;
                    subtotalamount = subtotalamount - EqpActualDiscount;



                    TaxableSubtotal = 0.0;
                    if (glLabourFee != null && glLabourFee.Value == "true")
                    {
                        subtotalamount = subtotalamount + agreementPdf.LabourFee;
                        TaxableSubtotal = subtotalamount - agreementPdf.LabourFee;
                    }
                    else
                    {
                        TaxableSubtotal = subtotalamount;
                    }
                    agreementPdf.TaxTotal = TaxableSubtotal * (agreementPdf.Tax / 100);
                    duesignamount = subtotalamount + agreementPdf.TaxTotal;

                    //// Mayur :: New onetimeservice + services join showing in aggrement :: Start

                    var OnetimeServiceContent = "";
                    var OnetimeServiceItems = "";
                    var subtotal = 0.0;
                    var subtotal1 = 0.0;
                    var subtotal2 = 0.0;
                    var tax1 = 0.0;
                    var tax2 = 0.0;
                    var totaltax = 0.0;
                    var Finaltotal = 0.0;


                    if (!string.IsNullOrWhiteSpace(agreementPdf.ContractType) && agreementPdf.ContractType.ToLower() == "commercialfire")
                    {
                        if (agreementPdf.ServiceList.Count > 0)
                        {
                            foreach (var item in agreementPdf.ServiceList)
                            {
                                OnetimeServiceItems = OnetimeServiceItems +
                                "<tr style='border-bottom:1px solid #ccc'>" +
                                "<td style=\"padding:5px 0px 5px 20px\">" +
                                "<strong><label>" + item.Name + "</label></strong>" +
                                "</td>" +
                                "<td style=\"padding:5px;text-align:center\"><strong><label>" + currentCurrency + FormatAmount(item.Total) + "</label></strong></td>" +
                                "</tr>";

                                subtotal1 += subtotal + (double)item.Total;

                                tax1 += subtotal1 * (agreementPdf.Tax / 100);

                            }

                        }
                        if (agreementPdf.NotARBEnabledServiceList.Count > 0)
                        {
                            foreach (var item in agreementPdf.NotARBEnabledServiceList)
                            {
                                OnetimeServiceItems = OnetimeServiceItems +
                                "<tr style='border-bottom:1px solid #ccc'>" +
                                "<td style=\"padding:5px 0px 5px 20px\">" +
                                "<strong><label>" + " + One Time Service Fee - " + item.Name + "</label></strong>" +
                                "</td>" +
                                "<td style=\"padding:5px;text-align:center\"><strong><label>" + currentCurrency + FormatAmount(item.Total) + "</label></strong></td>" +
                                "</tr>";

                                subtotal2 += subtotal + (double)item.Total;
                                tax2 += subtotal2 * (agreementPdf.Tax / 100);



                            }
                        }

                        if (agreementPdf.NonConfirmingFee > 0)
                        {
                            NonConfirmingFeeDivCommFire = "<tr style='border-bottom:1px solid #ccc'><td style=\"padding:5px 0px 5px 20px\"><strong><label> + Non Conforming Fee </label></strong></td><td style=\"padding:5px;text-align:center\"><strong><label>{0}</label></strong></td></tr>";
                            NonConfirmingFeeDivCommFire = string.Format(NonConfirmingFeeDivCommFire, currentCurrency + FormatAmount(agreementPdf.NonConfirmingFee));
                            OnetimeServiceItems += NonConfirmingFeeDivCommFire;
                            subtotal2 += subtotal + agreementPdf.NonConfirmingFee;
                            tax2 = subtotal2 * (agreementPdf.Tax / 100);
                        }

                        subtotal = subtotal1 + subtotal2;
                        totaltax = tax1 + tax2;
                        Finaltotal = Finaltotal + subtotal + totaltax;
                        OnetimeServiceContent = "<table style=\"width:100%;float:left;border-collapse:collapse;margin-bottom:30px;margin-top:10px;\">" +
                           " <thead> " +
                           " <tr style = 'background-color:#4f90bb;color:white;/* width:100%; */border:1px solid #4f90bb' >" +
                           " <th style = 'width: 80%;text-align:left;padding:5px 0px 5px 20px;border-right:1px solid #fff' > SERVICE </th>" +
                           " <th style = 'width: 14%;padding:5px 0px;border-right:1px solid #fff;text-align:center' > PRICE </th></tr>" +
                           " </thead > <tbody>" +
                           OnetimeServiceItems +

                           /// subtotal
                           "<tr style = '/* border-bottom:1px solid #ccc; */' >" +
                           "<td style = 'border: 0px 0px 0px 5px;text-align: right;border: none;' ><strong><label> SubTotal </label></strong></td>" +
                           "<td style = 'padding:5px;text-align:center' ><strong><label>" + currentCurrency + FormatAmount(subtotal) + " </label></strong></td>" +
                           "</tr> " +

                           /// totalTax
                           "<tr style = '/* border-bottom:1px solid #ccc; */'>" +
                           "<td style = 'padding:0px 0px 0px 10px;text-align: right;' ><strong><label> Tax </label></strong></td> " +
                           "<td style = 'padding:0px;text-align:center' ><strong><label>" + currentCurrency + FormatAmount(totaltax) + " </label></strong></td>" +
                           "</tr> " +

                           // Total

                           "<tr style = '/* border-bottom:1px solid #ccc; */' >" +
                           "<td style = 'padding:0px 0px 0px 5px;text-align: right; ' ><strong><label> Total </label></strong></td>" +
                           "<td style = 'padding:5px;text-align:center' ><strong><label>" + currentCurrency + FormatAmount(Finaltotal) + "</label></strong></td>" +
                           "</tr>" +



                           " </tbody></table> ";

                        templateVars.Add("OneTimeServiceFee", OnetimeServiceContent);
                        templateVars.Remove("CommercialServiceList");
                    }
                    else
                    {
                        //if (agreementPdf.ServiceList.Count > 0)
                        //{
                        //    foreach (var item in agreementPdf.ServiceList)
                        //    {
                        //        OnetimeServiceItems = OnetimeServiceItems +
                        //        "<tr style='border-bottom:1px solid #ccc;'>" +
                        //        "<td style=\"padding:5px 0px 5px 20px\">" +
                        //        "<label>" + item.Name + "</label>" +
                        //        "</td>" +
                        //        "<td style='padding:5px;text-align:center'><label>" + currentCurrency + FormatAmount(item.Total) + "</label></td>" +
                        //        "</tr>";

                        //        subtotal1 = subtotal + (double)item.Total;

                        //        tax1 = subtotal1 * (agreementPdf.Tax / 100);

                        //    }

                        //}
                        //if (agreementPdf.NotARBEnabledServiceList.Count > 0)
                        //{
                        //    foreach (var item in agreementPdf.NotARBEnabledServiceList)
                        //    {
                        //        OnetimeServiceItems = OnetimeServiceItems +
                        //        "<tr style='border-bottom:1px solid #ccc'>" +
                        //        "<td style=\"padding:5px 0px 5px 20px\">" +
                        //        "<label>" + " + One Time Service Fee - " + item.Name + "</label>" +
                        //        "</td>" +
                        //        "<td style=\"padding:5px;text-align:center\"><label>" + currentCurrency + FormatAmount(item.Total) + "</label></td>" +
                        //        "</tr>";

                        //        subtotal2 = subtotal + (double)item.Total;
                        //        tax2 = subtotal2 * (agreementPdf.Tax / 100);



                        //    }
                        //}
                        //subtotal = subtotal1 + subtotal2;
                        //totaltax = tax1 + tax2;
                        //Finaltotal = Finaltotal + subtotal + totaltax;
                        //OnetimeServiceContent = "<table style=\"width:100%;float:left;border-collapse:collapse;margin-bottom:5px;margin-top:10px;font-size:12px\">" +
                        //   " <thead> " +
                        //   " <tr style = 'background-color:#000000;color:white;font-size:12px;/* width:100%; */border:1px solid #4f90bb' >" +
                        //   " <th style = 'width: 80%;text-align:left;padding:5px 0px 5px 20px;border-right:1px solid #fff' > SERVICE </th>" +
                        //   " <th style = 'width: 14%;padding:5px 0px;border-right:1px solid #fff;text-align:center' > PRICE </th></tr>" +
                        //   " </thead > <tbody>" +
                        //   OnetimeServiceItems +

                        //   /// subtotal
                        //   "<tr style = '/* border-bottom:1px solid #ccc; */' >" +
                        //   "<td style = 'padding: 0px 0px 0px 0px;text-align: right;border: none;' ><strong><label> SubTotal </label></strong></td>" +
                        //   "<td style = 'padding:0px;text-align:center' ><strong><label>" + currentCurrency + FormatAmount(subtotal) + " </label></strong></td>" +
                        //   "</tr> " +

                        //   /// totalTax
                        //   "<tr style = '/* border-bottom:1px solid #ccc; */'>" +
                        //   "<td style = 'padding:0px 0px 0px 0px;text-align: right;' ><strong><label> Tax </label></strong></td> " +
                        //   "<td style = 'padding:0px;text-align:center' ><strong><label>" + currentCurrency + FormatAmount(totaltax) + " </label></strong></td>" +
                        //   "</tr> " +

                        //   // Total

                        //   "<tr style = '/* border-bottom:1px solid #ccc; */' >" +
                        //   "<td style = 'padding:0px 0px 0px 0px;text-align: right; ' ><strong><label> Total </label></strong></td>" +
                        //   "<td style = 'padding:0px;text-align:center' ><strong><label>" + currentCurrency + FormatAmount(Finaltotal) + "</label></strong></td>" +
                        //   "</tr>" +



                        //   " </tbody></table> ";

                        //templateVars.Add("OneTimeServiceFee", OnetimeServiceContent);
                        //templateVars.Remove("CommercialServiceList");

                        onetimeservicefee = agreementPdf.NotARBEnabledTotalPrice;
                        templateVars.Add("OneTimeServiceFee", "<table style='width:100%; float:left; border-collapse:collapse; margin-bottom:30px; margin-top:10px;'><thead><tr style='background-color:#4f90bb;color:white;width:100%;border:1px solid #4f90bb'><th style='width:86%; text-align:left; padding:5px 0px 5px 20px; border-right:1px solid #fff'></th></tr></thead><tbody>"
                               + "<tr style='height:40px;'><td valign='middle' style='font-weight:bold; border: 1px solid #000; text-align:left; padding-left:15px; background-color:#f3f3f3;'>+One Time Service Fee</td><td style='border: 1px solid #000;text-align:center;padding-left:10px;'>"
                               + currentCurrency + FormatAmount(onetimeservicefee) + "</td></tr>"
                               + "</tbody></table>");
                    }
                    //// Mayur :: New onetimeservice + services join showing in aggrement :: End

                    ////  " Mayur " Discount change if ACH payment" :: End
                
                    templateVars.Add("MonthlyServiceFeeTotal", currentCurrency + FormatAmount(serviceFeeTotal));
                    templateVars.Add("MonthlyServiceFeeFinalTotal", currentCurrency + FormatAmount(MonthlyServiceFee));
                    templateVars.Add("UpfrontAddOnTotal", currentCurrency + FormatAmount(upfrontamount));
                    templateVars.Add("ActivationFee", currentCurrency + FormatAmount(activationamount));
                    templateVars.Add("LabourFeeDfw", LabourfeeDfw);
                    templateVars.Add("LabourFeeRmr", LabourFeeRmR);
                    ////  " Mayur " Discount change if ACH payment" :: Start
                    templateVars.Add("BeforeSubTotal", currentCurrency + FormatAmount(SubTotalBeforeDiscount));
                    templateVars.Add("EquipDiscount", currentCurrency + FormatAmount(EqpActualDiscount));
                    ////  " Mayur " Discount change if ACH payment" :: End
                    templateVars.Add("NewSubTotal", currentCurrency + FormatAmount(subtotalamount));
                    templateVars.Add("TotalDueAtSigning", currentCurrency + FormatAmount(duesignamount));

                    if (CustomerPaymentInfo != null && CustomerPaymentInfo.Count > 0)
                    {
                        foreach (var item in CustomerPaymentInfo)
                        {
                            if (item.Payfor.ToLower() == "activation fee" && (item.PaymentType.ToLower() == "ach" || item.PaymentType.ToLower() == "cc"))
                            {
                                collectedamount += activationamount;
                                collectedamount += agreementPdf.NonConfirmingFee;
                            }
                            if (item.Payfor.ToLower() == "service" && (item.PaymentType.ToLower() == "ach" || item.PaymentType.ToLower() == "cc"))
                            {
                                collectedamount += serviceFeeTotal;
                            }
                            if (item.Payfor.ToLower() == "equipment" && (item.PaymentType.ToLower() == "ach" || item.PaymentType.ToLower() == "cc"))
                            {
                                collectedamount += upfrontamount;
                            }
                        }
                        var taxcollectedtotal = collectedamount * (agreementPdf.Tax / 100);
                        collectedamount += taxcollectedtotal;
                    }
                    if (collectedamount > 0)
                    {
                        templateVars.Add("TotalCollectedAmount", "<tr style='height: 25px;'><td valign='middle' style='font-weight:bold; border: 1px solid #000; text-align:right; padding: 15px 5px;'>Collected</td><td style='border:1px solid #000; padding: 15px 5px;text-align: right;padding-right: 10px;'>" + currentCurrency + FormatAmount(collectedamount) + "</td></tr>");
                    }
                }
                else
                {
                    var serviceFeeTotal = agreementPdf.MonthlyServiceFeeTotal;

                    if (agreementPdf.IsUpfrontPromo == true)
                    {
                        upfrontamount = agreementPdf.UpfrontAddOnTotalPromo;
                    }
                    var activationamount = agreementPdf.ActivationFee;


                    //// Mayur :: New onetimeservice + services join showing in aggrement :: Start
                    
                    var onetimeservicefee = 0.0;

                    var OnetimeServiceContent = "";
                    var OnetimeServiceItems = "";
                    var subtotal= 0.0;
                    var subtotal1 = 0.0;
                    var subtotal2 = 0.0;
                    var tax1 = 0.0;
                    var tax2 = 0.0;
                    var totaltax = 0.0;
                    var Finaltotal = 0.0;


                    if (!string.IsNullOrWhiteSpace(agreementPdf.ContractType) && agreementPdf.ContractType.ToLower() == "commercialfire")
                    {
                        if (agreementPdf.ServiceList.Count > 0)
                        {
                            foreach (var item in agreementPdf.ServiceList)
                            {
                                OnetimeServiceItems = OnetimeServiceItems +
                                "<tr style='border-bottom:1px solid #ccc'>"+
                                "<td style=\"padding:5px 0px 5px 20px\">" +
                                "<strong><label>"+ item.Name + "</label></strong>" +
                                "</td>" +
                                "<td style=\"padding:5px;text-align:center\"><strong><label>" + currentCurrency + FormatAmount(item.Total) + "</label></strong></td>" +
                                "</tr>";

                                subtotal1 += subtotal + (double)item.Total;
                              
                                tax1  += subtotal1 * (agreementPdf.Tax / 100);

                            }
                          
                        }
                        if (agreementPdf.NotARBEnabledServiceList.Count > 0)
                        {
                            foreach (var item in agreementPdf.NotARBEnabledServiceList)
                            {
                                OnetimeServiceItems = OnetimeServiceItems +
                                "<tr style='border-bottom:1px solid #ccc'>" +
                                "<td style=\"padding:5px 0px 5px 20px\">" +
                                "<strong><label>" + " + One Time Service Fee - " + item.Name + "</label></strong>" +
                                "</td>" +
                                "<td style=\"padding:5px;text-align:center\"><strong><label>" + currentCurrency + FormatAmount(item.Total) + "</label></strong></td>" +
                                "</tr>";

                                subtotal2 += subtotal + (double)item.Total;
                                tax2 += subtotal2 * (agreementPdf.Tax / 100);



                            }
                        }

                        if (agreementPdf.NonConfirmingFee > 0)
                        {
                            NonConfirmingFeeDivCommFire = "<tr style='border-bottom:1px solid #ccc'><td style=\"padding:5px 0px 5px 20px\"><strong><label> + Non Conforming Fee </label></strong></td><td style=\"padding:5px;text-align:center\"><strong><label>{0}</label></strong></td></tr>";
                            NonConfirmingFeeDivCommFire = string.Format(NonConfirmingFeeDivCommFire, currentCurrency + FormatAmount(agreementPdf.NonConfirmingFee));
                            OnetimeServiceItems += NonConfirmingFeeDivCommFire;
                            subtotal2 += subtotal + agreementPdf.NonConfirmingFee;
                            tax2 = subtotal2 * (agreementPdf.Tax / 100);
                        }

                        subtotal = subtotal1 + subtotal2;
                        totaltax = tax1 + tax2;
                        Finaltotal = Finaltotal + subtotal + totaltax;    
                        OnetimeServiceContent = "<table style=\"width:100%;float:left;border-collapse:collapse;margin-bottom:30px;margin-top:10px;\">" +
                          
                           " <tr style = 'background-color:#4f90bb;color:white;/* width:100%; */border:1px solid #4f90bb' >" +
                           " <th style = 'width: 80%;text-align:left;padding:5px 0px 5px 20px;border-right:1px solid #fff' > SERVICE </th>" +
                           " <th style = 'width: 14%;padding:5px 0px;border-right:1px solid #fff;text-align:center' > PRICE </th></tr>" +
                           " <tbody>" +
                           OnetimeServiceItems +

                           /// subtotal
                           "<tr style = '/* border-bottom:1px solid #ccc; */' >" +
                           "<td style = 'border: 0px 0px 0px 5px;text-align: right;border: none;' ><strong><label> SubTotal </label></strong></td>" +
                           "<td style = 'padding:5px;text-align:center' ><strong><label>"+ currentCurrency + FormatAmount(subtotal) +" </label></strong></td>" +
                           "</tr> " +

                           /// totalTax
                           "<tr style = '/* border-bottom:1px solid #ccc; */'>"+
                           "<td style = 'padding:0px 0px 0px 10px;text-align: right;' ><strong><label> Tax </label></strong></td> "+
                           "<td style = 'padding:0px;text-align:center' ><strong><label>"+ currentCurrency + FormatAmount(totaltax)+" </label></strong></td>"+
                           "</tr> "+

                            // Total

                           "<tr style = '/* border-bottom:1px solid #ccc; */' >" +
                           "<td style = 'padding:0px 0px 0px 5px;text-align: right; ' ><strong><label> Total </label></strong></td>" +
                           "<td style = 'padding:5px;text-align:center' ><strong><label>"+ currentCurrency + FormatAmount(Finaltotal) +"</label></strong></td>" +
                           "</tr>" +



                           " </tbody></table> ";

                        templateVars.Add("OneTimeServiceFee", OnetimeServiceContent);
                        templateVars.Remove("CommercialServiceList");
                    }
                    else
                    {
                        //if (agreementPdf.ServiceList.Count > 0)
                        //{
                        //    foreach (var item in agreementPdf.ServiceList)
                        //    {
                        //        OnetimeServiceItems = OnetimeServiceItems +
                        //        "<tr style='border-bottom:1px solid #ccc;'>" +
                        //        "<td style=\"padding:5px 0px 5px 20px\">" +
                        //        "<label>" + item.Name + "</label>" +
                        //        "</td>" +
                        //        "<td style='padding:5px;text-align:center'><label>" + currentCurrency + FormatAmount(item.Total) + "</label></td>" +
                        //        "</tr>";

                        //        subtotal1 = subtotal + (double)item.Total;

                        //        tax1 = subtotal1 * (agreementPdf.Tax / 100);

                        //    }

                        //}
                        //if (agreementPdf.NotARBEnabledServiceList.Count > 0)
                        //{
                        //    foreach (var item in agreementPdf.NotARBEnabledServiceList)
                        //    {
                        //        OnetimeServiceItems = OnetimeServiceItems +
                        //        "<tr style='border-bottom:1px solid #ccc'>" +
                        //        "<td style=\"padding:5px 0px 5px 20px\">" +
                        //        "<label>" + " + One Time Service Fee - " + item.Name + "</label>" +
                        //        "</td>" +
                        //        "<td style=\"padding:5px;text-align:center\"><label>" + currentCurrency + FormatAmount(item.Total) + "</label></td>" +
                        //        "</tr>";

                        //        subtotal2 = subtotal + (double)item.Total;
                        //        tax2 = subtotal2 * (agreementPdf.Tax / 100);



                        //    }
                        //}
                        //subtotal = subtotal1 + subtotal2;
                        //totaltax = tax1 + tax2;
                        //Finaltotal = Finaltotal + subtotal + totaltax;
                        //OnetimeServiceContent = "<table style=\"width:100%;float:left;border-collapse:collapse;margin-bottom:5px;margin-top:10px;font-size:12px\">" +
                        //   " <thead> " +
                        //   " <tr style = 'background-color:#000000;color:white;font-size:12px;/* width:100%; */border:1px solid #4f90bb' >" +
                        //   " <th style = 'width: 80%;text-align:left;padding:5px 0px 5px 20px;border-right:1px solid #fff' > SERVICE </th>" +
                        //   " <th style = 'width: 14%;padding:5px 0px;border-right:1px solid #fff;text-align:center' > PRICE </th></tr>" +
                        //   " </thead > <tbody>" +
                        //   OnetimeServiceItems +

                        //   /// subtotal
                        //   "<tr style = '/* border-bottom:1px solid #ccc; */' >" +
                        //   "<td style = 'padding: 0px 0px 0px 0px;text-align: right;border: none;' ><strong><label> SubTotal </label></strong></td>" +
                        //   "<td style = 'padding:0px;text-align:center' ><strong><label>" + currentCurrency + FormatAmount(subtotal) + " </label></strong></td>" +
                        //   "</tr> " +

                        //   /// totalTax
                        //   "<tr style = '/* border-bottom:1px solid #ccc; */'>" +
                        //   "<td style = 'padding:0px 0px 0px 0px;text-align: right;' ><strong><label> Tax </label></strong></td> " +
                        //   "<td style = 'padding:0px;text-align:center' ><strong><label>" + currentCurrency + FormatAmount(totaltax) + " </label></strong></td>" +
                        //   "</tr> " +

                        //   // Total

                        //   "<tr style = '/* border-bottom:1px solid #ccc; */' >" +
                        //   "<td style = 'padding:0px 0px 0px 0px;text-align: right; ' ><strong><label> Total </label></strong></td>" +
                        //   "<td style = 'padding:0px;text-align:center' ><strong><label>" + currentCurrency + FormatAmount(Finaltotal) + "</label></strong></td>" +
                        //   "</tr>" +



                        //   " </tbody></table> ";

                        //templateVars.Add("OneTimeServiceFee", OnetimeServiceContent);
                        //templateVars.Remove("CommercialServiceList");

                        onetimeservicefee = agreementPdf.NotARBEnabledTotalPrice;
                        templateVars.Add("OneTimeServiceFee", "<table style='width:100%; float:left; border-collapse:collapse; margin-bottom:30px; margin-top:10px;'><thead><tr style='background-color:#4f90bb;color:white;width:100%;border:1px solid #4f90bb'><th style='width:86%; text-align:left; padding:5px 0px 5px 20px; border-right:1px solid #fff'></th></tr></thead><tbody>"
                               + "<tr style='height:40px;'><td valign='middle' style='font-weight:bold; border: 1px solid #000; text-align:left; padding-left:15px; background-color:#f3f3f3;'>+One Time Service Fee</td><td style='border: 1px solid #000;text-align:center;padding-left:10px;'>"
                               + currentCurrency + FormatAmount(onetimeservicefee) + "</td></tr>"
                               + "</tbody></table>");
                    }
                    //// Mayur :: New onetimeservice + services join showing in aggrement :: End

                    ////////////////////////// old Onetimeservice templete
                    ///
                    // var onetimeservicefee = 0.0;
                    //if (agreementPdf.NotARBEnabledTotalPrice > 0)
                    //{
                    //    onetimeservicefee = agreementPdf.NotARBEnabledTotalPrice;
                    //    //templateVars.Add("OneTimeServiceFee", "< table style = 'width:100%'; float:left; border-collapse:collapse; margin-bottom:30px; margin-top:10px;' >< thead >< tr style = 'background-color:#4f90bb;color:white;width:100%;border:1px solid #4f90bb' >< th style = 'width:86%; text-align:left; padding:5px 0px 5px 20px; border-right:1px solid #fff' ></ th ></ tr ></ thead >< tbody > "
                    //    //     + "< tr style = 'height: 25px;' >< td valign = 'middle' style = 'font-weight:bold; border: 1px solid #000; text-align:right; padding-right:5px; background-color:#f3f3f3;' > +One Time Service Fee </ td >< td style = 'border: 1px solid #000;text-align: right;padding-right: 10px;' > " + currentCurrency + FormatAmount(onetimeservicefee) + "</ td ></ tr >
                    //    //     + "</ tbody ></ table > ");
                    //    templateVars.Add("OneTimeServiceFee", "<table style='width:100%; float:left; border-collapse:collapse; margin-bottom:30px; margin-top:10px;'><thead><tr style='background-color:#4f90bb;color:white;width:100%;border:1px solid #4f90bb'><th style='width:86%; text-align:left; padding:5px 0px 5px 20px; border-right:1px solid #fff'></th></tr></thead><tbody>"
                    //           + "<tr style='height:40px;'><td valign='middle' style='font-weight:bold; border: 1px solid #000; text-align:left; padding-left:15px; background-color:#f3f3f3;'>+One Time Service Fee</td><td style='border: 1px solid #000;text-align:center;padding-left:10px;'>"
                    //           + currentCurrency + FormatAmount(onetimeservicefee) + "</td></tr>"
                    //           + "</tbody></table>");

                    //}

                    /////////////////////

                    if (agreementPdf.IsServicePromo != true)
                    {
                        MonthlyServiceFee = serviceFeeTotal;
                    }
                    agreementPdf.NonConfirmingFee = agreementPdf.NonConfirmingFee;
                    
                    foreach(Equipment d in agreementPdf.EquipmentList )
                    {
                        if(d.DiscountInCurrency>0.0)
                        {
                            EquipIsPcnt = false;
                            EquipDiscount = d.DiscountInCurrency;
                        }
                        else
                        {
                            EquipIsPcnt = true;
                            EquipDiscount = d.DiscountPercentage;
                        }
                        break;
                    }
                    if(EquipIsPcnt)
                    {
                        EqpDiscAmount = ((upfrontamount * EquipDiscount) / 100);
                    }
                    else
                    {
                        EqpDiscAmount = EquipDiscount;
                    }
                    EqpActualDiscount = EqpDiscAmount;
                    var subtotalamount = MonthlyServiceFee + AdvanceMonitoringFee + upfrontamount + activationamount + agreementPdf.NonConfirmingFee + onetimeservicefee;
                    SubTotalBeforeDiscount = subtotalamount;
                    subtotalamount = subtotalamount - EqpActualDiscount;
                    var taxRate = agreementPdf.Tax / 100.0;
                    if(!string.IsNullOrWhiteSpace(agreementPdf.EstimatorId))
                    {
                        var totaltaxval = subtotalamount * (agreementPdf.Tax / 100);
                        agreementPdf.TaxTotal = totaltaxval;//agreementPdf.TaxTotal;
                    }
                    else
                    {
                        var indivisualSubtotaltex =
                         Math.Round(MonthlyServiceFee * taxRate, 2) +
                         Math.Round(AdvanceMonitoringFee * taxRate, 2) +
                         Math.Round(upfrontamount * taxRate, 2) +
                         Math.Round(activationamount * taxRate, 2) +
                         Math.Round(agreementPdf.NonConfirmingFee * taxRate, 2) +
                         Math.Round(onetimeservicefee * taxRate, 2);
                        double TaxableSubtotal = 0.0;
                        if (glLabourFee != null && glLabourFee.Value == "true")
                        {
                            indivisualSubtotaltex = indivisualSubtotaltex + agreementPdf.LabourFee;
                            TaxableSubtotal = indivisualSubtotaltex - agreementPdf.LabourFee;
                        }
                        else
                        {
                            TaxableSubtotal = indivisualSubtotaltex;
                        }
                        agreementPdf.TaxTotal = TaxableSubtotal;
                    }
                    
                    
                    var duesignamount = subtotalamount + agreementPdf.TaxTotal;
                
                    templateVars.Add("MonthlyServiceFeeTotal", currentCurrency + FormatAmount(serviceFeeTotal));
                    templateVars.Add("MonthlyServiceFeeFinalTotal", currentCurrency + FormatAmount(MonthlyServiceFee));
                    templateVars.Add("UpfrontAddOnTotal", currentCurrency + FormatAmount(upfrontamount));
                    templateVars.Add("ActivationFee", currentCurrency + FormatAmount(activationamount));
                    templateVars.Add("LabourFeeDfw", LabourfeeDfw);
                    templateVars.Add("LabourFeeRmr", LabourFeeRmR);
                    templateVars.Add("BeforeSubTotal", currentCurrency + FormatAmount(SubTotalBeforeDiscount));
                    templateVars.Add("EquipDiscount", currentCurrency + FormatAmount(EqpActualDiscount));
                    templateVars.Add("NewSubTotal", currentCurrency + FormatAmount(subtotalamount));
                    templateVars.Add("TotalDueAtSigning", currentCurrency + FormatAmount(duesignamount));
                    if (CustomerPaymentInfo != null && CustomerPaymentInfo.Count > 0)
                    {
                        foreach (var item in CustomerPaymentInfo)
                        {
                            if (item.Payfor.ToLower() == "activation fee" && (item.PaymentType.ToLower() == "ach" || item.PaymentType.ToLower() == "cc"))
                            {
                                collectedamount += agreementPdf.ActivationFee;
                                collectedamount += agreementPdf.NonConfirmingFee;
                            }
                            if (item.Payfor.ToLower() == "service" && (item.PaymentType.ToLower() == "ach" || item.PaymentType.ToLower() == "cc"))
                            {
                                collectedamount += agreementPdf.MonthlyServiceFeeTotal;
                            }
                            if (item.Payfor.ToLower() == "equipment" && (item.PaymentType.ToLower() == "ach" || item.PaymentType.ToLower() == "cc"))
                            {
                                collectedamount += agreementPdf.UpfrontAddOnTotal;
                            }
                        }
                        var taxcollectedtotal = collectedamount * (agreementPdf.Tax / 100);
                        collectedamount += taxcollectedtotal;
                    }
                    if (collectedamount > 0)
                    {
                        templateVars.Add("TotalCollectedAmount", "<tr style='height: 25px;'><td valign='middle' style='font-weight:bold; border: 1px solid #000; text-align:right; padding: 15px 5px;'>Collected</td><td style='border:1px solid #000; padding: 15px 5px;text-align: right;padding-right: 10px;'>" + currentCurrency + FormatAmount(collectedamount) + "</td></tr>");
                    }
                }


                templateVars.Add("TotalMonthlyMonitoring", currentCurrency + FormatAmount(agreementPdf.TotalMonthlyMonitoring));

                templateVars.Add("UpfrontAddOnTotals", currentCurrency + FormatAmount(agreementPdf.UpfrontAddOnTotal));
                if (agreementPdf.IsUpfrontPromo == true)
                {
                    string upfrontpromo = string.Format(@"  <tr style='background-color:#f2f2f2; color:#000; height: 30px;'>
                        <td style='text-align:right;font-weight:bold; background-color:#fff;'></td>

                        <td colspan='4' style='text-align:right; border:2px solid #000; border-right:1px solid #000; padding:0px 10px; font-size:15px; font-weight:bold;'>
                            UPFRONT ADD-ON TOTAL(Promo Discount)
                        </td>
                        <td style='text-align:center; border:2px solid #000; border-left:1px solid #000;'>
                            {0}
                        </td>
                    </tr>", currentCurrency + FormatAmount(agreementPdf.UpfrontAddOnTotalPromo));
                    templateVars.Add("UpfrontAddOnTotalsPromo", upfrontpromo);
                }
            
                templateVars.Add("ResidentialTechFirstHourCost", currentCurrency + agreementPdf.ResidentialTechFirstHourCost);
                templateVars.Add("CommercialTechFirstHourCost", currentCurrency + agreementPdf.CommercialTechFirstHourCost);

                templateVars.Add("KazarLogo", agreementPdf.KazarLogo);
                templateVars.Add("CompanyName", agreementPdf.CompanyName);
                templateVars.Add("CompanyStreet", agreementPdf.CompanyStreet);
                templateVars.Add("CompanySate", agreementPdf.CompanySate);
                templateVars.Add("CompanyWebsite", agreementPdf.CompanyWebsite);
                templateVars.Add("CompanyPhone", agreementPdf.CompanyPhone);
                templateVars.Add("CustomerName", agreementPdf.OwnerName);
                templateVars.Add("ContractCreatedDateVal", ContractCreatedDateVal);
                templateVars.Add("BusinessName", agreementPdf.BusinessName);
                templateVars.Add("BusinessNameWithDBA", agreementPdf.BusinessName + (!string.IsNullOrWhiteSpace(agreementPdf.DoingBusinessAs) ? (" (DBA: " + agreementPdf.DoingBusinessAs + ")") : ""));
                templateVars.Add("EffectiveDate", agreementPdf.EffectiveDate);
                templateVars.Add("OwnerAddress", agreementPdf.OwnerAddress);
                templateVars.Add("OwnerEmail", agreementPdf.OwnerEmail);
                templateVars.Add("OwnerPhone", Extentions.PhoneNumberFormatNew(agreementPdf.OwnerPhone));
                templateVars.Add("CustomerSignature", agreementPdf.CustomerSignature);
                templateVars.Add("ContractCreatedDate", agreementPdf.ContractCreatedDateVal);
                //templateVars.Add("CustomerSignatureDate", agreementPdf.CustomerSignatureDate != null ? agreementPdf.CustomerSignatureDate.Value.ToShortDateString() : "");
                //templateVars.Add("CustomerSignatureDate", agreementPdf.CustomerSignatureStringDate);
                templateVars.Add("CustomerSignatureDate", agreementPdf.CustomerSignatureStringDateVal != null && agreementPdf.CustomerSignatureStringDateVal != new DateTime() ? agreementPdf.CustomerSignatureStringDateVal.ToString("M/dd/yy") : "");
                templateVars.Add("CustomerSignatureDateDay", !string.IsNullOrWhiteSpace(agreementPdf.CustomerSignatureStringDate) ? (Convert.ToDateTime(agreementPdf.CustomerSignatureStringDateVal)).DateFormat("day") : "_");
                templateVars.Add("CustomerSignatureDateMonth", !string.IsNullOrWhiteSpace(agreementPdf.CustomerSignatureStringDate) ? (Convert.ToDateTime(agreementPdf.CustomerSignatureStringDateVal)).DateFormat("monthName") : "_");
                templateVars.Add("CustomerSignatureDateYear", !string.IsNullOrWhiteSpace(agreementPdf.CustomerSignatureStringDate) ? (Convert.ToDateTime(agreementPdf.CustomerSignatureStringDateVal)).DateFormat("year") : "_");
                if (!string.IsNullOrWhiteSpace(agreementPdf.CustomerSignature))
                {
                    templateVars.Add("CompanySignature", agreementPdf.CompanySignature);
                    templateVars.Add("CompanySignatureDate", agreementPdf.CompanySignatureDate);
                }

                if (agreementPdf.ACHDiscountAmount > 0)
                {
                    var serviceFeeTotal = agreementPdf.MonthlyServiceFeeTotal - agreementPdf.ACHDiscountAmount;

                    templateVars.Add("MonthlyMonitoringFee", currentCurrency + FormatAmount(serviceFeeTotal));
                }
                else
                {
                    templateVars.Add("MonthlyMonitoringFee", currentCurrency + FormatAmount(agreementPdf.MonthlyServiceFeeTotal));
                }
                templateVars.Add("Tax", currentCurrency + FormatAmount(agreementPdf.TaxTotal));
                templateVars.Add("Total", currentCurrency + FormatAmount(agreementPdf.Total));
                templateVars.Add("CompanyLogo", agreementPdf.CompanyLogo);
                templateVars.Add("DateOfTransaction", agreementPdf.DateOfTransaction.ToString("MMMM dd yyyy"));

                if (templateid.HasValue && templateid.Value > 0)
                {
                    templateVars.Add("Day", DateTime.Now.DateFormat("day"));
                    templateVars.Add("Month", DateTime.Now.DateFormat("monthName"));
                    templateVars.Add("Year", DateTime.Now.DateFormat("year"));
                }
                else
                {
                    templateVars.Add("Date", agreementPdf.DateOfTransaction.ToString("dd"));
                    templateVars.Add("Month", agreementPdf.DateOfTransaction.ToString("MMMM"));
                    templateVars.Add("Year", agreementPdf.DateOfTransaction.ToString("yyyy"));
                }
                double SubscribedMonths = 0;
                double MonthlyMonitoringFee = 0;

                double.TryParse(agreementPdf.SubscribedMonths, out SubscribedMonths);
                double.TryParse(agreementPdf.MonthlyMonitoringFee, out MonthlyMonitoringFee);


                templateVars.Add("SubscribedMonths", string.Format("{0} {1}", agreementPdf.SubscribedMonthsInWord, SubscribedMonthsText));
                templateVars.Add("SubscribedMonthsUperCase", string.Format("{0} {1}", agreementPdf.SubscribedMonthsInWord.ToUpper(), SubscribedMonthsText.ToUpper()));
                templateVars.Add("RenewalMonths", string.Format("{0} {1}", agreementPdf.RenewalMonths, "month"));
                templateVars.Add("RenewalTerm", string.Format("{0}", agreementPdf.RenewalMonths));
                templateVars.Add("TotalPayments", currentCurrency + SubscribedMonths * MonthlyMonitoringFee);
                templateVars.Add("Subtotal", currentCurrency + FormatAmount(agreementPdf.Subtotal));
                templateVars.Add("RevisionDate", "<span style='float:right; margin-top:25px;'>Rev. " + DateTime.Now.ToString("MMM") + DateTime.Now.ToString("yyyy") + "</span>");
                if (agreementPdf.ACHDiscountAmount > 0)
                {
                    templateVars.Add("ServiceSubtotal", "<tr style='background-color:#bfbfbf; color:#000; height: 30px;'><td style='text-align:center; border: 2px solid #000; border-right:1px solid #000; padding:0px 10px; font-size:15px; font-weight:bold;' colspan = 4>SubTotal</td><td style='text-align:center; border: 2px solid #000; border-left:1px solid #000;'>" + currentCurrency + FormatAmount(agreementPdf.TotalMonthlyMonitoring) + "</td></tr>");
                }
                if (!string.IsNullOrWhiteSpace(agreementPdf.BusinessName) && !string.IsNullOrWhiteSpace(agreementPdf.DoingBusinessAs))
                {
                    if (templateid.HasValue && templateid.Value > 0)
                    {
                        templateVars.Add("OwnerName", agreementPdf.DisplayName);
                    }
                    else
                    {
                        templateVars.Add("OwnerName", "<td valign = 'top' style = 'padding-left:10px;' colspan = '3'>Customer Name<div>" + agreementPdf.OwnerName + "</div></td>");
                    }
                    templateVars.Add("OwnerBusinessName", "<td valign = 'top' style = 'padding-left:10px;' colspan = '2'>Business Name<div>" + agreementPdf.BusinessName + "</div></td>");
                    templateVars.Add("OwnerDBA", "<td valign = 'top' style = 'padding-left:10px;' colspan = '2'>Doing Business As<div>" + agreementPdf.DoingBusinessAs + "</div></td>");
                    templateVars.Add("CancellationOwnerName", "<td valign = 'top' colspan = '3'><div>" + agreementPdf.DispalyName + "</div></td>");
                    templateVars.Add("OwnerDisplayName", "<td valign = 'top' style = 'padding-left:10px;' colspan = '3'>Customer Name<div>" + agreementPdf.DispalyName + "</div></td>");
                }
                else if (!string.IsNullOrWhiteSpace(agreementPdf.BusinessName))
                {
                    if (templateid.HasValue && templateid.Value > 0)
                    {
                        templateVars.Add("OwnerName", agreementPdf.DisplayName);
                    }
                    else
                    {
                        templateVars.Add("OwnerName", "<td valign = 'top' style = 'padding-left:10px;' colspan = '5'>Customer Name<div>" + agreementPdf.OwnerName + "</div></td>");
                    }
                    templateVars.Add("OwnerDisplayName", "<td valign = 'top' style = 'padding-left:10px;' colspan = '3'>Customer Name<div>" + agreementPdf.DispalyName + "</div></td>");
                    templateVars.Add("OwnerBusinessName", "<td valign = 'top' style = 'padding-left:10px;' colspan = '2'>Business Name<div>" + agreementPdf.BusinessName + "</div></td>");
                    templateVars.Add("CancellationOwnerName", "<td valign = 'top' colspan = '3'><div>" + agreementPdf.DispalyName + "</div></td>");
                }
                else if (!string.IsNullOrWhiteSpace(agreementPdf.DoingBusinessAs))
                {
                    if (templateid.HasValue && templateid.Value > 0)
                    {
                        templateVars.Add("OwnerName", agreementPdf.DisplayName);
                    }
                    else
                    {
                        templateVars.Add("OwnerName", "<td valign = 'top' style = 'padding-left:10px;' colspan = '5'>Customer Name<div>" + agreementPdf.OwnerName + "</div></td>");
                    }
                    templateVars.Add("OwnerDisplayName", "<td valign = 'top' style = 'padding-left:10px;' colspan = '3'>Customer Name<div>" + agreementPdf.DispalyName + "</div></td>");
                    templateVars.Add("OwnerDBA", "<td valign = 'top' style = 'padding-left:10px;' colspan = '2'>Doing Business As<div>" + agreementPdf.DoingBusinessAs + "</div></td>");
                    templateVars.Add("CancellationOwnerName", "<td valign = 'top' colspan = '3'><div>" + agreementPdf.DispalyName + "</div></td>");
                }
                else
                {
                    if (templateid.HasValue && templateid.Value > 0)
                    {
                        templateVars.Add("OwnerName", agreementPdf.OwnerName);
                    }
                    else
                    {
                        templateVars.Add("OwnerName", "<td valign = 'top' style = 'padding-left:10px;' colspan = '7'>Customer Name<div>" + agreementPdf.OwnerName + "</div></td>");
                    }
                    templateVars.Add("OwnerDisplayName", "<td valign = 'top' style = 'padding-left:10px;' colspan = '3'>Customer Name<div>" + agreementPdf.DispalyName + "</div></td>");
                    templateVars.Add("CancellationOwnerName", "<td valign = 'top' colspan = '3'><div>" + agreementPdf.DispalyName + "</div></td>");
                }


                if (!string.IsNullOrWhiteSpace(agreementPdf.SalesRepresentative) && agreementPdf.SalesRepresentative != "-1")
                {
                    templateVars.Add("SalesRepresentative", agreementPdf.SalesRepresentative);
                }
                else
                {
                    templateVars.Add("SalesRepresentative", "");
                }
                var EmergencyContactHeader = "";
                if (agreementPdf.EmergencyContactList != null && agreementPdf.EmergencyContactList.Count > 0)
                {
                    ContactNameHeader = "<div style='width: 24%; float:left;text-align:center;border:1px solid #ccc;font-weight: bold;'>Name</div>";
                    ContactPhoneHeader = " <div style='width: 24%; float:left; text-align:center;border:1px solid #ccc;font-weight: bold;'>Phone No</div>";
                    PhoneTypeHeader = "<div style='width: 24%; float:left; text-align:center;border:1px solid #ccc;font-weight: bold;'>Phone Type</div>";
                    ContactRelationshipHeader = "<div style='width: 24%; float:left; text-align:center;border:1px solid #ccc;font-weight: bold;'>Relationship</div>";
                    EmergencyContactHeader += ContactNameHeader + ContactPhoneHeader + PhoneTypeHeader + PhoneType + ContactRelationshipHeader;
                    EstimateEmergencyContactList = "<table style='width:100%;float:left;border-collapse:collapse;'>" +
                        "<thead>" +
                        "<tr style='background-color:#2ca01c;width:100%;border:1px solid #ccc; color:#fff;'>" +
                        "<th style='width:70%;text-align:left;padding:5px 0px 5px 20px;border-right:1px solid #ccc'>" +
                        "NAME</th>" +
                        "<th style='width:15%;padding:5px 0px;border-right:1px solid #ccc;text-align:center; text-transform:uppercase;'>" +
                        "Relationship</th>" +
                        //"<th style='width:14%;padding:5px 0px;border-right:1px solid #ccc;text-align:center; text-transform:uppercase;'>" +
                        //"Has Key</th>" +
                        "<th style='width:15%;padding:5px 0px;border-right:1px solid #ccc;text-align:center; text-transform:uppercase;'>Phone Number" +
                        "</th></tr></thead><tbody>";
                }
                EmergencyContactList += EmergencyContactHeader;
                foreach (var item in agreementPdf.EmergencyContactList)
                {
                    ContactName = "<div style='width: 24%; float:left; text-align:center;border:1px solid #ccc;'>" + item.FirstName + " " + item.LastName + "</div>";
                    ContactPhone = " <div style='width: 24%; float:left; text-align:center;border:1px solid #ccc;'>" + Extentions.PhoneNumberFormatNew(item.Phone) + "</div>";
                    if (item.PhoneType != "-1" && !string.IsNullOrEmpty(item.PhoneType))
                    {
                        PhoneType = "<div style='width: 24%; float:left; text-align:center;border:1px solid #ccc;'>" + item.PhoneType + "</div>";
                    }
                    else
                    {
                        PhoneType = "<div style='width: 24%; float:left; text-align:center;border:1px solid #ccc;'>-</div>";
                    }
                    if (item.RelationShip != "-1" && !string.IsNullOrEmpty(item.RelationShip))
                    {
                        ContactRelationship = "<div style='width: 24%; float:left; text-align:center;border:1px solid #ccc;'>" + item.RelationShip + "</div>";
                    }
                    else
                    {
                        ContactRelationship = "<div style='width: 24%; float:left; text-align:center;border:1px solid #ccc;'>-</div>";
                    }
                    EmergencyContactList += ContactName + ContactPhone + CenterSpace + PhoneType + ContactRelationship + HasKey;
                    EstimateEmergencyContactList += "<tr style='border:1px solid #ccc'>" +
                        "<td style='padding:5px 0px 5px 20px;border:1px solid #ccc'>" +
                        item.FirstName + " " + item.LastName +
                        "</td>" +
                        "<td style='padding:5px;text-align:center; border:1px solid #ccc'>";
                    if (item.RelationShip != "-1" && !string.IsNullOrEmpty(item.RelationShip))
                    {
                        EstimateEmergencyContactList += item.RelationShip;
                    }
                    else
                    {
                        EstimateEmergencyContactList += "-";
                    }
                    EstimateEmergencyContactList += "</td>" +
                    //"<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                    //HasKey +
                    //"</td>" +
                    "<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                    Extentions.PhoneNumberFormatNew(item.Phone) +
                    "</td></tr>";
                }
                if (agreementPdf.EmergencyContactList != null && agreementPdf.EmergencyContactList.Count > 0)
                {
                    EstimateEmergencyContactList += "</tbody></table>";
                }
                if (!string.IsNullOrWhiteSpace(agreementPdf.ContractType) && agreementPdf.ContractType.ToLower() == "commercialfire")
                {
                    CommercialEquipmentList += "<table style='width:100%; float:left; border-collapse:collapse; margin-bottom:30px; margin-top:10px;'>" +
                        "<thead>" +
                        "<tr style='background-color:#4f90bb;color:white;width:100%;border:1px solid #4f90bb'>" +
                        "<th style='width:67%;text-align:left;padding:5px 0px 5px 20px;border-right:1px solid #fff'>EQUIPMENT</th>" +
                        "<th style='width:5%;padding:5px 0px;border-right:1px solid #fff;text-align:center'>QTY</th>" +
                        "<th style='width:14%;padding:5px 0px;border-right:1px solid #fff;text-align:center'>UNIT PRICE</th>" +
                        "<th style='width:14%;padding:5px 0px;border-right:1px solid #fff;text-align:center'>TOTAL PRICE</th>" +
                        "</tr></thead><tbody>";
                }
                GlobalSetting glHidingUnitPrice = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", agreementPdf.CompanyId, "HideUnitPriceOnAgreement")).FirstOrDefault();
                foreach (var item in agreementPdf.EquipmentList)
                {
                    EquipmentName = "  <div style = 'width:39%;float:left;padding-left:10px;'><span> &nbsp;" + item.Name + " </span></div> ";
                    EquipmentQuantity = " <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + item.Quantity + " </span></div>";


                    DiscountUnitPrice = " <div style = 'width:15%;float:left;text-align:center'><span>" + currentCurrency + FormatAmount(item.DiscountUnitPricce) + " </span></div>";
                    TotalEquipment = "  <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + currentCurrency + FormatAmount(item.Total) + " </span></div>";
                    EquipmentList += " <div style='padding-top:5px;'>" + EquipmentName + EquipmentQuantity + UnitPrice + DiscountUnitPrice + TotalEquipment + "</div>";

                    if (item.IsEqpExist)
                    {
                        if(!string.IsNullOrWhiteSpace(agreementPdf.EstimatorId))
                        {
                            SKUDFW = "<td style='text-align:center; font-weight:bold; border: 1px solid #000; padding:0px 10px;'>" + item.SKU + " (Existing Equipment)</td>";
                            ProductDFW = "<td style='text-align:center; font-weight:bold; border: 1px solid #000; padding:0px 10px;'>" + item.Name + " (Existing Equipment)</td>";
                        }
                        else
                        {
                            ProductDFW = "<td style='text-align:center; font-weight:bold; border: 1px solid #000; padding:0px 10px;'>" + item.Name + " (Existing Equipment)</td>";
                        }
                        
                        
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(agreementPdf.EstimatorId))
                        {
                            SKUDFW = "<td style='text-align:center; font-weight:bold; border: 1px solid #000; padding:0px 10px;'>" + item.SKU + "</td>";
                            ProductDFW = "<td style='text-align:center; font-weight:bold; border: 1px solid #000; padding:0px 10px;'>" + item.Name + "</td>";
                        }
                        else
                        {
                            ProductDFW = "<td style='text-align:center; font-weight:bold; border: 1px solid #000; padding:0px 10px;'>" + item.Name + "</td>";
                        } 
                    }

                    QuantityDFW = "<td style='text-align:center; border:1px solid #000;'>" + item.Quantity + "</td>";

                    DiscountUnitPriceDFW = "<td style='text-align:center; border:1px solid #000;'>" + currentCurrency + FormatAmount(item.UnitPrice - item.DiscountUnitPricce) + "</td>";

                    TotalPriceDFW = "<td style='text-align:center; border:1px solid #000;'>" + currentCurrency + FormatAmount(item.Total) + "</td>";

                    UnitPriceDFW = "<td style='text-align:center; font-weight:bold; border:1px solid #000;'>" + currentCurrency + FormatAmount(item.UnitPrice) + "</td>";
                    TotalDiscountDFW = "<td style='text-align:center; border:1px solid #000;'>" + currentCurrency + FormatAmount(item.TotalDiscountUnitPrice) + "</td>";
                    if (glHidingUnitPrice != null && glHidingUnitPrice.Value == "true")
                    {
                        if (!string.IsNullOrWhiteSpace(agreementPdf.EstimatorId))
                        {
                            EquipmentListDFW += "<tr style='background - color:#f2f2f2; color:#000; height: 30px; border:2px solid #000;'>" + ProductDFW + SKUDFW + QuantityDFW + DiscountUnitPriceDFW + TotalPriceDFW + "</tr>";
                        }
                        else
                        {
                            EquipmentListDFW += "<tr style='background - color:#f2f2f2; color:#000; height: 30px; border:2px solid #000;'>" + ProductDFW + QuantityDFW + DiscountUnitPriceDFW + TotalPriceDFW + "</tr>";
                        }
                        
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(agreementPdf.EstimatorId))
                        {
                            EquipmentListDFW += "<tr style='background - color:#f2f2f2; color:#000; height: 30px; border:2px solid #000;'>" + ProductDFW + SKUDFW + QuantityDFW + UnitPriceDFW + DiscountUnitPriceDFW + TotalDiscountDFW + TotalPriceDFW + "</tr>";
                        }
                        else
                        {
                            EquipmentListDFW += "<tr style='background - color:#f2f2f2; color:#000; height: 30px; border:2px solid #000;'>" + ProductDFW + QuantityDFW + UnitPriceDFW + DiscountUnitPriceDFW + TotalDiscountDFW + TotalPriceDFW + "</tr>";
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(agreementPdf.ContractType) && agreementPdf.ContractType.ToLower() == "commercialfire")
                    {
                        CommercialEquipmentList += "<tr style='border-bottom:1px solid #ccc'>" +
                            "<td style='padding:5px 0px 5px 20px'>" +
                            "<strong>" +
                            "<label>" +
                            item.Name +
                            "</label>" +
                            "</strong></td>" +
                            "<td style='padding:5px;text-align:center'>" +
                            "<strong>" +
                            "<label>" +
                            item.Quantity +
                            "</label>" +
                            "</strong ></td>" +
                            "<td style='padding:5px;text-align:center'>" +
                            "<strong>" +
                            "<label>" +
                            currentCurrency + FormatAmount(item.UnitPrice) +
                            "</label>" +
                            "</strong></td>" +
                            "<td style='padding:5px;text-align:center'>" +
                            "<strong>" +
                            "<label>" +
                            currentCurrency + FormatAmount(item.Total) +
                            "</label></strong></td></tr> ";
                    }
                }
                if (!string.IsNullOrWhiteSpace(agreementPdf.ContractType) && agreementPdf.ContractType.ToLower() == "commercialfire")
                {
                    
                    CommercialEquipmentList += "<tr style='font-weight:bold'>" +
                        "<td colspan='3' style='padding-top:5px;text-align:right;padding-right:10px'>Subtotal</td>" +
                        "<td style='padding-top:5px; text-align:center; padding-left:10px;'>" +
                        currentCurrency + FormatAmount(agreementPdf.UpfrontAddOnTotal) +
                        "</td></tr>"
                        +"<tr style = 'font-weight:bold' > " +
                        "<td colspan='3' style='padding-top:5px;text-align:right;padding-right:10px'>Discount</td>" +
                        "<td style='padding-top:5px; text-align:center; padding-left:10px;'>" +
                        currentCurrency + FormatAmount(EqpActualDiscount) +
                        "</td></tr><tr style='font-weight:bold'>" +
                        "<td colspan='3' style='padding-top:5px;text-align:right;padding-right:10px'>Subtotal</td>" +
                        "<td style='padding-top:5px; text-align:center; padding-left:10px;'>" +
                        currentCurrency + FormatAmount(agreementPdf.UpfrontAddOnTotal- EqpActualDiscount) +
                        "</td></tr>"
                        + "<tr style='font-weight:bold'>" +
                        "<td colspan='3' style='padding-top:5px;text-align:right;padding-right:10px'>Tax</td>" +
                        "<td style='padding-top:5px; text-align:center; padding-left:10px;'>" +
                        currentCurrency + FormatAmount(((agreementPdf.UpfrontAddOnTotal - EqpActualDiscount) / 100) * agreementPdf.Tax) +
                        "</td></tr>"
                        + "<tr style='font-weight:bold'>" +
                        "<td colspan='3' style='padding-top:5px;text-align:right;padding-right:10px'>Total</td>" +
                        "<td style='padding-top:5px; text-align:center; padding-left:10px;'>" +
                        currentCurrency + FormatAmount((agreementPdf.UpfrontAddOnTotal - EqpActualDiscount) + (((agreementPdf.UpfrontAddOnTotal - EqpActualDiscount) / 100) * agreementPdf.Tax)) +
                        "</td></tr></tbody></table>";
                }
                if (agreementPdf.EquipmentList.Count == 0)
                {
                    CommercialEquipmentList = "";
                }
                #region Onit Smart Set up
                EquipmentListTable = "<table style='width:100%; float:left; border-collapse:collapse; border-bottom:1px solid #ccc;'>" +
                    "<tr style='text-transform:uppercase; font-weight:bold; border-bottom:1px solid #ccc;'>" +
                    "<td style='width:75%; padding:5px;'>" +
                    "Equipment" +
                    "</td>" +
                    "<td style='width:5%; padding:5px; text-align:center;'>" +
                    "Qty" +
                    "</td>" +
                    "<td style='width:10%; padding:5px; text-align:right;'>" +
                    "Price" +
                    "</td>" +
                    "<td style='width:10%; padding:5px; text-align:right;'>" +
                    "Total" +
                    "</td></tr> ";
                #endregion
                foreach (var item in agreementPdf.EquipmentList)
                {
                    EquipmentName = "  <div style = 'width:30%;float:left;padding-left:10px;'><span> &nbsp;" + item.Name + " </span></div> ";
                    EquipmentQuantity = " <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + item.Quantity + " </span></div>";
                    UnitPrice = " <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + currentCurrency + FormatAmount(item.UnitPrice) + " </span></div>";
                    DiscountUnitPrice = " <div style = 'width:20%;float:left;text-align:center'><span>" + currentCurrency + FormatAmount(item.DiscountUnitPricce) + " </span></div>";
                    TotalEquipment = "  <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + currentCurrency + FormatAmount(item.Total) + " </span></div>";
                    EquipmentListRab += " <div style='padding-top:5px;'>" + EquipmentName + EquipmentQuantity + UnitPrice + DiscountUnitPrice + TotalEquipment + "</div>";
                    EquipmentListTable += "<tr>" +
                        "<td style='padding:5px;'>" +
                        item.Name +
                        "</td>" +
                        "<td style='padding:5px; text-align:center;'>" +
                        item.Quantity +
                        "</td>" +
                        "<td style='padding:5px; text-align:right'>" +
                        currentCurrency + FormatAmount(item.UnitPrice) +
                        "</td>" +
                        "<td style='padding:5px; text-align:right;'>" +
                        currentCurrency + FormatAmount(item.Total) +
                        "</td></tr>";
                }
                EquipmentListTable += "</table>";
                templateVars.Add("EquipmentListOnit", EquipmentListTable);
                if (isServiceDetail == true)
                {
                    ServiceListTable = " <tr style='background-color:#000; color:#fff; height: 30px; border:2px solid #000;'><td style='width:40%; text-align:center; font-weight:bold;'>MONTHLY SERVICE</td><td style='text-align:center; font-weight:bold;'>Type</td><td style='text-align:center; font-weight:bold;'> Monthly</td><td style='text-align:center; font-weight:bold;'> Qty</td><td style='text-align:center; font-weight:bold;'>Total $</td></tr>";
                }
                else
                {
                    ServiceListTable = " <tr style='background-color:#000; color:#fff; height: 30px; border:2px solid #000;'><td colspan='4' style='text-align:center; font-weight:bold;'>MONTHLY SERVICE</td></tr>";
                }
                if (!string.IsNullOrWhiteSpace(agreementPdf.ContractType) && agreementPdf.ContractType.ToLower() == "commercialfire")
                {
                    CommercialServiceList += "<table style='width:100%; float:left; border-collapse:collapse; margin-bottom:30px; margin-top:10px;'>"
                        + "<thead>"
                        + "<tr style='background-color:#4f90bb;color:white;width:100%;border:1px solid #4f90bb'>"
                        + "<th style='width:86%; text-align:left; padding:5px 0px 5px 20px; border-right:1px solid #fff'>SERVICE</th>"
                        + "</tr></thead><tbody>";
                }
                #region Onit Service
                ServiceListOnit = "<table style='width:100%; float:left; border-collapse:collapse; border-bottom:1px solid #ccc;'>" +
                    "<tr style='text-transform:uppercase; font-weight:bold; border-bottom:1px solid #ccc;'>" +
                    "<td style='width:80%; padding:5px;'>" +
                    "Service" +
                    "</td>" +
                    "<td style='width:10%; padding:5px; text-align:right;'>" +
                    "</td>" +
                    "<td style='width:10%; padding:5px; text-align:right;'>" +
                    "</td></tr> ";
                templateVars.Add("SubtotalOnit", currentCurrency + FormatAmount(MonthlyServiceFee + upfrontamount));
                templateVars.Add("TaxOnit", currentCurrency + FormatAmount((MonthlyServiceFee + upfrontamount) * (agreementPdf.Tax / 100)));
                templateVars.Add("TotalOnit", currentCurrency + FormatAmount(MonthlyServiceFee + upfrontamount + ((MonthlyServiceFee + upfrontamount) * (agreementPdf.Tax / 100))));
                #endregion
                foreach (var item in ArbEnabledServiceList)
                {
                    ServiceName = "  <div style = 'width:54%;float:left;padding-left:10px;'><span> &nbsp;" + item.Name + " </span></div> ";
                    MonthlyRate = "  <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + currentCurrency + FormatAmount(item.MonthlyRate) + " </span></div> ";
                    DiscountRate = " <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + currentCurrency + FormatAmount(item.DiscountRate) + " </span></div>";
                    Total = " <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + currentCurrency + FormatAmount(item.Total) + " </span></div>";
                    ServiceList += " <div style='padding-top:5px;'>" + ServiceName + MonthlyRate + DiscountRate + Total + "</div>";
                    ServiceListOnit += "<tr>" +
                        "<td style='padding:5px;'>" +
                        item.Name +
                        "</td>" +
                        "<td style='padding:5px; text-align:right;'>" +
                        currentCurrency + FormatAmount(item.MonthlyRate) +
                        "</td>" +
                        "<td style='padding:5px; text-align:right;'>" +
                        currentCurrency + FormatAmount(item.Total) +
                        "</td></tr> ";
                    if (isServiceDetail == true)
                    {
                        MonthlyService = "<td style='text-align:right;font-weight:bold; border:1px solid #000; padding:0px 10px;'>" + item.Name + "</td>";
                    }
                    else
                    {
                        MonthlyService = "<td colspan='4' style='text-align:center;font-weight:bold; border:1px solid #000; padding:0px 10px;'>" + item.Name + "</td>";
                    }
                    Type = "<td style='text-align:center; border:1px solid #000;'>" + item.Category + "</td>";
                    Monthly = "<td style='text-align:center; font-weight:bold; border:1px solid #000;'>" + currentCurrency + FormatAmount(item.Total) + "</td>";
                    Qty = "<td style='text-align:center; border:1px solid #000;'>" + item.Quantity + "</td>";
                    TotalDfw = "<td style='text-align:center; border:1px solid #000;'>" + FormatAmount(item.Total) + "</td>";
                    if (isServiceDetail == true)
                    {
                        ServiceListDFW += "<tr style='background-color:#bfbfbf; color:#000; height: 30px; border:2px solid #000;'>" + MonthlyService + Type + Monthly + Qty + TotalDfw + "</tr>";
                    }
                    else
                    {
                        ServiceListDFW += "<tr style='background-color:#bfbfbf; color:#000; height: 30px; border:2px solid #000;><td style='width:100%; text-align:center; font-weight:bold;'>" + MonthlyService + "</td></tr>";
                    }
                    if (!string.IsNullOrWhiteSpace(agreementPdf.ContractType) && agreementPdf.ContractType.ToLower() == "commercialfire")
                    {
                        CommercialServiceList += "<tr style='border-bottom:1px solid #ccc'>"
                            + "<td style='padding:5px 0px 5px 20px'>" +
                            "<strong>" +
                            "<label>" +
                             item.Name +
                            "</label>" +
                            "</strong></td>"
                            + "</tr>";
                    }
                }
                ServiceListOnit += "</table>";
                templateVars.Add("ServiceListOnit", ServiceListOnit);
                if (!string.IsNullOrWhiteSpace(agreementPdf.ContractType) && agreementPdf.ContractType.ToLower() == "commercialfire")
                {
                    CommercialServiceList += "<tr style='font-weight:bold'>"
                        + "<td style='padding-top:5px; text-align:right; padding-right:10px'>Subtotal</td >"
                        + "<td style='padding-top:5px; text-align:center; padding-left:10px;'>" +
                        currentCurrency + FormatAmount(agreementPdf.MonthlyServiceFeeTotal) +
                        "</td></tr>"
                        + "<tr style='font-weight:bold'>"
                        + "<td style='padding-top:5px; text-align:right; padding-right:10px'>Tax</td>"
                        + "<td style='padding-top:5px; text-align:center; padding-left:10px;'>" +
                        currentCurrency + FormatAmount((agreementPdf.MonthlyServiceFeeTotal / 100) * agreementPdf.Tax) +
                        "</td></tr>"
                        + "<tr style='font-weight:bold'>"
                        + "<td style='padding-top:5px; text-align:right; padding-right:10px'>Total</td >"
                        + "<td style='padding-top:5px; text-align:center; padding-left:10px;'>" +
                        currentCurrency + FormatAmount(agreementPdf.MonthlyServiceFeeTotal + ((agreementPdf.MonthlyServiceFeeTotal / 100) * agreementPdf.Tax)) +
                        "</td></tr></tbody></table>";
                }
                if (ArbEnabledServiceList.Count == 0)
                {
                    CommercialServiceList = "";
                }
                foreach (var item in ArbEnabledServiceList)
                {
                    ServiceName = "  <div style = 'width:35%;float:left;padding-left:10px;'><span> &nbsp;" + item.Name + " </span></div> ";
                    MonthlyRate = "  <div style = 'width:20%;float:left;text-align:center'><span> &nbsp;" + currentCurrency + FormatAmount(item.MonthlyRate) + " </span></div> ";
                    DiscountRate = " <div style = 'width:20%;float:left;text-align:center'><span> &nbsp;" + currentCurrency + FormatAmount(item.DiscountRate) + " </span></div>";
                    Total = " <div style = 'float:left;text-align:center'><span> &nbsp;" + currentCurrency + FormatAmount(item.Total) + " </span></div>";
                    ServiceListRab += " <div style='padding-top:5px;'>" + ServiceName + MonthlyRate + DiscountRate + Total + "</div>";
                }
                if (agreementPdf.ListAgreementAnswer != null && agreementPdf.ListAgreementAnswer.Count > 0)
                {
                    string ansval = "";
                    foreach (var item in agreementPdf.ListAgreementAnswer)
                    {
                        if (item.QuestionId == 1)
                        {
                            if (item.Answer == "true")
                            {
                                ansval = "Yes";
                            }
                            else
                            {
                                ansval = "No";
                            }
                            divAnsval = "<div style='word-wrap: break-word;'>Are you the homeowner? (" + ansval + ")</div>";
                        }
                        if (item.QuestionId == 2)
                        {
                            if (item.Answer == "true")
                            {
                                ansval = "Yes";
                            }
                            else
                            {
                                ansval = "No";
                            }
                            divAnsval = "<div style='word-wrap: break-word;'>Is your home new construction? (" + ansval + ")</div>";
                        }
                        if (item.QuestionId == 3)
                        {
                            if (item.Answer == "true")
                            {
                                ansval = "Yes";
                            }
                            else
                            {
                                ansval = "No";
                            }

                            divAnsval = " <div style='word-wrap: break-word;'> Are you under any contractual agreement/ obligation with any other company for monitoring services? (" + ansval + ")</div> ";
                        }
                        if (item.QuestionId == 4)
                        {
                            if (item.Answer == "true")
                            {
                                ansval = "Yes";
                            }
                            else
                            {
                                ansval = "No";
                            }
                            divAnsval = " <div style='word-wrap: break-word;'>  I understand that the Company or any representaive of the Company cannot be responsible for cancelling any services with my current security company(if applicable). (" + ansval + ")</div> ";
                        }
                        if (item.QuestionId == 5)
                        {
                            if (item.Answer == "true")
                            {
                                ansval = "Yes";
                            }
                            else
                            {
                                ansval = "No";
                            }
                            String month = "";
                            if (!string.IsNullOrWhiteSpace(agreementPdf.SubscribedMonths) && agreementPdf.SubscribedMonths.ToLower() != "month to month")
                            {
                                if (agreementPdf.SubscribedMonths == "1")
                                {
                                    month = "<span>month</span>";
                                }
                                else
                                {
                                    month = "<span>months</span>";
                                }
                            }
                            divAnsval = "<div style = 'word-wrap: break-word;'> I understand that I have signed an agreement to receive monitoring services for<Span style = 'text-decoration:underline;font-weight :600;'> &nbsp; &nbsp; &nbsp; &nbsp;" + agreementPdf.SubscribedMonths + " &nbsp; &nbsp; &nbsp; &nbsp;</span><span>" + month + "</span></div>";
                        }
                        ListAgreementAnswer += divAnsval;
                    }
                }
                else
                {
                    ListAgreementAnswer = "<div style='word-wrap: break-word;'>1. Are you the homeowner?</div><div style = 'word-wrap: break-word;' > 2.Is your home new construction?</div><div style = 'word-wrap: break-word;' >3.Are you under any contractual agreement / obligation with any other company for monitoring services?</div> <div style = 'word-wrap: break-word;' > 4.I understand that the Company or any representaive of the Company cannot be responsible for cancelling any services with my current security company(if applicable).</div><div style = 'word-wrap: break-word;' >5.I understand that I have signed an agreement to receive monitoring services for<span style = 'text-decoration:underline;font-weight :600;' > &nbsp; &nbsp; &nbsp; &nbsp;" + agreementPdf.SubscribedMonths + "&nbsp;&nbsp;&nbsp;&nbsp;</span> <span> month </span></div>";
                }
                if (agreementPdf.CustomerAgreement != null)
                {
                    foreach (var item in agreementPdf.CustomerAgreement)
                    {

                        if (item.Type == "LoadAgreement")
                        {
                            CustomerAgreement = "<span> Load: </span><span style = 'font-weight:600;'>" + DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("MM/dd/yyyy") + "+ at+" + DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("hh:mm tt") + " </span><br/>";
                        }
                        if (item.Type == "SignAgreement")
                        {
                            CustomerAgreement = "<span> Sign: </span><span style = 'font-weight:600;'>" + DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("MM/dd/yyyy") + "+ at+" + DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("hh:mm tt") + " </span><br/>";

                        }
                        if (item.Type == "SubmitAgreement")
                        {
                            CustomerAgreement = "<span> Submit: </span><span style = 'font-weight:600;'>" + DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("MM/dd/yyyy") + "+ at+" + DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("hh:mm tt") + " </span><br/>";
                        }
                    }
                    CustomerAgreementTable = "<table style='width:100%'><tr style='background-color:darkgray;'><th style='width:33%; text-align:center;'>IP</th><th style='width:33%; text-align:center;'>USER AGENT</th><th style='width:33%; text-align:center;'>TIMESTAMP</th> </tr><tr><td style='width:33%; text-align:center;'>" + agreementPdf.SingleCustomerAgreement.IP + "</td><td style='width:33%; text-align:center;'>" + agreementPdf.SingleCustomerAgreement.UserAgent + "</td><td style='width:33%; text-align:center;'>" + CustomerAgreement + " </td></tr>";
                }
                #region BBB Conflict
                if (IsBBBConflict)
                {
                    foreach (var item in agreementPdf.CustomerAgreement)
                    {

                        if (item.Type == "AgreementCreate")
                        {
                            templateVars.Add("AgreementCreatedDate", DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("MM/dd/yy"));
                            templateVars.Add("AgreementCreatedDateTime", DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("MM/dd/yy") + " at " + DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("hh:mm tt"));
                        }
                        if (item.Type == "AgreementSend")
                        {
                            templateVars.Add("AgreementSendDateTime", DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("MM/dd/yy") + " at " + DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("hh:mm tt"));
                        }
                        if (item.Type == "AgreementSign")
                        {
                            templateVars.Add("AgreementSignDate", DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("MM/dd/yy"));
                            templateVars.Add("AgreementSignDateTime", DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("MM/dd/yy") + " at " + DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("hh:mm tt"));
                            templateVars.Add("AgreementSignIp", item.IP);
                        }
                        if (item.Type == "AgreementComplete")
                        {
                            templateVars.Add("AgreementCompletedDateTime", DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("MM/dd/yy") + " at " + DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("hh:mm tt"));
                        }
                    }
                }
                #endregion
                templateVars.Add("EmergencyContactList", EmergencyContactList);
                templateVars.Add("EquipmentList", EquipmentList);
                templateVars.Add("ServiceList", ServiceList);
                templateVars.Add("EquipmentListRab", EquipmentListRab);
                templateVars.Add("EquipmentListDFW", EquipmentListDFW);
                templateVars.Add("ServiceListRab", ServiceListRab);
                templateVars.Add("ServiceListDFW", ServiceListDFW);
                templateVars.Add("ServiceListTable", ServiceListTable);
                templateVars.Add("ListAgreementAnswer", ListAgreementAnswer);
                templateVars.Add("CustomerAgreementTable", CustomerAgreementTable);
                templateVars.Add("Password", agreementPdf.Password);
                templateVars.Add("ListContactEmergency", agreementPdf.ListContactEmergency);
                templateVars.Add("ListPaymentInfo", agreementPdf.ListPaymentInfo);
                templateVars.Add("CommercialEquipmentList", CommercialEquipmentList);
                
                if (!(agreementPdf.ContractType.ToLower() == "commercialfire"))
                { 
                templateVars.Add("CommercialServiceList", CommercialServiceList);
                }
                #region Invoice List
                string invDelListGutter = "<table style='width:30%; float:left; border-collapse:collapse; margin-top:10px;'>" +
                    "<tr>" +
                    "<td style='width:70%;'>" +
                    "</td>" +
                    "<td style='width:30%;'>" +
                    "</td></tr>" +
                    "<tr>" +
                    "<td colspan='2' style='font-size:18px; text-align:center;'>" +
                    "DETAILS" +
                    "</td></tr>" +
                    "<tr>" +
                    "<td style='border:1px solid #ccc; padding:2px 5px;'>" +
                    "Product Name" +
                    "</td>" +
                    "<td style='border:1px solid #ccc; padding:2px 5px; text-align:center;'>" +
                    "Rate" +
                    "</td></tr>";
                if (!string.IsNullOrWhiteSpace(agreementPdf.InvoiceId) && agreementPdf.IsInvoice == true)
                {
                 
                    templateVars.Add("EstimateId", agreementPdf.InvoiceId);
                    templateVars.Add("MonitoringAmount", currentCurrency + FormatAmount(agreementPdf.inv.MonitoringAmount.HasValue ? agreementPdf.inv.MonitoringAmount : 0.0));
                    templateVars.Add("MonitoringDescription", agreementPdf.inv.MonitoringDescription != "-1" ? agreementPdf.inv.MonitoringDescription : "");
                    if (!string.IsNullOrWhiteSpace(agreementPdf.inv.ContractTerm) && agreementPdf.inv.ContractTerm == "0")
                    {
                        templateVars.Add("ContractTerm", "Month to Month");
                    }
                    else if (!string.IsNullOrWhiteSpace(agreementPdf.inv.ContractTerm) && agreementPdf.inv.ContractTerm != "-1")
                    {
                        templateVars.Add("ContractTerm", Convert.ToInt32(agreementPdf.inv.ContractTerm) * 12);
                    }
                    else
                    {
                        templateVars.Add("ContractTerm", 0);
                    }
                    if (agreementPdf.InvoiceList != null)
                    {
                        foreach (var item in agreementPdf.InvoiceList)
                        {
                            InvoiceQTY = "<td style='text-align:right; border:1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + item.Quantity + "</td>";
                            InvoiceName = "<td style='text-align:center; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + item.EquipName + "</td>";
                            InvoicePrice = "<td style='text-align:right; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + currentCurrency + FormatAmount(item.UnitPrice) + "</td>";
                            InvoiceSubTotal = "<td style='text-align:right; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + currentCurrency + FormatAmount(item.TotalPrice) + "</td>";

                            InvoiceList += "<tr style=''>" + InvoiceQTY + InvoiceName + InvoicePrice + InvoiceSubTotal + "</tr>";
                            InvoiceTotalSubTotal += item.TotalPrice.Value;
                            invDelListGutter += "<tr>" +
                                "<td style='border:1px solid #ccc; padding:2px 5px;'>" +
                                item.EquipName +
                                "</td>" +
                                "<td style='border:1px solid #ccc; padding:2px 5px; text-align:center;'>" +
                                currentCurrency + FormatAmount(item.UnitPrice) +
                                "</td></tr>";
                        }
                    }
                    else
                    {
                        InvoiceQTY = "<td style='text-align:right; border:1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + 0 + "</td>";
                        InvoiceName = "<td style='text-align:center; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'> </td>";
                        InvoicePrice = "<td style='text-align:right; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + currentCurrency + FormatAmount(0.0) + "</td>";
                        InvoiceSubTotal = "<td style='text-align:right; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + currentCurrency + FormatAmount(0.0) + "</td>";

                        InvoiceList += "<tr style=''>" + InvoiceQTY + InvoiceName + InvoicePrice + InvoiceSubTotal + "</tr>";
                    }
                    InvoiceTotalSubTotalWithUpfront = InvoiceTotalSubTotal;

                    #region Estimate Upfront List
                    EstimateUpfrontChargeList = "<table style='width:100%;float:left;border-collapse:collapse;'>" +
                        "<thead><tr style='background-color:#2ca01c;width:100%;border:1px solid #ccc; color:#fff;'>" +
                        "<th style='width:75%;text-align:left;padding:5px 0px 5px 20px;border-right:1px solid #ccc'>" +
                        "PRODUCT" +
                        "</th>" +
                        "<th style='width:5%;padding:5px 0px;border-right:1px solid #ccc;text-align:center'>" +
                        "QTY</th>" +
                        "<th style='width:10%;padding:5px 0px;border-right:1px solid #ccc;text-align:center'>" +
                        "UNIT PRICE</th>" +
                        //"<th style='width:8%;padding:5px 0px;border-right:1px solid #ccc;text-align:center'>" +
                        //"DISCOUNT" +
                        //"</th>" +
                        "<th style='width:10%;padding:5px 0px;border-right:1px solid #ccc;text-align:center'>" +
                        "PRICE</th>" +
                        "</tr></thead><tbody>";
                    EstimateRecurringChargeList = "<table style='width:100%;float:left;border-collapse:collapse;'>" +
                        "<thead><tr style='background-color:#2ca01c;width:100%;border:1px solid #ccc; color:#fff;'>" +
                        "<th style='width:75%;text-align:left;padding:5px 0px 5px 20px;border-right:1px solid #ccc'>" +
                        "SERVICE</th>" +
                        "<th style='width:5%;padding:5px 0px;border-right:1px solid #ccc;text-align:center'> " +
                        "QTY " +
                        "</th><th style='width:10%;padding:5px 0px;border-right:1px solid #ccc;text-align:center'>" +
                        "UNIT PRICE" +
                        "</th><th style='width:10%;padding:5px 0px;border-right:1px solid #ccc;text-align:center'> " +
                        "PRICE " +
                        "</th></tr></thead><tbody>";
                    if (agreementPdf.InvoiceList != null)
                    {
                        foreach (var item in agreementPdf.InvoiceList)
                        {
                            if (item.EquipmentClassId == 1)
                            {
                                EstimateUpfrontChargeList += "<tr style='border:1px solid #ccc'>" +
                                "<td style='padding: 5px 0px 5px 20px; border: 1px solid #ccc'>" +
                                item.EquipName +
                                "</td>" +
                                "<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                item.Quantity +
                                "</td>" +
                                "<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                currentCurrency + FormatAmount(item.UnitPrice) +
                                "</td>" +
                                //"<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                //"116.99" +
                                //"</td>" +
                                "<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                currentCurrency + FormatAmount(item.TotalPrice) +
                                "</td></tr>";
                                EstimateUpfrontTotalSubTotal += item.TotalPrice.Value;
                            }
                            else if (item.EquipmentClassId == 2)
                            {
                                EstimateRecurringChargeList += "<tr style='border:1px solid #ccc'>" +
                                    "<td style='padding:5px 0px 5px 20px;border:1px solid #ccc'>" +
                                    item.EquipName +
                                    "</td>" +
                                    "<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                    item.Quantity +
                                    "</td>" +
                                    "<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                    currentCurrency + FormatAmount(item.UnitPrice) +
                                    "</td>" +
                                    "<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                    currentCurrency + FormatAmount(item.TotalPrice) +
                                    "</td></tr>";
                                EstimateRecurringTotalSubTotal += item.TotalPrice.Value;
                            }
                        }
                    }
                    else
                    {
                        EstimateRecurringChargeList += "<tr style='border:1px solid #ccc'><td style='padding:5px 0px 5px 20px;border:1px solid #ccc'>" +
                                    "" +
                                    "</td>" +
                                    "<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                    1 +
                                    "</td>" +
                                    "<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                    currentCurrency + FormatAmount(0.0) +
                                    "</td>" +
                                    "<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                    currentCurrency + FormatAmount(0.0) +
                                    "</td></tr>";
                        EstimateRecurringTotalSubTotal += 0.0;
                    }
                    EstimateUpfrontChargeList += "<tr style='border:1px solid #ccc'>" +
                                "<td style='padding: 5px 0px 5px 20px; border: 1px solid #ccc'>" +
                                "Advance Monitoring Fee" +
                                "</td>" +
                                "<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                1 +
                                "</td>" +
                                "<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                currentCurrency + FormatAmount(AdvanceMonitoringFee) +
                                "</td>" +
                                //"<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                //"116.99" +
                                //"</td>" +
                                "<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                currentCurrency + FormatAmount(AdvanceMonitoringFee) +
                                "</td></tr>";
                    EstimateUpfrontTotalSubTotal += AdvanceMonitoringFee;
                    EstimateUpfrontChargeList += "<tr style='border:1px solid #ccc'>" +
                                "<td style='padding: 5px 0px 5px 20px; border: 1px solid #ccc'>" +
                                "Non-Conforming Fee" +
                                "</td>" +
                                "<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                1 +
                                "</td>" +
                                "<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                currentCurrency + FormatAmount(agreementPdf.NonConfirmingFee) +
                                "</td>" +
                                //"<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                //"116.99" +
                                //"</td>" +
                                "<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                currentCurrency + FormatAmount(agreementPdf.NonConfirmingFee) +
                                "</td></tr>";
                    EstimateUpfrontTotalSubTotal += agreementPdf.NonConfirmingFee;
                    EstimateUpfrontChargeList += "<tr style='border:1px solid #ccc'>" +
                                "<td style='padding: 5px 0px 5px 20px; border: 1px solid #ccc'>" +
                                "Activation Fee" +
                                "</td>" +
                                "<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                1 +
                                "</td>" +
                                "<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                currentCurrency + FormatAmount(agreementPdf.ActivationFee) +
                                "</td>" +
                                //"<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                //"116.99" +
                                //"</td>" +
                                "<td style='padding:5px;text-align:center; border:1px solid #ccc'>" +
                                currentCurrency + FormatAmount(agreementPdf.ActivationFee) +
                                "</td></tr>";
                    EstimateUpfrontTotalSubTotal += agreementPdf.ActivationFee;
                    EstimateUpfrontChargeList += "<tr style=font-weight:bold'>" +
                        "<td colspan='2'>" +
                        "</td>" +
                        "<td style='padding:5px;text-align:right;padding-right:10px;background-color:#f8f8f8; border:1px solid #ccc;'>Subtotal</td>" +
                        "<td style='padding:5px; padding-left:10px; border:1px solid #ccc;text-align:center;'>" +
                        currentCurrency + FormatAmount(EstimateUpfrontTotalSubTotal) +
                        "</td></tr>" +
                        "<tr style='font-weight:bold'>" +
                        "<td colspan='2'></td>" +
                        "<td style='padding:5px;text-align:right;padding-right:10px;background-color:#f8f8f8; border:1px solid #ccc;'>" +
                        "Tax</td>" +
                        "<td style='padding:5px; padding-left:10px; border:1px solid #ccc;text-align:center;'>" +
                        currentCurrency + FormatAmount((EstimateUpfrontTotalSubTotal * agreementPdf.Tax) / 100) +
                        "</td></tr>" +
                        "<tr style='font-weight:bold'>" +
                        "<td colspan='2' ></td>" +
                        "<td style='padding:5px;text-align:right;padding-right:10px;background-color:#f8f8f8; border:1px solid #ccc;'>" +
                        "Total</td>" +
                        "<td style='padding:5px; padding-left:10px; border:1px solid #ccc;text-align:center;'>" +
                        currentCurrency + FormatAmount(EstimateUpfrontTotalSubTotal + ((EstimateUpfrontTotalSubTotal * agreementPdf.Tax) / 100)) +
                        "</td></tr></tbody></table>";
                    EstimateRecurringChargeList += "<tr style=font-weight:bold'>" +
                        "<td colspan='2'>" +
                        "</td>" +
                        "<td style='padding:5px;text-align:right;padding-right:10px;background-color:#f8f8f8; border:1px solid #ccc;'>Subtotal</td>" +
                        "<td style='padding:5px; padding-left:10px; border:1px solid #ccc;text-align:center;'>" +
                        currentCurrency + FormatAmount(EstimateRecurringTotalSubTotal) +
                        "</td></tr>" +
                        "<tr style='font-weight:bold'>" +
                        "<td colspan='2'></td>" +
                        "<td style='padding:5px;text-align:right;padding-right:10px;background-color:#f8f8f8; border:1px solid #ccc;'>" +
                        "Tax</td>" +
                        "<td style='padding:5px; padding-left:10px; border:1px solid #ccc;text-align:center;'>" +
                        currentCurrency + FormatAmount((EstimateRecurringTotalSubTotal * agreementPdf.Tax) / 100) +
                        "</td></tr>" +
                        "<tr style='font-weight:bold'>" +
                        "<td colspan='2' ></td>" +
                        "<td style='padding:5px;text-align:right;padding-right:10px;background-color:#f8f8f8; border:1px solid #ccc;'>" +
                        "Total</td>" +
                        "<td style='padding:5px; padding-left:10px; border:1px solid #ccc;text-align:center;'>" +
                        currentCurrency + FormatAmount(EstimateRecurringTotalSubTotal + ((EstimateRecurringTotalSubTotal * agreementPdf.Tax) / 100)) +
                        "</td></tr></tbody></table>";
                    templateVars.Add("EstimateUpfrontChargeList", EstimateUpfrontChargeList);
                    templateVars.Add("EstimateRecurringChargeList", EstimateRecurringChargeList);
                    templateVars.Add("EstimateEmergencyContactList", EstimateEmergencyContactList);
                    #endregion

                    if (!string.IsNullOrWhiteSpace(agreementPdf.inv.UpfrontMonth) && agreementPdf.inv.UpfrontMonth != "-1")
                    {
                        InvoiceQTY = "<td style='text-align:right; border:1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + agreementPdf.inv.UpfrontMonth + "</td>";
                        InvoiceName = "<td style='text-align:center; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + "Upfront month charge" + "</td>";
                        InvoicePrice = "<td style='text-align:right; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + currentCurrency + FormatAmount(agreementPdf.inv.MonitoringAmount.HasValue ? agreementPdf.inv.MonitoringAmount : 0.0) + "</td>";
                        InvoiceSubTotal = "<td style='text-align:right; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + currentCurrency + FormatAmount((agreementPdf.inv.MonitoringAmount.HasValue ? agreementPdf.inv.MonitoringAmount : 0.0) * (Convert.ToInt32(agreementPdf.inv.UpfrontMonth))) + "</td>";

                        InvoiceList += "<tr style=''>" + InvoiceQTY + InvoiceName + InvoicePrice + InvoiceSubTotal + "</tr>";

                        InvoiceTotalSubTotalWithUpfront += Convert.ToDouble((agreementPdf.inv.MonitoringAmount.HasValue ? agreementPdf.inv.MonitoringAmount : 0.0) * (Convert.ToInt32(agreementPdf.inv.UpfrontMonth)));
                    }

                    InvoiceQTY = "<td style='text-align:right; border:1px solid #ebebeb; padding:10px; height:30px' valign='middle'>1</td>";
                    InvoiceName = "<td style='text-align:center; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + "Prorated amount" + "</td>";
                    InvoicePrice = "<td style='text-align:right; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + currentCurrency + FormatAmount(agreementPdf.ProratedAmout) + "</td>";
                    InvoiceSubTotal = "<td style='text-align:right; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + currentCurrency + FormatAmount(agreementPdf.ProratedAmout) + "</td>";

                    InvoiceList += "<tr style=''>" + InvoiceQTY + InvoiceName + InvoicePrice + InvoiceSubTotal + "</tr>";
                    InvoiceTotalSubTotalWithUpfront += agreementPdf.ProratedAmout;

                    InvoiceFinalTotal = agreementPdf.inv.TotalAmount.Value;
                    templateVars.Add("EstimateSubtotal", currentCurrency + FormatAmount(InvoiceTotalSubTotal));
                    if (agreementPdf.inv.EstimateTerm == "50UponAcceptance50UponCompletion")
                    {
                        EstimateSigningAmount = (InvoiceTotalSubTotalWithUpfront + (Math.Round(agreementPdf.inv.Tax.Value, 2))) / 2;
                        templateVars.Add("SigningAmount", currentCurrency + FormatAmount(EstimateSigningAmount));
                    }
                    else if (agreementPdf.inv.EstimateTerm == "DueonAcceptance")
                    {
                        EstimateSigningAmount = InvoiceTotalSubTotalWithUpfront + (Math.Round(agreementPdf.inv.Tax.Value, 2));
                        templateVars.Add("SigningAmount", currentCurrency + FormatAmount(EstimateSigningAmount));
                    }
                    else if (agreementPdf.inv.EstimateTerm == "DueUponCompletion")
                    {
                        templateVars.Add("SigningAmount", currentCurrency + FormatAmount(EstimateSigningAmount));
                    }

                    else
                    {
                        EstimateSigningAmount = InvoiceTotalSubTotalWithUpfront + (Math.Round(agreementPdf.inv.Tax.Value, 2));
                        templateVars.Add("SigningAmount", currentCurrency + FormatAmount(EstimateSigningAmount));
                    }
                    EstimateSigningAmountWithProrate = InvoiceTotalSubTotalWithUpfront + (Math.Round(agreementPdf.inv.Tax.Value, 2));
                    templateVars.Add("SigningAmountWithProrated", currentCurrency + FormatAmount(EstimateSigningAmountWithProrate));
                    InvoiceList += "<tr style=''><td colspan='2' style ='text-align:right; padding:10px;'></td ><td style ='text-align:right; padding:10px;'>Subtotal</td><td style ='text-align:right; padding:10px;font-weight:bold;'>" +
                                    currentCurrency + FormatAmount(InvoiceTotalSubTotalWithUpfront) +
                                "</td ></tr>";
                    InvoiceList += "<tr style=''><td colspan='2' style ='text-align:right; padding:10px;'></td ><td style ='text-align:right; padding:10px;'>" + agreementPdf.inv.TaxType + "</td><td style ='text-align:right; padding:10px;font-weight:bold;'>" +
                                    currentCurrency + FormatAmount(Math.Round(agreementPdf.inv.Tax.Value, 2)) +
                                "</td ></tr>";
                    InvoiceList += "<tr style='font-weight:bold;'><td colspan ='2' style='text-align:right; padding:10px;'></td><td style ='text-align:right; padding:10px;'>Estimate Total</td> <td style ='text-align:right; padding:10px;'>" +
                                    currentCurrency + FormatAmount(InvoiceTotalSubTotalWithUpfront + (Math.Round(agreementPdf.inv.Tax.Value, 2))) +
                                "</td></tr>";
                }
                else
                {
                    templateVars.Add("MonitoringAmount", currentCurrency + FormatAmount(Convert.ToDouble(agreementPdf.MonthlyMonitoringFee)));
                    templateVars.Add("ContractTerm", agreementPdf.SubscribedMonths);
                    if (agreementPdf.EquipmentList != null)
                    {
                        foreach (var item in agreementPdf.EquipmentList)
                        {
                            InvoiceQTY = "<td style='text-align:right; border:1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + item.Quantity + "</td>";
                            InvoiceName = "<td style='text-align:center; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + item.Name + "</td>";
                            InvoicePrice = "<td style='text-align:right; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + currentCurrency + FormatAmount(item.UnitPrice) + "</td>";
                            InvoiceSubTotal = "<td style='text-align:right; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + currentCurrency + FormatAmount(item.Total) + "</td>";

                            InvoiceList += "<tr style=''>" + InvoiceQTY + InvoiceName + InvoicePrice + InvoiceSubTotal + "</tr>";
                            InvoiceTotalSubTotal += item.Total;
                        }
                    }
                    else
                    {
                        InvoiceQTY = "<td style='text-align:right; border:1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + 0 + "</td>";
                        InvoiceName = "<td style='text-align:center; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'> </td>";
                        InvoicePrice = "<td style='text-align:right; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + currentCurrency + FormatAmount(0.0) + "</td>";
                        InvoiceSubTotal = "<td style='text-align:right; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + currentCurrency + FormatAmount(0.0) + "</td>";

                        InvoiceList += "<tr style=''>" + InvoiceQTY + InvoiceName + InvoicePrice + InvoiceSubTotal + "</tr>";
                    }
                    InvoiceFinalTotal = InvoiceTotalSubTotal + ((InvoiceTotalSubTotal * agreementPdf.Tax) / 100);
                    InvoiceList += "<tr style=''><td colspan='2' style ='text-align:right; padding:10px;'></td ><td style ='text-align:right; padding:10px;'>Subtotal</td><td style ='text-align:right; padding:10px;font-weight:bold;'>" +
                                    currentCurrency + FormatAmount(InvoiceTotalSubTotal) +
                                "</td ></tr>";
                    InvoiceList += "<tr style=''><td colspan='2' style ='text-align:right; padding:10px;'></td ><td style ='text-align:right; padding:10px;'>Tax(" + agreementPdf.Tax + "%)</td><td style ='text-align:right; padding:10px;font-weight:bold;'>" +
                                    currentCurrency + FormatAmount(((InvoiceTotalSubTotal * agreementPdf.Tax) / 100)) +
                                "</td ></tr>";
                    InvoiceList += "<tr style='font-weight:bold;'><td colspan ='2' style='text-align:right; padding:10px;'></td><td style ='text-align:right; padding:10px;'>Total</td> <td style ='text-align:right; padding:10px;'>" +
                                    currentCurrency + FormatAmount(InvoiceFinalTotal) +
                                "</td></tr>";
                    templateVars.Add("EstimateSubtotal", currentCurrency + FormatAmount(InvoiceTotalSubTotal));
                    EstimateSigningAmount = (InvoiceFinalTotal + (!string.IsNullOrWhiteSpace(agreementPdf.MonthlyMonitoringFee) ? Convert.ToDouble(agreementPdf.MonthlyMonitoringFee) : 0.0));
                    templateVars.Add("SigningAmount", currentCurrency + FormatAmount(EstimateSigningAmount));

                    EstimateSigningAmountWithProrate = (InvoiceFinalTotal + (!string.IsNullOrWhiteSpace(agreementPdf.MonthlyMonitoringFee) ? Convert.ToDouble(agreementPdf.MonthlyMonitoringFee) : 0.0) + agreementPdf.ProratedAmout);
                    templateVars.Add("SigningAmountWithProrated", currentCurrency + FormatAmount(EstimateSigningAmountWithProrate));

                }
                templateVars.Add("EstimateContractServiceList", InvoiceList);
                invDelListGutter += "</table>";
                templateVars.Add("GutterProductList", invDelListGutter);
                templateVars.Add("CtGutterLogo", string.Concat(AppConfig.SiteDomain, "/Content/img/ct_gutter_logo_pdf.png"));
                templateVars.Add("InvoiceDiagram", !string.IsNullOrWhiteSpace(agreementPdf.InvoiceDiagram) ? agreementPdf.InvoiceDiagram : "");
                #endregion

                #region Financed Amount
                if (agreementPdf.MonthlyFinanceRate > 0.0)
                {
                    FinancedAmount = "<table style='width:100%;float:left; border-collapse:collapse;'>" +
                        "<tbody>" +
                        "<tr>" +
                        "<td style='width:180px;border:2px solid #000;background-color:#f8f8f8;padding:3px;'>" +
                        "Monthly Finance Rate</td>" +
                        "<td style='border:2px solid #000; padding:3px;'>" +
                        currentCurrency + FormatAmount(agreementPdf.MonthlyFinanceRate) +
                        "</td></tr></tbody></table>";
                }
                if (agreementPdf.FinancedAmout > 0.0)
                {
                    FinancedAmount += "<table style='width:100%;float:left; border-collapse:collapse; margin-top:-2px;'>" +
                        "<tbody><tr><td style='width:180px;border:2px solid #000;background-color:#f8f8f8;padding:3px;'>" +
                        "Financed Amount</td>" +
                        "<td style='border:2px solid #000; padding:3px;'>" +
                        currentCurrency + FormatAmount(agreementPdf.FinancedAmout) +
                        "</td></tr></tbody></table>";
                }
                templateVars.Add("FinancedAmountInfo", FinancedAmount);
                #endregion

                #region Estimator Detail
                if (agreementPdf.createEst != null && agreementPdf.createEst.Estimator != null && agreementPdf.IsEstimator == true)
                {
                    templateVars.Add("EstimatorId", agreementPdf.createEst.Estimator.EstimatorId);
                    templateVars.Add("eSecurityLogo", agreementPdf.createEst.eSecurityLogo);
                    templateVars.Add("SpecializedLogo", agreementPdf.createEst.specializedLogo);
                    if (agreementPdf.userInfo != null)
                    {
                        templateVars.Add("UserName", agreementPdf.userInfo.FirstName + " " + agreementPdf.userInfo.LastName);
                        templateVars.Add("UserPhone", agreementPdf.userInfo.Phone);
                        templateVars.Add("UserEmail", agreementPdf.userInfo.Email);
                    }
                    if (agreementPdf.createEst.estimatorDetails != null)
                    {
                        EstimatorDetailList = "<table style='width:100%; float:left; border-collapse:collapse; margin-bottom:30px;'>"
                                           + "<thead>"
                                           + "<tr style='background-color:#4f90bb; color:white;width:100%; border:1px solid #4f90bb;'>"
                                           + "<th style='width:46%; text-align:left; padding:5px 0px 5px 20px; border-right:1px solid #fff;'>"
                                           + "PRODUCT</th>"
                                           + "<th style='width:10%; padding:5px 0px; border-right:1px solid #fff; text-align:center;'>SKU</th>"
                                           + "<th style='width:5%; padding:5px 0px; border-right:1px solid #fff; text-align:center;'>QTY</th>";
                        if (agreementPdf.createEst._EstimatorPDFFilter.IncludeCost.Value)
                        {
                            EstimatorDetailList += "<th style='width:8%; padding:5px 0px; border-right:1px solid #fff; text-align:center;'>U COST</th>"
                                           + "<th style='width:8%; padding:5px 0px; border-right:1px solid #fff; text-align:center;'>T COST</th>";
                        }
                        if (agreementPdf.createEst._EstimatorPDFFilter.IncludeProfit.Value)
                        {
                            EstimatorDetailList += "<th style='width:8%; padding:5px 0px; border-right:1px solid #fff; text-align:center;'>PROFIT</th>";
                        }
                        if (agreementPdf.createEst._EstimatorPDFFilter.IncludeOverhead.Value)
                        {
                            EstimatorDetailList += "<th style='width:8%; padding:5px 0px; border-right:1px solid #fff; text-align:center;'>OVERHEAD</th>";
                        }
                        if (!agreementPdf.createEst._EstimatorPDFFilter.WithoutPricing.Value)
                        {
                            if (!agreementPdf.createEst._EstimatorPDFFilter.WithoutIndividualMaterialPricing.Value || !agreementPdf.createEst._EstimatorPDFFilter.WithoutIndividualLaborPricing.Value)
                            {
                                EstimatorDetailList += "<th style='width:8%; padding:5px 0px; border-right:1px solid #fff; text-align:center;'>U PRICE</th>"
                                    + "<th style='width:10%;padding:5px 0px;'>TOTAL PRICE</th>";
                            }
                        }

                        EstimatorDetailList += "</tr></thead>"
                        + "<tbody>";
                        if (agreementPdf.createEst._EstimatorPDFFilter.GroupedbyCategory.Value)
                        {
                            List<string> CategoryList = agreementPdf.createEst.estimatorDetails.GroupBy(x => x.CategoryVal).Select(x => x.Key).ToList();
                            foreach (var Category in CategoryList)
                            {
                                EstimatorDetailList += "<tr style='border-bottom:1px solid #ccc;'>"
                                     + "<td style ='padding:5px 0px 5px 40px;'>" +
                                     "<strong>" +
                                     "<label>";
                                if (Category == "Category")
                                {
                                    EstimatorDetailList += "Category";
                                }
                                else
                                {
                                    EstimatorDetailList += "Uncategorized";
                                }
                                EstimatorDetailList += "</label></strong></td></tr>";
                                EstimatorDetailList += GetEstimatorLineItems(agreementPdf.createEst.estimatorDetails.Where(x => x.CategoryVal == Category).ToList(), currentCurrency, agreementPdf.createEst._EstimatorPDFFilter);
                            }
                            //foreach (var item in agreementPdf.createEst.estimatorDetails)
                            //{
                            //    EstimatorProduct = "<td style='padding:5px 0px 5px 20px;'><strong><label>" + item.PartDescription + "</label></strong></td>";
                            //    EstimatorSKU = "<td style='padding:5px; text-align:center;'><strong><label>" + item.PartNumber + "</label></strong></td>";
                            //    EstimatorQTY = "<td style='padding:5px; text-align:center; '><strong><label>" + item.Qunatity + "</label></strong></td>";
                            //    EstimatorUCOST = "<td style='padding:5px; text-align:center; '><strong><label>" + currentCurrency + FormatAmount(item.UnitCost) + "</label></strong></td>";
                            //    EstimatorTCOST = "<td style='padding:5px; text-align:center; '><strong><label>" + currentCurrency + FormatAmount(item.TotalCost) + "</label></strong></td>";
                            //    EstimatorPROFIT = "<td style='padding:5px; text-align:center; '><strong><label>" + currentCurrency + FormatAmount(item.Profit) + "</label></strong></td>";
                            //    EstimatorOVERHEAD = "<td style='padding:5px; text-align:center; '><strong><label>" + currentCurrency + FormatAmount(item.Overhead) + "</label></strong></td>";

                            //    EstimatorDetailList += "<tr style='border-bottom:1px solid #ccc;'>" + EstimatorProduct + EstimatorSKU + EstimatorQTY + EstimatorUCOST + EstimatorTCOST + EstimatorPROFIT + EstimatorOVERHEAD + "</tr>";
                            //}
                        }
                        else if (agreementPdf.createEst._EstimatorPDFFilter.GroupedbySupplier.Value)
                        {
                            List<string> SupplierList = agreementPdf.createEst.estimatorDetails.GroupBy(x => x.SupplierVal).Select(x => x.Key).ToList();
                            foreach (var Supplierval in SupplierList)
                            {
                                EstimatorDetailList += "<tr>"
                                    + "<td style='padding:5px 0px 5px 40px;'>"
                                    + "<strong>"
                                    + "<label>"
                                    + (Supplierval == "Supplier" ? "Others" : Supplierval)
                                    + "</label></strong></td>"
                                    + "</tr>";
                                EstimatorDetailList += GetEstimatorLineItems(agreementPdf.createEst.estimatorDetails.Where(x => x.SupplierVal == Supplierval).ToList(), currentCurrency, agreementPdf.createEst._EstimatorPDFFilter);

                            }
                        }
                        else if (agreementPdf.createEst._EstimatorPDFFilter.GroupedbyLabor.Value || agreementPdf.createEst._EstimatorPDFFilter.GroupedbyMaterial.Value)
                        {
                            var Items = agreementPdf.createEst.estimatorDetails.Where(x => x.CategoryVal == "Labor").ToList();
                            if (agreementPdf.createEst._EstimatorPDFFilter.GroupedbyMaterial.Value)
                            {
                                Items = agreementPdf.createEst.estimatorDetails.Where(x => x.CategoryVal != "Labor").ToList();
                            }
                            EstimatorDetailList += GetEstimatorLineItems(Items, currentCurrency, agreementPdf.createEst._EstimatorPDFFilter);
                        }
                        else if (agreementPdf.createEst._EstimatorPDFFilter.GroupedbyLaborAndMaterial.Value)
                        {
                            EstimatorDetailList += "<tr>"
                                + "<td style='padding:5px 0px 5px 40px;'>"
                                + "<strong>"
                                + "<label>" +
                                "Labor"
                                + "</label></strong></td>"
                                + "</tr>";
                            var Items = agreementPdf.createEst.estimatorDetails.Where(x => x.CategoryVal == "Labor").ToList();
                            EstimatorDetailList += GetEstimatorLineItems(Items, currentCurrency, agreementPdf.createEst._EstimatorPDFFilter);
                            EstimatorDetailList += "<tr>"
                                + "<td style='padding:5px 0px 5px 40px;'>"
                                + "<strong>"
                                + "<label>"
                                + "Material"
                                + "</label></strong></td>"
                                + "</tr>";
                            Items = agreementPdf.createEst.estimatorDetails.Where(x => x.CategoryVal != "Labor").ToList();
                            EstimatorDetailList += GetEstimatorLineItems(Items, currentCurrency, agreementPdf.createEst._EstimatorPDFFilter);
                        }
                        else
                        {
                            EstimatorDetailList += GetEstimatorLineItems(agreementPdf.createEst.estimatorDetails, currentCurrency, agreementPdf.createEst._EstimatorPDFFilter);
                        }
                        EstimatorDetailList += "</tbody></table> ";
                        EstimatorDetailList += "<table style='width:100%; float:left; border-collapse:collapse; margin-top:30px; margin-bottom:100px; border-top:1px solid #4f90bb;'>";
                        if (agreementPdf.createEst._EstimatorPDFFilter.IncludeCost.Value)
                        {
                            EstimatorDetailList += "<tr><td style='padding-top:5px; width:90%; text-align:right; padding-right:10px; '>Total Cost</td><td style='padding-top:5px; width:10%; text-align:right; font-weight:bold;'>"
                            + currentCurrency + FormatAmount(agreementPdf.createEst.Estimator.TotalCost)
                         + "</td></tr>";
                        }
                        if (agreementPdf.createEst._EstimatorPDFFilter.IncludeProfit.Value || agreementPdf.createEst._EstimatorPDFFilter.IncludeMargin.Value)
                        {
                            EstimatorDetailList += "<tr><td style='padding-top:5px; width:90%; text-align:right; padding-right:10px; '>Total Profit/Margin</td><td style='padding-top:5px; width:10%; text-align:right; font-weight:bold;'>"
                         + currentCurrency + FormatAmount(agreementPdf.createEst.Estimator.TotalProfitAmount)
                         + "</td></tr>";
                        }

                        if (agreementPdf.createEst._EstimatorPDFFilter.IncludeOverhead.Value)
                        {
                            EstimatorDetailList += "<tr><td style='padding-top:5px; width:90%; text-align:right; padding-right:10px; '>Total Overhead</td><td style='padding-top:5px; width:10%; text-align:right; font-weight:bold;'>"
                         + currentCurrency + FormatAmount(agreementPdf.createEst.Estimator.TotalOverheadCostAmount)
                         + "</td></tr>";
                        }
                        if (!agreementPdf.createEst._EstimatorPDFFilter.GroupedbyLabor.Value && !agreementPdf.createEst._EstimatorPDFFilter.GroupedbyMaterial.Value
                                && !agreementPdf.createEst._EstimatorPDFFilter.WithoutPricing.Value)
                        {
                            EstimatorDetailList += "<tr><td style='padding-top:5px; width:90%; text-align:right; padding-right:10px; '>Sub Total</td><td style='padding-top:5px; width:10%; text-align:right; font-weight:bold;'>"
                         + currentCurrency + FormatAmount(agreementPdf.createEst.Estimator.TotalPrice)
                         + "</td></tr>";
                            EstimatorDetailList += "<tr><td style='padding-top:5px; width:90%; text-align:right; padding-right:10px; '>Total Tax</td><td style='padding-top:5px; width:10%; text-align:right; font-weight:bold;'>"
                             + currentCurrency + FormatAmount(agreementPdf.createEst.Estimator.TaxAmount)
                             + "</td></tr>";
                            EstimatorDetailList += "<tr><td style='padding-top:5px; width:90%; text-align:right; padding-right:10px; '>Total Amount</td><td style='padding-top:5px; width:10%; text-align:right; font-weight:bold;'>"
                             + currentCurrency + FormatAmount(agreementPdf.createEst.Estimator.TotalPrice + agreementPdf.createEst.Estimator.TaxAmount)
                             + "</td></tr>";
                        }
                        EstimatorDetailList += "</table>";


                    }
                    templateVars.Add("EstimatorDetailList", EstimatorDetailList);
                    if (agreementPdf.createEst.estimatorServices != null && agreementPdf.createEst._EstimatorPDFFilter.IncludeService.Value)
                    {
                        EstimatorServiceList = "<div style='page-break-after: always; display: block; clear: both; '></div>"
                                       + "<table style='width:100%; float:left; border-collapse:collapse; margin-bottom:20px; '>"
                                             + "<thead>"
                                             + "<tr style='background-color:#4f90bb; color:white;width:100%; border:1px solid #4f90bb;' >"
                                             + "<th style='width:65%;text-align:left;padding:5px 0px 5px 20px; border-right:1px solid #fff;'>SERVICE</th>"
                                             + "<th style='width:5%; padding:5px 0px; border-right:1px solid #fff; text-align:center;'>QTY</th>"
                                             + "<th style='width:15%; padding:5px 0px; border-right:1px solid #fff; text-align:center;'>UNIT PRICE</th>"
                                             + "<th style='width:15%; padding:5px 0px; border-right:1px solid #fff; text-align:center;'>TOTAL PRICE</th>"
                                             + "</tr></thead >"
                                             + "<tbody>";
                        foreach (var item in agreementPdf.createEst.estimatorServices)
                        {
                            EstimatorSerSERVICE = "<td style='padding:5px 0px 5px 20px;'><strong><label>" + item.EquipmentName + "</label></strong></td>";
                            EstimatorSerQTY = "<td style='padding:5px; text-align:center; '><strong><label>" + item.Quantity + "</label></strong></td>";
                            EstimatorSerUNITPRICE = "<td style='padding:5px; text-align:center; '><strong><label>" + currentCurrency + FormatAmount(item.UnitPrice) + "</label></strong></td>";
                            EstimatorSerTOTALPRICE = "<td style='padding:5px; text-align:center; '><strong><label>" + currentCurrency + FormatAmount(item.Amount) + "</label></strong></td>";

                            EstimatorServiceList += "<tr style='border-bottom:1px solid #ccc;'>" + EstimatorSerSERVICE + EstimatorSerQTY + EstimatorSerUNITPRICE + EstimatorSerTOTALPRICE + "</tr>";
                        }
                        EstimatorServiceList += "</tbody></table>";
                        EstimatorServiceList += "<table style='width:100%; float:left; border-collapse:collapse; margin-top:30px; border-top:1px solid #4f90bb;'>";
                        EstimatorServiceList += "<tr><td style='padding-top:5px; width:90%; text-align:right; padding-right:10px;'> Sub Total </td><td style='padding-top:5px; width:10%; text-align:right; font-weight:bold; '> "
                                                         + currentCurrency + FormatAmount(agreementPdf.createEst.Estimator.ServiceTotalAmount)
                                         + "</td></tr>";
                        if (agreementPdf.createEst.Estimator.ShowServicePlan.HasValue && agreementPdf.createEst.Estimator.ShowServicePlan.Value)
                        {
                            EstimatorServiceList += "<tr><td style='padding-top:5px; width:90%; text-align:right; padding-right:10px;'>" + agreementPdf.createEst.Estimator.ServicePlanTypeName + "</td><td style='padding-top:5px; width:10%; text-align:right; font-weight:bold; '> "
                                                         + currentCurrency + FormatAmount(agreementPdf.createEst.Estimator.ServicePlanAmount)
                                         + "</td></tr>";
                            EstimatorServiceList += "<tr><td style='padding-top:5px; width:90%; text-align:right; padding-right:10px;'> Sub Total </td><td style='padding-top:5px; width:10%; text-align:right; font-weight:bold; '> "
                                                             + currentCurrency + FormatAmount(agreementPdf.createEst.Estimator.ServiceTotalAmount + agreementPdf.createEst.Estimator.ServicePlanAmount)
                                             + "</td></tr>";
                        }

                        EstimatorServiceList += "<tr><td style='padding-top:5px; width:90%; text-align:right; padding-right:10px;'>Total Tax</td><td style='padding-top:5px; width:10%; text-align:right; font-weight:bold; '> "
                                                         + currentCurrency + FormatAmount(agreementPdf.createEst.Estimator.ServiceTaxAmount)
                                         + "</td></tr>";
                        EstimatorServiceList += "<tr><td style='padding-top:5px; width:90%; text-align:right; padding-right:10px;'>Total Amount</td><td style='padding-top:5px; width:10%; text-align:right; font-weight:bold;'> "
                                                         + currentCurrency + FormatAmount(Math.Round(agreementPdf.createEst.Estimator.ServiceTotalAmount.Value, 2) + Math.Round(agreementPdf.createEst.Estimator.ServiceTaxAmount.Value, 2) + Math.Round(agreementPdf.createEst.Estimator.ServicePlanAmount.Value, 2))
                                         + "</td></tr>";
                        EstimatorServiceList += "</table>";
                    }
                    templateVars.Add("EstimatorServiceList", EstimatorServiceList);
                    string fullname = agreementPdf.FirstName + " " + agreementPdf.MiddleName + " " + agreementPdf.LastName;
                    String result = "";

                    // Traverse the string.  
                    bool v = true;
                    for (int i = 0; i < fullname.Length; i++)
                    {
                        // If it is space, set v as true.  
                        if (fullname[i] == ' ')
                        {
                            v = true;
                        }

                        // Else check if v is true or not.  
                        // If true, copy character in output  
                        // string and set v as false.  
                        else if (fullname[i] != ' ' && v == true)
                        {
                            result += (fullname[i]);
                            v = false;
                        }
                    }
                    templateVars.Add("CustomerInitName", result);
                    if (agreementPdf.createEst.Estimator.ServicePlanType == "0.0095")
                    {
                        PremiumPlan = "<table style='width:100%; float:left; border-collapse:collapse; '>" +
                            "<tr>" +
                            "<td style='padding:20px 0px 0px 0px; font-size:18px; font-weight:bold; text-transform:uppercase;' >" +
                            "PREMIUM SERVICE PLAN Terms" +
                            "<span style='font-size:12px; margin-left:10px;'> " +
                            "Adds $11.00 per month" +
                            "</span>" +
                            "</td>" +
                            "</tr>" +
                            "<tr>" +
                            "<td>(24x7, 365 days)" +
                            "<br/>" +
                            "EST Premium Service Plan covers labor and equipment costs. " +
                            "The service plan can cover all types of protection systems including intrusion alarms, fire alarms, " +
                            "camera systems and access control systems.This plan covers normal 'wear and tear', repair or replacement." +
                            "Repair or replacement of equipment damaged by the customer, acts of God or vandalism is not covered. " +
                            "Includes access to the EST Telephone Support(24x7)." +
                            "</td>" +
                            "</tr>" +
                            "</table> ";
                    }
                    if (agreementPdf.createEst.Estimator.ServicePlanType == "0.005")
                    {
                        StandardPlan = "<table style='width:100%; float:left; border-collapse: collapse; '>" +
                            "<tr>" +
                            "<td style ='padding:20px 0px 0px 0px; font-size:18px; font-weight:bold; text-transform:uppercase;' >" +
                            "Standard service plan terms: " +
                            "<span style='font-size:12px; margin-left:10px;'> " +
                            "Price as listed" +
                            "</span> " +
                            "</td>" +
                            "</tr>" +
                            "<tr>" +
                            "<td>" +
                            "(Monday - Friday, 8am - 4pm) EST Standard Service Plan covers labor and equipment cost during normal business hours." +
                            "The service plan can cover all types of protection systems including intrusion alarms, fire alarms, camera systems and access control systems including intrusion alarms, fire alarms, " +
                            "camera systems and access control systems." +
                            "This plan covers normal 'wear and tear', repair or replacement.Repair or replacement of equipment damaged by the customer, acts of God or vandalism is not covered." +
                            "Service labor rates for after hours work are not included and are based on current EST service labor rate schedule." +
                            "Includes access to the EST Telephone Support(24x7)." +
                            "</td>" +
                            "</tr>" +
                            "</table>";
                    }
                    templateVars.Add("PremiumPlan", PremiumPlan);
                    templateVars.Add("StandardPlan", StandardPlan);
                    templateVars.Add("EstimatorContractTerm", agreementPdf.createEst.EstimatorContractTerm);
                    templateVars.Add("EstimatorInstallTotal", currentCurrency + FormatAmount(agreementPdf.createEst.Estimator.TotalPrice + agreementPdf.createEst.Estimator.TaxAmount));
                    templateVars.Add("EstimatorMonthlyTotal", currentCurrency + FormatAmount(Math.Round(agreementPdf.createEst.Estimator.ServiceTotalAmount.Value, 2) + Math.Round(agreementPdf.createEst.Estimator.ServiceTaxAmount.Value, 2) + Math.Round(agreementPdf.createEst.Estimator.ServicePlanAmount.Value, 2)) + "(Included Tax)");
                }
                #endregion

                #region payment
                if (agreementPdf.PaymentDetails != null)
                {
                    if (agreementPdf.PaymentDetails.Type == "Credit Card")
                    {
                        var tr1 = "<tr style='height:25px;'>" +
                                            "<td style='font-weight:bold; width:140px; padding-left:10px;'>" +
                                                "Credit Card Type: " +
                                            "</td>" +
                                            "<td>" + agreementPdf.PaymentDetails.CardType + "</td>" +
                                            "<td style='font-weight:bold; width:140px; padding-left:10px;'>" +
                                                "Payment Type: " +
                                            "</td>" +
                                            "<td></td>" +
                                        "</tr>";
                        var tr2 = "<tr style='height: 25px; '>" +
                                               "<td style = 'font-weight:bold; padding-left:10px; ' >" +
                                                     "Account" +
                                                "</td>" +
                                                "<td style = 'border-bottom:1px solid #000;' >" + agreementPdf.PaymentDetails.CardNumber + "</td>" +
                                            "<td style = 'padding-left:10px; border-bottom: 1px solid #fff;' >" +
                                                 "Exp" +
                                             "</td >" +
                                             "<td style = 'border-bottom:1px solid #000;' >" + agreementPdf.PaymentDetails.CardExpireDate + "</td>" +
                                        "</tr >";
                        var tr3 = "<tr style='height: 25px; '>" +
                                               "<td style = 'font-weight:bold; padding-left:10px; ' >" +
                                                    " Name on Card" +
                                            "</td >" +
                                           " <td colspan = '3' style = 'border-bottom:1px solid #000;' >" + agreementPdf.PaymentDetails.AccountName + "</td>" +
                                           "</tr >";
                        var tr4 = "<tr style='height: 25px; '>" +
                                               "<td style = 'font-weight:bold; padding-left:10px; ' >" +
                                                     "Address on Card" +
                                            "</td >" +
                                            "<td colspan = '3' style = 'border-bottom:1px solid #000;' ></td>" +
                                        "</tr > ";
                        var CreditCardTable = "<table style='border-collapse:collapse; width:100%; float:left; table-layout:fixed; border: 2px solid #000; font-size:13px; border-bottom:0px;'>" + tr1 + tr2 + tr3 + tr4 + "</table>";
                        templateVars.Add("CreditCardTable", CreditCardTable);
                    }
                    if (agreementPdf.PaymentDetails.Type == "ACH")
                    {
                        var tr1 = "<tr style='height: 25px; '>" +
                                              "<td style = 'font-weight:bold; width:140px; padding-left:10px; ' >" +
                                                    "Bank Account Type:" +
                                            "</td>" +
                                            "<td>" + agreementPdf.PaymentDetails.BankAccountType + "</td>" +
                                        "</tr> ";
                        var tr2 = "<tr style='height: 25px; '>" +
                                              "<td style = 'font-weight:bold; padding-left:10px; ' >" +
                                                    "Bank Name" +
                                               "</td>" +
                                               "<td style = 'border-bottom:1px solid #000;' ></td>" +
                                        "</tr> ";
                        var tr3 = "<tr style='height: 25px; '>" +
                                              "<td style = 'font-weight:bold; padding-left:10px; ' >" +
                                                    "Account" +
                                               "</td >" +
                                               "<td style = 'border-bottom:1px solid #000;' >" + agreementPdf.PaymentDetails.AcountNo + "</td>" +
                                        "</tr>";
                        var tr4 = "<tr style='height: 25px; '>" +
                                              "<td style = 'font-weight:bold; padding-left:10px;' >" +
                                                   "Routing" +
                                               "</td>" +
                                               "<td style = 'border-bottom:1px solid #000;' >" + agreementPdf.PaymentDetails.RoutingNo + "</td>" +
                                        "</tr> ";
                        var BankAccountTable = "<table style='border-collapse:collapse; width:100%; float:left; table-layout:fixed;border:2px solid #000; font-size:13px; border-bottom:0px;'>" + tr1 + tr2 + tr3 + tr4 + "</table>";
                        templateVars.Add("BankAccountTable", BankAccountTable);
                    }
                }
                #endregion

                #region NonConformingFee
                var NonConfirmingFeeDivRab = "";
                var NonConfirmingFeeDivDFW = "";
                if (agreementPdf.NonConfirmingFee > 0)
                {
                    NonConfirmingFeeDivRab = "<div style='width:80%;float:left;padding-left:10px;padding-top:5px'><span>NON CONFORMING FEE</span></div>" +
                                         "<div style='width:18%;float:left;text-align:right;padding-top:5px'>{0}</div>";
                    NonConfirmingFeeDivRab = string.Format(NonConfirmingFeeDivRab, currentCurrency + FormatAmount(agreementPdf.NonConfirmingFee));

                    NonConfirmingFeeDivDFW = "<tr style='height:25px;'><td valign ='middle' style='font-weight:bold; border:1px solid #000; text-align:right; padding-right:5px; background-color:#f3f3f3;' > +Non Conforming Fee </td>" +
                                            "<td style='border:1px solid #000;text-align: right;padding-right: 10px;'>{0}</td></tr>";
                    NonConfirmingFeeDivDFW = string.Format(NonConfirmingFeeDivDFW, currentCurrency + FormatAmount(agreementPdf.NonConfirmingFee));
                    NonConfirmingFeeDivCommFire = "<tr style='border-bottom:1px solid #ccc'><td style=\"padding:5px 0px 5px 20px\"><strong><label> + Non Conforming Fee </label></strong></td><td style=\"padding:5px;text-align:center\"><strong><label>{0}</label></strong></td></tr>";
                    NonConfirmingFeeDivCommFire = string.Format(NonConfirmingFeeDivCommFire, currentCurrency + FormatAmount(agreementPdf.NonConfirmingFee));
                }
                templateVars.Add("NonConfirmingFeeDivRab", NonConfirmingFeeDivRab);
                templateVars.Add("NonConfirmingFeeDivDFW", NonConfirmingFeeDivDFW);
                templateVars.Add("NonConfirmingFeeDivCommFire", NonConfirmingFeeDivCommFire);

                #endregion
                #region Advance Monitoring Service Fee
                var AdvanceMonitoringFeeRab = "";
                var AdvanceMonitoringFeeDFW = "";
                if (agreementPdf.AdvanceServiceFeeTotal > 0)
                {
                    AdvanceMonitoringFeeRab = "<div style='width:80%;float:left;padding-left:10px;padding-top:5px'><span>ADVANCE MONITORING  FEE</span></div>" +
                                         "<div style='width:18%;float:left;text-align:right;padding-top:5px'>{0}</div>";
                    AdvanceMonitoringFeeRab = string.Format(AdvanceMonitoringFeeRab, currentCurrency + FormatAmount(AdvanceMonitoringFee));

                    AdvanceMonitoringFeeDFW = "<tr style='height:25px;'><td valign ='middle' style='font-weight:bold; border:1px solid #000; text-align:right; padding-right:5px; background-color:#f3f3f3;' > +Advance Monitoring Fee </td>" +
                                            "<td style='border:1px solid #000;text-align: right;padding-right: 10px;'>{0}</td></tr>";
                    AdvanceMonitoringFeeDFW = string.Format(AdvanceMonitoringFeeDFW, currentCurrency + FormatAmount(AdvanceMonitoringFee));
                }
                templateVars.Add("AdvanceMonitoringFeeRab", AdvanceMonitoringFeeRab);
                templateVars.Add("AdvanceMonitoringFeeDFW", AdvanceMonitoringFeeDFW);

                #endregion
                EmailParser parser = null;
                if (templateid.HasValue && templateid.Value > 0)
                {
                    CustomerAgreementTemplate agreementTemplate = _CustomerAgreementTemplateDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and Id = {1}", agreementPdf.CompanyId, templateid.Value)).FirstOrDefault();
                    parser = new EmailParser(agreementTemplate.BodyContent, templateVars, false);
                    MailMessage message = new MailMessage();
                    message.Body = parser.Parse();
                    Body = message.Body;
                }
                else if (agreementPdf.FirstPage == true)
                {
                    EmailTemplate agreementTemplate = _EmailTemplateDataAccess.GetEmailTemplateByKeyAndCompanyId(agreementPdf.CompanyId, EmailTemplateKey.AgreementFirstPage.SmartAgreementFirstPage);
                    parser = new EmailParser(HttpContext.Current.Server.MapPath(agreementTemplate.BodyFile), templateVars, true);
                    MailMessage message = new MailMessage();
                    message.Body = parser.Parse();
                    Body = message.Body;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(agreementPdf.ContractType) && agreementPdf.ContractType.ToLower() == "commercial")
                    {
                        string TemplateKeyName = EmailTemplateKey.SmartAgreementCommercial.SmartAgreementCommercialRMR;
                        if (IsBBBConflict)
                        {
                            if (!string.IsNullOrWhiteSpace(agreementPdf.EstimatorId))
                            {
                                TemplateKeyName = EmailTemplateKey.SmartAgreement.SmartAgreementDFWSignedwithSKU;
                            }
                            else
                            {
                                TemplateKeyName = EmailTemplateKey.SmartAgreementCommercial.SmartAgreementCommercialSignedRMR;
                            }
                            
                        }
                        else if (!string.IsNullOrWhiteSpace(agreementPdf.EstimatorId))
                        {
                            TemplateKeyName = EmailTemplateKey.SmartAgreement.SmartAgreementDFWSignedwithSKU;
                        }
                        EmailTemplate agreementTemplate = _EmailTemplateDataAccess.GetEmailTemplateByKeyAndCompanyId(agreementPdf.CompanyId, TemplateKeyName);
                        parser = new EmailParser(HttpContext.Current.Server.MapPath(agreementTemplate.BodyFile), templateVars, true);
                        MailMessage message = new MailMessage();
                        message.Body = parser.Parse();
                        Body = message.Body;
                    }
                    else if (!string.IsNullOrWhiteSpace(agreementPdf.ContractType) && agreementPdf.ContractType.ToLower() == "commercialfire")
                    {
                        string TemplateKeyName = EmailTemplateKey.SmartAgreementCommercialFire.SmartAgreementCommercialFireRMR;
                        if (IsBBBConflict)
                        {
                            if (!string.IsNullOrWhiteSpace(agreementPdf.EstimatorId))
                            {
                                TemplateKeyName = EmailTemplateKey.SmartAgreement.SmartAgreementDFWSignedwithSKU;
                            }
                            else
                            {
                                TemplateKeyName = EmailTemplateKey.SmartAgreementCommercialFire.SmartAgreementCommercialFireSignedRMR;
                            } 
                        }
                        else if (!string.IsNullOrWhiteSpace(agreementPdf.EstimatorId))
                        {
                            TemplateKeyName = EmailTemplateKey.SmartAgreement.SmartAgreementDFWSignedwithSKU;
                        }
                        EmailTemplate agreementTemplate = _EmailTemplateDataAccess.GetEmailTemplateByKeyAndCompanyId(agreementPdf.CompanyId, TemplateKeyName);
                        parser = new EmailParser(HttpContext.Current.Server.MapPath(agreementTemplate.BodyFile), templateVars, true);
                        MailMessage message = new MailMessage();
                        message.Body = parser.Parse();
                        Body = message.Body;
                    }
                    else
                    {
                        string TemplateKeyName = EmailTemplateKey.SmartAgreement.SmartAgreementRMR;
                        if (IsBBBConflict)
                        {
                            if(!string.IsNullOrWhiteSpace(agreementPdf.EstimatorId))
                            {
                                TemplateKeyName = EmailTemplateKey.SmartAgreement.SmartAgreementDFWSignedwithSKU;
                            }
                            else
                            {
                                TemplateKeyName = EmailTemplateKey.SmartAgreement.SmartAgreementSignedRMR;
                            }
                            
                        }
                        else if(!string.IsNullOrWhiteSpace(agreementPdf.EstimatorId))
                        {
                            TemplateKeyName = EmailTemplateKey.SmartAgreement.SmartAgreementDFWSignedwithSKU;
                        }
                        EmailTemplate agreementTemplate = _EmailTemplateDataAccess.GetEmailTemplateByKeyAndCompanyId(agreementPdf.CompanyId, TemplateKeyName);
                        if(agreementTemplate != null)
						{
                            parser = new EmailParser(HttpContext.Current.Server.MapPath(agreementTemplate.BodyFile), templateVars, true);
                        }
						else
						{
                            parser = new EmailParser();
                        }
                        
                        MailMessage message = new MailMessage();
                        message.Body = parser.Parse();
                        Body = message.Body;
                    }

                }
            }
            catch (Exception ex)
            {
                //Logger.AddElmah(ex);
            }

            return Body;
        }


        public string MakeCancellationAgreementPdf(CustomerCancellationQueueAggrement agreementPdf)
        {
            string Body = "";
            string CancellationReason = "";
            string CancellationNote = "";
            foreach (var item in agreementPdf.CancellationReason)
            {
                if (agreementPdf.CustomerCancellationReasonList.FirstOrDefault(s => s.CancellationReason.Contains(item.Value)) != null)
                {
                    CancellationReason += "  <tr style=''><td valign='middle' style='box-sizing:border-box; padding-top:30px;'><input name='CanReason' value='1' type='checkbox' checked/>" + item.Text + "</td></tr>";
                }
                else
                {
                    CancellationReason += "  <tr style=''><td valign='middle' style='box-sizing:border-box; padding-top:30px;'><input name='CanReason' value='1' type='checkbox'/>" + item.Text + "</td></tr>";
                }
            }
            if (!string.IsNullOrEmpty(agreementPdf.Note))
            {
                CancellationNote = "<tr ><td valign = 'middle' style = 'box-sizing:border-box;' >" + agreementPdf.Note + "</ td ></ tr >";
            }
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("CompanyLogo", agreementPdf.CompanyLogo);
                templateVars.Add("CompanyName", agreementPdf.CompanyName);
                templateVars.Add("CompanyStreet", agreementPdf.CompanyStreet);
                templateVars.Add("CompanyState", agreementPdf.CompanyState);
                templateVars.Add("CompanyCity", agreementPdf.CompanyCity);
                templateVars.Add("CompanyWebsite", agreementPdf.CompanyWebsite);
                templateVars.Add("CompanyZipcode", agreementPdf.CompanyZipcode);
                templateVars.Add("CompanyPhone", agreementPdf.CompanyPhone);
                templateVars.Add("CompanyEmail", agreementPdf.CompanyEmail);
                templateVars.Add("CancellationReason", CancellationReason);
                templateVars.Add("CancellationNote", CancellationNote);
                templateVars.Add("CustomerSignature", agreementPdf.SignedImg);
                templateVars.Add("CustomerName", agreementPdf.CustomerName);
                templateVars.Add("CustomerAddress", agreementPdf.CustomerAddress);
                templateVars.Add("InstallAddress", agreementPdf.CustomerAddress);
                templateVars.Add("RemainingBalance", agreementPdf.RemainingBalance);
                templateVars.Add("TermDate", agreementPdf.CancellationDate.ToString("MM/dd/yy"));

                EmailParser parser = null;
                EmailTemplate agreementTemplate = _EmailTemplateDataAccess.GetEmailTemplateByKeyAndCompanyId(agreementPdf.CompanyId.ToString(), EmailTemplateKey.CustomerCancellationAggrement.CancellationAggrement);


                parser = new EmailParser(HttpContext.Current.Server.MapPath(agreementTemplate.BodyFile), templateVars, true);
                MailMessage message = new MailMessage();
                message.Body = parser.Parse();
                Body = message.Body;



            }
            catch (Exception ex)
            {
                //Logger.AddElmah(ex);
            }

            return Body;
        }
        public string MakeBlankAgreementPdf(InstallationAgreementModel agreementPdf)
        {
            string Body = "";
            string ContactName = "";
            string ContactPhone = "";
            string CenterSpace = "";
            string PhoneType = "";
            string ContactRelationship = "";
            string HasKey = "";
            string EmergencyContactList = "";
            string EquipmentName = "";
            string ServiceName = "";
            string MonthlyRate = "";
            string DiscountRate = "";
            string Total = "";
            string UnitPrice = "";
            string EquipmentQuantity = "";
            string DiscountUnitPrice = "";
            string TotalEquipment = "";
            string EquipmentList = "";
            string EquipmentListRab = "";
            string EquipmentListDFW = "";
            string ServiceList = "";
            string ServiceListRab = "";
            string ServiceListDFW = "";
            string SmartPackageEquipmentServiceList = "";
            string ListAgreementAnswer = "";
            string divAnsval = "";
            string CustomerAgreement = "";
            string CustomerAgreementTable = "";
            string currentCurrency = "";
            string Upfronaddon = "";
            string Service = "";
            string Points = "";
            string UpFront = "";
            string Qty = "";
            string TotalDfw = "";
            string MonthlyService = "";
            string Type = "";
            string Monthly = "";
            if (agreementPdf.CurrentCurrency != null)
            {
                currentCurrency = agreementPdf.CurrentCurrency;
            }
            else
            {
                currentCurrency = "$";
            }
            try
            {
                Hashtable templateVars = new Hashtable();
                templateVars.Add("CompanyName", agreementPdf.CompanyName);
                templateVars.Add("CompanyStreet", agreementPdf.CompanyStreet);
                templateVars.Add("CompanySate", agreementPdf.CompanySate);
                templateVars.Add("CompanyWebsite", agreementPdf.CompanyWebsite);
                templateVars.Add("CompanyLogo", agreementPdf.CompanyLogo);

                //EmergencyContactList

                //foreach (var item in agreementPdf.EmergencyContactList)
                //{
                ContactName = "<div style='width: 25%; float:left;'><span>&nbsp;</span></div>";
                ContactPhone = " <div style='width: 15%; float:left; text-align:center'><span> &nbsp;</span></div>";
                CenterSpace = " <div style='width: 15%; float:left; text-align:center'><span> &nbsp;</span></div>";
                PhoneType = "<div style='width: 15%; float:left; text-align:center'><span>&nbsp;</span></div>";
                ContactRelationship = "<div style='width: 15%; float:left; text-align:center'><span>&nbsp;</span></div>";
                HasKey = " <div style='width: 15%; float:left; text-align:right'> <span>&nbsp;</span></div>";

                EmergencyContactList += ContactName + ContactPhone + CenterSpace + PhoneType + ContactRelationship + HasKey;

                ContactName = "<div style='width: 25%; float:left;'><span>&nbsp;</span></div>";
                ContactPhone = " <div style='width: 15%; float:left; text-align:center'><span> &nbsp;</span></div>";
                CenterSpace = " <div style='width: 15%; float:left; text-align:center'><span> &nbsp;</span></div>";
                PhoneType = "<div style='width: 15%; float:left; text-align:center'><span>&nbsp;</span></div>";
                ContactRelationship = "<div style='width: 15%; float:left; text-align:center'><span>&nbsp;</span></div>";
                HasKey = " <div style='width: 15%; float:left; text-align:right'> <span>&nbsp;</span></div>";

                EmergencyContactList += ContactName + ContactPhone + CenterSpace + PhoneType + ContactRelationship + HasKey;

                ContactName = "<div style='width: 25%; float:left;'><span>&nbsp;</span></div>";
                ContactPhone = " <div style='width: 15%; float:left; text-align:center'><span> &nbsp;</span></div>";
                CenterSpace = " <div style='width: 15%; float:left; text-align:center'><span> &nbsp;</span></div>";
                PhoneType = "<div style='width: 15%; float:left; text-align:center'><span>&nbsp;</span></div>";
                ContactRelationship = "<div style='width: 15%; float:left; text-align:center'><span>&nbsp;</span></div>";
                HasKey = " <div style='width: 15%; float:left; text-align:right'> <span>&nbsp;</span></div>";

                EmergencyContactList += ContactName + ContactPhone + CenterSpace + PhoneType + ContactRelationship + HasKey;
                //}
                //SmartPackageEquipmentServiceList
                //foreach (var item in agreementPdf.SmartPackageEquipmentServiceList)
                //{
                //SmartPackageEquipmentServiceList += "<div style = 'width:50%; float:left; text-align:left;box-sizing:border-box;'></div>";
                //SmartPackageEquipmentServiceList += "<div style = 'width:50%; float:left; text-align:left;box-sizing:border-box;'></div>";
                //SmartPackageEquipmentServiceList += "<div style = 'width:50%; float:left; text-align:left;box-sizing:border-box;'></div>";
                //}
                //templateVars.Add("SmartPackageEquipmentServiceList", SmartPackageEquipmentServiceList);

                //EquipmentListDFW
                //foreach (var item in agreementPdf.EquipmentList)
                //{
                EquipmentName = "  <div style = 'width:39%;float:left;padding-left:10px;'><span> &nbsp; </span></div> ";
                EquipmentQuantity = " <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;</span></div>";
                UnitPrice = " <div style = 'width:15%;float:left;text-align:center'><span> &nbsp; </span></div>";
                DiscountUnitPrice = " <div style = 'width:15%;float:left;text-align:center'><span> </span></div>";
                TotalEquipment = "  <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;</span></div>";
                EquipmentList += " <div style='padding-top:5px;'>" + EquipmentName + EquipmentQuantity + UnitPrice + DiscountUnitPrice + TotalEquipment + "</div>";

                Upfronaddon = "<td style='text - align:right; font - weight:bold; border: 1px solid #000; padding:0px 10px;'></td>";
                Service = "<td style='text-align:center; border:1px solid #000;'></td>";
                Points = "<td style='text-align:center; border:1px solid #000;'></td>";
                UpFront = "<td style='text-align:center; font-weight:bold; border:1px solid #000;'></td>";
                Qty = "<td style='text-align:center; border:1px solid #000;'></td>";
                TotalDfw = "<td style='text-align:center; border:1px solid #000;'></td>";
                EquipmentListDFW += "<tr style='background - color:#f2f2f2; color:#000; height: 30px; border:2px solid #000;'>" + Upfronaddon + Service + Points + UpFront + Qty + TotalDfw + "</tr>";

                EquipmentName = "  <div style = 'width:39%;float:left;padding-left:10px;'><span> &nbsp; </span></div> ";
                EquipmentQuantity = " <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;</span></div>";
                UnitPrice = " <div style = 'width:15%;float:left;text-align:center'><span> &nbsp; </span></div>";
                DiscountUnitPrice = " <div style = 'width:15%;float:left;text-align:center'><span> </span></div>";
                TotalEquipment = "  <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;</span></div>";
                EquipmentList += " <div style='padding-top:5px;'>" + EquipmentName + EquipmentQuantity + UnitPrice + DiscountUnitPrice + TotalEquipment + "</div>";

                Upfronaddon = "<td style='text - align:right; font - weight:bold; border: 1px solid #000; padding:0px 10px;'></td>";
                Service = "<td style='text-align:center; border:1px solid #000;'></td>";
                Points = "<td style='text-align:center; border:1px solid #000;'></td>";
                UpFront = "<td style='text-align:center; font-weight:bold; border:1px solid #000;'></td>";
                Qty = "<td style='text-align:center; border:1px solid #000;'></td>";
                TotalDfw = "<td style='text-align:center; border:1px solid #000;'></td>";
                EquipmentListDFW += "<tr style='background - color:#f2f2f2; color:#000; height: 30px; border:2px solid #000;'>" + Upfronaddon + Service + Points + UpFront + Qty + TotalDfw + "</tr>";

                EquipmentName = "  <div style = 'width:39%;float:left;padding-left:10px;'><span> &nbsp; </span></div> ";
                EquipmentQuantity = " <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;</span></div>";
                UnitPrice = " <div style = 'width:15%;float:left;text-align:center'><span> &nbsp; </span></div>";
                DiscountUnitPrice = " <div style = 'width:15%;float:left;text-align:center'><span> </span></div>";
                TotalEquipment = "  <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;</span></div>";
                EquipmentList += " <div style='padding-top:5px;'>" + EquipmentName + EquipmentQuantity + UnitPrice + DiscountUnitPrice + TotalEquipment + "</div>";

                Upfronaddon = "<td style='text - align:right; font - weight:bold; border: 1px solid #000; padding:0px 10px;'></td>";
                Service = "<td style='text-align:center; border:1px solid #000;'></td>";
                Points = "<td style='text-align:center; border:1px solid #000;'></td>";
                UpFront = "<td style='text-align:center; font-weight:bold; border:1px solid #000;'></td>";
                Qty = "<td style='text-align:center; border:1px solid #000;'></td>";
                TotalDfw = "<td style='text-align:center; border:1px solid #000;'></td>";
                EquipmentListDFW += "<tr style='background - color:#f2f2f2; color:#000; height: 30px; border:2px solid #000;'>" + Upfronaddon + Service + Points + UpFront + Qty + TotalDfw + "</tr>";


                //}

                //EquipmentListRab
                //foreach (var item in agreementPdf.EquipmentList)
                //{
                EquipmentName = "  <div style = 'width:30%;float:left;padding-left:10px;'><span> &nbsp; </span></div> ";
                EquipmentQuantity = " <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;</span></div>";
                UnitPrice = " <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + currentCurrency + " </span></div>";
                DiscountUnitPrice = " <div style = 'width:20%;float:left;text-align:center'><span>" + currentCurrency + " </span></div>";
                TotalEquipment = "  <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + currentCurrency + " </span></div>";
                EquipmentListRab += " <div style='padding-top:5px;'>" + EquipmentName + EquipmentQuantity + UnitPrice + DiscountUnitPrice + TotalEquipment + "</div>";

                EquipmentName = "  <div style = 'width:30%;float:left;padding-left:10px;'><span> &nbsp; </span></div> ";
                EquipmentQuantity = " <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;</span></div>";
                UnitPrice = " <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + currentCurrency + " </span></div>";
                DiscountUnitPrice = " <div style = 'width:20%;float:left;text-align:center'><span>" + currentCurrency + " </span></div>";
                TotalEquipment = "  <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + currentCurrency + " </span></div>";
                EquipmentListRab += " <div style='padding-top:5px;'>" + EquipmentName + EquipmentQuantity + UnitPrice + DiscountUnitPrice + TotalEquipment + "</div>";

                EquipmentName = "  <div style = 'width:30%;float:left;padding-left:10px;'><span> &nbsp; </span></div> ";
                EquipmentQuantity = " <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;</span></div>";
                UnitPrice = " <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + currentCurrency + " </span></div>";
                DiscountUnitPrice = " <div style = 'width:20%;float:left;text-align:center'><span>" + currentCurrency + " </span></div>";
                TotalEquipment = "  <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + currentCurrency + " </span></div>";
                EquipmentListRab += " <div style='padding-top:5px;'>" + EquipmentName + EquipmentQuantity + UnitPrice + DiscountUnitPrice + TotalEquipment + "</div>";
                //}

                //ServiceListDFW
                //foreach (var item in agreementPdf.ServiceList)
                //{
                ServiceName = "  <div style = 'width:54%;float:left;padding-left:10px;'><span> &nbsp;</span></div> ";
                MonthlyRate = "  <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + currentCurrency + " </span></div> ";
                DiscountRate = " <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + currentCurrency + " </span></div>";
                Total = " <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + currentCurrency + " </span></div>";
                ServiceList += " <div style='padding-top:5px;'>" + ServiceName + MonthlyRate + DiscountRate + Total + "</div>";

                MonthlyService = "<td style='text-align:right;font-weight:bold; border:1px solid #000; padding:0px 10px;'></td>";
                Type = "<td style='text-align:center; border:1px solid #000;'></td>";
                Monthly = "<td style='text-align:center; font-weight:bold; border:1px solid #000;'>" + currentCurrency + "</td>";
                Qty = "<td style='text-align:center; border:1px solid #000;'></td>";
                TotalDfw = "<td style='text-align:center; border:1px solid #000;'></td>";
                ServiceListDFW += "<tr style='background-color:#bfbfbf; color:#000; height: 30px; border:2px solid #000;'>" + MonthlyService + Type + Monthly + Qty + TotalDfw + "</tr>";

                ServiceName = "  <div style = 'width:54%;float:left;padding-left:10px;'><span> &nbsp;</span></div> ";
                MonthlyRate = "  <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + currentCurrency + " </span></div> ";
                DiscountRate = " <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + currentCurrency + " </span></div>";
                Total = " <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + currentCurrency + " </span></div>";
                ServiceList += " <div style='padding-top:5px;'>" + ServiceName + MonthlyRate + DiscountRate + Total + "</div>";

                MonthlyService = "<td style='text-align:right;font-weight:bold; border:1px solid #000; padding:0px 10px;'></td>";
                Type = "<td style='text-align:center; border:1px solid #000;'></td>";
                Monthly = "<td style='text-align:center; font-weight:bold; border:1px solid #000;'>" + currentCurrency + "</td>";
                Qty = "<td style='text-align:center; border:1px solid #000;'></td>";
                TotalDfw = "<td style='text-align:center; border:1px solid #000;'></td>";
                ServiceListDFW += "<tr style='background-color:#bfbfbf; color:#000; height: 30px; border:2px solid #000;'>" + MonthlyService + Type + Monthly + Qty + TotalDfw + "</tr>";

                ServiceName = "  <div style = 'width:54%;float:left;padding-left:10px;'><span> &nbsp;</span></div> ";
                MonthlyRate = "  <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + currentCurrency + " </span></div> ";
                DiscountRate = " <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + currentCurrency + " </span></div>";
                Total = " <div style = 'width:15%;float:left;text-align:center'><span> &nbsp;" + currentCurrency + " </span></div>";
                ServiceList += " <div style='padding-top:5px;'>" + ServiceName + MonthlyRate + DiscountRate + Total + "</div>";

                MonthlyService = "<td style='text-align:right;font-weight:bold; border:1px solid #000; padding:0px 10px;'></td>";
                Type = "<td style='text-align:center; border:1px solid #000;'></td>";
                Monthly = "<td style='text-align:center; font-weight:bold; border:1px solid #000;'>" + currentCurrency + "</td>";
                Qty = "<td style='text-align:center; border:1px solid #000;'></td>";
                TotalDfw = "<td style='text-align:center; border:1px solid #000;'></td>";
                ServiceListDFW += "<tr style='background-color:#bfbfbf; color:#000; height: 30px; border:2px solid #000;'>" + MonthlyService + Type + Monthly + Qty + TotalDfw + "</tr>";
                //}

                //ServiceListRab
                //foreach (var item in agreementPdf.ServiceList)
                //{
                ServiceName = "  <div style = 'width:35%;float:left;padding-left:10px;'><span> &nbsp;</span></div> ";
                MonthlyRate = "  <div style = 'width:20%;float:left;text-align:center'><span> &nbsp;</span></div> ";
                DiscountRate = " <div style = 'width:20%;float:left;text-align:center'><span> &nbsp;</span></div>";
                Total = " <div style = 'float:left;text-align:center'><span> &nbsp;</span></div>";
                ServiceListRab += " <div style='padding-top:5px;'>" + ServiceName + MonthlyRate + DiscountRate + Total + "</div>";

                ServiceName = "  <div style = 'width:35%;float:left;padding-left:10px;'><span> &nbsp;</span></div> ";
                MonthlyRate = "  <div style = 'width:20%;float:left;text-align:center'><span> &nbsp;</span></div> ";
                DiscountRate = " <div style = 'width:20%;float:left;text-align:center'><span> &nbsp;</span></div>";
                Total = " <div style = 'float:left;text-align:center'><span> &nbsp;</span></div>";
                ServiceListRab += " <div style='padding-top:5px;'>" + ServiceName + MonthlyRate + DiscountRate + Total + "</div>";

                ServiceName = "  <div style = 'width:35%;float:left;padding-left:10px;'><span> &nbsp;</span></div> ";
                MonthlyRate = "  <div style = 'width:20%;float:left;text-align:center'><span> &nbsp;</span></div> ";
                DiscountRate = " <div style = 'width:20%;float:left;text-align:center'><span> &nbsp;</span></div>";
                Total = " <div style = 'float:left;text-align:center'><span> &nbsp;</span></div>";
                ServiceListRab += " <div style='padding-top:5px;'>" + ServiceName + MonthlyRate + DiscountRate + Total + "</div>";

                ServiceName = "  <div style = 'width:35%;float:left;padding-left:10px;'><span> &nbsp;</span></div> ";
                MonthlyRate = "  <div style = 'width:20%;float:left;text-align:center'><span> &nbsp;</span></div> ";
                DiscountRate = " <div style = 'width:20%;float:left;text-align:center'><span> &nbsp;</span></div>";
                Total = " <div style = 'float:left;text-align:center'><span> &nbsp;</span></div>";
                ServiceListRab += " <div style='padding-top:5px;'>" + ServiceName + MonthlyRate + DiscountRate + Total + "</div>";
                //}
                //if (agreementPdf.ListAgreementAnswer != null && agreementPdf.ListAgreementAnswer.Count > 0)
                //{
                //    string ansval = "";
                //    foreach (var item in agreementPdf.ListAgreementAnswer)
                //    {
                //        if (item.QuestionId == 1)
                //        {
                //            if (item.Answer == "true")
                //            {
                //                ansval = "Yes";
                //            }
                //            else
                //            {
                //                ansval = "No";
                //            }
                //            divAnsval = "<div style='word-wrap: break-word;'>Are you the homeowner? (" + ansval + ")</div>";
                //        }
                //        if (item.QuestionId == 2)
                //        {
                //            if (item.Answer == "true")
                //            {
                //                ansval = "Yes";
                //            }
                //            else
                //            {
                //                ansval = "No";
                //            }
                //            divAnsval = "<div style='word-wrap: break-word;'>Is your home new construction? (" + ansval + ")</div>";
                //        }
                //        if (item.QuestionId == 3)
                //        {
                //            if (item.Answer == "true")
                //            {
                //                ansval = "Yes";
                //            }
                //            else
                //            {
                //                ansval = "No";
                //            }

                //            divAnsval = " <div style='word-wrap: break-word;'> Are you under any contractual agreement/ obligation with any other company for monitoring services? (" + ansval + ")</div> ";
                //        }
                //        if (item.QuestionId == 4)
                //        {
                //            if (item.Answer == "true")
                //            {
                //                ansval = "Yes";
                //            }
                //            else
                //            {
                //                ansval = "No";
                //            }
                //            divAnsval = " <div style='word-wrap: break-word;'>  I understand that the Company or any representaive of the Company cannot be responsible for cancelling any services with my current security company(if applicable). (" + ansval + ")</div> ";
                //        }
                //        if (item.QuestionId == 5)
                //        {
                //            if (item.Answer == "true")
                //            {
                //                ansval = "Yes";
                //            }
                //            else
                //            {
                //                ansval = "No";
                //            }
                //            String month = "";
                //            if (agreementPdf.SubscribedMonths == "1")
                //            {
                //                month = "<span>month</span>";
                //            }
                //            else
                //            {
                //                month = "<span>months</span>";
                //            }
                //            divAnsval = "<div style = 'word-wrap: break-word;'> I understand that I have signed an agreement to receive monitoring services for<Span style = 'text-decoration:underline;font-weight :600;'> &nbsp; &nbsp; &nbsp; &nbsp;" + agreementPdf.SubscribedMonths + " &nbsp; &nbsp; &nbsp; &nbsp;</span><span>" + month + "</span></div>";
                //        }
                //        ListAgreementAnswer += divAnsval;
                //    }
                //}
                //else
                //{
                ListAgreementAnswer = "<div style='word-wrap: break-word;'>1. Are you the homeowner?</div><div style = 'word-wrap: break-word;' > 2.Is your home new construction?</div><div style = 'word-wrap: break-word;' >3.Are you under any contractual agreement / obligation with any other company for monitoring services?</div> <div style = 'word-wrap: break-word;' > 4.I understand that the Company or any representaive of the Company cannot be responsible for cancelling any services with my current security company(if applicable).</div><div style = 'word-wrap: break-word;' >5.I understand that I have signed an agreement to receive monitoring services for<span style = 'text-decoration:underline;font-weight :600;' > &nbsp; &nbsp; &nbsp; &nbsp;" + agreementPdf.SubscribedMonths + "&nbsp;&nbsp;&nbsp;&nbsp;</span> <span> month </span></div>";
                //}
                //if (agreementPdf.CustomerAgreement != null)
                //{
                //foreach (var item in agreementPdf.CustomerAgreement)
                //{

                //if (item.Type == "LoadAgreement")
                //{
                //    CustomerAgreement = "<span> Load: </span><span style = 'font-weight:600;'>" + DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("MM/dd/yyyy") + "+ at+" + DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("hh:mm tt") + " </span><br/>";
                //}
                //if (item.Type == "SignAgreement")
                //{
                //CustomerAgreement = "<span> Sign: </span><span style = 'font-weight:600;'> at </span><br/>";
                //CustomerAgreement = "<span> Sign: </span><span style = 'font-weight:600;'> at </span><br/>";
                //CustomerAgreement = "<span> Sign: </span><span style = 'font-weight:600;'> at </span><br/>";
                ////}
                ////if (item.Type == "SubmitAgreement")
                ////{
                ////    CustomerAgreement = "<span> Submit: </span><span style = 'font-weight:600;'>" + DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("MM/dd/yyyy") + "+ at+" + DateTimeExtension.UTCToClientTime(item.AddedDate.Value).ToString("hh:mm tt") + " </span><br/>";
                ////}
                ////}
                //CustomerAgreementTable = "<table style='width:100%'><tr style='background-color:darkgray;'><th style='width:33%; text-align:center;'>IP</th><th style='width:33%; text-align:center;'>USER AGENT</th><th style='width:33%; text-align:center;'>TIMESTAMP</th> </tr><tr><td style='width:33%; text-align:center;'></td><td style='width:33%; text-align:center;'></td><td style='width:33%; text-align:center;'>" + CustomerAgreement + " </td></tr>";
                ////}
                templateVars.Add("EmergencyContactList", EmergencyContactList);
                templateVars.Add("EquipmentList", EquipmentList);
                templateVars.Add("ServiceList", ServiceList);
                templateVars.Add("EquipmentListRab", EquipmentListRab);
                templateVars.Add("EquipmentListDFW", EquipmentListDFW);
                templateVars.Add("ServiceListRab", ServiceListRab);
                templateVars.Add("ServiceListDFW", ServiceListDFW);
                templateVars.Add("ListAgreementAnswer", ListAgreementAnswer);
                //templateVars.Add("CustomerAgreementTable", CustomerAgreementTable);

                #region payment
                //if (agreementPdf.PaymentDetails.Type == "Credit Card")
                //{
                var tr1 = "<tr style='height:25px;'>" +
                                    "<td style='font-weight:bold; width:140px; padding-left:10px;'>" +
                                        "Credit Card Type: " +
                                    "</td>" +
                                    "<td></td>" +
                                    "<td style='font-weight:bold; width:140px; padding-left:10px;'>" +
                                        "Payment Type: " +
                                    "</td>" +
                                    "<td></td>" +
                                "</tr>";
                var tr2 = "<tr style='height: 25px; '>" +
                                       "<td style = 'font-weight:bold; padding-left:10px; ' >" +
                                             "Account" +
                                        "</td>" +
                                        "<td style = 'border-bottom:1px solid #000;' ></td>" +
                                    "<td style = 'padding-left:10px; border-bottom: 1px solid #fff;' >" +
                                         "Exp" +
                                     "</td >" +
                                     "<td style = 'border-bottom:1px solid #000;' ></td>" +
                                "</tr >";
                var tr3 = "<tr style='height: 25px; '>" +
                                       "<td style = 'font-weight:bold; padding-left:10px; ' >" +
                                            " Name on Card" +
                                    "</td >" +
                                   " <td colspan = '3' style = 'border-bottom:1px solid #000;' ></td>" +
                                   "</tr >";
                var tr4 = "<tr style='height: 25px; '>" +
                                       "<td style = 'font-weight:bold; padding-left:10px; ' >" +
                                             "Address on Card" +
                                    "</td >" +
                                    "<td colspan = '3' style = 'border-bottom:1px solid #000;' ></td>" +
                                "</tr > ";
                var CreditCardTable = "<table style='border-collapse:collapse; width:100%; float:left; table-layout:fixed; border: 2px solid #000; font-size:13px; border-bottom:0px;'>" + tr1 + tr2 + tr3 + tr4 + "</table>";
                templateVars.Add("CreditCardTable", CreditCardTable);
                //}
                //if (agreementPdf.PaymentDetails.Type == "ACH")
                //{
                var tr5 = "<tr style='height: 25px; '>" +
                                      "<td style = 'font-weight:bold; width:140px; padding-left:10px; ' >" +
                                            "Bank Account Type:" +
                                    "</td>" +
                                    "<td></td >" +
                                "</tr> ";
                var tr6 = "<tr style='height: 25px; '>" +
                                      "<td style = 'font-weight:bold; padding-left:10px; ' >" +
                                            "Bank Name" +
                                       "</td>" +
                                       "<td style = 'border-bottom:1px solid #000;' ></td>" +
                                "</tr> ";
                var tr7 = "<tr style='height: 25px; '>" +
                                      "<td style = 'font-weight:bold; padding-left:10px; ' >" +
                                            "Account" +
                                       "</td >" +
                                       "<td style = 'border-bottom:1px solid #000;' ></td>" +
                                "</tr>";
                var tr8 = "<tr style='height: 25px; '>" +
                                      "<td style = 'font-weight:bold; padding-left:10px;' >" +
                                           "Routing" +
                                       "</td>" +
                                       "<td style = 'border-bottom:1px solid #000;' ></td>" +
                                "</tr> ";
                var BankAccountTable = "<table style='border-collapse:collapse; width:100%; float:left; table-layout:fixed;border:2px solid #000; font-size:13px; border-bottom:0px;'>" + tr5 + tr6 + tr7 + tr8 + "</table>";
                templateVars.Add("BankAccountTable", BankAccountTable);
                //}
                #endregion
                EmailParser parser = null;
                EmailTemplate agreementTemplate = _EmailTemplateDataAccess.GetEmailTemplateByKeyAndCompanyId(agreementPdf.CompanyId, EmailTemplateKey.BlankAgreementTemplates.BlankAgreement);


                parser = new EmailParser(HttpContext.Current.Server.MapPath(agreementTemplate.BodyFile), templateVars, true);
                MailMessage message = new MailMessage();
                message.Body = parser.Parse();
                Body = message.Body;
            }
            catch (Exception ex)
            {
                //Logger.AddElmah(ex);
            }

            return Body;
        }

        public string MakeCodeSafetyDocument(Ticket ticket)
        {
            string Body = "";
            string accountname = "";
            try
            {
                Hashtable templateVars = new Hashtable();
                if (ticket != null)
                {
                    var objcus = _CustomerDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", ticket.CustomerId)).FirstOrDefault();
                    if (objcus != null)
                    {
                        if (!string.IsNullOrWhiteSpace(objcus.DBA))
                        {
                            accountname = objcus.DBA;
                        }
                        else if (!string.IsNullOrWhiteSpace(objcus.BusinessName))
                        {
                            accountname = objcus.BusinessName;
                        }
                        else
                        {
                            accountname = objcus.FirstName + " " + objcus.LastName;
                        }
                        templateVars.Add("AccountName", accountname);
                        templateVars.Add("AccountEmail", objcus.EmailAddress);
                        templateVars.Add("Street", objcus.Street);
                        templateVars.Add("City", objcus.City);
                        templateVars.Add("State", objcus.State);
                        templateVars.Add("Zipcode", objcus.ZipCode);
                        templateVars.Add("Passcode", objcus.Passcode);
                    }
                }
                EmailParser parser = null;
                EmailTemplate agreementTemplate = _EmailTemplateDataAccess.GetEmailTemplateByTemplateKeyAndCompanyId(ticket.CompanyId.ToString(), EmailTemplateKey.CodeSafetyDocumentTemplate.CodeSafetyDocument);
                parser = new EmailParser(HttpContext.Current.Server.MapPath(agreementTemplate.BodyFile), templateVars, true);
                MailMessage message = new MailMessage();
                message.Body = parser.Parse();
                Body = message.Body;
            }
            catch (Exception ex)
            {
                //Logger.AddElmah(ex);
            }

            return Body;
        }

        public List<PaymentInfoCustomer> GetAllPaidPaymentInfoCustomer(Guid comid, Guid cusid)
        {
            DataTable dt = _PaymentInfoCustomerDataAccess.GetAllPaidPaymentInfoCustomer(comid, cusid);
            List<PaymentInfoCustomer> PaymentInfoList = new List<PaymentInfoCustomer>();
            PaymentInfoList = (from DataRow dr in dt.Rows
                               select new PaymentInfoCustomer()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   CompanyId = (Guid)dr["CompanyId"],
                                   Type = dr["Type"].ToString(),
                                   CustomerId = (Guid)dr["CustomerId"],
                                   Payfor = dr["Payfor"].ToString(),
                                   PaymentType = dr["PaymentType"].ToString()
                               }).ToList();
            return PaymentInfoList;
        }

        public List<AgreementQuestion> GetAllAgreementQuestionByCustomerType(string type)
        {
            return _AgreementQuestionDataAccess.GetByQuery(string.Format("SiteType = '{0}'", type)).ToList();
        }
    }
}
