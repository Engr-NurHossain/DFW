using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "PackageBase", Namespace = "http://www.piistech.com//entities")]
	public class PackageBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			PackageId = 1,
			Name = 2,
			CompanyId = 3,
			OptionEqpMaxLimit = 4,
			Rate = 5
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_PackageId = "PackageId";		            
		public const string Property_Name = "Name";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_OptionEqpMaxLimit = "OptionEqpMaxLimit";		            
		public const string Property_Rate = "Rate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _PackageId;	            
		private String _Name;	            
		private Guid _CompanyId;	            
		private Nullable<Int32> _OptionEqpMaxLimit;	            
		private Nullable<Double> _Rate;	            
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
		public String Name
		{	
			get{ return _Name; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Name, value, _Name);
				if (PropertyChanging(args))
				{
					_Name = value;
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
		public Nullable<Int32> OptionEqpMaxLimit
		{	
			get{ return _OptionEqpMaxLimit; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OptionEqpMaxLimit, value, _OptionEqpMaxLimit);
				if (PropertyChanging(args))
				{
					_OptionEqpMaxLimit = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> Rate
		{	
			get{ return _Rate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Rate, value, _Rate);
				if (PropertyChanging(args))
				{
					_Rate = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  PackageBase Clone()
		{
			PackageBase newObj = new  PackageBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.PackageId = this.PackageId;						
			newObj.Name = this.Name;						
			newObj.CompanyId = this.CompanyId;						
			newObj.OptionEqpMaxLimit = this.OptionEqpMaxLimit;						
			newObj.Rate = this.Rate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(PackageBase.Property_Id, Id);				
			info.AddValue(PackageBase.Property_PackageId, PackageId);				
			info.AddValue(PackageBase.Property_Name, Name);				
			info.AddValue(PackageBase.Property_CompanyId, CompanyId);				
			info.AddValue(PackageBase.Property_OptionEqpMaxLimit, OptionEqpMaxLimit);				
			info.AddValue(PackageBase.Property_Rate, Rate);				
		}
		#endregion

		
	}
}
