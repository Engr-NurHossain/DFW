using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CityZipCodeBase", Namespace = "http://www.piistech.com//entities")]
	public class CityZipCodeBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			ZipCode = 1,
			City = 2,
			State = 3,
			County = 4,
			AreaCode = 5,
			Latitude = 6,
			Longitude = 7,
			TimeZone = 8,
			Elevation = 9
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_ZipCode = "ZipCode";		            
		public const string Property_City = "City";		            
		public const string Property_State = "State";		            
		public const string Property_County = "County";		            
		public const string Property_AreaCode = "AreaCode";		            
		public const string Property_Latitude = "Latitude";		            
		public const string Property_Longitude = "Longitude";		            
		public const string Property_TimeZone = "TimeZone";		            
		public const string Property_Elevation = "Elevation";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _ZipCode;	            
		private String _City;	            
		private String _State;	            
		private String _County;	            
		private String _AreaCode;	            
		private Nullable<Double> _Latitude;	            
		private Nullable<Double> _Longitude;	            
		private String _TimeZone;	            
		private Nullable<Int32> _Elevation;	            
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
		public String ZipCode
		{	
			get{ return _ZipCode; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ZipCode, value, _ZipCode);
				if (PropertyChanging(args))
				{
					_ZipCode = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String City
		{	
			get{ return _City; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_City, value, _City);
				if (PropertyChanging(args))
				{
					_City = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String State
		{	
			get{ return _State; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_State, value, _State);
				if (PropertyChanging(args))
				{
					_State = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String County
		{	
			get{ return _County; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_County, value, _County);
				if (PropertyChanging(args))
				{
					_County = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AreaCode
		{	
			get{ return _AreaCode; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AreaCode, value, _AreaCode);
				if (PropertyChanging(args))
				{
					_AreaCode = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> Latitude
		{	
			get{ return _Latitude; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Latitude, value, _Latitude);
				if (PropertyChanging(args))
				{
					_Latitude = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> Longitude
		{	
			get{ return _Longitude; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Longitude, value, _Longitude);
				if (PropertyChanging(args))
				{
					_Longitude = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TimeZone
		{	
			get{ return _TimeZone; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TimeZone, value, _TimeZone);
				if (PropertyChanging(args))
				{
					_TimeZone = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> Elevation
		{	
			get{ return _Elevation; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Elevation, value, _Elevation);
				if (PropertyChanging(args))
				{
					_Elevation = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  CityZipCodeBase Clone()
		{
			CityZipCodeBase newObj = new  CityZipCodeBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.ZipCode = this.ZipCode;						
			newObj.City = this.City;						
			newObj.State = this.State;						
			newObj.County = this.County;						
			newObj.AreaCode = this.AreaCode;						
			newObj.Latitude = this.Latitude;						
			newObj.Longitude = this.Longitude;						
			newObj.TimeZone = this.TimeZone;						
			newObj.Elevation = this.Elevation;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CityZipCodeBase.Property_Id, Id);				
			info.AddValue(CityZipCodeBase.Property_ZipCode, ZipCode);				
			info.AddValue(CityZipCodeBase.Property_City, City);				
			info.AddValue(CityZipCodeBase.Property_State, State);				
			info.AddValue(CityZipCodeBase.Property_County, County);				
			info.AddValue(CityZipCodeBase.Property_AreaCode, AreaCode);				
			info.AddValue(CityZipCodeBase.Property_Latitude, Latitude);				
			info.AddValue(CityZipCodeBase.Property_Longitude, Longitude);				
			info.AddValue(CityZipCodeBase.Property_TimeZone, TimeZone);				
			info.AddValue(CityZipCodeBase.Property_Elevation, Elevation);				
		}
		#endregion

		
	}
}
