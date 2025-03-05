using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

using HS.Framework;

namespace HS.Entities.List
{
	[Serializable]
	[CollectionDataContract(Name = "AnnouncementList", Namespace = "http://www.piistech.com//list")]	
	public class AnnouncementList : BaseCollection<Announcement>
	{
		#region Constructors
	    public AnnouncementList() : base() { }
        public AnnouncementList(Announcement[] list) : base(list) { }
        public AnnouncementList(List<Announcement> list) : base(list) { }
		#endregion
		
		#region Custom Methods
		#endregion
	}	
}

