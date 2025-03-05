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
	public partial class AdditionalMembersAppointmentDataAccess
	{
        public DataTable GetAllAdditionalAppointmentByTicketId(Guid ticketId,Guid EmpId)
        {
            string subQUery = "";
            string sqlQuery = @"select 
                                ap.*,em.FirstName+' '+em.LastName as EmpName,lkstarttime.DisplayText as StartTime,lkendtime.DisplayText as EndTime
                                from AdditionalMembersAppointment ap
                                LEFT JOIN Employee em on em.UserId=ap.EmployeeId
                                left join Lookup lkstarttime on  lkstarttime.DataKey ='Arrival' 
                                and lkstarttime.DataValue = ap.AppointmentStartTime
                                left join Lookup lkendtime on  lkendtime.DataKey ='Arrival' 
                                and lkendtime.DataValue = ap.AppointmentEndTime
                                Where ap.AppointmentId='{0}'
                                and ap.AppointmentStartTime != '-1' and ap.AppointmentEndTime != '-1'";
            try
            {
                sqlQuery = string.Format(sqlQuery, ticketId, EmpId);
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
