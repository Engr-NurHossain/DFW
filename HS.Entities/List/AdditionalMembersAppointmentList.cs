using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "AdditionalMembersAppointmentList", Namespace = "http://www.piistech.com//list")]	
	public class AdditionalMembersAppointmentList : BaseCollection<AdditionalMembersAppointment>
	{
		#region Constructors
	    public AdditionalMembersAppointmentList() : base() { }
        public AdditionalMembersAppointmentList(AdditionalMembersAppointment[] list) : base(list) { }
        public AdditionalMembersAppointmentList(List<AdditionalMembersAppointment> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

