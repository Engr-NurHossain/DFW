using HS.DataAccess;
using HS.Entities;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Facade
{
    public class PaymentInfoCustomerFacade : BaseFacade
    {
        public PaymentInfoCustomerFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }

        PaymentInfoCustomerDataAccess _PaymentInfoCustomerDataAccess
        {
            get
            {
                return (PaymentInfoCustomerDataAccess)_ClientContext[typeof(PaymentInfoCustomerDataAccess)];
            }
        }
        PaymentInfoCustomerDraftDataAccess _PaymentInfoCustomerDraftDataAccess
        {
            get
            {
                return (PaymentInfoCustomerDraftDataAccess)_ClientContext[typeof(PaymentInfoCustomerDraftDataAccess)];
            }
        }
        PaymentProfileCustomerDataAccess _PaymentProfileCustomerDataAccess
        {
            get
            {
                return (PaymentProfileCustomerDataAccess)_ClientContext[typeof(PaymentProfileCustomerDataAccess)];
            }
        }
        public long InsertPaymentInfoCustomer(PaymentInfoCustomer pic)
        {
            return _PaymentInfoCustomerDataAccess.Insert(pic);
        }
        public long InsertPaymentInfoCustomerDraft(PaymentInfoCustomerDraft pic)
        {
            return _PaymentInfoCustomerDraftDataAccess.Insert(pic);
        }
        public bool UpdatePaymentInfoCustomer(PaymentInfoCustomer val)
        {
            return _PaymentInfoCustomerDataAccess.Update(val) > 0;
        }
        public PaymentInfoCustomer GetPayCustomerById(int value)
        {
            return _PaymentInfoCustomerDataAccess.Get(value);
        }
        public PaymentInfoCustomer GetPaymentInfoCustomerByCustomerId(Guid cusid)
        {
            return _PaymentInfoCustomerDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", cusid)).FirstOrDefault();
        }
        public PaymentInfoCustomer GetPaymentInfoCustomerByCustomerIdAndPayForService(Guid cusid)
        {
            return _PaymentInfoCustomerDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and Payfor = 'Service'", cusid)).FirstOrDefault();
        }
        public PaymentInfoCustomer GetPaymentInfoCustomerByCustomerIdAndPayFor(Guid cusid,string PayFor ="MMR")
        {
            return _PaymentInfoCustomerDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and Payfor = '{1}'", cusid, PayFor)).FirstOrDefault();
        }
        public PaymentInfoCustomer GetPaymentInfoCustomerServiceByCustomerId(Guid cusid)
        {
            return _PaymentInfoCustomerDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and Payfor = 'Service'", cusid)).FirstOrDefault();
        }
        public PaymentInfoCustomer GetPaymentInfoCustomerByCustomerIdAndPaymentInfoId(Guid cusId,int payId)
        {
            return _PaymentInfoCustomerDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and PaymentInfoId = '{1}'",cusId, payId)).FirstOrDefault();
        }
        public PaymentInfoCustomerDraft GetPaymentInfoCustomerDraftByCustomerId(Guid cusid)
        {
            return _PaymentInfoCustomerDraftDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", cusid)).OrderByDescending(x => x.Id).FirstOrDefault();
        }
        public bool DeletePaymentInfoCustomerById(int id)
        {
            return _PaymentInfoCustomerDataAccess.DeletePaymentInfoCustomerById(id);
        }

        public bool DeleteByCustomerIdCompanyIdAndPaymentInfoId(Guid customerId, Guid companyId, int paymentInfoId,int paymentinfoCusId)
        {
            return _PaymentInfoCustomerDataAccess.DeleteByCustomerIdCompanyIdAndPaymentInfoId(customerId, companyId, paymentInfoId, paymentinfoCusId);
        }

        public PaymentInfoCustomer GetByCustomerIdAndPayfor(Guid customerId, string payfor)
        {
            return _PaymentInfoCustomerDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and Payfor = '{1}'",customerId,payfor)).FirstOrDefault();
        }

        public List<PaymentInfoCustomer> GetAllPaymentInfoCustomerByCustomerId(Guid customerId)
        {
            return _PaymentInfoCustomerDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerId)).ToList();
        }

        public List<PaymentProfileCustomer> GetAllPaymentProfileCustomerByCustomerId(Guid customerid)
        {
            return _PaymentProfileCustomerDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerid)).ToList();
        }

        public long InsertPaymentProfileCustomer(PaymentProfileCustomer ppc)
        {
            return _PaymentProfileCustomerDataAccess.Insert(ppc);
        }

        public PaymentProfileCustomer PaymentProfileCustomerById(int paymentProfileId)
        {
            return _PaymentProfileCustomerDataAccess.Get(paymentProfileId);
        }

        public List<PaymentProfileCustomer> GetAllPaymentProfileCustomerByCustomerIdAndProfileType(Guid customerid, string type)
        {
            return _PaymentProfileCustomerDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and [Type] like '{1}%'", customerid, type)).ToList();
        }

        public PaymentProfileCustomer GetPaymentProfileByPaymentInfoId(int id)
        {
            return _PaymentProfileCustomerDataAccess.GetByQuery(string.Format("PaymentInfoId = {0}", id)).FirstOrDefault();
        }

        public List<PaymentProfileCustomer> GetPaymentProfileListByPaymentInfoId(int id)
        {
            return _PaymentProfileCustomerDataAccess.GetByQuery(string.Format("PaymentInfoId = {0}", id));
        }

        public PaymentInfoCustomer GetPaymentInfoCustomerByInvoiceId(string id)
        {
            return _PaymentInfoCustomerDataAccess.GetByQuery(string.Format("InvoiceId = '{0}' and IsPaid = 1", id)).FirstOrDefault();
        }

        public PaymentProfileCustomer GetPaymentProfileByTypeAndCustomerId(string type, Guid customerid)
        {
            return _PaymentProfileCustomerDataAccess.GetByQuery(string.Format("[Type] = '{0}' and CustomerId = '{1}'", type.Replace("'","''"), customerid)).FirstOrDefault();
        }

        public bool DeletePaymentProfileCustomerById(int id)
        {
            return _PaymentProfileCustomerDataAccess.Delete(id) > 0;
        }

        public List<PaymentInfoCustomer> GetPaymentInfoCustomerByPaymentInfoId(int PaymentInfoId)
        {
            return _PaymentInfoCustomerDataAccess.GetByQuery(string.Format("PaymentInfoId = '{0}'", PaymentInfoId));
        }

        public bool UpdatePaymentProfileCustomer(PaymentProfileCustomer item)
        {
            return _PaymentProfileCustomerDataAccess.Update(item)>0;
        }

        public bool DeletePaymentInfoCustomerByPayForAndCustomerId(string Payfor,Guid CustomerId)
        {
            return _PaymentInfoCustomerDataAccess.DeletePaymentInfoCustomerByPayForAndCustomerId(Payfor, CustomerId);
        }

        public PaymentProfileCustomer GetPaymentProfileCustomerById(int paymentProfileId)
        {
            return _PaymentProfileCustomerDataAccess.Get(paymentProfileId);
        }
    }
}
