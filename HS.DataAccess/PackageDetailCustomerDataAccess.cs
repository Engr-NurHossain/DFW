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
	public partial class PackageDetailCustomerDataAccess
	{
        public bool DeleteAllPackageDetailCustomerByCustomerIdAndComapnyId(Guid CustomerId, Guid CompanyId)
        {
            string SqlQuery = @"DELETE FROM PackageDetailCustomer
                                where CustomerId ='{0}' and CompanyId = '{1}'";
            SqlQuery = string.Format(SqlQuery, CustomerId, CompanyId);
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
    }	
}
