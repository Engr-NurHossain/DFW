using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "AAEmployeeDumpList", Namespace = "http://www.hims-tech.com//list")]	
	public class AAEmployeeDumpList : BaseCollection<AAEmployeeDump>
	{
		#region Constructors
	    public AAEmployeeDumpList() : base() { }
        public AAEmployeeDumpList(AAEmployeeDump[] list) : base(list) { }
        public AAEmployeeDumpList(List<AAEmployeeDump> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
