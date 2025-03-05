using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class PackageCommission 
	{
        public string PackageTypeVal { get; set; }
        public string TypeVal { get; set; }
        public string CommissionTypeVal { get; set; }
        public string LeadTypeVal { get; set; }
    }
}
