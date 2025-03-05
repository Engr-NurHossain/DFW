using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CityZipCodeList", Namespace = "http://www.piistech.com//list")]	
	public class CityZipCodeList : BaseCollection<CityZipCode>
	{
		#region Constructors
	    public CityZipCodeList() : base() { }
        public CityZipCodeList(CityZipCode[] list) : base(list) { }
        public CityZipCodeList(List<CityZipCode> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

