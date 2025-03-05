using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "EmployeeEvaluationBase", Namespace = "http://www.piistech.com//entities")]
	public class EmployeeEvaluationBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			UserId = 1,
			EvaluationReminderDate = 2,
			EvaluationType = 3,
			CreatedByUid = 4,
			CreatedDate = 5,
			LastUpdatedByUid = 6,
			LastUpdatedDate = 7,
			LastEvaluationDate = 8,
			NextEvaluationDate = 9
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_UserId = "UserId";		            
		public const string Property_EvaluationReminderDate = "EvaluationReminderDate";		            
		public const string Property_EvaluationType = "EvaluationType";		            
		public const string Property_CreatedByUid = "CreatedByUid";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdatedByUid = "LastUpdatedByUid";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_LastEvaluationDate = "LastEvaluationDate";		            
		public const string Property_NextEvaluationDate = "NextEvaluationDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _UserId;	            
		private Nullable<DateTime> _EvaluationReminderDate;	            
		private String _EvaluationType;	            
		private Guid _CreatedByUid;	            
		private DateTime _CreatedDate;	            
		private Guid _LastUpdatedByUid;	            
		private DateTime _LastUpdatedDate;	            
		private Nullable<DateTime> _LastEvaluationDate;	            
		private Nullable<DateTime> _NextEvaluationDate;	            
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
		public Nullable<DateTime> EvaluationReminderDate
		{	
			get{ return _EvaluationReminderDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EvaluationReminderDate, value, _EvaluationReminderDate);
				if (PropertyChanging(args))
				{
					_EvaluationReminderDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String EvaluationType
		{	
			get{ return _EvaluationType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EvaluationType, value, _EvaluationType);
				if (PropertyChanging(args))
				{
					_EvaluationType = value;
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
		public Guid LastUpdatedByUid
		{	
			get{ return _LastUpdatedByUid; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastUpdatedByUid, value, _LastUpdatedByUid);
				if (PropertyChanging(args))
				{
					_LastUpdatedByUid = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime LastUpdatedDate
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
		public Nullable<DateTime> LastEvaluationDate
		{	
			get{ return _LastEvaluationDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastEvaluationDate, value, _LastEvaluationDate);
				if (PropertyChanging(args))
				{
					_LastEvaluationDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> NextEvaluationDate
		{	
			get{ return _NextEvaluationDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_NextEvaluationDate, value, _NextEvaluationDate);
				if (PropertyChanging(args))
				{
					_NextEvaluationDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  EmployeeEvaluationBase Clone()
		{
			EmployeeEvaluationBase newObj = new  EmployeeEvaluationBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.UserId = this.UserId;						
			newObj.EvaluationReminderDate = this.EvaluationReminderDate;						
			newObj.EvaluationType = this.EvaluationType;						
			newObj.CreatedByUid = this.CreatedByUid;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.LastUpdatedByUid = this.LastUpdatedByUid;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.LastEvaluationDate = this.LastEvaluationDate;						
			newObj.NextEvaluationDate = this.NextEvaluationDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(EmployeeEvaluationBase.Property_Id, Id);				
			info.AddValue(EmployeeEvaluationBase.Property_UserId, UserId);				
			info.AddValue(EmployeeEvaluationBase.Property_EvaluationReminderDate, EvaluationReminderDate);				
			info.AddValue(EmployeeEvaluationBase.Property_EvaluationType, EvaluationType);				
			info.AddValue(EmployeeEvaluationBase.Property_CreatedByUid, CreatedByUid);				
			info.AddValue(EmployeeEvaluationBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(EmployeeEvaluationBase.Property_LastUpdatedByUid, LastUpdatedByUid);				
			info.AddValue(EmployeeEvaluationBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(EmployeeEvaluationBase.Property_LastEvaluationDate, LastEvaluationDate);				
			info.AddValue(EmployeeEvaluationBase.Property_NextEvaluationDate, NextEvaluationDate);				
		}
		#endregion

		
	}
}
