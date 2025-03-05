using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Linq;

namespace HS.DataAccess
{
	public partial class CustomerSystemNoDataAccess
	{
        public CustomerSystemNoDataAccess(string ConStr) : base(ConStr) { }
        string INSERTCUSTOMERSYSTEMNOCHECK = "INSERTCUSTOMERSYSTEMNOCHECK";

        public bool ReseedCustomerSystemNoTable()
        {
            string SqlQuery = @"Delete from CustomerSystemNo
                                DBCC CHECKIDENT('CustomerSystemNo', RESEED, 0)
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
        public long InsertAndCheck(CustomerSystemNoBase customerSystemNoObject)
        {
            try
            {
                SqlCommand cmd = GetSPCommand(INSERTCUSTOMERSYSTEMNOCHECK);

                AddParameter(cmd, pInt32Out(CustomerSystemNoBase.Property_Id));
                AddCommonParams(cmd, customerSystemNoObject);

                long result = InsertRecord(cmd);
                if (result > 0)
                {
                    customerSystemNoObject.RowState = BaseBusinessEntity.RowStateEnum.NormalRow;
                    customerSystemNoObject.Id = (Int32)GetOutParameter(cmd, CustomerSystemNoBase.Property_Id);
                }
                return result;
            }
            catch (SqlException x)
            {
                throw new ObjectInsertException(customerSystemNoObject, x);
            }
        }
        public DataTable GetAllCustomerNUmberByCompanyId(Guid companyID,string KeyVal)
        {
            string sqlQuery = @"select TOP 5 cusno.CustomerNo
                                from CustomerSystemNo cusno
                                where cusno.CompanyId='{0}'
                                and cusno.IsUsed=0 and cusno.IsReserved=0
                                and cusno.CustomerNo like '%{1}%'";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyID, KeyVal);
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

        public DataTable GetAllOpenCustomerSystemNoByCompanyIdandPlatform(Guid CompanyId, string PlatformPrifix)
        {
            string sqlQuery = @"select * from CustomerSystemNo where CustomerNo like '{0}%' and IsUsed = 0 and IsReserved = 0 and CompanyId = '{1}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, PlatformPrifix, CompanyId );
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

        public DataSet GetCustomerSystemNoListByCompanyIdAndPaging(Guid companyid, int pageno, int pagesize, string Search,string filter, string prefix)
        {
            GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            string NameSql = "";
            if (gs != null)
            {
                NameSql = gs.Value;
            }
            var filterQuery = "";
            if (!string.IsNullOrEmpty(filter) && filter != "-1")
            {
                if(filter == "Open")
                {
                    filterQuery = "and IsUsed = 0";
                }
                else
                {
                    filterQuery = "and IsUsed = 1";
                }
             
            }
            var Prefix = "";
            if (!string.IsNullOrEmpty(prefix) && prefix != "-1")
            {
                Prefix = string.Format(" and csn.CustomerNo like '{0}%'", prefix);
            }
                string sqlQuery = @"declare @pagestart int
                                declare @pageend int
                                declare @pageno int
                                declare @pagesize int
                                declare @CompanyId uniqueidentifier
                                set @CompanyId = '{0}'
                                set @pageno= {1}
                                set @pagesize = {2}

                                set @pagestart =(@pageno-1)* @pagesize
                                set @pageend = @pagesize

                                SELECT 
                                    csn.*,{5} as DisplayName
      
                                        INTO #CustomerSystemNo
                                        FROM CustomerSystemNo csn
                                        left join customer cus on cus.Id = csn.CustomerId
		                                WHERE 
			                                csn.CompanyId = @CompanyId
                                            {4} {7}

                                        SELECT * INTO #CustomerFilterSystemNo
                                        FROM #CustomerSystemNo
                                            
	                                    SELECT TOP (@pagesize)
                                        *
                                        FROM #CustomerFilterSystemNo
                                        where   Id NOT IN(Select TOP (@pagestart)  Id from #CustomerSystemNo order by CustomerNo asc){6}
                                        order by CustomerNo asc
     
                                        select count(*) [TotalCount]
                                        from #CustomerFilterSystemNo where Id>0 {6}

                                        DROP TABLE #CustomerSystemNo
                                        DROP TABLE #CustomerFilterSystemNo";
            string subquery = "";
            if (!string.IsNullOrWhiteSpace(Search))
            {
                subquery = string.Format(" and csn.CustomerNo like '{0}%'", Search);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, companyid, pageno, pagesize, Search, subquery, NameSql, filterQuery, Prefix);
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

     
    }	
}
