using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "PurchaseOrderBase", Namespace = "http://www.piistech.com//entities")]
	public class PurchaseOrderBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			PurchaseOrderId = 1,
			SuplierId = 2,
			CompanyId = 3,
			SoldBy = 4,
			DiscountCode = 5,
			DiscountType = 6,
			DiscountAmount = 7,
			Discountpercent = 8,
			Amount = 9,
			Tax = 10,
			Deposit = 11,
			TotalAmount = 12,
			BalanceDue = 13,
			Status = 14,
			OrderDate = 15,
			BillingAddress = 16,
			ShippingAddress = 17,
			ShippingVia = 18,
			ShippingDate = 19,
			ShippingCost = 20,
			TrackingNo = 21,
			Message = 22,
			TaxType = 23,
			Balance = 24,
			Description = 25,
			Signature = 26,
			CancelReason = 27,
			CreatedDate = 28,
			CreatedBy = 29,
			CreatedByUid = 30,
			LastUpdatedDate = 31,
			LastUpdatedByUid = 32,
			RecieveDate = 33,
			RecieveByUid = 34
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_PurchaseOrderId = "PurchaseOrderId";		            
		public const string Property_SuplierId = "SuplierId";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_SoldBy = "SoldBy";		            
		public const string Property_DiscountCode = "DiscountCode";		            
		public const string Property_DiscountType = "DiscountType";		            
		public const string Property_DiscountAmount = "DiscountAmount";		            
		public const string Property_Discountpercent = "Discountpercent";		            
		public const string Property_Amount = "Amount";		            
		public const string Property_Tax = "Tax";		            
		public const string Property_Deposit = "Deposit";		            
		public const string Property_TotalAmount = "TotalAmount";		            
		public const string Property_BalanceDue = "BalanceDue";		            
		public const string Property_Status = "Status";		            
		public const string Property_OrderDate = "OrderDate";		            
		public const string Property_BillingAddress = "BillingAddress";		            
		public const string Property_ShippingAddress = "ShippingAddress";		            
		public const string Property_ShippingVia = "ShippingVia";		            
		public const string Property_ShippingDate = "ShippingDate";		            
		public const string Property_ShippingCost = "ShippingCost";		            
		public const string Property_TrackingNo = "TrackingNo";		            
		public const string Property_Message = "Message";		            
		public const string Property_TaxType = "TaxType";		            
		public const string Property_Balance = "Balance";		            
		public const string Property_Description = "Description";		            
		public const string Property_Signature = "Signature";		            
		public const string Property_CancelReason = "CancelReason";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedByUid = "CreatedByUid";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_LastUpdatedByUid = "LastUpdatedByUid";		            
		public const string Property_RecieveDate = "RecieveDate";		            
		public const string Property_RecieveByUid = "RecieveByUid";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _PurchaseOrderId;	            
		private Guid _SuplierId;	            
		private Guid _CompanyId;	            
		private Guid _SoldBy;	            
		private String _DiscountCode;	            
		private String _DiscountType;	            
		private Nullable<Double> _DiscountAmount;	            
		private Nullable<Double> _Discountpercent;	            
		private Double _Amount;	            
		private Nullable<Double> _Tax;	            
		private Nullable<Double> _Deposit;	            
		private Nullable<Double> _TotalAmount;	            
		private Nullable<Double> _BalanceDue;	            
		private String _Status;	            
		private DateTime _OrderDate;	            
		private String _BillingAddress;	            
		private String _ShippingAddress;	            
		private String _ShippingVia;	            
		private Nullable<DateTime> _ShippingDate;	            
		private Nullable<Double> _ShippingCost;	            
		private String _TrackingNo;	            
		private String _Message;	            
		private String _TaxType;	            
		private Nullable<Double> _Balance;	            
		private String _Description;	            
		private String _Signature;	            
		private String _CancelReason;	            
		private DateTime _CreatedDate;	            
		private String _CreatedBy;	            
		private Guid _CreatedByUid;	            
		private DateTime _LastUpdatedDate;	            
		private Guid _LastUpdatedByUid;	            
		private Nullable<DateTime> _RecieveDate;	            
		private Guid _RecieveByUid;	            
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
		public String DiscountCode
		{	
			get{ return _DiscountCode; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DiscountCode, value, _DiscountCode);
				if (PropertyChanging(args))
				{
					_DiscountCode = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String DiscountType
		{	
			get{ return _DiscountType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DiscountType, value, _DiscountType);
				if (PropertyChanging(args))
				{
					_DiscountType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> DiscountAmount
		{	
			get{ return _DiscountAmount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DiscountAmount, value, _DiscountAmount);
				if (PropertyChanging(args))
				{
					_DiscountAmount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> Discountpercent
		{	
			get{ return _Discountpercent; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Discountpercent, value, _Discountpercent);
				if (PropertyChanging(args))
				{
					_Discountpercent = value;
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
		public String Signature
		{	
			get{ return _Signature; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Signature, value, _Signature);
				if (PropertyChanging(args))
				{
					_Signature = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CancelReason
		{	
			get{ return _CancelReason; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CancelReason, value, _CancelReason);
				if (PropertyChanging(args))
				{
					_CancelReason = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  PurchaseOrderBase Clone()
		{
			PurchaseOrderBase newObj = new  PurchaseOrderBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.PurchaseOrderId = this.PurchaseOrderId;						
			newObj.SuplierId = this.SuplierId;						
			newObj.CompanyId = this.CompanyId;						
			newObj.SoldBy = this.SoldBy;						
			newObj.DiscountCode = this.DiscountCode;						
			newObj.DiscountType = this.DiscountType;						
			newObj.DiscountAmount = this.DiscountAmount;						
			newObj.Discountpercent = this.Discountpercent;						
			newObj.Amount = this.Amount;						
			newObj.Tax = this.Tax;						
			newObj.Deposit = this.Deposit;						
			newObj.TotalAmount = this.TotalAmount;						
			newObj.BalanceDue = this.BalanceDue;						
			newObj.Status = this.Status;						
			newObj.OrderDate = this.OrderDate;						
			newObj.BillingAddress = this.BillingAddress;						
			newObj.ShippingAddress = this.ShippingAddress;						
			newObj.ShippingVia = this.ShippingVia;						
			newObj.ShippingDate = this.ShippingDate;						
			newObj.ShippingCost = this.ShippingCost;						
			newObj.TrackingNo = this.TrackingNo;						
			newObj.Message = this.Message;						
			newObj.TaxType = this.TaxType;						
			newObj.Balance = this.Balance;						
			newObj.Description = this.Description;						
			newObj.Signature = this.Signature;						
			newObj.CancelReason = this.CancelReason;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedByUid = this.CreatedByUid;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.LastUpdatedByUid = this.LastUpdatedByUid;						
			newObj.RecieveDate = this.RecieveDate;						
			newObj.RecieveByUid = this.RecieveByUid;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(PurchaseOrderBase.Property_Id, Id);				
			info.AddValue(PurchaseOrderBase.Property_PurchaseOrderId, PurchaseOrderId);				
			info.AddValue(PurchaseOrderBase.Property_SuplierId, SuplierId);				
			info.AddValue(PurchaseOrderBase.Property_CompanyId, CompanyId);				
			info.AddValue(PurchaseOrderBase.Property_SoldBy, SoldBy);				
			info.AddValue(PurchaseOrderBase.Property_DiscountCode, DiscountCode);				
			info.AddValue(PurchaseOrderBase.Property_DiscountType, DiscountType);				
			info.AddValue(PurchaseOrderBase.Property_DiscountAmount, DiscountAmount);				
			info.AddValue(PurchaseOrderBase.Property_Discountpercent, Discountpercent);				
			info.AddValue(PurchaseOrderBase.Property_Amount, Amount);				
			info.AddValue(PurchaseOrderBase.Property_Tax, Tax);				
			info.AddValue(PurchaseOrderBase.Property_Deposit, Deposit);				
			info.AddValue(PurchaseOrderBase.Property_TotalAmount, TotalAmount);				
			info.AddValue(PurchaseOrderBase.Property_BalanceDue, BalanceDue);				
			info.AddValue(PurchaseOrderBase.Property_Status, Status);				
			info.AddValue(PurchaseOrderBase.Property_OrderDate, OrderDate);				
			info.AddValue(PurchaseOrderBase.Property_BillingAddress, BillingAddress);				
			info.AddValue(PurchaseOrderBase.Property_ShippingAddress, ShippingAddress);				
			info.AddValue(PurchaseOrderBase.Property_ShippingVia, ShippingVia);				
			info.AddValue(PurchaseOrderBase.Property_ShippingDate, ShippingDate);				
			info.AddValue(PurchaseOrderBase.Property_ShippingCost, ShippingCost);				
			info.AddValue(PurchaseOrderBase.Property_TrackingNo, TrackingNo);				
			info.AddValue(PurchaseOrderBase.Property_Message, Message);				
			info.AddValue(PurchaseOrderBase.Property_TaxType, TaxType);				
			info.AddValue(PurchaseOrderBase.Property_Balance, Balance);				
			info.AddValue(PurchaseOrderBase.Property_Description, Description);				
			info.AddValue(PurchaseOrderBase.Property_Signature, Signature);				
			info.AddValue(PurchaseOrderBase.Property_CancelReason, CancelReason);				
			info.AddValue(PurchaseOrderBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(PurchaseOrderBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(PurchaseOrderBase.Property_CreatedByUid, CreatedByUid);				
			info.AddValue(PurchaseOrderBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(PurchaseOrderBase.Property_LastUpdatedByUid, LastUpdatedByUid);				
			info.AddValue(PurchaseOrderBase.Property_RecieveDate, RecieveDate);				
			info.AddValue(PurchaseOrderBase.Property_RecieveByUid, RecieveByUid);				
		}
		#endregion

		
	}
}
