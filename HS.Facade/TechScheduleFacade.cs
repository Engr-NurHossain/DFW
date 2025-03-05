using HS.DataAccess;
using HS.Entities;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Facade
{
    public class TechScheduleFacade : BaseFacade
    {
        public TechScheduleFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        TechScheduleDataAccess _TechScheduleDataAccess
        {
            get
            {
                return (TechScheduleDataAccess)_ClientContext[typeof(TechScheduleDataAccess)];
            }
        }
        EmployeeDataAccess _EmployeeDataAccess
        {
            get
            {
                return (EmployeeDataAccess)_ClientContext[typeof(EmployeeDataAccess)];
            }
        }
        CompanyDataAccess _CompanyDataAccess
        {
            get
            {
                return (CompanyDataAccess)_ClientContext[typeof(CompanyDataAccess)];
            }
        }
        public TechSchedule GetById(int value)
        {
            return _TechScheduleDataAccess.Get(value);
        }
        public List<TechSchedule> GetAllTechSchedule()
        {
            return _TechScheduleDataAccess.GetAll();
        }
        public List<TechSchedule> GetAllTechScheduleByEmployeeId(Guid customerId,Guid companyId)
        {

            DataTable dt = _TechScheduleDataAccess.GetAllTechScheduleByEmployeeId(customerId, companyId);
            List<TechSchedule> TechScheduleList = new List<TechSchedule>();
            TechScheduleList = (from DataRow dr in dt.Rows
                            select new TechSchedule()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                TechnicianName = dr["TechnicianName"].ToString(),
                                ArrivalTime = dr["ArrivalTime"].ToString(),
                                EstimatedArrival = dr["EstimatedArrival"].ToString(),
                                DepartureTime = dr["DepartureTime"].ToString(),
                                EmployeeId = (Guid)dr["EmployeeId"],
                                CustomerId = (Guid)dr["CustomerId"],
                                CompanyId = (Guid)dr["CompanyId"],
                                InstallDate = dr["InstallDate"] != DBNull.Value ? Convert.ToDateTime(dr["InstallDate"]) : new DateTime(),
                                CheckScheduleConflict = dr["CheckScheduleConflict"] != DBNull.Value ? Convert.ToBoolean(dr["CheckScheduleConflict"]) : false,
                                IsSendNotification = dr["IsSendNotification"] != DBNull.Value ? Convert.ToBoolean(dr["IsSendNotification"]) : false,
                                 
                            }).ToList();
            return TechScheduleList;

            //return _TechScheduleDataAccess.GetByQuery(string.Format(" EmployeeId = '{0}'", employeeId));
        }

        public TechSchedule GetTechScheduleById(int value)
        {
            return _TechScheduleDataAccess.Get(value);
        }
        public bool UpdateTechSchedule(TechSchedule ts)
        {
            return _TechScheduleDataAccess.Update(ts) > 0;
        }
        public long InsertTechSchedule(TechSchedule ts)
        {
            return _TechScheduleDataAccess.Insert(ts);
        }
        public List<Employee> GetAllEmployeeByEmployeeId(Guid employeeId)
        {
            return _EmployeeDataAccess.GetByQuery(string.Format(" EmployeeId= '{0}'", employeeId));
        }
        public bool DeleteTechSchedule(int techId)
        {
            return _TechScheduleDataAccess.Delete(techId) > 0;
        }
        
        public List<TechSchedule> GetAllTechSettingByCompanyId(Guid companyid)
        {
            return _TechScheduleDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", companyid));
        }

        public List<LeadTechScheduleCalendar> GetAllLeadTechScheduleByCompanyid(Guid companyid)
        {
            DataTable dt = _TechScheduleDataAccess.GetAllLeadTechScheduleByCompanyid(companyid);
            List<LeadTechScheduleCalendar> LeadTechScheduleList = new List<LeadTechScheduleCalendar>();
            LeadTechScheduleList = (from DataRow dr in dt.Rows
                                select new LeadTechScheduleCalendar()
                                {
                                   EventStartTime = dr["EventStartTime"].ToString(),
                                   EventEndTime = dr["EventEndTime"].ToString(),
                                    EventTime = dr["EventTime"].ToString(),
                                    EventDate = dr["EventDate"] != DBNull.Value ? Convert.ToDateTime(dr["EventDate"]) : new DateTime(),
                                    EventName = dr["EventName"].ToString(),
                                    EventId = dr["EventId"] != DBNull.Value ? Convert.ToInt32(dr["EventId"]) : 0,
                                    EventCustomer = dr["EventCustomer"] != DBNull.Value ? Convert.ToInt32(dr["EventCustomer"]) : 0
                                }).ToList();
            return LeadTechScheduleList;
        }
    }
}
