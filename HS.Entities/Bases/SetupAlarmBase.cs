using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "SetupAlarmBase", Namespace = "http://www.piistech.com//entities")]
	public class SetupAlarmBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CustomerId = 1,
			CompanyId = 2,
			PropertyType = 3,
			EmailAddress = 4,
			Phone = 5,
			DealerCustomer = 6,
			LoginName = 7,
			Password = 8,
			InsStreet = 9,
			InsState = 10,
			InsCity = 11,
			InsZip = 12,
			InsCountry = 13,
			InsTimeZone = 14,
			Culture = 15,
			PanelType = 16,
			PanelVersion = 17,
			ModelSerialNumber = 18,
			PhoneLinePresent = 19,
			CentrastationForwardingOption = 20,
			CentralStationAccountNo = 21,
			CentralStationRecieverNumber = 22,
			PackageId = 23,
			IgnoreLowCoverageError = 24,
			CustomerStatus = 25,
			CreatedBy = 26,
			CreatedDate = 27,
			LastUpdatedBy = 28,
			LastUpdatedDate = 29
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_PropertyType = "PropertyType";		            
		public const string Property_EmailAddress = "EmailAddress";		            
		public const string Property_Phone = "Phone";		            
		public const string Property_DealerCustomer = "DealerCustomer";		            
		public const string Property_LoginName = "LoginName";		            
		public const string Property_Password = "Password";		            
		public const string Property_InsStreet = "InsStreet";		            
		public const string Property_InsState = "InsState";		            
		public const string Property_InsCity = "InsCity";		            
		public const string Property_InsZip = "InsZip";		            
		public const string Property_InsCountry = "InsCountry";		            
		public const string Property_InsTimeZone = "InsTimeZone";		            
		public const string Property_Culture = "Culture";		            
		public const string Property_PanelType = "PanelType";		            
		public const string Property_PanelVersion = "PanelVersion";		            
		public const string Property_ModelSerialNumber = "ModelSerialNumber";		            
		public const string Property_PhoneLinePresent = "PhoneLinePresent";		            
		public const string Property_CentrastationForwardingOption = "CentrastationForwardingOption";		            
		public const string Property_CentralStationAccountNo = "CentralStationAccountNo";		            
		public const string Property_CentralStationRecieverNumber = "CentralStationRecieverNumber";		            
		public const string Property_PackageId = "PackageId";		            
		public const string Property_IgnoreLowCoverageError = "IgnoreLowCoverageError";		            
		public const string Property_CustomerStatus = "CustomerStatus";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CustomerId;	            
		private Guid _CompanyId;	            
		private String _PropertyType;	            
		private String _EmailAddress;	            
		private String _Phone;	            
		private String _DealerCustomer;	            
		private String _LoginName;	            
		private String _Password;	            
		private String _InsStreet;	            
		private String _InsState;	            
		private String _InsCity;	            
		private String _InsZip;	            
		private String _InsCountry;	            
		private String _InsTimeZone;	            
		private String _Culture;	            
		private String _PanelType;	            
		private String _PanelVersion;	            
		private String _ModelSerialNumber;	            
		private Nullable<Boolean> _PhoneLinePresent;	            
		private String _CentrastationForwardingOption;	            
		private String _CentralStationAccountNo;	            
		private String _CentralStationRecieverNumber;	            
		private Nullable<Int32> _PackageId;	            
		private Nullable<Boolean> _IgnoreLowCoverageError;	            
		private String _CustomerStatus;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
		private Guid _LastUpdatedBy;	            
		private DateTime _LastUpdatedDate;	            
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
		public String PropertyType
		{	
			get{ return _PropertyType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PropertyType, value, _PropertyType);
				if (PropertyChanging(args))
				{
					_PropertyType = value;
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
		public String DealerCustomer
		{	
			get{ return _DealerCustomer; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DealerCustomer, value, _DealerCustomer);
				if (PropertyChanging(args))
				{
					_DealerCustomer = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String LoginName
		{	
			get{ return _LoginName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LoginName, value, _LoginName);
				if (PropertyChanging(args))
				{
					_LoginName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Password
		{	
			get{ return _Password; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Password, value, _Password);
				if (PropertyChanging(args))
				{
					_Password = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String InsStreet
		{	
			get{ return _InsStreet; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InsStreet, value, _InsStreet);
				if (PropertyChanging(args))
				{
					_InsStreet = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String InsState
		{	
			get{ return _InsState; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InsState, value, _InsState);
				if (PropertyChanging(args))
				{
					_InsState = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String InsCity
		{	
			get{ return _InsCity; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InsCity, value, _InsCity);
				if (PropertyChanging(args))
				{
					_InsCity = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String InsZip
		{	
			get{ return _InsZip; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InsZip, value, _InsZip);
				if (PropertyChanging(args))
				{
					_InsZip = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String InsCountry
		{	
			get{ return _InsCountry; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InsCountry, value, _InsCountry);
				if (PropertyChanging(args))
				{
					_InsCountry = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String InsTimeZone
		{	
			get{ return _InsTimeZone; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InsTimeZone, value, _InsTimeZone);
				if (PropertyChanging(args))
				{
					_InsTimeZone = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Culture
		{	
			get{ return _Culture; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Culture, value, _Culture);
				if (PropertyChanging(args))
				{
					_Culture = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PanelType
		{	
			get{ return _PanelType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PanelType, value, _PanelType);
				if (PropertyChanging(args))
				{
					_PanelType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PanelVersion
		{	
			get{ return _PanelVersion; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PanelVersion, value, _PanelVersion);
				if (PropertyChanging(args))
				{
					_PanelVersion = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ModelSerialNumber
		{	
			get{ return _ModelSerialNumber; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ModelSerialNumber, value, _ModelSerialNumber);
				if (PropertyChanging(args))
				{
					_ModelSerialNumber = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> PhoneLinePresent
		{	
			get{ return _PhoneLinePresent; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PhoneLinePresent, value, _PhoneLinePresent);
				if (PropertyChanging(args))
				{
					_PhoneLinePresent = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CentrastationForwardingOption
		{	
			get{ return _CentrastationForwardingOption; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CentrastationForwardingOption, value, _CentrastationForwardingOption);
				if (PropertyChanging(args))
				{
					_CentrastationForwardingOption = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CentralStationAccountNo
		{	
			get{ return _CentralStationAccountNo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CentralStationAccountNo, value, _CentralStationAccountNo);
				if (PropertyChanging(args))
				{
					_CentralStationAccountNo = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CentralStationRecieverNumber
		{	
			get{ return _CentralStationRecieverNumber; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CentralStationRecieverNumber, value, _CentralStationRecieverNumber);
				if (PropertyChanging(args))
				{
					_CentralStationRecieverNumber = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> PackageId
		{	
			get{ return _PackageId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PackageId, value, _PackageId);
				if (PropertyChanging(args))
				{
					_PackageId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IgnoreLowCoverageError
		{	
			get{ return _IgnoreLowCoverageError; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IgnoreLowCoverageError, value, _IgnoreLowCoverageError);
				if (PropertyChanging(args))
				{
					_IgnoreLowCoverageError = value;
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
		public Guid CreatedBy
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
		public Guid LastUpdatedBy
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

		#endregion
		
		#region Cloning Base Objects
		public  SetupAlarmBase Clone()
		{
			SetupAlarmBase newObj = new  SetupAlarmBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CustomerId = this.CustomerId;						
			newObj.CompanyId = this.CompanyId;						
			newObj.PropertyType = this.PropertyType;						
			newObj.EmailAddress = this.EmailAddress;						
			newObj.Phone = this.Phone;						
			newObj.DealerCustomer = this.DealerCustomer;						
			newObj.LoginName = this.LoginName;						
			newObj.Password = this.Password;						
			newObj.InsStreet = this.InsStreet;						
			newObj.InsState = this.InsState;						
			newObj.InsCity = this.InsCity;						
			newObj.InsZip = this.InsZip;						
			newObj.InsCountry = this.InsCountry;						
			newObj.InsTimeZone = this.InsTimeZone;						
			newObj.Culture = this.Culture;						
			newObj.PanelType = this.PanelType;						
			newObj.PanelVersion = this.PanelVersion;						
			newObj.ModelSerialNumber = this.ModelSerialNumber;						
			newObj.PhoneLinePresent = this.PhoneLinePresent;						
			newObj.CentrastationForwardingOption = this.CentrastationForwardingOption;						
			newObj.CentralStationAccountNo = this.CentralStationAccountNo;						
			newObj.CentralStationRecieverNumber = this.CentralStationRecieverNumber;						
			newObj.PackageId = this.PackageId;						
			newObj.IgnoreLowCoverageError = this.IgnoreLowCoverageError;						
			newObj.CustomerStatus = this.CustomerStatus;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(SetupAlarmBase.Property_Id, Id);				
			info.AddValue(SetupAlarmBase.Property_CustomerId, CustomerId);				
			info.AddValue(SetupAlarmBase.Property_CompanyId, CompanyId);				
			info.AddValue(SetupAlarmBase.Property_PropertyType, PropertyType);				
			info.AddValue(SetupAlarmBase.Property_EmailAddress, EmailAddress);				
			info.AddValue(SetupAlarmBase.Property_Phone, Phone);				
			info.AddValue(SetupAlarmBase.Property_DealerCustomer, DealerCustomer);				
			info.AddValue(SetupAlarmBase.Property_LoginName, LoginName);				
			info.AddValue(SetupAlarmBase.Property_Password, Password);				
			info.AddValue(SetupAlarmBase.Property_InsStreet, InsStreet);				
			info.AddValue(SetupAlarmBase.Property_InsState, InsState);				
			info.AddValue(SetupAlarmBase.Property_InsCity, InsCity);				
			info.AddValue(SetupAlarmBase.Property_InsZip, InsZip);				
			info.AddValue(SetupAlarmBase.Property_InsCountry, InsCountry);				
			info.AddValue(SetupAlarmBase.Property_InsTimeZone, InsTimeZone);				
			info.AddValue(SetupAlarmBase.Property_Culture, Culture);				
			info.AddValue(SetupAlarmBase.Property_PanelType, PanelType);				
			info.AddValue(SetupAlarmBase.Property_PanelVersion, PanelVersion);				
			info.AddValue(SetupAlarmBase.Property_ModelSerialNumber, ModelSerialNumber);				
			info.AddValue(SetupAlarmBase.Property_PhoneLinePresent, PhoneLinePresent);				
			info.AddValue(SetupAlarmBase.Property_CentrastationForwardingOption, CentrastationForwardingOption);				
			info.AddValue(SetupAlarmBase.Property_CentralStationAccountNo, CentralStationAccountNo);				
			info.AddValue(SetupAlarmBase.Property_CentralStationRecieverNumber, CentralStationRecieverNumber);				
			info.AddValue(SetupAlarmBase.Property_PackageId, PackageId);				
			info.AddValue(SetupAlarmBase.Property_IgnoreLowCoverageError, IgnoreLowCoverageError);				
			info.AddValue(SetupAlarmBase.Property_CustomerStatus, CustomerStatus);				
			info.AddValue(SetupAlarmBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(SetupAlarmBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(SetupAlarmBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(SetupAlarmBase.Property_LastUpdatedDate, LastUpdatedDate);				
		}
		#endregion

		
	}
}
