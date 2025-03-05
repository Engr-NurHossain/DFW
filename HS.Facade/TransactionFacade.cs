using HS.DataAccess;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HS.Entities;
using System.Data;

namespace HS.Facade
{
    public class TransactionFacade:BaseFacade
    {
        public TransactionFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        TransactionDataAccess _TransactionDataAccess
        {
            get
            {
                return (TransactionDataAccess)_ClientContext[typeof(TransactionDataAccess)];
            }
        }
        RoutingNumberDataAccess _RoutingNumberDataAccess
        {
            get
            {
                return (RoutingNumberDataAccess)_ClientContext[typeof(RoutingNumberDataAccess)];
            }
        }

        TransactionExpenseDataAccess _TransactionExpenseDataAccess
        {
            get
            {
                return (TransactionExpenseDataAccess)_ClientContext[typeof(TransactionExpenseDataAccess)];
            }
        }
        CustomerCreditDataAccess _CustomerCreditDataAccess
        {
            get
            {
                return (CustomerCreditDataAccess)_ClientContext[typeof(CustomerCreditDataAccess)];
            }
        }
        InvoiceDataAccess _InvoiceDataAccess
        {
            get
            {
                return (InvoiceDataAccess)_ClientContext[typeof(InvoiceDataAccess)];
            }
        }

        TransactionHistoryDataAccess _TransactionHistoryDataAccess
        {
            get
            {
                return (TransactionHistoryDataAccess)_ClientContext[typeof(TransactionHistoryDataAccess)];
            }
        }
        RMRBillingMismatchDataAccess _RMRBillingMismatchDataAccess
        {
            get
            {
                return (RMRBillingMismatchDataAccess)_ClientContext[typeof(RMRBillingMismatchDataAccess)];
            }
        }
        TransactionQueueDataAccess _TransactionQueueDataAccess
        {
            get
            {
                return (TransactionQueueDataAccess)_ClientContext[typeof(TransactionQueueDataAccess)];
            }
        }
        public List<CustomerCredit> GetCustomerCreditListByCustomerId(Guid customerId)
        {
            return _CustomerCreditDataAccess.GetCustomerCreditListByCustomerId(customerId);
        }

        public TransactionHistory GetTransactionHistoryById(int transactionHistoryId)
        {
            return _TransactionHistoryDataAccess.Get(transactionHistoryId);
        }

