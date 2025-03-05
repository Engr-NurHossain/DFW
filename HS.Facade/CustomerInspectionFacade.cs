using HS.DataAccess;
using HS.Framework;
using HS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using HS.Entities.List;
using Forte;
using Forte.Entities;
using RestSharp;
using System.Security.Authentication;
using System.Net;

namespace HS.Facade
{
    public class CustomerInspectionFacade : BaseFacade
    {
        public CustomerInspectionFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        CustomerInspectionDataAccess _CustomerInspectionDataAccess
        {
            get
            {
                return (CustomerInspectionDataAccess)_ClientContext[typeof(CustomerInspectionDataAccess)];
            }
        }
        CustomerDataAccess _CustomerDataAccess
        {
            get
            {
                return (CustomerDataAccess)_ClientContext[typeof(CustomerDataAccess)];
            }
        }
        CompanyDataAccess _CompanyDataAccess
        {
            get
            {
                return (CompanyDataAccess)_ClientContext[typeof(CompanyDataAccess)];
            }
        }
        CustomerCompanyDataAccess _CustomerCompanyDataAccess
        {
            get
            {
                return (CustomerCompanyDataAccess)_ClientContext[typeof(CustomerCompanyDataAccess)];
            }
        }
        GlobalSettingDataAccess _GlobalSettingDataAccess
        {
            get
            {
                return new GlobalSettingDataAccess();
            }
        }
        public CustomerInspection GetCustomerInspectionById(int Id)
        {
            return _CustomerInspectionDataAccess.Get(Id);
        }
        public CustomerInspection GetCustomerInspectionByCustomerId(Guid customerId)
        {
            return _CustomerInspectionDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerId)).FirstOrDefault();
        }
        
        public List<CustomerInspection> GetAllCustomerInspections()
        {
            return _CustomerInspectionDataAccess.GetAll();
        }
        public bool InsertCustomerInspection(CustomerInspection customerInspection)
        {
            return _CustomerInspectionDataAccess.Insert(customerInspection)>0;
        }
        public bool UpdateCustomerInspection(CustomerInspection customerInspection)
        {
            return _CustomerInspectionDataAccess.Update(customerInspection) > 0;
        }
    }
}
