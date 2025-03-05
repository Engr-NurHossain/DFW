using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "PurchaseOrderWarehouseBase", Namespace = "http://www.piistech.com//entities")]
	public class PurchaseOrderWarehouseBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			PurchaseOrderId = 2,
			Action = 3,
			Status = 4,
			Location = 5,
			IsReceived = 6,
			CreatedByUid = 7,
			LastUpdatedDate = 8,
			LastUpdatedByUid = 9,
			RecieveDate = 10,
			RecieveByUid = 11,
			BranchDemandOrderId = 12,
			SuplierId = 13,
			SoldBy = 14,
			Amount = 15,
			TaxType = 16,
			Tax = 17,
			Deposit = 18,
			TotalAmount = 19,
			BalanceDue = 20,
			OrderDate = 21,
			BillingAddress = 22,
			ShippingAddress = 23,
			ShippingVia = 24,
			ShippingDate = 25,
			ShippingCost = 26,
			TrackingNo = 27,
			Message = 28,
			Balance = 29,
			Description = 30,
			CreatedDate = 31,
			IsBulkPO = 32,
			RecieveForUid = 33,
			POFor = 34,
			EstimatorId = 35
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_PurchaseOrderId = "PurchaseOrderId";		            
		public const string Property_Action = "Action";		            
		public const string Property_Status = "Status";		            
		public const string Property_Location = "Location";		            
		public const string Property_IsReceived = "IsReceived";		            
		public const string Property_CreatedByUid = "CreatedByUid";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_LastUpdatedByUid = "LastUpdatedByUid";		            
		public const string Property_RecieveDate = "RecieveDate";		            
		public const string Property_RecieveByUid = "RecieveByUid";		            
		public const string Property_BranchDemandOrderId = "BranchDemandOrderId";		            
		public const string Property_SuplierId = "SuplierId";		            
		public const string Property_SoldBy = "SoldBy";		            
		public const string Property_Amount = "Amount";		            
		public const string Property_TaxType = "TaxType";		            
		public const string Property_Tax = "Tax";		            
		public const string Property_Deposit = "Deposit";		            
		public const string Property_TotalAmount = "TotalAmount";		            
		public const string Property_BalanceDue = "BalanceDue";		            
		public const string Property_OrderDate = "OrderDate";		            
		public const string Property_BillingAddress = "BillingAddress";		            
		public const string Property_ShippingAddress = "ShippingAddress";		            
		public const string Property_ShippingVia = "ShippingVia";		            
		public const string Property_ShippingDate = "ShippingDate";		            
		public const string Property_ShippingCost = "ShippingCost";		            
		public const string Property_TrackingNo = "TrackingNo";		            
		public const string Property_Message = "Message";		            
		public const string Property_Balance = "Balance";		            
		public const string Property_Description = "Description";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_IsBulkPO = "IsBulkPO";		            
		public const string Property_RecieveForUid = "RecieveForUid";		            
		public const string Property_POFor = "POFor";		            
		public const string Property_EstimatorId = "EstimatorId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private String _PurchaseOrderId;	            
		private String _Action;	            
		private String _Status;	            
		private String _Location;	            
		private Boolean _IsReceived;	            
		private Guid _CreatedByUid;	            
		private DateTime _LastUpdatedDate;	            
		private Guid _LastUpdatedByUid;	            
		private Nullable<DateTime> _RecieveDate;	            
		private Guid _RecieveByUid;	            
		private String _BranchDemandOrderId;	            
		private Guid _SuplierId;	            
		private Guid _SoldBy;	            
		private Double _Amount;	            
		private String _TaxType;	            
		private Nullable<Double> _Tax;	            
		private Nullable<Double> _Deposit;	            
		private Nullable<Double> _TotalAmount;	            
		private Nullable<Double> _BalanceDue;	            
		private DateTime _OrderDate;	            
		private String _BillingAddress;	            
		private String _ShippingAddress;	            
		private String _ShippingVia;	            
		private Nullable<DateTime> _ShippingDate;	            
		private Nullable<Double> _ShippingCost;	            
		private String _TrackingNo;	            
		private String _Message;	            
		private Nullable<Double> _Balance;	            
		private String _Description;	            
		private DateTime _CreatedDate;	            
		private Nullable<Boolean> _IsBulkPO;	            
		private Guid _RecieveForUid;	            
		private Guid _POFor;	            
		private String _EstimatorId;	            
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
		public String Action
		{	
			get{ return _Action; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Action, value, _Action);
				if (PropertyChanging(args))
				{
					_Action = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Status
		{	
			get{ return _Status; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Status, value, _Status);
				if (PropertyChanging(args))
				{
					_Status = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Location
		{	
			get{ return _Location; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Location, value, _Location);
				if (PropertyChanging(args))
				{
					_Location = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean IsReceived
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
		public Guid LastUpdatedByUid
		{	
			get{ return _LastUpdatedByUid; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastUpdatedByUid, value, _LastUpdatedByUid);
				if (PropertyChanging(args))
				{
					_LastUpdatedByUid = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> RecieveDate
		{	
			get{ return _RecieveDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RecieveDate, value, _RecieveDate);
				if (PropertyChanging(args))
				{
					_RecieveDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid RecieveByUid
		{	
			get{ return _RecieveByUid; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RecieveByUid, value, _RecieveByUid);
				if (PropertyChanging(args))
				{
					_RecieveByUid = value;
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
		public Guid SuplierId
		{	
			get{ return _SuplierId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SuplierId, value, _SuplierId);
				if (PropertyChanging(args))
				{
					_SuplierId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid SoldBy
		{	
			get{ return _SoldBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SoldBy, value, _SoldBy);
				if (PropertyChanging(args))
				{
					_SoldBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Double Amount
		{	
			get{ return _Amount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Amount, value, _Amount);
				if (PropertyChanging(args))
				{
					_Amount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TaxType
		{	
			get{ return _TaxType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TaxType, value, _TaxType);
				if (PropertyChanging(args))
				{
					_TaxType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> Tax
		{	
			get{ return _Tax; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Tax, value, _Tax);
				if (PropertyChanging(args))
				{
					_Tax = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> Deposit
		{	
			get{ return _Deposit; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Deposit, value, _Deposit);
				if (PropertyChanging(args))
				{
					_Deposit = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> TotalAmount
		{	
			get{ return _TotalAmount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TotalAmount, value, _TotalAmount);
				if (PropertyChanging(args))
				{
					_TotalAmount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> BalanceDue
		{	
			get{ return _BalanceDue; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BalanceDue, value, _BalanceDue);
				if (PropertyChanging(args))
				{
					_BalanceDue = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime OrderDate
		{	
			get{ return _OrderDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OrderDate, value, _OrderDate);
				if (PropertyChanging(args))
				{
					_OrderDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BillingAddress
		{	
			get{ return _BillingAddress; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BillingAddress, value, _BillingAddress);
				if (PropertyChanging(args))
				{
					_BillingAddress = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ShippingAddress
		{	
			get{ return _ShippingAddress; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ShippingAddress, value, _ShippingAddress);
				if (PropertyChanging(args))
				{
					_ShippingAddress = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ShippingVia
		{	
			get{ return _ShippingVia; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ShippingVia, value, _ShippingVia);
				if (PropertyChanging(args))
				{
					_ShippingVia = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> ShippingDate
		{	
			get{ return _ShippingDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ShippingDate, value, _ShippingDate);
				if (PropertyChanging(args))
				{
					_ShippingDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> ShippingCost
		{	
			get{ return _ShippingCost; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ShippingCost, value, _ShippingCost);
				if (PropertyChanging(args))
				{
					_ShippingCost = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TrackingNo
		{	
			get{ return _TrackingNo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TrackingNo, value, _TrackingNo);
				if (PropertyChanging(args))
				{
					_TrackingNo = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Message
		{	
			get{ return _Message; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Message, value, _Message);
				if (PropertyChanging(args))
				{
					_Message = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> Balance
		{	
			get{ return _Balance; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Balance, value, _Balance);
				if (PropertyChanging(args))
				{
					_Balance = value;
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
		public Nullable<Boolean> IsBulkPO
		{	
			get{ return _IsBulkPO; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsBulkPO, value, _IsBulkPO);
				if (PropertyChanging(args))
				{
					_IsBulkPO = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid RecieveForUid
		{	
			get{ return _RecieveForUid; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RecieveForUid, value, _RecieveForUid);
				if (PropertyChanging(args))
				{
					_RecieveForUid = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid POFor
		{	
			get{ return _POFor; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_POFor, value, _POFor);
				if (PropertyChanging(args))
				{
					_POFor = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String EstimatorId
		{	
			get{ return _EstimatorId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EstimatorId, value, _EstimatorId);
				if (PropertyChanging(args))
				{
					_EstimatorId = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  PurchaseOrderWarehouseBase Clone()
		{
			PurchaseOrderWarehouseBase newObj = new  PurchaseOrderWarehouseBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.PurchaseOrderId = this.PurchaseOrderId;						
			newObj.Action = this.Action;						
			newObj.Status = this.Status;						
			newObj.Location = this.Location;						
			newObj.IsReceived = this.IsReceived;						
			newObj.CreatedByUid = this.CreatedByUid;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.LastUpdatedByUid = this.LastUpdatedByUid;						
			newObj.RecieveDate = this.RecieveDate;						
			newObj.RecieveByUid = this.RecieveByUid;						
			newObj.BranchDemandOrderId = this.BranchDemandOrderId;						
			newObj.SuplierId = this.SuplierId;						
			newObj.SoldBy = this.SoldBy;						
			newObj.Amount = this.Amount;						
			newObj.TaxType = this.TaxType;						
			newObj.Tax = this.Tax;						
			newObj.Deposit = this.Deposit;						
			newObj.TotalAmount = this.TotalAmount;						
			newObj.BalanceDue = this.BalanceDue;						
			newObj.OrderDate = this.OrderDate;						
			newObj.BillingAddress = this.BillingAddress;						
			newObj.ShippingAddress = this.ShippingAddress;						
			newObj.ShippingVia = this.ShippingVia;						
			newObj.ShippingDate = this.ShippingDate;						
			newObj.ShippingCost = this.ShippingCost;						
			newObj.TrackingNo = this.TrackingNo;						
			newObj.Message = this.Message;						
			newObj.Balance = this.Balance;						
			newObj.Description = this.Description;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.IsBulkPO = this.IsBulkPO;						
			newObj.RecieveForUid = this.RecieveForUid;						
			newObj.POFor = this.POFor;						
			newObj.EstimatorId = this.EstimatorId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(PurchaseOrderWarehouseBase.Property_Id, Id);				
			info.AddValue(PurchaseOrderWarehouseBase.Property_CompanyId, CompanyId);				
			info.AddValue(PurchaseOrderWarehouseBase.Property_PurchaseOrderId, PurchaseOrderId);				
			info.AddValue(PurchaseOrderWarehouseBase.Property_Action, Action);				
			info.AddValue(PurchaseOrderWarehouseBase.Property_Status, Status);				
			info.AddValue(PurchaseOrderWarehouseBase.Property_Location, Location);				
			info.AddValue(PurchaseOrderWarehouseBase.Property_IsReceived, IsReceived);				
			info.AddValue(PurchaseOrderWarehouseBase.Property_CreatedByUid, CreatedByUid);				
			info.AddValue(PurchaseOrderWarehouseBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(PurchaseOrderWarehouseBase.Property_LastUpdatedByUid, LastUpdatedByUid);				
			info.AddValue(PurchaseOrderWarehouseBase.Property_RecieveDate, RecieveDate);				
			info.AddValue(PurchaseOrderWarehouseBase.Property_RecieveByUid, RecieveByUid);				
			info.AddValue(PurchaseOrderWarehouseBase.Property_BranchDemandOrderId, BranchDemandOrderId);				
			info.AddValue(PurchaseOrderWarehouseBase.Property_SuplierId, SuplierId);				
			info.AddValue(PurchaseOrderWarehouseBase.Property_SoldBy, SoldBy);				
			info.AddValue(PurchaseOrderWarehouseBase.Property_Amount, Amount);				
			info.AddValue(PurchaseOrderWarehouseBase.Property_TaxType, TaxType);				
			info.AddValue(PurchaseOrderWarehouseBase.Property_Tax, Tax);				
			info.AddValue(PurchaseOrderWarehouseBase.Property_Deposit, Deposit);				
			info.AddValue(PurchaseOrderWarehouseBase.Property_TotalAmount, TotalAmount);				
			info.AddValue(PurchaseOrderWarehouseBase.Property_BalanceDue, BalanceDue);				
			info.AddValue(PurchaseOrderWarehouseBase.Property_OrderDate, OrderDate);				
			info.AddValue(PurchaseOrderWarehouseBase.Property_BillingAddress, BillingAddress);				
			info.AddValue(PurchaseOrderWarehouseBase.Property_ShippingAddress, ShippingAddress);				
			info.AddValue(PurchaseOrderWarehouseBase.Property_ShippingVia, ShippingVia);				
			info.AddValue(PurchaseOrderWarehouseBase.Property_ShippingDate, ShippingDate);				
			info.AddValue(PurchaseOrderWarehouseBase.Property_ShippingCost, ShippingCost);				
			info.AddValue(PurchaseOrderWarehouseBase.Property_TrackingNo, TrackingNo);				
			info.AddValue(PurchaseOrderWarehouseBase.Property_Message, Message);				
			info.AddValue(PurchaseOrderWarehouseBase.Property_Balance, Balance);				
			info.AddValue(PurchaseOrderWarehouseBase.Property_Description, Description);				
			info.AddValue(PurchaseOrderWarehouseBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(PurchaseOrderWarehouseBase.Property_IsBulkPO, IsBulkPO);				
			info.AddValue(PurchaseOrderWarehouseBase.Property_RecieveForUid, RecieveForUid);				
			info.AddValue(PurchaseOrderWarehouseBase.Property_POFor, POFor);				
			info.AddValue(PurchaseOrderWarehouseBase.Property_EstimatorId, EstimatorId);				
		}
		#endregion

		
	}
}
