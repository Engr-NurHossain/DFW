using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomerCancellationQueueBase", Namespace = "http://www.hims-tech.com//entities")]
	public class CustomerCancellationQueueBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CancellationId = 1,
			CustomerId = 2,
			Reason = 3,
			RemainingBalance = 4,
			CancellationDate = 5,
			IsSigned = 6,
			CreatedDate = 7,
			CreatedBy = 8,
			IsActive = 9,
			Note = 10,
			IsCancelled = 11,
			IsInvoiceOff = 12,
			IsBillingOff = 13,
			IsAlarmOff = 14,
			EmployeeReason = 15,
			ExpirationDays = 16,
			ExpirationDate = 17,
			FileId = 18
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CancellationId = "CancellationId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_Reason = "Reason";		            
		public const string Property_RemainingBalance = "RemainingBalance";		            
		public const string Property_CancellationDate = "CancellationDate";		            
		public const string Property_IsSigned = "IsSigned";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_IsActive = "IsActive";		            
		public const string Property_Note = "Note";		            
		public const string Property_IsCancelled = "IsCancelled";		            
		public const string Property_IsInvoiceOff = "IsInvoiceOff";		            
		public const string Property_IsBillingOff = "IsBillingOff";		            
		public const string Property_IsAlarmOff = "IsAlarmOff";		            
		public const string Property_EmployeeReason = "EmployeeReason";		            
		public const string Property_ExpirationDays = "ExpirationDays";		            
		public const string Property_ExpirationDate = "ExpirationDate";		            
		public const string Property_FileId = "FileId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CancellationId;	            
		private Guid _CustomerId;	            
		private String _Reason;	            
		private Nullable<Double> _RemainingBalance;	            
		private Nullable<DateTime> _CancellationDate;	            
		private Nullable<Boolean> _IsSigned;	            
		private Nullable<DateTime> _CreatedDate;	            
		private Guid _CreatedBy;	            
		private Nullable<Boolean> _IsActive;	            
		private String _Note;	            
		private Nullable<Boolean> _IsCancelled;	            
		private Nullable<Boolean> _IsInvoiceOff;	            
		private Nullable<Boolean> _IsBillingOff;	            
		private Nullable<Boolean> _IsAlarmOff;	            
		private String _EmployeeReason;	            
		private Nullable<Int32> _ExpirationDays;	            
		private Nullable<DateTime> _ExpirationDate;	            
		private Guid _FileId;	            
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
		public Guid CancellationId
		{	
			get{ return _CancellationId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CancellationId, value, _CancellationId);
				if (PropertyChanging(args))
				{
					_CancellationId = value;
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
		public String Reason
		{	
			get{ return _Reason; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Reason, value, _Reason);
				if (PropertyChanging(args))
				{
					_Reason = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> RemainingBalance
		{	
			get{ return _RemainingBalance; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RemainingBalance, value, _RemainingBalance);
				if (PropertyChanging(args))
				{
					_RemainingBalance = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> CancellationDate
		{	
			get{ return _CancellationDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CancellationDate, value, _CancellationDate);
				if (PropertyChanging(args))
				{
					_CancellationDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsSigned
		{	
			get{ return _IsSigned; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsSigned, value, _IsSigned);
				if (PropertyChanging(args))
				{
					_IsSigned = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> CreatedDate
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
		public Nullable<Boolean> IsActive
		{	
			get{ return _IsActive; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsActive, value, _IsActive);
				if (PropertyChanging(args))
				{
					_IsActive = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Note
		{	
			get{ return _Note; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Note, value, _Note);
				if (PropertyChanging(args))
				{
					_Note = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsCancelled
		{	
			get{ return _IsCancelled; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsCancelled, value, _IsCancelled);
				if (PropertyChanging(args))
				{
					_IsCancelled = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsInvoiceOff
		{	
			get{ return _IsInvoiceOff; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsInvoiceOff, value, _IsInvoiceOff);
				if (PropertyChanging(args))
				{
					_IsInvoiceOff = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsBillingOff
		{	
			get{ return _IsBillingOff; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsBillingOff, value, _IsBillingOff);
				if (PropertyChanging(args))
				{
					_IsBillingOff = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsAlarmOff
		{	
			get{ return _IsAlarmOff; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsAlarmOff, value, _IsAlarmOff);
				if (PropertyChanging(args))
				{
					_IsAlarmOff = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String EmployeeReason
		{	
			get{ return _EmployeeReason; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EmployeeReason, value, _EmployeeReason);
				if (PropertyChanging(args))
				{
					_EmployeeReason = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> ExpirationDays
		{	
			get{ return _ExpirationDays; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ExpirationDays, value, _ExpirationDays);
				if (PropertyChanging(args))
				{
					_ExpirationDays = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> ExpirationDate
		{	
			get{ return _ExpirationDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ExpirationDate, value, _ExpirationDate);
				if (PropertyChanging(args))
				{
					_ExpirationDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid FileId
		{	
			get{ return _FileId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FileId, value, _FileId);
				if (PropertyChanging(args))
				{
					_FileId = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  CustomerCancellationQueueBase Clone()
		{
			CustomerCancellationQueueBase newObj = new  CustomerCancellationQueueBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CancellationId = this.CancellationId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.Reason = this.Reason;						
			newObj.RemainingBalance = this.RemainingBalance;						
			newObj.CancellationDate = this.CancellationDate;						
			newObj.IsSigned = this.IsSigned;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.IsActive = this.IsActive;						
			newObj.Note = this.Note;						
			newObj.IsCancelled = this.IsCancelled;						
			newObj.IsInvoiceOff = this.IsInvoiceOff;						
			newObj.IsBillingOff = this.IsBillingOff;						
			newObj.IsAlarmOff = this.IsAlarmOff;						
			newObj.EmployeeReason = this.EmployeeReason;						
			newObj.ExpirationDays = this.ExpirationDays;						
			newObj.ExpirationDate = this.ExpirationDate;						
			newObj.FileId = this.FileId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomerCancellationQueueBase.Property_Id, Id);				
			info.AddValue(CustomerCancellationQueueBase.Property_CancellationId, CancellationId);				
			info.AddValue(CustomerCancellationQueueBase.Property_CustomerId, CustomerId);				
			info.AddValue(CustomerCancellationQueueBase.Property_Reason, Reason);				
			info.AddValue(CustomerCancellationQueueBase.Property_RemainingBalance, RemainingBalance);				
			info.AddValue(CustomerCancellationQueueBase.Property_CancellationDate, CancellationDate);				
			info.AddValue(CustomerCancellationQueueBase.Property_IsSigned, IsSigned);				
			info.AddValue(CustomerCancellationQueueBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(CustomerCancellationQueueBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(CustomerCancellationQueueBase.Property_IsActive, IsActive);				
			info.AddValue(CustomerCancellationQueueBase.Property_Note, Note);				
			info.AddValue(CustomerCancellationQueueBase.Property_IsCancelled, IsCancelled);				
			info.AddValue(CustomerCancellationQueueBase.Property_IsInvoiceOff, IsInvoiceOff);				
			info.AddValue(CustomerCancellationQueueBase.Property_IsBillingOff, IsBillingOff);				
			info.AddValue(CustomerCancellationQueueBase.Property_IsAlarmOff, IsAlarmOff);				
			info.AddValue(CustomerCancellationQueueBase.Property_EmployeeReason, EmployeeReason);				
			info.AddValue(CustomerCancellationQueueBase.Property_ExpirationDays, ExpirationDays);				
			info.AddValue(CustomerCancellationQueueBase.Property_ExpirationDate, ExpirationDate);				
			info.AddValue(CustomerCancellationQueueBase.Property_FileId, FileId);				
		}
		#endregion

		
	}
}
