using HS.DataAccess;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using HS.Entities;
using System.Data;
using System.Collections;
using HS.Framework.Utils;
using System.Net.Mail;
using System.Web;

namespace HS.Facade
{
    public class TicketFacade : BaseFacade
    {
        public TicketFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        TicketDataAccess _TicketDataAccess
        {
            get
            {
                return (TicketDataAccess)_ClientContext[typeof(TicketDataAccess)];
            }
        }
        EmployeeDataAccess _EmployeeDataAccess
        {
            get
            {
                return (EmployeeDataAccess)_ClientContext[typeof(EmployeeDataAccess)];
            }
        }
        EmailTemplateDataAccess _EmailTemplateDataAccess
        {
            get
            {
                return (EmailTemplateDataAccess)_ClientContext[typeof(EmailTemplateDataAccess)];
            }
        }
        TicketFileDataAccess _TicketFileDataAccess
        {
            get
            {
                return (TicketFileDataAccess)_ClientContext[typeof(TicketFileDataAccess)];
            }
        }
        TicketUserDataAccess _TicketUserDataAccess
        {
            get
            {
                return (TicketUserDataAccess)_ClientContext[typeof(TicketUserDataAccess)];
            }
        }
        InvoiceDataAccess _InvoiceDataAccess
        {
            get
            {
                return (InvoiceDataAccess)_ClientContext[typeof(InvoiceDataAccess)];
            }
        }
        InvoiceDetailDataAccess _InvoiceDetailDataAccess
        {
            get
            {
                return (InvoiceDetailDataAccess)_ClientContext[typeof(InvoiceDetailDataAccess)];
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
        NotificationDataAccess _NotificationDataAccess
        {
            get
            {
                return (NotificationDataAccess)_ClientContext[typeof(NotificationDataAccess)];
            }
        }
        NotificationUserDataAccess _NotificationUserDataAccess
        {
            get
            {
                return (NotificationUserDataAccess)_ClientContext[typeof(NotificationUserDataAccess)];
            }
        }
        CustomerAppointmentDataAccess _CustomerAppoinmentDataAccess
        {
            get
            {
                return (CustomerAppointmentDataAccess)_ClientContext[typeof(CustomerAppointmentDataAccess)];
            }
        }
        TicketReplyDataAccess _TicketReplyDataAccess
        {
            get
            {
                return (TicketReplyDataAccess)_ClientContext[typeof(TicketReplyDataAccess)];
            }
        }
        SalesCommissionDataAccess _SalesCommissionDataAccess
        {
            get
            {
                return (SalesCommissionDataAccess)_ClientContext[typeof(SalesCommissionDataAccess)];
            }
        }
        TechCommissionDataAccess _TechCommissionDataAccess
        {
            get
            {
                return (TechCommissionDataAccess)_ClientContext[typeof(TechCommissionDataAccess)];
            }
        }
        AddMemberCommissionDataAccess _AddMemberCommissionDataAccess
        {
            get
            {
                return (AddMemberCommissionDataAccess)_ClientContext[typeof(AddMemberCommissionDataAccess)];
            }
        }
        FinRepCommissionDataAccess _FinRepCommissionDataAccess
        {
            get
            {
                return (FinRepCommissionDataAccess)_ClientContext[typeof(FinRepCommissionDataAccess)];
            }
        }
        FollowUpCommissionDataAccess _FollowUpCommissionDataAccess
        {
            get
            {
                return (FollowUpCommissionDataAccess)_ClientContext[typeof(FollowUpCommissionDataAccess)];
            }
        }
        ServiceCallCommissionDataAccess _ServiceCallCommissionDataAccess
        {
            get
            {
                return (ServiceCallCommissionDataAccess)_ClientContext[typeof(ServiceCallCommissionDataAccess)];
            }
        }
        RescheduleCommissionDataAccess _RescheduleCommissionDataAccess
        {
            get
            {
                return (RescheduleCommissionDataAccess)_ClientContext[typeof(RescheduleCommissionDataAccess)];
            }
        }
        RescheduleTicketDataAccess _RescheduleTicketDataAccess
        {
            get
            {
                return (RescheduleTicketDataAccess)_ClientContext[typeof(RescheduleTicketDataAccess)];
            }
        }

        AdditionalMembersAppointmentDataAccess _AdditionalMembersAppointmentDataAccess
        {
            get
            {
                return (AdditionalMembersAppointmentDataAccess)_ClientContext[typeof(AdditionalMembersAppointmentDataAccess)];
            }
        }
        TicketCustomerNotificationDataAccess _TicketCustomerNotificationDataAccess
        {
            get
            {
                return (TicketCustomerNotificationDataAccess)_ClientContext[typeof(TicketCustomerNotificationDataAccess)];
            }
        }
        TicketStatusImageSettingDataAccess _TicketStatusImageSettingDataAccess
        {
            get
            {
                return (TicketStatusImageSettingDataAccess)_ClientContext[typeof(TicketStatusImageSettingDataAccess)];
            }
        }
        CustomerAppointmentEquipmentDataAccess _CustomerAppointmentEquipmentDataAccess
        {
            get
            {
                return (CustomerAppointmentEquipmentDataAccess)_ClientContext[typeof(CustomerAppointmentEquipmentDataAccess)];
            }
        }
        TicketPaymentDataAccess _TicketPaymentDataAccess
        {
            get
            {
                return (TicketPaymentDataAccess)_ClientContext[typeof(TicketPaymentDataAccess)];
            }
        }

        PaymentInfoCustomerDataAccess _PaymentInfoCustomerDataAccess
        {
            get
            {
                return (PaymentInfoCustomerDataAccess)_ClientContext[typeof(PaymentInfoCustomerDataAccess)];
            }
        }

        PaymentProfileCustomerDataAccess _PaymentProfileCustomerDataAccess
        {
            get
            {
                return (PaymentProfileCustomerDataAccess)_ClientContext[typeof(PaymentProfileCustomerDataAccess)];
            }
        }

        GlobalSettingDataAccess _GlobalSettingDataAccess
        {
            get
            {
                return (GlobalSettingDataAccess)_ClientContext[typeof(GlobalSettingDataAccess)];
            }
        }

        AdjustmentFundingDataAccess _AdjustmentFundingDataAccess
        {
            get
            {
                return (AdjustmentFundingDataAccess)_ClientContext[typeof(AdjustmentFundingDataAccess)];
            }
        }
        TicketNotificationEmailDataAccess _TicketNotificationEmailDataAccess
        {
            get
            {
                return (TicketNotificationEmailDataAccess)_ClientContext[typeof(TicketNotificationEmailDataAccess)];
            }
        }
        public TicketListModel GetTicketListByCustomerIdAndFilter(TicketFilter Filters, string techid)
        {
            TicketListModel Model = new TicketListModel();
            DataSet ds = _TicketDataAccess.GetTicketListByCustomerIdAndFilter(Filters, techid);

            Model.Tickets = (from DataRow dr in ds.Tables[0].Rows
                             select new Ticket()
                             {
                                 Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                 CompanyId = (Guid)dr["CompanyId"],
                                 CustomerId = (Guid)dr["CustomerId"],
                                 TicketId = (Guid)dr["TicketId"],
                                 LastUpdatedBy = (Guid)dr["LastUpdatedBy"],
                                 CreatedBy = (Guid)dr["CreatedBy"],
                                 Status = dr["Status"].ToString(),
                                 TicketType = dr["TicketType"].ToString(),
                                 AssignedTo = dr["AssignedTo"].ToString().TrimEnd(' ', ','),
                                 AdditionalMembers = dr["AdditionalMembers"].ToString().TrimEnd(' ', ','),
                                 Priority = dr["Priority"].ToString(),
                                 Subject = dr["Subject"].ToString(),
                                 Message = dr["Message"].ToString(),
                                 LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime(),
                                 CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                 CompletionDate = dr["CompletionDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletionDate"]) : new DateTime(),
                                 AttachmentsCount = dr["AttachmentsCount"] != DBNull.Value ? Convert.ToInt32(dr["AttachmentsCount"]) : 0,
                                 RepliesCount = dr["RepliesCount"] != DBNull.Value ? Convert.ToInt32(dr["RepliesCount"]) : 0,
                                 AppointmentStartTimeVal = dr["AppointmentStartTimeVal"].ToString(),
                                 AppointmentStartTime = dr["AppointmentStartTime"].ToString(),
                                 AppointmentEndTime = dr["AppointmentStartTime"].ToString(),
                                 AppointmentEndTimeVal = dr["AppointmentEndTimeVal"].ToString(),
                                 TicketTypeVal = dr["TicketTypeVal"].ToString(),
                                 StatusVal = dr["StatusVal"].ToString(),
                                 PriorityVal = dr["PriorityVal"].ToString(),
                                 CreatedByVal = dr["CreatedByVal"].ToString(),
                                 ExceedQuantity = dr["ExceedQuantity"] != DBNull.Value ? Convert.ToInt32(dr["ExceedQuantity"]) : 0,
                                 IsClosed = dr["IsClosed"] != DBNull.Value ? Convert.ToBoolean(dr["IsClosed"]) : false,
                                 IsImportedTicket = dr["IsImportedTicket"] != DBNull.Value ? Convert.ToBoolean(dr["IsImportedTicket"]) : false,
                                 ReferenceTicketId = dr["ReferenceTicketId"] != DBNull.Value ? Convert.ToInt32(dr["ReferenceTicketId"]) : 0
                             }).ToList();

            Model.TotalCount = ds.Tables[1].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCount"]) : 0;
            Model.PageNo = Filters.PageNo.Value;
            Model.PageSize = Filters.PageSize;
            Model.Searchtext = Filters.SearchText;
            Model.TicketType = Filters.TicketType;
            Model.TicketStatus = Filters.TicketStatus;
            Model.Assigned = Filters.Assigned;
            return Model;
            // return _TicketDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerId));
        }

        public TicketListModel GetTechTicketListByCustomerIdAndFilter(TicketFilter Filters)
        {
            TicketListModel Model = new TicketListModel();
            DataSet ds = _TicketDataAccess.GetTechTicketListByCustomerIdAndFilter(Filters);

            Model.Tickets = (from DataRow dr in ds.Tables[0].Rows
                             select new Ticket()
                             {
                                 Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                 CompanyId = (Guid)dr["CompanyId"],
                                 CustomerId = (Guid)dr["CustomerId"],
                                 TicketId = (Guid)dr["TicketId"],
                                 LastUpdatedBy = (Guid)dr["LastUpdatedBy"],
                                 CreatedBy = (Guid)dr["TicketId"],
                                 AssignedTo = dr["AssignedTo"].ToString().TrimEnd(' ', ','),
                                 AdditionalMembers = dr["AdditionalMembers"].ToString().TrimEnd(' ', ','),
                                 StatusVal = dr["StatusVal"].ToString(),
                                 Status = dr["Status"].ToString(),
                                 TicketType = dr["TicketType"].ToString(),
                                 Message = dr["Message"].ToString(),
                                 Priority = dr["Priority"].ToString(),

                                 LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime(),
                                 CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                 CompletionDate = dr["CompletionDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletionDate"]) : new DateTime(),


                             }).ToList();

            Model.TotalCount = ds.Tables[1].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCount"]) : 0;
            Model.PageNo = Filters.PageNo.Value;
            Model.PageSize = Filters.PageSize;
            Model.Searchtext = Filters.SearchText;
            Model.TicketType = Filters.TicketType;
            Model.TicketStatus = Filters.TicketStatus;
            Model.Assigned = Filters.Assigned;
            return Model;
            // return _TicketDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerId));
        }
        public List<TicketReply> GetAllTicketReplyByTicketId(Guid TicketId, string search)
        {
            DataSet ds = _TicketDataAccess.GetAllTicketReplyByTicketId(TicketId, search);

            List<TicketReply> TicketReplyes = (from DataRow dr in ds.Tables[0].Rows
                                               select new TicketReply()
                                               {
                                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                   TicketId = (Guid)dr["TicketId"],
                                                   UserId = (Guid)dr["UserId"],
                                                   RepliedDate = dr["RepliedDate"] != DBNull.Value ? Convert.ToDateTime(dr["RepliedDate"]) : new DateTime(),
                                                   Message = dr["Message"].ToString(),
                                                   LatLng = dr["LatLng"].ToString(),
                                                   CreatedByVal = dr["CreatedByVal"].ToString(),
                                                   TypeReply = dr["TypeReply"].ToString(),
                                                   ProfilePicture = dr["ProfilePicture"].ToString(),
                                                   IsPrivate = dr["IsPrivate"] != DBNull.Value ? Convert.ToBoolean(dr["IsPrivate"]) : false,
                                                   IsOverview = dr["IsOverview"] != DBNull.Value ? Convert.ToBoolean(dr["IsOverview"]) : false,
                                               }).ToList();



            //return _TicketDataAccess.Get(TicketId);
            return TicketReplyes;
        }
        public List<AssignTicket> GetAllAssignedTicketByUserId(AssignTicketFilter filter)
        {
            DataSet ds = _TicketDataAccess.GetAllAssignedTicketByUserId(filter);

            List<AssignTicket> AssignTicket = (from DataRow dr in ds.Tables[0].Rows
                                               select new AssignTicket()
                                               {

                                                   CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                                   CustomerId = dr["CustomerId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerId"]) : 0,
                                                   TicketId = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                   TicketType = dr["TicketType"].ToString(),
                                                   TicketStatus = dr["Status"].ToString(),
                                                   CreatedBy = dr["CreatedBy"].ToString(),
                                                   CustomerName = dr["CustomerName"].ToString(),
                                                   AppointmentStartTime = dr["AppointmentStartTime"].ToString(),
                                                   AppointmentEndTime = dr["AppointmentEndTime"].ToString(),
                                                   CompletionDate = dr["CompletionDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletionDate"]) : new DateTime(),
                                                   IsPrimary = dr["IsPrimary"] != DBNull.Value ? Convert.ToBoolean(dr["IsPrimary"]) : false,
                                                   NotificationOnly = dr["NotificationOnly"] != DBNull.Value ? Convert.ToBoolean(dr["NotificationOnly"]) : false,
                                                   AssignedUserName = dr["AssignedUserName"].ToString()
                                               }).ToList();



