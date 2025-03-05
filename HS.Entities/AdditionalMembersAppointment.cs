using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class AdditionalMembersAppointment 
	{
        public string EmpName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
