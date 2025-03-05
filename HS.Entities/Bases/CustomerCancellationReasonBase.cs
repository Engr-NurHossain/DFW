using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomerCancellationReasonBase", Namespace = "http://www.hims-tech.com//entities")]
	public class CustomerCancellationReasonBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CustomerCancellationReasonId = 1,
			CompanyId = 2,
			CustomerId = 3,
			CancellationReason = 4
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CustomerCancellationReasonId = "CustomerCancellationReasonId";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_CancellationReason = "CancellationReason";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CustomerCancellationReasonId;	            
		private Guid _CompanyId;	            
		private Guid _CustomerId;	            
		private String _CancellationReason;	            
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
		public Guid CustomerCancellationReasonId
		{	
			get{ return _CustomerCancellationReasonId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CustomerCancellationReasonId, value, _CustomerCancellationReasonId);
				if (PropertyChanging(args))
				{
					_CustomerCancellationReasonId = value;
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
		public String CancellationReason
		{	
			get{ return _CancellationReason; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CancellationReason, value, _CancellationReason);
				if (PropertyChanging(args))
				{
					_CancellationReason = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  CustomerCancellationReasonBase Clone()
		{
			CustomerCancellationReasonBase newObj = new  CustomerCancellationReasonBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CustomerCancellationReasonId = this.CustomerCancellationReasonId;						
			newObj.CompanyId = this.CompanyId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.CancellationReason = this.CancellationReason;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomerCancellationReasonBase.Property_Id, Id);				
			info.AddValue(CustomerCancellationReasonBase.Property_CustomerCancellationReasonId, CustomerCancellationReasonId);				
			info.AddValue(CustomerCancellationReasonBase.Property_CompanyId, CompanyId);				
			info.AddValue(CustomerCancellationReasonBase.Property_CustomerId, CustomerId);				
			info.AddValue(CustomerCancellationReasonBase.Property_CancellationReason, CancellationReason);				
		}
		#endregion

		
	}
}
