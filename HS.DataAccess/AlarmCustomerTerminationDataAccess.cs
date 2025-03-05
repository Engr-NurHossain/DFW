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
	public partial class AlarmCustomerTerminationDataAccess
	{
        public AlarmCustomerTerminationDataAccess(string ConnectionStr) : base(ConnectionStr) { }
        public DataTable GetAllAlarTerminationLogByCusId(Guid CustomerId)
        {
            string sqlQuery = @"";
          
       
            sqlQuery = @"select acus.*,emp.FirstName+' '+emp.LastName as TerminationBy from AlarmCustomerTermination acus
                        left join Employee emp on emp.UserId = acus.CreatedBy
                        where customerid= '{0}'";
           
           
            try
            {
                sqlQuery = string.Format(sqlQuery, CustomerId);
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
