using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "MenuCategoryMapList", Namespace = "http://www.hims-tech.com//list")]	
	public class MenuCategoryMapList : BaseCollection<MenuCategoryMap>
	{
		#region Constructors
	    public MenuCategoryMapList() : base() { }
        public MenuCategoryMapList(MenuCategoryMap[] list) : base(list) { }
        public MenuCategoryMapList(List<MenuCategoryMap> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
