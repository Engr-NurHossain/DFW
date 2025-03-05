using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "UserLoginList", Namespace = "http://www.piistech.com//list")]	
	public class UserLoginList : BaseCollection<UserLogin>
	{
		#region Constructors
	    public UserLoginList() : base() { }
        public UserLoginList(UserLogin[] list) : base(list) { }
        public UserLoginList(List<UserLogin> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

