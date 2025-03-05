using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "EmployeeTimeClockBase", Namespace = "http://www.piistech.com//entities")]
	public class EmployeeTimeClockBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			UserId = 1,
			ClockInTime = 2,
			ClockOutTime = 3,
			ClockInLat = 4,
			ClockInLng = 5,
			ClockOutLat = 6,
			ClockOutLng = 7,
			ClockInNote = 8,
			ClockOutNote = 9,
			ClockInCreatedBy = 10,
			ClockOutCreatedBy = 11,
			ClockedInSeconds = 12,
			LastUpdateBy = 13,
			LastUpdatedDate = 14
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_UserId = "UserId";		            
		public const string Property_ClockInTime = "ClockInTime";		            
		public const string Property_ClockOutTime = "ClockOutTime";		            
		public const string Property_ClockInLat = "ClockInLat";		            
		public const string Property_ClockInLng = "ClockInLng";		            
		public const string Property_ClockOutLat = "ClockOutLat";		            
		public const string Property_ClockOutLng = "ClockOutLng";		            
		public const string Property_ClockInNote = "ClockInNote";		            
		public const string Property_ClockOutNote = "ClockOutNote";		            
		public const string Property_ClockInCreatedBy = "ClockInCreatedBy";		            
		public const string Property_ClockOutCreatedBy = "ClockOutCreatedBy";		            
		public const string Property_ClockedInSeconds = "ClockedInSeconds";		            
		public const string Property_LastUpdateBy = "LastUpdateBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _UserId;	            
		private DateTime _ClockInTime;	            
		private Nullable<DateTime> _ClockOutTime;	            
		private String _ClockInLat;	            
		private String _ClockInLng;	            
		private String _ClockOutLat;	            
		private String _ClockOutLng;	            
		private String _ClockInNote;	            
		private String _ClockOutNote;	            
		private Guid _ClockInCreatedBy;	            
		private Guid _ClockOutCreatedBy;	            
		private Nullable<Int32> _ClockedInSeconds;	            
		private Guid _LastUpdateBy;	            
		private DateTime _LastUpdatedDate;	            
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
		public DateTime ClockInTime
		{	
			get{ return _ClockInTime; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ClockInTime, value, _ClockInTime);
				if (PropertyChanging(args))
				{
					_ClockInTime = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> ClockOutTime
		{	
			get{ return _ClockOutTime; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ClockOutTime, value, _ClockOutTime);
				if (PropertyChanging(args))
				{
					_ClockOutTime = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ClockInLat
		{	
			get{ return _ClockInLat; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ClockInLat, value, _ClockInLat);
				if (PropertyChanging(args))
				{
					_ClockInLat = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ClockInLng
		{	
			get{ return _ClockInLng; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ClockInLng, value, _ClockInLng);
				if (PropertyChanging(args))
				{
					_ClockInLng = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ClockOutLat
		{	
			get{ return _ClockOutLat; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ClockOutLat, value, _ClockOutLat);
				if (PropertyChanging(args))
				{
					_ClockOutLat = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ClockOutLng
		{	
			get{ return _ClockOutLng; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ClockOutLng, value, _ClockOutLng);
				if (PropertyChanging(args))
				{
					_ClockOutLng = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ClockInNote
		{	
			get{ return _ClockInNote; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ClockInNote, value, _ClockInNote);
				if (PropertyChanging(args))
				{
					_ClockInNote = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ClockOutNote
		{	
			get{ return _ClockOutNote; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ClockOutNote, value, _ClockOutNote);
				if (PropertyChanging(args))
				{
					_ClockOutNote = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid ClockInCreatedBy
		{	
			get{ return _ClockInCreatedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ClockInCreatedBy, value, _ClockInCreatedBy);
				if (PropertyChanging(args))
				{
					_ClockInCreatedBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid ClockOutCreatedBy
		{	
			get{ return _ClockOutCreatedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ClockOutCreatedBy, value, _ClockOutCreatedBy);
				if (PropertyChanging(args))
				{
					_ClockOutCreatedBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> ClockedInSeconds
		{	
			get{ return _ClockedInSeconds; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ClockedInSeconds, value, _ClockedInSeconds);
				if (PropertyChanging(args))
				{
					_ClockedInSeconds = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid LastUpdateBy
		{	
			get{ return _LastUpdateBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastUpdateBy, value, _LastUpdateBy);
				if (PropertyChanging(args))
				{
					_LastUpdateBy = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  EmployeeTimeClockBase Clone()
		{
			EmployeeTimeClockBase newObj = new  EmployeeTimeClockBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.UserId = this.UserId;						
			newObj.ClockInTime = this.ClockInTime;						
			newObj.ClockOutTime = this.ClockOutTime;						
			newObj.ClockInLat = this.ClockInLat;						
			newObj.ClockInLng = this.ClockInLng;						
			newObj.ClockOutLat = this.ClockOutLat;						
			newObj.ClockOutLng = this.ClockOutLng;						
			newObj.ClockInNote = this.ClockInNote;						
			newObj.ClockOutNote = this.ClockOutNote;						
			newObj.ClockInCreatedBy = this.ClockInCreatedBy;						
			newObj.ClockOutCreatedBy = this.ClockOutCreatedBy;						
			newObj.ClockedInSeconds = this.ClockedInSeconds;						
			newObj.LastUpdateBy = this.LastUpdateBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(EmployeeTimeClockBase.Property_Id, Id);				
			info.AddValue(EmployeeTimeClockBase.Property_UserId, UserId);				
			info.AddValue(EmployeeTimeClockBase.Property_ClockInTime, ClockInTime);				
			info.AddValue(EmployeeTimeClockBase.Property_ClockOutTime, ClockOutTime);				
			info.AddValue(EmployeeTimeClockBase.Property_ClockInLat, ClockInLat);				
			info.AddValue(EmployeeTimeClockBase.Property_ClockInLng, ClockInLng);				
			info.AddValue(EmployeeTimeClockBase.Property_ClockOutLat, ClockOutLat);				
			info.AddValue(EmployeeTimeClockBase.Property_ClockOutLng, ClockOutLng);				
			info.AddValue(EmployeeTimeClockBase.Property_ClockInNote, ClockInNote);				
			info.AddValue(EmployeeTimeClockBase.Property_ClockOutNote, ClockOutNote);				
			info.AddValue(EmployeeTimeClockBase.Property_ClockInCreatedBy, ClockInCreatedBy);				
			info.AddValue(EmployeeTimeClockBase.Property_ClockOutCreatedBy, ClockOutCreatedBy);				
			info.AddValue(EmployeeTimeClockBase.Property_ClockedInSeconds, ClockedInSeconds);				
			info.AddValue(EmployeeTimeClockBase.Property_LastUpdateBy, LastUpdateBy);				
			info.AddValue(EmployeeTimeClockBase.Property_LastUpdatedDate, LastUpdatedDate);				
		}
		#endregion

		
	}
}
