using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "PaymentBase", Namespace = "http://www.piistech.com//entities")]
	public class PaymentBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			InvoiceId = 2,
			PaymentMethodId = 3,
			Amount = 4,
			PaymentDate = 5,
			PaymentBy = 6,
			CreditCardId = 7,
			ReferenceNo = 8
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_InvoiceId = "InvoiceId";		            
		public const string Property_PaymentMethodId = "PaymentMethodId";		            
		public const string Property_Amount = "Amount";		            
		public const string Property_PaymentDate = "PaymentDate";		            
		public const string Property_PaymentBy = "PaymentBy";		            
		public const string Property_CreditCardId = "CreditCardId";		            
		public const string Property_ReferenceNo = "ReferenceNo";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _InvoiceId;	            
		private Int32 _PaymentMethodId;	            
		private Double _Amount;	            
		private DateTime _PaymentDate;	            
		private String _PaymentBy;	            
		private Nullable<Int32> _CreditCardId;	            
		private String _ReferenceNo;	            
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
		public Guid InvoiceId
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
		public Int32 PaymentMethodId
		{	
			get{ return _PaymentMethodId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PaymentMethodId, value, _PaymentMethodId);
				if (PropertyChanging(args))
				{
					_PaymentMethodId = value;
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
		public DateTime PaymentDate
		{	
			get{ return _PaymentDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PaymentDate, value, _PaymentDate);
				if (PropertyChanging(args))
				{
					_PaymentDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PaymentBy
		{	
			get{ return _PaymentBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PaymentBy, value, _PaymentBy);
				if (PropertyChanging(args))
				{
					_PaymentBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> CreditCardId
		{	
			get{ return _CreditCardId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreditCardId, value, _CreditCardId);
				if (PropertyChanging(args))
				{
					_CreditCardId = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  PaymentBase Clone()
		{
			PaymentBase newObj = new  PaymentBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.InvoiceId = this.InvoiceId;						
			newObj.PaymentMethodId = this.PaymentMethodId;						
			newObj.Amount = this.Amount;						
			newObj.PaymentDate = this.PaymentDate;						
			newObj.PaymentBy = this.PaymentBy;						
			newObj.CreditCardId = this.CreditCardId;						
			newObj.ReferenceNo = this.ReferenceNo;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(PaymentBase.Property_Id, Id);				
			info.AddValue(PaymentBase.Property_CompanyId, CompanyId);				
			info.AddValue(PaymentBase.Property_InvoiceId, InvoiceId);				
			info.AddValue(PaymentBase.Property_PaymentMethodId, PaymentMethodId);				
			info.AddValue(PaymentBase.Property_Amount, Amount);				
			info.AddValue(PaymentBase.Property_PaymentDate, PaymentDate);				
			info.AddValue(PaymentBase.Property_PaymentBy, PaymentBy);				
			info.AddValue(PaymentBase.Property_CreditCardId, CreditCardId);				
			info.AddValue(PaymentBase.Property_ReferenceNo, ReferenceNo);				
		}
		#endregion

		
	}
}
