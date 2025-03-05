using Elmah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.DataAccess
{
    public class HsErrorLog
    {
        public static void AddElmah(string message )
        {
            ErrorSignal.FromCurrentContext().Raise(new Exception(message));
        }
        public static void AddElmah(Exception message)
        {
            ErrorSignal.FromCurrentContext().Raise(message);
        }
    }
}
