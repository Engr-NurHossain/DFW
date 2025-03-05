using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomSurveyUserAnswersBase", Namespace = "http://www.piistech.com//entities")]
	public class CustomSurveyUserAnswersBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			SurveyId = 1,
			QuestionId = 2,
			AnswerId = 3,
			UserId = 4,
			Answer = 5,
			CreatedDate = 6,
			SurveyUserId = 7
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_SurveyId = "SurveyId";		            
		public const string Property_QuestionId = "QuestionId";		            
		public const string Property_AnswerId = "AnswerId";		            
		public const string Property_UserId = "UserId";		            
		public const string Property_Answer = "Answer";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_SurveyUserId = "SurveyUserId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _SurveyId;	            
		private Guid _QuestionId;	            
		private Guid _AnswerId;	            
		private Guid _UserId;	            
		private String _Answer;	            
		private DateTime _CreatedDate;	            
		private Guid _SurveyUserId;	            
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
		public Guid AnswerId
		{	
			get{ return _AnswerId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AnswerId, value, _AnswerId);
				if (PropertyChanging(args))
				{
					_AnswerId = value;
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
		public Guid SurveyUserId
		{	
			get{ return _SurveyUserId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SurveyUserId, value, _SurveyUserId);
				if (PropertyChanging(args))
				{
					_SurveyUserId = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  CustomSurveyUserAnswersBase Clone()
		{
			CustomSurveyUserAnswersBase newObj = new  CustomSurveyUserAnswersBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.SurveyId = this.SurveyId;						
			newObj.QuestionId = this.QuestionId;						
			newObj.AnswerId = this.AnswerId;						
			newObj.UserId = this.UserId;						
			newObj.Answer = this.Answer;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.SurveyUserId = this.SurveyUserId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomSurveyUserAnswersBase.Property_Id, Id);				
			info.AddValue(CustomSurveyUserAnswersBase.Property_SurveyId, SurveyId);				
			info.AddValue(CustomSurveyUserAnswersBase.Property_QuestionId, QuestionId);				
			info.AddValue(CustomSurveyUserAnswersBase.Property_AnswerId, AnswerId);				
			info.AddValue(CustomSurveyUserAnswersBase.Property_UserId, UserId);				
			info.AddValue(CustomSurveyUserAnswersBase.Property_Answer, Answer);				
			info.AddValue(CustomSurveyUserAnswersBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(CustomSurveyUserAnswersBase.Property_SurveyUserId, SurveyUserId);				
		}
		#endregion

		
	}
}
