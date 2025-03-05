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
    public class CustomerBillFacade : BaseFacade
    {
        public CustomerBillFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        CustomerBillDataAccess _CustomerBillDataAccess
        {
            get
            {
                return (CustomerBillDataAccess)_ClientContext[typeof(CustomerBillDataAccess)];
            }
        }
        BillFileDataAccess _BillFileDataAccess
        {
            get
            {
                return (BillFileDataAccess)_ClientContext[typeof(BillFileDataAccess)];
            }
        }
        public List<CustomerBill> GetAllCustomerBillByCustomerIdAndCompanyId(Guid customerID, Guid companyID)
        {
            DataTable dt = _CustomerBillDataAccess.GetAllCustomerBillByCustomerIdAndCompanyId(customerID, companyID);
            List<CustomerBill> CustomerBillList = new List<CustomerBill>();
            CustomerBillList = (from DataRow dr in dt.Rows
                                       select new CustomerBill()
                                       {
                                           Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                           BillNo = dr["BillNo"].ToString(),
                                           CompanyId = (Guid)dr["CompanyId"],
                                           CustomerId = (Guid)dr["CustomerId"],
                                           Type = dr["Type"].ToString(),
                                           Amount = dr["Amount"] != DBNull.Value ? Convert.ToInt32(dr["Amount"]) : 0,
                                           PaymentMethod = dr["PaymentMethod"].ToString(),
                                           PaymentStatus = dr["PaymentStatus"].ToString(),
                                           PaymentDate = dr["PaymentDate"] != DBNull.Value ? Convert.ToDateTime(dr["PaymentDate"]) : new DateTime(),
                                           PaymentDueDate = dr["PaymentDueDate"] != DBNull.Value ? Convert.ToDateTime(dr["PaymentDueDate"]) : new DateTime(),
                                           BillCycle = dr["BillCycle"].ToString(),
                                           Notes = dr["Notes"].ToString(),
                                           UpdatedBy = dr["UpdatedBy"].ToString(),
                                           UpdatedDate = dr["UpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["UpdatedDate"]) : new DateTime(),
                                           
                                       }).ToList();
            return CustomerBillList;
            //return _CustomerBillDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and CompanyId = '{1}'", customerID, companyID));
        }
        public CustomerBill GetAllBillByid(int value)
        {
            var result = _CustomerBillDataAccess.Get(value);
            return result;
        }
        public CustomerBill GetById(int value)
        {
            return _CustomerBillDataAccess.Get(value);
        }
        public bool UpdateCustomerBill(CustomerBill cb)
        {
            return _CustomerBillDataAccess.Update(cb) > 0;
        }
        public long InsertCustomerBill(CustomerBill cb)
        {
            return _CustomerBillDataAccess.Insert(cb);
        }
        public int InsertInitCustomerBill(CustomerBill objCustomerBill)
        {
            return (int)_CustomerBillDataAccess.Insert(objCustomerBill);
        }
        public bool UpdateCustomerBillInfo(CustomerBill objCustomerBill)
        {
            return _CustomerBillDataAccess.Update(objCustomerBill) > 0;
        }

        public BillFile GetBillFileById(int value)
        {
            return _BillFileDataAccess.Get(value);
        }
    }
}
