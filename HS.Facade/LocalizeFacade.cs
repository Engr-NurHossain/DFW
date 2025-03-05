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
    public class LocalizeFacade : BaseFacade
    {
        LanguageDataAccess _LanguageDataAccess;
        LocalizeResourceDataAccess _LocalizeResourceDataAccess;
        public LocalizeFacade(ClientContext clientContext)
            : base(clientContext)
        {
            _LanguageDataAccess = (LanguageDataAccess)_ClientContext[typeof(LanguageDataAccess)];
            _LocalizeResourceDataAccess = (LocalizeResourceDataAccess)_ClientContext[typeof(LocalizeResourceDataAccess)];
        }

        public LocalizeFacade()
        {
            _LanguageDataAccess = new LanguageDataAccess();
            _LocalizeResourceDataAccess = new LocalizeResourceDataAccess();
        }

        public LocalizeFacade(string constr)
        {
            _LanguageDataAccess = new LanguageDataAccess(constr);
            _LocalizeResourceDataAccess = new LocalizeResourceDataAccess(constr);
        }
        

        public List<Language> GetAllLanguageByCompanyId(Guid CompanyId)
        {
            return _LanguageDataAccess.GetByQuery(string.Format(" CompanyId = '{0}'", CompanyId));
        }
        public List<LocalizeResource> GetAllLocalizeResourceByCompanyId(Guid CompanyId)
        {
            return _LocalizeResourceDataAccess.GetByQuery(string.Format(" CompanyId = '{0}'", CompanyId));
        }

        public string GetResource(string resourceKey)
        {
            Guid comid = new Guid();
            #region Company Id
            if (System.Web.HttpContext.Current.Session[SessionKeys.CompanyId] != null)
            {
                comid = (Guid)System.Web.HttpContext.Current.Session[SessionKeys.CompanyId];
            }
            #endregion
            if (string.IsNullOrWhiteSpace(resourceKey))
            {
                return resourceKey;
            }
            string result = string.Empty;
            string currentLanguage = "en-US";
            string LanguagePack = RMRCacheKey.LanguagePack;

            if (HttpContext.Current.Request.Cookies["__Lng"] != null)
            {
                currentLanguage = HttpContext.Current.Request.Cookies["__Lng"].Value;
            }
            if(HttpContext.Current.Session[SessionKeys.CompanyId] != null)
            {
                string CompanyId = ((Guid)HttpContext.Current.Session[SessionKeys.CompanyId]).GetHashCode().ToString();
                LanguagePack += CompanyId;
            }

            List<LocalizeResource> resources = new List<LocalizeResource>();
            if(HttpContext.Current.Request.RawUrl.ToLower().IndexOf("ieatery.com") > -1)
            {
                System.Web.HttpRuntime.Cache.Remove("LanguagePack");
            }
            if (System.Web.HttpRuntime.Cache[LanguagePack] == null)
            {
                resources = _LocalizeResourceDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", comid)).ToList();
                System.Web.HttpRuntime.Cache[LanguagePack] = resources;
            }
            else
            {
                resources = (List<LocalizeResource>)System.Web.HttpRuntime.Cache[LanguagePack];
            }


            Language language = new Language();
            if (System.Web.HttpRuntime.Cache[RMRCacheKey.Language] == null)
            {
                language = _LanguageDataAccess.GetByQuery(" LanguageCulture='" + currentLanguage + "' ").FirstOrDefault();
                System.Web.HttpRuntime.Cache[RMRCacheKey.Language] = language;
            }
            else
            {
                language = (Language)System.Web.HttpRuntime.Cache[RMRCacheKey.Language];
                if (language.LanguageCulture != currentLanguage)
                {
                    language = _LanguageDataAccess.GetByQuery(" LanguageCulture='" + currentLanguage + "' ").FirstOrDefault();
                    System.Web.HttpRuntime.Cache[RMRCacheKey.Language] = language;
                }
            }
            if (resources.Count > 0)
            {
                var res = resources.Where(r => r.ResourceName.Trim().ToLower() == resourceKey.Trim().ToLower() && r.LanguageId == language.Id).FirstOrDefault();
                if (res != null)
                    result = res.ResourceValue.Trim();
                else
                    result = resourceKey;
            }

            return result;

        }

        public List<LocalizeResource> GetAllLocalizeResourceByCompanyIdAndLanguageId(Guid CompanyId, int langId)
        {
            return _LocalizeResourceDataAccess.GetByQuery(string.Format(" CompanyId = '{0}' and LanguageId = '{1}'", CompanyId, langId ));
        }

        public LocalizeResourceViewModel GetAllLocalizeResourceByFilter(LocalizeFilterModel filter)
        {
            DataSet ds =  _LocalizeResourceDataAccess.GetAllLocalizeResourceByFilter(filter);

            LocalizeResourceViewModel Model = new LocalizeResourceViewModel();
            Model.LocalizeResource = (from DataRow dr in ds.Tables[0].Rows
                                      select new LocalizeResource()
                                      {
                                          Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                          CompanyId = (Guid)dr["CompanyId"],
                                          LanguageId = dr["LanguageId"] != DBNull.Value ? Convert.ToInt32(dr["LanguageId"]) : 0,
                                          ResourceName = dr["ResourceName"].ToString(),
                                          ResourceValue = dr["ResourceValue"].ToString(),
                                      }).ToList();

            Model.TotalCount = ds.Tables[1].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCount"]) : 0;

            return Model;
        }

        public LocalizeResource GetLocalizeResourceById(int id)
        {
            return _LocalizeResourceDataAccess.Get(id);
        }

        public bool UpdateLocalizeResource(LocalizeResource lr)
        {
            return _LocalizeResourceDataAccess.Update(lr)>0;
        }

        public int InsertLocalizeResource(LocalizeResource model)
        {
            return (int)_LocalizeResourceDataAccess.Insert(model);
        }

        public bool DeleteLocalizeResource(int value)
        {
            return _LocalizeResourceDataAccess.Delete(value) > 0;
        }

        public int InsertLanguage(Language model)
        {
            return (int)_LanguageDataAccess.Insert(model);
        }
    }
}
