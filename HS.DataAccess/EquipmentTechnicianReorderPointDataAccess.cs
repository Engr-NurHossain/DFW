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
	public partial class EquipmentTechnicianReorderPointDataAccess
	{
        public DataTable GetEqpTechRP()
        {
            string sqlQuery = @"select 
                                etrp.EquipmentId,
                                etrp.TechnicianId,
                                ISNULL(etrp.ReorderPoint,0) ReorderPoint,
                                ISNULL(it.Quantity,0) as Have
                                from EquipmentTechnicianReorderPoint etrp
                                LEFT JOIN InventoryTech it on it.TechnicianId=etrp.TechnicianId AND it.EquipmentId=etrp.EquipmentId";

            try
            {
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
        public bool DeleteEquipmentTechnicianReorderPoint(Guid TechnicianId)
        {

            string sqlQuery = @"Delete from EquipmentTechnicianReorderPoint where TechnicianId = '{0}'";

            try
            {
                sqlQuery = string.Format(sqlQuery, TechnicianId);
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
