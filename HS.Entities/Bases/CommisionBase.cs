using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CommisionBase", Namespace = "http://www.piistech.com//entities")]
	public class CommisionBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			Name = 1,
			TimeFrame = 2,
			CommisionTypeId = 3,
			CommisionSessionId = 4,
			IsActive = 5
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_Name = "Name";		            
		public const string Property_TimeFrame = "TimeFrame";		            
		public const string Property_CommisionTypeId = "CommisionTypeId";		            
		public const string Property_CommisionSessionId = "CommisionSessionId";		            
		public const string Property_IsActive = "IsActive";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _Name;	            
		private String _TimeFrame;	            
		private Nullable<Int32> _CommisionTypeId;	            
		private Nullable<Int32> _CommisionSessionId;	            
		private Nullable<Boolean> _IsActive;	            
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
		public String Name
		{	
			get{ return _Name; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Name, value, _Name);
				if (PropertyChanging(args))
				{
					_Name = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TimeFrame
		{	
			get{ return _TimeFrame; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TimeFrame, value, _TimeFrame);
				if (PropertyChanging(args))
				{
					_TimeFrame = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> CommisionTypeId
		{	
			get{ return _CommisionTypeId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CommisionTypeId, value, _CommisionTypeId);
				if (PropertyChanging(args))
				{
					_CommisionTypeId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> CommisionSessionId
		{	
			get{ return _CommisionSessionId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CommisionSessionId, value, _CommisionSessionId);
				if (PropertyChanging(args))
				{
					_CommisionSessionId = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  CommisionBase Clone()
		{
			CommisionBase newObj = new  CommisionBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.Name = this.Name;						
			newObj.TimeFrame = this.TimeFrame;						
			newObj.CommisionTypeId = this.CommisionTypeId;						
			newObj.CommisionSessionId = this.CommisionSessionId;						
			newObj.IsActive = this.IsActive;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CommisionBase.Property_Id, Id);				
			info.AddValue(CommisionBase.Property_Name, Name);				
			info.AddValue(CommisionBase.Property_TimeFrame, TimeFrame);				
			info.AddValue(CommisionBase.Property_CommisionTypeId, CommisionTypeId);				
			info.AddValue(CommisionBase.Property_CommisionSessionId, CommisionSessionId);				
			info.AddValue(CommisionBase.Property_IsActive, IsActive);				
		}
		#endregion

		
	}
}
