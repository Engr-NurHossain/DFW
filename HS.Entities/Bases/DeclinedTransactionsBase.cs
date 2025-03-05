using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "DeclinedTransactionsBase", Namespace = "http://www.piistech.com//entities")]
	public class DeclinedTransactionsBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			CustomerId = 2,
			TransactionId = 3,
			InvoiceId = 4,
			Reason = 5,
			ReturnedDate = 6,
			ReturnAmount = 7,
			SubmitAmount = 8,
			SettlementBatch = 9,
			Comment = 10
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_TransactionId = "TransactionId";		            
		public const string Property_InvoiceId = "InvoiceId";		            
		public const string Property_Reason = "Reason";		            
		public const string Property_ReturnedDate = "ReturnedDate";		            
		public const string Property_ReturnAmount = "ReturnAmount";		            
		public const string Property_SubmitAmount = "SubmitAmount";		            
		public const string Property_SettlementBatch = "SettlementBatch";		            
		public const string Property_Comment = "Comment";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _CustomerId;	            
		private String _TransactionId;	            
		private String _InvoiceId;	            
		private String _Reason;	            
		private Nullable<DateTime> _ReturnedDate;	            
		private Nullable<Double> _ReturnAmount;	            
		private Nullable<Double> _SubmitAmount;	            
		private Nullable<DateTime> _SettlementBatch;	            
		private String _Comment;	            
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
		public Nullable<DateTime> ReturnedDate
		{	
			get{ return _ReturnedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReturnedDate, value, _ReturnedDate);
				if (PropertyChanging(args))
				{
					_ReturnedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> ReturnAmount
		{	
			get{ return _ReturnAmount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReturnAmount, value, _ReturnAmount);
				if (PropertyChanging(args))
				{
					_ReturnAmount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> SubmitAmount
		{	
			get{ return _SubmitAmount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SubmitAmount, value, _SubmitAmount);
				if (PropertyChanging(args))
				{
					_SubmitAmount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> SettlementBatch
		{	
			get{ return _SettlementBatch; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SettlementBatch, value, _SettlementBatch);
				if (PropertyChanging(args))
				{
					_SettlementBatch = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Comment
		{	
			get{ return _Comment; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Comment, value, _Comment);
				if (PropertyChanging(args))
				{
					_Comment = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  DeclinedTransactionsBase Clone()
		{
			DeclinedTransactionsBase newObj = new  DeclinedTransactionsBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.TransactionId = this.TransactionId;						
			newObj.InvoiceId = this.InvoiceId;						
			newObj.Reason = this.Reason;						
			newObj.ReturnedDate = this.ReturnedDate;						
			newObj.ReturnAmount = this.ReturnAmount;						
			newObj.SubmitAmount = this.SubmitAmount;						
			newObj.SettlementBatch = this.SettlementBatch;						
			newObj.Comment = this.Comment;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(DeclinedTransactionsBase.Property_Id, Id);				
			info.AddValue(DeclinedTransactionsBase.Property_CompanyId, CompanyId);				
			info.AddValue(DeclinedTransactionsBase.Property_CustomerId, CustomerId);				
			info.AddValue(DeclinedTransactionsBase.Property_TransactionId, TransactionId);				
			info.AddValue(DeclinedTransactionsBase.Property_InvoiceId, InvoiceId);				
			info.AddValue(DeclinedTransactionsBase.Property_Reason, Reason);				
			info.AddValue(DeclinedTransactionsBase.Property_ReturnedDate, ReturnedDate);				
			info.AddValue(DeclinedTransactionsBase.Property_ReturnAmount, ReturnAmount);				
			info.AddValue(DeclinedTransactionsBase.Property_SubmitAmount, SubmitAmount);				
			info.AddValue(DeclinedTransactionsBase.Property_SettlementBatch, SettlementBatch);				
			info.AddValue(DeclinedTransactionsBase.Property_Comment, Comment);				
		}
		#endregion

		
	}
}
