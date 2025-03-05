using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class VehicleRepairLog 
	{
        public string CreatedBy { set; get; }
        public string Vin { set; get; }
        public string DriverName { set; get; }

        private string _StrDate { set; get; }
        private string _StrRepairDate { set; get; }
        private string _StrTireRotation { set; get; }
        private string _StrOilChange { set; get; }

        public string StrDate {get{ return _StrDate;}set{this.RepairDate = value.ToDateTime();}}
        public string StrRepairDate { get { return _StrDate; } set { this.RepairDate = value.ToDateTime(); } }
        public string StrTireRotation { get { return _StrDate; } set { this.TireRotation = value.ToDateTime(); } }
        public string StrOilChange { get { return _StrDate; } set { this.OilChange = value.ToDateTime(); } }

    }
}
