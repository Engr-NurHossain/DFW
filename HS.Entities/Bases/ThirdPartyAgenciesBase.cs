using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "ThirdPartyAgenciesBase", Namespace = "http://www.hims-tech.com//entities")]
	public class ThirdPartyAgenciesBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			AgencyNo = 1,
			AgencyType = 2,
			AgencyName = 3,
			City = 4,
			State = 5,
			Zipcode = 6,
			Phone1 = 7,
			Phone2 = 8,
			ChangeDate = 9,
			Platform = 10
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_AgencyNo = "AgencyNo";		            
		public const string Property_AgencyType = "AgencyType";		            
		public const string Property_AgencyName = "AgencyName";		            
		public const string Property_City = "City";		            
		public const string Property_State = "State";		            
		public const string Property_Zipcode = "Zipcode";		            
		public const string Property_Phone1 = "Phone1";		            
		public const string Property_Phone2 = "Phone2";		            
		public const string Property_ChangeDate = "ChangeDate";		            
		public const string Property_Platform = "Platform";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _AgencyNo;	            
		private String _AgencyType;	            
		private String _AgencyName;	            
		private String _City;	            
		private String _State;	            
		private String _Zipcode;	            
		private String _Phone1;	            
		private String _Phone2;	            
		private Nullable<DateTime> _ChangeDate;	            
		private String _Platform;	            
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
		public String AgencyNo
		{	
			get{ return _AgencyNo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AgencyNo, value, _AgencyNo);
				if (PropertyChanging(args))
				{
					_AgencyNo = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AgencyType
		{	
			get{ return _AgencyType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AgencyType, value, _AgencyType);
				if (PropertyChanging(args))
				{
					_AgencyType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String AgencyName
		{	
			get{ return _AgencyName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AgencyName, value, _AgencyName);
				if (PropertyChanging(args))
				{
					_AgencyName = value;
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
		public String Phone1
		{	
			get{ return _Phone1; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Phone1, value, _Phone1);
				if (PropertyChanging(args))
				{
					_Phone1 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Phone2
		{	
			get{ return _Phone2; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Phone2, value, _Phone2);
				if (PropertyChanging(args))
				{
					_Phone2 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> ChangeDate
		{	
			get{ return _ChangeDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ChangeDate, value, _ChangeDate);
				if (PropertyChanging(args))
				{
					_ChangeDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Platform
		{	
			get{ return _Platform; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Platform, value, _Platform);
				if (PropertyChanging(args))
				{
					_Platform = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  ThirdPartyAgenciesBase Clone()
		{
			ThirdPartyAgenciesBase newObj = new  ThirdPartyAgenciesBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.AgencyNo = this.AgencyNo;						
			newObj.AgencyType = this.AgencyType;						
			newObj.AgencyName = this.AgencyName;						
			newObj.City = this.City;						
			newObj.State = this.State;						
			newObj.Zipcode = this.Zipcode;						
			newObj.Phone1 = this.Phone1;						
			newObj.Phone2 = this.Phone2;						
			newObj.ChangeDate = this.ChangeDate;						
			newObj.Platform = this.Platform;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(ThirdPartyAgenciesBase.Property_Id, Id);				
			info.AddValue(ThirdPartyAgenciesBase.Property_AgencyNo, AgencyNo);				
			info.AddValue(ThirdPartyAgenciesBase.Property_AgencyType, AgencyType);				
			info.AddValue(ThirdPartyAgenciesBase.Property_AgencyName, AgencyName);				
			info.AddValue(ThirdPartyAgenciesBase.Property_City, City);				
			info.AddValue(ThirdPartyAgenciesBase.Property_State, State);				
			info.AddValue(ThirdPartyAgenciesBase.Property_Zipcode, Zipcode);				
			info.AddValue(ThirdPartyAgenciesBase.Property_Phone1, Phone1);				
			info.AddValue(ThirdPartyAgenciesBase.Property_Phone2, Phone2);				
			info.AddValue(ThirdPartyAgenciesBase.Property_ChangeDate, ChangeDate);				
			info.AddValue(ThirdPartyAgenciesBase.Property_Platform, Platform);				
		}
		#endregion

		
	}
}
