using HS.DataAccess;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HS.Entities;
using System.Data;
using System.Collections;
using HS.Framework.Utils;
using System.Net.Mail;
using System.Web;

namespace HS.Facade
{
    public class MenuFacade : BaseFacade
    {
        WebsiteLocationDataAccess _WebsiteLocationDataAccess;
        RestMenuDataAccess _RestMenuDataAccess;
        public MenuFacade(ClientContext clientContext)
            : base(clientContext)
        {
            _WebsiteLocationDataAccess = (WebsiteLocationDataAccess)_ClientContext[typeof(WebsiteLocationDataAccess)];
            _RestMenuDataAccess = (RestMenuDataAccess)_ClientContext[typeof(RestMenuDataAccess)];
        }

        public MenuFacade()
        {
            _WebsiteLocationDataAccess = new WebsiteLocationDataAccess();
            _RestMenuDataAccess = new RestMenuDataAccess();
        }

        public MenuFacade(string constr)
        {
            _WebsiteLocationDataAccess = new WebsiteLocationDataAccess(constr);
            _RestMenuDataAccess = new RestMenuDataAccess(constr);
        }
        RestMenuItemCategoryToppingDataAccess _RestMenuItemCategoryToppingDataAccess
        {
            get
            {
                return (RestMenuItemCategoryToppingDataAccess)_ClientContext[typeof(RestMenuItemCategoryToppingDataAccess)];
            }
        }
        MenuItemDetailDataAccess _MenuItemDetailDataAccess
        {
            get
            {
                return (MenuItemDetailDataAccess)_ClientContext[typeof(MenuItemDetailDataAccess)];
            }
        }
        RestMenuItemDataAccess _RestMenuItemDataAccess
        {
            get
            {
                return (RestMenuItemDataAccess)_ClientContext[typeof(RestMenuItemDataAccess)];
            }
        }
        RestCategoryDataAccess _RestCategoryDataAccess
        {
            get
            {
                return (RestCategoryDataAccess)_ClientContext[typeof(RestCategoryDataAccess)];
            }
        }
        ToppingDataAccess _ToppingDataAccess
        {
            get
            {
                return (ToppingDataAccess)_ClientContext[typeof(ToppingDataAccess)];
            }
        }
        ToppingCategoryDataAccess _ToppingCategoryDataAccess
        {
            get
            {
                return (ToppingCategoryDataAccess)_ClientContext[typeof(ToppingCategoryDataAccess)];
            }
        }
        WebsiteConfigurationDataAccess _WebsiteConfigurationDataAccess
        {
            get
            {
                return (WebsiteConfigurationDataAccess)_ClientContext[typeof(WebsiteConfigurationDataAccess)];
            }
        }
        SeoDataAccess _SeoDataAccess
        {
            get
            {
                return (SeoDataAccess)_ClientContext[typeof(SeoDataAccess)];
            }
        }
        SiteContactDataAccess _SiteContactDataAccess
        {
            get
            {
                return (SiteContactDataAccess)_ClientContext[typeof(SiteContactDataAccess)];
            }
        }

        WebsiteLocationOperationDataAccess _WebsiteLocationOperationDataAccess
        {
            get
            {
                return (WebsiteLocationOperationDataAccess)_ClientContext[typeof(WebsiteLocationOperationDataAccess)];
            }
        }

        RestMenuItemCategoryURLDataAccess _RestMenuItemCategoryURLDataAccess
        {
            get
            {
                return (RestMenuItemCategoryURLDataAccess)_ClientContext[typeof(RestMenuItemCategoryURLDataAccess)];
            }
        }

        RestMenuItemCategoryDataAccess _RestMenuItemCategoryDataAccess
        {
            get
            {
                return (RestMenuItemCategoryDataAccess)_ClientContext[typeof(RestMenuItemCategoryDataAccess)];
            }
        }

        RestMenuCategoryDataAccess _RestMenuCategoryDataAccess
        {
            get
            {
                return (RestMenuCategoryDataAccess)_ClientContext[typeof(RestMenuCategoryDataAccess)];
            }
        }

        RestToppingCategoryDataAccess _RestToppingCategoryDataAccess
        {
            get
            {
                return (RestToppingCategoryDataAccess)_ClientContext[typeof(RestToppingCategoryDataAccess)];
            }
        }

        ResturantOrderDataAccess _ResturantOrderDataAccess
        {
            get
            {
                return (ResturantOrderDataAccess)_ClientContext[typeof(ResturantOrderDataAccess)];
            }
        }

        SocialMediaContentDataAccess _SocialMediaContentDataAccess
        {
            get
            {
                return (SocialMediaContentDataAccess)_ClientContext[typeof(SocialMediaContentDataAccess)];
            }
        }

        RestMenuItemAdditionalContentDataAccess _RestMenuItemAdditionalContentDataAccess
        {
            get
            {
                return (RestMenuItemAdditionalContentDataAccess)_ClientContext[typeof(RestMenuItemAdditionalContentDataAccess)];
            }
        }

        ResturantSystemSettingDataAccess _ResturantSystemSettingDataAccess
        {
            get
            {
                return (ResturantSystemSettingDataAccess)_ClientContext[typeof(ResturantSystemSettingDataAccess)];
            }
        }

        ResturantOrderDetailDataAccess _ResturantOrderDetailDataAccess
        {
            get
            {
                return (ResturantOrderDetailDataAccess)_ClientContext[typeof(ResturantOrderDetailDataAccess)];
            }
        }

        ResturantNeighborhoodDataAccess _ResturantNeighborhoodDataAccess
        {
            get
            {
                return (ResturantNeighborhoodDataAccess)_ClientContext[typeof(ResturantNeighborhoodDataAccess)];
            }
        }

        ResturantReviewDataAccess _ResturantReviewDataAccess
        {
            get
            {
                return (ResturantReviewDataAccess)_ClientContext[typeof(ResturantReviewDataAccess)];
            }
        }

        WebsiteLocationMapInfoDataAccess _WebsiteLocationMapInfoDataAccess
        {
            get
            {
                return (WebsiteLocationMapInfoDataAccess)_ClientContext[typeof(WebsiteLocationMapInfoDataAccess)];
            }
        }

        TrackingNumberSettingDataAccess _TrackingNumberSettingDataAccess
        {
            get
            {
                return (TrackingNumberSettingDataAccess)_ClientContext[typeof(TrackingNumberSettingDataAccess)];
            }
        }

        CustomerDataAccess _CustomerDataAccess
        {
            get
            {
                return (CustomerDataAccess)_ClientContext[typeof(CustomerDataAccess)];
            }
        }

        TrackingNumberRecordedDataAccess _TrackingNumberRecordedDataAccess
        {
            get
            {
                return (TrackingNumberRecordedDataAccess)_ClientContext[typeof(TrackingNumberRecordedDataAccess)];
            }
        }

        RestaurantRewardsDataAccess _RestaurantRewardsDataAccess
        {
            get
            {
                return (RestaurantRewardsDataAccess)_ClientContext[typeof(RestaurantRewardsDataAccess)];
            }
        }

        RestaurantCouponsDataAccess _RestaurantCouponsDataAccess
        {
            get
            {
                return (RestaurantCouponsDataAccess)_ClientContext[typeof(RestaurantCouponsDataAccess)];
            }
        }

