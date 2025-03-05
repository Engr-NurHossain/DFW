using HS.DataAccess;
using HS.Entities;
using HS.Entities.API;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;


namespace HS.Facade
{
    public class ApiFacade : BaseFacade
    {
        CustomerDataAccess _CustomerDataAccess = null;
        TicketDataAccess _TicketDataAccess = null;
        UserLoginDataAccess _UserLoginDataAccess = null;
        CustomerAppointmentDataAccess _CustomerAppointmentDataAccess = null;
        LookupDataAccess _LookupDataAccess = null;
        GlobalSettingDataAccess _GlobalSettingDataAccess = null;
        CustomerCompanyDataAccess _CustomerCompanyDataAccess = null;
        OpportunityDataAccess _OpportunityDataAccess = null;
        UserOrganizationDataAccess _UserOrganizationDataAccess = null; 
        public ApiFacade()
        {
            string constr  = System.Configuration.ConfigurationManager.ConnectionStrings["hsCoonectionStringGrateCrmApi"].ConnectionString;
            if (_CustomerCompanyDataAccess == null)
                _CustomerCompanyDataAccess = new CustomerCompanyDataAccess(constr);
            if (_UserOrganizationDataAccess == null)
                _UserOrganizationDataAccess = new UserOrganizationDataAccess(constr);
        }
        #region Previous
        //public ApiFacade()
        //{
        //    if (_CustomerDataAccess == null)
        //    {
        //        string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["hsCoonectionStringGrateCrmApi"].ConnectionString;
        //        _CustomerDataAccess = new CustomerDataAccess(ConnectionString);
        //    }
        //    if (_UserLoginDataAccess == null)
        //    {
        //        string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["hsCoonectionStringGrateCrmApi"].ConnectionString;
        //        _UserLoginDataAccess = new UserLoginDataAccess(ConnectionString);
        //    }
        //    if (_CustomerAppointmentDataAccess == null)
        //    {
        //        string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["hsCoonectionStringGrateCrmApi"].ConnectionString;
        //        _CustomerAppointmentDataAccess = new CustomerAppointmentDataAccess(ConnectionString);
        //    }
        //    if (_LookupDataAccess == null)
        //    {
        //        string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["hsCoonectionStringGrateCrmApi"].ConnectionString;
        //        _LookupDataAccess = new LookupDataAccess(ConnectionString);
        //    }
        //    if (_GlobalSettingDataAccess == null)
        //    {
        //        string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["hsCoonectionStringGrateCrmApi"].ConnectionString;
        //        _GlobalSettingDataAccess = new GlobalSettingDataAccess(ConnectionString);
        //    }
        //    if (_CustomerCompanyDataAccess == null)
        //    {
        //        string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["hsCoonectionStringGrateCrmApi"].ConnectionString;
        //        _CustomerCompanyDataAccess = new CustomerCompanyDataAccess(ConnectionString);
        //    }
        //}
        #endregion
        public ApiFacade(string ConnectionString)
        {
            if (_CustomerDataAccess == null)
                _CustomerDataAccess = new CustomerDataAccess(ConnectionString);
            if (_UserLoginDataAccess == null)
                _UserLoginDataAccess = new UserLoginDataAccess(ConnectionString);
            if (_CustomerAppointmentDataAccess == null)
                _CustomerAppointmentDataAccess = new CustomerAppointmentDataAccess(ConnectionString);
            if (_LookupDataAccess == null)
                _LookupDataAccess = new LookupDataAccess(ConnectionString);
            if (_GlobalSettingDataAccess == null)
                _GlobalSettingDataAccess = new GlobalSettingDataAccess(ConnectionString);
            if (_CustomerCompanyDataAccess == null)
                _CustomerCompanyDataAccess = new CustomerCompanyDataAccess(ConnectionString);
            if (_OpportunityDataAccess == null)
                _OpportunityDataAccess = new OpportunityDataAccess(ConnectionString);
            if (_TicketDataAccess == null)
                _TicketDataAccess = new TicketDataAccess(ConnectionString);
        }

