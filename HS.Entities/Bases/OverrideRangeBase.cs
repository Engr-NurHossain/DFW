using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "OverrideRangeBase", Namespace = "http://www.piistech.com//entities")]
	public class OverrideRangeBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			OverrideId = 1,
			RangeStart = 2,
			RangeEnd = 3,
			Amount = 4
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_OverrideId = "OverrideId";		            
		public const string Property_RangeStart = "RangeStart";		            
		public const string Property_RangeEnd = "RangeEnd";		            
		public const string Property_Amount = "Amount";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Nullable<Int32> _OverrideId;	            
		private Nullable<Int32> _RangeStart;	            
		private Nullable<Int32> _RangeEnd;	            
		private Nullable<Double> _Amount;	            
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
		public Nullable<Int32> OverrideId
		{	
			get{ return _OverrideId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OverrideId, value, _OverrideId);
				if (PropertyChanging(args))
				{
					_OverrideId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> RangeStart
		{	
			get{ return _RangeStart; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RangeStart, value, _RangeStart);
				if (PropertyChanging(args))
				{
					_RangeStart = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> RangeEnd
		{	
			get{ return _RangeEnd; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RangeEnd, value, _RangeEnd);
				if (PropertyChanging(args))
				{
					_RangeEnd = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> Amount
		{	
			get{ return _Amount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Amount, value, _Amount);
				if (PropertyChanging(args))
				{
					_Amount = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  OverrideRangeBase Clone()
		{
			OverrideRangeBase newObj = new  OverrideRangeBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.OverrideId = this.OverrideId;						
			newObj.RangeStart = this.RangeStart;						
			newObj.RangeEnd = this.RangeEnd;						
			newObj.Amount = this.Amount;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(OverrideRangeBase.Property_Id, Id);				
			info.AddValue(OverrideRangeBase.Property_OverrideId, OverrideId);				
			info.AddValue(OverrideRangeBase.Property_RangeStart, RangeStart);				
			info.AddValue(OverrideRangeBase.Property_RangeEnd, RangeEnd);				
			info.AddValue(OverrideRangeBase.Property_Amount, Amount);				
		}
		#endregion

		
	}
}
