using System;
using System.Runtime.Serialization;
using System.ServiceModel;

using HS.Framework;

namespace HS.Entities.Bases
{
	[Serializable]
    [DataContract(Name = "TicketFileBase", Namespace = "http://www.piistech.com//entities")]
	public class TicketFileBase : BaseBusinessEntity
	{
	
		#region Enum Collection
		public enum Columns
		{
			Id = 0,
			TicketId = 1,
			FileName = 2,
			Filesize = 3,
			FileLocation = 4,
			Description = 5,
			FileAddedBy = 6,
			FileAddedDate = 7,
			TicketBookingDetailsId = 8
		}
		#endregion
	
		#region Constants
		public const string Property_Id = "Id";		            
		public const string Property_TicketId = "TicketId";		            
		public const string Property_FileName = "FileName";		            
		public const string Property_Filesize = "Filesize";		            
		public const string Property_FileLocation = "FileLocation";		            
		public const string Property_Description = "Description";		            
		public const string Property_FileAddedBy = "FileAddedBy";		            
		public const string Property_FileAddedDate = "FileAddedDate";		            
		public const string Property_TicketBookingDetailsId = "TicketBookingDetailsId";		            
		#endregion
		
		#region Private Data Types
		private Int32 _Id;	            
		private Guid _TicketId;	            
		private String _FileName;	            
		private Int32 _Filesize;	            
		private String _FileLocation;	            
		private String _Description;	            
		private Guid _FileAddedBy;	            
		private DateTime _FileAddedDate;	            
		private Nullable<Int32> _TicketBookingDetailsId;	            
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
		public Guid TicketId
		{	
			get{ return _TicketId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TicketId, value, _TicketId);
				if (PropertyChanging(args))
				{
					_TicketId = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public String FileName
		{	
			get{ return _FileName; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FileName, value, _FileName);
				if (PropertyChanging(args))
				{
					_FileName = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Int32 Filesize
		{	
			get{ return _Filesize; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_Filesize, value, _Filesize);
				if (PropertyChanging(args))
				{
					_Filesize = value;
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
		public Guid FileAddedBy
		{	
			get{ return _FileAddedBy; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FileAddedBy, value, _FileAddedBy);
				if (PropertyChanging(args))
				{
					_FileAddedBy = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public DateTime FileAddedDate
		{	
			get{ return _FileAddedDate; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_FileAddedDate, value, _FileAddedDate);
				if (PropertyChanging(args))
				{
					_FileAddedDate = value;
					PropertyChanged(args);					
				}	
			}
        }

		[DataMember]
		public Nullable<Int32> TicketBookingDetailsId
		{	
			get{ return _TicketBookingDetailsId; }			
			set
			{
				PropertyChangingEventArgs args = new PropertyChangingEventArgs(Property_TicketBookingDetailsId, value, _TicketBookingDetailsId);
				if (PropertyChanging(args))
				{
					_TicketBookingDetailsId = value;
					PropertyChanged(args);					
				}	
			}
        }

		#endregion
		
		#region Cloning Base Objects
		public  TicketFileBase Clone()
		{
			TicketFileBase newObj = new  TicketFileBase();
			base.CloneBase(newObj);
			newObj.Id = this.Id;						
			newObj.TicketId = this.TicketId;						
			newObj.FileName = this.FileName;						
			newObj.Filesize = this.Filesize;						
			newObj.FileLocation = this.FileLocation;						
			newObj.Description = this.Description;						
			newObj.FileAddedBy = this.FileAddedBy;						
			newObj.FileAddedDate = this.FileAddedDate;						
			newObj.TicketBookingDetailsId = this.TicketBookingDetailsId;						
			
			return newObj;
		}
		#endregion
		
		#region Getting object by adding value of that properties 
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(TicketFileBase.Property_Id, Id);				
			info.AddValue(TicketFileBase.Property_TicketId, TicketId);				
			info.AddValue(TicketFileBase.Property_FileName, FileName);				
			info.AddValue(TicketFileBase.Property_Filesize, Filesize);				
			info.AddValue(TicketFileBase.Property_FileLocation, FileLocation);				
			info.AddValue(TicketFileBase.Property_Description, Description);				
			info.AddValue(TicketFileBase.Property_FileAddedBy, FileAddedBy);				
			info.AddValue(TicketFileBase.Property_FileAddedDate, FileAddedDate);				
			info.AddValue(TicketFileBase.Property_TicketBookingDetailsId, TicketBookingDetailsId);				
		}
		#endregion

		
	}
}
