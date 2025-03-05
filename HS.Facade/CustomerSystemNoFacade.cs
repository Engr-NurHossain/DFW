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
    public class CustomerSystemNoFacade:BaseFacade
    {
        public CustomerSystemNoFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        public CustomerSystemNoFacade()
        {

        }
        CustomerSystemNoDataAccess _CustomerSystemNoDataAccess
        {
            get
            {
                return (CustomerSystemNoDataAccess)_ClientContext[typeof(CustomerSystemNoDataAccess)];
            }
        }
        CustomerNoPrefixDataAccess _CustomerNoPrefixDataAccess
        {
            get
            {
                return (CustomerNoPrefixDataAccess)_ClientContext[typeof(CustomerNoPrefixDataAccess)];
            }
        }
        public List<CustomerSystemNo> GetAllCustomerSystemNoByCompanyId(Guid CompanyId)
        {
            return _CustomerSystemNoDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", CompanyId));
        }
        public List<CustomerSystemNo> GetAllReservedCustomerSystemNoByCustomerId(int CustomerId)
        {
            return _CustomerSystemNoDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and IsReserved = 1", CustomerId));
        }
        public List<CustomerSystemNo> GetAllOpenCustomerSystemNoByCompanyId(Guid CompanyId)
        {
            return _CustomerSystemNoDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and IsUsed=1", CompanyId));
        }
        public List<CustomerSystemNo> GetAllOpenCustomerSystemNoByCompanyIdandPlatformPrifix(Guid CompanyId, string PlatformPrifix)
        {

            DataTable dt = _CustomerSystemNoDataAccess.GetAllOpenCustomerSystemNoByCompanyIdandPlatform(CompanyId, PlatformPrifix);

            List<CustomerSystemNo> CustomerList = new List<CustomerSystemNo>();
            CustomerList = (from DataRow dr in dt.Rows
                            select new CustomerSystemNo()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                CompanyId = (Guid)dr["CompanyId"],
                                CustomerNo = dr["CustomerNo"].ToString(),
                                IsUsed = Convert.ToBoolean(dr["IsUsed"]),
                                IsReserved = Convert.ToBoolean(dr["IsReserved"]),
                                GenerateDate = dr["GenerateDate"] != DBNull.Value ? Convert.ToDateTime(dr["GenerateDate"]) : new DateTime(),
                                ReserveDate = dr["ReserveDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReserveDate"]) : new DateTime(),
                                UsedDate = dr["UsedDate"] != DBNull.Value ? Convert.ToDateTime(dr["UsedDate"]) : new DateTime(),
                                CustomerId = dr["CustomerId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerId"]) : 0
                            }).ToList();

            return CustomerList;
        }

      
        public long InsertCustomerSystemNo(CustomerSystemNo CustomerSystemNoObj)
        {
            return _CustomerSystemNoDataAccess.Insert(CustomerSystemNoObj);
        }
        public long InsertCustomerSystemNoCheck(CustomerSystemNo CustomerSystemNoObj)
        {
            return _CustomerSystemNoDataAccess.InsertAndCheck(CustomerSystemNoObj);
        }

        public CustomerSystemNo GetCustomerSystemNoObjectByNumberAndCompanyId(string CustomerNo,Guid CompanyId)
        {
            return _CustomerSystemNoDataAccess.GetByQuery(string.Format("CustomerNo='{0}' AND CompanyId = '{1}'", CustomerNo, CompanyId)).FirstOrDefault();
        }
        public bool UpdateCustomerSystemNo(CustomerSystemNo customerSystemNo)
        {
            return _CustomerSystemNoDataAccess.Update(customerSystemNo) > 0;
        }

        public CustomerSystemNo GetCustomerSystemNoObjectByCustomerIdAndCompanyId(int cusID,Guid CompanyId)
        {
            return _CustomerSystemNoDataAccess.GetByQuery(string.Format("CompanyId = '{0}' AND CustomerId='{1}' ", CompanyId, cusID)).FirstOrDefault();
        }
        public CustomerSystemNo GetCusSysNoByCustomerIdAndSysNo(int CustomerId,string SystmeNo)
        {
            return _CustomerSystemNoDataAccess.GetByQuery(string.Format("CustomerNO = '{0}' AND CustomerId='{1}' ", SystmeNo, CustomerId)).FirstOrDefault();
        }
        public CustomerSystemNo GetCusSysNoByCustomerNo(string SystmeNo)
        {
            return _CustomerSystemNoDataAccess.GetByQuery(string.Format("CustomerNO = '{0}'", SystmeNo)).FirstOrDefault();
        }
        public CustomerSystemNo GetSystemNoById(int value)
        {
            return _CustomerSystemNoDataAccess.Get(value);
        }

        public void DeleteCustomerSystemNo(int id)
        {
            _CustomerSystemNoDataAccess.Delete(id);
        }

        public CustomerSystemNumberWithModel GetCustomerSystemNoListByCompanyIdAndPaging(Guid comid, int pageno, int pagesize, string Search,string filter, string prefix)
        {
            DataSet ds = _CustomerSystemNoDataAccess.GetCustomerSystemNoListByCompanyIdAndPaging(comid, pageno, pagesize, Search,filter, prefix);

            List<CustomerSystemNo> CustomerList = new List<CustomerSystemNo>();
            CustomerList = (from DataRow dr in ds.Tables[0].Rows
                            select new CustomerSystemNo()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                CompanyId = (Guid)dr["CompanyId"],
                                CustomerNo = dr["CustomerNo"].ToString(),
                                CustomerName = dr["DisplayName"].ToString(),
                                IsUsed = Convert.ToBoolean(dr["IsUsed"]),
                                IsReserved = Convert.ToBoolean(dr["IsReserved"]),
                                GenerateDate = dr["GenerateDate"] != DBNull.Value ? Convert.ToDateTime(dr["GenerateDate"]) : new DateTime(),
                                ReserveDate = dr["ReserveDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReserveDate"]) : new DateTime(),
                                UsedDate = dr["UsedDate"] != DBNull.Value ? Convert.ToDateTime(dr["UsedDate"]) : new DateTime(),
                                CustomerId = dr["CustomerId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerId"]) : 0
                            }).ToList();

            TotalCustomerSystemNoCount TotalCustomerSystemNoCount = new TotalCustomerSystemNoCount();
            TotalCustomerSystemNoCount = (from DataRow dr in ds.Tables[1].Rows
                             select new TotalCustomerSystemNoCount()
                             {
                                 Counter = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                             }).FirstOrDefault();

            var totalCount = ds != null && ds.Tables.Count > 1 ? ds.Tables[1] : null;


            CustomerSystemNumberWithModel CustomerSystemNumberWithModel = new CustomerSystemNumberWithModel();
            CustomerSystemNumberWithModel.ListCustomerSystemNo = CustomerList;
            CustomerSystemNumberWithModel.TotalCustomerSystemNoCount = TotalCustomerSystemNoCount;

            return CustomerSystemNumberWithModel;
        }
        public List<CustomerNoPrefix> GetAllNumberPrefixByCompanyId(Guid comid)
        {
            return _CustomerNoPrefixDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", comid)).ToList();
        }
        public List<CustomerNoPrefix> GetNumberPrefixByCompanyIdAndPrifix(Guid comid, string prifix)
        {
            return _CustomerNoPrefixDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and Name = '{1}'", comid, prifix)).ToList();
        }
        public CustomerNoPrefix GetAllNumberPrefixByCentralstationName(Guid comid,string PlatformName)
        {
            return _CustomerNoPrefixDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and CentralstationName='{1}'", comid, PlatformName)).ToList().FirstOrDefault();

        }
        public CustomerNoPrefix GetNumberPrefixById(int value)
        {
            return _CustomerNoPrefixDataAccess.Get(value);
        }
    

    }
}
