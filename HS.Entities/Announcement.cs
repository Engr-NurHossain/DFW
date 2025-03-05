using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class Announcement 
	{
        public string CreatedbyName { get; set; }
    }
    public partial class AnnouncementExtend
    {
        public List<Announcement> AnnouncementList { get; set; }
        public int AnnouncementListCount { get; set; }
    }
}
