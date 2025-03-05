using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerAppointmentTechnicianList", Namespace = "http://www.piistech.com//list")]	
	public class CustomerAppointmentTechnicianList : BaseCollection<CustomerAppointmentTechnician>
	{
		#region Constructors
	    public CustomerAppointmentTechnicianList() : base() { }
        public CustomerAppointmentTechnicianList(CustomerAppointmentTechnician[] list) : base(list) { }
        public CustomerAppointmentTechnicianList(List<CustomerAppointmentTechnician> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

