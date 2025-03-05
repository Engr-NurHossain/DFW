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
	public partial class CustomerAddressDataAccess
	{
        public DataTable GetAllDiffrentAddressByCustomerId(Guid customerId)
        {
            string sqlQuery = @"select Street,city,state,zipcode from customeraddress where customerid = '{0}' 
                                group by Street,city,state,zipcode";
            try
            {
                sqlQuery = string.Format(sqlQuery, customerId);
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
