using HS.DataAccess;
using HS.Entities;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace HS.Facade
{
    public class ZipCodeAPIFacade : BaseFacade
    {
        CityZipCodeDataAccess _CityZipCodeDataAccess = null;
        CityZipCodeSearchLogDataAccess _CityZipCodeSearchLogDataAccess = null;
        LookupDataAccess _LookupDataAccess = null;

        public ZipCodeAPIFacade(ClientContext clientContext) : base(clientContext)
        {
            if (_CityZipCodeDataAccess == null)
                _CityZipCodeDataAccess = new CityZipCodeDataAccess(clientContext);
            if (_CityZipCodeSearchLogDataAccess == null)
                _CityZipCodeSearchLogDataAccess = new CityZipCodeSearchLogDataAccess(clientContext);
            if (_LookupDataAccess == null)
                _LookupDataAccess = new LookupDataAccess(clientContext);
        }
        public ZipCodeAPIFacade()
        {
            if (_CityZipCodeDataAccess == null)
            {
                string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["hsCoonectionString"].ConnectionString;
                _CityZipCodeDataAccess = new CityZipCodeDataAccess(ConnectionString);
            }
            if (_CityZipCodeSearchLogDataAccess == null)
            {
                string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["hsCoonectionString"].ConnectionString;
                _CityZipCodeSearchLogDataAccess = new CityZipCodeSearchLogDataAccess(ConnectionString);
            }
            if (_LookupDataAccess == null)
            {
                string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["hsCoonectionString"].ConnectionString;
                _LookupDataAccess = new LookupDataAccess(ConnectionString);
            }
        }
        public ZipCodeAPIFacade(string ConnectionStr)
        {
            if (_CityZipCodeDataAccess == null)
                _CityZipCodeDataAccess = new CityZipCodeDataAccess(ConnectionStr);
            if (_CityZipCodeSearchLogDataAccess == null)
                _CityZipCodeSearchLogDataAccess = new CityZipCodeSearchLogDataAccess(ConnectionStr);
            if (_LookupDataAccess == null)
                _LookupDataAccess = new LookupDataAccess(ConnectionStr);
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
                County = dataRow.Field<string>("County").UppercaseFirst(),

            }).ToList();
            return LeadCityStateSearchList;
        }

        public long InsertSearchLog(CityZipCodeSearchLog log)
        {
            return _CityZipCodeSearchLogDataAccess.Insert(log);
        }
        public bool InsertLookUp(Lookup lookup)
        {
            return _LookupDataAccess.Insert(lookup) > 0;
        }
    }
}
