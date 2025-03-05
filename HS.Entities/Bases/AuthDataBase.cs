using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "AuthDataBase", Namespace = "http://www.piistech.com//entities")]
	public class AuthDataBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			AuthRefId = 1,
			FirstName = 2,
			Lastname = 3,
			Name = 4,
			Amount = 5,
			CustomerProfileId = 6,
			PaymentprofileId = 7,
			CustomerNo = 8
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_AuthRefId = "AuthRefId";		            
		public const string Property_FirstName = "FirstName";		            
		public const string Property_Lastname = "Lastname";		            
		public const string Property_Name = "Name";		            
		public const string Property_Amount = "Amount";		            
		public const string Property_CustomerProfileId = "CustomerProfileId";		            
		public const string Property_PaymentprofileId = "PaymentprofileId";		            
		public const string Property_CustomerNo = "CustomerNo";		            
		#endregion
		
		#region Private Data Types
		private Int64 _Id;	            
		private String _AuthRefId;	            
		private String _FirstName;	            
		private String _Lastname;	            
		private String _Name;	            
		private Nullable<Double> _Amount;	            
		private String _CustomerProfileId;	            
		private String _PaymentprofileId;	            
		private String _CustomerNo;	            
		#endregion
		
		#region Properties		
		[DataMember]
		public Int64 Id
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
		public String AuthRefId
		{	
			get{ return _AuthRefId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AuthRefId, value, _AuthRefId);
				if (PropertyChanging(args))
				{
					_AuthRefId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String FirstName
		{	
			get{ return _FirstName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FirstName, value, _FirstName);
				if (PropertyChanging(args))
				{
					_FirstName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Lastname
		{	
			get{ return _Lastname; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Lastname, value, _Lastname);
				if (PropertyChanging(args))
				{
					_Lastname = value;
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
		public Nullable<Double> Amount
		{	
			get{ return _Amount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Amount, value, _Amount);
				if (PropertyChanging(args))
				{
					_Amount = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CustomerProfileId
		{	
			get{ return _CustomerProfileId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CustomerProfileId, value, _CustomerProfileId);
				if (PropertyChanging(args))
				{
					_CustomerProfileId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String PaymentprofileId
		{	
			get{ return _PaymentprofileId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PaymentprofileId, value, _PaymentprofileId);
				if (PropertyChanging(args))
				{
					_PaymentprofileId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CustomerNo
		{	
			get{ return _CustomerNo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CustomerNo, value, _CustomerNo);
				if (PropertyChanging(args))
				{
					_CustomerNo = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  AuthDataBase Clone()
		{
			AuthDataBase newObj = new  AuthDataBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.AuthRefId = this.AuthRefId;						
			newObj.FirstName = this.FirstName;						
			newObj.Lastname = this.Lastname;						
			newObj.Name = this.Name;						
			newObj.Amount = this.Amount;						
			newObj.CustomerProfileId = this.CustomerProfileId;						
			newObj.PaymentprofileId = this.PaymentprofileId;						
			newObj.CustomerNo = this.CustomerNo;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(AuthDataBase.Property_Id, Id);				
			info.AddValue(AuthDataBase.Property_AuthRefId, AuthRefId);				
			info.AddValue(AuthDataBase.Property_FirstName, FirstName);				
			info.AddValue(AuthDataBase.Property_Lastname, Lastname);				
			info.AddValue(AuthDataBase.Property_Name, Name);				
			info.AddValue(AuthDataBase.Property_Amount, Amount);				
			info.AddValue(AuthDataBase.Property_CustomerProfileId, CustomerProfileId);				
			info.AddValue(AuthDataBase.Property_PaymentprofileId, PaymentprofileId);				
			info.AddValue(AuthDataBase.Property_CustomerNo, CustomerNo);				
		}
		#endregion

		
	}
}
