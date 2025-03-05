using HS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace HS.Web.UI.Helper
{
    interface ICustomPrincipal : IPrincipal
    {
        Guid UserId { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
    }
    public class CustomPrincipal : ClaimsPrincipal, ICustomPrincipal
    {
        #region IPrincipal Members
        public new ClaimsIdentity Identity { get; private set; }

        #endregion
        public CustomPrincipal(UserLogin user, IIdentity identity)
           : base(identity)
        {
            this.Identity = new ClaimsIdentity(identity);

            this.UserId = user.UserId;
            this.DateCreated = user.LastUpdatedDate.HasValue ? user.LastUpdatedDate.Value : DateTime.Now;
            this.UserRole = user.UserType;
            this.CompanyId = user.DefaultCompanyId;
            this.CompanyName = user.CompanyName;
            this.UserTags = user.UserTags;
            this.EmailAddress = user.EmailAddress;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.PhoneNumber = user.PhoneNumber;
            this.IsClockedIn = user.ClockedInOutStatus == LabelHelper.TimeClockType.ClockIn;
            this.IsSupervisor = user.IsSupervisor;
            this.ClockInOutTime = user.ClockedInOutTime;

            //HttpContext.Current.Session[SessionKeys.UserName] = identity.Name;
            //HttpContext.Current.Session[SessionKeys.UserRole] = user.UserType;
        }

        #region ICustomPrinicpal Members
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserRole { get; set; }
        public string PhoneNumber { get; set; }
        public string CompanyName { set; get; }
        public string ProfilePicture { get; set; }
        public string UserTags { set; get; }
        public string EmailAddress { get; set; }
        public Nullable<Guid> CompanyId { set; get; }
        public DateTime DateCreated { get; set; }
        public bool IsClockedIn { set; get; }
        public Nullable<DateTime> ClockInOutTime { set; get; }
        public bool IsSupervisor { set; get; }
        //public Nullable<Guid> EmployeeId { set; get; }
        #endregion

        public string GetFullName()
        {
            return this.FirstName + " " + this.LastName;
        }
    }
}