using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HS.Alarm.AlarmOrder;
namespace HS.Alarm.OrderManager
{
    public class AlarmOrderManager
    {
        public HardwarePartInformation[] GetPartList(string username,string password)
        {
            OrderManagement om = new OrderManagement();
            om.AuthenticationValue = new Authentication()
            {
                User = username,
                Password = password
            };
            return om.GetPartList();
        }
    }
}
