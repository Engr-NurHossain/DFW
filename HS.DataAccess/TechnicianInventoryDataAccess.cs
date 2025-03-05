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
    public partial class TechnicianInventoryDataAccess
    {
        public DataTable GetAllTechnicianInventoryByEmployeeIdAndCompanyId(Guid EmployeeId, Guid CompanyId)
        {
            string sqlQuery = @"select 
	                                _ti.*,
	                                _eqp.Name as EquipmentName,
	                                _ec.Name as EquipmentType
                                from InventoryTech _ti
	                                left join Equipment _eqp
		                                on _ti.EquipmentId = _eqp.EquipmentId
	                                left join EquipmentClass _ec
		                                on _ec.Id = _eqp.EquipmentClassId

                                where _ti.CompanyId = '{0}'
	                                and _ti.TechnicianId = '{1}'

                                order by _ti.Id desc";
            try
            {
                sqlQuery = string.Format(sqlQuery,CompanyId, EmployeeId);
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
