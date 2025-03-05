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
    public class FundFacade : BaseFacade
    {
        FundingCompanyDataAccess _FundingCompanyDataAccess = null;
        public FundFacade(ClientContext clientContext)
            : base(clientContext)
        {
            if (_FundingCompanyDataAccess == null)
                _FundingCompanyDataAccess = (FundingCompanyDataAccess)_ClientContext[typeof(FundingCompanyDataAccess)];
        }
        public FundFacade()
        {
            if (_FundingCompanyDataAccess == null)
                _FundingCompanyDataAccess = new FundingCompanyDataAccess();
        }
        FundDataAccess _FundDataAccess
        {
            get
            {
                return (FundDataAccess)_ClientContext[typeof(FundDataAccess)];
            }
        }
        CustomerDataAccess _CustomerDataAccess
        {
            get
            {
                return (CustomerDataAccess)_ClientContext[typeof(CustomerDataAccess)];
            }
        }
        public List<Fund> GetAllFundingByCompanyIdandCustomerId(Guid CustomerId, Guid CompanyId)
        {
            return _FundDataAccess.GetByQuery(string.Format(" CompanyId ='{0}' and CustomerId = '{1}'", CompanyId, CustomerId));
        }
        public Fund GetFundById(int value)
        {
            return _FundDataAccess.Get(value);
        }
        public Fund GetById(int id)
        {
            return _FundDataAccess.Get(id);
        }
        public bool UpdateFunding(Fund fund)
        {
            return _FundDataAccess.Update(fund) > 0;
        }
        public long InsertFunding(Fund fund)
        {
            return _FundDataAccess.Insert(fund);
        }
        public List<Fund> GetAllFund()
        {
            return _FundDataAccess.GetAll();
        }
        public bool DeleteAddIncome(int incomeId)
        {
            return _FundDataAccess.Delete(incomeId) > 0;
        }
        public List<Fund> GetAllRevenueByCustomerIdAndCompanyId(Guid CustomerId, Guid CompanyId)
        {
            return _FundDataAccess.GetByQuery(string.Format(" CompanyId ='{0}' and CustomerId = '{1}'", CompanyId, CustomerId));
        }
        public Fund GetRevenueById(int value)
        {
            return _FundDataAccess.Get(value);
        }
        public List<Fund> GetRevenueByCustomerId(Guid customerId, Guid companyId)
        {
            DataTable dt = _FundDataAccess.GetRevenueByComanyIdAndCustomerId(customerId, companyId);
            List<Fund> RevenueFund = new List<Fund>();
            RevenueFund = (from DataRow dr in dt.Rows
                            select new Fund()
                            {
                                Revenue = dr["Revenue"] != DBNull.Value ? Convert.ToDouble(dr["Revenue"]) : 0
                            }).ToList();
            return RevenueFund;
        }

        public FundingCompany GetFundingCompanyById(int value)
        {
            return _FundingCompanyDataAccess.Get(value);
        }

        public bool UpdateFundingCompany(FundingCompany fc)
        {
            return _FundingCompanyDataAccess.Update(fc) > 0;
        }

        public long InsertFundingCompany(FundingCompany fc)
        {
            return _FundingCompanyDataAccess.Insert(fc);
        }

        public void DeleteFundingCompany(int id)
        {
            _FundingCompanyDataAccess.Delete(id);
        }

        public List<FundingCompany> GetAllFundingCompany()
        {
            return _FundingCompanyDataAccess.GetAll();
        }
    }
}
