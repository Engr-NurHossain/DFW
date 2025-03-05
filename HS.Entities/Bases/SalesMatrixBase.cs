using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "SalesMatrixBase", Namespace = "http://www.piistech.com//entities")]
	public class SalesMatrixBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			SalesMatrixId = 1,
			Type = 2,
			Min = 3,
			Max = 4,
			UserX = 5,
			Difference = 6
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_SalesMatrixId = "SalesMatrixId";		            
		public const string Property_Type = "Type";		            
		public const string Property_Min = "Min";		            
		public const string Property_Max = "Max";		            
		public const string Property_UserX = "UserX";		            
		public const string Property_Difference = "Difference";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _SalesMatrixId;	            
		private String _Type;	            
		private Double _Min;	            
		private Double _Max;	            
		private Double _UserX;	            
		private Nullable<Double> _Difference;	            
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
		public Guid SalesMatrixId
		{	
			get{ return _SalesMatrixId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SalesMatrixId, value, _SalesMatrixId);
				if (PropertyChanging(args))
				{
					_SalesMatrixId = value;
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
		public Double Min
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
		public Double Max
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

		[DataMember]
		public Double UserX
		{	
			get{ return _UserX; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UserX, value, _UserX);
				if (PropertyChanging(args))
				{
					_UserX = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> Difference
		{	
			get{ return _Difference; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Difference, value, _Difference);
				if (PropertyChanging(args))
				{
					_Difference = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  SalesMatrixBase Clone()
		{
			SalesMatrixBase newObj = new  SalesMatrixBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.SalesMatrixId = this.SalesMatrixId;						
			newObj.Type = this.Type;						
			newObj.Min = this.Min;						
			newObj.Max = this.Max;						
			newObj.UserX = this.UserX;						
			newObj.Difference = this.Difference;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(SalesMatrixBase.Property_Id, Id);				
			info.AddValue(SalesMatrixBase.Property_SalesMatrixId, SalesMatrixId);				
			info.AddValue(SalesMatrixBase.Property_Type, Type);				
			info.AddValue(SalesMatrixBase.Property_Min, Min);				
			info.AddValue(SalesMatrixBase.Property_Max, Max);				
			info.AddValue(SalesMatrixBase.Property_UserX, UserX);				
			info.AddValue(SalesMatrixBase.Property_Difference, Difference);				
		}
		#endregion

		
	}
}
