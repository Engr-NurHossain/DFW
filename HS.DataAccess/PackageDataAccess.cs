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
    public partial class PackageDataAccess
    {
        public PackageDataAccess(string Constr) : base(Constr) { }
        public DataTable GetAllPackageDeviceEquipmentListByPackageIdAndCompanyIdAndCustomerId(int PackageId, Guid companyId, Guid CustomerId)
        {
            string sqlQuery = @"select 
                                p.Id PackageId,
                                p.Name PackageName,
                                eqp.EquipmentId PackageEquipmentid,
                                eqp.Name EquipmentName,
                                eqp.Retail EquipmentCost,
                                'PackageDevice' PackageEquipmentType,
                                pd.IsFree EquipmentIsFreeFlag,
                                pd.Id PackageEqpId,
                                pd.EptNo as NumOfEquipment,
                                IsNULL(pdc.Id,0) IsSelected
                                from Package P
                                left join PackageDevice Pd 
                                on p.Id = Pd.PackageId and p.CompanyId = Pd.CompanyId 
                                left join Equipment eqp 
                                on pd.EquipmentId = eqp.EquipmentId and pd.CompanyId = eqp.CompanyId
                                left join PackageDetailCustomer pdc on  pdc.CustomerId='{2}' AND pdc.PackageEqpId = Pd.Id and pdc.Type = 'Device'
                                where P.Id = {0} and p.CompanyId = '{1}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, PackageId, companyId, CustomerId);
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

        public DataTable GetAllPackageIncludeEquipmentListByPackageIdAndCompanyId(int PackageId, Guid companyId)
        {
            string sqlQuery = @"select 
	                            p.Id PackageId,
	                            p.Name PackageName,
								eqp.EquipmentId PackageEquipmentid,
	                            eqp.Name EquipmentName,
	                            eqp.Retail EquipmentCost,
                                pd.EptNo as NumOfEquipment,
								'PackageDevice' PackageEquipmentType,
								pd.Id PackageEqpId,
                                pd.OrderBy 
                                from Package P
                                left join PackageInclude Pd 
                                on p.Id = Pd.PackageId and p.CompanyId = Pd.CompanyId 
                                left join Equipment eqp 
                                on pd.EquipmentId = eqp.EquipmentId and pd.CompanyId = eqp.CompanyId
                                where P.Id = {0} and p.CompanyId = '{1}'";
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

        public DataTable GetAllPackageOptionalEquipmentListByPackageIdAndCompanyIdAndCustomerId(int PackageId, Guid companyId, Guid CustomerId)
        {
            string sqlQuery = @"select 
	                                p.Id PackageId,
	                                p.Name PackageName,
									eqp.EquipmentId PackageEquipmentid,
	                                eqp.Name EquipmentName,
	                                eqp.Retail EquipmentCost,
									'PackageOptional' PackageEquipmentType,
                                    pd.IsFree EquipmentIsFreeFlag,
									pd.Id PackageEqpId,
									IsNULL(cae.Quantity,0) as NumofOptional,
                                    IsNULL(pdc.Id,0) IsSelected
                                from Package P
                                left join PackageOptional Pd 
                                on p.Id = Pd.PackageId and p.CompanyId = Pd.CompanyId 
                                left join Equipment eqp 
                                on pd.EquipmentId = eqp.EquipmentId and pd.CompanyId = eqp.CompanyId
                                left join PackageDetailCustomer pdc
								on  pdc.CustomerId='{2}' AND pdc.PackageEqpId = Pd.Id and pdc.Type = 'Optional'
								left join CustomerAppointment ca
								on ca.CustomerId = pdc.CustomerId
								left join CustomerAppointmentEquipment cae
								on cae.EquipmentId = Pd.EquipmentId
								and ca.AppointmentId = cae.AppointmentId
                                where P.Id = {0} and p.CompanyId = '{1}'	
                                Group By
								pdc.Id, 
								p.Id,
								p.Name,
								eqp.EquipmentId,
	                            eqp.Name,
	                            eqp.Retail,
                                pd.IsFree,
								pd.Id,
								cae.Quantity,
                                pdc.Id";
            try
            {
                sqlQuery = string.Format(sqlQuery, PackageId, companyId, CustomerId);
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

        public DataTable GetAllPackageIncludeProductsByCompanyId(Guid companyId)
        {
            string sqlQuery = @"select 
	                                _pi.*,
	                                _p.Name as IncludedPackageName,
	                                _eqp.Name as IncludedEquipmentName
	
                                from PackageInclude _pi
	                                left join Equipment _eqp 
		                                on _pi.EquipmentId = _eqp.EquipmentId 
		                                and _pi.CompanyId = _eqp.CompanyId
	                                left join Package _p
		                                on _pi.PackageId = _p.Id 
		                                and _pi.CompanyId = _p.CompanyId
                                where _pi.CompanyId = '{0}'
                                and _p.Name is not null
                                order by _p.Id";
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

        public DataTable GetAllPackageDeviceProductsByCompanyId(Guid companyId)
        {
            string sqlQuery = @"select 
	                                _pi.*,
	                                _p.Name as IncludedPackageName,
	                                _eqp.Name as IncludedEquipmentName
	
                                from PackageDevice _pi
	                                left join Equipment _eqp 
		                                on _pi.EquipmentId = _eqp.EquipmentId 
		                                and _pi.CompanyId = _eqp.CompanyId
	                                left join Package _p
		                                on _pi.PackageId = _p.Id 
		                                and _pi.CompanyId = _p.CompanyId
                                where _pi.CompanyId = '{0}'
                                and _p.Name is not null
                                order by _p.Id";
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

        public DataTable GetAllPackageOptionalProductsByCompanyId(Guid companyId)
        {
            string sqlQuery = @"select 
	                                _pi.*,
	                                _p.Name as IncludedPackageName,
	                                _eqp.Name as IncludedEquipmentName
	
                                from PackageOptional _pi
	                                left join Equipment _eqp 
		                                on _pi.EquipmentId = _eqp.EquipmentId 
		                                and _pi.CompanyId = _eqp.CompanyId
	                                left join Package _p
		                                on _pi.PackageId = _p.Id 
		                                and _pi.CompanyId = _p.CompanyId
                                where _pi.CompanyId = '{0}'
                                and _p.Name is not null
                                order by _p.Id ";
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

        public DataTable GetAllLeadPackageDetailByLeadIdandCompanyId(Guid companyId, int id)
        {
            string sqlQuery = @"select pac.Name as PackageName, cad.InstallType as PackageInstallType, pt.Name as PanelName
                                from Customer cs
                                join PackageCustomer pc
                                on pc.CustomerId = cs.CustomerId
                                join Package pac
                                on pac.PackageId = pc.PackageId
                                join CustomerCompany cc
                                on cc.CustomerId = cs.CustomerId
                                join CustomerAppointment ca
                                on ca.CustomerId = cs.CustomerId
                                join CustomerAppointmentDetail cad
                                on cad.AppointmentId = ca.AppointmentId
                                join PackageSystemCustomer psc
                                on psc.CustomerId = cs.CustomerId
                                join PanelType pt
                                on pt.Id = psc.PackageSystemId
								where cc.CompanyId = '{0}'
                                and cc.IsLead = 1
                                and cs.Id = {1}";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId, id);
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

        public DataTable GetAllSmartLeadPackageDetailByLeadIdandCompanyId(Guid companyId, int id)
        {
            string sqlQuery = @"Select
                                cs.Id,
                                pc.CompanyId,
                                sst.Name as SmartSystemTypeName,
                                sit.Name as SmartInstallTypeName,
                                sp.PackageName
                                from PackageCustomer pc
                                LEFT JOIN Customer cs on cs.CustomerId=pc.CustomerId
                                LEFT JOIN SmartSystemType sst on sst.Id=pc.SmartSystemTypeId
                                LEFT JOIN SmartInstallType sit on sit.Id=pc.SmartInstallTypeId
                                LEFT JOIN SmartPackage sp on sp.PackageId=pc.PackageId
                                where pc.CompanyId = '{0}'
                                and cs.Id = {1}";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId, id);
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
        public bool DeleteMMrRangeByPackageId(Guid companyId, Guid packageId)
        {
            string sqlQuery = @"Delete from MMRRange where CompanyId ='{0}' and PackageId = '{1}'";
            sqlQuery = string.Format(sqlQuery, companyId, packageId);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
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

        public DataTable GetAllPackageByCompanyId(Guid companyId)
        {
            string sqlQuery = @"select pack.*, mr.MaxMMR as MMRMax, mr.MinMMR as MMRMin
                                from Package pack
                                left join MMRRange mr
                                on mr.PackageId = pack.PackageId
                                where pack.CompanyId = '{0}'";
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
    }
}
