using HS.Entities;
using HS.Facade;
using HS.Framework;
using HS.Framework.Utils;
using HS.Web.UI.App_Start;
using HS.Web.UI.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;


namespace HS.Web.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //private IDisposable _sentry;
        protected void Application_Start()
        {
            // We add the query logging here so multiple DbContexts in the same project are supported
            //SentryDatabaseLogging.UseBreadcrumbs();

            // Set up the sentry SDK
            //_sentry = SentrySdk.Init(o =>
            //{
                // We store the DSN inside Web.config; make sure to use your own DSN!
            //    o.Dsn = ConfigurationManager.AppSettings["SentryDsn"];
                // Get ASP.NET integration
            //    o.AddAspNet();
                // Get Entity Framework integration
            //    o.AddEntityFramework();
            //});

            
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            

            AreaRegistration.RegisterAllAreas(); 
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //StartSendEmailForNotConvertedToCustomerWithinWeek();
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            if (ConfigurationManager.AppSettings["EligibleToRunScheduler"] == "true")
            {
                ///StartMainShceduler();
                #region 0. SchedulerRunner
                SchedulerRunner();
                #endregion

                #region 1.SubscriptionStatusChecker
                if (ConfigurationManager.AppSettings["EligibleToRunSubscriptionStatusScheduler"] == "true")
                {
                    //1.SubscriptionStatusChecker
                    SubscriptionStatusCheckerScheduler();
                }
                #endregion

                #region 2.ChecK Invoice Payments
                if(ConfigurationManager.AppSettings["EligibleToRunInvoicePaymentsScheduler"] == "true")
                {
                    ////2.ChecK Invoice Payments
                    StartCheckingInvoicePayments();
                }
                #endregion

                ////3.ReminderNotificationTasks 
                ReminderNotificationScheduler();

                #region 4.GenerateInvoice
                if(ConfigurationManager.AppSettings["EligibleToRunGeneratingInvoiceScheduler"] == "true")
                {
                    ////4.GenerateInvoice
                    StartGeneratingInvoice();
                }
                #endregion

                ////7.EstimateReminderCall
                //EstimateReminder();

                #region 8.EmailReminder
                if (ConfigurationManager.AppSettings["EligibleToRunEmailReminderScheduler"] == "true")
                {
                    ////8.EmailReminder
                    EmailReminderTimmer();
                }
                #endregion

                #region 9.Activity Notification Email Text 
                if (ConfigurationManager.AppSettings["EligibleToRunActivityNotificationScheduler"] == "true")
                {
                    ////9.ActivityNotificationEmailText
                    NotificationEmailTextActivityScheduler();
                }
                #endregion

                #region 10.Leads Data Sync From CSM 
                if (ConfigurationManager.AppSettings["EligibleToRunLeadImportScheduler"] == "true")
                {
                    ////10.Leads Data Sync From CSM
                    LeadImportFromCMS();
                }
                #endregion

                #region 11.Ticket Notification for Rug Cleaning
                if(ConfigurationManager.AppSettings["EligibleToRunLateNotificationForTicketScheduler"] == "true")
                {
                    ////11.Ticket Notification for Rug Cleaning
                    LateNotificationForTicket();
                }
                #endregion

                #region 12.Service Ticket Day Increment // Only for Grate
                if (ConfigurationManager.AppSettings["EligibleToRunDayIncrementServiceForTicketScheduler"] == "true")
                {
                    ////12.Service Ticket Day Increment
                    DayIncrementServiceForTicket();
                }
                #endregion

                #region 13.Cancell Customer //Only for DFW
                if(ConfigurationManager.AppSettings["EligibleToRunCustomerCancellationScheduler"] == "true")
                {
                    ////13.Cancell Customer //Only for DFW
                    CustomerCancellationScheduler();
                }
                #endregion

                #region 15.UserX Calculation //Only for DFW
                if (ConfigurationManager.AppSettings["EligibleToRunUserXCalculation"] == "true")
                {
                    ////13.Cancell Customer //Only for DFW
                    CustomerUserXCalculation();
                }
                #endregion

                #region 16.ARB Invoice Generation For Payment Method ACH and CC
                if (ConfigurationManager.AppSettings["EligibleToRunARBInvoiceForACHAndCC"] == "true")
                {
                    ////16
                    StartCreatingRecurringBillingInvoicesForACHAndCC();
                    CheckPaymentsForACHAndCCInvoices();
                }
                #endregion

                #region 17.ARB Invoice Generation
                if (ConfigurationManager.AppSettings["EligibleToRunRecurringBillingInvoice"] == "true")
                {
                    ////17
                    OwnBillingInvoiceGeneration();
                }
                #endregion

                #region 18.Evaluation Remainder
                if (ConfigurationManager.AppSettings["EligibleToRunEvaluationRemainderEmailScheduler"] == "true")
                {
                    ////17
                    EvaluationReminder();
                }
                #endregion

                #region 19.Employee PTO Hours Calculation Scheduler
                if (ConfigurationManager.AppSettings["EligibleToRunEmployeePTOHoursCalculationScheduler"] == "true")
                {
                    ////19
                    EmployeePTOHoursCalculationScheduler();
                }
                #endregion
            }
            if (ConfigurationManager.AppSettings["IeateryScheduler"] == "true")
            {
                StartOrderExpiration();
                StartTracking();
            }
        }
        //protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        //{
        //    //HttpContext.Current.Response.Headers.Remove("X-Powered-By");
        //    //HttpContext.Current.Response.Headers.Remove("X-AspNet-Version");
        //    //HttpContext.Current.Response.Headers.Remove("X-AspNetMvc-Version");
        //    //HttpContext.Current.Response.Headers.Remove("Server");
        //}s
        //protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        //{

        //}

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            //if (HttpContext.Current.Cache["rmrJobcache"] == null)
            //{
            //    HttpContext.Current.Cache.Add("rmrJobcache"
            //        , "rmrJobcacheval"
            //        , null
            //        , DateTime.MaxValue
            //        , TimeSpan.FromMinutes(1)
            //        , System.Web.Caching.CacheItemPriority.Default, RemoveRmrJobCache);
            //}
            //Context.StartSentryTransaction();

        }
        
        protected void Application_EndRequest()
        {
            //Context.FinishSentryTransaction();
        }

        //private void RemoveRmrJobCache(string key, object value, CacheItemRemovedReason reason )
        //{
        //    var client = new WebClient();
        //    client.DownloadData(Framework.Utils.AppConfig.SiteDomain + "/Home/Croncall");
        //    DateTime currenttime = DateTime.Now; 
        //    using (System.IO.StreamWriter file = new System.IO.StreamWriter(System.Web.Hosting.HostingEnvironment.MapPath(@"~\SchedulerReports\running_Task.txt"), true))
        //    {
        //        file.WriteLine(Framework.Utils.AppConfig.SiteDomain + " : Fire scheduler: " + currenttime.ToString("dd/MM/yyyy hh:mm:ss.fff"));
        //        file.Close();
        //    }

           
        //    StartMainShceduler();
             
        //}
        protected void Application_Error(object sender, EventArgs e)
        {
            // Get the exception object.
            Exception exc = Server.GetLastError();
            //SentrySdk.CaptureException(exc);

            Elmah.ErrorSignal.FromCurrentContext().Raise(exc);
            //Handle HTTP errors
            //if (!string.IsNullOrWhiteSpace(exc.Message))
            //{
            //    Response.RedirectPermanent("/ErrorPage");
            //}
            if (exc.GetType() == typeof(HttpException))
            {

                // The Complete Error Handling Example generates
                // some errors using URLs with "NoCatch" in them;
                // ignore these here to simulate what would happen
                // if a global.asax handler were not implemented.
                if (exc.Message.Contains("NoCatch") || exc.Message.Contains("maxUrlLength"))
                    return;
                //else if(exc.Message.Contains("The controller for path '/"))
                //{
                //    Response.RedirectPermanent("/App/Error");
                //}
                if(AppConfig.DebugMode == "false") { 
                    //string errorcode = "500";

                    if (exc.Message.ToLower().Contains("not found"))
                    {
                        // errorcode = "404";
                    }
                    //Send email to administrator with detail log. 
                }
                //Response.RedirectPermanent("/errorpage?code=" + errorcode);
                //Redirect HTTP errors to HttpError page
                //Server.Transfer("HttpErrorPage.aspx");
            }
            //Response.RedirectPermanent("/Home/ErrorFound");
        }
        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            if (User.Identity.IsAuthenticated)
            {
                //we will Try to make it more efficient later 
                if (HttpContext.Current.Session != null)
                {
                    UserLogin AppUser = new UserInitializer().GetCurrentUser(User.Identity.Name, new Guid());
                    CustomPrincipal UserPrincipal = new CustomPrincipal(AppUser, User.Identity);
                    HttpContext.Current.User = UserPrincipal; 
                }
            }
        }

        //protected void Application_End()
        //{
            // Close the Sentry SDK (flushes queued events to Sentry)
        //    _sentry?.Dispose();
        //}

        //private void StartSendEmailForNotConvertedToCustomerWithinWeek()
        //{
        //    new HS.Web.UI.Helper.ManageScheduleTasks().StartSendEmailForNotConvertedToCustomerWithinWeek();
        //}
        //private void StartMainShceduler()
        //{
        //    new HS.Web.UI.Helper.ManageScheduleTasks().StartMainShceduler();
        //}
        private void SchedulerRunner()
        {
            new HS.Web.UI.Helper.ManageScheduleTasks().SchedulerRunner();
        }
        private void StartCreatingRecurringBillingInvoicesForACHAndCC()
        {
            new HS.Web.UI.Helper.ManageRecurringBillingSchedulers().StartCreatingRecurringBillingInvoicesForACHAndCC();
        }
        private void OwnBillingInvoiceGeneration()
        {
            new HS.Web.UI.Helper.ManageRecurringBillingSchedulers().OwnBillingInvoiceGeneration();
        }
        private void CheckPaymentsForACHAndCCInvoices()
        {
            new HS.Web.UI.Helper.ManageRecurringBillingSchedulers().CheckPaymentsForACHAndCCInvoices();
        }
        private void StartGeneratingInvoice()
        {
            new HS.Web.UI.Helper.ManageScheduleTasks().StartGeneratingInvoice();
        }
        private void EmailReminderTimmer()
        {
            new HS.Web.UI.Helper.ManageScheduleTasks().EmailReminderTimmer();
        }
        private void StartCheckingInvoicePayments()
        {
            new HS.Web.UI.Helper.ManageScheduleTasks().StartCheckingInvoicePayments();
        }
        private void EvaluationReminder()
        {
            new HS.Web.UI.Helper.ManageScheduleTasks().SendEvaluationRemainderEmail();
        }
        private void EmployeePTOHoursCalculationScheduler()
        {
            new HS.Web.UI.Helper.ManageScheduleTasks().EmployeePTOHoursCalculationScheduler();
        }
        private void EstimateReminder()
        {
            new HS.Web.UI.Helper.ManageScheduleTasks().EstimateReminder();
        }
        private void ReminderNotificationScheduler()
        {
            new HS.Web.UI.Helper.ManageScheduleTasks().ReminderNotificationScheduler();
        }
        private void CustomerCancellationScheduler()
        {
            new HS.Web.UI.Helper.ManageScheduleTasks().CustomerCancellationScheduler();
        }
        private void SubscriptionStatusCheckerScheduler()
        {
            new HS.Web.UI.Helper.ManageScheduleTasks().StartCheckingSubscriptionStatus();
        }
        private void NotificationEmailTextActivityScheduler()
        {
            new HS.Web.UI.Helper.ManageScheduleTasks().StartCheckingActivityNotificationStatus();
        }
        private void LeadImportFromCMS()
        {
            new HS.Web.UI.Helper.ManageScheduleTasks().StartCheckingLeadImportFromCMS();
        }
        private void LateNotificationForTicket()
        {
            new HS.Web.UI.Helper.ManageScheduleTasks().StartCheckingLateNotificationForTicket();
        }
        private void DayIncrementServiceForTicket()
        {
            new HS.Web.UI.Helper.ManageScheduleTasks().StartCheckingDayIncrementServiceForTicket();
        }
        private void CustomerUserXCalculation()
        {
            new HS.Web.UI.Helper.ManageScheduleTasks().CustomerUserXCalculationScheduler();
        }

        private void StartOrderExpiration()
        {
            new HS.Web.UI.Helper.ManageScheduleTasks().StartOrderExpirationScheduler();
        }

        private void StartTracking()
        {
            new HS.Web.UI.Helper.ManageScheduleTasks().StartTrackingNumberScheduler();
        }
    }
}
