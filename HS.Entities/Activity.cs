using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class Activity 
	{
        public int CustomerId { get; set; }
        public int OpportunityId { get; set; }
        public int ContactId { get; set; }
        public string CreatedByVal { get; set; }
        public string ActivityTypeVal { get; set; }
        public string AssociatedTypeVal { get; set; }
        public string AssignedToVal { get; set; }
        public string AssociatedWithVal { get; set; }
        public string DisplayName { get; set; }
        public string AssociatedOpportunityVal { get; set; }
        public string AssociatedContactVal { get; set; }
        public bool FromCustomer { get; set; }
        public bool ActivityTab { get; set; }
        public string EmpName { get; set; }
        public string AssociatedCustomer { get; set; }
        public string AssociatedOpportunity { get; set; }
        public string AssociatedContact { get; set; }
        public string OriginVal { get; set; }
        public string DepartmentVal { get; set; }
    }
    public class ActivityCount
    {
        public int TotalCount { get; set; }
    }
    public class ActivityModel
    {
        public List<Activity> ActivityList { get; set; }
        public ActivityCount TotalCount { get; set; }
    }
    public class ActivityFilter
    {
        public int? UnitPerPage { get; set; }
        public int? PageNumber { get; set; }
        public string Order { get; set; }
        public string SearchText { get; set; }
        public string ActivityType { get; set; }
        public string ActivityStatus { get; set; }
        public string AssignTo { get; set; }
        public DateTime DueDateFrom { get; set; }
        public DateTime DueDateTo { get; set; }
        public DateTime CreatedDateFrom { get; set; }
        public DateTime CreatedDateTo { get; set; }
        public Guid CustomerId { get; set; }
        public List<string> ActivityList { get; set; }
        public Guid AssignToId { get; set; }

    }

    public class ActivityListFilterModel
    {
        public List<Activity> ListActivity { get; set; }
        public TotalActivityCountModel TotalActivityCountModel { get; set; }
    }

    public class TotalActivityCountModel
    {
        public int TotalCount { get; set; }
    }
}
