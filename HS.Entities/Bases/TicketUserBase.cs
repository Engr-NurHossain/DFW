using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "TicketUserBase", Namespace = "http://www.piistech.com//entities")]
	public class TicketUserBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			TiketId = 1,
			UserId = 2,
			IsPrimary = 3,
			AddedDate = 4,
			AddedBy = 5,
			NotificationOnly = 6,
			IsReschedulePay = 7
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_TiketId = "TiketId";		            
		public const string Property_UserId = "UserId";		            
		public const string Property_IsPrimary = "IsPrimary";		            
		public const string Property_AddedDate = "AddedDate";		            
		public const string Property_AddedBy = "AddedBy";		            
		public const string Property_NotificationOnly = "NotificationOnly";		            
		public const string Property_IsReschedulePay = "IsReschedulePay";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _TiketId;	            
		private Guid _UserId;	            
		private Boolean _IsPrimary;	            
		private DateTime _AddedDate;	            
		private Guid _AddedBy;	            
		private Nullable<Boolean> _NotificationOnly;	            
		private Nullable<Boolean> _IsReschedulePay;	            
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
		public Guid TiketId
		{	
			get{ return _TiketId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TiketId, value, _TiketId);
				if (PropertyChanging(args))
				{
					_TiketId = value;
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
		public Boolean IsPrimary
		{	
			get{ return _IsPrimary; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsPrimary, value, _IsPrimary);
				if (PropertyChanging(args))
				{
					_IsPrimary = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime AddedDate
		{	
			get{ return _AddedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AddedDate, value, _AddedDate);
				if (PropertyChanging(args))
				{
					_AddedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid AddedBy
		{	
			get{ return _AddedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AddedBy, value, _AddedBy);
				if (PropertyChanging(args))
				{
					_AddedBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> NotificationOnly
		{	
			get{ return _NotificationOnly; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_NotificationOnly, value, _NotificationOnly);
				if (PropertyChanging(args))
				{
					_NotificationOnly = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsReschedulePay
		{	
			get{ return _IsReschedulePay; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsReschedulePay, value, _IsReschedulePay);
				if (PropertyChanging(args))
				{
					_IsReschedulePay = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  TicketUserBase Clone()
		{
			TicketUserBase newObj = new  TicketUserBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.TiketId = this.TiketId;						
			newObj.UserId = this.UserId;						
			newObj.IsPrimary = this.IsPrimary;						
			newObj.AddedDate = this.AddedDate;						
			newObj.AddedBy = this.AddedBy;						
			newObj.NotificationOnly = this.NotificationOnly;						
			newObj.IsReschedulePay = this.IsReschedulePay;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(TicketUserBase.Property_Id, Id);				
			info.AddValue(TicketUserBase.Property_TiketId, TiketId);				
			info.AddValue(TicketUserBase.Property_UserId, UserId);				
			info.AddValue(TicketUserBase.Property_IsPrimary, IsPrimary);				
			info.AddValue(TicketUserBase.Property_AddedDate, AddedDate);				
			info.AddValue(TicketUserBase.Property_AddedBy, AddedBy);				
			info.AddValue(TicketUserBase.Property_NotificationOnly, NotificationOnly);				
			info.AddValue(TicketUserBase.Property_IsReschedulePay, IsReschedulePay);				
		}
		#endregion

		
	}
}
