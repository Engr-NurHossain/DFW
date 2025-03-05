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
	public partial class PayrollTermSheetManagerDataAccess
	{
        public DataTable GetPayrollTermSheetManagerList(Guid termSheetId)
        {
            string sqlQuery = @"select 
                                pal.Id,
                                pal.ManagerId,
                                emp.FirstName+' '+emp.LastName as Name,
                                pal.Value,
                                pal.Type
                                from PayrollTermSheetManager pal
                                LEFT JOIN Employee emp on emp.UserId=pal.ManagerId
                                Where pal.TermSheetId='{0}'                                
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, termSheetId);
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
