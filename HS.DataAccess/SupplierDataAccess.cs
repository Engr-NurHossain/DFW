using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.DataAccess
{
    public partial class SupplierDataAccess
    {
        public DataTable GetCustomerListBySearchKeyAndCompanyId(Guid companyId, string key, int MaxLoad)
        {
            string sqlQuery = @"select
                                Top {2} *
                                from Supplier
                                where CompanyName like '%{1}%' and CompanyId ='{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId, key, MaxLoad);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetAllSupplierByCompanyIdAndSearchKeyAndEquipmentId(Guid companyId, string key, int MaxLoad, Guid EquipmentId)
        {
            string sqlQuery = @"select
                                Top {2} sp.*
                                ,ISNULL(eqv.Cost,0) Cost
                                from EquipmentVendor eqv
								LEFT JOIN Supplier sp on sp.SupplierId=eqv.SupplierId
                                where CompanyName like '%{1}%' 
                                and CompanyId ='{0}'
                                and eqv.EquipmentId='{3}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId, key, MaxLoad, EquipmentId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetVendorNameByCompanyIdAndSupplierId(Guid companyId, int? id)
        {
            string sqlQuery = @"select  Name
                                from  Supplier  
                                where CompanyId = '{0}'
                                and Id = {1}";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId, id);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetAllSupplierListByCompanyId(Guid companyId)
        {
            string sqlQuery = @"select sp.*, emp.FirstName + ' ' + emp.LastName as ContactPersonName from Supplier sp
                                left join Employee emp on emp.UserId = sp.ContactPerson
                                where sp.CompanyId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetAllSupplierListByCompanyIdForExport(Guid companyId)
        {
            string sqlQuery = @"select sp.Companyname,sp.EmailAddress,sp.SalesRepName,sp.Street,sp.Zipcode,sp.City,sp.State,dbo.PhoneNumFormat(sp.Phone),sp.TaxId, emp.FirstName + ' ' + emp.LastName as ContactPersonName from Supplier sp
                                left join Employee emp on emp.UserId = sp.ContactPerson
                                where sp.CompanyId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetAllSupplierName()
        {
            string sqlQuery = @"select sp.SupplierId,  IIF(sp.Name is null or sp.Name = '',sp.CompanyName,sp.Name) as Name, sp.CompanyName from Supplier sp";
            try
            {
                sqlQuery = string.Format(sqlQuery);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
