using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "CustomerSnapshotBase", Namespace = "http://www.piistech.com//entities")]
	public class CustomerSnapshotBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CustomerId = 1,
			CompanyId = 2,
			Description = 3,
			Logdate = 4,
			Updatedby = 5,
			Type = 6
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CustomerId = "CustomerId";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_Description = "Description";		            
		public const string Property_Logdate = "Logdate";		            
		public const string Property_Updatedby = "Updatedby";		            
		public const string Property_Type = "Type";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CustomerId;	            
		private Guid _CompanyId;	            
		private String _Description;	            
		private DateTime _Logdate;	            
		private String _Updatedby;	            
		private String _Type;	            
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
		public String Description
		{	
			get{ return _Description; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Description, value, _Description);
				if (PropertyChanging(args))
				{
					_Description = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime Logdate
		{	
			get{ return _Logdate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Logdate, value, _Logdate);
				if (PropertyChanging(args))
				{
					_Logdate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Updatedby
		{	
			get{ return _Updatedby; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Updatedby, value, _Updatedby);
				if (PropertyChanging(args))
				{
					_Updatedby = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Type
		{	
			get{ return _Type; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Type, value, _Type);
				if (PropertyChanging(args))
				{
					_Type = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  CustomerSnapshotBase Clone()
		{
			CustomerSnapshotBase newObj = new  CustomerSnapshotBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CustomerId = this.CustomerId;						
			newObj.CompanyId = this.CompanyId;						
			newObj.Description = this.Description;						
			newObj.Logdate = this.Logdate;						
			newObj.Updatedby = this.Updatedby;						
			newObj.Type = this.Type;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(CustomerSnapshotBase.Property_Id, Id);				
			info.AddValue(CustomerSnapshotBase.Property_CustomerId, CustomerId);				
			info.AddValue(CustomerSnapshotBase.Property_CompanyId, CompanyId);				
			info.AddValue(CustomerSnapshotBase.Property_Description, Description);				
			info.AddValue(CustomerSnapshotBase.Property_Logdate, Logdate);				
			info.AddValue(CustomerSnapshotBase.Property_Updatedby, Updatedby);				
			info.AddValue(CustomerSnapshotBase.Property_Type, Type);				
		}
		#endregion

		
	}
}
