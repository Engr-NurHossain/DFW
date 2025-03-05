using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "ServiceAreaZipcodeBase", Namespace = "http://www.piistech.com//entities")]
	public class ServiceAreaZipcodeBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			Zipcode = 1
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_Zipcode = "Zipcode";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _Zipcode;	            
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
		public String Zipcode
		{	
			get{ return _Zipcode; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Zipcode, value, _Zipcode);
				if (PropertyChanging(args))
				{
					_Zipcode = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  ServiceAreaZipcodeBase Clone()
		{
			ServiceAreaZipcodeBase newObj = new  ServiceAreaZipcodeBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.Zipcode = this.Zipcode;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(ServiceAreaZipcodeBase.Property_Id, Id);				
			info.AddValue(ServiceAreaZipcodeBase.Property_Zipcode, Zipcode);				
		}
		#endregion

		
	}
}
