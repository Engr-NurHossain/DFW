using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "TicketNotificationEmailBase", Namespace = "http://www.hims-tech.com//entities")]
	public class TicketNotificationEmailBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			TicketStatus = 1,
			Email = 2,
			CreatedBy = 3,
			CreatedDate = 4
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_TicketStatus = "TicketStatus";		            
		public const string Property_Email = "Email";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _TicketStatus;	            
		private String _Email;	            
		private Guid _CreatedBy;	            
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
		public String TicketStatus
		{	
			get{ return _TicketStatus; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TicketStatus, value, _TicketStatus);
				if (PropertyChanging(args))
				{
					_TicketStatus = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Email
		{	
			get{ return _Email; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Email, value, _Email);
				if (PropertyChanging(args))
				{
					_Email = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid CreatedBy
		{	
			get{ return _CreatedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreatedBy, value, _CreatedBy);
				if (PropertyChanging(args))
				{
					_CreatedBy = value;
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
		public  TicketNotificationEmailBase Clone()
		{
			TicketNotificationEmailBase newObj = new  TicketNotificationEmailBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.TicketStatus = this.TicketStatus;						
			newObj.Email = this.Email;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(TicketNotificationEmailBase.Property_Id, Id);				
			info.AddValue(TicketNotificationEmailBase.Property_TicketStatus, TicketStatus);				
			info.AddValue(TicketNotificationEmailBase.Property_Email, Email);				
			info.AddValue(TicketNotificationEmailBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(TicketNotificationEmailBase.Property_CreatedDate, CreatedDate);				
		}
		#endregion

		
	}
}
