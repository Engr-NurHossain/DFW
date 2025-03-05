using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HS.Alarm.AlarmCustomer;
using HS.Entities;

namespace HS.Alarm.CustomerManager
{
    public class AlarmCreateCustomer
    {
        public CreateCustomerOutput CreateCustomer(SetupAlarm Model)
        //public CreateCustomerOutput CreateCustomer(SetupAlarm Model)
        {
            
            CreateCustomerInput ci = new CreateCustomerInput
            {
                PropertyType = PropertyTypeEnum.SingleFamilyHouse,
                CustomerAccountAddress = new AddressWithName
                {
                    FirstName = Model.FirstName,//"Jhon",
                    LastName = Model.LastName,//"Sam",
                    CountryId = CountryEnum.USA,
                    City = Model.City,//"New York",
                    Street1 = Model.Street1, //"Jones Street, Manhattan",
                    State = Model.State, //"ca",
                    Zip = Model.Zip //"90001"
                },

                CustomerAccountEmail = Model.EmailAddress,//"samjhon@gmail.com",
                CustomerAccountPhone = Model.Phone, //"2025250117",

                DealerCustomerId = Model.DealerCustomer,//"123456",

                DesiredLoginName = Model.LoginName, //"jonsam",
                DesiredPassword = Model.Password,//"123Abcdef",
                InstallationAddress = new Address
                {
                    CountryId = CountryEnum.USA,
                    City = Model.InsCity, //" New York",
                    Street1 =  Model.InsStreet,//"Jones Street, Manhattan",
                    State = Model.InsState,//"ca",
                    Zip = Model.InsZip//"90001"
                },
                InstallationTimeZone = TimeZoneEnum.Pacific,
                //Culture = CultureEnum.English_US, //init below
                //PanelType = PanelTypeEnum.TwoG, //init below
                //PanelVersion = PanelVersionEnum.TwoGig112, //init below
                ModemSerialNumber = Model.ModelSerialNumber, //"113179515122147",
                PhoneLinePresent = (Model.PhoneLinePresent.HasValue && Model.PhoneLinePresent.Value)?true:false,// false,
                CentralStationForwardingOption = CentralStationForwardingOptionEnum.Always,
                CentralStationAccountNumber = Model.CentralStationAccountNo, //"8999",//flv12/flv7
                CentralStationReceiverNumber = Model.CentralStationRecieverNumber, //"855-432-3890",
                PackageId = Model.PackageId,//"8999",
                IgnoreLowCoverageErrors = (Model.IgnoreLowCoverageError.HasValue && Model.IgnoreLowCoverageError.Value) ? true : false//true
            };
            
            #region PanelVersionEnumOpt
            if (Model.PanelVersion == "Other")
            {
                ci.PanelVersion = PanelVersionEnum.Other;
            }
            else if (Model.PanelVersion == "Unknown")
            {
                ci.PanelVersion = PanelVersionEnum.Unknown;
            }
            else if (Model.PanelVersion == "Concord3")
            {
                ci.PanelVersion = PanelVersionEnum.Concord3;
            }
            else if (Model.PanelVersion == "Concord4")
            {
                ci.PanelVersion = PanelVersionEnum.Concord4;
            }
            else if (Model.PanelVersion == "ConcordExpress")
            {
                ci.PanelVersion = PanelVersionEnum.ConcordExpress;
            }
            else if (Model.PanelVersion == "ConcordMonitronicsExpress")
            {
                ci.PanelVersion = PanelVersionEnum.ConcordMonitronicsExpress;
            }
            else if (Model.PanelVersion == "Concord311")
            {
                ci.PanelVersion = PanelVersionEnum.Concord311;
            }
            else if (Model.PanelVersion == "Concord312")
            {
                ci.PanelVersion = PanelVersionEnum.Concord312;
            }
            else if (Model.PanelVersion == "SimonXt10")
            {
                ci.PanelVersion = PanelVersionEnum.SimonXt10;
            }
            else if (Model.PanelVersion == "SimonXt12")
            {
                ci.PanelVersion = PanelVersionEnum.SimonXt12;
            }
            else if (Model.PanelVersion == "SimonXt11")
            {
                ci.PanelVersion = PanelVersionEnum.SimonXt11;
            }
            else if (Model.PanelVersion == "SimonXt13")
            {
                ci.PanelVersion = PanelVersionEnum.SimonXt13;
            }
            else if (Model.PanelVersion == "SimonXt14")
            {
                ci.PanelVersion = PanelVersionEnum.SimonXt14;
            }
            else if (Model.PanelVersion == "SimonXt14French")
            {
                ci.PanelVersion = PanelVersionEnum.SimonXt14French;
            }
            else if (Model.PanelVersion == "SimonXti")
            {
                ci.PanelVersion = PanelVersionEnum.SimonXti;
            }
            else if (Model.PanelVersion == "SimonXt14Spanish")
            {
                ci.PanelVersion = PanelVersionEnum.SimonXt14Spanish;
            }
            else if (Model.PanelVersion == "SimonXt14Portuguese")
            {
                ci.PanelVersion = PanelVersionEnum.SimonXt14Portuguese;
            }
            else if (Model.PanelVersion == "SimonXti17")
            {
                ci.PanelVersion = PanelVersionEnum.SimonXti17;
            }
            else if (Model.PanelVersion == "SimonXt16")
            {
                ci.PanelVersion = PanelVersionEnum.SimonXt16;
            }
            else if (Model.PanelVersion == "SimonXt16French")
            {
                ci.PanelVersion = PanelVersionEnum.SimonXt16French;
            }
            else if (Model.PanelVersion == "SimonXt16Spanish")
            {
                ci.PanelVersion = PanelVersionEnum.SimonXt16Spanish;
            }
            else if (Model.PanelVersion == "SimonXt16Portuguese")
            {
                ci.PanelVersion = PanelVersionEnum.SimonXt16Portuguese;
            }
            else if (Model.PanelVersion == "Simon32")
            {
                ci.PanelVersion = PanelVersionEnum.Simon32;
            }
            else if (Model.PanelVersion == "Simon33")
            {
                ci.PanelVersion = PanelVersionEnum.Simon33;
            }
            else if (Model.PanelVersion == "Simon34")
            {
                ci.PanelVersion = PanelVersionEnum.Simon34;
            }
            else if (Model.PanelVersion == "Simon35")
            {
                ci.PanelVersion = PanelVersionEnum.Simon35;
            }
            else if (Model.PanelVersion == "Simon36")
            {
                ci.PanelVersion = PanelVersionEnum.Simon36;
            }
            else if (Model.PanelVersion == "Simon37")
            {
                ci.PanelVersion = PanelVersionEnum.Simon37;
            }
            else if (Model.PanelVersion == "Simon38")
            {
                ci.PanelVersion = PanelVersionEnum.Simon38;
            }
            else if (Model.PanelVersion == "Simon39")
            {
                ci.PanelVersion = PanelVersionEnum.Simon39;
            }
            else if (Model.PanelVersion == "Simon40")
            {
                ci.PanelVersion = PanelVersionEnum.Simon40;
            }
            else if (Model.PanelVersion == "Simon41")
            {
                ci.PanelVersion = PanelVersionEnum.Simon41;
            }
            else if (Model.PanelVersion == "Simon42")
            {
                ci.PanelVersion = PanelVersionEnum.Simon42;
            }
            else if (Model.PanelVersion == "Simon43")
            {
                ci.PanelVersion = PanelVersionEnum.Simon43;
            }
            else if (Model.PanelVersion == "Simon44")
            {
                ci.PanelVersion = PanelVersionEnum.Simon44;
            }
            else if (Model.PanelVersion == "NX1")
            {
                ci.PanelVersion = PanelVersionEnum.NX1;
            }
            else if (Model.PanelVersion == "NX8e")
            {
                ci.PanelVersion = PanelVersionEnum.NX8e;
            }
            else if (Model.PanelVersion == "NX4v2")
            {
                ci.PanelVersion = PanelVersionEnum.NX4v2;
            }
            else if (Model.PanelVersion == "NX6v2")
            {
                ci.PanelVersion = PanelVersionEnum.NX6v2;
            }
            else if (Model.PanelVersion == "NX8v2")
            {
                ci.PanelVersion = PanelVersionEnum.NX8v2;
            }
            else if (Model.PanelVersion == "NX1EU")
            {
                ci.PanelVersion = PanelVersionEnum.NX1EU;
            }
            else if (Model.PanelVersion == "NX8eEU")
            {
                ci.PanelVersion = PanelVersionEnum.NX8eEU;
            }
            else if (Model.PanelVersion == "NX4v2EU")
            {
                ci.PanelVersion = PanelVersionEnum.NX4v2EU;
            }
            else if (Model.PanelVersion == "NX6v2EU")
            {
                ci.PanelVersion = PanelVersionEnum.NX6v2EU;
            }
            else if (Model.PanelVersion == "NX8v2EU")
            {
                ci.PanelVersion = PanelVersionEnum.NX8v2EU;
            }
            else if (Model.PanelVersion == "ElkGuard1")
            {
                ci.PanelVersion = PanelVersionEnum.ElkGuard1;
            }
            else if (Model.PanelVersion == "ElkGuard2")
            {
                ci.PanelVersion = PanelVersionEnum.ElkGuard2;
            }
            else if (Model.PanelVersion == "TwoGigPreRelease")
            {
                ci.PanelVersion = PanelVersionEnum.TwoGigPreRelease;
            }
            else if (Model.PanelVersion == "TwoGig12")
            {
                ci.PanelVersion = PanelVersionEnum.TwoGig12;
            }
            else if (Model.PanelVersion == "TwoGig13")
            {
                ci.PanelVersion = PanelVersionEnum.TwoGig13;
            }
            else if (Model.PanelVersion == "TwoGig14")
            {
                ci.PanelVersion = PanelVersionEnum.TwoGig14;
            }
            else if (Model.PanelVersion == "TwoGig15")
            {
                ci.PanelVersion = PanelVersionEnum.TwoGig15;
            }
            else if (Model.PanelVersion == "TwoGig16")
            {
                ci.PanelVersion = PanelVersionEnum.TwoGig16;
            }
            else if (Model.PanelVersion == "TwoGig18")
            {
                ci.PanelVersion = PanelVersionEnum.TwoGig18;
            }
            else if (Model.PanelVersion == "TwoGig18French")
            {
                ci.PanelVersion = PanelVersionEnum.TwoGig18French;
            }
            else if (Model.PanelVersion == "TwoGig19")
            {
                ci.PanelVersion = PanelVersionEnum.TwoGig19;
            }
            else if (Model.PanelVersion == "TwoGig19French")
            {
                ci.PanelVersion = PanelVersionEnum.TwoGig19French;
            }
            else if (Model.PanelVersion == "TwoGig19Spanish")
            {
                ci.PanelVersion = PanelVersionEnum.TwoGig19Spanish;
            }
            else if (Model.PanelVersion == "TwoGig110")
            {
                ci.PanelVersion = PanelVersionEnum.TwoGig110;
            }
            else if (Model.PanelVersion == "TwoGig110French")
            {
                ci.PanelVersion = PanelVersionEnum.TwoGig110French;
            }
            else if (Model.PanelVersion == "TwoGig110Spanish")
            {
                ci.PanelVersion = PanelVersionEnum.TwoGig110Spanish;
            }
            else if (Model.PanelVersion == "TwoGig112")
            {
                ci.PanelVersion = PanelVersionEnum.TwoGig112;
            }
            else if (Model.PanelVersion == "TwoGig112French")
            {
                ci.PanelVersion = PanelVersionEnum.TwoGig112French;
            }
            else if (Model.PanelVersion == "TwoGig112Spanish")
            {
                ci.PanelVersion = PanelVersionEnum.TwoGig112Spanish;
            }
            else if (Model.PanelVersion == "TwoGig112Portuguese")
            {
                ci.PanelVersion = PanelVersionEnum.TwoGig112Portuguese;
            }
            else if (Model.PanelVersion == "TwoGig113")
            {
                ci.PanelVersion = PanelVersionEnum.TwoGig113;
            }
            else if (Model.PanelVersion == "TwoGig113Spanish")
            {
                ci.PanelVersion = PanelVersionEnum.TwoGig113Spanish;
            }
            else if (Model.PanelVersion == "TwoGig113French")
            {
                ci.PanelVersion = PanelVersionEnum.TwoGig113French;
            }
            else if (Model.PanelVersion == "TwoGig113Portuguese")
            {
                ci.PanelVersion = PanelVersionEnum.TwoGig113Portuguese;
            }
            else if (Model.PanelVersion == "TwoGig113Turkish")
            {
                ci.PanelVersion = PanelVersionEnum.TwoGig113Turkish;
            }
            else if (Model.PanelVersion == "TwoGig114")
            {
                ci.PanelVersion = PanelVersionEnum.TwoGig114;
            }
            else if (Model.PanelVersion == "TwoGig114Spanish")
            {
                ci.PanelVersion = PanelVersionEnum.TwoGig114Spanish;
            }
            else if (Model.PanelVersion == "TwoGig114French")
            {
                ci.PanelVersion = PanelVersionEnum.TwoGig114French;
            }
            else if (Model.PanelVersion == "TwoGig114Portuguese")
            {
                ci.PanelVersion = PanelVersionEnum.TwoGig114Portuguese;
            }
            else if (Model.PanelVersion == "TwoGig114Turkish")
            {
                ci.PanelVersion = PanelVersionEnum.TwoGig114Turkish;
            }
            else if (Model.PanelVersion == "TwoGig116")
            {
                ci.PanelVersion = PanelVersionEnum.TwoGig116;
            }
            else if (Model.PanelVersion == "TwoGig116Spanish")
            {
                ci.PanelVersion = PanelVersionEnum.TwoGig116Spanish;
            }
            else if (Model.PanelVersion == "TwoGig116French")
            {
                ci.PanelVersion = PanelVersionEnum.TwoGig116French;
            }
            else if (Model.PanelVersion == "TwoGig116Portuguese")
            {
                ci.PanelVersion = PanelVersionEnum.TwoGig116Portuguese;
            }
            else if (Model.PanelVersion == "TwoGig116Turkish")
            {
                ci.PanelVersion = PanelVersionEnum.TwoGig116Turkish;
            }
            else if (Model.PanelVersion == "TwoGig117")
            {
                ci.PanelVersion = PanelVersionEnum.TwoGig117;
            }
            else if (Model.PanelVersion == "TwoGig117Spanish")
            {
                ci.PanelVersion = PanelVersionEnum.TwoGig117Spanish;
            }
            else if (Model.PanelVersion == "TwoGig117French")
            {
                ci.PanelVersion = PanelVersionEnum.TwoGig117French;
            }
            else if (Model.PanelVersion == "TwoGig117Portuguese")
            {
                ci.PanelVersion = PanelVersionEnum.TwoGig117Portuguese;
            }
            else if (Model.PanelVersion == "TwoGig117Turkish")
            {
                ci.PanelVersion = PanelVersionEnum.TwoGig117Turkish;
            }
            else if (Model.PanelVersion == "TwoGig118")
            {
                ci.PanelVersion = PanelVersionEnum.TwoGig118;
            }
            else if (Model.PanelVersion == "TouchlinkPreRelease")
            {
                ci.PanelVersion = PanelVersionEnum.TouchlinkPreRelease;
            }
            else if (Model.PanelVersion == "Pointhub")
            {
                ci.PanelVersion = PanelVersionEnum.Pointhub;
            }
            else if (Model.PanelVersion == "IQPanel")
            {
                ci.PanelVersion = PanelVersionEnum.IQPanel;
            }
            else if (Model.PanelVersion == "IQPanel141")
            {
                ci.PanelVersion = PanelVersionEnum.IQPanel141;
            }
            else if (Model.PanelVersion == "IQPanel142")
            {
                ci.PanelVersion = PanelVersionEnum.IQPanel142;
            }
            else if (Model.PanelVersion == "IQPanel143")
            {
                ci.PanelVersion = PanelVersionEnum.IQPanel143;
            }
            else if (Model.PanelVersion == "IQPanel150")
            {
                ci.PanelVersion = PanelVersionEnum.IQPanel150;
            }
            else if (Model.PanelVersion == "IQPanel151")
            {
                ci.PanelVersion = PanelVersionEnum.IQPanel151;
            }
            else if (Model.PanelVersion == "IQPanel152")
            {
                ci.PanelVersion = PanelVersionEnum.IQPanel152;
            }
            else if (Model.PanelVersion == "IQPanel160")
            {
                ci.PanelVersion = PanelVersionEnum.IQPanel160;
            }
            else if (Model.PanelVersion == "IQPanel161")
            {
                ci.PanelVersion = PanelVersionEnum.IQPanel161;
            }
            else if (Model.PanelVersion == "IQPanel162")
            {
                ci.PanelVersion = PanelVersionEnum.IQPanel162;
            }
            else if (Model.PanelVersion == "IQPanel153")
            {
                ci.PanelVersion = PanelVersionEnum.IQPanel153;
            }
            else if (Model.PanelVersion == "DSCTouchPanel143")
            {
                ci.PanelVersion = PanelVersionEnum.DSCTouchPanel143;
            }
            else if (Model.PanelVersion == "DSCTouchPanel150")
            {
                ci.PanelVersion = PanelVersionEnum.DSCTouchPanel150;
            }
            else if (Model.PanelVersion == "DSCTouchPanel151")
            {
                ci.PanelVersion = PanelVersionEnum.DSCTouchPanel151;
            }
            else if (Model.PanelVersion == "DSCTouchPanel152")
            {
                ci.PanelVersion = PanelVersionEnum.DSCTouchPanel152;
            }
            else if (Model.PanelVersion == "DSCTouchPanel153")
            {
                ci.PanelVersion = PanelVersionEnum.DSCTouchPanel153;
            }
            else if (Model.PanelVersion == "DSCTouchPanel160")
            {
                ci.PanelVersion = PanelVersionEnum.DSCTouchPanel160;
            }
            else if (Model.PanelVersion == "DSCTouchPanel161")
            {
                ci.PanelVersion = PanelVersionEnum.DSCTouchPanel161;
            }
            else if (Model.PanelVersion == "DSCTouchPanel162")
            {
                ci.PanelVersion = PanelVersionEnum.DSCTouchPanel162;
            }
            else if (Model.PanelVersion == "IQPanel163")
            {
                ci.PanelVersion = PanelVersionEnum.IQPanel163;
            }
            else if (Model.PanelVersion == "DSCTouchPanel163")
            {
                ci.PanelVersion = PanelVersionEnum.DSCTouchPanel163;
            }
            else if (Model.PanelVersion == "Building36")
            {
                ci.PanelVersion = PanelVersionEnum.Building36;
            }
            else if (Model.PanelVersion == "AdvisorPreRelease")
            {
                ci.PanelVersion = PanelVersionEnum.AdvisorPreRelease;
            }
            else if (Model.PanelVersion == "Impassa")
            {
                ci.PanelVersion = PanelVersionEnum.Impassa;
            }
            else if (Model.PanelVersion == "Impassa131")
            {
                ci.PanelVersion = PanelVersionEnum.Impassa131;
            }
            else if (Model.PanelVersion == "NeoHS2016")
            {
                ci.PanelVersion = PanelVersionEnum.NeoHS2016;
            }
            else if (Model.PanelVersion == "NeoHS2032")
            {
                ci.PanelVersion = PanelVersionEnum.NeoHS2032;
            }
            else if (Model.PanelVersion == "NeoHS2064")
            {
                ci.PanelVersion = PanelVersionEnum.NeoHS2064;
            }
            else if (Model.PanelVersion == "NeoHS2128")
            {
                ci.PanelVersion = PanelVersionEnum.NeoHS2128;
            }
            else if (Model.PanelVersion == "Neo12HS2016")
            {
                ci.PanelVersion = PanelVersionEnum.Neo12HS2016;
            }
            else if (Model.PanelVersion == "Neo12HS2032")
            {
                ci.PanelVersion = PanelVersionEnum.Neo12HS2032;
            }
            else if (Model.PanelVersion == "Neo12HS2064")
            {
                ci.PanelVersion = PanelVersionEnum.Neo12HS2064;
            }
            else if (Model.PanelVersion == "Neo12HS2128")
            {
                ci.PanelVersion = PanelVersionEnum.Neo12HS2128;
            }
            else if (Model.PanelVersion == "Neo13HS2016")
            {
                ci.PanelVersion = PanelVersionEnum.Neo13HS2016;
            }
            else if (Model.PanelVersion == "Neo13HS2032")
            {
                ci.PanelVersion = PanelVersionEnum.Neo13HS2032;
            }
            else if (Model.PanelVersion == "Neo13HS2064")
            {
                ci.PanelVersion = PanelVersionEnum.Neo13HS2064;
            }
            else if (Model.PanelVersion == "Neo13HS2128")
            {
                ci.PanelVersion = PanelVersionEnum.Neo13HS2128;
            }
            else if (Model.PanelVersion == "GoControl3")
            {
                ci.PanelVersion = PanelVersionEnum.GoControl3;
            }
            else if (Model.PanelVersion == "GoControl301")
            {
                ci.PanelVersion = PanelVersionEnum.GoControl301;
            }
            else if (Model.PanelVersion == "GoControl310")
            {
                ci.PanelVersion = PanelVersionEnum.GoControl310;
            }
            else if (Model.PanelVersion == "GoControl302")
            {
                ci.PanelVersion = PanelVersionEnum.GoControl302;
            }
            else if (Model.PanelVersion == "GoControl311")
            {
                ci.PanelVersion = PanelVersionEnum.GoControl311;
            }
            else if (Model.PanelVersion == "GoControl320")
            {
                ci.PanelVersion = PanelVersionEnum.GoControl320;
            }
            else if (Model.PanelVersion == "GoControl312")
            {
                ci.PanelVersion = PanelVersionEnum.GoControl312;
            }
            else if (Model.PanelVersion == "NucleusPreRelease")
            {
                ci.PanelVersion = PanelVersionEnum.NucleusPreRelease;
            }
            else if (Model.PanelVersion == "SEMVista15P")
            {
                ci.PanelVersion = PanelVersionEnum.SEMVista15P;
            }
            else if (Model.PanelVersion == "SEMVista20P")
            {
                ci.PanelVersion = PanelVersionEnum.SEMVista20P;
            }
            else if (Model.PanelVersion == "SEMVista10P")
            {
                ci.PanelVersion = PanelVersionEnum.SEMVista10P;
            }
            else if (Model.PanelVersion == "SEMVista15PUD")
            {
                ci.PanelVersion = PanelVersionEnum.SEMVista15PUD;
            }
            else if (Model.PanelVersion == "SEMVista20PUD")
            {
                ci.PanelVersion = PanelVersionEnum.SEMVista20PUD;
            }
            else if (Model.PanelVersion == "SEMVista10PUD")
            {
                ci.PanelVersion = PanelVersionEnum.SEMVista10PUD;
            }
            else if (Model.PanelVersion == "SEMPowerSeriesPC1616")
            {
                ci.PanelVersion = PanelVersionEnum.SEMPowerSeriesPC1616;
            }
            else if (Model.PanelVersion == "SEMPowerSeriesPC1832")
            {
                ci.PanelVersion = PanelVersionEnum.SEMPowerSeriesPC1832;
            }
            else if (Model.PanelVersion == "SEMPowerSeriesPC1864")
            {
                ci.PanelVersion = PanelVersionEnum.SEMPowerSeriesPC1864;
            }
            else if (Model.PanelVersion == "SEMPowerSeriesPC1808")
            {
                ci.PanelVersion = PanelVersionEnum.SEMPowerSeriesPC1808;
            }
            else if (Model.PanelVersion == "Panel19HP1")
            {
                ci.PanelVersion = PanelVersionEnum.Panel19HP1;
            }
            else if (Model.PanelVersion == "Panel19HM1")
            {
                ci.PanelVersion = PanelVersionEnum.Panel19HM1;
            }
            else if (Model.PanelVersion == "Panel19HP11")
            {
                ci.PanelVersion = PanelVersionEnum.Panel19HP11;
            }
            else if (Model.PanelVersion == "Panel19HM11")
            {
                ci.PanelVersion = PanelVersionEnum.Panel19HM11;
            }
            else if (Model.PanelVersion == "Panel19HS11")
            {
                ci.PanelVersion = PanelVersionEnum.Panel19HS11;
            }
            else if (Model.PanelVersion == "IQPanel2Release")
            {
                ci.PanelVersion = PanelVersionEnum.IQPanel2Release;
            }
            else if (Model.PanelVersion == "IQPanel203")
            {
                ci.PanelVersion = PanelVersionEnum.IQPanel203;
            }
            else if (Model.PanelVersion == "IQPanel210")
            {
                ci.PanelVersion = PanelVersionEnum.IQPanel210;
            }
            else if (Model.PanelVersion == "IQPanel202")
            {
                ci.PanelVersion = PanelVersionEnum.IQPanel202;
            }
            else if (Model.PanelVersion == "IQPanel201")
            {
                ci.PanelVersion = PanelVersionEnum.IQPanel201;
            }
            else if (Model.PanelVersion == "IQPanel204")
            {
                ci.PanelVersion = PanelVersionEnum.IQPanel204;
            }
            else if (Model.PanelVersion == "IQPanel205")
            {
                ci.PanelVersion = PanelVersionEnum.IQPanel205;
            }
            else if (Model.PanelVersion == "IQPanel206")
            {
                ci.PanelVersion = PanelVersionEnum.IQPanel206;
            }
            else if (Model.PanelVersion == "Vario557")
            {
                ci.PanelVersion = PanelVersionEnum.Vario557;
            }
            else if (Model.PanelVersion == "IotegaPreRelease")
            {
                ci.PanelVersion = PanelVersionEnum.IotegaPreRelease;
            }
            else if (Model.PanelVersion == "OneLinkPreRelease")
            {
                ci.PanelVersion = PanelVersionEnum.OneLinkPreRelease;
            }
            else if (Model.PanelVersion == "Panel24AIO")
            {
                ci.PanelVersion = PanelVersionEnum.Panel24AIO;
            }
            else if (Model.PanelVersion == "Panel24Hub")
            {
                ci.PanelVersion = PanelVersionEnum.Panel24Hub;
            }
            else if (Model.PanelVersion == "Panel24Hybrid")
            {
                ci.PanelVersion = PanelVersionEnum.Panel24Hybrid;
            }
            #endregion

            #region CultureEnumOpt
            if (Model.Culture == "Unknown")
            {
                ci.Culture = CultureEnum.Unknown;
            }
            else if (Model.Culture == "English_US")
            {
                ci.Culture = CultureEnum.English_US;
            }
            else if (Model.Culture == "French_Canada")
            {
                ci.Culture = CultureEnum.French_Canada;
            }
            else if (Model.Culture == "Spanish_US")
            {
                ci.Culture = CultureEnum.Spanish_US;
            }
            else if (Model.Culture == "Portuguese_Brazil")
            {
                ci.Culture = CultureEnum.Portuguese_Brazil;
            }
            else if (Model.Culture == "Spanish_Mexico")
            {
                ci.Culture = CultureEnum.Spanish_Mexico;
            }
            else if (Model.Culture == "English_SouthAfrica")
            {
                ci.Culture = CultureEnum.English_SouthAfrica;
            }
            else if (Model.Culture == "Spanish_Chile")
            {
                ci.Culture = CultureEnum.Spanish_Chile;
            }
            else if (Model.Culture == "Spanish_Argentina")
            {
                ci.Culture = CultureEnum.Spanish_Argentina;
            }
            else if (Model.Culture == "Spanish_Colombia")
            {
                ci.Culture = CultureEnum.Spanish_Colombia;
            }
            else if (Model.Culture == "English_NewZealand")
            {
                ci.Culture = CultureEnum.English_NewZealand;
            }
            else if (Model.Culture == "Spanish_Panama")
            {
                ci.Culture = CultureEnum.Spanish_Panama;
            }
            else if (Model.Culture == "Spanish_CostaRica")
            {
                ci.Culture = CultureEnum.Spanish_CostaRica;
            }
            else if (Model.Culture == "Spanish_Venezuela")
            {
                ci.Culture = CultureEnum.Spanish_Venezuela;
            }
            else if (Model.Culture == "English_TrinidadTobago")
            {
                ci.Culture = CultureEnum.English_TrinidadTobago;
            }
            else if (Model.Culture == "Spanish_Ecuador")
            {
                ci.Culture = CultureEnum.Spanish_Ecuador;
            }
            else if (Model.Culture == "Turkish_Turkey")
            {
                ci.Culture = CultureEnum.Turkish_Turkey;
            }
            else if (Model.Culture == "English_Jamaica")
            {
                ci.Culture = CultureEnum.English_Jamaica;
            }
            else if (Model.Culture == "English_Caribbean")
            {
                ci.Culture = CultureEnum.English_Caribbean;
            }
            else if (Model.Culture == "English_UK")
            {
                ci.Culture = CultureEnum.English_UK;
            }
            else if (Model.Culture == "SpanishSpain")
            {
                ci.Culture = CultureEnum.SpanishSpain;
            }
            else if (Model.Culture == "DutchNetherlands")
            {
                ci.Culture = CultureEnum.DutchNetherlands;
            }
            else if (Model.Culture == "FrenchFrance")
            {
                ci.Culture = CultureEnum.FrenchFrance;
            }
            else if (Model.Culture == "NorwegianBokmalNorway")
            {
                ci.Culture = CultureEnum.NorwegianBokmalNorway;
            }
            else if (Model.Culture == "PortuguesePortugal")
            {
                ci.Culture = CultureEnum.PortuguesePortugal;
            }
            else if (Model.Culture == "SwedishSweden")
            {
                ci.Culture = CultureEnum.SwedishSweden;
            }
            else if (Model.Culture == "EnglishCanada")
            {
                ci.Culture = CultureEnum.EnglishCanada;
            }
            else if (Model.Culture == "IcelandicIceland")
            {
                ci.Culture = CultureEnum.IcelandicIceland;
            }
            else if (Model.Culture == "HebrewIsrael")
            {
                ci.Culture = CultureEnum.HebrewIsrael;
            }
            else if (Model.Culture == "TestNumeric")
            {
                ci.Culture = CultureEnum.TestNumeric;
            }
            #endregion

            #region PanelTypeEnumOpt
            if (Model.PanelType == "NotSet")
            {
                ci.PanelType = PanelTypeEnum.NotSet;
            }
            else if (Model.PanelType == "Concord")
            {
                ci.PanelType = PanelTypeEnum.Concord;
            }
            else if (Model.PanelType == "Simon")
            {
                ci.PanelType = PanelTypeEnum.Simon;
            }
            else if (Model.PanelType == "NX")
            {
                ci.PanelType = PanelTypeEnum.NX;
            }
            else if (Model.PanelType == "Greybox")
            {
                ci.PanelType = PanelTypeEnum.Greybox;
            }
            else if (Model.PanelType == "ElkGuard")
            {
                ci.PanelType = PanelTypeEnum.ElkGuard;
            }
            else if (Model.PanelType == "TwoG")
            {
                ci.PanelType = PanelTypeEnum.TwoG;
            }
            else if (Model.PanelType == "Touchlink")
            {
                ci.PanelType = PanelTypeEnum.Touchlink;
            }
            else if (Model.PanelType == "PointHub")
            {
                ci.PanelType = PanelTypeEnum.PointHub;
            }
            else if (Model.PanelType == "IQPanel")
            {
                ci.PanelType = PanelTypeEnum.IQPanel;
            }
            else if (Model.PanelType == "Building36")
            {
                ci.PanelType = PanelTypeEnum.Building36;
            }
            else if (Model.PanelType == "Advisor")
            {
                ci.PanelType = PanelTypeEnum.Advisor;
            }
            else if (Model.PanelType == "Impassa")
            {
                ci.PanelType = PanelTypeEnum.Impassa;
            }
            else if (Model.PanelType == "Neo")
            {
                ci.PanelType = PanelTypeEnum.Neo;
            }
            else if (Model.PanelType == "GoControl3")
            {
                ci.PanelType = PanelTypeEnum.GoControl3;
            }
            else if (Model.PanelType == "Nucleus")
            {
                ci.PanelType = PanelTypeEnum.Nucleus;
            }
            else if (Model.PanelType == "SEMVista")
            {
                ci.PanelType = PanelTypeEnum.SEMVista;
            }
            else if (Model.PanelType == "SEMPowerSeries")
            {
                ci.PanelType = PanelTypeEnum.SEMPowerSeries;
            }
            else if (Model.PanelType == "Panel19")
            {
                ci.PanelType = PanelTypeEnum.Panel19;
            }
            else if (Model.PanelType == "IQPanel2")
            {
                ci.PanelType = PanelTypeEnum.IQPanel2;
            }
            else if (Model.PanelType == "Vario")
            {
                ci.PanelType = PanelTypeEnum.Vario;
            }
            else if (Model.PanelType == "Iotega")
            {
                ci.PanelType = PanelTypeEnum.Iotega;
            }
            else if (Model.PanelType == "OneLink")
            {
                ci.PanelType = PanelTypeEnum.OneLink;
            }
            else if (Model.PanelType == "Panel24")
            {
                ci.PanelType = PanelTypeEnum.Panel24;
            }
            else if (Model.PanelType == "NoPanel")
            {
                ci.PanelType = PanelTypeEnum.NoPanel;
            }
            #endregion

            #region TimeZoneEnumOpt
            //if (Model.InsTimeZone == "NotSet")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.NotSet;
            //}
            //else if (Model.InsTimeZone == "Eastern")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.Eastern;
            //}
            //else if (Model.InsTimeZone == "Central")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.Central;
            //}
            //else if (Model.InsTimeZone == "Mountain")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.Mountain;
            //}
            //else if (Model.InsTimeZone == "Pacific")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.Pacific;
            //}
            //else if (Model.InsTimeZone == "Alaska")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.Alaska;
            //}
            //else if (Model.InsTimeZone == "Hawaii")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.Hawaii;
            //}
            //else if (Model.InsTimeZone == "WestSamoa")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.WestSamoa;
            //}
            //else if (Model.InsTimeZone == "AtlanticNoDST")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.AtlanticNoDST;
            //}
            //else if (Model.InsTimeZone == "Guam")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.Guam;
            //}
            //else if (Model.InsTimeZone == "Palau")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.Palau;
            //}
            //else if (Model.InsTimeZone == "Arizona")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.Arizona;
            //}
            //else if (Model.InsTimeZone == "Newfoundland")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.Newfoundland;
            //}
            //else if (Model.InsTimeZone == "Atlantic")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.Atlantic;
            //}
            //else if (Model.InsTimeZone == "EasternNoDST")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.EasternNoDST;
            //}
            //else if (Model.InsTimeZone == "CentralNoDST")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.CentralNoDST;
            //}
            //else if (Model.InsTimeZone == "HawaiiAleutian")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.HawaiiAleutian;
            //}
            //else if (Model.InsTimeZone == "WakeIsland")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.WakeIsland;
            //}
            //else if (Model.InsTimeZone == "Pohnpei")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.Pohnpei;
            //}
            //else if (Model.InsTimeZone == "Brasilia")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.Brasilia;
            //}
            //else if (Model.InsTimeZone == "CentralBrazilian")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.CentralBrazilian;
            //}
            //else if (Model.InsTimeZone == "Amazon")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.Amazon;
            //}
            //else if (Model.InsTimeZone == "BraziliaNoDST")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.BraziliaNoDST;
            //}
            //else if (Model.InsTimeZone == "Fernando")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.Fernando;
            //}
            //else if (Model.InsTimeZone == "Muchosransk")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.Muchosransk;
            //}
            //else if (Model.InsTimeZone == "CentralMexico")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.CentralMexico;
            //}
            //else if (Model.InsTimeZone == "MountainMexico")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.MountainMexico;
            //}
            //else if (Model.InsTimeZone == "MountainMexicoNoDST")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.MountainMexicoNoDST;
            //}
            //else if (Model.InsTimeZone == "PacificMexico")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.PacificMexico;
            //}
            //else if (Model.InsTimeZone == "SouthAfrican")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.SouthAfrican;
            //}
            //else if (Model.InsTimeZone == "Chile")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.Chile;
            //}
            //else if (Model.InsTimeZone == "EasterIsland")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.EasterIsland;
            //}
            //else if (Model.InsTimeZone == "Argentina")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.Argentina;
            //}
            //else if (Model.InsTimeZone == "Colombia")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.Colombia;
            //}
            //else if (Model.InsTimeZone == "NewZealand")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.NewZealand;
            //}
            //else if (Model.InsTimeZone == "Turkey")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.Turkey;
            //}
            //else if (Model.InsTimeZone == "CentralAfrica")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.CentralAfrica;
            //}
            //else if (Model.InsTimeZone == "Venezuela")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.Venezuela;
            //}
            //else if (Model.InsTimeZone == "Peru")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.Peru;
            //}
            //else if (Model.InsTimeZone == "Ecuador")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.Ecuador;
            //}
            //else if (Model.InsTimeZone == "AustralianEastern")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.AustralianEastern;
            //}
            //else if (Model.InsTimeZone == "AustralianCentral")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.AustralianCentral;
            //}
            //else if (Model.InsTimeZone == "ChristmasIsland")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.ChristmasIsland;
            //}
            //else if (Model.InsTimeZone == "Norfolk")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.Norfolk;
            //}
            //else if (Model.InsTimeZone == "AustralianWestern")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.AustralianWestern;
            //}
            //else if (Model.InsTimeZone == "GMT")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.GMT;
            //}
            //else if (Model.InsTimeZone == "CentralEuropean")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.CentralEuropean;
            //}
            //else if (Model.InsTimeZone == "WesternEuropean")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.WesternEuropean;
            //}
            //else if (Model.InsTimeZone == "GMTNoDST")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.GMTNoDST;
            //}
            //else if (Model.InsTimeZone == "UruguayTime")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.UruguayTime;
            //}
            //else if (Model.InsTimeZone == "ParaguayTime")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.ParaguayTime;
            //}
            //else if (Model.InsTimeZone == "BoliviaTime")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.BoliviaTime;
            //}
            //else if (Model.InsTimeZone == "WestAfricaTime")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.WestAfricaTime;
            //}
            //else if (Model.InsTimeZone == "AustralianEasternNoDst")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.AustralianEasternNoDst;
            //}
            //else if (Model.InsTimeZone == "AustralianCentralNoDst")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.AustralianCentralNoDst;
            //}
            //else if (Model.InsTimeZone == "IndiaStandardTime")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.IndiaStandardTime;
            //}
            //else if (Model.InsTimeZone == "EasternMexico")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.EasternMexico;
            //}
            //else if (Model.InsTimeZone == "IndochinaTime")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.IndochinaTime;
            //}
            //else if (Model.InsTimeZone == "Fiji")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.Fiji;
            //}
            //else if (Model.InsTimeZone == "GreenwichMeanTimeIreland")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.GreenwichMeanTimeIreland;
            //}
            //else if (Model.InsTimeZone == "MalaysiaTime")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.MalaysiaTime;
            //}
            //else if (Model.InsTimeZone == "TestTimezone")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.TestTimezone;
            //}
            //else if (Model.InsTimeZone == "GulfStandardTime")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.GulfStandardTime;
            //}
            //else if (Model.InsTimeZone == "PhilippineTime")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.PhilippineTime;
            //}
            //else if (Model.InsTimeZone == "EasternEuropeanTime")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.EasternEuropeanTime;
            //}
            //else if (Model.InsTimeZone == "JapanStandardTime")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.JapanStandardTime;
            //}
            //else if (Model.InsTimeZone == "ArabStandardTime")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.ArabStandardTime;
            //}
            //else if (Model.InsTimeZone == "IsraelStandardTime")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.IsraelStandardTime;
            //}
            //else if (Model.InsTimeZone == "SingaporeTime")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.SingaporeTime;
            //}
            //else if (Model.InsTimeZone == "CentralEuropeanTime")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.CentralEuropeanTime;
            //}
            //else if (Model.InsTimeZone == "GTBStandardTime")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.GTBStandardTime;
            //}
            //else if (Model.InsTimeZone == "GuyanaTime")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.GuyanaTime;
            //}
            //else if (Model.InsTimeZone == "CentralTimeBelize")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.CentralTimeBelize;
            //}
            //else if (Model.InsTimeZone == "EasternEuropeanTimeFinland")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.EasternEuropeanTimeFinland;
            //}
            //else if (Model.InsTimeZone == "WesternIndonesianTime")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.WesternIndonesianTime;
            //}
            //else if (Model.InsTimeZone == "CentralIndonesianTime")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.CentralIndonesianTime;
            //}
            //else if (Model.InsTimeZone == "CentralIndonesianTime")
            //{
            //    ci.InstallationTimeZone = TimeZoneEnum.EasternIndonesianTime;
            //}
            #endregion



            CustomerManagement cm = new CustomerManagement();
            Authentication auth = new Authentication()
            {
                User = Model.AuthUser,
                Password = Model.AuthPass
            };

            cm.AuthenticationValue = auth;

            return  cm.CreateCustomer(ci); 
            
        }
    }
}
