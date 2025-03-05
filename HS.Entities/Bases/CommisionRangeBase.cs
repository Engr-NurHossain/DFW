using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CommisionRangeBase", Namespace = "http://www.piistech.com//entities")]
	public class CommisionRangeBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CommisionTypeId = 1,
			CommisionSessionId = 2,
			RangeStart = 3,
			RangeEnd = 4,
			Upfront = 5,
			Backend = 6,
			Bonus = 7,
			RentBonus = 8
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CommisionTypeId = "CommisionTypeId";		            
		public const string Property_CommisionSessionId = "CommisionSessionId";		            
		public const string Property_RangeStart = "RangeStart";		            
		public const string Property_RangeEnd = "RangeEnd";		            
		public const string Property_Upfront = "Upfront";		            
		public const string Property_Backend = "Backend";		            
		public const string Property_Bonus = "Bonus";		            
		public const string Property_RentBonus = "RentBonus";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Nullable<Int32> _CommisionTypeId;	            
		private Nullable<Int32> _CommisionSessionId;	            
		private Nullable<Int32> _RangeStart;	            
		private Nullable<Int32> _RangeEnd;	            
		private Nullable<Double> _Upfront;	            
		private Nullable<Double> _Backend;	            
		private Nullable<Double> _Bonus;	            
		private Nullable<Double> _RentBonus;	            
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
		public Nullable<Double> Upfront
		{	
			get{ return _Upfront; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Upfront, value, _Upfront);
				if (PropertyChanging(args))
				{
					_Upfront = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> Backend
		{	
			get{ return _Backend; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Backend, value, _Backend);
				if (PropertyChanging(args))
				{
					_Backend = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> Bonus
		{	
			get{ return _Bonus; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Bonus, value, _Bonus);
				if (PropertyChanging(args))
				{
					_Bonus = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> RentBonus
		{	
			get{ return _RentBonus; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RentBonus, value, _RentBonus);
				if (PropertyChanging(args))
				{
					_RentBonus = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  CommisionRangeBase Clone()
		{
			CommisionRangeBase newObj = new  CommisionRangeBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CommisionTypeId = this.CommisionTypeId;						
			newObj.CommisionSessionId = this.CommisionSessionId;						
			newObj.RangeStart = this.RangeStart;						
			newObj.RangeEnd = this.RangeEnd;						
			newObj.Upfront = this.Upfront;						
			newObj.Backend = this.Backend;						
			newObj.Bonus = this.Bonus;						
			newObj.RentBonus = this.RentBonus;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CommisionRangeBase.Property_Id, Id);				
			info.AddValue(CommisionRangeBase.Property_CommisionTypeId, CommisionTypeId);				
			info.AddValue(CommisionRangeBase.Property_CommisionSessionId, CommisionSessionId);				
			info.AddValue(CommisionRangeBase.Property_RangeStart, RangeStart);				
			info.AddValue(CommisionRangeBase.Property_RangeEnd, RangeEnd);				
			info.AddValue(CommisionRangeBase.Property_Upfront, Upfront);				
			info.AddValue(CommisionRangeBase.Property_Backend, Backend);				
			info.AddValue(CommisionRangeBase.Property_Bonus, Bonus);				
			info.AddValue(CommisionRangeBase.Property_RentBonus, RentBonus);				
		}
		#endregion

		
	}
}
