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
    public partial class ServiceEquipmentDataAccess
    {
        public bool DeleteServiceEquipmentsByServiceId(Guid equipmentId)
        {
            string SqlQuery = @"delete from ServiceEquipment where ServiceId = '{0}'";

            try
            {
                SqlQuery = string.Format(SqlQuery,equipmentId);

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

        public List<ServiceEquipment> GetEquipmentServiceListByServiceId(Guid equipmentId)
        {
            string SqlQuery = @"select 
                                se.Id,
                                se.CompanyId,
                                se.ServiceId,
                                se.EquipmentId,
                                se.Quantity,
                                ISNULL(se.RetailPrice,0) RetailPrice,
                                se.CreatedBy,
                                se.CreatedDate,
                                eq.Name as EquipmentName 
                                from ServiceEquipment se 
                                left join Equipment eq on se.EquipmentId = eq.EquipmentId
                                where se.ServiceId = '{0}'
                                ";

            try
            {
                SqlQuery = string.Format(SqlQuery, equipmentId);

                using (SqlCommand cmd = GetSQLCommand(SqlQuery))
                { 
                    SqlDataReader reader;
                    long result = SelectRecords(cmd, out reader);

                    List<ServiceEquipment> list = new List<ServiceEquipment>();

                    using (reader)
                    { 
                        while (reader.Read())
                        {
                            ServiceEquipment serviceEquipmentObject = new ServiceEquipment();
                            FillObject(serviceEquipmentObject, reader);
                            serviceEquipmentObject.EquipmentName = reader["EquipmentName"].ToString();

                            list.Add(serviceEquipmentObject);
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
    }
}
