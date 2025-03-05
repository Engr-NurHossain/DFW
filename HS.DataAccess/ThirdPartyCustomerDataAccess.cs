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
	public partial class ThirdPartyCustomerDataAccess
	{
        public ThirdPartyCustomerDataAccess() { }
        public ThirdPartyCustomerDataAccess(string ConnectionStr) : base(ConnectionStr) { }

        public List<ThirdPartyCustomer> GetThirdPartyCustomerByIsSold(bool IsSold)
        {
            string sqlQuery = @"
  select tc.*,cus.LastName,ROW_NUMBER() OVER (ORDER BY tc.Id) AS OfContract from  ThirdPartyCustomer tc
                                    left join customer cus on cus.CustomerId = tc.CustomerId
                                 left join customerextended ex on ex.CustomerId = tc.CustomerId
                                 where ex.CSAgreement = 'PUR' and tc.IsSold ='{0}'
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, IsSold);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    //return dsResult.Tables[0];
                    return GetList(cmd, -1);
                  
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataSet GetThirdPartyCustomersByIsSold(bool IsSold, DateTime? Start, DateTime? End,string order)
        {
            string DateQuery = "";
            if ((Start.HasValue && End.HasValue) && (Start != new DateTime() && End != new DateTime()))
            {
                DateQuery = string.Format("and tc.AccountOnlineDate between '{0}' and '{1}'", Start.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), End.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            string orderquery = "";
            string orderquery1 = "";
            #region Order
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/econtact")
                {
                    orderquery = "order by #cd.[eContact] asc";
                    orderquery1 = "order by [eContact] asc";
                }
                else if (order == "descending/econtact")
                {
                    orderquery = "order by #cd.[eContact] desc";
                    orderquery1 = "order by [eContact] desc";
                }
                else if (order == "ascending/cs")
                {
                    orderquery = "order by #cd.CustomerNumber asc";
                    orderquery1 = "order by CustomerNumber asc";
                }
                else if (order == "descending/cs")
                {
                    orderquery = "order by #cd.CustomerNumber desc";
                    orderquery1 = "order by CustomerNumber desc";
                }
                else if (order == "ascending/lastname")
                {
                    orderquery = "order by #cd.[LastName] asc";
                    orderquery1 = "order by [lastname] asc";
                }
                else if (order == "descending/lastname")
                {
                    orderquery = "order by #cd.[LastName] desc";
                    orderquery1 = "order by [LastName] desc";
                }
                else if (order == "ascending/transectionid")
                {
                    orderquery = "order by #cd.[TransId] asc";
                    orderquery1 = "order by [TransId] asc";
                }
                else if (order == "descending/transectionid")
                {
                    orderquery = "order by #cd.[TransId] desc";
                    orderquery1 = "order by [TransId] desc";
                }
                else if (order == "ascending/datesubmitted")
                {
                    orderquery = "order by #cd.[InstallDate] asc";
                    orderquery1 = "order by [InstallDate] asc";
                }
                else if (order == "descending/datesubmitted")
                {
                    orderquery = "order by #cd.[InstallDate] desc";
                    orderquery1 = "order by [InstallDate] desc";
                }
                else if (order == "ascending/ofcontract")
                {
                    orderquery = "order by #cd.[OfContract] asc";
                    orderquery1 = "order by [OfContract] asc";
                }
                else if (order == "descending/ofcontract")
                {
                    orderquery = "order by #cd.[OfContract] desc";
                    orderquery1 = "order by [OfContract] desc";
                }
                else if (order == "ascending/dealernumber")
                {
                    orderquery = "order by #cd.[DealerNumber] asc";
                    orderquery1 = "order by [DealerNumber] asc";
                }
                else if (order == "descending/dealernumber")
                {
                    orderquery = "order by #cd.[DealerNumber] desc";
                    orderquery1 = "order by [DealerNumber] desc";
                }
                else
                {
                    orderquery = "order by #cd.[OfContract]  desc";
                    orderquery1 = "order by OfContract desc";
                }

            }
            else
            {
                orderquery = "order by #cd.[OfContract] ";
                orderquery1 = "order by OfContract ";
            }
            #endregion
            string sqlQuery = @"
  select tc.*,cus.LastName,ROW_NUMBER() OVER (ORDER BY tc.Id) AS OfContract,ex.CreditTransectionId as TransId from  ThirdPartyCustomer tc
                                
                                    left join customer cus on cus.CustomerId = tc.CustomerId
                                 left join customerextended ex on ex.CustomerId = tc.CustomerId
                                 where ex.CSAgreement = 'PUR' and tc.IsSold ='{0}' and tc.Platform = 'Brinks'
                                   {1}
                                   {2}
                                ";
     

                              
            try
            {
                sqlQuery = string.Format(sqlQuery, IsSold,DateQuery,orderquery1);
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
        public DataSet GetThirdPartyUccCustomersOfNoAgency(DateTime? Start, DateTime? End)
        {
            string DateQuery = "";
            if ((Start.HasValue && End.HasValue) && (Start != new DateTime() && End != new DateTime()))
            {
                DateQuery = string.Format("and AccountOnlineDate between '{0}' and '{1}'", Start.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), End.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }

   
            string sqlQuery = @"SELECT  tcus.*,cus.Id as CustomerInt FROM ThirdPartyCustomer tcus
                                left join customer cus on cus.CustomerId = tcus.CustomerId
                                WHERE tcus.CustomerId NOT IN ( SELECT CustomerId FROM CustomerThirdPartyAgency) 
                                and tcus.Platform = 'UCC' {0}
                                ";



            try
            {
                sqlQuery = string.Format(sqlQuery, DateQuery);
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

        public bool UpdateBrinksToSold(DateTime? Start, DateTime? End)
        {
            string DateQuery = "";
            if ((Start.HasValue && End.HasValue) && (Start != new DateTime() && End != new DateTime()))
            {
                DateQuery = string.Format("and AccountOnlineDate between '{0}' and '{1}'", Start.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), End.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            string sqlQuery = @"Update ThirdPartyCustomer set IsSold=1 where IsSold=0 {0}";
            sqlQuery = string.Format(sqlQuery, DateQuery);
            try
            {
                sqlQuery = string.Format(sqlQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    //DataSet dsResult = GetDataSet(cmd);
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public DataTable GetBrinksReportExport(DateTime? Start, DateTime? End)
        {
            //GlobalSettingDataAccess gsd = new GlobalSettingDataAccess();
            //GlobalSetting gs = gsd.GetByQuery("SearchKey = 'CustomerNamingCondition'").FirstOrDefault();
            //string NameSql = "";
            //if (gs != null)
            //{
            //    NameSql = gs.Value;
            //}
            string sqlQuery = @"";
            //string subquery = "";
            //string statusquery = "";
            //if (!string.IsNullOrWhiteSpace(searchtext))
            //{
            //    subquery = string.Format("and cus.BusinessName like '%{0}%' or cus.DBA like '%{0}%' or cus.FirstName + ' ' + cus.LastName like '%{0}%' or cus.FirstName like '%{0}%' or cus.LastName like '%{0}%'", searchtext);
            //}
            //if (startdate.HasValue && enddate.HasValue && startdate.Value != new DateTime() && enddate.Value != new DateTime())
            //{

            //    sqlQuery = string.Format(sqlQuery, companyid, startdate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), enddate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), subquery, statusquery, NameSql);
            //}
            //else
            //{
            string DateQuery = "";
            if ((Start.HasValue && End.HasValue) && (Start != new DateTime() && End != new DateTime()))
            {
                DateQuery = string.Format("and tc.AccountOnlineDate between '{0}' and '{1}'", Start.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"), End.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            sqlQuery = @"Select tc.eContact as eContractID, tc.CustomerNumber as CS#
                             ,cus.LastName as [Last Name]
                             ,tc.TransectionID as [Transection ID]
                             ,Convert(date,(DATEADD(mi, DATEDIFF(mi, GETUTCDATE(), GETDATE()), tc.AccountOnlineDate))) as [Date Submitted]
                             ,ROW_NUMBER() OVER (ORDER BY tc.Id) AS [#Of Contract]
                             ,tc.DealerNumber as [Dealer Number]
                      
                              from ThirdPartyCustomer tc
                          left join customer cus on cus.CustomerId = tc.CustomerId
                             left join customerextended ex on ex.CustomerId = tc.CustomerId
                                 where ex.CSAgreement = 'PUR' and tc.IsSold ='false' and tc.Platform = 'Brinks'
                                {0}
                               ";
        //        sqlQuery = string.Format(sqlQuery);
            //}

            try
            {
                sqlQuery = string.Format(sqlQuery,DateQuery);
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
