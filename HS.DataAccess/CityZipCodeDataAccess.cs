using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.DataAccess
{
	public partial class CityZipCodeDataAccess
	{
        public CityZipCodeDataAccess(string ConnectionString) : base(ConnectionString) { }
        public DataTable GetLeadCityStateListBySearchKey(string key, int MaxLoad)
        {
            string sqlQuery = @"select
                                Top({1})
                                cz.ZipCode, cz.City, cz.State, cz.County
                                from CityZipCode cz
                                where cz.ZipCode like '{0}%'
                                order by cz.State";
            try
            {
                sqlQuery = string.Format(sqlQuery, key, MaxLoad);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }	
}
