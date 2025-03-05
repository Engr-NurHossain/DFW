using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "PackageCustomerBase", Namespace = "http://www.piistech.com//entities")]
	public class PackageCustomerBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			CustomerId = 2,
			PackageId = 3,
			SmartSystemTypeId = 4,
			SmartInstallTypeId = 5,
			ManufacturerId = 6,
			NonConformingFee = 7,
			ActivationFee = 8,
			WarrentyAvailable = 9,
			SmartSystemType = 10,
			LabourFee = 11,
			IsNFTTicket = 12
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_PackageId = "PackageId";		            
		public const string Property_SmartSystemTypeId = "SmartSystemTypeId";		            
		public const string Property_SmartInstallTypeId = "SmartInstallTypeId";		            
		public const string Property_ManufacturerId = "ManufacturerId";		            
		public const string Property_NonConformingFee = "NonConformingFee";		            
		public const string Property_ActivationFee = "ActivationFee";		            
		public const string Property_WarrentyAvailable = "WarrentyAvailable";		            
		public const string Property_SmartSystemType = "SmartSystemType";		            
		public const string Property_LabourFee = "LabourFee";		            
		public const string Property_IsNFTTicket = "IsNFTTicket";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _CustomerId;	            
		private Guid _PackageId;	            
		private Nullable<Int32> _SmartSystemTypeId;	            
		private Nullable<Int32> _SmartInstallTypeId;	            
		private Guid _ManufacturerId;	            
		private Nullable<Double> _NonConformingFee;	            
		private Nullable<Double> _ActivationFee;	            
		private Nullable<Boolean> _WarrentyAvailable;	            
		private String _SmartSystemType;	            
		private Nullable<Double> _LabourFee;	            
		private Nullable<Boolean> _IsNFTTicket;	            
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
		public Nullable<Int32> SmartSystemTypeId
		{	
			get{ return _SmartSystemTypeId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SmartSystemTypeId, value, _SmartSystemTypeId);
				if (PropertyChanging(args))
				{
					_SmartSystemTypeId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> SmartInstallTypeId
		{	
			get{ return _SmartInstallTypeId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SmartInstallTypeId, value, _SmartInstallTypeId);
				if (PropertyChanging(args))
				{
					_SmartInstallTypeId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid ManufacturerId
		{	
			get{ return _ManufacturerId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ManufacturerId, value, _ManufacturerId);
				if (PropertyChanging(args))
				{
					_ManufacturerId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> NonConformingFee
		{	
			get{ return _NonConformingFee; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_NonConformingFee, value, _NonConformingFee);
				if (PropertyChanging(args))
				{
					_NonConformingFee = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> ActivationFee
		{	
			get{ return _ActivationFee; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ActivationFee, value, _ActivationFee);
				if (PropertyChanging(args))
				{
					_ActivationFee = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> WarrentyAvailable
		{	
			get{ return _WarrentyAvailable; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_WarrentyAvailable, value, _WarrentyAvailable);
				if (PropertyChanging(args))
				{
					_WarrentyAvailable = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SmartSystemType
		{	
			get{ return _SmartSystemType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SmartSystemType, value, _SmartSystemType);
				if (PropertyChanging(args))
				{
					_SmartSystemType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> LabourFee
		{	
			get{ return _LabourFee; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LabourFee, value, _LabourFee);
				if (PropertyChanging(args))
				{
					_LabourFee = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsNFTTicket
		{	
			get{ return _IsNFTTicket; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsNFTTicket, value, _IsNFTTicket);
				if (PropertyChanging(args))
				{
					_IsNFTTicket = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  PackageCustomerBase Clone()
		{
			PackageCustomerBase newObj = new  PackageCustomerBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.PackageId = this.PackageId;						
			newObj.SmartSystemTypeId = this.SmartSystemTypeId;						
			newObj.SmartInstallTypeId = this.SmartInstallTypeId;						
			newObj.ManufacturerId = this.ManufacturerId;						
			newObj.NonConformingFee = this.NonConformingFee;						
			newObj.ActivationFee = this.ActivationFee;						
			newObj.WarrentyAvailable = this.WarrentyAvailable;						
			newObj.SmartSystemType = this.SmartSystemType;						
			newObj.LabourFee = this.LabourFee;						
			newObj.IsNFTTicket = this.IsNFTTicket;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(PackageCustomerBase.Property_Id, Id);				
			info.AddValue(PackageCustomerBase.Property_CompanyId, CompanyId);				
			info.AddValue(PackageCustomerBase.Property_CustomerId, CustomerId);				
			info.AddValue(PackageCustomerBase.Property_PackageId, PackageId);				
			info.AddValue(PackageCustomerBase.Property_SmartSystemTypeId, SmartSystemTypeId);				
			info.AddValue(PackageCustomerBase.Property_SmartInstallTypeId, SmartInstallTypeId);				
			info.AddValue(PackageCustomerBase.Property_ManufacturerId, ManufacturerId);				
			info.AddValue(PackageCustomerBase.Property_NonConformingFee, NonConformingFee);				
			info.AddValue(PackageCustomerBase.Property_ActivationFee, ActivationFee);				
			info.AddValue(PackageCustomerBase.Property_WarrentyAvailable, WarrentyAvailable);				
			info.AddValue(PackageCustomerBase.Property_SmartSystemType, SmartSystemType);				
			info.AddValue(PackageCustomerBase.Property_LabourFee, LabourFee);				
			info.AddValue(PackageCustomerBase.Property_IsNFTTicket, IsNFTTicket);				
		}
		#endregion

		
	}
}
