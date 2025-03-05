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
    public class QaAnswerFacade : BaseFacade
    {
        public QaAnswerFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        QaAnswerDataAccess _QaAnswerDataAccess
        {
            get
            {
                return (QaAnswerDataAccess)_ClientContext[typeof(QaAnswerDataAccess)];
            }
        }

        public long InsertQaAnswer(QaAnswer val)
        {
            return _QaAnswerDataAccess.Insert(val);
        }

        public bool UpdateQaAnswer(QaAnswer ans)
        {
            return _QaAnswerDataAccess.Update(ans) > 0;
        }

        public bool DeleteQa1AnswerByCustomerIdAndComapnyId(Guid CustomerId, Guid CompanyId)
        {
            var result = _QaAnswerDataAccess.DeleteQa1AnswerByCustomerIdAndComapnyId(CustomerId, CompanyId);
            return result;
        }

        public bool DeleteQa2AnswerByCustomerIdAndComapnyId(Guid CustomerId, Guid CompanyId)
        {
            var result = _QaAnswerDataAccess.DeleteQa2AnswerByCustomerIdAndComapnyId(CustomerId, CompanyId);
            return result;
        }

        public List<QaAnswer> GetAllQAByCompanyIdAndCustomerId(Guid customerid, Guid companyid)
        {
            return _QaAnswerDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and CompanyId = '{1}'", customerid, companyid));
        }

        public List<QaAnswer> GetQa1QuestionaireByCompanyIdandCustomerId(Guid companyId, Guid customerId)
        {
            DataTable dt = _QaAnswerDataAccess.GetQa1QuestionaireByCompanyIdandCustomerId(companyId, customerId);
            List<QaAnswer> Qa1List = new List<QaAnswer>();
            Qa1List = (from DataRow dr in dt.Rows
                               select new QaAnswer()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   CompanyId = (Guid)dr["CompanyId"],
                                   CustomerId = (Guid)dr["CustomerId"],
                                   QuestionId = dr["QuestionId"] != DBNull.Value ? Convert.ToInt32(dr["QuestionId"]) : 0,
                                   Answer = dr["Answer"].ToString(),
                                   QuestionTitle = dr["QuestionTitle"].ToString()
                               }).ToList();
            return Qa1List;
        }

        public QaAnswer GetQa1QuestionaireAnswerFalseByCompanyIdandCustomerId(Guid companyId, Guid customerId, int AnswerQuesId)
        {
            DataTable dt = _QaAnswerDataAccess.GetQa1QuestionaireAnswerFalseByCompanyIdandCustomerId(companyId, customerId, AnswerQuesId);
            QaAnswer Qa1AnswerList = new QaAnswer();
            Qa1AnswerList = (from DataRow dr in dt.Rows
                       select new QaAnswer()
                       {
                           Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                           CompanyId = (Guid)dr["CompanyId"],
                           CustomerId = (Guid)dr["CustomerId"],
                           QuestionId = dr["QuestionId"] != DBNull.Value ? Convert.ToInt32(dr["QuestionId"]) : 0,
                           Answer = dr["Answer"].ToString()
                       }).FirstOrDefault();
            return Qa1AnswerList;
        }

        public List<QaAnswer> GetQa2QuestionaireByCompanyIdandCustomerId(Guid companyId, Guid customerId)
        {
            DataTable dt = _QaAnswerDataAccess.GetQa2QuestionaireByCompanyIdandCustomerId(companyId, customerId);
            List<QaAnswer> Qa1List = new List<QaAnswer>();
            Qa1List = (from DataRow dr in dt.Rows
                       select new QaAnswer()
                       {
                           Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                           CompanyId = (Guid)dr["CompanyId"],
                           CustomerId = (Guid)dr["CustomerId"],
                           QuestionId = dr["QuestionId"] != DBNull.Value ? Convert.ToInt32(dr["QuestionId"]) : 0,
                           Answer = dr["Answer"].ToString(),
                           QuestionTitle = dr["QuestionTitle"].ToString()
                       }).ToList();
            return Qa1List;
        }
    }
}
