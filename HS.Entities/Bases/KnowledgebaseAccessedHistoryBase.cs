using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "KnowledgebaseAccessedHistoryBase", Namespace = "http://www.hims-tech.com//entities")]
	public class KnowledgebaseAccessedHistoryBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			KnowledgebaseId = 1,
			IsDocumentLibrary = 2,
			VisitedBy = 3,
			VisitedDate = 4
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_KnowledgebaseId = "KnowledgebaseId";		            
		public const string Property_IsDocumentLibrary = "IsDocumentLibrary";		            
		public const string Property_VisitedBy = "VisitedBy";		            
		public const string Property_VisitedDate = "VisitedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Int32 _KnowledgebaseId;	            
		private Boolean _IsDocumentLibrary;	            
		private Guid _VisitedBy;	            
		private DateTime _VisitedDate;	            
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
		public Guid VisitedBy
		{	
			get{ return _VisitedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_VisitedBy, value, _VisitedBy);
				if (PropertyChanging(args))
				{
					_VisitedBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime VisitedDate
		{	
			get{ return _VisitedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_VisitedDate, value, _VisitedDate);
				if (PropertyChanging(args))
				{
					_VisitedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  KnowledgebaseAccessedHistoryBase Clone()
		{
			KnowledgebaseAccessedHistoryBase newObj = new  KnowledgebaseAccessedHistoryBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.KnowledgebaseId = this.KnowledgebaseId;						
			newObj.IsDocumentLibrary = this.IsDocumentLibrary;						
			newObj.VisitedBy = this.VisitedBy;						
			newObj.VisitedDate = this.VisitedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(KnowledgebaseAccessedHistoryBase.Property_Id, Id);				
			info.AddValue(KnowledgebaseAccessedHistoryBase.Property_KnowledgebaseId, KnowledgebaseId);				
			info.AddValue(KnowledgebaseAccessedHistoryBase.Property_IsDocumentLibrary, IsDocumentLibrary);				
			info.AddValue(KnowledgebaseAccessedHistoryBase.Property_VisitedBy, VisitedBy);				
			info.AddValue(KnowledgebaseAccessedHistoryBase.Property_VisitedDate, VisitedDate);				
		}
		#endregion

		
	}
}