using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "MarchantInvoiceList", Namespace = "http://www.piistech.com//list")]	
	public class MarchantInvoiceList : BaseCollection<MarchantInvoice>
	{
		#region Constructors
	    public MarchantInvoiceList() : base() { }
        public MarchantInvoiceList(MarchantInvoice[] list) : base(list) { }
        public MarchantInvoiceList(List<MarchantInvoice> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

