using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "AccountHolderList", Namespace = "http://www.piistech.com//list")]	
	public class AccountHolderList : BaseCollection<AccountHolder>
	{
		#region Constructors
	    public AccountHolderList() : base() { }
        public AccountHolderList(AccountHolder[] list) : base(list) { }
        public AccountHolderList(List<AccountHolder> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

