using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "AuthDataList", Namespace = "http://www.piistech.com//list")]	
	public class AuthDataList : BaseCollection<AuthData>
	{
		#region Constructors
	    public AuthDataList() : base() { }
        public AuthDataList(AuthData[] list) : base(list) { }
        public AuthDataList(List<AuthData> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

