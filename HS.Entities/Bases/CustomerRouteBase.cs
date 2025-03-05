using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomerRouteBase", Namespace = "http://www.hims-tech.com//entities")]
	public class CustomerRouteBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CustomerId = 1,
			RouteId = 2,
			Name = 3,
			LastVisit = 4,
			CreatedBy = 5,
			CreatedDate = 6,
			UserId = 7
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_RouteId = "RouteId";		            
		public const string Property_Name = "Name";		            
		public const string Property_LastVisit = "LastVisit";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_UserId = "UserId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CustomerId;	            
		private Guid _RouteId;	            
		private String _Name;	            
		private Nullable<DateTime> _LastVisit;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
		private Guid _UserId;	            
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
		public String Name
		{	
			get{ return _Name; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Name, value, _Name);
				if (PropertyChanging(args))
				{
					_Name = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> LastVisit
		{	
			get{ return _LastVisit; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastVisit, value, _LastVisit);
				if (PropertyChanging(args))
				{
					_LastVisit = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  CustomerRouteBase Clone()
		{
			CustomerRouteBase newObj = new  CustomerRouteBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CustomerId = this.CustomerId;						
			newObj.RouteId = this.RouteId;						
			newObj.Name = this.Name;						
			newObj.LastVisit = this.LastVisit;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.UserId = this.UserId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomerRouteBase.Property_Id, Id);				
			info.AddValue(CustomerRouteBase.Property_CustomerId, CustomerId);				
			info.AddValue(CustomerRouteBase.Property_RouteId, RouteId);				
			info.AddValue(CustomerRouteBase.Property_Name, Name);				
			info.AddValue(CustomerRouteBase.Property_LastVisit, LastVisit);				
			info.AddValue(CustomerRouteBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(CustomerRouteBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(CustomerRouteBase.Property_UserId, UserId);				
		}
		#endregion

		
	}
}
