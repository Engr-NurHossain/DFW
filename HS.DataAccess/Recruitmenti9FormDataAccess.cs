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
	public partial class Recruitmenti9FormDataAccess
	{
        public bool DeleteRecruitmentI9FromByFormId(Guid FormId)
        {
            string sqlQuery = @"delete from Recruitmenti9Form where FormId = '{0}'";

            try
            {
                sqlQuery = string.Format(sqlQuery, FormId);
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
    }	
}
