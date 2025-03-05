using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomerMigrationBase", Namespace = "http://www.piistech.com//entities")]
	public class CustomerMigrationBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			CustomerId = 2,
			RefenrenceId = 3,
			Platform = 4,
			Note = 5,
			CreatedDate = 6,
			CreatedBy = 7
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_RefenrenceId = "RefenrenceId";		            
		public const string Property_Platform = "Platform";		            
		public const string Property_Note = "Note";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _CustomerId;	            
		private Int32 _RefenrenceId;	            
		private String _Platform;	            
		private String _Note;	            
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
		public Int32 RefenrenceId
		{	
			get{ return _RefenrenceId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RefenrenceId, value, _RefenrenceId);
				if (PropertyChanging(args))
				{
					_RefenrenceId = value;
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
		public  CustomerMigrationBase Clone()
		{
			CustomerMigrationBase newObj = new  CustomerMigrationBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.RefenrenceId = this.RefenrenceId;						
			newObj.Platform = this.Platform;						
			newObj.Note = this.Note;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomerMigrationBase.Property_Id, Id);				
			info.AddValue(CustomerMigrationBase.Property_CompanyId, CompanyId);				
			info.AddValue(CustomerMigrationBase.Property_CustomerId, CustomerId);				
			info.AddValue(CustomerMigrationBase.Property_RefenrenceId, RefenrenceId);				
			info.AddValue(CustomerMigrationBase.Property_Platform, Platform);				
			info.AddValue(CustomerMigrationBase.Property_Note, Note);				
			info.AddValue(CustomerMigrationBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(CustomerMigrationBase.Property_CreatedBy, CreatedBy);				
		}
		#endregion

		
	}
}
