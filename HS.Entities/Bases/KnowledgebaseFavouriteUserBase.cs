using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "KnowledgebaseFavouriteUserBase", Namespace = "http://www.piistech.com//entities")]
	public class KnowledgebaseFavouriteUserBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			UserId = 1,
			KnowledgebaseId = 2,
			IsFavourite = 3,
			Comment = 4,
			CreatedBy = 5,
			CreatedDate = 6,
			LastUpdatedBy = 7,
			LastUpdatedDate = 8
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_UserId = "UserId";		            
		public const string Property_KnowledgebaseId = "KnowledgebaseId";		            
		public const string Property_IsFavourite = "IsFavourite";		            
		public const string Property_Comment = "Comment";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _UserId;	            
		private Int32 _KnowledgebaseId;	            
		private Boolean _IsFavourite;	            
		private String _Comment;	            
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
		public Int32 KnowledgebaseId
		{	
			get{ return _KnowledgebaseId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_KnowledgebaseId, value, _KnowledgebaseId);
				if (PropertyChanging(args))
				{
					_KnowledgebaseId = value;
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
		public String Comment
		{	
			get{ return _Comment; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Comment, value, _Comment);
				if (PropertyChanging(args))
				{
					_Comment = value;
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
		public  KnowledgebaseFavouriteUserBase Clone()
		{
			KnowledgebaseFavouriteUserBase newObj = new  KnowledgebaseFavouriteUserBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.UserId = this.UserId;						
			newObj.KnowledgebaseId = this.KnowledgebaseId;						
			newObj.IsFavourite = this.IsFavourite;						
			newObj.Comment = this.Comment;						
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
			info.AddValue(KnowledgebaseFavouriteUserBase.Property_Id, Id);				
			info.AddValue(KnowledgebaseFavouriteUserBase.Property_UserId, UserId);				
			info.AddValue(KnowledgebaseFavouriteUserBase.Property_KnowledgebaseId, KnowledgebaseId);				
			info.AddValue(KnowledgebaseFavouriteUserBase.Property_IsFavourite, IsFavourite);				
			info.AddValue(KnowledgebaseFavouriteUserBase.Property_Comment, Comment);				
			info.AddValue(KnowledgebaseFavouriteUserBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(KnowledgebaseFavouriteUserBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(KnowledgebaseFavouriteUserBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(KnowledgebaseFavouriteUserBase.Property_LastUpdatedDate, LastUpdatedDate);				
		}
		#endregion

		
	}
}