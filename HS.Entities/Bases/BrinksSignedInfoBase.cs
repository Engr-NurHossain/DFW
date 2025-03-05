using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "BrinksSignedInfoBase", Namespace = "http://www.hims-tech.com//entities")]
	public class BrinksSignedInfoBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CustomerId = 1,
			IsSigned = 2,
			HasBillingInfo = 3,
			HasBusinessPicture = 4,
			CreatedBy = 5,
			CreatedDate = 6,
			IsCreditCheck = 7
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_IsSigned = "IsSigned";		            
		public const string Property_HasBillingInfo = "HasBillingInfo";		            
		public const string Property_HasBusinessPicture = "HasBusinessPicture";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_IsCreditCheck = "IsCreditCheck";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CustomerId;	            
		private Nullable<Boolean> _IsSigned;	            
		private Nullable<Boolean> _HasBillingInfo;	            
		private Nullable<Boolean> _HasBusinessPicture;	            
		private Guid _CreatedBy;	            
		private Nullable<DateTime> _CreatedDate;	            
		private Nullable<Boolean> _IsCreditCheck;	            
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
		public Guid CustomerId
		{	
			get{ return _CustomerId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CustomerId, value, _CustomerId);
				if (PropertyChanging(args))
				{
					_CustomerId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> IsSigned
		{	
			get{ return _IsSigned; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsSigned, value, _IsSigned);
				if (PropertyChanging(args))
				{
					_IsSigned = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> HasBillingInfo
		{	
			get{ return _HasBillingInfo; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_HasBillingInfo, value, _HasBillingInfo);
				if (PropertyChanging(args))
				{
					_HasBillingInfo = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Boolean> HasBusinessPicture
		{	
			get{ return _HasBusinessPicture; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_HasBusinessPicture, value, _HasBusinessPicture);
				if (PropertyChanging(args))
				{
					_HasBusinessPicture = value;
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
		public Nullable<Boolean> IsCreditCheck
		{	
			get{ return _IsCreditCheck; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsCreditCheck, value, _IsCreditCheck);
				if (PropertyChanging(args))
				{
					_IsCreditCheck = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  BrinksSignedInfoBase Clone()
		{
			BrinksSignedInfoBase newObj = new  BrinksSignedInfoBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CustomerId = this.CustomerId;						
			newObj.IsSigned = this.IsSigned;						
			newObj.HasBillingInfo = this.HasBillingInfo;						
			newObj.HasBusinessPicture = this.HasBusinessPicture;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.IsCreditCheck = this.IsCreditCheck;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(BrinksSignedInfoBase.Property_Id, Id);				
			info.AddValue(BrinksSignedInfoBase.Property_CustomerId, CustomerId);				
			info.AddValue(BrinksSignedInfoBase.Property_IsSigned, IsSigned);				
			info.AddValue(BrinksSignedInfoBase.Property_HasBillingInfo, HasBillingInfo);				
			info.AddValue(BrinksSignedInfoBase.Property_HasBusinessPicture, HasBusinessPicture);				
			info.AddValue(BrinksSignedInfoBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(BrinksSignedInfoBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(BrinksSignedInfoBase.Property_IsCreditCheck, IsCreditCheck);				
		}
		#endregion

		
	}
}
