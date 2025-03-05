using HS.Entities;
using HS.Framework;
using HS.Web.UI.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HS.Web.UI.Controllers
{
    public class VehicleManagementController : BaseController
    {
        // GET: VehicleManagement
        public ActionResult Index()
        {
            if (!base.SetLayoutCommons())
            {
                return RedirectToAction("Logout", "Login");
            }
            return View();
        }
     
        [Authorize]
        public ActionResult ShowVehicleList()
        {
            return View();
        }
        [Authorize]
        public PartialViewResult ShowVehilceListByOrder(string OrderBy)
        {
            List<VehicleDetail> vehicleList = _Util.Facade.VehicleFacade.GetAllVehileDetailByOrder(OrderBy);
            return PartialView("_ShowVehilceListByOrder",vehicleList);
        }

        public ActionResult VehicleRepairList(Guid VehicleId)
        {
            List<VehicleRepairLog> VehicleRepairLog = _Util.Facade.VehicleFacade.GetAllRepairLogsbyVehicleId(VehicleId);
            ViewBag.VehicleId = VehicleId;
            VehicleDetail vehicleDetail = _Util.Facade.VehicleFacade.GetVehicleByVehicleId(VehicleId);
            ViewBag.VIN = vehicleDetail.VIN;
            ViewBag.VehicleNo = vehicleDetail.VehicleNo;
            return View(VehicleRepairLog);
        }
        public ActionResult VehicleMilageList(Guid VehicleId)
        {
            List<VehicleMileageLog> VehicleMileageLog = _Util.Facade.VehicleFacade.GetAllMilageLogByVehicleId(VehicleId);
            ViewBag.VehicleId = VehicleId;
            VehicleDetail vehicleDetail = _Util.Facade.VehicleFacade.GetVehicleByVehicleId(VehicleId);
            ViewBag.VIN = vehicleDetail.VIN;
            ViewBag.VehicleNo = vehicleDetail.VehicleNo;

            return View(VehicleMileageLog);
        }
        [Authorize]
        [HttpPost]
        public JsonResult SaveVehicle(VehicleDetail vehicle)
        {
            bool result = false;
            string message = "";
            try
            {
                if (vehicle.Id > 0)
                {
                    _Util.Facade.VehicleFacade.UpdateVechile(vehicle);
                }
                else
                {
                    vehicle.VehicleId = Guid.NewGuid();
                    vehicle.TechnicianId = Guid.NewGuid();
                    vehicle.Id =(int) _Util.Facade.VehicleFacade.InsertVechile(vehicle);
                }
                result = true;
                message = "Vechile saved successfully";
            }
            catch (Exception ex)
            {
                result = false;
                message = "Internal Error";
            }
           
            return Json(new { result = result,message= message, id=vehicle.Id});

        }
        [Authorize]
        public ActionResult AddVehicleRepair(int? RepairId)
        {
            VehicleRepairLog log = new VehicleRepairLog();
            log.RepairDate = DateTime.Now.UTCCurrentTime(); ;
            if (RepairId.HasValue && RepairId > 0)
            {
                log = _Util.Facade.VehicleFacade.GetVehicleReapairLogById(RepairId.Value);
            }
            if (log == null)
            {
                log = new VehicleRepairLog();
                log.RepairDate = DateTime.Now.UTCCurrentTime();
               
            }
            return View(log);
        }
        
        [Authorize]
        public ActionResult AddNewVehicle(int? Id)
        {
            VehicleDetail vehicle = new VehicleDetail();
            if (Id.HasValue)
            {
                vehicle = _Util.Facade.VehicleFacade.GetVechileById(Id.Value);
            }
            else
            {
                vehicle = new VehicleDetail();
            }
            
            return View(vehicle);
        }

        [Authorize]
        [HttpGet]
        public ActionResult AddMileageData(Guid? VehicleId)
        {
            if (VehicleId.HasValue && VehicleId.Value != Guid.Empty)
            {
                ViewBag.VehicleId = VehicleId.Value.ToString();
                VehicleDetail vd = _Util.Facade.VehicleFacade.GetVehicleByVehicleId(VehicleId.Value);
                ViewBag.Vin = vd.VIN;
                ViewBag.VehicleNo = vd.VehicleNo;
            }
            return View();
        }
        [Authorize]
        [HttpGet]
        public ActionResult AddRepairLog(Guid?VehicleId)
        {
            if(VehicleId.HasValue && VehicleId.Value != Guid.Empty)
            {
                ViewBag.VehicleId = VehicleId.Value.ToString();
                VehicleDetail vd = _Util.Facade.VehicleFacade.GetVehicleByVehicleId(VehicleId.Value);
                ViewBag.Vin = vd.VIN;
                ViewBag.VehicleNo = vd.VehicleNo;
                ViewBag.DriverName = vd.DriverName;
                if(vd.DriverUserId != Guid.Empty)
                    ViewBag.DriverId = vd.DriverUserId.ToString();
            }
            return View();
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddRepairLog(List<VehicleRepairLog> Model)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            foreach(var item in Model)
            {
                VehicleDetail vd = _Util.Facade.VehicleFacade.GetVehicleByVehicleId(item.VehicleId);
                if (vd != null)
                {
                    item.Driver = vd.DriverUserId;
                    item.CreatedByUid = CurrentUser.UserId;
                    item.CreatedDate = DateTime.Now.UTCCurrentTime();
                    _Util.Facade.VehicleFacade.InsertVechileRepair(item);
                }
            }
            return Json(new { result=true,message="Repair log inserted successfully."});
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddMilageData(List<VehicleMileageLog> Model)
        {
            try
            {
                foreach (var item in Model)
                {
                    if (item.VehicleId != Guid.Empty)
                    {
                        _Util.Facade.VehicleFacade.InsertMilageLog(item);
                    }
                }
            }
            catch (Exception) {
                return Json(new { result=false,message= "internal error." });
            }

            return Json(new {  result=true,message="Inserted successfully." });
        }
        [Authorize]
        //[HttpPost]
        public JsonResult GetVehicleListByKey(string key)
        {
            string result = "[]";
            if (!string.IsNullOrWhiteSpace(key))
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User; 
                //int ItemsLoadCount = _Util.Facade.GlobalSettingsFacade.GetEquipmentSearchMaxLoad(CurrentUser.CompanyId.Value);
                int ItemsLoadCount = 20;
                List<VehicleDetail> vehicleList = _Util.Facade.VehicleFacade.GetVehicleListBySearchKeyAndCompanyId(key, ItemsLoadCount);
                if (vehicleList.Count > 0)
                    result = JsonConvert.SerializeObject(vehicleList);
            }

            return Json(new { result = result }, JsonRequestBehavior.AllowGet);

        }
        [Authorize]
        //[HttpPost]
        public JsonResult GetDriversListByKey(string key)
        {
            string result = "[]";
            if (!string.IsNullOrWhiteSpace(key))
            {
                var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
                int ItemsLoadCount = 20;
                List<Employee> EmpList = _Util.Facade.EmployeeFacade.GetAllEmployeeByCompanyIdAndKey(CurrentUser.CompanyId.Value,key).Where(x=>x.IsActive == true && x.IsDeleted == false).OrderBy(x=>x.EMPName).ToList();
                    result = JsonConvert.SerializeObject(EmpList.Select(x => new { x.EMPName,x.UserName,x.Id,x.UserId,x.PermissionGroupName }));
            }
            return Json(new { result = result }, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        [HttpPost]
        public JsonResult AddVehicleRepair(VehicleRepairLog vehicleRepair)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var result = false;
            string message = "Internal error. Please try again.";
            if ( vehicleRepair.VehicleId != Guid.Empty )
            { 
                vehicleRepair.CreatedByUid = CurrentUser.UserId;
                //vehicleRepair.UserId = CurrentUser.UserId;
                vehicleRepair.CreatedDate = DateTime.Now.UTCCurrentTime(); 
                vehicleRepair.Id = (int)_Util.Facade.VehicleFacade.InsertVechileRepair(vehicleRepair);
                if (vehicleRepair.Id > 0)
                {
                    result = true;
                    message = "Saved successfully.";
                }
            }
            else
            {
                result = false;
                message = "Vehicle not found. Please try again.";
            }
            return Json(new { result = result, message = message , id = vehicleRepair.Id });

        }

        [Authorize]
        [HttpPost]
        public ActionResult DeleteVehicle(int? Id)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            bool result = false;
            try
            {
                VehicleDetail vehicle = _Util.Facade.VehicleFacade.GetVechileById(Id.Value);
                if (vehicle == null)
                {
                    return Json(new { result = false, message = "Vehicle not found" });
                }
                //delete all vehicleRepairLog
                //delete all VehicleMileageLog
                //delete all EmployeeVehicle
                _Util.Facade.VehicleFacade.DeleteAllEmployeeVehicleByVehicleId(vehicle.VehicleId);
                _Util.Facade.VehicleFacade.DeleteAllVehicleRepairLogByVehicleId(vehicle.VehicleId);
                _Util.Facade.VehicleFacade.DeleteAllVehicleMilageLogByvehicleId(vehicle.VehicleId);
                
                _Util.Facade.VehicleFacade.DeleteVechile(Id.Value);
                
                result = true;
            }
            catch (Exception)
            {

            }
            return Json(new { result = true });
        }

    }
}