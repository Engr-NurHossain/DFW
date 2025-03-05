using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "RestaurantContentBase", Namespace = "http://www.hims-tech.com//entities")]
	public class RestaurantContentBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			PageName = 2,
			AnchorText = 3,
			MetaTitle = 4,
			MetaDescription = 5,
			IsPublish = 6,
			IsNavigation = 7,
			FolderName = 8,
			ContentURL = 9,
			CreatedBy = 10,
			CreatedDate = 11,
			LastUpdatedBy = 12,
			LastUpdatedDate = 13
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_PageName = "PageName";		            
		public const string Property_AnchorText = "AnchorText";		            
		public const string Property_MetaTitle = "MetaTitle";		            
		public const string Property_MetaDescription = "MetaDescription";		            
		public const string Property_IsPublish = "IsPublish";		            
		public const string Property_IsNavigation = "IsNavigation";		            
		public const string Property_FolderName = "FolderName";		            
		public const string Property_ContentURL = "ContentURL";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private String _PageName;	            
		private String _AnchorText;	            
		private String _MetaTitle;	            
		private String _MetaDescription;	            
		private Nullable<Boolean> _IsPublish;	            
		private Nullable<Boolean> _IsNavigation;	            
		private String _FolderName;	            
		private String _ContentURL;	            
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
		public String PageName
		{	
			get{ return _PageName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PageName, value, _PageName);
				if (PropertyChanging(args))
				{
					_PageName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AnchorText
		{	
			get{ return _AnchorText; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AnchorText, value, _AnchorText);
				if (PropertyChanging(args))
				{
					_AnchorText = value;
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
		public Nullable<Boolean> IsPublish
		{	
			get{ return _IsPublish; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsPublish, value, _IsPublish);
				if (PropertyChanging(args))
				{
					_IsPublish = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsNavigation
		{	
			get{ return _IsNavigation; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsNavigation, value, _IsNavigation);
				if (PropertyChanging(args))
				{
					_IsNavigation = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String FolderName
		{	
			get{ return _FolderName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FolderName, value, _FolderName);
				if (PropertyChanging(args))
				{
					_FolderName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ContentURL
		{	
			get{ return _ContentURL; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ContentURL, value, _ContentURL);
				if (PropertyChanging(args))
				{
					_ContentURL = value;
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
		public  RestaurantContentBase Clone()
		{
			RestaurantContentBase newObj = new  RestaurantContentBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.PageName = this.PageName;						
			newObj.AnchorText = this.AnchorText;						
			newObj.MetaTitle = this.MetaTitle;						
			newObj.MetaDescription = this.MetaDescription;						
			newObj.IsPublish = this.IsPublish;						
			newObj.IsNavigation = this.IsNavigation;						
			newObj.FolderName = this.FolderName;						
			newObj.ContentURL = this.ContentURL;						
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
			info.AddValue(RestaurantContentBase.Property_Id, Id);				
			info.AddValue(RestaurantContentBase.Property_CompanyId, CompanyId);				
			info.AddValue(RestaurantContentBase.Property_PageName, PageName);				
			info.AddValue(RestaurantContentBase.Property_AnchorText, AnchorText);				
			info.AddValue(RestaurantContentBase.Property_MetaTitle, MetaTitle);				
			info.AddValue(RestaurantContentBase.Property_MetaDescription, MetaDescription);				
			info.AddValue(RestaurantContentBase.Property_IsPublish, IsPublish);				
			info.AddValue(RestaurantContentBase.Property_IsNavigation, IsNavigation);				
			info.AddValue(RestaurantContentBase.Property_FolderName, FolderName);				
			info.AddValue(RestaurantContentBase.Property_ContentURL, ContentURL);				
			info.AddValue(RestaurantContentBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(RestaurantContentBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(RestaurantContentBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(RestaurantContentBase.Property_LastUpdatedDate, LastUpdatedDate);				
		}
		#endregion

		
	}
}
