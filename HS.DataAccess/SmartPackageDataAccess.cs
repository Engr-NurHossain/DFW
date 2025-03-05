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
	public partial class SmartPackageDataAccess
	{
        public SmartPackageDataAccess(string ConStr) : base(ConStr) { }
        public DataTable GetAllSmartPackageByCompanyId(Guid companyId)
        {
            string sqlQuery = @"select sp.*, st.Name as SystemType, it.Name as InstallType, emp.FirstName + ' ' + emp.LastName as UserName
                                from SmartPackage sp
								left Join SmartPackageSystemInstallTypeMap itm on sp.PackageId = itm.PackageId
                                left join SmartSystemType st on itm.SmartSystemTypeId = st.Id
                                left join SmartInstallType it on itm.SmartInstallTypeId = it.Id
                                left Join Employee emp on emp.UserId=sp.LastUpdatedBy
                                where sp.CompanyId='{0}' and  (sp.isDelete is null or sp.IsDelete = 0) order by sp.LastUpdatedDate desc";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId);
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

        public DataSet GetAllSmartPackageByFilter(SmartPackageFilter filter)
        {
            string SearchTextQuery = "";
            string FilterTextQuery = "";
            string subquery = "";
            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                SearchTextQuery += string.Format("and sp.UserType like '%{0}%' or sp.PackageName like '%{0}%' or sp.ActivationFee like '%{0}%' or sp.LastUpdatedDate like '%{0}%' or sp.PackageCode like '%{0}%' or st.Name like '%{0}%' or it.Name like '%{0}%'", filter.SearchText);
            }
            if(!string.IsNullOrEmpty(filter.FilterText) && filter.FilterText != "-1")
            {
                if(filter.FilterText == "True")
                {
                    FilterTextQuery += "and sp.IsActive = 1";
                }
                else
                {
                    FilterTextQuery += "and sp.IsActive = 0";
                }
                
            }
            string sqlQuery = @"declare @pagesize int
                                declare @pageno int 
                                set @pagesize = " + filter.UnitPerPage + @"
                                set @pageno = " + filter.PageNumber + @"
                                declare @pagestart int
                                set @pagestart=(@pageno-1)* @pagesize  
                                select * into #tempPkg from(select sp.*, st.Name as SystemType, it.Name as InstallType, emp.FirstName + ' ' + emp.LastName as UserName
                                from SmartPackage sp
								left Join SmartPackageSystemInstallTypeMap itm on sp.PackageId = itm.PackageId
                                left join SmartSystemType st on itm.SmartSystemTypeId = st.Id
                                left join SmartInstallType it on itm.SmartInstallTypeId = it.Id
                                left Join Employee emp on emp.UserId=sp.LastUpdatedBy
                                where sp.CompanyId='{0}' and  (sp.isDelete is null or sp.IsDelete = 0) {1}{2}) a
                                select * into #ftemp from #tempPkg 

                                select TOP (@pagesize) *  from #ftemp f
                                where  f.Id NOT IN(Select TOP (@pagestart) Id from #ftemp  order by Id desc)
                                {3}
                                select Count(Id) As TotalCount from #ftemp  

								drop table #tempPkg
								drop table #ftemp";

            #region Order
            if (!string.IsNullOrWhiteSpace(filter.Order))
            {
                if (filter.Order == "ascending /id")
                {
                    subquery = "order by Id asc";

                }
                else if (filter.Order == "descending/id")
                {
                    subquery = "order by Id desc";

                }
                else if (filter.Order == "ascending/name")
                {
                    subquery = "order by PackageName asc";

                }
                else if (filter.Order == "descending/name")
                {
                    subquery = "order by PackageName desc";

                }
                else if (filter.Order == "ascending/systype")
                {
                    subquery = "order by SystemType asc";

                }
                else if (filter.Order == "descending/systype")
                {
                    subquery = "order by SystemType desc";

                }
                else if (filter.Order == "ascending/installtype")
                {
                    subquery = "order by InstallType  asc";

                }
                else if (filter.Order == "descending/installtype")
                {
                    subquery = "order by InstallType  desc";

                }
                else if (filter.Order == "ascending/actfee")
                {
                    subquery = "order by ActivationFee asc";

                }
                else if (filter.Order == "descending/actfee")
                {
                    subquery = "order by ActivationFee desc";

                }

                else if (filter.Order == "ascending/status")
                {
                    subquery = "order by IsActive asc";

                }
                else if (filter.Order == "descending/status")
                {
                    subquery = "order by IsActive desc";

                }

                else if (filter.Order == "ascending/lastupdateddate")
                {
                    subquery = "order by  LastUpdatedDate desc";

                }
                else if (filter.Order == "descending/lastupdateddate")
                {
                    subquery = "order by  LastUpdatedDate desc";

                }
            }
            else
            {
                subquery = "order by Id desc";

            }
            #endregion

            try
            {
                sqlQuery = string.Format(sqlQuery, filter.CompanyId, SearchTextQuery,FilterTextQuery, subquery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);

                    return dsResult;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllDuplicateSmartPackagesByPackageName(string PackageName)
        {
            string sqlQuery = @"select count(Id) as ExistCount 
                                from SmartPackage 
                                where PackageName like '%{0}%' and (isDelete is null or IsDelete = 0)";
            try
            {
                sqlQuery = string.Format(sqlQuery, PackageName);
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

        public DataTable GetSmartPackageBySystemTypeIdAndInstallIdAndManufacturerId(int typeid, int installtypeid, Guid manuid, string usertype)
        {
            string sqlQuery = @"select 
                                spsim.PackageId,
                                sp.PackageName,
                                sp.MinCredit,
                                sp.UserType,
                                sp.ActivationFee,
                                sp.PackageCode
                                from SmartPackageSystemInstallTypeMap spsim
                                LEFT JOIN SmartPackage sp on sp.PackageId=spsim.PackageId
                                where spsim.SmartSystemTypeId= {0}
                                and spsim.SmartInstallTypeId= {1}
                                and sp.manufacturerid = '{2}'
								and sp.usertype='{3}'
                                and sp.IsActive=1
								and sp.packagename like '%transfer%'";
            try
            {
                sqlQuery = string.Format(sqlQuery, typeid, installtypeid, manuid, usertype);
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
