using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CompanyBase", Namespace = "http://www.hims-tech.com//entities")]
	public class CompanyBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			CompanyName = 2,
			UserName = 3,
			EmailAdress = 4,
			FirstName = 5,
			LastName = 6,
			Phone = 7,
			Fax = 8,
			Address = 9,
			Street = 10,
			City = 11,
			State = 12,
			ZipCode = 13,
			Website = 14,
			CompanyType = 15,
			Note = 16,
			CompanyLogo = 17,
			CreatedDate = 18,
			CreatedBy = 19
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_CompanyName = "CompanyName";		            
		public const string Property_UserName = "UserName";		            
		public const string Property_EmailAdress = "EmailAdress";		            
		public const string Property_FirstName = "FirstName";		            
		public const string Property_LastName = "LastName";		            
		public const string Property_Phone = "Phone";		            
		public const string Property_Fax = "Fax";		            
		public const string Property_Address = "Address";		            
		public const string Property_Street = "Street";		            
		public const string Property_City = "City";		            
		public const string Property_State = "State";		            
		public const string Property_ZipCode = "ZipCode";		            
		public const string Property_Website = "Website";		            
		public const string Property_CompanyType = "CompanyType";		            
		public const string Property_Note = "Note";		            
		public const string Property_CompanyLogo = "CompanyLogo";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private String _CompanyName;	            
		private String _UserName;	            
		private String _EmailAdress;	            
		private String _FirstName;	            
		private String _LastName;	            
		private String _Phone;	            
		private String _Fax;	            
		private String _Address;	            
		private String _Street;	            
		private String _City;	            
		private String _State;	            
		private String _ZipCode;	            
		private String _Website;	            
		private String _CompanyType;	            
		private String _Note;	            
		private String _CompanyLogo;	            
		private DateTime _CreatedDate;	            
		private Guid _CreatedBy;	            
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
		public Guid CompanyId
		{	
			get{ return _CompanyId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CompanyId, value, _CompanyId);
				if (PropertyChanging(args))
				{
					_CompanyId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CompanyName
		{	
			get{ return _CompanyName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CompanyName, value, _CompanyName);
				if (PropertyChanging(args))
				{
					_CompanyName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String UserName
		{	
			get{ return _UserName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UserName, value, _UserName);
				if (PropertyChanging(args))
				{
					_UserName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String EmailAdress
		{	
			get{ return _EmailAdress; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EmailAdress, value, _EmailAdress);
				if (PropertyChanging(args))
				{
					_EmailAdress = value;
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
		public String LastName
		{	
			get{ return _LastName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastName, value, _LastName);
				if (PropertyChanging(args))
				{
					_LastName = value;
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
		public String Fax
		{	
			get{ return _Fax; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Fax, value, _Fax);
				if (PropertyChanging(args))
				{
					_Fax = value;
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
		public String Street
		{	
			get{ return _Street; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Street, value, _Street);
				if (PropertyChanging(args))
				{
					_Street = value;
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
		public String Website
		{	
			get{ return _Website; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Website, value, _Website);
				if (PropertyChanging(args))
				{
					_Website = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CompanyType
		{	
			get{ return _CompanyType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CompanyType, value, _CompanyType);
				if (PropertyChanging(args))
				{
					_CompanyType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Note
		{	
			get{ return _Note; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Note, value, _Note);
				if (PropertyChanging(args))
				{
					_Note = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CompanyLogo
		{	
			get{ return _CompanyLogo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CompanyLogo, value, _CompanyLogo);
				if (PropertyChanging(args))
				{
					_CompanyLogo = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime CreatedDate
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

		[DataMember]
		public Guid CreatedBy
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

		#endregion
		
		#region Cloning Base Objects
		public  CompanyBase Clone()
		{
			CompanyBase newObj = new  CompanyBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.CompanyName = this.CompanyName;						
			newObj.UserName = this.UserName;						
			newObj.EmailAdress = this.EmailAdress;						
			newObj.FirstName = this.FirstName;						
			newObj.LastName = this.LastName;						
			newObj.Phone = this.Phone;						
			newObj.Fax = this.Fax;						
			newObj.Address = this.Address;						
			newObj.Street = this.Street;						
			newObj.City = this.City;						
			newObj.State = this.State;						
			newObj.ZipCode = this.ZipCode;						
			newObj.Website = this.Website;						
			newObj.CompanyType = this.CompanyType;						
			newObj.Note = this.Note;						
			newObj.CompanyLogo = this.CompanyLogo;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CompanyBase.Property_Id, Id);				
			info.AddValue(CompanyBase.Property_CompanyId, CompanyId);				
			info.AddValue(CompanyBase.Property_CompanyName, CompanyName);				
			info.AddValue(CompanyBase.Property_UserName, UserName);				
			info.AddValue(CompanyBase.Property_EmailAdress, EmailAdress);				
			info.AddValue(CompanyBase.Property_FirstName, FirstName);				
			info.AddValue(CompanyBase.Property_LastName, LastName);				
			info.AddValue(CompanyBase.Property_Phone, Phone);				
			info.AddValue(CompanyBase.Property_Fax, Fax);				
			info.AddValue(CompanyBase.Property_Address, Address);				
			info.AddValue(CompanyBase.Property_Street, Street);				
			info.AddValue(CompanyBase.Property_City, City);				
			info.AddValue(CompanyBase.Property_State, State);				
			info.AddValue(CompanyBase.Property_ZipCode, ZipCode);				
			info.AddValue(CompanyBase.Property_Website, Website);				
			info.AddValue(CompanyBase.Property_CompanyType, CompanyType);				
			info.AddValue(CompanyBase.Property_Note, Note);				
			info.AddValue(CompanyBase.Property_CompanyLogo, CompanyLogo);				
			info.AddValue(CompanyBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(CompanyBase.Property_CreatedBy, CreatedBy);				
		}
		#endregion

		
	}
}
