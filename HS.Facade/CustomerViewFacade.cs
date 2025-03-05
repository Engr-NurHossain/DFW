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
    public class CustomerViewFacade:BaseFacade
    {
        public CustomerViewFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        CustomerViewDataAccess _CustomerViewDataAccess
        {
            get
            {
                return (CustomerViewDataAccess)_ClientContext[typeof(CustomerViewDataAccess)];
            }
        }
        public CustomerView GetById(int value)
        {
            return _CustomerViewDataAccess.Get(value);
        }
        public long InsertViewList(CustomerView cv)
        {
            return _CustomerViewDataAccess.Insert(cv);
        }
        public List<CustomerView> GetCustomerViewListByCompanyIdandCustomerId(Guid companyID,bool recent,string UserName)
        {
            DataTable dt = _CustomerViewDataAccess.GetCustomerViewListByCompanyIdandCustomerId(companyID,recent, UserName);
            List<CustomerView> CusviewList = new List<CustomerView>();
            if (dt != null)
            {
                CusviewList = (from DataRow dr in dt.Rows
                    select new CustomerView()
                    { 
                        CustomerViewName = dr["CustomerViewName"].ToString(),
                        CustomerViewid = dr["CustomerViewid"] != DBNull.Value ? Convert.ToInt32(dr["CustomerViewid"]) : 0,
                        LastVistited = dr["LastVistited"] != DBNull.Value ? Convert.ToDateTime(dr["LastVistited"]) : new DateTime(),
                        CustomerViewBussiness = dr["CustomerViewBussiness"].ToString()
                    }).ToList();
            }
            return CusviewList;
            //return _CustomerViewDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", companyID));
        }
        public CustomerDetailTimestamp GetCustomerTimestampByCustomerId(Guid customerId, int PageNo, int PageSize)
        {
            DataSet dt = _CustomerViewDataAccess.GetCustomerTimestampByCustomerId(customerId,PageNo,PageSize);

            List<CustomerDetailTimestamp> TimestampList = new List<CustomerDetailTimestamp>();
            CustomerCount count = new CustomerCount();
            TimestampList = (from DataRow dr in dt.Tables[0].Rows
                           select new CustomerDetailTimestamp()
                           {
                               Name = dr["LastVisitedBy"].ToString(),
                               VisitedDate = dr["LastVistited"] != DBNull.Value ? Convert.ToDateTime(dr["LastVistited"]) : new DateTime()
                           }).ToList();
            count = (from DataRow dr in dt.Tables[1].Rows
                     select new CustomerCount()
                     {
                         TotalCustomer = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                     }).FirstOrDefault();

            CustomerDetailTimestamp CustomerDetailTimestamp = new CustomerDetailTimestamp();
            CustomerDetailTimestamp.CustomerDetailTimestampList = TimestampList;
            CustomerDetailTimestamp.Count = count;
            return CustomerDetailTimestamp;
        }
    }
}
