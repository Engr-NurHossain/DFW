using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "SmartPackageSystemInstallTypeMapBase", Namespace = "http://www.piistech.com//entities")]
	public class SmartPackageSystemInstallTypeMapBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			PackageId = 1,
			SmartSystemTypeId = 2,
			SmartInstallTypeId = 3
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_PackageId = "PackageId";		            
		public const string Property_SmartSystemTypeId = "SmartSystemTypeId";		            
		public const string Property_SmartInstallTypeId = "SmartInstallTypeId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _PackageId;	            
		private Int32 _SmartSystemTypeId;	            
		private Int32 _SmartInstallTypeId;	            
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
		public Guid PackageId
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
		public Int32 SmartSystemTypeId
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
		public Int32 SmartInstallTypeId
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
		public  SmartPackageSystemInstallTypeMapBase Clone()
		{
			SmartPackageSystemInstallTypeMapBase newObj = new  SmartPackageSystemInstallTypeMapBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.PackageId = this.PackageId;						
			newObj.SmartSystemTypeId = this.SmartSystemTypeId;						
			newObj.SmartInstallTypeId = this.SmartInstallTypeId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(SmartPackageSystemInstallTypeMapBase.Property_Id, Id);				
			info.AddValue(SmartPackageSystemInstallTypeMapBase.Property_PackageId, PackageId);				
			info.AddValue(SmartPackageSystemInstallTypeMapBase.Property_SmartSystemTypeId, SmartSystemTypeId);				
			info.AddValue(SmartPackageSystemInstallTypeMapBase.Property_SmartInstallTypeId, SmartInstallTypeId);				
		}
		#endregion

		
	}
}
