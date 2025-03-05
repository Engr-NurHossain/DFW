using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "PayrollCreditRatingBase", Namespace = "http://www.hims-tech.com//entities")]
	public class PayrollCreditRatingBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			PayrollCreditRatingId = 1,
			CompanyId = 2,
			MinCredit = 3,
			MaxCredit = 4,
			Point = 5,
			CreatedBy = 6,
			CreatedDate = 7,
			LastUpdateBy = 8,
			LastUpdateDate = 9,
			TermSheetId = 10,
			ACHBonusWaived = 11
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_PayrollCreditRatingId = "PayrollCreditRatingId";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_MinCredit = "MinCredit";		            
		public const string Property_MaxCredit = "MaxCredit";		            
		public const string Property_Point = "Point";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdateBy = "LastUpdateBy";		            
		public const string Property_LastUpdateDate = "LastUpdateDate";		            
		public const string Property_TermSheetId = "TermSheetId";		            
		public const string Property_ACHBonusWaived = "ACHBonusWaived";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _PayrollCreditRatingId;	            
		private Guid _CompanyId;	            
		private Int32 _MinCredit;	            
		private Int32 _MaxCredit;	            
		private Int32 _Point;	            
		private Guid _CreatedBy;	            
		private Nullable<DateTime> _CreatedDate;	            
		private Guid _LastUpdateBy;	            
		private Nullable<DateTime> _LastUpdateDate;	            
		private Guid _TermSheetId;	            
		private Nullable<Boolean> _ACHBonusWaived;	            
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
		public Guid PayrollCreditRatingId
		{	
			get{ return _PayrollCreditRatingId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PayrollCreditRatingId, value, _PayrollCreditRatingId);
				if (PropertyChanging(args))
				{
					_PayrollCreditRatingId = value;
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
		public Int32 MinCredit
		{	
			get{ return _MinCredit; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MinCredit, value, _MinCredit);
				if (PropertyChanging(args))
				{
					_MinCredit = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Int32 MaxCredit
		{	
			get{ return _MaxCredit; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MaxCredit, value, _MaxCredit);
				if (PropertyChanging(args))
				{
					_MaxCredit = value;
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

		[DataMember]
		public Nullable<Boolean> ACHBonusWaived
		{	
			get{ return _ACHBonusWaived; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ACHBonusWaived, value, _ACHBonusWaived);
				if (PropertyChanging(args))
				{
					_ACHBonusWaived = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  PayrollCreditRatingBase Clone()
		{
			PayrollCreditRatingBase newObj = new  PayrollCreditRatingBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.PayrollCreditRatingId = this.PayrollCreditRatingId;						
			newObj.CompanyId = this.CompanyId;						
			newObj.MinCredit = this.MinCredit;						
			newObj.MaxCredit = this.MaxCredit;						
			newObj.Point = this.Point;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.LastUpdateBy = this.LastUpdateBy;						
			newObj.LastUpdateDate = this.LastUpdateDate;						
			newObj.TermSheetId = this.TermSheetId;						
			newObj.ACHBonusWaived = this.ACHBonusWaived;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(PayrollCreditRatingBase.Property_Id, Id);				
			info.AddValue(PayrollCreditRatingBase.Property_PayrollCreditRatingId, PayrollCreditRatingId);				
			info.AddValue(PayrollCreditRatingBase.Property_CompanyId, CompanyId);				
			info.AddValue(PayrollCreditRatingBase.Property_MinCredit, MinCredit);				
			info.AddValue(PayrollCreditRatingBase.Property_MaxCredit, MaxCredit);				
			info.AddValue(PayrollCreditRatingBase.Property_Point, Point);				
			info.AddValue(PayrollCreditRatingBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(PayrollCreditRatingBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(PayrollCreditRatingBase.Property_LastUpdateBy, LastUpdateBy);				
			info.AddValue(PayrollCreditRatingBase.Property_LastUpdateDate, LastUpdateDate);				
			info.AddValue(PayrollCreditRatingBase.Property_TermSheetId, TermSheetId);				
			info.AddValue(PayrollCreditRatingBase.Property_ACHBonusWaived, ACHBonusWaived);				
		}
		#endregion

		
	}
}
