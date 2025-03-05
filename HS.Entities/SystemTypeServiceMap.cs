using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class SystemTypeServiceMap 
	{
        public string SystemType { get; set; }
        public string PackageName { get; set; }
        public List<Guid> PackageIdList { get; set; }
        public List<Guid> EquipmentIdList { get; set; }
        public string ServiceName { get; set; }
        public double Retail { get; set; }
    }
}
