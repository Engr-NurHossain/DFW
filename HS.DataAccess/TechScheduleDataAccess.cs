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
    public partial class TechScheduleDataAccess
    {
        public DataTable GetAllTechScheduleByEmployeeId(Guid customerId,Guid companyId)
        {
            string sqlQuery = @"select ts.*,  
                                emp.FirstName  as TechnicianName
                                from TechSchedule ts
                                left join Employee emp
                                on emp.UserId = ts.EmployeeId
                                where  ts.CustomerID ='{0}'
                                AND ts.CompanyID ='{1}'";
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

        public DataTable GetAllLeadTechScheduleByCompanyid(Guid companyId)
        {
            string sqlQuery = @"select 
                                ts.ArrivalTime as EventStartTime,
                                ts.DepartureTime as EventEndTime,
                                ts.EstimatedArrival as EventTime,
                                ts.InstallDate as EventDate,
                                emp.FirstName+' '+emp.LastName as EventName,
                                ts.Id as EventId,
                                cus.Id as EventCustomer
                                from TechSchedule ts
                                join CustomerCompany cc
                                on cc.CustomerId=ts.CustomerId
                                join Employee emp
                                on emp.UserId=ts.EmployeeId
                                join Customer cus
                                on cus.CustomerId=ts.CustomerId
                                where cc.CompanyId='97bcf758-a482-47eb-82b8-f88bf12293ff'
                                and cc.IsLead=1";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId);
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
