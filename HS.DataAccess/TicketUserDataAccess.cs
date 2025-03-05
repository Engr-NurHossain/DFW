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
    public partial class TicketUserDataAccess
    {
        public TicketUserDataAccess(string ConStr):base(ConStr) { }
        public bool DeleteTicketUserByTicketId(Guid ticketId, bool? isAssigned,bool notifyCheck)
        {
            string primaryCheckSQl = "";
            string NotifyCheckSql = "and NotificationOnly =0";
            if (isAssigned.HasValue && isAssigned.Value)
            {
                primaryCheckSQl = "and IsPrimary =1 and NotificationOnly = 0";
            }
            else if(isAssigned.HasValue && !isAssigned.Value)
            {
                primaryCheckSQl = "and IsPrimary =0 and NotificationOnly = 0";
            }else if (notifyCheck)
            {
                primaryCheckSQl = "and NotificationOnly =1";
            }
            string SqlQuery = @"delete from TicketUser 
                                where TiketId = '{0}'
                                {1}
                                ";
            try
            {
                SqlQuery = string.Format(SqlQuery, ticketId, primaryCheckSQl, NotifyCheckSql);
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

        public DataTable GetTicketUserListByUserId(Guid userid, string type)
        {
            string sqlQuery = @"select * from TicketUser tu
                                left join Ticket tk on tk.TicketId = tu.TiketId
                                where tu.TiketId = '{0}'
                                and tu.NotificationOnly = 0
                                and tu.IsPrimary = 1
                                and tk.TicketType = '{1}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, userid, type);
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
        public DataTable GetAllTicketAdditionalMemberByTicketId(Guid TicketId)
        {
            string sqlQuery = @"select tualist.UserId,emp.FirstName+' '+emp.LastName as FullName, tualist.IsReschedulePay from TicketUser tualist 
                                left join employee emp on emp.UserId = tualist.UserId
                                where tualist.TiketId ='{0}' and IsPrimary = 0 and tualist.NotificationOnly=0";
            try
            {
                sqlQuery = string.Format(sqlQuery, TicketId);
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
        public DataTable GetTicketUserByTicketIdAndAdditionalMember(Guid TicketId,Guid UserId)
        {
            string sqlQuery = @"select * from TicketUser where tiketId = '{0}' and UserId = '{1}' and IsPrimary = 0";
            try
            {
                sqlQuery = string.Format(sqlQuery, TicketId,UserId);
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
        
        public DataTable GetTicketUserListAndCustomerAppointmentByUserId(Guid userid, string type, string min, string max)
        {
            string sqlQuery = @"select tu.TiketId, format(ISNULL(CONVERT(datetime, ca.AppointmentDate + ' ' + IIF(ca.AppointmentStartTime != '-1', ca.AppointmentStartTime, '00:00')), '1900-01-01 00:00:00'), 'hh:mm tt') as StartDate,
                                format(ISNULL(CONVERT(datetime, ca.AppointmentDate + ' ' + IIF(ca.AppointmentEndTime != '-1', ca.AppointmentEndTime, '00:00')), '1900-01-01 00:00:00'), 'hh:mm tt') as Enddate,
                                cs.FirstName + ' ' + cs.LastName as CustomerName,
                                isnull((select Street from CustomerAddress where RefId = tk.BookingId and CustomerId = tk.CustomerId and AddressType = REPLACE(tk.TicketType, ' ', '') + 'Location'), cs.Street) as Street, 
								isnull((select City from CustomerAddress where RefId = tk.BookingId and CustomerId = tk.CustomerId and AddressType = REPLACE(tk.TicketType, ' ', '') + 'Location'), cs.City) as City, 
								isnull((select [State] from CustomerAddress where RefId = tk.BookingId and CustomerId = tk.CustomerId and AddressType = REPLACE(tk.TicketType, ' ', '') + 'Location'), cs.[State]) as [State],
								isnull((select ZipCode from CustomerAddress where RefId = tk.BookingId and CustomerId = tk.CustomerId and AddressType = REPLACE(tk.TicketType, ' ', '') + 'Location'), cs.ZipCode) as ZipCode,
                                ca.IsAllDay, emp.FirstName + ' ' + emp.LastName as EMPNAME,
                                tk.Id as TicketIntId,
                                tk.BookingId as BookingId
                                from TicketUser tu
                                left join Ticket tk on tk.TicketId = tu.TiketId
                                left join CustomerAppointment ca on ca.AppointmentId = tk.TicketId
								left join Customer cs on cs.CustomerId = tk.CustomerId
                                left join Employee emp on emp.UserId = tu.UserId
                                where tu.TiketId = '{0}'
                                and tu.NotificationOnly = 0
                                --and tu.IsPrimary = 1
                                and tk.TicketType = '{1}'
                                and tk.CompletionDate between '{2}' and '{3}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, userid, type, min, max);
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

        public bool UpdateTicketUserToSystemUserById(Guid UserId)
        {
            string sqlQuery = @" Update TicketUser SET UserId = '22222222-2222-2222-2222-222222222222' where UserId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, UserId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    UpdateRecord(cmd);
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
