using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerApiSettingList", Namespace = "http://www.piistech.com//list")]	
	public class CustomerApiSettingList : BaseCollection<CustomerApiSetting>
	{
		#region Constructors
	    public CustomerApiSettingList() : base() { }
        public CustomerApiSettingList(CustomerApiSetting[] list) : base(list) { }
        public CustomerApiSettingList(List<CustomerApiSetting> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

