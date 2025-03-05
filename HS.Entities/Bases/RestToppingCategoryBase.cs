using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "RestToppingCategoryBase", Namespace = "http://www.hims-tech.com//entities")]
	public class RestToppingCategoryBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			ToppingCategoryId = 1,
			ToppingCategory = 2,
			CompanyId = 3,
			RequiredItem = 4
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_ToppingCategoryId = "ToppingCategoryId";		            
		public const string Property_ToppingCategory = "ToppingCategory";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_RequiredItem = "RequiredItem";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _ToppingCategoryId;	            
		private String _ToppingCategory;	            
		private Guid _CompanyId;	            
		private Nullable<Int32> _RequiredItem;	            
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
		public Guid ToppingCategoryId
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

		[DataMember]
		public String ToppingCategory
		{	
			get{ return _ToppingCategory; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ToppingCategory, value, _ToppingCategory);
				if (PropertyChanging(args))
				{
					_ToppingCategory = value;
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
		public Nullable<Int32> RequiredItem
		{	
			get{ return _RequiredItem; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RequiredItem, value, _RequiredItem);
				if (PropertyChanging(args))
				{
					_RequiredItem = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  RestToppingCategoryBase Clone()
		{
			RestToppingCategoryBase newObj = new  RestToppingCategoryBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.ToppingCategoryId = this.ToppingCategoryId;						
			newObj.ToppingCategory = this.ToppingCategory;						
			newObj.CompanyId = this.CompanyId;						
			newObj.RequiredItem = this.RequiredItem;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(RestToppingCategoryBase.Property_Id, Id);				
			info.AddValue(RestToppingCategoryBase.Property_ToppingCategoryId, ToppingCategoryId);				
			info.AddValue(RestToppingCategoryBase.Property_ToppingCategory, ToppingCategory);				
			info.AddValue(RestToppingCategoryBase.Property_CompanyId, CompanyId);				
			info.AddValue(RestToppingCategoryBase.Property_RequiredItem, RequiredItem);				
		}
		#endregion

		
	}
}
