using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "RMRBillingMismatchBase", Namespace = "http://www.hims-tech.com//entities")]
	public class RMRBillingMismatchBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CustomerId = 1,
			TransactionId = 2,
			InvoiceId = 3,
			BillingAmount = 4,
			ChargedAmountByGateway = 5,
			IsResolved = 6,
			ResolvedBy = 7,
			ResolvedDate = 8,
			CreatedDate = 9
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_TransactionId = "TransactionId";		            
		public const string Property_InvoiceId = "InvoiceId";		            
		public const string Property_BillingAmount = "BillingAmount";		            
		public const string Property_ChargedAmountByGateway = "ChargedAmountByGateway";		            
		public const string Property_IsResolved = "IsResolved";		            
		public const string Property_ResolvedBy = "ResolvedBy";		            
		public const string Property_ResolvedDate = "ResolvedDate";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CustomerId;	            
		private String _TransactionId;	            
		private String _InvoiceId;	            
		private Double _BillingAmount;	            
		private Double _ChargedAmountByGateway;	            
		private Boolean _IsResolved;	            
		private Guid _ResolvedBy;	            
		private Nullable<DateTime> _ResolvedDate;	            
		private Nullable<DateTime> _CreatedDate;	            
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
		public String TransactionId
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
		public Double BillingAmount
		{	
			get{ return _BillingAmount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BillingAmount, value, _BillingAmount);
				if (PropertyChanging(args))
				{
					_BillingAmount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Double ChargedAmountByGateway
		{	
			get{ return _ChargedAmountByGateway; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ChargedAmountByGateway, value, _ChargedAmountByGateway);
				if (PropertyChanging(args))
				{
					_ChargedAmountByGateway = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean IsResolved
		{	
			get{ return _IsResolved; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsResolved, value, _IsResolved);
				if (PropertyChanging(args))
				{
					_IsResolved = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid ResolvedBy
		{	
			get{ return _ResolvedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ResolvedBy, value, _ResolvedBy);
				if (PropertyChanging(args))
				{
					_ResolvedBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> ResolvedDate
		{	
			get{ return _ResolvedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ResolvedDate, value, _ResolvedDate);
				if (PropertyChanging(args))
				{
					_ResolvedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> CreatedDate
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

		#endregion
		
		#region Cloning Base Objects
		public  RMRBillingMismatchBase Clone()
		{
			RMRBillingMismatchBase newObj = new  RMRBillingMismatchBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CustomerId = this.CustomerId;						
			newObj.TransactionId = this.TransactionId;						
			newObj.InvoiceId = this.InvoiceId;						
			newObj.BillingAmount = this.BillingAmount;						
			newObj.ChargedAmountByGateway = this.ChargedAmountByGateway;						
			newObj.IsResolved = this.IsResolved;						
			newObj.ResolvedBy = this.ResolvedBy;						
			newObj.ResolvedDate = this.ResolvedDate;						
			newObj.CreatedDate = this.CreatedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(RMRBillingMismatchBase.Property_Id, Id);				
			info.AddValue(RMRBillingMismatchBase.Property_CustomerId, CustomerId);				
			info.AddValue(RMRBillingMismatchBase.Property_TransactionId, TransactionId);				
			info.AddValue(RMRBillingMismatchBase.Property_InvoiceId, InvoiceId);				
			info.AddValue(RMRBillingMismatchBase.Property_BillingAmount, BillingAmount);				
			info.AddValue(RMRBillingMismatchBase.Property_ChargedAmountByGateway, ChargedAmountByGateway);				
			info.AddValue(RMRBillingMismatchBase.Property_IsResolved, IsResolved);				
			info.AddValue(RMRBillingMismatchBase.Property_ResolvedBy, ResolvedBy);				
			info.AddValue(RMRBillingMismatchBase.Property_ResolvedDate, ResolvedDate);				
			info.AddValue(RMRBillingMismatchBase.Property_CreatedDate, CreatedDate);				
		}
		#endregion

		
	}
}
