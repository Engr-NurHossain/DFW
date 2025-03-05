using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "RestCategoryBase", Namespace = "http://www.hims-tech.com//entities")]
	public class RestCategoryBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CategoryId = 1,
			CategoryName = 2,
			Description = 3,
			DaysAvailable = 4,
			TimeAvailable = 5,
			Status = 6,
			Image = 7,
			CreatedDate = 8,
			CreatedBy = 9,
			LastUpdatedBy = 10,
			LastUpdatedDate = 11,
			CompanyId = 12,
			UrlSlug = 13,
			DaysAvailableOption = 14,
			TimeAvailableOption = 15,
			WebsiteURL = 16,
			MetaTitle = 17,
			MetaDescription = 18,
			OrderBy = 19
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CategoryId = "CategoryId";		            
		public const string Property_CategoryName = "CategoryName";		            
		public const string Property_Description = "Description";		            
		public const string Property_DaysAvailable = "DaysAvailable";		            
		public const string Property_TimeAvailable = "TimeAvailable";		            
		public const string Property_Status = "Status";		            
		public const string Property_Image = "Image";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_UrlSlug = "UrlSlug";		            
		public const string Property_DaysAvailableOption = "DaysAvailableOption";		            
		public const string Property_TimeAvailableOption = "TimeAvailableOption";		            
		public const string Property_WebsiteURL = "WebsiteURL";		            
		public const string Property_MetaTitle = "MetaTitle";		            
		public const string Property_MetaDescription = "MetaDescription";		            
		public const string Property_OrderBy = "OrderBy";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CategoryId;	            
		private String _CategoryName;	            
		private String _Description;	            
		private String _DaysAvailable;	            
		private String _TimeAvailable;	            
		private Nullable<Boolean> _Status;	            
		private String _Image;	            
		private DateTime _CreatedDate;	            
		private Guid _CreatedBy;	            
		private Guid _LastUpdatedBy;	            
		private DateTime _LastUpdatedDate;	            
		private Guid _CompanyId;	            
		private String _UrlSlug;	            
		private String _DaysAvailableOption;	            
		private String _TimeAvailableOption;	            
		private String _WebsiteURL;	            
		private String _MetaTitle;	            
		private String _MetaDescription;	            
		private Nullable<Int32> _OrderBy;	            
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
		public Guid CategoryId
		{	
			get{ return _CategoryId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CategoryId, value, _CategoryId);
				if (PropertyChanging(args))
				{
					_CategoryId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CategoryName
		{	
			get{ return _CategoryName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CategoryName, value, _CategoryName);
				if (PropertyChanging(args))
				{
					_CategoryName = value;
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
		public String Image
		{	
			get{ return _Image; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Image, value, _Image);
				if (PropertyChanging(args))
				{
					_Image = value;
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

		[DataMember]
		public Nullable<Int32> OrderBy
		{	
			get{ return _OrderBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OrderBy, value, _OrderBy);
				if (PropertyChanging(args))
				{
					_OrderBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  RestCategoryBase Clone()
		{
			RestCategoryBase newObj = new  RestCategoryBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CategoryId = this.CategoryId;						
			newObj.CategoryName = this.CategoryName;						
			newObj.Description = this.Description;						
			newObj.DaysAvailable = this.DaysAvailable;						
			newObj.TimeAvailable = this.TimeAvailable;						
			newObj.Status = this.Status;						
			newObj.Image = this.Image;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.CompanyId = this.CompanyId;						
			newObj.UrlSlug = this.UrlSlug;						
			newObj.DaysAvailableOption = this.DaysAvailableOption;						
			newObj.TimeAvailableOption = this.TimeAvailableOption;						
			newObj.WebsiteURL = this.WebsiteURL;						
			newObj.MetaTitle = this.MetaTitle;						
			newObj.MetaDescription = this.MetaDescription;						
			newObj.OrderBy = this.OrderBy;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(RestCategoryBase.Property_Id, Id);				
			info.AddValue(RestCategoryBase.Property_CategoryId, CategoryId);				
			info.AddValue(RestCategoryBase.Property_CategoryName, CategoryName);				
			info.AddValue(RestCategoryBase.Property_Description, Description);				
			info.AddValue(RestCategoryBase.Property_DaysAvailable, DaysAvailable);				
			info.AddValue(RestCategoryBase.Property_TimeAvailable, TimeAvailable);				
			info.AddValue(RestCategoryBase.Property_Status, Status);				
			info.AddValue(RestCategoryBase.Property_Image, Image);				
			info.AddValue(RestCategoryBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(RestCategoryBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(RestCategoryBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(RestCategoryBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(RestCategoryBase.Property_CompanyId, CompanyId);				
			info.AddValue(RestCategoryBase.Property_UrlSlug, UrlSlug);				
			info.AddValue(RestCategoryBase.Property_DaysAvailableOption, DaysAvailableOption);				
			info.AddValue(RestCategoryBase.Property_TimeAvailableOption, TimeAvailableOption);				
			info.AddValue(RestCategoryBase.Property_WebsiteURL, WebsiteURL);				
			info.AddValue(RestCategoryBase.Property_MetaTitle, MetaTitle);				
			info.AddValue(RestCategoryBase.Property_MetaDescription, MetaDescription);				
			info.AddValue(RestCategoryBase.Property_OrderBy, OrderBy);				
		}
		#endregion

		
	}
}
