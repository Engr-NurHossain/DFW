using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "AddMemberCommissionList", Namespace = "http://www.piistech.com//list")]	
	public class AddMemberCommissionList : BaseCollection<AddMemberCommission>
	{
		#region Constructors
	    public AddMemberCommissionList() : base() { }
        public AddMemberCommissionList(AddMemberCommission[] list) : base(list) { }
        public AddMemberCommissionList(List<AddMemberCommission> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

