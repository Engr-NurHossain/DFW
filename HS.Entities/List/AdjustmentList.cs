using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "AdjustmentList", Namespace = "http://www.piistech.com//list")]	
	public class AdjustmentList : BaseCollection<Adjustment>
	{
		#region Constructors
	    public AdjustmentList() : base() { }
        public AdjustmentList(Adjustment[] list) : base(list) { }
        public AdjustmentList(List<Adjustment> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

