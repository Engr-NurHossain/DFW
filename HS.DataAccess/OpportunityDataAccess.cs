using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;
using System.Linq;
namespace HS.DataAccess
{
    public partial class OpportunityDataAccess
    {
        public OpportunityDataAccess(string connStr) : base(connStr) { }

        public OpportunityModel GetOpportunities(OpportunityFilter filter)
        {
            string searchTextQuery = "";
            string subquery = "";
            string subquery1 = "";
            string filterQuery = "";
            string subqueryCustomer = "";


            string CustomerIdFilter = "";
            string CountFilter = "";
            if (filter.CustomerId != new Guid() && filter.CustomerId != null)
            {
                CustomerIdFilter = string.Format("c.CustomerId = '{0}' and", filter.CustomerId);
                CountFilter = string.Format(" where c.CustomerId = '{0}'", filter.CustomerId);
            }

            if (!string.IsNullOrEmpty(filter.Type) && filter.Type != "-1")
            {
                filterQuery += string.Format(" c.Type = '{0}' and", filter.Type);
            }
            if (!string.IsNullOrEmpty(filter.OpporStatus) && filter.OpporStatus != "-1")
            {
                filterQuery += string.Format(" c.Status = '{0}' and", filter.OpporStatus); ;
            }
            if (!string.IsNullOrEmpty(filter.OpportunityProbability) && filter.OpportunityProbability != "-1")
            {
                filterQuery += string.Format(" c.Probability = '{0}' and", filter.OpportunityProbability);
            }
            if (!string.IsNullOrEmpty(filter.OpportunityDealReason) && filter.OpportunityDealReason != "-1")
            {
                filterQuery += string.Format(" c.DealReason = '{0}' and", filter.OpportunityDealReason);
            }

            if (!string.IsNullOrEmpty(filter.OpportunityDeliveryDays) && filter.OpportunityDeliveryDays != "-1")
            {
                filterQuery += string.Format(" c.DeliveryDays = '{0}' and", filter.OpportunityDeliveryDays);
            }
            if (!string.IsNullOrEmpty(filter.OpportunityCampaignSource) && filter.OpportunityCampaignSource != "-1")
            {
                filterQuery += string.Format(" c.CampaignSource = '{0}' and", filter.OpportunityCampaignSource);
            }

            if (filter.AccountOwnerId != new Guid() && filter.AccountOwnerId != null)
            {
                filterQuery += string.Format(" (c.AccountOwner = '{0}') and", filter.AccountOwnerId);
            }
            if (!string.IsNullOrEmpty(filter.RevenueFrom) && !string.IsNullOrEmpty(filter.RevenueTo))
            {
                filterQuery += string.Format(" CAST(c.Revenue as float) between {0} and {1} and", filter.RevenueFrom, filter.RevenueTo);
            }
            if (!string.IsNullOrEmpty(filter.ProjectedGpFrom) && !string.IsNullOrEmpty(filter.ProjectedGpTo))
            {
                filterQuery += string.Format(" CAST(c.ProjectedGp as float) between {0} and {1} and", filter.ProjectedGpFrom, filter.ProjectedGpTo);
            }
            if (!string.IsNullOrEmpty(filter.PointFrom) && !string.IsNullOrEmpty(filter.PointTo))
            {
                filterQuery += string.Format(" CAST(c.Points  as float) between {0} and {1} and", filter.PointFrom, filter.PointTo);
            }
            if ((filter.CreatedDateFrom != "01/01/0001" && filter.CreatedDateTo != "01/01/0001") && (!string.IsNullOrEmpty(filter.CreatedDateTo) && !string.IsNullOrEmpty(filter.CreatedDateFrom)))
            {
                filterQuery += string.Format(" c.CreatedDate between '{0}' and '{1}' and", Convert.ToDateTime(filter.CreatedDateFrom).SetZeroHour().ClientToUTCTime(), Convert.ToDateTime(filter.CreatedDateTo).SetMaxHour().SetMaxHour());

            }

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                searchTextQuery = @"   c.OpportunityName like @SearchText 
                                    or c.Type like @SearchText 
                                    or c.LeadSource like @SearchText 
                                    or c.Revenue like @SearchText 
                                    or c.AccountOwner like @SearchText and ";
            }
            if (filter.CustomerId != new Guid())
            {
                subqueryCustomer = string.Format("and c.CustomerId = '{0}'", filter.CustomerId);
            }
            List<Opportunity> OpportunityList = new List<Opportunity>();
            OpportunityCount TotalOpportunity = new OpportunityCount();
            string rawQuery = @"  
                                declare @pagesize int
                                declare @pageno int 
                                set @pagesize = " + filter.UnitPerPage + @"
                                set @pageno = " + filter.PageNumber + @"
                                declare @pagestart int
                                set @pagestart=(@pageno-1)* @pagesize  
                                select  * into #temptable from (select  c.*,
                                CASE WHEN c.Type = '-1' or c.Type is null  THEN '-' else lk.DisplayText  end as TypeVal,
                                CASE WHEN c.LeadSource = '-1' or c.LeadSource  IS NULL   THEN '-' else lksource.DisplayText end as LeadSourceVal,
                                CASE WHEN c.Status = '-1' or c.Status  IS NULL   THEN '-' else lkstatus.DisplayText end as StatusVal,
                                CASE WHEN c.Probability = '-1' or c.Probability  IS NULL   THEN '-' else lkp.DisplayText end as ProbabilityVal,
                                CASE WHEN c.DealReason = '-1' or c.DealReason  IS NULL   THEN '-' else lkoppdeal.DisplayText end as DealReasonVal,
                                CASE WHEN c.DeliveryDays = '-1' or c.DeliveryDays  IS NULL   THEN '-' else lkdelivery.DisplayText end as DeliveryDaysVal,
                                CASE WHEN c.CampaignSource = '-1' or c.CampaignSource  IS NULL   THEN '-' else lkcampaign.DisplayText end as CampaignSourceVal,
                                CASE WHEN c.VehicleCondition = '-1' or c.VehicleCondition is null  THEN '-' else lv.DisplayText  end as VehicleConditionVal,
                                cus.FirstName+' '+cus.LastName as CustomerName,
                                emp.FirstName+' '+emp.LastName as AccountOwnerName,
                                {7} as DisplayName,
                                cus.Id as CustomerIntId
                                FROM Opportunity c
                                
                                left join employee emp on emp.UserId = c.AccountOwner
                                left join customer cus on cus.CustomerId = c.CustomerId
                                left join [Lookup] lk on lk.DataKey = 'OpportunityType' and lk.DataValue = c.Type
                                left join [Lookup] lksource on lksource.DataKey = 'LeadSource' and lksource.DataValue = c.LeadSource
                                left join [Lookup] lkstatus on lkstatus.DataKey = 'OpportunityStatus' and lkstatus.DataValue = c.Status
                                left join [Lookup] lkp on lkp.DataKey = 'OpportunityProbability' and lkp.DataValue = c.Probability
                                left join [Lookup] lkoppdeal on lkoppdeal.DataKey = 'OpportunityDealReason' and lkoppdeal.DataValue = c.DealReason
                                left join [Lookup] lv on lv.DataKey = 'VehicleCondition' and lv.DataValue = c.VehicleCondition

