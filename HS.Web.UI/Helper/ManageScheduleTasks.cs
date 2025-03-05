using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using HS.Entities;
using HS.Facade;
using HS.Payments;
using HS.Web.UI.Helper;
using AuthorizeNet.Api.Contracts.V1;
using HS.Payments.RecurringBilling;
using HS.Payments.TransactionReporting;
using HS.Framework;
using HS.Framework.Utils;
using System.Collections;
using System.Web.Mvc;
using Rotativa;
using Rotativa.Options;
using Forte;
using Forte.Entities;
using HS.Entities.Custom;
using HS.CSM;
using HS.CSM.Models;
using System.Threading.Tasks;
using System.Net;
using EO.Internal;
using Plivo;
using System.Text.RegularExpressions;
using NLog;
using iTextSharp.text.log;

namespace HS.Web.UI.Helper
{
    public class ManageScheduleTasks
    {
        Logger logger;

        public ManageScheduleTasks()
        {
            logger = LogManager.GetCurrentClassLogger();
        }
        #region Schedulers 

        #region Test Scheduler Not functional
        #region MainSheduler
        /*
        public void StartMainShceduler()
        {
            DateTime currenttime = DateTime.Now;
            DateTime currentshcedulertime = DateTime.UtcNow;
            string eligibleToRunScheduler = ConfigurationManager.AppSettings["EligibleToRunScheduler"] != null ? Convert.ToString(ConfigurationManager.AppSettings["EligibleToRunScheduler"]) : "false";

            try
            {
                if (eligibleToRunScheduler.ToLower() == "true")
                {
                    //double dblmilisec = Convert.ToDouble("1") * 60000;
                    //System.Timers.Timer t = new System.Timers.Timer();
                    //t.Elapsed += new System.Timers.ElapsedEventHandler(CheckingShedulerRunning);
                    //t.Interval = dblmilisec;
                    //t.Enabled = true;
                    //t.AutoReset = true;
                    //t.Start();
                    var processingTask = Task.Run(() =>
                    { 
                        //CheckingShedulerRunning(currenttime, currentshcedulertime);
                    });
                }

            }
            catch (Exception ex)
            {

            }
        }
        */
        /*
        private void CheckingShedulerRunning(DateTime currenttime, DateTime currentUtctime)
        {
            //DateTime currenttime = DateTime.Now;
            string Now = currenttime.ToString("hh:mm tt");

            #region 1. SubscriptionStatusChecker  
            try
            {
                Logwrite("1. SubscriptionStatusChecker: " + currenttime.ToString("MM/dd/yyyy hh:mm tt"));
                string eligibleToRunSubscriptionStatusScheduler = ConfigurationManager.AppSettings["EligibleToRunSubscriptionStatusScheduler"] != null ? Convert.ToString(ConfigurationManager.AppSettings["EligibleToRunSubscriptionStatusScheduler"]).ToLower() : "false";

                string Start = ConfigurationManager.AppSettings["SubscriptionStatusCheckingTime"];
                var triger = Start.Equals(Now);
                if (triger && eligibleToRunSubscriptionStatusScheduler == "true")
                {
                    var processingTask = Task.Run(() =>
                    {
                        Logwrite("1. SubscriptionStatusChecker Execute: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                        SubscriptionStatusChecker(); //aync
                    });
                   
                }
            }
            catch (Exception ex)
            {
                Logwrite("1. SubscriptionStatusChecker EXCEPTION !!: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")+" EX.Message: "+ex.Message);
            }


            #endregion

            #region 2. InvoicePayments 
            try
            {
                //Logwrite("2. ChecK Invoice Payments: " + currenttime.ToString("MM/dd/yyyy hh:mm tt"));
                string Start = ConfigurationManager.AppSettings["InvoiceTransactionCheckingTime"];
                string eligibleToRunInvoicePaymentsScheduler = ConfigurationManager.AppSettings["EligibleToRunInvoicePaymentsScheduler"] != null ? Convert.ToString(ConfigurationManager.AppSettings["EligibleToRunInvoicePaymentsScheduler"]) : "false";

                var triger = Start.Equals(Now);
                if (triger && eligibleToRunInvoicePaymentsScheduler == "true")
                {
                    var processingTask = Task.Run(() =>
                    {
                        Logwrite("2. ChecK Invoice Payments Execute: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                        CheckInvoicePayments();//aync 
                    });
                }
            } catch (Exception ex)
            {
                Logwrite("2. ChecK Invoice Payments Exception!!: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt") + " EX.Message: " + ex.Message);
            }

            //Start 2. InvoiceTransactionCheckingTime
            #endregion

            #region 3. ReminderNotificationTasks
            try
            {
                //Logwrite("3. ReminderNotificationTasks: " + currenttime.ToString("MM/dd/yyyy hh:mm tt"));
                string Start = ConfigurationManager.AppSettings["ReminderNotificationClockOutTime"];
                string eligibleToRunReminderNotificationScheduler = ConfigurationManager.AppSettings["EligibleToRunReminderNotificationScheduler"] != null ? Convert.ToString(ConfigurationManager.AppSettings["EligibleToRunReminderNotificationScheduler"]) : "false";

                var triger = Start.Equals(Now);
                if (triger && eligibleToRunReminderNotificationScheduler == "true")
                {
                    var processingTask = Task.Run(() =>
                    {
                        Logwrite("3. ReminderNotificationTasks Execute: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                        ReminderNotificationTasks(); //aync
                    });
                }
            }
            catch (Exception ex)
            {
                Logwrite("3. ReminderNotificationTasks Exception: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt") + " EX.Message: " + ex.Message);
            }

            #endregion

            #region 5.SendEmailForNotSetCustomerBillingWithinWeek
            //string startThis = ConfigurationManager.AppSettings["EligibleToRunEmailForNotSetCustomerBillingScheduler"] != null ? Convert.ToString(ConfigurationManager.AppSettings["EligibleToRunEmailForNotSetCustomerBillingScheduler"]) : "false";

            #endregion

            #region 4.GenerateInvoiceScheduler
            try
            {
                //Logwrite("4. GenerateInvoiceScheduler: " + currenttime.ToString("MM/dd/yyyy hh:mm tt"));
                string Start = ConfigurationManager.AppSettings["InvoiceGenerateTime"];
                string eligibleToRunGeneratingInvoiceScheduler = ConfigurationManager.AppSettings["EligibleToRunGeneratingInvoiceScheduler"] != null ? Convert.ToString(ConfigurationManager.AppSettings["EligibleToRunGeneratingInvoiceScheduler"]) : "false";

                var triger = Start.Equals(Now);
                if (triger && eligibleToRunGeneratingInvoiceScheduler == "true")
                {
                    var processingTask = Task.Run(() =>
                    {
                        Logwrite("4. GenerateInvoiceScheduler Execute: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                        GenerateInvoice();//aync 
                    });
                }
            }
            catch (Exception ex)
            {
                Logwrite("4. GenerateInvoiceScheduler Exception!!: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt") + " EX.Message: " + ex.Message);
            }


            #endregion

            #region 7. EstimateReminderScheduler 
            try
            {
                //Logwrite("7. EstimateReminderScheduler: " + currenttime.ToString("MM/dd/yyyy hh:mm tt"));
                string Start = ConfigurationManager.AppSettings["EstimateReminderTime"];
                string eligibleToRunEstimateReminderScheduler = ConfigurationManager.AppSettings["EligibleToRunEstimateReminderScheduler"] != null ? Convert.ToString(ConfigurationManager.AppSettings["EligibleToRunEstimateReminderScheduler"]) : "false";

                bool triger = Start.Equals(Now);
                if (triger && eligibleToRunEstimateReminderScheduler == "true")
                {
                    var processingTask = Task.Run(() =>
                    {
                        EstimateReminderCall();//aync
                        Logwrite("7. EstimateReminderScheduler Execute: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                    });
                }
            }
            catch(Exception ex)
            {
                Logwrite("7. EstimateReminderScheduler Exception!!: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt") + " EX.Message: " + ex.Message);
            }

            #endregion

            #region 8. EmailReminderScheduler
            try
            {
                ///Logwrite("8. EmailReminderScheduler: " + currenttime.ToString("MM/dd/yyyy hh:mm tt"));

                var trigger = currenttime.Minute;
                string eligibleToRunEmailReminderScheduler = ConfigurationManager.AppSettings["EligibleToRunEmailReminderScheduler"] != null ? Convert.ToString(ConfigurationManager.AppSettings["EligibleToRunEmailReminderScheduler"]) : "false";

                if (eligibleToRunEmailReminderScheduler == "true" && (trigger == 00 || trigger == 15 || trigger == 30 || trigger == 45))
                {
                    var processingTask = Task.Run(() =>
                    {
                        Logwrite("8. EmailReminderScheduler Excute: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                        EmailReminder(currentUtctime);//aync 
                    });
                }
            }
            catch (Exception ex)
            {
                Logwrite("8. EmailReminderScheduler Exception: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt") + " EX.Message: " + ex.Message);
            }
            

            #endregion

            #region 10. LeadImportFromCMST 
            try
            {
                ///Logwrite("10. LeadImportFromCMST: " + currenttime.ToString("MM/dd/yyyy hh:mm tt"));
                string eligibleToRunLeadImportScheduler = ConfigurationManager.AppSettings["EligibleToRunLeadImportScheduler"] != null ? Convert.ToString(ConfigurationManager.AppSettings["EligibleToRunLeadImportScheduler"]) : "false";
                if (eligibleToRunLeadImportScheduler == "true" && currenttime.Minute % 5 == 0)//runs every 5 mins
                {
                    var processingTask = Task.Run(() =>
                    {
                        Logwrite("10. LeadImportFromCMST Execute: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                        LeadImportFromCMST();//aync 
                    });
                }
                    
            }
            catch (Exception) {
                Logwrite("10. LeadImportFromCMST EXCEPTION!!: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
            }
            #endregion

            #region 11. LateNotificationForTicket
            try
            {
                ///Logwrite("11. LateNotificationForTicket: " + currenttime.ToString("MM/dd/yyyy hh:mm tt"));
                var trigger = currenttime.Minute;
                string eligibleToRunLateNotificationForTicketScheduler = ConfigurationManager.AppSettings["EligibleToRunLateNotificationForTicketScheduler"] != null ? Convert.ToString(ConfigurationManager.AppSettings["EligibleToRunLateNotificationForTicketScheduler"]) : "false";

                if (eligibleToRunLateNotificationForTicketScheduler == "true" && trigger == 00 || trigger == 15 || trigger == 30 || trigger == 45)
                {
                    var processingTask = Task.Run(() =>
                    {
                        Logwrite("11. LateNotificationForTicket Excute: " + currenttime.ToString("MM/dd/yyyy hh:mm tt"));
                        LateNotificationTicketScheduler(DateTime.Now.ToString("yyyy-MM-dd HH:mm" + ":00.000"));//aync
                    });
                }
            }
            catch (Exception)
            {
                Logwrite("11. LateNotificationForTicket EXECEPTION!! : " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
            }

            #endregion

            #region 12. DayIncrementServiceForTicket
            try
            {
                ///Logwrite("12. DayIncrementServiceForTicket: " + currenttime.ToString("MM/dd/yyyy hh:mm tt"));
                var HourTrigger = currenttime.Hour;
                var MinuteTrigger = currenttime.Minute;
                string eligibleToRunDayIncrementServiceForTicketScheduler = ConfigurationManager.AppSettings["EligibleToRunDayIncrementServiceForTicketScheduler"] != null ? Convert.ToString(ConfigurationManager.AppSettings["EligibleToRunDayIncrementServiceForTicketScheduler"]) : "false";

                if (eligibleToRunDayIncrementServiceForTicketScheduler == "true" && HourTrigger == 23 && MinuteTrigger == 59)
                {
                    var processingTask = Task.Run(() =>
                    {
                        Logwrite("12. DayIncrementServiceForTicket Excute: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                        DayIncrementServiceTicketScheduler();//aync
                    });
                   
                }
            }
            catch(Exception)
            {
                Logwrite("12. DayIncrementServiceForTicket EXCEPTION!!: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
            }

            #endregion

            #region 14. ActivityNotificationEmailText // need to add 
            try
            {
                //Logwrite("14. ActivityNotificationEmailText: " + currenttime.ToString("MM/dd/yyyy hh:mm tt"));
                string Start = ConfigurationManager.AppSettings["ActivityNotificationStatusCheckingTime"];
                string EligibleToRunActivityNotificationEmailText = ConfigurationManager.AppSettings["EligibleToRunActivityNotificationEmailText"] != null ? Convert.ToString(ConfigurationManager.AppSettings["EligibleToRunActivityNotificationEmailText"]) : "false";
                bool triger = Start.Equals(Now);
                if (triger)
                {
                    var processingTask = Task.Run(() =>
                    {
                        Logwrite("14.ActivityNotificationEmailText Execute: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                        ActivityNotification();//aync
                    });
                    
                }
            }
            catch (Exception)
            {
                Logwrite("14.ActivityNotificationEmailText Exception!!!: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
            }

            #endregion

        }
        private void Logwrite(string content)
        {
            //using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\running.txt"), true))
            //{
            //    file.WriteLine(content);
            //    file.Close();
            //}
            //ErrorLog error = new ErrorLog();
            //error.ErrorId = Guid.NewGuid();
            //error.Message = content;
            //error.TimeUtc = DateTime.Now;
            //error.ErrorFor = "Scheduler";

            //SchedulerFacade SchedulerFacadeMaster = new SchedulerFacade();
            //SchedulerFacadeMaster.InsertErrorLog(error);
        }
        */
        #endregion

        #endregion Test Scheduler Not functional

