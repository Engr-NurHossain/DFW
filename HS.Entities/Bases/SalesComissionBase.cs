using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "SalesComissionBase", Namespace = "http://www.piistech.com//entities")]
	public class SalesComissionBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			PackageServiceId = 2,
			SalesLocation = 3,
			LeadType = 4,
			AmoutParcent = 5
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_PackageServiceId = "PackageServiceId";		            
		public const string Property_SalesLocation = "SalesLocation";		            
		public const string Property_LeadType = "LeadType";		            
		public const string Property_AmoutParcent = "AmoutParcent";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _PackageServiceId;	            
		private String _SalesLocation;	            
		private String _LeadType;	            
		private Double _AmoutParcent;	            
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
		public Guid PackageServiceId
		{	
			get{ return _PackageServiceId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PackageServiceId, value, _PackageServiceId);
				if (PropertyChanging(args))
				{
					_PackageServiceId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String SalesLocation
		{	
			get{ return _SalesLocation; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SalesLocation, value, _SalesLocation);
				if (PropertyChanging(args))
				{
					_SalesLocation = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String LeadType
		{	
			get{ return _LeadType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LeadType, value, _LeadType);
				if (PropertyChanging(args))
				{
					_LeadType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Double AmoutParcent
		{	
			get{ return _AmoutParcent; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AmoutParcent, value, _AmoutParcent);
				if (PropertyChanging(args))
				{
					_AmoutParcent = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  SalesComissionBase Clone()
		{
			SalesComissionBase newObj = new  SalesComissionBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.PackageServiceId = this.PackageServiceId;						
			newObj.SalesLocation = this.SalesLocation;						
			newObj.LeadType = this.LeadType;						
			newObj.AmoutParcent = this.AmoutParcent;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(SalesComissionBase.Property_Id, Id);				
			info.AddValue(SalesComissionBase.Property_CompanyId, CompanyId);				
			info.AddValue(SalesComissionBase.Property_PackageServiceId, PackageServiceId);				
			info.AddValue(SalesComissionBase.Property_SalesLocation, SalesLocation);				
			info.AddValue(SalesComissionBase.Property_LeadType, LeadType);				
			info.AddValue(SalesComissionBase.Property_AmoutParcent, AmoutParcent);				
		}
		#endregion

		
	}
}
