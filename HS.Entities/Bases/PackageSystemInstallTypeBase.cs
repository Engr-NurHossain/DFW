using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "PackageSystemInstallTypeBase", Namespace = "http://www.piistech.com//entities")]
	public class PackageSystemInstallTypeBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			SystemId = 2,
			Installtypevalue = 3
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_SystemId = "SystemId";		            
		public const string Property_Installtypevalue = "Installtypevalue";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Int32 _SystemId;	            
		private String _Installtypevalue;	            
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
		public Int32 SystemId
		{	
			get{ return _SystemId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SystemId, value, _SystemId);
				if (PropertyChanging(args))
				{
					_SystemId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Installtypevalue
		{	
			get{ return _Installtypevalue; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Installtypevalue, value, _Installtypevalue);
				if (PropertyChanging(args))
				{
					_Installtypevalue = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  PackageSystemInstallTypeBase Clone()
		{
			PackageSystemInstallTypeBase newObj = new  PackageSystemInstallTypeBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.SystemId = this.SystemId;						
			newObj.Installtypevalue = this.Installtypevalue;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(PackageSystemInstallTypeBase.Property_Id, Id);				
			info.AddValue(PackageSystemInstallTypeBase.Property_CompanyId, CompanyId);				
			info.AddValue(PackageSystemInstallTypeBase.Property_SystemId, SystemId);				
			info.AddValue(PackageSystemInstallTypeBase.Property_Installtypevalue, Installtypevalue);				
		}
		#endregion

		
	}
}
