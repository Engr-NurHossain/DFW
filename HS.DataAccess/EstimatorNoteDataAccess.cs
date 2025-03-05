using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.DataAccess
{
	public partial class EstimatorNoteDataAccess
	{
        //public DataTable GetEstimatorDetailListByEstimatorIdForPrint(string estimatorId)
        //{
        //          string sqlQuery = @"   
        //                 select *, S.CompanyName as SupplierVal ,ET.Name as CategoryVal from 
        //                 EstimatorDetail ED  left join
        //                  Supplier S
        //                   on  ED.SupplierId = S.SupplierId
        //             left join EquipmentType ET on ED.CategoryId = ET.Id
        //                  where ED.EstimatorId = '{0}'";
        //          try
        //          {
        //              sqlQuery = string.Format(sqlQuery, estimatorId);
        //              using (SqlCommand cmd = GetSQLCommand(sqlQuery))
        //              {
        //                  DataSet dsResult = GetDataSet(cmd);
        //                  return dsResult.Tables[0];
        //              }
        //          }
        //          catch (Exception ex)
        //          {
        //              return null;
        //          }

        //      }
        public DataTable NewGetAllEstimatorNoteByEstimatorIdAndCompanyId(string estimatorId)
        {
            string sqlQuery = @"   
                    select estdetl.*,eqip.SKU from EstimatorDetail estdetl
                    left join Equipment eqip on estdetl.EquipmentId = eqip.EquipmentId
                    Where estdetl.EstimatorId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery,estimatorId);
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
        public DataTable GetAllEstimatorNoteByEstimatorIdAndCompanyId(int estimatorId, Guid companyId)
        {
            string sqlQuery = @"   
                    select En.*, 
                    emp.FirstName + ' ' +emp.LastName as AddedByText
                    from EstimatorNote En
                    left join Employee emp on emp.UserId = en.AddedBy
                    where En.CompanyId = '{0}' and En.EstimatorId = '{1}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyId, estimatorId);
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
        public DataTable GetAllSuplierIdByEstimatorId(string estimatorId)
        {
            string sqlQuery = @"   
                    select ed.SupplierId from EstimatorDetail ed 
                    where ed.EstimatorId= '{0}'
                    group by ed.SupplierId";
            try
            {
                sqlQuery = string.Format(sqlQuery, estimatorId);
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