        public MenuListModel GetMenuList(Guid comid, int pageNo, int pageSize, string searchText, string order)
        {
            MenuListModel Model = new MenuListModel();
            DataSet ds = _RestMenuDataAccess.GetMenuList(comid, pageNo, pageSize, searchText, order);

            Model.Menus = (from DataRow dr in ds.Tables[0].Rows
                           select new RestMenu()
                           {
                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                               MenuName = dr["MenuName"].ToString(),
                               //MenuItemId= dr["MenuItemId"] != DBNull.Value ? Convert.ToInt32(dr["MenuItemId"]) : 0,
                               NumberOfItems = dr["NumberOfItems"] != DBNull.Value ? Convert.ToInt32(dr["NumberOfItems"]) : 0,
                               DaysAvailable = dr["DaysAvailable"].ToString(),
                               TimeAvailable = dr["TimeAvailable"].ToString(),
                               Status = Convert.ToBoolean(dr["Status"]),
                               DaysAvailableOption = dr["DaysAvailableOption"].ToString()
                           }).ToList();

            Model.TotalCount = ds.Tables[1].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCount"]) : 0;
            Model.PageNo = pageNo;
            Model.PageSize = pageSize;
            Model.Searchtext = searchText;
            return Model;
        }

        public List<RestMenuItem> GetAllMenuItem()
        {
            return _RestMenuItemDataAccess.GetAll();
        }

        public List<RestMenu> GetAllMenuByCompanyId(Guid comid)
        {
            return _RestMenuDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", comid)).ToList();
        }

        public List<RestMenuItem> GetAllMenuItemByCompanyId(Guid comid)
        {
            return _RestMenuItemDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", comid)).ToList();
        }

        public List<RestMenuItem> GetAllMenuItemByCompanyId(Guid comid, string MenuList, string TextSearch, string categoryList, bool isstatus)
        {
            DataTable dt = _RestMenuDataAccess.GetAllMenuItemByCompanyId(comid, MenuList, TextSearch, categoryList, isstatus);
            List<RestMenuItem> miList = new List<RestMenuItem>();
            miList = (from DataRow dr in dt.Rows
                      select new RestMenuItem()
                      {
                          Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                          ItemName = dr["ItemName"].ToString(),
                          ItemNumber = dr["ItemNumber"].ToString(),
                          Photo = dr["Photo"].ToString(),
                          CategoryModel = dr["Categories"].ToString(),
                          ToppingModel = dr["Toppings"].ToString(),
                          Status = dr["Status"] != DBNull.Value ? Convert.ToBoolean(dr["Status"]) : true,
                          Price = dr["Price"] != DBNull.Value ? Convert.ToDouble(dr["Price"]) : 0,
                          DaysAvailable = dr["DaysAvailable"].ToString(),
                          MenuId = dr["MenuId"] != DBNull.Value ? Convert.ToInt32(dr["MenuId"]) : 0,
                          MenuStr = dr["MenuStr"].ToString(),
                          UrlSlug = dr["UrlSlug"].ToString(),
                          DaysAvailableOption = dr["DaysAvailableOption"].ToString()
                      }).ToList();
            return miList;
        }

        public List<RestCategory> GetAllCategory(Guid comId)
        {
            return _RestCategoryDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", comId));
        }
        
        public RestMenu GetMenuById(int Id)
        {
            return _RestMenuDataAccess.Get(Id);
        }

        public RestMenu GetMenuByMenuId(Guid Id)
        {
            return _RestMenuDataAccess.GetByQuery(string.Format("MenuId = '{0}'", Id)).FirstOrDefault();
        }

        public RestMenu GetMenuByIdAndCompanyId(int Id, Guid comid)
        {
            return _RestMenuDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and Id = {1}", comid, Id)).FirstOrDefault();
        }

        public RestMenuItem GetMenuItemById(int Id)
        {
            return _RestMenuItemDataAccess.Get(Id);
        }
        public List<RestMenuItem> GetAllMenuItemByMenuId(Guid menuId)
        {
            DataTable dt = _RestMenuDataAccess.GetAllMenuItemByMenuId(menuId);
            List<RestMenuItem> miList = new List<RestMenuItem>();
            miList = (from DataRow dr in dt.Rows
                          select new RestMenuItem()
                          {
                              Id = dr["MenuItemId"] != DBNull.Value ? Convert.ToInt32(dr["MenuItemId"]) : 0,
                              ItemName = dr["ItemName"].ToString(),
                              ItemNumber=dr["ItemNumber"].ToString(),
                              Photo=dr["Photo"].ToString(),
                              CategoryModel=dr["Categories"].ToString(),
                              ToppingModel=dr["Toppings"].ToString(),
                              Status=dr["Status"]!=DBNull.Value? Convert.ToBoolean(dr["Status"]) : true,
                              Price = dr["Price"] != DBNull.Value ? Convert.ToDouble(dr["Price"]) : 0,
                              DaysAvailable=dr["DaysAvailable"].ToString(),
                              UrlSlug = dr["UrlSlug"].ToString(),
                          }).ToList();
            return miList;
        }
        public List<RestCategory> GetAllCategoryByMenuId(Guid menuId, Guid itemid, Guid comid)
        {
            DataTable dt = _RestCategoryDataAccess.GetAllCategoryByMenuId(menuId, itemid, comid);
            List<RestCategory> cList = new List<RestCategory>();
            cList = (from DataRow dr in dt.Rows
                      select new RestCategory()
                      {
                          Id = dr["CategoryId"] != DBNull.Value ? Convert.ToInt32(dr["CategoryId"]) : 0,
                          CategoryName = dr["CategoryName"].ToString()
                      }).ToList();
            return cList;
        }
        public List<RestToppingCategory> GetAllToppingByMenuId(Guid menuId, Guid itemid, Guid comid)
        {
            DataTable dt = _ToppingCategoryDataAccess.GetAllToppingCategoryByMenuId(menuId, itemid, comid);
            List<RestToppingCategory> tcList = new List<RestToppingCategory>();
            tcList = (from DataRow dr in dt.Rows
                     select new RestToppingCategory()
                     {
                         Id = dr["ToppingCategoryId"] != DBNull.Value ? Convert.ToInt32(dr["ToppingCategoryId"]) : 0,
                         ToppingCategory = dr["CategoryName"].ToString()
                     }).ToList();
            return tcList;
        }
        public List<RestMenuItem> GetMenuItemListBySearchKey(string key, int MaxLoad, string ExistItem)
        {
            DataTable dt = _RestMenuDataAccess.GetMenuItemListBySearchKey(key, MaxLoad, ExistItem);
            List<RestMenuItem> miList = new List<RestMenuItem>();
            miList = (from DataRow dr in dt.Rows
                       select new RestMenuItem()
                       {
                           Id = dr["MenuItemId"] != DBNull.Value ? Convert.ToInt32(dr["MenuItemId"]) : 0,
                           ItemName = dr["ItemName"].ToString()
                       }).ToList();
            return miList;
        }
        public int InsertMenu(RestMenu menu)
        {
            return (int)_RestMenuDataAccess.Insert(menu);
        }
        public int InsertMenuItem(RestMenuItem menuItem)
        {
            return (int)_RestMenuItemDataAccess.Insert(menuItem);
        }
        public long InsertMenuDetail(RestMenuItemCategoryTopping md)
        {
            return _RestMenuItemCategoryToppingDataAccess.Insert(md);
        }
        public long InsertMenuItemDetail(RestMenuItemCategory mid)
        {
            return _RestMenuItemCategoryDataAccess.Insert(mid);
        }
        public bool UpdateMenu(RestMenu menu)
        {
            return _RestMenuDataAccess.Update(menu) > 0;
        }
        public bool UpdateMenuItem(RestMenuItem menuItem)
        {
            return _RestMenuItemDataAccess.Update(menuItem) > 0;
        }
        public bool DeleteAllMenuDetailByMenuId(Guid menuId)
        {
            return _RestMenuDataAccess.DeleteByMenuId(menuId);
        }

