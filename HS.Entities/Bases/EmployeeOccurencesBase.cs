using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "EmployeeOccurencesBase", Namespace = "http://www.piistech.com//entities")]
	public class EmployeeOccurencesBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			UserId = 1,
			OccurenceDate = 2,
			Amount = 3,
			Notes = 4,
			CreatedByUid = 5,
			CreatedDate = 6,
			LastUpdatedByUid = 7,
			LastUpdatedDate = 8
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_UserId = "UserId";		            
		public const string Property_OccurenceDate = "OccurenceDate";		            
		public const string Property_Amount = "Amount";		            
		public const string Property_Notes = "Notes";		            
		public const string Property_CreatedByUid = "CreatedByUid";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_LastUpdatedByUid = "LastUpdatedByUid";		            
		public const string Property_LastUpdatedDate = "LastUpdatedDate";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _UserId;	            
		private Nullable<DateTime> _OccurenceDate;	            
		private Nullable<Double> _Amount;	            
		private String _Notes;	            
		private Guid _CreatedByUid;	            
		private DateTime _CreatedDate;	            
		private Guid _LastUpdatedByUid;	            
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
		public Nullable<DateTime> OccurenceDate
		{	
			get{ return _OccurenceDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_OccurenceDate, value, _OccurenceDate);
				if (PropertyChanging(args))
				{
					_OccurenceDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> Amount
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
		public String Notes
		{	
			get{ return _Notes; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Notes, value, _Notes);
				if (PropertyChanging(args))
				{
					_Notes = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid CreatedByUid
		{	
			get{ return _CreatedByUid; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_CreatedByUid, value, _CreatedByUid);
				if (PropertyChanging(args))
				{
					_CreatedByUid = value;
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
		public Guid LastUpdatedByUid
		{	
			get{ return _LastUpdatedByUid; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_LastUpdatedByUid, value, _LastUpdatedByUid);
				if (PropertyChanging(args))
				{
					_LastUpdatedByUid = value;
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
		public  EmployeeOccurencesBase Clone()
		{
			EmployeeOccurencesBase newObj = new  EmployeeOccurencesBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.UserId = this.UserId;						
			newObj.OccurenceDate = this.OccurenceDate;						
			newObj.Amount = this.Amount;						
			newObj.Notes = this.Notes;						
			newObj.CreatedByUid = this.CreatedByUid;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.LastUpdatedByUid = this.LastUpdatedByUid;						
			newObj.LastUpdatedDate = this.LastUpdatedDate;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(EmployeeOccurencesBase.Property_Id, Id);				
			info.AddValue(EmployeeOccurencesBase.Property_UserId, UserId);				
			info.AddValue(EmployeeOccurencesBase.Property_OccurenceDate, OccurenceDate);				
			info.AddValue(EmployeeOccurencesBase.Property_Amount, Amount);				
			info.AddValue(EmployeeOccurencesBase.Property_Notes, Notes);				
			info.AddValue(EmployeeOccurencesBase.Property_CreatedByUid, CreatedByUid);				
			info.AddValue(EmployeeOccurencesBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(EmployeeOccurencesBase.Property_LastUpdatedByUid, LastUpdatedByUid);				
			info.AddValue(EmployeeOccurencesBase.Property_LastUpdatedDate, LastUpdatedDate);				
		}
		#endregion

		
	}
}
