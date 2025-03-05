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
    public class CompanyFileFacade : BaseFacade
    {
        public CompanyFileFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }

        CompanyFileDataAccess _CompanyFileDataAccess
        {
            get
            {
                return (CompanyFileDataAccess)_ClientContext[typeof(CompanyFileDataAccess)];
            }
        }

        public List<CompanyFile> GetAllCompanyFile(Guid comid)
        {
            return _CompanyFileDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", comid));
        }

        public long InsertCompanyFile(CompanyFile cf)
        {
            return _CompanyFileDataAccess.Insert(cf);
        }

        public CompanyFile GetById(int value)
        {
            return _CompanyFileDataAccess.Get(value);
        }
        public void DeleteCompanyFile(int id)
        {
            _CompanyFileDataAccess.Delete(id);
        }
    }
}
