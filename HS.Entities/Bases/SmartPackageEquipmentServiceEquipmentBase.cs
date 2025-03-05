using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "SmartPackageEquipmentServiceEquipmentBase", Namespace = "http://www.piistech.com//entities")]
	public class SmartPackageEquipmentServiceEquipmentBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			SmartPackageEquipmentServiceId = 1,
			EquipmentId = 2,
			Quantity = 3,
			EquipmentPrice = 4
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_SmartPackageEquipmentServiceId = "SmartPackageEquipmentServiceId";		            
		public const string Property_EquipmentId = "EquipmentId";		            
		public const string Property_Quantity = "Quantity";		            
		public const string Property_EquipmentPrice = "EquipmentPrice";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _SmartPackageEquipmentServiceId;	            
		private Guid _EquipmentId;	            
		private Nullable<Int32> _Quantity;	            
		private Nullable<Double> _EquipmentPrice;	            
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
		public Guid SmartPackageEquipmentServiceId
		{	
			get{ return _SmartPackageEquipmentServiceId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SmartPackageEquipmentServiceId, value, _SmartPackageEquipmentServiceId);
				if (PropertyChanging(args))
				{
					_SmartPackageEquipmentServiceId = value;
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
		public Nullable<Int32> Quantity
		{	
			get{ return _Quantity; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Quantity, value, _Quantity);
				if (PropertyChanging(args))
				{
					_Quantity = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> EquipmentPrice
		{	
			get{ return _EquipmentPrice; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EquipmentPrice, value, _EquipmentPrice);
				if (PropertyChanging(args))
				{
					_EquipmentPrice = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  SmartPackageEquipmentServiceEquipmentBase Clone()
		{
			SmartPackageEquipmentServiceEquipmentBase newObj = new  SmartPackageEquipmentServiceEquipmentBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.SmartPackageEquipmentServiceId = this.SmartPackageEquipmentServiceId;						
			newObj.EquipmentId = this.EquipmentId;						
			newObj.Quantity = this.Quantity;						
			newObj.EquipmentPrice = this.EquipmentPrice;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(SmartPackageEquipmentServiceEquipmentBase.Property_Id, Id);				
			info.AddValue(SmartPackageEquipmentServiceEquipmentBase.Property_SmartPackageEquipmentServiceId, SmartPackageEquipmentServiceId);				
			info.AddValue(SmartPackageEquipmentServiceEquipmentBase.Property_EquipmentId, EquipmentId);				
			info.AddValue(SmartPackageEquipmentServiceEquipmentBase.Property_Quantity, Quantity);				
			info.AddValue(SmartPackageEquipmentServiceEquipmentBase.Property_EquipmentPrice, EquipmentPrice);				
		}
		#endregion

		
	}
}
