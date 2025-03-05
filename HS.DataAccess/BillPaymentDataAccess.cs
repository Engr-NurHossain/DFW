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
	public partial class BillPaymentDataAccess
	{
        public DataTable GetAllReceivePaymentList(Guid companyId, int? paymentId, int? SupplierId, Guid? empid)
        {
            string sqlQuery = @"select
                                    bill.Id
                                ,bill.BillNo as [Description]
                                ,bill.JobName as [JobName]
                                ,bill.InvoiceId as [InvoiceId]
                                ,bill.PurchaseOrderId as [POId]
                                , bill.PaymentDate  [CreatedDate]
                                , bill.PaymentDueDate [DueDate]
                                , bill.Amount [OriginalAmount]
                                , bill.PaymentDue [OpenBalance]
                                ,0 as Payment
                                from Bill bill
                                where bill.CompanyId ='{0}'
                                and bill.PaymentDue > 0 ";
            string forSupplier = " and bill.SupplierId = {0}";
            string foremployee = " and bill.EmployeeId = '{0}'";

            string TransactionQuery = @"DECLARE @CompanyId uniqueidentifier
                                        DECLARE @BillPaymentId int 
                                        set @CompanyId ='{0}'
                                        set @BillPaymentId = {1}
                                        select bill.Id
                                        ,bill.BillNo as [Description]
                                        ,bill.JobName as [JobName]
                                        ,bill.InvoiceId as [InvoiceId]
                                        ,bill.PurchaseOrderId as [POId]
                                        , bill.PaymentDate  [CreatedDate]
                                        , bill.PaymentDueDate [DueDate]
                                        , bill.Amount [OriginalAmount]
                                        , bill.PaymentDue [OpenBalance]
                                        , (select SUM(Amount) from BillPaymentHistory where BillPaymentId = @BillPaymentId and InvoiceId = bill.Id) as Payment
                                        from Bill bill
                                        left join BillPaymentHistory th on th.InvoiceId = bill.Id and th.BillPaymentId =@BillPaymentId
  
                                        where  bill.CompanyId =@CompanyId
                                        --and bill.PaymentDue > 0 or
                                        and bill.id in (select th.InvoiceId 
		                                        from BillPayment bp 
		                                        left join BillPaymentHistory bph 
		                                        on bph.BillPaymentId = bp.Id
		                                        where bp.id in(@BillPaymentId))";

            if (paymentId.HasValue)
            {
                sqlQuery = string.Format(TransactionQuery, companyId, paymentId.Value);
            }
            else
            {
                sqlQuery = string.Format(sqlQuery, companyId);

                if(SupplierId.HasValue && SupplierId > 0)
                {
                    sqlQuery = string.Concat(sqlQuery, string.Format(forSupplier, SupplierId));
                }
                if(empid.HasValue && empid.Value != new Guid())
                {
                    sqlQuery = string.Concat(sqlQuery, string.Format(foremployee, empid));
                }
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


        public DataTable GetCheckListByPaymentIdListAndCompanyId(Guid companyId, string paymentIdList)
        {

            string sqlQuery = @"select bph.BillPaymentId as PaymentId
                                ,bph.Amount  
                                ,sup.Name as SupplierName
                                ,bl.BillNo  as BillId
                                from BillPaymentHistory bph 
                                left join Bill bl on bl.Id = bph.InvoiceId
                                left join Supplier sup on bl.SupplierId = sup.Id
                                left join BillPayment bp on bp.Id = bph.BillPaymentId
                                where bph.BillPaymentId in ({0})
                                and bp.PaymentMethod='Check'
                                and bl.CompanyId='{1}'";

            sqlQuery = string.Format(sqlQuery,  paymentIdList, companyId);
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

        public DataTable GetVendorbillPaymentListBySupplierId(int id)
        {

            string sqlQuery = @"select bph.BillPaymentId as BpaymentId, bp.Amount as Tamount, bp.TransacationDate as Ddate, bp.Status as Bstatus, bp.Type as BType, bp.PaymentMethod as Bmethod,bp.ReferenceNo as BReferenceNo,
                                s.Name as Bname
                                from BillPaymentHistory bph
                                left join BillPayment bp
                                on bp.Id = bph.BillPaymentId
                                left join Bill b
                                on b.Id = bph.InvoiceId
                                left join Supplier s
                                on s.Id = b.SupplierId
                                where b.SupplierId = {0}
								group by bph.BillPaymentId, bp.Amount, bp.TransacationDate,
								bp.Status, bp.Type, bp.PaymentMethod,bp.ReferenceNo, s.Name";

            sqlQuery = string.Format(sqlQuery, id);
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

        public DataSet GetVendorbillPaymentList(DateTime? StartDate, DateTime? EndDate,string order)
        {
            string DateQuery = "";
            string orderquery = "";
            string orderquery1 = "";
            #region Order
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/date")
                {
                    orderquery = "order by #cd.[Ddate] asc";
                    orderquery1 = "order by [Ddate] asc";
                }
                else if (order == "descending/date")
                {
                    orderquery = "order by #cd.[Ddate] desc";
                    orderquery1 = "order by [Ddate] desc";
                }
                else if (order == "ascending/paymentmethod")
                {
                    orderquery = "order by #cd.Bmethod asc";
                    orderquery1 = "order by Bmethod asc";
                }
                else if (order == "descending/paymentmethod")
                {
                    orderquery = "order by #cd.Bmethod desc";
                    orderquery1 = "order by Bmethod desc";
                }
                else if (order == "ascending/referenceno")
                {
                    orderquery = "order by #cd.[BReferenceNo] asc";
                    orderquery1 = "order by [BReferenceNo] asc";
                }
                else if (order == "descending/referenceno")
                {
                    orderquery = "order by #cd.[BReferenceNo] desc";
                    orderquery1 = "order by [BReferenceNo] desc";
                }
                else if (order == "ascending/paidamt")
                {
                    orderquery = "order by #cd.[Tamount] asc";
                    orderquery1 = "order by [Tamount] asc";
                }
                else if (order == "descending/paidamt")
                {
                    orderquery = "order by #cd.[Tamount] desc";
                    orderquery1 = "order by [Tamount] desc";
                }
                else if (order == "ascending/status")
                {
                    orderquery = "order by #cd.[Bstatus] asc";
                    orderquery1 = "order by [Bstatus] asc";
                }
                else if (order == "descending/status")
                {
                    orderquery = "order by #cd.[Bstatus] desc";
                    orderquery1 = "order by [Bstatus] desc";
                }



                else
                {
                    orderquery = "order by #cd.[BpaymentId]  desc";
                    orderquery1 = "order by BpaymentId desc";
                }

            }
            else
            {
                orderquery = "order by #cd.[BpaymentId] desc";
                orderquery1 = "order by BpaymentId desc";
            }
            #endregion
            if (StartDate != new DateTime() && EndDate != new DateTime())
            {
                StartDate = Convert.ToDateTime(StartDate).SetZeroHour().ClientToUTCTime();
                EndDate = Convert.ToDateTime(EndDate).SetMaxHour().ClientToUTCTime();

                string StartDateQuery = StartDate.Value.ToString("yyyy-MM-dd 00:00:00.000");
                string EndDateQuery = EndDate.Value.ToString("yyyy-MM-dd 23:59:59.999");

                DateQuery = string.Format("Where bp.TransacationDate between '{0}' and '{1}'", StartDateQuery, EndDateQuery);
            }

            string sqlQuery = @"select bph.BillPaymentId as BpaymentId, bp.Amount as Tamount, bp.TransacationDate as Ddate, bp.Status as Bstatus, bp.Type as BType, bp.PaymentMethod as Bmethod,
                                s.Name as Bname,
                                bp.ReferenceNo as BReferenceNo
                                into #Testtable
                                from BillPaymentHistory bph
                                left join BillPayment bp
                                on bp.Id = bph.BillPaymentId
                                left join Bill b
                                on b.Id = bph.InvoiceId
                                left join Supplier s
                                on s.Id = b.SupplierId
                                {0}
								group by bph.BillPaymentId, bp.Amount, bp.TransacationDate,
								bp.Status, bp.Type, bp.PaymentMethod, s.Name, bp.ReferenceNo
                                
                               select * from #Testtable {1}
                               select sum(Tamount) as TotalAmount from #Testtable
                               drop table #Testtable ";

            sqlQuery = string.Format(sqlQuery, DateQuery, orderquery1);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }	
}
