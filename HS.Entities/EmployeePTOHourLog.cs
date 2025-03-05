using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class EmployeePTOHourLog 
	{
		public double TotalPTOHour {  get; set; }
		public double TotalRequestedHour {  get; set; }
        public double TotalUsed { get; set; }
        public double TotalAvailable { get; set; }
        public double Balance { get; set; }
        public double Requested { get; set; }
        public double PTOAvailable { get; set; } 
        public double TotalPTOEarned {  get; set; }	
		public double TotalWorkingSeconds {  get; set; }	
		public string EmployeeName { get; set; }	
		public double Totalearned { get; set; }	
		public Int32 EmployeeId { get; set; }	
		public DateTime HireDate { get; set; }	
		public string PayType { get; set; }	
		public string Status { get; set; }	

    }
}