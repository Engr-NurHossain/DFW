using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "PayrollPassThrusBase", Namespace = "http://www.hims-tech.com//entities")]
	public class PayrollPassThrusBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			PayrollPassThrusId = 1,
			CompanyId = 2,
			PassThrus = 3,
			IsBase = 4,
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
		public const string Property_PayrollPassThrusId = "PayrollPassThrusId";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_PassThrus = "PassThrus";		            
		public const string Property_IsBase = "IsBase";		            
		public const string Property_Amount = "Amount";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdateBy = "LastUpdateBy";		            
		public const string Property_LastUpdateDate = "LastUpdateDate";		            
		public const string Property_TermSheetId = "TermSheetId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _PayrollPassThrusId;	            
		private Guid _CompanyId;	            
		private String _PassThrus;	            
		private Nullable<Boolean> _IsBase;	            
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
		public Guid PayrollPassThrusId
		{	
			get{ return _PayrollPassThrusId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PayrollPassThrusId, value, _PayrollPassThrusId);
				if (PropertyChanging(args))
				{
					_PayrollPassThrusId = value;
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
		public String PassThrus
		{	
			get{ return _PassThrus; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PassThrus, value, _PassThrus);
				if (PropertyChanging(args))
				{
					_PassThrus = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsBase
		{	
			get{ return _IsBase; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsBase, value, _IsBase);
				if (PropertyChanging(args))
				{
					_IsBase = value;
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
		public  PayrollPassThrusBase Clone()
		{
			PayrollPassThrusBase newObj = new  PayrollPassThrusBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.PayrollPassThrusId = this.PayrollPassThrusId;						
			newObj.CompanyId = this.CompanyId;						
			newObj.PassThrus = this.PassThrus;						
			newObj.IsBase = this.IsBase;						
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
			info.AddValue(PayrollPassThrusBase.Property_Id, Id);				
			info.AddValue(PayrollPassThrusBase.Property_PayrollPassThrusId, PayrollPassThrusId);				
			info.AddValue(PayrollPassThrusBase.Property_CompanyId, CompanyId);				
			info.AddValue(PayrollPassThrusBase.Property_PassThrus, PassThrus);				
			info.AddValue(PayrollPassThrusBase.Property_IsBase, IsBase);				
			info.AddValue(PayrollPassThrusBase.Property_Amount, Amount);				
			info.AddValue(PayrollPassThrusBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(PayrollPassThrusBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(PayrollPassThrusBase.Property_LastUpdateBy, LastUpdateBy);				
			info.AddValue(PayrollPassThrusBase.Property_LastUpdateDate, LastUpdateDate);				
			info.AddValue(PayrollPassThrusBase.Property_TermSheetId, TermSheetId);				
		}
		#endregion

		
	}
}
