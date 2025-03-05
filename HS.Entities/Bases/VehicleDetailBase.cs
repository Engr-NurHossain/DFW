using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "VehicleDetailBase", Namespace = "http://www.piistech.com//entities")]
	public class VehicleDetailBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			VehicleNo = 1,
			VehicleId = 2,
			VIN = 3,
			LicenseNO = 4,
			Year = 5,
			Make = 6,
			Model = 7,
			UserId = 8,
			MillageData = 9,
			ExpirationTag = 10,
			TollTag = 11,
			TechnicianId = 12,
			QuickBookNo = 13
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_VehicleNo = "VehicleNo";		            
		public const string Property_VehicleId = "VehicleId";		            
		public const string Property_VIN = "VIN";		            
		public const string Property_LicenseNO = "LicenseNO";		            
		public const string Property_Year = "Year";		            
		public const string Property_Make = "Make";		            
		public const string Property_Model = "Model";		            
		public const string Property_UserId = "UserId";		            
		public const string Property_MillageData = "MillageData";		            
		public const string Property_ExpirationTag = "ExpirationTag";		            
		public const string Property_TollTag = "TollTag";		            
		public const string Property_TechnicianId = "TechnicianId";		            
		public const string Property_QuickBookNo = "QuickBookNo";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _VehicleNo;	            
		private Guid _VehicleId;	            
		private String _VIN;	            
		private String _LicenseNO;	            
		private String _Year;	            
		private String _Make;	            
		private String _Model;	            
		private Guid _UserId;	            
		private String _MillageData;	            
		private String _ExpirationTag;	            
		private String _TollTag;	            
		private Guid _TechnicianId;	            
		private String _QuickBookNo;	            
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
		public String VehicleNo
		{	
			get{ return _VehicleNo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_VehicleNo, value, _VehicleNo);
				if (PropertyChanging(args))
				{
					_VehicleNo = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid VehicleId
		{	
			get{ return _VehicleId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_VehicleId, value, _VehicleId);
				if (PropertyChanging(args))
				{
					_VehicleId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String VIN
		{	
			get{ return _VIN; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_VIN, value, _VIN);
				if (PropertyChanging(args))
				{
					_VIN = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String LicenseNO
		{	
			get{ return _LicenseNO; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LicenseNO, value, _LicenseNO);
				if (PropertyChanging(args))
				{
					_LicenseNO = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Year
		{	
			get{ return _Year; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Year, value, _Year);
				if (PropertyChanging(args))
				{
					_Year = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Make
		{	
			get{ return _Make; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Make, value, _Make);
				if (PropertyChanging(args))
				{
					_Make = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Model
		{	
			get{ return _Model; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Model, value, _Model);
				if (PropertyChanging(args))
				{
					_Model = value;
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
		public String MillageData
		{	
			get{ return _MillageData; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MillageData, value, _MillageData);
				if (PropertyChanging(args))
				{
					_MillageData = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ExpirationTag
		{	
			get{ return _ExpirationTag; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ExpirationTag, value, _ExpirationTag);
				if (PropertyChanging(args))
				{
					_ExpirationTag = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TollTag
		{	
			get{ return _TollTag; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TollTag, value, _TollTag);
				if (PropertyChanging(args))
				{
					_TollTag = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid TechnicianId
		{	
			get{ return _TechnicianId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TechnicianId, value, _TechnicianId);
				if (PropertyChanging(args))
				{
					_TechnicianId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String QuickBookNo
		{	
			get{ return _QuickBookNo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_QuickBookNo, value, _QuickBookNo);
				if (PropertyChanging(args))
				{
					_QuickBookNo = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  VehicleDetailBase Clone()
		{
			VehicleDetailBase newObj = new  VehicleDetailBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.VehicleNo = this.VehicleNo;						
			newObj.VehicleId = this.VehicleId;						
			newObj.VIN = this.VIN;						
			newObj.LicenseNO = this.LicenseNO;						
			newObj.Year = this.Year;						
			newObj.Make = this.Make;						
			newObj.Model = this.Model;						
			newObj.UserId = this.UserId;						
			newObj.MillageData = this.MillageData;						
			newObj.ExpirationTag = this.ExpirationTag;						
			newObj.TollTag = this.TollTag;						
			newObj.TechnicianId = this.TechnicianId;						
			newObj.QuickBookNo = this.QuickBookNo;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(VehicleDetailBase.Property_Id, Id);				
			info.AddValue(VehicleDetailBase.Property_VehicleNo, VehicleNo);				
			info.AddValue(VehicleDetailBase.Property_VehicleId, VehicleId);				
			info.AddValue(VehicleDetailBase.Property_VIN, VIN);				
			info.AddValue(VehicleDetailBase.Property_LicenseNO, LicenseNO);				
			info.AddValue(VehicleDetailBase.Property_Year, Year);				
			info.AddValue(VehicleDetailBase.Property_Make, Make);				
			info.AddValue(VehicleDetailBase.Property_Model, Model);				
			info.AddValue(VehicleDetailBase.Property_UserId, UserId);				
			info.AddValue(VehicleDetailBase.Property_MillageData, MillageData);				
			info.AddValue(VehicleDetailBase.Property_ExpirationTag, ExpirationTag);				
			info.AddValue(VehicleDetailBase.Property_TollTag, TollTag);				
			info.AddValue(VehicleDetailBase.Property_TechnicianId, TechnicianId);				
			info.AddValue(VehicleDetailBase.Property_QuickBookNo, QuickBookNo);				
		}
		#endregion

		
	}
}
