using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "SmartPackageEquipmentServiceBase", Namespace = "http://www.piistech.com//entities")]
	public class SmartPackageEquipmentServiceBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			PackageId = 2,
			EquipmentId = 3,
			IsFree = 4,
			EptNo = 5,
			Type = 6,
			Price = 7,
			Status = 8,
			LastUpdatedBy = 9,
			LastUpdatedDate = 10,
			SmartPackageEquipmentServiceId = 11,
			OriginalPrice = 12
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_PackageId = "PackageId";		            
		public const string Property_EquipmentId = "EquipmentId";		            
		public const string Property_IsFree = "IsFree";		            
		public const string Property_EptNo = "EptNo";		            
		public const string Property_Type = "Type";		            
		public const string Property_Price = "Price";		            
		public const string Property_Status = "Status";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_SmartPackageEquipmentServiceId = "SmartPackageEquipmentServiceId";		            
		public const string Property_OriginalPrice = "OriginalPrice";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _PackageId;	            
		private Guid _EquipmentId;	            
		private Boolean _IsFree;	            
		private Nullable<Int32> _EptNo;	            
		private String _Type;	            
		private Nullable<Double> _Price;	            
		private Boolean _Status;	            
		private Guid _LastUpdatedBy;	            
		private DateTime _LastUpdatedDate;	            
		private Guid _SmartPackageEquipmentServiceId;	            
		private Nullable<Double> _OriginalPrice;	            
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
		public Guid PackageId
		{	
			get{ return _PackageId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PackageId, value, _PackageId);
				if (PropertyChanging(args))
				{
					_PackageId = value;
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
		public Boolean IsFree
		{	
			get{ return _IsFree; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsFree, value, _IsFree);
				if (PropertyChanging(args))
				{
					_IsFree = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> EptNo
		{	
			get{ return _EptNo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EptNo, value, _EptNo);
				if (PropertyChanging(args))
				{
					_EptNo = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Type
		{	
			get{ return _Type; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Type, value, _Type);
				if (PropertyChanging(args))
				{
					_Type = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> Price
		{	
			get{ return _Price; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Price, value, _Price);
				if (PropertyChanging(args))
				{
					_Price = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean Status
		{	
			get{ return _Status; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Status, value, _Status);
				if (PropertyChanging(args))
				{
					_Status = value;
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
		public Guid SmartPackageEquipmentServiceId
		{	
			get{ return _SmartPackageEquipmentServiceId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SmartPackageEquipmentServiceId, value, _SmartPackageEquipmentServiceId);
				if (PropertyChanging(args))
				{
					_SmartPackageEquipmentServiceId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> OriginalPrice
		{	
			get{ return _OriginalPrice; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OriginalPrice, value, _OriginalPrice);
				if (PropertyChanging(args))
				{
					_OriginalPrice = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  SmartPackageEquipmentServiceBase Clone()
		{
			SmartPackageEquipmentServiceBase newObj = new  SmartPackageEquipmentServiceBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.PackageId = this.PackageId;						
			newObj.EquipmentId = this.EquipmentId;						
			newObj.IsFree = this.IsFree;						
			newObj.EptNo = this.EptNo;						
			newObj.Type = this.Type;						
			newObj.Price = this.Price;						
			newObj.Status = this.Status;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.SmartPackageEquipmentServiceId = this.SmartPackageEquipmentServiceId;						
			newObj.OriginalPrice = this.OriginalPrice;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(SmartPackageEquipmentServiceBase.Property_Id, Id);				
			info.AddValue(SmartPackageEquipmentServiceBase.Property_CompanyId, CompanyId);				
			info.AddValue(SmartPackageEquipmentServiceBase.Property_PackageId, PackageId);				
			info.AddValue(SmartPackageEquipmentServiceBase.Property_EquipmentId, EquipmentId);				
			info.AddValue(SmartPackageEquipmentServiceBase.Property_IsFree, IsFree);				
			info.AddValue(SmartPackageEquipmentServiceBase.Property_EptNo, EptNo);				
			info.AddValue(SmartPackageEquipmentServiceBase.Property_Type, Type);				
			info.AddValue(SmartPackageEquipmentServiceBase.Property_Price, Price);				
			info.AddValue(SmartPackageEquipmentServiceBase.Property_Status, Status);				
			info.AddValue(SmartPackageEquipmentServiceBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(SmartPackageEquipmentServiceBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(SmartPackageEquipmentServiceBase.Property_SmartPackageEquipmentServiceId, SmartPackageEquipmentServiceId);				
			info.AddValue(SmartPackageEquipmentServiceBase.Property_OriginalPrice, OriginalPrice);				
		}
		#endregion

		
	}
}
