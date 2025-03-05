using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "UserActivityCustomerBase", Namespace = "http://www.hims-tech.com//entities")]
	public class UserActivityCustomerBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CustomerId = 1,
			ActivityId = 2,
			RefId = 3
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_ActivityId = "ActivityId";		            
		public const string Property_RefId = "RefId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CustomerId;	            
		private Guid _ActivityId;	            
		private String _RefId;	            
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
		public Guid ActivityId
		{	
			get{ return _ActivityId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ActivityId, value, _ActivityId);
				if (PropertyChanging(args))
				{
					_ActivityId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String RefId
		{	
			get{ return _RefId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RefId, value, _RefId);
				if (PropertyChanging(args))
				{
					_RefId = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  UserActivityCustomerBase Clone()
		{
			UserActivityCustomerBase newObj = new  UserActivityCustomerBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CustomerId = this.CustomerId;						
			newObj.ActivityId = this.ActivityId;						
			newObj.RefId = this.RefId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(UserActivityCustomerBase.Property_Id, Id);				
			info.AddValue(UserActivityCustomerBase.Property_CustomerId, CustomerId);				
			info.AddValue(UserActivityCustomerBase.Property_ActivityId, ActivityId);				
			info.AddValue(UserActivityCustomerBase.Property_RefId, RefId);				
		}
		#endregion

		
	}
}
