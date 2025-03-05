using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "BuildLogBase", Namespace = "http://www.piistech.com//entities")]
	public class BuildLogBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			Version = 1,
			BuildDate = 2,
			CreatedDate = 3,
			CreatedBy = 4
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_Version = "Version";		            
		public const string Property_BuildDate = "BuildDate";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _Version;	            
		private Nullable<DateTime> _BuildDate;	            
		private Nullable<DateTime> _CreatedDate;	            
		private Guid _CreatedBy;	            
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
		public String Version
		{	
			get{ return _Version; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Version, value, _Version);
				if (PropertyChanging(args))
				{
					_Version = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> BuildDate
		{	
			get{ return _BuildDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_BuildDate, value, _BuildDate);
				if (PropertyChanging(args))
				{
					_BuildDate = value;
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

		#endregion
		
		#region Cloning Base Objects
		public  BuildLogBase Clone()
		{
			BuildLogBase newObj = new  BuildLogBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.Version = this.Version;						
			newObj.BuildDate = this.BuildDate;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.CreatedBy = this.CreatedBy;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(BuildLogBase.Property_Id, Id);				
			info.AddValue(BuildLogBase.Property_Version, Version);				
			info.AddValue(BuildLogBase.Property_BuildDate, BuildDate);				
			info.AddValue(BuildLogBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(BuildLogBase.Property_CreatedBy, CreatedBy);				
		}
		#endregion

		
	}
}