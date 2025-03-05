using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "MMRRangeBase", Namespace = "http://www.piistech.com//entities")]
	public class MMRRangeBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			PackageId = 2,
			MinMMR = 3,
			MaxMMR = 4
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_PackageId = "PackageId";		            
		public const string Property_MinMMR = "MinMMR";		            
		public const string Property_MaxMMR = "MaxMMR";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _PackageId;	            
		private Double _MinMMR;	            
		private Double _MaxMMR;	            
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
		public Double MinMMR
		{	
			get{ return _MinMMR; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MinMMR, value, _MinMMR);
				if (PropertyChanging(args))
				{
					_MinMMR = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Double MaxMMR
		{	
			get{ return _MaxMMR; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MaxMMR, value, _MaxMMR);
				if (PropertyChanging(args))
				{
					_MaxMMR = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  MMRRangeBase Clone()
		{
			MMRRangeBase newObj = new  MMRRangeBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.PackageId = this.PackageId;						
			newObj.MinMMR = this.MinMMR;						
			newObj.MaxMMR = this.MaxMMR;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(MMRRangeBase.Property_Id, Id);				
			info.AddValue(MMRRangeBase.Property_CompanyId, CompanyId);				
			info.AddValue(MMRRangeBase.Property_PackageId, PackageId);				
			info.AddValue(MMRRangeBase.Property_MinMMR, MinMMR);				
			info.AddValue(MMRRangeBase.Property_MaxMMR, MaxMMR);				
		}
		#endregion

		
	}
}
