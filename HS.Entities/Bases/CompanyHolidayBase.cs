using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CompanyHolidayBase", Namespace = "http://www.piistech.com//entities")]
	public class CompanyHolidayBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			Year = 2,
			Holiday = 3,
			HolidayDetails = 4,
			IsActive = 5,
			CreatedDate = 6,
			CreatedBy = 7
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_Year = "Year";		            
		public const string Property_Holiday = "Holiday";		            
		public const string Property_HolidayDetails = "HolidayDetails";		            
		public const string Property_IsActive = "IsActive";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private String _Year;	            
		private DateTime _Holiday;	            
		private String _HolidayDetails;	            
		private Boolean _IsActive;	            
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
		public String Year
		{	
			get{ return _Year; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Year, value, _Year);
				if (PropertyChanging(args))
				{
					_Year = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime Holiday
		{	
			get{ return _Holiday; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Holiday, value, _Holiday);
				if (PropertyChanging(args))
				{
					_Holiday = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String HolidayDetails
		{	
			get{ return _HolidayDetails; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_HolidayDetails, value, _HolidayDetails);
				if (PropertyChanging(args))
				{
					_HolidayDetails = value;
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
		public  CompanyHolidayBase Clone()
		{
			CompanyHolidayBase newObj = new  CompanyHolidayBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.Year = this.Year;						
			newObj.Holiday = this.Holiday;						
			newObj.HolidayDetails = this.HolidayDetails;						
			newObj.IsActive = this.IsActive;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CompanyHolidayBase.Property_Id, Id);				
			info.AddValue(CompanyHolidayBase.Property_CompanyId, CompanyId);				
			info.AddValue(CompanyHolidayBase.Property_Year, Year);				
			info.AddValue(CompanyHolidayBase.Property_Holiday, Holiday);				
			info.AddValue(CompanyHolidayBase.Property_HolidayDetails, HolidayDetails);				
			info.AddValue(CompanyHolidayBase.Property_IsActive, IsActive);				
			info.AddValue(CompanyHolidayBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(CompanyHolidayBase.Property_CreatedBy, CreatedBy);				
		}
		#endregion

		
	}
}