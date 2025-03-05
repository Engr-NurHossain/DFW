using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomerPackageServiceBase", Namespace = "http://www.hims-tech.com//entities")]
	public class CustomerPackageServiceBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			CustomerId = 2,
			PackageId = 3,
			EquipmentId = 4,
			MonthlyRate = 5,
			DiscountRate = 6,
			Total = 7,
			ManufacturerId = 8,
			LocationId = 9,
			TypeId = 10,
			ModelId = 11,
			FinishId = 12,
			CapacityId = 13,
			IsPackageService = 14,
			IsNonCommissionable = 15,
			AppointmentIntId = 16,
			IsInvoice = 17,
			AppointmentEquipmentIntId = 18
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_PackageId = "PackageId";		            
		public const string Property_EquipmentId = "EquipmentId";		            
		public const string Property_MonthlyRate = "MonthlyRate";		            
		public const string Property_DiscountRate = "DiscountRate";		            
		public const string Property_Total = "Total";		            
		public const string Property_ManufacturerId = "ManufacturerId";		            
		public const string Property_LocationId = "LocationId";		            
		public const string Property_TypeId = "TypeId";		            
		public const string Property_ModelId = "ModelId";		            
		public const string Property_FinishId = "FinishId";		            
		public const string Property_CapacityId = "CapacityId";		            
		public const string Property_IsPackageService = "IsPackageService";		            
		public const string Property_IsNonCommissionable = "IsNonCommissionable";		            
		public const string Property_AppointmentIntId = "AppointmentIntId";		            
		public const string Property_IsInvoice = "IsInvoice";		            
		public const string Property_AppointmentEquipmentIntId = "AppointmentEquipmentIntId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _CustomerId;	            
		private Guid _PackageId;	            
		private Guid _EquipmentId;	            
		private Nullable<Double> _MonthlyRate;	            
		private Nullable<Double> _DiscountRate;	            
		private Nullable<Double> _Total;	            
		private Guid _ManufacturerId;	            
		private Guid _LocationId;	            
		private Guid _TypeId;	            
		private Guid _ModelId;	            
		private Guid _FinishId;	            
		private Guid _CapacityId;	            
		private Nullable<Boolean> _IsPackageService;	            
		private Nullable<Boolean> _IsNonCommissionable;	            
		private Nullable<Int32> _AppointmentIntId;	            
		private Nullable<Boolean> _IsInvoice;	            
		private Nullable<Int32> _AppointmentEquipmentIntId;	            
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
		public Guid EquipmentId
		{	
			get{ return _EquipmentId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EquipmentId, value, _EquipmentId);
				if (PropertyChanging(args))
				{
					_EquipmentId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> MonthlyRate
		{	
			get{ return _MonthlyRate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MonthlyRate, value, _MonthlyRate);
				if (PropertyChanging(args))
				{
					_MonthlyRate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> DiscountRate
		{	
			get{ return _DiscountRate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DiscountRate, value, _DiscountRate);
				if (PropertyChanging(args))
				{
					_DiscountRate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> Total
		{	
			get{ return _Total; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Total, value, _Total);
				if (PropertyChanging(args))
				{
					_Total = value;
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
		public Guid LocationId
		{	
			get{ return _LocationId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LocationId, value, _LocationId);
				if (PropertyChanging(args))
				{
					_LocationId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid TypeId
		{	
			get{ return _TypeId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TypeId, value, _TypeId);
				if (PropertyChanging(args))
				{
					_TypeId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid ModelId
		{	
			get{ return _ModelId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ModelId, value, _ModelId);
				if (PropertyChanging(args))
				{
					_ModelId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid FinishId
		{	
			get{ return _FinishId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FinishId, value, _FinishId);
				if (PropertyChanging(args))
				{
					_FinishId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid CapacityId
		{	
			get{ return _CapacityId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CapacityId, value, _CapacityId);
				if (PropertyChanging(args))
				{
					_CapacityId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsPackageService
		{	
			get{ return _IsPackageService; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsPackageService, value, _IsPackageService);
				if (PropertyChanging(args))
				{
					_IsPackageService = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsNonCommissionable
		{	
			get{ return _IsNonCommissionable; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsNonCommissionable, value, _IsNonCommissionable);
				if (PropertyChanging(args))
				{
					_IsNonCommissionable = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> AppointmentIntId
		{	
			get{ return _AppointmentIntId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AppointmentIntId, value, _AppointmentIntId);
				if (PropertyChanging(args))
				{
					_AppointmentIntId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsInvoice
		{	
			get{ return _IsInvoice; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsInvoice, value, _IsInvoice);
				if (PropertyChanging(args))
				{
					_IsInvoice = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> AppointmentEquipmentIntId
		{	
			get{ return _AppointmentEquipmentIntId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AppointmentEquipmentIntId, value, _AppointmentEquipmentIntId);
				if (PropertyChanging(args))
				{
					_AppointmentEquipmentIntId = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  CustomerPackageServiceBase Clone()
		{
			CustomerPackageServiceBase newObj = new  CustomerPackageServiceBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.PackageId = this.PackageId;						
			newObj.EquipmentId = this.EquipmentId;						
			newObj.MonthlyRate = this.MonthlyRate;						
			newObj.DiscountRate = this.DiscountRate;						
			newObj.Total = this.Total;						
			newObj.ManufacturerId = this.ManufacturerId;						
			newObj.LocationId = this.LocationId;						
			newObj.TypeId = this.TypeId;						
			newObj.ModelId = this.ModelId;						
			newObj.FinishId = this.FinishId;						
			newObj.CapacityId = this.CapacityId;						
			newObj.IsPackageService = this.IsPackageService;						
			newObj.IsNonCommissionable = this.IsNonCommissionable;						
			newObj.AppointmentIntId = this.AppointmentIntId;						
			newObj.IsInvoice = this.IsInvoice;						
			newObj.AppointmentEquipmentIntId = this.AppointmentEquipmentIntId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomerPackageServiceBase.Property_Id, Id);				
			info.AddValue(CustomerPackageServiceBase.Property_CompanyId, CompanyId);				
			info.AddValue(CustomerPackageServiceBase.Property_CustomerId, CustomerId);				
			info.AddValue(CustomerPackageServiceBase.Property_PackageId, PackageId);				
			info.AddValue(CustomerPackageServiceBase.Property_EquipmentId, EquipmentId);				
			info.AddValue(CustomerPackageServiceBase.Property_MonthlyRate, MonthlyRate);				
			info.AddValue(CustomerPackageServiceBase.Property_DiscountRate, DiscountRate);				
			info.AddValue(CustomerPackageServiceBase.Property_Total, Total);				
			info.AddValue(CustomerPackageServiceBase.Property_ManufacturerId, ManufacturerId);				
			info.AddValue(CustomerPackageServiceBase.Property_LocationId, LocationId);				
			info.AddValue(CustomerPackageServiceBase.Property_TypeId, TypeId);				
			info.AddValue(CustomerPackageServiceBase.Property_ModelId, ModelId);				
			info.AddValue(CustomerPackageServiceBase.Property_FinishId, FinishId);				
			info.AddValue(CustomerPackageServiceBase.Property_CapacityId, CapacityId);				
			info.AddValue(CustomerPackageServiceBase.Property_IsPackageService, IsPackageService);				
			info.AddValue(CustomerPackageServiceBase.Property_IsNonCommissionable, IsNonCommissionable);				
			info.AddValue(CustomerPackageServiceBase.Property_AppointmentIntId, AppointmentIntId);				
			info.AddValue(CustomerPackageServiceBase.Property_IsInvoice, IsInvoice);				
			info.AddValue(CustomerPackageServiceBase.Property_AppointmentEquipmentIntId, AppointmentEquipmentIntId);				
		}
		#endregion

		
	}
}
