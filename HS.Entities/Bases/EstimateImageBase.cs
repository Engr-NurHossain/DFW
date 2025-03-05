using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "EstimateImageBase", Namespace = "http://www.hims-tech.com//entities")]
	public class EstimateImageBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			CustomerId = 2,
			InvoiceId = 3,
			ImageLoc = 4,
			ImageType = 5,
			SignDate = 6,
			UploadedDate = 7,
			CreatedBy = 8,
			IsDocument = 9,
			Size = 10
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_InvoiceId = "InvoiceId";		            
		public const string Property_ImageLoc = "ImageLoc";		            
		public const string Property_ImageType = "ImageType";		            
		public const string Property_SignDate = "SignDate";		            
		public const string Property_UploadedDate = "UploadedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_IsDocument = "IsDocument";		            
		public const string Property_Size = "Size";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _CustomerId;	            
		private String _InvoiceId;	            
		private String _ImageLoc;	            
		private String _ImageType;	            
		private Nullable<DateTime> _SignDate;	            
		private DateTime _UploadedDate;	            
		private Guid _CreatedBy;	            
		private Boolean _IsDocument;	            
		private Double _Size;	            
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
		public String ImageLoc
		{	
			get{ return _ImageLoc; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ImageLoc, value, _ImageLoc);
				if (PropertyChanging(args))
				{
					_ImageLoc = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ImageType
		{	
			get{ return _ImageType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ImageType, value, _ImageType);
				if (PropertyChanging(args))
				{
					_ImageType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> SignDate
		{	
			get{ return _SignDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SignDate, value, _SignDate);
				if (PropertyChanging(args))
				{
					_SignDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime UploadedDate
		{	
			get{ return _UploadedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UploadedDate, value, _UploadedDate);
				if (PropertyChanging(args))
				{
					_UploadedDate = value;
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
		public Boolean IsDocument
		{	
			get{ return _IsDocument; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsDocument, value, _IsDocument);
				if (PropertyChanging(args))
				{
					_IsDocument = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Double Size
		{	
			get{ return _Size; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Size, value, _Size);
				if (PropertyChanging(args))
				{
					_Size = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  EstimateImageBase Clone()
		{
			EstimateImageBase newObj = new  EstimateImageBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.InvoiceId = this.InvoiceId;						
			newObj.ImageLoc = this.ImageLoc;						
			newObj.ImageType = this.ImageType;						
			newObj.SignDate = this.SignDate;						
			newObj.UploadedDate = this.UploadedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.IsDocument = this.IsDocument;						
			newObj.Size = this.Size;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(EstimateImageBase.Property_Id, Id);				
			info.AddValue(EstimateImageBase.Property_CompanyId, CompanyId);				
			info.AddValue(EstimateImageBase.Property_CustomerId, CustomerId);				
			info.AddValue(EstimateImageBase.Property_InvoiceId, InvoiceId);				
			info.AddValue(EstimateImageBase.Property_ImageLoc, ImageLoc);				
			info.AddValue(EstimateImageBase.Property_ImageType, ImageType);				
			info.AddValue(EstimateImageBase.Property_SignDate, SignDate);				
			info.AddValue(EstimateImageBase.Property_UploadedDate, UploadedDate);				
			info.AddValue(EstimateImageBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(EstimateImageBase.Property_IsDocument, IsDocument);				
			info.AddValue(EstimateImageBase.Property_Size, Size);				
		}
		#endregion

		
	}
}
