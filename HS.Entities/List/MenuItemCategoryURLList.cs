using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "MenuItemCategoryURLList", Namespace = "http://www.hims-tech.com//list")]	
	public class MenuItemCategoryURLList : BaseCollection<MenuItemCategoryURL>
	{
		#region Constructors
	    public MenuItemCategoryURLList() : base() { }
        public MenuItemCategoryURLList(MenuItemCategoryURL[] list) : base(list) { }
        public MenuItemCategoryURLList(List<MenuItemCategoryURL> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
