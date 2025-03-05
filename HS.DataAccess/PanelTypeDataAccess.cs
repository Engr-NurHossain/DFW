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
	public partial class PanelTypeDataAccess
	{
        public DataTable GetPanelTypeListByCompanyId(Guid CompanyId)
        {
            string sqlQuery = @"select pt.*, 
CASE WHEN TRY_PARSE(pt.[Value] AS int USING 'sr-Latn-CS') > 0  
        THEN (select [Name] from Equipment where [Id] = pt.[Value])  
        ELSE ''  
  End as EquipName
from PanelType pt
                                
where pt.CompanyId = '{0}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, CompanyId);
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
