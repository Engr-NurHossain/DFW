using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "ToppingCategoryBase", Namespace = "http://www.piistech.com//entities")]
	public class ToppingCategoryBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			ToppingCategory = 1,
			CompanyId = 2
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_ToppingCategory = "ToppingCategory";		            
		public const string Property_CompanyId = "CompanyId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _ToppingCategory;	            
		private Guid _CompanyId;	            
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

		#endregion
		
		#region Cloning Base Objects
		public  ToppingCategoryBase Clone()
		{
			ToppingCategoryBase newObj = new  ToppingCategoryBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.ToppingCategory = this.ToppingCategory;						
			newObj.CompanyId = this.CompanyId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(ToppingCategoryBase.Property_Id, Id);				
			info.AddValue(ToppingCategoryBase.Property_ToppingCategory, ToppingCategory);				
			info.AddValue(ToppingCategoryBase.Property_CompanyId, CompanyId);				
		}
		#endregion

		
	}
}
