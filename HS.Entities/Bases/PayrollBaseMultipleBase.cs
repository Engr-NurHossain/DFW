using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "PayrollBaseMultipleBase", Namespace = "http://www.hims-tech.com//entities")]
	public class PayrollBaseMultipleBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			PayrollBaseMultipleId = 1,
			CompanyId = 2,
			BaseMultiple = 3,
			Amount = 4,
			CreatedBy = 5,
			CreatedDate = 6,
			LastUpdateBy = 7,
			LastUpdateDate = 8,
			TermSheetId = 9
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_PayrollBaseMultipleId = "PayrollBaseMultipleId";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_BaseMultiple = "BaseMultiple";		            
		public const string Property_Amount = "Amount";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdateBy = "LastUpdateBy";		            
		public const string Property_LastUpdateDate = "LastUpdateDate";		            
		public const string Property_TermSheetId = "TermSheetId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _PayrollBaseMultipleId;	            
		private Guid _CompanyId;	            
		private String _BaseMultiple;	            
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
		public Guid PayrollBaseMultipleId
		{	
			get{ return _PayrollBaseMultipleId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PayrollBaseMultipleId, value, _PayrollBaseMultipleId);
				if (PropertyChanging(args))
				{
					_PayrollBaseMultipleId = value;
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
		public String BaseMultiple
		{	
			get{ return _BaseMultiple; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BaseMultiple, value, _BaseMultiple);
				if (PropertyChanging(args))
				{
					_BaseMultiple = value;
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
		public  PayrollBaseMultipleBase Clone()
		{
			PayrollBaseMultipleBase newObj = new  PayrollBaseMultipleBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.PayrollBaseMultipleId = this.PayrollBaseMultipleId;						
			newObj.CompanyId = this.CompanyId;						
			newObj.BaseMultiple = this.BaseMultiple;						
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
			info.AddValue(PayrollBaseMultipleBase.Property_Id, Id);				
			info.AddValue(PayrollBaseMultipleBase.Property_PayrollBaseMultipleId, PayrollBaseMultipleId);				
			info.AddValue(PayrollBaseMultipleBase.Property_CompanyId, CompanyId);				
			info.AddValue(PayrollBaseMultipleBase.Property_BaseMultiple, BaseMultiple);				
			info.AddValue(PayrollBaseMultipleBase.Property_Amount, Amount);				
			info.AddValue(PayrollBaseMultipleBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(PayrollBaseMultipleBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(PayrollBaseMultipleBase.Property_LastUpdateBy, LastUpdateBy);				
			info.AddValue(PayrollBaseMultipleBase.Property_LastUpdateDate, LastUpdateDate);				
			info.AddValue(PayrollBaseMultipleBase.Property_TermSheetId, TermSheetId);				
		}
		#endregion

		
	}
}
