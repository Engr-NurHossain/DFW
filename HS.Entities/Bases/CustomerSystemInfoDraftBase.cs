using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomerSystemInfoDraftBase", Namespace = "http://www.piistech.com//entities")]
	public class CustomerSystemInfoDraftBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CustomerId = 1,
			CompanyId = 2,
			PanelType = 3,
			InstallType = 4,
			CellularBackup = 5,
			Zone1 = 6,
			Zone2 = 7,
			Zone3 = 8,
			Zone4 = 9,
			Zone5 = 10,
			Zone6 = 11,
			Zone7 = 12,
			Zone8 = 13,
			Zone9 = 14,
			SmartSystemTypeId = 15,
			SmartInstallTypeId = 16
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_PanelType = "PanelType";		            
		public const string Property_InstallType = "InstallType";		            
		public const string Property_CellularBackup = "CellularBackup";		            
		public const string Property_Zone1 = "Zone1";		            
		public const string Property_Zone2 = "Zone2";		            
		public const string Property_Zone3 = "Zone3";		            
		public const string Property_Zone4 = "Zone4";		            
		public const string Property_Zone5 = "Zone5";		            
		public const string Property_Zone6 = "Zone6";		            
		public const string Property_Zone7 = "Zone7";		            
		public const string Property_Zone8 = "Zone8";		            
		public const string Property_Zone9 = "Zone9";		            
		public const string Property_SmartSystemTypeId = "SmartSystemTypeId";		            
		public const string Property_SmartInstallTypeId = "SmartInstallTypeId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CustomerId;	            
		private Guid _CompanyId;	            
		private String _PanelType;	            
		private String _InstallType;	            
		private String _CellularBackup;	            
		private String _Zone1;	            
		private String _Zone2;	            
		private String _Zone3;	            
		private String _Zone4;	            
		private String _Zone5;	            
		private String _Zone6;	            
		private String _Zone7;	            
		private String _Zone8;	            
		private String _Zone9;	            
		private Nullable<Int32> _SmartSystemTypeId;	            
		private Nullable<Int32> _SmartInstallTypeId;	            
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
		public String PanelType
		{	
			get{ return _PanelType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PanelType, value, _PanelType);
				if (PropertyChanging(args))
				{
					_PanelType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String InstallType
		{	
			get{ return _InstallType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InstallType, value, _InstallType);
				if (PropertyChanging(args))
				{
					_InstallType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CellularBackup
		{	
			get{ return _CellularBackup; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CellularBackup, value, _CellularBackup);
				if (PropertyChanging(args))
				{
					_CellularBackup = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Zone1
		{	
			get{ return _Zone1; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Zone1, value, _Zone1);
				if (PropertyChanging(args))
				{
					_Zone1 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Zone2
		{	
			get{ return _Zone2; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Zone2, value, _Zone2);
				if (PropertyChanging(args))
				{
					_Zone2 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Zone3
		{	
			get{ return _Zone3; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Zone3, value, _Zone3);
				if (PropertyChanging(args))
				{
					_Zone3 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Zone4
		{	
			get{ return _Zone4; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Zone4, value, _Zone4);
				if (PropertyChanging(args))
				{
					_Zone4 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Zone5
		{	
			get{ return _Zone5; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Zone5, value, _Zone5);
				if (PropertyChanging(args))
				{
					_Zone5 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Zone6
		{	
			get{ return _Zone6; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Zone6, value, _Zone6);
				if (PropertyChanging(args))
				{
					_Zone6 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Zone7
		{	
			get{ return _Zone7; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Zone7, value, _Zone7);
				if (PropertyChanging(args))
				{
					_Zone7 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Zone8
		{	
			get{ return _Zone8; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Zone8, value, _Zone8);
				if (PropertyChanging(args))
				{
					_Zone8 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Zone9
		{	
			get{ return _Zone9; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Zone9, value, _Zone9);
				if (PropertyChanging(args))
				{
					_Zone9 = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  CustomerSystemInfoDraftBase Clone()
		{
			CustomerSystemInfoDraftBase newObj = new  CustomerSystemInfoDraftBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CustomerId = this.CustomerId;						
			newObj.CompanyId = this.CompanyId;						
			newObj.PanelType = this.PanelType;						
			newObj.InstallType = this.InstallType;						
			newObj.CellularBackup = this.CellularBackup;						
			newObj.Zone1 = this.Zone1;						
			newObj.Zone2 = this.Zone2;						
			newObj.Zone3 = this.Zone3;						
			newObj.Zone4 = this.Zone4;						
			newObj.Zone5 = this.Zone5;						
			newObj.Zone6 = this.Zone6;						
			newObj.Zone7 = this.Zone7;						
			newObj.Zone8 = this.Zone8;						
			newObj.Zone9 = this.Zone9;						
			newObj.SmartSystemTypeId = this.SmartSystemTypeId;						
			newObj.SmartInstallTypeId = this.SmartInstallTypeId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomerSystemInfoDraftBase.Property_Id, Id);				
			info.AddValue(CustomerSystemInfoDraftBase.Property_CustomerId, CustomerId);				
			info.AddValue(CustomerSystemInfoDraftBase.Property_CompanyId, CompanyId);				
			info.AddValue(CustomerSystemInfoDraftBase.Property_PanelType, PanelType);				
			info.AddValue(CustomerSystemInfoDraftBase.Property_InstallType, InstallType);				
			info.AddValue(CustomerSystemInfoDraftBase.Property_CellularBackup, CellularBackup);				
			info.AddValue(CustomerSystemInfoDraftBase.Property_Zone1, Zone1);				
			info.AddValue(CustomerSystemInfoDraftBase.Property_Zone2, Zone2);				
			info.AddValue(CustomerSystemInfoDraftBase.Property_Zone3, Zone3);				
			info.AddValue(CustomerSystemInfoDraftBase.Property_Zone4, Zone4);				
			info.AddValue(CustomerSystemInfoDraftBase.Property_Zone5, Zone5);				
			info.AddValue(CustomerSystemInfoDraftBase.Property_Zone6, Zone6);				
			info.AddValue(CustomerSystemInfoDraftBase.Property_Zone7, Zone7);				
			info.AddValue(CustomerSystemInfoDraftBase.Property_Zone8, Zone8);				
			info.AddValue(CustomerSystemInfoDraftBase.Property_Zone9, Zone9);				
			info.AddValue(CustomerSystemInfoDraftBase.Property_SmartSystemTypeId, SmartSystemTypeId);				
			info.AddValue(CustomerSystemInfoDraftBase.Property_SmartInstallTypeId, SmartInstallTypeId);				
		}
		#endregion

		
	}
}
