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
    public class BookingFacade : BaseFacade
    {
        public BookingFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }

        public BookingFacade()
        {
        }

        #region DataAccess
        BookingDetailsDataAccess _BookingDetailsDataAccess
        {
            get
            {
                return (BookingDetailsDataAccess)_ClientContext[typeof(BookingDetailsDataAccess)];
            }
        }
        BookingExtraItemDataAccess _BookingExtraItemDataAccess
        {
            get
            {
                return (BookingExtraItemDataAccess)_ClientContext[typeof(BookingExtraItemDataAccess)];
            }
        }

        TicketBookingDetailsDataAccess _TicketBookingDetailsDataAccess
        {
            get
            {
                return (TicketBookingDetailsDataAccess)_ClientContext[typeof(TicketBookingDetailsDataAccess)];
            }
        }
        TicketBookingExtraItemDataAccess _TicketBookingExtraItemDataAccess
        {
            get
            {
                return (TicketBookingExtraItemDataAccess)_ClientContext[typeof(TicketBookingExtraItemDataAccess)];
            }
        }

        BookingDataAccess _BookingDataAccess
        {
            get
            {
                return (BookingDataAccess)_ClientContext[typeof(BookingDataAccess)];
            }
        }
        EmailTemplateDataAccess _EmailTemplateDataAccess
        {
            get
            {
                return (EmailTemplateDataAccess)_ClientContext[typeof(EmailTemplateDataAccess)];
            }
        }
        CustomerFileDataAccess _CustomerFileDataAccess
        {
            get
            {
                return (CustomerFileDataAccess)_ClientContext[typeof(CustomerFileDataAccess)];
            }
        }
        #endregion



        public List<Booking> GetAllBookingByCompanyIdAndCustomerId(Guid CompanyId, Guid CustomerId)
        {
            return _BookingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and CustomerId='{1}'", CompanyId, CustomerId)).ToList();
        }


        public List<BookingDetails> GetAllBookingDetailsByCompanyId(Guid CompanyId)
        {
            return _BookingDetailsDataAccess.GetByQuery(string.Format(" CompanyId = '{0}'", CompanyId));
        }

        public List<Booking> GetAllBooking()
        {
            return _BookingDataAccess.GetAll();
        }

        public Booking GetById(int value)
        {
            return _BookingDataAccess.Get(value);
        }

        public List<Booking> GetAllBookingByBookingId(Guid CompanyId)
        {
            return _BookingDataAccess.GetByQuery(string.Format(" CompanyId = '{0}'", CompanyId));
        }

        public Booking GetBookingById(int value)
        {
            return _BookingDataAccess.Get(value);
        }

        public int InsertBooking(Booking booking)
        {
            return (int)_BookingDataAccess.Insert(booking);
        }

        public bool UpdateBooking(Booking booking)
        {
            return _BookingDataAccess.Update(booking) > 0;
        }

        public int InsertBookingDetails(BookingDetails item)
        {
            return (int)_BookingDetailsDataAccess.Insert(item);
        }

        public bool DeleteBookingById(int id)
        {
            return _BookingDataAccess.Delete(id) > 0;
        }

        public List<BookingDetails> GetBookingDetailsByBookingId(string BookingId)
        {
            return _BookingDetailsDataAccess.GetByQuery(string.Format("BookingId = '{0}'", BookingId)).ToList();
        }

        public bool UpdateBookingDetails(BookingDetails bookingDetails)
        {
            return _BookingDetailsDataAccess.Update(bookingDetails) > 0;
        }

        public List<Booking> GetAllBookingByCustomerId(Guid customerId)
        {
            return _BookingDataAccess.GetByQuery(string.Format(" CustomerId='{0}' and [Status] !='init'", customerId));
        }

        public bool DeleteAllBookingDetailsByBookingId(string bookingId)
        {
            return _BookingDataAccess.DeleteByBookingId(bookingId);
        }

        public List<BookingExtraItem> GetBookingExtraItemListByBookingId(int value)
        {
            return _BookingExtraItemDataAccess.GetByQuery(string.Format(" BookingId = {0} ", value));
        }
        public List<BookingExtraItem> GetBookingExtraItemListByBookingId(string BookingId)
        {
            if (!string.IsNullOrWhiteSpace(BookingId))
            {
                string[] tokens = BookingId.Split(new[] { "BK" }, StringSplitOptions.None);
                if(tokens.Length == 2)
                {
                    int BookingIntId = 0;

                    if(int.TryParse(tokens[1], out BookingIntId) && BookingIntId > 0)
                    {
                        return _BookingExtraItemDataAccess.GetByQuery(string.Format(" BookingId = {0} ", BookingIntId));
                    }
                }
                
            }
            return null;
            
        }
        public Booking GetByBookingId(string bookingId)
        {
            return _BookingDataAccess.GetByQuery(string.Format(" BookingId = '{0}'", bookingId)).FirstOrDefault();
        }

        public List<Lookup> GetRugShapeListBySearchKey(string key)
        {
            DataTable dt = _BookingDataAccess.GetRugShapeListBySearchKey(key);
            List<Lookup> RugShapeList = new List<Lookup>();
            RugShapeList = (from DataRow dr in dt.Rows
                            select new Lookup()
                            {
                                DataKey = dr["DataKey"].ToString(),
                                DisplayText = dr["DisplayText"].ToString()
                            }).ToList();
            return RugShapeList;
        }

        public List<Package> GetPackageList()
        {
            DataTable dt = _BookingDataAccess.GetPackageList();
            List<Package> PackageList = new List<Package>();
            PackageList = (from DataRow dr in dt.Rows
                           select new Package()
                           {
                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                               CompanyId = (Guid)dr["CompanyId"],
                               Name = dr["Name"].ToString(),
                               OptionEqpMaxLimit = (int)dr["OptionEqpMaxLimit"]

                           }).ToList();
            return PackageList;
        }

        public List<PackageListWithIncludeAndRate> GetPackageListWithIncludeAndRate()
        {
            DataTable dt = _BookingDataAccess.GetPackageListIncludeAndRate();
            List<PackageListWithIncludeAndRate> PackageList = new List<PackageListWithIncludeAndRate>();
            PackageList = (from DataRow dr in dt.Rows
                           select new PackageListWithIncludeAndRate()
                           {
                               Id = dr["PackageId"] != DBNull.Value ? Convert.ToInt32(dr["PackageId"]) : 0,
                               PackageName = dr["PackageName"].ToString(),
                               IncludedPack = dr["Included"].ToString(),
                               PackageRate = dr["PackageRate"] != DBNull.Value ? Convert.ToInt32(dr["PackageRate"]) : 0,

                           }).ToList();
            return PackageList;
        }

        public BookingListShapePackage GetShapeAndPackage()
        {
            DataSet ds = _BookingDataAccess.GetShapeAndPackageList();

            List<Lookup> ShapeList = new List<Lookup>();
            ShapeList = (from DataRow dr in ds.Tables[0].Rows
                         select new Lookup()
                         {
                             DataKey = dr["DataKey"].ToString(),
                             DisplayText = dr["DisplayText"].ToString()
                         }).ToList();

            List<Package> packageList = new List<Package>();
            packageList = (from DataRow dr in ds.Tables[1].Rows
                           select new Package()
                           {
                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                               CompanyId = (Guid)dr["CompanyId"],
                               Name = dr["Name"].ToString(),
                               OptionEqpMaxLimit = (int)dr["OptionEqpMaxLimit"]
                           }).ToList();


            BookingListShapePackage BookingListShapePackages = new BookingListShapePackage();
            BookingListShapePackages.ListLookUp = ShapeList;
            BookingListShapePackages.ListPackage = packageList;

            return BookingListShapePackages;
        }

        public List<PackageListWithIncludeAndRate> GetPackageAndInclude()
        {
            DataSet ds = _BookingDataAccess.GetPackageAndIncludeList();

            List<PackageListWithIncludeAndRate> packageList = new List<PackageListWithIncludeAndRate>();
            packageList = (from DataRow dr in ds.Tables[0].Rows
                           select new PackageListWithIncludeAndRate()
                           {
                               Id = dr["PackageId"] != DBNull.Value ? Convert.ToInt32(dr["PackageId"]) : 0,
                               PackageName = dr["PackageName"].ToString(),
                               PackageRate = dr["PackageRate"] != DBNull.Value ? Convert.ToDouble(dr["PackageRate"]) : 0,

                           }).ToList();

            List<PackageListWithIncludedItem> packageIncludeList = new List<PackageListWithIncludedItem>();
            packageIncludeList = (from DataRow dr in ds.Tables[1].Rows
                                  select new PackageListWithIncludedItem()
                                  {
                                      PackId = dr["PackId"] != DBNull.Value ? Convert.ToInt32(dr["PackId"]) : 0,
                                      PackageInclude = dr["PackageInclude"].ToString(),
                                  }).ToList();

            foreach (var item in packageList)
            {
                item.IncludedPack = String.Join(", ", packageIncludeList.Where(x => x.PackId == item.Id).Select(x => x.PackageInclude));
            }

            return packageList;
        }

        //Get All Booking List By CustomerId And CompanyId
        public List<Booking> GetAllBookingListByCustomerIdAndCompanyId(Guid CustomerId, Guid CompanyId)
        {
            DataTable dt = _BookingDataAccess.GetAllBookingListByCustomerIdAndCompanyId(CustomerId, CompanyId);
            List<Booking> BookingList = new List<Booking>();
            BookingList = (from DataRow dr in dt.Rows
                           select new Booking()
                           {
                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                               CreatedBy = (Guid)dr["CreatedBy"],
                               BillingAddress = dr["BillingAddress"].ToString(),
                               Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                               TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0,
                               CompanyId = (Guid)dr["CompanyId"],
                               CustomerId = (Guid)dr["CustomerId"],
                               CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.Now,
                               CustomerName = dr["CustomerName"].ToString(),
                               DiscountAmount = dr["DiscountAmount"] != DBNull.Value ? Convert.ToDouble(dr["DiscountAmount"]) : 0,
                               DiscountCode = dr["DiscountCode"].ToString(),
                               BookingId = dr["BookingId"].ToString(),
                               Status = dr["Status"].ToString(),
                               UserNum = dr["UserNum"].ToString(),
                               TaxType = dr["TaxType"].ToString(),
                               PickUpDate = dr["PickUpDate"] != DBNull.Value ? Convert.ToDateTime(dr["PickUpDate"]) : DateTime.Now,
                               Tax = dr["DiscountAmount"] != DBNull.Value ? Convert.ToDouble(dr["DiscountAmount"]) : 0,
                               LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : DateTime.Now,
                               CustomerViewedTime = dr["CustomerViewedTime"] != DBNull.Value ? Convert.ToDateTime(dr["CustomerViewedTime"]) : new DateTime(),
                               CustomerViewedType = dr["CustomerViewedType"].ToString(),
                               BookingSource = dr["BookingSource"].ToString()
                           }).ToList();

            return BookingList;
        }


        //public List<UserActivity> GetAllUserActivityCustomerListByCustomerId(Guid companyid, int pageno, int pagesize, DateTime? startdate, DateTime? enddate, string searchtext, Guid CustomerGId, string order)
        //{
        //    DataSet ds = _BookingDataAccess.GetAllUserActivityCustomerListByCustomerId(companyid, pageno, pagesize, startdate, enddate, searchtext, CustomerGId, order);
        //    List<UserActivity> BookingList = new List<UserActivity>();

        //    BookingList = (from DataRow dr in ds.Tables[0].Rows
        //                   select new UserActivity()
        //                   {
        //                       Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
        //                       CustomerGId = (Guid)dr["UACCustomerId"],
        //                       CustomerName = dr["CustomerName"].ToString(),
        //                       ActivityId = (Guid)dr["ActivityId"],
        //                       PageUrl = dr["PageUrl"].ToString(),
        //                       ReferrerUrl = dr["ReferrerUrl"].ToString(),
        //                       Action = dr["Action"].ToString(),
        //                       ActionDisplyText = dr["ActionDisplyText"].ToString(),
        //                       UserIp = dr["UserIp"].ToString(),
        //                       UserAgent = dr["UserAgent"].ToString(),
        //                       UserName = dr["UserName"].ToString(),
        //                       UserId = (Guid)dr["UserId"],
        //                       StatsDate = dr["StatsDate"] != DBNull.Value ? Convert.ToDateTime(dr["StatsDate"]) : DateTime.Now,
        //                   }).ToList();

        //    return BookingList;
        //}


        public UserActivityCustomerModel GetAllUserActivityCustomerListByCustomerId(int pageno, int pagesize, DateTime? startdate, DateTime? enddate, string searchtext, Guid CustomerGId, string order,string logstartdate,string logenddate)
        {

            DataSet ds = _BookingDataAccess.GetAllUserActivityCustomerListByCustomerId(pageno, pagesize, startdate, enddate, searchtext, CustomerGId, order, logstartdate, logenddate);
            List<UserActivity> buildList = new List<UserActivity>();
            buildList = (from DataRow dr in ds.Tables[0].Rows
                         select new UserActivity()
                         {
                             Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                             CustomerGId = (Guid)dr["UACCustomerId"],
                             CustomerName = dr["CustomerName"].ToString(),
                             ActivityId = (Guid)dr["ActivityId"],
                             PageUrl = dr["PageUrl"].ToString(),
                             ReferrerUrl = dr["ReferrerUrl"].ToString(),
                             Action = dr["Action"].ToString(),
                             ActionDisplyText = dr["ActionDisplyText"].ToString(),
                             UserIp = dr["UserIp"].ToString(),
                             UserAgent = dr["UserAgent"].ToString(),
                             UserName = dr["UserName"].ToString(),
                             UserId = (Guid)dr["UserId"],
                             StatsDate = dr["StatsDate"] != DBNull.Value ? Convert.ToDateTime(dr["StatsDate"]) : DateTime.Now,
                         }).ToList();

            SalesReportCountModel SalesReportCountModel = new SalesReportCountModel();
            SalesReportCountModel = (from DataRow dr in ds.Tables[1].Rows
                                     select new SalesReportCountModel()
                                     {
                                         TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                     }).FirstOrDefault();
            TotalSalesAmountModel TotalSalesAmountModel = new TotalSalesAmountModel();
            UserActivityCustomerModel SalesReportModel = new UserActivityCustomerModel();
            SalesReportModel.ListUserActivity = buildList;
            SalesReportModel.InvoiceReportCountModel = SalesReportCountModel;
            SalesReportModel.TotalInvoiceAmountModel = TotalSalesAmountModel;
            return SalesReportModel;
        }

        public DataTable GetAllUserActivityCustomerListByCustomerIdExport(Guid CustomerGId, DateTime? start, DateTime? end, string search)
        {
            return _BookingDataAccess.GetAllUserActivityCustomerListByCustomerIdExport(CustomerGId, start, end, search);
        }


        public bool IsVipFromBookingByCompanyIdAndCustomerId(Guid CompanyId, Guid CustomerId)
        {
            //return _BookingDataAccess.IsVipFromBookingByCompanyIdAndCustomerId(CompanyId, CustomerId));
            bool result = false;

            DataTable dt = _BookingDataAccess.IsVipFromBookingByCompanyIdAndCustomerId(CompanyId, CustomerId);


            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                result = Convert.ToBoolean(row["IsVip"]);
            }



            return result;

            //List<Package> PackageList = new List<Package>();
            //PackageList = (from DataRow dr in dt.Rows
            //               select new Package()
            //               {
            //                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
            //                   CompanyId = (Guid)dr["CompanyId"],
            //                   Name = dr["Name"].ToString(),
            //                   OptionEqpMaxLimit = (int)dr["OptionEqpMaxLimit"]

            //               }).ToList();
            //return PackageList;








        }

        //Get All Booking List For Report

        public EmployeeJobsReport GetAllBookingListForReport(DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize, bool IsPaid, string source)
        {
            DataSet ds = _BookingDataAccess.GetAllJobsReport(FilterStartDate, FilterEndDate, order, pageno, pagesize, IsPaid, source);

            EmployeeJobsReport JobsReportFilter = new EmployeeJobsReport();
            if (ds != null)
            {
                List<Booking> BookingList = new List<Booking>();
                BookingList = (from DataRow dr in ds.Tables[0].Rows
                               select new Booking()
                               {
                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                   CreatedBy = (Guid)dr["CreatedBy"],
                                   CreatedByVal = dr["CreatedByVal"].ToString(),
                                   BillingAddress = dr["BillingAddress"].ToString(),
                                   Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                                   TotalAmount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0,
                                   CompanyId = (Guid)dr["CompanyId"],
                                   CustomerId = (Guid)dr["CustomerId"],
                                   CustomerIntId = dr["CustomerIntId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerIntId"]) : 0,
                                   CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : DateTime.Now,
                                   CustomerName = dr["CustomerName"].ToString(),
                                   BookingSource=dr["BookingSource"].ToString(),
                                   BookingId = dr["BookingId"].ToString(),
                                   Status = dr["Status"].ToString(),
                                   PickUpDate = dr["PickUpDate"] != DBNull.Value ? Convert.ToDateTime(dr["PickUpDate"]) : DateTime.Now,
                               }).ToList();

                PayrollTotalCount PayrollTotalCount = new PayrollTotalCount();
                PayrollTotalCount = (from DataRow dr in ds.Tables[1].Rows
                                     select new PayrollTotalCount()
                                     {
                                         CountTotal = dr["CountTotal"] != DBNull.Value ? Convert.ToInt32(dr["CountTotal"]) : 0
                                     }).FirstOrDefault();
                TotalOnlineJobsCount TotalOnlineBooking = new TotalOnlineJobsCount();
                TotalOnlineBooking = (from DataRow dr in ds.Tables[2].Rows
                                      select new TotalOnlineJobsCount()
                                      {
                                          TotalOnlineBooking = dr["TotalOnline"] != DBNull.Value ? Convert.ToInt32(dr["TotalOnline"]) : 0
                                      }).FirstOrDefault();
                TotalSystemJobsCount TotalSystemBooking = new TotalSystemJobsCount();
                TotalSystemBooking = (from DataRow dr in ds.Tables[3].Rows
                                      select new TotalSystemJobsCount()
                                      {
                                          TotalSystemBooking = dr["TotalSystem"] != DBNull.Value ? Convert.ToInt32(dr["TotalSystem"]) : 0
                                      }).FirstOrDefault();
                TotalCost TotalAmount = new TotalCost();
                TotalAmount = (from DataRow dr in ds.Tables[4].Rows
                               select new TotalCost()
                               {
                                   Amount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0.0
                               }).FirstOrDefault();
                JobsReportFilter.JobReportList = BookingList;
                JobsReportFilter.PayrollTotalCount = PayrollTotalCount;
                JobsReportFilter.TotalOnlineJobsCount = TotalOnlineBooking;
                JobsReportFilter.TotalSystemJobsCount = TotalSystemBooking;
                JobsReportFilter.TotalAmount = TotalAmount;
            }
            return JobsReportFilter;
        }
        public DataTable GetJobReportExport(DateTime FilterStartDate, DateTime FilterEndDate, string order, bool IsPaid, string source)
        {
            return _BookingDataAccess.GetJobReportExport(FilterStartDate, FilterEndDate, order, IsPaid, source);
        }

        public List<TicketBookingExtraItem> GetTicketBookingExtraItemListByBookingId(string bookingId)
        {
            if (!string.IsNullOrWhiteSpace(bookingId))
            {
                string[] tokens = bookingId.Split(new[] { "BK" }, StringSplitOptions.None);
                if (tokens.Length == 2)
                {
                    int BookingIntId = 0;

                    if (int.TryParse(tokens[1], out BookingIntId) && BookingIntId > 0)
                    {
                        return _TicketBookingExtraItemDataAccess.GetByQuery(string.Format(" BookingId = {0} ", BookingIntId));
                    }
                }

            }
            return null;
        }

        public List<TicketBookingDetails> GetTicketBookingDetailsByBookingId(string bookingId)
        {
            //return _TicketBookingDetailsDataAccess.GetByQuery(string.Format("BookingId = '{0}'", bookingId)).ToList();

            DataTable dt = _TicketBookingDetailsDataAccess.GetBookingDetialsListByBookingId(bookingId);
            List<TicketBookingDetails> bookingDetailsList = new List<TicketBookingDetails>();
            bookingDetailsList = (from DataRow dr in dt.Rows
                                  select new TicketBookingDetails()
                                  {
                                      Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                      CompanyId = (Guid)dr["CompanyId"],
                                      BookingId = bookingId,
                                      RugType = dr["RugType"].ToString(),
                                      Package = dr["Package"].ToString(),
                                      Included = dr["Included"].ToString(),
                                      Extras = dr["Extras"].ToString(),
                                      DiscountType = dr["DiscountType"].ToString(),
                                      TaxType = dr["TaxType"].ToString(),
                                      Length = dr["Length"] != DBNull.Value ? Convert.ToDouble(dr["Length"]) : 0,
                                      Width = dr["Width"] != DBNull.Value ? Convert.ToDouble(dr["Width"]) : 0,
                                      Radius = dr["Radius"] != DBNull.Value ? Convert.ToDouble(dr["Radius"]) : 0,
                                      TotalSize = dr["TotalSize"] != DBNull.Value ? Convert.ToDouble(dr["TotalSize"]) : 0,
                                      Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                      TotalPrice = dr["TotalPrice"] != DBNull.Value ? Convert.ToDouble(dr["TotalPrice"]) : 0,
                                      UnitPrice = dr["UnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["UnitPrice"]) : 0,
                                      AddedBy = (Guid)dr["AddedBy"],
                                      AddedDate = dr["AddedDate"] != DBNull.Value ? Convert.ToDateTime(dr["AddedDate"]) : new DateTime(),
                                      Discount = dr["Discount"] != DBNull.Value ? Convert.ToDouble(dr["Discount"]) : 0,
                                      WidthInch = dr["WidthInch"] != DBNull.Value ? Convert.ToInt32(dr["WidthInch"]) : 0,
                                      LengthInch = dr["LengthInch"] != DBNull.Value ? Convert.ToInt32(dr["LengthInch"]) : 0,
                                      RadiusInch = dr["RadiusInch"] != DBNull.Value ? Convert.ToInt32(dr["RadiusInch"]) : 0,
                                      //TotalSizeInch = dr["TotalSizeInch"] != DBNull.Value ? Convert.ToInt32(dr["TotalSizeInch"]) : 0,
                                      RugConditions = dr["RugConditions"].ToString(),
                                      Comments = dr["Comments"].ToString()
                                  }).ToList();

            return bookingDetailsList;

        }

        //Get Booking Details By Booking Id
        public List<BookingDetails> GetBookingDetialsListByBookingId(string bookingId)
        {
            DataTable dt = _BookingDataAccess.GetBookingDetialsListByBookingId(bookingId);
            List<BookingDetails> bookingDetailsList = new List<BookingDetails>();
            bookingDetailsList = (from DataRow dr in dt.Rows
                                  select new BookingDetails()
                                  {
                                      Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                      CompanyId = (Guid)dr["CompanyId"],
                                      BookingId = bookingId,
                                      RugType = dr["RugType"].ToString(),
                                      Package = dr["Package"].ToString(),
                                      Included = dr["Included"].ToString(),
                                      Extras = dr["Extras"].ToString(),
                                      DiscountType = dr["DiscountType"].ToString(),
                                      TaxType = dr["TaxType"].ToString(),
                                      Length = dr["Length"] != DBNull.Value ? Convert.ToDouble(dr["Length"]) : 0,
                                      Width = dr["Width"] != DBNull.Value ? Convert.ToDouble(dr["Width"]) : 0,
                                      Radius = dr["Radius"] != DBNull.Value ? Convert.ToDouble(dr["Radius"]) : 0,
                                      TotalSize = dr["TotalSize"] != DBNull.Value ? Convert.ToDouble(dr["TotalSize"]) : 0,
                                      Quantity = dr["Quantity"] != DBNull.Value ? Convert.ToInt32(dr["Quantity"]) : 0,
                                      TotalPrice = dr["TotalPrice"] != DBNull.Value ? Convert.ToDouble(dr["TotalPrice"]) : 0,
                                      UnitPrice = dr["UnitPrice"] != DBNull.Value ? Convert.ToDouble(dr["UnitPrice"]) : 0,
                                      AddedBy = (Guid)dr["AddedBy"],
                                      AddedDate = dr["AddedDate"] != DBNull.Value ? Convert.ToDateTime(dr["AddedDate"]) : new DateTime(),
                                      Discount = dr["Discount"] != DBNull.Value ? Convert.ToDouble(dr["Discount"]) : 0,
                                      WidthInch = dr["WidthInch"] != DBNull.Value ? Convert.ToInt32(dr["WidthInch"]) : 0,
                                      LengthInch = dr["LengthInch"] != DBNull.Value ? Convert.ToInt32(dr["LengthInch"]) : 0,
                                      RadiusInch = dr["RadiusInch"] != DBNull.Value ? Convert.ToInt32(dr["RadiusInch"]) : 0,
                                      //TotalSizeInch = dr["TotalSizeInch"] != DBNull.Value ? Convert.ToInt32(dr["TotalSizeInch"]) : 0,
                                      
                                  }).ToList();

            return bookingDetailsList;
        }

        public int InsertBookingExtraItem(BookingExtraItem item)
        {
            return (int)_BookingExtraItemDataAccess.Insert(item);
        }

        public bool DeleteAllBookingExtraItemByBookingId(int id)
        {
            return _BookingExtraItemDataAccess.DeleteAllBookingExtraItemByBookingId(id);
        }

        //Get Email Subject And Email Description Using Booking ID
        public EmailHistory GetEmailSubAndDescriptionByBookingId(string bookingId)
        {
            DataTable dt = _BookingDataAccess.GetEmailSubAndDescriptionByBookingId(bookingId);
            EmailHistory history = new EmailHistory();
            history = (from DataRow dr in dt.Rows
                       select new EmailHistory()
                       {
                           Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                           EmailSubject = dr["EmailSubject"].ToString(),
                           EmailBodyContent = dr["EmailBodyContent"].ToString()
                       }).FirstOrDefault();

            return history;
        }
        //For Booking 
        public CustomerFile GetCustomerFileByDescriptionAndCustomerId(string description, Guid CustomerId)
        {
            return _CustomerFileDataAccess.GetByQuery(string.Format("FileDescription = '{0}'  and CustomerId ='{1}'", description, CustomerId)).FirstOrDefault();
        }
        public int UpdateCustomerFile(CustomerFile cf)
        {
            return (int)_CustomerFileDataAccess.Update(cf);
        }
        public long InsertCustomerFile(CustomerFile cf)
        {
            return _CustomerFileDataAccess.Insert(cf);
        }
        public int SaveBookingPdfFile(string fileName, string BookingNo, Guid CustomerId, Guid CompanyId, bool Signed = false)
        {
            //var CurrentUser = (HS.Web.UI.Helper.CustomPrincipal)User;
            CustomerFile cf = GetCustomerFileByDescriptionAndCustomerId(BookingNo + (Signed ? "_Signed" : ""), CustomerId);
            if (cf == null)
            {
                CustomerFile cuf = new CustomerFile()
                {
                    CompanyId = CompanyId,
                    FileId = Guid.NewGuid(),
                    CustomerId = CustomerId,
                    FileDescription = BookingNo + (Signed ? "_Signed" : ""),
                    FileFullName = BookingNo + (Signed ? "_Signed" : "") + ".pdf",
                    //Filename = fileName.IndexOf("/") == 0 ? fileName : "/" + fileName,
                    Filename = "/" + fileName,
                    IsActive = true,
                    Uploadeddate = DateTime.UtcNow,
                    CreatedBy = new Guid(),
                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                    UpdatedBy = new Guid(),
                    UpdatedDate = DateTime.Now.UTCCurrentTime()
                };
                return (int)InsertCustomerFile(cuf);
            }
            else
            {
                cf.Filename = fileName.IndexOf("/") == 0 ? fileName : "/" + fileName;
                return UpdateCustomerFile(cf);
            }
        }

        public bool DeleteTicketBookingDetailsByBookingId(string bookingId)
        {
            return _TicketBookingDetailsDataAccess.DeleteTicketBookingDetailsByBookingId(bookingId);
        }
        public bool DeleteTicketBookingDetailsById(int bookingId)
        {
            return _TicketBookingDetailsDataAccess.DeleteTicketBookingDetailsById(bookingId);
        }
        public bool DeleteTicketBookingExtraItemByBookingId(string bookingId)
        {
            if (!string.IsNullOrWhiteSpace(bookingId))
            {
                string[] tokens = bookingId.Split(new[] { "BK" }, StringSplitOptions.None);
                if (tokens.Length == 2)
                {
                    int BookingIntId = 0;

                    if (int.TryParse(tokens[1], out BookingIntId) && BookingIntId > 0)
                    {
                        return _TicketBookingExtraItemDataAccess.DeleteTicketBookingExtraItemByBookingId(BookingIntId);
                    }
                }

            }
            return false;

            
        }
        public bool DeleteTicketBookingExtraItemById(int bookingId)
        {
            return _TicketBookingExtraItemDataAccess.DeleteTicketBookingExtraItemById(bookingId);
        }


        public int InsertTicketBookingDetails(TicketBookingDetails item)
        {
            return (int)_TicketBookingDetailsDataAccess.Insert(item);
        }

        public int InsertTicketBookingExtraItem(TicketBookingExtraItem item)
        {
            return (int)_TicketBookingExtraItemDataAccess.Insert(item);
        }

        public Booking GetBookingByBookingId(string bookingId)
        {
            return _BookingDataAccess.GetByQuery(string.Format("BookingId = '{0}'",bookingId)).FirstOrDefault();
        }

        public FinishedJobReportModel GetFinishedJobreportsByFilter(FinishedJobFilter filter)
        {
            DataSet ds = _BookingDataAccess.GetFinishedJobreportsByFilter(filter);

            FinishedJobReportModel FinishedJobReportModel = new FinishedJobReportModel();
            FinishedJobReportModel.FinishedJobReportList = new List<FinishedJobReport>();
            FinishedJobReportModel.FinishedJobReportList = (from DataRow dr in ds.Tables[0].Rows
                select new FinishedJobReport()
                {
                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                    BookingIntId = dr["BookingIntId"] != DBNull.Value ? Convert.ToInt32(dr["BookingIntId"]) : 0,
                    BookingId = dr["BookingId"].ToString(),
                    CustomerId = dr["CustomerId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerId"]) : 0,
                    CustomerName = dr["CustomerName"].ToString(),
                    Phone = dr["Phone"].ToString(),
                    Address = dr["Address"].ToString(),
                    Discount = dr["Discount"] != DBNull.Value ? Convert.ToDouble(dr["Discount"]) : 0,
                    TotalPrice = dr["TotalPrice"] != DBNull.Value ? Convert.ToDouble(dr["TotalPrice"]) : 0,
                    Email = dr["Email"].ToString(),
                    
                }).ToList();

            FinishedJobReportModel.PayrollTotalCount = new PayrollTotalCount();
            FinishedJobReportModel.PayrollTotalCount = (from DataRow dr in ds.Tables[1].Rows
                select new PayrollTotalCount()
                {
                    CountTotal = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                }).FirstOrDefault();
            return FinishedJobReportModel;
        }

        public DataTable GetFinishedJobReportListForDownload(FinishedJobFilter filter)
        {
            return _BookingDataAccess.DownloadFinishedJobreportsByFilter(filter);
        }
        public List<PackageSummaryModel> GetPackageSummaryreportsByFilter(PackageSummaryFilter filter)
        {
            DataSet ds = _BookingDataAccess.GetPackageSummaryreportsByFilter(filter);

            List<PackageSummaryModel> FinishedJobReportList = new List<PackageSummaryModel>();
            FinishedJobReportList = (from DataRow dr in ds.Tables[0].Rows
                                     select new PackageSummaryModel()
                                     {
                                         BookingIntId = dr["BookingIntId"] != DBNull.Value ? Convert.ToInt32(dr["BookingIntId"]) : 0,
                                         BookingId = dr["BookingId"].ToString(),
                                         CustomerId = dr["CustomerId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerId"]) : 0,
                                         CustomerName = dr["CustomerName"].ToString(),
                                         IsPaid = dr["IsPaid"] != DBNull.Value ? Convert.ToBoolean(dr["IsPaid"]) : false,
                                         PackageName = dr["PackageName"].ToString(),
                                         RugQty = dr["RugQty"] != DBNull.Value ? Convert.ToInt32(dr["RugQty"]) : 0,
                                         RugShape = dr["RugShape"].ToString(),
                                         Value = dr["Value"] != DBNull.Value ? Convert.ToDouble(dr["Value"]) : 0,
                                     }).ToList();

            PayrollTotalCount PayrollTotalCount = new PayrollTotalCount();
            PayrollTotalCount = (from DataRow dr in ds.Tables[1].Rows
                                 select new PayrollTotalCount()
                                 {
                                     CountTotal = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                 }).FirstOrDefault();
            return FinishedJobReportList;
        }

        public bool DeleteTicketBookingDetailsExcludingIdByBookingId(string bookingId, string DetailsId)
        {
            return _TicketBookingDetailsDataAccess.DeleteTicketBookingDetailsExcludingIdByBookingId(bookingId, DetailsId);
        }

        public TicketBookingDetails GetTicketBookingDetailsById(int id)
        {
            return _TicketBookingDetailsDataAccess.Get(id);
        }

        public bool DeleteTicketBookingExtraItemExcludingIdByBookingId(int bookingIntId, string idList)
        {
            return _BookingExtraItemDataAccess.DeleteTicketBookingExtraItemExcludingIdByBookingId(bookingIntId, idList);
        }

        public TicketBookingExtraItem GetTicketBookingExtraItemById(int id)
        {
            return _TicketBookingExtraItemDataAccess.Get(id);
        }

        public bool UpdateTicketBookingDetails(TicketBookingDetails tbd)
        {
            return _TicketBookingDetailsDataAccess.Update(tbd)>0;
        }

        public bool UpdateTicketBookingExtraItem(TicketBookingExtraItem tBEI)
        {
            return _TicketBookingExtraItemDataAccess.Update(tBEI)>0;
        }
    }
}
