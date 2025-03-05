using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "TransactionHistoryBase", Namespace = "http://www.piistech.com//entities")]
	public class TransactionHistoryBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			TransactionId = 1,
			InvoiceId = 2,
			Amout = 3,
			Balance = 4,
			ReceivedBy = 5
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_TransactionId = "TransactionId";		            
		public const string Property_InvoiceId = "InvoiceId";		            
		public const string Property_Amout = "Amout";		            
		public const string Property_Balance = "Balance";		            
		public const string Property_ReceivedBy = "ReceivedBy";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Int32 _TransactionId;	            
		private Int32 _InvoiceId;	            
		private Double _Amout;	            
		private Double _Balance;	            
		private Guid _ReceivedBy;	            
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
		public Int32 TransactionId
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
		public Double Amout
		{	
			get{ return _Amout; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Amout, value, _Amout);
				if (PropertyChanging(args))
				{
					_Amout = value;
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

		[DataMember]
		public Guid ReceivedBy
		{	
			get{ return _ReceivedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReceivedBy, value, _ReceivedBy);
				if (PropertyChanging(args))
				{
					_ReceivedBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  TransactionHistoryBase Clone()
		{
			TransactionHistoryBase newObj = new  TransactionHistoryBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.TransactionId = this.TransactionId;						
			newObj.InvoiceId = this.InvoiceId;						
			newObj.Amout = this.Amout;						
			newObj.Balance = this.Balance;						
			newObj.ReceivedBy = this.ReceivedBy;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(TransactionHistoryBase.Property_Id, Id);				
			info.AddValue(TransactionHistoryBase.Property_TransactionId, TransactionId);				
			info.AddValue(TransactionHistoryBase.Property_InvoiceId, InvoiceId);				
			info.AddValue(TransactionHistoryBase.Property_Amout, Amout);				
			info.AddValue(TransactionHistoryBase.Property_Balance, Balance);				
			info.AddValue(TransactionHistoryBase.Property_ReceivedBy, ReceivedBy);				
		}
		#endregion

		
	}
}
