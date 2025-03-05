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
    public partial class BillPaymentHistoryDataAccess
    {
        public void InsertBillPaymentHistoryList(string sql)
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

        public DataTable GetAllBillPaymentHistoryByCompanyId(Guid companyId,DateTime?Start,DateTime?End)
        {
            string sqlQuery = @"declare @CompanyId uniqueidentifier
                                set @CompanyId = '{0}'
                                select bph.*
                                ,bl.BillNo 
                                ,sup.Name as SupplierName
                                ,sup.CompanyName as SupplierCompanyName
                                ,bp.TransacationDate
                                from BillPaymentHistory bph
                                left join Bill bl 
                                on bl.Id = bph.InvoiceId
                                left join Supplier sup 
                                on sup.Id = bl.SupplierId
                                left join BillPayment bp 
                                on bp.Id = bph.BillPaymentId

                                where bl.CompanyId = @CompanyId
                                and bp.CompanyId =@CompanyId
                                and sup.CompanyId =@CompanyId";

            if (Start.HasValue && End.HasValue)
            {
                sqlQuery += " and TransacationDate between '{1}' and '{2}'";
                sqlQuery = string.Format(sqlQuery, companyId,Start.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), End.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            else
            {
                sqlQuery = string.Format(sqlQuery, companyId);
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

        public DataTable GetAllBillPaymentHistoryReportByCompanyId(Guid companyId,DateTime?Start,DateTime?End)
        {
            string sqlQuery = @"Declare @CompanyId uniqueidentifier
                                set @CompanyId ='{0}'
                            select bl.BillNo
	                        ,sup.Name as [SupplierName]
	                        ,sup.CompanyName as [Supplier CompanyName]
                            , bph.Amount as [Total Amount]
                            ,bph.Balance as [Balance Due] 
                            ,bp.TransacationDate as [Transaction Date]
                            from BillPaymentHistory bph
                            left join Bill bl 
                            on bl.Id = bph.InvoiceId
                            left join Supplier sup 
                            on sup.Id = bl.SupplierId
                            left join BillPayment bp 
                            on bp.Id = bph.BillPaymentId

                            where bl.CompanyId = @CompanyId
                            and bp.CompanyId =@CompanyId
                            and sup.CompanyId =@CompanyId
                            ";

           if(Start.HasValue&& End.HasValue)
            {
                sqlQuery += @"and bp.TransacationDate between '{1}' 
	                                and '{2}'";

                sqlQuery = string.Format(sqlQuery, companyId,Start.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), End.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            else
            {
                sqlQuery = string.Format(sqlQuery, companyId);
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
