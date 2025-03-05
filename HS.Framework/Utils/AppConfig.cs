using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading;
using System.Globalization;
using System.Data.SqlClient;

namespace HS.Framework.Utils
{
    public class AppConfig
    {
        public static string GetIP
        {
            get
            {
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (!string.IsNullOrEmpty(ipAddress))
                {
                    string[] addresses = ipAddress.Split(',');
                    if (addresses.Length != 0)
                    {
                        return addresses[0];
                    }
                }

                return context.Request.ServerVariables["REMOTE_ADDR"];
            }
        }
        public static string GetUserAgent
        {
            get
            { 
                return System.Web.HttpContext.Current.Request.UserAgent;
            }
        }
        public static string WebPort
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["WebPort"];
            }
        }

        public static string RootLink
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["WebRoot"];
            }
        }
        public static string DomainSitePath
        {
            get
            {
                return ConfigurationManager.AppSettings["UrlFixer"];
            }
        }
        public static string HomePageUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["HomePageUrl"];
            }
        }
        public static string  CustomerTemplateFile
        {
            get
            {
                return ConfigurationManager.AppSettings["File.CustomerTemplate"];
            }
        }

        public static string LeadTemplateFile
        {
            get
            {
                return ConfigurationManager.AppSettings["File.LeadTemplate"];
            }
        }
        public static string ImageDomain
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["ImageDomain"];
            }
        }
        public static string UploadExactPath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["UploadExactPath"];
            }
        }
        public static string APIPrefix
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["APIPrefix"];
            }
        }
        public static bool ImageDebug
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["ImageDebug"].ToLower() == "true";
            }
        }

        public static bool SecurityOn
        {
            get
            {
                return bool.Parse(System.Configuration.ConfigurationManager.AppSettings["SecurityOn"]);
            }
        }
        public static bool iEateryRestaurantSetup
        {
            get
            {
                return bool.Parse(System.Configuration.ConfigurationManager.AppSettings["iEateryRestaurantSetup"]);
            }
        }
        public static DateTime ApplicationMinDate
        {
            get
            {  
                return new DateTime(1900, 04, 30, new CultureInfo("en").Calendar);    
            }
        }

        public static DateTime ApplicationMaxDate
        {
            get
            {
                return new DateTime(2029, 05, 13, new CultureInfo("en").Calendar);
            }
        }
        
        public static string DomainName
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["DomainName"];
            }
        }
        public static string SiteDomain
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["SiteDomain"];
            }
        }
        public static string ShortSiteDomain
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["ShortSiteDomain"];
            }
        }
        public static string SiteAPIDomain
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["SiteAPIDomain"];
            }
        }

        public static string DebugMode
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["DebugMode"];
            }
        }

        public static string DatabaseName
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["DatabaseName"];
            }
        }

        public static string DatabaseNameNew
        {
            get
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(System.Configuration.ConfigurationManager.ConnectionStrings["hsCoonectionString"].ConnectionString);
                return builder.InitialCatalog;
            }
        }

        public static string PDFGeneratorLink
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["PDFGeneratorLink"];
            }
        }

        public static int BranchLimit
        {
            get
            {
                return int.Parse(System.Configuration.ConfigurationManager.AppSettings["BranchLimit"]);
            }
        }
        public static int UserLimit
        {
            get
            {
                return int.Parse(System.Configuration.ConfigurationManager.AppSettings["UserLimit"]);
            }
        }

        public static bool HasProxy
        {
            get
            {
                return Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["HasProxy"]);
            }
        }

        public static string ProxyAddress
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["ProxyAddress"];
            }
        }

        public static int ProxyPort
        {
            get
            {
                return Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ProxyPort"]);
            }
        }

        public static bool LanguageEditEnabled
        {
            get
            {
                return Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["LanguageEditorEnabled"]);
            }
        }

        public static bool ReportMappingEditorEnable
        {
            get
            {
                return Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["ReportMappingEditorEnabled"]);
            }
        }

        public static string HelpLink
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["HelpLink"];
            }
        }
        
            public static string Production
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["Production"];
            }
        }
        public static string APIAccountName
        {
            get
            {
                return  System.Configuration.ConfigurationManager.AppSettings["APIAccountName"];
            }
        }
        public static string APIAccountPassword
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["APIAccountPassword"];
            }
        }
        public static string Signature
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["Signature"];
            }
        }

        public static bool UseSandBox
        {
            get
            {
                return bool.Parse(System.Configuration.ConfigurationManager.AppSettings["UseSandBox"]);
            }
        }
        public static string TransactionMode
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["TransactionMode"];
            }
        }
        public static decimal RiyalsPerDoller
        {
            get
            {
                return Decimal.Parse(System.Configuration.ConfigurationManager.AppSettings["RiyalsPerDoller"].ToString());
            }
        }
        public static string FeedbackEmail
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["FeedbackEmail"];
            }
        }
        public static string MailServer
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["MailServer"];
            }
        }
        public static string SenderEmail
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["SenderEmail"];
            }
        }
        public static string SenderPass
        {
            get
            {              //talibmMiah1$in talibMainMail1$
                return "talibMainMail1$";
            }
        }
        public static DateTime GetServerTime
        {
            get
            {
                return DateTime.Now.AddHours(double.Parse(System.Configuration.ConfigurationManager.AppSettings["AddHour"]));
            }
        }
        public static string ImageLink
        {
            get
            {
                return "http://talibconsultancy.co.uk/content/logo.png";
            }
        }
        public static string OldDb
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["olddb"];
            }
        }
        public static string NewDb
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["newdb"];
            }
        }
        public static string HomePageImage
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["HomePageImage"];
            }
        }
        public static string PublicPageTitle
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["PublicPageTitle"];
            }
        }
        public static string PrivatePageTitle
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["PrivatePageTitle"];
            }
        }
        public static string UccUrl
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["UccUrl"];
            }
        }
        public static string AWSS3Url
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["AWS.S3Url"];
            }
        }

        public static string AWSS3BucketName
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["AWS.S3BucketName"];
            }
        }
    }
}
