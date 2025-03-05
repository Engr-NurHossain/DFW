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
	public partial class EmployeePTOHourLogDataAccess
	{
        public EmployeePTOHourLogDataAccess(string ConnectionString) : base(ConnectionString)
        {

        }
        public EmployeePTOHourLogDataAccess()
        {

        }
        public DataTable GetAllEmployeePTOHourLogbyUserId(Guid userId,string Paytype)
        {
            string sqlQuery = @"Select ptolog.*,IsNull(emp.FirstName + ' ' + emp.LastName,'') As EmployeeName,emp.HireDate,emp.PayType from EmployeePTOHourLog ptolog
                                Left Join Employee emp on emp.UserId = ptolog.UserId
                                Where ptolog.UserId = '{0}' and emp.PayType = '{1}'
                                ";
            sqlQuery = string.Format(sqlQuery, userId, Paytype);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }	
}