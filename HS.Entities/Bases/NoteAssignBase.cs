using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "NoteAssignBase", Namespace = "http://www.piistech.com//entities")]
	public class NoteAssignBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			NoteId = 1,
			EmployeeId = 2
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_NoteId = "NoteId";		            
		public const string Property_EmployeeId = "EmployeeId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Int32 _NoteId;	            
		private Guid _EmployeeId;	            
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
		public Int32 NoteId
		{	
			get{ return _NoteId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_NoteId, value, _NoteId);
				if (PropertyChanging(args))
				{
					_NoteId = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  NoteAssignBase Clone()
		{
			NoteAssignBase newObj = new  NoteAssignBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.NoteId = this.NoteId;						
			newObj.EmployeeId = this.EmployeeId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(NoteAssignBase.Property_Id, Id);				
			info.AddValue(NoteAssignBase.Property_NoteId, NoteId);				
			info.AddValue(NoteAssignBase.Property_EmployeeId, EmployeeId);				
		}
		#endregion

		
	}
}
