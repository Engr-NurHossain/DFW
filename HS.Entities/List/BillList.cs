using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "BillList", Namespace = "http://www.piistech.com//list")]	
	public class BillList : BaseCollection<Bill>
	{
		#region Constructors
	    public BillList() : base() { }
        public BillList(Bill[] list) : base(list) { }
        public BillList(List<Bill> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

