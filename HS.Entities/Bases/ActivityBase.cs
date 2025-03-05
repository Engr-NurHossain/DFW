using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "ActivityBase", Namespace = "http://www.hims-tech.com//entities")]
	public class ActivityBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			ActivityId = 1,
			ActivityType = 2,
			Description = 3,
			AssignedTo = 4,
			DueDate = 5,
			Status = 6,
			AssociatedWith = 7,
			AssociatedType = 8,
			Note = 9,
			CreatedBy = 10,
			CreatedDate = 11,
			NotifyBy = 12,
			Origin = 13,
			Department = 14
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_ActivityId = "ActivityId";		            
		public const string Property_ActivityType = "ActivityType";		            
		public const string Property_Description = "Description";		            
		public const string Property_AssignedTo = "AssignedTo";		            
		public const string Property_DueDate = "DueDate";		            
		public const string Property_Status = "Status";		            
		public const string Property_AssociatedWith = "AssociatedWith";		            
		public const string Property_AssociatedType = "AssociatedType";		            
		public const string Property_Note = "Note";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_NotifyBy = "NotifyBy";		            
		public const string Property_Origin = "Origin";		            
		public const string Property_Department = "Department";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _ActivityId;	            
		private String _ActivityType;	            
		private String _Description;	            
		private Guid _AssignedTo;	            
		private Nullable<DateTime> _DueDate;	            
		private String _Status;	            
		private Guid _AssociatedWith;	            
		private String _AssociatedType;	            
		private String _Note;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
		private String _NotifyBy;	            
		private String _Origin;	            
		private String _Department;	            
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
		public String ActivityType
		{	
			get{ return _ActivityType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ActivityType, value, _ActivityType);
				if (PropertyChanging(args))
				{
					_ActivityType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Description
		{	
			get{ return _Description; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Description, value, _Description);
				if (PropertyChanging(args))
				{
					_Description = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid AssignedTo
		{	
			get{ return _AssignedTo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AssignedTo, value, _AssignedTo);
				if (PropertyChanging(args))
				{
					_AssignedTo = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> DueDate
		{	
			get{ return _DueDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DueDate, value, _DueDate);
				if (PropertyChanging(args))
				{
					_DueDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Status
		{	
			get{ return _Status; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Status, value, _Status);
				if (PropertyChanging(args))
				{
					_Status = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid AssociatedWith
		{	
			get{ return _AssociatedWith; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AssociatedWith, value, _AssociatedWith);
				if (PropertyChanging(args))
				{
					_AssociatedWith = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AssociatedType
		{	
			get{ return _AssociatedType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AssociatedType, value, _AssociatedType);
				if (PropertyChanging(args))
				{
					_AssociatedType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Note
		{	
			get{ return _Note; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Note, value, _Note);
				if (PropertyChanging(args))
				{
					_Note = value;
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
		public String NotifyBy
		{	
			get{ return _NotifyBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_NotifyBy, value, _NotifyBy);
				if (PropertyChanging(args))
				{
					_NotifyBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Origin
		{	
			get{ return _Origin; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Origin, value, _Origin);
				if (PropertyChanging(args))
				{
					_Origin = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Department
		{	
			get{ return _Department; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Department, value, _Department);
				if (PropertyChanging(args))
				{
					_Department = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  ActivityBase Clone()
		{
			ActivityBase newObj = new  ActivityBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.ActivityId = this.ActivityId;						
			newObj.ActivityType = this.ActivityType;						
			newObj.Description = this.Description;						
			newObj.AssignedTo = this.AssignedTo;						
			newObj.DueDate = this.DueDate;						
			newObj.Status = this.Status;						
			newObj.AssociatedWith = this.AssociatedWith;						
			newObj.AssociatedType = this.AssociatedType;						
			newObj.Note = this.Note;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.NotifyBy = this.NotifyBy;						
			newObj.Origin = this.Origin;						
			newObj.Department = this.Department;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(ActivityBase.Property_Id, Id);				
			info.AddValue(ActivityBase.Property_ActivityId, ActivityId);				
			info.AddValue(ActivityBase.Property_ActivityType, ActivityType);				
			info.AddValue(ActivityBase.Property_Description, Description);				
			info.AddValue(ActivityBase.Property_AssignedTo, AssignedTo);				
			info.AddValue(ActivityBase.Property_DueDate, DueDate);				
			info.AddValue(ActivityBase.Property_Status, Status);				
			info.AddValue(ActivityBase.Property_AssociatedWith, AssociatedWith);				
			info.AddValue(ActivityBase.Property_AssociatedType, AssociatedType);				
			info.AddValue(ActivityBase.Property_Note, Note);				
			info.AddValue(ActivityBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(ActivityBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(ActivityBase.Property_NotifyBy, NotifyBy);				
			info.AddValue(ActivityBase.Property_Origin, Origin);				
			info.AddValue(ActivityBase.Property_Department, Department);				
		}
		#endregion

		
	}
}
