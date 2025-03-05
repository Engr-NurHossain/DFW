using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;
using System.Linq;

namespace HS.DataAccess
{
	public partial class EstimatorDetailDataAccess
	{
        public EstimatorDetailDataAccess(string ConStr) : base(ConStr) { }
        public EstimatorDetailDataAccess() { }
        public bool DeleteEstimatorDetailsByEstimatorId(string estimatorId)
        {
            string SqlQuery = @"delete from EstimatorDetail where EstimatorId ='{0}' ";
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
