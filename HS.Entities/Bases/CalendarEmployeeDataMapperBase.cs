using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CalendarEmployeeDataMapperBase", Namespace = "http://www.hims-tech.com//entities")]
	public class CalendarEmployeeDataMapperBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			UserId = 1,
			IsActive = 2,
			MapType = 3,
			CreatedDate = 4,
			EmplyeeSelectedId = 5
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_UserId = "UserId";		            
		public const string Property_IsActive = "IsActive";		            
		public const string Property_MapType = "MapType";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_EmplyeeSelectedId = "EmplyeeSelectedId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _UserId;	            
		private Nullable<Boolean> _IsActive;	            
		private String _MapType;	            
		private DateTime _CreatedDate;	            
		private Guid _EmplyeeSelectedId;	            
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
		public String MapType
		{	
			get{ return _MapType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MapType, value, _MapType);
				if (PropertyChanging(args))
				{
					_MapType = value;
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
		public Guid EmplyeeSelectedId
		{	
			get{ return _EmplyeeSelectedId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EmplyeeSelectedId, value, _EmplyeeSelectedId);
				if (PropertyChanging(args))
				{
					_EmplyeeSelectedId = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  CalendarEmployeeDataMapperBase Clone()
		{
			CalendarEmployeeDataMapperBase newObj = new  CalendarEmployeeDataMapperBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.UserId = this.UserId;						
			newObj.IsActive = this.IsActive;						
			newObj.MapType = this.MapType;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.EmplyeeSelectedId = this.EmplyeeSelectedId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CalendarEmployeeDataMapperBase.Property_Id, Id);				
			info.AddValue(CalendarEmployeeDataMapperBase.Property_UserId, UserId);				
			info.AddValue(CalendarEmployeeDataMapperBase.Property_IsActive, IsActive);				
			info.AddValue(CalendarEmployeeDataMapperBase.Property_MapType, MapType);				
			info.AddValue(CalendarEmployeeDataMapperBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(CalendarEmployeeDataMapperBase.Property_EmplyeeSelectedId, EmplyeeSelectedId);				
		}
		#endregion

		
	}
}
