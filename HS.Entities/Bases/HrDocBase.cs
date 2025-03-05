using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "HrDocBase", Namespace = "http://www.piistech.com//entities")]
	public class HrDocBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			FileDescription = 1,
			Filename = 2,
			Uploadeddate = 3,
			UserName = 4,
			CompanyId = 5,
			CreatedBy = 6,
			CreatedDate = 7,
			Category = 8
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_FileDescription = "FileDescription";		            
		public const string Property_Filename = "Filename";		            
		public const string Property_Uploadeddate = "Uploadeddate";		            
		public const string Property_UserName = "UserName";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_CreatedBy = "CreatedBy";		            
		public const string Property_CreatedDate = "CreatedDate";		            
		public const string Property_Category = "Category";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private String _FileDescription;	            
		private String _Filename;	            
		private Nullable<DateTime> _Uploadeddate;	            
		private String _UserName;	            
		private Guid _CompanyId;	            
		private Guid _CreatedBy;	            
		private DateTime _CreatedDate;	            
		private String _Category;	            
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
		public String FileDescription
		{	
			get{ return _FileDescription; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FileDescription, value, _FileDescription);
				if (PropertyChanging(args))
				{
					_FileDescription = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String Filename
		{	
			get{ return _Filename; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Filename, value, _Filename);
				if (PropertyChanging(args))
				{
					_Filename = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<DateTime> Uploadeddate
		{	
			get{ return _Uploadeddate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Uploadeddate, value, _Uploadeddate);
				if (PropertyChanging(args))
				{
					_Uploadeddate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String UserName
		{	
			get{ return _UserName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_UserName, value, _UserName);
				if (PropertyChanging(args))
				{
					_UserName = value;
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
		public String Category
		{	
			get{ return _Category; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Category, value, _Category);
				if (PropertyChanging(args))
				{
					_Category = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  HrDocBase Clone()
		{
			HrDocBase newObj = new  HrDocBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.FileDescription = this.FileDescription;						
			newObj.Filename = this.Filename;						
			newObj.Uploadeddate = this.Uploadeddate;						
			newObj.UserName = this.UserName;						
			newObj.CompanyId = this.CompanyId;						
			newObj.CreatedBy = this.CreatedBy;						
			newObj.CreatedDate = this.CreatedDate;						
			newObj.Category = this.Category;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(HrDocBase.Property_Id, Id);				
			info.AddValue(HrDocBase.Property_FileDescription, FileDescription);				
			info.AddValue(HrDocBase.Property_Filename, Filename);				
			info.AddValue(HrDocBase.Property_Uploadeddate, Uploadeddate);				
			info.AddValue(HrDocBase.Property_UserName, UserName);				
			info.AddValue(HrDocBase.Property_CompanyId, CompanyId);				
			info.AddValue(HrDocBase.Property_CreatedBy, CreatedBy);				
			info.AddValue(HrDocBase.Property_CreatedDate, CreatedDate);				
			info.AddValue(HrDocBase.Property_Category, Category);				
		}
		#endregion

		
	}
}
