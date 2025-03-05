using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HS.Entities;
using HS.Framework;
using HS.DataAccess;
using System.Data;

namespace HS.Facade
{
    public class DeclinedTransactionsFacade:BaseFacade
    {
        public DeclinedTransactionsFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        DeclinedTransactionsDataAccess _DeclinedTransactionsDataAccess
        {
            get
            {
                return (DeclinedTransactionsDataAccess)_ClientContext[typeof(DeclinedTransactionsDataAccess)];
            }
        } 
        public List<string> GetExistingTransactionsByTransactionIdList(List<string> transactionIdList)
        {
            string TransactionIds = "";
            foreach (var item in transactionIdList)
            {
                if (string.IsNullOrWhiteSpace(item))
                    continue;
                TransactionIds += string.Format("'{0}',", item);
            }
            TransactionIds = TransactionIds.TrimEnd(',');

            return _DeclinedTransactionsDataAccess.GetExistingTransactionsByTransactionIdList(TransactionIds);
        }

        public int InsertDeclinedTransaction(DeclinedTransactions item)
        {
            return (int)_DeclinedTransactionsDataAccess.Insert(item);
        }

        public DeclinedTransactionView GetAllDeclinedTransactionsByFilter(AllReturnsFilter filter)
        {
            DataSet ds =  _DeclinedTransactionsDataAccess.GetAllDeclinedTransactionsByFilter(filter);
            DeclinedTransactionView Model = new DeclinedTransactionView();
            if (ds != null)
            {                
                Model.DeclinedTransaction = (from DataRow dr in ds.Tables[0].Rows
                                             select new DeclinedTransactions()
                                             {
                                                 Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                 CustomerIdValue = dr["CustomerIdValue"] != DBNull.Value ? Convert.ToInt32(dr["CustomerIdValue"]) : 0,
                                                 CompanyId = (Guid)dr["CompanyId"],
                                                 CustomerId = (Guid)dr["CustomerId"],
                                                 CustomerName = dr["CustomerName"].ToString(),
                                                 Comment = dr["Comment"].ToString(),
                                                 CustomerBusinessName = dr["CustomerBusinessName"].ToString(),
                                                 Reason = dr["Reason"].ToString(),
                                                 ReturnAmount = dr["ReturnAmount"] != DBNull.Value ? Convert.ToDouble(dr["ReturnAmount"]) : 0,
                                                 SettlementBatch = dr["SettlementBatch"] != DBNull.Value ? Convert.ToDateTime(dr["SettlementBatch"]) : new DateTime(),
                                                 SubmitAmount = dr["SubmitAmount"] != DBNull.Value ? Convert.ToDouble(dr["SubmitAmount"]) : 0,
                                                 TransactionId = dr["TransactionId"].ToString(),
                                                 InvoiceId = dr["InvoiceId"].ToString(),
                                                 ReturnedDate = dr["ReturnedDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReturnedDate"]) : new DateTime(),
                                                 InvId = dr["InvId"] != DBNull.Value ? Convert.ToInt32(dr["InvId"]) : 0
                                             }).ToList();
                Model.TotalAmountByPage = ds.Tables[1].Rows[0]["TotalAmountByPage"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["TotalAmountByPage"]) : 0;

                Model.TotalCount = ds != null && ds.Tables[2].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[0]["TotalCount"]) : 0;
            }
            return Model;

        }
        public DataTable GetAllCustomerByfilterDownload(Guid CompanyId, DateTime Start, DateTime End, string status, string paymentmethod,string SearchText)
        {
            return _DeclinedTransactionsDataAccess.GetAllCustomerByFilterDownload(CompanyId, Start, End, status, paymentmethod, SearchText);
        }
        public CustomerListWithCountModel GetAllCustomerByFilter(AllCustomerFilter filter)
        {
            DataSet ds = _DeclinedTransactionsDataAccess.GetAllCustomerByFilter(filter);
            DataTable dt = ds.Tables[0];
            DataTable dt1 = ds.Tables[1];
            DataTable dt2 = ds.Tables[2];

            List<Customer> CustomerList = new List<Customer>();
            CustomerList = (from DataRow dr in dt.Rows
                            select new Customer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                CustomerId = (Guid)dr["CustomerId"],
                                CustomerNo = dr["CustomerNo"].ToString(),
                                customerName = dr["Name"].ToString(),
                                PaymentMethod = dr["PaymentMethod"].ToString(),
                                BillAmount = dr["BillAmount"] != DBNull.Value ? Convert.ToDouble(dr["BillAmount"]) : 0,
                                MiddleName = dr["MiddleName"].ToString(),
                                BusinessName = dr["BusinessName"].ToString(),
                                Status = dr["Status"].ToString(),
                                Type = dr["Type"].ToString(),
                                Street = dr["Street"].ToString(),
                                City = dr["City"].ToString().UppercaseFirst(),
                                State = dr["State"].ToString(),
                                ZipCode = dr["ZipCode"].ToString(),
                                StreetPrevious = dr["StreetPrevious"].ToString(),
                                CityPrevious = dr["CityPrevious"].ToString().UppercaseFirst(),
                                StatePrevious = dr["StatePrevious"].ToString(),
                                ZipCodePrevious = dr["ZipCodePrevious"].ToString(),
                                Address = dr["Address"].ToString(),
                                Address2 = dr["Address2"].ToString(),
                                Area = dr["Area"].ToString(),
                                EmailAddress = dr["EmailAddress"].ToString(),
                                PrimaryPhone = dr["PrimaryPhone"].ToString(),
                      
                                
                        
                              
                                SubscriptionStatus = dr["SubscriptionStatus"].ToString(),
                                CreatedByUid = (Guid)dr["CreatedByUid"],
                            
                      
                            }).ToList();

            TotalCustomerCount TotalCustomer = new TotalCustomerCount();


          
            TotalCustomer = (from DataRow dr in dt2.Rows
                             select new TotalCustomerCount()
                             {
                                 Counter = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                             }).FirstOrDefault();

            CustomerListWithCountModel CustomerResultModel = new CustomerListWithCountModel();
            CustomerResultModel.TotalAmountByPage = ds.Tables[1].Rows[0]["TotalAmountByPage"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["TotalAmountByPage"]) : 0;

            CustomerResultModel.CustomerList = CustomerList;
            CustomerResultModel.TotalCustomerCount = TotalCustomer;
            return CustomerResultModel;
        }
    }
}
