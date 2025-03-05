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
    public class NoteAssignFacade : BaseFacade
    {
        public NoteAssignFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        NoteAssignDataAccess _NoteAssignDataAccess
        {
            get
            {
                return (NoteAssignDataAccess)_ClientContext[typeof(NoteAssignDataAccess)];
            }
        }
        EmployeeDataAccess _EmployeeDataAccess
        {
            get
            {
                return (EmployeeDataAccess)_ClientContext[typeof(EmployeeDataAccess)];
            }
        }

        public bool InsertNoteAssign(NoteAssign na)
        {
            return _NoteAssignDataAccess.Insert(na) > 0;
        }

        public List<NoteAssign> GetAllAssignByCustomerIdAndCompanyId(Guid CustomerId, Guid CompanyId)
        {
            DataTable dt = _NoteAssignDataAccess.GetAllAssignByCustomerIdAndCompanyId(CustomerId, CompanyId);
            List<NoteAssign> assignnum = new List<NoteAssign>();
            assignnum = (from DataRow dr in dt.Rows
                         select new NoteAssign()
                         {
                             Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                             NoteId = dr["NoteId"] != DBNull.Value ? Convert.ToInt32(dr["NoteId"]) : 0,
                             EmployeeId = new Guid(dr["EmployeeId"].ToString()),
                             AssignName = dr["AssignName"].ToString()
                         }).ToList();
            return assignnum;
        }

        public NoteAssign GetById(int value)
        {
            return _NoteAssignDataAccess.Get(value);
        }
        public long UpdateNoteAssign(NoteAssign na)
        {
            return _NoteAssignDataAccess.Update(na);
        }
        public List<NoteAssign> GetAllEmployeeNameeByEmployeeId(Guid employeeid)
        {
            DataTable dt = _NoteAssignDataAccess.GetAllEmployeeNameeByEmployeeId(employeeid);
            List<NoteAssign> assignnum = new List<NoteAssign>();
            assignnum = (from DataRow dr in dt.Rows
                         select new NoteAssign()
                         {
                             Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                             NoteId = dr["NoteId"] != DBNull.Value ? Convert.ToInt32(dr["NoteId"]) : 0,
                             EmployeeId = new Guid(dr["EmployeeId"].ToString()),
                             AssignName = dr["AssignName"].ToString()
                         }).ToList();
            return assignnum;
        }

        public NoteAssign GetAssignedNotesByNoteId(int NoteId)
        {
            return _NoteAssignDataAccess.GetByQuery(string.Format("NoteId = '{0}'", NoteId)).FirstOrDefault();
        }

        public List<NoteAssign> GetAssignedNotesListByNoteId(int NoteId)
        {
            return _NoteAssignDataAccess.GetByQuery(string.Format("NoteId = '{0}'", NoteId)).ToList();
        }
     
        public List<AssignEmployeeCustomerNote> GetAllAssignCustomerNoteListByNoteId(int noteId)
        {
            DataTable dt = _NoteAssignDataAccess.GetAllAssignCustomerNoteListByNoteId(noteId);

            List<AssignEmployeeCustomerNote> assignnum = new List<AssignEmployeeCustomerNote>();
            assignnum = (from DataRow dr in dt.Rows
                         select new AssignEmployeeCustomerNote()
                         {
                             AssignedEmpId = new Guid(dr["EmployeeId"].ToString())
                         }).ToList();
            return assignnum;
        }
        public bool CheckAndDeleteOldAssignedEmployee(int NoteId)
        {
            return _NoteAssignDataAccess.CheckAndDeleteOldAssignedEmployee(NoteId);
        }
        public bool DeleteAllAssignNoteByNoteId(int id)
        {
            return _NoteAssignDataAccess.DeleteAllAssignNoteByNoteId(id);
        }

        public string GetAssignEmployeeIdByNotesId(int id)
        {
            return _NoteAssignDataAccess.GetByQuery(string.Format("NoteId = '{0}'", id)).FirstOrDefault().EmployeeId.ToString();
        }
    }
}
