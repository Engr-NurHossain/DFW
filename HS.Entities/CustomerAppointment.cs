using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class CustomerAppointment 
	{
        public string ServiceArea { get; set; }

        public string SalesPerson { get; set; }
        public string ServicePerson { get; set; }
        public int CustomCustomerId { get; set; }
        public string Idstring { get; set; }
        public string WorkPerson { get; set; }
        
        //custom properties for service order module
        public string ServiceOrderCustomerName { get; set; }
        public string ServiceOrderCustomerEmail { get; set; }
        public string ServiceOrderCustomerPhone { get; set; }
        public string ServiceOrderEmployeeName { get; set; }
        //custom properties for Work order module
        public string WorkOrderCustomerName { get; set; }
        public string WorkOrderCustomerEmail { get; set; }
        public string WorkOrderCustomerPhone { get; set; }
        public string WorkOrderEmployeeName { get; set; }
        //
        public string AppointmentStartTimeVal { set; get; }
        public string AppointmentEndTimeVal { set; get; }

        public List<CustomerAppointmentEquipment> CustomerAppointmentEquipmentList { set; get; }
        public string CustomerName { get; set; }
    }

    public partial class ServiceOrderMaildetails
    {
        public string EmployeeEmail { get; set; }
        public string EmployeeName { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerMiddleName { get; set; }
        public string CustomerLastName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string AppointmentStartTime { get; set; }
        public string AppointmentEndTime { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }
    
}
