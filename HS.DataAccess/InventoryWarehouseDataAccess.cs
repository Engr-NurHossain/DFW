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
	public partial class InventoryWarehouseDataAccess
	{
        public DataTable inventoryWareAvailableCount(Guid equipmentId, Guid companyId)
        {
            string sqlQuery = @"select DISTINCT
                                ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryWarehouse invinner where invinner.EquipmentId=invw.EquipmentId and Type='Add')- (Select ISNULL(SUM(invinner2.Quantity),0) from InventoryWarehouse invinner2 where invinner2.EquipmentId=invw.EquipmentId and Type='Release')) as Quantity
                                from InventoryWarehouse invw
                                where invw.EquipmentId='{0}' and invw.CompanyId='{1}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, equipmentId, companyId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }	
}
