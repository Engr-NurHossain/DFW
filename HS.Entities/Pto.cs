using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
	public partial class Pto 
	{
        public string CreatedByVal { set; get; }
        public double TotalMinute { get; set; }
        public string TimeFromVal { set; get; }
        public string TimeToVal { set; get; }

        public int PTOMinutes { set; get; }
        public int RequestedMinutes { get; set; }
        public string LeaveType { get; set; }
        public string PtoRemain { get; set; }
        public string RequestedByVal { get; set; }
        #region Date Perpouse
        private string _StrStartDate;
        public string StrStartDate
        {
            get
            {
                return _StrStartDate;
            }
            set
            {
                _StrStartDate = value;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    var values = value.Trim().Split('/');
                    if (values.Length == 3)
                    {
                        int Day = 0;
                        int Month = 0;
                        int Year = 0;
                        if (int.TryParse(values[0], out Month) && Month > 0 && Month < 13
                            && int.TryParse(values[1], out Day) && Day > 0 && Day < 32
                            && int.TryParse(values[2], out Year) && Year > 1980)
                        {
                            this.StartDate = new DateTime(Year, Month, Day);
                        }
                    }
                }
            }
        }
        private string _StrEndDate;
        public string StrEndDate
        {
            get
            {
                return _StrEndDate;
            }
            set
            {
                _StrEndDate = value;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    var values = value.Trim().Split('/');
                    if (values.Length == 3)
                    {
                        int Day = 0;
                        int Month = 0;
                        int Year = 0;
                        if (int.TryParse(values[0], out Month) && Month > 0 && Month < 13
                            && int.TryParse(values[1], out Day) && Day > 0 && Day < 32
                            && int.TryParse(values[2], out Year) && Year > 2017)
                        {
                            this.EndDate = new DateTime(Year, Month, Day);
                        }
                    }
                }
            }
        }
        #endregion


    }

    public class PtoFilterModel
    {
        public List<Pto> ListPto { get; set; }
        public TotalCountPto TotalCountPto { get; set; }
    }

    public class TotalCountPto
    {
        public int CountTotal { get; set; }
    }
}
