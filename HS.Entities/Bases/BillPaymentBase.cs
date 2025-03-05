using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "BillPaymentBase", Namespace = "http://www.piistech.com//entities")]
	public class BillPaymentBase : BaseBusinessEntity
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
			PaymentMethod = 7,
			ReferenceNo = 8,
			AddedBy = 9,
			AddedDate = 10,
			PaymentInfoId = 11
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
		public const string Property_PaymentMethod = "PaymentMethod";		            
		public const string Property_ReferenceNo = "ReferenceNo";		            
		public const string Property_AddedBy = "AddedBy";		            
		public const string Property_AddedDate = "AddedDate";		            
		public const string Property_PaymentInfoId = "PaymentInfoId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _CustomerId;	            
		private String _Type;	            
		private Double _Amount;	            
		private String _Status;	            
		private DateTime _TransacationDate;	            
		private String _PaymentMethod;	            
		private String _ReferenceNo;	            
		private String _AddedBy;	            
		private Nullable<DateTime> _AddedDate;	            
		private Nullable<Int32> _PaymentInfoId;	            
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

		#endregion
		
		#region Cloning Base Objects
		public  BillPaymentBase Clone()
		{
			BillPaymentBase newObj = new  BillPaymentBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.Type = this.Type;						
			newObj.Amount = this.Amount;						
			newObj.Status = this.Status;						
			newObj.TransacationDate = this.TransacationDate;						
			newObj.PaymentMethod = this.PaymentMethod;						
			newObj.ReferenceNo = this.ReferenceNo;						
			newObj.AddedBy = this.AddedBy;						
			newObj.AddedDate = this.AddedDate;						
			newObj.PaymentInfoId = this.PaymentInfoId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(BillPaymentBase.Property_Id, Id);				
			info.AddValue(BillPaymentBase.Property_CompanyId, CompanyId);				
			info.AddValue(BillPaymentBase.Property_CustomerId, CustomerId);				
			info.AddValue(BillPaymentBase.Property_Type, Type);				
			info.AddValue(BillPaymentBase.Property_Amount, Amount);				
			info.AddValue(BillPaymentBase.Property_Status, Status);				
			info.AddValue(BillPaymentBase.Property_TransacationDate, TransacationDate);				
			info.AddValue(BillPaymentBase.Property_PaymentMethod, PaymentMethod);				
			info.AddValue(BillPaymentBase.Property_ReferenceNo, ReferenceNo);				
			info.AddValue(BillPaymentBase.Property_AddedBy, AddedBy);				
			info.AddValue(BillPaymentBase.Property_AddedDate, AddedDate);				
			info.AddValue(BillPaymentBase.Property_PaymentInfoId, PaymentInfoId);				
		}
		#endregion

		
	}
}
