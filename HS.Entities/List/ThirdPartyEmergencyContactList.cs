using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "ThirdPartyEmergencyContactList", Namespace = "http://www.hims-tech.com//list")]	
	public class ThirdPartyEmergencyContactList : BaseCollection<ThirdPartyEmergencyContact>
	{
		#region Constructors
	    public ThirdPartyEmergencyContactList() : base() { }
        public ThirdPartyEmergencyContactList(ThirdPartyEmergencyContact[] list) : base(list) { }
        public ThirdPartyEmergencyContactList(List<ThirdPartyEmergencyContact> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
