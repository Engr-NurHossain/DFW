using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "BuildLogList", Namespace = "http://www.piistech.com//list")]	
	public class BuildLogList : BaseCollection<BuildLog>
	{
		#region Constructors
	    public BuildLogList() : base() { }
        public BuildLogList(BuildLog[] list) : base(list) { }
        public BuildLogList(List<BuildLog> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}
