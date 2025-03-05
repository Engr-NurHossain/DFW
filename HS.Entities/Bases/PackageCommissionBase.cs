using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "PackageCommissionBase", Namespace = "http://www.piistech.com//entities")]
	public class PackageCommissionBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			PackageCommissionId = 1,
			Type = 2,
			LeadType = 3,
			PackageType = 4,
			CommissionType = 5,
			Commission = 6,
			CreatedBy = 7,
			CreatedDate = 8,
			LastUpdatedBy = 9,
			LastUpdatedDate = 10
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_PackageCommissionId = "PackageCommissionId";		            
		public const string Property_Type = "Type";		            
		public const string Property_LeadType = "LeadType";		            
		public const string Property_PackageType = "PackageType";		            
		public const string Property_CommissionType = "CommissionType";		            
		public const string Property_Commission = "Commission";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _PackageCommissionId;	            
		private String _Type;	            
		private String _LeadType;	            
		private String _PackageType;	            
		private String _CommissionType;	            
		private Double _Commission;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
		private Guid _LastUpdatedBy;	            
		private DateTime _LastUpdatedDate;	            
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
		public Guid PackageCommissionId
		{	
			get{ return _PackageCommissionId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PackageCommissionId, value, _PackageCommissionId);
				if (PropertyChanging(args))
				{
					_PackageCommissionId = value;
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
		public String PackageType
		{	
			get{ return _PackageType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PackageType, value, _PackageType);
				if (PropertyChanging(args))
				{
					_PackageType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String CommissionType
		{	
			get{ return _CommissionType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CommissionType, value, _CommissionType);
				if (PropertyChanging(args))
				{
					_CommissionType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Double Commission
		{	
			get{ return _Commission; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Commission, value, _Commission);
				if (PropertyChanging(args))
				{
					_Commission = value;
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
		public Guid LastUpdatedBy
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

		#endregion
		
		#region Cloning Base Objects
		public  PackageCommissionBase Clone()
		{
			PackageCommissionBase newObj = new  PackageCommissionBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.PackageCommissionId = this.PackageCommissionId;						
			newObj.Type = this.Type;						
			newObj.LeadType = this.LeadType;						
			newObj.PackageType = this.PackageType;						
			newObj.CommissionType = this.CommissionType;						
			newObj.Commission = this.Commission;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(PackageCommissionBase.Property_Id, Id);				
			info.AddValue(PackageCommissionBase.Property_PackageCommissionId, PackageCommissionId);				
			info.AddValue(PackageCommissionBase.Property_Type, Type);				
			info.AddValue(PackageCommissionBase.Property_LeadType, LeadType);				
			info.AddValue(PackageCommissionBase.Property_PackageType, PackageType);				
			info.AddValue(PackageCommissionBase.Property_CommissionType, CommissionType);				
			info.AddValue(PackageCommissionBase.Property_Commission, Commission);				
			info.AddValue(PackageCommissionBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(PackageCommissionBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(PackageCommissionBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(PackageCommissionBase.Property_LastUpdatedDate, LastUpdatedDate);				
		}
		#endregion

		
	}
}
