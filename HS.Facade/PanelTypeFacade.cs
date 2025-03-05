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
    public class PanelTypeFacade:BaseFacade
    {
        public PanelTypeFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        PanelTypeDataAccess _PanelTypeDataAccess
        {
            get
            {
                return (PanelTypeDataAccess)_ClientContext[typeof(PanelTypeDataAccess)];
            }
        }
        public PanelType GetById(int value)
        {
            return _PanelTypeDataAccess.Get(value);
        }
        public bool UpdatePanelType(PanelType eq)
        {
            return _PanelTypeDataAccess.Update(eq) > 0;
        }

        public long InsertPanelType(PanelType eq)
        {
            return _PanelTypeDataAccess.Insert(eq);
        }
        public List<PanelType> GetAllPanelType()
        {
            return _PanelTypeDataAccess.GetAll();
        }
        public List<PanelType> GetAllPanelTypeByCompanyId(Guid companyId)
        {
            DataTable dt = _PanelTypeDataAccess.GetPanelTypeListByCompanyId(companyId);
            List<PanelType> EstimateStatusDetailList = new List<PanelType>();
            EstimateStatusDetailList = (from DataRow dr in dt.Rows
                                        select new PanelType()
                                        {
                                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                            CompanyId = (Guid)dr["CompanyId"],
                                            Name = dr["Name"].ToString(),
                                            Value = dr["Value"].ToString(),
                                            IsActive = dr["IsActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsActive"]) : false,
                                            EquipName = dr["EquipName"].ToString()
                                        }).ToList();
            return EstimateStatusDetailList;
        }
        public bool DeletePanelType(int Id)
        {
            return _PanelTypeDataAccess.Delete(Id) > 0;
        }
    }
}
