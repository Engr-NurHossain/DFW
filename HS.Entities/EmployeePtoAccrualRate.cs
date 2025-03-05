using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class EmployeePtoAccrualRate 
	{
		public string AccrualPayType { get; set; }
		public string MaximumFormattedDays { get; set; }
		public string MinimumFormattedDays { get; set; }
		
	}
}