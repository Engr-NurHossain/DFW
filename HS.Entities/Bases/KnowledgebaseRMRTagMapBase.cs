using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "KnowledgebaseRMRTagMapBase", Namespace = "http://www.hims-tech.com//entities")]
	public class KnowledgebaseRMRTagMapBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			TagId = 1,
			KnowledgebaseId = 2,
			CreatedBy = 3,
			CreatedDate = 4,
			LastUpdatedBy = 5,
			LastUpdatedDate = 6,
			IsDeleted = 7
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_TagId = "TagId";		            
		public const string Property_KnowledgebaseId = "KnowledgebaseId";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_IsDeleted = "IsDeleted";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Int32 _TagId;	            
		private Int32 _KnowledgebaseId;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
		private Guid _LastUpdatedBy;	            
		private DateTime _LastUpdatedDate;	            
		private Boolean _IsDeleted;	            
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
		public Int32 TagId
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

		#endregion
		
		#region Cloning Base Objects
		public  KnowledgebaseRMRTagMapBase Clone()
		{
			KnowledgebaseRMRTagMapBase newObj = new  KnowledgebaseRMRTagMapBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.TagId = this.TagId;						
			newObj.KnowledgebaseId = this.KnowledgebaseId;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.IsDeleted = this.IsDeleted;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(KnowledgebaseRMRTagMapBase.Property_Id, Id);				
			info.AddValue(KnowledgebaseRMRTagMapBase.Property_TagId, TagId);				
			info.AddValue(KnowledgebaseRMRTagMapBase.Property_KnowledgebaseId, KnowledgebaseId);				
			info.AddValue(KnowledgebaseRMRTagMapBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(KnowledgebaseRMRTagMapBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(KnowledgebaseRMRTagMapBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(KnowledgebaseRMRTagMapBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(KnowledgebaseRMRTagMapBase.Property_IsDeleted, IsDeleted);				
		}
		#endregion

		
	}
}