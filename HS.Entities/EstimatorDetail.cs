using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class EstimatorDetail 
	{
        public EquipmentFile EquipmentFile { get; set; }

        public string CategoryVal { get; set; }
        public string SKU { get; set; }

        public string SupplierVal { get; set; }

        public string Manufacturer { get; set; }

        public List<EquipmentManufacturer> EquipmentManufacturers { set; get; }

        //Will be used for Manufacturer lists for selected equipment
        public List<Manufacturer> Manufacturers { set; get; }
        public string EquipmentDescription { get; set; }
        public double UnitPrice { get; set; }
        public double Qunatity2 { get; set; }

    }
}
