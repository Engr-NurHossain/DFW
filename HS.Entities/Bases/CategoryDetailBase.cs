using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CategoryDetailBase", Namespace = "http://www.piistech.com//entities")]
	public class CategoryDetailBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			MenuId = 1,
			CategoryId = 2
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_MenuId = "MenuId";		            
		public const string Property_CategoryId = "CategoryId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Nullable<Int32> _MenuId;	            
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
		public  CategoryDetailBase Clone()
		{
			CategoryDetailBase newObj = new  CategoryDetailBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.MenuId = this.MenuId;						
			newObj.CategoryId = this.CategoryId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CategoryDetailBase.Property_Id, Id);				
			info.AddValue(CategoryDetailBase.Property_MenuId, MenuId);				
			info.AddValue(CategoryDetailBase.Property_CategoryId, CategoryId);				
		}
		#endregion

		
	}
}
