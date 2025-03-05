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
	public partial class TicketFileDataAccess
	{
        public TicketFileDataAccess(string ConStr) : base(ConStr) { }
        public bool DeleteTicketFilesByBookingDetailsId(int Id)
        {
            string SqlQuery = @"delete from TicketFile where TicketBookingDetailsId = '{0}'";
            SqlQuery = string.Format(SqlQuery, Id);
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
