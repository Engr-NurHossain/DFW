using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.DataAccess
{
	public partial class QaAnswerDataAccess
	{
        public bool DeleteQa1AnswerByCustomerIdAndComapnyId(Guid CustomerId, Guid CompanyId)
        {
            string SqlQuery = @"select qa.Id into #qaans FROM QaAnswer qa
                                Left join QaQuestion qq on qq.Id= qa.QuestionId 
                                where qa.CustomerId ='{0}' and qa.CompanyId = '{1}' AND qq.Qa1=1
                                select * from #qaans
                                DELETE QaAnswer 
                                WHERE Id In(select * from #qaans)
                                drop TABLE  #qaans";
            SqlQuery = string.Format(SqlQuery, CustomerId, CompanyId);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(SqlQuery))
                {
                    ExecuteCommand(cmd);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool DeleteQa2AnswerByCustomerIdAndComapnyId(Guid CustomerId, Guid CompanyId)
        {
            string SqlQuery = @"select qa.Id into #qaans FROM QaAnswer qa
                                Left join QaQuestion qq on qq.Id= qa.QuestionId 
                                where qa.CustomerId ='{0}' and qa.CompanyId = '{1}' AND qq.Qa2=1
                                select * from #qaans
                                DELETE QaAnswer 
                                WHERE Id In(select * from #qaans)
                                drop TABLE  #qaans";
            SqlQuery = string.Format(SqlQuery, CustomerId, CompanyId);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(SqlQuery))
                {
                    ExecuteCommand(cmd);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public DataTable GetQa1QuestionaireByCompanyIdandCustomerId(Guid companyid, Guid customerid)
        {
            string sqlQuery = @"select 
                                    ans.*,
                                    ques.Title as QuestionTitle
                                from QaAnswer ans
                                join QaQuestion ques
                                    on ques.Id=ans.QuestionId
                                where 
                                    ans.CompanyId='{0}'
                                    and ans.CustomerId='{1}'
                                    and ques.Qa1=1";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyid, customerid);
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

        public DataTable GetQa1QuestionaireAnswerFalseByCompanyIdandCustomerId(Guid companyid, Guid customerid, int AnswerQuesId)
        {
            string sqlQuery = @"select *
                                from QaAnswer qans
                                where qans.CompanyId = '{0}'
                                and qans.CustomerId = '{1}'
                                and qans.QuestionId = '{2}'
								and qans.Answer = 'false'";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyid, customerid, AnswerQuesId);
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

        public DataTable GetQa2QuestionaireByCompanyIdandCustomerId(Guid companyid, Guid customerid)
        {
            string sqlQuery = @"select ans.*,ques.Title as QuestionTitle
                                from QaAnswer ans
                                join QaQuestion ques
                                on ques.Id=ans.QuestionId
                                where ans.CompanyId='{0}'
                                and ans.CustomerId='{1}'
                                and ques.Qa2=1";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyid, customerid);
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
    }	
}
