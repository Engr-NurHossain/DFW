using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "PayrollAgreementLengthBase", Namespace = "http://www.hims-tech.com//entities")]
	public class PayrollAgreementLengthBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			PayrollAgreementLengthId = 1,
			CompanyId = 2,
			AgreementLength = 3,
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
		public const string Property_PayrollAgreementLengthId = "PayrollAgreementLengthId";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_AgreementLength = "AgreementLength";		            
		public const string Property_Point = "Point";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdateBy = "LastUpdateBy";		            
		public const string Property_LastUpdateDate = "LastUpdateDate";		            
		public const string Property_TermSheetId = "TermSheetId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _PayrollAgreementLengthId;	            
		private Guid _CompanyId;	            
		private String _AgreementLength;	            
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
		public Guid PayrollAgreementLengthId
		{	
			get{ return _PayrollAgreementLengthId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PayrollAgreementLengthId, value, _PayrollAgreementLengthId);
				if (PropertyChanging(args))
				{
					_PayrollAgreementLengthId = value;
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
		public String AgreementLength
		{	
			get{ return _AgreementLength; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AgreementLength, value, _AgreementLength);
				if (PropertyChanging(args))
				{
					_AgreementLength = value;
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
		public  PayrollAgreementLengthBase Clone()
		{
			PayrollAgreementLengthBase newObj = new  PayrollAgreementLengthBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.PayrollAgreementLengthId = this.PayrollAgreementLengthId;						
			newObj.CompanyId = this.CompanyId;						
			newObj.AgreementLength = this.AgreementLength;						
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
			info.AddValue(PayrollAgreementLengthBase.Property_Id, Id);				
			info.AddValue(PayrollAgreementLengthBase.Property_PayrollAgreementLengthId, PayrollAgreementLengthId);				
			info.AddValue(PayrollAgreementLengthBase.Property_CompanyId, CompanyId);				
			info.AddValue(PayrollAgreementLengthBase.Property_AgreementLength, AgreementLength);				
			info.AddValue(PayrollAgreementLengthBase.Property_Point, Point);				
			info.AddValue(PayrollAgreementLengthBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(PayrollAgreementLengthBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(PayrollAgreementLengthBase.Property_LastUpdateBy, LastUpdateBy);				
			info.AddValue(PayrollAgreementLengthBase.Property_LastUpdateDate, LastUpdateDate);				
			info.AddValue(PayrollAgreementLengthBase.Property_TermSheetId, TermSheetId);				
		}
		#endregion

		
	}
}
