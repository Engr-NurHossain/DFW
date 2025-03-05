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
	public partial class RestToppingDataAccess
	{
        public RestToppingDataAccess(string ConStr) : base(ConStr) { }

        public bool DeleteByToppingCategoryId(Guid TCId)
        {

            string SqlQuery = @"delete from RestTopping where ToppingCategoryId ='{0}' ";
            SqlQuery = string.Format(SqlQuery, TCId);
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
        public DataTable GetToppingByCategoryId(Guid CategoryId)
        {
            string sqlQuery = @"
                                select t.Id, t.ToppingCategoryId, t.ToppingName, format(t.Price,'N2') as Price, t.IsAvailable, tc.ToppingCategory as ToppingCategoryName, t.IsDefault, t.Description  from RestTopping t
                                left join RestToppingCategory tc on t.ToppingCategoryId=tc.ToppingCategoryId
                                where tc.ToppingCategoryId='{0}'
                                ";
            //string subquery = "";

            sqlQuery = string.Format(sqlQuery, CategoryId);
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
        public DataSet GetToppingList(int pageNo, int pageSize, string searchText, string order, Guid comId)
        {
            string searchQuery = "";
            string subquery = "";
            string subquery1 = "";

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                searchQuery = string.Format("and t.ToppingName like '%{0}%' or tc.ToppingCategory like '%{0}%' or t.Price like '%{0}%'", searchText); ;
            }

            #region Order
            if (!string.IsNullOrWhiteSpace(order) && order != "undefined")
            {
                if (order == "ascending/toppingid")
                {
                    subquery = "order by #ToppingData.[Id] asc";
                    subquery1 = "order by [Id] asc";
                }
                else if (order == "descending/toppingid")
                {
                    subquery = "order by #ToppingData.[Id] desc";
                    subquery1 = "order by [Id] desc";
                }
                else if (order == "descending/toppingname")
                {
                    subquery = "order by #ToppingData.[ToppingName] desc";
                    subquery1 = "order by [ToppingName] desc";
                }
                else if (order == "ascending/toppingname")
                {
                    subquery = "order by #ToppingData.[ToppingName] asc";
                    subquery1 = "order by [ToppingName] asc";
                }
                else if (order == "ascending/toppingcategory")
                {
                    subquery = "order by #ToppingData.[ToppingCategory] asc";
                    subquery1 = "order by [ToppingCategory] asc";
                }
                else if (order == "descending/toppingcategory")
                {
                    subquery = "order by #ToppingData.[ToppingCategory] desc";
                    subquery1 = "order by [ToppingCategory] desc";
                }
                else if (order == "ascending/toppingprice")
                {
                    subquery = "order by #ToppingData.[ToppingPrice] asc";
                    subquery1 = "order by [ToppingPrice] asc";
                }
                else if (order == "descending/toppingprice")
                {
                    subquery = "order by #ToppingData.[ToppingPrice] desc";
                    subquery1 = "order by [ToppingPrice] desc";
                }
                else if (order == "ascending/toppingavailable")
                {
                    subquery = "order by #ToppingData.[IdAvailable] asc";
                    subquery1 = "order by [IsAvailable] asc";
                }
                else if (order == "descending/toppingavailable")
                {
                    subquery = "order by #ToppingData.[IdAvailable] desc";
                    subquery1 = "order by [IsAvailable] desc";
                }
                else
                {
                    subquery = "order by #ToppingData.[CategoryId] desc";
                    subquery1 = "order by CategoryId desc";
                }
            }
            else
            {
                subquery = "order by #ToppingData.[CategoryId] desc";
                subquery1 = "order by CategoryId desc";
            }
            #endregion
            string sqlQuery = @"declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)* @pagesize 
                                set @pageend = @pagesize

								select distinct tc.Id as [CategoryId],tc.ToppingCategory as [ToppingCategory], STUFF((SELECT ', ' + CAST(_topping.ToppingName AS nvarchar(150)) [text()]
         from RestTopping _topping
		 where _topping.ToppingCategoryId = tc.ToppingCategoryId
         FOR XML PATH(''), TYPE)
        .value('.','NVARCHAR(MAX)'),1,2,' ') as ToppingName, STUFF((SELECT ', ' + CAST(_topping.Price AS nvarchar(150)) [text()]
         from RestTopping _topping
		 where _topping.ToppingCategoryId = tc.ToppingCategoryId
         FOR XML PATH(''), TYPE)
        .value('.','NVARCHAR(MAX)'),1,2,' ') as ToppingPrice
								into #ToppingData from RestToppingCategory tc
                                left join RestTopping t on t.ToppingCategoryId=tc.ToppingCategoryId
                                where tc.CompanyId='{3}'
								{0}

								select * into #ToppingFilterdata from #ToppingData

								select top(@pagesize) * from #ToppingFilterdata
								where [CategoryId] not in (Select TOP (@pagestart)  [CategoryId] from #ToppingData {1})
								{2}

                                select Count([CategoryId]) As TotalCount from #ToppingFilterdata 

								drop table #ToppingData
								drop table #ToppingFilterdata
                                    ";
            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        searchQuery,
                                        subquery,
                                        subquery1,
                                        comId
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

        public DataTable GetToppingListPartial(Guid companyid, Guid toppingcateoryid)
        {
            string sqlQuery = @"select
	                            mitem.ItemName,
	                            mitem.Price,
	                            mitem.MaxQty,
	                            mitem.TimeAvailable,
	                            mitem.DaysAvailable,
	                            mitem.Photo,
                                mitem.[Status],
                                mitem.Id,
                                mitem.UrlSlug
	                                from RestToppingCategory toppcat

                            left join RestMenuItemCategoryTopping mitemdet on mitemdet.ToppingCategoryId = toppcat.ToppingCategoryId
                            left join RestMenuItem mitem on mitem.ItemId = mitemdet.ItemId
                            where toppcat.CompanyId =  '{0}'
                            and toppcat.ToppingCategoryId = '{1}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyid, toppingcateoryid);
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
