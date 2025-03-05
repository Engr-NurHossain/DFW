using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "LookupList", Namespace = "http://www.piistech.com//list")]	
	public class LookupList : BaseCollection<Lookup>
	{
		#region Constructors
	    public LookupList() : base() { }
        public LookupList(Lookup[] list) : base(list) { }
        public LookupList(List<Lookup> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

