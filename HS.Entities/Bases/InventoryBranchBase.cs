using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "InventoryBranchBase", Namespace = "http://www.piistech.com//entities")]
	public class InventoryBranchBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			BranchId = 2,
			EquipmentId = 3,
			Type = 4,
			Quantity = 5,
			PurchaseOrderId = 6,
			LastUpdatedBy = 7,
			LastUpdatedDate = 8,
			Description = 9
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_BranchId = "BranchId";		            
		public const string Property_EquipmentId = "EquipmentId";		            
		public const string Property_Type = "Type";		            
		public const string Property_Quantity = "Quantity";		            
		public const string Property_PurchaseOrderId = "PurchaseOrderId";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_Description = "Description";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Int32 _BranchId;	            
		private Guid _EquipmentId;	            
		private String _Type;	            
		private Int32 _Quantity;	            
		private String _PurchaseOrderId;	            
		private Guid _LastUpdatedBy;	            
		private DateTime _LastUpdatedDate;	            
		private String _Description;	            
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
		public Int32 BranchId
		{	
			get{ return _BranchId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BranchId, value, _BranchId);
				if (PropertyChanging(args))
				{
					_BranchId = value;
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
		public String Type
		{	
			get{ return _Type; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Type, value, _Type);
				if (PropertyChanging(args))
				{
					_Type = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Int32 Quantity
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
		public String PurchaseOrderId
		{	
			get{ return _PurchaseOrderId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PurchaseOrderId, value, _PurchaseOrderId);
				if (PropertyChanging(args))
				{
					_PurchaseOrderId = value;
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

		[DataMember]
		public String Description
		{	
			get{ return _Description; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Description, value, _Description);
				if (PropertyChanging(args))
				{
					_Description = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  InventoryBranchBase Clone()
		{
			InventoryBranchBase newObj = new  InventoryBranchBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.BranchId = this.BranchId;						
			newObj.EquipmentId = this.EquipmentId;						
			newObj.Type = this.Type;						
			newObj.Quantity = this.Quantity;						
			newObj.PurchaseOrderId = this.PurchaseOrderId;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.Description = this.Description;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(InventoryBranchBase.Property_Id, Id);				
			info.AddValue(InventoryBranchBase.Property_CompanyId, CompanyId);				
			info.AddValue(InventoryBranchBase.Property_BranchId, BranchId);				
			info.AddValue(InventoryBranchBase.Property_EquipmentId, EquipmentId);				
			info.AddValue(InventoryBranchBase.Property_Type, Type);				
			info.AddValue(InventoryBranchBase.Property_Quantity, Quantity);				
			info.AddValue(InventoryBranchBase.Property_PurchaseOrderId, PurchaseOrderId);				
			info.AddValue(InventoryBranchBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(InventoryBranchBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(InventoryBranchBase.Property_Description, Description);				
		}
		#endregion

		
	}
}
