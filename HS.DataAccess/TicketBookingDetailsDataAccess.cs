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
	public partial class TicketBookingDetailsDataAccess
	{
        public TicketBookingDetailsDataAccess(string ConStr) : base(ConStr) { }
        public DataTable GetBookingDetialsListByBookingId(string bookingId)
        {
            string sqlQuery = @" Select * from TicketBookingDetails
                                where BookingId = '{0}' ";
            try
            {
                sqlQuery = string.Format(sqlQuery, bookingId);
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

        public bool DeleteTicketBookingDetailsByBookingId(string bookingId)
        {
            string SqlQuery = @" DELETE from TicketBookingDetails where BookingId = '{0}' ";
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
        public bool DeleteTicketBookingDetailsById(int bookingId)
        {
            string SqlQuery = @" DELETE from TicketBookingDetails where Id = {0} ";
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
        public bool DeleteTicketBookingDetailsExcludingIdByBookingId(string bookingId, string detailsId)
        {
            string SqlQuery = @" delete from TicketBookingDetails where BookingId ='{0}' and Id not in ({1}) ";
            SqlQuery = string.Format(SqlQuery, bookingId, detailsId);
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
