using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "LanguageBase", Namespace = "http://www.piistech.com//entities")]
	public class LanguageBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			Name = 2,
			LanguageCulture = 3,
			Rtl = 4,
			Published = 5,
			DisplayOrder = 6
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_Name = "Name";		            
		public const string Property_LanguageCulture = "LanguageCulture";		            
		public const string Property_Rtl = "Rtl";		            
		public const string Property_Published = "Published";		            
		public const string Property_DisplayOrder = "DisplayOrder";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private String _Name;	            
		private String _LanguageCulture;	            
		private Boolean _Rtl;	            
		private Boolean _Published;	            
		private Int32 _DisplayOrder;	            
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
		public String LanguageCulture
		{	
			get{ return _LanguageCulture; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LanguageCulture, value, _LanguageCulture);
				if (PropertyChanging(args))
				{
					_LanguageCulture = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean Rtl
		{	
			get{ return _Rtl; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Rtl, value, _Rtl);
				if (PropertyChanging(args))
				{
					_Rtl = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean Published
		{	
			get{ return _Published; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Published, value, _Published);
				if (PropertyChanging(args))
				{
					_Published = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Int32 DisplayOrder
		{	
			get{ return _DisplayOrder; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DisplayOrder, value, _DisplayOrder);
				if (PropertyChanging(args))
				{
					_DisplayOrder = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  LanguageBase Clone()
		{
			LanguageBase newObj = new  LanguageBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.Name = this.Name;						
			newObj.LanguageCulture = this.LanguageCulture;						
			newObj.Rtl = this.Rtl;						
			newObj.Published = this.Published;						
			newObj.DisplayOrder = this.DisplayOrder;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(LanguageBase.Property_Id, Id);				
			info.AddValue(LanguageBase.Property_CompanyId, CompanyId);				
			info.AddValue(LanguageBase.Property_Name, Name);				
			info.AddValue(LanguageBase.Property_LanguageCulture, LanguageCulture);				
			info.AddValue(LanguageBase.Property_Rtl, Rtl);				
			info.AddValue(LanguageBase.Property_Published, Published);				
			info.AddValue(LanguageBase.Property_DisplayOrder, DisplayOrder);				
		}
		#endregion

		
	}
}
