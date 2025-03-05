using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "SmartPackageBase", Namespace = "http://www.hims-tech.com//entities")]
	public class SmartPackageBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			PackageId = 1,
			CompanyId = 2,
			SmartSystemTypeId = 3,
			SmartInstallTypeId = 4,
			PackageName = 5,
			EquipmentMaxLimit = 6,
			ActivationFee = 7,
			IsActive = 8,
			IsPromo = 9,
			StartDate = 10,
			EndDate = 11,
			TotalRMR = 12,
			LastUpdatedBy = 13,
			LastUpdatedDate = 14,
			NonConforming = 15,
			MinCredit = 16,
			MaxCredit = 17,
			ManufacturerId = 18,
			PackageCode = 19,
			UserType = 20,
			ConformingFee = 21,
			PackageType = 22,
			IsDelete = 23,
			CustomerNumber = 24
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_PackageId = "PackageId";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_SmartSystemTypeId = "SmartSystemTypeId";		            
		public const string Property_SmartInstallTypeId = "SmartInstallTypeId";		            
		public const string Property_PackageName = "PackageName";		            
		public const string Property_EquipmentMaxLimit = "EquipmentMaxLimit";		            
		public const string Property_ActivationFee = "ActivationFee";		            
		public const string Property_IsActive = "IsActive";		            
		public const string Property_IsPromo = "IsPromo";		            
		public const string Property_StartDate = "StartDate";		            
		public const string Property_EndDate = "EndDate";		            
		public const string Property_TotalRMR = "TotalRMR";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_NonConforming = "NonConforming";		            
		public const string Property_MinCredit = "MinCredit";		            
		public const string Property_MaxCredit = "MaxCredit";		            
		public const string Property_ManufacturerId = "ManufacturerId";		            
		public const string Property_PackageCode = "PackageCode";		            
		public const string Property_UserType = "UserType";		            
		public const string Property_ConformingFee = "ConformingFee";		            
		public const string Property_PackageType = "PackageType";		            
		public const string Property_IsDelete = "IsDelete";		            
		public const string Property_CustomerNumber = "CustomerNumber";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _PackageId;	            
		private Guid _CompanyId;	            
		private Int32 _SmartSystemTypeId;	            
		private Int32 _SmartInstallTypeId;	            
		private String _PackageName;	            
		private Int32 _EquipmentMaxLimit;	            
		private Nullable<Double> _ActivationFee;	            
		private Nullable<Boolean> _IsActive;	            
		private Nullable<Boolean> _IsPromo;	            
		private Nullable<DateTime> _StartDate;	            
		private Nullable<DateTime> _EndDate;	            
		private Nullable<Double> _TotalRMR;	            
		private Guid _LastUpdatedBy;	            
		private Nullable<DateTime> _LastUpdatedDate;	            
		private Nullable<Boolean> _NonConforming;	            
		private Nullable<Double> _MinCredit;	            
		private Nullable<Double> _MaxCredit;	            
		private Guid _ManufacturerId;	            
		private String _PackageCode;	            
		private String _UserType;	            
		private Nullable<Double> _ConformingFee;	            
		private String _PackageType;	            
		private Nullable<Boolean> _IsDelete;	            
		private String _CustomerNumber;	            
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
		public Guid PackageId
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
		public Int32 SmartSystemTypeId
		{	
			get{ return _SmartSystemTypeId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SmartSystemTypeId, value, _SmartSystemTypeId);
				if (PropertyChanging(args))
				{
					_SmartSystemTypeId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Int32 SmartInstallTypeId
		{	
			get{ return _SmartInstallTypeId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SmartInstallTypeId, value, _SmartInstallTypeId);
				if (PropertyChanging(args))
				{
					_SmartInstallTypeId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PackageName
		{	
			get{ return _PackageName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PackageName, value, _PackageName);
				if (PropertyChanging(args))
				{
					_PackageName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Int32 EquipmentMaxLimit
		{	
			get{ return _EquipmentMaxLimit; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EquipmentMaxLimit, value, _EquipmentMaxLimit);
				if (PropertyChanging(args))
				{
					_EquipmentMaxLimit = value;
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
		public Nullable<Boolean> IsPromo
		{	
			get{ return _IsPromo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsPromo, value, _IsPromo);
				if (PropertyChanging(args))
				{
					_IsPromo = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> StartDate
		{	
			get{ return _StartDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_StartDate, value, _StartDate);
				if (PropertyChanging(args))
				{
					_StartDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> EndDate
		{	
			get{ return _EndDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EndDate, value, _EndDate);
				if (PropertyChanging(args))
				{
					_EndDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> TotalRMR
		{	
			get{ return _TotalRMR; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TotalRMR, value, _TotalRMR);
				if (PropertyChanging(args))
				{
					_TotalRMR = value;
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
		public Nullable<Boolean> NonConforming
		{	
			get{ return _NonConforming; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_NonConforming, value, _NonConforming);
				if (PropertyChanging(args))
				{
					_NonConforming = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> MinCredit
		{	
			get{ return _MinCredit; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MinCredit, value, _MinCredit);
				if (PropertyChanging(args))
				{
					_MinCredit = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> MaxCredit
		{	
			get{ return _MaxCredit; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MaxCredit, value, _MaxCredit);
				if (PropertyChanging(args))
				{
					_MaxCredit = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid ManufacturerId
		{	
			get{ return _ManufacturerId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ManufacturerId, value, _ManufacturerId);
				if (PropertyChanging(args))
				{
					_ManufacturerId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PackageCode
		{	
			get{ return _PackageCode; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PackageCode, value, _PackageCode);
				if (PropertyChanging(args))
				{
					_PackageCode = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String UserType
		{	
			get{ return _UserType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UserType, value, _UserType);
				if (PropertyChanging(args))
				{
					_UserType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> ConformingFee
		{	
			get{ return _ConformingFee; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ConformingFee, value, _ConformingFee);
				if (PropertyChanging(args))
				{
					_ConformingFee = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PackageType
		{	
			get{ return _PackageType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PackageType, value, _PackageType);
				if (PropertyChanging(args))
				{
					_PackageType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsDelete
		{	
			get{ return _IsDelete; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsDelete, value, _IsDelete);
				if (PropertyChanging(args))
				{
					_IsDelete = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CustomerNumber
		{	
			get{ return _CustomerNumber; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CustomerNumber, value, _CustomerNumber);
				if (PropertyChanging(args))
				{
					_CustomerNumber = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  SmartPackageBase Clone()
		{
			SmartPackageBase newObj = new  SmartPackageBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.PackageId = this.PackageId;						
			newObj.CompanyId = this.CompanyId;						
			newObj.SmartSystemTypeId = this.SmartSystemTypeId;						
			newObj.SmartInstallTypeId = this.SmartInstallTypeId;						
			newObj.PackageName = this.PackageName;						
			newObj.EquipmentMaxLimit = this.EquipmentMaxLimit;						
			newObj.ActivationFee = this.ActivationFee;						
			newObj.IsActive = this.IsActive;						
			newObj.IsPromo = this.IsPromo;						
			newObj.StartDate = this.StartDate;						
			newObj.EndDate = this.EndDate;						
			newObj.TotalRMR = this.TotalRMR;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.NonConforming = this.NonConforming;						
			newObj.MinCredit = this.MinCredit;						
			newObj.MaxCredit = this.MaxCredit;						
			newObj.ManufacturerId = this.ManufacturerId;						
			newObj.PackageCode = this.PackageCode;						
			newObj.UserType = this.UserType;						
			newObj.ConformingFee = this.ConformingFee;						
			newObj.PackageType = this.PackageType;						
			newObj.IsDelete = this.IsDelete;						
			newObj.CustomerNumber = this.CustomerNumber;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(SmartPackageBase.Property_Id, Id);				
			info.AddValue(SmartPackageBase.Property_PackageId, PackageId);				
			info.AddValue(SmartPackageBase.Property_CompanyId, CompanyId);				
			info.AddValue(SmartPackageBase.Property_SmartSystemTypeId, SmartSystemTypeId);				
			info.AddValue(SmartPackageBase.Property_SmartInstallTypeId, SmartInstallTypeId);				
			info.AddValue(SmartPackageBase.Property_PackageName, PackageName);				
			info.AddValue(SmartPackageBase.Property_EquipmentMaxLimit, EquipmentMaxLimit);				
			info.AddValue(SmartPackageBase.Property_ActivationFee, ActivationFee);				
			info.AddValue(SmartPackageBase.Property_IsActive, IsActive);				
			info.AddValue(SmartPackageBase.Property_IsPromo, IsPromo);				
			info.AddValue(SmartPackageBase.Property_StartDate, StartDate);				
			info.AddValue(SmartPackageBase.Property_EndDate, EndDate);				
			info.AddValue(SmartPackageBase.Property_TotalRMR, TotalRMR);				
			info.AddValue(SmartPackageBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(SmartPackageBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(SmartPackageBase.Property_NonConforming, NonConforming);				
			info.AddValue(SmartPackageBase.Property_MinCredit, MinCredit);				
			info.AddValue(SmartPackageBase.Property_MaxCredit, MaxCredit);				
			info.AddValue(SmartPackageBase.Property_ManufacturerId, ManufacturerId);				
			info.AddValue(SmartPackageBase.Property_PackageCode, PackageCode);				
			info.AddValue(SmartPackageBase.Property_UserType, UserType);				
			info.AddValue(SmartPackageBase.Property_ConformingFee, ConformingFee);				
			info.AddValue(SmartPackageBase.Property_PackageType, PackageType);				
			info.AddValue(SmartPackageBase.Property_IsDelete, IsDelete);				
			info.AddValue(SmartPackageBase.Property_CustomerNumber, CustomerNumber);				
		}
		#endregion

		
	}
}
