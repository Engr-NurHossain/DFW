using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "ThirdPartySettingList", Namespace = "http://www.piistech.com//list")]	
	public class ThirdPartySettingList : BaseCollection<ThirdPartySetting>
	{
		#region Constructors
	    public ThirdPartySettingList() : base() { }
        public ThirdPartySettingList(ThirdPartySetting[] list) : base(list) { }
        public ThirdPartySettingList(List<ThirdPartySetting> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

