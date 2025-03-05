using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "KnowledgebaseAccountabilityBase", Namespace = "http://www.hims-tech.com//entities")]
	public class KnowledgebaseAccountabilityBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			KnowledgebaseId = 1,
			AssignedUser = 2,
			AssignedDate = 3,
			EndDate = 4,
			IsRead = 5,
			AssignedBy = 6
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_KnowledgebaseId = "KnowledgebaseId";		            
		public const string Property_AssignedUser = "AssignedUser";		            
		public const string Property_AssignedDate = "AssignedDate";		            
		public const string Property_EndDate = "EndDate";		            
		public const string Property_IsRead = "IsRead";		            
		public const string Property_AssignedBy = "AssignedBy";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Int32 _KnowledgebaseId;	            
		private Guid _AssignedUser;	            
		private DateTime _AssignedDate;	            
		private Nullable<DateTime> _EndDate;	            
		private Boolean _IsRead;	            
		private Guid _AssignedBy;	            
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
		public Int32 KnowledgebaseId
		{	
			get{ return _KnowledgebaseId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_KnowledgebaseId, value, _KnowledgebaseId);
				if (PropertyChanging(args))
				{
					_KnowledgebaseId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid AssignedUser
		{	
			get{ return _AssignedUser; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AssignedUser, value, _AssignedUser);
				if (PropertyChanging(args))
				{
					_AssignedUser = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime AssignedDate
		{	
			get{ return _AssignedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AssignedDate, value, _AssignedDate);
				if (PropertyChanging(args))
				{
					_AssignedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> EndDate
		{	
			get{ return _EndDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_EndDate, value, _EndDate);
				if (PropertyChanging(args))
				{
					_EndDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Boolean IsRead
		{	
			get{ return _IsRead; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsRead, value, _IsRead);
				if (PropertyChanging(args))
				{
					_IsRead = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Guid AssignedBy
		{	
			get{ return _AssignedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_AssignedBy, value, _AssignedBy);
				if (PropertyChanging(args))
				{
					_AssignedBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  KnowledgebaseAccountabilityBase Clone()
		{
			KnowledgebaseAccountabilityBase newObj = new  KnowledgebaseAccountabilityBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.KnowledgebaseId = this.KnowledgebaseId;						
			newObj.AssignedUser = this.AssignedUser;						
			newObj.AssignedDate = this.AssignedDate;						
			newObj.EndDate = this.EndDate;						
			newObj.IsRead = this.IsRead;						
			newObj.AssignedBy = this.AssignedBy;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(KnowledgebaseAccountabilityBase.Property_Id, Id);				
			info.AddValue(KnowledgebaseAccountabilityBase.Property_KnowledgebaseId, KnowledgebaseId);				
			info.AddValue(KnowledgebaseAccountabilityBase.Property_AssignedUser, AssignedUser);				
			info.AddValue(KnowledgebaseAccountabilityBase.Property_AssignedDate, AssignedDate);				
			info.AddValue(KnowledgebaseAccountabilityBase.Property_EndDate, EndDate);				
			info.AddValue(KnowledgebaseAccountabilityBase.Property_IsRead, IsRead);				
			info.AddValue(KnowledgebaseAccountabilityBase.Property_AssignedBy, AssignedBy);				
		}
		#endregion

		
	}
}