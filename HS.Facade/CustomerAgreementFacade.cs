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
    public class CustomerAgreementFacade : BaseFacade
    {
        public CustomerAgreementFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        CustomerAgreementDataAccess _CustomerAgreementDataAccess
        {
            get
            {
                return (CustomerAgreementDataAccess)_ClientContext[typeof(CustomerAgreementDataAccess)];
            }
        }
        CustomerDataAccess _CustomerDataAccess
        {
            get
            {
                return (CustomerDataAccess)_ClientContext[typeof(CustomerDataAccess)];
            }
        }

        public DataTable GetAllCustomerList(string query)
        {
            return _CustomerDataAccess.GetAllCustomerList(query);    
        }
        public DataTable GetAllLeadList(string query)
        {
            return _CustomerDataAccess.GetAllLeadList(query);
        }
        public long InsertCustomerAgreement(CustomerAgreement ca)
        {
            return _CustomerAgreementDataAccess.Insert(ca);
        }

        public bool UpdateCustomerAgreement(CustomerAgreement ca)
        {
            return _CustomerAgreementDataAccess.Update(ca) > 0;
        }
        public CustomerAgreement GetCustomerAgreementHistory(Guid CustomerId,string HistoryType)
        {
            return _CustomerAgreementDataAccess.GetByQuery(string.Format("CustomerId='{0}' and Type='{1}'", CustomerId, HistoryType)).FirstOrDefault();
        }
        public CustomerAgreement GetCustomerAgreementByCompanyIdAndCustomerId(Guid companyid, Guid customerid)
        {
            return _CustomerAgreementDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and CustomerId = '{1}'", companyid, customerid)).FirstOrDefault();
        }
        public List<CustomerAgreement> GetCustomerAgreementByCompanyIdAndCustomerId1(Guid companyid, Guid customerid)
        {
            return _CustomerAgreementDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and CustomerId = '{1}'", companyid, customerid)).ToList();
        }
        public CustomerAgreement GetCustomerAgreementByComIdAndCusIsAndLoadAgreement(Guid comid, Guid cusid)
        {
            return _CustomerAgreementDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and CustomerId = '{1}' and Type = 'LoadAgreement'", comid, cusid)).FirstOrDefault();
        }
        public CustomerAgreement GetCustomerAgreementByComIdAndCusIsAndSignAgreement(Guid comid, Guid cusid)
        {
            return _CustomerAgreementDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and CustomerId = '{1}' and Type = 'SignAgreement'", comid, cusid)).FirstOrDefault();
        }
        public CustomerAgreement GetCustomerAgreementByComIdAndCusIsAndSubmitAgreement(Guid comid, Guid cusid)
        {
            return _CustomerAgreementDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and CustomerId = '{1}' and Type = 'SubmitAgreement'", comid, cusid)).FirstOrDefault();
        }
        public List<CustomerAgreement> GetAllCustomerAgreementByCustomerId(Guid cusid)
        {
            return _CustomerAgreementDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", cusid)).ToList();
        }
        public void DeleteAgreementInfoById(int id)
        {
            _CustomerAgreementDataAccess.Delete(id);
        }
        public CustomerAgreement GetIpAndUserAgentByCustomerIdAndCompanyId(Guid comid, Guid Customerid)
        {
            DataTable dt = _CustomerAgreementDataAccess.GetIpAndUserAgentByCustomerIdAndCompanyId(comid, Customerid);
            CustomerAgreement CopanyList = new CustomerAgreement();
            CopanyList = (from DataRow dr in dt.Rows
                          select new CustomerAgreement()
                          {
                              IP = dr["IP"].ToString(),
                              UserAgent = dr["UserAgent"].ToString()
                          }).FirstOrDefault();
            return CopanyList;
        }

        public List<CustomerAgreement> GetCustomerAgreementByCompanyIdAndCustomerIdAndInvoiceId(Guid companyId, Guid customerId, string InvoiceId)
        {
            DataTable dt = _CustomerAgreementDataAccess.GetCustomerAgreementByCompanyIdAndCustomerIdAndInvoiceId(companyId, customerId, InvoiceId);
            List< CustomerAgreement> CopanyList = (from DataRow dr in dt.Rows
                          select new CustomerAgreement()
                          {
                              IP = dr["IP"].ToString(),
                              UserAgent = dr["UserAgent"].ToString(),
                              InvoiceId = dr["InvoiceId"].ToString(),
                              Type = dr["Type"].ToString(),
                              AddedDate = dr["AddedDate"] != DBNull.Value ? Convert.ToDateTime(dr["AddedDate"]) : new DateTime(),
                          }).ToList();
            return CopanyList;
        }
    }
}
