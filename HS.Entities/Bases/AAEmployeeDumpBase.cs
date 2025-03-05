using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "AAEmployeeDumpBase", Namespace = "http://www.hims-tech.com//entities")]
	public class AAEmployeeDumpBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			Name = 1,
			Email = 2,
			Mobile = 3,
			Address = 4,
			BrinksUserName = 5,
			Roles = 6,
			Status = 7,
			Created = 8,
			Updated = 9
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_Name = "Name";		            
		public const string Property_Email = "Email";		            
		public const string Property_Mobile = "Mobile";		            
		public const string Property_Address = "Address";		            
		public const string Property_BrinksUserName = "BrinksUserName";		            
		public const string Property_Roles = "Roles";		            
		public const string Property_Status = "Status";		            
		public const string Property_Created = "Created";		            
		public const string Property_Updated = "Updated";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _Name;	            
		private String _Email;	            
		private String _Mobile;	            
		private String _Address;	            
		private String _BrinksUserName;	            
		private String _Roles;	            
		private String _Status;	            
		private Nullable<DateTime> _Created;	            
		private Nullable<DateTime> _Updated;	            
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
		public String Email
		{	
			get{ return _Email; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Email, value, _Email);
				if (PropertyChanging(args))
				{
					_Email = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Mobile
		{	
			get{ return _Mobile; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Mobile, value, _Mobile);
				if (PropertyChanging(args))
				{
					_Mobile = value;
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
		public String BrinksUserName
		{	
			get{ return _BrinksUserName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BrinksUserName, value, _BrinksUserName);
				if (PropertyChanging(args))
				{
					_BrinksUserName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Roles
		{	
			get{ return _Roles; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Roles, value, _Roles);
				if (PropertyChanging(args))
				{
					_Roles = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Status
		{	
			get{ return _Status; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Status, value, _Status);
				if (PropertyChanging(args))
				{
					_Status = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> Created
		{	
			get{ return _Created; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Created, value, _Created);
				if (PropertyChanging(args))
				{
					_Created = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> Updated
		{	
			get{ return _Updated; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Updated, value, _Updated);
				if (PropertyChanging(args))
				{
					_Updated = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  AAEmployeeDumpBase Clone()
		{
			AAEmployeeDumpBase newObj = new  AAEmployeeDumpBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.Name = this.Name;						
			newObj.Email = this.Email;						
			newObj.Mobile = this.Mobile;						
			newObj.Address = this.Address;						
			newObj.BrinksUserName = this.BrinksUserName;						
			newObj.Roles = this.Roles;						
			newObj.Status = this.Status;						
			newObj.Created = this.Created;						
			newObj.Updated = this.Updated;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(AAEmployeeDumpBase.Property_Id, Id);				
			info.AddValue(AAEmployeeDumpBase.Property_Name, Name);				
			info.AddValue(AAEmployeeDumpBase.Property_Email, Email);				
			info.AddValue(AAEmployeeDumpBase.Property_Mobile, Mobile);				
			info.AddValue(AAEmployeeDumpBase.Property_Address, Address);				
			info.AddValue(AAEmployeeDumpBase.Property_BrinksUserName, BrinksUserName);				
			info.AddValue(AAEmployeeDumpBase.Property_Roles, Roles);				
			info.AddValue(AAEmployeeDumpBase.Property_Status, Status);				
			info.AddValue(AAEmployeeDumpBase.Property_Created, Created);				
			info.AddValue(AAEmployeeDumpBase.Property_Updated, Updated);				
		}
		#endregion

		
	}
}
