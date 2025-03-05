using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "SystemTypeServiceMapList", Namespace = "http://www.piistech.com//list")]	
	public class SystemTypeServiceMapList : BaseCollection<SystemTypeServiceMap>
	{
		#region Constructors
	    public SystemTypeServiceMapList() : base() { }
        public SystemTypeServiceMapList(SystemTypeServiceMap[] list) : base(list) { }
        public SystemTypeServiceMapList(List<SystemTypeServiceMap> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

