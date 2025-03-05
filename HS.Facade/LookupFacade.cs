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
using System.Web.Mvc;

namespace HS.Facade
{
    public class LookupFacade : BaseFacade
    {
        LookupDataAccess _LookupDataAccess = null;
        public LookupFacade(ClientContext clientContext)
            : base(clientContext)
        {

            _LookupDataAccess = (LookupDataAccess)_ClientContext[typeof(LookupDataAccess)];
        }

        public LookupFacade()
        {
            _LookupDataAccess = new LookupDataAccess();
        }

        public LookupFacade(string constr)
        {
            _LookupDataAccess = new LookupDataAccess(constr);
        }

        public List<Lookup> GetLookupByKeyWithParent(string key, bool IncludeInActive = false)
        {
            Guid comid = new Guid();
            #region Company Id
            if(System.Web.HttpContext.Current.Session[SessionKeys.CompanyId] != null)
            {
                comid = (Guid)System.Web.HttpContext.Current.Session[SessionKeys.CompanyId];
            }
            #endregion
            LocalizeFacade _localize = new LocalizeFacade();
            List<Lookup> resultLookup = new List<Lookup>();
            string currentLanguage = "en-US";
            if (HttpContext.Current.Request.Cookies["__Lng"] != null)
            {
                currentLanguage = HttpContext.Current.Request.Cookies["__Lng"].Value;
            }
            string cachekey = key + currentLanguage+ comid.ToString();
            System.Web.HttpRuntime.Cache.Remove(cachekey);
            if (System.Web.HttpRuntime.Cache[cachekey] == null)
            {
                DataTable dt = _LookupDataAccess.GetLookupByKeyWithParent(key, comid);
                resultLookup = (from DataRow dr in dt.Rows
                                select new Lookup()
                                {
                                    DisplayText = dr["DisplayText"].ToString(),
                                    DataValue = dr["DataValue"].ToString(),
                                    IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false
                                }).ToList();
                foreach (var lookup in resultLookup)
                {
                    lookup.DisplayText = _localize.GetResource(lookup.DisplayText);
                }
                System.Web.HttpRuntime.Cache[cachekey] = resultLookup;
            }
            else
            {
                resultLookup = (List<Lookup>)System.Web.HttpRuntime.Cache[cachekey];
            }
            if (!IncludeInActive && resultLookup != null && resultLookup.Count() > 0)
            {
                return resultLookup.Where(x => x.IsActive == true).ToList();
            }
            else
            {
                return resultLookup;
            }
        }

        #region Dropdown management
        public List<Lookup> GetLookupByKey(string key, bool IncludeInActive = false,bool ClearCache = false)
        {
            return lookups(key, IncludeInActive, ClearCache);
        }

        public List<SelectListItem> GetDropdownsByKey(string key, bool IncludeInActive = false)
        {
            List<Lookup> lookuplist = lookups(key, IncludeInActive);
            List<SelectListItem> selectListItems = lookuplist.OrderBy(x=>x.DataOrder).Select(x =>
                new SelectListItem()
                {
                    Text = x.DisplayText.ToString(),
                    Value = x.DataValue.ToString(),
                    Selected = (x.IsDefaultItem.HasValue && x.IsDefaultItem.Value)
                }).ToList();
            return selectListItems;
        }
     
