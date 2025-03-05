using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "BundleList", Namespace = "http://www.piistech.com//list")]	
	public class BundleList : BaseCollection<Bundle>
	{
		#region Constructors
	    public BundleList() : base() { }
        public BundleList(Bundle[] list) : base(list) { }
        public BundleList(List<Bundle> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

