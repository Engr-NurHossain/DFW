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
	public partial class TicketBookingExtraItemDataAccess
	{
        //Delete By Booking ID
        public bool DeleteTicketBookingExtraItemByBookingId(int bookingId)
        {
            string SqlQuery = @" DELETE from TicketBookingExtraItem where BookingId = '{0}' ";
            SqlQuery = string.Format(SqlQuery, bookingId);
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
        public bool DeleteTicketBookingExtraItemById(int Id)
        {
            string SqlQuery = @" DELETE from TicketBookingExtraItem where Id = '{0}' ";
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
