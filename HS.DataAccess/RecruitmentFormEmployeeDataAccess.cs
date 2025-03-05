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
	public partial class RecruitmentFormEmployeeDataAccess
	{
        public bool DeleteAllRecruitmentFormEmployeeByEmployeeId(Guid employeeId)
        {
            string sqlQuery = @"delete from RecruitmentFormEmployee where EmployeeId = '{0}'";

            try
            {
                sqlQuery = string.Format(sqlQuery,employeeId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }


        }
        public bool DeleteRecruitmentFormEmployeeByEmployeeIdAndRecruitmentFormId(Guid employeeId,int recruitmentFormId)
        {
            string sqlQuery = @"delete from RecruitmentFormEmployee where EmployeeId = '{0}' and [RecruitmentFormId] ={1}";

            try
            {
                sqlQuery = string.Format(sqlQuery, employeeId, recruitmentFormId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }


        }

        public DataTable GetAllEmployeeRecruitmentFormsByEmpId(Guid EmployeeId)
        {
            string sqlQuery = @"select rfe.*,rf.Name as FormName from RecruitmentFormEmployee rfe
                                left join RecruitmentForm rf on rf.Id = rfe.RecruitmentFormId
                                where rfe.EmployeeId = '{0}'";

            try
            {
                sqlQuery = string.Format(sqlQuery, EmployeeId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

    }	
}
