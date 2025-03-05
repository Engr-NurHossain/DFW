using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "AdditionalMembersAppointmentBase", Namespace = "http://www.piistech.com//entities")]
	public class AdditionalMembersAppointmentBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			AppointmentId = 1,
			CompanyId = 2,
			CustomerId = 3,
			EmployeeId = 4,
			AppointmentDate = 5,
			AppointmentStartTime = 6,
			AppointmentEndTime = 7,
			CreatedBy = 8,
			LastUpdatedBy = 9,
			LastUpdatedDate = 10,
			MemberAppointmentId = 11,
			IsAllDay = 12
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_AppointmentId = "AppointmentId";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_EmployeeId = "EmployeeId";		            
		public const string Property_AppointmentDate = "AppointmentDate";		            
		public const string Property_AppointmentStartTime = "AppointmentStartTime";		            
		public const string Property_AppointmentEndTime = "AppointmentEndTime";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_MemberAppointmentId = "MemberAppointmentId";		            
		public const string Property_IsAllDay = "IsAllDay";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _AppointmentId;	            
		private Guid _CompanyId;	            
		private Guid _CustomerId;	            
		private Guid _EmployeeId;	            
		private Nullable<DateTime> _AppointmentDate;	            
		private String _AppointmentStartTime;	            
		private String _AppointmentEndTime;	            
		private Guid _CreatedBy;	            
		private Guid _LastUpdatedBy;	            
		private DateTime _LastUpdatedDate;	            
		private Guid _MemberAppointmentId;	            
		private Nullable<Boolean> _IsAllDay;	            
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
		public Guid AppointmentId
		{	
			get{ return _AppointmentId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AppointmentId, value, _AppointmentId);
				if (PropertyChanging(args))
				{
					_AppointmentId = value;
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
		public Nullable<DateTime> AppointmentDate
		{	
			get{ return _AppointmentDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AppointmentDate, value, _AppointmentDate);
				if (PropertyChanging(args))
				{
					_AppointmentDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AppointmentStartTime
		{	
			get{ return _AppointmentStartTime; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AppointmentStartTime, value, _AppointmentStartTime);
				if (PropertyChanging(args))
				{
					_AppointmentStartTime = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AppointmentEndTime
		{	
			get{ return _AppointmentEndTime; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AppointmentEndTime, value, _AppointmentEndTime);
				if (PropertyChanging(args))
				{
					_AppointmentEndTime = value;
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
		public Guid MemberAppointmentId
		{	
			get{ return _MemberAppointmentId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MemberAppointmentId, value, _MemberAppointmentId);
				if (PropertyChanging(args))
				{
					_MemberAppointmentId = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  AdditionalMembersAppointmentBase Clone()
		{
			AdditionalMembersAppointmentBase newObj = new  AdditionalMembersAppointmentBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.AppointmentId = this.AppointmentId;						
			newObj.CompanyId = this.CompanyId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.EmployeeId = this.EmployeeId;						
			newObj.AppointmentDate = this.AppointmentDate;						
			newObj.AppointmentStartTime = this.AppointmentStartTime;						
			newObj.AppointmentEndTime = this.AppointmentEndTime;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.MemberAppointmentId = this.MemberAppointmentId;						
			newObj.IsAllDay = this.IsAllDay;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(AdditionalMembersAppointmentBase.Property_Id, Id);				
			info.AddValue(AdditionalMembersAppointmentBase.Property_AppointmentId, AppointmentId);				
			info.AddValue(AdditionalMembersAppointmentBase.Property_CompanyId, CompanyId);				
			info.AddValue(AdditionalMembersAppointmentBase.Property_CustomerId, CustomerId);				
			info.AddValue(AdditionalMembersAppointmentBase.Property_EmployeeId, EmployeeId);				
			info.AddValue(AdditionalMembersAppointmentBase.Property_AppointmentDate, AppointmentDate);				
			info.AddValue(AdditionalMembersAppointmentBase.Property_AppointmentStartTime, AppointmentStartTime);				
			info.AddValue(AdditionalMembersAppointmentBase.Property_AppointmentEndTime, AppointmentEndTime);				
			info.AddValue(AdditionalMembersAppointmentBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(AdditionalMembersAppointmentBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(AdditionalMembersAppointmentBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(AdditionalMembersAppointmentBase.Property_MemberAppointmentId, MemberAppointmentId);				
			info.AddValue(AdditionalMembersAppointmentBase.Property_IsAllDay, IsAllDay);				
		}
		#endregion

		
	}
}
