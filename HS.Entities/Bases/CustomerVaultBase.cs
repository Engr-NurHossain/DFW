using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomerVaultBase", Namespace = "http://www.piistech.com//entities")]
	public class CustomerVaultBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CustomerAccountId = 1,
			MonitoringID = 2,
			CustomerName = 3,
			Address1 = 4,
			Zip = 5,
			Company = 6,
			City = 7,
			LongCity = 8,
			County = 9,
			State = 10,
			Phone1 = 11,
			Email = 12,
			Area = 13,
			SaleDate = 14,
			InstallDate = 15,
			Install = 16,
			TimeRange = 17,
			ShortSaleDate = 18,
			ShortInstallDate = 19,
			Season = 20,
			CustomerAccountIdName = 21,
			AccountHolderID = 22,
			MonitoringCompanyID = 23,
			MonitoringCompany = 24,
			PanelTypeNameOLD = 25,
			SystemName = 26,
			SystemPackageName = 27,
			CreditScore = 28,
			StatusID = 29,
			ActivationFeeAmount = 30,
			AccountHolder1 = 31,
			MonthlyMonitoringBaseRate = 32,
			SystemServicesTotal = 33,
			MonthlyMonitoringRate = 34,
			SalesRep = 35,
			Technician = 36,
			FriendsFamilyRep = 37,
			Status = 38,
			EquipmentStatus = 39,
			isAccountOnline = 40,
			isAccountInService = 41,
			CaseCount = 42,
			PointsUsedSales = 43,
			LeadSource = 44,
			LeadSourceId = 45,
			isRepAccountHold = 46,
			isTechAccountHold = 47,
			DateEntered = 48,
			QA = 49,
			QA2 = 50,
			QA1Date = 51,
			QualityScore = 52,
			PreScreenDate = 53,
			FullReportDate = 54,
			ContractTerm = 55,
			BillingMethod = 56,
			Takeover = 57,
			DOB = 58,
			ContractId = 59,
			TransactionID = 60
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CustomerAccountId = "CustomerAccountId";		            
		public const string Property_MonitoringID = "MonitoringID";		            
		public const string Property_CustomerName = "CustomerName";		            
		public const string Property_Address1 = "Address1";		            
		public const string Property_Zip = "Zip";		            
		public const string Property_Company = "Company";		            
		public const string Property_City = "City";		            
		public const string Property_LongCity = "LongCity";		            
		public const string Property_County = "County";		            
		public const string Property_State = "State";		            
		public const string Property_Phone1 = "Phone1";		            
		public const string Property_Email = "Email";		            
		public const string Property_Area = "Area";		            
		public const string Property_SaleDate = "SaleDate";		            
		public const string Property_InstallDate = "InstallDate";		            
		public const string Property_Install = "Install";		            
		public const string Property_TimeRange = "TimeRange";		            
		public const string Property_ShortSaleDate = "ShortSaleDate";		            
		public const string Property_ShortInstallDate = "ShortInstallDate";		            
		public const string Property_Season = "Season";		            
		public const string Property_CustomerAccountIdName = "CustomerAccountIdName";		            
		public const string Property_AccountHolderID = "AccountHolderID";		            
		public const string Property_MonitoringCompanyID = "MonitoringCompanyID";		            
		public const string Property_MonitoringCompany = "MonitoringCompany";		            
		public const string Property_PanelTypeNameOLD = "PanelTypeNameOLD";		            
		public const string Property_SystemName = "SystemName";		            
		public const string Property_SystemPackageName = "SystemPackageName";		            
		public const string Property_CreditScore = "CreditScore";		            
		public const string Property_StatusID = "StatusID";		            
		public const string Property_ActivationFeeAmount = "ActivationFeeAmount";		            
		public const string Property_AccountHolder1 = "AccountHolder1";		            
		public const string Property_MonthlyMonitoringBaseRate = "MonthlyMonitoringBaseRate";		            
		public const string Property_SystemServicesTotal = "SystemServicesTotal";		            
		public const string Property_MonthlyMonitoringRate = "MonthlyMonitoringRate";		            
		public const string Property_SalesRep = "SalesRep";		            
		public const string Property_Technician = "Technician";		            
		public const string Property_FriendsFamilyRep = "FriendsFamilyRep";		            
		public const string Property_Status = "Status";		            
		public const string Property_EquipmentStatus = "EquipmentStatus";		            
		public const string Property_isAccountOnline = "isAccountOnline";		            
		public const string Property_isAccountInService = "isAccountInService";		            
		public const string Property_CaseCount = "CaseCount";		            
		public const string Property_PointsUsedSales = "PointsUsedSales";		            
		public const string Property_LeadSource = "LeadSource";		            
		public const string Property_LeadSourceId = "LeadSourceId";		            
		public const string Property_isRepAccountHold = "isRepAccountHold";		            
		public const string Property_isTechAccountHold = "isTechAccountHold";		            
		public const string Property_DateEntered = "DateEntered";		            
		public const string Property_QA = "QA";		            
		public const string Property_QA2 = "QA2";		            
		public const string Property_QA1Date = "QA1Date";		            
		public const string Property_QualityScore = "QualityScore";		            
		public const string Property_PreScreenDate = "PreScreenDate";		            
		public const string Property_FullReportDate = "FullReportDate";		            
		public const string Property_ContractTerm = "ContractTerm";		            
		public const string Property_BillingMethod = "BillingMethod";		            
		public const string Property_Takeover = "Takeover";		            
		public const string Property_DOB = "DOB";		            
		public const string Property_ContractId = "ContractId";		            
		public const string Property_TransactionID = "TransactionID";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _CustomerAccountId;	            
		private String _MonitoringID;	            
		private String _CustomerName;	            
		private String _Address1;	            
		private String _Zip;	            
		private String _Company;	            
		private String _City;	            
		private String _LongCity;	            
		private String _County;	            
		private String _State;	            
		private String _Phone1;	            
		private String _Email;	            
		private String _Area;	            
		private Nullable<DateTime> _SaleDate;	            
		private Nullable<DateTime> _InstallDate;	            
		private String _Install;	            
		private String _TimeRange;	            
		private Nullable<DateTime> _ShortSaleDate;	            
		private Nullable<DateTime> _ShortInstallDate;	            
		private String _Season;	            
		private String _CustomerAccountIdName;	            
		private String _AccountHolderID;	            
		private String _MonitoringCompanyID;	            
		private String _MonitoringCompany;	            
		private String _PanelTypeNameOLD;	            
		private String _SystemName;	            
		private String _SystemPackageName;	            
		private Nullable<Int32> _CreditScore;	            
		private String _StatusID;	            
		private Nullable<Double> _ActivationFeeAmount;	            
		private String _AccountHolder1;	            
		private Nullable<Double> _MonthlyMonitoringBaseRate;	            
		private Nullable<Double> _SystemServicesTotal;	            
		private Nullable<Double> _MonthlyMonitoringRate;	            
		private String _SalesRep;	            
		private String _Technician;	            
		private String _FriendsFamilyRep;	            
		private String _Status;	            
		private String _EquipmentStatus;	            
		private Nullable<Boolean> _isAccountOnline;	            
		private Nullable<Boolean> _isAccountInService;	            
		private Nullable<Int32> _CaseCount;	            
		private String _PointsUsedSales;	            
		private String _LeadSource;	            
		private String _LeadSourceId;	            
		private Nullable<Boolean> _isRepAccountHold;	            
		private Nullable<Boolean> _isTechAccountHold;	            
		private String _DateEntered;	            
		private String _QA;	            
		private String _QA2;	            
		private Nullable<DateTime> _QA1Date;	            
		private Nullable<Double> _QualityScore;	            
		private Nullable<DateTime> _PreScreenDate;	            
		private Nullable<DateTime> _FullReportDate;	            
		private String _ContractTerm;	            
		private String _BillingMethod;	            
		private Nullable<Boolean> _Takeover;	            
		private Nullable<DateTime> _DOB;	            
		private String _ContractId;	            
		private String _TransactionID;	            
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
		public String CustomerAccountId
		{	
			get{ return _CustomerAccountId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CustomerAccountId, value, _CustomerAccountId);
				if (PropertyChanging(args))
				{
					_CustomerAccountId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String MonitoringID
		{	
			get{ return _MonitoringID; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MonitoringID, value, _MonitoringID);
				if (PropertyChanging(args))
				{
					_MonitoringID = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CustomerName
		{	
			get{ return _CustomerName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CustomerName, value, _CustomerName);
				if (PropertyChanging(args))
				{
					_CustomerName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Address1
		{	
			get{ return _Address1; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Address1, value, _Address1);
				if (PropertyChanging(args))
				{
					_Address1 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Zip
		{	
			get{ return _Zip; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Zip, value, _Zip);
				if (PropertyChanging(args))
				{
					_Zip = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Company
		{	
			get{ return _Company; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Company, value, _Company);
				if (PropertyChanging(args))
				{
					_Company = value;
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
		public String LongCity
		{	
			get{ return _LongCity; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LongCity, value, _LongCity);
				if (PropertyChanging(args))
				{
					_LongCity = value;
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
		public String Phone1
		{	
			get{ return _Phone1; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Phone1, value, _Phone1);
				if (PropertyChanging(args))
				{
					_Phone1 = value;
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
		public Nullable<DateTime> SaleDate
		{	
			get{ return _SaleDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SaleDate, value, _SaleDate);
				if (PropertyChanging(args))
				{
					_SaleDate = value;
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
		public String Install
		{	
			get{ return _Install; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Install, value, _Install);
				if (PropertyChanging(args))
				{
					_Install = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TimeRange
		{	
			get{ return _TimeRange; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TimeRange, value, _TimeRange);
				if (PropertyChanging(args))
				{
					_TimeRange = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> ShortSaleDate
		{	
			get{ return _ShortSaleDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ShortSaleDate, value, _ShortSaleDate);
				if (PropertyChanging(args))
				{
					_ShortSaleDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> ShortInstallDate
		{	
			get{ return _ShortInstallDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ShortInstallDate, value, _ShortInstallDate);
				if (PropertyChanging(args))
				{
					_ShortInstallDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Season
		{	
			get{ return _Season; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Season, value, _Season);
				if (PropertyChanging(args))
				{
					_Season = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CustomerAccountIdName
		{	
			get{ return _CustomerAccountIdName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CustomerAccountIdName, value, _CustomerAccountIdName);
				if (PropertyChanging(args))
				{
					_CustomerAccountIdName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AccountHolderID
		{	
			get{ return _AccountHolderID; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AccountHolderID, value, _AccountHolderID);
				if (PropertyChanging(args))
				{
					_AccountHolderID = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String MonitoringCompanyID
		{	
			get{ return _MonitoringCompanyID; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MonitoringCompanyID, value, _MonitoringCompanyID);
				if (PropertyChanging(args))
				{
					_MonitoringCompanyID = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String MonitoringCompany
		{	
			get{ return _MonitoringCompany; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MonitoringCompany, value, _MonitoringCompany);
				if (PropertyChanging(args))
				{
					_MonitoringCompany = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PanelTypeNameOLD
		{	
			get{ return _PanelTypeNameOLD; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PanelTypeNameOLD, value, _PanelTypeNameOLD);
				if (PropertyChanging(args))
				{
					_PanelTypeNameOLD = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SystemName
		{	
			get{ return _SystemName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SystemName, value, _SystemName);
				if (PropertyChanging(args))
				{
					_SystemName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SystemPackageName
		{	
			get{ return _SystemPackageName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SystemPackageName, value, _SystemPackageName);
				if (PropertyChanging(args))
				{
					_SystemPackageName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> CreditScore
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
		public String StatusID
		{	
			get{ return _StatusID; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_StatusID, value, _StatusID);
				if (PropertyChanging(args))
				{
					_StatusID = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> ActivationFeeAmount
		{	
			get{ return _ActivationFeeAmount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ActivationFeeAmount, value, _ActivationFeeAmount);
				if (PropertyChanging(args))
				{
					_ActivationFeeAmount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AccountHolder1
		{	
			get{ return _AccountHolder1; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AccountHolder1, value, _AccountHolder1);
				if (PropertyChanging(args))
				{
					_AccountHolder1 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> MonthlyMonitoringBaseRate
		{	
			get{ return _MonthlyMonitoringBaseRate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MonthlyMonitoringBaseRate, value, _MonthlyMonitoringBaseRate);
				if (PropertyChanging(args))
				{
					_MonthlyMonitoringBaseRate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> SystemServicesTotal
		{	
			get{ return _SystemServicesTotal; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SystemServicesTotal, value, _SystemServicesTotal);
				if (PropertyChanging(args))
				{
					_SystemServicesTotal = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> MonthlyMonitoringRate
		{	
			get{ return _MonthlyMonitoringRate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MonthlyMonitoringRate, value, _MonthlyMonitoringRate);
				if (PropertyChanging(args))
				{
					_MonthlyMonitoringRate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SalesRep
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
		public String Technician
		{	
			get{ return _Technician; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Technician, value, _Technician);
				if (PropertyChanging(args))
				{
					_Technician = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String FriendsFamilyRep
		{	
			get{ return _FriendsFamilyRep; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FriendsFamilyRep, value, _FriendsFamilyRep);
				if (PropertyChanging(args))
				{
					_FriendsFamilyRep = value;
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
		public String EquipmentStatus
		{	
			get{ return _EquipmentStatus; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EquipmentStatus, value, _EquipmentStatus);
				if (PropertyChanging(args))
				{
					_EquipmentStatus = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> isAccountOnline
		{	
			get{ return _isAccountOnline; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_isAccountOnline, value, _isAccountOnline);
				if (PropertyChanging(args))
				{
					_isAccountOnline = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> isAccountInService
		{	
			get{ return _isAccountInService; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_isAccountInService, value, _isAccountInService);
				if (PropertyChanging(args))
				{
					_isAccountInService = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> CaseCount
		{	
			get{ return _CaseCount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CaseCount, value, _CaseCount);
				if (PropertyChanging(args))
				{
					_CaseCount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PointsUsedSales
		{	
			get{ return _PointsUsedSales; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PointsUsedSales, value, _PointsUsedSales);
				if (PropertyChanging(args))
				{
					_PointsUsedSales = value;
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
		public String LeadSourceId
		{	
			get{ return _LeadSourceId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LeadSourceId, value, _LeadSourceId);
				if (PropertyChanging(args))
				{
					_LeadSourceId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> isRepAccountHold
		{	
			get{ return _isRepAccountHold; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_isRepAccountHold, value, _isRepAccountHold);
				if (PropertyChanging(args))
				{
					_isRepAccountHold = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> isTechAccountHold
		{	
			get{ return _isTechAccountHold; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_isTechAccountHold, value, _isTechAccountHold);
				if (PropertyChanging(args))
				{
					_isTechAccountHold = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String DateEntered
		{	
			get{ return _DateEntered; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DateEntered, value, _DateEntered);
				if (PropertyChanging(args))
				{
					_DateEntered = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String QA
		{	
			get{ return _QA; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_QA, value, _QA);
				if (PropertyChanging(args))
				{
					_QA = value;
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
		public Nullable<Double> QualityScore
		{	
			get{ return _QualityScore; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_QualityScore, value, _QualityScore);
				if (PropertyChanging(args))
				{
					_QualityScore = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> PreScreenDate
		{	
			get{ return _PreScreenDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PreScreenDate, value, _PreScreenDate);
				if (PropertyChanging(args))
				{
					_PreScreenDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> FullReportDate
		{	
			get{ return _FullReportDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FullReportDate, value, _FullReportDate);
				if (PropertyChanging(args))
				{
					_FullReportDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ContractTerm
		{	
			get{ return _ContractTerm; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ContractTerm, value, _ContractTerm);
				if (PropertyChanging(args))
				{
					_ContractTerm = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BillingMethod
		{	
			get{ return _BillingMethod; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BillingMethod, value, _BillingMethod);
				if (PropertyChanging(args))
				{
					_BillingMethod = value;
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
		public String ContractId
		{	
			get{ return _ContractId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ContractId, value, _ContractId);
				if (PropertyChanging(args))
				{
					_ContractId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TransactionID
		{	
			get{ return _TransactionID; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TransactionID, value, _TransactionID);
				if (PropertyChanging(args))
				{
					_TransactionID = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  CustomerVaultBase Clone()
		{
			CustomerVaultBase newObj = new  CustomerVaultBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CustomerAccountId = this.CustomerAccountId;						
			newObj.MonitoringID = this.MonitoringID;						
			newObj.CustomerName = this.CustomerName;						
			newObj.Address1 = this.Address1;						
			newObj.Zip = this.Zip;						
			newObj.Company = this.Company;						
			newObj.City = this.City;						
			newObj.LongCity = this.LongCity;						
			newObj.County = this.County;						
			newObj.State = this.State;						
			newObj.Phone1 = this.Phone1;						
			newObj.Email = this.Email;						
			newObj.Area = this.Area;						
			newObj.SaleDate = this.SaleDate;						
			newObj.InstallDate = this.InstallDate;						
			newObj.Install = this.Install;						
			newObj.TimeRange = this.TimeRange;						
			newObj.ShortSaleDate = this.ShortSaleDate;						
			newObj.ShortInstallDate = this.ShortInstallDate;						
			newObj.Season = this.Season;						
			newObj.CustomerAccountIdName = this.CustomerAccountIdName;						
			newObj.AccountHolderID = this.AccountHolderID;						
			newObj.MonitoringCompanyID = this.MonitoringCompanyID;						
			newObj.MonitoringCompany = this.MonitoringCompany;						
			newObj.PanelTypeNameOLD = this.PanelTypeNameOLD;						
			newObj.SystemName = this.SystemName;						
			newObj.SystemPackageName = this.SystemPackageName;						
			newObj.CreditScore = this.CreditScore;						
			newObj.StatusID = this.StatusID;						
			newObj.ActivationFeeAmount = this.ActivationFeeAmount;						
			newObj.AccountHolder1 = this.AccountHolder1;						
			newObj.MonthlyMonitoringBaseRate = this.MonthlyMonitoringBaseRate;						
			newObj.SystemServicesTotal = this.SystemServicesTotal;						
			newObj.MonthlyMonitoringRate = this.MonthlyMonitoringRate;						
			newObj.SalesRep = this.SalesRep;						
			newObj.Technician = this.Technician;						
			newObj.FriendsFamilyRep = this.FriendsFamilyRep;						
			newObj.Status = this.Status;						
			newObj.EquipmentStatus = this.EquipmentStatus;						
			newObj.isAccountOnline = this.isAccountOnline;						
			newObj.isAccountInService = this.isAccountInService;						
			newObj.CaseCount = this.CaseCount;						
			newObj.PointsUsedSales = this.PointsUsedSales;						
			newObj.LeadSource = this.LeadSource;						
			newObj.LeadSourceId = this.LeadSourceId;						
			newObj.isRepAccountHold = this.isRepAccountHold;						
			newObj.isTechAccountHold = this.isTechAccountHold;						
			newObj.DateEntered = this.DateEntered;						
			newObj.QA = this.QA;						
			newObj.QA2 = this.QA2;						
			newObj.QA1Date = this.QA1Date;						
			newObj.QualityScore = this.QualityScore;						
			newObj.PreScreenDate = this.PreScreenDate;						
			newObj.FullReportDate = this.FullReportDate;						
			newObj.ContractTerm = this.ContractTerm;						
			newObj.BillingMethod = this.BillingMethod;						
			newObj.Takeover = this.Takeover;						
			newObj.DOB = this.DOB;						
			newObj.ContractId = this.ContractId;						
			newObj.TransactionID = this.TransactionID;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomerVaultBase.Property_Id, Id);				
			info.AddValue(CustomerVaultBase.Property_CustomerAccountId, CustomerAccountId);				
			info.AddValue(CustomerVaultBase.Property_MonitoringID, MonitoringID);				
			info.AddValue(CustomerVaultBase.Property_CustomerName, CustomerName);				
			info.AddValue(CustomerVaultBase.Property_Address1, Address1);				
			info.AddValue(CustomerVaultBase.Property_Zip, Zip);				
			info.AddValue(CustomerVaultBase.Property_Company, Company);				
			info.AddValue(CustomerVaultBase.Property_City, City);				
			info.AddValue(CustomerVaultBase.Property_LongCity, LongCity);				
			info.AddValue(CustomerVaultBase.Property_County, County);				
			info.AddValue(CustomerVaultBase.Property_State, State);				
			info.AddValue(CustomerVaultBase.Property_Phone1, Phone1);				
			info.AddValue(CustomerVaultBase.Property_Email, Email);				
			info.AddValue(CustomerVaultBase.Property_Area, Area);				
			info.AddValue(CustomerVaultBase.Property_SaleDate, SaleDate);				
			info.AddValue(CustomerVaultBase.Property_InstallDate, InstallDate);				
			info.AddValue(CustomerVaultBase.Property_Install, Install);				
			info.AddValue(CustomerVaultBase.Property_TimeRange, TimeRange);				
			info.AddValue(CustomerVaultBase.Property_ShortSaleDate, ShortSaleDate);				
			info.AddValue(CustomerVaultBase.Property_ShortInstallDate, ShortInstallDate);				
			info.AddValue(CustomerVaultBase.Property_Season, Season);				
			info.AddValue(CustomerVaultBase.Property_CustomerAccountIdName, CustomerAccountIdName);				
			info.AddValue(CustomerVaultBase.Property_AccountHolderID, AccountHolderID);				
			info.AddValue(CustomerVaultBase.Property_MonitoringCompanyID, MonitoringCompanyID);				
			info.AddValue(CustomerVaultBase.Property_MonitoringCompany, MonitoringCompany);				
			info.AddValue(CustomerVaultBase.Property_PanelTypeNameOLD, PanelTypeNameOLD);				
			info.AddValue(CustomerVaultBase.Property_SystemName, SystemName);				
			info.AddValue(CustomerVaultBase.Property_SystemPackageName, SystemPackageName);				
			info.AddValue(CustomerVaultBase.Property_CreditScore, CreditScore);				
			info.AddValue(CustomerVaultBase.Property_StatusID, StatusID);				
			info.AddValue(CustomerVaultBase.Property_ActivationFeeAmount, ActivationFeeAmount);				
			info.AddValue(CustomerVaultBase.Property_AccountHolder1, AccountHolder1);				
			info.AddValue(CustomerVaultBase.Property_MonthlyMonitoringBaseRate, MonthlyMonitoringBaseRate);				
			info.AddValue(CustomerVaultBase.Property_SystemServicesTotal, SystemServicesTotal);				
			info.AddValue(CustomerVaultBase.Property_MonthlyMonitoringRate, MonthlyMonitoringRate);				
			info.AddValue(CustomerVaultBase.Property_SalesRep, SalesRep);				
			info.AddValue(CustomerVaultBase.Property_Technician, Technician);				
			info.AddValue(CustomerVaultBase.Property_FriendsFamilyRep, FriendsFamilyRep);				
			info.AddValue(CustomerVaultBase.Property_Status, Status);				
			info.AddValue(CustomerVaultBase.Property_EquipmentStatus, EquipmentStatus);				
			info.AddValue(CustomerVaultBase.Property_isAccountOnline, isAccountOnline);				
			info.AddValue(CustomerVaultBase.Property_isAccountInService, isAccountInService);				
			info.AddValue(CustomerVaultBase.Property_CaseCount, CaseCount);				
			info.AddValue(CustomerVaultBase.Property_PointsUsedSales, PointsUsedSales);				
			info.AddValue(CustomerVaultBase.Property_LeadSource, LeadSource);				
			info.AddValue(CustomerVaultBase.Property_LeadSourceId, LeadSourceId);				
			info.AddValue(CustomerVaultBase.Property_isRepAccountHold, isRepAccountHold);				
			info.AddValue(CustomerVaultBase.Property_isTechAccountHold, isTechAccountHold);				
			info.AddValue(CustomerVaultBase.Property_DateEntered, DateEntered);				
			info.AddValue(CustomerVaultBase.Property_QA, QA);				
			info.AddValue(CustomerVaultBase.Property_QA2, QA2);				
			info.AddValue(CustomerVaultBase.Property_QA1Date, QA1Date);				
			info.AddValue(CustomerVaultBase.Property_QualityScore, QualityScore);				
			info.AddValue(CustomerVaultBase.Property_PreScreenDate, PreScreenDate);				
			info.AddValue(CustomerVaultBase.Property_FullReportDate, FullReportDate);				
			info.AddValue(CustomerVaultBase.Property_ContractTerm, ContractTerm);				
			info.AddValue(CustomerVaultBase.Property_BillingMethod, BillingMethod);				
			info.AddValue(CustomerVaultBase.Property_Takeover, Takeover);				
			info.AddValue(CustomerVaultBase.Property_DOB, DOB);				
			info.AddValue(CustomerVaultBase.Property_ContractId, ContractId);				
			info.AddValue(CustomerVaultBase.Property_TransactionID, TransactionID);				
		}
		#endregion

		
	}
}
