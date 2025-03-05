using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomerContactTrackBase", Namespace = "http://www.piistech.com//entities")]
	public class CustomerContactTrackBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CustomerId = 1,
			CustomerPlatform = 2,
			Note = 3,
			CreatedDate = 4,
			PlatformId = 5
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_CustomerPlatform = "CustomerPlatform";		            
		public const string Property_Note = "Note";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_PlatformId = "PlatformId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CustomerId;	            
		private String _CustomerPlatform;	            
		private String _Note;	            
		private Nullable<DateTime> _CreatedDate;	            
		private Nullable<Int32> _PlatformId;	            
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
		public String CustomerPlatform
		{	
			get{ return _CustomerPlatform; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CustomerPlatform, value, _CustomerPlatform);
				if (PropertyChanging(args))
				{
					_CustomerPlatform = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Note
		{	
			get{ return _Note; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Note, value, _Note);
				if (PropertyChanging(args))
				{
					_Note = value;
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
		public Nullable<Int32> PlatformId
		{	
			get{ return _PlatformId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_PlatformId, value, _PlatformId);
				if (PropertyChanging(args))
				{
					_PlatformId = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  CustomerContactTrackBase Clone()
		{
			CustomerContactTrackBase newObj = new  CustomerContactTrackBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CustomerId = this.CustomerId;						
			newObj.CustomerPlatform = this.CustomerPlatform;						
			newObj.Note = this.Note;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.PlatformId = this.PlatformId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomerContactTrackBase.Property_Id, Id);				
			info.AddValue(CustomerContactTrackBase.Property_CustomerId, CustomerId);				
			info.AddValue(CustomerContactTrackBase.Property_CustomerPlatform, CustomerPlatform);				
			info.AddValue(CustomerContactTrackBase.Property_Note, Note);				
			info.AddValue(CustomerContactTrackBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(CustomerContactTrackBase.Property_PlatformId, PlatformId);				
		}
		#endregion

		
	}
}
