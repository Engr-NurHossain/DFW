using HS.DataAccess;
using HS.Entities;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HS.Facade
{
    public class InvoiceFacade : BaseFacade
    {
        public InvoiceFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }

        public InvoiceFacade()
        {
        }
        #region DataAccess
        
        InvoiceDetailDataAccess _InvoiceDetailDataAccess
        {
            get
            {
                return (InvoiceDetailDataAccess)_ClientContext[typeof(InvoiceDetailDataAccess)];
            }
        }
        CustomerProratedBillDataAccess _CustomerProratedBillDataAccess
        {
            get
            {
                return (CustomerProratedBillDataAccess)_ClientContext[typeof(CustomerProratedBillDataAccess)];
            }
        }
        InvoiceNoteDataAccess _InvoiceNoteDataAccess
        {
            get
            {
                return (InvoiceNoteDataAccess)_ClientContext[typeof(InvoiceNoteDataAccess)];
            }
        }
        TransactionDataAccess _TransactionDataAccess
        {
            get
            {
                return (TransactionDataAccess)_ClientContext[typeof(TransactionDataAccess)];
            }
        }
        InvoiceDataAccess _InvoiceDataAccess
        {
            get
            {
                return (InvoiceDataAccess)_ClientContext[typeof(InvoiceDataAccess)];
            }
        }
        EmailTemplateDataAccess _EmailTemplateDataAccess
        {
            get
            {
                return (EmailTemplateDataAccess)_ClientContext[typeof(EmailTemplateDataAccess)];
            }
        }
        CustomerDataAccess _CustomerDataAccess
        {
            get
            {
                return (CustomerDataAccess)_ClientContext[typeof(CustomerDataAccess)];
            }
        }
        EstimateImageDataAccess _EstimateImageDataAccess
        {
            get
            {
                return (EstimateImageDataAccess)_ClientContext[typeof(EstimateImageDataAccess)];
            }
        }
        #endregion

        #region Estimate Reports

        public List<Invoice> GetAllEstimateByCompanyId(Guid CompanyId)
        {
            DataTable dt = _InvoiceDataAccess.GetAllEstimateByCompanyId(CompanyId, null, null);
            List<Invoice> InvoiceList = new List<Invoice>();
            InvoiceList = (from DataRow dr in dt.Rows
                           select new Invoice()
                           {
                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                               CreatedBy = dr["CreatedBy"].ToString(),
                               BillingAddress = dr["BillingAddress"].ToString(),
                               Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                               TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0,
                               CompanyId = (Guid)dr["CompanyId"],
                               CustomerId = (Guid)dr["CustomerId"],
                               CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.Now,
                               CustomerName = dr["Title"].ToString() + " " + dr["FirstName"].ToString() + " " + dr["LastName"].ToString(),
                               DiscountAmount = dr["DiscountAmount"] != DBNull.Value ? Convert.ToDouble(dr["DiscountAmount"]) : 0,
                               DiscountCode = dr["DiscountCode"].ToString(),
                               DueDate = dr["DueDate"] != DBNull.Value ? Convert.ToDateTime(dr["DueDate"]) : DateTime.Now,
                               InvoiceId = dr["InvoiceId"].ToString(),
                               IsEstimate = dr["IsEstimate"] != DBNull.Value ? Convert.ToBoolean(dr["IsEstimate"]) : false,
                               Status = dr["Status"].ToString(),
                               Tax = dr["DiscountAmount"] != DBNull.Value ? Convert.ToDouble(dr["DiscountAmount"]) : 0,
                           }).ToList();
            return InvoiceList;
        }

        public List<Invoice> GetAllEstimateByCompanyIdAndDates(Guid CompanyId, DateTime start, DateTime end)
        {
            DataTable dt = _InvoiceDataAccess.GetAllEstimateByCompanyId(CompanyId, start, end);
            List<Invoice> InvoiceList = new List<Invoice>();
            InvoiceList = (from DataRow dr in dt.Rows
                           select new Invoice()
                           {
                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                               CreatedBy = dr["CreatedBy"].ToString(),
                               BillingAddress = dr["BillingAddress"].ToString(),
                               Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                               TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0,
                               CompanyId = (Guid)dr["CompanyId"],
                               CustomerId = (Guid)dr["CustomerId"],
                               CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.Now,
                               CustomerName = dr["Title"].ToString() + " " + dr["FirstName"].ToString() + " " + dr["LastName"].ToString(),
                               DiscountAmount = dr["DiscountAmount"] != DBNull.Value ? Convert.ToDouble(dr["DiscountAmount"]) : 0,
                               DiscountCode = dr["DiscountCode"].ToString(),
                               DueDate = dr["DueDate"] != DBNull.Value ? Convert.ToDateTime(dr["DueDate"]) : DateTime.Now,
                               InvoiceId = dr["InvoiceId"].ToString(),
                               IsEstimate = dr["IsEstimate"] != DBNull.Value ? Convert.ToBoolean(dr["IsEstimate"]) : false,
                               Status = dr["Status"].ToString(),
                               Tax = dr["DiscountAmount"] != DBNull.Value ? Convert.ToDouble(dr["DiscountAmount"]) : 0,
                           }).ToList();
            return InvoiceList;
        }

        public EstimateReportModel GetAllEstimateSentByCompanyId(Guid CompanyId, DateTime start, DateTime end,string searchtext,string order, int pageno, int pagesize)
        {
            DataSet ds = _InvoiceDataAccess.GetAllEstimateSentByCompanyId(CompanyId, start, end, searchtext,order,pageno,pagesize);
            EstimateReportModel model = new EstimateReportModel();
            model.EstimateModelList = (from DataRow dr in ds.Tables[0].Rows
                                       select new EstimateModel()
                           { 
                               CustomerName = dr["CustomerName"].ToString(),
                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                               CustomerIntId = dr["CustomerIntId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerIntId"]) : 0,
                               EstimatorId = dr["EstimatorId"].ToString(),
                               Status = dr["Status"].ToString(),
                               SendDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : DateTime.Now, 
                           }).ToList();

            model.TotalCount = ds.Tables[1].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCount"]) : 0;
            return model;
        }
        #endregion


        public List<DashboardSalesAreaChart> GetDashboardSalesAreaChartData(Guid CompanyId, DateTime Start, DateTime End, string labelvalue, string tag, Guid empid)
        {
            DataTable dt = null;
            if (labelvalue == "invest")
            {
                dt = _InvoiceDataAccess.GetDashboardSalesAreaChartData(CompanyId, Start, End, labelvalue, tag, empid);
            }
            else
            {
                dt = _CustomerDataAccess.GetDashboardSalesAreaChartData(CompanyId, Start, End, labelvalue, tag, empid);
            }

            List<DashboardSalesAreaChart> DataList = new List<DashboardSalesAreaChart>();
            if (dt != null)
            {
                DataList = (from DataRow dr in dt.Rows
                            select new DashboardSalesAreaChart()
                            {
                                SaleDate = (dr["SaleDate"] != DBNull.Value ? Convert.ToDateTime(dr["SaleDate"]) : DateTime.Now),
                                SaleQuantity = dr["SaleQuantity"] != DBNull.Value ? Convert.ToInt32(dr["SaleQuantity"]) : 0,
                                TotalSaleAmount = dr["TotalSaleAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalSaleAmount"]) : 0.0
                            }).ToList();
            }
            return DataList;
        }

        #region InvoiceReport
        public List<Invoice> GetAllInvoiceByCompanyIdAndDates(Guid CompanyId, string start, string end, string Status, string Type)
        {
            DataTable dt = _InvoiceDataAccess.GetAllInvoiceByCompanyId(CompanyId, start, end, Status, Type);
            List<Invoice> InvoiceList = new List<Invoice>();
            InvoiceList = (from DataRow dr in dt.Rows
                           select new Invoice()
                           {
                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                               BalanceDue = dr["BalanceDue"] != DBNull.Value ? Convert.ToDouble(dr["BalanceDue"]) : 0,
                               CreatedBy = dr["CreatedBy"].ToString(),
                               BillingAddress = dr["BillingAddress"].ToString(),
                               Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                               TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0,
                               CompanyId = (Guid)dr["CompanyId"],
                               CustomerId = (Guid)dr["CustomerId"],
                               CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.Now,
                               CustomerName = dr["Title"].ToString() + " " + dr["FirstName"].ToString() + " " + dr["LastName"].ToString(),
                               DiscountAmount = dr["DiscountAmount"] != DBNull.Value ? Convert.ToDouble(dr["DiscountAmount"]) : 0,
                               DiscountCode = dr["DiscountCode"].ToString(),
                               DueDate = dr["DueDate"] != DBNull.Value ? Convert.ToDateTime(dr["DueDate"]) : DateTime.Now,
                               InvoiceId = dr["InvoiceId"].ToString(),
                               IsEstimate = dr["IsEstimate"] != DBNull.Value ? Convert.ToBoolean(dr["IsEstimate"]) : false,
                               Status = dr["Status"].ToString(),
                               Tax = dr["DiscountAmount"] != DBNull.Value ? Convert.ToDouble(dr["DiscountAmount"]) : 0,
                           }).ToList();
            return InvoiceList;
        }

        public List<Invoice> GetAllReceivePaymentsByCompanyId(Guid CompanyId)
        {
            //DataTable dt = _InvoiceDataAccess.GetAllInvoiceByCompanyId(CompanyId, start, end);
            DataTable dt = _InvoiceDataAccess.GetAllReceivePaymentsByCompanyId(CompanyId);
            List<Invoice> InvoiceList = new List<Invoice>();
            InvoiceList = (from DataRow dr in dt.Rows
                           select new Invoice()
                           {
                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                               BalanceDue = dr["BalanceDue"] != DBNull.Value ? Convert.ToDouble(dr["BalanceDue"]) : 0,
                               CreatedBy = dr["CreatedBy"].ToString(),
                               BillingAddress = dr["BillingAddress"].ToString(),
                               Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                               TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0,
                               CompanyId = (Guid)dr["CompanyId"],
                               CustomerId = (Guid)dr["CustomerId"],
                               CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.Now,
                               CustomerName = dr["Title"].ToString() + " " + dr["FirstName"].ToString() + " " + dr["LastName"].ToString(),
                               DiscountAmount = dr["DiscountAmount"] != DBNull.Value ? Convert.ToDouble(dr["DiscountAmount"]) : 0,
                               DiscountCode = dr["DiscountCode"].ToString(),
                               DueDate = dr["DueDate"] != DBNull.Value ? Convert.ToDateTime(dr["DueDate"]) : DateTime.Now,
                               InvoiceId = dr["InvoiceId"].ToString(),
                               IsEstimate = dr["IsEstimate"] != DBNull.Value ? Convert.ToBoolean(dr["IsEstimate"]) : false,
                               Status = dr["Status"].ToString(),
                               Tax = dr["DiscountAmount"] != DBNull.Value ? Convert.ToDouble(dr["DiscountAmount"]) : 0,
                           }).ToList();
            return InvoiceList;
        }

        public SalesARBInvoices GetAllARBInvoicesByfilter(AllInvoicesFilter filter,string BillicycleIdList,string InvoicestatusIdList)
        {
            SalesARBInvoices Model = new SalesARBInvoices();
            Model.InvStarDate = filter.StartDate;
            Model.InvEndDate = filter.EndDate;

            DataSet ds = _InvoiceDataAccess.GetAllARBInvoicesByfilter(filter, BillicycleIdList, InvoicestatusIdList);

            if (ds != null)
            {
                Model.InvoiceList = (from DataRow dr in ds.Tables[0].Rows
                                     select new ARBInvoiceModel()
                                     {
                                         Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                         BalanceDue = dr["BalanceDue"] != DBNull.Value ? Convert.ToDouble(dr["BalanceDue"]) : 0,
                                         InvId = dr["InvId"] != DBNull.Value ? Convert.ToInt32(dr["InvId"]) : 0,
                                         TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0,
                                         CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                         CustomerName = dr["CustomerName"].ToString(),
                                         InvoiceId = dr["InvoiceId"].ToString(),
                                         Status = dr["Status"].ToString(),
                                         BusinessName = dr["BusinessName"].ToString(),
                                         BillingCycle = dr["BillingCycle"].ToString(),
                                         AuthorizeRefId = dr["AuthorizeRefId"].ToString(),
                                         Description = dr["Description"].ToString(),
                                         TransactionId = dr["TransactionId"].ToString(),
                                         InvoiceDate = dr["InvoiceDate"] != DBNull.Value ? Convert.ToDateTime(dr["InvoiceDate"]) : new DateTime(),
                                         CustomerId = (Guid)dr["CustomerId"]
                                     }).ToList();
                Model.TotalAmountByPage = ds.Tables[1].Rows[0]["TotalAmountByPage"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["TotalAmountByPage"]) : 0;


                Model.Summary = (from DataRow dr in ds.Tables[2].Rows
                                 select new ARBSummary()
                                 {
                                     InActiveCustomer = dr["InActiveCustomer"] != DBNull.Value ? Convert.ToInt32(dr["InActiveCustomer"]) : 0,
                                     InActiveCustomerMMR = dr["InActiveCustomerMMR"] != DBNull.Value ? Convert.ToDouble(dr["InActiveCustomerMMR"]) : 0,
                                     MonthlyCustomer = dr["MonthlyCustomer"] != DBNull.Value ? Convert.ToInt32(dr["MonthlyCustomer"]) : 0,
                                     MonthlyMMR = dr["MonthlyMMR"] != DBNull.Value ? Convert.ToDouble(dr["MonthlyMMR"]) : 0,
                                     TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0,
                                     TotalCustomer = dr["TotalCustomer"] != DBNull.Value ? Convert.ToInt32(dr["TotalCustomer"]) : 0,
                                     TotalMMR = dr["TotalMMR"] != DBNull.Value ? Convert.ToDouble(dr["TotalMMR"]) : 0,
                                 }).ToList().FirstOrDefault();

            }


            #region SearchFilters
            Model.PageNo = filter.PageNo;
            Model.PageSize = filter.PageSize;
            Model.SearchBy = filter.SearchBy;
            Model.SearchText = filter.SearchText;
            Model.SortBy = filter.SortBy;
            Model.SortOrder = filter.SortOrder;
            Model.InvoiceFor = filter.InvoiceFor;
            Model.BillingCycle = filter.BillingCycle;


            #endregion
            return Model;

        }

        public SalesARBInvoices GetAllInvoicesByfilter(AllInvoicesFilter filter,string BillicycleIdList, string InvoicestatusIdList)
        {
            SalesARBInvoices Model = new SalesARBInvoices();
            Model.InvStarDate = filter.StartDate;
            Model.InvEndDate = filter.EndDate;

            DataSet ds = _InvoiceDataAccess.GetAllInvoicesByfilter(filter, BillicycleIdList, InvoicestatusIdList);

            if (ds != null)
            {
                Model.InvoiceList = (from DataRow dr in ds.Tables[0].Rows
                                     select new ARBInvoiceModel()
                                     {
                                         Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                         BalanceDue = dr["BalanceDue"] != DBNull.Value ? Convert.ToDouble(dr["BalanceDue"]) : 0,
                                  

                                         //BillingAddress = dr["BillingAddress"].ToString(), 
                                         TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0,
                                         CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                         CustomerName = dr["CustomerName"].ToString(),
                                         InvoiceId = dr["InvoiceId"].ToString(),
                                         Status = dr["Status"].ToString(),
                                         BusinessName = dr["BusinessName"].ToString(),
                                         BillingCycle = dr["BillingCycle"].ToString(),
                                         AuthorizeRefId = dr["AuthorizeRefId"].ToString(),
                                         Description = dr["Description"].ToString(),
                                         TransactionId = dr["TransactionId"].ToString(),
                                         InvoiceDate = dr["InvoiceDate"] != DBNull.Value ? Convert.ToDateTime(dr["InvoiceDate"]) : new DateTime(),
                                         CustomerIntId = dr["CustomerIntId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerIntId"]) : 0,
                                     }).ToList();
                Model.TotalAmountByPage = ds.Tables[1].Rows[0]["TotalAmountByPage"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["TotalAmountByPage"]) : 0;

                Model.Summary = (from DataRow dr in ds.Tables[2].Rows
                                 select new ARBSummary()
                                 {
                                     InActiveCustomer = dr["InActiveCustomer"] != DBNull.Value ? Convert.ToInt32(dr["InActiveCustomer"]) : 0,
                                     InActiveCustomerMMR = dr["InActiveCustomerMMR"] != DBNull.Value ? Convert.ToDouble(dr["InActiveCustomerMMR"]) : 0,
                                     MonthlyCustomer = dr["MonthlyCustomer"] != DBNull.Value ? Convert.ToInt32(dr["MonthlyCustomer"]) : 0,
                                     MonthlyMMR = dr["MonthlyMMR"] != DBNull.Value ? Convert.ToDouble(dr["MonthlyMMR"]) : 0,
                                     TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0,
                                     TotalCustomer = dr["TotalCustomer"] != DBNull.Value ? Convert.ToInt32(dr["TotalCustomer"]) : 0,
                                     TotalMMR = dr["TotalMMR"] != DBNull.Value ? Convert.ToDouble(dr["TotalMMR"]) : 0,
                                     InvoiceIdList = dr["InvoiceIdList"].ToString()
                                 }).ToList().FirstOrDefault();
            }


            #region SearchFilters
            Model.PageNo = filter.PageNo;
            Model.PageSize = filter.PageSize;
            Model.SearchBy = filter.SearchBy;
            Model.SearchText = filter.SearchText;
            Model.SortBy = filter.SortBy;
            Model.SortOrder = filter.SortOrder;
            Model.InvoiceFor = filter.InvoiceFor;
            Model.BillingCycle = filter.BillingCycle;


            #endregion
            return Model;

        }

        public SalesARBInvoices DownLoadAllInvoicesByfilter(AllInvoicesFilter filter,string BillicycleIdList,string InvoicestatusIdList)
        {
            SalesARBInvoices Model = new SalesARBInvoices();
            Model.InvStarDate = filter.StartDate;
            Model.InvEndDate = filter.EndDate;

            DataSet ds = _InvoiceDataAccess.GetAllInvoicesByfilter(filter, BillicycleIdList, InvoicestatusIdList);

            if (ds != null)
            {
                Model.ARBInvoiceDownLoadModelList= (from DataRow dr in ds.Tables[0].Rows
                                     select new ARBInvoiceDownLoadModel()
                                     {
                                         CustomerId = dr["CustomerIntId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerIntId"]) : 0,
                                         CustomerName = dr["CustomerName"].ToString(),
                                         InvoiceId = dr["InvoiceId"].ToString(),
                                         Billed_Amount = dr["TotalAmount"] != DBNull.Value ? Math.Round(Convert.ToDouble(dr["TotalAmount"]), 2)  : 0,
                                         Description = dr["Description"].ToString(),
                                         SettlementDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]).Date : new DateTime().Date,
                                         TransactionId = dr["TransactionId"].ToString(),
                                         Status = dr["Status"].ToString(),

                                         //Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                         //BalanceDue = dr["BalanceDue"] != DBNull.Value ? Convert.ToDouble(dr["BalanceDue"]) : 0,
                                         ////BillingAddress = dr["BillingAddress"].ToString(), 





                                         //BusinessName = dr["BusinessName"].ToString(),
                                         //BillingCycle = dr["BillingCycle"].ToString(),
                                         //AuthorizeRefId = dr["AuthorizeRefId"].ToString(),


                                         //InvoiceDate = dr["InvoiceDate"] != DBNull.Value ? Convert.ToDateTime(dr["InvoiceDate"]) : new DateTime(),

                                     }).ToList();
                //Model.Summary = (from DataRow dr in ds.Tables[1].Rows
                //                 select new ARBSummary()
                //                 {
                //                     InActiveCustomer = dr["InActiveCustomer"] != DBNull.Value ? Convert.ToInt32(dr["InActiveCustomer"]) : 0,
                //                     InActiveCustomerMMR = dr["InActiveCustomerMMR"] != DBNull.Value ? Convert.ToDouble(dr["InActiveCustomerMMR"]) : 0,
                //                     MonthlyCustomer = dr["MonthlyCustomer"] != DBNull.Value ? Convert.ToInt32(dr["MonthlyCustomer"]) : 0,
                //                     MonthlyMMR = dr["MonthlyMMR"] != DBNull.Value ? Convert.ToDouble(dr["MonthlyMMR"]) : 0,
                //                     TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0,
                //                     TotalCustomer = dr["TotalCustomer"] != DBNull.Value ? Convert.ToInt32(dr["TotalCustomer"]) : 0,
                //                     TotalMMR = dr["TotalMMR"] != DBNull.Value ? Convert.ToDouble(dr["TotalMMR"]) : 0,
                //                     InvoiceIdList = dr["InvoiceIdList"].ToString()
                //                 }).ToList().FirstOrDefault();
            }


            #region SearchFilters
            //Model.PageNo = filter.PageNo;
            //Model.PageSize = filter.PageSize;
            //Model.SearchBy = filter.SearchBy;
            //Model.SearchText = filter.SearchText;
            //Model.SortBy = filter.SortBy;
            //Model.SortOrder = filter.SortOrder;
            //Model.InvoiceFor = filter.InvoiceFor;
            //Model.BillingCycle = filter.BillingCycle;


            #endregion
            return Model;

        }


        public List<Invoice> GetAllInvoiceByCompanyId(Guid CompanyId)
        {
            //return _InvoiceDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and IsEstimate = 0  and Status != 'init'", CompanyId));

            DataTable dt = _InvoiceDataAccess.GetAllInvoiceByCompanyId(CompanyId, null, null, null, null);
            List<Invoice> InvoiceList = new List<Invoice>();
            InvoiceList = (from DataRow dr in dt.Rows
                           select new Invoice()
                           {
                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                               BalanceDue = dr["BalanceDue"] != DBNull.Value ? Convert.ToDouble(dr["BalanceDue"]) : 0,
                               CreatedBy = dr["CreatedBy"].ToString(),
                               BillingAddress = dr["BillingAddress"].ToString(),
                               Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                               TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0,
                               CompanyId = (Guid)dr["CompanyId"],
                               CustomerId = (Guid)dr["CustomerId"],
                               CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.Now,
                               CustomerName = dr["Title"].ToString() + " " + dr["FirstName"].ToString() + " " + dr["LastName"].ToString(),
                               DiscountAmount = dr["DiscountAmount"] != DBNull.Value ? Convert.ToDouble(dr["DiscountAmount"]) : 0,
                               DiscountCode = dr["DiscountCode"].ToString(),
                               DueDate = dr["DueDate"] != DBNull.Value ? Convert.ToDateTime(dr["DueDate"]) : DateTime.Now,
                               InvoiceId = dr["InvoiceId"].ToString(),
                               IsEstimate = dr["IsEstimate"] != DBNull.Value ? Convert.ToBoolean(dr["IsEstimate"]) : false,
                               Status = dr["Status"].ToString(),
                               Tax = dr["DiscountAmount"] != DBNull.Value ? Convert.ToDouble(dr["DiscountAmount"]) : 0,
                           }).ToList();
            return InvoiceList;
        }

        public List<TotalCustomerCount> GetTotalInvoiceAmountByCompanyId(AllInvoicesFilter filter)
        {
            List<TotalCustomerCount> customerCount = new List<TotalCustomerCount>();


            DataTable ds = _InvoiceDataAccess.GetTotalInvoiceByCompanyId(filter);


            customerCount = (from DataRow dr in ds.Rows
                             select new TotalCustomerCount()
                             {
                                 Counter = dr["TotalCustomer"] != DBNull.Value ? Convert.ToInt32(dr["TotalCustomer"]) : 0,
                                 TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToInt32(dr["TotalAmount"]) : 0

                             }).ToList();


            #endregion
            return customerCount;
        }

        public AllInvoicesModel GetAllInvoiceByCompanyIdAndFilter(AllInvoicesFilter filter)
        {
            AllInvoicesModel Model = new AllInvoicesModel();
            Model.InvoiceList = new List<Invoice>();
            #region SearchFilters
            if (string.IsNullOrWhiteSpace(filter.BillingCycle))
            {
                filter.BillingCycle = "-1";
            }
            if (!filter.BillingDay.HasValue || filter.BillingDay.Value > 28 || filter.BillingDay.Value == 0 || filter.BillingDay.Value < -1)
            {
                filter.BillingDay = -1;
            }
            if (string.IsNullOrWhiteSpace(filter.IsTax))
            {
                filter.IsTax = "-1";
            }
            if (string.IsNullOrWhiteSpace(filter.PaymentFilter))
            {
                filter.PaymentFilter = "-1";
            }
            #endregion
            DataSet ds = _InvoiceDataAccess.GetAllInvoiceByCompanyIdAndFilter(filter);
            /**
             * had to use invoice entity to carry customer table data.
             * because at first we created this for invoice list, requirement changes if we change the model, 
             * a lot of changes needs to be done. 
             * so we are still using this.
             * **/
            if (ds != null)
                Model.InvoiceList = (from DataRow dr in ds.Tables[0].Rows
                                     select new Invoice()
                                     {
                                         Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                         BalanceDue = dr["BalanceDue"] != DBNull.Value ? Convert.ToDouble(dr["BalanceDue"]) : 0,
                                         CreatedBy = dr["CreatedBy"].ToString(),
                                         //BillingAddress = dr["BillingAddress"].ToString(),
                                         Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                                         TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0,
                                         CompanyId = (Guid)dr["CompanyId"],
                                         CustomerId = (Guid)dr["CustomerId"],
                                         CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                         CustomerName = dr["Title"].ToString() + " " + dr["FirstName"].ToString() + " " + dr["LastName"].ToString(),
                                         CustomerNo = dr["CustomerNo"].ToString(),
                                         DiscountAmount = dr["DiscountAmount"] != DBNull.Value ? Convert.ToDouble(dr["DiscountAmount"]) : 0,
                                         DiscountCode = dr["DiscountCode"].ToString(),
                                         DueDate = dr["DueDate"] != DBNull.Value ? Convert.ToDateTime(dr["DueDate"]) : new DateTime(),
                                         InvoiceId = dr["InvoiceId"].ToString(),
                                         IsEstimate = dr["IsEstimate"] != DBNull.Value ? Convert.ToBoolean(dr["IsEstimate"]) : false,
                                         Status = dr["Status"].ToString(),
                                         Tax = dr["DiscountAmount"] != DBNull.Value ? Convert.ToDouble(dr["DiscountAmount"]) : 0,
                                         AuthRefId = dr["AuthRefId"].ToString(),
                                         CustomerBussinessName = dr["CustomerBussinessName"].ToString(),
                                         BillingCycle = dr["BillingCycle"].ToString(),
                                         LateFee = dr["LateFee"] != DBNull.Value ? Convert.ToDouble(dr["LateFee"]) : 0,
                                         LateAmount = dr["LateAmount"] != DBNull.Value ? Convert.ToDouble(dr["LateAmount"]) : 0,
                                         InstallDate = dr["FirstBilling"] != DBNull.Value ? Convert.ToDateTime(dr["FirstBilling"]) : new DateTime(),//will be used as FirstBilling(Customer) day
                                         DiscountType = dr["BillDay"].ToString(),//will be used as Bill day (Customer)
                                         PastDueAmount = dr["PastDueAmount"] != DBNull.Value ? Convert.ToDouble(dr["PastDueAmount"]) : 0,
                                         CurrentBilledAmount = dr["CurrentBilledAmount"] != DBNull.Value ? Convert.ToDouble(dr["CurrentBilledAmount"]) : 0,
                                         CustomerCreatedDate = dr["CustomerCreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CustomerCreatedDate"]) : new DateTime(),
                                         ReturnedDate = dr["ReturnedDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReturnedDate"]) : new DateTime()
                                     }).ToList();



            Model.TotalAmount = ds != null && ds.Tables[1].Rows[0]["TotalAmount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalAmount"]) : 0;
            Model.TotalBalance = ds != null && ds.Tables[1].Rows[0]["TotalBalance"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalBalance"]) : 0;
            Model.TotalCount = ds != null && ds.Tables[1].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCount"]) : 0;

            Model.TotalCustomerAll = ds != null && ds.Tables[1].Rows[0]["TotalCustomerAll"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCustomerAll"]) : 0;
            Model.TotalAmountAll = ds != null && ds.Tables[1].Rows[0]["TotalAmountAll"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["TotalAmountAll"]) : 0;
            Model.MonthlyCustomer = ds != null && ds.Tables[1].Rows[0]["MonthlyCustomer"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["MonthlyCustomer"]) : 0;
            Model.MonthlyAmount = ds != null && ds.Tables[1].Rows[0]["MonthlyAmount"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["MonthlyAmount"]) : 0;
            Model.ReturnedCustomer = ds != null && ds.Tables[1].Rows[0]["ReturnedCustomer"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["ReturnedCustomer"]) : 0;
            Model.ReturnedCustomerAmount = ds != null && ds.Tables[1].Rows[0]["ReturnedCustomerAmount"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["ReturnedCustomerAmount"]) : 0;

            #region SearchFilters

            Model.BillingCycle = filter.BillingCycle;
            Model.BillingDay = filter.BillingDay.Value;
            Model.IsTax = filter.IsTax;
            Model.PaymentFilter = filter.PaymentFilter;
            Model.InvoiceFor = filter.InvoiceFor;
            Model.Status = filter.Status;
            #endregion
            return Model;
        }

        public AllInvoicesModel GetAllInvoiceByCompanyIdForReport(AllInvoicesFilter filter)
        {
            AllInvoicesModel Model = new AllInvoicesModel();

            DataSet ds = _InvoiceDataAccess.GetAllInvoiceByCompanyIdForReport(filter);

            Model.InvoiceList = (from DataRow dr in ds.Tables[0].Rows
                                 select new Invoice()
                                 {
                                     Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                     BalanceDue = dr["BalanceDue"] != DBNull.Value ? Convert.ToDouble(dr["BalanceDue"]) : 0,
                                     CreatedBy = dr["CreatedBy"].ToString(),
                                     //BillingAddress = dr["BillingAddress"].ToString(),
                                     Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                                     TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0,
                                     CompanyId = (Guid)dr["CompanyId"],
                                     CustomerId = (Guid)dr["CustomerId"],
                                     CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                     CustomerName = dr["Title"].ToString() + " " + dr["FirstName"].ToString() + " " + dr["LastName"].ToString(),
                                     DiscountAmount = dr["DiscountAmount"] != DBNull.Value ? Convert.ToDouble(dr["DiscountAmount"]) : 0,
                                     DiscountCode = dr["DiscountCode"].ToString(),
                                     DueDate = dr["DueDate"] != DBNull.Value ? Convert.ToDateTime(dr["DueDate"]) : new DateTime(),
                                     InvoiceId = dr["InvoiceId"].ToString(),
                                     IsEstimate = dr["IsEstimate"] != DBNull.Value ? Convert.ToBoolean(dr["IsEstimate"]) : false,
                                     Status = dr["Status"].ToString(),
                                     Tax = dr["DiscountAmount"] != DBNull.Value ? Convert.ToDouble(dr["DiscountAmount"]) : 0,
                                     AuthRefId = dr["AuthRefId"].ToString(),
                                     CustomerBussinessName = dr["CustomerBussinessName"].ToString(),
                                     BillingCycle = dr["BillingCycle"].ToString(),
                                     LateFee = dr["LateFee"] != DBNull.Value ? Convert.ToDouble(dr["LateFee"]) : 0,
                                     LateAmount = dr["LateAmount"] != DBNull.Value ? Convert.ToDouble(dr["LateAmount"]) : 0,
                                     InstallDate = dr["FirstBilling"] != DBNull.Value ? Convert.ToDateTime(dr["FirstBilling"]) : new DateTime(),//will be used as FirstBilling(Customer) day
                                     DiscountType = dr["BillDay"].ToString(),//will be used as Bill day (Customer)
                                 }).ToList();
            return Model;
        }

        public Invoice GetInvoiceByInvoiceId(string invoiceId)
        {
            return _InvoiceDataAccess.GetByQuery(string.Format("InvoiceId='{0}'", invoiceId)).FirstOrDefault();
        }

        public DataTable GetAllInvoiceReportByCompanyId(Guid CompanyId, string Start, string End, string Status, string Type)
        {
            return _InvoiceDataAccess.GetAllInvoiceReportByCompanyId(CompanyId, Start, End, Status, Type);
        }



        public AllSalesInfoModel GetAllReceivePaymentsByCompanyIdAndFilter(Guid CompanyId, int PageNo, int PageSize, string SearchBy, string SearchText, string order,DateTime StartDate,DateTime EndDate)
        {
            AllSalesInfoModel Model = new AllSalesInfoModel();

            DataSet ds = _InvoiceDataAccess.GetAllReceivePaymentsByCompanyIdAndFilter(CompanyId, PageNo, PageSize, SearchBy, SearchText, order,StartDate,EndDate);

            Model.TransactionList = (from DataRow dr in ds.Tables[0].Rows
                                     select new Transaction()
                                     {
                                         Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                         Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                                         Status = dr["Status"].ToString(),
                                         Type = dr["Type"].ToString(),
                                         Balance = dr["Balance"] != DBNull.Value ? Convert.ToDouble(dr["Balance"]) : 0,
                                         CustomerId = (Guid)dr["CustomerId"],
                                         CustomerName = dr["FirstName"].ToString() + " " + dr["LastName"].ToString(),
                                         CompanyId = (Guid)dr["CompanyId"],
                                         InvoiceId = dr["InvoiceId"] != DBNull.Value ? Convert.ToInt32(dr["InvoiceId"]) : 0,
                                         TransacationDate = dr["TransacationDate"] != DBNull.Value ? Convert.ToDateTime(dr["TransacationDate"]) : new DateTime(),
                                         InvoiceDueDate = dr["InvoiceDueDate"] != DBNull.Value ? Convert.ToDateTime(dr["InvoiceDueDate"]) : new DateTime(),
                                         InvoiceIdStr = dr["InvoiceIdStr"].ToString(),
                                         CustomerBussinessName = dr["CustomerBussinessName"].ToString(),
                                         CustomerIdValue = dr["CustomerIdValue"] != DBNull.Value ? Convert.ToInt32(dr["CustomerIdValue"]) : 0,
                            

                                     }).ToList();
   
            Model.TotalAmountByPage = ds.Tables[1].Rows[0]["TotalAmountByPage"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["TotalAmountByPage"]) : 0;
            Model.TotalBalanceByPage = ds.Tables[1].Rows[0]["TotalBalanceByPage"] != DBNull.Value ? Convert.ToDouble(ds.Tables[1].Rows[0]["TotalBalanceByPage"]) : 0;
            Model.TotalAmount = ds.Tables[2].Rows[0]["TotalAmount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[0]["TotalAmount"]) : 0;
            Model.TotalBalance = ds.Tables[2].Rows[0]["TotalBalance"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[0]["TotalBalance"]) : 0;
            Model.TotalCount = ds.Tables[2].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[0]["TotalCount"]) : 0;
            Model.CustomerCount = ds.Tables[2].Rows[0]["CustomerCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[0]["CustomerCount"]) : 0;
            return Model;
        }
        public DataTable DownloadAllReceivePaymentsByCompanyIdAndFilter(Guid CompanyId, int? PageNo, int PageSize, string SearchBy, string SearchText, string order, DateTime StartDate, DateTime EndDate)
        {
            return _InvoiceDataAccess.DownloadAllReceivePaymentsByCompanyIdAndFilter(CompanyId, PageNo, PageSize, SearchBy, SearchText, order, StartDate, EndDate);
        }

        public DataTable GetAllEstimateReportByCompanyId(Guid CompanyId, DateTime? Start, DateTime? End)
        {
            return _InvoiceDataAccess.GetAllEstimateReportByCompanyId(CompanyId, Start, End);
        }
        public DataTable GetAllExportEstimateSentByCompanyId(Guid CompanyId, DateTime? Start, DateTime? End,string searchtext,string order)
        {
            return _InvoiceDataAccess.GetAllExportEstimateSentByCompanyId(CompanyId, Start, End,searchtext,order);
        }
        public List<Invoice> RabDataMigrationNewInvoiceList()
        {
            return _InvoiceDataAccess.RabDataMigrationNewInvoiceList();
        }

        public List<InvoiceDetail> GetInvoiceDetailsRabMigration(string invoiceId)
        {
            return _InvoiceDetailDataAccess.GetByQuery(string.Format(" InvoiceId ='{0}' and CreatedBy <> 'automated' and id > 7305", invoiceId));
        }
        public List<Invoice> GetAllInvoiceByCompanyIdAndCustomerId(Guid CompanyId, Guid CustomerId, string InvoiceType, CustomerFilter filter, bool IsDeclined)
        {//only for invoicecontroller - > InvoicePartial
            DataTable dt = _InvoiceDataAccess.GetAllInvoiceByCompanyIdAndCustomerId(CompanyId, CustomerId, InvoiceType, filter, IsDeclined);
            List<Invoice> InvoiceList = new List<Invoice>();
            InvoiceList = (from DataRow dr in dt.Rows
                           select new Invoice()
                           {
                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                               BalanceDue = dr["BalanceDue"] != DBNull.Value ? Convert.ToDouble(dr["BalanceDue"]) : 0,
                               CreatedBy = dr["CreatedBy"].ToString(),
                               BillingAddress = dr["BillingAddress"].ToString(),
                               Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                               TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0,
                               CompanyId = (Guid)dr["CompanyId"],
                               CustomerId = (Guid)dr["CustomerId"],
                               CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.Now,
                               //CustomerName = dr["Title"].ToString() + " " + dr["FirstName"].ToString() + " " + dr["LastName"].ToString(),
                               DiscountAmount = dr["DiscountAmount"] != DBNull.Value ? Convert.ToDouble(dr["DiscountAmount"]) : 0,
                               DiscountCode = dr["DiscountCode"].ToString(),
                               DueDate = dr["DueDate"] != DBNull.Value ? Convert.ToDateTime(dr["DueDate"]) : DateTime.Now,
                               InvoiceId = dr["InvoiceId"].ToString(),
                               IsEstimate = dr["IsEstimate"] != DBNull.Value ? Convert.ToBoolean(dr["IsEstimate"]) : false,
                               Status = dr["Status"].ToString(),
                               Tax = dr["DiscountAmount"] != DBNull.Value ? Convert.ToDouble(dr["DiscountAmount"]) : 0,
                               InvoiceNoteAddedDate = dr["InvoiceNoteAddedDate"] != DBNull.Value ? Convert.ToDateTime(dr["InvoiceNoteAddedDate"]) : new DateTime(),
                               InvoiceEquipDes = dr["InvoiceEquipDes"].ToString(),
                               CustomerMailAddress = dr["CustomerMailAddress"].ToString(),
                               CustomerViewedTime = dr["CustomerViewedTime"] != DBNull.Value ? Convert.ToDateTime(dr["CustomerViewedTime"]) : new DateTime(),
                               Description = dr["Description"].ToString(),
                               NotesInvoice = dr["NotesInvoice"].ToString(),
                               NoteInvoiceAddedBy = dr["NoteInvoiceAddedBy"].ToString(),
                               CustomerViewedType = dr["CustomerViewedType"].ToString(),
                               UserNum = dr["UserNum"].ToString(),
                               RefType = dr["RefType"].ToString(),
                               IsARBInvoice = dr["IsARBInvoice"] != DBNull.Value ? Convert.ToBoolean(dr["IsARBInvoice"]) : false,
                               AgingDate = dr["AgingDate"] != DBNull.Value ? Convert.ToInt32(dr["AgingDate"]) : 0,
                               InvoiceDate = dr["InvoiceDate"] != DBNull.Value ? Convert.ToDateTime(dr["InvoiceDate"]) : new DateTime()
                           }).ToList();
            return InvoiceList;
        }
        public List<Invoice> GetAllInvoice1ByCompanyIdAndCustomerId(Guid CompanyId, Guid CustomerId, string InvoiceType, bool IsDeclined)
        {//only for invoicecontroller - > InvoicePartial
            DataTable dt = _InvoiceDataAccess.GetAllInvoice1ByCompanyIdAndCustomerId(CompanyId, CustomerId, InvoiceType, IsDeclined);
            List<Invoice> InvoiceList = new List<Invoice>();
            InvoiceList = (from DataRow dr in dt.Rows
                           select new Invoice()
                           {
                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                               BalanceDue = dr["BalanceDue"] != DBNull.Value ? Convert.ToDouble(dr["BalanceDue"]) : 0,
                               CreatedBy = dr["CreatedBy"].ToString(),
                               BillingAddress = dr["BillingAddress"].ToString(),
                               Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                               TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0,
                               CompanyId = (Guid)dr["CompanyId"],
                               CustomerId = (Guid)dr["CustomerId"],
                               CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.Now,
                               //CustomerName = dr["Title"].ToString() + " " + dr["FirstName"].ToString() + " " + dr["LastName"].ToString(),
                               DiscountAmount = dr["DiscountAmount"] != DBNull.Value ? Convert.ToDouble(dr["DiscountAmount"]) : 0,
                               DiscountCode = dr["DiscountCode"].ToString(),
                               DueDate = dr["DueDate"] != DBNull.Value ? Convert.ToDateTime(dr["DueDate"]) : DateTime.Now,
                               InvoiceId = dr["InvoiceId"].ToString(),
                               IsEstimate = dr["IsEstimate"] != DBNull.Value ? Convert.ToBoolean(dr["IsEstimate"]) : false,
                               Status = dr["Status"].ToString(),
                               Tax = dr["DiscountAmount"] != DBNull.Value ? Convert.ToDouble(dr["DiscountAmount"]) : 0,
                               InvoiceNoteAddedDate = dr["InvoiceNoteAddedDate"] != DBNull.Value ? Convert.ToDateTime(dr["InvoiceNoteAddedDate"]) : new DateTime(),
                               InvoiceEquipDes = dr["InvoiceEquipDes"].ToString(),
                               CustomerMailAddress = dr["CustomerMailAddress"].ToString(),
                               CustomerViewedTime = dr["CustomerViewedTime"] != DBNull.Value ? Convert.ToDateTime(dr["CustomerViewedTime"]) : new DateTime(),
                               Description = dr["Description"].ToString(),
                               NotesInvoice = dr["NotesInvoice"].ToString(),
                               NoteInvoiceAddedBy = dr["NoteInvoiceAddedBy"].ToString(),
                               RefType = dr["RefType"].ToString()
                           }).ToList();
            return InvoiceList;
        }
        public List<Invoice> GetAllEstimateByCompanyIdAndCustomerId(Guid CompanyId, Guid CustomerId)
        {
            return _InvoiceDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and CustomerId='{1}'  and IsEstimate = 1", CompanyId, CustomerId)).ToList();
        }
        public List<InvoiceDetail> GetInvoiceDetialsListByInvoiceId(string invoiceId)
        {
            DataTable dt = _InvoiceDetailDataAccess.GetInvoiceDetialsListByInvoiceId(invoiceId);
            List<InvoiceDetail> InvoiceDetailList = new List<InvoiceDetail>();
            InvoiceDetailList = (from DataRow dr in dt.Rows
                                 select new InvoiceDetail()
                                 {
                                     Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                     EquipmentId = (Guid)dr["EquipmentId"],
                                     EquipmentClassId = dr["EquipmentClassId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentClassId"]) : 0,
                                     //EquipmentName =  string.IsNullOrWhiteSpace(dr["EquipName"].ToString())?dr["EquipmentName"].ToString(): dr["EquipName"].ToString(),
                                     //EquipmentDescription = dr["EquipmentDescription"].ToString(),
                                     BundleId = dr["BundleId"] != DBNull.Value ? Convert.ToInt32(dr["BundleId"]) : 0,
                                     CompanyId = (Guid)dr["CompanyId"],
                                     CreatedBy = dr["CreatedBy"].ToString(),
                                     CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                     InventoryId = (Guid)dr["InventoryId"],
                                     InvoiceId = invoiceId,
                                     EquipName = dr["EquipName"].ToString(),
                                     EquipDetail = dr["EquipDetail"].ToString(),
                                     Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                     TotalPrice = dr["TotalPrice"] != DBNull.Value ? Convert.ToDouble(dr["TotalPrice"]) : 0,
                                     VendorPrice = dr["VendorPrice"] != DBNull.Value ? Convert.ToDouble(dr["VendorPrice"]) : 0,
                                     TotalRetail = dr["TotalRetail"] != DBNull.Value ? Convert.ToDouble(dr["TotalRetail"]) : 0,
                                     UnitPrice = dr["UnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["UnitPrice"]) : 0,
                                     DiscountAmount = dr["DiscountAmount"] != DBNull.Value ? Convert.ToDouble(dr["DiscountAmount"]) : 0,
                                     Taxable = dr["Taxable"] != DBNull.Value ? Convert.ToBoolean(dr["Taxable"]) : true,
                                     Equipmentvendorcost = dr["EquipmentVendorCost"] != DBNull.Value ? Convert.ToDouble(dr["EquipmentVendorCost"]) : 0,
                                     EquipCategory = dr["EquipCategory"].ToString()

                                 }).ToList();
            return InvoiceDetailList;


            //return _InvoiceDetailDataAccess.GetByQuery(string.Format("InvoiceId ='{0}'",invoiceId));
        }


        //public List<InvoiceNote> GetInvoiceNoteAddedDateByCompanyIdandCustomerIdandInvoiceId(Guid comid, Guid cusid, int id)
        //{
        //    DataTable dt = _InvoiceDataAccess.GetInvoiceNoteAddedDateByCompanyIdandCustomerIdandInvoiceId(comid, cusid, id);
        //    List<InvoiceNote> NoteList = new List<InvoiceNote>();
        //    NoteList = (from DataRow dr in dt.Rows
        //                select new InvoiceNote()
        //                {
        //                    AddedDate = dr["AddedDate"] != DBNull.Value ? Convert.ToDateTime(dr["AddedDate"]) : DateTime.Now,
        //                }).ToList();
        //    return NoteList;
        //}
        public CustomerDetailsTabCount GetCustomerDetailsTabCountsByCustomerId(Guid customerId, Guid companyid)
        {
            DataTable dt = _InvoiceDataAccess.GetallTabCountsDetailsByCustomerId(customerId, companyid);
            List<CustomerDetailsTabCount> CustomerTabCounts = new List<CustomerDetailsTabCount>();
            CustomerTabCounts = (from DataRow dr in dt.Rows
                                 select new CustomerDetailsTabCount()
                                 {
                                     OpenEstimateCount = dr["EstimateOpenCount"] != DBNull.Value ? Convert.ToInt32(dr["EstimateOpenCount"]) : 0,
                                     CompletedEstimateCount = dr["EstimateCompletedCount"] != DBNull.Value ? Convert.ToInt32(dr["EstimateCompletedCount"]) : 0,
                                     OpenInvoiceCount = dr["InvoiceOpenCount"] != DBNull.Value ? Convert.ToInt32(dr["InvoiceOpenCount"]) : 0,
                                     PaidInvoiceCount = dr["InvoicePaidCount"] != DBNull.Value ? Convert.ToInt32(dr["InvoicePaidCount"]) : 0,
                                     RolledOverInvoiceCount = dr["InvoiceRolledOverCount"] != DBNull.Value ? Convert.ToInt32(dr["InvoiceRolledOverCount"]) : 0,
                                     ActiveFilesCount = dr["FilesActiveCount"] != DBNull.Value ? Convert.ToInt32(dr["FilesActiveCount"]) : 0,
                                     InActiveFilesCount = dr["FilesInActiveCount"] != DBNull.Value ? Convert.ToInt32(dr["FilesInActiveCount"]) : 0,
                                     FundingCount = dr["FundingCount"] != DBNull.Value ? Convert.ToInt32(dr["FundingCount"]) : 0,
                                     ExpenseCount = dr["ExpenseCount"] != DBNull.Value ? Convert.ToInt32(dr["ExpenseCount"]) : 0,

                                 }).ToList();
            return CustomerTabCounts.FirstOrDefault();
        }
        public List<InvoiceDetail> GetAllInvoiceDetailsByCompanyId(Guid CompanyId)
        {
            return _InvoiceDetailDataAccess.GetByQuery(string.Format(" CompanyId = '{0}'", CompanyId));
        }
        public List<Invoice> GetAllEstimate()
        {
            return _InvoiceDataAccess.GetAll();
        }
        public Invoice GetById(int value)
        {
            return _InvoiceDataAccess.Get(value);
        }
        
        public List<Invoice> GetAllEstimateByInvoiceId(Guid CompanyId)
        {
            return _InvoiceDataAccess.GetByQuery(string.Format(" CompanyId = '{0}'", CompanyId));
        }
        public List<Invoice> GetAllEstimateByDescription(string Description)
        {
            return _InvoiceDataAccess.GetByQuery(string.Format(" Description = '{0}' OR Message ='{0}' OR Memo ='{0}'", Description)).ToList(); ;
        }
        public Invoice GetInvoiceById(int value)
        {
            return _InvoiceDataAccess.Get(value);
        }
        public bool UpdateNotes(Invoice invoice)
        {
            return _InvoiceDataAccess.Update(invoice) > 0;
        }

        public int InsertInvoice(Invoice invoice)
        {
            if (invoice.TotalAmount != null && invoice.TotalAmount > 0)
            {
                invoice.TotalAmount = Math.Round(invoice.TotalAmount.Value, 2);
            }
            if (invoice.BalanceDue != null && invoice.BalanceDue > 0)
            {
                invoice.BalanceDue = Math.Round(invoice.BalanceDue.Value, 2);
            }

            return (int)_InvoiceDataAccess.Insert(invoice);
        }

        public bool UpdateInvoice(Invoice invoice)
        {
            if(invoice.TotalAmount != null && invoice.TotalAmount >0)
            {
                invoice.TotalAmount = Math.Round(invoice.TotalAmount.Value, 2);
            }
            if (invoice.BalanceDue != null && invoice.BalanceDue > 0)
            {
                invoice.BalanceDue = Math.Round(invoice.BalanceDue.Value, 2);
            }

            return _InvoiceDataAccess.Update(invoice) > 0;
        }
        public bool UpdateInvoiceTicketIdByCustomerId(Guid TicketId, Guid CustomerId)
        {
            return _InvoiceDataAccess.UpdateInvoiceTicketIdByCustomerId(TicketId, CustomerId);
        }
        public bool UpdateInvoiceDetail(InvoiceDetail invoice)
        {
            return _InvoiceDetailDataAccess.Update(invoice) > 0;
        }
        
        public DataTable GetInvoiceReport(int[] IdList, string[] columnList, Guid CompanyId)
        {
            return _InvoiceDataAccess.GetInvoiceReport(IdList, columnList, CompanyId);
        }
        public DataTable GetTicketReport(Guid customerid, string[] columnList, Guid CompanyId)
        {
            return _InvoiceDataAccess.GetTicketReport(customerid, columnList, CompanyId);
        }
        public DataTable GetEstimateReport(int[] IdList, string[] columnList, Guid CompanyId)
        {
            return _InvoiceDataAccess.GetEstimateReport(IdList, columnList, CompanyId);
        }
        public long InsertNotes(Invoice invoice)
        {
            return _InvoiceDataAccess.Insert(invoice);
        }

        public List<EquipmentSearchModel> GetEqupmentListBySearchKeyAndCompanyId(string key, Guid CompanyId, int MaxLoad, string ExistEquipment)
        {
            DataTable dt = _InvoiceDataAccess.GetEqupmentListBySearchKeyAndCompanyId(key, CompanyId, MaxLoad, ExistEquipment);
            List<EquipmentSearchModel> NoteList = new List<EquipmentSearchModel>();
            NoteList = (from DataRow dr in dt.Rows
                        select new EquipmentSearchModel()
                        {
                            EquipmentId = (Guid)dr["EquipmentId"],
                            EquipmentName = dr["EquipmentName"].ToString(),
                            QuantityAvailable = dr["QuantityAvailable"] != DBNull.Value ? Convert.ToInt32(dr["QuantityAvailable"]) : 0,
                            Reorderpoint = dr["Reorderpoint"] != DBNull.Value ? Convert.ToInt32(dr["Reorderpoint"]) : 0,
                            RetailPrice = dr["RetailPrice"] != DBNull.Value ? Convert.ToDouble(dr["RetailPrice"]) : 0.0,
                            SupplierCost = dr["SupplierCost"] != DBNull.Value ? Convert.ToDouble(dr["SupplierCost"]) : 0.0,
                            EquipmentType = dr["EquipmentType"].ToString(),
                            EquipmentDescription = dr["EquipmentDescription"].ToString(),
                            SKU = dr["SKU"].ToString(),
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                            ManufacturerName = dr["ManufacturerName"].ToString(),
                            IsTaxable=dr["IsTaxable"] !=DBNull.Value ? Convert.ToBoolean(dr["IsTaxable"]):true,
                            Equipmentvendorcost = dr["EquipmentVendorCost"] != DBNull.Value ? Convert.ToDouble(dr["EquipmentVendorCost"]) : 0.0

                            //EquipmentTypeId = dr["EquipmentTypeId"] != DBNull.Value ? Convert.ToInt32(dr["EquipmentTypeId"]) : -1,
                            //SupplierId = dr["SupplierId"] != DBNull.Value ? Convert.ToInt32(dr["SupplierId"]) : -1,
                        }).ToList();
            return NoteList;
        }
        public List<EquipmentSearchModel> GetEqupmentListBySearchKeyAndCompanyIdTechnicianId(string key, Guid CompanyId, int MaxLoad, Guid technicianId, string ExistEquipment)
        {
            DataTable dt = _InvoiceDataAccess.GetEqupmentListBySearchKeyAndCompanyIdTechnicianId(key, CompanyId, MaxLoad, technicianId, ExistEquipment);
            List<EquipmentSearchModel> NoteList = new List<EquipmentSearchModel>();
            NoteList = (from DataRow dr in dt.Rows
                        select new EquipmentSearchModel()
                        {
                            EquipmentId = (Guid)dr["EquipmentId"],
                            EquipmentName = dr["EquipmentName"].ToString(),
                            QuantityAvailable = dr["QuantityAvailable"] != DBNull.Value ? Convert.ToInt32(dr["QuantityAvailable"]) : 0,
                            Reorderpoint = dr["Reorderpoint"] != DBNull.Value ? Convert.ToInt32(dr["Reorderpoint"]) : 0,
                            RetailPrice = dr["RetailPrice"] != DBNull.Value ? Convert.ToDouble(dr["RetailPrice"]) : 0.0,
                            SupplierCost = dr["SupplierCost"] != DBNull.Value ? Convert.ToDouble(dr["SupplierCost"]) : 0.0,
                            EquipmentType = dr["EquipmentType"].ToString(),
                            EquipmentDescription = dr["EquipmentDescription"].ToString(),
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                            QuantityOnHand = dr["QuantityOnHand"] != DBNull.Value ? Convert.ToInt32(dr["QuantityOnHand"]) : 0,
                            WareHouseQuantity = dr["WarehouseQTY"] != DBNull.Value ? Convert.ToInt32(dr["WarehouseQTY"]) : 0,
                            ManufacturerName = dr["ManufacturerName"].ToString(),
                            SKU = dr["SKU"].ToString(),
                            Barcode = dr["Barcode"].ToString(),
                            Point = dr["Point"] != DBNull.Value ? Convert.ToDouble(dr["Point"]) : 0.0,
                            Equipmentvendorcost = dr["EquipmentVendorCost"] != DBNull.Value ? Convert.ToDouble(dr["EquipmentVendorCost"]) : 0.0
                        }).ToList();
            return NoteList;
        }
        public List<EquipmentSearchModel> GetOnlyEqupmentListBySearchKeyAndCompanyId(string key, Guid CompanyId, int MaxLoad, string ExistEquipment)
        {
            DataTable dt = _InvoiceDataAccess.GetOnlyEqupmentListBySearchKeyAndCompanyId(key, CompanyId, MaxLoad, ExistEquipment);
            List<EquipmentSearchModel> NoteList = new List<EquipmentSearchModel>();
            NoteList = (from DataRow dr in dt.Rows
                        select new EquipmentSearchModel()
                        {
                            EquipmentId = (Guid)dr["EquipmentId"],
                            EquipmentName = dr["EquipmentName"].ToString(),
                            QuantityAvailable = dr["QuantityAvailable"] != DBNull.Value ? Convert.ToInt32(dr["QuantityAvailable"]) : 0,
                            Reorderpoint = dr["Reorderpoint"] != DBNull.Value ? Convert.ToInt32(dr["Reorderpoint"]) : 0,
                            RetailPrice = dr["RetailPrice"] != DBNull.Value ? Convert.ToDouble(dr["RetailPrice"]) : 0.0,
                            SupplierCost = dr["SupplierCost"] != DBNull.Value ? Convert.ToDouble(dr["SupplierCost"]) : 0.0,
                            EquipmentType = dr["EquipmentType"].ToString(),
                            EquipmentDescription = dr["EquipmentDescription"].ToString(),
                            SKU = dr["SKU"].ToString(),
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                            ManufacturerName = dr["ManufacturerName"].ToString()
                        }).ToList();
            return NoteList;
        }
        public List<EquipmentSearchModel> GetEqupmentListByTypeAndSearchKeyAndCompanyId(string key, Guid CompanyId, int MaxLoad, string ExistEquipment,string EqpTypeId)
        {
            DataTable dt = _InvoiceDataAccess.GetEqupmentListByTypeAndSearchKeyAndCompanyId(key, CompanyId, MaxLoad, ExistEquipment, EqpTypeId);
            List<EquipmentSearchModel> NoteList = new List<EquipmentSearchModel>();
            NoteList = (from DataRow dr in dt.Rows
                        select new EquipmentSearchModel()
                        {
                            EquipmentId = (Guid)dr["EquipmentId"],
                            EquipmentName = dr["EquipmentName"].ToString(),
                            QuantityAvailable = dr["QuantityAvailable"] != DBNull.Value ? Convert.ToInt32(dr["QuantityAvailable"]) : 0,
                            Reorderpoint = dr["Reorderpoint"] != DBNull.Value ? Convert.ToInt32(dr["Reorderpoint"]) : 0,
                            RetailPrice = dr["RetailPrice"] != DBNull.Value ? Convert.ToDouble(dr["RetailPrice"]) : 0.0,
                            SupplierCost = dr["SupplierCost"] != DBNull.Value ? Convert.ToDouble(dr["SupplierCost"]) : 0.0,
                            EquipmentType = dr["EquipmentType"].ToString(),
                            EquipmentDescription = dr["EquipmentDescription"].ToString(),
                            SKU = dr["SKU"].ToString(),
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                            ManufacturerName = dr["ManufacturerName"].ToString()
                        }).ToList();
            return NoteList;
        }
        public List<EquipmentSearchModel> GetOnlyEqupmentListByTypeAndSearchKeyAndCompanyId(string key, Guid CompanyId, int MaxLoad, string ExistEquipment, string EqpTypeId)
        {
            DataTable dt = _InvoiceDataAccess.GetOnlyEqupmentListByTypeAndSearchKeyAndCompanyId(key, CompanyId, MaxLoad, ExistEquipment, EqpTypeId);
            List<EquipmentSearchModel> NoteList = new List<EquipmentSearchModel>();
            NoteList = (from DataRow dr in dt.Rows
                        select new EquipmentSearchModel()
                        {
                            EquipmentId = (Guid)dr["EquipmentId"],
                            EquipmentName = dr["EquipmentName"].ToString(),
                            QuantityAvailable = dr["QuantityAvailable"] != DBNull.Value ? Convert.ToInt32(dr["QuantityAvailable"]) : 0,
                            Reorderpoint = dr["Reorderpoint"] != DBNull.Value ? Convert.ToInt32(dr["Reorderpoint"]) : 0,
                            RetailPrice = dr["RetailPrice"] != DBNull.Value ? Convert.ToDouble(dr["RetailPrice"]) : 0.0,
                            SupplierCost = dr["SupplierCost"] != DBNull.Value ? Convert.ToDouble(dr["SupplierCost"]) : 0.0,
                            EquipmentType = dr["EquipmentType"].ToString(),
                            EquipmentDescription = dr["EquipmentDescription"].ToString(),
                            SKU = dr["SKU"].ToString(),
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                            ManufacturerName = dr["ManufacturerName"].ToString()
                        }).ToList();
            return NoteList;
        }
        public List<EquipmentSearchModel> GetOnlyServiceListBySearchKeyAndCompanyId(string key, Guid CompanyId, int MaxLoad, string ExistEquipment, string ServiceEquipment = "")
        {
            DataTable dt = _InvoiceDataAccess.GetOnlyServiceListBySearchKeyAndCompanyId(key, CompanyId, MaxLoad, ExistEquipment,ServiceEquipment);
            List<EquipmentSearchModel> NoteList = new List<EquipmentSearchModel>();
            NoteList = (from DataRow dr in dt.Rows
                        select new EquipmentSearchModel()
                        {
                            EquipmentId = (Guid)dr["EquipmentId"],
                            EquipmentName = dr["EquipmentName"].ToString(),
                            QuantityAvailable = dr["QuantityAvailable"] != DBNull.Value ? Convert.ToInt32(dr["QuantityAvailable"]) : 0,
                            Reorderpoint = dr["Reorderpoint"] != DBNull.Value ? Convert.ToInt32(dr["Reorderpoint"]) : 0,
                            RetailPrice = dr["RetailPrice"] != DBNull.Value ? Convert.ToDouble(dr["RetailPrice"]) : 0.0,
                            SupplierCost = dr["SupplierCost"] != DBNull.Value ? Convert.ToDouble(dr["SupplierCost"]) : 0.0,
                            EquipmentType = dr["EquipmentType"].ToString(),
                            EquipmentDescription = dr["EquipmentDescription"].ToString(),
                            ManufacturerName = dr["ManufacturerName"].ToString(),
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                            IsTaxable = dr["IsTaxable"] != DBNull.Value ? Convert.ToBoolean(dr["IsTaxable"]) : false,
                        }).ToList();
            return NoteList;
        }
        public List<EquipmentSearchModel> GetOnlyRMRServiceListBySearchKeyAndCompanyId(string key, Guid CompanyId, int MaxLoad, string ExistEquipment)
        {
            DataTable dt = _InvoiceDataAccess.GetOnlyRMRServiceListBySearchKeyAndCompanyId(key, CompanyId, MaxLoad, ExistEquipment);
            List<EquipmentSearchModel> NoteList = new List<EquipmentSearchModel>();
            NoteList = (from DataRow dr in dt.Rows
                        select new EquipmentSearchModel()
                        {
                            EquipmentId = (Guid)dr["EquipmentId"],
                            EquipmentName = dr["EquipmentName"].ToString(),
                            QuantityAvailable = dr["QuantityAvailable"] != DBNull.Value ? Convert.ToInt32(dr["QuantityAvailable"]) : 0,
                            Reorderpoint = dr["Reorderpoint"] != DBNull.Value ? Convert.ToInt32(dr["Reorderpoint"]) : 0,
                            RetailPrice = dr["RetailPrice"] != DBNull.Value ? Convert.ToDouble(dr["RetailPrice"]) : 0.0,
                            SupplierCost = dr["SupplierCost"] != DBNull.Value ? Convert.ToDouble(dr["SupplierCost"]) : 0.0,
                            EquipmentType = dr["EquipmentType"].ToString(),
                            EquipmentDescription = dr["EquipmentDescription"].ToString(),
                            ManufacturerName = dr["ManufacturerName"].ToString(),
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                            IsTaxable = dr["IsTaxable"] != DBNull.Value ? Convert.ToBoolean(dr["IsTaxable"]) : false,
                        }).ToList();
            return NoteList;
        }
        public List<EquipmentSearchModel> GetOnlyServiceListByTypeAndSearchKeyAndCompanyId(string key, Guid CompanyId, int MaxLoad, string ExistEquipment,string EqpTypeId)
        {
            DataTable dt = _InvoiceDataAccess.GetOnlyServiceListByTypeAndSearchKeyAndCompanyId(key, CompanyId, MaxLoad, ExistEquipment, EqpTypeId);
            List<EquipmentSearchModel> NoteList = new List<EquipmentSearchModel>();
            NoteList = (from DataRow dr in dt.Rows
                        select new EquipmentSearchModel()
                        {
                            EquipmentId = (Guid)dr["EquipmentId"],
                            EquipmentName = dr["EquipmentName"].ToString(),
                            QuantityAvailable = dr["QuantityAvailable"] != DBNull.Value ? Convert.ToInt32(dr["QuantityAvailable"]) : 0,
                            Reorderpoint = dr["Reorderpoint"] != DBNull.Value ? Convert.ToInt32(dr["Reorderpoint"]) : 0,
                            RetailPrice = dr["RetailPrice"] != DBNull.Value ? Convert.ToDouble(dr["RetailPrice"]) : 0.0,
                            SupplierCost = dr["SupplierCost"] != DBNull.Value ? Convert.ToDouble(dr["SupplierCost"]) : 0.0,
                            EquipmentType = dr["EquipmentType"].ToString(),
                            EquipmentDescription = dr["EquipmentDescription"].ToString(),
                            ManufacturerName = dr["ManufacturerName"].ToString(),
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                            IsTaxable = dr["IsTaxable"] != DBNull.Value ? Convert.ToBoolean(dr["IsTaxable"]) : false,
                        }).ToList();
            return NoteList;
        }
        public List<PaymentHistory> GetPaymentHistoryByInvoiceId(int InvoiceId)
        {
            DataTable dt = _InvoiceDataAccess.GetPaymentHistoryByInvoiceId(InvoiceId);
            List<PaymentHistory> PaymentHistoryList = new List<PaymentHistory>();
            PaymentHistoryList = (from DataRow dr in dt.Rows
                        select new PaymentHistory()
                        {
                            Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0.0,
                            numberCard = DESEncryptionDecryption.DecryptCipherTextToPlainText(dr["numberCard"].ToString()),
                            chkno = dr["chkno"].ToString(),
                            method = dr["method"].ToString()
                        }).ToList();
            return PaymentHistoryList;
        }
        public List<Invoice> GetInvoiceByTransactionId(string transactionId)
        {
            return _InvoiceDataAccess.GetInvoiceByTransactionId(transactionId);
        }
        public DataTable DownloadArbInvoiceReport(AllInvoicesFilter filter, string BillicycleIdList, string InvoicestatusIdList)
        {
            return _InvoiceDataAccess.GetAllInvoicesByfilterDownload(filter, BillicycleIdList, InvoicestatusIdList);
        }
        public List<Invoice> GetEstimateByCompnayIdAndKey(Guid CompanyId, Guid employeeId, string key, string emptag, Guid empid)
        {
            DataTable dt = _InvoiceDataAccess.GetEstimateByCompnayIdAndKey(CompanyId, employeeId, key, emptag, empid);
            List<Invoice> InvoiceList = new List<Invoice>();
            InvoiceList = (from DataRow dr in dt.Rows
                           select new Invoice()
                           {
                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                               CreatedBy = dr["CreatedBy"].ToString(),
                               BillingAddress = dr["BillingAddress"].ToString(),
                               Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                               TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0,
                               CompanyId = (Guid)dr["CompanyId"],
                               CustomerId = (Guid)dr["CustomerId"],
                               CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.Now,
                               CustomerName = dr["CustomerName"].ToString(),
                               DiscountAmount = dr["DiscountAmount"] != DBNull.Value ? Convert.ToDouble(dr["DiscountAmount"]) : 0,
                               DiscountCode = dr["DiscountCode"].ToString(),
                               DueDate = dr["DueDate"] != DBNull.Value ? Convert.ToDateTime(dr["DueDate"]) : DateTime.Now,
                               InvoiceId = dr["InvoiceId"].ToString(),
                               IsEstimate = dr["IsEstimate"] != DBNull.Value ? Convert.ToBoolean(dr["IsEstimate"]) : false,
                               Status = dr["Status"].ToString(),
                               Tax = dr["DiscountAmount"] != DBNull.Value ? Convert.ToDouble(dr["DiscountAmount"]) : 0,
                               LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : DateTime.Now,
                               InstallDate = dr["InstallDate"] != DBNull.Value ? Convert.ToDateTime(dr["InstallDate"]) : new DateTime(),
                               UserNum = dr["UserNum"].ToString(),
                               NoteAddedDate = dr["NoteAddedDate"] != DBNull.Value ? Convert.ToDateTime(dr["NoteAddedDate"]) : new DateTime(),
                           }).ToList();
            return InvoiceList;
        }

        public List<Invoice> GetInvoicebyBookingId(string bookingId)
        {
            return _InvoiceDataAccess.GetByQuery(string.Format(" BookingId = '{0}' ",bookingId));
        }

        public List<Invoice> GetInvoiceByKeyAndCompanyId(Guid CompanyId, string key, string emptag, Guid empid)
        {

            DataTable dt = _InvoiceDataAccess.GetInvoiceByKeyAndCompanyId(CompanyId, key, emptag, empid);
            List<Invoice> InvoiceList = new List<Invoice>();
            InvoiceList = (from DataRow dr in dt.Rows
                           select new Invoice()
                           {
                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                               CreatedBy = dr["CreatedBy"].ToString(),
                               BillingAddress = dr["BillingAddress"].ToString(),
                               Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                               TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0,
                               BalanceDue = dr["BalanceDue"] != DBNull.Value ? Convert.ToDouble(dr["BalanceDue"]) : 0,
                               Balance = dr["Balance"] != DBNull.Value ? Convert.ToDouble(dr["Balance"]) : 0,
                               CompanyId = (Guid)dr["CompanyId"],
                               CustomerId = (Guid)dr["CustomerId"],
                               CustomerIntId = dr["CustomerIntId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerIntId"]) : 0,
                               CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.Now,
                               CustomerName = dr["CustomerName"].ToString(),
                               DueDate = dr["DueDate"] != DBNull.Value ? Convert.ToDateTime(dr["DueDate"]) : DateTime.Now,
                               InvoiceId = dr["InvoiceId"].ToString(),
                               IsEstimate = dr["IsEstimate"] != DBNull.Value ? Convert.ToBoolean(dr["IsEstimate"]) : false,
                               Status = dr["Status"].ToString(),
                               Description = dr["Description"].ToString(),
                               Tax = dr["DiscountAmount"] != DBNull.Value ? Convert.ToDouble(dr["DiscountAmount"]) : 0,
                               LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : DateTime.Now,

                           }).ToList();
            return InvoiceList;
        }

        public Invoice GetByInvoiceId(string invoiceId)
        {
            return _InvoiceDataAccess.GetByQuery(string.Format(" InvoiceId = '{0}'", invoiceId)).FirstOrDefault();
        }

        public CustomerProratedBill GetCusProratedBillByCustomerId(Guid customerId)
        {
            return _CustomerProratedBillDataAccess.GetByQuery(string.Format(" CustomerId = '{0}'", customerId)).LastOrDefault();
        }

        public Invoice GetByInvoiceID(int id)
        {
            return _InvoiceDataAccess.Get(id);
        }
        

        public bool DeleteAllInvoiceDetailsByInvoiceId(string invoiceId)
        {
            return _InvoiceDetailDataAccess.DeleteByInvoiceId(invoiceId);
        }

        public int InsertInvoiceDetails(InvoiceDetail item)
        {
            if (!item.Taxable.HasValue)
            {
                item.Taxable = true;
            }
            return (int)_InvoiceDetailDataAccess.Insert(item);
        }

        public bool DeleteInvoiceById(int id)
        {
            return _InvoiceDataAccess.Delete(id) > 0;
        }
        public List<Invoice> GetInvoiceIdByCustomerIdAndCompanyId(Guid customerID, Guid companyID)
        {

            DataTable dt = _InvoiceDataAccess.GetAllDueInvoiceIdByCustomerId(customerID, companyID);
            List<Invoice> UnpaidId = new List<Invoice>();
            UnpaidId = (from DataRow dr in dt.Rows
                        select new Invoice()
                        {
                            InvoiceId = dr["InvoiceId"].ToString()
                        }).ToList();
            return UnpaidId;
        }

        public Invoice MakeInvoicePaidByInvoiceId(int id)
        {
            return _InvoiceDataAccess.Get(id);
        }

        public List<InvoiceDetail> GetInvoiceDetailsByInvoiceId(string InvoiceId)
        {
            return _InvoiceDetailDataAccess.GetByQuery(string.Format("InvoiceId = '{0}'", InvoiceId)).ToList();
        }
        public List<Invoice> GetInvoiceDetailsByInvoiceIdRMR(string InvoiceId)
        {
            return _InvoiceDataAccess
                .GetByQuery(string.Format("InvoiceId = '{0}' AND IsARBInvoice = 1", InvoiceId))
                .ToList();
        }

        public bool UpdateInvoiceDetails(InvoiceDetail invoiceDetail)
        {
            if (!invoiceDetail.Taxable.HasValue)
            {
                invoiceDetail.Taxable = true;
            }
            return _InvoiceDetailDataAccess.Update(invoiceDetail) > 0;
        }

            public List<Invoice> GetAllEstimateListByCustomerIdAndCompanyId(Guid CustomerId, Guid CompanyId, EstimateFilter filter)
        {
            DataTable dt = _InvoiceDataAccess.GetAllEstimateListByCustomerIdAndCompanyId(CustomerId, CompanyId, filter);
            List<Invoice> InvoiceList = new List<Invoice>();
            InvoiceList = (from DataRow dr in dt.Rows
                           select new Invoice()
                           {
                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                               CreatedBy = dr["CreatedBy"].ToString(),
                               BillingAddress = dr["BillingAddress"].ToString(),
                               Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                               TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0,
                               CompanyId = (Guid)dr["CompanyId"],
                               CustomerId = (Guid)dr["CustomerId"],
                               CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.Now,
                               CustomerName = dr["CustomerName"].ToString(),
                               DiscountAmount = dr["DiscountAmount"] != DBNull.Value ? Convert.ToDouble(dr["DiscountAmount"]) : 0,
                               DiscountCode = dr["DiscountCode"].ToString(),
                               DueDate = dr["DueDate"] != DBNull.Value ? Convert.ToDateTime(dr["DueDate"]) : new DateTime(),
                               InvoiceId = dr["InvoiceId"].ToString(),
                               IsEstimate = dr["IsEstimate"] != DBNull.Value ? Convert.ToBoolean(dr["IsEstimate"]) : false,
                               Status = dr["Status"].ToString(),
                               Tax = dr["DiscountAmount"] != DBNull.Value ? Convert.ToDouble(dr["DiscountAmount"]) : 0,
                               LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : DateTime.Now,
                               InstallDate = dr["InstallDate"] != DBNull.Value ? Convert.ToDateTime(dr["InstallDate"]) : new DateTime(),
                               UserNum = dr["UserNum"].ToString(),
                               NoteAddedDate = dr["NoteAddedDate"] != DBNull.Value ? Convert.ToDateTime(dr["NoteAddedDate"]) : new DateTime(),
                               CustomerViewedTime = dr["CustomerViewedTime"] != DBNull.Value ? Convert.ToDateTime(dr["CustomerViewedTime"]) : new DateTime(),
                               CancelReason = dr["CancelReason"].ToString(),
                               Description = dr["Description"].ToString(),
                               NotesInvoice = dr["NotesInvoice"].ToString(),
                               NoteInvoiceAddedBy = dr["NoteInvoiceAddedBy"].ToString(),
                               CreatedByUid = (Guid)dr["CreatedByUid"],
                               CustomerViewedType = dr["CustomerViewedType"].ToString()
                           }).ToList();
            return InvoiceList;
        }
       
        public List<Invoice> GetAllEstimatesNotConvertedToInvoice(Guid CompanyId)
        {
            return _InvoiceDataAccess.GetByQuery(string.Format("CompanyId= '{0}' and IsEstimate ='true' and DATEDIFF(day,CreatedDate,GETDATE())>7", CompanyId)).ToList();
        }

        public bool DeleteEstimateConvertToOrder(string invid)
        {
            return _InvoiceDataAccess.DeleteEstimateConvertToOrder(invid);
        }

        public List<Invoice> GetAllAutogeneratedUnpaidInvoiceByCompanyIdAndInvoiceFor(AllInvoicesFilter filter)
        {
            var dt = _InvoiceDataAccess.GetAllAutogeneratedUnpaidInvoiceByCompanyIdAndInvoiceFor(filter);
            var listInvoice = new List<Invoice>();
            if (dt != null)
            {
                listInvoice = dt.DataTableToList<Invoice>();
            }
            return listInvoice;
        }
        public List<Invoice> GetRecentInvoiceUnpaidByCompanyIdAndCustomerId(Guid CompanyId, Guid CustomerId)
        {
            DataTable dt = _InvoiceDataAccess.GetRecentInvoiceUnpaidByCompanyIdAndCustomerId(CompanyId, CustomerId);
            List<Invoice> InvoiceList = new List<Invoice>();
            InvoiceList = (from DataRow dr in dt.Rows
                           select new Invoice()
                           {
                               CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.Now,
                               InvoiceId = dr["InvoiceId"].ToString(),
                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0
                           }).ToList();
            return InvoiceList;
        }

        public List<Invoice> GetAllInvoiceByCustomerId(Guid customerId)
        {
            return _InvoiceDataAccess.GetByQuery(string.Format(" CustomerId='{0}' and [Status] !='init'", customerId));
        }
        public List<Invoice> GetAllInvoiceByCustomerIdandInvoiceFor(Guid customerId,string invoiceFor)
        {
            return _InvoiceDataAccess.GetByQuery(string.Format(" CustomerId='{0}' and [invoiceFor] ='{1}'", customerId,invoiceFor));
        }
        public Invoice GetInvoiceByCustomerIdAndStatus(Guid customerId,string status)
        {
            return _InvoiceDataAccess.GetByQuery(string.Format(" CustomerId='{0}' and [Status] ='{1}'", customerId,status)).FirstOrDefault();
        }
        public List<Invoice> GetAllOpenInvoiceByCustomerId(Guid customerId)
        {
            return _InvoiceDataAccess.GetByQuery(string.Format(" CustomerId='{0}' and [Status] ='Open' and IsEstimate = 0", customerId));
        }
        public List<Invoice> GetAllOpenEstimateByCustomerId(Guid customerId)
        {
            return _InvoiceDataAccess.GetByQuery(string.Format(" CustomerId='{0}' and IsEstimate = 1 and [status] in( 'Created','Customer Viewed','Resend To Customer','Sent To Customer') ", customerId));
        }

        public List<InvoiceDetail> GetAllInvoiceDetailEquipmentByEquipmentId(Guid EquipId)
        {
            return _InvoiceDetailDataAccess.GetByQuery(string.Format("EquipmentId = '{0}'", EquipId)).ToList();
        }

        public List<InvoiceDetail> GetInvoiceDetailsByCustomerId(Guid customerId)
        {
            DataSet dt =  _InvoiceDetailDataAccess.GetInvoiceDetailsByCustomerId(customerId);
            List<InvoiceDetail> InvoiceList = new List<InvoiceDetail>();
            InvoiceList = (from DataRow dr in dt.Tables[0].Rows
                           select new InvoiceDetail()
                           {
                              
                               InvoiceId = dr["InvoiceId"].ToString(),
                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                               EquipName = dr["EquipName"].ToString(),
                               Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                           }).ToList();
            return InvoiceList;
        }


        public List<InvoiceDetail> GetInvoiceDetailsServiceByCustomerId(Guid customerId)
        {
            DataSet dt = _InvoiceDetailDataAccess.GetInvoiceDetailsServiceByCustomerId(customerId);
            List<InvoiceDetail> InvoiceList = new List<InvoiceDetail>();
            InvoiceList = (from DataRow dr in dt.Tables[0].Rows
                           select new InvoiceDetail()
                           {

                               InvoiceId = dr["InvoiceId"].ToString(),
                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                               EquipName = dr["EquipName"].ToString(),
                               Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                           }).ToList();
            return InvoiceList;
        }




        public Double GetInvoiceBalanceDueByCustomerId(Guid CustomerId)
        {
            double BalanceDue = 0.0;
            DataTable dt = _InvoiceDataAccess.GetInvoiceBalanceDueByCustomerId(CustomerId);
            if (dt.Rows.Count > 0)
            {
                BalanceDue = dt.Rows[0]["BalanceDue"] != DBNull.Value ? Convert.ToDouble(dt.Rows[0]["BalanceDue"]) : 0.0;
            }
            return BalanceDue;
        }

        public bool CancellInvoiceByBookingId(string bookingId)
        {
            return _InvoiceDataAccess.CancellInvoiceByBookingId(bookingId);
        }

        public List<Invoice> GetInvoiceByRefType(string id)
        {
            return _InvoiceDataAccess.GetByQuery(string.Format("RefType = '{0}'", id));
        }

        public Invoice GetInvoiceByCustomerIdInvoiceId(Guid customerid, string invoiceid)
        {
            return _InvoiceDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and InvoiceId = '{1}'", customerid, invoiceid)).FirstOrDefault();
        }
        public Invoice GetInvoiceByCustomerId(Guid customerid)
        {
            return _InvoiceDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerid)).LastOrDefault();
        }

        public List<InvoiceDetail> GetInvoiceDetailByequipmentId(Guid eqpid)
        {
            return _InvoiceDetailDataAccess.GetByQuery(string.Format("EquipmentId = '{0}'", eqpid)).ToList();
        }

        public bool DeleteInvoiceDetailById(int value)
        {
            return _InvoiceDetailDataAccess.Delete(value) > 0;
        }

        public Invoice GetInvoiceByCustomerIdAndInstallationType(Guid id, string type)
        {
            return _InvoiceDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and [Status] != 'Init' and InstallationType = '{1}'", id, type)).FirstOrDefault();
        }

        public List<EstimateImage> GetAllEstimateImageByInvoiceId(string invid)
        {
            return _EstimateImageDataAccess.GetByQuery(string.Format("InvoiceId = '{0}'", invid)).ToList();
        }

        public Invoice GetOpenInvoicebyBookingId(string bookingId)
        {
            return _InvoiceDataAccess.GetByQuery(string.Format("BookingId = '{0}' and status = 'Open'  order by id desc", bookingId)).FirstOrDefault();
        }

        public Invoice GetPaidInvoiceByBookingId(string bookingId)
        {
            return _InvoiceDataAccess.GetByQuery(string.Format("BookingId = '{0}' and status = 'Paid'  order by id desc", bookingId)).FirstOrDefault();
        }

        public List<Invoice> GetAllInvoiceForRecurringBillInvoiceByFilter(Guid customerId, Guid companyId, DateTime StartDate, DateTime EndDate, string billingCycle, Guid RecurringId, string PaymentFilter)
        {
            string strStartDatequery = "", strEndDatequery = "";
            string strStartDate = StartDate.SetClientZeroHourToUTC().ToString();//ToString("yyyy-MM-dd 00:00:00.000");
            strStartDatequery = string.Format("and InvoiceDate >= '{0}'", strStartDate);
            string strEndDate = EndDate.SetClientMaxHourToUTC().ToString();//ToString("yyyy-MM-dd 23:59:59.999");
            strEndDatequery = string.Format("and InvoiceDate <= '{0}'", strEndDate);
            return _InvoiceDataAccess.GetByQuery(string.Format(" CustomerId='{0}' and CompanyId = '{1}' and InvoiceFor = '{6}' and BillingCycle = '{4}' and IsARBInvoice = 1 and IsEstimate = 0 and TicketId = '{5}' {2} {3}", customerId, companyId, strStartDatequery, strEndDatequery, billingCycle, RecurringId, PaymentFilter));
        }

        public List<InvoiceDetail> GetManuallyUnpaidRecurringBillingInvoiceDetailsListByCustomerId(Guid customerId)
        {
            return _InvoiceDetailDataAccess.GetUnpaidRecurringBillingInvoiceDetailsListByCustomerId(customerId);
        }
        public List<Invoice> GetManuallyUnpaidOthersInvoiceListByCustomerId(Guid customerId, Guid CompanyId)
        {
            return _InvoiceDataAccess.GetUnpaidOthersInvoiceListByCustomerId(customerId, CompanyId);
        }
        public List<Invoice> GetManuallyUnpaidRecurringBillingInvoiceListByCustomerId(Guid customerId, Guid CompanyId)
        {
            return _InvoiceDataAccess.GetUnpaidRecurringBillingInvoiceListByCustomerId(customerId, CompanyId);
        }
        public List<InvoiceDetail> GetManuallyUnpaidOthersInvoiceDetailsListByCustomerId(Guid customerId)
        {
            return _InvoiceDetailDataAccess.GetUnpaidOthersBillingInvoiceDetailsListByCustomerId(customerId);
        }
        public DataTable DownloadUnpaidRMRInvoices(string CustomerIdList, string InvoiceType, string PaymentMethod, Guid CompanyId)
        {
            return _InvoiceDataAccess.DownloadUnpaidRMRInvoiceExcelData(CustomerIdList, InvoiceType, PaymentMethod, CompanyId);
        }
        public List<Invoice> GetAllUnpaidInvoicesByCustomerId(Guid customerId)
        {
            return _InvoiceDataAccess.GetByQuery(string.Format(" CustomerId='{0}' and BalanceDue > 0 and IsEstimate = 0 and [Status] not in('Cancelled', 'Rolled Over', 'Init', 'Paid')", customerId));
        }
        public List<Invoice> GetUnpaidInvoiceFromExcelFileByInvoiceId(string InvoiceId, Guid CustomerId, Guid CompanyId)
        {
                return _InvoiceDataAccess.GetByQuery(string.Format(" InvoiceId in ({0}) and CustomerId = '{1}' and CompanyId = '{2}' and BalanceDue > 0 and IsEstimate = 0 and [Status] not in('Cancelled', 'Rolled Over', 'Init', 'Paid')", InvoiceId, CustomerId, CompanyId)).ToList();                      
        }
        public Invoice GetInvoiceByCustomerIdandIsARB(Guid customerid, string IsARB)
        {
            Invoice _invoice = new Invoice();
            if (IsARB == "RMR")
            {
                _invoice = _InvoiceDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and IsARBInvoice = 1", customerid)).LastOrDefault();
            }
           else if (IsARB == "Others")
            {
                _invoice = _InvoiceDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and (IsARBInvoice != 1 or IsARBInvoice is null)", customerid)).LastOrDefault();
            }
            else if(IsARB == "All")
            {
                _invoice = _InvoiceDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerid)).LastOrDefault();
            }
            return _invoice;
        }
        #region Invoice Statement
        public List<GeneratePdfInvoiceStatementModelList> GetAllForInvoiceStatementDataByCustomerIntIdListWithType(List<int> CustomerIdList, string StatementFor)
        {
            DataSet ds = _InvoiceDataAccess.GetAllForInvoiceStatementByCustomerIntIdList(CustomerIdList, StatementFor);
            List<GeneratePdfInvoiceStatementModelList> model = new List<GeneratePdfInvoiceStatementModelList>();
            List<GeneratePdfInvoiceStatementModel> NewInvoiceList = new List<GeneratePdfInvoiceStatementModel>();
            List<GeneratePdfInvoiceDetailsStatementModel> InvoiceDetailsList = new List<GeneratePdfInvoiceDetailsStatementModel>();
            List<GeneratePdfOtherDueOpenInvoiceStatementModel> DueInvoiceList = new List<GeneratePdfOtherDueOpenInvoiceStatementModel>();
            if (ds != null)
            {
                NewInvoiceList = (from DataRow dr in ds.Tables[0].Rows
                                          select new GeneratePdfInvoiceStatementModel()
                                          {
                                              CustomerId = dr["CustomerId"] != DBNull.Value ? dr["CustomerId"].ToString() : Guid.Empty.ToString(),
                                              CompanyId = dr["CompanyId"] != DBNull.Value ? dr["CompanyId"].ToString() : Guid.Empty.ToString(),
                                              TicketId = dr["TicketId"] != DBNull.Value ? dr["TicketId"].ToString() : Guid.Empty.ToString(),
                                              CustomerIntId = dr["CustomerIntId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerIntId"]) : 0,
                                              InvoiceIntId = dr["InvoiceIntId"] != DBNull.Value ? Convert.ToInt32(dr["InvoiceIntId"]) : 0,
                                              InstallDate = dr["InstallDate"] != DBNull.Value ? Convert.ToDateTime(dr["InstallDate"]) : new DateTime(),
                                              InvoiceDate = dr["InvoiceDate"] != DBNull.Value ? Convert.ToDateTime(dr["InvoiceDate"]) : new DateTime(),
                                              DueDate = dr["DueDate"] != DBNull.Value ? Convert.ToDateTime(dr["DueDate"]) : new DateTime(),
                                              Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                                              TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0,
                                              BalanceDue = dr["BalanceDue"] != DBNull.Value ? Convert.ToDouble(dr["BalanceDue"]) : 0,
                                              Tax = dr["Tax"] != DBNull.Value ? Convert.ToDouble(dr["Tax"]) : 0,
                                              DiscountAmount = dr["DiscountAmount"] != DBNull.Value ? Convert.ToDouble(dr["DiscountAmount"]) : 0,
                                              CreditBalance = dr["CreditBalance"] != DBNull.Value ? Convert.ToDouble(dr["CreditBalance"]) : 0,
                                              NetTotalBalance = dr["NetTotalBalance"] != DBNull.Value ? Convert.ToDouble(dr["NetTotalBalance"]) : 0,
                                              CustomerName = dr["CustomerName"].ToString(),
                                              Street = dr["Street"].ToString(),
                                              CustomerAddress = dr["CustomerAddress"].ToString(),
                                              PhoneNumber = dr["PhoneNumber"].ToString(),
                                              EmailAddress = dr["InvoiceEmailAddress"] != DBNull.Value ? dr["InvoiceEmailAddress"].ToString() : dr["EmailAddress"].ToString(),
                                              InvoiceId = dr["InvoiceId"].ToString(),
                                              Status = dr["Status"].ToString(),
                                              InvoiceFor = dr["InvoiceFor"].ToString(),
                                              Description = dr["Description"].ToString()
                                          }).ToList();
                InvoiceDetailsList = (from DataRow dr in ds.Tables[1].Rows
                                              select new GeneratePdfInvoiceDetailsStatementModel()
                                              {
                                                  Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                                  InvoiceId = dr["InvoiceId"].ToString(),
                                                  EquipDetail = dr["EquipDetail"].ToString(),
                                                  EquipName = dr["EquipName"].ToString(),
                                                  UnitPrice = dr["UnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["UnitPrice"]) : 0,
                                                  TotalPrice = dr["TotalPrice"] != DBNull.Value ? Convert.ToDouble(dr["TotalPrice"]) : 0
                                              }).ToList();
                DueInvoiceList = (from DataRow dr in ds.Tables[2].Rows
                                      select new GeneratePdfOtherDueOpenInvoiceStatementModel()
                                      {
                                          CustomerId = dr["CustomerId"] != DBNull.Value ? dr["CustomerId"].ToString() : Guid.Empty.ToString(),
                                          InvoiceIntId = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                          InvoiceDate = dr["InvoiceDate"] != DBNull.Value ? Convert.ToDateTime(dr["InvoiceDate"]) : new DateTime(),
                                          DueDate = dr["DueDate"] != DBNull.Value ? Convert.ToDateTime(dr["DueDate"]) : new DateTime(),
                                          Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                                          TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0,
                                          BalanceDue = dr["BalanceDue"] != DBNull.Value ? Convert.ToDouble(dr["BalanceDue"]) : 0,
                                          Tax = dr["Tax"] != DBNull.Value ? Convert.ToDouble(dr["Tax"]) : 0,
                                          DiscountAmount = dr["DiscountAmount"] != DBNull.Value ? Convert.ToDouble(dr["DiscountAmount"]) : 0,
                                          InvoiceId = dr["InvoiceId"].ToString(),
                                          Status = dr["Status"].ToString(),
                                          InvoiceFor = dr["InvoiceFor"].ToString(),
                                          Description = dr["Description"].ToString()
                                      }).ToList();
            }

            if(NewInvoiceList != null && NewInvoiceList.Count > 0)
            {
                foreach(var item in NewInvoiceList)
                {
                    GeneratePdfInvoiceStatementModelList dataModel = new GeneratePdfInvoiceStatementModelList();
                    dataModel.InvoiceStatement = item;
                    if (InvoiceDetailsList!= null && InvoiceDetailsList.Count > 0)
                    {
                        dataModel.InvoiceDetailsList = InvoiceDetailsList.Where(x => x.InvoiceId == item.InvoiceId).ToList();
                    }
                    if (DueInvoiceList != null && DueInvoiceList.Count > 0)
                    {
                        dataModel.DueOpenInvoiceList = DueInvoiceList.Where(x => x.CustomerId == item.CustomerId).ToList();
                        if(dataModel.DueOpenInvoiceList != null && dataModel.DueOpenInvoiceList.Count > 0)
                        {
                            dataModel.InvoiceStatement.NetDueAmount = dataModel.DueOpenInvoiceList.Sum(x => x.BalanceDue);
                        }
                    }
                    if(dataModel.InvoiceStatement == null || dataModel.InvoiceDetailsList == null )
                    {
                        continue;
                    }
                    model.Add(dataModel);
                }
            }

            return model;
        }

        public List<GeneratePdfInvoiceStatementModelList> GetPaidInvoiceStatementDataByInvoiceIdList(string StatementFor)
        {
            DataSet ds = _InvoiceDataAccess.GetAllPaidForInvoiceStatementByInvoiceIdList(StatementFor);
            List<GeneratePdfInvoiceStatementModelList> model = new List<GeneratePdfInvoiceStatementModelList>();
            List<GeneratePdfInvoiceStatementModel> NewInvoiceList = new List<GeneratePdfInvoiceStatementModel>();
            List<GeneratePdfInvoiceDetailsStatementModel> InvoiceDetailsList = new List<GeneratePdfInvoiceDetailsStatementModel>();
            List<GeneratePdfOtherDueOpenInvoiceStatementModel> DueInvoiceList = new List<GeneratePdfOtherDueOpenInvoiceStatementModel>();
            if (ds != null)
            {
                NewInvoiceList = (from DataRow dr in ds.Tables[0].Rows
                                  select new GeneratePdfInvoiceStatementModel()
                                  {
                                      CustomerId = dr["CustomerId"] != DBNull.Value ? dr["CustomerId"].ToString() : Guid.Empty.ToString(),
                                      CompanyId = dr["CompanyId"] != DBNull.Value ? dr["CompanyId"].ToString() : Guid.Empty.ToString(),
                                      TicketId = dr["TicketId"] != DBNull.Value ? dr["TicketId"].ToString() : Guid.Empty.ToString(),
                                      CustomerIntId = dr["CustomerIntId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerIntId"]) : 0,
                                      InvoiceIntId = dr["InvoiceIntId"] != DBNull.Value ? Convert.ToInt32(dr["InvoiceIntId"]) : 0,
                                      InstallDate = dr["InstallDate"] != DBNull.Value ? Convert.ToDateTime(dr["InstallDate"]) : new DateTime(),
                                      InvoiceDate = dr["InvoiceDate"] != DBNull.Value ? Convert.ToDateTime(dr["InvoiceDate"]) : new DateTime(),
                                      DueDate = dr["DueDate"] != DBNull.Value ? Convert.ToDateTime(dr["DueDate"]) : new DateTime(),
                                      Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                                      TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0,
                                      BalanceDue = dr["BalanceDue"] != DBNull.Value ? Convert.ToDouble(dr["BalanceDue"]) : 0,
                                      Tax = dr["Tax"] != DBNull.Value ? Convert.ToDouble(dr["Tax"]) : 0,
                                      DiscountAmount = dr["DiscountAmount"] != DBNull.Value ? Convert.ToDouble(dr["DiscountAmount"]) : 0,
                                      CreditBalance = dr["CreditBalance"] != DBNull.Value ? Convert.ToDouble(dr["CreditBalance"]) : 0,
                                      NetTotalBalance = dr["NetTotalBalance"] != DBNull.Value ? Convert.ToDouble(dr["NetTotalBalance"]) : 0,
                                      CustomerName = dr["CustomerName"].ToString(),
                                      Street = dr["Street"].ToString(),
                                      CustomerAddress = dr["CustomerAddress"].ToString(),
                                      PhoneNumber = dr["PhoneNumber"].ToString(),
                                      EmailAddress = dr["InvoiceEmailAddress"] != DBNull.Value ? dr["InvoiceEmailAddress"].ToString() : dr["EmailAddress"].ToString(),
                                      InvoiceId = dr["InvoiceId"].ToString(),
                                      Status = dr["Status"].ToString(),
                                      InvoiceFor = dr["InvoiceFor"].ToString(),
                                      Description = dr["Description"].ToString()
                                  }).ToList();
                InvoiceDetailsList = (from DataRow dr in ds.Tables[1].Rows
                                      select new GeneratePdfInvoiceDetailsStatementModel()
                                      {
                                          Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                          InvoiceId = dr["InvoiceId"].ToString(),
                                          EquipDetail = dr["EquipDetail"].ToString(),
                                          EquipName = dr["EquipName"].ToString(),
                                          UnitPrice = dr["UnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["UnitPrice"]) : 0,
                                          TotalPrice = dr["TotalPrice"] != DBNull.Value ? Convert.ToDouble(dr["TotalPrice"]) : 0
                                      }).ToList();
                if (ds.Tables.Count > 2)
                {
                    DueInvoiceList = (from DataRow dr in ds.Tables[2].Rows
                                      select new GeneratePdfOtherDueOpenInvoiceStatementModel()
                                      {
                                          CustomerId = dr["CustomerId"] != DBNull.Value ? dr["CustomerId"].ToString() : Guid.Empty.ToString(),
                                          InvoiceIntId = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                          InvoiceDate = dr["InvoiceDate"] != DBNull.Value ? Convert.ToDateTime(dr["InvoiceDate"]) : new DateTime(),
                                          DueDate = dr["DueDate"] != DBNull.Value ? Convert.ToDateTime(dr["DueDate"]) : new DateTime(),
                                          Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                                          TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0,
                                          BalanceDue = dr["BalanceDue"] != DBNull.Value ? Convert.ToDouble(dr["BalanceDue"]) : 0,
                                          Tax = dr["Tax"] != DBNull.Value ? Convert.ToDouble(dr["Tax"]) : 0,
                                          DiscountAmount = dr["DiscountAmount"] != DBNull.Value ? Convert.ToDouble(dr["DiscountAmount"]) : 0,
                                          InvoiceId = dr["InvoiceId"].ToString(),
                                          Status = dr["Status"].ToString(),
                                          InvoiceFor = dr["InvoiceFor"].ToString(),
                                          Description = dr["Description"].ToString()
                                      }).ToList();
                }
                else
                {
                    DueInvoiceList = null;
                }
            }

            if (NewInvoiceList != null && NewInvoiceList.Count > 0)
            {
                foreach (var item in NewInvoiceList)
                {
                    GeneratePdfInvoiceStatementModelList dataModel = new GeneratePdfInvoiceStatementModelList();
                    dataModel.InvoiceStatement = item;
                    if (InvoiceDetailsList != null && InvoiceDetailsList.Count > 0)
                    {
                        dataModel.InvoiceDetailsList = InvoiceDetailsList.Where(x => x.InvoiceId == item.InvoiceId).ToList();
                    }
                    if (DueInvoiceList != null && DueInvoiceList.Count > 0)
                    {
                        dataModel.DueOpenInvoiceList = DueInvoiceList.Where(x => x.CustomerId == item.CustomerId).ToList();
                        if (dataModel.DueOpenInvoiceList != null && dataModel.DueOpenInvoiceList.Count > 0)
                        {
                            dataModel.InvoiceStatement.NetDueAmount = dataModel.DueOpenInvoiceList.Sum(x => x.BalanceDue);
                        }
                    }
                    if (dataModel.InvoiceStatement == null || dataModel.InvoiceDetailsList == null)
                    {
                        continue;
                    }
                    model.Add(dataModel);
                }
            }

            return model;
        }
        public GeneratePdfOtherDueOpenInvoiceStatementModel GetTotalDueAmountForStatement(Guid customerId, string Type)
        {
            DataTable dt = _InvoiceDataAccess.GetTotalDueAmountForInvoiceStatement(customerId, Type);
            if (dt != null)
            {
                GeneratePdfOtherDueOpenInvoiceStatementModel InvoiceAmount = (from DataRow dr in dt.Rows
                                                                              select new GeneratePdfOtherDueOpenInvoiceStatementModel()
                                                                              {
                                                                                  TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0,
                                                                                  BalanceDue = dr["TotalDue"] != DBNull.Value ? Convert.ToDouble(dr["TotalDue"]) : 0
                                                                              }).FirstOrDefault();
                return InvoiceAmount;
            }
            return null;
        }
    
        public List<Invoice> GetUnpaidInvoiceListByCustomerIdandIsARB(Guid customerid, string IsARB)
        {
            List<Invoice> _invoiceList = new List<Invoice>();
            if (IsARB == "RMR")
            {
                _invoiceList = _InvoiceDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and IsARBInvoice = 1 and BalanceDue > 0 and IsEstimate = 0 and [Status] not in('Cancelled', 'Rolled Over', 'Init', 'Paid')", customerid)).ToList();
            }
            else if (IsARB == "Others")
            {
                _invoiceList = _InvoiceDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and (IsARBInvoice != 1 or IsARBInvoice is null) and BalanceDue > 0 and IsEstimate = 0 and [Status] not in('Cancelled', 'Rolled Over', 'Init', 'Paid')", customerid)).ToList();
            }
            else if (IsARB == "All")
            {
                _invoiceList = _InvoiceDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and BalanceDue > 0 and IsEstimate = 0 and [Status] not in('Cancelled', 'Rolled Over', 'Init', 'Paid')", customerid)).ToList();
            }
            return _invoiceList;
        }
        #endregion

        #region Recurring Billing Section
        public List<ARBUnpaidInvoiceGenerateList> GetAllRecurringBillingUppaidInvoiceByFilter(AllCustomerFilter filter)
        {
            DataTable dt = _InvoiceDataAccess.GetAllRecurringBillingUppaidInvoiceByFilter(filter);
            if (dt != null)
            {
                List<ARBUnpaidInvoiceGenerateList> Model = (from DataRow dr in dt.Rows
                                                            select new ARBUnpaidInvoiceGenerateList()
                                                            {
                                                                Id = dr["InvoiceIntId"] != DBNull.Value ? Convert.ToInt32(dr["InvoiceIntId"]) : 0,
                                                                CustomerIntId = dr["CustomerIntId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerIntId"]) : 0,
                                                                CustomerName = dr["CustomerName"].ToString(),
                                                                CustomerGuidId = dr["CustomerId"].ToString(),
                                                                InvoiceId = dr["InvoiceId"].ToString(),
                                                                EmailAddress = dr["EmailAddress"].ToString(),
                                                                Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                                                                //BillingMethodId = dr["CustomerPaymentProfileId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerPaymentProfileId"]) : 0,
                                                                DueDate = dr["DueDate"] != DBNull.Value ? Convert.ToDateTime(dr["DueDate"]).UTCToClientTime() : new DateTime(),
                                                                SubTotalAmountWithTax = dr["SubTotalWithTax"] != DBNull.Value ? Convert.ToDouble(dr["SubTotalWithTax"]) : 0,
                                                                TotalAmount = dr["TotalDue"] != DBNull.Value ? Convert.ToDouble(dr["TotalDue"]) : 0,
                                                                PastDueAmount = dr["PastDue"] != DBNull.Value ? Convert.ToDouble(dr["PastDue"]) : 0,
                                                                InvoiceDate = dr["InvoiceDate"] != DBNull.Value ? Convert.ToDateTime(dr["InvoiceDate"]).UTCToClientTime() : new DateTime()
                                                            }).ToList();

                return Model;
            }
            return null;
        }
        #endregion

        #region Digiture

        #endregion Digiture
    }
}                 