using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "PowerPayFinanceBase", Namespace = "http://www.hims-tech.com//entities")]
	public class PowerPayFinanceBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CustomerId = 1,
			Firstname = 2,
			Lastname = 3,
			DOB = 4,
			Phone = 5,
			Email = 6,
			SSN = 7,
			USCitizen = 8,
			PPAddressPB = 9,
			AddressStreetNumber = 10,
			AddressStreetName = 11,
			AddressStreetType = 12,
			POBox = 13,
			City = 14,
			State = 15,
			ZipCode = 16,
			AddressHouseType = 17,
			ActiveMilitary = 18,
			DriversLicense = 19,
			DriversLicenseState = 20,
			AnnualIncome = 21,
			IncomeFrequency = 22,
			EmployerOccupation = 23,
			EmployerName = 24,
			EmployerZip = 25,
			EmploymentType = 26,
			EmployerYears = 27,
			RequestedLoanAmount = 28,
			ith_email = 29,
			IHRMobileNumber = 30,
			CoFirstname = 31,
			CoLastname = 32,
			CoDOB = 33,
			CoPhone = 34,
			CoEmail = 35,
			CoSSN = 36,
			CoUSCitizen = 37,
			CoPPAddressPB = 38,
			CoAddressStreetNumber = 39,
			CoAddressStreetName = 40,
			CoAddressStreetType = 41,
			CoPOBox = 42,
			CoCity = 43,
			CoState = 44,
			CoZipCode = 45,
			CoAddressHouseType = 46,
			CoActiveMilitary = 47,
			CoDriversLicense = 48,
			CoDriversLicenseState = 49,
			CoAnnualIncome = 50,
			CoIncomeFrequency = 51,
			CoEmployerOccupation = 52,
			CoEmployerName = 53,
			CoEmployerZip = 54,
			CoEmploymentType = 55,
			CoEmployerYears = 56,
			IsApply = 57
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_Firstname = "Firstname";		            
		public const string Property_Lastname = "Lastname";		            
		public const string Property_DOB = "DOB";		            
		public const string Property_Phone = "Phone";		            
		public const string Property_Email = "Email";		            
		public const string Property_SSN = "SSN";		            
		public const string Property_USCitizen = "USCitizen";		            
		public const string Property_PPAddressPB = "PPAddressPB";		            
		public const string Property_AddressStreetNumber = "AddressStreetNumber";		            
		public const string Property_AddressStreetName = "AddressStreetName";		            
		public const string Property_AddressStreetType = "AddressStreetType";		            
		public const string Property_POBox = "POBox";		            
		public const string Property_City = "City";		            
		public const string Property_State = "State";		            
		public const string Property_ZipCode = "ZipCode";		            
		public const string Property_AddressHouseType = "AddressHouseType";		            
		public const string Property_ActiveMilitary = "ActiveMilitary";		            
		public const string Property_DriversLicense = "DriversLicense";		            
		public const string Property_DriversLicenseState = "DriversLicenseState";		            
		public const string Property_AnnualIncome = "AnnualIncome";		            
		public const string Property_IncomeFrequency = "IncomeFrequency";		            
		public const string Property_EmployerOccupation = "EmployerOccupation";		            
		public const string Property_EmployerName = "EmployerName";		            
		public const string Property_EmployerZip = "EmployerZip";		            
		public const string Property_EmploymentType = "EmploymentType";		            
		public const string Property_EmployerYears = "EmployerYears";		            
		public const string Property_RequestedLoanAmount = "RequestedLoanAmount";		            
		public const string Property_ith_email = "ith_email";		            
		public const string Property_IHRMobileNumber = "IHRMobileNumber";		            
		public const string Property_CoFirstname = "CoFirstname";		            
		public const string Property_CoLastname = "CoLastname";		            
		public const string Property_CoDOB = "CoDOB";		            
		public const string Property_CoPhone = "CoPhone";		            
		public const string Property_CoEmail = "CoEmail";		            
		public const string Property_CoSSN = "CoSSN";		            
		public const string Property_CoUSCitizen = "CoUSCitizen";		            
		public const string Property_CoPPAddressPB = "CoPPAddressPB";		            
		public const string Property_CoAddressStreetNumber = "CoAddressStreetNumber";		            
		public const string Property_CoAddressStreetName = "CoAddressStreetName";		            
		public const string Property_CoAddressStreetType = "CoAddressStreetType";		            
		public const string Property_CoPOBox = "CoPOBox";		            
		public const string Property_CoCity = "CoCity";		            
		public const string Property_CoState = "CoState";		            
		public const string Property_CoZipCode = "CoZipCode";		            
		public const string Property_CoAddressHouseType = "CoAddressHouseType";		            
		public const string Property_CoActiveMilitary = "CoActiveMilitary";		            
		public const string Property_CoDriversLicense = "CoDriversLicense";		            
		public const string Property_CoDriversLicenseState = "CoDriversLicenseState";		            
		public const string Property_CoAnnualIncome = "CoAnnualIncome";		            
		public const string Property_CoIncomeFrequency = "CoIncomeFrequency";		            
		public const string Property_CoEmployerOccupation = "CoEmployerOccupation";		            
		public const string Property_CoEmployerName = "CoEmployerName";		            
		public const string Property_CoEmployerZip = "CoEmployerZip";		            
		public const string Property_CoEmploymentType = "CoEmploymentType";		            
		public const string Property_CoEmployerYears = "CoEmployerYears";		            
		public const string Property_IsApply = "IsApply";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CustomerId;	            
		private String _Firstname;	            
		private String _Lastname;	            
		private Nullable<DateTime> _DOB;	            
		private String _Phone;	            
		private String _Email;	            
		private String _SSN;	            
		private String _USCitizen;	            
		private String _PPAddressPB;	            
		private String _AddressStreetNumber;	            
		private String _AddressStreetName;	            
		private String _AddressStreetType;	            
		private String _POBox;	            
		private String _City;	            
		private String _State;	            
		private String _ZipCode;	            
		private String _AddressHouseType;	            
		private String _ActiveMilitary;	            
		private String _DriversLicense;	            
		private String _DriversLicenseState;	            
		private Nullable<Double> _AnnualIncome;	            
		private String _IncomeFrequency;	            
		private String _EmployerOccupation;	            
		private String _EmployerName;	            
		private String _EmployerZip;	            
		private String _EmploymentType;	            
		private Nullable<Int32> _EmployerYears;	            
		private Nullable<Double> _RequestedLoanAmount;	            
		private String _ith_email;	            
		private String _IHRMobileNumber;	            
		private String _CoFirstname;	            
		private String _CoLastname;	            
		private Nullable<DateTime> _CoDOB;	            
		private String _CoPhone;	            
		private String _CoEmail;	            
		private String _CoSSN;	            
		private String _CoUSCitizen;	            
		private String _CoPPAddressPB;	            
		private String _CoAddressStreetNumber;	            
		private String _CoAddressStreetName;	            
		private String _CoAddressStreetType;	            
		private String _CoPOBox;	            
		private String _CoCity;	            
		private String _CoState;	            
		private String _CoZipCode;	            
		private String _CoAddressHouseType;	            
		private String _CoActiveMilitary;	            
		private String _CoDriversLicense;	            
		private String _CoDriversLicenseState;	            
		private Nullable<Double> _CoAnnualIncome;	            
		private String _CoIncomeFrequency;	            
		private String _CoEmployerOccupation;	            
		private String _CoEmployerName;	            
		private String _CoEmployerZip;	            
		private String _CoEmploymentType;	            
		private Nullable<Int32> _CoEmployerYears;	            
		private Nullable<Boolean> _IsApply;	            
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
		public String Firstname
		{	
			get{ return _Firstname; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Firstname, value, _Firstname);
				if (PropertyChanging(args))
				{
					_Firstname = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Lastname
		{	
			get{ return _Lastname; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Lastname, value, _Lastname);
				if (PropertyChanging(args))
				{
					_Lastname = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> DOB
		{	
			get{ return _DOB; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DOB, value, _DOB);
				if (PropertyChanging(args))
				{
					_DOB = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Phone
		{	
			get{ return _Phone; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Phone, value, _Phone);
				if (PropertyChanging(args))
				{
					_Phone = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Email
		{	
			get{ return _Email; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Email, value, _Email);
				if (PropertyChanging(args))
				{
					_Email = value;
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
		public String USCitizen
		{	
			get{ return _USCitizen; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_USCitizen, value, _USCitizen);
				if (PropertyChanging(args))
				{
					_USCitizen = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PPAddressPB
		{	
			get{ return _PPAddressPB; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PPAddressPB, value, _PPAddressPB);
				if (PropertyChanging(args))
				{
					_PPAddressPB = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AddressStreetNumber
		{	
			get{ return _AddressStreetNumber; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AddressStreetNumber, value, _AddressStreetNumber);
				if (PropertyChanging(args))
				{
					_AddressStreetNumber = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AddressStreetName
		{	
			get{ return _AddressStreetName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AddressStreetName, value, _AddressStreetName);
				if (PropertyChanging(args))
				{
					_AddressStreetName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AddressStreetType
		{	
			get{ return _AddressStreetType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AddressStreetType, value, _AddressStreetType);
				if (PropertyChanging(args))
				{
					_AddressStreetType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String POBox
		{	
			get{ return _POBox; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_POBox, value, _POBox);
				if (PropertyChanging(args))
				{
					_POBox = value;
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
		public String AddressHouseType
		{	
			get{ return _AddressHouseType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AddressHouseType, value, _AddressHouseType);
				if (PropertyChanging(args))
				{
					_AddressHouseType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ActiveMilitary
		{	
			get{ return _ActiveMilitary; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ActiveMilitary, value, _ActiveMilitary);
				if (PropertyChanging(args))
				{
					_ActiveMilitary = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String DriversLicense
		{	
			get{ return _DriversLicense; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DriversLicense, value, _DriversLicense);
				if (PropertyChanging(args))
				{
					_DriversLicense = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String DriversLicenseState
		{	
			get{ return _DriversLicenseState; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DriversLicenseState, value, _DriversLicenseState);
				if (PropertyChanging(args))
				{
					_DriversLicenseState = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> AnnualIncome
		{	
			get{ return _AnnualIncome; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AnnualIncome, value, _AnnualIncome);
				if (PropertyChanging(args))
				{
					_AnnualIncome = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IncomeFrequency
		{	
			get{ return _IncomeFrequency; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IncomeFrequency, value, _IncomeFrequency);
				if (PropertyChanging(args))
				{
					_IncomeFrequency = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String EmployerOccupation
		{	
			get{ return _EmployerOccupation; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EmployerOccupation, value, _EmployerOccupation);
				if (PropertyChanging(args))
				{
					_EmployerOccupation = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String EmployerName
		{	
			get{ return _EmployerName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EmployerName, value, _EmployerName);
				if (PropertyChanging(args))
				{
					_EmployerName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String EmployerZip
		{	
			get{ return _EmployerZip; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EmployerZip, value, _EmployerZip);
				if (PropertyChanging(args))
				{
					_EmployerZip = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String EmploymentType
		{	
			get{ return _EmploymentType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EmploymentType, value, _EmploymentType);
				if (PropertyChanging(args))
				{
					_EmploymentType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> EmployerYears
		{	
			get{ return _EmployerYears; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EmployerYears, value, _EmployerYears);
				if (PropertyChanging(args))
				{
					_EmployerYears = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> RequestedLoanAmount
		{	
			get{ return _RequestedLoanAmount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RequestedLoanAmount, value, _RequestedLoanAmount);
				if (PropertyChanging(args))
				{
					_RequestedLoanAmount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ith_email
		{	
			get{ return _ith_email; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ith_email, value, _ith_email);
				if (PropertyChanging(args))
				{
					_ith_email = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IHRMobileNumber
		{	
			get{ return _IHRMobileNumber; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IHRMobileNumber, value, _IHRMobileNumber);
				if (PropertyChanging(args))
				{
					_IHRMobileNumber = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoFirstname
		{	
			get{ return _CoFirstname; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoFirstname, value, _CoFirstname);
				if (PropertyChanging(args))
				{
					_CoFirstname = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoLastname
		{	
			get{ return _CoLastname; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoLastname, value, _CoLastname);
				if (PropertyChanging(args))
				{
					_CoLastname = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> CoDOB
		{	
			get{ return _CoDOB; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoDOB, value, _CoDOB);
				if (PropertyChanging(args))
				{
					_CoDOB = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoPhone
		{	
			get{ return _CoPhone; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoPhone, value, _CoPhone);
				if (PropertyChanging(args))
				{
					_CoPhone = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoEmail
		{	
			get{ return _CoEmail; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoEmail, value, _CoEmail);
				if (PropertyChanging(args))
				{
					_CoEmail = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoSSN
		{	
			get{ return _CoSSN; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoSSN, value, _CoSSN);
				if (PropertyChanging(args))
				{
					_CoSSN = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoUSCitizen
		{	
			get{ return _CoUSCitizen; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoUSCitizen, value, _CoUSCitizen);
				if (PropertyChanging(args))
				{
					_CoUSCitizen = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoPPAddressPB
		{	
			get{ return _CoPPAddressPB; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoPPAddressPB, value, _CoPPAddressPB);
				if (PropertyChanging(args))
				{
					_CoPPAddressPB = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoAddressStreetNumber
		{	
			get{ return _CoAddressStreetNumber; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoAddressStreetNumber, value, _CoAddressStreetNumber);
				if (PropertyChanging(args))
				{
					_CoAddressStreetNumber = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoAddressStreetName
		{	
			get{ return _CoAddressStreetName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoAddressStreetName, value, _CoAddressStreetName);
				if (PropertyChanging(args))
				{
					_CoAddressStreetName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoAddressStreetType
		{	
			get{ return _CoAddressStreetType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoAddressStreetType, value, _CoAddressStreetType);
				if (PropertyChanging(args))
				{
					_CoAddressStreetType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoPOBox
		{	
			get{ return _CoPOBox; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoPOBox, value, _CoPOBox);
				if (PropertyChanging(args))
				{
					_CoPOBox = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoCity
		{	
			get{ return _CoCity; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoCity, value, _CoCity);
				if (PropertyChanging(args))
				{
					_CoCity = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoState
		{	
			get{ return _CoState; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoState, value, _CoState);
				if (PropertyChanging(args))
				{
					_CoState = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoZipCode
		{	
			get{ return _CoZipCode; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoZipCode, value, _CoZipCode);
				if (PropertyChanging(args))
				{
					_CoZipCode = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoAddressHouseType
		{	
			get{ return _CoAddressHouseType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoAddressHouseType, value, _CoAddressHouseType);
				if (PropertyChanging(args))
				{
					_CoAddressHouseType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoActiveMilitary
		{	
			get{ return _CoActiveMilitary; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoActiveMilitary, value, _CoActiveMilitary);
				if (PropertyChanging(args))
				{
					_CoActiveMilitary = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoDriversLicense
		{	
			get{ return _CoDriversLicense; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoDriversLicense, value, _CoDriversLicense);
				if (PropertyChanging(args))
				{
					_CoDriversLicense = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoDriversLicenseState
		{	
			get{ return _CoDriversLicenseState; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoDriversLicenseState, value, _CoDriversLicenseState);
				if (PropertyChanging(args))
				{
					_CoDriversLicenseState = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> CoAnnualIncome
		{	
			get{ return _CoAnnualIncome; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoAnnualIncome, value, _CoAnnualIncome);
				if (PropertyChanging(args))
				{
					_CoAnnualIncome = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoIncomeFrequency
		{	
			get{ return _CoIncomeFrequency; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoIncomeFrequency, value, _CoIncomeFrequency);
				if (PropertyChanging(args))
				{
					_CoIncomeFrequency = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoEmployerOccupation
		{	
			get{ return _CoEmployerOccupation; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoEmployerOccupation, value, _CoEmployerOccupation);
				if (PropertyChanging(args))
				{
					_CoEmployerOccupation = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoEmployerName
		{	
			get{ return _CoEmployerName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoEmployerName, value, _CoEmployerName);
				if (PropertyChanging(args))
				{
					_CoEmployerName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoEmployerZip
		{	
			get{ return _CoEmployerZip; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoEmployerZip, value, _CoEmployerZip);
				if (PropertyChanging(args))
				{
					_CoEmployerZip = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoEmploymentType
		{	
			get{ return _CoEmploymentType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoEmploymentType, value, _CoEmploymentType);
				if (PropertyChanging(args))
				{
					_CoEmploymentType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> CoEmployerYears
		{	
			get{ return _CoEmployerYears; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoEmployerYears, value, _CoEmployerYears);
				if (PropertyChanging(args))
				{
					_CoEmployerYears = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsApply
		{	
			get{ return _IsApply; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsApply, value, _IsApply);
				if (PropertyChanging(args))
				{
					_IsApply = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  PowerPayFinanceBase Clone()
		{
			PowerPayFinanceBase newObj = new  PowerPayFinanceBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CustomerId = this.CustomerId;						
			newObj.Firstname = this.Firstname;						
			newObj.Lastname = this.Lastname;						
			newObj.DOB = this.DOB;						
			newObj.Phone = this.Phone;						
			newObj.Email = this.Email;						
			newObj.SSN = this.SSN;						
			newObj.USCitizen = this.USCitizen;						
			newObj.PPAddressPB = this.PPAddressPB;						
			newObj.AddressStreetNumber = this.AddressStreetNumber;						
			newObj.AddressStreetName = this.AddressStreetName;						
			newObj.AddressStreetType = this.AddressStreetType;						
			newObj.POBox = this.POBox;						
			newObj.City = this.City;						
			newObj.State = this.State;						
			newObj.ZipCode = this.ZipCode;						
			newObj.AddressHouseType = this.AddressHouseType;						
			newObj.ActiveMilitary = this.ActiveMilitary;						
			newObj.DriversLicense = this.DriversLicense;						
			newObj.DriversLicenseState = this.DriversLicenseState;						
			newObj.AnnualIncome = this.AnnualIncome;						
			newObj.IncomeFrequency = this.IncomeFrequency;						
			newObj.EmployerOccupation = this.EmployerOccupation;						
			newObj.EmployerName = this.EmployerName;						
			newObj.EmployerZip = this.EmployerZip;						
			newObj.EmploymentType = this.EmploymentType;						
			newObj.EmployerYears = this.EmployerYears;						
			newObj.RequestedLoanAmount = this.RequestedLoanAmount;						
			newObj.ith_email = this.ith_email;						
			newObj.IHRMobileNumber = this.IHRMobileNumber;						
			newObj.CoFirstname = this.CoFirstname;						
			newObj.CoLastname = this.CoLastname;						
			newObj.CoDOB = this.CoDOB;						
			newObj.CoPhone = this.CoPhone;						
			newObj.CoEmail = this.CoEmail;						
			newObj.CoSSN = this.CoSSN;						
			newObj.CoUSCitizen = this.CoUSCitizen;						
			newObj.CoPPAddressPB = this.CoPPAddressPB;						
			newObj.CoAddressStreetNumber = this.CoAddressStreetNumber;						
			newObj.CoAddressStreetName = this.CoAddressStreetName;						
			newObj.CoAddressStreetType = this.CoAddressStreetType;						
			newObj.CoPOBox = this.CoPOBox;						
			newObj.CoCity = this.CoCity;						
			newObj.CoState = this.CoState;						
			newObj.CoZipCode = this.CoZipCode;						
			newObj.CoAddressHouseType = this.CoAddressHouseType;						
			newObj.CoActiveMilitary = this.CoActiveMilitary;						
			newObj.CoDriversLicense = this.CoDriversLicense;						
			newObj.CoDriversLicenseState = this.CoDriversLicenseState;						
			newObj.CoAnnualIncome = this.CoAnnualIncome;						
			newObj.CoIncomeFrequency = this.CoIncomeFrequency;						
			newObj.CoEmployerOccupation = this.CoEmployerOccupation;						
			newObj.CoEmployerName = this.CoEmployerName;						
			newObj.CoEmployerZip = this.CoEmployerZip;						
			newObj.CoEmploymentType = this.CoEmploymentType;						
			newObj.CoEmployerYears = this.CoEmployerYears;						
			newObj.IsApply = this.IsApply;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(PowerPayFinanceBase.Property_Id, Id);				
			info.AddValue(PowerPayFinanceBase.Property_CustomerId, CustomerId);				
			info.AddValue(PowerPayFinanceBase.Property_Firstname, Firstname);				
			info.AddValue(PowerPayFinanceBase.Property_Lastname, Lastname);				
			info.AddValue(PowerPayFinanceBase.Property_DOB, DOB);				
			info.AddValue(PowerPayFinanceBase.Property_Phone, Phone);				
			info.AddValue(PowerPayFinanceBase.Property_Email, Email);				
			info.AddValue(PowerPayFinanceBase.Property_SSN, SSN);				
			info.AddValue(PowerPayFinanceBase.Property_USCitizen, USCitizen);				
			info.AddValue(PowerPayFinanceBase.Property_PPAddressPB, PPAddressPB);				
			info.AddValue(PowerPayFinanceBase.Property_AddressStreetNumber, AddressStreetNumber);				
			info.AddValue(PowerPayFinanceBase.Property_AddressStreetName, AddressStreetName);				
			info.AddValue(PowerPayFinanceBase.Property_AddressStreetType, AddressStreetType);				
			info.AddValue(PowerPayFinanceBase.Property_POBox, POBox);				
			info.AddValue(PowerPayFinanceBase.Property_City, City);				
			info.AddValue(PowerPayFinanceBase.Property_State, State);				
			info.AddValue(PowerPayFinanceBase.Property_ZipCode, ZipCode);				
			info.AddValue(PowerPayFinanceBase.Property_AddressHouseType, AddressHouseType);				
			info.AddValue(PowerPayFinanceBase.Property_ActiveMilitary, ActiveMilitary);				
			info.AddValue(PowerPayFinanceBase.Property_DriversLicense, DriversLicense);				
			info.AddValue(PowerPayFinanceBase.Property_DriversLicenseState, DriversLicenseState);				
			info.AddValue(PowerPayFinanceBase.Property_AnnualIncome, AnnualIncome);				
			info.AddValue(PowerPayFinanceBase.Property_IncomeFrequency, IncomeFrequency);				
			info.AddValue(PowerPayFinanceBase.Property_EmployerOccupation, EmployerOccupation);				
			info.AddValue(PowerPayFinanceBase.Property_EmployerName, EmployerName);				
			info.AddValue(PowerPayFinanceBase.Property_EmployerZip, EmployerZip);				
			info.AddValue(PowerPayFinanceBase.Property_EmploymentType, EmploymentType);				
			info.AddValue(PowerPayFinanceBase.Property_EmployerYears, EmployerYears);				
			info.AddValue(PowerPayFinanceBase.Property_RequestedLoanAmount, RequestedLoanAmount);				
			info.AddValue(PowerPayFinanceBase.Property_ith_email, ith_email);				
			info.AddValue(PowerPayFinanceBase.Property_IHRMobileNumber, IHRMobileNumber);				
			info.AddValue(PowerPayFinanceBase.Property_CoFirstname, CoFirstname);				
			info.AddValue(PowerPayFinanceBase.Property_CoLastname, CoLastname);				
			info.AddValue(PowerPayFinanceBase.Property_CoDOB, CoDOB);				
			info.AddValue(PowerPayFinanceBase.Property_CoPhone, CoPhone);				
			info.AddValue(PowerPayFinanceBase.Property_CoEmail, CoEmail);				
			info.AddValue(PowerPayFinanceBase.Property_CoSSN, CoSSN);				
			info.AddValue(PowerPayFinanceBase.Property_CoUSCitizen, CoUSCitizen);				
			info.AddValue(PowerPayFinanceBase.Property_CoPPAddressPB, CoPPAddressPB);				
			info.AddValue(PowerPayFinanceBase.Property_CoAddressStreetNumber, CoAddressStreetNumber);				
			info.AddValue(PowerPayFinanceBase.Property_CoAddressStreetName, CoAddressStreetName);				
			info.AddValue(PowerPayFinanceBase.Property_CoAddressStreetType, CoAddressStreetType);				
			info.AddValue(PowerPayFinanceBase.Property_CoPOBox, CoPOBox);				
			info.AddValue(PowerPayFinanceBase.Property_CoCity, CoCity);				
			info.AddValue(PowerPayFinanceBase.Property_CoState, CoState);				
			info.AddValue(PowerPayFinanceBase.Property_CoZipCode, CoZipCode);				
			info.AddValue(PowerPayFinanceBase.Property_CoAddressHouseType, CoAddressHouseType);				
			info.AddValue(PowerPayFinanceBase.Property_CoActiveMilitary, CoActiveMilitary);				
			info.AddValue(PowerPayFinanceBase.Property_CoDriversLicense, CoDriversLicense);				
			info.AddValue(PowerPayFinanceBase.Property_CoDriversLicenseState, CoDriversLicenseState);				
			info.AddValue(PowerPayFinanceBase.Property_CoAnnualIncome, CoAnnualIncome);				
			info.AddValue(PowerPayFinanceBase.Property_CoIncomeFrequency, CoIncomeFrequency);				
			info.AddValue(PowerPayFinanceBase.Property_CoEmployerOccupation, CoEmployerOccupation);				
			info.AddValue(PowerPayFinanceBase.Property_CoEmployerName, CoEmployerName);				
			info.AddValue(PowerPayFinanceBase.Property_CoEmployerZip, CoEmployerZip);				
			info.AddValue(PowerPayFinanceBase.Property_CoEmploymentType, CoEmploymentType);				
			info.AddValue(PowerPayFinanceBase.Property_CoEmployerYears, CoEmployerYears);				
			info.AddValue(PowerPayFinanceBase.Property_IsApply, IsApply);				
		}
		#endregion

		
	}
}
