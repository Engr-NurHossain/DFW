using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "EmployeeBase", Namespace = "http://www.hims-tech.com//entities")]
	public class EmployeeBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			UserId = 1,
			UserName = 2,
			Title = 3,
			FirstName = 4,
			LastName = 5,
			Email = 6,
			Street = 7,
			City = 8,
			State = 9,
			ZipCode = 10,
			Country = 11,
			Phone = 12,
			SSN = 13,
			IsActive = 14,
			IsDeleted = 15,
			HireDate = 16,
			ProfilePicture = 17,
			Session = 18,
			JobTitle = 19,
			PlaceOfBirth = 20,
			SalesCommissionStructure = 21,
			TechCommissionStructure = 22,
			RecruitmentProcess = 23,
			Recruited = 24,
			IsCalendar = 25,
			CalendarColor = 26,
			CreatedDate = 27,
			Status = 28,
			LastUpdatedBy = 29,
			LastUpdatedDate = 30,
			IsSupervisor = 31,
			SuperVisorId = 32,
			HourlyRate = 33,
			NoAutoClockOut = 34,
			FireLicenseExpirationDate = 35,
			SalesLicenseExpirationDate = 36,
			InstallLicenseExpirationDate = 37,
			DriversLicenseExpirationDate = 38,
			ClockInIP = 39,
			DOB = 40,
			BasePay = 41,
			EmpType = 42,
			Department = 43,
			PtoRate = 44,
			PtoHour = 45,
			PtoRemain = 46,
			IsPayroll = 47,
			LicenseNo = 48,
			AnniversaryDate = 49,
			BadgerUserId = 50,
			AlarmId = 51,
			UserXComission = 52,
			IsCurrentEmployee = 53,
			CSId = 54,
			Street2 = 55,
			City2 = 56,
			State2 = 57,
			ZipCode2 = 58,
			StreetPrevious = 59,
			IsSalesMatrixUserX = 60,
			TerminationDate = 61,
			CompanyId = 62,
			TermSheetId = 63,
			BrinksDealerUser = 64,
			BrinksDealerPassword = 65,
			IsSalesMatrix = 66,
			IsDefaultInCalendar = 67,
			IsLocation = 68,
			PasswordUpdateDays = 69,
			PayType = 70
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_UserId = "UserId";		            
		public const string Property_UserName = "UserName";		            
		public const string Property_Title = "Title";		            
		public const string Property_FirstName = "FirstName";		            
		public const string Property_LastName = "LastName";		            
		public const string Property_Email = "Email";		            
		public const string Property_Street = "Street";		            
		public const string Property_City = "City";		            
		public const string Property_State = "State";		            
		public const string Property_ZipCode = "ZipCode";		            
		public const string Property_Country = "Country";		            
		public const string Property_Phone = "Phone";		            
		public const string Property_SSN = "SSN";		            
		public const string Property_IsActive = "IsActive";		            
		public const string Property_IsDeleted = "IsDeleted";		            
		public const string Property_HireDate = "HireDate";		            
		public const string Property_ProfilePicture = "ProfilePicture";		            
		public const string Property_Session = "Session";		            
		public const string Property_JobTitle = "JobTitle";		            
		public const string Property_PlaceOfBirth = "PlaceOfBirth";		            
		public const string Property_SalesCommissionStructure = "SalesCommissionStructure";		            
		public const string Property_TechCommissionStructure = "TechCommissionStructure";		            
		public const string Property_RecruitmentProcess = "RecruitmentProcess";		            
		public const string Property_Recruited = "Recruited";		            
		public const string Property_IsCalendar = "IsCalendar";		            
		public const string Property_CalendarColor = "CalendarColor";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_Status = "Status";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_IsSupervisor = "IsSupervisor";		            
		public const string Property_SuperVisorId = "SuperVisorId";		            
		public const string Property_HourlyRate = "HourlyRate";		            
		public const string Property_NoAutoClockOut = "NoAutoClockOut";		            
		public const string Property_FireLicenseExpirationDate = "FireLicenseExpirationDate";		            
		public const string Property_SalesLicenseExpirationDate = "SalesLicenseExpirationDate";		            
		public const string Property_InstallLicenseExpirationDate = "InstallLicenseExpirationDate";		            
		public const string Property_DriversLicenseExpirationDate = "DriversLicenseExpirationDate";		            
		public const string Property_ClockInIP = "ClockInIP";		            
		public const string Property_DOB = "DOB";		            
		public const string Property_BasePay = "BasePay";		            
		public const string Property_EmpType = "EmpType";		            
		public const string Property_Department = "Department";		            
		public const string Property_PtoRate = "PtoRate";		            
		public const string Property_PtoHour = "PtoHour";		            
		public const string Property_PtoRemain = "PtoRemain";		            
		public const string Property_IsPayroll = "IsPayroll";		            
		public const string Property_LicenseNo = "LicenseNo";		            
		public const string Property_AnniversaryDate = "AnniversaryDate";		            
		public const string Property_BadgerUserId = "BadgerUserId";		            
		public const string Property_AlarmId = "AlarmId";		            
		public const string Property_UserXComission = "UserXComission";		            
		public const string Property_IsCurrentEmployee = "IsCurrentEmployee";		            
		public const string Property_CSId = "CSId";		            
		public const string Property_Street2 = "Street2";		            
		public const string Property_City2 = "City2";		            
		public const string Property_State2 = "State2";		            
		public const string Property_ZipCode2 = "ZipCode2";		            
		public const string Property_StreetPrevious = "StreetPrevious";		            
		public const string Property_IsSalesMatrixUserX = "IsSalesMatrixUserX";		            
		public const string Property_TerminationDate = "TerminationDate";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_TermSheetId = "TermSheetId";		            
		public const string Property_BrinksDealerUser = "BrinksDealerUser";		            
		public const string Property_BrinksDealerPassword = "BrinksDealerPassword";		            
		public const string Property_IsSalesMatrix = "IsSalesMatrix";		            
		public const string Property_IsDefaultInCalendar = "IsDefaultInCalendar";		            
		public const string Property_IsLocation = "IsLocation";		            
		public const string Property_PasswordUpdateDays = "PasswordUpdateDays";		            
		public const string Property_PayType = "PayType";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _UserId;	            
		private String _UserName;	            
		private String _Title;	            
		private String _FirstName;	            
		private String _LastName;	            
		private String _Email;	            
		private String _Street;	            
		private String _City;	            
		private String _State;	            
		private String _ZipCode;	            
		private String _Country;	            
		private String _Phone;	            
		private String _SSN;	            
		private Boolean _IsActive;	            
		private Boolean _IsDeleted;	            
		private Nullable<DateTime> _HireDate;	            
		private String _ProfilePicture;	            
		private String _Session;	            
		private String _JobTitle;	            
		private String _PlaceOfBirth;	            
		private String _SalesCommissionStructure;	            
		private String _TechCommissionStructure;	            
		private Nullable<Boolean> _RecruitmentProcess;	            
		private Nullable<Boolean> _Recruited;	            
		private Nullable<Boolean> _IsCalendar;	            
		private String _CalendarColor;	            
		private Nullable<DateTime> _CreatedDate;	            
		private String _Status;	            
		private String _LastUpdatedBy;	            
		private Nullable<DateTime> _LastUpdatedDate;	            
		private Nullable<Boolean> _IsSupervisor;	            
		private String _SuperVisorId;	            
		private Nullable<Double> _HourlyRate;	            
		private Nullable<Boolean> _NoAutoClockOut;	            
		private Nullable<DateTime> _FireLicenseExpirationDate;	            
		private Nullable<DateTime> _SalesLicenseExpirationDate;	            
		private Nullable<DateTime> _InstallLicenseExpirationDate;	            
		private Nullable<DateTime> _DriversLicenseExpirationDate;	            
		private String _ClockInIP;	            
		private Nullable<DateTime> _DOB;	            
		private String _BasePay;	            
		private String _EmpType;	            
		private String _Department;	            
		private Nullable<Double> _PtoRate;	            
		private Nullable<Double> _PtoHour;	            
		private Nullable<Double> _PtoRemain;	            
		private Nullable<Boolean> _IsPayroll;	            
		private String _LicenseNo;	            
		private Nullable<DateTime> _AnniversaryDate;	            
		private String _BadgerUserId;	            
		private String _AlarmId;	            
		private Nullable<Double> _UserXComission;	            
		private Nullable<Boolean> _IsCurrentEmployee;	            
		private Nullable<Int32> _CSId;	            
		private String _Street2;	            
		private String _City2;	            
		private String _State2;	            
		private String _ZipCode2;	            
		private String _StreetPrevious;	            
		private Nullable<Boolean> _IsSalesMatrixUserX;	            
		private Nullable<DateTime> _TerminationDate;	            
		private Guid _CompanyId;	            
		private Guid _TermSheetId;	            
		private String _BrinksDealerUser;	            
		private String _BrinksDealerPassword;	            
		private Nullable<Boolean> _IsSalesMatrix;	            
		private Nullable<Boolean> _IsDefaultInCalendar;	            
		private Nullable<Boolean> _IsLocation;	            
		private Nullable<Int32> _PasswordUpdateDays;	            
		private String _PayType;	            
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
		public Guid UserId
		{	
			get{ return _UserId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UserId, value, _UserId);
				if (PropertyChanging(args))
				{
					_UserId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String UserName
		{	
			get{ return _UserName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UserName, value, _UserName);
				if (PropertyChanging(args))
				{
					_UserName = value;
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
		public Boolean IsActive
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
		public Boolean IsDeleted
		{	
			get{ return _IsDeleted; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsDeleted, value, _IsDeleted);
				if (PropertyChanging(args))
				{
					_IsDeleted = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> HireDate
		{	
			get{ return _HireDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_HireDate, value, _HireDate);
				if (PropertyChanging(args))
				{
					_HireDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ProfilePicture
		{	
			get{ return _ProfilePicture; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ProfilePicture, value, _ProfilePicture);
				if (PropertyChanging(args))
				{
					_ProfilePicture = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Session
		{	
			get{ return _Session; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Session, value, _Session);
				if (PropertyChanging(args))
				{
					_Session = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String JobTitle
		{	
			get{ return _JobTitle; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_JobTitle, value, _JobTitle);
				if (PropertyChanging(args))
				{
					_JobTitle = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PlaceOfBirth
		{	
			get{ return _PlaceOfBirth; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PlaceOfBirth, value, _PlaceOfBirth);
				if (PropertyChanging(args))
				{
					_PlaceOfBirth = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SalesCommissionStructure
		{	
			get{ return _SalesCommissionStructure; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SalesCommissionStructure, value, _SalesCommissionStructure);
				if (PropertyChanging(args))
				{
					_SalesCommissionStructure = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TechCommissionStructure
		{	
			get{ return _TechCommissionStructure; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TechCommissionStructure, value, _TechCommissionStructure);
				if (PropertyChanging(args))
				{
					_TechCommissionStructure = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> RecruitmentProcess
		{	
			get{ return _RecruitmentProcess; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RecruitmentProcess, value, _RecruitmentProcess);
				if (PropertyChanging(args))
				{
					_RecruitmentProcess = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> Recruited
		{	
			get{ return _Recruited; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Recruited, value, _Recruited);
				if (PropertyChanging(args))
				{
					_Recruited = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsCalendar
		{	
			get{ return _IsCalendar; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsCalendar, value, _IsCalendar);
				if (PropertyChanging(args))
				{
					_IsCalendar = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CalendarColor
		{	
			get{ return _CalendarColor; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CalendarColor, value, _CalendarColor);
				if (PropertyChanging(args))
				{
					_CalendarColor = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> CreatedDate
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
		public Nullable<DateTime> LastUpdatedDate
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
		public Nullable<Boolean> IsSupervisor
		{	
			get{ return _IsSupervisor; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsSupervisor, value, _IsSupervisor);
				if (PropertyChanging(args))
				{
					_IsSupervisor = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SuperVisorId
		{	
			get{ return _SuperVisorId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SuperVisorId, value, _SuperVisorId);
				if (PropertyChanging(args))
				{
					_SuperVisorId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> HourlyRate
		{	
			get{ return _HourlyRate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_HourlyRate, value, _HourlyRate);
				if (PropertyChanging(args))
				{
					_HourlyRate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> NoAutoClockOut
		{	
			get{ return _NoAutoClockOut; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_NoAutoClockOut, value, _NoAutoClockOut);
				if (PropertyChanging(args))
				{
					_NoAutoClockOut = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> FireLicenseExpirationDate
		{	
			get{ return _FireLicenseExpirationDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FireLicenseExpirationDate, value, _FireLicenseExpirationDate);
				if (PropertyChanging(args))
				{
					_FireLicenseExpirationDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> SalesLicenseExpirationDate
		{	
			get{ return _SalesLicenseExpirationDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SalesLicenseExpirationDate, value, _SalesLicenseExpirationDate);
				if (PropertyChanging(args))
				{
					_SalesLicenseExpirationDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> InstallLicenseExpirationDate
		{	
			get{ return _InstallLicenseExpirationDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InstallLicenseExpirationDate, value, _InstallLicenseExpirationDate);
				if (PropertyChanging(args))
				{
					_InstallLicenseExpirationDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> DriversLicenseExpirationDate
		{	
			get{ return _DriversLicenseExpirationDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DriversLicenseExpirationDate, value, _DriversLicenseExpirationDate);
				if (PropertyChanging(args))
				{
					_DriversLicenseExpirationDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ClockInIP
		{	
			get{ return _ClockInIP; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ClockInIP, value, _ClockInIP);
				if (PropertyChanging(args))
				{
					_ClockInIP = value;
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
		public String BasePay
		{	
			get{ return _BasePay; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BasePay, value, _BasePay);
				if (PropertyChanging(args))
				{
					_BasePay = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String EmpType
		{	
			get{ return _EmpType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EmpType, value, _EmpType);
				if (PropertyChanging(args))
				{
					_EmpType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Department
		{	
			get{ return _Department; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Department, value, _Department);
				if (PropertyChanging(args))
				{
					_Department = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> PtoRate
		{	
			get{ return _PtoRate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PtoRate, value, _PtoRate);
				if (PropertyChanging(args))
				{
					_PtoRate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> PtoHour
		{	
			get{ return _PtoHour; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PtoHour, value, _PtoHour);
				if (PropertyChanging(args))
				{
					_PtoHour = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> PtoRemain
		{	
			get{ return _PtoRemain; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PtoRemain, value, _PtoRemain);
				if (PropertyChanging(args))
				{
					_PtoRemain = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsPayroll
		{	
			get{ return _IsPayroll; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsPayroll, value, _IsPayroll);
				if (PropertyChanging(args))
				{
					_IsPayroll = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String LicenseNo
		{	
			get{ return _LicenseNo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LicenseNo, value, _LicenseNo);
				if (PropertyChanging(args))
				{
					_LicenseNo = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> AnniversaryDate
		{	
			get{ return _AnniversaryDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AnniversaryDate, value, _AnniversaryDate);
				if (PropertyChanging(args))
				{
					_AnniversaryDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BadgerUserId
		{	
			get{ return _BadgerUserId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BadgerUserId, value, _BadgerUserId);
				if (PropertyChanging(args))
				{
					_BadgerUserId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AlarmId
		{	
			get{ return _AlarmId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AlarmId, value, _AlarmId);
				if (PropertyChanging(args))
				{
					_AlarmId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> UserXComission
		{	
			get{ return _UserXComission; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UserXComission, value, _UserXComission);
				if (PropertyChanging(args))
				{
					_UserXComission = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsCurrentEmployee
		{	
			get{ return _IsCurrentEmployee; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsCurrentEmployee, value, _IsCurrentEmployee);
				if (PropertyChanging(args))
				{
					_IsCurrentEmployee = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> CSId
		{	
			get{ return _CSId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CSId, value, _CSId);
				if (PropertyChanging(args))
				{
					_CSId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Street2
		{	
			get{ return _Street2; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Street2, value, _Street2);
				if (PropertyChanging(args))
				{
					_Street2 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String City2
		{	
			get{ return _City2; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_City2, value, _City2);
				if (PropertyChanging(args))
				{
					_City2 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String State2
		{	
			get{ return _State2; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_State2, value, _State2);
				if (PropertyChanging(args))
				{
					_State2 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ZipCode2
		{	
			get{ return _ZipCode2; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ZipCode2, value, _ZipCode2);
				if (PropertyChanging(args))
				{
					_ZipCode2 = value;
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
		public Nullable<Boolean> IsSalesMatrixUserX
		{	
			get{ return _IsSalesMatrixUserX; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsSalesMatrixUserX, value, _IsSalesMatrixUserX);
				if (PropertyChanging(args))
				{
					_IsSalesMatrixUserX = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> TerminationDate
		{	
			get{ return _TerminationDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TerminationDate, value, _TerminationDate);
				if (PropertyChanging(args))
				{
					_TerminationDate = value;
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
		public Guid TermSheetId
		{	
			get{ return _TermSheetId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TermSheetId, value, _TermSheetId);
				if (PropertyChanging(args))
				{
					_TermSheetId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BrinksDealerUser
		{	
			get{ return _BrinksDealerUser; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BrinksDealerUser, value, _BrinksDealerUser);
				if (PropertyChanging(args))
				{
					_BrinksDealerUser = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BrinksDealerPassword
		{	
			get{ return _BrinksDealerPassword; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BrinksDealerPassword, value, _BrinksDealerPassword);
				if (PropertyChanging(args))
				{
					_BrinksDealerPassword = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsSalesMatrix
		{	
			get{ return _IsSalesMatrix; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsSalesMatrix, value, _IsSalesMatrix);
				if (PropertyChanging(args))
				{
					_IsSalesMatrix = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsDefaultInCalendar
		{	
			get{ return _IsDefaultInCalendar; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsDefaultInCalendar, value, _IsDefaultInCalendar);
				if (PropertyChanging(args))
				{
					_IsDefaultInCalendar = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsLocation
		{	
			get{ return _IsLocation; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsLocation, value, _IsLocation);
				if (PropertyChanging(args))
				{
					_IsLocation = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> PasswordUpdateDays
		{	
			get{ return _PasswordUpdateDays; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PasswordUpdateDays, value, _PasswordUpdateDays);
				if (PropertyChanging(args))
				{
					_PasswordUpdateDays = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PayType
		{	
			get{ return _PayType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PayType, value, _PayType);
				if (PropertyChanging(args))
				{
					_PayType = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  EmployeeBase Clone()
		{
			EmployeeBase newObj = new  EmployeeBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.UserId = this.UserId;						
			newObj.UserName = this.UserName;						
			newObj.Title = this.Title;						
			newObj.FirstName = this.FirstName;						
			newObj.LastName = this.LastName;						
			newObj.Email = this.Email;						
			newObj.Street = this.Street;						
			newObj.City = this.City;						
			newObj.State = this.State;						
			newObj.ZipCode = this.ZipCode;						
			newObj.Country = this.Country;						
			newObj.Phone = this.Phone;						
			newObj.SSN = this.SSN;						
			newObj.IsActive = this.IsActive;						
			newObj.IsDeleted = this.IsDeleted;						
			newObj.HireDate = this.HireDate;						
			newObj.ProfilePicture = this.ProfilePicture;						
			newObj.Session = this.Session;						
			newObj.JobTitle = this.JobTitle;						
			newObj.PlaceOfBirth = this.PlaceOfBirth;						
			newObj.SalesCommissionStructure = this.SalesCommissionStructure;						
			newObj.TechCommissionStructure = this.TechCommissionStructure;						
			newObj.RecruitmentProcess = this.RecruitmentProcess;						
			newObj.Recruited = this.Recruited;						
			newObj.IsCalendar = this.IsCalendar;						
			newObj.CalendarColor = this.CalendarColor;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.Status = this.Status;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.IsSupervisor = this.IsSupervisor;						
			newObj.SuperVisorId = this.SuperVisorId;						
			newObj.HourlyRate = this.HourlyRate;						
			newObj.NoAutoClockOut = this.NoAutoClockOut;						
			newObj.FireLicenseExpirationDate = this.FireLicenseExpirationDate;						
			newObj.SalesLicenseExpirationDate = this.SalesLicenseExpirationDate;						
			newObj.InstallLicenseExpirationDate = this.InstallLicenseExpirationDate;						
			newObj.DriversLicenseExpirationDate = this.DriversLicenseExpirationDate;						
			newObj.ClockInIP = this.ClockInIP;						
			newObj.DOB = this.DOB;						
			newObj.BasePay = this.BasePay;						
			newObj.EmpType = this.EmpType;						
			newObj.Department = this.Department;						
			newObj.PtoRate = this.PtoRate;						
			newObj.PtoHour = this.PtoHour;						
			newObj.PtoRemain = this.PtoRemain;						
			newObj.IsPayroll = this.IsPayroll;						
			newObj.LicenseNo = this.LicenseNo;						
			newObj.AnniversaryDate = this.AnniversaryDate;						
			newObj.BadgerUserId = this.BadgerUserId;						
			newObj.AlarmId = this.AlarmId;						
			newObj.UserXComission = this.UserXComission;						
			newObj.IsCurrentEmployee = this.IsCurrentEmployee;						
			newObj.CSId = this.CSId;						
			newObj.Street2 = this.Street2;						
			newObj.City2 = this.City2;						
			newObj.State2 = this.State2;						
			newObj.ZipCode2 = this.ZipCode2;						
			newObj.StreetPrevious = this.StreetPrevious;						
			newObj.IsSalesMatrixUserX = this.IsSalesMatrixUserX;						
			newObj.TerminationDate = this.TerminationDate;						
			newObj.CompanyId = this.CompanyId;						
			newObj.TermSheetId = this.TermSheetId;						
			newObj.BrinksDealerUser = this.BrinksDealerUser;						
			newObj.BrinksDealerPassword = this.BrinksDealerPassword;						
			newObj.IsSalesMatrix = this.IsSalesMatrix;						
			newObj.IsDefaultInCalendar = this.IsDefaultInCalendar;						
			newObj.IsLocation = this.IsLocation;						
			newObj.PasswordUpdateDays = this.PasswordUpdateDays;						
			newObj.PayType = this.PayType;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(EmployeeBase.Property_Id, Id);				
			info.AddValue(EmployeeBase.Property_UserId, UserId);				
			info.AddValue(EmployeeBase.Property_UserName, UserName);				
			info.AddValue(EmployeeBase.Property_Title, Title);				
			info.AddValue(EmployeeBase.Property_FirstName, FirstName);				
			info.AddValue(EmployeeBase.Property_LastName, LastName);				
			info.AddValue(EmployeeBase.Property_Email, Email);				
			info.AddValue(EmployeeBase.Property_Street, Street);				
			info.AddValue(EmployeeBase.Property_City, City);				
			info.AddValue(EmployeeBase.Property_State, State);				
			info.AddValue(EmployeeBase.Property_ZipCode, ZipCode);				
			info.AddValue(EmployeeBase.Property_Country, Country);				
			info.AddValue(EmployeeBase.Property_Phone, Phone);				
			info.AddValue(EmployeeBase.Property_SSN, SSN);				
			info.AddValue(EmployeeBase.Property_IsActive, IsActive);				
			info.AddValue(EmployeeBase.Property_IsDeleted, IsDeleted);				
			info.AddValue(EmployeeBase.Property_HireDate, HireDate);				
			info.AddValue(EmployeeBase.Property_ProfilePicture, ProfilePicture);				
			info.AddValue(EmployeeBase.Property_Session, Session);				
			info.AddValue(EmployeeBase.Property_JobTitle, JobTitle);				
			info.AddValue(EmployeeBase.Property_PlaceOfBirth, PlaceOfBirth);				
			info.AddValue(EmployeeBase.Property_SalesCommissionStructure, SalesCommissionStructure);				
			info.AddValue(EmployeeBase.Property_TechCommissionStructure, TechCommissionStructure);				
			info.AddValue(EmployeeBase.Property_RecruitmentProcess, RecruitmentProcess);				
			info.AddValue(EmployeeBase.Property_Recruited, Recruited);				
			info.AddValue(EmployeeBase.Property_IsCalendar, IsCalendar);				
			info.AddValue(EmployeeBase.Property_CalendarColor, CalendarColor);				
			info.AddValue(EmployeeBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(EmployeeBase.Property_Status, Status);				
			info.AddValue(EmployeeBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(EmployeeBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(EmployeeBase.Property_IsSupervisor, IsSupervisor);				
			info.AddValue(EmployeeBase.Property_SuperVisorId, SuperVisorId);				
			info.AddValue(EmployeeBase.Property_HourlyRate, HourlyRate);				
			info.AddValue(EmployeeBase.Property_NoAutoClockOut, NoAutoClockOut);				
			info.AddValue(EmployeeBase.Property_FireLicenseExpirationDate, FireLicenseExpirationDate);				
			info.AddValue(EmployeeBase.Property_SalesLicenseExpirationDate, SalesLicenseExpirationDate);				
			info.AddValue(EmployeeBase.Property_InstallLicenseExpirationDate, InstallLicenseExpirationDate);				
			info.AddValue(EmployeeBase.Property_DriversLicenseExpirationDate, DriversLicenseExpirationDate);				
			info.AddValue(EmployeeBase.Property_ClockInIP, ClockInIP);				
			info.AddValue(EmployeeBase.Property_DOB, DOB);				
			info.AddValue(EmployeeBase.Property_BasePay, BasePay);				
			info.AddValue(EmployeeBase.Property_EmpType, EmpType);				
			info.AddValue(EmployeeBase.Property_Department, Department);				
			info.AddValue(EmployeeBase.Property_PtoRate, PtoRate);				
			info.AddValue(EmployeeBase.Property_PtoHour, PtoHour);				
			info.AddValue(EmployeeBase.Property_PtoRemain, PtoRemain);				
			info.AddValue(EmployeeBase.Property_IsPayroll, IsPayroll);				
			info.AddValue(EmployeeBase.Property_LicenseNo, LicenseNo);				
			info.AddValue(EmployeeBase.Property_AnniversaryDate, AnniversaryDate);				
			info.AddValue(EmployeeBase.Property_BadgerUserId, BadgerUserId);				
			info.AddValue(EmployeeBase.Property_AlarmId, AlarmId);				
			info.AddValue(EmployeeBase.Property_UserXComission, UserXComission);				
			info.AddValue(EmployeeBase.Property_IsCurrentEmployee, IsCurrentEmployee);				
			info.AddValue(EmployeeBase.Property_CSId, CSId);				
			info.AddValue(EmployeeBase.Property_Street2, Street2);				
			info.AddValue(EmployeeBase.Property_City2, City2);				
			info.AddValue(EmployeeBase.Property_State2, State2);				
			info.AddValue(EmployeeBase.Property_ZipCode2, ZipCode2);				
			info.AddValue(EmployeeBase.Property_StreetPrevious, StreetPrevious);				
			info.AddValue(EmployeeBase.Property_IsSalesMatrixUserX, IsSalesMatrixUserX);				
			info.AddValue(EmployeeBase.Property_TerminationDate, TerminationDate);				
			info.AddValue(EmployeeBase.Property_CompanyId, CompanyId);				
			info.AddValue(EmployeeBase.Property_TermSheetId, TermSheetId);				
			info.AddValue(EmployeeBase.Property_BrinksDealerUser, BrinksDealerUser);				
			info.AddValue(EmployeeBase.Property_BrinksDealerPassword, BrinksDealerPassword);				
			info.AddValue(EmployeeBase.Property_IsSalesMatrix, IsSalesMatrix);				
			info.AddValue(EmployeeBase.Property_IsDefaultInCalendar, IsDefaultInCalendar);				
			info.AddValue(EmployeeBase.Property_IsLocation, IsLocation);				
			info.AddValue(EmployeeBase.Property_PasswordUpdateDays, PasswordUpdateDays);				
			info.AddValue(EmployeeBase.Property_PayType, PayType);				
		}
		#endregion

		
	}
}
