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
    public partial class CustomerPackageServiceDataAccess
    {
        public CustomerPackageServiceDataAccess(string ConStr) : base(ConStr) { }
        public bool DeleteCustomerPackageServiceByCompanyIdCustomerId(Guid CompanyId, Guid CustomerId)
        {
            string SqlQuery = @"Delete from CustomerPackageService
                                where CompanyId='{0}' AND CustomerId='{1}'";
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
        public bool DeleteOnlyCustomerPackageServiceByCompanyIdCustomerId(Guid CompanyId, Guid CustomerId)
        {
            string SqlQuery = @"Delete from CustomerPackageService
                                where CompanyId='{0}' AND CustomerId='{1}' AND IsPackageService = 1";
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
        public DataTable GetCustomerPackageServiceById(int id)
        {
            string sqlQuery = @"select cpe.*,
                                eq.Name as EquipmentServiceName,
                                ISNULL(manu.[Name],'') as Manufacturer,
                                ISNULL(loc.[Name],'') as [Location],
                                ISNULL(typ.[Name],'') as [Type],
                                ISNULL(model.[Name],'') as Model,
                                ISNULL(finish.[Name],'') as Finish,
                                ISNULL(capacity.[Name],'') as Capacity
                                from CustomerPackageService cpe
                                LEFT JOIN Equipment eq on eq.EquipmentId=cpe.EquipmentId

                                left join Manufacturer manu on cpe.ManufacturerId = manu.ManufacturerId
                                left join ServiceDetailInfo loc on loc.ServiceInfoId = cpe.LocationId 
                                    and loc.[Type] = 'Location' 
                                    and cpe.EquipmentId = loc.ServiceId
                                left join ServiceDetailInfo typ on typ.ServiceInfoId = cpe.TypeId 
                                    and typ.[Type] = 'Type' 
                                    and cpe.EquipmentId = typ.ServiceId
                                left join ServiceDetailInfo model on model.ServiceInfoId = cpe.ModelId 
                                    and model.[Type] = 'Model' 
                                    and cpe.EquipmentId = model.ServiceId
                                left join ServiceDetailInfo finish on finish.ServiceInfoId = cpe.FinishId 
                                    and finish.[Type] = 'Finish' 
                                    and cpe.EquipmentId = finish.ServiceId
                                left join ServiceDetailInfo capacity on capacity.ServiceInfoId = cpe.CapacityId 
                                    and capacity.[Type] = 'Capacity' 
                                    and cpe.EquipmentId = capacity.ServiceId

                                where cpe.Id='{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, id);
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
