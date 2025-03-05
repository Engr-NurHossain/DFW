using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomerFileBase", Namespace = "http://www.hims-tech.com//entities")]
	public class CustomerFileBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			FileDescription = 1,
			Filename = 2,
			FileFullName = 3,
			Uploadeddate = 4,
			CustomerId = 5,
			CompanyId = 6,
			IsActive = 7,
			FileSize = 8,
			Tag = 9,
			InvoiceId = 10,
			GeeseFileType = 11,
			CreatedBy = 12,
			CreatedDate = 13,
			UpdatedBy = 14,
			UpdatedDate = 15,
			FileId = 16,
            AWSProcessStatus = 17,
            AWSUploadTS = 18,
            WMStatus = 19,
        }
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_FileDescription = "FileDescription";		            
		public const string Property_Filename = "Filename";		            
		public const string Property_FileFullName = "FileFullName";		            
		public const string Property_Uploadeddate = "Uploadeddate";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_IsActive = "IsActive";		            
		public const string Property_FileSize = "FileSize";		            
		public const string Property_Tag = "Tag";		            
		public const string Property_InvoiceId = "InvoiceId";		            
		public const string Property_GeeseFileType = "GeeseFileType";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_UpdatedBy = "UpdatedBy";		            
		public const string Property_UpdatedDate = "UpdatedDate";		            
		public const string Property_FileId = "FileId";
        public const string Property_AWSProcessStatus = "AWSProcessStatus";
        public const string Property_AWSUploadTS = "AWSUploadTS";
        public const string Property_WMStatus = "WMStatus";
        public const string Property_CustomerIntId = "CustomerIntId";

        #endregion

        #region Private Data Types
        private Int32 _Id;	            
		private String _FileDescription;	            
		private String _Filename;	            
		private String _FileFullName;	            
		private Nullable<DateTime> _Uploadeddate;	            
		private Int32 _CustomerIntId;
        private Guid _CustomerId;
        private Guid _CompanyId;	            
		private Nullable<Boolean> _IsActive;	            
		private Nullable<Double> _FileSize;	            
		private String _Tag;	            
		private String _InvoiceId;	            
		private String _GeeseFileType;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
		private Guid _UpdatedBy;	            
		private DateTime _UpdatedDate;	            
		private Guid _FileId;
        private string _AWSProcessStatus;        
        private Nullable<DateTime> _AWSUploadTS;
        private string _WMStatus;
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
        public int CustomerIntId
        {
            get { return _CustomerIntId; }
            set
            {
                PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CustomerIntId, value, _CustomerIntId);
                if (PropertyChanging(args))
                {
                    _CustomerIntId = value;
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

		[DataMember]
		public String Tag
		{	
			get{ return _Tag; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Tag, value, _Tag);
				if (PropertyChanging(args))
				{
					_Tag = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String InvoiceId
		{	
			get{ return _InvoiceId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InvoiceId, value, _InvoiceId);
				if (PropertyChanging(args))
				{
					_InvoiceId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String GeeseFileType
		{	
			get{ return _GeeseFileType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_GeeseFileType, value, _GeeseFileType);
				if (PropertyChanging(args))
				{
					_GeeseFileType = value;
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
		public Guid FileId
		{	
			get{ return _FileId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FileId, value, _FileId);
				if (PropertyChanging(args))
				{
					_FileId = value;
					PropertyChanged(args);					
				}	
			}
        }

        [DataMember]
        public string AWSProcessStatus
        {
            get { return _AWSProcessStatus; }
            set
            {
                PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AWSProcessStatus, value, _AWSProcessStatus);
                if (PropertyChanging(args))
                {
                    _AWSProcessStatus = value;
                    PropertyChanged(args);
                }
            }
        }

        [DataMember]
        public string WMStatus
        {
            get { return _WMStatus; }
            set
            {
                PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_WMStatus, value, _WMStatus);
                if (PropertyChanging(args))
                {
                    _WMStatus = value;
                    PropertyChanged(args);
                }
            }
        }

        [DataMember]
        public Nullable<DateTime> AWSUploadTS
        {
            get { return _AWSUploadTS; }
            set
            {
                PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AWSUploadTS, value, _AWSUploadTS);
                if (PropertyChanging(args))
                {
                    _AWSUploadTS = value;
                    PropertyChanged(args);
                }
            }
        }

        #endregion

        #region Cloning Base Objects
        public  CustomerFileBase Clone()
		{
			CustomerFileBase newObj = new  CustomerFileBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.FileDescription = this.FileDescription;						
			newObj.Filename = this.Filename;						
			newObj.FileFullName = this.FileFullName;						
			newObj.Uploadeddate = this.Uploadeddate;						
			newObj.CustomerId = this.CustomerId;						
			newObj.CompanyId = this.CompanyId;						
			newObj.IsActive = this.IsActive;						
			newObj.FileSize = this.FileSize;						
			newObj.Tag = this.Tag;						
			newObj.InvoiceId = this.InvoiceId;						
			newObj.GeeseFileType = this.GeeseFileType;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.UpdatedBy = this.UpdatedBy;						
			newObj.UpdatedDate = this.UpdatedDate;						
			newObj.FileId = this.FileId;            
            newObj.AWSProcessStatus = this.AWSProcessStatus;
			newObj.AWSUploadTS = this.AWSUploadTS;
            newObj.WMStatus = this.WMStatus;

            return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomerFileBase.Property_Id, Id);				
			info.AddValue(CustomerFileBase.Property_FileDescription, FileDescription);				
			info.AddValue(CustomerFileBase.Property_Filename, Filename);				
			info.AddValue(CustomerFileBase.Property_FileFullName, FileFullName);				
			info.AddValue(CustomerFileBase.Property_Uploadeddate, Uploadeddate);				
			info.AddValue(CustomerFileBase.Property_CustomerId, CustomerId);				
			info.AddValue(CustomerFileBase.Property_CompanyId, CompanyId);				
			info.AddValue(CustomerFileBase.Property_IsActive, IsActive);				
			info.AddValue(CustomerFileBase.Property_FileSize, FileSize);				
			info.AddValue(CustomerFileBase.Property_Tag, Tag);				
			info.AddValue(CustomerFileBase.Property_InvoiceId, InvoiceId);				
			info.AddValue(CustomerFileBase.Property_GeeseFileType, GeeseFileType);				
			info.AddValue(CustomerFileBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(CustomerFileBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(CustomerFileBase.Property_UpdatedBy, UpdatedBy);				
			info.AddValue(CustomerFileBase.Property_UpdatedDate, UpdatedDate);				
			info.AddValue(CustomerFileBase.Property_FileId, FileId);
            info.AddValue(CustomerFileBase.Property_AWSProcessStatus, AWSProcessStatus);
            info.AddValue(CustomerFileBase.Property_AWSUploadTS, AWSUploadTS);
            info.AddValue(CustomerFileBase.Property_WMStatus, WMStatus);
        }
		#endregion

		
	}
}
