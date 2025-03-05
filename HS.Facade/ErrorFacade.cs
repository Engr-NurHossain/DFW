using HS.DataAccess;
using HS.Entities;
using HS.Entities.Bases;
using HS.Framework;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Facade
{
    #region Digiture

    public class ErrorFacade : BaseFacade
    {
        public ErrorFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }

        ErrorLogDataAccess _ErrorLogDataAccess
        {
            get
            {
                return (ErrorLogDataAccess)_ClientContext[typeof(ErrorLogDataAccess)];
            }
        }

        public long InsertErrorLog(ErrorLog errLog)
        {
            long result = 0;
            try
            {
                result = _ErrorLogDataAccess.Insert(errLog);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return result;
        }
    }

    #endregion Digiture

}
