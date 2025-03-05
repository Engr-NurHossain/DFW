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
    public class CustomSurveyFacade : BaseFacade
    {
        public CustomSurveyFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        CustomSurveyDataAccess _CustomSurveyDataAccess
        {
            get
            {
                return (CustomSurveyDataAccess)_ClientContext[typeof(CustomSurveyDataAccess)];
            }
        }
        CustomSurveyUserDataAccess _CustomSurveyUserDataAccess
        {
            get
            {
                return (CustomSurveyUserDataAccess)_ClientContext[typeof(CustomSurveyUserDataAccess)];
            }
        }
        CustomSurveyQuestionDataAccess _CustomSurveyQuestionDataAccess
        {
            get
            {
                return (CustomSurveyQuestionDataAccess)_ClientContext[typeof(CustomSurveyQuestionDataAccess)];
            }
        }
        CustomSurveyAnswerDataAccess _CustomSurveyAnswerDataAccess
        {
            get
            {
                return (CustomSurveyAnswerDataAccess)_ClientContext[typeof(CustomSurveyAnswerDataAccess)];
            }
        }
        CustomSurveyUserAnswersDataAccess _CustomSurveyUserAnswersDataAccess
        {
            get
            {
                return (CustomSurveyUserAnswersDataAccess)_ClientContext[typeof(CustomSurveyUserAnswersDataAccess)];
            }
        }
        public List<CustomSurvey> GetAllCustomSurvey()
        {
            return _CustomSurveyDataAccess.GetAll();
        }
        public List<CustomSurveyUser> GetAllCustomSurveyUser()
        {
            return _CustomSurveyUserDataAccess.GetAll();
        }
        public List<CustomSurveyUser> GetAllCustomSurveyUserWithPagination(int PageNumber, int UnitPerPage, string SearchText)
        {
            DataTable dt = _CustomSurveyDataAccess.GetAllSurveyUser(PageNumber, UnitPerPage, SearchText);
            List<CustomSurveyUser> SurveyUserList = new List<CustomSurveyUser>();
            SurveyUserList = (from DataRow dr in dt.Rows
                         select new CustomSurveyUser()
                         {
                             SurveyId = (Guid)dr["SurveyId"],
                             SurveyName = dr["SurveyName"].ToString(),
                             UserName = dr["UserName"].ToString(),
                             UserId = (Guid)dr["UserId"],
                             AddedBy = (Guid)dr["AddedBy"],
                             AddedDate = dr["AddedDate"] != DBNull.Value ? Convert.ToDateTime(dr["AddedDate"]) : DateTime.Now,
                             Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                             SurveyUserId = (Guid)dr["SurveyUserId"],
                             Status = dr["Status"].ToString(),
                             AddedByName = dr["AddedByName"].ToString(),
                         }).ToList();
            return SurveyUserList;
        }
        public List<CustomSurvey> GetAllSurvey()
        {
            DataTable dt = _CustomSurveyDataAccess.GetAllSurvey();
            List<CustomSurvey> SurveList = new List<CustomSurvey>();
            SurveList = (from DataRow dr in dt.Rows
                        select new CustomSurvey()
                        {
                            SurveyId = (Guid)dr["SurveyId"],
                            SurveyName = dr["SurveyName"].ToString(),
                            CreatedBy = (Guid)dr["CreatedBy"],
                            CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.Now,
                            CreatedByName = dr["CreatedByName"].ToString(),
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                            QuesCount = dr["QuesCount"] != DBNull.Value ? Convert.ToInt32(dr["QuesCount"]) : 0,
                        }).ToList();
            return SurveList;
        }
        public List<CustomSurveyQuestion> GetAllSurveyQuestion(Guid SurveyId,string SearchText)
        {
            return _CustomSurveyDataAccess.GetAllSurveyQuestions(SurveyId,SearchText);
        }
        public List<CustomSurvey> GetAllCustomSurveyBySurveyName(string SurveyName)
        {
            return _CustomSurveyDataAccess.GetByQuery(string.Format(" SurveyName = '{0}'", SurveyName));
        }
        public long InsertCustomSurvey(CustomSurvey customsurvey)
        {
            return _CustomSurveyDataAccess.Insert(customsurvey);
        }
        public long InsertCustomSurveyQuestion(CustomSurveyQuestion customsurveyQues)
        {
            return _CustomSurveyQuestionDataAccess.Insert(customsurveyQues);
        }
        public long InsertCustomSurveyAnswer(CustomSurveyAnswer CustomSurveyAnswer)
        {
            return _CustomSurveyAnswerDataAccess.Insert(CustomSurveyAnswer);
        }
        public CustomSurveyUser GetCustomSurveyUserBySurveyUserId(Guid ticketId)
        {
            return _CustomSurveyUserDataAccess.GetByQuery(string.Format(" SurveyUserId = '{0}'",ticketId)).FirstOrDefault();
        }

        public CustomSurvey GetCustomSurveyBySurveyId(Guid surveyId)
        {
            return _CustomSurveyDataAccess.GetByQuery(string.Format(" SurveyId = '{0}'", surveyId)).FirstOrDefault();
        }
        public CustomSurveyQuestion GetCustomSurveyQuestionByQuestionId(Guid QuistionId)
        {
            return _CustomSurveyQuestionDataAccess.GetByQuery(string.Format(" QuestionId = '{0}'", QuistionId)).FirstOrDefault();
        }
        public CustomSurveyQuestion GetCustomSurveyQuestionById(int id)
        {
            return _CustomSurveyQuestionDataAccess.Get(id);
        }
        public CustomSurveyAnswer GetCustomSurveyAnswerById(int id)
        {
            return _CustomSurveyAnswerDataAccess.Get(id);
        }
        public List<CustomSurveyQuestion> GetSurveyQuestionsBySurveyId(Guid surveyId)
        {
            return _CustomSurveyQuestionDataAccess.GetByQuery(string.Format(" SurveyId = '{0}'", surveyId));
        }
        public CustomSurveyQuestion GetSurveyQuestionsByQuestionId(Guid QuestionId)
        {
            return _CustomSurveyQuestionDataAccess.GetByQuery(string.Format(" QuestionId = '{0}'", QuestionId)).FirstOrDefault();
        }
        public List<CustomSurveyAnswer> GetAllSurveyAnswerByQuestionId(Guid QuestionId)
        {
            return _CustomSurveyAnswerDataAccess.GetByQuery(string.Format(" QuestionId = '{0}'", QuestionId));
        }
        public List<CustomSurveyUserAnswers> GetAllSurveyUserAnswersBySurveyUserId(Guid SurveyUserId)
        {
            return _CustomSurveyUserAnswersDataAccess.GetByQuery(string.Format(" SurveyUserId = '{0}'", SurveyUserId));
        }
        public List<CustomSurveyAnswer> GetSurveyAnswersBySurveyQuestionIdList(List<Guid> QustionIds)
        {
            string Query = string.Format(" QuestionId in ('{0}') ", string.Join("','", QustionIds));
            return _CustomSurveyAnswerDataAccess.GetByQuery(Query);
        } 
        public CustomSurveyAnswer GetSurveyAnswerId(int Id)
        {
            return _CustomSurveyAnswerDataAccess.Get(Id);
        }
        public long UpdateSurveyAnswer(CustomSurveyAnswer customAns)
        {
            return _CustomSurveyAnswerDataAccess.Update(customAns);
        }
        public List<CustomSurveyAnswer> GetSurveyAnswersBySurveyId(Guid surveyId)
        {
            return _CustomSurveyAnswerDataAccess.GetByQuery(string.Format(" SurveyId = '{0}'", surveyId));
        }
        public int InsertCustomSurveyUserAnswer(CustomSurveyUserAnswers item)
        {
            return (int)_CustomSurveyUserAnswersDataAccess.Insert(item);
        }
        public int InsertCustomSurveyUser(CustomSurveyUser item)
        {
            return (int)_CustomSurveyUserDataAccess.Insert(item);
        }
        public bool DeleteAnswer(int id)
        {
            return _CustomSurveyAnswerDataAccess.Delete(id)>0;
        }
        public bool DeleteCustomSurveyUser(int id)
        {
            return _CustomSurveyUserDataAccess.Delete(id) > 0;
        }
        public bool DeleteQuestion(int id)
        {
            return _CustomSurveyQuestionDataAccess.Delete(id)>0;
        }

        public bool UpdateCustomSurveyUser(CustomSurveyUser cSU)
        {
            return _CustomSurveyUserDataAccess.Update(cSU)>0;
        }
        public bool UpdateCustomSurveyQuestion(CustomSurveyQuestion surveyQuestion)
        {
            return _CustomSurveyQuestionDataAccess.Update(surveyQuestion) > 0;
        }
        public List<CustomSurveyUserAnswers> GetCustomSurveyUserAnswersByQuestionList(List<Guid> QustionIds, Guid UserId, Guid SurveyUserId)
        {
            string Query = string.Format(" QuestionId in ('{0}') and UserId = '{1}' and SurveyUserId = '{2}'", string.Join("','", QustionIds), UserId,SurveyUserId);  

            return _CustomSurveyUserAnswersDataAccess.GetByQuery(Query);

            //return _CustomSurveyUserAnswersDataAccess.GetCustomSurveyUserAnswersByQuestionList(QustionIds);

        }
        public List<CustomSurveyUserAnswers> GetCustomSurveyUserAnswers(List<Guid> QustionIds, Guid SurveyUserId) //Guid UserId,
        {
            string Query = string.Format(" QuestionId in ('{0}')  and SurveyUserId = '{1}'", string.Join("','", QustionIds), SurveyUserId); //and UserId = '{1}' //UserId,

            return _CustomSurveyUserAnswersDataAccess.GetByQuery(Query);

            //return _CustomSurveyUserAnswersDataAccess.GetCustomSurveyUserAnswersByQuestionList(QustionIds);

        }

        public bool DeleteCustomSurveyUserAnswersByUserIdAndSurveyId(Guid userId, Guid surveyId,Guid SurveyUserId)
        {
            return _CustomSurveyUserAnswersDataAccess.DeleteCustomSurveyUserAnswersByUserIdAndSurveyId(userId, surveyId, SurveyUserId);
        }

        public CustomSurvey GetCustomSurveyById(int value)
        {
            return _CustomSurveyDataAccess.Get(value);
        }

        public bool DeleteCustomSurveyById(int value)
        {
            return _CustomSurveyDataAccess.Delete(value) > 0;
        }
        public bool UpdateSurveyName(CustomSurvey Survey)
        {
            return _CustomSurveyDataAccess.Update(Survey) > -1;
        }
        //public CustomSurveyUser GetCustomSurveyUserById(int value)
        //{
        //    return _CustomSurveyUserDataAccess.Get(value);
        //}
    }
}
