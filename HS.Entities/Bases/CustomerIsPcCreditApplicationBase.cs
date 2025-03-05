using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomerIsPcCreditApplicationBase", Namespace = "http://www.hims-tech.com//entities")]
	public class CustomerIsPcCreditApplicationBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CustomerId = 1,
			MerchantId = 2,
			AmountRequested = 3,
			OptionCode = 4,
			ProdSecuritySystem = 5,
			ProdMiscDescription = 6,
			ApplicantLastName = 7,
			ApplicantFirstName = 8,
			ApplicantMiddleName = 9,
			ApplicantNameSuffix = 10,
			ApplicantDOB = 11,
			ApplicantSSN = 12,
			ApplicantHomePhone = 13,
			ApplicantCellPhone = 14,
			ApplicantEmailAddress = 15,
			ApplicantDriversLicense = 16,
			ApplicantStreet = 17,
			ApplicantCity = 18,
			ApplicantState = 19,
			ApplicantZipCode = 20,
			ApplicantCountry = 21,
			ApplicantYearsAtAddress = 22,
			ApplicantMonthsAtAddress = 23,
			MortgagePayment = 24,
			MortgageHolder = 25,
			MortgageBalance = 26,
			BankName = 27,
			BankAcctType = 28,
			ApplicantPrevStreet = 29,
			ApplicantPrevCity = 30,
			ApplicantPrevState = 31,
			ApplicantPrevZipCode = 32,
			ApplicantPrevCountry = 33,
			ApplicantPrevYearsAtAddress = 34,
			ApplicantPrevMonthsAtAddress = 35,
			ApplicantEmployer = 36,
			ApplicantYearsEmployed = 37,
			ApplicantMonthsEmployed = 38,
			ApplicantEmployerPhone = 39,
			ApplicantPosition = 40,
			ApplicantGrossAnnualIncome = 41,
			ApplicantOtherIncome = 42,
			ApplicantOtherIncomeSource = 43,
			SelectedAssignedUserId = 44,
			CoapplicantLastName = 45,
			CoapplicantFirstName = 46,
			CoapplicantMiddleName = 47,
			CoapplicantNameSuffix = 48,
			CoapplicantDOB = 49,
			CoapplicantSSN = 50,
			CoapplicantHomePhone = 51,
			CpapplicantCellPhone = 52,
			CoapplicantDriversLicense = 53,
			CoapplicantEmailAddress = 54,
			CoapplicantStreet = 55,
			CoapplicantCity = 56,
			CoapplicantState = 57,
			CoapplicantZipCode = 58,
			CoapplicantCountry = 59,
			CoapplicantYearsAtAddress = 60,
			CoapplicantMonthsAtAddress = 61,
			CoapplicantEmployer = 62,
			CoapplicantYearsEmployed = 63,
			CoapplicantMonthsEmployed = 64,
			CoapplicantEmployerPhone = 65,
			CoapplicantPosition = 66,
			CoapplicantGrossAnnualIncome = 67,
			CoapplicantOtherIncome = 68,
			CoapplicantOtherIncomeSource = 69
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_MerchantId = "MerchantId";		            
		public const string Property_AmountRequested = "AmountRequested";		            
		public const string Property_OptionCode = "OptionCode";		            
		public const string Property_ProdSecuritySystem = "ProdSecuritySystem";		            
		public const string Property_ProdMiscDescription = "ProdMiscDescription";		            
		public const string Property_ApplicantLastName = "ApplicantLastName";		            
		public const string Property_ApplicantFirstName = "ApplicantFirstName";		            
		public const string Property_ApplicantMiddleName = "ApplicantMiddleName";		            
		public const string Property_ApplicantNameSuffix = "ApplicantNameSuffix";		            
		public const string Property_ApplicantDOB = "ApplicantDOB";		            
		public const string Property_ApplicantSSN = "ApplicantSSN";		            
		public const string Property_ApplicantHomePhone = "ApplicantHomePhone";		            
		public const string Property_ApplicantCellPhone = "ApplicantCellPhone";		            
		public const string Property_ApplicantEmailAddress = "ApplicantEmailAddress";		            
		public const string Property_ApplicantDriversLicense = "ApplicantDriversLicense";		            
		public const string Property_ApplicantStreet = "ApplicantStreet";		            
		public const string Property_ApplicantCity = "ApplicantCity";		            
		public const string Property_ApplicantState = "ApplicantState";		            
		public const string Property_ApplicantZipCode = "ApplicantZipCode";		            
		public const string Property_ApplicantCountry = "ApplicantCountry";		            
		public const string Property_ApplicantYearsAtAddress = "ApplicantYearsAtAddress";		            
		public const string Property_ApplicantMonthsAtAddress = "ApplicantMonthsAtAddress";		            
		public const string Property_MortgagePayment = "MortgagePayment";		            
		public const string Property_MortgageHolder = "MortgageHolder";		            
		public const string Property_MortgageBalance = "MortgageBalance";		            
		public const string Property_BankName = "BankName";		            
		public const string Property_BankAcctType = "BankAcctType";		            
		public const string Property_ApplicantPrevStreet = "ApplicantPrevStreet";		            
		public const string Property_ApplicantPrevCity = "ApplicantPrevCity";		            
		public const string Property_ApplicantPrevState = "ApplicantPrevState";		            
		public const string Property_ApplicantPrevZipCode = "ApplicantPrevZipCode";		            
		public const string Property_ApplicantPrevCountry = "ApplicantPrevCountry";		            
		public const string Property_ApplicantPrevYearsAtAddress = "ApplicantPrevYearsAtAddress";		            
		public const string Property_ApplicantPrevMonthsAtAddress = "ApplicantPrevMonthsAtAddress";		            
		public const string Property_ApplicantEmployer = "ApplicantEmployer";		            
		public const string Property_ApplicantYearsEmployed = "ApplicantYearsEmployed";		            
		public const string Property_ApplicantMonthsEmployed = "ApplicantMonthsEmployed";		            
		public const string Property_ApplicantEmployerPhone = "ApplicantEmployerPhone";		            
		public const string Property_ApplicantPosition = "ApplicantPosition";		            
		public const string Property_ApplicantGrossAnnualIncome = "ApplicantGrossAnnualIncome";		            
		public const string Property_ApplicantOtherIncome = "ApplicantOtherIncome";		            
		public const string Property_ApplicantOtherIncomeSource = "ApplicantOtherIncomeSource";		            
		public const string Property_SelectedAssignedUserId = "SelectedAssignedUserId";		            
		public const string Property_CoapplicantLastName = "CoapplicantLastName";		            
		public const string Property_CoapplicantFirstName = "CoapplicantFirstName";		            
		public const string Property_CoapplicantMiddleName = "CoapplicantMiddleName";		            
		public const string Property_CoapplicantNameSuffix = "CoapplicantNameSuffix";		            
		public const string Property_CoapplicantDOB = "CoapplicantDOB";		            
		public const string Property_CoapplicantSSN = "CoapplicantSSN";		            
		public const string Property_CoapplicantHomePhone = "CoapplicantHomePhone";		            
		public const string Property_CpapplicantCellPhone = "CpapplicantCellPhone";		            
		public const string Property_CoapplicantDriversLicense = "CoapplicantDriversLicense";		            
		public const string Property_CoapplicantEmailAddress = "CoapplicantEmailAddress";		            
		public const string Property_CoapplicantStreet = "CoapplicantStreet";		            
		public const string Property_CoapplicantCity = "CoapplicantCity";		            
		public const string Property_CoapplicantState = "CoapplicantState";		            
		public const string Property_CoapplicantZipCode = "CoapplicantZipCode";		            
		public const string Property_CoapplicantCountry = "CoapplicantCountry";		            
		public const string Property_CoapplicantYearsAtAddress = "CoapplicantYearsAtAddress";		            
		public const string Property_CoapplicantMonthsAtAddress = "CoapplicantMonthsAtAddress";		            
		public const string Property_CoapplicantEmployer = "CoapplicantEmployer";		            
		public const string Property_CoapplicantYearsEmployed = "CoapplicantYearsEmployed";		            
		public const string Property_CoapplicantMonthsEmployed = "CoapplicantMonthsEmployed";		            
		public const string Property_CoapplicantEmployerPhone = "CoapplicantEmployerPhone";		            
		public const string Property_CoapplicantPosition = "CoapplicantPosition";		            
		public const string Property_CoapplicantGrossAnnualIncome = "CoapplicantGrossAnnualIncome";		            
		public const string Property_CoapplicantOtherIncome = "CoapplicantOtherIncome";		            
		public const string Property_CoapplicantOtherIncomeSource = "CoapplicantOtherIncomeSource";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CustomerId;	            
		private Nullable<Int32> _MerchantId;	            
		private Nullable<Double> _AmountRequested;	            
		private String _OptionCode;	            
		private Nullable<Boolean> _ProdSecuritySystem;	            
		private String _ProdMiscDescription;	            
		private String _ApplicantLastName;	            
		private String _ApplicantFirstName;	            
		private String _ApplicantMiddleName;	            
		private String _ApplicantNameSuffix;	            
		private Nullable<DateTime> _ApplicantDOB;	            
		private String _ApplicantSSN;	            
		private String _ApplicantHomePhone;	            
		private String _ApplicantCellPhone;	            
		private String _ApplicantEmailAddress;	            
		private String _ApplicantDriversLicense;	            
		private String _ApplicantStreet;	            
		private String _ApplicantCity;	            
		private String _ApplicantState;	            
		private String _ApplicantZipCode;	            
		private String _ApplicantCountry;	            
		private String _ApplicantYearsAtAddress;	            
		private String _ApplicantMonthsAtAddress;	            
		private Nullable<Double> _MortgagePayment;	            
		private String _MortgageHolder;	            
		private Nullable<Double> _MortgageBalance;	            
		private String _BankName;	            
		private String _BankAcctType;	            
		private String _ApplicantPrevStreet;	            
		private String _ApplicantPrevCity;	            
		private String _ApplicantPrevState;	            
		private String _ApplicantPrevZipCode;	            
		private String _ApplicantPrevCountry;	            
		private String _ApplicantPrevYearsAtAddress;	            
		private String _ApplicantPrevMonthsAtAddress;	            
		private String _ApplicantEmployer;	            
		private String _ApplicantYearsEmployed;	            
		private String _ApplicantMonthsEmployed;	            
		private String _ApplicantEmployerPhone;	            
		private String _ApplicantPosition;	            
		private Nullable<Double> _ApplicantGrossAnnualIncome;	            
		private Nullable<Double> _ApplicantOtherIncome;	            
		private String _ApplicantOtherIncomeSource;	            
		private String _SelectedAssignedUserId;	            
		private String _CoapplicantLastName;	            
		private String _CoapplicantFirstName;	            
		private String _CoapplicantMiddleName;	            
		private String _CoapplicantNameSuffix;	            
		private Nullable<DateTime> _CoapplicantDOB;	            
		private String _CoapplicantSSN;	            
		private String _CoapplicantHomePhone;	            
		private String _CpapplicantCellPhone;	            
		private String _CoapplicantDriversLicense;	            
		private String _CoapplicantEmailAddress;	            
		private String _CoapplicantStreet;	            
		private String _CoapplicantCity;	            
		private String _CoapplicantState;	            
		private String _CoapplicantZipCode;	            
		private String _CoapplicantCountry;	            
		private String _CoapplicantYearsAtAddress;	            
		private String _CoapplicantMonthsAtAddress;	            
		private String _CoapplicantEmployer;	            
		private String _CoapplicantYearsEmployed;	            
		private String _CoapplicantMonthsEmployed;	            
		private String _CoapplicantEmployerPhone;	            
		private String _CoapplicantPosition;	            
		private Nullable<Double> _CoapplicantGrossAnnualIncome;	            
		private Nullable<Double> _CoapplicantOtherIncome;	            
		private String _CoapplicantOtherIncomeSource;	            
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
		public Nullable<Int32> MerchantId
		{	
			get{ return _MerchantId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MerchantId, value, _MerchantId);
				if (PropertyChanging(args))
				{
					_MerchantId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> AmountRequested
		{	
			get{ return _AmountRequested; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AmountRequested, value, _AmountRequested);
				if (PropertyChanging(args))
				{
					_AmountRequested = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String OptionCode
		{	
			get{ return _OptionCode; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OptionCode, value, _OptionCode);
				if (PropertyChanging(args))
				{
					_OptionCode = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> ProdSecuritySystem
		{	
			get{ return _ProdSecuritySystem; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ProdSecuritySystem, value, _ProdSecuritySystem);
				if (PropertyChanging(args))
				{
					_ProdSecuritySystem = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ProdMiscDescription
		{	
			get{ return _ProdMiscDescription; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ProdMiscDescription, value, _ProdMiscDescription);
				if (PropertyChanging(args))
				{
					_ProdMiscDescription = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ApplicantLastName
		{	
			get{ return _ApplicantLastName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ApplicantLastName, value, _ApplicantLastName);
				if (PropertyChanging(args))
				{
					_ApplicantLastName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ApplicantFirstName
		{	
			get{ return _ApplicantFirstName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ApplicantFirstName, value, _ApplicantFirstName);
				if (PropertyChanging(args))
				{
					_ApplicantFirstName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ApplicantMiddleName
		{	
			get{ return _ApplicantMiddleName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ApplicantMiddleName, value, _ApplicantMiddleName);
				if (PropertyChanging(args))
				{
					_ApplicantMiddleName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ApplicantNameSuffix
		{	
			get{ return _ApplicantNameSuffix; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ApplicantNameSuffix, value, _ApplicantNameSuffix);
				if (PropertyChanging(args))
				{
					_ApplicantNameSuffix = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> ApplicantDOB
		{	
			get{ return _ApplicantDOB; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ApplicantDOB, value, _ApplicantDOB);
				if (PropertyChanging(args))
				{
					_ApplicantDOB = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ApplicantSSN
		{	
			get{ return _ApplicantSSN; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ApplicantSSN, value, _ApplicantSSN);
				if (PropertyChanging(args))
				{
					_ApplicantSSN = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ApplicantHomePhone
		{	
			get{ return _ApplicantHomePhone; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ApplicantHomePhone, value, _ApplicantHomePhone);
				if (PropertyChanging(args))
				{
					_ApplicantHomePhone = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ApplicantCellPhone
		{	
			get{ return _ApplicantCellPhone; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ApplicantCellPhone, value, _ApplicantCellPhone);
				if (PropertyChanging(args))
				{
					_ApplicantCellPhone = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ApplicantEmailAddress
		{	
			get{ return _ApplicantEmailAddress; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ApplicantEmailAddress, value, _ApplicantEmailAddress);
				if (PropertyChanging(args))
				{
					_ApplicantEmailAddress = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ApplicantDriversLicense
		{	
			get{ return _ApplicantDriversLicense; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ApplicantDriversLicense, value, _ApplicantDriversLicense);
				if (PropertyChanging(args))
				{
					_ApplicantDriversLicense = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ApplicantStreet
		{	
			get{ return _ApplicantStreet; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ApplicantStreet, value, _ApplicantStreet);
				if (PropertyChanging(args))
				{
					_ApplicantStreet = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ApplicantCity
		{	
			get{ return _ApplicantCity; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ApplicantCity, value, _ApplicantCity);
				if (PropertyChanging(args))
				{
					_ApplicantCity = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ApplicantState
		{	
			get{ return _ApplicantState; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ApplicantState, value, _ApplicantState);
				if (PropertyChanging(args))
				{
					_ApplicantState = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ApplicantZipCode
		{	
			get{ return _ApplicantZipCode; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ApplicantZipCode, value, _ApplicantZipCode);
				if (PropertyChanging(args))
				{
					_ApplicantZipCode = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ApplicantCountry
		{	
			get{ return _ApplicantCountry; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ApplicantCountry, value, _ApplicantCountry);
				if (PropertyChanging(args))
				{
					_ApplicantCountry = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ApplicantYearsAtAddress
		{	
			get{ return _ApplicantYearsAtAddress; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ApplicantYearsAtAddress, value, _ApplicantYearsAtAddress);
				if (PropertyChanging(args))
				{
					_ApplicantYearsAtAddress = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ApplicantMonthsAtAddress
		{	
			get{ return _ApplicantMonthsAtAddress; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ApplicantMonthsAtAddress, value, _ApplicantMonthsAtAddress);
				if (PropertyChanging(args))
				{
					_ApplicantMonthsAtAddress = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> MortgagePayment
		{	
			get{ return _MortgagePayment; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MortgagePayment, value, _MortgagePayment);
				if (PropertyChanging(args))
				{
					_MortgagePayment = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String MortgageHolder
		{	
			get{ return _MortgageHolder; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MortgageHolder, value, _MortgageHolder);
				if (PropertyChanging(args))
				{
					_MortgageHolder = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> MortgageBalance
		{	
			get{ return _MortgageBalance; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MortgageBalance, value, _MortgageBalance);
				if (PropertyChanging(args))
				{
					_MortgageBalance = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BankName
		{	
			get{ return _BankName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BankName, value, _BankName);
				if (PropertyChanging(args))
				{
					_BankName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BankAcctType
		{	
			get{ return _BankAcctType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BankAcctType, value, _BankAcctType);
				if (PropertyChanging(args))
				{
					_BankAcctType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ApplicantPrevStreet
		{	
			get{ return _ApplicantPrevStreet; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ApplicantPrevStreet, value, _ApplicantPrevStreet);
				if (PropertyChanging(args))
				{
					_ApplicantPrevStreet = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ApplicantPrevCity
		{	
			get{ return _ApplicantPrevCity; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ApplicantPrevCity, value, _ApplicantPrevCity);
				if (PropertyChanging(args))
				{
					_ApplicantPrevCity = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ApplicantPrevState
		{	
			get{ return _ApplicantPrevState; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ApplicantPrevState, value, _ApplicantPrevState);
				if (PropertyChanging(args))
				{
					_ApplicantPrevState = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ApplicantPrevZipCode
		{	
			get{ return _ApplicantPrevZipCode; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ApplicantPrevZipCode, value, _ApplicantPrevZipCode);
				if (PropertyChanging(args))
				{
					_ApplicantPrevZipCode = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ApplicantPrevCountry
		{	
			get{ return _ApplicantPrevCountry; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ApplicantPrevCountry, value, _ApplicantPrevCountry);
				if (PropertyChanging(args))
				{
					_ApplicantPrevCountry = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ApplicantPrevYearsAtAddress
		{	
			get{ return _ApplicantPrevYearsAtAddress; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ApplicantPrevYearsAtAddress, value, _ApplicantPrevYearsAtAddress);
				if (PropertyChanging(args))
				{
					_ApplicantPrevYearsAtAddress = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ApplicantPrevMonthsAtAddress
		{	
			get{ return _ApplicantPrevMonthsAtAddress; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ApplicantPrevMonthsAtAddress, value, _ApplicantPrevMonthsAtAddress);
				if (PropertyChanging(args))
				{
					_ApplicantPrevMonthsAtAddress = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ApplicantEmployer
		{	
			get{ return _ApplicantEmployer; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ApplicantEmployer, value, _ApplicantEmployer);
				if (PropertyChanging(args))
				{
					_ApplicantEmployer = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ApplicantYearsEmployed
		{	
			get{ return _ApplicantYearsEmployed; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ApplicantYearsEmployed, value, _ApplicantYearsEmployed);
				if (PropertyChanging(args))
				{
					_ApplicantYearsEmployed = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ApplicantMonthsEmployed
		{	
			get{ return _ApplicantMonthsEmployed; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ApplicantMonthsEmployed, value, _ApplicantMonthsEmployed);
				if (PropertyChanging(args))
				{
					_ApplicantMonthsEmployed = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ApplicantEmployerPhone
		{	
			get{ return _ApplicantEmployerPhone; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ApplicantEmployerPhone, value, _ApplicantEmployerPhone);
				if (PropertyChanging(args))
				{
					_ApplicantEmployerPhone = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ApplicantPosition
		{	
			get{ return _ApplicantPosition; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ApplicantPosition, value, _ApplicantPosition);
				if (PropertyChanging(args))
				{
					_ApplicantPosition = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> ApplicantGrossAnnualIncome
		{	
			get{ return _ApplicantGrossAnnualIncome; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ApplicantGrossAnnualIncome, value, _ApplicantGrossAnnualIncome);
				if (PropertyChanging(args))
				{
					_ApplicantGrossAnnualIncome = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> ApplicantOtherIncome
		{	
			get{ return _ApplicantOtherIncome; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ApplicantOtherIncome, value, _ApplicantOtherIncome);
				if (PropertyChanging(args))
				{
					_ApplicantOtherIncome = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ApplicantOtherIncomeSource
		{	
			get{ return _ApplicantOtherIncomeSource; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ApplicantOtherIncomeSource, value, _ApplicantOtherIncomeSource);
				if (PropertyChanging(args))
				{
					_ApplicantOtherIncomeSource = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SelectedAssignedUserId
		{	
			get{ return _SelectedAssignedUserId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SelectedAssignedUserId, value, _SelectedAssignedUserId);
				if (PropertyChanging(args))
				{
					_SelectedAssignedUserId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoapplicantLastName
		{	
			get{ return _CoapplicantLastName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoapplicantLastName, value, _CoapplicantLastName);
				if (PropertyChanging(args))
				{
					_CoapplicantLastName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoapplicantFirstName
		{	
			get{ return _CoapplicantFirstName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoapplicantFirstName, value, _CoapplicantFirstName);
				if (PropertyChanging(args))
				{
					_CoapplicantFirstName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoapplicantMiddleName
		{	
			get{ return _CoapplicantMiddleName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoapplicantMiddleName, value, _CoapplicantMiddleName);
				if (PropertyChanging(args))
				{
					_CoapplicantMiddleName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoapplicantNameSuffix
		{	
			get{ return _CoapplicantNameSuffix; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoapplicantNameSuffix, value, _CoapplicantNameSuffix);
				if (PropertyChanging(args))
				{
					_CoapplicantNameSuffix = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> CoapplicantDOB
		{	
			get{ return _CoapplicantDOB; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoapplicantDOB, value, _CoapplicantDOB);
				if (PropertyChanging(args))
				{
					_CoapplicantDOB = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoapplicantSSN
		{	
			get{ return _CoapplicantSSN; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoapplicantSSN, value, _CoapplicantSSN);
				if (PropertyChanging(args))
				{
					_CoapplicantSSN = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoapplicantHomePhone
		{	
			get{ return _CoapplicantHomePhone; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoapplicantHomePhone, value, _CoapplicantHomePhone);
				if (PropertyChanging(args))
				{
					_CoapplicantHomePhone = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CpapplicantCellPhone
		{	
			get{ return _CpapplicantCellPhone; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CpapplicantCellPhone, value, _CpapplicantCellPhone);
				if (PropertyChanging(args))
				{
					_CpapplicantCellPhone = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoapplicantDriversLicense
		{	
			get{ return _CoapplicantDriversLicense; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoapplicantDriversLicense, value, _CoapplicantDriversLicense);
				if (PropertyChanging(args))
				{
					_CoapplicantDriversLicense = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoapplicantEmailAddress
		{	
			get{ return _CoapplicantEmailAddress; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoapplicantEmailAddress, value, _CoapplicantEmailAddress);
				if (PropertyChanging(args))
				{
					_CoapplicantEmailAddress = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoapplicantStreet
		{	
			get{ return _CoapplicantStreet; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoapplicantStreet, value, _CoapplicantStreet);
				if (PropertyChanging(args))
				{
					_CoapplicantStreet = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoapplicantCity
		{	
			get{ return _CoapplicantCity; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoapplicantCity, value, _CoapplicantCity);
				if (PropertyChanging(args))
				{
					_CoapplicantCity = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoapplicantState
		{	
			get{ return _CoapplicantState; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoapplicantState, value, _CoapplicantState);
				if (PropertyChanging(args))
				{
					_CoapplicantState = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoapplicantZipCode
		{	
			get{ return _CoapplicantZipCode; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoapplicantZipCode, value, _CoapplicantZipCode);
				if (PropertyChanging(args))
				{
					_CoapplicantZipCode = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoapplicantCountry
		{	
			get{ return _CoapplicantCountry; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoapplicantCountry, value, _CoapplicantCountry);
				if (PropertyChanging(args))
				{
					_CoapplicantCountry = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoapplicantYearsAtAddress
		{	
			get{ return _CoapplicantYearsAtAddress; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoapplicantYearsAtAddress, value, _CoapplicantYearsAtAddress);
				if (PropertyChanging(args))
				{
					_CoapplicantYearsAtAddress = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoapplicantMonthsAtAddress
		{	
			get{ return _CoapplicantMonthsAtAddress; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoapplicantMonthsAtAddress, value, _CoapplicantMonthsAtAddress);
				if (PropertyChanging(args))
				{
					_CoapplicantMonthsAtAddress = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoapplicantEmployer
		{	
			get{ return _CoapplicantEmployer; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoapplicantEmployer, value, _CoapplicantEmployer);
				if (PropertyChanging(args))
				{
					_CoapplicantEmployer = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoapplicantYearsEmployed
		{	
			get{ return _CoapplicantYearsEmployed; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoapplicantYearsEmployed, value, _CoapplicantYearsEmployed);
				if (PropertyChanging(args))
				{
					_CoapplicantYearsEmployed = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoapplicantMonthsEmployed
		{	
			get{ return _CoapplicantMonthsEmployed; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoapplicantMonthsEmployed, value, _CoapplicantMonthsEmployed);
				if (PropertyChanging(args))
				{
					_CoapplicantMonthsEmployed = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoapplicantEmployerPhone
		{	
			get{ return _CoapplicantEmployerPhone; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoapplicantEmployerPhone, value, _CoapplicantEmployerPhone);
				if (PropertyChanging(args))
				{
					_CoapplicantEmployerPhone = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoapplicantPosition
		{	
			get{ return _CoapplicantPosition; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoapplicantPosition, value, _CoapplicantPosition);
				if (PropertyChanging(args))
				{
					_CoapplicantPosition = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> CoapplicantGrossAnnualIncome
		{	
			get{ return _CoapplicantGrossAnnualIncome; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoapplicantGrossAnnualIncome, value, _CoapplicantGrossAnnualIncome);
				if (PropertyChanging(args))
				{
					_CoapplicantGrossAnnualIncome = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> CoapplicantOtherIncome
		{	
			get{ return _CoapplicantOtherIncome; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoapplicantOtherIncome, value, _CoapplicantOtherIncome);
				if (PropertyChanging(args))
				{
					_CoapplicantOtherIncome = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CoapplicantOtherIncomeSource
		{	
			get{ return _CoapplicantOtherIncomeSource; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CoapplicantOtherIncomeSource, value, _CoapplicantOtherIncomeSource);
				if (PropertyChanging(args))
				{
					_CoapplicantOtherIncomeSource = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  CustomerIsPcCreditApplicationBase Clone()
		{
			CustomerIsPcCreditApplicationBase newObj = new  CustomerIsPcCreditApplicationBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CustomerId = this.CustomerId;						
			newObj.MerchantId = this.MerchantId;						
			newObj.AmountRequested = this.AmountRequested;						
			newObj.OptionCode = this.OptionCode;						
			newObj.ProdSecuritySystem = this.ProdSecuritySystem;						
			newObj.ProdMiscDescription = this.ProdMiscDescription;						
			newObj.ApplicantLastName = this.ApplicantLastName;						
			newObj.ApplicantFirstName = this.ApplicantFirstName;						
			newObj.ApplicantMiddleName = this.ApplicantMiddleName;						
			newObj.ApplicantNameSuffix = this.ApplicantNameSuffix;						
			newObj.ApplicantDOB = this.ApplicantDOB;						
			newObj.ApplicantSSN = this.ApplicantSSN;						
			newObj.ApplicantHomePhone = this.ApplicantHomePhone;						
			newObj.ApplicantCellPhone = this.ApplicantCellPhone;						
			newObj.ApplicantEmailAddress = this.ApplicantEmailAddress;						
			newObj.ApplicantDriversLicense = this.ApplicantDriversLicense;						
			newObj.ApplicantStreet = this.ApplicantStreet;						
			newObj.ApplicantCity = this.ApplicantCity;						
			newObj.ApplicantState = this.ApplicantState;						
			newObj.ApplicantZipCode = this.ApplicantZipCode;						
			newObj.ApplicantCountry = this.ApplicantCountry;						
			newObj.ApplicantYearsAtAddress = this.ApplicantYearsAtAddress;						
			newObj.ApplicantMonthsAtAddress = this.ApplicantMonthsAtAddress;						
			newObj.MortgagePayment = this.MortgagePayment;						
			newObj.MortgageHolder = this.MortgageHolder;						
			newObj.MortgageBalance = this.MortgageBalance;						
			newObj.BankName = this.BankName;						
			newObj.BankAcctType = this.BankAcctType;						
			newObj.ApplicantPrevStreet = this.ApplicantPrevStreet;						
			newObj.ApplicantPrevCity = this.ApplicantPrevCity;						
			newObj.ApplicantPrevState = this.ApplicantPrevState;						
			newObj.ApplicantPrevZipCode = this.ApplicantPrevZipCode;						
			newObj.ApplicantPrevCountry = this.ApplicantPrevCountry;						
			newObj.ApplicantPrevYearsAtAddress = this.ApplicantPrevYearsAtAddress;						
			newObj.ApplicantPrevMonthsAtAddress = this.ApplicantPrevMonthsAtAddress;						
			newObj.ApplicantEmployer = this.ApplicantEmployer;						
			newObj.ApplicantYearsEmployed = this.ApplicantYearsEmployed;						
			newObj.ApplicantMonthsEmployed = this.ApplicantMonthsEmployed;						
			newObj.ApplicantEmployerPhone = this.ApplicantEmployerPhone;						
			newObj.ApplicantPosition = this.ApplicantPosition;						
			newObj.ApplicantGrossAnnualIncome = this.ApplicantGrossAnnualIncome;						
			newObj.ApplicantOtherIncome = this.ApplicantOtherIncome;						
			newObj.ApplicantOtherIncomeSource = this.ApplicantOtherIncomeSource;						
			newObj.SelectedAssignedUserId = this.SelectedAssignedUserId;						
			newObj.CoapplicantLastName = this.CoapplicantLastName;						
			newObj.CoapplicantFirstName = this.CoapplicantFirstName;						
			newObj.CoapplicantMiddleName = this.CoapplicantMiddleName;						
			newObj.CoapplicantNameSuffix = this.CoapplicantNameSuffix;						
			newObj.CoapplicantDOB = this.CoapplicantDOB;						
			newObj.CoapplicantSSN = this.CoapplicantSSN;						
			newObj.CoapplicantHomePhone = this.CoapplicantHomePhone;						
			newObj.CpapplicantCellPhone = this.CpapplicantCellPhone;						
			newObj.CoapplicantDriversLicense = this.CoapplicantDriversLicense;						
			newObj.CoapplicantEmailAddress = this.CoapplicantEmailAddress;						
			newObj.CoapplicantStreet = this.CoapplicantStreet;						
			newObj.CoapplicantCity = this.CoapplicantCity;						
			newObj.CoapplicantState = this.CoapplicantState;						
			newObj.CoapplicantZipCode = this.CoapplicantZipCode;						
			newObj.CoapplicantCountry = this.CoapplicantCountry;						
			newObj.CoapplicantYearsAtAddress = this.CoapplicantYearsAtAddress;						
			newObj.CoapplicantMonthsAtAddress = this.CoapplicantMonthsAtAddress;						
			newObj.CoapplicantEmployer = this.CoapplicantEmployer;						
			newObj.CoapplicantYearsEmployed = this.CoapplicantYearsEmployed;						
			newObj.CoapplicantMonthsEmployed = this.CoapplicantMonthsEmployed;						
			newObj.CoapplicantEmployerPhone = this.CoapplicantEmployerPhone;						
			newObj.CoapplicantPosition = this.CoapplicantPosition;						
			newObj.CoapplicantGrossAnnualIncome = this.CoapplicantGrossAnnualIncome;						
			newObj.CoapplicantOtherIncome = this.CoapplicantOtherIncome;						
			newObj.CoapplicantOtherIncomeSource = this.CoapplicantOtherIncomeSource;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_Id, Id);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_CustomerId, CustomerId);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_MerchantId, MerchantId);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_AmountRequested, AmountRequested);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_OptionCode, OptionCode);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_ProdSecuritySystem, ProdSecuritySystem);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_ProdMiscDescription, ProdMiscDescription);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_ApplicantLastName, ApplicantLastName);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_ApplicantFirstName, ApplicantFirstName);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_ApplicantMiddleName, ApplicantMiddleName);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_ApplicantNameSuffix, ApplicantNameSuffix);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_ApplicantDOB, ApplicantDOB);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_ApplicantSSN, ApplicantSSN);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_ApplicantHomePhone, ApplicantHomePhone);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_ApplicantCellPhone, ApplicantCellPhone);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_ApplicantEmailAddress, ApplicantEmailAddress);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_ApplicantDriversLicense, ApplicantDriversLicense);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_ApplicantStreet, ApplicantStreet);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_ApplicantCity, ApplicantCity);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_ApplicantState, ApplicantState);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_ApplicantZipCode, ApplicantZipCode);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_ApplicantCountry, ApplicantCountry);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_ApplicantYearsAtAddress, ApplicantYearsAtAddress);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_ApplicantMonthsAtAddress, ApplicantMonthsAtAddress);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_MortgagePayment, MortgagePayment);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_MortgageHolder, MortgageHolder);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_MortgageBalance, MortgageBalance);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_BankName, BankName);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_BankAcctType, BankAcctType);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_ApplicantPrevStreet, ApplicantPrevStreet);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_ApplicantPrevCity, ApplicantPrevCity);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_ApplicantPrevState, ApplicantPrevState);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_ApplicantPrevZipCode, ApplicantPrevZipCode);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_ApplicantPrevCountry, ApplicantPrevCountry);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_ApplicantPrevYearsAtAddress, ApplicantPrevYearsAtAddress);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_ApplicantPrevMonthsAtAddress, ApplicantPrevMonthsAtAddress);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_ApplicantEmployer, ApplicantEmployer);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_ApplicantYearsEmployed, ApplicantYearsEmployed);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_ApplicantMonthsEmployed, ApplicantMonthsEmployed);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_ApplicantEmployerPhone, ApplicantEmployerPhone);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_ApplicantPosition, ApplicantPosition);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_ApplicantGrossAnnualIncome, ApplicantGrossAnnualIncome);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_ApplicantOtherIncome, ApplicantOtherIncome);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_ApplicantOtherIncomeSource, ApplicantOtherIncomeSource);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_SelectedAssignedUserId, SelectedAssignedUserId);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_CoapplicantLastName, CoapplicantLastName);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_CoapplicantFirstName, CoapplicantFirstName);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_CoapplicantMiddleName, CoapplicantMiddleName);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_CoapplicantNameSuffix, CoapplicantNameSuffix);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_CoapplicantDOB, CoapplicantDOB);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_CoapplicantSSN, CoapplicantSSN);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_CoapplicantHomePhone, CoapplicantHomePhone);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_CpapplicantCellPhone, CpapplicantCellPhone);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_CoapplicantDriversLicense, CoapplicantDriversLicense);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_CoapplicantEmailAddress, CoapplicantEmailAddress);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_CoapplicantStreet, CoapplicantStreet);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_CoapplicantCity, CoapplicantCity);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_CoapplicantState, CoapplicantState);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_CoapplicantZipCode, CoapplicantZipCode);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_CoapplicantCountry, CoapplicantCountry);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_CoapplicantYearsAtAddress, CoapplicantYearsAtAddress);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_CoapplicantMonthsAtAddress, CoapplicantMonthsAtAddress);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_CoapplicantEmployer, CoapplicantEmployer);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_CoapplicantYearsEmployed, CoapplicantYearsEmployed);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_CoapplicantMonthsEmployed, CoapplicantMonthsEmployed);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_CoapplicantEmployerPhone, CoapplicantEmployerPhone);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_CoapplicantPosition, CoapplicantPosition);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_CoapplicantGrossAnnualIncome, CoapplicantGrossAnnualIncome);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_CoapplicantOtherIncome, CoapplicantOtherIncome);				
			info.AddValue(CustomerIsPcCreditApplicationBase.Property_CoapplicantOtherIncomeSource, CoapplicantOtherIncomeSource);				
		}
		#endregion

		
	}
}
