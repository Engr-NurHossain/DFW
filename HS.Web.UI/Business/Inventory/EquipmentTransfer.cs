using System;
using HS.Entities;
using HS.Web.UI.App_Start;
using HS.Framework;
using HS.Web.UI.Helper;
using HS.Entities.Result;

namespace HS.Web.UI.Business.Inventory
{
    public class EquipmentTransfer
    {
        private WebUtil __Util;
        protected WebUtil _Util
        {
            get
            {
                if (null == __Util)
                    __Util = new WebUtil();
                return __Util;
            }
        }

        public TransferResult TransferTechToTech(Guid FromTechnician, Guid EquipmentId, Guid CompanyId, Guid UserId, Guid ToTechnician, string Module = "[Inv]", int Quantity = 0)
        {
            bool Released = false;
            bool Added = false;
            string FromDesc = string.Empty;
            string ToDesc = string.Empty;

            switch (Module)
            {
                case "[TI-AIT]":
                    FromDesc = "Release from technician to technician ";
                    ToDesc = "Add from technician to technician ";
                    break;
                case "[Inv-Tech-Trf]":
                    FromDesc = "Send from technician to technician ";
                    ToDesc = "Receive from technician to technician ";
                    break;
                case "[TT-Approve]":
                    FromDesc = "Transfer Release from technician to technician ";
                    ToDesc = "Transfer Add from technician to technician ";
                    break;
                case "[PURORD-WHTT]":
                    FromDesc = $"Transfer Release from purchase order to technician";
                    ToDesc = "Transfer Add from purchase order to technician";
                    break;
               
                default:
                    FromDesc = "Tech to Tech ";
                    ToDesc = "Tech to Tech ";
                    break;
            }

            if (Quantity != 0)
            {
                if (FromTechnician != new Guid("22222222-2222-2222-2222-222222222221"))
                {
                    InventoryTech ReleaseInventoryTech = new InventoryTech()
                    {
                        CompanyId = CompanyId,
                        TechnicianId = FromTechnician,
                        EquipmentId = EquipmentId,
                        Type = "Release",
                        Quantity = Quantity,
                        LastUpdatedBy = UserId,
                        LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                        Description = FromDesc + Module
                    };
                    Released = _Util.Facade.InventoryFacade.InsertInventoryTech(ReleaseInventoryTech) > 0;
                }
                else
                {
                    Released = true;
                }
                InventoryTech AddInventoryTech = new InventoryTech()
                {
                    CompanyId = CompanyId,
                    TechnicianId = ToTechnician,
                    EquipmentId = EquipmentId,
                    Type = "Add",
                    Quantity = Quantity,
                    LastUpdatedBy = UserId,
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                    Description = ToDesc + Module
                };
                Added = _Util.Facade.InventoryFacade.InsertInventoryTech(AddInventoryTech) > 0;
            }
            return new TransferResult(Released, Added);
        }

        public TransferResult TransferWHToTech(Guid ToTechnician, Guid EquipmentId, Guid CompanyId, Guid UserId, Guid FromLocation, string Module = "[Inv]", int Quantity = 0, string PurchaseOrderId = null)
        {
            bool Released = false;
            bool Added = false;
            string FromDesc = string.Empty;
            string ToDesc = string.Empty;

            switch (Module)
            {
               
              
                case "[MSRSTK-WHTT]":
                    FromDesc = $"Send to technician from warehouse {FromLocation} by massrestock ";
                    ToDesc = $"Receive by technician from warehouse {FromLocation} by massrestock ";
                    break;
                case "[MSRSTK-WHTTC]":
                    FromDesc = $"Send to technician from warehouse {FromLocation} by massrestock ";
                    ToDesc = $"Receive by technician from warehouse {FromLocation} by massrestock ";
                    break;
                case "[SLTT-Approve]":
                    FromDesc = $"Transfer Release from warehouse {FromLocation} to technician ";
                    ToDesc = $"Transfer Add to technician from warehouse {FromLocation} ";
                    break;
                case "[WHTT-Approve]":
                    FromDesc = $"Transfer Release from warehouse {FromLocation} to technician ";
                    ToDesc = $"Transfer Add to technician from warehouse {FromLocation} ";
                    break;
                default:
                    FromDesc = $"warehouse {FromLocation} to technician ";
                    ToDesc = $"technician from warehouse {FromLocation} ";
                    break;
            }
            InventoryWarehouse invWare = new InventoryWarehouse()
            {
                CompanyId = CompanyId,
                EquipmentId = EquipmentId,
                Type = LabelHelper.InventoryType.Release,
                Quantity = Quantity,
                PurchaseOrderId = PurchaseOrderId,
                LastUpdatedBy = UserId,
                LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                Description = FromDesc + Module,
                LocationId = FromLocation
            };
            Released = _Util.Facade.InventoryFacade.InsertInventoryWareHouse(invWare) > 0;

            InventoryTech invTech = new InventoryTech()
            {
                CompanyId = CompanyId,
                EquipmentId = EquipmentId,
                Type = LabelHelper.InventoryType.Add,
                Quantity = Quantity,
                TechnicianId = ToTechnician,
                PurchaseOrderId = PurchaseOrderId,
                LastUpdatedBy = UserId,
                LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                Description = ToDesc + Module
            };
            Added = _Util.Facade.InventoryFacade.InsertInventoryTech(invTech) > 0;

            return new TransferResult(Released, Added);
        }