                                left join [Lookup] lkdelivery on lkdelivery.DataKey = 'OpportunityDeliveryDays' and lkdelivery.DataValue = c.DeliveryDays
                                left join [Lookup] lkcampaign on lkcampaign.DataKey = 'OpportunityCampaignSource' and lkcampaign.DataValue = c.CampaignSource
                                where  {2}{1}{5} c.Id > 0
                               
                                ) a
	                            select * into #ftemp from #temptable 
								 select TOP (@pagesize) *  from #ftemp f
                                where  f.Id NOT IN(Select TOP (@pagestart) Id from #ftemp  order by Id desc)
                                {0}
                               select Count(Id) As TotalCount from #ftemp  

								drop table #temptable
								drop table #ftemp";



            #region Order
            if (!string.IsNullOrWhiteSpace(filter.Order))
            {
                if (filter.Order == "ascending/opportunityname")
                {
                    subquery = "order by OpportunityName asc";

                }
                else if (filter.Order == "descending/opportunityname")
                {
                    subquery = "order by OpportunityName desc";

                }
                else if (filter.Order == "ascending/type")
                {
                    subquery = "order by Type asc";

                }
                else if (filter.Order == "descending/type")
                {
                    subquery = "order by Type desc";

                }
                else if (filter.Order == "ascending/leadsource")
                {
                    subquery = "order by LeadSource asc";

                }
                else if (filter.Order == "descending/leadsource")
                {
                    subquery = "order by LeadSource desc";

                }
                else if (filter.Order == "ascending/revenue")
                {
                    subquery = "order by Revenue asc";

                }
                else if (filter.Order == "descending/revenue")
                {
                    subquery = "order by Revenue desc";

                }
                else if (filter.Order == "ascending/projectedgp")
                {
                    subquery = "order by ProjectedGP asc";

                }
                else if (filter.Order == "descending/projectedgp")
                {
                    subquery = "order by ProjectedGP desc";

                }
                else if (filter.Order == "ascending/points")
                {
                    subquery = "order by Points asc";

                }
                else if (filter.Order == "descending/points")
                {
                    subquery = "order by Points desc";

                }
                else if (filter.Order == "ascending/totalprojectedgp")
                {
                    subquery = "order by TotalProjectedGP asc";

                }
                else if (filter.Order == "descending/totalprojectedgp")
                {
                    subquery = "order by  TotalProjectedGP desc";

                }

                else if (filter.Order == "ascending/status")
                {
                    subquery = "order by Status asc";

                }
                else if (filter.Order == "descending/status")
                {
                    subquery = "order by  Status desc";

                }

                else if (filter.Order == "ascending/probability")
                {
                    subquery = "order by Probability asc";

                }
                else if (filter.Order == "descending/probability")
                {
                    subquery = "order by  Probability desc";

                }

                else if (filter.Order == "ascending/dealreason")
                {
                    subquery = "order by DealReason asc";

                }
                else if (filter.Order == "descending/deliveryDays")
                {
                    subquery = "order by  DeliveryDays desc";

                }

                else if (filter.Order == "ascending/deliveryDays")
                {
                    subquery = "order by DeliveryDays asc";

                }
                else if (filter.Order == "descending/campaignSource")
                {
                    subquery = "order by  CampaignSource desc";

                }

                else if (filter.Order == "ascending/accountOwner")
                {
                    subquery = "order by accountOwner asc";

                }
                else if (filter.Order == "descending/accountOwner")
                {
                    subquery = "order by  accountOwner desc";

                }
                else if (filter.Order == "ascending/closeDate")
                {
                    subquery = "order by CloseDate asc";

                }
                else if (filter.Order == "descending/closeDate")
                {
                    subquery = "order by  CloseDate desc";

                }
                else if (filter.Order == "ascending/createddate")
                {
                    subquery = "order by  CreatedDate desc";

                }
                else if (filter.Order == "descending/createddate")
                {
                    subquery = "order by  CreatedDate desc";

                }
            }
            else
            {
                subquery = "order by Id desc";
                subquery1 = "order by Id desc";
            }
            #endregion
            #region Naming Condition
            string NamingSql = "''";
            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            if (gs != null)
            {
                NamingSql = gs.Value;
            }
            #endregion
            rawQuery = string.Format(rawQuery, subquery, searchTextQuery, filterQuery, subquery1, subqueryCustomer, CustomerIdFilter, CountFilter, NamingSql);
            using (SqlCommand cmd = GetSQLCommand(rawQuery))
            {
                AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", filter.SearchText)));
                DataSet dsResult = GetDataSet(cmd);
                DataTable dt = dsResult.Tables[0];
                DataTable dt1 = dsResult.Tables[1];
                try
                {
                    OpportunityList = (from DataRow dr in dt.Rows
                                       select new Opportunity()
                                       {
                                           Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                           OpportunityName = dr["OpportunityName"].ToString(),
                                           TypeVal = dr["TypeVal"].ToString(),
                                           DisplayName = dr["DisplayName"].ToString(),
                                           CustomerIntId = dr["CustomerIntId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerIntId"]) : 0,
                                           LeadSource = dr["LeadSource"].ToString(),
                                           Revenue = dr["Revenue"].ToString(),
                                           ProjectedGP = dr["ProjectedGP"].ToString(),
                                           Points = dr["Points"].ToString(),
                                           TotalProjectedGP = dr["TotalProjectedGP"].ToString(),
                                           CustomerName = dr["CustomerName"].ToString(),
                                           StatusVal = dr["StatusVal"].ToString(),
                                           ProbabilityVal = dr["ProbabilityVal"].ToString(),
                                           DealReasonVal = dr["DealReasonVal"].ToString(),
                                           DeliveryDaysVal = dr["DeliveryDaysVal"].ToString(),
                                           Competitors = dr["Competitors"].ToString(),
                                           AccountOwnerName = dr["AccountOwnerName"].ToString(),
                                           CampaignSourceVal = dr["CampaignSourceVal"].ToString(),
                                           AccountOwner = (Guid)dr["CustomerId"],
                                           VehicleCondition = dr["VehicleCondition"].ToString(),
                                           VehicleConditionVal = dr["VehicleConditionVal"].ToString(),
                                           LeadSourceVal = dr["LeadSourceVal"].ToString(),
                                           CloseDate = dr["CloseDate"] != DBNull.Value ? Convert.ToDateTime(dr["CloseDate"]) : new DateTime(),
                                           CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                       }).ToList();


                    TotalOpportunity = (from DataRow dr in dt1.Rows
                                        select new OpportunityCount()
                                        {
                                            TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                        }).FirstOrDefault();

                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            OpportunityModel opportunity = new OpportunityModel();
            opportunity.OpportunityList = OpportunityList;
            opportunity.TotalCount = TotalOpportunity;
            return opportunity;
        }

        public List<Opportunity> GetOpportunitiesBySearchKey(string key, string employeeTag, Guid empid)
        {
            string searchTextQuery = "";
            if (!string.IsNullOrWhiteSpace(key))
            {
                searchTextQuery = " where c.OpportunityName like @SearchText or c.Type like @SearchText or c.LeadSource like @SearchText or c.Revenue like @SearchText or c.AccountOwner like @SearchText ";
            }
            List<Opportunity> OpportunityList = new List<Opportunity>();
            string rawQuery = @" select  c.*,
                                CASE WHEN c.Type = '-1' or c.Type is null  THEN '-' else lk.DisplayText  end as TypeVal,
                                CASE WHEN c.LeadSource = '-1' or c.LeadSource  IS NULL   THEN '-' else lksource.DisplayText end as LeadSourceVal,
                                CASE WHEN c.Status = '-1' or c.Status  IS NULL   THEN '-' else lkstatus.DisplayText end as StatusVal,
                                CASE WHEN c.Probability = '-1' or c.Probability  IS NULL   THEN '-' else lkp.DisplayText end as ProbabilityVal,
                                CASE WHEN c.DealReason = '-1' or c.DealReason  IS NULL   THEN '-' else lkoppdeal.DisplayText end as DealReasonVal,
                                CASE WHEN c.DeliveryDays = '-1' or c.DeliveryDays  IS NULL   THEN '-' else lkdelivery.DisplayText end as DeliveryDaysVal,
                                CASE WHEN c.CampaignSource = '-1' or c.CampaignSource  IS NULL   THEN '-' else lkcampaign.DisplayText end as CampaignSourceVal,
                                cus.FirstName+' '+cus.LastName as CustomerName,emp.FirstName+' '+emp.LastName as AccountOwnerName  FROM Opportunity c
                                left join employee emp on emp.UserId = c.AccountOwner
                                left join customer cus on cus.CustomerId = c.CustomerId
                                left join [Lookup] lk on lk.DataKey = 'OpportunityType' and lk.DataValue = c.Type
                                left join [Lookup] lksource on lksource.DataKey = 'LeadSource' and lksource.DataValue = c.LeadSource
                                left join [Lookup] lkstatus on lkstatus.DataKey = 'OpportunityStatus' and lkstatus.DataValue = c.Status
                                left join [Lookup] lkp on lkp.DataKey = 'OpportunityProbability' and lkp.DataValue = c.Probability
                                left join [Lookup] lkoppdeal on lkoppdeal.DataKey = 'OpportunityDealReason' and lkoppdeal.DataValue = c.DealReason

                                left join [Lookup] lkdelivery on lkdelivery.DataKey = 'OpportunityDeliveryDays' and lkdelivery.DataValue = c.DeliveryDays
                                left join [Lookup] lkcampaign on lkcampaign.DataKey = 'OpportunityCampaignSource' and lkcampaign.DataValue = c.CampaignSource
                                {0} 
                                ";

            rawQuery = string.Format(rawQuery, searchTextQuery);
            using (SqlCommand cmd = GetSQLCommand(rawQuery))
            {
                AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", key)));
                DataSet dsResult = GetDataSet(cmd);
                DataTable dt = dsResult.Tables[0];

                try
                {
                    OpportunityList = (from DataRow dr in dt.Rows
                                       select new Opportunity()
                                       {
                                           Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                           OpportunityName = dr["OpportunityName"].ToString(),
                                           TypeVal = dr["TypeVal"].ToString(),
                                           LeadSource = dr["LeadSource"].ToString(),
                                           Revenue = dr["Revenue"].ToString(),
                                           ProjectedGP = dr["ProjectedGP"].ToString(),
                                           Points = dr["Points"].ToString(),
                                           TotalProjectedGP = dr["TotalProjectedGP"].ToString(),
                                           CustomerName = dr["CustomerName"].ToString(),
                                           StatusVal = dr["StatusVal"].ToString(),
                                           ProbabilityVal = dr["ProbabilityVal"].ToString(),
                                           DealReasonVal = dr["DealReasonVal"].ToString(),
                                           DeliveryDaysVal = dr["DeliveryDaysVal"].ToString(),
                                           Competitors = dr["Competitors"].ToString(),
                                           AccountOwnerName = dr["AccountOwnerName"].ToString(),
                                           CampaignSourceVal = dr["CampaignSourceVal"].ToString(),
                                           AccountOwner = (Guid)dr["CustomerId"],

                                           LeadSourceVal = dr["LeadSourceVal"].ToString(),
                                           CloseDate = dr["CloseDate"] != DBNull.Value ? Convert.ToDateTime(dr["CloseDate"]) : new DateTime(),
                                       }).ToList();


                }
                catch (Exception ex)
                {
                    return null;
                }
            }

            return OpportunityList;
        }

        public List<Opportunity> GetFilteredOpportunities(OpportunityFilter filter)
        {
            string searchTextQuery = "";
            string filterQuery = "";
            if (!string.IsNullOrEmpty(filter.Type) && filter.Type != "-1")
            {
                filterQuery += string.Format(" c.Type = '{0}' and", filter.Type);
            }
            if (!string.IsNullOrEmpty(filter.OpporStatus) && filter.OpporStatus != "-1")
            {
                filterQuery += string.Format(" c.Status = '{0}' and", filter.OpporStatus); ;
            }
            if (!string.IsNullOrEmpty(filter.OpportunityProbability) && filter.OpportunityProbability != "-1")
            {
                filterQuery += string.Format(" c.Probability = '{0}' and", filter.OpportunityProbability);
            }
            if (!string.IsNullOrEmpty(filter.OpportunityDealReason) && filter.OpportunityDealReason != "-1")
            {
                filterQuery += string.Format(" c.DealReason = '{0}' and", filter.OpportunityDealReason);
            }

            if (!string.IsNullOrEmpty(filter.OpportunityDeliveryDays) && filter.OpportunityDeliveryDays != "-1")
            {
                filterQuery += string.Format(" c.DeliveryDays = '{0}' and", filter.OpportunityDeliveryDays);
            }
            if (!string.IsNullOrEmpty(filter.OpportunityCampaignSource) && filter.OpportunityCampaignSource != "-1")
            {
                filterQuery += string.Format(" c.CampaignSource = '{0}' and", filter.OpportunityCampaignSource);
            }

            if (!string.IsNullOrEmpty(filter.AccountOwner) && filter.AccountOwner != "-1")
            {
                filterQuery += string.Format(" c.AccountOwner = '{0}' and ", filter.AccountOwner);
            }
            if (!string.IsNullOrEmpty(filter.RevenueFrom) && !string.IsNullOrEmpty(filter.RevenueTo))
            {
                filterQuery += string.Format(" CAST(c.Revenue as float) between {0} and {1} and", filter.RevenueFrom, filter.RevenueTo);
            }
            if (!string.IsNullOrEmpty(filter.ProjectedGpFrom) && !string.IsNullOrEmpty(filter.ProjectedGpTo))
            {
                filterQuery += string.Format(" CAST(c.ProjectedGp as float) between {0} and {1} and", filter.ProjectedGpFrom, filter.ProjectedGpTo);
            }
            if (!string.IsNullOrEmpty(filter.PointFrom) && !string.IsNullOrEmpty(filter.PointTo))
            {
                filterQuery += string.Format(" CAST(c.Points  as float) between {0} and {1} and", filter.PointFrom, filter.PointTo);
            }

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                searchTextQuery = " where c.OpportunityName like @SearchText or c.Type like @SearchText or c.LeadSource like @SearchText or c.Revenue like @SearchText or c.AccountOwner like @SearchText and ";
            }
            if (searchTextQuery == "" && filterQuery != "")
            {
                filterQuery = "Where " + filterQuery;
            }
            List<Opportunity> OpportunityList = new List<Opportunity>();

            string rawQuery = @" select  c.*,
                                CASE WHEN c.Type = '-1' or c.Type is null  THEN '-' else lk.DisplayText  end as TypeVal,
                                CASE WHEN c.LeadSource = '-1' or c.LeadSource  IS NULL   THEN '-' else lksource.DisplayText end as LeadSourceVal,
                                CASE WHEN c.Status = '-1' or c.Status  IS NULL   THEN '-' else lkstatus.DisplayText end as StatusVal,
                                CASE WHEN c.Probability = '-1' or c.Probability  IS NULL   THEN '-' else lkp.DisplayText end as ProbabilityVal,
                                CASE WHEN c.DealReason = '-1' or c.DealReason  IS NULL   THEN '-' else lkoppdeal.DisplayText end as DealReasonVal,
                                CASE WHEN c.DeliveryDays = '-1' or c.DeliveryDays  IS NULL   THEN '-' else lkdelivery.DisplayText end as DeliveryDaysVal,
                                CASE WHEN c.CampaignSource = '-1' or c.CampaignSource  IS NULL   THEN '-' else lkcampaign.DisplayText end as CampaignSourceVal,
                                cus.FirstName+' '+cus.LastName as CustomerName,emp.FirstName+' '+emp.LastName as AccountOwnerName  FROM Opportunity c
                                left join employee emp on emp.UserId = c.AccountOwner
                                left join customer cus on cus.CustomerId = c.CustomerId
                                left join [Lookup] lk on lk.DataKey = 'OpportunityType' and lk.DataValue = c.Type
                                left join [Lookup] lksource on lksource.DataKey = 'LeadSource' and lksource.DataValue = c.LeadSource
                                left join [Lookup] lkstatus on lkstatus.DataKey = 'OpportunityStatus' and lkstatus.DataValue = c.Status
                                left join [Lookup] lkp on lkp.DataKey = 'OpportunityProbability' and lkp.DataValue = c.Probability
                                left join [Lookup] lkoppdeal on lkoppdeal.DataKey = 'OpportunityDealReason' and lkoppdeal.DataValue = c.DealReason

                                left join [Lookup] lkdelivery on lkdelivery.DataKey = 'OpportunityDeliveryDays' and lkdelivery.DataValue = c.DeliveryDays
                                left join [Lookup] lkcampaign on lkcampaign.DataKey = 'OpportunityCampaignSource' and lkcampaign.DataValue = c.CampaignSource
                                {0}{1}
                                ";

            rawQuery = string.Format(rawQuery, searchTextQuery, filterQuery);
            if (filterQuery != "" || searchTextQuery != "")
            {
                rawQuery = rawQuery.Remove(rawQuery.Length - 38);
            }
            using (SqlCommand cmd = GetSQLCommand(rawQuery))
            {
                AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", filter.SearchText)));
                DataSet dsResult = GetDataSet(cmd);
                DataTable dt = dsResult.Tables[0];

                try
                {
                    OpportunityList = (from DataRow dr in dt.Rows
                                       select new Opportunity()
                                       {
                                           Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                           OpportunityName = dr["OpportunityName"].ToString(),
                                           TypeVal = dr["TypeVal"].ToString(),
                                           LeadSource = dr["LeadSource"].ToString(),
                                           Revenue = dr["Revenue"].ToString(),
                                           ProjectedGP = dr["ProjectedGP"].ToString(),
                                           Points = dr["Points"].ToString(),
                                           TotalProjectedGP = dr["TotalProjectedGP"].ToString(),
                                           CustomerName = dr["CustomerName"].ToString(),
                                           StatusVal = dr["StatusVal"].ToString(),
                                           ProbabilityVal = dr["ProbabilityVal"].ToString(),
                                           DealReasonVal = dr["DealReasonVal"].ToString(),
                                           DeliveryDaysVal = dr["DeliveryDaysVal"].ToString(),
                                           Competitors = dr["Competitors"].ToString(),
                                           AccountOwnerName = dr["AccountOwnerName"].ToString(),
                                           CampaignSourceVal = dr["CampaignSourceVal"].ToString(),
                                           AccountOwner = (Guid)dr["CustomerId"],

                                           LeadSourceVal = dr["LeadSourceVal"].ToString(),
                                           CloseDate = dr["CloseDate"] != DBNull.Value ? Convert.ToDateTime(dr["CloseDate"]) : new DateTime(),
                                       }).ToList();


                }
                catch (Exception ex)
                {
                    return null;
                }
            }

            return OpportunityList;
        }

