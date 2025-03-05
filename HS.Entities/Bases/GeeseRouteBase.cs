using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "GeeseRouteBase", Namespace = "http://www.hims-tech.com//entities")]
	public class GeeseRouteBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			Name = 1,
			RouteId = 2,
			CreatedBy = 3,
			CreatedDate = 4,
			UpdatedBy = 5,
			UpdatedDate = 6
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_Name = "Name";		            
		public const string Property_RouteId = "RouteId";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_UpdatedBy = "UpdatedBy";		            
		public const string Property_UpdatedDate = "UpdatedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _Name;	            
		private Guid _RouteId;	            
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
		public  GeeseRouteBase Clone()
		{
			GeeseRouteBase newObj = new  GeeseRouteBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.Name = this.Name;						
			newObj.RouteId = this.RouteId;						
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
			info.AddValue(GeeseRouteBase.Property_Id, Id);				
			info.AddValue(GeeseRouteBase.Property_Name, Name);				
			info.AddValue(GeeseRouteBase.Property_RouteId, RouteId);				
			info.AddValue(GeeseRouteBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(GeeseRouteBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(GeeseRouteBase.Property_UpdatedBy, UpdatedBy);				
			info.AddValue(GeeseRouteBase.Property_UpdatedDate, UpdatedDate);				
		}
		#endregion

		
	}
}
