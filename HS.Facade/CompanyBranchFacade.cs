using HS.DataAccess;
using HS.Entities;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace HS.Facade
{
    public class CompanyBranchFacade : BaseFacade
    {
        CompanyBranchDataAccess _CompanyBranchDataAccess;
        UserBranchDataAccess _UserBranchDataAccess;
        public CompanyBranchFacade(ClientContext clientContext)
            : base(clientContext)
        {
            _CompanyBranchDataAccess = (CompanyBranchDataAccess)_ClientContext[typeof(CompanyBranchDataAccess)];
            _UserBranchDataAccess = (UserBranchDataAccess)_ClientContext[typeof(UserBranchDataAccess)];
        }

        public CompanyBranchFacade()
        {
            _CompanyBranchDataAccess = new CompanyBranchDataAccess();
            _UserBranchDataAccess = new UserBranchDataAccess();
        }

        public CompanyBranchFacade(string constr)
        {
            _CompanyBranchDataAccess = new CompanyBranchDataAccess(constr);
            _UserBranchDataAccess = new UserBranchDataAccess(constr);
        }

        public CompanyBranch GetCompanyBranchById(int value)
        {
            return _CompanyBranchDataAccess.Get(value);
        }
        public List<CompanyBranch> GetAllCompanyBranchByCompanyId(Guid Companyid)
        {
            return _CompanyBranchDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", Companyid));
        }

        public CompanyBranch GetMainCompanyBranchByCompanyId(Guid companyid)
        {
            return _CompanyBranchDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and IsMainBranch = 1", companyid)).FirstOrDefault();
        }

        public bool UpdateCompanyBranch(CompanyBranch cb)
        {
            return _CompanyBranchDataAccess.Update(cb) > 0;
        }
        public long InsertCompanyBranch(CompanyBranch cb)
        {
            return _CompanyBranchDataAccess.Insert(cb);
        }

        public CompanyBranch GetCompanyBranchByIdAndCompanyId(Guid CompanyId, int Id)
        {
            return _CompanyBranchDataAccess.GetByQuery(string.Format("Id = {0} and CompanyId = '{1}'", Id, CompanyId)).FirstOrDefault() ;
        }

        public bool DeleteCompanyBranch(int Id)
        {
            return _CompanyBranchDataAccess.Delete(Id) > 0;
        }
        public List<CompanyBranch> GetAllCompanyBranchWithStateAndTimeZone(Guid CompanyId)
        {
            DataTable dt = _CompanyBranchDataAccess.GetAllCompanyBranchWithStateAndTimeZone(CompanyId);
            List<CompanyBranch> CompanyBranchList = new List<CompanyBranch>();
            CompanyBranchList = (from DataRow dr in dt.Rows
                                    select new CompanyBranch()
                                    {
                                        Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                        Name = dr["Name"].ToString(),
                                        State = dr["State"].ToString(),
                                        TimeZoneDisplayText = dr["TimeZoneDisplay"].ToString()
                                    }).ToList();
            return CompanyBranchList;

        }
        public List<CityTax> GetCityTaxRate(Guid CustomerId,Guid CompanyId)
        {
            DataTable dt = _CompanyBranchDataAccess.GetCityTaxRate(CustomerId,CompanyId);
            List<CityTax> CityTaxList = new List<CityTax>();
            CityTaxList = (from DataRow dr in dt.Rows
                                 select new CityTax()
                                 {
                                     //Name = dr["Name"].ToString(),
                                     State = dr["State"].ToString(),
                                     City = dr["City"].ToString(),
                                     Rate = dr["Rate"] != DBNull.Value ? Convert.ToDouble(dr["Rate"]) : 0,
                                 }).ToList();
            return CityTaxList;

        }

        public bool RemoveAllMainBranchOfaCompanyExcludingId(Guid CompanyId, int id)
        {
            return _CompanyBranchDataAccess.RemoveAllMainBranchOfaCompanyExcludingId(id, CompanyId);
        }

        public CompanyBranch GetMainBranchByCompanyId(Guid CompanyId)
        {
           return _CompanyBranchDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and IsMainBranch =1", CompanyId)).FirstOrDefault();
        }
        public string GetCompanyLogoForPDFByCompanyId(Guid CompanyId)
        {
            string result = string.Empty;

            var LogoUrl = _CompanyBranchDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and IsMainBranch = 'true'", CompanyId)).FirstOrDefault();

            if(LogoUrl != null)
            {
                if(!string.IsNullOrEmpty(LogoUrl.ColorLogo) && !string.IsNullOrWhiteSpace(LogoUrl.ColorLogo)  )
                {
                    result = LogoUrl.ColorLogo;
                }
                else
                {
                    result = WebConfigurationManager.AppSettings["Logo.DefaultEmailLogo"];
                }
            }
            
            //string EmailLogo = "__CompanyLogoColored" + CompanyId.ToString();
            //var globalsetting = new GlobalSetting();
            //if (System.Web.HttpRuntime.Cache[EmailLogo] == null)
            //{
            //    string DataKey = "CompanyLogoColored";
            //    globalsetting = _GlobalSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SearchKey = '{1}'", CompanyId, DataKey)).FirstOrDefault();
            //    result = globalsetting.Value;
            //    System.Web.HttpRuntime.Cache[EmailLogo] = result;
            //}
            //else
            //{
            //    result = (string)System.Web.HttpRuntime.Cache[EmailLogo];
            //}
            return result;


        }
        public string GetCompanyEmailLogoByCompanyId(Guid CompanyId)
        {
            string result = string.Empty;

            var LogoUrl = _CompanyBranchDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and IsMainBranch = 'true'", CompanyId)).FirstOrDefault();

            if (LogoUrl != null)
            {
                if (!string.IsNullOrEmpty(LogoUrl.EmailLogo) && !string.IsNullOrWhiteSpace(LogoUrl.EmailLogo))
                {
                    result = LogoUrl.EmailLogo;
                }
                else
                {
                    result = WebConfigurationManager.AppSettings["Logo.DefaultEmailLogo"];
                }
            }
            return result;
        }

        public long InsertUserBranch(UserBranch ub)
        {
            return _UserBranchDataAccess.Insert(ub);
        }
    }
}
