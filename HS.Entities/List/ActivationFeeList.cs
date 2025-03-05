using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "ActivationFeeList", Namespace = "http://www.piistech.com//list")]	
	public class ActivationFeeList : BaseCollection<ActivationFee>
	{
		#region Constructors
	    public ActivationFeeList() : base() { }
        public ActivationFeeList(ActivationFee[] list) : base(list) { }
        public ActivationFeeList(List<ActivationFee> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

