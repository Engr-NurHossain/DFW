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
	public partial class SmartPackageSystemInstallTypeMapDataAccess
	{
        public DataTable GetAllPackageByCompanySystemInstall(Guid companyId, int SystemId,int InstallTypeId,Guid? ManufacturerId)
        {

            string ManufacturerFilter = "";
            if(ManufacturerId.HasValue && ManufacturerId != Guid.Empty)
            {
                ManufacturerFilter = string.Format("and sp.ManufacturerId='{0}'", ManufacturerId);
            }

            string sqlQuery = @"select 
                                spsim.PackageId,
                                sp.PackageName,
                                sp.MinCredit,
                                sp.UserType,
                                sp.ActivationFee,
                                sp.PackageCode
                                from SmartPackageSystemInstallTypeMap spsim
                                LEFT JOIN SmartPackage sp on sp.PackageId=spsim.PackageId
                                where spsim.SmartSystemTypeId={0} 
                                and spsim.SmartInstallTypeId={1} 
                                {2} 
                                and sp.IsActive=1";
            try
            {
                sqlQuery = string.Format(sqlQuery, SystemId, InstallTypeId, ManufacturerFilter);
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
