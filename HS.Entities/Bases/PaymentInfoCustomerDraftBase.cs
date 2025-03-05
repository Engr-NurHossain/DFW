using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "PaymentInfoCustomerDraftBase", Namespace = "http://www.piistech.com//entities")]
	public class PaymentInfoCustomerDraftBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			CustomerId = 2,
			PaymentInfoId = 3,
			Type = 4,
			Payfor = 5
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_PaymentInfoId = "PaymentInfoId";		            
		public const string Property_Type = "Type";		            
		public const string Property_Payfor = "Payfor";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _CustomerId;	            
		private Int32 _PaymentInfoId;	            
		private String _Type;	            
		private String _Payfor;	            
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

		#endregion
		
		#region Cloning Base Objects
		public  PaymentInfoCustomerDraftBase Clone()
		{
			PaymentInfoCustomerDraftBase newObj = new  PaymentInfoCustomerDraftBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.PaymentInfoId = this.PaymentInfoId;						
			newObj.Type = this.Type;						
			newObj.Payfor = this.Payfor;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(PaymentInfoCustomerDraftBase.Property_Id, Id);				
			info.AddValue(PaymentInfoCustomerDraftBase.Property_CompanyId, CompanyId);				
			info.AddValue(PaymentInfoCustomerDraftBase.Property_CustomerId, CustomerId);				
			info.AddValue(PaymentInfoCustomerDraftBase.Property_PaymentInfoId, PaymentInfoId);				
			info.AddValue(PaymentInfoCustomerDraftBase.Property_Type, Type);				
			info.AddValue(PaymentInfoCustomerDraftBase.Property_Payfor, Payfor);				
		}
		#endregion

		
	}
}
