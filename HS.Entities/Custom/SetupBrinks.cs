using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Entities.Custom
{
    public class SetupBrinks
    {
        public string site_name { get; set; }
        public string sitetype_id { get; set; }
        public string sitestat_id { get; set; }
        public string site_addr1 { get; set; }
        public string site_addr2 { get; set; }
        public string city_name { get; set; }
        public string county_name { get; set; }
        public string state_id { get; set; }
        public string zip_code { get; set; }
        public string phone1 { get; set; }
        public string timezone_descr { get; set; }
        public string servco_no { get; set; }
        public string install_servco_no { get; set; }
        public string cspart_no { get; set; }
        public string subdivision { get; set; }
        public string cross_street { get; set; }
        public string codeword1 { get; set; }
        public string codeword2 { get; set; }
        public DateTime orig_install_date { get; set; }
        public string lang_id { get; set; }
        public string cs_no { get; set; }
        public string systype_id { get; set; }
        public string sec_systype_id { get; set; }
        public string panel_phone { get; set; }
        public string panel_location { get; set; }
        public string receiver_phone { get; set; }
        public string ati_hours { get; set; }
        public string ati_minutes { get; set; }
        public string panel_code { get; set; }
        public string twoway_device_id { get; set; }
        public string alkup_cs_no { get; set; }
        public string blkup_cs_no { get; set; }
        public string ontest_flag { get; set; }
        public string ontest_expire_date { get; set; }
        public string oos_flag { get; set; }
        public DateTime install_date { get; set; }
        public string monitor_type { get; set; }
        public Guid CustomerId { get; set; }
        
        public List<ZoneObject> ZoneObjectList { get; set;}
        public List<BrinksContactObject> contactList { get; set; }

        public bool? IsSigned { get; set; }
        public bool? HasBillingInfo { get; set; }

        public bool? HasBusinessPicture { get; set; }
        public bool? IsCreditCheck { get; set; }
    }
    public class BrinksCredentialsSetting
    {
        public string BrinksUserId { get; set; }

        public string BrinksPassword { get; set; }

        public string BrinksDelearId { get; set; }

        public bool BrinksInProduction { get; set; }

        public bool BrinksCreditCheck { get; set; }
    }
    public class BrinksDeviceList
    {
        public string twoway_device_id { get; set; }
        public string descr { get; set; }
    }
    public class BrinksEventHistory
    {
        public string cs_no { get; set; }
        public DateTime event_date { get; set; }
        public string zone_id { get; set; }
        public string event_id { get; set; }
        public string computed { get; set; }
    }
    public class BrinksTestCategory
    {
        public string testcat_id { get; set; }

        public string descr { get; set; }
        public string default_hours { get; set; }

    }
    public class SurveyQuestion
    {
        public bool SurveyCancellingService { get; set; }
        public bool SurveyConfirmContractLength { get; set; }
        public bool SurveyFamiliarizationPeriod { get; set; }
        public bool SurveyHomeowner { get; set; }
        public bool SurveyNewConstruction { get; set; }
        public bool SurveyUnderContract { get; set; }
        public bool IsExtendedService { get; set; }
    }

    public class PersonalGuarantee
    {
        public bool IsPersonalGuarantee { get; set; }
        public string Title { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }
    public class ZoneObject
    {
        public string cs_no { get; set; }
        public string zone_id { get; set; }
        public string zonestate_id { get; set; }
        public string event_id { get; set; }
        public string equiptype_id { get; set; }
        public string equiploc_id { get; set; }
        public string zone_comment { get; set; }
    }

    public class BrinksContactObject
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string phone1 { get; set; }
        public string contact_no { get; set; }
        public string ctaclink_no { get; set; }
        public string has_key_flag { get; set; }
        public string relation_id { get; set; }


    }
    public class BrinksConfirmationModel
    {
        public string table_name { get; set; }
        public string entry_id { get; set; }
        public string site_no { get; set; }
        public string cs_no { get; set; }

        public string err_no { get; set; }
        public string msg_type { get; set; }
        public string err_text { get; set; }
        public DateTime err_date { get; set; }
    }
    public class BrinksCredentialModel
    {
        public string Entity { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string CsNumber { get; set; }

       
    }

    public class BrinksEquipmentLocation
    {
        public string equiploc_id { get; set; }
        public string descr { get; set; }
    }

    public class BrinksEquipmentType
    {
        public string equiptype_id { get; set; }
        public string descr { get; set; }
    }
    public class BrinksEquipmentTypeEventMap
    {
        public string equiptype_id { get; set; }
        public string event_id { get; set; }
    }
    public class UccAgencyModel
    {
        public int AgencyNumber1 { get; set; }
        public string AgencyType1 { get; set; }
        public string AgencyName1 { get; set; }
        public string AgencyPhone1 { get; set; }

        public int AgencyNumber2 { get; set; }
        public string AgencyType2 { get; set; }
        public string AgencyName2 { get; set; }
        public string AgencyPhone2 { get; set; }

        public int AgencyNumber3 { get; set; }
        public string AgencyType3 { get; set; }
        public string AgencyName3 { get; set; }
        public string AgencyPhone3 { get; set; }

        public int AgencyNumber4 { get; set; }
        public string AgencyType4 { get; set; }
        public string AgencyName4 { get; set; }
        public string AgencyPhone4 { get; set; }

        public int AgencyNumber5 { get; set; }
        public string AgencyType5 { get; set; }
        public string AgencyName5 { get; set; }
        public string AgencyPhone5 { get; set; }


    }

    public class UccCredentialsSettings
    {
        public string UccUserName { get; set; }
        public string UccPassword { get; set; }

        public string UccSiteGroupNumber { get; set; }

        public bool UccInProduction { get; set; }

    }
    public class UccZoneModel
    {
        public string Point1 { get; set; }
        //public string SignalStatus { get; set; }
        public string EventCode1 { get; set; }
        public string Description1 { get; set; }
        //public string AreaNum { get; set; }

        public string Point2 { get; set; }
        //public string SignalStatus { get; set; }
        public string EventCode2 { get; set; }
        public string Description2 { get; set; }
        //public string AreaNum { get; set; }

        public string Point3 { get; set; }
        //public string SignalStatus { get; set; }
        public string EventCode3 { get; set; }
        public string Description3 { get; set; }
        //public string AreaNum { get; set; }

        public string Point4 { get; set; }
        //public string SignalStatus { get; set; }
        public string EventCode4 { get; set; }
        public string Description4 { get; set; }
        //public string AreaNum { get; set; }

        public string Point5 { get; set; }
        //public string SignalStatus { get; set; }
        public string EventCode5 { get; set; }
        public string Description5 { get; set; }
        //public string AreaNum { get; set; }
    }
    public class UccZone
    {
        public string Point { get; set; }

        public string EventCode { get; set; }
        public string Description { get; set; }

    }
    public class Device
    {
        public string TransmitterCode { get; set; }
        public string DeviceType { get; set; }
        public List<UccZone> Points { get; set; }
        public string ReceiverPhone { get; set; }
        public string PanelPhone { get; set; }
        public object PanelLocation { get; set; }
        public object TimerTestNum { get; set; }
        public object TimerTestHours { get; set; }
        public object TimerTestMinutes { get; set; }
        public object FailTimerTestEvent { get; set; }
        public object PrimaryTransmitterCode { get; set; }
        public object Info { get; set; }
        public bool InServiceFlag { get; set; }
        public object LineSecurity { get; set; }
        public object CommunicationType { get; set; }
        public object AltDeviceID { get; set; }
        public object ListenInDeviceType { get; set; }
        public object URLText { get; set; }
        public object URLTarget { get; set; }
        public object BillingID { get; set; }
        public object SilentFlag { get; set; }
        public DateTime? OOSStartDate { get; set; }
        public List<object> Areas { get; set; }
        public List<object> Schedules { get; set; }
        public List<object> UDF { get; set; }
        public int DevNum { get; set; }
    }

    public class SiteGroup
    {
        public int SiteGroupNum { get; set; }
        public object SiteGroupTypeOverride { get; set; }
    }

    #region Ucc Models

    public class UccTestModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string TransmitterCode { get; set; }
        public string TestCategory { get; set; }
        public string StartDate { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public string Comment { get; set; }

        public Guid CustomerGuidId { get; set; }

    }
    public class NmcTestModel
    {
        public string CsNo { get; set; }
      
        public int TestHour { get; set; }
        public int TestMinute { get; set; }
   
        public Guid CustomerGuidId { get; set; }

    }
    public class Phone
    {
        public string PhoneNumber { get; set; }
        public object Extension { get; set; }
        public object PhoneType { get; set; }
        //public bool AutoNotifyFlag { get; set; }
    }

    public class EmailAddressModel
    {
        public string EmailAddress { get; set; }
        public bool AutoNotifyFlag { get; set; }
    }

    public class EmergencyContactUCC
    {
        public object OrderNum { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public object Title { get; set; }
        public object MiddleName { get; set; }
        public object Suffix { get; set; }
        public List<Phone> Phones { get; set; }
        public List<EmailAddressModel> EmailAddresses { get; set; }
    }

    public class SiteContacts
    {
        public List<EmergencyContactUCC> Contacts { get; set; }
    }

    public class UccSiteContact
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string TransmitterCode { get; set; }
        public SiteContacts SiteContacts { get; set; }
       
    }

    public class UccEmergencyContactModel
    {
        public string FirstName1 { get; set; }
        public string LastName1 { get; set; }
        public string Phone1 { get; set; }
        public string PhoneType1 { get; set; }

        public string FirstName2 { get; set; }
        public string LastName2 { get; set; }
        public string Phone2 { get; set; }
        public string PhoneType2 { get; set; }

        public string FirstName3 { get; set; }
        public string LastName3 { get; set; }
        public string Phone3 { get; set; }
        public string PhoneType3 { get; set; }

        public string FirstName4 { get; set; }
        public string LastName4 { get; set; }
        public string Phone4 { get; set; }
        public string PhoneType4 { get; set; }

        public string TransmitterCode { get; set; }
    }

    public class PhoneObject
    {
        public string PhoneNumber { get; set; }
        public object Extension { get; set; }
        public string PhoneType { get; set; }
        //public bool AutoNotifyFlag { get; set; }
    }
    public class CodewordsObject
    {
        public string Codeword { get; set; }  
    }
    public class UDFObject
    {
        public string UDFCode { get; set; }
        public string UDFValue { get; set; }
    }
    public class UccResultList
    {
        public List<PhoneObject> Phones { get; set; }
        public List<CodewordsObject> Codewords { get; set; }
        public List<object> SiteInstructions { get; set; }
        public List<SiteAgencies> SiteAgencies { get; set; }
        public List<ContactObject> Contacts { get; set; }
        public List<Device> Devices { get; set; }
        public List<DispatchTypesObject> DispatchTypes { get; set; }

        public String[] DispatchTypesList { set; get; }
        public String[] DeviceTypeList { set; get; }
        public string ReceiverPhone { get; set; }
        public string PanelPhone { get; set; }

        public object SiteAddress { get; set; }
        public string SiteName { get; set; }
        public object SiteAddr2 { get; set; }
        public object City { get; set; }
        public object State { get; set; }
        public object ZipCode { get; set; }
        public object County { get; set; }
        public object Latitude { get; set; }
        public object Longitude { get; set; }
        public int TimeZoneNum { get; set; }
        public object CrossStreet { get; set; }
        public object SiteType { get; set; }
        public object Region { get; set; }
        public object Info { get; set; }
        public object Directions { get; set; }
        public object Pets { get; set; }
        public object Map { get; set; }
        public object MapPage { get; set; }
        public object MapCoordinates { get; set; }
        public object SiteID1 { get; set; }
        public object SiteID2 { get; set; }
        public object Subdivision { get; set; }
        public object ULCode { get; set; }
        public object SiteLanguage { get; set; }
        public object LockBoxCode { get; set; }
        public object LockBoxLocation { get; set; }
        public object BillingID { get; set; }
        public List<SiteGroup> SiteGroups { get; set; }
        public List<UDFObject> UDF { get; set; }
        public int SiteNum { get; set; }
        public string TransmitterCode { get; set; }
        public string CodeWord { get; set; }
    }

    public class HistoryList
    {
        public DateTime HistoryDate { get; set; }
        public DateTime UTCDate { get; set; }
        public string TransmitterCode { get; set; }
        public string SiteName { get; set; }
        public string EventCode { get; set; }
        public string EventCodeDescription { get; set; }
        public string OpAct { get; set; }
        public string OpActDescription { get; set; }
        public string CallDisposition { get; set; }
        public string CallDispositionDescription { get; set; }
        public string SignalCode { get; set; }
        public string Point { get; set; }
        public string PointDescription { get; set; }
        public string Comment { get; set; }
        public string AlarmNum { get; set; }
        public string AreaNum { get; set; }
        public string TestNum { get; set; }
        public string RawMessage { get; set; }
        public string Phone { get; set; }
        public string FullClearFlag { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int SiteNum { get; set; }
        public int DevNum { get; set; }
    }
    public class UccCustomerHistory
    {
      public List<HistoryList> Result { get; set; }
    }
    public class DispatchTypesObject
    {
        public string DispatchType { get; set; }
    }
    public class ResultUcc
    {
        public UccResultList Result { get; set; }
    }
    public class CreateUCCCustomer
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public UccResultList Site { get; set; }
    }
    public class SiteAgencies
    {
        public int AgencyNum { get; set; }
        public string AgencyType { get; set; }
        public object Permit { get; set; }
        public string AgencyName { get; set; }
        public string AgencyPhone { get; set; }
    }

    public class ContactPhone
    {
        public string PhoneNumber { get; set; }
        public object Extension { get; set; }
        public string PhoneType { get; set; }
        //public bool AutoNotifyFlag { get; set; }
    }
    public class EmailObject
    {
        public string EmailAddress { get; set; }
        public bool AutoNotifyFlag { get; set; }
    }
    public class DeviceUsers
    {
        public string TransmitterCode { get; set; }
        public string UserId { get; set; }
    }
    public class ContactListObject
    {
        public string ContactListType { get; set; }
        public int OrderNum { get; set; }
    }

    public class AllergiesObject
    {
        public string AllergyCode { get; set; }
        public string Comment { get; set; }
    }
    public class MedicalConditionsObject
    {
        public string MedicalConditionCode { get; set; }
        public string Comment { get; set; }
    }
    public class ContactObject
    {
        public int? OrderNum { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public object Title { get; set; }
        public object MiddleName { get; set; }
        public object Suffix { get; set; }
        public List<Phone> Phones { get; set; }
        public List<EmailObject> EmailAddresses { get; set; }
        public object Pin { get; set; }
        public object Authority { get; set; }
        public object Relation { get; set; }
        public bool EcvFlag { get; set; }
        public bool KeysFlag { get; set; }
    
        public string ContactInfo { get; set; }
        public object ContactType { get; set; }
        public DateTime? ExpireDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public List<DeviceUsers> DeviceUsers { get; set; }
        public List<ContactListObject> ContactList { get; set; }
        public object DateOfBirth { get; set; }
        public object Gender { get; set; }
        public object HospitalPreference { get; set; }
        public object PatientComment { get; set; }
        public List<AllergiesObject> Allergies { get; set; }
        public List<MedicalConditionsObject> MedicalConditions { get; set; }
    }
    public class UccErrorModel
    {
        public string ErrorNum { get; set; }
        public string ErrorMessage { get; set; }
    }
    #endregion

}
