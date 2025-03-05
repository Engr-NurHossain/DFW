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
	public partial class CustomerSignatureDataAccess
	{
        public CustomerSignatureDataAccess() { }
        public CustomerSignatureDataAccess(string ConnectionStr) : base(ConnectionStr) { }



        public bool DeleteAllSignatureByType(Guid customerId, string type)
        {
            string SqlQuery = @"delete from CustomerSignature where type = '{0}' and CustomerId = '{1}' ";
            try
            {
                SqlQuery = string.Format(SqlQuery,type,customerId);
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
