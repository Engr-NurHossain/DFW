using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "ActivityList", Namespace = "http://www.piistech.com//list")]	
	public class ActivityList : BaseCollection<Activity>
	{
		#region Constructors
	    public ActivityList() : base() { }
        public ActivityList(Activity[] list) : base(list) { }
        public ActivityList(List<Activity> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

