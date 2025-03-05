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
	public partial class RecurringBillingScheduleItemsDataAccess
	{
        public RecurringBillingScheduleItemsDataAccess() { }
        public RecurringBillingScheduleItemsDataAccess(string ConStr) : base(ConStr) { }
        public bool DeleteRecurringBillingScheduleItemsByScheduleId(Guid ScheduleId)
        {
            string SqlQuery = @"delete from RecurringBillingScheduleItems where ScheduleId ='{0}' ";
            SqlQuery = string.Format(SqlQuery, ScheduleId);
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
