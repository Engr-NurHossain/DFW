using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "MenuDetailList", Namespace = "http://www.piistech.com//list")]	
	public class MenuDetailList : BaseCollection<MenuDetail>
	{
		#region Constructors
	    public MenuDetailList() : base() { }
        public MenuDetailList(MenuDetail[] list) : base(list) { }
        public MenuDetailList(List<MenuDetail> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

