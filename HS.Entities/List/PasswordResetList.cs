using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "PasswordResetList", Namespace = "http://www.piistech.com//list")]	
	public class PasswordResetList : BaseCollection<PasswordReset>
	{
		#region Constructors
	    public PasswordResetList() : base() { }
        public PasswordResetList(PasswordReset[] list) : base(list) { }
        public PasswordResetList(List<PasswordReset> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

