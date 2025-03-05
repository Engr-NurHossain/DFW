using HS.DataAccess;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HS.Entities;
using System.Data;

namespace HS.Facade
{
    public class InvoiceNoteFacade : BaseFacade
    {
        public InvoiceNoteFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        #region DataAccess
        InvoiceDetailDataAccess _InvoiceDetailDataAccess
        {
            get
            {
                return (InvoiceDetailDataAccess)_ClientContext[typeof(InvoiceDetailDataAccess)];
            }
        }

        InvoiceDataAccess _InvoiceDataAccess
        {
            get
            {
                return (InvoiceDataAccess)_ClientContext[typeof(InvoiceDataAccess)];
            }
        }
        InvoiceNoteDataAccess _InvoiceNoteDataAccess
        {
            get
            {
                return (InvoiceNoteDataAccess)_ClientContext[typeof(InvoiceNoteDataAccess)];
            }
        }
        #endregion

        public int InsertInvoiceNote(InvoiceNote invNote)
        {
            return (int)_InvoiceNoteDataAccess.Insert(invNote);
        }

        public List<InvoiceNote> GetAllInvoiceNote()
        {
            return _InvoiceNoteDataAccess.GetAll();
        }

        public List<InvoiceNote> GetAllInvoiceNoteByInvoiceIdAndCompanyId(int InvoiceId, Guid ComId)
        {
            DataTable dt = _InvoiceNoteDataAccess.GetAllInvoiceNoteByInvoiceIdAndCompanyId(InvoiceId, ComId);
            List<InvoiceNote> NoteList = new List<InvoiceNote>();
            if (dt != null)
            {
                NoteList = (from DataRow dr in dt.Rows
                            select new InvoiceNote()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                AddedBy = (Guid)dr["AddedBy"],
                                AddedByText = dr["AddedByText"].ToString(),
                                AddedDate = dr["AddedDate"] != DBNull.Value ? Convert.ToDateTime(dr["AddedDate"]) : new DateTime(),
                                CompanyId = (Guid)dr["CompanyId"],
                                InvoiceId = dr["InvoiceId"] != DBNull.Value ? Convert.ToInt32(dr["InvoiceId"]) : 0,
                                Note = dr["Note"].ToString(),
                                
                            }).ToList();
            }
            return NoteList;
        }


    }
}
