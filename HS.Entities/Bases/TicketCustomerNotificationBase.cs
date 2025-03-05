using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "TicketCustomerNotificationBase", Namespace = "http://www.piistech.com//entities")]
	public class TicketCustomerNotificationBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			TicketStatus = 1,
			Text = 2,
			Email = 3,
			CreatedBy = 4,
			CreatedDate = 5,
			LastUpdatedBy = 6,
			LastUpdatedDate = 7,
			TicketType = 8
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_TicketStatus = "TicketStatus";		            
		public const string Property_Text = "Text";		            
		public const string Property_Email = "Email";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_TicketType = "TicketType";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _TicketStatus;	            
		private String _Text;	            
		private String _Email;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
		private Guid _LastUpdatedBy;	            
		private Nullable<DateTime> _LastUpdatedDate;	            
		private String _TicketType;	            
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
		public String Text
		{	
			get{ return _Text; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Text, value, _Text);
				if (PropertyChanging(args))
				{
					_Text = value;
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
		public Guid LastUpdatedBy
		{	
			get{ return _LastUpdatedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastUpdatedBy, value, _LastUpdatedBy);
				if (PropertyChanging(args))
				{
					_LastUpdatedBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> LastUpdatedDate
		{	
			get{ return _LastUpdatedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastUpdatedDate, value, _LastUpdatedDate);
				if (PropertyChanging(args))
				{
					_LastUpdatedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TicketType
		{	
			get{ return _TicketType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TicketType, value, _TicketType);
				if (PropertyChanging(args))
				{
					_TicketType = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  TicketCustomerNotificationBase Clone()
		{
			TicketCustomerNotificationBase newObj = new  TicketCustomerNotificationBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.TicketStatus = this.TicketStatus;						
			newObj.Text = this.Text;						
			newObj.Email = this.Email;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.TicketType = this.TicketType;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(TicketCustomerNotificationBase.Property_Id, Id);				
			info.AddValue(TicketCustomerNotificationBase.Property_TicketStatus, TicketStatus);				
			info.AddValue(TicketCustomerNotificationBase.Property_Text, Text);				
			info.AddValue(TicketCustomerNotificationBase.Property_Email, Email);				
			info.AddValue(TicketCustomerNotificationBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(TicketCustomerNotificationBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(TicketCustomerNotificationBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(TicketCustomerNotificationBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(TicketCustomerNotificationBase.Property_TicketType, TicketType);				
		}
		#endregion

		
	}
}