        public bool DeleteAllMenuDetailByMenuIdAndCategoryId(Guid menuId, Guid catid)
        {
            return _RestMenuDataAccess.DeleteByMenuIdAndCategoryId(menuId, catid);
        }

        public bool DeleteAllMenuDetailByCategoryId(int categoryId)
        {
            return _RestMenuDataAccess.DeleteByCategoryId(categoryId);
        }
        public bool DeleteAllMenuItemDetailByMenuItemId(Guid menuItemId)
        {
            return _MenuItemDetailDataAccess.DeleteByMenuItemId(menuItemId);
        }
        public bool DeleteMenuItemById(int id)
        {
            return _RestMenuItemDataAccess.Delete(id) > 0;
        }
        public bool DeleteMenuById(int id)
        {
            return _RestMenuDataAccess.Delete(id) > 0;
        }
        public WebsiteConfiguration GetWebsiteConfigurationById(int value)
        {
            return _WebsiteConfigurationDataAccess.Get(value);
        }

        public long InsertWebsiteConfiguration(WebsiteConfiguration web)
        {
            return _WebsiteConfigurationDataAccess.Insert(web);
        }

        public bool UpdateWebsiteConfiguration(WebsiteConfiguration web)
        {
            return _WebsiteConfigurationDataAccess.Update(web) > 0;
        }

        public List<WebsiteConfiguration> GetAllSiteConfig()
        {
            return _WebsiteConfigurationDataAccess.GetAll();
        }

        public List<WebsiteConfiguration> GetAllWebsiteConfig(Guid comid, int pageno, int pagesize, string searchtext, string order)
        {
            DataTable dt = _RestMenuDataAccess.GetAllWebsiteConfig(comid, pageno, pagesize, searchtext, order);
            List<WebsiteConfiguration> miList = new List<WebsiteConfiguration>();
            miList = (from DataRow dr in dt.Rows
                      select new WebsiteConfiguration()
                      {
                          Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                          SiteName = dr["SiteName"].ToString(),
                          DomainName = dr["DomainName"].ToString(),
                          Phone = dr["Phone"].ToString(),
                          IsEmail = dr["IsEmail"] != DBNull.Value ? Convert.ToBoolean(dr["IsEmail"]) : false,
                          ThemeLoc = dr["ThemeLoc"].ToString(),
                          CreatedBy = (Guid)dr["CreatedBy"],
                          CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                      }).ToList();
            return miList;
        }

        public WebsiteLocation GetSiteLocationById(int value)
        {
            return _WebsiteLocationDataAccess.Get(value);
        }

        public long InsertWebsiteLocation(WebsiteLocation wl)
        {
            return _WebsiteLocationDataAccess.Insert(wl);
        }

        public bool UpdateWebsiteLocation(WebsiteLocation wl)
        
        {
            return _WebsiteLocationDataAccess.Update(wl) > 0;
        }

        public List<WebsiteLocation> GetAllWebsiteLocation(Guid comid, int pageno, int pagesize, string searchtext, string order)
        {
            DataTable dt = _RestMenuDataAccess.GetAllWebsiteLocation(comid, pageno, pagesize, searchtext, order);
            List<WebsiteLocation> miList = new List<WebsiteLocation>();
            miList = (from DataRow dr in dt.Rows
                      select new WebsiteLocation()
                      {
                          Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                          CompanyId = (Guid)dr["CompanyId"],
                          Name = dr["Name"].ToString(),
                          Address = dr["Address"].ToString(),
                          Address2 = dr["Address2"].ToString(),
                          City = dr["City"].ToString(),
                          State = dr["State"].ToString(),
                          Zipcode = dr["Zipcode"].ToString(),
                          PrimaryContact = dr["PrimaryContact"].ToString(),
                          DomainName = dr["DomainName"].ToString(),
                          StorePhone = dr["StorePhone"].ToString(),
                          TrackingPhonePhone = dr["TrackingPhonePhone"].ToString(),
                          CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                          CreatedBy = (Guid)dr["CreatedBy"],
                          IsDefault = dr["IsDefault"] != DBNull.Value ? Convert.ToBoolean(dr["IsDefault"]) : false,
                      }).ToList();
            return miList;
        }

        public List<WebsiteLocation> GetAllWebLoc()
        {
            return _WebsiteLocationDataAccess.GetAll();
        }

        public List<WebsiteLocation> GetAllSiteLoc(Guid comid, string searchtext)
        {
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                return _WebsiteLocationDataAccess.GetByQuery(string.Format("(CompanyId = '{0}' or ReferCompanyId = '{0}') and Name like '%{1}%'", comid, searchtext)).ToList();
            }
            else
            {
                return _WebsiteLocationDataAccess.GetByQuery(string.Format("(CompanyId = '{0}' or ReferCompanyId = '{0}')", comid, searchtext)).ToList();
            }
            
        }

        public Seo GetSeoById(int value)
        {
            return _SeoDataAccess.Get(value);
        }

        public Seo GetSeoByPageURL(Guid comid, string url)
        {
            return _SeoDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and PageUrl = '{1}'", comid, url)).FirstOrDefault();
        }

        public bool UpdateSiteContent(Seo seo)
        {
            return _SeoDataAccess.Update(seo) > 0;
        }

        public long InsertSiteContent(Seo seo)
        {
            return _SeoDataAccess.Insert(seo);
        }

        public List<Seo> GetAllSeo()
        {
            return _SeoDataAccess.GetAll();
        }

        public List<Seo> GetAllSiteContent(Guid comid, int pageno, int pagesize, string searchtext, string order)
        {
            DataTable dt = _RestMenuDataAccess.GetAllSiteContent(comid, pageno, pagesize, searchtext, order);
            List<Seo> miList = new List<Seo>();
            miList = (from DataRow dr in dt.Rows
                      select new Seo()
                      {
                          Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                          CompanyId = (Guid)dr["CompanyId"],
                          Name = dr["Name"].ToString(),
                          PageUrl = dr["PageUrl"].ToString(),
                          MetaTitle = dr["MetaTitle"].ToString(),
                          MetaDescription = dr["MetaDescription"].ToString(),
                          MetaKeywords = dr["MetaKeywords"].ToString(),
                          OgTitle = dr["OgTitle"].ToString(),
                          OgDescription = dr["OgDescription"].ToString(),
                          FolderOption = dr["FolderOption"].ToString(),
                          IsNav = dr["IsNav"] != DBNull.Value ? Convert.ToBoolean(dr["IsNav"]) : false,
                          PublishOption = dr["PublishOption"].ToString()
                      }).ToList();
            return miList;
        }

        public bool DeleteSiteContent(int value)
        {
            return _SeoDataAccess.Delete(value) > 0;
        }

        public bool DeleteSiteLocation(int value)
        {
            return _WebsiteLocationDataAccess.Delete(value) > 0;
        }

        public bool DeleteSiteConfiguration(int value)
        {
            return _WebsiteConfigurationDataAccess.Delete(value) > 0;
        }

        public SiteContact GetSiteContactById(int value)
        {
            return _SiteContactDataAccess.Get(value);
        }

        public long InsertSiteContact(SiteContact contact)
        {
            return _SiteContactDataAccess.Insert(contact);
        }

        public bool UpdateSiteContact(SiteContact contact)
        {
            return _SiteContactDataAccess.Update(contact) > 0;
        }

