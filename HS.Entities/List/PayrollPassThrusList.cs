using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PayrollPassThrusList", Namespace = "http://www.hims-tech.com//list")]	
	public class PayrollPassThrusList : BaseCollection<PayrollPassThrus>
	{
		#region Constructors
	    public PayrollPassThrusList() : base() { }
        public PayrollPassThrusList(PayrollPassThrus[] list) : base(list) { }
        public PayrollPassThrusList(List<PayrollPassThrus> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
