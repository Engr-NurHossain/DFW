using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class Notification 
	{ 
        public string LastVisited { set; get; }
        public string WhoVal { set; get; }
        public bool IsRead { set; get; }
        public bool KnowledgebaseIsRead { get; set; }
    }
    public class NotificationViewModel
    { 
        public List<Notification> Notifications { set; get; }
        public int TotalCount { set; get; }
        public int DueDay { get; set; }
    }
    public class CountNotification
    {
        public int NotificationCount { set; get; }
        public int AnnouncementCount { set; get; }
    }

}
