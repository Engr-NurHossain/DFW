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
	public partial class PermissionGroupMapDataAccess
	{
        public PermissionGroupMapDataAccess(string ConStr) : base(ConStr) { }

        public bool DeleteExistingPermissionGroupMapByPermissionIdAndGroupId(int pid, int gid, Guid comid)
        {
            string sqlQuery = @"delete PermissionGroupMap where PermissionId = {0} and PermissionGroupId = {1} and CompanyId = '{2}'";

            try
            {
                sqlQuery = string.Format(sqlQuery, pid, gid, comid);
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

        public bool DeleteExistingPermissionGroupMapByPermissionParentIdAndGroupId(int parentId,int GroupId, Guid comid)
        {
            string sqlQuery = @"delete from PermissionGroupMap 
                                where PermissionId in (select id from Permission where ParentId = {0})
                                and PermissionGroupId = {1}
                                and CompanyId = '{2}'
                                delete from PermissionGroupMap where PermissionId = {0} 
                                and PermissionGroupId = {1}
                                and CompanyId = '{2}'
";
            try
            {
                sqlQuery = string.Format(sqlQuery,parentId,GroupId, comid);
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

        public bool InsertGroupMapClone(int cloneId,string permissionId, Guid comid)
        {
            var permissionIdList = permissionId.Split(',');
            string sqlQuery = "";
            foreach (var item in permissionIdList)
            {
                sqlQuery += @"Insert into PermissionGroupMap values('"+cloneId+"','"+item+"',1, '{0}') ";
            }
            
            try
            {
                sqlQuery = string.Format(sqlQuery, comid);
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
