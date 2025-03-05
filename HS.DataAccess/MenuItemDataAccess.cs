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
	public partial class MenuItemDataAccess
	{
        public MenuItemDataAccess(string ConStr) : base(ConStr) { }

        public DataTable GetAllMenuItemByMenuId(int MenuId)
        {
            string sqlQuery = @"
                                Select mi.Id as MenuItemId, mi.ItemName, mi.ItemNumber, mi.Photo, mi.Price, mi.DaysAvailable,mi.Status,
                               STUFF((
                                      select ', '  +c.CategoryName  from CategoryDetail cd left join Category c on cd.CategoryId=c.Id where cd.MenuId=mi.Id										
										FOR XML PATH('')
										), 1, 1, '') as Categories, 
                                STUFF((
                                       select ', ' +tc.ToppingCategory from MenuDetail md left join ToppingCategory tc on md.ToppingCategoryId=tc.Id where md.MenuId=mi.Id 
                                        FOR XML PATH('')),1,1,'') as Toppings
                                from MenuItemDetail mid
                                left join MenuItem mi on mid.MenuItemId=mi.Id
                                where mid.MenuId='{0}'
                                ";
            //string subquery = "";

            sqlQuery = string.Format(sqlQuery, MenuId);
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
        public DataTable GetMenuItemListBySearchKey(string key, int MaxLoad, string ExistItem)
        {
            string sqlQuery = @"
                                Declare @SerchText nvarchar(100)
                                set @SerchText = '%{0}%'
                                select
                                Top {1}
                                mi.Id as MenuItemId
		                        ,mi.ItemName as ItemName
                                from MenuItem mi
                                where (mi.Name like @SerchText)
                                {2}";
            var iExist = "";
            if (!string.IsNullOrEmpty(ExistItem))
            {
                iExist = string.Format("AND mi.Id not in {0}", ExistItem);
            }
            try
            {
                sqlQuery = string.Format(sqlQuery, key, MaxLoad, iExist);
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

        public DataTable GetAllMenuItemByCompanyId(Guid comid, string MenuList, string TextSearch)
        {
            string MenuQuery = "";
            string SearchQuery = "";
            if(!string.IsNullOrWhiteSpace(MenuList) && MenuList != "null")
            {
                MenuQuery = string.Format("and _itemdetail.MenuId in ({0})", MenuList);
            }
            if (!string.IsNullOrWhiteSpace(TextSearch))
            {
                SearchQuery = string.Format("and _item.ItemName like '%{0}%'", TextSearch);
            }
            string sqlQuery = @"
                                select _item.*, _itemdetail.MenuId,
                                STUFF((
                                      select ', '  +c.CategoryName  from CategoryDetail cd left join Category c on cd.CategoryId=c.Id where cd.MenuId=_itemdetail.Id										
										FOR XML PATH('')
										), 1, 1, '') as Categories, 
                                STUFF((
                                       select ', ' +tc.ToppingCategory from MenuDetail md left join ToppingCategory tc on md.ToppingCategoryId=tc.Id where md.MenuId=_itemdetail.Id 
                                        FOR XML PATH('')),1,1,'') as Toppings
                                from MenuItem _item
                                
                                left join MenuItemDetail _itemdetail on _item.Id = _itemdetail.MenuItemId
                                where _item.CompanyId = '{0}'
                                {1}
                                {2}
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, comid, MenuQuery, SearchQuery);
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
