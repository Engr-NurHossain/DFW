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
   public class ServiceAreaFacade : BaseFacade
    {
        public ServiceAreaFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        ServiceAreaZipcodeDataAccess _ServiceAreaZipcodeDataAccess
        {
            get
            {
                return (ServiceAreaZipcodeDataAccess)_ClientContext[typeof(ServiceAreaZipcodeDataAccess)];
            }
        }
        public long InsertServiceZipCode(ServiceAreaZipcode sg)
        {
            return _ServiceAreaZipcodeDataAccess.Insert(sg);
        }
        public bool UpdateServiceZipCode(ServiceAreaZipcode eq)
        {
            return _ServiceAreaZipcodeDataAccess.Update(eq) > 0;
        }
        public bool DeleteServiceZipCode(int Id)
        {
            return _ServiceAreaZipcodeDataAccess.Delete(Id) > 0;
        }
        public List<ServiceAreaZipcode> GetAllZip()
        {
            return _ServiceAreaZipcodeDataAccess.GetAll();
        }
        public List<ServiceAreaZipcode> GetAllZipCode(int PageNumber, int UnitPerPage, string SearchText)
        {
            return _ServiceAreaZipcodeDataAccess.GetAllZipcode(PageNumber, UnitPerPage, SearchText);
        }
        public ServiceAreaZipcode GetById(int value)
        {
            return _ServiceAreaZipcodeDataAccess.Get(value);
        }
      
    }
}
