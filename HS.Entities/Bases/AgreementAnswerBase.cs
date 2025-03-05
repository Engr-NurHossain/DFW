using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "AgreementAnswerBase", Namespace = "http://www.piistech.com//entities")]
	public class AgreementAnswerBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			QuestionId = 1,
			CustomerId = 2,
			Answer = 3,
			AnswerDate = 4,
			Ip = 5,
			UserAgent = 6
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_QuestionId = "QuestionId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_Answer = "Answer";		            
		public const string Property_AnswerDate = "AnswerDate";		            
		public const string Property_Ip = "Ip";		            
		public const string Property_UserAgent = "UserAgent";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Int32 _QuestionId;	            
		private Guid _CustomerId;	            
		private String _Answer;	            
		private Nullable<DateTime> _AnswerDate;	            
		private String _Ip;	            
		private String _UserAgent;	            
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
		public Nullable<DateTime> AnswerDate
		{	
			get{ return _AnswerDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AnswerDate, value, _AnswerDate);
				if (PropertyChanging(args))
				{
					_AnswerDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Ip
		{	
			get{ return _Ip; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Ip, value, _Ip);
				if (PropertyChanging(args))
				{
					_Ip = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String UserAgent
		{	
			get{ return _UserAgent; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UserAgent, value, _UserAgent);
				if (PropertyChanging(args))
				{
					_UserAgent = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  AgreementAnswerBase Clone()
		{
			AgreementAnswerBase newObj = new  AgreementAnswerBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.QuestionId = this.QuestionId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.Answer = this.Answer;						
			newObj.AnswerDate = this.AnswerDate;						
			newObj.Ip = this.Ip;						
			newObj.UserAgent = this.UserAgent;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(AgreementAnswerBase.Property_Id, Id);				
			info.AddValue(AgreementAnswerBase.Property_QuestionId, QuestionId);				
			info.AddValue(AgreementAnswerBase.Property_CustomerId, CustomerId);				
			info.AddValue(AgreementAnswerBase.Property_Answer, Answer);				
			info.AddValue(AgreementAnswerBase.Property_AnswerDate, AnswerDate);				
			info.AddValue(AgreementAnswerBase.Property_Ip, Ip);				
			info.AddValue(AgreementAnswerBase.Property_UserAgent, UserAgent);				
		}
		#endregion

		
	}
}
