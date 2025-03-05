using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "EstimatorFileBase", Namespace = "http://www.piistech.com//entities")]
	public class EstimatorFileBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			EstimatorId = 1,
			FileDescription = 2,
			Filename = 3,
			FileFullName = 4,
			FileSize = 5,
			IsActive = 6,
			CreatedBy = 7,
			CreatedDate = 8,
			UpdatedBy = 9,
			UpdatedDate = 10,
			EstimatorType = 11
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_EstimatorId = "EstimatorId";		            
		public const string Property_FileDescription = "FileDescription";		            
		public const string Property_Filename = "Filename";		            
		public const string Property_FileFullName = "FileFullName";		            
		public const string Property_FileSize = "FileSize";		            
		public const string Property_IsActive = "IsActive";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_UpdatedBy = "UpdatedBy";		            
		public const string Property_UpdatedDate = "UpdatedDate";		            
		public const string Property_EstimatorType = "EstimatorType";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _EstimatorId;	            
		private String _FileDescription;	            
		private String _Filename;	            
		private String _FileFullName;	            
		private Nullable<Double> _FileSize;	            
		private Nullable<Boolean> _IsActive;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
		private Guid _UpdatedBy;	            
		private DateTime _UpdatedDate;	            
		private String _EstimatorType;	            
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
		public String EstimatorId
		{	
			get{ return _EstimatorId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EstimatorId, value, _EstimatorId);
				if (PropertyChanging(args))
				{
					_EstimatorId = value;
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
		public Guid UpdatedBy
		{	
			get{ return _UpdatedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UpdatedBy, value, _UpdatedBy);
				if (PropertyChanging(args))
				{
					_UpdatedBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime UpdatedDate
		{	
			get{ return _UpdatedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UpdatedDate, value, _UpdatedDate);
				if (PropertyChanging(args))
				{
					_UpdatedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String EstimatorType
		{	
			get{ return _EstimatorType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EstimatorType, value, _EstimatorType);
				if (PropertyChanging(args))
				{
					_EstimatorType = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  EstimatorFileBase Clone()
		{
			EstimatorFileBase newObj = new  EstimatorFileBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.EstimatorId = this.EstimatorId;						
			newObj.FileDescription = this.FileDescription;						
			newObj.Filename = this.Filename;						
			newObj.FileFullName = this.FileFullName;						
			newObj.FileSize = this.FileSize;						
			newObj.IsActive = this.IsActive;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.UpdatedBy = this.UpdatedBy;						
			newObj.UpdatedDate = this.UpdatedDate;						
			newObj.EstimatorType = this.EstimatorType;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(EstimatorFileBase.Property_Id, Id);				
			info.AddValue(EstimatorFileBase.Property_EstimatorId, EstimatorId);				
			info.AddValue(EstimatorFileBase.Property_FileDescription, FileDescription);				
			info.AddValue(EstimatorFileBase.Property_Filename, Filename);				
			info.AddValue(EstimatorFileBase.Property_FileFullName, FileFullName);				
			info.AddValue(EstimatorFileBase.Property_FileSize, FileSize);				
			info.AddValue(EstimatorFileBase.Property_IsActive, IsActive);				
			info.AddValue(EstimatorFileBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(EstimatorFileBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(EstimatorFileBase.Property_UpdatedBy, UpdatedBy);				
			info.AddValue(EstimatorFileBase.Property_UpdatedDate, UpdatedDate);				
			info.AddValue(EstimatorFileBase.Property_EstimatorType, EstimatorType);				
		}
		#endregion

		
	}
}