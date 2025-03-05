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
    public class CustomerApiSettingFacade:BaseFacade
    {
        public CustomerApiSettingFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        CustomerCompanyDataAccess _CustomerCompanyDataAccess
        {
            get
            {
                return (CustomerCompanyDataAccess)_ClientContext[typeof(CustomerCompanyDataAccess)];
            }
        }
        CustomerApiSettingDataAccess _CustomerApiSettingDataAccess
        {
            get
            {
                return (CustomerApiSettingDataAccess)_ClientContext[typeof(CustomerApiSettingDataAccess)];
            }
        }
        public long InsertApiSetting(CustomerApiSetting api)
        {
            return _CustomerApiSettingDataAccess.Insert(api);
        }
        public CustomerApiSetting GetApiById(int id)
        {
            return _CustomerApiSettingDataAccess.Get(id);
        }
        public bool UpdateApi(CustomerApiSetting api)
        {
            return _CustomerApiSettingDataAccess.Update(api) > 0;
        }
        public CustomerApiSetting GetCustomerApiSettingByCustomerIdAndAccountName(string accountName, Guid CustomerId)
        {
            CustomerApiSetting setting = new CustomerApiSetting();
            CustomerCompany company = _CustomerCompanyDataAccess.GetByQuery(string.Format(" CustomerId = '{0}'", CustomerId.ToString())).FirstOrDefault();
            if(company != null)
            {
                string query = string.Format(" CompanyId = '{0}' AND CustomerId= '{1}' AND AccountName='{2}'  ", company.CompanyId, company.CustomerId, accountName);

                setting = _CustomerApiSettingDataAccess.GetByQuery(query).FirstOrDefault();
                 
            }
            return setting;

        }
        public List<CustomerApiSetting> GetAllApiSettingDetailByCustomerId(Guid customerid)
        {
            DataTable dt = _CustomerApiSettingDataAccess.GetAllApiSettingDetailByCustomerId(customerid);
            List<CustomerApiSetting> Apilist = new List<CustomerApiSetting>();
            Apilist = (from DataRow dr in dt.Rows
                               select new CustomerApiSetting()
                               {
                                   Url = dr["Url"].ToString(),
                                   UserName = dr["UserName"].ToString(),
                                   Password = dr["Password"].ToString(),
                                   AccountName = dr["AccountName"].ToString()
                               }).ToList();
            return Apilist;
        }
    }
}
