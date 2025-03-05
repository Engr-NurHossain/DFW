using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "BundleBase", Namespace = "http://www.piistech.com//entities")]
	public class BundleBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			Name = 2,
			SKU = 3,
			Info = 4,
			IsDisplay = 5,
			LastUpdatedDate = 6,
			LastUpdatedBy = 7
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_Name = "Name";		            
		public const string Property_SKU = "SKU";		            
		public const string Property_Info = "Info";		            
		public const string Property_IsDisplay = "IsDisplay";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private String _Name;	            
		private String _SKU;	            
		private String _Info;	            
		private Boolean _IsDisplay;	            
		private DateTime _LastUpdatedDate;	            
		private String _LastUpdatedBy;	            
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
		public String SKU
		{	
			get{ return _SKU; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_SKU, value, _SKU);
				if (PropertyChanging(args))
				{
					_SKU = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Info
		{	
			get{ return _Info; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Info, value, _Info);
				if (PropertyChanging(args))
				{
					_Info = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean IsDisplay
		{	
			get{ return _IsDisplay; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsDisplay, value, _IsDisplay);
				if (PropertyChanging(args))
				{
					_IsDisplay = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime LastUpdatedDate
		{	
			get{ return _LastUpdatedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastUpdatedDate, value, _LastUpdatedDate);
				if (PropertyChanging(args))
				{
					_LastUpdatedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String LastUpdatedBy
		{	
			get{ return _LastUpdatedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastUpdatedBy, value, _LastUpdatedBy);
				if (PropertyChanging(args))
				{
					_LastUpdatedBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  BundleBase Clone()
		{
			BundleBase newObj = new  BundleBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.Name = this.Name;						
			newObj.SKU = this.SKU;						
			newObj.Info = this.Info;						
			newObj.IsDisplay = this.IsDisplay;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(BundleBase.Property_Id, Id);				
			info.AddValue(BundleBase.Property_CompanyId, CompanyId);				
			info.AddValue(BundleBase.Property_Name, Name);				
			info.AddValue(BundleBase.Property_SKU, SKU);				
			info.AddValue(BundleBase.Property_Info, Info);				
			info.AddValue(BundleBase.Property_IsDisplay, IsDisplay);				
			info.AddValue(BundleBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(BundleBase.Property_LastUpdatedBy, LastUpdatedBy);				
		}
		#endregion

		
	}
}
