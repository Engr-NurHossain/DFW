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
	public partial class RestCategoryDataAccess
	{
        public DataTable GetAllCategoryByMenuId(Guid MenuId, Guid itemid, Guid comid)
        {
            string sqlQuery = @"
                                select c.Id as CategoryId,c.CategoryName as CategoryName  from RestMenuItemCategory cd 
                                left join RestCategory c
	                                on cd.CategoryId = c.CategoryId
                                where cd.MenuId='{0}' and cd.ItemId = '{1}' and c.CompanyId = '{2}'
                                ";
            //string subquery = "";

            sqlQuery = string.Format(sqlQuery, MenuId, itemid, comid);
            try
            {
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
        public bool DeleteByMenuId(Guid menuId, Guid itemid, int menuintid, int itemintid)
        {

            string SqlQuery = @"delete from RestMenuItemCategory where MenuId ='{0}' and ItemId = '{1}'
                                delete from RestMenuItemCategoryTopping where MenuId ='{0}' and ItemId = '{1}'
                                delete from RestMenuItemCategoryURL where MenuId = {2} and MenuItemId = {3}";
            SqlQuery = string.Format(SqlQuery, menuId, itemid, menuintid, itemintid);
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
        public bool DeleteByCategoryId(int categoryId)
        {

            string SqlQuery = @"delete from CategoryDetail where CategoryId ='{0}'";
            SqlQuery = string.Format(SqlQuery, categoryId);
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
        public DataSet GetCategoryList(Guid comid, int pageNo, int pageSize, string searchText, string order)
        {
            string searchQuery = "";
            string subquery = "";
            string subquery1 = "";

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                searchQuery = string.Format("and CategoryName like '%{0}%' or DaysAvailable like '%{0}%' or TimeAvailable like '%{0}%' or Status like '%{0}%'", searchText); ;
            }

            #region Order
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/categoryid")
                {
                    subquery = "order by #CategoryData.[Id] asc";
                    subquery1 = "order by [Id] asc";
                }
                else if (order == "descending/categoryname")
                {
                    subquery = "order by #CategoryData.[CategoryName] desc";
                    subquery1 = "order by [CategoryName] desc";
                }
                else if (order == "ascending/categoryname")
                {
                    subquery = "order by #CategoryData.[CategoryName] asc";
                    subquery1 = "order by [CategoryName] asc";
                }
                else if (order == "ascending/categorydescription")
                {
                    subquery = "order by #CategoryData.[Description] asc";
                    subquery1 = "order by [Description] asc";
                }
                else if (order == "descending/Categorydescription")
                {
                    subquery = "order by #CategoryData.[Description] desc";
                    subquery1 = "order by [Description] desc";
                }
                else if (order == "ascending/categorydayavailable")
                {
                    subquery = "order by #CategoryData.[DaysAvailable] asc";
                    subquery1 = "order by [DaysAvailable] asc";
                }
                else if (order == "descending/categorydayavailable")
                {
                    subquery = "order by #CategoryData.[DaysAvailable] desc";
                    subquery1 = "order by [DaysAvailable] desc";
                }
                else if (order == "ascending/categorytimeavailable")
                {
                    subquery = "order by #CategoryData.[TimeAvailable] asc";
                    subquery1 = "order by [TimeAvailable] asc";
                }
                else if (order == "descending/categorytimeavailable")
                {
                    subquery = "order by #CategoryData.[TimeAvailable] desc";
                    subquery1 = "order by [TimeAvailable] desc";
                }
                else if (order == "ascending/categorytatus")
                {
                    subquery = "order by #CategoryData.[Status] asc";
                    subquery1 = "order by [Status] asc";
                }
                else if (order == "descending/categorystatus")
                {
                    subquery = "order by #CategoryData.[Status] desc";
                    subquery1 = "order by [Status] desc";
                }
                else
                {
                    subquery = "order by #CategoryData.OrderBy asc";
                    subquery1 = "order by OrderBy asc";
                }

            }
            else
            {
                subquery = "order by #CategoryData.OrderBy asc";
                subquery1 = "order by OrderBy asc";
            }
            #endregion
            string sqlQuery = @"declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)* @pagesize 
                                set @pageend = @pagesize

								select *
								into #CategoryData from RestCategory
                                where (Status=1 or status=0)
                                and CompanyId = '{3}'
								{0}

								select * into #CategoryFilterdata from #CategoryData

								select top(@pagesize) * from #CategoryFilterdata
								where Id not in (Select TOP (@pagestart)  Id from #CategoryData {1})
								{2}

                                select Count(Id) As TotalCount from #CategoryFilterdata 

								drop table #CategoryData
								drop table #CategoryFilterdata
                                    ";
            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        searchQuery,
                                        subquery,
                                        subquery1,
                                        comid
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

        public DataTable GetCategoryItemListPartial(Guid companyid, int categoryid)
        {
            string sqlQuery = @"select
                                mitem.Photo,
                                mitem.ItemName,
                                mitem.Price,
                                mitem.DaysAvailable,
                                mitem.[Status],
                                mitem.Id,
                                mitem.UrlSlug
	                                from RestCategory as cat
									left join RestMenuItemCategory rmic on rmic.CategoryId = cat.CategoryId
                                left join RestMenuItem mitem on mitem.ItemId = rmic.ItemId
                                where cat.CompanyId = '{0}'
                                and cat.Id = {1}";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyid, categoryid);
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

        public DataSet GetMenuItemListByCompanyId(Guid comid, int pageNo, int pageSize, string searchText, string order)
        {
            string searchQuery = "";
            string subquery = "";
            string subquery1 = "";

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                searchQuery = string.Format("and (item.ItemName like '%{0}%' or item.DaysAvailable like '%{0}%' or item.TimeAvailable like '%{0}%' or item.[Status] like '%{0}%')", searchText); ;
            }

            #region Order
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/categoryid")
                {
                    subquery = "order by #MenuItemData.[Id] asc";
                    subquery1 = "order by [Id] asc";
                }
                else if (order == "descending/categoryname")
                {
                    subquery = "order by #MenuItemData.[CategoryName] desc";
                    subquery1 = "order by [CategoryName] desc";
                }
                else if (order == "ascending/categoryname")
                {
                    subquery = "order by #MenuItemData.[CategoryName] asc";
                    subquery1 = "order by [CategoryName] asc";
                }
                else if (order == "ascending/categorydescription")
                {
                    subquery = "order by #MenuItemData.[Description] asc";
                    subquery1 = "order by [Description] asc";
                }
                else if (order == "descending/Categorydescription")
                {
                    subquery = "order by #MenuItemData.[Description] desc";
                    subquery1 = "order by [Description] desc";
                }
                else if (order == "ascending/categorydayavailable")
                {
                    subquery = "order by #MenuItemData.[DaysAvailable] asc";
                    subquery1 = "order by [DaysAvailable] asc";
                }
                else if (order == "descending/categorydayavailable")
                {
                    subquery = "order by #MenuItemData.[DaysAvailable] desc";
                    subquery1 = "order by [DaysAvailable] desc";
                }
                else if (order == "ascending/categorytimeavailable")
                {
                    subquery = "order by #MenuItemData.[TimeAvailable] asc";
                    subquery1 = "order by [TimeAvailable] asc";
                }
                else if (order == "descending/categorytimeavailable")
                {
                    subquery = "order by #MenuItemData.[TimeAvailable] desc";
                    subquery1 = "order by [TimeAvailable] desc";
                }
                else if (order == "ascending/categorytatus")
                {
                    subquery = "order by #MenuItemData.[Status] asc";
                    subquery1 = "order by [Status] asc";
                }
                else if (order == "descending/categorystatus")
                {
                    subquery = "order by #MenuItemData.[Status] desc";
                    subquery1 = "order by [Status] desc";
                }
                else
                {
                    subquery = "order by #MenuItemData.[Id] desc";
                    subquery1 = "order by Id desc";
                }

            }
            else
            {
                subquery = "order by #MenuItemData.[Id] desc";
                subquery1 = "order by Id desc";
            }
            #endregion
            string sqlQuery = @"declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)* @pagesize 
                                set @pageend = @pagesize

								select distinct item.*, menu.MenuName as MenuName, menu.Id as MenuId
								into #MenuItemData from MenuItem item
								left join MenuItemDetail itemdetail on itemdetail.MenuItemId = item.Id
								left join Menu menu on menu.Id = itemdetail.MenuId
                                where (item.[Status]=1 or item.[status]=0)
                                and menu.CompanyId = '{3}'
								{0}

								select * into #MenuItemFilterData from #MenuItemData

								select top(@pagesize) * from #MenuItemFilterData
								where Id not in (Select TOP (@pagestart)  Id from #MenuItemData {1})
								{2}

                                select Count(Id) As TotalCount from #MenuItemFilterData 

								drop table #MenuItemData
								drop table #MenuItemFilterData
                                    ";
            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        searchQuery,
                                        subquery,
                                        subquery1,
                                        comid
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