        public List<SiteContact> GetAllSiteContactList(Guid comid, int pageno, int pagesize, string searchtext, string order)
        {
            DataTable dt = _RestMenuDataAccess.GetAllSiteContactList(comid, pageno, pagesize, searchtext, order);
            List<SiteContact> miList = new List<SiteContact>();
            miList = (from DataRow dr in dt.Rows
                      select new SiteContact()
                      {
                          Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                          FirstName = dr["FirstName"].ToString(),
                          LastName = dr["LastName"].ToString(),
                          CellPhone = dr["CellPhone"].ToString(),
                          WorkPhone = dr["WorkPhone"].ToString(),
                          Email = dr["Email"].ToString(),
                          IsEmail = dr["IsEmail"] != DBNull.Value ? Convert.ToBoolean(dr["IsEmail"]) : false,
                          IsText = dr["IsText"] != DBNull.Value ? Convert.ToBoolean(dr["IsText"]) : false,
                      }).ToList();
            return miList;
        }

        public List<SiteContact> GetAllSiteContact(Guid comid, string searchtext)
        {
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                return _SiteContactDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and (FirstName like '%{1}%' or LastName like '%{1}%' or FirstName + ' ' + LastName like '%{1}%')", comid, searchtext)).ToList();
            }
            else
            {
                return _SiteContactDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", comid, searchtext)).ToList();
            }
        }

        public bool DeleteSiteContact(int value)
        {
            return _SiteContactDataAccess.Delete(value) > 0;
        }

        public MenuListModel GetAllMenuItemList(Guid comid, int pageNo, int pageSize, string searchText, string order)
        {
            MenuListModel Model = new MenuListModel();
            DataSet ds = _RestMenuDataAccess.GetAllMenuItemList(comid, pageNo, pageSize, searchText, order);

            Model.Menus = (from DataRow dr in ds.Tables[0].Rows
                           select new RestMenu()
                           {
                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                               MenuName = dr["MenuName"].ToString(),
                               DaysAvailable = dr["DaysAvailable"].ToString(),
                               Status = Convert.ToBoolean(dr["Status"]),
                               ItemPhoto = dr["ItemPhoto"].ToString(),
                               ItemName = dr["ItemName"].ToString(),
                               CategoryName = dr["CategoryName"].ToString(),
                               ItemPrice = dr["ItemPrice"].ToString(),
                           }).ToList();

            Model.TotalCount = ds.Tables[1].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCount"]) : 0;
            Model.PageNo = pageNo;
            Model.PageSize = pageSize;
            Model.Searchtext = searchText;
            return Model;
        }

        public List<RestMenu> GetMenuListByMenuIdAndCompanyId(Guid comid, int id)
        {
            return _RestMenuDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and Id = {1}", comid, id)).ToList();
        }

        public List<MenuItem> GetMenuItemListByMenuIdAndCompanyId(Guid comid, int id)
        {
            DataTable dt = _RestMenuDataAccess.GetMenuItemListByMenuIdAndCompanyId(comid, id);
            List<MenuItem> miList = new List<MenuItem>();
            miList = (from DataRow dr in dt.Rows
                      select new MenuItem()
                      {
                          Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                          ItemName = dr["ItemName"].ToString(),
                          ItemNumber = dr["ItemNumber"].ToString(),
                          ItemLevel = dr["ItemLevel"].ToString(),
                          Description = dr["Description"].ToString(),
                          Photo = dr["Photo"].ToString(),
                          MaxQty = dr["MaxQty"] != DBNull.Value ? Convert.ToInt32(dr["MaxQty"]) : 0,
                          DaysAvailable = dr["DaysAvailable"].ToString(),
                          TimeAvailable = dr["TimeAvailable"].ToString(),
                          Price = dr["Price"] != DBNull.Value ? Convert.ToDouble(dr["Price"]) : 0,
                          Status = dr["Status"] != DBNull.Value ? Convert.ToBoolean(dr["Status"]) : false,
                          DaysAvailableOption = dr["DaysAvailableOption"].ToString(),
                          TimeAvailableOption = dr["TimeAvailableOption"].ToString(),
                          MenuId = dr["MenuId"] != DBNull.Value ? Convert.ToInt32(dr["MenuId"]) : 0,
                      }).ToList();
            return miList;
        }

        public List<Category> GetCategoryListByMenuIdAndCompanyId(Guid comid, int id)
        {
            DataTable dt = _RestMenuDataAccess.GetCategoryListByMenuIdAndCompanyId(comid, id);
            List<Category> miList = new List<Category>();
            miList = (from DataRow dr in dt.Rows
                      select new Category()
                      {
                          Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                          CategoryName = dr["CategoryName"].ToString(),
                          Description = dr["Description"].ToString(),
                          DaysAvailable = dr["DaysAvailable"].ToString(),
                          TimeAvailable = dr["TimeAvailable"].ToString(),
                          Status = dr["Status"] != DBNull.Value ? Convert.ToBoolean(dr["Status"]) : false,
                          Image = dr["Image"].ToString(),
                      }).ToList();
            return miList;
        }

        public List<ToppingCategory> GetToppingCategoryListByMenuIdAndCompanyId(Guid comid, int id)
        {
            DataTable dt = _RestMenuDataAccess.GetToppingCategoryListByMenuIdAndCompanyId(comid, id);
            List<ToppingCategory> miList = new List<ToppingCategory>();
            miList = (from DataRow dr in dt.Rows
                      select new ToppingCategory()
                      {
                          Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                          ToppingCategory = dr["ToppingCategory"].ToString(),
                          ToppingName = dr["ToppingName"].ToString(),
                          ToppingPrice = dr["ToppingPrice"].ToString()
                      }).ToList();
            return miList;
        }

        public bool UpdateSiteLocationOperation(WebsiteLocationOperation wlo)
        {
            return _WebsiteLocationOperationDataAccess.Update(wlo) > 0;
        }

        public long InsertSiteLocationOperation(WebsiteLocationOperation wlo)
        {
            return _WebsiteLocationOperationDataAccess.Insert(wlo);
        }

        public WebsiteLocationOperation GetWebsiteLocationOperationById(int value)
        {
            return _WebsiteLocationOperationDataAccess.Get(value);
        }

        public WebsiteLocationOperation GetWebsiteLocationOperationBySiteIdAndDayAndCompanyId(int siteid, string day, Guid comid)
        {
            return _WebsiteLocationOperationDataAccess.GetByQuery(string.Format("SiteLocationId = {0} and HoursofOperation like '%{1}%' and CompanyId = '{2}'", siteid, day, comid)).FirstOrDefault();
        }

        public List<WebsiteLocationOperation> GetAllWebsiteLocationOperationByLocationIdAndCompanyId(Guid comid, int locid, string type)
        {
            DataTable dt = _WebsiteLocationDataAccess.GetAllWebsiteLocationOperationByLocationIdAndCompanyId(comid, locid, type);
            List<WebsiteLocationOperation> miList = new List<WebsiteLocationOperation>();
            miList = (from DataRow dr in dt.Rows
                      select new WebsiteLocationOperation()
                      {
                          Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                          CompanyId = (Guid)dr["CompanyId"],
                          SiteLocationId = dr["SiteLocationId"] != DBNull.Value ? Convert.ToInt32(dr["SiteLocationId"]) : 0,
                          HoursofOperation = dr["HoursofOperation"].ToString(),
                          OperationStartTime = dr["OperationStartTime"].ToString(),
                          OperationEndTime = dr["OperationEndTime"].ToString(),
                          StoreOperationStartTime = dr["StoreOperationStartTime"].ToString(),
                          StoreOperationEndTime = dr["StoreOperationEndTime"].ToString(),
                          OperationStartTimeVal = dr["OperationStartTimeVal"].ToString(),
                          OperationEndTimeVal = dr["OperationEndTimeVal"].ToString(),
                          IsAdditional = dr["IsAdditional"] != DBNull.Value ? Convert.ToBoolean(dr["IsAdditional"]) : false,
                          StoreOperationStartTimeVal = dr["StoreOperationStartTimeVal"].ToString(),
                          StoreOperationEndTimeVal = dr["StoreOperationEndTimeVal"].ToString(),
                      }).ToList();
            return miList;
        }

        public bool DeleteWebsiteLocationOperation(int value)
        {
            return _WebsiteLocationOperationDataAccess.Delete(value) > 0;
        }

        public WebsiteLocation GetWebsiteLocationByCompanyId(Guid comid)
        {
            return _WebsiteLocationDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", comid)).FirstOrDefault();
        }

        public WebsiteLocation GetWebLocById(int value)
        {
            return _WebsiteLocationDataAccess.Get(value);
        }

        public List<WebsiteLocation> GetWebsiteLocationByCompanyIdAndStateAndCity(string state, string city, string address, string zip, string urlslug)
        {
            return _WebsiteLocationDataAccess.GetByQuery(string.Format("Address = '{3}' and ZipCode = '{4}' and [State] = '{1}' and City = '{2}'", state, city, address, zip, urlslug)).ToList();
        }

        public List<WebsiteLocation> GetWebsiteLocationByUrlSlug(string urlslug)
        {
            return _WebsiteLocationDataAccess.GetByQuery(string.Format("WebsiteURL = '{0}'", urlslug)).ToList();
        }

        public WebsiteLocation GetWebsiteLocationBySlug(string urlslug)
        {
            return _WebsiteLocationDataAccess.GetByQuery(string.Format("UrlSlug = '{0}'", urlslug)).FirstOrDefault();
        }

        public Menu GetMenuByCompanyIdAndMenuItemId(Guid comid, int id)
        {
            DataTable dt = _RestMenuDataAccess.GetMenuByCompanyIdAndMenuItemId(comid, id);
            Menu miList = new Menu();
            miList = (from DataRow dr in dt.Rows
                      select new Menu()
                      {
                          Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                          CompanyId = (Guid)dr["CompanyId"],
                          MenuName = dr["MenuName"].ToString(),
                          UrlSlug = dr["UrlSlug"].ToString()
                      }).FirstOrDefault();
            return miList;
        }

        public RestCategory GetCategoryByCompanyIdAndMenuId(Guid comid, int id)
        {
            DataTable dt = _RestMenuDataAccess.GetCategoryByCompanyIdAndMenuId(comid, id);
            RestCategory miList = new RestCategory();
            miList = (from DataRow dr in dt.Rows
                      select new RestCategory()
                      {
                          Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                          CompanyId = (Guid)dr["CompanyId"],
                          UrlSlug = dr["UrlSlug"].ToString()
                      }).FirstOrDefault();
            return miList;
        }

        public Category GetCategoryByCompanyIdAndMenuIdAndMenuItemId(Guid comid, int id, Guid itemid)
        {
            DataTable dt = _RestMenuDataAccess.GetCategoryByCompanyIdAndMenuIdAndMenuItemId(comid, id, itemid);
            Category miList = new Category();
            miList = (from DataRow dr in dt.Rows
                      select new Category()
                      {
                          Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                          CompanyId = (Guid)dr["CompanyId"],
                          UrlSlug = dr["UrlSlug"].ToString()
                      }).FirstOrDefault();
            return miList;
        }

        public long InsertMenuItemCategoryURL(RestMenuItemCategoryURL micu)
        {
            return _RestMenuItemCategoryURLDataAccess.Insert(micu);
        }

        

        public bool DeleteResturantLiveDB(Guid comid)
        {
            return _RestMenuDataAccess.DeleteResturantLiveDB(comid);
        }

        public long InsertMenuItemCategory(RestMenuCategory rmc)
        {
            return _RestMenuCategoryDataAccess.Insert(rmc);
        }

        public bool DeleteMenuItemAndRelation(Guid itemid)
        {
            return _RestMenuDataAccess.DeleteMenuItemAndRelation(itemid);
        }

        public bool DeleteMenuRelation(Guid menuid)
        {
            return _RestMenuDataAccess.DeleteMenuRelation(menuid);
        }

        public RestCategory GetCategoryById(int value)
        {
            return _RestCategoryDataAccess.Get(value);
        }

        public RestMenuItemCategory GetMenuItemCatgoryByCategoryId(Guid catid)
        {
            return _RestMenuItemCategoryDataAccess.GetByQuery(string.Format("CategoryId = '{0}'", catid)).FirstOrDefault();
        }

        public RestMenuItemCategoryTopping GetMenuItemCatgoryToppingByCategoryId(Guid catid)
        {
            return _RestMenuItemCategoryToppingDataAccess.GetByQuery(string.Format("ToppingCategoryId = '{0}'", catid)).FirstOrDefault();
        }

        public RestMenuCategory GetMenuCategoryByMenuIdAndCategoryId(Guid menuid, Guid categoryid)
        {
            return _RestMenuCategoryDataAccess.GetByQuery(string.Format("MenuId = '{0}' and CategoryId = '{1}'", menuid, categoryid)).FirstOrDefault();
        }

        public bool DeleteAllWebLocOptByCompanyIdAndSiteLocId(Guid comid, int sitelocid, string day)
        {
            return _RestMenuDataAccess.DeleteAllWebLocOptByCompanyIdAndSiteLocId(comid, sitelocid, day);
        }

        public ItemReportsModel GetItemsReportByCompanyId(Guid comid)
        {
            DataTable dt = _RestMenuDataAccess.GetItemsReportByCompanyId(comid);
            ItemReportsModel miList = new ItemReportsModel();
            miList = (from DataRow dr in dt.Rows
                      select new ItemReportsModel()
                      {
                          TotalActive = dr["TotalActive"] != DBNull.Value ? Convert.ToInt32(dr["TotalActive"]) : 0,
                          TotalItems = dr["TotalItems"] != DBNull.Value ? Convert.ToInt32(dr["TotalItems"]) : 0,
                          AveragePrice = dr["AveragePrice"] != DBNull.Value ? Convert.ToDouble(dr["AveragePrice"]) : 0.0,
                      }).FirstOrDefault();
            return miList;
        }

        public List<RestToppingCategory> GetAllToppingCategoryByCompanyId(Guid comid)
        {
            return _RestToppingCategoryDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", comid)).ToList();
        }

        public List<ResturantOrder> GetAllOrdersByCompanyId(Guid comid, string startdate, string enddate, Guid customerid, bool filter, string ordertype, string orderstatus)
        {
            string datequery = "";
            string Customerquery = "";
            string cancelQuery = "";
            if (!string.IsNullOrWhiteSpace(ordertype))
            {
                ordertype = string.Format("and OrderType = '{0}'", ordertype);
            }
            if (!string.IsNullOrWhiteSpace(orderstatus))
            {
                if (orderstatus.ToLower() == "progress")
                {
                    if (!string.IsNullOrWhiteSpace(ordertype) && ordertype.ToLower() == "pickup")
                    {
                        orderstatus = string.Format("and [Status] in ('Pending','Accepted','Readypick')", orderstatus);
                    }
                    else
                    {
                        orderstatus = string.Format("and [Status] in ('Pending','Accepted','Readydeliver')", orderstatus);
                    }
                }
                else
                {
                    orderstatus = string.Format("and [Status] in ('{0}')", orderstatus);
                }
            }
            if (customerid != new Guid())
            {
                Customerquery = string.Format("and CustomerId = '{0}'", customerid);
            }
            if (!string.IsNullOrWhiteSpace(startdate) && !string.IsNullOrWhiteSpace(enddate) && startdate != "01/01/0001" && enddate != "01/01/0001")
            {
                startdate = Convert.ToDateTime(startdate).ToString("yyyy-MM-dd 00:00:00.000");
                enddate = Convert.ToDateTime(enddate).ToString("yyyy-MM-dd 23:59:59.999");
                datequery = string.Format("and CreatedDate between (SELECT DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), '{0}')) and (SELECT DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), '{1}'))", startdate, enddate);
            }
            if(filter == true)
            {
                cancelQuery = "and IsDeleted = 1";
            }
            else
            {
                cancelQuery = "and IsDeleted != 1";
            }
            return _ResturantOrderDataAccess.GetByQuery(string.Format("CompanyId = '{0}' {1} {2} {4} {5}", comid, datequery, Customerquery, cancelQuery, ordertype, orderstatus)).ToList();
        }

        public List<ResturantOrder> GetAllNewOrdersByCompanyId(Guid comid, string startdate, string enddate)
        {
            string datequery = "";
            if (!string.IsNullOrWhiteSpace(startdate) && !string.IsNullOrWhiteSpace(enddate) && startdate != "01/01/0001" && enddate != "01/01/0001")
            {
                startdate = Convert.ToDateTime(startdate).ToString("yyyy-MM-dd 00:00:00.000");
                enddate = Convert.ToDateTime(enddate).ToString("yyyy-MM-dd 23:59:59.999");
                datequery = string.Format("and CreatedDate between (SELECT DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), '{0}')) and (SELECT DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), '{1}'))", startdate, enddate);
            }
            return _ResturantOrderDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and (IsViewed = 0 or IsViewed = '' or IsViewed is null) {1}", comid, datequery)).ToList();
        }

        public ResturantOrder GetOrderById(int value)
        {
            return _ResturantOrderDataAccess.Get(value);
        }

        public ResturantOrderDetail GetOrderDetailById(int value)
        {
            return _ResturantOrderDetailDataAccess.Get(value);
        }

        public bool UpdateOrder(ResturantOrder order)
        {
            return _ResturantOrderDataAccess.Update(order) > 0;
        }

        public SocialMediaContent GetMediaContentById(int value)
        {
            return _SocialMediaContentDataAccess.Get(value);
        }

        public SocialMediaContent GetMediaContentByCompanyIdAndName(Guid comid, string name)
        {
            return _SocialMediaContentDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and Name = '{1}'", comid, name)).FirstOrDefault();
        }

        public List<SocialMediaContent> GetAllMediaContentByCompanyId(Guid comid)
        {
            return _SocialMediaContentDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", comid));
        }

        public long InsertSocialMedia(SocialMediaContent smc)
        {
            return _SocialMediaContentDataAccess.Insert(smc);
        }

        public bool UpdateSocialMedia(SocialMediaContent smc)
        {
            return _SocialMediaContentDataAccess.Update(smc) > 0;
        }

        public bool DeleteSocialMedia(int value)
        {
            return _SocialMediaContentDataAccess.Delete(value) > 0;
        }

        public List<RestMenuItemCategory> GetAllMenuItemCategoryByMenuId(Guid menuid)
        {
            return _RestMenuItemCategoryDataAccess.GetByQuery(string.Format("MenuId = '{0}'", menuid)).ToList();
        }

        public List<RestMenuItem> GetMenuItemByUrlSlug(string urlslug, Guid comid)
        {
            return _RestMenuItemDataAccess.GetByQuery(string.Format("UrlSlug = '{0}' and CompanyId = '{1}'", urlslug, comid)).ToList();
        }

        public List<RestMenu> GetMenuByUrlSlug(string urlslug, Guid comid)
        {
            return _RestMenuDataAccess.GetByQuery(string.Format("UrlSlug = '{0}' and CompanyId = '{1}'", urlslug, comid)).ToList();
        }

        public List<RestCategory> GetCategoryByUrlSlug(string urlslug, Guid comid)
        {
            return _RestCategoryDataAccess.GetByQuery(string.Format("UrlSlug = '{0}' and CompanyId = '{1}'", urlslug, comid)).ToList();
        }

        public List<OrderSummeryDataModel> GetRestaurantOrderSummeryByCompanyId(Guid comid, string startdate, string enddate, string ordertype, string orderstatus)
        {
            DataTable dt = _ResturantOrderDataAccess.GetRestaurantOrderSummeryByCompanyId(comid, startdate, enddate, ordertype, orderstatus);
            List<OrderSummeryDataModel> miList = new List<OrderSummeryDataModel>();
            miList = (from DataRow dr in dt.Rows
                      select new OrderSummeryDataModel()
                      {
                          OrderType = dr["OrderType"].ToString(),
                          QuantityCount = dr["QuantityCount"] != DBNull.Value ? Convert.ToInt32(dr["QuantityCount"]) : 0,
                          InProgressCount = dr["InProgressCount"] != DBNull.Value ? Convert.ToInt32(dr["InProgressCount"]) : 0,
                          RejectedCount = dr["RejectedCount"] != DBNull.Value ? Convert.ToInt32(dr["RejectedCount"]) : 0,
                          CompletedCount = dr["CompletedCount"] != DBNull.Value ? Convert.ToInt32(dr["CompletedCount"]) : 0,
                          AverageOrder = dr["AverageOrder"] != DBNull.Value ? Convert.ToDouble(dr["AverageOrder"]) : 0,
                          CancellationCount = dr["CancellationCount"] != DBNull.Value ? Convert.ToInt32(dr["CancellationCount"]) : 0,
                          TotalRev = dr["TotalRev"] != DBNull.Value ? Convert.ToDouble(dr["TotalRev"]) : 0,
                      }).ToList();
            return miList;
        }

        public OrderSummeryDataModel GetRestaurantOrderSummeryByCompanyIdAndCustomerId(Guid comid, Guid cusid)
        {
            DataTable dt = _ResturantOrderDataAccess.GetRestaurantOrderSummeryByCompanyIdAndCustomerId(comid, cusid);
            OrderSummeryDataModel miList = new OrderSummeryDataModel();
            miList = (from DataRow dr in dt.Rows
                      select new OrderSummeryDataModel()
                      {
                          QuantityCount = dr["QuantityCount"] != DBNull.Value ? Convert.ToInt32(dr["QuantityCount"]) : 0,
                          TotalRev = dr["TotalRev"] != DBNull.Value ? Convert.ToDouble(dr["TotalRev"]) : 0,
                      }).FirstOrDefault();
            return miList;
        }

        public long InsertAdditionalContent(RestMenuItemAdditionalContent riac)
        {
            return _RestMenuItemAdditionalContentDataAccess.Insert(riac);
        }

        public List<RestMenuItemAdditionalContent> GetAllAdditionalContentByItemId(Guid itemid)
        {
            return _RestMenuItemAdditionalContentDataAccess.GetByQuery(string.Format("ItemId = '{0}'", itemid)).ToList();
        }

        public bool DeleteAllAdditionalContentByItemId(Guid itemid)
        {
            return _RestMenuDataAccess.DeleteAllAdditionalContentByItemId(itemid);
        }

        public List<ResturantOrder> GetAllOrderByExpirationTime(string exptime)
        {
            return _ResturantOrderDataAccess.GetByQuery(string.Format("[Status] = 'Pending' and (OrderDate <= '{0}' or AcceptDate <= '{0}')", exptime)).ToList();
        }

        public ResturantSystemSetting GetResturantSystemSettingByCompanyId(Guid comid)
        {
            return _ResturantSystemSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", comid)).FirstOrDefault();
        }

        public ResturantSystemSetting GetResturantSystemSettingById(int value)
        {
            return _ResturantSystemSettingDataAccess.Get(value);
        }

        public bool UpdateResturantSystemSetting(ResturantSystemSetting rss)
        {
            return _ResturantSystemSettingDataAccess.Update(rss) > 0;
        }

        public bool UpdateOrderDetail(ResturantOrderDetail rod)
        {
            return _ResturantOrderDetailDataAccess.Update(rod) > 0;
        }

        public long InsertResturantNeighbarhood(ResturantNeighborhood rn)
        {
            return _ResturantNeighborhoodDataAccess.Insert(rn);
        }

        public bool DeleteResturantNeighbarhoodByLocationId(int value)
        {
            return _RestMenuDataAccess.DeleteResturantNeighbarhoodByLocationId(value);
        }

        public List<ResturantNeighborhood> GetAllNeighbarhoodByLocationId(int value)
        {
            return _ResturantNeighborhoodDataAccess.GetByQuery(string.Format("SiteLocationId = {0}", value)).ToList();
        }

        public List<ResturantNeighborhood> GetAllNeighborhood()
        {
            return _ResturantNeighborhoodDataAccess.GetAll();
        }

        public List<ResturantNeighborhood> GetAllNeighbarhoodByKey(string key)
        {
            DataTable dt = _WebsiteLocationDataAccess.GetAllNeighbarhoodByKey(key);
            List<ResturantNeighborhood> miList = new List<ResturantNeighborhood>();
            miList = (from DataRow dr in dt.Rows
                      select new ResturantNeighborhood()
                      {
                          NeighborhoodName = dr["NeighborhoodName"].ToString(),
                          NeighborhoodURL = dr["NeighborhoodURL"].ToString()
                      }).ToList();
            return miList;
        }

        public ResturantSystemSetting GetAllResturantSystemSettingByCompanyId(Guid comid)
        {
            DataTable dt = _ResturantOrderDataAccess.GetAllResturantSystemSettingByCompanyId(comid);
            ResturantSystemSetting miList = new ResturantSystemSetting();
            miList = (from DataRow dr in dt.Rows
                      select new ResturantSystemSetting()
                      {
                          Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                          CompanyId = (Guid)dr["CompanyId"],
                          Restaurant = dr["Restaurant"].ToString(),
                          Logo = dr["Logo"].ToString(),
                          TaxRate = dr["TaxRate"] != DBNull.Value ? Convert.ToDouble(dr["TaxRate"]) : 0,
                          AuthApiLoginKey = dr["AuthApiLoginKey"].ToString(),
                          AuthApiTransactionKey = dr["AuthApiTransactionKey"].ToString(),
                          PrimaryContact = dr["PrimaryContactVal"].ToString(),
                      }).FirstOrDefault();
            return miList;
        }

        public List<WebsiteLocation> GetAllWebLocWithNoItems()
        {
            DataTable dt = _WebsiteLocationDataAccess.GetAllWebLocWithNoItems();
            List<WebsiteLocation> miList = new List<WebsiteLocation>();
            miList = (from DataRow dr in dt.Rows
                      select new WebsiteLocation()
                      {
                          CompanyId = (Guid)dr["CompanyId"],
                          Name = dr["Name"].ToString()
                      }).ToList();
            return miList;
        }

        public bool CloneRestaurantMenuByCompanyId(Guid comid, Guid oldcomid, Guid userid)
        {
            return _RestMenuDataAccess.CloneRestaurantMenuByCompanyId(comid, oldcomid, userid);
        }

        public bool CloneRestaurantCategoryByCompanyId(Guid comid, Guid oldcomid, Guid userid)
        {
            return _RestMenuDataAccess.CloneRestaurantCategoryByCompanyId(comid, oldcomid, userid);
        }

        public bool CloneRestaurantToppingByCompanyId(Guid comid, Guid oldcomid, Guid userid)
        {
            return _RestMenuDataAccess.CloneRestaurantToppingByCompanyId(comid, oldcomid, userid);
        }

        public bool CloneRestaurantMenuItemByCompanyId(Guid comid, Guid oldcomid, Guid userid)
        {
            return _RestMenuDataAccess.CloneRestaurantMenuItemByCompanyId(comid, oldcomid, userid);
        }

        public bool DeleteAllItemsByCompanyId(Guid comid)
        {
            return _RestMenuDataAccess.DeleteAllItemsByCompanyId(comid);
        }

        public List<ResturantReview> GetAllReviewsByCompanyId(Guid comid)
        {
            return _ResturantReviewDataAccess.GetByQuery(string.Format("CompanyId = '{0}' order by Id desc", comid)).ToList();
        }

        public ResturantReview GetReviewById(int value)
        {
            return _ResturantReviewDataAccess.Get(value);
        }

        public bool UpdateReview(ResturantReview rr)
        {
            return _ResturantReviewDataAccess.Update(rr) > 0;
        }

        public List<ResturantReview> GetAllReviewListByCompanyId(Guid comid)
        {
            DataTable dt = _RestMenuDataAccess.GetAllReviewListByCompanyId(comid);
            List<ResturantReview> miList = new List<ResturantReview>();
            miList = (from DataRow dr in dt.Rows
                      select new ResturantReview()
                      {
                          Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                          CompanyId = (Guid)dr["CompanyId"],
                          Name = dr["Name"].ToString(),
                          Email = dr["Email"].ToString(),
                          Comments = dr["Comments"].ToString(),
                          Rating = dr["Rating"] != DBNull.Value ? Convert.ToDouble(dr["Rating"]) : 0,
                          CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : new DateTime(),
                          IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
                          Reply = dr["Reply"].ToString(),
                          ReviewFor = dr["ReviewFor"].ToString(),
                          ItemName = dr["ItemName"].ToString(),
                      }).ToList();
            return miList;
        }

        public List<Seo> GetAllSeoByCompanyIdAndFolderOPtion(Guid comid)
        {
            return _SeoDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and FolderOption = '-1'", comid)).ToList();
        }

        public bool DeleteLocationMapInfo(Guid comid)
        {
            return _RestMenuDataAccess.DeleteLocationMapInfoByCompanyId(comid);
        }

        public long InsertLoctionMapInfo(WebsiteLocationMapInfo mapinfo)
        {
            return _WebsiteLocationMapInfoDataAccess.Insert(mapinfo);
        }

        public TrackingNumberSetting GetTrackingNumberSettingById(int value)
        {
            return _TrackingNumberSettingDataAccess.Get(value);
        }

        public TrackingNumberSetting GetTrackingNumberSettingByCompanyId(Guid comid)
        {
            return _TrackingNumberSettingDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", comid)).FirstOrDefault();
        }

        public List<WebsiteLocation> GetAllDifferentLocationsByCompanyId(Guid comid)
        {
            return _WebsiteLocationDataAccess.GetByQuery(string.Format("(CompanyId = '{0}' or ReferCompanyId = '{0}')", comid)).ToList();
        }

        public long InsertTrackingNumberSetting(TrackingNumberSetting tns)
        {
            return _TrackingNumberSettingDataAccess.Insert(tns);
        }

        public bool UpdateTrackingNumberSetting(TrackingNumberSetting tns)
        {
            return _TrackingNumberSettingDataAccess.Update(tns) > 0;
        }

        public List<TrackingNumberSetting> GetAllTrackingNumbersByComapnyId(Guid comid)
        {
            DataTable dt = _WebsiteLocationDataAccess.GetAllTrackingNumbersByComapnyId(comid);
            List<TrackingNumberSetting> miList = new List<TrackingNumberSetting>();
            miList = (from DataRow dr in dt.Rows
                      select new TrackingNumberSetting()
                      {
                          Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                          CompanyId = (Guid)dr["CompanyId"],
                          TrackingNumber = dr["TrackingNumber"].ToString(),
                          CompanyName = dr["CompanyName"].ToString(),
                          Comments = dr["Comments"].ToString(),
                          IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
                          IsRecorded = dr["IsRecorded"] != DBNull.Value ? Convert.ToBoolean(dr["IsRecorded"]) : false,
                          SubAccountId = dr["SubAccountId"].ToString(),
                          ForwardingNumber = dr["ForwardingNumber"].ToString(),
                      }).ToList();
            return miList;
        }

        public bool DeleteTrackingNumber(int value)
        {
            return _TrackingNumberSettingDataAccess.Delete(value) > 0;
        }

        public List<TrackingNumberSetting> GetAllSearchTrackingNumberSettingsByKeyAndCompanyId(Guid comid, string key)
        {
            return _TrackingNumberSettingDataAccess.GetByQuery(string.Format("TrackingNumber like '%{0}%' and CompanyId = '{1}' and IsActive = 1", key, comid)).ToList();
        }

        public List<Customer> GetAllCustomersByCompanyId(Guid comid)
        {
            DataTable dt = _CustomerDataAccess.GetAllCustomersByCompanyId(comid);
            List<Customer> miList = new List<Customer>();
            miList = (from DataRow dr in dt.Rows
                      select new Customer()
                      {
                          CustomerId = (Guid)dr["CustomerId"],
                          FirstName = dr["FirstName"].ToString(),
                          LastName = dr["LastName"].ToString(),
                      }).ToList();
            return miList;
        }

        public Customer GetCustomerByCellNumber(string number)
        {
            return _CustomerDataAccess.GetByQuery(string.Format("REPLACE(CellNo,'-','') = '{0}'", number.Replace("-", ""))).FirstOrDefault();
        }

        public List<TrackingNumberRecorded> GetAllTrackingNumberRecorded(Guid comid, string mindate, string maxdate)
        {
            DataTable dt = _CustomerDataAccess.GetAllTrackingNumberRecorded(comid, mindate, maxdate);
            List<TrackingNumberRecorded> miList = new List<TrackingNumberRecorded>();
            miList = (from DataRow dr in dt.Rows
                      select new TrackingNumberRecorded()
                      {
                          Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                          TrackingId = (Guid)dr["TrackingId"],
                          CallerId = (Guid)dr["CallerId"],
                          CompanyId = (Guid)dr["CompanyId"],
                          TrackingNumber = dr["TrackingNumber"].ToString(),
                          CallerNumber = dr["CallerNumber"].ToString(),
                          Status = dr["Status"].ToString(),
                          RecordDate = dr["RecordDate"] != DBNull.Value ? Convert.ToDateTime(dr["RecordDate"]) : new DateTime(),
                          CustomerId = (Guid)dr["CustomerId"],
                          CallerName = dr["CallerName"].ToString(),
                          RecordFile = dr["RecordFile"].ToString(),
                          LocationName = dr["LocationName"].ToString(),
                          TalkTimeSeconds = dr["TalkTimeSeconds"].ToString(),
                      }).ToList();
            return miList;
        }

        public TrackingNumberRecorded GetTrackingNumberRecordedById(int value)
        {
            return _TrackingNumberRecordedDataAccess.Get(value);
        }

        public bool UpdateTrackingNumberRecorded(TrackingNumberRecorded tnr)
        {
            return _TrackingNumberRecordedDataAccess.Update(tnr) > 0;
        }

        public long InsertRestaurantRewards(RestaurantRewards rr)
        {
            return _RestaurantRewardsDataAccess.Insert(rr);
        }

        public bool UpdateRestaurantRewards(RestaurantRewards rr)
        {
            return _RestaurantRewardsDataAccess.Update(rr) > 0;
        }

        public RestaurantRewards GetRestaurantRewards(Guid comid)
        {
            return _RestaurantRewardsDataAccess.GetByQuery(string.Format("CompanyId = '{0}'", comid)).FirstOrDefault();
        }

        public List<RestaurantCoupons> GetAllRestaurantCoupons(Guid comid)
        {
            DataTable dt = _ResturantOrderDataAccess.GetAllRestaurantCoupons(comid);
            List<RestaurantCoupons> miList = new List<RestaurantCoupons>();
            miList = (from DataRow dr in dt.Rows
                      select new RestaurantCoupons()
                      {
                          Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                          CouponCode = dr["CouponCode"].ToString(),
                          StartDate = dr["StartDate"] != DBNull.Value ? Convert.ToDateTime(dr["StartDate"]) : new DateTime(),
                          EndDate = dr["EndDate"] != DBNull.Value ? Convert.ToDateTime(dr["EndDate"]) : new DateTime(),
                          Discount = dr["Discount"].ToString(),
                          MinimumOrder = dr["MinimumOrder"].ToString(),
                          ReedemRequired = dr["ReedemRequired"].ToString(),
                          DiscountType = dr["DiscountType"].ToString(),
                          Status = dr["Status"] != DBNull.Value ? Convert.ToBoolean(dr["Status"]) : false,
                          CompanyId = (Guid)dr["CompanyId"],
                          CountRedeem = dr["CountRedeem"] != DBNull.Value ? Convert.ToInt32(dr["CountRedeem"]) : 0,
                      }).ToList();
            return miList;
        }

        public RestaurantCoupons GetCouponById(int value)
        {
            return _RestaurantCouponsDataAccess.Get(value);
        }

        public long InsertCoupons(RestaurantCoupons rc)
        {
            return _RestaurantCouponsDataAccess.Insert(rc);
        }

        public bool UpdateCoupons(RestaurantCoupons rc)
        {
            return _RestaurantCouponsDataAccess.Update(rc) > 0;
        }

        public RestaurantCoupons GetCouponsByCompanyIdandCode(Guid companyid, string code)
        {
            return _RestaurantCouponsDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and CouponCode = '{1}'", companyid, code)).FirstOrDefault();
        }

        public List<RestMenuItemCategory> GetAllMenuItemCategoryByCategoryId(Guid catid)
        {
            return _RestMenuItemCategoryDataAccess.GetByQuery(string.Format("CategoryId = '{0}'", catid)).ToList();
        }

        public RestMenuItem GetMenuItemByItemId(Guid itemid)
        {
            return _RestMenuItemDataAccess.GetByQuery(string.Format("ItemId = '{0}'", itemid)).FirstOrDefault();
        }

        public RestMenuItemCategoryURL GetMICUrlByMenuIdAndItemId(int menuid, int itemid)
        {
            return _RestMenuItemCategoryURLDataAccess.GetByQuery(string.Format("MenuId = {0} and MenuItemId = {1}", menuid, itemid)).FirstOrDefault();
        }

        public bool UpdateRestMenuItemCategoryURL(RestMenuItemCategoryURL rmic)
        {
            return _RestMenuItemCategoryURLDataAccess.Update(rmic) > 0;
        }

        public bool DeleteAllWebsiteLocationOperationByHoursOpt(string day, int siteid)
        {
            return _WebsiteLocationDataAccess.DeleteAllWebsiteLocationOperationByHoursOpt(day, siteid);
        }
    }
}
