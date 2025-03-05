using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "BundleEquipmentBase", Namespace = "http://www.piistech.com//entities")]
	public class BundleEquipmentBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			BundleId = 2,
			EquipmentId = 3,
			IsActive = 4,
			LastUpdatedDate = 5,
			LastUpdatedBy = 6
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_BundleId = "BundleId";		            
		public const string Property_EquipmentId = "EquipmentId";		            
		public const string Property_IsActive = "IsActive";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Int32 _BundleId;	            
		private Guid _EquipmentId;	            
		private Boolean _IsActive;	            
		private DateTime _LastUpdatedDate;	            
		private String _LastUpdatedBy;	            
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
		public Int32 BundleId
		{	
			get{ return _BundleId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BundleId, value, _BundleId);
				if (PropertyChanging(args))
				{
					_BundleId = value;
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
		public String LastUpdatedBy
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

		#endregion
		
		#region Cloning Base Objects
		public  BundleEquipmentBase Clone()
		{
			BundleEquipmentBase newObj = new  BundleEquipmentBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.BundleId = this.BundleId;						
			newObj.EquipmentId = this.EquipmentId;						
			newObj.IsActive = this.IsActive;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(BundleEquipmentBase.Property_Id, Id);				
			info.AddValue(BundleEquipmentBase.Property_CompanyId, CompanyId);				
			info.AddValue(BundleEquipmentBase.Property_BundleId, BundleId);				
			info.AddValue(BundleEquipmentBase.Property_EquipmentId, EquipmentId);				
			info.AddValue(BundleEquipmentBase.Property_IsActive, IsActive);				
			info.AddValue(BundleEquipmentBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(BundleEquipmentBase.Property_LastUpdatedBy, LastUpdatedBy);				
		}
		#endregion

		
	}
}
