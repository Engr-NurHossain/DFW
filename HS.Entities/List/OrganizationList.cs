using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "OrganizationList", Namespace = "http://www.piistech.com//list")]	
	public class OrganizationList : BaseCollection<Organization>
	{
		#region Constructors
	    public OrganizationList() : base() { }
        public OrganizationList(Organization[] list) : base(list) { }
        public OrganizationList(List<Organization> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

