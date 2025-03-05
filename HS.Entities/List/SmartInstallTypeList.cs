using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "SmartInstallTypeList", Namespace = "http://www.piistech.com//list")]	
	public class SmartInstallTypeList : BaseCollection<SmartInstallType>
	{
		#region Constructors
	    public SmartInstallTypeList() : base() { }
        public SmartInstallTypeList(SmartInstallType[] list) : base(list) { }
        public SmartInstallTypeList(List<SmartInstallType> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

