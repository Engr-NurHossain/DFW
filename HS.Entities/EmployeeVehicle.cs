using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class EmployeeVehicle 
	{
        public bool IsAssign { get; set; }
        public string EmployeeName { get; set; }
    }
}
