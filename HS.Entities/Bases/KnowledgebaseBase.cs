using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "KnowledgebaseBase", Namespace = "http://www.hims-tech.com//entities")]
	public class KnowledgebaseBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			Title = 1,
			Answer = 2,
			Tags = 3,
			CreatedBy = 4,
			CreatedDate = 5,
			LastUpdatedBy = 6,
			LastUpdatedDate = 7,
			IsDeleted = 8,
			IsDocumentLibrary = 9,
			IsFlag = 10,
			FlagBy = 11,
			FlagDate = 12,
			IsHidden = 13
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_Title = "Title";		            
		public const string Property_Answer = "Answer";		            
		public const string Property_Tags = "Tags";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_IsDeleted = "IsDeleted";		            
		public const string Property_IsDocumentLibrary = "IsDocumentLibrary";		            
		public const string Property_IsFlag = "IsFlag";		            
		public const string Property_FlagBy = "FlagBy";		            
		public const string Property_FlagDate = "FlagDate";		            
		public const string Property_IsHidden = "IsHidden";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _Title;	            
		private String _Answer;	            
		private String _Tags;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
		private Guid _LastUpdatedBy;	            
		private DateTime _LastUpdatedDate;	            
		private Boolean _IsDeleted;	            
		private Boolean _IsDocumentLibrary;	            
		private Boolean _IsFlag;	            
		private Guid _FlagBy;	            
		private Nullable<DateTime> _FlagDate;	            
		private Boolean _IsHidden;	            
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
		public String Answer
		{	
			get{ return _Answer; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Answer, value, _Answer);
				if (PropertyChanging(args))
				{
					_Answer = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Tags
		{	
			get{ return _Tags; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Tags, value, _Tags);
				if (PropertyChanging(args))
				{
					_Tags = value;
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
		public Boolean IsFlag
		{	
			get{ return _IsFlag; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsFlag, value, _IsFlag);
				if (PropertyChanging(args))
				{
					_IsFlag = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid FlagBy
		{	
			get{ return _FlagBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FlagBy, value, _FlagBy);
				if (PropertyChanging(args))
				{
					_FlagBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> FlagDate
		{	
			get{ return _FlagDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FlagDate, value, _FlagDate);
				if (PropertyChanging(args))
				{
					_FlagDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean IsHidden
		{	
			get{ return _IsHidden; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsHidden, value, _IsHidden);
				if (PropertyChanging(args))
				{
					_IsHidden = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  KnowledgebaseBase Clone()
		{
			KnowledgebaseBase newObj = new  KnowledgebaseBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.Title = this.Title;						
			newObj.Answer = this.Answer;						
			newObj.Tags = this.Tags;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.IsDeleted = this.IsDeleted;						
			newObj.IsDocumentLibrary = this.IsDocumentLibrary;						
			newObj.IsFlag = this.IsFlag;						
			newObj.FlagBy = this.FlagBy;						
			newObj.FlagDate = this.FlagDate;						
			newObj.IsHidden = this.IsHidden;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(KnowledgebaseBase.Property_Id, Id);				
			info.AddValue(KnowledgebaseBase.Property_Title, Title);				
			info.AddValue(KnowledgebaseBase.Property_Answer, Answer);				
			info.AddValue(KnowledgebaseBase.Property_Tags, Tags);				
			info.AddValue(KnowledgebaseBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(KnowledgebaseBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(KnowledgebaseBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(KnowledgebaseBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(KnowledgebaseBase.Property_IsDeleted, IsDeleted);				
			info.AddValue(KnowledgebaseBase.Property_IsDocumentLibrary, IsDocumentLibrary);				
			info.AddValue(KnowledgebaseBase.Property_IsFlag, IsFlag);				
			info.AddValue(KnowledgebaseBase.Property_FlagBy, FlagBy);				
			info.AddValue(KnowledgebaseBase.Property_FlagDate, FlagDate);				
			info.AddValue(KnowledgebaseBase.Property_IsHidden, IsHidden);				
		}
		#endregion

		
	}
}