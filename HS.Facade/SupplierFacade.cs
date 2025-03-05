using HS.DataAccess;
using HS.Framework;
using HS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace HS.Facade
{
    public class SupplierFacade : BaseFacade
    {
        public SupplierFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }

        SupplierDataAccess _SupplierDataAccess
        {
            get
            {
                return (SupplierDataAccess)_ClientContext[typeof(SupplierDataAccess)];
            }
        }

        SupplierBillDataAccess _SupplierBillDataAccess
        {
            get
            {
                return (SupplierBillDataAccess)_ClientContext[typeof(SupplierBillDataAccess)];
            }
        }

        SupplierFileDataAccess _SupplierFileDataAccess
        {
            get
            {
                return (SupplierFileDataAccess)_ClientContext[typeof(SupplierFileDataAccess)];
            }
        }

        public List<Supplier> GetAllSupplier()
        {
            return _SupplierDataAccess.GetAll();
        }
        public List<Supplier> GetAllSupplierName()
        {
            DataTable dt = _SupplierDataAccess.GetAllSupplierName();
            List<Supplier> VendorSearchList = new List<Supplier>();
            VendorSearchList = dt.AsEnumerable().Select(dataRow => new Supplier
            {

                Name = dataRow.Field<string>("Name"), 
                CompanyName = dataRow.Field<string>("CompanyName"),
                SupplierId = dataRow.Field<Guid>("SupplierId"),
            }).ToList();


            return VendorSearchList;
        }
        public List<Supplier> GetAllSupplierListByCompanyId(Guid comid)
        {
            DataTable dt = _SupplierDataAccess.GetAllSupplierListByCompanyId(comid);
            List<Supplier> VendorSearchList = new List<Supplier>();
            VendorSearchList = dt.AsEnumerable().Select(dataRow => new Supplier
            {
                Id = dataRow.Field<Int32>("Id"),
                Name = dataRow.Field<string>("Name"),
                Street = dataRow.Field<string>("Street"),
                City = dataRow.Field<string>("City"),
                State = dataRow.Field<string>("State"),
                Zipcode = dataRow.Field<string>("Zipcode"),
                SupplierId = dataRow.Field<Guid>("SupplierId"),
                CompanyName = dataRow.Field<string>("CompanyName"),
                Phone = dataRow.Field<string>("Phone"),
                EmailAddress = dataRow.Field<string>("EmailAddress"),
                Country = dataRow.Field<string>("Country"),
                Note = dataRow.Field<string>("Note"),
                Website = dataRow.Field<string>("Website"),
                ContactPersonName = dataRow.Field<string>("ContactPersonName"),
            }).ToList();
            return VendorSearchList;
        }
        public DataTable GetAllSupplierListByCompanyIdForExport(Guid comid)
        {
            return _SupplierDataAccess.GetAllSupplierListByCompanyIdForExport(comid); ;
        }
        public Supplier GetSupplierById(int value)
        {
            return _SupplierDataAccess.Get(value);
        }

        public int InsertSupplier(Supplier supplier)
        {
            return (int)_SupplierDataAccess.Insert(supplier);
        }

        public bool UpdateSupplier(Supplier supplier)
        {
            return _SupplierDataAccess.Update(supplier) > 0;
        }

        public bool DeleteSupplier(int supplierId)
        {
            return _SupplierDataAccess.Delete(supplierId) > 0;
        }
        public List<Supplier> GetAllSupplierByCompanyId(Guid companyId)
        {
            return _SupplierDataAccess.GetByQuery(string.Format(" CompanyId = '{0}'", companyId));
        }

        public int InsertInitSupplierBill(SupplierBill objSupplierBill)
        {
            return (int)_SupplierBillDataAccess.Insert(objSupplierBill);
        }

        public bool UpdateSupplierBill(SupplierBill objSupplierBill)
        {
            return _SupplierBillDataAccess.Update(objSupplierBill) > 0;
        }

        public long InsertSupplierFile(SupplierFile sf)
        {
            return _SupplierFileDataAccess.Insert(sf);
        }

        public List<Supplier> GetAllSupplierByCompanyIdAndSearchKey(Guid companyID, string key, int max)
        {
            DataTable dt = _SupplierDataAccess.GetCustomerListBySearchKeyAndCompanyId(companyID, key, max);
            List<Supplier> VendorSearchList = new List<Supplier>();
            VendorSearchList = dt.AsEnumerable().Select(dataRow => new Supplier
            {
                Id = dataRow.Field<Int32>("Id"),
                SupplierId = dataRow.Field<Guid>("SupplierId"),
                Name = dataRow.Field<string>("Name"),
                Street = dataRow.Field<string>("Street"),
                City = dataRow.Field<string>("City"),
                State = dataRow.Field<string>("State"),
                Zipcode = dataRow.Field<string>("Zipcode"),
                CompanyName = dataRow.Field<string>("CompanyName"),
                Phone = dataRow.Field<string>("Phone"),
                EmailAddress = dataRow.Field<string>("EmailAddress"),
                SalesRepName = dataRow.Field<string>("SalesRepName") != null ? dataRow.Field<string>("SalesRepName") : "",
            }).ToList();
            return VendorSearchList;
        }
        public List<SupplierCustom> GetAllSupplierByCompanyIdAndSearchKeyAndEquipmentId(Guid companyID, string key, int max, Guid EquipmentId)
        {
            DataTable dt = _SupplierDataAccess.GetAllSupplierByCompanyIdAndSearchKeyAndEquipmentId(companyID, key, max, EquipmentId);
            List<SupplierCustom> VendorSearchList = new List<SupplierCustom>();
            VendorSearchList = dt.AsEnumerable().Select(dataRow => new SupplierCustom
            {
                Id = dataRow.Field<Int32>("Id"),
                SupplierId = dataRow.Field<Guid>("SupplierId"),
                Name = dataRow.Field<string>("Name"),
                Street = dataRow.Field<string>("Street"),
                City = dataRow.Field<string>("City"),
                State = dataRow.Field<string>("State"),
                Zipcode = dataRow.Field<string>("Zipcode"),
                CompanyName = dataRow.Field<string>("CompanyName"),
                Phone = dataRow.Field<string>("Phone"),
                EmailAddress = dataRow.Field<string>("EmailAddress"),
                SalesRepName = dataRow.Field<string>("SalesRepName"),
                Cost = dataRow.Field<double>("Cost")
            }).ToList();
            return VendorSearchList;
        }
        public Supplier GetVendorNameByCompanyIdAndSupplierId(Guid companyID, int? id)
        {
            Supplier supplier = _SupplierDataAccess.GetByQuery(string.Format(" [CompanyId] ='{0}' AND Id={1}", companyID, id)).FirstOrDefault();

            return supplier;
        }

        public List<SupplierFile> GetAllSupplierFileBySupplierIdAndCompanyId(Guid comid, Guid supid)
        {
            return _SupplierFileDataAccess.GetByQuery(string.Format("CompanyId = '{0}' and SupplierId = '{1}'", comid, supid)).ToList();
        }

        public SupplierFile GetFileNameBySupplierId(int value)
        {
            return _SupplierFileDataAccess.Get(value);
        }
        public bool DeleteSupplierById(int value)
        {
            return _SupplierFileDataAccess.Delete(value) > 0;
        }

        public Supplier GetSupplierBySupplierId(Guid supid)
        {
            return _SupplierDataAccess.GetByQuery(string.Format("SupplierId = '{0}'", supid)).FirstOrDefault();
        }

        public Supplier GetSupplierByCompanyName(string companyName)
        {
            return _SupplierDataAccess.GetByQuery(string.Format("CompanyName = '{0}'", companyName)).FirstOrDefault();
        }
    }
}
