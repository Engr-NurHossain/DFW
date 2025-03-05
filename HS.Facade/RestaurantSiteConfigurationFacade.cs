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
    public class RestaurantSiteConfigurationFacade : BaseFacade
    {
        public RestaurantSiteConfigurationFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        RestaurantSiteConfigurationDataAccess _RestaurantSiteConfigurationDataAccess
        {
            get
            {
                return (RestaurantSiteConfigurationDataAccess)_ClientContext[typeof(RestaurantSiteConfigurationDataAccess)];
            }
        }

        public RestaurantSiteConfigurationListModel GetRestaurantSiteConfigurationList(int pageNo,int pageSize, string searchText,string order)
        {
            RestaurantSiteConfigurationListModel Model = new RestaurantSiteConfigurationListModel();
            DataSet ds = _RestaurantSiteConfigurationDataAccess.GetRestaurantSiteConfigurationList(pageNo, pageSize, searchText, order);

            Model.restaurantSiteConfiguration = (from DataRow dr in ds.Tables[0].Rows
                                     select new RestaurantSiteConfiguration()
                                     {
                                         Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                         SiteName = dr["SiteName"].ToString(),
                                         DomainName = dr["DomainName"].ToString(),
                                         StorePhone=dr["StorePhone"].ToString(),
                                         SendOrdersEmail = dr["SendOrdersEmail"].ToString(),
                                         ThemeURL=dr["ThemeURL"].ToString()
                                     }).ToList();

            Model.TotalCount = ds.Tables[1].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[0]["TotalCount"]) : 0;
            Model.PageNo = pageNo;
            Model.PageSize = pageSize;
            Model.Searchtext = searchText;
            return Model;
        }
        public List<RestaurantSiteConfiguration> GetAllRestaurantSiteConfiguration()
        {
            return _RestaurantSiteConfigurationDataAccess.GetAll();
        }    
        public RestaurantSiteConfiguration GetRestaurantSiteConfigurationById(int Id)
        {
            return _RestaurantSiteConfigurationDataAccess.Get(Id);
        }
        public int InsertSiteConfiguration(RestaurantSiteConfiguration restaurantSiteConfiguration)
        {
            return (int)_RestaurantSiteConfigurationDataAccess.Insert(restaurantSiteConfiguration);
        }
        public bool UpdateRestaurantSiteConfiguration(RestaurantSiteConfiguration restaurantSiteConfiguration)
        {
            return _RestaurantSiteConfigurationDataAccess.Update(restaurantSiteConfiguration) > 0;
        }
        public bool DeleteRestaurantSiteConfigurationById(int id)
        {
            return _RestaurantSiteConfigurationDataAccess.Delete(id) > 0;
        }
        
    }
}
