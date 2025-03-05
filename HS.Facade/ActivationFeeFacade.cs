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
    public class ActivationFeeFacade : BaseFacade
    {
        public ActivationFeeFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        ActivationFeeDataAccess _ActivationFeeDataAccess
        {
            get
            {
                return (ActivationFeeDataAccess)_ClientContext[typeof(ActivationFeeDataAccess)];
            }
        }
        public long InsertActivationFee(ActivationFee eq)
        {
            return _ActivationFeeDataAccess.Insert(eq);
        }
        public bool UpdateActivationFee(ActivationFee eq)
        {
            return _ActivationFeeDataAccess.Update(eq) > 0;
        }
        public bool DeleteActivationFee(int Id)
        {
            return _ActivationFeeDataAccess.Delete(Id) > 0;
        }
        public ActivationFee GetById(int value)
        {
            return _ActivationFeeDataAccess.Get(value);
        }
        public List<ActivationFee> GetAllActivationFeeByCompanyId(Guid companyId)
        {
            return _ActivationFeeDataAccess.GetByQuery(string.Format(" CompanyId = '{0}'", companyId));
        }
        public ActivationFee GetActivationFeeByCompanyId(Guid CompanyId)
        {
            return _ActivationFeeDataAccess.GetByQuery(string.Format("CompanyId = '{0}' Order By Id Desc", CompanyId)).FirstOrDefault();
        }
    }
}
