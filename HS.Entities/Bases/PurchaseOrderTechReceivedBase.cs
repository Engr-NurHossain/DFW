using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "PurchaseOrderTechReceivedBase", Namespace = "http://www.piistech.com//entities")]
	public class PurchaseOrderTechReceivedBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			BranchDemandOrderId = 1,
			EquipmentId = 2,
			EquipName = 3,
			EquipDetail = 4,
			BundleId = 5,
			Quantity = 6,
			UnitPrice = 7,
			TotalPrice = 8,
			RecieveQty = 9,
			IsReceived = 10,
			ReceivedDate = 11,
			ReceivedBy = 12,
			CreatedDate = 13,
			CreatedBy = 14
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_BranchDemandOrderId = "BranchDemandOrderId";		            
		public const string Property_EquipmentId = "EquipmentId";		            
		public const string Property_EquipName = "EquipName";		            
		public const string Property_EquipDetail = "EquipDetail";		            
		public const string Property_BundleId = "BundleId";		            
		public const string Property_Quantity = "Quantity";		            
		public const string Property_UnitPrice = "UnitPrice";		            
		public const string Property_TotalPrice = "TotalPrice";		            
		public const string Property_RecieveQty = "RecieveQty";		            
		public const string Property_IsReceived = "IsReceived";		            
		public const string Property_ReceivedDate = "ReceivedDate";		            
		public const string Property_ReceivedBy = "ReceivedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _BranchDemandOrderId;	            
		private Guid _EquipmentId;	            
		private String _EquipName;	            
		private String _EquipDetail;	            
		private Nullable<Int32> _BundleId;	            
		private Nullable<Int32> _Quantity;	            
		private Nullable<Double> _UnitPrice;	            
		private Nullable<Double> _TotalPrice;	            
		private Nullable<Int32> _RecieveQty;	            
		private Nullable<Boolean> _IsReceived;	            
		private Nullable<DateTime> _ReceivedDate;	            
		private Guid _ReceivedBy;	            
		private DateTime _CreatedDate;	            
		private Guid _CreatedBy;	            
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
		public String BranchDemandOrderId
		{	
			get{ return _BranchDemandOrderId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BranchDemandOrderId, value, _BranchDemandOrderId);
				if (PropertyChanging(args))
				{
					_BranchDemandOrderId = value;
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
		public Nullable<Boolean> IsReceived
		{	
			get{ return _IsReceived; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsReceived, value, _IsReceived);
				if (PropertyChanging(args))
				{
					_IsReceived = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> ReceivedDate
		{	
			get{ return _ReceivedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReceivedDate, value, _ReceivedDate);
				if (PropertyChanging(args))
				{
					_ReceivedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid ReceivedBy
		{	
			get{ return _ReceivedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReceivedBy, value, _ReceivedBy);
				if (PropertyChanging(args))
				{
					_ReceivedBy = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  PurchaseOrderTechReceivedBase Clone()
		{
			PurchaseOrderTechReceivedBase newObj = new  PurchaseOrderTechReceivedBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.BranchDemandOrderId = this.BranchDemandOrderId;						
			newObj.EquipmentId = this.EquipmentId;						
			newObj.EquipName = this.EquipName;						
			newObj.EquipDetail = this.EquipDetail;						
			newObj.BundleId = this.BundleId;						
			newObj.Quantity = this.Quantity;						
			newObj.UnitPrice = this.UnitPrice;						
			newObj.TotalPrice = this.TotalPrice;						
			newObj.RecieveQty = this.RecieveQty;						
			newObj.IsReceived = this.IsReceived;						
			newObj.ReceivedDate = this.ReceivedDate;						
			newObj.ReceivedBy = this.ReceivedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(PurchaseOrderTechReceivedBase.Property_Id, Id);				
			info.AddValue(PurchaseOrderTechReceivedBase.Property_BranchDemandOrderId, BranchDemandOrderId);				
			info.AddValue(PurchaseOrderTechReceivedBase.Property_EquipmentId, EquipmentId);				
			info.AddValue(PurchaseOrderTechReceivedBase.Property_EquipName, EquipName);				
			info.AddValue(PurchaseOrderTechReceivedBase.Property_EquipDetail, EquipDetail);				
			info.AddValue(PurchaseOrderTechReceivedBase.Property_BundleId, BundleId);				
			info.AddValue(PurchaseOrderTechReceivedBase.Property_Quantity, Quantity);				
			info.AddValue(PurchaseOrderTechReceivedBase.Property_UnitPrice, UnitPrice);				
			info.AddValue(PurchaseOrderTechReceivedBase.Property_TotalPrice, TotalPrice);				
			info.AddValue(PurchaseOrderTechReceivedBase.Property_RecieveQty, RecieveQty);				
			info.AddValue(PurchaseOrderTechReceivedBase.Property_IsReceived, IsReceived);				
			info.AddValue(PurchaseOrderTechReceivedBase.Property_ReceivedDate, ReceivedDate);				
			info.AddValue(PurchaseOrderTechReceivedBase.Property_ReceivedBy, ReceivedBy);				
			info.AddValue(PurchaseOrderTechReceivedBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(PurchaseOrderTechReceivedBase.Property_CreatedBy, CreatedBy);				
		}
		#endregion

		
	}
}
