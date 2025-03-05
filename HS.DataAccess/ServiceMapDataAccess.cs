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
    public partial class ServiceMapDataAccess
    {
        public List<ServiceMap> GetServiceModelListByServiceId(Guid equipmentId)
        {
            string sqlQuery = @"select SM.*,  
                                ISNULL(manu.[Name],'') as Manufacturer,
                                ISNULL(loc.[Name],'') as [Location],
                                ISNULL(typ.[Name],'') as [Type],
                                ISNULL(model.[Name],'') as Model,
                                ISNULL(finish.[Name],'') as Finish,
                                ISNULL(capacity.[Name],'') as Capacity,
                                ISNULL(eqp.[Name],'') as EquipmentName

                                from ServiceMap SM

                                left join Manufacturer manu on SM.ManufacturerId = manu.ManufacturerId

                                left join Equipment eqp on eqp.EquipmentId = SM.EquipmentId 
                                    and SM.EquipmentId != '00000000-0000-0000-0000-000000000000'

                                left join ServiceDetailInfo loc on loc.ServiceInfoId = SM.LocationId 
                                    and loc.[Type] = 'Location' and SM.ServiceId = loc.ServiceId

                                left join ServiceDetailInfo typ on typ.ServiceInfoId = SM.TypeId 
                                    and typ.[Type] = 'Type' and SM.ServiceId = typ.ServiceId

                                left join ServiceDetailInfo model on model.ServiceInfoId = SM.ModelId 
                                    and model.[Type] = 'Model' and SM.ServiceId = model.ServiceId

                                left join ServiceDetailInfo finish on finish.ServiceInfoId = SM.FinishId 
                                    and finish.[Type] = 'Finish' and SM.ServiceId = finish.ServiceId

                                left join ServiceDetailInfo capacity on capacity.ServiceInfoId = SM.CapacityId 
                                    and capacity.[Type] = 'Capacity' and SM.ServiceId = capacity.ServiceId


                                where SM.ServiceId = '{0}'";

            try
            {
                sqlQuery = string.Format(sqlQuery, equipmentId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    SqlDataReader reader;
                    long result = SelectRecords(cmd, out reader);
                    ServiceMapList list = new ServiceMapList();

                    using (reader)
                    {
                        while (reader.Read())
                        {
                            ServiceMap serviceMapObject = new ServiceMap();
                            FillObject(serviceMapObject, reader);
                            serviceMapObject.Manufacturer = reader["Manufacturer"].ToString();
                            serviceMapObject.Location = reader["Location"].ToString();
                            serviceMapObject.Type = reader["Type"].ToString();
                            serviceMapObject.Model = reader["Model"].ToString();
                            serviceMapObject.Finish = reader["Finish"].ToString();
                            serviceMapObject.Capacity = reader["Capacity"].ToString();
                            serviceMapObject.EquipmentName = reader["EquipmentName"].ToString();
                            list.Add(serviceMapObject);
                        }
                        reader.Close();
                    }

                    return list;
                }

            }
            catch (Exception)
            {

                return null;
            }
        }

        public bool DeleteServiceMapByServiceId(Guid serviceId)
        {

            string sqlQuery = @"Delete from ServiceMap where ServiceId = '{0}'";

            try
            {
                sqlQuery = string.Format(sqlQuery, serviceId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    ExecuteCommand(cmd);
                }
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }

        public List<ServiceMap> GetServiceMapListByServiceMap(ServiceMap serviceMap)
        {
            string sqlQuery = @"select SM.*,  
                                ISNULL(manu.[Name],'') as Manufacturer,
                                ISNULL(loc.[Name],'') as [Location],
                                ISNULL(typ.[Name],'') as [Type],
                                ISNULL(model.[Name],'') as Model,
                                ISNULL(finish.[Name],'') as Finish,
                                ISNULL(capacity.[Name],'') as Capacity

                                from ServiceMap SM

                                left join Manufacturer manu on SM.ManufacturerId = manu.ManufacturerId

                                left join ServiceDetailInfo loc on loc.ServiceInfoId = SM.LocationId 
                                    and loc.[Type] = 'Location' 
                                    and SM.ServiceId = loc.ServiceId

                                left join ServiceDetailInfo typ on typ.ServiceInfoId = SM.TypeId 
                                    and typ.[Type] = 'Type' 
                                    and SM.ServiceId = typ.ServiceId

                                left join ServiceDetailInfo model on model.ServiceInfoId = SM.ModelId 
                                    and model.[Type] = 'Model' 
                                    and SM.ServiceId = model.ServiceId

                                left join ServiceDetailInfo finish on finish.ServiceInfoId = SM.FinishId 
                                    and finish.[Type] = 'Finish' 
                                    and SM.ServiceId = finish.ServiceId

                                left join ServiceDetailInfo capacity on capacity.ServiceInfoId = SM.CapacityId 
                                    and capacity.[Type] = 'Capacity' 
                                    and SM.ServiceId = capacity.ServiceId


                                where SM.ServiceId = '{0}' 
                                {1}";
            string TypeSelectSql = "";
            if (serviceMap.ManufacturerId != Guid.Empty)
            {
                TypeSelectSql += string.Format(" AND SM.ManufacturerId='{0}' ", serviceMap.ManufacturerId); 
            }
            if (serviceMap.LocationId != Guid.Empty)
            {
                TypeSelectSql += string.Format(" AND SM.LocationId='{0}' ", serviceMap.LocationId);
            }
            if (serviceMap.TypeId != Guid.Empty)
            {
                TypeSelectSql += string.Format(" AND SM.TypeId='{0}' ", serviceMap.TypeId);
            }
            if (serviceMap.ModelId != Guid.Empty)
            {
                TypeSelectSql += string.Format(" AND SM.ModelId='{0}' ", serviceMap.ModelId);
            }
            if (serviceMap.FinishId != Guid.Empty)
            {
                TypeSelectSql += string.Format(" AND SM.FinishId='{0}' ", serviceMap.FinishId);
            }
            if (serviceMap.CapacityId != Guid.Empty)
            {
                TypeSelectSql += string.Format(" AND SM.CapacityId='{0}' ", serviceMap.CapacityId);
            }

            try
            {
                sqlQuery = string.Format(sqlQuery, serviceMap.ServiceId,TypeSelectSql);
                
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    SqlDataReader reader;
                    long result = SelectRecords(cmd, out reader);
                    ServiceMapList list = new ServiceMapList();

                    using (reader)
                    {
                        while (reader.Read())
                        {
                            ServiceMap serviceMapObject = new ServiceMap();
                            FillObject(serviceMapObject, reader);
                            serviceMapObject.Manufacturer = reader["Manufacturer"].ToString();
                            serviceMapObject.Location = reader["Location"].ToString();
                            serviceMapObject.Type = reader["Type"].ToString();
                            serviceMapObject.Model = reader["Model"].ToString();
                            serviceMapObject.Finish = reader["Finish"].ToString();
                            serviceMapObject.Capacity = reader["Capacity"].ToString();
                            list.Add(serviceMapObject);
                        }
                        reader.Close();
                    }

                    return list;
                }

            }
            catch (Exception)
            {

                return null;
            }
        }

        public bool DeleteServiceMapByServiceInfoId(Guid serviceInfoId)
        {
            if(serviceInfoId == Guid.Empty)
            {
                return false;
            }

            string sqlQuery = @"Declare @ServiceInfoId  uniqueidentifier
                                set @ServiceInfoId = '{0}'


                                delete from ServiceMap where LocationId = @ServiceInfoId
	                                Or TypeId = @ServiceInfoId
	                                Or ModelId = @ServiceInfoId
	                                Or FinishId = @ServiceInfoId
	                                Or CapacityId = @ServiceInfoId";
            try
            {
                sqlQuery = string.Format(sqlQuery, serviceInfoId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    ExecuteCommand(cmd);
                }
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }
    }
}
