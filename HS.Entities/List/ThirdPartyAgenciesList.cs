using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "ThirdPartyAgenciesList", Namespace = "http://www.hims-tech.com//list")]	
	public class ThirdPartyAgenciesList : BaseCollection<ThirdPartyAgencies>
	{
		#region Constructors
	    public ThirdPartyAgenciesList() : base() { }
        public ThirdPartyAgenciesList(ThirdPartyAgencies[] list) : base(list) { }
        public ThirdPartyAgenciesList(List<ThirdPartyAgencies> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
