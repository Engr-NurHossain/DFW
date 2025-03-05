using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forte
{
    internal static class GetServerDetails
    {
        private static string serverName;
        public static string Geturl(ForteOptions createOptions)
        {
            if (string.IsNullOrEmpty(createOptions.Server))
            {
                serverName = "";
                switch (createOptions.Environment.ToString().ToUpper())
                {
                    case "LIVE":
                        serverName = "https://api.forte.net/API/v2";
                        break;

                    case "SANDBOX":
                        serverName = "https://sandbox.forte.net/API/v2";
                        break;

                    default:
                        serverName = "https://sandbox.forte.net/API/v2";
                        break;
                }
            }
            else if ((createOptions.Server.ToLower().IndexOf("http") == 0))
            {
                serverName = createOptions.Server;
            }

            return serverName;
        }
    }
}
