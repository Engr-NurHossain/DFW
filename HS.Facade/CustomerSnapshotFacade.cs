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
    public class CustomerSnapshotFacade : BaseFacade
    {
        CustomerSnapshotDataAccess _CustomerSnapshotDataAccess = null;
        public CustomerSnapshotFacade(ClientContext clientContext)
            : base(clientContext)
        {
            if (_CustomerSnapshotDataAccess == null)
                _CustomerSnapshotDataAccess = (CustomerSnapshotDataAccess)_ClientContext[typeof(CustomerSnapshotDataAccess)];
        }
        public CustomerSnapshotFacade(string ConStr)
        {
            if (_CustomerSnapshotDataAccess == null)
                _CustomerSnapshotDataAccess = new CustomerSnapshotDataAccess(ConStr);
        }
        public List<CustomerSnapshot> GetAllCustomerSnapshotByCustomerIdAndCompanyId(Guid CustomerId, Guid companyId)
        {
            return _CustomerSnapshotDataAccess.GetByQuery(string.Format(" CompanyId = '{0}' and  CustomerId = '{1}'", companyId, CustomerId));
        }
        public List<CustomerSnapshot> GetAllCustomerNote()
        {
            return _CustomerSnapshotDataAccess.GetAll();
        }
        public CustomerSnapshot GetSnapshotById(int value)
        {
            var result = _CustomerSnapshotDataAccess.Get(value);
            return result;
        }
        public CustomerSnapshot GetById(int value)
        {
            return _CustomerSnapshotDataAccess.Get(value);
        }
        public bool UpdateSnapshot(CustomerSnapshot cs)
        {
            return _CustomerSnapshotDataAccess.Update(cs) > 0;
        }
        public bool InsertSnapshot(CustomerSnapshot cs)
        {
            return _CustomerSnapshotDataAccess.Insert(cs)>0;
        }
        public bool PushSnapshot(CustomerSnapshot cs, string InvoiceId)
        {
            return _CustomerSnapshotDataAccess.Push(cs, InvoiceId) > 0;
        }
        public CustomerSnapshot GetLeadJoinDateByCustomerIdCompanyIdandCreatedDateKey(Guid CustomerId,Guid CompanyId)
        {
            return _CustomerSnapshotDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and CompanyId = '{1}' and Description ='CreatedDate' ", CustomerId, CompanyId)).FirstOrDefault();
        }
        public List<CustomerSnapshot> GetLeadNoteByCustomerIdCompanyIdandLeadNoteKey(Guid CustomerId, Guid CompanyId)
        {
            return _CustomerSnapshotDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and CompanyId = '{1}' and Description like '%LeadNote%' ", CustomerId, CompanyId)).ToList();
        }
        public List<CustomerSnapshot> GetLeadFollowUpsByCustomerIdCompanyIdandLeadFollowUpKey(Guid CustomerId, Guid CompanyId)
        {
            return _CustomerSnapshotDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and CompanyId = '{1}' and Description like '%LeadFollowUp%' ", CustomerId, CompanyId)).ToList();
        }
        public List<CustomerSnapshot> GetLeadMailHistoryByCustomerIdCompanyIdandEmailUpKey(Guid CustomerId, Guid CompanyId)
        {
            return _CustomerSnapshotDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and CompanyId = '{1}' and Description like '%Email%' ", CustomerId, CompanyId)).ToList();
        }
        public CustomerSnapshot GetCustomerFromLeadConvartedHistoryByCustomerIdAndConvertLeadToCustomer(Guid CustomerId, Guid CompanyId)
        {
            return _CustomerSnapshotDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and CompanyId = '{1}' and Description = 'ConvertLeadToCustomer'", CustomerId, CompanyId)).FirstOrDefault();
        }
        public CustomerSnapshot GetCustomerCreatedHistoryByCustomerIdAndCustomerCreatedHistoryKey(Guid CustomerId, Guid CompanyId)
        {
            return _CustomerSnapshotDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and CompanyId = '{1}' and Type = 'CustomerCreatedHistory'", CustomerId, CompanyId)).FirstOrDefault();
        }
        public List<CustomerSnapshot> GetCustomerInvoiceHistoryByCustomerIdAndInvoiceCreatedKey(Guid CustomerId, Guid CompanyId)
        {
            return _CustomerSnapshotDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and CompanyId = '{1}' and Type = 'InvoiceCreated' order by Logdate desc", CustomerId, CompanyId)).ToList();
        }
        public List<CustomerSnapshot> GetCustomerEstimateHistoryByCustomerIdAndEstimateCreatedKey(Guid CustomerId, Guid CompanyId)
        {
            return _CustomerSnapshotDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and CompanyId = '{1}' and Type = 'EstimateCreateHistory' order by Logdate desc", CustomerId, CompanyId)).ToList();
        }
        public List<CustomerSnapshot> GetCustomerPaymentHistoryByCustomerIdAndEstimateCreatedKey(Guid CustomerId, Guid CompanyId)
        {
            return _CustomerSnapshotDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and CompanyId = '{1}' and Type = 'InvoicePaymentHistory' order by Logdate desc", CustomerId, CompanyId)).ToList();
        }
        public List<CustomerSnapshot> GetCustomerServiceOrderHistoryByCustomerIdAndServiceOrderCreatedKey(Guid CustomerId, Guid CompanyId)
        {
            return _CustomerSnapshotDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and CompanyId = '{1}' and Type ='ServiceOrderCreated' order by Logdate desc", CustomerId, CompanyId)).ToList();
        }

        public List<CustomerSnapshot> GetCustomerWorkOrderHistoryByCustomerIdAndWorkOrderCreatedKey(Guid CustomerId, Guid CompanyId)
        {
            return _CustomerSnapshotDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and CompanyId = '{1}' and Type = 'WorkOrderCreated' order by Logdate desc", CustomerId, CompanyId)).ToList();
        }

        public List<CustomerSnapshot> GetAllCustomerSendEmailHistoryByCompanyIdAndCustomerIdAndType(Guid customerid, Guid companyid)
        {
            return _CustomerSnapshotDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and CompanyId = '{1}' and Type = 'CustomerMailHistory' order by Logdate desc", customerid, companyid)).ToList();
        }

        public List<CustomerSnapshot> GetCustomerSnapshotDetail(string des)
        {
            DataTable dt = _CustomerSnapshotDataAccess.GetCustomerSnapshotDetail(des);
            List<CustomerSnapshot> Responsiblelist = new List<CustomerSnapshot>();
            Responsiblelist = (from DataRow dr in dt.Rows
                               select new CustomerSnapshot()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   CustomerId = (Guid)dr["CustomerId"],
                                   CompanyId = (Guid)dr["CompanyId"],
                                   Description = dr["Description"].ToString(),
                                   Logdate = dr["Logdate"] != DBNull.Value ? Convert.ToDateTime(dr["Logdate"]) : new DateTime(),
                                   Updatedby = dr["Updatedby"].ToString(),
                                   Type = dr["Type"].ToString()
                               }).ToList();
            return Responsiblelist;
        }
    }
}
