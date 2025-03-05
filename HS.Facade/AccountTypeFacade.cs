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
    public class AccountTypeFacade : BaseFacade
    {
        public AccountTypeFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        AccountTypeDataAccess _AccountTypeDataAccess
        {
            get
            {
                return (AccountTypeDataAccess)_ClientContext[typeof(AccountTypeDataAccess)];
            }
        }

        
        public List<AccountType> GetAccountTypeBySearchKey(string key)
        {
            return _AccountTypeDataAccess.GetByQuery(string.Format("Name like '%{0}%'", key));
        }

        public List<AccountType> GetAccountTypeByAccountTypeId(int id)
        {
            return _AccountTypeDataAccess.GetByQuery(string.Format("Id = '{0}'", id));
        }
    }
}
