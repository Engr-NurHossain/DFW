using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "UserCompanyList", Namespace = "http://www.piistech.com//list")]	
	public class UserCompanyList : BaseCollection<UserCompany>
	{
		#region Constructors
	    public UserCompanyList() : base() { }
        public UserCompanyList(UserCompany[] list) : base(list) { }
        public UserCompanyList(List<UserCompany> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

