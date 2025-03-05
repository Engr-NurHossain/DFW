using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "WebsiteLocationMapInfoBase", Namespace = "http://www.hims-tech.com//entities")]
	public class WebsiteLocationMapInfoBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			Latitude = 2,
			Longitude = 3,
			CreatedBy = 4,
			CreatedDate = 5,
			LastUpdatedBy = 6,
			LastUpdatedDate = 7
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_Latitude = "Latitude";		            
		public const string Property_Longitude = "Longitude";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private String _Latitude;	            
		private String _Longitude;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
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
		public String Latitude
		{	
			get{ return _Latitude; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Latitude, value, _Latitude);
				if (PropertyChanging(args))
				{
					_Latitude = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Longitude
		{	
			get{ return _Longitude; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Longitude, value, _Longitude);
				if (PropertyChanging(args))
				{
					_Longitude = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  WebsiteLocationMapInfoBase Clone()
		{
			WebsiteLocationMapInfoBase newObj = new  WebsiteLocationMapInfoBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.Latitude = this.Latitude;						
			newObj.Longitude = this.Longitude;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(WebsiteLocationMapInfoBase.Property_Id, Id);				
			info.AddValue(WebsiteLocationMapInfoBase.Property_CompanyId, CompanyId);				
			info.AddValue(WebsiteLocationMapInfoBase.Property_Latitude, Latitude);				
			info.AddValue(WebsiteLocationMapInfoBase.Property_Longitude, Longitude);				
			info.AddValue(WebsiteLocationMapInfoBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(WebsiteLocationMapInfoBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(WebsiteLocationMapInfoBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(WebsiteLocationMapInfoBase.Property_LastUpdatedDate, LastUpdatedDate);				
		}
		#endregion

		
	}
}
