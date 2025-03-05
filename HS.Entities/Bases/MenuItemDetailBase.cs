using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "MenuItemDetailBase", Namespace = "http://www.piistech.com//entities")]
	public class MenuItemDetailBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			MenuItemId = 1,
			MenuId = 2,
			ToppingId = 3,
			CategoryId = 4
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_MenuItemId = "MenuItemId";		            
		public const string Property_MenuId = "MenuId";		            
		public const string Property_ToppingId = "ToppingId";		            
		public const string Property_CategoryId = "CategoryId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Nullable<Int32> _MenuItemId;	            
		private Nullable<Int32> _MenuId;	            
		private Nullable<Int32> _ToppingId;	            
		private Nullable<Int32> _CategoryId;	            
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
		public Nullable<Int32> MenuItemId
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
		public Nullable<Int32> MenuId
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
		public Nullable<Int32> ToppingId
		{	
			get{ return _ToppingId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ToppingId, value, _ToppingId);
				if (PropertyChanging(args))
				{
					_ToppingId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> CategoryId
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

		#endregion
		
		#region Cloning Base Objects
		public  MenuItemDetailBase Clone()
		{
			MenuItemDetailBase newObj = new  MenuItemDetailBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.MenuItemId = this.MenuItemId;						
			newObj.MenuId = this.MenuId;						
			newObj.ToppingId = this.ToppingId;						
			newObj.CategoryId = this.CategoryId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(MenuItemDetailBase.Property_Id, Id);				
			info.AddValue(MenuItemDetailBase.Property_MenuItemId, MenuItemId);				
			info.AddValue(MenuItemDetailBase.Property_MenuId, MenuId);				
			info.AddValue(MenuItemDetailBase.Property_ToppingId, ToppingId);				
			info.AddValue(MenuItemDetailBase.Property_CategoryId, CategoryId);				
		}
		#endregion

		
	}
}
