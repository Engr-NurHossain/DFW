using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "EmergencyContactList", Namespace = "http://www.piistech.com//list")]	
	public class EmergencyContactList : BaseCollection<EmergencyContact>
	{
		#region Constructors
	    public EmergencyContactList() : base() { }
        public EmergencyContactList(EmergencyContact[] list) : base(list) { }
        public EmergencyContactList(List<EmergencyContact> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

