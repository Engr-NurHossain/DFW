using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "AlarmCustomerSelectedAddonList", Namespace = "http://www.hims-tech.com//list")]	
	public class AlarmCustomerSelectedAddonList : BaseCollection<AlarmCustomerSelectedAddon>
	{
		#region Constructors
	    public AlarmCustomerSelectedAddonList() : base() { }
        public AlarmCustomerSelectedAddonList(AlarmCustomerSelectedAddon[] list) : base(list) { }
        public AlarmCustomerSelectedAddonList(List<AlarmCustomerSelectedAddon> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
