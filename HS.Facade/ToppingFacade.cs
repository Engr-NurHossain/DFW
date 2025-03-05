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
    public class ToppingFacade : BaseFacade
    {
        public ToppingFacade(ClientContext clientContext)
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
        RestToppingDataAccess _RestToppingDataAccess
        {
            get
            {
                return (RestToppingDataAccess)_ClientContext[typeof(RestToppingDataAccess)];
            }
        }
        RestToppingCategoryDataAccess _RestToppingCategoryDataAccess
        {
            get
            {
                return (RestToppingCategoryDataAccess)_ClientContext[typeof(RestToppingCategoryDataAccess)];
            }
        }

        public ToppingListModel GetToppingList(int pageNo, int pageSize, string searchText, string order,Guid comId)
        {
            ToppingListModel Model = new ToppingListModel();
            DataSet ds = _RestToppingDataAccess.GetToppingList(pageNo, pageSize, searchText, order,comId);

            Model.ListToppingCategory = (from DataRow dr in ds.Tables[0].Rows
                                 select new RestToppingCategory()
                                 {
                                     CategoryId = dr["CategoryId"] != DBNull.Value ? Convert.ToInt32(dr["CategoryId"]) : 0,
                                     ToppingName = dr["ToppingName"].ToString(),
                                     ToppingCategory = dr["ToppingCategory"].ToString(),
                                     ToppingPrice = dr["ToppingPrice"].ToString(),
                                  
                                 }).ToList();

            Model.TotalCount = ds.Tables[1].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCount"]) : 0;
            Model.PageNo = pageNo;
            Model.PageSize = pageSize;
            Model.Searchtext = searchText;
            return Model;
        }

        //public List<MenuItem> GetAllMenuItem()
        //{
        //    return _MenuItemDataAccess.GetAll();
        //}
        public List<RestCategory> GetAllCategory()
        {
            return _RestCategoryDataAccess.GetAll();
        }
        //public List<Topping> GetToppingListByCategoryId(int categoryId)
        //{
        //    DataTable dt= _ToppingDataAccess.GetToppingByCategoryId(categoryId);
        //    List<Topping> toppingList = new List<Topping>();
        //    toppingList = (from DataRow dr in dt.Rows
        //                   select new Topping()
        //                   {
        //                       Id=dr["Id"]!=DBNull.Value?Convert.ToInt32(dr["Id"]):0,
        //                       ToppingCategoryId=dr["ToppingCategoryId"]!=DBNull.Value? Convert.ToInt32(dr["ToppingCategoryId"]) : 0,
        //                       ToppingName=dr["ToppingName"].ToString(),
        //                       Price=dr["Price"]!=DBNull.Value?Convert.ToDouble(dr["Price"]) : 0,
        //                       IsAvailable=dr["IsAvailable"]!=DBNull.Value?Convert.ToBoolean(dr["IsAvailable"]):false
        //                   }).ToList();
        //    return toppingList;
        //}
        public List<RestTopping> GetToppingListByCategoryId(Guid categoryId)
        {
            DataTable dt = _RestToppingDataAccess.GetToppingByCategoryId(categoryId);
            List<RestTopping> toppingList = new List<RestTopping>();
            toppingList = (from DataRow dr in dt.Rows
                           select new RestTopping()
                           {
                               Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                               ToppingCategoryId = (Guid)dr["ToppingCategoryId"],
                               ToppingName = dr["ToppingName"].ToString(),
                               ToppingCategoryName=dr["ToppingCategoryName"].ToString(),
                               Price = dr["Price"] != DBNull.Value ? Convert.ToDouble(dr["Price"]) : 0,
                               IsAvailable = dr["IsAvailable"] != DBNull.Value ? Convert.ToBoolean(dr["IsAvailable"]) : false,
                               IsDefault = dr["IsDefault"] != DBNull.Value ? Convert.ToBoolean(dr["IsDefault"]) : false,
                               Description = dr["Description"].ToString()
                           }).ToList();
            return toppingList;
            //return _ToppingDataAccess.GetByQuery(string.Format("ToppingCategoryId={0}", categoryId));
        }
        public RestToppingCategory GetToppingCategoryById(int Id)
        {
            return _RestToppingCategoryDataAccess.Get(Id);
        }
        public RestToppingCategory GetToppingCategoryByIdAndCompanyId(int Id, Guid comid)
        {
            return _RestToppingCategoryDataAccess.GetByQuery(string.Format("Id = {0} and CompanyId = '{1}'", Id, comid)).FirstOrDefault();
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
        public List<RestToppingCategory> GetAllTopping(Guid comId)
        {
            return _RestToppingCategoryDataAccess.GetByQuery(string.Format("CompanyId='{0}'",comId));
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
        //public Menu GetMenuById(int Id)
        //{
        //    return _TicketDataAccess.GetTicketByTicketId(ticketId);
        //}
        //public int InsertTicket(Ticket Ticket)
        //{
        //    return (int)_TicketDataAccess.Insert(Ticket);
        //}
        public bool DeleteAllToppingsByToppingCategoryId(Guid TCId)
        {
            return _RestToppingDataAccess.DeleteByToppingCategoryId(TCId);
        }
        public bool DeleteToppingCategoryById(int Id)
        {
            return _RestToppingCategoryDataAccess.Delete(Id)>0;
        }
        public bool UpdateToppingCategory(RestToppingCategory toppingCategory)
        {
            return _RestToppingCategoryDataAccess.Update(toppingCategory) > 0;
        }
        public bool UpdateTopping(RestTopping topping)
        {
            return _RestToppingDataAccess.Update(topping) > 0;
        }
        public long InsertTopping(RestTopping topping)
        {
            return _RestToppingDataAccess.Insert(topping);
        }
        public int InsertToppingCategory(RestToppingCategory toppingCategory)
        {
            return (int)_RestToppingCategoryDataAccess.Insert(toppingCategory);
        }
        //public bool UpdateTicket(Ticket ticket)
        //{
        //    return _TicketDataAccess.Update(ticket) > 0;
        //}
        //public List<TicketUser> GetTicketUserByTicketId(Guid ticketId)
        //{
        //    return _TicketUserDataAccess.GetByQuery(string.Format("TiketId = '{0}'", ticketId));
        //}

        //public bool DeleteTicketReplyById(int id)
        //{
        //    return _TicketReplyDataAccess.Delete(id)>0;
        //}

        public bool DeleteToppingById(int value)
        {
            return _RestToppingDataAccess.Delete(value) > 0;
        }

        public List<RestMenuItem> GetToppingListPartial(Guid Id, Guid toppingcateoryid)
        {
            DataTable dt = _RestToppingDataAccess.GetToppingListPartial(Id, toppingcateoryid);
            List<RestMenuItem> TList = new List<RestMenuItem>();
            TList = (from DataRow dr in dt.Rows
                      select new RestMenuItem()
                      {
                          ItemName = dr["ItemName"].ToString(),
                          Price = dr["Price"] != DBNull.Value ? Convert.ToInt32(dr["Price"]) : 0,
                          MaxQty = dr["MaxQty"] != DBNull.Value ? Convert.ToInt32(dr["MaxQty"]) : 0,
                          TimeAvailable = dr["TimeAvailable"].ToString(),
                          DaysAvailable = dr["DaysAvailable"].ToString(),
                          Photo = dr["Photo"].ToString(),
                          Status = dr["Status"] != DBNull.Value ? Convert.ToBoolean(dr["Status"]) : false,
                          Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                          UrlSlug = dr["UrlSlug"].ToString(),
                      }).ToList();
            return TList;
        }

    }
}
