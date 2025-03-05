using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CityTaxList", Namespace = "http://www.piistech.com//list")]	
	public class CityTaxList : BaseCollection<CityTax>
	{
		#region Constructors
	    public CityTaxList() : base() { }
        public CityTaxList(CityTax[] list) : base(list) { }
        public CityTaxList(List<CityTax> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

