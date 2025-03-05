using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "KnowledgeSearchedKeywordBase", Namespace = "http://www.hims-tech.com//entities")]
	public class KnowledgeSearchedKeywordBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			Keyword = 1,
			SearchedBy = 2,
			SearchedDate = 3
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_Keyword = "Keyword";		            
		public const string Property_SearchedBy = "SearchedBy";		            
		public const string Property_SearchedDate = "SearchedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _Keyword;	            
		private Guid _SearchedBy;	            
		private DateTime _SearchedDate;	            
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
		public String Keyword
		{	
			get{ return _Keyword; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Keyword, value, _Keyword);
				if (PropertyChanging(args))
				{
					_Keyword = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid SearchedBy
		{	
			get{ return _SearchedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SearchedBy, value, _SearchedBy);
				if (PropertyChanging(args))
				{
					_SearchedBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime SearchedDate
		{	
			get{ return _SearchedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SearchedDate, value, _SearchedDate);
				if (PropertyChanging(args))
				{
					_SearchedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  KnowledgeSearchedKeywordBase Clone()
		{
			KnowledgeSearchedKeywordBase newObj = new  KnowledgeSearchedKeywordBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.Keyword = this.Keyword;						
			newObj.SearchedBy = this.SearchedBy;						
			newObj.SearchedDate = this.SearchedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(KnowledgeSearchedKeywordBase.Property_Id, Id);				
			info.AddValue(KnowledgeSearchedKeywordBase.Property_Keyword, Keyword);				
			info.AddValue(KnowledgeSearchedKeywordBase.Property_SearchedBy, SearchedBy);				
			info.AddValue(KnowledgeSearchedKeywordBase.Property_SearchedDate, SearchedDate);				
		}
		#endregion

		
	}
}