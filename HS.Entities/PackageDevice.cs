using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
    public partial class PackageDevice
    {
        public string IncludedEquipmentName { get; set; }
        public string IncludedPackageName { get; set; }
    }
}
