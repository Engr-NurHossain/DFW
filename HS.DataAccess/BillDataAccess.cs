using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Text.RegularExpressions;

namespace HS.DataAccess
{
	public partial class BillDataAccess
	{
        public DataSet GetAllVendorBillViewByComanyId(Guid companyId, VendorBillFilter filter)
        {
            string searchTextQuery = "";
            string subquery = "";
            string subquery1 = "";
            string statusquery = "";
            string filterTextQuery = "";
            string DateQuery = "";
            string DateQuery2 = "";
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                searchTextQuery = "Type like @SearchText or PaymentStatus like @SearchText or BillNo like @SearchText or SupplierName like @SearchText or TotalAmount like @SearchText or OpenBalance like @SearchText or DueDate like @SearchText and ";
            }
            if(filter.filterText != null && filter.filterText != "-1")
            {
                filterTextQuery = string.Format("Type = '{0}' and", filter.filterText);
            }
            if (!string.IsNullOrWhiteSpace(filter.BillStatus))
            {
                if(filter.BillStatus == "Paid")
                {
                    statusquery = string.Format("and PaymentStatus = 'Paid'");
                }
                else if(filter.BillStatus == "Unpaid")
                {
                    statusquery = string.Format("and (PaymentStatus = 'Open' or PaymentStatus = 'Partial')");
                }
            }
            if (filter.StartDate != new DateTime() && filter.EndDate != new DateTime())
            {
                string StartDateQuery = filter.StartDate.ToString("yyyy-MM-dd 00:00:00.000");
                string EndDateQuery = filter.EndDate.ToString("yyyy-MM-dd 23:59:59.999");

                DateQuery = string.Format("and bil.PaymentDueDate between '{0}' and '{1}'", StartDateQuery, EndDateQuery);
                DateQuery2 = string.Format("Where bp.TransacationDate between '{0}' and '{1}'", StartDateQuery, EndDateQuery);
            }
            string sqlQuery = @"
                                declare @pagesize int
                                declare @pageno int 
                                set @pagesize = " + filter.UnitPerPage + @"
                                set @pageno = " + filter.PageNumber + @"
                                declare @pagestart int
                                set @pagestart=(@pageno-1)* @pagesize  

                                  select  * into #temptable from(select PaymentMethod,bil.Id		,
                                0 as PaymentId,	BillNo,			
                                PaymentDueDate as DueDate,		Amount as TotalAmount,	
                                PaymentDue as OpenBalance,0 as Payment ,PaymentStatus, 
                                'Bill' as [Type],
                                sup.CompanyName as SupplierName,
                                sup.Id as SupplierId,
                                isnull(emp.UserId, '00000000-0000-0000-0000-000000000000') as EMPID
                                from Bill bil
                                left join Employee emp on emp.UserId = bil.EmployeeId
                                left join Supplier sup 
                                on bil.SupplierId = sup.Id

