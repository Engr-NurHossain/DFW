using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "HrDocList", Namespace = "http://www.piistech.com//list")]	
	public class HrDocList : BaseCollection<HrDoc>
	{
		#region Constructors
	    public HrDocList() : base() { }
        public HrDocList(HrDoc[] list) : base(list) { }
        public HrDocList(List<HrDoc> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

