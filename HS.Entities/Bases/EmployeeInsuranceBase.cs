using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "EmployeeInsuranceBase", Namespace = "http://www.piistech.com//entities")]
	public class EmployeeInsuranceBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			UserId = 1,
			EligibleFrom = 2,
			Type = 3,
			Subtype = 4,
			InsuranceRate = 5,
			RateType = 6,
			IsActive = 7,
			CreatedByUid = 8,
			LastUpdatedByUid = 9,
			LastUpdatedDate = 10
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_UserId = "UserId";		            
		public const string Property_EligibleFrom = "EligibleFrom";		            
		public const string Property_Type = "Type";		            
		public const string Property_Subtype = "Subtype";		            
		public const string Property_InsuranceRate = "InsuranceRate";		            
		public const string Property_RateType = "RateType";		            
		public const string Property_IsActive = "IsActive";		            
		public const string Property_CreatedByUid = "CreatedByUid";		            
		public const string Property_LastUpdatedByUid = "LastUpdatedByUid";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _UserId;	            
		private Nullable<DateTime> _EligibleFrom;	            
		private String _Type;	            
		private String _Subtype;	            
		private Nullable<Double> _InsuranceRate;	            
		private String _RateType;	            
		private Boolean _IsActive;	            
		private Guid _CreatedByUid;	            
		private Guid _LastUpdatedByUid;	            
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
		public Guid UserId
		{	
			get{ return _UserId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UserId, value, _UserId);
				if (PropertyChanging(args))
				{
					_UserId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> EligibleFrom
		{	
			get{ return _EligibleFrom; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EligibleFrom, value, _EligibleFrom);
				if (PropertyChanging(args))
				{
					_EligibleFrom = value;
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
		public String Subtype
		{	
			get{ return _Subtype; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Subtype, value, _Subtype);
				if (PropertyChanging(args))
				{
					_Subtype = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> InsuranceRate
		{	
			get{ return _InsuranceRate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InsuranceRate, value, _InsuranceRate);
				if (PropertyChanging(args))
				{
					_InsuranceRate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String RateType
		{	
			get{ return _RateType; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RateType, value, _RateType);
				if (PropertyChanging(args))
				{
					_RateType = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean IsActive
		{	
			get{ return _IsActive; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsActive, value, _IsActive);
				if (PropertyChanging(args))
				{
					_IsActive = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid CreatedByUid
		{	
			get{ return _CreatedByUid; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreatedByUid, value, _CreatedByUid);
				if (PropertyChanging(args))
				{
					_CreatedByUid = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid LastUpdatedByUid
		{	
			get{ return _LastUpdatedByUid; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastUpdatedByUid, value, _LastUpdatedByUid);
				if (PropertyChanging(args))
				{
					_LastUpdatedByUid = value;
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
		public  EmployeeInsuranceBase Clone()
		{
			EmployeeInsuranceBase newObj = new  EmployeeInsuranceBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.UserId = this.UserId;						
			newObj.EligibleFrom = this.EligibleFrom;						
			newObj.Type = this.Type;						
			newObj.Subtype = this.Subtype;						
			newObj.InsuranceRate = this.InsuranceRate;						
			newObj.RateType = this.RateType;						
			newObj.IsActive = this.IsActive;						
			newObj.CreatedByUid = this.CreatedByUid;						
			newObj.LastUpdatedByUid = this.LastUpdatedByUid;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(EmployeeInsuranceBase.Property_Id, Id);				
			info.AddValue(EmployeeInsuranceBase.Property_UserId, UserId);				
			info.AddValue(EmployeeInsuranceBase.Property_EligibleFrom, EligibleFrom);				
			info.AddValue(EmployeeInsuranceBase.Property_Type, Type);				
			info.AddValue(EmployeeInsuranceBase.Property_Subtype, Subtype);				
			info.AddValue(EmployeeInsuranceBase.Property_InsuranceRate, InsuranceRate);				
			info.AddValue(EmployeeInsuranceBase.Property_RateType, RateType);				
			info.AddValue(EmployeeInsuranceBase.Property_IsActive, IsActive);				
			info.AddValue(EmployeeInsuranceBase.Property_CreatedByUid, CreatedByUid);				
			info.AddValue(EmployeeInsuranceBase.Property_LastUpdatedByUid, LastUpdatedByUid);				
			info.AddValue(EmployeeInsuranceBase.Property_LastUpdatedDate, LastUpdatedDate);				
		}
		#endregion

		
	}
}
