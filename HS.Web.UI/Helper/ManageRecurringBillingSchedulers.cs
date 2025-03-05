using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using AuthorizeNet.Api.Contracts.V1;
using Forte;
using Forte.Entities;
using HS.Entities;
using HS.Facade;
using HS.Framework;
using HS.Framework.Utils;
using HS.Payments.TransactionReporting;
using NLog;

namespace HS.Web.UI.Helper
{
    public class ManageRecurringBillingSchedulers
    {
        #region Recurring Billing Schedulers
        public void StartCreatingRecurringBillingInvoicesForACHAndCC()
        {
            try
            {
                double dblmilisec = Convert.ToDouble("1") * 60000;
                System.Timers.Timer t = new System.Timers.Timer();
                t.Elapsed += new System.Timers.ElapsedEventHandler(StartCreatingRecurringBillingInvoicesForACHAndCC);
                t.Interval = dblmilisec;
                t.Enabled = true;
                t.AutoReset = true;
                t.Start();

            }
            catch (Exception ex)
            {

            }
        }

        public void CheckPaymentsForACHAndCCInvoices()
        {
            try
            {
                double dblmilisec = Convert.ToDouble("1") * 60000;
                System.Timers.Timer t = new System.Timers.Timer();
                t.Elapsed += new System.Timers.ElapsedEventHandler(CheckPaymentsForTheInvoices);
                t.Interval = dblmilisec;
                t.Enabled = true;
                t.AutoReset = true;
                t.Start();

            }
            catch (Exception ex)
            {
                //logger.Error(ex);
            }
        }
        #endregion

        #region OwnBilling Schedulers
        public void OwnBillingInvoiceGeneration()
        {
            try
            {
                double dblmilisec = Convert.ToDouble("1") * 60000;
                System.Timers.Timer t = new System.Timers.Timer();
                t.Elapsed += new System.Timers.ElapsedEventHandler(StartOwnBillingInvoiceGeneration);
                t.Interval = dblmilisec;
                t.Enabled = true;
                t.AutoReset = true;
                t.Start();

            }
            catch (Exception ex)
            {
                //logger.Error(ex);
            }
        }
        public void OwnBillingPaymentCollection()
        {
            try
            {
                double dblmilisec = Convert.ToDouble("1") * 60000;
                System.Timers.Timer t = new System.Timers.Timer();
                t.Elapsed += new System.Timers.ElapsedEventHandler(StartOwnBillingPaymentCollection);
                t.Interval = dblmilisec;
                t.Enabled = true;
                t.AutoReset = true;
                t.Start();

            }
            catch (Exception ex)
            {

            }
        }


        private void StartOwnBillingPaymentCollection(object sender, System.Timers.ElapsedEventArgs e)
        {
            DateTime currenttime = DateTime.Now;
            string Now = currenttime.ToString("hh:mm tt");
            string start = ConfigurationManager.AppSettings["ARBInvoiceForACHAndCC-CreatingTime"];
            var triger = start.Equals(Now);
            if (triger)
            {


            }
        }

