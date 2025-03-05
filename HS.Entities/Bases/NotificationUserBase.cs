using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "NotificationUserBase", Namespace = "http://www.piistech.com//entities")]
	public class NotificationUserBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			NotificationId = 1,
			NotificationPerson = 2,
			IsRead = 3
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_NotificationId = "NotificationId";		            
		public const string Property_NotificationPerson = "NotificationPerson";		            
		public const string Property_IsRead = "IsRead";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _NotificationId;	            
		private Guid _NotificationPerson;	            
		private Boolean _IsRead;	            
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
		public Guid NotificationId
		{	
			get{ return _NotificationId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_NotificationId, value, _NotificationId);
				if (PropertyChanging(args))
				{
					_NotificationId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid NotificationPerson
		{	
			get{ return _NotificationPerson; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_NotificationPerson, value, _NotificationPerson);
				if (PropertyChanging(args))
				{
					_NotificationPerson = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean IsRead
		{	
			get{ return _IsRead; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsRead, value, _IsRead);
				if (PropertyChanging(args))
				{
					_IsRead = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  NotificationUserBase Clone()
		{
			NotificationUserBase newObj = new  NotificationUserBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.NotificationId = this.NotificationId;						
			newObj.NotificationPerson = this.NotificationPerson;						
			newObj.IsRead = this.IsRead;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(NotificationUserBase.Property_Id, Id);				
			info.AddValue(NotificationUserBase.Property_NotificationId, NotificationId);				
			info.AddValue(NotificationUserBase.Property_NotificationPerson, NotificationPerson);				
			info.AddValue(NotificationUserBase.Property_IsRead, IsRead);				
		}
		#endregion

		
	}
}
