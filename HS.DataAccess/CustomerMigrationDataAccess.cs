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
	public partial class CustomerMigrationDataAccess
	{
		public CustomerMigrationDataAccess(string ConStr) : base(ConStr) { }


        public DataTable IsSameWorkOrder(string Notes)
        {
            string sqlQuery = @"select * from Ticket
                                where Message like '<p>Id: {0}<%'";
            try
            {
                sqlQuery = string.Format(sqlQuery, Notes);
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
