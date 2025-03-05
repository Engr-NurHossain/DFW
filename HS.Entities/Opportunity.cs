using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class Opportunity 
	{
        public int CustomerIntId { get; set; }
        public string DisplayName { get; set; }
        public string AccountOwnerName { get; set; }
        public string TypeVal { get; set; }
        public string StatusVal { get; set; }
        public string AccessGivenToVal { get; set; }
        public string ProbabilityVal { get; set; }
        public string DealReasonVal { get; set; }
        public string DeliveryDaysVal { get; set; }
        public string CampaignSourceVal { get; set; }
        public string CustomerName { get; set; }
        public int IdCustomer { get; set; }
        public string LeadSourceVal { get; set; }
        public string FromCustomer { get; set; }
        public bool opportunityTab { get; set; }
        public string SourceLead { get; set; }
        public string ReasonDeal { get; set; }
        public string DeliverydayVal { get; set; }
        public string DeliveryDayDiff { get; set; }
        public string EmpName { get; set; }
        public string CustomerStreet { get; set; }
        public string CustomerCity { get; set; }
        public string CustomerState { get; set; }
        public string CustomerZip { get; set; }
        public string CustomerEmail { get; set; }
        public string MonthOpportunity { get; set; }
        public int CusIdInt { get; set; }
        public string CustomerDBA { get; set; }
        public string CustomerBusinessName { get; set; }
        public string VehicleConditionVal { get; set; }
    }
    public class OpportunityFilter
    {
        public int? UnitPerPage { get; set; }
        public int? PageNumber { get; set; }
        public string Order { get; set; }
        public string SearchText { get; set; }
        public string Type { get; set; }
        public string OpporStatus { get; set; }
        public string OpportunityProbability { get; set; }
        public string OpportunityDealReason { get; set; }
        public string OpportunityDeliveryDays { get; set; }
        public string OpportunityCampaignSource { get; set; }
        public string AccountOwner { get; set; }
        public string RevenueFrom { get; set; }
        public string RevenueTo { get; set; }
        public string ProjectedGpFrom { get; set; }
        public string ProjectedGpTo { get; set; }
        public string PointFrom { get; set; }
        public string PointTo { get; set; }
        public Guid CustomerId { get; set; }
        public Guid AccountOwnerId { get; set; }
        public string CreatedDateFrom { get; set; }
        public string CreatedDateTo { get; set; }
        

    }
    public class OpportunityCount
    {
        public int TotalCount { get; set; }
    }
    public class OpportunityModel
    {
        public List<Opportunity> OpportunityList { get; set; }
        public OpportunityCount TotalCount { get; set; }
    }

    public class OpportunityListFilterModel
    {
        public List<Opportunity> ListOpportunity { get; set; }
        public List<Opportunity> ListOpportunityJan { get; set; }
        public List<Opportunity> ListOpportunityFeb { get; set; }
        public List<Opportunity> ListOpportunityMar { get; set; }
        public List<Opportunity> ListOpportunityApr { get; set; }
        public List<Opportunity> ListOpportunityMay { get; set; }
        public List<Opportunity> ListOpportunityJun { get; set; }
        public List<Opportunity> ListOpportunityJul { get; set; }
        public List<Opportunity> ListOpportunityAug { get; set; }
        public List<Opportunity> ListOpportunitySep { get; set; }
        public List<Opportunity> ListOpportunityOct { get; set; }
        public List<Opportunity> ListOpportunityNov { get; set; }
        public List<Opportunity> ListOpportunityDec { get; set; }
        public TotalOpportunityCountModel TotalOpportunityCountModel { get; set; }
        public List<RevenueModel> RevenueModel { get; set; }
        public HardBacklogRevenueModel HardBacklogRevenueModelJan { get; set; }
        public HardBacklogRevenueModel HardBacklogRevenueModelFeb { get; set; }
        public HardBacklogRevenueModel HardBacklogRevenueModelMar { get; set; }
        public HardBacklogRevenueModel HardBacklogRevenueModelApr { get; set; }
        public HardBacklogRevenueModel HardBacklogRevenueModelMay { get; set; }
        public HardBacklogRevenueModel HardBacklogRevenueModelJun { get; set; }
        public HardBacklogRevenueModel HardBacklogRevenueModelJul { get; set; }
        public HardBacklogRevenueModel HardBacklogRevenueModelAug { get; set; }
        public HardBacklogRevenueModel HardBacklogRevenueModelSep { get; set; }
        public HardBacklogRevenueModel HardBacklogRevenueModelOct { get; set; }
        public HardBacklogRevenueModel HardBacklogRevenueModelNov { get; set; }
        public HardBacklogRevenueModel HardBacklogRevenueModelDec { get; set; }
    }

    public class TotalOpportunityCountModel
    {
        public int TotalCount { get; set; }
    }

    public class RevenueModel
    {
        public double TotalRevenueVal { get; set; }
        public double TotalProjectedGPVal { get; set; }
        public double TotalPointsVal { get; set; }
        public double ProjectedGPTotalVal { get; set; }
        public string DeliveryDayDiff { get; set; }
    }

    public class HardBacklogRevenueModel
    {
        public double TotalRevenueVal { get; set; }
        public double TotalProjectedGPVal { get; set; }
        public double TotalPointsVal { get; set; }
        public double ProjectedGPTotalVal { get; set; }
    }
}
