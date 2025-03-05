using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.DataAccess
{
	public partial class PtoDataAccess
	{
        public DataSet GetAllEmployeesPtoBySupervisorId(Guid userId, PayrollFilterModel filter, bool IsHrManager, bool? getreport)
        {
            string DateFilter = "";
            string PtoStatus = ""; 
            string SupervisorFilter = ""; 
            string UserFilter = ""; 

            string SqlQuery = @"
                                declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)*@pagesize
                                set @pageend = @pagesize

                                select pt.*
                                ,emp.FirstName + ' '+ emp.LastName as CreatedByVal
                                ,Request.FirstName+' '+Request.LastName as RequestedByVal
                                ,ISNULL(lkStart.DisplayText, '00:00AM') as TimeFromVal
                                ,ISNULL(lkEnd.DisplayText, '00:00AM') as TimeToVal
                                ,ISNULL(Request.PtoRemain,0) as PtoRemain
                                ,ISNULL(Request.PtoHour,0) as PtoHour
                                ,lktype.DisplayText as LeaveType,
                                IIF(DATEDIFF(hh, pt.StartDate, pt.EndDate) = 0,iif(pt.[Type] = 'FullDay',8,iif(pt.[Type] = 'HalfDay',4,DATEDIFF(hh, pt.StartDate +  CAST(iif(pt.TimeFrom != '-1' or pt.TimeFrom != '','00:00',pt.TimeFrom) as datetime), pt.StartDate + CAST(iif(pt.TimeTo != '-1' or pt.TimeTo != '','00:00',pt.TimeTo) as datetime)))),DATEDIFF(hh, pt.StartDate, pt.EndDate)) as Hours_Difference
                                into #pto
                                from Pto pt 
                                left join Employee emp on emp.UserId = pt.CreatedBy
                                left join Employee Request on Request.UserId = pt.UserId
                                left join Lookup lkStart on lkStart.DataKey='AbsenceCustomTime'
                                and lkStart.DataValue = pt.TimeFrom and pt.TimeFrom != '-1'


                                left join Lookup lkEnd on lkEnd.DataKey='AbsenceCustomTime'
                                and lkEnd.DataValue = pt.TimeTo and pt.TimeTo != '-1'
                                
                                left join Lookup lktype on lktype.DataKey='AbsenceType'
                                and lktype.DataValue = pt.Type 

                                where 1=1 
                                {5} {6} 
                                {1} {2}
                                
                                select * into #ptofilter
								from #pto

								select top(@pagesize) *
								from #ptofilter where Id not in(select top(@pagestart) Id from #pto #pt {3})
                                {4}
								select COUNT(*) CountTotal from #ptofilter

								drop table #pto
								drop table #ptofilter
                           
                                ";

            #region order by
            string subquery = "";
            string subquery1 = "";
            if (!string.IsNullOrWhiteSpace(filter.order))
            {
                if (filter.order == "ascending/type")
                {
                    subquery = "order by #pt.[Type] asc";
                    subquery1 = "order by [Type] asc";
                }
                else if (filter.order == "descending/type")
                {
                    subquery = "order by #pt.[Type] desc";
                    subquery1 = "order by [Type] desc";
                }
                else if (filter.order == "ascending/datetime")
                {
                    subquery = "order by #pt.StartDate asc";
                    subquery1 = "order by StartDate asc";
                }
                else if (filter.order == "descending/datetime")
                {
                    subquery = "order by #pt.StartDate desc";
                    subquery1 = "order by StartDate desc";
                }
                else if (filter.order == "ascending/hours")
                {
                    subquery = "order by #pt.Hours_Difference asc";
                    subquery1 = "order by Hours_Difference asc";
                }
                else if (filter.order == "descending/hours")
                {
                    subquery = "order by #pt.Hours_Difference desc";
                    subquery1 = "order by Hours_Difference desc";
                }
                else if (filter.order == "ascending/status")
                {
                    subquery = "order by #pt.[Status] asc";
                    subquery1 = "order by [Status] asc";
                }
                else if (filter.order == "descending/status")
                {
                    subquery = "order by #pt.[Status] desc";
                    subquery1 = "order by [Status] desc";
                }
                else if (filter.order == "ascending/payable")
                {
                    subquery = "order by #pt.[Payable] asc";
                    subquery1 = "order by [Payable] asc";
                }
                else if (filter.order == "descending/payable")
                {
                    subquery = "order by #pt.[Payable] desc";
                    subquery1 = "order by [Payable] desc";
                }
                else if (filter.order == "ascending/note")
                {
                    subquery = "order by #pt.[Notes] asc";
                    subquery1 = "order by [Notes] asc";
                }
                else if (filter.order == "descending/note")
                {
                    subquery = "order by #pt.[Notes] desc";
                    subquery1 = "order by [Notes] desc";
                }
                else
                {
                    subquery = "order by #pt.Id desc";
                    subquery1 = "order by Id desc";
                }
            }
            else
            {
                subquery = "order by #pt.Id desc";
                subquery1 = "order by Id desc";
            }
            #endregion

            #region Conditions
            if(!string.IsNullOrWhiteSpace(filter.CurrentEmployee) && filter.CurrentEmployee != "00000000-0000-0000-0000-000000000000")
            {
                UserFilter = string.Format(" and pt.UserId = '{0}'",filter.CurrentEmployee);
            }
            if(string.IsNullOrWhiteSpace(filter.StrStartDate))
            {
                filter.StrStartDate = filter.StartDate.ToString();
            }
            if (Convert.ToDateTime(filter.StrStartDate)!=new DateTime() && filter.EndDate != new DateTime())
            {
                DateFilter = string.Format("and pt.StartDate between '{0}' and '{1}'", Convert.ToDateTime(filter.StrStartDate).ToString("yyyy-MM-dd"), filter.EndDate.ToString("yyyy-MM-dd")); 
            }
            //if(filter.PtoStatus == "0")
            //{
            //    PtoStatus = "and pt.Status = 'Sent To Supervisor'";
            //}
            //if (filter.PtoStatus == "1")
            //{
            //    PtoStatus = "and pt.Status = 'Accepted'";
            //}
            //if (filter.PtoStatus == "2")
            //{
            //    PtoStatus = "and pt.Status = 'Rejected'";
            //}

            //string Query = "";
            string _SubQuery = "";
            if (!string.IsNullOrWhiteSpace(filter.PtoStatus))
            {
                if (filter.PtoStatus.Contains("0"))
                {
                    _SubQuery += " or pt.Status = 'Sent To Supervisor'";
                }
                if (filter.PtoStatus.Contains("1"))
                {
                    _SubQuery += " or pt.Status = 'Accepted'";
                }
                if (filter.PtoStatus.Contains("2"))
                {
                    _SubQuery += " or pt.Status = 'Rejected'";
                }
                if (filter.PtoStatus.Contains("Past"))
                {
                    _SubQuery += " or pt.Status = 'Past'";
                }
            }
            if (!string.IsNullOrWhiteSpace(_SubQuery))
            {
                _SubQuery = _SubQuery.Substring(3, _SubQuery.Length - 3);
                PtoStatus = string.Format(" and({0})", _SubQuery);
            }

            if (IsHrManager)
            {
                SupervisorFilter = "";
            }
            else
            {
                SupervisorFilter = string.Format("and pt.UserId in (select UserId from Employee where SuperVisorId = '{0}')",userId);
            }
            if(getreport.HasValue && getreport.Value == true)
            {
                filter.pageno1 = 1;
                filter.pagesize1 = 99999;
            }
            #endregion

            try
            {
                SqlQuery = string.Format(SqlQuery, 
                    userId, /*0*/ 
                    DateFilter, /*1*/
                    PtoStatus, /*2*/
                    subquery, /*3*/
                    subquery1, /*4*/
                    SupervisorFilter,/*5*/
                    UserFilter/*6*/);
                using (SqlCommand cmd = GetSQLCommand(SqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", filter.pageno1));
                    AddParameter(cmd, pInt32("pagesize", filter.pagesize1));

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetAllPtoByUserId(Guid userId, PayrollFilterModel filter, bool? getreport)
        {
            string DateFilterQuery = "";
            string PtoStatus = "";
            string SqlQuery = @"
                                declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)*@pagesize
                                set @pageend = @pagesize

                                select pt.*,lktype.DisplayText as LeaveType
                                ,emp.FirstName+' '+emp.LastName  as CreatedByVal
                                ,lkStart.DisplayText as TimeFromVal
                                ,lkEnd.DisplayText as TimeToVal,
                                IIF(DATEDIFF(hh, pt.StartDate, pt.EndDate) = 0,iif(pt.[Type] = 'FullDay',8,iif(pt.[Type] = 'HalfDay',4,DATEDIFF(hh, pt.StartDate + CAST(pt.TimeFrom as datetime), pt.StartDate + CAST(pt.TimeTo as datetime)))),DATEDIFF(hh, pt.StartDate, pt.EndDate)) as Hours_Difference
                                into #pto
                                from Pto pt 
                                left join Employee emp on pt.CreatedBy = emp.UserId

                                left join Lookup lkStart on lkStart.DataKey='AbsenceCustomTime'
                                and lkStart.DataValue = pt.TimeFrom and pt.TimeFrom != '-1'


                                left join Lookup lkEnd on lkEnd.DataKey='AbsenceCustomTime'
                                and lkEnd.DataValue = pt.TimeTo and pt.TimeTo != '-1'

                                left join Lookup lktype on lktype.DataKey='AbsenceType'
                                and lktype.DataValue = pt.Type 

                                where  pt.UserId = '{0}'
                                {1}{2}
                                select * into #ptofilter
								from #pto

								select top(@pagesize) *
								from #ptofilter where Id not in(select top(@pagestart) Id from #pto #pt {3})
                                {4}
								select COUNT(*) CountTotal from #ptofilter

								drop table #pto
								drop table #ptofilter
                                ";
            string subquery = "";
            string subquery1 = "";
            if (!string.IsNullOrWhiteSpace(filter.order))
            {
                if(filter.order == "ascending/type")
                {
                    subquery = "order by #pt.[Type] asc";
                    subquery1 = "order by [Type] asc";
                }
                else if(filter.order == "descending/type")
                {
                    subquery = "order by #pt.[Type] desc";
                    subquery1 = "order by [Type] desc";
                }
                else if (filter.order == "ascending/datetime")
                {
                    subquery = "order by #pt.StartDate asc";
                    subquery1 = "order by StartDate asc";
                }
                else if (filter.order == "descending/datetime")
                {
                    subquery = "order by #pt.StartDate desc";
                    subquery1 = "order by StartDate desc";
                }
                else if (filter.order == "ascending/hours")
                {
                    subquery = "order by #pt.Hours_Difference asc";
                    subquery1 = "order by Hours_Difference asc";
                }
                else if (filter.order == "descending/hours")
                {
                    subquery = "order by #pt.Hours_Difference desc";
                    subquery1 = "order by Hours_Difference desc";
                }
                else if (filter.order == "ascending/status")
                {
                    subquery = "order by #pt.[Status] asc";
                    subquery1 = "order by [Status] asc";
                }
                else if (filter.order == "descending/status")
                {
                    subquery = "order by #pt.[Status] desc";
                    subquery1 = "order by [Status] desc";
                }
                else if (filter.order == "ascending/payable")
                {
                    subquery = "order by #pt.[Payable] asc";
                    subquery1 = "order by [Payable] asc";
                }
                else if (filter.order == "descending/payable")
                {
                    subquery = "order by #pt.[Payable] desc";
                    subquery1 = "order by [Payable] desc";
                }
                else if (filter.order == "ascending/note")
                {
                    subquery = "order by #pt.[Notes] asc";
                    subquery1 = "order by [Notes] asc";
                }
                else if (filter.order == "descending/note")
                {
                    subquery = "order by #pt.[Notes] desc";
                    subquery1 = "order by [Notes] desc";
                }
            }
            else
            {
                subquery = "order by #pt.Id desc";
                subquery1 = "order by Id desc";
            }

            if(filter.StartDate != new DateTime() && filter.EndDate!= new DateTime())
            {
                DateFilterQuery = string.Format("and pt.StartDate between '{0}' and '{1}'",filter.StartDate.ToString("yyyy-MM-dd"), filter.EndDate.ToString("yyyy-MM-dd"));
            }
            //if (filter.PtoStatus == "0")
            //{
            //    PtoStatus = "and pt.Status = 'Sent To Supervisor'";
            //}
            //if (filter.PtoStatus == "1")
            //{
            //    PtoStatus = "and pt.Status = 'Accepted'";
            //}
            //if (filter.PtoStatus == "2")
            //{
            //    PtoStatus = "and pt.Status = 'Rejected'";
            //}
            //string Query = "";
            string _SubQuery = "";
            if(!string.IsNullOrWhiteSpace(filter.PtoStatus))
            {
                if (filter.PtoStatus.Contains("0"))
                {
                    _SubQuery += " or pt.Status = 'Sent To Supervisor'";
                }
                if (filter.PtoStatus.Contains("1"))
                {
                    _SubQuery += " or pt.Status = 'Accepted'";
                }
                if (filter.PtoStatus.Contains("2"))
                {
                    _SubQuery += " or pt.Status = 'Rejected'";
                }
                if (filter.PtoStatus.Contains("Past"))
                {
                    _SubQuery += " or pt.Status = 'Past'";
                }
            }
            

            if (!string.IsNullOrWhiteSpace(_SubQuery))
            {
                _SubQuery = _SubQuery.Substring(3, _SubQuery.Length - 3);
                PtoStatus = string.Format(" and({0})", _SubQuery);
            }
            if (getreport.HasValue && getreport.Value == true)
            {
                filter.pageno1 = 1;
                filter.pagesize1 = 99999;
            }
            try
            {
                SqlQuery = string.Format(SqlQuery, userId,DateFilterQuery,PtoStatus, subquery, subquery1);
                using (SqlCommand cmd = GetSQLCommand(SqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", filter.pageno1));
                    AddParameter(cmd, pInt32("pagesize", filter.pagesize1));

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataSet GetAllHolidayForCalendar(string UserIdList, DateTime SelectedDate, string status)
        {

            string SqlQuery = @"SELECT * FROM Pto pt where pt.Status = 'Accepted' {0} {1} {2}";

            string EmployeeSubquery = "";
            string MultipleStartDateSubquery = "";
            string MultipleEndDateSubquery = "";
            #region Conditions
            if (!string.IsNullOrWhiteSpace(UserIdList) && !string.IsNullOrEmpty(UserIdList) && UserIdList != "'null'" && UserIdList != "''")
            {
                EmployeeSubquery = string.Format("AND pt.UserId in ({0})", UserIdList);
            }
            if (status == "Weekly")
            {                
                var MultipleEndDate = SelectedDate.AddDays(6);
                MultipleStartDateSubquery = string.Format("AND pt.StartDate <= '{0} 23:59:59.000'", MultipleEndDate.ToString("yyyy-MM-dd"));
                MultipleEndDateSubquery = string.Format("AND pt.EndDate >= '{0} 00:00:00.000'", SelectedDate.ToString("yyyy-MM-dd"));
            }
            else if (status == "Daily")
            {
                MultipleStartDateSubquery = string.Format("AND pt.StartDate <= '{0} 23:59:59.000'", SelectedDate.ToString("yyyy-MM-dd"));
                MultipleEndDateSubquery = string.Format("AND pt.EndDate >= '{0} 00:00:00.000'", SelectedDate.ToString("yyyy-MM-dd"));
            }

            #endregion
            try
            {
                SqlQuery = string.Format(SqlQuery,
                    EmployeeSubquery, //0
                    MultipleStartDateSubquery, //1
                    MultipleEndDateSubquery/*2*/);
                using (SqlCommand cmd = GetSQLCommand(SqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
         
        public DataTable GetEmployeePtoHourByUserId(Guid UserId)
        {
            string sqlQuery = @"Select Sum(Minute) As TotalMinute from Pto Where UserId = '{0}' and Status = 'Accepted' and Payable = 1 ";
            try
            {
                sqlQuery = string.Format(sqlQuery, UserId);
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
