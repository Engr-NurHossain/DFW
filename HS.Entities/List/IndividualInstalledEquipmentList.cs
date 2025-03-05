using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "IndividualInstalledEquipmentList", Namespace = "http://www.hims-tech.com//list")]	
	public class IndividualInstalledEquipmentList : BaseCollection<IndividualInstalledEquipment>
	{
		#region Constructors
	    public IndividualInstalledEquipmentList() : base() { }
        public IndividualInstalledEquipmentList(IndividualInstalledEquipment[] list) : base(list) { }
        public IndividualInstalledEquipmentList(List<IndividualInstalledEquipment> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
