using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "EmployeeTimeClockSupervisorBase", Namespace = "http://www.piistech.com//entities")]
	public class EmployeeTimeClockSupervisorBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			UserId = 1,
			SupervisorId = 2,
			CreatedBy = 3,
			CreatedDate = 4
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_UserId = "UserId";		            
		public const string Property_SupervisorId = "SupervisorId";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _UserId;	            
		private Guid _SupervisorId;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
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
		public Guid SupervisorId
		{	
			get{ return _SupervisorId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SupervisorId, value, _SupervisorId);
				if (PropertyChanging(args))
				{
					_SupervisorId = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  EmployeeTimeClockSupervisorBase Clone()
		{
			EmployeeTimeClockSupervisorBase newObj = new  EmployeeTimeClockSupervisorBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.UserId = this.UserId;						
			newObj.SupervisorId = this.SupervisorId;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(EmployeeTimeClockSupervisorBase.Property_Id, Id);				
			info.AddValue(EmployeeTimeClockSupervisorBase.Property_UserId, UserId);				
			info.AddValue(EmployeeTimeClockSupervisorBase.Property_SupervisorId, SupervisorId);				
			info.AddValue(EmployeeTimeClockSupervisorBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(EmployeeTimeClockSupervisorBase.Property_CreatedDate, CreatedDate);				
		}
		#endregion

		
	}
}
