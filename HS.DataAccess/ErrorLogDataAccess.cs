using System;
using System.Data;
using System.Data.SqlClient;
using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Text.RegularExpressions;

namespace HS.DataAccess
{
	public partial class ErrorLogDataAccess
	{

        public ErrorLogDataAccess(string ConnectionString) : base(ConnectionString) { }

        public ErrorLogDataAccess() { }

        #region Digiture

        public long LogError(ErrorLog err)
        {
            long Id = 0;
            string sqlQuery = @"

                                INSERT INTO [dbo].[ErrorLog]   
                                ( [ErrorId], [ErrorFor], [Message], [TimeUtc] )   
                                VALUES   
                                (  
                                '{0}',  
                                '{1}',  
                                '{2}',  
                                '{3}'
                                )  
                                    ";
            try
            {
                err.Message = err.Message.Replace("'", "`");
                err.Message = Regex.Replace(err.Message, @"\s+", " ");
                sqlQuery = string.Format(sqlQuery,
                        err.ErrorId, err.ErrorFor, err.Message, err.TimeUtc);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    Id = InsertRecord(cmd);
                }
            }
            catch (Exception ex)
            {

            }
            return Id;
        }

        #endregion Digiture

    }
}
