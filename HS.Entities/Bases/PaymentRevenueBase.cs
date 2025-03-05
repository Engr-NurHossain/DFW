using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "PaymentRevenueBase", Namespace = "http://www.piistech.com//entities")]
	public class PaymentRevenueBase : BaseBusinessEntity
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
			ReferenceNo = 9,
			AddedBy = 10,
			AddedDate = 11,
			PaymentInfoId = 12,
			Desccription = 13,
			WorkOrder = 14
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
		public const string Property_ReferenceNo = "ReferenceNo";		            
		public const string Property_AddedBy = "AddedBy";		            
		public const string Property_AddedDate = "AddedDate";		            
		public const string Property_PaymentInfoId = "PaymentInfoId";		            
		public const string Property_Desccription = "Desccription";		            
		public const string Property_WorkOrder = "WorkOrder";		            
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
		private String _ReferenceNo;	            
		private String _AddedBy;	            
		private Nullable<DateTime> _AddedDate;	            
		private Nullable<Int32> _PaymentInfoId;	            
		private String _Desccription;	            
		private Nullable<Int32> _WorkOrder;	            
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
		public String Desccription
		{	
			get{ return _Desccription; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Desccription, value, _Desccription);
				if (PropertyChanging(args))
				{
					_Desccription = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> WorkOrder
		{	
			get{ return _WorkOrder; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_WorkOrder, value, _WorkOrder);
				if (PropertyChanging(args))
				{
					_WorkOrder = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  PaymentRevenueBase Clone()
		{
			PaymentRevenueBase newObj = new  PaymentRevenueBase();
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
			newObj.ReferenceNo = this.ReferenceNo;						
			newObj.AddedBy = this.AddedBy;						
			newObj.AddedDate = this.AddedDate;						
			newObj.PaymentInfoId = this.PaymentInfoId;						
			newObj.Desccription = this.Desccription;						
			newObj.WorkOrder = this.WorkOrder;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(PaymentRevenueBase.Property_Id, Id);				
			info.AddValue(PaymentRevenueBase.Property_CompanyId, CompanyId);				
			info.AddValue(PaymentRevenueBase.Property_CustomerId, CustomerId);				
			info.AddValue(PaymentRevenueBase.Property_Type, Type);				
			info.AddValue(PaymentRevenueBase.Property_Amount, Amount);				
			info.AddValue(PaymentRevenueBase.Property_Status, Status);				
			info.AddValue(PaymentRevenueBase.Property_TransacationDate, TransacationDate);				
			info.AddValue(PaymentRevenueBase.Property_CardTransactionId, CardTransactionId);				
			info.AddValue(PaymentRevenueBase.Property_PaymentMethod, PaymentMethod);				
			info.AddValue(PaymentRevenueBase.Property_ReferenceNo, ReferenceNo);				
			info.AddValue(PaymentRevenueBase.Property_AddedBy, AddedBy);				
			info.AddValue(PaymentRevenueBase.Property_AddedDate, AddedDate);				
			info.AddValue(PaymentRevenueBase.Property_PaymentInfoId, PaymentInfoId);				
			info.AddValue(PaymentRevenueBase.Property_Desccription, Desccription);				
			info.AddValue(PaymentRevenueBase.Property_WorkOrder, WorkOrder);				
		}
		#endregion

		
	}
}
