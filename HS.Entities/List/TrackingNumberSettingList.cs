using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "TrackingNumberSettingList", Namespace = "http://www.hims-tech.com//list")]	
	public class TrackingNumberSettingList : BaseCollection<TrackingNumberSetting>
	{
		#region Constructors
	    public TrackingNumberSettingList() : base() { }
        public TrackingNumberSettingList(TrackingNumberSetting[] list) : base(list) { }
        public TrackingNumberSettingList(List<TrackingNumberSetting> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
