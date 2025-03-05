using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "BillFileBase", Namespace = "http://www.piistech.com//entities")]
	public class BillFileBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			FileDescription = 1,
			Filename = 2,
			FileFullName = 3,
			Uploadeddate = 4,
			BillNo = 5,
			CompanyId = 6,
			IsActive = 7,
			FileSize = 8
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_FileDescription = "FileDescription";		            
		public const string Property_Filename = "Filename";		            
		public const string Property_FileFullName = "FileFullName";		            
		public const string Property_Uploadeddate = "Uploadeddate";		            
		public const string Property_BillNo = "BillNo";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_IsActive = "IsActive";		            
		public const string Property_FileSize = "FileSize";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _FileDescription;	            
		private String _Filename;	            
		private String _FileFullName;	            
		private Nullable<DateTime> _Uploadeddate;	            
		private String _BillNo;	            
		private Guid _CompanyId;	            
		private Nullable<Boolean> _IsActive;	            
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
		public String BillNo
		{	
			get{ return _BillNo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BillNo, value, _BillNo);
				if (PropertyChanging(args))
				{
					_BillNo = value;
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
		public  BillFileBase Clone()
		{
			BillFileBase newObj = new  BillFileBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.FileDescription = this.FileDescription;						
			newObj.Filename = this.Filename;						
			newObj.FileFullName = this.FileFullName;						
			newObj.Uploadeddate = this.Uploadeddate;						
			newObj.BillNo = this.BillNo;						
			newObj.CompanyId = this.CompanyId;						
			newObj.IsActive = this.IsActive;						
			newObj.FileSize = this.FileSize;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(BillFileBase.Property_Id, Id);				
			info.AddValue(BillFileBase.Property_FileDescription, FileDescription);				
			info.AddValue(BillFileBase.Property_Filename, Filename);				
			info.AddValue(BillFileBase.Property_FileFullName, FileFullName);				
			info.AddValue(BillFileBase.Property_Uploadeddate, Uploadeddate);				
			info.AddValue(BillFileBase.Property_BillNo, BillNo);				
			info.AddValue(BillFileBase.Property_CompanyId, CompanyId);				
			info.AddValue(BillFileBase.Property_IsActive, IsActive);				
			info.AddValue(BillFileBase.Property_FileSize, FileSize);				
		}
		#endregion

		
	}
}
