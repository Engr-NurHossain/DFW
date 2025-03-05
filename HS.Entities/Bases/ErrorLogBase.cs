using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "ErrorLogBase", Namespace = "http://www.piistech.com//entities")]
	public class ErrorLogBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			ErrorId = 1,
			ErrorFor = 2,
			Message = 3,
			TimeUtc = 4
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_ErrorId = "ErrorId";		            
		public const string Property_ErrorFor = "ErrorFor";		            
		public const string Property_Message = "Message";		            
		public const string Property_TimeUtc = "TimeUtc";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _ErrorId;	            
		private String _ErrorFor;	            
		private String _Message;	            
		private DateTime _TimeUtc;	            
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
		public Guid ErrorId
		{	
			get{ return _ErrorId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ErrorId, value, _ErrorId);
				if (PropertyChanging(args))
				{
					_ErrorId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ErrorFor
		{	
			get{ return _ErrorFor; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ErrorFor, value, _ErrorFor);
				if (PropertyChanging(args))
				{
					_ErrorFor = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Message
		{	
			get{ return _Message; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Message, value, _Message);
				if (PropertyChanging(args))
				{
					_Message = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime TimeUtc
		{	
			get{ return _TimeUtc; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TimeUtc, value, _TimeUtc);
				if (PropertyChanging(args))
				{
					_TimeUtc = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  ErrorLogBase Clone()
		{
			ErrorLogBase newObj = new  ErrorLogBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.ErrorId = this.ErrorId;						
			newObj.ErrorFor = this.ErrorFor;						
			newObj.Message = this.Message;						
			newObj.TimeUtc = this.TimeUtc;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(ErrorLogBase.Property_Id, Id);				
			info.AddValue(ErrorLogBase.Property_ErrorId, ErrorId);				
			info.AddValue(ErrorLogBase.Property_ErrorFor, ErrorFor);				
			info.AddValue(ErrorLogBase.Property_Message, Message);				
			info.AddValue(ErrorLogBase.Property_TimeUtc, TimeUtc);				
		}
		#endregion

		
	}
}
