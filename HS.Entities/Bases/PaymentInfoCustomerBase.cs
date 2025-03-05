using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "PaymentInfoCustomerBase", Namespace = "http://www.piistech.com//entities")]
	public class PaymentInfoCustomerBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			CustomerId = 2,
			PaymentInfoId = 3,
			Type = 4,
			Payfor = 5,
			IsPaid = 6,
			PaymentDate = 7,
			ForMonths = 8,
			InvoiceId = 9,
			Comment = 10
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_PaymentInfoId = "PaymentInfoId";		            
		public const string Property_Type = "Type";		            
		public const string Property_Payfor = "Payfor";		            
		public const string Property_IsPaid = "IsPaid";		            
		public const string Property_PaymentDate = "PaymentDate";		            
		public const string Property_ForMonths = "ForMonths";		            
		public const string Property_InvoiceId = "InvoiceId";		            
		public const string Property_Comment = "Comment";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _CustomerId;	            
		private Int32 _PaymentInfoId;	            
		private String _Type;	            
		private String _Payfor;	            
		private Nullable<Boolean> _IsPaid;	            
		private Nullable<DateTime> _PaymentDate;	            
		private Nullable<Int32> _ForMonths;	            
		private String _InvoiceId;	            
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
		public Int32 PaymentInfoId
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
		public String Payfor
		{	
			get{ return _Payfor; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Payfor, value, _Payfor);
				if (PropertyChanging(args))
				{
					_Payfor = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsPaid
		{	
			get{ return _IsPaid; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsPaid, value, _IsPaid);
				if (PropertyChanging(args))
				{
					_IsPaid = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> PaymentDate
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
		public Nullable<Int32> ForMonths
		{	
			get{ return _ForMonths; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ForMonths, value, _ForMonths);
				if (PropertyChanging(args))
				{
					_ForMonths = value;
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
		public  PaymentInfoCustomerBase Clone()
		{
			PaymentInfoCustomerBase newObj = new  PaymentInfoCustomerBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.PaymentInfoId = this.PaymentInfoId;						
			newObj.Type = this.Type;						
			newObj.Payfor = this.Payfor;						
			newObj.IsPaid = this.IsPaid;						
			newObj.PaymentDate = this.PaymentDate;						
			newObj.ForMonths = this.ForMonths;						
			newObj.InvoiceId = this.InvoiceId;						
			newObj.Comment = this.Comment;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(PaymentInfoCustomerBase.Property_Id, Id);				
			info.AddValue(PaymentInfoCustomerBase.Property_CompanyId, CompanyId);				
			info.AddValue(PaymentInfoCustomerBase.Property_CustomerId, CustomerId);				
			info.AddValue(PaymentInfoCustomerBase.Property_PaymentInfoId, PaymentInfoId);				
			info.AddValue(PaymentInfoCustomerBase.Property_Type, Type);				
			info.AddValue(PaymentInfoCustomerBase.Property_Payfor, Payfor);				
			info.AddValue(PaymentInfoCustomerBase.Property_IsPaid, IsPaid);				
			info.AddValue(PaymentInfoCustomerBase.Property_PaymentDate, PaymentDate);				
			info.AddValue(PaymentInfoCustomerBase.Property_ForMonths, ForMonths);				
			info.AddValue(PaymentInfoCustomerBase.Property_InvoiceId, InvoiceId);				
			info.AddValue(PaymentInfoCustomerBase.Property_Comment, Comment);				
		}
		#endregion

		
	}
}
