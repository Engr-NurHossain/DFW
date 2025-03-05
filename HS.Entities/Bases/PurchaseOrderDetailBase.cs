using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "PurchaseOrderDetailBase", Namespace = "http://www.piistech.com//entities")]
	public class PurchaseOrderDetailBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			PurchaseOrderId = 1,
			EquipmentId = 2,
			EquipName = 3,
			EquipDetail = 4,
			BundleId = 5,
			Quantity = 6,
			UnitPrice = 7,
			TotalPrice = 8,
			CreatedDate = 9,
			CreatedBy = 10,
			RecieveQty = 11,
			BulkStatus = 12
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_PurchaseOrderId = "PurchaseOrderId";		            
		public const string Property_EquipmentId = "EquipmentId";		            
		public const string Property_EquipName = "EquipName";		            
		public const string Property_EquipDetail = "EquipDetail";		            
		public const string Property_BundleId = "BundleId";		            
		public const string Property_Quantity = "Quantity";		            
		public const string Property_UnitPrice = "UnitPrice";		            
		public const string Property_TotalPrice = "TotalPrice";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_RecieveQty = "RecieveQty";		            
		public const string Property_BulkStatus = "BulkStatus";		            
		public const string Property_IsExists = "IsExists";
		public const string Property_TechnicianId = "TechnicianId";
		#endregion

		#region Private Data Types
		private Int32 _Id;	            
		private String _PurchaseOrderId;	            
		private Guid _EquipmentId;	            
		private String _EquipName;	            
		private String _EquipDetail;	            
		private Nullable<Int32> _BundleId;	            
		private Nullable<Int32> _Quantity;	            
		private Nullable<Double> _UnitPrice;	            
		private Nullable<Double> _TotalPrice;	            
		private DateTime _CreatedDate;	            
		private Guid _CreatedBy;	            
		private Nullable<Int32> _RecieveQty;	            
		private Nullable<Boolean> _BulkStatus;	            
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
		public String EquipName
		{	
			get{ return _EquipName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EquipName, value, _EquipName);
				if (PropertyChanging(args))
				{
					_EquipName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String EquipDetail
		{	
			get{ return _EquipDetail; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EquipDetail, value, _EquipDetail);
				if (PropertyChanging(args))
				{
					_EquipDetail = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> BundleId
		{	
			get{ return _BundleId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BundleId, value, _BundleId);
				if (PropertyChanging(args))
				{
					_BundleId = value;
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
		public Nullable<Double> UnitPrice
		{	
			get{ return _UnitPrice; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UnitPrice, value, _UnitPrice);
				if (PropertyChanging(args))
				{
					_UnitPrice = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> TotalPrice
		{	
			get{ return _TotalPrice; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TotalPrice, value, _TotalPrice);
				if (PropertyChanging(args))
				{
					_TotalPrice = value;
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
		public Guid CreatedBy
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
		public Nullable<Int32> RecieveQty
		{	
			get{ return _RecieveQty; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RecieveQty, value, _RecieveQty);
				if (PropertyChanging(args))
				{
					_RecieveQty = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> BulkStatus
		{	
			get{ return _BulkStatus; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BulkStatus, value, _BulkStatus);
				if (PropertyChanging(args))
				{
					_BulkStatus = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  PurchaseOrderDetailBase Clone()
		{
			PurchaseOrderDetailBase newObj = new  PurchaseOrderDetailBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.PurchaseOrderId = this.PurchaseOrderId;						
			newObj.EquipmentId = this.EquipmentId;						
			newObj.EquipName = this.EquipName;						
			newObj.EquipDetail = this.EquipDetail;						
			newObj.BundleId = this.BundleId;						
			newObj.Quantity = this.Quantity;						
			newObj.UnitPrice = this.UnitPrice;						
			newObj.TotalPrice = this.TotalPrice;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.RecieveQty = this.RecieveQty;						
			newObj.BulkStatus = this.BulkStatus;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(PurchaseOrderDetailBase.Property_Id, Id);				
			info.AddValue(PurchaseOrderDetailBase.Property_PurchaseOrderId, PurchaseOrderId);				
			info.AddValue(PurchaseOrderDetailBase.Property_EquipmentId, EquipmentId);				
			info.AddValue(PurchaseOrderDetailBase.Property_EquipName, EquipName);				
			info.AddValue(PurchaseOrderDetailBase.Property_EquipDetail, EquipDetail);				
			info.AddValue(PurchaseOrderDetailBase.Property_BundleId, BundleId);				
			info.AddValue(PurchaseOrderDetailBase.Property_Quantity, Quantity);				
			info.AddValue(PurchaseOrderDetailBase.Property_UnitPrice, UnitPrice);				
			info.AddValue(PurchaseOrderDetailBase.Property_TotalPrice, TotalPrice);				
			info.AddValue(PurchaseOrderDetailBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(PurchaseOrderDetailBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(PurchaseOrderDetailBase.Property_RecieveQty, RecieveQty);				
			info.AddValue(PurchaseOrderDetailBase.Property_BulkStatus, BulkStatus);				
		}
		#endregion

		
	}
}
