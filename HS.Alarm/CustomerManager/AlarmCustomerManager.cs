using HS.Alarm.AlarmCustomer;
using HS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using OctSol.Secure.Framework.Extensions;


namespace HS.Alarm.CustomerManager
{
    public class AlarmCustomerManager
    {
        public PanelDevice[] GetFullEquipmentList(int CustomerId)
        {
            CustomerManagement cm = new CustomerManagement();
            return cm.GetFullEquipmentList(CustomerId);
        }

        public bool TerminateCustomer(int CustomerId)
        {
            CustomerManagement cm = new CustomerManagement();
            return cm.TerminateCustomer(CustomerId, CustomerTerminateReasonEnum.NotUsingService);
        }

        public CheckCustomerLoginOutput CheckAvailableLoginName(string Email,string FirstName, string LastName,string Username,string Phone)
        {
            CustomerManagement cm = new CustomerManagement();
            return cm.CheckAvailableLoginName(new CheckCustomerLoginInput()
            {
                Email = Email,
                FirstName = FirstName,
                LastName = LastName,
                LoginName = Username,
                Phone = Phone
            });
        }

        public ActivateCommitmentOutput ActivateCommitment(int CommitmentId, string ModeMserialNumber, bool IsInstalledByExpectedInstaller, string PanelType)
        {
            ActivateCommitmentInput ACI = new ActivateCommitmentInput()
            {
                CommitmentId = CommitmentId,
                IgnoreLowCoverageErrors = true,
                ModemSerialNumber = ModeMserialNumber,
                IsInstalledByExpectedInstaller = true,
            };
            #region PanelTypeEnumOpt
            if (PanelType == "Concord")
            {
                ACI.PanelType = PanelTypeEnum.Concord;
            }
            else if (PanelType == "Simon")
            {
                ACI.PanelType = PanelTypeEnum.Simon;
            }
            else if (PanelType == "NX")
            {
                ACI.PanelType = PanelTypeEnum.NX;
            }
            else if (PanelType == "Greybox")
            {
                ACI.PanelType = PanelTypeEnum.Greybox;
            }
            else if (PanelType == "ElkGuard")
            {
                ACI.PanelType = PanelTypeEnum.ElkGuard;
            }
            else if (PanelType == "TwoG")
            {
                ACI.PanelType = PanelTypeEnum.TwoG;
            }
            else if (PanelType == "Touchlink")
            {
                ACI.PanelType = PanelTypeEnum.Touchlink;
            }
            else if (PanelType == "PointHub")
            {
                ACI.PanelType = PanelTypeEnum.PointHub;
            }
            else if (PanelType == "IQPanel")
            {
                ACI.PanelType = PanelTypeEnum.IQPanel;
            }
            else if (PanelType == "Building36")
            {
                ACI.PanelType = PanelTypeEnum.Building36;
            }
            else if (PanelType == "Advisor")
            {
                ACI.PanelType = PanelTypeEnum.Advisor;
            }
            else if (PanelType == "Impassa")
            {
                ACI.PanelType = PanelTypeEnum.Impassa;
            }
            else if (PanelType == "Neo")
            {
                ACI.PanelType = PanelTypeEnum.Neo;
            }
            else if (PanelType == "GoControl3")
            {
                ACI.PanelType = PanelTypeEnum.GoControl3;
            }
            else if (PanelType == "Nucleus")
            {
                ACI.PanelType = PanelTypeEnum.Nucleus;
            }
            else if (PanelType == "SEMVista")
            {
                ACI.PanelType = PanelTypeEnum.SEMVista;
            }
            else if (PanelType == "SEMPowerSeries")
            {
                ACI.PanelType = PanelTypeEnum.SEMPowerSeries;
            }
            
            else if (PanelType == "IQPanel2")
            {
                ACI.PanelType = PanelTypeEnum.IQPanel2;
            }
            else if (PanelType == "Vario")
            {
                ACI.PanelType = PanelTypeEnum.Vario;
            }
            else if (PanelType == "Iotega")
            {
                ACI.PanelType = PanelTypeEnum.Iotega;
            }
            else if (PanelType == "OneLink")
            {
                ACI.PanelType = PanelTypeEnum.OneLink;
            }
            
            else if (PanelType == "NoPanel")
            {
                ACI.PanelType = PanelTypeEnum.NoPanel;
            }
            else //if(PanelType == "NotSet")
            {
                ACI.PanelType = PanelTypeEnum.NotSet;
            }

            ACI.PanelType = PanelType.ToEnum<PanelTypeEnum>();
            #endregion

            //else if (PanelType == "Panel19")
            //{
            //    ACI.PanelType = PanelTypeEnum.Panel19;
            //}
            //else if (PanelType == "Panel24")
            //{
            //    ACI.PanelType = PanelTypeEnum.Panel24;
            //}

            CustomerManagement cm = new CustomerManagement();
            return cm.ActivateCommitment(ACI);

        }
        public CreateCustomerOutput CreateCommitment(SetupAlarm Model)
        {
            CreateCommitmentInput CCI = new CreateCommitmentInput()
            {
                CommitmentExpectedInfo = new CommitmentRelatedInfo()
                {
                    ExpectedInstallDate = Model.InstallationDate,
                    ExpectedInstallerLogin = Model.InstallerUserName,
                    //ExpectedNetwork = NetworkEnum.Verizon, init in bottom
                    //ExpectedPanelType =PanelTypeEnum.Advisor, init in bottom
                    ModemSerialNumber = Model.ModelSerialNumber
                },
                CustomerInput = new CreateCustomerInput()
                {
                    //PropertyType = PropertyTypeEnum.SingleFamilyHouse,
                    CustomerAccountAddress = new AddressWithName
                    {
                        FirstName = Model.FirstName,//"Jhon",
                        LastName = Model.LastName,//"Sam",
                        CountryId = CountryEnum.USA,
                        City = Model.City,//"New York",
                        Street1 = Model.Street, //"Jones Street, Manhattan",
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
                        Street1 = Model.InsStreet,//"Jones Street, Manhattan",
                        State = Model.InsState,//"ca",
                        Zip = Model.InsZip//"90001"
                    },
                    InstallationTimeZone = TimeZoneEnum.Pacific,
                    PhoneLinePresent = (Model.PhoneLinePresent.HasValue && Model.PhoneLinePresent.Value) ? true : false,// false,
                    CentralStationForwardingOption = CentralStationForwardingOptionEnum.Always,
                    CentralStationAccountNumber = Model.CentralStationAccountNo, //"8999",//flv12/flv7
                    CentralStationReceiverNumber = Model.CentralStationRecieverNumber, //"855-432-3890",
                    //PackageId = Model.PackageId,//"8999",
                    IgnoreLowCoverageErrors = (Model.IgnoreLowCoverageError.HasValue && Model.IgnoreLowCoverageError.Value) ? true : false//true
                }
            };

            #region PropertyType
            if (Model.PropertyType == "Business")
            {
                CCI.CustomerInput.PropertyType = PropertyTypeEnum.Business;
            }
            else if (Model.PropertyType == "Condo")
            {
                CCI.CustomerInput.PropertyType = PropertyTypeEnum.Condo;
            }
            else if (Model.PropertyType == "SingleFamilyHouse")
            {
                CCI.CustomerInput.PropertyType = PropertyTypeEnum.SingleFamilyHouse;
            }
            else if (Model.PropertyType == "Townhouse")
            {
                CCI.CustomerInput.PropertyType = PropertyTypeEnum.Townhouse;
            }
            else //if(Model.PropertyType == "NotSet")
            {
                CCI.CustomerInput.PropertyType = PropertyTypeEnum.NotSet;
            }
            #endregion

            #region PanelTypeEnumOpt
            if (Model.PanelType == "Concord")
            {
                CCI.CommitmentExpectedInfo.ExpectedPanelType = PanelTypeEnum.Concord;
            }
            else if (Model.PanelType == "Simon")
            {
                CCI.CommitmentExpectedInfo.ExpectedPanelType = PanelTypeEnum.Simon;
            }
            else if (Model.PanelType == "NX")
            {
                CCI.CommitmentExpectedInfo.ExpectedPanelType = PanelTypeEnum.NX;
            }
            else if (Model.PanelType == "Greybox")
            {
                CCI.CommitmentExpectedInfo.ExpectedPanelType = PanelTypeEnum.Greybox;
            }
            else if (Model.PanelType == "ElkGuard")
            {
                CCI.CommitmentExpectedInfo.ExpectedPanelType = PanelTypeEnum.ElkGuard;
            }
            else if (Model.PanelType == "TwoG")
            {
                CCI.CommitmentExpectedInfo.ExpectedPanelType = PanelTypeEnum.TwoG;
            }
            else if (Model.PanelType == "Touchlink")
            {
                CCI.CommitmentExpectedInfo.ExpectedPanelType = PanelTypeEnum.Touchlink;
            }
            else if (Model.PanelType == "PointHub")
            {
                CCI.CommitmentExpectedInfo.ExpectedPanelType = PanelTypeEnum.PointHub;
            }
            else if (Model.PanelType == "IQPanel")
            {
                CCI.CommitmentExpectedInfo.ExpectedPanelType = PanelTypeEnum.IQPanel;
            }
            else if (Model.PanelType == "Building36")
            {
                CCI.CommitmentExpectedInfo.ExpectedPanelType = PanelTypeEnum.Building36;
            }
            else if (Model.PanelType == "Advisor")
            {
                CCI.CommitmentExpectedInfo.ExpectedPanelType = PanelTypeEnum.Advisor;
            }
            else if (Model.PanelType == "Impassa")
            {
                CCI.CommitmentExpectedInfo.ExpectedPanelType = PanelTypeEnum.Impassa;
            }
            else if (Model.PanelType == "Neo")
            {
                CCI.CommitmentExpectedInfo.ExpectedPanelType = PanelTypeEnum.Neo;
            }
            else if (Model.PanelType == "GoControl3")
            {
                CCI.CommitmentExpectedInfo.ExpectedPanelType = PanelTypeEnum.GoControl3;
            }
            else if (Model.PanelType == "Nucleus")
            {
                CCI.CommitmentExpectedInfo.ExpectedPanelType = PanelTypeEnum.Nucleus;
            }
            else if (Model.PanelType == "SEMVista")
            {
                CCI.CommitmentExpectedInfo.ExpectedPanelType = PanelTypeEnum.SEMVista;
            }
            else if (Model.PanelType == "SEMPowerSeries")
            {
                CCI.CommitmentExpectedInfo.ExpectedPanelType = PanelTypeEnum.SEMPowerSeries;
            }
            
            else if (Model.PanelType == "IQPanel2")
            {
                CCI.CommitmentExpectedInfo.ExpectedPanelType = PanelTypeEnum.IQPanel2;
            }
            else if (Model.PanelType == "Vario")
            {
                CCI.CommitmentExpectedInfo.ExpectedPanelType = PanelTypeEnum.Vario;
            }
            else if (Model.PanelType == "Iotega")
            {
                CCI.CommitmentExpectedInfo.ExpectedPanelType = PanelTypeEnum.Iotega;
            }
            else if (Model.PanelType == "OneLink")
            {
                CCI.CommitmentExpectedInfo.ExpectedPanelType = PanelTypeEnum.OneLink;
            }
            
            else if (Model.PanelType == "NoPanel")
            {
                CCI.CommitmentExpectedInfo.ExpectedPanelType = PanelTypeEnum.NoPanel;
            }
            else //if(Model.PanelType == "NotSet")
            {
                CCI.CommitmentExpectedInfo.ExpectedPanelType = PanelTypeEnum.NotSet;
            }
            #endregion

            CCI.CommitmentExpectedInfo.ExpectedPanelType = Model.PanelType.ToEnum<PanelTypeEnum>();

            //else if (Model.PanelType == "Panel19")
            //{
            //    CCI.CommitmentExpectedInfo.ExpectedPanelType = PanelTypeEnum.Panel19;
            //}
            //else if (Model.PanelType == "Panel24")
            //{
            //    CCI.CommitmentExpectedInfo.ExpectedPanelType = PanelTypeEnum.Panel24;
            //}


            #region NetworkType

            if (Model.Network == "Skytel")
            {
                CCI.CommitmentExpectedInfo.ExpectedNetwork = NetworkEnum.Skytel;
            }
            else if (Model.Network == "WebLink")
            {
                CCI.CommitmentExpectedInfo.ExpectedNetwork = NetworkEnum.WebLink;
            }
            else if (Model.Network == "GSM")
            {
                CCI.CommitmentExpectedInfo.ExpectedNetwork = NetworkEnum.GSM;
            }
            else if (Model.Network == "Broadband")
            {
                CCI.CommitmentExpectedInfo.ExpectedNetwork = NetworkEnum.Broadband;
            }
            else if (Model.Network == "GSM2")
            {
                CCI.CommitmentExpectedInfo.ExpectedNetwork = NetworkEnum.GSM2;
            }
            else if (Model.Network == "GsmCanada")
            {
                CCI.CommitmentExpectedInfo.ExpectedNetwork = NetworkEnum.GsmCanada;
            }
            else if (Model.Network == "TMobile")
            {
                CCI.CommitmentExpectedInfo.ExpectedNetwork = NetworkEnum.TMobile;
            }
            else if (Model.Network == "TMobilePartner1")
            {
                CCI.CommitmentExpectedInfo.ExpectedNetwork = NetworkEnum.TMobilePartner1;
            }
            else if (Model.Network == "GsmCanada2")
            {
                CCI.CommitmentExpectedInfo.ExpectedNetwork = NetworkEnum.GsmCanada2;
            }
            else if (Model.Network == "Rogers")
            {
                CCI.CommitmentExpectedInfo.ExpectedNetwork = NetworkEnum.Rogers;
            }
            else if (Model.Network == "RogersPartner1")
            {
                CCI.CommitmentExpectedInfo.ExpectedNetwork = NetworkEnum.RogersPartner1;
            }
            else if (Model.Network == "ATTPartner1")
            {
                CCI.CommitmentExpectedInfo.ExpectedNetwork = NetworkEnum.ATTPartner1;
            }
            else if (Model.Network == "Verizon")
            {
                CCI.CommitmentExpectedInfo.ExpectedNetwork = NetworkEnum.Verizon;
            }
            else if (Model.Network == "Telefonica")
            {
                CCI.CommitmentExpectedInfo.ExpectedNetwork = NetworkEnum.Telefonica;
            }
            else if (Model.Network == "DealerProvisioned")
            {
                CCI.CommitmentExpectedInfo.ExpectedNetwork = NetworkEnum.DealerProvisioned;
            }
            else if (Model.Network == "Oi")
            {
                CCI.CommitmentExpectedInfo.ExpectedNetwork = NetworkEnum.Oi;
            }
            else if (Model.Network == "Vivo")
            {
                CCI.CommitmentExpectedInfo.ExpectedNetwork = NetworkEnum.Vivo;
            }
            else if (Model.Network == "Vodafone")
            {
                CCI.CommitmentExpectedInfo.ExpectedNetwork = NetworkEnum.Vodafone;
            }
            else if (Model.Network == "VerizonPartner1")
            {
                CCI.CommitmentExpectedInfo.ExpectedNetwork = NetworkEnum.VerizonPartner1;
            }
            else if (Model.Network == "Turkcell")
            {
                CCI.CommitmentExpectedInfo.ExpectedNetwork = NetworkEnum.Turkcell;
            }
            else if (Model.Network == "USCellular")
            {
                CCI.CommitmentExpectedInfo.ExpectedNetwork = NetworkEnum.USCellular;
            }
            else if (Model.Network == "Digicel")
            {
                CCI.CommitmentExpectedInfo.ExpectedNetwork = NetworkEnum.Digicel;
            }
            else if (Model.Network == "Spark")
            {
                CCI.CommitmentExpectedInfo.ExpectedNetwork = NetworkEnum.Spark;
            }
            else if (Model.Network == "Cotas")
            {
                CCI.CommitmentExpectedInfo.ExpectedNetwork = NetworkEnum.Cotas;
            }
            else if (Model.Network == "Eastlink")
            {
                CCI.CommitmentExpectedInfo.ExpectedNetwork = NetworkEnum.Eastlink;
            }
            else if (Model.Network == "TMDATA")
            {
                CCI.CommitmentExpectedInfo.ExpectedNetwork = NetworkEnum.TMDATA;
            }
            else if (Model.Network == "Millicom")
            {
                CCI.CommitmentExpectedInfo.ExpectedNetwork = NetworkEnum.Millicom;
            }
            else if (Model.Network == "TelefonicaPartner2")
            {
                CCI.CommitmentExpectedInfo.ExpectedNetwork = NetworkEnum.TelefonicaPartner2;
            }
            else if (Model.Network == "Telus")
            {
                CCI.CommitmentExpectedInfo.ExpectedNetwork = NetworkEnum.Telus;
            }
            else //if "NotSet"
            {
                CCI.CommitmentExpectedInfo.ExpectedNetwork = NetworkEnum.NotSet;
            }

            #endregion
            CustomerManagement cm = new CustomerManagement();
            Authentication auth = new Authentication()
            {
                User = Model.AuthUser,
                Password = Model.AuthPass
            };
            cm.AuthenticationValue = auth;
            return cm.CreateCommitment(CCI);

        }
        public CreateCustomerOutput CreateCustomer(SetupAlarm Model)
        {

            CreateCustomerInput ci = new CreateCustomerInput()
            {
                //PropertyType = PropertyTypeEnum.SingleFamilyHouse,
                CustomerAccountAddress = new AddressWithName
                {
                    FirstName = Model.FirstName,//"Jhon",
                    LastName = Model.LastName,//"Sam",
                    CountryId = CountryEnum.USA,
                    City = Model.City,//"New York",
                    Street1 = Model.Street, //"Jones Street, Manhattan",
                    State = Model.State, //"ca",
                    Zip = Model.Zip //"90001"
                },

                CustomerAccountEmail = Model.EmailAddress,//"samjhon@gmail.com",
                CustomerAccountPhone = Model.Phone, //"2025250117",
                
                DealerCustomerId = Model.DealerCustomer,//"123456",

                DesiredLoginName = Model.LoginName, //"jonsam",
                DesiredPassword = Model.LoginPassword,//"123Abcdef",

                //Culture = CultureEnum.English_US, //init below
                //PanelType = PanelTypeEnum.TwoG, //init below
                //PanelVersion = PanelVersionEnum.TwoGig112, //init below
                ModemSerialNumber = Model.ModelSerialNumber, //"113179515122147",
                PhoneLinePresent = (Model.PhoneLinePresent.HasValue && Model.PhoneLinePresent.Value) ? true : false,// false,
               // CentralStationForwardingOption = CentralStationForwardingOptionEnum.Always,
                CentralStationAccountNumber = Model.CentralStationAccountNo, //"8999",//flv12/flv7
                CentralStationReceiverNumber = Model.CentralStationRecieverNumber, //"855-432-3890",
                PackageId = Model.PackageId,//"8999",
                IgnoreLowCoverageErrors = (Model.IgnoreLowCoverageError.HasValue && Model.IgnoreLowCoverageError.Value) ? true : false//true
            };

            #region AddOnFeatures
            if(Model.adonitem != null && Model.adonitem.Count() > 0)
            {
                int IndexLength = Model.adonitem.Count();
                AddOnFeatureEnum[] enumList = new AddOnFeatureEnum[IndexLength];
                int count = 0;
                foreach (var item in Model.adonitem)
                {
                    enumList[count] = (HS.Alarm.AlarmCustomer.AddOnFeatureEnum)item;
                    count++;
                }

                ci.AddOnFeatures = enumList;

            }


            #endregion
            #region Provider Name
            if (!string.IsNullOrEmpty(Model.SalesRepName))
            {
                ci.SalesRepLoginName = Model.SalesRepName;                
            }
            if (!string.IsNullOrEmpty(Model.InstallerLoginName))
            {
                ci.InstallerLoginName = Model.InstallerLoginName;
                ci.LoginNameAtAuthenticationProvider = Model.InstallerLoginName;
            }
            
            #endregion
            #region PropertyType
            if (Model.PropertyType == "Business")
            {
                ci.PropertyType = PropertyTypeEnum.Business;
                ci.CustomerAccountAddress.CompanyName = Model.CompanyName;
            }
            else if (Model.PropertyType == "Condo")
            {
                ci.PropertyType = PropertyTypeEnum.Condo;
            }
            else if (Model.PropertyType == "SingleFamilyHouse")
            {
                ci.PropertyType = PropertyTypeEnum.SingleFamilyHouse;
            }
            else if (Model.PropertyType == "Townhouse")
            {
                ci.PropertyType = PropertyTypeEnum.Townhouse;
            }
            else //if(Model.PropertyType == "NotSet")
            {
                ci.PropertyType = PropertyTypeEnum.NotSet;
            }
            #endregion

            #region PanelVersionEnumOpt

            //Added by Digiture
            if(!string.IsNullOrEmpty(Model.PanelVersion))ci.PanelVersion = Model.PanelVersion.ToEnum<PanelVersionEnum>();

            //if (Model.PanelVersion == "Other")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Other;
            //}
            //else if (Model.PanelVersion == "Unknown")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Unknown;
            //}
            //else if (Model.PanelVersion == "Concord3")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Concord3;
            //}
            //else if (Model.PanelVersion == "Concord4")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Concord4;
            //}
            //else if (Model.PanelVersion == "ConcordExpress")
            //{
            //    ci.PanelVersion = PanelVersionEnum.ConcordExpress;
            //}
            //else if (Model.PanelVersion == "ConcordMonitronicsExpress")
            //{
            //    ci.PanelVersion = PanelVersionEnum.ConcordMonitronicsExpress;
            //}
            //else if (Model.PanelVersion == "Concord311")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Concord311;
            //}
            //else if (Model.PanelVersion == "Concord312")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Concord312;
            //}
            //else if (Model.PanelVersion == "SimonXt10")
            //{
            //    ci.PanelVersion = PanelVersionEnum.SimonXt10;
            //}
            //else if (Model.PanelVersion == "SimonXt12")
            //{
            //    ci.PanelVersion = PanelVersionEnum.SimonXt12;
            //}
            //else if (Model.PanelVersion == "SimonXt11")
            //{
            //    ci.PanelVersion = PanelVersionEnum.SimonXt11;
            //}
            //else if (Model.PanelVersion == "SimonXt13")
            //{
            //    ci.PanelVersion = PanelVersionEnum.SimonXt13;
            //}
            //else if (Model.PanelVersion == "SimonXt14")
            //{
            //    ci.PanelVersion = PanelVersionEnum.SimonXt14;
            //}
            //else if (Model.PanelVersion == "SimonXt14French")
            //{
            //    ci.PanelVersion = PanelVersionEnum.SimonXt14French;
            //}
            //else if (Model.PanelVersion == "SimonXti")
            //{
            //    ci.PanelVersion = PanelVersionEnum.SimonXti;
            //}
            //else if (Model.PanelVersion == "SimonXt14Spanish")
            //{
            //    ci.PanelVersion = PanelVersionEnum.SimonXt14Spanish;
            //}
            //else if (Model.PanelVersion == "SimonXt14Portuguese")
            //{
            //    ci.PanelVersion = PanelVersionEnum.SimonXt14Portuguese;
            //}
            //else if (Model.PanelVersion == "SimonXti17")
            //{
            //    ci.PanelVersion = PanelVersionEnum.SimonXti17;
            //}
            //else if (Model.PanelVersion == "SimonXt16")
            //{
            //    ci.PanelVersion = PanelVersionEnum.SimonXt16;
            //}
            //else if (Model.PanelVersion == "SimonXt16French")
            //{
            //    ci.PanelVersion = PanelVersionEnum.SimonXt16French;
            //}
            //else if (Model.PanelVersion == "SimonXt16Spanish")
            //{
            //    ci.PanelVersion = PanelVersionEnum.SimonXt16Spanish;
            //}
            //else if (Model.PanelVersion == "SimonXt16Portuguese")
            //{
            //    ci.PanelVersion = PanelVersionEnum.SimonXt16Portuguese;
            //}
            //else if (Model.PanelVersion == "Simon32")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Simon32;
            //}
            //else if (Model.PanelVersion == "Simon33")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Simon33;
            //}
            //else if (Model.PanelVersion == "Simon34")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Simon34;
            //}
            //else if (Model.PanelVersion == "Simon35")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Simon35;
            //}
            //else if (Model.PanelVersion == "Simon36")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Simon36;
            //}
            //else if (Model.PanelVersion == "Simon37")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Simon37;
            //}
            //else if (Model.PanelVersion == "Simon38")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Simon38;
            //}
            //else if (Model.PanelVersion == "Simon39")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Simon39;
            //}
            //else if (Model.PanelVersion == "Simon40")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Simon40;
            //}
            //else if (Model.PanelVersion == "Simon41")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Simon41;
            //}
            //else if (Model.PanelVersion == "Simon42")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Simon42;
            //}
            //else if (Model.PanelVersion == "Simon43")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Simon43;
            //}
            //else if (Model.PanelVersion == "Simon44")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Simon44;
            //}
            //else if (Model.PanelVersion == "NX1")
            //{
            //    ci.PanelVersion = PanelVersionEnum.NX1;
            //}
            //else if (Model.PanelVersion == "NX8e")
            //{
            //    ci.PanelVersion = PanelVersionEnum.NX8e;
            //}
            //else if (Model.PanelVersion == "NX4v2")
            //{
            //    ci.PanelVersion = PanelVersionEnum.NX4v2;
            //}
            //else if (Model.PanelVersion == "NX6v2")
            //{
            //    ci.PanelVersion = PanelVersionEnum.NX6v2;
            //}
            //else if (Model.PanelVersion == "NX8v2")
            //{
            //    ci.PanelVersion = PanelVersionEnum.NX8v2;
            //}
            //else if (Model.PanelVersion == "NX1EU")
            //{
            //    ci.PanelVersion = PanelVersionEnum.NX1EU;
            //}
            //else if (Model.PanelVersion == "NX8eEU")
            //{
            //    ci.PanelVersion = PanelVersionEnum.NX8eEU;
            //}
            //else if (Model.PanelVersion == "NX4v2EU")
            //{
            //    ci.PanelVersion = PanelVersionEnum.NX4v2EU;
            //}
            //else if (Model.PanelVersion == "NX6v2EU")
            //{
            //    ci.PanelVersion = PanelVersionEnum.NX6v2EU;
            //}
            //else if (Model.PanelVersion == "NX8v2EU")
            //{
            //    ci.PanelVersion = PanelVersionEnum.NX8v2EU;
            //}
            //else if (Model.PanelVersion == "ElkGuard1")
            //{
            //    ci.PanelVersion = PanelVersionEnum.ElkGuard1;
            //}
            //else if (Model.PanelVersion == "ElkGuard2")
            //{
            //    ci.PanelVersion = PanelVersionEnum.ElkGuard2;
            //}
            //else if (Model.PanelVersion == "TwoGigPreRelease")
            //{
            //    ci.PanelVersion = PanelVersionEnum.TwoGigPreRelease;
            //}
            //else if (Model.PanelVersion == "TwoGig12")
            //{
            //    ci.PanelVersion = PanelVersionEnum.TwoGig12;
            //}
            //else if (Model.PanelVersion == "TwoGig13")
            //{
            //    ci.PanelVersion = PanelVersionEnum.TwoGig13;
            //}
            //else if (Model.PanelVersion == "TwoGig14")
            //{
            //    ci.PanelVersion = PanelVersionEnum.TwoGig14;
            //}
            //else if (Model.PanelVersion == "TwoGig15")
            //{
            //    ci.PanelVersion = PanelVersionEnum.TwoGig15;
            //}
            //else if (Model.PanelVersion == "TwoGig16")
            //{
            //    ci.PanelVersion = PanelVersionEnum.TwoGig16;
            //}
            //else if (Model.PanelVersion == "TwoGig18")
            //{
            //    ci.PanelVersion = PanelVersionEnum.TwoGig18;
            //}
            //else if (Model.PanelVersion == "TwoGig18French")
            //{
            //    ci.PanelVersion = PanelVersionEnum.TwoGig18French;
            //}
            //else if (Model.PanelVersion == "TwoGig19")
            //{
            //    ci.PanelVersion = PanelVersionEnum.TwoGig19;
            //}
            //else if (Model.PanelVersion == "TwoGig19French")
            //{
            //    ci.PanelVersion = PanelVersionEnum.TwoGig19French;
            //}
            //else if (Model.PanelVersion == "TwoGig19Spanish")
            //{
            //    ci.PanelVersion = PanelVersionEnum.TwoGig19Spanish;
            //}
            //else if (Model.PanelVersion == "TwoGig110")
            //{
            //    ci.PanelVersion = PanelVersionEnum.TwoGig110;
            //}
            //else if (Model.PanelVersion == "TwoGig110French")
            //{
            //    ci.PanelVersion = PanelVersionEnum.TwoGig110French;
            //}
            //else if (Model.PanelVersion == "TwoGig110Spanish")
            //{
            //    ci.PanelVersion = PanelVersionEnum.TwoGig110Spanish;
            //}
            //else if (Model.PanelVersion == "TwoGig112")
            //{
            //    ci.PanelVersion = PanelVersionEnum.TwoGig112;
            //}
            //else if (Model.PanelVersion == "TwoGig112French")
            //{
            //    ci.PanelVersion = PanelVersionEnum.TwoGig112French;
            //}
            //else if (Model.PanelVersion == "TwoGig112Spanish")
            //{
            //    ci.PanelVersion = PanelVersionEnum.TwoGig112Spanish;
            //}
            //else if (Model.PanelVersion == "TwoGig112Portuguese")
            //{
            //    ci.PanelVersion = PanelVersionEnum.TwoGig112Portuguese;
            //}
            //else if (Model.PanelVersion == "TwoGig113")
            //{
            //    ci.PanelVersion = PanelVersionEnum.TwoGig113;
            //}
            //else if (Model.PanelVersion == "TwoGig113Spanish")
            //{
            //    ci.PanelVersion = PanelVersionEnum.TwoGig113Spanish;
            //}
            //else if (Model.PanelVersion == "TwoGig113French")
            //{
            //    ci.PanelVersion = PanelVersionEnum.TwoGig113French;
            //}
            //else if (Model.PanelVersion == "TwoGig113Portuguese")
            //{
            //    ci.PanelVersion = PanelVersionEnum.TwoGig113Portuguese;
            //}
            //else if (Model.PanelVersion == "TwoGig113Turkish")
            //{
            //    ci.PanelVersion = PanelVersionEnum.TwoGig113Turkish;
            //}
            //else if (Model.PanelVersion == "TwoGig114")
            //{
            //    ci.PanelVersion = PanelVersionEnum.TwoGig114;
            //}
            //else if (Model.PanelVersion == "TwoGig114Spanish")
            //{
            //    ci.PanelVersion = PanelVersionEnum.TwoGig114Spanish;
            //}
            //else if (Model.PanelVersion == "TwoGig114French")
            //{
            //    ci.PanelVersion = PanelVersionEnum.TwoGig114French;
            //}
            //else if (Model.PanelVersion == "TwoGig114Portuguese")
            //{
            //    ci.PanelVersion = PanelVersionEnum.TwoGig114Portuguese;
            //}
            //else if (Model.PanelVersion == "TwoGig114Turkish")
            //{
            //    ci.PanelVersion = PanelVersionEnum.TwoGig114Turkish;
            //}
            //else if (Model.PanelVersion == "TwoGig116")
            //{
            //    ci.PanelVersion = PanelVersionEnum.TwoGig116;
            //}
            //else if (Model.PanelVersion == "TwoGig116Spanish")
            //{
            //    ci.PanelVersion = PanelVersionEnum.TwoGig116Spanish;
            //}
            //else if (Model.PanelVersion == "TwoGig116French")
            //{
            //    ci.PanelVersion = PanelVersionEnum.TwoGig116French;
            //}
            //else if (Model.PanelVersion == "TwoGig116Portuguese")
            //{
            //    ci.PanelVersion = PanelVersionEnum.TwoGig116Portuguese;
            //}
            //else if (Model.PanelVersion == "TwoGig116Turkish")
            //{
            //    ci.PanelVersion = PanelVersionEnum.TwoGig116Turkish;
            //}
            //else if (Model.PanelVersion == "TwoGig117")
            //{
            //    ci.PanelVersion = PanelVersionEnum.TwoGig117;
            //}
            //else if (Model.PanelVersion == "TwoGig117Spanish")
            //{
            //    ci.PanelVersion = PanelVersionEnum.TwoGig117Spanish;
            //}
            //else if (Model.PanelVersion == "TwoGig117French")
            //{
            //    ci.PanelVersion = PanelVersionEnum.TwoGig117French;
            //}
            //else if (Model.PanelVersion == "TwoGig117Portuguese")
            //{
            //    ci.PanelVersion = PanelVersionEnum.TwoGig117Portuguese;
            //}
            //else if (Model.PanelVersion == "TwoGig117Turkish")
            //{
            //    ci.PanelVersion = PanelVersionEnum.TwoGig117Turkish;
            //}
            //else if (Model.PanelVersion == "TwoGig118")
            //{
            //    ci.PanelVersion = PanelVersionEnum.TwoGig118;
            //}
            //else if (Model.PanelVersion == "TouchlinkPreRelease")
            //{
            //    ci.PanelVersion = PanelVersionEnum.TouchlinkPreRelease;
            //}
            //else if (Model.PanelVersion == "Pointhub")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Pointhub;
            //}
            //else if (Model.PanelVersion == "IQPanel")
            //{
            //    ci.PanelVersion = PanelVersionEnum.IQPanel;
            //}
            //else if (Model.PanelVersion == "IQPanel141")
            //{
            //    ci.PanelVersion = PanelVersionEnum.IQPanel141;
            //}
            //else if (Model.PanelVersion == "IQPanel142")
            //{
            //    ci.PanelVersion = PanelVersionEnum.IQPanel142;
            //}
            //else if (Model.PanelVersion == "IQPanel143")
            //{
            //    ci.PanelVersion = PanelVersionEnum.IQPanel143;
            //}
            //else if (Model.PanelVersion == "IQPanel150")
            //{
            //    ci.PanelVersion = PanelVersionEnum.IQPanel150;
            //}
            //else if (Model.PanelVersion == "IQPanel261")
            //{
            //    ci.PanelVersion = PanelVersionEnum.IQPanel261;
            //}
            //else if (Model.PanelVersion == "IQPanel151")
            //{
            //    ci.PanelVersion = PanelVersionEnum.IQPanel151;
            //}
            //else if (Model.PanelVersion == "IQPanel152")
            //{
            //    ci.PanelVersion = PanelVersionEnum.IQPanel152;
            //}
            //else if (Model.PanelVersion == "IQPanel160")
            //{
            //    ci.PanelVersion = PanelVersionEnum.IQPanel160;
            //}
            //else if (Model.PanelVersion == "IQPanel161")
            //{
            //    ci.PanelVersion = PanelVersionEnum.IQPanel161;
            //}
            //else if (Model.PanelVersion == "IQPanel162")
            //{
            //    ci.PanelVersion = PanelVersionEnum.IQPanel162;
            //}
            //else if (Model.PanelVersion == "IQPanel153")
            //{
            //    ci.PanelVersion = PanelVersionEnum.IQPanel153;
            //}
            //else if (Model.PanelVersion == "IQPanel410")
            //{
            //    ci.PanelVersion = PanelVersionEnum.IQPanel410;
            //}
            //else if (Model.PanelVersion == "DSCTouchPanel143")
            //{
            //    ci.PanelVersion = PanelVersionEnum.DSCTouchPanel143;
            //}
            //else if (Model.PanelVersion == "DSCTouchPanel150")
            //{
            //    ci.PanelVersion = PanelVersionEnum.DSCTouchPanel150;
            //}
            //else if (Model.PanelVersion == "DSCTouchPanel151")
            //{
            //    ci.PanelVersion = PanelVersionEnum.DSCTouchPanel151;
            //}
            //else if (Model.PanelVersion == "DSCTouchPanel152")
            //{
            //    ci.PanelVersion = PanelVersionEnum.DSCTouchPanel152;
            //}
            //else if (Model.PanelVersion == "DSCTouchPanel153")
            //{
            //    ci.PanelVersion = PanelVersionEnum.DSCTouchPanel153;
            //}
            //else if (Model.PanelVersion == "DSCTouchPanel160")
            //{
            //    ci.PanelVersion = PanelVersionEnum.DSCTouchPanel160;
            //}
            //else if (Model.PanelVersion == "DSCTouchPanel161")
            //{
            //    ci.PanelVersion = PanelVersionEnum.DSCTouchPanel161;
            //}
            //else if (Model.PanelVersion == "DSCTouchPanel162")
            //{
            //    ci.PanelVersion = PanelVersionEnum.DSCTouchPanel162;
            //}
            //else if (Model.PanelVersion == "IQPanel163")
            //{
            //    ci.PanelVersion = PanelVersionEnum.IQPanel163;
            //}
            //else if (Model.PanelVersion == "DSCTouchPanel163")
            //{
            //    ci.PanelVersion = PanelVersionEnum.DSCTouchPanel163;
            //}
            //else if (Model.PanelVersion == "Building36")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Building36;
            //}
            //else if (Model.PanelVersion == "AdvisorPreRelease")
            //{
            //    ci.PanelVersion = PanelVersionEnum.AdvisorPreRelease;
            //}
            //else if (Model.PanelVersion == "Impassa")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Impassa;
            //}
            //else if (Model.PanelVersion == "Impassa131")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Impassa131;
            //}
            //else if (Model.PanelVersion == "NeoHS2016")
            //{
            //    ci.PanelVersion = PanelVersionEnum.NeoHS2016;
            //}
            //else if (Model.PanelVersion == "NeoHS2032")
            //{
            //    ci.PanelVersion = PanelVersionEnum.NeoHS2032;
            //}
            //else if (Model.PanelVersion == "NeoHS2064")
            //{
            //    ci.PanelVersion = PanelVersionEnum.NeoHS2064;
            //}
            //else if (Model.PanelVersion == "NeoHS2128")
            //{
            //    ci.PanelVersion = PanelVersionEnum.NeoHS2128;
            //}
            //else if (Model.PanelVersion == "Neo12HS2016")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Neo12HS2016;
            //}
            //else if (Model.PanelVersion == "Neo12HS2032")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Neo12HS2032;
            //}
            //else if (Model.PanelVersion == "Neo12HS2064")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Neo12HS2064;
            //}
            //else if (Model.PanelVersion == "Neo12HS2128")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Neo12HS2128;
            //}
            //else if (Model.PanelVersion == "Neo13HS2016")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Neo13HS2016;
            //}
            //else if (Model.PanelVersion == "Neo13HS2032")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Neo13HS2032;
            //}
            //else if (Model.PanelVersion == "Neo13HS2064")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Neo13HS2064;
            //}
            //else if (Model.PanelVersion == "Neo13HS2128")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Neo13HS2128;
            //}
            //else if (Model.PanelVersion == "GoControl3")
            //{
            //    ci.PanelVersion = PanelVersionEnum.GoControl3;
            //}
            //else if (Model.PanelVersion == "GoControl301")
            //{
            //    ci.PanelVersion = PanelVersionEnum.GoControl301;
            //}
            //else if (Model.PanelVersion == "GoControl310")
            //{
            //    ci.PanelVersion = PanelVersionEnum.GoControl310;
            //}
            //else if (Model.PanelVersion == "GoControl302")
            //{
            //    ci.PanelVersion = PanelVersionEnum.GoControl302;
            //}
            //else if (Model.PanelVersion == "GoControl311")
            //{
            //    ci.PanelVersion = PanelVersionEnum.GoControl311;
            //}
            //else if (Model.PanelVersion == "GoControl320")
            //{
            //    ci.PanelVersion = PanelVersionEnum.GoControl320;
            //}
            //else if (Model.PanelVersion == "GoControl312")
            //{
            //    ci.PanelVersion = PanelVersionEnum.GoControl312;
            //}
            //else if (Model.PanelVersion == "NucleusPreRelease")
            //{
            //    ci.PanelVersion = PanelVersionEnum.NucleusPreRelease;
            //}
            //else if (Model.PanelVersion == "SEMVista15P")
            //{
            //    ci.PanelVersion = PanelVersionEnum.SEMVista15P;
            //}
            //else if (Model.PanelVersion == "SEMVista20P")
            //{
            //    ci.PanelVersion = PanelVersionEnum.SEMVista20P;
            //}
            //else if (Model.PanelVersion == "SEMVista10P")
            //{
            //    ci.PanelVersion = PanelVersionEnum.SEMVista10P;
            //}
            //else if (Model.PanelVersion == "SEMVista15PUD")
            //{
            //    ci.PanelVersion = PanelVersionEnum.SEMVista15PUD;
            //}
            //else if (Model.PanelVersion == "SEMVista20PUD")
            //{
            //    ci.PanelVersion = PanelVersionEnum.SEMVista20PUD;
            //}
            //else if (Model.PanelVersion == "SEMVista10PUD")
            //{
            //    ci.PanelVersion = PanelVersionEnum.SEMVista10PUD;
            //}
            //else if (Model.PanelVersion == "SEMPowerSeriesPC1616")
            //{
            //    ci.PanelVersion = PanelVersionEnum.SEMPowerSeriesPC1616;
            //}
            //else if (Model.PanelVersion == "SEMPowerSeriesPC1832")
            //{
            //    ci.PanelVersion = PanelVersionEnum.SEMPowerSeriesPC1832;
            //}
            //else if (Model.PanelVersion == "SEMPowerSeriesPC1864")
            //{
            //    ci.PanelVersion = PanelVersionEnum.SEMPowerSeriesPC1864;
            //}
            //else if (Model.PanelVersion == "SEMPowerSeriesPC1808")
            //{
            //    ci.PanelVersion = PanelVersionEnum.SEMPowerSeriesPC1808;
            //}
            
            //else if (Model.PanelVersion == "IQPanel2Release")
            //{
            //    ci.PanelVersion = PanelVersionEnum.IQPanel2Release;
            //}
            //else if (Model.PanelVersion == "IQPanel203")
            //{
            //    ci.PanelVersion = PanelVersionEnum.IQPanel203;
            //}
            //else if (Model.PanelVersion == "IQPanel210")
            //{
            //    ci.PanelVersion = PanelVersionEnum.IQPanel210;
            //}
            //else if (Model.PanelVersion == "IQPanel202")
            //{
            //    ci.PanelVersion = PanelVersionEnum.IQPanel202;
            //}
            //else if (Model.PanelVersion == "IQPanel201")
            //{
            //    ci.PanelVersion = PanelVersionEnum.IQPanel201;
            //}
            //else if (Model.PanelVersion == "IQPanel204")
            //{
            //    ci.PanelVersion = PanelVersionEnum.IQPanel204;
            //}
            //else if (Model.PanelVersion == "IQPanel205")
            //{
            //    ci.PanelVersion = PanelVersionEnum.IQPanel205;
            //}
            //else if (Model.PanelVersion == "IQPanel206")
            //{
            //    ci.PanelVersion = PanelVersionEnum.IQPanel206;
            //}
            //else if (Model.PanelVersion == "Vario557")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Vario557;
            //}
            //else if (Model.PanelVersion == "IotegaPreRelease")
            //{
            //    ci.PanelVersion = PanelVersionEnum.IotegaPreRelease;
            //}
            
            //else if (Model.PanelVersion == "IQPanel260")
            //{
            //    ci.PanelVersion = PanelVersionEnum.IQPanel260;
            //}
            //else if (Model.PanelVersion == "Edge11")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Edge11;
            //}
            //else if (Model.PanelVersion == "OneLinkPreRelease")
            //{
            //    ci.PanelVersion = PanelVersionEnum.OneLinkPreRelease;
            //}
            //else if (Model.PanelVersion == "Panel24AIO")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Panel24AIO;
            //}
            //else if (Model.PanelVersion == "Panel24Hub")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Panel24Hub;
            //}
            //else if (Model.PanelVersion == "Panel24Hybrid")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Panel24Hybrid;
            //}
            //else if (Model.PanelVersion == "Panel19HP1")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Panel19HP1;
            //}
            //else if (Model.PanelVersion == "Panel19HM1")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Panel19HM1;
            //}
            //else if (Model.PanelVersion == "Panel19HP11")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Panel19HP11;
            //}
            //else if (Model.PanelVersion == "Panel19HM11")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Panel19HM11;
            //}
            //else if (Model.PanelVersion == "Panel19HS11")
            //{
            //    ci.PanelVersion = PanelVersionEnum.Panel19HS11;
            //}
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
            
            else if (Model.PanelType == "IQPanel2")
            {
                ci.PanelType = PanelTypeEnum.IQPanel2;
            }
            else if (Model.PanelType == "IQPanel4")
            {
                ci.PanelType = PanelTypeEnum.IQPanel4;
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
            
            else if (Model.PanelType == "NoPanel")
            {
                ci.PanelType = PanelTypeEnum.NoPanel;
            }
            #endregion


            //else if (Model.PanelType == "Panel19")
            //{
            //    ci.PanelType = PanelTypeEnum.Panel19;
            //}
            //else if (Model.PanelType == "Panel24")
            //{
            //    ci.PanelType = PanelTypeEnum.Panel24;
            //}

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

            #region CentralStationForwardingOptionEnum

            if (Model.CentralStationForwardingOption == "Always")
            {
                ci.CentralStationForwardingOption = CentralStationForwardingOptionEnum.Always;
            }
            else if (Model.CentralStationForwardingOption == "Never")
            {
                ci.CentralStationForwardingOption = CentralStationForwardingOptionEnum.Never;
            }
            else if (Model.CentralStationForwardingOption == "OnlyIfPhoneFails")
            {
                ci.CentralStationForwardingOption = CentralStationForwardingOptionEnum.OnlyIfPhoneFails;
            }
            else
            {
                ci.CentralStationForwardingOption = CentralStationForwardingOptionEnum.NotSet;
            }

            #endregion

            #region ForwardedEvents
            List<CentralStationEventGroupEnum> selectedEvents = new List<CentralStationEventGroupEnum>();

            foreach (string forwardedEvent in Model.ForwardedEvents?? Enumerable.Empty<String>())
            {
                if (forwardedEvent == "Alarms")
                {
                    selectedEvents.Add(CentralStationEventGroupEnum.Alarms);
                }
                else if (forwardedEvent == "Armings")
                {
                    selectedEvents.Add(CentralStationEventGroupEnum.Armings);
                }
                else if (forwardedEvent == "Bypass")
                {
                    selectedEvents.Add(CentralStationEventGroupEnum.Bypass);
                }
                else if (forwardedEvent == "Cancels")
                {
                    selectedEvents.Add(CentralStationEventGroupEnum.Cancels);
                }
                else if (forwardedEvent == "CrashAndSmash")
                {
                    selectedEvents.Add(CentralStationEventGroupEnum.CrashAndSmash);
                }
                else if (forwardedEvent == "PanelNotResponding")
                {
                    selectedEvents.Add(CentralStationEventGroupEnum.PanelNotResponding);
                }
                else if (forwardedEvent == "Panics")
                {
                    selectedEvents.Add(CentralStationEventGroupEnum.Panics);
                }
                else if (forwardedEvent == "PhoneCommFailure")
                {
                    selectedEvents.Add(CentralStationEventGroupEnum.PhoneCommFailure);
                }
                else if (forwardedEvent == "PhoneTests")
                {
                    selectedEvents.Add(CentralStationEventGroupEnum.PhoneTests);
                }
                else if (forwardedEvent == "SensorTampers")
                {
                    selectedEvents.Add(CentralStationEventGroupEnum.SensorTampers);
                }
                else if (forwardedEvent == "TroubleRestorals")
                {
                    selectedEvents.Add(CentralStationEventGroupEnum.TroubleRestorals);
                }
                else if (forwardedEvent == "Troubles")
                {
                    selectedEvents.Add(CentralStationEventGroupEnum.Troubles);
                }
            }
            #endregion
            
            #region Conditional Attributes set

            /*  Sytem installation address need to set properly. 
             *  User Address and system installation address might be different.
             */
            if (Model.SameInsAddress)
            {
                ci.InstallationAddress = new Address
                {
                    CountryId = CountryEnum.USA,
                    City = Model.City,//"New York",
                    Street1 = Model.Street, //"Jones Street, Manhattan",
                    State = Model.State, //"ca",
                    Zip = Model.Zip //"90001"
                };
            }
            else
            {
                ci.InstallationAddress = new Address
                {
                    CountryId = CountryEnum.USA,
                    City = Model.InsCity, //" New York",
                    Street1 = Model.InsStreet,//"Jones Street, Manhattan",
                    State = Model.InsState,//"ca",
                    Zip = Model.InsZip,//"90001",                    
                };
            }
            ci.InstallationTimeZone = TimeZoneEnum.NotSet;
            ci.CsEventGroupsToForward = selectedEvents.ToArray();

            #endregion


            CustomerManagement cm = new CustomerManagement();
            Authentication auth = new Authentication()
            {
                User = Model.AuthUser,
                Password = Model.AuthPass
            };
            cm.AuthenticationValue = auth;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            Alarm.AlarmCustomer.CreateCustomerOutput response = new HS.Alarm.AlarmCustomer.CreateCustomerOutput();
           
           
            response = cm.CreateCustomer(ci);
          
            return response;

        }
         
    }
}
