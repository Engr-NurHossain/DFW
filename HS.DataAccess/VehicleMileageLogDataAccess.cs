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
	public partial class VehicleMileageLogDataAccess
	{
        public List<VehicleMileageLog> GetAllMilageLogByVehicleId(Guid vehicleId)
        {
            string sqlQuery = @"select ml.*,vd.VIN from VehicleMileageLog ml
                            left join VehicleDetail vd on ml.VehicleId = vd.VehicleId
                            where ml.VehicleId = '{0}'";

            try
            {
                sqlQuery = string.Format(sqlQuery, vehicleId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    SqlDataReader reader;
                    long result = SelectRecords(cmd, out reader);
                    VehicleMileageLogList list = new VehicleMileageLogList();

                    using (reader)
                    {
                        while (reader.Read())
                        {
                            VehicleMileageLog VehicleMileageLogObject = new VehicleMileageLog();
                            FillObject(VehicleMileageLogObject, reader);
                            VehicleMileageLogObject.Vin = reader["VIN"].ToString(); 
                            list.Add(VehicleMileageLogObject);
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
        public bool DeleteAllVehicleMilageLogByvehicleId(Guid vehicleId)
        {
            string sqlQuery = @"delete from VehicleMileageLog where VehicleId ='{0}'";
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
