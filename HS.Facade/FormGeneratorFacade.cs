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
    public class FormGeneratorFacade : BaseFacade
    {
        public FormGeneratorFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        FormGeneratorDataAccess _FormGeneratorDataAccess
        {
            get
            {
                return (FormGeneratorDataAccess)_ClientContext[typeof(FormGeneratorDataAccess)];
            }
        }

        public List<FormGenerator> GetAllFormGeneratorByFormNameAndCompanyId(Guid ComapnyId, string key)
        {
            return _FormGeneratorDataAccess.GetByQuery(string.Format("CompanyId= '{0}' and FormName = '{1}' and IsActive = 1 order by OrderBy asc", ComapnyId, key)).ToList();
        }

        public List<FormGenerator> GetAllFormGeneratorByFormName(Guid ComapnyId, string key)
        {
            return _FormGeneratorDataAccess.GetByQuery(string.Format("CompanyId= '{0}' and FormName = '{1}' order by OrderBy asc", ComapnyId, key)).ToList();
        }

        public bool UpdateFormGenerator(FormGenerator fg)
        {
            return _FormGeneratorDataAccess.Update(fg) > 0;
        }
    }
}
