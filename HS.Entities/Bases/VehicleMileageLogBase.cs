using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "VehicleMileageLogBase", Namespace = "http://www.piistech.com//entities")]
	public class VehicleMileageLogBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			VehicleId = 1,
			Mileage = 2,
			ExteriorClean = 3,
			InteriorClean = 4,
			Vaccumed = 5,
			EquipmentOrganized = 6
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_VehicleId = "VehicleId";		            
		public const string Property_Mileage = "Mileage";		            
		public const string Property_ExteriorClean = "ExteriorClean";		            
		public const string Property_InteriorClean = "InteriorClean";		            
		public const string Property_Vaccumed = "Vaccumed";		            
		public const string Property_EquipmentOrganized = "EquipmentOrganized";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _VehicleId;	            
		private Nullable<Double> _Mileage;	            
		private Nullable<Boolean> _ExteriorClean;	            
		private Nullable<Boolean> _InteriorClean;	            
		private Nullable<Boolean> _Vaccumed;	            
		private Nullable<Boolean> _EquipmentOrganized;	            
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
		public Guid VehicleId
		{	
			get{ return _VehicleId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_VehicleId, value, _VehicleId);
				if (PropertyChanging(args))
				{
					_VehicleId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> Mileage
		{	
			get{ return _Mileage; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Mileage, value, _Mileage);
				if (PropertyChanging(args))
				{
					_Mileage = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> ExteriorClean
		{	
			get{ return _ExteriorClean; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ExteriorClean, value, _ExteriorClean);
				if (PropertyChanging(args))
				{
					_ExteriorClean = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> InteriorClean
		{	
			get{ return _InteriorClean; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InteriorClean, value, _InteriorClean);
				if (PropertyChanging(args))
				{
					_InteriorClean = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> Vaccumed
		{	
			get{ return _Vaccumed; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Vaccumed, value, _Vaccumed);
				if (PropertyChanging(args))
				{
					_Vaccumed = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> EquipmentOrganized
		{	
			get{ return _EquipmentOrganized; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EquipmentOrganized, value, _EquipmentOrganized);
				if (PropertyChanging(args))
				{
					_EquipmentOrganized = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  VehicleMileageLogBase Clone()
		{
			VehicleMileageLogBase newObj = new  VehicleMileageLogBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.VehicleId = this.VehicleId;						
			newObj.Mileage = this.Mileage;						
			newObj.ExteriorClean = this.ExteriorClean;						
			newObj.InteriorClean = this.InteriorClean;						
			newObj.Vaccumed = this.Vaccumed;						
			newObj.EquipmentOrganized = this.EquipmentOrganized;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(VehicleMileageLogBase.Property_Id, Id);				
			info.AddValue(VehicleMileageLogBase.Property_VehicleId, VehicleId);				
			info.AddValue(VehicleMileageLogBase.Property_Mileage, Mileage);				
			info.AddValue(VehicleMileageLogBase.Property_ExteriorClean, ExteriorClean);				
			info.AddValue(VehicleMileageLogBase.Property_InteriorClean, InteriorClean);				
			info.AddValue(VehicleMileageLogBase.Property_Vaccumed, Vaccumed);				
			info.AddValue(VehicleMileageLogBase.Property_EquipmentOrganized, EquipmentOrganized);				
		}
		#endregion

		
	}
}
