using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomerExtendedBase", Namespace = "http://www.hims-tech.com//entities")]
	public class CustomerExtendedBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CustomerId = 1,
			Takeover = 2,
			PreWired = 3,
			HardWired = 4,
			CSAgreement = 5,
			SalesPerson4 = 6,
			FinanceCompany = 7,
			ContractStartDate = 8,
			RemainingContractTerm = 9,
			IsFinanced = 10,
			Pets = 11,
			PetsType = 12,
			Repair = 13,
			RepairType = 14,
			BirthDateCoupon = 15,
			VipClubMember = 16,
			RWST1 = 17,
			RWST2 = 18,
			RWST3 = 19,
			RWST4 = 20,
			RWST5 = 21,
			RWST6 = 22,
			RWST7 = 23,
			RWST8 = 24,
			RWST9 = 25,
			RWST10 = 26,
			RWST11 = 27,
			RWST12 = 28,
			RWST13 = 29,
			RWST14 = 30,
			RWST15 = 31,
			RepsAssignedDate = 32,
			ContractSentBy = 33,
			SecondaryFirstName = 34,
			SecondaryLastName = 35,
			SecondarySSN = 36,
			SecondaryBirthDate = 37,
			SecondaryEmail = 38,
			FundingResult = 39,
			GrossFundedAmount = 40,
			NetFundedAmount = 41,
			DiscountFundedAmount = 42,
			DiscountFundedPercentage = 43,
			CustomerPaymentAmount = 44,
			FinanceRepCommissionRate = 45,
			LoanNumber = 46,
			CreditAppNumber = 47,
			Term = 48,
			APR = 49,
			MaxCreditLimit = 50,
			ApprovalDate = 51,
			MonthlyFinanceRate = 52,
			Batch = 53,
			FinanceRep = 54,
			CreditTransectionId = 55,
			BounceMatchId = 56,
			BounceStatus = 57,
			InstallFinishDate = 58,
			PromotionMonth = 59,
			PrepaidMonth = 60,
			PaymentEffectiveDate = 61,
			FacebookProfileUrl = 62,
			GoogleProfileUrl = 63,
			FacebookName = 64,
			GoogleName = 65,
			LeadVersion = 66,
			AppoinmentSetBy = 67,
			IsPcApplicationId = 68,
			IsPcCreditApproved = 69,
			BrinksCancelDate = 70,
			ReceivedDate = 71,
			BrinksFundingDate = 72,
			PlanCode = 73,
			NewMMR = 74,
			LoanAmount = 75,
			Payout = 76,
			FinanceFundingDate = 77,
			BrinksFundingStatus = 78,
			FinanceFundingStatus = 79,
			IsPcAppStatus = 80,
			AlarmBasicPackage = 81,
			NMCRefId = 82,
			CustomerSince = 83,
			ResignDate = 84,
			MonthlyBatch = 85,
			IsAgreementSMSSend = 86,
			UnlinkCustomer = 87,
			GeeseLead = 88,
			PowerPayAppId = 89,
			PowerPayAppStatus = 90,
			GeeseCount = 91,
			AvantgradRefId = 92,
			DrivingLicense = 93,
			CreatedDay = 94,
			ResignedBy = 95,
			ContractType = 96,
			IsSignAgrSendToCus = 97,
			IsTestAccount = 98,
			DealerFee = 99,
			ContractCreatedDate = 100,
			Warranty = 101,
			Keypad = 102,
			FrontEnd = 103,
			CycleStartDate = 104
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_Takeover = "Takeover";		            
		public const string Property_PreWired = "PreWired";		            
		public const string Property_HardWired = "HardWired";		            
		public const string Property_CSAgreement = "CSAgreement";		            
		public const string Property_SalesPerson4 = "SalesPerson4";		            
		public const string Property_FinanceCompany = "FinanceCompany";		            
		public const string Property_ContractStartDate = "ContractStartDate";		            
		public const string Property_RemainingContractTerm = "RemainingContractTerm";		            
		public const string Property_IsFinanced = "IsFinanced";		            
		public const string Property_Pets = "Pets";		            
		public const string Property_PetsType = "PetsType";		            
		public const string Property_Repair = "Repair";		            
		public const string Property_RepairType = "RepairType";		            
		public const string Property_BirthDateCoupon = "BirthDateCoupon";		            
		public const string Property_VipClubMember = "VipClubMember";		            
		public const string Property_RWST1 = "RWST1";		            
		public const string Property_RWST2 = "RWST2";		            
		public const string Property_RWST3 = "RWST3";		            
		public const string Property_RWST4 = "RWST4";		            
		public const string Property_RWST5 = "RWST5";		            
		public const string Property_RWST6 = "RWST6";		            
		public const string Property_RWST7 = "RWST7";		            
		public const string Property_RWST8 = "RWST8";		            
		public const string Property_RWST9 = "RWST9";		            
		public const string Property_RWST10 = "RWST10";		            
		public const string Property_RWST11 = "RWST11";		            
		public const string Property_RWST12 = "RWST12";		            
		public const string Property_RWST13 = "RWST13";		            
		public const string Property_RWST14 = "RWST14";		            
		public const string Property_RWST15 = "RWST15";		            
		public const string Property_RepsAssignedDate = "RepsAssignedDate";		            
		public const string Property_ContractSentBy = "ContractSentBy";		            
		public const string Property_SecondaryFirstName = "SecondaryFirstName";		            
		public const string Property_SecondaryLastName = "SecondaryLastName";		            
		public const string Property_SecondarySSN = "SecondarySSN";		            
		public const string Property_SecondaryBirthDate = "SecondaryBirthDate";		            
		public const string Property_SecondaryEmail = "SecondaryEmail";		            
		public const string Property_FundingResult = "FundingResult";		            
		public const string Property_GrossFundedAmount = "GrossFundedAmount";		            
		public const string Property_NetFundedAmount = "NetFundedAmount";		            
		public const string Property_DiscountFundedAmount = "DiscountFundedAmount";		            
		public const string Property_DiscountFundedPercentage = "DiscountFundedPercentage";		            
		public const string Property_CustomerPaymentAmount = "CustomerPaymentAmount";		            
		public const string Property_FinanceRepCommissionRate = "FinanceRepCommissionRate";		            
		public const string Property_LoanNumber = "LoanNumber";		            
		public const string Property_CreditAppNumber = "CreditAppNumber";		            
		public const string Property_Term = "Term";		            
		public const string Property_APR = "APR";		            
		public const string Property_MaxCreditLimit = "MaxCreditLimit";		            
		public const string Property_ApprovalDate = "ApprovalDate";		            
		public const string Property_MonthlyFinanceRate = "MonthlyFinanceRate";		            
		public const string Property_Batch = "Batch";		            
		public const string Property_FinanceRep = "FinanceRep";		            
		public const string Property_CreditTransectionId = "CreditTransectionId";		            
		public const string Property_BounceMatchId = "BounceMatchId";		            
		public const string Property_BounceStatus = "BounceStatus";		            
		public const string Property_InstallFinishDate = "InstallFinishDate";		            
		public const string Property_PromotionMonth = "PromotionMonth";		            
		public const string Property_PrepaidMonth = "PrepaidMonth";		            
		public const string Property_PaymentEffectiveDate = "PaymentEffectiveDate";		            
		public const string Property_FacebookProfileUrl = "FacebookProfileUrl";		            
		public const string Property_GoogleProfileUrl = "GoogleProfileUrl";		            
		public const string Property_FacebookName = "FacebookName";		            
		public const string Property_GoogleName = "GoogleName";		            
		public const string Property_LeadVersion = "LeadVersion";		            
		public const string Property_AppoinmentSetBy = "AppoinmentSetBy";		            
		public const string Property_IsPcApplicationId = "IsPcApplicationId";		            
		public const string Property_IsPcCreditApproved = "IsPcCreditApproved";		            
		public const string Property_BrinksCancelDate = "BrinksCancelDate";		            
		public const string Property_ReceivedDate = "ReceivedDate";		            
		public const string Property_BrinksFundingDate = "BrinksFundingDate";		            
		public const string Property_PlanCode = "PlanCode";		            
		public const string Property_NewMMR = "NewMMR";		            
		public const string Property_LoanAmount = "LoanAmount";		            
		public const string Property_Payout = "Payout";		            
		public const string Property_FinanceFundingDate = "FinanceFundingDate";		            
		public const string Property_BrinksFundingStatus = "BrinksFundingStatus";		            
		public const string Property_FinanceFundingStatus = "FinanceFundingStatus";		            
		public const string Property_IsPcAppStatus = "IsPcAppStatus";		            
		public const string Property_AlarmBasicPackage = "AlarmBasicPackage";		            
		public const string Property_NMCRefId = "NMCRefId";		            
		public const string Property_CustomerSince = "CustomerSince";		            
		public const string Property_ResignDate = "ResignDate";		            
		public const string Property_MonthlyBatch = "MonthlyBatch";		            
		public const string Property_IsAgreementSMSSend = "IsAgreementSMSSend";		            
		public const string Property_UnlinkCustomer = "UnlinkCustomer";		            
		public const string Property_GeeseLead = "GeeseLead";		            
		public const string Property_PowerPayAppId = "PowerPayAppId";		            
		public const string Property_PowerPayAppStatus = "PowerPayAppStatus";		            
		public const string Property_GeeseCount = "GeeseCount";		            
		public const string Property_AvantgradRefId = "AvantgradRefId";		            
		public const string Property_DrivingLicense = "DrivingLicense";		            
		public const string Property_CreatedDay = "CreatedDay";		            
		public const string Property_ResignedBy = "ResignedBy";		            
		public const string Property_ContractType = "ContractType";		            
		public const string Property_IsSignAgrSendToCus = "IsSignAgrSendToCus";		            
		public const string Property_IsTestAccount = "IsTestAccount";		            
		public const string Property_DealerFee = "DealerFee";		            
		public const string Property_ContractCreatedDate = "ContractCreatedDate";		            
		public const string Property_Warranty = "Warranty";		            
		public const string Property_Keypad = "Keypad";		            
		public const string Property_FrontEnd = "FrontEnd";		            
		public const string Property_CycleStartDate = "CycleStartDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CustomerId;	            
		private Nullable<Boolean> _Takeover;	            
		private Nullable<Boolean> _PreWired;	            
		private Nullable<Boolean> _HardWired;	            
		private String _CSAgreement;	            
		private Guid _SalesPerson4;	            
		private String _FinanceCompany;	            
		private Nullable<DateTime> _ContractStartDate;	            
		private String _RemainingContractTerm;	            
		private Nullable<Boolean> _IsFinanced;	            
		private String _Pets;	            
		private String _PetsType;	            
		private String _Repair;	            
		private String _RepairType;	            
		private String _BirthDateCoupon;	            
		private String _VipClubMember;	            
		private String _RWST1;	            
		private String _RWST2;	            
		private String _RWST3;	            
		private String _RWST4;	            
		private String _RWST5;	            
		private String _RWST6;	            
		private String _RWST7;	            
		private String _RWST8;	            
		private String _RWST9;	            
		private String _RWST10;	            
		private String _RWST11;	            
		private String _RWST12;	            
		private String _RWST13;	            
		private String _RWST14;	            
		private String _RWST15;	            
		private Nullable<DateTime> _RepsAssignedDate;	            
		private Guid _ContractSentBy;	            
		private String _SecondaryFirstName;	            
		private String _SecondaryLastName;	            
		private String _SecondarySSN;	            
		private Nullable<DateTime> _SecondaryBirthDate;	            
		private String _SecondaryEmail;	            
		private Nullable<Boolean> _FundingResult;	            
		private Nullable<Double> _GrossFundedAmount;	            
		private Nullable<Double> _NetFundedAmount;	            
		private Nullable<Double> _DiscountFundedAmount;	            
		private Nullable<Double> _DiscountFundedPercentage;	            
		private Nullable<Double> _CustomerPaymentAmount;	            
		private Nullable<Double> _FinanceRepCommissionRate;	            
		private String _LoanNumber;	            
		private String _CreditAppNumber;	            
		private String _Term;	            
		private String _APR;	            
		private Nullable<Double> _MaxCreditLimit;	            
		private Nullable<DateTime> _ApprovalDate;	            
		private Nullable<Double> _MonthlyFinanceRate;	            
		private String _Batch;	            
		private Guid _FinanceRep;	            
		private String _CreditTransectionId;	            
		private Nullable<Int32> _BounceMatchId;	            
		private String _BounceStatus;	            
		private Nullable<DateTime> _InstallFinishDate;	            
		private Nullable<Int32> _PromotionMonth;	            
		private Nullable<Int32> _PrepaidMonth;	            
		private Nullable<DateTime> _PaymentEffectiveDate;	            
		private String _FacebookProfileUrl;	            
		private String _GoogleProfileUrl;	            
		private String _FacebookName;	            
		private String _GoogleName;	            
		private String _LeadVersion;	            
		private Guid _AppoinmentSetBy;	            
		private String _IsPcApplicationId;	            
		private Nullable<Boolean> _IsPcCreditApproved;	            
		private Nullable<DateTime> _BrinksCancelDate;	            
		private Nullable<DateTime> _ReceivedDate;	            
		private Nullable<DateTime> _BrinksFundingDate;	            
		private String _PlanCode;	            
		private String _NewMMR;	            
		private String _LoanAmount;	            
		private String _Payout;	            
		private Nullable<DateTime> _FinanceFundingDate;	            
		private String _BrinksFundingStatus;	            
		private String _FinanceFundingStatus;	            
		private String _IsPcAppStatus;	            
		private String _AlarmBasicPackage;	            
		private String _NMCRefId;	            
		private Nullable<DateTime> _CustomerSince;	            
		private Nullable<DateTime> _ResignDate;	            
		private Nullable<Int32> _MonthlyBatch;	            
		private Nullable<Boolean> _IsAgreementSMSSend;	            
		private Nullable<Boolean> _UnlinkCustomer;	            
		private String _GeeseLead;	            
		private String _PowerPayAppId;	            
		private String _PowerPayAppStatus;	            
		private Nullable<Int32> _GeeseCount;	            
		private String _AvantgradRefId;	            
		private String _DrivingLicense;	            
		private Nullable<DateTime> _CreatedDay;	            
		private Guid _ResignedBy;	            
		private String _ContractType;	            
		private Nullable<Boolean> _IsSignAgrSendToCus;	            
		private Nullable<Boolean> _IsTestAccount;	            
		private Nullable<Decimal> _DealerFee;	            
		private Nullable<DateTime> _ContractCreatedDate;	            
		private String _Warranty;	            
		private String _Keypad;	            
		private String _FrontEnd;	            
		private Nullable<DateTime> _CycleStartDate;	            
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
		public Nullable<Boolean> Takeover
		{	
			get{ return _Takeover; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Takeover, value, _Takeover);
				if (PropertyChanging(args))
				{
					_Takeover = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> PreWired
		{	
			get{ return _PreWired; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PreWired, value, _PreWired);
				if (PropertyChanging(args))
				{
					_PreWired = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> HardWired
		{	
			get{ return _HardWired; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_HardWired, value, _HardWired);
				if (PropertyChanging(args))
				{
					_HardWired = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CSAgreement
		{	
			get{ return _CSAgreement; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CSAgreement, value, _CSAgreement);
				if (PropertyChanging(args))
				{
					_CSAgreement = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid SalesPerson4
		{	
			get{ return _SalesPerson4; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SalesPerson4, value, _SalesPerson4);
				if (PropertyChanging(args))
				{
					_SalesPerson4 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String FinanceCompany
		{	
			get{ return _FinanceCompany; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FinanceCompany, value, _FinanceCompany);
				if (PropertyChanging(args))
				{
					_FinanceCompany = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> ContractStartDate
		{	
			get{ return _ContractStartDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ContractStartDate, value, _ContractStartDate);
				if (PropertyChanging(args))
				{
					_ContractStartDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String RemainingContractTerm
		{	
			get{ return _RemainingContractTerm; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RemainingContractTerm, value, _RemainingContractTerm);
				if (PropertyChanging(args))
				{
					_RemainingContractTerm = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsFinanced
		{	
			get{ return _IsFinanced; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsFinanced, value, _IsFinanced);
				if (PropertyChanging(args))
				{
					_IsFinanced = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Pets
		{	
			get{ return _Pets; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Pets, value, _Pets);
				if (PropertyChanging(args))
				{
					_Pets = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PetsType
		{	
			get{ return _PetsType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PetsType, value, _PetsType);
				if (PropertyChanging(args))
				{
					_PetsType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Repair
		{	
			get{ return _Repair; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Repair, value, _Repair);
				if (PropertyChanging(args))
				{
					_Repair = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String RepairType
		{	
			get{ return _RepairType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RepairType, value, _RepairType);
				if (PropertyChanging(args))
				{
					_RepairType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BirthDateCoupon
		{	
			get{ return _BirthDateCoupon; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BirthDateCoupon, value, _BirthDateCoupon);
				if (PropertyChanging(args))
				{
					_BirthDateCoupon = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String VipClubMember
		{	
			get{ return _VipClubMember; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_VipClubMember, value, _VipClubMember);
				if (PropertyChanging(args))
				{
					_VipClubMember = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String RWST1
		{	
			get{ return _RWST1; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RWST1, value, _RWST1);
				if (PropertyChanging(args))
				{
					_RWST1 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String RWST2
		{	
			get{ return _RWST2; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RWST2, value, _RWST2);
				if (PropertyChanging(args))
				{
					_RWST2 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String RWST3
		{	
			get{ return _RWST3; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RWST3, value, _RWST3);
				if (PropertyChanging(args))
				{
					_RWST3 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String RWST4
		{	
			get{ return _RWST4; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RWST4, value, _RWST4);
				if (PropertyChanging(args))
				{
					_RWST4 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String RWST5
		{	
			get{ return _RWST5; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RWST5, value, _RWST5);
				if (PropertyChanging(args))
				{
					_RWST5 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String RWST6
		{	
			get{ return _RWST6; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RWST6, value, _RWST6);
				if (PropertyChanging(args))
				{
					_RWST6 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String RWST7
		{	
			get{ return _RWST7; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RWST7, value, _RWST7);
				if (PropertyChanging(args))
				{
					_RWST7 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String RWST8
		{	
			get{ return _RWST8; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RWST8, value, _RWST8);
				if (PropertyChanging(args))
				{
					_RWST8 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String RWST9
		{	
			get{ return _RWST9; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RWST9, value, _RWST9);
				if (PropertyChanging(args))
				{
					_RWST9 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String RWST10
		{	
			get{ return _RWST10; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RWST10, value, _RWST10);
				if (PropertyChanging(args))
				{
					_RWST10 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String RWST11
		{	
			get{ return _RWST11; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RWST11, value, _RWST11);
				if (PropertyChanging(args))
				{
					_RWST11 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String RWST12
		{	
			get{ return _RWST12; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RWST12, value, _RWST12);
				if (PropertyChanging(args))
				{
					_RWST12 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String RWST13
		{	
			get{ return _RWST13; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RWST13, value, _RWST13);
				if (PropertyChanging(args))
				{
					_RWST13 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String RWST14
		{	
			get{ return _RWST14; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RWST14, value, _RWST14);
				if (PropertyChanging(args))
				{
					_RWST14 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String RWST15
		{	
			get{ return _RWST15; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RWST15, value, _RWST15);
				if (PropertyChanging(args))
				{
					_RWST15 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> RepsAssignedDate
		{	
			get{ return _RepsAssignedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RepsAssignedDate, value, _RepsAssignedDate);
				if (PropertyChanging(args))
				{
					_RepsAssignedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid ContractSentBy
		{	
			get{ return _ContractSentBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ContractSentBy, value, _ContractSentBy);
				if (PropertyChanging(args))
				{
					_ContractSentBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SecondaryFirstName
		{	
			get{ return _SecondaryFirstName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SecondaryFirstName, value, _SecondaryFirstName);
				if (PropertyChanging(args))
				{
					_SecondaryFirstName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SecondaryLastName
		{	
			get{ return _SecondaryLastName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SecondaryLastName, value, _SecondaryLastName);
				if (PropertyChanging(args))
				{
					_SecondaryLastName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SecondarySSN
		{	
			get{ return _SecondarySSN; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SecondarySSN, value, _SecondarySSN);
				if (PropertyChanging(args))
				{
					_SecondarySSN = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> SecondaryBirthDate
		{	
			get{ return _SecondaryBirthDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SecondaryBirthDate, value, _SecondaryBirthDate);
				if (PropertyChanging(args))
				{
					_SecondaryBirthDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SecondaryEmail
		{	
			get{ return _SecondaryEmail; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SecondaryEmail, value, _SecondaryEmail);
				if (PropertyChanging(args))
				{
					_SecondaryEmail = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> FundingResult
		{	
			get{ return _FundingResult; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FundingResult, value, _FundingResult);
				if (PropertyChanging(args))
				{
					_FundingResult = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> GrossFundedAmount
		{	
			get{ return _GrossFundedAmount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_GrossFundedAmount, value, _GrossFundedAmount);
				if (PropertyChanging(args))
				{
					_GrossFundedAmount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> NetFundedAmount
		{	
			get{ return _NetFundedAmount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_NetFundedAmount, value, _NetFundedAmount);
				if (PropertyChanging(args))
				{
					_NetFundedAmount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> DiscountFundedAmount
		{	
			get{ return _DiscountFundedAmount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DiscountFundedAmount, value, _DiscountFundedAmount);
				if (PropertyChanging(args))
				{
					_DiscountFundedAmount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> DiscountFundedPercentage
		{	
			get{ return _DiscountFundedPercentage; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DiscountFundedPercentage, value, _DiscountFundedPercentage);
				if (PropertyChanging(args))
				{
					_DiscountFundedPercentage = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> CustomerPaymentAmount
		{	
			get{ return _CustomerPaymentAmount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CustomerPaymentAmount, value, _CustomerPaymentAmount);
				if (PropertyChanging(args))
				{
					_CustomerPaymentAmount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> FinanceRepCommissionRate
		{	
			get{ return _FinanceRepCommissionRate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FinanceRepCommissionRate, value, _FinanceRepCommissionRate);
				if (PropertyChanging(args))
				{
					_FinanceRepCommissionRate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String LoanNumber
		{	
			get{ return _LoanNumber; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LoanNumber, value, _LoanNumber);
				if (PropertyChanging(args))
				{
					_LoanNumber = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CreditAppNumber
		{	
			get{ return _CreditAppNumber; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreditAppNumber, value, _CreditAppNumber);
				if (PropertyChanging(args))
				{
					_CreditAppNumber = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Term
		{	
			get{ return _Term; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Term, value, _Term);
				if (PropertyChanging(args))
				{
					_Term = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String APR
		{	
			get{ return _APR; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_APR, value, _APR);
				if (PropertyChanging(args))
				{
					_APR = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> MaxCreditLimit
		{	
			get{ return _MaxCreditLimit; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MaxCreditLimit, value, _MaxCreditLimit);
				if (PropertyChanging(args))
				{
					_MaxCreditLimit = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> ApprovalDate
		{	
			get{ return _ApprovalDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ApprovalDate, value, _ApprovalDate);
				if (PropertyChanging(args))
				{
					_ApprovalDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> MonthlyFinanceRate
		{	
			get{ return _MonthlyFinanceRate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MonthlyFinanceRate, value, _MonthlyFinanceRate);
				if (PropertyChanging(args))
				{
					_MonthlyFinanceRate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Batch
		{	
			get{ return _Batch; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Batch, value, _Batch);
				if (PropertyChanging(args))
				{
					_Batch = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid FinanceRep
		{	
			get{ return _FinanceRep; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FinanceRep, value, _FinanceRep);
				if (PropertyChanging(args))
				{
					_FinanceRep = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CreditTransectionId
		{	
			get{ return _CreditTransectionId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreditTransectionId, value, _CreditTransectionId);
				if (PropertyChanging(args))
				{
					_CreditTransectionId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> BounceMatchId
		{	
			get{ return _BounceMatchId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BounceMatchId, value, _BounceMatchId);
				if (PropertyChanging(args))
				{
					_BounceMatchId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BounceStatus
		{	
			get{ return _BounceStatus; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BounceStatus, value, _BounceStatus);
				if (PropertyChanging(args))
				{
					_BounceStatus = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> InstallFinishDate
		{	
			get{ return _InstallFinishDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InstallFinishDate, value, _InstallFinishDate);
				if (PropertyChanging(args))
				{
					_InstallFinishDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> PromotionMonth
		{	
			get{ return _PromotionMonth; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PromotionMonth, value, _PromotionMonth);
				if (PropertyChanging(args))
				{
					_PromotionMonth = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> PrepaidMonth
		{	
			get{ return _PrepaidMonth; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PrepaidMonth, value, _PrepaidMonth);
				if (PropertyChanging(args))
				{
					_PrepaidMonth = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> PaymentEffectiveDate
		{	
			get{ return _PaymentEffectiveDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PaymentEffectiveDate, value, _PaymentEffectiveDate);
				if (PropertyChanging(args))
				{
					_PaymentEffectiveDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String FacebookProfileUrl
		{	
			get{ return _FacebookProfileUrl; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FacebookProfileUrl, value, _FacebookProfileUrl);
				if (PropertyChanging(args))
				{
					_FacebookProfileUrl = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String GoogleProfileUrl
		{	
			get{ return _GoogleProfileUrl; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_GoogleProfileUrl, value, _GoogleProfileUrl);
				if (PropertyChanging(args))
				{
					_GoogleProfileUrl = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String FacebookName
		{	
			get{ return _FacebookName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FacebookName, value, _FacebookName);
				if (PropertyChanging(args))
				{
					_FacebookName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String GoogleName
		{	
			get{ return _GoogleName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_GoogleName, value, _GoogleName);
				if (PropertyChanging(args))
				{
					_GoogleName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String LeadVersion
		{	
			get{ return _LeadVersion; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LeadVersion, value, _LeadVersion);
				if (PropertyChanging(args))
				{
					_LeadVersion = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid AppoinmentSetBy
		{	
			get{ return _AppoinmentSetBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AppoinmentSetBy, value, _AppoinmentSetBy);
				if (PropertyChanging(args))
				{
					_AppoinmentSetBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IsPcApplicationId
		{	
			get{ return _IsPcApplicationId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsPcApplicationId, value, _IsPcApplicationId);
				if (PropertyChanging(args))
				{
					_IsPcApplicationId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsPcCreditApproved
		{	
			get{ return _IsPcCreditApproved; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsPcCreditApproved, value, _IsPcCreditApproved);
				if (PropertyChanging(args))
				{
					_IsPcCreditApproved = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> BrinksCancelDate
		{	
			get{ return _BrinksCancelDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BrinksCancelDate, value, _BrinksCancelDate);
				if (PropertyChanging(args))
				{
					_BrinksCancelDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> ReceivedDate
		{	
			get{ return _ReceivedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReceivedDate, value, _ReceivedDate);
				if (PropertyChanging(args))
				{
					_ReceivedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> BrinksFundingDate
		{	
			get{ return _BrinksFundingDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BrinksFundingDate, value, _BrinksFundingDate);
				if (PropertyChanging(args))
				{
					_BrinksFundingDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PlanCode
		{	
			get{ return _PlanCode; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PlanCode, value, _PlanCode);
				if (PropertyChanging(args))
				{
					_PlanCode = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String NewMMR
		{	
			get{ return _NewMMR; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_NewMMR, value, _NewMMR);
				if (PropertyChanging(args))
				{
					_NewMMR = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String LoanAmount
		{	
			get{ return _LoanAmount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LoanAmount, value, _LoanAmount);
				if (PropertyChanging(args))
				{
					_LoanAmount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Payout
		{	
			get{ return _Payout; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Payout, value, _Payout);
				if (PropertyChanging(args))
				{
					_Payout = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> FinanceFundingDate
		{	
			get{ return _FinanceFundingDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FinanceFundingDate, value, _FinanceFundingDate);
				if (PropertyChanging(args))
				{
					_FinanceFundingDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BrinksFundingStatus
		{	
			get{ return _BrinksFundingStatus; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BrinksFundingStatus, value, _BrinksFundingStatus);
				if (PropertyChanging(args))
				{
					_BrinksFundingStatus = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String FinanceFundingStatus
		{	
			get{ return _FinanceFundingStatus; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FinanceFundingStatus, value, _FinanceFundingStatus);
				if (PropertyChanging(args))
				{
					_FinanceFundingStatus = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String IsPcAppStatus
		{	
			get{ return _IsPcAppStatus; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsPcAppStatus, value, _IsPcAppStatus);
				if (PropertyChanging(args))
				{
					_IsPcAppStatus = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AlarmBasicPackage
		{	
			get{ return _AlarmBasicPackage; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AlarmBasicPackage, value, _AlarmBasicPackage);
				if (PropertyChanging(args))
				{
					_AlarmBasicPackage = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String NMCRefId
		{	
			get{ return _NMCRefId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_NMCRefId, value, _NMCRefId);
				if (PropertyChanging(args))
				{
					_NMCRefId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> CustomerSince
		{	
			get{ return _CustomerSince; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CustomerSince, value, _CustomerSince);
				if (PropertyChanging(args))
				{
					_CustomerSince = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> ResignDate
		{	
			get{ return _ResignDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ResignDate, value, _ResignDate);
				if (PropertyChanging(args))
				{
					_ResignDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> MonthlyBatch
		{	
			get{ return _MonthlyBatch; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MonthlyBatch, value, _MonthlyBatch);
				if (PropertyChanging(args))
				{
					_MonthlyBatch = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsAgreementSMSSend
		{	
			get{ return _IsAgreementSMSSend; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsAgreementSMSSend, value, _IsAgreementSMSSend);
				if (PropertyChanging(args))
				{
					_IsAgreementSMSSend = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> UnlinkCustomer
		{	
			get{ return _UnlinkCustomer; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UnlinkCustomer, value, _UnlinkCustomer);
				if (PropertyChanging(args))
				{
					_UnlinkCustomer = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String GeeseLead
		{	
			get{ return _GeeseLead; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_GeeseLead, value, _GeeseLead);
				if (PropertyChanging(args))
				{
					_GeeseLead = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PowerPayAppId
		{	
			get{ return _PowerPayAppId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PowerPayAppId, value, _PowerPayAppId);
				if (PropertyChanging(args))
				{
					_PowerPayAppId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PowerPayAppStatus
		{	
			get{ return _PowerPayAppStatus; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PowerPayAppStatus, value, _PowerPayAppStatus);
				if (PropertyChanging(args))
				{
					_PowerPayAppStatus = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> GeeseCount
		{	
			get{ return _GeeseCount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_GeeseCount, value, _GeeseCount);
				if (PropertyChanging(args))
				{
					_GeeseCount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AvantgradRefId
		{	
			get{ return _AvantgradRefId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AvantgradRefId, value, _AvantgradRefId);
				if (PropertyChanging(args))
				{
					_AvantgradRefId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String DrivingLicense
		{	
			get{ return _DrivingLicense; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DrivingLicense, value, _DrivingLicense);
				if (PropertyChanging(args))
				{
					_DrivingLicense = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> CreatedDay
		{	
			get{ return _CreatedDay; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreatedDay, value, _CreatedDay);
				if (PropertyChanging(args))
				{
					_CreatedDay = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid ResignedBy
		{	
			get{ return _ResignedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ResignedBy, value, _ResignedBy);
				if (PropertyChanging(args))
				{
					_ResignedBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ContractType
		{	
			get{ return _ContractType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ContractType, value, _ContractType);
				if (PropertyChanging(args))
				{
					_ContractType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsSignAgrSendToCus
		{	
			get{ return _IsSignAgrSendToCus; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsSignAgrSendToCus, value, _IsSignAgrSendToCus);
				if (PropertyChanging(args))
				{
					_IsSignAgrSendToCus = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsTestAccount
		{	
			get{ return _IsTestAccount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsTestAccount, value, _IsTestAccount);
				if (PropertyChanging(args))
				{
					_IsTestAccount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Decimal> DealerFee
		{	
			get{ return _DealerFee; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DealerFee, value, _DealerFee);
				if (PropertyChanging(args))
				{
					_DealerFee = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> ContractCreatedDate
		{	
			get{ return _ContractCreatedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ContractCreatedDate, value, _ContractCreatedDate);
				if (PropertyChanging(args))
				{
					_ContractCreatedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Warranty
		{	
			get{ return _Warranty; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Warranty, value, _Warranty);
				if (PropertyChanging(args))
				{
					_Warranty = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Keypad
		{	
			get{ return _Keypad; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Keypad, value, _Keypad);
				if (PropertyChanging(args))
				{
					_Keypad = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String FrontEnd
		{	
			get{ return _FrontEnd; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FrontEnd, value, _FrontEnd);
				if (PropertyChanging(args))
				{
					_FrontEnd = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> CycleStartDate
		{	
			get{ return _CycleStartDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CycleStartDate, value, _CycleStartDate);
				if (PropertyChanging(args))
				{
					_CycleStartDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  CustomerExtendedBase Clone()
		{
			CustomerExtendedBase newObj = new  CustomerExtendedBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CustomerId = this.CustomerId;						
			newObj.Takeover = this.Takeover;						
			newObj.PreWired = this.PreWired;						
			newObj.HardWired = this.HardWired;						
			newObj.CSAgreement = this.CSAgreement;						
			newObj.SalesPerson4 = this.SalesPerson4;						
			newObj.FinanceCompany = this.FinanceCompany;						
			newObj.ContractStartDate = this.ContractStartDate;						
			newObj.RemainingContractTerm = this.RemainingContractTerm;						
			newObj.IsFinanced = this.IsFinanced;						
			newObj.Pets = this.Pets;						
			newObj.PetsType = this.PetsType;						
			newObj.Repair = this.Repair;						
			newObj.RepairType = this.RepairType;						
			newObj.BirthDateCoupon = this.BirthDateCoupon;						
			newObj.VipClubMember = this.VipClubMember;						
			newObj.RWST1 = this.RWST1;						
			newObj.RWST2 = this.RWST2;						
			newObj.RWST3 = this.RWST3;						
			newObj.RWST4 = this.RWST4;						
			newObj.RWST5 = this.RWST5;						
			newObj.RWST6 = this.RWST6;						
			newObj.RWST7 = this.RWST7;						
			newObj.RWST8 = this.RWST8;						
			newObj.RWST9 = this.RWST9;						
			newObj.RWST10 = this.RWST10;						
			newObj.RWST11 = this.RWST11;						
			newObj.RWST12 = this.RWST12;						
			newObj.RWST13 = this.RWST13;						
			newObj.RWST14 = this.RWST14;						
			newObj.RWST15 = this.RWST15;						
			newObj.RepsAssignedDate = this.RepsAssignedDate;						
			newObj.ContractSentBy = this.ContractSentBy;						
			newObj.SecondaryFirstName = this.SecondaryFirstName;						
			newObj.SecondaryLastName = this.SecondaryLastName;						
			newObj.SecondarySSN = this.SecondarySSN;						
			newObj.SecondaryBirthDate = this.SecondaryBirthDate;						
			newObj.SecondaryEmail = this.SecondaryEmail;						
			newObj.FundingResult = this.FundingResult;						
			newObj.GrossFundedAmount = this.GrossFundedAmount;						
			newObj.NetFundedAmount = this.NetFundedAmount;						
			newObj.DiscountFundedAmount = this.DiscountFundedAmount;						
			newObj.DiscountFundedPercentage = this.DiscountFundedPercentage;						
			newObj.CustomerPaymentAmount = this.CustomerPaymentAmount;						
			newObj.FinanceRepCommissionRate = this.FinanceRepCommissionRate;						
			newObj.LoanNumber = this.LoanNumber;						
			newObj.CreditAppNumber = this.CreditAppNumber;						
			newObj.Term = this.Term;						
			newObj.APR = this.APR;						
			newObj.MaxCreditLimit = this.MaxCreditLimit;						
			newObj.ApprovalDate = this.ApprovalDate;						
			newObj.MonthlyFinanceRate = this.MonthlyFinanceRate;						
			newObj.Batch = this.Batch;						
			newObj.FinanceRep = this.FinanceRep;						
			newObj.CreditTransectionId = this.CreditTransectionId;						
			newObj.BounceMatchId = this.BounceMatchId;						
			newObj.BounceStatus = this.BounceStatus;						
			newObj.InstallFinishDate = this.InstallFinishDate;						
			newObj.PromotionMonth = this.PromotionMonth;						
			newObj.PrepaidMonth = this.PrepaidMonth;						
			newObj.PaymentEffectiveDate = this.PaymentEffectiveDate;						
			newObj.FacebookProfileUrl = this.FacebookProfileUrl;						
			newObj.GoogleProfileUrl = this.GoogleProfileUrl;						
			newObj.FacebookName = this.FacebookName;						
			newObj.GoogleName = this.GoogleName;						
			newObj.LeadVersion = this.LeadVersion;						
			newObj.AppoinmentSetBy = this.AppoinmentSetBy;						
			newObj.IsPcApplicationId = this.IsPcApplicationId;						
			newObj.IsPcCreditApproved = this.IsPcCreditApproved;						
			newObj.BrinksCancelDate = this.BrinksCancelDate;						
			newObj.ReceivedDate = this.ReceivedDate;						
			newObj.BrinksFundingDate = this.BrinksFundingDate;						
			newObj.PlanCode = this.PlanCode;						
			newObj.NewMMR = this.NewMMR;						
			newObj.LoanAmount = this.LoanAmount;						
			newObj.Payout = this.Payout;						
			newObj.FinanceFundingDate = this.FinanceFundingDate;						
			newObj.BrinksFundingStatus = this.BrinksFundingStatus;						
			newObj.FinanceFundingStatus = this.FinanceFundingStatus;						
			newObj.IsPcAppStatus = this.IsPcAppStatus;						
			newObj.AlarmBasicPackage = this.AlarmBasicPackage;						
			newObj.NMCRefId = this.NMCRefId;						
			newObj.CustomerSince = this.CustomerSince;						
			newObj.ResignDate = this.ResignDate;						
			newObj.MonthlyBatch = this.MonthlyBatch;						
			newObj.IsAgreementSMSSend = this.IsAgreementSMSSend;						
			newObj.UnlinkCustomer = this.UnlinkCustomer;						
			newObj.GeeseLead = this.GeeseLead;						
			newObj.PowerPayAppId = this.PowerPayAppId;						
			newObj.PowerPayAppStatus = this.PowerPayAppStatus;						
			newObj.GeeseCount = this.GeeseCount;						
			newObj.AvantgradRefId = this.AvantgradRefId;						
			newObj.DrivingLicense = this.DrivingLicense;						
			newObj.CreatedDay = this.CreatedDay;						
			newObj.ResignedBy = this.ResignedBy;						
			newObj.ContractType = this.ContractType;						
			newObj.IsSignAgrSendToCus = this.IsSignAgrSendToCus;						
			newObj.IsTestAccount = this.IsTestAccount;						
			newObj.DealerFee = this.DealerFee;						
			newObj.ContractCreatedDate = this.ContractCreatedDate;						
			newObj.Warranty = this.Warranty;						
			newObj.Keypad = this.Keypad;						
			newObj.FrontEnd = this.FrontEnd;						
			newObj.CycleStartDate = this.CycleStartDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomerExtendedBase.Property_Id, Id);				
			info.AddValue(CustomerExtendedBase.Property_CustomerId, CustomerId);				
			info.AddValue(CustomerExtendedBase.Property_Takeover, Takeover);				
			info.AddValue(CustomerExtendedBase.Property_PreWired, PreWired);				
			info.AddValue(CustomerExtendedBase.Property_HardWired, HardWired);				
			info.AddValue(CustomerExtendedBase.Property_CSAgreement, CSAgreement);				
			info.AddValue(CustomerExtendedBase.Property_SalesPerson4, SalesPerson4);				
			info.AddValue(CustomerExtendedBase.Property_FinanceCompany, FinanceCompany);				
			info.AddValue(CustomerExtendedBase.Property_ContractStartDate, ContractStartDate);				
			info.AddValue(CustomerExtendedBase.Property_RemainingContractTerm, RemainingContractTerm);				
			info.AddValue(CustomerExtendedBase.Property_IsFinanced, IsFinanced);				
			info.AddValue(CustomerExtendedBase.Property_Pets, Pets);				
			info.AddValue(CustomerExtendedBase.Property_PetsType, PetsType);				
			info.AddValue(CustomerExtendedBase.Property_Repair, Repair);				
			info.AddValue(CustomerExtendedBase.Property_RepairType, RepairType);				
			info.AddValue(CustomerExtendedBase.Property_BirthDateCoupon, BirthDateCoupon);				
			info.AddValue(CustomerExtendedBase.Property_VipClubMember, VipClubMember);				
			info.AddValue(CustomerExtendedBase.Property_RWST1, RWST1);				
			info.AddValue(CustomerExtendedBase.Property_RWST2, RWST2);				
			info.AddValue(CustomerExtendedBase.Property_RWST3, RWST3);				
			info.AddValue(CustomerExtendedBase.Property_RWST4, RWST4);				
			info.AddValue(CustomerExtendedBase.Property_RWST5, RWST5);				
			info.AddValue(CustomerExtendedBase.Property_RWST6, RWST6);				
			info.AddValue(CustomerExtendedBase.Property_RWST7, RWST7);				
			info.AddValue(CustomerExtendedBase.Property_RWST8, RWST8);				
			info.AddValue(CustomerExtendedBase.Property_RWST9, RWST9);				
			info.AddValue(CustomerExtendedBase.Property_RWST10, RWST10);				
			info.AddValue(CustomerExtendedBase.Property_RWST11, RWST11);				
			info.AddValue(CustomerExtendedBase.Property_RWST12, RWST12);				
			info.AddValue(CustomerExtendedBase.Property_RWST13, RWST13);				
			info.AddValue(CustomerExtendedBase.Property_RWST14, RWST14);				
			info.AddValue(CustomerExtendedBase.Property_RWST15, RWST15);				
			info.AddValue(CustomerExtendedBase.Property_RepsAssignedDate, RepsAssignedDate);				
			info.AddValue(CustomerExtendedBase.Property_ContractSentBy, ContractSentBy);				
			info.AddValue(CustomerExtendedBase.Property_SecondaryFirstName, SecondaryFirstName);				
			info.AddValue(CustomerExtendedBase.Property_SecondaryLastName, SecondaryLastName);				
			info.AddValue(CustomerExtendedBase.Property_SecondarySSN, SecondarySSN);				
			info.AddValue(CustomerExtendedBase.Property_SecondaryBirthDate, SecondaryBirthDate);				
			info.AddValue(CustomerExtendedBase.Property_SecondaryEmail, SecondaryEmail);				
			info.AddValue(CustomerExtendedBase.Property_FundingResult, FundingResult);				
			info.AddValue(CustomerExtendedBase.Property_GrossFundedAmount, GrossFundedAmount);				
			info.AddValue(CustomerExtendedBase.Property_NetFundedAmount, NetFundedAmount);				
			info.AddValue(CustomerExtendedBase.Property_DiscountFundedAmount, DiscountFundedAmount);				
			info.AddValue(CustomerExtendedBase.Property_DiscountFundedPercentage, DiscountFundedPercentage);				
			info.AddValue(CustomerExtendedBase.Property_CustomerPaymentAmount, CustomerPaymentAmount);				
			info.AddValue(CustomerExtendedBase.Property_FinanceRepCommissionRate, FinanceRepCommissionRate);				
			info.AddValue(CustomerExtendedBase.Property_LoanNumber, LoanNumber);				
			info.AddValue(CustomerExtendedBase.Property_CreditAppNumber, CreditAppNumber);				
			info.AddValue(CustomerExtendedBase.Property_Term, Term);				
			info.AddValue(CustomerExtendedBase.Property_APR, APR);				
			info.AddValue(CustomerExtendedBase.Property_MaxCreditLimit, MaxCreditLimit);				
			info.AddValue(CustomerExtendedBase.Property_ApprovalDate, ApprovalDate);				
			info.AddValue(CustomerExtendedBase.Property_MonthlyFinanceRate, MonthlyFinanceRate);				
			info.AddValue(CustomerExtendedBase.Property_Batch, Batch);				
			info.AddValue(CustomerExtendedBase.Property_FinanceRep, FinanceRep);				
			info.AddValue(CustomerExtendedBase.Property_CreditTransectionId, CreditTransectionId);				
			info.AddValue(CustomerExtendedBase.Property_BounceMatchId, BounceMatchId);				
			info.AddValue(CustomerExtendedBase.Property_BounceStatus, BounceStatus);				
			info.AddValue(CustomerExtendedBase.Property_InstallFinishDate, InstallFinishDate);				
			info.AddValue(CustomerExtendedBase.Property_PromotionMonth, PromotionMonth);				
			info.AddValue(CustomerExtendedBase.Property_PrepaidMonth, PrepaidMonth);				
			info.AddValue(CustomerExtendedBase.Property_PaymentEffectiveDate, PaymentEffectiveDate);				
			info.AddValue(CustomerExtendedBase.Property_FacebookProfileUrl, FacebookProfileUrl);				
			info.AddValue(CustomerExtendedBase.Property_GoogleProfileUrl, GoogleProfileUrl);				
			info.AddValue(CustomerExtendedBase.Property_FacebookName, FacebookName);				
			info.AddValue(CustomerExtendedBase.Property_GoogleName, GoogleName);				
			info.AddValue(CustomerExtendedBase.Property_LeadVersion, LeadVersion);				
			info.AddValue(CustomerExtendedBase.Property_AppoinmentSetBy, AppoinmentSetBy);				
			info.AddValue(CustomerExtendedBase.Property_IsPcApplicationId, IsPcApplicationId);				
			info.AddValue(CustomerExtendedBase.Property_IsPcCreditApproved, IsPcCreditApproved);				
			info.AddValue(CustomerExtendedBase.Property_BrinksCancelDate, BrinksCancelDate);				
			info.AddValue(CustomerExtendedBase.Property_ReceivedDate, ReceivedDate);				
			info.AddValue(CustomerExtendedBase.Property_BrinksFundingDate, BrinksFundingDate);				
			info.AddValue(CustomerExtendedBase.Property_PlanCode, PlanCode);				
			info.AddValue(CustomerExtendedBase.Property_NewMMR, NewMMR);				
			info.AddValue(CustomerExtendedBase.Property_LoanAmount, LoanAmount);				
			info.AddValue(CustomerExtendedBase.Property_Payout, Payout);				
			info.AddValue(CustomerExtendedBase.Property_FinanceFundingDate, FinanceFundingDate);				
			info.AddValue(CustomerExtendedBase.Property_BrinksFundingStatus, BrinksFundingStatus);				
			info.AddValue(CustomerExtendedBase.Property_FinanceFundingStatus, FinanceFundingStatus);				
			info.AddValue(CustomerExtendedBase.Property_IsPcAppStatus, IsPcAppStatus);				
			info.AddValue(CustomerExtendedBase.Property_AlarmBasicPackage, AlarmBasicPackage);				
			info.AddValue(CustomerExtendedBase.Property_NMCRefId, NMCRefId);				
			info.AddValue(CustomerExtendedBase.Property_CustomerSince, CustomerSince);				
			info.AddValue(CustomerExtendedBase.Property_ResignDate, ResignDate);				
			info.AddValue(CustomerExtendedBase.Property_MonthlyBatch, MonthlyBatch);				
			info.AddValue(CustomerExtendedBase.Property_IsAgreementSMSSend, IsAgreementSMSSend);				
			info.AddValue(CustomerExtendedBase.Property_UnlinkCustomer, UnlinkCustomer);				
			info.AddValue(CustomerExtendedBase.Property_GeeseLead, GeeseLead);				
			info.AddValue(CustomerExtendedBase.Property_PowerPayAppId, PowerPayAppId);				
			info.AddValue(CustomerExtendedBase.Property_PowerPayAppStatus, PowerPayAppStatus);				
			info.AddValue(CustomerExtendedBase.Property_GeeseCount, GeeseCount);				
			info.AddValue(CustomerExtendedBase.Property_AvantgradRefId, AvantgradRefId);				
			info.AddValue(CustomerExtendedBase.Property_DrivingLicense, DrivingLicense);				
			info.AddValue(CustomerExtendedBase.Property_CreatedDay, CreatedDay);				
			info.AddValue(CustomerExtendedBase.Property_ResignedBy, ResignedBy);				
			info.AddValue(CustomerExtendedBase.Property_ContractType, ContractType);				
			info.AddValue(CustomerExtendedBase.Property_IsSignAgrSendToCus, IsSignAgrSendToCus);				
			info.AddValue(CustomerExtendedBase.Property_IsTestAccount, IsTestAccount);				
			info.AddValue(CustomerExtendedBase.Property_DealerFee, DealerFee);				
			info.AddValue(CustomerExtendedBase.Property_ContractCreatedDate, ContractCreatedDate);				
			info.AddValue(CustomerExtendedBase.Property_Warranty, Warranty);				
			info.AddValue(CustomerExtendedBase.Property_Keypad, Keypad);				
			info.AddValue(CustomerExtendedBase.Property_FrontEnd, FrontEnd);				
			info.AddValue(CustomerExtendedBase.Property_CycleStartDate, CycleStartDate);				
		}
		#endregion

		
	}
}