        private void StartOwnBillingInvoiceGeneration(object sender, System.Timers.ElapsedEventArgs e)
        {

            DateTime currenttime = DateTime.Now;
            string Now = currenttime.ToString("hh:mm tt");
            string start = ConfigurationManager.AppSettings["ARBInvoiceForACHAndCC-CreatingTime"];
            var triger = start.Equals(Now);
            if (triger)
            {
                SchedulerFacade SchedulerFacadeMaster = new SchedulerFacade();
                SchedulerFacade SchedulerFacade = null;
                try
                {
                    List<Organization> OrgList = SchedulerFacadeMaster.GetAllOrganizations();
                    Guid CreatedByUid = new Guid("22222222-2222-2222-2222-222222222222");
                    if (OrgList.Count > 0)
                    {
                        foreach (var Org in OrgList)
                        {
                            Guid CompanyId = Org.CompanyId;
                            SchedulerFacade = new SchedulerFacade(Org.ConnectionString);
                            GlobalSettingsFacade GlobalSettingsFacade = new GlobalSettingsFacade(Org.ConnectionString);
                            GlobalSetting AutoGenerateEnabled = GlobalSettingsFacade.GetGlobalSettingsByKey(CompanyId, "AutoGenerateRecurringBillingEnable");
                            // By Company Settings On Off Auto Generate Recurring Bill
                            if (AutoGenerateEnabled.Value == "false") { continue; }
                            //List<Customer> CustomerList = SchedulerFacade.GetCustomerListForRecurringBillingByCompanyId(CompanyId);
                            //if (CustomerList != null && CustomerList.Count > 0)
                            //{
                            //    CustomerList = CustomerList.OrderBy(x => x.Id).ToList();
                            //    foreach (var customer in CustomerList)
                            //    {
                            List<RecurringBillingSchedule> RecurringBillingList = SchedulerFacade.GetRecurringBillingByCustomerIdAndCompanyId(CompanyId);

                            if (RecurringBillingList != null && RecurringBillingList.Count > 0)
                            {
                                RecurringBillingList = RecurringBillingList.OrderBy(x => x.Id).ToList();
                                foreach (var recurring in RecurringBillingList)
                                {
                                    string InvoiceId = "";
                                    if (!recurring.StartDate.HasValue && !recurring.PaymentCollectionDate.HasValue) { continue; }
                                    DateTime InvoiceStartDate = DateTime.UtcNow.SetZeroHour();
                                    DateTime InvoiceEndDate = DateTime.UtcNow.SetZeroHour();
                                    #region Bill cycle 
                                    if (recurring.BillCycle.ToLower() == "weekly") { InvoiceStartDate = InvoiceStartDate.AddDays(-7); }
                                    else if (recurring.BillCycle.ToLower() == "bi-weekly") { InvoiceStartDate = InvoiceStartDate.AddDays(-14); }
                                    else if (recurring.BillCycle.ToLower() == "semi-monthly") { InvoiceStartDate = InvoiceStartDate.AddDays(-15); }
                                    else if (recurring.BillCycle.ToLower() == "monthly") { InvoiceStartDate = InvoiceStartDate.AddMonths(-1); }
                                    else if (recurring.BillCycle.ToLower() == "bi-monthly") { InvoiceStartDate = InvoiceStartDate.AddMonths(-2); }
                                    else if (recurring.BillCycle.ToLower() == "quarterly") { InvoiceStartDate = InvoiceStartDate.AddMonths(-3); }
                                    else if (recurring.BillCycle.ToLower() == "semi-annually") { InvoiceStartDate = InvoiceStartDate.AddMonths(-6); }
                                    else if (recurring.BillCycle.ToLower() == "annually") { InvoiceStartDate = InvoiceStartDate.AddYears(-1); }
                                    else { InvoiceStartDate = InvoiceStartDate.AddMonths(-1); }
                                    #endregion
                                    if (!string.IsNullOrWhiteSpace(recurring.LastRMRInvoiceRefId))
                                    {
                                        Invoice _inv = SchedulerFacade.GetInvoiceByInvoiceId(recurring.LastRMRInvoiceRefId);
                                        if (_inv != null)
                                        {
                                            DateTime CreateDate = _inv.CreatedDate.SetZeroHour();
                                            if (CreateDate <= InvoiceEndDate && CreateDate >= InvoiceStartDate)
                                            {
                                                continue;
                                            }
                                        }
                                    }
                                    else if (recurring.PreviousDate.HasValue)
                                    {
                                        DateTime CreateDate = recurring.PreviousDate.Value.SetZeroHour();
                                        if (CreateDate <= InvoiceEndDate && CreateDate >= InvoiceStartDate)
                                        {
                                            continue;
                                        }
                                    }
                                    if (recurring.NextDate.HasValue)
                                    {
                                        Customer customer = SchedulerFacade.GetCustomerByCustomerId(recurring.CustomerId);
                                        if (customer != null)
                                        {
                                            DateTime NextDate = recurring.NextDate.Value;
                                            var DayInAdvance = recurring.DayInAdvance;
                                            bool paymentFlag = true;
                                            if (DayInAdvance.HasValue && DayInAdvance > 0)
                                            {
                                                NextDate = NextDate.AddDays(-DayInAdvance.Value);
                                                paymentFlag = false;
                                            }
                                            if (recurring.EndDate.HasValue)
                                            {
                                                DateTime EndDateZeroHour = recurring.EndDate.Value.SetZeroHour();
                                                DateTime CurrentZeroHour = currenttime.SetZeroHour();
                                                if (EndDateZeroHour >= CurrentZeroHour)
                                                {
                                                    if ((recurring.Status == "Active" || recurring.Status == "FreeTrial") && NextDate.ToString("yyyy-MM-dd") == currenttime.ToString("yyyy-MM-dd"))
                                                    {
                                                        // Add Invoice Create Function
                                                        InvoiceId = GenerateInvoiceForRecurringBilling(customer, recurring, Org);

                                                        if (recurring.IsEInvoice.HasValue && recurring.IsEInvoice.Value && !string.IsNullOrWhiteSpace(InvoiceId))
                                                        {
                                                            // Add Email Send Function
                                                            Invoice _Invoice = SchedulerFacade.GetInvoiceByInvoiceId(InvoiceId);
                                                            if (_Invoice != null)
                                                            {
                                                                EmailNotificationSendForNewInvoiceCreation(customer, _Invoice, SchedulerFacade, Org.ConnectionString);
                                                            }
                                                        }
                                                        if (recurring.CollectOnline && !string.IsNullOrWhiteSpace(InvoiceId) && paymentFlag)
                                                        {
                                                            // Add Payment Collection Function
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    // Recurring Biling Expired.
                                                    recurring.Status = "Expired";
                                                    recurring.LastUpdatedDate = DateTime.UtcNow;
                                                    recurring.LastUpdatedBy = CreatedByUid;
                                                    SchedulerFacade.UpdateRecurringBilling(recurring);
                                                }
                                            }
                                            else
                                            {
                                                // Recurring Biling Invoice Create Unlimited Time.
                                                if ((recurring.Status == "Active" || recurring.Status == "FreeTrial") && NextDate.ToString("yyyy-MM-dd") == currenttime.ToString("yyyy-MM-dd"))
                                                {
                                                    // Add Invoice Create Function
                                                    InvoiceId = GenerateInvoiceForRecurringBilling(customer, recurring, Org);

                                                    if (recurring.IsEInvoice.HasValue && recurring.IsEInvoice.Value && !string.IsNullOrWhiteSpace(InvoiceId))
                                                    {
                                                        // Add Email Send Function
                                                        Invoice _Invoice = SchedulerFacade.GetInvoiceByInvoiceId(InvoiceId);
                                                        if (_Invoice != null)
                                                        {
                                                            EmailNotificationSendForNewInvoiceCreation(customer, _Invoice, SchedulerFacade, Org.ConnectionString);
                                                        }
                                                    }

                                                    if (recurring.CollectOnline && !string.IsNullOrWhiteSpace(InvoiceId) && paymentFlag)
                                                    {
                                                        // Add Payment Collection Function
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        //    }
                        //}
                    }
                }
                catch (Exception ex)
                {

                }

            }
        }


        #endregion


        //New Module
        private void StartCreatingRecurringBillingInvoicesForACHAndCC(object sender, System.Timers.ElapsedEventArgs e)
        {
            DateTime currenttime = DateTime.Now;
            string Now = currenttime.ToString("hh:mm tt");
            //using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\2.txt"), true))
            //{
            //    file.WriteLine("ChecK Invoice Payments: " + currenttime.ToString("MM/dd/yyyy hh:mm tt"));
            //    file.Close();
            //}
            string start = ConfigurationManager.AppSettings["ARBInvoiceForACHAndCC-CreatingTime"];
            var triger = start.Equals(Now);
            if (triger)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\2.txt"), true))
                {
                    file.WriteLine(DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                    file.Close();
                }
                StartCreatingARBnvoicesForACHAndCC(currenttime);
            }
        }
        //New Module
        private void CheckPaymentsForTheInvoices(object sender, System.Timers.ElapsedEventArgs e)
        {
            DateTime currenttime = DateTime.Now;
            string Now = currenttime.ToString("hh:mm tt");

            string start = ConfigurationManager.AppSettings["CheckPaymentForACHAndCC-Invoices"];
            var triger = start.Equals(Now);
            if (triger)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\2.txt"), true))
                {
                    file.WriteLine(DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                    file.Close();
                }
                CheckPayments(currenttime);
            }
        }




        #region Recurring Billing Functions
        //Need to send email when creating invoice
        private void StartCreatingARBnvoicesForACHAndCC(DateTime now)
        {
            SchedulerFacade SchedulerFacadeMaster = new SchedulerFacade();
            SchedulerFacade SchedulerFacade = null;
            string CurrentUser = "System";
            Guid CreatedByUid = new Guid("22222222-2222-2222-2222-222222222222");
            DateTime CreatedDate = DateTime.Today; //DateTime.Today;
            DateTime DueDate = CreatedDate.AddDays(-1).AddMonths(1); //DateTime.Today.AddDays(-1).AddMonths(1);


            List<Organization> OrgList = SchedulerFacadeMaster.GetAllOrganizations();
            foreach (var org in OrgList)
            {
                Guid CompanyId = org.CompanyId;
                SchedulerFacade = new SchedulerFacade(org.ConnectionString);
                string TaxPercentage = SchedulerFacade.GetSalesTax(CompanyId);
                #region Check system settings
                GlobalSetting RecurringBillingEnabled = SchedulerFacade.GetGlobalsettingBySearchKeyAndCompanyId("ACH-CC-InvoiceFirst-RecurringBillingEnabled", CompanyId);
                if (RecurringBillingEnabled == null || RecurringBillingEnabled.Value.ToLower() == "false")
                {
                    continue;
                }

                GlobalSetting PaymentGetway = SchedulerFacade.GetGlobalsettingBySearchKeyAndCompanyId("PaymentGetway", CompanyId);
                if (PaymentGetway == null || (PaymentGetway.Value.ToLower() != "forte" && PaymentGetway.Value.ToLower() != "authorize.net"))
                {
                    continue;
                }

                //This will show "Bill for the month of January"
                GlobalSetting MentionBillingMonth = SchedulerFacade.GetGlobalsettingBySearchKeyAndCompanyId("MentionBillingMonth", CompanyId);

                GlobalSetting BillFor = SchedulerFacade.GetGlobalsettingBySearchKeyAndCompanyId("BillFor", CompanyId);

                #endregion

                #region Company Billing String
                string CompanyBillingStr = "Alarm Monitoring Fee {0}";
                GlobalSetting companyBillingString = SchedulerFacade.GetGlobalsettingBySearchKeyAndCompanyId("ARBInvoiceMessage", CompanyId);
                if (companyBillingString != null)
                {
                    CompanyBillingStr = companyBillingString.Value + " {0}";
                    companyBillingString = null;
                }
                #endregion

                #region Billing Item Default Message
                GlobalSetting BllingItemDefaultMessage = SchedulerFacade.GetGlobalsettingBySearchKeyAndCompanyId("BllingItemDefaultMessage", CompanyId);
                String ItemsDefaultMessage = "Monitoring Fee";
                String ItemsDescription = "Monitoring Fee {0}";
                if (BllingItemDefaultMessage != null)
                {
                    ItemsDefaultMessage = BllingItemDefaultMessage.Value;
                    ItemsDescription = BllingItemDefaultMessage.Value + "{0}";

                }

                #endregion

                List<Customer> customers = SchedulerFacade.GetACHAndCCSubscribedCustomer();
                var AddressTemplate = SchedulerFacade.GetCustomerAddressFormat(CompanyId);

                foreach (Customer customer in customers)
                {

                    if (customer.LastGeneratedInvoice == CreatedDate)
                    {
                        continue;
                    }
                    if (customer.BillCycle == LabelHelper.BillCycle.Bi_Monthly)
                    {
                        if (customer.FirstBilling.Value.Month % 2 != CreatedDate.Month % 2)
                        {
                            continue;
                        }
                    }
                    List<Invoice> sameDayInvoices = SchedulerFacade.GetARBInvoiceByCustomerIdAndDate(customer.CustomerId, DateTime.Today);
                    if (sameDayInvoices != null && sameDayInvoices.Count > 0)
                    {
                        continue;
                    }
                    double TaxAmount = 0;
                    double TotalTax = 0;
                    string TaxType = "Non-Tax";
                    int MonthsCount = 1;

                    string BillingStr = "";

                    #region Billing String 

                    if (!string.IsNullOrWhiteSpace(customer.BillCycle))
                    {
                        DateTime BillingBaseMonth = DateTime.Today;

                        #region Bill day setup from bill cycle
                        if (customer.BillCycle == LabelHelper.BillCycle.Monthly)
                        {
                            if (BillFor != null && BillFor.Value == "Next Month")
                            {
                                BillingBaseMonth = BillingBaseMonth.AddMonths(1);
                            }
                            if (BillFor != null && BillFor.Value == "Last Month")
                            {
                                BillingBaseMonth = BillingBaseMonth.AddMonths(-1);
                            }
                        }
                        else if (customer.BillCycle == LabelHelper.BillCycle.Annual
                            || customer.BillCycle == LabelHelper.BillCycle.Annually)
                        {
                            if (BillFor != null && BillFor.Value == "Next Month")
                            {
                                BillingBaseMonth = BillingBaseMonth.AddYears(1);
                            }
                            if (BillFor != null && BillFor.Value == "Last Month")
                            {
                                BillingBaseMonth = BillingBaseMonth.AddYears(-1);
                            }
                        }
                        else if (customer.BillCycle == LabelHelper.BillCycle.SemiAnnual
                            || customer.BillCycle == LabelHelper.BillCycle.SemiAnnually)
                        {
                            if (BillFor != null && BillFor.Value == "Next Month")
                            {
                                BillingBaseMonth = BillingBaseMonth.AddMonths(6);
                            }
                            if (BillFor != null && BillFor.Value == "Last Month")
                            {
                                BillingBaseMonth = BillingBaseMonth.AddMonths(-6);
                            }
                        }
                        else if (customer.BillCycle == LabelHelper.BillCycle.Bi_Monthly)
                        {
                            if (BillFor != null && BillFor.Value == "Next Month")
                            {
                                BillingBaseMonth = BillingBaseMonth.AddMonths(2);
                            }
                            if (BillFor != null && BillFor.Value == "Last Month")
                            {
                                BillingBaseMonth = BillingBaseMonth.AddMonths(-2);
                            }
                        }
                        else if (customer.BillCycle == LabelHelper.BillCycle.Weekly)
                        {
                            if (BillFor != null && BillFor.Value == "Next Month")
                            {
                                BillingBaseMonth = BillingBaseMonth.AddDays(7);
                            }
                            if (BillFor != null && BillFor.Value == "Last Month")
                            {
                                BillingBaseMonth = BillingBaseMonth.AddDays(-7);
                            }
                        }
                        #endregion


                        if (customer.BillCycle == LabelHelper.BillCycle.Monthly)
                        {
                            MonthsCount = 1;
                            if (MentionBillingMonth != null && MentionBillingMonth.Value.ToLower() == "true")
                            {
                                BillingStr = string.Format("For the Month of {0} {1}", BillingBaseMonth.ToString("MMMM"), BillingBaseMonth.ToString("yyyy"));
                            }
                        }
                        else if (customer.BillCycle == LabelHelper.BillCycle.Bi_Monthly)
                        {
                            MonthsCount = 2;
                            if (customer.FirstBilling.Value.Month % 2 != DateTime.Now.Month % 2)
                            {
                                continue;
                            }
                            if (MentionBillingMonth != null && MentionBillingMonth.Value.ToLower() == "true")
                            {
                                BillingStr = string.Format("For the Month of {0} and {1}", BillingBaseMonth.ToString("MMMM"), BillingBaseMonth.AddMonths(1).ToString("MMMM"));
                            }
                        }
                        else if (customer.BillCycle == LabelHelper.BillCycle.Weekly)
                        {
                            if (customer.FirstBilling.Value.DayOfWeek != DateTime.Now.DayOfWeek)
                            {
                                continue;
                            }
                            //For now we I don't know what to say in here.
                            BillingStr = "weekly";
                        }
                        else if (customer.BillCycle == LabelHelper.BillCycle.Bi_Weekly || customer.BillCycle == LabelHelper.BillCycle.Semi_Monthly)
                        {
                            //For now we I don't know what to say in here.
                        }
                        else if (customer.BillCycle == LabelHelper.BillCycle.Quarterly)
                        {
                            //Jan 1//Feb 2//Mar 3//Apr 4//May 5//Jun 6//Jul 7//Aug 8//Sep 9//Oct 10//Nov 11//Dec 12
                            //12//3//6//9//
                            if (customer.FirstBilling.Value.Month % 3 != DateTime.Now.Month % 3)
                            {
                                continue;
                            }
                            MonthsCount = 3;
                            if (MentionBillingMonth != null && MentionBillingMonth.Value.ToLower() == "true")
                            {
                                BillingStr = String.Format("For {0} - {1}", BillingBaseMonth.ToString("MMMM"), BillingBaseMonth.AddMonths(2).ToString("MMMM"));
                            }
                            #region Not matches now
                            /*
                            if (DateTime.Now.Month == 12)
                            {
                                BillingStr = "Q1 (Jan-Mar)";
                            }
                            else if (DateTime.Now.Month == 3)
                            {
                                BillingStr = "Q2 (Apr-Jun)";

                            }
                            else if (DateTime.Now.Month == 6)
                            {
                                BillingStr = "Q3 (Jul-Sep)";

                            }
                            else if (DateTime.Now.Month == 9)
                            {
                                BillingStr = "Q4 (Oct-Dec)"; 
                            }*/
                            #endregion

                        }
                        else if (customer.BillCycle == LabelHelper.BillCycle.SemiAnnual
                            || customer.BillCycle == LabelHelper.BillCycle.SemiAnnually)
                        {
                            //Jan 1//Feb 2//Mar 3//Apr 4//May 5//Jun 6//Jul 7//Aug 8//Sep 9//Oct 10//Nov 11//Dec 12
                            if (customer.FirstBilling.Value.Month % 6 != DateTime.Now.Month % 6)
                            {
                                continue;
                            }
                            MonthsCount = 6;
                            if (MentionBillingMonth != null && MentionBillingMonth.Value.ToLower() == "true")
                            {
                                BillingStr = String.Format("For {0} - {1}", BillingBaseMonth.ToString("MMMM"), BillingBaseMonth.AddMonths(5).ToString("MMMM"));
                            }
                            #region Old Condtions
                            /*
                            if (DateTime.Now.Month == 12)
                            {
                                BillingStr = "Semi annual 1 (Jan-Jun)";
                            }
                            else if (DateTime.Now.Month == 6)
                            {
                                BillingStr = "Semi annual 2 (Jul-Dec)";

                            }*/
                            #endregion
                        }
                        else if (customer.BillCycle == LabelHelper.BillCycle.Annual)
                        {
                            if (customer.FirstBilling.Value.Month != DateTime.Now.Month)
                            {
                                continue;
                            }
                            MonthsCount = 12;
                            BillingStr = "Year " + BillingBaseMonth.Year.ToString();
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }
                    #endregion

                    #region Tax Calculations
                    MonthsCount = 1; // AS they want to see QTY =1 for all the cases. Still I have kept the variable, So that we can easyly change if we need.

                    double MonitoringFee = (double)customer.BillAmount;
                    TaxAmount = 0;
                    TaxType = "Non-Tax";
                    if (customer.BillTax.HasValue && customer.BillTax.Value)
                    {
                        /*(100*sattleAmount)/(100+taxPercentage)*/

                        MonitoringFee = ((100 * MonitoringFee) / (100 + (Convert.ToDouble(TaxPercentage))));
                        TaxAmount = (double)customer.BillAmount - MonitoringFee;

                        TaxType = "Sales Tax";
                        //TaxAmount = Math.Round((MonitoringFee * (Convert.ToDouble(TaxPercentage) / 100)), 2);
                        //MonitoringFee = MonitoringFee - TaxAmount;
                    }

                    #region Tax calculation previous
                    /*double CustomerTotalBillAmount = Math.Round(((Convert.ToDouble(customer.MonthlyMonitoringFee) * MonthsCount)), 2);
                    double CustomerTotalBillAmountWithTax = CustomerTotalBillAmount;

                    if (customer.BillTax.HasValue && customer.BillTax.Value && !string.IsNullOrWhiteSpace(TaxPercentage))
                    {
                        TotalTax = Math.Round((CustomerTotalBillAmountWithTax * (Convert.ToDouble(TaxPercentage) / 100)), 2);
                        CustomerTotalBillAmountWithTax += TotalTax;
                        CustomerTotalBillAmountWithTax = Math.Round(CustomerTotalBillAmountWithTax, 2);

                        TaxType = "Sales Tax";

                    }*/
                    #endregion

                    #endregion

                    #region Insert Invoice
                    Invoice inv = new Invoice()
                    {
                        CompanyId = CompanyId,
                        InvoiceFor = string.IsNullOrWhiteSpace(customer.PaymentMethod) ? LabelHelper.InvoiceFor.SystemGenerated : customer.PaymentMethod,
                        Tax = TotalTax,
                        InvoiceEmailAddress = customer.EmailAddress,
                        TotalAmount = MonitoringFee + TaxAmount,
                        BalanceDue = MonitoringFee + TaxAmount,
                        Amount = MonitoringFee,
                        DiscountAmount = 0,
                        Status = "Init",
                        DueDate = DueDate,//DateTime.Today.AddDays(-1).AddMonths(1),
                        CreatedBy = CurrentUser,
                        CreatedDate = CreatedDate,
                        InvoiceDate = CreatedDate,
                        LastUpdatedDate = DateTime.Now,
                        IsBill = false,
                        IsEstimate = false,
                        //LateFee = invoiceLateFee,
                        CustomerName = customer.FirstName + " " + customer.LastName,
                        CustomerId = customer.CustomerId,
                        BillingCycle = customer.BillCycle,
                        BillingAddress = AddressHelper.MakeCustomerAddress(customer, "BillingAddress", AddressTemplate),
                        ShippingAddress = AddressHelper.MakeCustomerAddress(customer, "ShippingAddress", AddressTemplate),

                        TaxType = TaxType,
                        Message = string.Format(CompanyBillingStr, BillingStr),
                        CreatedByUid = CreatedByUid,
                        IsARBInvoice = true,

                    };
                    inv.Id = SchedulerFacade.InsertInvoice(inv);
                    inv.InvoiceId = inv.Id.GenerateInvoiceNo();
                    inv.Status = LabelHelper.InvoiceStatus.Open;
                    SchedulerFacade.UpdateInvoice(inv);
                    #endregion

                    #region Insert Invoice Detail
                    InvoiceDetail invDet = new InvoiceDetail()
                    {
                        CompanyId = CompanyId,
                        CreatedBy = CurrentUser,
                        CreatedDate = CreatedDate,
                        InvoiceId = inv.InvoiceId,
                        //TotalPrice = Math.Round(Convert.ToDouble(customer.MonthlyMonitoringFee) * MonthsCount, 2) ,
                        //UnitPrice = Math.Round(Convert.ToDouble(customer.MonthlyMonitoringFee), 2),
                        TotalPrice = MonitoringFee,
                        UnitPrice = MonitoringFee,
                        Quantity = MonthsCount,
                        EquipName = ItemsDefaultMessage,
                        EquipDetail = string.Format(ItemsDescription, BillingStr) //with tax {1}% ,TaxPercentage
                    };
                    inv.Description = invDet.EquipDetail;
                    SchedulerFacade.InsertInvoiceDetails(invDet);

                    #endregion

                    customer.LastGeneratedInvoice = CreatedDate;
                    customer.LastUpdatedBy = CurrentUser;
                    SchedulerFacade.UpdateCustomer(customer);
                }
            }
        }

        private void CheckPayments(DateTime now)
        {
            SchedulerFacade SchedulerFacadeMaster = new SchedulerFacade();
            SchedulerFacade SchedulerFacade = null;
            string CurrentUser = "System";
            Guid CreatedByUid = new Guid("22222222-2222-2222-2222-222222222222");
            DateTime CreatedDate = DateTime.Today; //DateTime.Today;
            DateTime DueDate = CreatedDate.AddDays(-1).AddMonths(1); //DateTime.Today.AddDays(-1).AddMonths(1);
            List<Organization> OrgList = SchedulerFacadeMaster.GetAllOrganizations();
            foreach (var org in OrgList)
            {
                SchedulerFacade = new SchedulerFacade(org.ConnectionString);
                Guid CompanyId = org.CompanyId;
                GlobalSetting RecurringBillingEnabled = SchedulerFacade.GetGlobalsettingBySearchKeyAndCompanyId("ACH-CC-InvoiceFirst-RecurringBillingEnabled", CompanyId);
                if (RecurringBillingEnabled == null || RecurringBillingEnabled.Value.ToLower() == "false")
                {
                    continue;
                }

                GlobalSetting PaymentGetway = SchedulerFacade.GetGlobalsettingBySearchKeyAndCompanyId("PaymentGetway", CompanyId);
                if (PaymentGetway != null && PaymentGetway.Value.ToLower() == "forte")
                {
                    CheckPaymentsForte(org, SchedulerFacade, CurrentUser, CreatedByUid);
                }
                else if (PaymentGetway != null && PaymentGetway.Value.ToLower() == "authorize.net")
                {
                    CheckPaymentsAuthorize(org, SchedulerFacade, CurrentUser, CreatedByUid);
                }
            }
        }

        private void CheckPaymentsForte(Organization org, SchedulerFacade SchedulerFacade, string CurrentUser, Guid CreatedByUid)
        {
            Guid CompanyId = org.CompanyId;
            string TaxPercentage = SchedulerFacade.GetSalesTax(CompanyId);
            int InvoicePullingDays = 10;
            string TempStatus = "";


            #region Prduction or Live
            bool ForteInProduction = false;
            GlobalSetting globset2 = SchedulerFacade.GetGlobalsettingBySearchKeyAndCompanyId("ForteInProduction", CompanyId);
            if (globset2 != null && globset2.Value.ToLower() == "true")
            {
                ForteInProduction = true;
            }
            #endregion

            #region Forte 
            string ForteTransactionKey = SchedulerFacade.GetForteTransactionKeyByCompanyId(CompanyId, false);
            string ForteAPILoginId = SchedulerFacade.GetForteAPILoginIdByCompanyId(CompanyId, false);
            string ForteOrganizationId = SchedulerFacade.GetForteOrganizationIdByCompanyId(CompanyId, false);
            string ForteLocationId = SchedulerFacade.GetForteLocationIdByCompanyId(CompanyId, false);
            string ForteAuthAccountId = SchedulerFacade.GetForteAuthAccountIdByCompanyId(CompanyId, false);

            ForteOptions forte = new ForteOptions();
            forte.Organization_ID = ForteOrganizationId;
            forte.Location_Id = ForteLocationId;
            forte.AuthAccountId = ForteAuthAccountId;

            forte.UserId = ForteAPILoginId;
            forte.Password = ForteTransactionKey;

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3
                               | System.Net.SecurityProtocolType.Tls
                               | System.Net.SecurityProtocolType.Tls11
                               | System.Net.SecurityProtocolType.Tls12;

            if (ForteInProduction == true)
            {
                forte.Server = "https://api.forte.net/v3";
            }
            else
            {
                forte.Server = "https://sandbox.forte.net/api/v3";
            }

            ForteTransaction forteTrans = new ForteTransaction();
            ForteTransactionService forteTransaction;
            #endregion

            List<Customer> SubscribedCustomer = SchedulerFacade.GetBilledCustomer(InvoicePullingDays);

            foreach (Customer customer in SubscribedCustomer)
            {
                forte.Customer_Token = customer.CustomerToken;
                forteTransaction = new ForteTransactionService(forte);
                ForteTransectionResponse transResponse = new ForteTransectionResponse();
                try
                {
                    FortePaymentGetwayResponse response = forteTransaction.GetTransaction();
                    transResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ForteTransectionResponse>(response.Massege);

                    List<Invoice> invoices = SchedulerFacade.GetARBInvoicesByCustomerId(customer.CustomerId, InvoicePullingDays);

                    //Need to find out un-used Transactions

                    foreach (var invoice in invoices.Where(x => x.TransactionId == "" || x.TransactionId == null))
                    {
                        ResultTransection UnusedTransaction = transResponse.results.Where(x => x.received_date >= invoice.CreatedDate
                        && x.received_date <= invoice.CreatedDate.AddDays(InvoicePullingDays) && x.entered_by == "Scheduled").FirstOrDefault();
                        if (UnusedTransaction != null)
                        {
                            TempStatus = UnusedTransaction.status;

                        }
                        if (UnusedTransaction != null)
                        {
                            invoice.TransactionId = UnusedTransaction.transaction_id;
                            invoice.ForteStatus = UnusedTransaction.status;
                            SchedulerFacade.UpdateInvoice(invoice);


                            if (invoice.BalanceDue != UnusedTransaction.authorization_amount)
                            {
                                RMRBillingMismatch mismatch = new RMRBillingMismatch()
                                {
                                    BillingAmount = invoice.BalanceDue.Value,
                                    CustomerId = invoice.CustomerId,
                                    InvoiceId = invoice.InvoiceId,
                                    IsResolved = false,
                                    ResolvedBy = Guid.Empty,
                                    ResolvedDate = DateTime.Now.UTCCurrentTime(),
                                    TransactionId = UnusedTransaction.transaction_id,
                                    ChargedAmountByGateway = UnusedTransaction.authorization_amount,

                                };
                                SchedulerFacade.InsertRMRBillingMismatch(mismatch);
                            }
                        }
                    }
                    foreach (Invoice invoice in invoices.Where(x => x.TransactionId != ""))
                    {
                        ResultTransection UnusedTransaction = transResponse.results.Where(x => x.transaction_id == invoice.TransactionId).FirstOrDefault();
                        if (UnusedTransaction != null)
                        {
                            invoice.ForteStatus = UnusedTransaction.status;
                            RMRBillingMismatch mismatch = SchedulerFacade.GetRMRBillingMismatchByTransactionID(UnusedTransaction.transaction_id);
                            if (mismatch != null)
                            {
                                SchedulerFacade.UpdateInvoice(invoice);
                                continue;
                            }

                            if (invoice.ForteStatus.ToLower() == LabelHelper.FortePaymentStatus.Settled.ToLower()
                                || invoice.ForteStatus.ToLower() == LabelHelper.FortePaymentStatus.Complete.ToLower()
                                || invoice.ForteStatus.ToLower() == LabelHelper.FortePaymentStatus.Funded.ToLower())
                            {
                                invoice.Status = LabelHelper.InvoiceStatus.Paid;
                                invoice.ForteStatus = UnusedTransaction.status;
                                SchedulerFacade.UpdateInvoice(invoice);



                                Transaction transaction = new Transaction()
                                {
                                    CompanyId = CompanyId,
                                    CustomerId = customer.CustomerId,
                                    InvoiceIdStr = invoice.InvoiceId,
                                    PaymentMethod = customer.PaymentMethod,
                                    InvoiceId = invoice.Id,
                                    AddedBy = CurrentUser,
                                    Status = "Closed",
                                    TransacationDate = UnusedTransaction.received_date,
                                    CustomerName = customer.FirstName + " " + customer.LastName,
                                    Amount = UnusedTransaction.authorization_amount,
                                    Balance = 0,
                                    AddedDate = DateTime.Today,
                                    ReferenceNo = UnusedTransaction.transaction_id,
                                    CardTransactionId = UnusedTransaction.transaction_id,
                                    Type = "Payment",
                                    CreatedBy = CreatedByUid,
                                };
                                transaction.Id = SchedulerFacade.InsertTransaction(transaction);
                                TransactionHistory trhs = new TransactionHistory()
                                {
                                    InvoiceId = invoice.Id,
                                    InvoiceNumber = invoice.InvoiceId,
                                    TransacationDate = DateTime.Today,
                                    TransactionId = transaction.Id,
                                    Amout = (double)UnusedTransaction.authorization_amount,
                                    Balance = invoice.BalanceDue.HasValue ? invoice.BalanceDue.Value : 0,
                                    ReceivedBy = CreatedByUid
                                };
                                trhs.Id = SchedulerFacade.InsertTransactionHistory(trhs);

                            }
                            else if (invoice.ForteStatus.ToLower() == LabelHelper.FortePaymentStatus.Declined.ToLower()
                                || invoice.ForteStatus.ToLower() == LabelHelper.FortePaymentStatus.Failed.ToLower()
                                || invoice.ForteStatus.ToLower() == LabelHelper.FortePaymentStatus.Rejected.ToLower()
                                || invoice.ForteStatus.ToLower() == LabelHelper.FortePaymentStatus.Voided.ToLower())
                            {
                                invoice.Status = LabelHelper.InvoiceStatus.Declined;
                                invoice.ForteStatus = UnusedTransaction.status;
                                SchedulerFacade.UpdateInvoice(invoice);

                                DeclinedTransactions dt = new DeclinedTransactions()
                                {
                                    InvoiceId = invoice.InvoiceId,
                                    TransactionId = UnusedTransaction.transaction_id,
                                    CompanyId = CompanyId,
                                    CustomerId = customer.CustomerId,
                                    SubmitAmount = UnusedTransaction.authorization_amount,
                                    Reason = "Transaction declined by forte.net",
                                    Comment = string.Format(@"Transaction declined by forte.net. {0} has been created. Status: {1}", invoice.InvoiceId, UnusedTransaction.status),

                                    ReturnAmount = -1 * UnusedTransaction.authorization_amount,
                                    ReturnedDate = DateTime.Now.UTCCurrentTime(),
                                    SettlementBatch = DateTime.Now.UTCCurrentTime(),

                                };
                                SchedulerFacade.InsertDeclinedTransaction(dt);


                            }
                        }
                    }
                }
                catch (Exception ex) { }

            }
        }

        private void CheckPaymentsAuthorize(Organization org, SchedulerFacade SchedulerFacade, string CurrentUser, Guid CreatedByUid)
        {
            Guid CompanyId = org.CompanyId;
            string TaxPercentage = SchedulerFacade.GetSalesTax(CompanyId);
            int InvoicePullingDays = 10;

            #region Prduction or Live
            bool AuthInProduction = false;
            GlobalSetting globset2 = SchedulerFacade.GetGlobalsettingBySearchKeyAndCompanyId("Authorize.NetInProduction", CompanyId);
            if (globset2 != null && globset2.Value.ToLower() == "true")
            {
                AuthInProduction = true;
            }
            #endregion

            #region ACH
            string TransactionKey = SchedulerFacade.GetAuthTransactionKeyByCompanyId(CompanyId, true);
            string APILoginId = SchedulerFacade.GetAuthAPILoginIdByCompanyId(CompanyId, true);
            getSettledBatchListResponse BatchListResponse = GetSettledBatchList.Run(APILoginId, TransactionKey, InvoicePullingDays, AuthInProduction);
            if (BatchListResponse != null && BatchListResponse.batchList == null && BatchListResponse.messages != null)
            {
                //Error
            }
            else
            {
                BatchListProcessing(BatchListResponse, APILoginId, TransactionKey, CurrentUser, CompanyId, TaxPercentage, SchedulerFacade, AuthInProduction, true, org.CompanyName.ReplaceSpecialChar());
            }

            #endregion

            #region CC
            string TransactionKeyCC = SchedulerFacade.GetAuthTransactionKeyByCompanyId(CompanyId, false);
            string APILoginIdCC = SchedulerFacade.GetAuthAPILoginIdByCompanyId(CompanyId, false);
            if (APILoginIdCC != APILoginId && TransactionKeyCC != TransactionKey)
            {
                BatchListResponse = GetSettledBatchList.Run(APILoginIdCC, TransactionKeyCC, InvoicePullingDays, AuthInProduction);
                if (BatchListResponse != null && BatchListResponse.batchList == null && BatchListResponse.messages != null)
                {
                    //ERROR
                }
                else
                {
                    BatchListProcessing(BatchListResponse, APILoginId, TransactionKey, CurrentUser, CompanyId, TaxPercentage, SchedulerFacade, AuthInProduction, true, org.CompanyName.ReplaceSpecialChar());
                }
            }
            #endregion

        }

        private void BatchListProcessing(getSettledBatchListResponse BatchListResponse, string APILoginId, string TransactionKey, string CurrentUser
           , Guid CompanyId, string TaxPercentage, SchedulerFacade SchedulerFacade, bool AuthorizeInProduction, bool ACHPayment, string CompanyName)
        {
            Guid CreatedByUid = new Guid("22222222-2222-2222-2222-222222222222");
            try
            {
                #region BatchListResponse Processing
                if (BatchListResponse != null && BatchListResponse.batchList != null && BatchListResponse.batchList.Count() > 0)
                {
                    foreach (var batch in BatchListResponse.batchList)
                    {
                        try
                        {
                            #region loop through batch
                            getTransactionListResponse TransactionListResponse = GetTransactionList.Run(APILoginId, TransactionKey, batch.batchId, AuthorizeInProduction);
                            if (TransactionListResponse == null)
                            {
                                continue;
                            }
                            foreach (transactionSummaryType transaction in TransactionListResponse.transactions)
                            {
                                try
                                {
                                    #region Loop through transaction
                                    if (transaction.subscription == null)
                                    {
                                        //Not an ARB Transaction
                                        continue;
                                    }
                                    else if (transaction.subscription != null)// && transaction.invoiceNumber == LabelHelper.InvoiceNumberForARB.MonitoringFee)
                                    {
                                        Customer customer = new Customer();
                                        customer = SchedulerFacade.GetCustomerBySubscriptionId(transaction.subscription.id);
                                        #region If customer not found continue
                                        if (customer == null || customer.Id == 0)
                                        {
                                            continue;
                                        }
                                        #endregion

                                        #region Check If the transaction already exists
                                        Transaction CustomerTransaction = SchedulerFacade.GetTransactionByTransactionId(transaction.transId);
                                        if (CustomerTransaction != null && CustomerTransaction.Id > 0)
                                        {
                                            continue;
                                        }
                                        DeclinedTransactions declinedTransaction = SchedulerFacade.GetDeclinedTransactionByTransactionId(transaction.transId);
                                        if (declinedTransaction != null)
                                        {
                                            continue;
                                        }
                                        RMRBillingMismatch RMRBillingMismatch = SchedulerFacade.GetRMRBillingMismatchByTransactionID(transaction.transId);
                                        if (RMRBillingMismatch != null)
                                        {
                                            //Once it missmatches we are not going to do anything for this transaaction.
                                            continue;
                                        }
                                        Invoice ARBInvoice = SchedulerFacade.GetInvoiceByTransactionId(transaction.transId);
                                        if (ARBInvoice == null)
                                        {
                                            //New transaction
                                            ARBInvoice = SchedulerFacade.GetLatestARBInvoiceByCustomerId(customer.CustomerId);
                                        }
                                        if (ARBInvoice != null)
                                        {
                                            ARBInvoice.TransactionId = transaction.transId;
                                            ARBInvoice.ForteStatus = transaction.transactionStatus;
                                            if (Math.Round(ARBInvoice.TotalAmount.Value, 2) != (double)transaction.settleAmount)
                                            {
                                                RMRBillingMismatch rMRBillingMismatch = new RMRBillingMismatch()
                                                {
                                                    BillingAmount = ARBInvoice.TotalAmount.Value,
                                                    ChargedAmountByGateway = (double)transaction.settleAmount,
                                                    CreatedDate = DateTime.UtcNow,
                                                    CustomerId = ARBInvoice.CustomerId,
                                                    InvoiceId = ARBInvoice.InvoiceId,
                                                    IsResolved = false,
                                                    ResolvedBy = Guid.Empty,
                                                    TransactionId = transaction.transId,

                                                };

                                                SchedulerFacade.InsertRMRBillingMismatch(rMRBillingMismatch);
                                            }
                                            else if (transaction.transactionStatus == "settledSuccessfully")
                                            {
                                                ARBInvoice.BalanceDue = 0;
                                                ARBInvoice.Status = LabelHelper.InvoiceStatus.Paid;


                                                CustomerTransaction = new Transaction()
                                                {
                                                    InvoiceNo = ARBInvoice.InvoiceId,
                                                    PaymentMethod = ARBInvoice.InvoiceFor,
                                                    TransactionId = transaction.transId,
                                                    Type = "Payment",
                                                    CustomerId = customer.CustomerId,
                                                    Amount = (double)transaction.settleAmount,
                                                    Status = "Closed",
                                                    TransacationDate = transaction.submitTimeUTC,
                                                    CardTransactionId = transaction.transId,
                                                    AddedBy = "System",
                                                    AddedDate = DateTime.UtcNow,
                                                    CreatedBy = CreatedByUid,
                                                    CompanyId = ARBInvoice.CompanyId,

                                                };
                                                CustomerTransaction.Id = SchedulerFacade.InsertTransaction(CustomerTransaction);
                                                TransactionHistory transactionHistory = new TransactionHistory()
                                                {
                                                    Amout = CustomerTransaction.Amount,
                                                    Balance = CustomerTransaction.Amount,
                                                    InvoiceId = ARBInvoice.Id,
                                                    InvoiceBalanceDue = 0,
                                                    ReceivedBy = CreatedByUid,
                                                    TransactionId = CustomerTransaction.Id
                                                };
                                                SchedulerFacade.InsertTransactionHistory(transactionHistory);
                                            }
                                            else if (transaction.transactionStatus.ToLower() != "settledSuccessfully")
                                            {
                                                if (transaction.transactionStatus.ToLower() == "declined")
                                                {
                                                    ARBInvoice.Status = LabelHelper.InvoiceStatus.Declined;

                                                    declinedTransaction = new DeclinedTransactions()
                                                    {
                                                        CompanyId = ARBInvoice.CompanyId,
                                                        CustomerId = customer.CustomerId,
                                                        InvoiceId = ARBInvoice.InvoiceId,
                                                        TransactionId = ARBInvoice.TransactionId,
                                                        Reason = "Transaction declined by authorize.net.",
                                                        Comment = "Transaction declined by authorize.net.",
                                                        ReturnAmount = -1 * (double)transaction.settleAmount,
                                                        SubmitAmount = (double)transaction.settleAmount,
                                                        ReturnedDate = DateTime.Now.UTCCurrentTime(),
                                                        SettlementBatch = DateTime.Now.UTCCurrentTime(),

                                                    };
                                                    SchedulerFacade.InsertDeclinedTransaction(declinedTransaction);
                                                }
                                            }
                                            SchedulerFacade.UpdateInvoice(ARBInvoice);

                                            continue;

                                        }
                                        #endregion

                                        #region Tax Calculation
                                        //double MonitoringFee = (double)transaction.settleAmount;
                                        //double TaxAmount = 0;
                                        //string TaxType = "Non-Tax";
                                        //if (customer.BillTax.HasValue && customer.BillTax.Value)
                                        //{
                                        //    /*(100*sattleAmount)/(100+taxPercentage)*/

                                        //    MonitoringFee = ((100 * MonitoringFee) / (100 + (Convert.ToDouble(TaxPercentage))));
                                        //    TaxAmount = (double)transaction.settleAmount - MonitoringFee;

                                        //    TaxType = "Sales Tax";
                                        //}
                                        #endregion
                                    }
                                    #endregion
                                }
                                catch (Exception ex)
                                {
                                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\ErrorrsFromAuthorize-ach-cc.txt"), true))
                                    {
                                        file.WriteLine(string.Format("Exception occured for transaction: {0} batch{1} EX:{2}", transaction.transId, batch.batchId, ex.Message));
                                    }

                                }

                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\ErrorrsFromAuthorize-ach-cc.txt"), true))
                            {
                                file.WriteLine(string.Format("Exception occured for batch: {0} EX:{1}", batch.batchId, ex.Message));
                            }
                        }

                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\CheckPaymentReport.txt"), true))
                {
                    file.WriteLine(string.Format("Exception occured for company: {0} EX:{1}", CompanyId, ex.Message));
                }
            }

        }

        #endregion

        #region InvoiceGenerateForRecurringBilling

        public string GenerateInvoiceForRecurringBilling(Customer customer, RecurringBillingSchedule recurringBilling, Organization org)
        {
            DateTime CreatedDate = DateTime.UtcNow;
            string CurrentUser = "System";
            Guid CreatedByUid = new Guid("22222222-2222-2222-2222-222222222222");
            string result = "";
            DateTime TrimStartDate = new DateTime();
            if (recurringBilling.NextDate.HasValue)
            {
                TrimStartDate = recurringBilling.NextDate.Value.SetZeroHour();
            }
            else
            {
                return result;
            }
            try
            {
                #region Directory for saving reports
                string subPath = "~/SchedulerReports"; // your code goes here

                bool exists = System.IO.Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath(subPath));
                if (!exists)
                    System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath(subPath));

                #endregion

                #region RecurringBillingSchedulerRunLog

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\Recurring_Billing_Invoice_Generation_Timmer.txt"), true))
                {
                    file.WriteLine(Framework.Utils.AppConfig.SiteDomain + " : Fire scheduler For Recurring Billing Invoice Generation: " + CreatedDate.ToString("dd/MM/yyyy hh:mm:ss.fff"));
                    file.Close();
                }
                #endregion
                DateTime DueDate = CreatedDate.AddDays(-1).AddMonths(1); //DateTime.Today.AddDays(-1).AddMonths(1);
                SchedulerFacade SchedulerFacade = new SchedulerFacade(org.ConnectionString);
                var AddressTemplate = SchedulerFacade.GetCustomerAddressFormat(recurringBilling.CompanyId);

                #region InvoiceInsert
                string TaxType = "Non-Tax";
                if (recurringBilling.TaxPercentage > 0)
                {
                    TaxType = "Sales Tax";
                }
                Invoice inv = new Invoice()
                {
                    CompanyId = recurringBilling.CompanyId,
                    Tax = recurringBilling.TaxAmount,
                    TotalAmount = recurringBilling.TotalBillAmount,
                    BalanceDue = recurringBilling.TotalBillAmount,
                    Amount = recurringBilling.BillAmount,
                    DiscountAmount = 0,
                    Status = "Init",
                    DueDate = DueDate,//DateTime.Today.AddDays(-1).AddMonths(1),
                    CreatedBy = CurrentUser,
                    CreatedDate = CreatedDate,
                    InvoiceDate = CreatedDate,
                    CreatedByUid = CreatedByUid,
                    LastUpdatedDate = CreatedDate,
                    LastUpdatedByUid = CreatedByUid,
                    TicketId = recurringBilling.ScheduleId,
                    LateAmount = 0,
                    IsBill = false,
                    IsEstimate = false,
                    CustomerName = customer.FirstName + " " + customer.LastName,
                    CustomerId = customer.CustomerId,
                    BillingCycle = recurringBilling.BillCycle,
                    BillingAddress = recurringBilling.BillingAddress,
                    ShippingAddress = AddressHelper.MakeCustomerAddress(customer, "ShippingAddress", AddressTemplate),
                    TaxType = TaxType,
                    Description = recurringBilling.MessageOnInvoice,
                    IsARBInvoice = true,
                    InvoiceEmailAddress = recurringBilling.EmailAddress,
                    TaxPercentage = recurringBilling.TaxPercentage,
                    PaymentType = recurringBilling.PaymentMethod
                };
                inv.Id = SchedulerFacade.InsertInvoice(inv);
                inv.InvoiceId = inv.Id.GenerateInvoiceNo();
                if (!string.IsNullOrEmpty(recurringBilling.PaymentMethod) && !string.IsNullOrWhiteSpace(recurringBilling.PaymentMethod))
                {
                    var firstTwoChars = recurringBilling.PaymentMethod.Length <= 3 ? recurringBilling.PaymentMethod : recurringBilling.PaymentMethod.Substring(0, 3);
                    if (firstTwoChars.ToLower() == "cc_") { inv.InvoiceFor = LabelHelper.InvoiceFor.CreditCard; }
                    else if (firstTwoChars.ToLower() == "ach") { inv.InvoiceFor = LabelHelper.InvoiceFor.ACH; }
                    else { inv.InvoiceFor = LabelHelper.InvoiceFor.Invoice; }
                }
                else
                {
                    inv.InvoiceFor = LabelHelper.InvoiceFor.Invoice;
                }
                if (recurringBilling.Status.ToLower() == "freetrial")
                {
                    inv.InvoiceFor = LabelHelper.InvoiceFor.CreditCard;
                }
                #endregion

                #region InvoiceItems
                var ItemsList = SchedulerFacade.GetRecurringBillingItemsByScheduleId(recurringBilling.ScheduleId);
                List<InvoiceDetail> invoiceDetailList = new List<InvoiceDetail>();
                if (ItemsList.Count > 0)
                {
                    ItemsList = ItemsList.OrderBy(x => x.Id).ToList();
                    foreach (var item in ItemsList)
                    {
                        InvoiceDetail invDet = new InvoiceDetail()
                        {
                            CompanyId = recurringBilling.CompanyId,
                            CreatedBy = CurrentUser,
                            CreatedDate = CreatedDate,
                            InvoiceId = inv.InvoiceId,
                            TotalPrice = item.Amount,
                            UnitPrice = item.Rate,
                            Quantity = item.Qty,
                            EquipName = item.ProductName,
                            EquipDetail = item.Description,
                            Taxable = item.IsTaxable
                        };
                        invoiceDetailList.Add(invDet);
                    }
                }
                #region UnpaidRecurringInvoiceItems
                if (recurringBilling.IncludeOpenInvoices)
                {
                    var InvoiceItemsList = SchedulerFacade.GetUnpaidRecurringBillingInvoiceDetailsListByCustomerId(customer.CustomerId);
                    if (InvoiceItemsList.Count > 0)
                    {
                        InvoiceItemsList = InvoiceItemsList.OrderBy(x => x.Id).ToList();

                        foreach (var details in InvoiceItemsList)
                        {
                            var Discription = "(" + details.InvoiceId + ") " + details.EquipDetail;
                            details.EquipDetail = Discription;
                            details.Id = 0;
                            details.InvoiceId = inv.InvoiceId;
                            details.CreatedBy = CurrentUser;
                            details.CreatedDate = CreatedDate;
                            invoiceDetailList.Add(details);
                        }
                    }
                }
                #endregion
                #region UnpaidOthersInvoiceItems
                if (recurringBilling.OthersUnpaidBill.HasValue && recurringBilling.OthersUnpaidBill.Value)
                {
                    var InvoiceItemsList = SchedulerFacade.GetUnpaidOthersInvoiceDetailsListByCustomerId(customer.CustomerId);
                    if (InvoiceItemsList.Count > 0)
                    {
                        InvoiceItemsList = InvoiceItemsList.OrderBy(x => x.Id).ToList();
                        foreach (var details in InvoiceItemsList)
                        {
                            var Discription = "(" + details.InvoiceId + ") " + details.EquipDetail;
                            details.EquipDetail = Discription;
                            details.Id = 0;
                            details.InvoiceId = inv.InvoiceId;
                            details.CreatedBy = CurrentUser;
                            details.CreatedDate = CreatedDate;
                            invoiceDetailList.Add(details);
                        }
                    }
                }
                #endregion
                if (invoiceDetailList.Count > 0)
                {
                    foreach (var items in invoiceDetailList)
                    {
                        SchedulerFacade.InsertInvoiceDetails(items);
                    }
                }
                #endregion
                #region Update Customer      
                customer.LastGeneratedInvoice = CreatedDate;
                customer.LastUpdatedBy = CurrentUser;
                SchedulerFacade.UpdateCustomer(customer);
                #endregion

                double TotalDueAmount = 0;
                double SubTotalAmount = 0;
                double taxAmount = 0;
                double BillAmount = 0;

                #region HideInvoice
                if (recurringBilling.IncludeOpenInvoices)
                {
                    var InvoiceList = SchedulerFacade.GetUnpaidRecurringBillingInvoiceListByCustomerId(customer.CustomerId, recurringBilling.CompanyId);
                    if (InvoiceList.Count > 0)
                    {
                        InvoiceList = InvoiceList.OrderBy(x => x.Id).ToList();
                        foreach (var invoice in InvoiceList)
                        {
                            if (invoice.Tax != null && invoice.TotalAmount != null)
                            {
                                SubTotalAmount += (double)invoice.TotalAmount;
                                taxAmount += (double)invoice.Tax;
                                BillAmount += invoice.Amount;
                                if (invoice.BalanceDue != null)
                                {
                                    TotalDueAmount += (double)invoice.BalanceDue;
                                }
                            }
                            var Discription = "(Added in invoice number : " + inv.InvoiceId + ") " + invoice.Description;
                            invoice.Status = "Rolled Over";
                            invoice.Description = Discription;
                            invoice.LastUpdatedDate = CreatedDate;
                            invoice.LastUpdatedByUid = CreatedByUid;
                            SchedulerFacade.UpdateInvoice(invoice);
                        }
                    }
                }
                if (recurringBilling.OthersUnpaidBill.HasValue && recurringBilling.OthersUnpaidBill.Value)
                {
                    var InvoiceList = SchedulerFacade.GetUnpaidOthersInvoiceListByCustomerId(customer.CustomerId, recurringBilling.CompanyId);
                    if (InvoiceList.Count > 0)
                    {
                        InvoiceList = InvoiceList.OrderBy(x => x.Id).ToList();
                        foreach (var invoice in InvoiceList)
                        {
                            if (invoice.Tax != null && invoice.TotalAmount != null)
                            {
                                SubTotalAmount += (double)invoice.TotalAmount;
                                taxAmount += (double)invoice.Tax;
                                BillAmount += invoice.Amount;
                                if (invoice.BalanceDue != null)
                                {
                                    TotalDueAmount += (double)invoice.BalanceDue;
                                }
                            }
                            var Discription = "(Added in invoice number : " + inv.InvoiceId + ") " + invoice.Description;
                            invoice.Status = "Rolled Over";
                            invoice.Description = Discription;
                            invoice.LastUpdatedDate = CreatedDate;
                            invoice.LastUpdatedByUid = CreatedByUid;
                            SchedulerFacade.UpdateInvoice(invoice);
                        }
                    }
                }
                #endregion

                inv.TotalAmount += SubTotalAmount;
                inv.BalanceDue += TotalDueAmount;
                inv.Tax += taxAmount;
                inv.Amount += BillAmount;
                inv.LateAmount += BillAmount;
                if (SubTotalAmount == TotalDueAmount)
                {
                    inv.Status = "Open";
                }
                else
                {
                    inv.Status = "Partial";
                }
                bool r = SchedulerFacade.UpdateInvoice(inv);
                if (r) { result = inv.InvoiceId; }

                #region Customer Credit
                if (recurringBilling.Status == "FreeTrial")
                {
                    string CreditNote = string.Format(@"Free Trial Invoice# <a class=""cus-anchor"" href=""javascript:void(0)"" onclick=""OpenInvById('{0}')"">{1}</a>", inv.Id, inv.InvoiceId);
                    CustomerCredit cc = new CustomerCredit()
                    {
                        Amount = Math.Round(inv.BalanceDue.Value, 2),
                        CompanyId = inv.CompanyId,
                        CreatedBy = CreatedByUid,
                        CreatedDate = CreatedDate,
                        CustomerId = inv.CustomerId,
                        TransactionId = 0,
                        Type = LabelHelper.CustomerCreditType.Credit,
                        IsRMRCredit = true,
                        Note = CreditNote
                    };
                    SchedulerFacade.InsertCustomerCredit(cc);
                }
                #endregion

                #region Next Date Change
                DateTime? NewNextDate = null;

                if (recurringBilling.BillCycle.ToLower() == "daily")
                {
                    NewNextDate = TrimStartDate.AddDays(1);
                }
                else if (recurringBilling.BillCycle.ToLower() == "one-time")
                {
                    NewNextDate = null;
                }
                else if (recurringBilling.BillCycle.ToLower() == "weekly")
                {
                    NewNextDate = TrimStartDate.AddDays(7);
                }
                else if (recurringBilling.BillCycle.ToLower() == "bi-weekly")
                {
                    NewNextDate = TrimStartDate.AddDays(14);
                }
                else if (recurringBilling.BillCycle.ToLower() == "semi-monthly")
                {
                    NewNextDate = TrimStartDate.AddDays(15);
                }
                else if (recurringBilling.BillCycle.ToLower() == "monthly")
                {
                    NewNextDate = TrimStartDate.AddMonths(1);
                }
                else if (recurringBilling.BillCycle.ToLower() == "bi-monthly")
                {
                    NewNextDate = TrimStartDate.AddMonths(2);
                }
                else if (recurringBilling.BillCycle.ToLower() == "quarterly")
                {
                    NewNextDate = TrimStartDate.AddMonths(3);
                }
                else if (recurringBilling.BillCycle.ToLower() == "semi-annually")
                {
                    NewNextDate = TrimStartDate.AddMonths(6);
                }
                else if (recurringBilling.BillCycle.ToLower() == "annually")
                {
                    NewNextDate = TrimStartDate.AddYears(1);
                }
                else
                {
                    NewNextDate = TrimStartDate.AddMonths(1);
                }

                if (recurringBilling.EndDate.HasValue && NewNextDate.HasValue)
                {
                    DateTime EndDateZeroHour = recurringBilling.EndDate.Value.SetZeroHour();
                    if (NewNextDate.Value > EndDateZeroHour)
                    {
                        NewNextDate = null;
                    }
                }

                recurringBilling.PreviousDate = recurringBilling.NextDate.HasValue ? recurringBilling.NextDate.Value : DateTime.Now;
                recurringBilling.NextDate = NewNextDate;
                recurringBilling.LastUpdatedDate = CreatedDate;
                recurringBilling.LastUpdatedBy = CreatedByUid;
                recurringBilling.LastRMRInvoiceRefId = inv.InvoiceId;
                if (NewNextDate == null) { recurringBilling.Status = "Expired"; }

                SchedulerFacade.UpdateRecurringBilling(recurringBilling);

                #endregion

                return result;
            }
            catch (Exception e)
            {
                return "";
            }

        }
        #region Email Notification Send
        private bool EmailNotificationSendForNewInvoiceCreation(Customer _Customer, Invoice _Invoice, SchedulerFacade _SchedulerFacade, string ConnectionString)
        {
            bool result = false;
            try
            {
                if (_Customer != null && _Invoice != null && !string.IsNullOrWhiteSpace(ConnectionString))
                {
                    MailFacade _MailFacade = new MailFacade();
                    CustomerSnapshotFacade _CustomerSnapshotFacade = null;
                    _MailFacade = new MailFacade(ConnectionString);
                    _CustomerSnapshotFacade = new CustomerSnapshotFacade(ConnectionString);
                    ShortUrlFacade _ShortUrlFacade = new ShortUrlFacade();
                    string CurrentUser = "System";
                    Guid CreatedByUid = new Guid("22222222-2222-2222-2222-222222222222");
                    string SalesGuy = "", CompanyName = "", SalesPhone = "", CompanyEmail = "";
                    Guid EmployeeId = new Guid();
                    #region Send Email
                    if (!string.IsNullOrWhiteSpace(_Customer.Soldby) && Guid.TryParse(_Customer.Soldby, out EmployeeId) && EmployeeId != new Guid())
                    {
                        var objemp = _SchedulerFacade.GetEmployeeByEmployeeId(EmployeeId);
                        if (objemp != null)
                        {
                            SalesGuy = objemp.FirstName + " " + objemp.LastName;
                            if (!string.IsNullOrWhiteSpace(objemp.Phone)) { SalesPhone = objemp.Phone; }
                            if (!string.IsNullOrWhiteSpace(objemp.Email)) { CompanyEmail = objemp.Email; }
                        }
                    }
                    Company tempCom = _SchedulerFacade.GetCompanyByCompanyId(_Invoice.CompanyId);
                    if (tempCom != null)
                    {
                        if (string.IsNullOrWhiteSpace(SalesPhone)) { SalesPhone = tempCom.Phone; }
                        if (string.IsNullOrWhiteSpace(CompanyEmail)) { CompanyEmail = tempCom.EmailAdress; }
                        CompanyName = tempCom.CompanyName;
                    }
                    if (string.IsNullOrWhiteSpace(SalesGuy)) { SalesGuy = CurrentUser; }
                    string CustomerName = _Customer.FirstName + " " + _Customer.LastName;
                    string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(_Invoice.Id
                                        + "#"
                                        + _Invoice.CompanyId
                                        + "#"
                                        + _Customer.CustomerId);
                    string fullurl = string.Concat(AppConfig.SiteDomain, "/System-Generated-Customer-Invoice/", encryptedurl);
                    ShortUrl ShortUrl = _ShortUrlFacade.GetSortUrlByUrl(fullurl, _Customer.CustomerId);
                    string url = AppConfig.ShortSiteDomain + "/shrt/" + ShortUrl.Code;
                    EmailTemplate EmailTemplate = _MailFacade.GetTemplateByTemplateKey("InvoicePredefineEmailTemplate");
                    Hashtable datatemplate = new Hashtable();
                    datatemplate.Add("CustomerName", CustomerName);
                    datatemplate.Add("ExpirationDate", _Invoice.DueDate);
                    datatemplate.Add("SalesPhone Number", SalesPhone);
                    datatemplate.Add("CompanyName", CompanyName);
                    datatemplate.Add("SalesGuy", SalesGuy);
                    datatemplate.Add("url", url);
                    string emailtemplateBody = HS.Web.UI.Helper.LabelHelper.ParserHelper(EmailTemplate.BodyContent, datatemplate);

                    InvoiceCreatedEmail email = new InvoiceCreatedEmail()
                    {
                        CompanyName = CompanyName,
                        CustomerName = CustomerName,
                        BalanceDue = _Invoice.TotalAmount != null ? LabelHelper.CurrentTransMakeCurrency.MakeCurrency() + _Invoice.TotalAmount.Value.ToString("0,0.00") : "0.00",
                        DueDate = _Invoice.DueDate.HasValue ? _Invoice.DueDate.Value.ToString("MM/dd/yy") : "",
                        InvoiceId = _Invoice.InvoiceId,
                        ToEmail = _Customer.EmailAddress,
                        EmailBody = emailtemplateBody,
                        Subject = string.Format("New Invoice From {0}:{1}", CompanyName, _Invoice.InvoiceId),
                        FromEmail = CompanyEmail,
                        FromName = SalesGuy
                    };
                    result = _MailFacade.SendInvoiceCreatedEmail(email, _Invoice.CompanyId);
                    if (result)
                    {
                        #region Email Log History
                        CustomerSnapshot objInvoiceEmailLog = new CustomerSnapshot()
                        {
                            CustomerId = _Customer.CustomerId,
                            CompanyId = _Invoice.CompanyId,
                            Description = "Invoice:" + "  " + _Invoice.InvoiceId + " " + "email sent by " + "<b>" + CurrentUser.ToLower() + "</b>",
                            Logdate = DateTime.Now.UTCCurrentTime(),
                            Updatedby = CurrentUser,
                            Type = "CustomerMailHistory"
                        };
                        _CustomerSnapshotFacade.InsertSnapshot(objInvoiceEmailLog);
                        CustomerAgreement objagree = new CustomerAgreement()
                        {
                            CustomerId = _Customer.CustomerId,
                            CompanyId = _Invoice.CompanyId,
                            InvoiceId = _Invoice.InvoiceId,
                            Type = LabelHelper.EstimateStatus.SentToCustomer,
                            AddedDate = DateTime.Now.UTCCurrentTime()
                        };
                        _SchedulerFacade.InsertCustomerAgreement(objagree);
                        #endregion
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                return result;
            }
            return result;

        }
        #endregion

        #endregion
    }
}