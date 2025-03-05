using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Globalization;

namespace HS.DataAccess
{
    public partial class ScheduleDataAccess
    {
        public bool ReseedScheduleTable()
        {
            string SqlQuery = @"Delete from Schedule
                                DBCC CHECKIDENT('Schedule', RESEED, 0)
                                ";
            try
            {
                using (SqlCommand cmd = GetSQLCommand(SqlQuery))
                {

                    ExecuteCommand(cmd);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public DataSet GetCustomCalendarScheduleListByCompanyId(Guid companyID, string startdate, string defaultView, string typeval, string userval, bool IsType, string ControllerName, string skills, int TktId, string CusName)
        {

            string sqlQuery = @"
                                select ca.AppointmentId, ca.CustomerId, ca.AppointmentDate, ca.AppointmentStartTime, ca.AppointmentEndTime, ca.IsAllDay, ca.AppointmentType
                                into #CustomerAppointmentTemp from CustomerAppointment ca where ca.CompanyId = '{0}' {4}
                                select ca.AppointmentId, ca.CustomerId, ca.AppointmentDate, ca.AppointmentStartTime, ca.AppointmentEndTime, ca.IsAllDay, ca.EmployeeId
                                into #CustomerAppointmentTemp1 from AdditionalMembersAppointment ca where ca.CompanyId = '{0}' {4}


                                (select ticket.Id as EventLeadId,
                            	ticket.TicketId as EventTicketId,
                            	ca.AppointmentId as EventAppointmentId,
                            	ca.CustomerId as  EventCustomerId,
                            	ISNULL(CONVERT(datetime, ca.AppointmentDate + ' ' + IIF(ca.AppointmentStartTime != '-1', ca.AppointmentStartTime, '00:00')), '1900-01-01 00:00:00') as EventStartDate,
                            	ISNULL(CONVERT(datetime, ca.AppointmentDate + ' ' + IIF(ca.AppointmentEndTime != '-1', ca.AppointmentEndTime, '00:00')), '1900-01-01 00:00:00') as EventEndDate,
                            	lk.DataValue as EventType,
                            	lk.DisplayText as EventDisplayType,
                            	lk.AlterDisplayText as EventColor,
                            	emp.FirstName + ' ' + emp.LastName as EmployeeName,
                            	emp.UserId as EventEmployeeGuidId,
                            	emp.Id as EmployeeIntId,
                            	ca.IsAllDay as EventAllDay,
                            	convert(nvarchar(50), ca.AppointmentDate, 23) as EventDate,
                            	ticket.Status as EventStatus,
                                --ticket.IsCallAhead,
                                --(select (IIF(ISNULL(inv.MinimumValue,0) > 0 and ISNULL(inv.IsAdjustmentDeleted,0) = 0, cast((inv.Amount + inv.MinimumValue) as decimal(10,2)), cast(inv.Amount as decimal(10,2))) - IIF(ISNULL(inv.CouponDiscountAmount,0) > 0, inv.CouponDiscountAmount, 0) + IIF(ISNULL(inv.PortableAmount,0) > 0 and ISNULL(inv.PortableDeleted,0) = 0, inv.PortableAmount, 0)) from Invoice inv where inv.TicketId = cast(ticket.TicketId as nvarchar(50)) and inv.BookingId = ticket.BookingId) as TotalAmount,
                                ticket.Subject as EventSubject,
                            	ticket.BookingId as BookingId,
                            	emp.CalendarColor,
                            	imgset.TicketStatusColor as StatusColor,
                            	imgset.Filename as EventIcon,
                            	'' as EventAdditionalMember
                            	Into #tempCA
                            from #CustomerAppointmentTemp ca 
                            
                            	left join TicketUser tu on tu.TiketId =ca.AppointmentId and tu.IsPrimary = 1
                            	left join Ticket ticket on ticket.TicketId = tu.TiketId
                            	left join Employee emp on emp.UserId =tu.UserId
                            	left join [Lookup] lk on lk.DataValue = iif(ca.AppointmentType != '-1', ca.AppointmentType, '') and lk.DataKey = 'TicketType'
                            	left Join TicketStatusImageSetting imgset on imgset.TicketStatus = ticket.[Status]
                            	
                            where tu.NotificationOnly = 0
                                {1}
                            	{2}
                            	and emp.IsCalendar = 1
                            	and lk.AlterDisplayText1 is not null and lk.AlterDisplayText1 != '' and lk.AlterDisplayText1 = 'True'
                            	and emp.Recruited = 1
                            	and emp.IsActive = 1
                            	and emp.IsDeleted = 0
                            	and ticket.TicketId is not null
                                {7}
                                {3}
                            	)
                            
                            union

                            (select ticket.Id as EventLeadId,
                            	ticket.TicketId as EventTicketId,
                            	ca.AppointmentId as EventAppointmentId,
                            	ca.CustomerId as  EventCustomerId,
                            	ISNULL(CONVERT(datetime, ca.AppointmentDate + ' ' + IIF(ca.AppointmentStartTime != '-1', ca.AppointmentStartTime, '00:00')), '1900-01-01 00:00:00') as EventStartDate,
                            	ISNULL(CONVERT(datetime, ca.AppointmentDate + ' ' + IIF(ca.AppointmentEndTime != '-1', ca.AppointmentEndTime, '00:00')), '1900-01-01 00:00:00') as EventEndDate,
                            	lk.DataValue as EventType,
                            	lk.DisplayText as EventDisplayType,
                            	lk.AlterDisplayText as EventColor,
                            	emp.FirstName + ' ' + emp.LastName as EmployeeName,
                            	emp.UserId as EventEmployeeGuidId,
                            	emp.Id as EmployeeIntId,
                            	ca.IsAllDay as EventAllDay,
                            	convert(nvarchar(50), ca.AppointmentDate, 23) as EventDate,
                            	ticket.Status as EventStatus,
                                --ticket.IsCallAhead,
                                --(select (IIF(ISNULL(inv.MinimumValue,0) > 0 and ISNULL(inv.IsAdjustmentDeleted,0) = 0, cast((inv.Amount + inv.MinimumValue) as decimal(10,2)), cast(inv.Amount as decimal(10,2))) - IIF(ISNULL(inv.CouponDiscountAmount,0) > 0, inv.CouponDiscountAmount, 0) + IIF(ISNULL(inv.PortableAmount,0) > 0 and ISNULL(inv.PortableDeleted,0) = 0, inv.PortableAmount, 0)) from Invoice inv where inv.TicketId = cast(ticket.TicketId as nvarchar(50)) and inv.BookingId = ticket.BookingId) as TotalAmount,
                                ticket.Subject as EventSubject,
                            	ticket.BookingId as BookingId,
                            	emp.CalendarColor,
                            	imgset.TicketStatusColor as StatusColor,
                            	imgset.Filename as EventIcon,
                            	'Additional' as EventAdditionalMember
                            from #CustomerAppointmentTemp1 ca 

                            	left join TicketUser tu on tu.UserId = ca.EmployeeId and tu.IsPrimary = 0 
                            	left join Ticket ticket on ticket.TicketId = ca.AppointmentId 
                            	left join Employee emp on emp.UserId =tu.UserId
                            	left join[Lookup] lk on lk.DataValue = iif(ticket.TicketType != '-1', ticket.TicketType, '') and lk.DataKey = 'TicketType' 
                            	left Join TicketStatusImageSetting imgset on imgset.TicketStatus = ticket.[Status]
                            	
                            where tu.NotificationOnly = 0
                                {1}
                            	{2}
                            	and emp.IsCalendar = 1
                            	and lk.AlterDisplayText1 is not null and lk.AlterDisplayText1 != '' and lk.AlterDisplayText1 = 'True'
                            	and emp.Recruited = 1
                            	and emp.IsActive = 1
                            	and emp.IsDeleted = 0
                            	and ticket.TicketId is not null
                                {7}
                                {3})
                                {6}

                            	select tca.*,
                            	cus.Id as EventCustomerIntId,
                            	cus.Street as EventStreet,
                                cus.MonthlyMonitoringFee as TotalAmount,
                            	cus.City+', ' + cus.State +' '+ cus.ZipCode as EventLocate,
                            	case when cus.City is not null and cus.City != '' and cus.State is not null and cus.State != '' then cus.City+', ' + cus.State when (cus.City is null or cus.City = '') and cus.State is not null and cus.State != '' then cus.State when cus.City is not null and cus.City != '' and (cus.State is null or cus.State = '') then cus.City else '' end as EventTicketLocation,
                            	CASE WHEN cus.DBA is not null and cus.DBA != '' THEN cus.DBA WHEN cus.BusinessName is not null and cus.BusinessName != '' THEN cus.BusinessName ELSE cus.FirstName + ' ' + cus.LastName END AS EventCustomerName
                                --CASE WHEN (select top(1) bk.CellNo from Booking bk where bk.BookingId = tca.BookingId) is not null and (select top(1) bk.CellNo from Booking bk where bk.BookingId = tca.BookingId) != '' THEN (select top(1) bk.CellNo from Booking bk where bk.BookingId = tca.BookingId) WHEN cus.PrimaryPhone is not null and cus.PrimaryPhone != '' THEN cus.PrimaryPhone WHEN cus.CellNo is not null and cus.CellNo != '' THEN cus.CellNo ELSE '' END AS CusPhone
                            
                            	from #tempCA tca
                            	left join Customer cus on  tca.EventCustomerId = cus.CustomerId
                                where cus.Id > 0 {8}
                            	order by tca.EmployeeIntId, tca.EventStartDate
                            	Drop table #tempCA
                                drop table #CustomerAppointmentTemp
                                drop table #CustomerAppointmentTemp1

                            	-- For Getting Employee List
                            	{5}
                                ";


            string BookingSqlQuery = "";
            string empquery = "";
            string DateSubquery = "";
            string DateSubquery1 = "";
            string TicketTypesubquery = "";
            string Employeelist = "";
            string TicketStatusQuery = " and ticket.[Status] NOT LIKE '%cancel%' and ticket.[Status] NOT LIKE '%lost%' and ticket.[Status] NOT LIKE '%no%'"; /*and ticket.[Status] NOT LIKE '%lost%'*/
            string SkillsQuery = "";
            string SearchQuery = "";
            if (!string.IsNullOrWhiteSpace(CusName))
            {
                SearchQuery = string.Format("and cus.FirstName + ' '+ cus.LastName LIKE '%{0}%'", CusName);
            }
            else if (TktId > 0)
            {
                SearchQuery = string.Format("and tca.EventLeadId like '{0}%'", TktId);
            }

            if (!string.IsNullOrWhiteSpace(defaultView) && defaultView == "Daily" && !string.IsNullOrWhiteSpace(startdate))
            {
                DateSubquery = string.Format("and ca.AppointmentDate between '{0} 00:00:00.000' and '{0} 23:59:59.000'", startdate);
                DateSubquery1 = string.Format("and ca.ScheduleDate between '{0} 00:00:00.000' and '{0} 23:59:59.000'", startdate);
            }
            else if (!string.IsNullOrWhiteSpace(defaultView) && (defaultView == "Weekly" || defaultView == "List") && !string.IsNullOrWhiteSpace(startdate))
            {

                var datestart = Convert.ToDateTime(startdate);
                var dateend = Convert.ToDateTime(startdate).AddDays(6);
                DateSubquery = string.Format("and ca.AppointmentDate between '{0} 00:00:00.000' and '{1} 23:59:59.000'", datestart.ToString("yyyy-MM-dd"), dateend.ToString("yyyy-MM-dd"));
            }
            else if (!string.IsNullOrWhiteSpace(defaultView) && defaultView == "Monthly" && !string.IsNullOrWhiteSpace(startdate))
            {
                var stardate = Convert.ToDateTime(startdate);
                var datestart = new DateTime(stardate.Year, stardate.Month, 1);
                var dateend = new DateTime(stardate.Year, stardate.Month, DateTime.DaysInMonth(stardate.Year, stardate.Month));
                DateSubquery = string.Format("and ca.AppointmentDate between '{0} 00:00:00.000' and '{1} 23:59:59.000'", datestart.ToString("yyyy-MM-dd"), dateend.ToString("yyyy-MM-dd"));
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(defaultView) && defaultView == "Daily")
                {
                    DateSubquery = string.Format("and ca.AppointmentDate between '{0} 00:00:00.000' and '{0} 23:59:59.000'", DateTime.Now.ToString("yyyy-MM-dd"));
                }
                else if (!string.IsNullOrWhiteSpace(defaultView) && (defaultView == "Weekly" || defaultView == "List"))
                {
                    if (string.IsNullOrWhiteSpace(startdate))
                    {
                        startdate = DateTime.Now.ToString();
                    }
                    var datestart = Convert.ToDateTime(startdate);
                    var dateend = Convert.ToDateTime(startdate).AddDays(6);
                    DateSubquery = string.Format("and ca.AppointmentDate between '{0} 00:00:00.000' and '{1} 23:59:59.000'", datestart.ToString("yyyy-MM-dd"), dateend.ToString("yyyy-MM-dd"));
                }
                else if (!string.IsNullOrWhiteSpace(defaultView) && defaultView == "Monthly")
                {
                    var datestart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    var dateend = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
                    DateSubquery = string.Format("and ca.AppointmentDate between '{0} 00:00:00.000' and '{1} 23:59:59.000'", datestart.ToString("yyyy-MM-dd"), dateend.ToString("yyyy-MM-dd"));
                }
            }

            if (!string.IsNullOrWhiteSpace(skills) && skills != "'null'" && skills != "''")
            {
                SkillsQuery = string.Format(" and ticket.[Subject] in ({0}) ", skills);
            }
            if (!string.IsNullOrWhiteSpace(typeval) && typeval != "'null'" && typeval != "''")
            {
                if (!IsType)
                {
                    if (typeval.Contains(","))
                    {
                        TicketStatusQuery = string.Format(" and ticket.[Status] in ({0})", typeval);
                    }
                    else
                    {
                        TicketStatusQuery = string.Format(" and ticket.[Status] = {0}", typeval);
                    }
                }
                else
                {
                    TicketTypesubquery = string.Format("and ticket.TicketType in ({0})", typeval);
                }
            }
            if (!string.IsNullOrWhiteSpace(userval) && userval != "'null'" && userval != "''")
            {
                empquery = string.Format("and emp.UserId in ({0})", userval);
            }
            Employeelist = string.Format("select iif((select (select CHARINDEX('technician', Tag) from PermissionGroup _pgroup where _pgroup.Id = _up.PermissionGroupId) from UserPermission _up where _up.PermissionGroupId is not null and _up.UserId = emp.Userid) > 0, emp.FirstName + ' ' + emp.LastName, iif(emp.UserId = '22222222-2222-2222-2222-222222222222', emp.FirstName + ' ' + emp.LastName, emp.FirstName + ' ' + emp.LastName)) as ResourceName, emp.UserId, emp.Id, pg.[Name] from Employee emp join UserPermission up on up.UserId = emp.UserId join PermissionGroup pg on pg.Id = up.PermissionGroupId where emp.IsCalendar is not null and emp.IsCalendar = 1 and emp.Recruited = 1 and emp.IsActive = 1 and emp.IsDeleted = 0 and emp.CompanyId = '{0}' {1}", companyID, empquery);

            if (ControllerName == "Booking")
            {
                BookingSqlQuery = string.Format(@" union

                            (select ca.Id as EventLeadId,
                            	'00000000-0000-0000-0000-000000000000' as EventTicketId,
                            	'00000000-0000-0000-0000-000000000000' as EventAppointmentId,
                            	ca.CustomerId as  EventCustomerId,
                            	ISNULL(ca.StartTime, '2021-01-01 00:00:00') as EventStartDate,
                            	ISNULL(ca.EndTime, '2021-01-01 00:00:00') as EventEndDate,
                            	ca.ServiceType as EventType,
                            	ca.ServiceType as EventDisplayType,
                            	'ACDF87' as EventColor,
                            	emp.FirstName + ' ' + emp.LastName as EmployeeName,
                            	emp.UserId as EventEmployeeGuidId,
                            	emp.Id as EmployeeIntId,
                            	0 as EventAllDay,
                            	convert(nvarchar(50), ca.ScheduleDate, 23) as EventDate,
                            	'Created' as EventStatus,
                                NULL as IsCallAhead,
                                0 as TotalAmount,
                                '' as EventSubject,
                            	ca.BookingId as BookingId,
                            	emp.CalendarColor,
                            	'FFFFFF' as StatusColor,
                            	NULL as EventIcon,
                            	'Temp' as EventAdditionalMember
                            from AssignedServicesToEmployee ca 
                            	left join Employee emp on emp.UserId = ca.AssignedEmployee
                            	
                            where ca.ScheduleDate is not null  and ca.IsApproved = 0 and ca.AssignedEmployee != '00000000-0000-0000-0000-000000000000'
                                {0}
                            	{1}
                                )", empquery, DateSubquery1);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, companyID
                    , TicketStatusQuery
                    , TicketTypesubquery
                    , empquery
                    , DateSubquery
                    , Employeelist
                    , BookingSqlQuery
                    , SkillsQuery
                    , SearchQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
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
        public DataSet GetCustomCalendarScheduleListByCompanyId(Guid companyID, string startdate, string defaultView, string typeval, string userval, bool IsType, bool IsDeactive)
        {
            string CountQuery = "";
            string subQuery = "select emp.UserId into #CurrentEmp from Employee emp where emp.Recruited = 1 and emp.IsCalendar = 1 and emp.IsActive = 1 and emp.IsDeleted = 0";
            string EmpConditionQuery = @" and emp.IsCalendar = 1 and emp.Recruited = 1 and emp.IsActive = 1 and emp.IsDeleted = 0";
            string sqlQuery = @" {8}                        
                                (select ticket.Id as EventLeadId,
                            	ticket.TicketId as EventTicketId,
                            	ca.AppointmentId as EventAppointmentId,
                            	ca.CustomerId as  EventCustomerId,
                            	ISNULL(CONVERT(datetime, ca.AppointmentDate + ' ' + IIF(ca.AppointmentStartTime != '-1', ca.AppointmentStartTime, '00:00')), '1900-01-01 00:00:00') as EventStartDate,
                            	ISNULL(CONVERT(datetime, ca.AppointmentDate + ' ' + IIF(ca.AppointmentEndTime != '-1', ca.AppointmentEndTime, '00:00')), '1900-01-01 00:00:00') as EventEndDate,
                            	lk.DataValue as EventType,
                            	lk.DisplayText as EventDisplayType,
                            	lk.AlterDisplayText as EventColor,
                            	emp.FirstName + ' ' + emp.LastName as EmployeeName,
                            	emp.UserId as EventEmployeeGuidId,
                            	emp.Id as EmployeeIntId,
                            	ca.IsAllDay as EventAllDay,
                            	convert(nvarchar(50), ca.AppointmentDate, 23) as EventDate,
                            	ticket.Status as EventStatus,
                            	ticket.BookingId as BookingId,
                            	emp.CalendarColor,
                            	imgset.TicketStatusColor as StatusColor,
                            	imgset.Filename as EventIcon,
                            	'' as EventAdditionalMember
                            	Into #tempCA
                            from CustomerAppointment ca 
                            
                            	left join TicketUser tu on tu.TiketId =ca.AppointmentId and tu.IsPrimary = 1
                            	left join Ticket ticket on ticket.TicketId = tu.TiketId
                            	left join Employee emp on emp.UserId =tu.UserId
                            	left join [Lookup] lk on lk.DataValue = iif(ca.AppointmentType != '-1', ca.AppointmentType, '') and lk.DataKey = 'TicketType'
                            	left Join TicketStatusImageSetting imgset on imgset.TicketStatus = ticket.[Status]
                            	
                            where tu.NotificationOnly = 0
                                and ca.CompanyId = '{0}'
                                {1}
                            	{2}
                                {7}
                            	and lk.AlterDisplayText1 is not null and lk.AlterDisplayText1 != '' and lk.AlterDisplayText1 = 'True'
                                
                            	and ticket.TicketId is not null
                                {3}
                            	{4})
                            
                            union

                            (select ticket.Id as EventLeadId,
                            	ticket.TicketId as EventTicketId,
                            	ca.AppointmentId as EventAppointmentId,
                            	ca.CustomerId as  EventCustomerId,
                            	ISNULL(CONVERT(datetime, ca.AppointmentDate + ' ' + IIF(ca.AppointmentStartTime != '-1', ca.AppointmentStartTime, '00:00')), '1900-01-01 00:00:00') as EventStartDate,
                            	ISNULL(CONVERT(datetime, ca.AppointmentDate + ' ' + IIF(ca.AppointmentEndTime != '-1', ca.AppointmentEndTime, '00:00')), '1900-01-01 00:00:00') as EventEndDate,
                            	lk.DataValue as EventType,
                            	lk.DisplayText as EventDisplayType,
                            	lk.AlterDisplayText as EventColor,
                            	emp.FirstName + ' ' + emp.LastName as EmployeeName,
                            	emp.UserId as EventEmployeeGuidId,
                            	emp.Id as EmployeeIntId,
                            	ca.IsAllDay as EventAllDay,
                            	convert(nvarchar(50), ca.AppointmentDate, 23) as EventDate,
                            	ticket.Status as EventStatus,
                            	ticket.BookingId as BookingId,
                            	emp.CalendarColor,
                            	imgset.TicketStatusColor as StatusColor,
                            	imgset.Filename as EventIcon,
                            	'Additional' as EventAdditionalMember
                            from AdditionalMembersAppointment ca 

                            	left join TicketUser tu on tu.UserId = ca.EmployeeId and tu.IsPrimary = 0 
                            	left join Ticket ticket on ticket.TicketId = ca.AppointmentId 
                            	left join Employee emp on emp.UserId =tu.UserId
                            	left join[Lookup] lk on lk.DataValue = iif(ticket.TicketType != '-1', ticket.TicketType, '') and lk.DataKey = 'TicketType' 
                            	left Join TicketStatusImageSetting imgset on imgset.TicketStatus = ticket.[Status]
                            	
                            where tu.NotificationOnly = 0
                                and ca.CompanyId = '{0}'
                                {1}
                            	{2}
                            	and lk.AlterDisplayText1 is not null and lk.AlterDisplayText1 != '' and lk.AlterDisplayText1 = 'True'
                                {7}
                            	and ticket.TicketId is not null
                                {3}
                            	{4})
                            
                            	select tca.*,
                            	cus.Id as EventCustomerIntId,
                            	cus.Street as EventStreet,
                            	cus.City+', ' + cus.State +' '+ cus.ZipCode as EventLocate,
                            	case when cus.City is not null and cus.City != '' and cus.State is not null and cus.State != '' then cus.City+', ' + cus.State when (cus.City is null or cus.City = '') and cus.State is not null and cus.State != '' then cus.State when cus.City is not null and cus.City != '' and (cus.State is null or cus.State = '') then cus.City else '' end as EventTicketLocation,
                            	CASE WHEN cus.DBA is not null and cus.DBA != '' THEN cus.DBA WHEN cus.BusinessName is not null and cus.BusinessName != '' THEN cus.BusinessName ELSE cus.FirstName + ' ' + cus.LastName END AS EventCustomerName
                            
                            	from #tempCA tca
                            	left join Customer cus on  tca.EventCustomerId = cus.CustomerId
                            	order by tca.EmployeeIntId, tca.EventStartDate
                            	Drop table #tempCA

                           -- For Getting Employee List
                            	{5}
                           -- For Count Ticket
                                {6}
                                drop Table #CurrentEmp";

            string empquery = "";
            string DateSubquery = "";
            string TicketTypesubquery = "";
            string Employeelist = "";
            string TicketStatusQuery = " and ticket.[Status] != 'Lost' ";
            if (!string.IsNullOrWhiteSpace(defaultView) && defaultView == "Daily" && !string.IsNullOrWhiteSpace(startdate))
            {
                DateSubquery = string.Format("and ca.AppointmentDate between '{0} 00:00:00.000' and '{0} 23:59:59.000'", startdate);
            }
            else if (!string.IsNullOrWhiteSpace(defaultView) && (defaultView == "Weekly" || defaultView == "List") && !string.IsNullOrWhiteSpace(startdate))
            {

                var datestart = Convert.ToDateTime(startdate);
                var dateend = Convert.ToDateTime(startdate).AddDays(6);
                DateSubquery = string.Format("and ca.AppointmentDate between '{0} 00:00:00.000' and '{1} 23:59:59.000'", datestart.ToString("yyyy-MM-dd"), dateend.ToString("yyyy-MM-dd"));
            }
            else if (!string.IsNullOrWhiteSpace(defaultView) && defaultView == "Monthly" && !string.IsNullOrWhiteSpace(startdate))
            {
                var stardate = Convert.ToDateTime(startdate);
                var datestart = new DateTime(stardate.Year, stardate.Month, 1);
                var dateend = new DateTime(stardate.Year, stardate.Month, DateTime.DaysInMonth(stardate.Year, stardate.Month));
                DateSubquery = string.Format("and ca.AppointmentDate between '{0} 00:00:00.000' and '{1} 23:59:59.000'", datestart.ToString("yyyy-MM-dd"), dateend.ToString("yyyy-MM-dd"));
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(defaultView) && defaultView == "Daily")
                {
                    DateSubquery = string.Format("and ca.AppointmentDate between '{0} 00:00:00.000' and '{0} 23:59:59.000'", DateTime.Now.ToString("yyyy-MM-dd"));
                }
                else if (!string.IsNullOrWhiteSpace(defaultView) && (defaultView == "Weekly" || defaultView == "List"))
                {
                    if (string.IsNullOrWhiteSpace(startdate))
                    {
                        startdate = DateTime.Now.ToString();
                    }
                    var datestart = Convert.ToDateTime(startdate);
                    var dateend = Convert.ToDateTime(startdate).AddDays(6);
                    DateSubquery = string.Format("and ca.AppointmentDate between '{0} 00:00:00.000' and '{1} 23:59:59.000'", datestart.ToString("yyyy-MM-dd"), dateend.ToString("yyyy-MM-dd"));
                }
                else if (!string.IsNullOrWhiteSpace(defaultView) && defaultView == "Monthly")
                {
                    var datestart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    var dateend = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
                    DateSubquery = string.Format("and ca.AppointmentDate between '{0} 00:00:00.000' and '{1} 23:59:59.000'", datestart.ToString("yyyy-MM-dd"), dateend.ToString("yyyy-MM-dd"));
                }
            }

            if (!string.IsNullOrWhiteSpace(typeval) && typeval != "'null'" && typeval != "''")
            {
                if (!IsType)
                {
                    TicketStatusQuery = string.Format(" and ticket.[Status] = {0}", typeval);
                }
                else
                {
                    TicketTypesubquery = string.Format("and ticket.TicketType in ({0})", typeval);
                }
            }
            if (!string.IsNullOrWhiteSpace(userval) && userval != "'null'" && userval != "''")
            {
                empquery = string.Format("and emp.UserId in ({0})", userval);
            }
            Employeelist = string.Format("select iif((select (select CHARINDEX('technician', Tag) from PermissionGroup _pgroup where _pgroup.Id = _up.PermissionGroupId) from UserPermission _up where _up.PermissionGroupId is not null and _up.UserId = emp.Userid) > 0, emp.FirstName + ' ' + emp.LastName, iif(emp.UserId = '22222222-2222-2222-2222-222222222222', emp.FirstName + ' ' + emp.LastName, emp.FirstName + ' ' + emp.LastName)) as ResourceName, emp.UserId, emp.Id from Employee emp where emp.IsCalendar is not null and emp.IsCalendar = 1 and emp.Recruited = 1 and emp.IsActive = 1 and emp.IsDeleted = 0 and CompanyId = '{0}' {1}", companyID, empquery);
            if (IsDeactive)
            {
                subQuery = string.Format(@"select emp.UserId into #CurrentEmp from Employee emp where emp.Recruited = 1 and emp.IsCalendar = 1 and emp.IsActive = 1 and emp.IsDeleted = 0
                                    select distinct emp.UserId into #DeactiveId from CustomerAppointment ca 
                                    left join TicketUser tu on tu.TiketId = ca.AppointmentId 
                                    left join Employee emp on emp.UserId = tu.UserId 
                                    left join [Lookup] lk on lk.DataValue = iif(ca.AppointmentType != '-1', ca.AppointmentType, '') and lk.DataKey = 'TicketType' 
                                    where lk.AlterDisplayText1 is not null and lk.AlterDisplayText1 != '' and lk.AlterDisplayText1 = 'True' 
                                    {0} 
                                    and emp.UserId not in (select UserId from #CurrentEmp)", DateSubquery);
                CountQuery = string.Format(@"
                                    select Count(*) as TicketCount from CustomerAppointment ca 
                                    left join TicketUser tu on tu.TiketId = ca.AppointmentId 
                                    left join Employee emp on emp.UserId = tu.UserId 
                                    left join Ticket tic on tic.TicketId = tu.TiketId
                                    left join [Lookup] lk on lk.DataValue = iif(ca.AppointmentType != '-1', ca.AppointmentType, '') and lk.DataKey = 'TicketType' 
                                    where lk.AlterDisplayText1 is not null and lk.AlterDisplayText1 != '' and lk.AlterDisplayText1 = 'True' 
                                    {0} 
                                    and ca.CompanyId = '{1}'
									and tic.[Status] != 'Lost' 
									and tic.TicketId is not null 
                                    and emp.UserId in (select UserId from #CurrentEmp)", DateSubquery, companyID);
                EmpConditionQuery = "";
                empquery = "and emp.UserId in (select UserId from #DeactiveId)";
                Employeelist = string.Format(@"select emp.FirstName + ' ' + emp.LastName as ResourceName, emp.UserId, emp.Id from Employee emp where emp.UserId in (select UserId from #DeactiveId) and emp.CompanyId = '{0}'" +
                    "                         drop table #DeactiveId", companyID);
            }
            else
            {
                CountQuery = string.Format(@" 
                                    select Count(*) as TicketCount from CustomerAppointment ca 
                                    left join TicketUser tu on tu.TiketId = ca.AppointmentId 
                                    left join Employee emp on emp.UserId = tu.UserId 
                                    left join Ticket tic on tic.TicketId = tu.TiketId
                                    left join [Lookup] lk on lk.DataValue = iif(ca.AppointmentType != '-1', ca.AppointmentType, '') and lk.DataKey = 'TicketType' 
                                    where lk.AlterDisplayText1 is not null and lk.AlterDisplayText1 != '' and lk.AlterDisplayText1 = 'True' 
                                    and ca.CompanyId = '{1}'
									and tic.[Status] != 'Lost' 
									and tic.TicketId is not null                                    
                                    {0}
                                    and emp.UserId not in (select UserId from #CurrentEmp)", DateSubquery, companyID);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, companyID //0
                    , TicketStatusQuery //1
                    , TicketTypesubquery //2
                    , empquery  //3
                    , DateSubquery   //4                  
                    , Employeelist  //5
                    , CountQuery //6
                    , EmpConditionQuery //7
                     , subQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
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
        public DataSet GetCustomCalenderStatusByCompanyId(Guid companyID)
        {
            string sqlQuery = @"select TicketStatus, Filename, TicketStatusColor From TicketStatusImageSetting tsis join Lookup lk on lk.DataValue = tsis.TicketStatus AND lk.DataKey='TicketStatus' AND lk.IsActive = 1 where tsis.IsActive = 1 AND tsis.CompanyId = '{0}' AND  tsis.Filename != null OR tsis.Filename != '' ";

            try
            {
                sqlQuery = string.Format(sqlQuery, companyID);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
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
        public DataSet GetScheduleListByCompanyId(Guid companyID, string emptag, string empid, int ResourceLimit, int pageno, bool ReminderResult, int pageno1, string startdate, string defaultView, string typeval, string userval, string EventUserId, string TicketId, bool ispermit)
        {
            string LostStatusQuery = " and ticket.[Status] != 'Pending' ";
            if (!ispermit)
            {
                LostStatusQuery += string.Format(" and ticket.[Status] != 'Lost' ");
            }
            string sqlQuery = @"
                                declare @CompanyId uniqueidentifier
                                declare @pagestart int
                                declare @pageSize int
                                set @pagestart=({6}-1)* {5} 
                                set @pageSize = {5}
                                set @CompanyId = '{0}'

                                (select * into #calendar from (select
                                emp.FirstName + ' ' + emp.LastName as EventTitle,
                                ISNULL(CONVERT(datetime, ca.AppointmentDate + ' ' + IIF(ca.AppointmentStartTime != '-1', ca.AppointmentStartTime, '00:00')), '1900-01-01 00:00:00') as EventStartDate,
								ISNULL(CONVERT(datetime, ca.AppointmentDate + ' ' + IIF(ca.AppointmentEndTime != '-1', ca.AppointmentEndTime, '00:00')), '1900-01-01 00:00:00') as EventEndDate,
                                ca.AppointmentType as EventType,
                                lk.DisplayText as EventDisplayType,
                                ticket.Id as EventLeadId,
                                isnull(cus.CustomerId, '00000000-0000-0000-0000-000000000000') as EventCusId,
                                emp.UserId as EventCustomId,
								ca.AppointmentId as EventAppid,
                                iif((select (select CHARINDEX('technician', Tag) from PermissionGroup _pgroup where _pgroup.Id = _up.PermissionGroupId) from UserPermission _up where _up.PermissionGroupId is not null and _up.UserId = emp.Userid) > 0, emp.FirstName + ' ' + emp.LastName + ' (T)', iif(emp.UserId = '22222222-2222-2222-2222-222222222222', emp.FirstName + ' ' + emp.LastName + ' (T)', emp.FirstName + ' ' + emp.LastName)) as EventResourceName,
                                emp.IsCalendar as EventIsCalendar,
								iif((select count(map.Id) from Permission per left join PermissionGroupMap map on map.PermissionId = per.Id and map.CompanyId = per.CompanyId where Name = 'CustomerTicketDispatch') > 0 and ticket.IsDispatch != null and ticket.IsDispatch = 0 and ticket.[Status] != 'Completed' and ticket.IsClosed = 0, 'FFFF00', 
								
								iif((select AlterDisplayText from [LookUp] lkcolor where DataKey = 'TicketType' and lkcolor.DataValue = ca.AppointmentType) != 'FFFFFF' and (select AlterDisplayText from [LookUp] lkcolor where DataKey = 'TicketType' and lkcolor.DataValue = ca.AppointmentType) != '', (select AlterDisplayText from [LookUp] lkcolor where DataKey = 'TicketType' and lkcolor.DataValue = ca.AppointmentType), 'ccc')) as EventColor,
                                emp.Id as EventId,
                                cus.FirstName + ' ' + cus.LastName as EventCustomerName,
                                cus.BusinessName as EventBusinessName,
                                ca.IsAllDay as EventAllDay,
								0 as EventIsLead,
                                isnull((select Street from CustomerAddress where RefId = ticket.BookingId and CustomerId = ticket.CustomerId and AddressType = REPLACE(ticket.TicketType, ' ', '') + 'Location'), cus.Street) as EventStreet, 
								isnull((select IIF(cus.City != '', cus.City + ', ', '') + cus.[State] + cus.ZipCode from CustomerAddress where RefId = ticket.BookingId and CustomerId = ticket.CustomerId and AddressType = REPLACE(ticket.TicketType, ' ', '') + 'Location'), IIF(cus.City != '', cus.City + ', ', '') + cus.[State] + cus.ZipCode) as EventLocate,
                                convert(nvarchar(50), ca.AppointmentDate, 23) as EventDate,
                                '' as EventCalendarCount,
                                ticket.TicketId as EventTicketId
                                ,ticket.[Status] as EventStatus
                                ,'' as HoverTitle
                                
                                ,cus.DBA as EventDBA
                                ,ticket.BookingId as EventBookingId
                                ,imgset.[Filename] as EventIcon
                                ,ISNULL(RescheduleTicketId, 0) as EventRescheduleId
                                {19}
                                from CustomerAppointment ca
								left join Customer cus
                                on cus.CustomerId=ca.CustomerId
								left join TicketUser tu on   tu.TiketId =ca.AppointmentId  and tu.IsPrimary = 1
                                left join Ticket ticket on ticket.TicketId = tu.TiketId
								left join Employee emp on  emp.UserId =tu.UserId
                                left join [Lookup] lk on lk.DataValue = iif(ca.AppointmentType != '-1', ca.AppointmentType, '') and lk.DataKey = 'TicketType'
                                left Join TicketStatusImageSetting imgset on imgset.TicketStatus = ticket.[Status]
                                where ca.CompanyId = @CompanyId
                                and tu.NotificationOnly = 0
                                {12}
                                {16}
                                and cus.IsActive =1
                                and emp.IsCalendar is not null
                                and emp.IsCalendar = 1
                                and lk.AlterDisplayText1 is not null and lk.AlterDisplayText1 != '' and lk.AlterDisplayText1 = 'True'
                                and emp.Recruited = 1
                                and emp.IsActive = 1
                                and emp.IsDeleted = 0
                                and ticket.TicketId is not null
                                and cus.CustomerId is not null
                                {17}
                                {1}
                                {14}
                                {24}
                                {23}
                                

                                --union
								--(select cn.Notes + ' - ' + emp.FirstName + ' ' + emp.LastName as EventTitle,
        --                        ISNULL(cn.ReminderDate, '1900-01-01 00:00:00') as EventStartDate,
        --                        ISNULL(cn.ReminderEndDate, '1900-01-01 00:00:00') as EventEndDate,
        --                        'Reminder' as EventType,
        --                        '' as EventDisplayType,
        --                        cn.Id as EventLeadId,
        --                        c.CustomerId as EventCusId,
        --                        '00000000-0000-0000-0000-000000000000' as EventCustomId,
								--'00000000-0000-0000-0000-000000000000' as EventAppid,
        --                        iif((select (select CHARINDEX('technician', Tag) from PermissionGroup _pgroup where _pgroup.Id = _up.PermissionGroupId) from UserPermission _up where _up.PermissionGroupId is not null and _up.UserId = emp.Userid) > 0, emp.FirstName + ' ' + emp.LastName + ' (T)', iif(emp.UserId = '22222222-2222-2222-2222-222222222222', emp.FirstName + ' ' + emp.LastName + ' (T)', emp.FirstName + ' ' + emp.LastName)) as EventResourceName,
        --                        emp.IsCalendar as EventIsCalendar,
								--emp.CalendarColor as EventColor,
        --                        emp.id as EventId,
        --                        c.FirstName + ' ' + c.LastName as EventCustomerName,
        --                        cn.IsAllDay as EventAllDay,
								--cc.IsLead as EventIsLead,
        --                        c.Street as EventStreet,
								--IIF(c.City != '', c.City + ', ', '') + c.State + c.ZipCode as EventLocate,
        --                        '' as EventDate,
        --                        '' as EventCalendarCount,
        --                        '00000000-0000-0000-0000-000000000000' as EventTicketId
        --                        ,'' as EventStatus
        --                        ,'' as HoverTitle
                                
        --                        ,c.DBA as EventDBA
        --                        ,'' as EventBookingId
        --                        ,'' as EventIcon
        --                        ,0 as EventRescheduleId
                                
								--from  CustomerNote cn
								--left join Customer c on c.CustomerId =cn.CustomerId
        --                        left join CustomerCompany cc on cc.CustomerId = c.CustomerId
        --                        left join NoteAssign na on na.NoteId = cn.Id
								--left join Employee emp on emp.UserId = na.EmployeeId
								--where cn.IsClose = 0 and cn.IsShedule = 1
                                
        --                        and c.IsActive = 1
        --                        and cc.CompanyId = @CompanyId
        --                        and emp.IsCalendar is not null
        --                        and emp.IsCalendar = 1
        --                        and emp.Recruited = 1
        --                        and emp.IsActive = 1
        --                        and emp.IsDeleted = 0
        --                        )
                                
								union
								(select 
								'' as EventTitle,
								'1900-01-01 00:00:00.000' as EventStartDate,
								'1900-01-01 00:00:00.000' as EventEndDate,
								'' as EventType,
                                '' as EventDisplayType,
								'' as EventLeadId,
                                '00000000-0000-0000-0000-000000000000' as EventCusId,
                                emp.UserId as EventCustomId,
								'00000000-0000-0000-0000-000000000000' as EventAppid,
                                iif((select (select CHARINDEX('technician', Tag) from PermissionGroup _pgroup where _pgroup.Id = _up.PermissionGroupId) from UserPermission _up where _up.PermissionGroupId is not null and _up.UserId = emp.Userid) > 0, emp.FirstName + ' ' + emp.LastName + ' (T)', iif(emp.UserId = '22222222-2222-2222-2222-222222222222', emp.FirstName + ' ' + emp.LastName + ' (T)', emp.FirstName + ' ' + emp.LastName)) as EventResourceName,
								emp.IsCalendar as EventIsCalendar,
								emp.CalendarColor as EventColor,
                                emp.Id as EventId,
                                '' as EventCustomerName,
                                '' as EventBusinessName,
                                '' as EventAllDay,
								0 as EventIsLead,
                                '' as EventStreet,
								'' as EventLocate,
                                '' as EventDate,
                                '' as EventCalendarCount,
                                '00000000-0000-0000-0000-000000000000' as EventTicketId
                                ,'' as EventStatus
                                ,'' as HoverTitle
                                
                                ,'' as EventDBA
                                ,'' as EventBookingId
                                ,'' as EventIcon
                                ,0 as EventRescheduleId
                                {19}
								from Employee emp
								where emp.IsCalendar is not null
								and emp.IsCalendar = 1
                                and emp.Recruited = 1
                                and emp.IsActive = 1
                                and emp.IsDeleted = 0
                                {4})) schedule)

                                (select * into #calendarFilter
								from #calendar)
                                (select distinct(EventId) into #enventid_tab  from #calendar)
								{9}
								(select (select count(*) from #calendar) as EventTotalCount)
                                (select top(@pageSize) COUNT(*) as EventCounter from #calendarFilter where EventId not in (select top(@pagestart) EventId from #calendar))
                                (select emp.*, iif((select (select CHARINDEX('technician', Tag) from PermissionGroup _pgroup where _pgroup.Id = _up.PermissionGroupId) from UserPermission _up where _up.PermissionGroupId is not null and _up.UserId = emp.Userid) > 0, emp.FirstName + ' ' + emp.LastName + ' (T)', iif(emp.UserId = '22222222-2222-2222-2222-222222222222', emp.FirstName + ' ' + emp.LastName + ' (T)', emp.FirstName + ' ' + emp.LastName)) as EMPName from Employee emp where emp.IsCalendar is not null and emp.IsCalendar = 1 and emp.IsActive = 1 and emp.IsDeleted = 0 {4})
                                {25}
							    drop table #calendar
                                drop table #enventid_tab
                                drop table #calendarFilter";
            string subquery = "";
            string subquery1 = "";
            string subquery2 = "";
            string subquery3 = "";
            string subquery4 = "";
            string subquery5 = "";
            string subquery6 = "";
            string subquery7 = "";
            string datequery = "";
            string Isprimaryquery = "";
            string IdQuery = "";
            string DayViewDateQueryTicket = "";
            string DayViewDateQueryReminder = "";
            string EmptyAdditionalQuery = "";
            string DataAdditionalQuery = "";
            string AdditionalMemberQuery = "";
            string Additionalsubquery5 = "";
            string AdditionalSubquery = "";
            if ((string.IsNullOrWhiteSpace(emptag) || (emptag.ToLower().IndexOf("admin") == -1)
                && emptag.ToLower().IndexOf("hrmanager") == -1
                && emptag.ToLower().IndexOf("showallschedules") == -1))
            {
                if (empid != "00000000-0000-0000-0000-000000000000" && empid != "'00000000-0000-0000-0000-000000000000'")
                {
                    subquery = string.Format("and tu.UserId in ({0})", empid);
                    subquery1 = string.Format("and na.EmployeeId in ({0})", empid);
                    subquery2 = string.Format("and cat.EmployeeId in ({0})", empid);
                    subquery3 = string.Format("and emp.UserId in ({0})", empid);
                    AdditionalSubquery = string.Format("and ca.EmployeeId in ({0})", empid);
                }
            }
            else
            {
                subquery = "";
                subquery1 = "";
                subquery2 = "";
                subquery3 = "";
                AdditionalSubquery = "";
            }
            if (!string.IsNullOrWhiteSpace(defaultView) && defaultView == "Daily" && !string.IsNullOrWhiteSpace(startdate))
            {
                datequery = string.Format("and ca.AppointmentDate between '{0} 00:00:00.000' and '{0} 23:59:59.000'", startdate);
            }
            else if (!string.IsNullOrWhiteSpace(defaultView) && (defaultView == "Weekly" || defaultView == "List") && !string.IsNullOrWhiteSpace(startdate))
            {
                var datestart = Convert.ToDateTime(startdate);
                var dateend = Convert.ToDateTime(startdate).AddDays(6);
                datequery = string.Format("and ca.AppointmentDate between '{0} 00:00:00.000' and '{1} 23:59:59.000'", datestart.ToString("yyyy-MM-dd"), dateend.ToString("yyyy-MM-dd"));
            }
            else if (!string.IsNullOrWhiteSpace(defaultView) && defaultView == "Monthly" && !string.IsNullOrWhiteSpace(startdate))
            {
                var stardate = Convert.ToDateTime(startdate);
                var datestart = new DateTime(stardate.Year, stardate.Month, 1);
                var dateend = new DateTime(stardate.Year, stardate.Month, DateTime.DaysInMonth(stardate.Year, stardate.Month));
                datequery = string.Format("and ca.AppointmentDate between '{0} 00:00:00.000' and '{1} 23:59:59.000'", datestart.ToString("yyyy-MM-dd"), dateend.ToString("yyyy-MM-dd"));
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(defaultView) && defaultView == "Daily")
                {
                    datequery = string.Format("and ca.AppointmentDate between '{0} 00:00:00.000' and '{0} 23:59:59.000'", DateTime.Now.ToString("yyyy-MM-dd"));
                }
                else if (!string.IsNullOrWhiteSpace(defaultView) && (defaultView == "Weekly" || defaultView == "List"))
                {
                    if (string.IsNullOrWhiteSpace(startdate))
                    {
                        startdate = DateTime.Now.ToString();
                    }
                    var datestart = Convert.ToDateTime(startdate);
                    var dateend = Convert.ToDateTime(startdate).AddDays(6);
                    datequery = string.Format("and ca.AppointmentDate between '{0} 00:00:00.000' and '{1} 23:59:59.000'", datestart.ToString("yyyy-MM-dd"), dateend.ToString("yyyy-MM-dd"));
                }
                else if (!string.IsNullOrWhiteSpace(defaultView) && defaultView == "Monthly")
                {
                    var datestart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    var dateend = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
                    datequery = string.Format("and ca.AppointmentDate between '{0} 00:00:00.000' and '{1} 23:59:59.000'", datestart.ToString("yyyy-MM-dd"), dateend.ToString("yyyy-MM-dd"));
                }
            }

            if (!string.IsNullOrWhiteSpace(typeval) && typeval != "'null'" && typeval != "''")
            {
                subquery5 = string.Format("and ca.AppointmentType in ({0})", typeval);
                Additionalsubquery5 = string.Format("and ticket.TicketType in ({0})", typeval);
            }
            else
            {
                subquery5 = "";
                Additionalsubquery5 = "";
            }
            if (!string.IsNullOrWhiteSpace(defaultView) && (defaultView == "Monthly"))
            {
                subquery6 = "isnull(emp.CalendarColor, '0000FF') as EventColor";
            }
            else
            {
                subquery6 = "emp.CalendarColor as EventColor";
            }
            if (!string.IsNullOrWhiteSpace(TicketId))
            {
                IdQuery = string.Format("and ticket.TicketId = '{0}'", TicketId);
            }
            if (!string.IsNullOrWhiteSpace(defaultView) && defaultView == "Daily" && !string.IsNullOrWhiteSpace(EventUserId))
            {
                subquery4 = string.Format("(select * from #calendarFilter where EventId in (select top(@pageSize) * from #enventid_tab where EventId not in (select top(@pagestart) EventId from #calendarFilter group by EventId)) and {0} and EventCustomId = '{1}')", datequery, EventUserId);
                subquery7 = string.Format("(select EventCustomId as UserId from #calendarFilter where EventId in (select top(@pageSize) * from #enventid_tab where EventStartDate!='1900-01-01 00:00:00.000' and EventEndDate!='1900-01-01 00:00:00.000' and EventId not in (select top(@pagestart) EventId from #calendarFilter group by EventId)) and {0} and EventCustomId = '{1}')", datequery, EventUserId);
                EmptyAdditionalQuery = string.Format(",'' as EventAdditionalMember");
                DataAdditionalQuery = string.Format(",'Additional' as EventAdditionalMember");
                AdditionalMemberQuery = string.Format("union (select emp.FirstName + ' ' + emp.LastName as EventTitle, ISNULL(CONVERT(datetime, ca.AppointmentDate + ' ' + IIF(ca.AppointmentStartTime != '-1', ca.AppointmentStartTime, '00:00')), '1900-01-01 00:00:00') as EventStartDate,ISNULL(CONVERT(datetime, ca.AppointmentDate + ' ' + IIF(ca.AppointmentEndTime != '-1', ca.AppointmentEndTime, '00:00')), '1900-01-01 00:00:00') as EventEndDate,ticket.TicketType as EventType,lk.DisplayText as EventDisplayType,ticket.Id as EventLeadId,isnull(cus.CustomerId, '00000000-0000-0000-0000-000000000000') as EventCusId,emp.UserId as EventCustomId,ca.AppointmentId as EventAppid,iif((select(select CHARINDEX('technician', Tag) from PermissionGroup _pgroup where _pgroup.Id = _up.PermissionGroupId) from UserPermission _up where _up.PermissionGroupId is not null and _up.UserId = emp.Userid) > 0, emp.FirstName + ' ' + emp.LastName + ' (T)', iif(emp.UserId = '22222222-2222-2222-2222-222222222222', emp.FirstName + ' ' + emp.LastName + ' (T)', emp.FirstName + ' ' + emp.LastName)) as EventResourceName,emp.IsCalendar as EventIsCalendar,iif((select count(map.Id) from Permission per left join PermissionGroupMap map on map.PermissionId = per.Id and map.CompanyId = per.CompanyId where Name = 'CustomerTicketDispatch') > 0 and ticket.IsDispatch != null and ticket.IsDispatch = 0 and ticket.[Status] != 'Completed' and ticket.IsClosed = 0, 'FFFF00',iif((select AlterDisplayText from[LookUp] lkcolor where DataKey = 'TicketType' and lkcolor.DataValue = ticket.TicketType) != 'FFFFFF' and(select AlterDisplayText from[LookUp] lkcolor where DataKey = 'TicketType' and lkcolor.DataValue = ticket.TicketType) != '', (select AlterDisplayText from[LookUp] lkcolor where DataKey = 'TicketType' and lkcolor.DataValue = ticket.TicketType), 'ccc')) as EventColor,emp.Id as EventId,cus.FirstName + ' ' + cus.LastName as EventCustomerName,cus.BusinessName as EventBusinessName,ca.IsAllDay as EventAllDay,0 as EventIsLead,isnull((select Street from CustomerAddress where RefId = ticket.BookingId and CustomerId = ticket.CustomerId and AddressType = REPLACE(ticket.TicketType, ' ', '') + 'Location'), cus.Street) as EventStreet,isnull((select IIF(cus.City != '', cus.City + ', ', '') + cus.[State] + cus.ZipCode from CustomerAddress where RefId = ticket.BookingId and CustomerId = ticket.CustomerId and AddressType = REPLACE(ticket.TicketType, ' ', '') + 'Location'), IIF(cus.City != '', cus.City + ', ', '') + cus.[State] + cus.ZipCode) as EventLocate,convert(nvarchar(50), ca.AppointmentDate, 23) as EventDate,'' as EventCalendarCount,ticket.TicketId as EventTicketId,ticket.[Status] as EventStatus,'' as HoverTitle,cus.DBA as EventDBA,ticket.BookingId as EventBookingId,imgset.[Filename] as EventIcon,ISNULL(RescheduleTicketId, 0) as EventRescheduleId {0} from AdditionalMembersAppointment ca left join Customer cus on cus.CustomerId = ca.CustomerId left join TicketUser tu on tu.UserId = ca.EmployeeId and tu.IsPrimary = 0 left join Ticket ticket on ticket.TicketId = ca.AppointmentId left join Employee emp on emp.UserId = tu.UserId left join[Lookup] lk on lk.DataValue = iif(ticket.TicketType != '-1', ticket.TicketType, '') and lk.DataKey = 'TicketType' left Join TicketStatusImageSetting imgset on imgset.TicketStatus = ticket.[Status] where ca.CompanyId = @CompanyId and tu.NotificationOnly = 0 {1} {2} and cus.IsActive = 1 and cus.CustomerId is not null and emp.IsCalendar is not null and emp.IsCalendar = 1 and lk.AlterDisplayText1 is not null and lk.AlterDisplayText1 != '' and lk.AlterDisplayText1 = 'True' and emp.Recruited = 1 and emp.IsActive = 1 and emp.IsDeleted = 0 and ticket.TicketId is not null {3} {4} {5})", DataAdditionalQuery, Additionalsubquery5, IdQuery, datequery, AdditionalSubquery, LostStatusQuery);
            }
            else if (!string.IsNullOrWhiteSpace(defaultView) && defaultView == "Daily")
            {
                subquery4 = "(select * from #calendarFilter where EventId in (select top(@pageSize) * from #enventid_tab where EventId not in (select top(@pagestart) EventId from #calendarFilter group by EventId)))";
                subquery7 = "(select EventCustomId as UserId from #calendarFilter where EventId in (select top(@pageSize) * from #enventid_tab where EventStartDate!='1900-01-01 00:00:00.000' and EventEndDate!='1900-01-01 00:00:00.000' and EventId not in (select top(@pagestart) EventId from #calendarFilter group by EventId)))";
                EmptyAdditionalQuery = string.Format(",'' as EventAdditionalMember");
                DataAdditionalQuery = string.Format(",'Additional' as EventAdditionalMember");
                AdditionalMemberQuery = string.Format("union (select emp.FirstName + ' ' + emp.LastName as EventTitle, ISNULL(CONVERT(datetime, ca.AppointmentDate + ' ' + IIF(ca.AppointmentStartTime != '-1', ca.AppointmentStartTime, '00:00')), '1900-01-01 00:00:00') as EventStartDate,ISNULL(CONVERT(datetime, ca.AppointmentDate + ' ' + IIF(ca.AppointmentEndTime != '-1', ca.AppointmentEndTime, '00:00')), '1900-01-01 00:00:00') as EventEndDate,ticket.TicketType as EventType,lk.DisplayText as EventDisplayType,ticket.Id as EventLeadId,isnull(cus.CustomerId, '00000000-0000-0000-0000-000000000000') as EventCusId,emp.UserId as EventCustomId,ca.AppointmentId as EventAppid,iif((select(select CHARINDEX('technician', Tag) from PermissionGroup _pgroup where _pgroup.Id = _up.PermissionGroupId) from UserPermission _up where _up.PermissionGroupId is not null and _up.UserId = emp.Userid) > 0, emp.FirstName + ' ' + emp.LastName + ' (T)', iif(emp.UserId = '22222222-2222-2222-2222-222222222222', emp.FirstName + ' ' + emp.LastName + ' (T)', emp.FirstName + ' ' + emp.LastName)) as EventResourceName,emp.IsCalendar as EventIsCalendar,iif((select count(map.Id) from Permission per left join PermissionGroupMap map on map.PermissionId = per.Id and map.CompanyId = per.CompanyId where Name = 'CustomerTicketDispatch') > 0 and ticket.IsDispatch != null and ticket.IsDispatch = 0 and ticket.[Status] != 'Completed' and ticket.IsClosed = 0, 'FFFF00',iif((select AlterDisplayText from[LookUp] lkcolor where DataKey = 'TicketType' and lkcolor.DataValue = ticket.TicketType) != 'FFFFFF' and(select AlterDisplayText from[LookUp] lkcolor where DataKey = 'TicketType' and lkcolor.DataValue = ticket.TicketType) != '', (select AlterDisplayText from[LookUp] lkcolor where DataKey = 'TicketType' and lkcolor.DataValue = ticket.TicketType), 'ccc')) as EventColor,emp.Id as EventId,cus.FirstName + ' ' + cus.LastName as EventCustomerName,cus.BusinessName as EventBusinessName,ca.IsAllDay as EventAllDay,0 as EventIsLead,isnull((select Street from CustomerAddress where RefId = ticket.BookingId and CustomerId = ticket.CustomerId and AddressType = REPLACE(ticket.TicketType, ' ', '') + 'Location'), cus.Street) as EventStreet,isnull((select IIF(cus.City != '', cus.City + ', ', '') + cus.[State] + cus.ZipCode from CustomerAddress where RefId = ticket.BookingId and CustomerId = ticket.CustomerId and AddressType = REPLACE(ticket.TicketType, ' ', '') + 'Location'), IIF(cus.City != '', cus.City + ', ', '') + cus.[State] + cus.ZipCode) as EventLocate,convert(nvarchar(50), ca.AppointmentDate, 23) as EventDate,'' as EventCalendarCount,ticket.TicketId as EventTicketId,ticket.[Status] as EventStatus,'' as HoverTitle,cus.DBA as EventDBA,ticket.BookingId as EventBookingId,imgset.[Filename] as EventIcon,ISNULL(RescheduleTicketId, 0) as EventRescheduleId {0} from AdditionalMembersAppointment ca left join Customer cus on cus.CustomerId = ca.CustomerId left join TicketUser tu on tu.UserId = ca.EmployeeId and tu.IsPrimary = 0 left join Ticket ticket on ticket.TicketId = ca.AppointmentId left join Employee emp on emp.UserId = tu.UserId left join[Lookup] lk on lk.DataValue = iif(ticket.TicketType != '-1', ticket.TicketType, '') and lk.DataKey = 'TicketType' left Join TicketStatusImageSetting imgset on imgset.TicketStatus = ticket.[Status] where ca.CompanyId = @CompanyId and tu.NotificationOnly = 0 {1} {2} and cus.IsActive = 1 and cus.CustomerId is not null and emp.IsCalendar is not null and emp.IsCalendar = 1 and lk.AlterDisplayText1 is not null and lk.AlterDisplayText1 != '' and lk.AlterDisplayText1 = 'True' and emp.Recruited = 1 and emp.IsActive = 1 and emp.IsDeleted = 0 and ticket.TicketId is not null {3} {4} {5})", DataAdditionalQuery, Additionalsubquery5, IdQuery, datequery, AdditionalSubquery, LostStatusQuery);
            }
            else if (!string.IsNullOrWhiteSpace(defaultView) && defaultView == "Monthly")
            {
                Isprimaryquery = string.Format("and tu.IsPrimary = 1");
                subquery4 = "(select '' as HoverTitle, (select COUNT(*) from TicketUser where TiketId = EventTicketId and NotificationOnly = 0) as EventCalendarCount, '' as EventTitle,'1900-01-01 00:00:00.000' as EventStartDate,'1900-01-01 00:00:00.000' as EventEndDate,EventType,0 as EventLeadId,'00000000-0000-0000-0000-000000000000' as EventCusId,STUFF((SELECT  ',' + convert(nvarchar(MAX), _user.UserId) FROM TicketUser _user WHERE _user.TiketId = EventTicketId and _user.NotificationOnly = 0 FOR XML PATH('')), 1, 1, '') as EventCustomId,'00000000-0000-0000-0000-000000000000' as EventAppid,EventResourceName,EventIsCalendar,'' as EventColor,0 as EventId,'' as EventCustomerName,'' as EventBusinessName,'' as EventDBA, '' as EventBookingId,0 as EventAllDay,EventIsLead,'' as EventStreet,'' as EventLocate,EventDate, EventTicketId, EventStatus, EventDisplayType,'' as EventIcon, 0 as EventRescheduleId,'' as EventAdditionalMember from #calendarFilter group by EventCustomId, EventDate, EventResourceName,EventIsCalendar, EventIsLead, EventType, EventDisplayType, EventTicketId, EventStatus)";
                subquery7 = "(select EventCustomId as UserId from #calendarFilter where EventStartDate!='1900-01-01 00:00:00.000' and EventEndDate!='1900-01-01 00:00:00.000')";
            }
            else
            {
                subquery4 = "(select * from #calendarFilter)";
                subquery7 = "(select EventCustomId as UserId from #calendarFilter where EventStartDate!='1900-01-01 00:00:00.000' and EventEndDate!='1900-01-01 00:00:00.000')";
                EmptyAdditionalQuery = string.Format(",'' as EventAdditionalMember");
                DataAdditionalQuery = string.Format(",'Additional' as EventAdditionalMember");
                AdditionalMemberQuery = string.Format("union (select emp.FirstName + ' ' + emp.LastName as EventTitle, ISNULL(CONVERT(datetime, ca.AppointmentDate + ' ' + IIF(ca.AppointmentStartTime != '-1', ca.AppointmentStartTime, '00:00')), '1900-01-01 00:00:00') as EventStartDate,ISNULL(CONVERT(datetime, ca.AppointmentDate + ' ' + IIF(ca.AppointmentEndTime != '-1', ca.AppointmentEndTime, '00:00')), '1900-01-01 00:00:00') as EventEndDate,ticket.TicketType as EventType,lk.DisplayText as EventDisplayType,ticket.Id as EventLeadId,isnull(cus.CustomerId, '00000000-0000-0000-0000-000000000000') as EventCusId,emp.UserId as EventCustomId,ca.AppointmentId as EventAppid,iif((select(select CHARINDEX('technician', Tag) from PermissionGroup _pgroup where _pgroup.Id = _up.PermissionGroupId) from UserPermission _up where _up.PermissionGroupId is not null and _up.UserId = emp.Userid) > 0, emp.FirstName + ' ' + emp.LastName + ' (T)', iif(emp.UserId = '22222222-2222-2222-2222-222222222222', emp.FirstName + ' ' + emp.LastName + ' (T)', emp.FirstName + ' ' + emp.LastName)) as EventResourceName,emp.IsCalendar as EventIsCalendar,iif((select count(map.Id) from Permission per left join PermissionGroupMap map on map.PermissionId = per.Id and map.CompanyId = per.CompanyId where Name = 'CustomerTicketDispatch') > 0 and ticket.IsDispatch != null and ticket.IsDispatch = 0 and ticket.[Status] != 'Completed' and ticket.IsClosed = 0, 'FFFF00',iif((select AlterDisplayText from[LookUp] lkcolor where DataKey = 'TicketType' and lkcolor.DataValue = ticket.TicketType) != 'FFFFFF' and(select AlterDisplayText from[LookUp] lkcolor where DataKey = 'TicketType' and lkcolor.DataValue = ticket.TicketType) != '', (select AlterDisplayText from[LookUp] lkcolor where DataKey = 'TicketType' and lkcolor.DataValue = ticket.TicketType), 'ccc')) as EventColor,emp.Id as EventId,cus.FirstName + ' ' + cus.LastName as EventCustomerName,cus.BusinessName as EventBusinessName,ca.IsAllDay as EventAllDay,0 as EventIsLead,isnull((select Street from CustomerAddress where RefId = ticket.BookingId and CustomerId = ticket.CustomerId and AddressType = REPLACE(ticket.TicketType, ' ', '') + 'Location'), cus.Street) as EventStreet,isnull((select IIF(cus.City != '', cus.City + ', ', '') + cus.[State] + cus.ZipCode from CustomerAddress where RefId = ticket.BookingId and CustomerId = ticket.CustomerId and AddressType = REPLACE(ticket.TicketType, ' ', '') + 'Location'), IIF(cus.City != '', cus.City + ', ', '') + cus.[State] + cus.ZipCode) as EventLocate,convert(nvarchar(50), ca.AppointmentDate, 23) as EventDate,'' as EventCalendarCount,ticket.TicketId as EventTicketId,ticket.[Status] as EventStatus,'' as HoverTitle,cus.DBA as EventDBA,ticket.BookingId as EventBookingId,imgset.[Filename] as EventIcon,ISNULL(RescheduleTicketId, 0) as EventRescheduleId {0} from AdditionalMembersAppointment ca left join Customer cus on cus.CustomerId = ca.CustomerId left join TicketUser tu on tu.UserId = ca.EmployeeId and tu.IsPrimary = 0 left join Ticket ticket on ticket.TicketId = ca.AppointmentId left join Employee emp on emp.UserId = tu.UserId left join[Lookup] lk on lk.DataValue = iif(ticket.TicketType != '-1', ticket.TicketType, '') and lk.DataKey = 'TicketType' left Join TicketStatusImageSetting imgset on imgset.TicketStatus = ticket.[Status] where ca.CompanyId = @CompanyId and tu.NotificationOnly = 0 {1} {2} and cus.IsActive = 1 and cus.CustomerId is not null and emp.IsCalendar is not null and emp.IsCalendar = 1 and lk.AlterDisplayText1 is not null and lk.AlterDisplayText1 != '' and lk.AlterDisplayText1 = 'True' and emp.Recruited = 1 and emp.IsActive = 1 and emp.IsDeleted = 0 and ticket.TicketId is not null {3} {4} {5})", DataAdditionalQuery, Additionalsubquery5, IdQuery, datequery, AdditionalSubquery, LostStatusQuery);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, companyID//0
                    , subquery//1
                    , subquery1//2
                    , subquery2//3
                    , subquery3//4
                    , ResourceLimit//5
                    , pageno//6
                    , ReminderResult//7
                    , pageno1//8
                    , subquery4//9
                    , startdate//10
                    , typeval//11
                    , subquery5//12
                    , subquery6//13
                    , datequery//14
                    , Isprimaryquery//15
                    , IdQuery//16
                    , DayViewDateQueryTicket//17
                    , DayViewDateQueryReminder//18
                    , EmptyAdditionalQuery//19
                    , DataAdditionalQuery//20
                    , AdditionalSubquery//21
                    , Additionalsubquery5//22
                    , AdditionalMemberQuery//23
                    , LostStatusQuery //24
                    , subquery7//25
                    );
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
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

        public DataSet GetUserListByCompanyIdHaveEvent(Guid companyID, string startdate, string defaultView)
        {
            string sqlQuery = @"
                                select distinct emp.UserId
                                from CustomerAppointment ca
								left join Customer cus
                                on cus.CustomerId=ca.CustomerId
								left join TicketUser tu on   tu.TiketId =ca.AppointmentId  and tu.IsPrimary = 1
                                left join Ticket ticket on ticket.TicketId = tu.TiketId
								left join Employee emp on  emp.UserId =tu.UserId
                                where ca.CompanyId = '{0}'
                                and tu.NotificationOnly = 0
                                and cus.IsActive =1
                                and emp.IsCalendar is not null
                                and emp.IsCalendar = 1
                                and emp.Recruited = 1
                                and emp.IsActive = 1
                                and emp.IsDeleted = 0
                                and ticket.TicketId is not null
                                and cus.CustomerId is not null
                                 {1}";
            //string subquery = "";
            //string subquery1 = "";
            //string subquery2 = "";
            //string subquery3 = "";
            //string subquery4 = "";
            //string subquery5 = "";
            //string subquery6 = "";
            //string subquery7 = "";
            string datequery = "";
            //string Isprimaryquery = "";
            //string IdQuery = "";
            //string DayViewDateQueryTicket = "";
            //string DayViewDateQueryReminder = "";
            //string EmptyAdditionalQuery = "";
            //string DataAdditionalQuery = "";
            //string AdditionalMemberQuery = "";
            //string Additionalsubquery5 = "";
            //string AdditionalSubquery = "";
            //if ((string.IsNullOrWhiteSpace(emptag) || (emptag.ToLower().IndexOf("admin") == -1)
            //    && emptag.ToLower().IndexOf("hrmanager") == -1
            //    && emptag.ToLower().IndexOf("showallschedules") == -1))
            //{
            //    if (empid != "00000000-0000-0000-0000-000000000000" && empid != "'00000000-0000-0000-0000-000000000000'")
            //    {
            //        subquery = string.Format("and tu.UserId in ({0})", empid);
            //        subquery1 = string.Format("and na.EmployeeId in ({0})", empid);
            //        subquery2 = string.Format("and cat.EmployeeId in ({0})", empid);
            //        subquery3 = string.Format("and emp.UserId in ({0})", empid);
            //        AdditionalSubquery = string.Format("and ca.EmployeeId in ({0})", empid);
            //    }
            //}
            //else
            //{
            //    subquery = "";
            //    subquery1 = "";
            //    subquery2 = "";
            //    subquery3 = "";
            //    AdditionalSubquery = "";
            //}
            if (!string.IsNullOrWhiteSpace(defaultView) && defaultView == "Daily" && !string.IsNullOrWhiteSpace(startdate))
            {
                datequery = string.Format("and ca.AppointmentDate between '{0} 00:00:00.000' and '{0} 23:59:59.000'", startdate);
            }
            else if (!string.IsNullOrWhiteSpace(defaultView) && (defaultView == "Weekly" || defaultView == "List") && !string.IsNullOrWhiteSpace(startdate))
            {
                var datestart = Convert.ToDateTime(startdate);
                var dateend = Convert.ToDateTime(startdate).AddDays(6);
                datequery = string.Format("and ca.AppointmentDate between '{0} 00:00:00.000' and '{1} 23:59:59.000'", datestart.ToString("yyyy-MM-dd"), dateend.ToString("yyyy-MM-dd"));
            }
            else if (!string.IsNullOrWhiteSpace(defaultView) && defaultView == "Monthly" && !string.IsNullOrWhiteSpace(startdate))
            {
                var stardate = Convert.ToDateTime(startdate);
                var datestart = new DateTime(stardate.Year, stardate.Month, 1);
                var dateend = new DateTime(stardate.Year, stardate.Month, DateTime.DaysInMonth(stardate.Year, stardate.Month));
                datequery = string.Format("and ca.AppointmentDate between '{0} 00:00:00.000' and '{1} 23:59:59.000'", datestart.ToString("yyyy-MM-dd"), dateend.ToString("yyyy-MM-dd"));
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(defaultView) && defaultView == "Daily")
                {
                    datequery = string.Format("and ca.AppointmentDate between '{0} 00:00:00.000' and '{0} 23:59:59.000'", DateTime.Now.ToString("yyyy-MM-dd"));
                }
                else if (!string.IsNullOrWhiteSpace(defaultView) && (defaultView == "Weekly" || defaultView == "List"))
                {
                    if (string.IsNullOrWhiteSpace(startdate))
                    {
                        startdate = DateTime.Now.ToString();
                    }
                    var datestart = Convert.ToDateTime(startdate);
                    var dateend = Convert.ToDateTime(startdate).AddDays(6);
                    datequery = string.Format("and ca.AppointmentDate between '{0} 00:00:00.000' and '{1} 23:59:59.000'", datestart.ToString("yyyy-MM-dd"), dateend.ToString("yyyy-MM-dd"));
                }
                else if (!string.IsNullOrWhiteSpace(defaultView) && defaultView == "Monthly")
                {
                    var datestart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    var dateend = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
                    datequery = string.Format("and ca.AppointmentDate between '{0} 00:00:00.000' and '{1} 23:59:59.000'", datestart.ToString("yyyy-MM-dd"), dateend.ToString("yyyy-MM-dd"));
                }
            }

            //if (!string.IsNullOrWhiteSpace(typeval) && typeval != "'null'" && typeval != "''")
            //{
            //    subquery5 = string.Format("and ca.AppointmentType in ({0})", typeval);
            //    Additionalsubquery5 = string.Format("and ticket.TicketType in ({0})", typeval);
            //}
            //else
            //{
            //    subquery5 = "";
            //    Additionalsubquery5 = "";
            //}
            //if (!string.IsNullOrWhiteSpace(defaultView) && (defaultView == "Monthly"))
            //{
            //    subquery6 = "isnull(emp.CalendarColor, '0000FF') as EventColor";
            //}
            //else
            //{
            //    subquery6 = "emp.CalendarColor as EventColor";
            //}
            //if (!string.IsNullOrWhiteSpace(TicketId))
            //{
            //    IdQuery = string.Format("and ticket.TicketId = '{0}'", TicketId);
            //}
            //if (!string.IsNullOrWhiteSpace(defaultView) && defaultView == "Daily" && !string.IsNullOrWhiteSpace(EventUserId))
            //{
            //    subquery4 = string.Format("(select * from #calendarFilter where EventId in (select top(@pageSize) * from #enventid_tab where EventId not in (select top(@pagestart) EventId from #calendarFilter group by EventId)) and {0} and EventCustomId = '{1}')", datequery, EventUserId);
            //    subquery7 = string.Format("(select EventCustomId as UserId from #calendarFilter where EventId in (select top(@pageSize) * from #enventid_tab where EventStartDate!='1900-01-01 00:00:00.000' and EventEndDate!='1900-01-01 00:00:00.000' and EventId not in (select top(@pagestart) EventId from #calendarFilter group by EventId)) and {0} and EventCustomId = '{1}')", datequery, EventUserId);
            //    EmptyAdditionalQuery = string.Format(",'' as EventAdditionalMember");
            //    DataAdditionalQuery = string.Format(",'Additional' as EventAdditionalMember");
            //    AdditionalMemberQuery = string.Format("union (select emp.FirstName + ' ' + emp.LastName as EventTitle, ISNULL(CONVERT(datetime, ca.AppointmentDate + ' ' + IIF(ca.AppointmentStartTime != '-1', ca.AppointmentStartTime, '00:00')), '1900-01-01 00:00:00') as EventStartDate,ISNULL(CONVERT(datetime, ca.AppointmentDate + ' ' + IIF(ca.AppointmentEndTime != '-1', ca.AppointmentEndTime, '00:00')), '1900-01-01 00:00:00') as EventEndDate,ticket.TicketType as EventType,lk.DisplayText as EventDisplayType,ticket.Id as EventLeadId,isnull(cus.CustomerId, '00000000-0000-0000-0000-000000000000') as EventCusId,emp.UserId as EventCustomId,ca.AppointmentId as EventAppid,iif((select(select CHARINDEX('technician', Tag) from PermissionGroup _pgroup where _pgroup.Id = _up.PermissionGroupId) from UserPermission _up where _up.PermissionGroupId is not null and _up.UserId = emp.Userid) > 0, emp.FirstName + ' ' + emp.LastName + ' (T)', iif(emp.UserId = '22222222-2222-2222-2222-222222222222', emp.FirstName + ' ' + emp.LastName + ' (T)', emp.FirstName + ' ' + emp.LastName)) as EventResourceName,emp.IsCalendar as EventIsCalendar,iif((select count(map.Id) from Permission per left join PermissionGroupMap map on map.PermissionId = per.Id and map.CompanyId = per.CompanyId where Name = 'CustomerTicketDispatch') > 0 and ticket.IsDispatch != null and ticket.IsDispatch = 0 and ticket.[Status] != 'Completed' and ticket.IsClosed = 0, 'FFFF00',iif((select AlterDisplayText from[LookUp] lkcolor where DataKey = 'TicketType' and lkcolor.DataValue = ticket.TicketType) != 'FFFFFF' and(select AlterDisplayText from[LookUp] lkcolor where DataKey = 'TicketType' and lkcolor.DataValue = ticket.TicketType) != '', (select AlterDisplayText from[LookUp] lkcolor where DataKey = 'TicketType' and lkcolor.DataValue = ticket.TicketType), 'ccc')) as EventColor,emp.Id as EventId,cus.FirstName + ' ' + cus.LastName as EventCustomerName,cus.BusinessName as EventBusinessName,ca.IsAllDay as EventAllDay,0 as EventIsLead,isnull((select Street from CustomerAddress where RefId = ticket.BookingId and CustomerId = ticket.CustomerId and AddressType = REPLACE(ticket.TicketType, ' ', '') + 'Location'), cus.Street) as EventStreet,isnull((select IIF(cus.City != '', cus.City + ', ', '') + cus.[State] + cus.ZipCode from CustomerAddress where RefId = ticket.BookingId and CustomerId = ticket.CustomerId and AddressType = REPLACE(ticket.TicketType, ' ', '') + 'Location'), IIF(cus.City != '', cus.City + ', ', '') + cus.[State] + cus.ZipCode) as EventLocate,convert(nvarchar(50), ca.AppointmentDate, 23) as EventDate,'' as EventCalendarCount,ticket.TicketId as EventTicketId,ticket.[Status] as EventStatus,'' as HoverTitle,cus.DBA as EventDBA,ticket.BookingId as EventBookingId,imgset.[Filename] as EventIcon,ISNULL(RescheduleTicketId, 0) as EventRescheduleId {0} from AdditionalMembersAppointment ca left join Customer cus on cus.CustomerId = ca.CustomerId left join TicketUser tu on tu.UserId = ca.EmployeeId and tu.IsPrimary = 0 left join Ticket ticket on ticket.TicketId = ca.AppointmentId left join Employee emp on emp.UserId = tu.UserId left join[Lookup] lk on lk.DataValue = iif(ticket.TicketType != '-1', ticket.TicketType, '') and lk.DataKey = 'TicketType' left Join TicketStatusImageSetting imgset on imgset.TicketStatus = ticket.[Status] where ca.CompanyId = @CompanyId and tu.NotificationOnly = 0 {1} {2} and cus.IsActive = 1 and cus.CustomerId is not null and emp.IsCalendar is not null and emp.IsCalendar = 1 and lk.AlterDisplayText1 is not null and lk.AlterDisplayText1 != '' and lk.AlterDisplayText1 = 'True' and emp.Recruited = 1 and emp.IsActive = 1 and emp.IsDeleted = 0 and ticket.TicketId is not null {3} {4} {5})", DataAdditionalQuery, Additionalsubquery5, IdQuery, datequery, AdditionalSubquery, LostStatusQuery);
            //}
            //else if (!string.IsNullOrWhiteSpace(defaultView) && defaultView == "Daily")
            //{
            //    subquery4 = "(select * from #calendarFilter where EventId in (select top(@pageSize) * from #enventid_tab where EventId not in (select top(@pagestart) EventId from #calendarFilter group by EventId)))";
            //    subquery7 = "(select EventCustomId as UserId from #calendarFilter where EventId in (select top(@pageSize) * from #enventid_tab where EventStartDate!='1900-01-01 00:00:00.000' and EventEndDate!='1900-01-01 00:00:00.000' and EventId not in (select top(@pagestart) EventId from #calendarFilter group by EventId)))";
            //    EmptyAdditionalQuery = string.Format(",'' as EventAdditionalMember");
            //    DataAdditionalQuery = string.Format(",'Additional' as EventAdditionalMember");
            //    AdditionalMemberQuery = string.Format("union (select emp.FirstName + ' ' + emp.LastName as EventTitle, ISNULL(CONVERT(datetime, ca.AppointmentDate + ' ' + IIF(ca.AppointmentStartTime != '-1', ca.AppointmentStartTime, '00:00')), '1900-01-01 00:00:00') as EventStartDate,ISNULL(CONVERT(datetime, ca.AppointmentDate + ' ' + IIF(ca.AppointmentEndTime != '-1', ca.AppointmentEndTime, '00:00')), '1900-01-01 00:00:00') as EventEndDate,ticket.TicketType as EventType,lk.DisplayText as EventDisplayType,ticket.Id as EventLeadId,isnull(cus.CustomerId, '00000000-0000-0000-0000-000000000000') as EventCusId,emp.UserId as EventCustomId,ca.AppointmentId as EventAppid,iif((select(select CHARINDEX('technician', Tag) from PermissionGroup _pgroup where _pgroup.Id = _up.PermissionGroupId) from UserPermission _up where _up.PermissionGroupId is not null and _up.UserId = emp.Userid) > 0, emp.FirstName + ' ' + emp.LastName + ' (T)', iif(emp.UserId = '22222222-2222-2222-2222-222222222222', emp.FirstName + ' ' + emp.LastName + ' (T)', emp.FirstName + ' ' + emp.LastName)) as EventResourceName,emp.IsCalendar as EventIsCalendar,iif((select count(map.Id) from Permission per left join PermissionGroupMap map on map.PermissionId = per.Id and map.CompanyId = per.CompanyId where Name = 'CustomerTicketDispatch') > 0 and ticket.IsDispatch != null and ticket.IsDispatch = 0 and ticket.[Status] != 'Completed' and ticket.IsClosed = 0, 'FFFF00',iif((select AlterDisplayText from[LookUp] lkcolor where DataKey = 'TicketType' and lkcolor.DataValue = ticket.TicketType) != 'FFFFFF' and(select AlterDisplayText from[LookUp] lkcolor where DataKey = 'TicketType' and lkcolor.DataValue = ticket.TicketType) != '', (select AlterDisplayText from[LookUp] lkcolor where DataKey = 'TicketType' and lkcolor.DataValue = ticket.TicketType), 'ccc')) as EventColor,emp.Id as EventId,cus.FirstName + ' ' + cus.LastName as EventCustomerName,cus.BusinessName as EventBusinessName,ca.IsAllDay as EventAllDay,0 as EventIsLead,isnull((select Street from CustomerAddress where RefId = ticket.BookingId and CustomerId = ticket.CustomerId and AddressType = REPLACE(ticket.TicketType, ' ', '') + 'Location'), cus.Street) as EventStreet,isnull((select IIF(cus.City != '', cus.City + ', ', '') + cus.[State] + cus.ZipCode from CustomerAddress where RefId = ticket.BookingId and CustomerId = ticket.CustomerId and AddressType = REPLACE(ticket.TicketType, ' ', '') + 'Location'), IIF(cus.City != '', cus.City + ', ', '') + cus.[State] + cus.ZipCode) as EventLocate,convert(nvarchar(50), ca.AppointmentDate, 23) as EventDate,'' as EventCalendarCount,ticket.TicketId as EventTicketId,ticket.[Status] as EventStatus,'' as HoverTitle,cus.DBA as EventDBA,ticket.BookingId as EventBookingId,imgset.[Filename] as EventIcon,ISNULL(RescheduleTicketId, 0) as EventRescheduleId {0} from AdditionalMembersAppointment ca left join Customer cus on cus.CustomerId = ca.CustomerId left join TicketUser tu on tu.UserId = ca.EmployeeId and tu.IsPrimary = 0 left join Ticket ticket on ticket.TicketId = ca.AppointmentId left join Employee emp on emp.UserId = tu.UserId left join[Lookup] lk on lk.DataValue = iif(ticket.TicketType != '-1', ticket.TicketType, '') and lk.DataKey = 'TicketType' left Join TicketStatusImageSetting imgset on imgset.TicketStatus = ticket.[Status] where ca.CompanyId = @CompanyId and tu.NotificationOnly = 0 {1} {2} and cus.IsActive = 1 and cus.CustomerId is not null and emp.IsCalendar is not null and emp.IsCalendar = 1 and lk.AlterDisplayText1 is not null and lk.AlterDisplayText1 != '' and lk.AlterDisplayText1 = 'True' and emp.Recruited = 1 and emp.IsActive = 1 and emp.IsDeleted = 0 and ticket.TicketId is not null {3} {4} {5})", DataAdditionalQuery, Additionalsubquery5, IdQuery, datequery, AdditionalSubquery, LostStatusQuery);
            //}
            //else if (!string.IsNullOrWhiteSpace(defaultView) && defaultView == "Monthly")
            //{
            //    Isprimaryquery = string.Format("and tu.IsPrimary = 1");
            //    subquery4 = "(select '' as HoverTitle, (select COUNT(*) from TicketUser where TiketId = EventTicketId and NotificationOnly = 0) as EventCalendarCount, '' as EventTitle,'1900-01-01 00:00:00.000' as EventStartDate,'1900-01-01 00:00:00.000' as EventEndDate,EventType,0 as EventLeadId,'00000000-0000-0000-0000-000000000000' as EventCusId,STUFF((SELECT  ',' + convert(nvarchar(MAX), _user.UserId) FROM TicketUser _user WHERE _user.TiketId = EventTicketId and _user.NotificationOnly = 0 FOR XML PATH('')), 1, 1, '') as EventCustomId,'00000000-0000-0000-0000-000000000000' as EventAppid,EventResourceName,EventIsCalendar,'' as EventColor,0 as EventId,'' as EventCustomerName,'' as EventBusinessName,'' as EventDBA, '' as EventBookingId,0 as EventAllDay,EventIsLead,'' as EventStreet,'' as EventLocate,EventDate, EventTicketId, '' as EventStatus, EventDisplayType,'' as EventIcon, 0 as EventRescheduleId,'' as EventAdditionalMember from #calendarFilter group by EventCustomId, EventDate, EventResourceName,EventIsCalendar, EventIsLead, EventType, EventDisplayType, EventTicketId)";
            //    subquery7 = "(select EventCustomId as UserId from #calendarFilter where EventStartDate!='1900-01-01 00:00:00.000' and EventEndDate!='1900-01-01 00:00:00.000')";
            //}
            //else
            //{
            //    subquery4 = "(select * from #calendarFilter)";
            //    subquery7 = "(select EventCustomId as UserId from #calendarFilter where EventStartDate!='1900-01-01 00:00:00.000' and EventEndDate!='1900-01-01 00:00:00.000')";
            //    EmptyAdditionalQuery = string.Format(",'' as EventAdditionalMember");
            //    DataAdditionalQuery = string.Format(",'Additional' as EventAdditionalMember");
            //    AdditionalMemberQuery = string.Format("union (select emp.FirstName + ' ' + emp.LastName as EventTitle, ISNULL(CONVERT(datetime, ca.AppointmentDate + ' ' + IIF(ca.AppointmentStartTime != '-1', ca.AppointmentStartTime, '00:00')), '1900-01-01 00:00:00') as EventStartDate,ISNULL(CONVERT(datetime, ca.AppointmentDate + ' ' + IIF(ca.AppointmentEndTime != '-1', ca.AppointmentEndTime, '00:00')), '1900-01-01 00:00:00') as EventEndDate,ticket.TicketType as EventType,lk.DisplayText as EventDisplayType,ticket.Id as EventLeadId,isnull(cus.CustomerId, '00000000-0000-0000-0000-000000000000') as EventCusId,emp.UserId as EventCustomId,ca.AppointmentId as EventAppid,iif((select(select CHARINDEX('technician', Tag) from PermissionGroup _pgroup where _pgroup.Id = _up.PermissionGroupId) from UserPermission _up where _up.PermissionGroupId is not null and _up.UserId = emp.Userid) > 0, emp.FirstName + ' ' + emp.LastName + ' (T)', iif(emp.UserId = '22222222-2222-2222-2222-222222222222', emp.FirstName + ' ' + emp.LastName + ' (T)', emp.FirstName + ' ' + emp.LastName)) as EventResourceName,emp.IsCalendar as EventIsCalendar,iif((select count(map.Id) from Permission per left join PermissionGroupMap map on map.PermissionId = per.Id and map.CompanyId = per.CompanyId where Name = 'CustomerTicketDispatch') > 0 and ticket.IsDispatch != null and ticket.IsDispatch = 0 and ticket.[Status] != 'Completed' and ticket.IsClosed = 0, 'FFFF00',iif((select AlterDisplayText from[LookUp] lkcolor where DataKey = 'TicketType' and lkcolor.DataValue = ticket.TicketType) != 'FFFFFF' and(select AlterDisplayText from[LookUp] lkcolor where DataKey = 'TicketType' and lkcolor.DataValue = ticket.TicketType) != '', (select AlterDisplayText from[LookUp] lkcolor where DataKey = 'TicketType' and lkcolor.DataValue = ticket.TicketType), 'ccc')) as EventColor,emp.Id as EventId,cus.FirstName + ' ' + cus.LastName as EventCustomerName,cus.BusinessName as EventBusinessName,ca.IsAllDay as EventAllDay,0 as EventIsLead,isnull((select Street from CustomerAddress where RefId = ticket.BookingId and CustomerId = ticket.CustomerId and AddressType = REPLACE(ticket.TicketType, ' ', '') + 'Location'), cus.Street) as EventStreet,isnull((select IIF(cus.City != '', cus.City + ', ', '') + cus.[State] + cus.ZipCode from CustomerAddress where RefId = ticket.BookingId and CustomerId = ticket.CustomerId and AddressType = REPLACE(ticket.TicketType, ' ', '') + 'Location'), IIF(cus.City != '', cus.City + ', ', '') + cus.[State] + cus.ZipCode) as EventLocate,convert(nvarchar(50), ca.AppointmentDate, 23) as EventDate,'' as EventCalendarCount,ticket.TicketId as EventTicketId,ticket.[Status] as EventStatus,'' as HoverTitle,cus.DBA as EventDBA,ticket.BookingId as EventBookingId,imgset.[Filename] as EventIcon,ISNULL(RescheduleTicketId, 0) as EventRescheduleId {0} from AdditionalMembersAppointment ca left join Customer cus on cus.CustomerId = ca.CustomerId left join TicketUser tu on tu.UserId = ca.EmployeeId and tu.IsPrimary = 0 left join Ticket ticket on ticket.TicketId = ca.AppointmentId left join Employee emp on emp.UserId = tu.UserId left join[Lookup] lk on lk.DataValue = iif(ticket.TicketType != '-1', ticket.TicketType, '') and lk.DataKey = 'TicketType' left Join TicketStatusImageSetting imgset on imgset.TicketStatus = ticket.[Status] where ca.CompanyId = @CompanyId and tu.NotificationOnly = 0 {1} {2} and cus.IsActive = 1 and cus.CustomerId is not null and emp.IsCalendar is not null and emp.IsCalendar = 1 and lk.AlterDisplayText1 is not null and lk.AlterDisplayText1 != '' and lk.AlterDisplayText1 = 'True' and emp.Recruited = 1 and emp.IsActive = 1 and emp.IsDeleted = 0 and ticket.TicketId is not null {3} {4} {5})", DataAdditionalQuery, Additionalsubquery5, IdQuery, datequery, AdditionalSubquery, LostStatusQuery);
            //}
            try
            {
                sqlQuery = string.Format(sqlQuery, companyID//0   
                    , datequery//14
                    );
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
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

        public DataTable GetPermissionScheduleListByCompanyId(Guid companyID, string PermissionRole)
        {
            string sqlQuery = @"select *  into #temUnion from (select 
                                s.Title as EventTitle,
                                s.StartDate as EventStartDate,
                                s.EndDate as EventEndDate,
                                s.Type as EventType,
                                s.LeadId as EventLeadId,
                                cus.CustomerId as EventCusId,
                                s.Identifier as EventCustomId,
                                ca.AppointmentId as EventAppid,
								pg.Name as EventPermissionName
                                from Schedule s
                                join Customer cus
                                on cus.Id=s.LeadId
								join CustomerAppointment ca
								on ca.CustomerId = cus.CustomerId
                                and ca.EmployeeId = s.Identifier
								join Employee emp
								on emp.EmployeeId = s.Identifier
								left join UserPermission _up
								on emp.UserName = _up.UserName
                                left join PermissionGroup pg
                                on _up.PermissionGroupId = pg.Id
                                left join UserCompany _uc
                                on _uc.UserName = _up.UserName
                                where s.IsCompleted = 0
                                and s.CompanyId = '{0}'
								and pg.Name is not null
								and pg.Name = '{1}'
                                and s.Type = 'WorkOrder' or s.Type = 'ServiceOrder'
                                union
								(select sh.Title as EventTitle,
                                sh.StartDate as EventStartDate,
                                sh.EndDate as EventEndDate,
                                sh.Type as EventType,
                                sh.LeadId as EventLeadId,
                                '00000000-0000-0000-0000-000000000000' as EventCusId,
                                '00000000-0000-0000-0000-000000000000' as EventCustomId,
								'00000000-0000-0000-0000-000000000000' as EventAppid,
								pg.Name as EventPermissionName
								from Schedule sh
								join Customer cus
                                on cus.Id=sh.LeadId
								join Employee emp
								on emp.EmployeeId = sh.Identifier
								left join UserPermission _up
								on emp.UserName = _up.UserName
                                left join PermissionGroup pg
                                on _up.PermissionGroupId = pg.Id
                                left join UserCompany _uc
                                on _uc.UserName = _up.UserName
								where
                                and sh.CompanyId = '{0}'
								and sh.Type = 'QA1' or sh.Type = 'QA2'
                                and sh.IsCompleted = 0
								and pg.Name is not null
								and pg.Name = '{1}')) as temp
                                select * from #temUnion";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyID, PermissionRole);
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

        public DataTable GetSchedulingByCompanyIdAndFilterForGoogleMap(Guid companyID, string date, string type, string user, bool ispermit, bool IsType)
        {
            string LostStatusQuery = "";
            if (!ispermit)
            {
                LostStatusQuery = string.Format("and ticket.[Status] != 'Lost'");
            }
            string sqlQuery = @"declare @CompanyId uniqueidentifier
                                set @CompanyId = '{0}'

                                select
                                emp.FirstName + ' ' + emp.LastName as EventTitle,
                                ISNULL(CONVERT(datetime, ca.AppointmentDate + ' ' + IIF(ca.AppointmentStartTime != '-1', ca.AppointmentStartTime, '00:00')), '1900-01-01 00:00:00') as EventStartDate,
								ISNULL(CONVERT(datetime, ca.AppointmentDate + ' ' + IIF(ca.AppointmentEndTime != '-1', ca.AppointmentEndTime, '00:00')), '1900-01-01 00:00:00') as EventEndDate,
                                ca.AppointmentType as EventType,
                                lk.DisplayText as EventDisplayType,
                                ticket.Id as EventLeadId,
                                isnull(cus.CustomerId, '00000000-0000-0000-0000-000000000000') as EventCusId,
                                tu.UserId as EventCustomId,
								ca.AppointmentId as EventAppid,
                                iif((select (select CHARINDEX('technician', Tag) from PermissionGroup _pgroup where _pgroup.Id = _up.PermissionGroupId) from UserPermission _up where _up.PermissionGroupId is not null and _up.UserId = emp.Userid) > 0, emp.FirstName + ' ' + emp.LastName + ' (T)', iif(emp.UserId = '22222222-2222-2222-2222-222222222222', emp.FirstName + ' ' + emp.LastName + ' (T)', emp.FirstName + ' ' + emp.LastName)) as EventResourceName,
                                emp.IsCalendar as EventIsCalendar,
								iif((select count(map.Id) from Permission per left join PermissionGroupMap map on map.PermissionId = per.Id and map.CompanyId = per.CompanyId where Name = 'CustomerTicketDispatch') > 0 and ticket.IsDispatch != null and ticket.IsDispatch = 0 and ticket.[Status] != 'Completed' and ticket.IsClosed = 0, 'FFFF00', 
								
								iif((select AlterDisplayText from [LookUp] lkcolor where DataKey = 'TicketType' and lkcolor.DataValue = ca.AppointmentType) != 'FFFFFF' and (select AlterDisplayText from [LookUp] lkcolor where DataKey = 'TicketType' and lkcolor.DataValue = ca.AppointmentType) != '', (select AlterDisplayText from [LookUp] lkcolor where DataKey = 'TicketType' and lkcolor.DataValue = ca.AppointmentType), 'ccc')) as EventColor,
                                emp.Id as EventId,
                                emp.CalendarColor,
								imgset.TicketStatusColor as StatusColor,
                                cus.FirstName + ' ' + cus.LastName as EventCustomerName,
                                cus.Latlng,
                                cus.Id,
                                ca.IsAllDay as EventAllDay,
								0 as EventIsLead,
                                isnull((select Street from CustomerAddress where RefId = ticket.BookingId and CustomerId = ticket.CustomerId and AddressType = REPLACE(ticket.TicketType, ' ', '') + 'Location'), cus.Street) as EventStreet, 
								isnull((select IIF(cus.City != '', cus.City + ', ', '') + cus.[State] + ' ' + cus.ZipCode from CustomerAddress where RefId = ticket.BookingId and CustomerId = ticket.CustomerId and AddressType = REPLACE(ticket.TicketType, ' ', '') + 'Location'), IIF(cus.City != '', cus.City + ', ', '') + cus.[State] + ' ' + cus.ZipCode) as EventLocate,
                                convert(nvarchar(50), ca.AppointmentDate, 23) as EventDate,
                                '' as EventCalendarCount,
                                ticket.TicketId as EventTicketId
                                ,ticket.[Status] as EventStatus
                                ,'' as HoverTitle
                                ,iif(emp.UserId = '22222222-2222-2222-2222-222222222222', '0000', '1111') as EventUserTopOrder
                                ,cus.DBA as EventDBA
                                ,ticket.BookingId as EventBookingId
                                from CustomerAppointment ca
								left join Customer cus
                                on cus.CustomerId=ca.CustomerId
								left join TicketUser tu on   tu.TiketId =ca.AppointmentId 
                                left join Ticket ticket on ticket.TicketId = tu.TiketId
								left join Employee emp on  emp.UserId =tu.UserId
                                left join [Lookup] lk on lk.DataValue = iif(ca.AppointmentType != '-1', ca.AppointmentType, '') and lk.DataKey = 'TicketType'
                                left Join TicketStatusImageSetting imgset on imgset.TicketStatus = ticket.[Status]
                                where ca.CompanyId = @CompanyId
                                and tu.NotificationOnly = 0
                                and cus.Id > 0
                                and emp.IsCalendar = 1
                                and emp.Recruited = 1
        						and emp.IsActive = 1
								and emp.IsDeleted = 0
								and ca.AppointmentDate between '{1} 00:00:00.000' and '{1} 23:59:59.000'
                                and tu.IsPrimary = 1
                                and lk.AlterDisplayText1 is not null and lk.AlterDisplayText1 != '' and lk.AlterDisplayText1 = 'True'
                                {2}
                                {3}
                                {4}
                                and ticket.Id is not null
								and ticket.TicketId is not null
                                and cus.CustomerId is not null
                                order by EventStartDate asc ";

            string subquerytype = "";
            string subqueryuser = "";
            if (!string.IsNullOrWhiteSpace(type) && type != "'null'" && type != "''")
            {
                if (!IsType)
                {
                    LostStatusQuery = string.Format(" and ticket.[Status] = {0}", type);
                }
                else
                {
                    subquerytype = string.Format("and ca.AppointmentType in ({0})", type);
                    subquerytype += string.Format(" and ticket.TicketType in ({0})", type);
                }
            }
            if (!string.IsNullOrWhiteSpace(user) && user != "null" && user != "''")
            {
                subqueryuser = string.Format("and emp.UserId in ({0})", user);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, companyID, date, subquerytype, subqueryuser, LostStatusQuery);
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

        //public DataSet GetCustomCalendarScheduleListByCompanyId(Guid companyID, string startdate, string defaultView, string typeval, string userval, bool IsType, bool IsDeactive)
        //{
        //    string CountQuery = "";
        //    string subQuery = "select emp.UserId into #CurrentEmp from Employee emp where emp.Recruited = 1 and emp.IsCalendar = 1 and emp.IsActive = 1 and emp.IsDeleted = 0";
        //    string EmpConditionQuery = @" and emp.IsCalendar = 1 and emp.Recruited = 1 and emp.IsActive = 1 and emp.IsDeleted = 0";
        //    string sqlQuery = @" {8}                        
        //                        (select ticket.Id as EventLeadId,
        //                    	ticket.TicketId as EventTicketId,
        //                    	ca.AppointmentId as EventAppointmentId,
        //                    	ca.CustomerId as  EventCustomerId,
        //                    	ISNULL(CONVERT(datetime, ca.AppointmentDate + ' ' + IIF(ca.AppointmentStartTime != '-1', ca.AppointmentStartTime, '00:00')), '1900-01-01 00:00:00') as EventStartDate,
        //                    	ISNULL(CONVERT(datetime, ca.AppointmentDate + ' ' + IIF(ca.AppointmentEndTime != '-1', ca.AppointmentEndTime, '00:00')), '1900-01-01 00:00:00') as EventEndDate,
        //                    	lk.DataValue as EventType,
        //                    	lk.DisplayText as EventDisplayType,
        //                    	lk.AlterDisplayText as EventColor,
        //                    	emp.FirstName + ' ' + emp.LastName as EmployeeName,
        //                    	emp.UserId as EventEmployeeGuidId,
        //                    	emp.Id as EmployeeIntId,
        //                    	ca.IsAllDay as EventAllDay,
        //                    	convert(nvarchar(50), ca.AppointmentDate, 23) as EventDate,
        //                    	ticket.Status as EventStatus,
        //                    	ticket.BookingId as BookingId,
        //                    	emp.CalendarColor,
        //                    	imgset.TicketStatusColor as StatusColor,
        //                    	imgset.Filename as EventIcon,
        //                    	'' as EventAdditionalMember
        //                    	Into #tempCA
        //                    from CustomerAppointment ca 
                            
        //                    	left join TicketUser tu on tu.TiketId =ca.AppointmentId and tu.IsPrimary = 1
        //                    	left join Ticket ticket on ticket.TicketId = tu.TiketId
        //                    	left join Employee emp on emp.UserId =tu.UserId
        //                    	left join [Lookup] lk on lk.DataValue = iif(ca.AppointmentType != '-1', ca.AppointmentType, '') and lk.DataKey = 'TicketType'
        //                    	left Join TicketStatusImageSetting imgset on imgset.TicketStatus = ticket.[Status]
                            	
        //                    where tu.NotificationOnly = 0
        //                        and ca.CompanyId = '{0}'
        //                        {1}
        //                    	{2}
        //                        {7}
        //                    	and lk.AlterDisplayText1 is not null and lk.AlterDisplayText1 != '' and lk.AlterDisplayText1 = 'True'
                                
        //                    	and ticket.TicketId is not null
        //                        {3}
        //                    	{4})
                            
        //                    union

        //                    (select ticket.Id as EventLeadId,
        //                    	ticket.TicketId as EventTicketId,
        //                    	ca.AppointmentId as EventAppointmentId,
        //                    	ca.CustomerId as  EventCustomerId,
        //                    	ISNULL(CONVERT(datetime, ca.AppointmentDate + ' ' + IIF(ca.AppointmentStartTime != '-1', ca.AppointmentStartTime, '00:00')), '1900-01-01 00:00:00') as EventStartDate,
        //                    	ISNULL(CONVERT(datetime, ca.AppointmentDate + ' ' + IIF(ca.AppointmentEndTime != '-1', ca.AppointmentEndTime, '00:00')), '1900-01-01 00:00:00') as EventEndDate,
        //                    	lk.DataValue as EventType,
        //                    	lk.DisplayText as EventDisplayType,
        //                    	lk.AlterDisplayText as EventColor,
        //                    	emp.FirstName + ' ' + emp.LastName as EmployeeName,
        //                    	emp.UserId as EventEmployeeGuidId,
        //                    	emp.Id as EmployeeIntId,
        //                    	ca.IsAllDay as EventAllDay,
        //                    	convert(nvarchar(50), ca.AppointmentDate, 23) as EventDate,
        //                    	ticket.Status as EventStatus,
        //                    	ticket.BookingId as BookingId,
        //                    	emp.CalendarColor,
        //                    	imgset.TicketStatusColor as StatusColor,
        //                    	imgset.Filename as EventIcon,
        //                    	'Additional' as EventAdditionalMember
        //                    from AdditionalMembersAppointment ca 

        //                    	left join TicketUser tu on tu.UserId = ca.EmployeeId and tu.IsPrimary = 0 
        //                    	left join Ticket ticket on ticket.TicketId = ca.AppointmentId 
        //                    	left join Employee emp on emp.UserId =tu.UserId
        //                    	left join[Lookup] lk on lk.DataValue = iif(ticket.TicketType != '-1', ticket.TicketType, '') and lk.DataKey = 'TicketType' 
        //                    	left Join TicketStatusImageSetting imgset on imgset.TicketStatus = ticket.[Status]
                            	
        //                    where tu.NotificationOnly = 0
        //                        and ca.CompanyId = '{0}'
        //                        {1}
        //                    	{2}
        //                    	and lk.AlterDisplayText1 is not null and lk.AlterDisplayText1 != '' and lk.AlterDisplayText1 = 'True'
        //                        {7}
        //                    	and ticket.TicketId is not null
        //                        {3}
        //                    	{4})
                            
        //                    	select tca.*,
        //                    	cus.Id as EventCustomerIntId,
        //                    	cus.Street as EventStreet,
        //                    	cus.City+', ' + cus.State +' '+ cus.ZipCode as EventLocate,
        //                    	case when cus.City is not null and cus.City != '' and cus.State is not null and cus.State != '' then cus.City+', ' + cus.State when (cus.City is null or cus.City = '') and cus.State is not null and cus.State != '' then cus.State when cus.City is not null and cus.City != '' and (cus.State is null or cus.State = '') then cus.City else '' end as EventTicketLocation,
        //                    	CASE WHEN cus.DBA is not null and cus.DBA != '' THEN cus.DBA WHEN cus.BusinessName is not null and cus.BusinessName != '' THEN cus.BusinessName ELSE cus.FirstName + ' ' + cus.LastName END AS EventCustomerName
                            
        //                    	from #tempCA tca
        //                    	left join Customer cus on  tca.EventCustomerId = cus.CustomerId
        //                    	order by tca.EmployeeIntId, tca.EventStartDate
        //                    	Drop table #tempCA

        //                   -- For Getting Employee List
        //                    	{5}
        //                   -- For Count Ticket
        //                        {6}
        //                        drop Table #CurrentEmp";

        //    string empquery = "";
        //    string DateSubquery = "";
        //    string TicketTypesubquery = "";
        //    string Employeelist = "";
        //    string TicketStatusQuery = " and ticket.[Status] != 'Lost' ";
        //    if (!string.IsNullOrWhiteSpace(defaultView) && defaultView == "Daily" && !string.IsNullOrWhiteSpace(startdate))
        //    {
        //        DateSubquery = string.Format("and ca.AppointmentDate between '{0} 00:00:00.000' and '{0} 23:59:59.000'", startdate);
        //    }
        //    else if (!string.IsNullOrWhiteSpace(defaultView) && (defaultView == "Weekly" || defaultView == "List") && !string.IsNullOrWhiteSpace(startdate))
        //    {

        //        var datestart = Convert.ToDateTime(startdate);
        //        var dateend = Convert.ToDateTime(startdate).AddDays(6);
        //        DateSubquery = string.Format("and ca.AppointmentDate between '{0} 00:00:00.000' and '{1} 23:59:59.000'", datestart.ToString("yyyy-MM-dd"), dateend.ToString("yyyy-MM-dd"));
        //    }
        //    else if (!string.IsNullOrWhiteSpace(defaultView) && defaultView == "Monthly" && !string.IsNullOrWhiteSpace(startdate))
        //    {
        //        var stardate = Convert.ToDateTime(startdate);
        //        var datestart = new DateTime(stardate.Year, stardate.Month, 1);
        //        var dateend = new DateTime(stardate.Year, stardate.Month, DateTime.DaysInMonth(stardate.Year, stardate.Month));
        //        DateSubquery = string.Format("and ca.AppointmentDate between '{0} 00:00:00.000' and '{1} 23:59:59.000'", datestart.ToString("yyyy-MM-dd"), dateend.ToString("yyyy-MM-dd"));
        //    }
        //    else
        //    {
        //        if (!string.IsNullOrWhiteSpace(defaultView) && defaultView == "Daily")
        //        {
        //            DateSubquery = string.Format("and ca.AppointmentDate between '{0} 00:00:00.000' and '{0} 23:59:59.000'", DateTime.Now.ToString("yyyy-MM-dd"));
        //        }
        //        else if (!string.IsNullOrWhiteSpace(defaultView) && (defaultView == "Weekly" || defaultView == "List"))
        //        {
        //            if (string.IsNullOrWhiteSpace(startdate))
        //            {
        //                startdate = DateTime.Now.ToString();
        //            }
        //            var datestart = Convert.ToDateTime(startdate);
        //            var dateend = Convert.ToDateTime(startdate).AddDays(6);
        //            DateSubquery = string.Format("and ca.AppointmentDate between '{0} 00:00:00.000' and '{1} 23:59:59.000'", datestart.ToString("yyyy-MM-dd"), dateend.ToString("yyyy-MM-dd"));
        //        }
        //        else if (!string.IsNullOrWhiteSpace(defaultView) && defaultView == "Monthly")
        //        {
        //            var datestart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //            var dateend = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
        //            DateSubquery = string.Format("and ca.AppointmentDate between '{0} 00:00:00.000' and '{1} 23:59:59.000'", datestart.ToString("yyyy-MM-dd"), dateend.ToString("yyyy-MM-dd"));
        //        }
        //    }

        //    if (!string.IsNullOrWhiteSpace(typeval) && typeval != "'null'" && typeval != "''")
        //    {
        //        if (!IsType)
        //        {
        //            TicketStatusQuery = string.Format(" and ticket.[Status] = {0}", typeval);
        //        }
        //        else
        //        {
        //            TicketTypesubquery = string.Format("and ticket.TicketType in ({0})", typeval);
        //        }
        //    }
        //    if (!string.IsNullOrWhiteSpace(userval) && userval != "'null'" && userval != "''")
        //    {
        //        empquery = string.Format("and emp.UserId in ({0})", userval);
        //    }
        //    Employeelist = string.Format("select iif((select (select CHARINDEX('technician', Tag) from PermissionGroup _pgroup where _pgroup.Id = _up.PermissionGroupId) from UserPermission _up where _up.PermissionGroupId is not null and _up.UserId = emp.Userid) > 0, emp.FirstName + ' ' + emp.LastName, iif(emp.UserId = '22222222-2222-2222-2222-222222222222', emp.FirstName + ' ' + emp.LastName, emp.FirstName + ' ' + emp.LastName)) as ResourceName, emp.UserId, emp.Id from Employee emp where emp.IsCalendar is not null and emp.IsCalendar = 1 and emp.Recruited = 1 and emp.IsActive = 1 and emp.IsDeleted = 0 and CompanyId = '{0}' {1}", companyID, empquery);
        //    if (IsDeactive)
        //    {
        //        subQuery = string.Format(@"select emp.UserId into #CurrentEmp from Employee emp where emp.Recruited = 1 and emp.IsCalendar = 1 and emp.IsActive = 1 and emp.IsDeleted = 0
        //                            select distinct emp.UserId into #DeactiveId from CustomerAppointment ca 
        //                            left join TicketUser tu on tu.TiketId = ca.AppointmentId 
        //                            left join Employee emp on emp.UserId = tu.UserId 
        //                            left join [Lookup] lk on lk.DataValue = iif(ca.AppointmentType != '-1', ca.AppointmentType, '') and lk.DataKey = 'TicketType' 
        //                            where lk.AlterDisplayText1 is not null and lk.AlterDisplayText1 != '' and lk.AlterDisplayText1 = 'True' 
        //                            {0} 
        //                            and emp.UserId not in (select UserId from #CurrentEmp)", DateSubquery);
        //        CountQuery = string.Format(@"
        //                            select Count(*) as TicketCount from CustomerAppointment ca 
        //                            left join TicketUser tu on tu.TiketId = ca.AppointmentId 
        //                            left join Employee emp on emp.UserId = tu.UserId 
        //                            left join Ticket tic on tic.TicketId = tu.TiketId
        //                            left join [Lookup] lk on lk.DataValue = iif(ca.AppointmentType != '-1', ca.AppointmentType, '') and lk.DataKey = 'TicketType' 
        //                            where lk.AlterDisplayText1 is not null and lk.AlterDisplayText1 != '' and lk.AlterDisplayText1 = 'True' 
        //                            {0} 
        //                            and ca.CompanyId = '{1}'
								//	and tic.[Status] != 'Lost' 
								//	and tic.TicketId is not null 
        //                            and emp.UserId in (select UserId from #CurrentEmp)", DateSubquery, companyID);
        //        EmpConditionQuery = "";
        //        empquery = "and emp.UserId in (select UserId from #DeactiveId)";
        //        Employeelist = string.Format(@"select emp.FirstName + ' ' + emp.LastName as ResourceName, emp.UserId, emp.Id from Employee emp where emp.UserId in (select UserId from #DeactiveId) and emp.CompanyId = '{0}'" +
        //            "                         drop table #DeactiveId", companyID);
        //    }
        //    else
        //    {
        //        CountQuery = string.Format(@" 
        //                            select Count(*) as TicketCount from CustomerAppointment ca 
        //                            left join TicketUser tu on tu.TiketId = ca.AppointmentId 
        //                            left join Employee emp on emp.UserId = tu.UserId 
        //                            left join Ticket tic on tic.TicketId = tu.TiketId
        //                            left join [Lookup] lk on lk.DataValue = iif(ca.AppointmentType != '-1', ca.AppointmentType, '') and lk.DataKey = 'TicketType' 
        //                            where lk.AlterDisplayText1 is not null and lk.AlterDisplayText1 != '' and lk.AlterDisplayText1 = 'True' 
        //                            and ca.CompanyId = '{1}'
								//	and tic.[Status] != 'Lost' 
								//	and tic.TicketId is not null                                    
        //                            {0}
        //                            and emp.UserId not in (select UserId from #CurrentEmp)", DateSubquery, companyID);
        //    }
        //    try
        //    {
        //        sqlQuery = string.Format(sqlQuery, companyID //0
        //            , TicketStatusQuery //1
        //            , TicketTypesubquery //2
        //            , empquery  //3
        //            , DateSubquery   //4                  
        //            , Employeelist  //5
        //            , CountQuery //6
        //            , EmpConditionQuery //7
        //             , subQuery);
        //        using (SqlCommand cmd = GetSQLCommand(sqlQuery))
        //        {
        //            DataSet dsResult = GetDataSet(cmd);
        //            return dsResult;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        public DataSet GetCustomCalendarIndexPageDataByCompanyId(Guid companyID)
        {
            string sqlQuery = @"SELECT SearchKey, Value, IsActive, Description, OptionalValue FROM GlobalSetting where Tag = 'CustomCalendarSettings' {0} AND IsActive = 1 OR (SearchKey IN ('ScheduleCalendarDefaultView', 'ScheduleCalendarMinTimeRange', 'ScheduleCalendarMaxTimeRange', 'FirstDayOfWeek', 'ScheduleCalendarDefaultView'))
            select iif((select (select CHARINDEX('technician', Tag) from PermissionGroup _pgroup where _pgroup.Id = _up.PermissionGroupId) from UserPermission _up where _up.PermissionGroupId is not null and _up.UserId = emp.Userid) > 0, emp.FirstName + ' ' + emp.LastName, iif(emp.UserId = '22222222-2222-2222-2222-222222222222', emp.FirstName + ' ' + emp.LastName, emp.FirstName + ' ' + emp.LastName)) as ResourceName, emp.UserId, emp.Id from Employee emp where emp.IsCalendar is not null and emp.IsCalendar = 1 and emp.Recruited = 1 and emp.IsActive = 1 and emp.IsDeleted = 0 {0}
            select Id, DisplayText, DataValue, AlterDisplayText, IsDefaultItem from Lookup where DataKey='TicketType' AND AlterDisplayText1 = 'True' {0} order by DataOrder
            select TicketStatus, Filename, TicketStatusColor From TicketStatusImageSetting tsis join Lookup lk on lk.DataValue = tsis.TicketStatus AND lk.DataKey='TicketStatus' AND lk.IsActive = 1 where tsis.IsActive = 1 AND tsis.CompanyId = '{1}' AND  tsis.Filename != null OR tsis.Filename != '' ";

            string addQuery = string.Format("and CompanyId = '{0}'", companyID);

            try
            {
                sqlQuery = string.Format(sqlQuery, addQuery, companyID);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
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

    }
}
