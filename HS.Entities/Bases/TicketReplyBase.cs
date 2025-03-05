using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "TicketReplyBase", Namespace = "http://www.piistech.com//entities")]
	public class TicketReplyBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			TicketId = 1,
			UserId = 2,
			RepliedDate = 3,
			Message = 4,
			IsPrivate = 5,
			ReplyType = 6,
			LatLng = 7,
			IsOverview = 8
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_TicketId = "TicketId";		            
		public const string Property_UserId = "UserId";		            
		public const string Property_RepliedDate = "RepliedDate";		            
		public const string Property_Message = "Message";		            
		public const string Property_IsPrivate = "IsPrivate";		            
		public const string Property_ReplyType = "ReplyType";		            
		public const string Property_LatLng = "LatLng";		            
		public const string Property_IsOverview = "IsOverview";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _TicketId;	            
		private Guid _UserId;	            
		private DateTime _RepliedDate;	            
		private String _Message;	            
		private Nullable<Boolean> _IsPrivate;	            
		private String _ReplyType;	            
		private String _LatLng;	            
		private Nullable<Boolean> _IsOverview;	            
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
		public Guid TicketId
		{	
			get{ return _TicketId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TicketId, value, _TicketId);
				if (PropertyChanging(args))
				{
					_TicketId = value;
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
		public DateTime RepliedDate
		{	
			get{ return _RepliedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RepliedDate, value, _RepliedDate);
				if (PropertyChanging(args))
				{
					_RepliedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Message
		{	
			get{ return _Message; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Message, value, _Message);
				if (PropertyChanging(args))
				{
					_Message = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsPrivate
		{	
			get{ return _IsPrivate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsPrivate, value, _IsPrivate);
				if (PropertyChanging(args))
				{
					_IsPrivate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ReplyType
		{	
			get{ return _ReplyType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReplyType, value, _ReplyType);
				if (PropertyChanging(args))
				{
					_ReplyType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String LatLng
		{	
			get{ return _LatLng; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LatLng, value, _LatLng);
				if (PropertyChanging(args))
				{
					_LatLng = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsOverview
		{	
			get{ return _IsOverview; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsOverview, value, _IsOverview);
				if (PropertyChanging(args))
				{
					_IsOverview = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  TicketReplyBase Clone()
		{
			TicketReplyBase newObj = new  TicketReplyBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.TicketId = this.TicketId;						
			newObj.UserId = this.UserId;						
			newObj.RepliedDate = this.RepliedDate;						
			newObj.Message = this.Message;						
			newObj.IsPrivate = this.IsPrivate;						
			newObj.ReplyType = this.ReplyType;						
			newObj.LatLng = this.LatLng;						
			newObj.IsOverview = this.IsOverview;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(TicketReplyBase.Property_Id, Id);				
			info.AddValue(TicketReplyBase.Property_TicketId, TicketId);				
			info.AddValue(TicketReplyBase.Property_UserId, UserId);				
			info.AddValue(TicketReplyBase.Property_RepliedDate, RepliedDate);				
			info.AddValue(TicketReplyBase.Property_Message, Message);				
			info.AddValue(TicketReplyBase.Property_IsPrivate, IsPrivate);				
			info.AddValue(TicketReplyBase.Property_ReplyType, ReplyType);				
			info.AddValue(TicketReplyBase.Property_LatLng, LatLng);				
			info.AddValue(TicketReplyBase.Property_IsOverview, IsOverview);				
		}
		#endregion

		
	}
}
