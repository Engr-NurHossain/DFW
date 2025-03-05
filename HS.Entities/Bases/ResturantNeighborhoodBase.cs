using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "ResturantNeighborhoodBase", Namespace = "http://www.hims-tech.com//entities")]
	public class ResturantNeighborhoodBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			SiteLocationId = 1,
			NeighborhoodName = 2,
			NeighborhoodURL = 3,
			CreatedBy = 4,
			CreatedDate = 5
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_SiteLocationId = "SiteLocationId";		            
		public const string Property_NeighborhoodName = "NeighborhoodName";		            
		public const string Property_NeighborhoodURL = "NeighborhoodURL";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Int32 _SiteLocationId;	            
		private String _NeighborhoodName;	            
		private String _NeighborhoodURL;	            
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
		public Int32 SiteLocationId
		{	
			get{ return _SiteLocationId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SiteLocationId, value, _SiteLocationId);
				if (PropertyChanging(args))
				{
					_SiteLocationId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String NeighborhoodName
		{	
			get{ return _NeighborhoodName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_NeighborhoodName, value, _NeighborhoodName);
				if (PropertyChanging(args))
				{
					_NeighborhoodName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String NeighborhoodURL
		{	
			get{ return _NeighborhoodURL; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_NeighborhoodURL, value, _NeighborhoodURL);
				if (PropertyChanging(args))
				{
					_NeighborhoodURL = value;
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
		public  ResturantNeighborhoodBase Clone()
		{
			ResturantNeighborhoodBase newObj = new  ResturantNeighborhoodBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.SiteLocationId = this.SiteLocationId;						
			newObj.NeighborhoodName = this.NeighborhoodName;						
			newObj.NeighborhoodURL = this.NeighborhoodURL;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(ResturantNeighborhoodBase.Property_Id, Id);				
			info.AddValue(ResturantNeighborhoodBase.Property_SiteLocationId, SiteLocationId);				
			info.AddValue(ResturantNeighborhoodBase.Property_NeighborhoodName, NeighborhoodName);				
			info.AddValue(ResturantNeighborhoodBase.Property_NeighborhoodURL, NeighborhoodURL);				
			info.AddValue(ResturantNeighborhoodBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(ResturantNeighborhoodBase.Property_CreatedDate, CreatedDate);				
		}
		#endregion

		
	}
}
