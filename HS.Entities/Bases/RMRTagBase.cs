using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "RMRTagBase", Namespace = "http://www.piistech.com//entities")]
	public class RMRTagBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			TagIdentifier = 1,
			TagName = 2,
			CreatedDate = 3,
			CreatedBy = 4,
			LastUpdatedDate = 5,
			LastUpdatedBy = 6
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_TagIdentifier = "TagIdentifier";		            
		public const string Property_TagName = "TagName";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _TagIdentifier;	            
		private String _TagName;	            
		private DateTime _CreatedDate;	            
		private Guid _CreatedBy;	            
		private DateTime _LastUpdatedDate;	            
		private Guid _LastUpdatedBy;	            
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
		public Guid TagIdentifier
		{	
			get{ return _TagIdentifier; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TagIdentifier, value, _TagIdentifier);
				if (PropertyChanging(args))
				{
					_TagIdentifier = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TagName
		{	
			get{ return _TagName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TagName, value, _TagName);
				if (PropertyChanging(args))
				{
					_TagName = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  RMRTagBase Clone()
		{
			RMRTagBase newObj = new  RMRTagBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.TagIdentifier = this.TagIdentifier;						
			newObj.TagName = this.TagName;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(RMRTagBase.Property_Id, Id);				
			info.AddValue(RMRTagBase.Property_TagIdentifier, TagIdentifier);				
			info.AddValue(RMRTagBase.Property_TagName, TagName);				
			info.AddValue(RMRTagBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(RMRTagBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(RMRTagBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(RMRTagBase.Property_LastUpdatedBy, LastUpdatedBy);				
		}
		#endregion

		
	}
}
