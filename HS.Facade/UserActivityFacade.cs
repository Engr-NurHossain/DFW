using HS.DataAccess;
using HS.Entities;
using HS.Framework;
using HS.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Facade
{
    public class UserActivityFacade : BaseFacade
    {
        public UserActivityFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        UserActivityDataAccess _UserActivityDataAccess
        {
            get
            {
                return (UserActivityDataAccess)_ClientContext[typeof(UserActivityDataAccess)];
            }
        }
        public void AddUserActivity(UserActivity userActivity)
        {
            userActivity.UserAgent = AppConfig.GetUserAgent;
            userActivity.UserIp = AppConfig.GetIP;
            userActivity.StatsDate = DateTime.UtcNow;
            _UserActivityDataAccess.Insert(userActivity);
        }
        public void AddElmah(string message)
        {
            HsErrorLog.AddElmah(message);
        }

        public long InsertUserActivity(UserActivity ua)
        {
            return _UserActivityDataAccess.Insert(ua);
        }

        public UserActivity GetUserActivityByLoginAction(string username)
        {

            return _UserActivityDataAccess.GetUserActivityByLoginAction(username);

            //return _UserActivityDataAccess.GetByQuery(string.Format("Action = 'LogIn' and UserName = '{0}' order by Id desc offset 1 ROWS", username)).FirstOrDefault();
        }
    }
}
