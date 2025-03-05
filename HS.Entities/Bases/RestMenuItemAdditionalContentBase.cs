using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "RestMenuItemAdditionalContentBase", Namespace = "http://www.hims-tech.com//entities")]
	public class RestMenuItemAdditionalContentBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			ItemId = 1,
			Name = 2,
			ImageLoc = 3,
			CreatedBy = 4,
			CreatedDate = 5
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_ItemId = "ItemId";		            
		public const string Property_Name = "Name";		            
		public const string Property_ImageLoc = "ImageLoc";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _ItemId;	            
		private String _Name;	            
		private String _ImageLoc;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
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
		public String Name
		{	
			get{ return _Name; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Name, value, _Name);
				if (PropertyChanging(args))
				{
					_Name = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ImageLoc
		{	
			get{ return _ImageLoc; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ImageLoc, value, _ImageLoc);
				if (PropertyChanging(args))
				{
					_ImageLoc = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  RestMenuItemAdditionalContentBase Clone()
		{
			RestMenuItemAdditionalContentBase newObj = new  RestMenuItemAdditionalContentBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.ItemId = this.ItemId;						
			newObj.Name = this.Name;						
			newObj.ImageLoc = this.ImageLoc;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(RestMenuItemAdditionalContentBase.Property_Id, Id);				
			info.AddValue(RestMenuItemAdditionalContentBase.Property_ItemId, ItemId);				
			info.AddValue(RestMenuItemAdditionalContentBase.Property_Name, Name);				
			info.AddValue(RestMenuItemAdditionalContentBase.Property_ImageLoc, ImageLoc);				
			info.AddValue(RestMenuItemAdditionalContentBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(RestMenuItemAdditionalContentBase.Property_CreatedDate, CreatedDate);				
		}
		#endregion

		
	}
}
