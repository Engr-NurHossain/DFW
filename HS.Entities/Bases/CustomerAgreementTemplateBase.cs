using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomerAgreementTemplateBase", Namespace = "http://www.hims-tech.com//entities")]
	public class CustomerAgreementTemplateBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			CustomerId = 2,
			Name = 3,
			Description = 4,
			BodyContent = 5,
			IsActive = 6,
			CreatedBy = 7,
			CreatedDate = 8,
			LastUpdatedBy = 9,
			LastUpdatedDate = 10,
			ReferenceTemplateId = 11,
			IsFileTemplate = 12
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_Name = "Name";		            
		public const string Property_Description = "Description";		            
		public const string Property_BodyContent = "BodyContent";		            
		public const string Property_IsActive = "IsActive";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_ReferenceTemplateId = "ReferenceTemplateId";		            
		public const string Property_IsFileTemplate = "IsFileTemplate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _CustomerId;	            
		private String _Name;	            
		private String _Description;	            
		private String _BodyContent;	            
		private Nullable<Boolean> _IsActive;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
		private Guid _LastUpdatedBy;	            
		private DateTime _LastUpdatedDate;	            
		private Nullable<Int32> _ReferenceTemplateId;	            
		private Nullable<Boolean> _IsFileTemplate;	            
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
		public String BodyContent
		{	
			get{ return _BodyContent; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BodyContent, value, _BodyContent);
				if (PropertyChanging(args))
				{
					_BodyContent = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsActive
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

		[DataMember]
		public Nullable<Int32> ReferenceTemplateId
		{	
			get{ return _ReferenceTemplateId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReferenceTemplateId, value, _ReferenceTemplateId);
				if (PropertyChanging(args))
				{
					_ReferenceTemplateId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsFileTemplate
		{	
			get{ return _IsFileTemplate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsFileTemplate, value, _IsFileTemplate);
				if (PropertyChanging(args))
				{
					_IsFileTemplate = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  CustomerAgreementTemplateBase Clone()
		{
			CustomerAgreementTemplateBase newObj = new  CustomerAgreementTemplateBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.Name = this.Name;						
			newObj.Description = this.Description;						
			newObj.BodyContent = this.BodyContent;						
			newObj.IsActive = this.IsActive;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.ReferenceTemplateId = this.ReferenceTemplateId;						
			newObj.IsFileTemplate = this.IsFileTemplate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomerAgreementTemplateBase.Property_Id, Id);				
			info.AddValue(CustomerAgreementTemplateBase.Property_CompanyId, CompanyId);				
			info.AddValue(CustomerAgreementTemplateBase.Property_CustomerId, CustomerId);				
			info.AddValue(CustomerAgreementTemplateBase.Property_Name, Name);				
			info.AddValue(CustomerAgreementTemplateBase.Property_Description, Description);				
			info.AddValue(CustomerAgreementTemplateBase.Property_BodyContent, BodyContent);				
			info.AddValue(CustomerAgreementTemplateBase.Property_IsActive, IsActive);				
			info.AddValue(CustomerAgreementTemplateBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(CustomerAgreementTemplateBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(CustomerAgreementTemplateBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(CustomerAgreementTemplateBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(CustomerAgreementTemplateBase.Property_ReferenceTemplateId, ReferenceTemplateId);				
			info.AddValue(CustomerAgreementTemplateBase.Property_IsFileTemplate, IsFileTemplate);				
		}
		#endregion

		
	}
}
