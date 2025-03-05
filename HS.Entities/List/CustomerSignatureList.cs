using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CustomerSignatureList", Namespace = "http://www.hims-tech.com//list")]	
	public class CustomerSignatureList : BaseCollection<CustomerSignature>
	{
		#region Constructors
	    public CustomerSignatureList() : base() { }
        public CustomerSignatureList(CustomerSignature[] list) : base(list) { }
        public CustomerSignatureList(List<CustomerSignature> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
