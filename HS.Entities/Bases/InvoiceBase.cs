using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "InvoiceBase", Namespace = "http://www.hims-tech.com//entities")]
	public class InvoiceBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			InvoiceId = 1,
			CustomerId = 2,
			CompanyId = 3,
			Amount = 4,
			Tax = 5,
			DiscountCode = 6,
			DiscountAmount = 7,
			TotalAmount = 8,
			Status = 9,
			InvoiceDate = 10,
			IsEstimate = 11,
			IsBill = 12,
			BillingAddress = 13,
			DueDate = 14,
			Terms = 15,
			ShippingAddress = 16,
			ShippingVia = 17,
			ShippingDate = 18,
			TrackingNo = 19,
			ShippingCost = 20,
			Discountpercent = 21,
			BalanceDue = 22,
			Deposit = 23,
			Message = 24,
			TaxType = 25,
			Balance = 26,
			Memo = 27,
			InvoiceFor = 28,
			LateFee = 29,
			LateAmount = 30,
			InstallDate = 31,
			Description = 32,
			DiscountType = 33,
			BillingCycle = 34,
			EstimateTerm = 35,
			Signature = 36,
			CancelReason = 37,
			CreatedDate = 38,
			CreatedBy = 39,
			CreatedByUid = 40,
			LastUpdatedDate = 41,
			LastUpdatedByUid = 42,
			RefType = 43,
			PaymentType = 44,
			BookingId = 45,
			InstallationType = 46,
			SignatureDate = 47,
			InvoiceEmailAddress = 48,
			InvoiceCcEmailAddress = 49,
			MonitoringAmount = 50,
			ContractTerm = 51,
			MonitoringDescription = 52,
			IsARBInvoice = 53,
			TransactionId = 54,
			ForteStatus = 55,
			UpfrontMonth = 56,
			JobNo = 57,
			TaxPercentage = 58,
			TicketId = 59,
			ItemType = 60,
			BatchNumber = 61,
			FinanceCompany = 62,
			InvoiceContractDiagram = 63
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_InvoiceId = "InvoiceId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_Amount = "Amount";		            
		public const string Property_Tax = "Tax";		            
		public const string Property_DiscountCode = "DiscountCode";		            
		public const string Property_DiscountAmount = "DiscountAmount";		            
		public const string Property_TotalAmount = "TotalAmount";		            
		public const string Property_Status = "Status";		            
		public const string Property_InvoiceDate = "InvoiceDate";		            
		public const string Property_IsEstimate = "IsEstimate";		            
		public const string Property_IsBill = "IsBill";		            
		public const string Property_BillingAddress = "BillingAddress";		            
		public const string Property_DueDate = "DueDate";		            
		public const string Property_Terms = "Terms";		            
		public const string Property_ShippingAddress = "ShippingAddress";		            
		public const string Property_ShippingVia = "ShippingVia";		            
		public const string Property_ShippingDate = "ShippingDate";		            
		public const string Property_TrackingNo = "TrackingNo";		            
		public const string Property_ShippingCost = "ShippingCost";		            
		public const string Property_Discountpercent = "Discountpercent";		            
		public const string Property_BalanceDue = "BalanceDue";		            
		public const string Property_Deposit = "Deposit";		            
		public const string Property_Message = "Message";		            
		public const string Property_TaxType = "TaxType";		            
		public const string Property_Balance = "Balance";		            
		public const string Property_Memo = "Memo";		            
		public const string Property_InvoiceFor = "InvoiceFor";		            
		public const string Property_LateFee = "LateFee";		            
		public const string Property_LateAmount = "LateAmount";		            
		public const string Property_InstallDate = "InstallDate";		            
		public const string Property_Description = "Description";		            
		public const string Property_DiscountType = "DiscountType";		            
		public const string Property_BillingCycle = "BillingCycle";		            
		public const string Property_EstimateTerm = "EstimateTerm";		            
		public const string Property_Signature = "Signature";		            
		public const string Property_CancelReason = "CancelReason";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedByUid = "CreatedByUid";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_LastUpdatedByUid = "LastUpdatedByUid";		            
		public const string Property_RefType = "RefType";		            
		public const string Property_PaymentType = "PaymentType";		            
		public const string Property_BookingId = "BookingId";		            
		public const string Property_InstallationType = "InstallationType";		            
		public const string Property_SignatureDate = "SignatureDate";		            
		public const string Property_InvoiceEmailAddress = "InvoiceEmailAddress";		            
		public const string Property_InvoiceCcEmailAddress = "InvoiceCcEmailAddress";		            
		public const string Property_MonitoringAmount = "MonitoringAmount";		            
		public const string Property_ContractTerm = "ContractTerm";		            
		public const string Property_MonitoringDescription = "MonitoringDescription";		            
		public const string Property_IsARBInvoice = "IsARBInvoice";		            
		public const string Property_TransactionId = "TransactionId";		            
		public const string Property_ForteStatus = "ForteStatus";		            
		public const string Property_UpfrontMonth = "UpfrontMonth";		            
		public const string Property_JobNo = "JobNo";		            
		public const string Property_TaxPercentage = "TaxPercentage";		            
		public const string Property_TicketId = "TicketId";		            
		public const string Property_ItemType = "ItemType";		            
		public const string Property_BatchNumber = "BatchNumber";		            
		public const string Property_FinanceCompany = "FinanceCompany";		            
		public const string Property_InvoiceContractDiagram = "InvoiceContractDiagram";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _InvoiceId;	            
		private Guid _CustomerId;	            
		private Guid _CompanyId;	            
		private Double _Amount;	            
		private Nullable<Double> _Tax;	            
		private String _DiscountCode;	            
		private Nullable<Double> _DiscountAmount;	            
		private Nullable<Double> _TotalAmount;	            
		private String _Status;	            
		private Nullable<DateTime> _InvoiceDate;	            
		private Boolean _IsEstimate;	            
		private Nullable<Boolean> _IsBill;	            
		private String _BillingAddress;	            
		private Nullable<DateTime> _DueDate;	            
		private String _Terms;	            
		private String _ShippingAddress;	            
		private String _ShippingVia;	            
		private Nullable<DateTime> _ShippingDate;	            
		private String _TrackingNo;	            
		private Nullable<Double> _ShippingCost;	            
		private Nullable<Double> _Discountpercent;	            
		private Nullable<Double> _BalanceDue;	            
		private Nullable<Double> _Deposit;	            
		private String _Message;	            
		private String _TaxType;	            
		private Nullable<Double> _Balance;	            
		private String _Memo;	            
		private String _InvoiceFor;	            
		private Nullable<Double> _LateFee;	            
		private Nullable<Double> _LateAmount;	            
		private Nullable<DateTime> _InstallDate;	            
		private String _Description;	            
		private String _DiscountType;	            
		private String _BillingCycle;	            
		private String _EstimateTerm;	            
		private String _Signature;	            
		private String _CancelReason;	            
		private DateTime _CreatedDate;	            
		private String _CreatedBy;	            
		private Guid _CreatedByUid;	            
		private DateTime _LastUpdatedDate;	            
		private Guid _LastUpdatedByUid;	            
		private String _RefType;	            
		private String _PaymentType;	            
		private String _BookingId;	            
		private String _InstallationType;	            
		private Nullable<DateTime> _SignatureDate;	            
		private String _InvoiceEmailAddress;	            
		private String _InvoiceCcEmailAddress;	            
		private Nullable<Double> _MonitoringAmount;	            
		private String _ContractTerm;	            
		private String _MonitoringDescription;	            
		private Nullable<Boolean> _IsARBInvoice;	            
		private String _TransactionId;	            
		private String _ForteStatus;	            
		private String _UpfrontMonth;	            
		private String _JobNo;	            
		private Nullable<Double> _TaxPercentage;	            
		private Guid _TicketId;	            
		private String _ItemType;	            
		private String _BatchNumber;	            
		private String _FinanceCompany;	            
		private String _InvoiceContractDiagram;	            
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
		public String InvoiceId
		{	
			get{ return _InvoiceId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InvoiceId, value, _InvoiceId);
				if (PropertyChanging(args))
				{
					_InvoiceId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid CustomerId
		{	
			get{ return _CustomerId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CustomerId, value, _CustomerId);
				if (PropertyChanging(args))
				{
					_CustomerId = value;
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
		public Nullable<DateTime> InvoiceDate
		{	
			get{ return _InvoiceDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InvoiceDate, value, _InvoiceDate);
				if (PropertyChanging(args))
				{
					_InvoiceDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean IsEstimate
		{	
			get{ return _IsEstimate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsEstimate, value, _IsEstimate);
				if (PropertyChanging(args))
				{
					_IsEstimate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsBill
		{	
			get{ return _IsBill; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsBill, value, _IsBill);
				if (PropertyChanging(args))
				{
					_IsBill = value;
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
		public Nullable<DateTime> DueDate
		{	
			get{ return _DueDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DueDate, value, _DueDate);
				if (PropertyChanging(args))
				{
					_DueDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Terms
		{	
			get{ return _Terms; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Terms, value, _Terms);
				if (PropertyChanging(args))
				{
					_Terms = value;
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
		public String Memo
		{	
			get{ return _Memo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Memo, value, _Memo);
				if (PropertyChanging(args))
				{
					_Memo = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String InvoiceFor
		{	
			get{ return _InvoiceFor; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InvoiceFor, value, _InvoiceFor);
				if (PropertyChanging(args))
				{
					_InvoiceFor = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> LateFee
		{	
			get{ return _LateFee; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LateFee, value, _LateFee);
				if (PropertyChanging(args))
				{
					_LateFee = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> LateAmount
		{	
			get{ return _LateAmount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LateAmount, value, _LateAmount);
				if (PropertyChanging(args))
				{
					_LateAmount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> InstallDate
		{	
			get{ return _InstallDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InstallDate, value, _InstallDate);
				if (PropertyChanging(args))
				{
					_InstallDate = value;
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
		public String BillingCycle
		{	
			get{ return _BillingCycle; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BillingCycle, value, _BillingCycle);
				if (PropertyChanging(args))
				{
					_BillingCycle = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String EstimateTerm
		{	
			get{ return _EstimateTerm; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EstimateTerm, value, _EstimateTerm);
				if (PropertyChanging(args))
				{
					_EstimateTerm = value;
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
		public String RefType
		{	
			get{ return _RefType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RefType, value, _RefType);
				if (PropertyChanging(args))
				{
					_RefType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PaymentType
		{	
			get{ return _PaymentType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PaymentType, value, _PaymentType);
				if (PropertyChanging(args))
				{
					_PaymentType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BookingId
		{	
			get{ return _BookingId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BookingId, value, _BookingId);
				if (PropertyChanging(args))
				{
					_BookingId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String InstallationType
		{	
			get{ return _InstallationType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InstallationType, value, _InstallationType);
				if (PropertyChanging(args))
				{
					_InstallationType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> SignatureDate
		{	
			get{ return _SignatureDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SignatureDate, value, _SignatureDate);
				if (PropertyChanging(args))
				{
					_SignatureDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String InvoiceEmailAddress
		{	
			get{ return _InvoiceEmailAddress; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InvoiceEmailAddress, value, _InvoiceEmailAddress);
				if (PropertyChanging(args))
				{
					_InvoiceEmailAddress = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String InvoiceCcEmailAddress
		{	
			get{ return _InvoiceCcEmailAddress; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InvoiceCcEmailAddress, value, _InvoiceCcEmailAddress);
				if (PropertyChanging(args))
				{
					_InvoiceCcEmailAddress = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> MonitoringAmount
		{	
			get{ return _MonitoringAmount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MonitoringAmount, value, _MonitoringAmount);
				if (PropertyChanging(args))
				{
					_MonitoringAmount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ContractTerm
		{	
			get{ return _ContractTerm; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ContractTerm, value, _ContractTerm);
				if (PropertyChanging(args))
				{
					_ContractTerm = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String MonitoringDescription
		{	
			get{ return _MonitoringDescription; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MonitoringDescription, value, _MonitoringDescription);
				if (PropertyChanging(args))
				{
					_MonitoringDescription = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsARBInvoice
		{	
			get{ return _IsARBInvoice; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsARBInvoice, value, _IsARBInvoice);
				if (PropertyChanging(args))
				{
					_IsARBInvoice = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TransactionId
		{	
			get{ return _TransactionId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TransactionId, value, _TransactionId);
				if (PropertyChanging(args))
				{
					_TransactionId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ForteStatus
		{	
			get{ return _ForteStatus; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ForteStatus, value, _ForteStatus);
				if (PropertyChanging(args))
				{
					_ForteStatus = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String UpfrontMonth
		{	
			get{ return _UpfrontMonth; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UpfrontMonth, value, _UpfrontMonth);
				if (PropertyChanging(args))
				{
					_UpfrontMonth = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String JobNo
		{	
			get{ return _JobNo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_JobNo, value, _JobNo);
				if (PropertyChanging(args))
				{
					_JobNo = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> TaxPercentage
		{	
			get{ return _TaxPercentage; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TaxPercentage, value, _TaxPercentage);
				if (PropertyChanging(args))
				{
					_TaxPercentage = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid TicketId
		{	
			get{ return _TicketId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TicketId, value, _TicketId);
				if (PropertyChanging(args))
				{
					_TicketId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ItemType
		{	
			get{ return _ItemType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ItemType, value, _ItemType);
				if (PropertyChanging(args))
				{
					_ItemType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BatchNumber
		{	
			get{ return _BatchNumber; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BatchNumber, value, _BatchNumber);
				if (PropertyChanging(args))
				{
					_BatchNumber = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String FinanceCompany
		{	
			get{ return _FinanceCompany; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FinanceCompany, value, _FinanceCompany);
				if (PropertyChanging(args))
				{
					_FinanceCompany = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String InvoiceContractDiagram
		{	
			get{ return _InvoiceContractDiagram; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InvoiceContractDiagram, value, _InvoiceContractDiagram);
				if (PropertyChanging(args))
				{
					_InvoiceContractDiagram = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  InvoiceBase Clone()
		{
			InvoiceBase newObj = new  InvoiceBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.InvoiceId = this.InvoiceId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.CompanyId = this.CompanyId;						
			newObj.Amount = this.Amount;						
			newObj.Tax = this.Tax;						
			newObj.DiscountCode = this.DiscountCode;						
			newObj.DiscountAmount = this.DiscountAmount;						
			newObj.TotalAmount = this.TotalAmount;						
			newObj.Status = this.Status;						
			newObj.InvoiceDate = this.InvoiceDate;						
			newObj.IsEstimate = this.IsEstimate;						
			newObj.IsBill = this.IsBill;						
			newObj.BillingAddress = this.BillingAddress;						
			newObj.DueDate = this.DueDate;						
			newObj.Terms = this.Terms;						
			newObj.ShippingAddress = this.ShippingAddress;						
			newObj.ShippingVia = this.ShippingVia;						
			newObj.ShippingDate = this.ShippingDate;						
			newObj.TrackingNo = this.TrackingNo;						
			newObj.ShippingCost = this.ShippingCost;						
			newObj.Discountpercent = this.Discountpercent;						
			newObj.BalanceDue = this.BalanceDue;						
			newObj.Deposit = this.Deposit;						
			newObj.Message = this.Message;						
			newObj.TaxType = this.TaxType;						
			newObj.Balance = this.Balance;						
			newObj.Memo = this.Memo;						
			newObj.InvoiceFor = this.InvoiceFor;						
			newObj.LateFee = this.LateFee;						
			newObj.LateAmount = this.LateAmount;						
			newObj.InstallDate = this.InstallDate;						
			newObj.Description = this.Description;						
			newObj.DiscountType = this.DiscountType;						
			newObj.BillingCycle = this.BillingCycle;						
			newObj.EstimateTerm = this.EstimateTerm;						
			newObj.Signature = this.Signature;						
			newObj.CancelReason = this.CancelReason;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedByUid = this.CreatedByUid;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.LastUpdatedByUid = this.LastUpdatedByUid;						
			newObj.RefType = this.RefType;						
			newObj.PaymentType = this.PaymentType;						
			newObj.BookingId = this.BookingId;						
			newObj.InstallationType = this.InstallationType;						
			newObj.SignatureDate = this.SignatureDate;						
			newObj.InvoiceEmailAddress = this.InvoiceEmailAddress;						
			newObj.InvoiceCcEmailAddress = this.InvoiceCcEmailAddress;						
			newObj.MonitoringAmount = this.MonitoringAmount;						
			newObj.ContractTerm = this.ContractTerm;						
			newObj.MonitoringDescription = this.MonitoringDescription;						
			newObj.IsARBInvoice = this.IsARBInvoice;						
			newObj.TransactionId = this.TransactionId;						
			newObj.ForteStatus = this.ForteStatus;						
			newObj.UpfrontMonth = this.UpfrontMonth;						
			newObj.JobNo = this.JobNo;						
			newObj.TaxPercentage = this.TaxPercentage;						
			newObj.TicketId = this.TicketId;						
			newObj.ItemType = this.ItemType;						
			newObj.BatchNumber = this.BatchNumber;						
			newObj.FinanceCompany = this.FinanceCompany;						
			newObj.InvoiceContractDiagram = this.InvoiceContractDiagram;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(InvoiceBase.Property_Id, Id);				
			info.AddValue(InvoiceBase.Property_InvoiceId, InvoiceId);				
			info.AddValue(InvoiceBase.Property_CustomerId, CustomerId);				
			info.AddValue(InvoiceBase.Property_CompanyId, CompanyId);				
			info.AddValue(InvoiceBase.Property_Amount, Amount);				
			info.AddValue(InvoiceBase.Property_Tax, Tax);				
			info.AddValue(InvoiceBase.Property_DiscountCode, DiscountCode);				
			info.AddValue(InvoiceBase.Property_DiscountAmount, DiscountAmount);				
			info.AddValue(InvoiceBase.Property_TotalAmount, TotalAmount);				
			info.AddValue(InvoiceBase.Property_Status, Status);				
			info.AddValue(InvoiceBase.Property_InvoiceDate, InvoiceDate);				
			info.AddValue(InvoiceBase.Property_IsEstimate, IsEstimate);				
			info.AddValue(InvoiceBase.Property_IsBill, IsBill);				
			info.AddValue(InvoiceBase.Property_BillingAddress, BillingAddress);				
			info.AddValue(InvoiceBase.Property_DueDate, DueDate);				
			info.AddValue(InvoiceBase.Property_Terms, Terms);				
			info.AddValue(InvoiceBase.Property_ShippingAddress, ShippingAddress);				
			info.AddValue(InvoiceBase.Property_ShippingVia, ShippingVia);				
			info.AddValue(InvoiceBase.Property_ShippingDate, ShippingDate);				
			info.AddValue(InvoiceBase.Property_TrackingNo, TrackingNo);				
			info.AddValue(InvoiceBase.Property_ShippingCost, ShippingCost);				
			info.AddValue(InvoiceBase.Property_Discountpercent, Discountpercent);				
			info.AddValue(InvoiceBase.Property_BalanceDue, BalanceDue);				
			info.AddValue(InvoiceBase.Property_Deposit, Deposit);				
			info.AddValue(InvoiceBase.Property_Message, Message);				
			info.AddValue(InvoiceBase.Property_TaxType, TaxType);				
			info.AddValue(InvoiceBase.Property_Balance, Balance);				
			info.AddValue(InvoiceBase.Property_Memo, Memo);				
			info.AddValue(InvoiceBase.Property_InvoiceFor, InvoiceFor);				
			info.AddValue(InvoiceBase.Property_LateFee, LateFee);				
			info.AddValue(InvoiceBase.Property_LateAmount, LateAmount);				
			info.AddValue(InvoiceBase.Property_InstallDate, InstallDate);				
			info.AddValue(InvoiceBase.Property_Description, Description);				
			info.AddValue(InvoiceBase.Property_DiscountType, DiscountType);				
			info.AddValue(InvoiceBase.Property_BillingCycle, BillingCycle);				
			info.AddValue(InvoiceBase.Property_EstimateTerm, EstimateTerm);				
			info.AddValue(InvoiceBase.Property_Signature, Signature);				
			info.AddValue(InvoiceBase.Property_CancelReason, CancelReason);				
			info.AddValue(InvoiceBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(InvoiceBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(InvoiceBase.Property_CreatedByUid, CreatedByUid);				
			info.AddValue(InvoiceBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(InvoiceBase.Property_LastUpdatedByUid, LastUpdatedByUid);				
			info.AddValue(InvoiceBase.Property_RefType, RefType);				
			info.AddValue(InvoiceBase.Property_PaymentType, PaymentType);				
			info.AddValue(InvoiceBase.Property_BookingId, BookingId);				
			info.AddValue(InvoiceBase.Property_InstallationType, InstallationType);				
			info.AddValue(InvoiceBase.Property_SignatureDate, SignatureDate);				
			info.AddValue(InvoiceBase.Property_InvoiceEmailAddress, InvoiceEmailAddress);				
			info.AddValue(InvoiceBase.Property_InvoiceCcEmailAddress, InvoiceCcEmailAddress);				
			info.AddValue(InvoiceBase.Property_MonitoringAmount, MonitoringAmount);				
			info.AddValue(InvoiceBase.Property_ContractTerm, ContractTerm);				
			info.AddValue(InvoiceBase.Property_MonitoringDescription, MonitoringDescription);				
			info.AddValue(InvoiceBase.Property_IsARBInvoice, IsARBInvoice);				
			info.AddValue(InvoiceBase.Property_TransactionId, TransactionId);				
			info.AddValue(InvoiceBase.Property_ForteStatus, ForteStatus);				
			info.AddValue(InvoiceBase.Property_UpfrontMonth, UpfrontMonth);				
			info.AddValue(InvoiceBase.Property_JobNo, JobNo);				
			info.AddValue(InvoiceBase.Property_TaxPercentage, TaxPercentage);				
			info.AddValue(InvoiceBase.Property_TicketId, TicketId);				
			info.AddValue(InvoiceBase.Property_ItemType, ItemType);				
			info.AddValue(InvoiceBase.Property_BatchNumber, BatchNumber);				
			info.AddValue(InvoiceBase.Property_FinanceCompany, FinanceCompany);				
			info.AddValue(InvoiceBase.Property_InvoiceContractDiagram, InvoiceContractDiagram);				
		}
		#endregion

		
	}
}
