using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "EquipmentsFavouriteBase", Namespace = "http://www.hims-tech.com//entities")]
	public class EquipmentsFavouriteBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			UserId = 1,
			CompanyId = 2,
			EquipmentId = 3,
			IsFavourite = 4,
			CreatedBy = 5,
			CreatedDate = 6
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_UserId = "UserId";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_EquipmentId = "EquipmentId";		            
		public const string Property_IsFavourite = "IsFavourite";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _UserId;	            
		private Guid _CompanyId;	            
		private Guid _EquipmentId;	            
		private Boolean _IsFavourite;	            
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
		public Guid UserId
		{	
			get{ return _UserId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UserId, value, _UserId);
				if (PropertyChanging(args))
				{
					_UserId = value;
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
		public Guid EquipmentId
		{	
			get{ return _EquipmentId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EquipmentId, value, _EquipmentId);
				if (PropertyChanging(args))
				{
					_EquipmentId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean IsFavourite
		{	
			get{ return _IsFavourite; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsFavourite, value, _IsFavourite);
				if (PropertyChanging(args))
				{
					_IsFavourite = value;
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
		public  EquipmentsFavouriteBase Clone()
		{
			EquipmentsFavouriteBase newObj = new  EquipmentsFavouriteBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.UserId = this.UserId;						
			newObj.CompanyId = this.CompanyId;						
			newObj.EquipmentId = this.EquipmentId;						
			newObj.IsFavourite = this.IsFavourite;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(EquipmentsFavouriteBase.Property_Id, Id);				
			info.AddValue(EquipmentsFavouriteBase.Property_UserId, UserId);				
			info.AddValue(EquipmentsFavouriteBase.Property_CompanyId, CompanyId);				
			info.AddValue(EquipmentsFavouriteBase.Property_EquipmentId, EquipmentId);				
			info.AddValue(EquipmentsFavouriteBase.Property_IsFavourite, IsFavourite);				
			info.AddValue(EquipmentsFavouriteBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(EquipmentsFavouriteBase.Property_CreatedDate, CreatedDate);				
		}
		#endregion

		
	}
}
