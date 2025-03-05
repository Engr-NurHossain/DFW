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
    public class ServiceFeeFacade : BaseFacade
    {
        public ServiceFeeFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }

        ServiceFeeDataAccess _ServiceFeeDataAccess
        {
            get
            {
                return (ServiceFeeDataAccess)_ClientContext[typeof(ServiceFeeDataAccess)];
            }
        }

        public long InsertServiceFee(ServiceFee eq)
        {
            return _ServiceFeeDataAccess.Insert(eq);
        }
        public bool UpdateServiceFee(ServiceFee eq)
        {
            return _ServiceFeeDataAccess.Update(eq) > 0;
        }
        public bool DeleteServiceFee(int Id)
        {
            return _ServiceFeeDataAccess.Delete(Id) > 0;
        }
        public ServiceFee GetById(int value)
        {
            return _ServiceFeeDataAccess.Get(value);
        }
        public List<ServiceFee> GetAllServiceFeeByCompanyId(Guid companyId)
        {
            return _ServiceFeeDataAccess.GetByQuery(string.Format(" CompanyId = '{0}'", companyId));
        }
    }
}
