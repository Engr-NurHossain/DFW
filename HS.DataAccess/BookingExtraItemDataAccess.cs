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
    public partial class BookingExtraItemDataAccess
    {
        public BookingExtraItemDataAccess() { }
        public BookingExtraItemDataAccess(string ConnectionString) : base(ConnectionString)
        {

        }
        public bool DeleteAllBookingExtraItemByBookingId(int id)
        {
            string SqlQuery = @" DELETE FROM BookingExtraItem WHERE BookingId ='{0}' ";
            SqlQuery = string.Format(SqlQuery, id);
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

        public bool DeleteTicketBookingExtraItemExcludingIdByBookingId(int bookingIntId, string idList)
        {
            string SqlQuery = @" delete from TicketBookingExtraItem where BookingId ='{0}' and Id not in ({1}) ";
            SqlQuery = string.Format(SqlQuery, bookingIntId, idList);
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
