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
	public partial class CreditScoreGradeDataAccess
	{
        public DataTable GetCreditScoreGradeByScoreRange(int ScoreRange)
        {
            string sqlQuery = @"select * from CreditScoreGrade where MinScore <= {0} and MaxScore>= {0}";
            try
            {
                sqlQuery = string.Format(sqlQuery, ScoreRange);
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
