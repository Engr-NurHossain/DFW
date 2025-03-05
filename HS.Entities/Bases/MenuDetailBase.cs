using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "MenuDetailBase", Namespace = "http://www.piistech.com//entities")]
	public class MenuDetailBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			MenuId = 1,
			ToppingCategoryId = 2
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_MenuId = "MenuId";		            
		public const string Property_ToppingCategoryId = "ToppingCategoryId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Nullable<Int32> _MenuId;	            
		private Nullable<Int32> _ToppingCategoryId;	            
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
		public Nullable<Int32> ToppingCategoryId
		{	
			get{ return _ToppingCategoryId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ToppingCategoryId, value, _ToppingCategoryId);
				if (PropertyChanging(args))
				{
					_ToppingCategoryId = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  MenuDetailBase Clone()
		{
			MenuDetailBase newObj = new  MenuDetailBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.MenuId = this.MenuId;						
			newObj.ToppingCategoryId = this.ToppingCategoryId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(MenuDetailBase.Property_Id, Id);				
			info.AddValue(MenuDetailBase.Property_MenuId, MenuId);				
			info.AddValue(MenuDetailBase.Property_ToppingCategoryId, ToppingCategoryId);				
		}
		#endregion

		
	}
}
