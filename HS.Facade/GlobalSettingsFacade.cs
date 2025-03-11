using HS.DataAccess;
using HS.Entities;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HS.Facade
{

    public class GlobalSettingsFacade : BaseFacade
    {

        GlobalSettingDataAccess _GlobalSettingDataAccess;
        

        public GlobalSettingsFacade(ClientContext clientContext)
            : base(clientContext)
        {
            if (_GlobalSettingDataAccess == null)
                _GlobalSettingDataAccess = (GlobalSettingDataAccess)_ClientContext[typeof(GlobalSettingDataAccess)];
        }
        public GlobalSettingsFacade()
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null && !string.IsNullOrWhiteSpace(HttpContext.Current.Session[SessionKeys.CompanyConnectionString].ToString()))
            {
                string CompanyConnectionstr = HttpContext.Current.Session[SessionKeys.CompanyConnectionString].ToString();

                if (_GlobalSettingDataAccess == null)
                    _GlobalSettingDataAccess = new GlobalSettingDataAccess(CompanyConnectionstr);
            }
            else
            {
                _GlobalSettingDataAccess = new GlobalSettingDataAccess();
            }
        }
        public GlobalSettingsFacade(string ConStr)
        {
            _GlobalSettingDataAccess = new GlobalSettingDataAccess(ConStr);
        }
        CustomerDataAccess _CustomerDataAccess
        {
            get
            {
                return (CustomerDataAccess)_ClientContext[typeof(CustomerDataAccess)];
            }
        }
        public string GetExcelFormat(string resourceKey, Guid CompanyId)
        {
            string result = string.Empty;
            var globalsetting = new GlobalSetting();
            globalsetting = _GlobalSettingDataAccess.Get(resourceKey, CompanyId);
            if (globalsetting != null && !string.IsNullOrWhiteSpace(globalsetting.Value))
            {
                result = globalsetting.Value;
            }
            return result;
        }
        public List<GlobalSetting> GetSalesGlobalSettingsByCompanyIdAndTag(Guid CompanyId, string Tag)
        {
            return _GlobalSettingDataAccess.GetByQuery(string.Format("Tag = '{0}' AND CompanyId ='{1}'", Tag, CompanyId));
        }
        public List<GlobalSetting> GetAllGlobalSettingByCompanyId(Guid CompanyId)
        {
            return _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", CompanyId)).ToList();
        }

        public List<GlobalSetting> GetAllGlobalSettingsByCompanyId(Guid CompanyId)
        {
            return _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and IsActive =1 and Tag != 'RecurringBillingSetting'", CompanyId));
        }
        public List<GlobalSetting> GetAllGlobalSettingsForRecurringByCompanyId(Guid CompanyId)
        {
            return _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and Tag = 'RecurringBillingSetting' and InputType = 'checkbox'", CompanyId));
        }
        public List<GlobalSetting> GetCalendarGlobalSettingsByCompanyId(Guid CompanyId)
        {
            return _GlobalSettingDataAccess.GetByQuery(string.Format("Tag = 'CustomCalendarSettings' OR SearchKey IN('ScheduleCalendarDefaultView','ScheduleCalendarMinTimeRange','ScheduleCalendarMaxTimeRange', 'FirstDayOfWeek') AND CompanyId ='{0}'", CompanyId));
        }
        public string GetAvailabilityStartTime(Guid CompanyId)
        {
            GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, "EmployeeAvailabilityStartTime")).FirstOrDefault();
            return globalsetting == null ? "09:00" : globalsetting.Value;
        }
        public string GetAvailabilityEndTime(Guid CompanyId)
        {
            GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, "EmployeeAvailabilityEndTime")).FirstOrDefault();
            return globalsetting == null ? "17:00" : globalsetting.Value;
        }
        public string GetOnlyStrValueFromGlobalSettingByKey(string Key)
        {
            GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("SearchKey = '{0}'", Key)).FirstOrDefault();
            return (globalsetting == null ? "" : globalsetting.Value);
        }
        public List<GlobalSetting> GetAllJupiterSettingsByCompanyId(Guid CompanyId)
        {
            return _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and IsActive =1 and Tag = 'JupiterSetting'", CompanyId));
        }
        public List<GlobalSetting> GetAllIndividualGlobalSettingsByCompanyId(Guid CompanyId, string For)
        {
            return _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and IsActive =1 and Tag = '{1}'", CompanyId, For));
        }
        public GlobalSetting GetBINAPIKey(Guid companyId)
        {
            return _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = 'BINCodesAPIKey'", companyId)).FirstOrDefault();
        }

        public List<GlobalSetting> GetAllGlobalSettings()
        {
            return _GlobalSettingDataAccess.GetByQuery("IsActive =1 ");
        }
        public GlobalSetting GetGlobalSettingsDetailsById(int value)
        {
            return _GlobalSettingDataAccess.Get(value);
        }
        public bool UpdateGlobalSetting(GlobalSetting globalSetting)
        {
            return _GlobalSettingDataAccess.Update(globalSetting) > 0;
        }
        public GlobalSetting GetSalesTax(Guid CompanyId,Guid CustomerId)
        {
            GlobalSetting gl = new GlobalSetting();
            gl = _GlobalSettingDataAccess.GetByQuery(string.Format("SearchKey = 'Sales Tax' and CompanyId = '{0}' and IsActive = 'true'", CompanyId)).FirstOrDefault();
            if (CustomerId != new Guid())
            {
                Customer cus = _CustomerDataAccess.GetIndividualCustomerByCustomerId(CustomerId);
                if (cus != null)
                {
                    if (cus.TaxExemption == "Yes")
                    {
                        gl.Value = "0.00";
                    }
                }
            }
            return gl;
        }
        public GlobalSetting GetOutOfStateTax(Guid CompanyId)
        {
            return _GlobalSettingDataAccess.GetByQuery(string.Format("SearchKey = 'Out Of State' and CompanyId = '{0}' and IsActive = 'true'", CompanyId)).FirstOrDefault();
        }
        public GlobalSetting GetNonProfitTax(Guid CompanyId)
        {
           
            return _GlobalSettingDataAccess.GetByQuery(string.Format("SearchKey = 'Non Profit' and CompanyId = '{0}' and IsActive = 'true'", CompanyId)).FirstOrDefault();
        }
        public GlobalSetting GetInvoiceShippingSettingValueByCompanyId(Guid companyId)
        {
            return _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = 'InvoiceSettingsShipping'", companyId)).FirstOrDefault();
        }
        public GlobalSetting GetEstimateTaxSettingValueByCompanyId(Guid companyId)
        {
            return _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = 'EstimateTaxSetting'", companyId)).FirstOrDefault();
        }
        public GlobalSetting GetInvoiceDiscountSettingValueByCompanyId(Guid companyId)
        {
            return _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = 'InvoiceSettingsDiscount'", companyId)).FirstOrDefault();
        }
        public GlobalSetting GetEstimateServiceSettingValueByCompanyId(Guid companyId)
        {
            return _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = 'EstimateServiceSetting'", companyId)).FirstOrDefault();
        }
        public GlobalSetting GetLeadTrackingTabByCompanyId(Guid companyId)
        {
            return _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = 'LeadTrackingTab'", companyId)).FirstOrDefault();
        }

        public List<GlobalSetting> GetInvoiceSettingListByCompanyIdAndKey(string DataKey, Guid CompanyId)
        {
            return _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey in ({1})", CompanyId, DataKey));
        }
        public GlobalSetting GetInvoiceDepositSettingValueByCompanyId(Guid companyId)
        {
            return _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = 'InvoiceSettingsDeposit'", companyId)).FirstOrDefault();
        }
        public GlobalSetting GetInvoiceVendorPriceSettingValueByCompanyId(Guid companyId)
        {
            return _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = 'VendorPrice'", companyId)).FirstOrDefault();
        }
        public List<GlobalSetting> GetAllGlobalSettingsForEstimateByCompanyId(Guid companyId)
        {
            return _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' AND SearchKey in ('VendorPrice','InvoiceSettingsShipping','InvoiceSettingsDiscount','InvoiceSettingsDeposit', 'Sales Tax', 'Out Of State', 'Non Profit', 'EstimateMonitoringAmount', 'EstimateContractTerm','EstimateMonitoringDescription', 'EstimateOldButton')", companyId));
        }
        public GlobalSetting GetInvoiceSettingByCompanyIdAndKey(string DataKey, Guid CompanyId)
        {
            return _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, DataKey)).FirstOrDefault();
        }
        public string GetFacebookUrlByCompanyId(Guid CompanyId)
        {
            string result = string.Empty;
            var globalsetting = new GlobalSetting();
            string FaceBook = RMRCacheKey.FacebookUrl + CompanyId.ToString();
            if (System.Web.HttpRuntime.Cache[FaceBook] == null)
            {
                string DataKey = "FaceBook";
                globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, DataKey)).FirstOrDefault();
                result = globalsetting.Value;
                System.Web.HttpRuntime.Cache[FaceBook] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[FaceBook];
            }
            return result;
        }

        public string GetShippingSettingCompanyId(Guid CompanyId)
        {
            string result = "False";
            var globalsetting = new GlobalSetting();
            string InvoiceSettingsShipping = RMRCacheKey.InvoiceSettingsShipping + CompanyId.ToString();
            
            if (System.Web.HttpRuntime.Cache[InvoiceSettingsShipping] == null)
            {
                string DataKey = "InvoiceSettingsShipping";
                globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, DataKey)).FirstOrDefault();
                if (globalsetting != null)
                {
                    result = globalsetting.IsActive.Value.ToString(); 
                }
                
                System.Web.HttpRuntime.Cache[InvoiceSettingsShipping] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[InvoiceSettingsShipping];
            }
            return result;
        }

        public int GetMaxOrderingQuantityByCompanyId(Guid CompanyId)
        {
            int result = 0;
            var globalsetting = new GlobalSetting();
            string MaxOrderingQuantity = RMRCacheKey.MaxOrderingQuantity + CompanyId.ToString();

            if (System.Web.HttpRuntime.Cache[MaxOrderingQuantity] == null)
            {
                string DataKey = "MaxOrderingQuantity";
                globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, DataKey)).FirstOrDefault();
                if (globalsetting != null)
                {
                    int.TryParse(globalsetting.Value, out result);
                }

                System.Web.HttpRuntime.Cache[MaxOrderingQuantity] = result;
            }
            else
            {
                result = (int)System.Web.HttpRuntime.Cache[MaxOrderingQuantity];
            }
            return result;
        }

        public int GetMinimumPrepTimeByCompanyId(Guid CompanyId)
        {
            int result = 0;
            var globalsetting = new GlobalSetting();
            string MinimumPrepTime = RMRCacheKey.MinimumPrepTime + CompanyId.ToString();

            if (System.Web.HttpRuntime.Cache[MinimumPrepTime] == null)
            {
                string DataKey = "MinimumPrepTime";
                globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, DataKey)).FirstOrDefault();
                if (globalsetting != null)
                {
                    int.TryParse(globalsetting.Value, out result);
                }

                System.Web.HttpRuntime.Cache[MinimumPrepTime] = result;
            }
            else
            {
                result = (int)System.Web.HttpRuntime.Cache[MinimumPrepTime];
            }
            return result;
        }

        public string GetExpireTimeByCompanyId(Guid CompanyId)
        {
            string result = "0";
            var globalsetting = new GlobalSetting();
            string ExpireTime = RMRCacheKey.ExpireTime + CompanyId.ToString();

            if (System.Web.HttpRuntime.Cache[ExpireTime] == null)
            {
                string DataKey = "ExpireTime";
                globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, DataKey)).FirstOrDefault();
                if (globalsetting != null)
                {
                    result = globalsetting.Value;
                }

                System.Web.HttpRuntime.Cache[ExpireTime] = result;
            }
            else
            {
                result = System.Web.HttpRuntime.Cache[ExpireTime].ToString();
            }
            return result;
        }

        public string GetStartDayOfWeek(Guid CompanyId)
        {
            string result = "Sunday";
            var globalsetting = new GlobalSetting();
            string FirstDayOfWeek = RMRCacheKey.StartDayOfWeek + CompanyId.ToString();

            if (System.Web.HttpRuntime.Cache[FirstDayOfWeek] == null)
            {
                string DataKey = "FirstDayOfWeek";
                globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, DataKey)).FirstOrDefault();
                if (globalsetting != null)
                {
                    result = globalsetting.Value;
                }

                System.Web.HttpRuntime.Cache[FirstDayOfWeek] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[FirstDayOfWeek];
            }
            return result;
        }
        public string GetStartDayOfWeekForCalendar(Guid CompanyId)
        {
            string result = "Monday";
            var globalsetting = new GlobalSetting();
                string DataKey = "FirstDayOfWeek";
                globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, DataKey)).FirstOrDefault();
                if (globalsetting != null)
                {
                    result = globalsetting.Value;
                }
            return result;
        }
        public string GetCurrentCurrencyByCompanyId(Guid CompanyId)
        {
            string result = string.Empty;
            var globalsetting = new GlobalSetting();
            string CurrentCurrency = RMRCacheKey.CurrentCurrency + CompanyId.ToString();
            
            if (System.Web.HttpRuntime.Cache[CurrentCurrency] == null)
            {
                string DataKey = "CurrentCurrency";
                globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, DataKey)).FirstOrDefault();
                if (globalsetting != null)
                {
                    result = globalsetting.Value.ToString();
                }
                else
                {
                    result = "$";
                }
                
                System.Web.HttpRuntime.Cache[CurrentCurrency] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[CurrentCurrency];
            }
            return result;
        }

        public GlobalSetting GetGlobalsettingBySearchKeyAndCompanyId(string Key, object companyId)
        {
            return _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", companyId,Key)).FirstOrDefault();
        }

        public string GetLeadCreditReportCheck(Guid CompanyId)
        {
            string result = string.Empty;
            var globalsetting = new GlobalSetting();
            string LeadCreditReportCheck = RMRCacheKey.LeadCreditReportCheck + CompanyId.ToString();
            
            if (System.Web.HttpRuntime.Cache[LeadCreditReportCheck] == null)
            {
                string DataKey = "LeadCreditReportCheck";
                globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, DataKey)).FirstOrDefault();
                if (globalsetting != null)
                {
                    result = globalsetting.Value.ToString();
                }
                System.Web.HttpRuntime.Cache[LeadCreditReportCheck] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[LeadCreditReportCheck];
            }
            return result;
        }

        public string GetNotificationManagerSetting(Guid CompanyId)
        {
            string result = string.Empty;
            var globalsetting = new GlobalSetting();
            string NotificationManager = RMRCacheKey.NotificationManager + CompanyId.ToString();

            if (System.Web.HttpRuntime.Cache[NotificationManager] == null)
            {
                string DataKey = "NotificationManager";
                globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, DataKey)).FirstOrDefault();
                if (globalsetting != null)
                {
                    result = globalsetting.Value.ToString();
                }
                System.Web.HttpRuntime.Cache[NotificationManager] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[NotificationManager];
            }
            return result;
        }

        public string GetCustomFormGeneration(Guid CompanyId)
        {
            string result = string.Empty;
            var globalsetting = new GlobalSetting();
            string CustomFormGeneration = RMRCacheKey.CustomFormGeneration + CompanyId.ToString();
            
            if (System.Web.HttpRuntime.Cache[CustomFormGeneration] == null)
            {
                string DataKey = "CustomFormGeneration";
                globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, DataKey)).FirstOrDefault();
                if (globalsetting != null)
                {
                    result = globalsetting.Value.ToString();
                }
                System.Web.HttpRuntime.Cache[CustomFormGeneration] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[CustomFormGeneration];
            }
            return result;
        }

        public string GetScheduleCalendarDefaultView(Guid CompanyId)
        {
            string result = string.Empty;
            var globalsetting = new GlobalSetting();
            string ScheduleCalendarDefaultView = RMRCacheKey.ScheduleCalendarDefaultView + CompanyId.ToString();
            
            if (System.Web.HttpRuntime.Cache[ScheduleCalendarDefaultView] == null)
            {
                string DataKey = "ScheduleCalendarDefaultView";
                globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, DataKey)).FirstOrDefault();
                if (globalsetting != null)
                {
                    result = globalsetting.Value.ToString();
                }
                System.Web.HttpRuntime.Cache[ScheduleCalendarDefaultView] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[ScheduleCalendarDefaultView];
            }
            return result;
        }

        public string GetGeoLocation(Guid CompanyId)
        {
            string result = string.Empty;
            var globalsetting = new GlobalSetting();
            string GeoLocation = RMRCacheKey.GeoLocation + CompanyId.ToString();
            
            if (System.Web.HttpRuntime.Cache[GeoLocation] == null)
            {
                string DataKey = "GeoLocation";
                globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, DataKey)).FirstOrDefault();
                if (globalsetting != null)
                {
                    result = globalsetting.Value.ToString();
                }
                System.Web.HttpRuntime.Cache[GeoLocation] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[GeoLocation];
            }
            return result;
        }

        public string GetAgreementDocumentHeightByCompanyId(Guid CompanyId)
        {
            string result = string.Empty;
            var globalsetting = new GlobalSetting();
            string AgreementDocumentHeight = RMRCacheKey.AgreementDocumentHeightLoad + CompanyId.ToString();
            if (System.Web.HttpRuntime.Cache[AgreementDocumentHeight] == null)
            {
                string DataKey = "AgreementDocumentHeight";
                globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, DataKey)).FirstOrDefault();
                if(globalsetting != null)
                {
                    result = globalsetting.Value;
                    System.Web.HttpRuntime.Cache[AgreementDocumentHeight] = result;
                }
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[AgreementDocumentHeight];
            }
            return result;
        }

        public string GetYoutubeUrlByCompanyId(Guid CompanyId)
        {
            string result = string.Empty;
            string Youtube = RMRCacheKey.YoutubeUrl + CompanyId.ToString();
            var globalsetting = new GlobalSetting();
            if (System.Web.HttpRuntime.Cache[Youtube] == null)
            {
                string DataKey = "Youtube";
                globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, DataKey)).FirstOrDefault();
                result = globalsetting.Value;
                System.Web.HttpRuntime.Cache[Youtube] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[Youtube];
            }
            return result;
        }

        public string GetShoppingCartHeadingByCompanyId(Guid CompanyId)
        {
            string result = string.Empty;
            string ShoppingCartHeading = RMRCacheKey.ShoppingCartHeading + CompanyId.ToString();
            var globalsetting = new GlobalSetting();
            if (System.Web.HttpRuntime.Cache[ShoppingCartHeading] == null)
            {
                string DataKey = "ShoppingCartHeading";
                globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, DataKey)).FirstOrDefault();
                result = globalsetting.Value;
                System.Web.HttpRuntime.Cache[ShoppingCartHeading] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[ShoppingCartHeading];
            }
            return result;
        }

        public double GetDefaultProfitRate(Guid CompanyId)
        {
            string InvMsg = RMRCacheKey.EstimatorProfitRate + CompanyId.ToString();

            string result = string.Empty;
            if (System.Web.HttpRuntime.Cache[InvMsg] == null)
            {

                string SearchKey = "EstimatorProfitRate";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();

                result = globalsetting == null ? "20" : globalsetting.Value;
                System.Web.HttpRuntime.Cache[InvMsg] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[InvMsg];
            }
            return Convert.ToDouble(result);
        }
        public double GetDefaultOverHeadRate(Guid CompanyId)
        {
            string InvMsg = RMRCacheKey.EstimatorOverheadRate + CompanyId.ToString();

            string result = string.Empty;
            if (System.Web.HttpRuntime.Cache[InvMsg] == null)
            {

                string SearchKey = "EstimatorOverheadRate";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();

                result = globalsetting == null ? "60" : globalsetting.Value;
                System.Web.HttpRuntime.Cache[InvMsg] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[InvMsg];
            }
            return Convert.ToDouble(result);
        }

        public string GetTeamNameSignatureByCompanyId(Guid CompanyId)
        {
            string result = string.Empty;
            string TeamNameSignature = RMRCacheKey.TeamNameSignatureLoad + CompanyId.ToString();
            var globalsetting = new GlobalSetting();
            if (System.Web.HttpRuntime.Cache[TeamNameSignature] == null)
            {
                string DataKey = "TeamNameSignature";
                globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, DataKey)).FirstOrDefault();
                result = globalsetting.Value;
                System.Web.HttpRuntime.Cache[TeamNameSignature] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[TeamNameSignature];
            }
            return result;
        }
        public string GetEmailLogoByCompanyId(Guid CompanyId)
        {
            string result = string.Empty;
            string EmailLogo =  RMRCacheKey.EmailLogoUrl + CompanyId.ToString();
            var globalsetting = new GlobalSetting();
            if (System.Web.HttpRuntime.Cache[EmailLogo] == null)
            {
                string DataKey = "EmailLogo";
                globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, DataKey)).FirstOrDefault();
                result = globalsetting.Value;
                System.Web.HttpRuntime.Cache[EmailLogo] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[EmailLogo];
            }
            return result;
        }
        //public string GetCompanyColoredLogoByCompanyId(Guid CompanyId)
        //{
        //    string result = string.Empty;
        //    string Logo = RMRCacheKey.CompanyLogoColored + CompanyId.ToString();
        //    var globalsetting = new GlobalSetting();
        //    if (System.Web.HttpRuntime.Cache[Logo] == null)
        //    {
        //        string DataKey = "CompanyLogoColored";
        //        globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, DataKey)).FirstOrDefault();
        //        result = globalsetting.Value;
        //        System.Web.HttpRuntime.Cache[Logo] = result;
        //    }
        //    else
        //    {
        //        result = (string)System.Web.HttpRuntime.Cache[Logo];
        //    }
        //    return result;
            
        //}

        public List<GlobalSetting> GetQATechCallGlobSettingsBycompanyId(Guid CompanyId)
        {
            DataTable dt = _GlobalSettingDataAccess.GetQATechCallGlobSettingsBycompanyId(CompanyId);
            List<GlobalSetting> QATechCallSettingList = new List<GlobalSetting>();
            QATechCallSettingList = (from DataRow dr in dt.Rows
                        select new GlobalSetting()
                        {
                            Value = dr["Value"].ToString()
                        }).ToList();
            return QATechCallSettingList;
        }
        public string GetAuthAPILoginIdByCompanyId(Guid CompanyId,bool IsACH)
        {
            string result = string.Empty;
            string SearchKey = "AuthAPILoginId";
            string ApiLogin = RMRCacheKey.AuthAPILoginId + CompanyId.ToString();
            if (IsACH)
            {
                ApiLogin = RMRCacheKey.AuthAPILoginIdACH + CompanyId.ToString();
                SearchKey = "AuthAPILoginIdACH";
            }
            if (System.Web.HttpRuntime.Cache[ApiLogin] == null)
            {
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();
                if (globalsetting != null)
                {
                    result = globalsetting.Value;
                    System.Web.HttpRuntime.Cache[ApiLogin] = result;
                }
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[ApiLogin];
            }
            return result;
        }
        public string GetForteAPILoginIdByCompanyId(Guid CompanyId, bool IsACH)
        {
            string result = string.Empty;
            string SearchKey = "ForteAPILoginId";
            string ApiLogin = RMRCacheKey.ForteAPILoginId + CompanyId.ToString();
         
            if (System.Web.HttpRuntime.Cache[ApiLogin] == null)
            {
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();
                if (globalsetting != null)
                {
                    result = globalsetting.Value;
                    System.Web.HttpRuntime.Cache[ApiLogin] = result;
                }
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[ApiLogin];
            }
            return result;
        }
        public string GetAuthTransactionKeyByCompanyId(Guid CompanyId,bool IsAch)
        {
            string AuthKey = RMRCacheKey.AuthTransactionKey + CompanyId.ToString();
            string SearchKey = "AuthTransactionKey";
            string result = string.Empty;
            if (IsAch)
            {
                SearchKey = "AuthTransactionKeyACH";
                AuthKey = RMRCacheKey.AuthTransactionKeyACH + CompanyId.ToString();
            }

            if (System.Web.HttpRuntime.Cache[AuthKey] == null)
            {
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();
                if (globalsetting != null)
                {
                    result = globalsetting.Value;
                    System.Web.HttpRuntime.Cache[AuthKey] = result;
                }
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[AuthKey];
            }
            return result; 
        }
        public string GetForteTransactionKeyByCompanyId(Guid CompanyId, bool IsAch)
        {
            string ForteKey = RMRCacheKey.ForteTransactionKey + CompanyId.ToString();
            string SearchKey = "ForteTransactionKey";
            string result = string.Empty;

            
            if (System.Web.HttpRuntime.Cache[ForteKey] == null)
            {
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();
                if (globalsetting != null)
                {
                    result = globalsetting.Value;
                    System.Web.HttpRuntime.Cache[ForteKey] = result;
                }
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[ForteKey];
            }
            return result;
        }

        public string GetEcheckTypeDefault(Guid CompanyId)
        {
            string ForteKey = RMRCacheKey.EcheckTypeDefault + CompanyId.ToString();
            string SearchKey = "EcheckTypeDefault";
            string result = string.Empty;


            if (System.Web.HttpRuntime.Cache[ForteKey] == null)
            {
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();
                if (globalsetting != null)
                {
                    result = globalsetting.Value;
                    System.Web.HttpRuntime.Cache[ForteKey] = result;
                }
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[ForteKey];
            }
            return result;
        }

        public string GetForteOrganizationIdByCompanyId(Guid CompanyId, bool IsAch)
        {
            string ForteKey = RMRCacheKey.ForteOrganizationId + CompanyId.ToString();
            string SearchKey = "ForteOrganizationId";
            string result = string.Empty;


            if (System.Web.HttpRuntime.Cache[ForteKey] == null)
            {
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();
                if (globalsetting != null)
                {
                    result = globalsetting.Value;
                    System.Web.HttpRuntime.Cache[ForteKey] = result;
                }
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[ForteKey];
            }
            return result;
        }

        public string GetForteLocationIdByCompanyId(Guid CompanyId, bool IsAch)
        {
            string ForteKey = RMRCacheKey.ForteLocationId + CompanyId.ToString();
            string SearchKey = "ForteLocationId";
            string result = string.Empty;


            if (System.Web.HttpRuntime.Cache[ForteKey] == null)
            {
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();
                if (globalsetting != null)
                {
                    result = globalsetting.Value;
                    System.Web.HttpRuntime.Cache[ForteKey] = result;
                }
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[ForteKey];
            }
            return result;
        }

        public string GetForteAuthAccountIdByCompanyId(Guid CompanyId, bool IsAch)
        {
            string ForteKey = RMRCacheKey.ForteAuthAccountId + CompanyId.ToString();
            string SearchKey = "ForteAuthAccountId";
            string result = string.Empty;


            if (System.Web.HttpRuntime.Cache[ForteKey] == null)
            {
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();
                if (globalsetting != null)
                {
                    result = globalsetting.Value;
                    System.Web.HttpRuntime.Cache[ForteKey] = result;
                }
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[ForteKey];
            }
            return result;
        }

        public string GetCompanyLogoByCompanyId(Guid CompanyId)
        {
            string CompanyLogo = "__CompanyLogo" + CompanyId.ToString();

            string result = string.Empty;
            if (System.Web.HttpRuntime.Cache[CompanyLogo] == null)
            {

                string SearchKey = "CompanyLogo";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();
                result = globalsetting.Value;
                System.Web.HttpRuntime.Cache[CompanyLogo] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[CompanyLogo];
            }
            return result;
        }

        public string GetInvoiceMessageByCompanyId(Guid CompanyId)
        {
            string InvMsg = RMRCacheKey.InvoiceMessageGlobal + CompanyId.ToString();

            string result = string.Empty;
            if (System.Web.HttpRuntime.Cache[InvMsg] == null)
            {

                string SearchKey = "InvoiceMessage";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();

                result = globalsetting==null? "":globalsetting.Value;
                System.Web.HttpRuntime.Cache[InvMsg] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[InvMsg];
            }
            return result;
        }
        public string GetEstimateByCompanyId(Guid CompanyId)
        {
            string EstMsg = RMRCacheKey.EstimateMessageGlobal + CompanyId.ToString();

            string result = string.Empty;
            if (System.Web.HttpRuntime.Cache[EstMsg] == null)
            {

                string SearchKey = "EstimateMessage";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();
                result = globalsetting == null ? "" : globalsetting.Value;
                System.Web.HttpRuntime.Cache[EstMsg] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[EstMsg];
            }
            return result;
        }

        public string GetAuthSecretKeyByCompanyId(Guid CompanyId)
        {
            return _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = 'AuthSecretKey'", CompanyId)).FirstOrDefault().Value;
        }
        public GlobalSetting GetAreaZipcodeMenu(Guid CompanyId)
        {
            return _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = 'ServiceAreaZipcode'", CompanyId)).FirstOrDefault();
        }
        public GlobalSetting GetStreetType(Guid CompanyId, string searchKey)
        {
            return _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, searchKey)).FirstOrDefault();
        }
        public GlobalSetting GetCrossStreet(Guid CompanyId, string searchKey)
        {
            return _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, searchKey)).FirstOrDefault();
        }
        public int GetEquipmentSearchMaxLoad(Guid CompanyId)
        {
            string InvMsg = RMRCacheKey.EquipmentSearchMaxLoad + CompanyId.ToString();

            string result = string.Empty;
            if (System.Web.HttpRuntime.Cache[InvMsg] == null)
            {

                string SearchKey = "EquipmentSearchMaxLoad";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();

                result = globalsetting == null ? "40" : globalsetting.Value;
                System.Web.HttpRuntime.Cache[InvMsg] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[InvMsg];
            }
            return Convert.ToInt32(result);
        }
        public int GetEmergencyContactRequired(Guid CompanyId)
        {
            string InvMsg = RMRCacheKey.EmergencyContactRequired + CompanyId.ToString();

            string result = string.Empty;
            if (System.Web.HttpRuntime.Cache[InvMsg] == null)
            {

                string SearchKey = "EmergencyContactRequired";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();

                result = globalsetting == null ? "2" : globalsetting.Value;
                System.Web.HttpRuntime.Cache[InvMsg] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[InvMsg];
            }
            return Convert.ToInt32(result);
        }
        public bool GetSmartSetupOneServiceRequired(Guid CompanyId)
        {
            string InvSmartMsg = RMRCacheKey.SmartSetupOneServiceRequired + CompanyId.ToString();

            bool result = false;
            if (System.Web.HttpRuntime.Cache[InvSmartMsg] == null)
            {

                string SearchKey = "SmartSetupOneServiceRequired";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();

                result = globalsetting == null ? false : Convert.ToBoolean(globalsetting.Value);
                System.Web.HttpRuntime.Cache[InvSmartMsg] = result;
            }
            else
            {
                result = (bool)System.Web.HttpRuntime.Cache[InvSmartMsg];
            }
            return result;
        }
        public int GetMapZoomLevel(Guid CompanyId)
        {
            string InvMsg = RMRCacheKey.MapZoomLevel + CompanyId.ToString();

            string result = string.Empty;
            if (System.Web.HttpRuntime.Cache[InvMsg] == null)
            {

                string SearchKey = "MapZoomLevel";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();

                result = globalsetting == null ? "5" : globalsetting.Value;
                System.Web.HttpRuntime.Cache[InvMsg] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[InvMsg];
            }
            return Convert.ToInt32(result);
        }
        public string GetPayrollFilterWeek(Guid CompanyId)
        {
            string InvMsg = RMRCacheKey.PayrollFilterWeek + CompanyId.ToString();

            string result = string.Empty;
            if (System.Web.HttpRuntime.Cache[InvMsg] == null)
            {

                string SearchKey = "PayrollFilterWeek";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();

                result = globalsetting == null ? "ThisWeek" : globalsetting.Value;
                System.Web.HttpRuntime.Cache[InvMsg] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[InvMsg];
            }
            return result;
        }
        public int GetCustomerSearchMaxLoad(Guid CompanyId)
        {
            string InvMsg = RMRCacheKey.CustomerSearchMaxLoad + CompanyId.ToString();

            string result = string.Empty;
            if (System.Web.HttpRuntime.Cache[InvMsg] == null)
            {

                string SearchKey = "CustomerSearchMaxLoad";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();

                result = globalsetting == null ? "20" : globalsetting.Value;
                System.Web.HttpRuntime.Cache[InvMsg] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[InvMsg];
            }
            return Convert.ToInt32(result);
        }
        public int GetPayrollMaxLoad(Guid CompanyId)
        {
            string GetPayrollMaxLoad = RMRCacheKey.PayrollMaxLoad + CompanyId.ToString();
            int Size = 20;
            string result = string.Empty;
            if (System.Web.HttpRuntime.Cache[GetPayrollMaxLoad] == null)
            {
                string SearchKey = "PayrollMaxLoad";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();

                result = globalsetting == null ? "20" : globalsetting.Value;
                System.Web.HttpRuntime.Cache[GetPayrollMaxLoad] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[GetPayrollMaxLoad];
            }
            if(int.TryParse(result, out Size) && Size > 0)
            {
                return Size;
            } 
            return 20;

        }
        public int GetInventoryPagingLimit(Guid CompanyId)
        {
            string InventoryPaging = RMRCacheKey.InventoryPagingLimit + CompanyId.ToString();

            string result = string.Empty;
            if (System.Web.HttpRuntime.Cache[InventoryPaging] == null)
            {

                string SearchKey = "InventoryPagingLimit";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();

                result = globalsetting == null ? "20" : globalsetting.Value;
                System.Web.HttpRuntime.Cache[InventoryPaging] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[InventoryPaging];
            }
            return Convert.ToInt32(result);
        }
        public int GetCustomerInvoicePageLimit(Guid CompanyId)
        {
            string CustomerInvoicePageLimit = RMRCacheKey.CustomerInvoicePageLimit + CompanyId.ToString();

            string result = string.Empty;
            if (System.Web.HttpRuntime.Cache[CustomerInvoicePageLimit] == null)
            {

                string SearchKey = "CustomerInvoicePageLimit";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();

                result = globalsetting == null ? "20" : globalsetting.Value;
                System.Web.HttpRuntime.Cache[CustomerInvoicePageLimit] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[CustomerInvoicePageLimit];
            }
            return Convert.ToInt32(result);
        }

        public int GetLeadPageLength(Guid CompanyId)
        {
            string CustomerInvoicePageLimit = RMRCacheKey.LeadPageLength + CompanyId.ToString();

            string result = string.Empty;
            if (System.Web.HttpRuntime.Cache[CustomerInvoicePageLimit] == null)
            {

                string SearchKey = "LeadPageLength";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();

                result = globalsetting == null ? "20" : globalsetting.Value;
                System.Web.HttpRuntime.Cache[CustomerInvoicePageLimit] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[CustomerInvoicePageLimit];
            }
            return Convert.ToInt32(result);
        }

        public int GetCustomerFundingPageLimit(Guid CompanyId)
        {
            string CustomerFundingPageLimit = RMRCacheKey.CustomerFundingPageLimit + CompanyId.ToString();

            string result = string.Empty;
            if (System.Web.HttpRuntime.Cache[CustomerFundingPageLimit] == null)
            {

                string SearchKey = "CustomerFundingPageLimit";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();

                result = globalsetting == null ? "20" : globalsetting.Value;
                System.Web.HttpRuntime.Cache[CustomerFundingPageLimit] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[CustomerFundingPageLimit];
            }
            return Convert.ToInt32(result);
        }

        public int GetTicketPageLimit(Guid CompanyId)
        {
            string CustomerTicketPageLimit = RMRCacheKey.CustomerTicketPageLimit + CompanyId.ToString();

            string result = string.Empty;
            if (System.Web.HttpRuntime.Cache[CustomerTicketPageLimit] == null)
            {

                string SearchKey = "CustomerTicketPageLimit";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();

                result = globalsetting == null ? "20" : globalsetting.Value;
                System.Web.HttpRuntime.Cache[CustomerTicketPageLimit] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[CustomerTicketPageLimit];
            }
            return Convert.ToInt32(result);
        }
        public int GetPOPageLimit(Guid CompanyId)
        {
            string POPageLimit = RMRCacheKey.POPageLimit + CompanyId.ToString();

            string result = string.Empty;
            if (System.Web.HttpRuntime.Cache[POPageLimit] == null)
            {

                string SearchKey = "PurchaseOrderPageLimit";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();

                result = globalsetting == null ? "20" : globalsetting.Value;
                System.Web.HttpRuntime.Cache[POPageLimit] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[POPageLimit];
            }
            return Convert.ToInt32(result);
        }


        public int GetScheduleCalendarResourceLimit(Guid CompanyId)
        {
            string ScheduleCalendarResourceLimit = RMRCacheKey.ScheduleCalendarResourceLimit + CompanyId.ToString();

            string result = string.Empty;
            if (System.Web.HttpRuntime.Cache[ScheduleCalendarResourceLimit] == null)
            {

                string SearchKey = "ScheduleCalendarResourceLimit";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();

                result = globalsetting == null ? "20" : globalsetting.Value;
                System.Web.HttpRuntime.Cache[ScheduleCalendarResourceLimit] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[ScheduleCalendarResourceLimit];
            }
            return Convert.ToInt32(result);
        }

        public string GetGoogleMapAPIKeyByCompanyId(Guid CompanyId)
        {
            string GoogleMapAPIKey = RMRCacheKey.GoogleMapAPIKey + CompanyId.ToString();

            string result = string.Empty;
            if (System.Web.HttpRuntime.Cache[GoogleMapAPIKey] == null)
            {
                string SearchKey = "GoogleMapAPIKey";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();
                result = globalsetting == null ? "" : globalsetting.Value;
                System.Web.HttpRuntime.Cache[GoogleMapAPIKey] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[GoogleMapAPIKey];
            }
            return result;
        }

        public int GetCustomerSystemNoPagingLimit(Guid CompanyId)
        {
            string CustomerSystemNoPagingLimit = RMRCacheKey.CustomerSystemNoPagingLimit + CompanyId.ToString();

            string result = string.Empty;
            
            if (System.Web.HttpRuntime.Cache[CustomerSystemNoPagingLimit] == null)
            {

                string SearchKey = "CustomerSystemNoPagingLimit";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();

                result = globalsetting == null ? "20" : globalsetting.Value;
                System.Web.HttpRuntime.Cache[CustomerSystemNoPagingLimit] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[CustomerSystemNoPagingLimit];
            }
            return Convert.ToInt32(result);
        }
        public int GetVendorSearchMaxLoad(Guid CompanyId)
        {
            string VendorMsg = RMRCacheKey.VendorSearchMaxLoad + CompanyId.ToString();

            string result = string.Empty;
            if (System.Web.HttpRuntime.Cache[VendorMsg] == null)
            {

                string SearchKey = "VendorSearchMaxLoad";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();

                result = globalsetting == null ? "20" : globalsetting.Value;
                System.Web.HttpRuntime.Cache[VendorMsg] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[VendorMsg];
            }
            return Convert.ToInt32(result);
        }
        public int GetMenuItemSearchMaxLoad(Guid CompanyId)
        {
            string ItemMsg = RMRCacheKey.MenuItemSearchMaxLoad + CompanyId.ToString();

            string result = string.Empty;
            if (System.Web.HttpRuntime.Cache[ItemMsg] == null)
            {

                string SearchKey = "MenuItemSearchMaxLoad";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();

                result = globalsetting == null ? "50" : globalsetting.Value;
                System.Web.HttpRuntime.Cache[ItemMsg] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[ItemMsg];
            }
            return Convert.ToInt32(result);
        }
        public int GetLeadCityStateSearchMaxLoad(Guid CompanyId)
        {
            string LeadCityStateSearchMaxLoad = RMRCacheKey.LeadCityStateSearchMaxLoad + CompanyId.ToString();

            string result = string.Empty;
            if (System.Web.HttpRuntime.Cache[LeadCityStateSearchMaxLoad] == null)
            {

                string SearchKey = "LeadCityStateSearchMaxLoad";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();

                result = globalsetting == null ? "20" : globalsetting.Value;
                System.Web.HttpRuntime.Cache[LeadCityStateSearchMaxLoad] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[LeadCityStateSearchMaxLoad];
            }
            return Convert.ToInt32(result);
        }

        public int GetOrderListPageLimit(Guid CompanyId)
        {
            string OrderListPageLimit = RMRCacheKey.OrderListPageLimit + CompanyId.ToString();

            string result = string.Empty;
            if (System.Web.HttpRuntime.Cache[OrderListPageLimit] == null)
            {

                string SearchKey = "OrderListPageLimit";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();

                result = globalsetting == null ? "20" : globalsetting.Value;
                System.Web.HttpRuntime.Cache[OrderListPageLimit] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[OrderListPageLimit];
            }
            return Convert.ToInt32(result);
        }

        public string GetCompanyAddressFormat(Guid CompanyId)
        {
            string CompanyAddressPdfFormat = RMRCacheKey.CompanyAddressPdfFormat + CompanyId.ToString();

            string result = string.Empty;
            if (System.Web.HttpRuntime.Cache[CompanyAddressPdfFormat] == null)
            {
                string SearchKey = "CompanyAddressPdfFormat";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();
                result = globalsetting == null ? "" : globalsetting.Value;
                System.Web.HttpRuntime.Cache[CompanyAddressPdfFormat] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[CompanyAddressPdfFormat];
            }
            return result;
        }

        public string GetEmployeeAddressFormat(Guid CompanyId)
        {
            string EmployeeAddressPdfFormat = RMRCacheKey.EmployeeAddressPdfFormat + CompanyId.ToString();

            string result = string.Empty;
            if (System.Web.HttpRuntime.Cache[EmployeeAddressPdfFormat] == null)
            {
                string SearchKey = "EmployeeAddressPdfFormat";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();
                result = globalsetting == null ? "" : globalsetting.Value;
                System.Web.HttpRuntime.Cache[EmployeeAddressPdfFormat] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[EmployeeAddressPdfFormat];
            }
            return result;
        }

        public string GetSupplierAddressFormat(Guid CompanyId)
        {
            string SupplierAddressPdfFormat = RMRCacheKey.SupplierAddressPdfFormat + CompanyId.ToString();

            string result = string.Empty;
            if (System.Web.HttpRuntime.Cache[SupplierAddressPdfFormat] == null)
            {
                string SearchKey = "SupplierAddressPdfFormat";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();
                result = globalsetting == null ? "" : globalsetting.Value;
                System.Web.HttpRuntime.Cache[SupplierAddressPdfFormat] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[SupplierAddressPdfFormat];
            }
            return result;
        }

        public string GetScheduleCalendarMinTimeRange(Guid CompanyId)
        {
            string ScheduleCalendarMinTimeRange = RMRCacheKey.ScheduleCalendarMinTimeRange + CompanyId.ToString();

            string result = string.Empty;
            if (System.Web.HttpRuntime.Cache[ScheduleCalendarMinTimeRange] == null)
            {
                string SearchKey = "ScheduleCalendarMinTimeRange";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();
                result = globalsetting == null ? "" : globalsetting.Value;
                System.Web.HttpRuntime.Cache[ScheduleCalendarMinTimeRange] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[ScheduleCalendarMinTimeRange];
            }
            return result;
        }

        public string GetReminderMinTime(Guid CompanyId)
        {
            string ReminderMinTime = RMRCacheKey.ReminderMinTime + CompanyId.ToString();

            string result = string.Empty;
            if (System.Web.HttpRuntime.Cache[ReminderMinTime] == null)
            {
                string SearchKey = "ReminderMinTime";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();
                result = globalsetting == null ? "" : globalsetting.Value;
                System.Web.HttpRuntime.Cache[ReminderMinTime] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[ReminderMinTime];
            }
            return result;
        }
        public string GetMenuTimeAvailableStartTime(Guid CompanyId)
        {
            string MenuTimeAvailableStartTime = RMRCacheKey.MenuTimeAvailableStartTime + CompanyId.ToString();

            string result = string.Empty;
            if (System.Web.HttpRuntime.Cache[MenuTimeAvailableStartTime] == null)
            {
                string SearchKey = "MenuTimeAvailableStartTime";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();
                result = globalsetting == null ? "" : globalsetting.Value;
                System.Web.HttpRuntime.Cache[MenuTimeAvailableStartTime] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[MenuTimeAvailableStartTime];
            }
            return result;
        }
        public string GetPaymentOptionChoosing(Guid CompanyId)
        {
            string PaymentOptionChoosing = RMRCacheKey.PaymentOptionChoosing + CompanyId.ToString();

            string result = string.Empty;
            if (System.Web.HttpRuntime.Cache[PaymentOptionChoosing] == null)
            {
                string SearchKey = "PaymentOptionChoosing";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();
                result = globalsetting == null ? "" : globalsetting.Value;
                System.Web.HttpRuntime.Cache[PaymentOptionChoosing] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[PaymentOptionChoosing];
            }
            return result;
        }

        public string GetiEateryOrderType(Guid CompanyId)
        {
            string iEateryOrderType = RMRCacheKey.iEateryOrderType + CompanyId.ToString();

            string result = string.Empty;
            if (System.Web.HttpRuntime.Cache[iEateryOrderType] == null)
            {
                string SearchKey = "iEateryOrderType";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();
                result = globalsetting == null ? "" : globalsetting.Value;
                System.Web.HttpRuntime.Cache[iEateryOrderType] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[iEateryOrderType];
            }
            return result;
        }

        public string GetMenuTimeAvailableEndTime(Guid CompanyId)
        {
            string MenuTimeAvailableEndTime = RMRCacheKey.MenuTimeAvailableEndTime + CompanyId.ToString();

            string result = string.Empty;
            if (System.Web.HttpRuntime.Cache[MenuTimeAvailableEndTime] == null)
            {
                string SearchKey = "MenuTimeAvailableEndTime";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();
                result = globalsetting == null ? "" : globalsetting.Value;
                System.Web.HttpRuntime.Cache[MenuTimeAvailableEndTime] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[MenuTimeAvailableEndTime];
            }
            return result;
        }
        public string GetScheduleCalendarMaxTimeRange(Guid CompanyId)
        {
            string ScheduleCalendarMaxTimeRange = RMRCacheKey.ScheduleCalendarMaxTimeRange + CompanyId.ToString();

            string result = string.Empty;
            if (System.Web.HttpRuntime.Cache[ScheduleCalendarMaxTimeRange] == null)
            {
                string SearchKey = "ScheduleCalendarMaxTimeRange";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();
                result = globalsetting == null ? "" : globalsetting.Value;
                System.Web.HttpRuntime.Cache[ScheduleCalendarMaxTimeRange] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[ScheduleCalendarMaxTimeRange];
            }
            return result;
        }

        public string GetReminderMaxTime(Guid CompanyId)
        {
            string ReminderMaxTime = RMRCacheKey.ReminderMaxTime + CompanyId.ToString();

            string result = string.Empty;
            if (System.Web.HttpRuntime.Cache[ReminderMaxTime] == null)
            {
                string SearchKey = "ReminderMaxTime";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();
                result = globalsetting == null ? "" : globalsetting.Value;
                System.Web.HttpRuntime.Cache[ReminderMaxTime] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[ReminderMaxTime];
            }
            return result;
        }

        public string GetCustomerAddressFormat(Guid CompanyId)
        {
            string CustomerAddressPdfFormat = RMRCacheKey.CustomerAddressPdfFormat + CompanyId.ToString();

            string result = string.Empty;
            
            if (System.Web.HttpRuntime.Cache[CustomerAddressPdfFormat] == null)
            {
                string SearchKey = "CustomerAddressPdfFormat";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();
                result = globalsetting == null ? "" : globalsetting.Value;
                System.Web.HttpRuntime.Cache[CustomerAddressPdfFormat] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[CustomerAddressPdfFormat];
            }
            return result;
        }

        public int GetTicketDefaultTimeDuration(Guid CompanyId)
        {
            string CustomerAddressPdfFormat = RMRCacheKey.TicketDefaultTimeDuration + CompanyId.ToString();

            int result = 2;

            if (System.Web.HttpRuntime.Cache[CustomerAddressPdfFormat] == null)
            {
                string SearchKey = "TicketDefaultTimeDuration";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();
                if (globalsetting != null)
                {
                    int.TryParse(globalsetting.Value, out result);
                }
                System.Web.HttpRuntime.Cache[CustomerAddressPdfFormat] = result;
            }
            else
            {
                result = (int)System.Web.HttpRuntime.Cache[CustomerAddressPdfFormat];
            }
            return result;
        }

        public GlobalSetting GetGlobalSettingsByKey(Guid CompanyId,string searchKey)
        {
            GlobalSetting globalSetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, searchKey)).FirstOrDefault();
            if (globalSetting == null) { 
                globalSetting = new GlobalSetting();
                globalSetting.Value = string.Empty;
            }
            return globalSetting;
        }
        public GlobalSetting GetGlobalSettingsByKey(string searchKey)
        {
            GlobalSetting globalSetting = _GlobalSettingDataAccess.GetByQuery(string.Format("SearchKey = '{0}'", searchKey)).FirstOrDefault();
            if (globalSetting == null)
            {
                globalSetting = new GlobalSetting();
                globalSetting.Value = string.Empty;
            }
            return globalSetting;
        }
        public GlobalSetting GetGlobalSettingsByOnlyKey(string searchKey)
        {
            GlobalSetting globalSetting = _GlobalSettingDataAccess.GetByQuery(string.Format("SearchKey = '{0}'", searchKey)).FirstOrDefault();
            if (globalSetting == null)
            {
                globalSetting = new GlobalSetting();
                globalSetting.Value = string.Empty;
            }
            return globalSetting;
        }
        public GlobalSetting getTypeTaxByTaxTypeValueAndCompanyId(Guid companyid, double value)
        {
            return _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and Value = '{1}'", companyid, value)).FirstOrDefault();
        }

        public List<GlobalSetting> GetGlobalSettingByTag(string tag)
        {
            return _GlobalSettingDataAccess.GetByQuery(string.Format("Tag = '{0}'", tag)).ToList();
        }
        //public GlobalSetting GetAlarmSearchkeyandValue()
        //{

        //    DataTable dt = _GlobalSettingDataAccess.GetAlarmSearchkeyandValue();

        //    List<GlobalSetting> viewList = new List<GlobalSetting>();
        //    viewList = (from DataRow dr in dt.Rows
        //                select new GlobalSetting()
        //                {
        //                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,

        //                    SearchKey = dr["SearchKey"].ToString(),
        //                    Value = dr["Value"].ToString()
        //                }).ToList();
        //    GlobalSetting leadlist = new GlobalSetting();
        //    leadlist.globalsettings = viewList;
        //    return leadlist;
        //}
        public bool SendNotificationToTicketAdditionalMembers(Guid value)
        {
            string asd = string.Format("SearchKey = '{0}'", "SendNotificationToTicketAdditionalMembers");

            GlobalSetting GlobSet = _GlobalSettingDataAccess.GetByQuery(asd).FirstOrDefault();
            if(GlobSet == null || GlobSet.Value == null)
            {
                return true;
            }
            if(GlobSet.Value.ToLower() == "false")
            {
                return false;
            }
            return true;
        }

        public string GetFooterCompanyInformation(Guid companyId)
        {
            GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", companyId, "FooterCompanyInformation")).FirstOrDefault();
            return globalsetting == null ? "" : globalsetting.Value;
        }

        public string GetTicketAdditionalMemberOnlyTechnicianByCompanyId(Guid CompanyId)
        {
            string TicketAdditionalMemberOnlyTechnician = RMRCacheKey.TicketAdditionalMemberOnlyTechnician + CompanyId.ToString();

            string result = string.Empty;
            if (System.Web.HttpRuntime.Cache[TicketAdditionalMemberOnlyTechnician] == null)
            {
                string SearchKey = "TicketAdditionalMemberOnlyTechnician";
                GlobalSetting globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, SearchKey)).FirstOrDefault();
                result = globalsetting == null ? "" : globalsetting.Value;
                System.Web.HttpRuntime.Cache[TicketAdditionalMemberOnlyTechnician] = result;
            }
            else
            {
                result = (string)System.Web.HttpRuntime.Cache[TicketAdditionalMemberOnlyTechnician];
            }
            return result;
        }

        public long InsertGlobalSetting(GlobalSetting gs)
        {
            return _GlobalSettingDataAccess.Insert(gs);
        }
    }
}
