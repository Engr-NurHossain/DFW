using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
    public partial class CustomerNote
    {
        public List<CustomerNote> CustomerNotesList { get; set; }
        public string Color { get; set; }
        public bool IsInstantNotification { get; set; }
        public int ReplyCount { get; set; }
        public string NoteTypeValue { get; set; }
        public string Employeename { get; set; }
        public Guid EmployeeID { get; set; }
        public string AssignName { get; set; }
        public string cusName { get; set; }
        public string empName { get; set; }
        public string[] EmpIdList { get; set; }
        public List<AssignEmployeeCustomerNote> AssignEmpList { get; set; }
        public List<TaskNote> TaskNoteList { get; set; }
        public int PersonCount { get; set; }
        public int PersonCountFollowup { get; set; }
        public string[] cusIdVal { get; set; }
        public string[] cusIdValFollowup { get; set; }
        public string RemainderTime { get; set; }
        public string EmpId { get; set; }
        #region ForSending Scheduled Reminder
        public string ConStr { set; get; }
        #endregion
        public List<NoteAssign> ListNoteAssign { get; set; }
        public int LeadNoteCount { get; set; }
        public int TotalNoteCount { get; set; }
        public DateTime? reminderdatetimeforlog { get; set; }
    }

    public partial class AssignEmployeeCustomerNote
    {
        public Guid AssignedEmpId { get; set; }
    }
 
}
