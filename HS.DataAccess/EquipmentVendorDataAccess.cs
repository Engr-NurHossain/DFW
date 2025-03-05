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
    public partial class EquipmentVendorDataAccess
    {
        public bool UpdateEquipmentVendorSetIsPrimaryFalse(Guid equipmentId)
        {
            string sqlQuery = @"update EquipmentVendor set IsPrimary = 0
                                where EquipmentId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, equipmentId);

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

        public bool DeleteEquipmentVendorByEquipmentId(Guid equipmentId)
        {
            string sqlQuery = @"Delete From EquipmentVendor  
                                where EquipmentId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, equipmentId);

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
