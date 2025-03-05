using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "BillDetailList", Namespace = "http://www.piistech.com//list")]	
	public class BillDetailList : BaseCollection<BillDetail>
	{
		#region Constructors
	    public BillDetailList() : base() { }
        public BillDetailList(BillDetail[] list) : base(list) { }
        public BillDetailList(List<BillDetail> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

