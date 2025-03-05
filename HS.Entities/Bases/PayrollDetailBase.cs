using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "PayrollDetailBase", Namespace = "http://www.hims-tech.com//entities")]
	public class PayrollDetailBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			RepName = 2,
			RepCommission = 3,
			RepHoldback = 4,
			OverrideRep1 = 5,
			Override1 = 6,
			OverrideRep2 = 7,
			Override2 = 8,
			RepPaidDate = 9,
			TechName = 10,
			TechPay = 11,
			TechHoldback = 12,
			TechPaidDate = 13,
			OpenerCommission = 14,
			MiscRep1 = 15,
			MiscCommission1 = 16,
			MiscRep2 = 17,
			MiscCommission2 = 18,
			MiscRep3 = 19,
			MiscCommission3 = 20,
			MiscRep4 = 21,
			MiscCommission4 = 22,
			MiscRep5 = 23,
			MiscCommission5 = 24,
			MiscPaidDate = 25,
			CreatedBy = 26,
			CreatedDate = 27,
			LastUpdatedBy = 28,
			LastUpdatedDate = 29,
			RMAAccountNo = 30
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_RepName = "RepName";		            
		public const string Property_RepCommission = "RepCommission";		            
		public const string Property_RepHoldback = "RepHoldback";		            
		public const string Property_OverrideRep1 = "OverrideRep1";		            
		public const string Property_Override1 = "Override1";		            
		public const string Property_OverrideRep2 = "OverrideRep2";		            
		public const string Property_Override2 = "Override2";		            
		public const string Property_RepPaidDate = "RepPaidDate";		            
		public const string Property_TechName = "TechName";		            
		public const string Property_TechPay = "TechPay";		            
		public const string Property_TechHoldback = "TechHoldback";		            
		public const string Property_TechPaidDate = "TechPaidDate";		            
		public const string Property_OpenerCommission = "OpenerCommission";		            
		public const string Property_MiscRep1 = "MiscRep1";		            
		public const string Property_MiscCommission1 = "MiscCommission1";		            
		public const string Property_MiscRep2 = "MiscRep2";		            
		public const string Property_MiscCommission2 = "MiscCommission2";		            
		public const string Property_MiscRep3 = "MiscRep3";		            
		public const string Property_MiscCommission3 = "MiscCommission3";		            
		public const string Property_MiscRep4 = "MiscRep4";		            
		public const string Property_MiscCommission4 = "MiscCommission4";		            
		public const string Property_MiscRep5 = "MiscRep5";		            
		public const string Property_MiscCommission5 = "MiscCommission5";		            
		public const string Property_MiscPaidDate = "MiscPaidDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		public const string Property_RMAAccountNo = "RMAAccountNo";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private String _RepName;	            
		private Nullable<Double> _RepCommission;	            
		private Nullable<Double> _RepHoldback;	            
		private String _OverrideRep1;	            
		private String _Override1;	            
		private String _OverrideRep2;	            
		private String _Override2;	            
		private Nullable<DateTime> _RepPaidDate;	            
		private String _TechName;	            
		private Nullable<Double> _TechPay;	            
		private Nullable<Double> _TechHoldback;	            
		private Nullable<DateTime> _TechPaidDate;	            
		private Nullable<Double> _OpenerCommission;	            
		private String _MiscRep1;	            
		private Nullable<Double> _MiscCommission1;	            
		private String _MiscRep2;	            
		private Nullable<Double> _MiscCommission2;	            
		private String _MiscRep3;	            
		private Nullable<Double> _MiscCommission3;	            
		private String _MiscRep4;	            
		private Nullable<Double> _MiscCommission4;	            
		private String _MiscRep5;	            
		private Nullable<Double> _MiscCommission5;	            
		private Nullable<DateTime> _MiscPaidDate;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
		private Guid _LastUpdatedBy;	            
		private DateTime _LastUpdatedDate;	            
		private Nullable<Int32> _RMAAccountNo;	            
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
		public String RepName
		{	
			get{ return _RepName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RepName, value, _RepName);
				if (PropertyChanging(args))
				{
					_RepName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> RepCommission
		{	
			get{ return _RepCommission; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RepCommission, value, _RepCommission);
				if (PropertyChanging(args))
				{
					_RepCommission = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> RepHoldback
		{	
			get{ return _RepHoldback; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RepHoldback, value, _RepHoldback);
				if (PropertyChanging(args))
				{
					_RepHoldback = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String OverrideRep1
		{	
			get{ return _OverrideRep1; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OverrideRep1, value, _OverrideRep1);
				if (PropertyChanging(args))
				{
					_OverrideRep1 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Override1
		{	
			get{ return _Override1; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Override1, value, _Override1);
				if (PropertyChanging(args))
				{
					_Override1 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String OverrideRep2
		{	
			get{ return _OverrideRep2; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OverrideRep2, value, _OverrideRep2);
				if (PropertyChanging(args))
				{
					_OverrideRep2 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Override2
		{	
			get{ return _Override2; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Override2, value, _Override2);
				if (PropertyChanging(args))
				{
					_Override2 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> RepPaidDate
		{	
			get{ return _RepPaidDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RepPaidDate, value, _RepPaidDate);
				if (PropertyChanging(args))
				{
					_RepPaidDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TechName
		{	
			get{ return _TechName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TechName, value, _TechName);
				if (PropertyChanging(args))
				{
					_TechName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> TechPay
		{	
			get{ return _TechPay; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TechPay, value, _TechPay);
				if (PropertyChanging(args))
				{
					_TechPay = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> TechHoldback
		{	
			get{ return _TechHoldback; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TechHoldback, value, _TechHoldback);
				if (PropertyChanging(args))
				{
					_TechHoldback = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> TechPaidDate
		{	
			get{ return _TechPaidDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TechPaidDate, value, _TechPaidDate);
				if (PropertyChanging(args))
				{
					_TechPaidDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> OpenerCommission
		{	
			get{ return _OpenerCommission; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OpenerCommission, value, _OpenerCommission);
				if (PropertyChanging(args))
				{
					_OpenerCommission = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String MiscRep1
		{	
			get{ return _MiscRep1; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MiscRep1, value, _MiscRep1);
				if (PropertyChanging(args))
				{
					_MiscRep1 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> MiscCommission1
		{	
			get{ return _MiscCommission1; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MiscCommission1, value, _MiscCommission1);
				if (PropertyChanging(args))
				{
					_MiscCommission1 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String MiscRep2
		{	
			get{ return _MiscRep2; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MiscRep2, value, _MiscRep2);
				if (PropertyChanging(args))
				{
					_MiscRep2 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> MiscCommission2
		{	
			get{ return _MiscCommission2; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MiscCommission2, value, _MiscCommission2);
				if (PropertyChanging(args))
				{
					_MiscCommission2 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String MiscRep3
		{	
			get{ return _MiscRep3; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MiscRep3, value, _MiscRep3);
				if (PropertyChanging(args))
				{
					_MiscRep3 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> MiscCommission3
		{	
			get{ return _MiscCommission3; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MiscCommission3, value, _MiscCommission3);
				if (PropertyChanging(args))
				{
					_MiscCommission3 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String MiscRep4
		{	
			get{ return _MiscRep4; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MiscRep4, value, _MiscRep4);
				if (PropertyChanging(args))
				{
					_MiscRep4 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> MiscCommission4
		{	
			get{ return _MiscCommission4; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MiscCommission4, value, _MiscCommission4);
				if (PropertyChanging(args))
				{
					_MiscCommission4 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String MiscRep5
		{	
			get{ return _MiscRep5; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MiscRep5, value, _MiscRep5);
				if (PropertyChanging(args))
				{
					_MiscRep5 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> MiscCommission5
		{	
			get{ return _MiscCommission5; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MiscCommission5, value, _MiscCommission5);
				if (PropertyChanging(args))
				{
					_MiscCommission5 = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> MiscPaidDate
		{	
			get{ return _MiscPaidDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_MiscPaidDate, value, _MiscPaidDate);
				if (PropertyChanging(args))
				{
					_MiscPaidDate = value;
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

		[DataMember]
		public Nullable<Int32> RMAAccountNo
		{	
			get{ return _RMAAccountNo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_RMAAccountNo, value, _RMAAccountNo);
				if (PropertyChanging(args))
				{
					_RMAAccountNo = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  PayrollDetailBase Clone()
		{
			PayrollDetailBase newObj = new  PayrollDetailBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.RepName = this.RepName;						
			newObj.RepCommission = this.RepCommission;						
			newObj.RepHoldback = this.RepHoldback;						
			newObj.OverrideRep1 = this.OverrideRep1;						
			newObj.Override1 = this.Override1;						
			newObj.OverrideRep2 = this.OverrideRep2;						
			newObj.Override2 = this.Override2;						
			newObj.RepPaidDate = this.RepPaidDate;						
			newObj.TechName = this.TechName;						
			newObj.TechPay = this.TechPay;						
			newObj.TechHoldback = this.TechHoldback;						
			newObj.TechPaidDate = this.TechPaidDate;						
			newObj.OpenerCommission = this.OpenerCommission;						
			newObj.MiscRep1 = this.MiscRep1;						
			newObj.MiscCommission1 = this.MiscCommission1;						
			newObj.MiscRep2 = this.MiscRep2;						
			newObj.MiscCommission2 = this.MiscCommission2;						
			newObj.MiscRep3 = this.MiscRep3;						
			newObj.MiscCommission3 = this.MiscCommission3;						
			newObj.MiscRep4 = this.MiscRep4;						
			newObj.MiscCommission4 = this.MiscCommission4;						
			newObj.MiscRep5 = this.MiscRep5;						
			newObj.MiscCommission5 = this.MiscCommission5;						
			newObj.MiscPaidDate = this.MiscPaidDate;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			newObj.RMAAccountNo = this.RMAAccountNo;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(PayrollDetailBase.Property_Id, Id);				
			info.AddValue(PayrollDetailBase.Property_CompanyId, CompanyId);				
			info.AddValue(PayrollDetailBase.Property_RepName, RepName);				
			info.AddValue(PayrollDetailBase.Property_RepCommission, RepCommission);				
			info.AddValue(PayrollDetailBase.Property_RepHoldback, RepHoldback);				
			info.AddValue(PayrollDetailBase.Property_OverrideRep1, OverrideRep1);				
			info.AddValue(PayrollDetailBase.Property_Override1, Override1);				
			info.AddValue(PayrollDetailBase.Property_OverrideRep2, OverrideRep2);				
			info.AddValue(PayrollDetailBase.Property_Override2, Override2);				
			info.AddValue(PayrollDetailBase.Property_RepPaidDate, RepPaidDate);				
			info.AddValue(PayrollDetailBase.Property_TechName, TechName);				
			info.AddValue(PayrollDetailBase.Property_TechPay, TechPay);				
			info.AddValue(PayrollDetailBase.Property_TechHoldback, TechHoldback);				
			info.AddValue(PayrollDetailBase.Property_TechPaidDate, TechPaidDate);				
			info.AddValue(PayrollDetailBase.Property_OpenerCommission, OpenerCommission);				
			info.AddValue(PayrollDetailBase.Property_MiscRep1, MiscRep1);				
			info.AddValue(PayrollDetailBase.Property_MiscCommission1, MiscCommission1);				
			info.AddValue(PayrollDetailBase.Property_MiscRep2, MiscRep2);				
			info.AddValue(PayrollDetailBase.Property_MiscCommission2, MiscCommission2);				
			info.AddValue(PayrollDetailBase.Property_MiscRep3, MiscRep3);				
			info.AddValue(PayrollDetailBase.Property_MiscCommission3, MiscCommission3);				
			info.AddValue(PayrollDetailBase.Property_MiscRep4, MiscRep4);				
			info.AddValue(PayrollDetailBase.Property_MiscCommission4, MiscCommission4);				
			info.AddValue(PayrollDetailBase.Property_MiscRep5, MiscRep5);				
			info.AddValue(PayrollDetailBase.Property_MiscCommission5, MiscCommission5);				
			info.AddValue(PayrollDetailBase.Property_MiscPaidDate, MiscPaidDate);				
			info.AddValue(PayrollDetailBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(PayrollDetailBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(PayrollDetailBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(PayrollDetailBase.Property_LastUpdatedDate, LastUpdatedDate);				
			info.AddValue(PayrollDetailBase.Property_RMAAccountNo, RMAAccountNo);				
		}
		#endregion

		
	}
}
