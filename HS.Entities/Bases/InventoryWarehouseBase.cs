using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "InventoryWarehouseBase", Namespace = "http://www.piistech.com//entities")]
	public class InventoryWarehouseBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			EquipmentId = 2,
			Type = 3,
			Quantity = 4,
			PurchaseOrderId = 5,
			LastUpdatedBy = 6,
			LastUpdatedDate = 7,
			Description = 8,
            LocationId = 9
        }
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_EquipmentId = "EquipmentId";		            
		public const string Property_Type = "Type";		            
		public const string Property_Quantity = "Quantity";		            
		public const string Property_PurchaseOrderId = "PurchaseOrderId";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_Description = "Description";
        public const string Property_LocationId = "LocationId";
        #endregion

        #region Private Data Types
        private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _EquipmentId;	            
		private String _Type;	            
		private Int32 _Quantity;	            
		private String _PurchaseOrderId;	            
		private Guid _LastUpdatedBy;	            
		private DateTime _LastUpdatedDate;	            
		private String _Description;
        private Guid _LocationId;
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
        public Guid LocationId
        {
            get { return _LocationId; }
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

        #endregion

        #region Cloning Base Objects
        public  InventoryWarehouseBase Clone()
		{
			InventoryWarehouseBase newObj = new  InventoryWarehouseBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.EquipmentId = this.EquipmentId;						
			newObj.Type = this.Type;						
			newObj.Quantity = this.Quantity;						
			newObj.PurchaseOrderId = this.PurchaseOrderId;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.Description = this.Description;
            newObj.LocationId = this.LocationId;

            return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(InventoryWarehouseBase.Property_Id, Id);				
			info.AddValue(InventoryWarehouseBase.Property_CompanyId, CompanyId);				
			info.AddValue(InventoryWarehouseBase.Property_EquipmentId, EquipmentId);				
			info.AddValue(InventoryWarehouseBase.Property_Type, Type);				
			info.AddValue(InventoryWarehouseBase.Property_Quantity, Quantity);				
			info.AddValue(InventoryWarehouseBase.Property_PurchaseOrderId, PurchaseOrderId);				
			info.AddValue(InventoryWarehouseBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(InventoryWarehouseBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(InventoryWarehouseBase.Property_Description, Description);
            info.AddValue(InventoryWarehouseBase.Property_LocationId, LocationId);
        }
		#endregion

		
	}
}
