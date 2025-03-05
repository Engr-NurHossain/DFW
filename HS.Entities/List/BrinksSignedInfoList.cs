using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "BrinksSignedInfoList", Namespace = "http://www.hims-tech.com//list")]	
	public class BrinksSignedInfoList : BaseCollection<BrinksSignedInfo>
	{
		#region Constructors
	    public BrinksSignedInfoList() : base() { }
        public BrinksSignedInfoList(BrinksSignedInfo[] list) : base(list) { }
        public BrinksSignedInfoList(List<BrinksSignedInfo> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
