using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "EquipmentManufacturerBase", Namespace = "http://www.hims-tech.com//entities")]
	public class EquipmentManufacturerBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			EquipmentId = 1,
			ManufacturerId = 2,
			SKU = 3,
			Cost = 4,
			IsPrimary = 5,
			AddedBy = 6,
			AddedDate = 7,
			Variation = 8
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_EquipmentId = "EquipmentId";		            
		public const string Property_ManufacturerId = "ManufacturerId";		            
		public const string Property_SKU = "SKU";		            
		public const string Property_Cost = "Cost";		            
		public const string Property_IsPrimary = "IsPrimary";		            
		public const string Property_AddedBy = "AddedBy";		            
		public const string Property_AddedDate = "AddedDate";		            
		public const string Property_Variation = "Variation";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _EquipmentId;	            
		private Guid _ManufacturerId;	            
		private String _SKU;	            
		private Double _Cost;	            
		private Boolean _IsPrimary;	            
		private Guid _AddedBy;	            
		private DateTime _AddedDate;	            
		private String _Variation;	            
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
		public String SKU
		{	
			get{ return _SKU; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SKU, value, _SKU);
				if (PropertyChanging(args))
				{
					_SKU = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Double Cost
		{	
			get{ return _Cost; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Cost, value, _Cost);
				if (PropertyChanging(args))
				{
					_Cost = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean IsPrimary
		{	
			get{ return _IsPrimary; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsPrimary, value, _IsPrimary);
				if (PropertyChanging(args))
				{
					_IsPrimary = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid AddedBy
		{	
			get{ return _AddedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AddedBy, value, _AddedBy);
				if (PropertyChanging(args))
				{
					_AddedBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime AddedDate
		{	
			get{ return _AddedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AddedDate, value, _AddedDate);
				if (PropertyChanging(args))
				{
					_AddedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Variation
		{	
			get{ return _Variation; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Variation, value, _Variation);
				if (PropertyChanging(args))
				{
					_Variation = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  EquipmentManufacturerBase Clone()
		{
			EquipmentManufacturerBase newObj = new  EquipmentManufacturerBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.EquipmentId = this.EquipmentId;						
			newObj.ManufacturerId = this.ManufacturerId;						
			newObj.SKU = this.SKU;						
			newObj.Cost = this.Cost;						
			newObj.IsPrimary = this.IsPrimary;						
			newObj.AddedBy = this.AddedBy;						
			newObj.AddedDate = this.AddedDate;						
			newObj.Variation = this.Variation;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(EquipmentManufacturerBase.Property_Id, Id);				
			info.AddValue(EquipmentManufacturerBase.Property_EquipmentId, EquipmentId);				
			info.AddValue(EquipmentManufacturerBase.Property_ManufacturerId, ManufacturerId);				
			info.AddValue(EquipmentManufacturerBase.Property_SKU, SKU);				
			info.AddValue(EquipmentManufacturerBase.Property_Cost, Cost);				
			info.AddValue(EquipmentManufacturerBase.Property_IsPrimary, IsPrimary);				
			info.AddValue(EquipmentManufacturerBase.Property_AddedBy, AddedBy);				
			info.AddValue(EquipmentManufacturerBase.Property_AddedDate, AddedDate);				
			info.AddValue(EquipmentManufacturerBase.Property_Variation, Variation);				
		}
		#endregion

		
	}
}
