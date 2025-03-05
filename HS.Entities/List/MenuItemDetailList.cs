using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "MenuItemDetailList", Namespace = "http://www.piistech.com//list")]	
	public class MenuItemDetailList : BaseCollection<MenuItemDetail>
	{
		#region Constructors
	    public MenuItemDetailList() : base() { }
        public MenuItemDetailList(MenuItemDetail[] list) : base(list) { }
        public MenuItemDetailList(List<MenuItemDetail> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

