using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "SeoBase", Namespace = "http://www.hims-tech.com//entities")]
	public class SeoBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			PageUrl = 2,
			MetaTitle = 3,
			MetaDescription = 4,
			MetaKeywords = 5,
			OgTitle = 6,
			OgType = 7,
			OgImage = 8,
			OgUrl = 9,
			OgDescription = 10,
			TwitterCard = 11,
			TwitterUrl = 12,
			TwitterTitle = 13,
			TwitterDescription = 14,
			TwitterImage = 15,
			ItemScopePageType = 16,
			ItemPropName = 17,
			ItemPropTitle = 18,
			ItemPropDescription = 19,
			ItemPropImage = 20,
			IsActive = 21,
			Name = 22,
			IsFolder = 23,
			FolderOption = 24,
			IsNav = 25,
			PublishOption = 26
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_PageUrl = "PageUrl";		            
		public const string Property_MetaTitle = "MetaTitle";		            
		public const string Property_MetaDescription = "MetaDescription";		            
		public const string Property_MetaKeywords = "MetaKeywords";		            
		public const string Property_OgTitle = "OgTitle";		            
		public const string Property_OgType = "OgType";		            
		public const string Property_OgImage = "OgImage";		            
		public const string Property_OgUrl = "OgUrl";		            
		public const string Property_OgDescription = "OgDescription";		            
		public const string Property_TwitterCard = "TwitterCard";		            
		public const string Property_TwitterUrl = "TwitterUrl";		            
		public const string Property_TwitterTitle = "TwitterTitle";		            
		public const string Property_TwitterDescription = "TwitterDescription";		            
		public const string Property_TwitterImage = "TwitterImage";		            
		public const string Property_ItemScopePageType = "ItemScopePageType";		            
		public const string Property_ItemPropName = "ItemPropName";		            
		public const string Property_ItemPropTitle = "ItemPropTitle";		            
		public const string Property_ItemPropDescription = "ItemPropDescription";		            
		public const string Property_ItemPropImage = "ItemPropImage";		            
		public const string Property_IsActive = "IsActive";		            
		public const string Property_Name = "Name";		            
		public const string Property_IsFolder = "IsFolder";		            
		public const string Property_FolderOption = "FolderOption";		            
		public const string Property_IsNav = "IsNav";		            
		public const string Property_PublishOption = "PublishOption";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private String _PageUrl;	            
		private String _MetaTitle;	            
		private String _MetaDescription;	            
		private String _MetaKeywords;	            
		private String _OgTitle;	            
		private String _OgType;	            
		private String _OgImage;	            
		private String _OgUrl;	            
		private String _OgDescription;	            
		private String _TwitterCard;	            
		private String _TwitterUrl;	            
		private String _TwitterTitle;	            
		private String _TwitterDescription;	            
		private String _TwitterImage;	            
		private String _ItemScopePageType;	            
		private String _ItemPropName;	            
		private String _ItemPropTitle;	            
		private String _ItemPropDescription;	            
		private String _ItemPropImage;	            
		private Nullable<Boolean> _IsActive;	            
		private String _Name;	            
		private Nullable<Boolean> _IsFolder;	            
		private String _FolderOption;	            
		private Nullable<Boolean> _IsNav;	            
		private String _PublishOption;	            
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
		public String PageUrl
		{	
			get{ return _PageUrl; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PageUrl, value, _PageUrl);
				if (PropertyChanging(args))
				{
					_PageUrl = value;
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
		public String MetaKeywords
		{	
			get{ return _MetaKeywords; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MetaKeywords, value, _MetaKeywords);
				if (PropertyChanging(args))
				{
					_MetaKeywords = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String OgTitle
		{	
			get{ return _OgTitle; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OgTitle, value, _OgTitle);
				if (PropertyChanging(args))
				{
					_OgTitle = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String OgType
		{	
			get{ return _OgType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OgType, value, _OgType);
				if (PropertyChanging(args))
				{
					_OgType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String OgImage
		{	
			get{ return _OgImage; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OgImage, value, _OgImage);
				if (PropertyChanging(args))
				{
					_OgImage = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String OgUrl
		{	
			get{ return _OgUrl; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OgUrl, value, _OgUrl);
				if (PropertyChanging(args))
				{
					_OgUrl = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String OgDescription
		{	
			get{ return _OgDescription; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OgDescription, value, _OgDescription);
				if (PropertyChanging(args))
				{
					_OgDescription = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TwitterCard
		{	
			get{ return _TwitterCard; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TwitterCard, value, _TwitterCard);
				if (PropertyChanging(args))
				{
					_TwitterCard = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TwitterUrl
		{	
			get{ return _TwitterUrl; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TwitterUrl, value, _TwitterUrl);
				if (PropertyChanging(args))
				{
					_TwitterUrl = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TwitterTitle
		{	
			get{ return _TwitterTitle; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TwitterTitle, value, _TwitterTitle);
				if (PropertyChanging(args))
				{
					_TwitterTitle = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TwitterDescription
		{	
			get{ return _TwitterDescription; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TwitterDescription, value, _TwitterDescription);
				if (PropertyChanging(args))
				{
					_TwitterDescription = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TwitterImage
		{	
			get{ return _TwitterImage; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TwitterImage, value, _TwitterImage);
				if (PropertyChanging(args))
				{
					_TwitterImage = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ItemScopePageType
		{	
			get{ return _ItemScopePageType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ItemScopePageType, value, _ItemScopePageType);
				if (PropertyChanging(args))
				{
					_ItemScopePageType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ItemPropName
		{	
			get{ return _ItemPropName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ItemPropName, value, _ItemPropName);
				if (PropertyChanging(args))
				{
					_ItemPropName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ItemPropTitle
		{	
			get{ return _ItemPropTitle; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ItemPropTitle, value, _ItemPropTitle);
				if (PropertyChanging(args))
				{
					_ItemPropTitle = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ItemPropDescription
		{	
			get{ return _ItemPropDescription; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ItemPropDescription, value, _ItemPropDescription);
				if (PropertyChanging(args))
				{
					_ItemPropDescription = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ItemPropImage
		{	
			get{ return _ItemPropImage; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ItemPropImage, value, _ItemPropImage);
				if (PropertyChanging(args))
				{
					_ItemPropImage = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsActive
		{	
			get{ return _IsActive; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsActive, value, _IsActive);
				if (PropertyChanging(args))
				{
					_IsActive = value;
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
		public Nullable<Boolean> IsFolder
		{	
			get{ return _IsFolder; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsFolder, value, _IsFolder);
				if (PropertyChanging(args))
				{
					_IsFolder = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String FolderOption
		{	
			get{ return _FolderOption; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FolderOption, value, _FolderOption);
				if (PropertyChanging(args))
				{
					_FolderOption = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsNav
		{	
			get{ return _IsNav; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsNav, value, _IsNav);
				if (PropertyChanging(args))
				{
					_IsNav = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PublishOption
		{	
			get{ return _PublishOption; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PublishOption, value, _PublishOption);
				if (PropertyChanging(args))
				{
					_PublishOption = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  SeoBase Clone()
		{
			SeoBase newObj = new  SeoBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.PageUrl = this.PageUrl;						
			newObj.MetaTitle = this.MetaTitle;						
			newObj.MetaDescription = this.MetaDescription;						
			newObj.MetaKeywords = this.MetaKeywords;						
			newObj.OgTitle = this.OgTitle;						
			newObj.OgType = this.OgType;						
			newObj.OgImage = this.OgImage;						
			newObj.OgUrl = this.OgUrl;						
			newObj.OgDescription = this.OgDescription;						
			newObj.TwitterCard = this.TwitterCard;						
			newObj.TwitterUrl = this.TwitterUrl;						
			newObj.TwitterTitle = this.TwitterTitle;						
			newObj.TwitterDescription = this.TwitterDescription;						
			newObj.TwitterImage = this.TwitterImage;						
			newObj.ItemScopePageType = this.ItemScopePageType;						
			newObj.ItemPropName = this.ItemPropName;						
			newObj.ItemPropTitle = this.ItemPropTitle;						
			newObj.ItemPropDescription = this.ItemPropDescription;						
			newObj.ItemPropImage = this.ItemPropImage;						
			newObj.IsActive = this.IsActive;						
			newObj.Name = this.Name;						
			newObj.IsFolder = this.IsFolder;						
			newObj.FolderOption = this.FolderOption;						
			newObj.IsNav = this.IsNav;						
			newObj.PublishOption = this.PublishOption;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(SeoBase.Property_Id, Id);				
			info.AddValue(SeoBase.Property_CompanyId, CompanyId);				
			info.AddValue(SeoBase.Property_PageUrl, PageUrl);				
			info.AddValue(SeoBase.Property_MetaTitle, MetaTitle);				
			info.AddValue(SeoBase.Property_MetaDescription, MetaDescription);				
			info.AddValue(SeoBase.Property_MetaKeywords, MetaKeywords);				
			info.AddValue(SeoBase.Property_OgTitle, OgTitle);				
			info.AddValue(SeoBase.Property_OgType, OgType);				
			info.AddValue(SeoBase.Property_OgImage, OgImage);				
			info.AddValue(SeoBase.Property_OgUrl, OgUrl);				
			info.AddValue(SeoBase.Property_OgDescription, OgDescription);				
			info.AddValue(SeoBase.Property_TwitterCard, TwitterCard);				
			info.AddValue(SeoBase.Property_TwitterUrl, TwitterUrl);				
			info.AddValue(SeoBase.Property_TwitterTitle, TwitterTitle);				
			info.AddValue(SeoBase.Property_TwitterDescription, TwitterDescription);				
			info.AddValue(SeoBase.Property_TwitterImage, TwitterImage);				
			info.AddValue(SeoBase.Property_ItemScopePageType, ItemScopePageType);				
			info.AddValue(SeoBase.Property_ItemPropName, ItemPropName);				
			info.AddValue(SeoBase.Property_ItemPropTitle, ItemPropTitle);				
			info.AddValue(SeoBase.Property_ItemPropDescription, ItemPropDescription);				
			info.AddValue(SeoBase.Property_ItemPropImage, ItemPropImage);				
			info.AddValue(SeoBase.Property_IsActive, IsActive);				
			info.AddValue(SeoBase.Property_Name, Name);				
			info.AddValue(SeoBase.Property_IsFolder, IsFolder);				
			info.AddValue(SeoBase.Property_FolderOption, FolderOption);				
			info.AddValue(SeoBase.Property_IsNav, IsNav);				
			info.AddValue(SeoBase.Property_PublishOption, PublishOption);				
		}
		#endregion

		
	}
}
