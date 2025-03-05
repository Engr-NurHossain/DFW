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
	public partial class EstimatorFileDataAccess
	{
		public EstimatorFileDataAccess(string ConStr) : base(ConStr) { }
        public bool DeleteEstimatorFileByEstimatorId(string EstimatorId,string EstimatorType)
        {
            string SqlQuery = @"
                                Delete from EstimatorFile
                                where EstimatorId = '{0}'  and EstimatorType = '{1}'";
            SqlQuery = string.Format(SqlQuery, EstimatorId,EstimatorType);
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
    }	
}