        public List<CustomerAPI.CustomerAPIModels> GetAllCustomer(int id, Guid customerid)
        {
            var dt = _CustomerDataAccess.GetAllCustomerForAPI(id,customerid);
            var CustomerSearchList = new List<CustomerAPI.CustomerAPIModels>();
            CustomerSearchList = dt.AsEnumerable().Select(dataRow => new CustomerAPI.CustomerAPIModels
            {
                id = dataRow.Field<int>("id"),
                guid = dataRow.Field<Guid>("guid"),
                firstName = dataRow.Field<string>("firstName"),
                lastName = dataRow.Field<string>("lastName"),
                businessName = dataRow.Field<string>("businessName"),
                primaryPhone = dataRow.Field<string>("primaryPhone"),
                secondaryPhone = dataRow.Field<string>("secondaryPhone"),
                type = dataRow.Field<string>("type"),
                street = dataRow.Field<string>("street"),
                street2 = dataRow.Field<string>("street2"),
                city = dataRow.Field<string>("city"),
                city2 = dataRow.Field<string>("city2"),
                state = dataRow.Field<string>("state"),
                state2 = dataRow.Field<string>("state2"),
                zipCode = dataRow.Field<string>("zipCode"),
                zipCode2 = dataRow.Field<string>("zipCode2"),  
                emailAddress = dataRow.Field<string>("emailAddress"),
            }).ToList();
            return CustomerSearchList;
        } 
        public List<TicketAPI.TicketAPIModels> GetAllTickets(int Id, int CustomerId, Guid CustomerGuid)
        {
            DataTable dt = _TicketDataAccess.GetAllTicketsForAPI(Id, CustomerId, CustomerGuid);
            var TicketSearchList = new List<TicketAPI.TicketAPIModels>();
            TicketSearchList = dt.AsEnumerable().Select(dataRow => new TicketAPI.TicketAPIModels
            {
                id = dataRow.FieldOrDefault<int>("id"),
                customerguid = dataRow.FieldOrDefault<Guid>("customerguid"),
                customerId = dataRow.FieldOrDefault<int>("customerId"),
                appoinmentType = dataRow.Field<string>("appoinmentType"),
                customerName = dataRow.Field<string>("customerName"),
                customerBusinessName = dataRow.Field<string>("customerBusinessName"),
                appoinmentDate = dataRow.FieldOrDefault<DateTime>("appoinmentDate"),
                message = dataRow.Field<string>("message"),
                status = dataRow.Field<string>("status"),
                createdByGuid = dataRow.FieldOrDefault<Guid>("createdByGuid"),
                createdByName = dataRow.Field<string>("createdByName"),
                createdDate = dataRow.FieldOrDefault<DateTime>("createdDate"),
                assignedToGuid = dataRow.FieldOrDefault<Guid>("assignedToGuid"),
                assignedToName = dataRow.Field<string>("assignedToName"),
                startTime = dataRow.Field<string>("startTime"),
                endTime = dataRow.Field<string>("endTime"),
            }).ToList();
            return TicketSearchList;
        } 
        public List<OpportunityAPI.OpportunityAPIModels> GetAllOpportunities()
        {
            var dt = _OpportunityDataAccess.GetAllOpportunityForAPI();
            var OPList = new List<OpportunityAPI.OpportunityAPIModels>();
            OPList = (from DataRow dr in dt.Rows
                                       select new OpportunityAPI.OpportunityAPIModels()
                                       {
                                           accountId = dr["accountId"] != DBNull.Value ? Convert.ToInt32(dr["accountId"]) : 0,
                                           accountGuid = (Guid)dr["accountGuid"],
                                           description = dr["description"].ToString(),
                                           id = dr["id"] != DBNull.Value ? Convert.ToInt32(dr["id"]) : 0,
                                           status = dr["status"].ToString(),
                                           title = dr["title"].ToString(),

                                       }).ToList();
            return OPList; 
        }

