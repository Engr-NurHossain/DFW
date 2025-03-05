using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "KnowledgebaseGroupAccessBase", Namespace = "http://www.hims-tech.com//entities")]
	public class KnowledgebaseGroupAccessBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			KnowledgebaseId = 1,
			IsDocumentLibrary = 2,
			IsDefault = 3,
			UserGroupId = 4,
			CreatedBy = 5,
			CreatedDate = 6
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_KnowledgebaseId = "KnowledgebaseId";		            
		public const string Property_IsDocumentLibrary = "IsDocumentLibrary";		            
		public const string Property_IsDefault = "IsDefault";		            
		public const string Property_UserGroupId = "UserGroupId";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Int32 _KnowledgebaseId;	            
		private Boolean _IsDocumentLibrary;	            
		private Boolean _IsDefault;	            
		private Int32 _UserGroupId;	            
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
		public Boolean IsDocumentLibrary
		{	
			get{ return _IsDocumentLibrary; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsDocumentLibrary, value, _IsDocumentLibrary);
				if (PropertyChanging(args))
				{
					_IsDocumentLibrary = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean IsDefault
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
		public Int32 UserGroupId
		{	
			get{ return _UserGroupId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UserGroupId, value, _UserGroupId);
				if (PropertyChanging(args))
				{
					_UserGroupId = value;
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
		public  KnowledgebaseGroupAccessBase Clone()
		{
			KnowledgebaseGroupAccessBase newObj = new  KnowledgebaseGroupAccessBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.KnowledgebaseId = this.KnowledgebaseId;						
			newObj.IsDocumentLibrary = this.IsDocumentLibrary;						
			newObj.IsDefault = this.IsDefault;						
			newObj.UserGroupId = this.UserGroupId;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(KnowledgebaseGroupAccessBase.Property_Id, Id);				
			info.AddValue(KnowledgebaseGroupAccessBase.Property_KnowledgebaseId, KnowledgebaseId);				
			info.AddValue(KnowledgebaseGroupAccessBase.Property_IsDocumentLibrary, IsDocumentLibrary);				
			info.AddValue(KnowledgebaseGroupAccessBase.Property_IsDefault, IsDefault);				
			info.AddValue(KnowledgebaseGroupAccessBase.Property_UserGroupId, UserGroupId);				
			info.AddValue(KnowledgebaseGroupAccessBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(KnowledgebaseGroupAccessBase.Property_CreatedDate, CreatedDate);				
		}
		#endregion

		
	}
}