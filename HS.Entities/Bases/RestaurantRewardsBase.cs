using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "RestaurantRewardsBase", Namespace = "http://www.hims-tech.com//entities")]
	public class RestaurantRewardsBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			DollarSpent = 2,
			ReedemValue = 3,
			Status = 4,
			CreatedDate = 5,
			CreatedBy = 6,
			LastUpdatedBy = 7,
			LastUpdatedDate = 8
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_DollarSpent = "DollarSpent";		            
		public const string Property_ReedemValue = "ReedemValue";		            
		public const string Property_Status = "Status";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Double _DollarSpent;	            
		private Double _ReedemValue;	            
		private Boolean _Status;	            
		private DateTime _CreatedDate;	            
		private Guid _CreatedBy;	            
		private Guid _LastUpdatedBy;	            
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
		public Double DollarSpent
		{	
			get{ return _DollarSpent; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DollarSpent, value, _DollarSpent);
				if (PropertyChanging(args))
				{
					_DollarSpent = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Double ReedemValue
		{	
			get{ return _ReedemValue; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReedemValue, value, _ReedemValue);
				if (PropertyChanging(args))
				{
					_ReedemValue = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean Status
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

		#endregion
		
		#region Cloning Base Objects
		public  RestaurantRewardsBase Clone()
		{
			RestaurantRewardsBase newObj = new  RestaurantRewardsBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.DollarSpent = this.DollarSpent;						
			newObj.ReedemValue = this.ReedemValue;						
			newObj.Status = this.Status;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(RestaurantRewardsBase.Property_Id, Id);				
			info.AddValue(RestaurantRewardsBase.Property_CompanyId, CompanyId);				
			info.AddValue(RestaurantRewardsBase.Property_DollarSpent, DollarSpent);				
			info.AddValue(RestaurantRewardsBase.Property_ReedemValue, ReedemValue);				
			info.AddValue(RestaurantRewardsBase.Property_Status, Status);				
			info.AddValue(RestaurantRewardsBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(RestaurantRewardsBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(RestaurantRewardsBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(RestaurantRewardsBase.Property_LastUpdatedDate, LastUpdatedDate);				
		}
		#endregion

		
	}
}
