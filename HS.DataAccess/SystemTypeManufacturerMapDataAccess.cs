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
	public partial class SystemTypeManufacturerMapDataAccess
	{
        public DataTable GetAllSystemTypeManufacturerMap()
        {
            string sqlQuery = @"select sit.*, st.Name as SystemType, mf.Name as ManufacturerName 
                                from SystemTypeManufacturerMap as sit
                                left join SmartSystemType st on st.Id=sit.SystemId
                                left join Manufacturer mf on mf.ManufacturerId=sit.ManufacturerId
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

        public DataTable GetAllManufacturerBySystemId(int SystemId)
        {
            string sqlQuery = @"select 
                                 man.Name
                                ,ssit.SystemId 
                                ,man.ManufacturerId
                                from SystemTypeManufacturerMap ssit
                                lEFT JOIN Manufacturer man
                                on ssit.ManufacturerId=man.ManufacturerId
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