        public CompanyConneciton GetCompanyConnectionByUserName(string userName)
        {
            DataTable dt = _UserOrganizationDataAccess.GetCompanyConnectionByUsername(userName);
            List<CompanyConneciton> CompanyConnecitonList = new List<CompanyConneciton>();
            if (dt == null)
            {

            }
            else if (dt.Rows.Count > 0)
            {
                CompanyConnecitonList = (from DataRow dr in dt.Rows
                                         select new CompanyConneciton()
                                         {
                                             CompanyId = (Guid)dr["CompanyId"],
                                             ConnectionString = dr["ConnectionString"].ToString(),
                                             MasterPassword = dr["MasterPassword"].ToString()
                                         }).ToList();
            }
            return CompanyConnecitonList.FirstOrDefault();
        }

        //public CompanyConneciton GetBrinksCustomerFromApi(BrinksCredentialModel model)
        //{
        //    HS.Brinks.net.monitronics.senti.WSI wsi = new HS.Brinks.net.monitronics.senti.WSI();
        //    var result = wsi.GetData(Entity, UserId, Password, CsNumber, null);

        //    DataTable dt = _UserOrganizationDataAccess.GetCompanyConnectionByUsername(userName);
        //    List<CompanyConneciton> CompanyConnecitonList = new List<CompanyConneciton>();
        //    if (dt == null)
        //    {