        public TransferResult TransferTechToWH(Guid FromTechnician, Guid EquipmentId, Guid CompanyId, Guid UserId, Guid ToLocation, string Module = "[Inv]", int Quantity = 0)
        {
            bool Released = false;
            bool Added = false;
            string FromDesc = string.Empty;
            string ToDesc = string.Empty;

            switch (Module)
            {
                case "[TI-AIT]":
                    FromDesc = $"Release from technician to warehouse {ToLocation} ";
                    ToDesc = $"Receive by warehouse {ToLocation} from technician ";
                    break;
                
                case "[TTSL-Approve]":
                    FromDesc = $"Transfer Release from technician to warehouse {ToLocation} ";
                    ToDesc = $"Transfer Add to warehouse {ToLocation} from technician ";
                    break;
                case "[TTWH-Approve]":
                    FromDesc = $"Transfer Release from technician to warehouse {ToLocation} ";
                    ToDesc = $"Transfer Add to warehouse {ToLocation} from technician ";
                    break;
                default:
                    FromDesc = $"technician to warehouse {ToLocation} ";
                    ToDesc = $"warehouse {ToLocation} from technician ";
                    break;
            }
            if (Quantity != 0)
            {
                InventoryTech ReleaseInventoryTech = new InventoryTech()
                {
                    CompanyId = CompanyId,
                    TechnicianId = FromTechnician,
                    EquipmentId = EquipmentId,
                    Type = "Release",
                    Quantity = Quantity,
                    LastUpdatedBy = UserId,
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                    Description = FromDesc + Module
                };
                Released = _Util.Facade.InventoryFacade.InsertInventoryTech(ReleaseInventoryTech) > 0;
                InventoryWarehouse AddInventoryWarehouse = new InventoryWarehouse()
                {
                    CompanyId = CompanyId,
                    EquipmentId = EquipmentId,
                    Type = "Add",
                    Quantity = Quantity,
                    LastUpdatedBy = UserId,
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                    Description = ToDesc + Module,
                    LocationId = ToLocation
                };
                Added = _Util.Facade.InventoryFacade.InsertInventoryWareHouse(AddInventoryWarehouse) > 0;
            }
            return new TransferResult(Released, Added);
        }

        public TransferResult TransferWHToWH(Guid FromLocation, Guid EquipmentId, Guid CompanyId, Guid UserId, Guid ToLocation, string Module = "[Inv]", int Quantity = 0)
        {
            bool Released = false;
            bool Added = false;
            string FromDesc = string.Empty;
            string ToDesc = string.Empty;

            switch (Module)
            {
                case "[WHWH-Approve]":
                    FromDesc = $"Transfer Release from warehouse {FromLocation} to warehouse {ToLocation} ";
                    ToDesc = $"Transfer Add to warehouse {ToLocation} from warehouse {FromLocation} ";
                    break;
                default:
                    FromDesc = $"warehouse to warehouse {ToLocation} ";
                    ToDesc = $"warehouse {ToLocation} from warehouse ";
                    break;
            }
            if (Quantity != 0)
            {
                InventoryWarehouse ReleaseInventoryWarehouse = new InventoryWarehouse()
                {
                    CompanyId = CompanyId,
                    EquipmentId = EquipmentId,
                    Type = "Release",
                    Quantity = Quantity,
                    LastUpdatedBy = UserId,
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                    Description = FromDesc + Module,
                    LocationId = FromLocation
                };
                Released = _Util.Facade.InventoryFacade.InsertInventoryWareHouse(ReleaseInventoryWarehouse) > 0;

                InventoryWarehouse AddInventoryWarehouse = new InventoryWarehouse()
                {
                    CompanyId = CompanyId,
                    EquipmentId = EquipmentId,
                    Type = "Add",
                    Quantity = Quantity,
                    LastUpdatedBy = UserId,
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                    Description = ToDesc + Module,
                    LocationId = ToLocation
                };
                Added = _Util.Facade.InventoryFacade.InsertInventoryWareHouse(AddInventoryWarehouse) > 0;
            }
            return new TransferResult(Released, Added);
        }

        //public TransferResult TransferPOToWH(Guid FromTechnician, Guid EquipmentId, Guid CompanyId, Guid UserId, string Module = "[Inv]", int Quantity = 0)
        //{
        //    bool Released = false;
        //    bool Added = false;
        //    string FromDesc = string.Empty;
        //    string ToDesc = string.Empty;