        public List<Lookup> GetLookUpByKey(string key, bool IncludeInActive = false)
        {
            List<Lookup> lookuplist = lookups(key, IncludeInActive);
        
            return lookuplist;
        }
        private List<Lookup> lookups(string key, bool IncludeInActive = false, bool ClearCache = false)
        {
            Guid comid = new Guid();
            #region Company Id
            if (System.Web.HttpContext.Current.Session[SessionKeys.CompanyId] != null)
            {
                comid = (Guid)System.Web.HttpContext.Current.Session[SessionKeys.CompanyId];
            }
            #endregion
            LocalizeFacade _localize = new LocalizeFacade();
            List<Lookup> resultLookup = new List<Lookup>();
            string currentLanguage = "en-US";
            if (HttpContext.Current.Request.Cookies["__Lng"] != null)
            {
                currentLanguage = HttpContext.Current.Request.Cookies["__Lng"].Value;
            }
            string cachekey = key + currentLanguage+ comid.ToString();
            if (ClearCache)
            {
                System.Web.HttpRuntime.Cache.Remove(cachekey);
            }
            if (System.Web.HttpRuntime.Cache[cachekey] == null)
            {
                resultLookup = _LookupDataAccess.GetByQuery(string.Format(" DataKey = '{0}' and CompanyId = '{1}' order by DataOrder asc", key, comid));
                foreach (var lookup in resultLookup)
                {
                    lookup.DisplayText = _localize.GetResource(lookup.DisplayText);
                }
                //System.Web.HttpRuntime.Cache[cachekey] = resultLookup;
            }
            else
            {
                resultLookup = (List<Lookup>)System.Web.HttpRuntime.Cache[cachekey];
            }
            if (!IncludeInActive && resultLookup != null && resultLookup.Count() > 0)
            {
                return resultLookup.Where(x => x.IsActive == true).OrderBy(x => x.DataOrder).ToList();
                System.Web.HttpRuntime.Cache[cachekey] = resultLookup;
            }
            else
            {
                return resultLookup.OrderBy(x => x.DataOrder).ToList();
                System.Web.HttpRuntime.Cache[cachekey] = resultLookup;
            }
        }
        #endregion

        public List<Lookup> GetLookupByKeyWithoutOrder(string key, bool IncludeInActive = false)
        {
            Guid comid = new Guid();
            #region Company Id
            if (System.Web.HttpContext.Current.Session[SessionKeys.CompanyId] != null)
            {
                comid = (Guid)System.Web.HttpContext.Current.Session[SessionKeys.CompanyId];
            }
            #endregion
            LocalizeFacade _localize = new LocalizeFacade();
            List<Lookup> resultLookup = new List<Lookup>();
            string currentLanguage = "en-US";
            if (HttpContext.Current.Request.Cookies["__Lng"] != null)
            {
                currentLanguage = HttpContext.Current.Request.Cookies["__Lng"].Value;
            }
            string cachekey = key + currentLanguage+ comid.ToString();
            System.Web.HttpRuntime.Cache.Remove(cachekey);
            if (System.Web.HttpRuntime.Cache[cachekey] == null)
            {
                resultLookup = _LookupDataAccess.GetByQuery(string.Format(" DataKey = '{0}' and CompanyId = '{1}' and DataOrder != 0 order by DisplayText", key, comid));
                foreach (var lookup in resultLookup)
                {
                    lookup.DisplayText = _localize.GetResource(lookup.DisplayText);
                }
                System.Web.HttpRuntime.Cache[cachekey] = resultLookup;
            }
            else
            {
                resultLookup = (List<Lookup>)System.Web.HttpRuntime.Cache[cachekey];
            }
            if (!IncludeInActive && resultLookup != null && resultLookup.Count() > 0)
            {
                return resultLookup.Where(x => x.IsActive == true).ToList();
            }
            else
            {
                return resultLookup;
            }
        }
        public List<Lookup> GetAllLookupByKey(string dataKey)
        {
            return _LookupDataAccess.GetByQuery(string.Format(" DataKey = '{0}' and IsActive = 1 ", dataKey));
        }
        public List<Lookup> GetLookupByParentKey(string key)
        {
            Guid comid = new Guid();
            #region Company Id
            if (System.Web.HttpContext.Current.Session[SessionKeys.CompanyId] != null)
            {
                comid = (Guid)System.Web.HttpContext.Current.Session[SessionKeys.CompanyId];
            }
            #endregion
            LocalizeFacade _localize = new LocalizeFacade();
            List<Lookup> resultLookup = new List<Lookup>();
            string currentLanguage = "en-US";
            if (HttpContext.Current.Request.Cookies["__Lng"] != null)
            {
                currentLanguage = HttpContext.Current.Request.Cookies["__Lng"].Value;
            }
            string cachekey = key + currentLanguage + comid.ToString();
            System.Web.HttpRuntime.Cache.Remove(cachekey);
            if (System.Web.HttpRuntime.Cache[cachekey] == null)
            {
                resultLookup = _LookupDataAccess.GetByQuery(string.Format(" ParentDataKey = '{0}' and CompanyId = '{1}' order by DataOrder ", key, comid));
                foreach (var lookup in resultLookup)
                {
                    lookup.DisplayText = _localize.GetResource(lookup.DisplayText);
                }
                System.Web.HttpRuntime.Cache[cachekey] = resultLookup;
            }
            else
            {
                resultLookup = (List<Lookup>)System.Web.HttpRuntime.Cache[cachekey];
            }
            return resultLookup;
        }

