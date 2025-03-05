using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "TicketPaymentBase", Namespace = "http://www.piistech.com//entities")]
	public class TicketPaymentBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CustomerId = 1,
			TicketId = 2,
			IsPaid = 3,
			PaymentMethod = 4,
			ConfirmationNo = 5,
			CreatedDate = 6,
			CreatedBy = 7
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_TicketId = "TicketId";		            
		public const string Property_IsPaid = "IsPaid";		            
		public const string Property_PaymentMethod = "PaymentMethod";		            
		public const string Property_ConfirmationNo = "ConfirmationNo";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CustomerId;	            
		private Guid _TicketId;	            
		private Nullable<Boolean> _IsPaid;	            
		private String _PaymentMethod;	            
		private String _ConfirmationNo;	            
		private DateTime _CreatedDate;	            
		private Guid _CreatedBy;	            
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
		public Guid TicketId
		{	
			get{ return _TicketId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TicketId, value, _TicketId);
				if (PropertyChanging(args))
				{
					_TicketId = value;
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
		public String ConfirmationNo
		{	
			get{ return _ConfirmationNo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ConfirmationNo, value, _ConfirmationNo);
				if (PropertyChanging(args))
				{
					_ConfirmationNo = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  TicketPaymentBase Clone()
		{
			TicketPaymentBase newObj = new  TicketPaymentBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CustomerId = this.CustomerId;						
			newObj.TicketId = this.TicketId;						
			newObj.IsPaid = this.IsPaid;						
			newObj.PaymentMethod = this.PaymentMethod;						
			newObj.ConfirmationNo = this.ConfirmationNo;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(TicketPaymentBase.Property_Id, Id);				
			info.AddValue(TicketPaymentBase.Property_CustomerId, CustomerId);				
			info.AddValue(TicketPaymentBase.Property_TicketId, TicketId);				
			info.AddValue(TicketPaymentBase.Property_IsPaid, IsPaid);				
			info.AddValue(TicketPaymentBase.Property_PaymentMethod, PaymentMethod);				
			info.AddValue(TicketPaymentBase.Property_ConfirmationNo, ConfirmationNo);				
			info.AddValue(TicketPaymentBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(TicketPaymentBase.Property_CreatedBy, CreatedBy);				
		}
		#endregion

		
	}
}
