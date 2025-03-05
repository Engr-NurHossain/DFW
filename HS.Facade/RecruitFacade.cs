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
    public class RecruitFacade :BaseFacade
    {
        public RecruitFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }

        RecruitmentFormDataAccess _RecruitmentFormDataAccess
        {
            get
            {
                return (RecruitmentFormDataAccess)_ClientContext[typeof(RecruitmentFormDataAccess)];
            }
        }
        RecruitmentFormEmployeeDataAccess _RecruitmentFormEmployeeDataAccess
        {
            get
            {
                return (RecruitmentFormEmployeeDataAccess)_ClientContext[typeof(RecruitmentFormEmployeeDataAccess)];
            }
        }

        

        RecruitmentW9FormDataAccess _RecruitmentW9FormDataAccess
        {
            get
            {
                return (RecruitmentW9FormDataAccess)_ClientContext[typeof(RecruitmentW9FormDataAccess)];
            }
        }
        RecruitmentW4FormDataAccess _RecruitmentW4FormDataAccess
        {
            get
            {
                return (RecruitmentW4FormDataAccess)_ClientContext[typeof(RecruitmentW4FormDataAccess)];
            }
        }
        Recruitmenti9FormDataAccess _Recruitmenti9FormDataAccess
        {
            get
            {
                return (Recruitmenti9FormDataAccess)_ClientContext[typeof(Recruitmenti9FormDataAccess)];
            }
        }
        RecruitmentDocFormDataAccess _RecruitmentDocFormDataAccess
        {
            get
            {
                return (RecruitmentDocFormDataAccess)_ClientContext[typeof(RecruitmentDocFormDataAccess)];
            }
        }

        public List<RecruitmentForm> GetAllRecruitmentFormsByCompanyId(Guid CompanyId)
        {
            return _RecruitmentFormDataAccess.GetByQuery(string.Format("CompanyId= '{0}'", CompanyId));
        }

        public List<RecruitmentFormEmployee> GetAllEmployeeRecruitmentFormsByEmpId(Guid employeeId)
        {
            //return _RecruitmentFormEmployeeDataAccess.GetByQuery(string.Format("EmployeeId = '{0}'",employeeId));
            DataTable dt =  _RecruitmentFormEmployeeDataAccess.GetAllEmployeeRecruitmentFormsByEmpId(employeeId); 
            List<RecruitmentFormEmployee> Qa1List = new List<RecruitmentFormEmployee>();
            Qa1List = (from DataRow dr in dt.Rows
                       select new RecruitmentFormEmployee()
                       {
                           Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                           EmployeeId = (Guid)dr["EmployeeId"],
                           FormId = (Guid)dr["FormId"],
                           IsFillUp = dr["IsFillUp"] != DBNull.Value ? Convert.ToBoolean(dr["IsFillUp"]) : false,
                           IsSubmitted = dr["IsSubmitted"] != DBNull.Value ? Convert.ToBoolean(dr["IsSubmitted"]) : false,
                           FillDate = dr["FillDate"] != DBNull.Value ? Convert.ToDateTime(dr["FillDate"]) : new DateTime(),
                           SubmitDate = dr["SubmitDate"] != DBNull.Value ? Convert.ToDateTime(dr["SubmitDate"]) : new DateTime(),
                           RecruitmentFormId = dr["RecruitmentFormId"] != DBNull.Value ? Convert.ToInt32(dr["RecruitmentFormId"]) : 0,
                           FormName = dr["FormName"].ToString(),
                       }).ToList();
            return Qa1List;
        }

        public bool DeleteAllRecruitmentFormEmployeeByEmployeeId(Guid employeeId)
        {
            return _RecruitmentFormEmployeeDataAccess.DeleteAllRecruitmentFormEmployeeByEmployeeId(employeeId);
        }

        public int InsertRecruitmentFormEmployee(RecruitmentFormEmployee rFE)
        {
            return (int)_RecruitmentFormEmployeeDataAccess.Insert(rFE);
        }
        public List<RecruitmentForm> GetAllRecruitmentForms()
        {
            return _RecruitmentFormDataAccess.GetAll();
        }

        public int InsertW9Form(RecruitmentW9Form w9Form)
        {
            return (int)_RecruitmentW9FormDataAccess.Insert(w9Form);
        }

        public bool UpdateRecruitmentFormEmployee(RecruitmentFormEmployee rfe)
        {
            return _RecruitmentFormEmployeeDataAccess.Update(rfe)>0;
        }

        public RecruitmentW9Form GetW9FormByFormId(Guid formId)
        {
            return _RecruitmentW9FormDataAccess.GetByQuery(string.Format("FormId = '{0}'",formId)).FirstOrDefault();
        }

        public bool UpdateW9Form(RecruitmentW9Form model)
        {
            return _RecruitmentW9FormDataAccess.Update(model)>0;
        }

        public int InsertW4Form(RecruitmentW4Form w4Form)
        {
            return (int)_RecruitmentW4FormDataAccess.Insert(w4Form);
        }

        public RecruitmentW4Form GetW4FormByFormId(Guid formId)
        {
            return _RecruitmentW4FormDataAccess.GetByQuery(string.Format("FormId = '{0}'", formId)).FirstOrDefault();
        }

        public bool UpdateW4Form(RecruitmentW4Form model)
        {
            return _RecruitmentW4FormDataAccess.Update(model)>0;
        }

        public int InsertI9Form(Recruitmenti9Form i9Form)
        {
            return (int)_Recruitmenti9FormDataAccess.Insert(i9Form);
        }

        public Recruitmenti9Form GetI9FormByFormId(Guid formId)
        {
            return _Recruitmenti9FormDataAccess.GetByQuery(string.Format("FormId = '{0}'", formId)).FirstOrDefault();
        }

        public RecruitmentFormEmployee GetRecruitmentFormEmployeeByFormIdAndEmpId(Guid formId, Guid empId)
        {
            return _RecruitmentFormEmployeeDataAccess.GetByQuery(string.Format("FormId ='{0}' and EmployeeId ='{1}'", formId,empId)).FirstOrDefault();
        }

        public RecruitmentW4Form GetW4FormById(int id)
        {
            return _RecruitmentW4FormDataAccess.Get(id);
        }
        public Recruitmenti9Form GetI9FormById(int id)
        {
            return _Recruitmenti9FormDataAccess.Get(id);
        }

        public void DeleteRecruitmentFormEmployeeByEmployeeIdAndRecruitmentFormId(Guid employeeId, int recruitmentFormId)
        {
            _RecruitmentFormEmployeeDataAccess.DeleteRecruitmentFormEmployeeByEmployeeIdAndRecruitmentFormId(employeeId, recruitmentFormId);
        }

        public RecruitmentFormEmployee GetRecruitmentFormEmployeeByRecruitmentFormIdAndEmpId(int recruitmentFormId, Guid employeeId)
        {
            return _RecruitmentFormEmployeeDataAccess.GetByQuery(string.Format("EmployeeId ='{0}'  and RecruitmentFormId = {1}",employeeId,recruitmentFormId)).FirstOrDefault(); 
        }
        public bool DeleteRecruitmentW9FromByFormId(Guid FormId)
        {
            return _RecruitmentW9FormDataAccess.DeleteRecruitmentW9FromByFormId(FormId);
        }
        public bool DeleteRecruitmentW4FromByFormId(Guid FormId)
        {
            return _RecruitmentW4FormDataAccess.DeleteRecruitmentW4FromByFormId(FormId);
             
        }
        public bool DeleteRecruitmentI9FromByFormId(Guid FormId)
        {
            return _Recruitmenti9FormDataAccess.DeleteRecruitmentI9FromByFormId(FormId);
        }
        public bool DeleteRecruitmentDocFormByFormId(Guid FormId)
        {
            return _RecruitmentDocFormDataAccess.DeleteRecruitmentDocFormByFormId(FormId);
        }

        public int InsertDocForm(RecruitmentDocForm docForm)
        {
            return (int)_RecruitmentDocFormDataAccess.Insert(docForm);
        }

        public RecruitmentDocForm GetDocFormByFormId(Guid formId)
        {
            return _RecruitmentDocFormDataAccess.GetByQuery(string.Format(" FormId ='{0}'",formId)).FirstOrDefault();
        }

        public bool UpdateI9Form(Recruitmenti9Form rI9)
        {
            return _Recruitmenti9FormDataAccess.Update(rI9) > 0;
        }

        public bool UpdateDocForm(RecruitmentDocForm model)
        {
            return _RecruitmentDocFormDataAccess.Update(model) > 0;
        }
    }
}
