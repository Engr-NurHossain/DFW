using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.DataAccess
{

    public partial class CustomerAppointmentDataAccess
    {
        public CustomerAppointmentDataAccess(string ConnectionStr) : base(ConnectionStr) { }
        public CustomerAppointmentDataAccess() { }

        public bool ReseedCustomerAppointmentTable()
        {
            string SqlQuery = @"Delete from CustomerAppointment 
                                DBCC CHECKIDENT('CustomerAppointment', RESEED, 0)
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
        public DataTable GetScheduleIdByCustomerAppointmentId(Guid appointmentid)
        {
            string sqlQuery = @"select s.Id as scheduleid
                                from Schedule s
                                join CustomerAppointment ca
                                on ca.AppointmentId = CONVERT(uniqueidentifier, s.Identifier)
                                where ca.AppointmentId = '{0}'
                                and ca.AppointmentType = 'ServiceOrder'
                                and ca.Status = 1";
            try
            {
                sqlQuery = string.Format(sqlQuery, appointmentid);
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

        public DataTable GteEmployeeNameByCustomerAppointmentEmployeeId(Guid empid)
        {
            string sqlQuery = @"select emp.FirstName +' '+ emp.LastName as EMPName
                                from Employee emp
                                join CustomerAppointment ca
                                on ca.EmployeeId = emp.UserId
                                where ca.EmployeeId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, empid);
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

        public DataTable GetAllWorkOrderByCompanyId(Guid companyID)
        {
            string sqlQuery = @"select *
                                from CustomerAppointment ca
                                where ca.AppointmentType='WorkOrder'
                                and ca.CompanyId='{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyID);
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

        public DataTable GetAllSalesAppoinmentByEmployeeId(Guid customerId, Guid companyId, Guid? employeeId)
        {
            string sqlQuery = @"select ca.*,  
                                emp.FirstName as SalesPerson
                                from CustomerAppointment ca
                                left join Employee emp
                                on emp.UserId = ca.EmployeeId
                                where  ca.CustomerId ='{0}'
                                and emp.IsSalesPerson = 1
                                and ca.AppointmentType='Sales'";
            try
            {
                sqlQuery = string.Format(sqlQuery, customerId, companyId);
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
        public DataTable GetAllWorkOrderAppoinmentByEmployeeId(Guid customerId, Guid companyId, Guid? employeeId)
        {
            string sqlQuery = @"select ca.*,  
                                emp.FirstName as WorkPerson
                                from CustomerAppointment ca
                                left join Employee emp
                                on emp.UserId = ca.EmployeeId
                                where  ca.CustomerId ='{0}'
                                and emp.IsInstaller = 1
                                and ca.AppointmentType='WorkOrder'";
            try
            {
                sqlQuery = string.Format(sqlQuery, customerId, companyId);
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

        public DataTable GetAllWorkOrderAppoinmentByCustomerIdAndCompanyId(Guid customerId, Guid companyId)
        {
            string sqlQuery = @"select 
	                            CA.*,
	                            emp.FirstName+' '+emp.LastName as WorkPerson
	                            from 
		                            CustomerAppointment CA
		                            left join Employee emp
		                            on CA.EmployeeId = emp.UserId 
	                            where 
		                            CA.CustomerId = '{0}' and
                                    ca.CompanyId = '{1}' and 
		                            Ca.AppointmentType = 'WorkOrder'";
            try
            {
                sqlQuery = string.Format(sqlQuery, customerId, companyId);
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

        public DataTable GetAllServiceAppoinmentByEmployeeId(Guid customerId, Guid companyId, Guid? employeeId)
        {
            string sqlQuery = @"select ca.*,  
                                emp.FirstName as ServicePerson
                                from CustomerAppointment ca
                                left join Employee emp
                                on emp.UserId = ca.EmployeeId
                                where  ca.CustomerId ='{0}'
                                --and emp.IsServiceCall = 1
                                and ca.AppointmentType='ServiceOrder'";

            //string subquery = "";
            //if (isCurrentEmployee)
            //{
            //    subquery = string.Format(" AND ca.CustomerId ='{0}' and emp.IsServiceCall = 1", customerId);
            //}
            try
            {
                sqlQuery = string.Format(sqlQuery, customerId, companyId);
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

        public DataTable GetServiceOrderMailDetailsByAppointmentIdAndCompanyId(Guid AppointmentId, Guid CompanyId)
        {
            string sqlQuery = @"select 
                                Emp.UserName as EmpEmail,
                                Emp.FirstName+' '+Emp.LastName as EmployeeName,
                                Cus.FirstName as CustomerFirstName,
                                Cus.MiddleName as CustomerMiddleName,
                                Cus.LastName as CustomerLastName,
                                CA.AppointmentDate as AppointmentDate,
                                CA.AppointmentStartTime as AppointmentStartTime,
                                CA.AppointmentEndTime as AppointmentEndTime,
                                CA.Notes as Notes,
                                CA.LastUpdatedDate as LastUpdatedDate,
                                CA.LastUpdatedBy as LastUpdatedBy
                                from 
                                CustomerAppointment CA
                                left join Customer Cus
                                on CA.CustomerId = Cus.CustomerId 
                                left join Employee Emp 
                                on ca.EmployeeId = Emp.UserId 
                                where CA.AppointmentId = '{0}' 
                                and CA.CompanyId = '{1}'";

            try
            {
                sqlQuery = string.Format(sqlQuery, AppointmentId, CompanyId);
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

        public DataTable GetCustomerAppointmentEquipmentByCustomerIdAndAppointmentId(Guid AppointmentId, Guid CustomerId)
        {
            string sqlQuery = @"select 
	                                eq.Name, 
	                                cae.Quantity,
	                                cae.UnitPrice,
	                                Cae.TotalPrice 
                                    from Customer cu 
                                    left join CustomerAppointment ca
	                                on cu.CustomerId = ca.CustomerId
                                    left join CustomerAppointmentEquipment Cae
	                                on ca.AppointmentId=Cae.AppointmentId
                                    left join Equipment eq 
	                                on Cae.EquipmentId=eq.EquipmentId
                                    where Cae.AppointmentId='{0}' and
                                    cu.CustomerId='{1}'";

            try
            {
                sqlQuery = string.Format(sqlQuery, AppointmentId, CustomerId);
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

        public DataTable GetAllScheduleByCompanyIdAndCustomerId(Guid CompanyId, Guid EmployeeId)
        {
            string sqlFinal = @"select *  into #temUnion from (select 
	                            CA.AppointmentType EventType,
	                            CA.AppointmentDate EventStartDate,
	                            CA.AppointmentDate EventEndDate,
	                            CA.Notes EventName ,
								EmployeeId,
								 CAST(CA.AppointmentId AS nvarchar(50)) inedtity,
								 CAST(CA.CustomerId AS nvarchar(50)) EventCustomerId,
								s.LeadId EventLeadId
	                            from CustomerAppointment CA
	                            left join Customer Cus
	                            on CA.CustomerId = Cus.CustomerId 
	                            left join CustomerCompany CC
	                            on Cus.CustomerId = CC.CustomerId
								left join Schedule s
								on s.LeadId = Cus.Id
								where CC.CompanyId =  '{0}' AND ca.AppointmentDate IS NOT NULL AND CA.Status = 0
	                            UNION
	                            select 'TechSchedule' EventType, InstallDate EventStartDate, InstallDate EventEndDate, 'Tech Schedule' EventName, EmployeeId, '' inedtity, '' EventCustomerId, '0' EventLeadId from TechSchedule 
	                            UNION 
	                            select 'Note' EventType,cn.ReminderDate EventStartDate, cn.ReminderDate EventEndDate, Notes EventName, na.EmployeeId, CAST(c.Id  AS nvarchar(50)) inedtity, '' EventCustomerId, na.NoteId EventLeadId from NoteAssign na
	                            Left join CustomerNote cn 
								on cn.Id = na.NoteId
								Left JOin Customer c on c.CustomerId = cn.CustomerId
	                            Where cn.ReminderDate IS NOT NULL and IsShedule = 0 and IsFollowUp = 0) as temp
								UNION
								select 'FollowUp' EventType,cn.ReminderDate EventStartDate, cn.ReminderDate EventEndDate, Notes EventName, '00000000-0000-0000-0000-000000000000' EmployeeId, CAST(c.Id  AS nvarchar(50)) inedtity, CAST(c.CustomerId AS nvarchar(50)) EventCustomerId, cn.Id EventLeadId from CustomerNote cn
								Left JOin Customer c on c.CustomerId = cn.CustomerId
	                            Where IsShedule = 1 and IsFollowUp = 1
								UNION
								select s.Type EventType,StartDate EventStartDate,EndDate EventEndDate,s.Type EventName ,'00000000-0000-0000-0000-000000000000' EmployeeId, '' inedtity, '' EventCustomerId, s.LeadId EventLeadId from Schedule s
								where s.IsCompleted=0
	                            select * from #temUnion {1}
	                            drop table #temUnion";


            int CheckCount = 0;
            string empCheckSql = @"select count(up.Id) as 'EmpCheck'
                                from UserPermission up
                                Left join Employee e on e.UserName = up.UserName
                                left join PermissionGroupMap pgm on pgm.PermissionGroupId = up.PermissionGroupId and pgm.CompanyId = up.CompanyId
                                left join PermissionGroup pg on pg.Id = pgm.PermissionGroupId
                                where e.UserId = '{0}' AND pg.Name in ('SysAdmin', 'Stuff')";

            empCheckSql = string.Format(empCheckSql, EmployeeId);

            try
            {

                using (SqlCommand cmd = GetSQLCommand(empCheckSql))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    CheckCount = Convert.ToInt32(dsResult.Tables[0].Rows[0]["EmpCheck"]);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            string formatResult = "";

            if (CheckCount < 1) // Check  is Employee not admin.
            {
                formatResult = string.Format("where EmployeeId ='{0}'", EmployeeId.ToString());
            }
            sqlFinal = string.Format(sqlFinal, CompanyId.ToString(), formatResult);

            try
            {

                using (SqlCommand cmd = GetSQLCommand(sqlFinal))
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

        public DataTable GetDashBoardServiceBoardList(Guid CompanyId, DateTime StartTime, DateTime EndTime)
        {
            string sqlQuery = @"select 
                                	ca.AppointmentDate as AppointmentDate ,
                                	cus.FirstName as CustomerFirstName,
                                	cus.MiddleName as CustomerMiddleName,
                                	cus.LastName as CustomerLastName,
                                	emp.FirstName +' '+emp.LastName as EmployeeName,
                                	ca.AppointmentStartTime +' - '+ca.AppointmentEndTime as AppointmentTime,
                                	cus.Id as CustomerIntId,
                                	cus.CustomerId as CustomerGuidId,
                                	ca.AppointmentId as ServiceOrderId
                                from CustomerAppointment ca
                                	left join Customer cus
                                		on ca.CustomerId = cus.CustomerId
                                	left join CustomerCompany cc
                                		on cus.CustomerId = cc.CustomerId
                                	left join Employee emp 
                                		on ca.EmployeeId = emp.UserId
                                where ca.AppointmentType = 'ServiceOrder'
                                	and ca.Status = 0 
                                	and ca.CompanyId ='{0}'
                                	and ca.AppointmentDate between '{1}' and '{2}'
                                	and cus.Id is not null
                                	and emp.Id is not null ";

            try
            {
                sqlQuery = string.Format(sqlQuery, CompanyId, StartTime, EndTime);
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

        public DataTable GetAllCustomerAppointmentByAppIdCusId(Guid appid, Guid cusid)
        {
            string sqlQuery = @"select ca.* 
                                ,lkstt.DisplayText as AppointmentStartTimeVal
                                ,lkett.DisplayText as AppointmentEndTimeVal
                                from CustomerAppointment ca 
                                left join Lookup lkstt on ca.AppointmentStartTime = lkstt.DataValue
                                and lkstt.DataKey ='arrival'
                                left join Lookup lkett on ca.AppointmentEndTime = lkett.DataValue
                                and lkstt.DataKey ='arrival'

                                where ca.AppointmentId='{0}'
                                and ca.CustomerId ='{1}'";

            try
            {
                sqlQuery = string.Format(sqlQuery, appid, cusid);
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

    

        public DataTable GetDashBoardInstallationList(Guid CompanyId, DateTime StartTime, DateTime EndTime)
        {
            string sqlQuery = @"select 
                            	ca.AppointmentDate as AppointmentDate ,
                            	cus.FirstName as CustomerFirstName,
                            	cus.MiddleName as CustomerMiddleName,
                            	cus.LastName as CustomerLastName,
                            	emp.FirstName +' '+emp.LastName as EmployeeName,
                            	ca.AppointmentStartTime +' - '+ca.AppointmentEndTime as AppointmentTime,
                            	cus.Id as CustomerIntId,
                            	cus.CustomerId as CustomerGuidId,
                            	ca.AppointmentId as ServiceOrderId
                            from CustomerAppointment ca
                            	left join Customer cus
                            		on ca.CustomerId = cus.CustomerId
                            	left join CustomerCompany cc
                            		on cus.CustomerId = cc.CustomerId
                            	left join Employee emp 
                            		on ca.EmployeeId = emp.UserId
                            where ca.AppointmentType = 'WorkOrder'
                            	and ca.Status = 0 
                            	and ca.CompanyId ='{0}'
                            	and ca.AppointmentDate between '{1}' and '{2}'
                            	and cus.Id is not null
                            	and emp.Id is not null ";

            try
            {
                sqlQuery = string.Format(sqlQuery, CompanyId, StartTime, EndTime);
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
        public DataTable GetCustomerAptByNumber(string number)
        {
            string sqlQuery = @"select 
                                cusApt.AppointmentDate,
                                cusApt.AppointmentStartTime,
                                cusApt.AppointmentEndTime,
                                CASE When serZip.Id is null Then 'Out'
                                Else 'In'
                                END ServiceArea
                                from Customer cus
                                LEFT JOIN CustomerAppointment cusApt on cusApt.CustomerId=cus.CustomerId
                                LEFT JOIN ServiceAreaZipcode serZip on serZip.ZipCode=cus.ZipCode
                                where cus.Id='{0}'";
            sqlQuery = string.Format(sqlQuery, number);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult != null && dsResult.Tables.Count > 0 ? dsResult.Tables[0] : null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetCustomerAptByZipCode(string zipCode)
        {
            string sqlQuery = @"select 
                                cusApt.AppointmentDate,
                                cusApt.AppointmentStartTime,
                                cusApt.AppointmentEndTime,
                                CASE When serZip.Id is null Then 'Out'
                                Else 'In'
                                END ServiceArea
                                from Customer cus
                                LEFT JOIN CustomerAppointment cusApt on cusApt.CustomerId=cus.CustomerId
                                LEFT JOIN ServiceAreaZipcode serZip on serZip.ZipCode=cus.ZipCode
                                where cusApt.AppointmentDate > GetDate() AND cus.ZipCode='{0}'";
            sqlQuery = string.Format(sqlQuery, zipCode);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult != null && dsResult.Tables.Count > 0 ? dsResult.Tables[0] : null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetCustomerAptByDateAndCustomerNumber(DateTime appointmentDate, string customernumber, string StartTime, string EndTime)
        {
            string sqlQuery = @"select 
                                cusApt.AppointmentDate,
                                cusApt.AppointmentStartTime,
                                cusApt.AppointmentEndTime,
                                CASE When serZip.Id is null Then 'Out'
                                Else 'In'
                                END ServiceArea
                                from Customer cus
                                LEFT JOIN CustomerAppointment cusApt on cusApt.CustomerId=cus.CustomerId
                                LEFT JOIN ServiceAreaZipcode serZip on serZip.ZipCode=cus.ZipCode
                                where cus.Id={0} and cusApt.AppointmentDate='{1}' and cusApt.AppointmentStartTime='{2}' and cusApt.AppointmentEndTime='{3}'";
            sqlQuery = string.Format(sqlQuery, int.Parse(customernumber), appointmentDate.ToString("MM/dd/yyyy HH:mm tt"), StartTime, EndTime);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult != null && dsResult.Tables.Count > 0 ? dsResult.Tables[0] : null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetCustomerAptByDateAndZipCode(DateTime appointmentDate, string ZipCode)
        {
            string sqlQuery = @"select 
                                cusApt.AppointmentDate,
                                cusApt.AppointmentStartTime,
                                cusApt.AppointmentEndTime,
                                CASE When serZip.Id is null Then 'Out'
                                Else 'In'
                                END ServiceArea
                                from Customer cus
                                LEFT JOIN CustomerAppointment cusApt on cusApt.CustomerId=cus.CustomerId
                                LEFT JOIN ServiceAreaZipcode serZip on serZip.ZipCode=cus.ZipCode
                                where cusApt.AppointmentDate='{0}' And cus.ZipCode='{1}'";
            sqlQuery = string.Format(sqlQuery, appointmentDate.ToString("MM/dd/yyyy HH:mm tt"), ZipCode);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult != null && dsResult.Tables.Count > 0 ? dsResult.Tables[0] : null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetCustomerAptByDate(DateTime appointmentDate)
        {
            string sqlQuery = @"select 
                                cusApt.AppointmentDate,
                                cusApt.AppointmentStartTime,
                                cusApt.AppointmentEndTime,
                                CASE When serZip.Id is null Then 'Out'
                                Else 'In'
                                END ServiceArea
                                from Customer cus
                                LEFT JOIN CustomerAppointment cusApt on cusApt.CustomerId=cus.CustomerId
                                LEFT JOIN ServiceAreaZipcode serZip on serZip.ZipCode=cus.ZipCode
                                where cusApt.AppointmentDate='{0}'";
            sqlQuery = string.Format(sqlQuery, appointmentDate.ToString("MM/dd/yyyy HH:mm tt"));
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult != null && dsResult.Tables.Count > 0 ? dsResult.Tables[0] : null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetCustomerAptByANIAndDateAndCustomerNumber(string ANI, DateTime appointmentDate, string customernumber)
        {
            string sqlQuery = @"select 
                                cusApt.AppointmentDate,
                                cusApt.AppointmentStartTime,
                                cusApt.AppointmentEndTime,
                                CASE When serZip.Id is null Then 'Out'
                                Else 'In'
                                END ServiceArea
                                from Customer cus
                                LEFT JOIN CustomerAppointment cusApt on cusApt.CustomerId=cus.CustomerId
                                LEFT JOIN ServiceAreaZipcode serZip on serZip.ZipCode=cus.ZipCode
                                where cus.Id={0} or cusApt.AppointmentDate='{1}' or cus.PrimaryPhone='{2}'";
            sqlQuery = string.Format(sqlQuery, int.Parse(customernumber), appointmentDate.ToString("MM/dd/yyyy HH:mm tt"), ANI);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult != null && dsResult.Tables.Count > 0 ? dsResult.Tables[0] : null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetCustomerAppointmentListByDateSchedule(string date, Guid companyid)
        {
            string sqlQuery = @"select ca.*, cus.FirstName + ' ' + cus.LastName as CustomerName from customerappointment ca
                                left join Ticket tk on tk.TicketId = ca.AppointmentId
                                left join Customer cus on cus.CustomerId = ca.CustomerId
                                where CONVERT(datetime, ca.AppointmentDate + ' ' + IIF(ca.AppointmentEndTime != '-1', ca.AppointmentEndTime, '00:00')) = '{0}'
                                and ca.CompanyId = '{1}'
                                and tk.[Status] != 'Completed'
                                and tk.[Status] != 'Lost'
                                and tk.[Status] != 'Closed'
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, date, companyid);
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

        public bool DeleteAdditionalMembersAppointmentByAppointmentId(Guid appointid)
        {
            string sqlQuery = @"delete from AdditionalMembersAppointment where AppointmentId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, appointid);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
