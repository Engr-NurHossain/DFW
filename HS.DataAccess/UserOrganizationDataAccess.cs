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
    public partial class UserOrganizationDataAccess
    {
        public UserOrganizationDataAccess() { }

        public DataTable GetCompanyConnectionByUsername(string userName)
        {
            string sqlQuery = @"select UO.CompanyId
                                ,O.ConnectionString 
                                ,O.MasterPassword
                                ,O.CompanyName
                                from UserOrganization UO 
                                left join Organization O
                                on O.CompanyId = UO.CompanyId
                                where UO.UserName ='{0}'
                                and UO.IsActive =1
                                ";
            sqlQuery = string.Format(sqlQuery, userName);

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

        public DataTable GetCompanyConnectionByUsernameAndCompanyId(string userName, Guid comid)
        {
            string sqlQuery = @"select UO.CompanyId
                                ,O.ConnectionString 
                                ,O.MasterPassword
                                from UserOrganization UO 
                                left join Organization O
                                on O.CompanyId = UO.CompanyId
                                where UO.UserName ='{0}'
                                and UO.IsActive =1
                                and UO.CompanyId = '{1}'
                                ";
            sqlQuery = string.Format(sqlQuery, userName, comid);

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

        public DataTable GetIeateryCompanyConnectionByUsernameAndCompanyId(string userName, Guid comid)
        {
            string sqlQuery = @"select UO.CompanyId
                                ,O.ConnectionString 
                                ,O.MasterPassword
                                from UserOrganization UO 
                                left join Organization O
                                on O.CompanyId = UO.CompanyId
                                where UO.UserName ='{0}'
                                --and UO.IsActive =1
                                and UO.CompanyId = '{1}'
                                ";
            sqlQuery = string.Format(sqlQuery, userName, comid);

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
        public DataTable GetCompanyConnectionByCompanyId(string CompanyId)
        {
            string sqlQuery = @"select UO.CompanyId
                                ,O.ConnectionString, UO.UserName from UserOrganization UO 
                                left join Organization O
                                on O.CompanyId = UO.CompanyId
                                where UO.CompanyId ='{0}'
                                and UO.IsActive =1
                                ";
            sqlQuery = string.Format(sqlQuery, CompanyId);

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

        public DataTable GetIeateryCompanyConnectionByCompanyId(Guid CompanyId)
        {
            string sqlQuery = @"select UO.CompanyId
                                ,O.ConnectionString, UO.UserName from UserOrganization UO 
                                left join Organization O
                                on O.CompanyId = UO.CompanyId
                                where UO.CompanyId ='{0}'
                                --and UO.IsActive =1
                                ";
            sqlQuery = string.Format(sqlQuery, CompanyId);

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
        public DataTable GetUsersOrganizationListByUsername(string userName)
        {
            string sqlQuery = @"select distinct UO.*
                                ,Org.CompanyName
                                ,Org.ConnectionString
                                from UserOrganization UO
                                left join Organization Org 
                                on Org.CompanyId = UO.CompanyId
                                where UO.UserName = '{0}'
                                and (CompanyName is not null
								or ConnectionString is not null)
                                --and UO.IsActive =1
                                ";
            sqlQuery = string.Format(sqlQuery, userName);

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

        public bool SetDefaultUserCompany(string UserName, Guid CompanyId)
        {
            string sqlQuery = @"DECLARE @UserName nvarchar(250)
                                Declare @ComapnyId uniqueidentifier
                                set @UserName = '{0}'
                                set @ComapnyId = '{1}'
                                if exists ( select * from UserOrganization where UserName =@UserName and CompanyId =@ComapnyId)
                                begin
	                                update UserOrganization
	                                SET IsActive = 0
                                    WHERE UserName = @UserName

	                                UPDATE UserOrganization
                                    SET IsActive = 1
                                    WHERE UserName = @UserName
                                    and companyId = @ComapnyId
                                end
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, UserName, CompanyId);
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
        public bool DeleteUserOrganizationByUsernameAndCompanyId(string Username, Guid CompanyId)
        {
            string sqlQuery = @"delete from UserOrganization where 
                                    UserName = '{0}' and CompanyId = '{1}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, Username, CompanyId);
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
        public bool DeleteUserOrganizationByUsername(string userName)
        {
            string sqlQuery = @" delete from UserOrganization where 
                                    UserName = '{0}' ";
            try
            {
                sqlQuery = string.Format(sqlQuery, userName);
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

        public bool UpdateUserNameByUserName(string oldUsername, string newusername)
        {
            string sqlQuery = @" update UserOrganization
                                set UserName ='{1}' 
                                where UserName ='{0}'

                                update Organization set UserName  ='{1}'
                                where UserName ='{0}'
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, oldUsername, newusername);
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

        public bool InsertUserCredential(string userName, Guid companyId)
        {
            string sqlQuery = @"Declare @Username nvarchar(200) 
                                Declare @CompanyId uniqueidentifier 

                                set @Username= '{0}' 
                                set @CompanyId ='{1}' 
                                --UserOrganization
                                if not exists (select * from rmrmaster.dbo.UserOrganization where CompanyId =@CompanyId and UserName =@Username)
                                begin
	                                insert into UserOrganization ( CompanyId,UserName, IsActive) values 
	                                (@CompanyId,@Username,1) 
                                end";
            try
            {
                sqlQuery = string.Format(sqlQuery, userName,  companyId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
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

        public bool DeleteResturantMasterDB(Guid companyid)
        {
            string sqlQuery = @"declare @CompanyId uniqueidentifier 
                                set @CompanyId = '{0}'
                                
                                Delete from Organization where CompanyId = @CompanyId
                                Delete from UserOrganization where CompanyId = @CompanyId
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyid);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
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
