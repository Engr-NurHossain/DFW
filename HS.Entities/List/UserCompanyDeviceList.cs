using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "UserCompanyDeviceList", Namespace = "http://www.hims-tech.com//list")]	
	public class UserCompanyDeviceList : BaseCollection<UserCompanyDevice>
	{
		#region Constructors
	    public UserCompanyDeviceList() : base() { }
        public UserCompanyDeviceList(UserCompanyDevice[] list) : base(list) { }
        public UserCompanyDeviceList(List<UserCompanyDevice> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
