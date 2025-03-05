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
    public class AccountHolderFacade: BaseFacade
    {
        public AccountHolderFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        AccountHolderDataAccess _AccountHolderDataAccess
        {
            get
            {
                return (AccountHolderDataAccess)_ClientContext[typeof(AccountHolderDataAccess)];
            }
        }
        public long InsertAccountHolder(AccountHolder eq)
        {
            return _AccountHolderDataAccess.Insert(eq);
        }
        public bool UpdateAccountHolder(AccountHolder eq)
        {
            return _AccountHolderDataAccess.Update(eq) > 0;
        }
        public bool DeleteAccountHolder(int Id)
        {
            return _AccountHolderDataAccess.Delete(Id) > 0;
        }
        public AccountHolder GetById(int value)
        {
            return _AccountHolderDataAccess.Get(value);
        }
        public List<AccountHolder> GetAllAccountHolderByCompanyId(Guid companyId)
        {
            return _AccountHolderDataAccess.GetByQuery(string.Format(" CompanyId = '{0}'", companyId));
        }
    }
}
