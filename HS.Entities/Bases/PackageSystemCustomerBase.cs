using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "PackageSystemCustomerBase", Namespace = "http://www.piistech.com//entities")]
	public class PackageSystemCustomerBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			CustomerId = 2,
			PackageSystemId = 3
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_PackageSystemId = "PackageSystemId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _CustomerId;	            
		private Int32 _PackageSystemId;	            
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
		public Int32 PackageSystemId
		{	
			get{ return _PackageSystemId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PackageSystemId, value, _PackageSystemId);
				if (PropertyChanging(args))
				{
					_PackageSystemId = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  PackageSystemCustomerBase Clone()
		{
			PackageSystemCustomerBase newObj = new  PackageSystemCustomerBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.CustomerId = this.CustomerId;						
			newObj.PackageSystemId = this.PackageSystemId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(PackageSystemCustomerBase.Property_Id, Id);				
			info.AddValue(PackageSystemCustomerBase.Property_CompanyId, CompanyId);				
			info.AddValue(PackageSystemCustomerBase.Property_CustomerId, CustomerId);				
			info.AddValue(PackageSystemCustomerBase.Property_PackageSystemId, PackageSystemId);				
		}
		#endregion

		
	}
}
