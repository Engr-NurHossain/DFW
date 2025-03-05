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
	public partial class RestaurantSiteConfigurationDataAccess
	{
        public RestaurantSiteConfigurationDataAccess(string ConStr) : base(ConStr) { }

        public DataSet GetRestaurantSiteConfigurationList(int pageNo, int pageSize, string searchText, string order)
        {
            string searchQuery = "";
            string subquery = "";
            string subquery1 = "";

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                searchQuery = string.Format(" where rsc.SiteName like '%{0}%' or rsc.DomainName like '%{0}%' or rsc.StorePhone like '%{0}%' or rsc.ThemeURL like '%{0}%' or rsc.SendOrdersEmail like '%{0}%'", searchText); ;
            }

            #region Order
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/sitename")
                {
                    subquery = "order by #RestaurantSiteConfigurationData.[SiteName] asc";
                    subquery1 = "order by [SiteName] asc";
                }
                else if (order == "descending/sitename")
                {
                    subquery = "order by #RestaurantSiteConfigurationData.[SiteName] desc";
                    subquery1 = "order by [SiteName] desc";
                }
                else if (order == "descending/domainname")
                {
                    subquery = "order by #RestaurantSiteConfigurationData.[DomainName] desc";
                    subquery1 = "order by [DomainName] desc";
                }
                else if (order == "ascending/domainname")
                {
                    subquery = "order by #RestaurantSiteConfigurationData.[DomainName] asc";
                    subquery1 = "order by [DomainName] asc";
                }
                else if (order == "ascending/storephone")
                {
                    subquery = "order by #RestaurantSiteConfigurationData.[StorePhone] asc";
                    subquery1 = "order by [StorePhone] asc";
                }
                else if (order == "descending/storephone")
                {
                    subquery = "order by #RestaurantSiteConfigurationData.[StorePhone] desc";
                    subquery1 = "order by [StorePhone] desc";
                }

                else if (order == "ascending/sendordersemail")
                {
                    subquery = "order by #RestaurantSiteConfigurationData.[SendOrdersEmail] asc";
                    subquery1 = "order by [SendOrdersEmail] asc";
                }
                else if (order == "descending/sendordersemail")
                {
                    subquery = "order by #RestaurantSiteConfigurationData.[SendOrdersEmail] desc";
                    subquery1 = "order by [SendOrdersEmail] desc";
                }
                else if (order == "ascending/theme")
                {
                    subquery = "order by #RestaurantSiteConfigurationData.[ThemeURL] asc";
                    subquery1 = "order by [ThemeURL] asc";
                }
                else if (order == "descending/theme")
                {
                    subquery = "order by #RestaurantSiteConfigurationData.[ThemeURL] desc";
                    subquery1 = "order by [ThemeURL] desc";
                }
                
                else
                {
                    subquery = "order by #RestaurantSiteConfigurationData.[Id] desc";
                    subquery1 = "order by Id desc";
                }

            }
            else
            {
                subquery = "order by #RestaurantSiteConfigurationData.[Id] desc";
                subquery1 = "order by Id desc";
            }
            #endregion
            string sqlQuery = @"declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)* @pagesize 
                                set @pageend = @pagesize

								select rsc.*
								into #RestaurantSiteConfigurationData from RestaurantSiteConfiguration rsc

								select * into #RestaurantSiteConfigurationFilterdata from #RestaurantSiteConfigurationData

								select top(@pagesize) * from #RestaurantSiteConfigurationFilterdata
								where Id not in (Select TOP (@pagestart)  Id from #RestaurantSiteConfigurationData {1})
								{2}

                                select Count(Id) As TotalCount from #RestaurantSiteConfigurationFilterdata 

								drop table #RestaurantSiteConfigurationData
								drop table #RestaurantSiteConfigurationFilterdata
                                    ";
            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        searchQuery,
                                        subquery,
                                        subquery1
                                        );
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    AddParameter(cmd, pInt32("pageno", pageNo));
                    AddParameter(cmd, pInt32("pagesize", pageSize));
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
