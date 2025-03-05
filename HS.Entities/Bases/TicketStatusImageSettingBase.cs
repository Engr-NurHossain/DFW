using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "TicketStatusImageSettingBase", Namespace = "http://www.hims-tech.com//entities")]
	public class TicketStatusImageSettingBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			CompanyId = 1,
			TicketStatus = 2,
			FileDescription = 3,
			Filename = 4,
			FileFullName = 5,
			Uploadeddate = 6,
			IsActive = 7,
			FileSize = 8,
			TicketStatusColor = 9
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_CompanyId = "CompanyId";		            
		public const string Property_TicketStatus = "TicketStatus";		            
		public const string Property_FileDescription = "FileDescription";		            
		public const string Property_Filename = "Filename";		            
		public const string Property_FileFullName = "FileFullName";		            
		public const string Property_Uploadeddate = "Uploadeddate";		            
		public const string Property_IsActive = "IsActive";		            
		public const string Property_FileSize = "FileSize";		            
		public const string Property_TicketStatusColor = "TicketStatusColor";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _CompanyId;	            
		private String _TicketStatus;	            
		private String _FileDescription;	            
		private String _Filename;	            
		private String _FileFullName;	            
		private Nullable<DateTime> _Uploadeddate;	            
		private Nullable<Boolean> _IsActive;	            
		private Nullable<Double> _FileSize;	            
		private String _TicketStatusColor;	            
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
		public String TicketStatus
		{	
			get{ return _TicketStatus; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TicketStatus, value, _TicketStatus);
				if (PropertyChanging(args))
				{
					_TicketStatus = value;
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
		public String FileFullName
		{	
			get{ return _FileFullName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FileFullName, value, _FileFullName);
				if (PropertyChanging(args))
				{
					_FileFullName = value;
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
		public Nullable<Boolean> IsActive
		{	
			get{ return _IsActive; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_IsActive, value, _IsActive);
				if (PropertyChanging(args))
				{
					_IsActive = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Double> FileSize
		{	
			get{ return _FileSize; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FileSize, value, _FileSize);
				if (PropertyChanging(args))
				{
					_FileSize = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String TicketStatusColor
		{	
			get{ return _TicketStatusColor; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TicketStatusColor, value, _TicketStatusColor);
				if (PropertyChanging(args))
				{
					_TicketStatusColor = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  TicketStatusImageSettingBase Clone()
		{
			TicketStatusImageSettingBase newObj = new  TicketStatusImageSettingBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.CompanyId = this.CompanyId;						
			newObj.TicketStatus = this.TicketStatus;						
			newObj.FileDescription = this.FileDescription;						
			newObj.Filename = this.Filename;						
			newObj.FileFullName = this.FileFullName;						
			newObj.Uploadeddate = this.Uploadeddate;						
			newObj.IsActive = this.IsActive;						
			newObj.FileSize = this.FileSize;						
			newObj.TicketStatusColor = this.TicketStatusColor;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(TicketStatusImageSettingBase.Property_Id, Id);				
			info.AddValue(TicketStatusImageSettingBase.Property_CompanyId, CompanyId);				
			info.AddValue(TicketStatusImageSettingBase.Property_TicketStatus, TicketStatus);				
			info.AddValue(TicketStatusImageSettingBase.Property_FileDescription, FileDescription);				
			info.AddValue(TicketStatusImageSettingBase.Property_Filename, Filename);				
			info.AddValue(TicketStatusImageSettingBase.Property_FileFullName, FileFullName);				
			info.AddValue(TicketStatusImageSettingBase.Property_Uploadeddate, Uploadeddate);				
			info.AddValue(TicketStatusImageSettingBase.Property_IsActive, IsActive);				
			info.AddValue(TicketStatusImageSettingBase.Property_FileSize, FileSize);				
			info.AddValue(TicketStatusImageSettingBase.Property_TicketStatusColor, TicketStatusColor);				
		}
		#endregion

		
	}
}
