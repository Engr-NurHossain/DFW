using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "RestMenuItemCategoryToppingList", Namespace = "http://www.hims-tech.com//list")]	
	public class RestMenuItemCategoryToppingList : BaseCollection<RestMenuItemCategoryTopping>
	{
		#region Constructors
	    public RestMenuItemCategoryToppingList() : base() { }
        public RestMenuItemCategoryToppingList(RestMenuItemCategoryTopping[] list) : base(list) { }
        public RestMenuItemCategoryToppingList(List<RestMenuItemCategoryTopping> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
