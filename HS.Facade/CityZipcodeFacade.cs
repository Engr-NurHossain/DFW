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
    public class CityZipcodeFacade : BaseFacade
    {
        public CityZipcodeFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        CityZipCodeDataAccess _CityZipCodeDataAccess
        {
            get
            {
                return (CityZipCodeDataAccess)_ClientContext[typeof(CityZipCodeDataAccess)];
            }
        }

        public List<CityZipCode> GetLeadCityStateListBySearchKey(string key, int MaxLoad)
        {
            DataTable dt = _CityZipCodeDataAccess.GetLeadCityStateListBySearchKey(key, MaxLoad);
            List<CityZipCode> LeadCityStateSearchList = new List<CityZipCode>();
            LeadCityStateSearchList = dt.AsEnumerable().Select(dataRow => new CityZipCode
            {
                ZipCode = dataRow.Field<string>("ZipCode"),
                City = dataRow.Field<string>("City").UppercaseFirst(),
                State = dataRow.Field<string>("State"),
                County = dataRow.Field<string>("County")
            }).ToList();
            return LeadCityStateSearchList;
        }
    }
}
