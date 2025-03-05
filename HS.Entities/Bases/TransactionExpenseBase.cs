using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "TransactionExpenseBase", Namespace = "http://www.hims-tech.com//entities")]
	public class TransactionExpenseBase : BaseBusinessEntity
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
			ExpenseDate = 6,
			CardTransactionId = 7,
			PaymentMethod = 8,
			CheckNo = 9,
			ReferenceNo = 10,
			Description = 11,
			CreatedDate = 12,
			CreatedBy = 13,
			RefType = 14,
			UserId = 15,
			ExpenseType = 16,
			FilePath = 17,
			TicketNo = 18
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_Type = "Type";		            
		public const string Property_Amount = "Amount";		            
		public const string Property_Status = "Status";		            
		public const string Property_ExpenseDate = "ExpenseDate";		            
		public const string Property_CardTransactionId = "CardTransactionId";		            
		public const string Property_PaymentMethod = "PaymentMethod";		            
		public const string Property_CheckNo = "CheckNo";		            
		public const string Property_ReferenceNo = "ReferenceNo";		            
		public const string Property_Description = "Description";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_RefType = "RefType";		            
		public const string Property_UserId = "UserId";		            
		public const string Property_ExpenseType = "ExpenseType";		            
		public const string Property_FilePath = "FilePath";		            
		public const string Property_TicketNo = "TicketNo";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _CustomerId;	            
		private String _Type;	            
		private Double _Amount;	            
		private String _Status;	            
		private DateTime _ExpenseDate;	            
		private String _CardTransactionId;	            
		private String _PaymentMethod;	            
		private String _CheckNo;	            
		private String _ReferenceNo;	            
		private String _Description;	            
		private DateTime _CreatedDate;	            
		private Guid _CreatedBy;	            
		private String _RefType;	            
		private Guid _UserId;	            
		private String _ExpenseType;	            
		private String _FilePath;	            
		private Nullable<Int32> _TicketNo;	            
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
		public DateTime ExpenseDate
		{	
			get{ return _ExpenseDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ExpenseDate, value, _ExpenseDate);
				if (PropertyChanging(args))
				{
					_ExpenseDate = value;
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
		public Guid UserId
		{	
			get{ return _UserId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UserId, value, _UserId);
				if (PropertyChanging(args))
				{
					_UserId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ExpenseType
		{	
			get{ return _ExpenseType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ExpenseType, value, _ExpenseType);
				if (PropertyChanging(args))
				{
					_ExpenseType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String FilePath
		{	
			get{ return _FilePath; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FilePath, value, _FilePath);
				if (PropertyChanging(args))
				{
					_FilePath = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> TicketNo
		{	
			get{ return _TicketNo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TicketNo, value, _TicketNo);
				if (PropertyChanging(args))
				{
					_TicketNo = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  TransactionExpenseBase Clone()
		{
			TransactionExpenseBase newObj = new  TransactionExpenseBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.Type = this.Type;						
			newObj.Amount = this.Amount;						
			newObj.Status = this.Status;						
			newObj.ExpenseDate = this.ExpenseDate;						
			newObj.CardTransactionId = this.CardTransactionId;						
			newObj.PaymentMethod = this.PaymentMethod;						
			newObj.CheckNo = this.CheckNo;						
			newObj.ReferenceNo = this.ReferenceNo;						
			newObj.Description = this.Description;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.RefType = this.RefType;						
			newObj.UserId = this.UserId;						
			newObj.ExpenseType = this.ExpenseType;						
			newObj.FilePath = this.FilePath;						
			newObj.TicketNo = this.TicketNo;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(TransactionExpenseBase.Property_Id, Id);				
			info.AddValue(TransactionExpenseBase.Property_CompanyId, CompanyId);				
			info.AddValue(TransactionExpenseBase.Property_CustomerId, CustomerId);				
			info.AddValue(TransactionExpenseBase.Property_Type, Type);				
			info.AddValue(TransactionExpenseBase.Property_Amount, Amount);				
			info.AddValue(TransactionExpenseBase.Property_Status, Status);				
			info.AddValue(TransactionExpenseBase.Property_ExpenseDate, ExpenseDate);				
			info.AddValue(TransactionExpenseBase.Property_CardTransactionId, CardTransactionId);				
			info.AddValue(TransactionExpenseBase.Property_PaymentMethod, PaymentMethod);				
			info.AddValue(TransactionExpenseBase.Property_CheckNo, CheckNo);				
			info.AddValue(TransactionExpenseBase.Property_ReferenceNo, ReferenceNo);				
			info.AddValue(TransactionExpenseBase.Property_Description, Description);				
			info.AddValue(TransactionExpenseBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(TransactionExpenseBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(TransactionExpenseBase.Property_RefType, RefType);				
			info.AddValue(TransactionExpenseBase.Property_UserId, UserId);				
			info.AddValue(TransactionExpenseBase.Property_ExpenseType, ExpenseType);				
			info.AddValue(TransactionExpenseBase.Property_FilePath, FilePath);				
			info.AddValue(TransactionExpenseBase.Property_TicketNo, TicketNo);				
		}
		#endregion

		
	}
}
