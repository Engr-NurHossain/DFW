using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "MenuItemCategoryURLBase", Namespace = "http://www.hims-tech.com//entities")]
	public class MenuItemCategoryURLBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			MenuId = 1,
			MenuItemId = 2,
			MenuCategoryURL = 3,
			ItemCategoryURL = 4,
			CreatedDate = 5,
			CreatedBy = 6
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_MenuId = "MenuId";		            
		public const string Property_MenuItemId = "MenuItemId";		            
		public const string Property_MenuCategoryURL = "MenuCategoryURL";		            
		public const string Property_ItemCategoryURL = "ItemCategoryURL";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Int32 _MenuId;	            
		private Int32 _MenuItemId;	            
		private String _MenuCategoryURL;	            
		private String _ItemCategoryURL;	            
		private DateTime _CreatedDate;	            
		private Guid _CreatedBy;	            
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
		public Int32 MenuId
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
		public Int32 MenuItemId
		{	
			get{ return _MenuItemId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MenuItemId, value, _MenuItemId);
				if (PropertyChanging(args))
				{
					_MenuItemId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String MenuCategoryURL
		{	
			get{ return _MenuCategoryURL; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MenuCategoryURL, value, _MenuCategoryURL);
				if (PropertyChanging(args))
				{
					_MenuCategoryURL = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ItemCategoryURL
		{	
			get{ return _ItemCategoryURL; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ItemCategoryURL, value, _ItemCategoryURL);
				if (PropertyChanging(args))
				{
					_ItemCategoryURL = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  MenuItemCategoryURLBase Clone()
		{
			MenuItemCategoryURLBase newObj = new  MenuItemCategoryURLBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.MenuId = this.MenuId;						
			newObj.MenuItemId = this.MenuItemId;						
			newObj.MenuCategoryURL = this.MenuCategoryURL;						
			newObj.ItemCategoryURL = this.ItemCategoryURL;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(MenuItemCategoryURLBase.Property_Id, Id);				
			info.AddValue(MenuItemCategoryURLBase.Property_MenuId, MenuId);				
			info.AddValue(MenuItemCategoryURLBase.Property_MenuItemId, MenuItemId);				
			info.AddValue(MenuItemCategoryURLBase.Property_MenuCategoryURL, MenuCategoryURL);				
			info.AddValue(MenuItemCategoryURLBase.Property_ItemCategoryURL, ItemCategoryURL);				
			info.AddValue(MenuItemCategoryURLBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(MenuItemCategoryURLBase.Property_CreatedBy, CreatedBy);				
		}
		#endregion

		
	}
}
