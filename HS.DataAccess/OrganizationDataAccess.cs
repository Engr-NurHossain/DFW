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
	public partial class OrganizationDataAccess
	{
        public OrganizationDataAccess(ClientContext context, string ConnectionString) : base(context, ConnectionString) { }
        public OrganizationDataAccess(string ConnectionString) : base(ConnectionString) { }

        public OrganizationList GetAllOrganizationsByUsername (string username)
        {
            string SqlQuery = @"select * from Organization org 
                                left join UserOrganization uorg 
                                on org.CompanyId = uorg.CompanyId
                                where uorg.UserName ='{0}'";

            SqlQuery = string.Format(SqlQuery,username);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(SqlQuery))
                {
                    return GetList(cmd,-1);
                } 
            }
            catch (Exception)
            {
                return null;
            }

             
        }
    }	
}
