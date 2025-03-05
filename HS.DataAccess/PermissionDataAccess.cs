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
    public partial class PermissionDataAccess
    {

        public PermissionDataAccess() { }
        public PermissionDataAccess(string ConnStr):base(ConnStr) { }

        public List<Permission> GetPermissionByGroupId(int gid,string PermissionName)
        {
            List<Permission> permission = new List<Permission>();
            string SearchQuery = "";
            if(!string.IsNullOrEmpty(PermissionName))
            {
                SearchQuery = string.Format(" and (p.Name like '%{0}%' or p.DisplayText like '%{0}%')", PermissionName);
            }
            string sqlQuery = string.Format(@"select p.Id, p.Name, p.DisplayText, p.ParentId, 
                                CASE 
                                  WHEN pgp.Id IS NULL THEN  0 
                                  ELSE 1
                                END as IsAssign  from Permission p 
                                Left JOIN PermissionGroupMap pgp  on pgp.PermissionId = p.Id  AND pgp.PermissionGroupId= {0}
                                where  p.IsActive= 1 {1}", gid,SearchQuery);

            try
            {
                sqlQuery = string.Format(sqlQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    DataTable dtResultd = new DataTable();
                    dtResultd= dsResult.Tables[0];
                    permission = (from DataRow dr in dtResultd.Rows
                                    select new HS.Entities.Permission()
                                    { 
                                        Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                        Name = dr["Name"].ToString(),
                                        DisplayText = dr["DisplayText"].ToString(),  
                                        IsActive = dr["IsAssign"] != DBNull.Value ? Convert.ToBoolean(dr["IsAssign"]) : false,
                                        ParentId = dr["ParentId"] != DBNull.Value ? Convert.ToInt32(dr["ParentId"]) : 0,
                                    }).ToList();

                    return permission;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<int> GetPermissionParentIdList()
        {
            string sqlQuery = @"select ParentId from Permission 
                                where ParentId is not null
                                and ParentId > 0
                                group by ParentId ";

            try
            {
                List<int> ParentIdList = new List<int>();
                sqlQuery = string.Format(sqlQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    
                    SqlDataReader reader;
                    long result = SelectRecords(cmd, out reader);  

                    using (reader)
                    {
                        while (reader.Read())
                        {
                            ParentIdList.Add(reader.GetInt32(0));
                        }
                        reader.Close();
                    }
                }
                return ParentIdList;
            }
            catch (Exception ex)
            {
                return new List<int>();
            }
        }

        public DataTable GetAllCustomPermission()
        {
            string sqlQuery = @"SELECT	pg.Id as RoleId,
		                        pg.Name as RoleName, 
		                        p.Id as PermissionId,
		                        p.ParentId as PermissionParentId,
		                        p.Name as PermissionName
                          FROM [Permission] p
                          left join [PermissionGroupMap] pgm on pgm.PermissionId = p.Id and pgm.CompanyId = p.CompanyId
                          left join [PermissionGroup] pg on pg.Id = pgm.PermissionGroupId
                          order by pg.Id";
            try
            {
                sqlQuery = string.Format(sqlQuery);
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

        public bool CheckCurrentLogInUserHasPermissionByUserIdAndPermissionId(Guid UserId, int perId)
        {
            string sqlQuery = @"select up.UserId, ISNULL(up.PermissionId,pgm.PermissionId) PermissionId  into #allpermission from UserPermission up
                                Left join PermissionGroupMap pgm on pgm.PermissionGroupId = up.PermissionGroupId and pgm.CompanyId = up.CompanyId
                                select * from #allpermission
                                where UserId='{0}' and PermissionId in ({1})
                                drop table #allpermission";
            try
            {
                sqlQuery = string.Format(sqlQuery, UserId, perId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    if (dsResult.Tables[0].Rows.Count > 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public DataTable GetAllPermissionsByUserIdAndCompanyId(Guid UserId, Guid CompanyId)
        {
            string sqlQuery = @"select p.*
	                                from UserPermission UP
	                                left join PermissionGroupMap PGM
	                                on UP.PermissionGroupId = PGM.PermissionGroupId and PGM.CompanyId = UP.CompanyId
	                                left join Permission P
	                                on p.Id = PGM.PermissionId
	                                where UP.UserId = '{0}' and UP.CompanyId = '{1}'
                                UNION
                                SELECT 
	                                p.*
	                                from Permission p
	                                left join UserPermission up
	                                on p.Id = up.PermissionId
	                                where up.UserId = '{0}' and up.CompanyId = '{1}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, UserId, CompanyId);
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
        public DataTable GetAllPermissionsGroupmapByGropupId(int groupId, Guid comid)
        {
            string sqlQuery = @"select * from PermissionGroupMap where PermissionGroupId = {0} and CompanyId = '{1}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, groupId, comid);
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
