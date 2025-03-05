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
	public partial class EquipmentTypeDataAccess
	{
        public DataTable GetAllEquipmentTypeCategoryWithSubCategoryByCompanyId(Guid companyid)
        {
            string sqlQuery = @"
                                with Equipment_Data as 
                                (
                                    select ET.Id, cast(ET.Name as nvarchar(max)) as CategoryName
                                    from EquipmentType as ET
                                    where ET.Id in (select Id from EquipmentType where CompanyId = '{0}' and IsActive = 1 and ParentId IS NULL)                                
                                    union all                                
                                    select Child.Id, ED.CategoryName + ' -> ' + cast(Child.Name as nvarchar(max))
                                    from EquipmentType as Child
                                      inner join Equipment_Data as ED on ED.Id = Child.ParentId
                                    where ED.Id != Child.Id and Child.CompanyId = '{0}' and Child.IsActive = 1 and Child.ParentId IS NOT NULL
                                )
                                select Id, CategoryName from Equipment_Data
                                ";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyid);
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

        public DataTable GetAllEquipmentTypeCategoryWithSubCategoryByUserFilter(Guid companyId)
        {
            string sqlQuery = @"                                
                                with Equipment_Data as 
                                (
                                    select ET.Id, ET.Name, ET.ParentId, cast(ET.Id as nvarchar(max)) as CategoryId
                                    from EquipmentType as ET
                                    where ET.Id in (select Id from EquipmentType where CompanyId = '{0}' and IsActive = 1 and ParentId IS NULL)                
                                    union all                                
                                    select Child.Id, Child.Name, Child.ParentId, ED.CategoryId + '>' + cast(Child.Id as nvarchar(max))
                                    from EquipmentType as Child
                                    join Equipment_Data as ED on ED.Id = Child.ParentId
                                    where ED.Id != Child.Id and Child.CompanyId = '{0}' and Child.IsActive = 1 and Child.ParentId IS NOT NULL
                                )
                                select * from Equipment_Data
                                ";
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
    }	
}
