using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "SmartSystemInstallTypeList", Namespace = "http://www.piistech.com//list")]	
	public class SmartSystemInstallTypeList : BaseCollection<SmartSystemInstallType>
	{
		#region Constructors
	    public SmartSystemInstallTypeList() : base() { }
        public SmartSystemInstallTypeList(SmartSystemInstallType[] list) : base(list) { }
        public SmartSystemInstallTypeList(List<SmartSystemInstallType> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

