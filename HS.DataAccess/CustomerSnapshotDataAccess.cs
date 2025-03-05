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
    public partial class CustomerSnapshotDataAccess
    {
        public CustomerSnapshotDataAccess(string ConStr) : base(ConStr){ }
        public CustomerSnapshotDataAccess() { }

        public bool ReseedCustomerAppointmentTable()
        {
            string SqlQuery = @"Delete from CustomerSnapshot 
                                DBCC CHECKIDENT('CustomerSnapshot', RESEED, 0)
                                ";
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

        public DataTable GetCustomerSnapshotDetail(string des)
        {
            string sqlQuery = @"select *
                                from CustomerSnapshot
                                where Description like '%{0}%'";
            try
            {
                sqlQuery = string.Format(sqlQuery, des);
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
