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
	public partial class InventoryTechDataAccess
	{
        public InventoryTechDataAccess(string ConnectionStr) : base(ConnectionStr) { }
        public DataTable InventoryTechAvailableCount(Guid TechnicianId, Guid EquipmentId)
        {
            string sqlQuery = @"select DISTINCT
                                ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=invw.EquipmentId and invinner.TechnicianId=invw.TechnicianId and Type='Add')-
								(Select ISNULL(SUM(invinner2.Quantity),0) from InventoryTech invinner2 where invinner2.EquipmentId=invw.EquipmentId and invinner2.TechnicianId=invw.TechnicianId and Type='Release')) as Quantity
                                from InventoryTech invw
                                where invw.EquipmentId='{0}' and invw.TechnicianId='{1}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, EquipmentId, TechnicianId);
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
        public DataTable AlreadyReleaseCountByCAEId(int CAEId)
        {

            //select
            //(select ISNULL(SUM(invinner.Quantity), 0) from InventoryTech invinner
            //where Type = 'Release' and invinner.CustomerAppointmentEquipmentId = cae.Id and invinner.EquipmentId = cae.EquipmentId) as Rel1,

            //(select ISNULL(SUM(invinner.Quantity), 0) from InventoryTech invinner
            //where Type = 'Add' and invinner.CustomerAppointmentEquipmentId = cae.Id and invinner.EquipmentId = cae.EquipmentId) as Add1,

            //((select ISNULL(SUM(invinner.Quantity), 0) from InventoryTech invinner
            //where Type = 'Release' and invinner.CustomerAppointmentEquipmentId = cae.Id and invinner.EquipmentId = cae.EquipmentId)-
            //(select ISNULL(SUM(invinner.Quantity), 0) from InventoryTech invinner
            //where Type = 'Add' and invinner.CustomerAppointmentEquipmentId = cae.Id and invinner.EquipmentId = cae.EquipmentId)) as Qty

            //from CustomerAppointmentEquipment cae
            //where cae.Id = @CAEId


        //    string sqlQuery = @"select DISTINCT
        //                        ((Select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner where invinner.EquipmentId=invw.EquipmentId and invinner.TechnicianId=invw.TechnicianId and Type='Release' and invinner.CustomerAppointmentEquipmentId={0})-
								//(Select ISNULL(SUM(invinner2.Quantity),0) from InventoryTech invinner2 where invinner2.EquipmentId=invw.EquipmentId and invinner2.TechnicianId=invw.TechnicianId and Type='Add' and invinner2.CustomerAppointmentEquipmentId={0})) as Quantity
        //                        from InventoryTech invw
        //                        where invw.IsDeleted=0 and invw.CustomerAppointmentEquipmentId={0}";
            string sqlQuery = @"select 
                                ((select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner 
                                where Type='Release' and invinner.CustomerAppointmentEquipmentId=cae.Id and invinner.EquipmentId=cae.EquipmentId)-
                                (select ISNULL(SUM(invinner.Quantity),0) from InventoryTech invinner 
                                where Type='Add' and invinner.CustomerAppointmentEquipmentId=cae.Id and invinner.EquipmentId=cae.EquipmentId)) as Quantity

                                from CustomerAppointmentEquipment cae
                                where cae.Id={0}";
            try
            {
                sqlQuery = string.Format(sqlQuery, CAEId);
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
        public bool DeleteInventoryTechByCustomerAppointmentEquipmentIdAndType(int value)
        {
            string sqlQuery = @"delete from InventoryTech where CustomerAppointmentEquipmentId = {0} and [Type] = 'Release' and Description != 'Add to technician from ticket(Undo)'";
            try
            {
                sqlQuery = string.Format(sqlQuery, value);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }	
}
