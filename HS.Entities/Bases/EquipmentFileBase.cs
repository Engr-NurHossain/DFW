﻿using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "EquipmentFileBase", Namespace = "http://www.piistech.com//entities")]
	public class EquipmentFileBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			EquipmentId = 2,
			FileDescription = 3,
			Filename = 4,
			FileFullName = 5,
			FileType = 6,
			Uploadeddate = 7,
			IsActive = 8,
			FileSize = 9
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_EquipmentId = "EquipmentId";		            
		public const string Property_FileDescription = "FileDescription";		            
		public const string Property_Filename = "Filename";		            
		public const string Property_FileFullName = "FileFullName";		            
		public const string Property_FileType = "FileType";		            
		public const string Property_Uploadeddate = "Uploadeddate";		            
		public const string Property_IsActive = "IsActive";		            
		public const string Property_FileSize = "FileSize";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _EquipmentId;	            
		private String _FileDescription;	            
		private String _Filename;	            
		private String _FileFullName;	            
		private String _FileType;	            
		private Nullable<DateTime> _Uploadeddate;	            
		private Boolean _IsActive;	            
		private Nullable<Double> _FileSize;	            
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
		public Guid EquipmentId
		{	
			get{ return _EquipmentId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EquipmentId, value, _EquipmentId);
				if (PropertyChanging(args))
				{
					_EquipmentId = value;
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
		public String Filename
		{	
			get{ return _Filename; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Filename, value, _Filename);
				if (PropertyChanging(args))
				{
					_Filename = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String FileFullName
		{	
			get{ return _FileFullName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FileFullName, value, _FileFullName);
				if (PropertyChanging(args))
				{
					_FileFullName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String FileType
		{	
			get{ return _FileType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FileType, value, _FileType);
				if (PropertyChanging(args))
				{
					_FileType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> Uploadeddate
		{	
			get{ return _Uploadeddate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Uploadeddate, value, _Uploadeddate);
				if (PropertyChanging(args))
				{
					_Uploadeddate = value;
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
		public Nullable<Double> FileSize
		{	
			get{ return _FileSize; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FileSize, value, _FileSize);
				if (PropertyChanging(args))
				{
					_FileSize = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  EquipmentFileBase Clone()
		{
			EquipmentFileBase newObj = new  EquipmentFileBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.EquipmentId = this.EquipmentId;						
			newObj.FileDescription = this.FileDescription;						
			newObj.Filename = this.Filename;						
			newObj.FileFullName = this.FileFullName;						
			newObj.FileType = this.FileType;						
			newObj.Uploadeddate = this.Uploadeddate;						
			newObj.IsActive = this.IsActive;						
			newObj.FileSize = this.FileSize;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(EquipmentFileBase.Property_Id, Id);				
			info.AddValue(EquipmentFileBase.Property_CompanyId, CompanyId);				
			info.AddValue(EquipmentFileBase.Property_EquipmentId, EquipmentId);				
			info.AddValue(EquipmentFileBase.Property_FileDescription, FileDescription);				
			info.AddValue(EquipmentFileBase.Property_Filename, Filename);				
			info.AddValue(EquipmentFileBase.Property_FileFullName, FileFullName);				
			info.AddValue(EquipmentFileBase.Property_FileType, FileType);				
			info.AddValue(EquipmentFileBase.Property_Uploadeddate, Uploadeddate);				
			info.AddValue(EquipmentFileBase.Property_IsActive, IsActive);				
			info.AddValue(EquipmentFileBase.Property_FileSize, FileSize);				
		}
		#endregion

		
	}
}
