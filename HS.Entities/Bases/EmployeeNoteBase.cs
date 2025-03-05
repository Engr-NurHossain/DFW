using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "EmployeeNoteBase", Namespace = "http://www.piistech.com//entities")]
	public class EmployeeNoteBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			Notes = 1,
			ReminderDate = 2,
			EmployeeId = 3,
			CompanyId = 4,
			CreatedDate = 5,
			IsEmail = 6,
			IsText = 7,
			IsShedule = 8,
			IsFollowUp = 9,
			IsActive = 10,
			CreatedBy = 11,
			IsClose = 12
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_Notes = "Notes";		            
		public const string Property_ReminderDate = "ReminderDate";		            
		public const string Property_EmployeeId = "EmployeeId";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_IsEmail = "IsEmail";		            
		public const string Property_IsText = "IsText";		            
		public const string Property_IsShedule = "IsShedule";		            
		public const string Property_IsFollowUp = "IsFollowUp";		            
		public const string Property_IsActive = "IsActive";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_IsClose = "IsClose";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _Notes;	            
		private Nullable<DateTime> _ReminderDate;	            
		private Guid _EmployeeId;	            
		private Guid _CompanyId;	            
		private DateTime _CreatedDate;	            
		private Nullable<Boolean> _IsEmail;	            
		private Nullable<Boolean> _IsText;	            
		private Nullable<Boolean> _IsShedule;	            
		private Nullable<Boolean> _IsFollowUp;	            
		private Nullable<Boolean> _IsActive;	            
		private String _CreatedBy;	            
		private Nullable<Boolean> _IsClose;	            
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
		public Guid EmployeeId
		{	
			get{ return _EmployeeId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EmployeeId, value, _EmployeeId);
				if (PropertyChanging(args))
				{
					_EmployeeId = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  EmployeeNoteBase Clone()
		{
			EmployeeNoteBase newObj = new  EmployeeNoteBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.Notes = this.Notes;						
			newObj.ReminderDate = this.ReminderDate;						
			newObj.EmployeeId = this.EmployeeId;						
			newObj.CompanyId = this.CompanyId;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.IsEmail = this.IsEmail;						
			newObj.IsText = this.IsText;						
			newObj.IsShedule = this.IsShedule;						
			newObj.IsFollowUp = this.IsFollowUp;						
			newObj.IsActive = this.IsActive;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.IsClose = this.IsClose;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(EmployeeNoteBase.Property_Id, Id);				
			info.AddValue(EmployeeNoteBase.Property_Notes, Notes);				
			info.AddValue(EmployeeNoteBase.Property_ReminderDate, ReminderDate);				
			info.AddValue(EmployeeNoteBase.Property_EmployeeId, EmployeeId);				
			info.AddValue(EmployeeNoteBase.Property_CompanyId, CompanyId);				
			info.AddValue(EmployeeNoteBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(EmployeeNoteBase.Property_IsEmail, IsEmail);				
			info.AddValue(EmployeeNoteBase.Property_IsText, IsText);				
			info.AddValue(EmployeeNoteBase.Property_IsShedule, IsShedule);				
			info.AddValue(EmployeeNoteBase.Property_IsFollowUp, IsFollowUp);				
			info.AddValue(EmployeeNoteBase.Property_IsActive, IsActive);				
			info.AddValue(EmployeeNoteBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(EmployeeNoteBase.Property_IsClose, IsClose);				
		}
		#endregion

		
	}
}
