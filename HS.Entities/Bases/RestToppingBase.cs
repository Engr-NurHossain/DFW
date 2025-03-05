using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "RestToppingBase", Namespace = "http://www.hims-tech.com//entities")]
	public class RestToppingBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			ToppingId = 1,
			ToppingName = 2,
			Price = 3,
			IsAvailable = 4,
			CreatedDate = 5,
			CreatedBy = 6,
			LastUpdatedBy = 7,
			LastUpdatedDate = 8,
			CompanyId = 9,
			ToppingCategoryId = 10,
			IsDefault = 11,
			Description = 12
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_ToppingId = "ToppingId";		            
		public const string Property_ToppingName = "ToppingName";		            
		public const string Property_Price = "Price";		            
		public const string Property_IsAvailable = "IsAvailable";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_ToppingCategoryId = "ToppingCategoryId";		            
		public const string Property_IsDefault = "IsDefault";		            
		public const string Property_Description = "Description";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _ToppingId;	            
		private String _ToppingName;	            
		private Nullable<Double> _Price;	            
		private Nullable<Boolean> _IsAvailable;	            
		private DateTime _CreatedDate;	            
		private Guid _CreatedBy;	            
		private Guid _LastUpdatedBy;	            
		private DateTime _LastUpdatedDate;	            
		private Guid _CompanyId;	            
		private Guid _ToppingCategoryId;	            
		private Nullable<Boolean> _IsDefault;	            
		private String _Description;	            
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
		public Guid ToppingId
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
		public Nullable<Boolean> IsDefault
		{	
			get{ return _IsDefault; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsDefault, value, _IsDefault);
				if (PropertyChanging(args))
				{
					_IsDefault = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  RestToppingBase Clone()
		{
			RestToppingBase newObj = new  RestToppingBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.ToppingId = this.ToppingId;						
			newObj.ToppingName = this.ToppingName;						
			newObj.Price = this.Price;						
			newObj.IsAvailable = this.IsAvailable;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.CompanyId = this.CompanyId;						
			newObj.ToppingCategoryId = this.ToppingCategoryId;						
			newObj.IsDefault = this.IsDefault;						
			newObj.Description = this.Description;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(RestToppingBase.Property_Id, Id);				
			info.AddValue(RestToppingBase.Property_ToppingId, ToppingId);				
			info.AddValue(RestToppingBase.Property_ToppingName, ToppingName);				
			info.AddValue(RestToppingBase.Property_Price, Price);				
			info.AddValue(RestToppingBase.Property_IsAvailable, IsAvailable);				
			info.AddValue(RestToppingBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(RestToppingBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(RestToppingBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(RestToppingBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(RestToppingBase.Property_CompanyId, CompanyId);				
			info.AddValue(RestToppingBase.Property_ToppingCategoryId, ToppingCategoryId);				
			info.AddValue(RestToppingBase.Property_IsDefault, IsDefault);				
			info.AddValue(RestToppingBase.Property_Description, Description);				
		}
		#endregion

		
	}
}