        #region Recurring Billing Related Timers
        public void StartCheckingInvoicePayments()
        {
            try
            {
                double dblmilisec = Convert.ToDouble("1") * 60000;
                System.Timers.Timer t = new System.Timers.Timer();
                t.Elapsed += new System.Timers.ElapsedEventHandler(StartCheckingInvoicePayments);
                t.Interval = dblmilisec;
                t.Enabled = true;
                t.AutoReset = true;
                t.Start();

            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
        public void StartGeneratingInvoice()
        {
            try
            {
                double dblmilisec = Convert.ToDouble("1") * 60000;
                System.Timers.Timer t = new System.Timers.Timer();
                t.Elapsed += new System.Timers.ElapsedEventHandler(GenerateInvoiceScheduler);
                t.Interval = dblmilisec;
                t.Enabled = true;
                t.AutoReset = true;
                t.Start();
            }
            catch (Exception)
            {

            }
        }

        public void StartOrderExpirationScheduler()
        {
            try
            {
                double dblmilisec = Convert.ToDouble("1") * 60000;
                System.Timers.Timer t = new System.Timers.Timer();
                t.Elapsed += new System.Timers.ElapsedEventHandler(OrderExpirationSchedulerTrigger);
                t.Interval = dblmilisec;
                t.Enabled = true;
                t.AutoReset = true;
                t.Start();
            }
            catch (Exception)
            {

            }
        }

        public void StartTrackingNumberScheduler()
        {
            try
            {
                double dblmilisec = Convert.ToDouble("1") * 60000;
                System.Timers.Timer t = new System.Timers.Timer();
                t.Elapsed += new System.Timers.ElapsedEventHandler(TrackingNumberSchedulerTrigger);
                t.Interval = dblmilisec;
                t.Enabled = true;
                t.AutoReset = true;
                t.Start();
            }
            catch (Exception)
            {

            }
        }

        public void SchedulerRunner()
        {
            try
            {
                double dblmilisec = Convert.ToDouble("4") * 60000;
                System.Timers.Timer t = new System.Timers.Timer();
                t.Elapsed += new System.Timers.ElapsedEventHandler(SchedulerRunnerTrigger);
                t.Interval = dblmilisec;
                t.Enabled = true;
                t.AutoReset = true;
                t.Start();
            }
            catch (Exception)
            {

            }
        }
        public void StartCheckingSubscriptionStatus()
        {
            try
            {
                double dblmilisec = Convert.ToDouble("1") * 60000;
                System.Timers.Timer t = new System.Timers.Timer();
                t.Elapsed += new System.Timers.ElapsedEventHandler(StartCheckingSubscriptionStatusTrigger);
                t.Interval = dblmilisec;
                t.Enabled = true;
                t.AutoReset = true;
                t.Start();
            }
            catch (Exception)
            {

            }
        }
        #endregion

        #region Reminder Related Timers
        public void StartSendEmailForNotSetCustomerBillingWithinWeek()
        {
            try
            {
                string start = ConfigurationManager.AppSettings["EligibleToRunScheduler"] != null ? Convert.ToString(ConfigurationManager.AppSettings["EligibleToRunScheduler"]) : "false";
                string startThis = ConfigurationManager.AppSettings["EligibleToRunEmailForNotSetCustomerBillingScheduler"] != null ? Convert.ToString(ConfigurationManager.AppSettings["EligibleToRunEmailForNotSetCustomerBillingScheduler"]) : "false";
                if (start.ToLower() == "true" && startThis.ToLower() == "true")
                {
                    //double dblmilisec = ConfigurationManager.AppSettings["scheduleTimeinMinute"] != null ? Convert.ToDouble(ConfigurationManager.AppSettings["scheduleTimeinMinute"]) * 60000 : -1;
                    double dblmilisec = Convert.ToDouble("1") * 60000;
                    System.Timers.Timer t = new System.Timers.Timer();
                    t.Elapsed += new System.Timers.ElapsedEventHandler(SendEmailForNotSetCustomerBillingWithinWeek);
                    t.Interval = dblmilisec;
                    t.Enabled = true;
                    t.AutoReset = true;
                    t.Start();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
        public void StartCheckingActivityNotificationStatus()
        {
            try
            {


                string start = ConfigurationManager.AppSettings["EligibleToRunScheduler"] != null ? Convert.ToString(ConfigurationManager.AppSettings["EligibleToRunScheduler"]) : "false";
                string startThis = ConfigurationManager.AppSettings["EligibleToRunActivityNotificationScheduler"] != null ? Convert.ToString(ConfigurationManager.AppSettings["EligibleToRunActivityNotificationScheduler"]) : "false";
                if (start.ToLower() == "true" && startThis.ToLower() == "true")
                {
                    double dblmilisec = Convert.ToDouble("1") * 60000;
                    System.Timers.Timer t = new System.Timers.Timer();
                    t.Elapsed += new System.Timers.ElapsedEventHandler(StartCheckingActivityNotificationStatusTrigger);
                    t.Interval = dblmilisec;
                    t.Enabled = true;
                    t.AutoReset = true;
                    t.Start();
                }
            }
            catch (Exception)
            {

            }
        }
        public void StartSendEmailForEstimateNotConvertedWithinWeek()
        {
            try
            {
                string start = ConfigurationManager.AppSettings["EligibleToRunScheduler"] != null ? Convert.ToString(ConfigurationManager.AppSettings["EligibleToRunScheduler"]) : "false";
                if (start.ToLower() == "true")
                {
                    //Not functional
                    ////double dblmilisec = ConfigurationManager.AppSettings["scheduleTimeinMinute"] != null ? Convert.ToDouble(ConfigurationManager.AppSettings["scheduleTimeinMinute"]) * 60000 : -1;
                    //double dblmilisec = Convert.ToDouble("1") * 60000;
                    //System.Timers.Timer t = new System.Timers.Timer();
                    //t.Elapsed += new System.Timers.ElapsedEventHandler(SendEmailForEstimateNotConvertedWithinWeek);
                    //t.Interval = dblmilisec;
                    //t.Enabled = true;
                    //t.AutoReset = true;
                    //t.Start();
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
        public void EmailReminderTimmer()
        {
            try
            {
                //Need to fix email reminder first
                double dblmilisec = Convert.ToDouble("1") * 60000;
                System.Timers.Timer t = new System.Timers.Timer();
                t.Elapsed += new System.Timers.ElapsedEventHandler(EmailReminderScheduler);
                t.Interval = dblmilisec;
                t.Enabled = true;
                t.AutoReset = true;
                t.Start();

            }
            catch (Exception ex)
            {
            }
        }
        public void EstimateReminder()
        {
            try
            {
                string start = ConfigurationManager.AppSettings["EligibleToRunScheduler"] != null ? Convert.ToString(ConfigurationManager.AppSettings["EligibleToRunScheduler"]) : "false";
                string startthis = ConfigurationManager.AppSettings["EligibleToRunEstimateReminderScheduler"] != null ? Convert.ToString(ConfigurationManager.AppSettings["EligibleToRunEstimateReminderScheduler"]) : "false";
                if (start.ToLower() == "true" && startthis.ToLower() == "true")
                {
                    double dblmilisec = Convert.ToDouble("1") * 60000;
                    System.Timers.Timer t = new System.Timers.Timer();
                    t.Elapsed += new System.Timers.ElapsedEventHandler(EstimateReminderScheduler);
                    t.Interval = dblmilisec;
                    t.Enabled = true;
                    t.AutoReset = true;
                    t.Start();
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
        public void ReminderNotificationScheduler()
        {
            try
            {
                string start = ConfigurationManager.AppSettings["EligibleToRunScheduler"] != null ? Convert.ToString(ConfigurationManager.AppSettings["EligibleToRunScheduler"]) : "false";
                string startthis = ConfigurationManager.AppSettings["EligibleToRunReminderNotificationScheduler"] != null ? Convert.ToString(ConfigurationManager.AppSettings["EligibleToRunReminderNotificationScheduler"]) : "false";
                if (start.ToLower() == "true" && startthis.ToLower() == "true")
                {
                    double dblmilisec = Convert.ToDouble("1") * 60000;
                    System.Timers.Timer t = new System.Timers.Timer();
                    t.Elapsed += new System.Timers.ElapsedEventHandler(ReminderNotificationTrigger);
                    t.Interval = dblmilisec;
                    t.Enabled = true;
                    t.AutoReset = true;
                    t.Start();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
        public void StartCheckingLateNotificationForTicket()
        {
            try
            {
                string start = ConfigurationManager.AppSettings["EligibleToRunScheduler"] != null ? Convert.ToString(ConfigurationManager.AppSettings["EligibleToRunScheduler"]) : "false";
                string startthis = ConfigurationManager.AppSettings["EligibleToRunLateNotificationForTicketScheduler"] != null ? Convert.ToString(ConfigurationManager.AppSettings["EligibleToRunLateNotificationForTicketScheduler"]) : "false";
                if (start.ToLower() == "true" && startthis.ToLower() == "true")
                {
                    double dblmilisec = Convert.ToDouble("1") * 60000;
                    System.Timers.Timer t = new System.Timers.Timer();
                    t.Elapsed += new System.Timers.ElapsedEventHandler(StartCheckingLateNotificationForTicketTrigger);
                    t.Interval = dblmilisec;
                    t.Enabled = true;
                    t.AutoReset = true;
                    t.Start();
                }
            }
            catch (Exception)
            {

            }
        }



        //public void StartSendEmailForNotConvertedToCustomerWithinWeek()
        //{
        //    try
        //    {
        //        //double dblmilisec = ConfigurationManager.AppSettings["scheduleTimeinMinute"] != null ? Convert.ToDouble(ConfigurationManager.AppSettings["scheduleTimeinMinute"]) * 60000 : -1;
        //        double dblmilisec = Convert.ToDouble("1") * 60000;
        //        System.Timers.Timer t = new System.Timers.Timer();
        //        t.Elapsed += new System.Timers.ElapsedEventHandler(SendEmailForNotConvertedToCustomerWithinWeek);
        //        t.Interval = dblmilisec;
        //        t.Enabled = true;
        //        t.AutoReset = true;
        //        t.Start();
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        #endregion

        #region Other Timers
        public void CustomerCancellationScheduler()
        {
            try
            {
                string start = ConfigurationManager.AppSettings["EligibleToRunScheduler"] != null ? Convert.ToString(ConfigurationManager.AppSettings["EligibleToRunScheduler"]) : "false";
                string startthis = ConfigurationManager.AppSettings["EligibleToRunCustomerCancellationScheduler"] != null ? Convert.ToString(ConfigurationManager.AppSettings["EligibleToRunCustomerCancellationScheduler"]) : "false";
                if (start.ToLower() == "true" && startthis.ToLower() == "true")
                {
                    double dblmilisec = Convert.ToDouble("1") * 60000;
                    System.Timers.Timer t = new System.Timers.Timer();
                    t.Elapsed += new System.Timers.ElapsedEventHandler(CustomerCancellationTrigger);
                    t.Interval = dblmilisec;
                    t.Enabled = true;
                    t.AutoReset = true;
                    t.Start();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public void StartCheckingLeadImportFromCMS()
        {
            try
            {
                string start = ConfigurationManager.AppSettings["EligibleToRunScheduler"] != null ? Convert.ToString(ConfigurationManager.AppSettings["EligibleToRunScheduler"]) : "false";
                string startthis = ConfigurationManager.AppSettings["EligibleToRunLeadImportScheduler"] != null ? Convert.ToString(ConfigurationManager.AppSettings["EligibleToRunLeadImportScheduler"]) : "false";
                if (start.ToLower() == "true" && startthis.ToLower() == "true")
                {
                    double dblmilisec = Convert.ToDouble("1") * 60000 * 5;
                    System.Timers.Timer t = new System.Timers.Timer();
                    t.Elapsed += new System.Timers.ElapsedEventHandler(StartCheckingLeadImportFromCMSTrigger);
                    t.Interval = dblmilisec;
                    t.Enabled = true;
                    t.AutoReset = true;
                    t.Start();
                }
            }
            catch (Exception)
            {

            }
        }

        public void StartCheckingDayIncrementServiceForTicket()
        {
            try
            {
                string start = ConfigurationManager.AppSettings["EligibleToRunScheduler"] != null ? Convert.ToString(ConfigurationManager.AppSettings["EligibleToRunScheduler"]) : "false";
                string startthis = ConfigurationManager.AppSettings["EligibleToRunDayIncrementServiceForTicketScheduler"] != null ? Convert.ToString(ConfigurationManager.AppSettings["EligibleToRunDayIncrementServiceForTicketScheduler"]) : "false";
                if (start.ToLower() == "true" && startthis.ToLower() == "true")
                {
                    double dblmilisec = Convert.ToDouble("1") * 60000;
                    System.Timers.Timer t = new System.Timers.Timer();
                    t.Elapsed += new System.Timers.ElapsedEventHandler(StartCheckingDayIncrementServiceForTicketTrigger);
                    t.Interval = dblmilisec;
                    t.Enabled = true;
                    t.AutoReset = true;
                    t.Start();
                }
            }
            catch (Exception)
            {

            }
        }

        public void SendEvaluationRemainderEmail()
        {
            try
            {
                DateTime currenttime = DateTime.Now;
                string Now = currenttime.ToString("hh:mm tt");
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\1.txt"), true))
                {
                    file.WriteLine("EvaluationRemainderChecker: " + currenttime.ToString("MM/dd/yyyy hh:mm tt"));
                    file.Close();
                }
                string Start = ConfigurationManager.AppSettings["EvaluationRemainderTime"];
                var triger = Start.Equals(Now);


                if (triger)
                {
                    double dblmilisec = Convert.ToDouble("1") * 60000;
                    System.Timers.Timer t = new System.Timers.Timer();
                    t.Elapsed += new System.Timers.ElapsedEventHandler(SendEvaluationRemainderEmailTrigger);
                    t.Interval = dblmilisec;
                    t.Enabled = true;
                    t.AutoReset = true;
                    t.Start();
                }
            }
            catch (Exception)
            {

            }
        }
        public void CustomerUserXCalculationScheduler()
        {
            try
            {
                string start = ConfigurationManager.AppSettings["EligibleToRunScheduler"] != null ? Convert.ToString(ConfigurationManager.AppSettings["EligibleToRunScheduler"]) : "false";
                string startthis = ConfigurationManager.AppSettings["EligibleToRunUserXCalculation"] != null ? Convert.ToString(ConfigurationManager.AppSettings["EligibleToRunUserXCalculation"]) : "false";

                if (start.ToLower() == "true" && startthis.ToLower() == "true")
                {
                    double dblmilisec = Convert.ToDouble("1") * 60000;
                    System.Timers.Timer t = new System.Timers.Timer();
                    t.Elapsed += new System.Timers.ElapsedEventHandler(CustomerUserXTrigger);
                    t.Interval = dblmilisec;
                    t.Enabled = true;
                    t.AutoReset = true;
                    t.Start();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        #endregion

        #region Employee PTO Hours Calculation Scheduler Timers       
        public void EmployeePTOHoursCalculationScheduler()
        {
            try
            {
                string start = ConfigurationManager.AppSettings["EligibleToRunScheduler"] != null ? Convert.ToString(ConfigurationManager.AppSettings["EligibleToRunScheduler"]) : "false";
                string startthis = ConfigurationManager.AppSettings["EligibleToRunEmployeePTOHoursCalculationScheduler"] != null ? Convert.ToString(ConfigurationManager.AppSettings["EligibleToRunEmployeePTOHoursCalculationScheduler"]) : "false";

                if (start.ToLower() == "true" && startthis.ToLower() == "true")
                {
                    double dblmilisec = Convert.ToDouble("1") * 60000;
                    System.Timers.Timer t = new System.Timers.Timer();
                    t.Elapsed += new System.Timers.ElapsedEventHandler(EmployeePTOHoursCalculationTimer);
                    t.Interval = dblmilisec;
                    t.Enabled = true;
                    t.AutoReset = true;
                    t.Start();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        #endregion
        #endregion

        #region Scheduler Triggers

        //0.Scheduler Runner
        #region  SchedulerRunnerTrigger
        private void SchedulerRunnerTrigger(object sender, System.Timers.ElapsedEventArgs e)
        {
            WebClient client = new WebClient();
            client.DownloadData(Framework.Utils.AppConfig.SiteDomain + "/Home/Croncall");
            DateTime currenttime = DateTime.Now;
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\0.SchedulerRunner.txt"), true))
            {
                file.WriteLine(Framework.Utils.AppConfig.SiteDomain + " : Fire scheduler: " + currenttime.ToString("dd/MM/yyyy hh:mm:ss.fff"));
                file.Close();
            }
        }
        #endregion


        //1.SubscriptionStatusChecker
        private void StartCheckingSubscriptionStatusTrigger(object sender, System.Timers.ElapsedEventArgs e)
        {
            DateTime currenttime = DateTime.Now;
            string Now = currenttime.ToString("hh:mm tt");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\1.txt"), true))
            {
                file.WriteLine("SubscriptionStatusChecker: " + currenttime.ToString("MM/dd/yyyy hh:mm tt"));
                file.Close();
            }
            string Start = ConfigurationManager.AppSettings["SubscriptionStatusCheckingTime"];
            var triger = Start.Equals(Now);
            if (triger)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\1.txt"), true))
                {
                    file.WriteLine(DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                    file.Close();
                }
                SubscriptionStatusChecker();
            }
        }
        //2.ChecK Invoice Payments
        private void StartCheckingInvoicePayments(object sender, System.Timers.ElapsedEventArgs e)
        {
            DateTime currenttime = DateTime.Now;
            string Now = currenttime.ToString("hh:mm tt");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\2.txt"), true))
            {
                file.WriteLine("ChecK Invoice Payments: " + currenttime.ToString("MM/dd/yyyy hh:mm tt"));
                file.Close();
            }
            string start = ConfigurationManager.AppSettings["InvoiceTransactionCheckingTime"];
            var triger = start.Equals(Now);
            if (triger)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\2.txt"), true))
                {
                    file.WriteLine(DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                    file.Close();
                }
                CheckInvoicePayments();
            }
        }
        //3.ReminderNotificationTasks 
        private void ReminderNotificationTrigger(object sender, System.Timers.ElapsedEventArgs e)
        {
            DateTime currenttime = DateTime.Now;
            string Now = currenttime.ToString("hh:mm tt");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\3.txt"), true))
            {
                file.WriteLine("ReminderNotificationTasks: " + currenttime.ToString("MM/dd/yyyy hh:mm tt"));
                file.Close();
            }
            string start = ConfigurationManager.AppSettings["ReminderNotificationClockOutTime"];
            var triger = start.Equals(Now);
            if (triger)
            {

                ReminderNotificationTasks();
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\3.txt"), true))
                {
                    file.WriteLine(DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                    file.Close();
                }
            }
        }
        //CustomerCancellationTrigger
        private void CustomerCancellationTrigger(object sender, System.Timers.ElapsedEventArgs e)
        {
            DateTime currenttime = DateTime.Now;
            string Now = currenttime.ToString("hh:mm tt");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\4.txt"), true))
            {
                file.WriteLine("SubscriptionStatusChecker: " + currenttime.ToString("MM/dd/yyyy hh:mm tt"));
                file.Close();
            }
            string start = ConfigurationManager.AppSettings["CustomerCancellationTime"];
            var triger = start.Equals(Now);
            if (triger)
            {

                CustomerCancellationTask();
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\4.txt"), true))
                {
                    file.WriteLine(DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                    file.Close();
                }
            }
        }
        //4.GenerateInvoice
        private void GenerateInvoiceScheduler(object sender, System.Timers.ElapsedEventArgs e)
        {
            DateTime currenttime = DateTime.Now;
            string Now = currenttime.ToString("hh:mm tt");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\5.txt"), true))
            {
                file.WriteLine("GenerateInvoiceScheduler: " + currenttime.ToString("MM/dd/yyyy hh:mm tt"));
                file.Close();
            }
            string start = ConfigurationManager.AppSettings["InvoiceGenerateTime"];
            var triger = start.Equals(Now);
            if (triger)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\5.txt"), true))
                {
                    file.WriteLine(DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                    file.Close();
                }
                GenerateInvoice();
            }
        }
        //7.EstimateReminderCall
        private void EstimateReminderScheduler(object sender, System.Timers.ElapsedEventArgs e)
        {
            DateTime currenttime = DateTime.Now;
            string Now = currenttime.ToString("hh:mm tt");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\7.txt"), true))
            {
                file.WriteLine("EstimateReminderScheduler: " + currenttime.ToString("MM/dd/yyyy hh:mm tt"));
                file.Close();
            }
            string start = ConfigurationManager.AppSettings["EstimateReminderTime"];
            var triger = start.Equals(Now);
            if (triger)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\7.txt"), true))
                {
                    file.WriteLine(DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                    file.Close();
                }
                EstimateReminderCall();
            }
        }

        //8.EmailReminder
        private void EmailReminderScheduler(object sender, System.Timers.ElapsedEventArgs e)
        {
            var date = DateTime.UtcNow;
            DateTime currenttime = DateTime.Now;


            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\running_Task_timmer.txt"), true))
            {
                file.WriteLine(Framework.Utils.AppConfig.SiteDomain + " : Fire scheduler: " + currenttime.ToString("dd/MM/yyyy hh:mm:ss.fff"));
                file.Close();
            }

            string Now = currenttime.ToString("hh:mm tt");
            var trigger = date.Minute;
            if (trigger == 00 || trigger == 15 || trigger == 30 || trigger == 45)
            {
                EmailReminder(date);
            }

        }
        //9.Activity Notification Email
        private void StartCheckingActivityNotificationStatusTrigger(object sender, System.Timers.ElapsedEventArgs e)
        {
            DateTime date = DateTime.UtcNow;
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\9.txt"), true))
            {
                file.WriteLine("Activity Notification Email: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                file.Close();
            }
            var trigger = date.Minute;
            if (trigger == 00 || trigger == 15 || trigger == 30 || trigger == 45)
            {
                ActivityNotification(date);
            }
        }

        //10.Leads Data Sync From CSM

        private void StartCheckingLeadImportFromCMSTrigger(object sender, System.Timers.ElapsedEventArgs e)
        {
            //string start = ConfigurationManager.AppSettings["ActivityNotificationStatusCheckingTime"];
            //var triger = start.Equals(DateTime.Now.ToString("hh:mm tt"));
            //if (triger)
            //{
            //    using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\9.txt"), true))
            //    {
            //        file.WriteLine(DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
            //    }
            //    ActivityNotification();
            //}
            DateTime currenttime = DateTime.Now;
            string Now = currenttime.ToString("hh:mm tt");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\10.txt"), true))
            {
                file.WriteLine("LeadImportFromCMST: " + currenttime.ToString("MM/dd/yyyy hh:mm tt"));
                file.Close();
            }
            LeadImportFromCMST();

        }

        private void StartCheckingLateNotificationForTicketTrigger(object sender, System.Timers.ElapsedEventArgs e)
        {
            var date = DateTime.Now;
            var trigger = date.Minute;
            if (trigger == 00 || trigger == 15 || trigger == 30 || trigger == 45)
            {
                LateNotificationTicketScheduler(date.ToString("yyyy-MM-dd HH:mm" + ":00.000"));
            }
        }

        private void StartCheckingDayIncrementServiceForTicketTrigger(object sender, System.Timers.ElapsedEventArgs e)
        {
            var date = DateTime.Now;
            var HourTrigger = date.Hour;
            var MinuteTrigger = date.Minute;
            if (HourTrigger == 23 && MinuteTrigger == 59)
            {
                DayIncrementServiceTicketScheduler();
            }
        }
        private void SendEvaluationRemainderEmailTrigger(object sender, System.Timers.ElapsedEventArgs e)
        {
            var date = DateTime.Now;
            var HourTrigger = date.Hour;
            var MinuteTrigger = date.Minute;
            //if (HourTrigger == 23 && MinuteTrigger == 59)
            //{
            SendEvaluationRemainderEmailScheduler();
            //}
        }
        private void CustomerUserXTrigger(object sender, System.Timers.ElapsedEventArgs e)
        {
            DateTime currenttime = DateTime.Now;
            string Now = currenttime.ToString("hh:mm tt");
            string NowDay = currenttime.DayOfWeek.ToString();
            string start = ConfigurationManager.AppSettings["CustomerUserXTime"];
            string startDay = ConfigurationManager.AppSettings["CustomerUserXDay"];
            var triger = start.Equals(Now);
            var triger2 = startDay.Equals(NowDay);
            if (triger && triger2)
            {
                CustomerUserXTask();
            }
        }
        private void OrderExpirationSchedulerTrigger(object sender, System.Timers.ElapsedEventArgs e)
        {
            OrderExpirationScheduler();
            var datetime = DateTime.Now;
            if (datetime.ToString("HH:mm") == "23:59")
            {
                IeateryCouponCodeExpire(datetime.ToString("MM/dd/yyyy"));
            }
            IeateryAutoConfirmOrder();
        }

        private void TrackingNumberSchedulerTrigger(object sender, System.Timers.ElapsedEventArgs e)
        {
            IeateryTrackingNumberRecord();
        }
        #endregion

        #region Scheduler Tasks

        #region 1.SubscriptionStatusChecker
        private void SubscriptionStatusChecker()
        {
            #region SchedulerRunLog
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\SchedulerStartLogs.txt"), true))
                {
                    file.WriteLine(string.Format("1.SubscriptionStatusChecker run on {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")));

                }
            }
            catch (Exception) { }

            #endregion


            SchedulerFacade SchedulerFacadeMaster = new SchedulerFacade();
            SchedulerFacade SchedulerFacade = null;
            List<Organization> OrgList = SchedulerFacadeMaster.GetAllOrganizations();
            foreach (var org in OrgList)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\SubscriptionStatusReport.txt"), true))
                {
                    file.WriteLine("*****************************************************************************************");
                    file.WriteLine(string.Format("Starting For Company: #{0} at {1}", org.CompanyName, DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")));

                }

                Guid CompanyId = org.CompanyId;
                SchedulerFacade = new SchedulerFacade(org.ConnectionString);


                GlobalSetting PaymentGetway = SchedulerFacade.GetGlobalsettingBySearchKeyAndCompanyId("PaymentGetway", CompanyId);
                if (PaymentGetway != null && PaymentGetway.Value.ToLower() == "forte")
                {

                }
                else if (PaymentGetway != null && PaymentGetway.Value.ToLower() == "authorize.net")
                {
                    SubscriptionStatusCheckerAuthorizeNet(SchedulerFacade, CompanyId);
                }
            }
        }
        private void SubscriptionStatusCheckerAuthorizeNet(SchedulerFacade SchedulerFacade, Guid CompanyId)
        {
            #region For Authorize.net
            bool AuthorizeInProduction = false;
            GlobalSetting globset2 = SchedulerFacade.GetGlobalsettingBySearchKeyAndCompanyId("Authorize.NetInProduction", CompanyId);
            if (globset2 != null && globset2.Value.ToLower() == "true")
            {
                AuthorizeInProduction = true;
            }
            string ApiLoginIdCC = /*"7xZCH5KsE2e"*/SchedulerFacade.GetAuthAPILoginIdByCompanyId(CompanyId, false);
            string TransactionKeyCC = /*"232t7YP63K25tnN7";*/SchedulerFacade.GetAuthTransactionKeyByCompanyId(CompanyId, false);

            string ApiLoginACH = /*"7k29Xb2ueQ7";*/SchedulerFacade.GetAuthAPILoginIdByCompanyId(CompanyId, true);
            string TransactionKeyACH = /*"8Nm8Jg7eu7P2T8V4";*/SchedulerFacade.GetAuthTransactionKeyByCompanyId(CompanyId, true);
            string TaxPercentage = SchedulerFacade.GetSalesTax(CompanyId);


            List<Customer> SubscribedCustomer = SchedulerFacade.GetSubscribedAllCustomer(true);

            string trKey = "";
            string ApiId = "";

            foreach (Customer customer in SubscribedCustomer)
            {
                trKey = TransactionKeyACH;
                ApiId = ApiLoginACH;
                ARBGetSubscriptionResponse GetSub = GetSubscription.Run(ApiId, trKey, customer.AuthorizeRefId, AuthorizeInProduction);
                if (GetSub == null || GetSub.subscription == null)
                {
                    //trying with CC
                    trKey = TransactionKeyCC;
                    ApiId = ApiLoginIdCC;
                    GetSub = GetSubscription.Run(ApiId, trKey, customer.AuthorizeRefId, AuthorizeInProduction);
                }

                if (GetSub != null && GetSub.subscription != null)
                {
                    customer.SubscriptionStatus = GetSub.subscription.status.ToString();

                    #region Bill Amount And MonitoringFee 
                    if (customer.BillTax.HasValue && customer.BillTax == true)
                    {
                        #region Tax Calculation
                        double MonitoringFee = (double)GetSub.subscription.amount;
                        double TaxAmount = 0;


                        MonitoringFee = ((100 * MonitoringFee) / (100 + (Convert.ToDouble(TaxPercentage))));
                        TaxAmount = (double)GetSub.subscription.amount - MonitoringFee;

                        #endregion
                        customer.BillAmount = (double)GetSub.subscription.amount;
                        customer.MonthlyMonitoringFee = MonitoringFee.ToString();

                    }
                    else
                    {
                        customer.BillTax = false;
                        customer.BillAmount = (double)GetSub.subscription.amount;
                        customer.MonthlyMonitoringFee = GetSub.subscription.amount.ToString();
                    }
                    #endregion

                    if (GetSub.subscription.profile != null)
                    {
                        customer.AuthorizeCusProfileId = GetSub.subscription.profile.customerProfileId;
                        customer.AuthorizeDescription = GetSub.subscription.profile.description;
                        if (GetSub.subscription.profile.paymentProfile != null)
                        {
                            customer.AuthorizeCusPaymentProfileId = GetSub.subscription.profile.paymentProfile.customerPaymentProfileId;

                            if (GetSub.subscription.profile.paymentProfile.payment != null
                                && GetSub.subscription.profile.paymentProfile.payment.Item != null)
                            {
                                Type type = GetSub.subscription.profile.paymentProfile.payment.Item.GetType();
                                if (type == typeof(creditCardMaskedType))
                                {
                                    customer.PaymentMethod = LabelHelper.PaymentMethod.CreditCard;
                                }
                                if (type == typeof(bankAccountMaskedType))
                                {
                                    customer.PaymentMethod = LabelHelper.PaymentMethod.ACH;
                                }

                                //object item = response.subscription.profile.paymentProfile.payment.Item;
                                //object asd = ne
                                //item.GetType().GetProperty("cardNumber").GetValue(asd);
                                //var test2 = item.GetType().GetProperty("cardNumber");

                                //creditCardMaskedType cardMaskedTyp

                                //var card = Convert.ChangeType(item, typeof(PaymentMethodCard));
                            }

                            //response.subscription.profile.paymentProfile.payment.Item

                        }
                    }
                    if (GetSub.subscription.paymentSchedule != null)
                    {
                        customer.FirstBilling = GetSub.subscription.paymentSchedule.startDate;
                        if (GetSub.subscription.paymentSchedule.interval.unit == ARBSubscriptionUnitEnum.months)
                        {
                            if (GetSub.subscription.paymentSchedule.interval.length == 1)
                            {
                                customer.BillCycle = LabelHelper.BillCycle.Monthly;
                            }
                            else if (GetSub.subscription.paymentSchedule.interval.length == 12)
                            {
                                customer.BillCycle = LabelHelper.BillCycle.Annually;
                            }
                            else if (GetSub.subscription.paymentSchedule.interval.length == 6)
                            {
                                customer.BillCycle = LabelHelper.BillCycle.SemiAnnual;
                            }
                            else if (GetSub.subscription.paymentSchedule.interval.length == 3)
                            {
                                customer.BillCycle = LabelHelper.BillCycle.Quarterly;
                            }
                        }
                    }

                    SchedulerFacade.UpdateCustomer(customer);

                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\SubscriptionStatusReport.txt"), true))
                    {
                        file.WriteLine("Customer ID : {0} Is :{1}", customer.Id, GetSub.subscription.status);
                    }
                }


                /*
                else if (item.PaymentMethod == LabelHelper.PaymentMethod.CreditCard)
                {
                    trKey = TransactionKeyCC;
                    ApiId = ApiLoginIdCC;
                    ARBGetSubscriptionResponse GetSub = GetSubscription.Run(ApiId, trKey, item.AuthorizeRefId, AuthorizeInProduction);
                    if (GetSub == null || GetSub.subscription == null)
                    {
                        //trying with ach
                        trKey = TransactionKeyACH;
                        ApiId = ApiLoginACH;
                        GetSub = GetSubscription.Run(ApiId, trKey, item.AuthorizeRefId, AuthorizeInProduction);
                    }
                    if (GetSub != null && GetSub.subscription != null)
                    {
                        if (item.SubscriptionStatus != GetSub.subscription.status.ToString())
                        {
                            item.SubscriptionStatus = GetSub.subscription.status.ToString();
                            SchedulerFacade.UpdateCustomer(item);
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\SubscriptionStatusReport.txt"), true))
                            {
                                file.WriteLine("Customer ID : {0} Is :{1}", item.Id, GetSub.subscription.status);
                            }
                        }
                    }

                }*/
            }
            #endregion
        }
        #endregion

        #region 2.ChecK Invoice Payments
        private void CheckInvoicePayments()
        {
            #region SchedulerRunLog
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\SchedulerStartLogs.txt"), true))
                {
                    file.WriteLine(string.Format("2.CheckInvoicePayments run on {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")));

                }
            }
            catch (Exception) { }

            #endregion

            //string transactionId = "";
            string CurrentUser = "System";

            SchedulerFacade SchedulerFacadeMaster = new SchedulerFacade();
            SchedulerFacade SchedulerFacade = null;

            List<Organization> OrgList = SchedulerFacadeMaster.GetAllOrganizations();

            #region Directory for saving reports
            string subPath = "~/SchedulerReports"; // your code goes here

            bool exists = System.IO.Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath(subPath));
            if (!exists)
                System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath(subPath));

            #endregion

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\CheckPaymentReport.txt"), true))
            {
                file.WriteLine("CheckInvoicePayments Scheduler Started at {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
            }
            foreach (var org in OrgList)
            {
                try
                {

                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\CheckPaymentReport.txt"), true))
                    {
                        file.WriteLine("***********************{0}*****************************", org.CompanyName);
                    }

                    Guid CompanyId = org.CompanyId;
                    SchedulerFacade = new SchedulerFacade(org.ConnectionString);

                    string TaxPercentage = SchedulerFacade.GetSalesTax(CompanyId);
                    string CompanyBillingStr = "Alarm Monitoring Fee {0}";

                    #region Values From Global setting
                    //We will not check for payments if Our new billing system is turned on.
                    GlobalSetting RecurringBillingEnabled = SchedulerFacade.GetGlobalsettingBySearchKeyAndCompanyId("ACH-CC-RecurringBillingEnabled", CompanyId);
                    if (RecurringBillingEnabled != null && RecurringBillingEnabled.Value.ToLower() == "true")
                    {
                        continue;
                    }

                    GlobalSetting globset = SchedulerFacade.GetGlobalsettingBySearchKeyAndCompanyId("ARBInvoiceMessage", CompanyId);
                    if (globset != null)
                    {
                        CompanyBillingStr = globset.Value + " {0}";
                        globset = null;
                    }

                    //bool ForteInProduction = false;
                    //GlobalSetting ForteGlobset = SchedulerFacade.GetGlobalsettingBySearchKeyAndCompanyId("ForteInProduction", CompanyId);
                    //if (globset != null && globset.Value.ToLower() == "true")
                    //{
                    //    ForteInProduction = true;
                    //}
                    #endregion

                    #region Start Checking

                    #region Credit Card Check Day ready
                    //Process for credit card
                    List<int> CCCheckingDay = new List<int>();
                    globset = SchedulerFacade.GetGlobalsettingBySearchKeyAndCompanyId("DateForCheckCCPayments", CompanyId);
                    if (globset != null)
                    {
                        int CCDay;
                        if (globset.Value.IndexOf(';') > 0)
                        {
                            var CCDays = globset.Value.Split(';');
                            foreach (var da in CCDays)
                            {
                                if (int.TryParse(da, out CCDay))
                                {
                                    CCCheckingDay.Add(CCDay);
                                }
                            }
                        }
                        else if (globset.Value.IndexOf(',') > 0)
                        {
                            var CCDays = globset.Value.Split(',');
                            foreach (var da in CCDays)
                            {
                                if (int.TryParse(da, out CCDay))
                                {
                                    CCCheckingDay.Add(CCDay);
                                }
                            }
                        }
                        else
                        {
                            if (int.TryParse(globset.Value, out CCDay))
                            {
                                CCCheckingDay.Add(CCDay);
                            }
                        }
                        globset = null;
                    }
                    #endregion

                    #region ACH  Check Day ready
                    //Process for ach
                    List<int> ACHCchekingDay = new List<int>();
                    globset = SchedulerFacade.GetGlobalsettingBySearchKeyAndCompanyId("DateForCheckACHPayments", CompanyId);
                    GlobalSetting paymentGetway = SchedulerFacade.GetGlobalsettingBySearchKeyAndCompanyId("PaymentGetway", CompanyId);
                    if (globset != null)
                    {
                        //int.TryParse(globset.Value, out ACHCchekingDay);
                        //globset = null;

                        int ACHDay;
                        if (globset.Value.IndexOf(';') > 0)
                        {
                            var CCDays = globset.Value.Split(';');
                            foreach (var da in CCDays)
                            {
                                if (int.TryParse(da, out ACHDay))
                                {
                                    ACHCchekingDay.Add(ACHDay);
                                }
                            }
                        }
                        else if (globset.Value.IndexOf(',') > 0)
                        {
                            var CCDays = globset.Value.Split(',');
                            foreach (var da in CCDays)
                            {
                                if (int.TryParse(da, out ACHDay))
                                {
                                    ACHCchekingDay.Add(ACHDay);
                                }
                            }
                        }
                        else
                        {
                            if (int.TryParse(globset.Value, out ACHDay))
                            {
                                ACHCchekingDay.Add(ACHDay);
                            }
                        }
                        globset = null;
                    }
                    #endregion


                    if (paymentGetway.Value == LabelHelper.PaymentGetway.Authorize)
                    {
                        AuthorizePayment(CompanyId, SchedulerFacade, CurrentUser, CompanyBillingStr, TaxPercentage, org, ACHCchekingDay, CCCheckingDay);
                    }
                    else
                    {
                        FortePayment(CompanyId, SchedulerFacade, CurrentUser, CompanyBillingStr, TaxPercentage, org, ACHCchekingDay, CCCheckingDay);
                    }


                    #endregion
                }
                catch (Exception ex)
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\CheckPaymentReport.txt"), true))
                    {
                        file.WriteLine("****************************************************");
                        file.WriteLine(string.Format("Exception from company: {0}", org.CompanyName));
                        file.WriteLine("****************************************************");
                    }
                }
            }
        }

        private void AuthorizePayment(Guid CompanyId, SchedulerFacade SchedulerFacade, string CurrentUser, string CompanyBillingStr, string TaxPercentage, Organization org, List<int> ACHCchekingDay, List<int> CCCheckingDay)
        {
            GlobalSetting globset;

            #region Only for Authorize.net
            /**
             *These days used for data pulling 
             *
             */
            globset = SchedulerFacade.GetGlobalsettingBySearchKeyAndCompanyId("TransactionPullDayCountACH", CompanyId);
            int GetTransactionsForDaysACH = 7;
            if (globset != null)
            {
                int.TryParse(globset.Value, out GetTransactionsForDaysACH);
                globset = null;
            }
            int GetTransactionsForDaysCC = 7;
            globset = SchedulerFacade.GetGlobalsettingBySearchKeyAndCompanyId("TransactionPullDayCountCC", CompanyId);
            if (globset != null)
            {
                int.TryParse(globset.Value, out GetTransactionsForDaysCC);
                globset = null;
            }
            bool AuthInProduction = false;
            GlobalSetting globset2 = SchedulerFacade.GetGlobalsettingBySearchKeyAndCompanyId("Authorize.NetInProduction", CompanyId);
            if (globset2 != null && globset2.Value.ToLower() == "true")
            {
                AuthInProduction = true;
            }
            #endregion

            #region CC Payments
            if (CCCheckingDay != null && CCCheckingDay.Count() > 0 && CCCheckingDay.IndexOf(DateTime.Now.Day) > -1)
            {
                string TransactionKey = SchedulerFacade.GetAuthTransactionKeyByCompanyId(CompanyId, false);
                string APILoginId = SchedulerFacade.GetAuthAPILoginIdByCompanyId(CompanyId, false);
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\CheckPaymentReport.txt"), true))
                {
                    file.WriteLine(string.Format("Start Checking CC Payments; checking day = {0}", DateTime.Today.Day));
                    file.WriteLine(string.Format("Payments Time Period; From:{0},To:{1}", DateTime.Today.Subtract(TimeSpan.FromDays(GetTransactionsForDaysCC)), DateTime.Now.AddDays(1)));
                    file.WriteLine("****************************************************");
                }
                getSettledBatchListResponse BatchListResponse = GetSettledBatchList.Run(APILoginId, TransactionKey, GetTransactionsForDaysCC, AuthInProduction);
                if (BatchListResponse != null && BatchListResponse.batchList == null && BatchListResponse.messages != null)
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\CheckPaymentReport.txt"), true))
                    {
                        file.WriteLine("Message:{1} {0}", BatchListResponse.messages.message[0].text, BatchListResponse.messages.message[0].code);
                    }
                }
                BatchListProcessing(BatchListResponse, APILoginId, TransactionKey, CurrentUser, CompanyBillingStr, CompanyId, TaxPercentage, SchedulerFacade, AuthInProduction, false, org.CompanyName.ReplaceSpecialChar());

            }
            #endregion

            #region ACHPayments
            if (ACHCchekingDay != null && ACHCchekingDay.Count() > 0 && ACHCchekingDay.IndexOf(DateTime.Now.Day) > -1)
            {
                string TransactionKeyACH = SchedulerFacade.GetAuthTransactionKeyByCompanyId(CompanyId, true);
                string APILoginIdACH = SchedulerFacade.GetAuthAPILoginIdByCompanyId(CompanyId, true);

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\CheckPaymentReport.txt"), true))
                {
                    file.WriteLine(string.Format("Start Checking ACH Payments; checking day = {0} pulldaycount = {1}", DateTime.Today.Day, GetTransactionsForDaysACH));
                    file.WriteLine(string.Format("Payments Time Period; From:{0},To:{1}", DateTime.Today.Subtract(TimeSpan.FromDays(GetTransactionsForDaysACH)), DateTime.Now.AddDays(1)));
                    file.WriteLine("****************************************************");
                }

                getSettledBatchListResponse BatchListResponseACH = GetSettledBatchList.Run(APILoginIdACH, TransactionKeyACH, GetTransactionsForDaysACH, AuthInProduction);
                if (BatchListResponseACH != null && BatchListResponseACH.batchList == null && BatchListResponseACH.messages != null)
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\CheckPaymentReport.txt"), true))
                    {
                        file.WriteLine("Message:{1} {0}", BatchListResponseACH.messages.message[0].text, BatchListResponseACH.messages.message[0].code);
                    }
                }
                else
                {
                    BatchListProcessing(BatchListResponseACH, APILoginIdACH, TransactionKeyACH, CurrentUser, CompanyBillingStr, CompanyId, TaxPercentage, SchedulerFacade, AuthInProduction, true, org.CompanyName.ReplaceSpecialChar());
                }
            }
            #endregion

        }

        private void FortePayment(Guid CompanyId, SchedulerFacade SchedulerFacade, string CurrentUser, string CompanyBillingStr, string TaxPercentage, Organization org, List<int> ACHCchekingDay, List<int> CCCheckingDay)
        {
            GlobalSetting globset;

            #region Prduction or Live
            bool ForteInProduction = false;
            GlobalSetting globset2 = SchedulerFacade.GetGlobalsettingBySearchKeyAndCompanyId("ForteInProduction", CompanyId);
            if (globset2 != null && globset2.Value.ToLower() == "true")
            {
                ForteInProduction = true;
            }
            #endregion

            #region forte Payments

            string ForteTransactionKey = SchedulerFacade.GetForteTransactionKeyByCompanyId(CompanyId, false);
            string ForteAPILoginId = SchedulerFacade.GetForteAPILoginIdByCompanyId(CompanyId, false);
            string ForteOrganizationId = SchedulerFacade.GetForteOrganizationIdByCompanyId(CompanyId, false);
            string ForteLocationId = SchedulerFacade.GetForteLocationIdByCompanyId(CompanyId, false);
            string ForteAuthAccountId = SchedulerFacade.GetForteAuthAccountIdByCompanyId(CompanyId, false);

            //using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\CheckPaymentReport.txt"), true))
            //{
            //    file.WriteLine(string.Format("Start Checking CC Payments; checking day = {0}", DateTime.Today.Day));
            //    file.WriteLine(string.Format("Payments Time Period; From:{0},To:{1}", DateTime.Today.Subtract(TimeSpan.FromDays(GetTransactionsForDaysCC)), DateTime.Now.AddDays(1)));
            //    file.WriteLine("****************************************************");
            //}
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
            List<Customer> ccCustomerList = SchedulerFacade.GetForteSubscribedCustomerList(CompanyId).Where(x => x.PaymentMethod == "Credit Card").ToList();
            ForteTransaction forteTrans = new ForteTransaction();
            ForteTransactionService forteTransaction;
            foreach (var item in ccCustomerList)
            {
                forte.Customer_Token = item.CustomerToken;
                forteTransaction = new ForteTransactionService(forte);
                ForteTransectionResponse transResponse = new ForteTransectionResponse();
                try
                {
                    FortePaymentGetwayResponse response = forteTransaction.GetTransaction();
                    transResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ForteTransectionResponse>(response.Massege);
                    if (transResponse != null)
                    {
                        if (transResponse.results.Count > 0)
                        {
                            foreach (var itemTrans in transResponse.results)
                            {
                                if (itemTrans.entered_by == "Scheduled")
                                {

                                    #region Check If the transaction already exists
                                    Transaction transacction = SchedulerFacade.GetTransactionByTransactionId(itemTrans.transaction_id);

                                    if (transacction != null && transacction.Id > 0)
                                    {
                                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\CheckPaymentReport.txt"), true))
                                        {
                                            file.WriteLine(string.Format("Transaction #{0} already saved.", itemTrans.transaction_id));
                                            file.WriteLine(string.Format("Transaction Status {0}.", itemTrans.transaction_id));
                                        }
                                        continue;

                                    }
                                    else if (itemTrans.status != "settled" && itemTrans.status != "Complete" && itemTrans.status != "Funded")
                                    {
                                        DeclinedTransactions declinedTransaction = SchedulerFacade.GetDeclinedTransactionByTransactionId(itemTrans.transaction_id);
                                        if (declinedTransaction != null)
                                        {

                                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\CheckPaymentReport.txt"), true))
                                            {
                                                file.WriteLine(string.Format("Transaction #{0} already saved in declined transaction.", itemTrans.transaction_id));
                                            }
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\CheckPaymentReport.txt"), true))
                                        {
                                            file.WriteLine(string.Format("Transaction #{0} not found in db. Start processing to save.", itemTrans.transaction_id));
                                            file.WriteLine(string.Format("Transaction Status {0}.", itemTrans.status));
                                        }
                                    }

                                    #endregion


                                    #region BillCycle
                                    string BillingStr = "";
                                    if (item.BillCycle == LabelHelper.BillCycle.Monthly)
                                    {
                                        BillingStr = "for the month of " + DateTime.Now.AddMonths(1).ToString("MMMM-yyyy");
                                    }
                                    else if (item.BillCycle == LabelHelper.BillCycle.Quarterly)
                                    {
                                        //Jan 1//Feb 2//Mar 3//Apr 4//May 5//Jun 6//Jul 7//Aug 8//Sep 9//Oct 10//Nov 11//Dec 12

                                        if (DateTime.Now.Month == 12)
                                        {
                                            BillingStr = "for Q1 (Jan-Mar) " + DateTime.Now.AddMonths(1).ToString("yyyy");
                                        }
                                        else if (DateTime.Now.Month == 3)
                                        {

                                            BillingStr = "for Q2 (Apr-Jun) " + DateTime.Now.AddMonths(1).ToString("yyyy");
                                        }
                                        else if (DateTime.Now.Month == 6)
                                        {
                                            BillingStr = "for Q3 (Jul-Sep) " + DateTime.Now.AddMonths(1).ToString("yyyy");
                                        }
                                        else if (DateTime.Now.Month == 9)
                                        {
                                            BillingStr = "for Q4 (Oct-Dec) " + DateTime.Now.AddMonths(1).ToString("yyyy");
                                        }
                                    }
                                    else if (item.BillCycle == LabelHelper.BillCycle.SemiAnnual)
                                    {
                                        //Jan 1//Feb 2//Mar 3//Apr 4//May 5//Jun 6//Jul 7//Aug 8//Sep 9//Oct 10//Nov 11//Dec 12

                                        if (DateTime.Now.Month == 12)
                                        {

                                            BillingStr = "for semi annual 1 (Jan-Jun) " + DateTime.Now.AddMonths(1).ToString("yyyy");
                                        }
                                        else if (DateTime.Now.Month == 6)
                                        {
                                            BillingStr = "for semi annual 2 (Jul-Dec) " + DateTime.Now.AddMonths(1).ToString("yyyy");
                                        }

                                    }
                                    else if (item.BillCycle == LabelHelper.BillCycle.Annual)
                                    {
                                        if (DateTime.Now.Month == 12)
                                        {

                                            BillingStr = "for year " + DateTime.Now.Year.ToString();
                                        }
                                    }
                                    #endregion

                                    double MonitoringFee = (double)itemTrans.authorization_amount;
                                    double TaxAmount = 0;
                                    string TaxType = "Non-Tax";
                                    if (item.BillTax.HasValue && item.BillTax.Value)
                                    {
                                        /*(100*sattleAmount)/(100+taxPercentage)*/

                                        MonitoringFee = ((100 * MonitoringFee) / (100 + (Convert.ToDouble(TaxPercentage))));
                                        TaxAmount = (double)itemTrans.authorization_amount - MonitoringFee;

                                        TaxType = "Sales Tax";
                                        //TaxAmount = Math.Round((MonitoringFee * (Convert.ToDouble(TaxPercentage) / 100)), 2);
                                        //MonitoringFee = MonitoringFee - TaxAmount;
                                    }

                                    //last month invoice need to calculated
                                    string InvoiceFor = "";
                                    if (itemTrans.card.card_type.ToLower() == "mast" || itemTrans.card.card_type.ToLower() == "amex" || itemTrans.card.card_type.ToLower() == "disc" || itemTrans.card.card_type.ToLower() == "mast" || itemTrans.card.card_type.ToLower() == "mast" || itemTrans.card.card_type.ToLower() == "visa")
                                    {
                                        InvoiceFor = LabelHelper.PaymentMethod.CreditCard;
                                    }
                                    else
                                    {
                                        InvoiceFor = LabelHelper.PaymentMethod.ACH;
                                    }
                                    Invoice inv = new Invoice()
                                    {

                                        CompanyId = CompanyId,
                                        InvoiceFor = InvoiceFor,
                                        Amount = MonitoringFee,
                                        TotalAmount = (double)itemTrans.authorization_amount,
                                        BalanceDue = (double)itemTrans.authorization_amount,
                                        Status = LabelHelper.InvoiceStatus.Init,
                                        InvoiceDate = itemTrans.received_date,
                                        DueDate = DateTime.Today.AddMonths(1).AddDays(-1),
                                        CreatedBy = CurrentUser,
                                        CreatedDate = DateTime.Now,
                                        LastUpdatedDate = DateTime.Now,
                                        IsBill = false,
                                        IsEstimate = false,
                                        LateAmount = 0,
                                        LateFee = 0,
                                        BillingCycle = item.BillCycle,/*This one is required for sales->arb*/
                                        CustomerName = item.FirstName + " " + item.LastName,
                                        CustomerId = item.CustomerId,
                                        TaxType = TaxType,
                                        Tax = TaxAmount,
                                        DiscountAmount = 0,
                                        //AuthRefId = customer.AuthorizeRefId,
                                        Description = string.Format("Monitoring fee {0}", BillingStr),
                                        Message = string.Format(CompanyBillingStr, BillingStr),
                                        CreatedByUid = new Guid("22222222-2222-2222-2222-222222222222"),
                                        LastUpdatedByUid = new Guid("22222222-2222-2222-2222-222222222222"),
                                        IsARBInvoice = true,
                                        TransactionId = itemTrans.transaction_id
                                    };
                                    inv.Id = SchedulerFacade.InsertInvoice(inv);
                                    inv.InvoiceId = inv.Id.GenerateInvoiceNo();
                                    if (itemTrans.transaction_id != "" && (itemTrans.status == "settled" || itemTrans.status == "Complete" || itemTrans.status == "Funded"))
                                    {
                                        inv.Status = LabelHelper.InvoiceStatus.Paid;
                                        inv.BalanceDue = 0;
                                    }
                                    else
                                    {
                                        inv.Status = LabelHelper.InvoiceStatus.Declined;
                                    }

                                    SchedulerFacade.UpdateInvoice(inv);


                                    InvoiceDetail invDet = new InvoiceDetail()
                                    {
                                        CompanyId = item.CustomerId,
                                        CreatedBy = CurrentUser,
                                        CreatedDate = DateTime.Today,
                                        InvoiceId = inv.InvoiceId,
                                        TotalPrice = MonitoringFee,
                                        UnitPrice = MonitoringFee,
                                        Quantity = 1,
                                        EquipName = "MONITORING FEE",
                                        EquipDetail = string.Format("Monitoring fee {0}", BillingStr),
                                    };

                                    SchedulerFacade.InsertInvoiceDetails(invDet);

                                    #region if declined create a new invoice
                                    if (inv.Status == LabelHelper.InvoiceStatus.Declined)
                                    {
                                        string invoiceId = inv.InvoiceId;

                                        inv.Id = SchedulerFacade.InsertInvoice(inv);
                                        inv.InvoiceId = inv.Id.GenerateInvoiceNo();
                                        inv.Status = LabelHelper.InvoiceStatus.Open;
                                        inv.Description = string.Format("Duplicate Invoice {0}. Declined {1}", BillingStr, invoiceId);
                                        SchedulerFacade.UpdateInvoice(inv);

                                        invDet.InvoiceId = inv.InvoiceId;
                                        SchedulerFacade.InsertInvoiceDetails(invDet);
                                        try
                                        {
                                            DeclinedTransactions dt = new DeclinedTransactions()
                                            {
                                                InvoiceId = invoiceId,
                                                TransactionId = itemTrans.transaction_id,
                                                CompanyId = CompanyId,
                                                CustomerId = item.CustomerId,
                                                SubmitAmount = (double)itemTrans.authorization_amount,
                                                Reason = "Transaction declined by Forte.",
                                                Comment = string.Format("Transaction declined by Forte. {0} has been created. Status: {1}", inv.InvoiceId, itemTrans.status),

                                                ReturnAmount = -1 * (double)itemTrans.authorization_amount,
                                                ReturnedDate = DateTime.Now.UTCCurrentTime(),
                                                SettlementBatch = DateTime.Now.UTCCurrentTime(),

                                            };
                                            SchedulerFacade.InsertDeclinedTransaction(dt);
                                        }
                                        catch (Exception) { }


                                    }
                                    #endregion

                                    #region File Write
                                    string FilePath = @"~\SchedulerReports\CheckPaymentInvoiceReportCC-{0}.txt";

                                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(string.Format(FilePath, item.CompanyName)), true))
                                    {
                                        file.WriteLine(string.Format("TransactionID:{0} Invoice: {1} Date:{5} Status:{2} CustomerNo:{3} CustomerName: {4}",
                                            itemTrans.transaction_id, //0
                                            inv.InvoiceId,//1
                                            inv.Status,//2
                                            item.CustomerNo,//3 
                                            item.FirstName + " " + item.LastName,//4
                                            DateTime.Now.ToString("MM/dd/yyyy")));//5
                                    }
                                    #endregion

                                    item.LastGeneratedInvoice = DateTime.Today;
                                    SchedulerFacade.UpdateCustomer(item);
                                    if (itemTrans.transaction_id != "" && (itemTrans.status == "settled" || itemTrans.status == "Complete" || itemTrans.status == "Funded"))
                                    {

                                        Transaction tr = new Transaction()
                                        {
                                            CompanyId = CompanyId,
                                            CustomerId = item.CustomerId,
                                            InvoiceIdStr = inv.InvoiceId,
                                            PaymentMethod = item.PaymentMethod,
                                            InvoiceId = inv.Id,
                                            AddedBy = CurrentUser,
                                            Status = "Closed",
                                            TransacationDate = itemTrans.received_date,
                                            CustomerName = item.FirstName + " " + item.LastName,
                                            Amount = (double)itemTrans.authorization_amount,
                                            Balance = 0,
                                            AddedDate = DateTime.Today,
                                            ReferenceNo = itemTrans.transaction_id,
                                            CardTransactionId = itemTrans.transaction_id,
                                            Type = "Payment",
                                            CreatedBy = new Guid("22222222-2222-2222-2222-222222222222"),
                                        };
                                        //using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\CheckPaymentReport.txt"), true))
                                        //{
                                        //    file.WriteLine("Inserting Transaction");
                                        //}
                                        tr.Id = SchedulerFacade.InsertTransaction(tr);

                                        TransactionHistory trhs = new TransactionHistory()
                                        {
                                            InvoiceId = inv.Id,
                                            InvoiceNumber = inv.InvoiceId,
                                            TransacationDate = DateTime.Today,
                                            TransactionId = tr.Id,
                                            Amout = (double)itemTrans.authorization_amount,
                                            Balance = inv.BalanceDue.HasValue ? inv.BalanceDue.Value : 0,
                                            ReceivedBy = new Guid("22222222-2222-2222-2222-222222222222")
                                        };
                                        trhs.Id = SchedulerFacade.InsertTransactionHistory(trhs);
                                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\CheckPaymentReport.txt"), true))
                                        {
                                            file.WriteLine(string.Format("Transaction #{0}; All data saved successfully.", itemTrans.transaction_id));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }

            }
            #endregion



        }

        private void BatchListProcessing(getSettledBatchListResponse BatchListResponse, string APILoginId, string TransactionKey, string CurrentUser, string CompanyBillingStr
            , Guid CompanyId, string TaxPercentage, SchedulerFacade SchedulerFacade, bool AuthorizeInProduction, bool ACHPayment, string CompanyName)
        {
            try
            {
                #region BatchListResponse Processing
                if (BatchListResponse != null && BatchListResponse.batchList != null && BatchListResponse.batchList.Count() > 0)
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\CheckPaymentReport.txt"), true))
                    {
                        file.WriteLine(string.Format("Batch Found: {0}", BatchListResponse.batchList.Count()));
                    }
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

                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\CheckPaymentReport.txt"), true))
                            {
                                file.WriteLine(string.Format("Transactions Found: {0} in batch: {1} ", string.Join(",", TransactionListResponse.transactions.Select(x => x.transId)), batch.batchId));
                            }
                            foreach (transactionSummaryType transaction in TransactionListResponse.transactions)
                            {
                                try
                                {
                                    #region Loop through transaction
                                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\CheckPaymentReport.txt"), true))
                                    {
                                        file.WriteLine(string.Format("Starting with Transaction: #{0}", transaction.transId));
                                    }

                                    if (transaction.subscription == null)
                                    {
                                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\CheckPaymentReport.txt"), true))
                                        {
                                            file.WriteLine(string.Format("Not an ARB Transaction: {0}", transaction.transId));
                                        }
                                    }
                                    else if (transaction.subscription != null)// && transaction.invoiceNumber == LabelHelper.InvoiceNumberForARB.MonitoringFee)
                                    {
                                        //transactionId += ";" + transaction.subscription.id + System.Environment.NewLine;
                                        //System.IO.File.WriteAllText(@"D:\Development\Codes\CentralStationMarketing\homesecurity_svn\HS.Web.UI\transactionId.txt", transactionId);

                                        string BillingStr = "";
                                        Customer customer = new Customer();
                                        try
                                        {
                                            customer = SchedulerFacade.GetCustomerBySubscriptionId(transaction.subscription.id);
                                        }
                                        catch (Exception)
                                        {
                                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\CheckPaymentReport.txt"), true))
                                            {
                                                file.WriteLine("Customer got exception");
                                            }
                                        }
                                        #region If customer not found continue
                                        if (customer == null || customer.Id == 0)
                                        {
                                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\CheckPaymentReport.txt"), true))
                                            {
                                                file.WriteLine(string.Format("Customer not found for subscriptionID #{0}", transaction.subscription.id));
                                            }

                                            continue;
                                        }
                                        else
                                        {
                                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\CheckPaymentReport.txt"), true))
                                            {
                                                file.WriteLine(string.Format("Customer found for subscriptionID #{0}", transaction.subscription.id));
                                            }
                                        }
                                        #endregion

                                        #region Check If the transaction already exists
                                        Transaction transacction = SchedulerFacade.GetTransactionByTransactionId(transaction.transId);

                                        if (transacction != null && transacction.Id > 0)
                                        {
                                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\CheckPaymentReport.txt"), true))
                                            {
                                                file.WriteLine(string.Format("Transaction #{0} already saved.", transaction.transId));
                                                file.WriteLine(string.Format("Transaction Status {0}.", transaction.transactionStatus));
                                            }
                                            continue;
                                        }
                                        else if (transaction.transactionStatus != "settledSuccessfully" || ACHPayment)
                                        {
                                            DeclinedTransactions declinedTransaction = SchedulerFacade.GetDeclinedTransactionByTransactionId(transaction.transId);
                                            if (declinedTransaction != null)
                                            {

                                                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\CheckPaymentReport.txt"), true))
                                                {
                                                    file.WriteLine(string.Format("Transaction #{0} already saved in declined transaction.", transaction.transId));
                                                }
                                                continue;
                                            }
                                        }
                                        else
                                        {
                                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\CheckPaymentReport.txt"), true))
                                            {
                                                file.WriteLine(string.Format("Transaction #{0} not found in db. Start processing to save.", transaction.transId));
                                                file.WriteLine(string.Format("Transaction Status {0}.", transaction.transactionStatus));
                                            }
                                        }

                                        #region Transaction Status Log
                                        string TransactionLogFileName = "AuthTransactionStatus-ACH.txt";
                                        if (!ACHPayment)
                                        {
                                            TransactionLogFileName = "AuthTransactionStatus-CC.txt";
                                        }
                                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\" + TransactionLogFileName), true))
                                        {
                                            file.WriteLine(string.Format("Transaction Status {0}. Transaction #{1} Customer# {2}", transaction.transactionStatus, transaction.transId, customer.Id));
                                        }
                                        #endregion


                                        #endregion

                                        #region Billing String 
                                        if (!string.IsNullOrWhiteSpace(customer.BillCycle))
                                        {
                                            if (ACHPayment)
                                            {
                                                #region Ach billing string
                                                if (customer.BillCycle == LabelHelper.BillCycle.Monthly)
                                                {
                                                    BillingStr = "for the month of " + DateTime.Now.AddMonths(1).ToString("MMMM-yyyy");
                                                }
                                                else if (customer.BillCycle == LabelHelper.BillCycle.Quarterly)
                                                {
                                                    //Jan 1//Feb 2//Mar 3//Apr 4//May 5//Jun 6//Jul 7//Aug 8//Sep 9//Oct 10//Nov 11//Dec 12

                                                    if (DateTime.Now.Month == 12)
                                                    {
                                                        BillingStr = "for Q1 (Jan-Mar) " + DateTime.Now.AddMonths(1).ToString("yyyy");
                                                    }
                                                    else if (DateTime.Now.Month == 3)
                                                    {

                                                        BillingStr = "for Q2 (Apr-Jun) " + DateTime.Now.AddMonths(1).ToString("yyyy");
                                                    }
                                                    else if (DateTime.Now.Month == 6)
                                                    {
                                                        BillingStr = "for Q3 (Jul-Sep) " + DateTime.Now.AddMonths(1).ToString("yyyy");
                                                    }
                                                    else if (DateTime.Now.Month == 9)
                                                    {
                                                        BillingStr = "for Q4 (Oct-Dec) " + DateTime.Now.AddMonths(1).ToString("yyyy");
                                                    }
                                                }
                                                else if (customer.BillCycle == LabelHelper.BillCycle.SemiAnnual)
                                                {
                                                    //Jan 1//Feb 2//Mar 3//Apr 4//May 5//Jun 6//Jul 7//Aug 8//Sep 9//Oct 10//Nov 11//Dec 12

                                                    if (DateTime.Now.Month == 12)
                                                    {

                                                        BillingStr = "for semi annual 1 (Jan-Jun) " + DateTime.Now.AddMonths(1).ToString("yyyy");
                                                    }
                                                    else if (DateTime.Now.Month == 6)
                                                    {
                                                        BillingStr = "for semi annual 2 (Jul-Dec) " + DateTime.Now.AddMonths(1).ToString("yyyy");
                                                    }

                                                }
                                                else if (customer.BillCycle == LabelHelper.BillCycle.Annual)
                                                {
                                                    if (DateTime.Now.Month == 12)
                                                    {

                                                        BillingStr = "for year " + DateTime.Now.Year.ToString();
                                                    }
                                                }
                                                #endregion
                                            }
                                            else
                                            {
                                                #region CC Billing string
                                                if (customer.BillCycle == LabelHelper.BillCycle.Monthly)
                                                {
                                                    BillingStr = "for the month of " + DateTime.Now.ToString("MMMM-yyyy");
                                                }
                                                else if (customer.BillCycle == LabelHelper.BillCycle.Quarterly)
                                                {
                                                    //Jan 1//Feb 2//Mar 3//Apr 4//May 5//Jun 6//Jul 7//Aug 8//Sep 9//Oct 10//Nov 11//Dec 12

                                                    if (DateTime.Now.Month == 12 || DateTime.Now.Month == 1)
                                                    {
                                                        BillingStr = "for Q1 (Jan-Mar) " + DateTime.Now.AddMonths(1).ToString("yyyy");
                                                    }
                                                    else if (DateTime.Now.Month == 3 || DateTime.Now.Month == 4)
                                                    {
                                                        BillingStr = "for Q2 (Apr-Jun) " + DateTime.Now.AddMonths(1).ToString("yyyy");
                                                    }
                                                    else if (DateTime.Now.Month == 6 || DateTime.Now.Month == 7)
                                                    {
                                                        BillingStr = "for Q3 (Jul-Sep) " + DateTime.Now.AddMonths(1).ToString("yyyy");
                                                    }
                                                    else if (DateTime.Now.Month == 9 || DateTime.Now.Month == 10)
                                                    {
                                                        BillingStr = "for Q4 (Oct-Dec) " + DateTime.Now.AddMonths(1).ToString("yyyy");
                                                    }
                                                }
                                                else if (customer.BillCycle == LabelHelper.BillCycle.SemiAnnual)
                                                {
                                                    //Jan 1//Feb 2//Mar 3//Apr 4//May 5//Jun 6//Jul 7//Aug 8//Sep 9//Oct 10//Nov 11//Dec 12

                                                    if (DateTime.Now.Month == 12 || DateTime.Now.Month == 1)
                                                    {
                                                        BillingStr = "for semi annual 1 (Jan-Jun) " + DateTime.Now.AddMonths(1).ToString("yyyy");
                                                    }
                                                    else if (DateTime.Now.Month == 6 || DateTime.Now.Month == 7)
                                                    {
                                                        BillingStr = "for semi annual 2 (Jul-Dec) " + DateTime.Now.AddMonths(1).ToString("yyyy");
                                                    }
                                                }
                                                else if (customer.BillCycle == LabelHelper.BillCycle.Annual)
                                                {
                                                    if (DateTime.Now.Month == 12 || DateTime.Now.Month == 1)
                                                    {
                                                        BillingStr = "for year " + DateTime.Now.AddMonths(1).Year.ToString();
                                                    }
                                                }
                                                #endregion
                                            }
                                        }
                                        #endregion

                                        double MonitoringFee = (double)transaction.settleAmount;
                                        double TaxAmount = 0;
                                        string TaxType = "Non-Tax";
                                        if (customer.BillTax.HasValue && customer.BillTax.Value)
                                        {
                                            /*(100*sattleAmount)/(100+taxPercentage)*/

                                            MonitoringFee = ((100 * MonitoringFee) / (100 + (Convert.ToDouble(TaxPercentage))));
                                            TaxAmount = (double)transaction.settleAmount - MonitoringFee;

                                            TaxType = "Sales Tax";
                                            //TaxAmount = Math.Round((MonitoringFee * (Convert.ToDouble(TaxPercentage) / 100)), 2);
                                            //MonitoringFee = MonitoringFee - TaxAmount;
                                        }

                                        //last month invoice need to calculated
                                        Invoice inv = new Invoice()
                                        {
                                            CompanyId = CompanyId,
                                            InvoiceFor = ACHPayment ? LabelHelper.PaymentMethod.ACH : LabelHelper.PaymentMethod.CreditCard, //customer.PaymentMethod,
                                            Amount = MonitoringFee,
                                            TotalAmount = (double)transaction.settleAmount,
                                            BalanceDue = (double)transaction.settleAmount,
                                            Status = LabelHelper.InvoiceStatus.Init,
                                            InvoiceDate = transaction.submitTimeLocal,
                                            DueDate = DateTime.Today.AddMonths(1).AddDays(-1),
                                            CreatedBy = CurrentUser,
                                            CreatedDate = transaction.submitTimeUTC,
                                            LastUpdatedDate = DateTime.Now,
                                            IsBill = false,
                                            IsEstimate = false,
                                            LateAmount = 0,
                                            LateFee = 0,
                                            BillingCycle = customer.BillCycle,/*This one is required for sales->arb*/
                                            CustomerName = customer.FirstName + " " + customer.LastName,
                                            CustomerId = customer.CustomerId,
                                            TaxType = TaxType,
                                            Tax = TaxAmount,
                                            DiscountAmount = 0,
                                            AuthRefId = customer.AuthorizeRefId,
                                            Description = string.Format("Monitoring fee {0}", BillingStr),
                                            Message = string.Format(CompanyBillingStr, BillingStr),
                                            CreatedByUid = new Guid("22222222-2222-2222-2222-222222222222"),
                                            LastUpdatedByUid = new Guid("22222222-2222-2222-2222-222222222222"),
                                            IsARBInvoice = true,
                                            TransactionId = transaction.transId
                                        };
                                        inv.Id = SchedulerFacade.InsertInvoice(inv);
                                        inv.InvoiceId = inv.Id.GenerateInvoiceNo();
                                        if (transaction.transactionStatus == "settledSuccessfully")
                                        {
                                            inv.Status = LabelHelper.InvoiceStatus.Paid;
                                            inv.BalanceDue = 0;
                                        }
                                        else
                                        {
                                            inv.Status = LabelHelper.InvoiceStatus.Declined;
                                            //[Shariful-16-9-19]
                                            //[NOTE:Send mail for declined]
                                            //CustomerFacade CustomerFacade = new CustomerFacade();
                                            //Customer objcus = CustomerFacade.GetCustomerByCustomerId(inv.CustomerId);
                                            //DeclineMail DeclineMail = new DeclineMail();
                                            //DeclineMail.CustomerName = objcus.FirstName + " " + objcus.LastName;
                                            //DeclineMail.ToEmail = objcus.EmailAddress;
                                            //DeclineMail.DeclinationReason = transaction.transactionStatus;

                                            //MailFacade MailFacade = new MailFacade();
                                            //MailFacade.DeclineMail(DeclineMail, inv.CompanyId);
                                            //[~Shariful-16-9-19]
                                        }

                                        SchedulerFacade.UpdateInvoice(inv);

                                        InvoiceDetail invDet = new InvoiceDetail()
                                        {
                                            CompanyId = CompanyId,
                                            CreatedBy = CurrentUser,
                                            CreatedDate = DateTime.Today,
                                            InvoiceId = inv.InvoiceId,
                                            TotalPrice = MonitoringFee,
                                            UnitPrice = MonitoringFee,
                                            Quantity = 1,
                                            EquipName = "MONITORING FEE",
                                            EquipDetail = string.Format("Monitoring fee {0}", BillingStr),
                                        };

                                        SchedulerFacade.InsertInvoiceDetails(invDet);

                                        #region if declined create a new invoice
                                        if (inv.Status == LabelHelper.InvoiceStatus.Declined)
                                        {
                                            string invoiceId = inv.InvoiceId;

                                            inv.Id = SchedulerFacade.InsertInvoice(inv);
                                            inv.InvoiceId = inv.Id.GenerateInvoiceNo();
                                            inv.Status = LabelHelper.InvoiceStatus.Open;
                                            inv.Description = string.Format("Duplicate Invoice {0}. Declined {1}", BillingStr, invoiceId);
                                            SchedulerFacade.UpdateInvoice(inv);

                                            invDet.InvoiceId = inv.InvoiceId;
                                            SchedulerFacade.InsertInvoiceDetails(invDet);
                                            try
                                            {
                                                DeclinedTransactions dt = new DeclinedTransactions()
                                                {
                                                    InvoiceId = invoiceId,
                                                    TransactionId = transaction.transId,
                                                    CompanyId = CompanyId,
                                                    CustomerId = customer.CustomerId,
                                                    SubmitAmount = (double)transaction.settleAmount,
                                                    Reason = "Transaction declined by authorize.net.",
                                                    Comment = string.Format("Transaction declined by authorize.net. {0} has been created. Status: {1}", inv.InvoiceId, transaction.transactionStatus),

                                                    ReturnAmount = -1 * (double)transaction.settleAmount,
                                                    ReturnedDate = DateTime.Now.UTCCurrentTime(),
                                                    SettlementBatch = DateTime.Now.UTCCurrentTime(),

                                                };
                                                SchedulerFacade.InsertDeclinedTransaction(dt);
                                            }
                                            catch (Exception) { }


                                        }
                                        #endregion

                                        #region File Write
                                        string FilePath = @"~\SchedulerReports\CheckPaymentInvoiceReportCC-{0}.txt";
                                        if (ACHPayment)
                                        {
                                            FilePath = @"~\SchedulerReports\CheckPaymentInvoiceReportACH-{0}.txt";
                                        }

                                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(string.Format(FilePath, CompanyName)), true))
                                        {
                                            file.WriteLine(string.Format("TransactionID:{0} Invoice: {1} Date:{5} Status:{2} CustomerNo:{3} CustomerName: {4}",
                                                transaction.transId, //0
                                                inv.InvoiceId,//1
                                                inv.Status,//2
                                                customer.CustomerNo,//3 
                                                customer.FirstName + " " + customer.LastName,//4
                                                DateTime.Now.ToString("MM/dd/yyyy")));//5
                                        }
                                        #endregion

                                        customer.LastGeneratedInvoice = DateTime.Today;
                                        SchedulerFacade.UpdateCustomer(customer);
                                        if (transaction.transactionStatus == "settledSuccessfully")
                                        {

                                            Transaction tr = new Transaction()
                                            {
                                                CompanyId = CompanyId,
                                                CustomerId = customer.CustomerId,
                                                InvoiceIdStr = inv.InvoiceId,
                                                PaymentMethod = customer.PaymentMethod,
                                                InvoiceId = inv.Id,
                                                AddedBy = CurrentUser,
                                                Status = "Closed",
                                                TransacationDate = transaction.submitTimeUTC,
                                                CustomerName = customer.FirstName + " " + customer.LastName,
                                                Amount = (double)transaction.settleAmount,
                                                Balance = 0,
                                                AddedDate = DateTime.Today,
                                                ReferenceNo = transaction.transId,
                                                CardTransactionId = transaction.transId,
                                                Type = "Payment",
                                                CreatedBy = new Guid("22222222-2222-2222-2222-222222222222"),
                                            };
                                            //using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\CheckPaymentReport.txt"), true))
                                            //{
                                            //    file.WriteLine("Inserting Transaction");
                                            //}
                                            tr.Id = SchedulerFacade.InsertTransaction(tr);

                                            TransactionHistory trhs = new TransactionHistory()
                                            {
                                                InvoiceId = inv.Id,
                                                InvoiceNumber = inv.InvoiceId,
                                                TransacationDate = DateTime.Today,
                                                TransactionId = tr.Id,
                                                Amout = (double)transaction.settleAmount,
                                                Balance = inv.BalanceDue.HasValue ? inv.BalanceDue.Value : 0,
                                                ReceivedBy = new Guid("22222222-2222-2222-2222-222222222222")
                                            };
                                            trhs.Id = SchedulerFacade.InsertTransactionHistory(trhs);
                                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\CheckPaymentReport.txt"), true))
                                            {
                                                file.WriteLine(string.Format("Transaction #{0}; All data saved successfully.", transaction.transId));
                                            }
                                        }
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

        #region 3.ReminderNotificationTasks 
        private void ReminderNotificationTasks()
        {
            #region SchedulerRunLog
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\SchedulerStartLogs.txt"), true))
                {
                    file.WriteLine(string.Format("3.ReminderNotificationTasks  run on {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")));

                }
            }
            catch (Exception) { }

            #endregion

            SchedulerFacade SchedulerFacadeMaster = new SchedulerFacade();
            SchedulerFacade SchedulerFacade = null;
            ShortUrlFacade ShortUrlFacade = new ShortUrlFacade();
            List<Organization> OrgList = SchedulerFacadeMaster.GetAllOrganizations();
            HS.Facade.ShortUrlFacade suf = new Facade.ShortUrlFacade();
            Company Company;

            foreach (var item in OrgList)
            {
                Guid CompanyId = item.CompanyId;
                SchedulerFacade = new SchedulerFacade(item.ConnectionString);
                GlobalSettingsFacade GlobalSettingsFacade = new GlobalSettingsFacade(item.ConnectionString);
                Company = SchedulerFacade.GetCompanyByCompanyId(CompanyId);

                try
                {
                    #region Reminder Notifications
                    List<CustomerNote> TodaysReminders = SchedulerFacade.GetTodaysReminders();
                    foreach (var reminderitem in TodaysReminders)
                    {
                        List<NoteAssign> AssignedUsers = SchedulerFacade.GetAllAssignedUsersByCustomerNoteId(reminderitem.Id);
                        if (AssignedUsers != null && AssignedUsers.Count() > 0)
                        {
                            Customer Customer = SchedulerFacade.GetCustomerByCustomerId(reminderitem.CustomerId);
                            CustomerCompany cc = SchedulerFacade.GetCustomerCompanyByCustomerId(reminderitem.CustomerId);
                            string customerName = "";
                            if (Customer != null)
                            {
                                if (!String.IsNullOrWhiteSpace(Customer.BusinessName))
                                {
                                    customerName = Customer.BusinessName;

                                }
                                if (String.IsNullOrWhiteSpace(Customer.BusinessName))
                                {
                                    customerName = Customer.DBA;

                                }
                                if (String.IsNullOrWhiteSpace(Customer.BusinessName) && String.IsNullOrWhiteSpace(Customer.DBA))
                                {
                                    customerName = Customer.FirstName + " " + Customer.LastName;

                                }
                            }

                            if (Customer != null && cc != null)
                            {
                                //string ReminderLink = AppConfig.DomainSitePath + string.Format("/Customer/Customerdetail/?id={0}&taskid={2}&time={1}&from=notification#NotesTab", Customer.Id,reminderitem.ReminderDate.Value.ToString("HH:mm"),reminderitem.Id);
                                string ReminderLink = AppConfig.DomainSitePath + string.Format("/Customer/Customerdetail/?id={0}#NotesTab", Customer.Id);

                                if (cc.IsLead)
                                {
                                    ReminderLink = string.Format("/Lead/Leadsdetail/?id={0}#LeadFollowUpTab", Customer.Id);
                                }
                                string CustomerLink = AppConfig.DomainSitePath + string.Format("/Customer/Customerdetail/?id={0}", Customer.Id);
                                if (cc.IsLead)
                                {
                                    CustomerLink = string.Format("/Lead/Leadsdetail/?id={0}", Customer.Id);
                                }
                                #region Insert notification
                                Notification notification = new Notification()
                                {
                                    CompanyId = CompanyId,
                                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                                    NotificationId = Guid.NewGuid(),
                                    Type = LabelHelper.NotificationType.Employee,
                                    Who = reminderitem.CreatedByUid,
                                    What = string.Format(@"You have a <a class='cus-anchor' href='{2}'>task</a> on {3} at {1} for <a class='cus-anchor' href='{4}'>{5}</a> assigned by {0}", "{0}", reminderitem.ReminderDate.Value.UTCToServerTime().ToString("hh:mm tt"), ReminderLink, reminderitem.ReminderDate.Value.UTCToServerTime().ToString("dd/MM/yyyy"), CustomerLink, customerName),
                                    NotificationUrl = ReminderLink
                                };
                                SchedulerFacade.InsertNotification(notification);
                                #endregion

                                foreach (var user in AssignedUsers)
                                {
                                    #region set user to notification
                                    NotificationUser nu = new NotificationUser()
                                    {
                                        NotificationId = notification.NotificationId,
                                        IsRead = false,
                                        NotificationPerson = user.EmployeeId,
                                    };
                                    SchedulerFacade.InsertNotificationUser(nu);
                                    #endregion
                                }
                            }
                        }
                    }
                    #endregion
                }
                catch (Exception) { }

                try
                {
                    #region Auto ClockOut
                    EmployeeTimeClockFacade EmpTimeClockFacade = new EmployeeTimeClockFacade(item.ConnectionString);
                    List<EmployeeTimeClock> AutoClockOutList = EmpTimeClockFacade.GetAutoClockOutList();
                    GlobalSetting defultAutoClockout = GlobalSettingsFacade.GetGlobalSettingsByKey(CompanyId, "AutoClockOut");

                    foreach (var liItem in AutoClockOutList)
                    {
                        DateTime ClockInTime = liItem.ClockInTime;
                        DateTime ClockOutTime = new DateTime();
                        DateTime Now = DateTime.Now.UTCCurrentTime();

                        int mins = (Now - ClockInTime).Minutes;
                        int hours = ((Now - ClockInTime).Days * 24) + (Now - ClockInTime).Hours;

                        int TimeSpent = ((hours * 60) + mins) * 60;
                        if (defultAutoClockout != null && defultAutoClockout.Value.ToLower() == "8hoursclockout")
                        {
                            if (hours >= 8)
                            {
                                ClockOutTime = ClockInTime.AddHours(8);
                                TimeSpent = 8 * 60 * 60;
                            }
                            else
                            {
                                ClockOutTime = Now;
                            }
                        }
                        else if (defultAutoClockout != null && defultAutoClockout.Value.ToLower() == "12hoursclockout")
                        {
                            if (hours >= 12)
                            {
                                ClockOutTime = ClockInTime.AddHours(12);
                                TimeSpent = 12 * 60 * 60;
                            }
                            else
                            {
                                ClockOutTime = Now;
                            }
                        }
                        else
                        {
                            ClockOutTime = Now;
                        }



                        liItem.ClockOutCreatedBy = new Guid("22222222-2222-2222-2222-222222222222");
                        liItem.Lat = "";
                        liItem.Lng = "";
                        liItem.UserId = liItem.UserId;
                        liItem.Type = LabelHelper.TimeClockType.ClockOut;
                        liItem.ClockOutNote = "System auto clockout";
                        liItem.ClockOutTime = ClockOutTime;
                        liItem.ClockedInSeconds = TimeSpent;
                        liItem.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                        EmpTimeClockFacade.UpdateEmployeeTimeClock(liItem);
                    }
                    #endregion
                }
                catch (Exception) { }

            }
        }
        #endregion

        #region 4.GenerateInvoice
        /// <summary>
        /// 1.This function will call everyday 12:00 am
        /// 2.It will generate invoice for subscribed customers.
        /// 3.It will generate invoice for
        ///     a.Coustomer with payemnt method 'Invoice'
        ///     b.Customer has billing cycle
        ///     c.Customer has bill day
        /// 4.Invoice will generate on conditions
        ///     a.It will generate on monthly monitoring fee.
        ///     b.Tax will be calculated manually.
        /// 5.This function will go through all companies and customers.
        /// 6.Previous months undpaid invoice will be added with a late fee
        /// 7.Previous months invoice conditions
        ///     a.Previous months invoice will be taken without tax.
        ///     b.Tax will be calculated on new invoice.
        /// 7.Late fee condtions
        ///     a.Late amount will also under tax
        /// 8.Assuming this wont be partially paid
        /// </summary>
        public void GenerateInvoice()
        {

            #region SchedulerRunLog
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\SchedulerStartLogs.txt"), true))
                {
                    file.WriteLine(string.Format("4.GenerateInvoice  run on {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")));

                }
            }
            catch (Exception e) { }

            #endregion

            float invoiceLateFee = 15;
            string CurrentUser = "System";
            Guid CreatedByUid = new Guid("22222222-2222-2222-2222-222222222222");
            string TaxPercentage = "0";
            string InvoiceMessage = "Please, Pay the service fee as soon as possible to avoid late fee.";

            DateTime CreatedDate = DateTime.Today; //DateTime.Today;
            DateTime DueDate = CreatedDate.AddDays(-1).AddMonths(1); //DateTime.Today.AddDays(-1).AddMonths(1);

            SchedulerFacade SchedulerFacadeMaster = new SchedulerFacade();
            SchedulerFacade SchedulerFacade = null;
            ShortUrlFacade ShortUrlFacade = new ShortUrlFacade();
            List<Organization> OrgList = SchedulerFacadeMaster.GetAllOrganizations();
            HS.Facade.ShortUrlFacade suf = new Facade.ShortUrlFacade();
            Company Company;

            #region Directory for saving reports
            string subPath = "~/SchedulerReports"; // your code goes here

            bool exists = System.IO.Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath(subPath));
            if (!exists)
                System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath(subPath));

            #endregion

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\InvoiceReport.txt"), true))
            {
                file.WriteLine("GenerateInvoice Scheduler Started at {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
            }

            //Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception("GenerateInvoice Scheduler Started"));
            foreach (var item in OrgList)
            {

                try
                {
                    #region OrgLoop
                    Guid CompanyId = item.CompanyId;
                    //if(CompanyId !=  new Guid("A61B5B77-C472-48EE-A8C7-A2590744CCEA"))
                    //{
                    //    continue;
                    //}
                    string BillingStr = "";
                    SchedulerFacade = new SchedulerFacade(item.ConnectionString);
                    MailFacade MailFacade = new MailFacade();
                    MailFacade = new MailFacade(item.ConnectionString);
                    GlobalSettingsFacade GlobalSettingsFacade = new GlobalSettingsFacade(item.ConnectionString);
                    Company = SchedulerFacade.GetCompanyByCompanyId(CompanyId);
                    //need to make this Get function more efficient
                    List<Customer> CustomerList = SchedulerFacade.GetSubscribedCustomerList(CompanyId);
                    TaxPercentage = SchedulerFacade.GetSalesTax(CompanyId);
                    var AddressTemplate = SchedulerFacade.GetCustomerAddressFormat(CompanyId);







                    //Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(string.Format("CompanyName: {0},coustomer Count{1}",item.CompanyName,CustomerList.Count())));
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\InvoiceReport.txt"), true))
                    {
                        file.WriteLine(string.Format("CompanyName: {0},coustomer Count{1}", item.CompanyName, CustomerList.Count()));
                    }
                    //need to make this Get function more efficient
                    if (CustomerList == null)
                    {
                        continue;
                    }
                    foreach (var customer in CustomerList)
                    {

                        try
                        {

                            #region Customer Loop
                            //double PreviousMonthAmount = 0;
                            //string PreviousInvoiceId = "";
                            //double PreviousMonthAmountWithTax = 0; 
                            int MonthsCount = 1;

                            if (customer.LastGeneratedInvoice.HasValue
                                && customer.LastGeneratedInvoice != new DateTime()
                                && (customer.LastGeneratedInvoice.Value == CreatedDate
                                    || (customer.LastGeneratedInvoice.Value.Month == CreatedDate.Month //it means an invoice is already created in this month
                                        && customer.LastGeneratedInvoice.Value.Year == CreatedDate.Year)
                                    )
                                )
                            {
                                continue;
                            }

                            #region Billing String 
                            else if (!string.IsNullOrWhiteSpace(customer.BillCycle))
                            {
                                if (customer.BillCycle == LabelHelper.BillCycle.Monthly)
                                {
                                    BillingStr = string.Format("Month of {0} {1}", DateTime.Now.AddMonths(1).ToString("MMMM"), DateTime.Now.AddMonths(1).ToString("yyyy"));
                                }
                                else if (customer.BillCycle == LabelHelper.BillCycle.Quarterly)
                                {
                                    //Jan 1//Feb 2//Mar 3//Apr 4//May 5//Jun 6//Jul 7//Aug 8//Sep 9//Oct 10//Nov 11//Dec 12
                                    //12//3//6//9//
                                    MonthsCount = 3;
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

                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                else if (customer.BillCycle == LabelHelper.BillCycle.SemiAnnual)
                                {
                                    //Jan 1//Feb 2//Mar 3//Apr 4//May 5//Jun 6//Jul 7//Aug 8//Sep 9//Oct 10//Nov 11//Dec 12
                                    MonthsCount = 6;
                                    if (DateTime.Now.Month == 12)
                                    {
                                        BillingStr = "Semi annual 1 (Jan-Jun)";
                                    }
                                    else if (DateTime.Now.Month == 6)
                                    {
                                        BillingStr = "Semi annual 2 (Jul-Dec)";

                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                                else if (customer.BillCycle == LabelHelper.BillCycle.Annual)
                                {
                                    MonthsCount = 12;
                                    if (DateTime.Now.Month == 12)
                                    {
                                        BillingStr = "Year " + DateTime.Now.Year.ToString();
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
                            }
                            else
                            {
                                continue;
                            }
                            #endregion


                            //need to check customer subscription time period
                            #region Previous Amount Calculation
                            //if (customer.LastGeneratedInvoice.HasValue)
                            //{
                            //    Invoice tminv = SchedulerFacade.GetInvoiceByAddedDateAndCustomerId(customer.CustomerId, customer.LastGeneratedInvoice.Value, false);

                            //    //cancel for ach and credit card client requirement
                            //    if (tminv != null
                            //        && tminv.Status != LabelHelper.InvoiceStatus.Paid
                            //        && tminv.Status != LabelHelper.InvoiceStatus.Cancelled
                            //        && tminv.Status != LabelHelper.InvoiceStatus.OnHold
                            //        && tminv.DueDate < DateTime.Now
                            //        && tminv.BalanceDue.HasValue && tminv.BalanceDue.Value>0)
                            //    {
                            //        PreviousMonthAmount = tminv.BalanceDue.Value;
                            //        if (PreviousMonthAmount > 0)
                            //        {
                            //            //Previous month amount without late feee added
                            //            //PreviousMonthAmountWithTax = PreviousMonthAmount;

                            //            PreviousMonthAmount += (PreviousMonthAmount * (invoiceLateFee / 100));
                            //            tminv.Status = LabelHelper.InvoiceStatus.RolledOver;
                            //            SchedulerFacade.UpdateInvoice(tminv);
                            //            PreviousInvoiceId = tminv.InvoiceId;
                            //        }
                            //    }
                            //}
                            #endregion

                            double CustomerTotalBillAmount = Math.Round(((Convert.ToDouble(customer.MonthlyMonitoringFee) * MonthsCount)), 2);
                            double CustomerTotalBillAmountWithTax = CustomerTotalBillAmount;
                            double TotalTax = 0;
                            string TaxType = "Non-Tax";

                            #region Tax Calculation
                            //last month invoice need to calculated
                            if (customer.BillTax.HasValue && customer.BillTax.Value && !string.IsNullOrWhiteSpace(TaxPercentage))
                            {
                                TotalTax = Math.Round((CustomerTotalBillAmountWithTax * (Convert.ToDouble(TaxPercentage) / 100)), 2);
                                CustomerTotalBillAmountWithTax += TotalTax;
                                CustomerTotalBillAmountWithTax = Math.Round(CustomerTotalBillAmountWithTax, 2);

                                //if (PreviousMonthAmount > 0)
                                //{
                                //    TotalTax = Math.Round((PreviousMonthAmountWithTax * (Convert.ToDouble(TaxPercentage) / 100)), 2);
                                //    PreviousMonthAmountWithTax += TotalTax;
                                //    PreviousMonthAmountWithTax = Math.Round(PreviousMonthAmountWithTax, 2);
                                //}
                                //Calculate tax

                                TaxType = "Sales Tax";

                            }

                            #region Previous Amount Calculations
                            //CustomerTotalBillAmountWithTax = CustomerTotalBillAmountWithTax + PreviousMonthAmount;
                            //CustomerTotalBillAmount = CustomerTotalBillAmount + PreviousMonthAmount;
                            #endregion

                            #endregion

                            #region Insert Invoice
                            Invoice inv = new Invoice()
                            {
                                CompanyId = CompanyId,
                                InvoiceFor = LabelHelper.InvoiceFor.SystemGenerated,
                                Tax = TotalTax,
                                TotalAmount = CustomerTotalBillAmountWithTax,
                                BalanceDue = CustomerTotalBillAmountWithTax,
                                Amount = CustomerTotalBillAmount,
                                DiscountAmount = 0,
                                Status = "Init",
                                DueDate = DueDate,//DateTime.Today.AddDays(-1).AddMonths(1),
                                CreatedBy = CurrentUser,
                                CreatedDate = CreatedDate,
                                InvoiceDate = CreatedDate,
                                LastUpdatedDate = DateTime.Now,
                                IsBill = false,
                                IsEstimate = false,
                                LateFee = invoiceLateFee,
                                CustomerName = customer.FirstName + " " + customer.LastName,
                                CustomerId = customer.CustomerId,
                                BillingCycle = customer.BillCycle,
                                BillingAddress = AddressHelper.MakeCustomerAddress(customer, "BillingAddress", AddressTemplate),
                                ShippingAddress = AddressHelper.MakeCustomerAddress(customer, "ShippingAddress", AddressTemplate),

                                TaxType = TaxType,
                                Message = InvoiceMessage,
                                CreatedByUid = CreatedByUid,
                                IsARBInvoice = true,
                            };
                            inv.Id = SchedulerFacade.InsertInvoice(inv);
                            inv.InvoiceId = inv.Id.GenerateInvoiceNo();
                            inv.Status = "Open";
                            #endregion

                            #region Insert Invoice Detail
                            InvoiceDetail invDet = new InvoiceDetail()
                            {
                                CompanyId = CompanyId,
                                CreatedBy = CurrentUser,
                                CreatedDate = CreatedDate,
                                InvoiceId = inv.InvoiceId,
                                TotalPrice = Math.Round(Convert.ToDouble(customer.MonthlyMonitoringFee) * MonthsCount, 2),
                                UnitPrice = Math.Round(Convert.ToDouble(customer.MonthlyMonitoringFee), 2),
                                Quantity = MonthsCount,
                                EquipName = "MONITORING FEE",
                                EquipDetail = string.Format("Monthly Monitoring Fee for {0}", BillingStr) //with tax {1}% ,TaxPercentage
                            };
                            inv.Description = invDet.EquipDetail;
                            SchedulerFacade.InsertInvoiceDetails(invDet);

                            #region Unpaid Invoices of that customer
                            //List<InvoiceDetail> UnpaidInvoiceDetailList = SchedulerFacade.GetUnpaidInvoiceDetailListByCustomerId(customer.CustomerId);
                            double PreviousBalance = 0;
                            double LateAmount = 0;

                            #region Late Fee
                            GlobalSetting LateFeeEnabled = GlobalSettingsFacade.GetGlobalSettingsByKey(CompanyId, "LateFeeEnabled");
                            if (LateFeeEnabled != null && LateFeeEnabled.Value.ToLower() == "true")
                            {
                                float LateFeePercentage = 15;
                                GlobalSetting LateFeePercentageSetting = GlobalSettingsFacade.GetGlobalSettingsByKey(CompanyId, "LateFeePercentage");
                                if (LateFeePercentageSetting != null)
                                {
                                    float.TryParse(LateFeePercentageSetting.Value, out LateFeePercentage);
                                }
                                LateFeePercentage = LateFeePercentage / 100;

                                List<Invoice> UnpaidInvoiceList = SchedulerFacade.GetUnpaidInvoiceListByCustomerId(customer.CustomerId);

                                foreach (var unpaidInv in UnpaidInvoiceList)
                                {
                                    if (unpaidInv.BalanceDue.HasValue && unpaidInv.BalanceDue > 0)
                                    {
                                        PreviousBalance += unpaidInv.BalanceDue.Value;
                                        LateAmount += unpaidInv.BalanceDue.Value * LateFeePercentage;

                                        InvoiceDetail PrevinvDet = new InvoiceDetail()
                                        {
                                            CompanyId = CompanyId,
                                            CreatedBy = CurrentUser,
                                            CreatedDate = CreatedDate,
                                            InvoiceId = inv.InvoiceId,
                                            TotalPrice = Math.Round((unpaidInv.BalanceDue.Value + unpaidInv.BalanceDue.Value * 0.15), 2),
                                            UnitPrice = Math.Round((unpaidInv.BalanceDue.Value + unpaidInv.BalanceDue.Value * 0.15), 2),
                                            Quantity = MonthsCount,
                                            Taxable = false,
                                            EquipName = string.Format("Previous Balance {0}.", unpaidInv.InvoiceId),
                                            EquipDetail = string.Format("Due bill from invoice {0} with {1}% late fee", unpaidInv.InvoiceId, invoiceLateFee)

                                        };
                                        SchedulerFacade.InsertInvoiceDetails(PrevinvDet);
                                        unpaidInv.Status = LabelHelper.InvoiceStatus.RolledOver;
                                        SchedulerFacade.UpdateInvoice(unpaidInv);
                                    }

                                }
                            }

                            #endregion

                            #endregion
                            inv.LateFee = LateAmount;
                            inv.BalanceDue += (PreviousBalance + LateAmount);
                            inv.TotalAmount += (PreviousBalance + LateAmount);
                            inv.LateAmount = LateAmount;
                            SchedulerFacade.UpdateInvoice(inv);

                            #region ServerLog InvoiceReport.txt
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\InvoiceReport.txt"), true))
                            {
                                file.WriteLine(string.Format("Invoice: {0} Date:{3} CustomerNo:{1} CustomerName: {2}", inv.InvoiceId, customer.CustomerNo, customer.FirstName + " " + customer.LastName, DateTime.Now.ToString("MM/dd/yyyy")));
                            }
                            #endregion

                            #region Previous Months Amount
                            //if (PreviousMonthAmount > 0)
                            //{
                            //    InvoiceDetail invDet2 = new InvoiceDetail()
                            //    {
                            //        CompanyId = CompanyId,
                            //        CreatedBy = CurrentUser,
                            //        CreatedDate = CreatedDate,
                            //        InvoiceId = inv.InvoiceId,
                            //        TotalPrice = PreviousMonthAmount,
                            //        UnitPrice = PreviousMonthAmount,
                            //        Quantity = 1,
                            //        EquipName = "PREVIOUS MONITORING FEE",
                            //        EquipDetail = string.Format("Monitoring fee for previous months (Invoice: {1}) with {0}% of late fee", invoiceLateFee.ToString(), PreviousInvoiceId),
                            //        Taxable = false
                            //    };
                            //    SchedulerFacade.InsertInvoiceDetails(invDet2);
                            //}
                            #endregion
                            #endregion

                            customer.LastGeneratedInvoice = CreatedDate;
                            customer.LastUpdatedBy = CurrentUser;
                            SchedulerFacade.UpdateCustomer(customer);


                            //Notification Email Part
                            #region Send Invoice Email Notification to customer

                            bool notify = false;
                            GlobalSetting sendnotification = GlobalSettingsFacade.GetGlobalSettingsByKey(CompanyId, "SendInvoiceCreatedNotificationToCustomer");
                            if (sendnotification != null && sendnotification.Value.ToLower() == "true")
                            {
                                notify = true;
                            }

                            #region Send Invoice Email To Customer
                            if ((customer.PreferedEmail.HasValue && customer.PreferedEmail.Value) &&
                                customer.EmailAddress.IsValidEmailAddress() && notify)
                            {
                                Guid EmployeeId = new Guid();
                                string SalesGuy = "";
                                string SalesGuyEmail = "";
                                string SalesPhone = "";

                                #region Reply Email Name And Address
                                if (!string.IsNullOrWhiteSpace(customer.Soldby) && Guid.TryParse(customer.Soldby, out EmployeeId) && EmployeeId != new Guid())
                                {
                                    Employee emp = SchedulerFacade.GetEmployeeByEmployeeId(EmployeeId);
                                    if (emp != null)
                                    {
                                        SalesGuy = string.Concat(emp.FirstName, " ", emp.LastName);
                                        SalesGuyEmail = (string.IsNullOrWhiteSpace(emp.Email) && emp.Email.IsValidEmailAddress()) ? "info@rmrcloud.com" : emp.Email;
                                        SalesPhone = emp.Phone;
                                    }
                                }
                                else
                                {
                                    SalesGuy = item.CompanyName;
                                    SalesGuyEmail = "info@rmrcloud.com";
                                }

                                #endregion Reply Email Name And Address 
                                inv.CompanyInfo = GlobalSettingsFacade.GetCompanyAddressFormat(CompanyId);

                                #region PDF Generate
                                //CreateInvoice Model = GetCreateInvoiceModel(inv, invDetList, tempComp, tempCustomer);
                                //Model.Invoice = inv;
                                //Model.CompanyInfo = inv.CompanyInfo;
                                //List<CreateInvoice> ModelList = new List<CreateInvoice>();

                                //ModelList.Add(inv);
                                //ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("~/Views/Invoice/InvoicePdf.cshtml", ModelList)
                                //{
                                //    //FileName = "TestView.pdf",
                                //    PageSize = Rotativa.Options.Size.A4,
                                //    PageOrientation = Rotativa.Options.Orientation.Portrait,
                                //    PageMargins = { Left = 1, Right = 1 },

                                //};
                                //byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
                                #endregion

                                string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(inv.Id
                                                    + "#"
                                                    + CompanyId
                                                    + "#"
                                                    + customer.CustomerId);
                                string fullurl = string.Concat(AppConfig.SiteDomain, "/Customer-Invoice/", encryptedurl);
                                ShortUrl ShortUrl = ShortUrlFacade.GetSortUrlByUrl(fullurl, customer.CustomerId);

                                var EmailTemplate = MailFacade.GetTemplateByTemplateKey(EmailTemplateKey.PredefinedTemplates.InvoicePredefineEmailTemplate);
                                Hashtable datatemplate = new Hashtable();
                                datatemplate.Add("CustomerName", customer.FirstName + " " + customer.LastName);
                                datatemplate.Add("ExpirationDate", inv.DueDate);
                                datatemplate.Add("SalesPhone Number", string.IsNullOrWhiteSpace(SalesPhone) ? Company.Phone : SalesPhone);
                                datatemplate.Add("CompanyName", Company.CompanyName);
                                datatemplate.Add("SalesGuy", SalesGuy);
                                datatemplate.Add("url", string.Concat(AppConfig.ShortSiteDomain, "/shrt/", ShortUrl.Code));
                                string emailtemplate = HS.Web.UI.Helper.LabelHelper.ParserHelper(EmailTemplate.BodyContent, datatemplate);

                                InvoiceCreatedEmail email = new InvoiceCreatedEmail()
                                {
                                    CompanyName = Company.CompanyName,
                                    CustomerName = customer.FirstName + " " + customer.LastName,
                                    EmailBody = HttpUtility.HtmlDecode(emailtemplate),
                                    FromEmail = SalesGuyEmail,
                                    Subject = string.Format("New Invoice From {0}:{1}", Company.CompanyName, inv.InvoiceId),
                                    FromName = SalesGuy,
                                    ToEmail = customer.EmailAddress,
                                    /*InvoicePdf = new Attachment(new MemoryStream(applicationPDFData), inv.InvoiceId + ".pdf")*/
                                };
                                var EmailSent = MailFacade.SendInvoiceCreatedEmail(email, CompanyId);
                                //email.InvoicePdf.Dispose();

                                CustomerAgreement ca = new CustomerAgreement()
                                {
                                    CustomerId = customer.CustomerId,
                                    CompanyId = CompanyId,
                                    InvoiceId = inv.InvoiceId,
                                    Type = LabelHelper.EstimateStatus.SentToCustomer,
                                    AddedDate = DateTime.Now.UTCCurrentTime()
                                };
                                SchedulerFacade.InsertCustomerAgreement(ca);
                            }
                            #endregion

                            #endregion


                            #endregion
                        }
                        catch (Exception e)
                        {
                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\InvoiceReport.txt"), true))
                            {
                                file.WriteLine(string.Format("Exception occured for Customer : {0} ", customer.Id));
                            }
                        }

                    }
                    #endregion
                }
                catch (Exception)
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\InvoiceReport.txt"), true))
                    {
                        file.WriteLine(string.Format("Exception occured for company:{0}", item.CompanyName));
                    }
                }

            }
        }
        #endregion

        #region 5.SendEmailForNotSetCustomerBillingWithinWeek
        private void SendEmailForNotSetCustomerBillingWithinWeek(object sender, System.Timers.ElapsedEventArgs e)
        {
            #region SchedulerRunLog
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\SchedulerStartLogs.txt"), true))
                {
                    file.WriteLine(string.Format("5.SendEmailForNotSetCustomerBillingWithinWeek  run on {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")));

                }
            }
            catch (Exception ex) { }

            #endregion

            CustomerFacade gg = new CustomerFacade();

            MailFacade mail = new MailFacade();
            EmailNotSetCustomerBilling email = new EmailNotSetCustomerBilling();
            List<Customer> NotSetCustomerBill = gg.GetAllCustomerNotSetCustomerBilling();
            foreach (var item in NotSetCustomerBill)
            {
                email.CustomerName = item.FirstName + ' ' + item.LastName;
                email.EmailBody = " ";
                email.ToEmail = item.EmailAddress;
                if (item.EmailAddress != "")
                {
                    mail.EmailNotSetCustomerBilling(email);
                }

            }
        }
        #endregion

        #region 6.SendEmailForEstimateNotConvertedWithinWeek
        private void SendEmailForEstimateNotConvertedWithinWeek(object sender, System.Timers.ElapsedEventArgs e)
        {
            #region SchedulerRunLog
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\SchedulerStartLogs.txt"), true))
                {
                    file.WriteLine(string.Format("6.SendEmailForEstimateNotConvertedWithinWeek  run on {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")));

                }
            }
            catch (Exception) { }

            #endregion

            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(HttpContext.Current.User));
            InvoiceFacade _invoicefacade = new InvoiceFacade();
            MailFacade mail = new MailFacade();
            EmailOfEstimateNotConvertedNotification email = new EmailOfEstimateNotConvertedNotification();

            List<Invoice> EstimateNotConverted = _invoicefacade.GetAllEstimatesNotConvertedToInvoice(currentLoggedIn.CompanyId.Value);
            if (EstimateNotConverted.Count > 0)
            {

            }
            foreach (var item in EstimateNotConverted)
            {
                email.InvoiceCreator = item.CreatedBy;
                email.ToEmail = item.CreatedBy;
                email.EmailBody = "";
            }
        }
        #endregion

        #region 7.EstimateReminderCall
        /*
         *This Method will not check previous months invoices. 
         * 
         * 
         */
        private void EstimateReminderCall()
        {
            #region SchedulerRunLog
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\SchedulerStartLogs.txt"), true))
                {
                    file.WriteLine(string.Format("7.EstimateReminderCall run on {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")));

                }
            }
            catch (Exception e) { }

            #endregion
            SchedulerFacade SchedulerFacadeMaster = new SchedulerFacade();
            SchedulerFacade SchedulerFacade = null;
            MailFacade MailFacade = null;
            ShortUrlFacade ShortUrlFacade = new ShortUrlFacade();
            CustomerSnapshotFacade CustomerSnapshotFacade = null;
            List<Organization> OrgList = SchedulerFacadeMaster.GetAllOrganizations();
            int SelectedDays = 5;
            foreach (var org in OrgList)
            {
                Guid CompanyId = org.CompanyId;
                SchedulerFacade = new SchedulerFacade(org.ConnectionString);
                MailFacade = new MailFacade(org.ConnectionString);
                CustomerSnapshotFacade = new CustomerSnapshotFacade(org.ConnectionString);
                List<Invoice> EstimateList = SchedulerFacade.GetExpiringEstimateList(CompanyId, SelectedDays);
                if (EstimateList == null || EstimateList.Count() == 0)
                {
                    continue;
                }
                foreach (var est in EstimateList)
                {
                    Guid UserId;
                    Customer cust = SchedulerFacade.GetCustomerByCustomerId(est.CustomerId);
                    var EmailTemplate = MailFacade.GetTemplateByTemplateKey(EmailTemplateKey.PredefinedTemplates.EstimatePredefineEmailTemplate);
                    string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(est.CustomerId
                           + "#"
                           + CompanyId
                           + "#"
                           + est.Id);
                    string fullurl = string.Concat(AppConfig.SiteDomain, "/Leads-Estimate/", encryptedurl);
                    ShortUrl ShortUrl = ShortUrlFacade.GetSortUrlByUrl(fullurl, est.CustomerId);
                    string url = AppConfig.ShortSiteDomain + "/shrt/" + ShortUrl.Code;

                    Hashtable datatemplate = new Hashtable();
                    datatemplate.Add("CustomerName", string.Concat(cust.FirstName, " ", cust.LastName));
                    datatemplate.Add("ExpirationDate", est.DueDate);
                    datatemplate.Add("CompanyName", org.CompanyName);
                    datatemplate.Add("url", url);
                    string body = "";
                    string subject = string.Format("Reminder -New Estimate from {0}: {1}", org.CompanyName, est.InvoiceId);


                    InvoiceCreatedEmail email = new InvoiceCreatedEmail()
                    {
                        CompanyName = org.CompanyName,
                        CustomerName = string.Concat(cust.FirstName, " ", cust.LastName),
                        BalanceDue = est.TotalAmount != null ? "$" + est.TotalAmount.Value.ToString("0,0.00") : HS.Web.UI.Helper.LabelHelper.CurrentTransMakeCurrency.MakeCurrency(),
                        DueDate = est.DueDate.HasValue ? est.DueDate.Value.ToString("MM/dd/yy") : "",
                        InvoiceId = est.InvoiceId,
                        Subject = subject,
                        CustomerId = est.CustomerId.ToString(),
                    };


                    if (!string.IsNullOrWhiteSpace(cust.Soldby) && Guid.TryParse(cust.Soldby, out UserId) && UserId != new Guid())
                    {
                        //send sales person notification
                        Employee emp = SchedulerFacade.GetEmployeeByEmployeeId(UserId);
                        if (emp != null)
                        {
                            datatemplate.Add("SalesPhone Number", string.IsNullOrWhiteSpace(emp.Phone) ? org.Phone : emp.Phone);
                            datatemplate.Add("SalesGuy", string.Concat(emp.FirstName, " ", emp.LastName));
                            body = HS.Web.UI.Helper.LabelHelper.ParserHelper(EmailTemplate.BodyContent, datatemplate);
                            email.ToEmail = emp.Email;
                            email.EmailBody = body;
                            email.EmployeeId = cust.Soldby;
                            email.FromEmail = emp.Email.IsValidEmailAddress() ? emp.Email : "info@rmrcloud.com";
                            //FromName = CurrentUser.GetFullName()
                            var bl = MailFacade.SendEstimateCreatedEmail(email, CompanyId);
                            //email.InvoicePdf.Dispose();
                            if (bl)
                            {
                                string empName = "";
                                var empobj = SchedulerFacade.GetEmployeeByEmployeeId(est.CreatedByUid);
                                if (empobj != null)
                                {
                                    empName = empobj.FirstName + " " + empobj.LastName;
                                }
                                CustomerSnapshot objEstimateEmailLog = new CustomerSnapshot()
                                {
                                    CustomerId = est.CustomerId,
                                    CompanyId = CompanyId,
                                    Description = "Estimate:" + "  " + est.InvoiceId + " " + "email sent by " + "<b>" + empName + "</b>",
                                    Logdate = DateTime.Now.UTCCurrentTime(),
                                    Updatedby = "System",
                                    Type = "CustomerMailHistory"
                                };
                                CustomerSnapshotFacade.InsertSnapshot(objEstimateEmailLog);
                            }
                        }
                    }
                    if (!datatemplate.ContainsKey("SalesGuy"))
                    {
                        datatemplate.Add("SalesPhone Number", org.Phone);
                        datatemplate.Add("SalesGuy", org.CompanyName);
                        body = HS.Web.UI.Helper.LabelHelper.ParserHelper(EmailTemplate.BodyContent, datatemplate);

                        email.EmailBody = body;
                        email.EmployeeId = cust.Soldby;
                        email.FromEmail = "info@rmrcloud.com";
                    }
                    if (cust.EmailAddress.IsValidEmailAddress())
                    {
                        //send customer notification

                        email.ToEmail = cust.EmailAddress;
                        var bl = MailFacade.SendEstimateCreatedEmail(email, CompanyId);
                        //email.InvoicePdf.Dispose();
                        if (bl)
                        {
                            string empName = "";
                            var empobj = SchedulerFacade.GetEmployeeByEmployeeId(est.CreatedByUid);
                            if (empobj != null)
                            {
                                empName = empobj.FirstName + " " + empobj.LastName;
                            }
                            CustomerSnapshot objEstimateEmailLog = new CustomerSnapshot()
                            {
                                CustomerId = est.CustomerId,
                                CompanyId = CompanyId,
                                Description = "Estimate:" + "  " + est.InvoiceId + " " + "email sent by " + "<b>" + empName + "</b>",
                                Logdate = DateTime.Now.UTCCurrentTime(),
                                Updatedby = "System",
                                Type = "CustomerMailHistory"
                            };
                            CustomerSnapshotFacade.InsertSnapshot(objEstimateEmailLog);
                        }

                    }

                }
            }
        }
        #endregion

        #region 8.EmailReminder
        public void EmailReminder(DateTime date)
        {
            #region SchedulerRunLog
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\SchedulerStartLogs.txt"), true))
                {
                    file.WriteLine(string.Format("8.EmailReminder  run on {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")));

                }
            }
            catch (Exception e) { }

            #endregion
            SchedulerFacade SchedulerFacadeMaster = new SchedulerFacade();
            SchedulerFacade SchedulerFacade = null;
            MailFacade MailFacade = null;
            SMSFacade SMSFacade = null;
            //Check from
            string TodaysDate = date.AddMinutes(-2).ToString("yyyy-MM-dd  HH:mm") + ":00.000";
            //Check to 
            string TomorrowDate = date.AddMinutes(14).ToString("yyyy-MM-dd HH:mm") + ":59.000";
            List<Organization> OrgList = SchedulerFacadeMaster.GetAllOrganizations();

            foreach (var item in OrgList)
            {
                Guid CompanyId = item.CompanyId;
                SchedulerFacade = new SchedulerFacade(item.ConnectionString);
                MailFacade = new MailFacade(item.ConnectionString);
                SMSFacade = new SMSFacade(item.ConnectionString);
                List<CustomerNote> ReminderList = SchedulerFacade.GetAllCustomerNoteByCompanyIdAndIsSchedule(CompanyId, TodaysDate, TomorrowDate, item.ConnectionString).Where(x => x.IsClose == false).ToList();

                #region send reminder 
                if (ReminderList != null && ReminderList.Count > 0)
                {
                    List<NoteAssign> NoteAssignList = SchedulerFacade.GetAllNoteAssignByNoteIdList(ReminderList.Select(x => x.Id).ToList());

                    #region Employee Id ready
                    List<Guid> UserIdList = new List<Guid>();
                    if (NoteAssignList != null && NoteAssignList.Count > 0)
                    {
                        UserIdList.AddRange(NoteAssignList.Select(x => x.EmployeeId));
                    }
                    UserIdList.AddRange(ReminderList.Select(x => x.CreatedByUid));
                    #endregion

                    List<Employee> employees = SchedulerFacade.GetAllEmplyeeByEmployeeIdList(UserIdList);
                    List<Customer> customers = SchedulerFacade.GetAllCustomerByCustomerIdList(ReminderList.Select(x => x.CustomerId));

                    foreach (CustomerNote Reminder in ReminderList)
                    {
                        Customer objcus = customers.Where(x => x.CustomerId == Reminder.CustomerId).FirstOrDefault();//SchedulerFacade.GetCustomerByCustomerId(Reminder.CustomerId);
                        Employee _createdByName = employees.Where(x => x.UserId == Reminder.CreatedByUid).FirstOrDefault(); //SchedulerFacade.GetEmployeeByEmployeeId(Reminder.CreatedByUid);
                        List<NoteAssign> objassign = NoteAssignList.Where(x => x.NoteId == Reminder.Id).ToList();//SchedulerFacade.GetNoteAssignListByNoteId(Reminder.Id);
                        EmailToEmployeeFromFollowUpNote EmailToEmployeeFromFollowUpNote = new EmailToEmployeeFromFollowUpNote();
                        ReminderSms _remindersms = new ReminderSms();
                        string cusName = objcus.FirstName + " " + objcus.LastName;
                        _remindersms.CusId = objcus.Id;
                        _remindersms.CustomerName = cusName;
                        //var NoteTypeVal = SchedulerFacade.GetLookupByKey("NoteType").Where(x=>x.DataValue == Reminder.NoteType && x.CompanyId == item.CompanyId).FirstOrDefault();
                        //_remindersms.NoteType = NoteTypeVal != null && !string.IsNullOrWhiteSpace(NoteTypeVal.DataValue) && NoteTypeVal.DataValue != "-1" ? NoteTypeVal.DisplayText : "";
                        _remindersms.Message = Reminder.Notes;
                        if (Reminder.ReminderEndDate != null && Reminder.ReminderEndDate != new DateTime())
                        {
                            _remindersms.AttnBy = Reminder.ReminderEndDate.Value.UTCToServerTime().ToString("M/dd/yy");
                        }
                        _remindersms.CreatedBy = _createdByName != null ? _createdByName.FirstName + " " + _createdByName.LastName : "";
                        _remindersms.CreatedDate = Reminder.CreatedDate.UTCToServerTime().ToString("M/dd/yy");
                        _remindersms.CompanyName = item.CompanyName;
                        EmailToEmployeeFromFollowUpNote.CustomerName = cusName;
                        EmailToEmployeeFromFollowUpNote.EmailBody = Reminder.Notes;

                        #region Send Email and SMS TO EMP and CUstomer
                        foreach (var assign in objassign)
                        {
                            if (assign != null && assign.EmployeeId != Guid.Empty)
                            {
                                Employee empobj = employees.Where(x => x.UserId == assign.EmployeeId).FirstOrDefault();//SchedulerFacade.GetEmployeeByEmployeeId(assign.EmployeeId);
                                //Customer objcustomer = SchedulerFacade.GetCustomerByCustomerId(assign.EmployeeId);
                                //Reminder for can be customer//
                                //string textMessage = "Task for " + objcus.FirstName + " " + objcus.LastName + "(" + objcus.Id + "). Task-" + Reminder.Notes
                                //    + "Created by-" + _createdByName.FirstName + " " + _createdByName.LastName
                                //    + "Created on-" + Reminder.CreatedDate.UTCToServerTime().ToString("M/dd/yy")
                                //    + "From-" + item.CompanyName; 
                                if (empobj != null)
                                {
                                    if (Reminder.IsEmail == true && empobj.Email.IsValidEmailAddress())
                                    {
                                        EmailToEmployeeFromFollowUpNote.ToEmail = empobj.Email;
                                        EmailToEmployeeFromFollowUpNote.AssignPersonName = empobj.FirstName + " " + empobj.LastName;
                                        if (Reminder.ReminderEndDate != null && Reminder.ReminderEndDate != new DateTime())
                                        {
                                            EmailToEmployeeFromFollowUpNote.AttnBy = Reminder.ReminderEndDate.Value.UTCToServerTime().ToString("M/dd/yy");
                                        }
                                        EmailToEmployeeFromFollowUpNote.CreatedOn = Reminder.CreatedDate.UTCToServerTime().ToString("M/dd/yy");
                                        if (objcus != null)
                                        {
                                            EmailToEmployeeFromFollowUpNote.CustomerIntId = objcus.Id;
                                        }
                                        if (_createdByName != null)
                                        {
                                            EmailToEmployeeFromFollowUpNote.CreatedByName = _createdByName.FirstName + " " + _createdByName.LastName;
                                        }
                                        MailFacade.EmailToEmployeeFromFollowUpNotes(EmailToEmployeeFromFollowUpNote, Reminder.CompanyId);
                                        //Correspondance Record.
                                    }
                                    if (Reminder.IsText == true && !string.IsNullOrWhiteSpace(empobj.Phone))
                                    {
                                        List<string> receiverlist = new List<string>();
                                        receiverlist.Add(empobj.Phone);
                                        SMSFacade.ReminderFollowupSMS(Reminder.CompanyId, Guid.Empty, _remindersms, receiverlist);
                                        //Correspondance Record.
                                    }
                                }
                                else if (objcus != null)
                                {
                                    if (Reminder.IsEmail == true && objcus.EmailAddress.IsValidEmailAddress())
                                    {
                                        EmailToEmployeeFromFollowUpNote.ToEmail = objcus.EmailAddress;
                                        EmailToEmployeeFromFollowUpNote.AssignPersonName = objcus.FirstName + " " + objcus.LastName;
                                        if (Reminder.ReminderEndDate != null && Reminder.ReminderEndDate != new DateTime())
                                        {
                                            EmailToEmployeeFromFollowUpNote.AttnBy = Reminder.ReminderEndDate.Value.UTCToServerTime().ToString("M/dd/yy");
                                        }
                                        EmailToEmployeeFromFollowUpNote.CreatedOn = Reminder.CreatedDate.UTCToServerTime().ToString("M/dd/yy");
                                        if (objcus != null)
                                        {
                                            EmailToEmployeeFromFollowUpNote.CustomerIntId = objcus.Id;
                                        }
                                        if (_createdByName != null)
                                        {
                                            EmailToEmployeeFromFollowUpNote.CreatedByName = _createdByName.FirstName + " " + _createdByName.LastName;
                                        }
                                        MailFacade.EmailToEmployeeFromFollowUpNotes(EmailToEmployeeFromFollowUpNote, Reminder.CompanyId);
                                        //Correspondance Record.
                                    }
                                    if (Reminder.IsText == true && !string.IsNullOrWhiteSpace(objcus.PrimaryPhone))
                                    {
                                        List<string> receiverlist = new List<string>();
                                        receiverlist.Add(objcus.PrimaryPhone);
                                        SMSFacade.ReminderFollowupSMS(Reminder.CompanyId, Guid.Empty, _remindersms, receiverlist);
                                        //Correspondance Record.
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
                #endregion


            }
        }
        #endregion

        #region 9.Activity Notification Email
        private void ActivityNotification(DateTime date)
        {
            #region SchedulerRunLog
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\SchedulerStartLogs.txt"), true))
                {
                    file.WriteLine(string.Format("9.ActivityNotificationEmailText run on {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")));

                }
            }
            catch (Exception e)
            {
                logger.Error(e);
            }

            #endregion

            SchedulerFacade SchedulerFacadeMaster = new SchedulerFacade();
            SchedulerFacade SchedulerFacade = null;
            MailFacade MailFacade = null;
            SMSFacade SMSFacade = null;
            ShortUrlFacade ShortUrlFacade = new ShortUrlFacade();
            ActivityFacade ActivityFacade = null;
            //Check from
            string FromDate = date.ToString("yyyy-MM-dd  HH:mm") + ":00.000";
            //Check to 
            string ToDate = date.AddMinutes(14).ToString("yyyy-MM-dd HH:mm") + ":59.999";
            List<Organization> OrgList = SchedulerFacadeMaster.GetAllOrganizations();
            foreach (var org in OrgList)
            {
                Guid CompanyId = org.CompanyId;
                ActivityFacade = new ActivityFacade(org.ConnectionString);
                MailFacade = new MailFacade(org.ConnectionString);
                SMSFacade = new SMSFacade(org.ConnectionString);
                SchedulerFacade = new SchedulerFacade(org.ConnectionString);
                List<Activity> ActivityList = SchedulerFacade.GetExpiringActivityListByDueDate(FromDate, ToDate);
                if (ActivityList == null || ActivityList.Count() == 0)
                {
                    continue;
                }
                foreach (var activity in ActivityList)
                {
                    if (activity.NotifyBy == "-1")
                    {
                        continue;
                    }
                    else if (activity.AssignedTo == new Guid())
                    {
                        continue;
                    }
                    else if (activity.NotifyBy == "Email")
                    {
                        Employee objemp = SchedulerFacade.GetEmployeeByEmployeeId(activity.AssignedTo);
                        if (objemp != null && !string.IsNullOrWhiteSpace(objemp.Email))
                        {
                            if (objemp.Email.IsValidEmailAddress())
                            {
                                ActivityNotificationEmail ActivityNotificationEmail = new ActivityNotificationEmail();
                                ActivityNotificationEmail.Name = objemp.FirstName + " " + objemp.LastName;
                                ActivityNotificationEmail.ToEmail = objemp.Email;
                                ActivityNotificationEmail.Body = activity.Note;
                                MailFacade.SendActivityNotificationEmail(ActivityNotificationEmail, CompanyId);
                            }
                        }
                    }
                    else if (activity.NotifyBy == "Text")
                    {
                        Employee objemp = SchedulerFacade.GetEmployeeByEmployeeId(activity.AssignedTo);
                        if (objemp != null && !string.IsNullOrWhiteSpace(objemp.Phone))
                        {
                            ActivityNotificationSMS ActivityNotificationSMS = new ActivityNotificationSMS();
                            ActivityNotificationSMS.ReceiverNumber.Add(objemp.Phone);
                            ActivityNotificationSMS.Message = activity.Note;
                            SMSFacade.SendActivityNotificationSMS(ActivityNotificationSMS, Guid.Empty, CompanyId, ActivityNotificationSMS.ReceiverNumber, false, string.Concat(objemp.FirstName, " ", objemp.LastName));
                        }
                    }
                    else if (activity.NotifyBy == "Both")
                    {
                        Employee objemp = SchedulerFacade.GetEmployeeByEmployeeId(activity.AssignedTo);
                        if (objemp != null)
                        {
                            if (!string.IsNullOrWhiteSpace(objemp.Email))
                            {
                                if (objemp.Email.IsValidEmailAddress())
                                {
                                    ActivityNotificationEmail ActivityNotificationEmail = new ActivityNotificationEmail();
                                    ActivityNotificationEmail.Name = objemp.FirstName + " " + objemp.LastName;
                                    ActivityNotificationEmail.ToEmail = objemp.Email;
                                    ActivityNotificationEmail.Body = activity.Note;
                                    MailFacade.SendActivityNotificationEmail(ActivityNotificationEmail, CompanyId);
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(objemp.Phone))
                            {
                                ActivityNotificationSMS ActivityNotificationSMS = new ActivityNotificationSMS();
                                ActivityNotificationSMS.ReceiverNumber = new List<string>();
                                ActivityNotificationSMS.ReceiverNumber.Add(objemp.Phone);
                                ActivityNotificationSMS.Message = activity.Note;
                                SMSFacade.SendActivityNotificationSMS(ActivityNotificationSMS, Guid.Empty, CompanyId, ActivityNotificationSMS.ReceiverNumber, false, string.Concat(objemp.FirstName, " ", objemp.LastName));
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region 10.Leads Data Sync From CSM
        private void LeadImportFromCMST()
        {
            #region SchedulerRunLog
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\SchedulerStartLogCSM.txt"), true))
                {
                    file.WriteLine(string.Format("10.LeadImportFromCSM run on {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")));
                }
            }
            catch (Exception) { }

            #endregion

            SchedulerFacade SchedulerFacadeMaster = new SchedulerFacade();
            SchedulerFacade SchedulerFacade = null;
            List<Organization> OrgList = SchedulerFacadeMaster.GetAllOrganizations();

            foreach (var org in OrgList)
            {
                Guid CompanyId = org.CompanyId;
                SchedulerFacade = new SchedulerFacade(org.ConnectionString);

                GlobalSetting globset3 = SchedulerFacade.GetGlobalsettingBySearchKeyAndCompanyId("IsAutoLeadImportFromJupiter", CompanyId);
                if (globset3 == null || globset3.Value.ToLower() != "true" || string.IsNullOrWhiteSpace(globset3.Value))
                {
                    continue;
                }

                string SiteId = "";
                GlobalSetting globset1 = SchedulerFacade.GetGlobalsettingBySearchKeyAndCompanyId("SiteId", CompanyId);
                if (globset1 != null)
                {
                    SiteId = globset1.Value;
                }
                string Token = "";
                GlobalSetting globset2 = SchedulerFacade.GetGlobalsettingBySearchKeyAndCompanyId("Token", CompanyId);
                if (globset2 != null)
                {
                    Token = globset2.Value;
                }


                int qty = 1000;
                GlobalSetting globset4 = SchedulerFacade.GetGlobalsettingBySearchKeyAndCompanyId("Quantity", CompanyId);
                if (globset4 != null)
                {
                    qty = globset4.Value.ToInt();
                    if (qty < 100)
                    {
                        qty = 100;
                    }
                }

                int page = 1;
                GlobalSetting globset5 = SchedulerFacade.GetGlobalsettingBySearchKeyAndCompanyId("PageNumber", CompanyId);
                if (globset5 != null)
                {
                    page = globset5.Value.ToInt();
                }


                if (SiteId != "" && Token != "")
                {
                    // var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                    int CusCount = 0;
                    int LeadCount = 0;
                    DateTime Now = DateTime.Now.UTCCurrentTime();


                    GetLeadsRequest request = new GetLeadsRequest();

                    request.siteId = SiteId.ToInt();
                    request.token = Token;
                    request.endDate = new DateTime(Now.Year, Now.Month, Now.Day);
                    Now = Now.AddDays(-1);
                    request.startDate = new DateTime(Now.Year, Now.Month, Now.Day);
                    request.qty = qty;
                    request.page = page;

                    GetLeadsResponseList response = JupiterLeadMigration.GetLeads(request);
                    if (response != null && response.GetLeadsResponse.Count > 0)
                    {
                        //Lookup Lookupstatus = SchedulerFacade.GetLookupByKey("LeadStatus").Where(x => x.DisplayText == "New").FirstOrDefault();
                        string LookupstatusS = "New";
                        //if (Lookupstatus != null)
                        //{
                        //    LookupstatusS = Lookupstatus.DisplayText;
                        //}
                        // Lookup LookupLeadSource = SchedulerFacade.GetLookupByKey("LeadSource").Where(x => x.DisplayText == "Jupiter").FirstOrDefault();
                        string LookupLeadSourceS = "Jupiter";
                        //if (LookupLeadSource != null)
                        //{
                        //    LookupLeadSourceS = LookupLeadSource.DisplayText;
                        //}
                        foreach (var item in response.GetLeadsResponse)
                        {
                            #region if customer directly exists 
                            Customer cus = SchedulerFacade.GetCustomerByCentralStationRefId(item.id);
                            if (cus != null)
                            {
                                if (item.notes != null && item.notes.Count > 0)
                                {
                                    InsertCustomerNotes(item.notes, cus, SchedulerFacade, CompanyId);
                                }
                                continue;
                            }
                            #endregion

                            #region if CustomerContactTrack directly exists
                            CustomerContactTrack CustomerContactTrack = SchedulerFacade.GetCustomerContactTrackByPlatformId(item.id);
                            if (CustomerContactTrack != null)
                            {
                                if (item.notes != null && item.notes.Count > 0)
                                {
                                    InsertCustomerNotes(item.notes, cus, SchedulerFacade, CompanyId);
                                }
                                continue;
                            }
                            #endregion

                            if (string.IsNullOrWhiteSpace(item.email))
                            {
                                item.email = "";
                            }

                            cus = SchedulerFacade.GetCustomerByPhoneNoOrEmail(item.phone, item.email.Replace("'", "''"));
                            if (cus == null)
                            {

                                if (!string.IsNullOrWhiteSpace(item.phone) && item.phone.Length == 12)
                                {
                                    string TempphoneNo = item.phone.Substring(2, item.phone.Length - 2);

                                    long PhoneNumber = 0;
                                    if (long.TryParse(TempphoneNo, out PhoneNumber))
                                    {
                                        item.phone = String.Format("{0:###-###-####}", PhoneNumber);
                                    }
                                }

                                cus = new Customer()
                                {
                                    CentralStationRefId = item.id.ToString(),
                                    CmsRefId = request.siteId.ToString(),
                                    CustomerId = Guid.NewGuid(),
                                    FirstName = item.firstName,
                                    LastName = item.lastName,
                                    CreatedByUid = new Guid("22222222-2222-2222-2222-222222222222"),
                                    CreatedDate = item.timestamp,
                                    PrimaryPhone = item.phone,
                                    EmailAddress = item.email,
                                    City = item.city,
                                    State = item.state,
                                    ZipCode = item.zip,
                                    Street = item.street,
                                    Note = item.comment,
                                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                                    LastUpdatedBy = "system",
                                    LastUpdatedByUid = new Guid("22222222-2222-2222-2222-222222222222"),
                                    ReferringCustomer = Guid.Empty,
                                    ChildOf = Guid.Empty,
                                    AccessGivenTo = Guid.Empty,
                                    IsActive = true,
                                    JoinDate = item.timestamp,
                                    Status = LookupstatusS,
                                    //LeadSource = LookupLeadSourceS,
                                    LeadSource = item.campaign,
                                    //SoldBy2 = new Guid(),
                                    //SoldBy3 = new Guid()
                                };

                                bool soldStatus = true;
                                if (item.soldStatus != null && item.soldStatus == 0)
                                {
                                    soldStatus = true;
                                }
                                else if (item.soldStatus != null)
                                {
                                    soldStatus = false;
                                }
                                CustomerCompany cc = new CustomerCompany()
                                {
                                    CustomerId = cus.CustomerId,
                                    CompanyId = CompanyId,
                                    IsLead = soldStatus,
                                    IsActive = true
                                };

                                CustomerMigration Cm = new CustomerMigration()
                                {
                                    CompanyId = CompanyId,
                                    CreatedBy = new Guid("22222222-2222-2222-2222-222222222222"),
                                    CustomerId = cus.CustomerId,
                                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                                    Platform = LabelHelper.CustomerMigrationPlatforms.Jupiter,
                                    RefenrenceId = item.id,
                                    Note = "ServiceType: " + item.ServiceType,

                                };
                                SchedulerFacade.InsertCustomerMigration(Cm);

                                SchedulerFacade.InsertCustomer(cus);
                                SchedulerFacade.InsertCustomerCompany(cc);

                                CustomerContactTrack model = new CustomerContactTrack()
                                {
                                    CustomerId = cus.CustomerId,
                                    CustomerPlatform = LookupLeadSourceS,
                                    Note = item.recordingUrl,
                                    CreatedDate = item.timestamp,
                                    PlatformId = item.id,
                                };
                                SchedulerFacade.InsertCustomerContactTrack(model);
                                if (soldStatus == true)
                                {
                                    LeadCount++;
                                }
                                else
                                {
                                    CusCount++;
                                }

                            }
                            else
                            {
                                CustomerContactTrack model = new CustomerContactTrack()
                                {
                                    CustomerId = cus.CustomerId,
                                    CustomerPlatform = LookupLeadSourceS,
                                    Note = item.recordingUrl,
                                    CreatedDate = item.timestamp,
                                    PlatformId = item.id,
                                };
                                SchedulerFacade.InsertCustomerContactTrack(model);
                            }
                            InsertCustomerNotes(item.notes, cus, SchedulerFacade, CompanyId);

                            #region Schedule
                            DateTime appointmentDate = new DateTime();

                            if (item.isAppointmentSet.HasValue && item.isAppointmentSet > 0
                                && DateTime.TryParse(item.appointmentDate, out appointmentDate)
                                && appointmentDate != new DateTime())
                            {
                                #region Insert Ticket
                                Guid CustomerId = new Guid();
                                if (cus != null)
                                {
                                    CustomerId = cus.CustomerId;
                                }
                                //else if (cus != null)
                                //{
                                //    CustomerId = cus.CustomerId;
                                //}


                                Ticket Ticket = new Ticket()
                                {
                                    TicketId = Guid.NewGuid(),
                                    CompanyId = CompanyId,
                                    CustomerId = CustomerId,
                                    TicketType = LabelHelper.TicketType.Inspection,
                                    Subject = LabelHelper.TicketType.Inspection,
                                    Message = item.comment,
                                    CreatedBy = new Guid("22222222-2222-2222-2222-222222222222"),
                                    CreatedDate = item.timestamp,
                                    CompletionDate = appointmentDate,
                                    Status = LabelHelper.TicketStatus.Created,
                                    Priority = "",
                                    LastUpdatedBy = new Guid("22222222-2222-2222-2222-222222222222"),
                                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                                    HasInvoice = false,
                                    HasSurvey = false,
                                    IsClosed = false,
                                    IsAgreementTicket = false,
                                    IsDispatch = false,
                                };

                                SchedulerFacade.InsertTicket(Ticket);

                                TicketUser TicketUser = new TicketUser()
                                {
                                    TiketId = Ticket.TicketId,
                                    UserId = new Guid("22222222-2222-2222-2222-222222222222"),
                                    IsPrimary = true,
                                    AddedBy = new Guid("22222222-2222-2222-2222-222222222222"),
                                    NotificationOnly = false,
                                    AddedDate = DateTime.Now.UTCCurrentTime(),
                                };
                                SchedulerFacade.InsertTicketUser(TicketUser);

                                CustomerAppointment CustomerAppointment = new CustomerAppointment()
                                {
                                    AppointmentId = Ticket.TicketId,
                                    CompanyId = CompanyId,
                                    CustomerId = Ticket.CustomerId,
                                    EmployeeId = new Guid("22222222-2222-2222-2222-222222222222"),
                                    AppointmentType = Ticket.TicketType,
                                    AppointmentDate = appointmentDate,
                                    AppointmentStartTime = "12:00",
                                    AppointmentEndTime = "14:00",
                                    IsAllDay = false,
                                    Notes = item.comment,
                                    Status = false,
                                    CreatedBy = "System User",
                                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                                    LastUpdatedBy = "System User"
                                };
                                SchedulerFacade.InsertCustomerAppointment(CustomerAppointment);

                                #endregion
                            }
                            #endregion

                            #region Comments
                            //if (cus == null)
                            //{
                            //    if (!string.IsNullOrWhiteSpace(item.phone) && item.phone.Length == 12)
                            //    {
                            //        string TempphoneNo = item.phone.Substring(2, item.phone.Length - 2);

                            //        long PhoneNumber = 0;
                            //        if (long.TryParse(TempphoneNo, out PhoneNumber))
                            //        {
                            //            item.phone = String.Format("{0:###-###-####}", PhoneNumber);
                            //        }
                            //    }
                            //    cus = new Customer()
                            //    {
                            //        CentralStationRefId = item.id.ToString(),
                            //        CmsRefId = request.siteId.ToString(),
                            //        CustomerId = Guid.NewGuid(),
                            //        FirstName = item.firstName,
                            //        LastName = item.lastName,
                            //        CreatedByUid = new Guid("22222222-2222-2222-2222-222222222222"),
                            //        CreatedDate = item.timestamp,
                            //        PrimaryPhone = item.phone,
                            //        EmailAddress = item.email,
                            //        City = item.city,
                            //        State = item.state,
                            //        ZipCode = item.zip,
                            //        Street = item.street,
                            //        Note = item.comment,
                            //        LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                            //        LastUpdatedBy = "system",
                            //        LastUpdatedByUid = new Guid("22222222-2222-2222-2222-222222222222"),
                            //        ReferringCustomer = Guid.Empty,
                            //        ChildOf = Guid.Empty,
                            //        AccessGivenTo = Guid.Empty,
                            //        IsActive = true,
                            //        JoinDate = item.timestamp,
                            //        Status = LookupstatusS,
                            //        //LeadSource = LookupLeadSourceS
                            //        LeadSource = item.campaign,
                            //    };
                            //    CustomerCompany cc = new CustomerCompany()
                            //    {
                            //        CustomerId = cus.CustomerId,
                            //        CompanyId = CompanyId,
                            //        IsLead = true,
                            //    };
                            //    CustomerMigration Cm = new CustomerMigration()
                            //    {
                            //        CompanyId = CompanyId,
                            //        CreatedBy = new Guid("22222222-2222-2222-2222-222222222222"),
                            //        CustomerId = cus.CustomerId,
                            //        CreatedDate = DateTime.Now.UTCCurrentTime(),
                            //        Platform = LabelHelper.CustomerMigrationPlatforms.Jupiter,
                            //        RefenrenceId = item.id,
                            //        Note = "ServiceType: " + item.ServiceType,

                            //    };

                            //    SchedulerFacade.InsertCustomerMigration(Cm);
                            //    SchedulerFacade.InsertCustomer(cus);
                            //    SchedulerFacade.InsertCustomerCompany(cc);


                            //    CustomerContactTrack model = new CustomerContactTrack()
                            //    {
                            //        CustomerId = cus.CustomerId,
                            //        CustomerPlatform = LookupLeadSourceS,
                            //        Note = item.recordingUrl,
                            //        CreatedDate = item.timestamp,
                            //        PlatformId = item.id,
                            //    };
                            //    SchedulerFacade.InsertCustomerContactTrack(model);

                            //    CusCount++;
                            //}
                            //else
                            //{
                            //    #region if customer already exists just insert Call History
                            //    CustomerContactTrack model = new CustomerContactTrack()
                            //    {
                            //        CustomerId = cus.CustomerId,
                            //        CustomerPlatform = LookupLeadSourceS,
                            //        Note = item.recordingUrl,
                            //        CreatedDate = item.timestamp,
                            //        PlatformId = item.id,
                            //    };
                            //    SchedulerFacade.InsertCustomerContactTrack(model);
                            //    #endregion
                            //}
                            //#region If there is notes for this customer
                            //if (item.notes != null && item.notes.Count > 0)
                            //{
                            //    foreach (var NoteItem in item.notes)
                            //    {
                            //        Guid UserId = new Guid("22222222-2222-2222-2222-222222222222");
                            //        #region Get and update employee
                            //        Employee emp = null;
                            //        if (!string.IsNullOrWhiteSpace(NoteItem.userEmail))
                            //        {
                            //            emp = SchedulerFacade.GetEmployeeByEmailAddress(NoteItem.userEmail);
                            //        }
                            //        if (emp == null)
                            //        {
                            //            emp = SchedulerFacade.GetemployeeByFirstNameAndLastNameOrCSID(NoteItem.userName, NoteItem.userId);
                            //            if (emp == null)
                            //            {
                            //                #region Split FirstName And last name from full name
                            //                string FirstName = "";
                            //                string LastName = "";
                            //                if (!string.IsNullOrWhiteSpace(NoteItem.userName))
                            //                {
                            //                    var str = NoteItem.userName.Replace("  ", " ").Split(' ');
                            //                    if (str.Count() > 0)
                            //                    {
                            //                        var i = 1;
                            //                        FirstName = str[0];
                            //                        for (; i < str.Count() - 1; i++)
                            //                        {
                            //                            FirstName += " " + str[i];
                            //                        }
                            //                        if (str.Count() > 1)
                            //                        {
                            //                            LastName = str[i];
                            //                        }
                            //                        else
                            //                        {
                            //                            LastName = "";
                            //                        }

                            //                    }
                            //                    else
                            //                    {
                            //                        FirstName = NoteItem.userName;
                            //                        LastName = "";
                            //                    }
                            //                }
                            //                #endregion

                            //                #region Insert new employee with email address
                            //                Employee newEmp = new Employee()
                            //                {
                            //                    UserId = Guid.NewGuid(),
                            //                    FirstName = FirstName,
                            //                    LastName = LastName,
                            //                    Email = NoteItem.userEmail,
                            //                    UserName = NoteItem.userEmail,
                            //                    IsActive = false,
                            //                    IsCalendar = false,
                            //                    IsDeleted = false,
                            //                    IsCurrentEmployee = false,
                            //                    IsPayroll = false,
                            //                    IsSalesMatrixUserX = false,
                            //                    IsSupervisor = false,
                            //                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                            //                    CSId = NoteItem.userId,
                            //                    Recruited = true,
                            //                    LastUpdatedBy = "System User",
                            //                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                            //                    CompanyId = CompanyId
                            //                };
                            //                UserLogin ul = new UserLogin()
                            //                {
                            //                    UserName = NoteItem.userEmail,
                            //                    UserId = newEmp.UserId,
                            //                    EmailAddress = newEmp.Email,
                            //                    IsActive = false,
                            //                    IsDeleted = false,
                            //                    IsSupervisor = false,
                            //                    LastUpdatedBy = "System User",
                            //                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                            //                    FirstName = FirstName,
                            //                    LastName = LastName,
                            //                    CompanyId = CompanyId
                            //                };
                            //                UserCompany uc = new UserCompany()
                            //                {
                            //                    CompanyId = CompanyId,
                            //                    UserId = newEmp.UserId,
                            //                    IsDefault = true,
                            //                };

                            //                UserOrganization userOrganization = new UserOrganization()
                            //                {
                            //                    CompanyId = CompanyId,
                            //                    UserName = newEmp.UserName,
                            //                    IsActive = true
                            //                };
                            //                SchedulerFacade.InsertEmployee(newEmp);
                            //                SchedulerFacade.InsertUserLogin(ul);
                            //                SchedulerFacade.InsertUserCompany(uc);
                            //                SchedulerFacade.InsertUserOrganization(userOrganization);
                            //                #endregion
                            //            }
                            //        }
                            //        if (emp != null)
                            //        {
                            //            UserId = emp.UserId;
                            //            if (emp.CSId == null)
                            //            {
                            //                emp.CSId = NoteItem.userId;
                            //                SchedulerFacade.UpdateEmployee(emp);
                            //            }
                            //        }

                            //        #region Previous Condtions
                            //        //Employee emp = SchedulerFacade.GetemployeeByFirstNameAndLastNameOrCSID(NoteItem.userName, NoteItem.userId);
                            //        //if (emp != null)
                            //        //{
                            //        //    UserId = emp.UserId;

                            //        //    if (emp.CSId == null)
                            //        //    {
                            //        //        emp.CSId = NoteItem.userId;
                            //        //        SchedulerFacade.UpdateEmployee(emp);
                            //        //    }
                            //        //}
                            //        #endregion
                            //        #endregion
                            //        CustomerNote Note = new CustomerNote()
                            //        {
                            //            Notes = NoteItem.comment,
                            //            ReminderDate = null,
                            //            ReminderEndDate = null,
                            //            CustomerId = cus.CustomerId,
                            //            CompanyId = CompanyId,
                            //            CreatedDate = NoteItem.dateTime,
                            //            IsEmail = false,
                            //            IsText = false,
                            //            IsShedule = false,
                            //            IsFollowUp = false,
                            //            IsActive = true,
                            //            CreatedBy = NoteItem.userName,
                            //            IsClose = false,
                            //            IsAllDay = false,
                            //            CreatedByUid = new Guid("22222222-2222-2222-2222-222222222222"),
                            //            IsPin = false,
                            //            NoteType = "",
                            //        };
                            //        SchedulerFacade.InsertCustomerNote(Note);
                            //    }
                            //}
                            //#endregion

                            //#region Schedule
                            //DateTime appointmentDate = new DateTime();
                            //if (item.isAppointmentSet.HasValue && item.isAppointmentSet > 0
                            //    && DateTime.TryParse(item.appointmentDate, out appointmentDate) && appointmentDate != new DateTime())
                            //{
                            //    #region Insert Ticket
                            //    Ticket Ticket = new Ticket()
                            //    {
                            //        TicketId = Guid.NewGuid(),
                            //        CompanyId = CompanyId,
                            //        CustomerId = cus.CustomerId,
                            //        TicketType = LabelHelper.TicketType.Inspection,
                            //        Subject = LabelHelper.TicketType.Inspection,
                            //        Message = item.comment,
                            //        CreatedBy = new Guid("22222222-2222-2222-2222-222222222222"),
                            //        CreatedDate = item.timestamp,
                            //        CompletionDate = appointmentDate,
                            //        Status = LabelHelper.TicketStatus.Created,
                            //        Priority = "",
                            //        LastUpdatedBy = new Guid("22222222-2222-2222-2222-222222222222"),
                            //        LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                            //        HasInvoice = false,
                            //        HasSurvey = false,
                            //        IsClosed = false,
                            //        IsAgreementTicket = false,
                            //        IsDispatch = false,
                            //    };

                            //    SchedulerFacade.InsertTicket(Ticket);

                            //    TicketUser TicketUser = new TicketUser()
                            //    {
                            //        TiketId = Ticket.TicketId,
                            //        UserId = new Guid("22222222-2222-2222-2222-222222222222"),
                            //        IsPrimary = true,
                            //        AddedBy = new Guid("22222222-2222-2222-2222-222222222222"),
                            //        NotificationOnly = false,
                            //        AddedDate = DateTime.Now.UTCCurrentTime(),
                            //    };
                            //    SchedulerFacade.InsertTicketUser(TicketUser);

                            //    CustomerAppointment CustomerAppointment = new CustomerAppointment()
                            //    {
                            //        AppointmentId = Ticket.TicketId,
                            //        CompanyId = CompanyId,
                            //        CustomerId = Ticket.CustomerId,
                            //        EmployeeId = new Guid("22222222-2222-2222-2222-222222222222"),
                            //        AppointmentType = Ticket.TicketType,
                            //        AppointmentDate = appointmentDate,
                            //        AppointmentStartTime = "12:00",
                            //        AppointmentEndTime = "14:00",
                            //        IsAllDay = false,
                            //        Notes = item.comment,
                            //        Status = false,
                            //        CreatedBy = "System User"
                            //    };
                            //    SchedulerFacade.InsertCustomerAppointment(CustomerAppointment);

                            //    #endregion
                            //}
                            //#endregion
                            #endregion
                        }
                    }

                }
            }
        }
        private void InsertCustomerNotes(List<CSMNote> notes, Customer cus, SchedulerFacade SchedulerFacade, Guid CompanyId)
        {
            if (notes != null && notes.Count > 0)
            {
                foreach (CSMNote NoteItem in notes)
                {
                    CustomerNote customerNote = SchedulerFacade.GetCustomerNoteByThirdPartyId(NoteItem.noteID);
                    if (customerNote == null)
                    {
                        Guid UserId = new Guid("22222222-2222-2222-2222-222222222222");

                        #region get And Update Employee
                        Employee emp = null;
                        if (!string.IsNullOrWhiteSpace(NoteItem.userEmail))
                        {
                            emp = SchedulerFacade.GetEmployeeByEmailAddress(NoteItem.userEmail);
                        }
                        if (emp == null) //if there is no employee with this email address add new employee
                        {
                            emp = SchedulerFacade.GetemployeeByFirstNameAndLastNameOrCSID(NoteItem.userName, NoteItem.userId);
                            if (emp == null)
                            {
                                #region Split FirstName And last name from full name
                                string FirstName = "";
                                string LastName = "";
                                if (!string.IsNullOrWhiteSpace(NoteItem.userName))
                                {
                                    var str = NoteItem.userName.Replace("  ", " ").Split(' ');
                                    if (str.Count() > 0)
                                    {
                                        var i = 1;
                                        FirstName = str[0];
                                        for (; i < str.Count() - 1; i++)
                                        {
                                            FirstName += " " + str[i];
                                        }
                                        if (str.Count() > 1)
                                        {
                                            LastName = str[i];
                                        }
                                        else
                                        {
                                            LastName = "";
                                        }

                                    }
                                    else
                                    {
                                        FirstName = NoteItem.userName;
                                        LastName = "";
                                    }
                                }
                                #endregion

                                #region Insert new employee with email address
                                Employee newEmp = new Employee()
                                {
                                    CompanyId = CompanyId,
                                    UserId = Guid.NewGuid(),
                                    FirstName = FirstName,
                                    LastName = LastName,
                                    Email = NoteItem.userEmail,
                                    UserName = NoteItem.userEmail,
                                    IsActive = false,
                                    IsCalendar = false,
                                    IsDeleted = false,
                                    IsCurrentEmployee = false,
                                    IsPayroll = false,
                                    IsSalesMatrixUserX = false,
                                    IsSupervisor = false,
                                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                                    CSId = NoteItem.userId,
                                    Recruited = true,
                                    LastUpdatedBy = "System User",
                                    LastUpdatedDate = DateTime.Now.UTCCurrentTime()
                                };
                                UserLogin ul = new UserLogin()
                                {
                                    UserName = NoteItem.userEmail,
                                    UserId = newEmp.UserId,
                                    EmailAddress = newEmp.Email,
                                    IsActive = false,
                                    IsDeleted = false,
                                    IsSupervisor = false,
                                    LastUpdatedBy = "System User",
                                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                                    FirstName = FirstName,
                                    LastName = LastName,
                                    CompanyId = CompanyId
                                };
                                UserCompany uc = new UserCompany()
                                {
                                    CompanyId = CompanyId,
                                    UserId = newEmp.UserId,
                                    IsDefault = true,
                                };

                                UserOrganization userOrganization = new UserOrganization()
                                {
                                    CompanyId = CompanyId,
                                    UserName = newEmp.UserName,
                                    IsActive = true
                                };
                                SchedulerFacade.InsertEmployee(newEmp);
                                SchedulerFacade.InsertUserLogin(ul);
                                SchedulerFacade.InsertUserCompany(uc);
                                SchedulerFacade.InsertUserOrganization(userOrganization);
                                #endregion
                            }
                        }
                        else if (emp != null)
                        {
                            UserId = emp.UserId;
                            if (emp.CSId == null)
                            {
                                emp.CSId = NoteItem.userId;
                                SchedulerFacade.UpdateEmployee(emp);
                            }
                        }
                        #endregion

                        Guid CustomerId = new Guid();
                        if (cus != null)
                        {
                            CustomerId = cus.CustomerId;
                        }

                        #region Customer Note Insert
                        CustomerNote Note = new CustomerNote()
                        {
                            Notes = NoteItem.comment,
                            ReminderDate = null,
                            ReminderEndDate = null,
                            CustomerId = CustomerId,
                            CompanyId = CompanyId,
                            CreatedDate = NoteItem.dateTime,
                            IsEmail = false,
                            IsText = false,
                            IsShedule = false,
                            IsFollowUp = false,
                            IsActive = true,
                            CreatedBy = NoteItem.userName,
                            IsClose = false,
                            IsAllDay = false,
                            CreatedByUid = UserId,
                            IsPin = false,
                            NoteType = "",
                            ThirdPartyId = NoteItem.noteID
                        };
                        SchedulerFacade.InsertCustomerNote(Note);
                        #endregion
                    }
                }
            }

        }
        #endregion

        #region 11.Late Notification for Ticket
        private void LateNotificationTicketScheduler(string date)
        {
            #region SchedulerRunLog
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\SchedulerStartLogs.txt"), true))
                {
                    file.WriteLine(string.Format("11.LateNotificationTicketScheduler run on {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")));
                }
            }
            catch (Exception) { }

            #endregion

            SchedulerFacade SchedulerFacadeMaster = new SchedulerFacade();
            SchedulerFacade SchedulerFacade = null;
            List<Organization> OrgList = SchedulerFacadeMaster.GetAllOrganizations();
            MailFacade MailFacade = null;
            SMSFacade SMSFacade = null;
            GlobalSettingsFacade GlobalSettingsFacade = null;
            List<CustomerAppointment> TicketList = new List<CustomerAppointment>();
            foreach (var org in OrgList)
            {
                Guid CompanyId = org.CompanyId;
                MailFacade = new MailFacade(org.ConnectionString);
                SMSFacade = new SMSFacade(org.ConnectionString);
                SchedulerFacade = new SchedulerFacade(org.ConnectionString);
                GlobalSettingsFacade = new GlobalSettingsFacade(org.ConnectionString);
                string EmailTo = GlobalSettingsFacade.GetNotificationManagerSetting(CompanyId);
                TicketList = SchedulerFacade.GetCustomerAppointmentListByDateSchedule(date, CompanyId);
                if (TicketList != null && TicketList.Count > 0)
                {
                    foreach (var item in TicketList)
                    {
                        LateNotificationTicketEmail LateNotificationTicketEmail = new LateNotificationTicketEmail();
                        LateNotificationTicketEmail.TicketId = item.Id;
                        LateNotificationTicketEmail.CustomerName = item.CustomerName;
                        LateNotificationTicketEmail.ToEmail = EmailTo;
                        MailFacade.LateNotificationTicketEmail(LateNotificationTicketEmail, CompanyId);
                    }
                }
            }
        }
        #endregion

        #region 12.Service Ticket Day Increment
        private void DayIncrementServiceTicketScheduler()
        {
            #region SchedulerRunLog
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\SchedulerStartLogs.txt"), true))
                {
                    file.WriteLine(string.Format("12.DayIncrementServiceTicketScheduler run on {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")));
                }
            }
            catch (Exception) { }

            #endregion

            SchedulerFacade SchedulerFacadeMaster = new SchedulerFacade();
            SchedulerFacade SchedulerFacade = null;
            List<Organization> OrgList = SchedulerFacadeMaster.GetAllOrganizations();
            MailFacade MailFacade = null;
            SMSFacade SMSFacade = null;
            GlobalSettingsFacade GlobalSettingsFacade = null;
            List<Ticket> TicketList = new List<Ticket>();
            foreach (var org in OrgList)
            {
                Guid CompanyId = org.CompanyId;
                MailFacade = new MailFacade(org.ConnectionString);
                SMSFacade = new SMSFacade(org.ConnectionString);
                SchedulerFacade = new SchedulerFacade(org.ConnectionString);
                GlobalSettingsFacade = new GlobalSettingsFacade(org.ConnectionString);
                var objsettings = SchedulerFacade.GetGlobalSettingByKey("DayIncrementServiceTicket");
                if (objsettings != null && objsettings.Value.ToLower() == "true")
                {
                    TicketList = SchedulerFacade.GetTicketListByTicketTypeAndCompanyId(CompanyId);
                    if (TicketList != null && TicketList.Count > 0)
                    {
                        foreach (var item in TicketList)
                        {
                            item.CompletionDate = item.CompletionDate.AddDays(1);
                            SchedulerFacade.UpdateTicket(item);
                            var objappointment = SchedulerFacade.GetAppointmentByAppointmentId(item.TicketId);
                            if (objappointment != null)
                            {
                                objappointment.AppointmentDate = objappointment.AppointmentDate.Value.AddDays(1);
                                SchedulerFacade.UpdateAppointment(objappointment);
                            }
                        }
                    }
                }
            }
        }

        #endregion


        #region 13.CustomerCancellationTask
        private void CustomerCancellationTask()
        {
            SchedulerFacade SchedulerFacadeMaster = new SchedulerFacade();
            SchedulerFacade SchedulerFacade = null;
            ShortUrlFacade ShortUrlFacade = new ShortUrlFacade();
            List<Organization> OrgList = SchedulerFacadeMaster.GetAllOrganizations();
            List<CustomerCancellationQueue> requestedCustomerList = new List<CustomerCancellationQueue>();

            Company Company;

            foreach (var item in OrgList)
            {
                Guid CompanyId = item.CompanyId;
                SchedulerFacade = new SchedulerFacade(item.ConnectionString);
                Company = SchedulerFacade.GetCompanyByCompanyId(CompanyId);
                requestedCustomerList = SchedulerFacade.GetAllCancelledRequestedCustomer();
                foreach (var itemCus in requestedCustomerList)
                {
                    if (itemCus.CancellationDate.Value.Date == DateTime.Now.Date)
                    {
                        try
                        {
                            Customer cus = SchedulerFacade.GetCustomerByCustomerId(itemCus.CustomerId);
                            cus.IsActive = false;
                            cus.CustomerStatus = "9";
                            SchedulerFacade.UpdateCustomer(cus);
                        }
                        catch (Exception) { }
                    }

                }
            }
        }
        #endregion

        #region 15.CustomerUserXCalculation
        private void CustomerUserXTask()
        {
            SchedulerFacade SchedulerFacadeMaster = new SchedulerFacade();
            SchedulerFacade SchedulerFacade = null;
            ShortUrlFacade ShortUrlFacade = new ShortUrlFacade();
            List<Organization> OrgList = SchedulerFacadeMaster.GetAllOrganizations();

            Company Company;

            foreach (var item in OrgList)
            {
                Guid CompanyId = item.CompanyId;
                SchedulerFacade = new SchedulerFacade(item.ConnectionString);
                Company = SchedulerFacade.GetCompanyByCompanyId(CompanyId);

                #region Date calculation
                string UserXFilterWeek = "ThisWeek";
                DateTime EndDate = DateTime.Now;
                DateTime StartDate = DateTime.Now.AddDays(-7);

                GlobalSetting globset = SchedulerFacade.GetGlobalsettingBySearchKeyAndCompanyId("UserXFilterWeek", CompanyId);
                if (globset != null && !string.IsNullOrEmpty(globset.Value))
                {
                    UserXFilterWeek = globset.Value;
                }
                if (UserXFilterWeek == "LastWeekBefore")
                {
                    EndDate = DateTime.Now.AddDays(-7);
                    StartDate = DateTime.Now.AddDays(-14);
                }
                #endregion
                #region Base UserX
                double BaseUserX = 0;
                GlobalSetting globset2 = SchedulerFacade.GetGlobalsettingBySearchKeyAndCompanyId("BaseUserX", CompanyId);
                if (globset2 != null)
                {
                    double.TryParse(globset2.Value, out BaseUserX);
                }
                #endregion

                var UserList = SchedulerFacade.GetAllInsideSalesEmployee();
                foreach (var user in UserList)
                {
                    var CustomerTotalUserX = 0.0;
                    try
                    {
                        var AllUserX = SchedulerFacade.GetAllUserXByUserId(StartDate, EndDate, user.UserId);
                        if (AllUserX != null)
                        {
                            CustomerTotalUserX = BaseUserX + AllUserX.FirstCallUserX + AllUserX.OverallUserX + AllUserX.SoldtofundedUserX + AllUserX.NumberofSalesUserX + AllUserX.AppointmentSetUserX;
                        }

                        var EmployeeDetails = SchedulerFacade.GetEmployeeByEmployeeId(user.UserId);
                        EmployeeDetails.UserXComission = CustomerTotalUserX;
                        SchedulerFacade.UpdateEmployee(EmployeeDetails);
                    }
                    catch (Exception) { }
                }
            }
        }
        #endregion

        #region 16.EvaluationEmailSending
        private void SendEvaluationRemainderEmailScheduler()
        {
            #region SchedulerRunLog
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\SchedulerStartLogs.txt"), true))
                {
                    file.WriteLine(string.Format("8.EmailReminder  run on {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")));

                }
            }
            catch (Exception e) { }

            #endregion
            SchedulerFacade SchedulerFacadeMaster = new SchedulerFacade();
            SchedulerFacade SchedulerFacade = null;
            MailFacade MailFacade = null;

            List<Organization> OrgList = SchedulerFacadeMaster.GetAllOrganizations();

            foreach (var item in OrgList)
            {
                Guid CompanyId = item.CompanyId;
                SchedulerFacade = new SchedulerFacade(item.ConnectionString);
                MailFacade = new MailFacade(item.ConnectionString);

                List<EmployeeEvaluation> evaluationList = SchedulerFacade.GetAllEmplyeeEvaluationList();

                #region send reminder 
                if (evaluationList != null && evaluationList.Count > 0)
                {
                    foreach (var empEvaluation in evaluationList)
                    {
                        Employee emp = SchedulerFacade.GetEmployeeByEmployeeId(empEvaluation.UserId);
                        var ReminderDate = empEvaluation.EvaluationReminderDate.ToDateText();
                        var DateToday = DateTime.Now.Date.ToString("MM/dd/yyyy");
                        if (emp != null && !string.IsNullOrEmpty(emp.SuperVisorId) && emp.SuperVisorId != "-1" && empEvaluation.EvaluationReminderDate.HasValue && ReminderDate == DateToday)
                        {
                            Employee supervisor = SchedulerFacade.GetEmployeeByEmployeeId(new Guid(emp.SuperVisorId));
                            EvaluationRemainderEmail email = new EvaluationRemainderEmail();
                            email.LastEvaluation = empEvaluation.LastEvaluationDate.ToDateText();
                            email.NextEvaluation = empEvaluation.NextEvaluationDate.ToDateText();
                            email.EvaluationType = empEvaluation.EvaluationType;
                            email.CompanyName = item.CompanyName;
                            email.ToEmail = supervisor.Email;
                            email.EmployeeName = emp.FirstName + " " + emp.LastName;
                            MailFacade.EvaluationRemainderEmailSend(email, item.CompanyId);
                        }
                    }
                }
                #endregion

            }
            #endregion

        }
        #endregion

        #region Ieatery Scheduler
        private void OrderExpirationScheduler()
        {
            SchedulerFacade SchedulerFacadeMaster = new SchedulerFacade();
            SchedulerFacade SchedulerFacade = null;
            List<Organization> OrgList = SchedulerFacadeMaster.GetAllOrganizations();

            foreach (var item in OrgList)
            {
                Guid CompanyId = item.CompanyId;
                SchedulerFacade = new SchedulerFacade(item.ConnectionString);

                WebsiteLocation model = new WebsiteLocation();
                model = SchedulerFacade.GetWebsiteLocationByCompanyId(CompanyId);
                if (model != null)
                {
                    var currentdatetime = DateTime.Now.UTCCurrentTime();
                    var checkexptime = currentdatetime.AddMinutes(!string.IsNullOrWhiteSpace(model.ExpireTime) ? -Convert.ToInt32(model.ExpireTime) : -240).ToString("yyyy-MM-dd HH:mm:ss");
                    var orderlistobj = SchedulerFacade.GetAllOrderByExpirationTime(checkexptime);
                    if (orderlistobj != null && orderlistobj.Count > 0)
                    {
                        foreach (var ord in orderlistobj)
                        {
                            ord.IsViewed = true;
                            ord.Status = "Rejected";
                            ord.RejectedReason = "Restaurant is very busy!";
                            ord.RejectedDate = DateTime.Now.UTCCurrentTime();
                            ord.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
                            ord.LastUpdatedBy = "System";
                            SchedulerFacade.UpdateOrder(ord);
                        }
                    }
                }
            }
        }
        private void IeateryTrackingNumberRecord()
        {
            SchedulerFacade SchedulerFacadeMaster = new SchedulerFacade();
            SchedulerFacade SchedulerFacade = null;
            Organization OrgList = SchedulerFacadeMaster.GetAllOrganizations().FirstOrDefault();
            SchedulerFacade = new SchedulerFacade(OrgList.ConnectionString);
            var pauthid = SchedulerFacade.GetGlobalSettingByKey("PlivoAuthId");
            var pauthtoken = SchedulerFacade.GetGlobalSettingByKey("PlivoAuthToken");
            var api = new PlivoApi("MAN2Y3YZU5ZDMWZJEYOD", "OTVlNjg5YmMzMjNlMDc1MjYzMjg0OGQ1MTA0ZDZh");
            List<TrackingNumberRecorded> model = new List<TrackingNumberRecorded>();
            model = SchedulerFacade.GetAllTrackingNumberRecordedByCompanyId();
            var response = api.Call.List(limit: 20, offset: 0);
            if (response.Objects.Count > 0)
            {
                foreach (var rec in response.Objects)
                {
                    var objtrackrec = model.Where(x => x.CallerId.ToString() == rec.CallUuid).FirstOrDefault();
                    if (rec.CallUuid != (objtrackrec != null ? objtrackrec.CallerId.ToString() : ""))
                    {
                        if (!string.IsNullOrWhiteSpace(rec.EndTime))
                        {
                            TrackingNumberRecorded TrackingNumberRecorded = new TrackingNumberRecorded()
                            {
                                TrackingId = new Guid(),
                                CallerId = new Guid(rec.CallUuid),
                                CompanyId = new Guid(),
                                TrackingNumber = "+" + rec.ToNumber,
                                CallerNumber = "+" + rec.FromNumber,
                                Status = "Order",
                                RecordDate = DateTime.Parse(rec.InitiationTime),
                                CreatedBy = new Guid(),
                                LastUpdatedBy = new Guid(),
                                CreatedDate = DateTime.Now.UTCCurrentTime(),
                                LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                                CustomerId = new Guid(),
                                RecordFile = "",
                            };
                            TrackingNumberRecorded.Id = SchedulerFacade.InsertTrackingNumberRecorded(TrackingNumberRecorded);
                        }
                    }
                }
            }
            model = SchedulerFacade.GetAllTrackingNumberRecordedByCompanyId();
            if (model != null && model.Count > 0)
            {
                foreach (var item in model)
                {
                    var objtracknum = SchedulerFacade.GetTrackingNumberSettingByNumber(item.TrackingNumber.Replace("+1", ""));
                    if (objtracknum != null)
                    {
                        var objwebloc = SchedulerFacade.GetWebsiteLocationByCompanyId(objtracknum.CompanyId);
                        if (objwebloc != null)
                        {
                            item.CompanyId = objwebloc.ReferCompanyId != new Guid() ? objwebloc.ReferCompanyId : objwebloc.CompanyId;
                            item.TrackingNumber = objtracknum.TrackingNumber;
                            var objcustomer = SchedulerFacade.GetCustomerByCellNumber(item.CallerNumber.Replace("++1", "").Replace("+1", "").Replace("+", ""));
                            if (objcustomer != null)
                            {
                                item.CustomerId = objcustomer.CustomerId;
                                item.CallerNumber = objcustomer.CellNo;
                            }
                            else
                            {
                                item.CallerNumber = Regex.Replace(item.CallerNumber.Replace("++1", "").Replace("+1", "").Replace("+", ""), @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
                            }
                            SchedulerFacade.UpdateTrackingNumberRecorded(item);
                        }
                    }
                    var recresponse = api.Recording.List(callUuid: item.CallerId.ToString());
                    if (recresponse.Objects != null && recresponse.Objects.Count > 0)
                    {
                        foreach (var record in recresponse.Objects)
                        {
                            item.TrackingId = new Guid(record.RecordingId);
                            item.RecordDate = DateTime.Parse(record.AddTime);
                            item.RecordFile = record.RecordingUrl;
                            item.TalkTimeSeconds = (Convert.ToDouble(record.RecordingDurationMs) / 1000).ToString();
                            SchedulerFacade.UpdateTrackingNumberRecorded(item);
                        }
                    }
                }
            }
        }

        private void IeateryCouponCodeExpire(string date)
        {
            SchedulerFacade SchedulerFacadeMaster = new SchedulerFacade();
            SchedulerFacade SchedulerFacade = null;
            List<Organization> OrgList = SchedulerFacadeMaster.GetAllOrganizations();

            foreach (var item in OrgList)
            {
                Guid CompanyId = item.CompanyId;
                SchedulerFacade = new SchedulerFacade(item.ConnectionString);

                List<RestaurantCoupons> model = new List<RestaurantCoupons>();
                model = SchedulerFacade.GetAllRestaurantCouponsByCompanyIdAndEndDate(CompanyId, date);
                if (model != null && model.Count > 0)
                {
                    foreach (var item1 in model)
                    {
                        item1.Status = false;
                        SchedulerFacade.UpdateCoupons(item1);
                    }
                }
            }
        }

        private void IeateryAutoConfirmOrder()
        {
            SchedulerFacade SchedulerFacadeMaster = new SchedulerFacade();
            SchedulerFacade SchedulerFacade = null;
            List<Organization> OrgList = SchedulerFacadeMaster.GetAllOrganizations();

            foreach (var item in OrgList)
            {
                Guid CompanyId = item.CompanyId;
                SchedulerFacade = new SchedulerFacade(item.ConnectionString);

                List<ResturantOrder> model = new List<ResturantOrder>();
                var systemsettingobj = SchedulerFacade.GetSystemSettingByCompanyId(CompanyId);
                if (systemsettingobj != null && systemsettingobj.AutoConfirmOrder.HasValue && systemsettingobj.AutoConfirmOrder.Value == true)
                {
                    var currentdatetime = DateTime.Now.UTCCurrentTime();
                    model = SchedulerFacade.GetAllAcceptedOrdersByCompanyId(CompanyId);
                    if (model != null && model.Count > 0)
                    {
                        foreach (var item1 in model)
                        {
                            if (item1.Status.ToLower() == "accepted" && item1.AcceptDate.HasValue && item1.AcceptDate.Value <= currentdatetime)
                            {
                                if (item1.OrderType.ToLower() == "pickup")
                                {
                                    item1.Status = "Pickedup";
                                }
                                else if (item1.OrderType.ToLower() == "delivery")
                                {
                                    item1.Status = "Delivered";
                                }

                                SchedulerFacade.UpdateOrder(item1);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Employee PTO Hours Calculation Scheduler

        private void EmployeePTOHoursCalculationTimer(object sender, System.Timers.ElapsedEventArgs e)
        {
            DateTime currenttime = DateTime.Now;
            string Now = currenttime.ToString("h:mm tt");
            string NowDay = currenttime.DayOfWeek.ToString();
            string start = ConfigurationManager.AppSettings["EmployeePTOHoursCalculationTime"];
            string startDay = ConfigurationManager.AppSettings["EmployeePTOHoursCalculationDay"];
            var triger = start.Equals(Now);
            var triger2 = startDay.Equals(NowDay);
            if (triger && triger2)
            {
                EmployeePTOHoursCalculation(currenttime);
            }
        }

        private void EmployeePTOHoursCalculation(DateTime Today)
        {
            try
            { 
                SchedulerFacade SchedulerFacadeMaster = new SchedulerFacade();
                SchedulerFacade SchedulerFacade = null;
                List<Organization> OrgList = SchedulerFacadeMaster.GetAllOrganizations();
                Organization Org = OrgList.LastOrDefault();
                OrgList.Clear();
                OrgList.Add(Org);
                bool result = false;
                int errorcount = 0;
                foreach (var item in OrgList)
                {
                    int EmpCount = 0;
                    var TotalWorkinghours = 0.0;
                    var TotalsalaryWorkinghours = 0.0;
                    var OldTotalWorkinghours = 0.0;
                    double EmployeeLogWorkinghours = 0.0;
                    Guid CompanyId = item.CompanyId;
                    SchedulerFacade = new SchedulerFacade(item.ConnectionString); 
                    DateTime FromDay = Today.AddDays(-(int)Today.DayOfWeek - 14).SetZeroHour(); 
                    DateTime EndDay = FromDay.AddDays(6).SetZeroHour(); 
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\19.txt"), true))
                    {
                        file.WriteLine("Employee PTO Hours Calculation: " + Today.ToString("MM/dd/yyyy hh:mm tt"));
                        file.Close();
                    }
                    do
                    {
                        List<Employee> list = SchedulerFacade.GetAllEmployeeByHireDate(FromDay, EndDay);
                        List<EmployeePTOHourLog> clockInuserdata = SchedulerFacade.GetAllEmployeeTimeClockByPaytype(FromDay, EndDay); 

                        if (list != null && list.Count > 0)
                        {
                            EmpCount = list.Count;
                            foreach (var hireitem in list)
                            {
                                try
                                {
                                    TimeSpan difference = EndDay - hireitem.HireDate.Value;
                                    int totalDays = difference.Days;
                                    double PtoRate = 0.0;
                                    var totalPtohour = 0.0;
                                    var TotalWorkingSeconds = 0.0;
                                    if (totalDays > 0 && !string.IsNullOrWhiteSpace(hireitem.PayType))
                                    {
                                        EmployeePtoAccrualRate PtoAccrualRate = SchedulerFacade.GetEmployeePtoAccrualRate(totalDays, hireitem.PayType);

                                        if (PtoAccrualRate != null)
                                        {
                                            if (totalDays >= PtoAccrualRate.MinimumDay && totalDays <= PtoAccrualRate.MaximumDay)
                                            {
                                                PtoRate = PtoAccrualRate.AccrualRate.HasValue ? PtoAccrualRate.AccrualRate.Value : 0.0;
                                            }
                                        }
                                    }
                                    if (clockInuserdata != null && clockInuserdata.Where(x=>x.UserId == hireitem.UserId).Count() > 0)
                                    {
                                        TotalWorkingSeconds = clockInuserdata.Where(x => x.UserId == hireitem.UserId).Select(x => x.TotalWorkingSeconds).FirstOrDefault();
                                        if (TotalWorkingSeconds > 0)
                                        {
                                            if(hireitem.PayType == "Hourly")
                                            {
                                                TotalWorkinghours = (TotalWorkingSeconds / 3600);
                                                OldTotalWorkinghours = TotalWorkinghours;
                                                if (TotalWorkinghours > 40.00)
                                                {
                                                    TotalWorkinghours = 40.00;
                                                }
                                                else
                                                {
                                                    TotalWorkinghours = TotalWorkinghours;
                                                }
                                                EmployeeLogWorkinghours = OldTotalWorkinghours;
                                                totalPtohour = PtoRate * TotalWorkinghours;
                                            }
                                            else
                                            {
                                                if (TotalWorkingSeconds > 0)
                                                {
                                                    TotalsalaryWorkinghours = (TotalWorkingSeconds / 3600);
                                                    if(TotalsalaryWorkinghours > 40.00)
                                                    {
                                                        TotalsalaryWorkinghours = 40.00;
                                                    }
                                                    else
                                                    {
                                                        TotalsalaryWorkinghours = TotalsalaryWorkinghours;
                                                    }
                                                    EmployeeLogWorkinghours = Math.Round(TotalsalaryWorkinghours, 2);
                                                    totalPtohour = PtoRate;
                                                }
                                                else
                                                {
                                                    totalPtohour = PtoRate;
                                                    EmployeeLogWorkinghours = TotalWorkingSeconds;
                                                }
                                            }
                                            
                                        }
                                    }
                                    else if(hireitem.PayType == "Salary")
                                    {
                                        totalPtohour = PtoRate;
                                        EmployeeLogWorkinghours = TotalWorkingSeconds;
                                    }
                                    else
                                    {
                                        totalPtohour = 0;
                                        EmployeeLogWorkinghours = TotalWorkingSeconds;
                                    }
                                    EmployeePTOHourLog employeePTOHourLog = new EmployeePTOHourLog()
                                    {
                                        UserId = hireitem.UserId,
                                        FromDate = FromDay,
                                        EndDate = EndDay,
                                        PTOHour = totalPtohour,
                                        LastUpdatedDate = Today,
                                        CreatedDate = Today,
                                        WorkingHours = EmployeeLogWorkinghours,
                                        PtoRate = PtoRate
                                    };
                                    result = SchedulerFacade.InsertEmployeePTOHourLog(employeePTOHourLog) > 0; 
                                    if (result)
                                    {
                                        var ptodatas = SchedulerFacade.GetEmployeeTotalPtoHour(hireitem.UserId).FirstOrDefault();
                                        hireitem.PtoHour = ptodatas.TotalPTOHour;
                                        hireitem.PtoRate = PtoRate;
                                        SchedulerFacade.UpdateEmployee(hireitem);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\19.txt"), true))
                                    {
                                        file.WriteLine("Error: " + ex + " | Emp#" + hireitem.Id + " " + Today.ToString("MM/dd/yyyy hh:mm tt"));
                                        file.Close();
                                    }
                                    continue;
                                }
                                EmpCount--;
                            }
                            if (EmpCount < 1) { errorcount = 3; }
                        }
                        else
                        {
                            errorcount = 3;
                        }
                        errorcount++;
                    }
                    while (EmpCount > 0 || errorcount < 4);
                }
            }
            catch (Exception ex)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\19.txt"), true))
                {
                    file.WriteLine("Error: " + ex + " " + Today.ToString("MM/dd/yyyy hh:mm tt"));
                    file.Close();
                }
            }
        }
        #endregion
    }
}