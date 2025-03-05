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
	public partial class PayrollAdminFeeDataAccess
	{
        public DataTable GetPayrollAdminFeeList(Guid termSheetId)
        {
            string sqlQuery = @"select 
                                pal.Id,
                                lpal.DisplayText as AdminFee,
                                pal.Amount
                                from PayrollAdminFee pal
                                LEFT JOIN Lookup lpal on lpal.DataValue=pal.AdminFee AND lpal.DataKey='PayrollAdminFee'
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
