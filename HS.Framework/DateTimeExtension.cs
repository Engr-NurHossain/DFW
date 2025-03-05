using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace HS.Framework
{
    public static class DateTimeExtension
    {
        static Logger logger = LogManager.GetCurrentClassLogger();
        public static DateTime ClientToUTCTime(this DateTime datetime)
        {
            var CurrentTimeZone = "";
            var CurrentDate = "";
            var CurrentTime = "";
            TimeZoneInfo currentTimeZone = TimeZoneInfo.Local;
            DateTime LocalTime = new DateTime();
            string CookieCurrentUser = HttpContext.Current.Request.Cookies[CookieKeys.CurrentUser].Value;
            if (!string.IsNullOrWhiteSpace(CookieCurrentUser)) //&& datetime != new DateTime()
            {
                string[] splitCurrentUser = CookieCurrentUser.Split('&');
                CurrentTimeZone = splitCurrentUser[0];
                var ctz = CurrentTimeZone.Split('=');
                CurrentDate = splitCurrentUser[1];
                CurrentTime = splitCurrentUser[2];
                var TimeZoneId = ctz[1];
                var ctzid = splitCurrentUser[3].Split('=');
                var Hourstocount = Convert.ToInt32(TimeZoneId);
                datetime = datetime.AddMinutes(Hourstocount);
                //datetime.ToUniversalTime();
            }
            //if(datetime.Kind != DateTimeKind.Utc)
            //{
            //    int hour = LocalTime.Hour;
            //    int min = LocalTime.Minute;
            //    int sec = LocalTime.Second;
            //    DateTime EDateTime = new DateTime(datetime.Year, datetime.Month, datetime.Day, hour, min, sec);
            //    datetime = EDateTime.ToUniversalTime();
            //}
            //else
            //{
            //    datetime = datetime.ToUniversalTime();
            //}
            return datetime;
        }

        //public static DateTime GetLocalTime(this DateTime datetime)
        //{ 
        //    var CookieCurrentUser = HttpContext.Current.Request.Cookies[CookieKeys.CurrentUser].Value;
        //    if (!string.IsNullOrWhiteSpace(CookieCurrentUser))
        //    {
        //        string[] splitCurrentUser = CookieCurrentUser.Split('&');
        //        string CurrentTimeZone = splitCurrentUser[0];
        //        var ctz = CurrentTimeZone.Split('=');
        //        string CurrentDate = splitCurrentUser[1];
        //        string CurrentTime = splitCurrentUser[2];
        //        var TimeZoneId = ctz[1];
        //        var ctzid = splitCurrentUser[3].Split('=');
        //        var Hourstocount = Convert.ToInt32(TimeZoneId) * -1;
        //        datetime = DateTime.UtcNow.AddMinutes(Hourstocount);
        //    }
        //    return datetime;
        //}
        public static int UTCToClientTimeMin(this DateTime datetime)
        {
            var CurrentTimeZone = "";
            var CurrentDate = "";
            var CurrentTime = "";
            int offsetMins = 0;
            TimeZoneInfo currentTimeZone = TimeZoneInfo.Local;
            if (HttpContext.Current.Request.Cookies[CookieKeys.CurrentUser] != null)
            {
                try
                {
                    var CookieCurrentUser = HttpContext.Current.Request.Cookies[CookieKeys.CurrentUser].Value;
                    if (CookieCurrentUser != null)
                    {
                        string[] splitCurrentUser = CookieCurrentUser.Split('&');
                        CurrentTimeZone = splitCurrentUser[0];
                        var ctz = CurrentTimeZone.Split('=');
                        var TimeZoneId = ctz[1];
                        offsetMins = Convert.ToInt32(TimeZoneId) * -1;
                    }
                }
                catch (Exception e)
                {
                    return 0;
                }

            }
            else
            {
                var offset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
                offsetMins = offset.Hours * 60 + offset.Minutes;
            }

            return offsetMins;
        }
        public static DateTime UTCToClientTime(this DateTime datetime)
        {
            var CurrentTimeZone = "";
            var CurrentDate = "";
            var CurrentTime = "";
            TimeZoneInfo currentTimeZone = TimeZoneInfo.Local;
            if(HttpContext.Current.Request.Cookies[CookieKeys.CurrentUser] != null)
            {
                try
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
                        //int year = LocalTime.Year;
                        //int day = LocalTime.Day;
                        //int month = LocalTime.Month;
                        //if (!string.IsNullOrWhiteSpace(usertzname[1]))
                        //{
                        //    datetime = DateTime.SpecifyKind(datetime, DateTimeKind.Utc);
                        //    usertznameid = usertzname[1];
                        //    var infozone = TimeZoneInfo.FindSystemTimeZoneById(usertznameid);
                        //    DateTime datetime1 = TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(datetime, DateTimeKind.Utc), infozone);
                        //    datetime = new DateTime(year, month, day, datetime1.Hour, datetime1.Minute, datetime1.Second);
                        //}
                    }
                }catch(Exception e)
                {
                    //logger.Error(e);
                    return datetime;
                }
               
            }
            return datetime;
        }

        public static DateTime UTCToServerTime(this DateTime dateTime)
        {
            DateTime currentDate = DateTime.Now;
            TimeZone localZone = TimeZone.CurrentTimeZone;
            TimeSpan currentOffset = localZone.GetUtcOffset(currentDate);

            var Hourstocount = Convert.ToInt32(currentOffset.Hours * 60 + currentOffset.Minutes ) * -1;
            dateTime = dateTime.AddMinutes(Hourstocount);

            return dateTime;
        }
        public static DateTime StartOfWeek(this DateTime dt)
        {
            DateTime startweek = new DateTime();
            if(dt != null)
            {
                int weekday = dt.Day;
                string strDayOfWeek = dt.DayOfWeek.ToString();
                if(strDayOfWeek.ToLower() == "monday")
                {
                    weekday = weekday - 1;
                    int weekmonth = dt.Month;
                    int weekyear = dt.Year;
                    startweek = new DateTime(weekyear, weekmonth, weekday);
                }
                else if(strDayOfWeek.ToLower() == "tuesday")
                {
                    weekday = weekday - 2;
                    int weekmonth = dt.Month;
                    int weekyear = dt.Year;
                    startweek = new DateTime(weekyear, weekmonth, weekday);
                }
                else if (strDayOfWeek.ToLower() == "wednesday")
                {
                    weekday = weekday - 3;
                    int weekmonth = dt.Month;
                    int weekyear = dt.Year;
                    startweek = new DateTime(weekyear, weekmonth, weekday);
                }
                else if (strDayOfWeek.ToLower() == "thursday")
                {
                    weekday = weekday - 4;
                    int weekmonth = dt.Month;
                    int weekyear = dt.Year;
                    startweek = new DateTime(weekyear, weekmonth, weekday);
                }
                else if (strDayOfWeek.ToLower() == "friday")
                {
                    weekday = weekday - 5;
                    int weekmonth = dt.Month;
                    int weekyear = dt.Year;
                    startweek = new DateTime(weekyear, weekmonth, weekday);
                }
                else if (strDayOfWeek.ToLower() == "saturday")
                {
                    weekday = weekday - 6;
                    int weekmonth = dt.Month;
                    int weekyear = dt.Year;
                    startweek = new DateTime(weekyear, weekmonth, weekday);
                }
                else
                {
                    int weekmonth = dt.Month;
                    int weekyear = dt.Year;
                    startweek = new DateTime(weekyear, weekmonth, weekday);
                }
            }
            return startweek;
        }

        public static DateTime GetFirstDayOfWeek(this DateTime date)
        {
            var firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;

            while (date.DayOfWeek != firstDayOfWeek)
            {
                date = date.AddDays(-1);
            }

            return date;
        }
    }
}
