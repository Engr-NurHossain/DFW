using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CityZipCodeSearchLogList", Namespace = "http://www.piistech.com//list")]	
	public class CityZipCodeSearchLogList : BaseCollection<CityZipCodeSearchLog>
	{
		#region Constructors
	    public CityZipCodeSearchLogList() : base() { }
        public CityZipCodeSearchLogList(CityZipCodeSearchLog[] list) : base(list) { }
        public CityZipCodeSearchLogList(List<CityZipCodeSearchLog> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

