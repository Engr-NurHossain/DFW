using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CreditClassBase", Namespace = "http://www.piistech.com//entities")]
	public class CreditClassBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			Name = 1,
			Min = 2,
			Max = 3
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_Name = "Name";		            
		public const string Property_Min = "Min";		            
		public const string Property_Max = "Max";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _Name;	            
		private Nullable<Int32> _Min;	            
		private Nullable<Int32> _Max;	            
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
		public Nullable<Int32> Min
		{	
			get{ return _Min; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Min, value, _Min);
				if (PropertyChanging(args))
				{
					_Min = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> Max
		{	
			get{ return _Max; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Max, value, _Max);
				if (PropertyChanging(args))
				{
					_Max = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  CreditClassBase Clone()
		{
			CreditClassBase newObj = new  CreditClassBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.Name = this.Name;						
			newObj.Min = this.Min;						
			newObj.Max = this.Max;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CreditClassBase.Property_Id, Id);				
			info.AddValue(CreditClassBase.Property_Name, Name);				
			info.AddValue(CreditClassBase.Property_Min, Min);				
			info.AddValue(CreditClassBase.Property_Max, Max);				
		}
		#endregion

		
	}
}
