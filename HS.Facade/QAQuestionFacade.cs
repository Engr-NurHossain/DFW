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
    public class QAQuestionFacade : BaseFacade
    {
        public QAQuestionFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        QaQuestionDataAccess _QaQuestionDataAccess
        {
            get
            {
                return (QaQuestionDataAccess)_ClientContext[typeof(QaQuestionDataAccess)];
            }
        }

        public List<QaQuestion> GetQa1QuestionByCompanyId(Guid companyid)
        {
            return _QaQuestionDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and Qa1 = 1", companyid));
        }

        public List<QaQuestion> GetQa2QuestionByCompanyId(Guid companyid)
        {
            return _QaQuestionDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and Qa2 = 1", companyid));
        }

        public long InsertQaQuestion(QaQuestion QA)
        {
            return _QaQuestionDataAccess.Insert(QA);
        }

        public bool UpdateQaQuestion(QaQuestion value)
        {
            return _QaQuestionDataAccess.Update(value) > 0;
        }

        public List<QaQuestion> GetQa1QuestionListNotInQaAnswer(Guid companyid, Guid customerid)
        {
            DataTable dt = _QaQuestionDataAccess.GetQa1QuestionListNotInQaAnswer(companyid, customerid);
            List<QaQuestion> Qa1List = new List<QaQuestion>();
            Qa1List = (from DataRow dr in dt.Rows
                       select new QaQuestion()
                       {
                           Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                           CompanyId = (Guid)dr["CompanyId"],
                           //CustomerId = (Guid)dr["CustomerId"],
                           Title = dr["Title"].ToString(),
                           Qa1 = (dr["Qa1"] != DBNull.Value ? Convert.ToBoolean(dr["Qa1"]) : false),
                           Qa2 = (dr["Qa2"] != DBNull.Value ? Convert.ToBoolean(dr["Qa2"]) : false),
                           Type = dr["Type"].ToString(),
                           IsActive = (dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false),
                       }).ToList();
            return Qa1List;
        }
    }
}
