using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class SmartPackageEquipmentService 
	{
		public string EquipmentName { get; set; }
        public string PackageName { get; set; }
        public int EquipmentMaxLimit { get; set; }
        public double Retail { get; set; }
        public int NumofOptional { get; set; }
        public int IsSelected { get; set; }
        public string LastUpdatedName { get; set; }
        public string ManufacturerName { get; set; }
        public string SKU { get; set; }
        public List<SmartPackageEquipmentServiceEquipment> ServiceEquipments { set; get; }
    }
}
