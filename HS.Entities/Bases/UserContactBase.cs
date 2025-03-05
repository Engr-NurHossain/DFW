using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "UserContactBase", Namespace = "http://www.piistech.com//entities")]
	public class UserContactBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			UserId = 1,
			ContactId = 2,
			UserType = 3
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_UserId = "UserId";		            
		public const string Property_ContactId = "ContactId";		            
		public const string Property_UserType = "UserType";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _UserId;	            
		private Guid _ContactId;	            
		private String _UserType;	            
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
		public Guid UserId
		{	
			get{ return _UserId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UserId, value, _UserId);
				if (PropertyChanging(args))
				{
					_UserId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid ContactId
		{	
			get{ return _ContactId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ContactId, value, _ContactId);
				if (PropertyChanging(args))
				{
					_ContactId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String UserType
		{	
			get{ return _UserType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UserType, value, _UserType);
				if (PropertyChanging(args))
				{
					_UserType = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  UserContactBase Clone()
		{
			UserContactBase newObj = new  UserContactBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.UserId = this.UserId;						
			newObj.ContactId = this.ContactId;						
			newObj.UserType = this.UserType;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(UserContactBase.Property_Id, Id);				
			info.AddValue(UserContactBase.Property_UserId, UserId);				
			info.AddValue(UserContactBase.Property_ContactId, ContactId);				
			info.AddValue(UserContactBase.Property_UserType, UserType);				
		}
		#endregion

		
	}
}
