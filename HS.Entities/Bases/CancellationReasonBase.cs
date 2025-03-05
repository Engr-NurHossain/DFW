using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CancellationReasonBase", Namespace = "http://www.piistech.com//entities")]
	public class CancellationReasonBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			Reason = 1,
			OrderBy = 2,
			CompanyId = 3,
			IsActive = 4
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_Reason = "Reason";		            
		public const string Property_OrderBy = "OrderBy";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_IsActive = "IsActive";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _Reason;	            
		private Nullable<Int32> _OrderBy;	            
		private Guid _CompanyId;	            
		private Boolean _IsActive;	            
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
		public String Reason
		{	
			get{ return _Reason; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Reason, value, _Reason);
				if (PropertyChanging(args))
				{
					_Reason = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> OrderBy
		{	
			get{ return _OrderBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OrderBy, value, _OrderBy);
				if (PropertyChanging(args))
				{
					_OrderBy = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  CancellationReasonBase Clone()
		{
			CancellationReasonBase newObj = new  CancellationReasonBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.Reason = this.Reason;						
			newObj.OrderBy = this.OrderBy;						
			newObj.CompanyId = this.CompanyId;						
			newObj.IsActive = this.IsActive;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CancellationReasonBase.Property_Id, Id);				
			info.AddValue(CancellationReasonBase.Property_Reason, Reason);				
			info.AddValue(CancellationReasonBase.Property_OrderBy, OrderBy);				
			info.AddValue(CancellationReasonBase.Property_CompanyId, CompanyId);				
			info.AddValue(CancellationReasonBase.Property_IsActive, IsActive);				
		}
		#endregion

		
	}
}
