using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "EmployeeComputerBase", Namespace = "http://www.piistech.com//entities")]
	public class EmployeeComputerBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			UserId = 2,
			ComputerName = 3,
			ComputerPassword = 4,
			CreatedBy = 5,
			LastUpdatedBy = 6,
			LastUpdatedDate = 7
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_UserId = "UserId";		            
		public const string Property_ComputerName = "ComputerName";		            
		public const string Property_ComputerPassword = "ComputerPassword";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_LastUpdatedBy = "LastUpdatedBy";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private Guid _UserId;	            
		private String _ComputerName;	            
		private String _ComputerPassword;	            
		private Guid _CreatedBy;	            
		private Guid _LastUpdatedBy;	            
		private DateTime _LastUpdatedDate;	            
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
		public Guid UserId
		{	
			get{ return _UserId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UserId, value, _UserId);
				if (PropertyChanging(args))
				{
					_UserId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ComputerName
		{	
			get{ return _ComputerName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ComputerName, value, _ComputerName);
				if (PropertyChanging(args))
				{
					_ComputerName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String ComputerPassword
		{	
			get{ return _ComputerPassword; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_ComputerPassword, value, _ComputerPassword);
				if (PropertyChanging(args))
				{
					_ComputerPassword = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  EmployeeComputerBase Clone()
		{
			EmployeeComputerBase newObj = new  EmployeeComputerBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.UserId = this.UserId;						
			newObj.ComputerName = this.ComputerName;						
			newObj.ComputerPassword = this.ComputerPassword;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.LastUpdatedBy = this.LastUpdatedBy;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(EmployeeComputerBase.Property_Id, Id);				
			info.AddValue(EmployeeComputerBase.Property_CompanyId, CompanyId);				
			info.AddValue(EmployeeComputerBase.Property_UserId, UserId);				
			info.AddValue(EmployeeComputerBase.Property_ComputerName, ComputerName);				
			info.AddValue(EmployeeComputerBase.Property_ComputerPassword, ComputerPassword);				
			info.AddValue(EmployeeComputerBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(EmployeeComputerBase.Property_LastUpdatedBy, LastUpdatedBy);				
			info.AddValue(EmployeeComputerBase.Property_LastUpdatedDate, LastUpdatedDate);				
		}
		#endregion

		
	}
}
