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
	public partial class AgreementAnswerDataAccess
	{
        public AgreementAnswerDataAccess(string ConStr) : base(ConStr) { }
        public bool DeleteAgreementAnswerByCustomerId(Guid CustomerId)
        {
            string SqlQuery = @"
                            delete from AgreementAnswer
                            where CustomerId = '{0}' 
                ";
            SqlQuery = string.Format(SqlQuery, CustomerId);
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

        public bool DeleteAgreementAnswerByCustomerIdAndQuesId(Guid CustomerId, int quesid)
        {
            string SqlQuery = @"
                            delete from AgreementAnswer
                            where CustomerId = '{0}'
                            and QuestionId = '{1}'
                ";
            SqlQuery = string.Format(SqlQuery, CustomerId, quesid);
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
    }	
}
