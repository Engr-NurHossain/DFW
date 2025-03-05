using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.Entities
{
	public partial class UserLogin 
	{
        public string UserType{ set; get; }
        public Nullable<Guid> DefaultCompanyId { set; get; }
        //public Nullable<Guid> EmployeeId { set; get; }
        public string CompanyName { set; get; }
		public string UserTags { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string PhoneNumber { set; get; }
        public string ClockedInOutStatus { set; get; }
        public Nullable<DateTime> ClockedInOutTime { set; get; }
        public bool IsSupervisor { get; set; }
        public string SendMail { get; set; }
        public string Token { get; set; }
    }
}
