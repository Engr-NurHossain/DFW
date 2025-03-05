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
	public partial class SmartPackageEquipmentServiceEquipmentDataAccess
	{
        public DataTable GetSmartPackageEquipmentServiceEquipmentBySmartPackageEquipmentServiceId(Guid smartPackageEquipmentServiceId)
        {
            string sqlQuery = @"select  SPESE.*, eqp.[Name] as EquipmentName from SmartPackageEquipmentServiceEquipment SPESE
                                left join Equipment eqp on eqp.EquipmentId = SPESE.EquipmentId
                                where SPESE.SmartPackageEquipmentServiceId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, smartPackageEquipmentServiceId); 
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

        public bool DeleteSmartPackageEquipmentServiceBySmartPackageEquipmentServiceId(Guid smartPackageEquipmentServiceId)
        {
            string sqlQuery = @"Delete from SmartPackageEquipmentServiceEquipment
                                where SmartPackageEquipmentServiceId = '{0}' ";
            try
            {
                sqlQuery = string.Format(sqlQuery, smartPackageEquipmentServiceId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    ExecuteCommand(cmd);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }	
}
