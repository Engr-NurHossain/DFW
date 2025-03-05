using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "QaAnswerBase", Namespace = "http://www.piistech.com//entities")]
	public class QaAnswerBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			CustomerId = 2,
			QuestionId = 3,
			Answer = 4
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_QuestionId = "QuestionId";		            
		public const string Property_Answer = "Answer";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _CustomerId;	            
		private Int32 _QuestionId;	            
		private String _Answer;	            
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
		public Guid CompanyId
		{	
			get{ return _CompanyId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CompanyId, value, _CompanyId);
				if (PropertyChanging(args))
				{
					_CompanyId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid CustomerId
		{	
			get{ return _CustomerId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CustomerId, value, _CustomerId);
				if (PropertyChanging(args))
				{
					_CustomerId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Int32 QuestionId
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

		#endregion
		
		#region Cloning Base Objects
		public  QaAnswerBase Clone()
		{
			QaAnswerBase newObj = new  QaAnswerBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.QuestionId = this.QuestionId;						
			newObj.Answer = this.Answer;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(QaAnswerBase.Property_Id, Id);				
			info.AddValue(QaAnswerBase.Property_CompanyId, CompanyId);				
			info.AddValue(QaAnswerBase.Property_CustomerId, CustomerId);				
			info.AddValue(QaAnswerBase.Property_QuestionId, QuestionId);				
			info.AddValue(QaAnswerBase.Property_Answer, Answer);				
		}
		#endregion

		
	}
}
