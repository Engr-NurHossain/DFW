using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "PayrollMileageBase", Namespace = "http://www.hims-tech.com//entities")]
	public class PayrollMileageBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			PayrollMileageId = 1,
			CompanyId = 2,
			MileageMin = 3,
			MileageMax = 4,
			Amount = 5,
			CreatedBy = 6,
			CreatedDate = 7,
			LastUpdateBy = 8,
			LastUpdateDate = 9,
			TermSheetId = 10
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_PayrollMileageId = "PayrollMileageId";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_MileageMin = "MileageMin";		            
		public const string Property_MileageMax = "MileageMax";		            
		public const string Property_Amount = "Amount";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdateBy = "LastUpdateBy";		            
		public const string Property_LastUpdateDate = "LastUpdateDate";		            
		public const string Property_TermSheetId = "TermSheetId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _PayrollMileageId;	            
		private Guid _CompanyId;	            
		private Double _MileageMin;	            
		private Double _MileageMax;	            
		private Double _Amount;	            
		private Guid _CreatedBy;	            
		private Nullable<DateTime> _CreatedDate;	            
		private Guid _LastUpdateBy;	            
		private Nullable<DateTime> _LastUpdateDate;	            
		private Guid _TermSheetId;	            
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
		public Guid PayrollMileageId
		{	
			get{ return _PayrollMileageId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PayrollMileageId, value, _PayrollMileageId);
				if (PropertyChanging(args))
				{
					_PayrollMileageId = value;
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
		public Double MileageMin
		{	
			get{ return _MileageMin; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MileageMin, value, _MileageMin);
				if (PropertyChanging(args))
				{
					_MileageMin = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Double MileageMax
		{	
			get{ return _MileageMax; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MileageMax, value, _MileageMax);
				if (PropertyChanging(args))
				{
					_MileageMax = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Double Amount
		{	
			get{ return _Amount; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Amount, value, _Amount);
				if (PropertyChanging(args))
				{
					_Amount = value;
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
		public Nullable<DateTime> CreatedDate
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
		public Guid LastUpdateBy
		{	
			get{ return _LastUpdateBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastUpdateBy, value, _LastUpdateBy);
				if (PropertyChanging(args))
				{
					_LastUpdateBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> LastUpdateDate
		{	
			get{ return _LastUpdateDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastUpdateDate, value, _LastUpdateDate);
				if (PropertyChanging(args))
				{
					_LastUpdateDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid TermSheetId
		{	
			get{ return _TermSheetId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TermSheetId, value, _TermSheetId);
				if (PropertyChanging(args))
				{
					_TermSheetId = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  PayrollMileageBase Clone()
		{
			PayrollMileageBase newObj = new  PayrollMileageBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.PayrollMileageId = this.PayrollMileageId;						
			newObj.CompanyId = this.CompanyId;						
			newObj.MileageMin = this.MileageMin;						
			newObj.MileageMax = this.MileageMax;						
			newObj.Amount = this.Amount;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.LastUpdateBy = this.LastUpdateBy;						
			newObj.LastUpdateDate = this.LastUpdateDate;						
			newObj.TermSheetId = this.TermSheetId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(PayrollMileageBase.Property_Id, Id);				
			info.AddValue(PayrollMileageBase.Property_PayrollMileageId, PayrollMileageId);				
			info.AddValue(PayrollMileageBase.Property_CompanyId, CompanyId);				
			info.AddValue(PayrollMileageBase.Property_MileageMin, MileageMin);				
			info.AddValue(PayrollMileageBase.Property_MileageMax, MileageMax);				
			info.AddValue(PayrollMileageBase.Property_Amount, Amount);				
			info.AddValue(PayrollMileageBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(PayrollMileageBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(PayrollMileageBase.Property_LastUpdateBy, LastUpdateBy);				
			info.AddValue(PayrollMileageBase.Property_LastUpdateDate, LastUpdateDate);				
			info.AddValue(PayrollMileageBase.Property_TermSheetId, TermSheetId);				
		}
		#endregion

		
	}
}
