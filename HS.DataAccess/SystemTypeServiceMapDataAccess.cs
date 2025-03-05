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
	public partial class SystemTypeServiceMapDataAccess
	{
        public DataTable GetAllSystemTypeServiceMap()
        {
            string sqlQuery = @"select sit.*, st.Name as SystemType, sp.PackageName, eqp.Name as ServiceName, eqp.Retail
                                from SystemTypeServiceMap as sit
                                left join SmartSystemType st on st.Id=sit.SystemTypeId
                                Left join SmartPackage sp on sp.PackageId=sit.PackageId
                                left join Equipment eqp on eqp.EquipmentId=sit.EquipmentId
                                where st.Name is Not Null";
            try
            {
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
        public DataTable GetAllServiceForSmartSetUp()
        {
            string sqlQuery = @"select eqp.Id,eqp.EquipmentId,eqp.Name as ServiceName, eqp.Retail
                                from Equipment as eqp
                                where eqp.EquipmentClassId=2";
            try
            {
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
        public DataTable GetAllServiceBySystemId(int SystemId)
        {
            string sqlQuery = @"select 
                                 eqp.Name
                                ,ssit.SystemTypeId 
                                ,eqp.EquipmentId
                                from SystemTypeServiceMap ssit
                                lEFT JOIN Equipment eqp
                                on eqp.EquipmentId=ssit.EquipmentId
                                where ssit.Systemid={0}";
            try
            {
                sqlQuery = string.Format(sqlQuery, SystemId);
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
