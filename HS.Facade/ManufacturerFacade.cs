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
    public class ManufacturerFacade :BaseFacade
    {
        public ManufacturerFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        ManufacturerDataAccess _ManufacturerDataAccess
        {
            get
            {
                return (ManufacturerDataAccess)_ClientContext[typeof(ManufacturerDataAccess)];
            }
        }

        public List<Manufacturer> GetAllManufacturers()
        {
            return _ManufacturerDataAccess.GetAll();
        }
        public DataTable GetAllManufacturersExport(Guid CompanyId,string name)
        {
            return _ManufacturerDataAccess.GetAllManufacturersExport(CompanyId, name);
        }
        public Manufacturer GetById(int value)
        {
            return _ManufacturerDataAccess.Get(value);
        }

        public int InsertManufacturer(Manufacturer manu)
        {
            return (int)_ManufacturerDataAccess.Insert(manu);
        }

        public bool UpdateManufacturer(Manufacturer manu)
        {
            return _ManufacturerDataAccess.Update(manu)>0;
        }

        public List<Manufacturer> GetAllManufacturerByCompanyId(Guid companyId)
        {
            return _ManufacturerDataAccess.GetByQuery(string.Format(" CompanyId = '{0}'", companyId));
        }
        public List<Manufacturer> GetAllManufacturerByCompanyIdBasePackage(Guid CompanyId)
        {
            DataTable dt = _ManufacturerDataAccess.GetAllManufacturerByCompanyIdBasePackage(CompanyId);
            List<Manufacturer> ManufacturerList = new List<Manufacturer>();
            ManufacturerList = (from DataRow dr in dt.Rows
                               select new Manufacturer()
                               {
                                   Name = dr["Name"].ToString(),
                                   ManufacturerId = dr["ManufacturerId"] != DBNull.Value ? (Guid)(dr["ManufacturerId"]) : new Guid()
                               }).ToList();
            return ManufacturerList;
        }
        public ManufacturerModelWithPaging GetAllManufacturer(int pageno,int pagesize)
        {
            DataSet dsResult = _ManufacturerDataAccess.GetAllManufacturer(pageno,pagesize);
            DataTable dt = dsResult.Tables[0];
            DataTable dt1 = dsResult.Tables[1];
            DataTable dt2 = dsResult.Tables[2];
            List<Manufacturer> propertyList = new List<Manufacturer>();

            propertyList = (from DataRow dr in dt.Rows
                            select new Manufacturer()
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                        

                                Name = dr["Name"].ToString(),

                            }).ToList();



            ManufacturerModelWithPaging LeadList = new ManufacturerModelWithPaging();
            LeadList.TotalCount = dsResult.Tables[1].Rows[0]["TotalCount"] != DBNull.Value ? Convert.ToInt32(dsResult.Tables[1].Rows[0]["TotalCount"]) : 0;
            LeadList.ManufacturerCount = dsResult.Tables[2].Rows[0]["CountManu"] != DBNull.Value ? Convert.ToInt32(dsResult.Tables[2].Rows[0]["CountManu"]) : 0;

            LeadList.Manufacturerlist = propertyList;
     
            return LeadList;


        }
        public List<Manufacturer> GetAllManufacturerName()
        {
            DataTable dt = _ManufacturerDataAccess.GetAllManufacturerName();
            List<Manufacturer> VendorSearchList = new List<Manufacturer>();
            VendorSearchList = dt.AsEnumerable().Select(dataRow => new Manufacturer
            {

                Name = dataRow.Field<string>("Name"),


                ManufacturerId = dataRow.Field<Guid>("ManufacturerId"),
            }).ToList();


            return VendorSearchList;
        }
        public bool DeleteManufatcurerById(int id)
        {
            return _ManufacturerDataAccess.Delete(id)>0;
        }

        public Manufacturer GetManufacturerById(int manufacturerId)
        {
            return _ManufacturerDataAccess.Get(manufacturerId);
        }
        public List<Manufacturer> GetManufacturerListByEquipmentId(Guid EquipmentId)
        {
            DataTable dt = _ManufacturerDataAccess.GetManufacturerListByEquipmentId(EquipmentId);
            List<Manufacturer> ManufacturerList = new List<Manufacturer>();
            ManufacturerList = (from DataRow dr in dt.Rows
                        select new Manufacturer()
                        {
                            ManufacturerId = (Guid)dr["ManufacturerId"],
                            Name = dr["Name"].ToString(),
                            IsActive = dr["IsPrimary"] != DBNull.Value ? Convert.ToBoolean(dr["IsPrimary"]) : false,
                        }).ToList();
            return ManufacturerList;
        }
    }
}
