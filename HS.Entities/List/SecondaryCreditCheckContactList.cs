using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "SecondaryCreditCheckContactList", Namespace = "http://www.hims-tech.com//list")]	
	public class SecondaryCreditCheckContactList : BaseCollection<SecondaryCreditCheckContact>
	{
		#region Constructors
	    public SecondaryCreditCheckContactList() : base() { }
        public SecondaryCreditCheckContactList(SecondaryCreditCheckContact[] list) : base(list) { }
        public SecondaryCreditCheckContactList(List<SecondaryCreditCheckContact> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
