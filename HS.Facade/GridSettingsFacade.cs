using HS.DataAccess;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HS.Entities;
using System.Data;

namespace HS.Facade
{
    public class GridSettingsFacade : BaseFacade
    {
        GridSettingDataAccess _GridSettingDataAccess;
        public GridSettingsFacade(ClientContext clientContext)
            : base(clientContext)
        {
            _GridSettingDataAccess = (GridSettingDataAccess)_ClientContext[typeof(GridSettingDataAccess)];
        }

        public GridSettingsFacade()
        {
            _GridSettingDataAccess = new GridSettingDataAccess();
        }

        public GridSettingsFacade(string constr)
        {
            _GridSettingDataAccess = new GridSettingDataAccess(constr);
        }

        public List<GridSetting> GetByKey(string key, Guid CompanyId)
        {
            try
            {
                return _GridSettingDataAccess.GetByQuery(string.Format("[ListKeyName] ='{0}' and CompanyId='{1}' and IsActive=1  order by [OrderBy] asc", key, CompanyId));
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<GridSetting> GetByKeyCustomer(string key, Guid CompanyId)
        {
            return _GridSettingDataAccess.GetByQuery(string.Format("[ListKeyName] ='{0}' and CompanyId='{1}' and IsActive=1 order by [GroupOrder] asc", key,CompanyId.ToString()));
        }

        public List<GridSetting> GetAllByKey(string key, Guid CompanyId)
        {
            return _GridSettingDataAccess.GetByQuery(string.Format("[ListKeyName] ='{0}' and CompanyId='{1}' order by [OrderBy] asc", key,CompanyId.ToString()));
        }

        public List<GridSetting> GetAllGridSettingByCompanyId(Guid companyid)
        {
            return _GridSettingDataAccess.GetByQuery(string.Format("CompanyId='{0}'", companyid)).ToList();
        }

        public List<GridSetting> GetAllCustomerAndLeadGridSetting(Guid CompanyId)
        {
            return _GridSettingDataAccess.GetByQuery(string.Format("([ListKeyName] = 'CustomerGrid' or [ListKeyName] = 'LeadGrid') and CompanyId='{0}' order by [OrderBy] asc", CompanyId.ToString()));
        }

        public bool UpdateGridSettings(GridSetting item)
        {
            return _GridSettingDataAccess.Update(item)>0;
        }

        public List<CustomerLeadGridModel> GetCustomerLeadGridSetting(Guid comid)
        {
            DataTable dt = _GridSettingDataAccess.GetCustomerLeadGridSetting(comid);
            List<CustomerLeadGridModel> SearchList = new List<CustomerLeadGridModel>();
            SearchList = (from DataRow dr in dt.Rows
                          select new CustomerLeadGridModel()
                          {
                              CustomerGridId = dr["CustomerGridId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerGridId"]) : 0,
                              LeadGridId = dr["LeadGridId"] != DBNull.Value ? Convert.ToInt32(dr["LeadGridId"]) : 0,
                              ComId = (Guid)dr["ComId"],
                              SColumn = dr["SColumn"].ToString(),
                              CustomerColumnGroup = dr["CustomerColumnGroup"].ToString(),
                              LeadColumnGroup = dr["LeadColumnGroup"].ToString(),
                              CustomerGroupOrder = dr["CustomerGroupOrder"] != DBNull.Value ? Convert.ToInt32(dr["CustomerGroupOrder"]) : 0,
                              LeadGroupOrder = dr["LeadGroupOrder"] != DBNull.Value ? Convert.ToInt32(dr["LeadGroupOrder"]) : 0,
                              ByOrder = dr["ByOrder"] != DBNull.Value ? Convert.ToInt32(dr["ByOrder"]) : 0,
                              ActivateColumn = dr["ActivateColumn"] != DBNull.Value ? Convert.ToBoolean(dr["ActivateColumn"]) : false,
                              CustomerGridActive = dr["CustomerGridActive"] != DBNull.Value ? Convert.ToBoolean(dr["CustomerGridActive"]) : false,
                              LeadGridActive = dr["LeadGridActive"] != DBNull.Value ? Convert.ToBoolean(dr["LeadGridActive"]) : false,
                              CustomerFormActive = dr["CustomerFormActive"] != DBNull.Value ? Convert.ToBoolean(dr["CustomerFormActive"]) : false,
                              LeadFormActive = dr["LeadFormActive"] != DBNull.Value ? Convert.ToBoolean(dr["LeadFormActive"]) : false,
                              CustomerKey = dr["CustomerKey"].ToString(),
                              LeadKey = dr["LeadKey"].ToString(),
                              IsCustomerFilter = dr["IsCustomerFilter"] != DBNull.Value ? Convert.ToBoolean(dr["IsCustomerFilter"]) : false,
                              IsLeadFilter = dr["IsLeadFilter"] != DBNull.Value ? Convert.ToBoolean(dr["IsLeadFilter"]) : false,
                              IsCustomerRequired = dr["IsCustomerRequired"] != DBNull.Value ? Convert.ToBoolean(dr["IsCustomerRequired"]) : false,
                              IsLeadRequired = dr["IsLeadRequired"] != DBNull.Value ? Convert.ToBoolean(dr["IsLeadRequired"]) : false,
                              IsCustomerLabel = dr["IsCustomerLabel"] != DBNull.Value ? Convert.ToBoolean(dr["IsCustomerLabel"]) : false,
                              IsLeadLabel = dr["IsLeadLabel"] != DBNull.Value ? Convert.ToBoolean(dr["IsLeadLabel"]) : false
                          }).ToList();
            return SearchList;
        }

        public List<CustomerLeadDetailGridModel> GetCustomerLeadDetailGridSetting(Guid comid)
        {
            DataTable dt = _GridSettingDataAccess.GetCustomerLeadDetailGridSetting(comid);
            List<CustomerLeadDetailGridModel> SearchList = new List<CustomerLeadDetailGridModel>();
            SearchList = (from DataRow dr in dt.Rows
                          select new CustomerLeadDetailGridModel()
                          {
                              CustomerDetailId = dr["CustomerDetailId"] != DBNull.Value ? Convert.ToInt32(dr["CustomerDetailId"]) : 0,
                              LeadDetailId = dr["LeadDetailId"] != DBNull.Value ? Convert.ToInt32(dr["LeadDetailId"]) : 0,
                              DetailKey = dr["DetailKey"].ToString(),
                              CustomerDetailColumn = dr["CustomerDetailColumn"].ToString(),
                              LeadDetailColumn = dr["LeadDetailColumn"].ToString(),
                              CustomerDetailOrder = dr["CustomerDetailOrder"] != DBNull.Value ? Convert.ToInt32(dr["CustomerDetailOrder"]) : 0,
                              LeadDetailOrder = dr["LeadDetailOrder"] != DBNull.Value ? Convert.ToInt32(dr["LeadDetailOrder"]) : 0,
                              DetailActive = dr["DetailActive"] != DBNull.Value ? Convert.ToBoolean(dr["DetailActive"]) : false,
                              CustomerDetailForm = dr["CustomerDetailForm"] != DBNull.Value ? Convert.ToBoolean(dr["CustomerDetailForm"]) : false,
                              LeadDetailForm = dr["LeadDetailForm"] != DBNull.Value ? Convert.ToBoolean(dr["LeadDetailForm"]) : false,
                          }).ToList();
            return SearchList;
        }

        public List<GridSetting> GetAllFilterSettingByKeyAndCompanyIdAndIsFilter(string key, Guid comid)
        {
            return _GridSettingDataAccess.GetByQuery(string.Format("ListKeyName = '{0}' and CompanyId = '{1}' and IsFilter = 1", key, comid)).ToList();
        }
        public List<GridSetting> GetAllCustomerFilterSettingByKeyAndCompanyIdAndIsActive(Guid comid)
        {
            return _GridSettingDataAccess.GetByQuery(string.Format("ListKeyName = 'CustomerGrid' and CompanyId = '{0}' and FormActive = 1", comid)).ToList();
        }
        public List<GridSetting> GetAllLeadsFilterSettingByKeyAndCompanyIdAndIsActive(Guid comid)
        {
            return _GridSettingDataAccess.GetByQuery(string.Format("ListKeyName = 'LeadGrid' and CompanyId = '{0}' and FormActive = 1", comid)).ToList();
        }
        public List<GridSetting> GetAllLeadsFilterSettingByKeyAndCompanyIdAndIsActiveandKey(Guid comid,string Key)
        {
            return _GridSettingDataAccess.GetByQuery(string.Format("ListKeyName = 'LeadGrid' and CompanyId = '{0}' and FormActive = 1 and SelectedColumn ='{1}'", comid, Key)).ToList();
        }
 
        public long InsertGridSetting(GridSetting grid)
        {
            return _GridSettingDataAccess.Insert(grid);
        }
    }
}
