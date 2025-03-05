using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class ServiceMap 
	{
        public string Manufacturer { set; get; }
        public string Location { set; get; }
        public string Type { set; get; }
        public string Model { set; get; }
        public string Finish { set; get; }
        public string Capacity { set; get; }
        public string Service { set; get; }

        public string EquipmentName { set; get; }
        public string SelectedTypeName { set; get; }

	}
}
