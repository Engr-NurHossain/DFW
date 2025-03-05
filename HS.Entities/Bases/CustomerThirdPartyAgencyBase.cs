using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomerThirdPartyAgencyBase", Namespace = "http://www.hims-tech.com//entities")]
	public class CustomerThirdPartyAgencyBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CustomerId = 1,
			Agencytype = 2,
			Phone = 3,
			PermitNo = 4,
			PermType = 5,
			EffectiveDate = 6,
			ExpireDate = 7,
			Platform = 8,
			AgencyNo = 9,
			City = 10,
			State = 11,
			AgencyName = 12
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_Agencytype = "Agencytype";		            
		public const string Property_Phone = "Phone";		            
		public const string Property_PermitNo = "PermitNo";		            
		public const string Property_PermType = "PermType";		            
		public const string Property_EffectiveDate = "EffectiveDate";		            
		public const string Property_ExpireDate = "ExpireDate";		            
		public const string Property_Platform = "Platform";		            
		public const string Property_AgencyNo = "AgencyNo";		            
		public const string Property_City = "City";		            
		public const string Property_State = "State";		            
		public const string Property_AgencyName = "AgencyName";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CustomerId;	            
		private String _Agencytype;	            
		private String _Phone;	            
		private String _PermitNo;	            
		private String _PermType;	            
		private Nullable<DateTime> _EffectiveDate;	            
		private Nullable<DateTime> _ExpireDate;	            
		private String _Platform;	            
		private String _AgencyNo;	            
		private String _City;	            
		private String _State;	            
		private String _AgencyName;	            
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
		public Guid CustomerId
		{	
			get{ return _CustomerId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CustomerId, value, _CustomerId);
				if (PropertyChanging(args))
				{
					_CustomerId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Agencytype
		{	
			get{ return _Agencytype; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Agencytype, value, _Agencytype);
				if (PropertyChanging(args))
				{
					_Agencytype = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Phone
		{	
			get{ return _Phone; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Phone, value, _Phone);
				if (PropertyChanging(args))
				{
					_Phone = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PermitNo
		{	
			get{ return _PermitNo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PermitNo, value, _PermitNo);
				if (PropertyChanging(args))
				{
					_PermitNo = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PermType
		{	
			get{ return _PermType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PermType, value, _PermType);
				if (PropertyChanging(args))
				{
					_PermType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> EffectiveDate
		{	
			get{ return _EffectiveDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EffectiveDate, value, _EffectiveDate);
				if (PropertyChanging(args))
				{
					_EffectiveDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> ExpireDate
		{	
			get{ return _ExpireDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ExpireDate, value, _ExpireDate);
				if (PropertyChanging(args))
				{
					_ExpireDate = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  CustomerThirdPartyAgencyBase Clone()
		{
			CustomerThirdPartyAgencyBase newObj = new  CustomerThirdPartyAgencyBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CustomerId = this.CustomerId;						
			newObj.Agencytype = this.Agencytype;						
			newObj.Phone = this.Phone;						
			newObj.PermitNo = this.PermitNo;						
			newObj.PermType = this.PermType;						
			newObj.EffectiveDate = this.EffectiveDate;						
			newObj.ExpireDate = this.ExpireDate;						
			newObj.Platform = this.Platform;						
			newObj.AgencyNo = this.AgencyNo;						
			newObj.City = this.City;						
			newObj.State = this.State;						
			newObj.AgencyName = this.AgencyName;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomerThirdPartyAgencyBase.Property_Id, Id);				
			info.AddValue(CustomerThirdPartyAgencyBase.Property_CustomerId, CustomerId);				
			info.AddValue(CustomerThirdPartyAgencyBase.Property_Agencytype, Agencytype);				
			info.AddValue(CustomerThirdPartyAgencyBase.Property_Phone, Phone);				
			info.AddValue(CustomerThirdPartyAgencyBase.Property_PermitNo, PermitNo);				
			info.AddValue(CustomerThirdPartyAgencyBase.Property_PermType, PermType);				
			info.AddValue(CustomerThirdPartyAgencyBase.Property_EffectiveDate, EffectiveDate);				
			info.AddValue(CustomerThirdPartyAgencyBase.Property_ExpireDate, ExpireDate);				
			info.AddValue(CustomerThirdPartyAgencyBase.Property_Platform, Platform);				
			info.AddValue(CustomerThirdPartyAgencyBase.Property_AgencyNo, AgencyNo);				
			info.AddValue(CustomerThirdPartyAgencyBase.Property_City, City);				
			info.AddValue(CustomerThirdPartyAgencyBase.Property_State, State);				
			info.AddValue(CustomerThirdPartyAgencyBase.Property_AgencyName, AgencyName);				
		}
		#endregion

		
	}
}
