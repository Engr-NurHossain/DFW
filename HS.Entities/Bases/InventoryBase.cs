using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "InventoryBase", Namespace = "http://www.piistech.com//entities")]
	public class InventoryBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			InventoryId = 1,
			EquipmentId = 2,
			CompanyId = 3,
			Quantity = 4,
			Type = 5,
			SupplierCost = 6,
			Cost = 7,
			Retail = 8,
			CreatedDate = 9,
			CreatedBy = 10,
			CreatedByUid = 11,
			PurchaseOrderId = 12
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_InventoryId = "InventoryId";		            
		public const string Property_EquipmentId = "EquipmentId";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_Quantity = "Quantity";		            
		public const string Property_Type = "Type";		            
		public const string Property_SupplierCost = "SupplierCost";		            
		public const string Property_Cost = "Cost";		            
		public const string Property_Retail = "Retail";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedByUid = "CreatedByUid";		            
		public const string Property_PurchaseOrderId = "PurchaseOrderId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _InventoryId;	            
		private Guid _EquipmentId;	            
		private Guid _CompanyId;	            
		private Int32 _Quantity;	            
		private String _Type;	            
		private Nullable<Double> _SupplierCost;	            
		private Nullable<Double> _Cost;	            
		private Nullable<Double> _Retail;	            
		private DateTime _CreatedDate;	            
		private String _CreatedBy;	            
		private Guid _CreatedByUid;	            
		private String _PurchaseOrderId;	            
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
		public Guid InventoryId
		{	
			get{ return _InventoryId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InventoryId, value, _InventoryId);
				if (PropertyChanging(args))
				{
					_InventoryId = value;
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
		public Nullable<Double> SupplierCost
		{	
			get{ return _SupplierCost; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SupplierCost, value, _SupplierCost);
				if (PropertyChanging(args))
				{
					_SupplierCost = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> Cost
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
		public Nullable<Double> Retail
		{	
			get{ return _Retail; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Retail, value, _Retail);
				if (PropertyChanging(args))
				{
					_Retail = value;
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
		public String CreatedBy
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
		public Guid CreatedByUid
		{	
			get{ return _CreatedByUid; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreatedByUid, value, _CreatedByUid);
				if (PropertyChanging(args))
				{
					_CreatedByUid = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  InventoryBase Clone()
		{
			InventoryBase newObj = new  InventoryBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.InventoryId = this.InventoryId;						
			newObj.EquipmentId = this.EquipmentId;						
			newObj.CompanyId = this.CompanyId;						
			newObj.Quantity = this.Quantity;						
			newObj.Type = this.Type;						
			newObj.SupplierCost = this.SupplierCost;						
			newObj.Cost = this.Cost;						
			newObj.Retail = this.Retail;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedByUid = this.CreatedByUid;						
			newObj.PurchaseOrderId = this.PurchaseOrderId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(InventoryBase.Property_Id, Id);				
			info.AddValue(InventoryBase.Property_InventoryId, InventoryId);				
			info.AddValue(InventoryBase.Property_EquipmentId, EquipmentId);				
			info.AddValue(InventoryBase.Property_CompanyId, CompanyId);				
			info.AddValue(InventoryBase.Property_Quantity, Quantity);				
			info.AddValue(InventoryBase.Property_Type, Type);				
			info.AddValue(InventoryBase.Property_SupplierCost, SupplierCost);				
			info.AddValue(InventoryBase.Property_Cost, Cost);				
			info.AddValue(InventoryBase.Property_Retail, Retail);				
			info.AddValue(InventoryBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(InventoryBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(InventoryBase.Property_CreatedByUid, CreatedByUid);				
			info.AddValue(InventoryBase.Property_PurchaseOrderId, PurchaseOrderId);				
		}
		#endregion

		
	}
}
