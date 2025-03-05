using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "BillPaymentHistoryBase", Namespace = "http://www.piistech.com//entities")]
	public class BillPaymentHistoryBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			BillPaymentId = 1,
			InvoiceId = 2,
			Amount = 3,
			Balance = 4
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_BillPaymentId = "BillPaymentId";		            
		public const string Property_InvoiceId = "InvoiceId";		            
		public const string Property_Amount = "Amount";		            
		public const string Property_Balance = "Balance";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Int32 _BillPaymentId;	            
		private Int32 _InvoiceId;	            
		private Double _Amount;	            
		private Double _Balance;	            
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
		public Int32 BillPaymentId
		{	
			get{ return _BillPaymentId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BillPaymentId, value, _BillPaymentId);
				if (PropertyChanging(args))
				{
					_BillPaymentId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Int32 InvoiceId
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
		public Double Balance
		{	
			get{ return _Balance; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Balance, value, _Balance);
				if (PropertyChanging(args))
				{
					_Balance = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  BillPaymentHistoryBase Clone()
		{
			BillPaymentHistoryBase newObj = new  BillPaymentHistoryBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.BillPaymentId = this.BillPaymentId;						
			newObj.InvoiceId = this.InvoiceId;						
			newObj.Amount = this.Amount;						
			newObj.Balance = this.Balance;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(BillPaymentHistoryBase.Property_Id, Id);				
			info.AddValue(BillPaymentHistoryBase.Property_BillPaymentId, BillPaymentId);				
			info.AddValue(BillPaymentHistoryBase.Property_InvoiceId, InvoiceId);				
			info.AddValue(BillPaymentHistoryBase.Property_Amount, Amount);				
			info.AddValue(BillPaymentHistoryBase.Property_Balance, Balance);				
		}
		#endregion

		
	}
}
