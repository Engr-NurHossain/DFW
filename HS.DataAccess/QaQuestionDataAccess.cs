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
	public partial class QaQuestionDataAccess
	{
        public DataTable GetQa1QuestionListNotInQaAnswer(Guid companyid, Guid customerid)
        {
            string sqlQuery = @"select qs.*
                                from QaQuestion qs
                                join QaAnswer qa
                                on qa.QuestionId = qs.Id
                                where qa.Answer = 'false'
                                and qa.CompanyId = '{0}'
                                and qa.CustomerId = '{1}'";
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
