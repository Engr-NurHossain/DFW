using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "MenuList", Namespace = "http://www.piistech.com//list")]	
	public class MenuList : BaseCollection<Menu>
	{
		#region Constructors
	    public MenuList() : base() { }
        public MenuList(Menu[] list) : base(list) { }
        public MenuList(List<Menu> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

