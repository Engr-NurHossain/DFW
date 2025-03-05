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
	public partial class EquipmentsFavouriteDataAccess
	{
        public EquipmentsFavouriteDataAccess(string ConStr) : base(ConStr) { }

        public DataTable GetAllEquipmentsFavouriteByUserIdAndCompanyId(Guid companyid, Guid userid)
        {
            string sqlQuery = @"select eqp.Name, eqp.SKU, eqp.Retail, eqp.Id, eqp.EquipmentId, eqp.Comments from Equipment eqp
                                left join EquipmentsFavourite ef on ef.EquipmentId = eqp.EquipmentId
                                where ef.IsFavourite = 1
                                and ef.CompanyId = '{0}'
                                and ef.UserId = '{1}'";
            try
            {
                sqlQuery = string.Format(sqlQuery, companyid, userid);
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