        public List<Transaction> GetAllTransactionsByCustomerIdAndCompanyIdAndFilter(int customerId, Guid CompanyId,string SearchText, Guid empid, CustomerFilter filter)
        {
            //DataTable dt = _InvoiceNoteDataAccess.GetAllInvoiceNoteByInvoiceIdAndCompanyId(InvoiceId, ComId);
            DataTable dt = _TransactionDataAccess.GetAllTransactionsByCustomerIdAndCompanyIdAndFilter(customerId, CompanyId, SearchText, empid, filter);
            List<Transaction> TransactionList = new List<Transaction>();
            TransactionList = (from DataRow dr in dt.Rows
                        select new Transaction()
                        {
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                            invId = dr["invId"] != DBNull.Value ? Convert.ToInt32(dr["invId"]) : 0,
                            Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                            Status = dr["Status"].ToString(),
                            Note = dr["Note"].ToString(),
                            Type = dr["Type"].ToString(),
                            Balance = dr["Balance"] != DBNull.Value ? Convert.ToDouble(dr["Balance"]) : 0,
                            CustomerId = (Guid)dr["CustomerId"], 
                            CompanyId = (Guid)dr["CompanyId"],
                            CheckNo = dr["CheckNo"].ToString(),
                            InvoiceId = dr["InvoiceId"] != DBNull.Value ? Convert.ToInt32(dr["InvoiceId"]) : 0,
                            TransacationDate = dr["TransacationDate"] != DBNull.Value ? Convert.ToDateTime(dr["TransacationDate"]) : new DateTime(),
                            InvoiceDueDate = dr["InvoiceDueDate"] != DBNull.Value ? Convert.ToDateTime(dr["InvoiceDueDate"]) : new DateTime(),
                            InvoiceIdStr = dr["InvoiceIdStr"].ToString(),
                            PaymentMethod = dr["PaymentMethod"].ToString(),
                      
                            Description = dr["Description"].ToString(),
                            InvoiceNo = dr["InvoiceNo"].ToString(),
                            TransactionId = dr["CardTransactionId"].ToString(),
                            TransactionUserName = dr["ReceivedBy"].ToString(),
                            RefundAmount = dr["RefundAmount"] != DBNull.Value ? Convert.ToDouble(dr["RefundAmount"]) : 0,
                        }).ToList();
            return TransactionList;
        }
        public TransactionExpenseModel GetAllExpenseByComanyId(Guid CompanyId, VendorBillFilter filter)
        {
            DataSet dt = _TransactionDataAccess.GetAllExpenseByComanyId(CompanyId, filter);
            List<TransactionExpense> ExpenseList = new List<TransactionExpense>();
            TransactionExpenseCount TotalExpense = new TransactionExpenseCount();
            ExpenseList = (from DataRow dr in dt.Tables[0].Rows
                           select new TransactionExpense()
                           {
                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                               Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                               Status = dr["Status"].ToString(),
                               Type = dr["Type"].ToString(),
                               ExpenseTypeVal = dr["ExpenseTypeVal"].ToString(),
                               ExpenseType = dr["ExpenseType"].ToString(),
                               CustomerId = (Guid)dr["CustomerId"],
                               CompanyId = (Guid)dr["CompanyId"],
                               CheckNo = dr["CheckNo"].ToString(),
                               ExpenseBy = dr["ExpensedBy"].ToString(),
                               CreatedByVal = dr["CreatedByVal"].ToString(),
                               ExpenseDate = dr["ExpenseDate"] != DBNull.Value ? Convert.ToDateTime(dr["ExpenseDate"]) : new DateTime(),
                               RefType = dr["RefType"].ToString(),
                               PaymentMethod = dr["PaymentMethod"].ToString(),
                               Description = dr["Description"].ToString(),
                               FilePath = dr["FilePath"].ToString(),
                               TicketNo = dr["TicketNo"] != DBNull.Value ? Convert.ToInt32(dr["TicketNo"]) : 0,

                           }).ToList();

            TotalExpense = (from DataRow dr in dt.Tables[1].Rows
                            select new TransactionExpenseCount()
                            {
                                TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                            }).FirstOrDefault();

            TransactionExpenseModel model = new TransactionExpenseModel();
            model.TransactionExpenseList = ExpenseList;
            model.TransactionExpenseCount = TotalExpense;
            return model;
        }
        public List<TransactionExpense> GetAllExpenseByCustomerIdAndCompanyIdAndFilter(int customerId, Guid CompanyId, string SearchText, Guid empid, CustomerFilter filter)
        {
            //DataTable dt = _InvoiceNoteDataAccess.GetAllInvoiceNoteByInvoiceIdAndCompanyId(InvoiceId, ComId);
            DataTable dt = _TransactionDataAccess.GetAllExpenseByCustomerIdAndCompanyIdAndFilter(customerId, CompanyId, SearchText, empid, filter);
            List<TransactionExpense> ExpenseList = new List<TransactionExpense>();
            ExpenseList = (from DataRow dr in dt.Rows
                               select new TransactionExpense()
                               {
                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                    Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                                    Status = dr["Status"].ToString(),
                                    Type = dr["Type"].ToString(),
                                    FilePath = dr["FilePath"].ToString(),
                                    CustomerId = (Guid)dr["CustomerId"],
                                    CompanyId = (Guid)dr["CompanyId"],
                                    CheckNo = dr["CheckNo"].ToString(),
                                
                                    ExpenseDate = dr["ExpenseDate"] != DBNull.Value ? Convert.ToDateTime(dr["ExpenseDate"]) : new DateTime(),
                                    RefType = dr["RefType"].ToString(),
                                    PaymentMethod = dr["PaymentMethod"].ToString(),
                                    Description = dr["Description"].ToString(), 
                               }).ToList();
            return ExpenseList;
        }
        public List<TransactionExpense> GetAllExpenseByCustomerIdAndCompanyIdAndFilter(int customerId, Guid CompanyId, string SearchText, Guid empid)
        {
            //DataTable dt = _InvoiceNoteDataAccess.GetAllInvoiceNoteByInvoiceIdAndCompanyId(InvoiceId, ComId);
            DataTable dt = _TransactionDataAccess.GetAllExpenseByCustomerIdAndCompanyIdAndFilter(customerId, CompanyId, SearchText, empid);
            List<TransactionExpense> ExpenseList = new List<TransactionExpense>();
            ExpenseList = (from DataRow dr in dt.Rows
                           select new TransactionExpense()
                           {
                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                               Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                               Status = dr["Status"].ToString(),
                               Type = dr["Type"].ToString(),

                               CustomerId = (Guid)dr["CustomerId"],
                               CompanyId = (Guid)dr["CompanyId"],
                               CheckNo = dr["CheckNo"].ToString(),

                               ExpenseDate = dr["ExpenseDate"] != DBNull.Value ? Convert.ToDateTime(dr["ExpenseDate"]) : new DateTime(),
                               RefType = dr["RefType"].ToString(),
                               PaymentMethod = dr["PaymentMethod"].ToString(),
                               Description = dr["Description"].ToString(),


                           }).ToList();
            return ExpenseList;
        }
        public TransactionPdfModel GetTransactionPdfDataByTransactionId(int transactionId)
        {
            return _TransactionDataAccess.GetTransactionPdfDataByTransactionId(transactionId);
        }

