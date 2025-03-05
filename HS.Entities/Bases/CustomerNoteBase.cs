using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomerNoteBase", Namespace = "http://www.hims-tech.com//entities")]
	public class CustomerNoteBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			Notes = 1,
			ReminderDate = 2,
			ReminderEndDate = 3,
			CustomerId = 4,
			CompanyId = 5,
			CreatedDate = 6,
			IsEmail = 7,
			IsText = 8,
			IsShedule = 9,
			IsFollowUp = 10,
			IsActive = 11,
			CreatedBy = 12,
			IsClose = 13,
			IsAllDay = 14,
			CreatedByUid = 15,
			IsPin = 16,
			NoteType = 17,
			IsOverview = 18,
			OrderBy = 19,
			ReferenceTicketId = 20,
			ThirdPartyId = 21,
			IsPrimaryNote = 22,
			TeamSettingId = 23
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_Notes = "Notes";		            
		public const string Property_ReminderDate = "ReminderDate";		            
		public const string Property_ReminderEndDate = "ReminderEndDate";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_IsEmail = "IsEmail";		            
		public const string Property_IsText = "IsText";		            
		public const string Property_IsShedule = "IsShedule";		            
		public const string Property_IsFollowUp = "IsFollowUp";		            
		public const string Property_IsActive = "IsActive";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_IsClose = "IsClose";		            
		public const string Property_IsAllDay = "IsAllDay";		            
		public const string Property_CreatedByUid = "CreatedByUid";		            
		public const string Property_IsPin = "IsPin";		            
		public const string Property_NoteType = "NoteType";		            
		public const string Property_IsOverview = "IsOverview";		            
		public const string Property_OrderBy = "OrderBy";		            
		public const string Property_ReferenceTicketId = "ReferenceTicketId";		            
		public const string Property_ThirdPartyId = "ThirdPartyId";		            
		public const string Property_IsPrimaryNote = "IsPrimaryNote";		            
		public const string Property_TeamSettingId = "TeamSettingId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _Notes;	            
		private Nullable<DateTime> _ReminderDate;	            
		private Nullable<DateTime> _ReminderEndDate;	            
		private Guid _CustomerId;	            
		private Guid _CompanyId;	            
		private DateTime _CreatedDate;	            
		private Nullable<Boolean> _IsEmail;	            
		private Nullable<Boolean> _IsText;	            
		private Nullable<Boolean> _IsShedule;	            
		private Nullable<Boolean> _IsFollowUp;	            
		private Nullable<Boolean> _IsActive;	            
		private String _CreatedBy;	            
		private Nullable<Boolean> _IsClose;	            
		private Nullable<Boolean> _IsAllDay;	            
		private Guid _CreatedByUid;	            
		private Nullable<Boolean> _IsPin;	            
		private String _NoteType;	            
		private Nullable<Boolean> _IsOverview;	            
		private Nullable<Int32> _OrderBy;	            
		private Nullable<Int32> _ReferenceTicketId;	            
		private Nullable<Int32> _ThirdPartyId;	            
		private Nullable<Boolean> _IsPrimaryNote;	            
		private Nullable<Int32> _TeamSettingId;	            
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
		public String Notes
		{	
			get{ return _Notes; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Notes, value, _Notes);
				if (PropertyChanging(args))
				{
					_Notes = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> ReminderDate
		{	
			get{ return _ReminderDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReminderDate, value, _ReminderDate);
				if (PropertyChanging(args))
				{
					_ReminderDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> ReminderEndDate
		{	
			get{ return _ReminderEndDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReminderEndDate, value, _ReminderEndDate);
				if (PropertyChanging(args))
				{
					_ReminderEndDate = value;
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
		public Nullable<Boolean> IsEmail
		{	
			get{ return _IsEmail; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsEmail, value, _IsEmail);
				if (PropertyChanging(args))
				{
					_IsEmail = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsText
		{	
			get{ return _IsText; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsText, value, _IsText);
				if (PropertyChanging(args))
				{
					_IsText = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsShedule
		{	
			get{ return _IsShedule; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsShedule, value, _IsShedule);
				if (PropertyChanging(args))
				{
					_IsShedule = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsFollowUp
		{	
			get{ return _IsFollowUp; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsFollowUp, value, _IsFollowUp);
				if (PropertyChanging(args))
				{
					_IsFollowUp = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsActive
		{	
			get{ return _IsActive; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsActive, value, _IsActive);
				if (PropertyChanging(args))
				{
					_IsActive = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CreatedBy
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
		public Nullable<Boolean> IsClose
		{	
			get{ return _IsClose; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsClose, value, _IsClose);
				if (PropertyChanging(args))
				{
					_IsClose = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsAllDay
		{	
			get{ return _IsAllDay; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsAllDay, value, _IsAllDay);
				if (PropertyChanging(args))
				{
					_IsAllDay = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid CreatedByUid
		{	
			get{ return _CreatedByUid; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreatedByUid, value, _CreatedByUid);
				if (PropertyChanging(args))
				{
					_CreatedByUid = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsPin
		{	
			get{ return _IsPin; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsPin, value, _IsPin);
				if (PropertyChanging(args))
				{
					_IsPin = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String NoteType
		{	
			get{ return _NoteType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_NoteType, value, _NoteType);
				if (PropertyChanging(args))
				{
					_NoteType = value;
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

		[DataMember]
		public Nullable<Int32> OrderBy
		{	
			get{ return _OrderBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OrderBy, value, _OrderBy);
				if (PropertyChanging(args))
				{
					_OrderBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> ReferenceTicketId
		{	
			get{ return _ReferenceTicketId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReferenceTicketId, value, _ReferenceTicketId);
				if (PropertyChanging(args))
				{
					_ReferenceTicketId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> ThirdPartyId
		{	
			get{ return _ThirdPartyId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ThirdPartyId, value, _ThirdPartyId);
				if (PropertyChanging(args))
				{
					_ThirdPartyId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsPrimaryNote
		{	
			get{ return _IsPrimaryNote; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsPrimaryNote, value, _IsPrimaryNote);
				if (PropertyChanging(args))
				{
					_IsPrimaryNote = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> TeamSettingId
		{	
			get{ return _TeamSettingId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TeamSettingId, value, _TeamSettingId);
				if (PropertyChanging(args))
				{
					_TeamSettingId = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  CustomerNoteBase Clone()
		{
			CustomerNoteBase newObj = new  CustomerNoteBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.Notes = this.Notes;						
			newObj.ReminderDate = this.ReminderDate;						
			newObj.ReminderEndDate = this.ReminderEndDate;						
			newObj.CustomerId = this.CustomerId;						
			newObj.CompanyId = this.CompanyId;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.IsEmail = this.IsEmail;						
			newObj.IsText = this.IsText;						
			newObj.IsShedule = this.IsShedule;						
			newObj.IsFollowUp = this.IsFollowUp;						
			newObj.IsActive = this.IsActive;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.IsClose = this.IsClose;						
			newObj.IsAllDay = this.IsAllDay;						
			newObj.CreatedByUid = this.CreatedByUid;						
			newObj.IsPin = this.IsPin;						
			newObj.NoteType = this.NoteType;						
			newObj.IsOverview = this.IsOverview;						
			newObj.OrderBy = this.OrderBy;						
			newObj.ReferenceTicketId = this.ReferenceTicketId;						
			newObj.ThirdPartyId = this.ThirdPartyId;						
			newObj.IsPrimaryNote = this.IsPrimaryNote;						
			newObj.TeamSettingId = this.TeamSettingId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomerNoteBase.Property_Id, Id);				
			info.AddValue(CustomerNoteBase.Property_Notes, Notes);				
			info.AddValue(CustomerNoteBase.Property_ReminderDate, ReminderDate);				
			info.AddValue(CustomerNoteBase.Property_ReminderEndDate, ReminderEndDate);				
			info.AddValue(CustomerNoteBase.Property_CustomerId, CustomerId);				
			info.AddValue(CustomerNoteBase.Property_CompanyId, CompanyId);				
			info.AddValue(CustomerNoteBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(CustomerNoteBase.Property_IsEmail, IsEmail);				
			info.AddValue(CustomerNoteBase.Property_IsText, IsText);				
			info.AddValue(CustomerNoteBase.Property_IsShedule, IsShedule);				
			info.AddValue(CustomerNoteBase.Property_IsFollowUp, IsFollowUp);				
			info.AddValue(CustomerNoteBase.Property_IsActive, IsActive);				
			info.AddValue(CustomerNoteBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(CustomerNoteBase.Property_IsClose, IsClose);				
			info.AddValue(CustomerNoteBase.Property_IsAllDay, IsAllDay);				
			info.AddValue(CustomerNoteBase.Property_CreatedByUid, CreatedByUid);				
			info.AddValue(CustomerNoteBase.Property_IsPin, IsPin);				
			info.AddValue(CustomerNoteBase.Property_NoteType, NoteType);				
			info.AddValue(CustomerNoteBase.Property_IsOverview, IsOverview);				
			info.AddValue(CustomerNoteBase.Property_OrderBy, OrderBy);				
			info.AddValue(CustomerNoteBase.Property_ReferenceTicketId, ReferenceTicketId);				
			info.AddValue(CustomerNoteBase.Property_ThirdPartyId, ThirdPartyId);				
			info.AddValue(CustomerNoteBase.Property_IsPrimaryNote, IsPrimaryNote);				
			info.AddValue(CustomerNoteBase.Property_TeamSettingId, TeamSettingId);				
		}
		#endregion

		
	}
}
