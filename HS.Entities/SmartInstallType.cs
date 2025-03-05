using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class SmartInstallType 
	{
		public int SystemId { get; set; }

    }
    public partial class ManufacturerInstallType
    {
        public List<SmartInstallType> SmartInstallTypeList { get; set; }
        public List<Manufacturer> ManufacturerList { get; set; }
    }
}
