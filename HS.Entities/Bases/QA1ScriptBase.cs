using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "QA1ScriptBase", Namespace = "http://www.hims-tech.com//entities")]
	public class QA1ScriptBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CustomerId = 1,
			CompanyId = 2,
			FirstName = 3,
			LastName = 4,
			NameIsCorrect = 5,
			NameUpdateNote = 6,
			Street = 7,
			City = 8,
			State = 9,
			ZipCode = 10,
			AddressIsCorrect = 11,
			AddressUpdateNote = 12,
			IsLocationHome = 13,
			IsHomeRent = 14,
			HomeownerName = 15,
			HomeownerNameUpdateNote = 16,
			IsSpeckRep = 17,
			IsHomeownerAuthorized = 18,
			IsOnTheDeed = 19,
			IsResponsibleParty = 20,
			ResponsiblePartyName = 21,
			ReponsiblePartyNameUpdateNote = 22,
			IsSignShowingBusiness = 23,
			SignShowText = 24,
			IsOtherDocumentShowing = 25,
			OtherDocumentShowingText = 26,
			IsBusinessSecretaryOfState = 27,
			PrimaryPhone = 28,
			PrimaryPhoneIsCorrect = 29,
			PrimaryPhoneUpdateNote = 30,
			EmailAddress = 31,
			EmailIsCorrect = 32,
			EmailUpdateNote = 33,
			InstallTicketIsCorrect = 34,
			TermAndFeeIsCorrect = 35,
			IsCurrentlyInContract = 36,
			CurrentlyContractText = 37,
			IsUnderstandAffiliateAndService = 38,
			UnderstandText = 39,
			IsBrinksOrMonitronic = 40,
			BrinksOrMonitronicText = 41,
			BrinksHowLong = 42,
			IsPromisedByRep = 43,
			PromisedByRepText = 44,
			WhatWasPromised = 45,
			IsElectricityAvailable = 46,
			IsCorrectlyWWI = 47,
			IsCorrectlyWWINoText = 48,
			IsCorrectlyWWIYesText = 49,
			TicketScheduleTimeIsGood = 50,
			DateOfBirthIsCorrect = 51,
			DateofBirth = 52,
			DateOfBirthUpdateNote = 53,
			IsGenerallyResponsible = 54,
			Passcode = 55,
			CreatedBy = 56,
			CreatedByUid = 57,
			CreatedDate = 58,
			LastUpdatedByUid = 59,
			LastUpdatedDate = 60,
			IsCompleted = 61,
			IsSignShowingBusinessYes = 62,
			ManualNote = 63,
			QARep = 64,
			SalesRep = 65,
			InstallDate = 66,
			HowQA1Initiated = 67,
			IsCallingToday = 68,
			SSNIsCorrect = 69,
			SSN = 70,
			SSNUpdateNote = 71,
			PermitIsRequired = 72,
			BillingIsCorrect = 73,
			PermissionGoAhead = 74,
			PermissionGoAheadNote = 75,
			SoundIsCorrect = 76,
			SoundCorrectNote = 77,
			IsAssistYou = 78,
			IsPass = 79,
			PassNote = 80
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_FirstName = "FirstName";		            
		public const string Property_LastName = "LastName";		            
		public const string Property_NameIsCorrect = "NameIsCorrect";		            
		public const string Property_NameUpdateNote = "NameUpdateNote";		            
		public const string Property_Street = "Street";		            
		public const string Property_City = "City";		            
		public const string Property_State = "State";		            
		public const string Property_ZipCode = "ZipCode";		            
		public const string Property_AddressIsCorrect = "AddressIsCorrect";		            
		public const string Property_AddressUpdateNote = "AddressUpdateNote";		            
		public const string Property_IsLocationHome = "IsLocationHome";		            
		public const string Property_IsHomeRent = "IsHomeRent";		            
		public const string Property_HomeownerName = "HomeownerName";		            
		public const string Property_HomeownerNameUpdateNote = "HomeownerNameUpdateNote";		            
		public const string Property_IsSpeckRep = "IsSpeckRep";		            
		public const string Property_IsHomeownerAuthorized = "IsHomeownerAuthorized";		            
		public const string Property_IsOnTheDeed = "IsOnTheDeed";		            
		public const string Property_IsResponsibleParty = "IsResponsibleParty";		            
		public const string Property_ResponsiblePartyName = "ResponsiblePartyName";		            
		public const string Property_ReponsiblePartyNameUpdateNote = "ReponsiblePartyNameUpdateNote";		            
		public const string Property_IsSignShowingBusiness = "IsSignShowingBusiness";		            
		public const string Property_SignShowText = "SignShowText";		            
		public const string Property_IsOtherDocumentShowing = "IsOtherDocumentShowing";		            
		public const string Property_OtherDocumentShowingText = "OtherDocumentShowingText";		            
		public const string Property_IsBusinessSecretaryOfState = "IsBusinessSecretaryOfState";		            
		public const string Property_PrimaryPhone = "PrimaryPhone";		            
		public const string Property_PrimaryPhoneIsCorrect = "PrimaryPhoneIsCorrect";		            
		public const string Property_PrimaryPhoneUpdateNote = "PrimaryPhoneUpdateNote";		            
		public const string Property_EmailAddress = "EmailAddress";		            
		public const string Property_EmailIsCorrect = "EmailIsCorrect";		            
		public const string Property_EmailUpdateNote = "EmailUpdateNote";		            
		public const string Property_InstallTicketIsCorrect = "InstallTicketIsCorrect";		            
		public const string Property_TermAndFeeIsCorrect = "TermAndFeeIsCorrect";		            
		public const string Property_IsCurrentlyInContract = "IsCurrentlyInContract";		            
		public const string Property_CurrentlyContractText = "CurrentlyContractText";		            
		public const string Property_IsUnderstandAffiliateAndService = "IsUnderstandAffiliateAndService";		            
		public const string Property_UnderstandText = "UnderstandText";		            
		public const string Property_IsBrinksOrMonitronic = "IsBrinksOrMonitronic";		            
		public const string Property_BrinksOrMonitronicText = "BrinksOrMonitronicText";		            
		public const string Property_BrinksHowLong = "BrinksHowLong";		            
		public const string Property_IsPromisedByRep = "IsPromisedByRep";		            
		public const string Property_PromisedByRepText = "PromisedByRepText";		            
		public const string Property_WhatWasPromised = "WhatWasPromised";		            
		public const string Property_IsElectricityAvailable = "IsElectricityAvailable";		            
		public const string Property_IsCorrectlyWWI = "IsCorrectlyWWI";		            
		public const string Property_IsCorrectlyWWINoText = "IsCorrectlyWWINoText";		            
		public const string Property_IsCorrectlyWWIYesText = "IsCorrectlyWWIYesText";		            
		public const string Property_TicketScheduleTimeIsGood = "TicketScheduleTimeIsGood";		            
		public const string Property_DateOfBirthIsCorrect = "DateOfBirthIsCorrect";		            
		public const string Property_DateofBirth = "DateofBirth";		            
		public const string Property_DateOfBirthUpdateNote = "DateOfBirthUpdateNote";		            
		public const string Property_IsGenerallyResponsible = "IsGenerallyResponsible";		            
		public const string Property_Passcode = "Passcode";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedByUid = "CreatedByUid";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdatedByUid = "LastUpdatedByUid";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_IsCompleted = "IsCompleted";		            
		public const string Property_IsSignShowingBusinessYes = "IsSignShowingBusinessYes";		            
		public const string Property_ManualNote = "ManualNote";		            
		public const string Property_QARep = "QARep";		            
		public const string Property_SalesRep = "SalesRep";		            
		public const string Property_InstallDate = "InstallDate";		            
		public const string Property_HowQA1Initiated = "HowQA1Initiated";		            
		public const string Property_IsCallingToday = "IsCallingToday";		            
		public const string Property_SSNIsCorrect = "SSNIsCorrect";		            
		public const string Property_SSN = "SSN";		            
		public const string Property_SSNUpdateNote = "SSNUpdateNote";		            
		public const string Property_PermitIsRequired = "PermitIsRequired";		            
		public const string Property_BillingIsCorrect = "BillingIsCorrect";		            
		public const string Property_PermissionGoAhead = "PermissionGoAhead";		            
		public const string Property_PermissionGoAheadNote = "PermissionGoAheadNote";		            
		public const string Property_SoundIsCorrect = "SoundIsCorrect";		            
		public const string Property_SoundCorrectNote = "SoundCorrectNote";		            
		public const string Property_IsAssistYou = "IsAssistYou";		            
		public const string Property_IsPass = "IsPass";		            
		public const string Property_PassNote = "PassNote";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CustomerId;	            
		private Guid _CompanyId;	            
		private String _FirstName;	            
		private String _LastName;	            
		private String _NameIsCorrect;	            
		private String _NameUpdateNote;	            
		private String _Street;	            
		private String _City;	            
		private String _State;	            
		private String _ZipCode;	            
		private String _AddressIsCorrect;	            
		private String _AddressUpdateNote;	            
		private String _IsLocationHome;	            
		private String _IsHomeRent;	            
		private String _HomeownerName;	            
		private String _HomeownerNameUpdateNote;	            
		private String _IsSpeckRep;	            
		private String _IsHomeownerAuthorized;	            
		private String _IsOnTheDeed;	            
		private String _IsResponsibleParty;	            
		private String _ResponsiblePartyName;	            
		private String _ReponsiblePartyNameUpdateNote;	            
		private String _IsSignShowingBusiness;	            
		private String _SignShowText;	            
		private String _IsOtherDocumentShowing;	            
		private String _OtherDocumentShowingText;	            
		private String _IsBusinessSecretaryOfState;	            
		private String _PrimaryPhone;	            
		private String _PrimaryPhoneIsCorrect;	            
		private String _PrimaryPhoneUpdateNote;	            
		private String _EmailAddress;	            
		private String _EmailIsCorrect;	            
		private String _EmailUpdateNote;	            
		private String _InstallTicketIsCorrect;	            
		private String _TermAndFeeIsCorrect;	            
		private String _IsCurrentlyInContract;	            
		private String _CurrentlyContractText;	            
		private String _IsUnderstandAffiliateAndService;	            
		private String _UnderstandText;	            
		private String _IsBrinksOrMonitronic;	            
		private String _BrinksOrMonitronicText;	            
		private String _BrinksHowLong;	            
		private String _IsPromisedByRep;	            
		private String _PromisedByRepText;	            
		private String _WhatWasPromised;	            
		private String _IsElectricityAvailable;	            
		private String _IsCorrectlyWWI;	            
		private String _IsCorrectlyWWINoText;	            
		private String _IsCorrectlyWWIYesText;	            
		private String _TicketScheduleTimeIsGood;	            
		private String _DateOfBirthIsCorrect;	            
		private Nullable<DateTime> _DateofBirth;	            
		private String _DateOfBirthUpdateNote;	            
		private String _IsGenerallyResponsible;	            
		private String _Passcode;	            
		private String _CreatedBy;	            
		private Guid _CreatedByUid;	            
		private DateTime _CreatedDate;	            
		private Guid _LastUpdatedByUid;	            
		private DateTime _LastUpdatedDate;	            
		private Boolean _IsCompleted;	            
		private String _IsSignShowingBusinessYes;	            
		private String _ManualNote;	            
		private Guid _QARep;	            
		private Guid _SalesRep;	            
		private DateTime _InstallDate;	            
		private String _HowQA1Initiated;	            
		private String _IsCallingToday;	            
		private String _SSNIsCorrect;	            
		private String _SSN;	            
		private String _SSNUpdateNote;	            
		private String _PermitIsRequired;	            
		private String _BillingIsCorrect;	            
		private String _PermissionGoAhead;	            
		private String _PermissionGoAheadNote;	            
		private String _SoundIsCorrect;	            
		private String _SoundCorrectNote;	            
		private String _IsAssistYou;	            
		private String _IsPass;	            
		private String _PassNote;	            
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
		public Guid CompanyId
		{	
			get{ return _CompanyId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CompanyId, value, _CompanyId);
				if (PropertyChanging(args))
				{
					_CompanyId = value;
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
		public String NameIsCorrect
		{	
			get{ return _NameIsCorrect; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_NameIsCorrect, value, _NameIsCorrect);
				if (PropertyChanging(args))
				{
					_NameIsCorrect = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String NameUpdateNote
		{	
			get{ return _NameUpdateNote; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_NameUpdateNote, value, _NameUpdateNote);
				if (PropertyChanging(args))
				{
					_NameUpdateNote = value;
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
		public String AddressIsCorrect
		{	
			get{ return _AddressIsCorrect; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AddressIsCorrect, value, _AddressIsCorrect);
				if (PropertyChanging(args))
				{
					_AddressIsCorrect = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AddressUpdateNote
		{	
			get{ return _AddressUpdateNote; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AddressUpdateNote, value, _AddressUpdateNote);
				if (PropertyChanging(args))
				{
					_AddressUpdateNote = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IsLocationHome
		{	
			get{ return _IsLocationHome; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsLocationHome, value, _IsLocationHome);
				if (PropertyChanging(args))
				{
					_IsLocationHome = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IsHomeRent
		{	
			get{ return _IsHomeRent; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsHomeRent, value, _IsHomeRent);
				if (PropertyChanging(args))
				{
					_IsHomeRent = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String HomeownerName
		{	
			get{ return _HomeownerName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_HomeownerName, value, _HomeownerName);
				if (PropertyChanging(args))
				{
					_HomeownerName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String HomeownerNameUpdateNote
		{	
			get{ return _HomeownerNameUpdateNote; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_HomeownerNameUpdateNote, value, _HomeownerNameUpdateNote);
				if (PropertyChanging(args))
				{
					_HomeownerNameUpdateNote = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IsSpeckRep
		{	
			get{ return _IsSpeckRep; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsSpeckRep, value, _IsSpeckRep);
				if (PropertyChanging(args))
				{
					_IsSpeckRep = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IsHomeownerAuthorized
		{	
			get{ return _IsHomeownerAuthorized; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsHomeownerAuthorized, value, _IsHomeownerAuthorized);
				if (PropertyChanging(args))
				{
					_IsHomeownerAuthorized = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IsOnTheDeed
		{	
			get{ return _IsOnTheDeed; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsOnTheDeed, value, _IsOnTheDeed);
				if (PropertyChanging(args))
				{
					_IsOnTheDeed = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IsResponsibleParty
		{	
			get{ return _IsResponsibleParty; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsResponsibleParty, value, _IsResponsibleParty);
				if (PropertyChanging(args))
				{
					_IsResponsibleParty = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ResponsiblePartyName
		{	
			get{ return _ResponsiblePartyName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ResponsiblePartyName, value, _ResponsiblePartyName);
				if (PropertyChanging(args))
				{
					_ResponsiblePartyName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ReponsiblePartyNameUpdateNote
		{	
			get{ return _ReponsiblePartyNameUpdateNote; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReponsiblePartyNameUpdateNote, value, _ReponsiblePartyNameUpdateNote);
				if (PropertyChanging(args))
				{
					_ReponsiblePartyNameUpdateNote = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IsSignShowingBusiness
		{	
			get{ return _IsSignShowingBusiness; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsSignShowingBusiness, value, _IsSignShowingBusiness);
				if (PropertyChanging(args))
				{
					_IsSignShowingBusiness = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SignShowText
		{	
			get{ return _SignShowText; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SignShowText, value, _SignShowText);
				if (PropertyChanging(args))
				{
					_SignShowText = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IsOtherDocumentShowing
		{	
			get{ return _IsOtherDocumentShowing; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsOtherDocumentShowing, value, _IsOtherDocumentShowing);
				if (PropertyChanging(args))
				{
					_IsOtherDocumentShowing = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String OtherDocumentShowingText
		{	
			get{ return _OtherDocumentShowingText; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OtherDocumentShowingText, value, _OtherDocumentShowingText);
				if (PropertyChanging(args))
				{
					_OtherDocumentShowingText = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IsBusinessSecretaryOfState
		{	
			get{ return _IsBusinessSecretaryOfState; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsBusinessSecretaryOfState, value, _IsBusinessSecretaryOfState);
				if (PropertyChanging(args))
				{
					_IsBusinessSecretaryOfState = value;
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
		public String PrimaryPhoneIsCorrect
		{	
			get{ return _PrimaryPhoneIsCorrect; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PrimaryPhoneIsCorrect, value, _PrimaryPhoneIsCorrect);
				if (PropertyChanging(args))
				{
					_PrimaryPhoneIsCorrect = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PrimaryPhoneUpdateNote
		{	
			get{ return _PrimaryPhoneUpdateNote; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PrimaryPhoneUpdateNote, value, _PrimaryPhoneUpdateNote);
				if (PropertyChanging(args))
				{
					_PrimaryPhoneUpdateNote = value;
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
		public String EmailIsCorrect
		{	
			get{ return _EmailIsCorrect; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EmailIsCorrect, value, _EmailIsCorrect);
				if (PropertyChanging(args))
				{
					_EmailIsCorrect = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String EmailUpdateNote
		{	
			get{ return _EmailUpdateNote; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EmailUpdateNote, value, _EmailUpdateNote);
				if (PropertyChanging(args))
				{
					_EmailUpdateNote = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String InstallTicketIsCorrect
		{	
			get{ return _InstallTicketIsCorrect; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InstallTicketIsCorrect, value, _InstallTicketIsCorrect);
				if (PropertyChanging(args))
				{
					_InstallTicketIsCorrect = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TermAndFeeIsCorrect
		{	
			get{ return _TermAndFeeIsCorrect; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TermAndFeeIsCorrect, value, _TermAndFeeIsCorrect);
				if (PropertyChanging(args))
				{
					_TermAndFeeIsCorrect = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IsCurrentlyInContract
		{	
			get{ return _IsCurrentlyInContract; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsCurrentlyInContract, value, _IsCurrentlyInContract);
				if (PropertyChanging(args))
				{
					_IsCurrentlyInContract = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CurrentlyContractText
		{	
			get{ return _CurrentlyContractText; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CurrentlyContractText, value, _CurrentlyContractText);
				if (PropertyChanging(args))
				{
					_CurrentlyContractText = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IsUnderstandAffiliateAndService
		{	
			get{ return _IsUnderstandAffiliateAndService; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsUnderstandAffiliateAndService, value, _IsUnderstandAffiliateAndService);
				if (PropertyChanging(args))
				{
					_IsUnderstandAffiliateAndService = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String UnderstandText
		{	
			get{ return _UnderstandText; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UnderstandText, value, _UnderstandText);
				if (PropertyChanging(args))
				{
					_UnderstandText = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IsBrinksOrMonitronic
		{	
			get{ return _IsBrinksOrMonitronic; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsBrinksOrMonitronic, value, _IsBrinksOrMonitronic);
				if (PropertyChanging(args))
				{
					_IsBrinksOrMonitronic = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BrinksOrMonitronicText
		{	
			get{ return _BrinksOrMonitronicText; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BrinksOrMonitronicText, value, _BrinksOrMonitronicText);
				if (PropertyChanging(args))
				{
					_BrinksOrMonitronicText = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BrinksHowLong
		{	
			get{ return _BrinksHowLong; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BrinksHowLong, value, _BrinksHowLong);
				if (PropertyChanging(args))
				{
					_BrinksHowLong = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IsPromisedByRep
		{	
			get{ return _IsPromisedByRep; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsPromisedByRep, value, _IsPromisedByRep);
				if (PropertyChanging(args))
				{
					_IsPromisedByRep = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PromisedByRepText
		{	
			get{ return _PromisedByRepText; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PromisedByRepText, value, _PromisedByRepText);
				if (PropertyChanging(args))
				{
					_PromisedByRepText = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String WhatWasPromised
		{	
			get{ return _WhatWasPromised; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_WhatWasPromised, value, _WhatWasPromised);
				if (PropertyChanging(args))
				{
					_WhatWasPromised = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IsElectricityAvailable
		{	
			get{ return _IsElectricityAvailable; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsElectricityAvailable, value, _IsElectricityAvailable);
				if (PropertyChanging(args))
				{
					_IsElectricityAvailable = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IsCorrectlyWWI
		{	
			get{ return _IsCorrectlyWWI; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsCorrectlyWWI, value, _IsCorrectlyWWI);
				if (PropertyChanging(args))
				{
					_IsCorrectlyWWI = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IsCorrectlyWWINoText
		{	
			get{ return _IsCorrectlyWWINoText; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsCorrectlyWWINoText, value, _IsCorrectlyWWINoText);
				if (PropertyChanging(args))
				{
					_IsCorrectlyWWINoText = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IsCorrectlyWWIYesText
		{	
			get{ return _IsCorrectlyWWIYesText; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsCorrectlyWWIYesText, value, _IsCorrectlyWWIYesText);
				if (PropertyChanging(args))
				{
					_IsCorrectlyWWIYesText = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TicketScheduleTimeIsGood
		{	
			get{ return _TicketScheduleTimeIsGood; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TicketScheduleTimeIsGood, value, _TicketScheduleTimeIsGood);
				if (PropertyChanging(args))
				{
					_TicketScheduleTimeIsGood = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String DateOfBirthIsCorrect
		{	
			get{ return _DateOfBirthIsCorrect; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DateOfBirthIsCorrect, value, _DateOfBirthIsCorrect);
				if (PropertyChanging(args))
				{
					_DateOfBirthIsCorrect = value;
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
		public String DateOfBirthUpdateNote
		{	
			get{ return _DateOfBirthUpdateNote; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DateOfBirthUpdateNote, value, _DateOfBirthUpdateNote);
				if (PropertyChanging(args))
				{
					_DateOfBirthUpdateNote = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IsGenerallyResponsible
		{	
			get{ return _IsGenerallyResponsible; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsGenerallyResponsible, value, _IsGenerallyResponsible);
				if (PropertyChanging(args))
				{
					_IsGenerallyResponsible = value;
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
		public String CreatedBy
		{	
			get{ return _CreatedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreatedBy, value, _CreatedBy);
				if (PropertyChanging(args))
				{
					_CreatedBy = value;
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
		public Boolean IsCompleted
		{	
			get{ return _IsCompleted; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsCompleted, value, _IsCompleted);
				if (PropertyChanging(args))
				{
					_IsCompleted = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IsSignShowingBusinessYes
		{	
			get{ return _IsSignShowingBusinessYes; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsSignShowingBusinessYes, value, _IsSignShowingBusinessYes);
				if (PropertyChanging(args))
				{
					_IsSignShowingBusinessYes = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ManualNote
		{	
			get{ return _ManualNote; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ManualNote, value, _ManualNote);
				if (PropertyChanging(args))
				{
					_ManualNote = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid QARep
		{	
			get{ return _QARep; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_QARep, value, _QARep);
				if (PropertyChanging(args))
				{
					_QARep = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid SalesRep
		{	
			get{ return _SalesRep; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SalesRep, value, _SalesRep);
				if (PropertyChanging(args))
				{
					_SalesRep = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime InstallDate
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
		public String HowQA1Initiated
		{	
			get{ return _HowQA1Initiated; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_HowQA1Initiated, value, _HowQA1Initiated);
				if (PropertyChanging(args))
				{
					_HowQA1Initiated = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IsCallingToday
		{	
			get{ return _IsCallingToday; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsCallingToday, value, _IsCallingToday);
				if (PropertyChanging(args))
				{
					_IsCallingToday = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SSNIsCorrect
		{	
			get{ return _SSNIsCorrect; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SSNIsCorrect, value, _SSNIsCorrect);
				if (PropertyChanging(args))
				{
					_SSNIsCorrect = value;
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
		public String SSNUpdateNote
		{	
			get{ return _SSNUpdateNote; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SSNUpdateNote, value, _SSNUpdateNote);
				if (PropertyChanging(args))
				{
					_SSNUpdateNote = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PermitIsRequired
		{	
			get{ return _PermitIsRequired; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PermitIsRequired, value, _PermitIsRequired);
				if (PropertyChanging(args))
				{
					_PermitIsRequired = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BillingIsCorrect
		{	
			get{ return _BillingIsCorrect; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BillingIsCorrect, value, _BillingIsCorrect);
				if (PropertyChanging(args))
				{
					_BillingIsCorrect = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PermissionGoAhead
		{	
			get{ return _PermissionGoAhead; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PermissionGoAhead, value, _PermissionGoAhead);
				if (PropertyChanging(args))
				{
					_PermissionGoAhead = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PermissionGoAheadNote
		{	
			get{ return _PermissionGoAheadNote; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PermissionGoAheadNote, value, _PermissionGoAheadNote);
				if (PropertyChanging(args))
				{
					_PermissionGoAheadNote = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SoundIsCorrect
		{	
			get{ return _SoundIsCorrect; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SoundIsCorrect, value, _SoundIsCorrect);
				if (PropertyChanging(args))
				{
					_SoundIsCorrect = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SoundCorrectNote
		{	
			get{ return _SoundCorrectNote; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SoundCorrectNote, value, _SoundCorrectNote);
				if (PropertyChanging(args))
				{
					_SoundCorrectNote = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IsAssistYou
		{	
			get{ return _IsAssistYou; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsAssistYou, value, _IsAssistYou);
				if (PropertyChanging(args))
				{
					_IsAssistYou = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IsPass
		{	
			get{ return _IsPass; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsPass, value, _IsPass);
				if (PropertyChanging(args))
				{
					_IsPass = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PassNote
		{	
			get{ return _PassNote; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PassNote, value, _PassNote);
				if (PropertyChanging(args))
				{
					_PassNote = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  QA1ScriptBase Clone()
		{
			QA1ScriptBase newObj = new  QA1ScriptBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CustomerId = this.CustomerId;						
			newObj.CompanyId = this.CompanyId;						
			newObj.FirstName = this.FirstName;						
			newObj.LastName = this.LastName;						
			newObj.NameIsCorrect = this.NameIsCorrect;						
			newObj.NameUpdateNote = this.NameUpdateNote;						
			newObj.Street = this.Street;						
			newObj.City = this.City;						
			newObj.State = this.State;						
			newObj.ZipCode = this.ZipCode;						
			newObj.AddressIsCorrect = this.AddressIsCorrect;						
			newObj.AddressUpdateNote = this.AddressUpdateNote;						
			newObj.IsLocationHome = this.IsLocationHome;						
			newObj.IsHomeRent = this.IsHomeRent;						
			newObj.HomeownerName = this.HomeownerName;						
			newObj.HomeownerNameUpdateNote = this.HomeownerNameUpdateNote;						
			newObj.IsSpeckRep = this.IsSpeckRep;						
			newObj.IsHomeownerAuthorized = this.IsHomeownerAuthorized;						
			newObj.IsOnTheDeed = this.IsOnTheDeed;						
			newObj.IsResponsibleParty = this.IsResponsibleParty;						
			newObj.ResponsiblePartyName = this.ResponsiblePartyName;						
			newObj.ReponsiblePartyNameUpdateNote = this.ReponsiblePartyNameUpdateNote;						
			newObj.IsSignShowingBusiness = this.IsSignShowingBusiness;						
			newObj.SignShowText = this.SignShowText;						
			newObj.IsOtherDocumentShowing = this.IsOtherDocumentShowing;						
			newObj.OtherDocumentShowingText = this.OtherDocumentShowingText;						
			newObj.IsBusinessSecretaryOfState = this.IsBusinessSecretaryOfState;						
			newObj.PrimaryPhone = this.PrimaryPhone;						
			newObj.PrimaryPhoneIsCorrect = this.PrimaryPhoneIsCorrect;						
			newObj.PrimaryPhoneUpdateNote = this.PrimaryPhoneUpdateNote;						
			newObj.EmailAddress = this.EmailAddress;						
			newObj.EmailIsCorrect = this.EmailIsCorrect;						
			newObj.EmailUpdateNote = this.EmailUpdateNote;						
			newObj.InstallTicketIsCorrect = this.InstallTicketIsCorrect;						
			newObj.TermAndFeeIsCorrect = this.TermAndFeeIsCorrect;						
			newObj.IsCurrentlyInContract = this.IsCurrentlyInContract;						
			newObj.CurrentlyContractText = this.CurrentlyContractText;						
			newObj.IsUnderstandAffiliateAndService = this.IsUnderstandAffiliateAndService;						
			newObj.UnderstandText = this.UnderstandText;						
			newObj.IsBrinksOrMonitronic = this.IsBrinksOrMonitronic;						
			newObj.BrinksOrMonitronicText = this.BrinksOrMonitronicText;						
			newObj.BrinksHowLong = this.BrinksHowLong;						
			newObj.IsPromisedByRep = this.IsPromisedByRep;						
			newObj.PromisedByRepText = this.PromisedByRepText;						
			newObj.WhatWasPromised = this.WhatWasPromised;						
			newObj.IsElectricityAvailable = this.IsElectricityAvailable;						
			newObj.IsCorrectlyWWI = this.IsCorrectlyWWI;						
			newObj.IsCorrectlyWWINoText = this.IsCorrectlyWWINoText;						
			newObj.IsCorrectlyWWIYesText = this.IsCorrectlyWWIYesText;						
			newObj.TicketScheduleTimeIsGood = this.TicketScheduleTimeIsGood;						
			newObj.DateOfBirthIsCorrect = this.DateOfBirthIsCorrect;						
			newObj.DateofBirth = this.DateofBirth;						
			newObj.DateOfBirthUpdateNote = this.DateOfBirthUpdateNote;						
			newObj.IsGenerallyResponsible = this.IsGenerallyResponsible;						
			newObj.Passcode = this.Passcode;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedByUid = this.CreatedByUid;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.LastUpdatedByUid = this.LastUpdatedByUid;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.IsCompleted = this.IsCompleted;						
			newObj.IsSignShowingBusinessYes = this.IsSignShowingBusinessYes;						
			newObj.ManualNote = this.ManualNote;						
			newObj.QARep = this.QARep;						
			newObj.SalesRep = this.SalesRep;						
			newObj.InstallDate = this.InstallDate;						
			newObj.HowQA1Initiated = this.HowQA1Initiated;						
			newObj.IsCallingToday = this.IsCallingToday;						
			newObj.SSNIsCorrect = this.SSNIsCorrect;						
			newObj.SSN = this.SSN;						
			newObj.SSNUpdateNote = this.SSNUpdateNote;						
			newObj.PermitIsRequired = this.PermitIsRequired;						
			newObj.BillingIsCorrect = this.BillingIsCorrect;						
			newObj.PermissionGoAhead = this.PermissionGoAhead;						
			newObj.PermissionGoAheadNote = this.PermissionGoAheadNote;						
			newObj.SoundIsCorrect = this.SoundIsCorrect;						
			newObj.SoundCorrectNote = this.SoundCorrectNote;						
			newObj.IsAssistYou = this.IsAssistYou;						
			newObj.IsPass = this.IsPass;						
			newObj.PassNote = this.PassNote;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(QA1ScriptBase.Property_Id, Id);				
			info.AddValue(QA1ScriptBase.Property_CustomerId, CustomerId);				
			info.AddValue(QA1ScriptBase.Property_CompanyId, CompanyId);				
			info.AddValue(QA1ScriptBase.Property_FirstName, FirstName);				
			info.AddValue(QA1ScriptBase.Property_LastName, LastName);				
			info.AddValue(QA1ScriptBase.Property_NameIsCorrect, NameIsCorrect);				
			info.AddValue(QA1ScriptBase.Property_NameUpdateNote, NameUpdateNote);				
			info.AddValue(QA1ScriptBase.Property_Street, Street);				
			info.AddValue(QA1ScriptBase.Property_City, City);				
			info.AddValue(QA1ScriptBase.Property_State, State);				
			info.AddValue(QA1ScriptBase.Property_ZipCode, ZipCode);				
			info.AddValue(QA1ScriptBase.Property_AddressIsCorrect, AddressIsCorrect);				
			info.AddValue(QA1ScriptBase.Property_AddressUpdateNote, AddressUpdateNote);				
			info.AddValue(QA1ScriptBase.Property_IsLocationHome, IsLocationHome);				
			info.AddValue(QA1ScriptBase.Property_IsHomeRent, IsHomeRent);				
			info.AddValue(QA1ScriptBase.Property_HomeownerName, HomeownerName);				
			info.AddValue(QA1ScriptBase.Property_HomeownerNameUpdateNote, HomeownerNameUpdateNote);				
			info.AddValue(QA1ScriptBase.Property_IsSpeckRep, IsSpeckRep);				
			info.AddValue(QA1ScriptBase.Property_IsHomeownerAuthorized, IsHomeownerAuthorized);				
			info.AddValue(QA1ScriptBase.Property_IsOnTheDeed, IsOnTheDeed);				
			info.AddValue(QA1ScriptBase.Property_IsResponsibleParty, IsResponsibleParty);				
			info.AddValue(QA1ScriptBase.Property_ResponsiblePartyName, ResponsiblePartyName);				
			info.AddValue(QA1ScriptBase.Property_ReponsiblePartyNameUpdateNote, ReponsiblePartyNameUpdateNote);				
			info.AddValue(QA1ScriptBase.Property_IsSignShowingBusiness, IsSignShowingBusiness);				
			info.AddValue(QA1ScriptBase.Property_SignShowText, SignShowText);				
			info.AddValue(QA1ScriptBase.Property_IsOtherDocumentShowing, IsOtherDocumentShowing);				
			info.AddValue(QA1ScriptBase.Property_OtherDocumentShowingText, OtherDocumentShowingText);				
			info.AddValue(QA1ScriptBase.Property_IsBusinessSecretaryOfState, IsBusinessSecretaryOfState);				
			info.AddValue(QA1ScriptBase.Property_PrimaryPhone, PrimaryPhone);				
			info.AddValue(QA1ScriptBase.Property_PrimaryPhoneIsCorrect, PrimaryPhoneIsCorrect);				
			info.AddValue(QA1ScriptBase.Property_PrimaryPhoneUpdateNote, PrimaryPhoneUpdateNote);				
			info.AddValue(QA1ScriptBase.Property_EmailAddress, EmailAddress);				
			info.AddValue(QA1ScriptBase.Property_EmailIsCorrect, EmailIsCorrect);				
			info.AddValue(QA1ScriptBase.Property_EmailUpdateNote, EmailUpdateNote);				
			info.AddValue(QA1ScriptBase.Property_InstallTicketIsCorrect, InstallTicketIsCorrect);				
			info.AddValue(QA1ScriptBase.Property_TermAndFeeIsCorrect, TermAndFeeIsCorrect);				
			info.AddValue(QA1ScriptBase.Property_IsCurrentlyInContract, IsCurrentlyInContract);				
			info.AddValue(QA1ScriptBase.Property_CurrentlyContractText, CurrentlyContractText);				
			info.AddValue(QA1ScriptBase.Property_IsUnderstandAffiliateAndService, IsUnderstandAffiliateAndService);				
			info.AddValue(QA1ScriptBase.Property_UnderstandText, UnderstandText);				
			info.AddValue(QA1ScriptBase.Property_IsBrinksOrMonitronic, IsBrinksOrMonitronic);				
			info.AddValue(QA1ScriptBase.Property_BrinksOrMonitronicText, BrinksOrMonitronicText);				
			info.AddValue(QA1ScriptBase.Property_BrinksHowLong, BrinksHowLong);				
			info.AddValue(QA1ScriptBase.Property_IsPromisedByRep, IsPromisedByRep);				
			info.AddValue(QA1ScriptBase.Property_PromisedByRepText, PromisedByRepText);				
			info.AddValue(QA1ScriptBase.Property_WhatWasPromised, WhatWasPromised);				
			info.AddValue(QA1ScriptBase.Property_IsElectricityAvailable, IsElectricityAvailable);				
			info.AddValue(QA1ScriptBase.Property_IsCorrectlyWWI, IsCorrectlyWWI);				
			info.AddValue(QA1ScriptBase.Property_IsCorrectlyWWINoText, IsCorrectlyWWINoText);				
			info.AddValue(QA1ScriptBase.Property_IsCorrectlyWWIYesText, IsCorrectlyWWIYesText);				
			info.AddValue(QA1ScriptBase.Property_TicketScheduleTimeIsGood, TicketScheduleTimeIsGood);				
			info.AddValue(QA1ScriptBase.Property_DateOfBirthIsCorrect, DateOfBirthIsCorrect);				
			info.AddValue(QA1ScriptBase.Property_DateofBirth, DateofBirth);				
			info.AddValue(QA1ScriptBase.Property_DateOfBirthUpdateNote, DateOfBirthUpdateNote);				
			info.AddValue(QA1ScriptBase.Property_IsGenerallyResponsible, IsGenerallyResponsible);				
			info.AddValue(QA1ScriptBase.Property_Passcode, Passcode);				
			info.AddValue(QA1ScriptBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(QA1ScriptBase.Property_CreatedByUid, CreatedByUid);				
			info.AddValue(QA1ScriptBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(QA1ScriptBase.Property_LastUpdatedByUid, LastUpdatedByUid);				
			info.AddValue(QA1ScriptBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(QA1ScriptBase.Property_IsCompleted, IsCompleted);				
			info.AddValue(QA1ScriptBase.Property_IsSignShowingBusinessYes, IsSignShowingBusinessYes);				
			info.AddValue(QA1ScriptBase.Property_ManualNote, ManualNote);				
			info.AddValue(QA1ScriptBase.Property_QARep, QARep);				
			info.AddValue(QA1ScriptBase.Property_SalesRep, SalesRep);				
			info.AddValue(QA1ScriptBase.Property_InstallDate, InstallDate);				
			info.AddValue(QA1ScriptBase.Property_HowQA1Initiated, HowQA1Initiated);				
			info.AddValue(QA1ScriptBase.Property_IsCallingToday, IsCallingToday);				
			info.AddValue(QA1ScriptBase.Property_SSNIsCorrect, SSNIsCorrect);				
			info.AddValue(QA1ScriptBase.Property_SSN, SSN);				
			info.AddValue(QA1ScriptBase.Property_SSNUpdateNote, SSNUpdateNote);				
			info.AddValue(QA1ScriptBase.Property_PermitIsRequired, PermitIsRequired);				
			info.AddValue(QA1ScriptBase.Property_BillingIsCorrect, BillingIsCorrect);				
			info.AddValue(QA1ScriptBase.Property_PermissionGoAhead, PermissionGoAhead);				
			info.AddValue(QA1ScriptBase.Property_PermissionGoAheadNote, PermissionGoAheadNote);				
			info.AddValue(QA1ScriptBase.Property_SoundIsCorrect, SoundIsCorrect);				
			info.AddValue(QA1ScriptBase.Property_SoundCorrectNote, SoundCorrectNote);				
			info.AddValue(QA1ScriptBase.Property_IsAssistYou, IsAssistYou);				
			info.AddValue(QA1ScriptBase.Property_IsPass, IsPass);				
			info.AddValue(QA1ScriptBase.Property_PassNote, PassNote);				
		}
		#endregion

		
	}
}
