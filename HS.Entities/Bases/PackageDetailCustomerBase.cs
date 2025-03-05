using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "PackageDetailCustomerBase", Namespace = "http://www.piistech.com//entities")]
	public class PackageDetailCustomerBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			CustomerId = 2,
			Type = 3,
			IsIncluded = 4,
			PackageEqpId = 5
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_Type = "Type";		            
		public const string Property_IsIncluded = "IsIncluded";		            
		public const string Property_PackageEqpId = "PackageEqpId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _CustomerId;	            
		private String _Type;	            
		private Boolean _IsIncluded;	            
		private Int32 _PackageEqpId;	            
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
		public Int32 PackageEqpId
		{	
			get{ return _PackageEqpId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PackageEqpId, value, _PackageEqpId);
				if (PropertyChanging(args))
				{
					_PackageEqpId = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  PackageDetailCustomerBase Clone()
		{
			PackageDetailCustomerBase newObj = new  PackageDetailCustomerBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.Type = this.Type;						
			newObj.IsIncluded = this.IsIncluded;						
			newObj.PackageEqpId = this.PackageEqpId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(PackageDetailCustomerBase.Property_Id, Id);				
			info.AddValue(PackageDetailCustomerBase.Property_CompanyId, CompanyId);				
			info.AddValue(PackageDetailCustomerBase.Property_CustomerId, CustomerId);				
			info.AddValue(PackageDetailCustomerBase.Property_Type, Type);				
			info.AddValue(PackageDetailCustomerBase.Property_IsIncluded, IsIncluded);				
			info.AddValue(PackageDetailCustomerBase.Property_PackageEqpId, PackageEqpId);				
		}
		#endregion

		
	}
}
