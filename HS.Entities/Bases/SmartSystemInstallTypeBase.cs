using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "SmartSystemInstallTypeBase", Namespace = "http://www.piistech.com//entities")]
	public class SmartSystemInstallTypeBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			SystemId = 2,
			InstallTypeId = 3
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_SystemId = "SystemId";		            
		public const string Property_InstallTypeId = "InstallTypeId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Int32 _SystemId;	            
		private Int32 _InstallTypeId;	            
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
		public Int32 InstallTypeId
		{	
			get{ return _InstallTypeId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InstallTypeId, value, _InstallTypeId);
				if (PropertyChanging(args))
				{
					_InstallTypeId = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  SmartSystemInstallTypeBase Clone()
		{
			SmartSystemInstallTypeBase newObj = new  SmartSystemInstallTypeBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.SystemId = this.SystemId;						
			newObj.InstallTypeId = this.InstallTypeId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(SmartSystemInstallTypeBase.Property_Id, Id);				
			info.AddValue(SmartSystemInstallTypeBase.Property_CompanyId, CompanyId);				
			info.AddValue(SmartSystemInstallTypeBase.Property_SystemId, SystemId);				
			info.AddValue(SmartSystemInstallTypeBase.Property_InstallTypeId, InstallTypeId);				
		}
		#endregion

		
	}
}
