using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomSurveyAnswerBase", Namespace = "http://www.piistech.com//entities")]
	public class CustomSurveyAnswerBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			QuestionId = 1,
			AnswerId = 2,
			Answer = 3,
			CreatedBy = 4,
			CreatedDate = 5,
			OrderBy = 6
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_QuestionId = "QuestionId";		            
		public const string Property_AnswerId = "AnswerId";		            
		public const string Property_Answer = "Answer";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_OrderBy = "OrderBy";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _QuestionId;	            
		private Guid _AnswerId;	            
		private String _Answer;	            
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
		public  CustomSurveyAnswerBase Clone()
		{
			CustomSurveyAnswerBase newObj = new  CustomSurveyAnswerBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.QuestionId = this.QuestionId;						
			newObj.AnswerId = this.AnswerId;						
			newObj.Answer = this.Answer;						
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
			info.AddValue(CustomSurveyAnswerBase.Property_Id, Id);				
			info.AddValue(CustomSurveyAnswerBase.Property_QuestionId, QuestionId);				
			info.AddValue(CustomSurveyAnswerBase.Property_AnswerId, AnswerId);				
			info.AddValue(CustomSurveyAnswerBase.Property_Answer, Answer);				
			info.AddValue(CustomSurveyAnswerBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(CustomSurveyAnswerBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(CustomSurveyAnswerBase.Property_OrderBy, OrderBy);				
		}
		#endregion

		
	}
}
