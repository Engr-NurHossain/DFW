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
   public class EmployeeComputerFacade : BaseFacade
    {
        public EmployeeComputerFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        EmployeeComputerDataAccess _EmployeeComputerDataAccess
        {
            get
            {
                return (EmployeeComputerDataAccess)_ClientContext[typeof(EmployeeComputerDataAccess)];
            }
        }
    
        public EmployeeComputer GetComputerByUserId(Guid userId)
        {
            return _EmployeeComputerDataAccess.GetByQuery(string.Format(" UserId ='{0}'", userId)).FirstOrDefault();
        }

        public int InsertComputer(EmployeeComputer Computer)
        {
            return (int)_EmployeeComputerDataAccess.Insert(Computer);
        }
        public bool UpdateComputer(EmployeeComputer Computer)
        {
            return _EmployeeComputerDataAccess.Update(Computer) > 0;
        }
        public bool DeleteComputer(int id)
        {
            return _EmployeeComputerDataAccess.Delete(id) > 0;
        }


        public EmployeeComputer GetComputerById(int Id)
        {
            return _EmployeeComputerDataAccess.Get(Id);
        }
        public List<EmployeeComputer> GetAllComputerDetail(Guid ComputerId)
        {
            List<EmployeeComputer> ComputerGroup = _EmployeeComputerDataAccess.GetByQuery(string.Format(" VechileId ='{0}'", ComputerId)).ToList();
            return ComputerGroup;
        }
       
    }
}