        public List<Lookup> GetLookupByKeyForReport(string key)
        {
            Guid comid = new Guid();
            #region Company Id
            if (System.Web.HttpContext.Current.Session[SessionKeys.CompanyId] != null)
            {
                comid = (Guid)System.Web.HttpContext.Current.Session[SessionKeys.CompanyId];
            }
            #endregion
            LocalizeFacade _localize = new LocalizeFacade();
            List<Lookup> resultLookup = new List<Lookup>();
            string currentLanguage = "en-US";
            if (HttpContext.Current.Request.Cookies["__Lng"] != null)
            {
                currentLanguage = HttpContext.Current.Request.Cookies["__Lng"].Value;
            }
            string cachekey = key + currentLanguage+comid.ToString();
            if (System.Web.HttpRuntime.Cache[cachekey] == null)
            {
                resultLookup = _LookupDataAccess.GetByQuery(string.Format(" DataKey = '{0}' and DisplayText != 'Debit Card' and DataValue != 'Check' and CompanyId = '{1}' order by DataOrder ", key, comid));
                foreach (var lookup in resultLookup)
                {
                    lookup.DisplayText = _localize.GetResource(lookup.DisplayText);
                }
                System.Web.HttpRuntime.Cache[cachekey] = resultLookup;
            }
            else
            {
                resultLookup = (List<Lookup>)System.Web.HttpRuntime.Cache[cachekey];
            }
            return resultLookup;
        }

        public string GetDisplayTextByDataValueFromLLookup(string value)
        {
            Guid comid = new Guid();
            #region Company Id
            if (System.Web.HttpContext.Current.Session[SessionKeys.CompanyId] != null)
            {
                comid = (Guid)System.Web.HttpContext.Current.Session[SessionKeys.CompanyId];
            }
            #endregion
            if (!string.IsNullOrWhiteSpace(value))
            {
                var selected = _LookupDataAccess.GetByQuery(string.Format("DataValue = '{0}' and CompanyId = '{1}'", value, comid)).FirstOrDefault();
                return selected != null ? selected.DisplayText : "";
            }
            return value;
        }
        public string GetDisplayTextByDataKeyandDataValueFromLLookup(string key,string value)
        {
            Guid comid = new Guid();
            #region Company Id
            if (System.Web.HttpContext.Current.Session[SessionKeys.CompanyId] != null)
            {
                comid = (Guid)System.Web.HttpContext.Current.Session[SessionKeys.CompanyId];
            }
            #endregion
            if (!string.IsNullOrWhiteSpace(value))
            {
                var selected = _LookupDataAccess.GetByQuery(string.Format("DataKey = '{0}'and DataValue = '{1}' and CompanyId = '{2}'", key, value, comid)).FirstOrDefault();
                return selected != null ? selected.DisplayText : "";
            }
            return value;
        }
        public List<Lookup> GetLookupByKeyForArrivalOrDepartureTimeByMinAndMaxTimeRange(string key, string min, string max)
        {
            Guid comid = new Guid();
            #region Company Id
            if (System.Web.HttpContext.Current.Session[SessionKeys.CompanyId] != null)
            {
                comid = (Guid)System.Web.HttpContext.Current.Session[SessionKeys.CompanyId];
            }
            #endregion
            LocalizeFacade _localize = new LocalizeFacade();
            List<Lookup> resultLookup = new List<Lookup>();
            string currentLanguage = "en-US";
            if (HttpContext.Current.Request.Cookies["__Lng"] != null)
            {
                currentLanguage = HttpContext.Current.Request.Cookies["__Lng"].Value;
            }
            string cachekey = key + currentLanguage+ comid.ToString();
            System.Web.HttpRuntime.Cache.Remove(cachekey);
            if (System.Web.HttpRuntime.Cache[cachekey] == null)
            {
                if(!string.IsNullOrWhiteSpace(min) && !string.IsNullOrWhiteSpace(max) && min != "-1" && max != "-1")
                {
                    resultLookup = _LookupDataAccess.GetByQuery(string.Format(" DataKey = '{0}' and DataValue between '{1}' and '{2}' and CompanyId = '{3}' and IsActive = 1 order by DataOrder ", key, min, max, comid));
                }
                else
                {
                    resultLookup = _LookupDataAccess.GetByQuery(string.Format(" DataKey = '{0}' and CompanyId = '{1}' and IsActive = 1 order by DataOrder ", key, comid));
                }
                foreach (var lookup in resultLookup)
                {
                    lookup.DisplayText = _localize.GetResource(lookup.DisplayText);
                }
                System.Web.HttpRuntime.Cache[cachekey] = resultLookup;
            }
            else
            {
                resultLookup = (List<Lookup>)System.Web.HttpRuntime.Cache[cachekey];
            }
            return resultLookup;
        }

