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
    public partial class CustomerAppointmentTechnicianDataAccess
    {
        public bool DeleteAppointmentAllTech(int appId)
        { 
            string SqlQuery = @"delete from CustomerAppointmentTechnician 
                                where CustomerAppointmentId = '{0}'";
            try
            {
                SqlQuery = string.Format(SqlQuery, appId);
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
