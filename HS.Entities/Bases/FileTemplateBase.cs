using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "FileTemplateBase", Namespace = "http://www.hims-tech.com//entities")]
	public class FileTemplateBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			FileName = 1,
			FileDescription = 2,
			FileBody = 3,
			CreatedBy = 4,
			CreatedDate = 5,
			LastUpdatedBy = 6,
			LastUpdatedDate = 7,
			CompanyId = 8,
			IsCustomerSignRequired = 9
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_FileName = "FileName";		            
		public const string Property_FileDescription = "FileDescription";		            
		public const string Property_FileBody = "FileBody";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_IsCustomerSignRequired = "IsCustomerSignRequired";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _FileName;	            
		private String _FileDescription;	            
		private String _FileBody;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
		private Guid _LastUpdatedBy;	            
		private DateTime _LastUpdatedDate;	            
		private Guid _CompanyId;	            
		private Boolean _IsCustomerSignRequired;	            
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
		public String FileName
		{	
			get{ return _FileName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FileName, value, _FileName);
				if (PropertyChanging(args))
				{
					_FileName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String FileDescription
		{	
			get{ return _FileDescription; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FileDescription, value, _FileDescription);
				if (PropertyChanging(args))
				{
					_FileDescription = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String FileBody
		{	
			get{ return _FileBody; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FileBody, value, _FileBody);
				if (PropertyChanging(args))
				{
					_FileBody = value;
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
		public Boolean IsCustomerSignRequired
		{	
			get{ return _IsCustomerSignRequired; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsCustomerSignRequired, value, _IsCustomerSignRequired);
				if (PropertyChanging(args))
				{
					_IsCustomerSignRequired = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  FileTemplateBase Clone()
		{
			FileTemplateBase newObj = new  FileTemplateBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.FileName = this.FileName;						
			newObj.FileDescription = this.FileDescription;						
			newObj.FileBody = this.FileBody;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.CompanyId = this.CompanyId;						
			newObj.IsCustomerSignRequired = this.IsCustomerSignRequired;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(FileTemplateBase.Property_Id, Id);				
			info.AddValue(FileTemplateBase.Property_FileName, FileName);				
			info.AddValue(FileTemplateBase.Property_FileDescription, FileDescription);				
			info.AddValue(FileTemplateBase.Property_FileBody, FileBody);				
			info.AddValue(FileTemplateBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(FileTemplateBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(FileTemplateBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(FileTemplateBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(FileTemplateBase.Property_CompanyId, CompanyId);				
			info.AddValue(FileTemplateBase.Property_IsCustomerSignRequired, IsCustomerSignRequired);				
		}
		#endregion

		
	}
}
