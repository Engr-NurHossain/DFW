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
	public partial class CustomerCancellationReasonDataAccess
	{
        public bool DeleteCustomerCancellationReasonByCustomerId(Guid CustomerId)
        {
            string SqlQuery = @"
                            delete from CustomerCancellationReason where CustomerId ='{0}'
                ";
            SqlQuery = string.Format(SqlQuery, CustomerId);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(SqlQuery))
                {

                    ExecuteCommand(cmd);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }	
}
