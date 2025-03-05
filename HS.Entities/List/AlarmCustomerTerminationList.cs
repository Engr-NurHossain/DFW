using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "AlarmCustomerTerminationList", Namespace = "http://www.piistech.com//list")]	
	public class AlarmCustomerTerminationList : BaseCollection<AlarmCustomerTermination>
	{
		#region Constructors
	    public AlarmCustomerTerminationList() : base() { }
        public AlarmCustomerTerminationList(AlarmCustomerTermination[] list) : base(list) { }
        public AlarmCustomerTerminationList(List<AlarmCustomerTermination> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

