using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class EmployeeTimeClock 
	{
		public string Type { get; set; }
        public string Note { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Intime { get; set; }
        public string Outtime { get; set; }
        public string LastUpdatedName { get; set; }
    }
    public partial class EmployeeClockIO
    {
        public bool IsClockedIn { get; set; }
        public Nullable<DateTime> ClockInOutTime { get; set; }
    }
}
