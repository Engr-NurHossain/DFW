using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "TechnicianInventoryBase", Namespace = "http://www.piistech.com//entities")]
	public class TechnicianInventoryBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			TechnicianId = 2,
			EquipmentId = 3,
			Quantity = 4,
			LastUpdatedBy = 5,
			LastUpdatedDate = 6
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_TechnicianId = "TechnicianId";		            
		public const string Property_EquipmentId = "EquipmentId";		            
		public const string Property_Quantity = "Quantity";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _TechnicianId;	            
		private Guid _EquipmentId;	            
		private Int32 _Quantity;	            
		private String _LastUpdatedBy;	            
		private Nullable<DateTime> _LastUpdatedDate;	            
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
		public Int32 Quantity
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

		[DataMember]
		public Nullable<DateTime> LastUpdatedDate
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

		#endregion
		
		#region Cloning Base Objects
		public  TechnicianInventoryBase Clone()
		{
			TechnicianInventoryBase newObj = new  TechnicianInventoryBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.TechnicianId = this.TechnicianId;						
			newObj.EquipmentId = this.EquipmentId;						
			newObj.Quantity = this.Quantity;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(TechnicianInventoryBase.Property_Id, Id);				
			info.AddValue(TechnicianInventoryBase.Property_CompanyId, CompanyId);				
			info.AddValue(TechnicianInventoryBase.Property_TechnicianId, TechnicianId);				
			info.AddValue(TechnicianInventoryBase.Property_EquipmentId, EquipmentId);				
			info.AddValue(TechnicianInventoryBase.Property_Quantity, Quantity);				
			info.AddValue(TechnicianInventoryBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(TechnicianInventoryBase.Property_LastUpdatedDate, LastUpdatedDate);				
		}
		#endregion

		
	}
}
