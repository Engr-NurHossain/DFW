using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.Entities
{
    public partial class SetupAlarm
    {
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string CentralStationName { set; get; }

        public string City { set; get; }
        public string Street { set; get; }
        public string State { set; get; }
        public string Zip { set; get; }

        public DateTime? InstallationDate { set; get; }
        public string InstallerUserName { set; get; }
        public string Network { set; get; }

        public string AlarmRefId { set; get; }
        public string Action { set; get; }

        public string AuthUser { set; get; }
        public string AuthPass { set; get; }
        public string SalesRepName { set; get; }
        public string InstallerLoginName { set; get; }
        public bool SameInsAddress { get; set; }
        public string CentralStationForwardingOption { get; set; }
        public string CompanyName { get; set; }
        public string LoginPassword { get; set; }
        public string[] ForwardedEvents { get; set; }
        public int?[] adonitem { get; set; }

        public string CustomerType { get; set; }
        public bool? IsContractSigned { get; set; }        

    }
    public class AlamcredentialsSetting
    {
        public List<AlamcredentialsSetting> alarmcredentialssettings { get; set; }
        public string AlarmUsername { get; set; }
        public string AlarmPassword { get; set; }

        public string BrinksAlarmUsername { get; set; }

        public string BrinksAlarmPassword { get; set; }

        public bool Hasmultiplevalue { get; set; }

       
    }
    public class FeaturesReturnModel
    {
        public List<AddOns> Features { get; set; }
        public List<AddOns> Addons { get; set; }
    }
    public class AddOns
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
    public class PackageDataList
    {
        public string Display { get; set; }
        public string Value { get; set; }
        public bool Checked { get; set; }
    }
}
