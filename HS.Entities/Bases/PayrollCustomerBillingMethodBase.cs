using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "PayrollCustomerBillingMethodBase", Namespace = "http://www.hims-tech.com//entities")]
	public class PayrollCustomerBillingMethodBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			PayrollCustomerBillingMethodId = 1,
			CompanyId = 2,
			BillingMethod = 3,
			Point = 4,
			CreatedBy = 5,
			CreatedDate = 6,
			LastUpdateBy = 7,
			LastUpdateDate = 8,
			TermSheetId = 9
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_PayrollCustomerBillingMethodId = "PayrollCustomerBillingMethodId";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_BillingMethod = "BillingMethod";		            
		public const string Property_Point = "Point";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdateBy = "LastUpdateBy";		            
		public const string Property_LastUpdateDate = "LastUpdateDate";		            
		public const string Property_TermSheetId = "TermSheetId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _PayrollCustomerBillingMethodId;	            
		private Guid _CompanyId;	            
		private String _BillingMethod;	            
		private Int32 _Point;	            
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
		public Guid PayrollCustomerBillingMethodId
		{	
			get{ return _PayrollCustomerBillingMethodId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PayrollCustomerBillingMethodId, value, _PayrollCustomerBillingMethodId);
				if (PropertyChanging(args))
				{
					_PayrollCustomerBillingMethodId = value;
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
		public String BillingMethod
		{	
			get{ return _BillingMethod; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BillingMethod, value, _BillingMethod);
				if (PropertyChanging(args))
				{
					_BillingMethod = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Int32 Point
		{	
			get{ return _Point; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Point, value, _Point);
				if (PropertyChanging(args))
				{
					_Point = value;
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
		public  PayrollCustomerBillingMethodBase Clone()
		{
			PayrollCustomerBillingMethodBase newObj = new  PayrollCustomerBillingMethodBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.PayrollCustomerBillingMethodId = this.PayrollCustomerBillingMethodId;						
			newObj.CompanyId = this.CompanyId;						
			newObj.BillingMethod = this.BillingMethod;						
			newObj.Point = this.Point;						
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
			info.AddValue(PayrollCustomerBillingMethodBase.Property_Id, Id);				
			info.AddValue(PayrollCustomerBillingMethodBase.Property_PayrollCustomerBillingMethodId, PayrollCustomerBillingMethodId);				
			info.AddValue(PayrollCustomerBillingMethodBase.Property_CompanyId, CompanyId);				
			info.AddValue(PayrollCustomerBillingMethodBase.Property_BillingMethod, BillingMethod);				
			info.AddValue(PayrollCustomerBillingMethodBase.Property_Point, Point);				
			info.AddValue(PayrollCustomerBillingMethodBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(PayrollCustomerBillingMethodBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(PayrollCustomerBillingMethodBase.Property_LastUpdateBy, LastUpdateBy);				
			info.AddValue(PayrollCustomerBillingMethodBase.Property_LastUpdateDate, LastUpdateDate);				
			info.AddValue(PayrollCustomerBillingMethodBase.Property_TermSheetId, TermSheetId);				
		}
		#endregion

		
	}
}