            //return _TicketDataAccess.Get(TicketId);
            return AssignTicket;
        }
        public AssignAllTicket GetAllTicketByUserId(AssignTicketFilter filter, int PageNo, int PageSize)
        {
            DataSet ds = _TicketDataAccess.GetAllTicketByUserId(filter, PageNo, PageSize);

            List<AssignAllTicket> AssignTicket = (from DataRow dr in ds.Tables[0].Rows
                                               select new AssignAllTicket()
                                               {

                                                   CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                                   CustomerId = dr["CustomerId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerId"]) : 0,
                                                   TicketId = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                   TicketType = dr["TicketType"].ToString(),
                                                   TicketStatus = dr["Status"].ToString(),
                                                   CreatedBy = dr["CreatedBy"].ToString(),
                                                   CustomerName = dr["CustomerName"].ToString(),
                                                   AppointmentStartTime = dr["AppointmentStartTime"].ToString(),
                                                   AppointmentEndTime = dr["AppointmentEndTime"].ToString(),
                                                   CompletionDate = dr["CompletionDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletionDate"]) : new DateTime(),
                                                   IsPrimary = dr["IsPrimary"] != DBNull.Value ? Convert.ToBoolean(dr["IsPrimary"]) : false,
                                                   NotificationOnly = dr["NotificationOnly"] != DBNull.Value ? Convert.ToBoolean(dr["NotificationOnly"]) : false,
                                                   AssignedUserName = dr["AssignedUserName"].ToString()
                                               }).ToList();
            AssignAllTicket assigned = new AssignAllTicket();
            assigned.TotalCount = ds.Tables[1].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCount"]) : 0;
            assigned.CountTicket = ds.Tables[2].Rows[0]["CountTicket"] != DBNull.Value ? Convert.ToInt32(ds.Tables[2].Rows[0]["CountTicket"]) : 0;
            assigned.AssignTicketList = AssignTicket;

            //return _TicketDataAccess.Get(TicketId);
            return assigned;
        }
        public List<AssignTicket> GetAllAssignedInspectionTicketByUserId(AssignTicketFilter filter)
        {
            DataSet ds = _TicketDataAccess.GetAllAssignedTicketByUserId(filter);

            List<AssignTicket> AssignTicket = (from DataRow dr in ds.Tables[0].Rows
                                               select new AssignTicket()
                                               {

                                                   CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                                   CustomerId = dr["CustomerId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerId"]) : 0,
                                                   TicketId = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                   TicketType = dr["TicketType"].ToString(),
                                                   CreatedBy = dr["CreatedBy"].ToString(),
                                                   CustomerName = dr["CustomerName"].ToString(),
                                                   CompletionDate = dr["CompletionDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletionDate"]) : new DateTime(),
                                                   AppointmentStartTime = dr["AppointmentStartTime"].ToString(),
                                                   AppointmentEndTime = dr["AppointmentEndTime"].ToString()
                                               }).ToList();



            //return _TicketDataAccess.Get(TicketId);
            return AssignTicket;
        }
        public List<TicketUser> GetTicketUserListByTicketId(Guid ticketId)
        {

            DataTable Dt = _TicketDataAccess.GetTicketUserListByTicketId(ticketId);

            List<TicketUser> TicketUsers = (from DataRow dr in Dt.Rows
                                            select new TicketUser()
                                            {
                                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                TiketId = (Guid)dr["TiketId"],
                                                UserId = (Guid)dr["UserId"],
                                                AddedBy = (Guid)dr["UserId"],
                                                AddedDate = dr["AddedDate"] != DBNull.Value ? Convert.ToDateTime(dr["AddedDate"]) : new DateTime(),
                                                IsPrimary = dr["IsPrimary"] != DBNull.Value ? Convert.ToBoolean(dr["IsPrimary"]) : false,
                                                NotificationOnly = dr["NotificationOnly"] != DBNull.Value ? Convert.ToBoolean(dr["NotificationOnly"]) : false,
                                                FullName = dr["FullName"].ToString()
                                            }).ToList();
            return TicketUsers;
        }
        public Ticket GetAllTicketEstimatorByCustomerId(Guid customerid)
        {
            return _TicketDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerid)).FirstOrDefault();
        }
        public Ticket GetTicketById(int TicketId)
        {
            DataSet ds = _TicketDataAccess.GetTicketById(TicketId);

            Ticket Tickets = (from DataRow dr in ds.Tables[0].Rows
                              select new Ticket()
                              {
                                  Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                  CompanyId = (Guid)dr["CompanyId"],
                                  CustomerId = (Guid)dr["CustomerId"],
                                  TicketId = (Guid)dr["TicketId"],
                                  LastUpdatedBy = (Guid)dr["LastUpdatedBy"],
                                  CreatedBy = (Guid)dr["CreatedBy"],
                                  MiscName = dr["MiscName"].ToString(),
                                  MiscValue = dr["MiscValue"] != DBNull.Value ? Convert.ToDecimal(dr["MiscValue"]) : 0,
                                  Status = dr["Status"].ToString(),
                                  TicketType = dr["TicketType"].ToString(),
                                  AssignedTo = dr["AssignedTo"].ToString(),
                                  Priority = dr["Priority"].ToString(),
                                  Subject = dr["Subject"].ToString(),
                                  LastUpdatedDate = dr["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["LastUpdatedDate"]) : new DateTime(),
                                  CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                                  CompletionDate = dr["CompletionDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletionDate"]) : new DateTime(),
                                  CompletedDate = dr["CompletedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletedDate"]) : new DateTime(),
                                  Message = dr["Message"].ToString(),
                                  TicketTypeVal = dr["TicketTypeVal"].ToString(),
                                  StatusVal = dr["StatusVal"].ToString(),
                                  PriorityVal = dr["PriorityVal"].ToString(),
                                  CreatedByVal = dr["CreatedByVal"].ToString(),
                                  HasInvoice = dr["HasInvoice"] != DBNull.Value ? Convert.ToBoolean(dr["HasInvoice"]) : false,
                                  HasSurvey = dr["HasSurvey"] != DBNull.Value ? Convert.ToBoolean(dr["HasSurvey"]) : false,
                                  IsClosed = dr["IsClosed"] != DBNull.Value ? Convert.ToBoolean(dr["IsClosed"]) : false,
                                  IsAgreementTicket = dr["IsAgreementTicket"] != DBNull.Value ? Convert.ToBoolean(dr["IsAgreementTicket"]) : false,
                                  IsPayrollClosed = dr["IsPayrollClosed"] != DBNull.Value ? Convert.ToBoolean(dr["IsPayrollClosed"]) : false,
                                  Signature = dr["Signature"].ToString(),
                                  IsDispatch = dr["IsDispatch"] != DBNull.Value ? Convert.ToBoolean(dr["IsDispatch"]) : false,
                                  AssignedToId = (Guid)dr["AssignedToId"],
                                  ReferenceTicketId = dr["ReferenceTicketId"] != DBNull.Value ? Convert.ToInt32(dr["ReferenceTicketId"]) : 0,
                                  BookingId = dr["BookingId"].ToString(),
                                  Reason = dr["Reason"].ToString(),
                                  RackNo = dr["RackNo"].ToString(),
                                  Locations = dr["Locations"].ToString(),
                                  RescheduleTicketId = dr["RescheduleTicketId"] != DBNull.Value ? Convert.ToInt32(dr["RescheduleTicketId"]) : 0,
                                  TicketSignatureDate = dr["TicketSignatureDate"] != DBNull.Value ? Convert.ToDateTime(dr["TicketSignatureDate"]) : new DateTime(),
                              }).FirstOrDefault();



            //return _TicketDataAccess.Get(TicketId);
            return Tickets;
        }
        public Ticket GetTicketByTicketId(Guid ticketId)
        {
            return _TicketDataAccess.GetTicketByTicketId(ticketId);
            //return _TicketDataAccess.GetByQuery(string.Format("TicketId='{0}'", ticketId)).FirstOrDefault();
        }
        public bool UpdateAllTicketIsAgreementFalseByCustomerId(Guid CustomerId)
        {
            return _TicketDataAccess.UpdateAllTicketIsAgreementFalseByCustomerId(CustomerId);
        }
        public int InsertTicket(Ticket Ticket)
        {
            return (int)_TicketDataAccess.Insert(Ticket);
        }

        public bool UpdateTicket(Ticket ticket)
        {
            return _TicketDataAccess.Update(ticket) > 0;
        }
        public List<TicketUser> GetTicketUserByTicketId(Guid ticketId)
        {
            return _TicketUserDataAccess.GetByQuery(string.Format("TiketId = '{0}'", ticketId));
        }
        public List<TicketUser> GetTicketAssignedUserListByTicketId(Guid ticketId)
        {
            return _TicketUserDataAccess.GetByQuery(string.Format("TiketId = '{0}' and IsPrimary =1", ticketId));
        }

        public double GetTicketServiceFeeTotal(Guid ticketId)
        {
            double TicketServiceFeeTotal = 0;
            DataTable dt = _CustomerAppointmentEquipmentDataAccess.GetCustomerTicketServiceByTicketId(ticketId);
            if (dt.Rows.Count > 0)
            {
                var TicketServiceFeeString = dt.Rows[0]["ServiceFee"].ToString();
                double.TryParse(TicketServiceFeeString, out TicketServiceFeeTotal);
            }
            return TicketServiceFeeTotal;

        }
        public List<TicketUser> GetTicketAssignedUserListByUserId(Guid UserId)
        {
            return _TicketUserDataAccess.GetByQuery(string.Format("UserId = '{0}' and IsPrimary =1"));
        }
        public List<TicketUser> GetTicketAddtionalUsersByTicketId(Guid ticketId)
        {
            return _TicketUserDataAccess.GetByQuery(string.Format("TiketId = '{0}' and IsPrimary =0", ticketId));
        }


        public List<TicketUser> GetOnlyTicketAddtionalUsersByTicketId(Guid ticketId)
        {
            return _TicketUserDataAccess.GetByQuery(string.Format("TiketId = '{0}' and IsPrimary =0 and NotificationOnly = 0", ticketId));
        }
        public Ticket GetAgreementTicketByCustomerId(Guid CustomerId)
        {
            var query = string.Format("CustomerId='{0}' AND IsAgreementTicket=1", CustomerId);
            return _TicketDataAccess.GetByQuery(query).FirstOrDefault();
        }
        public int InsertTicketUser(TicketUser ticketUser)
        {
            return (int)_TicketUserDataAccess.Insert(ticketUser);
        }

        public bool UpdateTicketUser(TicketUser ticketUser)
        {
            return _TicketUserDataAccess.Update(ticketUser) > 0;
        }

        public bool DeleteTicketUserByTicketId(Guid ticketId, bool? IsAssigned, bool NotifyCheck = false)
        {
            return _TicketUserDataAccess.DeleteTicketUserByTicketId(ticketId, IsAssigned, NotifyCheck);
        }

        public bool DeleteTicketReplyById(int id)
        {
            return _TicketReplyDataAccess.Delete(id) > 0;
        }

        public int InsertTicketReply(TicketReply tR)
        {
            return (int)_TicketReplyDataAccess.Insert(tR);
        }

        public int InsertTicketFile(TicketFile ticketFile)
        {
            return (int)_TicketFileDataAccess.Insert(ticketFile);
        }

        public TicketCounter GetTicketCountByCompanyId(Guid comid, string starttime, string endtime, string date, Guid empid)
        {
            DataTable dt = _TicketDataAccess.GetTicketCountByCompanyId(comid, starttime, endtime, date, empid);
            TicketCounter ticketCount = new TicketCounter();
            ticketCount = (from DataRow dr in dt.Rows
                           select new TicketCounter()
                           {
                               TicketAppCounter = dr["TicketAppCounter"] != DBNull.Value ? Convert.ToInt32(dr["TicketAppCounter"]) : 0
                           }).FirstOrDefault();
            return ticketCount;
        }

        public TicketCounter1 GetTicketCount1ByCompanyId(Guid comid, string starttime, string endtime, string date, Guid empid)
        {
            DataTable dt = _TicketDataAccess.GetTicketCount1ByCompanyId(comid, starttime, endtime, date, empid);
            TicketCounter1 ticketCount = new TicketCounter1();
            ticketCount = (from DataRow dr in dt.Rows
                           select new TicketCounter1()
                           {
                               TicketAppCounter1 = dr["TicketAppCounter1"] != DBNull.Value ? Convert.ToInt32(dr["TicketAppCounter1"]) : 0
                           }).FirstOrDefault();
            return ticketCount;
        }

        public TicketCounterUser GetTicketCountUserByCompanyId(Guid comid, string starttime, string endtime, string date, Guid empid)
        {
            DataTable dt = _TicketDataAccess.GetTicketCountUserByCompanyId(comid, starttime, endtime, date, empid);
            TicketCounterUser ticketCount = new TicketCounterUser();
            ticketCount = (from DataRow dr in dt.Rows
                           select new TicketCounterUser()
                           {
                               TicketUserCounter = dr["TicketUserCounter"] != DBNull.Value ? Convert.ToInt32(dr["TicketUserCounter"]) : 0
                           }).FirstOrDefault();
            return ticketCount;
        }

        public List<TicketSchedule> GetTicketSchedulesByUserListAndAppoinmentDate(List<Guid> assignedList, DateTime AppoinmentDate)
        {
            DataTable dt = _TicketDataAccess.GetTicketSchedulesByUserListAndAppoinmentDate(assignedList, AppoinmentDate);

            List<TicketSchedule> TicketSchedule = (from DataRow dr in dt.Rows
                                                   select new TicketSchedule()
                                                   {
                                                       TicketId = (Guid)dr["TicketId"],
                                                       EmployeeName = dr["EmployeeName"].ToString(),
                                                       EndTime = dr["EndTime"].ToString(),
                                                       StartTime = dr["StartTime"].ToString(),
                                                   }).ToList();
            return TicketSchedule;
        }

        public List<Ticket> GetCustomerOverviewInformation(Guid comid, Guid cusid)
        {
            DataTable dt = _TicketDataAccess.GetCustomerOverviewInformation(comid, cusid);

            List<Ticket> TicketSchedule = (from DataRow dr in dt.Rows
                                           select new Ticket()
                                           {
                                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                               TicketType = dr["TicketType"].ToString(),
                                               TicketReplyCount = dr["TicketReplyCount"] != DBNull.Value ? Convert.ToInt32(dr["TicketReplyCount"]) : 0,
                                               CompletionDate = dr["CompletionDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletionDate"]) : new DateTime(),
                                               TicketUserName = dr["TicketUserName"].ToString(),
                                               AppointmentStartTime = dr["AppointmentStartTime"].ToString(),
                                               AppointmentEndTime = dr["AppointmentEndTime"].ToString(),
                                               Address = dr["Address"].ToString()
                                           }).ToList();
            return TicketSchedule;
        }
        public SalesPay GetSalesPayCalculationByTicketId(Guid ticketId)
        {
            DataTable dt = _EmployeeDataAccess.GetSalesPayCalculationByTicketId(ticketId);
            List<SalesPay> salesPay = new List<SalesPay>();
            salesPay = (from DataRow dr in dt.Rows
                        select new SalesPay()
                        {
                            TotalMultiple = dr["TotalMultiple"] != DBNull.Value ? Convert.ToDouble(dr["TotalMultiple"]) : 0,
                            Deductions = dr["Deductions"] != DBNull.Value ? Convert.ToDouble(dr["Deductions"]) : 0,
                            PassThrus = dr["PassThrus"] != DBNull.Value ? Convert.ToDouble(dr["PassThrus"]) : 0,
                            HoldBack = dr["HoldBack"] != DBNull.Value ? Convert.ToDouble(dr["HoldBack"]) : 0,
                            TermSheetId = (Guid)dr["TermSheetId"]
                        }).ToList();
            return salesPay.FirstOrDefault();
        }

        public bool InsertSalesCommission(SalesCommission salescommission)
        {
            return _SalesCommissionDataAccess.Insert(salescommission) > 0;
        }
        public SalesCommission GetSalesCommissionByTicketIdUserId(Guid TicketId, Guid UserId)
        {
            string query = "IsPermanent != 1 AND TicketId='" + TicketId + "' AND UserId='" + UserId + "'";
            return _SalesCommissionDataAccess.GetByQuery(query).FirstOrDefault();
        }
        public SalesCommission GetSalesMoveCommissionByTicketIdUserId(Guid TicketId, Guid UserId)
        {
            string query = "RMRCommissionCalculation='Moved' AND TicketId='" + TicketId + "' AND UserId='" + UserId + "'";
            return _SalesCommissionDataAccess.GetByQuery(query).FirstOrDefault();
        }
        public bool UpdateSalesCommission(SalesCommission salescommission)
        {
            return _SalesCommissionDataAccess.Update(salescommission) > 0;
        }
        public List<SalesCommission> GetSalesCommissionByTicketId(Guid ticketId, Guid CommissionUserId)
        {
            DataTable dt = _SalesCommissionDataAccess.GetSalesCommissionByTicketId(ticketId, CommissionUserId);

            List<SalesCommission> SalesCommissionList = (from DataRow dr in dt.Rows
                                                         select new SalesCommission()
                                                         {
                                                             Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                             SalesPerson = dr["SalesPerson"].ToString(),
                                                             RMRSold = dr["RMRSold"] != DBNull.Value ? Convert.ToDouble(dr["RMRSold"]) : 0,
                                                             RMRCommission = dr["RMRCommission"] != DBNull.Value ? Convert.ToDouble(dr["RMRCommission"]) : 0,
                                                             RMRCommissionCalculation = dr["RMRCommissionCalculation"].ToString(),
                                                             NoOfEquipment = dr["NoOfEquipment"] != DBNull.Value ? Convert.ToInt32(dr["NoOfEquipment"]) : 0,
                                                             EquipmentCommission = dr["EquipmentCommission"] != DBNull.Value ? Convert.ToDouble(dr["EquipmentCommission"]) : 0,
                                                             EquipmentCommissionCalculation = dr["EquipmentCommissionCalculation"].ToString(),
                                                             TotalCommission = dr["TotalCommission"] != DBNull.Value ? Convert.ToDouble(dr["TotalCommission"]) : 0,
                                                             Adjustment = dr["Adjustment"] != DBNull.Value ? Convert.ToDouble(dr["Adjustment"]) : 0,
                                                             SalesCommissionId = (Guid)dr["SalesCommissionId"],
                                                             OriginalPoint = dr["OriginalPoint"] != DBNull.Value ? Convert.ToDouble(dr["OriginalPoint"]) : 0,
                                                             AdjustablePoint = dr["AdjustablePoint"] != DBNull.Value ? Convert.ToDouble(dr["AdjustablePoint"]) : 0,
                                                         }).ToList();
            return SalesCommissionList;
        }
        public bool DeleteSalesCommissionByTicketId(Guid ticketId)
        {
            return _SalesCommissionDataAccess.DeleteSalesCommissionByTicketId(ticketId);
        }
        public bool DeleteExtraSalesCommission(Guid ticketId, string subquery)
        {
            return _SalesCommissionDataAccess.DeleteExtraSalesCommission(ticketId, subquery);
        }
        public bool DeleteExtraTechCommission(Guid ticketId, string userId)
        {
            return _TechCommissionDataAccess.DeleteExtraTechCommission(ticketId, userId);
        }
        public List<TechCommission> GetTechCommissionByTicketId(Guid ticketId, Guid CommissionUserId)
        {
            DataTable dt = _TechCommissionDataAccess.GetTechCommissionByTicketId(ticketId, CommissionUserId);

            List<TechCommission> TechCommissionList = (from DataRow dr in dt.Rows
                                                       select new TechCommission()
                                                       {
                                                           Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                           Technician = dr["Technician"].ToString(),
                                                           BaseRMR = dr["BaseRMR"] != DBNull.Value ? Convert.ToDouble(dr["BaseRMR"]) : 0,
                                                           BaseRMRCommission = dr["BaseRMRCommission"] != DBNull.Value ? Convert.ToDouble(dr["BaseRMRCommission"]) : 0,
                                                           BaseRMRCommissionCalculation = dr["BaseRMRCommissionCalculation"].ToString(),
                                                           AddedRMR = dr["AddedRMR"] != DBNull.Value ? Convert.ToInt32(dr["AddedRMR"]) : 0,
                                                           AddedRMRCommission = dr["AddedRMRCommission"] != DBNull.Value ? Convert.ToDouble(dr["AddedRMRCommission"]) : 0,
                                                           AddedRMRCommissionCalculation = dr["AddedRMRCommissionCalculation"].ToString(),
                                                           TotalCommission = dr["TotalCommission"] != DBNull.Value ? Convert.ToDouble(dr["TotalCommission"]) : 0,
                                                           TechCommissionId = (Guid)dr["TechCommissionId"],
                                                           Adjustment = dr["Adjustment"] != DBNull.Value ? Convert.ToDouble(dr["Adjustment"]) : 0,
                                                           OriginalPoint = dr["OriginalPoint"] != DBNull.Value ? Convert.ToDouble(dr["OriginalPoint"]) : 0,
                                                           AdjustablePoint = dr["AdjustablePoint"] != DBNull.Value ? Convert.ToDouble(dr["AdjustablePoint"]) : 0,
                                                       }).ToList();
            return TechCommissionList;
        }
        public bool InsertTechCommission(TechCommission techcommission)
        {
            return _TechCommissionDataAccess.Insert(techcommission) > 0;
        }
        public TechCommission GetTechCommissionByTicketIdUserId(Guid TicketId, Guid UserId)
        {
            string query = "TicketId='" + TicketId + "' AND UserId='" + UserId + "' AND BaseRMR is not NULL AND AddedRMR is not NULL";
            return _TechCommissionDataAccess.GetByQuery(query).FirstOrDefault();
        }
        public bool UpdateTechCommission(TechCommission techcommission)
        {
            return _TechCommissionDataAccess.Update(techcommission) > 0;
        }
        public FinRepCommission GetFinRepCommissionByTicketId(Guid ticketId)
        {
            string query = string.Format("TicketId='{0}'", ticketId);
            return _FinRepCommissionDataAccess.GetByQuery(query).FirstOrDefault();
        }
        public List<FinRepCommission> GetFinRepCommissionListByTicketId(Guid ticketId,Guid CommissionUserId)
        {
            DataTable dt = _FinRepCommissionDataAccess.GetFinRepCommissionByTicketId(ticketId, CommissionUserId);
            List<FinRepCommission> FinRepCommissionList = (from DataRow dr in dt.Rows
                                                                 select new FinRepCommission()
                                                                 {
                                                                     Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                                     FinanceRep = dr["FinanceRep"].ToString(),
                                                                     Commission = dr["Commission"] != DBNull.Value ? Convert.ToDouble(dr["Commission"]) : 0,
                                                                     Adjustment = dr["Adjustment"] != DBNull.Value ? Convert.ToDouble(dr["Adjustment"]) : 0,
                                                                     FinRepCommissionId = (Guid)dr["FinRepCommissionId"],
                                                                     UserId = (Guid)dr["UserId"],
                                                                     OriginalPoint = dr["OriginalPoint"] != DBNull.Value ? Convert.ToDouble(dr["OriginalPoint"]) : 0,
                                                                     AdjustablePoint = dr["AdjustablePoint"] != DBNull.Value ? Convert.ToDouble(dr["AdjustablePoint"]) : 0
                                                                 }).ToList();
            return FinRepCommissionList;
        }
        public bool InsertFinRepCommission(FinRepCommission finRepcommission)
        {
            return _FinRepCommissionDataAccess.Insert(finRepcommission) > 0;
        }
        public bool UpdateFinRepCommission(FinRepCommission finRepcommission)
        {
            return _FinRepCommissionDataAccess.Update(finRepcommission) > 0;
        }
        public List<AddMemberCommission> GetAddMemberCommissionByTicketId(Guid ticketId, Guid CommissionUserId)
        {
            DataTable dt = _AddMemberCommissionDataAccess.GetAddMemberCommissionByTicketId(ticketId, CommissionUserId);

            List<AddMemberCommission> AddMemberCommissionList = (from DataRow dr in dt.Rows
                                                                 select new AddMemberCommission()
                                                                 {
                                                                     Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                                     Technician = dr["Technician"].ToString(),
                                                                     Commission = dr["Commission"] != DBNull.Value ? Convert.ToDouble(dr["Commission"]) : 0,
                                                                     Adjustment = dr["Adjustment"] != DBNull.Value ? Convert.ToDouble(dr["Adjustment"]) : 0,
                                                                     AddMemberCommissionId = (Guid)dr["AddMemberCommissionId"],
                                                                     UserId = (Guid)dr["UserId"],
                                                                     OriginalPoint = dr["OriginalPoint"] != DBNull.Value ? Convert.ToDouble(dr["OriginalPoint"]) : 0,
                                                                     AdjustablePoint = dr["AdjustablePoint"] != DBNull.Value ? Convert.ToDouble(dr["AdjustablePoint"]) : 0
                                                                 }).ToList();
            return AddMemberCommissionList;
        }
        public List<AddMemberCommission> GetMemberCommissionListByIdList(List<int> idList)
        {
            StringBuilder strHTMLContent = new StringBuilder();
            strHTMLContent.Append(string.Join(",", idList));
            var query = string.Format(" TicketId in ('{0}') and IsPaid = 0", strHTMLContent.ToString());
            return _AddMemberCommissionDataAccess.GetByQuery(query);
        }
        public List<AddMemberCommission> GetMemberCommissionListByTicketIdList(List<Guid> idList)
        {
            return _AddMemberCommissionDataAccess.GetByQuery(string.Format(" TicketId in ('{0}') and IsPaid = 0", string.Join(",", idList)));
        }
        public List<FollowUpCommission> GetFollowUpCommissionListByIdList(List<int> idList)
        {
            StringBuilder strHTMLContent = new StringBuilder();
            strHTMLContent.Append(string.Join(",", idList));
            var query = string.Format(" TicketId in ('{0}') and IsPaid = 0", strHTMLContent.ToString());
            return _FollowUpCommissionDataAccess.GetByQuery(query);
        }
        public List<FollowUpCommission> GetFollowUpCommissionListByTicketIdList(List<Guid> idList)
        {
            return _FollowUpCommissionDataAccess.GetByQuery(string.Format(" TicketId in ('{0}') and IsPaid = 0", string.Join(",", idList)));
        }
        public List<RescheduleCommission> GetRescheduleCommissionListByIdList(List<int> idList)
        {
            StringBuilder strHTMLContent = new StringBuilder();
            strHTMLContent.Append(string.Join(",", idList));
            var query = string.Format(" TicketId in ('{0}') and IsPaid = 0", strHTMLContent.ToString());
            return _RescheduleCommissionDataAccess.GetByQuery(query);
        }
        public List<RescheduleCommission> GetRescheduleCommissionListByTicketIdList(List<Guid> idList)
        {
            return _RescheduleCommissionDataAccess.GetByQuery(string.Format(" TicketId in ('{0}') and IsPaid = 0", string.Join(",", idList)));
        }
        public List<AdjustmentFunding> GetAdjustFundingListByIdList(List<int> idList)
        {
            return _AdjustmentFundingDataAccess.GetByQuery(string.Format(" Id in ({0})", string.Join(",", idList)));
        }
        public List<ServiceCallCommission> GetServiceCallCommissionListByIdList(List<int> idList)
        {
            StringBuilder strHTMLContent = new StringBuilder();
            strHTMLContent.Append(string.Join(",", idList));
            var query = string.Format(" TicketId in ('{0}') and IsPaid = 0", strHTMLContent.ToString());
            return _ServiceCallCommissionDataAccess.GetByQuery(query);
        }

        public List<ServiceCallCommission> GetServiceCallCommissionListByTicketIdList(List<Guid> idList)
        {
            return _ServiceCallCommissionDataAccess.GetByQuery(string.Format(" TicketId in ('{0}') and IsPaid = 0", string.Join(",", idList)));
        }
        public int GetLastMemberBatchNo()
        {
            return _AddMemberCommissionDataAccess.GetLastMemberBatchNo();
        }

        public int GetLastServiceCallBatchNo()
        {
            return _ServiceCallCommissionDataAccess.GetLastServiceCallBatchNo();
        }
        public MemberCommisionReport GetAllMemberCommisionReport(DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize, bool IsPaid, Guid UserId, string SearchText, string MemberList)
        {
            DataSet ds = _AddMemberCommissionDataAccess.GetAllMemberCommReport(FilterStartDate, FilterEndDate, order, pageno, pagesize, IsPaid, UserId, SearchText, MemberList);
            List<AddMemberCommission> buildList = new List<AddMemberCommission>();
            buildList = (from DataRow dr in ds.Tables[1].Rows
                         select new AddMemberCommission()
                         {
                             Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                             CustomerName = dr["CustomerName"].ToString(),
                             TicketIdValue = dr["ticketIdValue"] != DBNull.Value ? Convert.ToInt32(dr["ticketIdValue"]) : 0,
                             Technician = dr["Technician"].ToString(),
                             CompletionDate = dr["CompletionDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletionDate"]) : new DateTime(),

                             CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                             Commission = dr["Commission"] != DBNull.Value ? Convert.ToDouble(dr["Commission"]) : 0,

                             IsPaid = (dr["IsPaid"] != DBNull.Value ? Convert.ToBoolean(dr["IsPaid"]) : false),
                             CreatedBy = (Guid)dr["CreatedBy"],
                             BalanceDue = dr["BalanceDue"] != DBNull.Value ? Convert.ToDouble(dr["BalanceDue"]) : 0,
                             Batch = dr["Batch"].ToString(),
                             CustomerIdValue = dr["CustomerIdValue"] != DBNull.Value ? Convert.ToInt32(dr["CustomerIdValue"]) : 0,
                             Adjustment = dr["Adjustment"] != DBNull.Value ? Convert.ToDouble(dr["Adjustment"]) : 0
                         }).ToList();

            PayrollTotalCount PayrollTotalCount = new PayrollTotalCount();
            PayrollTotalCount = (from DataRow dr in ds.Tables[0].Rows
                                 select new PayrollTotalCount()
                                 {
                                     CountTotal = dr["CountTotal"] != DBNull.Value ? Convert.ToInt32(dr["CountTotal"]) : 0
                                 }).FirstOrDefault();
            TotalPayrollSum TotalMemberCommisionCount = new TotalPayrollSum();
            TotalMemberCommisionCount = (from DataRow dr in ds.Tables[2].Rows
                                         select new TotalPayrollSum()
                                         {
                                             TotalAdjustment = dr["TotalAdjustment"] != DBNull.Value ? Convert.ToDouble(dr["TotalAdjustment"]) : 0.0,
                                             TotalCommission = dr["SumCommission"] != DBNull.Value ? Convert.ToDouble(dr["SumCommission"]) : 0.0,
                                             TotalUnpaidBalance = dr["SumUnpaid"] != DBNull.Value ? Convert.ToDouble(dr["SumUnpaid"]) : 0.0,
                                         }).FirstOrDefault();
            MemberCommisionReport MemberPayrollFilter = new MemberCommisionReport();
            MemberPayrollFilter.PayrollReportList = buildList;
            MemberPayrollFilter.PayrollTotalCount = PayrollTotalCount;
            MemberPayrollFilter.TotalMemberCommisionCount = TotalMemberCommisionCount;
            return MemberPayrollFilter;
        }

        public DataTable GetDownLoadAllMemberCommisionReport(DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize, bool IsPaid, Guid UserId, string SearchText, string MemberList)
        {
            DataSet ds = _AddMemberCommissionDataAccess.GetDownLoadAllMemberCommReport(FilterStartDate, FilterEndDate, order, pageno, pagesize, IsPaid, UserId, SearchText, MemberList);
            //List<AddMemberCommission> buildList = new List<AddMemberCommission>();
            DataTable buildList = ds.Tables[0];

            return buildList;
        }
        public DataTable GetDownLoadServiceCallCommReport(DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize, bool IsPaid, Guid UserId, string SearchText, string MemberList)
        {
            DataSet ds = _AddMemberCommissionDataAccess.GetDownLoadServiceCallCommReport(FilterStartDate, FilterEndDate, order, pageno, pagesize, IsPaid, UserId, SearchText, MemberList);
            //List<AddMemberCommission> buildList = new List<AddMemberCommission>();
            DataTable buildList = ds.Tables[0];

            return buildList;
        }
        public DataTable GetDownLoadAllFollowUpCommisionReport(DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize, bool IsPaid, Guid UserId, string SearchText, string MemberList)
        {
            DataSet ds = _AddMemberCommissionDataAccess.GetDownLoadFollowUpCommReport(FilterStartDate, FilterEndDate, order, pageno, pagesize, IsPaid, UserId, SearchText, MemberList);
            //List<AddMemberCommission> buildList = new List<AddMemberCommission>();
            DataTable buildList = ds.Tables[0];

            return buildList;
        }

        public DataTable GetDownLoadRescheduleCommReport(DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize, bool IsPaid, Guid UserId, string SearchText, string MemberList)
        {
            DataSet ds = _AddMemberCommissionDataAccess.GetDownLoadRescheduleCommReport(FilterStartDate, FilterEndDate, order, pageno, pagesize, IsPaid, UserId, SearchText, MemberList);
            //List<AddMemberCommission> buildList = new List<AddMemberCommission>();
            DataTable buildList = ds.Tables[0];

            return buildList;
        }


        public ServiceCallCommisionReport GetAllServiceCallCommisionReport(DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize, bool IsPaid, Guid UserId, string SearchText, string ServicePersonList)
        {
            DataSet ds = _AddMemberCommissionDataAccess.GetAllServiceCallReport(FilterStartDate, FilterEndDate, order, pageno, pagesize, IsPaid, UserId, SearchText, ServicePersonList);
            List<ServiceCallCommission> buildList = new List<ServiceCallCommission>();
            buildList = (from DataRow dr in ds.Tables[1].Rows
                         select new ServiceCallCommission()
                         {
                             Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                             CustomerName = dr["CustomerName"].ToString(),
                             TicketIdValue = dr["ticketIdValue"] != DBNull.Value ? Convert.ToInt32(dr["ticketIdValue"]) : 0,
                             Technician = dr["Technician"].ToString(),
                             CompletionDate = dr["CompletionDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletionDate"]) : new DateTime(),

                             CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                             Commission = dr["Commission"] != DBNull.Value ? Convert.ToDouble(dr["Commission"]) : 0,

                             IsPaid = (dr["IsPaid"] != DBNull.Value ? Convert.ToBoolean(dr["IsPaid"]) : false),
                             CreatedBy = (Guid)dr["CreatedBy"],
                             BalanceDue = dr["BalanceDue"] != DBNull.Value ? Convert.ToDouble(dr["BalanceDue"]) : 0,
                             Batch = dr["Batch"].ToString(),
                             CustomerIdValue = dr["CustomerIdValue"] != DBNull.Value ? Convert.ToInt32(dr["CustomerIdValue"]) : 0,
                             Adjustment = dr["Adjustment"] != DBNull.Value ? Convert.ToDouble(dr["Adjustment"]) : 0
                         }).ToList();

            PayrollTotalCount PayrollTotalCount = new PayrollTotalCount();
            PayrollTotalCount = (from DataRow dr in ds.Tables[0].Rows
                                 select new PayrollTotalCount()
                                 {
                                     CountTotal = dr["CountTotal"] != DBNull.Value ? Convert.ToInt32(dr["CountTotal"]) : 0
                                 }).FirstOrDefault();
            TotalPayrollSum TotalServiceCallCount = new TotalPayrollSum();
            TotalServiceCallCount = (from DataRow dr in ds.Tables[2].Rows
                                     select new TotalPayrollSum()
                                     {
                                         TotalAdjustment = dr["TotalAdjustment"] != DBNull.Value ? Convert.ToDouble(dr["TotalAdjustment"]) : 0.0,
                                         TotalCommission = dr["SumCommission"] != DBNull.Value ? Convert.ToDouble(dr["SumCommission"]) : 0.0,
                                         TotalUnpaidBalance = dr["SumUnpaid"] != DBNull.Value ? Convert.ToDouble(dr["SumUnpaid"]) : 0.0,
                                     }).FirstOrDefault();
            ServiceCallCommisionReport ServiceCalPayrollFilter = new ServiceCallCommisionReport();
            ServiceCalPayrollFilter.PayrollReportList = buildList;
            ServiceCalPayrollFilter.PayrollTotalCount = PayrollTotalCount;
            ServiceCalPayrollFilter.TotalServiceCallCount = TotalServiceCallCount;
            return ServiceCalPayrollFilter;
        }
        public FollowUpCommisionReport GetAllFollowUpCommisionReport(DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize, bool IsPaid, Guid UserId, string SearchText, string FollowUpPersonList)
        {
            DataSet ds = _AddMemberCommissionDataAccess.GetAllFollowUpReport(FilterStartDate, FilterEndDate, order, pageno, pagesize, IsPaid, UserId, SearchText, FollowUpPersonList);
            List<FollowUpCommission> buildList = new List<FollowUpCommission>();
            buildList = (from DataRow dr in ds.Tables[1].Rows
                         select new FollowUpCommission()
                         {
                             Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                             CustomerName = dr["CustomerName"].ToString(),
                             TicketIdValue = dr["ticketIdValue"] != DBNull.Value ? Convert.ToInt32(dr["ticketIdValue"]) : 0,
                             Technician = dr["Technician"].ToString(),
                             CompletionDate = dr["CompletionDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletionDate"]) : new DateTime(),

                             CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                             Commission = dr["Commission"] != DBNull.Value ? Convert.ToDouble(dr["Commission"]) : 0,

                             IsPaid = (dr["IsPaid"] != DBNull.Value ? Convert.ToBoolean(dr["IsPaid"]) : false),
                             CreatedBy = (Guid)dr["CreatedBy"],
                             BalanceDue = dr["BalanceDue"] != DBNull.Value ? Convert.ToDouble(dr["BalanceDue"]) : 0,
                             Batch = dr["Batch"].ToString(),
                             CustomerIdValue = dr["CustomerIdValue"] != DBNull.Value ? Convert.ToInt32(dr["CustomerIdValue"]) : 0,
                             Adjustment = dr["Adjustment"] != DBNull.Value ? Convert.ToDouble(dr["Adjustment"]) : 0
                         }).ToList();

            PayrollTotalCount PayrollTotalCount = new PayrollTotalCount();
            PayrollTotalCount = (from DataRow dr in ds.Tables[0].Rows
                                 select new PayrollTotalCount()
                                 {
                                     CountTotal = dr["CountTotal"] != DBNull.Value ? Convert.ToInt32(dr["CountTotal"]) : 0
                                 }).FirstOrDefault();
            TotalPayrollSum TotalFollowUp = new TotalPayrollSum();
            TotalFollowUp = (from DataRow dr in ds.Tables[2].Rows
                             select new TotalPayrollSum()
                             {
                                 TotalAdjustment = dr["TotalAdjustment"] != DBNull.Value ? Convert.ToDouble(dr["TotalAdjustment"]) : 0.0,
                                 TotalCommission = dr["SumCommission"] != DBNull.Value ? Convert.ToDouble(dr["SumCommission"]) : 0.0,
                                 TotalUnpaidBalance = dr["SumUnpaid"] != DBNull.Value ? Convert.ToDouble(dr["SumUnpaid"]) : 0.0,
                             }).FirstOrDefault();
            FollowUpCommisionReport FollowUpPayrollFilter = new FollowUpCommisionReport();
            FollowUpPayrollFilter.PayrollReportList = buildList;
            FollowUpPayrollFilter.PayrollTotalCount = PayrollTotalCount;
            FollowUpPayrollFilter.TotalFollowUp = TotalFollowUp;
            return FollowUpPayrollFilter;
        }
        public RescheduleCommisionReport GetAllRescheduleCommisionReport(DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize, bool IsPaid, Guid UserId, string SearchText, string RescheduleTech)
        {
            DataSet ds = _AddMemberCommissionDataAccess.GetAllRescheduleReport(FilterStartDate, FilterEndDate, order, pageno, pagesize, IsPaid, UserId, SearchText, RescheduleTech);
            List<RescheduleCommission> buildList = new List<RescheduleCommission>();
            buildList = (from DataRow dr in ds.Tables[1].Rows
                         select new RescheduleCommission()
                         {
                             Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                             CustomerName = dr["CustomerName"].ToString(),
                             TicketIdValue = dr["ticketIdValue"] != DBNull.Value ? Convert.ToInt32(dr["ticketIdValue"]) : 0,
                             Technician = dr["Technician"].ToString(),
                             CompletionDate = dr["CompletionDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletionDate"]) : new DateTime(),

                             CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                             Commission = dr["Commission"] != DBNull.Value ? Convert.ToDouble(dr["Commission"]) : 0,

                             IsPaid = (dr["IsPaid"] != DBNull.Value ? Convert.ToBoolean(dr["IsPaid"]) : false),
                             CreatedBy = (Guid)dr["CreatedBy"],
                             BalanceDue = dr["BalanceDue"] != DBNull.Value ? Convert.ToDouble(dr["BalanceDue"]) : 0,
                             Batch = dr["Batch"].ToString(),
                             CustomerIdValue = dr["CustomerIdValue"] != DBNull.Value ? Convert.ToInt32(dr["CustomerIdValue"]) : 0,
                             Adjustment = dr["Adjustment"] != DBNull.Value ? Convert.ToDouble(dr["Adjustment"]) : 0
                         }).ToList();

            PayrollTotalCount PayrollTotalCount = new PayrollTotalCount();
            PayrollTotalCount = (from DataRow dr in ds.Tables[0].Rows
                                 select new PayrollTotalCount()
                                 {
                                     CountTotal = dr["CountTotal"] != DBNull.Value ? Convert.ToInt32(dr["CountTotal"]) : 0
                                 }).FirstOrDefault();
            TotalPayrollSum TotalRescheduleCommision = new TotalPayrollSum();
            TotalRescheduleCommision = (from DataRow dr in ds.Tables[2].Rows
                                        select new TotalPayrollSum()
                                        {
                                            TotalAdjustment = dr["TotalAdjustment"] != DBNull.Value ? Convert.ToDouble(dr["TotalAdjustment"]) : 0.0,
                                            TotalCommission = dr["SumCommission"] != DBNull.Value ? Convert.ToDouble(dr["SumCommission"]) : 0.0,
                                            TotalUnpaidBalance = dr["SumUnpaid"] != DBNull.Value ? Convert.ToDouble(dr["SumUnpaid"]) : 0.0,
                                        }).FirstOrDefault();
            RescheduleCommisionReport ReschedulePayrollFilter = new RescheduleCommisionReport();
            ReschedulePayrollFilter.PayrollReportList = buildList;
            ReschedulePayrollFilter.PayrollTotalCount = PayrollTotalCount;
            ReschedulePayrollFilter.TotalRescheduleCommision = TotalRescheduleCommision;
            return ReschedulePayrollFilter;
        }

        public DataTable GetDownLoadFundedCommisionReport(DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize, string SearchText, bool IsPaid, string FilterText, string from, string UserGroup, string TicketType)
        {
            DataSet ds = _AddMemberCommissionDataAccess.GetDownloadAllFundedReport(FilterStartDate, FilterEndDate, order, pageno, pagesize, SearchText, IsPaid, FilterText, from, UserGroup, TicketType);
            //List<AddMemberCommission> buildList = new List<AddMemberCommission>();
            DataTable buildList = ds.Tables[0];

            return buildList;
        }
        public DataTable GetDownloadAdjustmentReport(DateTime FilterStartDate, DateTime FilterEndDate, string order, bool IsPaid, string FilterText, string SearchText)
        {
            DataSet ds = _AddMemberCommissionDataAccess.GetDownloadAdjustmentReport(FilterStartDate, FilterEndDate, order, IsPaid, FilterText, SearchText);
            DataTable buildList = ds.Tables[0];
            return buildList;
        }
        public FundedCommisionReportCluster GetAllFundedCommisionCluster(DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize, string SearchText, bool IsPaid, string FilterText, string UserGroup, string TicketType)
        {
            DataSet ds = _AddMemberCommissionDataAccess.GetAllFundedReportCluster(FilterStartDate, FilterEndDate, order, pageno, pagesize, SearchText, IsPaid, FilterText, UserGroup, TicketType);
            List<FundedCommisionCluster> buildList = new List<FundedCommisionCluster>();
            buildList = (from DataRow dr in ds.Tables[0].Rows
                         select new FundedCommisionCluster()
                         {
                             TicketId = dr["ticid"] != DBNull.Value ? Convert.ToInt32(dr["ticid"]) : 0,
                             CustomerName = dr["CustomerName"].ToString(),
                             SalesTotalCommission = dr["SalesTotalCommission"] != DBNull.Value ? Convert.ToDouble(dr["SalesTotalCommission"]) : 0,
                             TechTotalCommission = dr["TechTotalCommission"] != DBNull.Value ? Convert.ToDouble(dr["TechTotalCommission"]) : 0,
                             AddCommission = dr["AddCommission"] != DBNull.Value ? Convert.ToDouble(dr["AddCommission"]) : 0,
                             FinRepCommission = dr["FinRepCommission"] != DBNull.Value ? Convert.ToDouble(dr["FinRepCommission"]) : 0,
                             CallCommission = dr["CallCommission"] != DBNull.Value ? Convert.ToDouble(dr["CallCommission"]) : 0,
                             FollowUpCommission = dr["FollowUpCommission"] != DBNull.Value ? Convert.ToDouble(dr["FollowUpCommission"]) : 0,
                             RescheduleCommission = dr["RescheduleCommission"] != DBNull.Value ? Convert.ToDouble(dr["RescheduleCommission"]) : 0,
                             TicketType = dr["TicketType"].ToString(),
                             BalanceDue = dr["BalanceDue"] != DBNull.Value ? Convert.ToDouble(dr["BalanceDue"]) : 0
                         }).ToList();

            PayrollTotalCount PayrollTotalCount = new PayrollTotalCount();
            PayrollTotalCount = (from DataRow dr in ds.Tables[1].Rows
                                 select new PayrollTotalCount()
                                 {
                                     CountTotal = dr["CountTotal"] != DBNull.Value ? Convert.ToInt32(dr["CountTotal"]) : 0
                                 }).FirstOrDefault();
            FundedCommisionReportCluster FundedPayrollFilter = new FundedCommisionReportCluster();
            FundedPayrollFilter.PayrollReportCluster = buildList;
            FundedPayrollFilter.PayrollTotalCount = PayrollTotalCount;
            return FundedPayrollFilter;
        }
        public FundedCommisionReport GetAllFundedCommisionReport(DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize, string SearchText, bool IsPaid, string FilterText, string UserGroup, string TicketType, int? TicketId)
        {
            DataSet ds = _AddMemberCommissionDataAccess.GetAllFundedReport(FilterStartDate, FilterEndDate, order, pageno, pagesize, SearchText, IsPaid, FilterText, UserGroup, TicketType, TicketId);
            List<FundedCommision> buildList = new List<FundedCommision>();
            buildList = (from DataRow dr in ds.Tables[1].Rows
                         select new FundedCommision()
                         {
                             CustomerName = dr["CustomerName"].ToString(),
                             Technician = dr["Technician"].ToString(),
                             TicketId = dr["ticid"] != DBNull.Value ? Convert.ToInt32(dr["ticid"]) : 0,
                             SalesId = dr["SalesId"] != DBNull.Value ? Convert.ToInt32(dr["SalesId"]) : 0,
                             TechId = dr["TechId"] != DBNull.Value ? Convert.ToInt32(dr["TechId"]) : 0,
                             AddMemberId = dr["AddMemberId"] != DBNull.Value ? Convert.ToInt32(dr["AddMemberId"]) : 0,
                             FinRepId = dr["FinRepId"] != DBNull.Value ? Convert.ToInt32(dr["FinRepId"]) : 0,
                             ServiceCallId = dr["ServiceCallId"] != DBNull.Value ? Convert.ToInt32(dr["ServiceCallId"]) : 0,
                             FollowUpId = dr["FollowUpId"] != DBNull.Value ? Convert.ToInt32(dr["FollowUpId"]) : 0,
                             RescheduleId = dr["RescheduleId"] != DBNull.Value ? Convert.ToInt32(dr["RescheduleId"]) : 0,

                             CompletionDate = dr["CompletionDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletionDate"]) : new DateTime(),
                             PaidDate = dr["PaidDate"] != DBNull.Value ? Convert.ToDateTime(dr["PaidDate"]) : new DateTime(),
                             PayrollDate = dr["PayrollDate"] != DBNull.Value ? Convert.ToDateTime(dr["PayrollDate"]) : new DateTime(),
                             CustomerIdValue = dr["CustomerIdValue"] != DBNull.Value ? Convert.ToInt32(dr["CustomerIdValue"]) : 0,


                             IsPaid = (dr["IsPaid"] != DBNull.Value ? Convert.ToBoolean(dr["IsPaid"]) : false),

                             BalanceDue = dr["BalanceDue"] != DBNull.Value ? Convert.ToDouble(dr["BalanceDue"]) : 0,
                             Batch = dr["Batch"].ToString(),
                             SalesTotalCommission = dr["SalesTotalCommission"] != DBNull.Value ? Convert.ToDouble(dr["SalesTotalCommission"]) : 0,
                             TechTotalCommission = dr["TechTotalCommission"] != DBNull.Value ? Convert.ToDouble(dr["TechTotalCommission"]) : 0,
                             AddCommission = dr["AddCommission"] != DBNull.Value ? Convert.ToDouble(dr["AddCommission"]) : 0,
                             FinRepCommission = dr["FinRepCommission"] != DBNull.Value ? Convert.ToDouble(dr["FinRepCommission"]) : 0,
                             CallCommission = dr["CallCommission"] != DBNull.Value ? Convert.ToDouble(dr["CallCommission"]) : 0,
                             FollowUpCommission = dr["FollowUpCommission"] != DBNull.Value ? Convert.ToDouble(dr["FollowUpCommission"]) : 0,
                             RescheduleCommission = dr["RescheduleCommission"] != DBNull.Value ? Convert.ToDouble(dr["RescheduleCommission"]) : 0,
                             Type = dr["Type"].ToString(),
                             Adjustment = dr["Adjustment"] != DBNull.Value ? Convert.ToDouble(dr["Adjustment"]) : 0,
                             OriginalPoint = dr["OriginalPoint"] != DBNull.Value ? Convert.ToDouble(dr["OriginalPoint"]) : 0,
                             TicketType = dr["TicketType"].ToString()
                         }).ToList();

            PayrollTotalCount PayrollTotalCount = new PayrollTotalCount();
            PayrollTotalCount = (from DataRow dr in ds.Tables[0].Rows
                                 select new PayrollTotalCount()
                                 {
                                     CountTotal = dr["CountTotal"] != DBNull.Value ? Convert.ToInt32(dr["CountTotal"]) : 0
                                 }).FirstOrDefault();
            TotalPayrollSum TotalFundedCommision = new TotalPayrollSum();
            TotalFundedCommision = (from DataRow dr in ds.Tables[2].Rows
                                    select new TotalPayrollSum()
                                    {
                                        TotalAdjustment = dr["TotalAdjustment"] != DBNull.Value ? Convert.ToDouble(dr["TotalAdjustment"]) : 0.0,
                                        TotalSalesCommission = dr["TotalSalesCommission"] != DBNull.Value ? Convert.ToDouble(dr["TotalSalesCommission"]) : 0.0,
                                        TotalTechCommission = dr["TotalTechCommission"] != DBNull.Value ? Convert.ToDouble(dr["TotalTechCommission"]) : 0.0,
                                        TotalAddCommission = dr["TotalAddCommission"] != DBNull.Value ? Convert.ToDouble(dr["TotalAddCommission"]) : 0.0,
                                        TotalFinRepCommission = dr["TotalFinRepCommission"] != DBNull.Value ? Convert.ToDouble(dr["TotalFinRepCommission"]) : 0.0,
                                        TotalCallCommission = dr["TotalCallCommission"] != DBNull.Value ? Convert.ToDouble(dr["TotalCallCommission"]) : 0.0,
                                        TotalFollowUpCommission = dr["TotalFollowUpCommission"] != DBNull.Value ? Convert.ToDouble(dr["TotalFollowUpCommission"]) : 0.0,
                                        TotalRescheduleCommission = dr["TotalRescheduleCommission"] != DBNull.Value ? Convert.ToDouble(dr["TotalRescheduleCommission"]) : 0.0,
                                        TotalUnpaidBalance = dr["TotalUnpaid"] != DBNull.Value ? Convert.ToDouble(dr["TotalUnpaid"]) : 0.0,
                                        TotalPoint = dr["TotalPoint"] != DBNull.Value ? Convert.ToDouble(dr["TotalPoint"]) : 0.0,
                                    }).FirstOrDefault();
            FundedCommisionReport FundedPayrollFilter = new FundedCommisionReport();
            FundedPayrollFilter.PayrollReportList = buildList;
            FundedPayrollFilter.PayrollTotalCount = PayrollTotalCount;
            FundedPayrollFilter.TotalFundedCommision = TotalFundedCommision;
            return FundedPayrollFilter;
        }
        public AdjustmentFundingReport GetAllAdjustmentFundingReport(DateTime FilterStartDate, DateTime FilterEndDate, string order, int pageno, int pagesize, bool IsPaid, string FilterText, string SearchText)
        {
            DataSet ds = _AddMemberCommissionDataAccess.GetAllAdjustmentFundingReport(FilterStartDate, FilterEndDate, order, pageno, pagesize, IsPaid, FilterText, SearchText);
            List<AdjustmentFunding> buildList = new List<AdjustmentFunding>();
            buildList = (from DataRow dr in ds.Tables[1].Rows
                         select new AdjustmentFunding()
                         {
                             AdjustmentId = (Guid)dr["AdjustmentId"],
                             Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                             Reason = dr["Reason"].ToString(),
                             Amount = dr["Amount"] != DBNull.Value ? Convert.ToDouble(dr["Amount"]) : 0,
                             UserName = dr["UserName"].ToString(),
                             Batch = dr["Batch"] != DBNull.Value ? Convert.ToInt32(dr["Batch"]) : 0,
                             Date = dr["Date"] != DBNull.Value ? Convert.ToDateTime(dr["Date"]) : new DateTime(),
                             PayrollDate = dr["PayrollDate"] != DBNull.Value ? Convert.ToDateTime(dr["PayrollDate"]) : new DateTime()
                         }).ToList();

            PayrollTotalCount PayrollTotalCount = new PayrollTotalCount();
            PayrollTotalCount = (from DataRow dr in ds.Tables[0].Rows
                                 select new PayrollTotalCount()
                                 {
                                     CountTotal = dr["CountTotal"] != DBNull.Value ? Convert.ToInt32(dr["CountTotal"]) : 0
                                 }).FirstOrDefault();
            TotalAmount TotalAmount = new TotalAmount();
            TotalAmount = (from DataRow dr in ds.Tables[2].Rows
                           select new TotalAmount()
                           {
                               Amount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0.0
                           }).FirstOrDefault();
            AdjustmentFundingReport adjustmentFundingFilter = new AdjustmentFundingReport();
            adjustmentFundingFilter.PayrollReportList = buildList;
            adjustmentFundingFilter.PayrollTotalCount = PayrollTotalCount;
            adjustmentFundingFilter.TotalAmount = TotalAmount;
            return adjustmentFundingFilter;
        }

        public bool InsertAddMemberCommission(AddMemberCommission addMembercommission)
        {
            return _AddMemberCommissionDataAccess.Insert(addMembercommission) > 0;
        }
        public AddMemberCommission GetAddMemberCommissionByTicketIdUserId(Guid TicketId, Guid UserId)
        {
            string query = "TicketId='" + TicketId + "' AND UserId='" + UserId + "'";
            return _AddMemberCommissionDataAccess.GetByQuery(query).FirstOrDefault();
        }
        public bool UpdateAddMemberCommission(AddMemberCommission addMembercommission)
        {
            return _AddMemberCommissionDataAccess.Update(addMembercommission) > 0;
        }
        public bool DeleteAddMemberCommissionByTicketId(Guid ticketId)
        {
            return _AddMemberCommissionDataAccess.DeleteAddMemberCommissionByTicketId(ticketId);
        }

        public List<FollowUpCommission> GetFollowUpCommissionByTicketId(Guid ticketId, Guid UserId)
        {
            DataTable dt = _FollowUpCommissionDataAccess.GetFollowUpCommissionByTicketId(ticketId, UserId);

            List<FollowUpCommission> FollowUpCommissionList = (from DataRow dr in dt.Rows
                                                               select new FollowUpCommission()
                                                               {
                                                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                                   Technician = dr["Technician"].ToString(),
                                                                   Commission = dr["Commission"] != DBNull.Value ? Convert.ToDouble(dr["Commission"]) : 0,
                                                                   Adjustment = dr["Adjustment"] != DBNull.Value ? Convert.ToDouble(dr["Adjustment"]) : 0,
                                                                   FollowUpCommissionId = (Guid)dr["FollowUpCommissionId"],
                                                                   UserId = (Guid)dr["UserId"]
                                                               }).ToList();
            return FollowUpCommissionList;
        }
        public bool InsertFollowUpCommission(FollowUpCommission followUpcommission)
        {
            return _FollowUpCommissionDataAccess.Insert(followUpcommission) > 0;
        }
        public FollowUpCommission GetFollowUpCommissionByTicketIdUserId(Guid TicketId, Guid UserId)
        {
            string query = "TicketId='" + TicketId + "' AND UserId='" + UserId + "'";
            return _FollowUpCommissionDataAccess.GetByQuery(query).FirstOrDefault();
        }
        public bool UpdateFollowUpCommission(FollowUpCommission followUpcommission)
        {
            return _FollowUpCommissionDataAccess.Update(followUpcommission) > 0;
        }
        public bool DeleteFollowUpCommissionByTicketId(Guid ticketId)
        {
            return _FollowUpCommissionDataAccess.DeleteFollowUpCommissionByTicketId(ticketId);
        }

        public int InsertAddjustmentFunding(AdjustmentFunding funding)
        {
            return (int)_AdjustmentFundingDataAccess.Insert(funding);
        }
        public int UpdateAddjustmentFunding(AdjustmentFunding funding)
        {
            return (int)_AdjustmentFundingDataAccess.Update(funding);
        }
        public int InsertRescheduleTicket(RescheduleTicket ReTicket)
        {
            return (int)_RescheduleTicketDataAccess.Insert(ReTicket);
        }
        public int InsertAdditionalAppointment(AdditionalMembersAppointment appointment)
        {
            return (int)_AdditionalMembersAppointmentDataAccess.Insert(appointment);
        }
        public bool UpdateAdditionalAppointment(AdditionalMembersAppointment appointment)
        {
            return _AdditionalMembersAppointmentDataAccess.Update(appointment) > 0;
        }

        public List<AdditionalMembersAppointment> GetAllAdditionalMembersAppointmenByTicketIdAndEmpId(Guid TicketId, Guid EmpId)
        {
            string query = "AppointmentId='" + TicketId + "' AND EmployeeId='" + EmpId + "'";
            return _AdditionalMembersAppointmentDataAccess.GetByQuery(query).ToList();
        }
        public List<AdditionalMembersAppointment> GetAllAdditionalMembersAppointmenByTicketIdAndEmpIdwithname(Guid TicketId, Guid EmpId)
        {
            DataTable dt = _AdditionalMembersAppointmentDataAccess.GetAllAdditionalAppointmentByTicketId(TicketId, EmpId);

            List<AdditionalMembersAppointment> appointmentList = (from DataRow dr in dt.Rows
                                                                  select new AdditionalMembersAppointment()
                                                                  {
                                                                      Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                                      EmpName = dr["EmpName"].ToString(),
                                                                      AppointmentStartTime = dr["AppointmentStartTime"].ToString(),
                                                                      AppointmentEndTime = dr["AppointmentEndTime"].ToString(),
                                                                      StartTime = dr["StartTime"].ToString(),
                                                                      EndTime = dr["EndTime"].ToString()
                                                                  }).ToList();
            return appointmentList;
        }
        public long DeleteAdditionalMemberAppointment(int Id)
        {
            return _AdditionalMembersAppointmentDataAccess.Delete(Id);
        }

        public List<RescheduleCommission> GetRescheduleCommissionByTicketId(Guid ticketId, Guid UserId)
        {
            DataTable dt = _RescheduleCommissionDataAccess.GetRescheduleCommissionByTicketId(ticketId, UserId);

            List<RescheduleCommission> RescheduleCommissionList = (from DataRow dr in dt.Rows
                                                                   select new RescheduleCommission()
                                                                   {
                                                                       Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                                       Technician = dr["Technician"].ToString(),
                                                                       Commission = dr["Commission"] != DBNull.Value ? Convert.ToDouble(dr["Commission"]) : 0,
                                                                       Adjustment = dr["Adjustment"] != DBNull.Value ? Convert.ToDouble(dr["Adjustment"]) : 0,
                                                                       RescheduleCommissionId = (Guid)dr["RescheduleCommissionId"]
                                                                   }).ToList();
            return RescheduleCommissionList;
        }
        public bool InsertRescheduleCommission(RescheduleCommission reschedulecommission)
        {
            return _RescheduleCommissionDataAccess.Insert(reschedulecommission) > 0;
        }
        public RescheduleCommission GetRescheduleCommissionByTicketIdUserId(Guid TicketId, Guid UserId)
        {
            string query = "TicketId='" + TicketId + "' AND UserId='" + UserId + "'";
            return _RescheduleCommissionDataAccess.GetByQuery(query).FirstOrDefault();
        }
        public bool UpdateRescheduleCommission(RescheduleCommission reschedulecommission)
        {
            return _RescheduleCommissionDataAccess.Update(reschedulecommission) > 0;
        }
        public bool DeleteRescheduleCommissionByTicketId(Guid ticketId)
        {
            return _RescheduleCommissionDataAccess.DeleteRescheduleCommissionByTicketId(ticketId);
        }

        public List<ServiceCallCommission> GetServiceCallCommissionByTicketId(Guid ticketId, Guid CommissionUserId)
        {
            DataTable dt = _ServiceCallCommissionDataAccess.GetServiceCallCommissionByTicketId(ticketId, CommissionUserId);

            List<ServiceCallCommission> ServiceCallCommissionList = (from DataRow dr in dt.Rows
                                                                     select new ServiceCallCommission()
                                                                     {
                                                                         Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                                         Technician = dr["Technician"].ToString(),
                                                                         Commission = dr["Commission"] != DBNull.Value ? Convert.ToDouble(dr["Commission"]) : 0,
                                                                         Adjustment = dr["Adjustment"] != DBNull.Value ? Convert.ToDouble(dr["Adjustment"]) : 0,
                                                                         ServiceCallCommissionId = (Guid)dr["ServiceCallCommissionId"],
                                                                         UserId = (Guid)dr["UserId"]
                                                                     }).ToList();
            return ServiceCallCommissionList;
        }
        public bool InsertServiceCallCommission(ServiceCallCommission ServiceCallcommission)
        {
            return _ServiceCallCommissionDataAccess.Insert(ServiceCallcommission) > 0;
        }
        public ServiceCallCommission GetServiceCallCommissionByTicketIdUserId(Guid TicketId, Guid UserId)
        {
            string query = "TicketId='" + TicketId + "' AND UserId='" + UserId + "'";
            return _ServiceCallCommissionDataAccess.GetByQuery(query).FirstOrDefault();
        }
        public bool UpdateServiceCallCommission(ServiceCallCommission ServiceCallcommission)
        {
            return _ServiceCallCommissionDataAccess.Update(ServiceCallcommission) > 0;
        }
        public bool DeleteServiceCallCommissionByTicketId(Guid ticketId)
        {
            return _ServiceCallCommissionDataAccess.DeleteServiceCallCommissionByTicketId(ticketId);
        }

        public List<TicketUser> GetTicketUserListByUserId(Guid userid, string type)
        {
            DataTable dt = _TicketUserDataAccess.GetTicketUserListByUserId(userid, type);

            List<TicketUser> TechCommissionList = (from DataRow dr in dt.Rows
                                                   select new TicketUser()
                                                   {
                                                       TiketId = (Guid)dr["TiketId"]
                                                   }).ToList();
            return TechCommissionList;
        }

        public List<Ticket> GetAllTicketByBookingId(string bookingId)
        {
            return _TicketDataAccess.GetByQuery(string.Format("BookingId = '{0}' ", bookingId));
        }

        public List<TicketUser> GetTicketUserListAndCustomerAppointmentByUserId(Guid userid, string type, string mindate, string maxdate)
        {
            DataTable dt = _TicketUserDataAccess.GetTicketUserListAndCustomerAppointmentByUserId(userid, type, mindate, maxdate);

            List<TicketUser> TechCommissionList = (from DataRow dr in dt.Rows
                                                   select new TicketUser()
                                                   {
                                                       TiketId = (Guid)dr["TiketId"],
                                                       StartDate = dr["StartDate"].ToString(),
                                                       Enddate = dr["Enddate"].ToString(),
                                                       CustomerName = dr["CustomerName"].ToString(),
                                                       Street = dr["Street"].ToString(),
                                                       City = dr["City"].ToString(),
                                                       State = dr["State"].ToString(),
                                                       ZipCode = dr["ZipCode"].ToString(),
                                                       IsAllDay = dr["IsAllDay"] != DBNull.Value ? Convert.ToBoolean(dr["IsAllDay"]) : false,
                                                       EMPNAME = dr["EMPNAME"].ToString(),
                                                       TicketIntId = dr["TicketIntId"] != DBNull.Value ? Convert.ToInt32(dr["TicketIntId"]) : 0,
                                                       BookingId = dr["BookingId"].ToString(),
                                                   }).ToList();
            return TechCommissionList;
        }
        public List<TicketNotificationEmail> GetAllTicketNotificationEmailList()
        {
            DataTable dt = _TicketNotificationEmailDataAccess.GetAllTicketNotificationEmailList();

            List<TicketNotificationEmail> emailList = (from DataRow dr in dt.Rows
                                                   select new TicketNotificationEmail()
                                                   {
                                                       Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                       TicketStatusVal = dr["TicketStatusVal"].ToString(),
                                                       Email = dr["Email"].ToString(),
                                                   }).ToList();
            return emailList;
        }
        public TicketNotificationEmail GetTicketNotificationEmailbyId(int id)
        {
            return _TicketNotificationEmailDataAccess.Get(id);
        }
        public long UpdateTicketNotificationEmail(TicketNotificationEmail email)
        {
            return _TicketNotificationEmailDataAccess.Update(email);
        }
        public TicketNotificationEmail GetTicketNotificationEmailByStatus(string TicketStatus)
        {
            DataTable dt = _TicketNotificationEmailDataAccess.GetTicketNotificationEmailByTicketStatus(TicketStatus);

            List<TicketNotificationEmail> email = (from DataRow dr in dt.Rows
                                                       select new TicketNotificationEmail()
                                                       {
                                                           Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                           TicketStatusVal = dr["TicketStatusVal"].ToString(),
                                                           Email = dr["Email"].ToString(),
                                                       }).ToList();
            return email.FirstOrDefault();
        }
        public long InsertTicketNotificationEmail(TicketNotificationEmail email)
        {
            return _TicketNotificationEmailDataAccess.Insert(email);
        }
        public bool DeleteTicketNotificationEmail(int id)
        {
            return _TicketNotificationEmailDataAccess.Delete(id) > -1;
        }
        public List<TicketUser> GetAllTicketUserListByTicketIdAndNotificationOnly(Guid ticketid)
        {
            return _TicketUserDataAccess.GetByQuery(string.Format("TiketId = '{0}' and NotificationOnly = 0", ticketid)).ToList();
        }
        public List<TicketUser> GetAllNotificationTicketUserListByTicketId(Guid ticketid)
        {
            return _TicketUserDataAccess.GetByQuery(string.Format("TiketId = '{0}' and NotificationOnly = 1", ticketid)).ToList();
        }
        public List<AdditionalMember> GetAllTicketAdditionalMembersByTicketId(Guid ticketid)
        {
            DataTable dt = _TicketUserDataAccess.GetAllTicketAdditionalMemberByTicketId(ticketid);

            List<AdditionalMember> AdditionalMemberList = (from DataRow dr in dt.Rows
                                                           select new AdditionalMember()
                                                           {
                                                               UserId = (Guid)dr["UserId"],
                                                               FullName = dr["FullName"].ToString(),
                                                               IsReschedulePay = dr["IsReschedulePay"] != DBNull.Value ? Convert.ToBoolean(dr["IsReschedulePay"]) : false
                                                           }).ToList();
            return AdditionalMemberList;
        }
        public TicketUser GetTicketUserById(int value)
        {
            return _TicketUserDataAccess.Get(value);
        }
        public List<TicketUser> GetTicketUserByUserIdAndTicketId(Guid userid, Guid ticketid)
        {
            return _TicketUserDataAccess.GetByQuery(string.Format("UserId = '{0}' and TiketId = '{1}' and NotificationOnly = 0", userid, ticketid)).ToList();
        }

        public List<TicketUser> GetTicketUserByTicketIdAndPrimary(Guid userid, Guid ticketid)
        {
            return _TicketUserDataAccess.GetByQuery(string.Format("TiketId = '{1}' and NotificationOnly = 0 and IsPrimary = 1", userid, ticketid)).ToList();
        }

        public TicketUser GetTicketUserByTicketIdAndIsPrimary(Guid userid, Guid ticketid)
        {
            return _TicketUserDataAccess.GetByQuery(string.Format("TiketId = '{1}' and NotificationOnly = 0 and IsPrimary = 1", userid, ticketid)).FirstOrDefault();
        }

        public TicketUser GetTicketUserByTicketIdAndUserIdAndIsPrimary(Guid userid, Guid ticketid)
        {
            return _TicketUserDataAccess.GetByQuery(string.Format("TiketId = '{1}' and UserId = '{0}' and NotificationOnly = 0 and IsPrimary = 1", userid, ticketid)).FirstOrDefault();
        }

        public List<TicketUser> GetTicketUserByTicketIdAndAdditional(Guid userid, Guid ticketid)
        {
            return _TicketUserDataAccess.GetByQuery(string.Format("TiketId = '{1}' and UserId = '{0}' and NotificationOnly = 0", userid, ticketid)).ToList();
        }
        public TicketUser GetTicketUserByUserIdAndTicketIdForMail(Guid ticketid)
        {
            return _TicketUserDataAccess.GetByQuery(string.Format("TiketId = '{0}' and IsPrimary = 1", ticketid)).FirstOrDefault();
        }
        public List<TicketUser> GetTicketUserByTicketIdAndAdditionalMember(Guid ticketid, Guid userid)
        {
            DataTable dt = _TicketUserDataAccess.GetTicketUserByTicketIdAndAdditionalMember(ticketid, userid);

            List<TicketUser> TicketUserList = (from DataRow dr in dt.Rows
                                               select new TicketUser()
                                               {
                                                   Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,

                                                   UserId = (Guid)dr["UserId"],
                                                   TiketId = (Guid)dr["UserId"],

                                                   IsPrimary = dr["IsPrimary"] != DBNull.Value ? Convert.ToBoolean(dr["IsPrimary"]) : false,
                                                   IsReschedulePay = dr["IsReschedulePay"] != DBNull.Value ? Convert.ToBoolean(dr["IsReschedulePay"]) : false,
                                                   AddedBy = (Guid)dr["AddedBy"],
                                                   NotificationOnly = dr["NotificationOnly"] != DBNull.Value ? Convert.ToBoolean(dr["NotificationOnly"]) : false,
                                                   AddedDate = dr["AddedDate"] != DBNull.Value ? Convert.ToDateTime(dr["AddedDate"]) : new DateTime(),

                                               }).ToList();
            return TicketUserList;
        }
        public SalesCommission GetSalesComissionByComissionId(Guid comissionid)
        {
            return _SalesCommissionDataAccess.GetByQuery(string.Format("SalesCommissionId = '{0}'", comissionid)).FirstOrDefault();
        }
        public SalesCommission GetSalesComissionById(int Id)
        {
            return _SalesCommissionDataAccess.Get(Id);
        }
        public TechCommission GetTechComissionByComissionId(Guid comissionid)
        {
            return _TechCommissionDataAccess.GetByQuery(string.Format("TechCommissionId = '{0}'", comissionid)).FirstOrDefault();
        }
        public TechCommission GetTechCommissionById(int Id)
        {
            return _TechCommissionDataAccess.Get(Id);
        }
        public AddMemberCommission GetAdditionalComissionByComissionId(Guid comissionid)
        {
            return _AddMemberCommissionDataAccess.GetByQuery(string.Format("AddMemberCommissionId = '{0}'", comissionid)).FirstOrDefault();
        }
        public FinRepCommission GetFinRepComissionByComissionId(Guid comissionid)
        {
            return _FinRepCommissionDataAccess.GetByQuery(string.Format("FinRepCommissionId = '{0}'", comissionid)).FirstOrDefault();
        }

        public FollowUpCommission GetFollowupComissionByComissionId(Guid comissionid)
        {
            return _FollowUpCommissionDataAccess.GetByQuery(string.Format("FollowUpCommissionId = '{0}'", comissionid)).FirstOrDefault();
        }

        public RescheduleCommission GetReScheduleCommissionByComissionId(Guid comissionid)
        {
            return _RescheduleCommissionDataAccess.GetByQuery(string.Format("RescheduleCommissionId = '{0}'", comissionid)).FirstOrDefault();
        }

        public ServiceCallCommission GetServiceCallCommissionByComissionId(Guid comissionid)
        {
            return _ServiceCallCommissionDataAccess.GetByQuery(string.Format("ServiceCallCommissionId = '{0}'", comissionid)).FirstOrDefault();
        }

        public string FormatAmount(double? value)
        {
            string formatted = "0.00";
            if (value.HasValue)
            {
                formatted = string.Format(CultureInfo.InvariantCulture, "{0:N}", value);

            }
            return formatted;
        }
        public string MakeCustomerAddendumPdf(CustomerAddendumModel cusAddendum)
        {
            string currentCurrency = "";
            string ServiceList = "";
            string AddedList = "";
            string RemovedList = "";
            string EquipmentList = "";
            string Body = "";
            double MonitoringFee = 0;
            double AdditionalMonthlyFee = 0;
            double MonthlyRate = 0;
            double Added = 0;
            double Removed = 0;
            double BillingTicketTotalAmount = 0;
            double NewTicketTotalAmount = 0;
            double DefaultServiceAmount = 0;
            string KazarAddendumQTY = "";
            string KazarAddendumName = "";
            string KazarAddendumPrice = "";
            string KazarAddendumSubTotal = "";
            double KazarAddendumTotalSubTotal = 0.0;
            double KazarAddendumFinalTotal = 0.0;
            string KazarAddendumList = "";
            if (cusAddendum.CurrentCurrency != null)
            {
                currentCurrency = cusAddendum.CurrentCurrency;
            }
            else
            {
                currentCurrency = "$";
            }
            var objtiklist = GetTicketListByCustomerIdAndCompanyId(cusAddendum.CustomerId, cusAddendum.CompanyId);
            if (objtiklist != null && objtiklist.Count > 0)
            {
                foreach (var tik in objtiklist)
                {
                    var objappeqp = GetCustomerAppointmentEquipmentByAppointmentIdAndBilling(tik.TicketId);
                    if (objappeqp != null && objappeqp.Count > 0)
                    {
                        foreach (var eqp in objappeqp)
                        {
                            BillingTicketTotalAmount += eqp.TotalPrice;
                        }
                    }
                }
            }
            if (cusAddendum.ServiceEqpList != null)
            {
                foreach (var item in cusAddendum.ServiceEqpList)
                {
                    ServiceList += "<tr><td>" + item.EquipmentServiceName + "</td><tr>";
                    if (item.IsDefaultService == false)
                    {
                        MonitoringFee += item.TotalPrice;
                        NewTicketTotalAmount += item.TotalPrice;
                    }
                }

                var ACHDiscountAmount = 0.0;
                var objpayinfocus = _PaymentInfoCustomerDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and Payfor = 'MMR'", cusAddendum.CustomerId)).FirstOrDefault();
                if (objpayinfocus != null)
                {
                    var objpayprofile = _PaymentProfileCustomerDataAccess.GetByQuery(string.Format("PaymentInfoId = {0}", objpayinfocus.PaymentInfoId)).FirstOrDefault();
                    if (objpayprofile != null && objpayprofile.Type.ToLower().IndexOf("ach") > -1)
                    {
                        var objglobal = _GlobalSettingDataAccess.GetByQuery(string.Format("SearchKey = 'ACHDiscount'")).FirstOrDefault();
                        if (objglobal != null)
                        {
                            ACHDiscountAmount = Convert.ToDouble(objglobal.Value);
                        }
                    }
                }
                if (ACHDiscountAmount > 0)
                {
                    MonitoringFee = MonitoringFee - ACHDiscountAmount;
                    //NewTicketTotalAmount = NewTicketTotalAmount - ACHDiscountAmount;
                }
                MonitoringFee -= DefaultServiceAmount;
                if (BillingTicketTotalAmount > 0 && NewTicketTotalAmount > 0 && BillingTicketTotalAmount != NewTicketTotalAmount)
                {
                    if (NewTicketTotalAmount > BillingTicketTotalAmount)
                    {
                        Added = NewTicketTotalAmount - BillingTicketTotalAmount;
                    }
                    else
                    {
                        Removed = BillingTicketTotalAmount - NewTicketTotalAmount;
                    }
                }
                ServiceList = "<table style='width=100%';'border='1'>" + ServiceList + "</table>";
            }

            if (cusAddendum.EquipmentList != null)
            {
                foreach (var item in cusAddendum.EquipmentList)
                {
                    EquipmentList += "<tr><td>" + item.EquipmentServiceName + "</td><td>" + currentCurrency + item.TotalPrice.toFixed(2) + "</td><tr>";
                }
                EquipmentList = "<table style='width=100%';'border='1'>" + EquipmentList + "</table>";
            }
            try
            {
                Hashtable templateVars = new Hashtable();

                templateVars.Add("COMPANYLOGO", cusAddendum.CompanyLogo);
                templateVars.Add("KazarLogo", cusAddendum.KazarLogo);
                templateVars.Add("KazarLogoIcon", cusAddendum.KazarLogoIcon);
                templateVars.Add("COMPANYADDRESS", cusAddendum.CompanyAddress);

                templateVars.Add("COMPANYZIP", cusAddendum.CompanyZip);
                templateVars.Add("COMPANYCITY", cusAddendum.CompanyCity);
                templateVars.Add("COMPANYSTATE", cusAddendum.CompanyState);
                templateVars.Add("SERVICELIST", ServiceList);
                if (Added > 0)
                {
                    AddedList = "<table style='width=100%';'border='1'><tr><td>Added: " + currentCurrency + Added.toFixed(2) + "</td><tr></table>";
                    templateVars.Add("AddedList", AddedList);
                }
                if (Removed > 0)
                {
                    RemovedList = "<table style='width=100%';'border='1'><tr><td>Removed: " + currentCurrency + Removed.toFixed(2) + "</td><tr></table>";
                    templateVars.Add("RemovedList", RemovedList);
                }
                templateVars.Add("EQUIPMENTLIST", EquipmentList);
                if (cusAddendum.CompanyPhone != "")
                {
                    templateVars.Add("COMPANYPHONW", cusAddendum.CompanyPhone + " (Office)");
                }


                templateVars.Add("FIRSTNAME", cusAddendum.FirstName);
                templateVars.Add("LASTNAME", cusAddendum.LastName);
                templateVars.Add("CustomerStreet", cusAddendum.CustomerStreet);
                templateVars.Add("CustomerCity", cusAddendum.CustomerCity);
                templateVars.Add("CustomerState", cusAddendum.CustomerState);
                templateVars.Add("CustomerZip", cusAddendum.CustomerZip);
                templateVars.Add("INSTALLADDRESS", cusAddendum.InstallAddress);

                #region Kazar addendum Data
                templateVars.Add("WorkToBePerformed", cusAddendum.WorkToBePerformed);
                if (cusAddendum.ServiceEqpList != null)
                {
                    //templateVars.Add("MonthlyMonitoringFee", currentCurrency + FormatAmount(Convert.ToDouble(cusAddendum.ServiceEqpList.Where(x => x.EquipName == "Monthly Monitoring Rate").Select(x => x.TotalPrice).FirstOrDefault())));
                    foreach (var item in cusAddendum.ServiceEqpList)
                    {

                        if (item.IsBilling == true)
                        {
                            MonthlyRate += item.TotalPrice;
                        }
                        else
                        {
                            AdditionalMonthlyFee += item.TotalPrice;
                        }
                    }
                    templateVars.Add("AdditionalMonthlyFee", currentCurrency + FormatAmount(Convert.ToDouble(AdditionalMonthlyFee)));
                    templateVars.Add("MonthlyMonitoringFee", currentCurrency + FormatAmount(Convert.ToDouble(MonthlyRate)));
                    templateVars.Add("NewMonthlyTotal", currentCurrency + FormatAmount(Convert.ToDouble(AdditionalMonthlyFee + MonthlyRate)));
                }
                if (!string.IsNullOrWhiteSpace(cusAddendum.SalesRepresentative) && cusAddendum.SalesRepresentative != "-1")
                {
                    templateVars.Add("SalesRepresentative", cusAddendum.SalesRepresentative);
                }
                else
                {
                    templateVars.Add("SalesRepresentative", "");
                }

                if (cusAddendum.EquipmentList != null && cusAddendum.EquipmentList.Count() > 0)
                {
                    foreach (var item in cusAddendum.EquipmentList)
                    {
                        KazarAddendumQTY = "<td style='text-align:right; border:1px solid #ebebeb; padding:10px; height:30px; width:60px;' valign='middle'>" + item.Quantity + "</td>";
                        KazarAddendumName = "<td style='text-align:center; border: 1px solid #ebebeb; padding:10px; height:30px;  width:60%;' valign='middle'>" + item.EquipName + "</td>";
                        KazarAddendumPrice = "<td style='text-align:right; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + currentCurrency + FormatAmount(item.UnitPrice) + "</td>";
                        KazarAddendumSubTotal = "<td style='text-align:right; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + currentCurrency + FormatAmount(item.TotalPrice) + "</td>";

                        KazarAddendumList += "<tr style=''>" + KazarAddendumQTY + KazarAddendumName + KazarAddendumPrice + KazarAddendumSubTotal + "</tr>";
                        KazarAddendumTotalSubTotal += item.TotalPrice;
                    }
                }
                else
                {
                    KazarAddendumQTY = "<td style='text-align:right; border:1px solid #ebebeb; padding:10px; height:30px; width:60px;' valign='middle'>" + 0 + "</td>";
                    KazarAddendumName = "<td style='text-align:center; border: 1px solid #ebebeb; padding:10px; height:30px;  width:60%;' valign='middle'> </td>";
                    KazarAddendumPrice = "<td style='text-align:right; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + currentCurrency + FormatAmount(0.0) + "</td>";
                    KazarAddendumSubTotal = "<td style='text-align:right; border: 1px solid #ebebeb; padding:10px; height:30px' valign='middle'>" + currentCurrency + FormatAmount(0.0) + "</td>";

                    KazarAddendumList += "<tr style=''>" + KazarAddendumQTY + KazarAddendumName + KazarAddendumPrice + KazarAddendumSubTotal + "</tr>";
                }
                KazarAddendumFinalTotal = KazarAddendumTotalSubTotal + ((KazarAddendumTotalSubTotal * cusAddendum.Tax) / 100);
                KazarAddendumList += "<tr style=''><td colspan='2' style ='text-align:right; padding:10px;'></td ><td style ='text-align:right; padding:10px;'>Subtotal</td><td style ='text-align:right; padding:10px;font-weight:bold;'>" +
                                    currentCurrency + FormatAmount(KazarAddendumTotalSubTotal) +
                                "</td ></tr>";
                KazarAddendumList += "<tr style=''><td colspan='2' style ='text-align:right; padding:10px;'></td ><td style ='text-align:right; padding:10px;'>Tax(" + cusAddendum.Tax + "%)</td><td style ='text-align:right; padding:10px;font-weight:bold;'>" +
                                currentCurrency + FormatAmount(((KazarAddendumTotalSubTotal * cusAddendum.Tax) / 100)) +
                            "</td ></tr>";
                KazarAddendumList += "<tr style='font-weight:bold;'><td colspan ='2' style='text-align:right; padding:10px;'></td><td style ='text-align:right; padding:10px;'>Total</td> <td style ='text-align:right; padding:10px;'>" +
                                currentCurrency + FormatAmount(KazarAddendumFinalTotal) +
                            "</td></tr>";
                templateVars.Add("AddendumEquipmentList", KazarAddendumList);
                #endregion

                templateVars.Add("BUSINESSNAME", cusAddendum.BuisnessName);
                templateVars.Add("EMAILADDRESS", cusAddendum.EmailAddress);

                if (cusAddendum.CellPhone != "")
                {
                    templateVars.Add("CELLPHONE", cusAddendum.CellPhone + " (Cell Phone)");
                }
                if (cusAddendum.SitePhone != "")
                {
                    templateVars.Add("SITEPHONE", cusAddendum.SitePhone + " Primary Phone");
                }

                templateVars.Add("TICKETID", cusAddendum.TicketId);
                if (cusAddendum.ScheduleOn != new DateTime())
                {
                    templateVars.Add("SCHEDULEON", cusAddendum.ScheduleOn.ToString("MM/dd/yy"));
                }
                if (cusAddendum.AgreementSignDate != new DateTime())
                {
                    templateVars.Add("AgreementSignDate", cusAddendum.AgreementSignDate.ToString("M/dd/yy"));
                }
                if (cusAddendum.AddendumCreateDate != new DateTime())
                {
                    templateVars.Add("AddendumCreateDate", cusAddendum.AddendumCreateDate.ToString("M/dd/yy"));
                }
                templateVars.Add("RECURRINGAMOUNT", currentCurrency + String.Format("{0:0,0.00}", MonitoringFee));
                if (!string.IsNullOrWhiteSpace(cusAddendum.CustomerAddendumSignature))
                {
                    templateVars.Add("CustomerAddendumSignature", cusAddendum.CustomerAddendumSignature);
                    if (!string.IsNullOrWhiteSpace(cusAddendum.CompanySignature))
                    {
                        templateVars.Add("CompanySignature", cusAddendum.CompanySignature);
                        if (!string.IsNullOrWhiteSpace(cusAddendum.CompanySignatureDate))
                            templateVars.Add("CompanySignatureDate", cusAddendum.CompanySignatureDate);
                    }
                    if (!string.IsNullOrWhiteSpace(cusAddendum.CustomerAddendumStringSignatureDate))
                    {
                        templateVars.Add("CustomerAddendumSignatureDate", cusAddendum.CustomerAddendumStringSignatureDate);
                    }
                }

                //templateVars.Add("CustomerAddendumSignatureDate", cusAddendum.CustomerAddendumSignatureDate.UTCToClientTime().ToString("MM/dd/yyyy"));


                EmailParser parser = null;
                EmailTemplate CustomerAddendumTemplate = _EmailTemplateDataAccess.GetEmailTemplateByKeyAndCompanyId(cusAddendum.CompanyId.ToString(), EmailTemplateKey.CostomerAddendumPdf.CustomerAddendum);


                parser = new EmailParser(HttpContext.Current.Server.MapPath(CustomerAddendumTemplate.BodyFile), templateVars, true);
                MailMessage message = new MailMessage();
                message.Body = parser.Parse();
                Body = message.Body;



            }
            catch (Exception ex)
            {
                //Logger.AddElmah(ex);
            }

            return Body;
        }
        private long InsertCustomerAppoinment(CustomerAppointment ca)
        {
            return _CustomerAppoinmentDataAccess.Insert(ca);
        }

        public void CreateTicketsForApprovingBooking(string TicketStatus,
            Booking booking,//Guid CustomerId,
            int CustomerIntId,//Guid CompanyId, 
            Guid CreatedByUid,
            string CreatedBy,
            //string BookingId,//DateTime PickUpDate,//string PickUpAddress,//string DropOffAddress,
            string TicketTypePickup,
            string TicketTypeService,
            string TicketTypeDropOff,
            string AdminTag,
            string HrManagerTag,
            string NotificationType
            )
        {
            DateTime PickUpDate = booking.PickUpDate.HasValue ? booking.PickUpDate.Value : DateTime.Now.UTCCurrentTime().AddDays(1);
            DateTime NextWorkingDayPickUp = PickUpDate;//DateTime.Now.UTCCurrentTime().UTCToClientTime().AddDays(1);
            //Sun=0,Mon=1,Tue=2,Wed=3,Thu=4,Fri=5,Sat=6
            if (NextWorkingDayPickUp.DayOfWeek == 0)
            {
                NextWorkingDayPickUp = NextWorkingDayPickUp.AddDays(1);
            }
            DateTime DropOffDate = booking.DropOffDate.HasValue ? booking.DropOffDate.Value : PickUpDate.AddDays(5);
            DateTime NextWorkingDayService = NextWorkingDayPickUp.AddDays(1);//DateTime.Now.UTCCurrentTime().UTCToClientTime().AddDays(2);
            DateTime NextWorkingDayDropOff = DropOffDate;//DateTime.Now.UTCCurrentTime().UTCToClientTime().AddDays(6);

            if (NextWorkingDayService.DayOfWeek == 0)
            {
                NextWorkingDayService = NextWorkingDayService.AddDays(1);
            }
            if (NextWorkingDayDropOff.DayOfWeek == 0)
            {
                NextWorkingDayDropOff = NextWorkingDayDropOff.AddDays(1);
            }

            #region Create Ticket 
            {
                #region Ticket For Pickup
                Ticket TicketForPickup = new Ticket()
                {
                    TicketId = Guid.NewGuid(),
                    CompanyId = booking.CompanyId,
                    CustomerId = booking.CustomerId,
                    TicketType = TicketTypePickup,
                    Message = "Rug pickup.",
                    CreatedBy = CreatedByUid,
                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                    CompletionDate = NextWorkingDayPickUp,
                    Status = TicketStatus,
                    LastUpdatedBy = CreatedByUid,
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                    HasInvoice = false,
                    HasSurvey = false,
                    IsClosed = false,
                    IsAgreementTicket = false,
                    BookingId = booking.BookingId
                };
                InsertTicket(TicketForPickup);
                TicketUser TUPickup = new TicketUser()
                {
                    TiketId = TicketForPickup.TicketId,
                    UserId = new Guid("22222222-2222-2222-2222-222222222222"),
                    IsPrimary = true,
                    AddedDate = DateTime.Now.UTCCurrentTime(),
                    AddedBy = CreatedByUid,
                    NotificationOnly = false,
                };
                InsertTicketUser(TUPickup);
                CustomerAppointment CAForPickup = new CustomerAppointment()
                {
                    AppointmentId = TicketForPickup.TicketId,
                    CompanyId = TicketForPickup.CompanyId,
                    CustomerId = TicketForPickup.CustomerId,
                    EmployeeId = new Guid("22222222-2222-2222-2222-222222222222"),
                    AppointmentType = TicketForPickup.TicketType,
                    AppointmentStartTime = "08:00",
                    AppointmentEndTime = "10:00",
                    IsAllDay = false,
                    Notes = TicketForPickup.Message,
                    Status = false,
                    AppointmentDate = NextWorkingDayPickUp.SetZeroHour(),
                    CreatedBy = CreatedBy,
                    LastUpdatedBy = CreatedBy,
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                    Address = booking.PickUpLocation
                };
                InsertCustomerAppoinment(CAForPickup);

                #endregion
            }
            {
                #region Ticket For Service
                Ticket TicketForService = new Ticket()
                {
                    TicketId = Guid.NewGuid(),
                    CompanyId = booking.CompanyId,
                    CustomerId = booking.CustomerId,
                    TicketType = TicketTypeService,
                    Message = "Rug Servicing.",
                    CreatedBy = CreatedByUid,
                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                    CompletionDate = NextWorkingDayService,//bk.PickUpDate.HasValue ? bk.PickUpDate.Value : DateTime.Now.UTCCurrentTime(),
                    Status = TicketStatus,
                    LastUpdatedBy = CreatedByUid,
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                    HasInvoice = false,
                    HasSurvey = false,
                    IsClosed = false,
                    IsAgreementTicket = false,
                    BookingId = booking.BookingId
                };
                InsertTicket(TicketForService);
                TicketUser TUService = new TicketUser()
                {
                    TiketId = TicketForService.TicketId,
                    UserId = new Guid("22222222-2222-2222-2222-222222222222"),
                    IsPrimary = true,
                    AddedDate = DateTime.Now.UTCCurrentTime(),
                    AddedBy = CreatedByUid,
                    NotificationOnly = false,
                };
                InsertTicketUser(TUService);
                CustomerAppointment CAForService = new CustomerAppointment()
                {
                    AppointmentId = TicketForService.TicketId,
                    CompanyId = TicketForService.CompanyId,
                    CustomerId = TicketForService.CustomerId,
                    EmployeeId = new Guid("22222222-2222-2222-2222-222222222222"),
                    AppointmentType = TicketForService.TicketType,
                    AppointmentStartTime = "10:00",
                    AppointmentEndTime = "12:00",
                    IsAllDay = false,
                    Notes = TicketForService.Message,
                    Status = false,
                    AppointmentDate = NextWorkingDayService.SetZeroHour(),
                    CreatedBy = CreatedBy,
                    LastUpdatedBy = CreatedBy,
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime()
                };
                InsertCustomerAppoinment(CAForService);
                #endregion
            }
            {
                #region Ticket For DropOff
                Ticket TicketForDropOff = new Ticket()
                {
                    TicketId = Guid.NewGuid(),
                    CompanyId = booking.CompanyId,
                    CustomerId = booking.CustomerId,
                    TicketType = TicketTypeDropOff,
                    Message = "Rug Drop off.",
                    CreatedBy = CreatedByUid,
                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                    CompletionDate = NextWorkingDayDropOff,//bk.PickUpDate.HasValue ? bk.PickUpDate.Value : DateTime.Now.UTCCurrentTime(),
                    Status = TicketStatus,
                    LastUpdatedBy = CreatedByUid,
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                    HasInvoice = false,
                    HasSurvey = false,
                    IsClosed = false,
                    IsAgreementTicket = false,
                    BookingId = booking.BookingId
                };
                InsertTicket(TicketForDropOff);
                TicketUser TUDropOff = new TicketUser()
                {
                    TiketId = TicketForDropOff.TicketId,
                    UserId = new Guid("22222222-2222-2222-2222-222222222222"),
                    IsPrimary = true,
                    AddedDate = DateTime.Now.UTCCurrentTime(),
                    AddedBy = CreatedByUid,
                    NotificationOnly = false,
                };
                InsertTicketUser(TUDropOff);
                CustomerAppointment CAForDropOff = new CustomerAppointment()
                {
                    AppointmentId = TicketForDropOff.TicketId,
                    CompanyId = TicketForDropOff.CompanyId,
                    CustomerId = TicketForDropOff.CustomerId,
                    EmployeeId = new Guid("22222222-2222-2222-2222-222222222222"),
                    AppointmentType = TicketForDropOff.TicketType,
                    AppointmentStartTime = "12:00",
                    AppointmentEndTime = "14:00",
                    IsAllDay = false,
                    Notes = TicketForDropOff.Message,
                    Status = false,
                    CreatedBy = CreatedBy,
                    AppointmentDate = NextWorkingDayDropOff.SetZeroHour(),
                    LastUpdatedBy = CreatedBy,
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                    Address = booking.DropOffLocation
                };
                InsertCustomerAppoinment(CAForDropOff);

                #endregion
            }

            #endregion

            #region AddBooking Items to ticket

            #region TicketBookingDetails
            List<BookingDetails> bookingDetails = _BookingDetailsDataAccess.GetByQuery(string.Format("BookingId = '{0}'", booking.BookingId));
            if (bookingDetails != null && bookingDetails.Count > 0)
            {
                foreach (var item in bookingDetails)
                {
                    TicketBookingDetails ticketBookingDetails = new TicketBookingDetails()
                    {
                        CompanyId = item.CompanyId,
                        BookingId = item.BookingId,
                        RugType = item.RugType,

                        Length = item.Length,
                        LengthInch = item.LengthInch,

                        Width = item.Width,
                        WidthInch = item.WidthInch,

                        Radius = item.Radius,
                        RadiusInch = item.RadiusInch,

                        TotalSize = item.TotalSize,
                        Package = item.Package,

                        Included = item.Included,
                        Extras = item.Extras,
                        UnitPrice = item.UnitPrice,
                        DiscountType = item.DiscountType,
                        TaxType = item.TaxType,
                        Quantity = item.Quantity,
                        Discount = item.Discount,
                        TotalPrice = item.TotalPrice,
                        AddedDate = item.AddedDate,
                        AddedBy = item.AddedBy,
                    };
                    _TicketBookingDetailsDataAccess.Insert(ticketBookingDetails);
                }

            }
            #endregion

            #region TicketBookingExtraItem
            List<BookingExtraItem> BookingExtraItems = _BookingExtraItemDataAccess.GetByQuery(string.Format(" BookingId = '{0}'", booking.Id));
            if (BookingExtraItems != null && BookingExtraItems.Count > 0)
            {
                foreach (var item in BookingExtraItems)
                {
                    TicketBookingExtraItem extrItem = new TicketBookingExtraItem()
                    {
                        BookingId = item.BookingId,
                        EquipmentId = item.EquipmentId,
                        EquipName = item.EquipName,
                        EquipDetail = item.EquipDetail,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        Discount = item.Discount,
                        TotalPrice = item.TotalPrice,
                        CreatedDate = item.CreatedDate,
                        CreatedBy = item.CreatedBy,
                        Taxable = item.Taxable,

                    };
                    _TicketBookingExtraItemDataAccess.Insert(extrItem);

                }
            }
            #endregion

            #endregion

            #region Notification
            if (CreatedByUid == new Guid("22222222-2222-2222-2222-222222222222"))
            {
                #region Insert notification 
                Notification notification = new Notification()
                {
                    CompanyId = booking.CompanyId,
                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                    NotificationId = Guid.NewGuid(),
                    Type = NotificationType,
                    Who = booking.CustomerId,
                    What = string.Format(@"Customer <a class=""cus-anchor"" href=""{3}/Customer/Customerdetail/?id={1}"">{0}</a> has signed the booking  
                                        <a class=""cus-anchor"" href=""javascript:void(0)"" onclick=""OpenBkById('{2}')"">{2}</a>", "{0}", CustomerIntId, booking.BookingId, AppConfig.DomainSitePath),
                    NotificationUrl = AppConfig.DomainSitePath + "/Customer/Customerdetail/?id=" + CustomerIntId + "#BookingTab"
                };

                _NotificationDataAccess.Insert(notification);

                #endregion

                #region set user to notification
                EmployeeFacade EmployeeFacade = new EmployeeFacade();

                List<Guid> UserList = EmployeeFacade.GetEmployeeByCompanyIdAndTag(booking.CompanyId, AdminTag, new Guid()).Select(x => x.UserId).ToList();
                UserList.AddRange(EmployeeFacade.GetEmployeeByCompanyIdAndTag(booking.CompanyId, HrManagerTag, new Guid()).Select(x => x.UserId).ToList());
                UserList = UserList.GroupBy(x => x).Select(x => x.Key).ToList();

                foreach (var item in UserList)
                {
                    NotificationUser nu = new NotificationUser()
                    {
                        NotificationId = notification.NotificationId,
                        IsRead = false,
                        NotificationPerson = item,
                    };
                    _NotificationUserDataAccess.Insert(nu);
                }

                #endregion
            }
            #endregion

            #region Invoice

            List<BookingDetails> BkDetails = _BookingDetailsDataAccess.GetByQuery(string.Format("BookingId = '{0}'", booking.BookingId));
            List<BookingExtraItem> BkExtr = _BookingExtraItemDataAccess.GetByQuery(string.Format("BookingId = '{0}'", booking.Id));
            if ((BkDetails != null && BkDetails.Count > 0) || (BkExtr != null && BkExtr.Count > 0))
            {
                Invoice BkInv = new Invoice()
                {
                    InvoiceDate = booking.PickUpDate,
                    InvoiceFor = "Invoice",
                    Status = "Open",
                    CompanyId = booking.CompanyId,
                    CustomerId = booking.CustomerId,
                    CreatedDate = DateTime.Now.UTCCurrentTime(),
                    CreatedByUid = CreatedByUid,
                    CreatedBy = CreatedBy,
                    LastUpdatedDate = DateTime.Now.UTCCurrentTime(),
                    LastUpdatedByUid = CreatedByUid,
                    AuthRefId = "",
                    Description = "Created for " + booking.BookingId,
                    IsEstimate = false,

                    Amount = booking.Amount.HasValue ? booking.Amount.Value : 0,
                    Tax = booking.Tax,
                    DiscountAmount = booking.DiscountAmount,
                    TotalAmount = booking.TotalAmount,
                    BalanceDue = booking.TotalAmount,

                    DiscountType = "amount",
                    IsBill = false,
                    BillingAddress = booking.BillingAddress,
                    ShippingAddress = booking.BillingAddress,
                    BookingId = booking.BookingId,
                };

                BkInv.Id = (int)_InvoiceDataAccess.Insert(BkInv);

                // Only for rug tracker
                //if (booking.CompanyId.ToString() == "46FE75B2-B321-4645-90AF-D6ACC64B9AF6")
                //{
                    BkInv.DueDate = NextWorkingDayDropOff;
                    BkInv.Terms = "Custom";
                //}
                BkInv.InvoiceId = BkInv.Id.GenerateInvoiceNo();
                _InvoiceDataAccess.Update(BkInv);

                if (BkDetails != null && BkDetails.Count > 0)
                {
                    foreach (var item in BkDetails)
                    {
                        string EquipmentName = "Package {0}, Shape: {1} ({2} = {3}sf)";
                        string RugShape = string.Format(@"{0}'{1}""", item.Radius.HasValue ? item.Radius.Value : 0, item.RadiusInch.HasValue ? item.RadiusInch.Value : 0);
                        if (item.RugType != "Circle")
                        {
                            RugShape = string.Format(@"{0}'{1}""X{2}'{3}""",
                                item.Length.HasValue ? item.Length.Value : 0,
                                    item.LengthInch.HasValue ? item.LengthInch.Value : 0,
                                    item.Width.HasValue ? item.Width.Value : 0,
                                    item.WidthInch.HasValue ? item.WidthInch.Value : 0);
                        }
                        EquipmentName = string.Format(EquipmentName, item.Package, item.RugType, RugShape, Math.Round(item.TotalSize.HasValue ? item.TotalSize.Value : 0, 2));

                        string EquipmentDetails = "Included Items: {0}";
                        EquipmentDetails = string.Format(EquipmentDetails, string.IsNullOrWhiteSpace(item.Included) ? "" : item.Included);

                        InvoiceDetail invedet = new InvoiceDetail()
                        {
                            CompanyId = booking.CompanyId,
                            CreatedBy = CreatedBy,
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            InvoiceId = BkInv.InvoiceId,
                            Taxable = true,
                            Quantity = item.Quantity,
                            EquipDetail = EquipmentDetails,
                            EquipName = EquipmentName,
                            UnitPrice = item.UnitPrice * item.TotalSize,
                            TotalPrice = item.TotalPrice,
                            EquipmentId = Guid.Empty,
                            DiscountAmount = item.Discount
                        };
                        _InvoiceDetailDataAccess.Insert(invedet);
                    }
                }
                if (BkExtr != null && BkExtr.Count > 0)
                {
                    foreach (var item in BkExtr)
                    {
                        InvoiceDetail invedet = new InvoiceDetail()
                        {
                            CompanyId = booking.CompanyId,
                            CreatedBy = CreatedBy,
                            CreatedDate = DateTime.Now.UTCCurrentTime(),
                            InvoiceId = BkInv.InvoiceId,
                            Taxable = true,
                            Quantity = item.Quantity,
                            EquipDetail = item.EquipDetail,
                            EquipName = item.EquipName,
                            UnitPrice = item.UnitPrice,
                            TotalPrice = item.TotalPrice,
                            EquipmentId = item.EquipmentId,
                            DiscountAmount = item.Discount
                        };
                        _InvoiceDetailDataAccess.Insert(invedet);
                    }
                }
            }
            #endregion

        }

        public TicketUser GetTicketUserByTicketIdAndUserIdAndNotification(Guid ticketid, Guid userid)
        {
            return _TicketUserDataAccess.GetByQuery(string.Format("TiketId = '{0}' and UserId = '{1}' and NotificationOnly = 0 and IsPrimary = 0", ticketid, userid)).FirstOrDefault();
        }

        public List<TicketReply> GetTicketReplyListByTicketId(Guid ticketid)
        {
            return _TicketReplyDataAccess.GetByQuery(string.Format("TicketId = '{0}' and CHARINDEX('<p>', [Message]) = 1", ticketid)).ToList();
        }

        public List<Ticket> GetTicketListByCustomerIdAndBookingIdAndTicketType(Guid customerid, string bookingid)
        {
            return _TicketDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and BookingId = '{1}' and (TicketType = 'Drop Off' or TicketType = 'Service')", customerid, bookingid)).ToList();
        }

        public Ticket GetTicketListByCustomerIdAndBookingIdAndTicketTypeOnly(Guid customerid, string bookingid)
        {
            return _TicketDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and BookingId = '{1}' and TicketType = 'Service' and ([Status] != 'Completed' or IsClosed = 0)", customerid, bookingid)).FirstOrDefault();
        }

        public List<Ticket> GetTicketListByCustomerIdAndReferenceTicketId(Guid id)
        {
            return _TicketDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and ReferenceTicketId > 0", id)).ToList();
        }

        public List<Ticket> GetTicketListByReferenceTicketId(int id)
        {
            return _TicketDataAccess.GetByQuery(string.Format("ReferenceTicketId = {0}", id)).ToList();
        }

        public bool DeleteTicket(int value)
        {
            return _TicketDataAccess.Delete(value) > 0;
        }

        public bool UpdateTicketNotification(TicketCustomerNotification notify)
        {
            return _TicketCustomerNotificationDataAccess.Update(notify) > 0;
        }

        public long InsertTicketNotification(TicketCustomerNotification notify)
        {
            return _TicketCustomerNotificationDataAccess.Insert(notify);
        }

        public List<TicketCustomerNotification> GetAllTicketCustomerNotifications()
        {
            return _TicketCustomerNotificationDataAccess.GetAll();
        }

        public TicketCustomerNotification GetNotificationById(int value)
        {
            return _TicketCustomerNotificationDataAccess.Get(value);
        }

        public TicketCustomerNotification GetTicketCustomerNotificationByStatusAndType(string status, string type)
        {
            return _TicketCustomerNotificationDataAccess.GetByQuery(string.Format("TicketStatus = '{0}' and TicketType = '{1}'", status, type)).FirstOrDefault();
        }

        public bool DeleteTicketCustomerNotification(int value)
        {
            return _TicketCustomerNotificationDataAccess.Delete(value) > 0;
        }

        public TicketStatusImageSetting GetImageSettingById(int value)
        {
            return _TicketStatusImageSettingDataAccess.Get(value);
        }

        public long InsertStatusImageSetting(TicketStatusImageSetting img)
        {
            return _TicketStatusImageSettingDataAccess.Insert(img);
        }

        public bool UpdateStatusImageSetting(TicketStatusImageSetting img)
        {
            return _TicketStatusImageSettingDataAccess.Update(img) > 0;
        }

        public TicketStatusImageSetting GetStatusImageSettingByCompanyIdAndStatus(Guid companyid, string status)
        {
            return _TicketStatusImageSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and TicketStatus = '{1}'", companyid, status)).FirstOrDefault();
        }

        public List<TicketStatusImageSetting> GetAllStatusImageSettingByCompanyId(Guid companyid)
        {
            return _TicketStatusImageSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", companyid)).ToList();
        }

        public Ticket GetTicketByTicketIdAndCustomerId(Guid ticketid, Guid cusid)
        {
            return _TicketDataAccess.GetByQuery(string.Format("TicketId = '{0}' and CustomerId = '{1}'", ticketid, cusid)).FirstOrDefault();
        }

        public TicketUser GetTicketUserByTicketIdAndPrimary(Guid ticketid)
        {
            return _TicketUserDataAccess.GetByQuery(string.Format("TiketId = '{0}' and NotificationOnly = 0 and IsPrimary = 1", ticketid)).FirstOrDefault();
        }

        public Ticket GetTicketByCustomerId(Guid customerid)
        {
            return _TicketDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerid)).FirstOrDefault();
        }
        public List<Ticket> GetAllTicketByCustomerId(Guid customerid)
        {
            return _TicketDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerid));
        }
        public List<Ticket> GetAllInstallationTicketByCustomerId(Guid customerid)
        {
            return _TicketDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and TicketType = 'Installation'", customerid));
        }
        public Ticket GetInstallationTicketByCustomerId(Guid customerid)
        {
            return _TicketDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and TicketType = 'Installation'", customerid)).FirstOrDefault();
        }
        public List<Ticket> GetTicketListByCustomerIdAndCompanyIdAndNewTicketId(Guid customerid, Guid companyid, Guid ticketid)
        {
            return _TicketDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and CompanyId = '{1}' and TicketId != '{2}'", customerid, companyid, ticketid)).ToList();
        }

        public List<Ticket> GetTicketListByCustomerIdAndCompanyId(Guid customerid, Guid companyid)
        {
            return _TicketDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and CompanyId = '{1}'", customerid, companyid)).ToList();
        }

        public List<Ticket> GetTicketListByCustomerIdAndCompanyIdAndCompleted(Guid customerid, Guid companyid)
        {
            return _TicketDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and CompanyId = '{1}' and [Status] = 'Completed'", customerid, companyid)).ToList();
        }

        public List<Ticket> GetTicketListByCustomerIdAndCompanyIdAndNotCompleted(Guid customerid, Guid companyid)
        {
            return _TicketDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and CompanyId = '{1}' and [Status] != 'Completed'", customerid, companyid)).ToList();
        }

        public List<Ticket> GetTicketListByCustomerIdAndCompanyIdAndIsCompleted(Guid customerid, Guid companyid)
        {
            return _TicketDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and CompanyId = '{1}' and [Status] = 'Completed'", customerid, companyid)).ToList();
        }

        public List<CustomerAppointmentEquipment> GetCustomerAppointmentEquipmentByAppointmentIdAndBilling(Guid appointmentid)
        {
            return _CustomerAppointmentEquipmentDataAccess.GetByQuery(string.Format("AppointmentId = '{0}' and IsBilling = 1 and IsCopied != 1 and IsService = 1", appointmentid)).ToList();
        }

        public bool DeleteRescheduleTicket(int value)
        {
            return _RescheduleTicketDataAccess.Delete(value) > 0;
        }

        public List<RescheduleTicket> GetRescheduleTicketListByTicketId(Guid ticketid)
        {
            return _RescheduleTicketDataAccess.GetByQuery(string.Format("TicketId = '{0}'", ticketid)).ToList();
        }

        public TicketUser GetTicketMemberByTicketIdAndUserId(Guid ticketid, Guid userid)
        {
            return _TicketUserDataAccess.GetByQuery(string.Format("TiketId = '{0}' and UserId = '{1}'", ticketid, userid)).FirstOrDefault();
        }

        public DataSet CloneRescheduleTicketConfirmation(Guid oldticketid, string createdby, Guid createdbyuid, DateTime CompletionDate)
        {
            return _TicketDataAccess.CloneRescheduleTicketConfirmation(oldticketid, createdby, createdbyuid, CompletionDate);
        }

        public Ticket GetTicketByRescheduleTicketId(int ticketid)
        {
            return _TicketDataAccess.GetByQuery(string.Format("RescheduleTicketId = '{0}'", ticketid)).FirstOrDefault();
        }

        public List<TicketUser> GetAllAdditionalTicketUserByTicketId(Guid ticketid)
        {
            return _TicketUserDataAccess.GetByQuery(string.Format("TiketId = '{0}' and IsPrimary = 0 and NotificationOnly = 0", ticketid)).ToList();
        }
        public bool UpdateTicketUserToSystemUserById(Guid value)
        {
            return _TicketUserDataAccess.UpdateTicketUserToSystemUserById(value);
        }
        public void DeleteTicketUserById(int value)
        {
            _TicketUserDataAccess.Delete(value);
        }

        public CustomerAppointmentEquipment GetDefaultServiceByAppointmentId(Guid appid)
        {
            return _CustomerAppointmentEquipmentDataAccess.GetByQuery(string.Format("AppointmentId = '{0}' and IsDefaultService = 1 and IsBilling = 0", appid)).FirstOrDefault();
        }

        public List<TicketFile> GetTicketFilesByDetailsId(int id)
        {
            return _TicketFileDataAccess.GetByQuery(" TicketBookingDetailsId = " + id);
        }

        public bool DeleteTicketFilesByBookingDetailsId(int id)
        {
            return _TicketFileDataAccess.DeleteTicketFilesByBookingDetailsId(id);
        }

        public long InsertTicketPayment(TicketPayment pay)
        {
            return _TicketPaymentDataAccess.Insert(pay);
        }

        public TicketPayment GetTicketPaymentByTicketId(Guid TicketId)
        {
            return _TicketPaymentDataAccess.GetByQuery(string.Format("TicketId = '{0}'", TicketId)).FirstOrDefault();
        }

        public bool DeleteTicketPaymentByTicketId(int value)
        {
            return _TicketPaymentDataAccess.Delete(value) > 0;
        }

        public Ticket GetTicketByCustomerIdAndIsAgreementTicket(Guid id)
        {
            return _TicketDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and IsAgreementTicket = 1", id)).FirstOrDefault();
        }

        public TicketUser GetAllAssignedTicketCountByUserId(Guid userid)
        {
            DataTable dt = _TicketDataAccess.GetAllAssignedTicketCountByUserId(userid);

            TicketUser TicketUserList = (from DataRow dr in dt.Rows
                                         select new TicketUser()
                                         {
                                             MyTicketCount = dr["MyTicketCount"] != DBNull.Value ? Convert.ToInt32(dr["MyTicketCount"]) : 0,
                                         }).FirstOrDefault();
            return TicketUserList;
        }
        public TicketUser GetInstallationTicketTechByCustomerId(Guid cusid)
        {
            DataTable dt = _TicketDataAccess.GetInstallationTicketTechByCustomerId(cusid);

            TicketUser _TicketUser = (from DataRow dr in dt.Rows
                                         select new TicketUser()
                                         {
                                             EMPNAME = dr["TechName"].ToString(),
                                         }).FirstOrDefault();
            return _TicketUser;
        }
        public TicketListModel GetAllAssignedTicketListByUserId(Guid userid, int pageno, int pagesize)
        {
            DataSet ds = _TicketDataAccess.GetAllAssignedTicketListByUserId(userid, pageno, pagesize);
            TicketListModel model = new TicketListModel();
            model.Tickets = (from DataRow dr in ds.Tables[0].Rows
                             select new Ticket()
                             {
                                 Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                 Status = dr["Status"].ToString(),
                                 TicketType = dr["TicketType"].ToString(),
                                 CompletionDate = dr["CompletionDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletionDate"]) : new DateTime(),
                                 Message = dr["Message"].ToString(),
                                 CreatedUser = dr["CreatedUser"].ToString(),
                                 AssignUser = dr["AssignUser"].ToString(),
                             }).ToList();
            model.TotalCount = ds.Tables[1].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCount"]) : 0;
            return model;
        }
        public TicketStatusImageSetting GetTicketStatusImageSettingByStatus(string status)
        {
            return _TicketStatusImageSettingDataAccess.GetByQuery(string.Format("TicketStatus = '{0}'", status)).FirstOrDefault();
        }

        #region Ticket Summary Report

        public TicketListModel GetTicketSummaryList(TicketFilter Filters, FilterReportModel filter)
        {
            TicketListModel Model = new TicketListModel();
            DataSet ds = _TicketDataAccess.GetTicketSummaryList(Filters, filter);
            Model.TicketSummaryList = new List<TicketSummary>();
            if (ds != null)
                Model.TicketSummaryList = (from DataRow dr in ds.Tables[0].Rows
                                           select new TicketSummary()
                                           {
                                               TicketIntId = dr["TicketIntId"] != DBNull.Value ? Convert.ToInt32(dr["TicketIntId"]) : 0,
                                               CusIdInt = dr["CustomerIntId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerIntId"]) : 0,
                                               CustomerNo = dr["CustomerNo"].ToString()
                                           }).ToList();
            Model.TotalCount = ds.Tables[1].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCount"]) : 0;
            Model.PageNo = Filters.PageNo.Value;
            Model.PageSize = Filters.PageSize;
            Model.Searchtext = Filters.SearchText;

            return Model;
            // return _TicketDataAccess.GetByQuery(string.Format("CustomerId = '{0}'", customerId));
        }
        public DataTable GetTicketSummaryListForReport(TicketFilter Filters, FilterReportModel filter)
        {
            DataTable dt = _TicketDataAccess.GetTicketSummaryListForReport(Filters, filter);
            return dt;
        }

        #endregion

        #region PR Report

        public PRReport GetPRReportList(DateTime? Start, DateTime? End, int pageno, int pagesize, string searchtext,string order)
        {
            DataSet dsResult = _TicketDataAccess.GetPRReportList(Start, End, pageno, pagesize, searchtext, order);
            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];
            DataTable dt2 = dsResult.Tables[2];
            PRReport Model = new PRReport();
            List<PRReport> PRReportList = new List<PRReport>();
            ListCount Count = new ListCount();
            TotalCost TotalCost = new TotalCost();
            if (dt != null)
                PRReportList = (from DataRow dr in dt1.Rows
                                select new PRReport()
                                {
                                    TicketId = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                    CustomerId = dr["CustomerId"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                    CustomerNo = dr["CustomerNo"].ToString(),
                                    Name = dr["Name"].ToString(),
                                    EquipmentNames = dr["EquipmentNames"].ToString(),
                                    TotalPayments = dr["TotalPayments"] != DBNull.Value ? Convert.ToDouble(dr["TotalPayments"]) : 0,
                                    TicketStatus = dr["Status"].ToString(),
                                    TicketType = dr["TicketType"].ToString()
                                }).ToList();
            Count = (from DataRow dr in dt.Rows
                     select new ListCount()
                     {
                         Count = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                     }).FirstOrDefault();
            TotalCost = (from DataRow dr in dt2.Rows
                         select new TotalCost()
                         {
                             Amount = dr["TotalPayment"] != DBNull.Value ? Convert.ToDouble(dr["TotalPayment"]) : 0
                         }).FirstOrDefault();
            PRReport PRReport = new PRReport();
            PRReport.PRReportList = PRReportList;
            PRReport.Count = Count;
            PRReport.TotalCost = TotalCost;
            return PRReport;
        }
        public DataTable GetPRReportListForDownload(DateTime? Start, DateTime? End, string searchtext)
        {
            DataTable dt = _TicketDataAccess.GetPRReportListForDownload(Start, End, searchtext);
            return dt;
        }
        #endregion

        #region DeleteCommission
        public bool DeleteSalesCommissionById(int CommissionId)
        {
            return _SalesCommissionDataAccess.Delete(CommissionId) > 0;
        }
        public bool DeleteTechCommissionById(int CommissionId)
        {
            return _TechCommissionDataAccess.Delete(CommissionId) > 0;
        }
        public bool DeleteAddMemberCommissionById(int CommissionId)
        {
            return _AddMemberCommissionDataAccess.Delete(CommissionId) > 0;
        }
        public bool DeleteFinRepCommissionById(int CommissionId)
        {
            return _FinRepCommissionDataAccess.Delete(CommissionId) > 0;
        }
        public bool DeleteRescheduleCommissionById(int CommissionId)
        {
            return _RescheduleCommissionDataAccess.Delete(CommissionId) > 0;
        }
        public bool DeleteFollowUpCommissionById(int CommissionId)
        {
            return _FollowUpCommissionDataAccess.Delete(CommissionId) > 0;
        }
        public bool DeleteServiceCallCommissionById(int CommissionId)
        {
            return _ServiceCallCommissionDataAccess.Delete(CommissionId) > 0;
        }
        #endregion

        #region InstalledTicket Report
        public InstalledTicket InstalledTicketReportList(DateTime? Start, DateTime? End, int pageno, int pagesize, string searchtext, string PaymentMethod, string FundedStatus, Guid UserId,string order)
        {
            DataSet dsResult = _TicketDataAccess.InstalledTicketReportList(Start, End, pageno, pagesize, searchtext, PaymentMethod, FundedStatus, UserId, order);
            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];
            DataTable dt2 = dsResult.Tables[2];
            InstalledTicket Model = new InstalledTicket();
            List<InstalledTicket> InstalledTicketList = new List<InstalledTicket>();
            ListCount Count = new ListCount();
            TotalCost TotalCost = new TotalCost();
            if (dt != null)
                InstalledTicketList = (from DataRow dr in dt1.Rows
                                       select new InstalledTicket()
                                       {
                                           TicketId = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                           RMAAccountNo = dr["RMRAccountNo"] != DBNull.Value ? Convert.ToInt32(dr["RMRAccountNo"]) : 0,
                                           CustomerNo = dr["CustomerNo"].ToString(),
                                           OpenerName = dr["OpenerName"].ToString(),
                                           SoldBy = dr["Rep1"].ToString(),
                                           SoldBy2 = dr["Rep2"].ToString(),
                                           SoldBy3 = dr["Rep3"].ToString(),
                                           SoldBy4 = dr["Rep4"].ToString(),
                                           CustomerName = dr["CustomerName"].ToString(),
                                           InstallDate = dr["InstallDate"] != DBNull.Value ? Convert.ToDateTime(dr["InstallDate"]) : new DateTime(),
                                           Address = dr["Address"].ToString(),
                                           MonthlyMonitoringFee = dr["MonthlyMonitoringFee"].ToString(),
                                           //MonthlyMonitoringFee = dr["MonthlyMonitoringFee"] != DBNull.Value ? Convert.ToDouble(dr["MonthlyMonitoringFee"]) : 0,
                                           EquipmentNames = dr["EquipmentNames"].ToString(),
                                           EquipmentPoint = dr["EquipmentPoint"].ToString(),
                                           EquipmentCost = dr["EquipmentCost"] != DBNull.Value ? Convert.ToDouble(dr["EquipmentCost"]) : 0,
                                           TotalRevenue = dr["TotalCollected"] != DBNull.Value ? Convert.ToDouble(dr["TotalCollected"]) : 0,
                                           ContractTerm = dr["ContractTeam"].ToString(),
                                           PaymentMethod = dr["PaymentMethod"].ToString(),
                                           PaymentMethodVal = dr["PaymentMethodVal"].ToString(),
                                           Technician = dr["Technician"].ToString(),
                                           AccountStatus = dr["AccountStatus"].ToString(),
                                           CustomerType = dr["CustomerType"].ToString(),
                                           ServiceShedule = dr["ServiceShedule"] != DBNull.Value ? Convert.ToDateTime(dr["ServiceShedule"]) : new DateTime(),
                                           FundedStatus = dr["CustomerFunded"].ToString(),
                                           CreditScore = dr["CreditScore"].ToString(),
                                           NumnberOfPayment = dr["NumnberOfPayment"] != DBNull.Value ? Convert.ToInt32(dr["NumnberOfPayment"]) : 0,
                                           UserGroup = dr["UserGroup"].ToString(),
                                       }).ToList();
            Count = (from DataRow dr in dt.Rows
                     select new ListCount()
                     {
                         Count = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                     }).FirstOrDefault();
            TotalCost = (from DataRow dr in dt2.Rows
                         select new TotalCost()
                         {
                             Amount = dr["TotalAmount"] != DBNull.Value ? Convert.ToDouble(dr["TotalAmount"]) : 0.0,
                             EquipmentCost = dr["TotalEquipmentCost"] != DBNull.Value ? Convert.ToDouble(dr["TotalEquipmentCost"]) : 0.0
                         }).FirstOrDefault();
            InstalledTicket InstalledTicket = new InstalledTicket();
            InstalledTicket.InstalledTicketList = InstalledTicketList;
            InstalledTicket.Count = Count;
            InstalledTicket.TotalCost = TotalCost;
            return InstalledTicket;
        }
        public DataTable InstalledTicketReportListForDownload(DateTime? Start, DateTime? End, string searchtext, string PaymentMethod, string FundedStatus, Guid UserId)
        {
            DataTable dt = _TicketDataAccess.GetInstalledTicketReportListForDownload(Start, End, searchtext, PaymentMethod, FundedStatus, UserId);
            return dt;
        }
        #endregion

        #region Paid Commission Report
        public PayrollDetailModel PaidCommissionReportList(DateTime? Start, DateTime? End, int pageno, int pagesize, string searchtext,string order)
        {
            DataSet dsResult = _TicketDataAccess.PaidCommissionReportList(Start, End, pageno, pagesize, searchtext,order);
            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];
            PayrollDetailModel Model = new PayrollDetailModel();
            if (dt != null)
                Model.PayrollDetailList = (from DataRow dr in dt.Rows
                                           select new PayrollDetail()
                                           {
                                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                               //CompanyId = (Guid)dr["CompanyId"],
                                               RMAAccountNo = dr["RMAAccountNo"] != DBNull.Value ? Convert.ToInt32(dr["RMAAccountNo"]) : 0,
                                               RepName = dr["RepName"].ToString(),
                                               RepCommission = dr["RepCommission"] != DBNull.Value ? Convert.ToDouble(dr["RepCommission"]) : 0,
                                               RepHoldback = dr["RepHoldback"] != DBNull.Value ? Convert.ToDouble(dr["RepHoldback"]) : 0,
                                               OverrideRep1 = dr["OverrideRep1"].ToString(),
                                               Override1 = dr["Override1"].ToString(),
                                               OverrideRep2 = dr["OverrideRep2"].ToString(),
                                               Override2 = dr["Override2"].ToString(),
                                               RepPaidDate = dr["RepPaidDate"] != DBNull.Value ? Convert.ToDateTime(dr["RepPaidDate"]) : new DateTime(),
                                               TechName = dr["TechName"].ToString(),
                                               TechPay = dr["TechPay"] != DBNull.Value ? Convert.ToDouble(dr["TechPay"]) : 0,
                                               TechHoldback = dr["TechHoldback"] != DBNull.Value ? Convert.ToDouble(dr["TechHoldback"]) : 0,
                                               TechPaidDate = dr["TechPaidDate"] != DBNull.Value ? Convert.ToDateTime(dr["TechPaidDate"]) : new DateTime(),
                                               OpenerCommission = dr["OpenerCommission"] != DBNull.Value ? Convert.ToDouble(dr["OpenerCommission"]) : 0,
                                               MiscRep1 = dr["MiscRep1"].ToString(),
                                               MiscCommission1 = dr["MiscCommission1"] != DBNull.Value ? Convert.ToDouble(dr["MiscCommission1"]) : 0,
                                               MiscRep2 = dr["MiscRep2"].ToString(),
                                               MiscCommission2 = dr["MiscCommission2"] != DBNull.Value ? Convert.ToDouble(dr["MiscCommission2"]) : 0,
                                               MiscRep3 = dr["MiscRep3"].ToString(),
                                               MiscCommission3 = dr["MiscCommission3"] != DBNull.Value ? Convert.ToDouble(dr["MiscCommission3"]) : 0,
                                               MiscRep4 = dr["MiscRep4"].ToString(),
                                               MiscCommission4 = dr["MiscCommission4"] != DBNull.Value ? Convert.ToDouble(dr["MiscCommission4"]) : 0,
                                               MiscRep5 = dr["MiscRep5"].ToString(),
                                               MiscCommission5 = dr["MiscCommission5"] != DBNull.Value ? Convert.ToDouble(dr["MiscCommission5"]) : 0,
                                               MiscPaidDate = dr["MiscPaidDate"] != DBNull.Value ? Convert.ToDateTime(dr["MiscPaidDate"]) : new DateTime(),

                                           }).ToList();
            Model.TotalCount = (from DataRow dr in dt1.Rows
                                select new Count()
                                {
                                    TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                }).FirstOrDefault();

            return Model;
        }

        public CancelQueueModels CancelQueueReportList(DateTime? Start, DateTime? End, int pageno, int pagesize, string searchtext,string order)
        {
            DataSet dsResult = _TicketDataAccess.CancelQueueReportList(Start, End, pageno, pagesize, searchtext, order);
            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];
            CancelQueueModels Model = new CancelQueueModels();
            if (dt != null)
                Model.CancelQueueList = (from DataRow dr in dt.Rows
                                         select new CancelQueue()
                                         {

                                             CsNumber = dr["CustomerNo"].ToString(),
                                             CancelDate = dr["BrinksCancelDate"] != DBNull.Value ? Convert.ToDateTime(dr["BrinksCancelDate"]) : new DateTime(),

                                         }).ToList();
            Model.TotalCount = (from DataRow dr in dt1.Rows
                                select new Count()
                                {
                                    TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                }).FirstOrDefault();

            return Model;
        }

        public BrinksCustomerModels BrinksCustomerReportList(DateTime? Start, DateTime? End, int pageno, int pagesize, string searchtext,string order)
        {
            DataSet dsResult = _TicketDataAccess.BrinksCustomerReportList(Start, End, pageno, pagesize, searchtext,order);
            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];
            BrinksCustomerModels Model = new BrinksCustomerModels();
            if (dt != null)
                Model.CustomerList = (from DataRow dr in dt.Rows
                                      select new BrinksReportCustomer()
                                      {

                                          Account = dr["CustomerNo"].ToString(),
                                          FundingDate = dr["BrinksFundingDate"] != DBNull.Value ? Convert.ToDateTime(dr["BrinksFundingDate"]) : new DateTime(),
                                          ReceivedDate = dr["ReceivedDate"] != DBNull.Value ? Convert.ToDateTime(dr["ReceivedDate"]) : new DateTime(),
                                          GrossFundingAmmount = dr["GrossFundedAmount"] != DBNull.Value ? Convert.ToInt32(dr["GrossFundedAmount"]) : 0,

                                      }).ToList();
            Model.TotalCount = (from DataRow dr in dt1.Rows
                                select new Count()
                                {
                                    TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                }).FirstOrDefault();

            return Model;
        }
        public FundingVerificationModels FundingVerificationReportList(DateTime? Start, DateTime? End, int pageno, int pagesize, string searchtext,string order)
        {
            DataSet dsResult = _TicketDataAccess.FundingVerificationReportList(Start, End, pageno, pagesize, searchtext, order);
            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];
            FundingVerificationModels Model = new FundingVerificationModels();
            if (dt != null)
                Model.FundingVerificationList = (from DataRow dr in dt.Rows
                                                 select new FundingVerification()
                                                 {

                                                     CsNumber = dr["CustomerNo"].ToString(),
                                                     FundingDate = dr["FinanceFundingDate"] != DBNull.Value ? Convert.ToDateTime(dr["FinanceFundingDate"]) : new DateTime(),
                                                     LoanAmount = dr["LoanAmount"].ToString(),
                                                     PayOut = dr["PayOut"].ToString(),
                                                     NewMMR = dr["NewMMR"].ToString(),
                                                     FinanceCompany = dr["FinanceCompany"].ToString(),
                                                     PlanCode = dr["PlanCode"].ToString(),

                                                 }).ToList();
            Model.TotalCount = (from DataRow dr in dt1.Rows
                                select new Count()
                                {
                                    TotalCount = dr["TotalCount"] != DBNull.Value ? Convert.ToInt32(dr["TotalCount"]) : 0
                                }).FirstOrDefault();

            return Model;
        }

        public PayrollDetailModel GetPaidCommissionImoprtDataById(int Id)
        {
            DataSet dsResult = _TicketDataAccess.GetPaidCommissionImportDataById(Id);
            DataTable dt = dsResult.Tables[0];
            PayrollDetailModel Model = new PayrollDetailModel();
            if (dt != null)
                Model.PayrollDetailList = (from DataRow dr in dt.Rows
                                           select new PayrollDetail()
                                           {
                                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                               //CompanyId = (Guid)dr["CompanyId"],
                                               //RMAAccountNo = dr["RMRAccountNo"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                               RepName = dr["RepName"].ToString(),
                                               RepCommission = dr["RepCommission"] != DBNull.Value ? Convert.ToDouble(dr["RepCommission"]) : 0,
                                               RepHoldback = dr["RepHoldback"] != DBNull.Value ? Convert.ToDouble(dr["RepHoldback"]) : 0,
                                               OverrideRep1 = dr["OverrideRep1"].ToString(),
                                               Override1 = dr["Override1"].ToString(),
                                               OverrideRep2 = dr["OverrideRep2"].ToString(),
                                               Override2 = dr["Override2"].ToString(),
                                               RepPaidDate = dr["RepPaidDate"] != DBNull.Value ? Convert.ToDateTime(dr["RepPaidDate"]) : new DateTime(),
                                               TechName = dr["TechName"].ToString(),
                                               TechPay = dr["TechPay"] != DBNull.Value ? Convert.ToDouble(dr["TechPay"]) : 0,
                                               TechHoldback = dr["TechHoldback"] != DBNull.Value ? Convert.ToDouble(dr["TechHoldback"]) : 0,
                                               TechPaidDate = dr["TechPaidDate"] != DBNull.Value ? Convert.ToDateTime(dr["TechPaidDate"]) : new DateTime(),
                                               OpenerCommission = dr["OpenerCommission"] != DBNull.Value ? Convert.ToDouble(dr["OpenerCommission"]) : 0,
                                               MiscRep1 = dr["MiscRep1"].ToString(),
                                               MiscCommission1 = dr["MiscCommission1"] != DBNull.Value ? Convert.ToDouble(dr["MiscCommission1"]) : 0,
                                               MiscRep2 = dr["MiscRep2"].ToString(),
                                               MiscCommission2 = dr["MiscCommission2"] != DBNull.Value ? Convert.ToDouble(dr["MiscCommission2"]) : 0,
                                               MiscRep3 = dr["MiscRep3"].ToString(),
                                               MiscCommission3 = dr["MiscCommission3"] != DBNull.Value ? Convert.ToDouble(dr["MiscCommission3"]) : 0,
                                               MiscRep4 = dr["MiscRep4"].ToString(),
                                               MiscCommission4 = dr["MiscCommission4"] != DBNull.Value ? Convert.ToDouble(dr["MiscCommission4"]) : 0,
                                               MiscRep5 = dr["MiscRep5"].ToString(),
                                               MiscCommission5 = dr["MiscCommission5"] != DBNull.Value ? Convert.ToDouble(dr["MiscCommission5"]) : 0,
                                               MiscPaidDate = dr["MiscPaidDate"] != DBNull.Value ? Convert.ToDateTime(dr["MiscPaidDate"]) : new DateTime(),

                                           }).ToList();

            return Model;
        }

        public DataTable PaidCommissionReportListForDownload(DateTime? Start, DateTime? End, string searchtext)
        {
            DataTable dt = _TicketDataAccess.GetPaidCommissionReportListForDownload(Start, End, searchtext);
            return dt;
        }

        public DataTable CancelQueueReportListForDownload(DateTime? Start, DateTime? End, string searchtext)
        {
            DataTable dt = _TicketDataAccess.CancelQueueReportListForDownload(Start, End, searchtext);
            return dt;
        }
        public DataTable BrinksCustomerReportListForDownload(DateTime? Start, DateTime? End, string searchtext)
        {
            DataTable dt = _TicketDataAccess.BrinksCustomerReportListForDownload(Start, End, searchtext);
            return dt;
        }
        public DataTable BrinksFundingVerificationListForDownload(DateTime? Start, DateTime? End, string searchtext)
        {
            DataTable dt = _TicketDataAccess.BrinksFundingVerificationListForDownload(Start, End, searchtext);
            return dt;
        }
        #endregion

        #region Global Ticket Search
        public List<Ticket> GetTicketByKeyAndCompanyId(Guid CompanyId, string key, string emptag, Guid empid)
        {

            DataTable dt = _TicketDataAccess.GetTicketByKeyAndCompanyId(CompanyId, key, emptag, empid);
            List<Ticket> TicketList = new List<Ticket>();
            TicketList = (from DataRow dr in dt.Rows
                           select new Ticket()
                           {
                               Id = dr["TicketId"] != DBNull.Value ? Convert.ToInt32(dr["TicketId"]) : 0,
                               CusIdInt = dr["CusIntId"] != DBNull.Value ? Convert.ToInt32(dr["CusIntId"]) : 0,
                               CompletionDate = dr["CompletionDate"] != DBNull.Value ? Convert.ToDateTime(dr["CompletionDate"]) : new DateTime(),
                               CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                               TicketType = dr["TicketType"].ToString(),
                               Status = dr["Status"].ToString(),
                               BusinessName = dr["BusinessName"].ToString(),
                               CustomerName = dr["CustomerName"].ToString(),
                               PrimaryPhone =dr["PrimaryPhone"].ToString(),
                               AssignedTo = dr["AssignTo"].ToString()
                           }).ToList();
            return TicketList;
        }
        #endregion

        public TicketUser GetTicketTechByTicketId(Guid TicketId)
        {
            DataTable dt = _TicketDataAccess.GetTicketTechByTicketId(TicketId);

            TicketUser _TicketUser = (from DataRow dr in dt.Rows
                                      select new TicketUser()
                                      {
                                          EMPNAME = dr["TechName"].ToString(),
                                      }).FirstOrDefault();
            return _TicketUser;
        }
        public List<TicketReply> GetAllPublicTicketReplyByTicketId(Guid TicketId)
        {
            DataSet ds = _TicketDataAccess.GetAllPublicTicketReplyByTicketId(TicketId);

            List<TicketReply> TicketReplyes = (from DataRow dr in ds.Tables[0].Rows
                                               select new TicketReply()
                                               {
                                                   RepliedDate = dr["RepliedDate"] != DBNull.Value ? Convert.ToDateTime(dr["RepliedDate"]) : new DateTime(),
                                                   Message = dr["Message"].ToString(),
                                                   CreatedByVal = dr["NoteBy"].ToString()
                                               }).ToList();
            return TicketReplyes;
        }
    }
}
