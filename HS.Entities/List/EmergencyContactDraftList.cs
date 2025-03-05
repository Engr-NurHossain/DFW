using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "EmergencyContactDraftList", Namespace = "http://www.piistech.com//list")]	
	public class EmergencyContactDraftList : BaseCollection<EmergencyContactDraft>
	{
		#region Constructors
	    public EmergencyContactDraftList() : base() { }
        public EmergencyContactDraftList(EmergencyContactDraft[] list) : base(list) { }
        public EmergencyContactDraftList(List<EmergencyContactDraft> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

