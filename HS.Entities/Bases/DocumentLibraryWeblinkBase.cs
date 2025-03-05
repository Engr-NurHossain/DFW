using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "DocumentLibraryWeblinkBase", Namespace = "http://www.piistech.com//entities")]
	public class DocumentLibraryWeblinkBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			KnowledgebaseId = 1,
			Title = 2,
			Link = 3,
			IsRelated = 4
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_KnowledgebaseId = "KnowledgebaseId";		            
		public const string Property_Title = "Title";		            
		public const string Property_Link = "Link";		            
		public const string Property_IsRelated = "IsRelated";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Int32 _KnowledgebaseId;	            
		private String _Title;	            
		private String _Link;	            
		private Boolean _IsRelated;	            
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
		public String Title
		{	
			get{ return _Title; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Title, value, _Title);
				if (PropertyChanging(args))
				{
					_Title = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Link
		{	
			get{ return _Link; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Link, value, _Link);
				if (PropertyChanging(args))
				{
					_Link = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean IsRelated
		{	
			get{ return _IsRelated; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsRelated, value, _IsRelated);
				if (PropertyChanging(args))
				{
					_IsRelated = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  DocumentLibraryWeblinkBase Clone()
		{
			DocumentLibraryWeblinkBase newObj = new  DocumentLibraryWeblinkBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.KnowledgebaseId = this.KnowledgebaseId;						
			newObj.Title = this.Title;						
			newObj.Link = this.Link;						
			newObj.IsRelated = this.IsRelated;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(DocumentLibraryWeblinkBase.Property_Id, Id);				
			info.AddValue(DocumentLibraryWeblinkBase.Property_KnowledgebaseId, KnowledgebaseId);				
			info.AddValue(DocumentLibraryWeblinkBase.Property_Title, Title);				
			info.AddValue(DocumentLibraryWeblinkBase.Property_Link, Link);				
			info.AddValue(DocumentLibraryWeblinkBase.Property_IsRelated, IsRelated);				
		}
		#endregion

		
	}
}