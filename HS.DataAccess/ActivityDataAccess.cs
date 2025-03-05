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
	public partial class ActivityDataAccess
	{
        public ActivityDataAccess() { }
        public ActivityDataAccess(string ConStr) : base(ConStr) { }
        public ActivityModel GetActivities(ActivityFilter filter)
        {
            string searchTextQuery = "";
            string searchTextQuery1 = "";
            string subquery = "";
            string filterQuery = "";
            string filterQuery1 = "";
            string subqueryCustomer = "";
            string ActivitytIds = "";
            string AssignToIdFilter = "";
            string CountFilter = "";
            if (filter.AssignToId != new Guid() && filter.AssignToId != null)
            {
                AssignToIdFilter = string.Format(" AssignedTo = '{0}' and", filter.AssignToId);
                CountFilter = string.Format(" where AssignedTo = '{0}'", filter.AssignToId);
            }
            if (!string.IsNullOrEmpty(filter.ActivityType) && filter.ActivityType != "-1")
            {
                filterQuery += string.Format(" c.ActivityType = '{0}' and", filter.ActivityType);
             
            }
            if (!string.IsNullOrEmpty(filter.AssignTo) && filter.AssignTo != "00000000-0000-0000-0000-000000000000")
            {
                filterQuery += string.Format(" c.AssignedTo = '{0}' and", filter.AssignTo);
              
            }
            if (!string.IsNullOrEmpty(filter.ActivityStatus) && filter.ActivityStatus != "-1")
            {
                filterQuery += string.Format(" c.Status = '{0}' and", filter.ActivityStatus);
              
            }
            if (filter.DueDateFrom != new DateTime() && filter.DueDateTo != new DateTime())
            {
                filterQuery += string.Format(" c.DueDate between '{0}' and '{1}' and", filter.DueDateFrom.SetZeroHour().ClientToUTCTime(), filter.DueDateTo.SetMaxHour().ClientToUTCTime());

            }
            if (filter.CreatedDateFrom != new DateTime() && filter.CreatedDateTo != new DateTime())
            {
                filterQuery += string.Format(" c.CreatedDate between '{0}' and '{1}' and", filter.CreatedDateFrom.SetZeroHour().ClientToUTCTime(), filter.CreatedDateTo.SetMaxHour().SetMaxHour());

            }
            //if(filterQuery1 != "")
            //{
            //    filterQuery1 = filterQuery.Remove(filterQuery.Length - 4, 4);
            //}

            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                searchTextQuery = @" (c.ActivityType like @SearchText 
                                        or c.Status like @SearchText 
                                        or c.Description like @SearchText 
                                        or c.Note like @SearchText 
                                        or c.Id like @SearchText 
                                        or empAssignTo.FirstName + ' ' + empAssignTo.LastName like @SearchText) AND";
            }

            if(filter.CustomerId != new Guid())
            {
                subqueryCustomer = string.Format("c.AssociatedWith = '{0}' and ", filter.CustomerId);
            }
         
            List<Activity> ActivityList = new List<Activity>();
            ActivityCount TotalContact = new ActivityCount();
            string rawQuery = @"  
                                declare @pagesize int
                                declare @pageno int 
                                set @pagesize = " + filter.UnitPerPage + @"
                                set @pageno = " + filter.PageNumber + @"
                                declare @pagestart int
                                set @pagestart=(@pageno-1)* @pagesize  
                                 select  * into #temptable from (select  c.*,
                                CASE WHEN c.ActivityType = '-1' or c.ActivityType  IS NULL   THEN '-' else lkType.DisplayText end as ActivityTypeVal,
                                CASE WHEN c.AssociatedType = '-1' or c.AssociatedType  IS NULL   THEN '-' else lkasstype.DisplayText end as AssociatedTypeVal,
                                emp.FirstName+' '+emp.LastName as CreatedByVal,empAssignTo.FirstName+' '+empAssignTo.LastName as AssignedToVal,
                                {6} as DisplayName, 
                                cus.Id as CustomerId,
                                oppAssociatWith.OpportunityName as AssociatedOpportunityVal,
                                oppAssociatWith.Id as OpportunityId,
                                conAssociatWith.FirstName+' '+conAssociatWith.LastName as AssociatedContactVal,
                                conAssociatWith.Id as ContactId,
                                lkDepartment.DisplayText as DepartmentVal
                                FROM Activity c
                                left join employee emp on emp.UserId = c.CreatedBy
                                left join employee empAssignTo on empAssignTo.UserId = c.AssignedTo
                                left join Customer cus on cus.CustomerId = c.AssociatedWith
                                left join Opportunity oppAssociatWith on oppAssociatWith.OpportunityId = c.AssociatedWith
                                left join Contact conAssociatWith on conAssociatWith.ContactId = c.AssociatedWith
                                left join [Lookup] lkType on lkType.DataKey = 'ActivityType' and lkType.DataValue = c.ActivityType
                                left join [Lookup] lkasstype on lkasstype.DataKey = 'AssociatedType' and lkasstype.DataValue = c.AssociatedType
                                left join [Lookup] lkDepartment on lkDepartment.DataKey = 'ActivityDepartment' and lkDepartment.DataValue = c.Department
                                where {2}{1}{4}{3} c.Id > 0) a
                               
								select * into #ftemp from #temptable 

                                select TOP (@pagesize) *  from #ftemp f
                                where  f.Id NOT IN(Select TOP (@pagestart) Id from #ftemp )
                                {0}
                                select Count(Id) As TotalCount from #ftemp  

								drop table #temptable
								drop table #ftemp";



            #region Order
            if (!string.IsNullOrWhiteSpace(filter.Order))
            {
                if (filter.Order == "ascending/id")
                {
                    subquery = "order by Id asc";

                }
                else if (filter.Order == "descending/id")
                {
                    subquery = "order by Id desc";

                }
                else if (filter.Order == "ascending/activitytype")
                {
                    subquery = "order by ActivityType asc";

                }
                else if (filter.Order == "descending/activitytype")
                {
                    subquery = "order by ActivityType desc";

                }
                else if (filter.Order == "ascending/description")
                {
                    subquery = "order by Description asc";

                }
                else if (filter.Order == "descending/description")
                {
                    subquery = "order by Description desc";

                }
                else if (filter.Order == "ascending/assignedTo")
                {
                    subquery = "order by AssignedTo asc";

                }
                else if (filter.Order == "descending/assignedTo")
                {
                    subquery = "order by AssignedTo desc";

                }
                else if (filter.Order == "ascending/mobile")
                {
                    subquery = "order by mobile asc";

                }
                else if (filter.Order == "descending/duedate")
                {
                    subquery = "order by DueDate desc";

                }

                else if (filter.Order == "ascending/status")
                {
                    subquery = "order by Status asc";

                }
                else if (filter.Order == "descending/status")
                {
                    subquery = "order by status desc";

                }
                else if (filter.Order == "ascending/associatedtype")
                {
                    subquery = "order by AssociatedType asc";

                }
                else if (filter.Order == "descending/associatedtype")
                {
                    subquery = "order by  AssociatedType desc";

                }

                else if (filter.Order == "ascending/associatedwith")
                {
                    subquery = "order by AssociatedWith asc";

                }
                else if (filter.Order == "descending/associatedwith")
                {
                    subquery = "order by  AssociatedWith desc";

                }

                else if (filter.Order == "ascending/Note")
                {
                    subquery = "order by Note asc";

                }
                else if (filter.Order == "descending/Note")
                {
                    subquery = "order by  Note desc";

                }
             
                else if (filter.Order == "ascending/createddate")
                {
                    subquery = "order by  CreatedDate desc";

                }
                else if (filter.Order == "descending/createddate")
                {
                    subquery = "order by  CreatedDate desc";

                }
                else if (filter.Order == "ascending/department")
                {
                    subquery = "order by  Department desc";

                }
                else if (filter.Order == "descending/department")
                {
                    subquery = "order by  Department desc";

                }
            }
            else
            {
                subquery = "order by Id desc";

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
            rawQuery = string.Format(rawQuery, subquery, searchTextQuery, filterQuery, subqueryCustomer, AssignToIdFilter, CountFilter,NamingSql);
            using (SqlCommand cmd = GetSQLCommand(rawQuery))
            {
                AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", filter.SearchText)));
                DataSet dsResult = GetDataSet(cmd);
                DataTable dt = dsResult.Tables[0];
                DataTable dt1 = dsResult.Tables[1];
                try
                {
                    ActivityList = (from DataRow dr in dt.Rows
                                   select new Activity()
                                   {
                                       Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                       CustomerId = dr["CustomerId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerId"]) : 0,
                                       OpportunityId = dr["OpportunityId"] != DBNull.Value ? Convert.ToInt32(dr["OpportunityId"]) : 0,
                                       ContactId = dr["ContactId"] != DBNull.Value ? Convert.ToInt32(dr["ContactId"]) : 0,
                                       ActivityTypeVal = dr["ActivityTypeVal"].ToString(),
                                       AssociatedTypeVal = dr["AssociatedTypeVal"].ToString(),
                                       AssociatedType = dr["AssociatedType"].ToString(),
                                       Description = dr["Description"].ToString(),
                                       Note = dr["Note"].ToString(),
                                       Status = dr["Status"].ToString(),
                                       DisplayName = dr["DisplayName"].ToString(),
                                       AssignedToVal = dr["AssignedToVal"].ToString(),
                                       CreatedByVal = dr["CreatedByVal"].ToString(),
                                       AssociatedOpportunityVal = dr["AssociatedOpportunityVal"].ToString(),
                                       AssociatedContactVal = dr["AssociatedContactVal"].ToString(),
                                       DueDate = dr["DueDate"] != DBNull.Value ? Convert.ToDateTime(dr["DueDate"]) : new DateTime(),
                                       CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                       Origin = dr["Origin"].ToString(),
                                       DepartmentVal = dr["DepartmentVal"].ToString(),
                                       Department = dr["Department"].ToString(),
                                   }).ToList();


                    TotalContact = (from DataRow dr in dt1.Rows
                                    select new ActivityCount()
                                    {
                                        TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                    }).FirstOrDefault();

                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            ActivityModel activity = new ActivityModel();
            activity.ActivityList = ActivityList;
            activity.TotalCount = TotalContact;
            return activity;
        }

        public List<Activity> GetFilteredActivities(ActivityFilter filter)
        {
            string searchTextQuery = "";
   
        
            string filterQuery = "";
     
          
            string AssignToIdFilter = "";
            string CountFilter = "";
            if (filter.AssignToId != new Guid() && filter.AssignToId != null)
            {
                AssignToIdFilter = string.Format(" and AssignedTo = '{0}'", filter.AssignToId);
                CountFilter = string.Format(" and AssignedTo = '{0}'", filter.AssignToId);
            }
            if (!string.IsNullOrEmpty(filter.ActivityType) && filter.ActivityType != "-1")
            {
                filterQuery += string.Format(" and c.ActivityType = '{0}'", filter.ActivityType);

            }
            if (!string.IsNullOrEmpty(filter.AssignTo) && filter.AssignTo != "00000000-0000-0000-0000-000000000000")
            {
                filterQuery += string.Format(" and c.AssignedTo = '{0}'", filter.AssignTo);

            }
            if (!string.IsNullOrEmpty(filter.ActivityStatus) && filter.ActivityStatus != "-1")
            {
                filterQuery += string.Format("and c.Status = '{0}'", filter.ActivityStatus);

            }
            if (filter.DueDateFrom != new DateTime() && filter.DueDateTo != new DateTime())
            {
                filterQuery += string.Format(" and c.DueDate between '{0}' and '{1}'", filter.DueDateFrom, filter.DueDateTo);

            }
            if (filter.CreatedDateFrom != new DateTime() && filter.CreatedDateTo != new DateTime())
            {
                filterQuery += string.Format(" and c.CreatedDate between '{0}' and '{1}'", filter.CreatedDateFrom.SetZeroHour().ClientToUTCTime(), filter.CreatedDateTo.SetMaxHour().SetMaxHour());

            }
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                searchTextQuery = "and (c.ActivityType like @SearchText or c.Status like @SearchText or c.Description like @SearchText or c.Note like @SearchText or c.Id like @SearchText or empAssignTo.FirstName like @SearchText)";
               
            }
            //if (searchTextQuery == "" && filterQuery != "")
            //{
            //    filterQuery = " " + filterQuery;
            //}

            List<Activity> ActivityList = new List<Activity>();
            ActivityCount TotalContact = new ActivityCount();
            string rawQuery = @" select  c.*,
                                CASE WHEN c.ActivityType = '-1' or c.ActivityType  IS NULL   THEN '-' else lkType.DisplayText end as ActivityTypeVal,
                                CASE WHEN c.AssociatedType = '-1' or c.AssociatedType  IS NULL   THEN '-' else lkasstype.DisplayText end as AssociatedTypeVal,
                                emp.FirstName+' '+emp.LastName as CreatedByVal,empAssignTo.FirstName+' '+empAssignTo.LastName as AssignedToVal,
                                cusAssociatWith.FirstName+' '+cusAssociatWith.LastName as AssociatedWithVal , 
                                oppAssociatWith.OpportunityName as AssociatedOpportunityVal FROM Activity c
                                left join employee emp on emp.UserId = c.CreatedBy
                                left join employee empAssignTo on empAssignTo.UserId = c.AssignedTo
                                left join Customer cusAssociatWith on cusAssociatWith.CustomerId = c.AssociatedWith
                                left join Opportunity oppAssociatWith on oppAssociatWith.OpportunityId = c.AssociatedWith
                                left join [Lookup] lkType on lkType.DataKey = 'ActivityType' and lkType.DataValue = c.ActivityType
                                left join [Lookup] lkasstype on lkasstype.DataKey = 'AssociatedType' and lkasstype.DataValue = c.AssociatedType
                                Where c.Id is not null
                                {0}{1}
                                Order by c.Id desc
                              ";




            rawQuery = string.Format(rawQuery, searchTextQuery,filterQuery);
            if (filterQuery != "" || searchTextQuery != "")
            {
                rawQuery = rawQuery.Remove(rawQuery.Length - 70);
            }
            using (SqlCommand cmd = GetSQLCommand(rawQuery))
            {
                AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", filter.SearchText)));
                DataSet dsResult = GetDataSet(cmd);
                DataTable dt = dsResult.Tables[0];
      
                try
                {
                    ActivityList = (from DataRow dr in dt.Rows
                                    select new Activity()
                                    {
                                        Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                        ActivityTypeVal = dr["ActivityTypeVal"].ToString(),
                                        AssociatedTypeVal = dr["AssociatedTypeVal"].ToString(),
                                        AssociatedType = dr["AssociatedType"].ToString(),
                                        Description = dr["Description"].ToString(),
                                        Note = dr["Note"].ToString(),
                                        Status = dr["Status"].ToString(),
                                        Origin=dr["Origin"].ToString(),
                                        AssociatedWithVal = dr["AssociatedWithVal"].ToString(),
                                        AssignedToVal = dr["AssignedToVal"].ToString(),
                                        CreatedByVal = dr["CreatedByVal"].ToString(),
                                        AssociatedOpportunityVal = dr["AssociatedOpportunityVal"].ToString(),
                                        DueDate = dr["DueDate"] != DBNull.Value ? Convert.ToDateTime(dr["DueDate"]) : new DateTime(),
                                        CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                    }).ToList();



                }
                catch (Exception ex)
                {
                    return null;
                }
            }
       
            return ActivityList;
        }
        public DataTable GetAllActivityForExport(ActivityFilter filter)
        {
            string sqlQuery = @"DECLARE @SearchText nvarchar(50)
                                SET @SearchText = '{2}'
                                
                                select  c.Id as [Activity Id],
                                CASE WHEN c.ActivityType = '-1' or c.ActivityType  IS NULL   THEN '-' else lkType.DisplayText end as [Activity Type],
                                CASE WHEN c.AssociatedType = '-1' or c.AssociatedType  IS NULL   THEN '-' else lkasstype.DisplayText end as [Associated Type],
							
                                empAssignTo.FirstName+' '+empAssignTo.LastName as [Assigned To],
                                cusAssociatWith.FirstName+' '+cusAssociatWith.LastName as [Associated With] , 
                                oppAssociatWith.OpportunityName as [Associated Opportunity],c.Origin,c.Description,c.DueDate as [Due Date],c.Note,  emp.FirstName+' '+emp.LastName as [Created By] FROM Activity c
                                left join employee emp on emp.UserId = c.CreatedBy
                                left join employee empAssignTo on empAssignTo.UserId = c.AssignedTo
                                left join Customer cusAssociatWith on cusAssociatWith.CustomerId = c.AssociatedWith
                                left join Opportunity oppAssociatWith on oppAssociatWith.OpportunityId = c.AssociatedWith
                                left join [Lookup] lkType on lkType.DataKey = 'ActivityType' and lkType.DataValue = c.ActivityType
                                left join [Lookup] lkasstype on lkasstype.DataKey = 'AssociatedType' and lkasstype.DataValue = c.AssociatedType
                                Where c.Id is not null {0}{1}
                                order by [Activity Id] desc";

            string subquery = "";
            string searchTextQuery = "";
            string searchTextQuery1 = "";
            //string subquery = "";
            string filterQuery = "";
            string filterQuery1 = "";
            string subqueryCustomer = "";
            string ActivitytIds = "";
            string AssignToIdFilter = "";
            string CountFilter = "";
            if (filter.AssignToId != new Guid() && filter.AssignToId != null)
            {
                AssignToIdFilter = string.Format("and AssignedTo = '{0}'", filter.AssignToId);
                //CountFilter = string.Format(" where AssignedTo = '{0}'", filter.AssignToId);
            }
            if (!string.IsNullOrEmpty(filter.ActivityType) && filter.ActivityType != "-1")
            {
                filterQuery += string.Format("and c.ActivityType = '{0}'", filter.ActivityType);

            }
            if (!string.IsNullOrEmpty(filter.AssignTo) && filter.AssignTo != "00000000-0000-0000-0000-000000000000")
            {
                filterQuery += string.Format("and c.AssignedTo = '{0}'", filter.AssignTo);

            }
            if (!string.IsNullOrEmpty(filter.ActivityStatus) && filter.ActivityStatus != "-1")
            {
                filterQuery += string.Format("and c.Status = '{0}'", filter.ActivityStatus);

            }
            if (filter.DueDateFrom != new DateTime() && filter.DueDateTo != new DateTime())
            {
                filterQuery += string.Format("and c.DueDate between '{0}' and '{1}'", filter.DueDateFrom.SetZeroHour().ClientToUTCTime(), filter.DueDateTo.SetMaxHour().ClientToUTCTime());

            }
            if (filter.CreatedDateFrom != new DateTime() && filter.CreatedDateTo != new DateTime())
            {
                filterQuery += string.Format("and c.CreatedDate between '{0}' and '{1}'", filter.CreatedDateFrom.SetZeroHour().ClientToUTCTime(), filter.CreatedDateTo.SetMaxHour().SetMaxHour());

            }
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                searchTextQuery = @" and (c.ActivityType like @SearchText 
                                        or c.Status like @SearchText 
                                        or c.Description like @SearchText 
                                        or c.Note like @SearchText 
                                        or c.Id like @SearchText 
                                        or empAssignTo.FirstName + ' ' + empAssignTo.LastName like @SearchText)";
            }
            //if (!string.IsNullOrWhiteSpace(customerid) && customerid != new Guid().ToString())
            //{
            //    subquery = string.Format("where c.AssociatedWith = '{0}'", customerid);
            //}
            try
            {
                sqlQuery = string.Format(sqlQuery, filterQuery, searchTextQuery, filter.SearchText);
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
        
        public DataTable GetActivityInfoByIds(int Id)
        {
            string rawQuery = @" select  c.*,
                                    CASE WHEN c.ActivityType = '-1' or c.ActivityType  IS NULL   THEN '-' else lkType.DisplayText end as ActivityTypeVal,
                                    CASE WHEN c.AssociatedType = '-1' or c.AssociatedType  IS NULL   THEN '-' else lkasstype.DisplayText end as AssociatedTypeVal,
                                    emp.FirstName+' '+emp.LastName as CreatedByVal,empAssignTo.FirstName+' '+empAssignTo.LastName as AssignedToVal,
                                    cusAssociatWith.FirstName+' '+cusAssociatWith.LastName as AssociatedWithVal , 
                                    lkorigin.DisplayText as OriginVal,
                                    oppAssociatWith.OpportunityName as AssociatedOpportunityVal FROM Activity c
                                    left join employee emp on emp.UserId = c.CreatedBy
                                    left join employee empAssignTo on empAssignTo.UserId = c.AssignedTo
                                    left join Customer cusAssociatWith on cusAssociatWith.CustomerId = c.AssociatedWith
                                    left join Opportunity oppAssociatWith on oppAssociatWith.OpportunityId = c.AssociatedWith
                                    left join [Lookup] lkType on lkType.DataKey = 'ActivityType' and lkType.DataValue = c.ActivityType
                                    left join [Lookup] lkasstype on lkasstype.DataKey = 'AssociatedType' and lkasstype.DataValue = c.AssociatedType
                                    left join [Lookup] lkorigin on lkorigin.DataKey = 'ActivityOrigin' and lkorigin.DataValue = iif(c.Origin != '-1', c.Origin, '')
                                    where c.Id = {0} 
                               ";
            try
            {
                rawQuery = string.Format(rawQuery, Id);
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
        public DataTable GetAllActivityListByIds(string IdList)
        {
            string rawQuery = @" select  c.*,
                                    CASE WHEN c.ActivityType = '-1' or c.ActivityType  IS NULL   THEN '-' else lkType.DisplayText end as ActivityTypeVal,
                                    CASE WHEN c.AssociatedType = '-1' or c.AssociatedType  IS NULL   THEN '-' else lkasstype.DisplayText end as AssociatedTypeVal,
                                    emp.FirstName+' '+emp.LastName as CreatedByVal,empAssignTo.FirstName+' '+empAssignTo.LastName as AssignedToVal,
                                    cusAssociatWith.FirstName+' '+cusAssociatWith.LastName as AssociatedWithVal , 
                                    oppAssociatWith.OpportunityName as AssociatedOpportunityVal FROM Activity c
                                    left join employee emp on emp.UserId = c.CreatedBy
                                    left join employee empAssignTo on empAssignTo.UserId = c.AssignedTo
                                    left join Customer cusAssociatWith on cusAssociatWith.CustomerId = c.AssociatedWith
                                    left join Opportunity oppAssociatWith on oppAssociatWith.OpportunityId = c.AssociatedWith
                                    left join [Lookup] lkType on lkType.DataKey = 'ActivityType' and lkType.DataValue = c.ActivityType
                                    left join [Lookup] lkasstype on lkasstype.DataKey = 'AssociatedType' and lkasstype.DataValue = c.AssociatedType
                                 where c.Id in ({0}) 
                               ";
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

        public DataTable GetAllActivityListByAssociatWith(Guid AssociateWith)
        {
            string rawQuery = @" select  c.*,
                                    CASE WHEN c.ActivityType = '-1' or c.ActivityType  IS NULL   THEN '-' else lkType.DisplayText end as ActivityTypeVal,
                                    CASE WHEN c.AssociatedType = '-1' or c.AssociatedType  IS NULL   THEN '-' else lkasstype.DisplayText end as AssociatedTypeVal,
                                    emp.FirstName+' '+emp.LastName as CreatedByVal,empAssignTo.FirstName+' '+empAssignTo.LastName as AssignedToVal,
                                    cusAssociatWith.FirstName+' '+cusAssociatWith.LastName as AssociatedWithVal , 
                                    oppAssociatWith.OpportunityName as AssociatedOpportunityVal FROM Activity c
                                    left join employee emp on emp.UserId = c.CreatedBy
                                    left join employee empAssignTo on empAssignTo.UserId = c.AssignedTo
                                    left join Customer cusAssociatWith on cusAssociatWith.CustomerId = c.AssociatedWith
                                    left join Opportunity oppAssociatWith on oppAssociatWith.OpportunityId = c.AssociatedWith
                                    left join [Lookup] lkType on lkType.DataKey = 'ActivityType' and lkType.DataValue = c.ActivityType
                                    left join [Lookup] lkasstype on lkasstype.DataKey = 'AssociatedType' and lkasstype.DataValue = c.AssociatedType
                                    where c.AssociatedWith = '{0}'
                               ";
            try
            {
                rawQuery = string.Format(rawQuery, AssociateWith);
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

        public List<Activity> GetExpiringActivityListByDay()
        {
            //2017-08-25 00:00:00.000
            DateTime todaymax = DateTime.Now.UTCCurrentTime().AddDays(1).SetMaxHour();
            DateTime todaymin = DateTime.Now.UTCCurrentTime().SetZeroHour();
            string startdate = todaymax.ToString("yyyy-MM-dd 23:mm:ss.000");
            string enddate = todaymin.ToString("yyyy-MM-dd 00:mm:ss.000");
            string sqlQuery = @"select * from Activity
                                where DueDate between '{1}' and '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, startdate, enddate);
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


        public DataSet GetAllActivitiesReport(int pageno, int pagesize, DateTime? start, DateTime? end, string type, string status, string origin, string soldBy)
        {
            string sqlQuery = @"";
            string subquery = "";
            string filterquery = "";
            string originquery = "";
            string soldByQuery = "";
            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
            }
            
            if(start.HasValue && end.HasValue)
            {
                subquery = string.Format("where act.CreatedDate between '{0}' and '{1}'", start.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), end.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                if (!string.IsNullOrWhiteSpace(soldBy))
                {
                    soldByQuery = string.Format("and emp.UserId='{0}' ", soldBy);
                }
                if (!string.IsNullOrWhiteSpace(type) && type != "-1" && !string.IsNullOrWhiteSpace(status) && status != "-1")
                {
                    filterquery = string.Format("and act.ActivityType = '{0}' and act.[Status] = '{1}'", type, status);
                }
                else if (!string.IsNullOrWhiteSpace(status) && status != "-1")
                {
                    filterquery = string.Format("and act.[Status] = '{0}'", status);
                }
                else if (!string.IsNullOrWhiteSpace(type) && type != "-1")
                {
                    filterquery = string.Format("and act.ActivityType = '{0}'", type);
                }
                if (!string.IsNullOrWhiteSpace(origin) && origin != "-1")
                {
                    originquery = string.Format("and act.Origin = '{0}'", origin);
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(type) && type != "-1" && !string.IsNullOrWhiteSpace(status) && status != "-1" && !string.IsNullOrWhiteSpace(origin) && origin != "-1")
                {
                    filterquery = string.Format("where act.ActivityType = '{0}' and act.[Status] = '{1}' and act.Origin = '{2}'", type, status, origin);
                    if (!string.IsNullOrWhiteSpace(soldBy))
                    {
                        soldByQuery = string.Format("and emp.UserId='{0}' ", soldBy);
                    }
                }
                else if (!string.IsNullOrWhiteSpace(status) && status != "-1" && !string.IsNullOrWhiteSpace(origin) && origin != "-1")
                {
                    filterquery = string.Format("where act.[Status] = '{0}' and act.Origin = '{1}'", status, origin);
                    if (!string.IsNullOrWhiteSpace(soldBy))
                    {
                        soldByQuery = string.Format("and emp.UserId='{0}' ", soldBy);
                    }
                }
                else if (!string.IsNullOrWhiteSpace(type) && type != "-1" && !string.IsNullOrWhiteSpace(origin) && origin != "-1")
                {
                    filterquery = string.Format("where act.ActivityType = '{0}' and act.Origin = '{1}'", type, origin);
                    if (!string.IsNullOrWhiteSpace(soldBy))
                    {
                        soldByQuery = string.Format("and emp.UserId='{0}' ", soldBy);
                    }
                }
                else if (!string.IsNullOrWhiteSpace(status) && status != "-1")
                {
                    filterquery = string.Format("where act.[Status] = '{0}'", status);
                    if (!string.IsNullOrWhiteSpace(soldBy))
                    {
                        soldByQuery = string.Format("and emp.UserId='{0}' ", soldBy);
                    }
                }
                else if (!string.IsNullOrWhiteSpace(type) && type != "-1")
                {
                    filterquery = string.Format("where act.ActivityType = '{0}'", type);
                    if (!string.IsNullOrWhiteSpace(soldBy))
                    {
                        soldByQuery = string.Format("and emp.UserId='{0}' ", soldBy);
                    }
                }
                else if (!string.IsNullOrWhiteSpace(origin) && origin != "-1")
                {
                    originquery = string.Format("where act.Origin = '{0}'", origin);
                    if (!string.IsNullOrWhiteSpace(soldBy))
                    {
                        soldByQuery = string.Format("and emp.UserId='{0}' ", soldBy);
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(soldBy))
                    {
                        soldByQuery = string.Format("where emp.UserId='{0}' ", soldBy);
                    }
                }
            }
            sqlQuery = @"declare @pagestart int
                        declare @pageend int
                        set @pagestart=(@pageno-1)* @pagesize 
                        set @pageend = @pagesize

                        select Distinct act.Id, 
                        lktype.DisplayText as ActivityTypeVal, 
                        lkDepartment.DisplayText as DepartmentVal, 
                        emp.FirstName + ' ' + emp.LastName as EmpName,
                        {4} as AssociatedCustomer, act.[Status], act.[Description], act.DueDate,
                        opp.OpportunityName as AssociatedOpportunity, con.FirstName + ' ' + con.LastName as AssociatedContact,
                        act.Note, createdby.FirstName + ' ' + createdby.LastName as CreatedByVal, act.CreatedDate, act.Origin as OriginVal
                        into #Activity 
                        from Activity act
                        left join Lookup lktype on lktype.DataValue = iif(act.ActivityType != '-1', act.ActivityType, '') and lktype.DataKey = 'ActivityType'
                        left join Employee emp on emp.UserId = act.AssignedTo
                        left join Customer cus on cus.CustomerId = act.AssociatedWith
                        left join Opportunity opp on opp.OpportunityId = act.AssociatedWith
                        left join Contact con on con.ContactId = act.AssociatedWith
                        left join Employee createdby on createdby.UserId = act.CreatedBy
                        left join [Lookup] lkorigin on lkorigin.DataKey = 'ActivityOrigin' and lkorigin.DataValue = iif(act.Origin != '-1', act.Origin, '')
                        left join [Lookup] lkDepartment on lkDepartment.DataKey = 'ActivityDepartment' and lkDepartment.DataValue = act.Department
                        {0}
                        {1}
                        {2}
                        {3}
                        select * into #ActivityFilter from #Activity

                        select Top(@pagesize) * from #ActivityFilter
                        where Id not in (Select TOP (@pagestart)  Id from #Activity #act order by #act.Id desc)
                        order by Id desc

                        select COUNT(*) as TotalCount from #ActivityFilter

                        drop table #ActivityFilter
                        drop table #Activity";
            sqlQuery = string.Format(sqlQuery, subquery, filterquery, originquery, soldByQuery, NameSql);

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

        public DataTable GetAllActivitiesReportExport(DateTime? start, DateTime? end, string type, string status, string origin, string soldBy)
        {
            string sqlQuery = @"";
            string subquery = "";
            string filterquery = "";
            string originquery = "";
            string soldByQuery = "";
            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
            }
            
            if (start.HasValue && end.HasValue)
            {
                subquery = string.Format("where act.CreatedDate between '{0}' and '{1}'", start.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), end.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                if (!string.IsNullOrWhiteSpace(soldBy))
                {
                    soldByQuery = string.Format("and emp.UserId='{0}' ", soldBy);
                }
                if (!string.IsNullOrWhiteSpace(type) && type != "-1" && !string.IsNullOrWhiteSpace(status) && status != "-1")
                {
                    filterquery = string.Format("and act.ActivityType = '{0}' and act.[Status] = '{1}'", type, status);
                }
                else if (!string.IsNullOrWhiteSpace(status) && status != "-1")
                {
                    filterquery = string.Format("and act.[Status] = '{0}'", status);
                }
                else if (!string.IsNullOrWhiteSpace(type) && type != "-1")
                {
                    filterquery = string.Format("and act.ActivityType = '{0}'", type);
                }
                if (!string.IsNullOrWhiteSpace(origin) && origin != "-1")
                {
                    originquery = string.Format("and act.Origin = '{0}'", origin);
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(type) && type != "-1" && !string.IsNullOrWhiteSpace(status) && status != "-1" && !string.IsNullOrWhiteSpace(origin) && origin != "-1")
                {
                    filterquery = string.Format("where act.ActivityType = '{0}' and act.[Status] = '{1}' and act.Origin = '{2}'", type, status, origin);
                    if (!string.IsNullOrWhiteSpace(soldBy))
                    {
                        soldByQuery = string.Format("and emp.UserId='{0}' ", soldBy);
                    }
                }
                else if (!string.IsNullOrWhiteSpace(status) && status != "-1" && !string.IsNullOrWhiteSpace(origin) && origin != "-1")
                {
                    filterquery = string.Format("where act.[Status] = '{0}' and act.Origin = '{1}'", status, origin);
                    if (!string.IsNullOrWhiteSpace(soldBy))
                    {
                        soldByQuery = string.Format("and emp.UserId='{0}' ", soldBy);
                    }
                }
                else if (!string.IsNullOrWhiteSpace(type) && type != "-1" && !string.IsNullOrWhiteSpace(origin) && origin != "-1")
                {
                    filterquery = string.Format("where act.ActivityType = '{0}' and act.Origin = '{1}'", type, origin);
                    if (!string.IsNullOrWhiteSpace(soldBy))
                    {
                        soldByQuery = string.Format("and emp.UserId='{0}' ", soldBy);
                    }
                }
                else if (!string.IsNullOrWhiteSpace(status) && status != "-1")
                {
                    filterquery = string.Format("where act.[Status] = '{0}'", status);
                    if (!string.IsNullOrWhiteSpace(soldBy))
                    {
                        soldByQuery = string.Format("and emp.UserId='{0}' ", soldBy);
                    }
                }
                else if (!string.IsNullOrWhiteSpace(type) && type != "-1")
                {
                    filterquery = string.Format("where act.ActivityType = '{0}'", type);
                    if (!string.IsNullOrWhiteSpace(soldBy))
                    {
                        soldByQuery = string.Format("and emp.UserId='{0}' ", soldBy);
                    }
                }
                else if (!string.IsNullOrWhiteSpace(origin) && origin != "-1")
                {
                    originquery = string.Format("where act.Origin = '{0}'", origin);
                    if (!string.IsNullOrWhiteSpace(soldBy))
                    {
                        soldByQuery = string.Format("and emp.UserId='{0}' ", soldBy);
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(soldBy))
                    {
                        soldByQuery = string.Format("where emp.UserId='{0}' ", soldBy);
                    }
                }
            }
            sqlQuery = @"
                        select Distinct act.Id, lktype.DisplayText as [Activity Type], emp.FirstName + ' ' + emp.LastName as [Assigned To],
                        iif({4} is not null, {4}, iif(opp.OpportunityName is not null, opp.OpportunityName, iif(con.FirstName + ' ' + con.LastName is not null, con.FirstName + ' ' + con.LastName, ''))) as AssociatedWith, act.[Status], act.[Description], format(act.DueDate,'MM/dd/yyyy') as DueDate,
                        opp.OpportunityName as AssociatedOpportunity,
                        act.Note, createdby.FirstName + ' ' + createdby.LastName as CreatedBy
                        into #Activity 
                        from Activity act
                        left join Lookup lktype on lktype.DataValue = iif(act.ActivityType != '-1', act.ActivityType, '') and lktype.DataKey = 'ActivityType'
                        left join Employee emp on emp.UserId = act.AssignedTo
                        left join Customer cus on cus.CustomerId = act.AssociatedWith
                        left join Opportunity opp on opp.OpportunityId = act.AssociatedWith
                        left join Contact con on con.ContactId = act.AssociatedWith
                        left join Employee createdby on createdby.UserId = act.CreatedBy
                        left join [Lookup] lkorigin on lkorigin.DataKey = 'ActivityOrigin' and lkorigin.DataValue = iif(act.Origin != '-1', act.Origin, '')
                        {0}
                        {1}
                        {2}
                        {3}
                        select * into #ActivityFilter from #Activity

                        select * from #ActivityFilter
                        order by Id desc


                        drop table #ActivityFilter
                        drop table #Activity";
            sqlQuery = string.Format(sqlQuery, subquery, filterquery, originquery, soldByQuery, NameSql);

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
