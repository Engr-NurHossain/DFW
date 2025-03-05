using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "EquipmentVendorBase", Namespace = "http://www.hims-tech.com//entities")]
	public class EquipmentVendorBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			EquipmentId = 1,
			SupplierId = 2,
			Cost = 3,
			IsPrimary = 4,
			AddedBy = 5,
			AddedDate = 6,
			SKU = 7
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_EquipmentId = "EquipmentId";		            
		public const string Property_SupplierId = "SupplierId";		            
		public const string Property_Cost = "Cost";		            
		public const string Property_IsPrimary = "IsPrimary";		            
		public const string Property_AddedBy = "AddedBy";		            
		public const string Property_AddedDate = "AddedDate";		            
		public const string Property_SKU = "SKU";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _EquipmentId;	            
		private Guid _SupplierId;	            
		private Double _Cost;	            
		private Boolean _IsPrimary;	            
		private Guid _AddedBy;	            
		private DateTime _AddedDate;	            
		private String _SKU;	            
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
		public Guid SupplierId
		{	
			get{ return _SupplierId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SupplierId, value, _SupplierId);
				if (PropertyChanging(args))
				{
					_SupplierId = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  EquipmentVendorBase Clone()
		{
			EquipmentVendorBase newObj = new  EquipmentVendorBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.EquipmentId = this.EquipmentId;						
			newObj.SupplierId = this.SupplierId;						
			newObj.Cost = this.Cost;						
			newObj.IsPrimary = this.IsPrimary;						
			newObj.AddedBy = this.AddedBy;						
			newObj.AddedDate = this.AddedDate;						
			newObj.SKU = this.SKU;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(EquipmentVendorBase.Property_Id, Id);				
			info.AddValue(EquipmentVendorBase.Property_EquipmentId, EquipmentId);				
			info.AddValue(EquipmentVendorBase.Property_SupplierId, SupplierId);				
			info.AddValue(EquipmentVendorBase.Property_Cost, Cost);				
			info.AddValue(EquipmentVendorBase.Property_IsPrimary, IsPrimary);				
			info.AddValue(EquipmentVendorBase.Property_AddedBy, AddedBy);				
			info.AddValue(EquipmentVendorBase.Property_AddedDate, AddedDate);				
			info.AddValue(EquipmentVendorBase.Property_SKU, SKU);				
		}
		#endregion

		
	}
}
