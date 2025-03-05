using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "UserActivityBase", Namespace = "http://www.hims-tech.com//entities")]
	public class UserActivityBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			ActivityId = 1,
			PageUrl = 2,
			ReferrerUrl = 3,
			Action = 4,
			ActionDisplyText = 5,
			UserIp = 6,
			UserAgent = 7,
			UserName = 8,
			UserId = 9,
			StatsDate = 10,
			IsARB = 11
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_ActivityId = "ActivityId";		            
		public const string Property_PageUrl = "PageUrl";		            
		public const string Property_ReferrerUrl = "ReferrerUrl";		            
		public const string Property_Action = "Action";		            
		public const string Property_ActionDisplyText = "ActionDisplyText";		            
		public const string Property_UserIp = "UserIp";		            
		public const string Property_UserAgent = "UserAgent";		            
		public const string Property_UserName = "UserName";		            
		public const string Property_UserId = "UserId";		            
		public const string Property_StatsDate = "StatsDate";		            
		public const string Property_IsARB = "IsARB";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _ActivityId;	            
		private String _PageUrl;	            
		private String _ReferrerUrl;	            
		private String _Action;	            
		private String _ActionDisplyText;	            
		private String _UserIp;	            
		private String _UserAgent;	            
		private String _UserName;	            
		private Guid _UserId;	            
		private DateTime _StatsDate;	            
		private Nullable<Boolean> _IsARB;	            
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
		public String PageUrl
		{	
			get{ return _PageUrl; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PageUrl, value, _PageUrl);
				if (PropertyChanging(args))
				{
					_PageUrl = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ReferrerUrl
		{	
			get{ return _ReferrerUrl; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReferrerUrl, value, _ReferrerUrl);
				if (PropertyChanging(args))
				{
					_ReferrerUrl = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Action
		{	
			get{ return _Action; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Action, value, _Action);
				if (PropertyChanging(args))
				{
					_Action = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ActionDisplyText
		{	
			get{ return _ActionDisplyText; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ActionDisplyText, value, _ActionDisplyText);
				if (PropertyChanging(args))
				{
					_ActionDisplyText = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String UserIp
		{	
			get{ return _UserIp; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UserIp, value, _UserIp);
				if (PropertyChanging(args))
				{
					_UserIp = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String UserAgent
		{	
			get{ return _UserAgent; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UserAgent, value, _UserAgent);
				if (PropertyChanging(args))
				{
					_UserAgent = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String UserName
		{	
			get{ return _UserName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UserName, value, _UserName);
				if (PropertyChanging(args))
				{
					_UserName = value;
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
		public DateTime StatsDate
		{	
			get{ return _StatsDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_StatsDate, value, _StatsDate);
				if (PropertyChanging(args))
				{
					_StatsDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsARB
		{	
			get{ return _IsARB; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsARB, value, _IsARB);
				if (PropertyChanging(args))
				{
					_IsARB = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  UserActivityBase Clone()
		{
			UserActivityBase newObj = new  UserActivityBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.ActivityId = this.ActivityId;						
			newObj.PageUrl = this.PageUrl;						
			newObj.ReferrerUrl = this.ReferrerUrl;						
			newObj.Action = this.Action;						
			newObj.ActionDisplyText = this.ActionDisplyText;						
			newObj.UserIp = this.UserIp;						
			newObj.UserAgent = this.UserAgent;						
			newObj.UserName = this.UserName;						
			newObj.UserId = this.UserId;						
			newObj.StatsDate = this.StatsDate;						
			newObj.IsARB = this.IsARB;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(UserActivityBase.Property_Id, Id);				
			info.AddValue(UserActivityBase.Property_ActivityId, ActivityId);				
			info.AddValue(UserActivityBase.Property_PageUrl, PageUrl);				
			info.AddValue(UserActivityBase.Property_ReferrerUrl, ReferrerUrl);				
			info.AddValue(UserActivityBase.Property_Action, Action);				
			info.AddValue(UserActivityBase.Property_ActionDisplyText, ActionDisplyText);				
			info.AddValue(UserActivityBase.Property_UserIp, UserIp);				
			info.AddValue(UserActivityBase.Property_UserAgent, UserAgent);				
			info.AddValue(UserActivityBase.Property_UserName, UserName);				
			info.AddValue(UserActivityBase.Property_UserId, UserId);				
			info.AddValue(UserActivityBase.Property_StatsDate, StatsDate);				
			info.AddValue(UserActivityBase.Property_IsARB, IsARB);				
		}
		#endregion

		
	}
}
