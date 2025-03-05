using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "TeamSettingList", Namespace = "http://www.hims-tech.com//list")]	
	public class TeamSettingList : BaseCollection<TeamSetting>
	{
		#region Constructors
	    public TeamSettingList() : base() { }
        public TeamSettingList(TeamSetting[] list) : base(list) { }
        public TeamSettingList(List<TeamSetting> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
