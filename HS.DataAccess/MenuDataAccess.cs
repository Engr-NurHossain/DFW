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
	public partial class MenuDataAccess
	{
        public MenuDataAccess(string ConnectionString) : base(ConnectionString)
        {

        }

        public MenuDataAccess() { }
        public DataSet GetMenuList(Guid comid, int pageNo, int pageSize, string searchText, string order)
        {
            string searchQuery = "";
            string subquery = "";
            string subquery1 = "";
            string searchQuery1 = "";
            int noofitems = 0;
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                if(int.TryParse(searchText, out noofitems))
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

								select m.*,(select count(Id) from MenuItemDetail where MenuId=m.Id) as NumberOfItems
								into #MenuData from Menu m
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
                                {3}";
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
                                where wl.CompanyId = '{0}'
                                {3}
                              select * into #MenuFilterdata from #LocationsData
                               select * from #MenuFilterdata
                                {4}
                                drop table #LocationsData
								drop table #MenuFilterdata";
                            

            try
            {
                sqlQuery = string.Format(sqlQuery, comid, pageno, pagesize, searchquery,subquery, subquery1);
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
            string sqlQuery = @"select top({1}*{2}) _seo.* into #LocationsData from Seo _seo
                                where _seo.CompanyId = '{0}'
                                {3}
                               select * into #MenuFilterdata from #LocationsData
                               select * from #MenuFilterdata
                                {4}
                                drop table #LocationsData
								drop table #MenuFilterdata";
            try
            {
                sqlQuery = string.Format(sqlQuery, comid, pageno, pagesize, searchquery,subquery,subquery1);
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
                sqlQuery = string.Format(sqlQuery, pageno, pagesize, searchquery, comid,subquery,subquery1);
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
                if(order == "ascending/photo")
                {
                    subquery = "order by #md.ItemPhoto asc";
                    subquery1 = "order by ItemPhoto asc";
                }
                else if(order == "descending/photo")
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
                                into #MenuData from Menu _menu
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
            string sqlQuery = @"select _menu.* from Menu _menu
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
            string sqlQuery = @"select cat.* from Menu menu
                                left join MenuItemDetail itemdetail on itemdetail.MenuId = menu.Id
                                left join CategoryDetail catdetail on catdetail.MenuId = itemdetail.MenuItemId
                                left join Category cat on cat.Id = catdetail.CategoryId
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

        public DataTable GetCategoryByCompanyIdAndMenuIdAndMenuItemId(Guid comid, int id, int itemid)
        {
            string sqlQuery = @"select cat.* from Menu menu
                                left join MenuItemDetail itemdetail on itemdetail.MenuId = menu.Id
                                left join CategoryDetail catdetail on catdetail.MenuId = itemdetail.MenuItemId
                                left join Category cat on cat.Id = catdetail.CategoryId
                                where menu.CompanyId = '{0}' and menu.Id = {1} and itemdetail.MenuItemId = {2}";
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
    }	
}
