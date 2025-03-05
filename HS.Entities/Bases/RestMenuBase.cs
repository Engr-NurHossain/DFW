using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "RestMenuBase", Namespace = "http://www.hims-tech.com//entities")]
	public class RestMenuBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			MenuId = 2,
			MenuName = 3,
			Status = 4,
			TimeAvailable = 5,
			DaysAvailable = 6,
			Description = 7,
			Photo = 8,
			CreatedDate = 9,
			CreatedBy = 10,
			LastUpdatedBy = 11,
			LastUpdatedDate = 12,
			DaysAvailableOption = 13,
			TimeAvailableOption = 14,
			UrlSlug = 15,
			WebsiteURL = 16,
			MetaTitle = 17,
			MetaDescription = 18
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_MenuId = "MenuId";		            
		public const string Property_MenuName = "MenuName";		            
		public const string Property_Status = "Status";		            
		public const string Property_TimeAvailable = "TimeAvailable";		            
		public const string Property_DaysAvailable = "DaysAvailable";		            
		public const string Property_Description = "Description";		            
		public const string Property_Photo = "Photo";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_DaysAvailableOption = "DaysAvailableOption";		            
		public const string Property_TimeAvailableOption = "TimeAvailableOption";		            
		public const string Property_UrlSlug = "UrlSlug";		            
		public const string Property_WebsiteURL = "WebsiteURL";		            
		public const string Property_MetaTitle = "MetaTitle";		            
		public const string Property_MetaDescription = "MetaDescription";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _MenuId;	            
		private String _MenuName;	            
		private Nullable<Boolean> _Status;	            
		private String _TimeAvailable;	            
		private String _DaysAvailable;	            
		private String _Description;	            
		private String _Photo;	            
		private DateTime _CreatedDate;	            
		private Guid _CreatedBy;	            
		private Guid _LastUpdatedBy;	            
		private DateTime _LastUpdatedDate;	            
		private String _DaysAvailableOption;	            
		private String _TimeAvailableOption;	            
		private String _UrlSlug;	            
		private String _WebsiteURL;	            
		private String _MetaTitle;	            
		private String _MetaDescription;	            
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
		public Guid MenuId
		{	
			get{ return _MenuId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MenuId, value, _MenuId);
				if (PropertyChanging(args))
				{
					_MenuId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String MenuName
		{	
			get{ return _MenuName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MenuName, value, _MenuName);
				if (PropertyChanging(args))
				{
					_MenuName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> Status
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
		public String TimeAvailable
		{	
			get{ return _TimeAvailable; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TimeAvailable, value, _TimeAvailable);
				if (PropertyChanging(args))
				{
					_TimeAvailable = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String DaysAvailable
		{	
			get{ return _DaysAvailable; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DaysAvailable, value, _DaysAvailable);
				if (PropertyChanging(args))
				{
					_DaysAvailable = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Description
		{	
			get{ return _Description; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Description, value, _Description);
				if (PropertyChanging(args))
				{
					_Description = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Photo
		{	
			get{ return _Photo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Photo, value, _Photo);
				if (PropertyChanging(args))
				{
					_Photo = value;
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

		[DataMember]
		public String DaysAvailableOption
		{	
			get{ return _DaysAvailableOption; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DaysAvailableOption, value, _DaysAvailableOption);
				if (PropertyChanging(args))
				{
					_DaysAvailableOption = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TimeAvailableOption
		{	
			get{ return _TimeAvailableOption; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TimeAvailableOption, value, _TimeAvailableOption);
				if (PropertyChanging(args))
				{
					_TimeAvailableOption = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String UrlSlug
		{	
			get{ return _UrlSlug; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UrlSlug, value, _UrlSlug);
				if (PropertyChanging(args))
				{
					_UrlSlug = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String WebsiteURL
		{	
			get{ return _WebsiteURL; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_WebsiteURL, value, _WebsiteURL);
				if (PropertyChanging(args))
				{
					_WebsiteURL = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String MetaTitle
		{	
			get{ return _MetaTitle; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MetaTitle, value, _MetaTitle);
				if (PropertyChanging(args))
				{
					_MetaTitle = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String MetaDescription
		{	
			get{ return _MetaDescription; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MetaDescription, value, _MetaDescription);
				if (PropertyChanging(args))
				{
					_MetaDescription = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  RestMenuBase Clone()
		{
			RestMenuBase newObj = new  RestMenuBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.MenuId = this.MenuId;						
			newObj.MenuName = this.MenuName;						
			newObj.Status = this.Status;						
			newObj.TimeAvailable = this.TimeAvailable;						
			newObj.DaysAvailable = this.DaysAvailable;						
			newObj.Description = this.Description;						
			newObj.Photo = this.Photo;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.DaysAvailableOption = this.DaysAvailableOption;						
			newObj.TimeAvailableOption = this.TimeAvailableOption;						
			newObj.UrlSlug = this.UrlSlug;						
			newObj.WebsiteURL = this.WebsiteURL;						
			newObj.MetaTitle = this.MetaTitle;						
			newObj.MetaDescription = this.MetaDescription;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(RestMenuBase.Property_Id, Id);				
			info.AddValue(RestMenuBase.Property_CompanyId, CompanyId);				
			info.AddValue(RestMenuBase.Property_MenuId, MenuId);				
			info.AddValue(RestMenuBase.Property_MenuName, MenuName);				
			info.AddValue(RestMenuBase.Property_Status, Status);				
			info.AddValue(RestMenuBase.Property_TimeAvailable, TimeAvailable);				
			info.AddValue(RestMenuBase.Property_DaysAvailable, DaysAvailable);				
			info.AddValue(RestMenuBase.Property_Description, Description);				
			info.AddValue(RestMenuBase.Property_Photo, Photo);				
			info.AddValue(RestMenuBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(RestMenuBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(RestMenuBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(RestMenuBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(RestMenuBase.Property_DaysAvailableOption, DaysAvailableOption);				
			info.AddValue(RestMenuBase.Property_TimeAvailableOption, TimeAvailableOption);				
			info.AddValue(RestMenuBase.Property_UrlSlug, UrlSlug);				
			info.AddValue(RestMenuBase.Property_WebsiteURL, WebsiteURL);				
			info.AddValue(RestMenuBase.Property_MetaTitle, MetaTitle);				
			info.AddValue(RestMenuBase.Property_MetaDescription, MetaDescription);				
		}
		#endregion

		
	}
}
