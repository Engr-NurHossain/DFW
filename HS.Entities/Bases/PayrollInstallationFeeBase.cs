using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "PayrollInstallationFeeBase", Namespace = "http://www.hims-tech.com//entities")]
	public class PayrollInstallationFeeBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			PayrollInstallationFeeId = 1,
			CompanyId = 2,
			InstallationFeeMin = 3,
			InstallationFeeMax = 4,
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
		public const string Property_PayrollInstallationFeeId = "PayrollInstallationFeeId";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_InstallationFeeMin = "InstallationFeeMin";		            
		public const string Property_InstallationFeeMax = "InstallationFeeMax";		            
		public const string Property_Amount = "Amount";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdateBy = "LastUpdateBy";		            
		public const string Property_LastUpdateDate = "LastUpdateDate";		            
		public const string Property_TermSheetId = "TermSheetId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _PayrollInstallationFeeId;	            
		private Guid _CompanyId;	            
		private Double _InstallationFeeMin;	            
		private Double _InstallationFeeMax;	            
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
		public Guid PayrollInstallationFeeId
		{	
			get{ return _PayrollInstallationFeeId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PayrollInstallationFeeId, value, _PayrollInstallationFeeId);
				if (PropertyChanging(args))
				{
					_PayrollInstallationFeeId = value;
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
		public Double InstallationFeeMin
		{	
			get{ return _InstallationFeeMin; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InstallationFeeMin, value, _InstallationFeeMin);
				if (PropertyChanging(args))
				{
					_InstallationFeeMin = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Double InstallationFeeMax
		{	
			get{ return _InstallationFeeMax; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_InstallationFeeMax, value, _InstallationFeeMax);
				if (PropertyChanging(args))
				{
					_InstallationFeeMax = value;
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
		public  PayrollInstallationFeeBase Clone()
		{
			PayrollInstallationFeeBase newObj = new  PayrollInstallationFeeBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.PayrollInstallationFeeId = this.PayrollInstallationFeeId;						
			newObj.CompanyId = this.CompanyId;						
			newObj.InstallationFeeMin = this.InstallationFeeMin;						
			newObj.InstallationFeeMax = this.InstallationFeeMax;						
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
			info.AddValue(PayrollInstallationFeeBase.Property_Id, Id);				
			info.AddValue(PayrollInstallationFeeBase.Property_PayrollInstallationFeeId, PayrollInstallationFeeId);				
			info.AddValue(PayrollInstallationFeeBase.Property_CompanyId, CompanyId);				
			info.AddValue(PayrollInstallationFeeBase.Property_InstallationFeeMin, InstallationFeeMin);				
			info.AddValue(PayrollInstallationFeeBase.Property_InstallationFeeMax, InstallationFeeMax);				
			info.AddValue(PayrollInstallationFeeBase.Property_Amount, Amount);				
			info.AddValue(PayrollInstallationFeeBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(PayrollInstallationFeeBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(PayrollInstallationFeeBase.Property_LastUpdateBy, LastUpdateBy);				
			info.AddValue(PayrollInstallationFeeBase.Property_LastUpdateDate, LastUpdateDate);				
			info.AddValue(PayrollInstallationFeeBase.Property_TermSheetId, TermSheetId);				
		}
		#endregion

		
	}
}
