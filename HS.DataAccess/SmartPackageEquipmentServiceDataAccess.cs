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
	public partial class SmartPackageEquipmentServiceDataAccess
	{
        public SmartPackageEquipmentServiceDataAccess(string ConStr) : base(ConStr) { }
        public DataTable GetAllSmartPackageEquipmentServiceByPackageIdAndCompanyId(Guid PackageId, Guid companyId)
        {
            string sqlQuery = @"select 
                                spes.*,
								eqp.Name as EquipmentName,
                                eqp.EquipmentId,
                                eqp.Retail,
								sp.PackageName,
								sp.EquipmentMaxLimit
                                from SmartPackageEquipmentService spes
                                left join Equipment eqp 
                                on eqp.EquipmentId = spes.EquipmentId and eqp.CompanyId = spes.CompanyId
								LEFT JOIN SmartPackage sp
								on sp.PackageId=spes.PackageId and sp.CompanyId = spes.CompanyId
                                where spes.PackageId = '{0}' and spes.CompanyId = '{1}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, PackageId, companyId);
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
        public DataTable GetAllSmartPackageIncludeEquipmentByPackageIdAndCompanyId(Guid PackageId, Guid companyId)
        {
            string sqlQuery = @"select 
                                spes.*,
								eqp.Name as EquipmentName,
                                eqp.EquipmentId,
                                eqp.Retail,
								sp.PackageName,
								sp.EquipmentMaxLimit
                                from SmartPackageEquipmentService spes
                                left join Equipment eqp 
                                on eqp.EquipmentId = spes.EquipmentId and eqp.CompanyId = spes.CompanyId
								LEFT JOIN SmartPackage sp
								on sp.PackageId=spes.PackageId and sp.CompanyId = spes.CompanyId
                                where spes.PackageId = '{0}' and spes.CompanyId = '{1}' and spes.Type='Include'";
            try
            {
                sqlQuery = string.Format(sqlQuery, PackageId, companyId);
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
        public DataTable GetAllSmartPackageEquipmentServiceByTypeAndCompanyId(Guid companyId, string Type)
        {
            string sqlQuery = @"select 
	                                _pi.*,_eqp.SKU as SKU,
	                                _p.PackageName as PackageName,
	                                _eqp.Name as EquipmentName,
                                    emp.FirstName +' '+emp.LastName as UserName,
									man.Name as ManufacturerName
                                from SmartPackageEquipmentService _pi
	                                left join Equipment _eqp 
		                                on _pi.EquipmentId = _eqp.EquipmentId 
		                                and _pi.CompanyId = _eqp.CompanyId
	                                left join SmartPackage _p
		                                on _pi.PackageId = _p.PackageId 
		                                and _pi.CompanyId = _p.CompanyId
                                    Left join Employee emp 
                                        on emp.UserId = _pi.LastUpdatedBy 
									left join Manufacturer man
										on _eqp.ManufacturerId = man.Id
                                where _pi.CompanyId = '{0}' and _pi.Type='{1}'
                                and _p.PackageName is not null
                                order by _p.Id ";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId, Type);
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

        public DataSet GetAllSmartPackageEquipmentServiceListByTypeAndCompanyId(Guid companyId, string Type, int pageno, int pagesize, string status)
        {
            string subquery = "";
            if (!string.IsNullOrWhiteSpace(status) && status != "-1")
            {
                if(status == "Active")
                {
                    subquery = string.Format("and _pi.[Status] = 1");
                }
                else if(status == "Deactive")
                {
                    subquery = string.Format("and _pi.[Status] = 0");
                }
            }
            string sqlQuery = @"declare @pagestart int
                                declare @pageend int
                                set @pagestart=({2}-1)*{3} 
                                set @pageend = {3}

                                select 
	                                _pi.Id, _pi.CompanyId, _pi.PackageId, _pi.EquipmentId, _pi.IsFree, _pi.EptNo,
									_pi.[Type], _pi.Price, _pi.[Status], _pi.LastUpdatedBy, _pi.LastUpdatedDate,
									_pi.SmartPackageEquipmentServiceId,
	                                _p.PackageName as PackageName,
	                                _eqp.Name as EquipmentName,
                                    emp.FirstName +' '+emp.LastName as UserName,
									man.Name as ManufacturerName
									into #packEquipmentservice
                                from SmartPackageEquipmentService _pi
	                                left join Equipment _eqp 
		                                on _pi.EquipmentId = _eqp.EquipmentId 
		                                and _pi.CompanyId = _eqp.CompanyId
	                                left join SmartPackage _p
		                                on _pi.PackageId = _p.PackageId 
		                                and _pi.CompanyId = _p.CompanyId
                                    Left join Employee emp 
                                        on emp.UserId = _pi.LastUpdatedBy 
									left join Manufacturer man
										on _eqp.ManufacturerId = man.Id
                                where _pi.CompanyId = '{0}' and _pi.Type='{1}'
                                and _p.PackageName is not null
                                {5}
                                order by _p.Id

								select * into #FilterpackEquipmentservice
								from #packEquipmentservice

								SELECT TOP (@pageend) * FROM #FilterpackEquipmentservice
                                where  Id NOT IN(Select TOP (@pagestart)  Id from #packEquipmentservice)

                                select count(*) as TotalCount from #FilterpackEquipmentservice

								drop table #packEquipmentservice
								drop table #FilterpackEquipmentservice";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId, Type, pageno, pagesize, status, subquery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }	
}
