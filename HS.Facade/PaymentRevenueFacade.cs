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
    public class PaymentRevenueFacade : BaseFacade
    {
        public PaymentRevenueFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        PaymentRevenueDataAccess _PaymentRevenueDataAccess
        {
            get
            {
                return (PaymentRevenueDataAccess)_ClientContext[typeof(PaymentRevenueDataAccess)];
            }
        }

        public long InsertWorkOrderPaymentRevenue(PaymentRevenue pr)
        {
            return _PaymentRevenueDataAccess.Insert(pr);
        }

        public bool UpdateWorkOrderPaymentRevenue(PaymentRevenue pr)
        {
            return _PaymentRevenueDataAccess.Update(pr) > 0;
        }

        public PaymentRevenue GetWorkOrderPaymentRevenueByWorkId(int value)
        {
            return _PaymentRevenueDataAccess.GetByQuery(string.Format("WorkOrder = '{0}'", value)).FirstOrDefault();
        }
    }
}
