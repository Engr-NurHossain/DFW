using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "PackageOptionalBase", Namespace = "http://www.piistech.com//entities")]
	public class PackageOptionalBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			PackageId = 2,
			EquipmentId = 3,
			IsFree = 4
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_PackageId = "PackageId";		            
		public const string Property_EquipmentId = "EquipmentId";		            
		public const string Property_IsFree = "IsFree";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Int32 _PackageId;	            
		private Guid _EquipmentId;	            
		private Boolean _IsFree;	            
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
		public Int32 PackageId
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

		#endregion
		
		#region Cloning Base Objects
		public  PackageOptionalBase Clone()
		{
			PackageOptionalBase newObj = new  PackageOptionalBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.PackageId = this.PackageId;						
			newObj.EquipmentId = this.EquipmentId;						
			newObj.IsFree = this.IsFree;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(PackageOptionalBase.Property_Id, Id);				
			info.AddValue(PackageOptionalBase.Property_CompanyId, CompanyId);				
			info.AddValue(PackageOptionalBase.Property_PackageId, PackageId);				
			info.AddValue(PackageOptionalBase.Property_EquipmentId, EquipmentId);				
			info.AddValue(PackageOptionalBase.Property_IsFree, IsFree);				
		}
		#endregion

		
	}
}
