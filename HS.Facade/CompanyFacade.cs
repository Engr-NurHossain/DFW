using HS.DataAccess;
using HS.Entities;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HS.Facade
{
    public class CompanyFacade : BaseFacade
    {
        CompanyDataAccess _CompanyDataAccess;
        public CompanyFacade(ClientContext clientContext): base(clientContext)
        {
            _CompanyDataAccess = new CompanyDataAccess();
        }

        public CompanyFacade()
        {
            _CompanyDataAccess = new CompanyDataAccess();
        }

        public CompanyFacade(string constr)
        {
            _CompanyDataAccess = new CompanyDataAccess(constr);
        }

        SupplierDataAccess _SupplierDataAccess
        {
            get
            {
                return (SupplierDataAccess)_ClientContext[typeof(SupplierDataAccess)];
            }
        } 

        public List<Company> GetCompanyByUsername(string username)
        {
            return _CompanyDataAccess.GetByQuery(string.Format(" UserName ='{0}'", username));
        }
        public Company GetCompanyByComapnyId(Guid companyId)
        {
            return _CompanyDataAccess.GetByQuery(string.Format(" CompanyId ='{0}'", companyId)).FirstOrDefault();
        }

        public List<Company> GetAllCompanyByCompanyId(Guid companyId)
        {
            return _CompanyDataAccess.GetByQuery(string.Format(" CompanyId = '{0}'", companyId));
        }
        public Company GetById(int value)
        {
            return _CompanyDataAccess.Get(value);
        }
        public long InsertCompany(Company eq)
        {
            return _CompanyDataAccess.Insert(eq);
        }
        public bool UpdateCompany(Company eq)
        {
            return _CompanyDataAccess.Update(eq) > 0;
        }
        public List<Company> GetAllCompany()
        {
            return _CompanyDataAccess.GetAll();
        }
    }
}
