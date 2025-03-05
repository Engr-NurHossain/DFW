using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomerCreditBase", Namespace = "http://www.hims-tech.com//entities")]
	public class CustomerCreditBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			CustomerId = 2,
			TransactionId = 3,
			Type = 4,
			Amount = 5,
			CreatedBy = 6,
			CreatedDate = 7,
			IsRefund = 8,
			Note = 9,
			IsRMRCredit = 10,
			IsDeleted = 11,
			CreditReason = 12,
			DebitRefId = 13,
			CreditType = 14
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_TransactionId = "TransactionId";		            
		public const string Property_Type = "Type";		            
		public const string Property_Amount = "Amount";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_IsRefund = "IsRefund";		            
		public const string Property_Note = "Note";		            
		public const string Property_IsRMRCredit = "IsRMRCredit";		            
		public const string Property_IsDeleted = "IsDeleted";		            
		public const string Property_CreditReason = "CreditReason";		            
		public const string Property_DebitRefId = "DebitRefId";		            
		public const string Property_CreditType = "CreditType";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _CustomerId;	            
		private Int32 _TransactionId;	            
		private String _Type;	            
		private Double _Amount;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
		private Nullable<Boolean> _IsRefund;	            
		private String _Note;	            
		private Nullable<Boolean> _IsRMRCredit;	            
		private Nullable<Boolean> _IsDeleted;	            
		private String _CreditReason;	            
		private Nullable<Int32> _DebitRefId;	            
		private String _CreditType;	            
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
		public Int32 TransactionId
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
		public Nullable<Boolean> IsRefund
		{	
			get{ return _IsRefund; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsRefund, value, _IsRefund);
				if (PropertyChanging(args))
				{
					_IsRefund = value;
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
		public Nullable<Boolean> IsRMRCredit
		{	
			get{ return _IsRMRCredit; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsRMRCredit, value, _IsRMRCredit);
				if (PropertyChanging(args))
				{
					_IsRMRCredit = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsDeleted
		{	
			get{ return _IsDeleted; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsDeleted, value, _IsDeleted);
				if (PropertyChanging(args))
				{
					_IsDeleted = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CreditReason
		{	
			get{ return _CreditReason; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreditReason, value, _CreditReason);
				if (PropertyChanging(args))
				{
					_CreditReason = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> DebitRefId
		{	
			get{ return _DebitRefId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DebitRefId, value, _DebitRefId);
				if (PropertyChanging(args))
				{
					_DebitRefId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CreditType
		{	
			get{ return _CreditType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreditType, value, _CreditType);
				if (PropertyChanging(args))
				{
					_CreditType = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  CustomerCreditBase Clone()
		{
			CustomerCreditBase newObj = new  CustomerCreditBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.TransactionId = this.TransactionId;						
			newObj.Type = this.Type;						
			newObj.Amount = this.Amount;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.IsRefund = this.IsRefund;						
			newObj.Note = this.Note;						
			newObj.IsRMRCredit = this.IsRMRCredit;						
			newObj.IsDeleted = this.IsDeleted;						
			newObj.CreditReason = this.CreditReason;						
			newObj.DebitRefId = this.DebitRefId;						
			newObj.CreditType = this.CreditType;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomerCreditBase.Property_Id, Id);				
			info.AddValue(CustomerCreditBase.Property_CompanyId, CompanyId);				
			info.AddValue(CustomerCreditBase.Property_CustomerId, CustomerId);				
			info.AddValue(CustomerCreditBase.Property_TransactionId, TransactionId);				
			info.AddValue(CustomerCreditBase.Property_Type, Type);				
			info.AddValue(CustomerCreditBase.Property_Amount, Amount);				
			info.AddValue(CustomerCreditBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(CustomerCreditBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(CustomerCreditBase.Property_IsRefund, IsRefund);				
			info.AddValue(CustomerCreditBase.Property_Note, Note);				
			info.AddValue(CustomerCreditBase.Property_IsRMRCredit, IsRMRCredit);				
			info.AddValue(CustomerCreditBase.Property_IsDeleted, IsDeleted);				
			info.AddValue(CustomerCreditBase.Property_CreditReason, CreditReason);				
			info.AddValue(CustomerCreditBase.Property_DebitRefId, DebitRefId);				
			info.AddValue(CustomerCreditBase.Property_CreditType, CreditType);				
		}
		#endregion

		
	}
}
