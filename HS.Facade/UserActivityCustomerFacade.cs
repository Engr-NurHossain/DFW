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
    public class UserActivityCustomerFacade : BaseFacade
    {
        public UserActivityCustomerFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        UserActivityCustomerDataAccess _UserActivityCustomerDataAccess
        {
            get
            {
                return (UserActivityCustomerDataAccess)_ClientContext[typeof(UserActivityCustomerDataAccess)];
            }
        }
        public void AddUserActivityCustomer(UserActivityCustomer UserActivityCustomer)
        {
            //UserActivityCustomer.UserAgent = AppConfig.GetUserAgent;
            //UserActivityCustomer.UserIp = AppConfig.GetIP;
            //UserActivityCustomer.StatsDate = DateTime.Now;
            _UserActivityCustomerDataAccess.Insert(UserActivityCustomer);
        }
        public void AddElmah(string message)
        {
            HsErrorLog.AddElmah(message);
        }

        public long InsertUserActivityCustomer(UserActivityCustomer ua)
        {
            return _UserActivityCustomerDataAccess.Insert(ua);
        }

        public UserActivityCustomer GetUserActivityCustomerByLoginAction(string username)
        {
            return _UserActivityCustomerDataAccess.GetByQuery(string.Format("Action = 'LogIn' and UserName = '{0}' order by Id desc offset 1 ROWS", username)).FirstOrDefault();
        }
    }
}
