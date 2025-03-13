using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "RecurringBillingScheduleBase", Namespace = "http://www.piistech.com//entities")]
	public class RecurringBillingScheduleBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			ScheduleId = 1,
			CompanyId = 2,
			CustomerId = 3,
			TemplateName = 4,
			EmailAddress = 5,
			CCEmail = 6,
			BCCEmail = 7,
			Status = 8,
			AutomaticallySendEmail = 9,
			IncludeOpenInvoices = 10,
			CollectOnline = 11,
			CustomerPaymentProfileId = 12,
			BillCycle = 13,
			Interval = 14,
			StartDate = 15,
			EndDate = 16,
			BillingAddress = 17,
			BillAmount = 18,
			TaxPercentage = 19,
			TaxAmount = 20,
			TotalBillAmount = 21,
			MessageOnInvoice = 22,
			CreatedBy = 23,
			CreatedDate = 24,
			LastUpdatedBy = 25,
			LastUpdatedDate = 26,
			DayInAdvance = 27,
			PreviousDate = 28,
			NextDate = 29,
			OthersUnpaidBill = 30,
			PaymentMethod = 31,
			PaymentCollectionDate = 32,
			IsEInvoice = 33,
			IsEReceipt = 34,
			LastRMRInvoiceRefId = 35,
			IsReplacement = 36,
			IsTransfer = 37,
			IsFCReplacement = 38,
			IsPOO = 39
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_ScheduleId = "ScheduleId";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_TemplateName = "TemplateName";		            
		public const string Property_EmailAddress = "EmailAddress";		            
		public const string Property_CCEmail = "CCEmail";		            
		public const string Property_BCCEmail = "BCCEmail";		            
		public const string Property_Status = "Status";		            
		public const string Property_AutomaticallySendEmail = "AutomaticallySendEmail";		            
		public const string Property_IncludeOpenInvoices = "IncludeOpenInvoices";		            
		public const string Property_CollectOnline = "CollectOnline";		            
		public const string Property_CustomerPaymentProfileId = "CustomerPaymentProfileId";		            
		public const string Property_BillCycle = "BillCycle";		            
		public const string Property_Interval = "Interval";		            
		public const string Property_StartDate = "StartDate";		            
		public const string Property_EndDate = "EndDate";		            
		public const string Property_BillingAddress = "BillingAddress";		            
		public const string Property_BillAmount = "BillAmount";		            
		public const string Property_TaxPercentage = "TaxPercentage";		            
		public const string Property_TaxAmount = "TaxAmount";		            
		public const string Property_TotalBillAmount = "TotalBillAmount";		            
		public const string Property_MessageOnInvoice = "MessageOnInvoice";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_DayInAdvance = "DayInAdvance";		            
		public const string Property_PreviousDate = "PreviousDate";		            
		public const string Property_NextDate = "NextDate";		            
		public const string Property_OthersUnpaidBill = "OthersUnpaidBill";		            
		public const string Property_PaymentMethod = "PaymentMethod";		            
		public const string Property_PaymentCollectionDate = "PaymentCollectionDate";		            
		public const string Property_IsEInvoice = "IsEInvoice";		            
		public const string Property_IsEReceipt = "IsEReceipt";		            
		public const string Property_LastRMRInvoiceRefId = "LastRMRInvoiceRefId";		            
		public const string Property_IsReplacement = "IsReplacement";		            
		public const string Property_IsTransfer = "IsTransfer";		            
		public const string Property_IsFCReplacement = "IsFCReplacement";		            
		public const string Property_IsPOO = "IsPOO";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _ScheduleId;	            
		private Guid _CompanyId;	            
		private Guid _CustomerId;	            
		private String _TemplateName;	            
		private String _EmailAddress;	            
		private String _CCEmail;	            
		private String _BCCEmail;	            
		private String _Status;	            
		private Boolean _AutomaticallySendEmail;	            
		private Boolean _IncludeOpenInvoices;	            
		private Boolean _CollectOnline;	            
		private Nullable<Int32> _CustomerPaymentProfileId;	            
		private String _BillCycle;	            
		private Int32 _Interval;	            
		private Nullable<DateTime> _StartDate;	            
		private Nullable<DateTime> _EndDate;	            
		private String _BillingAddress;	            
		private Double _BillAmount;	            
		private Double _TaxPercentage;	            
		private Double _TaxAmount;	            
		private Double _TotalBillAmount;	            
		private String _MessageOnInvoice;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
		private Guid _LastUpdatedBy;	            
		private DateTime _LastUpdatedDate;	            
		private Nullable<Int32> _DayInAdvance;	            
		private Nullable<DateTime> _PreviousDate;	            
		private Nullable<DateTime> _NextDate;	            
		private Nullable<Boolean> _OthersUnpaidBill;	            
		private String _PaymentMethod;	            
		private Nullable<DateTime> _PaymentCollectionDate;	            
		private Nullable<Boolean> _IsEInvoice;	            
		private Nullable<Boolean> _IsEReceipt;	            
		private String _LastRMRInvoiceRefId;	            
		private Nullable<Boolean> _IsReplacement;	            
		private Nullable<Boolean> _IsTransfer;	            
		private Nullable<Boolean> _IsFCReplacement;	            
		private Nullable<Boolean> _IsPOO;	            
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
		public Guid ScheduleId
		{	
			get{ return _ScheduleId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ScheduleId, value, _ScheduleId);
				if (PropertyChanging(args))
				{
					_ScheduleId = value;
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
		public String TemplateName
		{	
			get{ return _TemplateName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TemplateName, value, _TemplateName);
				if (PropertyChanging(args))
				{
					_TemplateName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String EmailAddress
		{	
			get{ return _EmailAddress; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EmailAddress, value, _EmailAddress);
				if (PropertyChanging(args))
				{
					_EmailAddress = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CCEmail
		{	
			get{ return _CCEmail; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CCEmail, value, _CCEmail);
				if (PropertyChanging(args))
				{
					_CCEmail = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BCCEmail
		{	
			get{ return _BCCEmail; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BCCEmail, value, _BCCEmail);
				if (PropertyChanging(args))
				{
					_BCCEmail = value;
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
		public Boolean AutomaticallySendEmail
		{	
			get{ return _AutomaticallySendEmail; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AutomaticallySendEmail, value, _AutomaticallySendEmail);
				if (PropertyChanging(args))
				{
					_AutomaticallySendEmail = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean IncludeOpenInvoices
		{	
			get{ return _IncludeOpenInvoices; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IncludeOpenInvoices, value, _IncludeOpenInvoices);
				if (PropertyChanging(args))
				{
					_IncludeOpenInvoices = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean CollectOnline
		{	
			get{ return _CollectOnline; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CollectOnline, value, _CollectOnline);
				if (PropertyChanging(args))
				{
					_CollectOnline = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> CustomerPaymentProfileId
		{	
			get{ return _CustomerPaymentProfileId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CustomerPaymentProfileId, value, _CustomerPaymentProfileId);
				if (PropertyChanging(args))
				{
					_CustomerPaymentProfileId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BillCycle
		{	
			get{ return _BillCycle; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BillCycle, value, _BillCycle);
				if (PropertyChanging(args))
				{
					_BillCycle = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Int32 Interval
		{	
			get{ return _Interval; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Interval, value, _Interval);
				if (PropertyChanging(args))
				{
					_Interval = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> StartDate
		{	
			get{ return _StartDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_StartDate, value, _StartDate);
				if (PropertyChanging(args))
				{
					_StartDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> EndDate
		{	
			get{ return _EndDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EndDate, value, _EndDate);
				if (PropertyChanging(args))
				{
					_EndDate = value;
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
		public Double BillAmount
		{	
			get{ return _BillAmount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BillAmount, value, _BillAmount);
				if (PropertyChanging(args))
				{
					_BillAmount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Double TaxPercentage
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
		public Double TaxAmount
		{	
			get{ return _TaxAmount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TaxAmount, value, _TaxAmount);
				if (PropertyChanging(args))
				{
					_TaxAmount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Double TotalBillAmount
		{	
			get{ return _TotalBillAmount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TotalBillAmount, value, _TotalBillAmount);
				if (PropertyChanging(args))
				{
					_TotalBillAmount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String MessageOnInvoice
		{	
			get{ return _MessageOnInvoice; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MessageOnInvoice, value, _MessageOnInvoice);
				if (PropertyChanging(args))
				{
					_MessageOnInvoice = value;
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
		public Nullable<Int32> DayInAdvance
		{	
			get{ return _DayInAdvance; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DayInAdvance, value, _DayInAdvance);
				if (PropertyChanging(args))
				{
					_DayInAdvance = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> PreviousDate
		{	
			get{ return _PreviousDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PreviousDate, value, _PreviousDate);
				if (PropertyChanging(args))
				{
					_PreviousDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> NextDate
		{	
			get{ return _NextDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_NextDate, value, _NextDate);
				if (PropertyChanging(args))
				{
					_NextDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> OthersUnpaidBill
		{	
			get{ return _OthersUnpaidBill; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OthersUnpaidBill, value, _OthersUnpaidBill);
				if (PropertyChanging(args))
				{
					_OthersUnpaidBill = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PaymentMethod
		{	
			get{ return _PaymentMethod; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PaymentMethod, value, _PaymentMethod);
				if (PropertyChanging(args))
				{
					_PaymentMethod = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> PaymentCollectionDate
		{	
			get{ return _PaymentCollectionDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PaymentCollectionDate, value, _PaymentCollectionDate);
				if (PropertyChanging(args))
				{
					_PaymentCollectionDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsEInvoice
		{	
			get{ return _IsEInvoice; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsEInvoice, value, _IsEInvoice);
				if (PropertyChanging(args))
				{
					_IsEInvoice = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsEReceipt
		{	
			get{ return _IsEReceipt; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsEReceipt, value, _IsEReceipt);
				if (PropertyChanging(args))
				{
					_IsEReceipt = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String LastRMRInvoiceRefId
		{	
			get{ return _LastRMRInvoiceRefId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastRMRInvoiceRefId, value, _LastRMRInvoiceRefId);
				if (PropertyChanging(args))
				{
					_LastRMRInvoiceRefId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsReplacement
		{	
			get{ return _IsReplacement; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsReplacement, value, _IsReplacement);
				if (PropertyChanging(args))
				{
					_IsReplacement = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsTransfer
		{	
			get{ return _IsTransfer; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsTransfer, value, _IsTransfer);
				if (PropertyChanging(args))
				{
					_IsTransfer = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsFCReplacement
		{	
			get{ return _IsFCReplacement; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsFCReplacement, value, _IsFCReplacement);
				if (PropertyChanging(args))
				{
					_IsFCReplacement = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsPOO
		{	
			get{ return _IsPOO; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsPOO, value, _IsPOO);
				if (PropertyChanging(args))
				{
					_IsPOO = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  RecurringBillingScheduleBase Clone()
		{
			RecurringBillingScheduleBase newObj = new  RecurringBillingScheduleBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.ScheduleId = this.ScheduleId;						
			newObj.CompanyId = this.CompanyId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.TemplateName = this.TemplateName;						
			newObj.EmailAddress = this.EmailAddress;						
			newObj.CCEmail = this.CCEmail;						
			newObj.BCCEmail = this.BCCEmail;						
			newObj.Status = this.Status;						
			newObj.AutomaticallySendEmail = this.AutomaticallySendEmail;						
			newObj.IncludeOpenInvoices = this.IncludeOpenInvoices;						
			newObj.CollectOnline = this.CollectOnline;						
			newObj.CustomerPaymentProfileId = this.CustomerPaymentProfileId;						
			newObj.BillCycle = this.BillCycle;						
			newObj.Interval = this.Interval;						
			newObj.StartDate = this.StartDate;						
			newObj.EndDate = this.EndDate;						
			newObj.BillingAddress = this.BillingAddress;						
			newObj.BillAmount = this.BillAmount;						
			newObj.TaxPercentage = this.TaxPercentage;						
			newObj.TaxAmount = this.TaxAmount;						
			newObj.TotalBillAmount = this.TotalBillAmount;						
			newObj.MessageOnInvoice = this.MessageOnInvoice;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.DayInAdvance = this.DayInAdvance;						
			newObj.PreviousDate = this.PreviousDate;						
			newObj.NextDate = this.NextDate;						
			newObj.OthersUnpaidBill = this.OthersUnpaidBill;						
			newObj.PaymentMethod = this.PaymentMethod;						
			newObj.PaymentCollectionDate = this.PaymentCollectionDate;						
			newObj.IsEInvoice = this.IsEInvoice;						
			newObj.IsEReceipt = this.IsEReceipt;						
			newObj.LastRMRInvoiceRefId = this.LastRMRInvoiceRefId;						
			newObj.IsReplacement = this.IsReplacement;						
			newObj.IsTransfer = this.IsTransfer;						
			newObj.IsFCReplacement = this.IsFCReplacement;						
			newObj.IsPOO = this.IsPOO;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(RecurringBillingScheduleBase.Property_Id, Id);				
			info.AddValue(RecurringBillingScheduleBase.Property_ScheduleId, ScheduleId);				
			info.AddValue(RecurringBillingScheduleBase.Property_CompanyId, CompanyId);				
			info.AddValue(RecurringBillingScheduleBase.Property_CustomerId, CustomerId);				
			info.AddValue(RecurringBillingScheduleBase.Property_TemplateName, TemplateName);				
			info.AddValue(RecurringBillingScheduleBase.Property_EmailAddress, EmailAddress);				
			info.AddValue(RecurringBillingScheduleBase.Property_CCEmail, CCEmail);				
			info.AddValue(RecurringBillingScheduleBase.Property_BCCEmail, BCCEmail);				
			info.AddValue(RecurringBillingScheduleBase.Property_Status, Status);				
			info.AddValue(RecurringBillingScheduleBase.Property_AutomaticallySendEmail, AutomaticallySendEmail);				
			info.AddValue(RecurringBillingScheduleBase.Property_IncludeOpenInvoices, IncludeOpenInvoices);				
			info.AddValue(RecurringBillingScheduleBase.Property_CollectOnline, CollectOnline);				
			info.AddValue(RecurringBillingScheduleBase.Property_CustomerPaymentProfileId, CustomerPaymentProfileId);				
			info.AddValue(RecurringBillingScheduleBase.Property_BillCycle, BillCycle);				
			info.AddValue(RecurringBillingScheduleBase.Property_Interval, Interval);				
			info.AddValue(RecurringBillingScheduleBase.Property_StartDate, StartDate);				
			info.AddValue(RecurringBillingScheduleBase.Property_EndDate, EndDate);				
			info.AddValue(RecurringBillingScheduleBase.Property_BillingAddress, BillingAddress);				
			info.AddValue(RecurringBillingScheduleBase.Property_BillAmount, BillAmount);				
			info.AddValue(RecurringBillingScheduleBase.Property_TaxPercentage, TaxPercentage);				
			info.AddValue(RecurringBillingScheduleBase.Property_TaxAmount, TaxAmount);				
			info.AddValue(RecurringBillingScheduleBase.Property_TotalBillAmount, TotalBillAmount);				
			info.AddValue(RecurringBillingScheduleBase.Property_MessageOnInvoice, MessageOnInvoice);				
			info.AddValue(RecurringBillingScheduleBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(RecurringBillingScheduleBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(RecurringBillingScheduleBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(RecurringBillingScheduleBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(RecurringBillingScheduleBase.Property_DayInAdvance, DayInAdvance);				
			info.AddValue(RecurringBillingScheduleBase.Property_PreviousDate, PreviousDate);				
			info.AddValue(RecurringBillingScheduleBase.Property_NextDate, NextDate);				
			info.AddValue(RecurringBillingScheduleBase.Property_OthersUnpaidBill, OthersUnpaidBill);				
			info.AddValue(RecurringBillingScheduleBase.Property_PaymentMethod, PaymentMethod);				
			info.AddValue(RecurringBillingScheduleBase.Property_PaymentCollectionDate, PaymentCollectionDate);				
			info.AddValue(RecurringBillingScheduleBase.Property_IsEInvoice, IsEInvoice);				
			info.AddValue(RecurringBillingScheduleBase.Property_IsEReceipt, IsEReceipt);				
			info.AddValue(RecurringBillingScheduleBase.Property_LastRMRInvoiceRefId, LastRMRInvoiceRefId);				
			info.AddValue(RecurringBillingScheduleBase.Property_IsReplacement, IsReplacement);				
			info.AddValue(RecurringBillingScheduleBase.Property_IsTransfer, IsTransfer);				
			info.AddValue(RecurringBillingScheduleBase.Property_IsFCReplacement, IsFCReplacement);				
			info.AddValue(RecurringBillingScheduleBase.Property_IsPOO, IsPOO);				
		}
		#endregion

		
	}
}