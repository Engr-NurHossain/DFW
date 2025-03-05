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
    public class MmrFacade : BaseFacade
    {
        public MmrFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        MMRDataAccess _MMRDataAccess
        {
            get
            {
                return (MMRDataAccess)_ClientContext[typeof(MMRDataAccess)];
            }
        }

        public MMR GetById(int value)
        {
            return _MMRDataAccess.Get(value);
        }
        public List<MMR> GetAllMMR()
        {
            return _MMRDataAccess.GetAll();
        }
        public List<MMR> GetAllMMRByCompanyId(Guid companyId)
        {
            return _MMRDataAccess.GetByQuery(string.Format(" CompanyId = '{0}'", companyId));
        }
        public bool UpdateMMR(MMR eq)
        {
            return _MMRDataAccess.Update(eq) > 0;
        }

        public long InsertMMR(MMR eq)
        {
            return _MMRDataAccess.Insert(eq);
        }
        public MMR GetAllMMRById(int value)
        {
            return _MMRDataAccess.Get(value);
        }
        public bool DeleteMMR(int MMRId)
        {
            return _MMRDataAccess.Delete(MMRId) > 0;
        }
    }
}