        public List<Transaction> GetAllTransactions1ByCustomerIdAndCompanyIdAndFilter(int customerId, Guid CompanyId, string SearchText, Guid empid)
        {
            //DataTable dt = _InvoiceNoteDataAccess.GetAllInvoiceNoteByInvoiceIdAndCompanyId(InvoiceId, ComId);
            DataTable dt = _TransactionDataAccess.GetAllTransactions1ByCustomerIdAndCompanyIdAndFilter(customerId, CompanyId, SearchText, empid);
            List<Transaction> TransactionList = new List<Transaction>();
            TransactionList = (from DataRow dr in dt.Rows
                               select new Transaction()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                                   Status = dr["Status"].ToString(),
                                   Type = dr["Type"].ToString(),
                                   Balance = dr["Balance"] != DBNull.Value ? Convert.ToDouble(dr["Balance"]) : 0,
                                   CustomerId = (Guid)dr["CustomerId"],
                                   CompanyId = (Guid)dr["CompanyId"],
                                   CheckNo = dr["CheckNo"].ToString(),
                                   InvoiceId = dr["InvoiceId"] != DBNull.Value ? Convert.ToInt32(dr["InvoiceId"]) : 0,
                                   TransacationDate = dr["TransacationDate"] != DBNull.Value ? Convert.ToDateTime(dr["TransacationDate"]) : new DateTime(),
                                   InvoiceDueDate = dr["InvoiceDueDate"] != DBNull.Value ? Convert.ToDateTime(dr["InvoiceDueDate"]) : new DateTime(),
                                   InvoiceIdStr = dr["InvoiceIdStr"].ToString(),
                                   PaymentMethod = dr["PaymentMethod"].ToString(),
                                   Description = dr["Description"].ToString(),
                                   InvoiceNo = dr["InvoiceNo"].ToString(),
                                   TransactionId = dr["CardTransactionId"].ToString(),
                                   TransactionUserName = dr["ReceivedBy"].ToString()
                               }).ToList();
            return TransactionList;
        }

        public Transaction GetTransactionById(int id)
        {
            return _TransactionDataAccess.Get(id);
        }

        public TransactionExpense GetTransactionExpenseById(int value)
        {
            return _TransactionExpenseDataAccess.Get(value);
        }
        public DataTable GetFundingReport(int[] IdList, string[] columnList, Guid CompanyId)
        {
            return _TransactionDataAccess.GetFundingReport(IdList, columnList, CompanyId);
        }
        public DataTable GetExpenseReport(int[] IdList, string[] columnList, Guid CompanyId)
        {
            return _TransactionDataAccess.GetExpenseReport(IdList, columnList, CompanyId);
        }
        //it will work same as GetAllTransactionsByCustomerIdAndCompanyId but receives all Transaction...
        //not dependent on customer
        public AllSalesInfoModel GetAllTransactionsByCompanyId(Guid CompanyId,int PageSize,int PageNo,string SearchText,string SearchBy, string ColumnName, string AscOrDescVal, string FromDate, string ToDate,string order, string InvoiceStatus)
        {

            AllSalesInfoModel Model = new AllSalesInfoModel(); 

            //DataTable dt2 = _TransactionDataAccess.GetAllTransactionsByCustomerIdAndCompanyId(null, CompanyId);
            DataSet ds = _TransactionDataAccess.GetAllTransactionsByCompanyIdAndFilters(CompanyId,PageSize,SearchText,SearchBy,PageNo, ColumnName, AscOrDescVal, FromDate, ToDate,order, InvoiceStatus);
            Model.TransactionList = new List<Transaction>();
            Model.TransactionList = (from DataRow dr in ds.Tables[0].Rows
                               select new Transaction()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                                   Status = dr["Status"].ToString(),
                                   Type = dr["Type"].ToString(),
                                   Balance = dr["Balance"] != DBNull.Value ? Convert.ToDouble(dr["Balance"]) : 0,
                                   CustomerId = (Guid)dr["CustomerId"],
                                   CompanyId = (Guid)dr["CompanyId"],
                                   InvoiceId = dr["InvoiceId"] != DBNull.Value ? Convert.ToInt32(dr["InvoiceId"]) : 0,
                                   TransacationDate = dr["TransacationDate"] != DBNull.Value ? Convert.ToDateTime(dr["TransacationDate"]) : new DateTime(),
                                   InvoiceDueDate = dr["InvoiceDueDate"] != DBNull.Value ? Convert.ToDateTime(dr["InvoiceDueDate"]) : new DateTime(),
                                   InvoiceIdStr = dr["InvoiceIdStr"].ToString(),
                                   CustomerName = dr["CustomerName"].ToString(),
                                   AuthRefId = dr["AuthRefId"].ToString(),
                                   CustomerBussinessName = dr["CustomerBussinessName"].ToString()
                               }).ToList();
            Model.TotalAmountByPage = ds.Tables[1].Rows[0]["TotalAmountByPage"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["TotalAmountByPage"]) : 0;
            Model.TotalBalanceByPage = ds.Tables[1].Rows[0]["TotalBalanceByPage"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["TotalBalanceByPage"]) : 0;