        //    }
        //    else if (dt.Rows.Count > 0)
        //    {
        //        CompanyConnecitonList = (from DataRow dr in dt.Rows
        //                                 select new CompanyConneciton()
        //                                 {
        //                                     CompanyId = (Guid)dr["CompanyId"],
        //                                     ConnectionString = dr["ConnectionString"].ToString(),
        //                                     MasterPassword = dr["MasterPassword"].ToString()
        //                                 }).ToList();
        //    }
        //    return CompanyConnecitonList.FirstOrDefault();
        //}
        public List<Lookup> GetLookupByKey(string key)
        {
            List<Lookup> resultLookup = new List<Lookup>();
            resultLookup = _LookupDataAccess.GetByQuery(string.Format(" DataKey = '{0}'  order by DataOrder ", key));
            return resultLookup;
        }
        public Customer GetCustomerDetailsByApi(string phone)
        {   
            var query = string.Format("REPLACE(PrimaryPhone,'-','') = '{0}' OR REPLACE(SecondaryPhone,'-','') = '{0}'", phone);
            return _CustomerDataAccess.GetByQuery(query).FirstOrDefault();
        }
        public Customer GetCustomerDetailsByApicNumber(string phone, string cNumber)
        {
            var query = string.Format("(REPLACE(PrimaryPhone,'-','') = '{0}' OR REPLACE(SecondaryPhone,'-','') = '{0}') AND Id={1}", phone, cNumber);
            return _CustomerDataAccess.GetByQuery(query).FirstOrDefault();
        }
        public Customer GetCustomerDetailsBycNumber(string cNumber)
        {
            return _CustomerDataAccess.Get(Convert.ToInt32(cNumber));
        }
        public bool InsertCustomer(Customer customer)
        {
            customer.SearchText = string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9}, {10} {11} {12} {13} {14}",
                   /*{0}*/  customer.FirstName,
                   /*{1}*/  customer.LastName,
                   /*{1}*/  customer.BusinessName,
                   /*{2}*/  customer.Id.ToString(),
                   /*{3}*/  customer.MiddleName,
                   /*{4}*/  customer.CustomerNo,
                   /*{5}*/  customer.SecondCustomerNo,
                   /*{6}*/  customer.Type,
                   /*{7}*/  customer.Street,
                   /*{8}*/  customer.ZipCode,
                   /*{9}*/  customer.City,
                   /*{10}*/  customer.State,
                   /*{11}*/  customer.PrimaryPhone,
                   /*{12}*/  customer.CellNo,
                   /*{13}*/  customer.SecondaryPhone,
                   /*{14}*/  customer.EmailAddress);
            return _CustomerDataAccess.Insert(customer) > 0;
        }
        public bool InsertCustomerCompany(CustomerCompany customerCompanyDetails)
        {
            return _CustomerCompanyDataAccess.Insert(customerCompanyDetails) > 0;
        }
        public bool InsertCustomerApt(CustomerAppointment customerAptDetails)
        {
            return _CustomerAppointmentDataAccess.Insert(customerAptDetails) > 0;
        }
        public bool InsertLookUp(Lookup lookup)
        {
            return _LookupDataAccess.Insert(lookup) > 0;
        }
        public UserLogin GetUserLoginByUserPass(string userName, string password)
        {
            var query = "UserName='" + userName + "' And Password ='" + password + "'";
            return _UserLoginDataAccess.GetByQuery(query).FirstOrDefault();
        }

        public CustomerAppointment GetCustomerAptByNumber(string number)
        {
            var dt = _CustomerAppointmentDataAccess.GetCustomerAptByNumber(number);
            var customerAppointmentList = new List<CustomerAppointment>();
            customerAppointmentList = (from DataRow dr in dt.Rows
                                       select new CustomerAppointment()
                                       {
                                           AppointmentDate = dr["AppointmentDate"] != DBNull.Value ? Convert.ToDateTime(dr["AppointmentDate"]) : DateTime.Now,
                                           AppointmentStartTime = dr["AppointmentStartTime"].ToString(),
                                           AppointmentEndTime = dr["AppointmentEndTime"].ToString(),
                                           ServiceArea = dr["ServiceArea"].ToString()
                                       }).ToList();
            return customerAppointmentList.FirstOrDefault();
        }

        public CustomerAppointment GetCustomerAptByZipCode(string zipCode)
        {
            var dt = _CustomerAppointmentDataAccess.GetCustomerAptByZipCode(zipCode);
            var customerAppointmentList = new List<CustomerAppointment>();
            customerAppointmentList = (from DataRow dr in dt.Rows
                                       select new CustomerAppointment()
                                       {
                                           AppointmentDate = dr["AppointmentDate"] != DBNull.Value ? Convert.ToDateTime(dr["AppointmentDate"]) : DateTime.Now,
                                           AppointmentStartTime = dr["AppointmentStartTime"].ToString(),
                                           AppointmentEndTime = dr["AppointmentEndTime"].ToString(),
                                           ServiceArea = dr["ServiceArea"].ToString()
                                       }).ToList();
            return customerAppointmentList.FirstOrDefault();
        }
        public List<CustomerAppointment> GetCustomerAptListByZipCode(string zipCode)
        {
            var dt = _CustomerAppointmentDataAccess.GetCustomerAptByZipCode(zipCode);
            var customerAppointmentList = new List<CustomerAppointment>();
            if (dt != null && dt.Rows.Count > 0)
                customerAppointmentList = (from DataRow dr in dt.Rows
                                           select new CustomerAppointment()
                                           {
                                               AppointmentDate = dr["AppointmentDate"] != DBNull.Value ? Convert.ToDateTime(dr["AppointmentDate"]) : DateTime.Now,
                                               AppointmentStartTime = dr["AppointmentStartTime"].ToString(),
                                               AppointmentEndTime = dr["AppointmentEndTime"].ToString(),
                                               ServiceArea = dr["ServiceArea"].ToString()
                                           }).ToList();
            return customerAppointmentList;
        }
        public List<CustomerAppointment> GetCustomerAptByDate(DateTime appointmentDate)
        {
            var dt = _CustomerAppointmentDataAccess.GetCustomerAptByDate(appointmentDate);
            var customerAppointmentList = new List<CustomerAppointment>();
            customerAppointmentList = (from DataRow dr in dt.Rows
                                       select new CustomerAppointment()
                                       {
                                           AppointmentDate = dr["AppointmentDate"] != DBNull.Value ? Convert.ToDateTime(dr["AppointmentDate"]) : DateTime.Now,
                                           AppointmentStartTime = dr["AppointmentStartTime"].ToString(),
                                           AppointmentEndTime = dr["AppointmentEndTime"].ToString(),
                                           ServiceArea = dr["ServiceArea"].ToString()
                                       }).ToList();
            return customerAppointmentList;
        }
        public List<CustomerAppointment> GetCustomerAptByDateAndZipCode(DateTime appointmentDate, string ZipCode)
        {
            var dt = _CustomerAppointmentDataAccess.GetCustomerAptByDateAndZipCode(appointmentDate, ZipCode);
            var customerAppointmentList = new List<CustomerAppointment>();
            customerAppointmentList = (from DataRow dr in dt.Rows
                                       select new CustomerAppointment()
                                       {
                                           AppointmentDate = dr["AppointmentDate"] != DBNull.Value ? Convert.ToDateTime(dr["AppointmentDate"]) : DateTime.Now,
                                           AppointmentStartTime = dr["AppointmentStartTime"].ToString(),
                                           AppointmentEndTime = dr["AppointmentEndTime"].ToString(),
                                           ServiceArea = dr["ServiceArea"].ToString()
                                       }).ToList();
            return customerAppointmentList;
        }
        public CustomerAppointment GetCustomerAptByDateAndCustomerNumberTimeFrame(DateTime appointmentDate, string CustomerNumber,string StartTime,string EndTime)
        {
            var dt = _CustomerAppointmentDataAccess.GetCustomerAptByDateAndCustomerNumber(appointmentDate, CustomerNumber, StartTime, EndTime);
            var customerAppointmentList = new List<CustomerAppointment>();
            customerAppointmentList = (from DataRow dr in dt.Rows
                                       select new CustomerAppointment()
                                       {
                                           AppointmentDate = dr["AppointmentDate"] != DBNull.Value ? Convert.ToDateTime(dr["AppointmentDate"]) : DateTime.Now,
                                           AppointmentStartTime = dr["AppointmentStartTime"].ToString(),
                                           AppointmentEndTime = dr["AppointmentEndTime"].ToString(),
                                           ServiceArea = dr["ServiceArea"].ToString()
                                       }).ToList();
            return customerAppointmentList.FirstOrDefault();
        }

        public CustomerAppointment GetCustomerAptByANIAndDateAndCustomerNumber(string ANI, DateTime appointmentDate, string CustomerNumber)
        {
            var dt = _CustomerAppointmentDataAccess.GetCustomerAptByANIAndDateAndCustomerNumber(ANI, appointmentDate, CustomerNumber);
            var customerAppointmentList = new List<CustomerAppointment>();
            customerAppointmentList = (from DataRow dr in dt.Rows
                                       select new CustomerAppointment()
                                       {
                                           AppointmentDate = dr["AppointmentDate"] != DBNull.Value ? Convert.ToDateTime(dr["AppointmentDate"]) : DateTime.Now,
                                           AppointmentStartTime = dr["AppointmentStartTime"].ToString(),
                                           AppointmentEndTime = dr["AppointmentEndTime"].ToString(),
                                           ServiceArea = dr["ServiceArea"].ToString()
                                       }).ToList();
            return customerAppointmentList.FirstOrDefault();
        }
        public GlobalSetting GetGlobalSettingsByOnlyKey(string searchKey)
        {
            return _GlobalSettingDataAccess.GetByQuery(string.Format("SearchKey = '{0}'", searchKey)).FirstOrDefault();
        }

        public List<Customer> GetCustomerList()
        {
            return _CustomerDataAccess.GetAll();
        }
    }
}
