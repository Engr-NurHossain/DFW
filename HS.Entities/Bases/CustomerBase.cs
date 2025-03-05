using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomerBase", Namespace = "http://www.hims-tech.com//entities")]
	public class CustomerBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CustomerId = 1,
			CustomerNo = 2,
			Title = 3,
			FirstName = 4,
			LastName = 5,
			SSN = 6,
			Type = 7,
			BusinessName = 8,
			DateofBirth = 9,
			PrimaryPhone = 10,
			SecondaryPhone = 11,
			CellNo = 12,
			Fax = 13,
			EmailAddress = 14,
			CallingTime = 15,
			Address = 16,
			Address2 = 17,
			Street = 18,
			City = 19,
			State = 20,
			ZipCode = 21,
			Country = 22,
			StreetPrevious = 23,
			CityPrevious = 24,
			StatePrevious = 25,
			ZipCodePrevious = 26,
			CountryPrevious = 27,
			AccountNo = 28,
			IsAlarmCom = 29,
			CreditScore = 30,
			CreditScoreValue = 31,
			ContractTeam = 32,
			FundingCompany = 33,
			MonthlyMonitoringFee = 34,
			CellularBackup = 35,
			LeadSource = 36,
			CustomerFunded = 37,
			Maintenance = 38,
			Note = 39,
			SalesDate = 40,
			InstallDate = 41,
			CutInDate = 42,
			Installer = 43,
			Soldby = 44,
			FundingDate = 45,
			MiddleName = 46,
			JoinDate = 47,
			ReminderDate = 48,
			QA1 = 49,
			QA1Date = 50,
			QA2 = 51,
			QA2Date = 52,
			Status = 53,
			BillAmount = 54,
			PaymentMethod = 55,
			BillCycle = 56,
			BillDay = 57,
			BillNotes = 58,
			BillTax = 59,
			BillOutStanding = 60,
			ServiceDate = 61,
			Area = 62,
			StreetType = 63,
			Appartment = 64,
			Latlng = 65,
			SecondCustomerNo = 66,
			AdditionalCustomerNo = 67,
			IsTechCallPassed = 68,
			IsDirect = 69,
			AuthorizeRefId = 70,
			AuthorizeCusProfileId = 71,
			AuthorizeCusPaymentProfileId = 72,
			AuthorizeDescription = 73,
			IsRequiredCsvSync = 74,
			Passcode = 75,
			ActivationFee = 76,
			FirstBilling = 77,
			ActivationFeePaymentMethod = 78,
			IsActive = 79,
			LastGeneratedInvoice = 80,
			Singature = 81,
			CrossStreet = 82,
			DBA = 83,
			AlarmRefId = 84,
			TransunionRefId = 85,
			MonitronicsRefId = 86,
			CentralStationRefId = 87,
			CmsRefId = 88,
			PreferedEmail = 89,
			PreferedSms = 90,
			IsAgreement = 91,
			IsFireAccount = 92,
			CreatedByUid = 93,
			CreatedDate = 94,
			LastUpdatedBy = 95,
			LastUpdatedByUid = 96,
			LastUpdatedDate = 97,
			BusinessAccountType = 98,
			PhoneType = 99,
			Carrier = 100,
			ReferringCustomer = 101,
			EsistingPanel = 102,
			Ownership = 103,
			PurchasePrice = 104,
			ContractValue = 105,
			ChildOf = 106,
			EmailVerified = 107,
			HomeVerified = 108,
			County = 109,
			CustomerToken = 110,
			PaymentToken = 111,
			ScheduleToken = 112,
			EstCloseDate = 113,
			ProjectWalkDate = 114,
			BranchId = 115,
			SubscriptionStatus = 116,
			AnnualRevenue = 117,
			Website = 118,
			Market = 119,
			Passengers = 120,
			Budget = 121,
			SmartSetUpStep = 122,
			CustomerAccountType = 123,
			IsPrimaryPhoneVerified = 124,
			IsSecondaryPhoneVerified = 125,
			IsCellNoVerified = 126,
			HomeOwner = 127,
			AccessGivenTo = 128,
			DoNotCall = 129,
			PreferredContactMethod = 130,
			SalesLocation = 131,
			IsReceivePaymentMail = 132,
			BestTimeToCall = 133,
			CSProvider = 134,
			RenewalTerm = 135,
			PreferedCall = 136,
			IsAgreementSend = 137,
			IsACHDiscount = 138,
			CustomerStatus = 139,
			TransferCustomerId = 140,
			CancellationSignature = 141,
			OriginalContactDate = 142,
			DuplicateCustomer = 143,
			InspectionCompany = 144,
			SoldBy2 = 145,
			SoldBy3 = 146,
			ContactedPerviously = 147,
			MovingDate = 148,
			InstalledStatus = 149,
			AcquiredFrom = 150,
			FollowUpDate = 151,
			BuyoutAmountByADS = 152,
			BuyoutAmountBySalesRep = 153,
			FinancedTerm = 154,
			FinancedAmount = 155,
			Levels = 156,
			SoldAmount = 157,
			AgreementEmail = 158,
			AgreementPhoneNo = 159,
			TaxExemption = 160,
			AppoinmentSet = 161,
			BillingEmail = 162,
			BillingPhone = 163,
			BillingContact = 164,
			LastOpenedDate = 165,
			DisconnectServiceDate = 166,
			UCCRefId = 167,
			PlatformId = 168,
			RecommendedLevel = 169,
			ProfileImage = 170,
			BrinksRefId = 171,
			MapscoNo = 172,
			CustomerSignatureDate = 173,
			LeadSourceType = 174,
			MoveCustomerId = 175,
			EcontractId = 176,
			EcontractEnvlobeId = 177,
			Soldby1 = 178,
			SearchText = 179,
			IsContractSigned = 180,
			CustomerFundedDate = 181
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_CustomerNo = "CustomerNo";		            
		public const string Property_Title = "Title";		            
		public const string Property_FirstName = "FirstName";		            
		public const string Property_LastName = "LastName";		            
		public const string Property_SSN = "SSN";		            
		public const string Property_Type = "Type";		            
		public const string Property_BusinessName = "BusinessName";		            
		public const string Property_DateofBirth = "DateofBirth";		            
		public const string Property_PrimaryPhone = "PrimaryPhone";		            
		public const string Property_SecondaryPhone = "SecondaryPhone";		            
		public const string Property_CellNo = "CellNo";		            
		public const string Property_Fax = "Fax";		            
		public const string Property_EmailAddress = "EmailAddress";		            
		public const string Property_CallingTime = "CallingTime";		            
		public const string Property_Address = "Address";		            
		public const string Property_Address2 = "Address2";		            
		public const string Property_Street = "Street";		            
		public const string Property_City = "City";		            
		public const string Property_State = "State";		            
		public const string Property_ZipCode = "ZipCode";		            
		public const string Property_Country = "Country";		            
		public const string Property_StreetPrevious = "StreetPrevious";		            
		public const string Property_CityPrevious = "CityPrevious";		            
		public const string Property_StatePrevious = "StatePrevious";		            
		public const string Property_ZipCodePrevious = "ZipCodePrevious";		            
		public const string Property_CountryPrevious = "CountryPrevious";		            
		public const string Property_AccountNo = "AccountNo";		            
		public const string Property_IsAlarmCom = "IsAlarmCom";		            
		public const string Property_CreditScore = "CreditScore";		            
		public const string Property_CreditScoreValue = "CreditScoreValue";		            
		public const string Property_ContractTeam = "ContractTeam";		            
		public const string Property_FundingCompany = "FundingCompany";		            
		public const string Property_MonthlyMonitoringFee = "MonthlyMonitoringFee";		            
		public const string Property_CellularBackup = "CellularBackup";		            
		public const string Property_LeadSource = "LeadSource";		            
		public const string Property_CustomerFunded = "CustomerFunded";		            
		public const string Property_Maintenance = "Maintenance";		            
		public const string Property_Note = "Note";		            
		public const string Property_SalesDate = "SalesDate";		            
		public const string Property_InstallDate = "InstallDate";		            
		public const string Property_CutInDate = "CutInDate";		            
		public const string Property_Installer = "Installer";		            
		public const string Property_Soldby = "Soldby";		            
		public const string Property_FundingDate = "FundingDate";		            
		public const string Property_MiddleName = "MiddleName";		            
		public const string Property_JoinDate = "JoinDate";		            
		public const string Property_ReminderDate = "ReminderDate";		            
		public const string Property_QA1 = "QA1";		            
		public const string Property_QA1Date = "QA1Date";		            
		public const string Property_QA2 = "QA2";		            
		public const string Property_QA2Date = "QA2Date";		            
		public const string Property_Status = "Status";		            
		public const string Property_BillAmount = "BillAmount";		            
		public const string Property_PaymentMethod = "PaymentMethod";		            
		public const string Property_BillCycle = "BillCycle";		            
		public const string Property_BillDay = "BillDay";		            
		public const string Property_BillNotes = "BillNotes";		            
		public const string Property_BillTax = "BillTax";		            
		public const string Property_BillOutStanding = "BillOutStanding";		            
		public const string Property_ServiceDate = "ServiceDate";		            
		public const string Property_Area = "Area";		            
		public const string Property_StreetType = "StreetType";		            
		public const string Property_Appartment = "Appartment";		            
		public const string Property_Latlng = "Latlng";		            
		public const string Property_SecondCustomerNo = "SecondCustomerNo";		            
		public const string Property_AdditionalCustomerNo = "AdditionalCustomerNo";		            
		public const string Property_IsTechCallPassed = "IsTechCallPassed";		            
		public const string Property_IsDirect = "IsDirect";		            
		public const string Property_AuthorizeRefId = "AuthorizeRefId";		            
		public const string Property_AuthorizeCusProfileId = "AuthorizeCusProfileId";		            
		public const string Property_AuthorizeCusPaymentProfileId = "AuthorizeCusPaymentProfileId";		            
		public const string Property_AuthorizeDescription = "AuthorizeDescription";		            
		public const string Property_IsRequiredCsvSync = "IsRequiredCsvSync";		            
		public const string Property_Passcode = "Passcode";		            
		public const string Property_ActivationFee = "ActivationFee";		            
		public const string Property_FirstBilling = "FirstBilling";		            
		public const string Property_ActivationFeePaymentMethod = "ActivationFeePaymentMethod";		            
		public const string Property_IsActive = "IsActive";		            
		public const string Property_LastGeneratedInvoice = "LastGeneratedInvoice";		            
		public const string Property_Singature = "Singature";		            
		public const string Property_CrossStreet = "CrossStreet";		            
		public const string Property_DBA = "DBA";		            
		public const string Property_AlarmRefId = "AlarmRefId";		            
		public const string Property_TransunionRefId = "TransunionRefId";		            
		public const string Property_MonitronicsRefId = "MonitronicsRefId";		            
		public const string Property_CentralStationRefId = "CentralStationRefId";		            
		public const string Property_CmsRefId = "CmsRefId";		            
		public const string Property_PreferedEmail = "PreferedEmail";		            
		public const string Property_PreferedSms = "PreferedSms";		            
		public const string Property_IsAgreement = "IsAgreement";		            
		public const string Property_IsFireAccount = "IsFireAccount";		            
		public const string Property_CreatedByUid = "CreatedByUid";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedByUid = "LastUpdatedByUid";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_BusinessAccountType = "BusinessAccountType";		            
		public const string Property_PhoneType = "PhoneType";		            
		public const string Property_Carrier = "Carrier";		            
		public const string Property_ReferringCustomer = "ReferringCustomer";		            
		public const string Property_EsistingPanel = "EsistingPanel";		            
		public const string Property_Ownership = "Ownership";		            
		public const string Property_PurchasePrice = "PurchasePrice";		            
		public const string Property_ContractValue = "ContractValue";		            
		public const string Property_ChildOf = "ChildOf";		            
		public const string Property_EmailVerified = "EmailVerified";		            
		public const string Property_HomeVerified = "HomeVerified";		            
		public const string Property_County = "County";		            
		public const string Property_CustomerToken = "CustomerToken";		            
		public const string Property_PaymentToken = "PaymentToken";		            
		public const string Property_ScheduleToken = "ScheduleToken";		            
		public const string Property_EstCloseDate = "EstCloseDate";		            
		public const string Property_ProjectWalkDate = "ProjectWalkDate";		            
		public const string Property_BranchId = "BranchId";		            
		public const string Property_SubscriptionStatus = "SubscriptionStatus";		            
		public const string Property_AnnualRevenue = "AnnualRevenue";		            
		public const string Property_Website = "Website";		            
		public const string Property_Market = "Market";		            
		public const string Property_Passengers = "Passengers";		            
		public const string Property_Budget = "Budget";		            
		public const string Property_SmartSetUpStep = "SmartSetUpStep";		            
		public const string Property_CustomerAccountType = "CustomerAccountType";		            
		public const string Property_IsPrimaryPhoneVerified = "IsPrimaryPhoneVerified";		            
		public const string Property_IsSecondaryPhoneVerified = "IsSecondaryPhoneVerified";		            
		public const string Property_IsCellNoVerified = "IsCellNoVerified";		            
		public const string Property_HomeOwner = "HomeOwner";		            
		public const string Property_AccessGivenTo = "AccessGivenTo";		            
		public const string Property_DoNotCall = "DoNotCall";		            
		public const string Property_PreferredContactMethod = "PreferredContactMethod";		            
		public const string Property_SalesLocation = "SalesLocation";		            
		public const string Property_IsReceivePaymentMail = "IsReceivePaymentMail";		            
		public const string Property_BestTimeToCall = "BestTimeToCall";		            
		public const string Property_CSProvider = "CSProvider";		            
		public const string Property_RenewalTerm = "RenewalTerm";		            
		public const string Property_PreferedCall = "PreferedCall";		            
		public const string Property_IsAgreementSend = "IsAgreementSend";		            
		public const string Property_IsACHDiscount = "IsACHDiscount";		            
		public const string Property_CustomerStatus = "CustomerStatus";		            
		public const string Property_TransferCustomerId = "TransferCustomerId";		            
		public const string Property_CancellationSignature = "CancellationSignature";		            
		public const string Property_OriginalContactDate = "OriginalContactDate";		            
		public const string Property_DuplicateCustomer = "DuplicateCustomer";		            
		public const string Property_InspectionCompany = "InspectionCompany";		            
		public const string Property_SoldBy2 = "SoldBy2";		            
		public const string Property_SoldBy3 = "SoldBy3";		            
		public const string Property_ContactedPerviously = "ContactedPerviously";		            
		public const string Property_MovingDate = "MovingDate";		            
		public const string Property_InstalledStatus = "InstalledStatus";		            
		public const string Property_AcquiredFrom = "AcquiredFrom";		            
		public const string Property_FollowUpDate = "FollowUpDate";		            
		public const string Property_BuyoutAmountByADS = "BuyoutAmountByADS";		            
		public const string Property_BuyoutAmountBySalesRep = "BuyoutAmountBySalesRep";		            
		public const string Property_FinancedTerm = "FinancedTerm";		            
		public const string Property_FinancedAmount = "FinancedAmount";		            
		public const string Property_Levels = "Levels";		            
		public const string Property_SoldAmount = "SoldAmount";		            
		public const string Property_AgreementEmail = "AgreementEmail";		            
		public const string Property_AgreementPhoneNo = "AgreementPhoneNo";		            
		public const string Property_TaxExemption = "TaxExemption";		            
		public const string Property_AppoinmentSet = "AppoinmentSet";		            
		public const string Property_BillingEmail = "BillingEmail";		            
		public const string Property_BillingPhone = "BillingPhone";		            
		public const string Property_BillingContact = "BillingContact";		            
		public const string Property_LastOpenedDate = "LastOpenedDate";		            
		public const string Property_DisconnectServiceDate = "DisconnectServiceDate";		            
		public const string Property_UCCRefId = "UCCRefId";		            
		public const string Property_PlatformId = "PlatformId";		            
		public const string Property_RecommendedLevel = "RecommendedLevel";		            
		public const string Property_ProfileImage = "ProfileImage";		            
		public const string Property_BrinksRefId = "BrinksRefId";		            
		public const string Property_MapscoNo = "MapscoNo";		            
		public const string Property_CustomerSignatureDate = "CustomerSignatureDate";		            
		public const string Property_LeadSourceType = "LeadSourceType";		            
		public const string Property_MoveCustomerId = "MoveCustomerId";		            
		public const string Property_EcontractId = "EcontractId";		            
		public const string Property_EcontractEnvlobeId = "EcontractEnvlobeId";		            
		public const string Property_Soldby1 = "Soldby1";		            
		public const string Property_SearchText = "SearchText";		            
		public const string Property_IsContractSigned = "IsContractSigned";		            
		public const string Property_CustomerFundedDate = "CustomerFundedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CustomerId;	            
		private String _CustomerNo;	            
		private String _Title;	            
		private String _FirstName;	            
		private String _LastName;	            
		private String _SSN;	            
		private String _Type;	            
		private String _BusinessName;	            
		private Nullable<DateTime> _DateofBirth;	            
		private String _PrimaryPhone;	            
		private String _SecondaryPhone;	            
		private String _CellNo;	            
		private String _Fax;	            
		private String _EmailAddress;	            
		private String _CallingTime;	            
		private String _Address;	            
		private String _Address2;	            
		private String _Street;	            
		private String _City;	            
		private String _State;	            
		private String _ZipCode;	            
		private String _Country;	            
		private String _StreetPrevious;	            
		private String _CityPrevious;	            
		private String _StatePrevious;	            
		private String _ZipCodePrevious;	            
		private String _CountryPrevious;	            
		private String _AccountNo;	            
		private Nullable<Boolean> _IsAlarmCom;	            
		private String _CreditScore;	            
		private Nullable<Int32> _CreditScoreValue;	            
		private String _ContractTeam;	            
		private String _FundingCompany;	            
		private String _MonthlyMonitoringFee;	            
		private Nullable<Boolean> _CellularBackup;	            
		private String _LeadSource;	            
		private Nullable<Boolean> _CustomerFunded;	            
		private Nullable<Boolean> _Maintenance;	            
		private String _Note;	            
		private Nullable<DateTime> _SalesDate;	            
		private Nullable<DateTime> _InstallDate;	            
		private Nullable<DateTime> _CutInDate;	            
		private String _Installer;	            
		private String _Soldby;	            
		private Nullable<DateTime> _FundingDate;	            
		private String _MiddleName;	            
		private Nullable<DateTime> _JoinDate;	            
		private Nullable<DateTime> _ReminderDate;	            
		private String _QA1;	            
		private Nullable<DateTime> _QA1Date;	            
		private String _QA2;	            
		private Nullable<DateTime> _QA2Date;	            
		private String _Status;	            
		private Nullable<Double> _BillAmount;	            
		private String _PaymentMethod;	            
		private String _BillCycle;	            
		private Nullable<Int32> _BillDay;	            
		private String _BillNotes;	            
		private Nullable<Boolean> _BillTax;	            
		private Nullable<Double> _BillOutStanding;	            
		private Nullable<DateTime> _ServiceDate;	            
		private String _Area;	            
		private String _StreetType;	            
		private String _Appartment;	            
		private String _Latlng;	            
		private String _SecondCustomerNo;	            
		private String _AdditionalCustomerNo;	            
		private Nullable<Boolean> _IsTechCallPassed;	            
		private Nullable<Boolean> _IsDirect;	            
		private String _AuthorizeRefId;	            
		private String _AuthorizeCusProfileId;	            
		private String _AuthorizeCusPaymentProfileId;	            
		private String _AuthorizeDescription;	            
		private Nullable<Boolean> _IsRequiredCsvSync;	            
		private String _Passcode;	            
		private Nullable<Double> _ActivationFee;	            
		private Nullable<DateTime> _FirstBilling;	            
		private String _ActivationFeePaymentMethod;	            
		private Nullable<Boolean> _IsActive;	            
		private Nullable<DateTime> _LastGeneratedInvoice;	            
		private String _Singature;	            
		private String _CrossStreet;	            
		private String _DBA;	            
		private String _AlarmRefId;	            
		private String _TransunionRefId;	            
		private String _MonitronicsRefId;	            
		private String _CentralStationRefId;	            
		private String _CmsRefId;	            
		private Nullable<Boolean> _PreferedEmail;	            
		private Nullable<Boolean> _PreferedSms;	            
		private Nullable<Boolean> _IsAgreement;	            
		private Nullable<Boolean> _IsFireAccount;	            
		private Guid _CreatedByUid;	            
		private DateTime _CreatedDate;	            
		private String _LastUpdatedBy;	            
		private Guid _LastUpdatedByUid;	            
		private DateTime _LastUpdatedDate;	            
		private String _BusinessAccountType;	            
		private String _PhoneType;	            
		private String _Carrier;	            
		private Guid _ReferringCustomer;	            
		private String _EsistingPanel;	            
		private String _Ownership;	            
		private Nullable<Double> _PurchasePrice;	            
		private String _ContractValue;	            
		private Guid _ChildOf;	            
		private Nullable<Boolean> _EmailVerified;	            
		private Nullable<Boolean> _HomeVerified;	            
		private String _County;	            
		private String _CustomerToken;	            
		private String _PaymentToken;	            
		private String _ScheduleToken;	            
		private Nullable<DateTime> _EstCloseDate;	            
		private Nullable<DateTime> _ProjectWalkDate;	            
		private Nullable<Int32> _BranchId;	            
		private String _SubscriptionStatus;	            
		private Nullable<Double> _AnnualRevenue;	            
		private String _Website;	            
		private String _Market;	            
		private Nullable<Int32> _Passengers;	            
		private Nullable<Double> _Budget;	            
		private Nullable<Int32> _SmartSetUpStep;	            
		private String _CustomerAccountType;	            
		private Nullable<Boolean> _IsPrimaryPhoneVerified;	            
		private Nullable<Boolean> _IsSecondaryPhoneVerified;	            
		private Nullable<Boolean> _IsCellNoVerified;	            
		private String _HomeOwner;	            
		private Guid _AccessGivenTo;	            
		private Nullable<DateTime> _DoNotCall;	            
		private String _PreferredContactMethod;	            
		private String _SalesLocation;	            
		private Nullable<Boolean> _IsReceivePaymentMail;	            
		private String _BestTimeToCall;	            
		private String _CSProvider;	            
		private Nullable<Int32> _RenewalTerm;	            
		private Nullable<Boolean> _PreferedCall;	            
		private Nullable<Boolean> _IsAgreementSend;	            
		private Nullable<Boolean> _IsACHDiscount;	            
		private String _CustomerStatus;	            
		private Nullable<Int32> _TransferCustomerId;	            
		private String _CancellationSignature;	            
		private Nullable<DateTime> _OriginalContactDate;	            
		private Guid _DuplicateCustomer;	            
		private String _InspectionCompany;	            
		private Guid _SoldBy2;	            
		private Guid _SoldBy3;	            
		private String _ContactedPerviously;	            
		private Nullable<DateTime> _MovingDate;	            
		private String _InstalledStatus;	            
		private String _AcquiredFrom;	            
		private Nullable<DateTime> _FollowUpDate;	            
		private Nullable<Double> _BuyoutAmountByADS;	            
		private Nullable<Double> _BuyoutAmountBySalesRep;	            
		private Nullable<Double> _FinancedTerm;	            
		private Nullable<Double> _FinancedAmount;	            
		private Nullable<Double> _Levels;	            
		private Nullable<Double> _SoldAmount;	            
		private String _AgreementEmail;	            
		private String _AgreementPhoneNo;	            
		private String _TaxExemption;	            
		private String _AppoinmentSet;	            
		private String _BillingEmail;	            
		private String _BillingPhone;	            
		private String _BillingContact;	            
		private Nullable<DateTime> _LastOpenedDate;	            
		private Nullable<DateTime> _DisconnectServiceDate;	            
		private String _UCCRefId;	            
		private String _PlatformId;	            
		private Nullable<Int32> _RecommendedLevel;	            
		private String _ProfileImage;	            
		private String _BrinksRefId;	            
		private String _MapscoNo;	            
		private Nullable<DateTime> _CustomerSignatureDate;	            
		private String _LeadSourceType;	            
		private Guid _MoveCustomerId;	            
		private String _EcontractId;	            
		private String _EcontractEnvlobeId;	            
		private Guid _Soldby1;	            
		private String _SearchText;	            
		private Nullable<Boolean> _IsContractSigned;	            
		private Nullable<DateTime> _CustomerFundedDate;	            
		#endregion
		
		#region Properties		
		[DataMember]
		public Int32 Id
		{	
			get{ return _Id; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Id, value, _Id);
				if (PropertyChanging(args))
				{
					_Id = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid CustomerId
		{	
			get{ return _CustomerId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CustomerId, value, _CustomerId);
				if (PropertyChanging(args))
				{
					_CustomerId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CustomerNo
		{	
			get{ return _CustomerNo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CustomerNo, value, _CustomerNo);
				if (PropertyChanging(args))
				{
					_CustomerNo = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Title
		{	
			get{ return _Title; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Title, value, _Title);
				if (PropertyChanging(args))
				{
					_Title = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String FirstName
		{	
			get{ return _FirstName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FirstName, value, _FirstName);
				if (PropertyChanging(args))
				{
					_FirstName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String LastName
		{	
			get{ return _LastName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastName, value, _LastName);
				if (PropertyChanging(args))
				{
					_LastName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SSN
		{	
			get{ return _SSN; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SSN, value, _SSN);
				if (PropertyChanging(args))
				{
					_SSN = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Type
		{	
			get{ return _Type; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Type, value, _Type);
				if (PropertyChanging(args))
				{
					_Type = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BusinessName
		{	
			get{ return _BusinessName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BusinessName, value, _BusinessName);
				if (PropertyChanging(args))
				{
					_BusinessName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> DateofBirth
		{	
			get{ return _DateofBirth; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DateofBirth, value, _DateofBirth);
				if (PropertyChanging(args))
				{
					_DateofBirth = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PrimaryPhone
		{	
			get{ return _PrimaryPhone; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PrimaryPhone, value, _PrimaryPhone);
				if (PropertyChanging(args))
				{
					_PrimaryPhone = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SecondaryPhone
		{	
			get{ return _SecondaryPhone; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SecondaryPhone, value, _SecondaryPhone);
				if (PropertyChanging(args))
				{
					_SecondaryPhone = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CellNo
		{	
			get{ return _CellNo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CellNo, value, _CellNo);
				if (PropertyChanging(args))
				{
					_CellNo = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Fax
		{	
			get{ return _Fax; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Fax, value, _Fax);
				if (PropertyChanging(args))
				{
					_Fax = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String EmailAddress
		{	
			get{ return _EmailAddress; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EmailAddress, value, _EmailAddress);
				if (PropertyChanging(args))
				{
					_EmailAddress = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CallingTime
		{	
			get{ return _CallingTime; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CallingTime, value, _CallingTime);
				if (PropertyChanging(args))
				{
					_CallingTime = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Address
		{	
			get{ return _Address; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Address, value, _Address);
				if (PropertyChanging(args))
				{
					_Address = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Address2
		{	
			get{ return _Address2; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Address2, value, _Address2);
				if (PropertyChanging(args))
				{
					_Address2 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Street
		{	
			get{ return _Street; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Street, value, _Street);
				if (PropertyChanging(args))
				{
					_Street = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String City
		{	
			get{ return _City; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_City, value, _City);
				if (PropertyChanging(args))
				{
					_City = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String State
		{	
			get{ return _State; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_State, value, _State);
				if (PropertyChanging(args))
				{
					_State = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ZipCode
		{	
			get{ return _ZipCode; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ZipCode, value, _ZipCode);
				if (PropertyChanging(args))
				{
					_ZipCode = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Country
		{	
			get{ return _Country; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Country, value, _Country);
				if (PropertyChanging(args))
				{
					_Country = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String StreetPrevious
		{	
			get{ return _StreetPrevious; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_StreetPrevious, value, _StreetPrevious);
				if (PropertyChanging(args))
				{
					_StreetPrevious = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CityPrevious
		{	
			get{ return _CityPrevious; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CityPrevious, value, _CityPrevious);
				if (PropertyChanging(args))
				{
					_CityPrevious = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String StatePrevious
		{	
			get{ return _StatePrevious; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_StatePrevious, value, _StatePrevious);
				if (PropertyChanging(args))
				{
					_StatePrevious = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ZipCodePrevious
		{	
			get{ return _ZipCodePrevious; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ZipCodePrevious, value, _ZipCodePrevious);
				if (PropertyChanging(args))
				{
					_ZipCodePrevious = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CountryPrevious
		{	
			get{ return _CountryPrevious; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CountryPrevious, value, _CountryPrevious);
				if (PropertyChanging(args))
				{
					_CountryPrevious = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AccountNo
		{	
			get{ return _AccountNo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AccountNo, value, _AccountNo);
				if (PropertyChanging(args))
				{
					_AccountNo = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsAlarmCom
		{	
			get{ return _IsAlarmCom; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsAlarmCom, value, _IsAlarmCom);
				if (PropertyChanging(args))
				{
					_IsAlarmCom = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CreditScore
		{	
			get{ return _CreditScore; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreditScore, value, _CreditScore);
				if (PropertyChanging(args))
				{
					_CreditScore = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> CreditScoreValue
		{	
			get{ return _CreditScoreValue; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreditScoreValue, value, _CreditScoreValue);
				if (PropertyChanging(args))
				{
					_CreditScoreValue = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ContractTeam
		{	
			get{ return _ContractTeam; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ContractTeam, value, _ContractTeam);
				if (PropertyChanging(args))
				{
					_ContractTeam = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String FundingCompany
		{	
			get{ return _FundingCompany; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FundingCompany, value, _FundingCompany);
				if (PropertyChanging(args))
				{
					_FundingCompany = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String MonthlyMonitoringFee
		{	
			get{ return _MonthlyMonitoringFee; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MonthlyMonitoringFee, value, _MonthlyMonitoringFee);
				if (PropertyChanging(args))
				{
					_MonthlyMonitoringFee = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> CellularBackup
		{	
			get{ return _CellularBackup; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CellularBackup, value, _CellularBackup);
				if (PropertyChanging(args))
				{
					_CellularBackup = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String LeadSource
		{	
			get{ return _LeadSource; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LeadSource, value, _LeadSource);
				if (PropertyChanging(args))
				{
					_LeadSource = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> CustomerFunded
		{	
			get{ return _CustomerFunded; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CustomerFunded, value, _CustomerFunded);
				if (PropertyChanging(args))
				{
					_CustomerFunded = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> Maintenance
		{	
			get{ return _Maintenance; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Maintenance, value, _Maintenance);
				if (PropertyChanging(args))
				{
					_Maintenance = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Note
		{	
			get{ return _Note; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Note, value, _Note);
				if (PropertyChanging(args))
				{
					_Note = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> SalesDate
		{	
			get{ return _SalesDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SalesDate, value, _SalesDate);
				if (PropertyChanging(args))
				{
					_SalesDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> InstallDate
		{	
			get{ return _InstallDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InstallDate, value, _InstallDate);
				if (PropertyChanging(args))
				{
					_InstallDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> CutInDate
		{	
			get{ return _CutInDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CutInDate, value, _CutInDate);
				if (PropertyChanging(args))
				{
					_CutInDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Installer
		{	
			get{ return _Installer; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Installer, value, _Installer);
				if (PropertyChanging(args))
				{
					_Installer = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Soldby
		{	
			get{ return _Soldby; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Soldby, value, _Soldby);
				if (PropertyChanging(args))
				{
					_Soldby = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> FundingDate
		{	
			get{ return _FundingDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FundingDate, value, _FundingDate);
				if (PropertyChanging(args))
				{
					_FundingDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String MiddleName
		{	
			get{ return _MiddleName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MiddleName, value, _MiddleName);
				if (PropertyChanging(args))
				{
					_MiddleName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> JoinDate
		{	
			get{ return _JoinDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_JoinDate, value, _JoinDate);
				if (PropertyChanging(args))
				{
					_JoinDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> ReminderDate
		{	
			get{ return _ReminderDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReminderDate, value, _ReminderDate);
				if (PropertyChanging(args))
				{
					_ReminderDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String QA1
		{	
			get{ return _QA1; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_QA1, value, _QA1);
				if (PropertyChanging(args))
				{
					_QA1 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> QA1Date
		{	
			get{ return _QA1Date; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_QA1Date, value, _QA1Date);
				if (PropertyChanging(args))
				{
					_QA1Date = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String QA2
		{	
			get{ return _QA2; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_QA2, value, _QA2);
				if (PropertyChanging(args))
				{
					_QA2 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> QA2Date
		{	
			get{ return _QA2Date; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_QA2Date, value, _QA2Date);
				if (PropertyChanging(args))
				{
					_QA2Date = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Status
		{	
			get{ return _Status; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Status, value, _Status);
				if (PropertyChanging(args))
				{
					_Status = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> BillAmount
		{	
			get{ return _BillAmount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BillAmount, value, _BillAmount);
				if (PropertyChanging(args))
				{
					_BillAmount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PaymentMethod
		{	
			get{ return _PaymentMethod; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PaymentMethod, value, _PaymentMethod);
				if (PropertyChanging(args))
				{
					_PaymentMethod = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BillCycle
		{	
			get{ return _BillCycle; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BillCycle, value, _BillCycle);
				if (PropertyChanging(args))
				{
					_BillCycle = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> BillDay
		{	
			get{ return _BillDay; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BillDay, value, _BillDay);
				if (PropertyChanging(args))
				{
					_BillDay = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BillNotes
		{	
			get{ return _BillNotes; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BillNotes, value, _BillNotes);
				if (PropertyChanging(args))
				{
					_BillNotes = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> BillTax
		{	
			get{ return _BillTax; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BillTax, value, _BillTax);
				if (PropertyChanging(args))
				{
					_BillTax = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> BillOutStanding
		{	
			get{ return _BillOutStanding; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BillOutStanding, value, _BillOutStanding);
				if (PropertyChanging(args))
				{
					_BillOutStanding = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> ServiceDate
		{	
			get{ return _ServiceDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ServiceDate, value, _ServiceDate);
				if (PropertyChanging(args))
				{
					_ServiceDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Area
		{	
			get{ return _Area; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Area, value, _Area);
				if (PropertyChanging(args))
				{
					_Area = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String StreetType
		{	
			get{ return _StreetType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_StreetType, value, _StreetType);
				if (PropertyChanging(args))
				{
					_StreetType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Appartment
		{	
			get{ return _Appartment; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Appartment, value, _Appartment);
				if (PropertyChanging(args))
				{
					_Appartment = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Latlng
		{	
			get{ return _Latlng; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Latlng, value, _Latlng);
				if (PropertyChanging(args))
				{
					_Latlng = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SecondCustomerNo
		{	
			get{ return _SecondCustomerNo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SecondCustomerNo, value, _SecondCustomerNo);
				if (PropertyChanging(args))
				{
					_SecondCustomerNo = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AdditionalCustomerNo
		{	
			get{ return _AdditionalCustomerNo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AdditionalCustomerNo, value, _AdditionalCustomerNo);
				if (PropertyChanging(args))
				{
					_AdditionalCustomerNo = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsTechCallPassed
		{	
			get{ return _IsTechCallPassed; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsTechCallPassed, value, _IsTechCallPassed);
				if (PropertyChanging(args))
				{
					_IsTechCallPassed = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsDirect
		{	
			get{ return _IsDirect; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsDirect, value, _IsDirect);
				if (PropertyChanging(args))
				{
					_IsDirect = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AuthorizeRefId
		{	
			get{ return _AuthorizeRefId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AuthorizeRefId, value, _AuthorizeRefId);
				if (PropertyChanging(args))
				{
					_AuthorizeRefId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AuthorizeCusProfileId
		{	
			get{ return _AuthorizeCusProfileId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AuthorizeCusProfileId, value, _AuthorizeCusProfileId);
				if (PropertyChanging(args))
				{
					_AuthorizeCusProfileId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AuthorizeCusPaymentProfileId
		{	
			get{ return _AuthorizeCusPaymentProfileId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AuthorizeCusPaymentProfileId, value, _AuthorizeCusPaymentProfileId);
				if (PropertyChanging(args))
				{
					_AuthorizeCusPaymentProfileId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AuthorizeDescription
		{	
			get{ return _AuthorizeDescription; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AuthorizeDescription, value, _AuthorizeDescription);
				if (PropertyChanging(args))
				{
					_AuthorizeDescription = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsRequiredCsvSync
		{	
			get{ return _IsRequiredCsvSync; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsRequiredCsvSync, value, _IsRequiredCsvSync);
				if (PropertyChanging(args))
				{
					_IsRequiredCsvSync = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Passcode
		{	
			get{ return _Passcode; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Passcode, value, _Passcode);
				if (PropertyChanging(args))
				{
					_Passcode = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> ActivationFee
		{	
			get{ return _ActivationFee; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ActivationFee, value, _ActivationFee);
				if (PropertyChanging(args))
				{
					_ActivationFee = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> FirstBilling
		{	
			get{ return _FirstBilling; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FirstBilling, value, _FirstBilling);
				if (PropertyChanging(args))
				{
					_FirstBilling = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ActivationFeePaymentMethod
		{	
			get{ return _ActivationFeePaymentMethod; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ActivationFeePaymentMethod, value, _ActivationFeePaymentMethod);
				if (PropertyChanging(args))
				{
					_ActivationFeePaymentMethod = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsActive
		{	
			get{ return _IsActive; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsActive, value, _IsActive);
				if (PropertyChanging(args))
				{
					_IsActive = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> LastGeneratedInvoice
		{	
			get{ return _LastGeneratedInvoice; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastGeneratedInvoice, value, _LastGeneratedInvoice);
				if (PropertyChanging(args))
				{
					_LastGeneratedInvoice = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Singature
		{	
			get{ return _Singature; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Singature, value, _Singature);
				if (PropertyChanging(args))
				{
					_Singature = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CrossStreet
		{	
			get{ return _CrossStreet; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CrossStreet, value, _CrossStreet);
				if (PropertyChanging(args))
				{
					_CrossStreet = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String DBA
		{	
			get{ return _DBA; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DBA, value, _DBA);
				if (PropertyChanging(args))
				{
					_DBA = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AlarmRefId
		{	
			get{ return _AlarmRefId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AlarmRefId, value, _AlarmRefId);
				if (PropertyChanging(args))
				{
					_AlarmRefId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TransunionRefId
		{	
			get{ return _TransunionRefId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TransunionRefId, value, _TransunionRefId);
				if (PropertyChanging(args))
				{
					_TransunionRefId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String MonitronicsRefId
		{	
			get{ return _MonitronicsRefId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MonitronicsRefId, value, _MonitronicsRefId);
				if (PropertyChanging(args))
				{
					_MonitronicsRefId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CentralStationRefId
		{	
			get{ return _CentralStationRefId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CentralStationRefId, value, _CentralStationRefId);
				if (PropertyChanging(args))
				{
					_CentralStationRefId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CmsRefId
		{	
			get{ return _CmsRefId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CmsRefId, value, _CmsRefId);
				if (PropertyChanging(args))
				{
					_CmsRefId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> PreferedEmail
		{	
			get{ return _PreferedEmail; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PreferedEmail, value, _PreferedEmail);
				if (PropertyChanging(args))
				{
					_PreferedEmail = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> PreferedSms
		{	
			get{ return _PreferedSms; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PreferedSms, value, _PreferedSms);
				if (PropertyChanging(args))
				{
					_PreferedSms = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsAgreement
		{	
			get{ return _IsAgreement; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsAgreement, value, _IsAgreement);
				if (PropertyChanging(args))
				{
					_IsAgreement = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsFireAccount
		{	
			get{ return _IsFireAccount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsFireAccount, value, _IsFireAccount);
				if (PropertyChanging(args))
				{
					_IsFireAccount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid CreatedByUid
		{	
			get{ return _CreatedByUid; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreatedByUid, value, _CreatedByUid);
				if (PropertyChanging(args))
				{
					_CreatedByUid = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime CreatedDate
		{	
			get{ return _CreatedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreatedDate, value, _CreatedDate);
				if (PropertyChanging(args))
				{
					_CreatedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String LastUpdatedBy
		{	
			get{ return _LastUpdatedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastUpdatedBy, value, _LastUpdatedBy);
				if (PropertyChanging(args))
				{
					_LastUpdatedBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid LastUpdatedByUid
		{	
			get{ return _LastUpdatedByUid; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastUpdatedByUid, value, _LastUpdatedByUid);
				if (PropertyChanging(args))
				{
					_LastUpdatedByUid = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime LastUpdatedDate
		{	
			get{ return _LastUpdatedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastUpdatedDate, value, _LastUpdatedDate);
				if (PropertyChanging(args))
				{
					_LastUpdatedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BusinessAccountType
		{	
			get{ return _BusinessAccountType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BusinessAccountType, value, _BusinessAccountType);
				if (PropertyChanging(args))
				{
					_BusinessAccountType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PhoneType
		{	
			get{ return _PhoneType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PhoneType, value, _PhoneType);
				if (PropertyChanging(args))
				{
					_PhoneType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Carrier
		{	
			get{ return _Carrier; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Carrier, value, _Carrier);
				if (PropertyChanging(args))
				{
					_Carrier = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid ReferringCustomer
		{	
			get{ return _ReferringCustomer; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReferringCustomer, value, _ReferringCustomer);
				if (PropertyChanging(args))
				{
					_ReferringCustomer = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String EsistingPanel
		{	
			get{ return _EsistingPanel; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EsistingPanel, value, _EsistingPanel);
				if (PropertyChanging(args))
				{
					_EsistingPanel = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Ownership
		{	
			get{ return _Ownership; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Ownership, value, _Ownership);
				if (PropertyChanging(args))
				{
					_Ownership = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> PurchasePrice
		{	
			get{ return _PurchasePrice; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PurchasePrice, value, _PurchasePrice);
				if (PropertyChanging(args))
				{
					_PurchasePrice = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ContractValue
		{	
			get{ return _ContractValue; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ContractValue, value, _ContractValue);
				if (PropertyChanging(args))
				{
					_ContractValue = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid ChildOf
		{	
			get{ return _ChildOf; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ChildOf, value, _ChildOf);
				if (PropertyChanging(args))
				{
					_ChildOf = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> EmailVerified
		{	
			get{ return _EmailVerified; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EmailVerified, value, _EmailVerified);
				if (PropertyChanging(args))
				{
					_EmailVerified = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> HomeVerified
		{	
			get{ return _HomeVerified; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_HomeVerified, value, _HomeVerified);
				if (PropertyChanging(args))
				{
					_HomeVerified = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String County
		{	
			get{ return _County; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_County, value, _County);
				if (PropertyChanging(args))
				{
					_County = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CustomerToken
		{	
			get{ return _CustomerToken; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CustomerToken, value, _CustomerToken);
				if (PropertyChanging(args))
				{
					_CustomerToken = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PaymentToken
		{	
			get{ return _PaymentToken; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PaymentToken, value, _PaymentToken);
				if (PropertyChanging(args))
				{
					_PaymentToken = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ScheduleToken
		{	
			get{ return _ScheduleToken; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ScheduleToken, value, _ScheduleToken);
				if (PropertyChanging(args))
				{
					_ScheduleToken = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> EstCloseDate
		{	
			get{ return _EstCloseDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EstCloseDate, value, _EstCloseDate);
				if (PropertyChanging(args))
				{
					_EstCloseDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> ProjectWalkDate
		{	
			get{ return _ProjectWalkDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ProjectWalkDate, value, _ProjectWalkDate);
				if (PropertyChanging(args))
				{
					_ProjectWalkDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> BranchId
		{	
			get{ return _BranchId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BranchId, value, _BranchId);
				if (PropertyChanging(args))
				{
					_BranchId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SubscriptionStatus
		{	
			get{ return _SubscriptionStatus; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SubscriptionStatus, value, _SubscriptionStatus);
				if (PropertyChanging(args))
				{
					_SubscriptionStatus = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> AnnualRevenue
		{	
			get{ return _AnnualRevenue; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AnnualRevenue, value, _AnnualRevenue);
				if (PropertyChanging(args))
				{
					_AnnualRevenue = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Website
		{	
			get{ return _Website; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Website, value, _Website);
				if (PropertyChanging(args))
				{
					_Website = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Market
		{	
			get{ return _Market; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Market, value, _Market);
				if (PropertyChanging(args))
				{
					_Market = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> Passengers
		{	
			get{ return _Passengers; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Passengers, value, _Passengers);
				if (PropertyChanging(args))
				{
					_Passengers = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> Budget
		{	
			get{ return _Budget; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Budget, value, _Budget);
				if (PropertyChanging(args))
				{
					_Budget = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> SmartSetUpStep
		{	
			get{ return _SmartSetUpStep; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SmartSetUpStep, value, _SmartSetUpStep);
				if (PropertyChanging(args))
				{
					_SmartSetUpStep = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CustomerAccountType
		{	
			get{ return _CustomerAccountType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CustomerAccountType, value, _CustomerAccountType);
				if (PropertyChanging(args))
				{
					_CustomerAccountType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsPrimaryPhoneVerified
		{	
			get{ return _IsPrimaryPhoneVerified; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsPrimaryPhoneVerified, value, _IsPrimaryPhoneVerified);
				if (PropertyChanging(args))
				{
					_IsPrimaryPhoneVerified = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsSecondaryPhoneVerified
		{	
			get{ return _IsSecondaryPhoneVerified; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsSecondaryPhoneVerified, value, _IsSecondaryPhoneVerified);
				if (PropertyChanging(args))
				{
					_IsSecondaryPhoneVerified = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsCellNoVerified
		{	
			get{ return _IsCellNoVerified; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsCellNoVerified, value, _IsCellNoVerified);
				if (PropertyChanging(args))
				{
					_IsCellNoVerified = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String HomeOwner
		{	
			get{ return _HomeOwner; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_HomeOwner, value, _HomeOwner);
				if (PropertyChanging(args))
				{
					_HomeOwner = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid AccessGivenTo
		{	
			get{ return _AccessGivenTo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AccessGivenTo, value, _AccessGivenTo);
				if (PropertyChanging(args))
				{
					_AccessGivenTo = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> DoNotCall
		{	
			get{ return _DoNotCall; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DoNotCall, value, _DoNotCall);
				if (PropertyChanging(args))
				{
					_DoNotCall = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PreferredContactMethod
		{	
			get{ return _PreferredContactMethod; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PreferredContactMethod, value, _PreferredContactMethod);
				if (PropertyChanging(args))
				{
					_PreferredContactMethod = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SalesLocation
		{	
			get{ return _SalesLocation; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SalesLocation, value, _SalesLocation);
				if (PropertyChanging(args))
				{
					_SalesLocation = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsReceivePaymentMail
		{	
			get{ return _IsReceivePaymentMail; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsReceivePaymentMail, value, _IsReceivePaymentMail);
				if (PropertyChanging(args))
				{
					_IsReceivePaymentMail = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BestTimeToCall
		{	
			get{ return _BestTimeToCall; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BestTimeToCall, value, _BestTimeToCall);
				if (PropertyChanging(args))
				{
					_BestTimeToCall = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CSProvider
		{	
			get{ return _CSProvider; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CSProvider, value, _CSProvider);
				if (PropertyChanging(args))
				{
					_CSProvider = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> RenewalTerm
		{	
			get{ return _RenewalTerm; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RenewalTerm, value, _RenewalTerm);
				if (PropertyChanging(args))
				{
					_RenewalTerm = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> PreferedCall
		{	
			get{ return _PreferedCall; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PreferedCall, value, _PreferedCall);
				if (PropertyChanging(args))
				{
					_PreferedCall = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsAgreementSend
		{	
			get{ return _IsAgreementSend; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsAgreementSend, value, _IsAgreementSend);
				if (PropertyChanging(args))
				{
					_IsAgreementSend = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsACHDiscount
		{	
			get{ return _IsACHDiscount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsACHDiscount, value, _IsACHDiscount);
				if (PropertyChanging(args))
				{
					_IsACHDiscount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CustomerStatus
		{	
			get{ return _CustomerStatus; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CustomerStatus, value, _CustomerStatus);
				if (PropertyChanging(args))
				{
					_CustomerStatus = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> TransferCustomerId
		{	
			get{ return _TransferCustomerId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TransferCustomerId, value, _TransferCustomerId);
				if (PropertyChanging(args))
				{
					_TransferCustomerId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CancellationSignature
		{	
			get{ return _CancellationSignature; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CancellationSignature, value, _CancellationSignature);
				if (PropertyChanging(args))
				{
					_CancellationSignature = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> OriginalContactDate
		{	
			get{ return _OriginalContactDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OriginalContactDate, value, _OriginalContactDate);
				if (PropertyChanging(args))
				{
					_OriginalContactDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid DuplicateCustomer
		{	
			get{ return _DuplicateCustomer; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DuplicateCustomer, value, _DuplicateCustomer);
				if (PropertyChanging(args))
				{
					_DuplicateCustomer = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String InspectionCompany
		{	
			get{ return _InspectionCompany; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InspectionCompany, value, _InspectionCompany);
				if (PropertyChanging(args))
				{
					_InspectionCompany = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid SoldBy2
		{	
			get{ return _SoldBy2; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SoldBy2, value, _SoldBy2);
				if (PropertyChanging(args))
				{
					_SoldBy2 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid SoldBy3
		{	
			get{ return _SoldBy3; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SoldBy3, value, _SoldBy3);
				if (PropertyChanging(args))
				{
					_SoldBy3 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ContactedPerviously
		{	
			get{ return _ContactedPerviously; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ContactedPerviously, value, _ContactedPerviously);
				if (PropertyChanging(args))
				{
					_ContactedPerviously = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> MovingDate
		{	
			get{ return _MovingDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MovingDate, value, _MovingDate);
				if (PropertyChanging(args))
				{
					_MovingDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String InstalledStatus
		{	
			get{ return _InstalledStatus; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InstalledStatus, value, _InstalledStatus);
				if (PropertyChanging(args))
				{
					_InstalledStatus = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AcquiredFrom
		{	
			get{ return _AcquiredFrom; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AcquiredFrom, value, _AcquiredFrom);
				if (PropertyChanging(args))
				{
					_AcquiredFrom = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> FollowUpDate
		{	
			get{ return _FollowUpDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FollowUpDate, value, _FollowUpDate);
				if (PropertyChanging(args))
				{
					_FollowUpDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> BuyoutAmountByADS
		{	
			get{ return _BuyoutAmountByADS; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BuyoutAmountByADS, value, _BuyoutAmountByADS);
				if (PropertyChanging(args))
				{
					_BuyoutAmountByADS = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> BuyoutAmountBySalesRep
		{	
			get{ return _BuyoutAmountBySalesRep; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BuyoutAmountBySalesRep, value, _BuyoutAmountBySalesRep);
				if (PropertyChanging(args))
				{
					_BuyoutAmountBySalesRep = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> FinancedTerm
		{	
			get{ return _FinancedTerm; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FinancedTerm, value, _FinancedTerm);
				if (PropertyChanging(args))
				{
					_FinancedTerm = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> FinancedAmount
		{	
			get{ return _FinancedAmount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FinancedAmount, value, _FinancedAmount);
				if (PropertyChanging(args))
				{
					_FinancedAmount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> Levels
		{	
			get{ return _Levels; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Levels, value, _Levels);
				if (PropertyChanging(args))
				{
					_Levels = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> SoldAmount
		{	
			get{ return _SoldAmount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SoldAmount, value, _SoldAmount);
				if (PropertyChanging(args))
				{
					_SoldAmount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AgreementEmail
		{	
			get{ return _AgreementEmail; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AgreementEmail, value, _AgreementEmail);
				if (PropertyChanging(args))
				{
					_AgreementEmail = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AgreementPhoneNo
		{	
			get{ return _AgreementPhoneNo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AgreementPhoneNo, value, _AgreementPhoneNo);
				if (PropertyChanging(args))
				{
					_AgreementPhoneNo = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TaxExemption
		{	
			get{ return _TaxExemption; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TaxExemption, value, _TaxExemption);
				if (PropertyChanging(args))
				{
					_TaxExemption = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AppoinmentSet
		{	
			get{ return _AppoinmentSet; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AppoinmentSet, value, _AppoinmentSet);
				if (PropertyChanging(args))
				{
					_AppoinmentSet = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BillingEmail
		{	
			get{ return _BillingEmail; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BillingEmail, value, _BillingEmail);
				if (PropertyChanging(args))
				{
					_BillingEmail = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BillingPhone
		{	
			get{ return _BillingPhone; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BillingPhone, value, _BillingPhone);
				if (PropertyChanging(args))
				{
					_BillingPhone = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BillingContact
		{	
			get{ return _BillingContact; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BillingContact, value, _BillingContact);
				if (PropertyChanging(args))
				{
					_BillingContact = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> LastOpenedDate
		{	
			get{ return _LastOpenedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastOpenedDate, value, _LastOpenedDate);
				if (PropertyChanging(args))
				{
					_LastOpenedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> DisconnectServiceDate
		{	
			get{ return _DisconnectServiceDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DisconnectServiceDate, value, _DisconnectServiceDate);
				if (PropertyChanging(args))
				{
					_DisconnectServiceDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String UCCRefId
		{	
			get{ return _UCCRefId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UCCRefId, value, _UCCRefId);
				if (PropertyChanging(args))
				{
					_UCCRefId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PlatformId
		{	
			get{ return _PlatformId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PlatformId, value, _PlatformId);
				if (PropertyChanging(args))
				{
					_PlatformId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> RecommendedLevel
		{	
			get{ return _RecommendedLevel; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RecommendedLevel, value, _RecommendedLevel);
				if (PropertyChanging(args))
				{
					_RecommendedLevel = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ProfileImage
		{	
			get{ return _ProfileImage; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ProfileImage, value, _ProfileImage);
				if (PropertyChanging(args))
				{
					_ProfileImage = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BrinksRefId
		{	
			get{ return _BrinksRefId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BrinksRefId, value, _BrinksRefId);
				if (PropertyChanging(args))
				{
					_BrinksRefId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String MapscoNo
		{	
			get{ return _MapscoNo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MapscoNo, value, _MapscoNo);
				if (PropertyChanging(args))
				{
					_MapscoNo = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> CustomerSignatureDate
		{	
			get{ return _CustomerSignatureDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CustomerSignatureDate, value, _CustomerSignatureDate);
				if (PropertyChanging(args))
				{
					_CustomerSignatureDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String LeadSourceType
		{	
			get{ return _LeadSourceType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LeadSourceType, value, _LeadSourceType);
				if (PropertyChanging(args))
				{
					_LeadSourceType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid MoveCustomerId
		{	
			get{ return _MoveCustomerId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MoveCustomerId, value, _MoveCustomerId);
				if (PropertyChanging(args))
				{
					_MoveCustomerId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String EcontractId
		{	
			get{ return _EcontractId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EcontractId, value, _EcontractId);
				if (PropertyChanging(args))
				{
					_EcontractId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String EcontractEnvlobeId
		{	
			get{ return _EcontractEnvlobeId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EcontractEnvlobeId, value, _EcontractEnvlobeId);
				if (PropertyChanging(args))
				{
					_EcontractEnvlobeId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid Soldby1
		{	
			get{ return _Soldby1; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Soldby1, value, _Soldby1);
				if (PropertyChanging(args))
				{
					_Soldby1 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SearchText
		{	
			get{ return _SearchText; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SearchText, value, _SearchText);
				if (PropertyChanging(args))
				{
					_SearchText = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsContractSigned
		{	
			get{ return _IsContractSigned; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsContractSigned, value, _IsContractSigned);
				if (PropertyChanging(args))
				{
					_IsContractSigned = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> CustomerFundedDate
		{	
			get{ return _CustomerFundedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CustomerFundedDate, value, _CustomerFundedDate);
				if (PropertyChanging(args))
				{
					_CustomerFundedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  CustomerBase Clone()
		{
			CustomerBase newObj = new  CustomerBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CustomerId = this.CustomerId;						
			newObj.CustomerNo = this.CustomerNo;						
			newObj.Title = this.Title;						
			newObj.FirstName = this.FirstName;						
			newObj.LastName = this.LastName;						
			newObj.SSN = this.SSN;						
			newObj.Type = this.Type;						
			newObj.BusinessName = this.BusinessName;						
			newObj.DateofBirth = this.DateofBirth;						
			newObj.PrimaryPhone = this.PrimaryPhone;						
			newObj.SecondaryPhone = this.SecondaryPhone;						
			newObj.CellNo = this.CellNo;						
			newObj.Fax = this.Fax;						
			newObj.EmailAddress = this.EmailAddress;						
			newObj.CallingTime = this.CallingTime;						
			newObj.Address = this.Address;						
			newObj.Address2 = this.Address2;						
			newObj.Street = this.Street;						
			newObj.City = this.City;						
			newObj.State = this.State;						
			newObj.ZipCode = this.ZipCode;						
			newObj.Country = this.Country;						
			newObj.StreetPrevious = this.StreetPrevious;						
			newObj.CityPrevious = this.CityPrevious;						
			newObj.StatePrevious = this.StatePrevious;						
			newObj.ZipCodePrevious = this.ZipCodePrevious;						
			newObj.CountryPrevious = this.CountryPrevious;						
			newObj.AccountNo = this.AccountNo;						
			newObj.IsAlarmCom = this.IsAlarmCom;						
			newObj.CreditScore = this.CreditScore;						
			newObj.CreditScoreValue = this.CreditScoreValue;						
			newObj.ContractTeam = this.ContractTeam;						
			newObj.FundingCompany = this.FundingCompany;						
			newObj.MonthlyMonitoringFee = this.MonthlyMonitoringFee;						
			newObj.CellularBackup = this.CellularBackup;						
			newObj.LeadSource = this.LeadSource;						
			newObj.CustomerFunded = this.CustomerFunded;						
			newObj.Maintenance = this.Maintenance;						
			newObj.Note = this.Note;						
			newObj.SalesDate = this.SalesDate;						
			newObj.InstallDate = this.InstallDate;						
			newObj.CutInDate = this.CutInDate;						
			newObj.Installer = this.Installer;						
			newObj.Soldby = this.Soldby;						
			newObj.FundingDate = this.FundingDate;						
			newObj.MiddleName = this.MiddleName;						
			newObj.JoinDate = this.JoinDate;						
			newObj.ReminderDate = this.ReminderDate;						
			newObj.QA1 = this.QA1;						
			newObj.QA1Date = this.QA1Date;						
			newObj.QA2 = this.QA2;						
			newObj.QA2Date = this.QA2Date;						
			newObj.Status = this.Status;						
			newObj.BillAmount = this.BillAmount;						
			newObj.PaymentMethod = this.PaymentMethod;						
			newObj.BillCycle = this.BillCycle;						
			newObj.BillDay = this.BillDay;						
			newObj.BillNotes = this.BillNotes;						
			newObj.BillTax = this.BillTax;						
			newObj.BillOutStanding = this.BillOutStanding;						
			newObj.ServiceDate = this.ServiceDate;						
			newObj.Area = this.Area;						
			newObj.StreetType = this.StreetType;						
			newObj.Appartment = this.Appartment;						
			newObj.Latlng = this.Latlng;						
			newObj.SecondCustomerNo = this.SecondCustomerNo;						
			newObj.AdditionalCustomerNo = this.AdditionalCustomerNo;						
			newObj.IsTechCallPassed = this.IsTechCallPassed;						
			newObj.IsDirect = this.IsDirect;						
			newObj.AuthorizeRefId = this.AuthorizeRefId;						
			newObj.AuthorizeCusProfileId = this.AuthorizeCusProfileId;						
			newObj.AuthorizeCusPaymentProfileId = this.AuthorizeCusPaymentProfileId;						
			newObj.AuthorizeDescription = this.AuthorizeDescription;						
			newObj.IsRequiredCsvSync = this.IsRequiredCsvSync;						
			newObj.Passcode = this.Passcode;						
			newObj.ActivationFee = this.ActivationFee;						
			newObj.FirstBilling = this.FirstBilling;						
			newObj.ActivationFeePaymentMethod = this.ActivationFeePaymentMethod;						
			newObj.IsActive = this.IsActive;						
			newObj.LastGeneratedInvoice = this.LastGeneratedInvoice;						
			newObj.Singature = this.Singature;						
			newObj.CrossStreet = this.CrossStreet;						
			newObj.DBA = this.DBA;						
			newObj.AlarmRefId = this.AlarmRefId;						
			newObj.TransunionRefId = this.TransunionRefId;						
			newObj.MonitronicsRefId = this.MonitronicsRefId;						
			newObj.CentralStationRefId = this.CentralStationRefId;						
			newObj.CmsRefId = this.CmsRefId;						
			newObj.PreferedEmail = this.PreferedEmail;						
			newObj.PreferedSms = this.PreferedSms;						
			newObj.IsAgreement = this.IsAgreement;						
			newObj.IsFireAccount = this.IsFireAccount;						
			newObj.CreatedByUid = this.CreatedByUid;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.LastUpdatedByUid = this.LastUpdatedByUid;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.BusinessAccountType = this.BusinessAccountType;						
			newObj.PhoneType = this.PhoneType;						
			newObj.Carrier = this.Carrier;						
			newObj.ReferringCustomer = this.ReferringCustomer;						
			newObj.EsistingPanel = this.EsistingPanel;						
			newObj.Ownership = this.Ownership;						
			newObj.PurchasePrice = this.PurchasePrice;						
			newObj.ContractValue = this.ContractValue;						
			newObj.ChildOf = this.ChildOf;						
			newObj.EmailVerified = this.EmailVerified;						
			newObj.HomeVerified = this.HomeVerified;						
			newObj.County = this.County;						
			newObj.CustomerToken = this.CustomerToken;						
			newObj.PaymentToken = this.PaymentToken;						
			newObj.ScheduleToken = this.ScheduleToken;						
			newObj.EstCloseDate = this.EstCloseDate;						
			newObj.ProjectWalkDate = this.ProjectWalkDate;						
			newObj.BranchId = this.BranchId;						
			newObj.SubscriptionStatus = this.SubscriptionStatus;						
			newObj.AnnualRevenue = this.AnnualRevenue;						
			newObj.Website = this.Website;						
			newObj.Market = this.Market;						
			newObj.Passengers = this.Passengers;						
			newObj.Budget = this.Budget;						
			newObj.SmartSetUpStep = this.SmartSetUpStep;						
			newObj.CustomerAccountType = this.CustomerAccountType;						
			newObj.IsPrimaryPhoneVerified = this.IsPrimaryPhoneVerified;						
			newObj.IsSecondaryPhoneVerified = this.IsSecondaryPhoneVerified;						
			newObj.IsCellNoVerified = this.IsCellNoVerified;						
			newObj.HomeOwner = this.HomeOwner;						
			newObj.AccessGivenTo = this.AccessGivenTo;						
			newObj.DoNotCall = this.DoNotCall;						
			newObj.PreferredContactMethod = this.PreferredContactMethod;						
			newObj.SalesLocation = this.SalesLocation;						
			newObj.IsReceivePaymentMail = this.IsReceivePaymentMail;						
			newObj.BestTimeToCall = this.BestTimeToCall;						
			newObj.CSProvider = this.CSProvider;						
			newObj.RenewalTerm = this.RenewalTerm;						
			newObj.PreferedCall = this.PreferedCall;						
			newObj.IsAgreementSend = this.IsAgreementSend;						
			newObj.IsACHDiscount = this.IsACHDiscount;						
			newObj.CustomerStatus = this.CustomerStatus;						
			newObj.TransferCustomerId = this.TransferCustomerId;						
			newObj.CancellationSignature = this.CancellationSignature;						
			newObj.OriginalContactDate = this.OriginalContactDate;						
			newObj.DuplicateCustomer = this.DuplicateCustomer;						
			newObj.InspectionCompany = this.InspectionCompany;						
			newObj.SoldBy2 = this.SoldBy2;						
			newObj.SoldBy3 = this.SoldBy3;						
			newObj.ContactedPerviously = this.ContactedPerviously;						
			newObj.MovingDate = this.MovingDate;						
			newObj.InstalledStatus = this.InstalledStatus;						
			newObj.AcquiredFrom = this.AcquiredFrom;						
			newObj.FollowUpDate = this.FollowUpDate;						
			newObj.BuyoutAmountByADS = this.BuyoutAmountByADS;						
			newObj.BuyoutAmountBySalesRep = this.BuyoutAmountBySalesRep;						
			newObj.FinancedTerm = this.FinancedTerm;						
			newObj.FinancedAmount = this.FinancedAmount;						
			newObj.Levels = this.Levels;						
			newObj.SoldAmount = this.SoldAmount;						
			newObj.AgreementEmail = this.AgreementEmail;						
			newObj.AgreementPhoneNo = this.AgreementPhoneNo;						
			newObj.TaxExemption = this.TaxExemption;						
			newObj.AppoinmentSet = this.AppoinmentSet;						
			newObj.BillingEmail = this.BillingEmail;						
			newObj.BillingPhone = this.BillingPhone;						
			newObj.BillingContact = this.BillingContact;						
			newObj.LastOpenedDate = this.LastOpenedDate;						
			newObj.DisconnectServiceDate = this.DisconnectServiceDate;						
			newObj.UCCRefId = this.UCCRefId;						
			newObj.PlatformId = this.PlatformId;						
			newObj.RecommendedLevel = this.RecommendedLevel;						
			newObj.ProfileImage = this.ProfileImage;						
			newObj.BrinksRefId = this.BrinksRefId;						
			newObj.MapscoNo = this.MapscoNo;						
			newObj.CustomerSignatureDate = this.CustomerSignatureDate;						
			newObj.LeadSourceType = this.LeadSourceType;						
			newObj.MoveCustomerId = this.MoveCustomerId;						
			newObj.EcontractId = this.EcontractId;						
			newObj.EcontractEnvlobeId = this.EcontractEnvlobeId;						
			newObj.Soldby1 = this.Soldby1;						
			newObj.SearchText = this.SearchText;						
			newObj.IsContractSigned = this.IsContractSigned;						
			newObj.CustomerFundedDate = this.CustomerFundedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomerBase.Property_Id, Id);				
			info.AddValue(CustomerBase.Property_CustomerId, CustomerId);				
			info.AddValue(CustomerBase.Property_CustomerNo, CustomerNo);				
			info.AddValue(CustomerBase.Property_Title, Title);				
			info.AddValue(CustomerBase.Property_FirstName, FirstName);				
			info.AddValue(CustomerBase.Property_LastName, LastName);				
			info.AddValue(CustomerBase.Property_SSN, SSN);				
			info.AddValue(CustomerBase.Property_Type, Type);				
			info.AddValue(CustomerBase.Property_BusinessName, BusinessName);				
			info.AddValue(CustomerBase.Property_DateofBirth, DateofBirth);				
			info.AddValue(CustomerBase.Property_PrimaryPhone, PrimaryPhone);				
			info.AddValue(CustomerBase.Property_SecondaryPhone, SecondaryPhone);				
			info.AddValue(CustomerBase.Property_CellNo, CellNo);				
			info.AddValue(CustomerBase.Property_Fax, Fax);				
			info.AddValue(CustomerBase.Property_EmailAddress, EmailAddress);				
			info.AddValue(CustomerBase.Property_CallingTime, CallingTime);				
			info.AddValue(CustomerBase.Property_Address, Address);				
			info.AddValue(CustomerBase.Property_Address2, Address2);				
			info.AddValue(CustomerBase.Property_Street, Street);				
			info.AddValue(CustomerBase.Property_City, City);				
			info.AddValue(CustomerBase.Property_State, State);				
			info.AddValue(CustomerBase.Property_ZipCode, ZipCode);				
			info.AddValue(CustomerBase.Property_Country, Country);				
			info.AddValue(CustomerBase.Property_StreetPrevious, StreetPrevious);				
			info.AddValue(CustomerBase.Property_CityPrevious, CityPrevious);				
			info.AddValue(CustomerBase.Property_StatePrevious, StatePrevious);				
			info.AddValue(CustomerBase.Property_ZipCodePrevious, ZipCodePrevious);				
			info.AddValue(CustomerBase.Property_CountryPrevious, CountryPrevious);				
			info.AddValue(CustomerBase.Property_AccountNo, AccountNo);				
			info.AddValue(CustomerBase.Property_IsAlarmCom, IsAlarmCom);				
			info.AddValue(CustomerBase.Property_CreditScore, CreditScore);				
			info.AddValue(CustomerBase.Property_CreditScoreValue, CreditScoreValue);				
			info.AddValue(CustomerBase.Property_ContractTeam, ContractTeam);				
			info.AddValue(CustomerBase.Property_FundingCompany, FundingCompany);				
			info.AddValue(CustomerBase.Property_MonthlyMonitoringFee, MonthlyMonitoringFee);				
			info.AddValue(CustomerBase.Property_CellularBackup, CellularBackup);				
			info.AddValue(CustomerBase.Property_LeadSource, LeadSource);				
			info.AddValue(CustomerBase.Property_CustomerFunded, CustomerFunded);				
			info.AddValue(CustomerBase.Property_Maintenance, Maintenance);				
			info.AddValue(CustomerBase.Property_Note, Note);				
			info.AddValue(CustomerBase.Property_SalesDate, SalesDate);				
			info.AddValue(CustomerBase.Property_InstallDate, InstallDate);				
			info.AddValue(CustomerBase.Property_CutInDate, CutInDate);				
			info.AddValue(CustomerBase.Property_Installer, Installer);				
			info.AddValue(CustomerBase.Property_Soldby, Soldby);				
			info.AddValue(CustomerBase.Property_FundingDate, FundingDate);				
			info.AddValue(CustomerBase.Property_MiddleName, MiddleName);				
			info.AddValue(CustomerBase.Property_JoinDate, JoinDate);				
			info.AddValue(CustomerBase.Property_ReminderDate, ReminderDate);				
			info.AddValue(CustomerBase.Property_QA1, QA1);				
			info.AddValue(CustomerBase.Property_QA1Date, QA1Date);				
			info.AddValue(CustomerBase.Property_QA2, QA2);				
			info.AddValue(CustomerBase.Property_QA2Date, QA2Date);				
			info.AddValue(CustomerBase.Property_Status, Status);				
			info.AddValue(CustomerBase.Property_BillAmount, BillAmount);				
			info.AddValue(CustomerBase.Property_PaymentMethod, PaymentMethod);				
			info.AddValue(CustomerBase.Property_BillCycle, BillCycle);				
			info.AddValue(CustomerBase.Property_BillDay, BillDay);				
			info.AddValue(CustomerBase.Property_BillNotes, BillNotes);				
			info.AddValue(CustomerBase.Property_BillTax, BillTax);				
			info.AddValue(CustomerBase.Property_BillOutStanding, BillOutStanding);				
			info.AddValue(CustomerBase.Property_ServiceDate, ServiceDate);				
			info.AddValue(CustomerBase.Property_Area, Area);				
			info.AddValue(CustomerBase.Property_StreetType, StreetType);				
			info.AddValue(CustomerBase.Property_Appartment, Appartment);				
			info.AddValue(CustomerBase.Property_Latlng, Latlng);				
			info.AddValue(CustomerBase.Property_SecondCustomerNo, SecondCustomerNo);				
			info.AddValue(CustomerBase.Property_AdditionalCustomerNo, AdditionalCustomerNo);				
			info.AddValue(CustomerBase.Property_IsTechCallPassed, IsTechCallPassed);				
			info.AddValue(CustomerBase.Property_IsDirect, IsDirect);				
			info.AddValue(CustomerBase.Property_AuthorizeRefId, AuthorizeRefId);				
			info.AddValue(CustomerBase.Property_AuthorizeCusProfileId, AuthorizeCusProfileId);				
			info.AddValue(CustomerBase.Property_AuthorizeCusPaymentProfileId, AuthorizeCusPaymentProfileId);				
			info.AddValue(CustomerBase.Property_AuthorizeDescription, AuthorizeDescription);				
			info.AddValue(CustomerBase.Property_IsRequiredCsvSync, IsRequiredCsvSync);				
			info.AddValue(CustomerBase.Property_Passcode, Passcode);				
			info.AddValue(CustomerBase.Property_ActivationFee, ActivationFee);				
			info.AddValue(CustomerBase.Property_FirstBilling, FirstBilling);				
			info.AddValue(CustomerBase.Property_ActivationFeePaymentMethod, ActivationFeePaymentMethod);				
			info.AddValue(CustomerBase.Property_IsActive, IsActive);				
			info.AddValue(CustomerBase.Property_LastGeneratedInvoice, LastGeneratedInvoice);				
			info.AddValue(CustomerBase.Property_Singature, Singature);				
			info.AddValue(CustomerBase.Property_CrossStreet, CrossStreet);				
			info.AddValue(CustomerBase.Property_DBA, DBA);				
			info.AddValue(CustomerBase.Property_AlarmRefId, AlarmRefId);				
			info.AddValue(CustomerBase.Property_TransunionRefId, TransunionRefId);				
			info.AddValue(CustomerBase.Property_MonitronicsRefId, MonitronicsRefId);				
			info.AddValue(CustomerBase.Property_CentralStationRefId, CentralStationRefId);				
			info.AddValue(CustomerBase.Property_CmsRefId, CmsRefId);				
			info.AddValue(CustomerBase.Property_PreferedEmail, PreferedEmail);				
			info.AddValue(CustomerBase.Property_PreferedSms, PreferedSms);				
			info.AddValue(CustomerBase.Property_IsAgreement, IsAgreement);				
			info.AddValue(CustomerBase.Property_IsFireAccount, IsFireAccount);				
			info.AddValue(CustomerBase.Property_CreatedByUid, CreatedByUid);				
			info.AddValue(CustomerBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(CustomerBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(CustomerBase.Property_LastUpdatedByUid, LastUpdatedByUid);				
			info.AddValue(CustomerBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(CustomerBase.Property_BusinessAccountType, BusinessAccountType);				
			info.AddValue(CustomerBase.Property_PhoneType, PhoneType);				
			info.AddValue(CustomerBase.Property_Carrier, Carrier);				
			info.AddValue(CustomerBase.Property_ReferringCustomer, ReferringCustomer);				
			info.AddValue(CustomerBase.Property_EsistingPanel, EsistingPanel);				
			info.AddValue(CustomerBase.Property_Ownership, Ownership);				
			info.AddValue(CustomerBase.Property_PurchasePrice, PurchasePrice);				
			info.AddValue(CustomerBase.Property_ContractValue, ContractValue);				
			info.AddValue(CustomerBase.Property_ChildOf, ChildOf);				
			info.AddValue(CustomerBase.Property_EmailVerified, EmailVerified);				
			info.AddValue(CustomerBase.Property_HomeVerified, HomeVerified);				
			info.AddValue(CustomerBase.Property_County, County);				
			info.AddValue(CustomerBase.Property_CustomerToken, CustomerToken);				
			info.AddValue(CustomerBase.Property_PaymentToken, PaymentToken);				
			info.AddValue(CustomerBase.Property_ScheduleToken, ScheduleToken);				
			info.AddValue(CustomerBase.Property_EstCloseDate, EstCloseDate);				
			info.AddValue(CustomerBase.Property_ProjectWalkDate, ProjectWalkDate);				
			info.AddValue(CustomerBase.Property_BranchId, BranchId);				
			info.AddValue(CustomerBase.Property_SubscriptionStatus, SubscriptionStatus);				
			info.AddValue(CustomerBase.Property_AnnualRevenue, AnnualRevenue);				
			info.AddValue(CustomerBase.Property_Website, Website);				
			info.AddValue(CustomerBase.Property_Market, Market);				
			info.AddValue(CustomerBase.Property_Passengers, Passengers);				
			info.AddValue(CustomerBase.Property_Budget, Budget);				
			info.AddValue(CustomerBase.Property_SmartSetUpStep, SmartSetUpStep);				
			info.AddValue(CustomerBase.Property_CustomerAccountType, CustomerAccountType);				
			info.AddValue(CustomerBase.Property_IsPrimaryPhoneVerified, IsPrimaryPhoneVerified);				
			info.AddValue(CustomerBase.Property_IsSecondaryPhoneVerified, IsSecondaryPhoneVerified);				
			info.AddValue(CustomerBase.Property_IsCellNoVerified, IsCellNoVerified);				
			info.AddValue(CustomerBase.Property_HomeOwner, HomeOwner);				
			info.AddValue(CustomerBase.Property_AccessGivenTo, AccessGivenTo);				
			info.AddValue(CustomerBase.Property_DoNotCall, DoNotCall);				
			info.AddValue(CustomerBase.Property_PreferredContactMethod, PreferredContactMethod);				
			info.AddValue(CustomerBase.Property_SalesLocation, SalesLocation);				
			info.AddValue(CustomerBase.Property_IsReceivePaymentMail, IsReceivePaymentMail);				
			info.AddValue(CustomerBase.Property_BestTimeToCall, BestTimeToCall);				
			info.AddValue(CustomerBase.Property_CSProvider, CSProvider);				
			info.AddValue(CustomerBase.Property_RenewalTerm, RenewalTerm);				
			info.AddValue(CustomerBase.Property_PreferedCall, PreferedCall);				
			info.AddValue(CustomerBase.Property_IsAgreementSend, IsAgreementSend);				
			info.AddValue(CustomerBase.Property_IsACHDiscount, IsACHDiscount);				
			info.AddValue(CustomerBase.Property_CustomerStatus, CustomerStatus);				
			info.AddValue(CustomerBase.Property_TransferCustomerId, TransferCustomerId);				
			info.AddValue(CustomerBase.Property_CancellationSignature, CancellationSignature);				
			info.AddValue(CustomerBase.Property_OriginalContactDate, OriginalContactDate);				
			info.AddValue(CustomerBase.Property_DuplicateCustomer, DuplicateCustomer);				
			info.AddValue(CustomerBase.Property_InspectionCompany, InspectionCompany);				
			info.AddValue(CustomerBase.Property_SoldBy2, SoldBy2);				
			info.AddValue(CustomerBase.Property_SoldBy3, SoldBy3);				
			info.AddValue(CustomerBase.Property_ContactedPerviously, ContactedPerviously);				
			info.AddValue(CustomerBase.Property_MovingDate, MovingDate);				
			info.AddValue(CustomerBase.Property_InstalledStatus, InstalledStatus);				
			info.AddValue(CustomerBase.Property_AcquiredFrom, AcquiredFrom);				
			info.AddValue(CustomerBase.Property_FollowUpDate, FollowUpDate);				
			info.AddValue(CustomerBase.Property_BuyoutAmountByADS, BuyoutAmountByADS);				
			info.AddValue(CustomerBase.Property_BuyoutAmountBySalesRep, BuyoutAmountBySalesRep);				
			info.AddValue(CustomerBase.Property_FinancedTerm, FinancedTerm);				
			info.AddValue(CustomerBase.Property_FinancedAmount, FinancedAmount);				
			info.AddValue(CustomerBase.Property_Levels, Levels);				
			info.AddValue(CustomerBase.Property_SoldAmount, SoldAmount);				
			info.AddValue(CustomerBase.Property_AgreementEmail, AgreementEmail);				
			info.AddValue(CustomerBase.Property_AgreementPhoneNo, AgreementPhoneNo);				
			info.AddValue(CustomerBase.Property_TaxExemption, TaxExemption);				
			info.AddValue(CustomerBase.Property_AppoinmentSet, AppoinmentSet);				
			info.AddValue(CustomerBase.Property_BillingEmail, BillingEmail);				
			info.AddValue(CustomerBase.Property_BillingPhone, BillingPhone);				
			info.AddValue(CustomerBase.Property_BillingContact, BillingContact);				
			info.AddValue(CustomerBase.Property_LastOpenedDate, LastOpenedDate);				
			info.AddValue(CustomerBase.Property_DisconnectServiceDate, DisconnectServiceDate);				
			info.AddValue(CustomerBase.Property_UCCRefId, UCCRefId);				
			info.AddValue(CustomerBase.Property_PlatformId, PlatformId);				
			info.AddValue(CustomerBase.Property_RecommendedLevel, RecommendedLevel);				
			info.AddValue(CustomerBase.Property_ProfileImage, ProfileImage);				
			info.AddValue(CustomerBase.Property_BrinksRefId, BrinksRefId);				
			info.AddValue(CustomerBase.Property_MapscoNo, MapscoNo);				
			info.AddValue(CustomerBase.Property_CustomerSignatureDate, CustomerSignatureDate);				
			info.AddValue(CustomerBase.Property_LeadSourceType, LeadSourceType);				
			info.AddValue(CustomerBase.Property_MoveCustomerId, MoveCustomerId);				
			info.AddValue(CustomerBase.Property_EcontractId, EcontractId);				
			info.AddValue(CustomerBase.Property_EcontractEnvlobeId, EcontractEnvlobeId);				
			info.AddValue(CustomerBase.Property_Soldby1, Soldby1);				
			info.AddValue(CustomerBase.Property_SearchText, SearchText);				
			info.AddValue(CustomerBase.Property_IsContractSigned, IsContractSigned);				
			info.AddValue(CustomerBase.Property_CustomerFundedDate, CustomerFundedDate);				
		}
		#endregion

		
	}
}
