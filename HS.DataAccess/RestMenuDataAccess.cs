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
	public partial class RestMenuDataAccess
	{
		public RestMenuDataAccess(string ConnectionString) : base(ConnectionString)
		{

		}

		public RestMenuDataAccess() { }

        public DataSet GetMenuList(Guid comid, int pageNo, int pageSize, string searchText, string order)
        {
            string searchQuery = "";
            string subquery = "";
            string subquery1 = "";
            string searchQuery1 = "";
            int noofitems = 0;
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                if (int.TryParse(searchText, out noofitems))
                {
                    searchQuery1 = string.Format("and NumberOfItems = {0}", noofitems);
                }
                else
                {
                    searchQuery = string.Format("and (m.MenuName like '%{0}%' or m.DaysAvailable like '%{0}%' or m.TimeAvailable like '%{0}%' or m.Status like '%{0}%')", searchText);
                }

            }

            #region Order
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/menuid")
                {
                    subquery = "order by #MenuData.[Id] asc";
                    subquery1 = "order by [Id] asc";
                }
                else if (order == "descending/menuid")
                {
                    subquery = "order by #MenuData.[Id] desc";
                    subquery1 = "order by [Id] desc";
                }
                else if (order == "descending/menuname")
                {
                    subquery = "order by #MenuData.[MenuName] desc";
                    subquery1 = "order by [MenuName] desc";
                }
                else if (order == "ascending/menuname")
                {
                    subquery = "order by #MenuData.[MenuName] asc";
                    subquery1 = "order by [MenuName] asc";
                }
                //else if (order == "ascending/menuitemcount")
                //{
                //    subquery = "order by #MenuData.[NumberOfItems] asc";
                //    subquery1 = "order by [NumberOfItems] asc";
                //}
                //else if (order == "descending/menuitemcount")
                //{
                //    subquery = "order by #MenuData.[NumberOfItems] desc";
                //    subquery1 = "order by [NumberOfItems] desc";
                //}
                else if (order == "ascending/menudayavailable")
                {
                    subquery = "order by #MenuData.[DaysAvailable] asc";
                    subquery1 = "order by [DaysAvailable] asc";
                }
                else if (order == "descending/menudayavailable")
                {
                    subquery = "order by #MenuData.[DaysAvailable] desc";
                    subquery1 = "order by [DaysAvailable] desc";
                }
                else if (order == "ascending/menutimeavailable")
                {
                    subquery = "order by #MenuData.[TimeAvailable] asc";
                    subquery1 = "order by [TimeAvailable] asc";
                }
                else if (order == "descending/menutimeavailable")
                {
                    subquery = "order by #MenuData.[TimeAvailable] desc";
                    subquery1 = "order by [TimeAvailable] desc";
                }
                else if (order == "ascending/menustatus")
                {
                    subquery = "order by #MenuData.[Status] asc";
                    subquery1 = "order by [Status] asc";
                }
                else if (order == "descending/menustatus")
                {
                    subquery = "order by #MenuData.[Status] desc";
                    subquery1 = "order by [Status] desc";
                }
                else
                {
                    subquery = "order by #MenuData.[Id] desc";
                    subquery1 = "order by Id desc";
                }

            }
            else
            {
                subquery = "order by #MenuData.[Id] desc";
                subquery1 = "order by Id desc";
            }
            #endregion
            string sqlQuery = @"declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)* @pagesize 
                                set @pageend = @pagesize

								select m.*,(select count(Id) from RestMenuItemCategory where MenuId=m.MenuId) as NumberOfItems
								into #MenuData from RestMenu m
                                where (m.[Status]=1 or m.[status]=0)
                                and m.CompanyId = '{3}'
								{0}

								select * into #MenuFilterdata from #MenuData

								select top(@pagesize) * from #MenuFilterdata
								where Id not in (Select TOP (@pagestart)  Id from #MenuData {1})
                                {4}
								{2}

                                select Count(Id) As TotalCount from #MenuFilterdata 

								drop table #MenuData
								drop table #MenuFilterdata
                                    ";
            try
            {
                sqlQuery = string.Format(sqlQuery,
                                        searchQuery,
                                        subquery,
                                        subquery1,
                                        comid,
                                        searchQuery1
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

        public DataTable GetAllWebsiteConfig(Guid comid, int pageno, int pagesize, string searchtext, string order)
        {
            string subquery = "";
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                subquery = string.Format("and (wc.SiteName like '%{0}%' or wc.DomainName like '%{0}%')", searchtext);
            }
            string sqlQuery = @"select top({1}*{2}) wc.* from WebsiteConfiguration wc
                                where wc.CompanyId = '{0}'
                                {3}
                                order by wc.Id desc";
            try
            {
                sqlQuery = string.Format(sqlQuery, comid, pageno, pagesize, subquery);
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

        public DataTable GetAllWebsiteLocation(Guid comid, int pageno, int pagesize, string searchtext, string order)
        {
            string searchquery = "";
            string subquery = "";
            string subquery1 = "";

            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                searchquery = string.Format("and (wl.Name like '%{0}%' or wl.Address like '%{0}%' or wl.City like '%{0}%' or wl.State like '%{0}%' or wl.ZipCode like '%{0}%')", searchtext);
            }
            #region Order
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/locationid")
                {
                    subquery = "order by #MenuFilterdata.[Id] asc";
                    subquery1 = "order by [Id] asc";
                }
                else if (order == "descending/locationid")
                {
                    subquery = "order by  #MenuFilterdata.[Id] desc";
                    subquery1 = "order by [Id] desc";
                }
                else if (order == "descending/locationname")
                {
                    subquery = "order by #MenuFilterdata.[Name] desc";
                    subquery1 = "order by [Name] desc";
                }
                else if (order == "ascending/locationname")
                {
                    subquery = "order by #MenuFilterdata.[Name] asc";
                    subquery1 = "order by [Name] asc";
                }
                //else if (order == "ascending/menuitemcount")
                //{
                //    subquery = "order by #MenuData.[NumberOfItems] asc";
                //    subquery1 = "order by [NumberOfItems] asc";
                //}
                //else if (order == "descending/menuitemcount")
                //{
                //    subquery = "order by #MenuData.[NumberOfItems] desc";
                //    subquery1 = "order by [NumberOfItems] desc";
                //}
                else if (order == "ascending/streetaddress")
                {
                    subquery = "order by #MenuFilterdata.[Address] asc";
                    subquery1 = "order by [Address] asc";
                }
                else if (order == "descending/streetaddress")
                {
                    subquery = "order by #MenuFilterdata.[Address] desc";
                    subquery1 = "order by [Address] desc";
                }
                else if (order == "ascending/city")
                {
                    subquery = "order by #MenuFilterdata.[City] asc";
                    subquery1 = "order by [City] asc";
                }
                else if (order == "descending/city")
                {
                    subquery = "order by #MenuFilterdata.[City] desc";
                    subquery1 = "order by [City] desc";
                }
                else if (order == "ascending/state")
                {
                    subquery = "order by #MenuFilterdata.[State] asc";
                    subquery1 = "order by [State] asc";
                }
                else if (order == "descending/state")
                {
                    subquery = "order by #MenuFilterdata.[State] desc";
                    subquery1 = "order by [State] desc";
                }
                else if (order == "ascending/zip")
                {
                    subquery = "order by #MenuFilterdata.[Zipcode] asc";
                    subquery1 = "order by [Zipcode] asc";
                }
                else if (order == "descending/zip")
                {
                    subquery = "order by #MenuFilterdata.[Zipcode] desc";
                    subquery1 = "order by [Zipcode] desc";
                }
                else
                {
                    subquery = "order by #MenuFilterdata.[Id] desc";
                    subquery1 = "order by Id desc";
                }

            }
            else
            {
                subquery = "order by #MenuFilterdata.[Id] desc";
                subquery1 = "order by Id desc";
            }
            #endregion
            string sqlQuery = @"select top({1}*{2}) wl.* into #LocationsData from WebsiteLocation wl
                                where (wl.CompanyId = '{0}' or wl.ReferCompanyId = '{0}')
                                {3}
                              select * into #MenuFilterdata from #LocationsData
                               select * from #MenuFilterdata
                                {4}
                                drop table #LocationsData
								drop table #MenuFilterdata";


            try
            {
                sqlQuery = string.Format(sqlQuery, comid, pageno, pagesize, searchquery, subquery, subquery1);
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

        public DataTable GetAllSiteContent(Guid comid, int pageno, int pagesize, string searchtext, string order)
        {
            string searchquery = "";
            string subquery = "";
            string subquery1 = "";

            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                searchquery = string.Format("and (_seo.Name like '%{0}%' or _seo.MetaTitle like '%{0}%' or _seo.OgTitle like '%{0}%')", searchtext);
            }
            #region Order
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/seoid")
                {
                    subquery = "order by #MenuFilterdata.[Id] asc";
                    subquery1 = "order by [Id] asc";
                }
                else if (order == "descending/seoid")
                {
                    subquery = "order by  #MenuFilterdata.[Id] desc";
                    subquery1 = "order by [Id] desc";
                }
                else if (order == "descending/pagename")
                {
                    subquery = "order by #MenuFilterdata.[Name] desc";
                    subquery1 = "order by [Name] desc";
                }
                else if (order == "ascending/pagename")
                {
                    subquery = "order by #MenuFilterdata.[Name] asc";
                    subquery1 = "order by [Name] asc";
                }
                //else if (order == "ascending/menuitemcount")
                //{
                //    subquery = "order by #MenuData.[NumberOfItems] asc";
                //    subquery1 = "order by [NumberOfItems] asc";
                //}
                //else if (order == "descending/menuitemcount")
                //{
                //    subquery = "order by #MenuData.[NumberOfItems] desc";
                //    subquery1 = "order by [NumberOfItems] desc";
                //}
                else if (order == "ascending/anchortext")
                {
                    subquery = "order by #MenuFilterdata.[PageUrl] asc";
                    subquery1 = "order by [PageUrl] asc";
                }
                else if (order == "descending/anchortext")
                {
                    subquery = "order by #MenuFilterdata.[PageUrl] desc";
                    subquery1 = "order by [PageUrl] desc";
                }
                else if (order == "ascending/metatitle")
                {
                    subquery = "order by #MenuFilterdata.[MetaTitle] asc";
                    subquery1 = "order by [MetaTitle] asc";
                }
                else if (order == "descending/metatitle")
                {
                    subquery = "order by #MenuFilterdata.[MetaTitle] desc";
                    subquery1 = "order by [MetaTitle] desc";
                }
                else if (order == "ascending/metadescription")
                {
                    subquery = "order by #MenuFilterdata.[MetaDescription] asc";
                    subquery1 = "order by [MetaDescription] asc";
                }
                else if (order == "descending/metadescription")
                {
                    subquery = "order by #MenuFilterdata.[MetaDescription] desc";
                    subquery1 = "order by [MetaDescription] desc";
                }
                else if (order == "ascending/publish")
                {
                    subquery = "order by #MenuFilterdata.[OgTitle] asc";
                    subquery1 = "order by [OgTitle] asc";
                }
                else if (order == "descending/publish")
                {
                    subquery = "order by #MenuFilterdata.[OgTitle] desc";
                    subquery1 = "order by [OgTitle] desc";
                }
                else if (order == "ascending/navigation")
                {
                    subquery = "order by #MenuFilterdata.[OgDescription] asc";
                    subquery1 = "order by [OgDescription] asc";
                }
                else if (order == "descending/navigation")
                {
                    subquery = "order by #MenuFilterdata.[OgDescription] desc";
                    subquery1 = "order by [OgDescription] desc";
                }
                else
                {
                    subquery = "order by #MenuFilterdata.[Id] desc";
                    subquery1 = "order by Id desc";
                }

            }
            else
            {
                subquery = "order by #MenuFilterdata.[Id] desc";
                subquery1 = "order by Id desc";
            }
            #endregion
            string sqlQuery = @"select _seo.* into #LocationsData from Seo _seo
                                where _seo.CompanyId = '{0}'
                                {3}
                               select * into #MenuFilterdata from #LocationsData
                               select * from #MenuFilterdata
                                {4}
                                drop table #LocationsData
								drop table #MenuFilterdata";
            try
            {
                sqlQuery = string.Format(sqlQuery, comid, pageno, pagesize, searchquery, subquery, subquery1);
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

        public DataTable GetAllSiteContactList(Guid comid, int pageno, int pagesize, string searchtext, string order)
        {
            string searchquery = "";
            string subquery = "";
            string subquery1 = "";
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                searchquery = string.Format("and (contact.FirstName like '%{0}%' or contact.LastName like '%{0}%' or REPLACE(contact.WorkPhone,'-','') = '{0}'  or REPLACE(contact.CellPhone,'-','') = '{0}' or contact.Email like '%{0}%' or contact.FirstName + ' ' + contact.LastName like '%{0}%')", searchtext.Replace("-", ""));
            }
            #region Order
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/contactid")
                {
                    subquery = "order by #MenuFilterdata.[Id] asc";
                    subquery1 = "order by [Id] asc";
                }
                else if (order == "descending/contactid")
                {
                    subquery = "order by  #MenuFilterdata.[Id] desc";
                    subquery1 = "order by [Id] desc";
                }
                else if (order == "descending/contactname")
                {
                    subquery = "order by #MenuFilterdata.[FirstName] desc";
                    subquery1 = "order by [FirstName] desc";
                }
                else if (order == "ascending/contactname")
                {
                    subquery = "order by #MenuFilterdata.[FirstName] asc";
                    subquery1 = "order by [FirstName] asc";
                }
                //else if (order == "ascending/menuitemcount")
                //{
                //    subquery = "order by #MenuData.[NumberOfItems] asc";
                //    subquery1 = "order by [NumberOfItems] asc";
                //}
                //else if (order == "descending/menuitemcount")
                //{
                //    subquery = "order by #MenuData.[NumberOfItems] desc";
                //    subquery1 = "order by [NumberOfItems] desc";
                //}
                else if (order == "ascending/cellphone")
                {
                    subquery = "order by #MenuFilterdata.[CellPhone] asc";
                    subquery1 = "order by [CellPhone] asc";
                }
                else if (order == "descending/cellphone")
                {
                    subquery = "order by #MenuFilterdata.[CellPhone] desc";
                    subquery1 = "order by [CellPhone] desc";
                }
                else if (order == "ascending/workphone")
                {
                    subquery = "order by #MenuFilterdata.[WorkPhone] asc";
                    subquery1 = "order by [WorkPhone] asc";
                }
                else if (order == "descending/workphone")
                {
                    subquery = "order by #MenuFilterdata.[WorkPhone] desc";
                    subquery1 = "order by [WorkPhone] desc";
                }
                else if (order == "ascending/email")
                {
                    subquery = "order by #MenuFilterdata.[Email] asc";
                    subquery1 = "order by [Email] asc";
                }
                else if (order == "descending/email")
                {
                    subquery = "order by #MenuFilterdata.[Email] desc";
                    subquery1 = "order by [Email] desc";
                }

                else
                {
                    subquery = "order by #MenuFilterdata.[Id] desc";
                    subquery1 = "order by Id desc";
                }

            }
            else
            {
                subquery = "order by #MenuFilterdata.[Id] desc";
                subquery1 = "order by Id desc";
            }
            #endregion
            string sqlQuery = @"select top({0}*{1}) contact.* into #LocationsData from SiteContact contact
                                where contact.CompanyId = '{3}'
                                {2}
                               select * into #MenuFilterdata from #LocationsData
                               select * from #MenuFilterdata
                                {4}
                                drop table #LocationsData
								drop table #MenuFilterdata ";
            try
            {
                sqlQuery = string.Format(sqlQuery, pageno, pagesize, searchquery, comid, subquery, subquery1);
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

        public DataSet GetAllMenuItemList(Guid comid, int pageNo, int pageSize, string searchText, string order)
        {
            string searchQuery = "";
            string subquery = "";
            string subquery1 = "";

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                searchQuery = string.Format("and  (_menu.MenuName like '%{0}%' or _item.ItemName  like '%{0}%' or _category.CategoryName like '%{0}%')", searchText);
            }
            if (!string.IsNullOrWhiteSpace(order))
            {
                if (order == "ascending/photo")
                {
                    subquery = "order by #md.ItemPhoto asc";
                    subquery1 = "order by ItemPhoto asc";
                }
                else if (order == "descending/photo")
                {
                    subquery = "order by #md.ItemPhoto desc";
                    subquery1 = "order by ItemPhoto desc";
                }
                else if (order == "ascending/menuname")
                {
                    subquery = "order by #md.MenuName asc";
                    subquery1 = "order by MenuName asc";
                }
                else if (order == "descending/menuname")
                {
                    subquery = "order by #md.MenuName desc";
                    subquery1 = "order by MenuName desc";
                }
                else if (order == "ascending/itemname")
                {
                    subquery = "order by #md.ItemName asc";
                    subquery1 = "order by ItemName asc";
                }
                else if (order == "descending/itemname")
                {
                    subquery = "order by #md.ItemName desc";
                    subquery1 = "order by ItemName desc";
                }
                else if (order == "ascending/categoryname")
                {
                    subquery = "order by #md.CategoryName asc";
                    subquery1 = "order by CategoryName asc";
                }
                else if (order == "descending/categoryname")
                {
                    subquery = "order by #md.CategoryName desc";
                    subquery1 = "order by CategoryName desc";
                }
                else if (order == "ascending/itemprice")
                {
                    subquery = "order by #md.ItemPrice asc";
                    subquery1 = "order by ItemPrice asc";
                }
                else if (order == "descending/itemprice")
                {
                    subquery = "order by #md.ItemPrice desc";
                    subquery1 = "order by ItemPrice desc";
                }
                else if (order == "ascending/dayavailable")
                {
                    subquery = "order by #md.DaysAvailable asc";
                    subquery1 = "order by DaysAvailable asc";
                }
                else if (order == "descending/dayavailable")
                {
                    subquery = "order by #md.DaysAvailable desc";
                    subquery1 = "order by DaysAvailable desc";
                }
                else if (order == "ascending/menustatus")
                {
                    subquery = "order by #md.[Status] asc";
                    subquery1 = "order by [Status] asc";
                }
                else if (order == "descending/menustatus")
                {
                    subquery = "order by #md.[Status] desc";
                    subquery1 = "order by [Status] desc";
                }
                else
                {
                    subquery = "order by #md.Id desc";
                    subquery1 = "order by Id desc";
                }
            }
            else
            {
                subquery = "order by #md.Id desc";
                subquery1 = "order by Id desc";
            }
            string sqlQuery = @"declare @pagestart int
                                declare @pageend int
                                set @pagestart=(@pageno-1)* @pagesize 
                                set @pageend = @pagesize

								select distinct _menu.Id, _menu.Photo as ItemPhoto, STUFF((SELECT ', ' + CAST(ItemName AS nvarchar(150)) [text()]
         from MenuItem MenuItem
		 left join MenuItemDetail MenuItemDetail on MenuItemDetail.MenuItemId = MenuItem.Id
		 left join Menu Menu on Menu.Id = MenuItemDetail.MenuId
         where menu.CompanyId = '{3}' and Menu.Id = _menu.Id
         FOR XML PATH(''), TYPE)
        .value('.','NVARCHAR(MAX)'),1,2,' ') as ItemName, _menu.MenuName, STUFF((SELECT ', ' + CAST(Category.CategoryName AS nvarchar(150)) [text()]
         from MenuItemDetail MenuItemDetail
		 left join CategoryDetail CategoryDetail on CategoryDetail.MenuId = MenuItemDetail.MenuItemId
		 left join Category Category on Category.Id = CategoryDetail.CategoryId
		 left join Menu Menu on Menu.Id = MenuItemDetail.MenuId
         where Category.CompanyId = '{3}' and Menu.Id = _menu.Id
		 group by Category.CategoryName
         FOR XML PATH(''), TYPE)
        .value('.','NVARCHAR(MAX)'),1,2,' ') as CategoryName, STUFF((SELECT ', ' + CAST(Price AS nvarchar(150)) [text()]
         from MenuItem MenuItem
		 left join MenuItemDetail MenuItemDetail on MenuItemDetail.MenuItemId = MenuItem.Id
		 left join Menu Menu on Menu.Id = MenuItemDetail.MenuId
         where menu.CompanyId = '{3}' and Menu.Id = _menu.Id
         FOR XML PATH(''), TYPE)
        .value('.','NVARCHAR(MAX)'),1,2,' ') as ItemPrice, _menu.DaysAvailable, _menu.[Status]
                                into #MenuData from RestMenu _menu
                                left join MenuItemDetail _itemdetail on _menu.Id = _itemdetail.MenuId
                                left join MenuItem _item on _itemdetail.MenuItemId = _item.Id
                                left join MenuDetail _menudetail on _menu.Id = _menudetail.MenuId
                                left join CategoryDetail _categorydetail on _item.Id = _categorydetail.MenuId
                                left join Category _category on _category.Id = _categorydetail.CategoryId
                                where _menu.CompanyId = '{3}'
                                {0}

								select * into #MenuFilterdata from #MenuData

								select top(@pagesize) * from #MenuFilterdata
								where Id not in (Select TOP (@pagestart)  Id from #MenuData #md {1})
                                {2}
                                select Count(Id) As TotalCount from #MenuFilterdata 

								drop table #MenuData
								drop table #MenuFilterdata
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

        public DataTable GetMenuItemListByMenuIdAndCompanyId(Guid comid, int id)
        {
            string sqlQuery = @"select MenuItem.*, Menu.Id as MenuId from MenuItem MenuItem
                                left join MenuItemDetail MenuItemDetail on MenuItemDetail.MenuItemId = MenuItem.Id
                                left join Menu Menu on Menu.Id = MenuItemDetail.MenuId
                                where menu.CompanyId = '{0}' and Menu.Id = {1}";
            try
            {
                sqlQuery = string.Format(sqlQuery, comid, id);
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

        public DataTable GetCategoryListByMenuIdAndCompanyId(Guid comid, int id)
        {
            string sqlQuery = @"select Category.Id, Category.CategoryName, Category.[Description], Category.DaysAvailable, Category.TimeAvailable, Category.[Status], Category.[Image]
                                from MenuItemDetail MenuItemDetail
                                left join CategoryDetail CategoryDetail on CategoryDetail.MenuId = MenuItemDetail.MenuItemId
                                left join Category Category on Category.Id = CategoryDetail.CategoryId
                                left join Menu Menu on Menu.Id = MenuItemDetail.MenuId
                                where Category.CompanyId = '{0}' and Menu.Id = {1}
                                group by Category.Id, Category.CategoryName, Category.[Description], Category.DaysAvailable, Category.TimeAvailable, Category.[Status], Category.[Image]";
            try
            {
                sqlQuery = string.Format(sqlQuery, comid, id);
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

        public DataTable GetToppingCategoryListByMenuIdAndCompanyId(Guid comid, int id)
        {
            string sqlQuery = @"select distinct _tcategory.*, STUFF((SELECT ', ' + CAST(_topping.ToppingName AS nvarchar(150)) [text()]
         from Topping _topping
		 where _topping.ToppingCategoryId = _tcategory.Id
         FOR XML PATH(''), TYPE)
        .value('.','NVARCHAR(MAX)'),1,2,' ') as ToppingName, STUFF((SELECT ', ' + CAST(_topping.Price AS nvarchar(150)) [text()]
         from Topping _topping
		 where _topping.ToppingCategoryId = _tcategory.Id
         FOR XML PATH(''), TYPE)
        .value('.','NVARCHAR(MAX)'),1,2,' ') as ToppingPrice from ToppingCategory _tcategory
left join MenuDetail _detail on _detail.ToppingCategoryId = _tcategory.Id
left join MenuItemDetail _itemdetail on _itemdetail.MenuItemId = _detail.MenuId
left join Menu _menu on _menu.Id = _itemdetail.MenuId
where _menu.CompanyId = '{0}' and _menu.Id = {1}";
            try
            {
                sqlQuery = string.Format(sqlQuery, comid, id);
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

        public DataTable GetMenuByCompanyIdAndMenuItemId(Guid comid, int id)
        {
            string sqlQuery = @"select _menu.* from RestMenu _menu
                                left join MenuItemDetail _itemdetail on _itemdetail.MenuId = _menu.Id
                                left join MenuItem _item on _item.Id = _itemdetail.MenuItemId
                                where _item.Id = {1} and _menu.CompanyId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, comid, id);
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

        public DataTable GetCategoryByCompanyIdAndMenuId(Guid comid, int id)
        {
            string sqlQuery = @"select cat.* from RestMenu menu
                                left join RestMenuItemCategory itemdetail on itemdetail.MenuId = menu.MenuId
                                left join RestCategory cat on cat.CategoryId = itemdetail.CategoryId
                                where menu.CompanyId = '{0}' and menu.Id = {1}
                                and cat.CategoryName is not null";
            try
            {
                sqlQuery = string.Format(sqlQuery, comid, id);
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

        public DataTable GetCategoryByCompanyIdAndMenuIdAndMenuItemId(Guid comid, int id, Guid itemid)
        {
            string sqlQuery = @"select cat.* from RestMenu menu
                                left join RestMenuItemCategory itemdetail on itemdetail.MenuId = menu.MenuId
                                left join RestCategory cat on cat.CategoryId = itemdetail.CategoryId
                                where menu.CompanyId = '{0}' and menu.Id = {1} and itemdetail.ItemId = '{2}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, comid, id, itemid);
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



        public bool DeleteResturantLiveDB(Guid companyid)
        {
            string sqlQuery = @"declare @CompanyId uniqueidentifier 
                                set @CompanyId = '{0}'
                                
                                Delete from Company where CompanyId = @CompanyId
                                Delete from Employee where CompanyId = @CompanyId
                                Delete from CompanyBranch where CompanyId = @CompanyId
                                Delete from UserBranch where CompanyId = @CompanyId
                                Delete from UserCompany where CompanyId = @CompanyId
                                Delete from UserLogin where CompanyId = @CompanyId
                                Delete from PermissionGroup where CompanyId = @CompanyId
                                Delete from UserPermission where CompanyId = @CompanyId
                                Delete from WebsiteLocation where CompanyId = @CompanyId
                                Delete from Language where CompanyId = @CompanyId
                                Delete from GlobalSetting where CompanyId = @CompanyId
                                Delete from GridSetting where CompanyId = @CompanyId
                                Delete from Lookup where CompanyId = @CompanyId
                                Delete from LocalizeResource where CompanyId = @CompanyId
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyid);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public DataTable GetAllMenuItemByMenuId(Guid MenuId)
        {
            string sqlQuery = @"
                                Select distinct mi.Id as MenuItemId, mi.ItemName, mi.ItemNumber, mi.Photo, mi.Price, mi.DaysAvailable,mi.Status, mi.UrlSlug,
                               STUFF((
                                      select ', '  +c.CategoryName  from RestCategory c left join RestMenuItemCategory _mid on _mid.CategoryId = c.CategoryId where _mid.ItemId=mid.ItemId									
										FOR XML PATH('')
										), 1, 1, '') as Categories, 
                                STUFF((
                                       select ', ' +tc.ToppingCategory from RestToppingCategory tc left join RestMenuItemCategoryTopping mict on mict.ToppingCategoryId = tc.ToppingCategoryId where mict.ItemId = mi.ItemId
                                        FOR XML PATH('')),1,1,'') as Toppings
                                from RestMenuItemCategory mid
                                left join RestMenuItem mi on mid.ItemId=mi.ItemId
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

        public DataTable GetAllMenuItemByCompanyId(Guid comid, string MenuList, string TextSearch, string categoryList, bool isstatus)
        {
            string MenuQuery = "";
            string SearchQuery = "";
            string CategoryQuery = "";
            string StatusQuery = "";
            if (!string.IsNullOrWhiteSpace(MenuList) && MenuList != "null")
            {
                MenuQuery = string.Format("and _itemdetail.MenuId in ({0})", MenuList);
            }
            if (!string.IsNullOrWhiteSpace(categoryList) && categoryList != "null")
            {
                CategoryQuery = string.Format("and _itemdetail.CategoryId in ({0})", categoryList);
            }
            if (!string.IsNullOrWhiteSpace(TextSearch))
            {
                SearchQuery = string.Format("and _item.ItemName like '%{0}%'", TextSearch);
            }
            if(isstatus == true)
            {
                StatusQuery = string.Format("and _item.[Status] = 1");
            }
            string sqlQuery = @"
                                select distinct _item.*, (select Id from RestMenu where MenuId = _itemdetail.MenuId) as MenuId,
                                STUFF((
                                      select ', '  +c.CategoryName  from RestCategory c left join RestMenuItemCategory rmic on rmic.CategoryId = c.CategoryId where _itemdetail.ItemId = rmic.ItemId										
										FOR XML PATH('')
										), 1, 1, '') as Categories, 
                                STUFF((
                                       select ', '  +m.MenuName  from RestMenu m left join RestMenuItemCategory rmic on rmic.MenuId = m.MenuId where _itemdetail.ItemId = rmic.ItemId and _itemdetail.CategoryId = rmic.CategoryId										
										FOR XML PATH('')
										), 1, 1, '') as MenuStr,
                                STUFF((
                                       select ', ' +tc.ToppingCategory from RestToppingCategory tc left join RestMenuItemCategoryTopping mict on mict.ToppingCategoryId = tc.ToppingCategoryId where _itemdetail.ItemId = mict.ItemId
                                        FOR XML PATH('')),1,1,'') as Toppings
                                from RestMenuItem _item
                                
                                left join RestMenuItemCategory _itemdetail on _item.ItemId = _itemdetail.ItemId
                                where _item.CompanyId = '{0}'
                                and MenuId is not null
                                {1}
                                {2}
                                {3}
                                {4}
                                order by _item.OrderBy asc
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, comid, MenuQuery, SearchQuery, CategoryQuery, StatusQuery);
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

        public bool DeleteByMenuId(Guid menuId)
        {

            string SqlQuery = @"delete from RestMenuCategory where MenuId ='{0}' ";
            SqlQuery = string.Format(SqlQuery, menuId);
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

        public bool DeleteByMenuIdAndCategoryId(Guid menuId, Guid categoryid)
        {

            string SqlQuery = @"delete from RestMenuCategory where MenuId ='{0}' and CategoryId = '{1}'";
            SqlQuery = string.Format(SqlQuery, menuId, categoryid);
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

            string SqlQuery = @"delete from MenuDetail where ToppingCategoryId ='{0}' ";
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

        public bool DeleteAllWebLocOptByCompanyIdAndSiteLocId(Guid comid, int sitelocid, string day)
        {

            string SqlQuery = @"delete from WebsiteLocationOperation where CompanyId = '{0}' and SiteLocationId = {1} and HoursofOperation like '%{2}%'";
            SqlQuery = string.Format(SqlQuery, comid, sitelocid, day);
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

        public bool DeleteAllAdditionalContentByItemId(Guid itemid)
        {

            string SqlQuery = @"delete from RestMenuItemAdditionalContent where ItemId = '{0}'";
            SqlQuery = string.Format(SqlQuery, itemid);
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

        public bool DeleteMenuItemAndRelation(Guid itemid)
        {

            string SqlQuery = @"delete from RestMenuItemCategory where ItemId ='{0}'
                                delete from RestMenuItemCategoryTopping where ItemId ='{0}'";
            SqlQuery = string.Format(SqlQuery, itemid);
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

        public bool DeleteMenuRelation(Guid menuid)
        {

            string SqlQuery = @"delete from RestMenuItemCategory where MenuId ='{0}'
                                delete from RestMenuItemCategoryTopping where MenuId ='{0}'
                                delete from RestMenuCategory where MenuId ='{0}'";
            SqlQuery = string.Format(SqlQuery, menuid);
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

        public DataTable GetItemsReportByCompanyId(Guid comid)
        {
            string sqlQuery = @"select distinct 
                                (select COUNT(_item.Id) from RestMenuItem _item where _item.CompanyId = '{0}' and _item.[Status] = 1) as TotalActive
                                ,(select COUNT(_item.Id) from RestMenuItem _item where _item.CompanyId = '{0}') as TotalItems
                                ,(select ISNULL((SUM(_item.Price) / COUNT(_item.Id)), 0) from RestMenuItem _item where _item.CompanyId = '{0}' and _item.[Status] = 1) as AveragePrice
                                from RestMenuItem";
            try
            {
                sqlQuery = string.Format(sqlQuery, comid);
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

        public bool DeleteResturantNeighbarhoodByLocationId(int value)
        {
            string sqlQuery = @"delete from ResturantNeighborhood where SiteLocationId = {0}";
            try
            {
                sqlQuery = string.Format(sqlQuery, value);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CloneRestaurantMenuByCompanyId(Guid comid, Guid oldcomid, Guid userid)
        {
            string sqlQuery = @"declare @companyid uniqueidentifier
                                set @companyid = '{0}'
                                
                                declare @userid uniqueidentifier
                                set @userid = '{2}'

                                delete from RestMenu where CompanyId = @companyid

                                Insert into RestMenu([CompanyId],[MenuId],[MenuName],[Status],[TimeAvailable],[DaysAvailable],[Description],[Photo],[CreatedDate],[CreatedBy],[LastUpdatedBy],[LastUpdatedDate],[DaysAvailableOption],[TimeAvailableOption],[UrlSlug],[WebsiteURL],[MetaTitle],[MetaDescription])
                                select @companyid,(select NEWID()),[MenuName],[Status],[TimeAvailable],[DaysAvailable],[Description],[Photo],(select GetDate()),@userid,@userid,(select GetDate()),[DaysAvailableOption],[TimeAvailableOption],[UrlSlug],((select WebsiteURL from WebsiteLocation where CompanyId = @companyid) + '/' + [UrlSlug]),[MetaTitle],[MetaDescription] from RestMenu where CompanyId = '{1}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, comid, oldcomid, userid);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CloneRestaurantCategoryByCompanyId(Guid comid, Guid oldcomid, Guid userid)
        {
            string sqlQuery = @"declare @companyid uniqueidentifier
                                set @companyid = '{0}'
                                
                                declare @userid uniqueidentifier
                                set @userid = '{2}'

                                delete from RestCategory where CompanyId = @companyid

                                Insert into RestCategory([CategoryId],[CategoryName],[Description],[DaysAvailable],[TimeAvailable],[Status],[Image],[CreatedDate],[CreatedBy],[LastUpdatedBy],[LastUpdatedDate],[CompanyId],[UrlSlug],[DaysAvailableOption],[TimeAvailableOption],[WebsiteURL],[MetaTitle],[MetaDescription],[OrderBy])
                                select (select NEWID()),[CategoryName],[Description],[DaysAvailable],[TimeAvailable],[Status],[Image],(select GetDate()),@userid,@userid,(select GetDate()),@companyid,[UrlSlug],[DaysAvailableOption],[TimeAvailableOption],((select WebsiteURL from WebsiteLocation where CompanyId = @companyid) + '/' + [UrlSlug]),[MetaTitle],[MetaDescription],[OrderBy] from RestCategory where CompanyId = '{1}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, comid, oldcomid, userid);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CloneRestaurantToppingByCompanyId(Guid comid, Guid oldcomid, Guid userid)
        {
            string sqlQuery = @"Delete from RestToppingCategory where CompanyId = '{0}'
                                Delete from RestTopping where CompanyId = '{0}'
                                
                                declare @companyid uniqueidentifier
                                set @companyid = '{0}'
                                
                                declare @userid uniqueidentifier
                                set @userid = '{2}'

                                Select *
                                Into   #Temp
                                From   RestToppingCategory where CompanyId = '{1}'

                                Declare @categoryid uniqueidentifier

                                Declare @newcategoryid uniqueidentifier

                                While (Select Count(*) From #Temp) > 0
                                Begin

                                    Select Top 1 @categoryid = ToppingCategoryId From #Temp

	                                set @newcategoryid = (select NEWID())

                                    INSERT into [RestToppingCategory] ([ToppingCategoryId], [ToppingCategory], [CompanyId]) 
	                                select @newcategoryid, [ToppingCategory], @companyid from RestToppingCategory where ToppingCategoryId = @categoryid

	                                INSERT into [RestTopping] ([ToppingId], [ToppingName], [Price],[IsAvailable],[CreatedDate],[CreatedBy],[LastUpdatedBy],[LastUpdatedDate],[CompanyId],[ToppingCategoryId]) 
	                                select (select NEWID()), [ToppingName], [Price],[IsAvailable],(select GetDate()),@userid,@userid,(select GetDate()),@companyid,@newcategoryid from RestTopping where ToppingCategoryId = @categoryid

                                    Delete #Temp Where ToppingCategoryId = @categoryid

                                End";
            try
            {
                sqlQuery = string.Format(sqlQuery, comid, oldcomid, userid);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CloneRestaurantMenuItemByCompanyId(Guid comid, Guid oldcomid, Guid userid)
        {
            string sqlQuery = @"declare @companyid uniqueidentifier
                                set @companyid = '{0}'
                                
                                declare @userid uniqueidentifier
                                set @userid = '{2}'

                                delete from RestMenuItem where CompanyId = @companyid
                                delete from RestMenuCategory where CompanyId = @companyid
                                delete from RestMenuItemCategory where CompanyId = @companyid
                                delete from RestMenuItemCategoryTopping where CompanyId = @companyid

                                Select *
                                Into   #Temp
                                From   RestMenuItem where CompanyId = '{1}'

                                Declare @itemid uniqueidentifier

                                Declare @micid int

                                Declare @mictid int

                                Declare @newitemid uniqueidentifier

                                Declare @newmenuid uniqueidentifier

                                Declare @newcategoryid uniqueidentifier

                                Declare @newtopcategoryid uniqueidentifier

                                While (Select Count(*) From #Temp) > 0
                                Begin

                                    Select Top 1 @itemid = ItemId From #Temp

	                                set @newitemid = (select NEWID())

                                    INSERT into [RestMenuItem] ([ItemId], [ItemName], [ItemNumber],[ItemLevel],[Description],[Photo],[MaxQty],[DaysAvailable],[TimeAvailable],[Price],[Status],[DaysAvailableOption],[TimeAvailableOption],[CompanyId],[UrlSlug],[WebsiteURL],[MetaTitle],[MetaDescription],[DeliveryTime],[IsTax],[TaxPercentage],[OrderBy],[IsInstruction]) 
	                                select @newitemid, [ItemName], [ItemNumber],[ItemLevel],[Description],[Photo],[MaxQty],[DaysAvailable],[TimeAvailable],[Price],[Status],[DaysAvailableOption],[TimeAvailableOption],@companyid,[UrlSlug],[WebsiteURL],[MetaTitle],[MetaDescription],[DeliveryTime],[IsTax],[TaxPercentage],[OrderBy],[IsInstruction] from RestMenuItem where ItemId = @itemid

	                                select * into #temp1 from RestMenuItemCategory where ItemId = @itemid
	
	                                while (select COUNT(*) from #temp1) > 0
	                                Begin

                                        Select Top 1 @micid = Id From #temp1
		
		                                Select Top 1 @newmenuid = MenuId From #temp1

		                                Select Top 1 @newcategoryid = CategoryId From #temp1

		                                set @newmenuid = (select top 1 (select _menu.MenuId from RestMenu _menu where _menu.UrlSlug = _rm.UrlSlug and _menu.CompanyId = @companyid) from RestMenu _rm where _rm.MenuId = @newmenuid)

		                                set @newcategoryid = (select top 1 (select _cat.CategoryId from RestCategory _cat where _cat.UrlSlug = _rc.UrlSlug and _cat.CompanyId = @companyid) from RestCategory _rc where _rc.CategoryId = @newcategoryid)

		                                update RestMenuItem set WebsiteURL = (select WebsiteURL from WebsiteLocation where CompanyId = @companyid) + '/' + (select UrlSlug from RestMenu where MenuId = @newmenuid) + '/' + (select UrlSlug from RestMenuItem where ItemId = @itemid) where ItemId = @itemid

                                        INSERT into [RestMenuItemCategory] ([CompanyId], [MenuId], [CategoryId],[ItemId],[CreatedDate],[CreatedBy]) 
		                                values (@companyid, ISNULL(@newmenuid, '00000000-0000-0000-0000-000000000000'), ISNULL(@newcategoryid, '00000000-0000-0000-0000-000000000000'),@newitemid,(select GetDate()),@userid)

		                                INSERT into [RestMenuCategory] ([CompanyId], [MenuId], [CategoryId],[CreatedDate],[CreatedBy]) 
		                                values (@companyid, ISNULL(@newmenuid, '00000000-0000-0000-0000-000000000000'), ISNULL(@newcategoryid, '00000000-0000-0000-0000-000000000000'),(select GetDate()),@userid)

                                        INSERT into [RestMenuItemCategoryURL] ([MenuId], [MenuItemId],[MenuCategoryURL],[ItemCategoryURL],[CreatedDate],[CreatedBy]) 
		                                values (isnull((select Id from RestMenu where MenuId = @newmenuid), 0), isnull((select Id from RestMenuItem where ItemId = @newitemid), 0),((select WebsiteURL from RestMenu where MenuId = @newmenuid) + '/' + (select UrlSlug from RestCategory where CategoryId = @newcategoryid)),((select WebsiteURL from RestMenu where MenuId = @newmenuid) + '/' + (select UrlSlug from RestCategory where CategoryId = @newcategoryid) + '/' + (select UrlSlug from RestMenuItem where ItemId = @newitemid)),(select GetDate()),@userid)

		                                Delete #temp1 Where Id = @micid

	                                End
                                    
                                    select * into #temp2 from RestMenuItemCategoryTopping where ItemId = @itemid

	                                while (select COUNT(*) from #temp2) > 0
	                                Begin

		                                Select Top 1 @mictid = Id From #temp2
		
		                                Select Top 1 @newmenuid = MenuId From #temp2

		                                Select Top 1 @newtopcategoryid = ToppingCategoryId From #temp2

		                                set @newmenuid = (select top 1 (select _menu.MenuId from RestMenu _menu where _menu.UrlSlug = _rm.UrlSlug and _menu.CompanyId = @companyid) from RestMenu _rm where _rm.MenuId = @newmenuid)

		                                set @newtopcategoryid = (select top 1 (select _cat.ToppingCategoryId from RestToppingCategory _cat where _cat.ToppingCategory = _rc.ToppingCategory and _cat.CompanyId = @companyid) from RestToppingCategory _rc where _rc.ToppingCategoryId = @newtopcategoryid)
		
		                                INSERT into [RestMenuItemCategoryTopping] ([CompanyId], [MenuId], [ToppingCategoryId],[ItemId],[CreatedDate],[CreatedBy]) 
		                                values (@companyid, ISNULL(@newmenuid, '00000000-0000-0000-0000-000000000000'), ISNULL(@newtopcategoryid, '00000000-0000-0000-0000-000000000000'),@newitemid,(select GetDate()),@userid)

		                                Delete #temp2 Where Id = @mictid

	                                End

                                    Delete #Temp Where ItemId = @itemid

                                    drop table #temp1
                                    
                                    drop table #temp2

                                End";
            try
            {
                sqlQuery = string.Format(sqlQuery, comid, oldcomid, userid);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteAllItemsByCompanyId(Guid comid)
        {
            string sqlQuery = @"declare @companyid uniqueidentifier
                                set @companyid = '{0}'

                                delete from RestMenuItem where CompanyId = @companyid
                                delete from RestMenuCategory where CompanyId = @companyid
                                delete from RestMenuItemCategory where CompanyId = @companyid
                                delete from RestMenuItemCategoryTopping where CompanyId = @companyid";
            try
            {
                sqlQuery = string.Format(sqlQuery, comid);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public DataTable GetAllReviewListByCompanyId(Guid comid)
        {
            string sqlQuery = @"select rr.*, item.ItemName from ResturantReview rr
                                left join RestMenuItem item on item.CompanyId = rr.CompanyId and item.Id = iif(rr.ReviewFor = 'Restaurant', 0, convert(int, rr.ReviewFor))
                                where rr.Companyid = '{0}'
                                order by rr.Id desc";
            try
            {
                sqlQuery = string.Format(sqlQuery, comid);
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

        public bool DeleteLocationMapInfoByCompanyId(Guid comid)
        {
            string sqlQuery = @"delete from WebsiteLocationMapInfo where CompanyId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, comid);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }	
}
