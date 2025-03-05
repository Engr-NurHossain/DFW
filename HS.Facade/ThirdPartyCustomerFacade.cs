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
    public class ThirdPartyCustomerFacade : BaseFacade
    {
        ThirdPartyCustomerDataAccess _ThirdPartyCustomerDataAccess = null;


        public ThirdPartyCustomerFacade(ClientContext clientContext)
            : base(clientContext)
        {
            if (_ThirdPartyCustomerDataAccess == null)
                _ThirdPartyCustomerDataAccess = (ThirdPartyCustomerDataAccess)_ClientContext[typeof(ThirdPartyCustomerDataAccess)];

        }
        public ThirdPartyCustomerFacade(string ConStr)
        {
            if (_ThirdPartyCustomerDataAccess == null)
                _ThirdPartyCustomerDataAccess = new ThirdPartyCustomerDataAccess(ConStr);

        }
        public DataTable GetBrinksReportExport(DateTime? Start, DateTime? End)
        {
            return _ThirdPartyCustomerDataAccess.GetBrinksReportExport(Start, End);
        }
        public bool UpdateBrinksCustomerToSold(DateTime? Start, DateTime? End)
        {
            return _ThirdPartyCustomerDataAccess.UpdateBrinksToSold(Start, End);
        }
        public List<ThirdPartyCustomer> GetThirdPartyCustomerByIsSold(bool isSold)
        {
            return _ThirdPartyCustomerDataAccess.GetThirdPartyCustomerByIsSold(isSold);
        }
        public ThirdPartyCustomer GetThirdPartyCustomersByIsSold(bool isSold, DateTime? Start, DateTime? End,string order)
        {
            DataSet dsResult = _ThirdPartyCustomerDataAccess.GetThirdPartyCustomersByIsSold(isSold,Start,End,order);
            DataTable dt = dsResult.Tables[0];
            //DataTable dt1 = dsResult.Tables[1];
            //DataTable dt2 = dsResult.Tables[2];
            List<ThirdPartyCustomer> propertyList = new List<ThirdPartyCustomer>();
       

            propertyList = (from DataRow dr in dt.Rows
                            select new ThirdPartyCustomer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                CustomerId = (Guid)dr["CustomerId"],
                                //Address = dr["Address"].ToString(),
                                eContact = dr["eContact"].ToString(),
                                CustomerNumber = dr["CustomerNumber"] != DBNull.Value ? Convert.ToInt32(dr["CustomerNumber"]) : 0,
                                CustomerLastname = dr["LastName"].ToString(),
                                TransectionID = dr["TransId"].ToString(),
                                DealerNumber = dr["DealerNumber"].ToString(),
                                TempOfcontact = dr["OfContract"] != DBNull.Value ? Convert.ToInt32(dr["OfContract"]) : 0,
                                
                                AccountOnlineDate = dr["AccountOnlineDate"] != DBNull.Value ? Convert.ToDateTime(dr["AccountOnlineDate"]) : new DateTime(),
                                InstallDate = dr["InstallDate"] != DBNull.Value ? Convert.ToDateTime(dr["InstallDate"]) : new DateTime(),

                                //CreatedName = dr["CreatedName"].ToString()
                            }).ToList();


     

            ThirdPartyCustomer LeadList = new ThirdPartyCustomer();
            LeadList.ThirdPartyCustomerList = propertyList;
            return LeadList;
        }
        public ThirdPartyCustomer GetThirdPartyUccCustomersOfNoAgency( DateTime? Start, DateTime? End)
        {
            DataSet dsResult = _ThirdPartyCustomerDataAccess.GetThirdPartyUccCustomersOfNoAgency(Start, End);
            DataTable dt = dsResult.Tables[0];
            //DataTable dt1 = dsResult.Tables[1];
            //DataTable dt2 = dsResult.Tables[2];
            List<ThirdPartyCustomer> propertyList = new List<ThirdPartyCustomer>();


            propertyList = (from DataRow dr in dt.Rows
                            select new ThirdPartyCustomer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                CustomerInt = dr["CustomerInt"] != DBNull.Value ? Convert.ToInt32(dr["CustomerInt"]) : 0,
                                CustomerId = (Guid)dr["CustomerId"],
                                //Address = dr["Address"].ToString(),
                    
                                CustomerNumber = dr["CustomerNumber"] != DBNull.Value ? Convert.ToInt32(dr["CustomerNumber"]) : 0,
                            
                        
                       
                        
                                SiteName = dr["SiteName"].ToString(),
                                AccountOnlineDate = dr["AccountOnlineDate"] != DBNull.Value ? Convert.ToDateTime(dr["AccountOnlineDate"]) : new DateTime(),
                            

                        
                            }).ToList();




            ThirdPartyCustomer LeadList = new ThirdPartyCustomer();
            LeadList.ThirdPartyCustomerList = propertyList;
            return LeadList;
        }
        public bool InsertThirdPartyCustomer(ThirdPartyCustomer tc)
        {
            return _ThirdPartyCustomerDataAccess.Insert(tc) > 0;
        }
        public bool UpdateThirdPartyCustomer(ThirdPartyCustomer tc)
        {
            return _ThirdPartyCustomerDataAccess.Update(tc) > 0;
        }

    }
}
