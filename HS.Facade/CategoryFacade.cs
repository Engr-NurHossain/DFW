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
    public class CategoryFacade : BaseFacade
    {
        public CategoryFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        RestMenuDataAccess _RestMenuDataAccess
        {
            get
            {
                return (RestMenuDataAccess)_ClientContext[typeof(RestMenuDataAccess)];
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

        public CategoryListModel GetCategoryList(Guid comid, int pageNo,int pageSize, string searchText,string order)
        {
            CategoryListModel Model = new CategoryListModel();
            DataSet ds = _RestCategoryDataAccess.GetCategoryList(comid, pageNo, pageSize, searchText, order);

            Model.Categoriess = (from DataRow dr in ds.Tables[0].Rows
                                     select new RestCategory()
                                     {
                                         Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                         CategoryName = dr["CategoryName"].ToString(),
                                         Description = dr["Description"].ToString(),
                                         DaysAvailable=dr["DaysAvailable"].ToString(),
                                         TimeAvailable=dr["TimeAvailable"].ToString(),
                                         Status = dr["Status"] != DBNull.Value ? Convert.ToBoolean(dr["Status"]) : false,
                                         Image=dr["Image"].ToString(),
                                         UrlSlug = dr["UrlSlug"].ToString(),
                                         OrderBy = dr["OrderBy"] != DBNull.Value ? Convert.ToInt32(dr["OrderBy"]) : 0,
                                         DaysAvailableOption = dr["DaysAvailableOption"].ToString()
                                     }).ToList();

            Model.TotalCount = ds.Tables[1].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCount"]) : 0;
            Model.PageNo = pageNo;
            Model.PageSize = pageSize;
            Model.Searchtext = searchText;
            return Model;
        }

       
        public List<RestCategory> GetAllCategory()
        {
            return _RestCategoryDataAccess.GetAll();
        }
        public RestCategory GetCategoryById(int Id)
        {
            return _RestCategoryDataAccess.Get(Id);
        }
        public RestCategory GetCategoryByIdAndCompanyId(int Id, Guid comid)
        {
            return _RestCategoryDataAccess.GetByQuery(string.Format("Id = {0} and CompanyId = '{1}'", Id, comid)).FirstOrDefault();
        }
        public List<RestMenuItem> GetAllMenuItemByMenuId(Guid menuId)
        {
            DataTable dt = _RestMenuDataAccess.GetAllMenuItemByMenuId(menuId);
            List<RestMenuItem> miList = new List<RestMenuItem>();
            miList = (from DataRow dr in dt.Rows
                          select new RestMenuItem()
                          {
                              Id = dr["MenuItemId"] != DBNull.Value ? Convert.ToInt32(dr["MenuItemId"]) : 0,
                              ItemName = dr["ItemName"].ToString()
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
        
        public bool UpdateCategory(RestCategory category)
        {
            return _RestCategoryDataAccess.Update(category) > 0;
        }
        public long InsertCategory(RestCategory category)
        {
            return _RestCategoryDataAccess.Insert(category);
        }
        public bool DeleteAllMenuItemCategoryByMenuIdAndItemId(Guid menuId, Guid itemid, int menuintid, int itemintid)
        {
            return _RestCategoryDataAccess.DeleteByMenuId(menuId, itemid, menuintid, itemintid);
        }
        public bool DeleteAllCategoryDetailsByCategoryId(int categoryId)
        {
            return _RestCategoryDataAccess.DeleteByCategoryId(categoryId);
        }
        public bool DeleteCategoryById(int id)
        {
            return _RestCategoryDataAccess.Delete(id) > 0;
        }
        public List<RestMenuItem> GetCategoryItemListPartial(Guid Id, int categoryid)
        {
            DataTable dt = _RestCategoryDataAccess.GetCategoryItemListPartial(Id, categoryid);
            List<RestMenuItem> miList = new List<RestMenuItem>();
            miList = (from DataRow dr in dt.Rows
                      select new RestMenuItem()
                      {
                          Photo = dr["Photo"].ToString(),
                          ItemName = dr["ItemName"].ToString(),
                          Price = dr["Price"] != DBNull.Value ? Convert.ToInt32(dr["Price"]) : 0,
                          DaysAvailable = dr["DaysAvailable"].ToString(),
                          Status = dr["Status"] != DBNull.Value ? Convert.ToBoolean(dr["Status"]) : false,
                          Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                          UrlSlug = dr["UrlSlug"].ToString(),
                      }).ToList();
            return miList;
        }

        public MenuItemListModel GetMenuItemListByCompanyId(Guid comid, int pageNo, int pageSize, string searchText, string order)
        {
            MenuItemListModel Model = new MenuItemListModel();
            DataSet ds = _RestCategoryDataAccess.GetMenuItemListByCompanyId(comid, pageNo, pageSize, searchText, order);

            Model.MenuItems = (from DataRow dr in ds.Tables[0].Rows
                                 select new RestMenuItem()
                                 {
                                     Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                     ItemName = dr["ItemName"].ToString(),
                                     Description = dr["Description"].ToString(),
                                     DaysAvailable = dr["DaysAvailable"].ToString(),
                                     TimeAvailable = dr["TimeAvailable"].ToString(),
                                     Status = dr["Status"] != DBNull.Value ? Convert.ToBoolean(dr["Status"]) : false,
                                     Photo = dr["Photo"].ToString(),
                                     Price = dr["Price"] != DBNull.Value ? Convert.ToDouble(dr["Price"]) : 0,
                                     MenuName = dr["MenuName"].ToString(),
                                     MenuId = dr["MenuId"] != DBNull.Value ? Convert.ToInt32(dr["MenuId"]) : 0,
                                 }).ToList();

            Model.TotalCount = ds.Tables[1].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCount"]) : 0;
            Model.PageNo = pageNo;
            Model.PageSize = pageSize;
            Model.Searchtext = searchText;
            return Model;
        }
    }
}
