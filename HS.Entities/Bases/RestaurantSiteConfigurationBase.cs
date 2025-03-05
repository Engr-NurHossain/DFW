using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "RestaurantSiteConfigurationBase", Namespace = "http://www.hims-tech.com//entities")]
	public class RestaurantSiteConfigurationBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			SiteName = 2,
			DomainName = 3,
			StorePhone = 4,
			SendOrdersEmail = 5,
			ThemeURL = 6,
			CreatedBy = 7,
			CreatedDate = 8,
			LastUpdatedBy = 9,
			LastUpdatedDate = 10
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_SiteName = "SiteName";		            
		public const string Property_DomainName = "DomainName";		            
		public const string Property_StorePhone = "StorePhone";		            
		public const string Property_SendOrdersEmail = "SendOrdersEmail";		            
		public const string Property_ThemeURL = "ThemeURL";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private String _SiteName;	            
		private String _DomainName;	            
		private String _StorePhone;	            
		private String _SendOrdersEmail;	            
		private String _ThemeURL;	            
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
		public String SiteName
		{	
			get{ return _SiteName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SiteName, value, _SiteName);
				if (PropertyChanging(args))
				{
					_SiteName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String DomainName
		{	
			get{ return _DomainName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DomainName, value, _DomainName);
				if (PropertyChanging(args))
				{
					_DomainName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String StorePhone
		{	
			get{ return _StorePhone; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_StorePhone, value, _StorePhone);
				if (PropertyChanging(args))
				{
					_StorePhone = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SendOrdersEmail
		{	
			get{ return _SendOrdersEmail; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SendOrdersEmail, value, _SendOrdersEmail);
				if (PropertyChanging(args))
				{
					_SendOrdersEmail = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ThemeURL
		{	
			get{ return _ThemeURL; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ThemeURL, value, _ThemeURL);
				if (PropertyChanging(args))
				{
					_ThemeURL = value;
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
		public  RestaurantSiteConfigurationBase Clone()
		{
			RestaurantSiteConfigurationBase newObj = new  RestaurantSiteConfigurationBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.SiteName = this.SiteName;						
			newObj.DomainName = this.DomainName;						
			newObj.StorePhone = this.StorePhone;						
			newObj.SendOrdersEmail = this.SendOrdersEmail;						
			newObj.ThemeURL = this.ThemeURL;						
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
			info.AddValue(RestaurantSiteConfigurationBase.Property_Id, Id);				
			info.AddValue(RestaurantSiteConfigurationBase.Property_CompanyId, CompanyId);				
			info.AddValue(RestaurantSiteConfigurationBase.Property_SiteName, SiteName);				
			info.AddValue(RestaurantSiteConfigurationBase.Property_DomainName, DomainName);				
			info.AddValue(RestaurantSiteConfigurationBase.Property_StorePhone, StorePhone);				
			info.AddValue(RestaurantSiteConfigurationBase.Property_SendOrdersEmail, SendOrdersEmail);				
			info.AddValue(RestaurantSiteConfigurationBase.Property_ThemeURL, ThemeURL);				
			info.AddValue(RestaurantSiteConfigurationBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(RestaurantSiteConfigurationBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(RestaurantSiteConfigurationBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(RestaurantSiteConfigurationBase.Property_LastUpdatedDate, LastUpdatedDate);				
		}
		#endregion

		
	}
}
