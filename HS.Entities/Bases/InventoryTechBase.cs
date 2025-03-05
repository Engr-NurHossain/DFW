using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "InventoryTechBase", Namespace = "http://www.piistech.com//entities")]
	public class InventoryTechBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			TechnicianId = 2,
			EquipmentId = 3,
			Type = 4,
			Quantity = 5,
			PurchaseOrderId = 6,
			LastUpdatedBy = 7,
			LastUpdatedDate = 8,
			Description = 9,
			CustomerAppointmentEquipmentId = 10
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_TechnicianId = "TechnicianId";		            
		public const string Property_EquipmentId = "EquipmentId";		            
		public const string Property_Type = "Type";		            
		public const string Property_Quantity = "Quantity";		            
		public const string Property_PurchaseOrderId = "PurchaseOrderId";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_Description = "Description";		            
		public const string Property_CustomerAppointmentEquipmentId = "CustomerAppointmentEquipmentId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _TechnicianId;	            
		private Guid _EquipmentId;	            
		private String _Type;	            
		private Int32 _Quantity;	            
		private String _PurchaseOrderId;	            
		private Guid _LastUpdatedBy;	            
		private DateTime _LastUpdatedDate;	            
		private String _Description;	            
		private Nullable<Int32> _CustomerAppointmentEquipmentId;	            
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
		public Guid TechnicianId
		{	
			get{ return _TechnicianId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TechnicianId, value, _TechnicianId);
				if (PropertyChanging(args))
				{
					_TechnicianId = value;
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

		[DataMember]
		public Nullable<Int32> CustomerAppointmentEquipmentId
		{	
			get{ return _CustomerAppointmentEquipmentId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CustomerAppointmentEquipmentId, value, _CustomerAppointmentEquipmentId);
				if (PropertyChanging(args))
				{
					_CustomerAppointmentEquipmentId = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  InventoryTechBase Clone()
		{
			InventoryTechBase newObj = new  InventoryTechBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.TechnicianId = this.TechnicianId;						
			newObj.EquipmentId = this.EquipmentId;						
			newObj.Type = this.Type;						
			newObj.Quantity = this.Quantity;						
			newObj.PurchaseOrderId = this.PurchaseOrderId;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.Description = this.Description;						
			newObj.CustomerAppointmentEquipmentId = this.CustomerAppointmentEquipmentId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(InventoryTechBase.Property_Id, Id);				
			info.AddValue(InventoryTechBase.Property_CompanyId, CompanyId);				
			info.AddValue(InventoryTechBase.Property_TechnicianId, TechnicianId);				
			info.AddValue(InventoryTechBase.Property_EquipmentId, EquipmentId);				
			info.AddValue(InventoryTechBase.Property_Type, Type);				
			info.AddValue(InventoryTechBase.Property_Quantity, Quantity);				
			info.AddValue(InventoryTechBase.Property_PurchaseOrderId, PurchaseOrderId);				
			info.AddValue(InventoryTechBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(InventoryTechBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(InventoryTechBase.Property_Description, Description);				
			info.AddValue(InventoryTechBase.Property_CustomerAppointmentEquipmentId, CustomerAppointmentEquipmentId);				
		}
		#endregion

		
	}
}
