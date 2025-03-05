using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class EmployeeInsurance 
	{
        public string SubTypeMedicle { get; set; }
        public string SubTypeDental { get; set; }
        public string SubTypeVision { get; set; }
    }
}
