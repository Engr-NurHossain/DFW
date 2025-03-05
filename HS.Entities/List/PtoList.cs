using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PtoList", Namespace = "http://www.piistech.com//list")]	
	public class PtoList : BaseCollection<Pto>
	{
		#region Constructors
	    public PtoList() : base() { }
        public PtoList(Pto[] list) : base(list) { }
        public PtoList(List<Pto> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

