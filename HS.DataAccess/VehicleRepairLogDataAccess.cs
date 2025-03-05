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
    public partial class VehicleRepairLogDataAccess
    {
        public List<VehicleRepairLog> GetAllRepairLogsbyVehicleId(Guid vehicleId)
        {
            string sqlQuery = @"select vrl.* ,
                                vd.VIN as VIN,
                                empDriver.FirstName + ' '+empDriver.LastName  as DriverName,
                                empCreatedBy.FirstName + ' '+empCreatedBy.LastName  as CreatedBy

                                from VehicleRepairLog vrl
                                left join VehicleDetail vd on vd.VehicleId = vrl.VehicleId
                                left join Employee empDriver on vrl.Driver = empDriver.UserId
                                left join Employee empCreatedBy on vrl.CreatedByUid = empCreatedBy.UserId
                                                                where vrl.VehicleId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, vehicleId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    SqlDataReader reader;
                    long result = SelectRecords(cmd, out reader);
                    VehicleRepairLogList list = new VehicleRepairLogList();

                    using (reader)
                    {
                        while (reader.Read())
                        {
                            VehicleRepairLog VehicleRepairLogObject = new VehicleRepairLog();
                            FillObject(VehicleRepairLogObject, reader);
                            VehicleRepairLogObject.Vin = reader["VIN"].ToString();
                            VehicleRepairLogObject.DriverName = reader["DriverName"].ToString();
                            VehicleRepairLogObject.CreatedBy = reader["CreatedBy"].ToString(); 
                            list.Add(VehicleRepairLogObject);
                        }
                        reader.Close();
                    }

                    return list;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool DeleteAllVehicleRepairLogByVehicleId(Guid vehicleId)
        {
            string sqlQuery = @"delete from VehicleRepairLog where VehicleId ='{0}'";
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
        
    }
}
