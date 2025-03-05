using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.DataAccess
{
    public partial class DeclinedTransactionsDataAccess
    {
        public DeclinedTransactionsDataAccess(string ConStr) : base(ConStr) { }

        public List<string> GetExistingTransactionsByTransactionIdList(string transactionIds)
        {
            string sqlQuery = @"select TransactionId from DeclinedTransactions where TransactionId in({0})";
            try
            {
                sqlQuery = string.Format(sqlQuery, transactionIds);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    SqlDataReader reader;
                    long result = SelectRecords(cmd, out reader);
                    List<string> list = new List<string>();
                    using (reader)
                    {
                        // Read rows until end of result or number of rows specified is reached
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                                list.Add(reader.GetString(0));
                        } 
                        reader.Close();
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet GetAllDeclinedTransactionsByFilter(AllReturnsFilter filter)
        {
         
            string subquery = "";
            string subquery1 = "";
            string DateFilter = "";
            string PaymentTypeFilter = "";
            if(filter.StartDate != new DateTime() && filter.EndDate != new DateTime())
            {
                var StartDate = filter.StartDate.UTCToClientTime().SetZeroHour();
                var EndDate = filter.EndDate.UTCToClientTime().SetMaxHour();
                DateFilter = string.Format("and dt.ReturnedDate  between '{0}' and '{1}'", StartDate, EndDate);
            }
            if(!string.IsNullOrWhiteSpace(filter.PaymentType) && filter.PaymentType != "-1")
            {
                if(filter.PaymentType == "ACH")
                {
                    PaymentTypeFilter = @"and (inv.PaymentType = 'ACH' OR inv.PaymentType like 'ACH_%')";
                }
                else if (filter.PaymentType == "CC")
                {
                    PaymentTypeFilter = @"and (inv.PaymentType = 'Credit Card' OR inv.PaymentType = 'CC' OR inv.PaymentType like 'CC_%')";
                }
                else if (filter.PaymentType == "Recurring")
                {
                    PaymentTypeFilter = "and inv.IsARBInvoice = 1";
                }
                else if (filter.PaymentType == "OneTime")
                {
                    PaymentTypeFilter = "and inv.IsARBInvoice = 0";
                } 
            }
            #region Order
            if (!string.IsNullOrWhiteSpace(filter.Order))
            {
                if (filter.Order == "ascending/transactionid")
                {
                    subquery = "order by #DeclinedTransactions.[TransactionId] asc";
                    subquery1 = "order by [TransactionId] asc";
                }
                else if (filter.Order == "descending/transactionid")
                {
                    subquery = "order by #DeclinedTransactions.[TransactionId] desc";
                    subquery1 = "order by [TransactionId] desc";
                }
                else if (filter.Order == "ascending/invoiceno")
                {
                    subquery = "order by #DeclinedTransactions.[InvoiceId] asc";
                    subquery1 = "order by [InvoiceId] asc";
                }
                else if (filter.Order == "descending/invoiceno")
                {
                    subquery = "order by #DeclinedTransactions.[InvoiceId] desc";
                    subquery1 = "order by [InvoiceId] desc";
                }
                else if (filter.Order == "ascending/customername")
                {
                    subquery = "order by #DeclinedTransactions.[CustomerName] asc";
                    subquery1 = "order by [CustomerName] asc";
                }
                else if (filter.Order == "descending/customername")
                {
                    subquery = "order by #DeclinedTransactions.[CustomerName] desc";
                    subquery1 = "order by [CustomerName] desc";
                }
                else if (filter.Order == "ascending/returnamount")
                {
                    subquery = "order by #DeclinedTransactions.[ReturnAmount] asc";
                    subquery1 = "order by [ReturnAmount] asc";
                }
                else if (filter.Order == "descending/returnamount")
                {
                    subquery = "order by #DeclinedTransactions.[ReturnAmount] desc";
                    subquery1 = "order by [ReturnAmount] desc";
                }
                else if (filter.Order == "ascending/date")
                {
                    subquery = "order by #DeclinedTransactions.[ReturnedDate] asc";
                    subquery1 = "order by [ReturnedDate] desc";
                }
                else if (filter.Order == "descending/date")
                {
                    subquery = "order by #DeclinedTransactions.[ReturnedDate] asc";
                    subquery1 = "order by [ReturnedDate] asc";
                }
                else if (filter.Order == "ascending/reason")
                {
                    subquery = "order by #DeclinedTransactions.[Reason] asc";
                    subquery1 = "order by [Reason] asc";
                }
                else if (filter.Order == "descending/reason")
                {
                    subquery = "order by #DeclinedTransactions.[Reason] desc";
                    subquery1 = "order by [Reason] desc";
                }
                else if (filter.Order == "ascending/comments")
                {
                    subquery = "order by #DeclinedTransactions.[Comment] asc";
                    subquery1 = "order by [Comment] asc";
                }
                else if (filter.Order == "descending/comments")
                {
                    subquery = "order by #DeclinedTransactions.[Comment] desc";
                    subquery1 = "order by [Comment] desc";
                }
               
            }
            else
            {
                subquery = "order by #DeclinedTransactions.[Id] desc";
                subquery1 = "order by Id desc";
            }
            #endregion
            string sqlQuery = @"declare @pagestart int
                                declare @pageend int
                                declare @pageno int
                                declare @pagesize int
                                declare @CompanyId uniqueidentifier
                                --declare @SearchText nvarchar(200)

                                set @CompanyId ='{0}'
                                --set @SearchText = '%{1}%'

                                set @pageno ={2}
                                set @pagesize = {3}

                                set @pagestart=(@pageno-1)* @pagesize 
                                set @pageend = @pagesize

                                select * into #DeclinedTransactions  from (
                                select dt.*
                                ,ISNULL(cus.FirstName+' '+cus.LastName,'') as CustomerName
                                ,cus.Id as CustomerIdValue
                                ,ISNULL(cus.BusinessName,'') as CustomerBusinessName,
                                 inv.Id as InvId
                                from DeclinedTransactions dt
                                left join Customer cus on dt.CustomerId = cus.CustomerId
                                left join Invoice inv on dt.InvoiceId = inv.InvoiceId
                                where dt.CompanyId=@CompanyId 
                                and (dt.TransactionId like @SearchText 
                                or dt.Reason like @SearchText
                                or dt.InvoiceId like @SearchText
                                or dt.Comment like @SearchText
                                or cus.FirstName+' '+cus.LastName like @SearchText
                                or cus.BusinessName like @SearchText) 
                                {6}
                                {7}
                                )a

                                SELECT TOP (@pagesize) * into #Testtable FROM #DeclinedTransactions
                                where   Id NOT IN(Select TOP (@pagestart) Id from #DeclinedTransactions {4})
                                {5}

                             
                                select *  from #Testtable
				                select sum(ReturnAmount) as TotalAmountByPage from #TestTable 

                                select count(id) as TotalCount from #DeclinedTransactions
                                drop table #DeclinedTransactions
                                drop table #Testtable
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery
                    ,filter.CompanyId.Value
                    ,filter.SearchText
                    ,filter.PageNo
                    ,filter.PageSize
                    ,subquery
                    ,subquery1
                    ,DateFilter
                    ,PaymentTypeFilter);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", filter.SearchText)));

                    return GetDataSet(cmd);
                    //SqlDataReader reader;
                    //long result = SelectRecords(cmd, out reader);
                    //List<DeclinedTransactions> list = new DeclinedTransactionsList();
                    //using (reader)
                    //{
                    //    while (reader.Read())
                    //    {
                    //        DeclinedTransactions DeclinedTransactionsObject = new DeclinedTransactions();
                    //        FillObject(DeclinedTransactionsObject, reader);
                    //        DeclinedTransactionsObject.CustomerBusinessName = reader["CustomerBusinessName"].ToString();
                    //        DeclinedTransactionsObject.CustomerName = reader["CustomerName"].ToString();
                    //        list.Add(DeclinedTransactionsObject);
                    //    }
                    //    reader.Close();
                    //}
                    
                    //return list;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetAllCustomerByFilterDownload(Guid companyId, DateTime Start, DateTime End, string status, string paymentmethod,string SearchText)
        {

            string subquery = "";
            string subquery1 = "";
            string searchTextQuery = "";
            string StatusQuery = "";
            string DateQuery = "";
            string DateQuery2 = "";
            string paymentmethodquery = "";
            if (Start != new DateTime() && End != new DateTime())
            {
                var StartDate = Start.UTCToClientTime().SetZeroHour();
                var EndDate = End.UTCToClientTime().SetMaxHour();
                DateQuery = string.Format("and cu.CreatedDate between '{0}' and '{1}'", StartDate, EndDate);
                DateQuery2 = string.Format("and CreatedDate between '{0}' and '{1}'", StartDate, EndDate);
            }
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                searchTextQuery = "and ((cu.SearchText like @SearchText) OR (cu.DBA like @SearchText))";
            }
            if (!string.IsNullOrWhiteSpace(status))
            {
                if (status == "suspended" || status == "active")
                {
                    StatusQuery = string.Format(" and SubscriptionStatus = '{0}'", status);
                }
                else
                {
                    StatusQuery = " and (SubscriptionStatus != 'suspended' and SubscriptionStatus != 'active')";
                }

            }
            if (!string.IsNullOrWhiteSpace(paymentmethod))
            {
                if (paymentmethod == "ACH")
                {
                    paymentmethodquery = string.Format(" and PaymentMethod = '{0}'", paymentmethod);
                }
                else if (paymentmethod == "Invoice")
                {
                    paymentmethodquery = string.Format(" and PaymentMethod = '{0}'", paymentmethod);
                }
                else
                {
                    paymentmethodquery = " and (PaymentMethod = 'Credit Card' )";
                }

            }

            //    if (Start.HasValue && End.HasValue)
            //    {
            //        sqlQuery = @"select cus.Id,  

            //                                {2} [Lead Name] ,
            //                                 cus.Status as [Lead Status],
            //                                 (ee.FirstName +' '+ ee.LastName) as [Sales Opener],
            //                                 lk.DisplayText  as [Lead Source],
            //                                (emp.FirstName +' '+ emp.LastName) as [Sales Person],

            //                                cast(cus.FollowUpDate as date)  as [Follow Up Date],
            //                                 lktype.DisplayText as [Lead Type]

            //from Customer cus
            //left join CustomerCompany cc on cc.CustomerId = cus.CustomerId
            //                        left join Employee ee on cus.SoldBy2 = ee.UserId
            //                        left join Employee emp on cus.SoldBy = emp.UserId

            //                        left join LookUp lk on cus.LeadSource = lk.DataValue and lk.DataKey='LeadSource'
            //                        left join LookUp lktype on lktype.DataValue = cus.Type  and lktype.DataKey='CustomerType' 

            //where cc.CompanyId = '{0}'
            //                            and cus.FollowUpDate != ''
            //                            {3}
            //                           {4}
            //                           {5}
            //                       order by cus.FollowUpDate Desc
            //                    ";
            //        sqlQuery = string.Format(sqlQuery, companyId, subquery, NameSql, searchQuery, filterquery, DateQuery);
            //        // sqlQuery = string.Format(sqlQuery, companyId, Start.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), End.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), subquery, NameSql);
            //    }
            string sqlQuery = @"  
                               
                                                           
                                select  CASE
										WHEN cu.BusinessName IS NULL OR cu.BusinessName = '' THEN  cu.FirstName +' '+cu.LastName 
										ELSE cu.BusinessName
                                        END
                                        [Customer Name],cu.SubscriptionStatus as [Subscription Status],cu.Type ,cu.EmailAddress as Email ,cu.PrimaryPhone as [Phone No] ,cu.PaymentMethod as [Payment Method], cu.BillAmount as [Subscription Amount] FROM Customer cu
                               
                                where cu.AuthorizeRefId != '' and cu.AuthorizeRefId is not null {1}{2}{3} {4}{6}
                                order by Id desc
                                ";
           
                sqlQuery = string.Format(sqlQuery, companyId, searchTextQuery

                    , subquery
                    , StatusQuery
                    , DateQuery
                    , DateQuery2
                    , paymentmethodquery);
            
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", SearchText)));

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetAllCustomerByFilter(AllCustomerFilter filter)
        {

            string subquery = "";
            string subquery1 = "";
            string searchTextQuery = "";
            string searchTextQuery1 = "";
            string StatusQuery = "";
            string DateQuery = "";
            string DateQuery2 = "";
            string paymentmethodquery = "";
            if (filter.StartDate != new DateTime() && filter.EndDate != new DateTime())
            {
                var StartDate = filter.StartDate.UTCToClientTime().SetZeroHour();
                var EndDate = filter.EndDate.UTCToClientTime().SetMaxHour();
                DateQuery = string.Format("and cu.CreatedDate between '{0}' and '{1}'", StartDate, EndDate);
                DateQuery2 = string.Format("and CreatedDate between '{0}' and '{1}'", StartDate, EndDate);
            }
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                searchTextQuery = "and ((cu.SearchText like @SearchText) OR (cu.DBA like @SearchText))";
            }
            if (!string.IsNullOrWhiteSpace(filter.SearchText))
            {
                searchTextQuery1 = "and ((SearchText like @SearchText) OR (DBA like @SearchText))";
            }
            if (!string.IsNullOrWhiteSpace(filter.Status))
            {
                if(filter.Status == "suspended" || filter.Status == "active")
                {
                    StatusQuery = string.Format(" and SubscriptionStatus = '{0}'", filter.Status);
                }
                else
                {
                    StatusQuery = " and (SubscriptionStatus != 'suspended' and SubscriptionStatus != 'active')";
                }
              
            }
            if (!string.IsNullOrWhiteSpace(filter.Paymentmethod))
            {
                if (filter.Paymentmethod == "ACH" )
                {
                    paymentmethodquery = string.Format(" and PaymentMethod = '{0}'", filter.Paymentmethod);
                }
               else if (filter.Paymentmethod == "Invoice")
                {
                    paymentmethodquery = string.Format(" and PaymentMethod = '{0}'", filter.Paymentmethod);
                }
                else
                {
                    paymentmethodquery = " and (PaymentMethod = 'Credit Card' )";
                }

            }
            #region Order
            if (!string.IsNullOrWhiteSpace(filter.Order))
            {
                if (filter.Order == "ascending/customername")
                {
                    subquery = "order by cu.FirstName asc";
                    subquery1 = "order by cu.FirstName asc";
                }
                else if (filter.Order == "descending/customername")
                {
                    subquery = "order by cu.FirstName desc";
                    subquery1 = "order by cu.FirstName desc";
                }
                else if (filter.Order == "ascending/status")
                {
                    subquery = "order by [Status] asc";
                    subquery1 = "order by [Status] asc";
                }
                else if (filter.Order == "descending/status")
                {
                    subquery = "order by [Status] desc";
                    subquery1 = "order by [Status] desc";
                }
                else if (filter.Order == "ascending/email")
                {
                    subquery = "order by [EmailAddress] asc";
                    subquery1 = "order by [EmailAddress] asc";
                }
                else if (filter.Order == "descending/email")
                {
                    subquery = "order by [EmailAddress] desc";
                    subquery1 = "order by [EmailAddress] desc";
                }
                else if (filter.Order == "ascending/type")
                {
                    subquery = "order by [Type] asc";
                    subquery1 = "order by [Type] asc";
                }
                else if (filter.Order == "descending/type")
                {
                    subquery = "order by [Type] desc";
                    subquery1 = "order by [Type] desc";
                }
                else if (filter.Order == "ascending/phoneno")
                {
                    subquery = "order by [CellNo] asc";
                    subquery1 = "order by [CellNo] desc";
                }
                else if (filter.Order == "descending/phoneno")
                {
                    subquery = "order by [CellNo] asc";
                    subquery1 = "order by [CellNo] asc";
                }
              
            }
            else
            {
                subquery = "order by [Id] desc";
                subquery1 = "order by Id desc";
            }
            #endregion
            string sqlQuery = @"  
                                declare @pagesize int
                                declare @pageno int 
                                set @pagesize = " + filter.PageSize + @"
                                set @pageno = " + filter.PageNo + @"
                                declare @pagestart int
                                set @pagestart=(@pageno-1)* @pagesize  
                                select  TOP (@pagesize) cu.*, CASE
										WHEN cu.BusinessName IS NULL OR cu.BusinessName = '' THEN  cu.FirstName +' '+cu.LastName 
										ELSE cu.BusinessName
                                        END
                                        [Name] into #Testtable FROM Customer cu
                               
                                where cu.AuthorizeRefId != '' and cu.AuthorizeRefId is not null {0}{2}{5}{3} and  cu.Id NOT IN(Select TOP (@pagestart) Id from Customer {1})
                                {1}
                                                                      
                                select *  from #Testtable
				                select sum(BillAmount) as TotalAmountByPage from #TestTable 
                                select Count(Id) As TotalCount from Customer  where AuthorizeRefId != '' and AuthorizeRefId is not null {2}{5}{4}
                                --and (FirstName+' '+LastName like '%{6}%' or LastName like '%{6}%' or BusinessName like '%{6}%' or EmailAddress like '%{6}%' or PrimaryPhone like '%{6}%')
                                {7}
                                drop table #Testtable
";
            try
            {
                sqlQuery = string.Format(sqlQuery
                   
                    , searchTextQuery
                    
                    , subquery
                    , StatusQuery
                    ,DateQuery
                    , DateQuery2
                    ,paymentmethodquery
                    , filter.SearchText
                    , searchTextQuery1
                   );
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pNVarChar("SearchText", string.Format("%{0}%", filter.SearchText)));

                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                    //SqlDataReader reader;
                    //long result = SelectRecords(cmd, out reader);
                    //List<DeclinedTransactions> list = new DeclinedTransactionsList();
                    //using (reader)
                    //{
                    //    while (reader.Read())
                    //    {
                    //        DeclinedTransactions DeclinedTransactionsObject = new DeclinedTransactions();
                    //        FillObject(DeclinedTransactionsObject, reader);
                    //        DeclinedTransactionsObject.CustomerBusinessName = reader["CustomerBusinessName"].ToString();
                    //        DeclinedTransactionsObject.CustomerName = reader["CustomerName"].ToString();
                    //        list.Add(DeclinedTransactionsObject);
                    //    }
                    //    reader.Close();
                    //}

                    //return list;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
