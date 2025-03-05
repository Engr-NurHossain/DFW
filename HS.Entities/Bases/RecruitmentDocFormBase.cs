using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "RecruitmentDocFormBase", Namespace = "http://www.piistech.com//entities")]
	public class RecruitmentDocFormBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			FormId = 1,
			Name = 2,
			FileLocation = 3
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_FormId = "FormId";		            
		public const string Property_Name = "Name";		            
		public const string Property_FileLocation = "FileLocation";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _FormId;	            
		private String _Name;	            
		private String _FileLocation;	            
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
		public Guid FormId
		{	
			get{ return _FormId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FormId, value, _FormId);
				if (PropertyChanging(args))
				{
					_FormId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Name
		{	
			get{ return _Name; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Name, value, _Name);
				if (PropertyChanging(args))
				{
					_Name = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String FileLocation
		{	
			get{ return _FileLocation; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FileLocation, value, _FileLocation);
				if (PropertyChanging(args))
				{
					_FileLocation = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  RecruitmentDocFormBase Clone()
		{
			RecruitmentDocFormBase newObj = new  RecruitmentDocFormBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.FormId = this.FormId;						
			newObj.Name = this.Name;						
			newObj.FileLocation = this.FileLocation;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(RecruitmentDocFormBase.Property_Id, Id);				
			info.AddValue(RecruitmentDocFormBase.Property_FormId, FormId);				
			info.AddValue(RecruitmentDocFormBase.Property_Name, Name);				
			info.AddValue(RecruitmentDocFormBase.Property_FileLocation, FileLocation);				
		}
		#endregion

		
	}
}
