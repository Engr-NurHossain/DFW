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
    public partial class CustomerPackageEqpDataAccess
    {
        public CustomerPackageEqpDataAccess(string ConStr) : base(ConStr) { }
        public bool DeleteCustomerPackageEqpByCompanyIdCustomerId(Guid CompanyId, Guid CustomerId)
        {
            string SqlQuery = @"Delete from CustomerPackageEqp
                                where CompanyId='{0}' AND CustomerId='{1}'
                                AND (IsIncluded=1 OR IsDevice=1 OR IsOptionalEqp=1 OR IsServiceEquipment =1)";
            SqlQuery = string.Format(SqlQuery, CompanyId,CustomerId);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(SqlQuery))
                {
                    ExecuteCommand(cmd);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool DeleteAllCustomerPackageEqpByCompanyIdCustomerId(Guid CompanyId, Guid CustomerId)
        {
            string SqlQuery = @"Delete from CustomerPackageEqp
                                where CompanyId='{0}' AND CustomerId='{1}' ";
            SqlQuery = string.Format(SqlQuery, CompanyId, CustomerId);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(SqlQuery))
                {
                    ExecuteCommand(cmd);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public bool DeleteOnlyCustomerPackageEqpByCompanyIdCustomerId(Guid CompanyId, Guid CustomerId)
        {
            string SqlQuery = @"Delete from CustomerPackageEqp
                                where CompanyId='{0}' AND CustomerId='{1}' AND IsPackageEqp =1";
            SqlQuery = string.Format(SqlQuery, CompanyId, CustomerId);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(SqlQuery))
                {
                    ExecuteCommand(cmd);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool DeleteCustomerPackageEqpByCustomerIdServiceIdAndPackageId(Guid customerId, Guid equipmentId, Guid packageId)
        {
            string SqlQuery = @"Delete from CustomerPackageEqp
                                where CustomerId ='{0}' 
                                    and PackageId = '{2}'
                                    and ServiceId = '{1}'";
            SqlQuery = string.Format(SqlQuery, customerId, equipmentId, packageId);
            try
            {
                using (SqlCommand cmd = GetSQLCommand(SqlQuery))
                {
                    ExecuteCommand(cmd);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public DataTable GetAllCustomerPackageEqpByCustomerId(Guid CustomerId)
        {
            string sqlQuery = @"Select cusEqp.*,eqp.Name as EquipmentName,eqp.Point as Point
                                
                                from CustomerPackageEqp cusEqp
                                LEFT JOIN Equipment eqp on eqp.EquipmentId=cusEqp.EquipmentId
                                where cusEqp.CustomerId='{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, CustomerId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataTable GetCustomerPackageServiceByCustomerId(Guid CustomerId)
        {
            string sqlQuery = @"select cps.*, eqp.[Name] as EquipmentName from CustomerPackageService cps
                                left join Equipment eqp on eqp.EquipmentId = cps.EquipmentId
                                where cps.CustomerId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, CustomerId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    DataSet dsResult = GetDataSet(cmd);
                    return dsResult.Tables[0];
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
