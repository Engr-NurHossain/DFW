using HS.Entities;
using HS.Framework.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using HS.Entities.Bases;

namespace HS.Tracker.API.Models
{
    public class Logging : BaseDataAccess
    {
        private const string INSERTERRORLOG = "InsertErrorLog";
        
        private void AddCommonParams(SqlCommand cmd, ErrorLogBase errorLogObject)
        {
            AddParameter(cmd, pGuid(ErrorLogBase.Property_ErrorId, errorLogObject.ErrorId));
            AddParameter(cmd, pNVarChar(ErrorLogBase.Property_ErrorFor, 600, errorLogObject.ErrorFor));
            AddParameter(cmd, pNVarChar(ErrorLogBase.Property_Message, errorLogObject.Message));
            AddParameter(cmd, pDateTime(ErrorLogBase.Property_TimeUtc, errorLogObject.TimeUtc));
        }

        public void AddLogging(ErrorLog errLog)
        {
            try
            {
                SqlCommand cmd = GetSPCommand(INSERTERRORLOG);

                AddParameter(cmd, pInt32Out(ErrorLogBase.Property_Id));
                AddCommonParams(cmd, (ErrorLogBase)errLog);

                long result = InsertRecord(cmd);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}