        public DataTable GetOpportunityListByCompanyId(Guid CustomerId)
        {
            string rawQuery = @"  select  * into #temptable from (select  c.*,
                                CASE WHEN c.Type = '-1' or c.Type is null  THEN '-' else lk.DisplayText  end as TypeVal,
                                CASE WHEN c.LeadSource = '-1' or c.LeadSource  IS NULL   THEN '-' else lksource.DisplayText end as LeadSourceVal,
                                CASE WHEN c.Status = '-1' or c.Status  IS NULL   THEN '-' else lkstatus.DisplayText end as StatusVal,
                                CASE WHEN c.Probability = '-1' or c.Probability  IS NULL   THEN '-' else lkp.DisplayText end as ProbabilityVal,
                                CASE WHEN c.DealReason = '-1' or c.DealReason  IS NULL   THEN '-' else lkoppdeal.DisplayText end as DealReasonVal,
                                CASE WHEN c.DeliveryDays = '-1' or c.DeliveryDays  IS NULL   THEN '-' else lkdelivery.DisplayText end as DeliveryDaysVal,
                                CASE WHEN c.CampaignSource = '-1' or c.CampaignSource  IS NULL   THEN '-' else lkcampaign.DisplayText end as CampaignSourceVal,
                                CASE WHEN c.VehicleCondition = '-1' or c.VehicleCondition  IS NULL   THEN '-' else lv.DisplayText end as VehicleConditionVal,
                                cus.FirstName+' '+cus.LastName as CustomerName,emp.FirstName+' '+emp.LastName as AccountOwnerName,empAccess.FirstName+' '+empAccess.LastName as AccessGivenToVal  FROM Opportunity c
                                left join employee emp on emp.UserId = c.AccountOwner
                                left join employee empAccess on empAccess.UserId = c.AccessGivenTo
                                left join customer cus on cus.CustomerId = c.CustomerId
                                left join [Lookup] lk on lk.DataKey = 'OpportunityType' and lk.DataValue = c.Type
                                left join [Lookup] lv on lv.DataKey = 'VehicleCondition' and lv.DataValue = c.VehicleCondition
                                left join [Lookup] lksource on lksource.DataKey = 'LeadSource' and lksource.DataValue = c.LeadSource
                                left join [Lookup] lkstatus on lkstatus.DataKey = 'OpportunityStatus' and lkstatus.DataValue = c.Status
                                left join [Lookup] lkp on lkp.DataKey = 'OpportunityProbability' and lkp.DataValue = c.Probability
                                left join [Lookup] lkoppdeal on lkoppdeal.DataKey = 'OpportunityDealReason' and lkoppdeal.DataValue = c.DealReason

                                left join [Lookup] lkdelivery on lkdelivery.DataKey = 'OpportunityDeliveryDays' and lkdelivery.DataValue = c.DeliveryDays
                                left join [Lookup] lkcampaign on lkcampaign.DataKey = 'OpportunityCampaignSource' and lkcampaign.DataValue = c.CampaignSource
                            
                                where c.CustomerId = '{0}'  ) a
                                select * from #temptable t
                                group by t.Id, t.OpportunityId,t.CustomerId,t.OpportunityName,t.Type,t.LeadSource,t.Revenue,t.ProjectedGP,t.Points,t.Points,t.TotalProjectedGP,t.CloseDate,t.Status,t.IsForecast,t.Competitors,t.CampaignSource,t.AccountOwner,t.CreatedBy,t.CreatedDate,t.LastUpdatedDate,t.Probability,t.DealReason,t.DeliveryDays,t.TypeVal, t.CampaignSourceVal,t.DealReasonVal,t.DeliveryDaysVal,
                                t.LeadSourceVal,t.ProbabilityVal,t.StatusVal,t.CustomerName,t.AccountOwnerName,t.Market,t.Capacity,t.Model,t.Make,t.Used,t.Note,t.YearModel,t.AccessGivenTo,t.AccessGivenToVal,t.VehicleCondition,t.VehicleConditionVal,t.CloseDateSetDate
                                drop table #temptable";
            try
            {
                rawQuery = string.Format(rawQuery, CustomerId);
                using (SqlCommand cmd = GetSQLCommand(rawQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    DataTable dt = dsResult.Tables[0];
                    return dt;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetOpportunityDetailById(int Id)
        {
            string rawQuery = @"  select  * into #temptable from (select  c.*,
                                CASE WHEN c.Type = '-1' or c.Type is null  THEN '-' else lk.DisplayText  end as TypeVal,
                                CASE WHEN c.LeadSource = '-1' or c.LeadSource  IS NULL   THEN '-' else lksource.DisplayText end as LeadSourceVal,
                                CASE WHEN c.Status = '-1' or c.Status  IS NULL   THEN '-' else lkstatus.DisplayText end as StatusVal,
                                CASE WHEN c.Probability = '-1' or c.Probability  IS NULL   THEN '-' else lkp.DisplayText end as ProbabilityVal,
                                CASE WHEN c.DealReason = '-1' or c.DealReason  IS NULL   THEN '-' else lkoppdeal.DisplayText end as DealReasonVal,
                                CASE WHEN c.DeliveryDays = '-1' or c.DeliveryDays  IS NULL   THEN '-' else lkdelivery.DisplayText end as DeliveryDaysVal,
                                CASE WHEN c.CampaignSource = '-1' or c.CampaignSource  IS NULL   THEN '-' else lkcampaign.DisplayText end as CampaignSourceVal,
                                CASE WHEN c.VehicleCondition = '-1' or c.VehicleCondition is null  THEN '-' else lv.DisplayText  end as VehicleConditionVal,
                                {1} as CustomerName, cus.Id as IdCustomer,emp.FirstName+' '+emp.LastName as AccountOwnerName,empAccess.FirstName+' '+empAccess.LastName as AccessGivenToVal  FROM Opportunity c
                                left join employee emp on emp.UserId = c.AccountOwner
                                left join employee empAccess on empAccess.UserId = c.AccessGivenTo
                                left join customer cus on cus.CustomerId = c.CustomerId
                                left join [Lookup] lk on lk.DataKey = 'OpportunityType' and lk.DataValue = c.Type
                                left join [Lookup] lksource on lksource.DataKey = 'LeadSource' and lksource.DataValue = c.LeadSource
                                left join [Lookup] lkstatus on lkstatus.DataKey = 'OpportunityStatus' and lkstatus.DataValue = c.Status
                                left join [Lookup] lkp on lkp.DataKey = 'OpportunityProbability' and lkp.DataValue = c.Probability
                                left join [Lookup] lkoppdeal on lkoppdeal.DataKey = 'OpportunityDealReason' and lkoppdeal.DataValue = c.DealReason
                                left join [Lookup] lv on lv.DataKey = 'VehicleCondition' and lv.DataValue = c.VehicleCondition
                                left join [Lookup] lkdelivery on lkdelivery.DataKey = 'OpportunityDeliveryDays' and lkdelivery.DataValue = c.DeliveryDays
                                left join [Lookup] lkcampaign on lkcampaign.DataKey = 'OpportunityCampaignSource' and lkcampaign.DataValue = c.CampaignSource
                            
                                where c.Id = '{0}'  ) a
                                select * from #temptable t
                                group by t.Id, t.OpportunityId,t.CustomerId,t.OpportunityName,t.Type,t.LeadSource,t.Revenue,t.ProjectedGP,t.Points,t.Points,t.TotalProjectedGP,t.CloseDate,t.Status,t.IsForecast,t.Competitors,t.CampaignSource,t.AccountOwner,t.CreatedBy,t.CreatedDate,t.LastUpdatedDate,t.Probability,t.DealReason,t.DeliveryDays,t.TypeVal, t.CampaignSourceVal,t.DealReasonVal,t.DeliveryDaysVal,
                                t.LeadSourceVal,t.ProbabilityVal,t.StatusVal,t.CustomerName, t.IdCustomer,t.AccountOwnerName,t.Market,t.Capacity,t.Model,t.Make,t.Used,t.Note,t.YearModel,t.AccessGivenTo,t.AccessGivenToVal,t.VehicleCondition,t.VehicleConditionVal,t.CloseDateSetDate
                                drop table #temptable";
            #region Naming Condition
            string NamingSql = "''";
            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            if (gs != null)
            {
                NamingSql = gs.Value;
            }
            #endregion
            try
            {
                rawQuery = string.Format(rawQuery, Id, NamingSql);
                using (SqlCommand cmd = GetSQLCommand(rawQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    DataTable dt = dsResult.Tables[0];
                    return dt;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetOpportunityListByIds(string IdList)
        {
            string rawQuery = @"  select  * into #temptable from (select  c.*,
                                CASE WHEN c.Type = '-1' or c.Type is null  THEN '-' else lk.DisplayText  end as TypeVal,
                                CASE WHEN c.LeadSource = '-1' or c.LeadSource  IS NULL   THEN '-' else lksource.DisplayText end as LeadSourceVal,
                                CASE WHEN c.Status = '-1' or c.Status  IS NULL   THEN '-' else lkstatus.DisplayText end as StatusVal,
                                CASE WHEN c.Probability = '-1' or c.Probability  IS NULL   THEN '-' else lkp.DisplayText end as ProbabilityVal,
                                CASE WHEN c.DealReason = '-1' or c.DealReason  IS NULL   THEN '-' else lkoppdeal.DisplayText end as DealReasonVal,
                                CASE WHEN c.DeliveryDays = '-1' or c.DeliveryDays  IS NULL   THEN '-' else lkdelivery.DisplayText end as DeliveryDaysVal,
                                CASE WHEN c.CampaignSource = '-1' or c.CampaignSource  IS NULL   THEN '-' else lkcampaign.DisplayText end as CampaignSourceVal,
                                cus.FirstName+' '+cus.LastName as CustomerName,emp.FirstName+' '+emp.LastName as AccountOwnerName  FROM Opportunity c
                                left join employee emp on emp.UserId = c.AccountOwner
                                left join customer cus on cus.CustomerId = c.CustomerId
                                left join [Lookup] lk on lk.DataKey = 'OpportunityType' and lk.DataValue = c.Type
                                left join [Lookup] lksource on lksource.DataKey = 'LeadSource' and lksource.DataValue = c.LeadSource
                                left join [Lookup] lkstatus on lkstatus.DataKey = 'OpportunityStatus' and lkstatus.DataValue = c.Status
                                left join [Lookup] lkp on lkp.DataKey = 'OpportunityProbability' and lkp.DataValue = c.Probability
                                left join [Lookup] lkoppdeal on lkoppdeal.DataKey = 'OpportunityDealReason' and lkoppdeal.DataValue = c.DealReason

                                left join [Lookup] lkdelivery on lkdelivery.DataKey = 'OpportunityDeliveryDays' and lkdelivery.DataValue = c.DeliveryDays
                                left join [Lookup] lkcampaign on lkcampaign.DataKey = 'OpportunityCampaignSource' and lkcampaign.DataValue = c.CampaignSource
                            
                                where c.Id in ({0})  ) a
                                select * from #temptable t
                                group by t.Id, t.OpportunityId,t.CustomerId,t.OpportunityName,t.Type,t.LeadSource,t.Revenue,t.ProjectedGP,t.Points,t.Points,t.TotalProjectedGP,t.CloseDate,t.Status,t.IsForecast,t.Competitors,t.CampaignSource,t.AccountOwner,t.CreatedBy,t.CreatedDate,t.LastUpdatedDate,t.Probability,t.DealReason,t.DeliveryDays,t.TypeVal, t.CampaignSourceVal,t.DealReasonVal,t.DeliveryDaysVal,
                                t.LeadSourceVal,t.ProbabilityVal,t.StatusVal,t.CustomerName,t.AccountOwnerName,t.Market,t.Capacity,t.Model,t.Make,t.Used,t.Note,t.YearModel,,t.AccessGivenTo,t.AccessGivenToVal
                                drop table #temptable";
            try
            {
                rawQuery = string.Format(rawQuery, IdList);
                using (SqlCommand cmd = GetSQLCommand(rawQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    DataTable dt = dsResult.Tables[0];
                    return dt;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetAllOpportunityList(string query)
        {
            string sqlQuery = @"SELECT *
                                FROM
                                  ( SELECT opp.OpportunityId, opp.OpportunityName
                                    FROM Opportunity opp
									WHERE (opp.OpportunityName like '%{0}%')
                                  ) tmp";
            try
            {
                sqlQuery = string.Format(sqlQuery, query);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllOpportunityForAPI()
        {
            string sqlQuery = @"select op.CustomerId as accountGuid,
                                op.[Status] as [status],
                                op.OpportunityName as title,
                                op.OpportunityName as [description],
                                op.Id as id,
                                cus.Id as accountId
                                 from Opportunity op
                                 left join Customer cus 
                                 on cus.CustomerId = op.CustomerId";
            try
            {
                //sqlQuery = string.Format(sqlQuery, query);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllOpportunityForExport(string customerid)
        {
            string sqlQuery = @"SELECT c.OpportunityName,c.ProjectedGP,c.Points,c.TotalProjectedGP,c.CloseDate,Competitors,
                                
                                CASE WHEN c.Type = '-1' or c.Type is null  THEN '-' else lk.DisplayText  end as Type,
                                CASE WHEN c.LeadSource = '-1' or c.LeadSource  IS NULL   THEN '-' else lksource.DisplayText end as LeadSource,
                                CASE WHEN c.Status = '-1' or c.Status  IS NULL   THEN '-' else lkstatus.DisplayText end as Status,
                                CASE WHEN c.Probability = '-1' or c.Probability  IS NULL   THEN '-' else lkp.DisplayText end as Probability,
                                CASE WHEN c.DealReason = '-1' or c.DealReason  IS NULL   THEN '-' else lkoppdeal.DisplayText end as DealReason,
                                CASE WHEN c.DeliveryDays = '-1' or c.DeliveryDays  IS NULL   THEN '-' else lkdelivery.DisplayText end as DeliveryDays,
                                CASE WHEN c.CampaignSource = '-1' or c.CampaignSource  IS NULL   THEN '-' else lkcampaign.DisplayText end as CampaignSource,
                                cus.FirstName+' '+cus.LastName as CustomerName,emp.FirstName+' '+emp.LastName as AccountOwnerName  FROM Opportunity c
                                left join employee emp on emp.UserId = c.AccountOwner
                                left join customer cus on cus.CustomerId = c.CustomerId
                                left join [Lookup] lk on lk.DataKey = 'OpportunityType' and lk.DataValue = c.Type
                                left join [Lookup] lksource on lksource.DataKey = 'LeadSource' and lksource.DataValue = c.LeadSource
                                left join [Lookup] lkstatus on lkstatus.DataKey = 'OpportunityStatus' and lkstatus.DataValue = c.Status
                                left join [Lookup] lkp on lkp.DataKey = 'OpportunityProbability' and lkp.DataValue = c.Probability
                                left join [Lookup] lkoppdeal on lkoppdeal.DataKey = 'OpportunityDealReason' and lkoppdeal.DataValue = c.DealReason

                                left join [Lookup] lkdelivery on lkdelivery.DataKey = 'OpportunityDeliveryDays' and lkdelivery.DataValue = c.DeliveryDays
                                left join [Lookup] lkcampaign on lkcampaign.DataKey = 'OpportunityCampaignSource' and lkcampaign.DataValue = c.CampaignSource
                                {0}";

            string subquery = "";
            if (!string.IsNullOrWhiteSpace(customerid) && customerid != new Guid().ToString())
            {
                subquery = string.Format("where c.CustomerId = '{0}'", customerid);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, subquery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllOpportunityDatabaseForExport()
        {
            string sqlQuery = @"SELECT * from Opportunity";


            try
            {

                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<Opportunity> GetAllOpportunityByOpportunityIdList(string OpportunityIdList)
        {
            string sqlQuery = @"SELECT *
                                FROM Opportunity
                                 where OpportunityId in ({0})";
            try
            {
                sqlQuery = string.Format(sqlQuery, OpportunityIdList);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    return GetList(cmd, -1);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet GetAllOpportunitiesReport(int pageno, int pagesize, DateTime? start, DateTime? end, string OppType, string year, string deliveryday, string month, string accOwner, string soldBy, string statusType)
        {
            string sqlQuery = @"";
            string subquery = "";
            string opptypequery = "";
            string DataQuery = "";
            string subDateQuery = "";
            string ClusterQuery = "";
            string deliveryquery = "";
            string NamingSql = "''";
            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            if (gs != null)
            {
                NamingSql = gs.Value;
            }
            if (start.HasValue && end.HasValue)
            {
                if (!string.IsNullOrWhiteSpace(OppType) && OppType == "SoftBacklog")
                {
                    subquery = string.Format("where opp.CreatedDate between '{0}' and '{1}'", start.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), end.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    opptypequery = string.Format("and opp.IsForecast = 0 and opp.CloseDate is not NULL");
                    DataQuery = string.Format("select Top(@pagesize) * from #OpportunityFilter where Id not in (Select TOP(@pagestart)  Id from #Opportunity #opp order by #opp.Id desc) order by Id desc");
                    ClusterQuery = string.Format("select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, DeliveryDayDiff as DeliveryDayDiff, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter group by DeliveryDayDiff order by convert(int,iif(DeliveryDayDiff='> 120','121',DeliveryDayDiff)) asc");
                    if (!string.IsNullOrWhiteSpace(deliveryday))
                    {
                        deliveryquery = string.Format("and opp.DeliveryDays in ({0})", deliveryday);
                    }
                    if (!string.IsNullOrWhiteSpace(accOwner) && accOwner!="'undefined'")
                    {
                        deliveryquery += string.Format("and emp.UserId in ({0}) ", accOwner);
                    }
                    if (!string.IsNullOrWhiteSpace(soldBy))
                    {
                        deliveryquery += string.Format("and emp.UserId ='{0}'", soldBy);
                    }
                    if (!string.IsNullOrWhiteSpace(statusType))
                    {
                        if (statusType == "SoftBackLog")
                        {
                            deliveryquery += " and statusopp.DataValue != 'Closed Won' and statusopp.DataValue != 'ClosedLost' and statusopp.DataValue != 'Dead' and CAST(opp.CloseDate as date) > CAST(getdate() as date)";
                        }
                        else if (statusType == "ClosedLost")
                        {
                            deliveryquery += " and (statusopp.DataValue = 'ClosedLost' Or statusopp.DataValue = 'Dead')";
                        }
                        else if (statusType == "ClosedWon")
                        {
                            deliveryquery += " and statusopp.DataValue = 'Closed Won'";
                        }
                        else if (statusType == "Aged")
                        {
                            deliveryquery += " and statusopp.DataValue != 'Closed Won' and statusopp.DataValue != 'ClosedLost' and statusopp.DataValue != 'Dead' and CAST(opp.CloseDate as date) < CAST(getdate() as date)";
                        }
                    }
                }
                else
                {
                    subquery = string.Format("where opp.CreatedDate between '{0}' and '{1}'", start.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), end.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    DataQuery = string.Format("select Top(@pagesize) * from #OpportunityFilter where Id not in (Select TOP(@pagestart)  Id from #Opportunity #opp order by #opp.Id desc) order by Id desc");
                    if (!string.IsNullOrWhiteSpace(accOwner) && accOwner != "'undefined'")
                    {
                        deliveryquery += string.Format("and emp.UserId in ({0}) ", accOwner);
                    }
                    if (!string.IsNullOrWhiteSpace(soldBy))
                    {
                        deliveryquery += string.Format("and emp.UserId ='{0}'", soldBy);
                    }
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(OppType) && OppType == "SoftBacklog")
                {
                    opptypequery = string.Format("where opp.IsForecast = 0 and opp.CloseDate is not NULL");
                    DataQuery = string.Format("select Top(@pagesize) * from #OpportunityFilter where Id not in (Select TOP(@pagestart)  Id from #Opportunity #opp order by #opp.Id desc) order by Id desc");
                    ClusterQuery = string.Format("select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, DeliveryDayDiff as DeliveryDayDiff, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter group by DeliveryDayDiff order by convert(int,iif(DeliveryDayDiff='> 120','121',DeliveryDayDiff)) asc");
                    if (!string.IsNullOrWhiteSpace(deliveryday))
                    {
                        deliveryquery = string.Format("and opp.DeliveryDays in ({0})", deliveryday);
                    }
                    if (!string.IsNullOrWhiteSpace(accOwner) && accOwner != "'undefined'")
                    {
                        deliveryquery += string.Format("and emp.UserId in ({0}) ", accOwner);
                    }
                    if (!string.IsNullOrWhiteSpace(soldBy))
                    {
                        deliveryquery += string.Format("and emp.UserId ='{0}'", soldBy);
                    }
                    if (!string.IsNullOrWhiteSpace(statusType))
                    {
                        if (statusType == "SoftBackLog")
                        {
                            deliveryquery += " and statusopp.DataValue != 'Closed Won' and statusopp.DataValue != 'ClosedLost' and statusopp.DataValue != 'Dead' and CAST(opp.CloseDate as date) > CAST(getdate() as date)";
                        }
                        else if (statusType == "ClosedLost")
                        {
                            deliveryquery += " and (statusopp.DataValue = 'ClosedLost' Or statusopp.DataValue = 'Dead')";
                        }
                        else if (statusType == "ClosedWon")
                        {
                            deliveryquery += " and statusopp.DataValue = 'Closed Won'";
                        }
                        else if (statusType == "Aged")
                        {
                            deliveryquery += " and statusopp.DataValue != 'Closed Won' and statusopp.DataValue != 'ClosedLost' and statusopp.DataValue != 'Dead' and CAST(opp.CloseDate as date) < CAST(getdate() as date)";
                        }
                    }
                }
                else
                {
                    DataQuery = string.Format("select Top(@pagesize) * from #OpportunityFilter where Id not in (Select TOP(@pagestart)  Id from #Opportunity #opp order by #opp.Id desc) order by Id desc");
                    if (!string.IsNullOrWhiteSpace(accOwner) && accOwner != "'undefined'")
                    {
                        deliveryquery += string.Format("where emp.UserId in ({0}) ", accOwner);
                        if (!string.IsNullOrWhiteSpace(soldBy))
                        {
                            deliveryquery += string.Format("and emp.UserId ='{0}'", soldBy);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(soldBy))
                        {
                            deliveryquery += string.Format("where emp.UserId ='{0}'", soldBy);
                        }
                    }
                }
            }
            sqlQuery = @"declare @pagestart int
                        declare @pageend int
                        set @pagestart=(@pageno-1)* @pagesize 
                        set @pageend = @pagesize

                        select Distinct opp.Id,
                        Case When DATEDIFF(day, opp.CloseDateSetDate, opp.CloseDate) < 31 Then '30'
						When DATEDIFF(day, opp.CloseDateSetDate, opp.CloseDate) > 30 and DATEDIFF(day, opp.CloseDateSetDate, opp.CloseDate) < 61 Then '60'
						When DATEDIFF(day, opp.CloseDateSetDate, opp.CloseDate) > 60 and DATEDIFF(day, opp.CloseDateSetDate, opp.CloseDate) < 91 Then '90'
						When DATEDIFF(day, opp.CloseDateSetDate, opp.CloseDate) > 90 and DATEDIFF(day, opp.CloseDateSetDate, opp.CloseDate) < 121 Then '120'
						When DATEDIFF(day, opp.CloseDateSetDate, opp.CloseDate) > 120 Then '> 120'
						ELSE '30'
						END as DeliveryDayDiff,
                        opp.OpportunityName, ISNULL(TRY_CONVERT(numeric(38,12),opp.ProjectedGP), 0) as ProjectedGP, ISNULL(TRY_CONVERT(numeric(38,12),opp.Points), 0) as Points, ISNULL(TRY_CONVERT(numeric(38,12),opp.TotalProjectedGP), 0) as TotalProjectedGP,
						opp.CloseDate, typeopp.DisplayText as TypeVal, lksource.DisplayText as SourceLead, opp.[Status], opp.Probability,
						reason.DisplayText as ReasonDeal, delivery.DisplayText as DeliverydayVal, camp.DisplayText as CampaignSourceVal,
						{6} as CustomerName, emp.FirstName + ' ' + emp.LastName as EmpName,
                        statusopp.DisplayText as StatusVal, lkprob.DisplayText as ProbabilityVal, opp.Revenue, cus.Street as CustomerStreet,
                        cus.City as CustomerCity, cus.[State] as CustomerState, cus.ZipCode as CustomerZip, cus.EmailAddress as CustomerEmail
						,opp.CreatedDate, cus.Id as CusIdInt, cus.DBA as CustomerDBA, cus.BusinessName as CustomerBusinessName
                        into #Opportunity
						from Opportunity opp
						left join Lookup lksource on lksource.DataValue = iif(opp.LeadSource != '-1', opp.LeadSource, '') and lksource.DataKey = 'LeadSource'
						left join Lookup reason on reason.DataValue = iif(opp.DealReason != '-1', opp.DealReason, '') and reason.DataKey = 'OpportunityDealReason'
						left join Lookup delivery on delivery.DataValue = iif(opp.DeliveryDays != '-1', opp.DeliveryDays, '') and delivery.DataKey = 'OpportunityDeliveryDays'
						left join Lookup camp on camp.DataValue = iif(opp.CampaignSource != '-1', opp.CampaignSource, '') and camp.DataKey = 'OpportunityCampaignSource'
						left join Customer cus on cus.CustomerId = opp.CustomerId
						left join Employee emp on emp.UserId = opp.AccountOwner
                        left join Lookup statusopp on statusopp.DataValue = iif(opp.[Status] != '-1', opp.[Status], '') and statusopp.DataKey = 'OpportunityStatus'
						left join Lookup typeopp on typeopp.DataValue = iif(opp.[Type] != '-1', opp.[Type], '') and typeopp.DataKey = 'OpportunityType'
						left join Lookup lkprob on lkprob.DataValue = iif(opp.Probability != '-1', opp.Probability, '') and lkprob.DataKey = 'OpportunityProbability'
                        {0}
                        {1}
                        {5}
                        (select * into #OpportunityFilter from #Opportunity)

                        {2}

                        (select COUNT(Id) as TotalCount from #OpportunityFilter {3})

                        {4}

                        drop table #OpportunityFilter
                        drop table #Opportunity";
            sqlQuery = string.Format(sqlQuery, subquery, opptypequery, DataQuery, subDateQuery, ClusterQuery, deliveryquery, NamingSql);

            try
            {

                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", pageno));
                    AddParameter(cmd, pInt32("pagesize", pagesize));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetAllOpportunitiesReportHard(int pageno, int pagesize, string OppType, string year, string deliveryday, string month, string accOwner, string soldBy)
        {
            string sqlQuery = @"";
            string subquery = "";
            string opptypequery = "";
            string DataQuery = "";
            string subDateQuery = "";
            string ClusterQuery = "";
            string deliveryquery = "";

            //if (start.HasValue && end.HasValue)
            //{
            //    //if (!string.IsNullOrWhiteSpace(OppType) && OppType == "SoftBacklog")
            //    //{
            //    //    subquery = string.Format("where opp.CreatedDate between '{0}' and '{1}'", start.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), end.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            //    //    opptypequery = string.Format("and opp.IsForecast = 0 and opp.DeliveryDays != '-1'");
            //    //    DataQuery = string.Format("select Top(@pagesize) * from #OpportunityFilter where Id not in (Select TOP(@pagestart)  Id from #Opportunity #opp order by #opp.Id desc) order by Id desc");
            //    //    ClusterQuery = string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, DeliverydayVal as DeliveryDayVals, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter group by DeliverydayVal)");
            //    //    if (!string.IsNullOrWhiteSpace(deliveryday))
            //    //    {
            //    //        deliveryquery = string.Format("and opp.DeliveryDays in ({0})", deliveryday);
            //    //    }
            //    //}
            //    if (!string.IsNullOrWhiteSpace(OppType) && OppType == "HardBacklog")
            //    {
            //        if (!string.IsNullOrWhiteSpace(month))
            //        {
            //            if (month == "01")
            //            {
            //                DataQuery = string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
            //                ClusterQuery = string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
            //                subDateQuery = string.Format("where CreatedDate between '{0}' and '{1}'", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
            //            }
            //            else if (month == "02")
            //            {
            //                DataQuery = string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-28 23:59:59.999");
            //                ClusterQuery = string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-28 23:59:59.999");
            //                subDateQuery = string.Format("where CreatedDate between '{0}' and '{1}'", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-28 23:59:59.999");
            //            }
            //            else if (month == "03")
            //            {
            //                DataQuery = string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
            //                ClusterQuery = string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
            //                subDateQuery = string.Format("where CreatedDate between '{0}' and '{1}'", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
            //            }
            //            else if (month == "04")
            //            {
            //                DataQuery = string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-30 23:59:59.999");
            //                ClusterQuery = string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-30 23:59:59.999");
            //                subDateQuery = string.Format("where CreatedDate between '{0}' and '{1}'", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-30 23:59:59.999");
            //            }
            //            else if (month == "05")
            //            {
            //                DataQuery = string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
            //                ClusterQuery = string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
            //                subDateQuery = string.Format("where CreatedDate between '{0}' and '{1}'", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
            //            }
            //            else if (month == "06")
            //            {
            //                DataQuery = string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-30 23:59:59.999");
            //                ClusterQuery = string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-30 23:59:59.999");
            //                subDateQuery = string.Format("where CreatedDate between '{0}' and '{1}'", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-30 23:59:59.999");
            //            }
            //            else if (month == "07")
            //            {
            //                DataQuery = string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
            //                ClusterQuery = string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
            //                subDateQuery = string.Format("where CreatedDate between '{0}' and '{1}'", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
            //            }
            //            else if (month == "08")
            //            {
            //                DataQuery = string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
            //                ClusterQuery = string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
            //                subDateQuery = string.Format("where CreatedDate between '{0}' and '{1}'", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
            //            }
            //            else if (month == "09")
            //            {
            //                DataQuery = string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-30 23:59:59.999");
            //                ClusterQuery = string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-30 23:59:59.999");
            //                subDateQuery = string.Format("where CreatedDate between '{0}' and '{1}'", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-30 23:59:59.999");
            //            }
            //            else if (month == "10")
            //            {
            //                DataQuery = string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
            //                ClusterQuery = string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
            //                subDateQuery = string.Format("where CreatedDate between '{0}' and '{1}'", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
            //            }
            //            else if (month == "11")
            //            {
            //                DataQuery = string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-30 23:59:59.999");
            //                ClusterQuery = string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-30 23:59:59.999");
            //                subDateQuery = string.Format("where CreatedDate between '{0}' and '{1}'", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-30 23:59:59.999");
            //            }
            //            else if (month == "12")
            //            {
            //                DataQuery = string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
            //                ClusterQuery = string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
            //                subDateQuery = string.Format("where CreatedDate between '{0}' and '{1}'", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
            //            }
            //        }
            //        else
            //        {
            //            if (!string.IsNullOrWhiteSpace(year))
            //            {
            //                for (int i = 1; i <= 12; i++)
            //                {
            //                    if (i == 1)
            //                    {
            //                        DataQuery += string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-01-01 00:00:00.000", year + "-01-31 23:59:59.999");
            //                        ClusterQuery += string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-01-01 00:00:00.000", year + "-01-31 23:59:59.999");
            //                    }
            //                    if (i == 2)
            //                    {
            //                        DataQuery += string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-02-01 00:00:00.000", year + "-02-28 23:59:59.999");
            //                        ClusterQuery += string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-02-01 00:00:00.000", year + "-02-28 23:59:59.999");
            //                    }
            //                    if (i == 3)
            //                    {
            //                        DataQuery += string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-03-01 00:00:00.000", year + "-03-31 23:59:59.999");
            //                        ClusterQuery += string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-03-01 00:00:00.000", year + "-03-31 23:59:59.999");
            //                    }
            //                    if (i == 4)
            //                    {
            //                        DataQuery += string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-04-01 00:00:00.000", year + "-04-30 23:59:59.999");
            //                        ClusterQuery += string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-04-01 00:00:00.000", year + "-04-30 23:59:59.999");
            //                    }
            //                    if (i == 5)
            //                    {
            //                        DataQuery += string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-05-01 00:00:00.000", year + "-05-31 23:59:59.999");
            //                        ClusterQuery += string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-05-01 00:00:00.000", year + "-05-31 23:59:59.999");
            //                    }
            //                    if (i == 6)
            //                    {
            //                        DataQuery += string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-06-01 00:00:00.000", year + "-06-30 23:59:59.999");
            //                        ClusterQuery += string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-06-01 00:00:00.000", year + "-06-30 23:59:59.999");
            //                    }
            //                    if (i == 7)
            //                    {
            //                        DataQuery += string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-07-01 00:00:00.000", year + "-07-31 23:59:59.999");
            //                        ClusterQuery += string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-07-01 00:00:00.000", year + "-07-31 23:59:59.999");
            //                    }
            //                    if (i == 8)
            //                    {
            //                        DataQuery += string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-08-01 00:00:00.000", year + "-08-31 23:59:59.999");
            //                        ClusterQuery += string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-08-01 00:00:00.000", year + "-08-31 23:59:59.999");
            //                    }
            //                    if (i == 9)
            //                    {
            //                        DataQuery += string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-09-01 00:00:00.000", year + "-09-30 23:59:59.999");
            //                        ClusterQuery += string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-09-01 00:00:00.000", year + "-09-30 23:59:59.999");
            //                    }
            //                    if (i == 10)
            //                    {
            //                        DataQuery += string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-10-01 00:00:00.000", year + "-10-31 23:59:59.999");
            //                        ClusterQuery += string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-10-01 00:00:00.000", year + "-10-31 23:59:59.999");
            //                    }
            //                    if (i == 11)
            //                    {
            //                        DataQuery += string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-11-01 00:00:00.000", year + "-11-30 23:59:59.999");
            //                        ClusterQuery += string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-11-01 00:00:00.000", year + "-11-30 23:59:59.999");
            //                    }
            //                    if (i == 12)
            //                    {
            //                        DataQuery += string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-12-01 00:00:00.000", year + "-12-31 23:59:59.999");
            //                        ClusterQuery += string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-12-01 00:00:00.000", year + "-12-31 23:59:59.999");
            //                    }
            //                }
            //                subDateQuery = string.Format("where CreatedDate between '{0}' and '{1}'", year + "-01-01 00:00:00.000", year + "-12-31 23:59:59.999");
            //            }
            //        }
            //        subquery = string.Format("where opp.CreatedDate between '{0}' and '{1}'", start.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), end.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            //        if(!string.IsNullOrWhiteSpace(accOwner))
            //        {
            //            opptypequery = string.Format("and opp.IsForecast = 1 and emp.UserId='{0}'", accOwner);
            //        }
            //        else
            //        {
            //            opptypequery = string.Format("and opp.IsForecast = 1");
            //        }           
            //    }
            //    //else
            //    //{
            //    //    subquery = string.Format("where opp.CreatedDate between '{0}' and '{1}'", start.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), end.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            //    //    DataQuery = string.Format("select Top(@pagesize) * from #OpportunityFilter where Id not in (Select TOP(@pagestart)  Id from #Opportunity #opp order by #opp.Id desc) order by Id desc");
            //    //}
            //}
            //else
            //{
            //if (!string.IsNullOrWhiteSpace(OppType) && OppType == "SoftBacklog")
            //{
            //    opptypequery = string.Format("where opp.IsForecast = 0 and opp.DeliveryDays != '-1'");
            //    DataQuery = string.Format("select Top(@pagesize) * from #OpportunityFilter where Id not in (Select TOP(@pagestart)  Id from #Opportunity #opp order by #opp.Id desc) order by Id desc");
            //    ClusterQuery = string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, DeliverydayVal as DeliveryDayVals, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter group by DeliverydayVal)");
            //    if (!string.IsNullOrWhiteSpace(deliveryday))
            //    {
            //        deliveryquery = string.Format("and opp.DeliveryDays in ({0})", deliveryday);
            //    }
            //}
            if (!string.IsNullOrWhiteSpace(OppType) && OppType == "HardBacklog")
            {
                if (!string.IsNullOrWhiteSpace(month))
                {
                    if (month == "01")
                    {
                        DataQuery = string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
                        ClusterQuery = string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
                        subDateQuery = string.Format("where CreatedDate between '{0}' and '{1}'", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
                    }
                    else if (month == "02")
                    {
                        DataQuery = string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-28 23:59:59.999");
                        ClusterQuery = string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-28 23:59:59.999");
                        subDateQuery = string.Format("where CreatedDate between '{0}' and '{1}'", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-28 23:59:59.999");
                    }
                    else if (month == "03")
                    {
                        DataQuery = string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
                        ClusterQuery = string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
                        subDateQuery = string.Format("where CreatedDate between '{0}' and '{1}'", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
                    }
                    else if (month == "04")
                    {
                        DataQuery = string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-30 23:59:59.999");
                        ClusterQuery = string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-30 23:59:59.999");
                        subDateQuery = string.Format("where CreatedDate between '{0}' and '{1}'", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-30 23:59:59.999");
                    }
                    else if (month == "05")
                    {
                        DataQuery = string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
                        ClusterQuery = string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
                        subDateQuery = string.Format("where CreatedDate between '{0}' and '{1}'", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
                    }
                    else if (month == "06")
                    {
                        DataQuery = string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-30 23:59:59.999");
                        ClusterQuery = string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-30 23:59:59.999");
                        subDateQuery = string.Format("where CreatedDate between '{0}' and '{1}'", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-30 23:59:59.999");
                    }
                    else if (month == "07")
                    {
                        DataQuery = string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
                        ClusterQuery = string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
                        subDateQuery = string.Format("where CreatedDate between '{0}' and '{1}'", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
                    }
                    else if (month == "08")
                    {
                        DataQuery = string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
                        ClusterQuery = string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
                        subDateQuery = string.Format("where CreatedDate between '{0}' and '{1}'", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
                    }
                    else if (month == "09")
                    {
                        DataQuery = string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-30 23:59:59.999");
                        ClusterQuery = string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-30 23:59:59.999");
                        subDateQuery = string.Format("where CreatedDate between '{0}' and '{1}'", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-30 23:59:59.999");
                    }
                    else if (month == "10")
                    {
                        DataQuery = string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
                        ClusterQuery = string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
                        subDateQuery = string.Format("where CreatedDate between '{0}' and '{1}'", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
                    }
                    else if (month == "11")
                    {
                        DataQuery = string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-30 23:59:59.999");
                        ClusterQuery = string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-30 23:59:59.999");
                        subDateQuery = string.Format("where CreatedDate between '{0}' and '{1}'", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-30 23:59:59.999");
                    }
                    else if (month == "12")
                    {
                        DataQuery = string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
                        ClusterQuery = string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
                        subDateQuery = string.Format("where CreatedDate between '{0}' and '{1}'", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(year))
                    {
                        for (int i = 1; i <= 12; i++)
                        {
                            if (i == 1)
                            {
                                DataQuery += string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-01-01 00:00:00.000", year + "-01-31 23:59:59.999");
                                ClusterQuery += string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-01-01 00:00:00.000", year + "-01-31 23:59:59.999");
                            }
                            if (i == 2)
                            {
                                DataQuery += string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-02-01 00:00:00.000", year + "-02-28 23:59:59.999");
                                ClusterQuery += string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-02-01 00:00:00.000", year + "-02-28 23:59:59.999");
                            }
                            if (i == 3)
                            {
                                DataQuery += string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-03-01 00:00:00.000", year + "-03-31 23:59:59.999");
                                ClusterQuery += string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-03-01 00:00:00.000", year + "-03-31 23:59:59.999");
                            }
                            if (i == 4)
                            {
                                DataQuery += string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-04-01 00:00:00.000", year + "-04-30 23:59:59.999");
                                ClusterQuery += string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-04-01 00:00:00.000", year + "-04-30 23:59:59.999");
                            }
                            if (i == 5)
                            {
                                DataQuery += string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-05-01 00:00:00.000", year + "-05-31 23:59:59.999");
                                ClusterQuery += string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-05-01 00:00:00.000", year + "-05-31 23:59:59.999");
                            }
                            if (i == 6)
                            {
                                DataQuery += string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-06-01 00:00:00.000", year + "-06-30 23:59:59.999");
                                ClusterQuery += string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-06-01 00:00:00.000", year + "-06-30 23:59:59.999");
                            }
                            if (i == 7)
                            {
                                DataQuery += string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-07-01 00:00:00.000", year + "-07-31 23:59:59.999");
                                ClusterQuery += string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-07-01 00:00:00.000", year + "-07-31 23:59:59.999");
                            }
                            if (i == 8)
                            {
                                DataQuery += string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-08-01 00:00:00.000", year + "-08-31 23:59:59.999");
                                ClusterQuery += string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-08-01 00:00:00.000", year + "-08-31 23:59:59.999");
                            }
                            if (i == 9)
                            {
                                DataQuery += string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-09-01 00:00:00.000", year + "-09-30 23:59:59.999");
                                ClusterQuery += string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-09-01 00:00:00.000", year + "-09-30 23:59:59.999");
                            }
                            if (i == 10)
                            {
                                DataQuery += string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-10-01 00:00:00.000", year + "-10-31 23:59:59.999");
                                ClusterQuery += string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-10-01 00:00:00.000", year + "-10-31 23:59:59.999");
                            }
                            if (i == 11)
                            {
                                DataQuery += string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-11-01 00:00:00.000", year + "-11-30 23:59:59.999");
                                ClusterQuery += string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-11-01 00:00:00.000", year + "-11-30 23:59:59.999");
                            }
                            if (i == 12)
                            {
                                DataQuery += string.Format("select * from #OpportunityFilter where CreatedDate between '{0}' and '{1}' order by Id desc ", year + "-12-01 00:00:00.000", year + "-12-31 23:59:59.999");
                                ClusterQuery += string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter where CreatedDate between '{0}' and '{1}')", year + "-12-01 00:00:00.000", year + "-12-31 23:59:59.999");
                            }
                        }
                        subDateQuery = string.Format("where CreatedDate between '{0}' and '{1}'", year + "-01-01 00:00:00.000", year + "-12-31 23:59:59.999");
                    }
                }
                if (!string.IsNullOrWhiteSpace(accOwner))
                {
                    opptypequery = string.Format("where opp.IsForecast = 1 and emp.UserId in ({0}) ", accOwner);

                }
                else
                {
                    opptypequery = string.Format("where opp.IsForecast = 1 ");
                }
                if (!string.IsNullOrWhiteSpace(soldBy))
                {
                    opptypequery += string.Format("and emp.UserId='{0}'", soldBy);
                }
            }
            //else
            //{
            //    DataQuery = string.Format("select Top(@pagesize) * from #OpportunityFilter where Id not in (Select TOP(@pagestart)  Id from #Opportunity #opp order by #opp.Id desc) order by Id desc");
            //}
            //}
            sqlQuery = @"declare @pagestart int
                        declare @pageend int
                        set @pagestart=(@pageno-1)* @pagesize 
                        set @pageend = @pagesize

                        select Distinct opp.Id, opp.OpportunityName, ISNULL(TRY_CONVERT(numeric(38,12),opp.ProjectedGP), 0) as ProjectedGP, ISNULL(TRY_CONVERT(numeric(38,12),opp.Points), 0) as Points, ISNULL(TRY_CONVERT(numeric(38,12),opp.TotalProjectedGP), 0) as TotalProjectedGP,
						opp.CloseDate, typeopp.DisplayText as TypeVal, lksource.DisplayText as SourceLead, opp.[Status], opp.Probability,
						reason.DisplayText as ReasonDeal, delivery.DisplayText as DeliverydayVal, camp.DisplayText as CampaignSourceVal,
						cus.FirstName + ' ' + cus.LastName as CustomerName, emp.FirstName + ' ' + emp.LastName as EmpName,
                        statusopp.DisplayText as StatusVal, lkprob.DisplayText as ProbabilityVal, opp.Revenue, cus.Street as CustomerStreet,
                        cus.City as CustomerCity, cus.[State] as CustomerState, cus.ZipCode as CustomerZip, cus.EmailAddress as CustomerEmail
						,opp.CreatedDate, cus.Id as CusIdInt, cus.DBA as CustomerDBA, cus.BusinessName as CustomerBusinessName
                        into #Opportunity
						from Opportunity opp
						left join Lookup lksource on lksource.DataValue = iif(opp.LeadSource != '-1', opp.LeadSource, '') and lksource.DataKey = 'LeadSource'
						left join Lookup reason on reason.DataValue = iif(opp.DealReason != '-1', opp.DealReason, '') and reason.DataKey = 'OpportunityDealReason'
						left join Lookup delivery on delivery.DataValue = iif(opp.DeliveryDays != '-1', opp.DeliveryDays, '') and delivery.DataKey = 'OpportunityDeliveryDays'
						left join Lookup camp on camp.DataValue = iif(opp.CampaignSource != '-1', opp.CampaignSource, '') and camp.DataKey = 'OpportunityCampaignSource'
						left join Customer cus on cus.CustomerId = opp.CustomerId
						left join Employee emp on emp.UserId = opp.AccountOwner
                        left join Lookup statusopp on statusopp.DataValue = iif(opp.[Status] != '-1', opp.[Status], '') and statusopp.DataKey = 'OpportunityStatus'
						left join Lookup typeopp on typeopp.DataValue = iif(opp.[Type] != '-1', opp.[Type], '') and typeopp.DataKey = 'OpportunityType'
						left join Lookup lkprob on lkprob.DataValue = iif(opp.Probability != '-1', opp.Probability, '') and lkprob.DataKey = 'OpportunityProbability'
                        {0}
                        {1}
                        {5}
                        (select * into #OpportunityFilter from #Opportunity)

                        {2}

                        (select COUNT(Id) as TotalCount from #OpportunityFilter {3})

                        {4}

                        drop table #OpportunityFilter
                        drop table #Opportunity";
            sqlQuery = string.Format(sqlQuery, subquery, opptypequery, DataQuery, subDateQuery, ClusterQuery, deliveryquery);

            try
            {

                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", pageno));
                    AddParameter(cmd, pInt32("pagesize", pagesize));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetAllOpportunitiesForHadrBacklogReportExport(string OppType, string year, string delivery, string month, string accOwner, string soldBy)
        {
            string sqlQuery = @"";
            string opptypequery = "";
            string datequery = "";
            string deliveryquery = "";

            if (!string.IsNullOrWhiteSpace(year) && !string.IsNullOrWhiteSpace(month))
            {
                if (month == "01")
                {
                    datequery = string.Format("and opp.CreatedDate between '{0}' and '{1}'", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
                }
                else if (month == "02")
                {
                    datequery = string.Format("and opp.CreatedDate between '{0}' and '{1}'", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-28 23:59:59.999");
                }
                else if (month == "03")
                {
                    datequery = string.Format("and opp.CreatedDate between '{0}' and '{1}'", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
                }
                else if (month == "04")
                {
                    datequery = string.Format("and opp.CreatedDate between '{0}' and '{1}'", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-30 23:59:59.999");
                }
                else if (month == "05")
                {
                    datequery = string.Format("and opp.CreatedDate between '{0}' and '{1}'", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
                }
                else if (month == "06")
                {
                    datequery = string.Format("and opp.CreatedDate between '{0}' and '{1}'", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-30 23:59:59.999");
                }
                else if (month == "07")
                {
                    datequery = string.Format("and opp.CreatedDate between '{0}' and '{1}'", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
                }
                else if (month == "08")
                {
                    datequery = string.Format("and opp.CreatedDate between '{0}' and '{1}'", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
                }
                else if (month == "09")
                {
                    datequery = string.Format("and opp.CreatedDate between '{0}' and '{1}'", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-30 23:59:59.999");
                }
                else if (month == "10")
                {
                    datequery = string.Format("and opp.CreatedDate between '{0}' and '{1}'", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
                }
                else if (month == "11")
                {
                    datequery = string.Format("and opp.CreatedDate between '{0}' and '{1}'", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-30 23:59:59.999");
                }
                else if (month == "12")
                {
                    datequery = string.Format("and opp.CreatedDate between '{0}' and '{1}'", year + "-" + month + "-01 00:00:00.000", year + "-" + month + "-31 23:59:59.999");
                }
            }
            else if (!string.IsNullOrWhiteSpace(year))
            {
                datequery = string.Format("and opp.CreatedDate between '{0}' and '{1}'", year + "-01-01 00:00:00.000", year + "-12-31 23:59:59.999");
            }
            if (!string.IsNullOrWhiteSpace(accOwner))
            {
                datequery += string.Format("and emp.UserId in ({0}) ", accOwner);
            }
            if (!string.IsNullOrWhiteSpace(soldBy))
            {
                datequery += string.Format("and emp.UserId ='{0}'", soldBy);
            }
            //      if (start.HasValue && end.HasValue)
            //      {
            //          //if (!string.IsNullOrWhiteSpace(OppType) && OppType == "SoftBacklog")
            //          //{
            //          //    opptypequery = string.Format("and opp.IsForecast = 0 and opp.DeliveryDays != '-1'");
            //          //    if (!string.IsNullOrWhiteSpace(delivery))
            //          //    {
            //          //        deliveryquery = string.Format("and opp.DeliveryDays in ({0})", delivery);
            //          //    }
            //          //}
            //          if (!string.IsNullOrWhiteSpace(OppType) && OppType == "HardBacklog")
            //          {
            //              opptypequery = string.Format(" and opp.IsForecast = 1");
            //          }
            //          sqlQuery = @"
            //                  select Distinct opp.Id, opp.OpportunityName, opp.ProjectedGP, opp.Points, opp.TotalProjectedGP,
            //opp.CloseDate, typeopp.DisplayText as TypeVal, lksource.DisplayText as SourceLead, opp.[Status], opp.Probability,
            //reason.DisplayText as ReasonDeal, delivery.DisplayText as DeliverydayVal, camp.DisplayText as CampaignSourceVal,
            //cus.FirstName + ' ' + cus.LastName as CustomerName, emp.FirstName + ' ' + emp.LastName as EmpName,
            //                  statusopp.DisplayText as StatusVal, lkprob.DisplayText as ProbabilityVal, format(opp.CreatedDate,'MM') as CreatedMonth
            //from Opportunity opp
            //left join Lookup lksource on lksource.DataValue = iif(opp.LeadSource != '-1', opp.LeadSource, '') and lksource.DataKey = 'LeadSource'
            //left join Lookup reason on reason.DataValue = iif(opp.DealReason != '-1', opp.DealReason, '') and reason.DataKey = 'OpportunityDealReason'
            //left join Lookup delivery on delivery.DataValue = iif(opp.DeliveryDays != '-1', opp.DeliveryDays, '') and delivery.DataKey = 'OpportunityDeliveryDays'
            //left join Lookup camp on camp.DataValue = iif(opp.CampaignSource != '-1', opp.CampaignSource, '') and camp.DataKey = 'OpportunityCampaignSource'
            //left join Customer cus on cus.CustomerId = opp.CustomerId
            //left join Employee emp on emp.UserId = opp.AccountOwner
            //                  left join Lookup statusopp on statusopp.DataValue = iif(opp.[Status] != '-1', opp.[Status], '') and statusopp.DataKey = 'OpportunityStatus'
            //left join Lookup typeopp on typeopp.DataValue = iif(opp.[Type] != '-1', opp.[Type], '') and typeopp.DataKey = 'OpportunityType'
            //left join Lookup lkprob on lkprob.DataValue = iif(opp.Probability != '-1', opp.Probability, '') and lkprob.DataKey = 'OpportunityProbability'
            //                  where opp.CreatedDate between '{0}' and '{1}'
            //                  {2}
            //                  {3}
            //                  order by CreatedMonth asc, Id desc";
            //          sqlQuery = string.Format(sqlQuery, start.Value.ToString("yyyy-MM-dd 00:00:00.000"), end.Value.ToString("yyyy-MM-dd 23:59:59.000"), opptypequery, deliveryquery);
            //      }
            //      else
            //      {
            //if (!string.IsNullOrWhiteSpace(OppType) && OppType == "SoftBacklog")
            //{
            //    opptypequery = string.Format("where opp.IsForecast = 0 and opp.DeliveryDays != '-1'");
            //    if (!string.IsNullOrWhiteSpace(delivery))
            //    {
            //        deliveryquery = string.Format("and opp.DeliveryDays in ({0})", delivery);
            //    }
            //}
            if (!string.IsNullOrWhiteSpace(OppType) && OppType == "HardBacklog")
            {
                opptypequery = string.Format(" where opp.IsForecast = 1");
            }
            sqlQuery = @"
                        select Distinct opp.Id, opp.OpportunityName, opp.ProjectedGP, opp.Points, opp.TotalProjectedGP,
						format(opp.CloseDate,'MM/dd/yyyy') as CloseDate, typeopp.DisplayText as TypeVal, lksource.DisplayText as SourceLead, opp.[Status], opp.Probability,
						reason.DisplayText as ReasonDeal, delivery.DisplayText as DeliverydayVal, camp.DisplayText as CampaignSourceVal,
						cus.FirstName + ' ' + cus.LastName as CustomerName, emp.FirstName + ' ' + emp.LastName as EmpName,
                        statusopp.DisplayText as StatusVal, lkprob.DisplayText as ProbabilityVal, format(opp.CreatedDate,'MM') as CreatedMonth, format(opp.CreatedDate,'MMMM') as Month
						from Opportunity opp
						left join Lookup lksource on lksource.DataValue = iif(opp.LeadSource != '-1', opp.LeadSource, '') and lksource.DataKey = 'LeadSource'
						left join Lookup reason on reason.DataValue = iif(opp.DealReason != '-1', opp.DealReason, '') and reason.DataKey = 'OpportunityDealReason'
						left join Lookup delivery on delivery.DataValue = iif(opp.DeliveryDays != '-1', opp.DeliveryDays, '') and delivery.DataKey = 'OpportunityDeliveryDays'
						left join Lookup camp on camp.DataValue = iif(opp.CampaignSource != '-1', opp.CampaignSource, '') and camp.DataKey = 'OpportunityCampaignSource'
						left join Customer cus on cus.CustomerId = opp.CustomerId
						left join Employee emp on emp.UserId = opp.AccountOwner
                        left join Lookup statusopp on statusopp.DataValue = iif(opp.[Status] != '-1', opp.[Status], '') and statusopp.DataKey = 'OpportunityStatus'
						left join Lookup typeopp on typeopp.DataValue = iif(opp.[Type] != '-1', opp.[Type], '') and typeopp.DataKey = 'OpportunityType'
						left join Lookup lkprob on lkprob.DataValue = iif(opp.Probability != '-1', opp.Probability, '') and lkprob.DataKey = 'OpportunityProbability'
                        {0}
                        {1}
                        {2}
                        order by CreatedMonth asc, Id desc";
            sqlQuery = string.Format(sqlQuery, opptypequery, datequery, deliveryquery);
            //}
            try
            {

                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetAllOpportunitiesReportExport(DateTime? start, DateTime? end, string OppType, string year, string delivery, string month, string accOwner, string soldBy,string statusType)
        {
            string sqlQuery = @"";
            string subquery = "";
            string opptypequery = "";
            string DataQuery = "";
            string deliveryquery = "";
            string NamingSql = "''";
            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            if (gs != null)
            {
                NamingSql = gs.Value;
            }

            if (start.HasValue && end.HasValue)
            {
                if (!string.IsNullOrWhiteSpace(OppType) && OppType == "SoftBacklog")
                {
                    subquery = string.Format("where opp.CreatedDate between '{0}' and '{1}'", start.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), end.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    opptypequery = string.Format("and opp.IsForecast = 0 and opp.CloseDate is not NULL");
                    DataQuery = string.Format("select * from #OpportunityFilter order by convert(int,iif(DeliverydayVal='> 120','121',DeliverydayVal)) asc, Id desc");
                    //ClusterQuery = string.Format("(select ISNULL(SUM(TRY_CONVERT(numeric(38,12),Revenue)), 0) as TotalRevenueVal, DeliverydayVal as DeliveryDayVals, ISNULL(SUM(TRY_CONVERT(numeric(38,12),ProjectedGP)), 0) as TotalProjectedGPVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),Points)), 0) as TotalPointsVal, ISNULL(SUM(TRY_CONVERT(numeric(38,12),TotalProjectedGP)), 0) as ProjectedGPTotalVal from #OpportunityFilter group by DeliverydayVal)");
                    if (!string.IsNullOrWhiteSpace(delivery))
                    {
                        deliveryquery = string.Format("and opp.DeliveryDays in ({0})", delivery);
                    }
                    if (!string.IsNullOrWhiteSpace(accOwner))
                    {
                        deliveryquery += string.Format("and emp.UserId in ({0}) ", accOwner);
                    }
                    if (!string.IsNullOrWhiteSpace(soldBy))
                    {
                        deliveryquery += string.Format("and emp.UserId ='{0}'", soldBy);
                    }
                    if (!string.IsNullOrWhiteSpace(statusType))
                    {
                        if (statusType == "SoftBackLog")
                        {
                            deliveryquery += " and statusopp.DataValue != 'Closed Won' and statusopp.DataValue != 'ClosedLost' and statusopp.DataValue != 'Dead' and CAST(opp.CloseDate as date) > CAST(getdate() as date)";
                        }
                        else if (statusType == "ClosedLost")
                        {
                            deliveryquery += " and (statusopp.DataValue = 'ClosedLost' Or statusopp.DataValue = 'Dead')";
                        }
                        else if (statusType == "ClosedWon")
                        {
                            deliveryquery += " and statusopp.DataValue = 'Closed Won'";
                        }
                        else if (statusType == "Aged")
                        {
                            deliveryquery += " and statusopp.DataValue != 'Closed Won' and statusopp.DataValue != 'ClosedLost' and statusopp.DataValue != 'Dead' and CAST(opp.CloseDate as date) < CAST(getdate() as date)";
                        }
                    }
                }
                else
                {
                    subquery = string.Format("where opp.CreatedDate between '{0}' and '{1}'", start.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), end.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    DataQuery = string.Format("select * from #OpportunityFilter order by Id desc");
                    if (!string.IsNullOrWhiteSpace(accOwner))
                    {
                        deliveryquery += string.Format("and emp.UserId in ({0}) ", accOwner);
                    }

                    if (!string.IsNullOrWhiteSpace(soldBy))
                    {
                        deliveryquery += string.Format("and emp.UserId ='{0}'", soldBy);
                    }
                }

            }
            else
            {
                if (!string.IsNullOrWhiteSpace(OppType) && OppType == "SoftBacklog")
                {
                    opptypequery = string.Format("where opp.IsForecast = 0 and opp.CloseDate is not NULL");
                    DataQuery = string.Format("select * from #OpportunityFilter order by convert(int,iif(DeliverydayVal='> 120','121',DeliverydayVal)) asc, Id desc");
                    if (!string.IsNullOrWhiteSpace(delivery))
                    {
                        deliveryquery = string.Format("and opp.DeliveryDays in ({0})", delivery);
                    }
                    if (!string.IsNullOrWhiteSpace(accOwner))
                    {
                        deliveryquery += string.Format("and emp.UserId in ({0}) ", accOwner);
                    }
                    if (!string.IsNullOrWhiteSpace(soldBy))
                    {
                        deliveryquery += string.Format("and emp.UserId ='{0}'", soldBy);
                    }
                    if (!string.IsNullOrWhiteSpace(statusType))
                    {
                        if (statusType == "SoftBackLog")
                        {
                            deliveryquery += " and statusopp.DataValue != 'Closed Won' and statusopp.DataValue != 'ClosedLost' and statusopp.DataValue != 'Dead' and CAST(opp.CloseDate as date) > CAST(getdate() as date)";
                        }
                        else if (statusType == "ClosedLost")
                        {
                            deliveryquery += " and (statusopp.DataValue = 'ClosedLost' Or statusopp.DataValue = 'Dead')";
                        }
                        else if (statusType == "ClosedWon")
                        {
                            deliveryquery += " and statusopp.DataValue = 'Closed Won'";
                        }
                        else if (statusType == "Aged")
                        {
                            deliveryquery += " and statusopp.DataValue != 'Closed Won' and statusopp.DataValue != 'ClosedLost' and statusopp.DataValue != 'Dead' and CAST(opp.CloseDate as date) < CAST(getdate() as date)";
                        }
                    }
                }
                else
                {
                    DataQuery = string.Format("select * from #OpportunityFilter order by Id desc");
                    if (!string.IsNullOrWhiteSpace(accOwner) && accOwner != "'undefined'")
                    {
                        deliveryquery += string.Format("where emp.UserId in ({0}) ", accOwner);
                        if (!string.IsNullOrWhiteSpace(soldBy))
                        {
                            deliveryquery += string.Format("and emp.UserId ='{0}'", soldBy);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(soldBy))
                        {
                            deliveryquery += string.Format("where emp.UserId ='{0}'", soldBy);
                        }
                    }
                }

            }
            sqlQuery = @"
                        select Distinct opp.Id, opp.OpportunityName, opp.ProjectedGP, opp.Points, opp.TotalProjectedGP,
                        format(opp.CloseDate,'MM/dd/yyyy') as CloseDate, typeopp.DisplayText as TypeVal, lksource.DisplayText as SourceLead, opp.[Status], opp.Probability,
                        reason.DisplayText as ReasonDeal, delivery.DisplayText as DeliverydayVal, camp.DisplayText as CampaignSourceVal,
                        {4} as CustomerName, emp.FirstName + ' ' + emp.LastName as EmpName,
                        statusopp.DisplayText as StatusVal, lkprob.DisplayText as ProbabilityVal
                        into #Opportunity
						from Opportunity opp
						left join Lookup lksource on lksource.DataValue = iif(opp.LeadSource != '-1', opp.LeadSource, '') and lksource.DataKey = 'LeadSource'
						left join Lookup reason on reason.DataValue = iif(opp.DealReason != '-1', opp.DealReason, '') and reason.DataKey = 'OpportunityDealReason'
						left join Lookup delivery on delivery.DataValue = iif(opp.DeliveryDays != '-1', opp.DeliveryDays, '') and delivery.DataKey = 'OpportunityDeliveryDays'
						left join Lookup camp on camp.DataValue = iif(opp.CampaignSource != '-1', opp.CampaignSource, '') and camp.DataKey = 'OpportunityCampaignSource'
						left join Customer cus on cus.CustomerId = opp.CustomerId
						left join Employee emp on emp.UserId = opp.AccountOwner
                        left join Lookup statusopp on statusopp.DataValue = iif(opp.[Status] != '-1', opp.[Status], '') and statusopp.DataKey = 'OpportunityStatus'
						left join Lookup typeopp on typeopp.DataValue = iif(opp.[Type] != '-1', opp.[Type], '') and typeopp.DataKey = 'OpportunityType'
						left join Lookup lkprob on lkprob.DataValue = iif(opp.Probability != '-1', opp.Probability, '') and lkprob.DataKey = 'OpportunityProbability'
                        {0}
                        {1}
                        {3}
                        (select * into #OpportunityFilter from #Opportunity)

                        {2}

                        drop table #OpportunityFilter
                        drop table #Opportunity";
            sqlQuery = string.Format(sqlQuery, subquery, opptypequery, DataQuery, deliveryquery, NamingSql);

            try
            {

                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
