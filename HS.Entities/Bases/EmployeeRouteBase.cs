using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "EmployeeRouteBase", Namespace = "http://www.hims-tech.com//entities")]
	public class EmployeeRouteBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			RouteId = 1,
			UserId = 2,
			CreatedBy = 3,
			CreatedDate = 4,
			UpdatedBy = 5,
			UpdatedDate = 6
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_RouteId = "RouteId";		            
		public const string Property_UserId = "UserId";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_UpdatedBy = "UpdatedBy";		            
		public const string Property_UpdatedDate = "UpdatedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _RouteId;	            
		private Guid _UserId;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
		private Guid _UpdatedBy;	            
		private DateTime _UpdatedDate;	            
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
		public Guid RouteId
		{	
			get{ return _RouteId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RouteId, value, _RouteId);
				if (PropertyChanging(args))
				{
					_RouteId = value;
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
		public Guid UpdatedBy
		{	
			get{ return _UpdatedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UpdatedBy, value, _UpdatedBy);
				if (PropertyChanging(args))
				{
					_UpdatedBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime UpdatedDate
		{	
			get{ return _UpdatedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UpdatedDate, value, _UpdatedDate);
				if (PropertyChanging(args))
				{
					_UpdatedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  EmployeeRouteBase Clone()
		{
			EmployeeRouteBase newObj = new  EmployeeRouteBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.RouteId = this.RouteId;						
			newObj.UserId = this.UserId;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.UpdatedBy = this.UpdatedBy;						
			newObj.UpdatedDate = this.UpdatedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(EmployeeRouteBase.Property_Id, Id);				
			info.AddValue(EmployeeRouteBase.Property_RouteId, RouteId);				
			info.AddValue(EmployeeRouteBase.Property_UserId, UserId);				
			info.AddValue(EmployeeRouteBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(EmployeeRouteBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(EmployeeRouteBase.Property_UpdatedBy, UpdatedBy);				
			info.AddValue(EmployeeRouteBase.Property_UpdatedDate, UpdatedDate);				
		}
		#endregion

		
	}
}
