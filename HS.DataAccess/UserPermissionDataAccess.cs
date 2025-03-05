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
    public partial class UserPermissionDataAccess
    {
        public UserPermissionDataAccess() { }
        public UserPermissionDataAccess(string ConStr) : base(ConStr) { }
        public bool DeleteAllUserPermissionsByUserId(Guid UserId)
        {
            string SqlQuery = @"delete from UserPermission where UserId = '{0}' ";
            SqlQuery = string.Format(SqlQuery, UserId);
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
        public bool DeleteAllUserPermissionsByUserIdAndCompanyId(Guid UserId, Guid CompanyId)
        {
            string SqlQuery = @"delete from UserPermission where UserId = '{0}' and CompanyId = '{1}' ";
            SqlQuery = string.Format(SqlQuery, UserId, CompanyId);
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



        public DataTable GetAllUserPermissionsPermissionGroupId(int id, Guid CompanyId)
        {
            string SqlQuery = @"select up.*, emp.FirstName + ' ' + emp.LastName as FullName, ul.EmailAddress from UserPermission up
		                        left join UserLogin ul on ul.UserId = up.UserId
		                        left join Employee emp on emp.UserId = ul.UserId
		                        where PermissionGroupId='{0}' and up.CompanyId='{1}'";
          
            try
            {
                SqlQuery = string.Format(SqlQuery, id, CompanyId);
                using (SqlCommand cmd = GetSQLCommand(SqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null; ;
            }
        }
    }
}
