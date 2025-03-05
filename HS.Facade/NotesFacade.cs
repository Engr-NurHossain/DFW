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
    public class NotesFacade : BaseFacade
    {
        public NotesFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        CustomerNoteDataAccess _CustomerNoteDataAccess
        {
            get
            {
                return (CustomerNoteDataAccess)_ClientContext[typeof(CustomerNoteDataAccess)];
            }
        }
        MissingNoteDataAccess _MissingNoteDataAccess
        {
            get
            {
                return (MissingNoteDataAccess)_ClientContext[typeof(MissingNoteDataAccess)];
            }
        }
        OrganizationDataAccess _OrganizationDataAccess
        {
            get
            {
                return (OrganizationDataAccess)_ClientContext[typeof(OrganizationDataAccess)];
            }
        }
        CustomerDataAccess _CustomerDataAccess
        {
            get
            {
                return (CustomerDataAccess)_ClientContext[typeof(CustomerDataAccess)];
            }
        }
        NoteAssignDataAccess _NoteAssignDataAccess
        {
            get
            {
                return (NoteAssignDataAccess)_ClientContext[typeof(NoteAssignDataAccess)];
            }
        }
        TaskNoteDataAccess _TaskNoteDataAccess
        {
            get
            {
                return (TaskNoteDataAccess)_ClientContext[typeof(TaskNoteDataAccess)];
            }
        }
        public CustomerNote GetById(int value)
        {
            return _CustomerNoteDataAccess.Get(value);
        }
        public List<CustomerNote> GetAllCustomerNote()
        {
            return _CustomerNoteDataAccess.GetAll();
        }
        public List<CustomerNote> GetAssignedNotesListByCustomerId(Guid customerId)
        {
            return _CustomerNoteDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerId)).ToList();
        }
        public List<CustomerNote> GetNotesListByCustomerId(Guid customerId, Guid CompanyId)
        {

            DataTable dt = _CustomerNoteDataAccess.GetAssignedNotesListByCustomerId(customerId, CompanyId);
            List<CustomerNote> NoteList = new List<CustomerNote>();
            NoteList = (from DataRow dr in dt.Rows
                        select new CustomerNote()
                        {
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                            Notes = dr["Notes"].ToString(),
                            NoteTypeValue = dr["NoteTypeValue"].ToString(),
                            Color = dr["Color"].ToString(),
                            ReminderDate = dr["ReminderDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReminderDate"]) : new DateTime(),
                            ReminderEndDate = dr["ReminderEndDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReminderEndDate"]) : new DateTime(),
                            CustomerId = (Guid)dr["CustomerId"],
                            CompanyId = (Guid)dr["CompanyId"],
                            CreatedByUid = (Guid)dr["CreatedByUid"],
                            CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                            IsEmail = dr["IsEmail"] != DBNull.Value ? Convert.ToBoolean(dr["IsEmail"]) : false,
                            IsText = dr["IsText"] != DBNull.Value ? Convert.ToBoolean(dr["IsText"]) : false,
                            IsShedule = dr["IsShedule"] != DBNull.Value ? Convert.ToBoolean(dr["IsShedule"]) : false,
                            IsFollowUp = dr["IsFollowUp"] != DBNull.Value ? Convert.ToBoolean(dr["IsFollowUp"]) : false,
                            IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
                            IsClose = dr["IsClose"] != DBNull.Value ? Convert.ToBoolean(dr["IsClose"]) : false,
                            IsAllDay = dr["IsAllDay"] != DBNull.Value ? Convert.ToBoolean(dr["IsAllDay"]) : false,
                            IsPin = dr["IsPin"] != DBNull.Value ? Convert.ToBoolean(dr["IsPin"]) : false,
                            CreatedBy = dr["CreatedBy"].ToString(),
                            empName = dr["empName"].ToString(),
                            AssignName = dr["AssignName"].ToString().TrimEnd(' ', ','),
                            IsOverview = dr["IsOverview"] != DBNull.Value ? Convert.ToBoolean(dr["IsOverview"]) : false,
                            ReferenceTicketId = dr["ReferenceTicketId"] != DBNull.Value ? Convert.ToInt32(dr["ReferenceTicketId"]) : 0,
                        }).ToList();
            return NoteList;
        }
        public List<CustomerNote> GetAllCustomerNoteByCustomerId(Guid CustomerId, Guid CompanyId)
        {
            DataTable dt = _CustomerNoteDataAccess.GetAllCustomerNoteByCustomerId(CustomerId, CompanyId);
            List<CustomerNote> NoteList = new List<CustomerNote>();
            NoteList = (from DataRow dr in dt.Rows
                        select new CustomerNote()
                        {
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                            Notes = dr["Notes"].ToString(),
                            NoteTypeValue = dr["NoteTypeValue"].ToString(),
                            Color = dr["Color"].ToString(),
                            ReminderDate = dr["ReminderDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReminderDate"]) : new DateTime(),
                            ReminderEndDate = dr["ReminderEndDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReminderEndDate"]) : new DateTime(),
                            CustomerId = (Guid)dr["CustomerId"],
                            CompanyId = (Guid)dr["CompanyId"],
                            CreatedByUid = (Guid)dr["CreatedByUid"],
                            CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                            IsEmail = dr["IsEmail"] != DBNull.Value ? Convert.ToBoolean(dr["IsEmail"]) :false,
                            IsText = dr["IsText"] != DBNull.Value ? Convert.ToBoolean(dr["IsText"]) : false,
                            IsShedule = dr["IsShedule"] != DBNull.Value ? Convert.ToBoolean(dr["IsShedule"]) : false,
                            IsFollowUp = dr["IsFollowUp"] != DBNull.Value ? Convert.ToBoolean(dr["IsFollowUp"]) : false,
                            IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
                            IsClose = dr["IsClose"] != DBNull.Value ? Convert.ToBoolean(dr["IsClose"]) : false,
                            IsAllDay = dr["IsAllDay"] != DBNull.Value ? Convert.ToBoolean(dr["IsAllDay"]) : false,
                            IsPin = dr["IsPin"] != DBNull.Value ? Convert.ToBoolean(dr["IsPin"]) : false,
                            CreatedBy = dr["CreatedBy"].ToString(),
                            empName = dr["empName"].ToString(),
                            AssignName = dr["AssignName"].ToString().TrimEnd(' ', ','),
                            IsOverview = dr["IsOverview"] != DBNull.Value ? Convert.ToBoolean(dr["IsOverview"]) : false,
                            ReferenceTicketId = dr["ReferenceTicketId"] != DBNull.Value ? Convert.ToInt32(dr["ReferenceTicketId"]) : 0,
                        }).ToList();
            return NoteList;
            //return _CustomerNoteDataAccess.GetByQuery(string.Format(" CustomerId = '{0}' AND CompanyId='{1}' order by CreatedDate desc", CustomerId, CompanyId));
        }

        public CustomerNote GetCustomerNoteByThirdPartyId(int noteID)
        {
            return _CustomerNoteDataAccess.GetByQuery(string.Format("ThirdPartyId ='{0}'",noteID)).FirstOrDefault();
        }

        public List<CustomerNote> GetAllCustomerNotesByCustomerId(Guid CustomerId, Guid CompanyId, int pageno, int pagesize,DateTime? StartDate,DateTime? EndDate,string SearchText)
        {
            DataTable dt = _CustomerNoteDataAccess.GetAllCustomerNotesByCustomerId(CustomerId, CompanyId, pageno, pagesize, StartDate, EndDate, SearchText);
            List<CustomerNote> NoteList = new List<CustomerNote>();
           NoteList = (from DataRow dr in dt.Rows
                        select new CustomerNote()
                        {
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                            Notes = dr["Notes"].ToString(),
                            ReplyCount = dr["ReplyCount"] != DBNull.Value ? Convert.ToInt32(dr["ReplyCount"]) : 0,
                            NoteTypeValue = dr["NoteTypeValue"].ToString(),
                            Color = dr["Color"].ToString(),
                            ReminderDate = dr["ReminderDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReminderDate"]) : new DateTime(),
                            ReminderEndDate = dr["ReminderEndDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReminderEndDate"]) : new DateTime(),
                            CustomerId = (Guid)dr["CustomerId"],
                            CompanyId = (Guid)dr["CompanyId"],
                            CreatedByUid = (Guid)dr["CreatedByUid"],
                            CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                            IsEmail = dr["IsEmail"] != DBNull.Value ? Convert.ToBoolean(dr["IsEmail"]) : false,
                            IsText = dr["IsText"] != DBNull.Value ? Convert.ToBoolean(dr["IsText"]) : false,
                            IsShedule = dr["IsShedule"] != DBNull.Value ? Convert.ToBoolean(dr["IsShedule"]) : false,
                            IsFollowUp = dr["IsFollowUp"] != DBNull.Value ? Convert.ToBoolean(dr["IsFollowUp"]) : false,
                            IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
                            IsClose = dr["IsClose"] != DBNull.Value ? Convert.ToBoolean(dr["IsClose"]) : false,
                            IsAllDay = dr["IsAllDay"] != DBNull.Value ? Convert.ToBoolean(dr["IsAllDay"]) : false,
                            IsPin = dr["IsPin"] != DBNull.Value ? Convert.ToBoolean(dr["IsPin"]) : false,
                            CreatedBy = dr["CreatedBy"].ToString(),
                            empName = dr["empName"].ToString(), 
                            AssignName = dr["AssignName"].ToString().TrimEnd(' ', ','),
                            IsOverview = dr["IsOverview"] != DBNull.Value ? Convert.ToBoolean(dr["IsOverview"]) : false,
                            TotalNoteCount = dr["TotalNoteCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalNoteCount"]) : 0,
                        }).ToList();
            return NoteList;
            //return _CustomerNoteDataAccess.GetByQuery(string.Format(" CustomerId = '{0}' AND CompanyId='{1}' order by CreatedDate desc", CustomerId, CompanyId));
        }

        public CustomerNote GetNotesById(int value)
        {
            var result = _CustomerNoteDataAccess.Get(value);
            return result;
        }
        public bool UpdateNotes(CustomerNote cn)
        {
            return _CustomerNoteDataAccess.Update(cn) > 0;
        }
        public long InsertCustomerNote(CustomerNote cn)
        {
            return _CustomerNoteDataAccess.Insert(cn);
        }

        public long InsertMissingNote(MissingNote note)
        {
            return _MissingNoteDataAccess.Insert(note);
        }
        public List<CustomerNote> GetAllCustomerNoteByCompanyId(Guid CompanyId)
        {
            return _CustomerNoteDataAccess.GetByQuery(string.Format(" CompanyId = '{0}'", CompanyId));
        }

        public int InsertTaskNote(TaskNote tNote)
        {
            return (int)_TaskNoteDataAccess.Insert(tNote);
        }
        public bool UpdateTaskNote(TaskNote tNote)
        {
            return _TaskNoteDataAccess.Update(tNote) > 0;
        }
        public TaskNote GetTaskNoteById(int id)
        {
            var result = _TaskNoteDataAccess.Get(id);
            return result;
        }
        public List<TaskNote> GetAllTaskNoteByTaskIdAndCompanyId(int TaskId, Guid ComId)
        {
            DataTable dt = _CustomerNoteDataAccess.GetAllTaskNoteByTaskIdAndCompanyId(TaskId, ComId);
            List<TaskNote> NoteList = new List<TaskNote>();
            if (dt != null)
            {
                NoteList = (from DataRow dr in dt.Rows
                            select new TaskNote()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                AddedBy = (Guid)dr["AddedBy"],
                                AddedByText = dr["AddedByText"].ToString(),
                                AddedDate = dr["AddedDate"] != DBNull.Value ? Convert.ToDateTime(dr["AddedDate"]) : new DateTime(),
                                CompanyId = (Guid)dr["CompanyId"],
                                TaskId = dr["TaskId"] != DBNull.Value ? Convert.ToInt32(dr["TaskId"]) : 0,
                                Note = dr["Note"].ToString(),

                            }).ToList();
            }
            return NoteList;
        }
        public bool DeleteCustomerNote(int techId)
        {
            return _CustomerNoteDataAccess.Delete(techId) > 0;
        }
        public List<CustomerNote> GetNoteidByCustomerNoteId()
        {
            DataTable dt = _CustomerNoteDataAccess.GetNoteidByCustomerNoteId();
            List<CustomerNote> noteidlist = new List<CustomerNote>();
            noteidlist = (from DataRow dr in dt.Rows
                          select new CustomerNote()
                          {
                              Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                          }).ToList();
            return noteidlist;
        }
        public CustomerNote GetAllNotesByCustomerNoteId(int id)
        {
            DataTable dt = _CustomerNoteDataAccess.GetAllNotesByCustomerNoteId(id);
            List<CustomerNote> NoteList = new List<CustomerNote>();
            NoteList = (from DataRow dr in dt.Rows
                        select new CustomerNote()
                        {
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                            Notes = dr["Notes"].ToString(),
                            TeamSettingId = dr["TeamSettingId"] != DBNull.Value ? Convert.ToInt32(dr["TeamSettingId"]) : 0,
                            NoteType = dr["NoteType"].ToString(),
                            Color = dr["Color"].ToString(),
                            ReminderDate = dr["ReminderDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReminderDate"]) : new DateTime(),
                            CustomerId = (Guid)dr["CustomerId"],
                            CompanyId = (Guid)dr["CompanyId"],
                            CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                            AssignName = dr["AssignName"].ToString(), 
                            EmployeeID = dr["EmployeeId"] != DBNull.Value ? (Guid)(dr["EmployeeId"]) : new Guid(),
                            //myNewRow.myGuidColumn = myGuid == null ? (object)DBNull.Value : myGuid.Value
                            IsEmail = (dr["IsEmail"] != DBNull.Value ? Convert.ToBoolean(dr["IsEmail"]) : false),
                            IsText = (dr["IsText"] != DBNull.Value ? Convert.ToBoolean(dr["IsText"]) : false),
                            IsClose = (dr["IsClose"] != DBNull.Value ? Convert.ToBoolean(dr["IsClose"]) : false),
                            IsPin = (dr["IsPin"] != DBNull.Value ? Convert.ToBoolean(dr["IsPin"]) : false),
                            IsOverview = (dr["IsOverview"] != DBNull.Value ? Convert.ToBoolean(dr["IsOverview"]) : false),
                            CreatedByUid = (Guid)dr["CreatedByUid"]
                        }).ToList();
            return NoteList[0];
        }

        public List<CustomerNote> GetAllCustomerNotesByCustomerNoteId(int id)
        {
            DataTable dt = _CustomerNoteDataAccess.GetAllNotesByCustomerNoteId(id);
            List<CustomerNote> NoteList = new List<CustomerNote>();
            NoteList = (from DataRow dr in dt.Rows
                        select new CustomerNote()
                        {
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                            Notes = dr["Notes"].ToString(),
                            ReminderDate = dr["ReminderDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReminderDate"]) : new DateTime(),
                            CustomerId = (Guid)dr["CustomerId"],
                            CompanyId = (Guid)dr["CompanyId"],
                            CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                            AssignName = dr["AssignName"].ToString(),
                            EmployeeID = dr["EmployeeId"] != DBNull.Value ? (Guid)(dr["EmployeeId"]) : new Guid(),
                            //myNewRow.myGuidColumn = myGuid == null ? (object)DBNull.Value : myGuid.Value
                            IsEmail = (dr["IsEmail"] != DBNull.Value ? Convert.ToBoolean(dr["IsEmail"]) : false),
                            IsText = (dr["IsText"] != DBNull.Value ? Convert.ToBoolean(dr["IsText"]) : false),
                        }).ToList();
            return NoteList;
        }
        public CustomerNote GetCustomerNotesByCustomerNoteId(int id)
        {
            DataTable dt = _CustomerNoteDataAccess.GetTopNotesByCustomerNoteId(id);
            List<CustomerNote> NoteList = new List<CustomerNote>();
            NoteList = (from DataRow dr in dt.Rows
                        select new CustomerNote()
                        {
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                            Notes = dr["Notes"].ToString(),
                            ReminderDate = dr["ReminderDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReminderDate"]) : new DateTime(),
                            CustomerId = (Guid)dr["CustomerId"],
                            CompanyId = (Guid)dr["CompanyId"],
                            CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                            AssignName = dr["AssignName"].ToString(),
                            EmployeeID = dr["EmployeeId"] != DBNull.Value ? (Guid)(dr["EmployeeId"]) : new Guid(),
                            //myNewRow.myGuidColumn = myGuid == null ? (object)DBNull.Value : myGuid.Value
                            IsEmail = (dr["IsEmail"] != DBNull.Value ? Convert.ToBoolean(dr["IsEmail"]) : false),
                            IsText = (dr["IsText"] != DBNull.Value ? Convert.ToBoolean(dr["IsText"]) : false),
                            IsShedule = (dr["IsShedule"] != DBNull.Value ? Convert.ToBoolean(dr["IsShedule"]) : false),

                        }).ToList();
            CustomerNote customerNote = new CustomerNote();
            customerNote.CustomerNotesList = NoteList;
            return customerNote;
        }
        public List<CustomerNote> GetNoteAssignEmployeeIdByNoteId(int id)
        {
            return _CustomerNoteDataAccess.GetByQuery(string.Format("Id = '{0}'", id)).ToList();
        }
        public List<CustomerNote> GetCustomerNoteByNoteType(string NoteType)
        {
            return _CustomerNoteDataAccess.GetByQuery(string.Format("NoteType = '{0}'", NoteType)).ToList();
        }

        public List<CustomerNote> GetNotesByCustomerCompany(Guid customerid, Guid companyid)
        {
            DataTable dt = _CustomerNoteDataAccess.GetNotesByCustomerCompany(companyid, customerid);
            List<CustomerNote> LeadNoteList = new List<CustomerNote>();
            LeadNoteList = (from DataRow dr in dt.Rows
                            select new CustomerNote()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                Notes = dr["Notes"].ToString(),
                                ReminderDate = dr["ReminderDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReminderDate"]) : new DateTime(),
                                CustomerId = (Guid)dr["CustomerId"],
                                CompanyId = (Guid)dr["CompanyId"],
                                CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                IsEmail = (dr["IsEmail"] != DBNull.Value ? Convert.ToBoolean(dr["IsEmail"]) : false),
                                IsText = (dr["IsText"] != DBNull.Value ? Convert.ToBoolean(dr["IsText"]) : false),
                                IsShedule = (dr["IsShedule"] != DBNull.Value ? Convert.ToBoolean(dr["IsShedule"]) : false),
                            }).ToList();
            return LeadNoteList;
        }

        public List<CustomerNote> GetLeadNotesByCustomerIdandCompanyId(Guid customerid, Guid companyid)
        {
            DataTable dt = _CustomerNoteDataAccess.GetLeadNotesByCustomerIdandCompanyId(companyid, customerid);
            List<CustomerNote> LeadNoteList = new List<CustomerNote>();
            LeadNoteList = (from DataRow dr in dt.Rows
                            select new CustomerNote()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                Notes = dr["Notes"].ToString(),
                                ReminderDate = dr["ReminderDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReminderDate"]) : new DateTime(),
                                CustomerId = (Guid)dr["CustomerId"],
                                CompanyId = (Guid)dr["CompanyId"],
                                CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                IsEmail = (dr["IsEmail"] != DBNull.Value ? Convert.ToBoolean(dr["IsEmail"]) : false),
                                IsText = (dr["IsText"] != DBNull.Value ? Convert.ToBoolean(dr["IsText"]) : false),
                                IsShedule = (dr["IsShedule"] != DBNull.Value ? Convert.ToBoolean(dr["IsShedule"]) : false),
                                EmployeeID = dr["EmployeeID"] != DBNull.Value ? (Guid)dr["EmployeeId"]:Guid.Empty
                            }).ToList();
            return LeadNoteList;
        }

        public List<CustomerNote> GetAllCustomerNoteByCompanyIdAndIsSchedule(Guid CompanyId, string DateToday, string Constr)
        {
            return _CustomerNoteDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and IsShedule = 'true' and ReminderDate >= '{1}'", CompanyId, DateToday));
        }

        public List<CustomerNote> GetReminderScheduleByReminderDateAndEmployeeId(string datetime, Guid empid, string enddate)
        {
            DataTable dt = _CustomerNoteDataAccess.GetReminderScheduleByReminderDateAndEmployeeId(datetime, empid, enddate);
            List<CustomerNote> LeadNoteList = new List<CustomerNote>();
            LeadNoteList = (from DataRow dr in dt.Rows
                            select new CustomerNote()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                Notes = dr["Notes"].ToString()
                            }).ToList();
            return LeadNoteList;
        }

        public List<CustomerNote> GetReminderSchedule1ByReminderDateAndEmployeeId(string datetime, Guid empid, string enddate)
        {
            DataTable dt = _CustomerNoteDataAccess.GetReminderSchedule1ByReminderDateAndEmployeeId(datetime, empid, enddate);
            List<CustomerNote> LeadNoteList = new List<CustomerNote>();
            LeadNoteList = (from DataRow dr in dt.Rows
                            select new CustomerNote()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                Notes = dr["Notes"].ToString()
                            }).ToList();
            return LeadNoteList;
        }

        public ReminderCounter GetCountCustomerNoteByCompanyId (Guid comid, string startdate, string enddate)
        {
            DataTable dt = _CustomerNoteDataAccess.GetCountCustomerNoteByCompanyId(comid,startdate, enddate);
            ReminderCounter LeadNoteList = new ReminderCounter();
            LeadNoteList = (from DataRow dr in dt.Rows
                            select new ReminderCounter()
                            {
                                ReminderCount = dr["ReminderCount"] != DBNull.Value ? Convert.ToInt32(dr["ReminderCount"]) : 0
                            }).FirstOrDefault();
            return LeadNoteList;
        }
    }
}
