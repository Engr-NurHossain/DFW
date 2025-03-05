using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CompanyBranchBase", Namespace = "http://www.piistech.com//entities")]
	public class CompanyBranchBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			Name = 2,
			Street = 3,
			City = 4,
			State = 5,
			ZipCode = 6,
			Logo = 7,
			ColorLogo = 8,
			EmailLogo = 9,
			TimeZone = 10,
			Tax = 11,
			IsMainBranch = 12,
			Division = 13,
			Region = 14
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_Name = "Name";		            
		public const string Property_Street = "Street";		            
		public const string Property_City = "City";		            
		public const string Property_State = "State";		            
		public const string Property_ZipCode = "ZipCode";		            
		public const string Property_Logo = "Logo";		            
		public const string Property_ColorLogo = "ColorLogo";		            
		public const string Property_EmailLogo = "EmailLogo";		            
		public const string Property_TimeZone = "TimeZone";		            
		public const string Property_Tax = "Tax";		            
		public const string Property_IsMainBranch = "IsMainBranch";		            
		public const string Property_Division = "Division";		            
		public const string Property_Region = "Region";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private String _Name;	            
		private String _Street;	            
		private String _City;	            
		private String _State;	            
		private String _ZipCode;	            
		private String _Logo;	            
		private String _ColorLogo;	            
		private String _EmailLogo;	            
		private String _TimeZone;	            
		private Nullable<Double> _Tax;	            
		private Nullable<Boolean> _IsMainBranch;	            
		private String _Division;	            
		private String _Region;	            
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
		public String Logo
		{	
			get{ return _Logo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Logo, value, _Logo);
				if (PropertyChanging(args))
				{
					_Logo = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ColorLogo
		{	
			get{ return _ColorLogo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ColorLogo, value, _ColorLogo);
				if (PropertyChanging(args))
				{
					_ColorLogo = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String EmailLogo
		{	
			get{ return _EmailLogo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EmailLogo, value, _EmailLogo);
				if (PropertyChanging(args))
				{
					_EmailLogo = value;
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
		public Nullable<Double> Tax
		{	
			get{ return _Tax; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Tax, value, _Tax);
				if (PropertyChanging(args))
				{
					_Tax = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsMainBranch
		{	
			get{ return _IsMainBranch; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsMainBranch, value, _IsMainBranch);
				if (PropertyChanging(args))
				{
					_IsMainBranch = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Division
		{	
			get{ return _Division; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Division, value, _Division);
				if (PropertyChanging(args))
				{
					_Division = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Region
		{	
			get{ return _Region; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Region, value, _Region);
				if (PropertyChanging(args))
				{
					_Region = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  CompanyBranchBase Clone()
		{
			CompanyBranchBase newObj = new  CompanyBranchBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.Name = this.Name;						
			newObj.Street = this.Street;						
			newObj.City = this.City;						
			newObj.State = this.State;						
			newObj.ZipCode = this.ZipCode;						
			newObj.Logo = this.Logo;						
			newObj.ColorLogo = this.ColorLogo;						
			newObj.EmailLogo = this.EmailLogo;						
			newObj.TimeZone = this.TimeZone;						
			newObj.Tax = this.Tax;						
			newObj.IsMainBranch = this.IsMainBranch;						
			newObj.Division = this.Division;						
			newObj.Region = this.Region;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CompanyBranchBase.Property_Id, Id);				
			info.AddValue(CompanyBranchBase.Property_CompanyId, CompanyId);				
			info.AddValue(CompanyBranchBase.Property_Name, Name);				
			info.AddValue(CompanyBranchBase.Property_Street, Street);				
			info.AddValue(CompanyBranchBase.Property_City, City);				
			info.AddValue(CompanyBranchBase.Property_State, State);				
			info.AddValue(CompanyBranchBase.Property_ZipCode, ZipCode);				
			info.AddValue(CompanyBranchBase.Property_Logo, Logo);				
			info.AddValue(CompanyBranchBase.Property_ColorLogo, ColorLogo);				
			info.AddValue(CompanyBranchBase.Property_EmailLogo, EmailLogo);				
			info.AddValue(CompanyBranchBase.Property_TimeZone, TimeZone);				
			info.AddValue(CompanyBranchBase.Property_Tax, Tax);				
			info.AddValue(CompanyBranchBase.Property_IsMainBranch, IsMainBranch);				
			info.AddValue(CompanyBranchBase.Property_Division, Division);				
			info.AddValue(CompanyBranchBase.Property_Region, Region);				
		}
		#endregion

		
	}
}
