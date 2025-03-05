using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;



namespace HS.Entities
{
	public partial class CustomerFile 
	{
        public string CreatedName { get; set; }
        public DateTime SentDate { get; set; }
        public DateTime ExiprationDate { get; set; }
        public Int32 ExpirationDays { get; set; }
        public int LeadFileCount { get; set; }
        public partial class UploadedFileName
        {
            public string CustomerFileName { set; get; }
            public string CustomerFileDescription { set; get; }
        }
    }
    public class LeadActivity
    {
        public int Id { set; get; }
        public string PageUrl { set; get; }
        public string ReferrerUrl { set; get; }
        public string Action { set; get; }
        public string ActionDisplyText { set; get; }
        public string UserIp { set; get; }
        public string UserAgent { set; get; }
        public DateTime StatsDate { set; get; }
        public int PassedTime { set; get; }
        public Guid LeadId { set; get; }
        public string PassedTimeInMin { set; get; }
        public string StatsDateInPMAM { set; get; }
    }

    public class LeadActivityListFilter
    {
        public string Searchtext { set; get; }
        public int PageNo { set; get; }
        public int PageSize { set; get; }
        public Guid CompanyId { set; get; }
        public string order { get; set; }
        public string leadId { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }


    public class LeadActivityViewModel
    {
        public List<LeadActivity> LeadActivityList { set; get; }
        public int TotalTimeInMilliseconds { set; get; }
        public String TotalTime { set; get; }
    }


}
