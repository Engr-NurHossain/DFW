using HS.DataAccess;
using HS.Entities;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Facade
{
    public class ThirdPartySettingFacade : BaseFacade
    {
        public ThirdPartySettingFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        ThirdPartySettingDataAccess _ThirdPartySettingDataAccess
        {
            get
            {
                return (ThirdPartySettingDataAccess)_ClientContext[typeof(ThirdPartySettingDataAccess)];
            }
        }
        public ThirdPartySetting GetThirdPartyById(int value)
        {
            return _ThirdPartySettingDataAccess.Get(value);
        }
        public List<ThirdPartySetting> GetAllAlarmSettingByCompanyId(Guid Companyid)
        {
            DataTable dt = _ThirdPartySettingDataAccess.GetAllAlarmSettingByCompanyId(Companyid);
            List<ThirdPartySetting> alarmlist = new List<ThirdPartySetting>();
            alarmlist = (from DataRow dr in dt.Rows
                                select new ThirdPartySetting()
                                {
                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                    CompanyId = (Guid)dr["CompanyId"],
                                    Type = dr["Type"].ToString(),
                                    Name = dr["Name"].ToString(),
                                    Value = dr["Value"].ToString()
                                }).ToList();
            return alarmlist;
        }

        public List<ThirdPartySetting> GetAllTechSettingByCompanyId(Guid Companyid)
        {
            DataTable dt = _ThirdPartySettingDataAccess.GetAllTechSettingByCompanyId(Companyid);
            List<ThirdPartySetting> techlist = new List<ThirdPartySetting>();
            techlist = (from DataRow dr in dt.Rows
                         select new ThirdPartySetting()
                         {
                             Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                             CompanyId = (Guid)dr["CompanyId"],
                             Type = dr["Type"].ToString(),
                             Name = dr["Name"].ToString(),
                             Value = dr["Value"].ToString()
                         }).ToList();
            return techlist;
        }

        public List<ThirdPartySetting> GetAllAuthorizeSettingByCompanyId(Guid Companyid)
        {
            DataTable dt = _ThirdPartySettingDataAccess.GetAllAuthorizeSettingByCompanyId(Companyid);
            List<ThirdPartySetting> authorizelist = new List<ThirdPartySetting>();
            authorizelist = (from DataRow dr in dt.Rows
                                select new ThirdPartySetting()
                                {
                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                    CompanyId= (Guid)dr["CompanyId"],
                                    Type = dr["Type"].ToString(),
                                    Name=dr["Name"].ToString(),
                                    Value=dr["Value"].ToString()
                                }).ToList();
            return authorizelist;
        }
        public bool UpdateThirdPartySetting(ThirdPartySetting cb)
        {
            return _ThirdPartySettingDataAccess.Update(cb) > 0;
        }
        public long InsertThirdPartySetting(ThirdPartySetting cb)
        {
            return _ThirdPartySettingDataAccess.Insert(cb);
        }
        public bool DeleteThirdPartySetting(int Id)
        {
            return _ThirdPartySettingDataAccess.Delete(Id) > 0;
        }
    }
}
