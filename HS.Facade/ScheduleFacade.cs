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
    public class ScheduleFacade : BaseFacade
    {
            public ScheduleFacade(ClientContext clientContext)
            : base(clientContext)
        {
           
        }
        ScheduleDataAccess _ScheduleDataAccess
        {
            get
            {
                return (ScheduleDataAccess)_ClientContext[typeof(ScheduleDataAccess)];
            }
        }
        CustomerAppointmentDataAccess _CustomerAppointmentDataAccess
        {
            get
            {
                return (CustomerAppointmentDataAccess)_ClientContext[typeof(CustomerAppointmentDataAccess)];
            }
        }
        CalendarEmployeeDataMapperDataAccess _CalendarEmployeeDataMapperDataAccess
        {
            get
            {
                return (CalendarEmployeeDataMapperDataAccess)_ClientContext[typeof(CalendarEmployeeDataMapperDataAccess)];
            }
        }
        public long InsertSchedule(Schedule s)
        {
            return _ScheduleDataAccess.Insert(s);
        }

        public bool UpdateSchedule(Schedule supdate)
        {
            return _ScheduleDataAccess.Update(supdate) > 0;
        }

        public Schedule GetByQA1Id(int value)
        {
            return _ScheduleDataAccess.GetByQuery(string.Format("LeadId = '{0}' and Type = 'QA1'", value)).FirstOrDefault();
        }
        public Schedule GetByQA2Id(int value)
        {
            return _ScheduleDataAccess.GetByQuery(string.Format("LeadId = '{0}' and Type = 'QA2'", value)).FirstOrDefault();
        }

        public ScheduleCalendarList GetScheduleListByCompanyId(Guid companyid, string emptag, string empid, int ResourceLimit, int pageno, bool ReminderResult, int pageno1, string startdate, string defaultView, string typeval, string Userval, string EventUserId, string TicketId, bool ispermit)
        {
            DataSet ds = _ScheduleDataAccess.GetScheduleListByCompanyId(companyid, emptag, empid, ResourceLimit, pageno, ReminderResult, pageno1, startdate, defaultView, typeval, Userval, EventUserId, TicketId, ispermit);
            ScheduleCalendarList model = new ScheduleCalendarList();
            model.ListFollowUpSchedule = (from DataRow dr in ds.Tables[0].Rows
                                          select new PreCustomerNote()
                                          {
                                              EventTitle = dr["EventTitle"].ToString(),
                                              EventStartDate = dr["EventType"].ToString() == "Reminder" ? (dr["EventStartDate"] != DBNull.Value ? Convert.ToDateTime(dr["EventStartDate"]).UTCToClientTime().ToString() : new DateTime().ToString()) : dr["EventStartDate"].ToString(),  
                                              EventEndDate = dr["EventType"].ToString() == "Reminder" ? (dr["EventEndDate"] != DBNull.Value ? Convert.ToDateTime(dr["EventEndDate"]).UTCToClientTime().ToString() : new DateTime().ToString()) : dr["EventEndDate"].ToString(),
                                              EventType = dr["EventType"].ToString(),
                                              EventLeadId = dr["EventLeadId"] != DBNull.Value ? Convert.ToInt32(dr["EventLeadId"]) : 0,
                                              EventCusId = dr["EventCusId"].ToString(),
                                              EventCustomId = dr["EventCustomId"].ToString(),
                                              EventResourceName = dr["EventResourceName"].ToString(),
                                              EventIsCalendar = dr["EventIsCalendar"] != DBNull.Value ? Convert.ToBoolean(dr["EventIsCalendar"]) : false,
                                              EventColor = dr["EventColor"].ToString(),
                                              EventId = dr["EventId"] != DBNull.Value ? Convert.ToInt32(dr["EventId"]) : 0,
                                              EventCustomerName = dr["EventCustomerName"].ToString(),
                                              EventAllDay = dr["EventAllDay"] != DBNull.Value ? Convert.ToBoolean(dr["EventAllDay"]) : false,
                                              EventAppid = dr["EventAppid"].ToString(),
                                              EventIsLead = dr["EventIsLead"] != DBNull.Value ? Convert.ToBoolean(dr["EventIsLead"]) : false,
                                              EventStreet = dr["EventStreet"].ToString(),
                                              EventLocate = dr["EventLocate"].ToString(),
                                              EventDate = dr["EventDate"].ToString(),
                                              EventCalendarCount = dr["EventCalendarCount"].ToString(),
                                              EventTicketId = dr["EventTicketId"].ToString(),
                                              EventStatus = dr["EventStatus"].ToString(),
                                              HoverTitle = dr["HoverTitle"].ToString(),
                                              EventDisplayType = dr["EventDisplayType"].ToString(),
                                              EventDBA = dr["EventDBA"].ToString(),
                                              EventBookingId = dr["EventBookingId"].ToString(),
                                              EventAdditionalMember = dr["EventAdditionalMember"].ToString(),
                                              EventRescheduleId = dr["EventRescheduleId"] != DBNull.Value ? Convert.ToInt32(dr["EventRescheduleId"]) : 0,
                                              EventBusinessName = dr["EventBusinessName"].ToString()
                                          }).ToList();
            model.EventCount = (from DataRow dr in ds.Tables[1].Rows
                                select new EventCount()
                                {
                                    EventTotalCount = dr["EventTotalCount"] != DBNull.Value ? Convert.ToInt32(dr["EventTotalCount"]) : 0,
                                }).FirstOrDefault();
            model.EventFilterCount = (from DataRow dr in ds.Tables[2].Rows
                                select new EventFilterCount()
                                {
                                    EventCounter = dr["EventCounter"] != DBNull.Value ? Convert.ToInt32(dr["EventCounter"]) : 0,
                                }).FirstOrDefault();
            model.ListEmployeeCalendar = (from DataRow dr in ds.Tables[3].Rows
                                          select new Employee()
                                          {
                                              Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                              UserId = (Guid)dr["UserId"],
                                              FirstName = dr["FirstName"].ToString(),
                                              LastName = dr["LastName"].ToString(),
                                              EMPName = dr["EMPName"].ToString()
                                          }).ToList();
            model.ListUserIdHaveEvent = (from DataRow dr in ds.Tables[4].Rows
                                         select new Employee()
                                         {
                                             UserId = (Guid)dr["UserId"]
                                         }).ToList();
            return model;
        }

        public ScheduleCalendarList GetUserListByCompanyIdHaveEvent(Guid companyid, string startdate, string defaultView)
        {
            DataSet ds = _ScheduleDataAccess.GetUserListByCompanyIdHaveEvent(companyid, startdate, defaultView);
            ScheduleCalendarList model = new ScheduleCalendarList();
            model.ListUserIdHaveEvent = (from DataRow dr in ds.Tables[0].Rows
                                         select new Employee()
                                         {
                                             UserId = (Guid)dr["UserId"]
                                         }).ToList();
            return model;
        }
        public List<CustomCalendarTicketStatus> GetCustomCalenderStatusByCompanyId(Guid companyid)
        {
            DataSet ds = _ScheduleDataAccess.GetCustomCalenderStatusByCompanyId(companyid);
            List<CustomCalendarTicketStatus> model = new List<CustomCalendarTicketStatus>();
            if (ds != null)
            {
                model = (from DataRow dr in ds.Tables[0].Rows
                         select new CustomCalendarTicketStatus()
                         {
                             TicketStatus = dr["TicketStatus"].ToString(),
                             Filename = dr["Filename"].ToString(),
                             TicketStatusColor = dr["TicketStatusColor"].ToString()
                         }).ToList();
            }
            return model;
        }
        public Schedule GetScheduleByCustomerid(int id)
        {
            return _ScheduleDataAccess.GetByQuery(string.Format("LeadId = '{0}' and Type = 'WorkOrder'", id)).FirstOrDefault();
        }

        public Schedule GetServiceScheduleByCustomerid(int id)
        {
            return _ScheduleDataAccess.GetByQuery(string.Format("LeadId = '{0}' and Type = 'ServiceOrder'", id)).FirstOrDefault();
        }

        public List<CustomerAppointment> GetAppointmentScheduleByAppointmentDatetimeAndEmployeeid(string date, string time, Guid empid, string endtime)
        {
            return _CustomerAppointmentDataAccess.GetByQuery(string.Format("AppointmentDate = '{0}' and AppointmentStartTime <= '{1}' and EmployeeId = '{2}' and AppointmentEndTime >= '{3}'", date, time, empid, endtime)).ToList();
        }
        public List<CustomerAppointment> GetAppointmentScheduleByAppointmentStartAndEndDatetimeAndEmployeeid(string date, string time, Guid empid, string endtime)
        {
            return _CustomerAppointmentDataAccess.GetByQuery(string.Format("AppointmentDate = '{0}' and (AppointmentStartTime between '{1}' and '{3}' or AppointmentEndTime between '{1}' and '{3}') and EmployeeId = '{2}'", date, time, empid, endtime)).ToList();
        }
        public List<CustomerAppointment> GetAppointmentScheduleByAppointmentStartEndDatetimeAndEmployeeid(string date, string time, Guid empid, string endtime)
        {
            return _CustomerAppointmentDataAccess.GetByQuery(string.Format("AppointmentDate = '{0}' and AppointmentStartTime = '{1}' and EmployeeId = '{2}' and AppointmentEndTime = '{3}'", date, time, empid, endtime)).ToList();
        }
        public List<CustomerAppointment> GetAppointmentScheduleByAppointmentEndDatetimeAndEmployeeid(string date, string time, Guid empid)
        {
            return _CustomerAppointmentDataAccess.GetByQuery(string.Format("AppointmentDate = '{0}' and AppointmentEndTime = '{1}' and EmployeeId = '{2}'", date, time, empid)).ToList();
        }

        public List<PreCustomerNote> GetSchedulingByCompanyIdAndFilterForGoogleMap(Guid companyid, string date, string type, string user, bool ispermit, bool IsType)
        {
            DataTable dt = _ScheduleDataAccess.GetSchedulingByCompanyIdAndFilterForGoogleMap(companyid, date, type, user, ispermit, IsType);
            List<PreCustomerNote> model = new List<PreCustomerNote>();
            model = (from DataRow dr in dt.Rows
                                    select new PreCustomerNote()
                                    {
                                        EventTitle = dr["EventTitle"].ToString(),
                                        EventStartDate = dr["EventType"].ToString() == "Reminder" ? (dr["EventStartDate"] != DBNull.Value ? Convert.ToDateTime(dr["EventStartDate"]).UTCToClientTime().ToString() : new DateTime().ToString()) : dr["EventStartDate"].ToString(),
                                        EventEndDate = dr["EventType"].ToString() == "Reminder" ? (dr["EventEndDate"] != DBNull.Value ? Convert.ToDateTime(dr["EventEndDate"]).UTCToClientTime().ToString() : new DateTime().ToString()) : dr["EventEndDate"].ToString(),
                                        EventType = dr["EventType"].ToString(),
                                        EventLeadId = dr["EventLeadId"] != DBNull.Value ? Convert.ToInt32(dr["EventLeadId"]) : 0,
                                        EventCusId = dr["EventCusId"].ToString(),
                                        EventCustomId = dr["EventCustomId"].ToString(),
                                        EventResourceName = dr["EventResourceName"].ToString(),
                                        EventIsCalendar = dr["EventIsCalendar"] != DBNull.Value ? Convert.ToBoolean(dr["EventIsCalendar"]) : false,
                                        EventColor = dr["EventColor"].ToString(),
                                        EventEmployeeColor = dr["CalendarColor"].ToString(),
                                        EventStatusColor = dr["StatusColor"].ToString(),
                                        EventId = dr["EventId"] != DBNull.Value ? Convert.ToInt32(dr["EventId"]) : 0,
                                        EventCustomerName = dr["EventCustomerName"].ToString(),
                                        EventAllDay = dr["EventAllDay"] != DBNull.Value ? Convert.ToBoolean(dr["EventAllDay"]) : false,
                                        EventAppid = dr["EventAppid"].ToString(),
                                        EventIsLead = dr["EventIsLead"] != DBNull.Value ? Convert.ToBoolean(dr["EventIsLead"]) : false,
                                        EventStreet = dr["EventStreet"].ToString(),
                                        EventLocate = dr["EventLocate"].ToString(),
                                        EventDate = dr["EventDate"].ToString(),
                                        EventCalendarCount = dr["EventCalendarCount"].ToString(),
                                        EventTicketId = dr["EventTicketId"].ToString(),
                                        EventStatus = dr["EventStatus"].ToString(),
                                        HoverTitle = dr["HoverTitle"].ToString(),
                                        EventDisplayType = dr["EventDisplayType"].ToString(),
                                        EventDBA = dr["EventDBA"].ToString(),
                                        EventBookingId = dr["EventBookingId"].ToString(),
                                        EventLocationFlag = dr["Latlng"].ToString(),
                                        EventCustomerIntId = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0
                                    }).ToList();
            return model;
        }
        public CustomCalendarAllTaskList GetCustomCalendarScheduleListByCompanyId(Guid companyid, string startdate, string defaultView, string typeval, string Userval, bool IsType, string ControllerName, string skills, int TktId = 0, string CusName = "")
        {
            DataSet ds = _ScheduleDataAccess.GetCustomCalendarScheduleListByCompanyId(companyid, startdate, defaultView, typeval, Userval, IsType, ControllerName, skills, TktId, CusName);
            CustomCalendarAllTaskList model = new CustomCalendarAllTaskList();
            if (ds != null)
            {
                model.CalendarTaskList = (from DataRow dr in ds.Tables[0].Rows
                                          select new CustomCalendarAllRecords()
                                          {
                                              EventLeadId = dr["EventLeadId"] != DBNull.Value ? Convert.ToInt32(dr["EventLeadId"]) : 0,
                                              EventStartDate = dr["EventStartDate"] != DBNull.Value ? Convert.ToDateTime(dr["EventStartDate"]).ToString() : new DateTime().ToString(),
                                              EventEndDate = dr["EventEndDate"] != DBNull.Value ? Convert.ToDateTime(dr["EventEndDate"]).ToString() : new DateTime().ToString(),
                                              EventType = dr["EventType"].ToString(),
                                              EventTicketId = dr["EventTicketId"].ToString(),
                                              EventAppointmentId = dr["EventAppointmentId"].ToString(),
                                              EventDisplayType = dr["EventDisplayType"].ToString(),
                                              EventColor = dr["EventColor"].ToString(),
                                              EventCustomerId = dr["EventCustomerId"].ToString(),
                                              EmployeeName = dr["EmployeeName"].ToString(),
                                              EventEmployeeGuidId = dr["EventEmployeeGuidId"].ToString(),
                                              EmployeeIntId = dr["EmployeeIntId"] != DBNull.Value ? Convert.ToInt32(dr["EmployeeIntId"]) : 0,
                                              CustomerIntId = dr["EventCustomerIntId"] != DBNull.Value ? Convert.ToInt32(dr["EventCustomerIntId"]) : 0,
                                              EventAllDay = dr["EventAllDay"] != DBNull.Value ? Convert.ToBoolean(dr["EventAllDay"]) : false,
                                              //IsCalled = dr["IsCallAhead"] != DBNull.Value ? Convert.ToBoolean(dr["IsCallAhead"]) : false,
                                              TicketAmount = 0,//dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0.00,
                                              EventCustomerName = dr["EventCustomerName"].ToString(),
                                              //EventPhone = dr["CusPhone"].ToString(),
                                              EventStreet = dr["EventStreet"].ToString(),
                                              EventLocate = dr["EventLocate"].ToString(),
                                              EventTicketAddress = dr["EventTicketLocation"].ToString(),
                                              EventDate = dr["EventDate"].ToString(),
                                              EventStatus = dr["EventStatus"].ToString(),
                                              EventSubject = dr["EventSubject"].ToString(),
                                              EventIcon = dr["EventIcon"].ToString(),
                                              BookingId = dr["BookingId"].ToString(),
                                              StatusColor = dr["StatusColor"].ToString(),
                                              EmpColor = dr["CalendarColor"].ToString(),
                                              EventAdditionalMember = dr["EventAdditionalMember"].ToString()
                                          }).ToList();
                model.CalendarEmployeeList = (from DataRow dr in ds.Tables[1].Rows
                                              select new CustomCalendarEmployees()
                                              {
                                                  Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                  //Rank = dr["EmployeeRank"] != DBNull.Value ? Convert.ToInt32(dr["EmployeeRank"]) : 0,
                                                  UserId = dr["UserId"].ToString(),
                                                  GroupName = dr["Name"].ToString(),
                                                  ResourceName = dr["ResourceName"].ToString()
                                              }).OrderBy(x => x.ResourceName).ToList();
            }
            return model;
        }
        
        public CustomCalendarScheduleCalendarList GetCustomCalendarIndexPageDataByCompanyId(Guid companyid)
        {
            DataSet ds = _ScheduleDataAccess.GetCustomCalendarIndexPageDataByCompanyId(companyid);
            CustomCalendarScheduleCalendarList model = new CustomCalendarScheduleCalendarList();
            if (ds != null)
            {                
                model.CalendarGlobalRecords = (from DataRow dr in ds.Tables[0].Rows
                                               select new CustomCalendarGlobalRecords()
                                               {
                                                   SearchKey = dr["SearchKey"].ToString(),
                                                   IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
                                                   Value = dr["Value"].ToString(),
                                                   IsBottom = dr["Description"].ToString(),
                                                   OptionalValue = dr["OptionalValue"].ToString()
                                               }).ToList();
                model.CalendarEmployeeList = (from DataRow dr in ds.Tables[1].Rows
                                              select new CustomCalendarEmployees()
                                              {
                                                  Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                  UserId = dr["UserId"].ToString(),
                                                  ResourceName = dr["ResourceName"].ToString()
                                              }).ToList();
                model.CalendarViewTicketType = (from DataRow dr in ds.Tables[2].Rows
                                                select new CustomCalendarTicketTypes()
                                                {
                                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                    DisplayText = dr["DisplayText"].ToString(),
                                                    DataValue = dr["DataValue"].ToString(),
                                                    AlterDisplayText = dr["AlterDisplayText"].ToString(),
                                                    IsDefaultItem = dr["IsDefaultItem"] != DBNull.Value ? Convert.ToBoolean(dr["IsDefaultItem"]) : false
                                                }).ToList();
                model.CalendarTicketStatus = (from DataRow dr in ds.Tables[3].Rows
                                              select new CustomCalendarTicketStatus()
                                              {
                                                  TicketStatus = dr["TicketStatus"].ToString(),
                                                  Filename = dr["Filename"].ToString(),
                                                  TicketStatusColor = dr["TicketStatusColor"].ToString()
                                              }).ToList();
            }
            return model;
        }
        #region Employee Selection save 
        public bool InsertCalendarMapping(CalendarEmployeeDataMapper model)
        {
            return _CalendarEmployeeDataMapperDataAccess.Insert(model) > 0;
        }
        public bool UpdateCalendarMapping(CalendarEmployeeDataMapper model)
        {
            return _CalendarEmployeeDataMapperDataAccess.Update(model) > 0;
        }
        public List<CalendarEmployeeDataMapper> GetAllCalendarMappingByUserId(Guid UserId)
        {
            return _CalendarEmployeeDataMapperDataAccess.GetByQuery(string.Format("UserId = '{0}'",UserId));
        }
        public List<CalendarEmployeeDataMapper> GetAllActiveCalendarMappingByUserId(Guid UserId)
        {
            return _CalendarEmployeeDataMapperDataAccess.GetByQuery(string.Format("UserId = '{0}' and IsActive = 1", UserId));
        }
        public CalendarEmployeeDataMapper GetCalendarMappingByUserIdandType(Guid UserId, string type)
        {
            return _CalendarEmployeeDataMapperDataAccess.GetByQuery(string.Format("UserId = '{0}' and MapType = '{1}'", UserId, type)).FirstOrDefault();
        }
        public CalendarEmployeeDataMapper GetCalendarMappingByUserIdandEmpId(Guid UserId, Guid Empid)
        {
            return _CalendarEmployeeDataMapperDataAccess.GetByQuery(string.Format("UserId = '{0}' and EmplyeeSelectedId = '{1}'", UserId, Empid)).FirstOrDefault();
        }
        #endregion
    }
}
