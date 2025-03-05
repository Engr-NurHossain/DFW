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
	public partial class TransactionHistoryDataAccess
    {
        public TransactionHistoryDataAccess() { }
        public TransactionHistoryDataAccess(string ConStr) : base(ConStr) { }

        public void InsertTransactionHistoryList(string sql)
        {
            try
            {
                SqlCommand cmd = GetSQLCommand(sql);
                 
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (Exception e)
            {

            }
             
        }
        public DataTable GetAllTransactionHistoryByCompanyId(Guid CompanyId,DateTime? Start, DateTime? End)
        {
            string sqlQuery = @"Declare @CompanyId uniqueidentifier
                            set @CompanyId ='{0}'
                            select th.*
                            ,inv.InvoiceId as [InvoiceNumber]
                            ,cus.Title +' '+cus.FirstName +' '+cus.LastName as [CustomerName]
                            ,tr.TransacationDate 
                                from TransactionHistory th
                            left join [Transaction] tr 
	                            on th.TransactionId = tr.Id
                            left join Customer cus 
	                            on cus.CustomerId = tr.CustomerId
                            left join Invoice inv 
	                            on th.InvoiceId =inv.Id
                            where tr.CompanyId = @CompanyId ";
            if (Start.HasValue && End.HasValue)
            {
                sqlQuery += @" and tr.TransacationDate between '{1}' and '{2}'";
                sqlQuery = string.Format(sqlQuery, CompanyId,Start.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), End.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            else
            {
                sqlQuery = string.Format(sqlQuery, CompanyId);
            }
            try
            { 
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
