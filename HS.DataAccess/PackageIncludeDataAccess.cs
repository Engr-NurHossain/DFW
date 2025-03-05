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
	public partial class PackageIncludeDataAccess
	{
        public PackageIncludeList GetPackageIncludeListByCustomerId(Guid customerId)
        {
            string sqlQuery = @"select pinc.* from PackageInclude pinc 
                                left join PackageCustomer pc on pc.PackageId = pinc.PackageId
                                where pc.CustomerId ='{0}'
                                ";
            sqlQuery = string.Format(sqlQuery, customerId);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {

                    return GetList(cmd, -1);
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }	
}
