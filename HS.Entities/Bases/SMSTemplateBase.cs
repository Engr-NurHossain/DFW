using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "SMSTemplateBase", Namespace = "http://www.piistech.com//entities")]
	public class SMSTemplateBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			TemplateKey = 2,
			Name = 3,
			Description = 4,
			ToNumber = 5,
			Body = 6,
			IsActive = 7,
			LastUpdatedBy = 8,
			LastUpdatedDate = 9
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_TemplateKey = "TemplateKey";		            
		public const string Property_Name = "Name";		            
		public const string Property_Description = "Description";		            
		public const string Property_ToNumber = "ToNumber";		            
		public const string Property_Body = "Body";		            
		public const string Property_IsActive = "IsActive";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private String _TemplateKey;	            
		private String _Name;	            
		private String _Description;	            
		private String _ToNumber;	            
		private String _Body;	            
		private Boolean _IsActive;	            
		private Guid _LastUpdatedBy;	            
		private DateTime _LastUpdatedDate;	            
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
		public String TemplateKey
		{	
			get{ return _TemplateKey; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TemplateKey, value, _TemplateKey);
				if (PropertyChanging(args))
				{
					_TemplateKey = value;
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
		public String Description
		{	
			get{ return _Description; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Description, value, _Description);
				if (PropertyChanging(args))
				{
					_Description = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ToNumber
		{	
			get{ return _ToNumber; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ToNumber, value, _ToNumber);
				if (PropertyChanging(args))
				{
					_ToNumber = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Body
		{	
			get{ return _Body; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Body, value, _Body);
				if (PropertyChanging(args))
				{
					_Body = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean IsActive
		{	
			get{ return _IsActive; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsActive, value, _IsActive);
				if (PropertyChanging(args))
				{
					_IsActive = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid LastUpdatedBy
		{	
			get{ return _LastUpdatedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastUpdatedBy, value, _LastUpdatedBy);
				if (PropertyChanging(args))
				{
					_LastUpdatedBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime LastUpdatedDate
		{	
			get{ return _LastUpdatedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastUpdatedDate, value, _LastUpdatedDate);
				if (PropertyChanging(args))
				{
					_LastUpdatedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  SMSTemplateBase Clone()
		{
			SMSTemplateBase newObj = new  SMSTemplateBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.TemplateKey = this.TemplateKey;						
			newObj.Name = this.Name;						
			newObj.Description = this.Description;						
			newObj.ToNumber = this.ToNumber;						
			newObj.Body = this.Body;						
			newObj.IsActive = this.IsActive;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(SMSTemplateBase.Property_Id, Id);				
			info.AddValue(SMSTemplateBase.Property_CompanyId, CompanyId);				
			info.AddValue(SMSTemplateBase.Property_TemplateKey, TemplateKey);				
			info.AddValue(SMSTemplateBase.Property_Name, Name);				
			info.AddValue(SMSTemplateBase.Property_Description, Description);				
			info.AddValue(SMSTemplateBase.Property_ToNumber, ToNumber);				
			info.AddValue(SMSTemplateBase.Property_Body, Body);				
			info.AddValue(SMSTemplateBase.Property_IsActive, IsActive);				
			info.AddValue(SMSTemplateBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(SMSTemplateBase.Property_LastUpdatedDate, LastUpdatedDate);				
		}
		#endregion

		
	}
}
