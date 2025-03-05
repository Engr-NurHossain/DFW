using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "ToppingBase", Namespace = "http://www.hims-tech.com//entities")]
	public class ToppingBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			ToppingName = 1,
			Price = 2,
			IsAvailable = 3,
			CreatedDate = 4,
			CreatedBy = 5,
			LastUpdatedBy = 6,
			LastUpdatedDate = 7,
			ToppingCategoryId = 8,
			CompanyId = 9
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_ToppingName = "ToppingName";		            
		public const string Property_Price = "Price";		            
		public const string Property_IsAvailable = "IsAvailable";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_ToppingCategoryId = "ToppingCategoryId";		            
		public const string Property_CompanyId = "CompanyId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _ToppingName;	            
		private Nullable<Double> _Price;	            
		private Nullable<Boolean> _IsAvailable;	            
		private DateTime _CreatedDate;	            
		private Guid _CreatedBy;	            
		private Guid _LastUpdatedBy;	            
		private DateTime _LastUpdatedDate;	            
		private Nullable<Int32> _ToppingCategoryId;	            
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
		public String ToppingName
		{	
			get{ return _ToppingName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ToppingName, value, _ToppingName);
				if (PropertyChanging(args))
				{
					_ToppingName = value;
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
		public Nullable<Boolean> IsAvailable
		{	
			get{ return _IsAvailable; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsAvailable, value, _IsAvailable);
				if (PropertyChanging(args))
				{
					_IsAvailable = value;
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
		public  ToppingBase Clone()
		{
			ToppingBase newObj = new  ToppingBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.ToppingName = this.ToppingName;						
			newObj.Price = this.Price;						
			newObj.IsAvailable = this.IsAvailable;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.ToppingCategoryId = this.ToppingCategoryId;						
			newObj.CompanyId = this.CompanyId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(ToppingBase.Property_Id, Id);				
			info.AddValue(ToppingBase.Property_ToppingName, ToppingName);				
			info.AddValue(ToppingBase.Property_Price, Price);				
			info.AddValue(ToppingBase.Property_IsAvailable, IsAvailable);				
			info.AddValue(ToppingBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(ToppingBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(ToppingBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(ToppingBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(ToppingBase.Property_ToppingCategoryId, ToppingCategoryId);				
			info.AddValue(ToppingBase.Property_CompanyId, CompanyId);				
		}
		#endregion

		
	}
}
