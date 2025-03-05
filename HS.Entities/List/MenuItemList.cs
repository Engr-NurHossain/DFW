using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "MenuItemList", Namespace = "http://www.piistech.com//list")]	
	public class MenuItemList : BaseCollection<MenuItem>
	{
		#region Constructors
	    public MenuItemList() : base() { }
        public MenuItemList(MenuItem[] list) : base(list) { }
        public MenuItemList(List<MenuItem> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

