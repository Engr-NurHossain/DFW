using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "RMRTagMapBase", Namespace = "http://www.piistech.com//entities")]
	public class RMRTagMapBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			TagId = 1,
			ContactId = 2,
			CreatedBy = 3,
			CreatedDate = 4,
			LastUpdatedBy = 5,
			LastUpdatedDate = 6
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_TagId = "TagId";		            
		public const string Property_ContactId = "ContactId";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _TagId;	            
		private Guid _ContactId;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
		private Guid _LastUpdatedBy;	            
		private DateTime _LastUpdatedDate;	            
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
		public Guid TagId
		{	
			get{ return _TagId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TagId, value, _TagId);
				if (PropertyChanging(args))
				{
					_TagId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid ContactId
		{	
			get{ return _ContactId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ContactId, value, _ContactId);
				if (PropertyChanging(args))
				{
					_ContactId = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  RMRTagMapBase Clone()
		{
			RMRTagMapBase newObj = new  RMRTagMapBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.TagId = this.TagId;						
			newObj.ContactId = this.ContactId;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(RMRTagMapBase.Property_Id, Id);				
			info.AddValue(RMRTagMapBase.Property_TagId, TagId);				
			info.AddValue(RMRTagMapBase.Property_ContactId, ContactId);				
			info.AddValue(RMRTagMapBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(RMRTagMapBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(RMRTagMapBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(RMRTagMapBase.Property_LastUpdatedDate, LastUpdatedDate);				
		}
		#endregion

		
	}
}
