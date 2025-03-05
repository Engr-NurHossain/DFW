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
    public partial class TransactionDataAccess
    {
        public TransactionDataAccess() { }
        public TransactionDataAccess(string ConStr) : base(ConStr) { }

        public TransactionPdfModel GetTransactionPdfDataByTransactionId(int transactionhistoryId)
        {
            string SqlQuery = @"select cu.FirstName + ' ' + cu.LastName as CustomerName
                                ,cu.BusinessName as BusinessName
                                ,inv.InvoiceId as InvoiceId
                                ,tr.TransacationDate as PaymentDate
                                ,tr.PaymentMethod as PaymentMethod
                                ,trh.Amout as PaymentAmount
                                ,inv.TotalAmount as InvoiceTotal
                                ,trh.Balance as InvoicePreviousBalance
                                ,inv.BalanceDue  as InvoiceBalance
                                ,tr.CardTransactionId as TransactionId
                                ,tr.CheckNo as CheckNo

                                 from [Transaction] tr
                                left join TransactionHistory trh on trh.TransactionId = tr.Id
                                left join Invoice inv on inv.id= trh.InvoiceId
                                left join Customer cu on cu.CustomerId = inv.CustomerId

                                where trh.Id = {0}";
            SqlQuery = string.Format(SqlQuery, transactionhistoryId);

            try
            {
                using (SqlCommand cmd = GetSQLCommand(SqlQuery))
                {
                    SqlDataReader reader;
                    long result = SelectRecords(cmd, out reader);
                    using (reader)
                    {
                        if (reader.Read())
                        {
                            TransactionPdfModel transactionObject = new TransactionPdfModel();
                            transactionObject.CustomerName = reader.GetString(0);
                            transactionObject.BusinessName = reader.GetString(1);
                            transactionObject.InvoiceId = reader.GetString(2);
                            transactionObject.PaymentDate = reader.GetDateTime(3);
                            transactionObject.PaymentMethod = reader.GetString(4);
                            transactionObject.PaymentAmount = reader.GetDouble(5);
                            transactionObject.InvoiceTotal = reader.GetDouble(6);
                            transactionObject.InvoicePreviousBalance = reader.GetDouble(7);
                            transactionObject.InvoiceBalance = reader.GetDouble(8);
                            transactionObject.TransactionId = reader.GetString(9);
                            transactionObject.CheckNo = reader.GetString(10);
                            reader.Close();
                            return transactionObject;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }

        public bool ReseedTransactionTable()
        {
            string SqlQuery = @"Delete from TransactionHistory 
                                Delete from Transaction
                                DBCC CHECKIDENT('TransactionHistory', RESEED, 0) 
                                DBCC CHECKIDENT('Transaction', RESEED, 0) 
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
        public DataTable GetAllExpenseByCustomerIdAndCompanyIdAndFilter(int? customerId, Guid CompanyId, string SearchText, Guid empid, CustomerFilter filter)
        {
            var strStartDate = "";
            var strEndDate = "";
            var dateRange = "";
            string searchSql = @"
                                and (th.CardTransactionId like @SearchText
	                                or th.checkno like @SearchText
	                               
                                )";

            string sqlQuery = @"DECLARE @CustomerId uniqueidentifier
                                DECLARE @CompanyId uniqueidentifier
                                Declare @SearchText nvarchar(100)
                                declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)* @pagesize 
                                set @pageend = @pagesize
                                set @CustomerId = (select CustomerId from customer where id = {0} )
                                set @CompanyId = '{1}'
                                set @SearchText ='%{2}%'
 
                                 select th.Id, th.ExpenseDate
	                       
                                , th.[Type]
                                , th.referenceNo 
                                , th.checkno
                                , th.RefType
                                , th.FilePath
                                , th.Amount 
                                , th.[Status]
                                , th.CustomerId
                                , th.CompanyId
                                , th.PaymentMethod
                                 ,th.Description
                           
								--,emp.FirstName + ' ' + emp.LastName as TransactionUserName
                                --,receivedBy.FirstName+' '+receivedBy.LastName as ReceivedBy
                              
                             
                          

                                INTO #CustomerFund
	                                from TransactionExpense  th
                                
                                where th.CompanyId =@CompanyId 
                                and th.CustomerId = @CustomerId
                                {4}{7}
                             
                                select * into #CustomerFilterFund
								FROM #CustomerFund
								SELECT TOP (@pagesize)
                                  *
                                FROM #CustomerFilterFund
                                where Id NOT IN(Select TOP (@pagestart) Id from #CustomerFund {5})
                                {6}
                               
                                drop table #CustomerFilterFund
								drop table #CustomerFund";
            string subquery = "";
            string query = "";
            string query1 = "";

            #region Order
            if (!string.IsNullOrWhiteSpace(filter.order))
            {
                if (filter.order == "ascending/fundingdate")
                {
                    query = "order by #CustomerFilterFund.[ExpenseDate] asc";
                    query1 = "order by [ExpenseDate] asc";
                }
                else if (filter.order == "descending/fundingdate")
                {
                    query = "order by #CustomerFilterFund.[ExpenseDate] desc";
                    query1 = "order by [ExpenseDate] desc";
                }
                else if (filter.order == "ascending/receivedby")
                {
                    query = "order by #CustomerFilterFund.[ReceivedBy] asc";
                    query1 = "order by [ReceivedBy] asc";
                }
                else if (filter.order == "descending/receivedby")
                {
                    query = "order by #CustomerFilterFund.[ReceivedBy] desc";
                    query1 = "order by [ReceivedBy] desc";
                }
                else if (filter.order == "ascending/invoiceno")
                {
                    query = "order by #CustomerFilterFund.[InvoiceNo] asc";
                    query1 = "order by [InvoiceNo] asc";
                }
                else if (filter.order == "descending/invoiceno")
                {
                    query = "order by #CustomerFilterFund.[InvoiceNo] desc";
                    query1 = "order by [InvoiceNo] desc";
                }
                else if (filter.order == "ascending/description")
                {
                    query = "order by #CustomerFilterFund.[Description] asc";
                    query1 = "order by [Description] asc";
                }
                else if (filter.order == "descending/description")
                {
                    query = "order by #CustomerFilterFund.[Description] desc";
                    query1 = "order by [Description] desc";
                }
                else if (filter.order == "ascending/paymethod")
                {
                    query = "order by #CustomerFilterFund.[PaymentMethod] asc";
                    query1 = "order by [PaymentMethod] desc";
                }
                else if (filter.order == "descending/paymethod")
                {
                    query = "order by #CustomerFilterFund.[PaymentMethod] asc";
                    query1 = "order by [PaymentMethod] asc";
                }
                else if (filter.order == "ascending/checkno")
                {
                    query = "order by #CustomerFilterFund.[CheckNo] asc";
                    query1 = "order by [CheckNo] asc";
                }
                else if (filter.order == "descending/checkno")
                {
                    query = "order by #CustomerFilterFund.[CheckNo] desc";
                    query1 = "order by [CheckNo] desc";
                }
                else if (filter.order == "ascending/transactionid")
                {
                    query = "order by #CustomerFilterFund.[InvoiceIdStr] asc";
                    query1 = "order by [InvoiceIdStr] asc";
                }
                else if (filter.order == "descending/transactionid")
                {
                    query = "order by #CustomerFilterFund.[InvoiceIdStr] desc";
                    query1 = "order by [InvoiceIdStr] desc";
                }
                else if (filter.order == "ascending/amount")
                {
                    query = "order by #CustomerFilterFund.[Amount]  asc";
                    query1 = "order by Amount asc";
                }
                else if (filter.order == "descending/amount")
                {
                    query = "order by #CustomerFilterFund.[Amount]  desc";
                    query1 = "order by Amount desc";
                }


            }
            else
            {
                query = "order by #CustomerFilterFund.[ExpenseDate] desc";
                query1 = "order by ExpenseDate desc";
            }
            #endregion

            if (filter.StrStartDate.ToString("yyyy-MM-dd HH:mm:ss") != "0001-01-01 00:00:00")
            {
                strStartDate = filter.StrStartDate.SetZeroHour().ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss");
                strEndDate = filter.StrEndDate.SetMaxHour().ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss");
                dateRange = string.Format("and th.CreatedDate between '{0}' and '{1}'", strStartDate, strEndDate);
            }
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                subquery = searchSql;
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, customerId, CompanyId, SearchText, empid, subquery, query, query1, dateRange);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", filter.PageNo));
                    AddParameter(cmd, pInt32("pagesize", filter.PageSize));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet GetAllExpenseByComanyId(Guid companyId, VendorBillFilter filter)
        {
            string searchTextQuery = "";
            string subquery = "";
            string subquery1 = "";
            string filterTextQuery = "";
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                searchTextQuery = " t.Type like @SearchText or t.Status like @SearchText or t.Description like @SearchText or t.PaymentMethod like @SearchText or t.Amount like @SearchText or t.CheckNo like @SearchText or t.CardTransactionId like @SearchText or t.CreatedDate like @SearchText or  emp.FirstName+' '+emp.LastName like @SearchText or emp2.FirstName+' '+emp2.LastName like @SearchText or  t.ExpenseType like @SearchText and";
            }
            if (filter.StartDate != new DateTime() && filter.EndDate != new DateTime())
            {
                var StartDate = filter.StartDate.UTCToClientTime().SetZeroHour();
                var EndDate = filter.EndDate.UTCToClientTime().SetMaxHour();
                filterTextQuery += string.Format("ExpenseDate between '{0}' and '{1}' and ", StartDate,EndDate);
            }
            if(!string.IsNullOrEmpty(filter.ExpensedBy) && filter.ExpensedBy != "00000000-0000-0000-0000-000000000000")
            {
                filterTextQuery += string.Format(" emp.UserId = '{0}' and ", filter.ExpensedBy);
            }
            if (!string.IsNullOrEmpty(filter.ExpenseType) && filter.ExpenseType != "-1")
            {
                filterTextQuery += string.Format(" ExpenseType = '{0}' and ", filter.ExpenseType);
            }

            string sqlQuery = @"
                                declare @pagesize int
                                declare @pageno int 
                                set @pagesize = " + filter.UnitPerPage + @"
                                set @pageno = " + filter.PageNumber + @"
                                declare @pagestart int
                                set @pagestart=(@pageno-1)* @pagesize  

                                select  * into #temptable from(
                                    select t.*
                                    ,lkType.DisplayText as ExpenseTypeVal,emp.FirstName+' '+emp.LastName as ExpensedBy
                                    ,emp2.FirstName+' '+emp2.LastName as CreatedByVal  from TransactionExpense t 
                                    left join [Lookup] lkType on lkType.DataKey = 'ExpenseCategory' and lkType.DataValue = t.ExpenseType
                                    left join Employee emp on emp.UserId = t.UserId
                                    left join Employee emp2 on emp2.UserId = t.CreatedBy  where {1}{2} t.Id > 0) a

                                select  * into #Filtertable from #temptable 
                                select TOP (@pagesize) * from #Filtertable c where c.Id NOT IN(Select TOP (@pagestart) Id from TransactionExpense {0})
                                {0}
                                select Count(Id) As TotalCount from #Filtertable 
                                drop table #temptable
                                drop table #Filtertable
                                ";
            #region Order
         
            string query = "";
            string query1 = "";
            if (!string.IsNullOrWhiteSpace(filter.Order))
            {
                if (filter.Order == "ascending/fundingdate")
                {
                    query = "order by ExpenseDate asc";
           
                }
                else if (filter.Order == "descending/fundingdate")
                {
                    query = "order by [ExpenseDate] desc";
                    
                }
                else if (filter.Order == "ascending/receivedby")
                {
                    query = "order by [ReceivedBy] asc";
                 
                }
                else if (filter.Order == "descending/receivedby")
                {
                    query = "order by [ReceivedBy] desc";
                
                }
                else if (filter.Order == "ascending/invoiceno")
                {
                    query = "order by [InvoiceNo] asc";
                   
                }
                else if (filter.Order == "descending/invoiceno")
                {
                    query = "order by [InvoiceNo] desc";
            
                }
                else if (filter.Order == "ascending/description")
                {
                    query = "order by [Description] asc";
               
                }
                else if (filter.Order == "descending/description")
                {
                    query = "order by [Description] desc";
                
                }
                else if (filter.Order == "ascending/paymethod")
                {
                    query = "order by [PaymentMethod] asc";
                
                }
                else if (filter.Order == "descending/paymethod")
                {
                    query = "order by [PaymentMethod] desc";
                  
                }
                else if (filter.Order == "ascending/checkno")
                {
                    query = "order by [CheckNo] asc";
              
                }
                else if (filter.Order == "descending/checkno")
                {
                    query = "order by [CheckNo] desc";
                
                }
                else if (filter.Order == "ascending/transactionid")
                {
                    query = "order by [InvoiceIdStr] asc";
                  
                }
                else if (filter.Order == "descending/transactionid")
                {
                    query = "order by [InvoiceIdStr] desc";
      
                }
                else if (filter.Order == "ascending/amount")
                {
                    query = "order by [Amount]  asc";
              
                }
                else if (filter.Order == "descending/amount")
                {
                    query = "order by [Amount]  desc";
            
                }
                else if (filter.Order == "ascending/expensetype")
                {
                    query = "order by [ExpenseType]  asc";
                 
                }
                else if (filter.Order == "descending/expensetype")
                {
                    query = "order by [ExpenseType]  desc";
                 
                }
                else if (filter.Order == "ascending/status")
                {
                    query = "order by [Status]  asc";
               
                }
                else if (filter.Order == "descending/status")
                {
                    query = "order by [Status]  desc";
                   
                }
                else if (filter.Order == "ascending/ticket")
                {
                    query = "order by [TicketNo]  asc";

                }
                else if (filter.Order == "descending/ticket")
                {
                    query = "order by [TicketNo]  desc";

                }
            }
            else
            {
                query = "order by [Id] desc";
          
            }
            #endregion

            sqlQuery = string.Format(sqlQuery, query, searchTextQuery, filterTextQuery);

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
        public DataTable GetAllExpenseByCustomerIdAndCompanyIdAndFilter(int? customerId, Guid CompanyId, string SearchText, Guid empid)
        {

            string searchSql = @"
                                and (th.CardTransactionId like @SearchText
	                                or th.checkno like @SearchText
	                              
                                )";

            string sqlQuery = @"DECLARE @CustomerId uniqueidentifier
                                DECLARE @CompanyId uniqueidentifier
                                Declare @SearchText nvarchar(100)
                            
                                set @CustomerId = (select CustomerId from customer where id = {0} )
                                set @CompanyId = '{1}'
                                set @SearchText ='%{2}%'
 
                                 select th.Id, th.ExpenseDate
	                       
                                , th.[Type]
                                , th.referenceNo 
                                , th.checkno
                                , th.RefType
                              
                                , th.Amount 
                                , th.[Status]
                                , th.CustomerId
                                , th.CompanyId
                                , th.PaymentMethod
                                 ,th.Description
                           
								--,emp.FirstName + ' ' + emp.LastName as TransactionUserName
                                --,receivedBy.FirstName+' '+receivedBy.LastName as ReceivedBy
                                from TransactionExpense  th

                                where th.CompanyId =@CompanyId 
                                and th.CustomerId = @CustomerId
                                {4}";

            string subquery = "";
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                subquery = searchSql;
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, customerId, CompanyId, SearchText, empid, subquery);
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
        public DataTable GetAllTransactionsByCustomerIdAndCompanyIdAndFilter(int? customerId, Guid CompanyId, string SearchText, Guid empid, CustomerFilter filter)
        {

            string searchSql = @"
                                and (tr.CardTransactionId like @SearchText
	                                or tr.checkno like @SearchText
	                                or inv.InvoiceId like @SearchText
                                )";

            var strStartDate = "";
            var strEndDate = "";
            var dateRange = "";

            string sqlQuery = @"DECLARE @CustomerId uniqueidentifier
                                DECLARE @CompanyId uniqueidentifier
                                Declare @SearchText nvarchar(100)
                                declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)* @pagesize 
                                set @pageend = @pagesize
                                set @CustomerId = (select CustomerId from customer where id = {0} )
                                set @CompanyId = '{1}'
                                set @SearchText ='%{2}%'
 
                                select th.Id
                                , tr.TransacationDate
                                ,tr.Note
	                            , inv.Id InvoiceId
                                , inv.InvoiceId  InvoiceNo
                                , tr.[Type]
                                , tr.referenceNo as InvoiceIdStr
                                , tr.checkno
                                , tr.TransacationDate as InvoiceDueDate
                                , 0 as Balance
                                , th.Amout Amount
                                , tr.[Status]
                                , tr.CustomerId
                                , tr.CompanyId
                                , tr.PaymentMethod
                           
                                , inv.Description
                                , inv.Id as invId
                                , tr.CardTransactionId

                                , CASE WHEN th.ReceivedBy ='11111111-1111-1111-1111-111111111111' 
			                          THEN 'Customer'
                                  WHEN th.ReceivedBy ='22222222-2222-2222-2222-222222222222' 
			                          THEN 'System'
                                  ELSE receivedBy.FirstName+' '+receivedBy.LastName END AS ReceivedBy,
                                (select SUM(Amount) from [Transaction] where ReferenceNo = inv.InvoiceId and [Status] = 'Refund') as RefundAmount

                                INTO #CustomerFund
	                                from TransactionHistory  th
                                left join [Transaction] tr on th.TransactionId = tr.Id
                                left join Invoice inv on inv.Id = th.InvoiceId
                                left join Customer cus on cus.CustomerId = inv.CustomerId
                                --left join Employee emp on emp.UserId = '{3}'
                                left join Employee receivedBy on receivedBy.UserId = th.ReceivedBy
                                
                                    and receivedBy.UserId !='00000000-0000-0000-0000-000000000000'

                                where tr.CompanyId =@CompanyId 
                                and tr.CustomerId = @CustomerId
                                and th.Amout!=0
                                {4}{7}
                                order by tr.TransacationDate desc
                                select * into #CustomerFilterFund
								FROM #CustomerFund
								SELECT TOP (@pagesize)
                                  *
                                FROM #CustomerFilterFund
                                where Id NOT IN(Select TOP (@pagestart) Id from #CustomerFund {5})
                                {6}
                             
                                drop table #CustomerFilterFund
								drop table #CustomerFund";
            string subquery = "";
            string query = "";
            string query1 = "";

            #region Order
            if (!string.IsNullOrWhiteSpace(filter.order))
            {
                if (filter.order == "ascending/fundingdate")
                {
                    query = "order by #CustomerFilterFund.[TransacationDate] asc";
                    query1 = "order by [TransacationDate] asc";
                }
                else if (filter.order == "descending/fundingdate")
                {
                    query = "order by #CustomerFilterFund.[TransacationDate] desc";
                    query1 = "order by [TransacationDate] desc";
                }
                else if (filter.order == "ascending/receivedby")
                {
                    query = "order by #CustomerFilterFund.[ReceivedBy] asc";
                    query1 = "order by [ReceivedBy] asc";
                }
                else if (filter.order == "descending/receivedby")
                {
                    query = "order by #CustomerFilterFund.[ReceivedBy] desc";
                    query1 = "order by [ReceivedBy] desc";
                }
                else if (filter.order == "ascending/invoiceno")
                {
                    query = "order by #CustomerFilterFund.[InvoiceNo] asc";
                    query1 = "order by [InvoiceNo] asc";
                }
                else if (filter.order == "descending/invoiceno")
                {
                    query = "order by #CustomerFilterFund.[InvoiceNo] desc";
                    query1 = "order by [InvoiceNo] desc";
                }
                else if (filter.order == "ascending/description")
                {
                    query = "order by #CustomerFilterFund.[Description] asc";
                    query1 = "order by [Description] asc";
                }
                else if (filter.order == "descending/description")
                {
                    query = "order by #CustomerFilterFund.[Description] desc";
                    query1 = "order by [Description] desc";
                }
                else if (filter.order == "ascending/paymethod")
                {
                    query = "order by #CustomerFilterFund.[PaymentMethod] asc";
                    query1 = "order by [PaymentMethod] desc";
                }
                else if (filter.order == "descending/paymethod")
                {
                    query = "order by #CustomerFilterFund.[PaymentMethod] asc";
                    query1 = "order by [PaymentMethod] asc";
                }
                else if (filter.order == "ascending/checkno")
                {
                    query = "order by #CustomerFilterFund.[CheckNo] asc";
                    query1 = "order by [CheckNo] asc";
                }
                else if (filter.order == "descending/checkno")
                {
                    query = "order by #CustomerFilterFund.[CheckNo] desc";
                    query1 = "order by [CheckNo] desc";
                }
                else if (filter.order == "ascending/transactionid")
                {
                    query = "order by #CustomerFilterFund.[InvoiceIdStr] asc";
                    query1 = "order by [InvoiceIdStr] asc";
                }
                else if (filter.order == "descending/transactionid")
                {
                    query = "order by #CustomerFilterFund.[InvoiceIdStr] desc";
                    query1 = "order by [InvoiceIdStr] desc";
                }
                else if (filter.order == "ascending/amount")
                {
                    query = "order by #CustomerFilterFund.[Amount]  asc";
                    query1 = "order by Amount asc";
                }
                else if (filter.order == "descending/amount")
                {
                    query = "order by #CustomerFilterFund.[Amount]  desc";
                    query1 = "order by Amount desc";
                }
               else
                {
                    query = "order by #CustomerFilterFund.[TransacationDate] desc";
                    query1 = "order by TransacationDate desc";
                }

            }
            else
            {
                query = "order by #CustomerFilterFund.[TransacationDate] desc";
                query1 = "order by TransacationDate desc";
            }
            #endregion
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                subquery = searchSql;
            }
            if (filter.StrStartDate.ToString("yyyy-MM-dd HH:mm:ss") != "0001-01-01 00:00:00")
            {
                strStartDate = filter.StrStartDate.SetZeroHour().ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss");
                strEndDate = filter.StrEndDate.SetMaxHour().ClientToUTCTime().ToString("yyyy-MM-dd HH:mm:ss");
                dateRange = string.Format("and inv.CreatedDate between '{0}' and '{1}'", strStartDate, strEndDate);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, customerId, CompanyId, SearchText, empid, subquery,query,query1, dateRange);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", filter.PageNo));
                    AddParameter(cmd, pInt32("pagesize", filter.PageSize));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllTransactions1ByCustomerIdAndCompanyIdAndFilter(int? customerId, Guid CompanyId, string SearchText, Guid empid)
        {

            string searchSql = @"
                                and (tr.CardTransactionId like @SearchText
	                                or tr.checkno like @SearchText
	                                or inv.InvoiceId like @SearchText
                                )
                                ";

            string sqlQuery = @"DECLARE @CustomerId uniqueidentifier
                                DECLARE @CompanyId uniqueidentifier
                                Declare @SearchText nvarchar(100)

                                set @CustomerId = (select CustomerId from customer where id = {0} )
                                set @CompanyId = '{1}'
                                set @SearchText ='%{2}%'
 
                                select tr.Id, tr.TransacationDate
	                                ,inv.Id InvoiceId, inv.InvoiceId  InvoiceNo
                                , tr.[Type]
                                , tr.referenceNo as InvoiceIdStr
                                , tr.checkno
                                , tr.TransacationDate as InvoiceDueDate
                                , 0 as Balance
                                , th.Amout Amount
                                , tr.[Status]
                                , tr.CustomerId
                                , tr.CompanyId
                                , tr.PaymentMethod
                                ,inv.Description
                                ,tr.CardTransactionId
								--,emp.FirstName + ' ' + emp.LastName as TransactionUserName
                                ,receivedBy.FirstName+' '+receivedBy.LastName as ReceivedBy

	                                from TransactionHistory  th
                                left join [Transaction] tr on th.TransactionId = tr.Id
                                left join Invoice inv on inv.Id = th.InvoiceId
                                left join Customer cus on cus.CustomerId = inv.CustomerId
                                --left join Employee emp on emp.UserId = '{3}'
                                left join Employee receivedBy on receivedBy.UserId = th.ReceivedBy
                                    and receivedBy.UserId !='00000000-0000-0000-0000-000000000000'

                                where tr.CompanyId =@CompanyId 
                                and tr.CustomerId = @CustomerId
                                and th.Amout!=0
                                {4}
                                order by tr.TransacationDate desc";
            string subquery = "";
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                subquery = searchSql;
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, customerId, CompanyId, SearchText, empid, subquery);
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

        public DataSet GetAllTransactionsByCompanyIdAndFilters(Guid companyId, int PageSize, string searchText, string SearchBy, int pageNo, string colnum, string AscOrDescVal, string FromDate, string ToDate,string order,string InvoiceStatus)
        {

            string sqlQuery = @"DECLARE @CompanyId uniqueidentifier
                                DECLARE @pagestart int
                                DECLARE @pageend int
                                DECLARE @pageno int
                                DECLARE @pagesize int
                                --DECLARE @SearchText nvarchar(50)
                                DECLARE @SearchBy nvarchar(50)

                                --SET @SearchText = '%{0}%'
                                SET @SearchBy = '%{1}%'
                                SET @pageno = {2} --default 1
                                SET @pagesize = {3} --default 10
                                SET @CompanyId = '{4}'


                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize
 
                               select IDENTITY(int, 1,1) AS ID_Num , * into #TransactionData 
                                from (
 
                                select 0 as Id , inv.Id as InvoiceId,inv.CreatedDate as TransacationDate,'Invoice' as [Type],InvoiceId InvoiceIdStr,DueDate as InvoiceDueDate ,BalanceDue as Balance
                                ,TotalAmount as Amount,inv.[Status],inv.CustomerId , CompanyId 
                                ,cus.FirstName + ' '+cus.LastName as CustomerName
                                ,cus.AuthorizeRefId as AuthRefId,
                                cus.BusinessName as CustomerBussinessName
                                from invoice inv
                                left join Customer cus on cus.CustomerId = inv.CustomerId 

                                where IsBill =0 and IsEstimate =0 and inv.[Status] !='Init'
                                and CompanyId = @CompanyId 
                                {7} 
                                    union 


	                                select 0 as Id , inv.Id as InvoiceId,inv.CreatedDate as TransacationDate,'Estimate' as [Type],InvoiceId InvoiceIdStr,DueDate as InvoiceDueDate ,BalanceDue as Balance
	                                ,TotalAmount as Amount,inv.[Status],inv.CustomerId , CompanyId 
	                                ,cus.FirstName + ' '+cus.LastName as CustomerName
	                                ,cus.AuthorizeRefId as AuthRefId,
	                                cus.BusinessName as CustomerBussinessName
	                                from invoice inv
	                                left join Customer cus on cus.CustomerId = inv.CustomerId 

	                                where IsBill =0 and IsEstimate =1 and inv.[Status] !='Init'
	                                and CompanyId = @CompanyId 
                                    {7} 
                                    union 


                                    select tr.Id , 0 as InvoiceId , TransacationDate,tr.[Type], '-' as InvoiceIdStr , TransacationDate , 0
                                    ,tr.Amount ,tr.[Status],tr.CustomerId , tr.CompanyId 
	                                ,cus.FirstName + ' '+cus.LastName as CustomerName
                                    ,cus.AuthorizeRefId as AuthRefId,
                                    cus.BusinessName as CustomerBussinessName
	                                from [Transaction] tr
	                                left join customer cus on tr.CustomerId = cus.CustomerId

                                where {8} tr.CompanyId =@CompanyId) a
                                
 
                                    select * into #TransactionDataFiltered from(SELECT  * FROM #TransactionData
                                    where ((InvoiceIdStr like @SearchText  or [Type]   like @SearchText or CustomerName like @SearchText or AuthRefId like @SearchText)
		                                and ([Type] like @SearchBy))) a
	                                  
                                    select top(@pagesize) * into #Testtable from #TransactionDataFiltered
								    where ID_Num not in (Select TOP (@pagestart)  ID_Num from #TransactionDataFiltered {9})
                                    --select TOP (@pagesize) * from #TransactionDataFiltered   where   ID_Num NOT IN(Select TOP (@pagestart) ID_Num from #TransactionDataFiltered {9}) 
									{10}
                                    select *  from #Testtable
									select sum(Balance) as TotalBalanceByPage,sum(Amount) as TotalAmountByPage from #TestTable 
                 

	                                select 
										(select count(*) from #TransactionData
										where Type='Estimate') as TotalOpenEstimates,
										(select sum(Amount) from #TransactionData
										where Type='Estimate') as TotalOpenEstimatesAmount,
										(select sum(Amount) from #TransactionData
										where Status='Paid') as TotalRevenue,
										(select sum(Amount) from #TransactionData
										where Status='Open' 
										and CAST(InvoiceDueDate AS DATE) < CAST(GETDATE() AS DATE)
										and DATEPART(yyyy, InvoiceDueDate) = DATEPART(yyyy, getdate())
										) as AccountsReceivable,
		                                (select count(*)  from #TransactionData
	                                where ((InvoiceIdStr like @SearchText  or [Type]   like @SearchText or CustomerName like @SearchText or AuthRefId like @SearchText)
		                                and ([Type] like @SearchBy))) as [TotalCount],
										sum(Balance) as TotalBalance
		                                from  #TransactionData
                                        where [Type] = 'Invoice'

                                    DROP TABLE #TransactionData
                                    DROP TABLE #TransactionDataFiltered 
                                    DROP TABLE #Testtable";
            string subquery = "";
            string subquery1 = "";
            string orderquery = "";
            string orderquery1 = "";
            #region Order
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/customername")
                {
                    orderquery = "order by [CustomerName] asc";
                    orderquery1 = "order by [CustomerName] asc";
                }
                else if (order == "descending/customername")
                {
                    orderquery = "order by [CustomerName] desc";
                    orderquery1 = "order by [CustomerName] desc";
                }
                else if (order == "ascending/invoiceno")
                {
                    orderquery = "order by [InvoiceId] asc";
                    orderquery1 = "order by [InvoiceId] asc";
                }
                else if (order == "descending/invoiceno")
                {
                    orderquery = "order by [InvoiceId] desc";
                    orderquery1 = "order by [InvoiceId] desc";
                }
                else if (order == "ascending/duedate")
                {
                    orderquery = "order by [InvoiceDueDate] asc";
                    orderquery1 = "order by [InvoiceDueDate] asc";
                }
                else if (order == "descending/duedate")
                {
                    orderquery = "order by [InvoiceDueDate] desc";
                    orderquery1 = "order by [InvoiceDueDate] desc";
                }
                else if (order == "ascending/type")
                {
                    orderquery = "order by [Type] asc";
                    orderquery1 = "order by [Type] asc";
                }
                else if (order == "descending/type")
                {
                    orderquery = "order by [Type] desc";
                    orderquery1 = "order by [Type] desc";
                }
                else if (order == "ascending/balance")
                {
                    orderquery = "order by [Balance] asc";
                    orderquery1 = "order by [Balance] asc";
                }
                else if (order == "descending/balance")
                {
                    orderquery = "order by [Balance] desc";
                    orderquery1 = "order by [Balance] desc";
                }
                else if (order == "ascending/total")
                {
                    orderquery = "order by [Amount] asc";
                    orderquery1 = "order by [Amount] desc";
                }
                else if (order == "descending/total")
                {
                    orderquery = "order by #TransactionData.[Amount] asc";
                    orderquery1 = "order by [Amount] asc";
                }
                else if (order == "ascending/status")
                {
                    orderquery = "order by [Status] asc";
                    orderquery1 = "order by [Status] asc";
                }
                else if (order == "descending/status")
                {
                    orderquery = "order by [Status] desc";
                    orderquery1 = "order by [Status] desc";
                }
              

            }
            else
            {
                orderquery = "order by [ID_Num] desc";
                orderquery1 = "order by ID_Num desc";
            }
            #endregion
            if (!string.IsNullOrWhiteSpace(FromDate) && !string.IsNullOrWhiteSpace(ToDate) && FromDate != "01/01/0001" && ToDate != "01/01/0001")
            {
                DateTime FromDatetime = new DateTime();
                DateTime EndDatetime = new DateTime();
                DateTime.TryParse(FromDate, out FromDatetime);
                DateTime.TryParse(ToDate, out EndDatetime);
                FromDatetime = FromDatetime.SetZeroHour();
                EndDatetime = EndDatetime.SetMaxHour();
                if(FromDatetime != new DateTime() && EndDatetime != new DateTime())
                {
                    subquery = string.Format("and inv.CreatedDate between '{0}' and '{1}'", FromDatetime, EndDatetime);
                    subquery1 = string.Format("tr.TransacationDate between '{0}' and '{1}' and ", FromDatetime, EndDatetime);
                }
                
            }

            try
            {

                if (string.IsNullOrWhiteSpace(colnum))
                {
                    AscOrDescVal = "";
                }
                else
                {
                    colnum = "ORDER BY " + colnum;
                }


                sqlQuery = string.Format(sqlQuery
                    , searchText//0
                    , SearchBy
                    , pageNo
                    , PageSize
                    , companyId
                    , colnum
                    , AscOrDescVal
                    , subquery
                    , subquery1
                    ,orderquery//9
                    ,orderquery1//10
                    );
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))

                {
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", searchText)));

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult/*.Tables[0]*/;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable DownLoadAllTransactionsByCompanyIdAndFilters(Guid companyId, int PageSize, string searchText, string SearchBy, int pageNo, string colnum, string AscOrDescVal, string FromDate, string ToDate, string order)
        {

            string sqlQuery = @"DECLARE @CompanyId uniqueidentifier
                                DECLARE @pagestart int
                                DECLARE @pageend int
                                DECLARE @pageno int
                                DECLARE @pagesize int
                               -- DECLARE @SearchText nvarchar(50)
                                DECLARE @SearchBy nvarchar(50)

                                --SET @SearchText = '%{0}%'
                                SET @SearchBy = '%{1}%'
                                SET @pageno = {2} --default 1
                                SET @pagesize = {3} --default 10
                                SET @CompanyId = '{4}'


                                SET @pagestart=(@pageno-1)* @pagesize 
                                SET @pageend = @pagesize
 
                               select IDENTITY(int, 1,1) AS ID_Num , * into #TransactionData 
                                from (
 
                                select 0 as Id,cus.Id as [Customer ID] , inv.Id as InvoiceId,inv.CreatedDate as TransacationDate,'Invoice' as [Type],InvoiceId InvoiceIdStr,DueDate as InvoiceDueDate ,BalanceDue as Balance
                                ,TotalAmount as Amount,inv.[Status],inv.CustomerId , CompanyId 
                                ,cus.FirstName + ' '+cus.LastName as CustomerName
                                ,cus.AuthorizeRefId as AuthRefId,
                                cus.BusinessName as CustomerBussinessName
                                from invoice inv
                                left join Customer cus on cus.CustomerId = inv.CustomerId 

                                where IsBill =0 and IsEstimate =0 and inv.[Status] !='Init'
                                and CompanyId = @CompanyId 
                                {7} 
                                    union 


	                                select 0 as Id,cus.Id as [Customer ID] , inv.Id as InvoiceId,inv.CreatedDate as TransacationDate,'Estimate' as [Type],InvoiceId InvoiceIdStr,DueDate as InvoiceDueDate ,BalanceDue as Balance
	                                ,TotalAmount as Amount,inv.[Status],inv.CustomerId , CompanyId 
	                                ,cus.FirstName + ' '+cus.LastName as CustomerName
	                                ,cus.AuthorizeRefId as AuthRefId,
	                                cus.BusinessName as CustomerBussinessName
	                                from invoice inv
	                                left join Customer cus on cus.CustomerId = inv.CustomerId 

	                                where IsBill =0 and IsEstimate =1 and inv.[Status] !='Init'
	                                and CompanyId = @CompanyId 
                                    {7} 
                                    union 


                                    select cus.Id as [Customer ID],tr.Id , 0 as InvoiceId , TransacationDate,tr.[Type], '-' as InvoiceIdStr , TransacationDate , 0
                                    ,tr.Amount ,tr.[Status],tr.CustomerId , tr.CompanyId 
	                                ,cus.FirstName + ' '+cus.LastName as CustomerName
                                    ,cus.AuthorizeRefId as AuthRefId,
                                    cus.BusinessName as CustomerBussinessName
	                                from [Transaction] tr
	                                left join customer cus on tr.CustomerId = cus.CustomerId

                                where {8} tr.CompanyId =@CompanyId) a
                                
 
                                    select * into #TransactionDataFiltered from(SELECT  * FROM #TransactionData
                                    where ((InvoiceIdStr like @SearchText  or [Type]   like @SearchText or CustomerName like @SearchText or AuthRefId like @SearchText)
		                                and ([Type] like @SearchBy))) a
	                                  
                                    select CASE WHEN (CustomerBussinessName = '' or CustomerBussinessName is null) THEN  CustomerName WHEN (CustomerBussinessName !='' or CustomerBussinessName is not null) THEN  CustomerBussinessName end [Customer Name]
                                    ,Type as [Site Type],InvoiceIdStr as [Estimate No:/ Invoice No],
									format(InvoiceDueDate,'M/d/yy') as [Due Date],cast(Balance as decimal(10,2)) as Balance ,cast(Amount as decimal(10,2)) as Total,Status from #TransactionDataFiltered   where   ID_Num NOT IN(Select TOP (@pagestart) ID_Num from #TransactionDataFiltered {9}) 
									{10}

                                    DROP TABLE #TransactionData
                                    DROP TABLE #TransactionDataFiltered";
            string subquery = "";
            string subquery1 = "";
            string orderquery = "";
            string orderquery1 = "";
            #region Order
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending /customername")
                {
                    orderquery = "order by [CustomerName] asc";
                    orderquery1 = "order by [CustomerName] asc";
                }
                else if (order == "descending/customername")
                {
                    orderquery = "order by [CustomerName] desc";
                    orderquery1 = "order by [CustomerName] desc";
                }
                else if (order == "ascending/invoiceno")
                {
                    orderquery = "order by [InvoiceId] asc";
                    orderquery1 = "order by [InvoiceId] asc";
                }
                else if (order == "descending/invoiceno")
                {
                    orderquery = "order by [InvoiceId] desc";
                    orderquery1 = "order by [InvoiceId] desc";
                }
                else if (order == "ascending/duedate")
                {
                    orderquery = "order by [InvoiceDueDate] asc";
                    orderquery1 = "order by [InvoiceDueDate] asc";
                }
                else if (order == "descending/duedate")
                {
                    orderquery = "order by [InvoiceDueDate] desc";
                    orderquery1 = "order by [InvoiceDueDate] desc";
                }
                else if (order == "ascending/type")
                {
                    orderquery = "order by [Type] asc";
                    orderquery1 = "order by [Type] asc";
                }
                else if (order == "descending/type")
                {
                    orderquery = "order by [Type] desc";
                    orderquery1 = "order by [Type] desc";
                }
                else if (order == "ascending/balance")
                {
                    orderquery = "order by [Balance] asc";
                    orderquery1 = "order by [Balance] asc";
                }
                else if (order == "descending/balance")
                {
                    orderquery = "order by [Balance] desc";
                    orderquery1 = "order by [Balance] desc";
                }
                else if (order == "ascending/total")
                {
                    orderquery = "order by [Amount] asc";
                    orderquery1 = "order by [Amount] desc";
                }
                else if (order == "descending/total")
                {
                    orderquery = "order by #TransactionData.[Amount] asc";
                    orderquery1 = "order by [Amount] asc";
                }
                else if (order == "ascending/status")
                {
                    orderquery = "order by [Status] asc";
                    orderquery1 = "order by [Status] asc";
                }
                else if (order == "descending/status")
                {
                    orderquery = "order by [Status] desc";
                    orderquery1 = "order by [Status] desc";
                }


            }
            else
            {
                orderquery = "order by [ID_Num] desc";
                orderquery1 = "order by ID_Num desc";
            }
            #endregion
            if (!string.IsNullOrWhiteSpace(FromDate) && !string.IsNullOrWhiteSpace(ToDate) && FromDate != "undefined" && ToDate != "undefined" && FromDate != "01/01/0001" && ToDate != "01/01/0001")
            {
                DateTime FromDatetime = new DateTime();
                DateTime EndDatetime = new DateTime();
                DateTime.TryParse(FromDate, out FromDatetime);
                DateTime.TryParse(ToDate, out EndDatetime);
                FromDatetime = FromDatetime.SetZeroHour();
                EndDatetime = EndDatetime.SetMaxHour();
                if (FromDatetime != new DateTime() && EndDatetime != new DateTime())
                {
                    subquery = string.Format("and inv.CreatedDate between '{0}' and '{1}'", FromDatetime, EndDatetime);
                    subquery1 = string.Format("tr.TransacationDate between '{0}' and '{1}' and ", FromDatetime, EndDatetime);
                }

            }

            try
            {

                if (string.IsNullOrWhiteSpace(colnum))
                {
                    AscOrDescVal = "";
                }
                else
                {
                    colnum = "ORDER BY " + colnum;
                }


                sqlQuery = string.Format(sqlQuery
                    , searchText//0
                    , SearchBy//1
                    , pageNo//2
                    , PageSize//3
                    , companyId//4
                    , colnum//5
                    , AscOrDescVal//6
                    , subquery//7
                    , subquery1//8
                    , orderquery//9
                    , orderquery1//10
                    );
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", searchText)));

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataSet GetAllTransactionsByCompanyIdForReports(Guid companyId)
        {

            string sqlQuery = @"DECLARE @CompanyId uniqueidentifier
                                SET @CompanyId = '{0}'

                               select IDENTITY(int, 1,1) AS ID_Num , * into #TransactionData 
                                from (
 
                                select 0 as Id , inv.Id as InvoiceId,inv.CreatedDate as TransacationDate,'Invoice' as [Type],InvoiceId InvoiceIdStr,DueDate as InvoiceDueDate ,BalanceDue as Balance
                                ,TotalAmount as Amount,inv.[Status],inv.CustomerId , CompanyId 
                                ,cus.FirstName + ' '+cus.LastName as CustomerName
                                ,cus.AuthorizeRefId as AuthRefId,
                                cus.BusinessName as CustomerBussinessName
                                from invoice inv
                                left join Customer cus on cus.CustomerId = inv.CustomerId 

                                where IsBill =0 and IsEstimate =0 and inv.[Status] !='Init'
                                and CompanyId = @CompanyId 
                                    union 


	                                select 0 as Id , inv.Id as InvoiceId,inv.CreatedDate as TransacationDate,'Estimate' as [Type],InvoiceId InvoiceIdStr,DueDate as InvoiceDueDate ,BalanceDue as Balance
	                                ,TotalAmount as Amount,inv.[Status],inv.CustomerId , CompanyId 
	                                ,cus.FirstName + ' '+cus.LastName as CustomerName
	                                ,cus.AuthorizeRefId as AuthRefId,
	                                cus.BusinessName as CustomerBussinessName
	                                from invoice inv
	                                left join Customer cus on cus.CustomerId = inv.CustomerId 

	                                where IsBill =0 and IsEstimate =1 and inv.[Status] !='Init'
	                                and CompanyId = @CompanyId 
                                    union 


                                    select tr.Id , 0 as InvoiceId , TransacationDate,tr.[Type], '-' as InvoiceIdStr , TransacationDate , 0
                                    ,tr.Amount ,tr.[Status],tr.CustomerId , tr.CompanyId 
	                                ,cus.FirstName + ' '+cus.LastName as CustomerName
                                    ,cus.AuthorizeRefId as AuthRefId,
                                    cus.BusinessName as CustomerBussinessName
	                                from [Transaction] tr
	                                left join customer cus on tr.CustomerId = cus.CustomerId

                                where tr.CompanyId =@CompanyId) a
                                
 
                                    SELECT * FROM #TransactionData
                                   
                                    DROP TABLE #TransactionData";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId);
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

        public DataTable GetAllTransactionsByCompanyId(Guid CompanyId)
        {
            string sqlQuery = @" 
                                DECLARE @CompanyId uniqueidentifier
                                set @CompanyId = '{0}'
 
                                select 0 as Id , Id as InvoiceId,CreatedDate as TransacationDate,'Invoice' as [Type],InvoiceId InvoiceIdStr,DueDate as InvoiceDueDate ,BalanceDue as Balance
                                ,TotalAmount as Amount,[Status],CustomerId , CompanyId from invoice 

                                where IsBill =0 and IsEstimate =0 and [Status] !='Init'
                                and CompanyId = @CompanyId
                                and BalanceDue>0
                                    ";
            try
            {
                sqlQuery = string.Format(sqlQuery, CompanyId);
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

        public DataTable GetReceivePaymentListByCustomerId(int customerId, Guid CompanyId, int? TransactionId)
        {
            string sqlQuery = @"
                                DECLARE @CustomerId uniqueidentifier
                                set @CustomerId = (select CustomerId from customer where id = {0} )
                                select
                                    inv.Id
                                ,inv.InvoiceId as [Description]
                                , inv.CreatedDate  
                                , inv.DueDate
                                , inv.TotalAmount [OriginalAmount]
                                , inv.BalanceDue [OpenBalance]
                                ,0 as Payment
                                --, (select sum (Amout) from TransactionHistory where InvoiceId = inv.Id and TransactionId = tr.Id) as Payment
                                from Invoice inv
                                --left join [Transaction] tr on tr.InvoiceId = inv.id
                                --and tr.CompanyId  = inv.CompanyId
                                --left join TransactionHistory th on th.InvoiceId = inv.Id
                                --and th.TransactionId = tr.Id

                                where inv.CustomerId =@CustomerId 
                                and inv.CompanyId ='{1}'
                                and inv.IsEstimate = 0
                                and inv.BalanceDue > 0 
                                and (inv.[Status] != 'Declined' and inv.[Status] != 'Init' and inv.[Status] != 'Cancelled'  and inv.[Status] != 'Rolled Over' )
                                order by inv.id desc
                                ";

            string TransactionQuery = @"DECLARE @CustomerId uniqueidentifier
                                        DECLARE @CompanyId uniqueidentifier
                                        DECLARE @TransactionId int
                                        set @CustomerId = (select CustomerId from customer where id = {0} )
                                        set @CompanyId ='{1}'
                                        set @TransactionId = {2}
                                        select
                                            inv.Id
                                        ,inv.InvoiceId as [Description]
                                        , inv.CreatedDate  
                                        , inv.DueDate
                                        , inv.TotalAmount [OriginalAmount]
                                        , th.Balance [OpenBalance]
                                        , th.Amout as Payment
                                        , (select sum (Amout) from TransactionHistory where InvoiceId = inv.Id and TransactionId = @TransactionId) as Payment
                                        from Invoice inv
                                        left join [TransactionHistory] th on th.InvoiceId = inv.Id and th.TransactionId =@TransactionId
  
                                        where inv.CustomerId =@CustomerId 
                                        and inv.CompanyId =@CompanyId
                                        and inv.IsEstimate = 0
                                        --and inv.BalanceDue > 0
                                        and  inv.id in (select th.InvoiceId 
		                                        from [Transaction] tr 
		                                        left join TransactionHistory th 
		                                        on th.TransactionId = tr.Id
		                                        where tr.id in(@TransactionId))";

            if (TransactionId.HasValue)
            {
                sqlQuery = string.Format(TransactionQuery, customerId, CompanyId, TransactionId.Value);
            }
            else
            {
                sqlQuery = string.Format(sqlQuery, customerId, CompanyId);
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
        
        public DataTable GetTransactionQueueCustomerId(Guid CustomerId, string StartTime, string EndTime,double amount)
        {
            string sqlQuery = @" 
                                Select * from TransactionQueue where  (CreatedDate between '{1}' and '{2}') and CustomerId = '{0}' and cast(amount as decimal(10,2)) = {3}";

           
            try
            {
                sqlQuery = string.Format(sqlQuery, CustomerId, StartTime, EndTime, Math.Round(amount, 2) );
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
        public DataTable GetAllTransactionsReportByCompanyId(Guid CompanyId, DateTime? Start, DateTime? End)
        {
            string sqlQuery = @" 
                                Declare @CompanyId uniqueidentifier
                                set @CompanyId ='{0}'
 
                                select
                                inv.InvoiceId as [InvoiceNumber]
                                ,cus.Title +' '+cus.FirstName +' '+cus.LastName as [CustomerName]
                                ,th.Amout as [Amount Paid]
                                ,th.Balance as [Previous Balance]
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
                sqlQuery += "and tr.TransacationDate between '{1}' and '{2}'";
                sqlQuery = string.Format(sqlQuery, CompanyId, Start.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), End.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
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
        public DataTable GetFundingReport(int[] IdList, string[] Colmuns, Guid CompanyId)
        {
            DataTable dt = new DataTable();
            string InIdListFilter = "";
            if (IdList != null && IdList.Length > 0)
            {
                string Ids = "";
                foreach (int id in IdList)
                {
                    Ids += id + ",";
                }
                Ids += "0";
                InIdListFilter = "And th.Id in(" + Ids + ")";
            }
            string ColumnList = "";
            if (Colmuns != null && Colmuns.Length > 0)
            {
                foreach (string column in Colmuns)
                {
                    if (column == "FundingDate")
                    {
                        ColumnList += "convert(date,tr.TransacationDate) as TransactionDate";
                    }
                    else if (column == "ReceivedBy")
                    {
                        ColumnList += ",CASE WHEN th.ReceivedBy ='11111111-1111-1111-1111-111111111111' THEN 'Customer'WHEN th.ReceivedBy = '22222222-2222-2222-2222-222222222222'THEN 'System' ELSE receivedBy.FirstName + ' ' + receivedBy.LastName END AS ReceivedBy";
                    }
                    else if (column == "InvoiceNo")
                    {
                        ColumnList += ",inv.InvoiceId";
                    }
                    else if (column == "Description")
                    {
                        ColumnList += ",inv.Description";
                    }
                    else if (column == "PMTMethod")
                    {
                        ColumnList += ",tr.PaymentMethod";
                    }
                    else if (column == "CheckNo")
                    {
                        ColumnList += ",tr.checkno";
                    }
                    else if (column == "TransactionId")
                    {
                        ColumnList += ",tr.CardTransactionId";
                    }
                    else if (column == "Amount")
                    {
                        ColumnList += ",th.Amout Amount";
                    }
                  

                }
            }
            string sqlQuery = @"
                            select {0} 
                            from TransactionHistory  th
                            left join [Transaction] tr on th.TransactionId = tr.Id
                            left join Invoice inv on inv.Id = th.InvoiceId
                            left join Customer cus on cus.CustomerId = inv.CustomerId
                     
                            left join Employee receivedBy on receivedBy.UserId = th.ReceivedBy
                            and receivedBy.UserId !='00000000-0000-0000-0000-000000000000'
                            where inv.CompanyId = '{1}'
                            {2}
	
                            ";
            try
            {
                sqlQuery = string.Format(sqlQuery, ColumnList, CompanyId, InIdListFilter);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception)
            {
                return null;
            }
            //return dt;
        }

        public DataTable GetExpenseReport(int[] IdList, string[] Colmuns, Guid CompanyId)
        {
            DataTable dt = new DataTable();
            string InIdListFilter = "";
            if (IdList != null && IdList.Length > 0)
            {
                string Ids = "";
                foreach (int id in IdList)
                {
                    Ids += id + ",";
                }
                Ids += "0";
                InIdListFilter = "And th.Id in(" + Ids + ")";
            }
            string ColumnList = "";
            if (Colmuns != null && Colmuns.Length > 0)
            {
                foreach (string column in Colmuns)
                {
                    if (column == "ExpenseDate")
                    {
                        ColumnList += "th.ExpenseDate";
                    }
                    else if (column == "Description")
                    {
                        ColumnList += " ,th.Description";
                    }
                    else if (column == "PMTMethod")
                    {
                        ColumnList += ", th.PaymentMethod";
                    }
                    else if (column == "CheckNo")
                    {
                        ColumnList += ",th.checkno";
                    }
                    else if (column == "PMTMethod")
                    {
                        ColumnList += ",th.PaymentMethod";
                    }
             
                    else if (column == "Amount")
                    {
                        ColumnList += ",th.Amount ";
                    }


                }
            }
            string sqlQuery = @"
                            select {0} 
                            from TransactionExpense  th
                            where th.CompanyId = '{1}'
                            {2}
	
                            ";
            try
            {
                sqlQuery = string.Format(sqlQuery, ColumnList, CompanyId, InIdListFilter);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception)
            {
                return null;
            }
            //return dt;
        }
        public DataTable GetAllFilterTransactionsByCompanyId(Guid comid, string Fdate, string Tdate)
        {
            string sqlQuery = @"select 0 as Id, inv.Id as InvoiceId,CreatedDate as TransacationDate,'Invoice' as [Type],InvoiceId InvoiceIdStr,
                                DueDate as InvoiceDueDate ,BalanceDue as Balance
                                ,TotalAmount as Amount,inv.[Status],inv.CustomerId , CompanyId 
                                ,cus.FirstName + ' '+cus.LastName as CustomerName
                                ,cus.AuthorizeRefId as AuthRefId,
                                cus.BusinessName as CustomerBussinessName
                                from Invoice inv
                                left join Customer cus
                                on cus.CustomerId = inv.CustomerId
                                where IsBill =0 and IsEstimate =0 and inv.[Status] !='Init'
                                and CompanyId = '{0}'
                                and inv.CreatedDate between '{1}' and '{2}'
                                union
                                select tr.Id , 0 as InvoiceId , TransacationDate,tr.[Type], '-' as InvoiceIdStr , TransacationDate , 0
                                ,tr.Amount ,tr.[Status],tr.CustomerId , tr.CompanyId 
                                ,cus.FirstName + ' '+cus.LastName as CustomerName
                                ,cus.AuthorizeRefId as AuthRefId,
                                cus.BusinessName as CustomerBussinessName
                                from [Transaction] tr
                                left join Customer cus
                                on cus.CustomerId = tr.CustomerId
                                where tr.CompanyId = '{0}'
                                and tr.TransacationDate between '{1}' and '{2}'
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, comid, Fdate, Tdate);
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
        public bool DeleteTransactionAndHistoryByTranId(int id)
        {
            string sqlQuery = @"
                                delete from [Transaction] where id ={0}

                                delete from  TransactionHistory where TransactionId = {0}
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, id);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    ExecuteCommand(cmd);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteTransactionAndHistoryByCardTranId(string TransactionId, Guid CustomerId)
        {
            string sqlQuery = @"
                                Declare @TransactionId int 
                                set @TransactionId = (select id from [Transaction] 
                                    where CardTransactionId = '{0}' 
                                        and CustomerId = '{1}')

                                delete from [Transaction] where id = @TransactionId
                                delete from TransactionHistory where TransactionId = @TransactionId
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, TransactionId, CustomerId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    ExecuteCommand(cmd);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #region Recurring Billing Mismatch
        public DataSet GetUnResolveRecurringBillingMismatchList(DateTime? Start, DateTime? End, int pageno, int pagesize, string searchtext)
        {
            string DateQuery = "";
            string SearchText = "";
            if (Start != new DateTime() && End != new DateTime())
            {
                string StartDateQuery = Start.Value.ToString("yyyy-MM-dd 00:00:00.000");
                string EndDateQuery = End.Value.ToString("yyyy-MM-dd 23:59:59.999");

                DateQuery = string.Format("and MM.CreatedDate between '{0}' and '{1}'", StartDateQuery, EndDateQuery);
            }
            //if (!string.IsNullOrWhiteSpace(searchtext))
            //{
            //    SearchText = string.Format("and (CONVERT(nvarchar(MAX), MM.TransactionId) = '{0}' or CONVERT(nvarchar(MAX), MM.InvoiceId) = '{0}' or  cus.Title +' '+ cus.FirstName +' '+ cus.LastName like '%{0}%')", searchtext);
            //}
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                SearchText = string.Format("and (CONVERT(nvarchar(MAX), MM.TransactionId) = @SearchText or CONVERT(nvarchar(MAX), MM.InvoiceId) = @SearchText or  cus.Title +' '+ cus.FirstName +' '+ cus.LastName like '%@SearchText%')", searchtext);
            }
            string sqlQuery = @"DECLARE @Date nvarchar(50)
	                                DECLARE @pagestart int
	                                DECLARE @pageend int
	                                DECLARE @pageno int
	                                DECLARE @pagesize int

                                    SET @pageno = {0} --default 1
									SET @pagesize = {1} --default 10
                                    SET @pagestart=(@pageno-1)* @pagesize 
                                    SET @pageend = @pagesize

                                select 
											MM.Id
                                            ,cus.Id as CustomerId
											,cus.Title +' '+ cus.FirstName +' '+ cus.LastName as CustomerName
											,MM.TransactionId
											,MM.InvoiceId
											,MM.BillingAmount
											,MM.ChargedAmountByGateway
			                                             
								
                                into #TestData

                                from RMRBillingMismatch MM
									left join Customer cus on cus.CustomerId = MM.CustomerId
									left join Employee emp on emp.UserId = MM.ResolvedBy
									where MM.IsResolved = 0
                                {2}
                                {3}
								SELECT TOP (@pagesize) #TD.* into #Testtable
	                                                FROM #TestData #TD
	                                                where Id NOT IN(Select TOP (@pagestart) Id from #TestData #_PD Order by CustomerName asc)
	                                                Order by CustomerName asc
                                   select *  from #Testtable
				                  select sum(BillingAmount) as TotalAmountByPage from #TestTable 
	                                                select count(Id) as [TotalCount] from #TestData
	                                                DROP TABLE #TestData
                                                    DROP TABLE #TestTable
                                                  ";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        pageno,
                                        pagesize,
                                        DateQuery,
                                        SearchText
                                        );
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("{0}", searchtext)));

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable DownloadUnResolveRecurringBillingMismatchList(DateTime? Start, DateTime? End, string searchtext)
        {
            string DateQuery = "";
            string SearchText = "";
            if (Start != new DateTime() && End != new DateTime())
            {
                string StartDateQuery = Start.Value.ToString("yyyy-MM-dd 00:00:00.000");
                string EndDateQuery = End.Value.ToString("yyyy-MM-dd 23:59:59.999");

                DateQuery = string.Format("and MM.CreatedDate between '{0}' and '{1}'", StartDateQuery, EndDateQuery);
            }
            //if (!string.IsNullOrWhiteSpace(searchtext))
            //{
            //    SearchText = string.Format("and (CONVERT(nvarchar(MAX), MM.TransactionId) = '{0}' or CONVERT(nvarchar(MAX), MM.InvoiceId) = '{0}' or  cus.Title +' '+ cus.FirstName +' '+ cus.LastName like '%{0}%')", searchtext);
            //}
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                SearchText = string.Format("and (CONVERT(nvarchar(MAX), MM.TransactionId) = @SearchText or CONVERT(nvarchar(MAX), MM.InvoiceId) = @SearchText or  cus.Title +' '+ cus.FirstName +' '+ cus.LastName like '%@SearchText%')", searchtext);
            }
            string sqlQuery = @"select 
											cus.Title +' '+ cus.FirstName +' '+ cus.LastName as [Customer Name]
											,MM.TransactionId as [Transaction Id]
											,MM.InvoiceId as [Invoice Id]
											,MM.BillingAmount as [Billing Amount]
											,MM.ChargedAmountByGateway as [Charged Amount By Gateway]

                                from RMRBillingMismatch MM
									left join Customer cus on cus.CustomerId = MM.CustomerId
									left join Employee emp on emp.UserId = MM.ResolvedBy
									where MM.IsResolved = 0

									Order by [Customer Name] asc";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        DateQuery,
                                        SearchText
                                        );
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("{0}", searchtext)));

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetResolveRecurringBillingMismatchList(DateTime? Start, DateTime? End, int pageno, int pagesize, string searchtext)
        {
            string DateQuery = "";
            string SearchText = "";
            if (Start != new DateTime() && End != new DateTime())
            {
                string StartDateQuery = Start.Value.ToString("yyyy-MM-dd 00:00:00.000");
                string EndDateQuery = End.Value.ToString("yyyy-MM-dd 23:59:59.999");

                DateQuery = string.Format("and MM.ResolvedDate between '{0}' and '{1}'", StartDateQuery, EndDateQuery);
            }
      
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                SearchText = string.Format("and (CONVERT(nvarchar(MAX), MM.TransactionId) = @SearchText or CONVERT(nvarchar(MAX), MM.InvoiceId) = @SearchText or  cus.Title +' '+ cus.FirstName +' '+ cus.LastName like '%@SearchText%')", searchtext);
            }

            string sqlQuery = @"DECLARE @Date nvarchar(50)
	                                DECLARE @pagestart int
	                                DECLARE @pageend int
	                                DECLARE @pageno int
	                                DECLARE @pagesize int

                                    SET @pageno = {0} --default 1
									SET @pagesize = {1} --default 10
                                    SET @pagestart=(@pageno-1)* @pagesize 
                                    SET @pageend = @pagesize

                                select 
											MM.Id
                                            ,cus.Id as CustomerId
											,cus.Title +' '+ cus.FirstName +' '+ cus.LastName as CustomerName
											,MM.TransactionId
											,MM.InvoiceId
											,MM.BillingAmount
											,MM.ChargedAmountByGateway
											,emp.Title +' '+ emp.FirstName +' '+ emp.LastName as ResolvedBy
											,MM.ResolvedDate
			                                             
								
                                into #TestData

                                from RMRBillingMismatch MM
									left join Customer cus on cus.CustomerId = MM.CustomerId
									left join Employee emp on emp.UserId = MM.ResolvedBy
									where MM.IsResolved = 1
                                {2}
                                {3}
								SELECT TOP (@pagesize) #TD.* into #TestTable
	                                                FROM #TestData #TD
	                                                where Id NOT IN(Select TOP (@pagestart) Id from #TestData #_PD Order by CustomerName asc)
	                                                Order by CustomerName asc
                                  select *  from #Testtable
				                  select sum(BillingAmount) as TotalAmountByPage from #TestTable 
	                                                select count(Id) as [TotalCount] from #TestData
	                                                DROP TABLE #TestData
                                                    DROP TABLE #TestTable
";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        pageno,
                                        pagesize,
                                        DateQuery,
                                        SearchText
                                        );
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("{0}", searchtext)));

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable DownloadResolveRecurringBillingMismatchList(DateTime? Start, DateTime? End, string searchtext)
        {
            string DateQuery = "";
            string SearchText = "";
            if (Start != new DateTime() && End != new DateTime())
            {
                string StartDateQuery = Start.Value.ToString("yyyy-MM-dd 00:00:00.000");
                string EndDateQuery = End.Value.ToString("yyyy-MM-dd 23:59:59.999");

                DateQuery = string.Format("and MM.ResolvedDate between '{0}' and '{1}'", StartDateQuery, EndDateQuery);
            }
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                SearchText = string.Format("and (CONVERT(nvarchar(MAX), MM.TransactionId) = @SearchText or CONVERT(nvarchar(MAX), MM.InvoiceId) = @SearchText or  cus.Title +' '+ cus.FirstName +' '+ cus.LastName like '%@SearchText%')", searchtext);
            }
            string sqlQuery = @"select 
											cus.Title +' '+ cus.FirstName +' '+ cus.LastName as [Customer Name]
											,MM.TransactionId as [Transaction Id]
											,MM.InvoiceId as [Invoice Id]
											,MM.BillingAmount as [Billing Amount]
											,MM.ChargedAmountByGateway as [Charged Amount By Gateway]
                                            ,emp.Title +' '+ emp.FirstName +' '+ emp.LastName as [Resolved By]
											,FORMAT(MM.ResolvedDate,'MM/dd/yyyy hh:mm tt') AS [Resolved Date]

                                from RMRBillingMismatch MM
									left join Customer cus on cus.CustomerId = MM.CustomerId
									left join Employee emp on emp.UserId = MM.ResolvedBy
									where MM.IsResolved = 1

									Order by [Customer Name] asc";

            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        DateQuery,
                                        SearchText
                                        );
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("{0}", searchtext)));

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        public Transaction GetTransactionDataByCustomerIdAndInvoiceId(Guid CusId, string InvId)
        {
            string sqlQuery = @"
                                select top (1) t.* from [Transaction] t
                                Left Join TransactionHistory th on th.TransactionId = t.Id
                                Left Join Invoice inv on inv.Id = th.InvoiceId
                                where t.CustomerId='{0}'
                                and inv.InvoiceId = '{1}' and t.[Status] = 'Closed' order by t.Id desc
                                ";
            Transaction _Transaction = new Transaction();
            try
            {
                sqlQuery = string.Format(sqlQuery, CusId, InvId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    SqlDataReader reader;
                    long rows = SelectRecords(cmd, out reader);
                    using (reader)
                    {
                        if (reader.Read())
                        {
                            FillObject(_Transaction, reader);
                            _Transaction.Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0;
                            _Transaction.CustomerId = (Guid)reader["CustomerId"];
                            _Transaction.CompanyId = (Guid)reader["CompanyId"];
                            _Transaction.Amount = reader["Amount"] != DBNull.Value ? Convert.ToDouble(reader["Amount"]) : 0;
                            _Transaction.Status = reader["Status"].ToString();
                            _Transaction.Type = reader["Type"].ToString();
                            _Transaction.TransacationDate = reader["TransacationDate"] != DBNull.Value ? Convert.ToDateTime(reader["TransacationDate"]) : new DateTime();
                            _Transaction.PaymentMethod = reader["PaymentMethod"].ToString();
                            _Transaction.CardTransactionId = reader["CardTransactionId"].ToString();
                            _Transaction.CheckNo = reader["CheckNo"].ToString();
                            _Transaction.ReferenceNo = reader["ReferenceNo"].ToString();
                            _Transaction.AddedBy = reader["AddedBy"].ToString();
                            _Transaction.Note = reader["Note"].ToString();
                            _Transaction.AddedDate = reader["AddedDate"] != DBNull.Value ? Convert.ToDateTime(reader["AddedDate"]) : new DateTime();
                            _Transaction.PaymentInfoId = reader["PaymentInfoId"] != DBNull.Value ? Convert.ToInt32(reader["PaymentInfoId"]) : 0;
                            _Transaction.CreatedBy = (Guid)reader["CreatedBy"];
                        }
                    }
                }
                return _Transaction;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetLatestPaymentDateByInvoiceId(int Id)
        {
            string sqlQuery = @" 
                                select top(1) tr.TransacationDate from [Transaction] tr join TransactionHistory trh on trh.TransactionId = tr.Id where trh.InvoiceId = {0} order by tr.TransacationDate desc";
            try
            {
                sqlQuery = string.Format(sqlQuery, Id);
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
