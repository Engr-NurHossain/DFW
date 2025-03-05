using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "AgreementQuestionBase", Namespace = "http://www.hims-tech.com//entities")]
	public class AgreementQuestionBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			Title = 1,
			IsActive = 2,
			SiteType = 3,
			QuestionId = 4
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_Title = "Title";		            
		public const string Property_IsActive = "IsActive";		            
		public const string Property_SiteType = "SiteType";		            
		public const string Property_QuestionId = "QuestionId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _Title;	            
		private Nullable<Boolean> _IsActive;	            
		private String _SiteType;	            
		private Nullable<Int32> _QuestionId;	            
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
		public Nullable<Boolean> IsActive
		{	
			get{ return _IsActive; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsActive, value, _IsActive);
				if (PropertyChanging(args))
				{
					_IsActive = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SiteType
		{	
			get{ return _SiteType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SiteType, value, _SiteType);
				if (PropertyChanging(args))
				{
					_SiteType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> QuestionId
		{	
			get{ return _QuestionId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_QuestionId, value, _QuestionId);
				if (PropertyChanging(args))
				{
					_QuestionId = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  AgreementQuestionBase Clone()
		{
			AgreementQuestionBase newObj = new  AgreementQuestionBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.Title = this.Title;						
			newObj.IsActive = this.IsActive;						
			newObj.SiteType = this.SiteType;						
			newObj.QuestionId = this.QuestionId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(AgreementQuestionBase.Property_Id, Id);				
			info.AddValue(AgreementQuestionBase.Property_Title, Title);				
			info.AddValue(AgreementQuestionBase.Property_IsActive, IsActive);				
			info.AddValue(AgreementQuestionBase.Property_SiteType, SiteType);				
			info.AddValue(AgreementQuestionBase.Property_QuestionId, QuestionId);				
		}
		#endregion

		
	}
}
