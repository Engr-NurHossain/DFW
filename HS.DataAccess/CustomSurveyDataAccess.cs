using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;
using System.Linq;

namespace HS.DataAccess
{
	public partial class CustomSurveyDataAccess
	{
        public CustomSurveyDataAccess(string ConStr) : base(ConStr) { }
        public DataTable GetAllSurvey()
        {
            string sqlQuery = @"  select cs.*,emp.FirstName+' '+emp.LastName as CreatedByName
                                 ,(select COUNT(Id) from CustomSurveyQuestion csq where csq.SurveyId=cs.SurveyId) as QuesCount
                                 from CustomSurvey cs 
                                 left join Employee emp on cs.CreatedBy = emp.UserId ";
            try
            {
             
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetAllSurveyUser(int PageNumber, int UnitPerPage, string SearchText)
        {
            
            List<ServiceAreaZipcode> content = new List<ServiceAreaZipcode>();
            string rawQuery = @"  
                                declare @pagesize int
                                declare @pageno int 
                                set @pagesize = " + UnitPerPage + @"
                                set @pageno = " + PageNumber + @"
                                declare @pagestart int
                                set @pagestart=(@pageno-1)* @pagesize  
                                select  TOP (@pagesize) csu.*,cus.FirstName+' '+cus.LastName as UserName,cs.SurveyName,emp.FirstName+' '+emp.LastName as AddedByName
                                from CustomSurveyUser csu 
                                left join Customer cus on csu.UserId = cus.CustomerId
                                left join CustomSurvey  cs on cs.SurveyId = csu.SurveyId  
                                left join Employee emp on emp.UserId = csu.AddedBy
                                where cus.FirstName+' '+cus.LastName  like'%" + SearchText + @"%'AND csu.Id NOT IN(Select TOP (@pagestart) Id from CustomSurveyUser)
                                select Count(Id) As TotalCount from CustomSurveyUser";




            try
            {

                using (SqlCommand cmd = GetSQLCommand(rawQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
     
        }

        public List<CustomSurveyQuestion> GetAllSurveyQuestions(Guid SurveyId,string SearchText)
        {
          

            List<CustomSurveyQuestion> content = new List<CustomSurveyQuestion>();
            string rawQuery = @"
                                select   * FROM CustomSurveyQuestion
                                where Question like'%" + SearchText + @"%' and SurveyId = '{0}' order by Orderby";


            string sqlQuery = string.Format(rawQuery, SurveyId);

            using (SqlCommand cmd = GetSQLCommand(sqlQuery))
            {
                DataSet dsResult = GetDataSet(cmd);
                DataTable dt = dsResult.Tables[0];
                try
                {
                    content = (from DataRow dr in dt.Rows
                               select new CustomSurveyQuestion()
                               {

                                   Question = dr["Question"].ToString(),
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   OrderBy = dr["OrderBy"] != DBNull.Value ? Convert.ToInt32(dr["OrderBy"]) : 0,
                                   QuestionId = (Guid)dr["QuestionId"],
                                   QuestionType = dr["QuestionType"].ToString(),
                                   CreatedBy = (Guid)dr["CreatedBy"],
                                   CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.Now,
                                   
                               }).ToList();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            return content;
        }
    }	
}
