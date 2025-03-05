using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "NotificationBase", Namespace = "http://www.piistech.com//entities")]
	public class NotificationBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			NotificationId = 2,
			Who = 3,
			What = 4,
			CreatedDate = 5,
			Type = 6,
			NotificationUrl = 7
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_NotificationId = "NotificationId";		            
		public const string Property_Who = "Who";		            
		public const string Property_What = "What";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_Type = "Type";		            
		public const string Property_NotificationUrl = "NotificationUrl";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _NotificationId;	            
		private Guid _Who;	            
		private String _What;	            
		private DateTime _CreatedDate;	            
		private String _Type;	            
		private String _NotificationUrl;	            
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
		public Guid Who
		{	
			get{ return _Who; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Who, value, _Who);
				if (PropertyChanging(args))
				{
					_Who = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String What
		{	
			get{ return _What; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_What, value, _What);
				if (PropertyChanging(args))
				{
					_What = value;
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
		public String NotificationUrl
		{	
			get{ return _NotificationUrl; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_NotificationUrl, value, _NotificationUrl);
				if (PropertyChanging(args))
				{
					_NotificationUrl = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  NotificationBase Clone()
		{
			NotificationBase newObj = new  NotificationBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.NotificationId = this.NotificationId;						
			newObj.Who = this.Who;						
			newObj.What = this.What;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.Type = this.Type;						
			newObj.NotificationUrl = this.NotificationUrl;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(NotificationBase.Property_Id, Id);				
			info.AddValue(NotificationBase.Property_CompanyId, CompanyId);				
			info.AddValue(NotificationBase.Property_NotificationId, NotificationId);				
			info.AddValue(NotificationBase.Property_Who, Who);				
			info.AddValue(NotificationBase.Property_What, What);				
			info.AddValue(NotificationBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(NotificationBase.Property_Type, Type);				
			info.AddValue(NotificationBase.Property_NotificationUrl, NotificationUrl);				
		}
		#endregion

		
	}
}