                                where 
                                PaymentStatus != 'Init' 
                                {3}
                                {4}
                                union 
                                select PaymentMethod,0 as Id	, 
                                Id as PaymentId,	'-' as BillNo,	
                                bp.TransacationDate as DueDate,	bp.Amount as Amount,	
                                0 as OpenBalance, bp.Amount as Payment ,[Status] as PaymentStatus, 
                                'Payment' as [Type],
                                '' as SupplierName,
                                0 as SupplierId,
                                '00000000-0000-0000-0000-000000000000' as EMPID
                                from BillPayment bp {5}) a
                                select TOP (@pagesize) * into #Testtable from #temptable c
                                 where {1}{2}  c.Id NOT IN(Select TOP (@pagestart) Id from bill {0})
                                 {0}
                                 select * from #Testtable
                                 select sum(TotalAmount) as TTAmount ,sum(OpenBalance) as TotalOpenBalance from #TestTable where Type='Bill'
		                        drop table #temptable
                                drop table #Testtable
                                ";
            #region Order
            if (!string.IsNullOrWhiteSpace(filter.Order))
            {
                if (filter.Order == "ascending/billid")
                {
                    subquery = "order by BillNo asc";

                }
                else if (filter.Order == "descending/billid")
                {
                    subquery = "order by BillNo desc";

                }
                else if (filter.Order == "ascending/suppliername")
                {
                    subquery = "order by SupplierName asc";

                }
                else if (filter.Order == "descending/suppliername")
                {
                    subquery = "order by SupplierName desc";

                }
                else if (filter.Order == "ascending/totalamount")
                {
                    subquery = "order by TotalAmount asc";

                }
                else if (filter.Order == "descending/totalamount")
                {
                    subquery = "order by TotalAmount desc";

                }
                else if (filter.Order == "ascending/balance")
                {
                    subquery = "order by OpenBalance asc";

                }
                else if (filter.Order == "descending/balance")
                {
                    subquery = "order by OpenBalance desc";

                }
                else if (filter.Order == "ascending/duedate")
                {
                    subquery = "order by DueDate asc";

                }
                else if (filter.Order == "descending/duedate")
                {
                    subquery = "order by DueDate desc";

                }
                else if (filter.Order == "ascending/duedate")
                {
                    subquery = "order by DueDate asc";

                }
 

                else if (filter.Order == "ascending/status")
                {
                    subquery = "order by PaymentStatus asc";

                }
                else if (filter.Order == "descending/status")
                {
                    subquery = "order by  PaymentStatus desc";

                }

            
            }
            else
            {
                subquery = "order by Id desc";
                subquery1 = "order by Id desc";
            }
            #endregion

