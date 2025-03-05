using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "RestrictedZipCodeList", Namespace = "http://www.hims-tech.com//list")]	
	public class RestrictedZipCodeList : BaseCollection<RestrictedZipCode>
	{
		#region Constructors
	    public RestrictedZipCodeList() : base() { }
        public RestrictedZipCodeList(RestrictedZipCode[] list) : base(list) { }
        public RestrictedZipCodeList(List<RestrictedZipCode> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
