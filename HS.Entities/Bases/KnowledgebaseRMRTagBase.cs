using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "KnowledgebaseRMRTagBase", Namespace = "http://www.hims-tech.com//entities")]
	public class KnowledgebaseRMRTagBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			TagName = 1,
			CreatedDate = 2,
			CreatedBy = 3,
			LastUpdatedDate = 4,
			LastUpdatedBy = 5,
			IsDeleted = 6,
			IsFavourite = 7,
			IsKnowledgebaseNav = 8
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_TagName = "TagName";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_IsDeleted = "IsDeleted";		            
		public const string Property_IsFavourite = "IsFavourite";		            
		public const string Property_IsKnowledgebaseNav = "IsKnowledgebaseNav";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _TagName;	            
		private DateTime _CreatedDate;	            
		private Guid _CreatedBy;	            
		private DateTime _LastUpdatedDate;	            
		private Guid _LastUpdatedBy;	            
		private Boolean _IsDeleted;	            
		private Boolean _IsFavourite;	            
		private Boolean _IsKnowledgebaseNav;	            
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

		[DataMember]
		public Boolean IsDeleted
		{	
			get{ return _IsDeleted; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsDeleted, value, _IsDeleted);
				if (PropertyChanging(args))
				{
					_IsDeleted = value;
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
		public Boolean IsKnowledgebaseNav
		{	
			get{ return _IsKnowledgebaseNav; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsKnowledgebaseNav, value, _IsKnowledgebaseNav);
				if (PropertyChanging(args))
				{
					_IsKnowledgebaseNav = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  KnowledgebaseRMRTagBase Clone()
		{
			KnowledgebaseRMRTagBase newObj = new  KnowledgebaseRMRTagBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.TagName = this.TagName;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.IsDeleted = this.IsDeleted;						
			newObj.IsFavourite = this.IsFavourite;						
			newObj.IsKnowledgebaseNav = this.IsKnowledgebaseNav;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(KnowledgebaseRMRTagBase.Property_Id, Id);				
			info.AddValue(KnowledgebaseRMRTagBase.Property_TagName, TagName);				
			info.AddValue(KnowledgebaseRMRTagBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(KnowledgebaseRMRTagBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(KnowledgebaseRMRTagBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(KnowledgebaseRMRTagBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(KnowledgebaseRMRTagBase.Property_IsDeleted, IsDeleted);				
			info.AddValue(KnowledgebaseRMRTagBase.Property_IsFavourite, IsFavourite);				
			info.AddValue(KnowledgebaseRMRTagBase.Property_IsKnowledgebaseNav, IsKnowledgebaseNav);				
		}
		#endregion

		
	}
}