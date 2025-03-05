using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "EmployeeLeadSourceBase", Namespace = "http://www.piistech.com//entities")]
	public class EmployeeLeadSourceBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			EmployeeId = 1,
			LeadSource = 2
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_EmployeeId = "EmployeeId";		            
		public const string Property_LeadSource = "LeadSource";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _EmployeeId;	            
		private String _LeadSource;	            
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
		public Guid EmployeeId
		{	
			get{ return _EmployeeId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EmployeeId, value, _EmployeeId);
				if (PropertyChanging(args))
				{
					_EmployeeId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String LeadSource
		{	
			get{ return _LeadSource; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LeadSource, value, _LeadSource);
				if (PropertyChanging(args))
				{
					_LeadSource = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  EmployeeLeadSourceBase Clone()
		{
			EmployeeLeadSourceBase newObj = new  EmployeeLeadSourceBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.EmployeeId = this.EmployeeId;						
			newObj.LeadSource = this.LeadSource;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(EmployeeLeadSourceBase.Property_Id, Id);				
			info.AddValue(EmployeeLeadSourceBase.Property_EmployeeId, EmployeeId);				
			info.AddValue(EmployeeLeadSourceBase.Property_LeadSource, LeadSource);				
		}
		#endregion

		
	}
}
