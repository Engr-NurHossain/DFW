using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomerSignatureBase", Namespace = "http://www.hims-tech.com//entities")]
	public class CustomerSignatureBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CustomerId = 1,
			ReferenceIdGuid = 2,
			ReferenceIdnvarchar = 3,
			Type = 4,
			Signature = 5,
			CreatedDate = 6,
			CreatedBy = 7
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_ReferenceIdGuid = "ReferenceIdGuid";		            
		public const string Property_ReferenceIdnvarchar = "ReferenceIdnvarchar";		            
		public const string Property_Type = "Type";		            
		public const string Property_Signature = "Signature";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CustomerId;	            
		private Guid _ReferenceIdGuid;	            
		private String _ReferenceIdnvarchar;	            
		private String _Type;	            
		private String _Signature;	            
		private DateTime _CreatedDate;	            
		private Guid _CreatedBy;	            
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
		public Guid ReferenceIdGuid
		{	
			get{ return _ReferenceIdGuid; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReferenceIdGuid, value, _ReferenceIdGuid);
				if (PropertyChanging(args))
				{
					_ReferenceIdGuid = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ReferenceIdnvarchar
		{	
			get{ return _ReferenceIdnvarchar; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ReferenceIdnvarchar, value, _ReferenceIdnvarchar);
				if (PropertyChanging(args))
				{
					_ReferenceIdnvarchar = value;
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
		public String Signature
		{	
			get{ return _Signature; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Signature, value, _Signature);
				if (PropertyChanging(args))
				{
					_Signature = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime CreatedDate
		{	
			get{ return _CreatedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreatedDate, value, _CreatedDate);
				if (PropertyChanging(args))
				{
					_CreatedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid CreatedBy
		{	
			get{ return _CreatedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreatedBy, value, _CreatedBy);
				if (PropertyChanging(args))
				{
					_CreatedBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  CustomerSignatureBase Clone()
		{
			CustomerSignatureBase newObj = new  CustomerSignatureBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CustomerId = this.CustomerId;						
			newObj.ReferenceIdGuid = this.ReferenceIdGuid;						
			newObj.ReferenceIdnvarchar = this.ReferenceIdnvarchar;						
			newObj.Type = this.Type;						
			newObj.Signature = this.Signature;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomerSignatureBase.Property_Id, Id);				
			info.AddValue(CustomerSignatureBase.Property_CustomerId, CustomerId);				
			info.AddValue(CustomerSignatureBase.Property_ReferenceIdGuid, ReferenceIdGuid);				
			info.AddValue(CustomerSignatureBase.Property_ReferenceIdnvarchar, ReferenceIdnvarchar);				
			info.AddValue(CustomerSignatureBase.Property_Type, Type);				
			info.AddValue(CustomerSignatureBase.Property_Signature, Signature);				
			info.AddValue(CustomerSignatureBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(CustomerSignatureBase.Property_CreatedBy, CreatedBy);				
		}
		#endregion

		
	}
}
