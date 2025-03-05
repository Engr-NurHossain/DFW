using HS.Framework;
using HS.DataAccess;
using HS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace HS.Facade
{
    public class VehicleFacade : BaseFacade
    {
        public VehicleFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        VehicleDetailDataAccess _VehicleDetailDataAccess
        {
            get
            {
                return (VehicleDetailDataAccess)_ClientContext[typeof(VehicleDetailDataAccess)];
            }
        }
        VehicleRepairLogDataAccess _VehicleRepairLogDataAccess
        {
            get
            {
                return (VehicleRepairLogDataAccess)_ClientContext[typeof(VehicleRepairLogDataAccess)];
            }
        }

        VehicleMileageLogDataAccess _VehicleMileageLogDataAccess
        {
            get
            {
                return (VehicleMileageLogDataAccess)_ClientContext[typeof(VehicleMileageLogDataAccess)];
            }
        }
        EmployeeVehicleDataAccess _EmployeeVehicleDataAccess
        {
            get
            {
                return (EmployeeVehicleDataAccess)_ClientContext[typeof(EmployeeVehicleDataAccess)];
            }
        }
        public List<VehicleMileageLog> GetAllMilageLogByVehicleId(Guid vehicleId)
        {
            return _VehicleMileageLogDataAccess.GetAllMilageLogByVehicleId(vehicleId);
        }
        public VehicleDetail GetVehicleByUserId(Guid userId)
        {
            return _VehicleDetailDataAccess.GetByQuery(string.Format(" UserId ='{0}'", userId)).FirstOrDefault();
        }
        public List<VehicleDetail> GetAllVehicle()
        {
            return _VehicleDetailDataAccess.GetAll();
        }
        public EmployeeVehicle GetEmployeeVehicleByUserId(Guid userId)
        {
            return _EmployeeVehicleDataAccess.GetByQuery(string.Format(" EmployeeId ='{0}'", userId)).FirstOrDefault();
        }
      
        public int InsertVechile(VehicleDetail vehicle)
        {
            return (int)_VehicleDetailDataAccess.Insert(vehicle);
        }
        public bool UpdateVechile(VehicleDetail vechile)
        {
            return _VehicleDetailDataAccess.Update(vechile) > 0;
        }

        public int InsertEmployeeVehicle(EmployeeVehicle vehicle)
        {
            return (int)_EmployeeVehicleDataAccess.Insert(vehicle);
        }
        public bool UpdateEmployeeVehicle(EmployeeVehicle vechile)
        {
            return _EmployeeVehicleDataAccess.Update(vechile) > 0;
        }
        public bool DeleteVechile(int id)
        {
            return _VehicleDetailDataAccess.Delete(id) > 0;
        }
        public List<VehicleDetail> GetVehicleListBySearchKeyAndCompanyId(string key, int MaxLoad)
        {
            DataTable dt = _VehicleDetailDataAccess.GetVehicleListBySearchKeyAndCompanyId(key, MaxLoad);
            List<VehicleDetail> NoteList = new List<VehicleDetail>();
            NoteList = (from DataRow dr in dt.Rows
                        select new VehicleDetail()
                        {
                            VehicleId = (Guid)dr["VehicleId"],
                            VehicleNo = dr["VehicleNo"].ToString(),
                            Model = dr["Model"].ToString(),
                            Make = dr["Make"].ToString(),
                            Year = dr["Year"].ToString(),
                            DriverName = dr["DriverName"].ToString(),
                            DriverUserId = (Guid)dr["DriverUserId"]
                        }).ToList();
            return NoteList;
        }
        public VehicleDetail GetVechileById(int Id)
        {
            return _VehicleDetailDataAccess.Get(Id);
        }
        public VehicleDetail GetVehicleByVehicleId(Guid vehicleId)
        {
            return _VehicleDetailDataAccess.GetVehicleByVehicleId(vehicleId);
        }
        public List<VehicleDetail> GetAllVehileDetailByOrder(string OrderBy)
        {
            List<VehicleDetail> VechileGroup = _VehicleDetailDataAccess.GetAllVehileDetailByOrder(OrderBy);
            return VechileGroup;
        }

        public int InsertMilageLog(VehicleMileageLog item)
        {
            return (int)_VehicleMileageLogDataAccess.Insert(item);
        }

        public List<VehicleDetail> GetAllVechileDetailByVehicleId(Guid VechileId)
        {
            List<VehicleDetail> VechileGroup = _VehicleDetailDataAccess.GetByQuery(string.Format(" VechileId ='{0}'", VechileId)).ToList();
            return VechileGroup;
        } 
        public int InsertVechileRepair(VehicleRepairLog vehicle)
        {
            return (int) _VehicleRepairLogDataAccess.Insert(vehicle);
        }
        public bool UpdateVechileRepair(VehicleRepairLog vechile)
        {
            return _VehicleRepairLogDataAccess.Update(vechile) > 0;
        }

        public VehicleRepairLog GetVehicleReapairLogById(int value)
        {
            return _VehicleRepairLogDataAccess.Get(value);
        }
        public List<VehicleRepairLog> GetAllRepairLogsbyVehicleId(Guid vehicleId)
        {
            //return _VehicleRepairLogDataAccess.GetByQuery(string.Format(" VehicleId ='{0}'", vehicleId)).ToList();
            return _VehicleRepairLogDataAccess.GetAllRepairLogsbyVehicleId(vehicleId);
        }
        public bool DeleteVechileRepair(int id)
        {
            return _VehicleRepairLogDataAccess.Delete(id) > 0;
        }


        public VehicleRepairLog GetVechileRepairById(int Id)
        {
            return _VehicleRepairLogDataAccess.Get(Id);
        }
        public List<VehicleRepairLog> GetAllVechileRepairDetail(Guid UserId)
        {
            List<VehicleRepairLog> VechileRepairGroup = _VehicleRepairLogDataAccess.GetByQuery(string.Format(" UserId ='{0}'", UserId)).ToList();
            return VechileRepairGroup;
        }

        public List<VehicleDetail> GetAllAvailableVehileDetailByUserId(Guid userId)
        {
            return _VehicleDetailDataAccess.GetAllAvailableVehileDetailByUserId(userId);
        }

        public EmployeeVehicle GetEmployeeVehicleByVehicleId(Guid vehicleId)
        {
            return _EmployeeVehicleDataAccess.GetByQuery(string.Format(" VehicleId ='{0}'", vehicleId)).FirstOrDefault();
        }
        public List<EmployeeVehicle> GetAllEmployeeVehicleByVehicleId(Guid vehicleId)
        {
            DataTable dt = _EmployeeVehicleDataAccess.GetAllEmployeeVehicleByVehicleId(vehicleId);
            List<EmployeeVehicle> empVehicleList = new List<EmployeeVehicle>();
            empVehicleList = (from DataRow dr in dt.Rows
                        select new EmployeeVehicle()
                        {
                            VehicleId = (Guid)dr["VehicleId"],
                          
                            EmployeeName = dr["EmployeeName"].ToString(),
                            EmployeeId = (Guid)dr["EmployeeId"]
                        }).ToList();
            return empVehicleList;

        }
        public bool DeleteEmployeeVehicleByEmployeeId(Guid employeeId)
        {
            return _EmployeeVehicleDataAccess.DeleteEmployeeVehicleByEmployeeId(employeeId);
        }

        public bool DeleteEmployeeVehicleByVehicleId(Guid vehicleid)
        {
            return _EmployeeVehicleDataAccess.DeleteEmployeeVehicleByVehicleId(vehicleid);
        }

        public bool DeleteAllEmployeeVehicleByVehicleId(Guid vehicleId)
        {
            return _EmployeeVehicleDataAccess.DeleteAllEmployeeVehicleByVehicleId(vehicleId);
        }

        public bool DeleteAllVehicleRepairLogByVehicleId(Guid vehicleId)
        {
            return _VehicleRepairLogDataAccess.DeleteAllVehicleRepairLogByVehicleId(vehicleId);
        }

        public bool DeleteAllVehicleMilageLogByvehicleId(Guid vehicleId)
        {
            return _VehicleMileageLogDataAccess.DeleteAllVehicleMilageLogByvehicleId(vehicleId);
        }
    }
}
