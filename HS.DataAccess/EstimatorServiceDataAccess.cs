using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.DataAccess;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;
using System.Linq;

namespace HS.DataAccess
{
	public partial class EstimatorServiceDataAccess
    {
        public EstimatorServiceDataAccess(string ConStr) : base(ConStr) { }
        public bool DeleteEstimatorServiceByEstimatorId(string estimatorId)
		{
            string SqlQuery = @"delete from EstimatorService where EstimatorId ='{0}' and IsOneTimeService = 0 OR  IsOneTimeService is null";
            SqlQuery = string.Format(SqlQuery, estimatorId);
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

        public bool DeleteEstimatorOneTimeServiceByEstimatorId(string estimatorId)
        {
            string SqlQuery = @"delete from EstimatorService where EstimatorId ='{0}' and IsOneTimeService = 1";
            SqlQuery = string.Format(SqlQuery, estimatorId);
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
