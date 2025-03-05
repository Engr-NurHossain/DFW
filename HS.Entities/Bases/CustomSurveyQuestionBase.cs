using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomSurveyQuestionBase", Namespace = "http://www.piistech.com//entities")]
	public class CustomSurveyQuestionBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			SurveyId = 1,
			QuestionId = 2,
			Question = 3,
			QuestionType = 4,
			CreatedBy = 5,
			CreatedDate = 6,
			OrderBy = 7
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_SurveyId = "SurveyId";		            
		public const string Property_QuestionId = "QuestionId";		            
		public const string Property_Question = "Question";		            
		public const string Property_QuestionType = "QuestionType";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_OrderBy = "OrderBy";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _SurveyId;	            
		private Guid _QuestionId;	            
		private String _Question;	            
		private String _QuestionType;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
		private Nullable<Int32> _OrderBy;	            
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
		public Guid SurveyId
		{	
			get{ return _SurveyId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SurveyId, value, _SurveyId);
				if (PropertyChanging(args))
				{
					_SurveyId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid QuestionId
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

		[DataMember]
		public String Question
		{	
			get{ return _Question; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Question, value, _Question);
				if (PropertyChanging(args))
				{
					_Question = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String QuestionType
		{	
			get{ return _QuestionType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_QuestionType, value, _QuestionType);
				if (PropertyChanging(args))
				{
					_QuestionType = value;
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
		public Nullable<Int32> OrderBy
		{	
			get{ return _OrderBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OrderBy, value, _OrderBy);
				if (PropertyChanging(args))
				{
					_OrderBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  CustomSurveyQuestionBase Clone()
		{
			CustomSurveyQuestionBase newObj = new  CustomSurveyQuestionBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.SurveyId = this.SurveyId;						
			newObj.QuestionId = this.QuestionId;						
			newObj.Question = this.Question;						
			newObj.QuestionType = this.QuestionType;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.OrderBy = this.OrderBy;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomSurveyQuestionBase.Property_Id, Id);				
			info.AddValue(CustomSurveyQuestionBase.Property_SurveyId, SurveyId);				
			info.AddValue(CustomSurveyQuestionBase.Property_QuestionId, QuestionId);				
			info.AddValue(CustomSurveyQuestionBase.Property_Question, Question);				
			info.AddValue(CustomSurveyQuestionBase.Property_QuestionType, QuestionType);				
			info.AddValue(CustomSurveyQuestionBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(CustomSurveyQuestionBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(CustomSurveyQuestionBase.Property_OrderBy, OrderBy);				
		}
		#endregion

		
	}
}
