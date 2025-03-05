using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerAppointmentList", Namespace = "http://www.piistech.com//list")]	
	public class CustomerAppointmentList : BaseCollection<CustomerAppointment>
	{
		#region Constructors
	    public CustomerAppointmentList() : base() { }
        public CustomerAppointmentList(CustomerAppointment[] list) : base(list) { }
        public CustomerAppointmentList(List<CustomerAppointment> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

