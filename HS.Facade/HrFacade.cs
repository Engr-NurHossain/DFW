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
    public class HrFacade : BaseFacade
    {
        public HrFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        EmployeeDataAccess _EmployeeDataAccess
        {
            get
            {
                return (EmployeeDataAccess)_ClientContext[typeof(EmployeeDataAccess)];
            }
        }
        EmployeePtoAccrualRate _EmployeePtoAccrualRateDataAccess
        {
            get
            {
                return (EmployeePtoAccrualRate)_ClientContext[typeof(EmployeePtoAccrualRate)];
            }
        }
        EmployeeInsuranceDataAccess _EmployeeInsuranceDataAccess
        {
            get
            {
                return (EmployeeInsuranceDataAccess)_ClientContext[typeof(EmployeeInsuranceDataAccess)];
            }
        }
        EmployeeOccurencesDataAccess _EmployeeOccurencesDataAccess
        {
            get
            {
                return (EmployeeOccurencesDataAccess)_ClientContext[typeof(EmployeeOccurencesDataAccess)];
            }
        }

        EmployeeWriteUpDataAccess _EmployeeWriteUpDataAccess
        {
            get
            {
                return (EmployeeWriteUpDataAccess)_ClientContext[typeof(EmployeeWriteUpDataAccess)];
            }
        }
        EmployeeEvaluationDataAccess _EmployeeEvaluationDataAccess
        {
            get
            {
                return (EmployeeEvaluationDataAccess)_ClientContext[typeof(EmployeeEvaluationDataAccess)];
            }
        }

        public int InsertEmployeeHumanRes(Employee employee)
        {
            return (int)_EmployeeDataAccess.Insert(employee);
        }
        public bool UpdateEmployeeHumanRes(Employee insurence)
        {
            return _EmployeeDataAccess.Update(insurence) > 0;
        }
        public List<Employee> GetAllEmployeeHumanRes()
        {
            return _EmployeeDataAccess.GetAll();
        }
        public List<Employee> GetEmployeeHumanResByUserId(Guid userId)
        {
            string query = "UserId='" + userId + "'";
            return _EmployeeDataAccess.GetByQuery(query);
        }


        public int InsertEmployeeInsurance(EmployeeInsurance insurence)
        {
            return (int)_EmployeeInsuranceDataAccess.Insert(insurence);
        }
        public bool UpdateEmployeeInsurance(EmployeeInsurance insurence)
        {
            return _EmployeeInsuranceDataAccess.Update(insurence) > 0;
        }
        public List<EmployeeInsurance> GetAllEmployeeInsurance()
        {
            return _EmployeeInsuranceDataAccess.GetAll();
        }
        public List<EmployeeInsurance> GetEmployeeInsuranceByUserId(Guid userId)
        {
            string query = "UserId='" + userId + "'";
            return _EmployeeInsuranceDataAccess.GetByQuery(query);
        }
         
        public int InsertEmployeeEvaluation(EmployeeEvaluation evaluation)
        {
            return (int)_EmployeeEvaluationDataAccess.Insert(evaluation);
        }
        public bool UpdateEmployeeEvaluation(EmployeeEvaluation evaluation)
        {
            return _EmployeeEvaluationDataAccess.Update(evaluation) > 0;
        }
        public List<EmployeeEvaluation> GetAllEmployeeEvaluation()
        {
            return _EmployeeEvaluationDataAccess.GetAll();
        }
        public bool DeleteInsurence(int id)
        {
            return _EmployeeInsuranceDataAccess.Delete(id) > 0;
        }
        public bool DeleteOccurence(int id)
        {
            return _EmployeeOccurencesDataAccess.Delete(id) > 0;
        }
        
        public List<EmployeeEvaluation> GetEmployeeEvaluationByUserId(Guid userId)
        {
            string query = "UserId='" + userId + "'";
            return _EmployeeEvaluationDataAccess.GetByQuery(query);
        }

        public int InsertEmployeeOccurance(EmployeeOccurences occurence)
        {
            return (int)_EmployeeOccurencesDataAccess.Insert(occurence);
        }
        public bool UpdateEmployeeOccurance(EmployeeOccurences occurence)
        {
            return _EmployeeOccurencesDataAccess.Update(occurence) > 0;
        }
        public List<EmployeeOccurences> GetAllEmployeeOccurance()
        {
            return _EmployeeOccurencesDataAccess.GetAll();
        }
        public EmployeeOccurences GetEmployeeOccuranceById(int Id)
        {
            return _EmployeeOccurencesDataAccess.Get(Id);
        }
        public List<EmployeeOccurences> GetEmployeeOccuranceByUserId(Guid userId)
        {
            string query = "UserId='" + userId + "' And OccurenceDate > DATEADD(year, -1, GetDate())";
            return _EmployeeOccurencesDataAccess.GetByQuery(query);
        }
       
        public List<EmployeeWriteUp> GetEmployeeWriteUpByUserId(Guid userId)
        {
            DataTable dt = _EmployeeWriteUpDataAccess.GetEmployeeWriteUpByUserId(userId);
            List<EmployeeWriteUp> EmployeeWriteUpList = new List<EmployeeWriteUp>();
            EmployeeWriteUpList = (from DataRow dr in dt.Rows
                           select new EmployeeWriteUp()
                           {
                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                               Category = dr["Category"].ToString(),
                               Description = dr["Description"].ToString(),
                               FileName = dr["FileName"].ToString(),
                               FilePath = dr["FilePath"].ToString(),
                               SupervisorName = dr["SupervisorName"].ToString(),
                               UserId = (Guid)dr["UserId"],
                               WriteupId = (Guid)dr["WriteupId"],
                              
                               CreatedBy = (Guid)dr["CreatedBy"],
                               CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                               WriteupDate = dr["WriteupDate"] != DBNull.Value ? Convert.ToDateTime(dr["WriteupDate"]) : new DateTime(),
                           }).ToList();


            return EmployeeWriteUpList;
        }
        public EmployeeWriteUp GetEmployeeWriteUpById(int Id)
        {
        
            return _EmployeeWriteUpDataAccess.Get(Id);
        }
        public long UpdateEmpWriteUp(EmployeeWriteUp writeup)
        {
          
            return _EmployeeWriteUpDataAccess.Update(writeup);
        }
        public long DeleteEmpWriteUp(int Id)
        {

            return _EmployeeWriteUpDataAccess.Delete(Id);
        }
        public long InsertEmpWriteUp(EmployeeWriteUp writeup)
        {

            return _EmployeeWriteUpDataAccess.Insert(writeup);
        }
    }
}
