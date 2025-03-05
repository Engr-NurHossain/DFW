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
	public partial class VehicleDetailDataAccess
	{
        public DataTable GetVehicleListBySearchKeyAndCompanyId(string key,int MaxLoad)
        {
            string sqlQuery = @" declare @SearchText nvarchar(100)
                                declare @MaxLoad int 

                                set @MaxLoad = {1}
                                set @SearchText ='%{0}%'
                      
                                select Top(@MaxLoad) vd.* 
                                ,emp.FirstName + ' '+emp.LastName as DriverName,
                                ISNULL(emp.UserId,'00000000-0000-0000-0000-000000000000') as DriverUserId
                                from VehicleDetail vd
                                left join EmployeeVehicle ev on ev.VehicleId = vd.VehicleId 
                                left join Employee emp on emp.UserId = ev.EmployeeId
                                where vehicleNo like @SearchText";
            try
            {
                sqlQuery = string.Format(sqlQuery, key, MaxLoad);
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

        public List<VehicleDetail> GetAllAvailableVehileDetailByUserId(Guid userId)
        {
            string sqlQuery = @" select * from VehicleDetail 
                                    where VehicleId not in (select VehicleId from EmployeeVehicle 
                                                                where EmployeeId != '{0}')";
            try
            {
                sqlQuery = string.Format(sqlQuery, userId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    return GetList(cmd, -1);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<VehicleDetail> GetAllVehileDetailByOrder(string OrderBy)
        {
            string orderByQuery = "";
            if(!string.IsNullOrWhiteSpace(OrderBy) && OrderBy.Split('-').Length == 2)
            {
                orderByQuery = string.Format("order by {1} {0}",OrderBy.Split('-')[0],OrderBy.Split('-')[1]);
            }
            string sqlQuery = @" select vd.*,emp.FirstName + ' '+emp.LastName as DriverName,
                                ISNULL(emp.UserId,'00000000-0000-0000-0000-000000000000') as DriverUserId
                                from VehicleDetail vd 
                                left join EmployeeVehicle ev on ev.VehicleId = vd.VehicleId 
                                left join Employee emp on emp.UserId = ev.EmployeeId
                                {0}
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, orderByQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    SqlDataReader reader;
                    long result = SelectRecords(cmd, out reader); 
                    VehicleDetailList list = new VehicleDetailList();

                    using (reader)
                    { 
                        while (reader.Read())
                        {
                            VehicleDetail vehicleDetailObject = new VehicleDetail();
                            FillObject(vehicleDetailObject, reader);
                            vehicleDetailObject.DriverName = reader["DriverName"].ToString();
                            vehicleDetailObject.DriverUserId = (Guid)reader["DriverUserId"]; 
                            list.Add(vehicleDetailObject);
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

        public VehicleDetail GetVehicleByVehicleId(Guid vehicleId)
        {
            string sqlQuery = @" select Top(1) vd.*
                                ,emp.FirstName + ' '+emp.LastName as DriverName,
                                ISNULL(emp.UserId,'00000000-0000-0000-0000-000000000000') as DriverUserId
                                from VehicleDetail vd
                                left join EmployeeVehicle ev on ev.VehicleId = vd.VehicleId 
                                left join Employee emp on emp.UserId = ev.EmployeeId
                                where vd.VehicleId  = '{0}'  ";
            try
            { 
               
                using (SqlCommand cmd = GetSQLCommand(string.Format(sqlQuery,vehicleId)))
                {
                    SqlDataReader reader;
                    long result = SelectRecords(cmd, out reader);
                    VehicleDetail vehicleDetailObject = new VehicleDetail();
                    using (reader)
                    {
                        if (reader.Read())
                        { 
                            FillObject(vehicleDetailObject, reader);
                            vehicleDetailObject.DriverName = reader["DriverName"].ToString();
                            vehicleDetailObject.DriverUserId = (Guid)reader["DriverUserId"]; 
                        }
                        reader.Close();
                    }

                    return vehicleDetailObject;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }	
}
