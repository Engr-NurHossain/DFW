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
	public partial class BillDetailDataAccess
	{
		public DataTable GetBillDetialsListByBillId(int BillId)
        {
            string sqlQuery = @" select billd.*,
                                at.Name as EquipmentName,
                                billd.Dscription as EquipmentDescription
                                from BillDetail billd 
                                --left join Equipment eq 
                                --on eq.EquipmentId = billd.EquipmentId
                                left join AccountType at
                                on at.Id = billd.AccoutTypeId
                                where billd.CustomerBillId ='{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, BillId);
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

        public bool DeleteAllBillDetailsByBillId(int invoiceId)
        {

            string SqlQuery = @"delete from BillDetail where CustomerBillId = '{0}' ";
            SqlQuery = string.Format(SqlQuery, invoiceId);
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

        public bool InsertTransactionHistoryList(string sql)
        { 
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sql))
                {
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
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
