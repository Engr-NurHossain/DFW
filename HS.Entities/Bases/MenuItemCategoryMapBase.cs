using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "MenuItemCategoryMapBase", Namespace = "http://www.hims-tech.com//entities")]
	public class MenuItemCategoryMapBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			MenuId = 2,
			CategoryId = 3,
			ItemId = 4,
			CreatedDate = 5,
			CreatedBy = 6
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_MenuId = "MenuId";		            
		public const string Property_CategoryId = "CategoryId";		            
		public const string Property_ItemId = "ItemId";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _MenuId;	            
		private Guid _CategoryId;	            
		private Guid _ItemId;	            
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
		public  MenuItemCategoryMapBase Clone()
		{
			MenuItemCategoryMapBase newObj = new  MenuItemCategoryMapBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.MenuId = this.MenuId;						
			newObj.CategoryId = this.CategoryId;						
			newObj.ItemId = this.ItemId;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(MenuItemCategoryMapBase.Property_Id, Id);				
			info.AddValue(MenuItemCategoryMapBase.Property_CompanyId, CompanyId);				
			info.AddValue(MenuItemCategoryMapBase.Property_MenuId, MenuId);				
			info.AddValue(MenuItemCategoryMapBase.Property_CategoryId, CategoryId);				
			info.AddValue(MenuItemCategoryMapBase.Property_ItemId, ItemId);				
			info.AddValue(MenuItemCategoryMapBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(MenuItemCategoryMapBase.Property_CreatedBy, CreatedBy);				
		}
		#endregion

		
	}
}
