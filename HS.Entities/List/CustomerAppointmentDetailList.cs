using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerAppointmentDetailList", Namespace = "http://www.piistech.com//list")]	
	public class CustomerAppointmentDetailList : BaseCollection<CustomerAppointmentDetail>
	{
		#region Constructors
	    public CustomerAppointmentDetailList() : base() { }
        public CustomerAppointmentDetailList(CustomerAppointmentDetail[] list) : base(list) { }
        public CustomerAppointmentDetailList(List<CustomerAppointmentDetail> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

