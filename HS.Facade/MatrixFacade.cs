using HS.DataAccess;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HS.Entities;

namespace HS.Facade
{
    public class MatrixFacade:BaseFacade
    {
        public MatrixFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        public SalesMatrixDataAccess _SalesMatrixDataAccess
        {
            get
            {
                return (SalesMatrixDataAccess)_ClientContext[typeof(SalesMatrixDataAccess)];
            }
        }

        public List<SalesMatrix> GetAllSalesMatrixByCompanyId(Guid CompanyId)
        {
            return _SalesMatrixDataAccess.GetByQuery(string.Format("CompanyId = '{0}'",CompanyId));
        }
        public List<SalesMatrix> GetAllSalesMatrix()
        {
            return _SalesMatrixDataAccess.GetByQuery(string.Format(""));
        }

     

        public bool UpdateSalesMatrix(SalesMatrix sm)
        {
            return _SalesMatrixDataAccess.Update(sm)>0;
        }

        public long InsertSalesMatrix(SalesMatrix sm)
        {
            return _SalesMatrixDataAccess.Insert(sm);
        }

        public SalesMatrix GetSalesMatrixById(int Id)
        {
            return _SalesMatrixDataAccess.Get(Id);
        }

        public void DeleteSalesMatrix(int id)
        {
            _SalesMatrixDataAccess.Delete(id);
        }
    }
}
