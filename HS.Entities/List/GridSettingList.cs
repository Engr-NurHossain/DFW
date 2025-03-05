using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "GridSettingList", Namespace = "http://www.piistech.com//list")]	
	public class GridSettingList : BaseCollection<GridSetting>
	{
		#region Constructors
	    public GridSettingList() : base() { }
        public GridSettingList(GridSetting[] list) : base(list) { }
        public GridSettingList(List<GridSetting> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

