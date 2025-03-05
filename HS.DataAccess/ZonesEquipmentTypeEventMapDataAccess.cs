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
	public partial class ZonesEquipmentTypeEventMapDataAccess
	{
        public DataTable GetEquipmentTypeEventMapByEventCode(string EventCode)
        {
          
            string sqlQuery = @"select zm.*,eqpType.EquipmentType as EquipTypeVal from ZonesEquipmentTypeEventMap zm
            left join ZonesEquipmentType eqpType on eqpType.EqpmentTypeId = zm.EquipmentTypeId where zm.EventId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, EventCode);
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
    }	
}