        public Lookup GetLookUpById(int value)
        {
            return _LookupDataAccess.Get(value);
        }

        public bool UpdateLookUp(Lookup lu)
        {
            if(lu.IsDefaultItem == true)
            {
                _LookupDataAccess.SetAllDefaultItemsToFalseByDataKey(lu.DataKey);
            }
            var res = _LookupDataAccess.Update(lu) > 0;
            if (res)
            {
                string currentLanguage = "en-US";
                if (HttpContext.Current != null
                    && HttpContext.Current.Request != null
                    && HttpContext.Current.Request.Cookies["__Lng"] != null)
                {
                    currentLanguage = HttpContext.Current.Request.Cookies["__Lng"].Value;
                }
                string cachekey = lu.DataKey + currentLanguage+lu.CompanyId.ToString();
                System.Web.HttpRuntime.Cache.Remove(cachekey);
            }
            return res;
        }

        public List<Lookup> GetAllLookup()
        {
            return _LookupDataAccess.GetAll();
        }

        public List<Lookup> GetAllLookupByCompanyId(Guid companyid)
        {
            return _LookupDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", companyid)).ToList();
        }

        public List<string> GetDataKeyList()
        {
            return _LookupDataAccess.GetDataKeyList();
        }

        public Lookup GetLookupByKeyAndValue(string dataKey, string dataValue)
        {
            Guid comid = new Guid();
            #region Company Id
            if (System.Web.HttpContext.Current.Session[SessionKeys.CompanyId] != null)
            {
                comid = (Guid)System.Web.HttpContext.Current.Session[SessionKeys.CompanyId];
            }
            #endregion
            return _LookupDataAccess.GetByQuery(string.Format(" datakey='{0}' and datavalue='{1}' and CompanyId = '{2}'", dataKey, dataValue, comid)).FirstOrDefault();
        }
        public List<Lookup> GetCsReceiverNumberLookupByKeyAndPrifix(string dataKey, string dataValue)
        {
            Guid comid = new Guid();

            #region Company Id
            if (System.Web.HttpContext.Current.Session[SessionKeys.CompanyId] != null)
            {
                comid = (Guid)System.Web.HttpContext.Current.Session[SessionKeys.CompanyId];
            }
            #endregion

            return _LookupDataAccess.GetByQuery(string.Format(" datakey='{0}' and DisplayText like '%({1})' and CompanyId = '{2}'", dataKey, dataValue, comid)).ToList();
          
        }
        public Lookup GetLookupByKeyAndId(string dataKey, int id)
        {
            Guid comid = new Guid();
            #region Company Id
            if (System.Web.HttpContext.Current.Session[SessionKeys.CompanyId] != null)
            {
                comid = (Guid)System.Web.HttpContext.Current.Session[SessionKeys.CompanyId];
            }
            #endregion
            return _LookupDataAccess.GetByQuery(string.Format(" datakey='{0}' and Id={1} and CompanyId = '{2}'", dataKey, id, comid)).FirstOrDefault();
        }

