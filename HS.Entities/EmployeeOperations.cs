using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class EmployeeOperations 
	{
		
	}
	public class EmployeeWeeklyOperations
	{
		public string MonStart { get; set; }
		public string MonEnd { get; set; }
		public string TueStart { get; set; }
		public string TueEnd { get; set; }
		public string WedStart { get; set; }
		public string WedEnd { get; set; }
		public string ThuStart { get; set; }
		public string ThuEnd { get; set; }
		public string FriStart { get; set; }
		public string FriEnd { get; set; }
		public string SatStart { get; set; }
		public string SatEnd { get; set; }
		public string SunStart { get; set; }
		public string SunEnd { get; set; }
		public string UserId { get; set; }
		public string CurrentDate { get; set; }
	}
}