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
	public partial class ManufacturerDataAccess
	{
        public ManufacturerDataAccess(string ConStr) : base(ConStr) { }
        public bool ReseedManufacturerTable()
        {
            string SqlQuery = @"Delete from Manufacturer
                                DBCC CHECKIDENT('Manufacturer', RESEED, 0)
                                ";
            try
            {
                using (SqlCommand cmd = GetSQLCommand(SqlQuery))
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
        public DataTable GetAllManufacturerName()
        {
            string sqlQuery = @"select mn.ManufacturerId, mn.Name as Name from Manufacturer mn";
            try
            {
                sqlQuery = string.Format(sqlQuery);
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
        public DataTable GetAllManufacturerByCompanyIdBasePackage(Guid companyId)
        {
            string sqlQuery = @"select *From manufacturer mf
                                where mf.ManufacturerId in(select ManufacturerId from SmartPackage)
                                and mf.CompanyId='{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId);
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
        public DataSet GetAllManufacturer(int pageno, int pagesize)

        {

            int pagestart = (pageno - 1) * pagesize;
            int pageend = pagesize;
            string subquery = "";
       
          string sqlQuery = @"
                                

							select Id,Name into #Manutable from Manufacturer
                           
                                select * into #ManuIdData from #Manutable where Id> 0 								
                                select top({2}) * into #Testtable from #ManuIdData
								where Id not in (Select TOP ({3}) Id from #Manutable #cd )
							   

							    select *  from #Testtable
							
                                  

                                select Count(Id) As TotalCount from #ManuIdData
                                select Count(Id) as CountManu from #Testtable

								drop table #Manutable
								drop table #ManuIdData
                                drop table #Testtable
                                                           ";
            sqlQuery = string.Format(sqlQuery, subquery, pageno, pagesize, pagestart, pageend );

            try
            {

                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", pageno));
                    AddParameter(cmd, pInt32("pagesize", pagesize));
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetManufacturerListByEquipmentId(Guid EquipmentId)
        {
            string sqlQuery = @"select manu.Name, manu.ManufacturerId, Eqmanu.IsPrimary into #ManufacturerTempTable from Manufacturer manu
                                left join EquipmentManufacturer Eqmanu on Eqmanu.ManufacturerId = manu.ManufacturerId
                                where Eqmanu.EquipmentId ='{0}' and Eqmanu.IsPrimary = 1

                                select manu.Name, manu.ManufacturerId, Eqmanu.IsPrimary from Manufacturer manu
                                left join EquipmentManufacturer Eqmanu on Eqmanu.ManufacturerId = manu.ManufacturerId
                                where Eqmanu.EquipmentId ='{0}' and Eqmanu.IsPrimary = 0 and manu.ManufacturerId not in (select ManufacturerId from #ManufacturerTempTable)

                                union

                                select * from #ManufacturerTempTable

                                drop table #ManufacturerTempTable";
            try
            {
                sqlQuery = string.Format(sqlQuery, EquipmentId);
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

        public List<Manufacturer> GetManufacturersByEquipmentId(Guid equipmentId)
        {
            string sqlQuery = @"select * from Manufacturer where ManufacturerId in 
                    (select ManufacturerId from EquipmentManufacturer where EquipmentId = '{0}')";
            try
            {
                sqlQuery = string.Format(sqlQuery, equipmentId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    return GetList(cmd, -1);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }	
}
