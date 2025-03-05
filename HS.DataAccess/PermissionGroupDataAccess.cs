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
    public partial class PermissionGroupDataAccess
    {
        public PermissionGroupDataAccess(){}
        public PermissionGroupDataAccess(string ConStr) : base(ConStr){ }

        public List<PermissionGroup> GetAllPermissionGroup(Guid comId)
        {
            string sqlQuery = @"select *,
                            (select distinct count(ul.id) from UserCompany uc
                             left join UserLogin ul 
	                         on ul.UserId = uc.UserId
                             left join UserPermission up 
	                         on up.UserId = ul.UserId
                             where ul.Id is not null
						     and ul.IsDeleted = 0
							 and up.PermissionGroupId = pg.Id)  as UserCount
                            from PermissionGroup pg
                            where CompanyId ='{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, comId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    SqlDataReader reader;
                    long result = SelectRecords(cmd, out reader);
                    List<PermissionGroup> list = new List<PermissionGroup>();

                    using (reader)
                    {
                        while (reader.Read())
                        {
                            PermissionGroup permissionGroupObject = new PermissionGroup();
                            FillObject(permissionGroupObject, reader);
                            permissionGroupObject.UserCount = Convert.ToInt32(reader["UserCount"]);
                            list.Add(permissionGroupObject);
                        } 
                        reader.Close();
                    }

                    return list;
                }

            }
            catch (Exception) { return null; }
        }

        public List<PermissionGroup> GetAllPermissionGroupForCurrentEmployee(Guid comId)
        {
            string sqlQuery = @"select *,
                            (select distinct count(ul.id) from UserCompany uc
                             left join UserLogin ul 
	                         on ul.UserId = uc.UserId
                             left join UserPermission up 
	                         on up.UserId = ul.UserId
                             Left Join Employee emp
							 on emp.UserId = ul.UserId
                             where ul.Id is not null
						     and ul.IsDeleted = 0
							 and up.PermissionGroupId = pg.Id
                             and emp.IsCurrentEmployee = 1)  as UserCount
                             ,(select distinct count(ul.id) from UserCompany uc
                             left join UserLogin ul 
	                         on ul.UserId = uc.UserId
                             left join UserPermission up 
	                         on up.UserId = ul.UserId
                             Left Join Employee emp
							 on emp.UserId = ul.UserId
                             where ul.Id is not null
						     and ul.IsDeleted = 0
							 and up.PermissionGroupId = pg.Id)  as TotalUserCount
                            from PermissionGroup pg
                            where CompanyId ='{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, comId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    SqlDataReader reader;
                    long result = SelectRecords(cmd, out reader);
                    List<PermissionGroup> list = new List<PermissionGroup>();

                    using (reader)
                    {
                        while (reader.Read())
                        {
                            PermissionGroup permissionGroupObject = new PermissionGroup();
                            FillObject(permissionGroupObject, reader);
                            permissionGroupObject.UserCount = Convert.ToInt32(reader["UserCount"]);
                            permissionGroupObject.TotalUserCount = Convert.ToInt32(reader["TotalUserCount"]);
                            list.Add(permissionGroupObject);
                        }
                        reader.Close();
                    }

                    return list;
                }

            }
            catch (Exception) { return null; }
        }
    }
}
