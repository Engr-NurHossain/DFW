using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "InvoiceList", Namespace = "http://www.piistech.com//list")]	
	public class InvoiceList : BaseCollection<Invoice>
	{
		#region Constructors
	    public InvoiceList() : base() { }
        public InvoiceList(Invoice[] list) : base(list) { }
        public InvoiceList(List<Invoice> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

