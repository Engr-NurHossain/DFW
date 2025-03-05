using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomerPackageEqpBase", Namespace = "http://www.hims-tech.com//entities")]
	public class CustomerPackageEqpBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			CustomerId = 2,
			PackageId = 3,
			EquipmentId = 4,
			IsIncluded = 5,
			IsDevice = 6,
			IsOptionalEqp = 7,
			Quantity = 8,
			UnitPrice = 9,
			DiscountUnitPricce = 10,
			DiscountPckage = 11,
			Total = 12,
			IsServiceEquipment = 13,
			ServiceId = 14,
			IsTransfered = 15,
			IsEqpExist = 16,
			IsPackageEqp = 17,
			IsNonCommissionable = 18,
			AppointmentIntId = 19,
			IsInvoice = 20,
			AppointmentEquipmentIntId = 21,
			Discountpercent=22,
			DiscountInAmount=23
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_PackageId = "PackageId";		            
		public const string Property_EquipmentId = "EquipmentId";		            
		public const string Property_IsIncluded = "IsIncluded";		            
		public const string Property_IsDevice = "IsDevice";		            
		public const string Property_IsOptionalEqp = "IsOptionalEqp";		            
		public const string Property_Quantity = "Quantity";		            
		public const string Property_UnitPrice = "UnitPrice";		            
		public const string Property_DiscountUnitPricce = "DiscountUnitPricce";		            
		public const string Property_DiscountPckage = "DiscountPckage";
		public const string Property_Total = "Total";		            
		public const string Property_IsServiceEquipment = "IsServiceEquipment";		            
		public const string Property_ServiceId = "ServiceId";		            
		public const string Property_IsTransfered = "IsTransfered";		            
		public const string Property_IsEqpExist = "IsEqpExist";		            
		public const string Property_IsPackageEqp = "IsPackageEqp";		            
		public const string Property_IsNonCommissionable = "IsNonCommissionable";		            
		public const string Property_AppointmentIntId = "AppointmentIntId";		            
		public const string Property_IsInvoice = "IsInvoice";		            
		public const string Property_AppointmentEquipmentIntId = "AppointmentEquipmentIntId";
		public const string Property_DiscountPercent = "DiscountPercent";
		public const string Property_DiscountInAmount = "DiscountInAmount";

		#endregion

		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _CustomerId;	            
		private Guid _PackageId;	            
		private Guid _EquipmentId;	            
		private Boolean _IsIncluded;	            
		private Boolean _IsDevice;	            
		private Boolean _IsOptionalEqp;	            
		private Nullable<Int32> _Quantity;	            
		private Nullable<Double> _UnitPrice;	            
		private Nullable<Double> _DiscountUnitPricce;	            
		private Nullable<Double> _DiscountPckage;
		private Nullable<Double> _DiscountPckagePercentage;
		private Nullable<Double> _DiscountPckageAmount;
		private Nullable<Double> _Total;	            
		private Nullable<Boolean> _IsServiceEquipment;	            
		private Guid _ServiceId;	            
		private Nullable<Boolean> _IsTransfered;	            
		private Nullable<Boolean> _IsEqpExist;	            
		private Nullable<Boolean> _IsPackageEqp;	            
		private Nullable<Boolean> _IsNonCommissionable;	            
		private Nullable<Int32> _AppointmentIntId;	            
		private Nullable<Boolean> _IsInvoice;	            
		private Nullable<Int32> _AppointmentEquipmentIntId;

        private Nullable<Double> _DiscountPercent;
        private Nullable<Double> _DiscountInAmount;
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
		public Boolean IsIncluded
		{	
			get{ return _IsIncluded; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsIncluded, value, _IsIncluded);
				if (PropertyChanging(args))
				{
					_IsIncluded = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean IsDevice
		{	
			get{ return _IsDevice; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsDevice, value, _IsDevice);
				if (PropertyChanging(args))
				{
					_IsDevice = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean IsOptionalEqp
		{	
			get{ return _IsOptionalEqp; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsOptionalEqp, value, _IsOptionalEqp);
				if (PropertyChanging(args))
				{
					_IsOptionalEqp = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> Quantity
		{	
			get{ return _Quantity; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Quantity, value, _Quantity);
				if (PropertyChanging(args))
				{
					_Quantity = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> UnitPrice
		{	
			get{ return _UnitPrice; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UnitPrice, value, _UnitPrice);
				if (PropertyChanging(args))
				{
					_UnitPrice = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> DiscountUnitPricce
		{	
			get{ return _DiscountUnitPricce; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DiscountUnitPricce, value, _DiscountUnitPricce);
				if (PropertyChanging(args))
				{
					_DiscountUnitPricce = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> DiscountPckage
		{	
			get{ return _DiscountPckage; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DiscountPckage, value, _DiscountPckage);
				if (PropertyChanging(args))
				{
					_DiscountPckage = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> DiscountPckagePercentage
		{
			get { return _DiscountPckagePercentage; }
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DiscountPercent, value, _DiscountPckagePercentage);
				if (PropertyChanging(args))
				{
					_DiscountPckagePercentage = value;
					PropertyChanged(args);
				}
			}
		}

		[DataMember]
		public Nullable<Double> DiscountPckageAmount
		{
			get { return _DiscountPckageAmount; }
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DiscountInAmount, value, _DiscountPckageAmount);
				if (PropertyChanging(args))
				{
					_DiscountPckageAmount = value;
					PropertyChanged(args);
				}
			}
		}

		[DataMember]
		public Nullable<Double> Total
		{	
			get{ return _Total; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Total, value, _Total);
				if (PropertyChanging(args))
				{
					_Total = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsServiceEquipment
		{	
			get{ return _IsServiceEquipment; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsServiceEquipment, value, _IsServiceEquipment);
				if (PropertyChanging(args))
				{
					_IsServiceEquipment = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid ServiceId
		{	
			get{ return _ServiceId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ServiceId, value, _ServiceId);
				if (PropertyChanging(args))
				{
					_ServiceId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsTransfered
		{	
			get{ return _IsTransfered; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsTransfered, value, _IsTransfered);
				if (PropertyChanging(args))
				{
					_IsTransfered = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsEqpExist
		{	
			get{ return _IsEqpExist; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsEqpExist, value, _IsEqpExist);
				if (PropertyChanging(args))
				{
					_IsEqpExist = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsPackageEqp
		{	
			get{ return _IsPackageEqp; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsPackageEqp, value, _IsPackageEqp);
				if (PropertyChanging(args))
				{
					_IsPackageEqp = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsNonCommissionable
		{	
			get{ return _IsNonCommissionable; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsNonCommissionable, value, _IsNonCommissionable);
				if (PropertyChanging(args))
				{
					_IsNonCommissionable = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> AppointmentIntId
		{	
			get{ return _AppointmentIntId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AppointmentIntId, value, _AppointmentIntId);
				if (PropertyChanging(args))
				{
					_AppointmentIntId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsInvoice
		{	
			get{ return _IsInvoice; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsInvoice, value, _IsInvoice);
				if (PropertyChanging(args))
				{
					_IsInvoice = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> AppointmentEquipmentIntId
		{	
			get{ return _AppointmentEquipmentIntId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AppointmentEquipmentIntId, value, _AppointmentEquipmentIntId);
				if (PropertyChanging(args))
				{
					_AppointmentEquipmentIntId = value;
					PropertyChanged(args);					
				}	
			}
        }


        [DataMember]
        public Nullable<Double> DiscountPercent
        {
            get { return _DiscountPercent; }
            set
            {
                PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DiscountPercent, value, _DiscountPercent);
                if (PropertyChanging(args))
                {
                    _DiscountPercent = value;
                    PropertyChanged(args);
                }
            }
        }

        [DataMember]
        public Nullable<Double> DiscountInAmount
        {
            get { return _DiscountInAmount; }
            set
            {
                PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_DiscountInAmount, value, _DiscountInAmount);
                if (PropertyChanging(args))
                {
                    _DiscountInAmount = value;
                    PropertyChanged(args);
                }
            }
        }


        #endregion

        #region Cloning Base Objects
        public  CustomerPackageEqpBase Clone()
		{
			CustomerPackageEqpBase newObj = new  CustomerPackageEqpBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.PackageId = this.PackageId;						
			newObj.EquipmentId = this.EquipmentId;						
			newObj.IsIncluded = this.IsIncluded;						
			newObj.IsDevice = this.IsDevice;						
			newObj.IsOptionalEqp = this.IsOptionalEqp;						
			newObj.Quantity = this.Quantity;						
			newObj.UnitPrice = this.UnitPrice;						
			newObj.DiscountUnitPricce = this.DiscountUnitPricce;						
			newObj.DiscountPckage = this.DiscountPckage;						
			newObj.Total = this.Total;						
			newObj.IsServiceEquipment = this.IsServiceEquipment;						
			newObj.ServiceId = this.ServiceId;						
			newObj.IsTransfered = this.IsTransfered;						
			newObj.IsEqpExist = this.IsEqpExist;						
			newObj.IsPackageEqp = this.IsPackageEqp;						
			newObj.IsNonCommissionable = this.IsNonCommissionable;						
			newObj.AppointmentIntId = this.AppointmentIntId;						
			newObj.IsInvoice = this.IsInvoice;						
			newObj.AppointmentEquipmentIntId = this.AppointmentEquipmentIntId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomerPackageEqpBase.Property_Id, Id);				
			info.AddValue(CustomerPackageEqpBase.Property_CompanyId, CompanyId);				
			info.AddValue(CustomerPackageEqpBase.Property_CustomerId, CustomerId);				
			info.AddValue(CustomerPackageEqpBase.Property_PackageId, PackageId);				
			info.AddValue(CustomerPackageEqpBase.Property_EquipmentId, EquipmentId);				
			info.AddValue(CustomerPackageEqpBase.Property_IsIncluded, IsIncluded);				
			info.AddValue(CustomerPackageEqpBase.Property_IsDevice, IsDevice);				
			info.AddValue(CustomerPackageEqpBase.Property_IsOptionalEqp, IsOptionalEqp);				
			info.AddValue(CustomerPackageEqpBase.Property_Quantity, Quantity);				
			info.AddValue(CustomerPackageEqpBase.Property_UnitPrice, UnitPrice);				
			info.AddValue(CustomerPackageEqpBase.Property_DiscountUnitPricce, DiscountUnitPricce);				
			info.AddValue(CustomerPackageEqpBase.Property_DiscountPckage, DiscountPckage);				
			info.AddValue(CustomerPackageEqpBase.Property_Total, Total);				
			info.AddValue(CustomerPackageEqpBase.Property_IsServiceEquipment, IsServiceEquipment);				
			info.AddValue(CustomerPackageEqpBase.Property_ServiceId, ServiceId);				
			info.AddValue(CustomerPackageEqpBase.Property_IsTransfered, IsTransfered);				
			info.AddValue(CustomerPackageEqpBase.Property_IsEqpExist, IsEqpExist);				
			info.AddValue(CustomerPackageEqpBase.Property_IsPackageEqp, IsPackageEqp);				
			info.AddValue(CustomerPackageEqpBase.Property_IsNonCommissionable, IsNonCommissionable);				
			info.AddValue(CustomerPackageEqpBase.Property_AppointmentIntId, AppointmentIntId);				
			info.AddValue(CustomerPackageEqpBase.Property_IsInvoice, IsInvoice);				
			info.AddValue(CustomerPackageEqpBase.Property_AppointmentEquipmentIntId, AppointmentEquipmentIntId);				
		}
		#endregion

		
	}
}
