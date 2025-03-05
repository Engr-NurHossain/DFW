using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class EquipmentManufacturer 
	{
        public string Name { get; set; }
        public string ManufacturerName { get; set; }
    }
}
