using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "PtoBase", Namespace = "http://www.piistech.com//entities")]
	public class PtoBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			UserId = 1,
			Type = 2,
			StartDate = 3,
			EndDate = 4,
			LeaveTime = 5,
			TimeFrom = 6,
			TimeTo = 7,
			Notes = 8,
			Status = 9,
			CreatedBy = 10,
			CreatedDate = 11,
			LastUpdatedBy = 12,
			LastUpdatedDate = 13,
			Payable = 14,
			RejectNote = 15,
			Minute = 16
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_UserId = "UserId";		            
		public const string Property_Type = "Type";		            
		public const string Property_StartDate = "StartDate";		            
		public const string Property_EndDate = "EndDate";		            
		public const string Property_LeaveTime = "LeaveTime";		            
		public const string Property_TimeFrom = "TimeFrom";		            
		public const string Property_TimeTo = "TimeTo";		            
		public const string Property_Notes = "Notes";		            
		public const string Property_Status = "Status";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_Payable = "Payable";		            
		public const string Property_RejectNote = "RejectNote";		            
		public const string Property_Minute = "Minute";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _UserId;	            
		private String _Type;	            
		private Nullable<DateTime> _StartDate;	            
		private Nullable<DateTime> _EndDate;	            
		private String _LeaveTime;	            
		private String _TimeFrom;	            
		private String _TimeTo;	            
		private String _Notes;	            
		private String _Status;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
		private Guid _LastUpdatedBy;	            
		private DateTime _LastUpdatedDate;	            
		private Nullable<Boolean> _Payable;	            
		private String _RejectNote;	            
		private Nullable<Int32> _Minute;	            
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
		public Nullable<DateTime> StartDate
		{	
			get{ return _StartDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_StartDate, value, _StartDate);
				if (PropertyChanging(args))
				{
					_StartDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> EndDate
		{	
			get{ return _EndDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EndDate, value, _EndDate);
				if (PropertyChanging(args))
				{
					_EndDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String LeaveTime
		{	
			get{ return _LeaveTime; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LeaveTime, value, _LeaveTime);
				if (PropertyChanging(args))
				{
					_LeaveTime = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TimeFrom
		{	
			get{ return _TimeFrom; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TimeFrom, value, _TimeFrom);
				if (PropertyChanging(args))
				{
					_TimeFrom = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TimeTo
		{	
			get{ return _TimeTo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TimeTo, value, _TimeTo);
				if (PropertyChanging(args))
				{
					_TimeTo = value;
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
		public Nullable<Boolean> Payable
		{	
			get{ return _Payable; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Payable, value, _Payable);
				if (PropertyChanging(args))
				{
					_Payable = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String RejectNote
		{	
			get{ return _RejectNote; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RejectNote, value, _RejectNote);
				if (PropertyChanging(args))
				{
					_RejectNote = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> Minute
		{	
			get{ return _Minute; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Minute, value, _Minute);
				if (PropertyChanging(args))
				{
					_Minute = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  PtoBase Clone()
		{
			PtoBase newObj = new  PtoBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.UserId = this.UserId;						
			newObj.Type = this.Type;						
			newObj.StartDate = this.StartDate;						
			newObj.EndDate = this.EndDate;						
			newObj.LeaveTime = this.LeaveTime;						
			newObj.TimeFrom = this.TimeFrom;						
			newObj.TimeTo = this.TimeTo;						
			newObj.Notes = this.Notes;						
			newObj.Status = this.Status;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.Payable = this.Payable;						
			newObj.RejectNote = this.RejectNote;						
			newObj.Minute = this.Minute;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(PtoBase.Property_Id, Id);				
			info.AddValue(PtoBase.Property_UserId, UserId);				
			info.AddValue(PtoBase.Property_Type, Type);				
			info.AddValue(PtoBase.Property_StartDate, StartDate);				
			info.AddValue(PtoBase.Property_EndDate, EndDate);				
			info.AddValue(PtoBase.Property_LeaveTime, LeaveTime);				
			info.AddValue(PtoBase.Property_TimeFrom, TimeFrom);				
			info.AddValue(PtoBase.Property_TimeTo, TimeTo);				
			info.AddValue(PtoBase.Property_Notes, Notes);				
			info.AddValue(PtoBase.Property_Status, Status);				
			info.AddValue(PtoBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(PtoBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(PtoBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(PtoBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(PtoBase.Property_Payable, Payable);				
			info.AddValue(PtoBase.Property_RejectNote, RejectNote);				
			info.AddValue(PtoBase.Property_Minute, Minute);				
		}
		#endregion

		
	}
}
