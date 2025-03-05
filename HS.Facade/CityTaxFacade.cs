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
    public class CityTaxFacade : BaseFacade
    {
        public CityTaxFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        CityTaxDataAccess _CityTaxDataAccess
        {
            get
            {
                return (CityTaxDataAccess)_ClientContext[typeof(CityTaxDataAccess)];
            }
        }

        RestrictedZipCodeDataAccess _RestrictedZipCodeDataAccess
        {
            get
            {
                return (RestrictedZipCodeDataAccess)_ClientContext[typeof(RestrictedZipCodeDataAccess)];
            }
        }

        public List<RestrictedZipCode> GetAllRestrictedZipCode()
        {
            return _RestrictedZipCodeDataAccess.GetAll();
        }
        public CityTax GetCityTaxById(int value)
        {
            return _CityTaxDataAccess.Get(value);
        }

        
        public List<CityTax> GetAllCityTaxByCompanyId(Guid Companyid)
        {
            return _CityTaxDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", Companyid));
        }
        public bool UpdateCityTax(CityTax cb)
        {
            return _CityTaxDataAccess.Update(cb) > 0;
        }
        public long InsertCityTax(CityTax cb)
        {
            return _CityTaxDataAccess.Insert(cb);
        }
        public bool DeleteCityTax(int Id)
        {
            return _CityTaxDataAccess.Delete(Id) > 0;
        }
        public long DeleteRestrictedZipCode(int Id)
        {
            return _RestrictedZipCodeDataAccess.Delete(Id);
        }
        public long InsertRestrictedZipCode(RestrictedZipCode restrictedZipCode)
        {
            return _RestrictedZipCodeDataAccess.Insert(restrictedZipCode);
        }
        public RestrictedZipCode GetRestrictedZipCode(int pageno, int pagesize,string searchtext)
        {
            DataSet dsResult = _RestrictedZipCodeDataAccess.GetAllRestrictedZipCode( pageno, pagesize, searchtext);
            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];
          
            List<RestrictedZipCode> propertyList = new List<RestrictedZipCode>();
       

            propertyList = (from DataRow dr in dt.Rows
                            select new RestrictedZipCode()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,

                          
                                Zipcode = dr["Zipcode"].ToString(),
                                CreatedBy = dr["CreatedBy"].ToString(),
                                CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),

                                
                            }).ToList();


         

            RestrictedZipCode LeadList = new RestrictedZipCode();
            LeadList.TotalCount = dsResult.Tables[1].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(dsResult.Tables[1].Rows[0]["TotalCount"]) : 0;
            LeadList.restrictedzipcodeList = propertyList;


            return LeadList;
        }
        public RestrictedZipCode GetRestrictedZipcodelistbyzipcode( string searchtext)
        {
            DataSet dsResult = _RestrictedZipCodeDataAccess.GetRestrictedZipcodelistbyzipcode( searchtext);
            DataTable dt = dsResult.Tables[0];
  

            List<RestrictedZipCode> propertyList = new List<RestrictedZipCode>();


            propertyList = (from DataRow dr in dt.Rows
                            select new RestrictedZipCode()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,


                                Zipcode = dr["Zipcode"].ToString(),
                                CreatedBy = dr["CreatedBy"].ToString(),
                                CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),


                            }).ToList();




            RestrictedZipCode LeadList = new RestrictedZipCode();
       
            LeadList.restrictedzipcodeList = propertyList;
            LeadList.TotalCount = dsResult.Tables[1].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(dsResult.Tables[1].Rows[0]["TotalCount"]) : 0;


            return LeadList;
        }
    }
}
