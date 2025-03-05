using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class PermissionGroup 
	{
        public int UserCount { set; get; }
        public int TotalUserCount { get; set; }

        public List<UserMgmtList> UserList { get; set; }
    }
    public partial class PermissionGroupWithUserList
    {
        public int PermissionGroupId { set; get; }

        public Guid CompanyId { set; get; }

        public string Name { set; get; }

        public string Tag { set; get; }


        public int UserCount { set; get; }
        public int TotalUserCount { get; set; }

        public List<PermissionGroup> PermissionGroupList { get; set; }

        

    }
}