        //    switch (Module)
        //    {
        //        case "[TI-AIT]":
        //            FromDesc = "Release from technician to warehouse ";
        //            ToDesc = "Receive by warehouse from technician ";
        //            break;
        //        case "[TT-Approve]":
        //            FromDesc = "Transfer Release from technician to warehouse ";
        //            ToDesc = "Transfer Add to warehouse from technician ";
        //            break;
        //        default:
        //            FromDesc = "Tech to WH ";
        //            ToDesc = "WH from Tech ";
        //            break;
        //    }
        //    if (Quantity != 0)
        //    {
        //        InventoryTech ReleaseInventoryTech = new InventoryTech()
        //        {
        //            CompanyId = CompanyId,
        //            TechnicianId = FromTechnician,
        //            EquipmentId = EquipmentId,
        //            Type = "Release",
        //            Quantity = Quantity,
        //            LastUpdatedBy = UserId,
        //            LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
        //            Description = FromDesc + Module
        //        };
        //        Released = _Util.Facade.InventoryFacade.InsertInventoryTech(ReleaseInventoryTech) > 0;
        //        InventoryWarehouse AddInventoryWarehouse = new InventoryWarehouse()
        //        {
        //            CompanyId = CompanyId,
        //            EquipmentId = EquipmentId,
        //            Type = "Add",
        //            Quantity = Quantity,
        //            LastUpdatedBy = UserId,
        //            LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
        //            Description = ToDesc + Module
        //        };
        //        Added = _Util.Facade.InventoryFacade.InsertInventoryWareHouse(AddInventoryWarehouse) > 0;
        //    }
        //    return new TransferResult(Released, Added);
        //}

        //public TransferResult TransferWHToWH(Guid FromLocation, Guid EquipmentId, Guid CompanyId, Guid UserId, Guid ToLocation, string Module = "[Inv]", int Quantity = 0)
        //{
        //    bool Released = false;
        //    bool Added = false;
        //    string FromDesc = string.Empty;
        //    string ToDesc = string.Empty;
        //    switch (Module)
        //    {
        //        case "[WHTT-Approve]":
        //            FromDesc = $"Transfer Release from warehouse {FromLocation} to warehouse {ToLocation} ";
        //            ToDesc = $"Transfer Add to warehouse {ToLocation} from warehouse {FromLocation} ";
        //            break;
        //        default:
        //            FromDesc = $"warehouse to warehouse {ToLocation} ";
        //            ToDesc = $"warehouse {ToLocation} from warehouse ";
        //            break;
        //    }
        //    if (Quantity != 0)
        //    {
        //        InventoryWarehouse ReleaseInventoryWarehouse = new InventoryWarehouse()
        //        {
        //            CompanyId = CompanyId,
        //            EquipmentId = EquipmentId,
        //            Type = "Release",
        //            Quantity = Quantity,
        //            LastUpdatedBy = UserId,
        //            LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
        //            Description = FromDesc + Module,
        //            LocationId = FromLocation
        //        };
        //        Released = _Util.Facade.InventoryFacade.InsertInventoryWareHouse(ReleaseInventoryWarehouse) > 0;
        //        InventoryWarehouse AddInventoryWarehouse = new InventoryWarehouse()
        //        {
        //            CompanyId = CompanyId,
        //            EquipmentId = EquipmentId,
        //            Type = "Add",
        //            Quantity = Quantity,
        //            LastUpdatedBy = UserId,
        //            LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
        //            Description = ToDesc + Module,
        //            LocationId = ToLocation
        //        };
        //        Added = _Util.Facade.InventoryFacade.InsertInventoryWareHouse(AddInventoryWarehouse) > 0;
        //    }
        //    return new TransferResult(Released, Added);
        //}


    }

    public static class VirtualLocations
    {

        public static bool IsVirtualLocation(this Guid LocationId)
        {
            bool IsLocation = false;
            if (LocationId.ToString().IsVirtualLocation())
            {
                IsLocation = true;
            }
            return IsLocation;
        }

        public static bool IsVirtualLocation(this string LocationId)
        {
            bool IsLocation = false;
            if (LocationId.Equals("22222222-2222-2222-2222-222222222221") ||
                LocationId.Equals("22222222-2222-2222-2222-222222222222") ||
                LocationId.Equals("22222222-2222-2222-2222-222222222223") ||
                LocationId.Equals("22222222-2222-2222-2222-222222222224") ||
                LocationId.Equals("22222222-2222-2222-2222-222222222225") ||
                LocationId.Equals("22222222-2222-2222-2222-222222222226") ||
                LocationId.Equals("22222222-2222-2222-2222-222222222231") ||
                LocationId.Equals("22222222-2222-2222-2222-222222222232")
                )
            {
                IsLocation = true;
            }
            return IsLocation;
        }
    }
}