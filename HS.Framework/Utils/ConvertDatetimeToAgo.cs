using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HS.Framework.Utils
{
    public static class ConvertDatetimeToAgo
    {
        public static DateTime TimeAgo(this DateTime datetime)
        {
            //string Timestamp = "";
            //TimeSpan span = DateTime.Now - dt;
            //if (span.Days > 365)
            //{
            //    int years = (span.Days / 365);
            //    if (span.Days % 365 != 0)
            //        years += 1;
            //    Timestamp += String.Format("{0} {1}",
            //    years, years == 1 ? "year" : "years");
            //}
            //if (span.Days > 30)
            //{
            //    int months = (span.Days / 30);
            //    if (span.Days % 31 != 0)
            //    Timestamp += months += 1;
            //    return String.Format(" {0} {1}",
            //    months, months == 1 ? "month" : "months");
            //}
            //if (span.Days > 0)
            //    Timestamp += String.Format(" {0} {1}",
            //    span.Days, span.Days == 1 ? "day" : "days");
            //if (span.Hours > 0)
            //    Timestamp += String.Format(" {0} {1}",
            //    span.Hours, span.Hours == 1 ? "hour" : "hours");
            //if (span.Minutes > 0)
            //    Timestamp += String.Format(" {0} {1}",
            //    span.Minutes, span.Minutes == 1 ? "minute" : "minutes");
            //if (span.Seconds > 5)
            //    Timestamp += String.Format(" {0} seconds", span.Seconds);
            //if (span.Seconds <= 5)
            //    return "just now";

            //if(Timestamp == "")
            //{
            //    return string.Empty;
            //}
            //else
            //{
            //    return "about" + Timestamp + " ago";
            //}
            var CurrentTimeZone = "";
            var CurrentDate = "";
            var CurrentTime = "";
            TimeZoneInfo currentTimeZone = TimeZoneInfo.Local;
            if (HttpContext.Current.Request.Cookies[CookieKeys.CurrentUser] != null)
            {
                var CookieCurrentUser = HttpContext.Current.Request.Cookies[CookieKeys.CurrentUser].Value;
                if (CookieCurrentUser != null)
                {
                    string[] splitCurrentUser = CookieCurrentUser.Split('&');
                    CurrentTimeZone = splitCurrentUser[0];
                    var ctz = CurrentTimeZone.Split('=');
                    CurrentDate = splitCurrentUser[1];
                    CurrentTime = splitCurrentUser[2];
                    var TimeZoneId = ctz[1];
                    var usertzname = splitCurrentUser[3].Split('=');
                    var usertznameid = "";
                    var ctzid = splitCurrentUser[3].Split('=');
                    var Hourstocount = Convert.ToInt32(TimeZoneId) * -1;
                    datetime = datetime.AddMinutes(Hourstocount);
                   
                }
            }
            return datetime;
        }
    }
}
