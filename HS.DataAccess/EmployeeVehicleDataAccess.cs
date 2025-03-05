using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.DataAccess
{
	public partial class EmployeeVehicleDataAccess
	{
        public bool DeleteEmployeeVehicleByEmployeeId(Guid employeeId)
        {
            string sqlQuery = @"delete from EmployeeVehicle where EmployeeId ='{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, employeeId);

                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
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
        public bool DeleteEmployeeVehicleByVehicleId(Guid vehicleid)
        {
            string sqlQuery = @"delete from EmployeeVehicle where VehicleId ='{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, vehicleid);

                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
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
        public bool DeleteAllEmployeeVehicleByVehicleId(Guid vehicleId)
        {
            string sqlQuery = @"delete from EmployeeVehicle where VehicleId ='{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, vehicleId);

                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
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
        public DataTable GetAllEmployeeVehicleByVehicleId(Guid VehicleId)
        {
            string sqlQuery = @" select ev.*,emp.FirstName+' '+emp.LastName as EmployeeName from EmployeeVehicle ev
                                 left join Employee emp on emp.UserId = ev.EmployeeId 
                                 where VehicleId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, VehicleId);
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
