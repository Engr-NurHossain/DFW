using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "ToppingList", Namespace = "http://www.piistech.com//list")]	
	public class ToppingList : BaseCollection<Topping>
	{
		#region Constructors
	    public ToppingList() : base() { }
        public ToppingList(Topping[] list) : base(list) { }
        public ToppingList(List<Topping> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

