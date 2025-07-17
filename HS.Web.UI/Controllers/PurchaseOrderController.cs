using ClosedXML.Excel;
using HS.DataAccess;
using HS.Entities;
using HS.Entities.Result;
using HS.Framework;
using HS.Framework.Utils;
using HS.Web.UI.Business.Inventory;
using HS.Web.UI.Helper;
using Newtonsoft.Json;
using NLog;
using PdfSharp.Pdf.Content.Objects;
using Rotativa;
using Rotativa.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web.Mvc;
using ExcelCl = HS.Web.UI.Helper.ExcelFormatHelper;
namespace HS.Web.UI.Controllers
{
    public class PurchaseOrderController : BaseController
    {
        public PurchaseOrderController()
        {
            logger = LogManager.GetCurrentClassLogger();
        }

        // GET: PurchaseOrder
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult OpenDemandOrderBranch(string DOId)
        {
            OpenDemandOrderBranchModel Model = new OpenDemandOrderBranchModel();
            Model.PurchaseOrderBranch = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderBranchByPOId(DOId);
            Model.PurchaseOrderDetail = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderDetailListByPurchaseOrderId(DOId);

            return View("_OpenDemandOrderBranch", Model);
        }
        [Authorize]
        [HttpPost]
        public JsonResult DeliverDO(string DOId, List<Guid> EquipmentIdList)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (EquipmentIdList != null && EquipmentIdList.Count() > 0)
            {
                List<PurchaseOrderDetail> PODList = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderDetailListByPurchaseOrderId(DOId);
                PurchaseOrderBranch PurchaseOrderBranch = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderBranchByPOId(DOId);
                PurchaseOrderTech POTech = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderTechByPOId(PurchaseOrderBranch.TechDemandOrderId);

                foreach (var item in PODList)
                {
                    if (EquipmentIdList.IndexOf(item.EquipmentId) > -1 && item.Quantity > 0)
                    {
                        #region Received
                        PurchaseOrderBranchReceived rec = new PurchaseOrderBranchReceived()
                        {
                            CreatedBy = CurrentUser.UserId,
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            IsReceived = false,
                            EquipmentId = item.EquipmentId,
                            Quantity = item.Quantity,
                            BranchDemandOrderId = PurchaseOrderBranch.DemandOrderId,
                            EquipDetail = item.EquipDetail,
                            EquipName = item.EquipName,
                            ReceivedBy = Guid.Empty,
                            RecieveQty = item.Quantity,
                            TotalPrice = item.TotalPrice,
                            UnitPrice = item.UnitPrice,

                        };
                        _Util.Facade.PurchaseOrderFacade.InsertPurchaseOrderBranchReceived(rec);

                        PurchaseOrderTechReceived Techrec = new PurchaseOrderTechReceived()
                        {
                            CreatedBy = CurrentUser.UserId,
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            IsReceived = false,
                            EquipmentId = item.EquipmentId,
                            Quantity = item.Quantity,
                            BranchDemandOrderId = POTech.DemandOrderId,
                            EquipDetail = item.EquipDetail,
                            EquipName = item.EquipName,
                            ReceivedBy = Guid.Empty,
                            RecieveQty = item.Quantity,
                            TotalPrice = item.TotalPrice,
                            UnitPrice = item.UnitPrice,

                        };
                        _Util.Facade.PurchaseOrderFacade.InsertPurchaseOrderTechReceived(Techrec);
                        #endregion
                        item.RecieveQty = item.Quantity;
                        _Util.Facade.PurchaseOrderFacade.UpdatePurchaseOrderDetail(item);
                    }
                }
                POTech.Status = LabelHelper.DemandOrderStatus.ReadyToReceived;
                PurchaseOrderBranch.Status = LabelHelper.DemandOrderStatus.DOComplete;
                var POAnotherList = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderDetailListByPurchaseOrderId(DOId).Where(m => m.Quantity != m.RecieveQty).ToList();
                if (POAnotherList.Count > 0)
                {
                    PurchaseOrderBranch.Status = LabelHelper.DemandOrderStatus.DOPartialComplete;
                    POTech.Status = LabelHelper.DemandOrderStatus.PartiallyReadyToReceived;
                }
                _Util.Facade.PurchaseOrderFacade.UpdatePurchaseOrderBranch(PurchaseOrderBranch);
                _Util.Facade.PurchaseOrderFacade.UpdatePurchaseOrderTech(POTech);
            }

            return Json(new { result = true, message = "" });
        }
        [Authorize]
        [HttpPost]
        public JsonResult DOReceive(int? Id)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var PotechId = new Guid();
            if (Id > 0)
            {
                PurchaseOrderTechReceived PORecievedTech = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderTechReceivedById(Id.Value);

                PurchaseOrderTech POTech = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderTechByPOId(PORecievedTech.BranchDemandOrderId);
                PotechId = POTech.TechnicianId;
                PurchaseOrderBranch POBranch = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderBranchByTechPOId(POTech.DemandOrderId);

                PurchaseOrderBranchReceived PORecievedBranch = _Util.Facade.PurchaseOrderFacade.PurchaseOrderBranchReceivedByDOIDEqp(POBranch.DemandOrderId, PORecievedTech.EquipmentId);

                PurchaseOrderDetail PODetailsTech = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderDetailByPurchaseOrderId(POTech.DemandOrderId, PORecievedTech.EquipmentId);
                PurchaseOrderDetail PODetailsBranch = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderDetailByPurchaseOrderId(POBranch.DemandOrderId, PORecievedTech.EquipmentId);

                #region Warehouse
                InventoryWarehouse invw = new InventoryWarehouse()
                {
                    CompanyId = CurrentUser.CompanyId.Value,
                    EquipmentId = PORecievedTech.EquipmentId,
                    Type = LabelHelper.InventoryType.Release,
                    Quantity = PORecievedTech.Quantity.HasValue ? PORecievedTech.Quantity.Value : 0,
                    PurchaseOrderId = PORecievedTech.BranchDemandOrderId,
                    LastUpdatedBy = CurrentUser.UserId,
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                    Description = "Send to branch from warehouse"
                };
                _Util.Facade.InventoryFacade.InsertInventoryWareHouse(invw);
                #endregion

                #region Inventory Branch
                InventoryBranch invBr = new InventoryBranch()
                {
                    CompanyId = CurrentUser.CompanyId.Value,
                    EquipmentId = PORecievedTech.EquipmentId,
                    Type = LabelHelper.InventoryType.Add,
                    Quantity = PORecievedTech.Quantity.HasValue ? PORecievedTech.Quantity.Value : 0,
                    PurchaseOrderId = PORecievedTech.BranchDemandOrderId,
                    LastUpdatedBy = CurrentUser.UserId,
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                    Description = "Receive by branch from warehouse"
                };
                _Util.Facade.InventoryFacade.InsertInventoryBranch(invBr);
                invBr = new InventoryBranch()
                {
                    CompanyId = CurrentUser.CompanyId.Value,
                    EquipmentId = PORecievedTech.EquipmentId,
                    Type = LabelHelper.InventoryType.Release,
                    Quantity = PORecievedTech.Quantity.HasValue ? PORecievedTech.Quantity.Value : 0,
                    PurchaseOrderId = PORecievedTech.BranchDemandOrderId,
                    LastUpdatedBy = CurrentUser.UserId,
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                    Description = "Send to technician from branch"
                };
                _Util.Facade.InventoryFacade.InsertInventoryBranch(invBr);
                #endregion

                #region Inventory Tech

                InventoryTech invtech = new InventoryTech()
                {
                    CompanyId = CurrentUser.CompanyId.Value,
                    EquipmentId = PORecievedTech.EquipmentId,
                    Type = LabelHelper.InventoryType.Add,
                    Quantity = PORecievedTech.Quantity.HasValue ? PORecievedTech.Quantity.Value : 0,
                    PurchaseOrderId = PORecievedTech.BranchDemandOrderId,
                    LastUpdatedBy = CurrentUser.UserId,
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                    Description = "Receive by technician from branch",
                    TechnicianId = POTech.TechnicianId,
                };
                _Util.Facade.InventoryFacade.InsertInventoryTech(invtech);

                #endregion

                #region PurchaseOrder
                if (PODetailsTech.RecieveQty == null)
                {
                    PODetailsTech.RecieveQty = 0;
                }
                PODetailsTech.RecieveQty += PORecievedTech.Quantity;
                _Util.Facade.PurchaseOrderFacade.UpdatePurchaseOrderDetail(PODetailsTech);
                #endregion

                #region Recieve
                PORecievedBranch.IsReceived = PORecievedTech.IsReceived = true;
                PORecievedBranch.ReceivedDate = PORecievedTech.ReceivedDate = DateTime.Now;
                PORecievedBranch.ReceivedBy = PORecievedTech.ReceivedBy = CurrentUser.UserId;
                _Util.Facade.PurchaseOrderFacade.UpdatePurchaseOrderBranchReceived(PORecievedBranch);
                _Util.Facade.PurchaseOrderFacade.UpdatePurchaseOrderTechReceived(PORecievedTech);
                #endregion

                #region StatusChange
                var POAnotherList = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderDetailListByPurchaseOrderId(POTech.DemandOrderId).Where(m => m.Quantity != m.RecieveQty).ToList();
                if (POAnotherList.Count == 0)
                {
                    POTech.Status = LabelHelper.DemandOrderStatus.Received;
                    _Util.Facade.PurchaseOrderFacade.UpdatePurchaseOrderTech(POTech);
                }
                else
                {
                    var purchaseOrderTechReceivedCount = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderTechReceivedListByDOId(POTech.DemandOrderId).Where(m => m.IsReceived == false).ToList();
                    if (purchaseOrderTechReceivedCount.Count == 0)
                    {
                        POTech.Status = LabelHelper.DemandOrderStatus.WaitingForReceived;
                        _Util.Facade.PurchaseOrderFacade.UpdatePurchaseOrderTech(POTech);
                    }
                }
                #endregion
            }

