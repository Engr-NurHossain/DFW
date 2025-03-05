using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "UserOrganizationList", Namespace = "http://www.piistech.com//list")]	
	public class UserOrganizationList : BaseCollection<UserOrganization>
	{
		#region Constructors
	    public UserOrganizationList() : base() { }
        public UserOrganizationList(UserOrganization[] list) : base(list) { }
        public UserOrganizationList(List<UserOrganization> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

