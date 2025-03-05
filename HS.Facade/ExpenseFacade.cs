using HS.DataAccess;
using HS.Framework;
using HS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace HS.Facade
{
    public class ExpenseFacade : BaseFacade
    {
        public ExpenseFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        public ExpenseFacade()
        {

        }
        InvoiceDataAccess _InvoiceDataAccess
        {
            get
            {
                return (InvoiceDataAccess)_ClientContext[typeof(InvoiceDataAccess)];
            }
        }
        BillDataAccess _BillDataAccess
        {
            get
            {
                return (BillDataAccess)_ClientContext[typeof(BillDataAccess)];
            }
        }

        public List<Invoice> GetAllVendorBillByEmployeeIdAndCompanyId(Guid EmpId, Guid CmpId)
        {
            return _InvoiceDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and CompanyId = '{1}' and IsBill = 1", EmpId, CmpId)).ToList();
        }

        public ExpenseSummary GetExpenseSummary(Guid CompanyId)
        {
            DataTable dt = _BillDataAccess.GetExpenseSummary(CompanyId);
            List<ExpenseSummary> ExpenseSummary = new List<ExpenseSummary>();
            ExpenseSummary = (from DataRow dr in dt.Rows
                           select new ExpenseSummary()
                           {
                               OverDue = dr["OverDue"] != DBNull.Value ? Convert.ToDouble(dr["OverDue"]) : 0,
                               Paid = dr["Paid"] != DBNull.Value ? Convert.ToDouble(dr["Paid"]) : 0,
                               OpenBill = dr["OpenBill"] != DBNull.Value ? Convert.ToDouble(dr["OpenBill"]) : 0,
                           }).ToList();

            return ExpenseSummary.FirstOrDefault();
        }
    }
}