            return Json(new { result = true, message = "", techId = PotechId });
        }
        [Authorize]
        [HttpPost]
        public JsonResult CreateAutoPO(string DOId)
        {
            bool PurchaseOrderCreate = false;
            var PurchaseOrderId = "";
            var PurchaseOrderIntId = 0;
            var TotalAmount = 0.0;
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            Company Company = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value);
            Hashtable datatemplate = new Hashtable();
            datatemplate.Add("ComapnyName", Company.CompanyName);
            datatemplate.Add("Address", Company.Address);
            datatemplate.Add("Street", Company.Street);
            datatemplate.Add("City", Company.City);
            datatemplate.Add("State", Company.State);
            datatemplate.Add("Zip", Company.ZipCode);
            datatemplate.Add("CompanyPhone", Company.Phone.FormatedPhoneNum());
            datatemplate.Add("EmailAddress", Company.EmailAdress);
            datatemplate.Add("WebAddress", Company.Website);
            string CompanyInfo = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(CurrentUser.CompanyId.Value);
            CompanyInfo = HS.Web.UI.Helper.LabelHelper.ParserHelper(CompanyInfo, datatemplate);
            string poPreText = "PO";
            GlobalSetting _poPretxt = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "PrefixForPurchaseOrderId");
            if (_poPretxt != null && !string.IsNullOrWhiteSpace(_poPretxt.Value) && _poPretxt.Value != "")
            {
                poPreText = _poPretxt.Value;
            }
            List<PurchaseOrderDetail> PODList = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderDetailListByPurchaseOrderId(DOId);
            PurchaseOrderBranch PurchaseOrderBranch = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderBranchByPOId(DOId);
            PurchaseOrderTech POTech = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderTechByPOId(PurchaseOrderBranch.TechDemandOrderId);

            foreach (var item in PODList)
            {
                var equipmentDetails = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentId(item.EquipmentId);
                var inventoryWareAvailableCount = _Util.Facade.InventoryFacade.inventoryWareAvailableCount(item.EquipmentId, CurrentUser.CompanyId.Value);
                var RemainCount = item.Quantity - inventoryWareAvailableCount;
                if (equipmentDetails != null && RemainCount > 0 && item.Quantity != item.RecieveQty)
                {
                    if (!PurchaseOrderCreate)
                    {
                        var PurchaseOrderWarehouse = new PurchaseOrderWarehouse()
                        {
                            CompanyId = CurrentUser.CompanyId.Value,
                            POFor = new Guid("22222222-2222-2222-2222-222222222222"),
                            Status = LabelHelper.PurchaseOrderStatus.Init,
                            IsReceived = false,
                            CreatedByUid = CurrentUser.UserId,
                            LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                            LastUpdatedByUid = CurrentUser.UserId,
                            Amount = 0.0,
                            Balance = 0.0,
                            BalanceDue = 0.0,
                            BillingAddress = "",
                            TotalAmount = 0.0,
                            Tax = 0.0,
                            Deposit = 0.0,
                            ShippingCost = 0.0,
                            ShippingAddress = CompanyInfo,
                            TaxType = "",
                            Message = "",
                            ShippingVia = "",
                            Description = "",
                            TrackingNo = "",
                            RecieveByUid = new Guid(),
                            ShippingDate = DateTime.Now.AddDays(1),
                            OrderDate = DateTime.Now.UTCCurrentTime(),
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                        };
                        PurchaseOrderIntId = PurchaseOrderWarehouse.Id = _Util.Facade.PurchaseOrderFacade.InsertPurchaseOrderWarehouse(PurchaseOrderWarehouse);
                        PurchaseOrderId = PurchaseOrderWarehouse.PurchaseOrderId = PurchaseOrderWarehouse.Id.GeneratePONo(poPreText);
                        PurchaseOrderCreate = _Util.Facade.PurchaseOrderFacade.UpdatePurchaseOrderWarehouse(PurchaseOrderWarehouse);
                    }
                    item.UnitPrice = equipmentDetails.Retail;
                    item.TotalPrice = equipmentDetails.Retail * RemainCount;
                    item.Quantity = RemainCount;
                    item.CreatedDate = DateTime.Now.UTCCurrentTime();
                    item.CreatedBy = CurrentUser.UserId;
                    item.BundleId = 0;
                    item.PurchaseOrderId = PurchaseOrderId;
                    TotalAmount += Convert.ToDouble(item.TotalPrice);
                    _Util.Facade.PurchaseOrderFacade.InsertPurchaseOrderDetail(item);

                    PurchaseOrderBranch.Status = LabelHelper.DemandOrderStatus.POCreated;
                    _Util.Facade.PurchaseOrderFacade.UpdatePurchaseOrderBranch(PurchaseOrderBranch);
                }
            }
            if (PurchaseOrderIntId > 0)
            {
                var POWareDetails = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderWarehouseById(PurchaseOrderIntId);
                if (POWareDetails != null)
                {
                    POWareDetails.TotalAmount = POWareDetails.Amount = TotalAmount;
                    _Util.Facade.PurchaseOrderFacade.UpdatePurchaseOrderWarehouse(POWareDetails);
                }
            }
            return Json(new { result = true, message = "", POID = PurchaseOrderIntId });
        }
        [Authorize]
        public ActionResult BranchDemandOrderPartial(PurchaseOrderFilter filters)
        {
            return PartialView("_BranchDemandOrderPartial");
        }
        [Authorize]
        public ActionResult BranchDemandOrderList(PurchaseOrderFilter filters)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            #region Permission Check
            if (!base.IsPermitted(UserPermissions.InventoryPermissions.PurchaseOrderTechTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            #endregion

            #region Inits
            if (filters == null)
            {
                filters = new PurchaseOrderFilter();
            }

            if (filters.PageNo == 0)
            {
                filters.PageNo = 1;
            }
            if (filters.EmployeeId == Guid.Empty)
            {
                filters.EmployeeId = CurrentUser.UserId;
            }
            filters.CompanyId = CurrentUser.CompanyId.Value;
            filters.PageSize = _Util.Facade.GlobalSettingsFacade.GetPOPageLimit(CurrentUser.CompanyId.Value);
            POListModel Model = new POListModel();
            #endregion

            //Model = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderListByFiltersTech(filters);
            Model = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderListByFiltersBranch(filters);
            ViewBag.PageNumber = filters.PageNo;
            ViewBag.OutOfNumber = 0;

            if (Model.PurchaseOrderBranchList.Count() > 0)
            {
                ViewBag.OutOfNumber = Model.TotalCount;
            }

            if ((int)ViewBag.PageNumber * filters.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * filters.PageSize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / filters.PageSize);
            ViewBag.order = filters.order;

            return View("_BranchDemandOrderListPartial", Model);
        }

        [Authorize]
        public ActionResult PurchaseOrderPartial(PurchaseOrderFilter filters)
        {
            List<SelectListItem> EstimatorIdList = new List<SelectListItem>();

            EstimatorIdList.Add(new SelectListItem()
            {
                Text = "Select Estimator Id",
                Value = "-1"
            });
            EstimatorIdList.AddRange(_Util.Facade.PurchaseOrderFacade.GetEstimatorIdListOfPurchaseOrder().Select(x =>
                                       new SelectListItem()
                                       {
                                           Text = x.Id,
                                           Value = x.Id
                                       }).ToList());
            ViewBag.EstimatorIdList = EstimatorIdList;

            List<SelectListItem> POStatus = new List<SelectListItem>();
            POStatus.Add(
                new SelectListItem
                {
                    Text = "Open",
                    Value = "1"
                });
            POStatus.Add(
                new SelectListItem
                {
                    Text = LabelHelper.PurchaseOrderStatus.Received,
                    Value = "2"
                });
            POStatus.Add(
                new SelectListItem
                {
                    Text = LabelHelper.PurchaseOrderStatus.ReceivedPartially,
                    Value = "3"
                });
            ViewBag.POStatus = POStatus;

            List<string> selectsts = new List<string>();
            if (!string.IsNullOrWhiteSpace(filters.selectsts) && filters.selectsts.ToLower() != "null")
            {
                string[] splituser = filters.selectsts.Split(',');
                if (splituser.Length > 0)
                {
                    filters.selectsts = string.Format("{0}", string.Join(",", splituser.Select(i => i.Replace("'", ""))));
                    foreach (var item in splituser)
                    {
                        selectsts.Add(item);
                    }
                }
            }
            ViewBag.selectsts = selectsts;
            ViewBag.EstimatorId = filters.EstimatorId;
            return PartialView("_PurchaseOrderPartial");
        }

        [Authorize]
        public ActionResult PurchaseOrderListPartial(PurchaseOrderFilter filters)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            DateTime StartDate = new DateTime();
            DateTime EndDate = new DateTime(); 
            #region Permission Check
            if (!base.IsPermitted(UserPermissions.InventoryPermissions.PurchaseOrderWareHouseTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            #endregion

            List<string> selectSts = new List<string>();
            if (!string.IsNullOrWhiteSpace(filters.selectsts) && filters.selectsts.ToLower() != "null")
            {
                string[] splituser = filters.selectsts.Split(',');
                if (splituser.Length > 0)
                {
                    filters.selectsts = string.Format("{0}", string.Join(",", splituser.Select(i => i.Replace("'", ""))));
                    foreach (var item in splituser)
                    {
                        selectSts.Add(item);
                    }
                }
            }

            #region Inits
            if (filters == null)
            {
                filters = new PurchaseOrderFilter();
            }

            if (filters.PageNo == 0)
            {
                filters.PageNo = 1;
            }
            filters.CompanyId = CurrentUser.CompanyId.Value;
            filters.EmployeeId = CurrentUser.UserId;
            filters.PageSize = _Util.Facade.GlobalSettingsFacade.GetPOPageLimit(CurrentUser.CompanyId.Value);
            POListModel Model = new POListModel();
            #endregion

            if (filters.GetReport)
            {
                if (filters.CompanyId == new Guid())
                {
                    filters.CompanyId = CurrentUser.CompanyId.Value;
                }

                //DataTable dt;
                if (!string.IsNullOrWhiteSpace(filters.Start) && !string.IsNullOrWhiteSpace(filters.End))
                {
                    StartDate = Convert.ToDateTime(filters.Start).SetZeroHour().ClientToUTCTime();
                    EndDate = Convert.ToDateTime(filters.End).SetMaxHour().ClientToUTCTime();
                    
                    DataTable dt = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderExportListByFilters(filters, StartDate, EndDate);
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["Created Date"] != DBNull.Value)
                        {
                            DateTime createdDate = (DateTime)row["Created Date"];
                            row["Created Date"] = createdDate.UTCToClientTime().ToString("M/d/yyyy");
                        }
                    }
                    dt.Columns.Remove("Id");
                    dt.Columns.Remove("CompanyId");
                    dt.Columns.Remove("CreatedByUid");
                    dt.Columns.Remove("SuplierId");
                    dt.Columns.Remove("POFor");
                    dt.Columns.Remove("RowNum");
                    dt.Columns.Remove("TotalOrderPrice");
                    int[] colarray = { 8 };
                    int[] rowarray = { dt.Rows.Count + 2 };
                    return MakeExcelFromDataTable(dt, "Purchase Order", rowarray, colarray);
                }
                else
                {
                    DataTable dt = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderExportListByFilters(filters, StartDate, EndDate);
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["Created Date"] != DBNull.Value)
                        {
                            DateTime createdDate = (DateTime)row["Created Date"];
                            row["Created Date"] = createdDate.UTCToClientTime().ToString("M/d/yyyy");
                        }
                    }
                    int[] colarray = { 8 };
                    int[] rowarray = { dt.Rows.Count + 2 };
                    dt.Columns.Remove("Id");
                    dt.Columns.Remove("CompanyId");
                    dt.Columns.Remove("CreatedByUid");
                    dt.Columns.Remove("SuplierId");
                    dt.Columns.Remove("POFor");
                    dt.Columns.Remove("RowNum");
                    dt.Columns.Remove("TotalOrderPrice");
                    return MakeExcelFromDataTable(dt, "Purchase Order", rowarray, colarray);
                }
            }
             
            filters.IsAllTechPO = true;
            if (!base.IsPermitted(UserPermissions.InventoryPermissions.ShowAllTechnicianPO))
            {
                filters.IsAllTechPO = false;
            }
            if (!string.IsNullOrWhiteSpace(filters.Start) && filters.Start != "undefined" && filters.Start != "01/01/0001" && !string.IsNullOrWhiteSpace(filters.End) && filters.Start != "01/01/0001")
            {
                StartDate = Convert.ToDateTime(filters.Start).SetZeroHour().ClientToUTCTime();
                EndDate = Convert.ToDateTime(filters.End).SetMaxHour().ClientToUTCTime();

                Model = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderListByFilters(filters,StartDate,EndDate);
            }
            Model = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderListByFilters(filters, StartDate, EndDate);
            ViewBag.PageNumber = filters.PageNo;
            ViewBag.OutOfNumber = 0;

            if (Model.PurchaseOrderWarehouseList.Count() > 0)
            {
                ViewBag.OutOfNumber = Model.TotalCount;
            }

            if ((int)ViewBag.PageNumber * filters.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * filters.PageSize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / filters.PageSize);
            ViewBag.order = filters.order;
            string poPreText = "PO";
            GlobalSetting _poPretxt = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "PrefixForPurchaseOrderId");
            if (_poPretxt != null && !string.IsNullOrWhiteSpace(_poPretxt.Value) && _poPretxt.Value != "")
            {
                poPreText = _poPretxt.Value;
            }
            Model.PrefixForPurchaseOrderId = poPreText;

            bool TechnicianColumnPO = false;
            GlobalSetting tehcnicianColumn = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "PurchaseOrderTechnicianColumn");
            if (tehcnicianColumn != null && !string.IsNullOrWhiteSpace(tehcnicianColumn.Value))
            {
                TechnicianColumnPO = Convert.ToBoolean(tehcnicianColumn.Value);
            }
            ViewBag.TechnicianColumnPO = TechnicianColumnPO;

            return View("_PurchaseOrderListPartial", Model);
        }
        private FileContentResult MakeExcelFromDataTable(DataTable dtResult, string ReportFor, int[] rowIndex, int[] col_Format_Number)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                if (dtResult != null)
                {

                    var worksheet = wb.Worksheets.Add(dtResult);
                    if (ReportFor == "BrinksReport")
                    {
                        wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        wb.Style.Font.Bold = true;
                        worksheet.AutoFilter.Enabled = false;

                        worksheet.Column(1).Style.Font.SetFontName("Times New Roman");
                        worksheet.Column(2).Style.Font.SetFontName("Times New Roman");
                        worksheet.Column(3).Style.Font.SetFontName("Times New Roman");
                        worksheet.Column(4).Style.Font.SetFontName("Times New Roman");
                        worksheet.Column(5).Style.Font.SetFontName("Times New Roman");
                        worksheet.Column(6).Style.Font.SetFontName("Times New Roman");
                        worksheet.Column(7).Style.Font.SetFontName("Times New Roman");
                        worksheet.Column(1).Style.Font.SetFontSize(12);
                        worksheet.Column(2).Style.Font.SetFontSize(12);
                        worksheet.Column(3).Style.Font.SetFontSize(12);
                        worksheet.Column(4).Style.Font.SetFontSize(12);
                        worksheet.Column(5).Style.Font.SetFontSize(12);
                        worksheet.Column(6).Style.Font.SetFontSize(12);
                        worksheet.Column(7).Style.Font.SetFontSize(12);

                        worksheet.Column(1).Width = 13.71;
                        worksheet.Column(2).Width = 13.71;
                        worksheet.Column(3).Width = 13.71;
                        worksheet.Column(4).Width = 13.71;
                        worksheet.Column(5).Width = 13.71;
                        worksheet.Column(6).Width = 13.71;
                        worksheet.Column(7).Width = 13.71;



                        worksheet.Column(1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Column(2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Column(3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        worksheet.Column(4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                        worksheet.Column(5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Column(6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Column(7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        worksheet.Ranges("A1:G1").Style.Font.Bold = true;
                        worksheet.Cells("A1:G1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Cells("A1:D1").Style.Font.SetFontSize(11);
                        worksheet.Cells("A1:D1").Style.Font.SetFontName("Calibri");
                        worksheet.Cells("A1:D1").Style.Font.SetBold();
                        worksheet.Cells("A1:G1").Style.Font.SetFontColor(XLColor.CoolBlack);
                        worksheet.Cells("A1:D1").Style.Fill.BackgroundColor = XLColor.Yellow;
                        worksheet.Cells("E1:G1").Style.Fill.BackgroundColor = XLColor.AshGrey;


                    }
                    var format = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey("CurrentCurrencyExcelFormat");
                    if (col_Format_Number != null && format != null && rowIndex != null)
                    {
                        foreach (int itemcol in col_Format_Number)
                        {
                            for (int i = 1; i < rowIndex[0]; i++)
                            {
                                worksheet.Cell(i, itemcol).Style.NumberFormat.Format = format.Value;

                            }
                        }
                    }
                    MemoryStream memorystreem = new MemoryStream();
                    wb.SaveAs(memorystreem);
                    var fName = string.Format("{0}-{1}.xlsx", ReportFor, DateTime.Now.UTCCurrentTime().ToString("MM/dd/yyyy"));

                    byte[] fileContents = memorystreem.ToArray();
                    var userAgent = HttpContext.Request.UserAgent.ToLower();
                    if (userAgent.Contains("iphone;") || userAgent.Contains("ipad;"))
                    {
                        return File(fileContents, ExcelCl.Format("ExcelFormat"), fName);
                    }
                    else
                    {
                        return File(fileContents, ExcelCl.Format("ExcelFormat"), fName);
                    }
                }
                else
                {
                    byte[] fileContents = new byte[1];
                    return File(fileContents, ExcelCl.Format("ExcelFormat"), "empty.xlsx");
                }
            }
        }
        [Authorize]
        public ActionResult BranchPurchaseOrderPartial(PurchaseOrderFilter filters)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            List<CompanyBranch> BranchList = new List<CompanyBranch>();
            if (currentLoggedIn == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            BranchList = _Util.Facade.CompanyBranchFacade.GetAllCompanyBranchByCompanyId(currentLoggedIn.CompanyId.Value);
            return PartialView("_BranchPurchaseOrderPartial", BranchList);
        }

        [Authorize]
        public ActionResult BranchPurchaseOrderListPartial(PurchaseOrderFilter filters)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            #region Permission Check
            if (!base.IsPermitted(UserPermissions.InventoryPermissions.PurchaseOrderBranchTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            #endregion

            #region Inits
            if (filters == null)
            {
                filters = new PurchaseOrderFilter();
            }

            if (filters.PageNo == 0)
            {
                filters.PageNo = 1;
            }
            if (filters.BranchId == 0)
            {
                var BranchInfo = _Util.Facade.EmployeeFacade.GetUserBranchByCompanyIdAndUserId(CurrentUser.CompanyId.Value, CurrentUser.UserId);
                if (BranchInfo != null)
                {
                    filters.BranchId = BranchInfo.BranchId;
                }
            }
            filters.CompanyId = CurrentUser.CompanyId.Value;
            filters.PageSize = _Util.Facade.GlobalSettingsFacade.GetPOPageLimit(CurrentUser.CompanyId.Value);
            POListModel Model = new POListModel();
            #endregion

            Model = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderListByFiltersBranch(filters);
            ViewBag.PageNumber = filters.PageNo;
            ViewBag.OutOfNumber = 0;

            if (Model.PurchaseOrderList.Count() > 0)
            {
                ViewBag.OutOfNumber = Model.TotalCount;
            }

            if ((int)ViewBag.PageNumber * filters.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * filters.PageSize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / filters.PageSize);
            ViewBag.order = filters.order;
            return View("_BranchPurchaseOrderListPartial", Model);
        }

        [Authorize]
        public ActionResult TechPurchaseOrderPartial(PurchaseOrderFilter filters)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            List<Employee> TechnicianList = new List<Employee>();
            if (currentLoggedIn == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            if (base.IsPermitted(UserPermissions.InventoryPermissions.TechAllPurchaseOrder))
            {
                if (currentLoggedIn.UserRole.ToLower().IndexOf("technician") != 0 && currentLoggedIn.UserRole.ToLower().IndexOf("installation") != 0)
                {
                    TechnicianList = _Util.Facade.EmployeeFacade.GetTechtransferEmployeeByCompanyIdAndTag(currentLoggedIn.CompanyId.Value, filters.Searchtext, LabelHelper.UserTags.Technicians, currentLoggedIn.UserId);

                    //TechnicianList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(currentLoggedIn.CompanyId.Value, LabelHelper.UserTags.Technicians, new Guid());
                }
                else
                {

                    TechnicianList = _Util.Facade.EmployeeFacade.GetSingleTechnicianList(currentLoggedIn.UserId);
                }
            }
          
            return PartialView("_TechPurchaseOrderPartial", TechnicianList);
        }

        [Authorize]
        public ActionResult TechPurchaseOrderListPartial(PurchaseOrderFilter filters)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            #region Permission Check
            if (!base.IsPermitted(UserPermissions.InventoryPermissions.PurchaseOrderTechTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            #endregion

            #region Inits
            if (filters == null)
            {
                filters = new PurchaseOrderFilter();
            }

            if (filters.PageNo == 0)
            {
                filters.PageNo = 1;
            }
            if (filters.EmployeeId == Guid.Empty)
            {
                filters.EmployeeId = CurrentUser.UserId;
            }
            filters.CompanyId = CurrentUser.CompanyId.Value;
            filters.PageSize = _Util.Facade.GlobalSettingsFacade.GetPOPageLimit(CurrentUser.CompanyId.Value);
            POListModel Model = new POListModel();
            #endregion

            Model = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderListByFiltersTech(filters);
            Model.PurchaseOrderTechReceivedList = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderTechReceivedListByTech(filters);
            ViewBag.PageNumber = filters.PageNo;
            ViewBag.OutOfNumber = 0;

            if (Model.PurchaseOrderTechList.Count() > 0)
            {
                ViewBag.OutOfNumber = Model.TotalCount;
            }

            if ((int)ViewBag.PageNumber * filters.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * filters.PageSize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / filters.PageSize);
            ViewBag.order = filters.order;
            ViewBag.orderrcv = filters.orderrcv;
            ViewBag.EmployeeId = filters.EmployeeId;
            ViewBag.SearchText = filters.Searchtext;
            ViewBag.SearchTextRcv = filters.SearchtextRcv;
            return View("_TechPurchaseOrderListPartial", Model);
        }

        #region PurchaseOrder
        [Authorize]
        public ActionResult AddPurchaseOrder(int? Id, string PurchaseOrderId, bool? Receive, string OpenTab, Guid? EmployeeId, int? BranchId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var BranchList = _Util.Facade.CompanyBranchFacade.GetAllCompanyBranchByCompanyId(CurrentUser.CompanyId.Value);
            CreatePurchaseOrder model = new CreatePurchaseOrder();

            if (Id.HasValue && Id > 0 || (!string.IsNullOrWhiteSpace(PurchaseOrderId)))
            {
                if (Id.HasValue && Id > 0)
                {
                    model.PurchaseOrderWarehouse = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderWarehouseById(Id.Value);
                }
                else
                {
                    model.PurchaseOrderWarehouse = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderWarehouseByPurchaseOrderId(PurchaseOrderId);
                }
                if (model.PurchaseOrderWarehouse == null)
                {
                    return PartialView("~/Views/Shared/_NotFound.cshtml");
                }
                if (model.PurchaseOrderWarehouse.OrderDate == new DateTime())
                {
                    model.PurchaseOrderWarehouse.OrderDate = DateTime.Today.AddDays(1);
                }
                model.PurchaseOrderDetail = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderDetailListByPurchaseOrderId(model.PurchaseOrderWarehouse.PurchaseOrderId);
                if (model.PurchaseOrderWarehouse.SuplierId != new Guid())
                {
                    model.Supplier = _Util.Facade.SupplierFacade.GetSupplierBySupplierId(model.PurchaseOrderWarehouse.SuplierId);
                }
                else
                {
                    model.Supplier = new Supplier();
                }
                if (Receive.HasValue && Receive == true)
                {
                    model.ReceiveNow = Receive.Value;
                    if (model.PurchaseOrderDetail.Any(x => x.RecieveQty > 0))
                    {
                        model.PurchaseOrderWarehouse.RecieveDate = model.PurchaseOrderDetail.Max(x => x.CreatedDate);
                    }
                    else
                    {
                        model.PurchaseOrderWarehouse.RecieveDate = DateTime.Now.UTCCurrentTime();

                    }
                }
            
                else if (model.PurchaseOrderWarehouse.Status == LabelHelper.PurchaseOrderStatus.Received)
                {
                    model.ReceiveNow = true;
                    if (model.PurchaseOrderDetail.Any(x => x.RecieveQty > 0))
                    {
                        model.PurchaseOrderWarehouse.RecieveDate = model.PurchaseOrderDetail.Max(x => x.CreatedDate);
                    }
                    else
                    {
                        model.PurchaseOrderWarehouse.RecieveDate = DateTime.Now.UTCCurrentTime();

                    }
                }
                if (OpenTab == "Ware")
                {
                    var PurchaseOrderWare = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderWarehouseByPOId(model.PurchaseOrderWarehouse.PurchaseOrderId);
                    if (PurchaseOrderWare != null)
                    {
                        model.PurchaseOrderWarehouse.Status = PurchaseOrderWare.Status;
                    }
                }
                Estimator POWEstimator = _Util.Facade.EstimatorFacade.GetEstimatorByEstimatorId(model.PurchaseOrderWarehouse.EstimatorId);
                if (POWEstimator != null)
                {
                    model.PurchaseOrderWarehouse.EstimatorIntId = POWEstimator.Id;
                }
                
            }
            else
            {
                string poPreText = "PO";
                GlobalSetting _poPretxt = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "PrefixForPurchaseOrderId");
                if (_poPretxt != null && !string.IsNullOrWhiteSpace(_poPretxt.Value) && _poPretxt.Value != "")
                {
                    poPreText = _poPretxt.Value;
                }
                Company Company = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value);
                Hashtable datatemplate = new Hashtable();
                datatemplate.Add("ComapnyName", Company.CompanyName);
                datatemplate.Add("Address", Company.Address);
                datatemplate.Add("Street", Company.Street);
                datatemplate.Add("City", Company.City);
                datatemplate.Add("State", Company.State);
                datatemplate.Add("Zip", Company.ZipCode);
                datatemplate.Add("CompanyPhone", Company.Phone);
                datatemplate.Add("EmailAddress", Company.EmailAdress);
                datatemplate.Add("WebAddress", Company.Website);

                string CompanyInfo = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(CurrentUser.CompanyId.Value);
                CompanyInfo = HS.Web.UI.Helper.LabelHelper.ParserHelper(CompanyInfo, datatemplate);

                if (OpenTab == "Ware")
                {
                    model.PurchaseOrderWarehouse = new PurchaseOrderWarehouse()
                    {
                        CompanyId = CurrentUser.CompanyId.Value,
                        POFor = new Guid("22222222-2222-2222-2222-222222222222"),
                        Status = LabelHelper.PurchaseOrderStatus.Init,
                        IsReceived = false,
                        CreatedByUid = CurrentUser.UserId,
                        LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                        LastUpdatedByUid = CurrentUser.UserId,
                        Amount = 0.0,
                        Balance = 0.0,
                        BalanceDue = 0.0,
                        BillingAddress = "",
                        TotalAmount = 0.0,
                        Tax = 0.0,
                        Deposit = 0.0,
                        ShippingCost = 0.0,
                        ShippingAddress = CompanyInfo,
                        TaxType = "",
                        Message = "",
                        ShippingVia = "",
                        Description = "",
                        TrackingNo = "",
                        RecieveByUid = new Guid(),
                        RecieveForUid = CurrentUser.UserId,
                        ShippingDate = DateTime.Now.AddDays(1),
                        OrderDate = DateTime.Now.UTCCurrentTime(),
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                    };
                    model.PurchaseOrderWarehouse.Id = _Util.Facade.PurchaseOrderFacade.InsertPurchaseOrderWarehouse(model.PurchaseOrderWarehouse);
                    model.PurchaseOrderWarehouse.PurchaseOrderId = model.PurchaseOrderWarehouse.Id.GeneratePONo(poPreText);
                    _Util.Facade.PurchaseOrderFacade.UpdatePurchaseOrderWarehouse(model.PurchaseOrderWarehouse);
                }
                model.PurchaseOrderDetail = new List<PurchaseOrderDetail>();
                model.Supplier = new Supplier();
                model.PurchaseOrderWarehouse.POFor=new Guid();
            }

            #region ViewBags

            ViewBag.POShipVia = _Util.Facade.LookupFacade.GetLookupByKey("POShipVia").OrderBy(x => x.DisplayText != "Select One").ThenBy(x => x.DisplayText).Select(x =>
                new SelectListItem()
                {
                    Text = x.DisplayText.ToString(),
                    Value = x.DataValue.ToString()
                }).ToList();

            List<SelectListItem> supList = _Util.Facade.SupplierFacade.GetAllSupplier().OrderBy(m => m.Name).Select(x =>
                new SelectListItem()
                {
                    Text = x.CompanyName.ToString(),
                    Value = x.SupplierId.ToString()
                }).ToList();
            supList.Insert(0, new SelectListItem()
            {
                Text = "Select One",
                Value = new Guid().ToString()
            });

            ViewBag.SupplierList = supList.OrderBy(x => x.Text != "Select One").ThenBy(x => x.Text).ToList();

            List<Employee> EmpList = _Util.Facade.EmployeeFacade.GetAllEmployee(CurrentUser.CompanyId.Value);
            var emplst = new List<SelectListItem>();

            emplst.Add(new SelectListItem
            {
                Text = "Select One",
                Value = "00000000-0000-0000-0000-000000000000",
                Selected = true
            });

            if (EmpList != null)
            {
                emplst.AddRange(EmpList.Select(x =>
                    new SelectListItem()
                    {
                        Text = x.FirstName + " " + x.LastName,
                        Value = x.UserId.ToString(),
                    }));
            }

            var TransferLocations = _Util.Facade.EmployeeFacade.GetTransferLocations(CurrentUser.CompanyId.Value);

            foreach (var location in TransferLocations)
            {
                emplst.Add(new SelectListItem()
                {
                    Text = location.UserName,
                    Value = location.UserId.ToString(),
                    Selected = location.UserId.ToString() == "22222222-2222-2222-2222-222222222222",
                });
            }


            //emplst.Add(new SelectListItem()
            //{
            //    Text = "Warehouse",
            //    Value = "22222222-2222-2222-2222-222222222222"
            //});

            //emplst.Add(new SelectListItem()
            //{
            //    Text = "Commercial lost bucket",
            //    Value = "22222222-2222-2222-2222-222222222223"
            //});

            //emplst.Add(new SelectListItem()
            //{
            //    Text = "Residential lost bucket",
            //    Value = "22222222-2222-2222-2222-222222222224"
            //});

            //emplst.Add(new SelectListItem()
            //{
            //    Text = "Warehouse lost bucket",
            //    Value = "22222222-2222-2222-2222-222222222225"
            //});

            //emplst.Add(new SelectListItem()
            //{
            //    Text = "RMA getting equipment back bucket",
            //    Value = "22222222-2222-2222-2222-222222222226"
            //});

            //emplst.Add(new SelectListItem()
            //{
            //    Text = "RMA getting a refund",
            //    Value = "22222222-2222-2222-2222-222222222231"
            //});

            //emplst.Add(new SelectListItem()
            //{
            //    Text = "Everything else bucket",
            //    Value = "22222222-2222-2222-2222-222222222232"
            //});

            //emplst.Insert(0, new SelectListItem()
            //{
            //    Text = "Select One",
            //    Value = new Guid().ToString()
            //});

            //ViewBag.EmployeeList = emplst.OrderBy(x => x.Text != "Select One").ThenBy(x => x.Text).ToList();
            ViewBag.EmployeeList = emplst
              .OrderBy(x => x.Text != "Select One")  
              .ThenBy(x => x.Text == "Warehouse" ? 0
                          : x.Text.EndsWith(" Bucket") ? 1
                          : x.Text == "X-Unused 01" ? 2
                          : x.Text == "X-Unused 02" ? 3
                          : 4)  
              .ThenBy(x => x.Text)
              .ToList();


            ViewBag.OpenTab = OpenTab;

            #endregion
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddPurchaseOrder(CreatePurchaseOrder model)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Bill Bill = new Bill();
            var BranchList = _Util.Facade.CompanyBranchFacade.GetAllCompanyBranchByCompanyId(CurrentUser.CompanyId.Value);
            bool Result = false;
            #region validations
            if (model.PurchaseOrderWarehouse.Id == 0 || string.IsNullOrWhiteSpace(model.PurchaseOrderWarehouse.PurchaseOrderId))
            {
                return Json(new { result = Result, message = "Id not found." });
            }
            #endregion
            PurchaseOrderWarehouse tempPo = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderWarehouseById(model.PurchaseOrderWarehouse.Id);
            #region Validations
            if (tempPo == null)
            {
                return Json(new { result = Result, message = "PO not found please create a new one." });
            }
            if (tempPo.CompanyId != CurrentUser.CompanyId.Value)
            {
                return Json(new { result = Result, message = "Company not matched please create a new one." });
            }
            if (tempPo.PurchaseOrderId != model.PurchaseOrderWarehouse.PurchaseOrderId)
            {
                return Json(new { result = Result, message = "Validation failed. Please try again." });
            }
            #endregion

            model.PurchaseOrderWarehouse.CompanyId = tempPo.CompanyId;
            model.PurchaseOrderWarehouse.OrderDate = tempPo.CreatedDate;

            if (model.PurchaseOrderWarehouse.Status == LabelHelper.PurchaseOrderStatus.Init)
            {
                model.PurchaseOrderWarehouse.Status = LabelHelper.PurchaseOrderStatus.Created;
            }
            else
            {
                model.PurchaseOrderWarehouse.Status = tempPo.Status;
            }
            model.PurchaseOrderWarehouse.EstimatorId = tempPo.EstimatorId;
            model.PurchaseOrderWarehouse.CreatedByUid = tempPo.CreatedByUid;
            model.PurchaseOrderWarehouse.CreatedDate = tempPo.CreatedDate;
            model.PurchaseOrderWarehouse.CreatedByUid = tempPo.CreatedByUid;
            model.PurchaseOrderWarehouse.LastUpdatedByUid = CurrentUser.UserId;
            model.PurchaseOrderWarehouse.LastUpdatedDate = DateTime.Now.UTCCurrentTime();
            if (model.ReceiveNow)
            {
                var TotalQty = 0;
                var TotalReceiveQty = 0;
                if (model.PurchaseOrderDetail != null && model.PurchaseOrderDetail.Count > 0)
                {
                    foreach (var item in model.PurchaseOrderDetail)
                    {
                        TotalQty += item.Quantity.Value;
                        TotalReceiveQty += item.RecieveQty.Value;
                    }
                }
                if (TotalQty > TotalReceiveQty)
                {
                    model.PurchaseOrderWarehouse.Status = LabelHelper.PurchaseOrderStatus.ReceivedPartially;
                    model.PurchaseOrderWarehouse.RecieveByUid = CurrentUser.UserId;
                    model.PurchaseOrderWarehouse.RecieveDate = DateTime.Now.UTCCurrentTime();
                }
                else if (TotalQty <= TotalReceiveQty)
                {
                    model.PurchaseOrderWarehouse.Status = LabelHelper.PurchaseOrderStatus.Received;
                    model.PurchaseOrderWarehouse.RecieveByUid = CurrentUser.UserId;
                    model.PurchaseOrderWarehouse.RecieveDate = DateTime.Now.UTCCurrentTime();
                }

                if (model.OpenTab == "Ware")
                {
                    var PurchaseOrderWare = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderWarehouseByPOId(model.PurchaseOrderWarehouse.PurchaseOrderId);
                    PurchaseOrderWare.Status = LabelHelper.PurchaseOrderStatus.Received;
                    PurchaseOrderWare.IsReceived = true;
                    PurchaseOrderWare.RecieveByUid = CurrentUser.UserId;
                    PurchaseOrderWare.RecieveDate = DateTime.Now.UTCCurrentTime();
                    _Util.Facade.PurchaseOrderFacade.UpdatePurchaseOrderWarehouse(PurchaseOrderWare);
                }
            }
            else
            {
                model.PurchaseOrderWarehouse.RecieveForUid = tempPo.RecieveForUid;
                model.PurchaseOrderWarehouse.RecieveByUid = tempPo.RecieveByUid;
                model.PurchaseOrderWarehouse.RecieveDate = tempPo.RecieveDate;
            }

            Result = _Util.Facade.PurchaseOrderFacade.UpdatePurchaseOrderWarehouse(model.PurchaseOrderWarehouse);
            var purchaseOrderDetailget = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderDetailListByPurchaseOrderId(model.PurchaseOrderWarehouse.PurchaseOrderId);

            if (Result)
            {
                _Util.Facade.PurchaseOrderFacade.DeleteAllPurchaseOrderDetailByPurchaseOrderId(model.PurchaseOrderWarehouse.PurchaseOrderId);
                if (model.PurchaseOrderWarehouse.Status == LabelHelper.PurchaseOrderStatus.Received && model.PurchaseOrderDetail.Where(x => x.RecieveQty > 0).Count() > 0)
                {
                    Supplier supplierobj = _Util.Facade.SupplierFacade.GetSupplierBySupplierId(model.PurchaseOrderWarehouse.SuplierId);
                    if (supplierobj != null)
                    {
                        Bill.SupplierId = supplierobj.Id;
                    }

                    Bill.CompanyId = CurrentUser.CompanyId.Value;
                    Bill.UpdatedBy = User.Identity.Name;
                    Bill.UpdatedDate = DateTime.Now.UTCCurrentTime();
                    Bill.PaymentStatus = "Open";
                    Bill.BillFor = "Vendor";
                    Bill.Notes = "Bill From Purchase Order " + model.PurchaseOrderWarehouse.PurchaseOrderId;
                    Bill.Amount = model.PurchaseOrderWarehouse.Amount;
                    Bill.PaymentDate = DateTime.Now.UTCCurrentTime();
                    Bill.PaymentDue = model.PurchaseOrderWarehouse.Amount;
                    Bill.PaymentDueDate = DateTime.Now.UTCCurrentTime().AddMonths(1);
                    _Util.Facade.BillFacade.InsertBill(Bill);
                    Bill.BillNo = Bill.Id.GenerateBillNO();
                    _Util.Facade.BillFacade.UpdateBill(Bill);
                }
           
                 foreach (var item in model.PurchaseOrderDetail.GroupBy(x => x.EquipmentId).Select(g => g.First()))
                    {
                    var existingDetail = purchaseOrderDetailget?.FirstOrDefault(x => x.EquipmentId == item.EquipmentId);

                    if (item.CurrentQty > 0)
                    {
                        item.CreatedDate = DateTime.Now.UTCCurrentTime();
                    }
                    else
                    {
                 
                        item.CreatedDate = existingDetail != null ? existingDetail.CreatedDate : DateTime.Now.UTCCurrentTime();
                    }


                    item.CreatedBy = CurrentUser.UserId;
                    item.BundleId = 0;
                    item.PurchaseOrderId = model.PurchaseOrderWarehouse.PurchaseOrderId;
                    _Util.Facade.PurchaseOrderFacade.InsertPurchaseOrderDetail(item);

                    if (model.ReceiveNow && item.EquipmentId != new Guid() && item.RecieveQty > 0)
                    {
                        Equipment eq = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentId(item.EquipmentId);
                        if (eq != null)
                        {
                            eq.SupplierCost = item.UnitPrice;
                            if (model.PurchaseOrderWarehouse.SuplierId != new Guid())
                            {
                                Supplier sup = _Util.Facade.SupplierFacade.GetSupplierBySupplierId(model.PurchaseOrderWarehouse.SuplierId);
                                if (sup != null)
                                {
                                    eq.SupplierId = sup.Id;
                                }
                            }
                            _Util.Facade.EquipmentFacade.UpdateEquipment(eq);
                        }
                        //what happens if equipment not found ?
                        if (model.OpenTab == "Ware" && model.PurchaseOrderWarehouse.POFor != null)
                        {
                            //#region Order For
                            if (model.PurchaseOrderWarehouse.POFor.IsVirtualLocation())
                            {
                                if (item.CurrentQty != 0)
                                {
                                    InventoryWarehouse invWare = new InventoryWarehouse()
                                    {
                                        CompanyId = CurrentUser.CompanyId.Value,
                                        EquipmentId = item.EquipmentId,
                                        Type = LabelHelper.InventoryType.Add,
                                        Quantity = item.CurrentQty,
                                        PurchaseOrderId = model.PurchaseOrderWarehouse.PurchaseOrderId,
                                        LastUpdatedBy = CurrentUser.UserId,
                                        LastUpdatedDate = DateTime.Now,
                                        Description = "Added to location by PO",
                                        LocationId = model.PurchaseOrderWarehouse.POFor
                                    };
                                    _Util.Facade.InventoryFacade.InsertInventoryWareHouse(invWare);
                                }
                            }
                            
                            //#endregion

                            else if (model.PurchaseOrderWarehouse.POFor != new Guid("00000000-0000-0000-0000-000000000000")
                                && !model.PurchaseOrderWarehouse.POFor.IsVirtualLocation() && item.CurrentQty > 0)
                            {
                                //InventoryTech invTech = new InventoryTech()
                                //{
                                //    CompanyId = CurrentUser.CompanyId.Value,
                                //    EquipmentId = item.EquipmentId,
                                //    Type = LabelHelper.InventoryType.Add,
                                //    Quantity = item.RecieveQty.Value,
                                //    TechnicianId = model.PurchaseOrderWarehouse.POFor,
                                //    PurchaseOrderId = model.PurchaseOrderWarehouse.PurchaseOrderId,
                                //    LastUpdatedBy = CurrentUser.UserId,
                                //    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                                //    Description = "Added for technician by PO"
                                //};
                                //_Util.Facade.InventoryFacade.InsertInventoryTech(invTech);

                                //invTech.Type = LabelHelper.InventoryType.Release;
                                //invTech.TechnicianId = new Guid("33333333-3333-3333-3333-333333333333");
                                //_Util.Facade.InventoryFacade.InsertInventoryTech(invTech);

                                TechTransferRequest techTransferRequest = new TechTransferRequest();
                                techTransferRequest.Items = new List<AssignedInventoryTechReceived>();
                                techTransferRequest.Items.Add(new AssignedInventoryTechReceived()
                                {
                                    EquipmentId = item.EquipmentId,
                                    TechnicianId = new Guid("22222222-2222-2222-2222-222222222221"),
                                    ReceivedBy = model.PurchaseOrderWarehouse.POFor,
                                    Quantity = item.CurrentQty,
                                    CreatedBy = CurrentUser.UserId,
                                    CreatedDate = DateTime.Now,
                                    IsApprove = true,
                                    IsReceived = true,
                                    
                                    ReqSrc = "[PURORD-WHTT]"
                                });

                                var result = _Util.Facade.InventoryFacade.InsertTechTransferwithoutapprove(techTransferRequest, CurrentUser.UserId) > 0;
                                var transferItem = techTransferRequest.Items.First();
                                var  transferResult = new EquipmentTransfer().TransferWHToTech((Guid)transferItem.ReceivedBy, transferItem.EquipmentId, CurrentUser.CompanyId.Value, CurrentUser.UserId, techTransferRequest.TechnicianId, transferItem.ReqSrc, transferItem.Quantity); //"[TT-Approve]"
                               
                                logger.WithProperty("tags", $"Trasfer Equipment Approved datetime {DateTime.Now} and {transferItem.Id}")
                                 .WithProperty("params", JsonConvert.SerializeObject(transferItem))
                                 .Trace($"Equipment History Approved by {CurrentUser.UserId}.");

                            }

                            else
                            {
                                if (item.CurrentQty != 0)
                                {
                                    InventoryWarehouse invWare = new InventoryWarehouse()
                                    {
                                        CompanyId = CurrentUser.CompanyId.Value,
                                        EquipmentId = item.EquipmentId,
                                        Type = LabelHelper.InventoryType.Add,
                                        Quantity = item.CurrentQty,
                                        PurchaseOrderId = model.PurchaseOrderWarehouse.PurchaseOrderId,
                                        LastUpdatedBy = CurrentUser.UserId,
                                        LastUpdatedDate = DateTime.Now,
                                        Description = "Added to warehouse by PO",
                                        LocationId = model.PurchaseOrderWarehouse.POFor
                                    };
                                    _Util.Facade.InventoryFacade.InsertInventoryWareHouse(invWare);
                                }
                            }
                            
                        }

                    }
                    if (model.PurchaseOrderWarehouse.Status == LabelHelper.PurchaseOrderStatus.Received && item.RecieveQty > 0)
                    {
                        BillDetail objBillDetail = new BillDetail()
                        {
                            CustomerBillId = Bill.Id,
                            EquipmentId = item.EquipmentId,
                            AccoutTypeId = 15,
                            Dscription = item.EquipName,
                            Quantity = item.Quantity,
                            Rate = item.UnitPrice,
                            Amount = item.TotalPrice,
                            ItemName = ""
                        };
                        _Util.Facade.BillFacade.InsertBillDetail(objBillDetail);
                    }
                }
                if (model.PurchaseOrderWarehouse.Status == LabelHelper.PurchaseOrderStatus.Received && model.PurchaseOrderDetail.Where(x => x.RecieveQty > 0).Count() > 0)
                {
                    var objbillfile = _Util.Facade.BillFacade.GetBillFileListByBillId(model.PurchaseOrderWarehouse.PurchaseOrderId);
                    if (objbillfile != null && objbillfile.Count > 0)
                    {
                        foreach (var file in objbillfile)
                        {
                            var filebill = _Util.Facade.BillFacade.GetBillFileById(file.Id);
                            if (filebill != null)
                            {
                                BillFile objfilebill = new BillFile()
                                {
                                    FileDescription = filebill.FileDescription,
                                    Filename = filebill.Filename,
                                    FileFullName = filebill.FileFullName,
                                    Uploadeddate = filebill.Uploadeddate,
                                    BillNo = Bill.Id.GenerateBillNO(),
                                    CompanyId = CurrentUser.CompanyId.Value,
                                    IsActive = filebill.IsActive,
                                    FileSize = filebill.FileSize
                                };
                                _Util.Facade.BillFacade.InsertBillFile(objfilebill);
                            }
                        }
                    }
                }
            }
            if (tempPo.Status == LabelHelper.PurchaseOrderStatus.Created || tempPo.Status == LabelHelper.PurchaseOrderStatus.ReceivedPartially)
            {
                if (model.PurchaseOrderDetail.Count > 0)
                {
                    Guid[] excludedTechnicianIds = new Guid[]
                {
            new Guid("22222222-2222-2222-2222-222222222222"),
            new Guid("22222222-2222-2222-2222-222222222223"),
            new Guid("22222222-2222-2222-2222-222222222224"),
            new Guid("22222222-2222-2222-2222-222222222225"),
            new Guid("22222222-2222-2222-2222-222222222226"),
            new Guid("22222222-2222-2222-2222-222222222231"),
            new Guid("22222222-2222-2222-2222-222222222232"),
            new Guid("22222222-2222-2222-2222-222222222233")
                };
                    foreach (var purchaseDetails in model.PurchaseOrderDetail.GroupBy(x => x.EquipmentId).Select(g => g.First()))
                    {
                        if(!Array.Exists(excludedTechnicianIds, id => id == model.PurchaseOrderWarehouse.POFor))
                        {
                            continue;
                        }
                        _Util.Facade.InventoryFacade.InsertInventoryPO(new InventoryAssignedPO()
                        {
                            EquipmentId = purchaseDetails.EquipmentId.ToString(),
                            IsApprove = true,
                            IsDecline = false,
                            Description = model.PurchaseOrderWarehouse.Description,
                            IsReceived = false,
                            Quantity = Convert.ToInt32(purchaseDetails.CurrentQty),
                            ReceivedBy = model.PurchaseOrderWarehouse.POFor.ToString(),
                            ReceivedDate = DateTime.Now,
                            TechnicianId = model.TechnicianId.ToString(), //   TechnicianId = new Guid("22222222-2222-2222-2222-222222222221"),
                            CreatedBy = CurrentUser.UserId.ToString(),
                            ClosedBy = CurrentUser.UserId.ToString()

                        });
                    }
                }
            }
            return Json(new { result = true, message = "" });
        }



        [Authorize]
        public ActionResult ReceivePOHistory(int Id)
        {
            var ReceivePOHistoryList = _Util.Facade.PurchaseOrderFacade.GetReceivePOHistoryByPurchaseOrderId(Id);
            return View(ReceivePOHistoryList);
        }
        #endregion

        #region RequestOrderList
        [Authorize]
        public ActionResult RequestOrderList()
        {


            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            DateTime defaultTime = DateTime.Now.UTCCurrentTime();

            //List<SelectListItem> supList = _Util.Facade.SupplierFacade.GetAllSupplier().OrderBy(m => m.Name).Select(x =>
            //    new SelectListItem()
            //    {
            //        Text = x.CompanyName.ToString(),
            //        Value = x.SupplierId.ToString()
            //    }).ToList();
            //supList.Insert(0, new SelectListItem()
            //{
            //    Text = "Select Vendor",
            //    Value = new Guid().ToString()
            //});

            //ViewBag.SupplierList = supList.OrderBy(x => x.Text != "Select Vendor").ThenBy(x => x.Text).ToList();


            List<Supplier> supList = _Util.Facade.SupplierFacade.GetAllSupplier();
            ViewBag.SupplierList = supList.OrderBy(x => x.CompanyName).ThenBy(x => x.CompanyName).ToList();
            List<SelectListItem> shortcut = _Util.Facade.LookupFacade.GetLookupByKey("PTOFilterOptions").Select(x =>
               new SelectListItem()
               {
                   Text = x.DisplayText.ToString(),
                   Value = x.DataValue.ToString()
               }).ToList();
            shortcut.Add(new SelectListItem()
            {
                Text = "Next Week",
                Value = "NextWeek"
            });
            List<Employee> TechnicianList = _Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(CurrentUser.CompanyId.Value, LabelHelper.UserTags.Technicians, new Guid()).ToList();
            List<Employee> TechList = new List<Employee>();
            foreach (var techItem in TechnicianList)
            {
                techItem.EmployeeAddress = MakeAddress(techItem.Street, techItem.City, techItem.State, techItem.ZipCode);
                TechList.Add(techItem);
            }
            List<SelectListItem> PickupShipped = _Util.Facade.LookupFacade.GetLookupByKey("RequestOrderList").Select(x =>
              new SelectListItem()
              {
                  Text = x.DisplayText.ToString(),
                  Value = x.DataValue.ToString()
              }).ToList();
            PickupShipped.Insert(0, new SelectListItem()
            {
                Text = "Select Pickup/Shipped",
                Value = "-1"
            });
            ViewBag.PickupShipped = PickupShipped;
            //List<SelectListItem> AssignedEmployeeList = TechnicianList.Select(x =>
            //    new SelectListItem()
            //    {
            //        Text = x.FirstName + " " + x.LastName,
            //        Value = x.UserId.ToString(),
            //    }).ToList();
            #endregion

            #region assigned employee
            //AssignedEmployeeList.Insert(0, new SelectListItem()
            //{
            //    Text = "Select Technician",
            //    Value = Guid.Empty.ToString()
            //});
            //AssignedEmployeeList.Insert(1, new SelectListItem()
            //{
            //    Text = "System User",
            //    Value = "22222222-2222-2222-2222-222222222222",
            //});
            ViewBag.AssignedEmployee = TechList.OrderBy(x => x.FirstName).ToList();
            ViewBag.DefaultTime = defaultTime;
            ViewBag.Shortcut = shortcut.OrderBy(x => x.Text != "Select Shortcut").ThenBy(x => x.Text).ToList();
            return View();
        }
        public static string MakeAddress(string street, string city, string state, string zip)
        {
            string address = "";
            address += street;
            if (!string.IsNullOrEmpty(city))
            {
                address += string.Format(" {0}", city.CapitalizeFirst());
            }
            if (!string.IsNullOrEmpty(address) && (!string.IsNullOrEmpty(state) || !string.IsNullOrEmpty(zip)))
            {
                address += ", ";
            }
            address += string.Format("{0} {1}", state, zip);
            return address;
        }
        //public static string MakeSupplierAddress(Supplier sup)
        //{
        //    string address = "";
        //    address += sup.Street;
        //    if (!string.IsNullOrEmpty(sup.City))
        //    {
        //        address += string.Format(" {0}", sup.City.CapitalizeFirst());
        //    }
        //    if (!string.IsNullOrEmpty(address) && (!string.IsNullOrEmpty(sup.State) || !string.IsNullOrEmpty(sup.Zipcode)))
        //    {
        //        address += ", ";
        //    }
        //    address += string.Format("{0} {1}", sup.State, sup.Zipcode);
        //    return address;
        //}
        [Authorize]
        public PartialViewResult RequestOrderListPartial(DateTime? startDate, DateTime? endDate, Guid? userId)
        {
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (currentLoggedIn == null)
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }

            List<CustomerAppointmentEquipment> model = new List<CustomerAppointmentEquipment>();
            if (startDate != null && endDate != null && userId != null)
            {
                model = _Util.Facade.PurchaseOrderFacade.GetRequestOrderListByFilter(startDate.Value, endDate.Value, userId.Value);
            }
            else if (userId != null && ((startDate == new DateTime() && endDate == new DateTime()) || (startDate == null && endDate == null)))
            {
                var strDate = _Util.Facade.GlobalSettingsFacade.GetGlobalsettingBySearchKeyAndCompanyId("CompanyStartDate", currentLoggedIn.CompanyId.Value);
                startDate = strDate.Value.ToDateTime();
                endDate = DateTime.Now;
                model = _Util.Facade.PurchaseOrderFacade.GetRequestOrderListByFilter(startDate.Value, endDate.Value, userId.Value);
            }
            return PartialView("RequestOrderListPartial", model);
        }
        public PartialViewResult SendEmailRequisition(Guid? SupplierId, Guid? TechnicianId, string PickupShipped, string FullfillmentDate)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            CreateCustomerRequisition model = new CreateCustomerRequisition();
            var objcom = _Util.Facade.CompanyFacade.GetAllCompanyByCompanyId(CurrentUser.CompanyId.Value).FirstOrDefault();
            if (TechnicianId.Value != Guid.Empty)
            {
                Employee objTech = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(TechnicianId.Value);
                model.CcSendEmailAddress = objTech.Email;
            }
            if (SupplierId.Value != Guid.Empty)
            {
                Supplier objSup = _Util.Facade.SupplierFacade.GetSupplierBySupplierId(SupplierId.Value);
                model.SendEmailAddress = objSup.EmailAddress;
                model.SendContactNumber = objSup.Phone;
                ViewBag.MailSendToName = objSup.CompanyName;
            }
            if (objcom != null)
            {
                model.CompanyName = objcom.CompanyName;
                model.CompanyEmail = objcom.EmailAdress;
            }
            model.ShortUrl = string.Concat(AppConfig.ShortSiteDomain, "/shrt/");
            if (Session[SessionKeys.RequisitionPdfSession] != null)
            {
                List<RequisitionSessionModel> ModelList = (List<RequisitionSessionModel>)Session[SessionKeys.RequisitionPdfSession];
                ViewBag.pdfLocation = AppConfig.DomainSitePath + "/" + ModelList.Where(x => x.UserId == CurrentUser.UserId.ToString()).Select(x => x.FileName).FirstOrDefault();
            }
            model.SMSBody = string.Concat("New Purchase Requisition from", " ", model.CompanyName, Environment.NewLine
                , Environment.NewLine, model.ShortUrl, "##url##");
            model.EmailTemplate = _Util.Facade.MailFacade.GetTemplateByTemplateKey("RequisitionPredefineEmailTemplate");
            var objemp = _Util.Facade.EmployeeFacade.GetEmployeeByEmployeeId(CurrentUser.UserId);
            if (objemp != null)
            {
                ViewBag.SalesGuy = objemp.FirstName + " " + objemp.LastName;
                if (!string.IsNullOrWhiteSpace(objemp.Phone))
                {
                    ViewBag.SalesPhone = objemp.Phone;
                }
                else
                {
                    ViewBag.SalesPhone = objcom.Phone;
                }
            }
            string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(CurrentUser.CompanyId.Value + "#");
            string fullurl = string.Concat(AppConfig.SiteDomain, "/Requisition-Order/", encryptedurl);
            ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, CurrentUser.CompanyId.Value);
            ViewBag.url = AppConfig.ShortSiteDomain + "/shrt/" + ShortUrl.Code;
            return PartialView("SendEmailRequisition", model);
        }
        [HttpPost]
        public JsonResult AddMailRequestOrderList(CreateCustomerAppoinmentEquipment CreateCustomerAppoinmentEquipment, string ccEmail)
        {
            bool EmailSent = false;
            double totalAmount = 0.0;
            string POID = "";
            var currentLoggedIn = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            Company tempCom = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(currentLoggedIn.CompanyId.Value);
            tempCom.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(currentLoggedIn.CompanyId.Value);
            ViewBag.CompanyId = tempCom.CompanyId.ToString();
            CreateCustomerAppoinmentEquipment.EmployeeInfo = _Util.Facade.GlobalSettingsFacade.GetEmployeeAddressFormat(currentLoggedIn.CompanyId.Value);
            CreateCustomerAppoinmentEquipment.SupplierInfo = _Util.Facade.GlobalSettingsFacade.GetSupplierAddressFormat(currentLoggedIn.CompanyId.Value);
            CreateCustomerAppoinmentEquipment.CompanyLogo = tempCom.CompanyLogo;
            CreateCustomerAppoinmentEquipment.CompanyAddress = tempCom.Address;
            CreateCustomerAppoinmentEquipment.CompanyStreet = tempCom.Street;
            string ComCity = "";
            string ComState = "";
            if (!string.IsNullOrWhiteSpace(tempCom.City))
            {
                ComCity = tempCom.City.UppercaseFirst() + ", ";
            }
            if (!string.IsNullOrWhiteSpace(tempCom.State))
            {
                ComState = tempCom.State + " ";
            }
            CreateCustomerAppoinmentEquipment.companyStreetInfo = ComCity + ComState + tempCom.ZipCode;
            CreateCustomerAppoinmentEquipment.CompanyCity = tempCom.City.UppercaseFirst();
            CreateCustomerAppoinmentEquipment.CompanyState = tempCom.State;
            CreateCustomerAppoinmentEquipment.CompanyZip = tempCom.ZipCode;
            CreateCustomerAppoinmentEquipment.CompanyPhone = tempCom.Phone;
            CreateCustomerAppoinmentEquipment.CompanyEmail = tempCom.EmailAdress;
            CreateCustomerAppoinmentEquipment.CompanyName = tempCom.CompanyName;
            CreateCustomerAppoinmentEquipment.PhoneNum = tempCom.Phone;
            CreateCustomerAppoinmentEquipment.CompanyWebsite = tempCom.Website;
            CreateCustomerAppoinmentEquipment.CompanyInfo = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(currentLoggedIn.CompanyId.Value);
            if (CreateCustomerAppoinmentEquipment.PickupShipped == "Pickup")
            {
                CreateCustomerAppoinmentEquipment.ShowShippingAddress = false;
                CreateCustomerAppoinmentEquipment.ShippingAddress = "";
            }
            else if (CreateCustomerAppoinmentEquipment.PickupShipped == "Shipped")
            {
                CreateCustomerAppoinmentEquipment.ShowShippingAddress = true;
            }
            if (CreateCustomerAppoinmentEquipment.TechnicianId != null || CreateCustomerAppoinmentEquipment.TechnicianId != Guid.Empty)
            {
                CreateCustomerAppoinmentEquipment.TechnicianModel = _Util.Facade.EmployeeFacade.GetEmployeeByUserId(CreateCustomerAppoinmentEquipment.TechnicianId);
            }
            if (CreateCustomerAppoinmentEquipment.SupplierId != null || CreateCustomerAppoinmentEquipment.SupplierId != Guid.Empty)
            {
                CreateCustomerAppoinmentEquipment.SupplierModel = _Util.Facade.SupplierFacade.GetSupplierBySupplierId(CreateCustomerAppoinmentEquipment.SupplierId);
            }
            if (CreateCustomerAppoinmentEquipment.SendMail)
            {
                string mailFilename = "";
                if (Session[SessionKeys.RequisitionPdfSession] != null)
                {
                    List<RequisitionSessionModel> ModelList = (List<RequisitionSessionModel>)Session[SessionKeys.RequisitionPdfSession];
                    mailFilename = AppConfig.DomainSitePath + "/" + ModelList.Where(x => x.UserId == currentLoggedIn.UserId.ToString()).Select(x => x.FileName).FirstOrDefault();
                }
                if (string.IsNullOrWhiteSpace(mailFilename))
                {
                    ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("~/Views/PurchaseOrder/RequisitionListPdf.cshtml", CreateCustomerAppoinmentEquipment)
                    {
                        PageSize = Size.A4,
                        PageOrientation = Rotativa.Options.Orientation.Portrait,
                        PageMargins = { Left = 1, Right = 1 },
                    };
                    string pdfname = "";
                    Random rand = new Random();
                    pdfname = "RequisitionList_" + rand.Next().ToString();
                    byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
                    string filename = ConfigurationManager.AppSettings["File.RequisitionFiles"];
                    var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(currentLoggedIn.CompanyId.Value).CompanyName.ReplaceSpecialChar();
                    filename = string.Format(filename, comname);
                    filename += DateTime.Now.UTCCurrentTime().Year.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month.ToString() + "/" + pdfname + ".pdf";
                    string Serverfilename = FileHelper.GetFileFullPath(filename);
                    FileHelper.SaveFile(applicationPDFData, Serverfilename);
                    List<RequisitionSessionModel> ModelList = new List<RequisitionSessionModel>();
                    ModelList.Add(new RequisitionSessionModel
                    {
                        FileName = filename,
                        UserId = currentLoggedIn.UserId.ToString()
                    });
                    Session[SessionKeys.RequisitionPdfSession] = ModelList;
                    mailFilename = AppConfig.DomainSitePath + "/" + filename;
                }
                RequisitionCreatedEmail email = new RequisitionCreatedEmail()
                {
                    CompanyName = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(currentLoggedIn.CompanyId.Value).CompanyName,
                    //CustomerName = Model.Invoice.CustomerName,
                    //BalanceDue = Model.Invoice.TotalAmount != null ? LabelHelper.CurrentTransMakeCurrency.MakeCurrency() + Model.Invoice.TotalAmount.Value.ToString("0,0.00") : "0.00",
                    //DueDate = Model.Invoice.DueDate.HasValue ? Model.Invoice.DueDate.Value.ToString("MM/dd/yy") : "",
                    //InvoiceId = Model.Invoice.InvoiceId,
                    ToEmail = CreateCustomerAppoinmentEquipment.EmailAddress,
                    EmailBody = CreateCustomerAppoinmentEquipment.EmailDescription,
                    ccEmail = ccEmail,
                    FromEmail = currentLoggedIn.EmailAddress.IsValidEmailAddress() ? currentLoggedIn.EmailAddress : "info@rmrcloud.com",
                    FromName = string.Concat(currentLoggedIn.FirstName, " ", currentLoggedIn.LastName),
                    RequisitionPdf = new Attachment(
                                  FileHelper.GetFileFullPath(mailFilename),
                                 MediaTypeNames.Application.Octet)
                };
                EmailSent = _Util.Facade.MailFacade.SendRequisitionCreatedEmail(email, currentLoggedIn.CompanyId.Value);
                return Json(new { result = true, message = string.Concat("Successfully Emailed to ", CreateCustomerAppoinmentEquipment.EmailAddress), EmailSent = EmailSent });
            }
            else
            {
                #region Purchase Order Create
                if (CreateCustomerAppoinmentEquipment.TechnicianId == null)
                {
                    CreateCustomerAppoinmentEquipment.TechnicianId = new Guid("22222222-2222-2222-2222-222222222222");
                }
                CreatePurchaseOrder model = new CreatePurchaseOrder();
                string poPreText = "PO";
                GlobalSetting _poPretxt = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(currentLoggedIn.CompanyId.Value, "PrefixForPurchaseOrderId");
                if (_poPretxt != null && !string.IsNullOrWhiteSpace(_poPretxt.Value) && _poPretxt.Value != "")
                {
                    poPreText = _poPretxt.Value;
                }
                model.PurchaseOrderWarehouse = new PurchaseOrderWarehouse()
                {
                    SuplierId = CreateCustomerAppoinmentEquipment.SupplierId,
                    POFor = CreateCustomerAppoinmentEquipment.TechnicianId,
                    CompanyId = currentLoggedIn.CompanyId.Value,
                    Status = LabelHelper.PurchaseOrderStatus.Created,
                    IsReceived = false,
                    CreatedByUid = currentLoggedIn.UserId,
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                    LastUpdatedByUid = currentLoggedIn.UserId,
                    Amount = 0.0,
                    Balance = 0.0,
                    BalanceDue = 0.0,
                    BillingAddress = "",
                    TotalAmount = 0.0,
                    Tax = 0.0,
                    Deposit = 0.0,
                    ShippingCost = 0.0,
                    ShippingAddress = CreateCustomerAppoinmentEquipment.ShippingAddress,
                    TaxType = "",
                    Message = "",
                    ShippingVia = "",
                    Description = "",
                    TrackingNo = "",
                    RecieveByUid = new Guid(),
                    RecieveForUid = currentLoggedIn.UserId,
                    ShippingDate = CreateCustomerAppoinmentEquipment.FullfillmentDate.ToDateTime().UTCCurrentTime(),
                    OrderDate = DateTime.Now.UTCCurrentTime(),
                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                };
                model.PurchaseOrderWarehouse.Id = _Util.Facade.PurchaseOrderFacade.InsertPurchaseOrderWarehouse(model.PurchaseOrderWarehouse);
                model.PurchaseOrderWarehouse.PurchaseOrderId = model.PurchaseOrderWarehouse.Id.GeneratePONo(poPreText);
                _Util.Facade.PurchaseOrderFacade.UpdatePurchaseOrderWarehouse(model.PurchaseOrderWarehouse);

                CreateCustomerAppoinmentEquipment.POId = model.PurchaseOrderWarehouse.PurchaseOrderId;

                foreach (var pod in CreateCustomerAppoinmentEquipment.CusAppoinmentEquipmentList)
                {
                    Equipment eq = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentId(pod.EquipmentId);
                    EquipmentVendor ev = _Util.Facade.EquipmentFacade.GetEquipmentVendorByEquipmentIdAndIsPrimary(pod.EquipmentId);
                    PurchaseOrderDetail purDetl = new PurchaseOrderDetail();
                    purDetl.EquipmentId = pod.EquipmentId;
                    purDetl.EquipName = eq.Name;
                    purDetl.EquipDetail = eq.SKU;
                    purDetl.Quantity = pod.OrderingQuantity;
                    purDetl.UnitPrice = ev != null ? ev.Cost : eq.SupplierCost;
                    purDetl.TotalPrice = pod.OrderingQuantity * (ev != null ? ev.Cost : eq.SupplierCost);
                    purDetl.CreatedDate = DateTime.Now.UTCCurrentTime();
                    purDetl.CreatedBy = currentLoggedIn.UserId;
                    purDetl.BundleId = 0;
                    purDetl.PurchaseOrderId = model.PurchaseOrderWarehouse.PurchaseOrderId;
                    totalAmount = totalAmount + purDetl.TotalPrice.Value;
                    _Util.Facade.PurchaseOrderFacade.InsertPurchaseOrderDetail(purDetl);
                }
                model.PurchaseOrderWarehouse.Amount = totalAmount;
                model.PurchaseOrderWarehouse.TotalAmount = totalAmount;
                _Util.Facade.PurchaseOrderFacade.UpdatePurchaseOrderWarehouse(model.PurchaseOrderWarehouse);
                #endregion

                ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("~/Views/PurchaseOrder/RequisitionListPdf.cshtml", CreateCustomerAppoinmentEquipment)
                {
                    PageSize = Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    PageMargins = { Left = 1, Right = 1 },
                };
                string pdfname = "";
                Random rand = new Random();
                pdfname = "RequisitionList_" + rand.Next().ToString();
                byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
                string filename = ConfigurationManager.AppSettings["File.RequisitionFiles"];
                var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(currentLoggedIn.CompanyId.Value).CompanyName.ReplaceSpecialChar();
                filename = string.Format(filename, comname);
                filename += DateTime.Now.UTCCurrentTime().Year.ToString() + "/" + DateTime.Now.UTCCurrentTime().Month.ToString() + "/" + pdfname + ".pdf";
                string Serverfilename = FileHelper.GetFileFullPath(filename);
                FileHelper.SaveFile(applicationPDFData, Serverfilename);
                List<RequisitionSessionModel> ModelList = new List<RequisitionSessionModel>();
                ModelList.Add(new RequisitionSessionModel
                {
                    FileName = filename,
                    UserId = currentLoggedIn.UserId.ToString()
                });
                Session[SessionKeys.RequisitionPdfSession] = ModelList;
            }
            return Json(true);
        }

        #endregion

        #region DemandOrder
        [Authorize]
        public ActionResult AddDemandOrder(int? Id, string PurchaseOrderId, bool? Receive, string OpenTab, Guid? EmployeeId, int? BranchId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var BranchList = _Util.Facade.CompanyBranchFacade.GetAllCompanyBranchByCompanyId(CurrentUser.CompanyId.Value);
            CreatePurchaseOrder model = new CreatePurchaseOrder();

            if (Id.HasValue && Id > 0 || (!string.IsNullOrWhiteSpace(PurchaseOrderId)))
            {
                if (Id.HasValue && Id > 0)
                {
                    model.PurchaseOrderTech = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderTechById(Id.Value);
                }
                else
                {
                    model.PurchaseOrderTech = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderTechByPurchaseOrderId(PurchaseOrderId);
                }

                if (model.PurchaseOrderTech == null)
                {
                    return PartialView("~/Views/Shared/_NotFound.cshtml");
                }

                model.PurchaseOrderDetail = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderDetailListByPurchaseOrderId(model.PurchaseOrderTech.DemandOrderId);

            }
            else
            {
                Company Company = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value);
                Hashtable datatemplate = new Hashtable();
                datatemplate.Add("ComapnyName", Company.CompanyName);
                datatemplate.Add("Address", Company.Address);
                datatemplate.Add("Street", Company.Street);
                datatemplate.Add("City", Company.City);
                datatemplate.Add("State", Company.State);
                datatemplate.Add("Zip", Company.ZipCode);
                datatemplate.Add("CompanyPhone", Company.Phone);
                datatemplate.Add("EmailAddress", Company.EmailAdress);
                datatemplate.Add("WebAddress", Company.Website);

                string CompanyInfo = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(CurrentUser.CompanyId.Value);
                CompanyInfo = HS.Web.UI.Helper.LabelHelper.ParserHelper(CompanyInfo, datatemplate);

                if (OpenTab == "Tech")
                {
                    var BranchIdInner = 0;
                    if (EmployeeId == null)
                    {
                        EmployeeId = CurrentUser.UserId;
                        var BranchInfo = _Util.Facade.EmployeeFacade.GetUserBranchByCompanyIdAndUserId(CurrentUser.CompanyId.Value, CurrentUser.UserId);
                        if (BranchInfo != null)
                        {
                            BranchIdInner = BranchInfo.BranchId;
                        }
                    }
                    else
                    {
                        var BranchInfo = _Util.Facade.EmployeeFacade.GetUserBranchByCompanyIdAndUserId(CurrentUser.CompanyId.Value, EmployeeId.Value);
                        if (BranchInfo != null)
                        {
                            BranchIdInner = BranchInfo.BranchId;
                        }
                    }
                    model.PurchaseOrderTech = new PurchaseOrderTech()
                    {
                        CompanyId = CurrentUser.CompanyId.Value,
                        Status = LabelHelper.PurchaseOrderStatus.Init,
                        TechnicianId = EmployeeId.Value,
                        CreatedByUid = CurrentUser.UserId,
                        LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                        LastUpdatedByUid = CurrentUser.UserId,
                        CreatedDate = DateTime.Now.UTCCurrentTime(),
                    };
                    model.PurchaseOrderTech.Id = _Util.Facade.PurchaseOrderFacade.InsertPurchaseOrderTech(model.PurchaseOrderTech);
                    model.PurchaseOrderTech.DemandOrderId = model.PurchaseOrderTech.Id.GenerateDONoTech();
                    _Util.Facade.PurchaseOrderFacade.UpdatePurchaseOrderTech(model.PurchaseOrderTech);

                    model.PurchaseOrderBranch = new PurchaseOrderBranch()
                    {
                        CompanyId = CurrentUser.CompanyId.Value,
                        Status = LabelHelper.PurchaseOrderStatus.Init,
                        IsReceived = false,
                        BranchId = BranchIdInner,
                        CreatedByUid = CurrentUser.UserId,
                        LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                        LastUpdatedByUid = CurrentUser.UserId,
                        CreatedDate = DateTime.Now.UTCCurrentTime()
                    };
                    model.PurchaseOrderBranch.Id = _Util.Facade.PurchaseOrderFacade.InsertPurchaseOrderBranch(model.PurchaseOrderBranch);
                    model.PurchaseOrderBranch.DemandOrderId = model.PurchaseOrderBranch.Id.GenerateDONoBranch();
                    model.PurchaseOrderBranch.TechDemandOrderId = model.PurchaseOrderTech.DemandOrderId;
                    _Util.Facade.PurchaseOrderFacade.UpdatePurchaseOrderBranch(model.PurchaseOrderBranch);

                }
                else if (OpenTab == "Branch")
                {

                }
                model.PurchaseOrderDetail = new List<PurchaseOrderDetail>();
                model.Supplier = new Supplier();
            }

            #region ViewBags

            ViewBag.POShipVia = _Util.Facade.LookupFacade.GetLookupByKey("POShipVia").Select(x =>
                new SelectListItem()
                {
                    Text = x.DisplayText.ToString(),
                    Value = x.DataValue.ToString()
                }).ToList();

            List<SelectListItem> supList = _Util.Facade.SupplierFacade.GetAllSupplier().Select(x =>
              new SelectListItem()
              {
                  Text = x.CompanyName.ToString(),
                  Value = x.SupplierId.ToString()
              }).ToList();
            supList.Insert(0, new SelectListItem()
            {
                Text = "Select One",
                Value = new Guid().ToString()
            });

            ViewBag.SupplierList = supList.OrderBy(x => x.Text != "Select One").ThenBy(x => x.Text).ToList();

            List<Employee> EmpList = _Util.Facade.EmployeeFacade.GetAllEmployee(CurrentUser.CompanyId.Value);
            var emplst = new List<SelectListItem>();
            if (EmpList != null)
            {
                emplst = EmpList.Select(x =>
                new SelectListItem()
                {
                    Text = x.FirstName + " " + x.LastName,
                    Value = x.UserId.ToString(),
                }).ToList();
            }
            emplst.Insert(0, new SelectListItem()
            {
                Text = "Select One",
                Value = new Guid().ToString()
            });
            ViewBag.EmployeeList = emplst.OrderBy(x => x.Text != "Select One").ThenBy(x => x.Text).ToList();
            ViewBag.OpenTab = OpenTab;

            #endregion
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddDemandOrder(CreatePurchaseOrder model)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            _Util.Facade.PurchaseOrderFacade.DeleteAllPurchaseOrderDetailByPurchaseOrderId(model.PurchaseOrderTech.DemandOrderId);
            foreach (var item in model.PurchaseOrderDetail)
            {
                item.CreatedDate = DateTime.Now.UTCCurrentTime();
                item.CreatedBy = CurrentUser.UserId;
                item.BundleId = 0;
                item.PurchaseOrderId = model.PurchaseOrderTech.DemandOrderId;
                _Util.Facade.PurchaseOrderFacade.InsertPurchaseOrderDetail(item);
            }
            _Util.Facade.PurchaseOrderFacade.DeleteAllPurchaseOrderDetailByPurchaseOrderId(model.PurchaseOrderBranch.DemandOrderId);
            foreach (var item in model.PurchaseOrderDetail)
            {
                item.CreatedDate = DateTime.Now.UTCCurrentTime();
                item.CreatedBy = CurrentUser.UserId;
                item.BundleId = 0;
                item.PurchaseOrderId = model.PurchaseOrderBranch.DemandOrderId;
                _Util.Facade.PurchaseOrderFacade.InsertPurchaseOrderDetail(item);
            }

            var PurchaseOrderBrachDetails = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderBranchByPOId(model.PurchaseOrderBranch.DemandOrderId);
            if (PurchaseOrderBrachDetails != null)
            {
                PurchaseOrderBrachDetails.Status = LabelHelper.PurchaseOrderStatus.Created;
                PurchaseOrderBrachDetails.LastUpdatedByUid = CurrentUser.UserId;
                PurchaseOrderBrachDetails.LastUpdatedDate = DateTime.Now;
                _Util.Facade.PurchaseOrderFacade.UpdatePurchaseOrderBranch(PurchaseOrderBrachDetails);
            }

            var PurchaseOrderTechDetails = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderTechByPOId(model.PurchaseOrderTech.DemandOrderId);
            if (PurchaseOrderTechDetails != null)
            {
                PurchaseOrderTechDetails.Status = LabelHelper.PurchaseOrderStatus.Created;
                PurchaseOrderTechDetails.LastUpdatedByUid = CurrentUser.UserId;
                PurchaseOrderTechDetails.LastUpdatedDate = DateTime.Now;
                _Util.Facade.PurchaseOrderFacade.UpdatePurchaseOrderTech(PurchaseOrderTechDetails);
            }

            return Json(new { result = true, message = "", techId = PurchaseOrderTechDetails.TechnicianId });
        }

        [Authorize]
        [HttpPost]
        public JsonResult AddDemandOrderTicket(CreatePurchaseOrder model)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var BranchIdInner = 0;
            var BranchInfo = _Util.Facade.EmployeeFacade.GetUserBranchByCompanyIdAndUserId(CurrentUser.CompanyId.Value, CurrentUser.UserId);
            if (BranchInfo != null)
            {
                BranchIdInner = BranchInfo.BranchId;
            }
            model.PurchaseOrderTech = new PurchaseOrderTech()
            {
                CompanyId = CurrentUser.CompanyId.Value,
                Status = LabelHelper.PurchaseOrderStatus.Created,
                TechnicianId = model.TechnicianId,
                CreatedByUid = CurrentUser.UserId,
                LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                LastUpdatedByUid = CurrentUser.UserId,
                CreatedDate = DateTime.Now.UTCCurrentTime(),
                TicketId = model.TicketId
            };
            model.PurchaseOrderTech.Id = _Util.Facade.PurchaseOrderFacade.InsertPurchaseOrderTech(model.PurchaseOrderTech);
            model.PurchaseOrderTech.DemandOrderId = model.PurchaseOrderTech.Id.GenerateDONoTech();
            _Util.Facade.PurchaseOrderFacade.UpdatePurchaseOrderTech(model.PurchaseOrderTech);

            model.PurchaseOrderBranch = new PurchaseOrderBranch()
            {
                CompanyId = CurrentUser.CompanyId.Value,
                Status = LabelHelper.PurchaseOrderStatus.Created,
                IsReceived = false,
                BranchId = BranchIdInner,
                CreatedByUid = CurrentUser.UserId,
                LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                LastUpdatedByUid = CurrentUser.UserId,
                CreatedDate = DateTime.Now.UTCCurrentTime()
            };
            model.PurchaseOrderBranch.Id = _Util.Facade.PurchaseOrderFacade.InsertPurchaseOrderBranch(model.PurchaseOrderBranch);
            model.PurchaseOrderBranch.DemandOrderId = model.PurchaseOrderBranch.Id.GenerateDONoBranch();
            model.PurchaseOrderBranch.TechDemandOrderId = model.PurchaseOrderTech.DemandOrderId;
            _Util.Facade.PurchaseOrderFacade.UpdatePurchaseOrderBranch(model.PurchaseOrderBranch);

            _Util.Facade.PurchaseOrderFacade.DeleteAllPurchaseOrderDetailByPurchaseOrderId(model.PurchaseOrderTech.DemandOrderId);
            foreach (var item in model.PurchaseOrderDetail)
            {
                item.CreatedDate = DateTime.Now.UTCCurrentTime();
                item.CreatedBy = CurrentUser.UserId;
                item.BundleId = 0;
                item.PurchaseOrderId = model.PurchaseOrderTech.DemandOrderId;
                _Util.Facade.PurchaseOrderFacade.InsertPurchaseOrderDetail(item);
            }
            _Util.Facade.PurchaseOrderFacade.DeleteAllPurchaseOrderDetailByPurchaseOrderId(model.PurchaseOrderBranch.DemandOrderId);
            foreach (var item in model.PurchaseOrderDetail)
            {
                item.CreatedDate = DateTime.Now.UTCCurrentTime();
                item.CreatedBy = CurrentUser.UserId;
                item.BundleId = 0;
                item.PurchaseOrderId = model.PurchaseOrderBranch.DemandOrderId;
                _Util.Facade.PurchaseOrderFacade.InsertPurchaseOrderDetail(item);
            }
            return Json(new { result = true, message = "" });
        }
        #endregion

        [Authorize]
        [HttpPost]
        public JsonResult ApprovePO(int POId, string YesOrNo)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            var result = false;
            string message = "Purchase order approve succesfully";
            if (YesOrNo == "No")
            {
                message = "Purchase order unapprove succesfully";
            }
            if (POId != 0 && !string.IsNullOrEmpty(YesOrNo))
            {
                var PurchaseOrderDetails = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderById(POId);
                if (PurchaseOrderDetails != null)
                {
                    var PurchaseOrderId = PurchaseOrderDetails.PurchaseOrderId;
                    var PurchaseOrderBranchDetails = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderBranchByPOId(PurchaseOrderId);
                    if (PurchaseOrderBranchDetails != null)
                    {
                        if (YesOrNo == "Yes")
                        {
                            PurchaseOrderBranchDetails.Action = LabelHelper.PurchaseOrderAction.Approve;
                            PurchaseOrderBranchDetails.LastUpdatedDate = DateTime.Now;
                            PurchaseOrderBranchDetails.LastUpdatedByUid = CurrentUser.UserId;
                            _Util.Facade.PurchaseOrderFacade.UpdatePurchaseOrderBranch(PurchaseOrderBranchDetails);

                            var PurchaseOrderWarehouse = new PurchaseOrderWarehouse()
                            {
                                CompanyId = CurrentUser.CompanyId.Value,
                                PurchaseOrderId = PurchaseOrderId,
                                POFor = new Guid("22222222-2222-2222-2222-222222222222"),
                                Status = LabelHelper.PurchaseOrderStatus.Init,
                                IsReceived = false,
                                Action = LabelHelper.PurchaseOrderAction.RecieveOn,
                                CreatedByUid = CurrentUser.UserId,
                                LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                                LastUpdatedByUid = CurrentUser.UserId
                            };
                            _Util.Facade.PurchaseOrderFacade.InsertPurchaseOrderWarehouse(PurchaseOrderWarehouse);
                            result = true;
                        }
                        else if (YesOrNo == "No")
                        {
                            _Util.Facade.PurchaseOrderFacade.DeletePurchaseOrderBranchById(PurchaseOrderBranchDetails.Id);
                            result = true;
                        }
                    }
                }
            }
            return Json(new { result = result, message = message });
        }

        [Authorize]
        [HttpPost]
        public JsonResult SendPurchaseOrderEmail(int POId, string EmailSubject, string EmailBody, string ToEmail, string CcEmail)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            CreatePurchaseOrder Model = new CreatePurchaseOrder();
            Model.PurchaseOrderWarehouse = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderWarehouseById(POId);
            Model.PurchaseOrderDetail = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderDetailListByPurchaseOrderId(Model.PurchaseOrderWarehouse.PurchaseOrderId);
            Model.Company = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value);
            Model.Supplier = _Util.Facade.SupplierFacade.GetSupplierBySupplierId(Model.PurchaseOrderWarehouse.SuplierId);
            string ReplyEmail = CurrentUser.EmailAddress;
            if (!ReplyEmail.IsValidEmailAddress())
            {
                ReplyEmail = Model.Company.EmailAdress.IsValidEmailAddress() ? Model.Company.EmailAdress : "info@rmrcloud.com";
            }
            byte[] pdf = CreatePurchasOrderPdf(Model, null);
            Stream stream = new MemoryStream(pdf);
            PurchaseOrderCreatedEmail email = new PurchaseOrderCreatedEmail()
            {
                CompanyName = CurrentUser.CompanyName,
                FromName = CurrentUser.GetFullName(),
                PurchaseOrderPdf = new System.Net.Mail.Attachment(stream, Model.PurchaseOrderWarehouse.PurchaseOrderId + ".pdf"),
                EmailBody = EmailBody,
                Subject = EmailSubject,
                ToEmail = ToEmail,
                ccEmail = CcEmail,
                POId = Model.PurchaseOrderWarehouse.PurchaseOrderId,
                UserId = CurrentUser.UserId,
                OrderDate = Model.PurchaseOrderWarehouse.OrderDate == new DateTime() ? DateTime.Now.UTCCurrentTime().UTCToClientTime().ToString("MM/dd/yy") : Model.PurchaseOrderWarehouse.OrderDate.ToString("MM/dd/yy"),
                ReplyEmail = ReplyEmail,
                CompanyId = CurrentUser.CompanyId.Value

            };
            _Util.Facade.MailFacade.EmailToSupplierForPurchaseOrder(email);
            Model.PurchaseOrderWarehouse.Status = LabelHelper.PurchaseOrderStatus.SentToVendor;
            _Util.Facade.PurchaseOrderFacade.UpdatePurchaseOrderWarehouse(Model.PurchaseOrderWarehouse);

            return Json(new { result = true, message = "Email sent successfully." });
        }

        [Authorize]
        public JsonResult GetPurchaseOrderPdf(CreatePurchaseOrder Model, string purchaseid)
        {
            string FilePath = "";
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            //return File(CreatePurchasOrderPdf(Model), System.Net.Mime.MediaTypeNames.Application.Octet, "InvoiceList.pdf");
            string file = Convert.ToBase64String(CreatePurchasOrderPdf(Model, purchaseid));
            byte[] imageBytes = Convert.FromBase64String(file);
            string tempFolderName = ConfigurationManager.AppSettings["File.CustomerFiles"];
            tempFolderName = tempFolderName.TrimEnd('/');
            var comname = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value).CompanyName;
            tempFolderName = string.Format(tempFolderName, comname.ReplaceSpecialChar());
            tempFolderName += "/" + DateTime.Now.Month + "-" + DateTime.Now.Day;

            Random rand = new Random();
            string FileName = rand.Next().ToString();
            FileName += "-___" + "PurchaseOrder.pdf";
            string tempFolderPath = System.Web.Hosting.HostingEnvironment.MapPath("~/" + tempFolderName);
            if (FileHelper.CreateFolderIfNeeded(tempFolderPath))
            {
                try
                {
                    FilePath = Path.Combine(tempFolderPath, FileName);
                    System.IO.File.WriteAllBytes(FilePath, Convert.FromBase64String(file));
                }
                catch (Exception ec)
                {
                    logger.Error(ec);
                }
            }
            string filePath = string.Concat("/", tempFolderName, "/", FileName);
            return Json(new { result = true, data = filePath });
        }

        [Authorize]
        public ActionResult GetPurchaseOrder()
        {
            return View();
        }

        [Authorize]
        public ActionResult SendEmailPurchaseOrder(int PoId)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            CreatePurchaseOrder Model = new CreatePurchaseOrder();
            Model.PurchaseOrderWarehouse = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderWarehouseById(PoId);
            if (Model.PurchaseOrderWarehouse == null)
            {
                return PartialView("~/Views/Shared/_NotFound.cshtml");
            }
            Model.PurchaseOrderDetail = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderDetailListByPurchaseOrderId(Model.PurchaseOrderWarehouse.PurchaseOrderId);
            if (Model.PurchaseOrderDetail == null || Model.PurchaseOrderDetail.Count() == 0)
            {
                return PartialView("~/Views/Shared/_NotFound.cshtml");
            }
            Model.Supplier = _Util.Facade.SupplierFacade.GetSupplierBySupplierId(Model.PurchaseOrderWarehouse.SuplierId);
            if (Model.Supplier == null)
            {
                //return PartialView("~/Views/Shared/_NotFound.cshtml");
                Model.Supplier = new Supplier();
            }

            Model.Company = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value);


            ViewBag.pdfData = Convert.ToBase64String(CreatePurchasOrderPdf(Model, null));


            #region UrlMakingPart

            string encryptedurl = DESEncryptionDecryption.EncryptPlainTextToCipherText(Model.PurchaseOrderWarehouse.PurchaseOrderId
                                        + "#"
                                        + CurrentUser.CompanyId.Value
                                        + "#"
                                        + Model.Supplier.SupplierId);
            string fullurl = string.Concat(AppConfig.SiteDomain, "/Supplier-PO/", encryptedurl);
            ShortUrl ShortUrl = _Util.Facade.ShortUrlFacade.GetSortUrlByUrl(fullurl, null);
            #endregion

            #region SMSPart
            ViewBag.SMSBody = string.Concat("New Purchase Order from", " ", Model.Company.CompanyName, ": ", Model.PurchaseOrderWarehouse.PurchaseOrderId, Environment.NewLine
                , Environment.NewLine, Model.ShortUrl, AppConfig.ShortSiteDomain + "/shrt/" + ShortUrl.Code);
            #endregion
            #region Email Part 
            string poPreText = "PO";
            GlobalSetting _poPretxt = _Util.Facade.GlobalSettingsFacade.GetGlobalSettingsByKey(CurrentUser.CompanyId.Value, "PrefixForPurchaseOrderId");
            if (_poPretxt != null && !string.IsNullOrWhiteSpace(_poPretxt.Value) && _poPretxt.Value != "")
            {
                poPreText = _poPretxt.Value;
            }
            EmailTemplate template = _Util.Facade.MailFacade.GetTemplateByTemplateKey(EmailTemplateKey.PredefinedTemplates.POPredefineEmailTemplate);
            Hashtable datatemplate = new Hashtable();
            datatemplate.Add("SupplierName", Model.Supplier.CompanyName);
            datatemplate.Add("PrefixForPOId", poPreText);
            if (!string.IsNullOrWhiteSpace(CurrentUser.PhoneNumber))
            {
                datatemplate.Add("PhoneNumber", CurrentUser.PhoneNumber);
            }
            else
            {
                datatemplate.Add("PhoneNumber", Model.Company.Phone);

            }
            datatemplate.Add("SenderName", CurrentUser.FirstName + " " + CurrentUser.LastName);
            datatemplate.Add("CompanyName", Model.Company.CompanyName);
            datatemplate.Add("url", AppConfig.ShortSiteDomain + "/shrt/" + ShortUrl.Code);

            ViewBag.EmailBody = HS.Web.UI.Helper.LabelHelper.ParserHelper(template.BodyContent, datatemplate);

            #endregion

            return View(Model);
        }
        [Authorize]
        [HttpPost]
        public JsonResult DeletePurchaseOrder(int Id)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            PurchaseOrderWarehouse POW = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderWarehouseById(Id);
            if (POW != null)
            {
                if (POW.Status == LabelHelper.PurchaseOrderStatus.Received)
                {
                    return Json(new { result = false, message = "Purchase Order already received. If you still need to delete this PO please contact system admin." });
                }
                else
                {
                    _Util.Facade.PurchaseOrderFacade.DeletePurchaseOrderWareById(Id);
                    _Util.Facade.PurchaseOrderFacade.DeleteAllPurchaseOrderDetailByPurchaseOrderId(POW.PurchaseOrderId);
                    return Json(new { result = true, message = "Purchase Order deleted successfully." });
                }
            }
            return Json(new { result = false, message = "Purchase Order not found." });
        }

        [Authorize]
        [HttpPost]
        public JsonResult CreatePurchaseOrderForLowStockProducts()
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;

            List<Equipment> lowstockEquipments = _Util.Facade.EquipmentFacade.GetAllLowsStockProductsByComapnyId(CurrentUser.CompanyId.Value);

            return Json(true);
        }

        [Authorize]
        [HttpPost]
        public JsonResult CreateBillFromPO(string POId, Guid supid)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            Supplier sup = new Supplier();
            PurchaseOrderWarehouse PO = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderByPurchaseOrderId(POId);
            if (PO == null)
            {
                return Json(new { result = false, message = "PO not found." });
            }
            if (PO.Status == LabelHelper.PurchaseOrderStatus.BillCreated)
            {
                return Json(new { result = false, message = "Bill already created for this PO." });
            }
            else if (PO.Status != LabelHelper.PurchaseOrderStatus.Received)
            {
                return Json(new { result = false, message = "Products of this purchase order not received yet." });
            }

            List<PurchaseOrderDetail> podlist = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderDetailListByPurchaseOrderId(POId);

            if (podlist == null || podlist.Count() == 0)
            {
                return Json(new { result = false, message = "No items found." });
            }

            if (PO.SuplierId != new Guid())
            {
                sup = _Util.Facade.SupplierFacade.GetSupplierBySupplierId(PO.SuplierId);
            }
            else
            {
                sup = _Util.Facade.SupplierFacade.GetSupplierBySupplierId(supid);
            }

            Bill bill = new Bill()
            {
                BillNo = "",
                CompanyId = CurrentUser.CompanyId.Value,
                SupplierId = sup != null ? sup.Id : 0,
                Type = "",
                Amount = PO.Amount,
                PaymentMethod = "",
                PaymentStatus = "Open",
                PaymentDate = DateTime.Now.UTCCurrentTime(),
                PaymentDueDate = DateTime.Now.UTCCurrentTime(),
                BillCycle = "",
                Notes = "",
                UpdatedBy = User.Identity.Name,
                UpdatedDate = DateTime.Now.UTCCurrentTime(),
                PaymentDue = PO.TotalAmount,
                SupplierAddress = PO.BillingAddress,
                PurchaseOrderId = PO.PurchaseOrderId
            };
            bill.Id = _Util.Facade.BillFacade.InsertBill(bill);
            bill.BillNo = "Bill-00" + bill.Id.ToString();
            _Util.Facade.BillFacade.UpdateBill(bill);

            foreach (var item in podlist)
            {
                BillDetail bd = new BillDetail()
                {
                    CustomerBillId = bill.Id,
                    Quantity = item.Quantity,
                    Amount = item.TotalPrice,
                    AccoutTypeId = 14,
                    Dscription = item.EquipDetail,
                    EquipmentName = item.EquipName,
                    EquipmentId = item.EquipmentId,
                    EquipmentDescription = item.EquipDetail
                };
                _Util.Facade.BillFacade.InsertBillDetail(bd);
            }
            PO.Status = LabelHelper.PurchaseOrderStatus.BillCreated;
            _Util.Facade.PurchaseOrderFacade.UpdatePurchaseOrderWarehouse(PO);

            return Json(new { result = true, message = string.Format("Bill {0} created", bill.BillNo) });
        }
        #region BadInventory
        [Authorize]
        public ActionResult BadInventoryList()
        {
            return View();
        }

        [Authorize]
        public ActionResult BadInventoryPartial(BadInventoryFilter filters)
        {
            return PartialView();
        }

        [Authorize]
        public ActionResult BadInventoryListPartial(BadInventoryFilter filters)
        {
            var CurrentUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));

            #region Permission Check
            if (!base.IsPermitted(UserPermissions.InventoryPermissions.BadInventoryTab))
            {
                return PartialView("~/Views/Shared/_AccessDenied.cshtml");
            }
            #endregion

            #region Inits
            if (filters == null)
            {
                filters = new BadInventoryFilter();
            }

            if (filters.PageNo == 0)
            {
                filters.PageNo = 1;
            }
            filters.CompanyId = CurrentUser.CompanyId.Value;
            filters.PageSize = Convert.ToInt32(_Util.Facade.GlobalSettingsFacade.GetGlobalsettingBySearchKeyAndCompanyId("BadInventoryPageLimit", CurrentUser.CompanyId.Value).Value);
            BIListModel Model = new BIListModel();
            #endregion

            if (CurrentUser.UserTags.ToLower().IndexOf("technician") != 0)
            {
                Model = _Util.Facade.PurchaseOrderFacade.GetBadInventoryListByFilters(filters, new Guid());
            }
            else
            {
                Model = _Util.Facade.PurchaseOrderFacade.GetBadInventoryListByFilters(filters, CurrentUser.UserId);
            }
            ViewBag.PageNumber = filters.PageNo;
            ViewBag.OutOfNumber = 0;

            if (Model.EquipmentReturnList.Count() > 0)
            {
                ViewBag.OutOfNumber = Model.TotalCount;
            }

            if ((int)ViewBag.PageNumber * filters.PageSize > (int)ViewBag.OutOfNumber)
            {
                ViewBag.CurrentNumber = (int)ViewBag.OutOfNumber;
            }
            else
            {
                ViewBag.CurrentNumber = (int)ViewBag.PageNumber * filters.PageSize;
            }
            ViewBag.PageCount = Math.Ceiling((double)ViewBag.OutOfNumber / filters.PageSize);
            ViewBag.order = filters.order;
            return View(Model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult AddBadInventory(int? Id)
        {
            var Currentuser = (CustomPrincipal)(User);
            List<SelectListItem> EquipmentList = new List<SelectListItem>();
            EquipmentList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            ViewBag.EquipmentList = EquipmentList;
            List<SelectListItem> CustomerList = new List<SelectListItem>();
            CustomerList.Add(new SelectListItem()
            {
                Text = "Please Select",
                Value = "-1"
            });
            ViewBag.CustomerList = CustomerList;

            List<SelectListItem> TechnicianList = new List<SelectListItem>();
            if (Currentuser.UserTags.ToLower().IndexOf("technician") != 0)
            {
                TechnicianList.Add(new SelectListItem()
                {
                    Text = "Please Select One",
                    Value = new Guid().ToString()
                });
                TechnicianList.AddRange(_Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(Currentuser.CompanyId.Value, LabelHelper.UserTags.Technicians, new Guid()).OrderBy(x => x.FirstName != "Please Select One").ThenBy(x => x.FirstName).Select(x =>
                                           new SelectListItem()
                                           {
                                               Text = x.FirstName + " " + x.LastName,
                                               Value = x.UserId.ToString(),
                                               Selected = true
                                           }).ToList());

            }
            else
            {
                TechnicianList.AddRange(_Util.Facade.EmployeeFacade.GetEmployeeByCompanyIdAndTag(Currentuser.CompanyId.Value, LabelHelper.UserTags.Technicians, Currentuser.UserId).OrderBy(x => x.FirstName != "Please Select One").ThenBy(x => x.FirstName).Select(x =>
                                           new SelectListItem()
                                           {
                                               Text = x.FirstName + " " + x.LastName,
                                               Value = x.UserId.ToString(),
                                               Selected = true
                                           }).ToList());
            }
            ViewBag.techlist = TechnicianList.OrderBy(x => x.Text != "Please Select One").ThenBy(x => x.Text).ToList();
            List<SelectListItem> badstatus = new List<SelectListItem>();
            badstatus.AddRange(_Util.Facade.LookupFacade.GetLookupByKey("BadInventoryStatus").OrderBy(x => x.DisplayText != "Please Select One").ThenBy(x => x.DisplayText).Select(x =>
            new SelectListItem()
            {
                Text = x.DisplayText.ToString(),
                Value = x.DataValue.ToString()
            }).ToList());
            ViewBag.badStatus = badstatus;

            var equipmentReturn = new EquipmentReturn();
            if (Id > 0)
            {
                equipmentReturn = _Util.Facade.PurchaseOrderFacade.GetEquipmentReturnById(Id.Value);
                var equiments = _Util.Facade.EquipmentFacade.GetEquipmentByEquipmentIdAndCompanyId(equipmentReturn.EquipmentId, Currentuser.CompanyId.Value);
                var equipClass = _Util.Facade.EquipmentFacade.GetAllEquipmentClassByCompanyId(Currentuser.CompanyId.Value);
                var manufacaturer = _Util.Facade.ManufacturerFacade.GetById(equiments.ManufacturerId);
                if (manufacaturer != null)
                {
                    EquipmentList.Add(new SelectListItem()
                    {
                        Text = manufacaturer.Name.ToString() + "(" + equiments.Name.ToString() + ")",
                        Value = equiments.EquipmentId.ToString()
                    });
                }
                else
                {
                    manufacaturer = new Manufacturer();
                    EquipmentList.Add(new SelectListItem()
                    {
                        Text = "(" + equiments.Name.ToString() + ")",
                        Value = equiments.EquipmentId.ToString()
                    });
                }
                var Customers = _Util.Facade.CustomerFacade.GetCustomerByCustomerId(equipmentReturn.CustomerId);
                CustomerList.Add(new SelectListItem()
                {
                    Text = Customers.FirstName + " " + Customers.LastName,
                    Value = Customers.CustomerId.ToString()
                });
            }
            ViewBag.EquipmentList = EquipmentList;
            return View(equipmentReturn);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddBadInventory(EquipmentReturn equipmentReturn)
        {
            var CurrentUser = (CustomPrincipal)(User);
            string message = "";
            bool result = false;

            if (equipmentReturn.Id > 0)
            {
                EquipmentReturn oldEquipmentReturn = _Util.Facade.PurchaseOrderFacade.GetEquipmentReturnById(equipmentReturn.Id);
                oldEquipmentReturn.InvoiceNo = equipmentReturn.InvoiceNo;
                oldEquipmentReturn.LastUpdatedBy = CurrentUser.UserId;
                oldEquipmentReturn.LastUpdatedDate = DateTime.Now;
                oldEquipmentReturn.EquipmentId = equipmentReturn.EquipmentId;
                oldEquipmentReturn.TechnicianId = equipmentReturn.TechnicianId;
                oldEquipmentReturn.CustomerId = equipmentReturn.CustomerId;
                oldEquipmentReturn.Quantity = equipmentReturn.Quantity;
                oldEquipmentReturn.PurchaseDate = equipmentReturn.PurchaseDate;
                oldEquipmentReturn.Description = equipmentReturn.Description;
                oldEquipmentReturn.Status = equipmentReturn.Status;
                if ((DateTime.Now - Convert.ToDateTime(equipmentReturn.PurchaseDate)).TotalDays > 0)
                {
                    oldEquipmentReturn.WanrantyAvailable = true;
                }
                else
                {
                    oldEquipmentReturn.WanrantyAvailable = false;
                }
                result = _Util.Facade.PurchaseOrderFacade.UpdateEquipmentReturn(oldEquipmentReturn);
                if (result)
                {
                    // EquipmentReturnNote
                    EquipmentReturnNote equipmentReturnNote = _Util.Facade.PurchaseOrderFacade.GetByReturnId(oldEquipmentReturn.ReturnId);
                    equipmentReturnNote.ReturnId = oldEquipmentReturn.ReturnId;
                    equipmentReturnNote.Description = equipmentReturn.Description;
                    equipmentReturnNote.LastUpdatedBy = CurrentUser.UserId;
                    equipmentReturnNote.LastUpdatedDate = DateTime.Now;
                    _Util.Facade.PurchaseOrderFacade.UpdateEquipmentReturnNote(equipmentReturnNote);

                    // EquipmentReturnVendor
                    EquipmentReturnVendor equipmentReturnVendor = _Util.Facade.PurchaseOrderFacade.GetVendorByReturnId(oldEquipmentReturn.ReturnId);
                    equipmentReturnVendor.ReturnId = oldEquipmentReturn.ReturnId;
                    equipmentReturnVendor.Description = equipmentReturn.Description;
                    equipmentReturnVendor.Status = equipmentReturn.Status;
                    equipmentReturnNote.LastUpdatedBy = CurrentUser.UserId;
                    equipmentReturnNote.LastUpdatedDate = DateTime.Now;
                    _Util.Facade.PurchaseOrderFacade.UpdateEquipmentReturnVendor(equipmentReturnVendor);

                    message = "Updated Successfully";
                    return Json(new { result = result, message = message });
                }
                message = "Update Fail!!!";
                return Json(new { result = result, message = message });
            }
            var TotalDays = (DateTime.Now - Convert.ToDateTime(equipmentReturn.PurchaseDate)).TotalDays;
            if (TotalDays >= 0 && TotalDays <= 365)
            {
                equipmentReturn.WanrantyAvailable = true;
            }
            else
            {
                equipmentReturn.WanrantyAvailable = false;
            }
            equipmentReturn.LastUpdatedBy = CurrentUser.UserId;
            equipmentReturn.LastUpdatedDate = DateTime.Now;
            equipmentReturn.ReturnId = Guid.NewGuid();
            equipmentReturn.CompanyId = CurrentUser.CompanyId.Value;
            result = _Util.Facade.PurchaseOrderFacade.InsertEquipmentReturn(equipmentReturn);
            if (result)
            {
                // EquipmentReturnNote
                EquipmentReturnNote equipmentReturnNote = new EquipmentReturnNote();
                equipmentReturnNote.ReturnId = equipmentReturn.ReturnId;
                equipmentReturnNote.Description = equipmentReturn.Description;
                equipmentReturnNote.LastUpdatedBy = CurrentUser.UserId;
                equipmentReturnNote.LastUpdatedDate = DateTime.Now;
                equipmentReturnNote.CompanyId = CurrentUser.CompanyId.Value;
                _Util.Facade.PurchaseOrderFacade.InsertEquipmentReturnNote(equipmentReturnNote);

                // EquipmentReturnVendor
                EquipmentReturnVendor equipmentReturnVendor = new EquipmentReturnVendor();
                equipmentReturnVendor.ReturnId = equipmentReturn.ReturnId;
                equipmentReturnVendor.Description = equipmentReturn.Description;
                equipmentReturnVendor.Status = equipmentReturn.Status;
                equipmentReturnVendor.LastUpdatedBy = CurrentUser.UserId;
                equipmentReturnVendor.LastUpdatedDate = DateTime.Now;
                equipmentReturnVendor.CompanyId = CurrentUser.CompanyId.Value;
                _Util.Facade.PurchaseOrderFacade.InsertEquipmentReturnVendor(equipmentReturnVendor);

                message = "Saved Successfully";
                return Json(new { result = result, message = message });
            }
            message = "Something Wrong";
            return Json(new { result = result, message = message });
        }

        [Authorize]
        [HttpPost]
        public JsonResult DeleteEquipmentReturn(int? id)
        {
            var result = false;
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (CurrentLoggedInUser == null)
            {
                return Json(result);
            }
            if (id.HasValue && id.Value > 0)
            {
                result = _Util.Facade.PurchaseOrderFacade.DeleteEquipmentReturn(id.Value);
            }
            return Json(result);
        }

        [Authorize]
        [HttpPost]
        public JsonResult ReceiveEquipmentReturn(int? id)
        {
            var result = false;
            var CurrentLoggedInUser = ((HS.Web.UI.Helper.CustomPrincipal)(User));
            if (CurrentLoggedInUser == null)
            {
                return Json(result);
            }
            if (id.HasValue && id.Value > 0)
            {
                var EquipmentReturnDetails = _Util.Facade.PurchaseOrderFacade.GetEquipmentReturnById(id.Value);
                if (EquipmentReturnDetails != null)
                {
                    EquipmentReturnDetails.Status = LabelHelper.BadInventoryStatus.Received;
                    result = _Util.Facade.PurchaseOrderFacade.UpdateEquipmentReturn(EquipmentReturnDetails);
                }
            }
            return Json(result);
        }
        #endregion
        private byte[] CreatePurchasOrderPdf(CreatePurchaseOrder Model, string purchaseid)
        {
            var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            if (Model.Company == null)
            {
                Model.Company = _Util.Facade.CompanyFacade.GetCompanyByComapnyId(CurrentUser.CompanyId.Value);
            }
            if (string.IsNullOrWhiteSpace(Model.Company.CompanyLogo))
            {
                Model.Company.CompanyLogo = _Util.Facade.CompanyBranchFacade.GetCompanyLogoForPDFByCompanyId(CurrentUser.CompanyId.Value);
            }
            if (string.IsNullOrWhiteSpace(Model.CompanyAddressFormat))
            {
                Model.CompanyAddressFormat = _Util.Facade.GlobalSettingsFacade.GetCompanyAddressFormat(CurrentUser.CompanyId.Value);
            }
            if (!string.IsNullOrWhiteSpace(purchaseid))
            {
                Model.PurchaseOrderWarehouse = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderWarehouseById(Convert.ToInt32(purchaseid));
            }
            if (Model.Supplier == null)
            {
                Model.Supplier = _Util.Facade.SupplierFacade.GetSupplierBySupplierId(Model.PurchaseOrderWarehouse.SuplierId);
            }
            Model.PurchaseOrderDetail = _Util.Facade.PurchaseOrderFacade.GetPurchaseOrderDetailListByPurchaseOrderId(Model.PurchaseOrderWarehouse.PurchaseOrderId);
            Hashtable datatemplate = new Hashtable();
            #region CompanyAddress Info
            datatemplate.Add("ComapnyName", Model.Company.CompanyName);
            datatemplate.Add("Address", Model.Company.Address);
            datatemplate.Add("Street", Model.Company.Street);
            datatemplate.Add("City", Model.Company.City);
            datatemplate.Add("State", Model.Company.State);
            datatemplate.Add("Zip", Model.Company.ZipCode);
            datatemplate.Add("CompanyPhone", Model.Company.Phone);
            datatemplate.Add("EmailAddress", Model.Company.EmailAdress);
            datatemplate.Add("WebAddress", Model.Company.Website);
            Model.CompanyAddressFormat = HS.Web.UI.Helper.LabelHelper.ParserHelper(Model.CompanyAddressFormat, datatemplate);
            #endregion
            ViewAsPdf actionPDF = new Rotativa.ViewAsPdf("PurchaseOrderPdf", Model)
            {
                PageSize = Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                PageMargins = { Left = 1, Right = 1 },
            };
            byte[] applicationPDFData = actionPDF.BuildPdf(ControllerContext);
            return applicationPDFData;
            //return File(applicationPDFData, System.Net.Mime.MediaTypeNames.Application.Octet, "InvoiceList.pdf");

        }

        //public ActionResult RequisitionListPdf()
        //{
        //    return new ViewAsPdf()
        //    {
        //        PageSize = Size.A4,
        //        PageOrientation = Rotativa.Options.Orientation.Portrait,
        //        PageMargins = { Left = 1, Right = 1 },
        //    };
        //}
    }
}