            sqlQuery = string.Format(sqlQuery,subquery, searchTextQuery,filterTextQuery, statusquery, DateQuery, DateQuery2);
             
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", filter.SearchText)));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllBillReportByCompanyId(Guid companyId,DateTime?Start,DateTime?End, FilterReportModel filter)
        {
            string sqlQuery = @"";

            string subquery = "";
            if (!string.IsNullOrWhiteSpace(filter.searchtext))
            {
                subquery += string.Format("and (bl.BillNo like '%{0}%' or sup.Name like '%{0}%' or sup.CompanyName like '%{0}%')", filter.searchtext);
            }
            if (!string.IsNullOrWhiteSpace(filter.id))
            {
                subquery += string.Format("and bl.Amount like '%{0}%'", filter.id);
            }
            if (!string.IsNullOrWhiteSpace(filter.convertmindate) && !string.IsNullOrWhiteSpace(filter.convertmaxdate))
            {
                var datemin = Convert.ToDateTime(filter.convertmindate);
                var date = Convert.ToDateTime(filter.convertmaxdate);
                subquery += string.Format("and bl.PaymentDueDate between '{0}' and '{1}'", datemin.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.convertmindate))
            {
                var date = Convert.ToDateTime(filter.convertmindate);
                subquery += string.Format("and bl.PaymentDueDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.convertmaxdate))
            {
                var date = Convert.ToDateTime(filter.convertmaxdate);
                subquery += string.Format("and bl.PaymentDueDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }

            if (Start.HasValue && End.HasValue)
            {
                sqlQuery = @"declare @CompanyId  uniqueidentifier
                            set @CompanyId = '{0}'
 
                            select bl.Id, bl.BillNo as [Bill No.] 
                            ,sup.Name as [Supplier Name]
                            ,sup.CompanyName as [Supplier Company]
                            ,bl.Amount as [Amount]
                            ,convert(date,bl.PaymentDueDate) as [Payment Due Date] into #TestTable
                            from Bill bl
                            left join Supplier sup on bl.SupplierId = sup.Id
                            where sup.CompanyId = @CompanyId
                            and bl.CompanyId =  @CompanyId
                            and bl.PaymentStatus!='init'
                            and (bl.PaymentDate between '{1}' and '{2}' 
	                            or bl.PaymentDueDate between '{1}' and '{2}' )
                            {3}
                            order by bl.BillNo asc

                            select * from #TestTable order by Id desc
							drop table #TestTable";
                sqlQuery = string.Format(sqlQuery, companyId, Start.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), End.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), subquery);
            }
            else
            {
                sqlQuery = @"declare @CompanyId  uniqueidentifier
                            set @CompanyId = '{0}'
 
                            select bl.Id,bl.BillNo as [Bill No.] 
                            ,sup.Name as [Supplier Name]
                            ,sup.CompanyName as [Supplier Company]
                            ,bl.Amount as [Amount]
                            ,convert(date,bl.PaymentDueDate) as [Payment Due Date] into #TestTable
                            from Bill bl
                            left join Supplier sup on bl.SupplierId = sup.Id
                            where sup.CompanyId = @CompanyId
                            and bl.CompanyId =  @CompanyId
                            and bl.PaymentStatus!='init' 
                            {1}
                            order by bl.BillNo asc

                            select * from #TestTable order by Id desc
							drop table #TestTable";
                sqlQuery = string.Format(sqlQuery, companyId, subquery);
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
        public DataSet GetAllBillByCompanyId(Guid companyId,DateTime?Start, DateTime? End, FilterReportModel filter,string order)
        {
            string sqlQuery = @"";
            sqlQuery = string.Format(sqlQuery, companyId);
            string orderquery = "";
            string orderquery1 = "";
            #region Order
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/billno")
                {
                    orderquery = "order by #cd.[BillNo] asc";
                    orderquery1 = "order by [BillNo] asc";
                }
                else if (order == "descending/billno")
                {
                    orderquery = "order by #cd.[BillNo] desc";
                    orderquery1 = "order by [BillNo] desc";
                }
                else if (order == "ascending/supplier")
                {
                    orderquery = "order by #cd.SupplierName asc";
                    orderquery1 = "order by SupplierName asc";
                }
                else if (order == "descending/supplier")
                {
                    orderquery = "order by #cd.SupplierName desc";
                    orderquery1 = "order by SupplierName desc";
                }
                else if (order == "ascending/company")
                {
                    orderquery = "order by #cd.[SupplierCompanyName] asc";
                    orderquery1 = "order by [SupplierCompanyName] asc";
                }
                else if (order == "descending/company")
                {
                    orderquery = "order by #cd.[SupplierCompanyName] desc";
                    orderquery1 = "order by [SupplierCompanyName] desc";
                }
                else if (order == "ascending/duedate")
                {
                    orderquery = "order by #cd.[PaymentDueDate] asc";
                    orderquery1 = "order by [PaymentDueDate] asc";
                }
                else if (order == "descending/duedate")
                {
                    orderquery = "order by #cd.[PaymentDueDate] desc";
                    orderquery1 = "order by [PaymentDueDate] desc";
                }
                else if (order == "ascending/amount")
                {
                    orderquery = "order by #cd.[Amount] asc";
                    orderquery1 = "order by [Amount] asc";
                }
                else if (order == "descending/amount")
                {
                    orderquery = "order by #cd.[Amount] desc";
                    orderquery1 = "order by [Amount] desc";
                }


                else
                {
                    orderquery = "order by #cd.[Id]  desc";
                    orderquery1 = "order by Id desc";
                }

            }
            else
            {
                orderquery = "order by #cd.[Id] desc";
                orderquery1 = "order by Id desc";
            }
            #endregion
            string subquery = "";
            if (!string.IsNullOrWhiteSpace(filter.searchtext))
            {
                subquery += string.Format("and (bl.BillNo like '%{0}%' or sup.Name like '%{0}%' or sup.CompanyName like '%{0}%')", filter.searchtext);
            }
            if (!string.IsNullOrWhiteSpace(filter.id))
            {
                subquery += string.Format("and bl.Amount like '%{0}%'", filter.id);
            }
            if (!string.IsNullOrWhiteSpace(filter.convertmindate) && !string.IsNullOrWhiteSpace(filter.convertmaxdate))
            {
                var datemin = Convert.ToDateTime(filter.convertmindate);
                var date = Convert.ToDateTime(filter.convertmaxdate);
                subquery += string.Format("and bl.PaymentDueDate between '{0}' and '{1}'", datemin.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.convertmindate))
            {
                var date = Convert.ToDateTime(filter.convertmindate);
                subquery += string.Format("and bl.PaymentDueDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }
            else if (!string.IsNullOrWhiteSpace(filter.convertmaxdate))
            {
                var date = Convert.ToDateTime(filter.convertmaxdate);
                subquery += string.Format("and bl.PaymentDueDate between '{0}' and '{1}'", date.ToString("yyyy-MM-dd 00:00:00.000"), date.ToString("yyyy-MM-dd 23:59:59.999"));
            }

            if (Start.HasValue && End.HasValue)
            {
                sqlQuery = @"Declare @CompanyId uniqueidentifier
                            set @CompanyId ='{0}'
                            select bl.*
                            ,sup.Name as SupplierName
                            ,sup.CompanyName as SupplierCompanyName into #TestTable
                            from Bill bl
                            left join Supplier sup on bl.SupplierId = sup.Id
                            where sup.CompanyId = @CompanyId
                            and bl.CompanyId =  @CompanyId
                            and bl.PaymentStatus!='init'
                            and bl.PaymentDate between '{1}' and '{2}' 
                            {3}
                            order by bl.BillNo asc

                            select * from #TestTable {4}
							select sum(Amount) as TotalAmount from #TestTable
							drop table #TestTable";
                sqlQuery = string.Format(sqlQuery, companyId,Start.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), End.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), subquery,orderquery1);
            }
            else
            {
                sqlQuery = @"Declare @CompanyId uniqueidentifier
                            set @CompanyId ='{0}'
                            select bl.*
                            ,sup.Name as SupplierName
                            ,sup.CompanyName as SupplierCompanyName into #TestTable
                            from Bill bl
                            left join Supplier sup on bl.SupplierId = sup.Id
                            where sup.CompanyId = @CompanyId
                            and bl.CompanyId =  @CompanyId
                            and bl.PaymentStatus!='init'
                            {1}
                            order by bl.BillNo asc

                            select * from #TestTable {2}
							select sum(Amount) as TotalAmount from #TestTable
							drop table #TestTable";
                sqlQuery = string.Format(sqlQuery,companyId, subquery,orderquery1);
            }

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

        public DataSet GetVendorBillDeatailByVendorId(int id, string order,int PageNo,int PageSize,string SearchText)
        {
            string subquery = "";
            string sqlQuery = @"declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)* @pagesize
                                set @pageend = @pagesize
                                declare @supplierId int 
                                set @supplierId = {0}
                                select PaymentMethod,
                                Id	,
                                0 as PaymentId,
                                BillNo,
                                JobName,
                                PaymentDueDate as DueDate,
                                Amount as TotalAmount,
                                PaymentDue as OpenBalance,
                                0 as Payment ,
                                PaymentDate as CreatedDate,
                                PaymentStatus,
                                'Bill' as [Type] ,b.SupplierId, b.InvoiceId, b.PurchaseOrderId, b.Notes into #bills  from Bill b
                                where b.SupplierId = @supplierId and PaymentStatus != 'Init' and PaymentDue > 0

                               select * into #BillFilterData
                                FROM #bills {2}

							   SELECT TOP (@pageend)
                                  * Into #billsdata
                                FROM #BillFilterData _bill
                                where   Id NOT IN(Select TOP (@pagestart)  Id from #bills _bill {1})
								{1}

								select * from #billsdata

								select  at.Name as [Type] , bd.CustomerBillId from EquipmentType at 
								left join BillDetail bd on bd.AccoutTypeId = at.Id
								left join #billsdata bill on bill.Id = bd.CustomerBillId
								where bill.SupplierId = @supplierId
                                group by at.Name, bd.CustomerBillId

                                select count(Id) as TotalCount from Bill
								where SupplierId = @supplierId and PaymentStatus != 'Init' and PaymentDue > 0

								drop table #bills

								drop table #billsdata

								drop table #BillFilterData

								";
            string filtertext = "";

            if (!string.IsNullOrWhiteSpace(SearchText) && SearchText != "undefined")
            {
                filtertext =  string.Format(" Where BillNo like '%{0}%' or JobName like '%{0}%' or TotalAmount like '%{0}%' or OpenBalance like '%{0}%' or CreatedDate like '%{0}%' or PaymentStatus like '%{0}%' or Type like '%{0}%' or SupplierId like '%{0}%' or InvoiceId like '%{0}%' or Notes like '%{0}%' or PurchaseOrderId like '%{0}%' ", SearchText);
            }
            if (!string.IsNullOrWhiteSpace(order))
            {
                if(order == "date-asc")
                {
                    subquery = "order by _bill.CreatedDate asc";
                }
                else if(order == "date-desc")
                {
                    subquery = "order by _bill.CreatedDate desc";
                }
                else if(order=="date-desc-id-desc")
                {
                    subquery = "order by _bill.CreatedDate desc,_bill.Id desc";
                }
                if (order == "type-asc")
                {
                    subquery = "order by _bill.BillNo asc";
                }
                else if (order == "type-desc")
                {
                    subquery = "order by _bill.BillNo desc";
                }
                if (order == "no-asc")
                {
                    subquery = "order by _bill.Id asc";
                }
                else if (order == "no-desc")
                {
                    subquery = "order by _bill.Id desc";
                }
                if (order == "payee-asc")
                {
                    subquery = "order by _bill.Id asc";
                }
                else if (order == "payee-desc")
                {
                    subquery = "order by _bill.Id desc";
                }
                if (order == "category-asc")
                {
                    subquery = "order by _bill.Id asc";
                }
                else if (order == "category-desc")
                {
                    subquery = "order by _bill.Id desc";
                }
                if (order == "amount-asc")
                {
                    subquery = "order by _bill.Id asc";
                }
                else if (order == "amount-desc")
                {
                    subquery = "order by _bill.Id desc";
                }
            }
            sqlQuery = string.Format(sqlQuery, id, subquery, filtertext);

            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", PageNo));
                    AddParameter(cmd, pInt32("pagesize", PageSize));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetVendorBillPayableList(Guid companyid, DateTime? Start, DateTime? End,string order)
        {
            string orderquery = "";
            string orderquery1 = "";
            #region Order
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/type")
                {
                    orderquery = "order by #packagedataFilter.[BillNo] asc";
                    orderquery1 = "order by [BillNo] asc";
                }
                else if (order == "descending/type")
                {
                    orderquery = "order by #packagedataFilter.[BillNo] desc";
                    orderquery1 = "order by [BillNo] desc";
                }
                else if (order == "ascending/tamount")
                {
                    orderquery = "order by #packagedataFilter.TotalAmount asc";
                    orderquery1 = "order by TotalAmount asc";
                }
                else if (order == "descending/tamount")
                {
                    orderquery = "order by #packagedataFilter.TotalAmount desc";
                    orderquery1 = "order by TotalAmount desc";
                }
                else if (order == "ascending/balance")
                {
                    orderquery = "order by #packagedataFilter.[OpenBalance] asc";
                    orderquery1 = "order by [OpenBalance] asc";
                }
                else if (order == "descending/balance")
                {
                    orderquery = "order by #packagedataFilter.[OpenBalance] desc";
                    orderquery1 = "order by [OpenBalance] desc";
                }
                else if (order == "ascending/duedate")
                {
                    orderquery = "order by #packagedataFilter.[DueDate] asc";
                    orderquery1 = "order by [DueDate] asc";
                }
                else if (order == "descending/duedate")
                {
                    orderquery = "order by #packagedataFilter.[DueDate] desc";
                    orderquery1 = "order by [DueDate] desc";
                }
                else if (order == "ascending/status")
                {
                    orderquery = "order by #packagedataFilter.[PaymentStatus] asc";
                    orderquery1 = "order by [PaymentStatus] asc";
                }
                else if (order == "descending/status")
                {
                    orderquery = "order by #packagedataFilter.[PaymentStatus] desc";
                    orderquery1 = "order by [PaymentStatus] desc";
                }
           
           

            }
            else
            {
                orderquery = "order by #packagedataFilter.[Id] desc";
                orderquery1 = "order by Id desc";
            }
            #endregion
            string DateQuery = "";
            if (Start != new DateTime() && End != new DateTime())
            {
                Start = Convert.ToDateTime(Start).SetZeroHour().ClientToUTCTime();
                End = Convert.ToDateTime(End).SetMaxHour().ClientToUTCTime();

                string StartDateQuery = Start.Value.ToString("yyyy-MM-dd 00:00:00.000");
                string EndDateQuery = End.Value.ToString("yyyy-MM-dd 23:59:59.999");

                DateQuery = string.Format("and PaymentDueDate between '{0}' and '{1}'", StartDateQuery, EndDateQuery);
            }

            string sqlQuery = @"select PaymentMethod,Id		,0 as PaymentId,	BillNo,			PaymentDueDate as DueDate,		Amount as TotalAmount,	PaymentDue as OpenBalance,0 as Payment ,PaymentStatus, 'Bill' as [Type]  from Bill b
                                where b.CompanyId = '{0}' and PaymentStatus = 'Open' {1} {2}";

            sqlQuery = string.Format(sqlQuery, companyid, DateQuery, orderquery1);

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

        public DataTable GetVendorBillAmountPanelByCompanyId(Guid companyid)
        {
            string sqlQuery = @"select  * into #Bill from Bill where PaymentStatus in('Open', 'Partial') and CompanyId='{0}'
                                select (select SUM( PaymentDue) from #Bill where PaymentDueDate > GETDATE()) VendorBillOpen
                                , (select SUM( PaymentDue) from #Bill  where PaymentDueDate < GETDATE()) VendorBillOverDue
                                ,(select  SUM( Amount) from BillPayment where CompanyId='{0}') VendorBillPaid
                                Drop table #Bill";

            sqlQuery = string.Format(sqlQuery, companyid);

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

        public DataTable GetVendorDetailBillAmountPanelByCompanyId(Guid companyid, int id)
        {
            string sqlQuery = @"select  * into #Bill from Bill where SupplierId={1} ANd PaymentStatus in('Open', 'Partial') and CompanyId='{0}'


select (select SUM( PaymentDue) from #Bill where PaymentDueDate > GETDATE()) VendorBillOpen
, (select SUM( PaymentDue) from #Bill  where PaymentDueDate < GETDATE()) VendorBillOverDue,
(select  SUM( Amount) from BillPayment where CompanyId='{0}') VendorBillPaid
 

Drop table #Bill";

            sqlQuery = string.Format(sqlQuery, companyid, id);

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
        public DataTable GetExpenseSummary(Guid companyId)
        {
            string sqlQuery = @"
                    declare @CompanyId uniqueidentifier
                    set @CompanyId = '{0}'

                    --Open bill
                    select SUM(PaymentDue) as OpenBill
                    ,(select SUM(Amount) from BillPayment where TransacationDate 
                    between '{1}' --last month
                    and '{2}' -- this month
                    and CompanyId=@CompanyId) as Paid
                    ,(--over due
                    select SUM(PaymentDue) as OverDue 
                    from Bill 
                    where CompanyId = @CompanyId
                    and PaymentStatus !='Init'
                    and PaymentStatus !='Paid'
                    and PaymentDate <= '{3}' --less than today
                    ) as OverDue

                    from Bill where CompanyId = @CompanyId
                    and PaymentStatus !='Init'
                    and PaymentStatus !='Paid'
                    ";

            

            try
            {
                sqlQuery = string.Format(sqlQuery,
                    companyId,
                    DateTime.Today.AddDays(1).AddMonths(-1).ToString("yyyy-MM-dd HH:mm:ss"),
                    DateTime.Today.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"),
                    DateTime.Today.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss")
                    );
                //2018-01-17 00:00:00
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
