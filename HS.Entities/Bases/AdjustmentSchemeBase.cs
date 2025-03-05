using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "AdjustmentSchemeBase", Namespace = "http://www.piistech.com//entities")]
	public class AdjustmentSchemeBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			Name = 1,
			ComissionSessionId = 2
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_Name = "Name";		            
		public const string Property_ComissionSessionId = "ComissionSessionId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _Name;	            
		private Nullable<Int32> _ComissionSessionId;	            
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
		public Nullable<Int32> ComissionSessionId
		{	
			get{ return _ComissionSessionId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ComissionSessionId, value, _ComissionSessionId);
				if (PropertyChanging(args))
				{
					_ComissionSessionId = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  AdjustmentSchemeBase Clone()
		{
			AdjustmentSchemeBase newObj = new  AdjustmentSchemeBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.Name = this.Name;						
			newObj.ComissionSessionId = this.ComissionSessionId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(AdjustmentSchemeBase.Property_Id, Id);				
			info.AddValue(AdjustmentSchemeBase.Property_Name, Name);				
			info.AddValue(AdjustmentSchemeBase.Property_ComissionSessionId, ComissionSessionId);				
		}
		#endregion

		
	}
}
