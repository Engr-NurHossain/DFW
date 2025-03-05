using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "AssignedInventoryTechReceivedList", Namespace = "http://www.piistech.com//list")]	
	public class AssignedInventoryTechReceivedList : BaseCollection<AssignedInventoryTechReceived>
	{
		#region Constructors
	    public AssignedInventoryTechReceivedList() : base() { }
        public AssignedInventoryTechReceivedList(AssignedInventoryTechReceived[] list) : base(list) { }
        public AssignedInventoryTechReceivedList(List<AssignedInventoryTechReceived> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

