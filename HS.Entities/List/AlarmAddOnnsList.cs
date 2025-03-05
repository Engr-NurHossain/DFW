using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "AlarmAddOnnsList", Namespace = "http://www.hims-tech.com//list")]	
	public class AlarmAddOnnsList : BaseCollection<AlarmAddOnns>
	{
		#region Constructors
	    public AlarmAddOnnsList() : base() { }
        public AlarmAddOnnsList(AlarmAddOnns[] list) : base(list) { }
        public AlarmAddOnnsList(List<AlarmAddOnns> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
