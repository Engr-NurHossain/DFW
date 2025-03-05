using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "MenuItemCategoryMapList", Namespace = "http://www.hims-tech.com//list")]	
	public class MenuItemCategoryMapList : BaseCollection<MenuItemCategoryMap>
	{
		#region Constructors
	    public MenuItemCategoryMapList() : base() { }
        public MenuItemCategoryMapList(MenuItemCategoryMap[] list) : base(list) { }
        public MenuItemCategoryMapList(List<MenuItemCategoryMap> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
