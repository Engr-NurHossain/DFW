using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CategoryDetailList", Namespace = "http://www.piistech.com//list")]	
	public class CategoryDetailList : BaseCollection<CategoryDetail>
	{
		#region Constructors
	    public CategoryDetailList() : base() { }
        public CategoryDetailList(CategoryDetail[] list) : base(list) { }
        public CategoryDetailList(List<CategoryDetail> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

