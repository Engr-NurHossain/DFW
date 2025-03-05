using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "UserContactList", Namespace = "http://www.piistech.com//list")]	
	public class UserContactList : BaseCollection<UserContact>
	{
		#region Constructors
	    public UserContactList() : base() { }
        public UserContactList(UserContact[] list) : base(list) { }
        public UserContactList(List<UserContact> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

