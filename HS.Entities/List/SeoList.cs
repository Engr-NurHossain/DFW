using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "SeoList", Namespace = "http://www.piistech.com//list")]	
	public class SeoList : BaseCollection<Seo>
	{
		#region Constructors
	    public SeoList() : base() { }
        public SeoList(Seo[] list) : base(list) { }
        public SeoList(List<Seo> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

