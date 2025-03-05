using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "ServiceMapBase", Namespace = "http://www.piistech.com//entities")]
	public class ServiceMapBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			ManufacturerId = 2,
			LocationId = 3,
			TypeId = 4,
			ModelId = 5,
			FinishId = 6,
			CapacityId = 7,
			ServiceId = 8,
			EquipmentId = 9
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_ManufacturerId = "ManufacturerId";		            
		public const string Property_LocationId = "LocationId";		            
		public const string Property_TypeId = "TypeId";		            
		public const string Property_ModelId = "ModelId";		            
		public const string Property_FinishId = "FinishId";		            
		public const string Property_CapacityId = "CapacityId";		            
		public const string Property_ServiceId = "ServiceId";		            
		public const string Property_EquipmentId = "EquipmentId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _ManufacturerId;	            
		private Guid _LocationId;	            
		private Guid _TypeId;	            
		private Guid _ModelId;	            
		private Guid _FinishId;	            
		private Guid _CapacityId;	            
		private Guid _ServiceId;	            
		private Guid _EquipmentId;	            
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
		public Guid ServiceId
		{	
			get{ return _ServiceId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ServiceId, value, _ServiceId);
				if (PropertyChanging(args))
				{
					_ServiceId = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  ServiceMapBase Clone()
		{
			ServiceMapBase newObj = new  ServiceMapBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.ManufacturerId = this.ManufacturerId;						
			newObj.LocationId = this.LocationId;						
			newObj.TypeId = this.TypeId;						
			newObj.ModelId = this.ModelId;						
			newObj.FinishId = this.FinishId;						
			newObj.CapacityId = this.CapacityId;						
			newObj.ServiceId = this.ServiceId;						
			newObj.EquipmentId = this.EquipmentId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(ServiceMapBase.Property_Id, Id);				
			info.AddValue(ServiceMapBase.Property_CompanyId, CompanyId);				
			info.AddValue(ServiceMapBase.Property_ManufacturerId, ManufacturerId);				
			info.AddValue(ServiceMapBase.Property_LocationId, LocationId);				
			info.AddValue(ServiceMapBase.Property_TypeId, TypeId);				
			info.AddValue(ServiceMapBase.Property_ModelId, ModelId);				
			info.AddValue(ServiceMapBase.Property_FinishId, FinishId);				
			info.AddValue(ServiceMapBase.Property_CapacityId, CapacityId);				
			info.AddValue(ServiceMapBase.Property_ServiceId, ServiceId);				
			info.AddValue(ServiceMapBase.Property_EquipmentId, EquipmentId);				
		}
		#endregion

		
	}
}
