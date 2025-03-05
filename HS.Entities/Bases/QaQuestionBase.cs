using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "QaQuestionBase", Namespace = "http://www.piistech.com//entities")]
	public class QaQuestionBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			Title = 2,
			Qa1 = 3,
			Qa2 = 4,
			Type = 5,
			IsActive = 6
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_Title = "Title";		            
		public const string Property_Qa1 = "Qa1";		            
		public const string Property_Qa2 = "Qa2";		            
		public const string Property_Type = "Type";		            
		public const string Property_IsActive = "IsActive";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private String _Title;	            
		private Boolean _Qa1;	            
		private Boolean _Qa2;	            
		private String _Type;	            
		private Boolean _IsActive;	            
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
		public Boolean Qa1
		{	
			get{ return _Qa1; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Qa1, value, _Qa1);
				if (PropertyChanging(args))
				{
					_Qa1 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean Qa2
		{	
			get{ return _Qa2; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Qa2, value, _Qa2);
				if (PropertyChanging(args))
				{
					_Qa2 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Type
		{	
			get{ return _Type; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Type, value, _Type);
				if (PropertyChanging(args))
				{
					_Type = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean IsActive
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

		#endregion
		
		#region Cloning Base Objects
		public  QaQuestionBase Clone()
		{
			QaQuestionBase newObj = new  QaQuestionBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.Title = this.Title;						
			newObj.Qa1 = this.Qa1;						
			newObj.Qa2 = this.Qa2;						
			newObj.Type = this.Type;						
			newObj.IsActive = this.IsActive;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(QaQuestionBase.Property_Id, Id);				
			info.AddValue(QaQuestionBase.Property_CompanyId, CompanyId);				
			info.AddValue(QaQuestionBase.Property_Title, Title);				
			info.AddValue(QaQuestionBase.Property_Qa1, Qa1);				
			info.AddValue(QaQuestionBase.Property_Qa2, Qa2);				
			info.AddValue(QaQuestionBase.Property_Type, Type);				
			info.AddValue(QaQuestionBase.Property_IsActive, IsActive);				
		}
		#endregion

		
	}
}
