using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomSurveyBase", Namespace = "http://www.piistech.com//entities")]
	public class CustomSurveyBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			SurveyId = 1,
			SurveyName = 2,
			CreatedBy = 3,
			CreatedDate = 4
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_SurveyId = "SurveyId";		            
		public const string Property_SurveyName = "SurveyName";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _SurveyId;	            
		private String _SurveyName;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
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
		public String SurveyName
		{	
			get{ return _SurveyName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SurveyName, value, _SurveyName);
				if (PropertyChanging(args))
				{
					_SurveyName = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  CustomSurveyBase Clone()
		{
			CustomSurveyBase newObj = new  CustomSurveyBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.SurveyId = this.SurveyId;						
			newObj.SurveyName = this.SurveyName;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomSurveyBase.Property_Id, Id);				
			info.AddValue(CustomSurveyBase.Property_SurveyId, SurveyId);				
			info.AddValue(CustomSurveyBase.Property_SurveyName, SurveyName);				
			info.AddValue(CustomSurveyBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(CustomSurveyBase.Property_CreatedDate, CreatedDate);				
		}
		#endregion

		
	}
}
