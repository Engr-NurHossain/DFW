using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerAppointmentEquipmentList", Namespace = "http://www.piistech.com//list")]	
	public class CustomerAppointmentEquipmentList : BaseCollection<CustomerAppointmentEquipment>
	{
		#region Constructors
	    public CustomerAppointmentEquipmentList() : base() { }
        public CustomerAppointmentEquipmentList(CustomerAppointmentEquipment[] list) : base(list) { }
        public CustomerAppointmentEquipmentList(List<CustomerAppointmentEquipment> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

