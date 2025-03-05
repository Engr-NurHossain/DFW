using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "OverrideBase", Namespace = "http://www.piistech.com//entities")]
	public class OverrideBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			Name = 1,
			Timeframe = 2,
			StartDayWk = 3,
			StartDayMonth = 4,
			IsActive = 5
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_Name = "Name";		            
		public const string Property_Timeframe = "Timeframe";		            
		public const string Property_StartDayWk = "StartDayWk";		            
		public const string Property_StartDayMonth = "StartDayMonth";		            
		public const string Property_IsActive = "IsActive";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _Name;	            
		private String _Timeframe;	            
		private String _StartDayWk;	            
		private String _StartDayMonth;	            
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
		public String Timeframe
		{	
			get{ return _Timeframe; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Timeframe, value, _Timeframe);
				if (PropertyChanging(args))
				{
					_Timeframe = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String StartDayWk
		{	
			get{ return _StartDayWk; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_StartDayWk, value, _StartDayWk);
				if (PropertyChanging(args))
				{
					_StartDayWk = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String StartDayMonth
		{	
			get{ return _StartDayMonth; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_StartDayMonth, value, _StartDayMonth);
				if (PropertyChanging(args))
				{
					_StartDayMonth = value;
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
		public  OverrideBase Clone()
		{
			OverrideBase newObj = new  OverrideBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.Name = this.Name;						
			newObj.Timeframe = this.Timeframe;						
			newObj.StartDayWk = this.StartDayWk;						
			newObj.StartDayMonth = this.StartDayMonth;						
			newObj.IsActive = this.IsActive;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(OverrideBase.Property_Id, Id);				
			info.AddValue(OverrideBase.Property_Name, Name);				
			info.AddValue(OverrideBase.Property_Timeframe, Timeframe);				
			info.AddValue(OverrideBase.Property_StartDayWk, StartDayWk);				
			info.AddValue(OverrideBase.Property_StartDayMonth, StartDayMonth);				
			info.AddValue(OverrideBase.Property_IsActive, IsActive);				
		}
		#endregion

		
	}
}
