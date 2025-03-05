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
	public partial class PackageCommissionDataAccess
	{
        public DataTable GetAllPackageComission()
        {
            string sqlQuery = @"select pc.*,lkLeadType.DisplayText as LeadTypeVal,lkPkgType.DisplayText as PackageTypeVal,lkSiteType.DisplayText as TypeVal,lkComType.DisplayText as CommissionTypeVal from PackageCommission pc
                                left join lookup lkLeadType on lkLeadType.Datavalue = pc.LeadType and lkLeadType.Datakey = 'CustomerType'
                                left join lookup lkPkgType on lkPkgType.Datavalue = pc.PackageType and lkPkgType.Datakey = 'PackageType'
                                left join lookup lkComType on lkComType.Datavalue = pc.CommissionType and lkComType.Datakey = 'CommissionType'
                                left join lookup lkSiteType on lkSiteType.Datavalue = pc.Type and lkSiteType.Datakey = 'Type'";
            try
            {
                sqlQuery = string.Format(sqlQuery);
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
