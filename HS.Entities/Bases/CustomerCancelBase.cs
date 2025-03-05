using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomerCancelBase", Namespace = "http://www.piistech.com//entities")]
	public class CustomerCancelBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			CustomerId = 2,
			EmployeeId = 3,
			CancelDatet = 4,
			CancelReason = 5,
			IsActivated = 6
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_EmployeeId = "EmployeeId";		            
		public const string Property_CancelDatet = "CancelDatet";		            
		public const string Property_CancelReason = "CancelReason";		            
		public const string Property_IsActivated = "IsActivated";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _CustomerId;	            
		private Guid _EmployeeId;	            
		private Nullable<DateTime> _CancelDatet;	            
		private String _CancelReason;	            
		private Nullable<Boolean> _IsActivated;	            
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
		public Guid EmployeeId
		{	
			get{ return _EmployeeId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EmployeeId, value, _EmployeeId);
				if (PropertyChanging(args))
				{
					_EmployeeId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> CancelDatet
		{	
			get{ return _CancelDatet; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CancelDatet, value, _CancelDatet);
				if (PropertyChanging(args))
				{
					_CancelDatet = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CancelReason
		{	
			get{ return _CancelReason; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CancelReason, value, _CancelReason);
				if (PropertyChanging(args))
				{
					_CancelReason = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsActivated
		{	
			get{ return _IsActivated; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsActivated, value, _IsActivated);
				if (PropertyChanging(args))
				{
					_IsActivated = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  CustomerCancelBase Clone()
		{
			CustomerCancelBase newObj = new  CustomerCancelBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.EmployeeId = this.EmployeeId;						
			newObj.CancelDatet = this.CancelDatet;						
			newObj.CancelReason = this.CancelReason;						
			newObj.IsActivated = this.IsActivated;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomerCancelBase.Property_Id, Id);				
			info.AddValue(CustomerCancelBase.Property_CompanyId, CompanyId);				
			info.AddValue(CustomerCancelBase.Property_CustomerId, CustomerId);				
			info.AddValue(CustomerCancelBase.Property_EmployeeId, EmployeeId);				
			info.AddValue(CustomerCancelBase.Property_CancelDatet, CancelDatet);				
			info.AddValue(CustomerCancelBase.Property_CancelReason, CancelReason);				
			info.AddValue(CustomerCancelBase.Property_IsActivated, IsActivated);				
		}
		#endregion

		
	}
}
