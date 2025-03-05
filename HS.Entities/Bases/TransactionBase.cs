using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "TransactionBase", Namespace = "http://www.hims-tech.com//entities")]
	public class TransactionBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			CustomerId = 2,
			Type = 3,
			Amount = 4,
			Status = 5,
			TransacationDate = 6,
			CardTransactionId = 7,
			PaymentMethod = 8,
			CheckNo = 9,
			ReferenceNo = 10,
			AddedBy = 11,
			AddedDate = 12,
			CreatedBy = 13,
			PaymentInfoId = 14,
			Note = 15,
			PaymentProfileId = 16,
			IsRMR = 17
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_Type = "Type";		            
		public const string Property_Amount = "Amount";		            
		public const string Property_Status = "Status";		            
		public const string Property_TransacationDate = "TransacationDate";		            
		public const string Property_CardTransactionId = "CardTransactionId";		            
		public const string Property_PaymentMethod = "PaymentMethod";		            
		public const string Property_CheckNo = "CheckNo";		            
		public const string Property_ReferenceNo = "ReferenceNo";		            
		public const string Property_AddedBy = "AddedBy";		            
		public const string Property_AddedDate = "AddedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_PaymentInfoId = "PaymentInfoId";		            
		public const string Property_Note = "Note";		            
		public const string Property_PaymentProfileId = "PaymentProfileId";		            
		public const string Property_IsRMR = "IsRMR";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _CustomerId;	            
		private String _Type;	            
		private Double _Amount;	            
		private String _Status;	            
		private DateTime _TransacationDate;	            
		private String _CardTransactionId;	            
		private String _PaymentMethod;	            
		private String _CheckNo;	            
		private String _ReferenceNo;	            
		private String _AddedBy;	            
		private Nullable<DateTime> _AddedDate;	            
		private Guid _CreatedBy;	            
		private Nullable<Int32> _PaymentInfoId;	            
		private String _Note;	            
		private Nullable<Int32> _PaymentProfileId;	            
		private Nullable<Boolean> _IsRMR;	            
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
		public DateTime TransacationDate
		{	
			get{ return _TransacationDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TransacationDate, value, _TransacationDate);
				if (PropertyChanging(args))
				{
					_TransacationDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CardTransactionId
		{	
			get{ return _CardTransactionId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CardTransactionId, value, _CardTransactionId);
				if (PropertyChanging(args))
				{
					_CardTransactionId = value;
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
		public String CheckNo
		{	
			get{ return _CheckNo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CheckNo, value, _CheckNo);
				if (PropertyChanging(args))
				{
					_CheckNo = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ReferenceNo
		{	
			get{ return _ReferenceNo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReferenceNo, value, _ReferenceNo);
				if (PropertyChanging(args))
				{
					_ReferenceNo = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AddedBy
		{	
			get{ return _AddedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AddedBy, value, _AddedBy);
				if (PropertyChanging(args))
				{
					_AddedBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> AddedDate
		{	
			get{ return _AddedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AddedDate, value, _AddedDate);
				if (PropertyChanging(args))
				{
					_AddedDate = value;
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
		public Nullable<Int32> PaymentInfoId
		{	
			get{ return _PaymentInfoId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PaymentInfoId, value, _PaymentInfoId);
				if (PropertyChanging(args))
				{
					_PaymentInfoId = value;
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
		public Nullable<Int32> PaymentProfileId
		{	
			get{ return _PaymentProfileId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PaymentProfileId, value, _PaymentProfileId);
				if (PropertyChanging(args))
				{
					_PaymentProfileId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsRMR
		{	
			get{ return _IsRMR; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsRMR, value, _IsRMR);
				if (PropertyChanging(args))
				{
					_IsRMR = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  TransactionBase Clone()
		{
			TransactionBase newObj = new  TransactionBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.Type = this.Type;						
			newObj.Amount = this.Amount;						
			newObj.Status = this.Status;						
			newObj.TransacationDate = this.TransacationDate;						
			newObj.CardTransactionId = this.CardTransactionId;						
			newObj.PaymentMethod = this.PaymentMethod;						
			newObj.CheckNo = this.CheckNo;						
			newObj.ReferenceNo = this.ReferenceNo;						
			newObj.AddedBy = this.AddedBy;						
			newObj.AddedDate = this.AddedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.PaymentInfoId = this.PaymentInfoId;						
			newObj.Note = this.Note;						
			newObj.PaymentProfileId = this.PaymentProfileId;						
			newObj.IsRMR = this.IsRMR;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(TransactionBase.Property_Id, Id);				
			info.AddValue(TransactionBase.Property_CompanyId, CompanyId);				
			info.AddValue(TransactionBase.Property_CustomerId, CustomerId);				
			info.AddValue(TransactionBase.Property_Type, Type);				
			info.AddValue(TransactionBase.Property_Amount, Amount);				
			info.AddValue(TransactionBase.Property_Status, Status);				
			info.AddValue(TransactionBase.Property_TransacationDate, TransacationDate);				
			info.AddValue(TransactionBase.Property_CardTransactionId, CardTransactionId);				
			info.AddValue(TransactionBase.Property_PaymentMethod, PaymentMethod);				
			info.AddValue(TransactionBase.Property_CheckNo, CheckNo);				
			info.AddValue(TransactionBase.Property_ReferenceNo, ReferenceNo);				
			info.AddValue(TransactionBase.Property_AddedBy, AddedBy);				
			info.AddValue(TransactionBase.Property_AddedDate, AddedDate);				
			info.AddValue(TransactionBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(TransactionBase.Property_PaymentInfoId, PaymentInfoId);				
			info.AddValue(TransactionBase.Property_Note, Note);				
			info.AddValue(TransactionBase.Property_PaymentProfileId, PaymentProfileId);				
			info.AddValue(TransactionBase.Property_IsRMR, IsRMR);				
		}
		#endregion

		
	}
}
