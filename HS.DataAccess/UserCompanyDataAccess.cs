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
	public partial class UserCompanyDataAccess
	{
        public UserCompanyDataAccess() { }
        public UserCompanyDataAccess(ClientContext context, string ConnectionString) : base(context, ConnectionString) { }
        public UserCompanyDataAccess(string ConnectionString) : base(ConnectionString) { }
        public DataTable GetUsersCompanyListByUserId(Guid UserId)
        {

            string sqlQuery = @"declare @Username uniqueidentifier 
                                set @Username = '{0}'
                                select uc.* ,
                                com.CompanyName 
                                from UserCompany uc
                                left join Company com 
	                                on com.CompanyId = uc.CompanyId 
                                where uc.UserId = @Username";
            try
            {
                sqlQuery = string.Format(sqlQuery, UserId);
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
        public bool RemoveDefaultCompanyByUserId(Guid UserId)
        {

            string sqlQuery = @"UPDATE UserCompany
                                SET IsDefault = 0
                                where UserId ='{0}' ";
            try
            {
                sqlQuery = string.Format(sqlQuery, UserId);
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
        public bool DeleteUserCompanyByUserIdAndCompanyId(Guid UserId,Guid CompanyId)
        {
            string sqlQuery = @" delete from UserCompany where 
                                    UserId = '{0}' and CompanyId = '{1}' ";
            try
            {
                sqlQuery = string.Format(sqlQuery, UserId,CompanyId);
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
        public bool DeleteUserCompanyByUserId(Guid UserId)
        {
            string sqlQuery = @" delete from UserCompany where 
                                    UserId = '{0}' ";
            try
            {
                sqlQuery = string.Format(sqlQuery, UserId);
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

        public bool SetDefaultUserCompany(Guid UserId, Guid CompanyId)
        { 
            string sqlQuery = @"UPDATE UserCompany
                                SET IsDefault = 0
                                WHERE UserId = '{0}'

                                UPDATE UserCompany
                                SET IsDefault = 1
                                WHERE UserId = '{0}'
                                and companyId = '{1}'
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, UserId, CompanyId);
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
        public DataTable GetAllUsersListByCompanyId(Guid CompanyId)
        {

            string sqlQuery = @"select Emp.* 
                                from Employee Emp
                                Left Join UserCompany Uc 
                                on Emp.UserId =  Uc.UserId
                                where Uc.CompanyId = '{0}' and Emp.IsActive = 'True'";
            try
            {
                sqlQuery = string.Format(sqlQuery, CompanyId);
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
