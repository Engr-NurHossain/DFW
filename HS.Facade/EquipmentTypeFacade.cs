using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HS.DataAccess;
using HS.Framework;
using HS.Entities;
using System.Data;
using System.Web.Mvc;

namespace HS.Facade
{
    public class EquipmentTypeFacade: BaseFacade
    {
        public EquipmentTypeFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        EquipmentTypeDataAccess _EquipmentTypeDataAccess
        {
            get
            {
                return (EquipmentTypeDataAccess)_ClientContext[typeof(EquipmentTypeDataAccess)];
            }
        }

        public List<EquipmentType> GetAllProductCategory()
        {
            return _EquipmentTypeDataAccess.GetAll();
        }

        public EquipmentType GetById(int value)
        {
            return _EquipmentTypeDataAccess.Get(value);
        }

        public List<EquipmentType> GetAllProductCategoryByCompanyId(Guid companyId)
        {
            return _EquipmentTypeDataAccess.GetByQuery(string.Format(" CompanyId = '{0}' and IsActive = 1", companyId));
        }
        public List<SelectListItem> GetAllEquipmentCategoryWithSubCategoryByCompanyId(Guid companyId)
        {
            DataTable dt = _EquipmentTypeDataAccess.GetAllEquipmentTypeCategoryWithSubCategoryByCompanyId(companyId);
            if (dt != null)
            {
                List<SelectListItem> CategoryList = (from DataRow dr in dt.Rows
                                                    select new SelectListItem()
                                                    {
                                                        Value = dr["Id"].ToString(),
                                                        Text = dr["CategoryName"].ToString()
                                                    }).ToList();
                return CategoryList;
            }
            return null;
        }
        public List<EquipmentTypeListViewByFilterModel> GetAllProductCategoryByUserFilter(Guid companyId)
        {
            DataTable dt = _EquipmentTypeDataAccess.GetAllEquipmentTypeCategoryWithSubCategoryByUserFilter(companyId);            
            if (dt != null)
            {
                List<EquipmentTypeListViewByFilterModel> CategoryList = (from DataRow dr in dt.Rows
                                            select new EquipmentTypeListViewByFilterModel()
                                                     {
                                                         Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                                         Name = dr["Name"].ToString(),
                                                         ParentId = dr["ParentId"] != DBNull.Value ? Convert.ToInt32(dr["ParentId"]) : 0,
                                                         CategoryId = dr["CategoryId"].ToString()
                                                    }).ToList();
                return CategoryList;
            }
            return null;
        }
        public long InsertManufacturer(EquipmentType manu)
        {
            return _EquipmentTypeDataAccess.Insert(manu);
        }

        public bool UpdateManufacturer(EquipmentType manu)
        {
            return _EquipmentTypeDataAccess.Update(manu) > 0;
        }

        public EquipmentType GetEquipmentTypeById(int value)
        {
            return _EquipmentTypeDataAccess.Get(value);
        }

        public bool UpdateEquipmentType(EquipmentType eq)
        {
            return _EquipmentTypeDataAccess.Update(eq)>0;
        }

        public long InsertEquipmentType(EquipmentType eq)
        {
            return _EquipmentTypeDataAccess.Insert(eq);
        }
        public bool DeleteEquipmentType(int ProductId)
        {
            return _EquipmentTypeDataAccess.Delete(ProductId) > 0;
        }

        public List<EquipmentType> GetEquipmentTypeListByKey(string key)
        {
            return _EquipmentTypeDataAccess.GetByQuery(string.Format("Name like '%{0}%'", key)).ToList();
        }
    }
}

