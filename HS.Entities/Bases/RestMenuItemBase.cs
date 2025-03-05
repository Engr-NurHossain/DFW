using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "RestMenuItemBase", Namespace = "http://www.hims-tech.com//entities")]
	public class RestMenuItemBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			ItemId = 1,
			ItemName = 2,
			ItemNumber = 3,
			ItemLevel = 4,
			Description = 5,
			Photo = 6,
			MaxQty = 7,
			DaysAvailable = 8,
			TimeAvailable = 9,
			Price = 10,
			Status = 11,
			DaysAvailableOption = 12,
			TimeAvailableOption = 13,
			CompanyId = 14,
			UrlSlug = 15,
			WebsiteURL = 16,
			MetaTitle = 17,
			MetaDescription = 18,
			DeliveryTime = 19,
			IsTax = 20,
			TaxPercentage = 21,
			OrderBy = 22,
			IsInstruction = 23,
			CreatedBy = 24,
			CreatedDate = 25,
			LastUpdatedBy = 26,
			LastUpdatedDate = 27
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_ItemId = "ItemId";		            
		public const string Property_ItemName = "ItemName";		            
		public const string Property_ItemNumber = "ItemNumber";		            
		public const string Property_ItemLevel = "ItemLevel";		            
		public const string Property_Description = "Description";		            
		public const string Property_Photo = "Photo";		            
		public const string Property_MaxQty = "MaxQty";		            
		public const string Property_DaysAvailable = "DaysAvailable";		            
		public const string Property_TimeAvailable = "TimeAvailable";		            
		public const string Property_Price = "Price";		            
		public const string Property_Status = "Status";		            
		public const string Property_DaysAvailableOption = "DaysAvailableOption";		            
		public const string Property_TimeAvailableOption = "TimeAvailableOption";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_UrlSlug = "UrlSlug";		            
		public const string Property_WebsiteURL = "WebsiteURL";		            
		public const string Property_MetaTitle = "MetaTitle";		            
		public const string Property_MetaDescription = "MetaDescription";		            
		public const string Property_DeliveryTime = "DeliveryTime";		            
		public const string Property_IsTax = "IsTax";		            
		public const string Property_TaxPercentage = "TaxPercentage";		            
		public const string Property_OrderBy = "OrderBy";		            
		public const string Property_IsInstruction = "IsInstruction";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _ItemId;	            
		private String _ItemName;	            
		private String _ItemNumber;	            
		private String _ItemLevel;	            
		private String _Description;	            
		private String _Photo;	            
		private Nullable<Int32> _MaxQty;	            
		private String _DaysAvailable;	            
		private String _TimeAvailable;	            
		private Nullable<Double> _Price;	            
		private Nullable<Boolean> _Status;	            
		private String _DaysAvailableOption;	            
		private String _TimeAvailableOption;	            
		private Guid _CompanyId;	            
		private String _UrlSlug;	            
		private String _WebsiteURL;	            
		private String _MetaTitle;	            
		private String _MetaDescription;	            
		private Nullable<Int32> _DeliveryTime;	            
		private Nullable<Boolean> _IsTax;	            
		private Nullable<Double> _TaxPercentage;	            
		private Nullable<Int32> _OrderBy;	            
		private Nullable<Boolean> _IsInstruction;	            
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
		public Guid ItemId
		{	
			get{ return _ItemId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ItemId, value, _ItemId);
				if (PropertyChanging(args))
				{
					_ItemId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ItemName
		{	
			get{ return _ItemName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ItemName, value, _ItemName);
				if (PropertyChanging(args))
				{
					_ItemName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ItemNumber
		{	
			get{ return _ItemNumber; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ItemNumber, value, _ItemNumber);
				if (PropertyChanging(args))
				{
					_ItemNumber = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ItemLevel
		{	
			get{ return _ItemLevel; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ItemLevel, value, _ItemLevel);
				if (PropertyChanging(args))
				{
					_ItemLevel = value;
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
		public Nullable<Int32> MaxQty
		{	
			get{ return _MaxQty; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MaxQty, value, _MaxQty);
				if (PropertyChanging(args))
				{
					_MaxQty = value;
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
		public Nullable<Double> Price
		{	
			get{ return _Price; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Price, value, _Price);
				if (PropertyChanging(args))
				{
					_Price = value;
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
		public Nullable<Int32> DeliveryTime
		{	
			get{ return _DeliveryTime; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DeliveryTime, value, _DeliveryTime);
				if (PropertyChanging(args))
				{
					_DeliveryTime = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsTax
		{	
			get{ return _IsTax; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsTax, value, _IsTax);
				if (PropertyChanging(args))
				{
					_IsTax = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> TaxPercentage
		{	
			get{ return _TaxPercentage; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TaxPercentage, value, _TaxPercentage);
				if (PropertyChanging(args))
				{
					_TaxPercentage = value;
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

		[DataMember]
		public Nullable<Boolean> IsInstruction
		{	
			get{ return _IsInstruction; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsInstruction, value, _IsInstruction);
				if (PropertyChanging(args))
				{
					_IsInstruction = value;
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
		public  RestMenuItemBase Clone()
		{
			RestMenuItemBase newObj = new  RestMenuItemBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.ItemId = this.ItemId;						
			newObj.ItemName = this.ItemName;						
			newObj.ItemNumber = this.ItemNumber;						
			newObj.ItemLevel = this.ItemLevel;						
			newObj.Description = this.Description;						
			newObj.Photo = this.Photo;						
			newObj.MaxQty = this.MaxQty;						
			newObj.DaysAvailable = this.DaysAvailable;						
			newObj.TimeAvailable = this.TimeAvailable;						
			newObj.Price = this.Price;						
			newObj.Status = this.Status;						
			newObj.DaysAvailableOption = this.DaysAvailableOption;						
			newObj.TimeAvailableOption = this.TimeAvailableOption;						
			newObj.CompanyId = this.CompanyId;						
			newObj.UrlSlug = this.UrlSlug;						
			newObj.WebsiteURL = this.WebsiteURL;						
			newObj.MetaTitle = this.MetaTitle;						
			newObj.MetaDescription = this.MetaDescription;						
			newObj.DeliveryTime = this.DeliveryTime;						
			newObj.IsTax = this.IsTax;						
			newObj.TaxPercentage = this.TaxPercentage;						
			newObj.OrderBy = this.OrderBy;						
			newObj.IsInstruction = this.IsInstruction;						
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
			info.AddValue(RestMenuItemBase.Property_Id, Id);				
			info.AddValue(RestMenuItemBase.Property_ItemId, ItemId);				
			info.AddValue(RestMenuItemBase.Property_ItemName, ItemName);				
			info.AddValue(RestMenuItemBase.Property_ItemNumber, ItemNumber);				
			info.AddValue(RestMenuItemBase.Property_ItemLevel, ItemLevel);				
			info.AddValue(RestMenuItemBase.Property_Description, Description);				
			info.AddValue(RestMenuItemBase.Property_Photo, Photo);				
			info.AddValue(RestMenuItemBase.Property_MaxQty, MaxQty);				
			info.AddValue(RestMenuItemBase.Property_DaysAvailable, DaysAvailable);				
			info.AddValue(RestMenuItemBase.Property_TimeAvailable, TimeAvailable);				
			info.AddValue(RestMenuItemBase.Property_Price, Price);				
			info.AddValue(RestMenuItemBase.Property_Status, Status);				
			info.AddValue(RestMenuItemBase.Property_DaysAvailableOption, DaysAvailableOption);				
			info.AddValue(RestMenuItemBase.Property_TimeAvailableOption, TimeAvailableOption);				
			info.AddValue(RestMenuItemBase.Property_CompanyId, CompanyId);				
			info.AddValue(RestMenuItemBase.Property_UrlSlug, UrlSlug);				
			info.AddValue(RestMenuItemBase.Property_WebsiteURL, WebsiteURL);				
			info.AddValue(RestMenuItemBase.Property_MetaTitle, MetaTitle);				
			info.AddValue(RestMenuItemBase.Property_MetaDescription, MetaDescription);				
			info.AddValue(RestMenuItemBase.Property_DeliveryTime, DeliveryTime);				
			info.AddValue(RestMenuItemBase.Property_IsTax, IsTax);				
			info.AddValue(RestMenuItemBase.Property_TaxPercentage, TaxPercentage);				
			info.AddValue(RestMenuItemBase.Property_OrderBy, OrderBy);				
			info.AddValue(RestMenuItemBase.Property_IsInstruction, IsInstruction);				
			info.AddValue(RestMenuItemBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(RestMenuItemBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(RestMenuItemBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(RestMenuItemBase.Property_LastUpdatedDate, LastUpdatedDate);				
		}
		#endregion

		
	}
}
