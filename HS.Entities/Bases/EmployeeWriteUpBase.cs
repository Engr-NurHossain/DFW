using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "EmployeeWriteUpBase", Namespace = "http://www.piistech.com//entities")]
	public class EmployeeWriteUpBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			WriteupId = 1,
			UserId = 2,
			Supervisor = 3,
			WriteupDate = 4,
			Category = 5,
			Description = 6,
			CreatedBy = 7,
			CreatedDate = 8,
			FileName = 9,
			FilePath = 10
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_WriteupId = "WriteupId";		            
		public const string Property_UserId = "UserId";		            
		public const string Property_Supervisor = "Supervisor";		            
		public const string Property_WriteupDate = "WriteupDate";		            
		public const string Property_Category = "Category";		            
		public const string Property_Description = "Description";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_FileName = "FileName";		            
		public const string Property_FilePath = "FilePath";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _WriteupId;	            
		private Guid _UserId;	            
		private Guid _Supervisor;	            
		private Nullable<DateTime> _WriteupDate;	            
		private String _Category;	            
		private String _Description;	            
		private Guid _CreatedBy;	            
		private Nullable<DateTime> _CreatedDate;	            
		private String _FileName;	            
		private String _FilePath;	            
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
		public Guid WriteupId
		{	
			get{ return _WriteupId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_WriteupId, value, _WriteupId);
				if (PropertyChanging(args))
				{
					_WriteupId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid UserId
		{	
			get{ return _UserId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UserId, value, _UserId);
				if (PropertyChanging(args))
				{
					_UserId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid Supervisor
		{	
			get{ return _Supervisor; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Supervisor, value, _Supervisor);
				if (PropertyChanging(args))
				{
					_Supervisor = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> WriteupDate
		{	
			get{ return _WriteupDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_WriteupDate, value, _WriteupDate);
				if (PropertyChanging(args))
				{
					_WriteupDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Category
		{	
			get{ return _Category; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Category, value, _Category);
				if (PropertyChanging(args))
				{
					_Category = value;
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
		public Nullable<DateTime> CreatedDate
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
		public String FilePath
		{	
			get{ return _FilePath; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FilePath, value, _FilePath);
				if (PropertyChanging(args))
				{
					_FilePath = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  EmployeeWriteUpBase Clone()
		{
			EmployeeWriteUpBase newObj = new  EmployeeWriteUpBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.WriteupId = this.WriteupId;						
			newObj.UserId = this.UserId;						
			newObj.Supervisor = this.Supervisor;						
			newObj.WriteupDate = this.WriteupDate;						
			newObj.Category = this.Category;						
			newObj.Description = this.Description;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.FileName = this.FileName;						
			newObj.FilePath = this.FilePath;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(EmployeeWriteUpBase.Property_Id, Id);				
			info.AddValue(EmployeeWriteUpBase.Property_WriteupId, WriteupId);				
			info.AddValue(EmployeeWriteUpBase.Property_UserId, UserId);				
			info.AddValue(EmployeeWriteUpBase.Property_Supervisor, Supervisor);				
			info.AddValue(EmployeeWriteUpBase.Property_WriteupDate, WriteupDate);				
			info.AddValue(EmployeeWriteUpBase.Property_Category, Category);				
			info.AddValue(EmployeeWriteUpBase.Property_Description, Description);				
			info.AddValue(EmployeeWriteUpBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(EmployeeWriteUpBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(EmployeeWriteUpBase.Property_FileName, FileName);				
			info.AddValue(EmployeeWriteUpBase.Property_FilePath, FilePath);				
		}
		#endregion

		
	}
}