        public int InsertLookup(Lookup lk)
        {
            lk.Id = (int)_LookupDataAccess.Insert(lk);
            if (lk.Id > 0)
            {
                string currentLanguage = "en-US";
                if (HttpContext.Current != null
                    && HttpContext.Current.Request != null
                    && HttpContext.Current.Request.Cookies["__Lng"] != null)
                {
                    currentLanguage = HttpContext.Current.Request.Cookies["__Lng"].Value;
                }
                string cachekey = lk.DataKey + currentLanguage;
                System.Web.HttpRuntime.Cache.Remove(cachekey);
            }
            return lk.Id;
        }
        public bool DeleteLookupByIdAndDataKeyWithChild(int lookupId, string DataKey,Guid CompanyId)
        {
            if (!string.IsNullOrWhiteSpace(DataKey) && _LookupDataAccess.DeleteLookUpWithChild(lookupId))
            {
                string currentLanguage = "en-US";
                if (HttpContext.Current != null
                    && HttpContext.Current.Request != null
                    && HttpContext.Current.Request.Cookies["__Lng"] != null)
                {
                    currentLanguage = HttpContext.Current.Request.Cookies["__Lng"].Value;
                }
                string cachekey = DataKey + currentLanguage + CompanyId.ToString();
                System.Web.HttpRuntime.Cache.Remove(cachekey);
                return true;
            }
            return false;
        }
        public bool DeleteLookupByIdAndDataKey(int lookupId, string DataKey, Guid CompanyId)
        {
            if (!string.IsNullOrWhiteSpace(DataKey) && _LookupDataAccess.Delete(lookupId) > 0)
            {
                string currentLanguage = "en-US";
                if (HttpContext.Current != null
                    && HttpContext.Current.Request != null
                    && HttpContext.Current.Request.Cookies["__Lng"] != null)
                {
                    currentLanguage = HttpContext.Current.Request.Cookies["__Lng"].Value;
                }
                string cachekey = DataKey + currentLanguage + CompanyId.ToString();
                System.Web.HttpRuntime.Cache.Remove(cachekey);
                return true;
            }
            return false;
        }

        public Lookup GetDisplayTextByDataKeyAndDataValue(string key, string value)
        {
            Guid comid = new Guid();
            #region Company Id
            if (System.Web.HttpContext.Current.Session[SessionKeys.CompanyId] != null)
            {
                comid = (Guid)System.Web.HttpContext.Current.Session[SessionKeys.CompanyId];
            }
            #endregion
            return _LookupDataAccess.GetByQuery(string.Format("DataKey = '{0}' and DataValue = '{1}' and CompanyId = '{2}'", key, value, comid)).FirstOrDefault();
        }

        public List<Lookup> GetAllLookupNotAvailableByDataKeyAndDataValue(string key, string value, Guid comid)
        {
            return _LookupDataAccess.GetByQuery(string.Format("DataKey = '{0}' and DataValue not in ({1}) and CompanyId = '{2}'", key, value, comid)).ToList();
        }

        public Lookup GetLookupByKeyAndValueAndCompanyId(string dataKey, string dataValue, Guid comid)
        {
            return _LookupDataAccess.GetByQuery(string.Format(" datakey='{0}' and datavalue='{1}' and CompanyId = '{2}'", dataKey, dataValue, comid)).FirstOrDefault();
        }
    }
}
