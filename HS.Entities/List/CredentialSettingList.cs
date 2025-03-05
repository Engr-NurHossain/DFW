using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "CredentialSettingList", Namespace = "http://www.piistech.com//list")]	
	public class CredentialSettingList : BaseCollection<CredentialSetting>
	{
		#region Constructors
	    public CredentialSettingList() : base() { }
        public CredentialSettingList(CredentialSetting[] list) : base(list) { }
        public CredentialSettingList(List<CredentialSetting> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

