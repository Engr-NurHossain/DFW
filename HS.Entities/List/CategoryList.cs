using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CategoryList", Namespace = "http://www.piistech.com//list")]	
	public class CategoryList : BaseCollection<Category>
	{
		#region Constructors
	    public CategoryList() : base() { }
        public CategoryList(Category[] list) : base(list) { }
        public CategoryList(List<Category> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

