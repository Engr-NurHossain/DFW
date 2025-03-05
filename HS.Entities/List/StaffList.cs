using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "StaffList", Namespace = "http://www.piistech.com//list")]	
	public class StaffList : BaseCollection<Staff>
	{
		#region Constructors
	    public StaffList() : base() { }
        public StaffList(Staff[] list) : base(list) { }
        public StaffList(List<Staff> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

