using HS.DataAccess;
using HS.Entities;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Facade
{
    public class BillFacade:BaseFacade
    {
        public BillFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        BillDataAccess _BillDataAccess
        {
            get
            {
                return (BillDataAccess)_ClientContext[typeof(BillDataAccess)];
            }
        }
        BillDetailDataAccess _BillDetailDataAccess
        {
            get
            {
                return (BillDetailDataAccess)_ClientContext[typeof(BillDetailDataAccess)];
            }
        }
        BillPaymentDataAccess _BillPaymentDataAccess
        {
            get
            {
                return (BillPaymentDataAccess)_ClientContext[typeof(BillPaymentDataAccess)];
            }
        }
        BillPaymentHistoryDataAccess _BillPaymentHistoryDataAccess
        {
            get
            {
                return (BillPaymentHistoryDataAccess)_ClientContext[typeof(BillPaymentHistoryDataAccess)];
            }
        }
        BillFileDataAccess _BillFileDataAccess
        {
            get
            {
                return (BillFileDataAccess)_ClientContext[typeof(BillFileDataAccess)];
            }
        }
        public Bill GetBillById(int value)
        {
            return _BillDataAccess.Get(value);
        }

        public List<BillDetail> GetBillDetialsListByBillId(int BillId)
        {
            DataTable dt = _BillDetailDataAccess.GetBillDetialsListByBillId(BillId);
            List<BillDetail> NoteList = new List<BillDetail>();
            NoteList = (from DataRow dr in dt.Rows
                        select new BillDetail()
                        {
                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                            EquipmentId = (Guid)dr["EquipmentId"],
                            EquipmentName = dr["EquipmentName"].ToString(),
                            EquipmentDescription = dr["EquipmentDescription"].ToString(),
                            CustomerBillId = BillId,
                            Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                            Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                            Rate = dr["Rate"] != DBNull.Value ? Convert.ToDouble(dr["Rate"]) : 0,
                            Dscription = dr["Dscription"].ToString(),
                            AccoutTypeId = dr["AccoutTypeId"] != DBNull.Value ? Convert.ToInt32(dr["AccoutTypeId"]) : 0,
                            ItemName = dr["ItemName"].ToString(), 
                        }).ToList();
            return NoteList;
        }

        public int InsertBill(Bill Bill)
        {
            return (int)_BillDataAccess.Insert(Bill);
        }
        public List<BillFile> GetAllBillFileByBillNo(string BillNo)
        {
            return _BillFileDataAccess.GetByQuery(string.Format("BillNo ='{0}'", BillNo)).ToList();
        }
        public int InsertBillFile(BillFile Bill)
        {
            return (int)_BillFileDataAccess.Insert(Bill);
        }
        public int DeleteBillFile(int id)
        {
            return (int)_BillFileDataAccess.Delete(id);
        }
        public bool UpdateBill(Bill bill)
        {
            return _BillDataAccess.Update(bill)>0;
        }
        public bool UpdateBillPayment(BillPayment bill)
        {
            return _BillPaymentDataAccess.Update(bill) > 0;
        }
        public List<Bill> GetAllVendorBillByEmployeeIdAndCompanyId(Guid EmpId, Guid CmpId)
        {
            return _BillDataAccess.GetByQuery(string.Format("CompanyId ='{0}'and PaymentStatus !='Init'", CmpId)).ToList();
        }

        public DataTable GetAllBillPaymentHistoryReportByCompanyId(Guid CompanyId,DateTime?StartDate,DateTime?EndDate)
        {
            return _BillPaymentHistoryDataAccess.GetAllBillPaymentHistoryReportByCompanyId(CompanyId,StartDate,EndDate);
        }

        public DataTable GetAllBillReportByCompanyId(Guid CompanyId,DateTime?StartDate,DateTime?EndDate, FilterReportModel filter)
        {
            return _BillDataAccess.GetAllBillReportByCompanyId(CompanyId,StartDate,EndDate, filter);
        }

        public bool DeleteAllBillDetailsByBillId(int id)
        {
            return _BillDetailDataAccess.DeleteAllBillDetailsByBillId(id);
        }

        #region Bills Report
        public BillingReportModel GetAllBillByCompanyId(Guid CompanyId, FilterReportModel filter,string order)
        {
            DataSet dsResult = _BillDataAccess.GetAllBillByCompanyId(CompanyId,null,null, filter,order);
            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];
            BillingReportModel BillingReportModel = new BillingReportModel();
            TotalBill TotalBillingAmount = new TotalBill();
            List<Bill> TransactionList = new List<Bill>();
            TransactionList = (from DataRow dr in dt.Rows
                               select new Bill()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                                   BillCycle = dr["BillCycle"].ToString(),
                                   BillNo = dr["BillNo"].ToString(),
                                   CompanyId = (Guid)dr["CompanyId"],
                                   Notes = dr["Notes"].ToString(),
                                   PaymentDate = dr["PaymentDate"] != DBNull.Value ? Convert.ToDateTime(dr["PaymentDate"]) : new DateTime(),
                                   PaymentDue = dr["PaymentDue"] != DBNull.Value ? Convert.ToDouble(dr["PaymentDue"]) : 0,
                                   PaymentDueDate = dr["PaymentDueDate"] != DBNull.Value ? Convert.ToDateTime(dr["PaymentDueDate"]) : new DateTime(),
                                   PaymentMethod = dr["PaymentMethod"].ToString(),
                                   PaymentStatus = dr["PaymentStatus"].ToString(),
                                   //RefNo = dr["RefNo"].ToString(),
                                   SupplierName = dr["SupplierName"].ToString(),
                                   SupplierCompanyName = dr["SupplierCompanyName"].ToString(),
                                   SupplierId = dr["SupplierId"] != DBNull.Value ? Convert.ToInt32(dr["SupplierId"]) : 0,
                                   Type = dr["Type"].ToString(),
                                   UpdatedBy = dr["UpdatedBy"].ToString(),
                                   UpdatedDate = dr["UpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["UpdatedDate"]) : new DateTime(),

                               }).ToList();
            TotalBillingAmount = (from DataRow dr in dt1.Rows
                                  select new TotalBill()
                                  {
                                      TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToInt32(dr["TotalAmount"]) : 0
                                  }).FirstOrDefault();
            BillingReportModel.BillList = TransactionList;
            BillingReportModel.TotalBill = TotalBillingAmount;
            return BillingReportModel;
        }

        public BillingReportModel GetAllBillByCompanyIdAndDates(Guid CompanyId,DateTime Start,DateTime End, FilterReportModel filter,string order)
        {
            DataSet dsResult = _BillDataAccess.GetAllBillByCompanyId(CompanyId,Start,End, filter, order);
            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];
            BillingReportModel BillingReportModel = new BillingReportModel();
            TotalBill TotalBillingAmount = new TotalBill();
            List<Bill> TransactionList = new List<Bill>();
            TransactionList = (from DataRow dr in dt.Rows
                               select new Bill()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                                   BillCycle = dr["BillCycle"].ToString(),
                                   BillNo = dr["BillNo"].ToString(),
                                   CompanyId = (Guid)dr["CompanyId"],
                                   Notes = dr["Notes"].ToString(),
                                   PaymentDate = dr["PaymentDate"] != DBNull.Value ? Convert.ToDateTime(dr["PaymentDate"]) : new DateTime(),
                                   PaymentDue = dr["PaymentDue"] != DBNull.Value ? Convert.ToDouble(dr["PaymentDue"]) : 0,
                                   PaymentDueDate = dr["PaymentDueDate"] != DBNull.Value ? Convert.ToDateTime(dr["PaymentDueDate"]) : new DateTime(),
                                   PaymentMethod = dr["PaymentMethod"].ToString(),
                                   PaymentStatus = dr["PaymentStatus"].ToString(),
                                   //RefNo = dr["RefNo"].ToString(),
                                   SupplierName = dr["SupplierName"].ToString(),
                                   SupplierCompanyName = dr["SupplierCompanyName"].ToString(),
                                   SupplierId = dr["SupplierId"] != DBNull.Value ? Convert.ToInt32(dr["SupplierId"]) : 0,
                                   Type = dr["Type"].ToString(),
                                   UpdatedBy = dr["UpdatedBy"].ToString(),
                                   UpdatedDate = dr["UpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["UpdatedDate"]) : new DateTime(),

                               }).ToList();
            TotalBillingAmount = (from DataRow dr in dt1.Rows
                                  select new TotalBill()
                                  {
                                      TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0.0
                                  }).FirstOrDefault();
            BillingReportModel.BillList = TransactionList;
            BillingReportModel.TotalBill = TotalBillingAmount;
            return BillingReportModel;
        }
        #endregion
        public void InsertBillDetailList(List<BillDetail> BillDetailList)
        {
            string BillDetailInsertTemplate =
                @"INSERT INTO [BillDetail] ( [CustomerBillId], [EquipmentId], [AccoutTypeId],[Dscription],[Quantity],[Rate],[Amount]) VALUES ({0},'{1}',{2},'{3}',{4},{5},{6});";
            string sql = "";
            foreach (var item in BillDetailList)
            {
                sql += string.Format(BillDetailInsertTemplate, item.CustomerBillId, item.EquipmentId, item.AccoutTypeId, item.Dscription, item.Quantity, item.Rate, item.Amount) + Environment.NewLine;
            }
            _BillDetailDataAccess.InsertTransactionHistoryList(sql);
        }
        

        public List<CheckPayment> GetCheckListByPaymentIdListAndCompanyId(Guid CompanyId, List<int> checkPaymentIdList)
        {
            string PaymentIdList = "0";
            foreach(var item in checkPaymentIdList)
            {
                PaymentIdList += "," + item.ToString();
            }

            DataTable dt = _BillPaymentDataAccess.GetCheckListByPaymentIdListAndCompanyId(CompanyId, PaymentIdList);
            List<CheckPayment> TransactionList = new List<CheckPayment>();
            TransactionList = (from DataRow dr in dt.Rows
                               select new CheckPayment()
                               { 
                                   PaymentId = dr["PaymentId"] != DBNull.Value ? Convert.ToInt32(dr["PaymentId"]) : 0,
                                   Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                                   BillId = dr["BillId"].ToString(),
                                   SupplierName = dr["SupplierName"].ToString(),
                               }).ToList();
            return TransactionList;
        }

        public BillPayment GetBillPaymentByIdAndCompanyId(int BillPaymentId, Guid CompanyId)
        {
            return _BillPaymentDataAccess.GetByQuery(string.Format("Id = {0} and CompanyId='{1}'",BillPaymentId,CompanyId)).FirstOrDefault();
        }

        public List<BillPaymentHistory> GetVendorbillPaymentListBySupplierId(int id)
        {
            DataTable dt = _BillPaymentDataAccess.GetVendorbillPaymentListBySupplierId(id);
            List<BillPaymentHistory> TransactionList = new List<BillPaymentHistory>();
            TransactionList = (from DataRow dr in dt.Rows
                               select new BillPaymentHistory()
                               {
                                   BpaymentId = dr["BpaymentId"] != DBNull.Value ? Convert.ToInt32(dr["BpaymentId"]) : 0,
                                   Tamount = dr["Tamount"] != DBNull.Value ? Convert.ToDouble(dr["Tamount"]) : 0,
                                   Ddate = dr["Ddate"] != DBNull.Value ? Convert.ToDateTime(dr["Ddate"]) : new DateTime(),
                                   Bstatus = dr["Bstatus"].ToString(),
                                   BType = dr["BType"].ToString(),
                                   Bmethod = dr["Bmethod"].ToString(),
                                   BReferenceNo = dr["BReferenceNo"].ToString(),
                                   Bname = dr["Bname"].ToString()
                               }).ToList();
            return TransactionList;
        }

        public BillPaymentHistory GetVendorbillPaymentList(DateTime? StartDate, DateTime? EndDate,string order)
        {
            DataSet dsResult = _BillPaymentDataAccess.GetVendorbillPaymentList(StartDate, EndDate,order);
            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];
            //      DataTable dt = _BillPaymentDataAccess.GetVendorbillPaymentList();
              List<BillPaymentHistory> TransactionList = new List<BillPaymentHistory>();
     //       BillPaymentHistory TransactionList = new BillPaymentHistory();
      

            TransactionList = (from DataRow dr in dt.Rows
                               select new BillPaymentHistory()
                               {
                                   BpaymentId = dr["BpaymentId"] != DBNull.Value ? Convert.ToInt32(dr["BpaymentId"]) : 0,
                                   Tamount = dr["Tamount"] != DBNull.Value ? Convert.ToDouble(dr["Tamount"]) : 0,
                                   Ddate = dr["Ddate"] != DBNull.Value ? Convert.ToDateTime(dr["Ddate"]) : new DateTime(),
                                   Bstatus = dr["Bstatus"].ToString(),
                                   BType = dr["BType"].ToString(),
                                   Bmethod = dr["Bmethod"].ToString(),
                                   Bname = dr["Bname"].ToString(),
                                   BReferenceNo = dr["BReferenceNo"].ToString()
                               }).ToList();
      
            BillPaymentHistory billpayment = new BillPaymentHistory();
            billpayment.TotalAmount = dsResult.Tables[1].Rows[0]["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dsResult.Tables[1].Rows[0]["TotalAmount"]) : 0;

            billpayment.BillPaymentHistoryList = TransactionList;
            return billpayment;
        }

        public ShowBillModel GetAllVendorBillViewByComanyId(Guid CompanyId, VendorBillFilter filter)
        {
            //DataTable dt = _BillPaymentDataAccess.GetAllReceivePaymentList(CompanyId);
            DataSet dsResult = _BillDataAccess.GetAllVendorBillViewByComanyId(CompanyId, filter);
            DataTable dt = dsResult.Tables[0];
            List<ShowBillModel> TransactionList = new List<ShowBillModel>();
            TransactionList = (from DataRow dr in dt.Rows
                               select new ShowBillModel()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   PaymentId = dr["PaymentId"] != DBNull.Value ? Convert.ToInt32(dr["PaymentId"]) : 0,
                                   BillNo = dr["BillNo"].ToString(),
                                   TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) :0,
                                   DueDate = dr["DueDate"] != DBNull.Value ? Convert.ToDateTime(dr["DueDate"]) : new DateTime(),
                                   OpenBalance = dr["OpenBalance"] != DBNull.Value ? Convert.ToDouble(dr["OpenBalance"]) : 0,
                                   PaymentStatus = dr["PaymentStatus"].ToString(),
                                   Type = dr["Type"].ToString(),
                                   SupplierName = dr["SupplierName"].ToString(),
                                   PaymentMethod = dr["PaymentMethod"].ToString(),
                                   SupplierId = dr["SupplierId"] != DBNull.Value ? Convert.ToInt32(dr["SupplierId"]) : 0,
                                   EMPID = (Guid)dr["EMPID"]
                               }).ToList();
            ShowBillModel showbill = new ShowBillModel();
            showbill.TTAmount = dsResult.Tables[1].Rows[0]["TTAmount"] != DBNull.Value ? Convert.ToDouble(dsResult.Tables[1].Rows[0]["TTAmount"]) : 0;
            showbill.TotalOpenBalance = dsResult.Tables[1].Rows[0]["TotalOpenBalance"] != DBNull.Value ? Convert.ToDouble(dsResult.Tables[1].Rows[0]["TotalOpenBalance"]) : 0;

            showbill.ShowBillModelList = TransactionList;
            return showbill;
        }

        public Bill GetBillByBillNo(string billNo)
        {
            return _BillDataAccess.GetByQuery(string.Format(" BillNo = '{0}'", billNo)).FirstOrDefault();
        }

        #region Bill Payment Report

        public List<BillPaymentHistory> GetAllBillPaymentHistoryByCompanyIdAndDates(Guid CompanyId,DateTime StartDate, DateTime EndDate)
        {
            DataTable dt = _BillPaymentHistoryDataAccess.GetAllBillPaymentHistoryByCompanyId(CompanyId,StartDate,EndDate);
            List<BillPaymentHistory> TransactionList = new List<BillPaymentHistory>();
            TransactionList = (from DataRow dr in dt.Rows
                               select new BillPaymentHistory()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                                   Balance = dr["Balance"] != DBNull.Value ? Convert.ToDouble(dr["Balance"]) : 0,
                                   BillPaymentId = dr["BillPaymentId"] != DBNull.Value ? Convert.ToInt32(dr["BillPaymentId"]) : 0,
                                   InvoiceId = dr["InvoiceId"] != DBNull.Value ? Convert.ToInt32(dr["InvoiceId"]) : 0,
                                   BillNo = dr["BillNo"].ToString(),
                                   SupplierName = dr["SupplierName"].ToString(),
                                   SupplierCompanyName = dr["SupplierCompanyName"].ToString(),
                                   TransacationDate = dr["TransacationDate"] != DBNull.Value ? Convert.ToDateTime(dr["TransacationDate"]) : new DateTime(),

                               }).ToList();
            return TransactionList;
        }
        public List<BillPaymentHistory> GetAllBillPaymentHistoryByCompanyId(Guid CompanyId)
        {
            DataTable dt = _BillPaymentHistoryDataAccess.GetAllBillPaymentHistoryByCompanyId(CompanyId,null,null);
            List<BillPaymentHistory> TransactionList = new List<BillPaymentHistory>();
            TransactionList = (from DataRow dr in dt.Rows
                               select new BillPaymentHistory()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                                   Balance = dr["Balance"] != DBNull.Value ? Convert.ToDouble(dr["Balance"]) : 0,
                                   BillPaymentId = dr["BillPaymentId"] != DBNull.Value ? Convert.ToInt32(dr["BillPaymentId"]) : 0,
                                   InvoiceId = dr["InvoiceId"] != DBNull.Value ? Convert.ToInt32(dr["InvoiceId"]) : 0,
                                   BillNo = dr["BillNo"].ToString(),
                                   SupplierName = dr["SupplierName"].ToString(),
                                   SupplierCompanyName = dr["SupplierCompanyName"].ToString(),
                                   TransacationDate = dr["TransacationDate"] != DBNull.Value ? Convert.ToDateTime(dr["TransacationDate"]) : new DateTime(),

                               }).ToList();
            return TransactionList;
        }

        #endregion

        public List<BillPaymentHistory> GetAllBillHistoryByBillId(int value)
        {
            return _BillPaymentHistoryDataAccess.GetByQuery(string.Format("BillPaymentId ={0}",value));
        }

        public List<OutStandingTransactions> GetAllReceivePaymentList(Guid CompanyId, int? paymentId,int ? SupplierId, Guid? empid)
        {
            DataTable dt = _BillPaymentDataAccess.GetAllReceivePaymentList(CompanyId, paymentId, SupplierId, empid);
            List<OutStandingTransactions> TransactionList = new List<OutStandingTransactions>();
            TransactionList = (from DataRow dr in dt.Rows
                               select new OutStandingTransactions()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   Description = dr["Description"].ToString(),
                                   JobName = dr["JobName"].ToString(),
                                   InvoiceId = dr["InvoiceId"].ToString(),
                                   POId = dr["POId"].ToString(),
                                   CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                   DueDate = dr["DueDate"] != DBNull.Value ? Convert.ToDateTime(dr["DueDate"]) : new DateTime(),
                                   OriginalAmount = dr["OriginalAmount"] != DBNull.Value ? Convert.ToDouble(dr["OriginalAmount"]) : 0,
                                   OpenBalance = dr["OpenBalance"] != DBNull.Value ? Convert.ToDouble(dr["OpenBalance"]) : 0,
                                   Payment = dr["Payment"] != DBNull.Value ? Convert.ToDouble(dr["Payment"]) : 0,
                                   
                               }).ToList();
            return TransactionList;
        }

        public int InsertBillPayment(BillPayment bp)
        {
            return (int)_BillPaymentDataAccess.Insert(bp);
        }

        public void InsertBillPaymentHistoryList(List<BillPaymentHistory> bphistory)
        {
            string TrHistoryInsertTemplate = "INSERT INTO [BillPaymentHistory] ( [BillPaymentId], [InvoiceId], [Amount],[Balance]) VALUES ({0},{1},{2},{3});";
            string sql = "";
            foreach (var item in bphistory)
            {
                sql += string.Format(TrHistoryInsertTemplate, item.BillPaymentId, item.InvoiceId, item.Amount, item.Balance) + Environment.NewLine;
            }
            _BillPaymentHistoryDataAccess.InsertBillPaymentHistoryList(sql);
        }

        public Supplier GetVendorBillDeatailByVendorId(int id, string order,int PageNo, int PageSize,string SearchText)
        {
            Supplier SupplierModel = new Supplier();

            List<CustomerBillAccoutType> accountType = new List<CustomerBillAccoutType>();
            List<ShowBillModel> TransactionList = new List<ShowBillModel>();
            TotalCount totalCount = new TotalCount();
            DataSet dsResult = _BillDataAccess.GetVendorBillDeatailByVendorId(id, order,PageNo, PageSize,SearchText);
            DataTable dt = dsResult.Tables[0];
            DataTable dtCategory = dsResult.Tables[1];
            /*CustomerBillAccoutType */
            accountType = (from DataRow dr1 in dtCategory.Rows
                           select new CustomerBillAccoutType()
                           {
                               CustomerBillID = dr1["CustomerBillID"] != DBNull.Value ? Convert.ToInt32(dr1["CustomerBillID"]) : 0,
                               Type = dr1["Type"].ToString()
                           }).ToList();
            TransactionList = (from DataRow dr in dt.Rows
                               select new ShowBillModel()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   BillNo = dr["BillNo"].ToString(),
                                   JobName=dr["JobName"].ToString(),
                                   TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0,
                                   DueDate = dr["DueDate"] != DBNull.Value ? Convert.ToDateTime(dr["DueDate"]) : new DateTime(),
                                   OpenBalance = dr["OpenBalance"] != DBNull.Value ? Convert.ToDouble(dr["OpenBalance"]) : 0,
                                   PaymentStatus = dr["PaymentStatus"].ToString(),
                                   Type = dr["Type"].ToString(),
                                   CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                   //VendorName = dr["VendorName"].ToString(),
                                   PaymentMethod = dr["PaymentMethod"].ToString(),
                                   CustomerBillAccoutType = accountType.Where(a=>a.CustomerBillID == Convert.ToInt32(dr["Id"])).ToList(),
                                   InvoiceId = dr["InvoiceId"].ToString(),
                                   PurchaseOrderId = dr["PurchaseOrderId"].ToString(),
                                   Notes = dr["Notes"].ToString()
                               }).ToList();
            totalCount = (from DataRow dr in dsResult.Tables[2].Rows
                          select new TotalCount()
                          {
                              CountTotal = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                          }).FirstOrDefault();
            SupplierModel.SupplierBillList = new List<ShowBillModel>();
            SupplierModel.SupplierBillList = TransactionList;
            SupplierModel.TotalCount = totalCount;
            return SupplierModel;
        }

        public List<ShowBillModel> GetVendorBillPayableList(Guid companyid, DateTime? Start, DateTime? End,string order)
        {
            DataTable dt = _BillDataAccess.GetVendorBillPayableList(companyid, Start, End, order);
            List<ShowBillModel> TransactionList = new List<ShowBillModel>();
            TransactionList = (from DataRow dr in dt.Rows
                               select new ShowBillModel()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   BillNo = dr["BillNo"].ToString(),
                                   TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0,
                                   DueDate = dr["DueDate"] != DBNull.Value ? Convert.ToDateTime(dr["DueDate"]) : new DateTime(),
                                   OpenBalance = dr["OpenBalance"] != DBNull.Value ? Convert.ToDouble(dr["OpenBalance"]) : 0,
                                   PaymentStatus = dr["PaymentStatus"].ToString(),
                                   Type = dr["Type"].ToString(),
                                   //VendorName = dr["VendorName"].ToString(),
                                   PaymentMethod = dr["PaymentMethod"].ToString()
                               }).ToList();
            return TransactionList;
        }

        public long InsertBillDetail(BillDetail bd)
        {
            return _BillDetailDataAccess.Insert(bd);
        }

        public bool UpdateBillDetail(BillDetail bd)
        {
            return _BillDetailDataAccess.Update(bd) > 0;
        }

        public VendorBillAmountPanel GetVendorBillAmountPanelByCompanyId(Guid companyid)
        {
            DataTable dt = _BillDataAccess.GetVendorBillAmountPanelByCompanyId(companyid);
            VendorBillAmountPanel TransactionList = new VendorBillAmountPanel();
            TransactionList = (from DataRow dr in dt.Rows
                               select new VendorBillAmountPanel()
                               {
                                   VendorBillOverDue = dr["VendorBillOverDue"] != DBNull.Value ? Convert.ToDouble(dr["VendorBillOverDue"]) : 0.0,
                                   VendorBillOpen = dr["VendorBillOpen"] != DBNull.Value ? Convert.ToDouble(dr["VendorBillOpen"]) : 0.0,
                                   VendorBillPaid = dr["VendorBillPaid"] != DBNull.Value ? Convert.ToDouble(dr["VendorBillPaid"]) : 0.0,
                               }).FirstOrDefault();
            return TransactionList;
        }

        public VendorBillAmountPanel GetVendorDetailBillAmountPanelByCompanyId(Guid companyid, int id)
        {
            DataTable dt = _BillDataAccess.GetVendorDetailBillAmountPanelByCompanyId(companyid, id);
            VendorBillAmountPanel TransactionList = new VendorBillAmountPanel();
            TransactionList = (from DataRow dr in dt.Rows
                               select new VendorBillAmountPanel()
                               {
                                   VendorBillOverDue = dr["VendorBillOverDue"] != DBNull.Value ? Convert.ToDouble(dr["VendorBillOverDue"]) : 0.0,
                                   VendorBillOpen = dr["VendorBillOpen"] != DBNull.Value ? Convert.ToDouble(dr["VendorBillOpen"]) : 0.0,
                                   VendorBillPaid = dr["VendorBillPaid"] != DBNull.Value ? Convert.ToDouble(dr["VendorBillPaid"]) : 0.0,
                               }).FirstOrDefault();
            return TransactionList;
        }

        public List<BillDetail> getBillDetailsVendorListByBillId(int id)
        {
            return _BillDetailDataAccess.GetByQuery(string.Format("CustomerBillId = '{0}'", id)).ToList();
        }

        public List<BillFile> GetBillFileListByBillId(string billid)
        {
            return _BillFileDataAccess.GetByQuery(string.Format("BillNo = '{0}'", billid)).ToList();
        }

        public bool UpdateBillFile(BillFile file)
        {
            return _BillFileDataAccess.Update(file) > 0;
        }

        public BillFile GetBillFileById(int value)
        {
            return _BillFileDataAccess.Get(value);
        }
    }
}
