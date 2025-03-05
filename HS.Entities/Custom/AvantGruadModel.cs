using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HS.Entities.Custom
{
    public class AvantGruadModel
    {
        [XmlRoot(ElementName = "Phone", Namespace = "http://tempuri.org/")]
        public class Phone
        {
            [XmlElement(ElementName = "PhoneNumber", Namespace = "http://tempuri.org/")]
            public string PhoneNumber { get; set; }
            [XmlElement(ElementName = "Extension", Namespace = "http://tempuri.org/")]
            public string Extension { get; set; }
            [XmlElement(ElementName = "PhoneType", Namespace = "http://tempuri.org/")]
            public string PhoneType { get; set; }
            [XmlElement(ElementName = "AutoNotifyFlag", Namespace = "http://tempuri.org/")]
            public string AutoNotifyFlag { get; set; }
        }

        [XmlRoot(ElementName = "Phones", Namespace = "http://tempuri.org/")]
        public class Phones
        {
            [XmlElement(ElementName = "Phone", Namespace = "http://tempuri.org/")]
            public Phone Phone { get; set; }
        }

        [XmlRoot(ElementName = "Codeword", Namespace = "http://tempuri.org/")]
        public class Codeword
        {
            [XmlElement(ElementName = "Codeword", Namespace = "http://tempuri.org/")]
            public string Codewords { get; set; }
        }

        [XmlRoot(ElementName = "Codewords", Namespace = "http://tempuri.org/")]
        public class Codewords
        {
            [XmlElement(ElementName = "Codeword", Namespace = "http://tempuri.org/")]
            public Codeword Codeword { get; set; }
        }

        [XmlRoot(ElementName = "SiteInstruction", Namespace = "http://tempuri.org/")]
        public class SiteInstruction
        {
            [XmlElement(ElementName = "SeqNum", Namespace = "http://tempuri.org/")]
            public string SeqNum { get; set; }
            [XmlElement(ElementName = "Description", Namespace = "http://tempuri.org/")]
            public string Description { get; set; }
            [XmlElement(ElementName = "Instruction", Namespace = "http://tempuri.org/")]
            public string Instruction { get; set; }
        }

        [XmlRoot(ElementName = "SiteInstructions", Namespace = "http://tempuri.org/")]
        public class SiteInstructions
        {
            [XmlElement(ElementName = "SiteInstruction", Namespace = "http://tempuri.org/")]
            public SiteInstruction SiteInstruction { get; set; }
        }

        [XmlRoot(ElementName = "SiteAgency", Namespace = "http://tempuri.org/")]
        public class SiteAgency
        {
            [XmlElement(ElementName = "AgencyNum", Namespace = "http://tempuri.org/")]
            public string AgencyNum { get; set; }
            [XmlElement(ElementName = "AgencyType", Namespace = "http://tempuri.org/")]
            public string AgencyType { get; set; }
            [XmlElement(ElementName = "Permit", Namespace = "http://tempuri.org/")]
            public string Permit { get; set; }
            [XmlElement(ElementName = "AgencyName", Namespace = "http://tempuri.org/")]
            public string AgencyName { get; set; }
            [XmlElement(ElementName = "AgencyPhone", Namespace = "http://tempuri.org/")]
            public string AgencyPhone { get; set; }
        }

        [XmlRoot(ElementName = "SiteAgencies", Namespace = "http://tempuri.org/")]
        public class SiteAgencies
        {
            [XmlElement(ElementName = "SiteAgency", Namespace = "http://tempuri.org/")]
            public SiteAgency SiteAgency { get; set; }
        }

        [XmlRoot(ElementName = "EmailAddress", Namespace = "http://tempuri.org/")]
        public class EmailAddress
        {
            [XmlElement(ElementName = "EmailAddress", Namespace = "http://tempuri.org/")]
            public string EmailAddresses { get; set; }
            [XmlElement(ElementName = "AutoNotifyFlag", Namespace = "http://tempuri.org/")]
            public string AutoNotifyFlag { get; set; }
        }

        [XmlRoot(ElementName = "EmailAddresses", Namespace = "http://tempuri.org/")]
        public class EmailAddresses
        {
            [XmlElement(ElementName = "EmailAddress", Namespace = "http://tempuri.org/")]
            public EmailAddress EmailAddress { get; set; }
        }

        [XmlRoot(ElementName = "DeviceUser", Namespace = "http://tempuri.org/")]
        public class DeviceUser
        {
            [XmlElement(ElementName = "TransmitterCode", Namespace = "http://tempuri.org/")]
            public string TransmitterCode { get; set; }
            [XmlElement(ElementName = "UserId", Namespace = "http://tempuri.org/")]
            public string UserId { get; set; }
        }

        [XmlRoot(ElementName = "DeviceUsers", Namespace = "http://tempuri.org/")]
        public class DeviceUsers
        {
            [XmlElement(ElementName = "DeviceUser", Namespace = "http://tempuri.org/")]
            public DeviceUser DeviceUser { get; set; }
        }

        [XmlRoot(ElementName = "ContactListMember", Namespace = "http://tempuri.org/")]
        public class ContactListMember
        {
            [XmlElement(ElementName = "ContactListType", Namespace = "http://tempuri.org/")]
            public string ContactListType { get; set; }
            [XmlElement(ElementName = "OrderNum", Namespace = "http://tempuri.org/")]
            public string OrderNum { get; set; }
        }

        [XmlRoot(ElementName = "ContactList", Namespace = "http://tempuri.org/")]
        public class ContactList
        {
            [XmlElement(ElementName = "ContactListMember", Namespace = "http://tempuri.org/")]
            public ContactListMember ContactListMember { get; set; }
        }

        [XmlRoot(ElementName = "Allergies", Namespace = "http://tempuri.org/")]
        public class Allergies
        {
            [XmlElement(ElementName = "AllergyCode", Namespace = "http://tempuri.org/")]
            public string AllergyCode { get; set; }
            [XmlElement(ElementName = "Comment", Namespace = "http://tempuri.org/")]
            public string Comment { get; set; }
        }

        [XmlRoot(ElementName = "MedicalConditions", Namespace = "http://tempuri.org/")]
        public class MedicalConditions
        {
            [XmlElement(ElementName = "MedicalConditionCode", Namespace = "http://tempuri.org/")]
            public string MedicalConditionCode { get; set; }
            [XmlElement(ElementName = "Comment", Namespace = "http://tempuri.org/")]
            public string Comment { get; set; }
        }

        [XmlRoot(ElementName = "Contact", Namespace = "http://tempuri.org/")]
        public class Contact
        {
            [XmlElement(ElementName = "OrderNum", Namespace = "http://tempuri.org/")]
            public string OrderNum { get; set; }
            [XmlElement(ElementName = "LastName", Namespace = "http://tempuri.org/")]
            public string LastName { get; set; }
            [XmlElement(ElementName = "FirstName", Namespace = "http://tempuri.org/")]
            public string FirstName { get; set; }
            [XmlElement(ElementName = "Title", Namespace = "http://tempuri.org/")]
            public string Title { get; set; }
            [XmlElement(ElementName = "MiddleName", Namespace = "http://tempuri.org/")]
            public string MiddleName { get; set; }
            [XmlElement(ElementName = "Suffix", Namespace = "http://tempuri.org/")]
            public string Suffix { get; set; }
            [XmlElement(ElementName = "Phones", Namespace = "http://tempuri.org/")]
            public Phones Phones { get; set; }
            [XmlElement(ElementName = "EmailAddresses", Namespace = "http://tempuri.org/")]
            public EmailAddresses EmailAddresses { get; set; }
            [XmlElement(ElementName = "Pin", Namespace = "http://tempuri.org/")]
            public string Pin { get; set; }
            [XmlElement(ElementName = "Authority", Namespace = "http://tempuri.org/")]
            public string Authority { get; set; }
            [XmlElement(ElementName = "Relation", Namespace = "http://tempuri.org/")]
            public string Relation { get; set; }
            [XmlElement(ElementName = "EcvFlag", Namespace = "http://tempuri.org/")]
            public string EcvFlag { get; set; }
            [XmlElement(ElementName = "KeysFlag", Namespace = "http://tempuri.org/")]
            public string KeysFlag { get; set; }
            [XmlElement(ElementName = "ContactListMemberOnlyFlag", Namespace = "http://tempuri.org/")]
            public string ContactListMemberOnlyFlag { get; set; }
            [XmlElement(ElementName = "ContactInfo", Namespace = "http://tempuri.org/")]
            public string ContactInfo { get; set; }
            [XmlElement(ElementName = "ContactType", Namespace = "http://tempuri.org/")]
            public string ContactType { get; set; }
            [XmlElement(ElementName = "ExpireDate", Namespace = "http://tempuri.org/")]
            public string ExpireDate { get; set; }
            [XmlElement(ElementName = "EffectiveDate", Namespace = "http://tempuri.org/")]
            public string EffectiveDate { get; set; }
            [XmlElement(ElementName = "DeviceUsers", Namespace = "http://tempuri.org/")]
            public DeviceUsers DeviceUsers { get; set; }
            [XmlElement(ElementName = "ContactList", Namespace = "http://tempuri.org/")]
            public ContactList ContactList { get; set; }
            [XmlElement(ElementName = "DateOfBirth", Namespace = "http://tempuri.org/")]
            public string DateOfBirth { get; set; }
            [XmlElement(ElementName = "Gender", Namespace = "http://tempuri.org/")]
            public string Gender { get; set; }
            [XmlElement(ElementName = "HospitalPreference", Namespace = "http://tempuri.org/")]
            public string HospitalPreference { get; set; }
            [XmlElement(ElementName = "PatientComment", Namespace = "http://tempuri.org/")]
            public string PatientComment { get; set; }
            [XmlElement(ElementName = "Allergies", Namespace = "http://tempuri.org/")]
            public Allergies Allergies { get; set; }
            [XmlElement(ElementName = "MedicalConditions", Namespace = "http://tempuri.org/")]
            public MedicalConditions MedicalConditions { get; set; }
        }

        [XmlRoot(ElementName = "Contacts", Namespace = "http://tempuri.org/")]
        public class Contacts
        {
            [XmlElement(ElementName = "Contact", Namespace = "http://tempuri.org/")]
            public Contact Contact { get; set; }
        }

        [XmlRoot(ElementName = "Point", Namespace = "http://tempuri.org/")]
        public class Point
        {
            [XmlElement(ElementName = "Point", Namespace = "http://tempuri.org/")]
            public string Points { get; set; }
            [XmlElement(ElementName = "SignalStatus", Namespace = "http://tempuri.org/")]
            public string SignalStatus { get; set; }
            [XmlElement(ElementName = "SignalCode", Namespace = "http://tempuri.org/")]
            public string SignalCode { get; set; }
            [XmlElement(ElementName = "EventCode", Namespace = "http://tempuri.org/")]
            public string EventCode { get; set; }
            [XmlElement(ElementName = "Description", Namespace = "http://tempuri.org/")]
            public string Description { get; set; }
            [XmlElement(ElementName = "EqLoc", Namespace = "http://tempuri.org/")]
            public string EqLoc { get; set; }
            [XmlElement(ElementName = "EqType", Namespace = "http://tempuri.org/")]
            public string EqType { get; set; }
            [XmlElement(ElementName = "AreaNum", Namespace = "http://tempuri.org/")]
            public string AreaNum { get; set; }
        }

        [XmlRoot(ElementName = "Points", Namespace = "http://tempuri.org/")]
        public class Points
        {
            [XmlElement(ElementName = "Point", Namespace = "http://tempuri.org/")]
            public Point Point { get; set; }
        }

        [XmlRoot(ElementName = "Area", Namespace = "http://tempuri.org/")]
        public class Area
        {
            [XmlElement(ElementName = "AreaNum", Namespace = "http://tempuri.org/")]
            public string AreaNum { get; set; }
            [XmlElement(ElementName = "Description", Namespace = "http://tempuri.org/")]
            public string Description { get; set; }
            [XmlElement(ElementName = "OpenEventCode", Namespace = "http://tempuri.org/")]
            public string OpenEventCode { get; set; }
            [XmlElement(ElementName = "CloseEventCode", Namespace = "http://tempuri.org/")]
            public string CloseEventCode { get; set; }
            [XmlElement(ElementName = "SchedNum", Namespace = "http://tempuri.org/")]
            public string SchedNum { get; set; }
        }

        [XmlRoot(ElementName = "Areas", Namespace = "http://tempuri.org/")]
        public class Areas
        {
            [XmlElement(ElementName = "Area", Namespace = "http://tempuri.org/")]
            public Area Area { get; set; }
        }

        [XmlRoot(ElementName = "ScheduleDetail", Namespace = "http://tempuri.org/")]
        public class ScheduleDetail
        {
            [XmlElement(ElementName = "OpenDayOfWeek", Namespace = "http://tempuri.org/")]
            public string OpenDayOfWeek { get; set; }
            [XmlElement(ElementName = "OpenTime", Namespace = "http://tempuri.org/")]
            public string OpenTime { get; set; }
            [XmlElement(ElementName = "CloseDayOfWeek", Namespace = "http://tempuri.org/")]
            public string CloseDayOfWeek { get; set; }
            [XmlElement(ElementName = "CloseTime", Namespace = "http://tempuri.org/")]
            public string CloseTime { get; set; }
        }

        [XmlRoot(ElementName = "Details", Namespace = "http://tempuri.org/")]
        public class Details
        {
            [XmlElement(ElementName = "ScheduleDetail", Namespace = "http://tempuri.org/")]
            public ScheduleDetail ScheduleDetail { get; set; }
        }

        [XmlRoot(ElementName = "Schedule", Namespace = "http://tempuri.org/")]
        public class Schedule
        {
            [XmlElement(ElementName = "SchedNum", Namespace = "http://tempuri.org/")]
            public string SchedNum { get; set; }
            [XmlElement(ElementName = "Description", Namespace = "http://tempuri.org/")]
            public string Description { get; set; }
            [XmlElement(ElementName = "FailOpenEventCode", Namespace = "http://tempuri.org/")]
            public string FailOpenEventCode { get; set; }
            [XmlElement(ElementName = "FailCloseEventCode", Namespace = "http://tempuri.org/")]
            public string FailCloseEventCode { get; set; }
            [XmlElement(ElementName = "EarlyOpenMinutes", Namespace = "http://tempuri.org/")]
            public string EarlyOpenMinutes { get; set; }
            [XmlElement(ElementName = "LateOpenMinutes", Namespace = "http://tempuri.org/")]
            public string LateOpenMinutes { get; set; }
            [XmlElement(ElementName = "EarlyCloseMinutes", Namespace = "http://tempuri.org/")]
            public string EarlyCloseMinutes { get; set; }
            [XmlElement(ElementName = "LateCloseMinutes", Namespace = "http://tempuri.org/")]
            public string LateCloseMinutes { get; set; }
            [XmlElement(ElementName = "EarlyOpenEventCode", Namespace = "http://tempuri.org/")]
            public string EarlyOpenEventCode { get; set; }
            [XmlElement(ElementName = "LateOpenEventCode", Namespace = "http://tempuri.org/")]
            public string LateOpenEventCode { get; set; }
            [XmlElement(ElementName = "EarlyCloseEventCode", Namespace = "http://tempuri.org/")]
            public string EarlyCloseEventCode { get; set; }
            [XmlElement(ElementName = "LateCloseEventCode", Namespace = "http://tempuri.org/")]
            public string LateCloseEventCode { get; set; }
            [XmlElement(ElementName = "MultiAreaOption", Namespace = "http://tempuri.org/")]
            public string MultiAreaOption { get; set; }
            [XmlElement(ElementName = "Details", Namespace = "http://tempuri.org/")]
            public Details Details { get; set; }
        }

        [XmlRoot(ElementName = "Schedules", Namespace = "http://tempuri.org/")]
        public class Schedules
        {
            [XmlElement(ElementName = "Schedule", Namespace = "http://tempuri.org/")]
            public Schedule Schedule { get; set; }
        }

        [XmlRoot(ElementName = "UDF", Namespace = "http://tempuri.org/")]
        public class UDF
        {
            [XmlElement(ElementName = "UDFCode", Namespace = "http://tempuri.org/")]
            public string UDFCode { get; set; }
            [XmlElement(ElementName = "UDFValue", Namespace = "http://tempuri.org/")]
            public string UDFValue { get; set; }
            [XmlElement(ElementName = "UDF", Namespace = "http://tempuri.org/")]
            public UDF UDFs { get; set; }
        }

        [XmlRoot(ElementName = "Device", Namespace = "http://tempuri.org/")]
        public class Device
        {
            [XmlElement(ElementName = "TransmitterCode", Namespace = "http://tempuri.org/")]
            public string TransmitterCode { get; set; }
            [XmlElement(ElementName = "DeviceType", Namespace = "http://tempuri.org/")]
            public string DeviceType { get; set; }
            [XmlElement(ElementName = "Points", Namespace = "http://tempuri.org/")]
            public Points Points { get; set; }
            [XmlElement(ElementName = "ReceiverPhone", Namespace = "http://tempuri.org/")]
            public string ReceiverPhone { get; set; }
            [XmlElement(ElementName = "PanelPhone", Namespace = "http://tempuri.org/")]
            public string PanelPhone { get; set; }
            [XmlElement(ElementName = "PanelLocation", Namespace = "http://tempuri.org/")]
            public string PanelLocation { get; set; }
            [XmlElement(ElementName = "TimerTestNum", Namespace = "http://tempuri.org/")]
            public string TimerTestNum { get; set; }
            [XmlElement(ElementName = "TimerTestHours", Namespace = "http://tempuri.org/")]
            public string TimerTestHours { get; set; }
            [XmlElement(ElementName = "TimerTestMinutes", Namespace = "http://tempuri.org/")]
            public string TimerTestMinutes { get; set; }
            [XmlElement(ElementName = "FailTimerTestEvent", Namespace = "http://tempuri.org/")]
            public string FailTimerTestEvent { get; set; }
            [XmlElement(ElementName = "PrimaryTransmitterCode", Namespace = "http://tempuri.org/")]
            public string PrimaryTransmitterCode { get; set; }
            [XmlElement(ElementName = "Info", Namespace = "http://tempuri.org/")]
            public string Info { get; set; }
            [XmlElement(ElementName = "InServiceFlag", Namespace = "http://tempuri.org/")]
            public string InServiceFlag { get; set; }
            [XmlElement(ElementName = "LineSecurity", Namespace = "http://tempuri.org/")]
            public string LineSecurity { get; set; }
            [XmlElement(ElementName = "CommunicationType", Namespace = "http://tempuri.org/")]
            public string CommunicationType { get; set; }
            [XmlElement(ElementName = "AltDeviceID", Namespace = "http://tempuri.org/")]
            public string AltDeviceID { get; set; }
            [XmlElement(ElementName = "ListenInDeviceType", Namespace = "http://tempuri.org/")]
            public string ListenInDeviceType { get; set; }
            [XmlElement(ElementName = "URLText", Namespace = "http://tempuri.org/")]
            public string URLText { get; set; }
            [XmlElement(ElementName = "URLTarget", Namespace = "http://tempuri.org/")]
            public string URLTarget { get; set; }
            [XmlElement(ElementName = "BillingID", Namespace = "http://tempuri.org/")]
            public string BillingID { get; set; }
            [XmlElement(ElementName = "SilentFlag", Namespace = "http://tempuri.org/")]
            public string SilentFlag { get; set; }
            [XmlElement(ElementName = "OOSStartDate", Namespace = "http://tempuri.org/")]
            public string OOSStartDate { get; set; }
            [XmlElement(ElementName = "Areas", Namespace = "http://tempuri.org/")]
            public Areas Areas { get; set; }
            [XmlElement(ElementName = "Schedules", Namespace = "http://tempuri.org/")]
            public Schedules Schedules { get; set; }
            [XmlElement(ElementName = "UDF", Namespace = "http://tempuri.org/")]
            public UDF UDF { get; set; }
            [XmlElement(ElementName = "DevNum", Namespace = "http://tempuri.org/")]
            public string DevNum { get; set; }
        }

        [XmlRoot(ElementName = "Devices", Namespace = "http://tempuri.org/")]
        public class Devices
        {
            [XmlElement(ElementName = "Device", Namespace = "http://tempuri.org/")]
            public Device Device { get; set; }
        }

        [XmlRoot(ElementName = "DispatchType", Namespace = "http://tempuri.org/")]
        public class DispatchType
        {
            [XmlElement(ElementName = "DispatchType", Namespace = "http://tempuri.org/")]
            public string DispatchTypes { get; set; }
        }

        [XmlRoot(ElementName = "DispatchTypes", Namespace = "http://tempuri.org/")]
        public class DispatchTypes
        {
            [XmlElement(ElementName = "DispatchType", Namespace = "http://tempuri.org/")]
            public DispatchType DispatchType { get; set; }
        }

        [XmlRoot(ElementName = "SiteGroup", Namespace = "http://tempuri.org/")]
        public class SiteGroup
        {
            [XmlElement(ElementName = "SiteGroupNum", Namespace = "http://tempuri.org/")]
            public string SiteGroupNum { get; set; }
            [XmlElement(ElementName = "SiteGroupTypeOverride", Namespace = "http://tempuri.org/")]
            public string SiteGroupTypeOverride { get; set; }
        }

        [XmlRoot(ElementName = "SiteGroups", Namespace = "http://tempuri.org/")]
        public class SiteGroups
        {
            [XmlElement(ElementName = "SiteGroup", Namespace = "http://tempuri.org/")]
            public SiteGroup SiteGroup { get; set; }
        }

        [XmlRoot(ElementName = "Site", Namespace = "http://tempuri.org/")]
        public class Site
        {
            [XmlElement(ElementName = "Phones", Namespace = "http://tempuri.org/")]
            public Phones Phones { get; set; }
            [XmlElement(ElementName = "Codewords", Namespace = "http://tempuri.org/")]
            public Codewords Codewords { get; set; }
            [XmlElement(ElementName = "SiteInstructions", Namespace = "http://tempuri.org/")]
            public SiteInstructions SiteInstructions { get; set; }
            [XmlElement(ElementName = "SiteAgencies", Namespace = "http://tempuri.org/")]
            public SiteAgencies SiteAgencies { get; set; }
            [XmlElement(ElementName = "Contacts", Namespace = "http://tempuri.org/")]
            public Contacts Contacts { get; set; }
            [XmlElement(ElementName = "Devices", Namespace = "http://tempuri.org/")]
            public Devices Devices { get; set; }
            [XmlElement(ElementName = "DispatchTypes", Namespace = "http://tempuri.org/")]
            public DispatchTypes DispatchTypes { get; set; }
            [XmlElement(ElementName = "SiteAddress", Namespace = "http://tempuri.org/")]
            public string SiteAddress { get; set; }
            [XmlElement(ElementName = "SiteName", Namespace = "http://tempuri.org/")]
            public string SiteName { get; set; }
            [XmlElement(ElementName = "SiteAddr2", Namespace = "http://tempuri.org/")]
            public string SiteAddr2 { get; set; }
            [XmlElement(ElementName = "City", Namespace = "http://tempuri.org/")]
            public string City { get; set; }
            [XmlElement(ElementName = "State", Namespace = "http://tempuri.org/")]
            public string State { get; set; }
            [XmlElement(ElementName = "ZipCode", Namespace = "http://tempuri.org/")]
            public string ZipCode { get; set; }
            [XmlElement(ElementName = "County", Namespace = "http://tempuri.org/")]
            public string County { get; set; }
            [XmlElement(ElementName = "Latitude", Namespace = "http://tempuri.org/")]
            public string Latitude { get; set; }
            [XmlElement(ElementName = "Longitude", Namespace = "http://tempuri.org/")]
            public string Longitude { get; set; }
            [XmlElement(ElementName = "TimeZoneNum", Namespace = "http://tempuri.org/")]
            public string TimeZoneNum { get; set; }
            [XmlElement(ElementName = "CrossStreet", Namespace = "http://tempuri.org/")]
            public string CrossStreet { get; set; }
            [XmlElement(ElementName = "SiteType", Namespace = "http://tempuri.org/")]
            public string SiteType { get; set; }
            [XmlElement(ElementName = "Region", Namespace = "http://tempuri.org/")]
            public string Region { get; set; }
            [XmlElement(ElementName = "Info", Namespace = "http://tempuri.org/")]
            public string Info { get; set; }
            [XmlElement(ElementName = "Directions", Namespace = "http://tempuri.org/")]
            public string Directions { get; set; }
            [XmlElement(ElementName = "Pets", Namespace = "http://tempuri.org/")]
            public string Pets { get; set; }
            [XmlElement(ElementName = "Map", Namespace = "http://tempuri.org/")]
            public string Map { get; set; }
            [XmlElement(ElementName = "MapPage", Namespace = "http://tempuri.org/")]
            public string MapPage { get; set; }
            [XmlElement(ElementName = "MapCoordinates", Namespace = "http://tempuri.org/")]
            public string MapCoordinates { get; set; }
            [XmlElement(ElementName = "SiteID1", Namespace = "http://tempuri.org/")]
            public string SiteID1 { get; set; }
            [XmlElement(ElementName = "SiteID2", Namespace = "http://tempuri.org/")]
            public string SiteID2 { get; set; }
            [XmlElement(ElementName = "Subdivision", Namespace = "http://tempuri.org/")]
            public string Subdivision { get; set; }
            [XmlElement(ElementName = "ULCode", Namespace = "http://tempuri.org/")]
            public string ULCode { get; set; }
            [XmlElement(ElementName = "SiteLanguage", Namespace = "http://tempuri.org/")]
            public string SiteLanguage { get; set; }
            [XmlElement(ElementName = "LockBoxCode", Namespace = "http://tempuri.org/")]
            public string LockBoxCode { get; set; }
            [XmlElement(ElementName = "LockBoxLocation", Namespace = "http://tempuri.org/")]
            public string LockBoxLocation { get; set; }
            [XmlElement(ElementName = "BillingID", Namespace = "http://tempuri.org/")]
            public string BillingID { get; set; }
            [XmlElement(ElementName = "SiteGroups", Namespace = "http://tempuri.org/")]
            public SiteGroups SiteGroups { get; set; }
            [XmlElement(ElementName = "UDF", Namespace = "http://tempuri.org/")]
            public UDF UDF { get; set; }
            [XmlElement(ElementName = "SiteNum", Namespace = "http://tempuri.org/")]
            public string SiteNum { get; set; }
        }

        [XmlRoot(ElementName = "Signal", Namespace = "http://tempuri.org/")]
        public class Signal
        {
            [XmlElement(ElementName = "TransmitterCode", Namespace = "http://tempuri.org/")]
            public string TransmitterCode { get; set; }
            [XmlElement(ElementName = "SignalFormat", Namespace = "http://tempuri.org/")]
            public string SignalFormat { get; set; }
            [XmlElement(ElementName = "SignalCode", Namespace = "http://tempuri.org/")]
            public string SignalCode { get; set; }
            [XmlElement(ElementName = "Point", Namespace = "http://tempuri.org/")]
            public string Point { get; set; }
            [XmlElement(ElementName = "Area", Namespace = "http://tempuri.org/")]
            public string Area { get; set; }
            [XmlElement(ElementName = "UserID", Namespace = "http://tempuri.org/")]
            public string UserID { get; set; }
            [XmlElement(ElementName = "Text", Namespace = "http://tempuri.org/")]
            public string Text { get; set; }
            [XmlElement(ElementName = "Date", Namespace = "http://tempuri.org/")]
            public string Date { get; set; }
            [XmlElement(ElementName = "ANIPhone", Namespace = "http://tempuri.org/")]
            public string ANIPhone { get; set; }
            [XmlElement(ElementName = "Longitude", Namespace = "http://tempuri.org/")]
            public string Longitude { get; set; }
            [XmlElement(ElementName = "Latitude", Namespace = "http://tempuri.org/")]
            public string Latitude { get; set; }
            [XmlElement(ElementName = "FileName", Namespace = "http://tempuri.org/")]
            public string FileName { get; set; }
            [XmlElement(ElementName = "URL", Namespace = "http://tempuri.org/")]
            public string URL { get; set; }
            [XmlElement(ElementName = "VideoType", Namespace = "http://tempuri.org/")]
            public string VideoType { get; set; }
            [XmlElement(ElementName = "TestSignalFlag", Namespace = "http://tempuri.org/")]
            public string TestSignalFlag { get; set; }
        }

        [XmlRoot(ElementName = "Signals", Namespace = "http://tempuri.org/")]
        public class Signals
        {
            [XmlElement(ElementName = "Signal", Namespace = "http://tempuri.org/")]
            public Signal Signal { get; set; }
        }

        [XmlRoot(ElementName = "ImportSite", Namespace = "http://tempuri.org/")]
        public class ImportSite
        {
            [XmlElement(ElementName = "UserName", Namespace = "http://tempuri.org/")]
            public string UserName { get; set; }
            [XmlElement(ElementName = "Password", Namespace = "http://tempuri.org/")]
            public string Password { get; set; }
            [XmlElement(ElementName = "Site", Namespace = "http://tempuri.org/")]
            public Site Site { get; set; }
            [XmlElement(ElementName = "Signals", Namespace = "http://tempuri.org/")]
            public Signals Signals { get; set; }
            [XmlAttribute(AttributeName = "xmlns")]
            public string Xmlns { get; set; }
        }

        [XmlRoot(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public class Body
        {
            [XmlElement(ElementName = "ImportSite", Namespace = "http://tempuri.org/")]
            public ImportSite ImportSite { get; set; }
        }

        [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public class Envelope
        {
            [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
            public Body Body { get; set; }
            [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
            public string Xsi { get; set; }
            [XmlAttribute(AttributeName = "xsd", Namespace = "http://www.w3.org/2000/xmlns/")]
            public string Xsd { get; set; }
            [XmlAttribute(AttributeName = "soap", Namespace = "http://www.w3.org/2000/xmlns/")]
            public string Soap { get; set; }
        }

    }
}
