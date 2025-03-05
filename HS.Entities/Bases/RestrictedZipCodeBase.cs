using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "RestrictedZipCodeBase", Namespace = "http://www.hims-tech.com//entities")]
	public class RestrictedZipCodeBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			Zipcode = 1,
			CreatedBy = 2,
			CreatedDate = 3
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_Zipcode = "Zipcode";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _Zipcode;	            
		private String _CreatedBy;	            
		private Nullable<DateTime> _CreatedDate;	            
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

		[DataMember]
		public String CreatedBy
		{	
			get{ return _CreatedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreatedBy, value, _CreatedBy);
				if (PropertyChanging(args))
				{
					_CreatedBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> CreatedDate
		{	
			get{ return _CreatedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreatedDate, value, _CreatedDate);
				if (PropertyChanging(args))
				{
					_CreatedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  RestrictedZipCodeBase Clone()
		{
			RestrictedZipCodeBase newObj = new  RestrictedZipCodeBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.Zipcode = this.Zipcode;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(RestrictedZipCodeBase.Property_Id, Id);				
			info.AddValue(RestrictedZipCodeBase.Property_Zipcode, Zipcode);				
			info.AddValue(RestrictedZipCodeBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(RestrictedZipCodeBase.Property_CreatedDate, CreatedDate);				
		}
		#endregion

		
	}
}
