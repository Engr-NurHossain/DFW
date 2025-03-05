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
	public partial class EquipmentManufacturerDataAccess
	{
        public bool UpdateEquipmentManufacturerSetIsPrimaryFalse(Guid equipmentId)
        {
            string sqlQuery = @"update EquipmentManufacturer set IsPrimary = 0
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
