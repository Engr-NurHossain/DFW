using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.DataAccess
{
    public partial class CustomSurveyUserAnswersDataAccess
    {
        //will be used later.
        //public List<CustomSurveyUserAnswers> GetCustomSurveyUserAnswersByQuestionList(List<Guid> qustionIds)
        //{
        //    string sqlQuery = @"";

        //    try
        //    {
        //        string Query = string.Format(" QuestionId in ('{0}') ", string.Join("','", qustionIds)); 
        //        sqlQuery = string.Format(sqlQuery, Query);
        //        using (SqlCommand cmd = GetSQLCommand(sqlQuery))
        //        {
        //            return GetList(cmd,-1);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
        public bool DeleteCustomSurveyUserAnswersByUserIdAndSurveyId(Guid userId, Guid surveyId,Guid SurveyUserId)
        {
            string sqlQuery = @"delete from CustomSurveyUserAnswers where UserId = '{0}' and  SurveyId ='{1}' and SurveyUserId={2}";
            try
            {
                sqlQuery = string.Format(sqlQuery, userId, surveyId, SurveyUserId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    ExecuteCommand(cmd);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
