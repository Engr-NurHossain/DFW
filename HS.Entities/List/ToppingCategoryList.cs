using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "ToppingCategoryList", Namespace = "http://www.piistech.com//list")]	
	public class ToppingCategoryList : BaseCollection<ToppingCategory>
	{
		#region Constructors
	    public ToppingCategoryList() : base() { }
        public ToppingCategoryList(ToppingCategory[] list) : base(list) { }
        public ToppingCategoryList(List<ToppingCategory> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