            Model.TotalOpenEstimates = ds.Tables[2].Rows[0]["TotalOpenEstimates"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[0]["TotalOpenEstimates"]) : 0;
            Model.TotalOpenEstimatesAmount = ds.Tables[2].Rows[0]["TotalOpenEstimatesAmount"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalOpenEstimatesAmount"]) : 0;
            Model.TotalRevenue = ds.Tables[2].Rows[0]["TotalRevenue"] != DBNull.Value ? Convert.ToDouble(ds.Tables[2].Rows[0]["TotalRevenue"]) : 0;
            Model.AccountsReceivable = ds.Tables[2].Rows[0]["AccountsReceivable"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[0]["AccountsReceivable"]) : 0;
            Model.TotalCount = ds.Tables[2].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[0]["TotalCount"]) : 0;

            return Model;
        }

        public DataTable DownLoadAllTransactionsByCompanyId(Guid CompanyId, int PageSize, int PageNo, string SearchText, string SearchBy, string ColumnName, string AscOrDescVal, string FromDate, string ToDate, string order)
        {

            AllSalesInfoModel Model = new AllSalesInfoModel();

            //DataTable dt2 = _TransactionDataAccess.GetAllTransactionsByCustomerIdAndCompanyId(null, CompanyId);
            return _TransactionDataAccess.DownLoadAllTransactionsByCompanyIdAndFilters(CompanyId, PageSize, SearchText, SearchBy, PageNo, ColumnName, AscOrDescVal, FromDate, ToDate, order);
           
        }


        public bool UpdateTransactionExpense(TransactionExpense model)
        {
            return _TransactionExpenseDataAccess.Update(model)>0;
        }

        public AllSalesInfoModel GetAllTransactionsByCompanyIdForReport(Guid CompanyId)
        {
            AllSalesInfoModel Model = new AllSalesInfoModel();

            DataSet ds = _TransactionDataAccess.GetAllTransactionsByCompanyIdForReports(CompanyId);
            Model.TransactionList = new List<Transaction>();
            Model.TransactionList = (from DataRow dr in ds.Tables[0].Rows
                                     select new Transaction()
                                     {
                                         Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                         Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                                         Status = dr["Status"].ToString(),
                                         Type = dr["Type"].ToString(),
                                         Balance = dr["Balance"] != DBNull.Value ? Convert.ToDouble(dr["Balance"]) : 0,
                                         CustomerId = (Guid)dr["CustomerId"],
                                         CompanyId = (Guid)dr["CompanyId"],
                                         InvoiceId = dr["InvoiceId"] != DBNull.Value ? Convert.ToInt32(dr["InvoiceId"]) : 0,
                                         TransacationDate = dr["TransacationDate"] != DBNull.Value ? Convert.ToDateTime(dr["TransacationDate"]) : new DateTime(),
                                         InvoiceDueDate = dr["InvoiceDueDate"] != DBNull.Value ? Convert.ToDateTime(dr["InvoiceDueDate"]) : new DateTime(),
                                         InvoiceIdStr = dr["InvoiceIdStr"].ToString(),
                                         CustomerName = dr["CustomerName"].ToString(),
                                         AuthRefId = dr["AuthRefId"].ToString(),
                                         CustomerBussinessName = dr["CustomerBussinessName"].ToString()
                                     }).ToList();
            return Model;
        }
        public bool DeleteTransactionAndHistoryByTranId(int id)
        {
            return _TransactionDataAccess.DeleteTransactionAndHistoryByTranId(id);
        }
        public bool DeleteTransactionAndHistoryByCardTranId(string TransactionId, Guid CustomerId)
        {
            return _TransactionDataAccess.DeleteTransactionAndHistoryByCardTranId(TransactionId,CustomerId);
        }
        
        public Transaction GetTransactionByIdAndCompanyId(int TransactionId, Guid CompanyId)
        {
            return _TransactionDataAccess.GetByQuery(string.Format("CompanyId = '{1}' and id = {0}",TransactionId,CompanyId)).FirstOrDefault();
        }

        public List<Transaction> GetAllTransactionsByCompanyId(Guid CompanyId)
        {
             
            DataTable dt = _TransactionDataAccess.GetAllTransactionsByCompanyId(CompanyId);
            List<Transaction> TransactionList = new List<Transaction>();
            TransactionList = (from DataRow dr in dt.Rows
                               select new Transaction()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                                   Status = dr["Status"].ToString(),
                                   Type = dr["Type"].ToString(),
                                   Balance = dr["Balance"] != DBNull.Value ? Convert.ToDouble(dr["Balance"]) : 0,
                                   CustomerId = (Guid)dr["CustomerId"],
                                   CompanyId = (Guid)dr["CompanyId"],
                                   InvoiceId = dr["InvoiceId"] != DBNull.Value ? Convert.ToInt32(dr["InvoiceId"]) : 0,
                                   TransacationDate = dr["TransacationDate"] != DBNull.Value ? Convert.ToDateTime(dr["TransacationDate"]) : new DateTime(),
                                   InvoiceDueDate = dr["InvoiceDueDate"] != DBNull.Value ? Convert.ToDateTime(dr["InvoiceDueDate"]) : new DateTime(),
                                   InvoiceIdStr = dr["InvoiceIdStr"].ToString(),
                               }).ToList();
            return TransactionList;
        }

        public List<TransactionHistory> GetAllTransactionHistoryByTransacetionId(int value)
        {
            return _TransactionHistoryDataAccess.GetByQuery(string.Format("TransactionId ={0}", value));
        }
         
        public List<OutStandingTransactions> GetReceivePaymentListByCustomerId(int customerId,Guid CompanyId,int?TransactionId)
        {
            DataTable dt = _TransactionDataAccess.GetReceivePaymentListByCustomerId(customerId, CompanyId, TransactionId);
            List<OutStandingTransactions> TransactionList = new List<OutStandingTransactions>();
            TransactionList = (from DataRow dr in dt.Rows
                               select new OutStandingTransactions()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0, 
                                   Description = dr["Description"].ToString(),
                                   CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                   DueDate = dr["DueDate"] != DBNull.Value ? Convert.ToDateTime(dr["DueDate"]) : new DateTime(),
                                   OriginalAmount = dr["OriginalAmount"] != DBNull.Value ? Convert.ToDouble(dr["OriginalAmount"]) : 0,
                                   OpenBalance = dr["OpenBalance"] != DBNull.Value ? Convert.ToDouble(dr["OpenBalance"]) : 0,
                                   Payment = dr["Payment"] != DBNull.Value ? Convert.ToDouble(dr["Payment"]) : 0,
                               }).ToList();
            return TransactionList;
        }

        #region Receive Payment Report

        public List<TransactionHistory> GetAllTransactionHistoryByCompanyIdAndDates(Guid CompanyId, DateTime Start, DateTime End)
        {
            DataTable dt = _TransactionHistoryDataAccess.GetAllTransactionHistoryByCompanyId(CompanyId, Start, End);
            List<TransactionHistory> TransactionList = new List<TransactionHistory>();
            TransactionList = (from DataRow dr in dt.Rows
                               select new TransactionHistory()
                               {
                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                    Amout = dr["Amout"] != DBNull.Value ? Convert.ToDouble(dr["Amout"]) : 0,
                                    Balance = dr["Balance"] != DBNull.Value ? Convert.ToDouble(dr["Balance"]) : 0,
                                    InvoiceId = dr["InvoiceId"] != DBNull.Value ? Convert.ToInt32(dr["InvoiceId"]) : 0,
                                    TransacationDate = dr["TransacationDate"] != DBNull.Value ? Convert.ToDateTime(dr["TransacationDate"]) : new DateTime(),
                                    CustomerName = dr["CustomerName"].ToString(),
                                    InvoiceNumber = dr["InvoiceNumber"].ToString(),
                                    TransactionId = dr["TransactionId"] != DBNull.Value ? Convert.ToInt32(dr["TransactionId"]) : 0,
                               }).ToList();
            return TransactionList;
        }

        public double GetCustomerCreditAmountByCustomerId(Guid customerId)
        {
            return _CustomerCreditDataAccess.GetCustomerCreditAmountByCustomerId(customerId);
        }
        public double GetCustomerCreditAmountByCustomerIdWithBoolValue(Guid customerId, bool RMRCredit, bool OtherCredit)
        {
            return _CustomerCreditDataAccess.GetRMRCustomerCreditAmountByCustomerId(customerId, RMRCredit, OtherCredit);
        }

        public DataTable GetAllTransactionHistoryReportByCompanyId(Guid CompanyId,DateTime?Start,DateTime?End)
        {
             return _TransactionDataAccess.GetAllTransactionsReportByCompanyId(CompanyId,Start,End);
        }

        public List<TransactionHistory> GetAllTransactionHistoryByCompanyId(Guid CompanyId)
        {
            DataTable dt = _TransactionHistoryDataAccess.GetAllTransactionHistoryByCompanyId(CompanyId,null,null);
            List<TransactionHistory> TransactionList = new List<TransactionHistory>();
            TransactionList = (from DataRow dr in dt.Rows
                               select new TransactionHistory()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   Amout = dr["Amout"] != DBNull.Value ? Convert.ToDouble(dr["Amout"]) : 0,
                                   Balance = dr["Balance"] != DBNull.Value ? Convert.ToDouble(dr["Balance"]) : 0,
                                   InvoiceId = dr["InvoiceId"] != DBNull.Value ? Convert.ToInt32(dr["InvoiceId"]) : 0,
                                   TransacationDate = dr["TransacationDate"] != DBNull.Value ? Convert.ToDateTime(dr["TransacationDate"]) : new DateTime(),
                                   CustomerName = dr["CustomerName"].ToString(),
                                   InvoiceNumber = dr["InvoiceNumber"].ToString(),
                                   TransactionId = dr["TransactionId"] != DBNull.Value ? Convert.ToInt32(dr["TransactionId"]) : 0,
                               }).ToList();
            return TransactionList;
        }
        #endregion 

        public List<TransactionHistory> GetAllTransactionHistoryByInvoiceId(int value)
        {
            return _TransactionHistoryDataAccess.GetByQuery(string.Format("InvoiceId ={0}", value));
        }

        public Transaction GetTransactionByInvoiceId(int InvoiceId)
        {
            return _TransactionDataAccess.GetByQuery(string.Format("InvoiceId = {0}", InvoiceId)).FirstOrDefault();
        }

        public Transaction GetTransactionByReferenceId(string InvoiceId)
        {
            return _TransactionDataAccess.GetByQuery(string.Format("ReferenceNo = '{0}'", InvoiceId)).FirstOrDefault();
        }

        public int InsertTransactionHistory(TransactionHistory trh)
        {
            return (int)_TransactionHistoryDataAccess.Insert(trh);
        }

        public int InsertTransactionExpense(TransactionExpense tre)
        {
            return (int)_TransactionExpenseDataAccess.Insert(tre);
        }
        public int InsertTransactionQueue(TransactionQueue transqueue)
        {
            return (int)_TransactionQueueDataAccess.Insert(transqueue);
        }
        public List<TransactionQueue> GetTransactionQueueCustomerId(Guid customerId,string starttime, string endtime,double amount)
        {
            DataTable dt = _TransactionDataAccess.GetTransactionQueueCustomerId(customerId, starttime, endtime, amount);
            List<TransactionQueue> TransactionQueueList = new List<TransactionQueue>();
            TransactionQueueList = (from DataRow dr in dt.Rows
                               select new TransactionQueue()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                 
                                   Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                               }).ToList();
            return TransactionQueueList;
        }
        public void InsertTransactionHistoryList(List<TransactionHistory> trhistory)
        {

            string TrHistoryInsertTemplate = "INSERT INTO [TransactionHistory] ( TransactionId, InvoiceId, Amout,Balance,ReceivedBy) VALUES ({0},{1},{2},{3},'{4}');";
            string sql = "";
            foreach(var item in trhistory)
            {
                sql += string.Format(TrHistoryInsertTemplate,item.TransactionId,item.InvoiceId,item.Amout,item.Balance,item.ReceivedBy) +Environment.NewLine;
            }
            _TransactionHistoryDataAccess.InsertTransactionHistoryList(sql);
        }

        public int InsertTransaction(Transaction transaction)
        {
            return (int)_TransactionDataAccess.Insert(transaction);
        }

        public bool UpdateTransaction(Transaction transaction)
        {
            return _TransactionDataAccess.Update(transaction)>0;
        }

        public AllSalesInfoModel GetAllFilterTransactionsByCompanyId(Guid CompanyId, string Fdate, string Tdate)
        {
            AllSalesInfoModel Model = new AllSalesInfoModel();
            DataTable dt = _TransactionDataAccess.GetAllFilterTransactionsByCompanyId(CompanyId, Fdate, Tdate);
            Model.TransactionList = new List<Transaction>();
            Model.TransactionList = (from DataRow dr in dt.Rows
                                     select new Transaction()
                                     {
                                         Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                         Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                                         Status = dr["Status"].ToString(),
                                         Type = dr["Type"].ToString(),
                                         Balance = dr["Balance"] != DBNull.Value ? Convert.ToDouble(dr["Balance"]) : 0,
                                         CustomerId = (Guid)dr["CustomerId"],
                                         CompanyId = (Guid)dr["CompanyId"],
                                         InvoiceId = dr["InvoiceId"] != DBNull.Value ? Convert.ToInt32(dr["InvoiceId"]) : 0,
                                         TransacationDate = dr["TransacationDate"] != DBNull.Value ? Convert.ToDateTime(dr["TransacationDate"]) : new DateTime(),
                                         InvoiceDueDate = dr["InvoiceDueDate"] != DBNull.Value ? Convert.ToDateTime(dr["InvoiceDueDate"]) : new DateTime(),
                                         InvoiceIdStr = dr["InvoiceIdStr"].ToString(),
                                         CustomerName = dr["CustomerName"].ToString(),
                                         AuthRefId = dr["AuthRefId"].ToString(),
                                         CustomerBussinessName = dr["CustomerBussinessName"].ToString()
                                     }).ToList();
            return Model;
        }

        public List<Invoice> GetInvoiceListByCardTransactionIdList(List<string> transactionIdList)
        {
            string TransactionIds = "";
            foreach (var item in transactionIdList)
            {
                if (string.IsNullOrWhiteSpace(item))
                    continue;
                TransactionIds += string.Format("'{0}',",item);
            }
            TransactionIds = TransactionIds.TrimEnd(',');
            return _InvoiceDataAccess.GetInvoiceListByCardTransactionIdList(TransactionIds);
        }

        public int InsertCustomerCredit(CustomerCredit cusCredit)
        {
            return (int)_CustomerCreditDataAccess.Insert(cusCredit);
        }
        public int UpdateCustomerCredit(CustomerCredit cusCredit)
        {
            return (int)_CustomerCreditDataAccess.Update(cusCredit);
        }

        public List<TransactionHistory> GetTransactionHistoryByInvoiceId(int id)
        {
            return _TransactionHistoryDataAccess.GetByQuery(string.Format("InvoiceId = {0}",id));
        }

        public bool DeleteTransaction(int TransactionId)
        {
            return _TransactionDataAccess.Delete(TransactionId)>0;
        }

        public bool DeleteTransactionHistoryById(int id)
        {
            return _TransactionHistoryDataAccess.Delete(id)>0;
        }

        public Transaction GetTransactionByCustomerIdAndInvoiceId(Guid customerid, string invid)
        {
            return _TransactionDataAccess.GetTransactionDataByCustomerIdAndInvoiceId(customerid, invid);
            //return _TransactionDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and ReferenceNo = '{1}' and [Status] = 'Closed' order by Id desc", customerid, invid)).FirstOrDefault();
        }
        #region  Recurring Billing Mismatch
        public RMRBillingMismatch GetRecurringBillingMismatchById(int Id)
        {
            return _RMRBillingMismatchDataAccess.Get(Id);
        }
        public int InsertRecurringBillingMismatch(RMRBillingMismatch RMR)
        {
            return (int)_RMRBillingMismatchDataAccess.Insert(RMR);
        }
        public bool UpdateRecurringBillingMismatch(RMRBillingMismatch model)
        {
            return _RMRBillingMismatchDataAccess.Update(model) > 0;
        }
        public RMRBillingMismatchModel GetUnResolveRecurringBillingMismatchList(DateTime? Start, DateTime? End, int pageno, int pagesize, string searchtext)
        {
            DataSet dt = _TransactionDataAccess.GetUnResolveRecurringBillingMismatchList(Start, End, pageno, pagesize, searchtext);
            RMRBillingMismatchModel Model = new RMRBillingMismatchModel();
            Model.RMRBillingMismatchList = (from DataRow dr in dt.Tables[0].Rows
                                            select new RMRBillingMismatch()
                                            {
                                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                CustomerIntId = dr["CustomerId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerId"]) : 0,
                                                CustomerName = dr["CustomerName"].ToString(),
                                                TransactionId = dr["TransactionId"].ToString(),
                                                InvoiceId = dr["InvoiceId"].ToString(),
                                                BillingAmount = dr["BillingAmount"] != DBNull.Value ? Convert.ToDouble(dr["BillingAmount"]) : 0,
                                                ChargedAmountByGateway = dr["ChargedAmountByGateway"] != DBNull.Value ? Convert.ToDouble(dr["ChargedAmountByGateway"]) : 0,
                                                
                                            }).ToList();
            Model.TotalAmountByPage = dt.Tables[1].Rows[0]["TotalAmountByPage"] != DBNull.Value ? Convert.ToDouble(dt.Tables[1].Rows[0]["TotalAmountByPage"]) : 0;

            Model.TotalCount = (from DataRow dr in dt.Tables[2].Rows
                                select new Count()
                                {
                                    TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                }).FirstOrDefault();

            return Model;
        }
        public DataTable DownloadUnResolveRecurringBillingMismatchList(DateTime? Start, DateTime? End, string searchtext)
        {
            DataTable dt = _TransactionDataAccess.DownloadUnResolveRecurringBillingMismatchList(Start, End, searchtext);
            return dt;
        }
        public RMRBillingMismatchModel GetResolveRecurringBillingMismatchList(DateTime? Start, DateTime? End, int pageno, int pagesize, string searchtext)
        {
            DataSet dt = _TransactionDataAccess.GetResolveRecurringBillingMismatchList(Start,End,pageno,pagesize,searchtext);
            RMRBillingMismatchModel Model = new RMRBillingMismatchModel();
            Model.RMRBillingMismatchList = (from DataRow dr in dt.Tables[0].Rows
                           select new RMRBillingMismatch()
                           {
                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                               CustomerIntId = dr["CustomerId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerId"]) : 0,
                               CustomerName = dr["CustomerName"].ToString(),
                               TransactionId = dr["TransactionId"].ToString(),
                               InvoiceId = dr["InvoiceId"].ToString(),
                               BillingAmount = dr["BillingAmount"] != DBNull.Value ? Convert.ToDouble(dr["BillingAmount"]) : 0,
                               ChargedAmountByGateway = dr["ChargedAmountByGateway"] != DBNull.Value ? Convert.ToDouble(dr["ChargedAmountByGateway"]) : 0,
                               ResolvedByName = dr["ResolvedBy"].ToString(),
                               ResolvedDate = dr["ResolvedDate"] != DBNull.Value ? Convert.ToDateTime(dr["ResolvedDate"]) : new DateTime(),

                           }).ToList();
            Model.TotalAmountByPage = dt.Tables[1].Rows[0]["TotalAmountByPage"] != DBNull.Value ? Convert.ToDouble(dt.Tables[1].Rows[0]["TotalAmountByPage"]) : 0;


            Model.TotalCount = (from DataRow dr in dt.Tables[2].Rows
                            select new Count()
                            {
                                TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                            }).FirstOrDefault();
          
            return Model;
        }
        public DataTable DownloadResolveRecurringBillingMismatchList(DateTime? Start, DateTime? End, string searchtext)
        {
            DataTable dt = _TransactionDataAccess.DownloadResolveRecurringBillingMismatchList(Start, End, searchtext);
            return dt;
        }
        #endregion        
        public InvoicePaymentDate GetLatestPaymentDateByInvoiceId(int id)
        {
            DataTable dt = _TransactionDataAccess.GetLatestPaymentDateByInvoiceId(id);
            InvoicePaymentDate PaymentDate = new InvoicePaymentDate();
            if(dt != null)
            {
                PaymentDate = (from DataRow dr in dt.Rows
                 select new InvoicePaymentDate()
                 {
                     PaymentDate = dr["TransacationDate"] != DBNull.Value ? Convert.ToDateTime(dr["TransacationDate"]) : new DateTime()
                 }).FirstOrDefault();
            }
            return PaymentDate;
        }

        public RoutingNumber GetBankInfoFromRoutingNumber(string routingnumber)
        {
            return _RoutingNumberDataAccess.GetByQuery(string.Format("RoutingNumber = {0}",routingnumber)).FirstOrDefault();
        }

        public bool InsertRoutingNumber(RoutingNumber routingNumber)
        {
            return _RoutingNumberDataAccess.Insert(routingNumber) > 0;
        }
    }
}
