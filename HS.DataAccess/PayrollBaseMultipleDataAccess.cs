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
	public partial class PayrollBaseMultipleDataAccess
	{
        public DataTable GetPayrollBaseMultipleList(Guid termSheetId)
        {
            string sqlQuery = @"select 
                                pal.Id,
                                lpal.DisplayText as BaseMultiple,
                                pal.Amount
                                from PayrollBaseMultiple pal
                                LEFT JOIN Lookup lpal on lpal.DataValue=pal.BaseMultiple AND lpal.DataKey='PayrollBaseMultiple'
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
