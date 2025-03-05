using HS.Alarm.AlarmDealer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HS.Alarm.DealerManager
{
    public class AlarmDealerManager
    {
        public GetPackageIdsOutput GetBasicPackages(String UserName,string Password)
        {

            var delermgmt = new DealerManagement();
            delermgmt.AuthenticationValue = new Authentication()
            {
                User = UserName,
                Password = Password
            };
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var packageId = delermgmt.GetPackageIds(new GetPackageIdsInput()
            {
                accountType = AccountTypeEnum.Security,
                CallerVersion = 1

            });

           

            return packageId;
        }
        public GetPackageIdsOutput GetVideoPackages(String UserName, string Password)
        {

            var delermgmt = new DealerManagement();
            delermgmt.AuthenticationValue = new Authentication()
            {
                User = UserName,
                Password = Password
            };
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var packageId = delermgmt.GetPackageIds(new GetPackageIdsInput()
            {
                accountType = AccountTypeEnum.Video,
                CallerVersion = 1

            });



            return packageId;
        }
        public GetPackageIdsOutput GetEnergyAutomationPackages(String UserName, string Password)
        {

            var delermgmt = new DealerManagement();
            delermgmt.AuthenticationValue = new Authentication()
            {
                User = UserName,
                Password = Password
            };
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var packageId = delermgmt.GetPackageIds(new GetPackageIdsInput()
            {
                accountType = AccountTypeEnum.EnergyAutomation,
                CallerVersion = 1

            });



            return packageId;
        }

        public GetPackageIdsOutput GetEnergyPointCentralPackages(String UserName, string Password)
        {

            var delermgmt = new DealerManagement();
            delermgmt.AuthenticationValue = new Authentication()
            {
                User = UserName,
                Password = Password
            };
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var packageId = delermgmt.GetPackageIds(new GetPackageIdsInput()
            {
                accountType = AccountTypeEnum.PointCentral,
                CallerVersion = 1

            });



            return packageId;
        }
        public GetPackageIdsOutput GetEnergyBeClosePackages(String UserName, string Password)
        {

            var delermgmt = new DealerManagement();
            delermgmt.AuthenticationValue = new Authentication()
            {
                User = UserName,
                Password = Password
            };
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var packageId = delermgmt.GetPackageIds(new GetPackageIdsInput()
            {
                accountType = AccountTypeEnum.BeClose,
                CallerVersion = 1

            });



            return packageId;
        }

        public Rep[] GetRepList()
        {
            var Reps = new DealerManagement();
            Reps.AuthenticationValue = new Authentication()
            {
                User = "RABSecurityWebservices",
                Password = "MaishaM@2"
            };
            var asd = Reps.GetRepList();
            return asd;
        }
    }
}
