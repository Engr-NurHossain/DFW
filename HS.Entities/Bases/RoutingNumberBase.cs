using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "RoutingNumberBase", Namespace = "http://www.piistech.com//entities")]
	public class RoutingNumberBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			RoutingNumber = 1,
			BankName = 2,
			City = 3,
			State = 4,
			Address = 5,
			ContactNo = 6,
			ZipCode = 7
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_RoutingNumber = "RoutingNumber";		            
		public const string Property_BankName = "BankName";		            
		public const string Property_City = "City";		            
		public const string Property_State = "State";		            
		public const string Property_Address = "Address";		            
		public const string Property_ContactNo = "ContactNo";		            
		public const string Property_ZipCode = "ZipCode";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _RoutingNumber;	            
		private String _BankName;	            
		private String _City;	            
		private String _State;	            
		private String _Address;	            
		private String _ContactNo;	            
		private String _ZipCode;	            
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
		public String RoutingNumber
		{	
			get{ return _RoutingNumber; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RoutingNumber, value, _RoutingNumber);
				if (PropertyChanging(args))
				{
					_RoutingNumber = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String BankName
		{	
			get{ return _BankName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BankName, value, _BankName);
				if (PropertyChanging(args))
				{
					_BankName = value;
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
		public String Address
		{	
			get{ return _Address; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Address, value, _Address);
				if (PropertyChanging(args))
				{
					_Address = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ContactNo
		{	
			get{ return _ContactNo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ContactNo, value, _ContactNo);
				if (PropertyChanging(args))
				{
					_ContactNo = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  RoutingNumberBase Clone()
		{
			RoutingNumberBase newObj = new  RoutingNumberBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.RoutingNumber = this.RoutingNumber;						
			newObj.BankName = this.BankName;						
			newObj.City = this.City;						
			newObj.State = this.State;						
			newObj.Address = this.Address;						
			newObj.ContactNo = this.ContactNo;						
			newObj.ZipCode = this.ZipCode;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(RoutingNumberBase.Property_Id, Id);				
			info.AddValue(RoutingNumberBase.Property_RoutingNumber, RoutingNumber);				
			info.AddValue(RoutingNumberBase.Property_BankName, BankName);				
			info.AddValue(RoutingNumberBase.Property_City, City);				
			info.AddValue(RoutingNumberBase.Property_State, State);				
			info.AddValue(RoutingNumberBase.Property_Address, Address);				
			info.AddValue(RoutingNumberBase.Property_ContactNo, ContactNo);				
			info.AddValue(RoutingNumberBase.Property_ZipCode, ZipCode);				
		}
		#endregion

		
	}
}
