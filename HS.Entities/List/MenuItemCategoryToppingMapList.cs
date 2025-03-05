using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "MenuItemCategoryToppingMapList", Namespace = "http://www.hims-tech.com//list")]	
	public class MenuItemCategoryToppingMapList : BaseCollection<MenuItemCategoryToppingMap>
	{
		#region Constructors
	    public MenuItemCategoryToppingMapList() : base() { }
        public MenuItemCategoryToppingMapList(MenuItemCategoryToppingMap[] list) : base(list) { }
        public MenuItemCategoryToppingMapList(List<MenuItemCategoryToppingMap> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
