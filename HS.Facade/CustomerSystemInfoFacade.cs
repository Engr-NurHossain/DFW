using HS.DataAccess;
using HS.Entities;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Facade
{
    public class CustomerSystemInfoFacade:BaseFacade
    {
        public CustomerSystemInfoFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        CustomerSystemInfoDataAccess _CustomerSystemInfoDataAccess
        {
            get
            {
                return (CustomerSystemInfoDataAccess)_ClientContext[typeof(CustomerSystemInfoDataAccess)];
            }
        }
     
        CustomerSystemInfoDraftDataAccess _CustomerSystemInfoDraftDataAccess
        {
            get
            {
                return (CustomerSystemInfoDraftDataAccess)_ClientContext[typeof(CustomerSystemInfoDraftDataAccess)];
            }
        }
        PanelTypeDataAccess _PanelTypeDataAccess
        {
            get
            {
                return (PanelTypeDataAccess)_ClientContext[typeof(PanelTypeDataAccess)];
            }
        }
        public long InsertSystemInfo(CustomerSystemInfo system)
        {
            return _CustomerSystemInfoDataAccess.Insert(system);
        }
        public long InsertSystemInfoDraft(CustomerSystemInfoDraft system)
        {
            return _CustomerSystemInfoDraftDataAccess.Insert(system);
        }
        public CustomerSystemInfo GetSystemInfoById(int id)
        {
            return _CustomerSystemInfoDataAccess.Get(id);
        }

        public CustomerSystemInfoDraft GetSystemInfoDraftById(int id)
        {
            return _CustomerSystemInfoDraftDataAccess.Get(id);
        }
        public bool UpdateSystemInfo(CustomerSystemInfo system)
        {
            return _CustomerSystemInfoDataAccess.Update(system) > 0;
        }
        public bool UpdateSystemInfoDraft(CustomerSystemInfoDraft system)
        {
            return _CustomerSystemInfoDraftDataAccess.Update(system) > 0;
        }
        public CustomerSystemInfo GetCustomerSystemInfoById(int value)
        {
            return _CustomerSystemInfoDataAccess.Get(value);
        }

        public CustomerSystemInfoDraft GetCustomerSystemInfoDraftById(int value)
        {
            return _CustomerSystemInfoDraftDataAccess.Get(value);
        }
        public List<PanelType> GetAllPanelTypeByCompanyId(Guid companyId)
        {
            return _PanelTypeDataAccess.GetByQuery(string.Format(" CompanyId = '{0}'", companyId));
        }

        public CustomerSystemInfo GetCustomerSystemInfoByCustomerIdAndCompanyId(Guid CustomerId, Guid CompanyId)
        {
            return _CustomerSystemInfoDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and CompanyId = '{1}'", CustomerId, CompanyId)).FirstOrDefault();
        }
        public CustomerSystemInfoDraft GetCustomerSystemInfoDraftByCustomerIdAndCompanyId(Guid CustomerId, Guid CompanyId)
        {
            return _CustomerSystemInfoDraftDataAccess.GetByQuery(string.Format("CustomerId = '{0}' and CompanyId = '{1}'", CustomerId, CompanyId)).FirstOrDefault();
        }
        public CustomerSystemInfo GetAllCustomerSystemInfoDetailsByCustomerId(Guid customerid)
        {
            DataTable dt = _CustomerSystemInfoDataAccess.GetAllCustomerSystemInfoDetailsByCustomerId(customerid);
            CustomerSystemInfo SystemList = new CustomerSystemInfo();
            SystemList = (from DataRow dr in dt.Rows
                          select new CustomerSystemInfo()
                          {
                              Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                              CompanyId = (Guid)dr["CompanyId"],
                              CustomerId = (Guid)dr["CustomerId"],
                              PanelType = dr["PanelType"].ToString(),
                              InstallType = dr["InstallType"].ToString(),
                              CellularBackup = dr["CellularBackup"].ToString(),
                              Zone1 = dr["Zone1"].ToString(),
                              Zone2 = dr["Zone2"].ToString(),
                              Zone3 = dr["Zone3"].ToString(),
                              Zone4 = dr["Zone4"].ToString(),
                              Zone5 = dr["Zone5"].ToString(),
                              Zone6 = dr["Zone6"].ToString(),
                              Zone7 = dr["Zone7"].ToString(),
                              Zone8 = dr["Zone8"].ToString(),
                              Zone9 = dr["Zone9"].ToString(),
                             InstallTypeVal = dr["InstallTypeVal"].ToString(),
                             LeadInstallTypeVal = dr["LeadInstallTypeVal"].ToString(),

                          }).ToList().FirstOrDefault();
            return SystemList;
        }
    }
}
