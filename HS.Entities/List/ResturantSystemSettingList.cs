using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "ResturantSystemSettingList", Namespace = "http://www.hims-tech.com//list")]	
	public class ResturantSystemSettingList : BaseCollection<ResturantSystemSetting>
	{
		#region Constructors
	    public ResturantSystemSettingList() : base() { }
        public ResturantSystemSettingList(ResturantSystemSetting[] list) : base(list) { }
        public ResturantSystemSettingList(List<ResturantSystemSetting> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
